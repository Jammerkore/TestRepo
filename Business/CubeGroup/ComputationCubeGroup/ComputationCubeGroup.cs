using System;
using System.Collections;
using System.Globalization;
using System.Diagnostics;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The UndoEntry class is used to store information about a ComputationCell for undo.
	/// </summary>

	public class UndoEntry
	{
		//=======
		// FIELDS
		//=======

		private ComputationCell _currCompCell;
		private ComputationCell _oldCompCell;
		// Begin TT#1954-MD - JSmith - Assortment Performance
        private ExtensionCell _currExtCell;
        private ExtensionCell _oldExtCell;
        private eSetCellMode _setCellMode;   // RO-4741 - JSmith - Need to scroll to variables prior to making change
		// End TT#1954-MD - JSmith - Assortment Performance

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of UndoEntry using the given ComputationCellReference.
		/// </summary>
		/// <param name="aCurrCompCell">
		/// The cell being backed up.
		/// </param>
		/// <param name="aOldCompCell">
		/// A copy of the cell being backed up.
		/// </param>

        // Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
		//public UndoEntry(ComputationCell aCurrCompCell, ComputationCell aOldCompCell)
        public UndoEntry(ComputationCell aCurrCompCell, ComputationCell aOldCompCell, eSetCellMode aSetCellMode)
		// End RO-4741 - JSmith - Need to scroll to variables prior to making change
		{
			_currCompCell = aCurrCompCell;
			_oldCompCell = aOldCompCell;
            // Begin TT#1954-MD - JSmith - Assortment Performance
            _currExtCell = null;
            _oldExtCell = null;
            _setCellMode = aSetCellMode;   // RO-4741 - JSmith - Need to scroll to variables prior to making change
            // End TT#1954-MD - JSmith - Assortment Performance
		}

        // Begin TT#1954-MD - JSmith - Assortment Performance
        // Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
		//public UndoEntry(ComputationCell aCurrCompCell, ComputationCell aOldCompCell, ExtensionCell aCurrExtCell, ExtensionCell aOldExtCell)
        public UndoEntry(ComputationCell aCurrCompCell, ComputationCell aOldCompCell, ExtensionCell aCurrExtCell, ExtensionCell aOldExtCell, eSetCellMode aSetCellMode)
		// End RO-4741 - JSmith - Need to scroll to variables prior to making change
		{
			_currCompCell = aCurrCompCell;
			_oldCompCell = aOldCompCell;
            _currExtCell = aCurrExtCell;
            _oldExtCell = aOldExtCell;
            _setCellMode = aSetCellMode;   // RO-4741 - JSmith - Need to scroll to variables prior to making change
		}
		// End TT#1954-MD - JSmith - Assortment Performance

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the current ComputationCell.
		/// </summary>

		public ComputationCell CurrComputationCell
		{
			get
			{
				return _currCompCell;
			}
		}

		/// <summary>
		/// Gets the backed up copy of the ComputationCell.
		/// </summary>

		public ComputationCell OldComputationCell
		{
			get
			{
				return _oldCompCell;
			}
		}

        // Begin TT#1954-MD - JSmith - Assortment Performance
        public ExtensionCell CurrExtCell
        {
            get
            {
                return _currExtCell;
            }
        }

        public ExtensionCell OldExtCell
        {
            get
            {
                return _oldExtCell;
            }
        }

        // Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
        public eSetCellMode SetCellMode
        {
            get
            {
                return _setCellMode;
            }
        }
		// End RO-4741 - JSmith - Need to scroll to variables prior to making change
		// End TT#1954-MD - JSmith - Assortment Performance
	}

	/// <summary>
	/// The UndoList class is used to store occurrences of UndoEntry objects.
	/// </summary>

	public class UndoList : Stack
	{
		//=======
		// FIELDS
		//=======

		private int _undoSequence;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of UndoList using the given undo sequence.
		/// </summary>
		/// <param name="aUndoSequence">'
		/// The undo sequence that indicates the undo level.
		/// </param>

		public UndoList(int aUndoSequence)
		{
			_undoSequence = aUndoSequence;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the undo sequence for this UndoEntry.
		/// </summary>

		public int UndoSequence
		{
			get
			{
				return _undoSequence;
			}
		}
	}

	/// <summary>
	/// The ComputationCubeGroup class defines the CubeGroup for a set of Computation cubes.
	/// </summary>
	/// <remarks>
	/// ComputationCubeGroup inherits from the base CubeGroup class.  ComputationCubeGroup adds the additional functionality of a ComputationSchedule, plus
	/// the ability to retrieve wafers of cube data.
	/// </remarks>

	abstract public class ComputationCubeGroup : CubeGroup
	{
		//=======
		// FIELDS
		//=======

		protected System.Collections.Queue _changedList;
		private System.Collections.Queue _compInfoList;

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        //private int _maxUndos = 5;
        private int _maxUndos;
        private bool _reinitTotals;
        //End TT#2 - JScott - Assortment Planning - Phase 2
        private int _maxUndoCells = 1000;
		private int _undoSequence;
		private Stack _undoStack;
		private UndoList _currUndoList;
        private UndoList _pendingUndoList;   // TT#1954-MD - JSmith - Assortment Performance
		private bool _createNewUndoList;
		protected bool _userChanged;
		private Queue _lockRetotalQueue;
		private Hashtable _lockRetotalHash;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ComputationCubeGroup that are owned by the given SessionAddressBlock and Transaction.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to the current SessionAddressBlock.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to the current Transaction.
		/// </param>

		public ComputationCubeGroup(SessionAddressBlock aSAB, Transaction aTransaction)
			: base(aSAB, aTransaction)
		{
			try
			{
				_changedList = new System.Collections.Queue();
				_compInfoList = new System.Collections.Queue();
                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                _maxUndos = 5;
                _reinitTotals = false;
                //End TT#2 - JScott - Assortment Planning - Phase 2
                _undoSequence = -1;
				_undoStack = new Stack();
				_currUndoList = null;
				_createNewUndoList = true;
				_userChanged = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        /// <summary>
        /// Creates a new instance of ComputationCubeGroup that are owned by the given SessionAddressBlock and Transaction.
        /// </summary>
        /// <param name="aSAB">
        /// A reference to the current SessionAddressBlock.
        /// </param>
        /// <param name="aTransaction">
        /// A reference to the current Transaction.
        /// </param>
        /// <param name="aMaxUndos">
        /// The number of Undo operations to allow.
        /// </param>

        public ComputationCubeGroup(SessionAddressBlock aSAB, Transaction aTransaction, int aMaxUndos, bool aReinitTotals)
            : base(aSAB, aTransaction)
        {
            try
            {
                _changedList = new System.Collections.Queue();
                _compInfoList = new System.Collections.Queue();
                _maxUndos = aMaxUndos;
                _reinitTotals = aReinitTotals;
                _undoSequence = -1;
                _undoStack = new Stack();
                _currUndoList = null;
                _createNewUndoList = true;
                _userChanged = false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        //End TT#2 - JScott - Assortment Planning - Phase 2
        //===========
		// PROPERTIES
		//===========

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        /// <summary>
        /// Gets the changed list.
        /// </summary>

        public bool ReinitTotals
        {
            get
            {
                return _reinitTotals;
            }
        }

        //End TT#2 - JScott - Assortment Planning - Phase 2
		/// <summary>
		/// Gets the changed list.
		/// </summary>

		public System.Collections.Queue ChangedList
		{
			get
			{
				return _changedList;
			}
		}

		/// <summary>
		/// Returns the Queue containing this list of ComputationInfo objects that were created during the last computation.
		/// </summary>

		public System.Collections.Queue CompInfoList
		{
			get
			{
				return _compInfoList;
			}
		}

		/// <summary>
		/// Gets the boolean indicating if the user has made a changed to a cell.
		/// </summary>

		public bool UserChanged
		{
			get
			{
				return _userChanged;
			}
		}

		/// <summary>
		/// Gets the Queue that contains the ComputationCellReference of cells to sum locks for.
		/// </summary>

		public Queue LockRetotalQueue
		{
			get
			{
				if (_lockRetotalQueue == null)
				{
					_lockRetotalQueue = new Queue();
				}

				return _lockRetotalQueue;
			}
		}

		/// <summary>
		/// Gets the Queue that contains the ComputationCellReference of cells to sum locks for.
		/// </summary>

		public Hashtable LockRetotalHash
		{
			get
			{
				if (_lockRetotalHash == null)
				{
					_lockRetotalHash = new Hashtable();
				}

				return _lockRetotalHash;
			}
		}

		protected bool CreateNewUndoList
		{
			set
			{
				_createNewUndoList = value;
			}
		}
		//========
		// METHODS
		//========

		//Begin Enhancement - JScott - Add Balance Low Levels functionality
		/// <summary>
		/// Closes this PlanCubeGroup.
		/// </summary>

		public override void CloseCubeGroup()
		{
			try
			{
				base.CloseCubeGroup();

				_changedList.Clear();
				_compInfoList.Clear();
				_undoStack.Clear();

				if (_lockRetotalQueue != null)
				{
					_lockRetotalQueue.Clear();
				}

				if (_lockRetotalHash != null)
				{
					_lockRetotalHash.Clear();
				}

				_changedList = null;
				_compInfoList = null;
				_undoStack = null;
				_currUndoList = null;
				_lockRetotalQueue = null;
				_lockRetotalHash = null;
                _pendingUndoList = null;   // TT#1954-MD - JSmith - Assortment Performance
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Enhancement - JScott - Add Balance Low Levels functionality
		/// <summary>
		/// Creates a ComputationCubeGroupWaferInfo object from a CubeWaferCoordinateList object.
		/// </summary>
		/// <param name="aCoorList">
		/// The list of wafer coordinates to create a ComputationCubeGroupWaferInfo with.
		/// </param>
		/// <returns></returns>

		abstract protected ComputationCubeGroupWaferInfo CreateWaferInfo(CubeWaferCoordinateList aCoorList);

		/// <summary>
		/// Private method that determines the eCubeType that is specified by the given ComputationCubeGroupWaferInfo objects.
		/// </summary>
		/// <param name="aGlobalWaferInfo">
		/// The ComputationCubeGroupWaferInfo object that contains the global cube flags that are used to determine the eCubeType.
		/// </param>
		/// <param name="aRowWaferInfo">
		/// The ComputationCubeGroupWaferInfo object that contains the row cube flags that are used to determine the eCubeType.
		/// </param>
		/// <param name="aColWaferInfo">
		/// The ComputationCubeGroupWaferInfo object that contains the col cube flags that are used to determine the eCubeType.
		/// </param>
		/// <returns>
		/// The eCubeType of the cube that is described by the given ComputationCubeGroupWaferInfo objects.
		/// </returns>
		
		abstract protected eCubeType DetermineCubeType(ComputationCubeGroupWaferInfo aGlobalWaferInfo, ComputationCubeGroupWaferInfo aRowWaferInfo, ComputationCubeGroupWaferInfo aColWaferInfo);

		/// <summary>
		/// This method converts a set of common, row, and column CubeWaferCoordinateList objects for a given eCubeType into the corresponding
		/// ComputationCellReference.
		/// </summary>
		/// <param name="aCubeType">
		/// The eCubeType that identifies the ComputationCube this Cell exists in.
		/// </param>
		/// <param name="aCommonWaferList">
		/// The CubeWaferCoordinateList that contains the common CubeWaferCoordinates.
		/// </param>
		/// <param name="aRowWaferList">
		/// The CubeWaferCoordinateList that contains the row CubeWaferCoordinates.
		/// </param>
		/// <param name="aColWaferList">
		/// The CubeWaferCoordinateList that contains the column CubeWaferCoordinates.
		/// </param>
		/// <returns>
		/// The ComputationCellReference identifying the Cell.
		/// </returns>

		abstract protected ComputationCellReference ConvertCubeWaferInfoToCellReference(
			eCubeType aCubeType,
			CubeWaferCoordinateList aCommonWaferList,
			CubeWaferCoordinateList aRowWaferList,
			CubeWaferCoordinateList aColWaferList,
			ComputationCubeGroupWaferInfo aGlobalWaferInfo,
			ComputationCubeGroupWaferInfo aRowWaferInfo,
			ComputationCubeGroupWaferInfo aColWaferInfo);

		/// <summary>
		/// Abtract method that creates a new CustomStoreFilter object for the given Filter ID.
		/// </summary>
		/// <param name="aFilterID">
		/// The ID of the CustomStoreFilter to create.
		/// </param>
		/// <returns>
		/// A new CustomStoreFilter object.
		/// </returns>
        //abstract protected CustomStoreFilter CreateCustomStoreFilter(int aFilterID);

        abstract protected filter CreateCustomStoreFilter(int aFilterID);

		/// <summary>
		/// Clears the undo stack.
		/// </summary>

		public void ClearUndoStack()
		{
			try
			{
				_undoStack = new Stack();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Backs up a cells value and lock flag.
		/// </summary>
        /// <param name="aCurrCompCell">
		/// The ComputationCellReference of the ComputationCell to backup.
		/// </param>

		// Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
		//public void BackupCellForUndo(ComputationCell aCurrCompCell, ComputationCell aOldCompCell)
		public void BackupCellForUndo(ComputationCell aCurrCompCell, ComputationCell aOldCompCell, eSetCellMode aSetCellMode)
		// End RO-4741 - JSmith - Need to scroll to variables prior to making change
		{
			try
			{
				if (_currUndoList != null)
				{
					// Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
					//_currUndoList.Push(new UndoEntry(aCurrCompCell, aOldCompCell));
					_currUndoList.Push(new UndoEntry(aCurrCompCell, aOldCompCell, aSetCellMode));
					// End RO-4741 - JSmith - Need to scroll to variables prior to making change
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Adds the given PlanCellReference to the Change List.
		/// </summary>
        /// <param name="aCompCellRef">
		/// The PlanCellReference to add to the Change List.
		/// </param>

		public void AddCellToChangedList(ComputationCellReference aCompCellRef)
		{
			try
			{
				_changedList.Enqueue(aCompCellRef.Copy());
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// This method recomputes a ComputationCube.
		/// </summary>
		/// <param name="aHandleErrors">
		/// Indicates whether error should be handled (caught, user message displayed) by this call or thown back to the caller.
		/// </param>
		/// <remarks>
		/// After changes are made to ComputationCubeCell in a ComputationCube, this method is called to apply the ComputationCubeCell's change rules.
		/// </remarks>

        public void RecomputeCubes(bool aHandleErrors)
        {
            ChangeMethodProfile changeMethodProfile;
			ComputationCellReference compCellRef;
			ComputationSchedule compSchd;
			Stack holdUndoStack;
			int i;
			int undoCellCount;
			UndoList undoList;
			string errorMessage;

            try
			{
                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                compSchd = new ComputationSchedule(this);
                
                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                try
				{
                    //Begin TT#2 - JScott - Assortment Planning - Phase 2
                    //compSchd = new ComputationSchedule(this);

                    //End TT#2 - JScott - Assortment Planning - Phase 2
                    while (_changedList.Count > 0)
					{
						compCellRef = (ComputationCellReference)_changedList.Dequeue();
						compSchd.ScheduleAutoTotals(compCellRef);

						changeMethodProfile = compCellRef.GetPrimaryChangeMethodProfile();

						if (changeMethodProfile != null)
						{
							//foreach (ComputationScheduleEntry e in compSchd.ScheduleList)
							//{
							//    Debug.WriteLine(e.FormulaSpreadProfile.Name);
							//}

							changeMethodProfile.ExecuteChangeMethod(compSchd, compCellRef, "ComputationSchedule::RecomputeCubes::1");
						}

						changeMethodProfile = compCellRef.GetSecondaryChangeMethodProfile();

						if (changeMethodProfile != null)
						{
							changeMethodProfile.ExecuteChangeMethod(compSchd, compCellRef, "ComputationSchedule::RecomputeCubes::2");
						}
					}

					compSchd.Execute();

					//foreach (ComputationScheduleEntry e in compSchd.ScheduleList)
					//{
					//    Debug.WriteLine(e.FormulaSpreadProfile.Name);
					//}

					holdUndoStack = new Stack();
					undoCellCount = 0;
					i = 0;

					while (_undoStack.Count > 0 && i < _maxUndos && undoCellCount < _maxUndoCells)
					{
						undoList = (UndoList)_undoStack.Pop();
						holdUndoStack.Push(undoList);
						undoCellCount += undoList.Count;
						i++;
					}

					_undoStack = new Stack();

					while (holdUndoStack.Count > 0)
					{
						_undoStack.Push(holdUndoStack.Pop());
					}
                }
				catch (FormulaConflictException)
				{
					UndoLastRecompute();

					if (aHandleErrors)
					{
						SAB.MessageCallback.HandleMessage(
							MIDText.GetText(eMIDTextCode.msg_pl_FormulaConflict),
							MIDText.GetText(eMIDTextCode.msg_pl_GenericMessageBoxTitle),
							System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    }
					else
					{
						throw;
					}
				}
				catch (CircularReferenceException)
				{
					UndoLastRecompute();

					if (aHandleErrors)
					{
						SAB.MessageCallback.HandleMessage(
							MIDText.GetText(eMIDTextCode.msg_pl_CircularReference),
							MIDText.GetText(eMIDTextCode.msg_pl_GenericMessageBoxTitle),
							System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
					}
					else
					{
						throw;
					}
				}
				catch (CellCompChangedException)
				{
					UndoLastRecompute();

					if (aHandleErrors)
					{
						SAB.MessageCallback.HandleMessage(
							MIDText.GetText(eMIDTextCode.msg_pl_CompChanged),
							MIDText.GetText(eMIDTextCode.msg_pl_GenericMessageBoxTitle),
							System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
					}
					else
					{
						throw;
					}
				}
				catch (NoCellsToSpreadTo)
				{
					UndoLastRecompute();

					if (aHandleErrors)
					{
						SAB.MessageCallback.HandleMessage(
							MIDText.GetText(eMIDTextCode.msg_pl_NoCellToSpreadTo),
							MIDText.GetText(eMIDTextCode.msg_pl_GenericMessageBoxTitle),
							System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
					}
					else
					{
						throw;
					}
				}
				catch (CellNotAvailableException exc)
				{
					UndoLastRecompute();

					if (aHandleErrors)
					{
						errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_pl_CellNotAvailable), exc.Message);

						SAB.MessageCallback.HandleMessage(
							errorMessage,
							MIDText.GetText(eMIDTextCode.msg_pl_GenericMessageBoxTitle),
							System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
					}
					else
					{
						throw;
					}
				}
				catch (CustomUserErrorException exc)
				{
					UndoLastRecompute();

					if (aHandleErrors)
					{
						errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_CustomUserException), exc.Message);

						SAB.MessageCallback.HandleMessage(
							errorMessage,
							MIDText.GetText(eMIDTextCode.msg_pl_GenericMessageBoxTitle),
							System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
					}
					else
					{
						throw;
					}
				}
				finally
				{
					_createNewUndoList = true;
					//Begin Enhancement - JScott - Add Balance Low Levels functionality
					//intClearComputations();
					ClearComputations();
					//End Enhancement - JScott - Add Balance Low Levels functionality
                    //Begin TT#2 - JScott - Assortment Planning - Phase 2

                    if (_reinitTotals)
                    {
                        foreach (ComputationCellReference totCellRef in compSchd.AutoTotalList)
                        {
                            totCellRef.isCellReinit = true;
                        }
                    }
                    //End TT#2 - JScott - Assortment Planning - Phase 2
                }
            }
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#1954-MD - JSmith - Assortment Performance
        public void ClearPendingUndoList()
        {
            _pendingUndoList = new UndoList(0);
        }
		
		/// <summary>
        /// Backs up a cells value and lock flag.
        /// </summary>
        /// <param name="aCurrCompCell">
        /// The ComputationCellReference of the ComputationCell to backup.
        /// </param>

        public void BackupCellForPendingUndo(
            ComputationCell aCurrCompCell, 
            ComputationCell aOldCompCell,
            ExtensionCell aCurrExtCell,
            ExtensionCell aOldExtCell,
            eSetCellMode aSetCellMode   // RO-4741 - JSmith - Need to scroll to variables prior to making change
            )
        {
            try
            {
                if (_pendingUndoList != null)
                {
                    // Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
					//_pendingUndoList.Push(new UndoEntry(aCurrCompCell, aOldCompCell, aCurrExtCell, aOldExtCell));
                    _pendingUndoList.Push(new UndoEntry(aCurrCompCell, aOldCompCell, aCurrExtCell, aOldExtCell, aSetCellMode));
					// End RO-4741 - JSmith - Need to scroll to variables prior to making change
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void UndoLastPendingRecompute()
        {
            UndoEntry undoEntry;

            try
            {
                if (_pendingUndoList.Count > 0)
                {
                    while (_pendingUndoList.Count > 0)
                    {
                        undoEntry = (UndoEntry)_pendingUndoList.Pop();
                        undoEntry.CurrComputationCell.CopyFrom(undoEntry.OldComputationCell);

                        // Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
                        if (undoEntry.SetCellMode == eSetCellMode.Initialize)
                        {
                            undoEntry.CurrComputationCell.isInitialized = false;
                            undoEntry.CurrComputationCell.isCurrentInitialized = false;
                        }
                        else if (undoEntry.SetCellMode == eSetCellMode.InitializeCurrent)
                        {
                            undoEntry.CurrComputationCell.isCurrentInitialized = false;
                        }
						// End RO-4741 - JSmith - Need to scroll to variables prior to making change

                        if (undoEntry.CurrExtCell != null)
                        {
                            undoEntry.CurrExtCell.PostInitValue = undoEntry.OldExtCell.PostInitValue;
                            undoEntry.CurrExtCell.PreInitValue = undoEntry.OldExtCell.PreInitValue;
                            if (undoEntry.CurrExtCell.isCompInfoAllocated)
                            {
                                undoEntry.CurrExtCell.GetCompInfo().isAutoTotalsProcessed = false;
                                undoEntry.CurrExtCell.GetCompInfo().isCompChanged = false;
                                undoEntry.CurrExtCell.GetCompInfo().isCompLocked = false;
                                undoEntry.CurrExtCell.GetCompInfo().isExcludedFromSpread = false;
                            }
                        }
                    }
                }

                _pendingUndoList = null;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
		// End TT#1954-MD - JSmith - Assortment Performance

		/// <summary>
		/// This method loops through all of the Cubes in the collection and performs an undo on the last sequence.
		/// </summary>

		public void UndoLastRecompute()
		{
			UndoList undoList;
			UndoEntry undoEntry;

			try
			{
				if (_undoStack.Count > 0)
				{
					undoList = (UndoList)_undoStack.Pop();

					while (undoList.Count > 0)
					{
						undoEntry = (UndoEntry)undoList.Pop();
						undoEntry.CurrComputationCell.CopyFrom(undoEntry.OldComputationCell);
					}

					_undoSequence--;

					if (_undoStack.Count > 0)
					{
						_currUndoList = (UndoList)_undoStack.Peek();
					}
					else
					{
						_currUndoList = null;
					}

					_createNewUndoList = true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the current Store Group for the Cube Group.
		/// </summary>
		/// <param name="aStoreGroupProfile">
		/// The StoreGroupProfile that describes the current Store Group.
		/// </param>

		public void SetStoreGroup(StoreGroupProfile aStoreGroupProfile)
		{
			try
			{
				CurrentStoreGroupProfile = aStoreGroupProfile;

				ClearGroupTotalCubes();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the the filter that will be applied to the Store ProfileList.
		/// </summary>
		/// <param name="aFilterID">
		/// The ID of the Filter to apply.
		/// </param>
        public bool SetStoreFilter(int filterRID, PlanCubeGroup planCubeGroup)
		{
			//CustomStoreFilter customStoreFilter;
			// BEGIN Issue 5727 stodd 
			//bool filterWasSuccessful = true;
			// END Issue 5727
			try
			{
				ResetFilteredList(eProfileType.Store);

                if (filterRID != -1)
				{
					ClearStoreTotalCubes();

                    //filter customStoreFilter = CreateCustomStoreFilter(aFilterID);

                    filter customStoreFilter = filterDataHelper.LoadExistingFilter(filterRID);
                    customStoreFilter.SetExtraInfoForCubes(_SAB, _transaction, planCubeGroup);
					// BEGIN Issue 5727 stodd 
                    //if (customStoreFilter.FilterOutdatedInformation)
                    //    filterWasSuccessful = false;
					// END Issue 5727 stodd 
                    //Begin TT#1818 - DOConnell - Delete of Date Range referenced in a filter is allowed 
                    //if (filterWasSuccessful)
					ApplyFilter(customStoreFilter, eFilterType.Temporary);
                    //End TT#1818 - DOConnell - Delete of Date Range referenced in a filter is allowed
					customStoreFilter.Dispose();
				}

				ClearGroupTotalCubes();
				ClearStoreTotalCubes();
				// BEGIN Issue 5727 stodd 
				return true;
				// END Issue 5727 stodd 
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		virtual public void ClearStoreTotalCubes()
		{
			ComputationCube compCube;

			try
			{
				foreach (DictionaryEntry dictEntry in CubeTable)
				{
					compCube = (ComputationCube)dictEntry.Value;
					if (compCube.isStoreTotalCube)
					{
						compCube.Clear();
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		virtual public void ClearGroupTotalCubes()
		{
			ComputationCube compCube;

			try
			{
				foreach (DictionaryEntry dictEntry in CubeTable)
				{
					compCube = (ComputationCube)dictEntry.Value;
					if (compCube.isGroupTotalCube)
					{
						compCube.Clear();
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the lock status of a cell specified by the given CubeWaferCoordinateLists.
		/// </summary>
		/// <param name="aCommonWaferList">
		/// A CubeWaferCoordinateList that contains the common CubeWaferCoordinates.
		/// </param>
		/// <param name="aRowWaferList">
		/// A CubeWaferCoordinateList that contains the row CubeWaferCoordinates.
		/// </param>
		/// <param name="aColWaferList">
		/// A CubeWaferCoordinateList that contains the column CubeWaferCoordinates.
		/// </param>
		/// <param name="aLockStatus">
		/// A boolean indicating if the Cell should be marked locked or unlocked.
		/// </param>

		public void SetCellLockStatus(CubeWaferCoordinateList aCommonWaferList, CubeWaferCoordinateList aRowWaferList, CubeWaferCoordinateList aColWaferList, bool aLockStatus)
		{
			ComputationCellReference compCellRef;
			ComputationCubeGroupWaferInfo globalWaferInfo;
			ComputationCubeGroupWaferInfo rowWaferInfo;
			ComputationCubeGroupWaferInfo colWaferInfo;
			eCubeType cubeType;

			try
			{
				if (aCommonWaferList != null)
				{
					globalWaferInfo = CreateWaferInfo(aCommonWaferList);

					if (aRowWaferList != null)
					{
						rowWaferInfo = CreateWaferInfo(aRowWaferList);

						if (aColWaferList != null)
						{
							colWaferInfo = CreateWaferInfo(aColWaferList);
							cubeType = DetermineCubeType(globalWaferInfo, rowWaferInfo, colWaferInfo);
							compCellRef = ConvertCubeWaferInfoToCellReference(
								cubeType,
								aCommonWaferList,
								aRowWaferList,
								aColWaferList,
								globalWaferInfo,
								rowWaferInfo,
								colWaferInfo);

							if (compCellRef != null && compCellRef.isCellValid && !compCellRef.isCellNull && !compCellRef.isCellBlocked && !compCellRef.isCellHidden)
							{
								if (compCellRef.isCellReadOnly)
								{
									throw new MIDException(eErrorLevel.severe,
										(int)eMIDTextCode.msg_pl_CellIsReadOnly,
										MIDText.GetText(eMIDTextCode.msg_pl_CellIsReadOnly));
								}
								else if (compCellRef.isCellDisplayOnly)
								{
									throw new MIDException(eErrorLevel.severe,
										(int)eMIDTextCode.msg_pl_CellIsDisplayOnly,
										MIDText.GetText(eMIDTextCode.msg_pl_CellIsDisplayOnly));
								}
								else if (compCellRef.isCellProtected)
								{
									throw new MIDException(eErrorLevel.severe,
										(int)eMIDTextCode.msg_pl_CellIsProtected,
										MIDText.GetText(eMIDTextCode.msg_pl_CellIsProtected));
								}
								else if (compCellRef.isCellClosed)
								{
									throw new MIDException(eErrorLevel.severe,
										(int)eMIDTextCode.msg_pl_CellIsClosed,
										MIDText.GetText(eMIDTextCode.msg_pl_CellIsClosed));
								}
								else if (compCellRef.isCellIneligible)
								{
									throw new MIDException(eErrorLevel.severe,
										(int)eMIDTextCode.msg_pl_CellIsIneligible,
										MIDText.GetText(eMIDTextCode.msg_pl_CellIsIneligible));
								}

								//Begin Enhancement - JScott - Add Balance Low Levels functionality
								//intGetCurrUndoList();
								CreateUndoRestorePoint();
								//End Enhancement - JScott - Add Balance Low Levels functionality
								compCellRef.SetCellLock(aLockStatus);
								//Begin TT#1134 - JScott - MID 4.0 does not automatically save locked cells in the OTS forecast

								_userChanged = true;
								//End TT#1134 - JScott - MID 4.0 does not automatically save locked cells in the OTS forecast
							}
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the lock status of a cell specified by the given CubeWaferCoordinateLists.
		/// </summary>
		/// <param name="aCommonWaferList">
		/// A CubeWaferCoordinateList that contains the common CubeWaferCoordinates.
		/// </param>
		/// <param name="aRowWaferList">
		/// A CubeWaferCoordinateList that contains the row CubeWaferCoordinates.
		/// </param>
		/// <param name="aColWaferList">
		/// A CubeWaferCoordinateList that contains the column CubeWaferCoordinates.
		/// </param>
		/// <param name="aLockStatus">
		/// A boolean indicating if the Cell should be marked locked or unlocked.
		/// </param>

        public ComputationCellReference SetCellRecursiveLockStatus(CubeWaferCoordinateList aCommonWaferList, CubeWaferCoordinateList aRowWaferList, CubeWaferCoordinateList aColWaferList, bool aLockStatus)	// TT#3848 - stodd - Locked cell not able to be changed after unlocking
		{
			ComputationCellReference compCellRef = null;		// TT#3848 - stodd - Locked cell not able to be changed after unlocking
			ComputationCubeGroupWaferInfo globalWaferInfo;
			ComputationCubeGroupWaferInfo rowWaferInfo;
			ComputationCubeGroupWaferInfo colWaferInfo;
			eCubeType cubeType;

			try
			{
				if (aCommonWaferList != null)
				{
					globalWaferInfo = CreateWaferInfo(aCommonWaferList);

					if (aRowWaferList != null)
					{
						rowWaferInfo = CreateWaferInfo(aRowWaferList);

						if (aColWaferList != null)
						{
							colWaferInfo = CreateWaferInfo(aColWaferList);
							cubeType = DetermineCubeType(globalWaferInfo, rowWaferInfo, colWaferInfo);
							compCellRef = ConvertCubeWaferInfoToCellReference(
								cubeType,
								aCommonWaferList,
								aRowWaferList,
								aColWaferList,
								globalWaferInfo,
								rowWaferInfo,
								colWaferInfo);

							// Begin TT#3848 - stodd - Locked cell not able to be changed after unlocking
                            SetCellRecursiveLockStatus(compCellRef, aLockStatus);

                            //if (compCellRef != null && compCellRef.isCellValid && !compCellRef.isCellNull && !compCellRef.isCellBlocked && !compCellRef.isCellHidden)
                            //{
                            //    if (compCellRef.isCellReadOnly)
                            //    {
                            //        throw new MIDException(eErrorLevel.severe,
                            //            (int)eMIDTextCode.msg_pl_CellIsReadOnly,
                            //            MIDText.GetText(eMIDTextCode.msg_pl_CellIsReadOnly));
                            //    }
                            //    else if (compCellRef.isCellDisplayOnly)
                            //    {
                            //        throw new MIDException(eErrorLevel.severe,
                            //            (int)eMIDTextCode.msg_pl_CellIsDisplayOnly,
                            //            MIDText.GetText(eMIDTextCode.msg_pl_CellIsDisplayOnly));
                            //    }
                            //    else if (compCellRef.isCellProtected)
                            //    {
                            //        throw new MIDException(eErrorLevel.severe,
                            //            (int)eMIDTextCode.msg_pl_CellIsProtected,
                            //            MIDText.GetText(eMIDTextCode.msg_pl_CellIsProtected));
                            //    }
                            //    else if (compCellRef.isCellClosed)
                            //    {
                            //        throw new MIDException(eErrorLevel.severe,
                            //            (int)eMIDTextCode.msg_pl_CellIsClosed,
                            //            MIDText.GetText(eMIDTextCode.msg_pl_CellIsClosed));
                            //    }
                            //    else if (compCellRef.isCellIneligible)
                            //    {
                            //        throw new MIDException(eErrorLevel.severe,
                            //            (int)eMIDTextCode.msg_pl_CellIsIneligible,
                            //            MIDText.GetText(eMIDTextCode.msg_pl_CellIsIneligible));
                            //    }

                            //    LockRetotalHash.Clear();
                            //    LockRetotalQueue.Clear();

                            //    //Begin Enhancement - JScott - Add Balance Low Levels functionality
                            //    //intGetCurrUndoList();
                            //    CreateUndoRestorePoint();
                            //    //End Enhancement - JScott - Add Balance Low Levels functionality
                            //    intSetRecursiveCellLock(compCellRef, aLockStatus);
                            //    intProcessTotalCellLockSum();
                            //    //Begin TT#1134 - JScott - MID 4.0 does not automatically save locked cells in the OTS forecast

                            //    _userChanged = true;
                            //    //End TT#1134 - JScott - MID 4.0 does not automatically save locked cells in the OTS forecast
							//}
							// End TT#3848 - stodd - Locked cell not able to be changed after unlocking
						}
					}
				}

                return compCellRef;		// TT#3848 - stodd - Locked cell not able to be changed after unlocking
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// Begin TT#3848 - stodd - Locked cell not able to be changed after unlocking
        /// <summary>
        /// Sets the lock status of a cell.
        /// </summary>
        /// <param name="compCellRef"></param>
        /// <param name="aLockStatus"></param>
        public void SetCellRecursiveLockStatus(ComputationCellReference compCellRef, bool aLockStatus)
        {
            try
            {

                if (compCellRef != null && compCellRef.isCellValid && !compCellRef.isCellNull && !compCellRef.isCellBlocked && !compCellRef.isCellHidden)
                {
                    if (compCellRef.isCellReadOnly)
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_pl_CellIsReadOnly,
                            MIDText.GetText(eMIDTextCode.msg_pl_CellIsReadOnly));
                    }
                    else if (compCellRef.isCellDisplayOnly)
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_pl_CellIsDisplayOnly,
                            MIDText.GetText(eMIDTextCode.msg_pl_CellIsDisplayOnly));
                    }
                    else if (compCellRef.isCellProtected)
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_pl_CellIsProtected,
                            MIDText.GetText(eMIDTextCode.msg_pl_CellIsProtected));
                    }
                    else if (compCellRef.isCellClosed)
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_pl_CellIsClosed,
                            MIDText.GetText(eMIDTextCode.msg_pl_CellIsClosed));
                    }
                    else if (compCellRef.isCellIneligible)
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_pl_CellIsIneligible,
                            MIDText.GetText(eMIDTextCode.msg_pl_CellIsIneligible));
                    }

                    LockRetotalHash.Clear();
                    LockRetotalQueue.Clear();

                    //Begin Enhancement - JScott - Add Balance Low Levels functionality
                    //intGetCurrUndoList();
                    CreateUndoRestorePoint();
                    //End Enhancement - JScott - Add Balance Low Levels functionality
                    intSetRecursiveCellLock(compCellRef, aLockStatus);
                    intProcessTotalCellLockSum();
                    //Begin TT#1134 - JScott - MID 4.0 does not automatically save locked cells in the OTS forecast

                    _userChanged = true;
                    //End TT#1134 - JScott - MID 4.0 does not automatically save locked cells in the OTS forecast
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
		// End TT#3848 - stodd - Locked cell not able to be changed after unlocking

		//BEGIN TT#1996 - stodd - Rtl and Cost do not update on Assortment tab dynamically
		public void SetCellValue(CubeWaferCoordinateList aCommonWaferList, CubeWaferCoordinateList aRowWaferList, CubeWaferCoordinateList aColWaferList, 
			double aValue, string aUnitScaling, string aDollarScaling)
		{
			SetCellValue(aCommonWaferList, aRowWaferList, aColWaferList, aValue, aUnitScaling, aDollarScaling, false);
		}
		//END TT#1996 - stodd - Rtl and Cost do not update on Assortment tab dynamically

		/// <summary>
		/// Sets the cell specified by the given CubeWaferCoordinateLists to the given value.
		/// </summary>
		/// <param name="aCommonWaferList">
		/// A CubeWaferCoordinateList that contains the common CubeWaferCoordinates.
		/// </param>
		/// <param name="aRowWaferList">
		/// A CubeWaferCoordinateList that contains the row CubeWaferCoordinates.
		/// </param>
		/// <param name="aColWaferList">
		/// A CubeWaferCoordinateList that contains the column CubeWaferCoordinates.
		/// </param>
		/// <param name="aValue">
		/// The double value that the cell will be updated with.
		/// </param>

		//Begin Modification - JScott - Add Scaling Decimals
		//public void SetCellValue(CubeWaferCoordinateList aCommonWaferList, CubeWaferCoordinateList aRowWaferList, CubeWaferCoordinateList aColWaferList, double aValue, int aUnitScaling, int aDollarScaling)
		//BEGIN TT#1996 - stodd - Rtl and Cost do not update on Assortment tab dynamically
		public void SetCellValue(CubeWaferCoordinateList aCommonWaferList, CubeWaferCoordinateList aRowWaferList, CubeWaferCoordinateList aColWaferList, double aValue, string aUnitScaling, string aDollarScaling, bool ignoreDisplayOnly)
		//END TT#1996 - stodd - Rtl and Cost do not update on Assortment tab dynamically
		//End Modification - JScott - Add Scaling Decimals
		{
			ComputationCellReference compCellRef;
			ComputationCubeGroupWaferInfo globalWaferInfo;
			ComputationCubeGroupWaferInfo rowWaferInfo;
			ComputationCubeGroupWaferInfo colWaferInfo;
			eCubeType cubeType;
			string errorMessage;

			try
			{
				if (aCommonWaferList != null)
				{
					globalWaferInfo = CreateWaferInfo(aCommonWaferList);

					if (aRowWaferList != null)
					{
						rowWaferInfo = CreateWaferInfo(aRowWaferList);

						if (aColWaferList != null)
						{
							colWaferInfo = CreateWaferInfo(aColWaferList);
							cubeType = DetermineCubeType(globalWaferInfo, rowWaferInfo, colWaferInfo);
							compCellRef = ConvertCubeWaferInfoToCellReference(
								cubeType,
								aCommonWaferList,
								aRowWaferList,
								aColWaferList,
								globalWaferInfo,
								rowWaferInfo,
								colWaferInfo);

							if (compCellRef != null && compCellRef.isCellValid && !compCellRef.isCellNull && !compCellRef.isCellBlocked && !compCellRef.isCellHidden)
							{
								if (compCellRef.isCellReadOnly)
								{
									throw new MIDException(eErrorLevel.severe,
										(int)eMIDTextCode.msg_pl_CellIsReadOnly,
										MIDText.GetText(eMIDTextCode.msg_pl_CellIsReadOnly));
								}
								//BEGIN TT#1996 - stodd - Rtl and Cost do not update on Assortment tab dynamically
								else if (compCellRef.isCellDisplayOnly && !ignoreDisplayOnly)
								//END TT#1996 - stodd - Rtl and Cost do not update on Assortment tab dynamically
								{
									throw new MIDException(eErrorLevel.severe,
										(int)eMIDTextCode.msg_pl_CellIsDisplayOnly,
										MIDText.GetText(eMIDTextCode.msg_pl_CellIsDisplayOnly));
								}
								else if (compCellRef.isCellProtected)
								{
									throw new MIDException(eErrorLevel.severe,
										(int)eMIDTextCode.msg_pl_CellIsProtected,
										MIDText.GetText(eMIDTextCode.msg_pl_CellIsProtected));
								}
								else if (compCellRef.isCellClosed)
								{
									throw new MIDException(eErrorLevel.severe,
										(int)eMIDTextCode.msg_pl_CellIsClosed,
										MIDText.GetText(eMIDTextCode.msg_pl_CellIsClosed));
								}
								else if (compCellRef.isCellIneligible)
								{
									throw new MIDException(eErrorLevel.severe,
										(int)eMIDTextCode.msg_pl_CellIsIneligible,
										MIDText.GetText(eMIDTextCode.msg_pl_CellIsIneligible));
								}

								//Begin Enhancement - JScott - Add Balance Low Levels functionality
								//intGetCurrUndoList();
								CreateUndoRestorePoint();
								//End Enhancement - JScott - Add Balance Low Levels functionality

								switch (compCellRef.GetVariableStyleVariableProfile().VariableStyle)
								{
									case eVariableStyle.Units:
										//Begin Track #5819 -- Scaling Decimal when set to 100.0 or 1000.0 receive "Input not Valid" when trying to make a change.
										////Begin Modification - JScott - Add Scaling Decimals
										////compCellRef.SetEntryCellValue((double)(decimal)(aValue * aUnitScaling));
										//compCellRef.SetEntryCellValue((double)(decimal)(aValue * Convert.ToInt32(aUnitScaling)));
										////End Modification - JScott - Add Scaling Decimals
										compCellRef.SetEntryCellValue((double)(decimal)(aValue * Convert.ToDouble(aUnitScaling)));
										//End Track #5819 -- Scaling Decimal when set to 100.0 or 1000.0 receive "Input not Valid" when trying to make a change.
										break;

									case eVariableStyle.Dollar:
										//Begin Track #5819 -- Scaling Decimal when set to 100.0 or 1000.0 receive "Input not Valid" when trying to make a change.
										////Begin Modification - JScott - Add Scaling Decimals
										////compCellRef.SetEntryCellValue((double)(decimal)(aValue * aDollarScaling));
										//compCellRef.SetEntryCellValue((double)(decimal)(aValue * Convert.ToInt32(aDollarScaling)));
										////End Modification - JScott - Add Scaling Decimals
										compCellRef.SetEntryCellValue((double)(decimal)(aValue * Convert.ToDouble(aDollarScaling)));
										//End Track #5819 -- Scaling Decimal when set to 100.0 or 1000.0 receive "Input not Valid" when trying to make a change.
										break;

									default:
										compCellRef.SetEntryCellValue(aValue);
										break;
								}

								_userChanged = true;
							}
						}
					}
				}
			}
			catch (CellNotAvailableException exc)
			{
				errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_pl_CellNotAvailable), exc.Message);

				SAB.MessageCallback.HandleMessage(
					errorMessage,
					MIDText.GetText(eMIDTextCode.msg_pl_GenericMessageBoxTitle),
					System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);

				throw new CellUnavailableException();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//BEGIN TT#1996 - stodd - Rtl and Cost do not update on Assortment tab dynamically
		public void SetIsCellInitialized(CubeWaferCoordinateList aCommonWaferList, CubeWaferCoordinateList aRowWaferList, CubeWaferCoordinateList aColWaferList)
		{
			ComputationCellReference compCellRef;
			ComputationCubeGroupWaferInfo globalWaferInfo;
			ComputationCubeGroupWaferInfo rowWaferInfo;
			ComputationCubeGroupWaferInfo colWaferInfo;
			eCubeType cubeType;
			string errorMessage;

			try
			{
				if (aCommonWaferList != null)
				{
					globalWaferInfo = CreateWaferInfo(aCommonWaferList);

					if (aRowWaferList != null)
					{
						rowWaferInfo = CreateWaferInfo(aRowWaferList);

						if (aColWaferList != null)
						{
							colWaferInfo = CreateWaferInfo(aColWaferList);
							cubeType = DetermineCubeType(globalWaferInfo, rowWaferInfo, colWaferInfo);
							compCellRef = ConvertCubeWaferInfoToCellReference(
								cubeType,
								aCommonWaferList,
								aRowWaferList,
								aColWaferList,
								globalWaferInfo,
								rowWaferInfo,
								colWaferInfo);

							compCellRef.InitCellValue();
							//compCellRef.isCellInitialized = false;
							//compCellRef.isCellChanged = true;
						}
					}
				}
			}
			catch (CellNotAvailableException exc)
			{
				errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_pl_CellNotAvailable), exc.Message);

				SAB.MessageCallback.HandleMessage(
					errorMessage,
					MIDText.GetText(eMIDTextCode.msg_pl_GenericMessageBoxTitle),
					System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);

				throw new CellUnavailableException();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// Begin TT#1189-md - stodd - adding locking to group allocation
        public ComputationCellReference GetCell(CubeWaferCoordinateList aCommonWaferList, CubeWaferCoordinateList aRowWaferList, CubeWaferCoordinateList aColWaferList)
		{
			ComputationCellReference compCellRef =  null;
			ComputationCubeGroupWaferInfo globalWaferInfo;
			ComputationCubeGroupWaferInfo rowWaferInfo;
			ComputationCubeGroupWaferInfo colWaferInfo;
			eCubeType cubeType;
			string errorMessage;

			try
			{
				if (aCommonWaferList != null)
				{
					globalWaferInfo = CreateWaferInfo(aCommonWaferList);

					if (aRowWaferList != null)
					{
						rowWaferInfo = CreateWaferInfo(aRowWaferList);

						if (aColWaferList != null)
						{
							colWaferInfo = CreateWaferInfo(aColWaferList);
							cubeType = DetermineCubeType(globalWaferInfo, rowWaferInfo, colWaferInfo);
							compCellRef = ConvertCubeWaferInfoToCellReference(
								cubeType,
								aCommonWaferList,
								aRowWaferList,
								aColWaferList,
								globalWaferInfo,
								rowWaferInfo,
								colWaferInfo);

							double val = compCellRef.CurrentCellValue;
							compCellRef.InitCellValue();
						}
					}
				}
                return compCellRef;
			}
			// End TT#1189-md - stodd - adding locking to group allocation
			catch (CellNotAvailableException exc)
			{
				errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_pl_CellNotAvailable), exc.Message);

				SAB.MessageCallback.HandleMessage(
					errorMessage,
					MIDText.GetText(eMIDTextCode.msg_pl_GenericMessageBoxTitle),
					System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);

				throw new CellUnavailableException();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// This method clears all computation objects that were used during the last computation, but are no longer needed.
		/// </summary>

		//Begin Enhancement - JScott - Add Balance Low Levels functionality
		//protected void intClearComputations()
		protected void ClearComputations()
		//End Enhancement - JScott - Add Balance Low Levels functionality
		{
			try
			{
				foreach (ExtensionCell extCell in _compInfoList)
				{
					extCell.SetCompInfo(null);
				}

				_changedList.Clear();
				_compInfoList.Clear();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the current Undo List, creating if necessary.
		/// </summary>
		/// <returns>
		/// The current Undo List.
		/// </returns>

		//Begin Enhancement - JScott - Add Balance Low Levels functionality
		//protected UndoList intGetCurrUndoList()
		protected void CreateUndoRestorePoint()
		//End Enhancement - JScott - Add Balance Low Levels functionality
		{
			try
			{
				if (_createNewUndoList || _currUndoList == null)
				{
					_undoSequence++;
					_currUndoList = new UndoList(_undoSequence);
					_undoStack.Push(_currUndoList);
					_createNewUndoList = false;
				}
				//Begin Enhancement - JScott - Add Balance Low Levels functionality

				//return _currUndoList;
				//End Enhancement - JScott - Add Balance Low Levels functionality
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets a ComputationCellReference locked status.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to set the lock status for.
		/// </param>
		/// <param name="aLock">
		/// The lock status to set.
		/// </param>

		virtual protected void intSetRecursiveCellLock(ComputationCellReference aCompCellRef, bool aLock)	// TT#3809 - stodd - Locked Cell doesn't save when processing Need
		{
			ArrayList detailCellRefList;

			try
			{
				aCompCellRef.SetCellLock(aLock);

				detailCellRefList = aCompCellRef.GetComponentDetailCellRefArray(false);

				foreach (ComputationCellReference compCellRef in detailCellRefList)
				{
					intSetRecursiveCellLock(compCellRef, aLock);
				}

				intQueueTotalCellLockSum(aCompCellRef);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Adds an ComputationCellReference to the queue for Lock summing.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to queue for Lock summing.
		/// </param>

		protected void intQueueTotalCellLockSum(ComputationCellReference aCompCellRef)
		{
			ArrayList totalCellRefList;
			ComputationCellReference totCellRef;

			try
			{
				totalCellRefList = aCompCellRef.GetTotalCellRefArray();

				foreach (ComputationCellReference totalCellRef in totalCellRefList)
				{
					foreach (QuantityVariableProfile quanVar in totalCellRef.ComputationCube.QuantityVariableProfileList)
					{
						totCellRef = (ComputationCellReference)totalCellRef.Copy();
						totCellRef[totCellRef.ComputationCube.QuantityVariableProfileType] = quanVar.Key;

						if (totCellRef.GetVariableScopeVariableProfile().VariableScope == eVariableScope.Static)
						{
							if (!LockRetotalHash.Contains(totCellRef))
							{
								LockRetotalHash[totCellRef] = null;
								LockRetotalQueue.Enqueue(totCellRef);
							}
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Processes the Lock summing Queue.
		/// </summary>

		protected void intProcessTotalCellLockSum()
		{
			ComputationCellReference compCellRef;

			try
			{
				while (LockRetotalQueue.Count > 0)
				{
					compCellRef = (ComputationCellReference)LockRetotalQueue.Dequeue();
					compCellRef.SumDetailCellLocks();
					intQueueTotalCellLockSum(compCellRef);
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
