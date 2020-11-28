using System;

namespace MIDRetail.Business
{
	static public class AssortmentCellFlagValues
	{
		//=======
		// FIELDS
		//=======

		private const ushort Blocked =	ComputationCellFlagValues.BaseStartingFlag;			// Cell has been blocked by the user.
		private const ushort Fixed	 =	ComputationCellFlagValues.BaseStartingFlag << 1;	// Cell is fixed and can not be changed.
        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        private const ushort Reinit  =  ComputationCellFlagValues.BaseStartingFlag << 2;	// Cell is fixed and can not be changed.
        //End TT#2 - JScott - Assortment Planning - Phase 2

		//========
		// METHODS
		//========

		/// <summary>
		/// Gets the value of the Blocked flag.
		/// </summary>

		static public bool isBlocked(ComputationCellFlags aFlags)
		{
			try
			{
				return ((aFlags.Flags & Blocked) > 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the value of the Blocked flag.
		/// </summary>

		static public void isBlocked(ref ComputationCellFlags aFlags, bool aValue)
		{
			try
			{
				if (aValue)
				{
					aFlags.Flags = (ushort)(aFlags.Flags | Blocked);
				}
				else
				{
					aFlags.Flags = (ushort)(aFlags.Flags & ~Blocked);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the value of the Fixed flag.
		/// </summary>

		static public bool isFixed(ComputationCellFlags aFlags)
		{
			try
			{
				return ((aFlags.Flags & Fixed) > 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the value of the Fixed flag.
		/// </summary>

		static public void isFixed(ref ComputationCellFlags aFlags, bool aValue)
		{
			try
			{
				if (aValue)
				{
					aFlags.Flags = (ushort)(aFlags.Flags | Fixed);
				}
				else
				{
					aFlags.Flags = (ushort)(aFlags.Flags & ~Fixed);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
        //Begin TT#2 - JScott - Assortment Planning - Phase 2

        /// <summary>
        /// Gets the value of the Reinit flag.
        /// </summary>

        static public bool isReinit(ComputationCellFlags aFlags)
        {
            try
            {
                return ((aFlags.Flags & Reinit) > 0);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// Sets the value of the Reinit flag.
        /// </summary>

        static public void isReinit(ref ComputationCellFlags aFlags, bool aValue)
        {
            try
            {
                if (aValue)
                {
                    aFlags.Flags = (ushort)(aFlags.Flags | Reinit);
                }
                else
                {
                    aFlags.Flags = (ushort)(aFlags.Flags & ~Reinit);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        //End TT#2 - JScott - Assortment Planning - Phase 2
    }

	/// <summary>
	/// The AssortmentCell class defines the abstract cell for an Assortment.
	/// </summary>
	/// <remarks>
	/// AssortmentCell contains additional information that is required by an Assortment.  This information includes a 2-byte flag, and a reference
	/// to a ComputationInfo object that is created during the Computation process.
	/// </remarks>

	[Serializable]
	public class AssortmentCell : ComputationCell
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AssortmentCell.
		/// </summary>

		public AssortmentCell()
			: base()
		{
		}

		/// <summary>
		/// Creates a new instance of AssortmentCell and initializes it with the given flags and value.
		/// </summary>
		/// <param name="aFlags">
		/// The flags to initialize the AssortmentCell to.
		/// </param>

		public AssortmentCell(ComputationCellFlags aFlags, double aValue)
			: base(aFlags, aValue)
		{
		}

		/// <summary>
		/// Creates a new instance of AssortmentCell and initializes it with a copy of the given AssortmentCell.
		/// </summary>
		/// <param name="aAssortmentCell">
		/// The AssortmentCell to copy from.
		/// </param>

		protected AssortmentCell(AssortmentCell aAssortmentCell)
			: base(aAssortmentCell)
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
				return
					isNull ||
					isBlocked;
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
					!isBlocked &&
					!isLocked &&
					!isFixed &&
					!isHidden;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the Cell can be initialize for the current value.
		/// </summary>

		override public bool isCellAvailableForCurrentInitialization
		{
			get
			{
				return !isReadOnly;
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
					!isLocked &&
					!isFixed;
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
        //            !isFixed;
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
				return !isReadOnly;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the Cell can be locked by the User.
		/// </summary>

		override public bool isCellAvailableForLocking
		{
			get
			{
				return
					!isDisplayOnly &&
					!isNull &&
					!isBlocked &&
					//!isFixed &&	// TT#3750 - stodd - "Total %" not locking
					!isHidden &&
					!isReadOnly;
			}
		}

		/// <summary>
		/// Gets or sets the value of the Blocked flag.
		/// </summary>

		public bool isBlocked
		{
			get
			{
				try
				{
					return AssortmentCellFlagValues.isBlocked(_flags);
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
					AssortmentCellFlagValues.isBlocked(ref _flags, value);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the Fixed flag.
		/// </summary>

		public bool isFixed
		{
			get
			{
				try
				{
					return AssortmentCellFlagValues.isFixed(_flags);
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
					AssortmentCellFlagValues.isFixed(ref _flags, value);
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
        /// Gets or sets the value of the Reinit flag.
        /// </summary>

        public bool isReinit
        {
            get
            {
                try
                {
                    return AssortmentCellFlagValues.isReinit(_flags);
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
                    AssortmentCellFlagValues.isReinit(ref _flags, value);
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
		/// Override.  Method defining the Copy functionality.  Copy creates a deep copy. 
		/// </summary>
		/// <returns>
		/// Object reference to the copied object.
		/// </returns>

		override public Cell Copy()
		{
			AssortmentCell assrtCell;

			try
			{
				assrtCell = new AssortmentCell();
				assrtCell.CopyFrom(this);

				return assrtCell;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
