using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;  // TT#59 Implement Temp Locks
using System.Data;                 // TT#488 - MD - Jellis - Group Allocation
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;

namespace MIDRetail.Business.Allocation
{
	#region HdrAllocationBin
	/// <summary>
	/// Container for basic allocation tracking fields.
	/// </summary>
	public struct HdrAllocationBin
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private int _qtyToAllocate;
		private int _qtyAllocated;
        private int _itemQtyAllocated; // TT#1401 - JEllis - Urban Reservation Stores pt 1
        private int _origItemQtyAllocated; // TT#1401 - JEllis - Urban Reservation Stores pt 1
        //private int _imoQtyAllocated;  // TT#1401 - JEllis - Urban Reservation Stores pt 1
		private int _filterQtyAllocated;
		private int _unitMultiple;
		private int _sequence;
		private bool _isChanged;
		private bool _isNew;
		private int _storesWithAllocationCount; // MID Track 4448 AnF Audit Enhancement
		private bool _calcStoresWithAllocation; // MID Track 4448 AnF Audit Enhancement
		#endregion Fields

		#region Constructors
		//============
		// CONSTRUCTOR
		//============
		#endregion Constructors

		#region Properties
		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets or sets IsChanged database update flag
		/// </summary>
		internal bool IsChanged
		{
			get
			{
				return _isChanged;
			}
			set
			{
				_isChanged = value;
			}
		}

		/// <summary>
		/// Gets or sets IsNew database update flag
		/// </summary>
		internal bool IsNew
		{
			get 
			{
				return _isNew;
			}
			set
			{
				_isNew = value;
			}
		}

		/// <summary>
		/// Get or Set sequence
		/// </summary>
		public int Sequence
		{
			get 
			{
				return _sequence;
			}
			set
			{
				if (Sequence != value)
				{
					_sequence = value;
					this.IsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets QtyToAllocate
		/// </summary>
		public int QtyToAllocate
		{
			get
			{
				return _qtyToAllocate;
			}
		}

		/// <summary>
		/// Gets QtyAllocated
		/// </summary>
		public int QtyAllocated
		{
			get
			{
				return _qtyAllocated;
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets ItemQtyAllocated
        /// </summary>
        public int ItemQtyAllocated
        {
            get
            {
                return _itemQtyAllocated;
            }
        }
        /// <summary>
        /// Gets ItemQtyAllocated
        /// </summary>
        public int OrigItemQtyAllocated
        {
            get
            {
                return _origItemQtyAllocated;
            }
        }
        /// <summary>
        /// Gets ItemQtyAllocated
        /// </summary>
        public int ImoQtyAllocated
        {
            get
            {
                // begin TT#1401 - JEllis Urban Virtual Store Warehouse pt 29
                int imoQtyAllocated =
                    _qtyAllocated
                    - _itemQtyAllocated;
                if (imoQtyAllocated < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg) 
                        + ": ImoQtyAllocated in " + GetType().Name);
                }
                return imoQtyAllocated;
                //return _imoQtyAllocated;
                // end TT#1401 - Jellis - Urban Virtual Store Warehouse pt 29
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1

		/// <summary>
		/// Gets Filter QtyAllocated
		/// </summary>
		public int FilterQtyAllocated
		{
			get
			{
				return _filterQtyAllocated;
			}
		}

		/// <summary>
		/// Gets unit allocation multiple
		/// </summary>
		public int UnitMultiple
		{
			get
			{
				if (_unitMultiple <= 0)
				{
					return 1;
				}
				else
				{
					return _unitMultiple;
				}
			}
		}
		//begin MID Track 4448 AnF Audit Enhancement
		public int StoresWithAllocationCount
		{
			get
			{
				return _storesWithAllocationCount;
			}
			set
			{
				_storesWithAllocationCount = value;
			}
		}
		public bool CalcStoresWithAllocationCount
		{
			get
			{
				return this._calcStoresWithAllocation;
			}
			set
			{
				_calcStoresWithAllocation = value;
			}
		}
		// end MID Track 4448 AnF Audit Enhancement
		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========
		#region QuantityToAllocate
		/// <summary>
		/// Sets QtyToAllocate
		/// </summary>
		/// <param name="aQtyToAllocate">Quantity to allocate.</param>
		internal void SetQtyToAllocate(int aQtyToAllocate)
		{
			if (aQtyToAllocate < 0)
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_QtyToAllocateCannotBeNeg,
					MIDText.GetText(eMIDTextCode.msg_QtyToAllocateCannotBeNeg));
			}
			else
			{
				if (QtyToAllocate != aQtyToAllocate)
				{
					_qtyToAllocate = aQtyToAllocate;
					this.IsChanged = true;
				}
			}
		}
		#endregion QuantityToAllocate

		#region QuantityAllocated
		/// <summary>
		/// Sets Qty Allocated
		/// </summary>
		/// <param name="aQtyAllocated">Quantity allocated.</param>
		internal void SetQtyAllocated (int aQtyAllocated)
		{
			if (aQtyAllocated < 0)
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                        + ": SetQtyAllocated in " + GetType().Name);               // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
            }
			else
			{
				_qtyAllocated = aQtyAllocated;
			}
		}
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Sets Item Qty Allocated
        /// </summary>
        /// <param name="aQtyAllocated">Item Quantity allocated.</param>
        internal void SetItemQtyAllocated(int aQtyAllocated)
        {
            if (aQtyAllocated < 0)
            {
                throw new MIDException(eErrorLevel.severe,
                    (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                        + ": SetItemQtyAllocated in " + GetType().Name);               // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
            }
            else
            {
                _itemQtyAllocated = aQtyAllocated;
            }
        }

        // Begin TT#1401 - RMatelic - Reservation Stores >>> guessing at this method to be similar to _itemQtyAllocated method above 
        /// <summary>
        /// Sets Orig Item Qty Allocated
        /// </summary>
        /// <param name="aQtyAllocated">Orig Item Quantity allocated.</param>
        internal void SetOrigItemQtyAllocated(int aQtyAllocated)
        {
            if (aQtyAllocated < 0)
            {
                throw new MIDException(eErrorLevel.severe,
                    (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                        + ": SetOrigItemQtyAllocated in " + GetType().Name);               // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
            }
            else
            {
                _origItemQtyAllocated = aQtyAllocated;
            }
        }
        // End TT#1401  

        // begin TT#1401 - JEllis - Virtual Store Warehouse pt 29
        ///// <summary>
        ///// Sets IMO Qty Allocated
        ///// </summary>
        ///// <param name="aQtyAllocated">IMO Quantity allocated.</param>
        //internal void SetImoQtyAllocated(int aQtyAllocated)
        //{
        //    if (aQtyAllocated < 0)
        //    {
        //        throw new MIDException(eErrorLevel.severe,
        //            (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
        //                MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        //                + ": SetImoQtyAllocated in " + GetType().Name);               // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        //    }
        //    else
        //    {
        //        _imoQtyAllocated = aQtyAllocated;
        //    }
        //}
        // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1
		#endregion QuantityAllocated

		#region FilterQuantityAllocated
		/// <summary>
		/// Sets Filter QtyAllocated
		/// </summary>
		/// <param name="aQtyAllocated">Quantity Allocated.</param>
		internal void SetFilterQtyAllocated(int aQtyAllocated)
		{
			if (aQtyAllocated < 0)
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                        + ": SetFilterQtyAllocated in " + GetType().Name);               // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
            }
			else
			{
				_filterQtyAllocated = aQtyAllocated;
			}
		}
		#endregion FilterQuantityAllocated

		#region UnitMulitple
		/// <summary>
		/// Sets Unit Allocation Multiple
		/// </summary>
		/// <param name="aMultiple">Multiple value</param>
		internal void SetUnitMultiple(int aMultiple)
		{
			if (aMultiple < 1)
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_MultipleCannotBeLessThan1,
					MIDText.GetText(eMIDTextCode.msg_MultipleCannotBeLessThan1));
			}
			else
			{
				if (this.UnitMultiple != aMultiple)
				{
					_unitMultiple = aMultiple;
					this.IsChanged = true;
				}
			}
		}
		#endregion UnitMultiple
		#endregion Methods
	} 
	#endregion HdrAllocationBin

    // begin TT#586 - MD - Jellis - Add 2-dimensional Spread
    #region ConstraintBin
    public class ConstraintBin
    {
        private int _priorAllocated;
        private int _minimum;
        private int _maximum;
        private int _multiple;
        private bool _isOut;
        private bool _isLocked;

        /// <summary>
        /// Initializes a ConstraintBin
        /// </summary>
        public ConstraintBin()
        {
            _priorAllocated = 0;
            _minimum = 0;
            _maximum = int.MaxValue;
            _multiple = 1;
            _isOut = false;
            _isLocked = false;
        }
        /// <summary>
        /// Initializes a ConstraintBin
        /// </summary>
        /// <param name="aPriorAllocated">Units already allocated</param>
        /// <param name="aMin">Minimum Allocation Value</param>
        /// <param name="aMax">Maximum Allocation Value</param>
        /// <param name="aMult">Allocation Multiple</param>
        /// <param name="isOut">True: "out" contraint is active--in this case, aPriorAllocated will be set to zero; False: "out" constraint is not active</param>
        /// <param name="isLocked">True: aPriorAllocated is the locked as the allocation; False: aPriorAllocated is the starting allocation</param>
        /// <remarks>"isLocked" true forces the allocation to be the PriorAllocated units; "isOut" true forces a zero allocation: "isOut" will win over "isLock" when both are true.</remarks>
        public ConstraintBin(uint aPriorAllocated, uint aMin, uint aMax, uint aMult, bool isOut, bool isLocked)
        {
            _minimum = (int)aMin;
            _maximum = (int)aMax;
            _multiple = (int)aMult;
            _isOut = isOut;
            if (_isOut)
            {
                _priorAllocated = 0;
            }
            else
            {
                _priorAllocated = (int)aPriorAllocated;
            }
            _isLocked = isLocked;
        }
        /// <summary>
        /// Gets Prior Allocated
        /// </summary>
        public int PriorAllocated
        {
            get { return _priorAllocated; }
        }
        /// <summary>
        /// Gets the Minimum
        /// </summary>
        public int Minimum
        {
            get { return _minimum; }
        }
        /// <summary>
        /// Gets the Maximum
        /// </summary>
        public int Maximum
        {
            get { return _maximum; }
        }
        /// <summary>
        /// Gets the Multiple
        /// </summary>
        public int Multiple
        {
            get { return _multiple; }
        }
        /// <summary>
        /// Gets the Out constraint
        /// </summary>
        public bool IsOut
        {
            get { return _isOut; }
        }
        public bool IsLocked
        {
            get { return _isLocked; }
        }
    }
    #endregion ConstraintBin
    // end TT#586 - MD - Jellis - Add 2-dimensional Spread

	#region MinMaxAllocationBin
	/// <summary>
	/// Container for allocation minimum and maximum quantities
	/// </summary>
	public struct MinMaxAllocationBin
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private int _minimum;
		private int _maximum;
        private int _shipUpTo;       // TT#617 - RMatelic - Allocation Override - Add Ship Up To Rule (#36, #39)
		#endregion Fields

		#region Properties
		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets the minimum allocation quantity.
		/// </summary>
		public int Minimum
		{
			get
			{
				if (_minimum < 0)
				{
					return 0;
				}
				else
				{
					return _minimum;
				}
			}
		}

		/// <summary>
		/// Gets the largest possible maximum.
		/// </summary>
		public int LargestMaximum
		{
			get
			{
				return int.MaxValue;
			}
		}

		/// <summary>
		/// Gets the maximum allocation quantity.
		/// </summary>
		public int Maximum
		{
			get
			{
				if (_maximum < 0)
				{
                    return LargestMaximum; // TT#946 - MD - Jellis - Group Allocation Not Working
				}
				else
				{
					return _maximum;
				}
			}
		}

        // Begin TT#617 - RMatelic - Allocation Override - Add Ship Up To Rule (#36, #39)
        /// <summary>
        /// Gets the Ship Up To allocation quantity.
        /// </summary>
        public int ShipUpTo
        {
            get
            {
                if (_shipUpTo < 0)
                {
                    return 0;
                }
                else
                {
                    return _shipUpTo;
                }
            }
        }
        // End TT#617  
		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========
		#region Minimum
		/// <summary>
		/// Sets Minimum value
		/// </summary>	
		/// <param name="aMinimum">Minimum value.</param>
		internal void SetMinimum(int aMinimum)
		{
			if (aMinimum < 0)
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_MinAllocationCannotBeNeg,
					MIDText.GetText(eMIDTextCode.msg_MinAllocationCannotBeNeg));
			}
			else
			{
				_minimum = aMinimum;
			}
		}
		#endregion Minimum

		#region Maximum
		/// <summary>
		/// Sets Maximum allocation value.
		/// </summary>
		/// <param name="aMaximum">Maximum value.</param>
		internal void SetMaximum(int aMaximum)
		{
			if (aMaximum < 0)
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_MaxAllocationCannotBeNeg,
					MIDText.GetText(eMIDTextCode.msg_MaxAllocationCannotBeNeg));
			}
			else
			{
				_maximum = aMaximum;
			}
		}
		#endregion Maximum
        // Begin TT#617 - RMatelic - Allocation Override - Add Ship Up To Rule (#36, #39)
        #region ShipUpTo
        /// <summary>
        /// Sets ShipUpTo allocation value.
        /// </summary>
        /// <param name="aShipUpTo">ShipUpTo value.</param>
        internal void SetShipUpTo(int aShipUpTo)
        {
            if (aShipUpTo < 0)
            {
                _shipUpTo = 0;
            }
            else
            {
                _shipUpTo = aShipUpTo;
            }
        }
        #endregion ShipUpTo
        // End TT#617  
		#endregion Methods
	}
	#endregion MinMaxAllocationBin

	#region PackContentBin
	/// <summary>
	/// Container for Pack Content.
	/// </summary>
//Begin Track #4302 - JScott - Size Codes in wrong order afer release
//	public class PackContentBin
	public class PackContentBin : IComparable
//End Track #4302 - JScott - Size Codes in wrong order afer release
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private int _contentCodeRID;
		private int _origContentCodeRID;
		private int _units;
		private int _sequence;
		private bool _isChanged;
		private bool _isNew;
		#endregion Fields

		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Used to build an instance of this class.
		/// </summary>
		public PackContentBin()
		{
			_contentCodeRID = Include.NoRID;
			_origContentCodeRID = Include.NoRID;
			_units = 0;
			_sequence = 0;
			_isChanged = false;
			_isNew = false;
		}
		#endregion Constructors

		#region Properties
		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets contentCodeRID
		/// </summary>
		public int ContentCodeRID
		{
			get 
			{
				return _contentCodeRID;
			}
		}

		/// <summary>
		/// Gets Original contentCodeRID
		/// </summary>
		public int OriginalContentCodeRID
		{
			get 
			{
				return _origContentCodeRID;
			}
		}

		/// <summary>
		/// Gets ContentUnits
		/// </summary>
		public int ContentUnits
		{
			get
			{
				return _units;
			}
		}

		/// <summary>
		/// Get sequence
		/// </summary>
		public int Sequence
		{
			get 
			{
				return _sequence;
			}
			set
			{
				if (Sequence != value)
				{
					_sequence = value;
					this.ContentIsChanged = true;
				}
			}
		}

		/// <summary>
		/// Get or set Content Is Changed
		/// </summary>
		internal bool ContentIsChanged
		{
			get
			{
				return _isChanged;
			}
			set
			{
				_isChanged = value;
			}
		}

		/// <summary>
		/// Get or set Content Is New
		/// </summary>
		internal bool ContentIsNew
		{
			get
			{
				return _isNew;
			}
			set
			{
				_isNew = value;
			}
		}
		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========
//Begin Track #4302 - JScott - Size Codes in wrong order afer release
		#region IComparable Members
		public int CompareTo(object obj)
		{
			PackContentBin pcb;

			if (obj.GetType() == typeof(PackContentBin))
			{
				pcb = (PackContentBin)obj;
				if (this.Sequence == pcb.Sequence)
				{
					if (this.ContentCodeRID < pcb.ContentCodeRID)
					{
						return -1;
					}
					else
					{
						return 1;
					}
				}
				else if (this.Sequence < pcb.Sequence)
				{
					return -1;
				}
				else
				{
					return 1;
				}
			}
			else
			{
				return 1;
			}
		}
		#endregion

//End Track #4302 - JScott - Size Codes in wrong order afer release
		#region ContentCodeRID
		/// <summary>
		/// Sets ContentCodeRID
		/// </summary>
		/// <param name="aContentCodeRID">RID value for the color or size code</param>
        internal void SetContentCodeRID(int aContentCodeRID) // Assortment: Color/Size changes
		{
            if (this.ContentCodeRID != aContentCodeRID) // Assortment: Color/Size changes
			{
                _contentCodeRID = aContentCodeRID; // Assortment: Color/Size changes
				_isChanged = true;
			}
            if (_origContentCodeRID == Include.NoRID) // Assortment: Color/Size changes
			{
                _origContentCodeRID = ContentCodeRID; // Assortment: Color/Size changes
			}
		}
		#endregion ContentCodeRID

		#region ContentUnits
		/// <summary>
		/// Sets Content Units
		/// </summary>
		/// <param name="aUnits">Units value.</param>
		internal void SetContentUnits(int aUnits)
		{
			if (aUnits < 0)
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_UnitsInPackCannotBeNeg,
					MIDText.GetText(eMIDTextCode.msg_UnitsInPackCannotBeNeg));
			}
			if (this.ContentUnits != aUnits)
			{
				_units = aUnits;
				this.ContentIsChanged = true;
			}
		}
		#endregion ContentUnits

		#region ContentBinCompare
		/// <summary>
		/// Compares this content bin with another content bin to determine if they are equal.
		/// </summary>
		/// <param name="aContentBin">Content Bin to which this content bin is compared.</param>
		/// <returns>True if the content is the same; false if the content is different.</returns>
		public bool PackContentEqual(PackContentBin aContentBin)
		{
			if (this.ContentCodeRID == aContentBin.ContentCodeRID)
			{
				if (this.ContentUnits == aContentBin.ContentUnits)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
		#endregion ContentBinCompare
		#endregion Methods
	}
	#endregion PackContentBin

    // begin Assortment: Added PackSizeBin for color/size changes
    #region PackSizeBin
    public class PackSizeBin : PackContentBin, IComparable
    {
        #region Fields
        int _hdr_PCSZ_Key;
        int _sizeUnitsAllocated;	// TT#1667-MD - stodd - Fill Size for Pack --- received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
        #endregion Fields

        #region Constructor
        public PackSizeBin(int aHdrPCSZ_Key, int aSizeCodeRID, int aSizeUnits, int aSeq)
        {
            _hdr_PCSZ_Key = aHdrPCSZ_Key;
            base.SetContentCodeRID(aSizeCodeRID);
            base.SetContentUnits(aSizeUnits);
            base.Sequence = aSeq;
        }
        #endregion Constructor

        #region Properties
        public int HDR_PCSZ_KEY
        {
            get { return _hdr_PCSZ_Key; }
        }

		// Begin TT#1667-MD - stodd - Fill Size for Pack --- received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
        public int SizeUnitsAllocated
        {
            get { return _sizeUnitsAllocated; }
            set { _sizeUnitsAllocated = value; }
        }
		// End TT#1667-MD - stodd - Fill Size for Pack --- received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
        #endregion Properties

        #region Methods
        public int CompareTo(PackSizeBin psb)
        {
            return base.CompareTo(psb);
        }
        #endregion Methods
    }
    #endregion PackSizeBin
    // end Assortment: Added PackSizeBin for color/size changes
	#region PackColorSize
	/// <summary>
	/// Describes the color-size content of a pack.
	/// </summary>
//Begin Track #4302 - JScott - Size Codes in wrong order afer release
//	public class PackColorSize
	public class PackColorSize : IComparable
//End Track #4302 - JScott - Size Codes in wrong order afer release
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private PackContentBin _color;
		private int _totalSizeUnits;
		private Hashtable _size;
		// begin MID Track 3634 Add display Sequence to size
		private int _maxSizeSequence;
		private int _firstSizeSequence;
		// end MID Track 3634 Add display Sequence to size
        private int _hdrPCRID;            // Assortment
        private string _name;             // Assortment
        private string _description;      // Assortment
        private int _last_PCSZ_Key_Used;  // Assortment: color/size change
		#endregion Fields

		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Used to build an instance of this class
		/// </summary>
		public PackColorSize (int aHDR_PC_RID, int aColorCodeRID, int aColorUnitsInPack, int aColorSeqInPack)
		{
            _color = new PackContentBin();
            _totalSizeUnits = 0; 
            _hdrPCRID = aHDR_PC_RID;                     // Assortment
            this.SetColorCodeRID(aColorCodeRID);             // Assortment
            this.SetColorUnitsInPack(aColorUnitsInPack); // Assortment
            this.ColorSequenceInPack = aColorSeqInPack;   // Assortment
            this.ColorContentIsNew = true;               // Assortment

			// begin MID Track 3634 Add display Sequence to size
			_maxSizeSequence = 0;
			_firstSizeSequence = -1;
			// end MID Track 3634 Add display Sequence to size
            _name = null;               // Assortment
            _description = null;        // Assortment
            _last_PCSZ_Key_Used = 0;    // Assortment: color/size change
		}
		#endregion Constructors

		#region Properties
		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets the color content bin.
		/// </summary>
		public PackContentBin ColorBin
		{
			get
			{
				return _color;
			}
		}
		/// <summary>
		/// Gets or sets ColorCodeRID
		/// </summary>
		/// <remarks>
		/// ColorCodeRID is the Record ID on the database for this color.
		/// </remarks>
		public int ColorCodeRID
		{
			get
			{
				return _color.ContentCodeRID;
			}
		}

		/// <summary>
		/// Gets the original color key(as it existed on the database at create time).
		/// </summary>
		internal int OriginalColorCodeRID
		{
			get
			{
				return _color.OriginalContentCodeRID;
			}
		}

		/// <summary>
		/// Gets Color Units within Pack
		/// </summary>
		public int ColorUnitsInPack
		{
			get 
			{
				return _color.ContentUnits;
			}
		}

		/// <summary>
		/// Gets or sets Color Sequence within Pack
		/// </summary>
		public int ColorSequenceInPack
		{
			get 
			{
				return _color.Sequence;
			}
			set
			{
				_color.Sequence = value;
			}
		}

		/// <summary>
		/// Gets Total Size Units within the color in a pack.
		/// </summary>
		public int TotalSizeUnitsInPackColor
		{
			get
			{
				return _totalSizeUnits;
			}
		}
        /// <summary>
        /// Gets the HDR_PC_RID
        /// </summary>
        public int HdrPCRID
        {
            get
            {
                return _hdrPCRID;
            }
        }

        /// <summary>
        /// Gets or sets Color Name
        /// </summary>
        public string ColorName
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Gets or sets Color Description
        /// </summary>
        public string ColorDescription
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

		/// <summary>
		/// Gets the size content of the color.
		/// This property was added by DAT
		/// </summary>
		public Hashtable ColorSizes
		{
			get
			{
				if (_size == null)
				{
					CreatePackColorSizeHash();
				}
				return _size;
			}
		}


		/// <summary>
		/// Gets the number of sizes in a color of a pack
		/// </summary>
		public int SizeCountInColor
		{
			get
			{
				if (_size == null)
				{
					return 0;
				}
				else
				{
					return _size.Count;
				}
			}
		}

		/// <summary>
		/// Gets or sets color content is changed
		/// </summary>
		internal bool ColorContentIsChanged
		{
			get
			{
				return _color.ContentIsChanged;
			}
			set
			{
				_color.ContentIsChanged = value;
			}
		}


		/// <summary>
		/// Gets or sets color content is New
		/// </summary>
		internal bool ColorContentIsNew
		{
			get
			{
				return _color.ContentIsNew;
			}
			set
			{
				_color.ContentIsNew = value;
			}
		}
        public int Last_PCSZ_Key_Used
        {
            get
            {
                return _last_PCSZ_Key_Used;
            }
            set
            {
                if (value > -1)
                {
                    _last_PCSZ_Key_Used = value;
                }
            }
        }
		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========
        #region HdrPCRID
        /// <summary>
        /// Sets HdrPCRID
        /// </summary>
        /// <param name="aPCRID">database RID that identifies the pack color</param>
        internal void SetPackColorRID(int aPCRID)
        {
            if (HdrPCRID != aPCRID)
            {
                _hdrPCRID = aPCRID;
            }
        }
        #endregion HdrPCRID

//Begin Track #4302 - JScott - Size Codes in wrong order afer release
		#region IComparable Members
		public int CompareTo(object obj)
		{
			PackColorSize pcs;

			if (obj.GetType() == typeof(PackColorSize))
			{
				pcs = (PackColorSize)obj;
				if (this.ColorSequenceInPack == pcs.ColorSequenceInPack)
				{
					if (this.ColorCodeRID < pcs.ColorCodeRID)
					{
						return -1;
					}
					else
					{
						return 1;
					}
				}
				else if (this.ColorSequenceInPack < pcs.ColorSequenceInPack)
				{
					return -1;
				}
				else
				{
					return 1;
				}
			}
			else
			{
				return 1;
			}
		}
		#endregion

//End Track #4302 - JScott - Size Codes in wrong order afer release
		#region ColorCodeRID
		/// <summary>
		/// Sets color key
		/// </summary>
		/// <param name="aKey">Color RID.</param>
		internal void SetColorCodeRID(int aKey)
		{
			_color.SetContentCodeRID(aKey);
		}
		#endregion ColorCodeRID

		#region ColorUnits
		/// <summary>
		/// Sets Color Units in Pack
		/// </summary>
		/// <param name="aUnits"></param>
		internal void SetColorUnitsInPack(int aUnits)
		{
			_color.SetContentUnits(aUnits);
		}
		#endregion ColorUnits

		#region Size
		#region CreatePackColorSizeHash
		internal void CreatePackColorSizeHash()
		{
			_size = new Hashtable();
		}

		#endregion CreatePackColorSizeHash
		#region SizeIsInColor
		/// <summary>
		/// Indicates whether the given size is defined in the color.
		/// </summary>
		/// <param name="aSizeRID">Database RID for the width-size</param>
		/// <returns>True when size defined in the color.</returns>
		public bool SizeIsInColor(int aSizeRID)
		{
			if (SizeCountInColor == 0)
			{
				CreatePackColorSizeHash();
				return false;
			}
			else
			{
				return _size.Contains(aSizeRID);
			}
		}
		#endregion SizeIsInColor

		#region PackSizeBin
		/// <summary>
		/// Get Pack Size Bin for the specified size.
		/// </summary>
		/// <param name="aSizeRID">Database RID for the width-size.</param>
		/// <returns>A pack size bin that describes the size content of the pack-color</returns>
        public PackSizeBin GetSizeBin(int aSizeRID) // Assortment: added pack size bin
		{
			if (SizeIsInColor(aSizeRID))
			{
                return (PackSizeBin)_size[aSizeRID]; // Assortment: added pack size bin
			}
			else
			{
				throw new MIDException (eErrorLevel.warning,
					(int)eMIDTextCode.msg_SizeNotDefinedInPackColor,
					MIDText.GetText(eMIDTextCode.msg_SizeNotDefinedInPackColor));	
			}
		}
		#endregion PackSizeBin

		#region AddSizeToPackColor
		/// <summary>
		/// Adds size content to a color within a pack.
		/// </summary>
		/// <param name="aSizeRID">Database RID for this width-size on the database</param>
		/// <param name="aSizeUnits">Total units of this size within the color of the pack.</param>
        /// <returns>PCSZ Key within Pack color</returns>
		internal int AddSize(int aSizeRID, int aSizeUnits, int aSizeSequence) // Assortment: Return size key
		{
            _last_PCSZ_Key_Used++;
            return AddSize(_last_PCSZ_Key_Used, aSizeRID, aSizeUnits, aSizeSequence);  // Assortment: Return size key          
        }
        internal int AddSize(int aHDR_PCSZ_KEY, int aSizeRID, int aSizeUnits, int aSizeSequence) // Assortment: Color/size change
        {
            // note: "_content" replaced by "_packSizeBin" in this method.
            PackSizeBin _packSizeBin; // Assortment: added pack size bin
			if (SizeIsInColor(aSizeRID))
			{
				throw new MIDException(eErrorLevel.warning,
					(int)(eMIDTextCode.msg_al_DuplicateSizeInPackColorNotAllowed),
					MIDText.GetText(eMIDTextCode.msg_al_DuplicateSizeInPackColorNotAllowed));
			}
            _packSizeBin = new PackSizeBin(aHDR_PCSZ_KEY, aSizeRID, aSizeUnits, aSizeSequence); // Assortment: added pack size bin
            //_content.SetContentCodeRID(aSizeRID);  // Assortment: added pack size bin
            _size.Add(aSizeRID, _packSizeBin); // Assortment: added pack size bin
			//_content.SetContentUnits(aSizeUnits);
			// begin MID Track 3634 Add display sequence number to size
			if (aSizeSequence < 0
				|| aSizeSequence == this._firstSizeSequence)
			{
                _packSizeBin.Sequence = this._maxSizeSequence + 1; // Assortment: added pack size bin
			}
			else
			{
                _packSizeBin.Sequence = aSizeSequence; // Assortment: added pack size bin
			}
            if (this._maxSizeSequence < _packSizeBin.Sequence) // Assortment: added pack size bin
			{
                this._maxSizeSequence = _packSizeBin.Sequence; // Assortment: added pack size bin
			}
			if (_size.Count == 1)
			{
				this._firstSizeSequence = aSizeSequence;
			}
			// end MID Track 3634 Add display sequence number to size
            _packSizeBin.ContentIsNew = true; // Assortment: added pack size bin
			_totalSizeUnits += aSizeUnits;
            
            // Begin TT#81 - Ron Matelic - Database error adding sizes under certain conditions
            this.ColorContentIsChanged = true;
            // End TT#81
            
            return _packSizeBin.HDR_PCSZ_KEY; // Assortment: return size database key
		}
		#endregion AddSizeToPackColor

		#region RemoveSize
		/// <summary>
		/// Removes a size from the pack color.
		/// </summary> 
		/// <param name="aSizeRID">Database RID for the size</param>
		internal void RemoveSize(int aSizeRID)
		{
            // note:  "_content" replaced by "_packSizeBin" in this method
            PackSizeBin _packSizeBin; // Assortment: added pack size bin
			if (_size != null
				//&& !_size.Contains(aSizeRID))  //RonM: replaced with following line 
				&& _size.Contains(aSizeRID))
			{
                _packSizeBin = GetSizeBin(aSizeRID); // Assortment: added pack size bin
                this._totalSizeUnits -= _packSizeBin.ContentUnits; // Assortment: added pack size bin
				_size.Remove(aSizeRID);
			}
		}
		#endregion RemoveSize

		#region SizeRID
		/// <summary>
		/// Sets Size RID for a size within a Pack Color.
		/// </summary> 
		/// <param name="aOldSizeRID">Database RID for the old size</param>
		/// <param name="aNewSizeRID">Database RID for the new size</param>
		internal void SetSizeRID(int aOldSizeRID, int aNewSizeRID)
		{
			if (!SizeIsInColor(aOldSizeRID))
			{
				throw new MIDException(eErrorLevel.warning,
					(int)(eMIDTextCode.msg_SizeNotDefinedInPackColor),
					MIDText.GetText(eMIDTextCode.msg_SizeNotDefinedInPackColor));
			}
			if (SizeIsInColor(aNewSizeRID))
			{
				throw new MIDException(eErrorLevel.warning,
					(int)(eMIDTextCode.msg_al_DuplicateSizeInPackColorNotAllowed),
					MIDText.GetText(eMIDTextCode.msg_al_DuplicateSizeInPackColorNotAllowed));
			}
            PackSizeBin _packSizeBin = GetSizeBin(aOldSizeRID); // Assortment: added pack size bin
            _packSizeBin.SetContentCodeRID(aNewSizeRID); // Assortment: added pack size bin
            _size.Add(aNewSizeRID, _packSizeBin); // Assortment: added pack size bin
			_size.Remove(aOldSizeRID);
		}

		/// <summary>
		/// Gets original size key as it existed at create time
		/// </summary>
		/// <param name="aSizeRID">RID of the size</param>
		/// <returns>RID of the original size</returns>
		internal int GetOriginalSizeKey(int aSizeRID)
		{
			return (this.GetSizeBin(aSizeRID)).OriginalContentCodeRID;
		}
		#endregion PackSizeRID

		#region PackColorSizeUnitsInPack
		//===============================//
		// Pack Color Size Units In Pack //
		//===============================//
		/// <summary>
		/// Gets number of units of the specified size contained in the pack color
		/// </summary>
		/// <param name="aSizeRID">Database RID for the size</param>
		/// <returns>Units of this size in the pack color</returns>
		public int GetSizeUnitsInColor (int aSizeRID)
		{
			return GetSizeUnitsInColor(GetSizeBin(aSizeRID));
		}
	
		/// <summary>
		/// Gets number of units of the specified size contained in the pack color
		/// </summary>
		/// <param name="aSize">PackSizeBin that describes pack size</param>
		/// <returns>Units of this size in the pack color</returns>
        internal int GetSizeUnitsInColor(PackSizeBin aSize) // Assortment: added pack size bin
		{
			return aSize.ContentUnits;
		}

		/// <summary>
		/// Sets Size Units in the pack color
		/// </summary>
		/// <param name="aSizeRID">RID of the size</param>
		/// <param name="aSizeUnits">Size Units in pack color</param>
		internal void SetSizeUnitsInColor(int aSizeRID, int aSizeUnits)
		{
            SetSizeUnitsInColor((PackSizeBin)_size[aSizeRID], aSizeUnits); // Assortment: added pack size bin
		}

		/// <summary>
		/// Sets Size Units in the pack color
		/// </summary>
		/// <param name="aSize">PackSizeBin that describes pack color size</param>
		/// <param name="aSizeUnits">Size Units in pack color</param>
        internal void SetSizeUnitsInColor(PackSizeBin aSize, int aSizeUnits) // Assortment: added pack size bin
		{
			int _newTotalSizeUnits = this.TotalSizeUnitsInPackColor
				+ aSizeUnits
				- aSize.ContentUnits;
			if (_newTotalSizeUnits < 0)
			{
				throw new MIDException (eErrorLevel.warning,
					(int)eMIDTextCode.msg_AccumBulkSizeUnitsToAllocateCannotBeNeg,
					MIDText.GetText(eMIDTextCode.msg_AccumBulkSizeUnitsToAllocateCannotBeNeg));
			}
			else
			{
				aSize.SetContentUnits(aSizeUnits);
				_totalSizeUnits = _newTotalSizeUnits;
			}
		}
		#endregion PackColorSizeUnitsInPack
		
		#region PackSizeSequence
		/// <summary>
		/// Gets Size Sequence in the pack
		/// </summary>
		/// <param name="aSizeRID">RID for the size</param>
		/// <returns>Size sequence within pack color.</returns>
		internal int GetPackColorSizeSequence(int aSizeRID)
		{
			return GetPackColorSizeSequence(GetSizeBin(aSizeRID));
		}

		/// <summary>
		/// Gets Size Sequence in the pack color
		/// </summary>
		/// <param name="aSize">PackSizeBin that describes the pack color size</param>
		/// <return>Size Sequence in pack color</return>
        internal int GetPackColorSizeSequence(PackSizeBin aSize) // Assortment: added pack size bin
		{
			return aSize.Sequence;
		}

		/// <summary>
		/// Sets Size Sequence in the pack color
		/// </summary>
		/// <param name="aSizeRID">RID for the size</param>
		/// <param name="aSizeSequence">Size Sequence in pack color</param>
		internal void SetPackColorSizeSequence(int aSizeRID, int aSizeSequence)
		{
			SetPackColorSizeSequence(GetSizeBin(aSizeRID), aSizeSequence);
		}

		/// <summary>
		/// Sets Size Sequence in the pack color
		/// </summary>
		/// <param name="aSize">PackSizeBin that describes the pack color size</param>
		/// <param name="aSizeSequence">Size Sequence in pack color</param>
        internal void SetPackColorSizeSequence(PackSizeBin aSize, int aSizeSequence) // Assortment: added pack size bin
		{
			if (aSizeSequence < 1) 
			{
				aSize.Sequence = _size.Count;
			}
			else
			{
				aSize.Sequence = aSizeSequence;
			}
		}
		#endregion PackSizeSequence

		#region PackColorCompare
		/// <summary>
		/// Compares this pack color to another pack color to determine if they are equal.
		/// </summary>
		/// <param name="aPackColor">PackColorSize to which to compare this pack color.</param>
		/// <returns>True if equal; false otherwise.</returns>
		public bool PackColorEqual (PackColorSize aPackColor)
		{
			if (this.ColorBin.PackContentEqual(aPackColor.ColorBin))
			{
                foreach (PackSizeBin psb in this.ColorSizes.Values) // Assortment: added pack size bin
				{
                    if (aPackColor.ColorSizes.Contains(psb.ContentCodeRID)) // Assortment: added pack size bin
					{
                        if (!(psb.PackContentEqual((PackSizeBin)aPackColor.ColorSizes[psb.ContentCodeRID]))) // Assortment: added pack size bin
						{
							return false;
						}
					}
					else
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
		#endregion PackColorCompare
		#endregion Size
		#endregion Methods
	}
	#endregion PackColorSize 

	#region CharacteristicsBin
	public class CharacteristicsBin
	{
		#region Fields
		//=======
		// FIELDS
		//=======

		private string _name;
		private string _value;
		private eStoreCharType _char_type;
		#endregion Fields

		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Used to construct an instance of this class.
		/// </summary>
		public CharacteristicsBin(string Char_Name, string Char_Value, eStoreCharType Char_Type) 
		{
			_name = Char_Name;
			_value = Char_Value;
			_char_type = Char_Type;
		}
		#endregion Constructors

		#region Properties
		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets PackName
		/// </summary>
		/// <remarks>
		/// User specified name for the pack on the allocation header
		/// </remarks>
		public string Name
		{
			get
			{
				return _name;
			}
		}

		public string Value
		{
			get 
			{
				return _value;
			}
		}

		public eStoreCharType DataType
		{
			get 
			{
				return _char_type;
			}
		}
		#endregion Properties
	}
	#endregion

	#region PackHdr	
	/// <summary>
	/// Describes an instance of a pack.
	/// </summary>
	public class PackHdr
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private string _packName;
        private int _hdrRID;  // TT#488 - MD - Jellis - Group Allocation
		private int _packRID;
		private int _masterOrSubordinatePackRID; // MID Track 4029 Re-Work MasterPO Logic
		private string _origPackName;
		private string _subtotalPackName;
        private string _assortmentPackName; // TT#488 - MD - Jellis - Group Allocation
		private HdrAllocationBin _pack;
		private bool _generic;
		private int _totalColorUnits;
		private int _reservePacks;
		private Hashtable _packColors;
		private StoreAllocationStructureStatus _sass; // MID Track 3994 Performance
		private StorePackAllocated[] _storePackAllocated;
        private bool[] _storeLocked;   // TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
        private bool[] _storeTempLock;          // TT#59 Implement Temp Lock
        private bool[] _storeItemTempLock;          // TT#1401 - JEllis - Urban Reservation Stores pt 2
        private int _associatedPackRID;
        private int _sequence;
        private List<int> _associatedPackRIDs = null;   // TT#1966-MD - JSmith - DC Fulfillment
        private string _associatedPackName;  // TT#2119-MD - JSmith - Cancelling allocation on subordinate headers on master runs forever if subordinates have same pack names with different size runs
        private bool _storeEligibilityLoaded = false;
        private int _planHnRID;
		#endregion Fields

		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Used to construct an instance of this class.
		/// </summary>
		/// <param name="aPackName"></param>
		internal PackHdr(string aPackName) 
		{
			_packName = aPackName;
            _hdrRID = Include.NoRID; // TT#488 - MD - Jellis - Group Allocation
			_packRID = Include.NoRID; // MID Track 4029 Re-Work MasterPO Logic
			_masterOrSubordinatePackRID = Include.NoRID; // MID Track 4029 Re-Work MasterPO Logic
			_origPackName = aPackName;
			_subtotalPackName = null;
            _assortmentPackName = null; // TT#488 -  MD - Jellis - Group Allocation
			_pack.SetQtyAllocated(0);
            _pack.SetItemQtyAllocated(0); // TT#1401 - JEllis - Urban Reservation Stores pt 1
            //_pack.SetImoQtyAllocated(0);  // TT#1401 - JEllis - Urban Reservation Stores pt 1  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
			_pack.SetQtyToAllocate(0);
			_pack.SetUnitMultiple(1);
			_pack.IsChanged = false;
			_pack.IsNew = false;
			_generic = false;
			_totalColorUnits = 0;
			_reservePacks = 0;
			_sass = new StoreAllocationStructureStatus(false);
			_pack.StoresWithAllocationCount = 0;           // MID Track 4448 AnF Audit Enhancement
			_pack.CalcStoresWithAllocationCount = false;  // MID Track 4448 AnF Audit Enhancement
		}
		#endregion Constructors

		#region Properties
		//===========
		// PROPERTIES
		//===========
		/// <summary>
        /// Gets _storeEligibilityLoaded flag
        /// </summary>
        internal bool StoreEligibilityLoaded
        {
            get
            {
                return _storeEligibilityLoaded;
            }
            set
            {
                _storeEligibilityLoaded = value;
            }
        }

        /// <summary>
		/// Gets or sets IsChanged database update flag
		/// </summary>
		internal bool PackIsChanged
		{
			get
			{
				return _pack.IsChanged;
			}
			set
			{
				_pack.IsChanged = value;
			}
		}

		/// <summary>
		/// Gets or sets IsNew database update flag
		/// </summary>
		internal bool PackIsNew
		{
			get 
			{
				return _pack.IsNew;
			}
			set
			{
				_pack.IsNew = value;
			}
		}

		/// <summary>
		/// Gets original pack name (as it existed at create time).
		/// </summary>
		internal string OriginalPackName
		{
			get
			{
				return _origPackName;
			}
		}

		/// <summary>
		/// Gets PackName
		/// </summary>
		/// <remarks>
		/// User specified name for the pack on the allocation header
		/// </remarks>
		public string PackName
		{
			get
			{
				return _packName;
			}
		}

		public int PackRID
		{
			get 
			{
				return _packRID;
			}
		}

        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets the HdrRID associated with this pack
        /// </summary>
        /// <remarks>Identifies the home Header for this pack</remarks>
        public int HdrRID
        {
            get
            {
                return _hdrRID;
            }
            set
            {
                _hdrRID = value;
            }
        }
        // end TT#488 - MD - Jellis - Group Allocation

		// begin MID Track 4029 ReWork MasterPO Logic
		public int MasterOrSubordinatePackRID
		{
			get
			{
				return _masterOrSubordinatePackRID;
			}
			set
			{
				_masterOrSubordinatePackRID = value;
			}
		}
		// end MID Track 4029 ReWork MasterPO Logic

		/// <summary>
		/// Gets SubtotalPackName
		/// </summary>
		/// <remarks>
		/// System generated name for the pack in subtotals.
		/// </remarks>
		public string SubtotalPackName
		{
			get
			{
				return _subtotalPackName;
			}
			set
			{
				_subtotalPackName = value;
			}
		}
        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets AssortmentPackName
        /// </summary>
        /// <remarks>Generated name for the pack when pack is in an assortment or group allocation</remarks>
        public string AssortmentPackName
        {
            get
            {
                return _assortmentPackName;
            }
            set
            {
                _assortmentPackName = value;
            }
        }
        // end TT#488 - MD - Jellis - Group Allocation

		/// <summary>
		/// Gets Pack Multiple
		/// </summary>
		public int PackMultiple
		{
			get
			{
				return _pack.UnitMultiple;
			}
		}

		/// <summary>
		/// Gets or sets the number of packs to allocate
		/// </summary>
		public int PacksToAllocate
		{
			get
			{
				return _pack.QtyToAllocate;
			}
		}

		/// <summary>
		/// Gets the number of packs allocated
		/// </summary>
		public int PacksAllocated
		{
			get
			{
				return _pack.QtyAllocated;
			}
		}

        //begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets the number of Item Packs allocated
        /// </summary>
        public int ItemPacksAllocated
        {
            get
            {
                return _pack.ItemQtyAllocated;
            }
        }
        /// <summary>
        /// Gets the number of IMO Packs allocated
        /// </summary>
        public int ImoPacksAllocated
        {
            get
            {
                return _pack.ImoQtyAllocated;
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1

		/// <summary>
		/// Gets the Total Units to Allocate for this pack
		/// </summary>
		public int UnitsToAllocate
		{
			get
			{
				return PacksToAllocate * PackMultiple;
			}
		}

		/// <summary>
		/// Gets the Total Units Allocated for this pack
		/// </summary>
		public int UnitsAllocated
		{
			get
			{
				return PacksAllocated * PackMultiple;
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets the Total Item Units Allocated for this pack
        /// </summary>
        public int ItemUnitsAllocated
        {
            get
            {
                return ItemPacksAllocated * PackMultiple;
            }
        }
        /// <summary>
        /// Gets the Total IMO Units Allocated for this pack
        /// </summary>
        public int ImoUnitsAllocated
        {
            get
            {
                return ImoPacksAllocated * PackMultiple;
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1

		/// <summary>
		/// Gets the number of color units across all colors in the pack
		/// </summary>
		public int TotalColorUnitsInPack
		{
			get
			{
				return _totalColorUnits;
			}
		}

		/// <summary>
		/// Gets the number of reserve packs.
		/// </summary>
		public int ReservePacks
		{
			get
			{
				return _reservePacks;
			}
		}

		/// <summary>
		/// Gets the pack type.
		/// </summary>
		/// <remarks>
		/// Pack type can be generic or non-generic. 
		/// <list type="bullet">
		/// <item>Generic packs are allocated proportionally as received when the total store allocation is broken out by pack, bulk and color.</item>
		/// <item>Non-generic packs are allocated based on content need when the total store allocation is broken out by pack, bulk and color.</item>
		/// </list>
		/// Any pack may contain color and/or size content. A non-generic pack must contain color and/or size content unless it is a work up pack buy in which case the system will calculate content.
		/// </remarks>>
		public bool GenericPack
		{
			get
			{
				return _generic;
			}
		}

		/// <summary>
		/// Gets the associated pack RID .
		/// </summary>
		public int AssociatedPackRID
		{
			get
			{
				return _associatedPackRID;
			}
		}

        // Begin TT#1966-MD - JSmith - DC Fulfillment
        public List<int> AssociatedPackRIDs
        {
            get
            {
                if (_associatedPackRIDs == null)
                {
                    _associatedPackRIDs = new List<int>();
                }
                return _associatedPackRIDs;
            }
        }

        // Begin TT#2119-MD - JSmith - Cancelling allocation on subordinate headers on master runs forever if subordinates have same pack names with different size runs
		/// <summary>
		/// Gets associated pack name.
		/// </summary>
        internal string AssociatedPackName
		{
			get
			{
                return _associatedPackName;
			}
		}
		// End TT#2119-MD - JSmith - Cancelling allocation on subordinate headers on master runs forever if subordinates have same pack names with different size runs
        // End TT#1966-MD - JSmith - DC Fulfillment

        /// <summary>
        /// Gets or sets the pack sequence
        /// </summary>
        public int Sequence
        {
            get 
            {
                return _sequence;
            }
            set 
            {
                _sequence = value;
            }
        }
		/// <summary>
		/// Gets the color content of the pack.
		/// This property was added by DAT
		/// </summary>
		public Hashtable PackColors
		{
			get
			{
				if (_packColors == null)
				{
					CreatePackColorHash();
				}
				return _packColors;
			}
		}

		/// <summary>
		/// Gets the number of colors in the pack.
		/// </summary>
		public int PackColorCount
		{
			get
			{
				if (_packColors == null)
				{
					return 0;
				}
				else
				{
					return _packColors.Count;
				}
			}
		}

		/// <summary>
		/// Gets the number of dimensions in the store allocation array.
		/// </summary>
		public int StoreDimensions
		{
			get 
			{
				return _storePackAllocated.Length;
			}
		}
		// begin MID Track 3994 Performance
		internal StoreAllocationStructureStatus AllocationStructureStatus
		{
			get
			{
				return this._sass;
			}
			set 
			{
				this._sass = value;
			}
		}
		// end MID Track 3994 Performance
		//begin MID Track 4448 AnF Audit Enhancement
		/// <summary>
		/// Gets Count of stores with an allocation
		/// </summary>
		public int StoresWithAllocationCount
		{
			get
			{
				if (_pack.CalcStoresWithAllocationCount)
				{
					_pack.StoresWithAllocationCount = 0;
					for (int i=0; i < this._storePackAllocated.Length; i++)
					{
						if (this.GetStorePacksAllocated(i) > 0)
						{
							_pack.StoresWithAllocationCount++;
						}
					}
					_pack.CalcStoresWithAllocationCount = false;
				}
				return _pack.StoresWithAllocationCount;
			}
		}
		// end MID Track 4448 AnF Audit Enhancement
		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========
		#region PackName
		/// <summary>
		/// Sets pack name.
		/// </summary>
		/// <param name="aNewPackName">Unique string identification for the pack.</param>
		internal void SetPackName(string aNewPackName)
		{
			if (this.PackName != aNewPackName)
			{
				_packName = aNewPackName;
				this.PackIsChanged = true;
			}
		}

		/// <summary>
		/// Sets subtotal pack name (the name used to track the pack in subtotals).
		/// </summary>
		/// <param name="aSubtotalPackName">An unique string identification for the subtotal pack name</param>
		internal void SetSubtotalPackName(string aSubtotalPackName)
		{
			_subtotalPackName = aSubtotalPackName;
		}

        // Begin TT#1966-MD - JSmith - DC Fulfillment
        /// <summary>
        /// Sets original pack name (the name used to track the pack in subtotals).
        /// </summary>
        /// <param name="aOriginalPackName">The original pack name</param>
        internal void SetOriginalPackName(string aOriginalPackName)
        {
            _origPackName = aOriginalPackName;
        }
		
        // Begin TT#2119-MD - JSmith - Cancelling allocation on subordinate headers on master runs forever if subordinates have same pack names with different size runs
		/// <summary>
        /// Sets associated pack name (the name used to track the pack in subtotals).
        /// </summary>
        /// <param name="aAssociatedPackName">The pack name in the associated header</param>
        internal void SetAssociatedPackName(string aAssociatedPackName)
        {
            _associatedPackName = aAssociatedPackName;
        }
		// End TT#2119-MD - JSmith - Cancelling allocation on subordinate headers on master runs forever if subordinates have same pack names with different size runs
        // End TT#1966-MD - JSmith - DC Fulfillment
		#endregion PackName

		#region PackRID
		/// <summary>
		/// Sets pack RID.
		/// </summary>
		/// <param name="aPackRID">RID for the pack.</param>
		internal void SetPackRID(int aPackRID)
		{
			if (this.PackRID != aPackRID)
			{
				_packRID = aPackRID;
				this.PackIsChanged = true;
			}
		}
		#endregion PackRID
 
		#region AssociatedPackRID
		/// <summary>
		/// Sets associated pack RID.
		/// </summary>
		/// <param name="aPackRID">RID for the pack.</param>
		internal void SetAssociatedPackRID(int aPackRID)
		{
			if (this.AssociatedPackRID != aPackRID)
			{
				_associatedPackRID = aPackRID;
				this.PackIsChanged = true;
			}
		}

        // Begin TT#1966-MD - JSmith - DC Fulfillment
        /// <summary>
        /// Sets associated pack RIDs if 1 to many relationship.
        /// </summary>
        /// <param name="aPackRID">RID for the pack.</param>
        internal void SetAssociatedPackRIDs(int aPackRID)
        {
            if (!AssociatedPackRIDs.Contains(aPackRID))
            {
                AssociatedPackRIDs.Add(aPackRID);
                this.PackIsChanged = true;
            }
        }
        // End TT#1966-MD - JSmith - DC Fulfillment
		#endregion AssociatedPackRID
 
		#region PacksToAllocate
		/// <summary>
		/// Sets the number of packs to allocate
		/// </summary>
		/// <param name="aPacksToAllocate">Number of packs to allocate</param>
		internal void SetPacksToAllocate(int aPacksToAllocate)
		{
			_pack.SetQtyToAllocate(aPacksToAllocate);
		}
		#endregion PacksToAllocate

		#region PacksAllocated
		/// <summary>
		/// Sets the number of packs allocated
		/// </summary>
		/// <param name="aPacksAllocated">Number of packs allocated</param>
		internal void SetPacksAllocated(int aPacksAllocated)
		{
			_pack.SetQtyAllocated(aPacksAllocated);
		}

        // begin TT#1401 - JEllis - Reservation Stores pt 1
        /// <summary>
        /// Sets the number of Item Packs allocated
        /// </summary>
        /// <param name="aPacksAllocated">Number of Item Packs allocated</param>
        internal void SetItemPacksAllocated(int aPacksAllocated)
        {
            _pack.SetItemQtyAllocated(aPacksAllocated);
        }


        // begin TT#1401 - JEllis - Virtual Store Warehouse pt 29
        ///// <summary>
        ///// Sets the number of IMO Packs allocated
        ///// </summary>
        ///// <param name="aPacksAllocated">Number of IMO Packs allocated</param>
        //internal void SetImoPacksAllocated(int aPacksAllocated)
        //{
        //    _pack.SetImoQtyAllocated(aPacksAllocated);
        //}
        // end TT#1401 - JEllis - Virtual Store Warehouse pt 29
        // end TT#1401 - JEllis - Reservation Stores pt 1
		#endregion PacksAllocated

		#region ReservePacks
		/// <summary>
		/// Sets reserve packs
		/// </summary>
		/// <param name="aReservePacks">Reserve pack quantity.</param>
		internal void SetReservePacks(int aReservePacks)
		{
			if (aReservePacks < 0)
			{
				throw new MIDException(eErrorLevel.warning,
					(int)eMIDTextCode.msg_ReserveQtyCannotBeNeg,
					MIDText.GetText(eMIDTextCode.msg_ReserveQtyCannotBeNeg));
			}
			if (this.ReservePacks != aReservePacks)
			{
				_reservePacks = aReservePacks;
				this.PackIsChanged = true;
			}
		}
		#endregion ReservePacks

		#region PackType
		/// <summary>
		/// Sets Generic Pack flag value.
		/// </summary>
		/// <param name="aGenericPack">Generic Pack flag value (true or false).</param>
		internal void SetGenericPack(bool aGenericPack)
		{
			if (this.GenericPack != aGenericPack)
			{
				_generic = aGenericPack;
				this.PackIsChanged = true;
			}
		}
		#endregion Pack Type

		#region PackMultiple
		/// <summary>
		/// Sets Pack Multiple
		/// </summary>
		/// <param name="aMultiple">A Multiple value.</param>
		internal void SetPackMultiple(int aMultiple)
		{
			_pack.SetUnitMultiple(aMultiple);
		}
		#endregion PackMultiple

		#region PackColor
		#region PackColorHash
		/// <summary>
		/// Creates Pack Color Hashtable
		/// </summary>
		internal void CreatePackColorHash()
		{
			_packColors = new Hashtable();
		}
		#endregion PackColorHash

		#region PackColorIsChanged
		internal bool PackColorIsChanged(int ColorRID)
		{
			if (ColorIsInPack(ColorRID))
			{
				return ((PackColorSize)(this.PackColors[ColorRID])).ColorContentIsChanged;
			}
			return false;
		}
		#endregion PackColorIsChanged

		#region PackColorIsNew
		internal bool PackColorIsNew(int ColorRID)
		{
			if (ColorIsInPack(ColorRID))
			{
				return ((PackColorSize)(this.PackColors[ColorRID])).ColorContentIsNew;
			}
			return false;
		}
		#endregion PackColorIsNew

		#region ColorIsInPack
		/// <summary>
		/// Indicates whether the specified color is in the pack.
		/// </summary>
		/// <param name="aColorCodeRID">Database RID for the color.</param>
		/// <returns></returns>
		public bool ColorIsInPack(int aColorCodeRID)
		{
			if (_packColors == null)
			{
				CreatePackColorHash();
				return false;
			}
			else
			{
				return _packColors.Contains(aColorCodeRID);
			}
		}
		#endregion ColorIsInPack

		#region GetColorBin
		/// <summary>
		/// Get Pack Color Bin for the specified color.
		/// </summary>
		/// <param name="aColorCodeRID">Database RID for the color.</param>
		/// <returns>A pack color bin that describes the color content of the pack-color</returns>
		public PackColorSize GetColorBin(int aColorCodeRID)
		{
			if (ColorIsInPack(aColorCodeRID))
			{
				return (PackColorSize)_packColors[aColorCodeRID];
			}
			throw new MIDException (eErrorLevel.warning,
				(int)eMIDTextCode.msg_ColorNotDefinedInPack,
				MIDText.GetText(eMIDTextCode.msg_ColorNotDefinedInPack));
		}
		#endregion GetColorBin

		#region AddColor
		/// <summary>
		/// Adds a color to the pack.
		/// </summary> 
		/// <param name="aColorCodeRID">Database RID for the color</param>
		/// <param name="aColorUnits">Total number of units of this color in this pack.</param>
		/// <remarks>If the color is already in the pack, the existing color units are overlayed.</remarks>
        internal PackColorSize AddColor(int aColorCodeRID, int aColorUnits, int aColorSequence) // Assortment: Added return
        // begin Assortment: Color/size change
        {
            return AddColor(-(this.PackColorCount + 1), aColorCodeRID, aColorUnits, aColorSequence);
        }
        /// <summary>
		/// Adds a color to the pack.
		/// </summary> 
		/// <param name="aColorCodeRID">Database RID for the color</param>
		/// <param name="aColorUnits">Total number of units of this color in this pack.</param>
		/// <remarks>If the color is already in the pack, the existing color units are overlayed.</remarks>
        internal PackColorSize AddColor(int aHDR_PC_RID, int aColorCodeRID, int aColorUnits, int aColorSequence) // Assortment: Added return
 		// end Assortment: Color/size change
        {
			PackColorSize _content;
			if (ColorIsInPack(aColorCodeRID))
			{
				throw new MIDException(eErrorLevel.warning,
					(int)(eMIDTextCode.msg_al_DuplicateColorInPackNotAllowed),
					MIDText.GetText(eMIDTextCode.msg_al_DuplicateColorInPackNotAllowed));
			}
            // begin Assortment: Changed signature for adding a color to a pack
			//_content = new PackColorSize();
			//_content.SetColorCodeRID(aColorCodeRID);
			//_packColors.Add(aColorCodeRID, _content);
			//_content.SetColorUnitsInPack(aColorUnits);
			//_content.ColorSequenceInPack = aColorSequence;
			//_content.ColorContentIsNew = true;
			//this._totalColorUnits += _content.ColorUnitsInPack;
            _content = new PackColorSize(aHDR_PC_RID, aColorCodeRID, aColorUnits, aColorSequence);
            _packColors.Add(aColorCodeRID, _content);
            this._totalColorUnits += _content.ColorUnitsInPack;
            return _content;
            // end Assortment: Changed signature for adding a color to a pack
		}
		#endregion AddColor

		#region RemoveColorFromPack
		/// <summary>
		/// Removes a color from the pack.
		/// </summary> 
		/// <param name="aColorCodeRID">Database RID for the color</param>
        // Begin TT#2039 - JSmith - Header error on "Reset" - Cannot remove color from pack when packs allocated
        //internal void RemoveColor(int aColorCodeRID)
        internal void RemoveColor(int aColorCodeRID, bool aRemovingPack)
        // End TT#2039
		{
			PackColorSize _content;
            // Begin TT#863 - RMatelic - Allocation Workspace-cannot add header, fields appear to be locked from entry
            //       unrelated to Test Track but found when testing
            //if (_packColors != null
            //    && !_packColors.Contains(aColorCodeRID))
            if (_packColors != null && _packColors.Contains(aColorCodeRID))
            // End TT#863
			{
                // Begin TT#2039 - JSmith - Header error on "Reset" - Cannot remove color from pack when packs allocated
                //if (this.PacksAllocated > 0)
                if (this.PacksAllocated > 0 &&
                    !aRemovingPack)
                // End TT#2039
				{
					throw new MIDException(eErrorLevel.severe,
						(int)(eMIDTextCode.msg_al_CannotRemovePackColorWhenPacksAloctd),
						MIDText.GetText(eMIDTextCode.msg_al_CannotRemovePackColorWhenPacksAloctd));
				}
				_content = GetColorBin(aColorCodeRID);
				this._totalColorUnits -= _content.ColorUnitsInPack;
				_packColors.Remove(aColorCodeRID);
			}
		}
		#endregion RemoveColorFromPack

		#region PackColorCodeRID
		/// <summary>
		/// Sets color key.
		/// </summary> 
		/// <param name="aOldColorCodeRID">Database RID for the old color</param>
		/// <param name="aNewColorCodeRID">Database RID for the new color</param>
		internal void SetColorCodeRID(int aOldColorCodeRID, int aNewColorCodeRID)
		{
			if (!ColorIsInPack(aOldColorCodeRID))
			{
				throw new MIDException(eErrorLevel.warning,
					(int)(eMIDTextCode.msg_ColorNotDefinedInPack),
					MIDText.GetText(eMIDTextCode.msg_ColorNotDefinedInPack));
			}
			if (ColorIsInPack(aNewColorCodeRID))
			{
				throw new MIDException(eErrorLevel.warning,
					(int)(eMIDTextCode.msg_al_DuplicateColorInPackNotAllowed),
					MIDText.GetText(eMIDTextCode.msg_al_DuplicateColorInPackNotAllowed));
			}
			PackColorSize _content = GetColorBin(aOldColorCodeRID);
			_content.SetColorCodeRID(aNewColorCodeRID);
			_packColors.Add(aNewColorCodeRID, _content);
			_packColors.Remove(aOldColorCodeRID);
		}

		/// <summary>
		/// Gets original color key as it existed at create time.
		/// </summary>
		/// <param name="aColorRID">RID of the color</param>
		/// <returns>Original color RID as it existed on database at create time.</returns>
		internal int GetOriginalColorCodeRID(int aColorRID)
		{
			return (GetColorBin(aColorRID)).OriginalColorCodeRID;
		}
		#endregion PackColorCodeRID

		#region PackColorUnitsInPack
		//==========================//
		// Pack Color Units In Pack //
		//==========================//
		/// <summary>
		/// Gets number of units of the specified color contained in the pack
		/// </summary>
		/// <param name="aColorCodeRID">Database RID for the color</param>
		/// <returns>Units of this color in the pack</returns>
		public int GetColorUnitsInPack (int aColorCodeRID)
		{
			if (ColorIsInPack(aColorCodeRID))
			{
				return GetColorUnitsInPack(GetColorBin(aColorCodeRID));
			}
			else
			{
				throw new MIDException(eErrorLevel.warning,
					(int)(eMIDTextCode.msg_ColorNotDefinedInPack),
					MIDText.GetText(eMIDTextCode.msg_ColorNotDefinedInPack));
			}
		}
	
		/// <summary>
		/// Gets number of units of the specified color contained in the pack
		/// </summary>
		/// <param name="aColor">PackColorSize that describes pack color</param>
		/// <returns>Units of this color in the pack</returns>
		internal int GetColorUnitsInPack (PackColorSize aColor)
		{
			return aColor.ColorUnitsInPack;
		}

		/// <summary>
		/// Sets Color Units in the pack
		/// </summary>
		/// <param name="aColorCodeRID">Color Key</param>
		/// <param name="aColorUnits">Color Units in pack</param>
		internal void SetPackColorUnits(int aColorCodeRID, int aColorUnits)
		{
			if (ColorIsInPack(aColorCodeRID))
			{
				SetPackColorUnits((PackColorSize)_packColors[aColorCodeRID], aColorUnits);
			}
			else
			{
				throw new MIDException(eErrorLevel.warning,
					(int)(eMIDTextCode.msg_ColorNotDefinedInPack),
					MIDText.GetText(eMIDTextCode.msg_ColorNotDefinedInPack));
			}
		}

		/// <summary>
		/// Sets Color Units in the pack
		/// </summary>
		/// <param name="aColor">PackColorSize that describes pack color</param>
		/// <param name="aColorUnits">Color Units in pack</param>
		internal void SetPackColorUnits(PackColorSize aColor, int aColorUnits)
		{
			int _newTotalColorUnits = TotalColorUnitsInPack
				+ aColorUnits
				- aColor.ColorUnitsInPack;
			if (_newTotalColorUnits < 0)
			{
				throw new MIDException (eErrorLevel.warning,
					(int)eMIDTextCode.msg_AccumColorUnitsInPackCannotBeNeg,
					MIDText.GetText(eMIDTextCode.msg_AccumColorUnitsInPackCannotBeNeg));
			}
			else
			{
				aColor.SetColorUnitsInPack(aColorUnits);
				_totalColorUnits = _newTotalColorUnits;
			}
		}
		#endregion PackColorUnits
	
		#region PackColorUnitsAllocated
		//============================//
		// Pack Color Units Allocated //
		//============================//
		/// <summary>
		/// Gets the units allocated in the specified color of the pack.
		/// </summary>
		/// <param name="aColorCodeRID">Database RID of the color</param>
		/// <returns>Number of units of this color allocated by this pack.</returns>
		public int GetColorUnitsAllocated (int aColorCodeRID)
		{
			if (ColorIsInPack(aColorCodeRID))
			{
				return GetColorUnitsAllocated(GetColorBin(aColorCodeRID));
			}
			else
			{
				throw new MIDException(eErrorLevel.warning,
					(int)(eMIDTextCode.msg_ColorNotDefinedInPack),
					MIDText.GetText(eMIDTextCode.msg_ColorNotDefinedInPack));
			}
		}

		/// <summary>
		/// Gets the units allocated in the specified color of the pack.
		/// </summary>
		/// <param name="aColor">PackColorSize that describes color in pack.</param>
		/// <returns>Number of units of this color allocated by this pack.</returns>
		internal int GetColorUnitsAllocated (PackColorSize aColor)
		{
			return aColor.ColorUnitsInPack * PacksAllocated;
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets the Item units allocated in the specified color of the pack.
        /// </summary>
        /// <param name="aColorCodeRID">Database RID of the color</param>
        /// <returns>Number of item units of this color allocated by this pack.</returns>
        public int GetColorItemUnitsAllocated(int aColorCodeRID)
        {
            if (ColorIsInPack(aColorCodeRID))
            {
                return GetColorItemUnitsAllocated(GetColorBin(aColorCodeRID));
            }
            else
            {
                throw new MIDException(eErrorLevel.warning,
                    (int)(eMIDTextCode.msg_ColorNotDefinedInPack),
                    MIDText.GetText(eMIDTextCode.msg_ColorNotDefinedInPack));
            }
        }

        /// <summary>
        /// Gets the item units allocated in the specified color of the pack.
        /// </summary>
        /// <param name="aColor">PackColorSize that describes color in pack.</param>
        /// <returns>Number of item  units of this color allocated by this pack.</returns>
        internal int GetColorItemUnitsAllocated(PackColorSize aColor)
        {
            return aColor.ColorUnitsInPack * ItemPacksAllocated;
        }
        /// <summary>
        /// Gets the IMO units allocated in the specified color of the pack.
        /// </summary>
        /// <param name="aColorCodeRID">Database RID of the color</param>
        /// <returns>Number of IMO units of this color allocated by this pack.</returns>
        public int GetColorImoUnitsAllocated(int aColorCodeRID)
        {
            if (ColorIsInPack(aColorCodeRID))
            {
                return GetColorImoUnitsAllocated(GetColorBin(aColorCodeRID));
            }
            else
            {
                throw new MIDException(eErrorLevel.warning,
                    (int)(eMIDTextCode.msg_ColorNotDefinedInPack),
                    MIDText.GetText(eMIDTextCode.msg_ColorNotDefinedInPack));
            }
        }

        /// <summary>
        /// Gets the IMO units allocated in the specified color of the pack.
        /// </summary>
        /// <param name="aColor">PackColorSize that describes color in pack.</param>
        /// <returns>Number of IMO  units of this color allocated by this pack.</returns>
        internal int GetColorImoUnitsAllocated(PackColorSize aColor)
        {
            return aColor.ColorUnitsInPack * ImoPacksAllocated;
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1
		#endregion PackColorUnitsAllocated
		
		#region PackColorSequence
		/// <summary>
		/// Gets Color Sequence in the pack
		/// </summary>
		/// <param name="aColorCodeRID">Color Key</param>
		/// <returns>Color sequence within pack.</returns>
		internal int GetPackColorSequence(int aColorCodeRID)
		{
			return GetPackColorSequence(GetColorBin(aColorCodeRID));
		}

		/// <summary>
		/// Gets Color Sequence in the pack
		/// </summary>
		/// <param name="aColor">PackColorSize that describes the pack color</param>
		/// <return>Color Sequence in pack</return>
		internal int GetPackColorSequence(PackColorSize aColor)
		{
			return aColor.ColorSequenceInPack;
		}

		/// <summary>
		/// Sets Color Sequence in the pack
		/// </summary>
		/// <param name="aColorCodeRID">Color Key</param>
		/// <param name="aColorSequence">Color Sequence in pack</param>
		internal void SetPackColorSequence(int aColorCodeRID, int aColorSequence)
		{
			if (ColorIsInPack(aColorCodeRID))
			{
				SetPackColorSequence((PackColorSize)_packColors[aColorCodeRID], aColorSequence);
			}
			else
			{
				throw new MIDException(eErrorLevel.warning,
					(int)(eMIDTextCode.msg_ColorNotDefinedInPack),
					MIDText.GetText(eMIDTextCode.msg_ColorNotDefinedInPack));
			}
		}

		/// <summary>
		/// Sets Color Sequence in the pack
		/// </summary>
		/// <param name="aColor">PackColorSize that describes the pack color</param>
		/// <param name="aColorSequence">Color Sequence in pack</param>
		internal void SetPackColorSequence(PackColorSize aColor, int aColorSequence)
		{
			if (aColorSequence < 1) 
			{
				aColor.ColorSequenceInPack = _packColors.Count;
			}
			else
			{
				aColor.ColorSequenceInPack = aColorSequence;
			}
		}
		#endregion PackColorSequence
		#endregion PackColor

		#region PackSize
		#region PackColorSizeCount
		/// <summary>
		/// Gets the number of sizes in the specified color of the pack.
		/// </summary>
		/// <param name="aColorCodeRID">Database RID for the color.</param>
		/// <returns>Number of sizes in the specified color within the pack.</returns>
		/// <exception>If the specified color is not defined in the pack a color not found exception is thrown and the returned count is zero</exception>
		public int ColorSizeCount (int aColorCodeRID)
		{
			if (ColorIsInPack(aColorCodeRID))
			{
				return GetColorBin(aColorCodeRID).SizeCountInColor;
			}
			else
			{
				return 0;
			}
		}
		#endregion PackColorSizeCount

		#region PackColorSizeIsChanged
		internal bool PackColorSizeIsChanged(int aColorRID, int aSizeRID)
		{
			if (ColorIsInPack(aColorRID))
			{
				if (((PackColorSize)(this.PackColors[aColorRID])).SizeIsInColor(aSizeRID))
				{
                    return ((PackSizeBin)((PackColorSize)(this.PackColors[aColorRID])).ColorSizes[aSizeRID]).ContentIsChanged; // Assortment: added pack size bin
				}
			}
			return false;
		}
		#endregion PackColorSizeIsChanged

		#region PackColorSizeIsNew
		internal bool PackColorSizeIsNew(int aColorRID, int aSizeRID)
		{
			if (ColorIsInPack(aColorRID))
			{
				if (((PackColorSize)(this.PackColors[aColorRID])).SizeIsInColor(aSizeRID))
				{
                    return ((PackSizeBin)((PackColorSize)(this.PackColors[aColorRID])).ColorSizes[aSizeRID]).ContentIsNew; // Assortment: added pack size bin
				}
			}
			return false;
		}
		#endregion PackColorSizeIsNew

		#region AddSizeToPackColor
		/// <summary>
		/// Add a width-size to a color within a pack. 
		/// </summary>  
		/// <param name="aColorCodeRID">Database RID for this color.</param>
		/// <param name="aSizeRID">Database RID of this width-size.</param>
		/// <param name="aSizeUnits">Total size units for this color in this pack.</param>
		/// <remarks>
		/// <para>
		/// If the color and size are already defined in the pack, then the total size units will overlay the existing quantity.
		/// </para><para>
		/// If the color is not defined on the pack, it will be added (color units in the pack will be zero in this case; it is the caller's responsibility to set this field)
		/// </para>
		/// </remarks>
		internal void AddSizeToColor (int aColorCodeRID, int aSizeRID, int aSizeUnits, int aSizeSequence)
		{
			if (!ColorIsInPack(aColorCodeRID))
			{
				AddColor (aColorCodeRID, aSizeUnits, 0);
			}
			GetColorBin(aColorCodeRID).AddSize (aSizeRID, aSizeUnits, aSizeSequence);
		}
		#endregion AddSizeToPackColor

		#region PackColorSizeKey
		/// <summary>
		/// Gets original color key (as it exists at create time).
		/// </summary>
		/// <param name="aColorRID">RID of the color.</param>
		/// <param name="aSizeRID">RID of the size.</param>
		internal int GetOriginalPackColorSizeKey(int aColorRID, int aSizeRID)
		{
			return (GetColorBin(aColorRID)).GetOriginalSizeKey(aSizeRID);
		}
		#endregion PackColorSizeKey

		#region RemoveSizeFromPackColor
		/// <summary>
		/// Removes a size from a pack color.
		/// </summary> 
		/// <param name="aColorCodeRID">Database RID for the color</param>
		/// <param name="aSizeRID">Database RID for the size</param>
		//		internal void RemoveColor(int aColorCodeRID, int aSizeRID)
		internal void RemoveSizeFromColor(int aColorCodeRID, int aSizeRID)
		{
			this.GetColorBin(aColorCodeRID).RemoveSize(aSizeRID);
		}
		#endregion RemoveSizeFromPackColor

		#region PackSizeUnitsInColor
		/// <summary>
		/// Gets the color-size units in the pack for the specified color and size.
		/// </summary>
		/// <param name="aColorCodeRID">Database RID for the color.</param>
		/// <param name="aSizeRID">Database RID for the width size</param>
		/// <returns>Color-size units within the pack for the specifed color and width-size</returns>
		public int GetColorSizeUnitsInPack (int aColorCodeRID, int aSizeRID)
		{
			return GetColorSizeUnitsInPack(GetColorBin(aColorCodeRID), aSizeRID);
		}

		/// <summary>
		/// Gets the color-size units in the pack for the specified color and size.
		/// </summary>
		/// <param name="aColor">PackColorSize that describes the pack color.</param>
		/// <param name="aSizeRID">Database RID for the width size</param>
		/// <returns>Color-size units within the pack for the specifed color and width-size</returns>
		internal int GetColorSizeUnitsInPack (PackColorSize aColor, int aSizeRID)
		{
			return aColor.GetSizeUnitsInColor(aSizeRID);
		}

		/// <summary>
		/// Sets the color-size units in the pack for the specified color and size.
		/// </summary>
		/// <param name="aColorCodeRID">Database RID for the color.</param>
		/// <param name="aSizeRID">Database RID for the width size</param>
		/// <param name="aSizeUnits">Color-size units within the pack for the specified color and width-size.</param>
		internal void SetColorSizeUnitsInPack (int aColorCodeRID, int aSizeRID, int aSizeUnits)
		{
			SetColorSizeUnitsInPack(GetColorBin(aColorCodeRID), aSizeRID, aSizeUnits);
		}

		/// <summary>
		/// Sets the color-size units in the pack for the specified color and size.
		/// </summary>
		/// <param name="aColor">PackColorSize that describes the pack color.</param>
		/// <param name="aSizeRID">Database RID for the width size</param>
		/// <param name="aSizeUnits">Color-size units within the pack for the specified color and width-size.</param>
		internal void SetColorSizeUnitsInPack (PackColorSize aColor, int aSizeRID, int aSizeUnits)
		{
			aColor.SetSizeUnitsInColor(aSizeRID, aSizeUnits);
		}
		#endregion PackSizeUnitsInColor

		#region PackSizeUnitsAllocated
		/// <summary>
		/// Gets the size units allocated within the specified color of the pack.
		/// </summary>
		/// <param name="aColorCodeRID">Database RID for the color.</param>
		/// <param name="aSizeRID">Database RID for the width-size.</param>
		/// <returns>Units allocated in this color and size by the pack.</returns>
		public int GetColorSizeUnitsAllocated (int aColorCodeRID, int aSizeRID)
		{
			return GetColorSizeUnitsAllocated(GetColorBin(aColorCodeRID), aSizeRID);
		} 

		/// <summary>
		/// Gets the size units allocated within the specified color of the pack.
		/// </summary>
		/// <param name="aColor">PackColorSize that describes the color.</param>
		/// <param name="aSizeRID">Database RID for the width-size.</param>
		/// <returns>Units allocated in this color and size by the pack.</returns>
		public int GetColorSizeUnitsAllocated (PackColorSize aColor, int aSizeRID)
		{
			return PacksAllocated * aColor.GetSizeUnitsInColor(aSizeRID);
		}
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets the size item units allocated within the specified color of the pack.
        /// </summary>
        /// <param name="aColorCodeRID">Database RID for the color.</param>
        /// <param name="aSizeRID">Database RID for the width-size.</param>
        /// <returns>Item Units allocated in this color and size by the pack.</returns>
        public int GetColorSizeItemUnitsAllocated(int aColorCodeRID, int aSizeRID)
        {
            return GetColorSizeItemUnitsAllocated(GetColorBin(aColorCodeRID), aSizeRID);
        }

        /// <summary>
        /// Gets the size Item units allocated within the specified color of the pack.
        /// </summary>
        /// <param name="aColor">PackColorSize that describes the color.</param>
        /// <param name="aSizeRID">Database RID for the width-size.</param>
        /// <returns>Item Units allocated in this color and size by the pack.</returns>
        public int GetColorSizeItemUnitsAllocated(PackColorSize aColor, int aSizeRID)
        {
            return ItemPacksAllocated * aColor.GetSizeUnitsInColor(aSizeRID);
        }
        /// <summary>
        /// Gets the size IMO units allocated within the specified color of the pack.
        /// </summary>
        /// <param name="aColorCodeRID">Database RID for the color.</param>
        /// <param name="aSizeRID">Database RID for the width-size.</param>
        /// <returns>IMO Units allocated in this color and size by the pack.</returns>
        public int GetColorSizeImoUnitsAllocated(int aColorCodeRID, int aSizeRID)
        {
            return GetColorSizeImoUnitsAllocated(GetColorBin(aColorCodeRID), aSizeRID);
        }

        /// <summary>
        /// Gets the size IMO units allocated within the specified color of the pack.
        /// </summary>
        /// <param name="aColor">PackColorSize that describes the color.</param>
        /// <param name="aSizeRID">Database RID for the width-size.</param>
        /// <returns>IMO Units allocated in this color and size by the pack.</returns>
        public int GetColorSizeImoUnitsAllocated(PackColorSize aColor, int aSizeRID)
        {
            return ImoPacksAllocated * aColor.GetSizeUnitsInColor(aSizeRID);
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1
		#endregion PackSizeUnitsAllocated

		#region PackColorSizeSequence
		/// <summary>
		/// Gets Size Sequence in the pack color
		/// </summary>
		/// <param name="aColorCodeRID">Color Key</param>
		/// <param name="aSizeRID">RID of the size</param>
		/// <returns>Size sequence within pack color.</returns>
		internal int GetPackColorSizeSequence(int aColorCodeRID, int aSizeRID)
		{
			return GetPackColorSizeSequence(GetColorBin(aColorCodeRID), aSizeRID);
		}

		/// <summary>
		/// Gets Size Sequence in the pack color
		/// </summary>
		/// <param name="aColor">PackColorSize that describes the pack color</param>
		/// <param name="aSizeRID">RID of the size</param>
		/// <return>Size Sequence within pack color</return>
		internal int GetPackColorSizeSequence(PackColorSize aColor, int aSizeRID)
		{
			return aColor.GetPackColorSizeSequence(aSizeRID);
		}

		/// <summary>
		/// Sets Size Sequence in the pack color
		/// </summary>
		/// <param name="aColorCodeRID">Color Key</param>
		/// <param name="aSizeRID">RID of the size</param>
		/// <param name="aSizeSequence">Size Sequence in pack color</param>
		internal void SetPackColorSizeSequence(int aColorCodeRID, int aSizeRID, int aSizeSequence)
		{
			SetPackColorSizeSequence(GetColorBin(aColorCodeRID), aSizeRID, aSizeSequence);
		}
	
		/// <summary>
		/// Sets Size Sequence in the pack color
		/// </summary>
		/// <param name="aColor">PackColorSize that describes the pack color</param>
		/// <param name="aSizeRID">RID of the size</param>
		/// <param name="aSizeSequence">Size Sequence in pack color</param>
		internal void SetPackColorSizeSequence(PackColorSize aColor, int aSizeRID, int aSizeSequence)
		{
			aColor.SetPackColorSizeSequence(aSizeRID, aSizeSequence);
		}
		#endregion PackColorSequence
		#endregion PackSize

		#region PackContentEqual
		/// <summary>
		/// Compares the content of this pack to another to determine if equal.
		/// </summary>
		/// <param name="aPack">PackHdr to which to compare this pack.</param>
		/// <returns>True: packs have same content; False: packs are different.</returns>
		public bool PackContentEqual (PackHdr aPack)
		{
			if (this.PacksToAllocate > aPack.PacksToAllocate
				| this.PackMultiple != aPack.PackMultiple
				| this.GenericPack != aPack.GenericPack)
			{
				return false;
			}
			else
			{
				PackColorSize apPackColorSize;
				foreach(PackColorSize pcs in aPack.PackColors.Values)
				{
					if (aPack.PackColors.Contains(pcs.ColorCodeRID))
					{
						apPackColorSize = (PackColorSize)aPack.PackColors[pcs.ColorCodeRID];
						if (apPackColorSize.ColorUnitsInPack != pcs.ColorUnitsInPack)
						{
							return false;
						}
					}
					else
					{
						return false;
					}
				}
			}
			return true;
		}
		#endregion PackContentEqual

		#region PackStore
		#region StoreDimension
		/// <summary>
		/// Sets store dimension and store count.
		/// </summary>
		/// <param name="aStoreCount">Number of stores</param>
		/// <param name="aSubtotal">True: indicates this pack is a subtotal; False: indicates this pack is not a subtotal);</param>
		internal void SetStoreDimension(int aStoreCount, bool aSubtotal)
		{
			if (aStoreCount < 1)
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_NumberOfStoresCannotBeLessThan1,
					MIDText.GetText(eMIDTextCode.msg_NumberOfStoresCannotBeLessThan1));
			}
			else
			{
				_storePackAllocated = new StorePackAllocated[aStoreCount];
                _storeLocked = new bool[aStoreCount];   // TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
                _storeTempLock = new bool[aStoreCount];    // TT#59 Implement Temp Lock
                _storeItemTempLock = new bool[aStoreCount];    // TT#1401 - JEllis - Urban Reservation Stores pt 2
                for (int i = 0; i < aStoreCount; ++i)
				{
					_storePackAllocated[i].AllDetailAuditFlags = 0;
					_storePackAllocated[i].AllShipFlags = 0;
					_storePackAllocated[i].PacksAllocated = 0;
					_storePackAllocated[i].PacksAllocatedByAuto = 0;
					_storePackAllocated[i].PacksAllocatedByRule = 0;
					_storePackAllocated[i].PacksShipped = 0;
                    // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
                    _storePackAllocated[i].ItemPacksAllocated = 0;
                    //_storePackAllocated[i].ImoPacksAllocated = 0;  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                    // end TT#1401 - JEllis - Urban Reservation Stores pt 1
					if (aSubtotal)
					{
						// subtotals are initialized in reverse of a pack associated
						// with a header.
						_storePackAllocated[i].PackMinimum =
							_storePackAllocated[i].LargestPackMaximum;
					}
					else
					{
						_storePackAllocated[i].PackMaximum =
							_storePackAllocated[i].LargestPackMaximum;
						_storePackAllocated[i].PackPrimaryMaximum =
							_storePackAllocated[i].LargestPackMaximum;
					}
					_storePackAllocated[i].StorePackChosenRuleType = eRuleType.None;
					_storePackAllocated[i].StorePackChosenRuleLayerID = Include.NoLayerID;
				}
			}
		}  
		#endregion StoreDimension

		#region StoreLargestMaximum
		/// <summary>
		/// Returns largest possible maximum value.
		/// </summary>
		/// <returns></returns>
		internal int GetStorePackLargestMaximum()
		{
			return _storePackAllocated[0].LargestPackMaximum;
		}
		#endregion StoreLargestMaximum

		#region AuditFlags
		/// <summary>
		/// Gets all detail audit flags simultaneously for the indicated store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Ushort value each of whose bits represent a single flag setting.</returns>
        //internal ushort GetAllDetailAuditFlags(int aStoreIndex) // TT#488 - MD - Jellis - Group Allocation
		internal uint GetAllDetailAuditFlags(int aStoreIndex) // TT#488 - MD - Jellis - Group Allocation
        {
			return _storePackAllocated[aStoreIndex].AllDetailAuditFlags;
		}

		/// <summary>
		/// Sets all detail audit flags simultaneously for the indicated store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <param name="aFlags">Flag values</param>
        //internal void SetAllDetailAuditFlags (int aStoreIndex, ushort aFlags) // TT#488 - MD - Jellis - Group Allocation
		internal void SetAllDetailAuditFlags (int aStoreIndex, uint aFlags)   // TT#488 - MD - Jellis - Group Allocation
		{
			_storePackAllocated[aStoreIndex].AllDetailAuditFlags = aFlags;
		}

        /// <summary>
        /// Gets all detail audit flags simultaneously for the indicated store
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
        /// <returns>Ushort value each of whose bits represent a single flag setting.</returns>
        //internal ushort GetAllDetailAuditFlags(int aStoreIndex) // TT#488 - MD - Jellis - Group Allocation
        internal ushort GetAllGeneralAuditFlags(int aStoreIndex) // TT#488 - MD - Jellis - Group Allocation
        {
            return _storePackAllocated[aStoreIndex].AllGeneralAudits;
        }

        /// <summary>
        /// Sets all detail audit flags simultaneously for the indicated store.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
        /// <param name="aFlags">Flag values</param>
        //internal void SetAllDetailAuditFlags (int aStoreIndex, ushort aFlags) // TT#488 - MD - Jellis - Group Allocation
        internal void SetAllGeneralAuditFlags(int aStoreIndex, ushort aFlags)   // TT#488 - MD - Jellis - Group Allocation
        {
            _storePackAllocated[aStoreIndex].AllGeneralAudits = aFlags;
        }
        #endregion AuditFlags

		#region ShipFlags
		/// <summary>
		/// Gets all Ship flags simultaneously for the indicated store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Ushort value each of whose bits represent a single flag setting.</returns>
		internal byte GetAllShipFlags (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].AllShipFlags;
		}

		/// <summary>
		/// Sets all Ship flags simultaneously for the indicated store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <param name="aFlags">Flag values</param>
		internal void SetAllShipFlags (int aStoreIndex, byte aFlags)
		{
			_storePackAllocated[aStoreIndex].AllShipFlags = aFlags;
		}
		#endregion ShipFlags

		#region StoreIsChanged
		//==================//
		// Store Is Changed //
		//==================//
		/// <summary>
		/// Gets StorePackIsChanged for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True: Store Pack has changed; False: Store Pack has not changed.</returns>
		public bool GetStorePackIsChanged (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].StorePackIsChanged;
		}

		/// <summary>
		/// Sets StorePackIsChanged to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: Store Pack has changed; False: Store Pack has not changed.</param>
		internal void SetStorePackIsChanged (int aStoreIndex, bool aFlagValue)
		{
			_storePackAllocated[aStoreIndex].StorePackIsChanged = aFlagValue;
		}
		#endregion StoreIsChanged

		#region StoreIsNew
		//==================//
		// Store Is New //
		//==================//
		/// <summary>
		/// Gets StorePackIsNew for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True: Store Pack is New; False: Store Pack is not New.</returns>
		public bool GetStorePackIsNew (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].StorePackIsNew;
		}

		/// <summary>
		/// Sets StorePackIsNew to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: Store Pack is New; False: Store Pack is not New.</param>
		internal void SetStorePackIsNew (int aStoreIndex, bool aFlagValue)
		{
			_storePackAllocated[aStoreIndex].StorePackIsNew = aFlagValue;
		}
		#endregion StoreIsNew

		#region StoreIsManuallyAllocated
		//=============================//
		// Store Is Manually Allocated //
		//=============================//
		/// <summary>
		/// Gets StorePackIsManuallyAllocated for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's pack allocation has been manually specified by the user.</returns>
		public bool GetStorePackIsManuallyAllocated (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].PackIsManuallyAllocated;
		}

		/// <summary>
		/// Sets StorePackIsManuallyAllocated to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStorePackIsManuallyAllocated (int aStoreIndex, bool aFlagValue)
		{
			_storePackAllocated[aStoreIndex].PackIsManuallyAllocated = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
        /// <summary>
        /// Gets StorePackItemIsManuallyAllocated for the specified store
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>True when the store's pack item allocation has been manually specified by the user.</returns>
        public bool GetStorePackItemIsManuallyAllocated(int aStoreIndex) // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        {
            return _storePackAllocated[aStoreIndex].PackItemIsManuallyAllocated;  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        }

        /// <summary>
        /// Sets StorePackItemIsManuallyAllocated to specified value for specified store.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <param name="aFlagValue">Flag value</param>
        internal void SetStorePackItemIsManuallyAllocated(int aStoreIndex, bool aFlagValue) // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        {
            _storePackAllocated[aStoreIndex].PackItemIsManuallyAllocated = aFlagValue;  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
            this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 2
		#endregion StoreIsManuallyAllocated

		#region StoreWasAutoAllocated
		//==========================//
		// Store Was Auto Allocated //
		//==========================//
		/// <summary>
		/// Gets StorePackWasAutoAllocated for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's pack allocation has been manually specified by the user.</returns>
		public bool GetStorePackWasAutoAllocated (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].PackWasAutoAllocated;
		}

		/// <summary>
		/// Sets StorePackWasAutoAllocated to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStorePackWasAutoAllocated (int aStoreIndex, bool aFlagValue)
		{
			_storePackAllocated[aStoreIndex].PackWasAutoAllocated = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreWasAutoAllocated

		#region StoreChosenRuleAcceptedByHeader
		//======================================//
		// Store Chosen Rule Accepted By Header //
		//======================================//
		/// <summary>
		/// Gets StorePackChosenRuleAcceptedByHeader for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True indicates the chosen rule was accepted as an allocation rule on Header.</returns>
		public bool GetStorePackChosenRuleAcceptedByHeader (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].PackChosenRuleAcceptedByHeader; // TT#488 - MD - Jellis - Group Allocation
		}

		/// <summary>
		/// Sets StorePackChosenRuleAcceptedByHeader to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStorePackChosenRuleAcceptedByHeader (int aStoreIndex, bool aFlagValue)
		{
			_storePackAllocated[aStoreIndex].PackChosenRuleAcceptedByHeader = aFlagValue; // TT#488 - MD - Jellis - Group Allocation
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreChosenRuleAcceptedByHeader

        // begin TT#488 - MD - Jellis - Group Allocation
        #region StoreChosenRuleAcceptedByGroup
        //====================================//
        // Store Chosen Rule Accepted By Group//
        //====================================//
        /// <summary>
        /// Gets StorePackChosenRuleAcceptedByGroup for the specified store
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>True indicates the chosen rule was accepted as an allocation rule in Group Allocation.</returns>
        public bool GetStorePackChosenRuleAcceptedByGroup(int aStoreIndex)
        {
            return _storePackAllocated[aStoreIndex].PackChosenRuleAcceptedByGroup;
        }

        /// <summary>
        /// Sets StorePackChosenRuleAcceptedByGroup to specified value for specified store.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <param name="aFlagValue">Flag value</param>
        internal void SetStorePackChosenRuleAcceptedByGroup(int aStoreIndex, bool aFlagValue)
        {
            _storePackAllocated[aStoreIndex].PackChosenRuleAcceptedByGroup = aFlagValue; 
            this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); 
        }
        #endregion StoreChosenRuleAcceptedByGroup

        // end TT#488 - MD - Jellis - Group Allocation
	
		#region StoreRuleAllocationFromParentComponent
		//=========================================//
		// Store RuleAllocationFromParentComponent //
		//=========================================//
		/// <summary>
		/// Gets StorePackRuleAllocationFromParentComponent for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True indicates partially or completely allocated by the chosen rule of a parent component</returns>
		public bool GetStorePackRuleAllocationFromParentComponent (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].StorePackRuleAllocationFromParentComponent;
		}

		/// <summary>
		/// Sets StorePackRuleAllocationFromParentComponent to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStorePackRuleAllocationFromParentComponent (int aStoreIndex, bool aFlagValue)
		{
			_storePackAllocated[aStoreIndex].StorePackRuleAllocationFromParentComponent = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreRuleAllocationFromParentComponent

        // begin TT#488 - MD - Jellis - Group Allocation
        #region StoreRuleAllocationFromGroupComponent
        //=========================================//
        // Store RuleAllocationFromGroupComponent //
        //=========================================//
        /// <summary>
        /// Gets StorePackRuleAllocationFromGroupComponent for the specified store
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>True indicates partially or completely allocated by the chosen rule of a group component</returns>
        public bool GetStorePackRuleAllocationFromGroupComponent(int aStoreIndex)
        {
            return _storePackAllocated[aStoreIndex].StorePackRuleAllocationFromGroupComponent;
        }

        /// <summary>
        /// Sets StorePackRuleAllocationFromGroupComponent to specified value for specified store.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <param name="aFlagValue">Flag value</param>
        internal void SetStorePackRuleAllocationFromGroupComponent(int aStoreIndex, bool aFlagValue)
        {
            _storePackAllocated[aStoreIndex].StorePackRuleAllocationFromGroupComponent = aFlagValue;
            this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
        }
        #endregion StoreRuleAllocationFromGroupComponent
        // end TT#488 - MD - Jellis - Group Allocation

		#region StoreRuleAllocationFromChildComponent
		//========================================//
		// Store RuleAllocationFromChildComponent //
		//========================================//
		/// <summary>
		/// Gets StorePackRuleAllocationFromChildComponent for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True indicates partially or completely allocated by the chosen rule(s) of children components</returns>
		public bool GetStorePackRuleAllocationFromChildComponent (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].StorePackRuleAllocationFromChildComponent;
		}

		/// <summary>
		/// Sets StorePackRuleAllocationFromChildComponent to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStorePackRuleAllocationFromChildComponent (int aStoreIndex, bool aFlagValue)
		{
			_storePackAllocated[aStoreIndex].StorePackRuleAllocationFromChildComponent = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreRuleAllocationFromChildComponent

		#region StoreRuleAllocationFromChosenRule
		//====================================//
		// Store RuleAllocationFromChosenRule //
		//====================================//
		/// <summary>
		/// Gets StorePackRuleAllocationFromChosenRule for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True indicates partially or completely allocated by the chosen rule.</returns>
		public bool GetStorePackRuleAllocationFromChosenRule (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].StorePackRuleAllocationFromChosenRule;
		}

		/// <summary>
		/// Sets StorePackRuleAllocationFromChosenRule to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStorePackRuleAllocationFromChosenRule (int aStoreIndex, bool aFlagValue)
		{
			_storePackAllocated[aStoreIndex].StorePackRuleAllocationFromChosenRule = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreRuleAllocationFromChosenRule

		#region StoreAllocationFromPackContentBreakOut
		//====================================//
		// Store AllocationFromPackContentBreakOut //
		//====================================//
		/// <summary>
		/// Gets StorePackAllocationFromPackContentBreakOut for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True indicates partially or completely allocated by the chosen rule.</returns>
		public bool GetStorePackAllocationFromPackContentBreakOut (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].StorePackAllocationFromPackContentBreakOut;
		}

		/// <summary>
		/// Sets StorePackAllocationFromPackContentBreakOut to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStorePackAllocationFromPackContentBreakOut (int aStoreIndex, bool aFlagValue)
		{
			_storePackAllocated[aStoreIndex].StorePackAllocationFromPackContentBreakOut = aFlagValue;
		}
		#endregion StoreAllocationFromPackContentBreakOut


		// begin MID Track 4448 AnF Audit Enhancement
		#region StoreAllocationModifiedAfterMultiHeaderSplit
		//====================================================//
		// Store Allocation Modified After Multi Header Split //
		//====================================================//
		/// <summary>
		/// Gets StorePackAllocationModifiedAfterMultiHeaderSplit for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's pack allocation was changed after a multi header split.</returns>
		public bool GetStorePackAllocationModifiedAfterMultiHeaderSplit (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].PackAllocationModifiedAfterMultiHeaderSplit;
		}

		/// <summary>
		/// Sets StorePackAllocationModifiedAfterMultiHeaderSplit to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStorePackAllocationModifiedAfterMultiHeaderSplit (int aStoreIndex, bool aFlagValue)
		{
			_storePackAllocated[aStoreIndex].PackAllocationModifiedAfterMultiHeaderSplit = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreAllocationModifiedAfterMultiHeaderSplit
		// end MID Track 4448 AnF Audit Enhancement

		#region StoreOut
		//===========//
		// Store Out //
		//===========//
		/// <summary>
		/// Gets StorePackOut for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's pack allocation has been manually specified by the user.</returns>
		public bool GetStorePackOut (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].PackOut;
		}

		/// <summary>
		/// Sets StorePackOut to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStorePackOut (int aStoreIndex, bool aFlagValue)
		{
			_storePackAllocated[aStoreIndex].PackOut = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreOut
	
		#region StoreLocked
		//==============//
		// Store Locked //
		//==============//
		/// <summary>
		/// Gets StorePackLocked for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's pack allocation has been manually specified by the user.</returns>
		public bool GetStorePackLocked (int aStoreIndex)
		{
            // begin TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
            //return _storePackAllocated[aStoreIndex].PackLocked;
            return _storeLocked[aStoreIndex];
            // end TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
		}

		/// <summary>
		/// Sets StorePackLocked to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Lock Flag value</param>
		internal void SetStorePackLocked (int aStoreIndex, bool aFlagValue)
		{
            // begin  TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
            //_storePackAllocated[aStoreIndex].PackLocked = aFlagValue;
            //this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
            _storeLocked[aStoreIndex] = aFlagValue;
            // end TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
		}
		#endregion StoreLocked
	
		#region StoreTempLock
		//=================//
		// Store Temp Lock //
		//=================//
		/// <summary>
		/// Gets StorePackTempLock for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's pack allocation has been manually specified by the user.</returns>
		public bool GetStorePackTempLock (int aStoreIndex)
		{
			//return _storePackAllocated[aStoreIndex].PackTempLock;   // TT#59 Implement Temp Locks
            return _storeTempLock[aStoreIndex];                       // TT#59 Implement Temp Locks
		}

		/// <summary>
		/// Sets StorePackTempLock to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Temp Lock Flag value</param>
		internal void SetStorePackTempLock (int aStoreIndex, bool aFlagValue)
		{
            // begin TT#59 Implement Temp Locks
			//_storePackAllocated[aStoreIndex].PackTempLock = aFlagValue;
			//this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
            _storeTempLock[aStoreIndex] = aFlagValue;
            // end TT#59 Implement Temp Locks
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
        /// <summary>
        /// Gets StorePackItemTempLock for the specified store
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>True when the store's pack item allocation has been manually specified by the user.</returns>
        public bool GetStorePackItemTempLock(int aStoreIndex)
        {
            return _storeItemTempLock[aStoreIndex];      
        }

        /// <summary>
        /// Sets StorePackTItemempLock to specified value for specified store.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <param name="aFlagValue">Temp Lock Flag value</param>
        internal void SetStorePackItemTempLock(int aStoreIndex, bool aFlagValue)
        {
            _storeItemTempLock[aStoreIndex] = aFlagValue;
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 2

        // begin TT#59 Implement Temp Locks
        /// <summary>
        /// Resets Store Temp Locks to "false" for all stores.
        /// </summary>
        internal void ResetTempLocks()
        {
            if (_storeTempLock != null)
            {
                _storeTempLock = new bool[this.StoreDimensions];
                _storeItemTempLock = new bool[this.StoreDimensions]; // TT#1401 - JEllis - Urban Reservation Stores pt 2
            }
        }
        // end TT#59 Implement Temp Locks
		#endregion StoreTempLock
	
		#region StoreHadNeed
		//================//
		// Store Had Need //
		//================//
		/// <summary>
		/// Gets StorePackHadNeed for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's pack allocation has been manually specified by the user.</returns>
		public bool GetStorePackHadNeed (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].PackHadNeed;
		}

		/// <summary>
		/// Sets StorePackHadNeed to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">"Had Need" Flag value</param>
		internal void SetStorePackHadNeed (int aStoreIndex, bool aFlagValue)
		{
			_storePackAllocated[aStoreIndex].PackHadNeed = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance

		}
		#endregion StoreHadNeed
	
		#region StorePacksAllocated
		//=======================//
		// Store Packs Allocated //
		//=======================//
		/// <summary>
		/// Gets StorePacksAllocated for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's pack allocation has been manually specified by the user.</returns>
		public int GetStorePacksAllocated (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].PacksAllocated;
		}

		/// <summary>
		/// Sets StorePacksAllocated to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aPacksAllocated">Number of packs allocated</param>
		internal void SetStorePacksAllocated (int aStoreIndex, int aPacksAllocated)
		{
			SetPacksAllocated(PacksAllocated - _storePackAllocated[aStoreIndex].PacksAllocated);
			_storePackAllocated[aStoreIndex].PacksAllocated = aPacksAllocated;
			SetPacksAllocated(PacksAllocated + _storePackAllocated[aStoreIndex].PacksAllocated);
			this.SetStorePackIsChanged(aStoreIndex, true);
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreQtyAllocatedStructure(), true); // MID Track 3994 Performance
		    _pack.CalcStoresWithAllocationCount = true; // MID Track 4448 AnF Audit Enhancement
		}
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets StoreItemPacksAllocated for the specified store
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>Store's Item Packs allocated</returns>
        public int GetStoreItemPacksAllocated(int aStoreIndex)
        {
            return _storePackAllocated[aStoreIndex].ItemPacksAllocated;
        }

        /// <summary>
        /// Sets StoreItemPacksAllocated to specified value for specified store.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <param name="aPacksAllocated">Number of Item Packs allocated</param>
        internal void SetStoreItemPacksAllocated(int aStoreIndex, int aPacksAllocated)
        {
            SetItemPacksAllocated(ItemPacksAllocated - _storePackAllocated[aStoreIndex].ItemPacksAllocated);
            _storePackAllocated[aStoreIndex].ItemPacksAllocated = aPacksAllocated;
            SetItemPacksAllocated(ItemPacksAllocated + _storePackAllocated[aStoreIndex].ItemPacksAllocated);
            this.SetStorePackIsChanged(aStoreIndex, true);
            this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreItemQtyAllocatedStructure(), true); // TT#1401 - JEllis - Urban Reservation Stores pt 4
            _pack.CalcStoresWithAllocationCount = true; // MID Track 4448 AnF Audit Enhancement
        }
        /// <summary>
        /// Gets StoreImoPacksAllocated for the specified store
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>Store's IMO Packs allocated</returns>
        public int GetStoreImoPacksAllocated(int aStoreIndex)
        {
            return _storePackAllocated[aStoreIndex].ImoPacksAllocated;
        }

        // begin TT#1401 - JEllis - Urban Virtusl Store Warehouse  pt 29
        ///// <summary>
        ///// Sets StoreImoPacksAllocated to specified value for specified store.
        ///// </summary>
        ///// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        ///// <param name="aPacksAllocated">Number of IMO Packs allocated</param>
        //internal void SetStoreImoPacksAllocated(int aStoreIndex, int aPacksAllocated)
        //{
        //    SetImoPacksAllocated(ImoPacksAllocated - _storePackAllocated[aStoreIndex].ImoPacksAllocated);
        //    _storePackAllocated[aStoreIndex].ImoPacksAllocated = aPacksAllocated;
        //    SetImoPacksAllocated(ImoPacksAllocated + _storePackAllocated[aStoreIndex].ImoPacksAllocated); // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 14
        //    this.SetStorePackIsChanged(aStoreIndex, true);
        //    _pack.CalcStoresWithAllocationCount = true; // MID Track 4448 AnF Audit Enhancement
        //}
        // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1
		#endregion StorePacksAllocated
	
		#region StoreOrigPacksAllocated
		//================================//
		// Store Original Packs Allocated //
		//================================//
		/// <summary>
		/// Gets StoreOrigPacksAllocated for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		public int GetStoreOrigPacksAllocated (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].OrigPacksAllocated;
		}

		/// <summary>
		/// Sets StoreOrigPacksAllocated to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aPacksAllocated">Number of packs allocated</param>
		internal void SetStoreOrigPacksAllocated (int aStoreIndex, int aPacksAllocated)
		{
			_storePackAllocated[aStoreIndex].OrigPacksAllocated = aPacksAllocated;
		}
		#endregion StoreOrigPacksAllocated


		#region StorePackMaximum
		//====================//
		// Store Pack Maximum //
		//====================//
		/// <summary>
		/// Gets Store Pack Maximum allocation for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Pack Maximum allocation for the specified store</returns>
		public int GetStorePackMaximum (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].PackMaximum;
		}

		/// <summary>
		/// Sets Store Pack Maximum allocation for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aMaximum">Maximum pack allocation value</param>
		internal void SetStorePackMaximum (int aStoreIndex, int aMaximum)
		{
			_storePackAllocated[aStoreIndex].PackMaximum = aMaximum;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreMaximumStructure(), true); // MID Track 3994 Performance
		}
		#endregion StorePackMaximum
	
		#region StorePackMinimum
		//====================//
		// Store Pack Minimum //
		//====================//
		/// <summary>
		/// Gets Store Pack Minimum allocation for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Pack Minimum allocation for the specified store</returns>
		public int GetStorePackMinimum (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].PackMinimum;
		}

		
		/// <summary>
		/// Sets Store Pack Minimum allocation for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aMinimum">Minimum pack allocation value</param>
		internal void SetStorePackMinimum (int aStoreIndex, int aMinimum)
		{
			_storePackAllocated[aStoreIndex].PackMinimum = aMinimum;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreMinimumStructure(), true); // MID Track 3994 Performance
		}
		#endregion StorePackMinimum
	
		#region StorePackPrimaryMaximum
		//============================//
		// Store Pack Primary Maximum //
		//============================//
		/// <summary>
		/// Gets Store Pack Primary Maximum allocation for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Pack Maximum allocation for the specified store</returns>
		public int GetStorePackPrimaryMaximum (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].PackPrimaryMaximum;
		}
		
		/// <summary>
		/// Sets Store Pack Primary Maximum allocation for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aPrimaryMaximum">Primary Maximum pack allocation value</param>
		internal void SetStorePackPrimaryMaximum (int aStoreIndex, int aPrimaryMaximum)
		{
			_storePackAllocated[aStoreIndex].PackPrimaryMaximum = aPrimaryMaximum;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStorePrimaryMaxStructure(), true); // MID Track 3994 Performance
		}
		#endregion StorePackPrimaryMaximum
	
		#region StorePacksAllocatedByAuto
		//===============================//
		// Store Packs Allocated By Auto //
		//===============================//
		/// <summary>
		/// Gets StorePacksAllocatedByAuto for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Number of packs allocated by an auto allocation process.</returns>
		public int GetStorePacksAllocatedByAuto (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].PacksAllocatedByAuto;
		}

		/// <summary>
		/// Sets StorePacksAllocatedByAuto for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aPacksAllocated">Number of packs allocated by auto allocation process.</param>
		internal void SetStorePacksAllocatedByAuto (int aStoreIndex, int aPacksAllocated)
		{
			_storePackAllocated[aStoreIndex].PacksAllocatedByAuto = aPacksAllocated;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreQtyAllocatedByAutoStructure(), true); // MID Track 3994 Performance
		}
		#endregion StorePacksAllocatedByAuto
	
		#region StorePacksAllocatedByRule
		//===============================//
		// Store Packs Allocated By Rule //
		//===============================//
		/// <summary>
		/// Gets StorePacksAllocatedByRule for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Number of packs allocated by a Rule allocation process.</returns>
		public int GetStorePacksAllocatedByRule (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].PacksAllocatedByRule;
		}

		/// <summary>
		/// Sets StorePacksAllocatedByRule for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aPacksAllocated">Number of packs allocated by Rule allocation process.</param>
		internal void SetStorePacksAllocatedByRule (int aStoreIndex, int aPacksAllocated)
		{
			_storePackAllocated[aStoreIndex].PacksAllocatedByRule = aPacksAllocated;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreQtyAllocatedByRuleStructure(), true); // MID Track 3994 Performance
		}
		#endregion StorePacksAllocatedByRule
	
		#region StoreChosenRuleType
		//========================//
		// Store Chosen Rule Type //
		//========================//
		/// <summary>
		/// Gets StoreChosenRuleType for the specified store in this Pack.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Pack Chosen Rule Type.</returns>
		public eRuleType GetStorePacksChosenRuleType (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].StorePackChosenRuleType;
		}

		/// <summary>
		/// Sets StoreChosenRuleType for the specified store in this Pack.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aChosenRuleType">Rule Type.</param>
		internal void SetStorePacksChosenRuleType (int aStoreIndex, eRuleType aChosenRuleType)
		{
			_storePackAllocated[aStoreIndex].StorePackChosenRuleType = aChosenRuleType;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreChosenRuleQtyStructure(), true); // MID Track 3994 Performance

		}
		#endregion StoreChosenRuleType

		#region StoreChosenRuleLayerID
		//============================//
		// Store Chosen Rule Layer ID //
		//============================//
		/// <summary>
		/// Gets StoreChosenRuleLayerID for the specified store in this Pack.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Pack Chosen Rule RID.</returns>
		public int GetStorePacksChosenRuleLayerID (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].StorePackChosenRuleLayerID;
		}

		/// <summary>
		/// Sets StoreChosenRuleLayerID for the specified store in this Pack.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aChosenRuleLayerID">Pack Chosen Rule ID.</param>
		internal void SetStorePacksChosenRuleLayerID (int aStoreIndex, int aChosenRuleLayerID)
		{
			_storePackAllocated[aStoreIndex].StorePackChosenRuleLayerID = aChosenRuleLayerID;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreChosenRuleLayerStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreChosenRuleLayerID

		#region StoreChosenRulePacksAllocated
		//===================================//
		// Store Chosen Rule Packs Allocated //
		//===================================//
		/// <summary>
		/// Gets StoreChosenRulePacksAllocated for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Number of packs allocated by the chosen rule.</returns>
		public int GetStoreChosenRulePacksAllocated (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].StoreChosenRulePacksAllocated;
		}

		/// <summary>
		/// Sets StoreChosenRulePacksAllocated for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aPacksAllocated">Number Packs Allocated allocated by chosen rule.</param>
		internal void SetStoreChosenRulePacksAllocated (int aStoreIndex, int aPacksAllocated)
		{
			_storePackAllocated[aStoreIndex].StoreChosenRulePacksAllocated = aPacksAllocated;
		}
		#endregion StoreChosenRulePacksAllocated

		#region StoreLastNeedDay
		//=======================//
		// Store Last Need Day //
		//=======================//
		/// <summary>
		/// Gets StoreLastNeedDay for the specified store in this Pack.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Pack Last Need Day.</returns>
		public DateTime GetStorePackLastNeedDay (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].StorePackLastNeedDay;
		}

		/// <summary>
		/// Sets StoreLastNeedDay for the specified store in this Pack.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aLastNeedDay">Pack Last Need Day.</param>
		internal void SetStorePackLastNeedDay (int aStoreIndex, DateTime aLastNeedDay)
		{
			_storePackAllocated[aStoreIndex].StorePackLastNeedDay = aLastNeedDay;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreNeedDayStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreLastNeedDay

		#region StoreUnitNeedBefore
		//=======================//
		// Store Unit Need Before //
		//=======================//
		/// <summary>
		/// Gets StoreUnitNeedBefore for the specified store in this Pack.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Pack Unit Need Before.</returns>
		public int GetStorePackUnitNeedBefore (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].StorePackUnitNeedBefore;
		}

		/// <summary>
		/// Sets StoreUnitNeedBefore for the specified store in this Pack.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aUnitNeedBefore">Pack Unit Need Before.</param>
		internal void SetStorePackUnitNeedBefore (int aStoreIndex, int aUnitNeedBefore)
		{
			_storePackAllocated[aStoreIndex].StorePackUnitNeedBefore = aUnitNeedBefore;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreNeedBeforeStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreUnitNeedBefore

		#region StorePercentNeedBefore
		//=======================//
		// Store Percent Need Before //
		//=======================//
		/// <summary>
		/// Gets StorePercentNeedBefore for the specified store in this Pack.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Pack Percent Need Before.</returns>
		public double GetStorePackPercentNeedBefore (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].StorePackPercentNeedBefore;
		}

		/// <summary>
		/// Sets StorePercentNeedBefore for the specified store in this Pack.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aPercentNeedBefore">Pack Percent Need Before.</param>
		internal void SetStorePackPercentNeedBefore (int aStoreIndex, double aPercentNeedBefore)
		{
			_storePackAllocated[aStoreIndex].StorePackPercentNeedBefore = aPercentNeedBefore;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStorePercentNeedBeforeStructure(), true); // MID Track 3994 Performance
		}
		#endregion StorePercentNeedBefore

		#region StorePackShippingStarted
		//=============================//
		// Store Pack Shipping Started //
		//=============================//
		/// <summary>
		/// Gets Store Pack Shipping Started audit flag for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True if shipping has started for this store.</returns>
		public bool GetStorePackShippingStarted (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].ShippingStarted;
		}

		/// <summary>
		/// Sets Store Pack Shipping Started audit flag for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Shipping Started Flag value</param>
		/// <returns></returns>
		internal void SetStorePackShippingStarted(int aStoreIndex, bool aFlagValue)
		{
			_storePackAllocated[aStoreIndex].ShippingStarted = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreShippedStatusFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StorePackShippingStarted
	
		#region StorePackShippingComplete
		//==============================//
		// Store Pack Shipping Complete //
		//==============================//
		/// <summary>
		/// Gets Store Pack Shipping Complete audit flag for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True if shipping has Complete for this store.</returns>
		public bool GetStorePackShippingComplete (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].ShippingComplete;
		}

		/// <summary>
		/// Sets Store Pack Shipping Complete audit flag for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Shipping Complete Flag value</param>
		/// <returns></returns>
		internal void SetStorePackShippingComplete(int aStoreIndex, bool aFlagValue)
		{
			_storePackAllocated[aStoreIndex].ShippingComplete = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreShippedStatusFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StorePackShippingComplete
	
		#region StorePacksShipped
		//====================//
		// Store Pack Shipped //
		//====================//
		/// <summary>
		/// Gets StorePacksShipped to the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Number of packs shipped to the specified store.</returns>
		public int GetStorePacksShipped (int aStoreIndex)
		{
			return _storePackAllocated[aStoreIndex].PacksShipped;
		}

		/// <summary>
		/// Sets StorePacksShipped to the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aPacksShipped">Number of packs shipped to the specified store.</param>
		internal void SetStorePacksShipped (int aStoreIndex, int aPacksShipped)
		{
			_storePackAllocated[aStoreIndex].PacksShipped = aPacksShipped;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreQtyShippedStructure(), true); // MID Track 3994 Performance
		}
		#endregion StorePacksShipped
	
		#region RemoveStores
		//===============//
		// Remove Stores //
		//===============//
		/// <summary>
		/// Removes stores from pack.
		/// </summary>
		internal void RemoveStores()
		{
			this._storePackAllocated = null;
            _storeLocked = null;            // TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
            this._storeTempLock = null;     // TT#59 Implement Temp Locks
            this._storeItemTempLock = null;     // TT#1401 - JEllis - Urban Reservation Stores pt 2
        }
		#endregion RemoveStores

		#endregion PackStore

		#region PackCopy
		/// <summary>
		/// Creates a copy of this pack with a new name. 
		/// </summary>
		/// <remarks>
		/// This copy creates a copy of the contents of this pack under in the given CopyToPack.  Store information, units
		/// allocated/packs allocated are not copied.  Also, reserve packs is not copied.
		/// --warning: the purpose of this method is to facilitate functions from within AllocationProfile.  
		/// </remarks>
		/// <param name="aCopyToPack">PackHdr that is the target of the copy.</param>
		public void CopyPackContentTo (PackHdr aCopyToPack)
		{
			aCopyToPack.SetGenericPack(this.GenericPack);
			aCopyToPack.SetPackMultiple(this.PackMultiple);
			aCopyToPack.SetPacksToAllocate(this.PacksToAllocate);
			foreach (PackColorSize pc in this.PackColors.Values)
			{
				aCopyToPack.AddColor(pc.ColorCodeRID, pc.ColorUnitsInPack, pc.ColorSequenceInPack);
                foreach (PackSizeBin psb in pc.ColorSizes.Values) // Assortment: added pack size bin
				{
                    aCopyToPack.AddSizeToColor(pc.ColorCodeRID, psb.ContentCodeRID, psb.ContentUnits, psb.Sequence); // Assortment: added pack size bin
				}
			}
		}
        #endregion PackCopy

        #region StoreEligibility
        /// <summary>
        /// Gets store Eligibility flag value.
        /// </summary>
        /// <param name="aStore">Index_RID identifier for the store</param>
        /// <returns>True if eligible, false if not eligible.</returns>
        public bool GetStoreIsEligible(Index_RID aStore)
        {
            return _storePackAllocated[aStore.Index].StoreIsEligible;
        }

        /// <summary>
        /// Sets a store's Eligibility flag value.
        /// </summary>
        /// <param name="aStore">Index_RID identifier for the store.</param>
        /// <param name="aFlagValue">True if eligible, false if not.</param>
        internal void SetStoreIsEligible(Index_RID aStore, bool aFlagValue)
        {
            if (!_storeEligibilityLoaded || GetStoreIsEligible(aStore) != aFlagValue)
            {
                //SetIsChanged(eAllocationSummaryNode.Total, true);
                _storePackAllocated[aStore.Index].StoreIsEligible = aFlagValue;
                //SetStoreIsChanged(eAllocationSummaryNode.Total, aStore, true);
                this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true);
            }
        }
        #endregion StoreEligibility
        #endregion Methods
    }
    #endregion PackHdr

    #region PackHdrCompareAscend
    /// <summary>
    /// Indicates the ascending sequence of two packs.
    /// </summary>
    public class PackHdrCompareAscend : IComparer
	{
		/// <summary>
		/// Compares packs x and y and inicates the ascending sequence of the two. 
		/// </summary>
		/// <param name="x">First of two PackHdr's</param>
		/// <param name="y">Second of two PackHdr's</param>
		/// <returns>-1 if the pack multiple of x is less than the pack multiple of y; +1 if the pack multiple of y is less than the pack multiple of x; -1 if the pack multiples are equal and packs to allocate on y is less than the packs to allocate on x; +1 if the multiples are equal and the packs to allocate on x are less than the packs to allocate on y; 0 otherwise (equal).</returns>
		public int Compare (object x, object y)
		{
			if (!(x is PackHdr && y is PackHdr))
			{
				throw new MIDException(eErrorLevel.severe,
					(int)(eMIDTextCode.msg_CompareObjectMustBePackHdr),
					MIDText.GetText(eMIDTextCode.msg_CompareObjectMustBePackHdr));
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return +1;
			}
			//			int cmp = String.Compare(((PackHdr)x).PackName, ((PackHdr)y).PackName);
			//			if (cmp == 0)
			//			{
			if (((PackHdr)x).PackMultiple < ((PackHdr)y).PackMultiple)
			{
				return -1;
			}
			if (((PackHdr)x).PackMultiple > ((PackHdr)y).PackMultiple)
			{
				return +1;
			}
			if (((PackHdr)x).PacksToAllocate < ((PackHdr)y).PacksToAllocate)
			{
				return +1;
			}
			if (((PackHdr)x).PacksToAllocate > ((PackHdr)y).PacksToAllocate)
			{
				return -1;
			}
			return String.Compare(((PackHdr)x).PackName, ((PackHdr)y).PackName);;
			//			}
			//			return cmp;
		}
	}
	/// <summary>
	/// Indicates the descending sequence of two packs.
	/// </summary>
	public class PackHdrCompareDescend : IComparer
	{
		/// <summary>
		/// Compares packs x and y and inicates the ascending sequence of the two. 
		/// </summary>
		/// <param name="x">First of two PackHdr's</param>
		/// <param name="y">Second of two PackHdr's</param>
		/// <returns>-1 if the pack multiple of x is less than the pack multiple of y; +1 if the pack multiple of y is less than the pack multiple of x; -1 if the pack multiples are equal and packs to allocate on y is less than the packs to allocate on x; +1 if the multiples are equal and the packs to allocate on x are less than the packs to allocate on y; 0 otherwise (equal).</returns>
		public int Compare (object x, object y)
		{
			if (!(x is PackHdr && y is PackHdr))
			{
				throw new MIDException(eErrorLevel.severe,
					(int)(eMIDTextCode.msg_CompareObjectMustBePackHdr),
					MIDText.GetText(eMIDTextCode.msg_CompareObjectMustBePackHdr));
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return +1;
			}
			//			int cmp = String.Compare(((PackHdr)x).PackName, ((PackHdr)y).PackName);
			//			if (cmp == 0)
			//			{
			if (((PackHdr)x).PackMultiple < ((PackHdr)y).PackMultiple)
			{
				return +1;
			}
			if (((PackHdr)x).PackMultiple > ((PackHdr)y).PackMultiple)
			{
				return -1;
			}
			if (((PackHdr)x).PacksToAllocate < ((PackHdr)y).PacksToAllocate)
			{
				return +1;
			}
			if (((PackHdr)x).PacksToAllocate > ((PackHdr)y).PacksToAllocate)
			{
				return -1;
			}
			return String.Compare(((PackHdr)x).PackName, ((PackHdr)y).PackName);;
			//			}
			//			return cmp;
		}
	}
	#endregion PackCompareAscend

    // begin TT#488 - MD - Jellis - Group Allocation
    #region PackHdrAsrtCompare
    /// <summary>
    /// Indicates the ascending sequence of two packs.
    /// </summary>
    public class PackHdrAsrtCompareAscend : IComparer
    {
        /// <summary>
        /// Compares packs x and y and inicates the ascending sequence of the two (from Assortment/Group Allocation Point of view--uses Assortment Pack Name rather than pack name). 
        /// </summary>
        /// <param name="x">First of two PackHdr's</param>
        /// <param name="y">Second of two PackHdr's</param>
        /// <returns>-1 if the pack multiple of x is less than the pack multiple of y; +1 if the pack multiple of y is less than the pack multiple of x; -1 if the pack multiples are equal and packs to allocate on y is less than the packs to allocate on x; +1 if the multiples are equal and the packs to allocate on x are less than the packs to allocate on y; 0 otherwise (equal).</returns>
        public int Compare(object x, object y)
        {
            if (!(x is PackHdr && y is PackHdr))
            {
                throw new MIDException(eErrorLevel.severe,
                    (int)(eMIDTextCode.msg_CompareObjectMustBePackHdr),
                    MIDText.GetText(eMIDTextCode.msg_CompareObjectMustBePackHdr));
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return +1;
            }
            //			int cmp = String.Compare(((PackHdr)x).PackName, ((PackHdr)y).PackName);
            //			if (cmp == 0)
            //			{
            if (((PackHdr)x).PackMultiple < ((PackHdr)y).PackMultiple)
            {
                return -1;
            }
            if (((PackHdr)x).PackMultiple > ((PackHdr)y).PackMultiple)
            {
                return +1;
            }
            if (((PackHdr)x).PacksToAllocate < ((PackHdr)y).PacksToAllocate)
            {
                return +1;
            }
            if (((PackHdr)x).PacksToAllocate > ((PackHdr)y).PacksToAllocate)
            {
                return -1;
            }
            return String.Compare(((PackHdr)x).AssortmentPackName, ((PackHdr)y).AssortmentPackName); ;
            //			}
            //			return cmp;
        }
    }
    /// <summary>
    /// Indicates the descending sequence of two packs.
    /// </summary>
    public class PackHdrAsrtCompareDescend : IComparer
    {
        /// <summary>
        /// Compares packs x and y and inicates the ascending sequence of the two (from Assortment/Group Allocation Point of view--uses Assortment Pack Name rather than Pack name). 
        /// </summary>
        /// <param name="x">First of two PackHdr's</param>
        /// <param name="y">Second of two PackHdr's</param>
        /// <returns>-1 if the pack multiple of x is less than the pack multiple of y; +1 if the pack multiple of y is less than the pack multiple of x; -1 if the pack multiples are equal and packs to allocate on y is less than the packs to allocate on x; +1 if the multiples are equal and the packs to allocate on x are less than the packs to allocate on y; 0 otherwise (equal).</returns>
        public int Compare(object x, object y)
        {
            if (!(x is PackHdr && y is PackHdr))
            {
                throw new MIDException(eErrorLevel.severe,
                    (int)(eMIDTextCode.msg_CompareObjectMustBePackHdr),
                    MIDText.GetText(eMIDTextCode.msg_CompareObjectMustBePackHdr));
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return +1;
            }
            //			int cmp = String.Compare(((PackHdr)x).PackName, ((PackHdr)y).PackName);
            //			if (cmp == 0)
            //			{
            if (((PackHdr)x).PackMultiple < ((PackHdr)y).PackMultiple)
            {
                return +1;
            }
            if (((PackHdr)x).PackMultiple > ((PackHdr)y).PackMultiple)
            {
                return -1;
            }
            if (((PackHdr)x).PacksToAllocate < ((PackHdr)y).PacksToAllocate)
            {
                return +1;
            }
            if (((PackHdr)x).PacksToAllocate > ((PackHdr)y).PacksToAllocate)
            {
                return -1;
            }
            return String.Compare(((PackHdr)x).AssortmentPackName, ((PackHdr)y).AssortmentPackName); ;
            //			}
            //			return cmp;
        }
    }
    #endregion PackHdrAsrtCompare
    // end TT#488 - MD - Jellis - Group Allocation

	#region HdrSizeBin
	/// <summary>
	/// Container for the Bulk Size units on an Allocation Header.
	/// </summary>
//Begin Track #4302 - JScott - Size Codes in wrong order afer release
//	public class HdrSizeBin
	public class HdrSizeBin : IComparable
//End Track #4302 - JScott - Size Codes in wrong order afer release
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private HdrColorBin _color;
        private int _hdr_BCSZ_KEY;  // Assortment: Color/Size Changes
        private int _SizeCodeRID;   // Assortment: Color/Size Changes
        private int _origSizeCodeRID;   // Assortment: Color/Size Changes
		//private int _SizeKey;     // Assortment: Color/Size Changes
        //private int _origSizeKey;  // Assortment: Color/Size Changes
		private HdrAllocationBin _sizeBin;
		private MinMaxAllocationBin _minMaxBin;
		private int _reserveUnits;
		private StoreAllocationStructureStatus _sass; // MID Track 3994 Performance
		private StoreSizeAllocated[] _storeSizeAllocated;
        private bool[] _storeLocked;       // TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
        private bool[] _storeTempLock;     // TT#59 Implement Temp Locks
        private bool[] _storeItemTempLock;     // TT#1401 - JEllis - Urban Reservation Stores pt 2

		#endregion Fields

		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Used to construct an instance of this class.
		/// </summary>
		/// <param name="aColor">HdrColorBin that describes the color to which this size belongs.</param>
        internal HdrSizeBin(HdrColorBin aColor)
        {
            // begin fix: Size keys start at 0
            //int last_BCSZ_Key_Used = aColor.Last_BCSZ_Key_Used++;
            aColor.Last_BCSZ_Key_Used++;
            int last_BCSZ_Key_Used = aColor.Last_BCSZ_Key_Used;
            // end fix: Size keys start at 0
            CreateHdrSizeBin(aColor, last_BCSZ_Key_Used);
        }
        /// <summary>
		/// Used to construct an instance of this class.
		/// </summary>
		/// <param name="aColor">HdrColorBin that describes the color to which this size belongs.</param>
        /// <param name="aBCSZ_KEY">HdrBCSZ_Key for this size in this color</param>
		internal HdrSizeBin(HdrColorBin aColor, int aBCSZ_KEY)
		{
            int last_BCSZ_Key_Used = aBCSZ_KEY;
            if (last_BCSZ_Key_Used < 0)   // Size Keys start at 0: Negative key should trigger generating new key, "0" key should not trigger this!
            {
                // begin fix: Size Keys start at 0
                //last_BCSZ_Key_Used = aColor.Last_BCSZ_Key_Used++;
                aColor.Last_BCSZ_Key_Used++;
                last_BCSZ_Key_Used = aColor.Last_BCSZ_Key_Used;
                // end fix: Size Keys start at 0
            }
            CreateHdrSizeBin(aColor, last_BCSZ_Key_Used);
        }
        private void CreateHdrSizeBin(HdrColorBin aColor, int aBCSZ_KEY)
        {
			_color = aColor;

            // Begin TT#81 - Ron Matelic - Database error adding sizes under certain conditions
            _color.ColorIsChanged = true;
            // End TT#81

            // Begin Assortment: Color/Size Changes
			//_SizeKey = Include.NoRID;
			//_origSizeKey = Include.NoRID;
            _origSizeCodeRID = Include.NoRID;
            _hdr_BCSZ_KEY = aBCSZ_KEY;
            _SizeCodeRID = Include.NoRID;
            // End Assortment: Color/Size Changes
			_sizeBin.SetQtyAllocated(0);
			_sizeBin.SetQtyToAllocate(0);
			_sizeBin.SetUnitMultiple(1);
            // begin TT#1401 - JEllis - Urbban Reservation Stores pt 1
            _sizeBin.SetItemQtyAllocated(0);
            //_sizeBin.SetImoQtyAllocated(0);  // TT#1401 - JEllis - Virtual Store Warehouse pt 29
            // end TT#1401 - JEllis - Urban Reservation Stores pt 1
			_sizeBin.Sequence = 0;
			_sizeBin.IsChanged = false;
			_sizeBin.IsNew = false;
			_minMaxBin.SetMaximum(_minMaxBin.LargestMaximum);
			_minMaxBin.SetMinimum(0);
			_reserveUnits = 0;
			_storeSizeAllocated = null;
			_sass = new StoreAllocationStructureStatus(false); // MID Track 3994 Performance
			_sizeBin.StoresWithAllocationCount = 0;         // MID Track 4448 AnF Audit Enhancement
			_sizeBin.CalcStoresWithAllocationCount = false;  // MID Track 4448 AnF Audit Enhancement
		}
		#endregion Constructors

		#region Properties
		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets or sets IsChanged database update flag
		/// </summary>
		internal bool SizeIsChanged
		{
			get
			{
				return _sizeBin.IsChanged;
			}
			set
			{
				_sizeBin.IsChanged = value;
			}
		}

		/// <summary>
		/// Gets or sets IsNew database update flag
		/// </summary>
		internal bool SizeIsNew
		{
			get 
			{
				return _sizeBin.IsNew;
			}
			set
			{
				_sizeBin.IsNew = value;
			}
		}


		/// <summary>
		/// Gets the HdrColorBin where this size resides.
		/// </summary>
		public HdrColorBin Color
		{
			get
			{
				return _color;
			}
		}

        // Begin Assortment: Color/Size Change
        /// <summary>
        /// Gets or sets the HDR_BCSZ_KEY that is unique within color
        /// </summary>
        public int HDR_BCSZ_KEY
        {
            get
            {
                return _hdr_BCSZ_KEY;
            }
            set
            {
                _hdr_BCSZ_KEY = value;
            }
        }
		// /// <summary>
		// /// Gets the SizeKey 
		// /// </summary>
		//public int SizeKey
		//{
		//	get
		//	{
		//		return _SizeKey;
		//	}
		//}
        /// <summary>
        /// Obsolete property to retrieve "Size Code Key"
        /// </summary>
        public int SizeKey
        {
            get
            {
                return SizeCodeRID;
            }
        }
        /// <summary>
        /// Gets the SizeCodeRID
        /// </summary>
        public int SizeCodeRID  // Assortment: Color/size change
        {
            get
            {
                return _SizeCodeRID;
            }
        }

		 /// <summary>
		 /// Gets the original SizeRID as it existed at create time. 
		 /// </summary>
		internal int OriginalSizeCodeRID
		{
			get
			{
				return _origSizeCodeRID;
			}
		}
        // End Assortment: Color/Size Change

		/// <summary>
		/// Gets the size units to allocate
		/// </summary>
		public int SizeUnitsToAllocate
		{
			get
			{
				return _sizeBin.QtyToAllocate;
			}
		}

		/// <summary>
		/// Gets the size units allocated
		/// </summary>
		public int SizeUnitsAllocated
		{
			get
			{
				return _sizeBin.QtyAllocated;
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets the size item units allocated
        /// </summary>
        public int SizeItemUnitsAllocated
        {
            get
            {
                return _sizeBin.ItemQtyAllocated;
            }
        }
        /// <summary>
        /// Gets the size IMO units allocated
        /// </summary>
        public int SizeImoUnitsAllocated
        {
            get
            {
                return _sizeBin.ImoQtyAllocated;
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1

		/// <summary>
		/// Gets or sets the size multiple.
		/// </summary>
		public int SizeMultiple
		{
			get
			{
				return _sizeBin.UnitMultiple;
			}
		}

		/// <summary>
		/// Gets or sets size sequence
		/// </summary>
		public int SizeSequence
		{
			get
			{
				return _sizeBin.Sequence;
			}
			set
			{
				_sizeBin.Sequence = value;
			}
		}

		/// <summary>
		/// Gets or sets the allocation size minimum
		/// </summary>
		public int SizeMinimum
		{
			get
			{
				return _minMaxBin.Minimum;
			}
		}

		/// <summary>
		/// Gets the largest possible size maximum
		/// </summary>
		public int SizeLargestMaximum
		{
			get
			{
				return _minMaxBin.LargestMaximum;
			}
		}

		/// <summary>
		/// Gets the allocation size maximum
		/// </summary>
		public int SizeMaximum
		{
			get
			{
				return _minMaxBin.Maximum;
			}
		}

		/// <summary>
		/// Gets or sets ReserveUnits.
		/// </summary>
		public int ReserveUnits
		{
			get
			{
				return _reserveUnits;
			}
		}

		/// <summary>
		/// Gets the number of store dimensions in the store allocation array.
		/// </summary>
		public int StoreDimensions
		{
			get 
			{
				return _storeSizeAllocated.Length;
			}
		}
		// begin MID Track 3994 Performance
		internal StoreAllocationStructureStatus AllocationStructureStatus
		{
			get
			{
				return this._sass;
			}
			set 
			{
				this._sass = value;
			}
		}
		// end MID Track 3994 Performance
		//begin MID Track 4448 AnF Audit Enhancement
		/// <summary>
		/// Gets Count of stores with an allocation
		/// </summary>
		public int StoresWithAllocationCount
		{
			get
			{
				if (_sizeBin.CalcStoresWithAllocationCount)
				{
					_sizeBin.StoresWithAllocationCount = 0;
					for (int i=0; i < this._storeSizeAllocated.Length; i++)
					{
						if (this.GetStoreSizeUnitsAllocated(i) > 0)
						{
							_sizeBin.StoresWithAllocationCount++;
						}
					}
					_sizeBin.CalcStoresWithAllocationCount = false;
				}
				return _sizeBin.StoresWithAllocationCount;
			}
		}
		// end MID Track 4448 AnF Audit Enhancement
		#endregion Properties
	
		#region Methods
		//========
		// METHODS
		//========
//Begin Track #4302 - JScott - Size Codes in wrong order afer release
		#region IComparable Members
		public int CompareTo(object obj)
		{
			HdrSizeBin hsb;

			if (obj.GetType() == typeof(HdrSizeBin))
			{
				hsb = (HdrSizeBin)obj;
				if (this.SizeSequence == hsb.SizeSequence)
				{
					//if (this.SizeKey < hsb.SizeKey) // Assortment: Color/Size Changes
                    if (this.SizeCodeRID < hsb.SizeCodeRID) // Assortment: Color/Size Changes
					{
						return -1;
					}
					else
					{
						return 1;
					}
				}
				else if (this.SizeSequence < hsb.SizeSequence)
				{
					return -1;
				}
				else
				{
					return 1;
				}
			}
			else
			{
				return 1;
			}
		}
		#endregion

//End Track #4302 - JScott - Size Codes in wrong order afer release
		#region SizeCodeRID
		/// <summary>
		/// Sets Size RID
		/// </summary>
		/// <param name="aSizeCodeRID">RID that identifies the size in the Size Code table</param>
        internal void SetSizeCodeRID(int aSizeCodeRID) // Assortment: Color/Size Change
		{
			//if (SizeKey != aSizeRID)  // Assortment: Color/Size Change
            if (SizeCodeRID != aSizeCodeRID) // Assorment: Color/Size Change
			{
				//_SizeKey = aSizeRID; // Assortment: Color/Size Change
                _SizeCodeRID = aSizeCodeRID;
				SetSizeMultiple(1);
				this.SizeIsChanged = true;
			}
			this.SetSizeMaximum(_minMaxBin.LargestMaximum);
            if (_origSizeCodeRID == Include.NoRID)// Assortment: Color/Size Change
            {
                _origSizeCodeRID = _SizeCodeRID; // Assortment: Color/Size Change
            }
		}

        /// <summary>
        /// Sets Size Code RID
        /// </summary>
        /// <param name="aSizeCodeRID">RID that identifies the size</param>
        internal void ResetSizeRID(int aSizeCodeRID) // Assortment: Color/Size Change
        {
            if (_SizeCodeRID == Include.NoRID)// Assortment: Color/Size Change
            {
                throw new MIDException(eErrorLevel.severe,
                    (int)(eMIDTextCode.msg_al_MustCreateSizeBeforeResetting),
                    MIDText.GetText(eMIDTextCode.msg_al_MustCreateSizeBeforeResetting));
            }
            if (_SizeCodeRID != aSizeCodeRID)// Assortment: Color/Size Change
            {
                _SizeCodeRID = aSizeCodeRID;// Assortment: Color/Size Change
                this.SizeIsChanged = true;
            }
        }
		#endregion SizeCodeRID

		#region SizeUnitsToAllocate
		/// <summary>
		/// Sets the size units to allocate
		/// </summary>
		/// <param name="aUnitsToAllocate">Units to allocate value.</param>
		internal void SetSizeUnitsToAllocate(int aUnitsToAllocate)
		{
			_sizeBin.SetQtyToAllocate(aUnitsToAllocate);
		}
		#endregion SizeUnitsToAllocate

		#region SizeUnitsAllocated
		/// <summary>
		/// Sets size units allocated
		/// </summary>
		/// <param name="aUnitsAllocated">Units allocated</param>
		internal void SetSizeUnitsAllocated(int aUnitsAllocated)
		{
			_sizeBin.SetQtyAllocated(aUnitsAllocated);
		}
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Sets size item units allocated
        /// </summary>
        /// <param name="aUnitsAllocated">Units allocated</param>
        internal void SetSizeItemUnitsAllocated(int aUnitsAllocated)
        {
            _sizeBin.SetItemQtyAllocated(aUnitsAllocated);
        }

        // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        ///// <summary>
        ///// Sets size IMO units allocated
        ///// </summary>
        ///// <param name="aUnitsAllocated">Units allocated</param>
        //internal void SetSizeImoUnitsAllocated(int aUnitsAllocated)
        //{
        //    _sizeBin.SetImoQtyAllocated(aUnitsAllocated);
        //}
        // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1
		#endregion SizeUnitsAllocated

		#region SizeMultiple
		/// <summary>
		/// Sets Size Multiple
		/// </summary>
		/// <param name="aMultiple">Multiple value</param>
		internal void SetSizeMultiple(int aMultiple)
		{
			_sizeBin.SetUnitMultiple(aMultiple);
		}
		#endregion SizeMultiple

		#region SizeMinimum
		/// <summary>
		/// Sets Size Minimum		
		/// </summary>
		/// <param name="aMinimum">Minimum value.</param>
		internal void SetSizeMinimum(int aMinimum)
		{
			if (SizeMinimum != aMinimum)
			{
				_minMaxBin.SetMinimum(aMinimum);
				this.SizeIsChanged = true;
			}
		}
		#endregion SizeMinimum

		#region Size Maximum
		/// <summary>
		/// Sets size maximum								
		/// </summary>
		/// <param name="aMaximum">Maximum value</param>
		internal void SetSizeMaximum (int aMaximum)
		{
			if (this.SizeMaximum != aMaximum)
			{
				_minMaxBin.SetMaximum(aMaximum);
				this.SizeIsChanged = true;
			}
		}
		#endregion SizeMaximum

		#region SizeReserveUnits
		/// <summary>
		/// Sets Size Reserve
		/// </summary>
		/// <param name="aReserveUnits">Reserve quantity.</param>
		internal void SetReserveUnits(int aReserveUnits)
		{
			if (aReserveUnits < 0)
			{
				throw new MIDException(eErrorLevel.warning,
					(int)eMIDTextCode.msg_ReserveQtyCannotBeNeg,
					MIDText.GetText(eMIDTextCode.msg_ReserveQtyCannotBeNeg));
			}
			if (ReserveUnits != aReserveUnits)
			{
				_reserveUnits = aReserveUnits;
				this.SizeIsChanged = true;
			}
		}
		#endregion SizeReserveUnits

		#region SizeBinContentCompare
		/// <summary>
		/// Compares ths size bin with another to determine if they both contain the same content.
		/// </summary>
		/// <param name="aSizeBin">Size Bin to which this size bin will be compared.</param>
		/// <returns>True when equal content; false when different content.</returns>
		public bool SizeBinContentEqual (HdrSizeBin aSizeBin)
		{
			//if (this.SizeKey == aSizeBin.SizeKey) // Assortment: Color/Size Changes
            if (this.SizeCodeRID == aSizeBin.SizeCodeRID) // Assortment: Color/Size Changes
			{
				if (this.SizeUnitsToAllocate != aSizeBin.SizeUnitsToAllocate)
				{
					return false;
				}
			}
			else
			{
				return false;
			}
			return true;
		}
		#endregion SizeBinContentCompare

		#region Store
		#region Store Dimension
		/// <summary>
		/// Sets store count and number of store dimensions.
		/// </summary>
		/// <param name="aStoreCount">Number of stores</param>
		/// <param name="aSubtotal">True: Indicates subtotal; False:  indicates not subtotal</param>
		internal void SetStoreDimension(int aStoreCount, bool aSubtotal)
		{
			if (aStoreCount < 1)
			{
				throw new MIDException (eErrorLevel.fatal,
					(int)eMIDTextCode.msg_NumberOfStoresCannotBeLessThan1,
					MIDText.GetText(eMIDTextCode.msg_NumberOfStoresCannotBeLessThan1));
			}
			else
			{
				_storeSizeAllocated = new StoreSizeAllocated[aStoreCount];
				_storeSizeAllocated.Initialize();
                _storeLocked = new bool[aStoreCount]; // TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
                _storeTempLock = new bool[aStoreCount];    // TT#59 Implement Temp Locks
                _storeItemTempLock = new bool[aStoreCount];    // TT#1401 - JEllis - Urban Rerservation Stores pt 2
                for (int i = 0; i < aStoreCount; ++i)
				{
					_storeSizeAllocated[i].StoreSizeIsChanged = false;
					_storeSizeAllocated[i].StoreSizeIsNew = false;
					_storeSizeAllocated[i].AllDetailAuditFlags = 0;
					_storeSizeAllocated[i].AllShipStatusFlags = 0;
					_storeSizeAllocated[i].StoreSizeUnitMinimum = 0;
					_storeSizeAllocated[i].StoreSizeUnitsAllocated = 0;
					_storeSizeAllocated[i].StoreSizeUnitsAllocatedByAuto = 0;
					_storeSizeAllocated[i].StoreSizeUnitsShipped = 0;
                    // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
                    _storeSizeAllocated[i].StoreSizeItemUnitsAllocated = 0;
                    // end TT#1401 - JEllis - Urban Reservation Stores pt 1
					if (aSubtotal)
					{
						//Subtotals are initialized in reverse to non-totals.
						_storeSizeAllocated[i].StoreSizeUnitMinimum =
							_storeSizeAllocated[i].StoreSizeLargestMaximum;
					}
					else
					{
						_storeSizeAllocated[i].StoreSizeUnitMaximum =
							_storeSizeAllocated[i].StoreSizeLargestMaximum;
						_storeSizeAllocated[i].StoreSizeUnitPrimaryMaximum =
							_storeSizeAllocated[i].StoreSizeLargestMaximum;
                        //_storeSizeAllocated[i].StoreSizeLocked =  // TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
                        _storeLocked[i] =                           // TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
							this.Color.GetStoreLocked(i);
						_storeSizeAllocated[i].StoreSizeOut =
							this.Color.GetStoreOut(i);
						//_storeSizeAllocated[i].StoreSizeTempLock =    // TT#59 Implement Temp Lock
                        _storeTempLock[i] =                             // TT#59 Implement Temp Lock
							this.Color.GetStoreTempLock(i);
                        // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
                        _storeItemTempLock[i] =                            
                            this.Color.GetStoreItemTempLock(i);
                        // end TT#1401 - JEllis - Urban Reservation Stores pt 2
                    }
				}
			}
		}
		#endregion StoreDimension

		#region AuditFlags
		/// <summary>
		/// Gets all detail audit flags simultaneously for the indicated store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Ushort value each of whose bits represent a single flag setting.</returns>
        //internal ushort GetAllDetailAuditFlags (int aStoreIndex) // TT#488 - MD - Jellis - Group Allocation
        internal uint GetAllDetailAuditFlags(int aStoreIndex) // TT#488 - MD - Jellis - Group Allocation
		{
			return _storeSizeAllocated[aStoreIndex].AllDetailAuditFlags;
		}

		/// <summary>
		/// Sets all detail audit flags simultaneously for the indicated store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <param name="aFlags">Flag values</param>
        //internal void SetAllDetailAuditFlags (int aStoreIndex, ushort aFlags) // TT#488 - MD - Jellis - Group Allocation
		internal void SetAllDetailAuditFlags (int aStoreIndex, uint aFlags)     // TT#488 - MD - Jellis - Group Allocation
		{
			_storeSizeAllocated[aStoreIndex].AllDetailAuditFlags = aFlags;
		}
		#endregion AuditFlags
	
		#region StoreIsChanged
		//==================//
		// Store Is Changed //
		//==================//
		/// <summary>
		/// Gets StoreSizeIsChanged for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True: store size has changed; False: store size has not changed.</returns>
		internal bool GetStoreSizeIsChanged (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeIsChanged;
		}

		/// <summary>
		/// Sets StoreSizeIsChanged to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: store size has changed; False: store size has not changed.</param>
		internal void SetStoreSizeIsChanged (int aStoreIndex, bool aFlagValue)
		{
			_storeSizeAllocated[aStoreIndex].StoreSizeIsChanged = aFlagValue;
		}
		#endregion StoreIsChanged

		#region StoreIsNew
		//==================//
		// Store Is New //
		//==================//
		/// <summary>
		/// Gets StoreSizeIsNew for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True: store size is New; False: store size is not New.</returns>
		internal bool GetStoreSizeIsNew (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeIsNew;
		}

		/// <summary>
		/// Sets StoreSizeIsNew to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: store size is New; False: store size is not New.</param>
		internal void SetStoreSizeIsNew (int aStoreIndex, bool aFlagValue)
		{
			_storeSizeAllocated[aStoreIndex].StoreSizeIsNew = aFlagValue;
		}
		#endregion StoreIsNew

		#region StoreIsManuallyAllocated
		//=============================//
		// Store Is Manually Allocated //
		//=============================//
		/// <summary>
		/// Gets StoreSizeIsManuallyAllocated for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's Size allocation has been manually specified by the user.</returns>
		public bool GetStoreSizeIsManuallyAllocated (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeIsManuallyAllocated;
		}

		/// <summary>
		/// Sets StoreSizeIsManuallyAllocated to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStoreSizeIsManuallyAllocated (int aStoreIndex, bool aFlagValue)
		{
			_storeSizeAllocated[aStoreIndex].StoreSizeIsManuallyAllocated = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
        /// <summary>
        /// Gets StoreSizeItemIsManuallyAllocated for the specified store
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>True when the store's Size Item allocation has been manually specified by the user.</returns>
        public bool GetStoreSizeItemIsManuallyAllocated(int aStoreIndex)              // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F      
        {
            return _storeSizeAllocated[aStoreIndex].StoreSizeItemIsManuallyAllocated; // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        }

        /// <summary>
        /// Sets StoreSizeItemIsManuallyAllocated to specified value for specified store.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <param name="aFlagValue">Flag value</param>
        internal void SetStoreSizeItemIsManuallyAllocated(int aStoreIndex, bool aFlagValue)  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        {
            _storeSizeAllocated[aStoreIndex].StoreSizeItemIsManuallyAllocated = aFlagValue;  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
            this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 2

		#endregion StoreIsManuallyAllocated

		#region StoreHadNeed
		//================//
		// Store Had Need //
		//================//
		/// <summary>
		/// Gets StoreSizeHadNeed for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store had size need.</returns>
		public bool GetStoreSizeHadNeed (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeHadNeed;
		}

		/// <summary>
		/// Sets StoreSizeHadNeed to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True when the store had size need</param>
		internal void SetStoreSizeHadNeed (int aStoreIndex, bool aFlagValue)
		{
			_storeSizeAllocated[aStoreIndex].StoreSizeHadNeed = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreHadNeed

		#region StoreWasAutoAllocated
		//==========================//
		// Store Was Auto Allocated //
		//==========================//
		/// <summary>
		/// Gets StoreSizeWasAutoAllocated for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's Size allocation has been manually specified by the user.</returns>
		public bool GetStoreSizeWasAutoAllocated (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeWasAutoAllocated;
		}

		/// <summary>
		/// Sets StoreSizeWasAutoAllocated to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStoreSizeWasAutoAllocated (int aStoreIndex, bool aFlagValue)
		{
			_storeSizeAllocated[aStoreIndex].StoreSizeWasAutoAllocated = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreWasAutoAllocated

		#region StoreOut
		//===========//
		// Store Out //
		//===========//	
		/// <summary>
		/// Gets StoreSizeOut for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's Size allocation has been manually specified by the user.</returns>
		public bool GetStoreSizeOut (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeOut;
		}

		/// <summary>
		/// Sets StoreSizeOut to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStoreSizeOut (int aStoreIndex, bool aFlagValue)
		{
			_storeSizeAllocated[aStoreIndex].StoreSizeOut = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreOut

		#region StoreLocked
		//==============//
		// Store Locked //
		//==============//	
		/// <summary>
		/// Gets StoreSizeLocked for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's Size allocation has been manually specified by the user.</returns>
		public bool GetStoreSizeLocked (int aStoreIndex)
		{
            //return _storeSizeAllocated[aStoreIndex].StoreSizeLocked; // TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
            return _storeLocked[aStoreIndex];                          // TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
		}

		/// <summary>
		/// Sets StoreSizeLocked to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStoreSizeLocked (int aStoreIndex, bool aFlagValue)
		{
            // begin TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
            //_storeSizeAllocated[aStoreIndex].StoreSizeLocked = aFlagValue;
            //this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
            _storeLocked[aStoreIndex] = aFlagValue;
            // end TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
		}
		#endregion StoreLocked

		#region StoreTempLock
		//================//
		// Store TempLock //
		//================//	
		/// <summary>
		/// Gets StoreSizeTempLock for the specified store and size
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's Size allocation has been manually specified by the user.</returns>
		public bool GetStoreSizeTempLock (int aStoreIndex)
		{
			//return _storeSizeAllocated[aStoreIndex].StoreSizeTempLock;  // TT#59 Implement Temp Locks
            return _storeTempLock[aStoreIndex];                           // TT#59 Implement Temp Locks
		}

		/// <summary>
		/// Sets StoreSizeTempLock to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStoreSizeTempLock (int aStoreIndex, bool aFlagValue)
		{
            // begin TT#59 Implement Temp Locks
			//_storeSizeAllocated[aStoreIndex].StoreSizeTempLock = aFlagValue;
			//this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
            _storeTempLock[aStoreIndex] = aFlagValue;
            // end TT#59 Implement Temp Locks
		}

        // begin TT#1401 - JEllis - Reservation Stores pt 2
        /// <summary>
        /// Gets StoreSizeItemTempLock for the specified store and size
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>True when the store's Size Item allocation has been manually specified by the user.</returns>
        public bool GetStoreSizeItemTempLock(int aStoreIndex)
        {
            return _storeItemTempLock[aStoreIndex];      
        }

        /// <summary>
        /// Sets StoreSizeItemTempLock to specified value for specified store.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <param name="aFlagValue">Flag value</param>
        internal void SetStoreSizeItemTempLock(int aStoreIndex, bool aFlagValue)
        {
            _storeItemTempLock[aStoreIndex] = aFlagValue;
        }
        // end TT#1401 - JEllis - Reservation Stores pt 2

        // begin TT#59 Implement Temp Locks
        /// <summary>
        /// Reset Store Temp Lock to false for all stores
        /// </summary>
        internal void ResetTempLocks()
        {
            if (_storeTempLock != null)
            {
                _storeTempLock = new bool[this.StoreDimensions];
                _storeItemTempLock = new bool[this.StoreDimensions]; // TT#1401 - JEllis - Reservation Stores pt 2
            }
        }
        // end TT#59 Implement Temp Locks
		#endregion StoreTempLock

		#region StoreFilledSizeHole
		//========================//
		// Store Filled Size Hole //
		//========================//	
		/// <summary>
		/// Gets StoreSizeFilledSizeHole audit flag for this store.
		/// </summary>
		/// <param name="aStoreIndex">RID identifying this store.</param>
		/// <returns>True indicates size units were allocated to fill a size "hole" based on size curves.</returns>
		public bool GetStoreSizeFilledSizeHole (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeFilledSizeHole;
		}

		/// <summary>
		/// Sets StoreSizeFilledSizeHole audit flag to the specified value for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">RID identifying the store.</param>
		/// <param name="aFlagValue">Flag value.</param>
		internal void SetStoreSizeFilledSizeHole (int aStoreIndex, bool aFlagValue)
		{
			_storeSizeAllocated[aStoreIndex].StoreSizeFilledSizeHole = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreFilledSizeHole

		#region StoreAllocationFromBottomUpSize
		//========================//
		// Store Allocation From Bottom Up Size //
		//========================//	
		/// <summary>
		/// Gets StoreSizeAllocationFromBottomUpSize audit flag for this store.
		/// </summary>
		/// <param name="aStoreIndex">RID identifying this store.</param>
		/// <returns>True indicates size units were allocated to fill a size "hole" based on size curves.</returns>
		public bool GetStoreSizeAllocationFromBottomUpSize (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeAllocationFromBottomUpSize;
		}

		/// <summary>
		/// Sets StoreSizeAllocationFromBottomUpSize audit flag to the specified value for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">RID identifying the store.</param>
		/// <param name="aFlagValue">Flag value.</param>
		internal void SetStoreSizeAllocationFromBottomUpSize (int aStoreIndex, bool aFlagValue)
		{
			_storeSizeAllocated[aStoreIndex].StoreSizeAllocationFromBottomUpSize = aFlagValue;
		}
		#endregion StoreAllocationFromBottomUpSize

		#region StoreAllocationFromBulkSizeBreakOut
		//========================//
		// Store AllocationFromBulkSizeBreakOut //
		//========================//	
		/// <summary>
		/// Gets StoreSizeAllocationFromBulkSizeBreakOut audit flag for this store.
		/// </summary>
		/// <param name="aStoreIndex">RID identifying this store.</param>
		/// <returns>True indicates size units were allocated to fill a size "hole" based on size curves.</returns>
		public bool GetStoreSizeAllocationFromBulkSizeBreakOut (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeAllocationFromBulkSizeBreakOut;
		}

		/// <summary>
		/// Sets StoreSizeAllocationFromBulkSizeBreakOut audit flag to the specified value for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">RID identifying the store.</param>
		/// <param name="aFlagValue">Flag value.</param>
		internal void SetStoreSizeAllocationFromBulkSizeBreakOut (int aStoreIndex, bool aFlagValue)
		{
			_storeSizeAllocated[aStoreIndex].StoreSizeAllocationFromBulkSizeBreakOut = aFlagValue;
		}
		#endregion StoreAllocationFromBulkSizeBreakOut

		#region StoreAllocationModifiedAfterMultiHeaderSplit
		//====================================================//
		// Store Allocation Modified After Multi Header Split //
		//====================================================//
		/// <summary>
		/// Gets Store Allocation Modified After Multi Header Split Audit Flag for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's Size allocation was changed after a multi header split.</returns>
		public bool GetStoreAllocationModifiedAfterMultiHeaderSplit (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeAllocationModifiedAfterMultiHeaderSplit;
		}

		/// <summary>
		/// Sets Store Allocation Modified After Multi Header Split Audit Flag to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStoreAllocationModifiedAfterMultiHeaderSplit (int aStoreIndex, bool aFlagValue)
		{
			_storeSizeAllocated[aStoreIndex].StoreSizeAllocationModifiedAfterMultiHeaderSplit = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreAllocationModifiedAfterMultiHeaderSplit


		#region StoreSizePctToColorTotal
		//=========================================//
		// Store Size percent to store Color Total //
		//=========================================//
		/// <summary>
		/// Gets Store Size Percent to Color Total 
		/// </summary>
		/// <param name="aStoreIndex">Store Index that identifies this store.</param>
		/// <returns>Store's size allocation percent of store's total color allocation</returns>
		public double GetStoreSizePctToColor(int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizePctToColorTotal;
		}

		/// <summary>
		/// Sets Store Size Percent to Color Total 
		/// </summary>
		/// <param name="aStoreIndex">Store Index that identifies this store</param>
		/// <param name="aPercentageValue">Store's size allocation percent of store's total color allocation.</param>
		internal void SetStoreSizePctToColor (int aStoreIndex, double aPercentageValue)
		{
			_storeSizeAllocated[aStoreIndex].StoreSizePctToColorTotal= aPercentageValue;
		}
		#endregion StoreTotalSizePctToColorTotal

		#region StoreUnitsAllocated
		//=======================//
		// Store Units Allocated //
		//=======================//	
		/// <summary>
		/// Gets StoreSizeUnitsAllocated for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>Units allocated to the specified store in this size.</returns>
		public int GetStoreSizeUnitsAllocated (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeUnitsAllocated;
		}

		/// <summary>
		/// Sets StoreSizeUnitsAllocated to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aSizeUnitsAllocated">Size units allocated</param>
		internal void SetStoreSizeUnitsAllocated (int aStoreIndex, int aSizeUnitsAllocated)
		{
			SetSizeUnitsAllocated(SizeUnitsAllocated - _storeSizeAllocated[aStoreIndex].StoreSizeUnitsAllocated);
			_storeSizeAllocated[aStoreIndex].StoreSizeUnitsAllocated = aSizeUnitsAllocated;
			SetSizeUnitsAllocated(SizeUnitsAllocated + _storeSizeAllocated[aStoreIndex].StoreSizeUnitsAllocated);
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreQtyAllocatedStructure(), true); // MID Track 3994 Performance
		    this._sizeBin.CalcStoresWithAllocationCount = true;
		}
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets StoreSizeItemUnitsAllocated for the specified store
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>Item Units allocated to the specified store in this size.</returns>
        public int GetStoreSizeItemUnitsAllocated(int aStoreIndex)
        {
            return _storeSizeAllocated[aStoreIndex].StoreSizeItemUnitsAllocated;
        }

        /// <summary>
        /// Sets StoreSizeItemUnitsAllocated to specified value for specified store.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <param name="aSizeUnitsAllocated">Size Item units allocated</param>
        internal void SetStoreSizeItemUnitsAllocated(int aStoreIndex, int aSizeUnitsAllocated)
        {
            SetSizeItemUnitsAllocated(SizeItemUnitsAllocated - _storeSizeAllocated[aStoreIndex].StoreSizeItemUnitsAllocated);
            _storeSizeAllocated[aStoreIndex].StoreSizeItemUnitsAllocated = aSizeUnitsAllocated;
            SetSizeItemUnitsAllocated(SizeItemUnitsAllocated + _storeSizeAllocated[aStoreIndex].StoreSizeItemUnitsAllocated);
            this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreItemQtyAllocatedStructure(), true); // TT#1401 - JEllis - Urban Reservation Stores pt 4
            //this._sizeBin.CalcStoresWithAllocationCount = true; // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        }
        /// <summary>
        /// Gets StoreSizeImoUnitsAllocated for the specified store
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>IMO Units allocated to the specified store in this size.</returns>
        public int GetStoreSizeImoUnitsAllocated(int aStoreIndex)
        {
            return _storeSizeAllocated[aStoreIndex].StoreSizeImoUnitsAllocated;
        }

        // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        ///// <summary>
        ///// Sets StoreSizeImoUnitsAllocated to specified value for specified store.
        ///// </summary>
        ///// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        ///// <param name="aSizeUnitsAllocated">Size IMO units allocated</param>
        //internal void SetStoreSizeImoUnitsAllocated(int aStoreIndex, int aSizeUnitsAllocated)
        //{
        //    SetSizeImoUnitsAllocated(SizeImoUnitsAllocated - _storeSizeAllocated[aStoreIndex].StoreSizeImoUnitsAllocated);
        //    _storeSizeAllocated[aStoreIndex].StoreSizeImoUnitsAllocated = aSizeUnitsAllocated;
        //    SetSizeImoUnitsAllocated(SizeImoUnitsAllocated + _storeSizeAllocated[aStoreIndex].StoreSizeImoUnitsAllocated);
        //    this._sizeBin.CalcStoresWithAllocationCount = true;
        //}
        // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        // end TT31401 - JEllis - Urban Reservation Stores pt 1
		#endregion StoreUnitsAllocated

		#region StoreOrigUnitsAllocated
		//=======================//
		// Store Units Allocated //
		//=======================//	
		/// <summary>
		/// Gets StoreSizeOrigUnitsAllocated for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>Original Units allocated to the specified store in this size.</returns>
		public int GetStoreSizeOrigUnitsAllocated (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeOrigUnitsAllocated;
		}

		/// <summary>
		/// Sets StoreSizeOrigUnitsAllocated to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aSizeOrigUnitsAllocated">Size units allocated</param>
		internal void SetStoreSizeOrigUnitsAllocated (int aStoreIndex, int aSizeOrigUnitsAllocated)
		{
			_storeSizeAllocated[aStoreIndex].StoreSizeOrigUnitsAllocated = aSizeOrigUnitsAllocated;
		}
		#endregion StoreOrigUnitsAllocated


		#region StoreMaximum
		//===============//
		// Store Maximum //
		//===============//	
		/// <summary>
		/// Gets Store Size Maximum allocation for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Size Maximum allocation for the specified store</returns>
		public int GetStoreSizeMaximum (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeUnitMaximum;
		}

		/// <summary>
		/// Sets Store Size Maximum allocation for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aMaximum">Maximum Size allocation value</param>
		internal void SetStoreSizeMaximum (int aStoreIndex, int aMaximum)
		{
			_storeSizeAllocated[aStoreIndex].StoreSizeUnitMaximum = aMaximum;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreMaximumStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreMaximum

		#region StoreMinimum
		//===============//
		// Store Minimum //
		//===============//
		/// <summary>
		/// Gets Store Size Minimum allocation for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Size Minimum allocation for the specified store</returns>
		public int GetStoreSizeMinimum (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeUnitMinimum;
		}
		
		/// <summary>
		/// Sets Store Size Minimum allocation for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aMinimum">Minimum Size allocation value</param>
		internal void SetStoreSizeMinimum (int aStoreIndex, int aMinimum)
		{
			_storeSizeAllocated[aStoreIndex].StoreSizeUnitMinimum = aMinimum;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreMinimumStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreMinimum

		#region StorePrimaryMaximum
		//=======================//
		// Store Primary Maximum //
		//=======================//
		/// <summary>
		/// Gets Store Size Primary Maximum allocation for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Size Maximum allocation for the specified store</returns>
		public int GetStoreSizePrimaryMaximum (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeUnitPrimaryMaximum;
		}
		
		/// <summary>
		/// Sets Store Size Primary Maximum allocation for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aPrimaryMaximum">Primary Maximum Size allocation value</param>
		internal void SetStoreSizePrimaryMaximum (int aStoreIndex, int aPrimaryMaximum)
		{
			_storeSizeAllocated[aStoreIndex].StoreSizeUnitPrimaryMaximum = aPrimaryMaximum;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStorePrimaryMaxStructure(), true); // MID Track 3994 Performance
		}
		#endregion StorePrimaryMaximum

		#region StoreUnitsAllocatedByAuto
		//===============================//
		// Store Units Allocated By Auto //
		//===============================//
		/// <summary>
		/// Gets StoreSizeUnitsAllocatedByAuto for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Size units allocated by an auto allocation process.</returns>
		public int GetStoreSizeUnitsAllocatedByAuto (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeUnitsAllocatedByAuto;
		}

		/// <summary>
		/// Sets StoreSizeUnitsAllocatedByAuto for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aSizeUnitsAllocated">Size units allocated by auto allocation process.</param>
		internal void SetStoreSizeUnitsAllocatedByAuto (int aStoreIndex, int aSizeUnitsAllocated)
		{
			_storeSizeAllocated[aStoreIndex].StoreSizeUnitsAllocatedByAuto = aSizeUnitsAllocated;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreQtyAllocatedByAutoStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreUnitsAllocatedByAuto

		#region StoreShippingStarted
		/// <summary>
		/// Gets all ship flags simultaneously for the indicated store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>byte value each of whose bits represent a single flag setting.</returns>
		internal byte GetAllShipFlags (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].AllShipStatusFlags;
		}

		/// <summary>
		/// Sets all ship flags simultaneously for the indicated store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <param name="aFlags">Flag values</param>
		internal void SetAllShipFlags (int aStoreIndex, byte aFlags)
		{
			_storeSizeAllocated[aStoreIndex].AllShipStatusFlags = aFlags;
		}
	
		//========================//
		// Store Shipping Started //
		//========================//
		/// <summary>
		/// Gets Store Size Shipping Started audit flag for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True if shipping has started for this store.</returns>
		public bool GetStoreSizeShippingStarted (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeShippingStarted;
		}

		/// <summary>
		/// Sets Store Size Shipping Started audit flag for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		/// <returns></returns>
		internal void SetStoreSizeShippingStarted(int aStoreIndex, bool aFlagValue)
		{
			_storeSizeAllocated[aStoreIndex].StoreSizeShippingStarted = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreShippedStatusFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreShippingStarted

		#region StoreShippingComplete
		//=========================//
		// Store Shipping Complete //
		//=========================//
		/// <summary>
		/// Gets Store Size Shipping Complete audit flag for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True if shipping has Complete for this store.</returns>
		public bool GetStoreSizeShippingComplete (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeShippingComplete;
		}

		/// <summary>
		/// Sets Store Size Shipping Complete audit flag for the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		/// <returns></returns>
		internal bool SetStoreSizeShippingComplete(int aStoreIndex, bool aFlagValue)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeShippingComplete = aFlagValue;
            //this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreShippedStatusFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreShippingComplete

		#region StoreUnitsShipped
		//=====================//
		// Store Units Shipped //
		//=====================//
		/// <summary>
		/// Gets StoreSizeUnitsShipped to the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Size units shipped to the specified store.</returns>
		public int GetStoreSizeUnitsShipped (int aStoreIndex)
		{
			return _storeSizeAllocated[aStoreIndex].StoreSizeUnitsShipped;
		}

		/// <summary>
		/// Sets StoreSizeUnitsShipped to the specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aSizeUnitsShipped">Size units shipped to the specified store.</param>
		internal void SetStoreSizeUnitsShipped (int aStoreIndex, int aSizeUnitsShipped)
		{
			_storeSizeAllocated[aStoreIndex].StoreSizeUnitsShipped = aSizeUnitsShipped;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreQtyShippedStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreUnitsShipped

        // begin TT#246 - MD - JEllis - AnF VSW In-Store Minimum
        #region StoreItemMinimum
        /// <summary>
        /// Gets Store Size Item Minimum for the specified store.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
        /// <returns>Size Item Minimum for the specified store</returns>
        public int GetStoreItemMinimum(int aStoreIndex)
        {
            return _storeSizeAllocated[aStoreIndex].StoreSizeItemMinimum;
        }

        /// <summary>
        /// Sets Store Size Item Minimum for the specified store.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <param name="aMinimum">Size Item Minimum value</param>
        internal void SetStoreItemMinimum(int aStoreIndex, int aMinimum)
        {
            _storeSizeAllocated[aStoreIndex].StoreSizeItemMinimum = aMinimum;
            this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreItemMinimumStructure(), true);
        }
        // begin TT#246 - MD - JEllis - AnF VSW In Store Minimum phase 2
        /// <summary>
        /// Gets Store Size Item Ideal Minimum for the specified store.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
        /// <returns>Size Item Ideal Minimum for the specified store</returns>
        public int GetStoreItemIdealMinimum(int aStoreIndex)
        {
            return _storeSizeAllocated[aStoreIndex].StoreSizeItemIdealMinimum;
        }

        /// <summary>
        /// Sets Store Size Item Ideal Minimum for the specified store.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <param name="aMinimum">Size Item Ideal Minimum value</param>
        internal void SetStoreItemIdealMinimum(int aStoreIndex, int aMinimum)
        {
            _storeSizeAllocated[aStoreIndex].StoreSizeItemIdealMinimum = aMinimum;
            this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreItemMinimumStructure(), true);
        }
        // end TT#246 - MD - JEllis - AnF VSW In STore Minimum phase 2
        #endregion StoreItemMinimum
        // end TT#246 - MD - JEllis - AnF VSW In-Store Minimum
		#endregion Store

        // Removed Commented code.
		#endregion Methods
	}
	#endregion HdrSizeBin

	#region HdrSizeBinProcesOrder
	/// <summary>
	/// Used to determine the order in which to process multiple HdrSizeBins
	/// </summary>
	public class HdrSizeBinProcessOrder:IComparer
	{
		public int Compare(object x, object y)
		{
			if (!(x is HdrSizeBin) | !(y is HdrSizeBin))
			{
				throw new MIDException(eErrorLevel.severe,
					(int)(eMIDTextCode.msg_al_CompareObjectMustBeHdrSizeBin),
					MIDText.GetText(eMIDTextCode.msg_al_CompareObjectMustBeHdrSizeBin));
			}
			if (x == null || y == null)
			{
				if (y != null)
				{
					return -1;
				}
				if (x != null)
				{
					return +1;
				}
				return 0;
			}
			int xUnits = (((HdrSizeBin)x).SizeUnitsToAllocate
				- ((HdrSizeBin)x).SizeUnitsAllocated);
			int yUnits = (((HdrSizeBin)y).SizeUnitsToAllocate
				- ((HdrSizeBin)y).SizeUnitsAllocated);
			if (xUnits < yUnits)
			{
				return -1;
			}
			if (xUnits > yUnits)
			{
				return +1;
			}
			if (((HdrSizeBin)x).SizeMultiple < ((HdrSizeBin)y).SizeMultiple)
			{
				return -1;
			}
			if (((HdrSizeBin)x).SizeMultiple > ((HdrSizeBin)y).SizeMultiple)
			{
				return +1;
			}
			return -1;
		}
	}
	#endregion HdrSizeBinProcessOrder

	#region HdrColorBin
	/// <summary>
	/// Container for bulk Pack units on an allocation header.
	/// </summary>
//Begin Track #4302 - JScott - Size Codes in wrong order afer release
//	public class HdrColorBin
	public class HdrColorBin : IComparable
//End Track #4302 - JScott - Size Codes in wrong order afer release
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private int _colorCodeRID;
		private int _origColorCodeRID;
		private int _masterOrSubordinateColorCodeRID; // MID Track 4029 ReWork MasterPO Logic
        private int _colorNodeRID;
        private int _hdrRID;
        private HdrAllocationBin _colorBin;
		private int _totalSizeUnitsToAllocate;
		private MinMaxAllocationBin _minMaxBin;
		private int _reserveUnits;
		private Hashtable _size;
		private StoreAllocationStructureStatus _sass; // MID Track 3994 Performance
		private StoreColorAllocated[] _storeColorAllocated;
        private bool[] _storeLocked;                 // TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
        private bool[] _storeTempLock;             // TT#59 Implement Temp Lock
        private bool[] _storeItemTempLock;             // TT#1401 - JEllis - Reservation Stores pt 2
        // BEGIN MID Change j.ellis Ability to show size need in size view
		private int _sizeCurveGroupRID;              
		private eMerchandiseType _sizeCurveMerchType; 
		private int _sizeCurveMerchHnRID;
		private int _sizeCurveMerchPhRID;
		private int _sizeCurveMerchPhlSeq;
		private int _sizeConstraintRID;
		private int _sizeAlternateRID;
		private bool _normalizeSizeCurves; // MID Track 4861 Size Curve Normalization
        // begin TT#41 - MD - JEllis - UC#2 - Size Inventory Min Max pt 1
        private eMerchandiseType _sizeCurveIbMerchType;
        private int _sizeCurveIbMerchHnRID;
        private int _sizeCurveIbMerchPhRID;
        private int _sizeCurveIbMerchPhlSeq;
        // end TT#41 - MD - JEllis - UC#2 - Size Inventory Min Max pt 1
        //private bool _recalcSizePlans;  // TT#519 - MD - Jellis - VSW - Size Minimums not working
		// END MID Change j.ellis Ability to show size need in size view
		// begin MID Track 3634 Add display sequence for size
		private int _maxSizeSequence;
		private int _firstSizeSequence;
		// end MID track 3634 Add display sequence for size
        private int      _hdrBCRID;         // Assortment BEGIN
        private string   _name;              
        private string   _description;
        private int      _asrtBCRID;
        private int _lastBCSZ_KeyUsed;  // Assortment END
        private eVSWSizeConstraints _vswSizeConstraints; // TT#246 - MD - JEllis - VSW Size In Store Minimums pt 2
        private ColorStatusFlags _colorStatusFlags;      // TT#246 - MD - Jellis - VSW Size In STore Minimums pt 5
        private eFillSizesToType _FillSizesToType; //TT#848-MD -jsobek -Fill to Size Plan Presentation
        private bool _storeEligibilityLoaded = false;
        #endregion

        #region Constructors
        //=============
        // CONSTRUCTORS
        //=============
        /// <summary>
        /// Used to construct an instance of this class.
        /// </summary>
        public HdrColorBin ()
		{
			_colorCodeRID = Include.NoRID;
			_origColorCodeRID = Include.NoRID;
			_masterOrSubordinateColorCodeRID = Include.NoRID; // MID Track 4029 ReWork MasterPO Logic
            _colorNodeRID = Include.NoRID;
            _hdrRID = Include.NoRID;
            _colorBin.SetQtyAllocated(0);
			_colorBin.SetQtyToAllocate(0);
			_colorBin.SetUnitMultiple(1);
            // begin TT#1401 -  Urban Reservation Stores pt 1
            _colorBin.SetItemQtyAllocated(0);
            //_colorBin.SetImoQtyAllocated(0);  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
            // end TT#1401 - Urban Reservation Stores pt 1
			_colorBin.Sequence = 0;
			_colorBin.IsChanged = false;
			_colorBin.IsNew = false;
			_totalSizeUnitsToAllocate = 0;
			_minMaxBin.SetMaximum(_minMaxBin.LargestMaximum);
			_minMaxBin.SetMinimum(0);
			_reserveUnits = 0;
			_size = null;
			_storeColorAllocated = null;
			// BEGIN MID Change j.ellis Ability to show size need in size view
			_sizeCurveGroupRID = Include.NoRID;              
			_sizeCurveMerchPhRID = Include.NoRID;
			_sizeCurveMerchPhlSeq = 0;
			_sizeConstraintRID = Include.NoRID;
			_sizeAlternateRID = Include.NoRID;
			_normalizeSizeCurves = true; // MID Track 4861 Size Curve Normalization
            // begin TT#41 - MD - JEllis - UC#2 - Size Inventory Min Max pt 1
            _sizeCurveIbMerchType = eMerchandiseType.Undefined;
            _sizeCurveIbMerchHnRID = Include.NoRID;
            _sizeCurveIbMerchPhRID = Include.NoRID;
            _sizeCurveIbMerchPhlSeq = 0;
            // end TT#41 - MD - JEllis - UC#2 - Size Inventory Min Max pt 1
            //_recalcSizePlans = false;  // TT#519 - MD - Jellis - VSW - Size Minimums not working
			// END MID Change j.ellis Ability to show size need in size view
			// begin MID 3634 Add display sequence for size
			_maxSizeSequence = 0;
			_firstSizeSequence = -1;
			// end MID 3634 Add display sequence for size
			this._sass = new StoreAllocationStructureStatus(false);  // MID Track 3994 Performance
			_colorBin.StoresWithAllocationCount = 0; // MID Track 4448 AnF Audit Enhancement
		    _colorBin.CalcStoresWithAllocationCount = false; // MID Track 4448 AnF Audit Enhancement
            // Assortment BEGIN
            _hdrBCRID = Include.NoRID;
            _name = null;
            _description = null;
            _lastBCSZ_KeyUsed = 0;
            // Assortment END
            _vswSizeConstraints = eVSWSizeConstraints.None; // TT#246 - MD - JEllis - VSW Size In Store Minimums pt 2
            _colorStatusFlags.AllFlags = 0;                 // TT#246 - MD - Jellis - AnF VSW Size In Store MInimums pt 5
	}
        #endregion

        #region Properties
        //===========
        // PROPERTIES
        //===========
        /// <summary>
        /// Gets _storeEligibilityLoaded flag
        /// </summary>
        internal bool StoreEligibilityLoaded
        {
            get
            {
                return _storeEligibilityLoaded;
            }
            set
            {
                _storeEligibilityLoaded = value;
            }
        }

        /// <summary>
        /// Gets or sets IsChanged database update flag
        /// </summary>
        internal bool ColorIsChanged
		{
			get
			{
				return _colorBin.IsChanged;
			}
			set
			{
				_colorBin.IsChanged = value;
			}
		}

		/// <summary>
		/// Gets or sets IsNew database update flag
		/// </summary>
		internal bool ColorIsNew
		{
            // begin TT#488 - MD -Jellis - Group ALlocation
            get { return (_hdrBCRID < 0); }
            //get 
            //{
            //    return _colorBin.IsNew;
            //}
            //set
            //{
            //    _colorBin.IsNew = value;
            //}
            // end TT#488 - MD - Jellis _ Group Allocation
		}
        /// <summary>
        /// Gets the HDR_BC_RID
        /// </summary>
        public int HdrBCRID
        {
            get
            {
                return _hdrBCRID;
            }
        }

		/// <summary>
		/// Gets the Color Key
		/// </summary>
		public int ColorCodeRID
		{
			get
			{
				return _colorCodeRID;
			}
		}

		/// <summary>
		/// Gets the original Color Key as it existed at create time
		/// </summary>
		internal int OriginalColorCodeRID
		{
			get
			{
				return _origColorCodeRID;
			}
		}

        internal int ColorNodeRID
        {
            get
            {
                return _colorNodeRID;
            }
            set
            {
                _colorNodeRID = value;
            }
        }

        /// <summary>
        /// Gets or sets the HdrRID associated with this pack
        /// </summary>
        /// <remarks>Identifies the home Header for this pack</remarks>
        public int HdrRID
        {
            get
            {
                return _hdrRID;
            }
            set
            {
                _hdrRID = value;
            }
        }

        // begin MID Track 4029 ReWork MasterPO Logic
        internal int MasterOrSubordinateColorCodeRID
		{
			get
			{
				return _masterOrSubordinateColorCodeRID;
			}
			set
			{
				_masterOrSubordinateColorCodeRID = value;
			}
		}
		// end MID Track 4029 ReWork MasterPO Logic
		/// <summary>
		/// Gets the bulk color units to allocate
		/// </summary>
		public int ColorUnitsToAllocate
		{
			get
			{
				return _colorBin.QtyToAllocate;
			}
		}

		/// <summary>
		/// Gets the bulk color units allocated.
		/// </summary>
		public int ColorUnitsAllocated
		{
			get
			{
				return _colorBin.QtyAllocated;
			}
		}

        // begin TT#1401 - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets the bulk color Item units allocated.
        /// </summary>
        public int ColorItemUnitsAllocated
        {
            get
            {
                return _colorBin.ItemQtyAllocated;
            }
        }
        /// <summary>
        /// Gets the bulk color IMO units allocated.
        /// </summary>
        public int ColorImoUnitsAllocated
        {
            get
            {
                return _colorBin.ImoQtyAllocated;
            }
        }
        // end TT#1401 - Urban Reservation Stores pt 1

		/// <summary>
		/// Gets the bulk color multiple.
		/// </summary>
		public int ColorMultiple
		{
			get
			{
				return _colorBin.UnitMultiple;
			}
		}

		/// <summary>
		/// Gets or sets color sequence.
		/// </summary>
		public int ColorSequence
		{
			get
			{
				return _colorBin.Sequence;
			}
			set
			{
				_colorBin.Sequence = value;
			}
		}

		/// <summary>
		/// Gets the allocation color minimum.
		/// </summary>
		public int ColorMinimum
		{
			get
			{
				return _minMaxBin.Minimum;
			}
		}

		/// <summary>
		/// Gets the allocation color maximum.
		/// </summary>
		public int ColorMaximum
		{
			get
			{
				return _minMaxBin.Maximum;
			}
		}

		/// <summary>
		/// Gets ReserveUnits.
		/// </summary>
		public int ReserveUnits
		{
			get
			{
				return _reserveUnits;
			}
		}

		/// <summary>
		/// Gets the size content of the color.
		/// This property was added by DAT
		/// </summary>
		public Hashtable ColorSizes
		{
			get
			{
				if (_size == null)
				{
					CreateSizeHash();
				}
				return _size;
			}
		}

		/// <summary>
		/// Returns largest possible maximum
		/// </summary>
		internal int LargestMaximum
		{
			get
			{
				return _minMaxBin.LargestMaximum;
			}
		}

		/// <summary>
		/// Gets the total size units to allocate in the color. 
		/// </summary>
		public int TotalSizeUnitsToAllocate
		{
			get 
			{
				return _totalSizeUnitsToAllocate;
			}
		}

		/// <summary>
		/// Gets the number of dimensions in the store allocation array.
		/// </summary>
		public int StoreDimensions
		{
			get 
			{
				return _storeColorAllocated.Length;
			}
		}

		// BEGIN MID Change j.ellis Ability to show size need in size view
		/// <summary>
		/// Gets or sets the Size Curve Group key associated with this color.
		/// </summary>
		internal int SizeCurveGroupKey
		{
			get
			{
				return this._sizeCurveGroupRID;
			}
			set
			{
				this._sizeCurveGroupRID = value;
			}
		}
		/// <summary>
		/// Gets or sets the Size Curve Merchandise Type associated with this color.
		/// </summary>
		internal eMerchandiseType SizeCurveMerchandiseType
		{
			get
			{
				return this._sizeCurveMerchType;
			}
			set
			{
				this._sizeCurveMerchType = value;
			}
		}
		/// <summary>
		/// Gets or sets the Size Curve Size Merchandise Key associated with this color.
		/// </summary>
		internal int SizeCurveMerchandiseKey
		{
			get
			{
				return this._sizeCurveMerchHnRID;
			}
			set
			{
				this._sizeCurveMerchHnRID = value;
			}
		}
		/// <summary>
		/// Gets or sets the Size Curve Merchandise PH key associated with this color.
		/// </summary>
		internal int SizeCurveMerchPhRID
		{
			get
			{
				return this._sizeCurveMerchPhRID;
			}
			set
			{
				this._sizeCurveMerchPhRID = value;
			}
		}
		/// <summary>
		/// Gets or sets the Size Curve Merchandise PHL Seq associated with this color.
		/// </summary>
		internal int SizeCurveMerchPhlSeq
		{
			get
			{
				return this._sizeCurveMerchPhlSeq;
			}
			set
			{
				this._sizeCurveMerchPhlSeq = value;
			}
		}
		/// <summary>
		/// Gets or sets the Size Curve Size Constraint Key associated with this color.
		/// </summary>
		internal int SizeConstraintKey
		{
			get
			{
				return this._sizeConstraintRID;
			}
			set
			{
				this._sizeConstraintRID = value;
			}
		}
		/// <summary>
		/// Gets or sets the Size Curve Size Alternate Key associated with this color.
		/// </summary>
		internal int SizeAlternateKey
		{
			get
			{
				return this._sizeAlternateRID;
			}
			set
			{
				this._sizeAlternateRID = value;
			}
		}
	
		// begin MID Track 4861 Size Curve Normalization
		/// <summary>
		/// Gets or sets the BOOL NormalizeSizeCurves
		/// </summary>
		internal bool NormalizeSizeCurves
		{
			get
			{
				return this._normalizeSizeCurves;
			}
			set
			{
				this._normalizeSizeCurves = value;
			}
		}
		// end MID Track 4861 Size Curve Normalization
        // begin TT#41 - MD - JEllis - UC#2 - Size Inventory Min Max pt 1
        internal eMerchandiseType SizeCurveIbMerchandiseType
        {
            get
            {
                return _sizeCurveIbMerchType;
            }
            set
            {
                _sizeCurveIbMerchType = value;
            }
        }
        internal int SizeCurveIbMerchandiseKey
        {
            get
            {
                return _sizeCurveIbMerchHnRID;
            }
            set
            {
                _sizeCurveIbMerchHnRID = value;
            }
        }
        internal int SizeCurveIbMerchPhRID
        {
            get
            {
                return _sizeCurveIbMerchPhRID;
            }
            set
            {
                _sizeCurveIbMerchPhRID = value;
            }
        }
        internal int SizeCurveIbMerchPhlSeq
        {
            get
            {
                return _sizeCurveIbMerchPhlSeq;
            }
            set
            {
                _sizeCurveIbMerchPhlSeq = value;
            }
        }
        // end TT#41 - MD - JEllis - UC#2 - Size Inventory Min Max pt 1

        // begin TT#246 - MD - JEllis - VSW Size In Store Minimums pt2
        internal eVSWSizeConstraints VSWSizeConstraints
        {
            get
            {
                return _vswSizeConstraints;
            }
            set
            {
                _vswSizeConstraints = value;
            }
        }
        // end TT#246 - MD - JEllis - VSW Size in Store Minimums pt 2
        //Begin TT#848-MD -jsobek -Fill to Size Plan Presentation
        internal eFillSizesToType FillSizesToType 
        {
            get
            {
                return _FillSizesToType;
            }
            set
            {
                _FillSizesToType = value;
            }
        }
        //End TT#848-MD -jsobek -Fill to Size Plan Presentation

        // begin   TT#519 - MD - Jellis - VSW - Size Minimums not working
        ///// <summary>
        ///// Gets or sets the Recalc Size Plans flag.
        ///// </summary>
        //internal bool RecalcSizePlans
        //{
        //    get
        //    {
        //        return this._recalcSizePlans;
        //    }
        //    set
        //    {
        //        this._recalcSizePlans = value;
        //    }
        //}
        // end   TT#519 - MD - Jellis - VSW - Size Minimums not working
		// END MID Change j.ellis Ability to show size need in size view		}
        // begin TT#246 - MD - Jellis - AnF VSW In Store Minimum pt 5
        /// <summary>
        /// Gets or sets the CalcVswSizeConstraints Flag
        /// </summary>
        internal bool CalcVswSizeConstraints
        {
            get
            {
                return _colorStatusFlags.CalcVswSizeConstraints;
            }
            set
            {
                _colorStatusFlags.CalcVswSizeConstraints = value;
            }
        }
        internal uint ColorStatusFlags
        {
            get { return _colorStatusFlags.AllFlags; }
        }
        // end TT#246 - MD - Jellis - AnF VSW In Store Minimum pt 5
		// begin MID Track 3994 Performance
		internal StoreAllocationStructureStatus AllocationStructureStatus
		{
			get
			{
				return this._sass;
			}
			set 
			{
				_sass = value;
			}
		}
		// end MID Track 3994 Performance

        // Asortment BEGIN
        /// <summary>
        /// Gets or sets the color name for placeholders.
        /// </summary>
        public string ColorName
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }
        /// <summary>
        /// Gets or sets the color description for placeholders.
        /// </summary>
        public string ColorDescription
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }
        /// <summary>
        /// Gets or sets the corresponding placeholder key associated with this color.
        /// </summary>
        public int AsrtBCRID
        {
            get
            {
                return this._asrtBCRID;
            }
            set
            {
                this._asrtBCRID = value;
            }
        }
		//begin MID Track 4448 AnF Audit Enhancement
		/// <summary>
		/// Gets Count of stores with an allocation
		/// </summary>
		public int StoresWithAllocationCount
		{
			get
			{
				if (_colorBin.CalcStoresWithAllocationCount)
				{
					_colorBin.StoresWithAllocationCount = 0;
					for (int i=0; i < this._storeColorAllocated.Length; i++)
					{
						if (this.GetStoreUnitsAllocated(i) > 0)
						{
							_colorBin.StoresWithAllocationCount++;
						}
					}
					_colorBin.CalcStoresWithAllocationCount = false;
				}
				return _colorBin.StoresWithAllocationCount;
			}
		}
		// end MID Track 4448 AnF Audit Enhancement
        public int Last_BCSZ_Key_Used
        {
            get
            {
                return _lastBCSZ_KeyUsed;
            }
            set
            {
                if (value > -1)
                {
                    _lastBCSZ_KeyUsed = value;
                }
            }
        }
		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========
        #region HdrBCRID
        /// <summary>
        /// Sets HdrBC_RID
        /// </summary>
        /// <param name="aBCRID">database RID that identifies the color</param>
        internal void SetHdrBCRID(int aBCRID)
        {
            if (HdrBCRID != aBCRID)
            {
                _hdrBCRID = aBCRID;
                this.ColorIsChanged = true;
            }
            
        }
        #endregion HdrBC_RID
//Begin Track #4302 - JScott - Size Codes in wrong order afer release
		#region IComparable Members
		public int CompareTo(object obj)
		{
			HdrColorBin hcb;

			if (obj.GetType() == typeof(HdrColorBin))
			{
				hcb = (HdrColorBin)obj;
				if (this.ColorSequence == hcb.ColorSequence)
				{
					if (this.ColorCodeRID < hcb.ColorCodeRID)
					{
						return -1;
					}
					else
					{
						return 1;
					}
				}
				else if (this.ColorSequence < hcb.ColorSequence)
				{
					return -1;
				}
				else
				{
					return 1;
				}
			}
			else
			{
				return 1;
			}
		}
		#endregion

//End Track #4302 - JScott - Size Codes in wrong order afer release

        // begin TT#246 - MD - Jellis - AnF VSW In STore Minimums pt 5
        #region ColorStatusFlags
        internal void SetColorStatusFlags(ColorStatusFlags aColorStatusFlags)
        {
            _colorStatusFlags = aColorStatusFlags;
        }
        #endregion ColorStatusFlags
        // end TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5
		#region ColorCodeRID
		/// <summary>
		/// Sets Color Key
		/// </summary>
		/// <param name="aColorRID">RID that identifies the color</param>
		internal void SetColorCodeRID(int aColorRID)
		{
			if (ColorCodeRID != aColorRID)
			{
				_colorCodeRID = aColorRID;
				SetColorMultiple(1);
				this.ColorIsChanged = true;
			}
			this.SetColorMaximum(_minMaxBin.LargestMaximum);
			if (_origColorCodeRID == Include.NoRID)
			{
				_origColorCodeRID = _colorCodeRID;
			}
		}

		/// <summary>
		/// Resets Color Key
		/// </summary>
		/// <param name="aColorRID">RID that identifies the color</param>
		internal void ResetColorCodeRID(int aColorRID)
		{
			if (_colorCodeRID == Include.NoRID)
			{
				throw new MIDException(eErrorLevel.severe,
					(int)(eMIDTextCode.msg_al_MustCreateColorBeforeResetting),
					MIDText.GetText(eMIDTextCode.msg_al_MustCreateColorBeforeResetting));
			}
			if (_colorCodeRID != aColorRID)
			{
				_colorCodeRID = aColorRID;
				this.ColorIsChanged = true;
			}
		}
		#endregion ColorCodeRID

		#region UnitsToAllocate
		//=========================//
		// Color Units To Allocate //
		//=========================//
		/// <summary>
		/// Sets Bulk Color units to allocate
		/// </summary>
		/// <param name="aColorUnitsToAllocate"></param>
		internal void SetColorUnitsToAllocate(int aColorUnitsToAllocate)
		{
			_colorBin.SetQtyToAllocate(aColorUnitsToAllocate);
		}
		#endregion UnitsToAllocate

		#region UnitsAllocated
		//=======================//
		// Color Units Allocated //
		//=======================//
		/// <summary>
		/// Sets bulk color Units Allocated
		/// </summary>
		/// <param name="aColorUnitsAllocated">Units allocated in the color.</param>
		internal void SetColorUnitsAllocated(int aColorUnitsAllocated)
		{
			_colorBin.SetQtyAllocated(aColorUnitsAllocated);
		}
        // begin TT#1401 - Urban Reservation Stores pt 1
        /// <summary>
        /// Sets bulk color Item Units Allocated
        /// </summary>
        /// <param name="aColorItemUnitsAllocated">Item Units allocated in the color.</param>
        internal void SetColorItemUnitsAllocated(int aColorItemUnitsAllocated)
        {
            _colorBin.SetItemQtyAllocated(aColorItemUnitsAllocated);
        }

        // begin TT#1401 - Urban Virtual Store Warehouse pt 29
        ///// <summary>
        ///// Sets bulk color IMO Units Allocated
        ///// </summary>
        ///// <param name="aColorImoUnitsAllocated">IMO Units allocated in the color.</param>
        //internal void SetColorImoUnitsAllocated(int aColorImoUnitsAllocated)
        //{
        //    _colorBin.SetImoQtyAllocated(aColorImoUnitsAllocated);
        //}
        // end TT#1401 - Urban Virtual Store Warehouse pt 29
        // end TT#1401 - Urban Reservation Stores pt 1
		#endregion UnitsAllocated

		#region ColorMultiple
		//================//
		// Color Multiple //
		//================//
		/// <summary>
		/// Sets bulk color multiple
		/// </summary>
		/// <param name="aKey">Color RID.</param>
		internal void SetColorMultiple(int aKey)
		{
			_colorBin.SetUnitMultiple(aKey);
		}
		#endregion ColorMultiple

		#region ColorMinimum
		//===============//
		// Color Minimum //
		//===============//
		/// <summary>
		/// Set allocation color minimum
		/// </summary>
		/// <param name="aMinimum">Minimum value.</param>
		internal void SetColorMinimum (int aMinimum)
		{
			if (ColorMinimum != aMinimum)
			{
				_minMaxBin.SetMinimum(aMinimum);
				this.ColorIsChanged = true;
			}
		}
		#endregion ColorMinimum

		#region ColorMaximum
		//===============//
		// Color Maximum //
		//===============//
		/// <summary>
		/// Sets allocation color maximum
		/// </summary>
		/// <param name="aMaximum">Maximum value.</param>
		internal void SetColorMaximum(int aMaximum)
		{
			if (ColorMaximum != aMaximum)
			{
				_minMaxBin.SetMaximum(aMaximum);
				this.ColorIsChanged = true;
			}
		}
		#endregion ColorMaximum

		#region ColorReserveUnits
		//=====================//
		// Color Reserve Units //
		//=====================//
		/// <summary>
		/// Sets Reserve Units
		/// </summary>
		/// <param name="aReserveUnits">Reserve units value.</param>
		internal void SetReserveUnits(int aReserveUnits)
		{
			if (aReserveUnits < 0)
			{
				throw new MIDException(eErrorLevel.warning,
					(int)eMIDTextCode.msg_ReserveQtyCannotBeNeg,
					MIDText.GetText(eMIDTextCode.msg_ReserveQtyCannotBeNeg));
			}
			if (ReserveUnits != aReserveUnits)
			{
				_reserveUnits = aReserveUnits;
				this.ColorIsChanged = true;
			}
		}
		#endregion ColorReserveUnits

		#region ColorContentCompare
		//=======================//
		// Color Content Compare //
		//=======================//
		/// <summary>
		/// Compares this color bin to another color bin to determine if they have the same content.
		/// </summary>
		/// <param name="aColorBin">Color Bin to which this color bin is to be compared.</param>
		/// <param name="aVerifySize">True indicates size content must be equal to return an equal condition; false indicates  size content is irrelevant.</param>
		/// <returns>True if both bins contain the same content; false if they contain different content</returns>
		public bool ColorBinContentEqual(HdrColorBin aColorBin, bool aVerifySize)
		{
			if (this.ColorCodeRID == aColorBin.ColorCodeRID)
			{
				if (this.ColorUnitsToAllocate == aColorBin.ColorUnitsToAllocate)
				{
					if (aVerifySize)
					{
						foreach (HdrSizeBin sizeBin in this.ColorSizes.Values)
						{
							//if (aColorBin.ColorSizes.Contains(sizeBin.SizeKey)) // Assortment: Color/Size changes
                            if (aColorBin.ColorSizes.Contains(sizeBin.SizeCodeRID)) // Assortment: color/size changes
							{
                                // Begin Assortment: Color/size changes
								//if (!(sizeBin.SizeBinContentEqual((HdrSizeBin)aColorBin.ColorSizes[sizeBin.SizeKey])))
                                if (!(sizeBin.SizeBinContentEqual((HdrSizeBin)aColorBin.ColorSizes[sizeBin.SizeCodeRID]))) // Assortment: color/size changes
                                // End Assortment: Color/Size Changes
                                {
									return false;
								}
							}
							else
							{
								return false;
							}
						}
					}
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
			return true;
		}
		#endregion ColorContentCompare
        #region ColorName
        /// <summary>
        /// Sets color name  
        /// </summary>
        internal void SetColorName(string aName)
        {
            if (ColorName != aName)
            {
                _name = aName;
                this.ColorIsChanged = true;
            }
        }
        #endregion ColorName
		
        #region ColorDescription
        /// <summary>
        /// Sets color name  
        /// </summary>
        internal void SetColorDescription(string aDescription)
        {
            if (ColorDescription != aDescription)
            {
                _description = aDescription;
                this.ColorIsChanged = true;
            }
        }
        #endregion ColorDescription

        #region ColorAsrtBCRID
        /// <summary>
        /// Sets color name  
        /// </summary> 
        internal void SetAsrtBCRID(int asrtBCRID)
        {
            if (AsrtBCRID != asrtBCRID)
            {
                _asrtBCRID = asrtBCRID;
                this.ColorIsChanged = true;
            }
        }
        #endregion ColorAsrtBCRID

		#region Size
		#region CreateSizeHash
		/// <summary>
		/// Creates an instance of the bulk color-size hashtable.
		/// </summary>
		internal void CreateSizeHash()
		{
			_size = new Hashtable();
		}

		#endregion CreateSizeHash
		#region SizeIsInColorTest
		/// <summary>
		/// Indicates whether the specified size is defined in the color.
		/// </summary>
		/// <param name="aSizeRID">Database RID of the width-size</param>
		/// <returns>True if the size is in the color.</returns>
		public bool SizeIsInColor (int aSizeRID)
		{
			if (_size == null)
			{
				CreateSizeHash();
				return false;
			}
			else
			{
				return _size.Contains(aSizeRID);
			}
		}
		#endregion SizeIsInColorTest

		#region GetSizeBin
        // begin TT#1176 - MD - Jellis - Group Allocation - Size Need not observing inv min max
        /// <summary>
        /// Gets HdrSizeBin for the specified width-size.
        /// </summary>
        /// <param name="aSizeRID">Database RID for this width-size</param>
        /// <returns>HdrSizeBin for the requested width-size</returns>
        public HdrSizeBin GetSizeBin(int aSizeRID)
        {
            HdrSizeBin sizeBin;
            eMIDTextCode statusReasonCode;
            if (TryGetSizeBin(aSizeRID, out sizeBin, out statusReasonCode))
            {
                return sizeBin;
            }
            throw new MIDException(eErrorLevel.severe,
                (int)statusReasonCode,
                MIDText.GetText(statusReasonCode)
                + " : ColorCodeRID [" + _colorCodeRID.ToString(CultureInfo.CurrentUICulture) + "]"
                + " : Source/Method [" + GetType().Name + " / GetSizeBin]");
        }
        public bool TryGetSizeBin(int aSizeRID, out HdrSizeBin aSizeBin, out eMIDTextCode aStatusReasonCode)
        {
            aSizeBin = (HdrSizeBin)ColorSizes[aSizeRID];
            if (aSizeBin == null)
            {
                aStatusReasonCode = eMIDTextCode.msg_SizeNotDefinedInBulkColor;
                return false;
            }
            aStatusReasonCode = eMIDTextCode.msg_al_TryGetSuccessful;
            return true;
        }
        ///// <summary>
        ///// Gets HdrSizeBin for the specified width-size.
        ///// </summary>
        ///// <param name="aSizeRID">Database RID for this width-size</param>
        ///// <returns>HdrSizeBin for the requested width-size</returns>
        //public HdrSizeBin GetSizeBin(int aSizeRID)
        //{
        //    if (SizeIsInColor(aSizeRID))
        //    {
        //        return (HdrSizeBin)ColorSizes[aSizeRID];
        //    }
        //    else
        //    {
        //        throw new MIDException (eErrorLevel.severe,
        //            (int)eMIDTextCode.msg_SizeNotDefinedInBulkColor,
        //            MIDText.GetText(eMIDTextCode.msg_SizeNotDefinedInBulkColor));
        //    }
        //}
        // end TT#1176 - MD - Jellis- Group Allocation - Size Need not observing inv min max
		#endregion GetSizeBin

		#region AddSizeToColor
		/// <summary>
		/// Adds size content to a color.
		/// </summary>
		/// <param name="aSizeCodeRID">Database RID for the width-size</param>
		/// <param name="aSizeUnits">Total bulk units of this size within the color.</param>
		/// <param name="aSizeSequence">Display sequence for the size</param>
		internal HdrSizeBin AddSize(int aSizeCodeRID, int aSizeUnits, int aSizeSequence)  // Assortment: color/size changes
		{
            return AddSize(Include.NoRID, aSizeCodeRID, aSizeUnits, aSizeSequence);
        }
        internal HdrSizeBin AddSize(int aBCSZ_Key, int aSizeCodeRID, int aSizeUnits, int aSizeSequence)
        {
			HdrSizeBin _content;

			int newTotalSizeUnitsToAllocate = _totalSizeUnitsToAllocate;
            if (this.SizeIsInColor(aSizeCodeRID)) // Assortment: Color/Size change
			{
                _content = GetSizeBin(aSizeCodeRID); // Assortment: Color/Size change
				newTotalSizeUnitsToAllocate -= _content.SizeUnitsToAllocate;
			}
			else                   
			{
				//	        	_content = new HdrSizeBin(aColor);
				_content = new HdrSizeBin(this, aBCSZ_Key);  // Assortment: Color/size change (set new size to negative key value)
                _content.SetSizeCodeRID(aSizeCodeRID);  // Assortment: Color/Size change
                ColorSizes.Add(aSizeCodeRID, _content); // Assortment: Color/Size change
				_content.SizeIsNew = true;
			}
            if (_content.SizeCodeRID == aSizeCodeRID) // Assortment: Color/Size change
			{
				if (newTotalSizeUnitsToAllocate < 0)
				{
					throw new MIDException (eErrorLevel.warning,
						(int)eMIDTextCode.msg_AccumBulkSizeUnitsToAllocateCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_AccumBulkSizeUnitsToAllocateCannotBeNeg));
				}
				else
				{
					_content.SetSizeUnitsToAllocate(aSizeUnits);
					_totalSizeUnitsToAllocate = newTotalSizeUnitsToAllocate + _content.SizeUnitsToAllocate;
					// begin MID Track 3634 Add display sequence to size
                    // Begin TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
                    //if (aSizeSequence < 1 
                    //    || aSizeSequence == this._firstSizeSequence)
                    if (aSizeSequence < 1
                        || aSizeSequence == this._firstSizeSequence || SequenceExists(aSizeSequence))
                    // End TT#234 
					{
						//_content.SizeSequence = ColorSizes.Count;
						_content.SizeSequence = this._maxSizeSequence + 1;
					}
					else
					{
						_content.SizeSequence = aSizeSequence;
					}
					if (_content.SizeSequence > this._maxSizeSequence)
					{
						this._maxSizeSequence = _content.SizeSequence;
					}
					if (ColorSizes.Count == 1)
					{
						this._firstSizeSequence = aSizeSequence;
					}
					// end MID Track 3634 Add display sequence to size
				}				
			}
			else
			{
				throw new MIDException (eErrorLevel.warning,
					(int)eMIDTextCode.msg_BulkSizeKeyOutOfSync,
					MIDText.GetText(eMIDTextCode.msg_BulkSizeKeyOutOfSync));
			}
            return _content; // Assortment: color/size change
		}
        // Begin TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
        private bool SequenceExists(int aSizeSequence)
        {
            bool sizeSequenceExists = false;
            if (ColorSizes.Count > 0)
            {
                foreach (HdrSizeBin sizeBin in ColorSizes.Values)
                {
                    if (sizeBin.SizeSequence == aSizeSequence)
                    {
                        sizeSequenceExists = true;
                        break;
                    }
                }
            }
            return sizeSequenceExists;
        }
        // End TT#234  
		#endregion AddSizeToColor

		#region RemoveSizeFromColor
		/// <summary>
		/// Removes size content from a color.
		/// </summary>
		/// <param name="aSizeRID">Database RID for the width-size to remove</param>
		internal void RemoveSize(int aSizeRID)
		{
			RemoveSize(GetSizeBin(aSizeRID));
		}

		/// <summary>
		/// Removes size content from a color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the size.</param>
		internal void RemoveSize(HdrSizeBin aSize)
		{
			_totalSizeUnitsToAllocate -= aSize.SizeUnitsToAllocate;
            ColorSizes.Remove(aSize.SizeCodeRID);// Assortment: Color/Size change 
		}
		#endregion RemoveSizeFromColor

		#region SizeCodeRID
		/// <summary>
		/// Sets Size Code RID
		/// </summary>
		/// <param name="aSizeCodeRID">RID that identifies the old size code</param>
		/// <param name="aNewSizeCodeRID">RID that identifies the new size code</param>
		internal void SetColorSizeCodeRID(int aSizeCodeRID, int aNewSizeCodeRID)
		{
			HdrSizeBin aSize = GetSizeBin(aSizeCodeRID);
			if (SizeIsInColor(aNewSizeCodeRID))
			{
				throw new MIDException(eErrorLevel.warning,
					(int)(eMIDTextCode.msg_al_NewBulkColorSizeAlreadyExists),
					MIDText.GetText(eMIDTextCode.msg_al_NewBulkColorSizeAlreadyExists));
			}
			ColorSizes.Remove(aSizeCodeRID);
            aSize.ResetSizeRID(aNewSizeCodeRID); // Assortment: Color/Size change
			ColorSizes.Add(aNewSizeCodeRID, aSize);
		}

		/// <summary>
		/// Gets original size RID;
		/// </summary>
		/// <param name="aSizeCodeRID">RID of the size code </param>
		/// <returns>Original size code RID as it existed at create time.</returns>
		internal int GetOriginalSizeCodeRID (int aSizeCodeRID)
		{
            return (GetSizeBin(aSizeCodeRID)).OriginalSizeCodeRID; // Assortment: Color/Size change
		}
		#endregion SizeCodeRID

		#region SizeUnitsToAllocate
		/// <summary>
		/// Get units to allocate for this size.
		/// </summary>
		/// <param name="aSizeRID">Database RID for this width-size</param>
		/// <returns>Units to allocate for this size.</returns>
		public int GetSizeUnitsToAllocate(int aSizeRID)
		{
			return GetSizeUnitsToAllocate(GetSizeBin(aSizeRID));
		}

		/// <summary>
		/// Get units to allocate for this size.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this width-size</param>
		/// <returns>Units to allocate for this size.</returns>
		internal int GetSizeUnitsToAllocate(HdrSizeBin aSize)
		{
			return aSize.SizeUnitsToAllocate;
		}

		/// <summary>
		/// Set units to allocate for this size.
		/// </summary>
		/// <param name="aSizeRID">Database RID for this width-size</param>
		/// <param name="aUnitsToAllocate">Size Units To Allocate</param>
		public void SetSizeUnitsToAllocate(int aSizeRID, int aUnitsToAllocate)
		{
			SetSizeUnitsToAllocate(GetSizeBin(aSizeRID), aUnitsToAllocate);
		}

		/// <summary>
		/// Set units to allocate for this size.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this width-size</param>
		/// <param name="aUnitsToAllocate">Size Units To Allocate</param>
		internal void SetSizeUnitsToAllocate(HdrSizeBin aSize, int aUnitsToAllocate)
		{
			int difference =
				aUnitsToAllocate
				- GetSizeUnitsToAllocate(aSize);
			aSize.SetSizeUnitsToAllocate(aUnitsToAllocate);
			this._totalSizeUnitsToAllocate += difference;
		}
		#endregion SizeUnitsToAllocate

		#region SizeUnitsAllocated
		/// <summary>
		/// Get size units allocated for the specified size
		/// </summary>
		/// <param name="aSizeRID">Database RID for the width-size</param>
		/// <returns>Units allocated for the specified width-size</returns>
		public int GetSizeUnitsAllocated (int aSizeRID)
		{
			return GetSizeUnitsAllocated(GetSizeBin(aSizeRID));
		}

		/// <summary>
		/// Get size units allocated for the specified size
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the width-size</param>
		/// <returns>Units allocated for the specified width-size</returns>
		internal int GetSizeUnitsAllocated (HdrSizeBin aSize)
		{
			return aSize.SizeUnitsAllocated;
		}

		/// <summary>
		/// Adjusts total size units allocated to the stores.
		/// </summary>
		/// <param name="aSizeRID">Database RID for the width-size</param>
		/// <param name="aAdjValue">Adjust Value</param>
		internal void AdjustSizeUnitsAllocated (int aSizeRID, int aAdjValue)
		{
			SetSizeUnitsAllocated(aSizeRID, 
				GetSizeUnitsAllocated(aSizeRID)
				+ aAdjValue);
		}

		/// <summary>
		/// Set size units allocated for the specified size
		/// </summary>
		/// <param name="aSizeRID">Database RID for the width-size</param>
		/// <param name="aUnitsAllocated">Units allocated for the specified width-size</param>
		public void SetSizeUnitsAllocated (int aSizeRID, int aUnitsAllocated)
		{
			GetSizeBin(aSizeRID).SetSizeUnitsAllocated(aUnitsAllocated);
		}
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Get size item units allocated for the specified size
        /// </summary>
        /// <param name="aSizeRID">Database RID for the width-size</param>
        /// <returns>Item Units allocated for the specified width-size</returns>
        public int GetSizeItemUnitsAllocated(int aSizeRID)
        {
            return GetSizeItemUnitsAllocated(GetSizeBin(aSizeRID));
        }

        /// <summary>
        /// Get size item units allocated for the specified size
        /// </summary>
        /// <param name="aSize">HdrSizeBin that describes the width-size</param>
        /// <returns>Item Units allocated for the specified width-size</returns>
        internal int GetSizeItemUnitsAllocated(HdrSizeBin aSize)
        {
            return aSize.SizeItemUnitsAllocated;
        }

        /// <summary>
        /// Adjusts total size item units allocated to the stores.
        /// </summary>
        /// <param name="aSizeRID">Database RID for the width-size</param>
        /// <param name="aAdjValue">Adjust Value</param>
        internal void AdjustSizeItemUnitsAllocated(int aSizeRID, int aAdjValue)
        {
            SetSizeItemUnitsAllocated(aSizeRID,
                GetSizeItemUnitsAllocated(aSizeRID)
                + aAdjValue);
        }

        /// <summary>
        /// Set size item units allocated for the specified size
        /// </summary>
        /// <param name="aSizeRID">Database RID for the width-size</param>
        /// <param name="aItemUnitsAllocated">ItemUnits allocated for the specified width-size</param>
        public void SetSizeItemUnitsAllocated(int aSizeRID, int aItemUnitsAllocated)
        {
            GetSizeBin(aSizeRID).SetSizeItemUnitsAllocated(aItemUnitsAllocated);
        }


        /// <summary>
        /// Get size IMO units allocated for the specified size
        /// </summary>
        /// <param name="aSizeRID">Database RID for the width-size</param>
        /// <returns>IMO Units allocated for the specified width-size</returns>
        public int GetSizeImoUnitsAllocated(int aSizeRID)
        {
            return GetSizeImoUnitsAllocated(GetSizeBin(aSizeRID));
        }

        /// <summary>
        /// Get size IMO units allocated for the specified size
        /// </summary>
        /// <param name="aSize">HdrSizeBin that describes the width-size</param>
        /// <returns>IMO Units allocated for the specified width-size</returns>
        internal int GetSizeImoUnitsAllocated(HdrSizeBin aSize)
        {
            return aSize.SizeImoUnitsAllocated;
        }

        // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        ///// <summary>
        ///// Adjusts total size IMO units allocated to the stores.
        ///// </summary>
        ///// <param name="aSizeRID">Database RID for the width-size</param>
        ///// <param name="aAdjValue">Adjust Value</param>
        //internal void AdjustSizeImoUnitsAllocated(int aSizeRID, int aAdjValue)
        //{
        //    SetSizeImoUnitsAllocated(aSizeRID,
        //        GetSizeImoUnitsAllocated(aSizeRID)
        //        + aAdjValue);
        //}

        ///// <summary>
        ///// Set size IMO units allocated for the specified size
        ///// </summary>
        ///// <param name="aSizeRID">Database RID for the width-size</param>
        ///// <param name="aImoUnitsAllocated">IMO Units allocated for the specified width-size</param>
        //public void SetSizeImoUnitsAllocated(int aSizeRID, int aImoUnitsAllocated)
        //{
        //    GetSizeBin(aSizeRID).SetSizeImoUnitsAllocated(aImoUnitsAllocated);   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 14
        //} 
        // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        // end TT31401 - JEllis - Urban Reservation Stores pt 1
		#endregion SizeUnitsAllocated

		#region SizeMinimum
		/// <summary>
		/// Gets bulk size allocation minimum for the specified width-size.
		/// </summary>
		/// <param name="aSizeRID">Database RID for the width-size</param>
		/// <returns>Size allocation minimum for the specified width-size</returns>
		public int GetSizeMinimum (int aSizeRID)
		{
			return GetSizeMinimum(GetSizeBin(aSizeRID));
		}
		
		/// <summary>
		/// Gets bulk size allocation minimum for the specified width-size.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the width-size</param>
		/// <returns>Size allocation minimum for the specified width-size</returns>
		internal int GetSizeMinimum(HdrSizeBin aSize)
		{
			return aSize.SizeMinimum;
		}

		/// <summary>
		/// Sets bulk size allocation minimum for the specified width-size.
		/// </summary>
		/// <param name="aSizeRID">Database RID for the width-size</param>
		/// <param name="aMinimum">Minimum value.</param>
		internal void SetSizeMinimum (int aSizeRID, int aMinimum)
		{
			SetSizeMinimum (GetSizeBin(aSizeRID), aMinimum);
		}
	
		/// <summary>
		/// Sets bulk size allocation minimum for the specified width-size.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the width-size</param>
		/// <param name="aMinimum">Minimum value.</param>
		internal void SetSizeMinimum (HdrSizeBin aSize, int aMinimum)
		{
			aSize.SetSizeMinimum(aMinimum);
		}
		#endregion SizeMinimum

		#region SizeMaximum
		/// <summary>
		/// Gets bulk size allocation Maximum for the specified width-size.
		/// </summary>
		/// <param name="aSizeRID">Database RID for the width-size</param>
		/// <returns>Size allocation Maximum for the specified width-size</returns>
		public int GetSizeMaximum (int aSizeRID)
		{
			return GetSizeMaximum(GetSizeBin(aSizeRID));
		}
		
		/// <summary>
		/// Gets bulk size allocation Maximum for the specified width-size.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the width-size</param>
		/// <returns>Size allocation Maximum for the specified width-size</returns>
		internal int GetSizeMaximum(HdrSizeBin aSize)
		{
			return aSize.SizeMaximum;
		}

		/// <summary>
		/// Sets bulk size allocation Maximum for the specified width-size.
		/// </summary>
		/// <param name="aSizeRID">Database RID for the width-size</param>
		/// <param name="aMaximum">Maximum value.</param>
		internal void SetSizeMaximum (int aSizeRID, int aMaximum)
		{
			SetSizeMaximum (GetSizeBin(aSizeRID), aMaximum);
		}
	
		/// <summary>
		/// Sets bulk size allocation Maximum for the specified width-size.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the width-size</param>
		/// <param name="aMaximum">Maximum value.</param>
		internal void SetSizeMaximum (HdrSizeBin aSize, int aMaximum)
		{
			aSize.SetSizeMaximum(aMaximum);
		}
		#endregion SizeMaximum

		#region SizeMultiple
		/// <summary>
		/// Gets bulk size allocation Multiple for the specified width-size.
		/// </summary>
		/// <param name="aSizeRID">Database RID for the width-size</param>
		/// <returns>Size allocation Multiple for the specified width-size</returns>
		public int GetSizeMultiple (int aSizeRID)
		{
			return GetSizeMultiple(GetSizeBin(aSizeRID));
		}
		
		/// <summary>
		/// Gets bulk size allocation Multiple for the specified width-size.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the width-size</param>
		/// <returns>Size allocation Multiple for the specified width-size</returns>
		internal int GetSizeMultiple(HdrSizeBin aSize)
		{
			return aSize.SizeMultiple;
		}

		/// <summary>
		/// Sets bulk size allocation Multiple for the specified width-size.
		/// </summary>
		/// <param name="aSizeRID">Database RID for the width-size</param>
		/// <param name="aMultiple">Multiple value.</param>
		internal void SetSizeMultiple (int aSizeRID, int aMultiple)
		{
			SetSizeMultiple (GetSizeBin(aSizeRID), aMultiple);
		}
	
		/// <summary>
		/// Sets bulk size allocation Multiple for the specified width-size.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the width-size</param>
		/// <param name="aMultiple">Multiple value.</param>
		internal void SetSizeMultiple (HdrSizeBin aSize, int aMultiple)
		{
			aSize.SetSizeMultiple(aMultiple);
		}
		#endregion SizeMultiple
		#endregion Size

		#region Store
		#region StoreDimension
		/// <summary>
		/// Sets Store Dimension and store count
		/// </summary>
		/// <param name="aStoreCount">Number of stores</param>
		/// <param name="aSubtotal">True: Indicates this color is represents a subtotal; False:  indicates this color is not a subtotal.</param>
		internal void SetStoreDimension (int aStoreCount, bool aSubtotal)
		{
			if (aStoreCount < 1)
			{
				throw new MIDException (eErrorLevel.fatal,
					(int)eMIDTextCode.msg_NumberOfStoresCannotBeLessThan1,
					MIDText.GetText(eMIDTextCode.msg_NumberOfStoresCannotBeLessThan1));
			}
			else
			{
				_storeColorAllocated = new StoreColorAllocated[aStoreCount];
				_storeColorAllocated.Initialize();
                _storeLocked = new bool[aStoreCount];  // TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
                _storeTempLock = new bool[aStoreCount];  // TT#59 Implement Temp Locks
                _storeItemTempLock = new bool[aStoreCount];  // TT#1401 - JEllis - Urban Reservation Stores pt 2
                for (int i = 0; i < aStoreCount; ++i)
				{
					_storeColorAllocated[i].StoreColorIsChanged = false;
					_storeColorAllocated[i].StoreColorIsNew = false;
					_storeColorAllocated[i].AllDetailAuditFlags = 0;
					_storeColorAllocated[i].AllShipStatusFlags = 0;
					_storeColorAllocated[i].StoreColorUnitMinimum = 0;
					_storeColorAllocated[i].StoreColorUnitsAllocated = 0;
					_storeColorAllocated[i].StoreColorUnitsAllocatedByAuto = 0;
					_storeColorAllocated[i].StoreColorUnitsAllocatedByRule = 0;
					_storeColorAllocated[i].StoreColorUnitsShipped = 0;
					_storeColorAllocated[i].TotalSizeUnitsAllocated = 0;
                    // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
                    _storeColorAllocated[i].StoreColorItemUnitsAllocated = 0;
                    _storeColorAllocated[i].TotalSizeItemUnitsAllocated = 0;
                    _storeColorAllocated[i].TotalSizeItemUnitsManuallyAllocated = 0; // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
                    //_storeColorAllocated[i].StoreColorImoUnitsAllocated = 0;  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                    _storeColorAllocated[i].TotalSizeImoUnitsAllocated = 0;
                    // end TT#1401 - JEllis - Urban Reservation Stores pt 1
					if (aSubtotal)
					{
						// Subtotals are initialized in reverse to how a color
						// attached to a header is initialized.
						_storeColorAllocated[i].StoreColorUnitMinimum =
							_storeColorAllocated[i].LargestColorMaximum;
					}
					else
					{
						_storeColorAllocated[i].StoreColorUnitMaximum =
							_storeColorAllocated[i].LargestColorMaximum;
						_storeColorAllocated[i].StoreColorUnitPrimaryMaximum =
							_storeColorAllocated[i].LargestColorMaximum;
					}
					_storeColorAllocated[i].StoreColorChosenRuleType = eRuleType.None;
					_storeColorAllocated[i].StoreColorChosenRuleLayerID = Include.NoLayerID;
				}
				foreach (HdrSizeBin s in ColorSizes.Values)
				{
					s.SetStoreDimension(aStoreCount, aSubtotal);
				}
			}
		}
		#endregion StoreDimension

		#region StoreColorTotalSizeAllocated
		//==================================//
		// Store Color Total Size Allocated //
		//==================================//
		/// <summary>
		/// Gets store's total size allocation within this color
		/// </summary>
		/// <param name="aStoreIndex">Store Index that identifies this store.</param>
		/// <returns>Total size units allocated across all sizes within the color.</returns>
		public int GetStoreColorTotalSizeAllocated (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].TotalSizeUnitsAllocated;
		}

		/// <summary>
		/// Sets store's total size allocation within this color
		/// </summary>
		/// <param name="aStoreIndex">Store Index that identifies this store</param>
		/// <param name="aTotalAllocated">Total allocated across all sizes in the color.</param>
		internal void SetStoreColorTotalSizeAllocated (int aStoreIndex, int aTotalAllocated)
		{
			_storeColorAllocated[aStoreIndex].TotalSizeUnitsAllocated = aTotalAllocated;
		}
        // begin TT#1401 - Urban Reservation Stores
        /// <summary>
        /// Gets store's total size item allocation within this color
        /// </summary>
        /// <param name="aStoreIndex">Store Index that identifies this store.</param>
        /// <returns>Total size item units allocated across all sizes within the color.</returns>
        public int GetStoreColorTotalSizeItemAllocated(int aStoreIndex)
        {
            return _storeColorAllocated[aStoreIndex].TotalSizeItemUnitsAllocated;
        }

        // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        public int GetStoreColorTotalSizeItemManuallyAllocated(int aStoreIndex)
        {
            return _storeColorAllocated[aStoreIndex].TotalSizeItemUnitsManuallyAllocated;
        }
        public void SetStoreColorTotalSizeItemManuallyAllocated(int aStoreIndex, int aTotalItemManuallyAllocated)
        {
            _storeColorAllocated[aStoreIndex].TotalSizeItemUnitsManuallyAllocated = aTotalItemManuallyAllocated;
        }
        // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 284

        /// <summary>
        /// Sets store's total size Item allocation within this color
        /// </summary>
        /// <param name="aStoreIndex">Store Index that identifies this store</param>
        /// <param name="aTotalItemAllocated">Total Item allocated across all sizes in the color.</param>
        internal void SetStoreColorTotalSizeItemAllocated(int aStoreIndex, int aTotalItemAllocated)
        {
            _storeColorAllocated[aStoreIndex].TotalSizeItemUnitsAllocated = aTotalItemAllocated;
        }


        /// <summary>
        /// Gets store's total size IMO allocation within this color
        /// </summary>
        /// <param name="aStoreIndex">Store Index that identifies this store.</param>
        /// <returns>Total size IMO units allocated across all sizes within the color.</returns>
        public int GetStoreColorTotalSizeImoAllocated(int aStoreIndex)
        {
            return _storeColorAllocated[aStoreIndex].TotalSizeImoUnitsAllocated;
        }

        /// <summary>
        /// Sets store's total size IMO allocation within this color
        /// </summary>
        /// <param name="aStoreIndex">Store Index that identifies this store</param>
        /// <param name="aTotalImoAllocated">Total IMO allocated across all sizes in the color.</param>
        internal void SetStoreColorTotalSizeImoAllocated(int aStoreIndex, int aTotalImoAllocated)
        {
            _storeColorAllocated[aStoreIndex].TotalSizeImoUnitsAllocated = aTotalImoAllocated;
        }
        // end TT31401 - Urban Reservation Stores
		#endregion StoreColorTotalSizeAllocated

		#region ReCalcStoreTotalSizePctToColorTotal
		//===========================================================//
		// ReCalculate Store Total Size percent to store Color Total //
		//===========================================================//
		/// <summary>
		/// Gets ReCalculate Store Total Size Percent to Color Total Flag Value
		/// </summary>
		/// <param name="aStoreIndex">Store Index that identifies this store.</param>
		/// <returns>True: recalculate store's total size allocation percent to store color allocation</returns>
		public bool GetReCalcStoreTotalSizePctToColor(int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].ReCalcStoreSizePctToColorTotal;
		}

		/// <summary>
		/// Sets ReCalculate Store Total Size Percent to Color Total Flag Value
		/// </summary>
		/// <param name="aStoreIndex">Store Index that identifies this store</param>
		/// <param name="aFlagValue">True: recalculate store's total size allocation percent to store color allocation</param>
		internal void SetReCalcStoreTotalSizePctToColor (int aStoreIndex, bool aFlagValue)
		{
			_storeColorAllocated[aStoreIndex].ReCalcStoreSizePctToColorTotal= aFlagValue;
		}
		#endregion ReCalcStoreTotalSizePctToColorTotal

		#region StoreTotalSizePctToColorTotal
		//===============================================//
		// Store Total Size percent to store Color Total //
		//===============================================//
		/// <summary>
		/// Gets Store Total Size Percent to Color Total 
		/// </summary>
		/// <param name="aStoreIndex">Store Index that identifies this store.</param>
		/// <returns>Store's total size allocation percent of store's total color allocation</returns>
		public double GetStoreTotalSizePctToColor(int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreTotalSizePctToColorTotal;
		}

		/// <summary>
		/// Sets Store Total Size Percent to Color Total 
		/// </summary>
		/// <param name="aStoreIndex">Store Index that identifies this store</param>
		/// <param name="aPercentageValue">Store's total size allocation percent of store's total color allocation.</param>
		internal void SetStoreTotalSizePctToColor (int aStoreIndex, double aPercentageValue)
		{
			_storeColorAllocated[aStoreIndex].StoreTotalSizePctToColorTotal = aPercentageValue;
		}
		#endregion StoreTotalSizePctToColorTotal

		#region StoreUnitsAllocated
		//=======================//
		// Store Units Allocated //
		//=======================//
		/// <summary>
		/// Gets StoreUnitsAllocated for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>Store's Color Units allocated.</returns>
		public int GetStoreUnitsAllocated (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorUnitsAllocated;
		}

		/// <summary>
		/// Gets StoreUnitsAllocated for the specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>Store's Color Units allocated.</returns>
		public int GetStoreUnitsAllocated (int aSizeRID, int aStoreIndex)
		{
			return GetStoreUnitsAllocated (GetSizeBin(aSizeRID), aStoreIndex);
		}

		/// <summary>
		/// Gets StoreUnitsAllocated for the specified store and size in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>Store's Color Units allocated.</returns>
		internal int GetStoreUnitsAllocated (HdrSizeBin aSize, int aStoreIndex)
		{
			return aSize.GetStoreSizeUnitsAllocated(aStoreIndex);
		}

		/// <summary>
		/// Sets StoreUnitsAllocated to specified value for specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aUnitsAllocated">Color units allocated</param>
		internal void SetStoreUnitsAllocated (int aStoreIndex, int aUnitsAllocated)
		{
			this.SetColorUnitsAllocated(this.ColorUnitsAllocated - _storeColorAllocated[aStoreIndex].StoreColorUnitsAllocated);
			_storeColorAllocated[aStoreIndex].StoreColorUnitsAllocated = aUnitsAllocated;
            //RecalcSizePlans = true; // MID Change j.ellis allow size need etc. on size review  // TT#519 - MD - Jellis - VSW - Size Minimums not working
			this.SetColorUnitsAllocated(this.ColorUnitsAllocated + _storeColorAllocated[aStoreIndex].StoreColorUnitsAllocated);
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreQtyAllocatedStructure(), true); // MID Track 3994 Performance
		    _colorBin.CalcStoresWithAllocationCount = true;                            // MID Track 4448 AnF Audit Enhancement
		}

		/// <summary>
		/// Sets StoreUnitsAllocated to specified value for the specified store in the specified size of this color.
		/// </summary>
		/// <param name="aSizeRID">RID of the size.</param>
		/// <param name="aStoreIndex">Index value for the store.</param>
		/// <param name="aUnitsAllocated">Units allocated</param>
		internal void SetStoreUnitsAllocated (int aSizeRID, int aStoreIndex, int aUnitsAllocated)
		{
			SetStoreUnitsAllocated(GetSizeBin(aSizeRID), aStoreIndex, aUnitsAllocated);
		}

		/// <summary>
		/// Sets StoreUnitsAllocated to specified value for the specified store in the specified size of this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the size.</param>
		/// <param name="aStoreIndex">Index value for the store.</param>
		/// <param name="aUnitsAllocated">Units allocated</param>
		internal void SetStoreUnitsAllocated (HdrSizeBin aSize, int aStoreIndex, int aUnitsAllocated)
		{
			SetStoreColorTotalSizeAllocated(aStoreIndex,
				GetStoreColorTotalSizeAllocated(aStoreIndex) 
				- GetStoreUnitsAllocated(aSize, aStoreIndex));
			aSize.SetStoreSizeUnitsAllocated(aStoreIndex, aUnitsAllocated);
			SetStoreColorTotalSizeAllocated(aStoreIndex,
				GetStoreColorTotalSizeAllocated(aStoreIndex)
				+ GetStoreUnitsAllocated(aSize, aStoreIndex));
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
   		/// <summary>
		/// Gets StoreItemUnitsAllocated for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>Store's Color Item Units allocated.</returns>
		public int GetStoreItemUnitsAllocated (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorItemUnitsAllocated;
		}

		/// <summary>
		/// Gets StoreItemUnitsAllocated for the specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>Store's Color Item Units allocated.</returns>
		public int GetStoreItemUnitsAllocated (int aSizeRID, int aStoreIndex)
		{
			return GetStoreItemUnitsAllocated (GetSizeBin(aSizeRID), aStoreIndex);
		}

		/// <summary>
		/// Gets StoreItemUnitsAllocated for the specified store and size in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>Store's Color Item Units allocated.</returns>
		internal int GetStoreItemUnitsAllocated (HdrSizeBin aSize, int aStoreIndex)
		{
			return aSize.GetStoreSizeItemUnitsAllocated(aStoreIndex);
		}

		/// <summary>
		/// Sets StoreItemUnitsAllocated to specified value for specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aItemUnitsAllocated">Color item units allocated</param>
		internal void SetStoreItemUnitsAllocated (int aStoreIndex, int aItemUnitsAllocated)
		{
			this.SetColorItemUnitsAllocated(this.ColorItemUnitsAllocated - _storeColorAllocated[aStoreIndex].StoreColorItemUnitsAllocated);
			_storeColorAllocated[aStoreIndex].StoreColorItemUnitsAllocated = aItemUnitsAllocated;
            //RecalcSizePlans = true; // MID Change j.ellis allow size need etc. on size review  // TT#519 - MD - Jellis - VSW - Size Minimums not working
			this.SetColorItemUnitsAllocated(this.ColorItemUnitsAllocated + _storeColorAllocated[aStoreIndex].StoreColorItemUnitsAllocated);
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreItemQtyAllocatedStructure(), true); //TT#1401 - JEllis - Urban Reseravation Stores Pt 4
		    _colorBin.CalcStoresWithAllocationCount = true;                            // MID Track 4448 AnF Audit Enhancement
		}

		/// <summary>
		/// Sets StoreItemUnitsAllocated to specified value for the specified store in the specified size of this color.
		/// </summary>
		/// <param name="aSizeRID">RID of the size.</param>
		/// <param name="aStoreIndex">Index value for the store.</param>
		/// <param name="aItemUnitsAllocated">Item Units allocated</param>
		internal void SetStoreItemUnitsAllocated (int aSizeRID, int aStoreIndex, int aItemUnitsAllocated)
		{
			SetStoreItemUnitsAllocated(GetSizeBin(aSizeRID), aStoreIndex, aItemUnitsAllocated);
		}

		/// <summary>
		/// Sets StoreItemUnitsAllocated to specified value for the specified store in the specified size of this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the size.</param>
		/// <param name="aStoreIndex">Index value for the store.</param>
		/// <param name="aItemUnitsAllocated">Item Units allocated</param>
		internal void SetStoreItemUnitsAllocated (HdrSizeBin aSize, int aStoreIndex, int aItemUnitsAllocated)
		{
			SetStoreColorTotalSizeItemAllocated(aStoreIndex,
				GetStoreColorTotalSizeItemAllocated(aStoreIndex) 
				- GetStoreItemUnitsAllocated(aSize, aStoreIndex));
			aSize.SetStoreSizeItemUnitsAllocated(aStoreIndex, aItemUnitsAllocated);
			SetStoreColorTotalSizeItemAllocated(aStoreIndex,
				GetStoreColorTotalSizeItemAllocated(aStoreIndex)
				+ GetStoreItemUnitsAllocated(aSize, aStoreIndex));
		}

        /// <summary>
        /// Gets StoreImoUnitsAllocated for the specified store in this color.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>Store's Color IMO Units allocated.</returns>
        public int GetStoreImoUnitsAllocated(int aStoreIndex)
        {
            return _storeColorAllocated[aStoreIndex].StoreColorImoUnitsAllocated;
        }

        /// <summary>
        /// Gets StoreImoUnitsAllocated for the specified store and size in this color.
        /// </summary>
        /// <param name="aSizeRID">RID for the size.</param>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>Store's Color IMO Units allocated.</returns>
        public int GetStoreImoUnitsAllocated(int aSizeRID, int aStoreIndex)
        {
            return GetStoreImoUnitsAllocated(GetSizeBin(aSizeRID), aStoreIndex);
        }

        /// <summary>
        /// Gets StoreImoUnitsAllocated for the specified store and size in this color.
        /// </summary>
        /// <param name="aSize">HdrSizeBin that describes this size.</param>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>Store's Color IMO Units allocated.</returns>
        internal int GetStoreImoUnitsAllocated(HdrSizeBin aSize, int aStoreIndex)
        {
            return aSize.GetStoreSizeImoUnitsAllocated(aStoreIndex);
        }

        // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        ///// <summary>
        ///// Sets StoreImoUnitsAllocated to specified value for specified store in this color.
        ///// </summary>
        ///// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        ///// <param name="aImoUnitsAllocated">Color IMO units allocated</param>
        //internal void SetStoreImoUnitsAllocated(int aStoreIndex, int aImoUnitsAllocated)
        //{
        //    this.SetColorImoUnitsAllocated(this.ColorImoUnitsAllocated - _storeColorAllocated[aStoreIndex].StoreColorImoUnitsAllocated);
        //    _storeColorAllocated[aStoreIndex].StoreColorImoUnitsAllocated = aImoUnitsAllocated;
        //    RecalcSizePlans = true; // MID Change j.ellis allow size need etc. on size review
        //    this.SetColorImoUnitsAllocated(this.ColorImoUnitsAllocated + _storeColorAllocated[aStoreIndex].StoreColorImoUnitsAllocated);
        //    _colorBin.CalcStoresWithAllocationCount = true;                            // MID Track 4448 AnF Audit Enhancement
        //}

        ///// <summary>
        ///// Sets StoreImoUnitsAllocated to specified value for the specified store in the specified size of this color.
        ///// </summary>
        ///// <param name="aSizeRID">RID of the size.</param>
        ///// <param name="aStoreIndex">Index value for the store.</param>
        ///// <param name="aImoUnitsAllocated">IMO Units allocated</param>
        //internal void SetStoreImoUnitsAllocated(int aSizeRID, int aStoreIndex, int aImoUnitsAllocated)
        //{
        //    SetStoreImoUnitsAllocated(GetSizeBin(aSizeRID), aStoreIndex, aImoUnitsAllocated);
        //}

        ///// <summary>
        ///// Sets StoreImoUnitsAllocated to specified value for the specified store in the specified size of this color.
        ///// </summary>
        ///// <param name="aSize">HdrSizeBin that describes the size.</param>
        ///// <param name="aStoreIndex">Index value for the store.</param>
        ///// <param name="aImoUnitsAllocated">IMO Units allocated</param>
        //internal void SetStoreImoUnitsAllocated(HdrSizeBin aSize, int aStoreIndex, int aImoUnitsAllocated)
        //{
        //    SetStoreColorTotalSizeImoAllocated(aStoreIndex,
        //        GetStoreColorTotalSizeImoAllocated(aStoreIndex)
        //        - GetStoreImoUnitsAllocated(aSize, aStoreIndex));
        //    aSize.SetStoreSizeImoUnitsAllocated(aStoreIndex, aImoUnitsAllocated);
        //    SetStoreColorTotalSizeImoAllocated(aStoreIndex,
        //        GetStoreColorTotalSizeImoAllocated(aStoreIndex)
        //        + GetStoreImoUnitsAllocated(aSize, aStoreIndex));
        //}
        // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1
		#endregion StoreUnitsAllocated

		#region StoreOrigUnitsAllocated
		/// <summary>
		/// Gets StoreOrigUnitsAllocated for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>Store's Color Original Units allocated.</returns>
		public int GetStoreOrigUnitsAllocated (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorOrigUnitsAllocated;
		}

		/// <summary>
		/// Gets StoreOrigUnitsAllocated for the specified store and size in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>Store's Color-size Original Units allocated.</returns>
		internal int GetStoreOrigUnitsAllocated (HdrSizeBin aSize, int aStoreIndex)
		{
			return aSize.GetStoreSizeOrigUnitsAllocated(aStoreIndex);
		}

		/// <summary>
		/// Sets StoreOrigUnitsAllocated to specified value for specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aUnitsAllocated">Color units allocated</param>
		internal void SetStoreOrigUnitsAllocated (int aStoreIndex, int aUnitsAllocated)
		{
			_storeColorAllocated[aStoreIndex].StoreColorOrigUnitsAllocated = aUnitsAllocated;
		}

		/// <summary>
		/// Sets StoreOrigUnitsAllocated to specified value for the specified store in the specified size of this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the size.</param>
		/// <param name="aStoreIndex">Index value for the store.</param>
		/// <param name="aUnitsAllocated">Units allocated</param>
		internal void SetStoreOrigUnitsAllocated (HdrSizeBin aSize, int aStoreIndex, int aUnitsAllocated)
		{
			aSize.SetStoreSizeOrigUnitsAllocated(aStoreIndex, aUnitsAllocated);
		}

		#endregion StoreOrigUnitsAllocated
        
		#region StoreUnitsAllocatedByAuto
		//================================//
		// Store Units Allocated by Auto //
		//===============================//
		/// <summary>
		/// Gets StoreUnitsAllocatedByAuto for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color units allocated by an auto allocation process.</returns>
		public int GetStoreUnitsAllocatedByAuto (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorUnitsAllocatedByAuto;
		}

		/// <summary>
		/// Gets StoreUnitsAllocatedByAuto for the specified store in this color.
		/// </summary>
		/// <param name="aSizeRID">RID of the size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color-Size units allocated by an auto allocation process.</returns>
		public int GetStoreUnitsAllocatedByAuto (int aSizeRID, int aStoreIndex)
		{
			return GetStoreUnitsAllocatedByAuto (GetSizeBin(aSizeRID), aStoreIndex);
		}

		/// <summary>
		/// Gets StoreUnitsAllocatedByAuto for the specified store in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color-Size units allocated by an auto allocation process.</returns>
		internal int GetStoreUnitsAllocatedByAuto (HdrSizeBin aSize, int aStoreIndex)
		{
			return aSize.GetStoreSizeUnitsAllocatedByAuto(aStoreIndex);
		}
 
		/// <summary>
		/// Sets StoreUnitsAllocatedByAuto for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aUnitsAllocated">Color units allocated by auto allocation process.</param>
		internal void SetStoreUnitsAllocatedByAuto (int aStoreIndex, int aUnitsAllocated)
		{
			_storeColorAllocated[aStoreIndex].StoreColorUnitsAllocatedByAuto = aUnitsAllocated;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreQtyAllocatedByAutoStructure(), true); // MID Track 3994 Performance
		}

		/// <summary>
		/// Sets StoreUnitsAllocatedByAuto for the specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aUnitsAllocated">Color-size units allocated by auto allocation process.</param>
		internal void SetStoreUnitsAllocatedByAuto (int aSizeRID, int aStoreIndex, int aUnitsAllocated)
		{
			SetStoreUnitsAllocatedByAuto(GetSizeBin(aSizeRID), aStoreIndex, aUnitsAllocated);
		}

		/// <summary>
		/// Sets StoreUnitsAllocatedByAuto for the specified store and size in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aUnitsAllocated">Color-size Units allocated by auto allocation process.</param>
		internal void SetStoreUnitsAllocatedByAuto (HdrSizeBin aSize, int aStoreIndex, int aUnitsAllocated)
		{
			aSize.SetStoreSizeUnitsAllocatedByAuto(aStoreIndex, aUnitsAllocated);
		}
		#endregion StoreUnitsAllocatedByAuto

		#region StoreUnitsAllocatedByRule
		//================================//
		// Store Units Allocated by Rule //
		//===============================//
		/// <summary>
		/// Gets StoreUnitsAllocatedByRule for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color units allocated by an Rule allocation process.</returns>
		public int GetStoreUnitsAllocatedByRule (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorUnitsAllocatedByRule;
		}

		/// <summary>
		/// Sets StoreUnitsAllocatedByRule for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aUnitsAllocated">Color units allocated by Rule allocation process.</param>
		internal void SetStoreUnitsAllocatedByRule (int aStoreIndex, int aUnitsAllocated)
		{
			_storeColorAllocated[aStoreIndex].StoreColorUnitsAllocatedByRule = aUnitsAllocated;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreQtyAllocatedByRuleStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreUnitsAllocatedByRule

		#region StoreChosenRuleType
		//========================//
		// Store Chosen Rule Type //
		//========================//
		/// <summary>
		/// Gets StoreChosenRuleType for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color Chosen Rule Type.</returns>
		public eRuleType GetStoreChosenRuleType (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorChosenRuleType;
		}

		/// <summary>
		/// Sets StoreChosenRuleType for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aChosenRuleType">Rule Type.</param>
		internal void SetStoreChosenRuleType (int aStoreIndex, eRuleType aChosenRuleType)
		{
			_storeColorAllocated[aStoreIndex].StoreColorChosenRuleType = aChosenRuleType;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreChosenRuleQtyStructure(), true); // MID Track 3994 Performance

		}
		#endregion StoreChosenRuleType

		#region StoreChosenRuleLayerID
		//============================//
		// Store Chosen Rule Layer ID //
		//============================//
		/// <summary>
		/// Gets StoreChosenRuleLayerID for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color Chosen Rule Layer ID.</returns>
		public int GetStoreChosenRuleLayerID (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorChosenRuleLayerID;
		}
 
		/// <summary>
		/// Sets StoreChosenRuleLayerID for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aChosenRuleLayerID">Color Chosen Rule Layer ID.</param>
		internal void SetStoreChosenRuleLayerID (int aStoreIndex, int aChosenRuleLayerID)
		{
			_storeColorAllocated[aStoreIndex].StoreColorChosenRuleLayerID = aChosenRuleLayerID;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreChosenRuleLayerStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreChosenRuleLayerID

		#region StoreChosenRuleUnitsAllocated
		//================================//
		// Store Chosen Rule Units Allocated //
		//===============================//
		/// <summary>
		/// Gets StoreChosenRuleUnitsAllocated for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color units allocated by the Chosen Rule.</returns>
		public int GetStoreChosenRuleUnitsAllocated (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorChosenRuleUnitsAllocated;
		}

		/// <summary>
		/// Sets StoreChosenRuleUnitsAllocated for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aUnitsAllocated">Color Chosen Rule Units Allocated.</param>
		internal void SetStoreChosenRuleUnitsAllocated (int aStoreIndex, int aUnitsAllocated)
		{
			_storeColorAllocated[aStoreIndex].StoreColorChosenRuleUnitsAllocated = aUnitsAllocated;
		}
		#endregion StoreChosenRuleUnitsAllocated

		#region StoreLastNeedDay
		//=======================//
		// Store Last Need Day //
		//=======================//
		/// <summary>
		/// Gets StoreLastNeedDay for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color Last Need Day.</returns>
		public DateTime GetStoreLastNeedDay (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorLastNeedDay;
		}

		/// <summary>
		/// Sets StoreLastNeedDay for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aLastNeedDay">Color Last Need Day.</param>
		internal void SetStoreLastNeedDay (int aStoreIndex, DateTime aLastNeedDay)
		{
			_storeColorAllocated[aStoreIndex].StoreColorLastNeedDay = aLastNeedDay;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreNeedDayStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreLastNeedDay

		#region StoreUnitNeedBefore
		//=======================//
		// Store Unit Need Before //
		//=======================//
		/// <summary>
		/// Gets StoreUnitNeedBefore for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color Unit Need Before.</returns>
		public int GetStoreUnitNeedBefore (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorUnitNeedBefore;
		}

		/// <summary>
		/// Sets StoreUnitNeedBefore for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aUnitNeedBefore">Color Unit Need Before.</param>
		internal void SetStoreUnitNeedBefore (int aStoreIndex, int aUnitNeedBefore)
		{
			_storeColorAllocated[aStoreIndex].StoreColorUnitNeedBefore = aUnitNeedBefore;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreNeedBeforeStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreUnitNeedBefore

		#region StorePercentNeedBefore
		//=======================//
		// Store Percent Need Before //
		//=======================//
		/// <summary>
		/// Gets StorePercentNeedBefore for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color Percent Need Before.</returns>
		public double GetStorePercentNeedBefore (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorPercentNeedBefore;
		}

		/// <summary>
		/// Sets StorePercentNeedBefore for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aPercentNeedBefore">Color Percent Need Before.</param>
		internal void SetStorePercentNeedBefore (int aStoreIndex, double aPercentNeedBefore)
		{
			_storeColorAllocated[aStoreIndex].StoreColorPercentNeedBefore = aPercentNeedBefore;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStorePercentNeedBeforeStructure(), true); // MID Track 3994 Performance
		}
		#endregion StorePercentNeedBefore

		#region StoreDetailAuditFlags
		/// <summary>
		/// Gets all detail audit flags simultaneously for the indicated store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>uint value whose bits represent the status of each flag.</returns>
        //internal ushort GetAllDetailAuditFlags(int aStoreIndex) // TT#488 - MD - Jellis - Group Allocation
        internal uint GetAllDetailAuditFlags(int aStoreIndex) // TT#488 - MD - Jellis - Group Allocation
		{
			return _storeColorAllocated[aStoreIndex].AllDetailAuditFlags;
		}

		/// <summary>
		/// Sets all detail audit flags simultaneously for the indicated store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <param name="aFlags">Flag Values.</param>
        //internal void SetAllDetailAuditFlags(int aStoreIndex, ushort aFlags)  // TT#488 - MD - Jellis - Group Allocation
        internal void SetAllDetailAuditFlags(int aStoreIndex, uint aFlags)  // TT#488 - MD - Jellis - Group Allocation
		{
			_storeColorAllocated[aStoreIndex].AllDetailAuditFlags = aFlags;
		}

        /// <summary>
        /// Gets all detail audit flags simultaneously for the indicated store
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
        /// <returns>Ushort value each of whose bits represent a single flag setting.</returns>
        //internal ushort GetAllDetailAuditFlags(int aStoreIndex) // TT#488 - MD - Jellis - Group Allocation
        internal ushort GetAllGeneralAuditFlags(int aStoreIndex) // TT#488 - MD - Jellis - Group Allocation
        {
            return _storeColorAllocated[aStoreIndex].AllGeneralAudits;
        }

        /// <summary>
        /// Sets all detail audit flags simultaneously for the indicated store.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
        /// <param name="aFlags">Flag values</param>
        //internal void SetAllDetailAuditFlags (int aStoreIndex, ushort aFlags) // TT#488 - MD - Jellis - Group Allocation
        internal void SetAllGeneralAuditFlags(int aStoreIndex, ushort aFlags)   // TT#488 - MD - Jellis - Group Allocation
        {
            _storeColorAllocated[aStoreIndex].AllGeneralAudits = aFlags;
        }
        #endregion StoreAuditFlags

        #region StoreEligibility
        /// <summary>
        /// Gets store Eligibility flag value.
        /// </summary>
        /// <param name="aStore">Index_RID identifier for the store</param>
        /// <returns>True if eligible, false if not eligible.</returns>
        public bool GetStoreIsEligible(Index_RID aStore)
        {
            return _storeColorAllocated[aStore.Index].StoreIsEligible;
        }

        /// <summary>
        /// Sets a store's Eligibility flag value.
        /// </summary>
        /// <param name="aStore">Index_RID identifier for the store.</param>
        /// <param name="aFlagValue">True if eligible, false if not.</param>
        internal void SetStoreIsEligible(Index_RID aStore, bool aFlagValue)
        {
            if (!_storeEligibilityLoaded || GetStoreIsEligible(aStore) != aFlagValue)
            {
                //SetIsChanged(eAllocationSummaryNode.Total, true);
                _storeColorAllocated[aStore.Index].StoreIsEligible = aFlagValue;
                //SetStoreIsChanged(aStore, true);
                this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true);
            }
        }
        #endregion StoreEligibility

        

        #region StoreIsChanged
        //==================//
        // Store Is Changed //
        //==================//
        /// <summary>
        /// Gets StoreIsChanged for a specified store in this color
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>True: store color information has changed; False: store color information has not changed.</returns>
        internal bool GetStoreIsChanged (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorIsChanged;
		}

		/// <summary>
		/// Gets StoreIsChanged for a specified store and size within this color.
		/// </summary>
		/// <param name="aSizeRID">RID for the specified size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True: store color size information has changed; False: store color size information has not changed.</returns>
		internal bool GetStoreIsChanged (int aSizeRID, int aStoreIndex)
		{
			return GetStoreIsChanged(this.GetSizeBin(aSizeRID), aStoreIndex);
		}

		/// <summary>
		/// Gets StoreIsChanged for a specified store and size within this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True: store color size information has changed; False: store color size information has not changed.</returns>
		internal bool GetStoreIsChanged (HdrSizeBin aSize, int aStoreIndex)
		{
			return aSize.GetStoreSizeIsChanged(aStoreIndex);
		}

		/// <summary>
		/// Sets StoreIsChanged to specified value for specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: store color size information has changed; False: store color size information has not changed.</param>
		internal void SetStoreIsChanged (int aStoreIndex, bool aFlagValue)
		{
			_storeColorAllocated[aStoreIndex].StoreColorIsChanged = aFlagValue;
		}

		/// <summary>
		/// Sets StoreIsChanged to specified value for specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID of the size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: store color size information has changed; False: store color size information has not changed.</param>
		internal void SetStoreIsChanged (int aSizeRID, int aStoreIndex, bool aFlagValue)
		{
			SetStoreIsChanged(GetSizeBin(aSizeRID), aStoreIndex, aFlagValue);
		}

		/// <summary>
		/// Sets StoreIsChanged to specified value for specified store and size in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: store color size information has changed; False: store color size information has not changed.</param>
		internal void SetStoreIsChanged (HdrSizeBin aSize, int aStoreIndex, bool aFlagValue)
		{
			aSize.SetStoreSizeIsChanged(aStoreIndex, aFlagValue);
		}
		#endregion StoreIsChanged

		#region StoreIsNew
		//==============//
		// Store Is New //
		//==============//
		/// <summary>
		/// Gets StoreIsNew for a specified store in this color
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True: store color information is New; False: store color information is not New.</returns>
		internal bool GetStoreIsNew (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorIsNew;
		}

		/// <summary>
		/// Gets StoreIsNew for a specified store and size within this color.
		/// </summary>
		/// <param name="aSizeRID">RID for the specified size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True: store color size information is New; False: store color size information is not New.</returns>
		internal bool GetStoreIsNew (int aSizeRID, int aStoreIndex)
		{
			return GetStoreIsNew(this.GetSizeBin(aSizeRID), aStoreIndex);
		}

		/// <summary>
		/// Gets StoreIsNew for a specified store and size within this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True: store color size information is New; False: store color size information is not New.</returns>
		internal bool GetStoreIsNew (HdrSizeBin aSize, int aStoreIndex)
		{
			return aSize.GetStoreSizeIsNew(aStoreIndex);
		}

		/// <summary>
		/// Sets StoreIsNew to specified value for specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: store color size information is New; False: store color size information is not New.</param>
		internal void SetStoreIsNew (int aStoreIndex, bool aFlagValue)
		{
			_storeColorAllocated[aStoreIndex].StoreColorIsNew = aFlagValue;
		}

		/// <summary>
		/// Sets StoreIsNew to specified value for specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID of the size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: store color size information is New; False: store color size information is not New.</param>
		internal void SetStoreIsNew (int aSizeRID, int aStoreIndex, bool aFlagValue)
		{
			SetStoreIsNew(GetSizeBin(aSizeRID), aStoreIndex, aFlagValue);
		}

		/// <summary>
		/// Sets StoreIsNew to specified value for specified store and size in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: store color size information is New; False: store color size information is not New.</param>
		internal void SetStoreIsNew (HdrSizeBin aSize, int aStoreIndex, bool aFlagValue)
		{
			aSize.SetStoreSizeIsNew(aStoreIndex, aFlagValue);
		}
		#endregion StoreIsNew

		#region StoreIsManuallyAllocated
		//=============================//
		// Store Is Manually Allocated //
		//=============================//
		/// <summary>
		/// Gets StoreIsManuallyAllocated for a specified store in this color
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's color allocation has been manually specified by the user.</returns>
		public bool GetStoreIsManuallyAllocated (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorIsManuallyAllocated;
		}

		/// <summary>
		/// Gets StoreIsManuallyAllocated for a specified store and size within this color.
		/// </summary>
		/// <param name="aSizeRID">RID for the specified size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's color-size allocation has been manually specified by the user.</returns>
		public bool GetStoreIsManuallyAllocated (int aSizeRID, int aStoreIndex)
		{
			return GetStoreIsManuallyAllocated(this.GetSizeBin(aSizeRID), aStoreIndex);
		}

		/// <summary>
		/// Gets StoreIsManuallyAllocated for a specified store and size within this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's color-size allocation has been manually specified by the user.</returns>
		internal bool GetStoreIsManuallyAllocated (HdrSizeBin aSize, int aStoreIndex)
		{
			return aSize.GetStoreSizeIsManuallyAllocated(aStoreIndex);
		}

		/// <summary>
		/// Sets StoreIsManuallyAllocated to specified value for specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: Color was manually allocated for this store; false, otherwise.</param>
		internal void SetStoreIsManuallyAllocated (int aStoreIndex, bool aFlagValue)
		{
			_storeColorAllocated[aStoreIndex].StoreColorIsManuallyAllocated = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}

		/// <summary>
		/// Sets StoreIsManuallyAllocated to specified value for specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID of the size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: Size was manually allocated for this store; false, otherwise.</param>
		internal void SetStoreIsManuallyAllocated (int aSizeRID, int aStoreIndex, bool aFlagValue)
		{
			SetStoreIsManuallyAllocated(GetSizeBin(aSizeRID), aStoreIndex, aFlagValue);
		}

		/// <summary>
		/// Sets StoreIsManuallyAllocated to specified value for specified store and size in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: Size was manually allocated for this store; false, otherwise.</param>
		internal void SetStoreIsManuallyAllocated (HdrSizeBin aSize, int aStoreIndex, bool aFlagValue)
		{
			aSize.SetStoreSizeIsManuallyAllocated(aStoreIndex, aFlagValue);
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
        /// <summary>
        /// Gets StoreItemIsManuallyAllocated for a specified store in this color
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>True when the store's color item allocation has been manually specified by the user.</returns>
        public bool GetStoreItemIsManuallyAllocated(int aStoreIndex)  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        {
            return _storeColorAllocated[aStoreIndex].StoreColorItemIsManuallyAllocated;  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        }

        /// <summary>
        /// Gets StoreItemIsManuallyAllocated for a specified store and size within this color.
        /// </summary>
        /// <param name="aSizeRID">RID for the specified size</param>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>True when the store's color-size item allocation has been manually specified by the user.</returns>
        public bool GetStoreItemIsManuallyAllocated(int aSizeRID, int aStoreIndex)   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F     
        {
            return GetStoreItemIsManuallyAllocated(this.GetSizeBin(aSizeRID), aStoreIndex); // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        }

        /// <summary>
        /// Gets StoreItemIsManuallyAllocated for a specified store and size within this color.
        /// </summary>
        /// <param name="aSize">HdrSizeBin that describes this size</param>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>True when the store's color-size item allocation has been manually specified by the user.</returns>
        internal bool GetStoreItemIsManuallyAllocated(HdrSizeBin aSize, int aStoreIndex) // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        {
            return aSize.GetStoreSizeItemIsManuallyAllocated(aStoreIndex);  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        }

        /// <summary>
        /// Sets StoreItemIsManuallyAllocated to specified value for specified store in this color.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <param name="aFlagValue">True: Color Item was manually allocated for this store; false, otherwise.</param>
        internal void SetStoreItemIsManuallyAllocated(int aStoreIndex, bool aFlagValue) // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        {
            _storeColorAllocated[aStoreIndex].StoreColorItemIsManuallyAllocated = aFlagValue;  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
            this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
        }

        /// <summary>
        /// Sets StoreItemIsManuallyAllocated to specified value for specified store and size in this color.
        /// </summary>
        /// <param name="aSizeRID">RID of the size</param>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <param name="aFlagValue">True: Size item was manually allocated for this store; false, otherwise.</param>
        internal void SetStoreItemIsManuallyAllocated(int aSizeRID, int aStoreIndex, bool aFlagValue) // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        {
            SetStoreItemIsManuallyAllocated(GetSizeBin(aSizeRID), aStoreIndex, aFlagValue); // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        }

        /// <summary>
        /// Sets StoreItemIsManuallyAllocated to specified value for specified store and size in this color.
        /// </summary>
        /// <param name="aSize">HdrSizeBin that describes this size</param>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <param name="aFlagValue">True: Size item was manually allocated for this store; false, otherwise.</param>
        internal void SetStoreItemIsManuallyAllocated(HdrSizeBin aSize, int aStoreIndex, bool aFlagValue) // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        {
            aSize.SetStoreSizeItemIsManuallyAllocated(aStoreIndex, aFlagValue); // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 2
		#endregion StoreIsManuallyAllocated

		#region StoreWasAutoAllocated
		//==========================//
		// Store Was Auto Allocated //
		//==========================//
		/// <summary>
		/// Gets StoreWasAutoAllocated for the specified store in this color
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's Color allocation was auto allocated by the system</returns>
		public bool GetStoreWasAutoAllocated (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorWasAutoAllocated;
		}

		/// <summary>
		/// Gets StoreWasAutoAllocated for the specified store and size in this color
		/// </summary>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's Color-Size allocation was auto allocated by the system</returns>
		public bool GetStoreWasAutoAllocated (int aSizeRID, int aStoreIndex)
		{
			return GetStoreWasAutoAllocated (GetSizeBin(aSizeRID), aStoreIndex);
		}

		/// <summary>
		/// Gets StoreWasAutoAllocated for the specified store and size in this color
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's Color-Size allocation was auto allocated by the system</returns>
		internal bool GetStoreWasAutoAllocated (HdrSizeBin aSize, int aStoreIndex)
		{
			return aSize.GetStoreSizeWasAutoAllocated(aStoreIndex);
		}

		/// <summary>
		/// Sets StoreWasAutoAllocated to specified value for specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStoreWasAutoAllocated (int aStoreIndex, bool aFlagValue)
		{
			_storeColorAllocated[aStoreIndex].StoreColorWasAutoAllocated = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}

		/// <summary>
		/// Sets StoreWasAutoAllocated to specified value for specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStoreWasAutoAllocated (int aSizeRID, int aStoreIndex, bool aFlagValue)
		{
			SetStoreWasAutoAllocated(GetSizeBin(aSizeRID), aStoreIndex, aFlagValue);
		}

		/// <summary>
		/// Sets StoreWasAutoAllocated to specified value for specified store and size in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStoreWasAutoAllocated (HdrSizeBin aSize, int aStoreIndex, bool aFlagValue)
		{
			aSize.SetStoreSizeWasAutoAllocated(aStoreIndex, aFlagValue);
		}
		#endregion StoreWasAutoAllocated

		#region StoreChosenRuleAcceptedByHeader
		//======================================//
		// Store Chosen Rule Accepted by Header //
		//======================================//
		/// <summary>
		/// Gets StoreColorChosenRuleAcceptedByHeader for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's Color Rule allocation has been accepted on header.</returns>
        public bool GetStoreChosenRuleAcceptedByHeader(int aStoreIndex) // TT#488 - MD - Jellis - Group Allocation
		{
			return _storeColorAllocated[aStoreIndex].StoreColorChosenRuleAcceptedByHeader;  // TT#488 - MD - Jellis - Group Allocation
		}

		/// <summary>
		/// Sets StoreColorChosenRuleAcceptedByHeader to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStoreChosenRuleAcceptedByHeader (int aStoreIndex, bool aFlagValue)
		{
            _storeColorAllocated[aStoreIndex].StoreColorChosenRuleAcceptedByHeader = aFlagValue; // TT#488 - MD - Jellis - Group Allocation
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreChosenRuleAcceptedByHeader
    
        // begin TT#488 - MD - Jellis - Group Allocation
        #region StoreChosenRuleAcceptedByGroup
        //=====================================//
        // Store Chosen Rule Accepted by Group //
        //=====================================//
        /// <summary>
        /// Gets StoreColorChosenRuleAcceptedByGroup for the specified store
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>True when the store's Color Rule allocation has been accepted in Group Allocation.</returns>
        public bool GetStoreChosenRuleAcceptedByGroup(int aStoreIndex) 
        {
            return _storeColorAllocated[aStoreIndex].StoreColorChosenRuleAcceptedByGroup;  
        }

        /// <summary>
        /// Sets StoreColorChosenRuleAcceptedByGroup to specified value for specified store.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <param name="aFlagValue">Flag value</param>
        internal void SetStoreChosenRuleAcceptedByGroup(int aStoreIndex, bool aFlagValue)
        {
            _storeColorAllocated[aStoreIndex].StoreColorChosenRuleAcceptedByGroup = aFlagValue; 
            this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true);
        }
        #endregion StoreChosenRuleAcceptedByGroup

        // end TT#488 - MD - Jellis - Group Allocation

		#region StoreRuleAllocationFromParentComponent
		//=========================================//
		// Store RuleAllocationFromParentComponent //
		//=========================================//
		/// <summary>
		/// Gets StoreColorRuleAllocationFromParentComponent for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's Color allocation has been manually specified by the user.</returns>
		public bool GetStoreRuleAllocationFromParentComponent (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorRuleAllocationFromParentComponent;
		}

		/// <summary>
		/// Sets StoreColorRuleAllocationFromParentComponent to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStoreRuleAllocationFromParentComponent (int aStoreIndex, bool aFlagValue)
		{
			_storeColorAllocated[aStoreIndex].StoreColorRuleAllocationFromParentComponent = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreRuleAllocationFromParentComponent

        // begin TT#488 - MD - Jellis - Group Allocation
        #region StoreRuleAllocationFromGroupComponent
        //=========================================//
        // Store RuleAllocationFromGroupComponent //
        //=========================================//
        /// <summary>
        /// Gets StoreColorRuleAllocationFromGrouptComponent for the specified store
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>True when the store's Color allocation has been partially or completely derived from a Group Allocation Component.</returns>
        public bool GetStoreRuleAllocationFromGroupComponent(int aStoreIndex)
        {
            return _storeColorAllocated[aStoreIndex].StoreColorRuleAllocationFromGroupComponent;
        }

        /// <summary>
        /// Sets StoreColorRuleAllocationFromGroupComponent to specified value for specified store.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <param name="aFlagValue">Flag value</param>
        internal void SetStoreRuleAllocationFromGroupComponent(int aStoreIndex, bool aFlagValue)
        {
            _storeColorAllocated[aStoreIndex].StoreColorRuleAllocationFromGroupComponent = aFlagValue;
            this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
        }
        #endregion StoreRuleAllocationFromGroupComponent

        // end TT#488 - MD - Jellis - Group Allocation 

		#region StoreRuleAllocationFromChildComponent
		//=========================================//
		// Store RuleAllocationFromChildComponent //
		//=========================================//
		/// <summary>
		/// Gets StoreColorRuleAllocationFromChildComponent for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's Color allocation has been manually specified by the user.</returns>
		public bool GetStoreRuleAllocationFromChildComponent (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorRuleAllocationFromChildComponent;
		}

		/// <summary>
		/// Sets StoreColorRuleAllocationFromChildComponent to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStoreRuleAllocationFromChildComponent (int aStoreIndex, bool aFlagValue)
		{
			_storeColorAllocated[aStoreIndex].StoreColorRuleAllocationFromChildComponent = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreRuleAllocationFromChildComponent

		#region StoreRuleAllocationFromChosenRule
		//=========================================//
		// Store RuleAllocationFromChosenRule //
		//=========================================//
		/// <summary>
		/// Gets StoreColorRuleAllocationFromChosenRule for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's Color allocation has been manually specified by the user.</returns>
		public bool GetStoreRuleAllocationFromChosenRule (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorRuleAllocationFromChosenRule;
		}

		/// <summary>
		/// Sets StoreColorRuleAllocationFromChosenRule to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStoreRuleAllocationFromChosenRule (int aStoreIndex, bool aFlagValue)
		{
			_storeColorAllocated[aStoreIndex].StoreColorRuleAllocationFromChosenRule = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}
		#endregion StoreRuleAllocationFromChosenRule

		#region StoreAllocationFromBottomUpSize
		//=========================================//
		// Store AllocationFromBottomUpSize //
		//=========================================//
		/// <summary>
		/// Gets StoreColorAllocationFromBottomUpSize for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's Color allocation has been manually specified by the user.</returns>
		public bool GetStoreAllocationFromBottomUpSize (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorAllocationFromBottomUpSize;
		}

		/// <summary>
		/// Sets StoreColorAllocationFromBottomUpSize to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStoreAllocationFromBottomUpSize (int aStoreIndex, bool aFlagValue)
		{
			_storeColorAllocated[aStoreIndex].StoreColorAllocationFromBottomUpSize = aFlagValue;
		}
		#endregion StoreAllocationFromBottomUpSize


		#region StoreAllocationFromBulkSizeBreakOut
		//=========================================//
		// Store AllocationFromBulkSizeBreakOut //
		//=========================================//
		/// <summary>
		/// Gets StoreColorAllocationFromBulkSizeBreakOut for the specified store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's Color allocation has been manually specified by the user.</returns>
		public bool GetStoreAllocationFromBulkSizeBreakOut (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorAllocationFromBulkSizeBreakOut;
		}

		/// <summary>
		/// Sets StoreColorAllocationFromBulkSizeBreakOut to specified value for specified store.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">Flag value</param>
		internal void SetStoreAllocationFromBulkSizeBreakOut (int aStoreIndex, bool aFlagValue)
		{
			_storeColorAllocated[aStoreIndex].StoreColorAllocationFromBulkSizeBreakOut = aFlagValue;
		}
		#endregion StoreAllocationFromBulkSizeBreakOut


		#region StoreOut
		//===========//
		// Store Out //
		//===========//
		/// <summary>
		/// Gets StoreOut for the specified store in this color
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's has been "outted" for this color.</returns>
		public bool GetStoreOut (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorOut;
		}

		/// <summary>
		/// Gets StoreOut for the specified store and size in this color
		/// </summary>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's has been "outted" for this color.</returns>
		public bool GetStoreOut (int aSizeRID, int aStoreIndex)
		{
			return GetStoreOut(GetSizeBin(aSizeRID), aStoreIndex);
		}
		/// <summary>
		/// Gets StoreOut for the specified store and size in this color
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's has been "outted" for this color.</returns>
		internal bool GetStoreOut (HdrSizeBin aSize, int aStoreIndex)
		{
			return aSize.GetStoreSizeOut(aStoreIndex);
		}

		/// <summary>
		/// Sets StoreOut to specified value for specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True when the store has been "outted" for this color; otherwise, false.</param>
		internal void SetStoreOut (int aStoreIndex, bool aFlagValue)
		{
			_storeColorAllocated[aStoreIndex].StoreColorOut = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}

		/// <summary>
		/// Sets StoreOut to specified value for specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID for this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True when the store has been "outted" for this size in this color; otherwise, false.</param>
		internal void SetStoreOut (int aSizeRID, int aStoreIndex, bool aFlagValue)
		{
			SetStoreOut(GetSizeBin(aSizeRID), aStoreIndex, aFlagValue);
		}

		/// <summary>
		/// Sets StoreOut to specified value for specified store in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True when the store has been "outted" for this size in this color; otherwise, false.</param>
		internal void SetStoreOut (HdrSizeBin aSize, int aStoreIndex, bool aFlagValue)
		{
			aSize.SetStoreSizeOut(aStoreIndex, aFlagValue);
		}
		#endregion StoreOut

		#region StoreLocked
		//==============//
		// Store Locked //
		//==============//
		/// <summary>
		/// Gets StoreLocked for the specified store in this color
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's has been "Locked" for this color.</returns>
		public bool GetStoreLocked (int aStoreIndex)
		{
            //return _storeColorAllocated[aStoreIndex].StoreColorLocked;   // TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
            return _storeLocked[aStoreIndex];                              // TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
		}

		/// <summary>
		/// Gets StoreLocked for the specified store and size in this color
		/// </summary>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's has been "Locked" for this color-size.</returns>
		public bool GetStoreLocked (int aSizeRID, int aStoreIndex)
		{
			return GetStoreLocked(GetSizeBin(aSizeRID), aStoreIndex);
		}
		/// <summary>
		/// Gets StoreLocked for the specified store and size in this color
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's has been "Locked" for this color-size.</returns>
        public bool GetStoreLocked(HdrSizeBin aSize, int aStoreIndex) // TT#1189 - MD - Jellis - Group Allocation - Implement Lock Not Persist to Database pt 1
		{
			return aSize.GetStoreSizeLocked(aStoreIndex);
		}

		/// <summary>
		/// Sets StoreLocked to specified value for specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True when the store has been "Locked" for this color; otherwise, false.</param>
        public void SetStoreLocked(int aStoreIndex, bool aFlagValue)   // TT#1189 - MD - Jellis - Group Allocation - Implement Lock Not Persist to Database pt 1
		{
            // begin TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
            //_storeColorAllocated[aStoreIndex].StoreColorLocked = aFlagValue;
            //this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
            _storeLocked[aStoreIndex] = aFlagValue;
            // end TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
		}

		/// <summary>
		/// Sets StoreLocked to specified value for specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID for this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True when the store has been "Locked" for this size in this color; otherwise, false.</param>
        public void SetStoreLocked(int aSizeRID, int aStoreIndex, bool aFlagValue) // TT#1189 - MD - Jellis - Group Allocation - Implement Lock Not Persist to Database pt 1
		{
			SetStoreLocked(GetSizeBin(aSizeRID), aStoreIndex, aFlagValue);
		}

		/// <summary>
		/// Sets StoreLocked to specified value for specified store in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True when the store has been "Locked" for this size in this color; otherwise, false.</param>
        public void SetStoreLocked(HdrSizeBin aSize, int aStoreIndex, bool aFlagValue) // TT#1189 - MD - Jellis - Group Allocation - Implement Lock Not Persist to Database pt 1
		{
			aSize.SetStoreSizeLocked(aStoreIndex, aFlagValue);
		}
		#endregion StoreLocked

		#region StoreTempLock
		//================//
		// Store TempLock //
		//================//
		/// <summary>
		/// Gets StoreTempLock for the specified store in this color
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store has been "Temporarily Locked" for this color.</returns>
		public bool GetStoreTempLock (int aStoreIndex)
		{
			//return _storeColorAllocated[aStoreIndex].StoreColorTempLock;   // TT#59 Implement Temp Locks
            return _storeTempLock[aStoreIndex];                              // TT#59 Implement Temp Locks
		}

		/// <summary>
		/// Gets StoreTempLock for the specified store and size in this color
		/// </summary>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's has been "Temporarily Locked" for this color-size.</returns>
		public bool GetStoreTempLock (int aSizeRID, int aStoreIndex)
		{
			return GetStoreTempLock(GetSizeBin(aSizeRID), aStoreIndex);
		}
		/// <summary>
		/// Gets StoreTempLock for the specified store and size in this color
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's has been "Temporarily Locked" for this color-size.</returns>
		internal bool GetStoreTempLock (HdrSizeBin aSize, int aStoreIndex)
		{
			return aSize.GetStoreSizeTempLock(aStoreIndex);
		}

		/// <summary>
		/// Sets StoreTempLock to specified value for specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True when the store has been "Temporarily Locked" for this color; otherwise, false.</param>
		internal void SetStoreTempLock (int aStoreIndex, bool aFlagValue)
		{
            // begin TT#59 Implement Temp Locks
			//_storeColorAllocated[aStoreIndex].StoreColorTempLock = aFlagValue;
			//this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
            _storeTempLock[aStoreIndex] = aFlagValue;
            // end TT#59 Implement Temp Locks
		}

		/// <summary>
		/// Sets StoreTempLock to specified value for specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID for this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True when the store has been "Temporarily Locked" for this size in this color; otherwise, false.</param>
		internal void SetStoreTempLock (int aSizeRID, int aStoreIndex, bool aFlagValue)
		{
			SetStoreTempLock(GetSizeBin(aSizeRID), aStoreIndex, aFlagValue);
		}

		/// <summary>
		/// Sets StoreTempLock to specified value for specified store in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True when the store has been "Temporarily Locked" for this size in this color; otherwise, false.</param>
		internal void SetStoreTempLock (HdrSizeBin aSize, int aStoreIndex, bool aFlagValue)
		{
			aSize.SetStoreSizeTempLock(aStoreIndex, aFlagValue);
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
        /// <summary>
        /// Gets StoreItemTempLock for the specified store in this color
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>True when the store has been "Temporarily Locked" for this color Item.</returns>
        public bool GetStoreItemTempLock(int aStoreIndex)
        {
            return _storeItemTempLock[aStoreIndex];                              
        }

        /// <summary>
        /// Gets StoreItemTempLock for the specified store and size in this color
        /// </summary>
        /// <param name="aSizeRID">RID for the size.</param>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>True when the store's has been "Temporarily Locked" for this color-size Item.</returns>
        public bool GetStoreItemTempLock(int aSizeRID, int aStoreIndex)
        {
            return GetStoreItemTempLock(GetSizeBin(aSizeRID), aStoreIndex);
        }
        /// <summary>
        /// Gets StoreItemTempLock for the specified store and size in this color
        /// </summary>
        /// <param name="aSize">HdrSizeBin that describes this size.</param>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <returns>True when the store's has been "Temporarily Locked" for this color-size item.</returns>
        internal bool GetStoreItemTempLock(HdrSizeBin aSize, int aStoreIndex)
        {
            return aSize.GetStoreSizeItemTempLock(aStoreIndex);
        }

        /// <summary>
        /// Sets StoreItemTempLock to specified value for specified store in this color.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <param name="aFlagValue">True when the store has been "Temporarily Locked" for this color item; otherwise, false.</param>
        internal void SetStoreItemTempLock(int aStoreIndex, bool aFlagValue)
        {
            _storeItemTempLock[aStoreIndex] = aFlagValue;
        }

        /// <summary>
        /// Sets StoreItemTempLock to specified value for specified store and size in this color.
        /// </summary>
        /// <param name="aSizeRID">RID for this size.</param>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <param name="aFlagValue">True when the store has been "Temporarily Locked" for this size in this color item; otherwise, false.</param>
        internal void SetStoreItemTempLock(int aSizeRID, int aStoreIndex, bool aFlagValue)
        {
            SetStoreItemTempLock(GetSizeBin(aSizeRID), aStoreIndex, aFlagValue);
        }

        /// <summary>
        /// Sets StoreItemTempLock to specified value for specified store in this color.
        /// </summary>
        /// <param name="aSize">HdrSizeBin that describes this size.</param>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <param name="aFlagValue">True when the store has been "Temporarily Locked" for this size in this color item; otherwise, false.</param>
        internal void SetStoreItemTempLock(HdrSizeBin aSize, int aStoreIndex, bool aFlagValue)
        {
            aSize.SetStoreSizeItemTempLock(aStoreIndex, aFlagValue);
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 2

        // begin TT#59 Implement Temp Locks
        internal void ResetTempLocks()
        {
            if (_storeTempLock != null)
            {
                _storeTempLock = new bool[this.StoreDimensions];
                _storeItemTempLock = new bool[this.StoreDimensions]; // TT#1401 - JEllis - Urban Reservation Stores pt 2
            }
        }
        // end TT#59 Implement Temp Locks
		#endregion StoreTempLock

		#region StoreHadNeed
		//================//
		// Store Had Need //
		//================//
		/// <summary>
		/// Gets StoreHadNeed for the specified store in this color
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store Had a Need for this color.</returns>
		public bool GetStoreHadNeed (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorHadNeed;
		}

		/// <summary>
		/// Gets StoreHadNeed for the specified store and size in this color
		/// </summary>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store Had a Need for this color-size.</returns>
		public bool GetStoreHadNeed (int aSizeRID, int aStoreIndex)
		{
			return GetStoreHadNeed(GetSizeBin(aSizeRID), aStoreIndex);
		}
		/// <summary>
		/// Gets StoreHadNeed for the specified store and size in this color
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's has been "Had Need" for this color.</returns>
		internal bool GetStoreHadNeed (HdrSizeBin aSize, int aStoreIndex)
		{
			return aSize.GetStoreSizeHadNeed(aStoreIndex);
		}

		/// <summary>
		/// Sets StoreHadNeed to specified value for specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True when the store Had a Need for this color; otherwise, false.</param>
		internal void SetStoreHadNeed (int aStoreIndex, bool aFlagValue)
		{
			_storeColorAllocated[aStoreIndex].StoreColorHadNeed = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}

		/// <summary>
		/// Sets StoreHadNeed to specified value for specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID for this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True when the store Had a Need for this size in this color; otherwise, false.</param>
		internal void SetStoreHadNeed (int aSizeRID, int aStoreIndex, bool aFlagValue)
		{
			SetStoreHadNeed(GetSizeBin(aSizeRID), aStoreIndex, aFlagValue);
		}

		/// <summary>
		/// Sets StoreHadNeed to specified value for specified store in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True when the store Had a Need for this size in this color; otherwise, false.</param>
		internal void SetStoreHadNeed (HdrSizeBin aSize, int aStoreIndex, bool aFlagValue)
		{
			aSize.SetStoreSizeHadNeed(aStoreIndex, aFlagValue);
		}
		#endregion StoreHadNeed

		#region StoreFilledSizeHole
		//======================//
		// Store FilledSizeHole //
		//======================//
		/// <summary>
		/// Gets StoreFilledSizeHole for the specified store in this color
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store "FilledSizeHole" for this color.</returns>
		public bool GetStoreFilledSizeHole (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorFilledSizeHole;
		}

		/// <summary>
		/// Gets StoreFilledSizeHole for the specified store and size in this color
		/// </summary>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store "FilledSizeHole" for this color.</returns>
		public bool GetStoreFilledSizeHole (int aSizeRID, int aStoreIndex)
		{
			return GetStoreFilledSizeHole(GetSizeBin(aSizeRID), aStoreIndex);
		}
		/// <summary>
		/// Gets StoreFilledSizeHole for the specified store and size in this color
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store "FilledSizeHole" for this color.</returns>
		internal bool GetStoreFilledSizeHole (HdrSizeBin aSize, int aStoreIndex)
		{
			return aSize.GetStoreSizeFilledSizeHole(aStoreIndex);
		}

		/// <summary>
		/// Sets StoreFilledSizeHole to specified value for specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True when the store "FilledSizeHole" for this color; otherwise, false.</param>
		internal void SetStoreFilledSizeHole (int aStoreIndex, bool aFlagValue)
		{
			_storeColorAllocated[aStoreIndex].StoreColorFilledSizeHole = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}

		/// <summary>
		/// Sets StoreFilledSizeHole to specified value for specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID for this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True when the store "FilledSizeHole" for this size in this color; otherwise, false.</param>
		internal void SetStoreFilledSizeHole (int aSizeRID, int aStoreIndex, bool aFlagValue)
		{
			SetStoreFilledSizeHole(GetSizeBin(aSizeRID), aStoreIndex, aFlagValue);
		}

		/// <summary>
		/// Sets StoreFilledSizeHole to specified value for specified store in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True when the store "FilledSizeHole" for this size in this color; otherwise, false.</param>
		internal void SetStoreFilledSizeHole (HdrSizeBin aSize, int aStoreIndex, bool aFlagValue)
		{
			aSize.SetStoreSizeFilledSizeHole(aStoreIndex, aFlagValue);
		}
		#endregion StoreFilledSizeHole

		// begin MID Track 4448 Anf Audit Enhancement
		#region StoreAllocationModifiedAfterMultiHeaderSplit
		//====================================================//
		// Store Allocation Modified After Multi Header Split //
		//====================================================//
		/// <summary>
		/// Gets StoreAllocationModifiedAfterMultiHeaderSplit audit flag for a specified store in this color
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's color allocation was changed after a multi header split.</returns>
		public bool GetStoreAllocationModifiedAfterMultiHeaderSplit (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorAllocationModifiedAfterMultiHeaderSplit;
		}

		/// <summary>
		/// Gets StoreAllocationModifiedAfterMultiHeaderSplit audit flag for a specified store in this color
		/// </summary>
		/// <param name="aSizeRID">RID for the specified size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's color allocation was changed after a multi header split.</returns>
		public bool GetStoreAllocationModifiedAfterMultiHeaderSplit (int aSizeRID, int aStoreIndex)
		{
			return GetStoreAllocationModifiedAfterMultiHeaderSplit (this.GetSizeBin(aSizeRID), aStoreIndex);
		}

		/// <summary>
		/// Gets StoreAllocationModifiedAfterMultiHeaderSplit audit flag for a specified store in this color
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True when the store's color allocation was changed after a multi header split.</returns>
		internal bool GetStoreAllocationModifiedAfterMultiHeaderSplit  (HdrSizeBin aSize, int aStoreIndex)
		{
			return aSize.GetStoreAllocationModifiedAfterMultiHeaderSplit(aStoreIndex);
		}

		/// <summary>
		/// Sets StoreAllocationModifiedAfterMultiHeaderSplit to specified value for specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: Color Allocation for this store changed after a multi header split; false, otherwise.</param>
		internal void SetStoreAllocationModifiedAfterMultiHeaderSplit (int aStoreIndex, bool aFlagValue)
		{
			_storeColorAllocated[aStoreIndex].StoreColorIsManuallyAllocated = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreDetailAuditFlagsStructure(), true); // MID Track 3994 Performance
		}

		/// <summary>
		/// Sets StoreIsManuallyAllocated to specified value for specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID of the size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: Size Allocation for this store changed after a multi header split; false, otherwise.</param>
		internal void SetStoreAllocationModifiedAfterMultiHeaderSplit (int aSizeRID, int aStoreIndex, bool aFlagValue)
		{
			SetStoreAllocationModifiedAfterMultiHeaderSplit(GetSizeBin(aSizeRID), aStoreIndex, aFlagValue);
		}

		/// <summary>
		/// Sets StoreAllocationModifiedAfterMultiHeaderSplit to specified value for specified store in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: Size Allocation for this store changed after a multi header split; false, otherwise.</param>
		internal void SetStoreAllocationModifiedAfterMultiHeaderSplit (HdrSizeBin aSize, int aStoreIndex, bool aFlagValue)
		{
			aSize.SetStoreAllocationModifiedAfterMultiHeaderSplit(aStoreIndex, aFlagValue);
		}
		#endregion StoreIsManuallyAllocated
		// end MID Track 4448 AnF Audit Enhancement

		#region StoreMaximum
		//===============//
		// Store Maximum //
		//===============//
		/// <summary>
		/// Gets Store Maximum allocation constraint for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color Maximum allocation for the specified store</returns>
		public int GetStoreMaximum (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorUnitMaximum;
		}

		/// <summary>
		/// Gets Store Maximum allocation constraint for the specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID of the size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color Maximum allocation for the specified store</returns>
		public int GetStoreMaximum(int aSizeRID, int aStoreIndex)
		{
			return GetStoreMaximum(GetSizeBin(aSizeRID), aStoreIndex);
		}

		/// <summary>
		/// Gets Store Maximum allocation constraint for the specified store in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color Maximum allocation for the specified store</returns>
		internal int GetStoreMaximum(HdrSizeBin aSize, int aStoreIndex)
		{
			return aSize.GetStoreSizeMaximum(aStoreIndex);
		}

		/// <summary>
		/// Sets Store Maximum allocation for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aMaximum">Maximum Color allocation value</param>
		internal void SetStoreMaximum (int aStoreIndex, int aMaximum)
		{
			_storeColorAllocated[aStoreIndex].StoreColorUnitMaximum = aMaximum;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreMaximumStructure(), true); // MID Track 3994 Performance
		}

		/// <summary>
		/// Sets Store Maximum allocation for the specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aMaximum">Maximum Color-size allocation value</param>
		internal void SetStoreMaximum (int aSizeRID, int aStoreIndex, int aMaximum)
		{
			SetStoreMaximum(GetSizeBin(aSizeRID), aStoreIndex, aMaximum);
		}

		/// <summary>
		/// Sets Store Maximum allocation for the specified store and size in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aMaximum">Maximum Color-size allocation value</param>
		internal void SetStoreMaximum (HdrSizeBin aSize, int aStoreIndex, int aMaximum)
		{
			aSize.SetStoreSizeMaximum (aStoreIndex, aMaximum);
		}
		#endregion StoreMaximum

		#region StoreMinimum
		//===============//
		// Store Minimum //
		//===============//
		/// <summary>
		/// Gets Store Minimum allocation constraint for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color Minimum allocation for the specified store</returns>
		public int GetStoreMinimum (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorUnitMinimum;
		}

		/// <summary>
		/// Gets Store Minimum allocation constraint for the specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID of the size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color Minimum allocation for the specified store</returns>
		public int GetStoreMinimum(int aSizeRID, int aStoreIndex)
		{
			return GetStoreMinimum(GetSizeBin(aSizeRID), aStoreIndex);
		}

		/// <summary>
		/// Gets Store Minimum allocation constraint for the specified store in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color Minimum allocation for the specified store</returns>
		internal int GetStoreMinimum(HdrSizeBin aSize, int aStoreIndex)
		{
			return aSize.GetStoreSizeMinimum(aStoreIndex);
		}

		/// <summary>
		/// Sets Store Minimum allocation for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aMinimum">Minimum Color allocation value</param>
		internal void SetStoreMinimum (int aStoreIndex, int aMinimum)
		{
			_storeColorAllocated[aStoreIndex].StoreColorUnitMinimum = aMinimum;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreMinimumStructure(), true); // MID Track 3994 Performance
		}

		/// <summary>
		/// Sets Store Minimum allocation for the specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aMinimum">Minimum Color-size allocation value</param>
		internal void SetStoreMinimum (int aSizeRID, int aStoreIndex, int aMinimum)
		{
			SetStoreMinimum(GetSizeBin(aSizeRID), aStoreIndex, aMinimum);
		}

		/// <summary>
		/// Sets Store Minimum allocation for the specified store and size in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aMinimum">Minimum Color-size allocation value</param>
		internal void SetStoreMinimum (HdrSizeBin aSize, int aStoreIndex, int aMinimum)
		{
			aSize.SetStoreSizeMinimum (aStoreIndex, aMinimum);
		}
		#endregion StoreMinimum

		#region StorePrimaryMaximum
		//======================//
		// Store PrimaryMaximum //
		//======================//
		/// <summary>
		/// Gets Store PrimaryMaximum allocation constraint for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color PrimaryMaximum allocation for the specified store</returns>
		public int GetStorePrimaryMaximum (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorUnitPrimaryMaximum;
		}

		/// <summary>
		/// Gets Store PrimaryMaximum allocation constraint for the specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID of the size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color PrimaryMaximum allocation for the specified store</returns>
		public int GetStorePrimaryMaximum(int aSizeRID, int aStoreIndex)
		{
			return GetStorePrimaryMaximum(GetSizeBin(aSizeRID), aStoreIndex);
		}

		/// <summary>
		/// Gets Store PrimaryMaximum allocation constraint for the specified store in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color PrimaryMaximum allocation for the specified store</returns>
		internal int GetStorePrimaryMaximum(HdrSizeBin aSize, int aStoreIndex)
		{
			return aSize.GetStoreSizePrimaryMaximum(aStoreIndex);
		}

		/// <summary>
		/// Sets Store PrimaryMaximum allocation for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aPrimaryMaximum">PrimaryMaximum Color allocation value</param>
		internal void SetStorePrimaryMaximum (int aStoreIndex, int aPrimaryMaximum)
		{
			_storeColorAllocated[aStoreIndex].StoreColorUnitPrimaryMaximum = aPrimaryMaximum;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStorePrimaryMaxStructure(), true); // MID Track 3994 Performance
		}

		/// <summary>
		/// Sets Store PrimaryMaximum allocation for the specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aPrimaryMaximum">PrimaryMaximum Color-size allocation value</param>
		internal void SetStorePrimaryMaximum (int aSizeRID, int aStoreIndex, int aPrimaryMaximum)
		{
			SetStorePrimaryMaximum(GetSizeBin(aSizeRID), aStoreIndex, aPrimaryMaximum);
		}

		/// <summary>
		/// Sets Store PrimaryMaximum allocation for the specified store and size in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aPrimaryMaximum">PrimaryMaximum Color-size allocation value</param>
		internal void SetStorePrimaryMaximum (HdrSizeBin aSize, int aStoreIndex, int aPrimaryMaximum)
		{
			aSize.SetStoreSizePrimaryMaximum (aStoreIndex, aPrimaryMaximum);
		}
		#endregion StorePrimaryMaximum

		#region StoreShippingStarted
		/// <summary>
		/// Gets all ship flags simultaneously for the indicated store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>byte value whose bits represent the status of each flag.</returns>
		internal byte GetAllShipFlags(int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].AllShipStatusFlags;
		}

		/// <summary>
		/// Sets all ship flags simultaneously for the indicated store
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <param name="aFlags">Flag Values.</param>
		internal void SetAllShipFlags(int aStoreIndex, byte aFlags)
		{
			_storeColorAllocated[aStoreIndex].AllShipStatusFlags = aFlags;
		}
	
		/// <summary>
		/// Gets Store Shipping Started for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True if shipping has started for this store.</returns>
		public bool GetStoreShippingStarted (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorShippingStarted;
		}

		/// <summary>
		/// Gets Store Shipping Started for the specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True if shipping has started for this store.</returns>
		public bool GetStoreShippingStarted (int aSizeRID, int aStoreIndex)
		{
			return GetStoreShippingStarted (GetSizeBin(aSizeRID), aStoreIndex);
		}

		/// <summary>
		/// Gets Store Shipping Started for the specified store and size in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True if shipping has started for this store.</returns>
		internal bool GetStoreShippingStarted (HdrSizeBin aSize, int aStoreIndex)
		{
			return aSize.GetStoreSizeShippingStarted(aStoreIndex);
		}

		/// <summary>
		/// Sets Store Shipping Started flag for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: indicates Shipping has started for this store in this color; False: no shipping activity for this store in this color.</param>
		internal void SetStoreShippingStarted(int aStoreIndex, bool aFlagValue)
		{
			_storeColorAllocated[aStoreIndex].StoreColorShippingStarted = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreShippedStatusFlagsStructure(), true); // MID Track 3994 Performance
		}

		/// <summary>
		/// Sets Store Shipping Started flag for the specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: indicates Shipping has started for this store in this color-size; False: no shipping activity for this store in this color-size.</param>
		internal void SetStoreShippingStarted(int aSizeRID, int aStoreIndex, bool aFlagValue)
		{
			SetStoreShippingStarted(GetSizeBin(aSizeRID), aStoreIndex, aFlagValue);
		}

		/// <summary>
		/// Sets Store Shipping Started flag for the specified store and size in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: indicates Shipping has started for this store in this color-size; False: no shipping activity for this store in this color-size.</param>
		internal void SetStoreShippingStarted(HdrSizeBin aSize, int aStoreIndex, bool aFlagValue)
		{
			aSize.SetStoreSizeShippingStarted(aStoreIndex, aFlagValue);
		}
		#endregion StoreShippingStarted

		#region StoreShippingComplete
		/// <summary>
		/// Gets Store Shipping Complete Flag value for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True if shipping is Complete for this store.</returns>
		public bool GetStoreShippingComplete (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorShippingComplete;
		}

		/// <summary>
		/// Gets Store Shipping Complete for the specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True if shipping is Complete for this store color size.</returns>
		public bool GetStoreShippingComplete (int aSizeRID, int aStoreIndex)
		{
			return GetStoreShippingComplete (GetSizeBin(aSizeRID), aStoreIndex);
		}

		/// <summary>
		/// Gets Store Shipping Complete for the specified store and size in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <returns>True if shipping has Complete for this store color size.</returns>
		internal bool GetStoreShippingComplete (HdrSizeBin aSize, int aStoreIndex)
		{
			return aSize.GetStoreSizeShippingComplete(aStoreIndex);
		}

		/// <summary>
		/// Sets Store Shipping Complete flag for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: indicates Shipping is Complete for this store in this color; False: no shipping activity for this store in this color.</param>
		internal void SetStoreShippingComplete(int aStoreIndex, bool aFlagValue)
		{
			_storeColorAllocated[aStoreIndex].StoreColorShippingComplete = aFlagValue;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreShippedStatusFlagsStructure(), true); // MID Track 3994 Performance
		}

		/// <summary>
		/// Sets Store Shipping Complete flag for the specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: indicates Shipping is Complete for this store in this color-size; False: shipping is not complete for this store (assuming units were allocated to the store).</param>
		internal void SetStoreShippingComplete(int aSizeRID, int aStoreIndex, bool aFlagValue)
		{
			SetStoreShippingComplete(GetSizeBin(aSizeRID), aStoreIndex, aFlagValue);
		}

		/// <summary>
		/// Sets Store Shipping Complete flag for the specified store and size in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes this size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aFlagValue">True: indicates Shipping is Complete for this store in this color-size; False: shipping is not complete for this store in this color-size (assuming units were allocated to the store in this color-size.</param>
		internal void SetStoreShippingComplete(HdrSizeBin aSize, int aStoreIndex, bool aFlagValue)
		{
			aSize.SetStoreSizeShippingComplete(aStoreIndex, aFlagValue);
		}
		#endregion StoreShippingComplete

		#region StoreUnitsShipped
		//=====================//
		// Store Units Shipped //
		//=====================//
		/// <summary>
		/// Gets StoreUnitsShipped for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color units allocated by an auto allocation process.</returns>
		public int GetStoreUnitsShipped (int aStoreIndex)
		{
			return _storeColorAllocated[aStoreIndex].StoreColorUnitsShipped;
		}

		/// <summary>
		/// Gets StoreUnitsShipped for the specified store in this color.
		/// </summary>
		/// <param name="aSizeRID">RID of the size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color-Size units allocated by an auto allocation process.</returns>
		public int GetStoreUnitsShipped (int aSizeRID, int aStoreIndex)
		{
			return GetStoreUnitsShipped (GetSizeBin(aSizeRID), aStoreIndex);
		}

		/// <summary>
		/// Gets StoreUnitsShipped for the specified store in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the size</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
		/// <returns>Color-Size units allocated by an auto allocation process.</returns>
		internal int GetStoreUnitsShipped (HdrSizeBin aSize, int aStoreIndex)
		{
			return aSize.GetStoreSizeUnitsShipped(aStoreIndex);
		}
 
		/// <summary>
		/// Sets StoreUnitsShipped for the specified store in this color.
		/// </summary>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aUnitsShipped">Color units shipped.</param>
		internal void SetStoreUnitsShipped (int aStoreIndex, int aUnitsShipped)
		{
			_storeColorAllocated[aStoreIndex].StoreColorUnitsShipped = aUnitsShipped;
			this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreQtyShippedStructure(), true); // MID Track 3994 Performance
		}

		/// <summary>
		/// Sets StoreUnitsShipped for the specified store and size in this color.
		/// </summary>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aUnitsShipped">Color-size units Shipped.</param>
		internal void SetStoreUnitsShipped (int aSizeRID, int aStoreIndex, int aUnitsShipped)
		{
			SetStoreUnitsShipped(GetSizeBin(aSizeRID), aStoreIndex, aUnitsShipped);
		}

		/// <summary>
		/// Sets StoreUnitsShipped for the specified store and size in this color.
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the size.</param>
		/// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
		/// <param name="aUnitsShipped">Color-size units shipped.</param>
		internal void SetStoreUnitsShipped (HdrSizeBin aSize, int aStoreIndex, int aUnitsShipped)
		{
			aSize.SetStoreSizeUnitsShipped(aStoreIndex, aUnitsShipped);
		}
		#endregion StoreUnitsShipped

        // begin TT#246 - MD - JEllis - AnF VSW In-Store Minimum pt 5
        #region StoreItemMinimum
        /// <summary>
        /// Gets Store Color Item Ideal Minimum for the specified store.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID)</param>
        /// <returns>Color Item Ideal Minimum for the specified store</returns>
        public int GetStoreItemIdealMinimum(int aStoreIndex)
        {
            return _storeColorAllocated[aStoreIndex].StoreSizeItemIdealMinimum;
        }

        /// <summary>
        /// Sets Store Color Item Ideal Minimum for the specified store.
        /// </summary>
        /// <param name="aStoreIndex">Index identifier for the store (not RID).</param>
        /// <param name="aMinimum">Size Item Ideal Minimum value</param>
        internal void SetStoreItemIdealMinimum(int aStoreIndex, int aMinimum)
        {
            _storeColorAllocated[aStoreIndex].StoreSizeItemIdealMinimum = aMinimum;
            this._sass.SetSQL_StructureChange(StoreAllocationQuickRequest.GetStoreItemMinimumStructure(), true);
        }
        // end TT#246 - MD - JEllis - AnF VSW In STore Minimum phase 2
        #endregion StoreItemMinimum
        // end TT#246 - MD - JEllis - AnF VSW In-Store Minimum pt 5
		#endregion Store
		#endregion Methods
	}
	#endregion HdrColorBin

	#region HdrColorBinProcessOrder
	/// <summary>
	/// Used to determine the order in which to process multiple HdrColorBins
	/// </summary>
	public class HdrColorBinProcessOrder:IComparer
	{
		public int Compare(object x, object y)
		{
			if (!(x is HdrColorBin) | !(y is HdrColorBin))
			{
				throw new MIDException(eErrorLevel.severe,
					(int)(eMIDTextCode.msg_al_CompareObjectMustBeHdrColorBin),
					MIDText.GetText(eMIDTextCode.msg_al_CompareObjectMustBeHdrColorBin));
			}
			if (x == null || y == null)
			{
				if (y != null)
				{
					return -1;
				}
				if (x != null)
				{
					return +1;
				}
				return 0;
			}
			int xUnits = (((HdrColorBin)x).ColorUnitsToAllocate
				- ((HdrColorBin)x).ColorUnitsAllocated);
			int yUnits = (((HdrColorBin)y).ColorUnitsToAllocate
				- ((HdrColorBin)y).ColorUnitsAllocated);
			if (xUnits < yUnits)
			{
				return -1;
			}
			if (xUnits > yUnits)
			{
				return +1;
			}
			if (((HdrColorBin)x).ColorMultiple < ((HdrColorBin)y).ColorMultiple)
			{
				return -1;
			}
			if (((HdrColorBin)x).ColorMultiple > ((HdrColorBin)y).ColorMultiple)
			{
				return +1;
			}
			if (((HdrColorBin)x).ColorCodeRID > ((HdrColorBin)y).ColorCodeRID)
			{
				return -1;
			}
			if (((HdrColorBin)x).ColorCodeRID < ((HdrColorBin)y).ColorCodeRID)
			{
				return +1;
			}
			return 0;
		}
	}
	#endregion HdrColorBinProcessOrder

	#region AllocationCapacityBin
	/// <summary>
	/// Structure to indicate when an allocation may exeed capacity.
	/// </summary>
	/// <remarks>
	/// This structure contains the following information:
	/// <list type="bullet">
	/// <item>StoreGroupLevelRID: Identifies the group of stores that may exceed capacity.</item>
	/// <item>ExceedCapacity: True indicates these stores may exceed capacity; false indicates these stores may not exceed capacity.</item>
	/// <item>ExceedByPercent: The percentage amount these stores may exceed capacity.</item>
	/// </list>
	/// </remarks>
	public class AllocationCapacityBin
	{
		//=======
		// FIELDS
		//=======
		int _sglRID;
		bool _exceedCapacity;
		double _exceedByPercent;

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets store group level RID.
		/// </summary>
		public int StoreGroupLevelRID
		{
			get 
			{
				return _sglRID;
			}
		}

		/// <summary>
		/// Gets Exceed Capacity flag value.
		/// </summary>
		public bool ExceedCapacity
		{
			get
			{
				return _exceedCapacity;
			}
		}

		/// <summary>
		/// Gets Exceed Capacity Percent.
		/// </summary>
		public double ExceedCapacityByPercent
		{
			get
			{
				return _exceedByPercent;
			}
		}

		//========
		// METHODS
		//========
		/// <summary>
		/// Sets store grouplevel RID
		/// </summary>
		/// <param name="aRID">RID of teh store group level.</param>
		internal void SetStoreGroupLevelRID(int aRID)
		{
			_sglRID = aRID;
		}
		 
		/// <summary>
		/// Sets Exceed Capacity flag value.
		/// </summary>
		/// <param name="aExceedCapacity">An exceed capacity flag value: true or false.</param>
		internal void SetExceedCapacity(bool aExceedCapacity)
		{
			_exceedCapacity = aExceedCapacity;
		}

		/// <summary>
		/// Sets Exceed Capacity by Percent value
		/// </summary>
		/// <param name="aPercent">A Percent value.</param>
		internal void SetExceedCapacityByPercent(double aPercent)
		{
			// RonM commented out edit for MIDTrack 1345
			//if (aPercent < 0)
			//{
			//	throw new MIDException (eErrorLevel.severe,
			//		(int)eMIDTextCode.msg_CapacityPctCannotBeNeg,
			//		MIDText.GetText(eMIDTextCode.msg_CapacityPctCannotBeNeg));
			//}
			_exceedByPercent = aPercent;
		}
	}
	#endregion HdrCapacityBin

	#region ColorOrSizeMinMaxBin
	/// <summary>
	/// Container to hold color or size minimum and maximum allocation constraints.
	/// </summary>
	public class ColorOrSizeMinMaxBin
	{
		//=======
		// FIELDS
		//=======
		int _ColorOrSizeRID;
		MinMaxAllocationBin _MinMax;

		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of this class.
		/// </summary>
		/// <param name="aKey">Color or Size RID associated with this bin.</param>
		public ColorOrSizeMinMaxBin(int aKey)
		{
			_ColorOrSizeRID = aKey;
			_MinMax.SetMinimum(0);
			_MinMax.SetMaximum(_MinMax.LargestMaximum);
		}

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets Color or size key which identifies this bin.
		/// </summary>
		public int ColorOrSizeRID
		{
			get
			{
				return _ColorOrSizeRID;
			}
		}

		/// <summary>
		/// Gets the maximum constraint
		/// </summary>
		public int Maximum
		{
			get
			{
				return _MinMax.Maximum;
			}
		}

		/// <summary>
		/// Gets the largest possible maximum.
		/// </summary>
		public int LargestMaximum
		{
			get
			{
				return _MinMax.LargestMaximum;
			}
		}

		/// <summary>
		/// Gets the minimum constraint
		/// </summary>
		public int Minimum
		{
			get
			{
				return _MinMax.Minimum;
			}
		}

		//========
		// METHODS
		//========
		/// <summary>
		/// Sets color or size key which identifies this bin
		/// </summary>
		/// <param name="aRID"></param>
		internal void SetColorOrSizeRID(int aRID)
		{
			_ColorOrSizeRID = aRID;
		}
 
		/// <summary>
		/// Sets the maximum constraint
		/// </summary>
		/// <param name="aMaximum">A maximum value.</param>
		internal void SetMaximum (int aMaximum)
		{
			_MinMax.SetMaximum(aMaximum);
		}
         
		/// <summary>
		/// Sets minimum constraint
		/// </summary>
		/// <param name="aMinimum">A minimum value.</param>
		internal void SetMinimum (int aMinimum)
		{
			_MinMax.SetMinimum(aMinimum);
		}
	}
	#endregion ColorOrSizeMinMaxBin

	#region ColorSizeMinMaxBin
	/// <summary>
	/// Container to hold color-size minimum and maximum allocation constraints.
	/// </summary>
	public class ColorSizeMinMaxBin
	{
		//=======
		// FIELDS
		//=======
		int _ColorRID;
		MinMaxAllocationBin _allSize;
		Hashtable _sizeMinMax;

		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of this class
		/// </summary>
		/// <param name="aColorRID">Color RID</param>
		public ColorSizeMinMaxBin (int aColorRID)
		{
			_ColorRID = aColorRID;
			_allSize.SetMaximum(_allSize.LargestMaximum);
			_allSize.SetMinimum(0);
		}

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets which identifies the color.
		/// </summary>
		public int ColorRID
		{
			get
			{
				return _ColorRID;
			}
		}

		/// <summary>
		/// Gets or sets the All Size Minimum 
		/// </summary>
		public int AllSizeMinimum
		{
			get
			{
				return _allSize.Minimum;
			}
		}
		
		/// <summary>
		/// Gets or sets the All Size Maximum
		/// </summary>
		public int AllSizeMaximum
		{
			get
			{
				return _allSize.Maximum;
			}
		}

		/// <summary>
		/// Gets the hashtable containing specific size mins and max's. An entry in this table is a  ColorOrSizeMinMaxBin class.
		/// </summary>
		public Hashtable SizeMinMaxTable
		{
			get
			{
				return _sizeMinMax;
			}
		}

		//========
		// METHODS
		//========
		/// <summary>
		/// Sets all size minimum
		/// </summary>
		/// <param name="aMinimum">A minimum value</param>
		internal void SetAllSizeMinimum(int aMinimum)
		{
			_allSize.SetMinimum(aMinimum);
		}

		/// <summary>
		/// Sets all size maximum
		/// </summary>
		/// <param name="aMaximum">A maximum value.</param>
		internal void SetAllSizeMaximum(int aMaximum)
		{
			_allSize.SetMaximum(aMaximum);
		}
		
		/// <summary>
		/// Determines if the a size min/max is defined for the method's color.
		/// </summary>
		/// <param name="aSizeRID">Size RID</param>
		/// <returns></returns>
		public bool IsSizeMinMaxInMethodColor(int aSizeRID)
		{
			if (_sizeMinMax == null)
			{
				return false;
			}
			if (!(_sizeMinMax.Contains(aSizeRID)))
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Add a size min/max to the method's color.
		/// </summary>
		/// <param name="aSizeRID">Size RID that identifies the size.</param>
		internal void AddSizeMinMaxToMethodColor (int aSizeRID)
		{
			if (IsSizeMinMaxInMethodColor(aSizeRID))
			{
				throw new MIDException (eErrorLevel.warning,
					(int)eMIDTextCode.msg_DuplicateColorSizeNotAllowed,
					MIDText.GetText(eMIDTextCode.msg_DuplicateColorSizeNotAllowed));
			}
			ColorOrSizeMinMaxBin sizeMinMaxBin = new ColorOrSizeMinMaxBin(aSizeRID);
			_sizeMinMax.Add(aSizeRID, sizeMinMaxBin);
		}

		/// <summary>
		/// Removes size min's and max's from method's color.
		/// </summary>
		/// <param name="aSizeRID">Size RID</param>
		internal void RemoveSizeMinMax (int aSizeRID)
		{
			if (IsSizeMinMaxInMethodColor(aSizeRID))
			{
				return;
			}
			_sizeMinMax.Remove(aSizeRID);
		}

		/// <summary>
		/// Gets size maximum constraint for the specified size.
		/// </summary>
		/// <param name="aSizeRID">Size RID</param>
		/// <returns>Maximum constraint value for the size.</returns>
		public int GetSizeMaximum (int aSizeRID)
		{
			if (!(IsSizeMinMaxInMethodColor(aSizeRID)))
			{
				return AllSizeMaximum;
			}
			ColorOrSizeMinMaxBin sizeMinMaxBin = (ColorOrSizeMinMaxBin)_sizeMinMax[aSizeRID];
			return sizeMinMaxBin.Maximum;
		}

		/// <summary>
		/// Sets the size maximum constraint for the specified size.
		/// </summary>
		/// <param name="aSizeRID">Size RID</param>
		/// <param name="aMaximum">Maximum value</param>
		internal void SetSizeMaximum (int aSizeRID, int aMaximum)
		{
			ColorOrSizeMinMaxBin sizeMinMaxBin;
			if (!(IsSizeMinMaxInMethodColor(aSizeRID)))
			{
				this.AddSizeMinMaxToMethodColor(aSizeRID);
			}
			sizeMinMaxBin = (ColorOrSizeMinMaxBin)_sizeMinMax[aSizeRID];
			sizeMinMaxBin.SetMaximum(aMaximum);
		}

		/// <summary>
		/// Gets size Minimum constraint for the specified size.
		/// </summary>
		/// <param name="aSizeRID">Size RID</param>
		/// <returns>Minimum constraint value for the size.</returns>
		public int GetSizeMinimum (int aSizeRID)
		{
			if (!(IsSizeMinMaxInMethodColor(aSizeRID)))
			{
				return AllSizeMinimum;
			}
			ColorOrSizeMinMaxBin sizeMinMaxBin = (ColorOrSizeMinMaxBin)_sizeMinMax[aSizeRID];
			return sizeMinMaxBin.Minimum;
		}

		/// <summary>
		/// Sets the size Minimum constraint for the specified size.
		/// </summary>
		/// <param name="aSizeRID">Size RID</param>
		/// <param name="aMinimum">Minimum value.</param>
		internal void SetSizeMinimum (int aSizeRID, int aMinimum)
		{
			ColorOrSizeMinMaxBin sizeMinMaxBin;
			if (!(IsSizeMinMaxInMethodColor(aSizeRID)))
			{
				AddSizeMinMaxToMethodColor(aSizeRID);
			}
			sizeMinMaxBin = (ColorOrSizeMinMaxBin)_sizeMinMax[aSizeRID];
			sizeMinMaxBin.SetMinimum(aMinimum);
		}
	}
	#endregion ColorSizeMinMaxBin

	#region AllocationGradeBin
    // begin TT#488 - MD - Jellis - Activate Group Allocation Method
    /// <summary>
    /// Root class for AllocationGradeBin
    /// </summary>
    public class AllocationGradeBinRoot
    {
        //=======
        // FIELDS
        //=======
        private string _grade;
        private double _boundary;
        private int _gradeSglRID;  
        private bool _isNew;
        private bool _isChanged;

        //=============
        // CONSTRUCTORS
        //=============
        public AllocationGradeBinRoot()
        {
            _isNew = false;
            _isChanged = false;
            _grade = null;
            _boundary = 0.0;
            _gradeSglRID = Include.AllStoreGroupLevelRID; 
        }

        //===========
        // PROPERTIES
        //===========
        /// <summary>
        /// Gets or sets IsChanged database update flag
        /// </summary>
        internal bool GradeIsChanged
        {
            get
            {
                return _isChanged;
            }
            set
            {
                _isChanged = value;
            }
        }

        /// <summary>
        /// Gets or sets IsNew database update flag
        /// </summary>
        internal bool GradeIsNew
        {
            get
            {
                return _isNew;
            }
            set
            {
                _isNew = value;
            }
        }


        /// <summary>
        /// Gets the grade.
        /// </summary>
        public string Grade
        {
            get
            {
                return _grade;
            }
        }

        /// <summary>
        /// Gets the grade's lower boundary definition.
        /// </summary>
        public double LowBoundary
        {
            get
            {
                return _boundary;
            }
        }
        public int GradeSglRID
        {
            get
            {
                return _gradeSglRID;
            }
        }

        //========
        // METHODS
        //========
        /// <summary>
        /// Sets the grade name or id.
        /// </summary>
        /// <param name="aGrade">A grade name or id.</param>
        internal void SetGrade(string aGrade)
        {
            _grade = aGrade;
        }

        /// <summary>
        /// Sets the grade's lower boundary definition.
        /// </summary>
        /// <param name="aLowBoundary"></param>
        internal void SetLowBoundary(double aLowBoundary)
        {
            if (aLowBoundary < 0)
            {
                throw new MIDException(eErrorLevel.severe,
                    (int)eMIDTextCode.msg_GradeBoundaryCannotBeNeg,
                    MIDText.GetText(eMIDTextCode.msg_GradeBoundaryCannotBeNeg));
            }
            _boundary = aLowBoundary;
        }

        /// <summary>
        /// Sets grade atrribute set
        /// </summary>
        internal void SetGradeAttributeSet(int aGradeSglRID)
        {
            _gradeSglRID = aGradeSglRID;
        }
    }
    /// <summary>
    /// Container for grade allocation constraints.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This structure holds the definition of a grade and the associated allocation constraints.
    /// </para><para>
    /// A grade is a partition of stores based on how they compare as a percentage to an average store
    /// (the quantity being compared is usually sales values over a specified time period).
    /// </para><para> 
    /// So a grade is defined as a range of percentages with a lower boundary and an upper boundary.
    /// The lowest possible boundary for a grade is 0. No two grades can have the same lower boundary.
    /// </para><para>
    /// The upper boundary for any grade is not always defined (ie. it may be infinite). When there are
    /// multiple grades, they are associated with their lower boundary and sorted in that order. So an
    /// upper boundary exists for a given grade if there is another grade defined with a lower boundary that
    /// is greater than the given grade's lower boundary.
    /// </para><para>
    /// The grade to which a store belongs is the grade with the largest lower boundary not greater than
    /// the store's percent to average. 
    /// </para><para>   
    /// The following information is contained in this structure:
    /// <list type="bullet">
    /// <item>Grade: A text definition of the grade.</item>
    /// <item>LowBoundary: A non-negative percentage that identifies the smallest possible relationship to the average.</item>
    /// <item>GradeMinimum: The minimum allocation constraint for any store within the grade.</item>
    /// <item>GradeMaximum: The maximum allocation constraint for any store within the grade.</item>
    /// <item>GradeAdMinimum: The minimum allocation constraint for promotional events for any store within the grade.</item>
    /// <item>GradeColorMinimum: The minimum color allocation constraint for any store within the grade.</item>
    /// <item>GradeColorMaximum: The maximum color allocation constraint for any store within the grade.</item>
    /// </list>
    /// </para>
    /// </remarks>
    public class AllocationGradeBin:AllocationGradeBinRoot
    {
        //=======
        // FIELDS
        //=======
        private MinMaxAllocationBin _gradeMinMax;
        private MinMaxAllocationBin _origGradeMinMax; 
        private int _gradeAdMinimum;
        private int _origGradeAdMinimum;
        private MinMaxAllocationBin _gradeColorMinMax;
        //private int _gradeShipUpTo;  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)  // TT#488 - MD - Jellis - Group Allocation (field not used) 
        //private int _gradeSglRID;         // TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35) // TT#488 - MD - Jellis - Group Allocation (did not intend to "override" base)
        //private bool _isNew;             // TT#488 - MD - Jellis - Group Allocation (field not used) 
        //private bool _isChanged;         // TT#488 - MD - Jellis - Group Allocation (field not used)  

        //=============
        // CONSTRUCTORS
        //=============
        public AllocationGradeBin()
        {
            _gradeMinMax.SetMaximum(_gradeMinMax.LargestMaximum);
            _gradeMinMax.SetMinimum(0);
            _gradeMinMax.SetShipUpTo(0);
            _origGradeMinMax.SetMaximum(_gradeMinMax.LargestMaximum);
            _origGradeMinMax.SetMinimum(0);
        }

        //===========
        // PROPERTIES
        //===========
        /// <summary>
        /// Gets the grade's minimum allocation constraint.
        /// </summary>
        public int GradeMinimum
        {
            get
            {
                return _gradeMinMax.Minimum;
            }
        }

        /// <summary>
        /// Gets the grade's original minimum allocation constraint (before adjustments).
        /// </summary>
        public int GradeOriginalMinimum
        {
            get
            {
                return _origGradeMinMax.Minimum;
            }
        }

        /// <summary>
        /// Gets the grade's maximum allocation constraint.
        /// </summary>
        public int GradeMaximum
        {
            get
            {
                return _gradeMinMax.Maximum;
            }
        }
        /// <summary>
        /// Gets the grade's original maximum allocation constraint (before adjustments).
        /// </summary>
        public int GradeOriginalMaximum
        {
            get
            {
                return _origGradeMinMax.Maximum;
            }
        }

        /// <summary>
        /// Gets largest possible grade maximum.
        /// </summary>
        public int GradeLargestMaximum
        {
            get
            {
                return _gradeMinMax.LargestMaximum;
            }
        }

        /// <summary>
        /// Gets the grade's AdMinimum allocation constraint.
        /// </summary>
        public int GradeAdMinimum
        {
            get
            {
                return _gradeAdMinimum;
            }
        }

        /// <summary>
        /// Gets the grade's original Ad minimum allocation constraint (before adjustments).
        /// </summary>
        public int GradeOriginalAdMinimum
        {
            get
            {
                return this._origGradeAdMinimum;
            }
        }

        /// <summary>
        /// Gets the grade's color minimum allocation constraint.
        /// </summary>
        public int GradeColorMinimum
        {
            get
            {
                return _gradeColorMinMax.Minimum;
            }
        }

        /// <summary>
        /// Gets the grade's color maximum allocation constraint.
        /// </summary>
        public int GradeColorMaximum
        {
            get
            {
                return _gradeColorMinMax.Maximum;
            }
        }
        public int GradeShipUpTo
        {
            get
            {
                return _gradeMinMax.ShipUpTo;
            }
        }
        /// <summary>
        /// Gets the largest possible color maximum.
        /// </summary>
        public int GradeColorLargestMaximum
        {
            get
            {
                return _gradeColorMinMax.LargestMaximum;
            }
        }
        // begin TT#488 - MD - Jellis - Group Allocation (did not intend to "override" base)
        //public int GradeSglRID
        //{
        //    get
        //    {
        //        return _gradeSglRID;
        //    }
        //}
        // end TT#488 - MD - Jellis - Group Allocation (did not intent to "override" base)

        //========
        // METHODS
        //========
        /// <summary>
        /// Sets Grade minimum
        /// </summary>
        /// <param name="aMinimum">Minimum value</param>
        internal void SetGradeMinimum(int aMinimum)
        {
            _gradeMinMax.SetMinimum(aMinimum);
        }

        /// <summary>
        /// Sets Grade original minimum before any adjustments
        /// </summary>
        /// <param name="aMinimum">Minimum value</param>
        internal void SetGradeOriginalMinimum(int aMinimum)
        {
            _origGradeMinMax.SetMinimum(aMinimum);
        }

        /// <summary>
        /// Sets grade allocation maximum.
        /// </summary>
        /// <param name="aMaximum">A maximum value.</param>
        internal void SetGradeMaximum(int aMaximum)
        {
            _gradeMinMax.SetMaximum(aMaximum);
        }

        /// <summary>
        /// Sets Grade original maximum before any adjustments
        /// </summary>
        /// <param name="aMaximum">Maximum value</param>
        internal void SetGradeOriginalMaximum(int aMaximum)
        {
            _origGradeMinMax.SetMaximum(aMaximum);
        }

        /// <summary>
        /// Sets grade allocation ad minimum
        /// </summary>
        /// <param name="aMinimum">A minimum value.</param>
        internal void SetGradeAdMinimum(int aMinimum)
        {
            _gradeAdMinimum = aMinimum;
        }

        /// <summary>
        /// Sets Grade original Ad minimum before any adjustments
        /// </summary>
        /// <param name="aMinimum">Minimum value</param>
        internal void SetGradeOriginalAdMinimum(int aMinimum)
        {
            this._origGradeAdMinimum = aMinimum;
        }

        /// <summary>
        /// Sets grade color minimum
        /// </summary>
        /// <param name="aMinimum">A minimum value.</param>
        internal void SetGradeColorMinimum(int aMinimum)
        {
            _gradeColorMinMax.SetMinimum(aMinimum);
        }

        /// <summary>
        /// Sets grade color maximum
        /// </summary>
        /// <param name="aMaximum">A maximum value.</param>
        internal void SetGradeColorMaximum(int aMaximum)
        {
            _gradeColorMinMax.SetMaximum(aMaximum);
        }

        /// <summary>
        /// Sets grade ShipUpTo
        /// </summary>
        internal void SetGradeShipUpTo(int aGradeShipUpTo)
        {
            _gradeMinMax.SetShipUpTo(aGradeShipUpTo);
        }
    }
    public class GroupAllocationGradeBin : AllocationGradeBin
    {
        //=======
        // FIELDS
        //=======
        private MinMaxAllocationBin _groupGradeMinMax;
        private MinMaxAllocationBin _origGroupGradeMinMax;

        //=============
        // CONSTRUCTORS
        //=============
        public GroupAllocationGradeBin()
        {
            _groupGradeMinMax.SetMaximum(_groupGradeMinMax.LargestMaximum);
            _groupGradeMinMax.SetMinimum(0);
            _groupGradeMinMax.SetShipUpTo(0);
            _origGroupGradeMinMax.SetMaximum(_groupGradeMinMax.LargestMaximum);
            _origGroupGradeMinMax.SetMinimum(0);
        }

        //===========
        // PROPERTIES
        //===========
        /// <summary>
        /// Gets the group grade's minimum allocation constraint.
        /// </summary>
        public int GroupGradeMinimum
        {
            get
            {
                return _groupGradeMinMax.Minimum;
            }
        }

        /// <summary>
        /// Gets the group grade's original minimum allocation constraint (before adjustments).
        /// </summary>
        public int GroupGradeOriginalMinimum
        {
            get
            {
                return _origGroupGradeMinMax.Minimum;
            }
        }

        /// <summary>
        /// Gets the group grade's maximum allocation constraint.
        /// </summary>
        public int GroupGradeMaximum
        {
            get
            {
                return _groupGradeMinMax.Maximum;
            }
        }
        /// <summary>
        /// Gets the group grade's original maximum allocation constraint (before adjustments).
        /// </summary>
        public int GroupGradeOriginalMaximum
        {
            get
            {
                return _origGroupGradeMinMax.Maximum;
            }
        }

        /// <summary>
        /// Gets largest possible group grade maximum.
        /// </summary>
        public int GroupGradeLargestMaximum
        {
            get
            {
                return _groupGradeMinMax.LargestMaximum;
            }
        }

        public int GroupGradeShipUpTo
        {
            get
            {
                return _groupGradeMinMax.ShipUpTo;
            }
        }

        //========
        // METHODS
        //========
        /// <summary>
        /// Sets Group Grade minimum
        /// </summary>
        /// <param name="aMinimum">Minimum value</param>
        internal void SetGroupGradeMinimum(int aMinimum)
        {
            _groupGradeMinMax.SetMinimum(aMinimum);
        }

        /// <summary>
        /// Sets Group Grade original minimum before any adjustments
        /// </summary>
        /// <param name="aMinimum">Minimum value</param>
        internal void SetGroupGradeOriginalMinimum(int aMinimum)
        {
            _origGroupGradeMinMax.SetMinimum(aMinimum);
        }

        /// <summary>
        /// Sets group grade allocation maximum.
        /// </summary>
        /// <param name="aMaximum">A maximum value.</param>
        internal void SetGroupGradeMaximum(int aMaximum)
        {
            _groupGradeMinMax.SetMaximum(aMaximum);
        }

        /// <summary>
        /// Sets Group Grade original maximum before any adjustments
        /// </summary>
        /// <param name="aMaximum">Maximum value</param>
        internal void SetGroupGradeOriginalMaximum(int aMaximum)
        {
            _origGroupGradeMinMax.SetMaximum(aMaximum);
        }
    }    

    ///// <summary>
    ///// Container for grade allocation constraints.
    ///// </summary>
    ///// <remarks>
    ///// <para>
    ///// This structure holds the definition of a grade and the associated allocation constraints.
    ///// </para><para>
    ///// A grade is a partition of stores based on how they compare as a percentage to an average store
    ///// (the quantity being compared is usually sales values over a specified time period).
    ///// </para><para> 
    ///// So a grade is defined as a range of percentages with a lower boundary and an upper boundary.
    ///// The lowest possible boundary for a grade is 0. No two grades can have the same lower boundary.
    ///// </para><para>
    ///// The upper boundary for any grade is not always defined (ie. it may be infinite). When there are
    ///// multiple grades, they are associated with their lower boundary and sorted in that order. So an
    ///// upper boundary exists for a given grade if there is another grade defined with a lower boundary that
    ///// is greater than the given grade's lower boundary.
    ///// </para><para>
    ///// The grade to which a store belongs is the grade with the largest lower boundary not greater than
    ///// the store's percent to average. 
    ///// </para><para>   
    ///// The following information is contained in this structure:
    ///// <list type="bullet">
    ///// <item>Grade: A text definition of the grade.</item>
    ///// <item>LowBoundary: A non-negative percentage that identifies the smallest possible relationship to the average.</item>
    ///// <item>GradeMinimum: The minimum allocation constraint for any store within the grade.</item>
    ///// <item>GradeMaximum: The maximum allocation constraint for any store within the grade.</item>
    ///// <item>GradeAdMinimum: The minimum allocation constraint for promotional events for any store within the grade.</item>
    ///// <item>GradeColorMinimum: The minimum color allocation constraint for any store within the grade.</item>
    ///// <item>GradeColorMaximum: The maximum color allocation constraint for any store within the grade.</item>
    ///// </list>
    ///// </para>
    ///// </remarks>
    //public class AllocationGradeBin
    //{
    //    //=======
    //    // FIELDS
    //    //=======
    //    private string _grade;
    //    private double _boundary;
    //    private MinMaxAllocationBin _gradeMinMax;
    //    private MinMaxAllocationBin _origGradeMinMax; // MID Track 3097 Min/max not working for packs.
    //    private int _gradeAdMinimum;
    //    private int _origGradeAdMinimum;   // MID track 3097 Min/max not working for packs
    //    private MinMaxAllocationBin _gradeColorMinMax;
    //    private int _gradeShipUpTo;  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39) 
    //    private int _gradeSglRID;         // TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
    //    private bool _isNew;
    //    private bool _isChanged;

    //    //=============
    //    // CONSTRUCTORS
    //    //=============
    //    public AllocationGradeBin()
    //    {
    //        _isNew = false;
    //        _isChanged = false;
    //        _grade = null;
    //        _boundary = 0.0;
    //        _gradeMinMax.SetMaximum(_gradeMinMax.LargestMaximum);
    //        _gradeMinMax.SetMinimum(0);
    //        //BEGIN MID Track 3097 Min/max not working for packs
    //        _origGradeMinMax.SetMaximum(_gradeMinMax.LargestMaximum);
    //        _origGradeMinMax.SetMinimum(0);
    //        _gradeShipUpTo = (0);  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
    //        //END MID Track 3097 Min/max not working for packs
    //        _gradeSglRID = Include.AllStoreGroupLevelRID; // TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
    //    }

    //    //===========
    //    // PROPERTIES
    //    //===========
    //    /// <summary>
    //    /// Gets or sets IsChanged database update flag
    //    /// </summary>
    //    internal bool GradeIsChanged
    //    {
    //        get
    //        {
    //            return _isChanged;
    //        }
    //        set
    //        {
    //            _isChanged = value;
    //        }
    //    }

    //    /// <summary>
    //    /// Gets or sets IsNew database update flag
    //    /// </summary>
    //    internal bool GradeIsNew
    //    {
    //        get
    //        {
    //            return _isNew;
    //        }
    //        set
    //        {
    //            _isNew = value;
    //        }
    //    }


    //    /// <summary>
    //    /// Gets the grade.
    //    /// </summary>
    //    public string Grade
    //    {
    //        get
    //        {
    //            return _grade;
    //        }
    //    }

    //    /// <summary>
    //    /// Gets the grade's lower boundary definition.
    //    /// </summary>
    //    public double LowBoundary
    //    {
    //        get
    //        {
    //            return _boundary;
    //        }
    //    }

    //    /// <summary>
    //    /// Gets the grade's minimum allocation constraint.
    //    /// </summary>
    //    public int GradeMinimum
    //    {
    //        get
    //        {
    //            return _gradeMinMax.Minimum;
    //        }
    //    }

    //    // BEGIN MID Track 3097 Mins/Max not working for packs
    //    /// <summary>
    //    /// Gets the grade's original minimum allocation constraint (before adjustments).
    //    /// </summary>
    //    public int GradeOriginalMinimum
    //    {
    //        get
    //        {
    //            return _origGradeMinMax.Minimum;
    //        }
    //    }
    //    // END MID Track 3097 Mins/Max not working for packs

    //    /// <summary>
    //    /// Gets the grade's maximum allocation constraint.
    //    /// </summary>
    //    public int GradeMaximum
    //    {
    //        get
    //        {
    //            return _gradeMinMax.Maximum;
    //        }
    //    }
    //    // BEGIN MID Track 3097 Mins/Max not working for packs
    //    /// <summary>
    //    /// Gets the grade's original maximum allocation constraint (before adjustments).
    //    /// </summary>
    //    public int GradeOriginalMaximum
    //    {
    //        get
    //        {
    //            return _origGradeMinMax.Maximum;
    //        }
    //    }
    //    // END MID Track 3097 Mins/Max not working for packs


    //    /// <summary>
    //    /// Gets largest possible grade maximum.
    //    /// </summary>
    //    public int GradeLargestMaximum
    //    {
    //        get
    //        {
    //            return _gradeMinMax.LargestMaximum;
    //        }
    //    }

    //    /// <summary>
    //    /// Gets the grade's AdMinimum allocation constraint.
    //    /// </summary>
    //    public int GradeAdMinimum
    //    {
    //        get
    //        {
    //            return _gradeAdMinimum;
    //        }
    //    }

    //    // BEGIN MID Track 3097 Mins/Max not working for packs
    //    /// <summary>
    //    /// Gets the grade's original Ad minimum allocation constraint (before adjustments).
    //    /// </summary>
    //    public int GradeOriginalAdMinimum
    //    {
    //        get
    //        {
    //            return this._origGradeAdMinimum;
    //        }
    //    }
    //    // END MID Track 3097 Mins/Max not working for packs

    //    /// <summary>
    //    /// Gets the grade's color minimum allocation constraint.
    //    /// </summary>
    //    public int GradeColorMinimum
    //    {
    //        get
    //        {
    //            return _gradeColorMinMax.Minimum;
    //        }
    //    }

    //    /// <summary>
    //    /// Gets the grade's color maximum allocation constraint.
    //    /// </summary>
    //    public int GradeColorMaximum
    //    {
    //        get
    //        {
    //            return _gradeColorMinMax.Maximum;
    //        }
    //    }
    //    // BEGIN TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
    //    public int GradeShipUpTo
    //    {
    //        get
    //        {
    //            return _gradeShipUpTo;
    //        }
    //    }
    //    // END TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
    //    /// <summary>
    //    /// Gets the largest possible color maximum.
    //    /// </summary>
    //    public int GradeColorLargestMaximum
    //    {
    //        get
    //        {
    //            return _gradeColorMinMax.LargestMaximum;
    //        }
    //    }
    //    // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
    //    public int GradeSglRID
    //    {
    //        get
    //        {
    //            return _gradeSglRID;
    //        }
    //    }
    //    // End TT#618

    //    //========
    //    // METHODS
    //    //========
    //    /// <summary>
    //    /// Sets the grade name or id.
    //    /// </summary>
    //    /// <param name="aGrade">A grade name or id.</param>
    //    internal void SetGrade(string aGrade)
    //    {
    //        _grade = aGrade;
    //    }

    //    /// <summary>
    //    /// Sets the grade's lower boundary definition.
    //    /// </summary>
    //    /// <param name="aLowBoundary"></param>
    //    internal void SetLowBoundary(double aLowBoundary)
    //    {
    //        if (aLowBoundary < 0)
    //        {
    //            throw new MIDException(eErrorLevel.severe,
    //                (int)eMIDTextCode.msg_GradeBoundaryCannotBeNeg,
    //                MIDText.GetText(eMIDTextCode.msg_GradeBoundaryCannotBeNeg));
    //        }
    //        _boundary = aLowBoundary;
    //    }

    //    /// <summary>
    //    /// Sets Grade minimum
    //    /// </summary>
    //    /// <param name="aMinimum">Minimum value</param>
    //    internal void SetGradeMinimum(int aMinimum)
    //    {
    //        _gradeMinMax.SetMinimum(aMinimum);
    //    }

    //    // BEGIN MID Track #3097 Min/Max not working for pack
    //    /// <summary>
    //    /// Sets Grade original minimum before any adjustments
    //    /// </summary>
    //    /// <param name="aMinimum">Minimum value</param>
    //    internal void SetGradeOriginalMinimum(int aMinimum)
    //    {
    //        _origGradeMinMax.SetMinimum(aMinimum);
    //    }
    //    // END MID Track #3097 Min/Max not working for pack

    //    /// <summary>
    //    /// Sets grade allocation maximum.
    //    /// </summary>
    //    /// <param name="aMaximum">A maximum value.</param>
    //    internal void SetGradeMaximum(int aMaximum)
    //    {
    //        _gradeMinMax.SetMaximum(aMaximum);
    //    }

    //    // BEGIN MID Track #3097 Min/Max not working for pack
    //    /// <summary>
    //    /// Sets Grade original maximum before any adjustments
    //    /// </summary>
    //    /// <param name="aMaximum">Maximum value</param>
    //    internal void SetGradeOriginalMaximum(int aMaximum)
    //    {
    //        _origGradeMinMax.SetMaximum(aMaximum);
    //    }
    //    // END MID Track #3097 Min/Max not working for pack

    //    /// <summary>
    //    /// Sets grade allocation ad minimum
    //    /// </summary>
    //    /// <param name="aMinimum">A minimum value.</param>
    //    internal void SetGradeAdMinimum(int aMinimum)
    //    {
    //        _gradeAdMinimum = aMinimum;
    //    }

    //    // BEGIN MID Track #3097 Min/Max not working for pack
    //    /// <summary>
    //    /// Sets Grade original Ad minimum before any adjustments
    //    /// </summary>
    //    /// <param name="aMinimum">Minimum value</param>
    //    internal void SetGradeOriginalAdMinimum(int aMinimum)
    //    {
    //        this._origGradeAdMinimum = aMinimum;
    //    }
    //    // END MID Track #3097 Min/Max not working for pack


    //    /// <summary>
    //    /// Sets grade color minimum
    //    /// </summary>
    //    /// <param name="aMinimum">A minimum value.</param>
    //    internal void SetGradeColorMinimum(int aMinimum)
    //    {
    //        _gradeColorMinMax.SetMinimum(aMinimum);
    //    }

    //    /// <summary>
    //    /// Sets grade color maximum
    //    /// </summary>
    //    /// <param name="aMaximum">A maximum value.</param>
    //    internal void SetGradeColorMaximum(int aMaximum)
    //    {
    //        _gradeColorMinMax.SetMaximum(aMaximum);
    //    }
    //    // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
    //    /// <summary>
    //    /// Sets grade atrribute set
    //    /// </summary>
    //    internal void SetGradeAttributeSet(int aGradeSglRID)
    //    {
    //        _gradeSglRID = aGradeSglRID;
    //    }
    //    // End TT#618 
    //    // Begin TT#617 - RMatelic - Allocation Override - Add Ship Up To Rule (#36, #39)
    //    /// <summary>
    //    /// Sets grade ShipUpTo
    //    /// </summary>
    //    internal void SetGradeShipUpTo(int aGradeShipUpTo)
    //    {
    //        _gradeShipUpTo = aGradeShipUpTo;
    //    }
    //    // End TT#617 
    //}    
	#endregion AllocationGradeBin 

	#region AllocationGradeBinCompareDescend
	/// <summary>
	/// Descend IComparer for the Allocation Grade Bin. 
	/// </summary>
	public class AllocationGradeBinCompareDescend:IComparer
	{
		/// <summary>
		/// Sorts AllocationGradeBin's in descending order by LowBoundary.
		/// </summary>
		/// <param name="aGradeBin">1st Grade Bin</param>
		/// <param name="bGradeBin">2nd Grade Bin</param>
		/// <returns>-1 when aGradeBin is greater than, 0 when equal to and 1 when aGradeBin is less than bGradeBin.</returns>
		public int Compare (object aGradeBin, object bGradeBin)
		{
			if (!(aGradeBin is AllocationGradeBin) || !(bGradeBin is AllocationGradeBin))
			{
				throw new MIDException(eErrorLevel.severe,
					(int)(eMIDTextCode.msg_al_CompareObjectsMustBeAllocationGradeBin),
					MIDText.GetText(eMIDTextCode.msg_al_CompareObjectsMustBeAllocationGradeBin));
			}
			if (aGradeBin == null)
			{
				return - 1;
			}
			if (bGradeBin == null)
			{
				return +1;
			}
			if (((AllocationGradeBin)aGradeBin).LowBoundary > ((AllocationGradeBin)bGradeBin).LowBoundary)
			{
				return -1;
			}
			if (((AllocationGradeBin)aGradeBin).LowBoundary < ((AllocationGradeBin)bGradeBin).LowBoundary)
			{
				return 1;
			}
            // begin TT#1100 - Group Allocation Method Out of Range Error
            // NOTE:  When Attributes were used with Grades, the sort was inconsistent.
            //       (ie. the boundaries were in order but the sets within boundary were haphazard); 
            //        with Assortment/Group it must be consistent and predictable
            if (((AllocationGradeBin)aGradeBin).GradeSglRID > ((AllocationGradeBin)bGradeBin).GradeSglRID)
            {
                return -1;
            }
            if (((AllocationGradeBin)aGradeBin).GradeSglRID < ((AllocationGradeBin)bGradeBin).GradeSglRID)
            {
                return 1;
            }
            // end TT#1100 - Group Allocation Method Out of Range Error
			return 0;
		}
	}
	#endregion AllocationGradeBinCompareDescend

	#region AllocationGradeBinCompareAscend
	/// <summary>
	/// Ascend IComparer for the Allocation Grade Bin. 
	/// </summary>
	public class AllocationGradeBinCompareAscend:IComparer
	{
		/// <summary>
		/// Sorts AllocationGradeBin's in ascending order by LowBoundary.
		/// </summary>
		/// <param name="aGradeBin">1st Grade Bin</param>
		/// <param name="bGradeBin">2nd Grade Bin</param>
		/// <returns>1 when aGradeBin is greater than, 0 when equal to and -1 when aGradeBin is less than bGradeBin.</returns>
		public int Compare (object aGradeBin, object bGradeBin)
		{
			if (!(aGradeBin is AllocationGradeBin) || !(bGradeBin is AllocationGradeBin))
			{
				throw new MIDException(eErrorLevel.severe,
					(int)(eMIDTextCode.msg_al_CompareObjectsMustBeAllocationGradeBin),
					MIDText.GetText(eMIDTextCode.msg_al_CompareObjectsMustBeAllocationGradeBin));;
			}
			if (aGradeBin == null)
			{
				return -1;
			}
			if (bGradeBin == null)
			{
				return +1;
			}
			if (((AllocationGradeBin)aGradeBin).LowBoundary > ((AllocationGradeBin)bGradeBin).LowBoundary)
			{
				return 1;
			}
			if (((AllocationGradeBin)aGradeBin).LowBoundary < ((AllocationGradeBin)bGradeBin).LowBoundary)
			{
				return -1;
			}
			return 0;
		}
	}
	#endregion AllocationGradeBinCompareAscend

	#region AllocationMerchBin
	/// <summary>
	/// Container to hold references to the merchandise hierarchy.
	/// </summary>
	/// <remarks>
	/// Contents of this container:
	/// <list type="bullet">
	/// <item>MdseHnRID: Identifies a merchandise instance on the hierarchy.</item>
	/// <item>ProductHnLvlRID: Identifies a level within the product hierarchy.</item>
	/// </list>
	/// </remarks>
	public struct AllocationMerchBin
	{
		//=======
		// FIELDS
		//=======
		private int _mdseHnRID;
		private int _productHnLvlRID;
		private int _productHnLvlSeq;

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets merchandie hierarchy node RID.
		/// </summary>
		public int MdseHnRID
		{
			get
			{
				return _mdseHnRID;
			}
		}

		/// <summary>
		/// Gets product hierarchy node level RID
		/// </summary>
		public int ProductHnLvlRID
		{
			get
			{
				return _productHnLvlRID;
			}
		}

		/// <summary>
		/// Gets product hierarchy node level sequence
		/// </summary>
		public int ProductHnLvlSeq
		{
			get
			{
				return _productHnLvlSeq;
			}
		}
		//========
		// METHODS
		//========
		/// <summary>
		/// Sets merchandise hierarchy node RID
		/// </summary>
		/// <param name="aMdseHnRID">A merchandise hierarchy node RID</param>
		internal void SetMdseHnRID(int aMdseHnRID)
		{
			_mdseHnRID = aMdseHnRID;
		}

		/// <summary>
		/// Sets product hierarchy node level RID.
		/// </summary>
		/// <param name="aProductHnLvlRID">A Product Hierarchy RID</param>
		internal void SetProductHnLvlRID(int aProductHnLvlRID)
		{
			_productHnLvlRID = aProductHnLvlRID;
		}

		/// <summary>
		/// Sets product hierarchy node level sequence.
		/// </summary>
		/// <param name="aProductHnLvlSeq">A product hierarchy sequence.</param>
		internal void SetProductHnLvlSeq(int aProductHnLvlSeq)
		{
			_productHnLvlSeq = aProductHnLvlSeq;
		}
	}
	#endregion AllocationMerchBin
 
	#region RuleBin
	/// <summary>
	/// Container to hold allocation rules.
	/// </summary>
	public struct RuleBin
	{
		//=======
		// FIELDS
		//=======
		private eRuleType _ruleType;
		private double    _ruleQty;

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets RuleType
		/// </summary>
		public eRuleType RuleType
		{
			get 
			{
				return _ruleType;
			}
		}

		/// <summary>
		/// Gets Rule quantity.
		/// </summary>
		public double RuleQty
		{
			get
			{
				return _ruleQty;
			}
		}

		//========
		// METHODS
		//========
		/// <summary>
		/// Sets Rule Type
		/// </summary>
		/// <param name="aRuleType">eRuleType</param>
		internal void SetRuleType(eRuleType aRuleType)
		{
			_ruleType = aRuleType;
		}

		/// <summary>
		/// Sets Rule Quantity
		/// </summary>
		/// <param name="aQty">A quantity.</param>
		internal void SetRuleQty(double aQty)
		{
			if (aQty < 0)
			{
				throw new MIDException(eErrorLevel.warning,
					(int)(eMIDTextCode.msg_RuleAllocationCannotBeNeg),
					MIDText.GetText(eMIDTextCode.msg_RuleAllocationCannotBeNeg));
			}
			_ruleQty = aQty;
		}
	}
	#endregion RuleBin

	// begin MID Track 4448 AnF Audit Enhancement
	#region AllocationAuditStruct
	public struct AllocationActionAuditStruct
	{
		//========//
		// FIELDS //
		//========//
		private eAllocationMethodType _allocationMethodType;
        private GeneralComponent _component;
		private int _qtyAllocatedBeforeAction;
		private int _storesWithAllocationBeforeAction;
		private bool _auditUnitsAllocated;
		private bool _auditStoreStyle;
    // begin TT#1199 - MD - NEED allocation # of stores is zero - rbeck
        private StoreVector _storeVector;
        private AllocationProfile _ap;
    // end TT#1199 - MD - NEED allocation # of stores is zero - rbeck
		//==============//
		// Constructors //
		//==============//
		/// <summary>
		/// Creates an instance of the Allocation Action Audit Structure
		/// </summary>
		/// <param name="aAllocationMethodType">Allocation Action Type</param>
        /// <param name="aAllocationProfile">Allocation Profile to which the action applies (required only when units are being audited)</param>
		/// <param name="aComponent">Component on which the action occurred</param>
		/// <param name="aQtyAllocatedBeforeAction">Quantity Allocated before action</param>
		/// <param name="aStoresWithAllocationBeforeAction">Number of stores allocated before action</param>
		/// <param name="aAuditUnitsAllocated">True: Units allocated is included in audit; False: units allocated not included in audit</param>
		/// <param name="aAuditStoreStyle">True: units allocated before action are style units; False:  units allocated before action are size units</param>
        /// <remarks>aAuditStoreStyle is valid only when aAuditUnitsAllocated is true</remarks>
    // begin TT#1199 - MD - NEED allocation # of stores is zero - rbeck
        //public AllocationActionAuditStruct (int aAllocationMethodType, GeneralComponent aComponent, int aQtyAllocatedBeforeAction, int aStoresWithAllocationBeforeAction, bool aAuditUnitsAllocated, bool aAuditStoreStyle)
        public AllocationActionAuditStruct(int aAllocationMethodType, AllocationProfile aAllocationProfile
                                         , GeneralComponent aComponent, int aQtyAllocatedBeforeAction
                                         , int aStoresWithAllocationBeforeAction, bool aAuditUnitsAllocated, bool aAuditStoreStyle)
    // end  TT#1199 - MD - NEED allocation # of stores is zero - rbeck
        {
			if (aAllocationMethodType == (int)eAllocationMethodType.BackoutAllocation
				&& aComponent.ComponentType == eComponentType.ColorAndSize)
			{
				_allocationMethodType = eAllocationMethodType.BackoutSizeAllocation;
			}
			else
			{																				  
				_allocationMethodType = (eAllocationMethodType)aAllocationMethodType;
			}
			_component = aComponent;
			_qtyAllocatedBeforeAction = aQtyAllocatedBeforeAction;
			_storesWithAllocationBeforeAction = aStoresWithAllocationBeforeAction;
			_auditUnitsAllocated = aAuditUnitsAllocated;
			_auditStoreStyle = aAuditStoreStyle;
        // begin  TT#1199 - MD - NEED allocation # of stores is zero - rbeck
            _ap = aAllocationProfile;                      
            if (_auditUnitsAllocated && _ap != null)
            {
                _storeVector = new StoreVector();
                foreach (Index_RID storeIdxRID in _ap.AppSessionTransaction.StoreIndexRIDArray())
                {
                    _storeVector.SetStoreValue(storeIdxRID.RID,
                                                _ap.GetStoreQtyAllocated(aComponent, storeIdxRID)
                                                );
                }
                if (_storeVector.AllStoreTotalValue != aQtyAllocatedBeforeAction)
                {

                    throw new MIDException( eErrorLevel.severe,
                                            (int)(eMIDTextCode.msg_StoreQuantitiesNotEqual),
                                            MIDText.GetTextOnly(eMIDTextCode.msg_StoreQuantitiesNotEqual) );
                }
            }
            else
            {
                _storeVector = null;
            }          
        // end  TT#1199 - MD - NEED allocation # of stores is zero - rbeck
		}

		//============//
		// Properties //
		//============//
		/// <summary>
		/// Gets Allocation Action Type
		/// </summary>
		public eAllocationActionType AllocationActionType
		{
			get
			{
				return (eAllocationActionType)_allocationMethodType;
			}
		}
		/// <summary>
		/// Gets Allocation Action Type
		/// </summary>
		public eAllocationMethodType AllocationMethodType
		{
			get
			{
				return _allocationMethodType;
			}
		}
		/// <summary>
		/// Gets Component on which action occurred
		/// </summary>
		public GeneralComponent Component
		{
			get
			{
				return _component;
			}
		}
		/// <summary>
		/// Gets Quantity Allocated Before Action
		/// </summary>
		public int QtyAllocatedBeforeAction
		{
			get
			{ 
				return _qtyAllocatedBeforeAction;
			}
		}
		/// <summary>
		/// Get Count of stores with an allocation before Action
		/// </summary>
		public int StoresWithAllocationBeforeAction
		{
			get
			{
				return _storesWithAllocationBeforeAction;
			}
		}
		public bool AuditUnits
		{
			get
			{
				return this._auditUnitsAllocated;
			}
		}public bool AuditStyle
		{
			get
			{
				return this._auditStoreStyle;
			}
		}
    // begin TT#1199 - MD - NEED allocation # of stores is zero - rbeck
        public int CountStoresWhoseAllocationChanged
        {
            get
            {
                if (_ap == null || _storeVector == null)
                {
                    return 0;
                }
                int count = 0;
                foreach (Index_RID storeIdxRID in _ap.AppSessionTransaction.StoreIndexRIDArray())
                {
                    if (GetStoreBeforeQtyAllocated(storeIdxRID) !=
                        _ap.GetStoreQtyAllocated(Component, storeIdxRID))
                    {
                        count++;
                    }
                }
                return count;
            }
        }
        public int GetStoreBeforeQtyAllocated(Index_RID aStoreIdxRID)
        {
            return GetStoreBeforeQtyAllocated(aStoreIdxRID.RID);
        }
        public int GetStoreBeforeQtyAllocated(int aStoreRID)
        {
            if (_storeVector == null)
            {
                return 0;
            }
            return (int)_storeVector.GetStoreValue(aStoreRID);
        }
    // end TT#1199 - MD - NEED allocation # of stores is zero - rbeck
	}
	#endregion AllocationAuditStruct
	// end MID Track 4448 AnF Audit Enhancment

    // begin TT#59 Implement Temp Locks
    #region AllocationWaferCellChange
    /// <summary>
    /// Structure describing an Allocation Wafer Cell change.
    /// </summary>
    [Serializable]
    public struct AllocationWaferCellChange
    {
        private int _waferRow;
        private int _waferCol;
        private int _cellRowWithinWafer;
        private int _cellColWithinWafer;
        private double _cellValue;
        private int _changeSequence;

        /// <summary>
        /// Allocation Wafer Cell Change Request Structure
        /// </summary>
        /// <param name="aWaferRow">Wafer Row identifying the row where the wafer resides</param>
        /// <param name="aWaferCol">Wafer Column idetifying the column where the wafer resides</param>
        /// <param name="aCellRowInWafer">Identifies the row within the wafer where the cell that changed resides </param>
        /// <param name="aCellColInWafer">Identifies the column within the wafter where the cell that changed resides</param>
        /// <param name="aCellValue">The new cell value</param>
        /// <param name="aChangeSequence">Identifies the sequence in which this change occurred.</param>
        public AllocationWaferCellChange(int aWaferRow, int aWaferCol, int aCellRowInWafer, int aCellColInWafer, double aCellValue, int aChangeSequence)
        {
            _waferRow = aWaferRow;
            _waferCol = aWaferCol;
            _cellRowWithinWafer = aCellRowInWafer;
            _cellColWithinWafer = aCellColInWafer;
            _cellValue = aCellValue;
            _changeSequence = aChangeSequence;
        }

        public int WaferRow
        {
            get { return _waferRow; }
        }
        public int WaferCol
        {
            get { return _waferCol; }
        }
        public int CellRowInWafer
        {
            get { return _cellRowWithinWafer; }
        }
        public int CellColInWafer
        {
            get { return _cellColWithinWafer; }
        }
        public double WaferCellValue
        {
            get { return _cellValue; }
        }
        public int ChangeRequestSequence
        {
            get { return _changeSequence; }
        }
    }
    /// <summary>
    /// Compares AllocationWaferCellChanges to identify the correct processing order
    /// </summary>
    [Serializable]
    public class AllocationWaferCellChangeProcessOrder : IComparer<AllocationWaferCellChange>
	{
		/// <summary>
        /// Compares AllocationWaferCellChanges x and y to identify the correct processing order  
		/// </summary>
        /// <param name="x">First of two AllocationWaferCellChanges</param>
        /// <param name="y">Second of two AllocationWaferCellChanges</param>
		/// <returns>-1 if x is processed first; +1 if y is processed first; -1 otherwise (meaning x will process first).</returns>
        public int Compare(AllocationWaferCellChange x, AllocationWaferCellChange y)
		{
            if (x.WaferRow < y.WaferRow)
			{
				return -1;
			}
            if (x.WaferRow > y.WaferRow)
            {
				return +1;
			}
            if (x.WaferCol < y.WaferCol)
            {
				return +1;
			}
            if (x.WaferCol > y.WaferCol)
            {
                return -1;
            }
            // Style Review: process Bulk after color (detail is to right of matrix)
            if (x.CellColInWafer < y.CellColInWafer)
            {
                return +1;
            }
            if (x.CellColInWafer > y.CellColInWafer)
            {
                return -1;
            }
            // Size Review:  Process Color totals, size totals after detail (detail is at bottom of matrix)
            if (x.CellRowInWafer < y.CellRowInWafer)
            {
                return +1;
            }
            if (x.CellRowInWafer > y.CellRowInWafer)
            {
                return -1;
            }

            if (x.ChangeRequestSequence < y.ChangeRequestSequence)
            {
                return -1;
            }
            if (x.ChangeRequestSequence > y.ChangeRequestSequence)
            {
                return +1;
            }
            return -1;
        }
	}
    [Serializable]
    public class AllocationWaferCellChangeList:ICloneable
    {
        private SortedList<AllocationWaferCellChange, AllocationWaferCellChange> _waferCellChangeList;

        public AllocationWaferCellChangeList()
        {
            _waferCellChangeList =
                new SortedList<AllocationWaferCellChange, AllocationWaferCellChange>(new AllocationWaferCellChangeProcessOrder());
        }
        public void AddAllocationWaferCellChange(int aWaferRow, int aWaferCol, int aCellRowInWafer, int aCellColInWafer, double aCellValue)
        {
            AllocationWaferCellChange awcc = new AllocationWaferCellChange(aWaferRow, aWaferCol, aCellRowInWafer, aCellColInWafer, aCellValue, _waferCellChangeList.Count);
            _waferCellChangeList.Add(awcc, awcc);
        }
        public int Count
        {
            get { return _waferCellChangeList.Count; }
        }
        public IEnumerator GetEnumerator()
        {
            return _waferCellChangeList.Values.GetEnumerator();
        }
        /// <summary>
        /// Returns a clone of this object.
        /// </summary>
        /// <returns>
        /// A Clone of the object.
        /// </returns>

        public object Clone()
        {
            AllocationWaferCellChangeList newList = new AllocationWaferCellChangeList();
            foreach (AllocationWaferCellChange awcc in this)
            {
                newList.AddAllocationWaferCellChange(awcc.WaferRow, awcc.WaferCol, awcc.CellRowInWafer, awcc.CellColInWafer, awcc.WaferCellValue);
            }
            return newList;
        }
    }
    #endregion AllocationWaferCellChange

    #region HeaderInformationStruct
    [Serializable]
    public struct HeaderInformationStruct
    {
        private int _headerRID;
        private string _headerID;
        private string _styleID;
        private string _otsForecastNodeID;
        private double _factorPct;
        private string _onhandNodeID;
        private int _gradeWeekCount;
        private double _pctNeedLimit;
        private DateTime _beginDay;
        private string _capacityNodeID;
        private AllocationGradeBin[] _allocationGrades;
        private HeaderColorInformationStruct[] _headerColorInformationList;


        public HeaderInformationStruct(
            int aHeaderRID,
            string aHeaderID,
            string aStyleID,
            string aOtsForecastNodeID,
            double aFactorPct,
            string aOnHandNodeID,
            int aGradeWeekCount,
            double aPctNeedLimit,
            DateTime aBeginDay,
            string aCapacityNodeID,
            ArrayList aAllocationGradeBinList,
            HeaderColorInformationStruct[] aHeaderColorInformationList)
        {
            _headerRID = aHeaderRID;
            _headerID = aHeaderID;
            _styleID = aStyleID;
            _otsForecastNodeID = aOtsForecastNodeID;
            _factorPct = aFactorPct;
            _onhandNodeID = aOnHandNodeID;
            _gradeWeekCount = aGradeWeekCount;
            _pctNeedLimit = aPctNeedLimit;
            _beginDay = aBeginDay;
            _capacityNodeID = aCapacityNodeID;
            _allocationGrades = new AllocationGradeBin[aAllocationGradeBinList.Count];
            int i = 0;
            foreach (AllocationGradeBin agb in aAllocationGradeBinList)
            {
                _allocationGrades[i] = agb;
                i++;
            }
            _headerColorInformationList = aHeaderColorInformationList;


        }
        public int HeaderRID
        {
            get { return _headerRID; }
        }
        public string HeaderID
        {
            get { return _headerID; }
        }
        public string StyleID
        {
            get { return _styleID; }
        }
        public string OTSForcastNodeID
        {
            get { return _otsForecastNodeID; }
        }
        public double FactorPct
        {
            get { return _factorPct; }
        }
        public string OnhandNodeID
        {
            get { return _onhandNodeID; }
        }
        public int GradeWeekCount
        {
            get { return _gradeWeekCount; }
        }
        public double PercentNeedLimit
        {
            get { return _pctNeedLimit; }
        }
        public DateTime BeginDay
        {
            get { return _beginDay; }
        }
        public string CapacityNodeID
        {
            get { return _capacityNodeID; }
        }
        public AllocationGradeBin[] AllocationGrades
        {
            get { return _allocationGrades; }
        }
        public HeaderColorInformationStruct[] HeaderColorInformationList
        {
            get { return _headerColorInformationList; }
        }
    }
    public struct HeaderColorInformationStruct
    {
        private string _colorCodeID;
        private string _colorCodeName;
        private int _colorCodeRID;
        private string _colorHnID;
        private int _colorHnRID;
        private int _colorMinimum;
        private int _colorMaximum;
        public HeaderColorInformationStruct(
            string aColorCodeID,
            string aColorCodeName,
            int aColorCodeRID,
            string aColorHnID,
            int aColorHnRID,
            int aColorMinimum,
            int aColorMaximum)
        {
            _colorCodeID = aColorCodeID;
            _colorCodeName = aColorCodeName;
            _colorCodeRID = aColorCodeRID;
            _colorHnID = aColorHnID;
            _colorHnRID = aColorHnRID;
            _colorMinimum = aColorMinimum;
            _colorMaximum = aColorMaximum;
        }
        public string ColorCodeID
        {
            get { return _colorCodeID; }
        }
        public string ColorCodeName
        {
            get { return _colorCodeName; }
        }
        public int ColorCodeRID
        {
            get { return _colorCodeRID; }
        }
        public string ColorHierarchyNodeID
        {
            get { return _colorHnID; }
        }
        public int ColorHierarchyNodeRID
        {
            get { return _colorHnRID; }
        }
        public int ColorMinimum
        {
            get { return _colorMinimum; }
        }
        public int ColorMaximum
        {
            get { return _colorMaximum; }
        }
    }

    #endregion HeaderInformationStruct
    // end TT#59 Implement Temp Locks

    // begin TT#667 allow pack or bulk reserve
    /// <summary>
    /// Describes the Reserve Allocation Specification
    /// </summary>
    public struct AllocateReserveSpecification
    {
        private bool _isReserveSpecPercent;
        private double _reserveTotal;
        private double _reservePack;
        private double _reserveBulk;
        /// <summary>
        /// Creates an instance of the AllocateReserveSpecification
        /// </summary>
        /// <param name="aIsReserveSpecPercent">True:  Reserve quantities are percents</param>
        /// <param name="aReserveTotal">Total Reserve.  When percent, total reserve is percent of total receipts. Total Reserve is units when not percent of total receipts.</param>
        /// <param name="aReservePack">Pack reserve amount.  When percent, pack reserve is the portion of the total reserve units that are in packs; When units, pack reserve must be less than Total Reserve. (pack + bulk reserve must equal total reserve if units or 100% if percent when pack or bulk reserve is greater than 0)</param>
        /// <param name="aReserveBulk">Bulk reserve amount.  When percent, bulk reserve is the portion of the total reserve units that are in bulk; When units, bulk reserve must be less than Total Reserve (pack + bulk reserve must equal total reserve if units or 100% if percent when pack or bulk reserve is greater than 0.)</param>
        public AllocateReserveSpecification(bool aIsReserveSpecPercent, double aReserveTotal, double aReservePack, double aReserveBulk)
        {
            if (aReserveTotal < 0)
            {
                throw new MIDException(
                    eErrorLevel.severe,
                    (int)eMIDTextCode.msg_MustBeNonNegative,
                    MIDText.GetTextOnly(eMIDTextCode.msg_MustBeNonNegative)
                    + " " + MIDText.GetTextOnly(eMIDTextCode.lbl_Reserve));
            }
            if (aReservePack < 0)
            {
                throw new MIDException(
                    eErrorLevel.severe,
                    (int)eMIDTextCode.msg_MustBeNonNegative,
                    MIDText.GetTextOnly(eMIDTextCode.msg_MustBeNonNegative)
                    + " " + MIDText.GetTextOnly(eMIDTextCode.lbl_ReserveAsPacks));
            }
            if (aReserveBulk < 0)
            {
                throw new MIDException(
                    eErrorLevel.severe,
                    (int)eMIDTextCode.msg_MustBeNonNegative,
                    MIDText.GetTextOnly(eMIDTextCode.msg_MustBeNonNegative)
                    + " " + MIDText.GetTextOnly(eMIDTextCode.lbl_ReserveAsBulk));
            } 
            if (aReservePack > 0)
            {
                if (aReserveBulk > 0)
                {
                    if (aIsReserveSpecPercent)
                    {
                        if (aReservePack + aReserveBulk != 100.00)
                        {
                            throw new MIDException(
                                eErrorLevel.severe,
                                (int)eMIDTextCode.msg_MustEqual100,
                                MIDText.GetTextOnly(eMIDTextCode.msg_MustEqual100) 
                                + ": " + MIDText.GetTextOnly(eMIDTextCode.lbl_ReserveAsPacks) 
                                + ", " + MIDText.GetTextOnly(eMIDTextCode.lbl_ReserveAsBulk));
                        }
                    }
                    else
                    {
                        if (aReserveTotal != aReservePack + aReserveBulk)
                        {
                            throw new MIDException(
                                eErrorLevel.severe, 
                                (int)eMIDTextCode.msg_al_AsBulkAsPackNotEqualReserve, 
                                MIDText.GetText(eMIDTextCode.msg_al_AsBulkAsPackNotEqualReserve));
                        }
                    }
                }
            }
            _isReserveSpecPercent = aIsReserveSpecPercent;
            _reserveTotal = aReserveTotal;
            _reservePack = aReservePack;
            _reserveBulk = aReserveBulk;
        }
        /// <summary>
        /// True:  reserve quantities are percentages False: reserve quantities are units
        /// </summary>
        public bool IsReserveSpecPercent
        {
            get { return _isReserveSpecPercent; }
        }
        /// <summary>
        /// Total amount to put in reserve.  When this is a percentage, it is interpreted as a percentage of total receipts.
        /// </summary>
        public double ReserveTotal
        {
            get { return _reserveTotal; }
        }
        /// <summary>
        /// Amount of the reserve quantity that is Bulk. When this is a percentage, it is interpreted as a percentage of the total reserve units. 
        /// </summary>
        public double ReserveBulk
        {
            get { return _reserveBulk; }
        }
        /// <summary>
        ///  Amounto of the reserve quantity that is Packs.  When this is a percentage, it is interpreted as a percentage of the total reserve units.  NOTE: the actual number of packs put into reserve is subject to round off error due to pack multiples.
        /// </summary>
        public double ReservePack
        {
            get { return _reservePack; }
        }
    }

    // end TT#667 allow pack or bulk reserve

    // begin TT#41 - MD - JEllis - Size Inventory Min Max pt 1
    public struct SizeNeedMethodKey
    {
        private int _hashCode;
        private eMerchandiseType _merchType;
        private int _hnRID;
        private int _phRID;
        private int _phSequence;
        private int _constraintRID;
        private int _alternateRID;
        private int _sizeGroupRID;
        private int _sizeCurveGroupRID;
        private bool _normalizeSizeCurves;
        private eMerchandiseType _ibMerchType;
        private int _ibHnRID;
        private int _ibPhRID;
        private int _ibPhSequence;
        private eVSWSizeConstraints _vswSizeConstraints;  // TT#246 - MD - JEllis - VSW Size In Store Minimums pt 3
        private eFillSizesToType _FillSizesToType; //TT#848-MD -jsobek -Fill to Size Plan Presentation
                /// <summary>
        /// Constructor used to create an instance of the HeaderSizeNeedMethodKey
        /// </summary>
        /// <param name="aMerchType">eMerchandiseType that identifies the Merchandise Basis Hierarchy Node type (undefined identifies a Basis HnRID)</param>
        /// <param name="aMerchBasisHnRID">RID that identifies the Merchandise Basis Hierarchy when aMerchType is "undefined" (must be Included.NoRID when aMerchType is NOT "undefined").</param>
        /// <param name="aMerchPhRID">RID that identifies the Product Hierarchy when aMerchType is NOT "undefined" (use Include.NoRID when Basis Hierarchy is "undefined")</param>
        /// <param name="aMerchPhlSeq">Product Hierarchy Sequence when aMerchType is "defined" (use '0' when Basis Hierarchy is "undefined")</param>
        /// <param name="aConstraintRID">RID that identifies the constraint model (use Include.NoRID when not specified)</param>
        /// <param name="aAlternateRID">RID that identifies the alternate Size model (use Include.NoRID when not specified)</param>
        /// <param name="aSizeGroupRID">RID that identifies the size group</param>
        /// <param name="aSizeCurveGroupRID">RID that identifies the size curve group</param>
        /// <param name="aNormalizeSizeCurve">True: Normalize Size Curves; False: do not normalize size curves</param>
        /// <param name="aIbMerchType">eMerchandiseType that identifies the Inventory Merchandise Basis Hierarchy Node type (undefined identifies an Inventory Merchandise Basis HnRID)</param>
        /// <param name="aIbMerchBasisHnRID">RID identifies Inventory Merchandise Basis Hierarchy Node (must be Included.NoRID when aIbMerchType is NOT "undefined"; May be Include.NoRID when aIbMerchType is "undefined") </param>
        /// <param name="aIbPhRID">RID identifies Inventory Product Hierarchy when aIbMerchType is NOT "undefined"; MUST be Include.NoRID when aIbMerchType is "Undefined"</param>
        /// <param name="aIbPhlSequence">Sequence identifies Product Hierarchy Level when aIbMerchType is NOT "undefined"; MUST be 0 when aIbMerhcType is "Undefined"</param>
        /// <param name="aVswSizeConstraints">VSW Size Constraint option</param>
        public SizeNeedMethodKey(
            eMerchandiseType aMerchType,
            int aMerchBasisHnRID,
            int aMerchPhRID,
            int aMerchPhlSeq,
            int aConstraintRID,
            int aAlternateRID,
            int aSizeGroupRID,
            int aSizeCurveGroupRID,
            bool aNormalizeSizeCurve,
            eMerchandiseType aIbMerchType,
            int aIbMerchBasisHnRID,
            int aIbPhRID,
            int aIbPhlSequence, // TT#246 - MD - JEllis - VSW Size In Store Minimums pt 3
            eVSWSizeConstraints aVswSizeConstraints, // TT#246 - MD - JEllis - VSW Size IN Store Minimums pt 3
            eFillSizesToType aFillSizesToType //TT#848-MD -jsobek -Fill to Size Plan Presentation
            )
        {
            _merchType = aMerchType;
            _hnRID = aMerchBasisHnRID;
            _phRID = aMerchPhRID;
            _phSequence = aMerchPhlSeq;
            _constraintRID = aConstraintRID;
            _alternateRID = aAlternateRID;
            _sizeGroupRID = aSizeGroupRID;
            _sizeCurveGroupRID = aSizeCurveGroupRID;
            _normalizeSizeCurves = aNormalizeSizeCurve;
            _ibMerchType = aIbMerchType;
            _ibHnRID = aIbMerchBasisHnRID;
            _ibPhRID = aIbPhRID;
            _ibPhSequence = aIbPhlSequence;
            _vswSizeConstraints = aVswSizeConstraints; // TT#246 - MD - JEllis - VSW Size In Store Minimums pt 3
            _FillSizesToType = aFillSizesToType; //TT#848-MD -jsobek -Fill to Size Plan Presentation
            Object[] hashObject = { _merchType, _hnRID, _phRID, _phSequence, _constraintRID, _alternateRID, _sizeGroupRID, _sizeCurveGroupRID, _normalizeSizeCurves, _ibMerchType, _ibHnRID, _ibPhRID, _ibPhSequence, _vswSizeConstraints, _FillSizesToType };   // TT#246 - MD - JEllis - VSW Size In Store Minimums pt 3    //TT#848-MD -jsobek -Fill to Size Plan Presentation      
            _hashCode = MID_HashCode.CalculateHashCode(hashObject);
        }

        /// <summary>
        /// Gets the Merchandise Basis Type for the associated Size Method
        /// </summary>
        public eMerchandiseType MerchandiseType
        {
            get { return _merchType; }
        }
        /// <summary>
        /// Gets the Merchandise Basis Hierarchy Node RID for the associated Size Method
        /// </summary>
        public int MerchandiseBasisHnRID
        {
            get { return _hnRID; }
        }
        /// <summary>
        /// Gets the Merchandise Basis Product Hierarchy RID for the associated Size Method
        /// </summary>
        public int MerchandiseBasisPhRID
        {
            get { return _phRID; }
        }
        /// <summary>
        /// Gets the Merchandise Basis Product Hierarchy Level Sequence for the associated Size Method
        /// </summary>
        public int MerchandiseBasisPhlSequence
        {
            get { return _phSequence; }
        }
        /// <summary>
        /// Gets the Constraint Model RID for the associated Size Method
        /// </summary>
        public int ConstraintRID
        {
            get { return _constraintRID; }
        }
        /// <summary>
        /// Gets the Alternate Size Model RID for the associated Size Method
        /// </summary>
        public int AlternateRID
        {
            get { return _alternateRID; }
        }
        /// <summary>
        /// Gets the Size Group RID for the associated Size Method
        /// </summary>
        public int SizeGroupRID
        {
            get { return _sizeGroupRID; }
        }
        /// <summary>
        /// Gets the Size Curve Group RID for the associated Size Method
        /// </summary>
        public int SizeCurveGroupRID
        {
            get { return _sizeCurveGroupRID; }
        }
        /// <summary>
        /// Gets the Normalize Size Curves bool value for the associated Size Method
        /// </summary>
        public bool NormalizeSizeCurves
        {
            get { return _normalizeSizeCurves; }
        }
        /// <summary>
        /// Gets the Inventory Basis Merchandise Type for the associated Size Method
        /// </summary>
        public eMerchandiseType InventoryBasisMerchandiseType
        {
            get { return _ibMerchType; }
        }
        /// <summary>
        /// Gets the Inventory Basis Merchandise Hierarchy Node RID for the associated Size Method
        /// </summary>
        public int InventoryBasisMerchandiseHnRID
        {
            get { return _ibHnRID; }
        }
        /// <summary>
        /// Gets the Inventory Basis Merchandise Product Hierarchy RID for the associated Size Method
        /// </summary>
        public int InventoryBasisMerchandisePhRID
        {
            get { return _ibPhRID; }
        }
        /// <summary>
        /// Gets the Inventory Basis Merchandise Product Hierarchy Level Sequence for the associated Size Method
        /// </summary>
        public int InventoryBasisMerchandisePhlSeq
        {
            get { return _ibPhSequence; }
        }
        // begin TT#246 - MD - JEllis - VSW Size In Store minimums pt 3
        public eVSWSizeConstraints VSWSizeConstraints
        {
            get { return _vswSizeConstraints; }
        }
        // end TT#246 - MD - JEllis - VSW Size In Store minimums pt 3
        //Begin TT#848-MD -jsobek -Fill to Size Plan Presentation
        public eFillSizesToType FillSizesToType
        {
            get { return _FillSizesToType; }
        }
        //End TT#848-MD -jsobek -Fill to Size Plan Presentation
        /// <summary>
        /// True: "object" is equal to this structure; False: "object" is not equal to this structure
        /// </summary>
        /// <param name="obj">Object to compare to this object</param>
        /// <returns></returns>
        public override bool  Equals(object obj)
        {
            if (obj.GetHashCode() != GetHashCode())
            {
                return false;
            }
            if (obj.GetType() != typeof(HeaderSizeNeedMethodKey))
            {
                return false;
            }
            SizeNeedMethodKey snmk =  (SizeNeedMethodKey)obj;
            if (_merchType != snmk._merchType
                || _phRID != snmk._phRID
                || _phSequence != snmk._phSequence
                || _constraintRID != snmk._constraintRID
                || _alternateRID != snmk._alternateRID
                || _sizeGroupRID != snmk._sizeGroupRID
                || _sizeCurveGroupRID != snmk._sizeCurveGroupRID
                || _normalizeSizeCurves != snmk._normalizeSizeCurves
                || _ibMerchType != snmk._ibMerchType
                || _ibHnRID != snmk._ibHnRID
                || _ibPhRID != snmk._ibPhRID
                || _ibPhSequence != snmk._ibPhSequence // TT#246 - MD - Jellis - VSW Size In STore Minimums pt3
                || _vswSizeConstraints != snmk._vswSizeConstraints // TT#246 - MD - Jellis - VSW Size In Store minimums pt 3
                || _FillSizesToType != snmk._FillSizesToType //TT#848-MD -jsobek -Fill to Size Plan Presentation
                )  
            {
                return false;
            }
            return true;
        }
        public static bool operator ==(SizeNeedMethodKey rSnmk, SizeNeedMethodKey lSnmk) 
        { 
            // Null check 
            if (Object.ReferenceEquals(rSnmk, null)) 
            { 
                if (Object.ReferenceEquals(lSnmk, null)) 
                { 
                    // Both are null.  They do equal each other 
                    return true; 
                } 
 
                // Only 1 is null the other is not so they do not equal 
                return false; 
            }
            if (Object.ReferenceEquals(lSnmk, null))
            {
                return false;
            }
            return rSnmk.Equals(lSnmk); 
        }

        public static bool operator !=(SizeNeedMethodKey rSnmk, SizeNeedMethodKey lSnmk)
        {
            // Null check 

            return !(rSnmk == lSnmk);
        }
        /// <summary>
        /// Gets the hash code for this structure instance
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _hashCode;
        }
    }
    struct HeaderSizeNeedMethodKey
    {
        private int _hashCode;
        private int _hdrRID;
        private SizeNeedMethodKey _sizeNeedMethodKey;
        public HeaderSizeNeedMethodKey(int aHdrRID, SizeNeedMethodKey aSizeNeedMethodKey)
        {
            _hdrRID = aHdrRID;
            _sizeNeedMethodKey = aSizeNeedMethodKey;
            object[] hashParm = {_hdrRID, _sizeNeedMethodKey};
            _hashCode = MID_HashCode.CalculateHashCode(hashParm);
        }
        /// <summary>
        /// Gets the Header RID associated with this structure
        /// </summary>
        public int HeaderRID
        {
            get { return _hdrRID; }
        }
        public SizeNeedMethodKey SizeNeedMethodKey
        {
            get { return _sizeNeedMethodKey; }
        }
        /// <summary>
        /// True: "object" is equal to this structure; False: "object" is not equal to this structure
        /// </summary>
        /// <param name="obj">Object to compare to this object</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj.GetHashCode() != GetHashCode())
            {
                return false;
            }
            if (obj.GetType() != typeof(HeaderSizeNeedMethodKey))
            {
                return false;
            }
            HeaderSizeNeedMethodKey hsnmk = (HeaderSizeNeedMethodKey)obj;
            if (_hdrRID != hsnmk._hdrRID
                || _sizeNeedMethodKey != hsnmk._sizeNeedMethodKey)
            {
                return false;
            }
            return true;
        }
        public static bool operator ==(HeaderSizeNeedMethodKey rSnmk, HeaderSizeNeedMethodKey lSnmk)
        {
            // Null check 
            if (Object.ReferenceEquals(rSnmk, null))
            {
                if (Object.ReferenceEquals(lSnmk, null))
                {
                    // Both are null.  They do equal each other 
                    return true;
                }

                // Only 1 is null the other is not so they do not equal 
                return false;
            }
            if (Object.ReferenceEquals(lSnmk, null))
            {
                return false;
            }
            return rSnmk.Equals(lSnmk);
        }

        public static bool operator !=(HeaderSizeNeedMethodKey rSnmk, HeaderSizeNeedMethodKey lSnmk)
        {
            // Null check 

            return !(rSnmk == lSnmk);
        }
        /// <summary>
        /// Gets the hash code for this structure instance
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _hashCode;
        }
    }
    // end TT#41 - MD - JEllis - Size Inventory Min Max pt 1
    // begin TT#3060 - Loehman - Jellis - System Argument Error
    struct StoreColorCurveOnHandKey
    {
        int _hashCode;
        DateTime _onhandDate;
        int _storeRID;
        int _colorRID;
        int _sizeHnCurveKey;
        int _sizeCurveGroupRID;
        int _sizeConstraintRID;
        int _sizeAlternateRID;
        public StoreColorCurveOnHandKey
            (DateTime aOnhandDate,
            int aStoreRID,
            int aColorRID,
            int aSizeHnCurveKey,
            int aSizeCurveGroupRID,
            int aSizeConstraintRID,
            int aSizeAlternateRID)
        {
            _onhandDate = aOnhandDate;
            _storeRID = aStoreRID;
            _colorRID = aColorRID;
            _sizeHnCurveKey = aSizeHnCurveKey;
            _sizeCurveGroupRID = aSizeCurveGroupRID;
            _sizeConstraintRID = aSizeConstraintRID;
            _sizeAlternateRID = aSizeAlternateRID;
            Object[] hashObject = { _onhandDate, _storeRID, _colorRID, _sizeHnCurveKey, _sizeCurveGroupRID, _sizeConstraintRID, _sizeAlternateRID, _sizeCurveGroupRID};
            _hashCode = MID_HashCode.CalculateHashCode(hashObject);
        }
        public DateTime OnhandDate
        {
            get { return _onhandDate; }
        }
        public int StoreRID
        {
            get { return _storeRID; }
        }
        public int ColorRID
        {
            get { return _colorRID; }
        }
        public int SizeHnCurveKey
        {
            get { return _sizeHnCurveKey; }
        }
        public int SizeCurveGroupRID
        {
            get { return _sizeCurveGroupRID; }
        }
        public int SizeConstraintRID
        {
            get { return _sizeConstraintRID; }
        }
        public int SizeAlternateRID
        {
            get { return _sizeAlternateRID; }
        }

        /// <summary>
        /// True: "object" is equal to this structure; False: "object" is not equal to this structure
        /// </summary>
        /// <param name="obj">Object to compare to this object</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj.GetHashCode() != GetHashCode())
            {
                return false;
            }
            if (obj.GetType() != typeof(StoreColorCurveOnHandKey))
            {
                return false;
            }
            StoreColorCurveOnHandKey sccok = (StoreColorCurveOnHandKey)obj;
            if (_onhandDate != sccok._onhandDate
                || _storeRID != sccok._storeRID // TT#3078 - MD - Jellis - Size Allocation Wrong when begin date specified
                || _colorRID != sccok._colorRID
                || _sizeHnCurveKey != sccok._sizeHnCurveKey
                || _sizeCurveGroupRID != sccok._sizeCurveGroupRID
                || _sizeConstraintRID != sccok._sizeConstraintRID
                || _sizeAlternateRID != sccok._sizeAlternateRID)
            {
                return false;
            }
            return true;
        }
        public static bool operator ==(StoreColorCurveOnHandKey rSccok, StoreColorCurveOnHandKey lSccok)
        {
            // Null check 
            if (Object.ReferenceEquals(rSccok, null))
            {
                if (Object.ReferenceEquals(lSccok, null))
                {
                    // Both are null.  They do equal each other 
                    return true;
                }

                // Only 1 is null the other is not so they do not equal 
                return false;
            }
            if (Object.ReferenceEquals(lSccok, null))
            {
                return false;
            }
            return rSccok.Equals(lSccok);
        }

        public static bool operator !=(StoreColorCurveOnHandKey rSccok, StoreColorCurveOnHandKey lSccok)
        {
            // Null check 

            return !(rSccok == lSccok);
        }
        public override int GetHashCode()
        {
            return _hashCode;
        }

    }

    // end TT#3060 - Loehman - Jellis - System Argument Error

    // begin TT#488 - MD - Jellis - Group Allocation
    [Serializable]
    public struct AllocationCriteria
    {
        private int _beginCdrRid;
        private int _shipToCdrRid;

        private bool _exceedMaximums;

        private int _capacityStoreGroupRID;
        private Dictionary<int, AllocationCapacityBin> _capacity; 
        
        private int _gradeStoreGroupRID; 
        private int _gradeWeekCount;
        private Dictionary<int, Dictionary<double, GroupAllocationGradeBin>> _grade;

        private bool _useStoreGradeDefault;
        private bool _lineItemMinOverrideInd;
        private int _lineItemMinOverride;

        private bool _useAllColorsMinDefault;
        private bool _useAllColorsMaxDefault; 
        private int _allColorMultiple;
        private int _allSizeMultiple;
        private MinMaxAllocationBin _allColor;
        private Dictionary<int, ColorOrSizeMinMaxBin> _colorMinMax;
        private Dictionary<int, ColorSizeMinMaxBin> _colorSizeMinMax;

        private bool _exceedCapacity;

        private bool _reserveQtyIsPercent;
        private double _reserveQty;
        private double _reserveAsBulk;
        private double _reserveAsPacks;

        private bool _usePctNeedDefault;
        private double _percentNeedLimit;

        private bool _merchUnspecified;
        private AllocationMerchBin _OTSPlan;

        private bool _onHandUnspecified;
        private AllocationMerchBin _OTSOnHand;

        private bool _useFactorPctDefault;
        private double _OTSPlanFactorPercent;

        private char _inventoryInd;
        private bool _inventoryMinMax;
        private eMerchandiseType _inventoryMerchandiseType;
        private int _inventory_MERCH_HN_RID;
        private int _inventory_MERCH_PH_RID;
        private int _inventory_MERCH_PHL_SEQ;

        // BEGIN TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis
        private char _HdrinventoryInd;
        private bool _HdrinventoryMinMax;
        private eMerchandiseType _HdrinventoryMerchandiseType;
        private int _Hdrinventory_MERCH_HN_RID;
        private int _Hdrinventory_MERCH_PH_RID;
        private int _Hdrinventory_MERCH_PHL_SEQ;
        // END TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis

        private IMOProfileList _ipl;
        private List<HeaderPackRoundingOverride> _packRoundingOverrideList;
        public AllocationCriteria(
            SessionAddressBlock aSAB
                )
        {
            _beginCdrRid = Include.NoRID;
            _shipToCdrRid = Include.NoRID;

            GlobalOptionsProfile global = aSAB.ApplicationServerSession.GlobalOptions;
            _allColorMultiple = 1;
            _allSizeMultiple = 1;
            _useAllColorsMinDefault = true;
            _useAllColorsMaxDefault = true;
            _allColor = new MinMaxAllocationBin();
            _allColor.SetMaximum(_allColor.LargestMaximum); // TT#946 - MD - Jellis - Group Allocation Not Working
            _colorMinMax = new Dictionary<int, ColorOrSizeMinMaxBin>();
            _colorSizeMinMax = new Dictionary<int, ColorSizeMinMaxBin>();
            _capacity = new Dictionary<int,AllocationCapacityBin>();

            _exceedMaximums = false;
            _exceedCapacity = false;

            _useStoreGradeDefault = true;

            _gradeWeekCount = global.StoreGradePeriod;
            _capacityStoreGroupRID = global.AllocationStoreGroupRID;
            _gradeStoreGroupRID = global.AllocationStoreGroupRID;
            _grade = new Dictionary<int,Dictionary<double,GroupAllocationGradeBin>>();

            _lineItemMinOverrideInd = false;
            _lineItemMinOverride = 0;

            _reserveQtyIsPercent = false;
            _reserveQty = 0;
            _reserveAsBulk = 0;
            _reserveAsPacks = 0;

            _usePctNeedDefault = true;
            _percentNeedLimit = global.PercentNeedLimit;

            _merchUnspecified = true;
            _OTSPlan = new AllocationMerchBin();
            _OTSPlan.SetMdseHnRID(Include.DefaultPlanHnRID);
			_OTSPlan.SetProductHnLvlRID(Include.DefaultPlanHnRID);
			_OTSPlan.SetProductHnLvlSeq(0);

            _onHandUnspecified = true;
            _OTSOnHand = new AllocationMerchBin();
            _OTSOnHand.SetMdseHnRID(Include.DefaultPlanHnRID);
			_OTSOnHand.SetProductHnLvlRID(Include.DefaultPlanHnRID);
			_OTSOnHand.SetProductHnLvlSeq(0);

            _useFactorPctDefault = true;
			_OTSPlanFactorPercent = Include.DefaultPlanFactorPercent;;

            _inventoryMinMax = false;
            _inventoryInd = 'A';
            _inventory_MERCH_PHL_SEQ = 0;
            _inventory_MERCH_PH_RID = Include.NoRID;
            _inventory_MERCH_HN_RID = Include.NoRID;
            _inventoryMerchandiseType = eMerchandiseType.Undefined;
            HierarchyProfile hp = aSAB.HierarchyServerSession.GetMainHierarchyData();
            for (int levelIndex = 1; levelIndex <= hp.HierarchyLevels.Count; levelIndex++)
            {
                HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[levelIndex];
                if (hlp.LevelType == eHierarchyLevelType.Style)
                {
                    _inventory_MERCH_PHL_SEQ = hlp.Level;
                    _inventory_MERCH_PH_RID = hp.Key;
                    _inventory_MERCH_HN_RID = Include.NoRID;
                    _inventoryMerchandiseType = eMerchandiseType.HierarchyLevel;
                    break;
                }
            }
            // BEGIN TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis
            _HdrinventoryMinMax = false;
            _HdrinventoryInd = 'A';
            _Hdrinventory_MERCH_PHL_SEQ = 0;
            _Hdrinventory_MERCH_PH_RID = Include.NoRID;
            _Hdrinventory_MERCH_HN_RID = Include.NoRID;
            _HdrinventoryMerchandiseType = eMerchandiseType.Undefined;
            HierarchyProfile hp2 = aSAB.HierarchyServerSession.GetMainHierarchyData();
            for (int levelIndex = 1; levelIndex <= hp2.HierarchyLevels.Count; levelIndex++)
            {
                HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp2.HierarchyLevels[levelIndex];
                if (hlp.LevelType == eHierarchyLevelType.Style)
                {
                    _Hdrinventory_MERCH_PHL_SEQ = hlp.Level;
                    _Hdrinventory_MERCH_PH_RID = hp2.Key;
                    _Hdrinventory_MERCH_HN_RID = Include.NoRID;
                    _HdrinventoryMerchandiseType = eMerchandiseType.HierarchyLevel;
                    break;
                }
            }
            // End TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis
            _ipl = null;
            _packRoundingOverrideList = new List<HeaderPackRoundingOverride>();
        }
        /// <summary>
        /// Gets or sets the Shipping Horizon Beginning date CDR RID
        /// </summary>
        public int BeginCdrRID
        {
            get { return _beginCdrRid; }
            set {_beginCdrRid = value; }
        }
        /// <summary>
        /// Gets or sets the Shipping Horizon Ending date CDR RID
        /// </summary>
        public int ShipToCdrRID
        {
            get { return _shipToCdrRid; }
            set { _shipToCdrRid = value; }
        }

        /// <summary>
        /// Gets or sets the sGstoreGroupRID
        /// </summary>
        public int GradeStoreGroupRID
        {
            get { return _gradeStoreGroupRID; }
            set { _gradeStoreGroupRID = value; }
        }
        /// <summary>
        /// Gets or sets the CapacityStoreGroupRID which identifies the StoreGroupRID used by Capacity.
        /// </summary>
        public int CapacityStoreGroupRID
        {
            get { return _capacityStoreGroupRID; }
            set { _capacityStoreGroupRID = value; }
        }
        
        /// <summary>
        /// Gets or sets the ExceedMaximums flag value.
        /// </summary>
        /// <remarks>
        /// True indicates that an allocation may exceed the "grade" maximum.
        /// </remarks>
        public bool ExceedMaximums
        {
            get { return _exceedMaximums; }
            set { _exceedMaximums = value; }
        }
        /// <summary>
        /// Gets or sets the ExceedCapacity flag value
        /// </summary>
        public bool ExceedCapacity
        {
            get { return _exceedCapacity; }
            set { _exceedCapacity = value; }
        }

        /// <summary>
        /// Gets or sets UseStoreGradeDefault
        /// </summary>
        public bool UseStoreGradeDefault
        {
            get { return _useStoreGradeDefault; }
            set { _useStoreGradeDefault = value; }
        }
		/// <summary>
		/// Gets or sets the number of weeks to use to calculate grades.
		/// </summary>
		public int GradeWeekCount
		{
            get { return _gradeWeekCount; }
            set
            {
                if (value < 1)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_GradeWeekCountCannotBeLT1,
                        MIDText.GetText(eMIDTextCode.msg_GradeWeekCountCannotBeLT1));
                }
                _gradeWeekCount = value;
            }
		}
 
        /// <summary>
        /// Gets or sets LineItemMinOverrideInd
        /// </summary>
        public bool LineItemMinOverrideInd
        {
            get { return _lineItemMinOverrideInd; }
            set { _lineItemMinOverrideInd = value; }
        }
        /// <summary>
        /// Gets or sets LineItemMinOverride
        /// </summary>
        public int LineItemMinOverride
        {
            get { return _lineItemMinOverride; }
            set { _lineItemMinOverride = value; }
        }

        /// <summary>
        /// Gets or sets UseAllColorsMinDefault
        /// </summary>
        public bool UseAllColorsMinDefault
        {
            get { return _useAllColorsMinDefault; }
            set { _useAllColorsMinDefault = value; }	// TT#1089 - md - stodd - all color min not holding - 
        }
        /// <summary>
        /// Gets or sets UseAllColorsMaxDefault
        /// </summary>
        public bool UseAllColorsMaxDefault
        {
            get { return _useAllColorsMaxDefault; }
            set { _useAllColorsMaxDefault = value; }
        }
        /// <summary>
        /// Gets or set AllColorMultiple
        /// </summary>
        public int AllColorMultiple
        {
            get { return _allColorMultiple; }
            set
            {
                if (value < 1)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_MultipleCannotBeLessThan1,
                        MIDText.GetText(eMIDTextCode.msg_MultipleCannotBeLessThan1));
                }
                _allColorMultiple = value;
            }
        }
        /// <summary>
        /// Gets or sets AllSizeMultiple
        /// </summary>
        public int AllSizeMultiple
        {
            get { return _allSizeMultiple; }
            set
            {
                if (value < 1)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_MultipleCannotBeLessThan1,
                        MIDText.GetText(eMIDTextCode.msg_MultipleCannotBeLessThan1));
                }
                _allSizeMultiple = value;
            }
        }
        /// <summary>
        /// Gets or sets AllColorMinimum
        /// </summary>
        public int AllColorMinimum
        {
            get { return _allColor.Minimum; }
            set { _allColor.SetMinimum(value); }
        }
        /// <summary>
        /// Gets or sets AllColorMaximum
        /// </summary>
        public int AllColorMaximum
        {
            get { return _allColor.Maximum; }
            set { _allColor.SetMaximum(value); }
        }
        /// <summary>
        /// Gets or sets AllColorShipUpTo
        /// </summary>
        public int AllColorShipUpTo
        {
            get { return _allColor.ShipUpTo; }
            set { _allColor.SetShipUpTo(value); }
        }

        /// <summary>
		/// Gets or sets the UsePctNeedDefault flag value.
		/// </summary>
		public bool UsePctNeedDefault
		{
			get	{ return _usePctNeedDefault; }
            set { _usePctNeedDefault = value; }
        }
		/// <summary>
		/// Gets or sets the percent need limit.
		/// </summary>
		/// <remarks>
		/// Controls the need algorithm. The need allocation process will stop for stores whose percent need is equal to or less than this limit.
		/// The first time a store achieves this limit during the current process, the store will get no additional units.
		/// </remarks>
		public double PercentNeedLimit
		{
			get { return _percentNeedLimit; }
            set { _percentNeedLimit = value; }
		}

		/// <summary>
		/// Gets or sets ReserveIsPercent flag value.
		/// </summary>
		public bool ReserveIsPercent
		{
			get { return _reserveQtyIsPercent; }
            set { _reserveQtyIsPercent = value; }
		}

		/// <summary>
		/// Gets or sets ReserveQty.
		/// </summary>
		/// <remarks>
		/// This quantity is a percent when ReserveIsPercet is true; otherwise, it is a unit value.
		/// </remarks>
		public double ReserveQty
		{
			get	{ return _reserveQty; }
            set { _reserveQty = value; }
		}
        /// <summary>
        /// Gets or sets ReserveAsBulk which is the portion of the reserve allocation  that should be bulk
        /// </summary>
		public double ReserveAsBulk
		{
			get { return _reserveAsBulk; }
            set { _reserveAsBulk = value; }
		}

        /// <summary>
        /// Gets or sets ReserveAsPacks which is the portion of the reserve allocation that should be pack
        /// </summary>
		public double ReserveAsPacks
		{
			get { return _reserveAsPacks; }
            set { _reserveAsPacks = value; }
		}

		/// <summary>
        /// Gets or sets the MerchUnspecified flag value.
        /// </summary>
        /// <remarks>
        /// True indicates that no Basis forecast level or node is set.
        /// </remarks>
        public bool MerchUnspecified
        {
            get { return _merchUnspecified; }
            set { _merchUnspecified = value; }
        }
        /// <summary>
		/// Gets or sets OTSPlanPHL which identifies a dynamic product level within the heirarchy as a source of plans.
		/// </summary>
		/// <remarks>
		/// This variable identifies the level within the hierarchy where the system should look for plans.  The plan chosen is relative to the style on the header.
		/// </remarks>
		public int OTSPlanPHL
		{
			get	{ return _OTSPlan.ProductHnLvlRID; }
            set { _OTSPlan.SetProductHnLvlRID(value); }
		}
        /// <summary>
        /// Gets or sets OTSPlanPHLSeq which identifies the Product Hierarchy Node Level Sequence
        /// </summary>
		public int OTSPlanPHLSeq
		{
			get	{ return _OTSPlan.ProductHnLvlSeq; }
            set { _OTSPlan.SetProductHnLvlSeq(value); }
		}
		/// <summary>
		/// Gets or sets OTSPlanRID which identifies a specific merchandise plan within the hierarchy as a source of plans.
		/// </summary>
		public int OTSPlanRID
		{
			get	{ return _OTSPlan.MdseHnRID; }
            set { _OTSPlan.SetMdseHnRID(value); }
		}

		/// <summary>
        /// Gets or sets the OnHandUnspecified flag value.
        /// </summary>
        /// <remarks>
        /// True indicates that no Basis Onhand forecast level or node is set.
        /// </remarks>
        public bool OnHandUnspecified
        {
            get { return _onHandUnspecified; }
            set { _onHandUnspecified = value; }
        }
        /// <summary>
		/// Gets or sets the OTSOnHandPHL which identifies a dynamic product level within the hierarchy as a source for store actual Onhands and Intransit. The merchandise chosen at this level will be relative to the header's style.
		/// </summary>
		public int OTSOnHandPHL
		{
			get	{ return _OTSOnHand.ProductHnLvlRID; }
            set { _OTSOnHand.SetProductHnLvlRID(value); }
		}
        /// <summary>
        /// Gets or sets OTSOnHandPHLSeq which identifies the Product Hierarchy Node Level Sequence
        /// </summary>
		public int OTSOnHandPHLSeq
		{
			get	{ return _OTSOnHand.ProductHnLvlSeq; }
            set { _OTSOnHand.SetProductHnLvlSeq(value); }
		}

		/// <summary>
		/// Gets or sets the OTSOnHandRID which identifies a specific merchandise node within the hierarchy as the source for store actual onhands and intransit.
		/// </summary>
		public int OTSOnHandRID
		{
			get { return _OTSOnHand.MdseHnRID; }
            set { _OTSOnHand.SetMdseHnRID(value); }
		}

		/// <summary>
		/// Gets or sets the UseFactorPctDefault flag value.
		/// </summary>
		public bool UseFactorPctDefault
		{
			get { return _useFactorPctDefault; }
            set { _useFactorPctDefault = value; }
		}
        /// <summary>
		/// Gets or sets the OTSPlanFactorPercent.
		/// </summary>
		/// <remarks>
		/// This percent is used to extract the part of a plan that the onhand source represents.
		/// </remarks>
		public double OTSPlanFactorPercent
		{
			get	{ return _OTSPlanFactorPercent;	}
            set { _OTSPlanFactorPercent = value; }
        }

        public char InventoryInd
        {
            get { return _inventoryInd; }
            set
            {
                _inventoryInd = value;
                if (value == 'A')
                {
                    _inventoryMinMax = false;   // TT#946 - MD - Jellis - Group Allocation Not Working
                }
                else
                {
                    _inventoryMinMax = true;    // TT#946 - MD - Jellis - Group Allocation Not Working
                }
            }
        }
        /// <summary>
        /// Gets or sets InventoryMinMax
        /// </summary>
        public bool InventoryMinMax
        {
            get { return _inventoryMinMax; }
        }
 
 
        /// <summary>
        /// Gets or sets Inventory Merchandise Type
        /// </summary>
        public eMerchandiseType InventoryMerchandiseType
        {
            get { return _inventoryMerchandiseType; }
            set { _inventoryMerchandiseType = value; }
        }
        /// <summary>
        /// Gets or sets Inventory_MERCH_HN_RID
        /// </summary>
        public int Inventory_MERCH_HN_RID
        {
            get { return _inventory_MERCH_HN_RID; }
            set { _inventory_MERCH_HN_RID = value; }
        }

        /// <summary>
        /// Gets or sets Inventory_MERCH_PH_RID
        /// </summary>
        public int Inventory_MERCH_PH_RID
        {
            get { return _inventory_MERCH_PH_RID; }
            set { _inventory_MERCH_PH_RID = value; }
        }

        /// <summary>
        /// Gets or sets Inventory_MERCH_PHL_SEQ
        /// </summary>
        public int Inventory_MERCH_PHL_SEQ
        {
            get { return _inventory_MERCH_PHL_SEQ; }
            set { _inventory_MERCH_PHL_SEQ = value; }
        }

        // BEGIN TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis
        public char HdrInventoryInd
        {
            get { return _HdrinventoryInd; }
            set
            {
                _HdrinventoryInd = value;
                if (value == 'A')
                {
                    _HdrinventoryMinMax = false;   // TT#946 - MD - Jellis - Group Allocation Not Working
                }
                else
                {
                    _HdrinventoryMinMax = true;    // TT#946 - MD - Jellis - Group Allocation Not Working
                }
            }
        }
        /// <summary>
        /// Gets or sets InventoryMinMax
        /// </summary>
        public bool HdrInventoryMinMax
        {
            get { return _HdrinventoryMinMax; }
        }


        /// <summary>
        /// Gets or sets Inventory Merchandise Type
        /// </summary>
        public eMerchandiseType HdrInventoryMerchandiseType
        {
            get { return _HdrinventoryMerchandiseType; }
            set { _HdrinventoryMerchandiseType = value; }
        }
        /// <summary>
        /// Gets or sets Inventory_MERCH_HN_RID
        /// </summary>
        public int HdrInventory_MERCH_HN_RID
        {
            get { return _Hdrinventory_MERCH_HN_RID; }
            set { _Hdrinventory_MERCH_HN_RID = value; }
        }

        /// <summary>
        /// Gets or sets Inventory_MERCH_PH_RID
        /// </summary>
        public int HdrInventory_MERCH_PH_RID
        {
            get { return _Hdrinventory_MERCH_PH_RID; }
            set { _Hdrinventory_MERCH_PH_RID = value; }
        }

        /// <summary>
        /// Gets or sets Inventory_MERCH_PHL_SEQ
        /// </summary>
        public int HdrInventory_MERCH_PHL_SEQ
        {
            get { return _Hdrinventory_MERCH_PHL_SEQ; }
            set { _Hdrinventory_MERCH_PHL_SEQ = value; }
        }
        // END TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis
        /// <summary>
        /// Gets or sets StoreVSWOverride (when not null, identifies Store VSW Override criteria)
        /// </summary>
        public IMOProfileList StoreVSWOverride
        {
            get { return _ipl; }
            set { _ipl = value; }
        }
        public List<HeaderPackRoundingOverride> PackRoundingOverrideList
        {
            get
            {
                return _packRoundingOverrideList;
            }
            set
            {
                _packRoundingOverrideList = value;
            }
        }
        public int GradeCount
        {
            get { return _grade.Count; }
        }
        /// <summary>
        /// Add store grade by set, boundary and name
        /// </summary>
        /// <param name="aAttributeSetRID">Attribute Set RID (aka Store Group Level RID</param>
        /// <param name="aLowBoundary">Grade Low Boundary</param>
        /// <param name="aGradeName">Grade Name</param>
        public void AddGrade(int aAttributeSetRID, double aLowBoundary ,string aGradeName)
		{
			GroupAllocationGradeBin gradeBin;
            Dictionary<double, GroupAllocationGradeBin> boundaryDICT;
            if (!_grade.TryGetValue(aAttributeSetRID, out boundaryDICT))
            {
                boundaryDICT = new Dictionary<double, GroupAllocationGradeBin>();
                _grade.Add(aAttributeSetRID, boundaryDICT);
            }
            if (!boundaryDICT.TryGetValue(aLowBoundary, out gradeBin))
			{
				gradeBin = new GroupAllocationGradeBin();
				gradeBin.SetLowBoundary(aLowBoundary);
				gradeBin.SetGrade(aGradeName);
				gradeBin.SetGradeAdMinimum(0);
				gradeBin.SetGradeColorMaximum(gradeBin.GradeColorLargestMaximum);
				gradeBin.SetGradeColorMinimum(0);
				gradeBin.SetGradeMaximum(gradeBin.GradeLargestMaximum);
				gradeBin.SetGradeMinimum(0);
                gradeBin.SetGradeAttributeSet(aAttributeSetRID);
                gradeBin.SetGradeShipUpTo(0);
                gradeBin.SetGroupGradeMaximum(gradeBin.GradeLargestMaximum);
                gradeBin.SetGroupGradeMinimum(0);
                gradeBin.SetGroupGradeOriginalMaximum(gradeBin.GradeLargestMaximum);
                gradeBin.SetGroupGradeOriginalMinimum(0);
                boundaryDICT.Add(aLowBoundary, gradeBin);
			}
			else
			{
				// BEGIN TT#618 - STOdd - Allocation Override - Add Attribute Sets (#35)

				// *****   TEMPORARY  ***** //

				// Until the TT#618 processing is begun, I've commented this out.
				// The method that calls this method is sending every row from the 
				// store grades table. This table now contains the grades for each attribute set.
				// This causes this function to throw the exception.

				//throw new MIDException (eErrorLevel.warning,
				//    (int)eMIDTextCode.msg_DuplicateGradeNameNotAllowed,
				//    MIDText.GetText(eMIDTextCode.msg_DuplicateGradeNameNotAllowed));

				// END TT#618 - STOdd - Allocation Override - Add Attribute Sets (#35)
			}
		}

        public Dictionary<int, Dictionary<double, AllocationGradeBin>> GetHeaderAllocationGrades()
        {
            Dictionary<int, Dictionary<double, AllocationGradeBin>> headerGrades 
                = new Dictionary<int, Dictionary<double, AllocationGradeBin>>();
            Dictionary<double, AllocationGradeBin> headerBoundaryDict;
            AllocationGradeBin gradeBin;
            foreach (KeyValuePair<int, Dictionary<double, GroupAllocationGradeBin>> gradeKeyValue in _grade)
            {
                headerBoundaryDict = new Dictionary<double, AllocationGradeBin>();
                foreach (KeyValuePair<double, GroupAllocationGradeBin> boundaryKeyValue in gradeKeyValue.Value)
                {
                    gradeBin = new AllocationGradeBin();
                    gradeBin.SetLowBoundary(boundaryKeyValue.Value.LowBoundary);
				    gradeBin.SetGrade(boundaryKeyValue.Value.Grade);
				    gradeBin.SetGradeAdMinimum(boundaryKeyValue.Value.GradeAdMinimum);
				    gradeBin.SetGradeColorMaximum(boundaryKeyValue.Value.GradeColorMaximum);
				    gradeBin.SetGradeColorMinimum(boundaryKeyValue.Value.GradeColorMinimum);
				    gradeBin.SetGradeMaximum(boundaryKeyValue.Value.GradeMaximum);
				    gradeBin.SetGradeMinimum(boundaryKeyValue.Value.GradeMinimum);
                    gradeBin.SetGradeAttributeSet(boundaryKeyValue.Value.GradeSglRID);
                    gradeBin.SetGradeShipUpTo(boundaryKeyValue.Value.GradeShipUpTo);
                    headerBoundaryDict.Add(boundaryKeyValue.Key, gradeBin);
                }
                headerGrades.Add(gradeKeyValue.Key, headerBoundaryDict);
            }
            return headerGrades;
        }
 
        public Dictionary<int, Dictionary<double, GroupAllocationGradeBin>> GetGroupAllocationGrades()
        {
            Dictionary<int, Dictionary<double, GroupAllocationGradeBin>> groupGrades
                = new Dictionary<int,Dictionary<double,GroupAllocationGradeBin>>();
            Dictionary<double, GroupAllocationGradeBin> groupBoundaryDict;
            GroupAllocationGradeBin gradeBin;
            foreach (KeyValuePair<int, Dictionary<double, GroupAllocationGradeBin>> gradeKeyValue in _grade)
            {
                groupBoundaryDict = new Dictionary<double, GroupAllocationGradeBin>();
                foreach (KeyValuePair<double, GroupAllocationGradeBin> boundaryKeyValue in gradeKeyValue.Value)
                {
                    gradeBin = new GroupAllocationGradeBin();
                    gradeBin.SetLowBoundary(boundaryKeyValue.Value.LowBoundary);
				    gradeBin.SetGrade(boundaryKeyValue.Value.Grade);
				    gradeBin.SetGradeAdMinimum(boundaryKeyValue.Value.GradeAdMinimum);
				    gradeBin.SetGradeColorMaximum(boundaryKeyValue.Value.GradeColorMaximum);
				    gradeBin.SetGradeColorMinimum(boundaryKeyValue.Value.GradeColorMinimum);
				    gradeBin.SetGradeMaximum(boundaryKeyValue.Value.GradeMaximum);
				    gradeBin.SetGradeMinimum(boundaryKeyValue.Value.GradeMinimum);
                    gradeBin.SetGradeAttributeSet(boundaryKeyValue.Value.GradeSglRID);
                    gradeBin.SetGradeShipUpTo(boundaryKeyValue.Value.GradeShipUpTo);
                    gradeBin.SetGroupGradeMaximum(boundaryKeyValue.Value.GroupGradeMaximum);
                    gradeBin.SetGroupGradeMinimum(boundaryKeyValue.Value.GroupGradeMinimum);
                    groupBoundaryDict.Add(boundaryKeyValue.Key, gradeBin);
                }
                groupGrades.Add(gradeKeyValue.Key, groupBoundaryDict);
            }
            return groupGrades;
        }
        public void SetGradeAttributeSet(int aAttributeSetRID, double aLowBoundary)
        {
            Dictionary<double, GroupAllocationGradeBin> boundaryDICT;
            if (_grade.TryGetValue(aAttributeSetRID, out boundaryDICT))
            {
                GroupAllocationGradeBin gradeBin;
                if (boundaryDICT.TryGetValue(aLowBoundary, out gradeBin))
                {
                    gradeBin.SetGradeAttributeSet(aAttributeSetRID);
                    return;
                }
            }
            throw new MIDException(eErrorLevel.warning,
                (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
                MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));

        }
        /// <summary>
        /// Set Group Grade Minimum by Attribute Set and grade Low Boundary
        /// </summary>
        /// <param name="aAttributeSetRID">Attribute set RID (aka Store Group Level RID)</param>
        /// <param name="aLowBoundary">Grade Low Boundary</param>
        /// <param name="aMinimum">Group Minimum quantity</param>
        public void SetGroupGradeMinimum(int aAttributeSetRID, double aLowBoundary, int aMinimum)
        {
            Dictionary<double, GroupAllocationGradeBin> boundaryDICT;
            if (_grade.TryGetValue(aAttributeSetRID, out boundaryDICT))
            {
                GroupAllocationGradeBin gradeBin;
                if (boundaryDICT.TryGetValue(aLowBoundary, out gradeBin))
                {
                    gradeBin.SetGroupGradeMinimum(aMinimum);
                    return;
                }
            }
            throw new MIDException(eErrorLevel.warning,
                (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
                MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
        }   
        /// <summary>
        /// Set Grade Minimum by Attribute Set and grade Low Boundary
        /// </summary>
        /// <param name="aAttributeSetRID">Attribute set RID (aka Store Group Level RID)</param>
        /// <param name="aLowBoundary">Grade Low Boundary</param>
        /// <param name="aMinimum">Minimum quantity</param>
		public void SetHeaderGradeMinimum (int aAttributeSetRID, double aLowBoundary, int aMinimum)
		{
            Dictionary<double, GroupAllocationGradeBin> boundaryDICT;
            if (_grade.TryGetValue(aAttributeSetRID, out boundaryDICT))
            {
                GroupAllocationGradeBin gradeBin;
                if (boundaryDICT.TryGetValue(aLowBoundary, out gradeBin))
                {
                    gradeBin.SetGradeMinimum(aMinimum);
                    return;
                }
            }
            throw new MIDException(eErrorLevel.warning,
                (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
                MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
		}
        /// <summary>
        /// Set Grade Ad Minimum by Attribute Set and grade Low Boundary
        /// </summary>
        /// <param name="aAttributeSetRID">Attribute set RID (aka Store Group Level RID)</param>
        /// <param name="aLowBoundary">Grade Low Boundary</param>
        /// <param name="aAdMinimum">Ad Minimum quantity</param>
        public void SetHeaderGradeAdMinimum(int aAttributeSetRID, double aLowBoundary, int aAdMinimum)
		{
            Dictionary<double, GroupAllocationGradeBin> boundaryDICT;
            if (_grade.TryGetValue(aAttributeSetRID, out boundaryDICT))
            {
                GroupAllocationGradeBin gradeBin;
                if (boundaryDICT.TryGetValue(aLowBoundary, out gradeBin))
                {
                    gradeBin.SetGradeAdMinimum(aAdMinimum);
                    return;
                }
            }
            throw new MIDException(eErrorLevel.warning,
                (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
                MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
		}
        /// <summary>
        /// Set Group Grade Maximum by Attribute Set and Low Boundary
        /// </summary>
        /// <param name="aAttributeSetRID">Attribute Set RID (aka Store Group Level RID)</param>
        /// <param name="aLowBoundary">Grade Low Boundary</param>
        /// <param name="aMaximum">Group Maximum quantity</param>
        public void SetGroupGradeMaximum(int aAttributeSetRID, double aLowBoundary, int aMaximum)
        {
            Dictionary<double, GroupAllocationGradeBin> boundaryDICT;
            if (_grade.TryGetValue(aAttributeSetRID, out boundaryDICT))
            {
                GroupAllocationGradeBin gradeBin;
                if (boundaryDICT.TryGetValue(aLowBoundary, out gradeBin))
                {
                    gradeBin.SetGroupGradeMaximum(aMaximum);
                    return;
                }
            }
            throw new MIDException(eErrorLevel.warning,
                (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
                MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
        }
        /// <summary>
        /// Set Grade Maximum by Attribute Set and Low Boundary
        /// </summary>
        /// <param name="aAttributeSetRID">Attribute Set RID (aka Store Group Level RID)</param>
        /// <param name="aLowBoundary">Grade Low Boundary</param>
        /// <param name="aMaximum">Maximum quantity</param>
        public void SetHeaderGradeMaximum(int aAttributeSetRID, double aLowBoundary, int aMaximum)
		{
            Dictionary<double, GroupAllocationGradeBin> boundaryDICT;
            if (_grade.TryGetValue(aAttributeSetRID, out boundaryDICT))
            {
                GroupAllocationGradeBin gradeBin;
                if (boundaryDICT.TryGetValue(aLowBoundary, out gradeBin))
                {
                    gradeBin.SetGradeMaximum(aMaximum);
                    return;
                }
            }
            throw new MIDException(eErrorLevel.warning,
                (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
                MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
		}
        public void SetHeaderGradeShipUpTo (int aAttributeSetRID, double aLowBoundary, int aShipUpTo)
        {
            Dictionary<double, GroupAllocationGradeBin> boundaryDICT;
            if (_grade.TryGetValue(aAttributeSetRID, out boundaryDICT))
            {
                GroupAllocationGradeBin gradeBin;
                if (boundaryDICT.TryGetValue(aLowBoundary, out gradeBin))
                {
                    gradeBin.SetGradeShipUpTo(aShipUpTo);
                    return;
                }
            }
            throw new MIDException(eErrorLevel.warning,
                (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
                MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
        }
        public void SetColorGradeMinimum(int aAttributeSetRID, double aLowBoundary, int aColorMinimum)
        {
            Dictionary<double, GroupAllocationGradeBin> boundaryDICT;
            if (_grade.TryGetValue(aAttributeSetRID, out boundaryDICT))
            {
                GroupAllocationGradeBin gradeBin;
                if (boundaryDICT.TryGetValue(aLowBoundary, out gradeBin))
                {
                    gradeBin.SetGradeColorMinimum(aColorMinimum);
                    return;
                }
            }
            throw new MIDException(eErrorLevel.warning,
                (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
                MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
        }
        public void SetColorGradeMaximum(int aAttributeSetRID, double aLowBoundary, int aColorMaximum)
        {
            Dictionary<double, GroupAllocationGradeBin> boundaryDICT;
            if (_grade.TryGetValue(aAttributeSetRID, out boundaryDICT))
            {
                GroupAllocationGradeBin gradeBin;
                if (boundaryDICT.TryGetValue(aLowBoundary, out gradeBin))
                {
                    //gradeBin.SetGradeColorMinimum(aColorMaximum); // TT#4081 - MD - Jellis - Grade Color Min_Max not observed 
                    gradeBin.SetGradeColorMaximum(aColorMaximum);   // TT#4081 - MD - Jellis - Grade Color Min_Max not observed
                    return;
                }
            }
            throw new MIDException(eErrorLevel.warning,
                (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
                MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
        }
        /// <summary>
        /// Add Color Minimum and Maximum table entry
        /// </summary>
        /// <param name="aColorRID"></param>
        /// <returns></returns>
        public ColorOrSizeMinMaxBin AddColorMinMax(int aColorRID)
        {
            ColorOrSizeMinMaxBin colorBin;
            if (!_colorMinMax.TryGetValue(aColorRID, out colorBin)) // TT#1014 - Allocation Override - Duplicate Color Not Allowed
            {
                colorBin = new ColorOrSizeMinMaxBin(aColorRID);
                colorBin.SetMaximum(AllColorMaximum);
                colorBin.SetMinimum(AllColorMinimum);
                _colorMinMax.Add(aColorRID, colorBin);
                return colorBin;
            }
            throw new MIDException(eErrorLevel.warning,
                (int)eMIDTextCode.msg_DuplicateColorNotAllowed,
                MIDText.GetText(eMIDTextCode.msg_DuplicateColorNotAllowed));
        }
        /// <summary>
        /// Determines if there is a min/max for the color.
        /// </summary>
        /// <param name="aColorRID">Color RID</param>
        /// <returns>True when there is a min/max for the color; false if there is no specific min/max for the color</returns>
        public bool ColorMinMaxExists(int aColorRID)
        {
            return _colorMinMax.ContainsKey(aColorRID);
        }

        /// <summary>
        /// Removes color from method's color min/max table.
        /// </summary>
        /// <param name="aColorRID"></param>
        public void RemoveColorMinMaxFromMethod(int aColorRID)
        {
            _colorMinMax.Remove(aColorRID);
        }
        /// <summary>
        /// Get Color Maximum
        /// </summary>
        /// <param name="aColorRID">RID that identifies the color</param>
        /// <returns>Color Maximum</returns>
        public int GetColorMax(int aColorRID)
        {
            ColorOrSizeMinMaxBin colorBin;
            if (_colorMinMax.TryGetValue(aColorRID, out colorBin))
            {
                return colorBin.Maximum;
            }
            throw new MIDException(eErrorLevel.warning,
                (int)eMIDTextCode.msg_ColorNotDefinedInMethod,
                MIDText.GetText(eMIDTextCode.msg_ColorNotDefinedInMethod));
        }
        /// <summary>
        /// Set Color Maximum
        /// </summary>
        /// <param name="aColorRID">RID that identifies the color</param>
        /// <param name="aMaximum">Color Maximum value</param>
        public void SetColorMax(int aColorRID, int aMaximum)
        {
            ColorOrSizeMinMaxBin colorBin;
            if (!_colorMinMax.TryGetValue(aColorRID, out colorBin))
            {
                colorBin = AddColorMinMax(aColorRID);
            }
            colorBin.SetMaximum(aMaximum);
        }
        /// <summary>
        /// Gets Color Minimum
        /// </summary>
        /// <param name="aColorRID">RID that identifies the color</param>
        /// <returns>Color minmum value</returns>
        public int GetColorMin(int aColorRID)
        {
            ColorOrSizeMinMaxBin colorBin;
            if (_colorMinMax.TryGetValue(aColorRID, out colorBin))
            {
                return colorBin.Minimum;
            }
            throw new MIDException(eErrorLevel.warning,
                (int)eMIDTextCode.msg_ColorNotDefinedInMethod,
                MIDText.GetText(eMIDTextCode.msg_ColorNotDefinedInMethod));
        }
        /// <summary>
        /// Set Color Minimum
        /// </summary>
        /// <param name="aColorRID">RID that identifies the color</param>
        /// <param name="aMinimum">Color Minimum value</param>
        public void SetColorMin(int aColorRID, int aMinimum)
        {
             ColorOrSizeMinMaxBin colorBin;
            if (!_colorMinMax.TryGetValue(aColorRID, out colorBin))
            {
                colorBin = AddColorMinMax(aColorRID);
            }
            colorBin.SetMinimum(aMinimum);
        }
        /// <summary>
        /// Determine if color has sizes
        /// </summary>
        /// <param name="aColorRID">RID that identifies the color</param>
        /// <returns>True: color has sizes; False: color does not have size defined</returns>
        public bool DoesColorHaveSizes(int aColorRID)
        {
            return _colorSizeMinMax.ContainsKey(aColorRID);
        }
        /// <summary>
        /// Determine if a size minimum/maximum is defined for a color
        /// </summary>
        /// <param name="aColorRID">RID that identifies the color</param>
        /// <param name="aSizeRID">RID that identifies the size</param>
        /// <returns>True: Size has minimum/maximum defined for color; False: Size does not have minimum/maximum defined in color</returns>
        public bool IsColorSizeMinMax(int aColorRID, int aSizeRID)
        {
            if (!(DoesColorHaveSizes(aColorRID)))
            {
                return false;
            }
            ColorSizeMinMaxBin aMinMaxBin = (ColorSizeMinMaxBin)_colorSizeMinMax[aColorRID];
            return aMinMaxBin.IsSizeMinMaxInMethodColor(aSizeRID);
        }
        /// <summary>
        /// Get Color Size Minimum
        /// </summary>
        /// <param name="aColorRID">RID that identifies the color</param>
        /// <param name="aSizeRID">RID that identifies the size</param>
        /// <returns>Size Minimum value</returns>
        public int GetColorSizeMin(int aColorRID, int aSizeRID)
        {
            ColorSizeMinMaxBin minMaxBin;
            if (_colorSizeMinMax.TryGetValue(aColorRID, out minMaxBin))
            {
                return minMaxBin.GetSizeMinimum(aSizeRID);
            }
            return 0;
        }
        /// <summary>
        /// Set Color Size Minimum
        /// </summary>
        /// <param name="aColorRID">RID that identifies the color</param>
        /// <param name="aSizeRID">RID that identifies the size</param>
        /// <param name="aMinimum">Size minimum value</param>
        public void SetColorSizeMin(int aColorRID, int aSizeRID, int aMinimum)
        {
            ColorSizeMinMaxBin minMaxBin;
            if (!_colorSizeMinMax.TryGetValue(aColorRID, out minMaxBin))
            {
                minMaxBin = new ColorSizeMinMaxBin(aColorRID);
                _colorSizeMinMax.Add(aColorRID, minMaxBin);
            }
            minMaxBin.SetSizeMinimum(aSizeRID, aMinimum);
        }
        /// <summary>
        /// Get Color Size maximum
        /// </summary>
        /// <param name="aColorRID">RID that identifies the color</param>
        /// <param name="aSizeRID">RID that identifies the size</param>
        /// <returns>Size Maximum value</returns>
        public int GetColorSizeMax(int aColorRID, int aSizeRID)
        {
            ColorSizeMinMaxBin minMaxBin;
            if (_colorSizeMinMax.TryGetValue(aColorRID, out minMaxBin))
            {
                return minMaxBin.GetSizeMaximum(aSizeRID);
            }
            return int.MaxValue; // Default Maximum should be largest value not 0
        }
        /// <summary>
        /// Set Color Size Maximum
        /// </summary>
        /// <param name="aColorRID">RID that identifies the color</param>
        /// <param name="aSizeRID">RID that identifies the size</param>
        /// <param name="aMaximum">Size Maximum value</param>
        public void SetColorSizeMax(int aColorRID, int aSizeRID, int aMaximum)
        {
            ColorSizeMinMaxBin minMaxBin;
            if (!_colorSizeMinMax.TryGetValue(aColorRID, out minMaxBin))
            {
                minMaxBin = new ColorSizeMinMaxBin(aColorRID);
                _colorSizeMinMax.Add(aColorRID, minMaxBin);
            }
            minMaxBin.SetSizeMaximum(aSizeRID, aMaximum);
        }
        /// <summary>
        /// Deterimine if an Attribute Set (aka Store Group Level) is defined for capacity
        /// </summary>
        /// <param name="aAttributeSetRID">RID that identifies the Attribute Set (aka Store Group Level)</param>
        /// <returns>True: Attribute Set is defined; False: Attribute Set is not defined</returns>
		public bool CapacityStrGroupLvlExists(int aAttributeSetRID)
		{
            return _capacity.ContainsKey(aAttributeSetRID);
		}
        /// <summary>
        /// Remove Attribute Set (aka Store Group Level) from capacity
        /// </summary>
        /// <param name="aAttributeSetRID">RID that identifies the Attribute Set (aka Store Group Level)</param>
        public void RemoveStrGroupLvlFromMethod(int aAttributeSetRID)
		{
            _capacity.Remove(aAttributeSetRID);
		}
		/// <summary>
		/// Adds Attribute Set to Capacity.
		/// </summary>
        /// <param name="aAttributeSetRID">RID that identifies the Attribute Set (aka Store Group Level)</param>
        public AllocationCapacityBin AddCapacityStrGroupLvl(int aAttributeSetRID)
		{
            AllocationCapacityBin capacityBin;    
            if (_capacity.TryGetValue(aAttributeSetRID, out capacityBin))
            {
                throw new MIDException (eErrorLevel.warning,
                    (int)eMIDTextCode.msg_DuplicateStrGroupLvlNotAllowed,
                    MIDText.GetText(eMIDTextCode.msg_DuplicateStrGroupLvlNotAllowed));
            }
            // Begin TT#1010 - MD - stodd - ALloc Override-Cpacity tab-ener values and Process, get MID Form error - 
            if (capacityBin == null)
            {
                capacityBin = new AllocationCapacityBin();
            }
            // End TT#1010 - MD - stodd - ALloc Override-Cpacity tab-ener values and Process, get MID Form error - 
            capacityBin.SetStoreGroupLevelRID(aAttributeSetRID);
            capacityBin.SetExceedCapacity(false);
            capacityBin.SetExceedCapacityByPercent(0.0d);
            _capacity.Add(aAttributeSetRID, capacityBin);
            return capacityBin;
		}
        /// <summary>
        /// Get Attribute Set Exceed Capacity Indicator
        /// </summary>
        /// <param name="aAttributeSetRID">RID that identifies the Attribute Set (aka Store Group Level)</param>
        /// <returns></returns>
        public bool GetStrGroupLvlExceedCapacity(int aAttributeSetRID)
		{
            AllocationCapacityBin capacityBin;
            if (!_capacity.TryGetValue(aAttributeSetRID, out capacityBin))
            {
                throw new MIDException (eErrorLevel.warning,
                    (int)eMIDTextCode.msg_StrGroupLvlNotDefinedInMethod,
                    MIDText.GetText(eMIDTextCode.msg_StrGroupLvlNotDefinedInMethod));
            }
            return capacityBin.ExceedCapacity;
		}

		/// <summary>
        /// Sets Attribute Set (aka Store Group Level) exceed capacity flag value
		/// </summary>
        /// <param name="aAttributeSetRID">RID that identifies the Attribute Set (aka Store Group Level)</param>
        /// <param name="aExceedCapacity">Exceed Capacity Flag Value: true or false.</param>
        public void SetStrGroupLvlExceedCapacity(int aAttributeSetRID, bool aExceedCapacity)
		{
            AllocationCapacityBin capacityBin;
            if (!_capacity.TryGetValue(aAttributeSetRID, out capacityBin))
            {
                capacityBin = AddCapacityStrGroupLvl(aAttributeSetRID);
            }
            capacityBin.SetExceedCapacity(aExceedCapacity);
		}
        /// <summary>
        ///  Get Attribute Set (aka Store Group Level) Exceed Capacity By Percent
        /// </summary>
        /// <param name="aAttributeSetRID">RID that identifies the Attribute Set (aka Store Group Level)</param>
        /// <returns>Exceed Capacity By Percentage</returns>
        public double GetStrGroupLvlExceedCapacityByPct(int aAttributeSetRID)
		{
            AllocationCapacityBin capacityBin;
            if (!_capacity.TryGetValue(aAttributeSetRID, out capacityBin))
            {
                capacityBin = AddCapacityStrGroupLvl(aAttributeSetRID);
            }
            return capacityBin.ExceedCapacityByPercent;
		}

		/// <summary>
        /// Sets Attribute Set (aka Store Group Level) exceed capacity by percent value.
		/// </summary>
        /// <param name="aAttributeSetRID">RID that identifies the Attribute Set (aka Store Group Level)</param>
        /// <param name="aPercent">Percent by which the allocation may exceed capacity.</param>
        public void SetStrGroupLvlExceedCapacityByPct(int aAttributeSetRID, double aPercent)
		{
            AllocationCapacityBin capacityBin;
            if (!_capacity.TryGetValue(aAttributeSetRID, out capacityBin))
            {
                capacityBin = AddCapacityStrGroupLvl(aAttributeSetRID);
            }
            capacityBin.SetExceedCapacityByPercent(aPercent);
		}

    }
    /// <summary>
    /// Structure to hold Buid Item Indicators
    /// </summary>
    class BuildItemIndicators
    {
        private int _buildItemKey;
        private bool _buildItem;
        private bool _calcAsrtItemVsw;
        private bool _buildingItemVswValues;
        private bool _buildItemSuspended;
        private bool _calculateImoAloctnMax;
        private bool _determineHeaderVswProcessOrder;
        public BuildItemIndicators(int aBuildItemKey)
        {
            _buildItemKey = aBuildItemKey;
            _buildItem = false;
            _buildingItemVswValues = false;
            _buildItemSuspended = false;
            _calculateImoAloctnMax = true;
            _determineHeaderVswProcessOrder = true;
            _calcAsrtItemVsw = true;
        }
        public int BuildItemKey
        {
            get { return _buildItemKey; }
        }
        /// <summary>
        /// Gets or Sets bool that indicates whether to determine the header VSW processing order
        /// </summary>
        public bool DetermineHeaderVswProcessOrder
        {
            get { return _determineHeaderVswProcessOrder; }
            set { _determineHeaderVswProcessOrder = value; }
        }
        /// <summary>
        /// Gets or Sets bool that indicates whether to build item (ie. calculate VSW split)
        /// </summary>
        public bool BuildItem
        {
            get { return _buildItem; }
            set 
            { 
                _buildItem = value;
                if (_buildItem == true)
                {
                    _calcAsrtItemVsw = true;
                }
            }
        }
        /// <summary>
        /// Gets ot set bool that indicates whether Build Item is in progress.
        /// </summary>
        public bool BuildingItemVswValues
        {
            get { return _buildingItemVswValues; }
            set { _buildingItemVswValues = value; }
        }
        /// <summary>
        /// Gets or sets bool that indicates whether Build Item is suspended (ie. calculate VSW split is temporarily suspended so that other actions can occur without the overhead of the item value calculations)
        /// </summary>
        public bool BuildItemSuspended
        {
            get { return _buildItemSuspended; }
            set { _buildItemSuspended = value; }
        }
        /// <summary>
        /// Gets or sets bool that indicates whether to Calculate the Imo Allocation Maximum
        /// </summary>
        public bool CalculateImoAloctnMax
        {
            get { return _calculateImoAloctnMax; }
            set { _calculateImoAloctnMax = value; }
        }
        public bool CalculateAssortmentItemVsw
        {
            get { return _calcAsrtItemVsw; }
            set { _calcAsrtItemVsw = value; }
        }
    }
    // end TT#488 - MD - Jellis - Group Allocation
}
