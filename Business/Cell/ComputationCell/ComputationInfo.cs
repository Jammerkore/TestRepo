using System;

namespace MIDRetail.Business
{
	/// <summary>
	/// The ComputationInfo structure contains information about the Cell that is used during the Computation process.
	/// </summary>

	[Serializable]
	public class ComputationInfo
	{
		//=======
		// FIELDS
		//=======

		private ComputationInfoFlags _flags;
		private ComputationScheduleFormulaEntry _scheduledFormula;
		private ComputationScheduleSpreadEntry _scheduledSpread;
        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        //private ComputationScheduleEntry _postExecuteReInitScheduleEntry;
        //private ComputationScheduleEntry _postExecuteReInitTriggerScheduleEntry;
        //End TT#2 - JScott - Assortment Planning - Phase 2
        private System.Collections.ArrayList _scheduledSpreadsArray;
		private ComputationCell _previousValue;
		private bool _isExcludedFromSpread;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of the ComputationInfo class.
		/// </summary>

		public ComputationInfo()
		{
			try
			{
				_flags.Clear();
				_scheduledFormula = null;
				_scheduledSpread = null;
                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //_postExecuteReInitScheduleEntry = null;
                //_postExecuteReInitTriggerScheduleEntry = null;
                //End TT#2 - JScott - Assortment Planning - Phase 2
                _scheduledSpreadsArray = new System.Collections.ArrayList();
				_previousValue = null;
				_isExcludedFromSpread = false;
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
		/// Gets the flags.
		/// </summary>

		public ComputationInfoFlags Flags
		{
			get
			{
				return _flags;
			}
		}

		/// <summary>
		/// Gets or sets the CompChanged flag.  The CompChanged flag indicates if a Cell has already been changed by a previous computation.
		/// </summary>

		public bool isCompChanged
		{
			get
			{
				try
				{
					return _flags.isCompChanged;
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
					_flags.isCompChanged = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the UserChanged flag.  The UserChanged flag indicates if the Cell was changed by the User.
		/// </summary>

		public bool isUserChanged
		{
			get
			{
				try
				{
					return _flags.isUserChanged;
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
					_flags.isUserChanged = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the CompLocked flag.  The CompLocked flag indicates if a Cell has already been changed by a previous computation.
		/// </summary>

		public bool isCompLocked
		{
			get
			{
				try
				{
					return _flags.isCompLocked;
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
					_flags.isCompLocked = value;
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
		/// Gets or sets the AutoTotalsProcessed flag.  The AutoTotalsProcessed flag indicates if a Cell has had the Autototals processed.
		/// </summary>

		public bool isAutoTotalsProcessed
		{
			get
			{
				try
				{
					return _flags.isAutoTotalsProcessed;
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
					_flags.isAutoTotalsProcessed = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//End Track #5752 - JScott - Calculation Time
		/// <summary>
		/// Gets or sets the ComputationScheduleEntry object for the Cell.
		/// </summary>

		public ComputationScheduleFormulaEntry ScheduledFormula
		{
			get
			{
				return _scheduledFormula;
			}
			set
			{
				_scheduledFormula = value;
			}
		}

		/// <summary>
		/// Gets or sets the ComputationScheduleEntry object for the Cell.
		/// </summary>

		public ComputationScheduleSpreadEntry ScheduledSpread
		{
			get
			{
				return _scheduledSpread;
			}
			set
			{
				_scheduledSpread = value;
			}
		}

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        ///// <summary>
        ///// Gets or sets the ComputationScheduleEntry object for the Cell.
        ///// </summary>

        //public ComputationScheduleEntry PostExecuteReInitScheduleEntry
        //{
        //    get
        //    {
        //        return _postExecuteReInitScheduleEntry;
        //    }
        //    set
        //    {
        //        _postExecuteReInitScheduleEntry = value;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the ComputationScheduleEntry object for the Cell.
        ///// </summary>

        //public ComputationScheduleEntry PostExecuteReInitTriggerScheduleEntry
        //{
        //    set
        //    {
        //        _postExecuteReInitTriggerScheduleEntry = value;
        //    }
        //}

        //End TT#2 - JScott - Assortment Planning - Phase 2
        /// <summary>
		/// Gets the ArrayList containing the spreads scheduled for this Cell.  A Cell can have multiple spreads scheduled from it.
		/// </summary>

		public System.Collections.ArrayList ScheduledSpreadsArray
		{
			get
			{
				return _scheduledSpreadsArray;
			}
		}

		/// <summary>
		/// Gets or sets the previous value for this cell.
		/// </summary>

		public ComputationCell PreviousValue
		{
			get
			{
				return _previousValue;
			}
			set
			{
				_previousValue = value;
			}
		}

		/// <summary>
		/// Gets or sets the ExcludedFromSpread flag for this cell.
		/// </summary>

		public bool isExcludedFromSpread
		{
			get
			{
				return _isExcludedFromSpread;
			}
			set
			{
				_isExcludedFromSpread = value;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Gets a boolean indicator that determines if a formula is scheduled for the Cell.
		/// </summary>

		public bool isFormulaPending(ComputationScheduleEntry aScheduleEntry)
		{
            //Begin TT#2 - JScott - Assortment Planning - Phase 2
            //return (_scheduledFormula != null || (aScheduleEntry != null && _postExecuteReInitTriggerScheduleEntry != null && _postExecuteReInitTriggerScheduleEntry != aScheduleEntry));
            return _scheduledFormula != null;
            //End TT#2 - JScott - Assortment Planning - Phase 2
        }

		/// <summary>
		/// Gets a boolean indicator that determines if a formula is scheduled for the Cell.
		/// </summary>

		public bool isSpreadPending(ComputationScheduleEntry aScheduleEntry)
		{
			try
			{
				return _scheduledSpread != null && !_scheduledSpread.ComputationCellRef.Equals(aScheduleEntry.ComputationCellRef);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Clears the scheduled formula for this Cell.
		/// </summary>

		public void ClearScheduledFormula()
		{
			_scheduledFormula = null;
		}

		/// <summary>
		/// Clears the scheduled formula for this Cell.
		/// </summary>

		public void ClearScheduledSpread()
		{
			_scheduledSpread = null;
		}

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        ///// <summary>
        ///// Clears the scheduled formula for this Cell.
        ///// </summary>

        //public void ClearPostExecuteReInitScheduledFormula()
        //{
        //    _postExecuteReInitScheduleEntry = null;
        //    _postExecuteReInitTriggerScheduleEntry = null;
        //}
        //End TT#2 - JScott - Assortment Planning - Phase 2
    }
}
