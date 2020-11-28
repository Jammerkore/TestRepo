using System;

namespace MIDRetail.Business
{
	static public class ComputationCellFlagValues
	{
		//=======
		// FIELDS
		//=======

		private const ushort None = 0x0000;
		private const ushort Initialized = 0x0001; // Cell has been through the initialization process.  Does not indicate a change in value actually ocurred.
		private const ushort CurrentInitialized = 0x0002; // Cell has been through the initialization process for the current value.  Does not indicate a change in value actually ocurred.
		private const ushort LoadedFromDB = 0x0004; // Cell has been through the DB read process.  Does not indicate an value was found.
		private const ushort Null = 0x0008; // Cell is null (does not exist on cube).
		private const ushort ReadOnly = 0x0010; // Cell is marked as read-only.  Value will not be saved to DB.
		private const ushort DisplayOnly = 0x0020; // Cell is marked as display-only.  Value can not be changed on screen.
		private const ushort Changed = 0x0040; // Cell has been changed from its original DB value (by an init or a user change).
		private const ushort Locked = 0x0080; // Cell has been locked by the user.
		private const ushort HiddenInitialized = 0x0100; // Cell's hidden flag has been initialized.
		private const ushort Hidden = 0x0200; // Cell's value is hidden and will not be displayed on the screen.  At this time, the only hidden values are Basis values that are beyond the selected Basis timeline.
		private const ushort ExtensionCreated = 0x0400; // Cell has had an ExtensionCell created for it.
		public const ushort BaseStartingFlag = 0x0800;

		//========
		// METHODS
		//========

		/// <summary>
		/// Gets the value of the Initialized flag.
		/// </summary>

		static public bool isInitialized(ComputationCellFlags aFlags)
		{
			try
			{
				return ((aFlags.Flags & Initialized) > 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the value of the Initialized flag.
		/// </summary>

		static public void isInitialized(ref ComputationCellFlags aFlags, bool aValue)
		{
			try
			{
				if (aValue)
				{
					aFlags.Flags = (ushort)(aFlags.Flags | Initialized);
				}
				else
				{
					aFlags.Flags = (ushort)(aFlags.Flags & ~Initialized);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the value of the CurrentInitialized flag.
		/// </summary>

		static public bool isCurrentInitialized(ComputationCellFlags aFlags)
		{
			try
			{
				return ((aFlags.Flags & CurrentInitialized) > 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the value of the CurrentInitialized flag.
		/// </summary>

		static public void isCurrentInitialized(ref ComputationCellFlags aFlags, bool aValue)
		{
			try
			{
				if (aValue)
				{
					aFlags.Flags = (ushort)(aFlags.Flags | CurrentInitialized);
				}
				else
				{
					aFlags.Flags = (ushort)(aFlags.Flags & ~CurrentInitialized);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the value of the LoadedFromDB flag.
		/// </summary>

		static public bool isLoadedFromDB(ComputationCellFlags aFlags)
		{
			try
			{
				return ((aFlags.Flags & LoadedFromDB) > 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the value of the LoadedFromDB flag.
		/// </summary>

		static public void isLoadedFromDB(ref ComputationCellFlags aFlags, bool aValue)
		{
			try
			{
				if (aValue)
				{
					aFlags.Flags = (ushort)(aFlags.Flags | LoadedFromDB);
				}
				else
				{
					aFlags.Flags = (ushort)(aFlags.Flags & ~LoadedFromDB);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the value of the Null flag.
		/// </summary>

		static public bool isNull(ComputationCellFlags aFlags)
		{
			try
			{
				return ((aFlags.Flags & Null) > 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the value of the Null flag.
		/// </summary>

		static public void isNull(ref ComputationCellFlags aFlags, bool aValue)
		{
			try
			{
				if (aValue)
				{
					aFlags.Flags = (ushort)(aFlags.Flags | Null);
				}
				else
				{
					aFlags.Flags = (ushort)(aFlags.Flags & ~Null);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the value of the ReadOnly flag.
		/// </summary>

		static public bool isReadOnly(ComputationCellFlags aFlags)
		{
			try
			{
				return ((aFlags.Flags & ReadOnly) > 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the value of the ReadOnly flag.
		/// </summary>

		static public void isReadOnly(ref ComputationCellFlags aFlags, bool aValue)
		{
			try
			{
				if (aValue)
				{
					aFlags.Flags = (ushort)(aFlags.Flags | ReadOnly);
				}
				else
				{
					aFlags.Flags = (ushort)(aFlags.Flags & ~ReadOnly);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the value of the DisplayOnly flag.
		/// </summary>

		static public bool isDisplayOnly(ComputationCellFlags aFlags)
		{
			try
			{
				return ((aFlags.Flags & DisplayOnly) > 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the value of the DisplayOnly flag.
		/// </summary>

		static public void isDisplayOnly(ref ComputationCellFlags aFlags, bool aValue)
		{
			try
			{
				if (aValue)
				{
					aFlags.Flags = (ushort)(aFlags.Flags | DisplayOnly);
				}
				else
				{
					aFlags.Flags = (ushort)(aFlags.Flags & ~DisplayOnly);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the value of the Changed flag.
		/// </summary>

		static public bool isChanged(ComputationCellFlags aFlags)
		{
			try
			{
				return ((aFlags.Flags & Changed) > 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the value of the Changed flag.
		/// </summary>

		static public void isChanged(ref ComputationCellFlags aFlags, bool aValue)
		{
			try
			{
				if (aValue)
				{
					aFlags.Flags = (ushort)(aFlags.Flags | Changed);
				}
				else
				{
					aFlags.Flags = (ushort)(aFlags.Flags & ~Changed);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the value of the Locked flag.
		/// </summary>

		static public bool isLocked(ComputationCellFlags aFlags)
		{
			try
			{
				return ((aFlags.Flags & Locked) > 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the value of the Locked flag.
		/// </summary>

		static public void isLocked(ref ComputationCellFlags aFlags, bool aValue)
		{
			try
			{
				if (aValue)
				{
					aFlags.Flags = (ushort)(aFlags.Flags | Locked);
				}
				else
				{
					aFlags.Flags = (ushort)(aFlags.Flags & ~Locked);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the value of the HiddenInitialized flag.
		/// </summary>

		static public bool isHiddenInitialized(ComputationCellFlags aFlags)
		{
			try
			{
				return ((aFlags.Flags & HiddenInitialized) > 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the value of the HiddenInitialized flag.
		/// </summary>

		static public void isHiddenInitialized(ref ComputationCellFlags aFlags, bool aValue)
		{
			try
			{
				if (aValue)
				{
					aFlags.Flags = (ushort)(aFlags.Flags | HiddenInitialized);
				}
				else
				{
					aFlags.Flags = (ushort)(aFlags.Flags & ~HiddenInitialized);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the value of the Hidden flag.
		/// </summary>

		static public bool isHidden(ComputationCellFlags aFlags)
		{
			try
			{
				return ((aFlags.Flags & Hidden) > 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the value of the Hidden flag.
		/// </summary>

		static public void isHidden(ref ComputationCellFlags aFlags, bool aValue)
		{
			try
			{
				if (aValue)
				{
					aFlags.Flags = (ushort)(aFlags.Flags | Hidden);
				}
				else
				{
					aFlags.Flags = (ushort)(aFlags.Flags & ~Hidden);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the value of the ExtensionCreated flag.
		/// </summary>

		static public bool isExtensionCreated(ComputationCellFlags aFlags)
		{
			try
			{
				return ((aFlags.Flags & ExtensionCreated) > 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the value of the ExtensionCreated flag.
		/// </summary>

		static public void isExtensionCreated(ref ComputationCellFlags aFlags, bool aValue)
		{
			try
			{
				if (aValue)
				{
					aFlags.Flags = (ushort)(aFlags.Flags | ExtensionCreated);
				}
				else
				{
					aFlags.Flags = (ushort)(aFlags.Flags & ~ExtensionCreated);
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
	/// The ComputationCell class defines the abstract cell for a Computation.
	/// </summary>
	/// <remarks>
	/// ComputationCell contains additional information that is required by a Computation.  This information includes a 2-byte flag, and a reference
	/// to a ComputationInfo object that is created during the Computation process.
	/// </remarks>

	[Serializable]
	abstract public class ComputationCell : Cell
	{
		//=======
		// FIELDS
		//=======

		protected ComputationCellFlags _flags;
		protected double _value;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ComputationCell.
		/// </summary>

		public ComputationCell()
			: base()
		{
			try
			{
				_flags.Clear();
				_value = 0;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of ComputationCell and initializes it with the given flags and value.
		/// </summary>
		/// <param name="aFlags">
		/// The flags to initialize the ComputationCell to.
		/// </param>

		public ComputationCell(ComputationCellFlags aFlags, double aValue)
			: base()
		{
			_flags = aFlags;
			_value = aValue;
		}

		/// <summary>
		/// Creates a new instance of ComputationCell and initializes it with a copy of the given ComputationCell.
		/// </summary>
        /// <param name="aCompCell">
		/// The ComputationCell to copy from.
		/// </param>

		protected ComputationCell(ComputationCell aCompCell)
			: base()
		{
			try
			{
				CopyFrom(aCompCell);
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
		/// Returns a boolean indicating if the Cell had a value.
		/// </summary>

		abstract public bool isCellHasNoValue { get; }

		/// <summary>
		/// Returns a boolean indicating if the Cell can be entered by the User.
		/// </summary>

		abstract public bool isCellAvailableForEntry { get; }

		/// <summary>
		/// Returns a boolean indicating if the Cell can be initialize for the current value.
		/// </summary>

		abstract public bool isCellAvailableForCurrentInitialization { get; }

		/// <summary>
		/// Returns a boolean indicating if the Cell can be updated by a computation.
		/// </summary>

		abstract public bool isCellAvailableForComputation { get; }

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        ///// <summary>
        ///// Returns a boolean indicating if the Cell can be updated by a forced ReInit.
        ///// </summary>

        //abstract public bool isCellAvailableForForcedReInit { get; }

        //End TT#2 - JScott - Assortment Planning - Phase 2
        /// <summary>
		/// Returns a boolean indicating if the Cell can be updated by a copy.
		/// </summary>

		abstract public bool isCellAvailableForCopy { get; }

		/// <summary>
		/// Returns a boolean indicating if the Cell can be locked by the User.
		/// </summary>

		abstract public bool isCellAvailableForLocking { get; }

		/// <summary>
		/// Gets the flags.
		/// </summary>

		public ComputationCellFlags Flags
		{
			get
			{
				return _flags;
			}
		}

		/// <summary>
		/// Gets or sets the value of the Initialized flag.
		/// </summary>

		public bool isInitialized
		{
			get
			{
				try
				{
					return ComputationCellFlagValues.isInitialized(_flags);
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
					ComputationCellFlagValues.isInitialized(ref _flags, value);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the CurrentInitialized flag.
		/// </summary>

		public bool isCurrentInitialized
		{
			get
			{
				try
				{
					return ComputationCellFlagValues.isCurrentInitialized(_flags);
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
					ComputationCellFlagValues.isCurrentInitialized(ref _flags, value);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the LoadedFromDB flag.
		/// </summary>

		public bool isLoadedFromDB
		{
			get
			{
				try
				{
					return ComputationCellFlagValues.isLoadedFromDB(_flags);
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
					ComputationCellFlagValues.isLoadedFromDB(ref _flags, value);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the Null flag.
		/// </summary>

		public bool isNull
		{
			get
			{
				try
				{
					return ComputationCellFlagValues.isNull(_flags);
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
					ComputationCellFlagValues.isNull(ref _flags, value);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the ReadOnly flag.
		/// </summary>

		public bool isReadOnly
		{
			get
			{
				try
				{
					return ComputationCellFlagValues.isReadOnly(_flags);
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
					ComputationCellFlagValues.isReadOnly(ref _flags, value);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the DisplayOnly flag.
		/// </summary>

		public bool isDisplayOnly
		{
			get
			{
				try
				{
					return ComputationCellFlagValues.isDisplayOnly(_flags);
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
					ComputationCellFlagValues.isDisplayOnly(ref _flags, value);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the Changed flag.
		/// </summary>

		public bool isChanged
		{
			get
			{
				try
				{
					return ComputationCellFlagValues.isChanged(_flags);
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
					ComputationCellFlagValues.isChanged(ref _flags, value);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the Locked flag.
		/// </summary>

		public bool isLocked
		{
			get
			{
				try
				{
					return ComputationCellFlagValues.isLocked(_flags);
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
					ComputationCellFlagValues.isLocked(ref _flags, value);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the HiddenInitialized flag.
		/// </summary>

		public bool isHiddenInitialized
		{
			get
			{
				try
				{
					return ComputationCellFlagValues.isHiddenInitialized(_flags);
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
					ComputationCellFlagValues.isHiddenInitialized(ref _flags, value);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the Hidden flag.
		/// </summary>

		public bool isHidden
		{
			get
			{
				try
				{
					return ComputationCellFlagValues.isHidden(_flags);
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
					ComputationCellFlagValues.isHidden(ref _flags, value);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the ExtensionCreated flag.
		/// </summary>

		public bool isExtensionCreated
		{
			get
			{
				try
				{
					return ComputationCellFlagValues.isExtensionCreated(_flags);
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
					ComputationCellFlagValues.isExtensionCreated(ref _flags, value);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the numeric cell value.
		/// </summary>

		public double Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Override.  Method defining the Clone functionality.  Clone creates a shallow copy of the object.
		/// </summary>
		/// <returns>
		/// Object reference to cloned object.
		/// </returns>

		override public Cell Clone()
		{
			try
			{
				return Copy();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Override.  Method that copies values from a ComputationCell object to the current object.
		/// </summary>
		/// <param name="aCell">
		/// The ComputationCell object to copy from.
		/// </param>

		override public void CopyFrom(Cell aCell)
		{
			try
			{
				_flags = ((ComputationCell)aCell)._flags;
				_value = ((ComputationCell)aCell)._value;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Override.  Defines the method to clear the contents of the ComputationCell.
		/// </summary>

		override public void Clear()
		{
			try
			{
				_flags.Clear();
				_value = 0;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
