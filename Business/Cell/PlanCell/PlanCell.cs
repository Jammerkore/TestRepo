using System;

namespace MIDRetail.Business
{
	static public class PlanCellFlagValues
	{
		//=======
		// FIELDS
		//=======

		private const ushort Ineligible	= ComputationCellFlagValues.BaseStartingFlag;		// Cell's store is ineligible.
		private const ushort Protected	= ComputationCellFlagValues.BaseStartingFlag << 1;	// Cell is protected.
		private const ushort Closed		= ComputationCellFlagValues.BaseStartingFlag << 2;	// Cell's store is closed.

		//========
		// METHODS
		//========

		/// <summary>
		/// Gets the value of the Ineligible flag.
		/// </summary>

		static public bool isIneligible(ComputationCellFlags aFlags)
		{
			try
			{
				return ((aFlags.Flags & Ineligible) > 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the value of the Ineligible flag.
		/// </summary>

		static public void isIneligible(ref ComputationCellFlags aFlags, bool aValue)
		{
			try
			{
				if (aValue)
				{
					aFlags.Flags = (ushort)(aFlags.Flags | Ineligible);
				}
				else
				{
					aFlags.Flags = (ushort)(aFlags.Flags & ~Ineligible);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the value of the Protected flag.
		/// </summary>

		static public bool isProtected(ComputationCellFlags aFlags)
		{
			try
			{
				return ((aFlags.Flags & Protected) > 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the value of the Protected flag.
		/// </summary>

		static public void isProtected(ref ComputationCellFlags aFlags, bool aValue)
		{
			try
			{
				if (aValue)
				{
					aFlags.Flags = (ushort)(aFlags.Flags | Protected);
				}
				else
				{
					aFlags.Flags = (ushort)(aFlags.Flags & ~Protected);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the value of the Closed flag.
		/// </summary>

		static public bool isClosed(ComputationCellFlags aFlags)
		{
			try
			{
				return ((aFlags.Flags & Closed) > 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the value of the Closed flag.
		/// </summary>

		static public void isClosed(ref ComputationCellFlags aFlags, bool aValue)
		{
			try
			{
				if (aValue)
				{
					aFlags.Flags = (ushort)(aFlags.Flags | Closed);
				}
				else
				{
					aFlags.Flags = (ushort)(aFlags.Flags & ~Closed);
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
	/// The PlanCell class defines the abstract cell for a Plan.
	/// </summary>
	/// <remarks>
	/// PlanCell contains additional information that is required by a Plan.  This information includes a 2-byte flag, and a reference
	/// to a ComputationInfo object that is created during the Computation process.
	/// </remarks>

	[Serializable]
	public class PlanCell : ComputationCell
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of PlanCell.
		/// </summary>

		public PlanCell()
			: base()
		{
		}

		/// <summary>
		/// Creates a new instance of PlanCell and initializes it with the given flags and value.
		/// </summary>
		/// <param name="aFlags">
		/// The flags to initialize the PlanCell to.
		/// </param>

		public PlanCell(ComputationCellFlags aFlags, double aValue)
			: base(aFlags, aValue)
		{
		}

		/// <summary>
		/// Creates a new instance of PlanCell and initializes it with a copy of the given PlanCell.
		/// </summary>
		/// <param name="aPlanCell">
		/// The PlanCell to copy from.
		/// </param>

		protected PlanCell(PlanCell aPlanCell)
			: base(aPlanCell)
		{
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns a boolean indicating if the Cell had a value.
		/// </summary>

		override public bool isCellHasNoValue
		{
			get
			{
				return isNull;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the Cell can be entered by the User.
		/// </summary>

		override public bool isCellAvailableForEntry
		{
			get
			{
				return
					!isReadOnly &&
					!isNull &&
					!isLocked &&
					!isProtected &&
					!isClosed &&
					!isHidden &&
					!isIneligible;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the Cell can be initialize for the current value.
		/// </summary>

		override public bool isCellAvailableForCurrentInitialization
		{
			get
			{
				return
					!isReadOnly &&
					!isProtected;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the Cell can be updated by a computation.
		/// </summary>

		override public bool isCellAvailableForComputation
		{
			get
			{
				return
					!isReadOnly &&
					!isProtected &&
					!isLocked;
			}
		}

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        ///// <summary>
        ///// Returns a boolean indicating if the Cell can be updated by a forced ReInit.
        ///// </summary>

        //override public bool isCellAvailableForForcedReInit
        //{
        //    get
        //    {
        //        return
        //            !isReadOnly &&
        //            !isProtected;
        //    }
        //}

        //End TT#2 - JScott - Assortment Planning - Phase 2
        /// <summary>
		/// Returns a boolean indicating if the Cell can be updated by a copy.
		/// </summary>

		override public bool isCellAvailableForCopy
		{
			get
			{
				return
					!isReadOnly &&
					!isProtected;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the Cell can be locked by the User.
		/// </summary>

		override public bool isCellAvailableForLocking
		{
			get
			{
                // Begin Track #5542 - JSmith - Unable to lock cells
                //return isNull;
				// Begin TT#1318-MD -stodd - OTS Forecast REview Store Multi Level- Cascade Unlock Entire sheet does not unlock ineligible values for Reg Stock or Sales R/P.
				//return !isNull;
                return 
                    !isNull &&
                    !isIneligible;
				// End TT#1318-MD -stodd - OTS Forecast REview Store Multi Level- Cascade Unlock Entire sheet does not unlock ineligible values for Reg Stock or Sales R/P.
                // End Track #5542
			}
		}

		/// <summary>
		/// Gets or sets the value of the Ineligible flag.
		/// </summary>

		public bool isIneligible
		{
			get
			{
				try
				{
					return PlanCellFlagValues.isIneligible(_flags);
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
					PlanCellFlagValues.isIneligible(ref _flags, value);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the Protected flag.
		/// </summary>

		public bool isProtected
		{
			get
			{
				try
				{
					return PlanCellFlagValues.isProtected(_flags);
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
					PlanCellFlagValues.isProtected(ref _flags, value);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the Closed flag.
		/// </summary>

		public bool isClosed
		{
			get
			{
				try
				{
					return PlanCellFlagValues.isClosed(_flags);
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
					PlanCellFlagValues.isClosed(ref _flags, value);
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
		/// Override.  Method defining the Copy functionality.  Copy creates a deep copy. 
		/// </summary>
		/// <returns>
		/// Object reference to the copied object.
		/// </returns>

		override public Cell Copy()
		{
			PlanCell planCell;

			try
			{
				planCell = new PlanCell();
				planCell.CopyFrom(this);

				return planCell;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
