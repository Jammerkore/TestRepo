using System;
using System.Collections;
using System.IO;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using System.Diagnostics;

namespace MIDRetail.Business
{
	/// <summary>
	/// The ChangeMethodProfile class defines a change rule.
	/// </summary>
	/// <remarks>
	/// This class identifies the information needed to define a change rule.  Information includes an Id and a name.
	/// </remarks>

	abstract public class ChangeMethodProfile : Profile
	{
		//=======
		// FIELDS
		//=======

		private string _name;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ChangeMethodProfile using the given Id and Name.
		/// </summary>
		/// <param name="aKey">
		/// The Id of this profile.
		/// </param>
		/// <param name="aName">
		/// The name for this change rule.
		/// </param>

		public ChangeMethodProfile(int aKey, string aName)
			: base(aKey)
		{
			_name = aName;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.ChangeMethod;
			}
		}

		/// <summary>
		/// Gets the name.
		/// </summary>

		public string Name
		{
			get
			{
				return _name;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// This method is called to execute the change rules.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The aComputationCellRef for the change.
		/// </param>

		public void ExecuteChangeMethod(ComputationSchedule aCompSchd, ComputationCellReference aCompCellRef, string aCaller)
		{
			try
			{
				// FUTURE TODO:  Add logic here to dump out debug statements for Computation Tracing
				Execute(aCompSchd, aCompCellRef);
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// This abstract method is called to execute the change rules.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The aComputationCellRef for the change.
		/// </param>

		abstract public void Execute(ComputationSchedule aCompSchd, ComputationCellReference aCompCellRef);
	}

	/// <summary>
	/// The FormulaSpreadProfile class defines a formula or a spread.
	/// </summary>
	/// <remarks>
	/// This class identifies the information needed to define a formula or a spread.  Information includes an Id and a name.
	/// </remarks>

	abstract public class FormulaSpreadProfile : Profile
	{
		//=======
		// FIELDS
		//=======

		private string _name;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of FormulaSpreadProfile using the given Id and Name.
		/// </summary>
		/// <param name="aKey">
		/// The Id of this profile.
		/// </param>
		/// <param name="aName">
		/// The name for this formula or spread.
		/// </param>

		public FormulaSpreadProfile(int aKey, string aName)
			: base(aKey)
		{
			_name = aName;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the name.
		/// </summary>

		public string Name
		{
			get
			{
				return _name;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// This abstract method is called to execute the forumula or spread.
		/// </summary>
		/// <param name="aCompSchdEntry">
		/// The schedule entry that is being executed.
		/// </param>
		/// <param name="aGetCellMode">
		/// The eGetCellMode for the calculation.
		/// </param>
		/// <param name="aSetCellMode">
		/// The eSetCellMode for the calculation.
		/// </param>

		abstract public eComputationFormulaReturnType ExecuteCalc(ComputationScheduleEntry aCompSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, string aCaller);
        //Begin TT#2 - JScott - Assortment Planning - Phase 2

        ///// <summary>
        ///// Gets a boolean indicating whether the spread-from cell should be reinited after execution of the spread.
        ///// </summary>

        //virtual public bool PostExecuteReInit
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}
        //End TT#2 - JScott - Assortment Planning - Phase 2
    }

	/// <summary>
	/// The FormulaProfile class defines a formula.
	/// </summary>
	/// <remarks>
	/// This class identifies the information needed to define a formula.  Information includes an Id and a name.
	/// </remarks>

	abstract public class FormulaProfile : FormulaSpreadProfile
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of FormulaProfile using the given Id and Name.
		/// </summary>
		/// <param name="aKey">
		/// The Id of this profile.
		/// </param>
		/// <param name="aName">
		/// The name for this formula or spread.
		/// </param>

		public FormulaProfile(int aKey, string aName)
			: base(aKey, aName)
		{
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Formula;
			}
		}

		//========
		// METHODS
		//========

		//Begin Track #5752 - JScott - Calculation Time
		///// <summary>
		///// This method is called to execute the forumula or spread.
		///// </summary>
		///// <param name="aCompSchdEntry">
		///// The schedule entry that is being executed.
		///// </param>
		///// <param name="aGetCellMode">
		///// The eGetCellMode for the calculation.
		///// </param>
		///// <param name="aSetCellMode">
		///// The eSetCellMode for the calculation.
		///// </param>

		//override public eComputationFormulaReturnType ExecuteCalc(ComputationScheduleEntry aCompSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, string aCaller)
		//{
		//    ComputationScheduleFormulaEntry schedEntry;

		//    try
		//    {
		//        // FUTURE TODO:  Add logic here to dump out debug statements for Computation Tracing
		//        return Execute(aCompSchdEntry, aGetCellMode, aSetCellMode);
		//    }
		//    catch (CellPendingException exc)
		//    {
		//        schedEntry = (ComputationScheduleFormulaEntry)aCompSchdEntry;
		//        schedEntry.LastPendingCell = exc.ComputationCellReference;
		//        throw;
		//    }
		//    catch (Exception)
		//    {
		//        throw;
		//    }
		//}
		//End Track #5752 - JScott - Calculation Time
	}

	/// <summary>
	/// The SpreadProfile class defines a spread.
	/// </summary>
	/// <remarks>
	/// This class identifies the information needed to define a spread.  Information includes an Id and a name.
	/// </remarks>

	abstract public class SpreadProfile : FormulaSpreadProfile
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of SpreadProfile using the given Id and Name.
		/// </summary>
		/// <param name="aKey">
		/// The Id of this profile.
		/// </param>
		/// <param name="aName">
		/// The name for this formula or spread.
		/// </param>

		public SpreadProfile(int aKey, string aName)
			: base(aKey, aName)
		{
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets a boolean indicating whether the spread-to cell should be marked as user-changed.
		/// </summary>

		abstract public bool CascadeChangeMethods { get; }

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Spread;
			}
		}

		//========
		// METHODS
		//========

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        ///// <summary>
        ///// Gets a boolean indicating whether the spread-to cell should be marked as user-changed.
        ///// </summary>

        //virtual public bool GetCascadeChangeMethods(ComputationCellReference aCompCellRef)
        //{
        //    try
        //    {
        //        return CascadeChangeMethods;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //End TT#2 - JScott - Assortment Planning - Phase 2
		abstract public ArrayList GetSpreadToCellReferenceList(ComputationCellReference aCompCellRef);

		//Begin Track #5752 - JScott - Calculation Time
		///// <summary>
		///// This virtual method is called to execute the forumula or spread.
		///// </summary>
		///// <param name="aCompSchdEntry">
		///// The schedule entry that is being executed.
		///// </param>
		///// <param name="aGetCellMode">
		///// The eGetCellMode for the calculation.
		///// </param>
		///// <param name="aSetCellMode">
		///// The eSetCellMode for the calculation.
		///// </param>

		//override public eComputationFormulaReturnType ExecuteCalc(ComputationScheduleEntry aCompSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, string aCaller)
		//{
		//    ComputationScheduleSpreadEntry schedEntry;

		//    try
		//    {
		//        // FUTURE TODO:  Add logic here to dump out debug statements for Computation Tracing
		//        return Execute(aCompSchdEntry, aGetCellMode, aSetCellMode);
		//    }
		//    catch (CellPendingException exc)
		//    {
		//        schedEntry = (ComputationScheduleSpreadEntry)aCompSchdEntry;
		//        schedEntry.LastPendingCell = exc.ComputationCellReference;
		//        throw;
		//    }
		//    catch (Exception)
		//    {
		//        throw;
		//    }
		//}
		//End Track #5752 - JScott - Calculation Time
	}

	/// <summary>
	/// ComputationScheduleEntry is an abstract class that stores the CellReference and Formula or Spread for a scheduled
	/// calculation.
	/// </summary>
	/// <remarks>
	/// This is the base class for a computation schedule entry, and contains the information required to execute a formula or
	/// spread entry.  Both formula and spread entries inherit off of this base class.
	/// </remarks>

	abstract public class ComputationScheduleEntry
	{
		//=======
		// FIELDS
		//=======

		protected ComputationCellReference _compCellRef;
		protected eComputationScheduleEntryType _scheduleEntryType;
		protected FormulaSpreadProfile _formulaSpreadProfile;
		protected int _schedulePriority;
		protected int _cubePriority;
		protected ComputationCellReference _lastPendingCell = null;   // RO-4741 - JSmith - Need to scroll to variables prior to making change
        protected eComputationFormulaReturnType _computationFormulaReturnType;   // TT#1954-MD - JSmith - Assortment Performance

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ComputationScheduleEntry for a given FormulaSpreadProfile and ComputationCellReference.
		/// </summary>
		/// <param name="aCompCellRef">
		/// A reference to a ComputationCellReference that indicates which cell is to be calculated.
		/// </param>
		/// <param name="aFormulaSpreadProfile">
		/// A reference to a FormulaSpreadProfile that indicates which formula or spread to execute.
		/// </param>

		public ComputationScheduleEntry(
			ComputationCellReference aCompCellRef,
			FormulaSpreadProfile aFormulaSpreadProfile,
			int aSchedulePriority,
			int aCubePriority)
		{
			_compCellRef = aCompCellRef;
			_formulaSpreadProfile = aFormulaSpreadProfile;
			_schedulePriority = aSchedulePriority;
			_cubePriority = aCubePriority;
            _computationFormulaReturnType = eComputationFormulaReturnType.Successful;   // TT#1954-MD - JSmith - Assortment Performance
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the reference to the ComputationCellReference.
		/// </summary>

		public ComputationCellReference ComputationCellRef
		{
			get
			{
				return _compCellRef;
			}
		}

		/// <summary>
		/// Gets the schedule entry type, which indicates a formula or spread.
		/// </summary>

		public eComputationScheduleEntryType ScheduleEntryType
		{
			get
			{
				return _scheduleEntryType;
			}
		}

		/// <summary>
		/// Gets the reference to the FormulaSpreadProfile.
		/// </summary>

		public FormulaSpreadProfile FormulaSpreadProfile
		{
			get
			{
				return _formulaSpreadProfile;
			}
			set
			{
				_formulaSpreadProfile = value;
			}
		}

		/// <summary>
		/// Gets the Schedule priority of the formula.
		/// </summary>

		public int SchedulePriority
		{
			get
			{
				return _schedulePriority;
			}
			set
			{
				_schedulePriority = value;
			}
		}

		/// <summary>
		/// Gets the Cube priority of the formula.
		/// </summary>

		public int CubePriority
		{
			get
			{
				return _cubePriority;
			}
			set
			{
				_cubePriority = value;
			}
		}

		/// <summary>
		/// Gets or sets the last cell that was pending during the execution of the calculation.
		/// </summary>

		public ComputationCellReference LastPendingCell
		{
			get
			{
				return _lastPendingCell;
			}
			set
			{
                // Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
				//_lastPendingCell = value;
                if (_lastPendingCell == null)
                {
                    _lastPendingCell = value;
                }
				// End RO-4741 - JSmith - Need to scroll to variables prior to making change
			}
		}

        // Begin TT#1954-MD - JSmith - Assortment Performance
        public eComputationFormulaReturnType ComputationFormulaReturnType
        {
            get
            {
                return _computationFormulaReturnType;
            }
            set
            {
                _computationFormulaReturnType = value;
            }
        }
		// End TT#1954-MD - JSmith - Assortment Performance

        //===========
        // METHODS
        //===========
		// Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
        public void ClearLastPendingCell()
        {
            _lastPendingCell = null;
        }
		// End RO-4741 - JSmith - Need to scroll to variables prior to making change
    }

	/// <summary>
	/// ComputationScheduleFormulaEntry is a class that stores the CellReference and Formula for a scheduled
	/// calculation.  Inherits from ComputationScheduleEntry.
	/// </summary>
	/// <remarks>
	/// This class contains the information required to execute a formula.  No additional fields beyond those supplied
	/// in the base class are required.
	/// </remarks>

	abstract public class ComputationScheduleFormulaEntry : ComputationScheduleEntry
	{
		//=======
		// FIELDS
		//=======

		private ComputationCellReference _changedCompCellRef;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		///  Creates a new instance of ComputationScheduleFormulaEntry for a given formula and ComputationCellReference.
		/// </summary>
		/// <param name="aCompCellRef">
		/// A reference to a ComputationCellReference that indicates which cell is to be calculated.
		/// </param>
		/// <param name="aFormulaProfile">
		/// A reference to a FormulaSpreadProfile that indicates which formula to execute.
		/// </param>

		public ComputationScheduleFormulaEntry(
			ComputationCellReference aChangedCompCellRef,
			ComputationCellReference aCompCellRef,
			FormulaProfile aFormulaProfile,
			eComputationScheduleEntryType aScheduleEntryType,
			int aSchedulePriority,
			int aCubePriority)
			: base(aCompCellRef, aFormulaProfile, aSchedulePriority, aCubePriority)
		{
			_changedCompCellRef = aChangedCompCellRef;
			_scheduleEntryType = aScheduleEntryType;
		}

		//===========
		// PROPERTIES
		//===========

		public ComputationCellReference ChangedCompCellRef
		{
			get
			{
				return _changedCompCellRef;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Returns a hash code for the given object, using the hashcode from the ComputationCellReference and schedule entry type.
		/// </summary>
		/// <returns>
		/// A hash code for the given object, using the hashcode from the ComputationCellReference and schedule entry type.
		/// </returns>

		public override int GetHashCode()
		{
			try
			{
				return _compCellRef.GetHashCode() + (int)_scheduleEntryType;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a value indicating whether an instance of ComputationScheduleFormulaEntry is equal to a specified object.
		/// </summary>
		/// <param name="aObject">
		/// An object to compare with this instance.
		/// </param>
		/// <returns>
		/// <I>true</I> if value is an instance of ComputationScheduleFormulaEntry and equals the value of this instance; otherwise,
		/// <I>false</I>.
		/// </returns>

		public override bool Equals(object aObject)
		{
			try
			{
				return (_compCellRef.Equals(((ComputationScheduleEntry)aObject).ComputationCellRef) &&
					_scheduleEntryType == ((ComputationScheduleEntry)aObject).ScheduleEntryType);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// ComputationScheduleSpreadEntry is a class that stores the CellReference and Spread for a scheduled
	/// spread.  Inherits from ComputationScheduleEntry.
	/// </summary>
	/// <remarks>
	/// This class contains the information required to execute a spread.  In addition to the functionality of the base
	/// class, ComputationScheduleSpreadEntry contains an additiona ArrayList that contains the SpreadCellReference object
	/// that point to each cell involved in the spread.
	/// </remarks>

	abstract public class ComputationScheduleSpreadEntry : ComputationScheduleEntry
	{
		//=======
		// FIELDS
		//=======

		private ArrayList _spreadCellRefList;
		private Hashtable _excludedCellRefHash;
		private Hashtable _includedCellRefHash;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ComputationScheduleSpreadEntry, using the given ComputationCellReference and FormulaSpreadProfile.
		/// </summary>
		/// <param name="aCompCellRef">
		/// A reference to a ComputationCellReference object that locates the cell being spread from.
		/// </param>
		/// <param name="aSpreadProfile">
		/// A reference to the FormulaSpreadProfile that is being scheduled for this cell.
		/// </param>

		public ComputationScheduleSpreadEntry(
			ComputationCellReference aCompCellRef,
			SpreadProfile aSpreadProfile,
			int aSchedulePriority,
			int aCubePriority)
			: base(aCompCellRef, aSpreadProfile, aSchedulePriority, aCubePriority)
		{
			_scheduleEntryType = eComputationScheduleEntryType.Spread;
			_excludedCellRefHash = new Hashtable();
			_includedCellRefHash = new Hashtable();
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the ArrayList of SpreadCellReferences that indicates the cells to spread to.
		/// </summary>

		public ArrayList SpreadCellRefList
		{
			get
			{
				return _spreadCellRefList;
			}
			set
			{
				_spreadCellRefList = value;
			}
		}

		/// <summary>
		/// Gets or sets the ArrayList of SpreadCellReferences that indicates the cells to spread to.
		/// </summary>

		public Hashtable ExcludedCellRefHash
		{
			get
			{
				return _excludedCellRefHash;
			}
		}

		/// <summary>
		/// Gets or sets the ArrayList of SpreadCellReferences that indicates the cells to spread to.
		/// </summary>

		public Hashtable IncludedCellRefHash
		{
			get
			{
				return _includedCellRefHash;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Returns a hash code for the given object, using the hashcode from the ComputationCellReference, schedule entry type, and
		/// FormulaSpreadProfile.
		/// </summary>
		/// <returns>
		/// A hash code for the given object, using the hashcode from the ComputationCellReference, schedule entry type, and
		/// FormulaSpreadProfile.
		/// </returns>

		public override int GetHashCode()
		{
			try
			{
				return _compCellRef.GetHashCode() + (int)_scheduleEntryType + _formulaSpreadProfile.Key;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a value indicating whether an instance of ComputationScheduleSpreadEntry is equal to a specified object.
		/// </summary>
		/// <param name="aObject">
		/// An object to compare with this instance.
		/// </param>
		/// <returns>
		/// <I>true</I> if value is an instance of ComputationScheduleSpreadEntry and equals the value of this instance; otherwise,
		/// <I>false</I>.
		/// </returns>

		public override bool Equals(object aObject)
		{
			try
			{
				return (_compCellRef.Equals(((ComputationScheduleEntry)aObject).ComputationCellRef) &&
					_scheduleEntryType == ((ComputationScheduleEntry)aObject).ScheduleEntryType &&
					_formulaSpreadProfile.Key == ((ComputationScheduleEntry)aObject).FormulaSpreadProfile.Key);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Checks to see if the spread-from or any spread-to cell is pending.  Also builds the included and excluded cell lists.
		/// </summary>

        public void CheckSpreadCellsForPending(out bool isPendingException) //TT#1659-MD -jsobek -CelllPendingException Performance
		{
            isPendingException = false; //TT#1659-MD -jsobek -CelllPendingException Performance
			try
			{
				//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
				//if (ComputationCellRef.isCellFormulaPending(this) || ComputationCellRef.isCellSpreadPending(this))
				if (ComputationCellRef.isCellFormulaPending(this, true) || ComputationCellRef.isCellSpreadPending(this, true))
				//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
				{
                    //Begin TT#1659-MD -jsobek -CelllPendingException Performance
                    //throw new CellPendingException(ComputationCellRef);
                    isPendingException = true;
                    _lastPendingCell = ComputationCellRef;
                    return;
                    //End TT#1659-MD -jsobek -CelllPendingException Performance
				}

				foreach (ComputationCellReference compCellRef in _spreadCellRefList)
				{
					if (!_excludedCellRefHash.Contains(compCellRef) && !_includedCellRefHash.Contains(compCellRef))
					{
						//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
						//if (compCellRef.isCellFormulaPending(this) || compCellRef.isCellSpreadPending(this))
						if (compCellRef.isCellFormulaPending(this, true) || compCellRef.isCellSpreadPending(this, true))
						//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
						{
                            //Begin TT#1659-MD -jsobek -CelllPendingException Performance
                            //throw new CellPendingException(ComputationCellRef);
                            isPendingException = true;
                            //Begin TT#1659-MD -RMatelic - CelllPendingException Performance >>> correct next line
                            //_lastPendingCell = ComputationCellRef;
							// Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
                            //_lastPendingCell = compCellRef;
                            LastPendingCell = compCellRef;
							// End RO-4741 - JSmith - Need to scroll to variables prior to making change
                            //End TT#1659-MD 
                            return;
                            //End TT#1659-MD -jsobek -CelllPendingException Performance
						}
						else
						{
							if (!compCellRef.isCellReadOnly &&
								!compCellRef.isCellProtected &&
								!compCellRef.isCellClosed &&
								!compCellRef.isCellIneligible &&
								!compCellRef.isCellCompChanged &&
								!compCellRef.isCellLocked)
							{
								if (isAvailableToSpreadTo(ComputationCellRef, compCellRef))
								{
									_includedCellRefHash.Add(compCellRef, compCellRef);
								}
								else
								{
									_excludedCellRefHash.Add(compCellRef, compCellRef);
								}
							}
							else
							{
								_excludedCellRefHash.Add(compCellRef, compCellRef);
							}
						}
					}
				}
			}
			// Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
			//catch (CellPendingException exc)
            //{
            //    _lastPendingCell = exc.ComputationCellReference;
            //    throw;
            //}
			catch (CellPendingException exc)
			{
                //_lastPendingCell = exc.ComputationCellReference;
                LastPendingCell = exc.ComputationCellReference;
                throw;
			}
			// End RO-4741 - JSmith - Need to scroll to variables prior to making change
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines if a given cell is available to spread to.
		/// </summary>
		/// <param name="aSpreadFromCellRef">
		/// The cell being spread from.
		/// </param>
		/// <param name="aCompCellRef">
		/// The cell being spread to.
		/// </param>
		/// <returns>
		/// A boolean indicating if a given cell is available to spread to.
		/// </returns>

		private bool isAvailableToSpreadTo(ComputationCellReference aSpreadFromCellRef, ComputationCellReference aCompCellRef)
		{
			ArrayList cellRefList;
			bool detAvail;

			try
			{
				if (!aCompCellRef.isCellLocked && !aCompCellRef.isCellCompLocked && !aCompCellRef.isCellCompChanged)
				{
					cellRefList = aCompCellRef.GetSpreadDetailCellRefArray(aCompCellRef.isCellHidden);

					if (cellRefList.Count > 0)
					{
						detAvail = false;

						foreach (ComputationCellReference compCellRef in cellRefList)
						{
							if (isAvailableToSpreadTo(aCompCellRef, compCellRef))
							{
								detAvail = true;
								break;
							}
						}

						if (detAvail)
						{
							cellRefList = aCompCellRef.GetTotalCellRefArray();

							foreach (ComputationCellReference compCellRef in cellRefList)
							{
								if (!compCellRef.Equals(aSpreadFromCellRef) && (compCellRef.isCellLocked || compCellRef.isCellCompLocked || compCellRef.isCellCompChanged))
								{
									detAvail = false;
									break;
								}
							}
						}

						return detAvail;
					}
					else
					{
						return true;
					}
				}
				else
				{
					return false;
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
	/// ComputationSchedule is a class that contains the list of schedule entries that are to be executed during the
	/// recalculation process.
	/// </summary>
	/// <remarks>
	/// The ComputationSchedule is a class that handles all aspects of the scheduling and execution process for a recalculation.
	/// Functionality includes the ability to insert formulas and spread into the schedule, and the ability to execute the
	/// schedule.
	/// </remarks>

	public class ComputationSchedule
	{
		//=======
		// FIELDS
		//=======

		private ComputationCubeGroup _compCubeGroup;
		private ArrayList _scheduleList;
        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        private ArrayList _autoTotalList;
        //End TT#2 - JScott - Assortment Planning - Phase 2
        // Begin TT#1954-MD - JSmith - Assortment
        PerfRecalcHash _formulaHash;
        bool _showBenchmarking = false;
        // End TT#1954-MD - JSmith - Assortment

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ComputationSchedule, using the given ComputationCubeGroup.
		/// </summary>
		/// <param name="aCompCubeGroup">
		/// The ComputationCubeGroup that is creating this object.
		/// </param>

		public ComputationSchedule(ComputationCubeGroup aCompCubeGroup)
		{
			try
			{
				_compCubeGroup = aCompCubeGroup;
				_scheduleList = new ArrayList();
                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                _autoTotalList = new ArrayList();
                //End TT#2 - JScott - Assortment Planning - Phase 2

                // Begin TT#1954-MD - JSmith - Assortment
                string parmStr = MIDConfigurationManager.AppSettings["MIDOnlyFunctions"];
                if (parmStr != null)
                {
                    parmStr = parmStr.ToLower();
                    if (parmStr == "true" || parmStr == "yes" || parmStr == "t" || parmStr == "y" || parmStr == "1")
                    {
                        parmStr = MIDConfigurationManager.AppSettings["ShowCalcTimings"];
                        if (parmStr != null)
                        {
                            parmStr = parmStr.ToLower();
                            if (parmStr == "true" || parmStr == "yes" || parmStr == "t" || parmStr == "y" || parmStr == "1")
                            {
                                _showBenchmarking = true;
                            }
                        }
                    }
                }
                // End TT#1954-MD - JSmith - Assortment
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

		/// <summary>
		/// Gets the CubeGroup object.
		/// </summary>

		public ComputationCubeGroup ComputationCubeGroup
		{
			get
			{
				return _compCubeGroup;
			}
		}

		public ArrayList ScheduleList
		{
			get
			{
				return _scheduleList;
			}
		}

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        public ArrayList AutoTotalList
        {
            get
            {
                return _autoTotalList;
            }
        }

        //End TT#2 - JScott - Assortment Planning - Phase 2
        //========
		// METHODS
		//========

		/// <summary>
		/// Clears the schedule queue of this ComputationSchedule instance.
		/// </summary>

		public void Clear()
		{
			try
			{
				_scheduleList.Clear();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts an the given Cell's Init formula into the schedule queue for the given changed ComputationCellReference and target ComputationCellReference.
		/// </summary>
		/// <param name="aChangedCompCellRef">
		/// The ComputationCellReference that identifies the cell that has triggered this computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the cell being computed.
		/// </param>

		public void InsertInitFormula(
			ComputationCellReference aChangedCompCellRef,
			ComputationCellReference aCompCellRef)
		{
			FormulaProfile initFormula;

			try
			{
				initFormula = aCompCellRef.GetInitFormulaProfile();

				if (initFormula != null)
				{
					//Begin Track #5752 - JScott - Calculation Time
					//intInsertFormula(aChangedCompCellRef, aCompCellRef, initFormula, eComputationScheduleEntryType.Formula);
					intInsertFormula(aChangedCompCellRef, aCompCellRef, initFormula, eComputationScheduleEntryType.Formula, 0);
					//End Track #5752 - JScott - Calculation Time
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts an the given Cell's Init formula as an AutoTotal into the schedule queue for the given changed ComputationCellReference and target ComputationCellReference.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the cell being computed.
		/// </param>

		public void ScheduleAutoTotals(
			ComputationCellReference aCompCellRef)
		{
			try
			{
				//Begin Track #5809 - JScott - Circular Reference when changing Low-Level-Totals in Multi-Chain
				//intScheduleTotals(aCompCellRef);
				intScheduleTotals(aCompCellRef, null);
				//End Track #5809 - JScott - Circular Reference when changing Low-Level-Totals in Multi-Chain
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts an the given Cell's Init formula as an AutoTotal into the schedule queue for the given changed ComputationCellReference and target ComputationCellReference.
		/// </summary>
		/// <param name="aChangedCompCellRef">
		/// The ComputationCellReference that identifies the cell that has triggered this computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the cell being computed.
		/// </param>

		public void InsertAutoTotalFormula(
			ComputationCellReference aChangedCompCellRef,
			ComputationCellReference aCompCellRef)
		{
			FormulaProfile autoTotalFormula;

			try
			{
				autoTotalFormula = aCompCellRef.GetInitFormulaProfile();

				if (autoTotalFormula != null)
				{
					//Begin Track #5752 - JScott - Calculation Time
					//intInsertFormula(aChangedCompCellRef, aCompCellRef, autoTotalFormula, eComputationScheduleEntryType.AutoTotal);
					intInsertFormula(aChangedCompCellRef, aCompCellRef, autoTotalFormula, eComputationScheduleEntryType.AutoTotal, 1);
					//End Track #5752 - JScott - Calculation Time
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        ///// <summary>
        ///// Inserts an the given Cell's Init formula as an PostExecuteReInit into the schedule queue for the given changed ComputationCellReference and target ComputationCellReference.
        ///// </summary>
        ///// <param name="aChangedCompCellRef">
        ///// The ComputationCellReference that identifies the cell that has triggered this computation.
        ///// </param>
		
        //public void InsertPostExecuteReInitFormula(
        //    ComputationScheduleEntry aScheduleEntry,
        //    ComputationCellReference aChangedCompCellRef)
        //{
        //    FormulaProfile initFormula;
        //    ComputationScheduleFormulaEntry compEntry;

        //    try
        //    {
        //        if (aChangedCompCellRef.CellPostExecuteReInitScheduleEntry == null)
        //        {
        //            initFormula = aChangedCompCellRef.GetInitFormulaProfile();

        //            if (initFormula != null)
        //            {
        //                aChangedCompCellRef.CellPostExecuteReInitTriggerScheduleEntry = aScheduleEntry;
        //                compEntry = aChangedCompCellRef.CreateScheduleFormulaEntry(aChangedCompCellRef, initFormula, eComputationScheduleEntryType.ForcedReInit, aChangedCompCellRef.ComputationCube.CubePriority, aChangedCompCellRef.ComputationCube.CubePriority);
        //                aChangedCompCellRef.CellPostExecuteReInitScheduleEntry = compEntry;
        //                _scheduleList.Add(compEntry);
        //            }
        //        }
        //        else
        //        {
        //            throw new FormulaConflictException();
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        string message = exc.ToString();
        //        throw;
        //    }
        //}

        //End TT#2 - JScott - Assortment Planning - Phase 2
        /// <summary>
		/// Inserts a formula into the schedule queue for the given changed ComputationCellReference, target ComputationCellReference, and FormulaSpreadProfile.
		/// </summary>
		/// <param name="aChangedCompCellRef">
		/// The ComputationCellReference that identifies the cell that has triggered this computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the cell being computed.
		/// </param>
		/// <param name="aFormula">
		/// The FormulaSpreadProfile of the forumla to execute.
		/// </param>

		public void InsertFormula(
			ComputationCellReference aChangedCompCellRef,
			ComputationCellReference aCompCellRef,
			FormulaProfile aFormula)
		{
			try
			{
				//Begin Track #5752 - JScott - Calculation Time
				//intInsertFormula(aChangedCompCellRef, aCompCellRef, aFormula, eComputationScheduleEntryType.Formula);
				intInsertFormula(aChangedCompCellRef, aCompCellRef, aFormula, eComputationScheduleEntryType.Formula, 0);
				//End Track #5752 - JScott - Calculation Time
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
		/// <param name="aSpreadFromCellRef">
		/// The ComputationCellReference that identifies the cell being spread from.
		/// </param>
		/// <param name="aSpreadToCellRefs">
		/// An ArrayList of ComputationCellReferences that identify the cells being spread to.
		/// </param>
		/// <param name="aSpread">
		/// The FormulaSpreadProfile of the spread to execute.
		/// </param>

		public void InsertSpread(
			ComputationCellReference aSpreadFromCellRef,
			ArrayList aSpreadToCellRefs,
			SpreadProfile aSpread)
		{
			try
			{
				InsertSpread(aSpreadFromCellRef, aSpreadToCellRefs, aSpread, null);
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
		/// <param name="aSpreadFromCellRef">
		/// The ComputationCellReference that identifies the cell being spread from.
		/// </param>
		/// <param name="aSpreadToCellRefs">
		/// An ArrayList of ComputationCellReferences that identify the cells being spread to.
		/// </param>
		/// <param name="aSpread">
		/// The FormulaSpreadProfile of the spread to execute.
		/// </param>
		/// <param name="aCascadeChangeMethodProf">
		/// The ChangeMethodProfile that will be executed for each spread-to cell for a cascade spread.
		/// </param>

		public void InsertSpread(
			ComputationCellReference aSpreadFromCellRef,
			ArrayList aSpreadToCellRefs,
			SpreadProfile aSpread,
			ChangeMethodProfile aCascadeChangeMethodProf)
		{
			ComputationScheduleSpreadEntry spreadFromScheduleEntry;
			ChangeMethodProfile changeMethodProfile;
			string calcDebug = "";
			ArrayList autoTotalList;

			try
			{
				autoTotalList = new ArrayList();

				//Begin Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
				//if (!aSpreadFromCellRef.isCellSpreadScheduled(aSpread.Key))
				if (!aSpreadFromCellRef.isCellSpreadScheduled(aSpread.Key) && aSpreadFromCellRef.canCellBeScheduled)
				//End Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
				{
					aSpreadFromCellRef.AddCellScheduledSpread(aSpread.Key);
					spreadFromScheduleEntry = aSpreadFromCellRef.CreateScheduleSpreadEntry(aSpread, 0, aSpreadFromCellRef.ComputationCube.CubePriority);


                    //Begin TT#1659-MD -jsobek -CelllPendingException Performance
                    spreadFromScheduleEntry.SpreadCellRefList = aSpreadToCellRefs;
                    _scheduleList.Add(spreadFromScheduleEntry);
                    //End TT#1659-MD -jsobek -CelllPendingException Performance


                    //Begin TT#2 - JScott - Assortment Planning - Phase 2
                    //if (aSpread.PostExecuteReInit)
                    //{
                    //    InsertPostExecuteReInitFormula(spreadFromScheduleEntry, aSpreadFromCellRef);
                    //}

                    //End TT#2 - JScott - Assortment Planning - Phase 2
                    foreach (ComputationCellReference compCellRef in aSpreadToCellRefs)
					{
						if (!compCellRef.isCellUserChanged)
						{
							if (compCellRef.CellScheduledSpread == null)
							{
								if (!compCellRef.isCellReadOnly &&
									!compCellRef.isCellProtected &&
									!compCellRef.isCellClosed &&
									!compCellRef.isCellIneligible &&
									//Begin TT#1143 - JScott - Total % change receives Nothing to Spread exception
									!compCellRef.isCellFixed &&
									!compCellRef.isCellBlocked &&
									//End TT#1143 - JScott - Total % change receives Nothing to Spread exception
									!compCellRef.isCellLocked)
								{
									compCellRef.CellScheduledSpread = spreadFromScheduleEntry;

									if (aSpread.CascadeChangeMethods)
									{
										if (aCascadeChangeMethodProf != null)
										{
											aCascadeChangeMethodProf.ExecuteChangeMethod(this, compCellRef, "ComputationSchedule::InsertSpread::1");
										}
										else
										{
											changeMethodProfile = compCellRef.GetPrimaryChangeMethodProfile();

											if (changeMethodProfile != null)
											{
												changeMethodProfile.ExecuteChangeMethod(this, compCellRef, "ComputationSchedule::InsertSpread::2");
											}
										}
									}

									changeMethodProfile = compCellRef.GetSecondaryChangeMethodProfile();

									if (changeMethodProfile != null)
									{
										changeMethodProfile.ExecuteChangeMethod(this, compCellRef, "ComputationSchedule::InsertSpread::3");
									}

									autoTotalList.Add(compCellRef);
								}
							}
							else
							{
								if (aSpreadFromCellRef.ComputationCube.CubePriority < compCellRef.CellScheduledSpread.CubePriority)
								{
									compCellRef.CellScheduledSpread = aSpreadFromCellRef.CreateScheduleSpreadEntry(aSpread, 0, aSpreadFromCellRef.ComputationCube.CubePriority);
								}
								else if (aSpreadFromCellRef.ComputationCube.CubePriority == compCellRef.CellScheduledSpread.CubePriority)
								{
									if (compCellRef.CellScheduledSpread.FormulaSpreadProfile.Key != aSpread.Key)
									{
										calcDebug = "Formula Conflict: Attempting to schedule spread " + aSpread.Name + " for " + compCellRef.GetCalcVariableProfile().VariableName +
											" that already has formula/spread " + compCellRef.CellScheduledSpread.FormulaSpreadProfile.Name + " scheduled for it";
                                        _compCubeGroup.SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, calcDebug, "Scheduler");	// TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
										throw new FormulaConflictException();
									}
								}
							}
						}
					}

					foreach (ComputationCellReference compCellRef in autoTotalList)
					{
						//Begin Track #5809 - JScott - Circular Reference when changing Low-Level-Totals in Multi-Chain
						//intScheduleTotals(compCellRef);
						intScheduleTotals(compCellRef, aSpreadFromCellRef);
						//End Track #5809 - JScott - Circular Reference when changing Low-Level-Totals in Multi-Chain
					}

                    //Begin TT#1659-MD -jsobek -CelllPendingException Performance
					//spreadFromScheduleEntry.SpreadCellRefList = aSpreadToCellRefs;
					//_scheduleList.Add(spreadFromScheduleEntry);
                    //End TT#1659-MD -jsobek -CelllPendingException Performance
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes each item in the schedule queue.
		/// </summary>
		/// <remarks>
		/// For each schedule entry on the queue, the entry is first removed from the queue and the formula or spread is
		/// executed.  If the formula or spread was successful, then the scheduled formula entries in the cells are cleared.
		/// If the execution failed with a CellPendingException (calculation is awaiting other calculations to finish), the
		/// entry is enqueued back onto the schedule queue.  Processing continues until there are no schedule entries left 
		/// (successful completion) or a pass through the queue does not result in any entries being processed (circular
		/// reference).
		/// </remarks>

		public void Execute()
		{
			Hashtable scheduleHash;
			Queue scheduleQueue;
			SortedList hashKeyList;
			int lastScheduleQueueCount;
			int lastScheduleCount;
			int currScheduleCount;
			//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
			int autoTotalsSkipped;
			int autoTotalsQueueSkipped;
			eComputationFormulaReturnType retCode;
			//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
			int i;
			int j;
			ComputationScheduleEntry scheduleEntry;
			Hashtable pendingHash;
			System.IO.StreamWriter debugOut;
			IDictionaryEnumerator pendingEnum;
			Hashtable circularHash;
			ArrayList chainListList;
			CircularList chainList;
			CircularEntry pendingCircEntry;
			CircularEntry currCircEntry;
			int chainCount;
			// Begin TT#1954-MD - JSmith - Assortment
            System.Diagnostics.Stopwatch stopWatch = null;
            int schedulePasses = 0;
            System.IO.StreamWriter perfOut = null;
            System.Diagnostics.Stopwatch entryStopWatch = null;
            ArrayList alTimings = null;
			// End TT#1954-MD - JSmith - Assortment

			try
			{
				if (_scheduleList.Count > 0)
				{
					// Begin TT#1954-MD - JSmith - Assortment
                    if (_showBenchmarking)
                    {
                        perfOut = new StreamWriter(System.Environment.CurrentDirectory + "/RecalcTimings.out", false);
                        perfOut.WriteLine("Number of Schedule Entries: " + _scheduleList.Count);
                        perfOut.WriteLine(" ");
                        alTimings = new ArrayList();
                    }
					// End TT#1954-MD - JSmith - Assortment
					scheduleHash = new Hashtable();
					hashKeyList = new SortedList();

					foreach (ComputationScheduleEntry schedEntry in _scheduleList)
					{
						scheduleQueue = (Queue)scheduleHash[schedEntry.SchedulePriority];

						if (scheduleQueue == null)
						{
							scheduleQueue = new Queue();
							scheduleHash[schedEntry.SchedulePriority] = scheduleQueue;
							hashKeyList.Add(schedEntry.SchedulePriority, null);
						}

						scheduleQueue.Enqueue(schedEntry);
					}

					// Begin TT#1954-MD - JSmith - Assortment
                    if (_showBenchmarking)
                    {
                        _formulaHash = new PerfRecalcHash();
                        stopWatch = new System.Diagnostics.Stopwatch();
                        stopWatch.Start();
                        schedulePasses = 0;
                        entryStopWatch = new System.Diagnostics.Stopwatch();
                    }
					// End TT#1954-MD - JSmith - Assortment
					scheduleQueue = (Queue)scheduleHash[0];

					if (scheduleQueue != null)
					{
						lastScheduleQueueCount = 0;
						//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv

						//while (lastScheduleQueueCount != scheduleQueue.Count && scheduleQueue.Count > 0)
						autoTotalsQueueSkipped = 0;

						while (lastScheduleQueueCount != scheduleQueue.Count && scheduleQueue.Count > autoTotalsQueueSkipped)
						//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
						{
							lastScheduleQueueCount = scheduleQueue.Count;
							//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
							autoTotalsQueueSkipped = 0;
							//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv

							for (i = 0; i < lastScheduleQueueCount; i++)
							{
                                // Begin TT#1954-MD - JSmith - Assortment
                                if (_showBenchmarking)
                                {
                                    entryStopWatch.Restart();
                                }
                                // End TT#1954-MD - JSmith - Assortment
								scheduleEntry = (ComputationScheduleEntry)scheduleQueue.Dequeue();

								try
								{
									//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
									//if (intProcessScheduleEntry(scheduleEntry) == eComputationFormulaReturnType.Pending)
									//{
									//    scheduleQueue.Enqueue(scheduleEntry);
									//}
                                    _compCubeGroup.ClearPendingUndoList();   // TT#1954-MD - JSmith - Assortment Performance
									retCode = intProcessScheduleEntry(scheduleEntry);

                                    // Begin TT#1954-MD - JSmith - Assortment Performance
                                    //if (retCode == eComputationFormulaReturnType.Pending)
                                    //{
                                    //    scheduleQueue.Enqueue(scheduleEntry);
                                    //}
									if (retCode == eComputationFormulaReturnType.Pending
                                        || scheduleEntry.ComputationFormulaReturnType == eComputationFormulaReturnType.Pending)
									{
                                        _compCubeGroup.UndoLastPendingRecompute();
                                        scheduleEntry.ComputationFormulaReturnType = eComputationFormulaReturnType.Successful;
                                        scheduleEntry.ClearLastPendingCell();   // RO-4741 - JSmith - Need to scroll to variables prior to making change
										scheduleQueue.Enqueue(scheduleEntry);
									}
									// End TT#1954-MD - JSmith - Assortment Performance
									else if (retCode == eComputationFormulaReturnType.SkippedAutoTotal)
									{
										autoTotalsQueueSkipped++;
										scheduleQueue.Enqueue(scheduleEntry);
									}
									//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
								}
								catch (CellPendingException)
								{
									scheduleQueue.Enqueue(scheduleEntry);
									continue;
								}

                                // Begin TT#1954-MD - JSmith - Assortment
                                if (_showBenchmarking)
                                {
                                    entryStopWatch.Stop();
                                    alTimings.Add(new PerfRecalcSchedEntry(schedulePasses, scheduleEntry, retCode, entryStopWatch.ElapsedMilliseconds));
                                }
                                // End TT#1954-MD - JSmith - Assortment
							}
							
							// Begin TT#1954-MD - JSmith - Assortment
                            schedulePasses++;
							// End TT#1954-MD - JSmith - Assortment
						}
					}

					lastScheduleCount = 0;
					//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv

					//while (lastScheduleCount != (currScheduleCount = intGetScheduleCount(scheduleHash)) && currScheduleCount > 0)
					autoTotalsSkipped = 0;

					while (lastScheduleCount != (currScheduleCount = intGetScheduleCount(scheduleHash)) && currScheduleCount > autoTotalsSkipped)
					//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
					{
						lastScheduleCount = currScheduleCount;
						//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
						autoTotalsSkipped = 0;
						//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv

						for (i = 0; i < hashKeyList.Count; i++)
						{
							scheduleQueue = (Queue)scheduleHash[hashKeyList.GetKey(i)];
							lastScheduleQueueCount = 0;
							//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv

							//while (lastScheduleQueueCount != scheduleQueue.Count && scheduleQueue.Count > 0)
							autoTotalsQueueSkipped = 0;

							while (lastScheduleQueueCount != scheduleQueue.Count && scheduleQueue.Count > autoTotalsQueueSkipped)
							//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
							{
								lastScheduleQueueCount = scheduleQueue.Count;
								//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
								autoTotalsQueueSkipped = 0;
								//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv

								for (j = 0; j < lastScheduleQueueCount; j++)
								{
                                    // Begin TT#1954-MD - JSmith - Assortment
                                    if (_showBenchmarking)
                                    {
                                        entryStopWatch.Restart();
                                    }
                                    // End TT#1954-MD - JSmith - Assortment
									scheduleEntry = (ComputationScheduleEntry)scheduleQueue.Dequeue();

									try
									{
										//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
										//if (intProcessScheduleEntry(scheduleEntry) == eComputationFormulaReturnType.Pending)
										//{
										//    scheduleQueue.Enqueue(scheduleEntry);
										//}
                                        _compCubeGroup.ClearPendingUndoList();   // TT#1954-MD - JSmith - Assortment Performance
										retCode = intProcessScheduleEntry(scheduleEntry);

                                        // Begin TT#1954-MD - JSmith - Assortment Performance
                                        //if (retCode == eComputationFormulaReturnType.Pending)
                                        //{
                                        //    scheduleQueue.Enqueue(scheduleEntry);
                                        //}
										if (retCode == eComputationFormulaReturnType.Pending
                                            || scheduleEntry.ComputationFormulaReturnType == eComputationFormulaReturnType.Pending)
										{
                                            _compCubeGroup.UndoLastPendingRecompute();
                                            scheduleEntry.ComputationFormulaReturnType = eComputationFormulaReturnType.Successful;
                                            scheduleEntry.ClearLastPendingCell();   // RO-4741 - JSmith - Need to scroll to variables prior to making change
											scheduleQueue.Enqueue(scheduleEntry);
										}
										// End TT#1954-MD - JSmith - Assortment Performance
										else if (retCode == eComputationFormulaReturnType.SkippedAutoTotal)
										{
											autoTotalsQueueSkipped++;
											scheduleQueue.Enqueue(scheduleEntry);
										}
										//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
									}
									catch (CellPendingException)
									{
										scheduleQueue.Enqueue(scheduleEntry);
										continue;
									}

                                    // Begin TT#1954-MD - JSmith - Assortment
                                    if (_showBenchmarking)
                                    {
                                        entryStopWatch.Stop();
                                        alTimings.Add(new PerfRecalcSchedEntry(schedulePasses, scheduleEntry, retCode, entryStopWatch.ElapsedMilliseconds));
                                    }
                                    // End TT#1954-MD - JSmith - Assortment
								}
								
								// Begin TT#1954-MD - JSmith - Assortment
                                schedulePasses++;
								// End TT#1954-MD - JSmith - Assortment
							}
							//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv

							autoTotalsSkipped += autoTotalsQueueSkipped;
							//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
						}
					}

					// Begin TT#1954-MD - JSmith - Assortment
                    if (_showBenchmarking)
                    {
                        PrintTimings(perfOut, alTimings);
                        stopWatch.Stop();
                        perfOut.WriteLine(" ");
                        perfOut.WriteLine("Passes Through Schedule: " + schedulePasses);
                        perfOut.WriteLine("Schedule Execution Time (milliseconds): " + stopWatch.ElapsedMilliseconds);

                        _formulaHash.SortAndPrint(perfOut);

                        perfOut.Close();
                    }
					// End TT#1954-MD - JSmith - Assortment
					
					//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
					//if (currScheduleCount > 0)
					if (currScheduleCount > autoTotalsSkipped)
					//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
					{
						debugOut = new StreamWriter(System.Environment.CurrentDirectory + "/CircularReferenceReport.out", false);

						try
						{
							debugOut.WriteLine("CIRCULAR REFERENCE REPORT");
							debugOut.WriteLine("=========================");
							debugOut.WriteLine();
							debugOut.WriteLine("Waiting Schedule Entries");
							debugOut.WriteLine("------------------------");
							debugOut.WriteLine();

							pendingHash = new Hashtable();

							for (i = 0; i < hashKeyList.Count; i++)
							{
								scheduleQueue = (Queue)scheduleHash[hashKeyList.GetKey(i)];

								while (scheduleQueue.Count > 0)
								{
									scheduleEntry = (ComputationScheduleEntry)scheduleQueue.Dequeue();

									WriteScheduleEntryDebugInfo(debugOut, scheduleEntry);

									switch (scheduleEntry.ScheduleEntryType)
									{
										case eComputationScheduleEntryType.Spread :

											if (scheduleEntry.LastPendingCell.CellCoordinates.Equals(scheduleEntry.ComputationCellRef.CellCoordinates))
											{
												foreach (ComputationCellReference compRef in ((ComputationScheduleSpreadEntry)scheduleEntry).SpreadCellRefList)
												{
													//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
													//if (!compRef.isCellFormulaPending(scheduleEntry) &&
													if (!compRef.isCellFormulaPending(scheduleEntry, true) &&
													//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
														compRef.CellScheduledSpread != null &&
														compRef.CellScheduledSpread.ComputationCellRef.Equals(scheduleEntry.ComputationCellRef))
													{
														pendingHash.Add(compRef.ComputationCell, new CircularEntry(scheduleEntry.LastPendingCell, scheduleEntry));
													}
												}
											}
											else
											{
												foreach (ComputationCellReference compRef in ((ComputationScheduleSpreadEntry)scheduleEntry).SpreadCellRefList)
												{
													if (!scheduleEntry.LastPendingCell.Equals(compRef) &&
														//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
														//!compRef.isCellFormulaPending(scheduleEntry) &&
														!compRef.isCellFormulaPending(scheduleEntry, true) &&
														//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
														compRef.CellScheduledSpread != null &&
														compRef.CellScheduledSpread.ComputationCellRef.Equals(scheduleEntry.ComputationCellRef))
													{
														pendingHash.Add(compRef.ComputationCell, new CircularEntry(scheduleEntry.LastPendingCell, scheduleEntry));
													}
												}
											}

											break;

										case eComputationScheduleEntryType.AutoTotal:

											//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
											if (scheduleEntry.ComputationCellRef.isCellInitialized)
											{
											//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
												pendingHash.Add(scheduleEntry.ComputationCellRef.ComputationCell, new CircularEntry(scheduleEntry.LastPendingCell, scheduleEntry));
											//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
											}
											//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv

											break;

										case eComputationScheduleEntryType.Formula:

											pendingHash.Add(scheduleEntry.ComputationCellRef.ComputationCell, new CircularEntry(scheduleEntry.LastPendingCell, scheduleEntry));

											break;
									}
								}
							}

							debugOut.WriteLine();
							debugOut.WriteLine();
							debugOut.WriteLine("Circular Chains");
							debugOut.WriteLine("---------------");

							pendingEnum = pendingHash.GetEnumerator();
							chainListList = new ArrayList();
							chainCount = 0;

							while (pendingEnum.MoveNext())
							{
								circularHash = new Hashtable();

								pendingCircEntry = (CircularEntry)pendingEnum.Value;

								while (pendingCircEntry != null && !circularHash.Contains(pendingCircEntry.ComputationCellRef.ComputationCell))
								{
									circularHash.Add(pendingCircEntry.ComputationCellRef.ComputationCell, null);
									pendingCircEntry = (CircularEntry)pendingHash[pendingCircEntry.ComputationCellRef.ComputationCell];
								}

								if (pendingCircEntry == null)
								{
									debugOut.WriteLine();
									debugOut.WriteLine("Encountered broken Chain " + chainCount);
									debugOut.WriteLine();
									continue;
								}

								chainList = new CircularList();
								currCircEntry = pendingCircEntry;
							
								do
								{
									chainList.Add(currCircEntry.ComputationCellRef.ComputationCell);
									currCircEntry = (CircularEntry)pendingHash[currCircEntry.ComputationCellRef.ComputationCell];
								} while (currCircEntry.ComputationCellRef.ComputationCell != pendingCircEntry.ComputationCellRef.ComputationCell);

								if (!chainListList.Contains(chainList))
								{
									chainCount++;
									debugOut.WriteLine();
									debugOut.WriteLine("Chain " + chainCount + ":");
									debugOut.WriteLine("--------");
									debugOut.WriteLine();

									foreach (ComputationCell compCell in chainList)
									{
										currCircEntry = (CircularEntry)pendingHash[compCell];

										WriteScheduleEntryDebugInfo(debugOut, currCircEntry.SchdEntry);

										debugOut.WriteLine("|");
										debugOut.WriteLine("|");
										debugOut.WriteLine("V");
										debugOut.WriteLine();
									}

									debugOut.WriteLine("Waiting for first entry");
									chainListList.Add(chainList);
								}
							}
						}
						catch (Exception exc)
						{
							debugOut.WriteLine();
							debugOut.WriteLine();
							debugOut.WriteLine("Error encountered producing report: " + exc.Message);
							debugOut.WriteLine("Error report terminated");
						}
						finally
						{
							debugOut.Close();
						}

						throw new CircularReferenceException();
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void WriteScheduleEntryDebugInfo(System.IO.StreamWriter aDebugOut, ComputationScheduleEntry aScheduleEntry)
		{
			bool firstPass;

			try
			{
				switch (aScheduleEntry.ScheduleEntryType)
				{
					case eComputationScheduleEntryType.Spread:

						aDebugOut.WriteLine("Spread:      " + aScheduleEntry.FormulaSpreadProfile.Name);
						aDebugOut.WriteLine("From:        " + aScheduleEntry.ComputationCellRef.GetCellDescription());
						aDebugOut.Write("To:          ");
						firstPass = true;

						foreach (ComputationCellReference compRef in ((ComputationScheduleSpreadEntry)aScheduleEntry).SpreadCellRefList)
						{
							if (firstPass)
							{
								aDebugOut.WriteLine(compRef.GetCellDescription());
							}
							else
							{
								aDebugOut.WriteLine("             " + compRef.GetCellDescription());
							}

							firstPass = false;
						}

						if (aScheduleEntry.LastPendingCell.CellCoordinates.Equals(aScheduleEntry.ComputationCellRef.CellCoordinates))
						{
							aDebugOut.WriteLine("Waiting for FROM Cell");
							aDebugOut.WriteLine();
						}
						else
						{
							aDebugOut.WriteLine("Waiting for TO Cell: " + aScheduleEntry.LastPendingCell.GetCellDescription());
							aDebugOut.WriteLine();
						}

						break;

					case eComputationScheduleEntryType.AutoTotal:

						//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
						if (aScheduleEntry.ComputationCellRef.isCellInitialized)
						{
						//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
							aDebugOut.WriteLine("AutoTotal:   " + aScheduleEntry.FormulaSpreadProfile.Name);
							aDebugOut.WriteLine("Calculating: " + aScheduleEntry.ComputationCellRef.GetCellDescription());
							aDebugOut.WriteLine("Waiting for: " + aScheduleEntry.LastPendingCell.GetCellDescription());
							aDebugOut.WriteLine();
						//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
						}
						//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv

						break;

					case eComputationScheduleEntryType.Formula:

						aDebugOut.WriteLine("Formula:     " + aScheduleEntry.FormulaSpreadProfile.Name);
						aDebugOut.WriteLine("Calculating: " + aScheduleEntry.ComputationCellRef.GetCellDescription());
						aDebugOut.WriteLine("Waiting for: " + aScheduleEntry.LastPendingCell.GetCellDescription());
						aDebugOut.WriteLine();

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
		/// Method that walk recursively through the totals for the given ComputationCellReference and schedules non-comparative variables.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to initialize the totals for.
		/// </param>

		//Begin Track #5809 - JScott - Circular Reference when changing Low-Level-Totals in Multi-Chain
		//private void intScheduleTotals(ComputationCellReference aCompCellRef)
		private void intScheduleTotals(ComputationCellReference aCompCellRef, ComputationCellReference aSpreadFromCompCellRef)
		//End Track #5809 - JScott - Circular Reference when changing Low-Level-Totals in Multi-Chain
		{
			ArrayList totalCellRefList;
			ChangeMethodProfile changeMethodProfile;
            //Begin TT#2 - JScott - Assortment Planning - Phase 2
            ComputationCellReference totCellRef;
            //End TT#2 - JScott - Assortment Planning - Phase 2

			try
			{
				totalCellRefList = aCompCellRef.GetTotalCellRefArray();

				foreach (ComputationCellReference compCellRef in totalCellRefList)
				{
					//Begin Track #5957 - JScott - Totals not re-calcing on a change to week
					////Begin Track #5752 - JScott - Calculation Time
					//if (!aCompCellRef.isCellAutoTotalsProcessed)
					//{
					//    aCompCellRef.isCellAutoTotalsProcessed = true;
					////End Track #5752 - JScott - Calculation Time
					if (!compCellRef.isCellAutoTotalsProcessed)
					{
						compCellRef.isCellAutoTotalsProcessed = true;
                        //Begin TT#2 - JScott - Assortment Planning - Phase 2

                        if (_compCubeGroup.ReinitTotals)
                        {
                            foreach (QuantityVariableProfile quanVar in compCellRef.ComputationCube.QuantityVariableProfileList)
                            {
                                totCellRef = (ComputationCellReference)compCellRef.Copy();
                                totCellRef[totCellRef.ComputationCube.QuantityVariableProfileType] = quanVar.Key;
                                _autoTotalList.Add(totCellRef);
                            }
                        }

                        //End TT#2 - JScott - Assortment Planning - Phase 2
                        //Skip scheduling the total if it is the cell that is being spread from (total should equal spread to cells).
						//If cell has been changed by a different formula/spread, that formula/spread will schedule the AutoTotal.
						if (aSpreadFromCompCellRef == null || !compCellRef.Equals(aSpreadFromCompCellRef))
						{
						//End Track #5809 - JScott - Circular Reference when changing Low-Level-Totals in Multi-Chain
							foreach (QuantityVariableProfile quanVar in compCellRef.ComputationCube.QuantityVariableProfileList)
							{
								compCellRef[compCellRef.ComputationCube.QuantityVariableProfileType] = quanVar.Key;

								//Begin Track #5752 - JScott - Calculation Time
								changeMethodProfile = compCellRef.GetAutototalChangeMethodProfile();

								if (changeMethodProfile != null)
								{
								//End Track #5752 - JScott - Calculation Time
									if (compCellRef.GetVariableScopeVariableProfile().VariableScope == eVariableScope.Static)
									{
										//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
										//if (!compCellRef.isCellUserChanged && !compCellRef.isCellFormulaPending(null))
										if (!compCellRef.isCellUserChanged && !compCellRef.isCellFormulaPending(null, true))
										//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
										{
										//Begin Track #5752 - JScott - Calculation Time
										//changeMethodProfile = compCellRef.GetAutototalChangeMethodProfile();

										//if (changeMethodProfile != null)
										//{
										//End Track #5752 - JScott - Calculation Time
											changeMethodProfile.ExecuteChangeMethod(this, compCellRef, "ComputationSchedule::intScheduleTotals");
										}
									}
								}
							}
						//Begin Track #5809 - JScott - Circular Reference when changing Low-Level-Totals in Multi-Chain
						}
						//End Track #5809 - JScott - Circular Reference when changing Low-Level-Totals in Multi-Chain
					//Begin Track #5752 - JScott - Calculation Time
					}
					//End Track #5752 - JScott - Calculation Time
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts a formula into the schedule queue for the given changed ComputationCellReference, target ComputationCellReference, and FormulaSpreadProfile.
		/// </summary>
		/// <param name="aChangedCompCellRef">
		/// The ComputationCellReference that identifies the cell that has triggered this computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that identifies the cell being computed.
		/// </param>
		/// <param name="aFormula">
		/// The FormulaSpreadProfile of the forumla to execute.
		/// </param>

		private void intInsertFormula(
			ComputationCellReference aChangedCompCellRef,
			ComputationCellReference aCompCellRef,
			FormulaProfile aFormula,
			//Begin Track #5752 - JScott - Calculation Time
			//eComputationScheduleEntryType aCompSchedEntryType)
			eComputationScheduleEntryType aCompSchedEntryType,
			int aSchedulePriority)
			//End Track #5752 - JScott - Calculation Time
		{
			ChangeMethodProfile changeMethodProfile;
			string calcDebug = "";

            //Debug.WriteLine("intInsertFormula Formula: " + aFormula.Name + " " + aCompCellRef.CellKeys);              

			try
			{
				//Begin Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
				if (aCompCellRef.canCellBeScheduled)
				{
				//End Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
					if (!aCompCellRef.isCellUserChanged)
					{
						//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
						//if (!aCompCellRef.isCellFormulaPending(null))
						if (!aCompCellRef.isCellFormulaPending(null, true))
						//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
						{
							//Begin Track #5752 - JScott - Calculation Time
							//aCompCellRef.CellScheduledFormula = aCompCellRef.CreateScheduleFormulaEntry(aChangedCompCellRef, aFormula, aCompSchedEntryType, 0, aChangedCompCellRef.ComputationCube.CubePriority);
							aCompCellRef.CellScheduledFormula = aCompCellRef.CreateScheduleFormulaEntry(aChangedCompCellRef, aFormula, aCompSchedEntryType, aSchedulePriority, aChangedCompCellRef.ComputationCube.CubePriority);
							//End Track #5752 - JScott - Calculation Time
							_scheduleList.Add(aCompCellRef.CellScheduledFormula);

                            //Begin TT#2 - JScott - Assortment Planning - Phase 2
                            //if (aFormula.PostExecuteReInit)
                            //{
                            //    InsertPostExecuteReInitFormula(aCompCellRef.CellScheduledFormula, aChangedCompCellRef);
                            //}

                            //End TT#2 - JScott - Assortment Planning - Phase 2
                            intScheduleTotals(aCompCellRef, null);
							changeMethodProfile = aCompCellRef.GetSecondaryChangeMethodProfile();

							if (changeMethodProfile != null)
							{
								changeMethodProfile.ExecuteChangeMethod(this, aCompCellRef, "ComputationSchedule::InsertFormula");
							}
						}
						else
						{
							if (aCompCellRef.CellScheduledFormula.ScheduleEntryType == eComputationScheduleEntryType.Spread)
							{
								//Begin Track #5752 - JScott - Calculation Time
								//aCompCellRef.CellScheduledFormula = aCompCellRef.CreateScheduleFormulaEntry(aChangedCompCellRef, aFormula, aCompSchedEntryType, 0, aChangedCompCellRef.ComputationCube.CubePriority);
								aCompCellRef.CellScheduledFormula = aCompCellRef.CreateScheduleFormulaEntry(aChangedCompCellRef, aFormula, aCompSchedEntryType, aSchedulePriority, aChangedCompCellRef.ComputationCube.CubePriority);
								//End Track #5752 - JScott - Calculation Time
								_scheduleList.Add(aCompCellRef.CellScheduledFormula);
                                //Begin TT#2 - JScott - Assortment Planning - Phase 2

                                //if (aFormula.PostExecuteReInit)
                                //{
                                //    InsertPostExecuteReInitFormula(aCompCellRef.CellScheduledFormula, aChangedCompCellRef);
                                //}
                                //End TT#2 - JScott - Assortment Planning - Phase 2
                            }
							else
							{
								if (aCompCellRef.CellScheduledFormula.FormulaSpreadProfile.Key != aFormula.Key)
								{
									calcDebug = "Formula Conflict: Attempting to schedule formula " + aFormula.Name + " for " + aCompCellRef.GetCalcVariableProfile().VariableName +
										" that already has formula/spread " + aCompCellRef.CellScheduledFormula.FormulaSpreadProfile.Name + " scheduled for it";
									_compCubeGroup.SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, calcDebug, "Scheduler");
									throw new FormulaConflictException();
								}
							}
						}
					}
					else
					{
						calcDebug = "Formula Conflict: Attempting to schedule formula " + aFormula.Name + " for " + aCompCellRef.GetCalcVariableProfile().VariableName + " that was changed by User entry";
						_compCubeGroup.SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, calcDebug, "Scheduler");
						throw new FormulaConflictException();
					}
				//Begin Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
				}
				//End Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Processes one schedule entry.
		/// </summary>
		/// <param name="aScheduleEntry">
		/// The ComputationScheduleEntry to process
		/// </param>

		private eComputationFormulaReturnType intProcessScheduleEntry(ComputationScheduleEntry aScheduleEntry)
		{
			// Begin TT#1954-MD - JSmith - Assortment
            eComputationFormulaReturnType retCode = eComputationFormulaReturnType.Successful;
			PerfRecalcHashEntry perfRecalcHashEntry = null;
			// End TT#1954-MD - JSmith - Assortment

			try
			{
				// Begin TT#1954-MD - JSmith - Assortment
                if (_showBenchmarking)
                {
                    perfRecalcHashEntry = (PerfRecalcHashEntry)_formulaHash[aScheduleEntry.FormulaSpreadProfile.Name];

                    if (perfRecalcHashEntry == null)
                    {
                        perfRecalcHashEntry = new PerfRecalcHashEntry(aScheduleEntry.FormulaSpreadProfile);
                        _formulaHash[aScheduleEntry.FormulaSpreadProfile.Name] = perfRecalcHashEntry;
                    }

                    perfRecalcHashEntry.StartTimer();
                }
				// End TT#1954-MD - JSmith - Assortment
				
				retCode = eComputationFormulaReturnType.Successful;

				switch (aScheduleEntry.ScheduleEntryType)
				{
					case eComputationScheduleEntryType.AutoTotal :

						//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
						//if (aScheduleEntry.LastPendingCell == null || (!aScheduleEntry.LastPendingCell.isCellFormulaPending(aScheduleEntry) && !aScheduleEntry.LastPendingCell.isCellSpreadPending(aScheduleEntry)))
						if (aScheduleEntry.ComputationCellRef.isCellInitialized)
						{
							if (aScheduleEntry.LastPendingCell == null || (!aScheduleEntry.LastPendingCell.isCellFormulaPending(aScheduleEntry, true) && !aScheduleEntry.LastPendingCell.isCellSpreadPending(aScheduleEntry, true)))
						//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
							{
								retCode = aScheduleEntry.FormulaSpreadProfile.ExecuteCalc(aScheduleEntry, eGetCellMode.Current, eSetCellMode.AutoTotal, "ComputationSchedule::intProcessScheduleEntry::1");

								if (retCode == eComputationFormulaReturnType.Successful)
								{
									aScheduleEntry.ComputationCellRef.ClearCellScheduledFormula();
								}
							}
							else
							{
								//Begin Track #5752 - JScott - Calculation Time
								//throw new CellPendingException(aScheduleEntry.ComputationCellRef);
								retCode = eComputationFormulaReturnType.Pending;
								//End Track #5752 - JScott - Calculation Time
							}
						//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
						}
						else
						{
							retCode = eComputationFormulaReturnType.SkippedAutoTotal;
						}
						//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv

						break;

                    //Begin TT#2 - JScott - Assortment Planning - Phase 2
                    //case eComputationScheduleEntryType.ForcedReInit:

                    //    if (aScheduleEntry.LastPendingCell == null || (!aScheduleEntry.LastPendingCell.isCellFormulaPending(aScheduleEntry, true) && !aScheduleEntry.LastPendingCell.isCellSpreadPending(aScheduleEntry, true)))
                    //    {
                    //        retCode = aScheduleEntry.FormulaSpreadProfile.ExecuteCalc(aScheduleEntry, eGetCellMode.Current, eSetCellMode.ForcedReInit, "ComputationSchedule::intProcessScheduleEntry::2");

                    //        if (retCode == eComputationFormulaReturnType.Successful)
                    //        {
                    //            aScheduleEntry.ComputationCellRef.ClearCellPostExecuteReInitScheduledFormula();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        retCode = eComputationFormulaReturnType.Pending;
                    //    }

                    //    break;

                    //End TT#2 - JScott - Assortment Planning - Phase 2
                    case eComputationScheduleEntryType.Formula:

						//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
						//if (aScheduleEntry.LastPendingCell == null || (!aScheduleEntry.LastPendingCell.isCellFormulaPending(aScheduleEntry) && !aScheduleEntry.LastPendingCell.isCellSpreadPending(aScheduleEntry)))
						if (aScheduleEntry.LastPendingCell == null || (!aScheduleEntry.LastPendingCell.isCellFormulaPending(aScheduleEntry, true) && !aScheduleEntry.LastPendingCell.isCellSpreadPending(aScheduleEntry, true)))
						//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
						{
							retCode = aScheduleEntry.FormulaSpreadProfile.ExecuteCalc(aScheduleEntry, eGetCellMode.Current, eSetCellMode.Computation, "ComputationSchedule::intProcessScheduleEntry::3");

                            // Begin TT#1954-MD - JSmith - Assortment Performance
							//if (retCode == eComputationFormulaReturnType.Successful)
                            if (aScheduleEntry.ComputationFormulaReturnType == eComputationFormulaReturnType.Pending)
                            {
                                retCode = eComputationFormulaReturnType.Pending;
                            }
							else if (retCode == eComputationFormulaReturnType.Successful)
							// End TT#1954-MD - JSmith - Assortment Performance
							{
								aScheduleEntry.ComputationCellRef.ClearCellScheduledFormula();
							}
						}
						else
						{
							//Begin Track #5752 - JScott - Calculation Time
							//throw new CellPendingException(aScheduleEntry.ComputationCellRef);
							retCode = eComputationFormulaReturnType.Pending;
							//End Track #5752 - JScott - Calculation Time
						}

						break;

					case eComputationScheduleEntryType.Spread :

						//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
						//if (aScheduleEntry.LastPendingCell == null || (!aScheduleEntry.LastPendingCell.isCellFormulaPending(aScheduleEntry) && !aScheduleEntry.LastPendingCell.isCellSpreadPending(aScheduleEntry)))
						if (aScheduleEntry.LastPendingCell == null || (!aScheduleEntry.LastPendingCell.isCellFormulaPending(aScheduleEntry, true) && !aScheduleEntry.LastPendingCell.isCellSpreadPending(aScheduleEntry, true)))
						//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
						{
							//Begin TT#1659-MD -jsobek -CelllPendingException Performance
                            bool isPendingException = false;
							((ComputationScheduleSpreadEntry)aScheduleEntry).CheckSpreadCellsForPending(out isPendingException);
                            if (isPendingException == false)
                            {

                                if (((ComputationScheduleSpreadEntry)aScheduleEntry).IncludedCellRefHash.Count > 0 ||
                                    ((ComputationScheduleSpreadEntry)aScheduleEntry).ComputationCellRef.isCellCompChanged)
                                {
                                    retCode = aScheduleEntry.FormulaSpreadProfile.ExecuteCalc(aScheduleEntry, eGetCellMode.Current, eSetCellMode.Computation, "ComputationSchedule::intProcessScheduleEntry::3");
                                }
                                else
                                {
                                    retCode = eComputationFormulaReturnType.Successful;
                                }

                                if (retCode == eComputationFormulaReturnType.Successful)
                                {
                                    if (((ComputationScheduleSpreadEntry)aScheduleEntry).SpreadCellRefList != null)
                                    {
                                        foreach (ComputationCellReference compCellRef in ((ComputationScheduleSpreadEntry)aScheduleEntry).SpreadCellRefList)
                                        {
                                            compCellRef.ClearCellScheduledSpread();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                retCode = eComputationFormulaReturnType.Pending;
                            }
                            //End TT#1659-MD -jsobek -CelllPendingException Performance
						}
						else
						{
							//Begin Track #5752 - JScott - Calculation Time
							//throw new CellPendingException(aScheduleEntry.ComputationCellRef);
							retCode = eComputationFormulaReturnType.Pending;
							//End Track #5752 - JScott - Calculation Time
						}

						break;
				}

				return retCode;
			}
			//Begin Track #5752 - JScott - Calculation Time
			catch (CellPendingException)
			{
				retCode = eComputationFormulaReturnType.Pending;
				return retCode;
			}
			//End Track #5752 - JScott - Calculation Time
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
			// Begin TT#1954-MD - JSmith - Assortment
            finally
            {
                if (_showBenchmarking)
                {
                    switch (retCode)
                    {
                        case eComputationFormulaReturnType.Successful:
                            perfRecalcHashEntry.StopTimer(PerfStopType.Completed);
                            break;

                        case eComputationFormulaReturnType.SkippedAutoTotal:
                            perfRecalcHashEntry.StopTimer(PerfStopType.Skipped);
                            break;

                        default:
                            perfRecalcHashEntry.StopTimer(PerfStopType.Pending);
                            break;
                    }
                }
            }
			// End TT#1954-MD - JSmith - Assortment
		}

		private int intGetScheduleCount(Hashtable aScheduleHash)
		{
			IDictionaryEnumerator dictEnum;
			int count;

			try
			{
				dictEnum = aScheduleHash.GetEnumerator();
				count = 0;

				while (dictEnum.MoveNext())
				{
					count += ((Queue)dictEnum.Value).Count;
				}

				return count;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#1954-MD - JSmith - Assortment
        public void PrintTimings(System.IO.StreamWriter aOutFile, ArrayList alTimings)
        {
            const string cPassHeading = "Pass";
            const string cTypeHeading = "Type";
            const string cFormulaHeading = "Formula";
            const string cReturnCodeHeading = "Return Code";
            const string cCubeHeading = "Cube";
            const string cKeysHeading = "Keys";
            const string cTimeHeading = "Total Time (milliseconds)";
            const string cUnderline = "------------------------------------------------------------------";

            int maxPassSize = cPassHeading.Length;
            int maxTypeSize = cTypeHeading.Length;
            int maxNameSize = cFormulaHeading.Length;
            int maxReturnCodeSize = cReturnCodeHeading.Length;
            int maxCubeSize = cCubeHeading.Length;
            int maxKeySize = cKeysHeading.Length;
            int maxDurationSize = cTimeHeading.Length;

            try
            {
                foreach (PerfRecalcSchedEntry entry in alTimings)
                {
                    maxPassSize = Math.Max(maxPassSize, entry.Pass.Length);
                    maxTypeSize = Math.Max(maxTypeSize, entry.Type.Length);
                    maxNameSize = Math.Max(maxNameSize, entry.Name.Length);
                    maxReturnCodeSize = Math.Max(maxReturnCodeSize, entry.ReturnCode.Length);
                    maxCubeSize = Math.Max(maxCubeSize, entry.Cube.Length);
                    maxKeySize = Math.Max(maxKeySize, entry.Keys.Length);
                    maxDurationSize = Math.Max(maxDurationSize, entry.Duration.Length);
                }

                aOutFile.WriteLine(
                    cPassHeading.PadRight(maxPassSize) +
                    "\t" + cTypeHeading.PadRight(maxTypeSize) +
                    "\t" + cFormulaHeading.PadRight(maxNameSize) +
                    "\t" + cReturnCodeHeading.PadRight(maxReturnCodeSize) +
                    "\t" + cCubeHeading.PadRight(maxCubeSize) +
                    "\t" + cKeysHeading.PadRight(maxKeySize) +
                    "\t" + cTimeHeading);

                aOutFile.WriteLine(
                    cUnderline.Substring(0, cPassHeading.Length).PadRight(maxPassSize) +
                    "\t" + cUnderline.Substring(0, cTypeHeading.Length).PadRight(maxTypeSize) +
                    "\t" + cUnderline.Substring(0, cFormulaHeading.Length).PadRight(maxNameSize) +
                    "\t" + cUnderline.Substring(0, cReturnCodeHeading.Length).PadRight(maxReturnCodeSize) +
                    "\t" + cUnderline.Substring(0, cCubeHeading.Length).PadRight(maxCubeSize) +
                    "\t" + cUnderline.Substring(0, cKeysHeading.Length).PadRight(maxKeySize) +
                    "\t" + cUnderline.Substring(0, cTimeHeading.Length));

                foreach (PerfRecalcSchedEntry entry in alTimings)
                {
                    aOutFile.WriteLine(
                        entry.Pass.PadRight(maxPassSize) +
                        "\t" + entry.Type.PadRight(maxTypeSize) +
                        "\t" + entry.Name.PadRight(maxNameSize) +
                        "\t" + entry.ReturnCode.PadRight(maxReturnCodeSize) +
                        "\t" + entry.Cube.PadRight(maxCubeSize) +
                        "\t" + entry.Keys.PadRight(maxKeySize) +
                        "\t" + entry.Duration);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
		// End TT#1954-MD - JSmith - Assortment
	}

	public class CircularList : ArrayList
	{
		public override bool Equals(object obj)
		{
			ArrayList list;

			try
			{
				list = (ArrayList)obj;

				if (list.Count == this.Count)
				{
					foreach (object entry in list)
					{
						if (!this.Contains(entry))
						{
							return false;
						}
					}

					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        /// <summary>
        /// Override of the Hash Code generator.
        /// </summary>
        /// <returns>Hash code for this object</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
	}

	public class CircularEntry
	{
		private ComputationCellReference _compCellRef;
		private ComputationScheduleEntry _schdEntry;

		public CircularEntry(ComputationCellReference aCompCellRef, ComputationScheduleEntry aSchdEntry)
		{
			_compCellRef = aCompCellRef;
			_schdEntry = aSchdEntry;
		}

		public ComputationCellReference ComputationCellRef
		{
			get
			{
				return _compCellRef;
			}
		}

		public ComputationScheduleEntry SchdEntry
		{
			get
			{
				return _schdEntry;
			}
		}
	}
	
	// Begin TT#1954-MD - JSmith - Assortment
    public enum PerfStopType
    {
        Completed,
        Pending,
        Skipped
    }

    public class PerfRecalcHash : Hashtable
    {
        private class SortKey : IComparable
        {
            public PerfRecalcHashEntry PerfRecalcHashEntry;

            public SortKey(PerfRecalcHashEntry aPerfRecalcHashEntry)
            {
                PerfRecalcHashEntry = aPerfRecalcHashEntry;
            }

            public override bool Equals(object obj)
            {
                SortKey typeObj = (SortKey)obj;

                return PerfRecalcHashEntry.Name == typeObj.PerfRecalcHashEntry.Name;
            }

            public override int GetHashCode()
            {
                return PerfRecalcHashEntry.GetHashCode();
            }

            public int CompareTo(object obj)
            {
                SortKey typeObj = (SortKey)obj;

                if (PerfRecalcHashEntry.TotalMillseconds != typeObj.PerfRecalcHashEntry.TotalMillseconds)
                {
                    if (PerfRecalcHashEntry.TotalMillseconds < typeObj.PerfRecalcHashEntry.TotalMillseconds)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return string.Compare(PerfRecalcHashEntry.Name, typeObj.PerfRecalcHashEntry.Name) * -1;
                }
            }
        }

        public void SortAndPrint(System.IO.StreamWriter aOutFile)
        {
            const string cFormulaHeading = "Formula";
            const string cAttemptHeading = "Attempted";
            const string cCompleteHeading = "Completed";
            const string cSkipHeading = "Skipped";
            const string cTimeHeading = "Total Time (milliseconds)";
            const string cUnderline = "------------------------------------------------------------------";

            SortedList sortList;
            int maxNameSize;
            int maxAttemptSize;
            int maxCompleteSize;
            int maxSkipSize;
            IDictionaryEnumerator iEnum;
            PerfRecalcHashEntry perfRecalcHashEntry;

            try
            {
                sortList = new SortedList();
                iEnum = this.GetEnumerator();
                maxNameSize = cFormulaHeading.Length;
                maxAttemptSize = cAttemptHeading.Length;
                maxCompleteSize = cCompleteHeading.Length;
                maxSkipSize = cSkipHeading.Length;

                while (iEnum.MoveNext())
                {
                    perfRecalcHashEntry = (PerfRecalcHashEntry)iEnum.Value;
                    maxNameSize = Math.Max(maxNameSize, perfRecalcHashEntry.Name.Length);
                    maxAttemptSize = Math.Max(maxAttemptSize, perfRecalcHashEntry.AttemptedExecCount.ToString().Length);
                    maxCompleteSize = Math.Max(maxCompleteSize, perfRecalcHashEntry.CompletedExecCount.ToString().Length);
                    maxSkipSize = Math.Max(maxSkipSize, perfRecalcHashEntry.SkippedExecCount.ToString().Length);
                    sortList.Add(new SortKey(perfRecalcHashEntry), perfRecalcHashEntry);
                }

                aOutFile.WriteLine();
                iEnum = sortList.GetEnumerator();

                aOutFile.WriteLine(
                    cFormulaHeading.PadRight(maxNameSize) +
                    "\t" + cAttemptHeading.PadRight(maxAttemptSize) +
                    "\t" + cCompleteHeading.PadRight(maxCompleteSize) +
                    "\t" + cSkipHeading.PadRight(maxSkipSize) +
                    "\t" + cTimeHeading);

                aOutFile.WriteLine(
                    cUnderline.Substring(0, cFormulaHeading.Length).PadRight(maxNameSize) +
                    "\t" + cUnderline.Substring(0, cAttemptHeading.Length).PadRight(maxAttemptSize) +
                    "\t" + cUnderline.Substring(0, cCompleteHeading.Length).PadRight(maxCompleteSize) +
                    "\t" + cUnderline.Substring(0, cSkipHeading.Length).PadRight(maxSkipSize) +
                    "\t" + cUnderline.Substring(0, cTimeHeading.Length));

                while (iEnum.MoveNext())
                {
                    perfRecalcHashEntry = (PerfRecalcHashEntry)iEnum.Value;
                    aOutFile.WriteLine(
                        perfRecalcHashEntry.Name.PadRight(maxNameSize) +
                        "\t" + perfRecalcHashEntry.AttemptedExecCount.ToString().PadRight(maxAttemptSize) +
                        "\t" + perfRecalcHashEntry.CompletedExecCount.ToString().PadRight(maxCompleteSize) +
                        "\t" + perfRecalcHashEntry.SkippedExecCount.ToString().PadRight(maxSkipSize) +
                        "\t" + perfRecalcHashEntry.TotalMillseconds);
                }

                aOutFile.WriteLine();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
    }

    public class PerfRecalcHashEntry
    {
        private string _name;
        private int _attemptedExecCount;
        private int _completedExecCount;
        private int _skippedExecCount;
        private System.Diagnostics.Stopwatch _stopWatch;
        private Stack _runningStack;

        public PerfRecalcHashEntry(FormulaSpreadProfile aFormulaSpread)
        {
            _name = aFormulaSpread.Name;
            _attemptedExecCount = 0;
            _completedExecCount = 0;
            _skippedExecCount = 0;
            _stopWatch = new System.Diagnostics.Stopwatch();
            _runningStack = new Stack();
        }

        public PerfRecalcHashEntry(string aName)
        {
            _name = aName;
            _attemptedExecCount = 0;
            _completedExecCount = 0;
            _skippedExecCount = 0;
            _stopWatch = new System.Diagnostics.Stopwatch();
            _runningStack = new Stack();
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public long TotalMillseconds
        {
            get
            {
                return _stopWatch.ElapsedMilliseconds;
            }
        }

        public int AttemptedExecCount
        {
            get
            {
                return _attemptedExecCount;
            }
        }

        public int CompletedExecCount
        {
            get
            {
                return _completedExecCount;
            }
        }

        public int SkippedExecCount
        {
            get
            {
                return _skippedExecCount;
            }
        }

        public void StartTimer()
        {
            try
            {
                _attemptedExecCount++;

                if (_runningStack.Count == 0)
                {
                    _stopWatch.Start();
                }

                _runningStack.Push(this);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void ResumeTimer()
        {
            try
            {
                if (_runningStack.Count == 0)
                {
                    _stopWatch.Start();
                }

                _runningStack.Push(this);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void PauseTimer()
        {
            try
            {
                _runningStack.Pop();

                if (_runningStack.Count == 0)
                {
                    _stopWatch.Stop();
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void StopTimer(PerfStopType aStopType)
        {
            try
            {
                _runningStack.Pop();

                if (_runningStack.Count == 0)
                {
                    _stopWatch.Stop();
                }

                if (aStopType == PerfStopType.Completed)
                {
                    _completedExecCount++;
                }
                else if (aStopType == PerfStopType.Skipped)
                {
                    _skippedExecCount++;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public override bool Equals(object obj)
        {
            return _name == ((PerfRecalcHashEntry)obj)._name;
        }

        public override int GetHashCode()
        {
            return _name.Length;
        }
    }

    public class PerfRecalcSchedEntry
    {
        private int _pass;
        private string _type;
        private string _name;
        private eComputationFormulaReturnType _returnCode;
        private string _cube;
        private string _keys;
        private long _duration;

        public PerfRecalcSchedEntry(int iPass, ComputationScheduleEntry scheduleEntry,
            eComputationFormulaReturnType returnCode,
            long timing)
        {
            _pass = iPass;
            if (scheduleEntry.ScheduleEntryType == eComputationScheduleEntryType.Spread)
            {
                _type = "Spread";
            }
            else if (scheduleEntry.ScheduleEntryType == eComputationScheduleEntryType.AutoTotal)
            {
                _type = "AutoTotal";
            }
            else
            {
                _type = "Formula";
            }
            _name = scheduleEntry.FormulaSpreadProfile.Name;
            _returnCode = returnCode;
            _cube = scheduleEntry.ComputationCellRef.Cube.ToString();
            _keys = scheduleEntry.ComputationCellRef.CellKeys;
            _duration = timing;
        }

        public string Pass
        {
            get
            {
                return _pass.ToString();
            }
        }

        public string Type
        {
            get
            {
                return _type;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string ReturnCode
        {
            get
            {
                return _returnCode.ToString();
            }
        }

        public string Cube
        {
            get
            {
                return _cube;
            }
        }

        public string Keys
        {
            get
            {
                return _keys;
            }
        }

        public string Duration
        {
            get
            {
                return _duration.ToString();
            }
        }
    }
	// End TT#1954-MD - JSmith - Assortment
}