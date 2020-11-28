using System;
using System.Collections;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;

namespace MIDRetail.Business.Allocation
{
	#region StoreAllocatedBaseBin
	/// <summary>
	/// Store allocation basic tracking information.
	/// </summary>
	internal struct StoreAllocatedBaseBin
	{
		//=======
		// FIELDS
		//=======
		private AllocationStoreDetailAuditFlags _storeDetailAudit;
		private int _qtyAllocated;
        private int _itemQtyAllocated; // TT#1401 - JEllis - Urban Reservation Store
        //private int _imoQtyAllocated; // TT#1401 - JEllis - Urban Reservation Store  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
		private int _origQtyAllocated;
        private MinMaxAllocationBin _minMax;
		private int _primaryMaximum;
		private int _qtyAllocatedByAuto;
        private bool _isChanged;
		private bool _isNew;

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========
        //internal ushort AllDetailAuditFlags  // TT#488 - MD - Jellis - Group Allocation
        internal uint AllDetailAuditFlags      // TT#488 - MD - Jellis - Group Allocation
		{
			get
			{
				return _storeDetailAudit.AllFlags;
			}
			set
			{
				_storeDetailAudit.AllFlags = value;
			}
		}
		/// <summary>
		/// Gets or sets IsManuallyAllocated audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the user manually allocated the units to this store.</remarks>
		internal bool IsManuallyAllocated
		{
			get
			{
				return _storeDetailAudit.IsManuallyAllocated;
			}
			set
			{
				if (IsManuallyAllocated != value)
				{
					_storeDetailAudit.IsManuallyAllocated = value;
					this.IsChanged = true;
				}
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
        /// <summary>
        /// Gets or sets ItemIsManuallyAllocated audit flag.
        /// </summary>
        /// <remarks>
        /// True indicates the user manually allocated the units to this store item.</remarks>
        internal bool ItemIsManuallyAllocated                     // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        {
            get
            {
                return _storeDetailAudit.ItemIsManuallyAllocated; // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
            }
            set
            {
                if (ItemIsManuallyAllocated != value)             // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
                {
                    _storeDetailAudit.ItemIsManuallyAllocated = value; // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
                    this.IsChanged = true;
                }
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 2

        // begin TT#1334 - Urban - Jellis - Balance to VSW Enhancement
        /// <summary>
        /// Gets or sets ItemQtyIsLocked audit flag
        /// </summary>
        internal bool ItemQtyIsLocked
        {
            get
            {
                return _storeDetailAudit.ItemQtyIsLocked;
            }
            set
            {
                _storeDetailAudit.ItemQtyIsLocked = value;
            }
        }
        // end TT#1334 - Urban - Jellis - Balance to VSW Enhancement

		/// <summary>
		/// Gets or sets WasAutoAllocated audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the system allocated units to the store (when IsManuallyAllocated
		/// is also true, the auto allocated units were overridden by the user.
		/// </remarks>
		internal bool WasAutoAllocated
		{
			get
			{
				return _storeDetailAudit.WasAutoAllocated;
			}
			set
			{
				if (WasAutoAllocated != value)
				{
					_storeDetailAudit.WasAutoAllocated = value;
					this.IsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets ChosenRuleAcceptedByHeader audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the Chosen Rule was accepted On Header.
		/// </remarks>
		internal bool ChosenRuleAcceptedByHeader // TT#488 - MD - Jellis - Group Allocation
		{
			get
			{
                return _storeDetailAudit.ChosenRuleAcceptedByHeader; // TT#488 - MD - Jellis - Group Allocation
			}
			set
			{
                if (_storeDetailAudit.ChosenRuleAcceptedByHeader != value) // TT#488 - MD - Jellis - Group Allocation
				{
                    _storeDetailAudit.ChosenRuleAcceptedByHeader = value; // TT#488 - MD - Jellis - Group Allocation
					this.IsChanged = true;
				}
			}
		}

        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets ChosenRuleAcceptedByGroup audit flag.
        /// </summary>
        /// <remarks>
        /// True indicates the Chosen Rule was accepted in Group Allocation.
        /// </remarks>
        internal bool ChosenRuleAcceptedByGroup // TT#488 - MD - Jellis - Group Allocation
        {
            get
            {
                return _storeDetailAudit.ChosenRuleAcceptedByGroup;
            }
            set
            {
                if (_storeDetailAudit.ChosenRuleAcceptedByGroup != value) 
                {
                    _storeDetailAudit.ChosenRuleAcceptedByGroup = value; 
                    this.IsChanged = true;
                }
            }
        }
        // end TT#488 - MD - Jellis - Group Allocation

		/// <summary>
		/// Gets or sets Out audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates a chosen rule has "outted" this store from allocation. When 
		/// IsManuallyAllocated is true, the user has overridden the out rule with an allocation.
		/// </remarks>
		internal bool Out
		{
			get
			{
				return _storeDetailAudit.Out;
			}
			set
			{
				if (Out != value)
				{
					_storeDetailAudit.Out = value;
					this.IsChanged = true;
				}
			}
		}

        // begin TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
        ///// <summary>
        ///// Gets or sets the Lock audit flag.
        ///// </summary>
        ///// <remarks>
        ///// True prevents the system from changing the allocation.  This lock is saved on the
        ///// database.
        ///// </remarks>
        //internal bool Locked
        //{
        //    get
        //    {
        //        return _storeDetailAudit.Locked;
        //    }
        //    set
        //    {
        //        if (Locked != value)
        //        {
        //            _storeDetailAudit.Locked = value;
        //            this.IsChanged = true;
        //        }
        //    }
        //}
        // end TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2

		// begin TT#59 Implement Temp Locks
        // /// <summary>
		// /// Gets or sets the TempLock audit flag.
		// /// </summary>
		// /// <remarks>
		// /// True prevents the system from changing the allocation.  This lock is not saved
		// /// on the database. It is used to temporarily hold an allocation value during
		// /// an allocation process.
		// /// </remarks>
		//internal bool TempLock
		//{
		//	get
		//	{
		//		return _storeDetailAudit.TempLock;
		//	}
		//	set
		//	{
		//		_storeDetailAudit.TempLock = value;
		//	}
		//}
        // end TT#59 Implement Temp Locks

		/// <summary>
		/// Gets or sets the HadNeed audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates that units were allocated based on need.
		/// </remarks>
		internal bool HadNeed
		{
			get 
			{
				return _storeDetailAudit.HadNeed;
			}
			set
			{
				if (HadNeed != value)
				{
					_storeDetailAudit.HadNeed = value;
					this.IsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets the FilledSizeHole audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates that units were allocated to fill in size holes.
		/// </remarks>
		internal bool FilledSizeHole
		{
			get
			{
				return _storeDetailAudit.FilledSizeHole;
			}
			set
			{
				if (FilledSizeHole != value)
				{
					_storeDetailAudit.FilledSizeHole = value;
					this.IsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets Rule Allocation from Parent Component flag
		/// </summary>
		internal bool RuleAllocationFromParentComponent
		{
			get
			{
				return _storeDetailAudit.RuleAllocationFromParentComponent;
			}
			set
			{
				if (RuleAllocationFromParentComponent != value)
				{
					_storeDetailAudit.RuleAllocationFromParentComponent = value;
					this.IsChanged = true;
				}
			}
		}

        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets Rule Allocation from Group Component flag
        /// </summary>
        internal bool RuleAllocationFromGroupComponent
        {
            get
            {
                return _storeDetailAudit.RuleAllocationFromGroupComponent;
            }
            set
            {
                if (RuleAllocationFromGroupComponent != value)
                {
                    _storeDetailAudit.RuleAllocationFromGroupComponent = value;
                    this.IsChanged = true;
                }
            }
        }
        // end TT#488 - MD - Jellis - Group Allocation


        /// <summary>
        /// Gets or Sets Rule Allocation From Child Component Flag
        /// </summary>
		internal bool RuleAllocationFromChildComponent
		{
			get
			{
				return _storeDetailAudit.RuleAllocationFromChildComponent;
			}
			set
			{
				if (RuleAllocationFromChildComponent != value)
				{
					_storeDetailAudit.RuleAllocationFromChildComponent = value;
					this.IsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets Rule Allocation From Chosen Rule Flag
		/// </summary>
		internal bool RuleAllocationFromChosenRule
		{
			get
			{
				return _storeDetailAudit.RuleAllocationFromChosenRule;
			}
			set
			{
				if (RuleAllocationFromChosenRule != value)
				{
					_storeDetailAudit.RuleAllocationFromChosenRule = value;
					this.IsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets Allocation From BottomUpSize Flag
		/// </summary>
		internal bool AllocationFromBottomUpSize
		{
			get
			{
				return _storeDetailAudit.AllocationFromBottomUpSize;
			}
			set
			{
				if (AllocationFromBottomUpSize != value)
				{
					_storeDetailAudit.AllocationFromBottomUpSize = value;
					this.IsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets Allocation From Bulk Size Break Out Flag
		/// </summary>
		internal bool AllocationFromBulkSizeBreakOut
		{
			get
			{
				return _storeDetailAudit.AllocationFromBulkSizeBreakOut;
			}
			set
			{
				if (AllocationFromBulkSizeBreakOut != value)
				{
					_storeDetailAudit.AllocationFromBulkSizeBreakOut = value;
					this.IsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets Allocation From PackContent Break Out Flag
		/// </summary>
		internal bool AllocationFromPackContentBreakOut
		{
			get
			{
				return _storeDetailAudit.AllocationFromPackContentBreakOut;
			}
			set
			{
				if (AllocationFromPackContentBreakOut != value)
				{
					_storeDetailAudit.AllocationFromPackContentBreakOut = value;
					this.IsChanged = true;
				}
			}
		}

		// begin MID Track 4448 AnF Audit Enhancement
		/// <summary>
		/// Gets or sets AllocationModifiedAfterMultiHeaderSplit Flag
		/// </summary>
		internal bool AllocationModifiedAfterMultiHeaderSplit
		{
			get
			{
				return _storeDetailAudit.AllocationModifiedAfterMultiHeaderSplit;
			}
			set
			{
				if (AllocationModifiedAfterMultiHeaderSplit != value)
				{
					_storeDetailAudit.AllocationModifiedAfterMultiHeaderSplit = value;
					this.IsChanged = true;
				}
			}
		}
		// end MID Track 4448 AnF Audit Enhancement

		/// <summary>
		/// Gets or sets QtyAllocated to store.
		/// </summary>
		/// <remarks>
		/// QtyAllocated must be a non-negative integer.
		/// </remarks>
		internal int QtyAllocated
		{
			get
			{
				return _qtyAllocated;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                        + ": QtyAllocated in " + GetType().Name);     // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                }
				else
				{
					if (QtyAllocated != value)
					{
						_qtyAllocated = value;
						this.IsChanged = true;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets OriginalQtyAllocated to store.
		/// </summary>
		/// <remarks>
		/// OriginalQtyAllocated must be a non-negative integer.
		/// </remarks>
		internal int OriginalQtyAllocated
		{
			get
			{
				return _origQtyAllocated;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                        + ": OriginalQtyAllocated in " + GetType().Name);     // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                }
				else
				{
					if (OriginalQtyAllocated != value)
					{
						_origQtyAllocated = value;
					}
				}
			}
		}

        // begin TT#1401 - JEllis - Urban Reserveation Stores pt 1
        internal int ItemQtyAllocated
        {
            get
            {
                return _itemQtyAllocated;
            }
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                        + ": ItemQtyAllocated in " + GetType().Name);     // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                }
                else
                {
                    if (ItemQtyAllocated != value)
                    {
                        _itemQtyAllocated = value;
                    }
                }
            }
        }
        internal int ImoQtyAllocated
        {
            // begin TT#1401 - JEllis Urban Virtual Store Warehouse pt 29
            get
            {
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
            }
            //get
            //{
            //    return _imoQtyAllocated;
            //}
            //set
            //{
            //    if (value < 0)
            //    {
            //        throw new MIDException(eErrorLevel.severe,
            //            (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
            //            MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
            //            + ": ImoQtyAllocated in " + GetType().Name);     // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
            //    }
            //    else
            //    {
            //        if (ImoQtyAllocated != value)
            //        {
            //            _imoQtyAllocated = value;
            //        }
            //    }
            //}
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1

		/// <summary>
		/// Gets or sets the allocation maximum.
		/// </summary>
		internal int Maximum
		{
			get
			{
				return _minMax.Maximum;
			}
			set
			{ 
				if (_minMax.Maximum != value)
				{
					_minMax.SetMaximum(value);
					this.IsChanged = true;
				}
			}
		}

        // Begin TT#617 - RMatelic - Allocation Override - Add Ship Up To Rule (#36, #39)
        /// <summary>
        /// Gets or sets the allocation maximum.
        /// </summary>
        internal int ShipUpTo
        {
            get
            {
                return _minMax.ShipUpTo;
            }
            set
            {
                if (_minMax.ShipUpTo != value)
                {
                    _minMax.SetShipUpTo(value);
                    this.IsChanged = true;
                }
            }
        }
        // End TT#617  

		/// <summary>
		/// Gets the largest possible maximum.
		/// </summary>
		internal int LargestMaximum
		{
			get
			{
				return _minMax.LargestMaximum;
			}
		}

		/// <summary>
		/// Gets or sets the allocation minimum.
		/// </summary>
		internal int Minimum
		{
			get
			{
				return _minMax.Minimum;
			}
			set
			{
				if (_minMax.Minimum != value)
				{
					_minMax.SetMinimum(value);
					this.IsChanged = true;
				}
			}
		}
		
		/// <summary>
		/// Gets or sets the primary maximum.
		/// </summary>
		internal int PrimaryMaximum
		{
			get
			{
				return _primaryMaximum;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_PMaxAllocationCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_PMaxAllocationCannotBeNeg));
				}
				else
				{
					if (PrimaryMaximum != value)
					{
						_primaryMaximum = value;
						this.IsChanged = true;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets QtyAllocatedByAuto.
		/// </summary>
		internal int QtyAllocatedByAuto
		{
			get
			{
				return _qtyAllocatedByAuto;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_AutoAllocationCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_AutoAllocationCannotBeNeg));
				}
				else
				{
					if (QtyAllocatedByAuto != value)
					{
						_qtyAllocatedByAuto = value;
						this.IsChanged = true;
					}
				}
			}
		}

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
		
		//========
		// METHODS
		//========
	}
	#endregion Store Allocated Base Bin

	#region Store Allocated Bin
	/// <summary>
	/// Store allocation basic tracking information plus Rule allocation tracking information.
	/// </summary>
	internal struct StoreAllocatedBin
	{
		//=======
		// FIELDS
		//=======
		private StoreAllocatedBaseBin _storeBase;
		private DateTime _lastNeedDay;                    
		private int _unitNeedBefore;                      // Based on _shipDay
		private double _percentNeedBefore;                // Based on _shipDay
		private int _chosenRuleLayerID;
		private eRuleType _chosenRuleType;
		private int _chosenRuleQtyAllocated;
		private int _qtyAllocatedByRule;
		//=============
		// CONSTRUCTORS
		//=============
	

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
				return _storeBase.IsChanged;
			}
			set
			{
				_storeBase.IsChanged = value;
			}
		}

		/// <summary>
		/// Gets or sets IsNew database update flag
		/// </summary>
		internal bool IsNew
		{
			get 
			{
				return _storeBase.IsNew;
			}
			set
			{
				_storeBase.IsNew = value;
			}
		}
		/// <summary>
		/// Gets or sets StoreLastNeedDay
		/// </summary>
		/// <remarks>
		/// Format DateTime
		/// </remarks>
		internal DateTime StoreLastNeedDay
		{
			get
			{
				return _lastNeedDay;
			}
			set
			{
				if (StoreLastNeedDay != value)
				{
					_lastNeedDay = value;
					this.IsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets Store's Unit Need Before.
		/// </summary>
		internal int StoreUnitNeedBefore
		{
			get
			{
				return _unitNeedBefore;
			}
			set
			{
				if (StoreUnitNeedBefore != value)
				{
					_unitNeedBefore = value;
					this.IsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets store's PercentNeedBefore
		/// </summary>
		internal double StorePercentNeedBefore
		{
			get
			{
				return _percentNeedBefore;
			}
			set
			{
				if (StorePercentNeedBefore != value)
				{
					_percentNeedBefore = value;
					this.IsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets AllDetailFlags simultaneously.
		/// </summary>
        //internal ushort AllDetailAuditFlags // TT#488 - MD - Jellis - Group ALlocation
        internal uint AllDetailAuditFlags     // TT#488 - MD - Jellis - Group Allocation
		{
			get 
			{
				return _storeBase.AllDetailAuditFlags;
			}
			set
			{
				_storeBase.AllDetailAuditFlags = value;
			}
		}

		/// <summary>
		/// Gets or sets the chosen rule.
		/// </summary>
		internal eRuleType ChosenRuleType
		{
			get
			{
				return _chosenRuleType;
			}
			set
			{
				if (ChosenRuleType != value)
				{
					_chosenRuleType = value;
					this.IsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets IsManuallyAllocated audit flag.
		/// </summary>
		internal bool IsManuallyAllocated
		{
			get
			{
				return _storeBase.IsManuallyAllocated;
			}
			set
			{
				_storeBase.IsManuallyAllocated = value;
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
        /// <summary>
        /// Gets or sets ItemIsManuallyAllocated audit flag.
        /// </summary>
        internal bool ItemIsManuallyAllocated                     // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F 
        {
            get
            {
                return _storeBase.ItemIsManuallyAllocated;        // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F  
            }
            set
            {
                _storeBase.ItemIsManuallyAllocated = value;       // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 2

        // begin TT#1334 - Urban - Jellis - Balance to VSW Enhancement
        /// <summary>
        /// Gets or sets Store Item Qty Is Locked flag
        /// </summary>
        internal bool ItemQtyIsLocked
        {
            get
            {
                return _storeBase.ItemQtyIsLocked;
            }
            set
            {
                _storeBase.ItemQtyIsLocked = value;
            }
        }
        // end TT#1334 - Urban - Jellis - Balance to VSW Enhancement

		/// <summary>
		/// Gets or sets WasAutoAllocated audit flag.
		/// </summary>
		internal bool WasAutoAllocated
		{
			get
			{
				return _storeBase.WasAutoAllocated;
			}
			set
			{
				_storeBase.WasAutoAllocated = value;
			}
		}

		/// <summary>
		/// Gets or sets ChosenRuleAcceptedByHeader audit flag.
		/// </summary>
		internal bool ChosenRuleAcceptedByHeader // TT#488 - MD - Jellis - Group Allocation
		{
			get
			{
				return _storeBase.ChosenRuleAcceptedByHeader; // TT#488 - MD - Jellis - Group Allocation
			}
			set
			{
				_storeBase.ChosenRuleAcceptedByHeader = value; // TT#488 - MD - Jellis - Group Allocation
			}
		}

        // Begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets ChosenRuleAcceptedByGroup audit flag.
        /// </summary>
        internal bool ChosenRuleAcceptedByGroup
        {
            get
            {
                return _storeBase.ChosenRuleAcceptedByGroup;
            }
            set
            {
                _storeBase.ChosenRuleAcceptedByGroup = value; 
            }
        }
        // End TT#488 - MD - Jellis - Group Allocation

		/// <summary>
		/// Gets or sets Out audit flag.
		/// </summary>
		internal bool Out
		{
			get
			{
				return _storeBase.Out;
			}
			set
			{
				_storeBase.Out = value;
			}
		}

        // begin TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
        ///// <summary>
        ///// Gets or sets Locked audit flag.
        ///// </summary>
        //internal bool Locked
        //{
        //    get
        //    {
        //        return _storeBase.Locked;
        //    }
        //    set
        //    {
        //        _storeBase.Locked = value;
        //    }
        //}
        // end TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2

		// begin TT#59 Implement Temp Locks
        // /// <summary>
		// /// Gets or sets TempLock audit flag.
		// /// </summary>
		//internal bool TempLock
		//{
		//	get
		//	{
		//		return _storeBase.TempLock;
		//	}
		//	set
		//	{
		//		_storeBase.TempLock = value;
		//	}
		//}
        // end TT#59 Implement Temp Locks

		/// <summary>
		/// Gets or sets HadNeed audit flag.
		/// </summary>
		internal bool HadNeed
		{
			get
			{
				return _storeBase.HadNeed;
			}
			set
			{
				_storeBase.HadNeed = value;
			}
		}

		/// <summary>
		/// Gets or sets FilledSizeHole audit flag.
		/// </summary>
		internal bool FilledSizeHole
		{
			get
			{
				return _storeBase.FilledSizeHole;
			}
			set
			{
				_storeBase.FilledSizeHole = value;
			}
		}

		/// <summary>
		/// Gets or sets Rule Allocation From Parent Component flag
		/// </summary>
		internal bool RuleAllocationFromParentComponent
		{
			get
			{
				return _storeBase.RuleAllocationFromParentComponent;
			}
			set
			{
				if (RuleAllocationFromParentComponent != value)
				{
					_storeBase.RuleAllocationFromParentComponent = value;
					this.IsChanged = true;
				}
			}
		}

        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets Rule Allocation From Group Component flag
        /// </summary>
        internal bool RuleAllocationFromGroupComponent
        {
            get
            {
                return _storeBase.RuleAllocationFromGroupComponent;
            }
            set
            {
                if (RuleAllocationFromGroupComponent != value)
                {
                    _storeBase.RuleAllocationFromGroupComponent = value;
                    this.IsChanged = true;
                }
            }
        }
        // end TT#488 - MD - Jellis - Group Allocation

		/// <summary>
		/// Gets or Sets Rule Allocation From Child Component Flag
		/// </summary>
		internal bool RuleAllocationFromChildComponent
		{
			get
			{
				return _storeBase.RuleAllocationFromChildComponent;
			}
			set
			{
				if (RuleAllocationFromChildComponent != value)
				{
					_storeBase.RuleAllocationFromChildComponent = value;
					this.IsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets Rule Allocation From Chosen Rule Flag.
		/// </summary>
		internal bool RuleAllocationFromChosenRule
		{
			get
			{
				return _storeBase.RuleAllocationFromChosenRule;
			}
			set
			{
				if (RuleAllocationFromChosenRule != value)
				{
					_storeBase.RuleAllocationFromChosenRule = value;
					this.IsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets Allocation From BottomUpSize Flag.
		/// </summary>
		internal bool AllocationFromBottomUpSize
		{
			get
			{
				return _storeBase.AllocationFromBottomUpSize;
			}
			set
			{
				if (AllocationFromBottomUpSize != value)
				{
					_storeBase.AllocationFromBottomUpSize = value;
					this.IsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets Allocation From BulkSizeBreakOut Flag.
		/// </summary>
		internal bool AllocationFromBulkSizeBreakOut
		{
			get
			{
				return _storeBase.AllocationFromBulkSizeBreakOut;
			}
			set
			{
				if (AllocationFromBulkSizeBreakOut != value)
				{
					_storeBase.AllocationFromBulkSizeBreakOut = value;
					this.IsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets Allocation From PackContentBreakOut Flag.
		/// </summary>
		internal bool AllocationFromPackContentBreakOut
		{
			get
			{
				return _storeBase.AllocationFromPackContentBreakOut;
			}
			set
			{
				if (AllocationFromPackContentBreakOut != value)
				{
					_storeBase.AllocationFromPackContentBreakOut = value;
					this.IsChanged = true;
				}
			}
		}

		// begin MID Track 4448 AnF Audit Enhancement
	    /// <summary>
	    /// Gets or Sets Allocation Modified After Multi Header Split Audit Flag
	    /// </summary>
		internal bool AllocationModifiedAfterMultiHeaderSplit
		{
			get
			{
				return _storeBase.AllocationModifiedAfterMultiHeaderSplit;
			}
			set
			{
				if (AllocationModifiedAfterMultiHeaderSplit != value)
				{
					_storeBase.AllocationModifiedAfterMultiHeaderSplit = value;
					this.IsChanged = true;
				}
			}
		}
		// end MID Track 4448 AnF Audit Enhancement

		/// <summary>
		/// Gets or sets QtyAllocated to store.
		/// </summary>
		internal int QtyAllocated
		{
			get
			{
				return _storeBase.QtyAllocated;
			}
			set
			{
				_storeBase.QtyAllocated = value;
			}
		}

		/// <summary>
		/// Gets or sets OriginalQtyAllocated to store.
		/// </summary>
		internal int OriginalQtyAllocated
		{
			get
			{
				return _storeBase.OriginalQtyAllocated;
			}
			set
			{
				_storeBase.OriginalQtyAllocated = value;
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets or sets ImoQtyAllocated to store.
        /// </summary>
        internal int ImoQtyAllocated
        {
            get
            {
                return _storeBase.ImoQtyAllocated;
            }
            // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
            //set
            //{
            //    _storeBase.ImoQtyAllocated = value;
            //}
            // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        }

        /// <summary>
        /// Gets or sets ItemQtyAllocated to store.
        /// </summary>
        internal int ItemQtyAllocated
        {
            get
            {
                return _storeBase.ItemQtyAllocated;
            }
            set
            {
                _storeBase.ItemQtyAllocated = value;
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1

		/// <summary>
		/// Gets or sets store allocation maximum.
		/// </summary>
		internal int Maximum
		{
			get
			{
				return _storeBase.Maximum;
			}
			set
			{
				_storeBase.Maximum = value;
			}
		}

        // Begin TT#617 - RMatelic - Allocation Override - Add Ship Up To Rule (#36, #39)
        /// <summary>
        /// Gets or sets store allocation Ship Up To.
        /// </summary>
        internal int ShipUpTo
        {
            get
            {
                return _storeBase.ShipUpTo;
            }
            set
            {
                _storeBase.ShipUpTo = value;
            }
        }
        // End TT#617  

		internal int LargestMaximum
		{
			get
			{
				return _storeBase.LargestMaximum;
			}
		}

		/// <summary>
		/// Gets or sets store allocation PrimaryMaximum
		/// </summary>
		internal int PrimaryMaximum
		{
			get
			{
				return _storeBase.PrimaryMaximum;
			}
			set
			{
				_storeBase.PrimaryMaximum = value;
			}
		}

		/// <summary>
		/// Gets or sets store allocation minimum.
		/// </summary>
		internal int Minimum
		{
			get
			{
				return _storeBase.Minimum;
			}
			set
			{
				_storeBase.Minimum = value;
			}
		}
		
		/// <summary>
		/// Gets or sets QtyAllocatedByAuto.
		/// </summary>
		internal int QtyAllocatedByAuto
		{
			get
			{
				return _storeBase.QtyAllocatedByAuto;
			}
			set
			{
				_storeBase.QtyAllocatedByAuto = value;
			}
		}
 
		/// <summary>
		/// Gets or sets QtyAllocatedByRule.
		/// </summary>
		internal int QtyAllocatedByRule
		{
			get
			{
				return _qtyAllocatedByRule;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_RuleAllocationCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_RuleAllocationCannotBeNeg));
				}
				else
				{
					if (QtyAllocatedByRule != value)
					{
						_qtyAllocatedByRule = value;
						this.IsChanged = true;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets ChosenRuleLayerID.
		/// </summary>
		internal int ChosenRuleLayerID
		{
			get
			{
				return _chosenRuleLayerID;
			}
			set
			{
				if (ChosenRuleLayerID != value)
				{
					_chosenRuleLayerID = value;
					this.IsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets ChosenRuleQtyAllocated.
		/// </summary>
		internal int ChosenRuleQtyAllocated
		{
			get
			{
				return _chosenRuleQtyAllocated;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_RuleAllocationCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_RuleAllocationCannotBeNeg));
				}
				else
				{
					if (ChosenRuleQtyAllocated != value)
					{
						_chosenRuleQtyAllocated = value;
						this.IsChanged = true;
					}
				}
			}
		}

		//========
		// METHODS
		//========
	}
	#endregion Store Allocated Bin

	// MID BEGIN Change j.ellis Allow Soft Rule in Velocity Interactive
	#region Store Allocated Hold Bin
	/// <summary>
	/// Store allocation bin for holding a pre-existing allocation.
	/// </summary>
	internal struct StoreAllocatedHoldBin
	{
		//=======
		// FIELDS
		//=======
		private AllocationStoreDetailAuditFlags _storeDetailAudit;
		private int _qtyAllocated;
		private int _qtyAllocatedByAuto;
		private int _qtyAllocatedByRule;
		private int _qtyAllocatedManually;
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        private int _itemQtyAllocated;
        private int _itemQtyManuallyAllocated;
        private int _imoQtyAllocated;
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1
		private int _chosenRuleLayerID;
		private eRuleType _chosenRuleType;
		private int _chosenRuleQtyAllocated;

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========
        //internal ushort AllDetailAuditFlags  // TT#488 - MD - Jellis - Group ALlocation
        internal uint AllDetailAuditFlags      // TT#488 - MD - Jellis - Group Allocation
		{
			get
			{
				return _storeDetailAudit.AllFlags;
			}
			set
			{
				_storeDetailAudit.AllFlags = value;
			}
		}
		/// <summary>
		/// Gets or sets IsManuallyAllocated audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the user manually allocated the units to this store.</remarks>
		internal bool IsManuallyAllocated
		{
			get
			{
				return _storeDetailAudit.IsManuallyAllocated;
			}
			set
			{
				if (IsManuallyAllocated != value)
				{
					_storeDetailAudit.IsManuallyAllocated = value;
				}
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
        /// <summary>
        /// Gets or sets ItemIsManuallyAllocated audit flag.
        /// </summary>
        /// <remarks>
        /// True indicates the user manually allocated the units to this store item.</remarks>
        internal bool ItemIsManuallyAllocated  // TT#1401 - JEllis - Urban Virtual Store warehouse pt 28F
        {
            get
            {
                return _storeDetailAudit.ItemIsManuallyAllocated;  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
            }
            set
            {
                if (ItemIsManuallyAllocated != value)  // TT#1401 - JEllis - Urban Virtual Store warehouse pt 28F
                {
                    _storeDetailAudit.ItemIsManuallyAllocated = value;   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F      
                }
            }
        }
        // end TT#1401 - JEllis - Urban Reservation STores pt 2

		/// <summary>
		/// Gets or sets WasAutoAllocated audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the system allocated units to the store (when IsManuallyAllocated
		/// is also true, the auto allocated units were overridden by the user.
		/// </remarks>
		internal bool WasAutoAllocated
		{
			get
			{
				return _storeDetailAudit.WasAutoAllocated;
			}
			set
			{
				if (WasAutoAllocated != value)
				{
					_storeDetailAudit.WasAutoAllocated = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets ChosenRuleAcceptedByHeader audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the Chosen Rule was accepted on header.
		/// </remarks>
        internal bool ChosenRuleAcceptedByHeader // TT#488 - MD - Jellis - Group Allocation
		{
			get
			{
                return _storeDetailAudit.ChosenRuleAcceptedByHeader; // TT#488 - MD - Jellis - Group Allocation
			}
			set
			{
                if (_storeDetailAudit.ChosenRuleAcceptedByHeader != value) // TT#488 - MD - Jellis - Group Allocation
				{
                    _storeDetailAudit.ChosenRuleAcceptedByHeader = value; // TT#488 - MD - Jellis - Group Allocation
				}
			}
		}

        // Begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets ChosenRuleAcceptedByGroup audit flag.
        /// </summary>
        /// <remarks>
        /// True indicates the Chosen Rule was accepted.
        /// </remarks>
        internal bool ChosenRuleAcceptedByGroup
        {
            get
            {
                return _storeDetailAudit.ChosenRuleAcceptedByGroup; 
            }
            set
            {
                if (_storeDetailAudit.ChosenRuleAcceptedByGroup != value) 
                {
                    _storeDetailAudit.ChosenRuleAcceptedByGroup = value; 
                }
            }
        }
        // end TT#488 - MD - Jellis - Group Allocation

		/// <summary>
		/// Gets or sets Out audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates a chosen rule has "outted" this store from allocation. When 
		/// IsManuallyAllocated is true, the user has overridden the out rule with an allocation.
		/// </remarks>
		internal bool Out
		{
			get
			{
				return _storeDetailAudit.Out;
			}
			set
			{
				if (Out != value)
				{
					_storeDetailAudit.Out = value;
				}
			}
		}

        // begin TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
        ///// <summary>
        ///// Gets or sets the Lock audit flag.
        ///// </summary>
        ///// <remarks>
        ///// True prevents the system from changing the allocation.  This lock is saved on the
        ///// database.
        ///// </remarks>
        //internal bool Locked
        //{
        //    get
        //    {
        //        return _storeDetailAudit.Locked;
        //    }
        //    set
        //    {
        //        if (Locked != value)
        //        {
        //            _storeDetailAudit.Locked = value;
        //        }
        //    }
        //}
        // end TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2

        // begin TT#59 Implement Temp Locks
		// /// <summary>
		// /// Gets or sets the TempLock audit flag.
		// /// </summary>
		// /// <remarks>
		// /// True prevents the system from changing the allocation.  This lock is not saved
		// /// on the database. It is used to temporarily hold an allocation value during
		// /// an allocation process.
		// /// </remarks>
		//internal bool TempLock
		//{
		//	get
		//	{
		//		return _storeDetailAudit.TempLock;
		//	}
		//	set
		//	{
		//		_storeDetailAudit.TempLock = value;
		//	}
		//}
        // end TT#59 Implement Temp Locks

		/// <summary>
		/// Gets or sets the HadNeed audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates that units were allocated based on need.
		/// </remarks>
		internal bool HadNeed
		{
			get 
			{
				return _storeDetailAudit.HadNeed;
			}
			set
			{
				if (HadNeed != value)
				{
					_storeDetailAudit.HadNeed = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the FilledSizeHole audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates that units were allocated to fill in size holes.
		/// </remarks>
		internal bool FilledSizeHole
		{
			get
			{
				return _storeDetailAudit.FilledSizeHole;
			}
			set
			{
				if (FilledSizeHole != value)
				{
					_storeDetailAudit.FilledSizeHole = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets Rule Allocation from Parent Component flag
		/// </summary>
		internal bool RuleAllocationFromParentComponent
		{
			get
			{
				return _storeDetailAudit.RuleAllocationFromParentComponent;
			}
			set
			{
				if (RuleAllocationFromParentComponent != value)
				{
					_storeDetailAudit.RuleAllocationFromParentComponent = value;
				}
			}
		}

        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets Rule Allocation from Group Component flag
        /// </summary>
        internal bool RuleAllocationFromGroupComponent
        {
            get
            {
                return _storeDetailAudit.RuleAllocationFromGroupComponent;
            }
            set
            {
                if (RuleAllocationFromGroupComponent != value)
                {
                    _storeDetailAudit.RuleAllocationFromGroupComponent = value;
                }
            }
        }

        // end TT#488 - MD - Jellis - Group Allocation

		/// <summary>
		/// Gets or Sets Rule Allocation From Child Component Flag
		/// </summary>
		internal bool RuleAllocationFromChildComponent
		{
			get
			{
				return _storeDetailAudit.RuleAllocationFromChildComponent;
			}
			set
			{
				if (RuleAllocationFromChildComponent != value)
				{
					_storeDetailAudit.RuleAllocationFromChildComponent = value;
				}
			}
		}

		/// <summary>
		/// Gets or Sets Rule Allocation From Chosen Rule Flag
		/// </summary>
		internal bool RuleAllocationFromChosenRule
		{
			get
			{
				return _storeDetailAudit.RuleAllocationFromChosenRule;
			}
			set
			{
				if (RuleAllocationFromChosenRule != value)
				{
					_storeDetailAudit.RuleAllocationFromChosenRule = value;
				}
			}
		}

		/// <summary>
		/// Gets or Sets Allocation From BottomUpSize Flag
		/// </summary>
		internal bool AllocationFromBottomUpSize
		{
			get
			{
				return _storeDetailAudit.AllocationFromBottomUpSize;
			}
			set
			{
				if (AllocationFromBottomUpSize != value)
				{
					_storeDetailAudit.AllocationFromBottomUpSize = value;
				}
			}
		}

		/// <summary>
		/// Gets or Sets Allocation From Bulk Size Break Out Flag
		/// </summary>
		internal bool AllocationFromBulkSizeBreakOut
		{
			get
			{
				return _storeDetailAudit.AllocationFromBulkSizeBreakOut;
			}
			set
			{
				if (AllocationFromBulkSizeBreakOut != value)
				{
					_storeDetailAudit.AllocationFromBulkSizeBreakOut = value;
				}
			}
		}

		/// <summary>
		/// Gets or Sets Allocation From PackContent Break Out Flag
		/// </summary>
		internal bool AllocationFromPackContentBreakOut
		{
			get
			{
				return _storeDetailAudit.AllocationFromPackContentBreakOut;
			}
			set
			{
				if (AllocationFromPackContentBreakOut != value)
				{
					_storeDetailAudit.AllocationFromPackContentBreakOut = value;
				}
			}
		}


		/// <summary>
		/// Gets or sets QtyAllocated to store.
		/// </summary>
		/// <remarks>
		/// QtyAllocated must be a non-negative integer.
		/// </remarks>
		internal int QtyAllocated
		{
			get
			{
				return _qtyAllocated;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                        + ": QtyAllocated in " + GetType().Name);     // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                }
				else
				{
					if (QtyAllocated != value)
					{
						_qtyAllocated = value;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets QtyAllocatedManually to store.
		/// </summary>
		internal int QtyAllocatedManually
		{
			get
			{
				return _qtyAllocatedManually;
			}
			set
			{
                _qtyAllocatedManually = value;	
			}
		}

		/// <summary>
		/// Gets or sets QtyAllocatedByAuto.
		/// </summary>
		internal int QtyAllocatedByAuto
		{
			get
			{
				return _qtyAllocatedByAuto;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_AutoAllocationCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_AutoAllocationCannotBeNeg));
				}
				else
				{
					if (QtyAllocatedByAuto != value)
					{
						_qtyAllocatedByAuto = value;
					}
				}
			}
		}
        
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets or sets ItemQtyAllocated to store.
        /// </summary>
        /// <remarks>
        /// ItemQtyAllocated must be a non-negative integer.
        /// </remarks>
        internal int ItemQtyAllocated
        {
            get
            {
                return _itemQtyAllocated;
            }
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                        + ": ItemQtyAllocated in " + GetType().Name);     // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                }
                else
                {
                    if (ItemQtyAllocated != value)
                    {
                        _itemQtyAllocated = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets ItemQtyManuallyAllocated to store.
        /// </summary>
        internal int ItemQtyManuallyAllocated
        {
            get
            {
                return _itemQtyManuallyAllocated;
            }
            set
            {
                // begin TT#3478 - MD - Jellis - Gabes Velocity Manually Allocated does not hold (pt 2)
                //// begin TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                //if (value < 0)
                //{
                //    throw new MIDException(eErrorLevel.severe,
                //        (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                //        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                //        + ": ItemQtyManuallyAllocated in " + GetType().Name);     // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                //}
                //// end TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                // end TT#3478 - MD - Jellis - Gabes Velocity Manually Allocated does not hold (pt 2)
                _itemQtyManuallyAllocated = value;
            }
        }

        /// <summary>
        /// Gets or sets ImoQtyAllocated to store.
        /// </summary>
        /// <remarks>
        /// ImoQtyAllocated must be a non-negative integer.
        /// </remarks>
        internal int ImoQtyAllocated
        {
            // begin TT#1008 - MD - Jellis - Get Non Negative Message for various variables
            get
            {
                int imoQtyAllocated =
                    QtyAllocated
                    - ItemQtyAllocated;
                if (imoQtyAllocated < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg) 
                        + ": ImoQtyAllocated in " + GetType().Name);           
                }
                return imoQtyAllocated;
            }
            //get
            //{
            //    return _imoQtyAllocated;
            //}
            //set
            //{
            //    if (value < 0)
            //    {
            //        throw new MIDException(eErrorLevel.severe,
            //            (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
            //            MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
            //            + ": ImoQtyAllocated in " + GetType().Name);                // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
            //    }
            //    else
            //    {
            //        if (ImoQtyAllocated != value)
            //        {
            //            _imoQtyAllocated = value;
            //        }
            //    }
            //}
            // end TT#1008 - MD - Jellis - Get Non Negative Message for various variables
        }
        //end TT#1401 - JEllis - Urban Reservation Stores pt 1

		/// <summary>
		/// Gets or sets the chosen rule.
		/// </summary>
		internal eRuleType ChosenRuleType
		{
			get
			{
				return _chosenRuleType;
			}
			set
			{
				if (ChosenRuleType != value)
				{
					_chosenRuleType = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets QtyAllocatedByRule.
		/// </summary>
		internal int QtyAllocatedByRule
		{
			get
			{
				return _qtyAllocatedByRule;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_RuleAllocationCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_RuleAllocationCannotBeNeg));
				}
				else
				{
					if (QtyAllocatedByRule != value)
					{
						_qtyAllocatedByRule = value;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets ChosenRuleLayerID.
		/// </summary>
		internal int ChosenRuleLayerID
		{
			get
			{
				return _chosenRuleLayerID;
			}
			set
			{
				if (ChosenRuleLayerID != value)
				{
					_chosenRuleLayerID = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets ChosenRuleQtyAllocated.
		/// </summary>
		internal int ChosenRuleQtyAllocated
		{
			get
			{
				return _chosenRuleQtyAllocated;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_RuleAllocationCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_RuleAllocationCannotBeNeg));
				}
				else
				{
					if (ChosenRuleQtyAllocated != value)
					{
						_chosenRuleQtyAllocated = value;
					}
				}
			}
		}
		//========
		// METHODS
		//========
	}
	#endregion Store Allocated Hold Bin
	// MID END Change j.ellis

	#region Store Total Allocated
	/// <summary>
	/// Fields to track and audit a store's various total allocations.
	/// </summary>
	internal struct StoreTotalAllocated
	{
		//=======
		// FIELDS
		//=======
		private StoreAllocatedBin _storeTotal;
        private int _storeImoAloctnMax;            // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 17
        private int _storeImoMaxValue;             // TT#1401 - JEllis - Urban Reservation Stores pt 1
        private int _storeImoMinShipQty;           // TT#1401 - JEllis - Urban Reservation Stores pt 1
        private double _storeImoPackThreshold;     // TT#1401 - JEllis - Urban Reservation Stores pt 1 // TT#1401 - JEllis - Urban Reservation Stores pt 5
		private int _storeGradeIdx;
		private int _storeTotalCapacity;
		private int _storeUsedCapacity;
		private int _storeCapacityMaximum;
		private double _storeCapacityExceedByPct;
		private StoreAllocatedBin _storeGenericTypeTotal;  
		private int _storeTotalGenericUnitsAllocated;
        private int _storeTotalGenericItemUnitsAllocated; // TT#1401 - JEllis - Urban Reservation Stores pt 1
        private int _storeTotalGenericItemUnitsManuallyAllocated; // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        private int _storeTotalGenericImoUnitsAllocated;  // TT#1401 - JEllis - Urban Reservation Stores pt 1
		private StoreAllocatedBin _storeDetailTypeTotal;
		private int _storeTotalNonGenericUnitsAllocated;
        private int _storeTotalNonGenericItemUnitsAllocated; // TT#1401 - JEllis - Urban Reservation Stores pt 1
        private int _storeTotalNonGenericItemUnitsManuallyAllocated; // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        private int _storeTotalNonGenericImoUnitsAllocated;  // TT#1401 - JEllis - Urban Reservation Stores pt 1
        private StoreAllocatedBin _storeBulkTotal;
		private int _storeBulkColorTotalUnitsAllocated;
        private int _storeBulkColorTotalItemUnitsAllocated; // TT#1401 - JEllis - Urban Reservation Stores pt 1
        private int _storeBulkColorTotalItemUnitsManuallyAllocated; // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        private int _storeBulkColorTotalImoUnitsAllocated;  // TT#1401 - JEllis - Urban Reservation Stores pt 1
        private int _storeBulkSizeTotalUnitsAllocated;    // TT#1021 - MD - Jellis - Header Status Wrong
        private DateTime _shipDay;                        // changed by DAT
        private DateTime _DB_ShipDay;                     // TT#5502 - JSmith - Can't get intransit to relieve
//		private DateTime _lastNeedDay;                    // changed by DAT
//		private int _unitNeedBefore;                      // Based on _shipDay
//		private double _percentNeedBefore;                // Based on _shipDay
		private AllocationStoreGeneralAuditFlags _storeGeneralAudit;
		private ShippingStatusFlags _storeShipStatus;
		private int _unitsShipped;
		private bool _storeTotalStyleAloctnManuallyChngd;   // MID Track 4448 ANF Audit Enhancement
		private bool _storeSizeAloctnManuallyChngd;         // MID Track 4448 ANF Audit Enhancement
		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets or sets IsChanged database update flag
		/// </summary>
		internal bool StoreTotalIsChanged
		{
			get
			{
				return _storeTotal.IsChanged;
			}
			set
			{
				_storeTotal.IsChanged = value;
			}
		}

		/// <summary>
		/// Gets or sets IsNew database update flag
		/// </summary>
		internal bool StoreTotalIsNew
		{
			get 
			{
				return _storeTotal.IsNew;
			}
			set
			{
				_storeTotal.IsNew = value;
			}
		}

		/// <summary>
		/// Gets or sets all store total detail audit flags simultaneously.
		/// </summary>
        //internal ushort StoreTotalAllDetailAuditFlags  // TT#488 - MD - Jellis - Group ALlocation
        internal uint StoreTotalAllDetailAuditFlags  // TT#488 - MD - Jellis - Group ALlocation
		{
			get
			{
				return _storeTotal.AllDetailAuditFlags;
			}
			set
			{
				_storeTotal.AllDetailAuditFlags = value;
			}
		}

		/// <summary>
		/// Gets or sets StoreTotalIsManuallyAllocated audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates that the user has manually specified the Total unit allocated
		/// </remarks>
		internal bool StoreTotalIsManuallyAllocated 
		{
			get
			{
				return _storeTotal.IsManuallyAllocated;
			}
			set
			{
				_storeTotal.IsManuallyAllocated = value;
			}
		}
        
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
        /// <summary>
        /// Gets or sets StoreTotalItemIsManuallyAllocated audit flag.
        /// </summary>
        /// <remarks>
        /// True indicates that the user has manually specified the Total Item unit allocated
        /// </remarks>
        internal bool StoreTotalItemIsManuallyAllocated  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        {
            get
            {
                return _storeTotal.ItemIsManuallyAllocated; // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
            }
            set
            {
                _storeTotal.ItemIsManuallyAllocated = value; // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 2

        // begin TT#1334 - Urban - Jellis - Balance to VSW Enhancement
        internal bool StoreItemQtyIsLocked
        {
            get
            {
                return _storeTotal.ItemQtyIsLocked; 
            }
            set
            {
                _storeTotal.ItemQtyIsLocked = value;
            }
        } 
        // end TT#1334 - Urban - Jellis - Balance to VSW Enhancement

		/// <summary>
		/// Gets or sets StoreTotalWasAutoAllocated audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates that an auto allocation process affected the store's total allocation.
		/// </remarks>
		internal bool StoreTotalWasAutoAllocated
		{
			get
			{
				return _storeTotal.WasAutoAllocated;
			}
			set
			{
				_storeTotal.WasAutoAllocated = value;
			}
		}
		
		/// <summary>
		/// Gets or sets StoreTotalChosenRuleAcceptedHeader audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the chosen rule for the store's Total component was accepted on Header.
		/// </remarks>
		internal bool StoreTotalChosenRuleAcceptedByHeader // TT#488 - MD - Jellis - Group Allocation
		{
			get
			{
				return _storeTotal.ChosenRuleAcceptedByHeader; // TT#488 - MD - Jellis - Group Allocation
			}
			set
			{
				_storeTotal.ChosenRuleAcceptedByHeader = value; // TT#488 - MD - Jellis - Group Allocation
			}
		}
	
        // Begin TT#488  - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets StoreTotalChosenRuleAcceptedGroup audit flag.
        /// </summary>
        /// <remarks>
        /// True indicates the chosen rule for the store's Total component was accepted in Group Allocation.
        /// </remarks>
        internal bool StoreTotalChosenRuleAcceptedByGroup
        {
            get
            {
                return _storeTotal.ChosenRuleAcceptedByGroup; 
            }
            set
            {
                _storeTotal.ChosenRuleAcceptedByGroup = value; 
            }
        }
        // end TT#488 - MD - Jellis - Group Allocation

		/// <summary>
		/// Gets or sets StoreTotalOut audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the store cannot be given any allocation on the Total Component.
		/// </remarks>
		internal bool StoreTotalOut
		{
			get
			{
				return _storeTotal.Out;
			}
			set
			{
				_storeTotal.Out = value;
			}
		}

        // begin TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
        ///// <summary>
        ///// Gets or sets StoreTotalLocked audit flag. 
        ///// </summary>
        ///// <remarks>
        ///// True prevents the store's total allocation from changing via an auto allocation
        ///// process.  This lock is permanent and saved on the database.
        ///// </remarks>
        //internal bool StoreTotalLocked
        //{
        //    get
        //    {
        //        return _storeTotal.Locked;
        //    }
        //    set
        //    {
        //        _storeTotal.Locked = value;
        //    }
        //}
        // end TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
		
        // begin TT#59 Implement Temp Locks
		// /// <summary>
		// /// Gets or sets StoreTotalTempLock audit flag.
		// /// </summary>
		// /// <remarks>
		// /// True prevents the store's total allocation from changing via an auto allocation.
		// /// </remarks>
		//internal bool StoreTotalTempLock
		//{
		//	get 
		//	{
		//		return _storeTotal.TempLock;
		//	}
		//	set
		//	{
		//		_storeTotal.TempLock = value;
		//	}
		//}
        // end TT#59 Implement Temp Locks

		/// <summary>
		/// Gets or sets StoreTotalHadNeed audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the store's allocation was based on need.</remarks>
		internal bool StoreTotalHadNeed
		{
			get
			{
				return _storeTotal.HadNeed;
			}
			set
			{
				_storeTotal.HadNeed = value;
			}
		}

		/// <summary>
		/// Gets or sets StoreTotalFilledSizeHole audit flag.
		/// </summary>
		/// <return>
		/// True indicates units were allocated to fill a size need for some color and size.
		/// </return>
		internal bool StoreTotalFilledSizeHole
		{
			get
			{
				return _storeTotal.FilledSizeHole;
			}
			set
			{
				_storeTotal.FilledSizeHole = value;
			}
		}

		/// <summary>
		/// Gets or sets store total rule allocation from parent flag
		/// </summary>
		internal bool StoreTotalRuleAllocationFromParentComponent
		{
			get
			{
				return _storeTotal.RuleAllocationFromParentComponent;
			}
			set
			{
				_storeTotal.RuleAllocationFromParentComponent = value;
			}
		}

        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets store total rule allocation from group flag
        /// </summary>
        internal bool StoreTotalRuleAllocationFromGroupComponent
        {
            get
            {
                return _storeTotal.RuleAllocationFromGroupComponent;
            }
            set
            {
                _storeTotal.RuleAllocationFromGroupComponent = value;
            }
        }

        // end TT#488- MD - Jellis - Group Allocation

		/// <summary>
		/// Gets or sets store total rule allocation from child flag
		/// </summary>
		internal bool StoreTotalRuleAllocationFromChildComponent
		{
			get
			{
				return _storeTotal.RuleAllocationFromChildComponent;
			}
			set
			{
				_storeTotal.RuleAllocationFromChildComponent = value;
			}
		}

		/// <summary>
		/// Gets or sets store total rule allocation from chosen rule flag
		/// </summary>
		internal bool StoreTotalRuleAllocationFromChosenRule
		{
			get
			{
				return _storeTotal.RuleAllocationFromChosenRule;
			}
			set
			{
				_storeTotal.RuleAllocationFromChosenRule = value;
			}
		}

		/// <summary>
		/// Gets or sets store total allocation from Bottom Up Size 
		/// </summary>
		internal bool StoreTotalAllocationFromBottomUpSize
		{
			get
			{
				return _storeTotal.AllocationFromBottomUpSize;
			}
			set
			{
				_storeTotal.AllocationFromBottomUpSize = value;
			}
		}

		/// <summary>
		/// Gets or sets store total allocation from Bulk Size Break Out 
		/// </summary>
		internal bool StoreTotalAllocationFromBulkSizeBreakOut
		{
			get
			{
				return _storeTotal.AllocationFromBulkSizeBreakOut;
			}
			set
			{
				_storeTotal.AllocationFromBulkSizeBreakOut = value;
			}
		}

		/// <summary>
		/// Gets or sets store Total allocation from Pack Content Break Out
		/// </summary>
		internal bool StoreTotalAllocationFromPackContentBreakOut
		{
			get
			{
				return _storeTotal.AllocationFromPackContentBreakOut;
			}
			set
			{
				_storeTotal.AllocationFromPackContentBreakOut = value;
			}
		}

		// begin MID Track 4448 AnF Audit Enhancement
		/// <summary>
		/// Gets or sets Store Total Allocation Modified After Multi Split Audit Flag
		/// </summary>
		internal bool StoreTotalAllocationModifiedAfterMultiSplit
		{
			get
			{
				return _storeTotal.AllocationModifiedAfterMultiHeaderSplit;
			}
			set
			{
				_storeTotal.AllocationModifiedAfterMultiHeaderSplit = value;
			}
		}
		// end MID Track 4448 AnF Audit Enhancement

		/// <summary>
		/// Gets or sets the store's TotalUnitsAllocated.
		/// </summary>
		internal int StoreTotalUnitsAllocated
		{
			get
			{
				return _storeTotal.QtyAllocated;
			}
			set
			{
				_storeTotal.QtyAllocated = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's TotalOrigUnitsAllocated.
		/// </summary>
		internal int StoreTotalOrigUnitsAllocated
		{
			get
			{
				return _storeTotal.OriginalQtyAllocated;
			}
			set
			{
				_storeTotal.OriginalQtyAllocated = value;
			}
		}
		
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets or sets the store's TotalItemUnitsAllocated.
        /// </summary>
        internal int StoreTotalItemUnitsAllocated
        {
            get
            {
                return _storeTotal.ItemQtyAllocated;
            }
            set
            {
                _storeTotal.ItemQtyAllocated = value;
            }
        }

        /// <summary>
        /// Gets or sets the store's TotalImoUnitsAllocated.
        /// </summary>
        internal int StoreTotalImoUnitsAllocated
        {
            get
            {
                return _storeTotal.ImoQtyAllocated;
            }
            // begin TT#1401 - Jellis - Urban Virtual Store Warehouse pt 29
            //set
            //{
            //    _storeTotal.ImoQtyAllocated = value;
            //}
            // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1


		/// <summary>
		/// Gets largest maximum value.
		/// </summary>
		internal int StoreLargestMaximum
		{
			get
			{
				return _storeTotal.LargestMaximum;
			}
		}

		/// <summary>
		/// Gets or sets the store's Total allocation maximum.
		/// </summary>
		internal int StoreTotalMaximum
		{
			get
			{
				return _storeTotal.Maximum;
			}
			set
			{
				_storeTotal.Maximum = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's Total allocation minimum.
		/// </summary>
		internal int StoreTotalMinimum
		{
			get
			{
				return _storeTotal.Minimum;
			}
			set
			{
				_storeTotal.Minimum = value;
			}
		}

        // Begin TT#617 - RMatelic - Allocation Override - Add Ship Up To Rule (#36, #39)
        /// <summary>
        /// Gets or sets the store's ShipUpTo amount.
        /// </summary>
        internal int StoreTotalShipUpTo
        {
            get
            {
                return _storeTotal.ShipUpTo;
            }
            set
            {
                _storeTotal.ShipUpTo = value;
            }
        }
        // End TT#617  

		/// <summary>
		/// Gets or sets the store's total primary allocation maximum.
		/// </summary>
		internal int StoreTotalPrimaryMaximum
		{
			get
			{
				return _storeTotal.PrimaryMaximum;
			}
			set
			{
				_storeTotal.PrimaryMaximum = value;
			}
		}

		/// <summary>
		/// Gets or sets store grade index value.
		/// </summary>
		internal int StoreGradeIdx
		{
			get
			{
				return _storeGradeIdx;
			}
			set
			{
				if (StoreGradeIdx != value)
				{
					_storeGradeIdx = value;
					this.StoreTotalIsChanged = true;
				}
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        internal int StoreImoMaxValue
        {
            get
            {
                return _storeImoMaxValue;
            }
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg,MIDText.GetText(eMIDTextCode.lbl_IMO_MAX_VALUE))) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                }
                _storeImoMaxValue = value;
            }
        }
        // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 17
        internal int StoreImoAloctnMax
        {
            get
            {
                return _storeImoAloctnMax;
            }
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "StoreImoAloctnMax")) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                }
                _storeImoAloctnMax = value;
            }
        }
        // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 17
        internal int StoreImoMinShipQty
        {
            get
            {
                return _storeImoMinShipQty;
            }
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, MIDText.GetText(eMIDTextCode.lbl_IMO_MIN_SHIP_QTY))) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                }
                _storeImoMinShipQty = value;
            }
        }
        internal double StoreImoPackThreshold   // TT#1401 - JEllis - Urban Reservation Stores pt 5
        {
            get
            {
                return _storeImoPackThreshold; // TT#1401 - JEllis - Urban Reservation Stores pt 5
            }
            set
            {
                if (value < 0
                    || value > 1)
                {
                    throw new MIDException(eErrorLevel.error,
                        (int)eMIDTextCode.msg_al_ValueMustBeBetween0and1,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueMustBeBetween0and1, MIDText.GetText(eMIDTextCode.lbl_IMO_PCT_PK_THRSHLD))));
                }
                _storeImoPackThreshold = value; // TT#1401 - JEllis - Urban Reservation Stores pt 5
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1

		/// <summary>
		/// Gets or sets the store's total Capacity.
		/// </summary>
		internal int StoreTotalCapacity
		{
			get
			{
				return _storeTotalCapacity;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_CapacityCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_CapacityCannotBeNeg));
				}
				// Note:  value is not saved to database; 
				_storeTotalCapacity = value;
			}
		}

	    /// <summary>
     	/// Gets or sets the store's used Capacity.
        /// </summary>
		internal int StoreUsedCapacity
		{
			get
			{
				return _storeUsedCapacity;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_al_CapacityUsedCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_al_CapacityUsedCannotBeNeg));
				}
				// Note:  value is not saved to database; 
				_storeUsedCapacity = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's total Capacity allocation maximum.
		/// </summary>
		internal int StoreCapacityMaximum
		{
			get
			{
				return _storeCapacityMaximum;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_CapacityCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_CapacityCannotBeNeg));
				}
				if (StoreCapacityMaximum != value)
				{
					_storeCapacityMaximum = value;
					this.StoreTotalIsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets the store's capacity exceed by percent.
		/// </summary>
		internal double StoreCapacityExceedByPct
		{
			get
			{
				return _storeCapacityExceedByPct;
			}
			set
			{
				if (StoreCapacityExceedByPct != value)
				{
					_storeCapacityExceedByPct = value;
					this.StoreTotalIsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets store's total units allocated via an auto allocation process.
		/// </summary>
		internal int StoreTotalUnitsAllocatedByAuto
		{
			get
			{
				return _storeTotal.QtyAllocatedByAuto;
			}
			set
			{
				_storeTotal.QtyAllocatedByAuto = value;
			}
		}

		/// <summary>
		/// Gets or sets store's total units allocated by rule.
		/// </summary>
		internal int StoreTotalUnitsAllocatedByRule
		{
			get
			{
				return _storeTotal.QtyAllocatedByRule;
			}
			set
			{
				_storeTotal.QtyAllocatedByRule = value;
			}
		}

 		/// <summary>
		/// Gets or sets the chosen rule.
		/// </summary>
		internal eRuleType StoreTotalChosenRuleType
		{
			get
			{
				return _storeTotal.ChosenRuleType;
			}
			set
			{
				_storeTotal.ChosenRuleType = value;
			}
		}

		/// <summary>
		/// Gets or sets store's total chosen rule RID.
		/// </summary>
		internal int StoreTotalChosenRuleLayerID
		{
			get
			{
				return _storeTotal.ChosenRuleLayerID;
			}
			set
			{
				_storeTotal.ChosenRuleLayerID = value;
			}
		}

		/// <summary>
		/// Gets or sets store's total units allocated by the chosen rule.
		/// </summary>
		internal int StoreTotalChosenRuleUnitsAllocated
		{
			get
			{
				return _storeTotal.ChosenRuleQtyAllocated;
			}
			set
			{
				_storeTotal.ChosenRuleQtyAllocated = value;
			}
		}


		/// <summary>
		/// Gets or sets Store Total LastNeedDay
		/// </summary>
		/// <remarks>
		/// DateTime Format
		/// </remarks>
		internal DateTime StoreTotalLastNeedDay
		{
			get
			{
				return _storeTotal.StoreLastNeedDay;
			}
			set
			{
				_storeTotal.StoreLastNeedDay = value;
			}
		}

		/// <summary>
		/// Gets or sets Store's Total Unit Need Before.
		/// </summary>
		internal int StoreTotalUnitNeedBefore
		{
			get
			{
				return _storeTotal.StoreUnitNeedBefore;
			}
			set
			{
				_storeTotal.StoreUnitNeedBefore = value;
			}
		}

		/// <summary>
		/// Gets or sets store's PercentNeedBefore
		/// </summary>
		internal double StoreTotalPercentNeedBefore
		{
			get
			{
				return _storeTotal.StorePercentNeedBefore;
			}
			set
			{
				_storeTotal.StorePercentNeedBefore = value;
			}
		}

        // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F -- calculation wrong if bulk not detail
        ///// <summary>
        ///// Gets StoreTypeUnitsAllocated.
        ///// </summary>
        ///// <remarks>
        ///// </remarks>
        //internal int StoreTypeUnitsAllocated
        //{
        //    get
        //    {
        //        return StoreGenericTypeUnitsAllocated + StoreDetailTypeUnitsAllocated;
        //    }
        //}

        ///// <summary>
        ///// Gets StoreTypeOrigUnitsAllocated.
        ///// </summary>
        ///// <remarks>
        ///// </remarks>
        //internal int StoreTypeOrigUnitsAllocated
        //{
        //    get
        //    {
        //        return StoreGenericTypeOrigUnitsAllocated 
        //            + StoreDetailTypeOrigUnitsAllocated;
        //    }
        //}
        // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F -- calculation wrong if bulk not detail

		/// <summary>
		/// Gets or sets IsChanged database update flag
		/// </summary>
		internal bool StoreGenericTypeTotalIsChanged
		{
			get
			{
				return _storeGenericTypeTotal.IsChanged;
			}
			set
			{
				_storeGenericTypeTotal.IsChanged = value;
			}
		}

		/// <summary>
		/// Gets or sets IsNew database update flag
		/// </summary>
		internal bool StoreGenericTypeTotalIsNew
		{
			get 
			{
				return _storeGenericTypeTotal.IsNew;
			}
			set
			{
				_storeGenericTypeTotal.IsNew = value;
			}
		}

		// BEGIN MID Track 4787/4788 stodd 10.22.2007
		// begin MID Track 4448 ANF Audit Enhancement
		/// <summary>
		/// Gets or sets all store Generic Type detail audit flags simultaneously.
		/// </summary>
        //internal ushort StoreGenericTypeAllDetailAuditFlags // TT#488 - MD - Jellis - Group ALlocation
        internal uint StoreGenericTypeAllDetailAuditFlags // TT#488 - MD - Jellis - Group ALlocation
		{
			get
			{
				return _storeGenericTypeTotal.AllDetailAuditFlags;
			}
			set
			{
				_storeGenericTypeTotal.AllDetailAuditFlags = value;
			}
		}
		// END MID Track 4787/4788 stodd 10.22.2007
		// end MID Track 4448 ANF Audit Enhancement

		/// <summary>
		/// Gets or sets StoreGenericTypeIsManuallyAllocated audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates that the user has manually specified the GenericType unit allocated
		/// </remarks>
		internal bool StoreGenericTypeIsManuallyAllocated 
		{
			get
			{
				return _storeGenericTypeTotal.IsManuallyAllocated;
			}
			set
			{
				_storeGenericTypeTotal.IsManuallyAllocated = value;
			}
		}
        
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
        /// <summary>
        /// Gets or sets StoreGenericTypeItemIsManuallyAllocated audit flag.
        /// </summary>
        /// <remarks>
        /// True indicates that the user has manually specified the GenericType item unit allocated
        /// </remarks>
        internal bool StoreGenericTypeItemIsManuallyAllocated
        {
            get
            {
                return _storeGenericTypeTotal.ItemIsManuallyAllocated;
            }
            set
            {
                _storeGenericTypeTotal.ItemIsManuallyAllocated = value;
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 2

		/// <summary>
		/// Gets or sets StoreGenericTypeWasAutoAllocated audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates that an auto allocation process affected the store's GenericType allocation.
		/// </remarks>
		internal bool StoreGenericTypeWasAutoAllocated
		{
			get
			{
				return _storeGenericTypeTotal.WasAutoAllocated;
			}
			set
			{
				_storeGenericTypeTotal.WasAutoAllocated = value;
			}
		}
		
		/// <summary>
		/// Gets or sets StoreGenericTypeChosenRuleAcceptedByHeader audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the chosen rule for the store's GenericType component was accepted on Header.
		/// </remarks>
        internal bool StoreGenericTypeChosenRuleAcceptedByHeader // TT#488 - MD - Jellis - Group Allocation
		{
			get
			{
                return _storeGenericTypeTotal.ChosenRuleAcceptedByHeader; // TT#488 - MD - Jellis - Group Allocation
			}
			set
			{
                _storeGenericTypeTotal.ChosenRuleAcceptedByHeader = value; // TT#488 - MD - Jellis - Group Allocation
			}
		}
	
        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets StoreGenericTypeChosenRuleAcceptedByGroup audit flag.
        /// </summary>
        /// <remarks>
        /// True indicates the chosen rule for the store's GenericType component was accepted in Group Allocation.
        /// </remarks>
        internal bool StoreGenericTypeChosenRuleAcceptedByGroup
        {
            get
            {
                return _storeGenericTypeTotal.ChosenRuleAcceptedByGroup;
            }
            set
            {
                _storeGenericTypeTotal.ChosenRuleAcceptedByGroup = value;
            }
        }
        // end TT#488 - MD - Jellis - Group Allocation

		/// <summary>
		/// Gets or sets StoreGenericTypeOut audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the store cannot be given any allocation on the GenericType Component.
		/// </remarks>
		internal bool StoreGenericTypeOut
		{
			get
			{
				return _storeGenericTypeTotal.Out;
			}
			set
			{
				_storeGenericTypeTotal.Out = value;
			}
		}

        // begin TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
        ///// <summary>
        ///// Gets or sets StoreGenericTypeLocked audit flag. 
        ///// </summary>
        ///// <remarks>
        ///// True prevents the store's GenericType allocation from changing via an auto allocation
        ///// process.  This lock is permanent and saved on the database.
        ///// </remarks>
        //internal bool StoreGenericTypeLocked
        //{
        //    get
        //    {
        //        return _storeGenericTypeTotal.Locked;
        //    }
        //    set
        //    {
        //        _storeGenericTypeTotal.Locked = value;
        //    }
        //}
        // end TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
		 
		// begin TT#59 Implement Temp Locks
        // /// <summary>
		// /// Gets or sets StoreGenericTypeTempLock audit flag.
		// /// </summary>
		// /// <remarks>
		// /// True prevents the store's GenericType allocation from changing via an auto allocation.
		// /// </remarks>
		//internal bool StoreGenericTypeTempLock
		//{
		//	get 
		//	{
		//		return _storeGenericTypeTotal.TempLock;
		//	}
		//	set
		//	{
		//		_storeGenericTypeTotal.TempLock = value;
		//	}
		//}
        // end TT#59 Implement Temp Locks

		/// <summary>
		/// Gets or sets StoreGenericTypeHadNeed audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the store's allocation was based on need.</remarks>
		internal bool StoreGenericTypeHadNeed
		{
			get
			{
				return _storeGenericTypeTotal.HadNeed;
			}
			set
			{
				_storeGenericTypeTotal.HadNeed = value;
			}
		}


		/// <summary>
		/// Gets or sets store Generic Type rule allocation from parent flag
		/// </summary>
		internal bool StoreGenericTypeRuleAllocationFromParentComponent
		{
			get
			{
				return _storeGenericTypeTotal.RuleAllocationFromParentComponent;
			}
			set
			{
				_storeGenericTypeTotal.RuleAllocationFromParentComponent = value;
			}
		}

        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets store Generic Type rule allocation from group flag
        /// </summary>
        internal bool StoreGenericTypeRuleAllocationFromGroupComponent
        {
            get
            {
                return _storeGenericTypeTotal.RuleAllocationFromGroupComponent;
            }
            set
            {
                _storeGenericTypeTotal.RuleAllocationFromGroupComponent = value;
            }
        }
        // end TT#488 - MD - Jellis - Group Allocation

		/// <summary>
		/// Gets or sets store Generic Type rule allocation from child flag
		/// </summary>
		internal bool StoreGenericTypeRuleAllocationFromChildComponent
		{
			get
			{
				return _storeGenericTypeTotal.RuleAllocationFromChildComponent;
			}
			set
			{
				_storeGenericTypeTotal.RuleAllocationFromChildComponent = value;
			}
		}

		/// <summary>
		/// Gets or sets store Generic Type rule allocation from chosen rule flag
		/// </summary>
		internal bool StoreGenericTypeRuleAllocationFromChosenRule
		{
			get
			{
				return _storeGenericTypeTotal.RuleAllocationFromChosenRule;
			}
			set
			{
				_storeGenericTypeTotal.RuleAllocationFromChosenRule = value;
			}
		}

		/// <summary>
		/// Gets or sets store Generic Type allocation from Bottom Up Size
		/// </summary>
		internal bool StoreGenericTypeAllocationFromBottomUpSize
		{
			get
			{
				return _storeGenericTypeTotal.AllocationFromBottomUpSize;
			}
			set
			{
				_storeGenericTypeTotal.AllocationFromBottomUpSize = value;
			}
		}


		/// <summary>
		/// Gets or sets store Generic Type allocation from Bulk Size Break Out
		/// </summary>
		internal bool StoreGenericTypeAllocationFromBulkSizeBreakOut
		{
			get
			{
				return _storeGenericTypeTotal.AllocationFromBulkSizeBreakOut;
			}
			set
			{
				_storeGenericTypeTotal.AllocationFromBulkSizeBreakOut = value;
			}
		}

		/// <summary>
		/// Gets or sets store Generic Type allocation from Pack Content Break Out
		/// </summary>
		internal bool StoreGenericTypeAllocationFromPackContentBreakOut
		{
			get
			{
				return _storeGenericTypeTotal.AllocationFromPackContentBreakOut;
			}
			set
			{
				_storeGenericTypeTotal.AllocationFromPackContentBreakOut = value;
			}
		}
		// begin MID Track 4448 AnF Audit Enhancement
		/// <summary>
		/// Gets or sets Store Generic Type Total Allocation Modified After Multi Split Audit Flag
		/// </summary>
		internal bool StoreGenericTypeTotalAllocationModifiedAfterMultiSplit
		{
			get
			{
				return _storeGenericTypeTotal.AllocationModifiedAfterMultiHeaderSplit;
			}
			set
			{
				_storeGenericTypeTotal.AllocationModifiedAfterMultiHeaderSplit = value;
			}
		}
		// end MID Track 4448 AnF Audit Enhancement

		/// <summary>
		/// Gets or sets the store's GenericTypeUnitsAllocated.
		/// </summary>
		internal int StoreGenericTypeUnitsAllocated
		{
			get
			{
				return _storeGenericTypeTotal.QtyAllocated;
			}
			set
			{
				_storeGenericTypeTotal.QtyAllocated = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's GenericTypeOrigUnitsAllocated.
		/// </summary>
		internal int StoreGenericTypeOrigUnitsAllocated
		{
			get
			{
				return _storeGenericTypeTotal.OriginalQtyAllocated;
			}
			set
			{
				_storeGenericTypeTotal.OriginalQtyAllocated = value;
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets or sets the store's GenericTypeItemUnitsAllocated.
        /// </summary>
        internal int StoreGenericTypeItemUnitsAllocated
        {
            get
            {
                return _storeGenericTypeTotal.ItemQtyAllocated;
            }
            set
            {
                _storeGenericTypeTotal.ItemQtyAllocated = value;
            }
        }

        internal int StoreGenericTypeItemUnitsManuallyAllocated
        {
            get
            {
                return _storeTotalGenericItemUnitsManuallyAllocated;
            }
            set
            {
                _storeTotalGenericItemUnitsManuallyAllocated = value;
            }
        }
        /// <summary>
        /// Gets or sets the store's GenericTypeImoUnitsAllocated.
        /// </summary>
        internal int StoreGenericTypeImoUnitsAllocated
        {
            get
            {
                return _storeGenericTypeTotal.ImoQtyAllocated;
            }
            // begin TT#1401 - JEllis - Urban Virtusl Store Warehouse pt 29
            //set
            //{
            //    _storeGenericTypeTotal.ImoQtyAllocated = value;
            //}
            // end TT#1401 - Jellis - Urban Virtual Store Warehouse pt 29
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1
		
		/// <summary>
		/// Gets or sets the store's GenericType allocation maximum.
		/// </summary>
		internal int StoreGenericTypeMaximum
		{
			get
			{
				return _storeGenericTypeTotal.Maximum;
			}
			set
			{
				_storeGenericTypeTotal.Maximum = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's GenericType allocation minimum.
		/// </summary>
		internal int StoreGenericTypeMinimum
		{
			get
			{
				return _storeGenericTypeTotal.Minimum;
			}
			set
			{
				_storeGenericTypeTotal.Minimum = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's GenericType primary allocation maximum.
		/// </summary>
		internal int StoreGenericTypePrimaryMaximum
		{
			get
			{
				return _storeGenericTypeTotal.PrimaryMaximum;
			}
			set
			{
				_storeGenericTypeTotal.PrimaryMaximum = value;
			}
		}

		/// <summary>
		/// Gets or sets store's GenericType units allocated via an auto allocation process.
		/// </summary>
		internal int StoreGenericTypeUnitsAllocatedByAuto
		{
			get
			{
				return _storeGenericTypeTotal.QtyAllocatedByAuto;
			}
			set
			{
				_storeGenericTypeTotal.QtyAllocatedByAuto = value;
			}
		}

		/// <summary>
		/// Gets or sets store's GenericType units allocated by rule.
		/// </summary>
		internal int StoreGenericTypeUnitsAllocatedByRule
		{
			get
			{
				return _storeGenericTypeTotal.QtyAllocatedByRule;
			}
			set
			{
				_storeGenericTypeTotal.QtyAllocatedByRule = value;
			}
		}

		/// <summary>
		/// Gets or sets the chosen rule.
		/// </summary>
		internal eRuleType StoreGenericTypeChosenRuleType
		{
			get
			{
				return _storeGenericTypeTotal.ChosenRuleType;
			}
			set
			{
				_storeGenericTypeTotal.ChosenRuleType = value;
			}
		}

		/// <summary>
		/// Gets or sets store's GenericType chosen rule Layer ID.
		/// </summary>
		internal int StoreGenericTypeChosenRuleLayerID
		{
			get
			{
				return _storeGenericTypeTotal.ChosenRuleLayerID;
			}
			set
			{
				_storeGenericTypeTotal.ChosenRuleLayerID = value;
			}
		}

		/// <summary>
		/// Gets or sets store's GenericType units allocated by the chosen rule.
		/// </summary>
		internal int StoreGenericTypeChosenRuleUnitsAllocated
		{
			get
			{
				return _storeGenericTypeTotal.ChosenRuleQtyAllocated;
			}
			set
			{
				_storeGenericTypeTotal.ChosenRuleQtyAllocated = value;
			}
		}


		/// <summary>
		/// Gets or sets Store Generic Type LastNeedDay
		/// </summary>
		/// <remarks>
		/// DateTime Format
		/// </remarks>
		internal DateTime StoreGenericTypeLastNeedDay
		{
			get
			{
				return _storeGenericTypeTotal.StoreLastNeedDay;
			}
			set
			{
				_storeGenericTypeTotal.StoreLastNeedDay = value;
			}
		}

		/// <summary>
		/// Gets or sets Store's Generic Type Unit Need Before.
		/// </summary>
		internal int StoreGenericTypeUnitNeedBefore
		{
			get
			{
				return _storeGenericTypeTotal.StoreUnitNeedBefore;
			}
			set
			{
				_storeGenericTypeTotal.StoreUnitNeedBefore = value;
			}
		}

		/// <summary>
		/// Gets or sets store's Generic Type PercentNeedBefore
		/// </summary>
		internal double StoreGenericTypePercentNeedBefore
		{
			get
			{
				return _storeGenericTypeTotal.StorePercentNeedBefore;
			}
			set
			{
				_storeGenericTypeTotal.StorePercentNeedBefore = value;
			}
		}

		/// <summary>
		/// Gets or sets IsChanged database update flag
		/// </summary>
		internal bool StoreDetailTypeTotalIsChanged
		{
			get
			{
				return _storeDetailTypeTotal.IsChanged;
			}
			set
			{
				_storeDetailTypeTotal.IsChanged = value;
			}
		}

		/// <summary>
		/// Gets or sets IsNew database update flag
		/// </summary>
		internal bool StoreDetailTypeTotalIsNew
		{
			get 
			{
				return _storeDetailTypeTotal.IsNew;
			}
			set
			{
				_storeDetailTypeTotal.IsNew = value;
			}
		}

		// begin MID Track 4448 ANF Audit Enhancement
		/// <summary>
		/// Gets or sets all store Detail Type detail audit flags simultaneously.
		/// </summary>
        //internal ushort StoreDetailTypeAllDetailAuditFlags  // TT#488 - MD - Jellis - Group Allocation
        internal uint StoreDetailTypeAllDetailAuditFlags  // TT#488 - MD - Jellis - Group Allocation
		{
			get
			{
				return _storeDetailTypeTotal.AllDetailAuditFlags;
			}
			set
			{
				_storeDetailTypeTotal.AllDetailAuditFlags = value;
			}
		}
		// end MID Track 4448 ANF Audit Enhancement

		/// <summary>
		/// Gets or sets StoreDetailTypeIsManuallyAllocated audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates that the user has manually specified the DetailType unit allocated
		/// </remarks>
		internal bool StoreDetailTypeIsManuallyAllocated 
		{
			get
			{
				return _storeDetailTypeTotal.IsManuallyAllocated;
			}
			set
			{
				_storeDetailTypeTotal.IsManuallyAllocated = value;
			}
		}
        
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
        /// <summary>
        /// Gets or sets StoreDetailTypeItemIsManuallyAllocated audit flag.
        /// </summary>
        /// <remarks>
        /// True indicates that the user has manually specified the DetailType item unit allocated
        /// </remarks>
        internal bool StoreDetailTypeItemIsManuallyAllocated
        {
            get
            {
                return _storeDetailTypeTotal.ItemIsManuallyAllocated;
            }
            set
            {
                _storeDetailTypeTotal.ItemIsManuallyAllocated = value;
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 2

		/// <summary>
		/// Gets or sets StoreDetailTypeWasAutoAllocated audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates that an auto allocation process affected the store's DetailType allocation.
		/// </remarks>
		internal bool StoreDetailTypeWasAutoAllocated
		{
			get
			{
				return _storeDetailTypeTotal.WasAutoAllocated;
			}
			set
			{
				_storeDetailTypeTotal.WasAutoAllocated = value;
			}
		}
		
		/// <summary>
		/// Gets or sets StoreDetailTypeChosenRuleAcceptedByHeader audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the chosen rule for the store's DetailType component was accepted on Header.
		/// </remarks>
		internal bool StoreDetailTypeChosenRuleAcceptedByHeader // TT#488 - MD - Jellis - Group Allocation
		{
			get
			{
				return _storeDetailTypeTotal.ChosenRuleAcceptedByHeader; // TT#488 - MD - Jellis - Group Allocation
			}
			set
			{
				_storeDetailTypeTotal.ChosenRuleAcceptedByHeader = value; // TT#488 - MD - Jellis - Group Allocation
			}
		}
	
        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets StoreDetailTypeChosenRuleAcceptedByGroup audit flag.
        /// </summary>
        /// <remarks>
        /// True indicates the chosen rule for the store's DetailType component was accepted in Group Allocation.
        /// </remarks>
        internal bool StoreDetailTypeChosenRuleAcceptedByGroup
        {
            get
            {
                return _storeDetailTypeTotal.ChosenRuleAcceptedByGroup;
            }
            set
            {
                _storeDetailTypeTotal.ChosenRuleAcceptedByGroup = value; 
            }
        }
        // end TT#488 - MD - Jellis - Group Allocation

		/// <summary>
		/// Gets or sets StoreDetailTypeOut audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the store cannot be given any allocation on the DetailType Component.
		/// </remarks>
		internal bool StoreDetailTypeOut
		{
			get
			{
				return _storeDetailTypeTotal.Out;
			}
			set
			{
				_storeDetailTypeTotal.Out = value;
			}
		}

        // begin TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
        ///// <summary>
        ///// Gets or sets StoreDetailTypeLocked audit flag. 
        ///// </summary>
        ///// <remarks>
        ///// True prevents the store's DetailType allocation from changing via an auto allocation
        ///// process.  This lock is permanent and saved on the database.
        ///// </remarks>
        //internal bool StoreDetailTypeLocked
        //{
        //    get
        //    {
        //        return _storeDetailTypeTotal.Locked;
        //    }
        //    set
        //    {
        //        _storeDetailTypeTotal.Locked = value;
        //    }
        //}
        // end TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
		 
		// begin TT#59 Implement Temp Locks
        // /// <summary>
		// /// Gets or sets StoreDetailTypeTempLock audit flag.
		// /// </summary>
		// /// <remarks>
		// /// True prevents the store's DetailType allocation from changing via an auto allocation.
		// /// </remarks>
		//internal bool StoreDetailTypeTempLock
		//{
		//	get 
		//	{
		//		return _storeDetailTypeTotal.TempLock;
		//	}
		//	set
		//	{
		//		_storeDetailTypeTotal.TempLock = value;
		//	}
		//}
        // end TT#59 Implement Temp Locks

		/// <summary>
		/// Gets or sets StoreDetailTypeHadNeed audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the store's allocation was based on need.</remarks>
		internal bool StoreDetailTypeHadNeed
		{
			get
			{
				return _storeDetailTypeTotal.HadNeed;
			}
			set
			{
				_storeDetailTypeTotal.HadNeed = value;
			}
		}

		/// <summary>
		/// Gets or sets StoreDetailTypeFilledSizeHole audit flag.
		/// </summary>
		/// <return>
		/// True indicates units were allocated to fill a size need for some color and size.
		/// </return>
		internal bool StoreDetailTypeFilledSizeHole
		{
			get
			{
				return _storeDetailTypeTotal.FilledSizeHole;
			}
			set
			{
				_storeDetailTypeTotal.FilledSizeHole = value;
			}
		}


		/// <summary>
		/// Gets or sets store DetailType Rule allocation from parent flag
		/// </summary>
		internal bool StoreDetailTypeRuleAllocationFromParentComponent
		{
			get
			{
				return _storeDetailTypeTotal.RuleAllocationFromParentComponent;
			}
			set
			{
				_storeDetailTypeTotal.RuleAllocationFromParentComponent = value;
			}
		}

        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets store DetailType Rule allocation from group flag
        /// </summary>
        internal bool StoreDetailTypeRuleAllocationFromGroupComponent
        {
            get
            {
                return _storeDetailTypeTotal.RuleAllocationFromGroupComponent;
            }
            set
            {
                _storeDetailTypeTotal.RuleAllocationFromGroupComponent = value;
            }
        }
        // end TT#488 - MD - Jellis - Group Allocation

		/// <summary>
		/// Gets or sets store DetailType Rule allocation from child flag
		/// </summary>
		internal bool StoreDetailTypeRuleAllocationFromChildComponent
		{
			get
			{
				return _storeDetailTypeTotal.RuleAllocationFromChildComponent;
			}
			set
			{
				_storeDetailTypeTotal.RuleAllocationFromChildComponent = value;
			}
		}

		/// <summary>
		/// Gets or sets store DetailType Rule allocation from chosen Rule flag
		/// </summary>
		internal bool StoreDetailTypeRuleAllocationFromChosenRule
		{
			get
			{
				return _storeDetailTypeTotal.RuleAllocationFromChosenRule;
			}
			set
			{
				_storeDetailTypeTotal.RuleAllocationFromChosenRule = value;
			}
		}

		/// <summary>
		/// Gets or sets store DetailType allocation from Bottom Up Size
		/// </summary>
		internal bool StoreDetailTypeAllocationFromBottomUpSize
		{
			get
			{
				return _storeDetailTypeTotal.AllocationFromBottomUpSize;
			}
			set
			{
				_storeDetailTypeTotal.AllocationFromBottomUpSize = value;
			}
		}

		/// <summary>
		/// Gets or sets store DetailType allocation from Pack Content Break Out
		/// </summary>
		internal bool StoreDetailTypeAllocationFromPackContentBreakOut
		{
			get
			{
				return _storeDetailTypeTotal.AllocationFromPackContentBreakOut;
			}
			set
			{
				_storeDetailTypeTotal.AllocationFromPackContentBreakOut = value;
			}
		}

		/// <summary>
		/// Gets or sets store DetailType allocation from Bulk Size Break Out
		/// </summary>
		internal bool StoreDetailTypeAllocationFromBulkSizeBreakOut
		{
			get
			{
				return _storeDetailTypeTotal.AllocationFromBulkSizeBreakOut;
			}
			set
			{
				_storeDetailTypeTotal.AllocationFromBulkSizeBreakOut = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's DetailTypeUnitsAllocated.
		/// </summary>
		internal int StoreDetailTypeUnitsAllocated
		{
			get
			{
				return _storeDetailTypeTotal.QtyAllocated;
			}
			set
			{
				_storeDetailTypeTotal.QtyAllocated = value;
			}
		}
		
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets or sets the store's DetailTypeItemUnitsAllocated.
        /// </summary>
        internal int StoreDetailTypeItemUnitsAllocated
        {
            get
            {
                return _storeDetailTypeTotal.ItemQtyAllocated;
            }
            set
            {
                _storeDetailTypeTotal.ItemQtyAllocated = value;
            }
        }

        /// <summary>
        /// Gets or sets the store's DetailTypeImoUnitsAllocated.
        /// </summary>
        internal int StoreDetailTypeImoUnitsAllocated
        {
            get
            {
                return _storeDetailTypeTotal.ImoQtyAllocated;
            }
            // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
            //set
            //{
            //    _storeDetailTypeTotal.ImoQtyAllocated = value;
            //}
            // end TT#1401 - JEllis -Urban Virtusl Store Warehouse pt 29
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1

		// begin MID Track 4448 AnF Audit Enhancement
		/// <summary>
		/// Gets or sets Store Detail Type Total Allocation Modified After Multi Split Audit Flag
		/// </summary>
		internal bool StoreDetailTypeAllocationModifiedAfterMultiSplit
		{
			get
			{
				return _storeDetailTypeTotal.AllocationModifiedAfterMultiHeaderSplit;
			}
			set
			{
				_storeDetailTypeTotal.AllocationModifiedAfterMultiHeaderSplit = value;
			}
		}
		// end MID Track 4448 AnF Audit Enhancement

		/// <summary>
		/// Gets or sets the store's DetailTypeOrigUnitsAllocated.
		/// </summary>
		internal int StoreDetailTypeOrigUnitsAllocated
		{
			get
			{
				return _storeDetailTypeTotal.OriginalQtyAllocated;
			}
			set
			{
				_storeDetailTypeTotal.OriginalQtyAllocated = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's DetailType allocation maximum.
		/// </summary>
		internal int StoreDetailTypeMaximum
		{
			get
			{
				return _storeDetailTypeTotal.Maximum;
			}
			set
			{
				_storeDetailTypeTotal.Maximum = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's DetailType allocation minimum.
		/// </summary>
		internal int StoreDetailTypeMinimum
		{
			get
			{
				return _storeDetailTypeTotal.Minimum;
			}
			set
			{
				_storeDetailTypeTotal.Minimum = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's DetailType primary allocation maximum.
		/// </summary>
		internal int StoreDetailTypePrimaryMaximum
		{
			get
			{
				return _storeDetailTypeTotal.PrimaryMaximum;
			}
			set
			{
				_storeDetailTypeTotal.PrimaryMaximum = value;
			}
		}

		/// <summary>
		/// Gets or sets store's DetailType units allocated via an auto allocation process.
		/// </summary>
		internal int StoreDetailTypeUnitsAllocatedByAuto
		{
			get
			{
				return _storeDetailTypeTotal.QtyAllocatedByAuto;
			}
			set
			{
				_storeDetailTypeTotal.QtyAllocatedByAuto = value;
			}
		}

		/// <summary>
		/// Gets or sets store's DetailType units allocated by rule.
		/// </summary>
		internal int StoreDetailTypeUnitsAllocatedByRule
		{
			get
			{
				return _storeDetailTypeTotal.QtyAllocatedByRule;
			}
			set
			{
				_storeDetailTypeTotal.QtyAllocatedByRule = value;
			}
		}

		/// <summary>
		/// Gets or sets the chosen rule.
		/// </summary>
		internal eRuleType StoreDetailTypeChosenRuleType
		{
			get
			{
				return _storeDetailTypeTotal.ChosenRuleType;
			}
			set
			{
				_storeDetailTypeTotal.ChosenRuleType = value;
			}
		}

		/// <summary>
		/// Gets or sets store's DetailType chosen rule Layer ID.
		/// </summary>
		internal int StoreDetailTypeChosenRuleLayerID
		{
			get
			{
				return _storeDetailTypeTotal.ChosenRuleLayerID;
			}
			set
			{
				_storeDetailTypeTotal.ChosenRuleLayerID = value;
			}
		}

		/// <summary>
		/// Gets or sets store's DetailType units allocated by the chosen rule.
		/// </summary>
		internal int StoreDetailTypeChosenRuleUnitsAllocated
		{
			get
			{
				return _storeDetailTypeTotal.ChosenRuleQtyAllocated;
			}
			set
			{
				_storeDetailTypeTotal.ChosenRuleQtyAllocated = value;
			}
		}


		/// <summary>
		/// Gets or sets Store Detail Type Total LastNeedDay
		/// </summary>
		/// <remarks>
		/// DateTime Format
		/// </remarks>
		internal DateTime StoreDetailTypeLastNeedDay
		{
			get
			{
				return _storeDetailTypeTotal.StoreLastNeedDay;
			}
			set
			{
				_storeDetailTypeTotal.StoreLastNeedDay = value;
			}
		}

		/// <summary>
		/// Gets or sets Store's Detail Type Total Unit Need Before.
		/// </summary>
		internal int StoreDetailTypeUnitNeedBefore
		{
			get
			{
				return _storeDetailTypeTotal.StoreUnitNeedBefore;
			}
			set
			{
				_storeDetailTypeTotal.StoreUnitNeedBefore = value;
			}
		}

		/// <summary>
		/// Gets or sets store's PercentNeedBefore
		/// </summary>
		internal double StoreDetailTypePercentNeedBefore
		{
			get
			{
				return _storeDetailTypeTotal.StorePercentNeedBefore;
			}
			set
			{
				_storeDetailTypeTotal.StorePercentNeedBefore = value;
			}
		}

		/// <summary>
		/// Gets or sets store's total generic pack units allocated.
		/// </summary>
		/// <remarks>
		/// Accumulated generic pack units allocated.
		/// </remarks>
		internal int StoreTotalGenericUnitsAllocated
		{
			get
			{
				return _storeTotalGenericUnitsAllocated;
			}
			set
			{
                // begin TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "StoreTotalGenericUnitsAllocated")
                        + " in " + GetType().Name)); 

                }
                // end TT#1008 - MD - Jellis - Get Non Negative Message for various variables
				_storeTotalGenericUnitsAllocated = value;
			}
		}

		/// <summary>
		/// Gets or sets StoreTotalNonGenericUnitsAllocated.
		/// </summary>
		/// <remarks>
		/// Accumulated non-generic pack units allocated
		/// </remarks>
		internal int StoreTotalNonGenericUnitsAllocated
		{
			get
			{
				return _storeTotalNonGenericUnitsAllocated;
			}
			set
			{
                // begin TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "StoreTotalNonGenericUnitsAllocated")
                        + " in " + GetType().Name)); 

                }
                // end TT#1008 - MD - Jellis - Get Non Negative Message for various variables
				_storeTotalNonGenericUnitsAllocated = value;
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets or sets store's total generic pack item units allocated.
        /// </summary>
        /// <remarks>
        /// Accumulated generic pack item units allocated.
        /// </remarks>
        internal int StoreTotalGenericItemUnitsAllocated
        {
            get
            {
                return _storeTotalGenericItemUnitsAllocated;
            }
            set
            {
                // begin TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "StoreTotalGenericItemUnitsAllocated")
                        + " in " + GetType().Name)); 

                }
                // end TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                _storeTotalGenericItemUnitsAllocated = value;
            }
        }

        internal int StoreTotalGenericItemUnitsManuallyAllocated
        {
            get
            {
                return _storeTotalGenericItemUnitsManuallyAllocated;
            }
            set
            {
                // begin TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "StoreTotalGenericItemUnitsManuallyAllocated")
                        + " in " + GetType().Name)); 

                }
                // end TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                _storeTotalGenericItemUnitsManuallyAllocated = value;
            }
        }

        /// <summary>
        /// Gets or sets StoreTotalNonGenericItemUnitsAllocated.
        /// </summary>
        /// <remarks>
        /// Accumulated non-generic pack item units allocated
        /// </remarks>
        internal int StoreTotalNonGenericItemUnitsAllocated
        {
            get
            {
                return _storeTotalNonGenericItemUnitsAllocated;
            }
            set
            {
                // begin TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "StoreTotalNonGenericItemUnitsAllocated")
                        + " in " + GetType().Name)); 

                }
                // end TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                _storeTotalNonGenericItemUnitsAllocated = value;
            }
        }

        internal int StoreTotalNonGenericItemUnitsManuallyAllocated
        {
            get
            {
                return _storeTotalNonGenericItemUnitsManuallyAllocated;
            }
            set
            {
                // begin TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "StoreTotalNonGenericItemUnitsManuallyAllocated")
                        + " in " + GetType().Name)); 

                }
                // end TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                _storeTotalNonGenericItemUnitsManuallyAllocated = value;
            }
        }

        /// <summary>
        /// Gets or sets store's total generic pack IMO units allocated.
        /// </summary>
        /// <remarks>
        /// Accumulated generic pack IMO units allocated.
        /// </remarks>
        internal int StoreTotalGenericImoUnitsAllocated
        {
            get
            {
                return _storeTotalGenericImoUnitsAllocated;
            }
            set
            {
                // begin TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "StoreTotalGenericImoUnitsAllocated")
                        + " in " + GetType().Name)); 

                }
                // end TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                _storeTotalGenericImoUnitsAllocated = value;
            }
        }

        /// <summary>
        /// Gets or sets StoreTotalNonGenericImoUnitsAllocated.
        /// </summary>
        /// <remarks>
        /// Accumulated non-generic pack IMO units allocated
        /// </remarks>
        internal int StoreTotalNonGenericImoUnitsAllocated
        {
            get
            {
                return _storeTotalNonGenericImoUnitsAllocated;
            }
            set
            {
                // begin TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, " StoreTotalNonGenericImoUnitsAllocated")
                        + " in " + GetType().Name)); 

                }
                // end TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                _storeTotalNonGenericImoUnitsAllocated = value;
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1

		/// <summary>
		/// Gets or sets IsChanged database update flag
		/// </summary>
		internal bool StoreBulkIsChanged
		{
			get
			{
				return _storeBulkTotal.IsChanged;
			}
			set
			{
				_storeBulkTotal.IsChanged = value;
			}
		}

		/// <summary>
		/// Gets or sets IsNew database update flag
		/// </summary>
		internal bool StoreBulkIsNew
		{
			get 
			{
				return _storeBulkTotal.IsNew;
			}
			set
			{
				_storeBulkTotal.IsNew = value;
			}
		}

		// begin MID Track 4448 ANF Audit Enhancement
		/// <summary>
		/// Gets or sets all store Bulk detail audit flags simultaneously.
		/// </summary>
        //internal ushort StoreBulkAllDetailAuditFlags // TT#488 - MD - Jellis - Group Allocation
        internal uint StoreBulkAllDetailAuditFlags // TT#488 - MD - Jellis - Group Allocation
		{
			get
			{
				return _storeBulkTotal.AllDetailAuditFlags;
			}
			set
			{
				_storeBulkTotal.AllDetailAuditFlags = value;
			}
		}
		// end MID Track 4448 ANF Audit Enhancement

		/// <summary>
		/// Gets or sets StoreBulkIsManuallyAllocated audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates that the user has manually specified the Bulk unit allocated
		/// </remarks>
		internal bool StoreBulkIsManuallyAllocated 
		{
			get
			{
				return _storeBulkTotal.IsManuallyAllocated;
			}
			set
			{
				_storeBulkTotal.IsManuallyAllocated = value;
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
        /// <summary>
        /// Gets or sets StoreBulkItemIsManuallyAllocated audit flag.
        /// </summary>
        /// <remarks>
        /// True indicates that the user has manually specified the Bulk item unit allocated
        /// </remarks>
        internal bool StoreBulkItemIsManuallyAllocated        // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F  
        {
            get
            {
                return _storeBulkTotal.ItemIsManuallyAllocated;   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F    
            }
            set
            {
                _storeBulkTotal.ItemIsManuallyAllocated = value;  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 2
        
		/// <summary>
		/// Gets or sets StoreBulkWasAutoAllocated audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates that an auto allocation process affected the store's Bulk allocation.
		/// </remarks>
		internal bool StoreBulkWasAutoAllocated
		{
			get
			{
				return _storeBulkTotal.WasAutoAllocated;
			}
			set
			{
				_storeBulkTotal.WasAutoAllocated = value;
			}
		}
		
		/// <summary>
		/// Gets or sets StoreBulkChosenRuleAcceptedByHeader audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the chosen rule for the store's Bulk component was accepted on Header.
		/// </remarks>
		internal bool StoreBulkChosenRuleAcceptedByHeader // TT#488 - MD - Jellis - Group Allocation
		{
			get
			{
				return _storeBulkTotal.ChosenRuleAcceptedByHeader; // TT#488 - MD - Jellis - Group Allocation
 			}
			set
			{
				_storeBulkTotal.ChosenRuleAcceptedByHeader = value; // TT#488 - MD - Jellis - Group Allocation
			}
		}
	
        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets StoreBulkChosenRuleAcceptedByGroup audit flag.
        /// </summary>
        /// <remarks>
        /// True indicates the chosen rule for the store's Bulk component was accepted in Group Allocation.
        /// </remarks>
        internal bool StoreBulkChosenRuleAcceptedByGroup 
        {
            get
            {
                return _storeBulkTotal.ChosenRuleAcceptedByGroup;
            }
            set
            {
                _storeBulkTotal.ChosenRuleAcceptedByGroup = value;
            }
        }
        // end TT#488 - MD - Jellis - Group Allocation

		/// <summary>
		/// Gets or sets StoreBulkOut audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the store cannot be given any allocation on the Bulk Component.
		/// </remarks>
		internal bool StoreBulkOut
		{
			get
			{
				return _storeBulkTotal.Out;
			}
			set
			{
				_storeBulkTotal.Out = value;
			}
		}

        // begin TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
        ///// <summary>
        ///// Gets or sets StoreBulkLocked audit flag. 
        ///// </summary>
        ///// <remarks>
        ///// True prevents the store's Bulk allocation from changing via an auto allocation
        ///// process.  This lock is permanent and saved on the database.
        ///// </remarks>
        //internal bool StoreBulkLocked
        //{
        //    get
        //    {
        //        return _storeBulkTotal.Locked;
        //    }
        //    set
        //    {
        //        _storeBulkTotal.Locked = value;
        //    }
        //}
        // end TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
		 
		// begin TT#59 Implement Temp Locks
        // /// <summary>
		// /// Gets or sets StoreBulkTempLock audit flag.
		// /// </summary>
		// /// <remarks>
		// /// True prevents the store's Bulk allocation from changing via an auto allocation.
		// /// </remarks>
		//internal bool StoreBulkTempLock
		//{
		//	get 
		//	{
		//		return _storeBulkTotal.TempLock;
		//	}
		//	set
		//	{
		//		_storeBulkTotal.TempLock = value;
		//	}
		//}
        // end TT#59 Implement Temp Locks

		/// <summary>
		/// Gets or sets StoreBulkHadNeed audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the store's allocation was based on need.</remarks>
		internal bool StoreBulkHadNeed
		{
			get
			{
				return _storeBulkTotal.HadNeed;
			}
			set
			{
				_storeBulkTotal.HadNeed = value;
			}
		}

		/// <summary>
		/// Gets or sets StoreBulkFilledSizeHole audit flag.
		/// </summary>
		/// <return>
		/// True indicates units were allocated to fill a size need for some color and size.
		/// </return>
		internal bool StoreBulkFilledSizeHole
		{
			get
			{
				return _storeBulkTotal.FilledSizeHole;
			}
			set
			{
				_storeBulkTotal.FilledSizeHole = value;
			}
		}


		/// <summary>
		/// Gets or sets store Bulk Rule allocation from parent flag
		/// </summary>
		internal bool StoreBulkRuleAllocationFromParentComponent
		{
			get
			{
				return _storeBulkTotal.RuleAllocationFromParentComponent;
			}
			set
			{
				_storeBulkTotal.RuleAllocationFromParentComponent = value;
			}
		}

        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets store Bulk Rule allocation from group flag
        /// </summary>
        internal bool StoreBulkRuleAllocationFromGroupComponent
        {
            get
            {
                return _storeBulkTotal.RuleAllocationFromGroupComponent;
            }
            set
            {
                _storeBulkTotal.RuleAllocationFromGroupComponent = value;
            }
        }
        // end TT#488 - MD - Jellis - Group Allocation

		/// <summary>
		/// Gets or sets store Bulk Rule allocation from child flag
		/// </summary>
		internal bool StoreBulkRuleAllocationFromChildComponent
		{
			get
			{
				return _storeBulkTotal.RuleAllocationFromChildComponent;
			}
			set
			{
				_storeBulkTotal.RuleAllocationFromChildComponent = value;
			}
		}

		/// <summary>
		/// Gets or sets store Bulk Rule allocation from chosen Rule flag
		/// </summary>
		internal bool StoreBulkRuleAllocationFromChosenRule
		{
			get
			{
				return _storeBulkTotal.RuleAllocationFromChosenRule;
			}
			set
			{
				_storeBulkTotal.RuleAllocationFromChosenRule = value;
			}
		}


		/// <summary>
		/// Gets or sets store Bulk allocation from Bottom Up Size
		/// </summary>
		internal bool StoreBulkAllocationFromBottomUpSize
		{
			get
			{
				return _storeBulkTotal.AllocationFromBottomUpSize;
			}
			set
			{
				_storeBulkTotal.AllocationFromBottomUpSize = value;
			}
		}

		/// <summary>
		/// Gets or sets store Bulk allocation from Pack Content Break Out
		/// </summary>
		internal bool StoreBulkAllocationFromPackContentBreakOut
		{
			get
			{
				return _storeBulkTotal.AllocationFromPackContentBreakOut;
			}
			set
			{
				_storeBulkTotal.AllocationFromPackContentBreakOut = value;
			}
		}

		/// <summary>
		/// Gets or sets store Bulk allocation from Bulk Size Break Out
		/// </summary>
		internal bool StoreBulkAllocationFromBulkSizeBreakOut
		{
			get
			{
				return _storeBulkTotal.AllocationFromBulkSizeBreakOut;
			}
			set
			{
				_storeBulkTotal.AllocationFromBulkSizeBreakOut = value;
			}
		}

		// begin MID Track 4448 AnF Audit Enhancement
		/// <summary>
		/// Gets or sets Store Bulk Total Allocation Modified After Multi Split Audit Flag
		/// </summary>
		internal bool StoreBulkTotalAllocationModifiedAfterMultiSplit
		{
			get
			{
				return _storeBulkTotal.AllocationModifiedAfterMultiHeaderSplit;
			}
			set
			{
				_storeBulkTotal.AllocationModifiedAfterMultiHeaderSplit = value;
			}
		}
		// end MID Track 4448 AnF Audit Enhancement

		/// <summary>
		/// Gets or sets the store's BulkUnitsAllocated.
		/// </summary>
		internal int StoreBulkUnitsAllocated
		{
			get
			{
				return _storeBulkTotal.QtyAllocated;
			}
			set
			{
				_storeBulkTotal.QtyAllocated = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the store's BulkOrigUnitsAllocated.
		/// </summary>
		internal int StoreBulkOrigUnitsAllocated
		{
			get
			{
				return _storeBulkTotal.OriginalQtyAllocated;
			}
			set
			{
				_storeBulkTotal.OriginalQtyAllocated = value;
			}
		}

        // begin TT#1401 - JEllis Urban Reservation Stores pt 1
        /// <summary>
        /// Gets or sets the store's BulkItemUnitsAllocated.
        /// </summary>
        internal int StoreBulkItemUnitsAllocated
        {
            get
            {
                return _storeBulkTotal.ItemQtyAllocated;
            }
            set
            {
                _storeBulkTotal.ItemQtyAllocated = value;
            }
        }

        /// <summary>
        /// Gets or sets the store's BulkImoUnitsAllocated.
        /// </summary>
        internal int StoreBulkImoUnitsAllocated
        {
            get
            {
                return _storeBulkTotal.ImoQtyAllocated;
            }
            // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
            //set
            //{
            //    _storeBulkTotal.ImoQtyAllocated = value;
            //}
            // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        }
        // end TT#1401 - JEllis Urban Reservation Stores pt 1

		/// <summary>
		/// Gets or sets the store's Bulk allocation maximum.
		/// </summary>
		internal int StoreBulkMaximum
		{
			get
			{
				return _storeBulkTotal.Maximum;
			}
			set
			{
				_storeBulkTotal.Maximum = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's Bulk allocation minimum.
		/// </summary>
		internal int StoreBulkMinimum
		{
			get
			{
				return _storeBulkTotal.Minimum;
			}
			set
			{
				_storeBulkTotal.Minimum = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's Bulk primary allocation maximum.
		/// </summary>
		internal int StoreBulkPrimaryMaximum
		{
			get
			{
				return _storeBulkTotal.PrimaryMaximum;
			}
			set
			{
				_storeBulkTotal.PrimaryMaximum = value;
			}
		}

		/// <summary>
		/// Gets or sets store's Bulk units allocated via an auto allocation process.
		/// </summary>
		internal int StoreBulkUnitsAllocatedByAuto
		{
			get
			{
				return _storeBulkTotal.QtyAllocatedByAuto;
			}
			set
			{
				_storeBulkTotal.QtyAllocatedByAuto = value;
			}
		}

		/// <summary>
		/// Gets or sets store's Bulk units allocated by rule.
		/// </summary>
		internal int StoreBulkUnitsAllocatedByRule
		{
			get
			{
				return _storeBulkTotal.QtyAllocatedByRule;
			}
			set
			{
				_storeBulkTotal.QtyAllocatedByRule = value;
			}
		}

		/// <summary>
		/// Gets or sets the chosen rule.
		/// </summary>
		internal eRuleType StoreBulkChosenRuleType
		{
			get
			{
				return _storeBulkTotal.ChosenRuleType;
			}
			set
			{
				_storeBulkTotal.ChosenRuleType = value;
			}
		}


		/// <summary>
		/// Gets or sets store's Bulk chosen rule layer ID.
		/// </summary>
		internal int StoreBulkChosenRuleLayerID
		{
			get
			{
				return _storeBulkTotal.ChosenRuleLayerID;
			}
			set
			{
				_storeBulkTotal.ChosenRuleLayerID = value;
			}
		}

		/// <summary>
		/// Gets or sets store's Bulk units allocated by the chosen rule.
		/// </summary>
		internal int StoreBulkChosenRuleUnitsAllocated
		{
			get
			{
				return _storeBulkTotal.ChosenRuleQtyAllocated;
			}
			set
			{
				_storeBulkTotal.ChosenRuleQtyAllocated = value;
			}
		}

		/// <summary>
		/// Gets or sets Store Bulk Total LastNeedDay
		/// </summary>
		/// <remarks>
		/// DateTime Format
		/// </remarks>
		internal DateTime StoreBulkLastNeedDay
		{
			get
			{
				return _storeBulkTotal.StoreLastNeedDay;
			}
			set
			{
				_storeBulkTotal.StoreLastNeedDay = value;
			}
		}

		/// <summary>
		/// Gets or sets Store's Bulk Total Unit Need Before.
		/// </summary>
		internal int StoreBulkUnitNeedBefore
		{
			get
			{
				return _storeBulkTotal.StoreUnitNeedBefore;
			}
			set
			{
				_storeBulkTotal.StoreUnitNeedBefore = value;
			}
		}

		/// <summary>
		/// Gets or sets store's Bulk PercentNeedBefore
		/// </summary>
		internal double StoreBulkPercentNeedBefore
		{
			get
			{
				return _storeBulkTotal.StorePercentNeedBefore;
			}
			set
			{
				_storeBulkTotal.StorePercentNeedBefore = value;
			}
		}

		 
		/// <summary>
		/// Gets or sets store's bulk color total units allocated across all bulk colors.
		/// </summary>
		internal int StoreBulkColorTotalUnitsAllocated
		{
			get
			{
				return _storeBulkColorTotalUnitsAllocated;
			}
			set
			{
                // begin TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "StoreBulkColorTotalUnitsAllocated")
                        + " in " + GetType().Name)); 

                }
                // end TT#1008 - MD - Jellis - Get Non Negative Message for various variables
				_storeBulkColorTotalUnitsAllocated = value;
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets or sets store's bulk color total item units allocated across all bulk colors.
        /// </summary>
        internal int StoreBulkColorTotalItemUnitsAllocated
        {
            get
            {
                return _storeBulkColorTotalItemUnitsAllocated;
            }
            set
            {
                // begin TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "StoreBulkColorTotalItemUnitsAllocated")
                        + " in " + GetType().Name));
                }
                // end TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                _storeBulkColorTotalItemUnitsAllocated = value;
            }
        }


        internal int StoreBulkColorTotalItemUnitsManuallyAllocated
        {
            get
            {
                return _storeBulkColorTotalItemUnitsManuallyAllocated;
            }
            set
            {
                _storeBulkColorTotalItemUnitsManuallyAllocated = value;
            }
        }

        /// <summary>
        /// Gets or sets store's bulk color total IMO units allocated across all bulk colors.
        /// </summary>
        internal int StoreBulkColorTotalImoUnitsAllocated
        {
            get
            {
                return _storeBulkColorTotalImoUnitsAllocated;
            }
            set
            {
                // begin TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "StoreBulkColorTotalImoUnitsAllocated")
                        + " in " + GetType().Name)); 

                }
                // end TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                _storeBulkColorTotalImoUnitsAllocated = value;
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1


        // begin TT#1021 - MD - Jellis - Header Status Wrong
        /// <summary>
        /// Gets or sets store's bulk size total units allocated across all bulk colors.
        /// </summary>
        internal int StoreBulkSizeTotalUnitsAllocated
        {
            get
            {
                return _storeBulkSizeTotalUnitsAllocated;
            }
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "StoreBulkSizeTotalUnitsAllocated")
                        + " in " + GetType().Name));

                }
                _storeBulkSizeTotalUnitsAllocated = value;
            }
        }
        // end TT#1021 - MD - Jellis - Header Status Wrong


		/// <summary>
		/// Gets or sets StoreShipDay
		/// </summary>
		/// <remarks>
		/// Format yyyymmdd
		/// </remarks>
		internal DateTime StoreShipDay
		{
			get 
			{
				return _shipDay;
			}
			set
			{
				if (StoreShipDay != value)
				{
					_shipDay = value;
					this.StoreTotalIsChanged = true;
				}
			}
		}

        // Begin TT#5502 - JSmith - Can't get intransit to relieve
		/// <summary>
        /// Gets or sets StoreShipDay saved to the database
        /// </summary>
        /// <remarks>
        /// Format yyyymmdd
        /// </remarks>
        internal DateTime DB_StoreShipDay
        {
            get
            {
                return _DB_ShipDay;
            }
            set
            {
                if (_DB_ShipDay != value)
                {
                    _DB_ShipDay = value;
                }
            }
        }
		// End TT#5502 - JSmith - Can't get intransit to relieve

		/// <summary>
		/// Gets or sets store' general audits simultaneously.
		/// </summary>
		internal ushort AllGeneralAudits    // TT#1401 - JEllis - Urban Reservation Store pt 11
		{
			get 
			{
				return _storeGeneralAudit.AllFlags;
			}
			set
			{
				_storeGeneralAudit.AllFlags = value;
			}
		}

		/// <summary>
		/// Gets or sets the StorePercentNeedLimitReached audit flag.
		/// </summary>
		internal bool StorePercentNeedLimitReached
		{
			get
			{
				return _storeGeneralAudit.PercentNeedLimitReached;
			}
			set
			{
				if (StorePercentNeedLimitReached != value)
				{
					_storeGeneralAudit.PercentNeedLimitReached = value;
					this.StoreTotalIsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets StoreCapacityMaximumReached general audit flag.
		/// </summary>
		internal bool StoreCapacityMaximumReached
		{
			get
			{
				return _storeGeneralAudit.CapacityMaximumReached;
			}
			set
			{
				if (StoreCapacityMaximumReached != value)
				{
					_storeGeneralAudit.CapacityMaximumReached = value;
					this.StoreTotalIsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or set StorePrimaryMaximumReached general audit flag.
		/// </summary>
		internal bool StorePrimaryMaximumReached
		{
			get
			{
				return _storeGeneralAudit.PrimaryMaximumReached;
			}
			set
			{
				if (StorePrimaryMaximumReached != value)
				{
					_storeGeneralAudit.PrimaryMaximumReached = value;
					this.StoreTotalIsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets StoreMayExceedMax general audit flag.
		/// </summary>
		internal bool StoreMayExceedMax
		{
			get
			{
				return _storeGeneralAudit.MayExceedMax;
			}
			set
			{
				if (StoreMayExceedMax != value)
				{
					_storeGeneralAudit.MayExceedMax = value;
					this.StoreTotalIsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets StoreMayExceedPrimary general audit flag
		/// </summary>
		internal bool StoreMayExceedPrimary
		{
			get
			{
				return _storeGeneralAudit.MayExceedPrimary;
			}
			set
			{
				if (StoreMayExceedPrimary != value)
				{
					_storeGeneralAudit.MayExceedPrimary = value;
					this.StoreTotalIsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets StoreMayExceedCapacity general audit flag.
		/// </summary>
		internal bool StoreMayExceedCapacity
		{
			get
			{
				return _storeGeneralAudit.MayExceedCapacity;
			}
			set
			{
				if (StoreMayExceedCapacity != value)
				{
					_storeGeneralAudit.MayExceedCapacity = value;
					this.StoreTotalIsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets store's eligibility
		/// </summary>
		internal bool StoreIsEligible
		{
			get
			{
				return _storeGeneralAudit.StoreIsEligible;
			}
			set
			{
				if (StoreIsEligible != value)
				{
					_storeGeneralAudit.StoreIsEligible = value;
					this.StoreTotalIsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets store's allocation priority
		/// </summary>
		internal bool StoreAllocationPriority
		{
			get
			{
				return _storeGeneralAudit.StoreAllocationPriority;
			}
			set
			{
				if (StoreAllocationPriority != value)
				{
					_storeGeneralAudit.StoreAllocationPriority = value;
					this.StoreTotalIsChanged = true;
				}
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Store pt 11
        /// <summary>
        /// Gets or sets store's Imo Inclusion status (True: store is included in allocation of IMO Header--meaningful only when header is an IMO header)
        /// </summary>
        internal bool IncludeStoreInAllocation
        {
            get
            {
                return _storeGeneralAudit.IncludeStoreInAllocation;
            }
            set
            {
                if (IncludeStoreInAllocation != value)
                {
                    _storeGeneralAudit.IncludeStoreInAllocation = value;
                    StoreTotalIsChanged = true;
                }
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Store pt 11

		/// <summary>
		/// Gets or sets All StoreTotal Ship flags simultaneously 
		/// </summary>
		internal byte StoreTotalAllShipFlags
		{
			get
			{
				return _storeShipStatus.AllFlags;
			}
			set
			{
				_storeShipStatus.AllFlags = value;
			}
		}

		/// <summary>
		/// Gets or sets StoreTotalShippingStarted audit flag.
		/// </summary>
		internal bool StoreTotalShippingStarted
		{
			get
			{
				return _storeShipStatus.ShippingStarted;
			}
			set
			{
				if (StoreTotalShippingStarted != value)
				{
					_storeShipStatus.ShippingStarted = value;
					this.StoreTotalIsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets StoreTotalShippingComplete audit flag.
		/// </summary>
		internal bool StoreTotalShippingComplete
		{
			get
			{
				return _storeShipStatus.ShippingComplete;
			}
			set
			{
				if (StoreTotalShippingComplete != value)
				{
					_storeShipStatus.ShippingComplete = value;
					this.StoreTotalIsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets StoreTotalShippingOnHold audit flag.
		/// </summary>
		internal bool StoreTotalShippingOnHold
		{
			get
			{
				return _storeShipStatus.ShippingOnHold;
			}
			set
			{
				if (StoreTotalShippingOnHold != value)
				{
					_storeShipStatus.ShippingOnHold = value;
					this.StoreTotalIsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets StoreTotalUnitsShipped
		/// </summary>
		internal int StoreTotalUnitsShipped
		{
			get
			{
				return _unitsShipped;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_QtyShippedCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_QtyShippedCannotBeNeg));
				}
				else
				{
					if (StoreTotalUnitsShipped != value)
					{
						_unitsShipped = value;
						this.StoreTotalIsChanged = true;
					}
				}
			}
		}

		// begin MID Track 4448 ANF Audit Enhancement
		/// <summary>
		/// Gets or sets whether the store style allocation was manually changed (does not indicate whether the size allocation was manually changed). The change may have occurred for the Total store or for any pack or any bulk color on the header
		/// </summary>
		internal bool StoreStyleAllocationManuallyChanged
		{
			get
			{
				return this._storeTotalStyleAloctnManuallyChngd;
			}
			set
			{
				this._storeTotalStyleAloctnManuallyChngd = value;
			}
		}
		/// <summary>
		/// Gets or set whether the store size allocation was manually changed for any bulk size within any bulk color
		/// </summary>
		internal bool StoreSizeAllocationManuallyChanged
		{
			get
			{
				return this._storeSizeAloctnManuallyChngd;
			}
			set
			{
				this._storeSizeAloctnManuallyChngd = value;
			}
		}
		// end MID Track 4448 ANF Audit Enhancement

		//========
		// METHODS
		//========
	}
	#endregion Store Total Allocated

	#region Store Pack Allocated
	/// <summary>
	/// Fields to track and audit a store's pack allocations.
	/// </summary>
	internal struct StorePackAllocated
	{
		//=======
		// FIELDS
		//=======
		private StoreAllocatedBin _storePack;
		private int _packsShipped;
		private ShippingStatusFlags _storePackShipStatus;
		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets or sets IsChanged database update flag
		/// </summary>
		internal bool StorePackIsChanged
		{
			get
			{
				return _storePack.IsChanged;
			}
			set
			{
				_storePack.IsChanged = value;
			}
		}

		/// <summary>
		/// Gets or sets IsNew database update flag
		/// </summary>
		internal bool StorePackIsNew
		{
			get 
			{
				return _storePack.IsNew;
			}
			set
			{
				_storePack.IsNew = value;
			}
		}


		/// <summary>
		/// Gets or sets value for All Detail Audit Flags simultaneously.
		/// </summary>
        //internal ushort AllDetailAuditFlags // TT#488 - MD - Jellis - Group Allocation
        internal uint AllDetailAuditFlags // TT#488 - MD - Jellis - Group Allocation
		{
			get
			{
				return _storePack.AllDetailAuditFlags;
			}
			set
			{
				_storePack.AllDetailAuditFlags = value;
			}
		}

		/// <summary>
		/// Gets or sets value for all ship status flags simultaneously.
		/// </summary>
		internal byte AllShipFlags
		{
			get
			{
				return _storePackShipStatus.AllFlags;
			}
			set
			{
				_storePackShipStatus.AllFlags = value;
			}
		}

		/// <summary>
		/// Gets or sets PackIsManuallyAllocated audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates that the user has manually specified the packs allocated
		/// </remarks>
		internal bool PackIsManuallyAllocated 
		{
			get
			{
				return _storePack.IsManuallyAllocated;
			}
			set
			{
				_storePack.IsManuallyAllocated = value;
			}
		}
        
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
        /// <summary>
        /// Gets or sets PackItemIsManuallyAllocated audit flag.
        /// </summary>
        /// <remarks>
        /// True indicates that the user has manually specified the Item packs allocated
        /// </remarks>
        internal bool PackItemIsManuallyAllocated            // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        {
            get
            {
                return _storePack.ItemIsManuallyAllocated;   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F  
            }
            set
            {
                _storePack.ItemIsManuallyAllocated = value;  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 2

		/// <summary>
		/// Gets or sets PackWasAutoAllocated audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates that an auto allocation process affected the store's pack allocation.
		/// </remarks>
		internal bool PackWasAutoAllocated
		{
			get
			{
				return _storePack.WasAutoAllocated;
			}
			set
			{
				_storePack.WasAutoAllocated = value;
			}
		}
		
		/// <summary>
		/// Gets or sets PackChosenRuleAcceptedByHeader audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the chosen rule for the store's pack component was accepted on Header.
		/// </remarks>
		internal bool PackChosenRuleAcceptedByHeader // TT#488 - MD - Jellis - Group Allocation
		{
			get
			{
				return _storePack.ChosenRuleAcceptedByHeader;  // TT#488 - MD - Jellis - Group Allocation
			}
			set
			{
				_storePack.ChosenRuleAcceptedByHeader = value; // TT#488 - MD - Jellis - Group Allocation
			}
		}
	
        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets PackChosenRuleAcceptedByGroup audit flag.
        /// </summary>
        /// <remarks>
        /// True indicates the chosen rule for the store's pack component was accepted in Group Allocation.
        /// </remarks>
        internal bool PackChosenRuleAcceptedByGroup
        {
            get
            {
                return _storePack.ChosenRuleAcceptedByGroup;
            }
            set
            {
                _storePack.ChosenRuleAcceptedByGroup = value;
            }
        }
        // end TT#488 - MD - Jellis - Group Allocation

		/// <summary>
		/// Gets or sets PackOut audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the store cannot be given any allocation on the pack Component.
		/// </remarks>
		internal bool PackOut
		{
			get
			{
				return _storePack.Out;
			}
			set
			{
				_storePack.Out = value;
			}
		}

        // begin TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
        ///// <summary>
        ///// Gets or sets PackLocked audit flag. 
        ///// </summary>
        ///// <remarks>
        ///// True prevents the store's pack allocation from changing via an auto allocation
        ///// process.  This lock is permanent and saved on the database.
        ///// </remarks>
        //internal bool PackLocked
        //{
        //    get
        //    {
        //        return _storePack.Locked;
        //    }
        //    set
        //    {
        //        _storePack.Locked = value;
        //    }
        //}
        // end TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
		
        // begin TT#59 Implement Temp Locks
		// /// <summary>
		// /// Gets or sets PackTempLock audit flag.
		// /// </summary>
		// /// <remarks>
		// /// True prevents the store's pack allocation from changing via an auto allocation.
		// /// </remarks>
		//internal bool PackTempLock
		//{
		//	get 
		//	{
		//		return _storePack.TempLock;
		//	}
		//	set
		//	{
		//		_storePack.TempLock = value;
		//	}
		//}
        // end TT#59 Implement Temp Locks


		/// <summary>
		/// Gets or sets store Pack Rule allocation from parent flag
		/// </summary>
		internal bool StorePackRuleAllocationFromParentComponent
		{
			get
			{
				return _storePack.RuleAllocationFromParentComponent;
			}
			set
			{
				_storePack.RuleAllocationFromParentComponent = value;
			}
		}

        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets store Pack Rule allocation from group flag
        /// </summary>
        internal bool StorePackRuleAllocationFromGroupComponent
        {
            get
            {
                return _storePack.RuleAllocationFromGroupComponent;
            }
            set
            {
                _storePack.RuleAllocationFromGroupComponent = value;
            }
        }
        // end TT#488 - MD - Jellis - Group Allocation

		/// <summary>
		/// Gets or sets store Pack Rule allocation from child flag
		/// </summary>
		internal bool StorePackRuleAllocationFromChildComponent
		{
			get
			{
				return _storePack.RuleAllocationFromChildComponent;
			}
			set
			{
				_storePack.RuleAllocationFromChildComponent = value;
			}
		}

		/// <summary>
		/// Gets or sets store Pack Rule allocation from chosen Rule flag
		/// </summary>
		internal bool StorePackRuleAllocationFromChosenRule
		{
			get
			{
				return _storePack.RuleAllocationFromChosenRule;
			}
			set
			{
				_storePack.RuleAllocationFromChosenRule = value;
			}
		}

		/// <summary>
		/// Gets or sets store Pack allocation from Pack Content Break Out 
		/// </summary>
		internal bool StorePackAllocationFromPackContentBreakOut
		{
			get
			{
				return _storePack.AllocationFromPackContentBreakOut;
			}
			set
			{
				_storePack.AllocationFromPackContentBreakOut = value;
			}
		}


		/// <summary>
		/// Gets or sets PackHadNeed audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the store's allocation was based on need.</remarks>
		internal bool PackHadNeed
		{
			get
			{
				return _storePack.HadNeed;
			}
			set
			{
				_storePack.HadNeed = value;
			}
		}

		// begin MID Track 4448 AnF Audit Enhancement
		/// <summary>
		/// Gets or sets PackAllocationModifiedAfterMultiHeaderSplit audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates that the pack allocation was changed after a multi header split
		/// </remarks>
		internal bool PackAllocationModifiedAfterMultiHeaderSplit 
		{
			get
			{
				return _storePack.AllocationModifiedAfterMultiHeaderSplit;
			}
			set
			{
				_storePack.AllocationModifiedAfterMultiHeaderSplit = value;
			}
		}
		// end MID Track 4448 AnF Audit Enhancement

		/// <summary>
		/// Gets or sets the store's PacksAllocated.
		/// </summary>
		internal int PacksAllocated
		{
			get
			{
				return _storePack.QtyAllocated;
			}
			set
			{
				_storePack.QtyAllocated = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's Original PacksAllocated.
		/// </summary>
		internal int OrigPacksAllocated
		{
			get
			{
				return _storePack.OriginalQtyAllocated;
			}
			set
			{
				_storePack.OriginalQtyAllocated = value;
			}
		}
		
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets or sets the store's ItemPacksAllocated.
        /// </summary>
        internal int ItemPacksAllocated
        {
            get
            {
                return _storePack.ItemQtyAllocated;
            }
            set
            {
                _storePack.ItemQtyAllocated = value;
            }
        }


        /// <summary>
        /// Gets or sets the store's ImoPacksAllocated.
        /// </summary>
        internal int ImoPacksAllocated
        {
            get
            {
                return _storePack.ImoQtyAllocated;
            }
            // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
            //set
            //{
            //    _storePack.ImoQtyAllocated = value;
            //}
            // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1

		/// <summary>
		/// Gets or sets the store's pack allocation maximum.
		/// </summary>
		internal int PackMaximum
		{
			get
			{
				return _storePack.Maximum;
			}
			set
			{
				_storePack.Maximum = value;
			}
		}

		/// <summary>
		/// Gets the largest pack maximum.
		/// </summary>
		internal int LargestPackMaximum
		{
			get
			{
				return _storePack.LargestMaximum;
			}
		}

		/// <summary>
		/// Gets or sets the store's pack allocation minimum.
		/// </summary>
		internal int PackMinimum
		{
			get
			{
				return _storePack.Minimum;
			}
			set
			{
				_storePack.Minimum = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's pack primary allocation maximum.
		/// </summary>
		internal int PackPrimaryMaximum
		{
			get
			{
				return _storePack.PrimaryMaximum;
			}
			set
			{
				_storePack.PrimaryMaximum = value;
			}
		}

		/// <summary>
		/// Gets or sets store's packs allocated via an auto allocation process.
		/// </summary>
		internal int PacksAllocatedByAuto
		{
			get
			{
				return _storePack.QtyAllocatedByAuto;
			}
			set
			{
				_storePack.QtyAllocatedByAuto = value;
			}
		}

		/// <summary>
		/// Gets or sets store's packs allocated by rule.
		/// </summary>
		internal int PacksAllocatedByRule
		{
			get
			{
				return _storePack.QtyAllocatedByRule;
			}
			set
			{
				_storePack.QtyAllocatedByRule = value;
			}
		}

		/// <summary>
		/// Gets or sets the chosen rule.
		/// </summary>
		internal eRuleType StorePackChosenRuleType
		{
			get
			{
				return _storePack.ChosenRuleType;
			}
			set
			{
				_storePack.ChosenRuleType = value;
			}
		}

		/// <summary>
		/// Gets or sets store's Pack chosen rule layer ID.
		/// </summary>
		internal int StorePackChosenRuleLayerID
		{
			get
			{
				return _storePack.ChosenRuleLayerID;
			}
			set
			{
				_storePack.ChosenRuleLayerID = value;
			}
		}

		/// <summary>
		/// Gets or sets store's packs allocated by the chosen rule.
		/// </summary>
		internal int StoreChosenRulePacksAllocated
		{
			get
			{
				return _storePack.ChosenRuleQtyAllocated;
			}
			set
			{
				_storePack.ChosenRuleQtyAllocated = value;
			}
		}

		/// <summary>
		/// Gets or sets Store Pack LastNeedDay
		/// </summary>
		/// <remarks>
		/// DateTime Format
		/// </remarks>
		internal DateTime StorePackLastNeedDay
		{
			get
			{
				return this._storePack.StoreLastNeedDay;
			}
			set
			{
				_storePack.StoreLastNeedDay = value;
			}
		}

		/// <summary>
		/// Gets or sets Store's Pack Unit Need Before.
		/// </summary>
		internal int StorePackUnitNeedBefore
		{
			get
			{
				return _storePack.StoreUnitNeedBefore;
			}
			set
			{
				_storePack.StoreUnitNeedBefore = value;
			}
		}

		/// <summary>
		/// Gets or sets store's Pack PercentNeedBefore
		/// </summary>
		internal double StorePackPercentNeedBefore
		{
			get
			{
				return _storePack.StorePercentNeedBefore;
			}
			set
			{
				_storePack.StorePercentNeedBefore = value;
			}
		}


		/// <summary>
		/// Gets or sets ShippingStarted audit flag.
		/// </summary>
		internal bool ShippingStarted
		{
			get
			{
				return _storePackShipStatus.ShippingStarted;
			}
			set
			{
				if (ShippingStarted != value)
				{
					_storePackShipStatus.ShippingStarted = value;
					this.StorePackIsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets ShippingComplete audit flag.
		/// </summary>
		internal bool ShippingComplete
		{
			get
			{
				return _storePackShipStatus.ShippingComplete;
			}
			set
			{
				if (ShippingComplete != value)
				{
					_storePackShipStatus.ShippingComplete = value;
					this.StorePackIsChanged = true;
				}
			}
		}


		/// <summary>
		/// Gets or sets PackShipped
		/// </summary>
		internal int PacksShipped
		{
			get
			{
				return _packsShipped;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_QtyShippedCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_QtyShippedCannotBeNeg));
				}
				else
				{
					if (PacksShipped != value)
					{
						_packsShipped = value;
						this.StorePackIsChanged = true;
					}
				}
			}
		}

		//========
		// METHODS
		//========
	}
	#endregion Store Pack Allocated

	#region Store Color Allocated
	/// <summary>
	/// Fields to track and audit a store's color allocations.
	/// </summary>
	internal struct StoreColorAllocated
	{
		//=======
		// FIELDS
		//=======
		private StoreAllocatedBin _storeColor;
		private ShippingStatusFlags _storeColorShipStatus;
		private int _colorUnitsShipped;
		private int _storeTotalSizeUnitsAllocated;
        private int _storeTotalSizeItemUnitsAllocated; // TT#1401 - JEllis - Urban Reservation Stores pt 1
        private int _storeTotalSizeItemUnitsManuallyAllocated; // TT#1401 - JEllis -Urban Virtual Store Warehouse pt 28F
        private int _storeTotalSizeImoUnitsAllocated;  // TT#1401 - JEllis - Urban Reservation Stores pt 1
		private double _storeTotalSizePctToColorTotal;
		private bool _recalcStoreSizePctToColorTotal;
        private int _itemIdealMinimum; // TT#246 - MD - JEllis - AnF VSW In Store Minimum pt 5
        //=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets or sets IsChanged database update flag
		/// </summary>
		internal bool StoreColorIsChanged
		{
			get
			{
				return _storeColor.IsChanged;
			}
			set
			{
				_storeColor.IsChanged = value;
			}
		}

		/// <summary>
		/// Gets or sets IsNew database update flag
		/// </summary>
		internal bool StoreColorIsNew
		{
			get 
			{
				return _storeColor.IsNew;
			}
			set
			{
				_storeColor.IsNew = value;
			}
		}


		/// <summary>
		/// Gets or sets all detail audit flags simultaneously.
		/// </summary>
        //internal ushort AllDetailAuditFlags // TT#488 - MD - Jellis - Group Allocation
        internal uint AllDetailAuditFlags // TT#488 - MD - Jellis - Group Allocation
        {
			get
			{
				return _storeColor.AllDetailAuditFlags;
			}
			set
			{
				_storeColor.AllDetailAuditFlags = value;
			}
		}

		/// <summary>
		/// Gets or sets all ship status flags simultaneously.
		/// </summary>
		internal byte AllShipStatusFlags
		{
			get
			{
				return _storeColorShipStatus.AllFlags;
			}
			set
			{
				_storeColorShipStatus.AllFlags = value;
			}
		}
		/// <summary>
		/// Gets or sets StoreColorIsManuallyAllocated audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates that the user has manually specified the color units allocated
		/// </remarks>
		internal bool StoreColorIsManuallyAllocated 
		{
			get
			{
				return _storeColor.IsManuallyAllocated;
			}
			set
			{
				_storeColor.IsManuallyAllocated = value;
			}
		}
        
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
        /// <summary>
        /// Gets or sets StoreColorItemIsManuallyAllocated audit flag.
        /// </summary>
        /// <remarks>
        /// True indicates that the user has manually specified the color item units allocated
        /// </remarks>
        internal bool StoreColorItemIsManuallyAllocated     // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        {
            get
            {
                return _storeColor.ItemIsManuallyAllocated;  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
            }
            set
            {
                _storeColor.ItemIsManuallyAllocated = value;  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 2

		/// <summary>
		/// Gets or sets StoreColorWasAutoAllocated audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates that an auto allocation process affected the store's Color allocation.
		/// </remarks>
		internal bool StoreColorWasAutoAllocated
		{
			get
			{
				return _storeColor.WasAutoAllocated;
			}
			set
			{
				_storeColor.WasAutoAllocated = value;
			}
		}
		
		/// <summary>
		/// Gets or sets StoreColorChosenRuleAcceptedByHeader audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the chosen rule for the store's color component was accepted on Header.
		/// </remarks>
        internal bool StoreColorChosenRuleAcceptedByHeader // TT#488 - MD - Jellis - Group Allocation
		{
			get
			{
                return _storeColor.ChosenRuleAcceptedByHeader; // TT#488 - MD - Jellis - Group Allocation
			}
			set
			{
                _storeColor.ChosenRuleAcceptedByHeader = value; // TT#488 - MD - Jellis - Group Allocation
			}
		}

        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets StoreColorChosenRuleAcceptedByGroup audit flag.
        /// </summary>
        /// <remarks>
        /// True indicates the chosen rule for the store's color component was accepted in Group Allocation.
        /// </remarks>
        internal bool StoreColorChosenRuleAcceptedByGroup
        {
            get
            {
                return _storeColor.ChosenRuleAcceptedByGroup;
            }
            set
            {
                _storeColor.ChosenRuleAcceptedByGroup = value;
            }
        }
        // end TT#488 - MD - Jellis - Group Allocation
	
		/// <summary>
		/// Gets or sets StoreColorOut audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the store cannot be given any allocation on the color Component.
		/// </remarks>
		internal bool StoreColorOut
		{
			get
			{
				return _storeColor.Out;
			}
			set
			{
				_storeColor.Out = value;
			}
		}

        // begin TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
        ///// <summary>
        ///// Gets or sets StoreColorLocked audit flag. 
        ///// </summary>
        ///// <remarks>
        ///// True prevents the store's color allocation from changing via an auto allocation
        ///// process.  This lock is permanent and saved on the database.
        ///// </remarks>
        //internal bool StoreColorLocked
        //{
        //    get
        //    {
        //        return _storeColor.Locked;
        //    }
        //    set
        //    {
        //        _storeColor.Locked = value;
        //    }
        //}
        // end TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
        // begin TT#246 - MD - JEllis - AnF VSW In Store Minimum pt 5
        internal int StoreSizeItemIdealMinimum
        {
            get
            {
                return _itemIdealMinimum;
            }
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "StoreSizeItemIdealMinimum"))  // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                    //MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg)           // TT#891 - MD - Jellis - Group Allocation Need Gets Error  
                    //+ ": StoreSizeItemIdealMinimum in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error

                }
                _itemIdealMinimum = value;
            }
        }
        // end TT#246 - MD - JEllis - AnF VSW In Store Minimum pt 5
		// begin TT#59 Implement Temp Locks
        // /// <summary>
		// /// Gets or sets StoreColorTempLock audit flag.
		// /// </summary>
		// /// <remarks>
		// /// True prevents the store's color allocation from changing via an auto allocation.
		// /// </remarks>
		//internal bool StoreColorTempLock
		//{
		//	get 
		//	{
		//		return _storeColor.TempLock;
		//	}
		//	set
		//	{
		//		_storeColor.TempLock = value;
		//	}
		//}
        // end TT#59 Implement Temp Locks

		/// <summary>
		/// Gets or sets StoreColorHadNeed audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the store's allocation was based on need.</remarks>
		internal bool StoreColorHadNeed
		{
			get
			{
				return _storeColor.HadNeed;
			}
			set
			{
				_storeColor.HadNeed = value;
			}
		}

		/// <summary>
		/// Gets or sets StoreColorFilledSizeHole audit flag.
		/// </summary>
		internal bool StoreColorFilledSizeHole
		{
			get
			{
				return _storeColor.FilledSizeHole;
			}
			set
			{
				_storeColor.FilledSizeHole = value;
			}
		}


		/// <summary>
		/// Gets or sets store Color Rule allocation from parent flag
		/// </summary>
		internal bool StoreColorRuleAllocationFromParentComponent
		{
			get
			{
				return _storeColor.RuleAllocationFromParentComponent;
			}
			set
			{
				_storeColor.RuleAllocationFromParentComponent = value;
			}
		}

        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets store Color Rule allocation from group flag
        /// </summary>
        internal bool StoreColorRuleAllocationFromGroupComponent
        {
            get
            {
                return _storeColor.RuleAllocationFromGroupComponent;
            }
            set
            {
                _storeColor.RuleAllocationFromGroupComponent = value;
            }
        }

        // end TT#488 - MD - Jellis - Group Allocation


		/// <summary>
		/// Gets or sets store Color Rule allocation from child flag
		/// </summary>
		internal bool StoreColorRuleAllocationFromChildComponent
		{
			get
			{
				return _storeColor.RuleAllocationFromChildComponent;
			}
			set
			{
				_storeColor.RuleAllocationFromChildComponent = value;
			}
		}

		/// <summary>
		/// Gets or sets store Color Rule allocation from chosen Rule flag
		/// </summary>
		internal bool StoreColorRuleAllocationFromChosenRule
		{
			get
			{
				return _storeColor.RuleAllocationFromChosenRule;
			}
			set
			{
				_storeColor.RuleAllocationFromChosenRule = value;
			}
		}

		/// <summary>
		/// Gets or sets store Color allocation from bottom up size
		/// </summary>
		internal bool StoreColorAllocationFromBottomUpSize
		{
			get
			{
				return _storeColor.AllocationFromBottomUpSize;
			}
			set
			{
				_storeColor.AllocationFromBottomUpSize = value;
			}
		}

		/// <summary>
		/// Gets or sets store Color allocation from Bulk Size BreakOut
		/// </summary>
		internal bool StoreColorAllocationFromBulkSizeBreakOut
		{
			get
			{
				return _storeColor.AllocationFromBulkSizeBreakOut;
			}
			set
			{
				_storeColor.AllocationFromBulkSizeBreakOut = value;
			}
		}

        // begin MID Track 4448 AnF Audit Enhancement
		/// <summary>
		/// Gets or sets Store Color Allocation Modified After Multi Header Split Audit Flag
		/// </summary>
		internal bool StoreColorAllocationModifiedAfterMultiHeaderSplit
		{
			get
			{
				return _storeColor.AllocationModifiedAfterMultiHeaderSplit;
			}
			set
			{
				_storeColor.AllocationModifiedAfterMultiHeaderSplit = value;
			}
		}
		// end MID Track 4448 AnF Audit Enhancement

		/// <summary>
		/// Gets or sets the store's ColorUnitsAllocated.
		/// </summary>
		internal int StoreColorUnitsAllocated
		{
			get
			{
				return _storeColor.QtyAllocated;
			}
			set
			{
				_storeColor.QtyAllocated = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's ColorOrigUnitsAllocated.
		/// </summary>
		internal int StoreColorOrigUnitsAllocated
		{
			get
			{
				return _storeColor.OriginalQtyAllocated;
			}
			set
			{
				_storeColor.OriginalQtyAllocated = value;
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets or sets the store's ColorItemUnitsAllocated.
        /// </summary>
        internal int StoreColorItemUnitsAllocated
        {
            get
            {
                return _storeColor.ItemQtyAllocated;
            }
            set
            {
                _storeColor.ItemQtyAllocated = value;
            }
        }

        /// <summary>
        /// Gets or sets the store's ColorImoUnitsAllocated.
        /// </summary>
        internal int StoreColorImoUnitsAllocated
        {
            get
            {
                return _storeColor.ImoQtyAllocated;
            }
            // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
            //set
            //{
            //    _storeColor.ImoQtyAllocated = value;
            //}
            // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1

		/// <summary>
		/// Gets Largest Color Maximum.
		/// </summary>
		internal int LargestColorMaximum
		{
			get
			{
				return _storeColor.LargestMaximum;
			}
		}

		/// <summary>
		/// Gets or sets the store's color unit allocation maximum.
		/// </summary>
		internal int StoreColorUnitMaximum
		{
			get
			{
				return _storeColor.Maximum;
			}
			set
			{
				_storeColor.Maximum = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's color unit allocation minimum.
		/// </summary>
		internal int StoreColorUnitMinimum
		{
			get
			{
				return _storeColor.Minimum;
			}
			set
			{
				_storeColor.Minimum = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's color unit primary allocation maximum.
		/// </summary>
		internal int StoreColorUnitPrimaryMaximum
		{
			get
			{
				return _storeColor.PrimaryMaximum;
			}
			set
			{
				_storeColor.PrimaryMaximum = value;
			}
		}

		/// <summary>
		/// Gets or sets store's color units allocated via an auto allocation process.
		/// </summary>
		internal int StoreColorUnitsAllocatedByAuto
		{
			get
			{
				return _storeColor.QtyAllocatedByAuto;
			}
			set
			{
				_storeColor.QtyAllocatedByAuto = value;
			}
		}

		/// <summary>
		/// Gets or sets store's color units allocated by rule.
		/// </summary>
		internal int StoreColorUnitsAllocatedByRule
		{
			get
			{
				return _storeColor.QtyAllocatedByRule;
			}
			set
			{
				_storeColor.QtyAllocatedByRule = value;
			}
		}

		/// <summary>
		/// Gets or sets the chosen rule.
		/// </summary>
		internal eRuleType StoreColorChosenRuleType
		{
			get
			{
				return _storeColor.ChosenRuleType;
			}
			set
			{
				_storeColor.ChosenRuleType = value;
			}
		}


		/// <summary>
		/// Gets or sets store's color chosen rule layer ID.
		/// </summary>
		internal int StoreColorChosenRuleLayerID
		{
			get
			{
				return _storeColor.ChosenRuleLayerID;
			}
			set
			{
				_storeColor.ChosenRuleLayerID = value;
			}
		}


		/// <summary>
		/// Gets or sets store's color units allocated by the chosen rule.
		/// </summary>
		internal int StoreColorChosenRuleUnitsAllocated
		{
			get
			{
				return _storeColor.ChosenRuleQtyAllocated;
			}
			set
			{
				_storeColor.ChosenRuleQtyAllocated = value;
			}
		}


		/// <summary>
		/// Gets or sets Store Color LastNeedDay
		/// </summary>
		/// <remarks>
		/// DateTime Format
		/// </remarks>
		internal DateTime StoreColorLastNeedDay
		{
			get
			{
				return _storeColor.StoreLastNeedDay;
			}
			set
			{
				_storeColor.StoreLastNeedDay = value;
			}
		}

		/// <summary>
		/// Gets or sets Store's Color Unit Need Before.
		/// </summary>
		internal int StoreColorUnitNeedBefore
		{
			get
			{
				return _storeColor.StoreUnitNeedBefore;
			}
			set
			{
				_storeColor.StoreUnitNeedBefore = value;
			}
		}

		/// <summary>
		/// Gets or sets store's Color PercentNeedBefore
		/// </summary>
		internal double StoreColorPercentNeedBefore
		{
			get
			{
				return _storeColor.StorePercentNeedBefore;
			}
			set
			{
				_storeColor.StorePercentNeedBefore = value;
			}
		}


		/// <summary>
		/// Gets or sets StoreColorShippingStarted audit flag.
		/// </summary>
		internal bool StoreColorShippingStarted
		{
			get
			{
				return _storeColorShipStatus.ShippingStarted;
			}
			set
			{
				if (StoreColorShippingStarted != value)
				{
					_storeColorShipStatus.ShippingStarted = value;
					this.StoreColorIsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets StoreColorShippingComplete audit flag.
		/// </summary>
		internal bool StoreColorShippingComplete
		{
			get
			{
				return _storeColorShipStatus.ShippingComplete;
			}
			set
			{
				if (StoreColorShippingComplete != value)
				{
					_storeColorShipStatus.ShippingComplete = value;
					this.StoreColorIsChanged = true;
				}
			}
		}


		/// <summary>
		/// Gets or sets StoreColorUnitsShipped
		/// </summary>
		internal int StoreColorUnitsShipped
		{
			get
			{
				return _colorUnitsShipped;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_QtyShippedCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_QtyShippedCannotBeNeg));
				}
				else
				{
					if (StoreColorUnitsShipped != value)
					{
						_colorUnitsShipped = value;
						this.StoreColorIsChanged = true;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets TotalSizeUnitsAllocated.
		/// </summary>
		/// <remarks>
		/// Total size units allocated across all sizes in the color for a store.
		/// </remarks>
		internal int TotalSizeUnitsAllocated
		{
			get
			{
				return _storeTotalSizeUnitsAllocated;
			}
			set
			{
				if (value <0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                        + ": TotalSizeUnitsAllocated in " + GetType().Name);        // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                }
				else
				{
					_storeTotalSizeUnitsAllocated = value;
		
				}
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets or sets TotalSizeItemUnitsAllocated.
        /// </summary>
        /// <remarks>
        /// Total size item units allocated across all sizes in the color for a store.
        /// </remarks>
        internal int TotalSizeItemUnitsAllocated
        {
            get
            {
                return _storeTotalSizeItemUnitsAllocated;
            }
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                        + ": TotalSizeItemUnitsAllocated in " + GetType().Name);     // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                }
                else
                {
                    _storeTotalSizeItemUnitsAllocated = value;

                }
            }
        }
        internal int TotalSizeItemUnitsManuallyAllocated
        {
            get
            {
                return _storeTotalSizeItemUnitsManuallyAllocated;
            }
            set
            {
                _storeTotalSizeItemUnitsManuallyAllocated = value;
            }
        }
        /// <summary>
        /// Gets or sets TotalSizeImoUnitsAllocated.
        /// </summary>
        /// <remarks>
        /// Total size IMO units allocated across all sizes in the color for a store.
        /// </remarks>
        internal int TotalSizeImoUnitsAllocated
        {
            get
            {
                return _storeTotalSizeImoUnitsAllocated;
            }
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                        + ": TotalSizeImoUnitsAllocated in " + GetType().Name);     // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                }
                else
                {
                    _storeTotalSizeImoUnitsAllocated = value;

                }
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1

		/// <summary>
		/// Gets or sets ReCalcStoreSizePctToColorTotal flag
		/// </summary>
		internal bool ReCalcStoreSizePctToColorTotal
		{
			get
			{
				return this._recalcStoreSizePctToColorTotal;
			}
			set
			{
				this._recalcStoreSizePctToColorTotal = value;
			}
		}

		/// <summary>
		/// Gets or set the store's total size allocation percent to store color total allocation
		/// </summary>
		internal double StoreTotalSizePctToColorTotal
		{
			get
			{
				return this._storeTotalSizePctToColorTotal;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_al_StoreSizePctToColorTotalCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_al_StoreSizePctToColorTotalCannotBeNeg));
				}
				this._storeTotalSizePctToColorTotal = value;
			}
		}


		//========
		// METHODS
		//========
	}
	#endregion Store Color Allocated

	#region Store Size Allocated
	/// <summary>
	/// Fields to track and audit a store's width size allocations.
	/// </summary>
	internal struct StoreSizeAllocated
	{
		//=======
		// FIELDS
		//=======
		private StoreAllocatedBaseBin _storeSize;
		private ShippingStatusFlags _storeSizeShipStatus;
		private int _sizeUnitsShipped;
		private double _storeSizePctToColorTotal;
        private int _itemMinimum;  // TT#246 - MD - JEllis - AnF VSW In-Store Minimum
        private int _itemIdealMinimum; // TT#246 - MD - JEllis - AnF VSW In Store Minimum phase 2
		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets or sets IsChanged database update flag
		/// </summary>
		internal bool StoreSizeIsChanged
		{
			get
			{
				return _storeSize.IsChanged;
			}
			set
			{
				_storeSize.IsChanged = value;
			}
		}

		/// <summary>
		/// Gets or sets IsNew database update flag
		/// </summary>
		internal bool StoreSizeIsNew
		{
			get 
			{
				return _storeSize.IsNew;
			}
			set
			{
				_storeSize.IsNew = value;
			}
		}

		/// <summary>
		/// Gets or sets all detail audit flags simultaneously.
		/// </summary>
        //internal ushort AllDetailAuditFlags // TT#488 - MD - Jellis - Group ALlocation
        internal uint AllDetailAuditFlags     // TT#488 - MD - Jellis - Group Allocation
        {
			get
			{
				return _storeSize.AllDetailAuditFlags;
			}
			set
			{
				_storeSize.AllDetailAuditFlags = value;
			}
		}

		/// <summary>
		/// Gets or sets all ship status flags simultaneously.
		/// </summary>
		internal byte AllShipStatusFlags
		{
			get
			{
				return _storeSizeShipStatus.AllFlags;
			}
			set
			{
				_storeSizeShipStatus.AllFlags = value;
			}
		}

		/// <summary>
		/// Gets or sets StoreSizeIsManuallyAllocated audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates that the user has manually specified the size units allocated
		/// </remarks>
		internal bool StoreSizeIsManuallyAllocated 
		{
			get
			{
				return _storeSize.IsManuallyAllocated;
			}
			set
			{
				_storeSize.IsManuallyAllocated = value;
			}
		}
        
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
        /// <summary>
        /// Gets or sets StoreSizeItemIsManuallyAllocated audit flag.
        /// </summary>
        /// <remarks>
        /// True indicates that the user has manually specified the size item units allocated
        /// </remarks>
        internal bool StoreSizeItemIsManuallyAllocated     // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        {
            get
            {
                return _storeSize.ItemIsManuallyAllocated; // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F 
            }
            set
            {
                _storeSize.ItemIsManuallyAllocated = value; // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 2

		/// <summary>
		/// Gets or sets StoreSizeWasAutoAllocated audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates that an auto allocation process affected the store's size allocation.
		/// </remarks>
		internal bool StoreSizeWasAutoAllocated
		{
			get
			{
				return _storeSize.WasAutoAllocated;
			}
			set
			{
				_storeSize.WasAutoAllocated = value;
			}
		}
		
		/// <summary>
		/// Gets or sets StoreSizeChosenRuleAcceptedByHeader audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the chosen rule for the store's size component was accepted on Header.
		/// </remarks>
		internal bool StoreSizeChosenRuleAcceptedByHeader // TT#488 - MD - Jellis - Group Allocation
		{
			get
			{
				return _storeSize.ChosenRuleAcceptedByHeader; // TT#488 - MD - Jellis - Group Allocation
			}
			set
			{
				_storeSize.ChosenRuleAcceptedByHeader = value; // TT#488  - MD - Jellis - Group Allocation
			}
		}
	
        // begin TT#488- MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets StoreSizeChosenRuleAcceptedByGroup audit flag.
        /// </summary>
        /// <remarks>
        /// True indicates the chosen rule for the store's size component was accepted in Group Allocation.
        /// </remarks>
        internal bool StoreSizeChosenRuleAcceptedByGroup
        {
            get
            {
                return _storeSize.ChosenRuleAcceptedByGroup; 
            }
            set
            {
                _storeSize.ChosenRuleAcceptedByGroup = value; 
            }
        }
        // end TT#488 - MD - Jellis - Group Allocation

		/// <summary>
		/// Gets or sets StoreSizeOut audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the store cannot be given any allocation on the size Component.
		/// </remarks>
		internal bool StoreSizeOut
		{
			get
			{
				return _storeSize.Out;
			}
			set
			{
				_storeSize.Out = value;
			}
		}

        // begin TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
        ///// <summary>
        ///// Gets or sets StoreSizeLocked audit flag. 
        ///// </summary>
        ///// <remarks>
        ///// True prevents the store's size allocation from changing via an auto allocation
        ///// process.  This lock is permanent and saved on the database.
        ///// </remarks>
        //internal bool StoreSizeLocked
        //{
        //    get
        //    {
        //        return _storeSize.Locked;
        //    }
        //    set
        //    {
        //        _storeSize.Locked = value;
        //    }
        //}
        // end TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
		 
        // begin TT#246 - MD - JEllis - AnF VSW In-Store Minimum
        internal int StoreSizeItemMinimum
        {
            get
            {
                return _itemMinimum;
            }
            set
            {
                if (value < 0)
                {
                    throw new MIDException (eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "StoreSizeItemMinimum"))  // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                    //MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg)     // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                    //+ ": StoreSizeItemMinimum in " + GetType().Name);   // TT#891 - MD - Jellis - Group Allocation Need Gets Error  
                }
                _itemMinimum = value;
            }
        }
        // begin TT#246 - MD - JEllis - AnF VSW In Store Minimum phase 2
        internal int StoreSizeItemIdealMinimum
        {
            get
            {
                return _itemIdealMinimum;
            }
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "StoreSizeItemIdealMinimum"))  // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                    //MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg)      // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                    //+ ": StoreSizeItemIdealMinimum in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error

                }
                _itemIdealMinimum = value;
            }
        }
        // end TT#246 - MD - JEllis - AnF VSW In Store Minimum phase 2
        // end TT#246 - MD - JEllis - AnF VSW In-Store Minimum

		// begin TT#59 Implement Temp Locks
        // /// <summary>
		// /// Gets or sets StoreSizeTempLock audit flag.
		// /// </summary>
		// /// <remarks>
		// /// True prevents the store's size allocation from changing via an auto allocation.
		// /// </remarks>
		//internal bool StoreSizeTempLock
		//{
		//	get 
		//	{
		//		return _storeSize.TempLock;
		//	}
		//	set
		//	{
		//		_storeSize.TempLock = value;
		//	}
		//}
        // end TT#59 Implement Temp Locks

		/// <summary>
		/// Gets or sets StoreSizeHadNeed audit flag.
		/// </summary>
		/// <remarks>
		/// True indicates the store's allocation was based on need.</remarks>
		internal bool StoreSizeHadNeed
		{
			get
			{
				return _storeSize.HadNeed;
			}
			set
			{
				_storeSize.HadNeed = value;
			}
		}

		/// <summary>
		/// Gets or sets StoreSizeFilledSizeHole audit flag
		/// </summary>
		internal bool StoreSizeFilledSizeHole
		{
			get
			{
				return _storeSize.FilledSizeHole;
			}
			set
			{
				_storeSize.FilledSizeHole = value;
			}
		}

		/// <summary>
		/// Gets or sets StoreSizeAllocationFromBottomUpSize audit flag
		/// </summary>
		internal bool StoreSizeAllocationFromBottomUpSize
		{
			get
			{
				return _storeSize.AllocationFromBottomUpSize;
			}
			set
			{
				_storeSize.AllocationFromBottomUpSize = value;
			}
		}
        
		/// <summary>
		/// Gets or sets StoreSizeAllocationFromBBulkSizeBreakOut audit flag
		/// </summary>
		internal bool StoreSizeAllocationFromBulkSizeBreakOut
		{
			get
			{
				return _storeSize.AllocationFromBulkSizeBreakOut;
			}
			set
			{
				_storeSize.AllocationFromBulkSizeBreakOut = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's SizeUnitsAllocated.
		/// </summary>
		internal int StoreSizeUnitsAllocated
		{
			get
			{
				return _storeSize.QtyAllocated;
			}
			set
			{
				_storeSize.QtyAllocated = value;
			}
		}
		
        // begin TT#1401 - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets or sets the store's SizeItemUnitsAllocated.
        /// </summary>
        internal int StoreSizeItemUnitsAllocated
        {
            get
            {
                return _storeSize.ItemQtyAllocated;
            }
            set
            {
                _storeSize.ItemQtyAllocated = value;
            }
        }
        /// <summary>
        /// Gets or sets the store's SizeImoUnitsAllocated.
        /// </summary>
        internal int StoreSizeImoUnitsAllocated
        {
            get
            {
                return _storeSize.ImoQtyAllocated;
            }
            // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
            //set
            //{
            //    _storeSize.ImoQtyAllocated = value;
            //}
            // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        }
        // end TT#1401 - Urban Reservation Stores pt 1


		/// <summary>
		/// Gets or sets store Size Rule allocation from parent flag
		/// </summary>
		internal bool StoreSizeRuleAllocationFromParentComponent
		{
			get
			{
				return _storeSize.RuleAllocationFromParentComponent;
			}
			set
			{
				_storeSize.RuleAllocationFromParentComponent = value;
			}
		}

		/// <summary>
		/// Gets or sets store Size Rule allocation from child flag
		/// </summary>
		internal bool StoreSizeRuleAllocationFromChildComponent
		{
			get
			{
				return _storeSize.RuleAllocationFromChildComponent;
			}
			set
			{
				_storeSize.RuleAllocationFromChildComponent = value;
			}
		}

		/// <summary>
		/// Gets or sets store Size Rule allocation from chosen Rule flag
		/// </summary>
		internal bool StoreSizeRuleAllocationFromChosenRule
		{
			get
			{
				return _storeSize.RuleAllocationFromChosenRule;
			}
			set
			{
				_storeSize.RuleAllocationFromChosenRule = value;
			}
		}

        // begin MID Track 4448 Anf Audit Enhancement
		/// <summary>
		/// Gets or sets store Size Allocation Modified After Multi Header Split Audit flag
		/// </summary>
		internal bool StoreSizeAllocationModifiedAfterMultiHeaderSplit
		{
			get
			{
				return _storeSize.AllocationModifiedAfterMultiHeaderSplit;
			}
			set
			{
				_storeSize.AllocationModifiedAfterMultiHeaderSplit = value;
			}
		}
		// end MID Track 4448 Anf Audit Enhancemnt

		/// <summary>
		/// Gets or sets the store's SizeOrigUnitsAllocated.
		/// </summary>
		internal int StoreSizeOrigUnitsAllocated
		{
			get
			{
				return _storeSize.OriginalQtyAllocated;
			}
			set
			{
				_storeSize.OriginalQtyAllocated = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's size unit allocation maximum.
		/// </summary>
		internal int StoreSizeUnitMaximum
		{
			get
			{
				return _storeSize.Maximum;
			}
			set
			{
				_storeSize.Maximum = value;
			}
		}

		/// <summary>
		/// Gets the largest size maximum.
		/// </summary>
		internal int StoreSizeLargestMaximum
		{
			get
			{
				return _storeSize.LargestMaximum;
			}
		}

		/// <summary>
		/// Gets or sets the store's size unit allocation minimum.
		/// </summary>
		internal int StoreSizeUnitMinimum
		{
			get
			{
				return _storeSize.Minimum;
			}
			set
			{
				_storeSize.Minimum = value;
			}
		}

		/// <summary>
		/// Gets or sets the store's size unit primary allocation maximum.
		/// </summary>
		internal int StoreSizeUnitPrimaryMaximum
		{
			get
			{
				return _storeSize.PrimaryMaximum;
			}
			set
			{
				_storeSize.PrimaryMaximum = value;
			}
		}

		/// <summary>
		/// Gets or sets store's size units allocated via an auto allocation process.
		/// </summary>
		internal int StoreSizeUnitsAllocatedByAuto
		{
			get
			{
				return _storeSize.QtyAllocatedByAuto;
			}
			set
			{
				_storeSize.QtyAllocatedByAuto = value;
			}
		}

		/// <summary>
		/// Gets or sets StoreSizeShippingStarted audit flag.
		/// </summary>
		internal bool StoreSizeShippingStarted
		{
			get
			{
				return _storeSizeShipStatus.ShippingStarted;
			}
			set
			{
				if (StoreSizeShippingStarted != value)
				{
					_storeSizeShipStatus.ShippingStarted = value;
					this.StoreSizeIsChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets StoreSizeShippingComplete audit flag.
		/// </summary>
		internal bool StoreSizeShippingComplete
		{
			get
			{
				return _storeSizeShipStatus.ShippingComplete;
			}
			set
			{
				if (StoreSizeShippingComplete != value)
				{
					_storeSizeShipStatus.ShippingComplete = value;
					this.StoreSizeIsChanged = true;
				}
			}
		}


		/// <summary>
		/// Gets or sets StoreSizeUnitsShipped
		/// </summary>
		internal int StoreSizeUnitsShipped
		{
			get
			{
				return _sizeUnitsShipped;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_QtyShippedCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_QtyShippedCannotBeNeg));
				}
				else
				{
					if (StoreSizeUnitsShipped != value)
					{
						_sizeUnitsShipped = value;
						this.StoreSizeIsChanged = true;
					}
				}
			}
		}
		/// <summary>
		/// Gets or set the store's size allocation percent to store color total allocation
		/// </summary>
		internal double StoreSizePctToColorTotal
		{
			get
			{
				return this._storeSizePctToColorTotal;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_al_StoreSizePctToColorTotalCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_al_StoreSizePctToColorTotalCannotBeNeg));
				}
				this._storeSizePctToColorTotal = value;
			}
		}
		//========
		// METHODS
		//========
	}
	#endregion Store Size Allocated

    #region Obsolete Code
    // begin TT#4345 - MD - JEllis - GA VSW calculated incorrectly
    // moved to IntransitReader
    //// begin TT#1287 - JEllis - Inventory Minimum Maximum
    
    //#region StoreSalesITHorizon
    //[Serializable]
    //public struct StoreSalesITHorizon
    //{
    //    private Horizon_ID _horizon_ID; // TT#4345 - MD - Jellis - GA VSW calculated incorrectly
    //    private int[] _storeRIDList;
    //    private int[] _horizonStart;
    //    private int[] _horizonEnd;
    //    //public StoreSalesITHorizon(int[] aStoreRIDList, int[] aHorizonStart, int[] aHorizonEnd)  // TT#4345 - MD - Jellis - GA VSW calculated incorrectly
    //    public StoreSalesITHorizon(Horizon_ID aHorizon_ID, int[] aStoreRIDList, int[] aHorizonStart, int[] aHorizonEnd)  // TT#4345 - MD - Jellis - GA VSW calculated incorrectly
    //    {
    //        _horizon_ID = aHorizon_ID;  // TT#4345 - MD - Jellis - GA VSW calculated incorrectly
    //        _storeRIDList = aStoreRIDList;
    //        _horizonStart = aHorizonStart;
    //        _horizonEnd = aHorizonEnd;
    //    }
    //    // begin TT#4345 - MD - Jellis - GA VSW calculated incorrectly
    //    /// <summary>
    //    /// Gets ID associated with this StoreSalesITHorizon
    //    /// </summary>
    //    public Horizon_ID HorizonID
    //    {
    //        get { return _horizon_ID; }
    //    }
    //    // end TT#4345 - MD - Jellis - GA VSW calculated incorrectly
    //    public int[] StoreRIDList
    //    {
    //        get { return _storeRIDList; }
    //    }
    //    public int[] StoreHorizonStart
    //    {
    //        get { return _horizonStart; }
    //    }
    //    public int[] StoreHorizonEnd
    //    {
    //        get { return _horizonEnd; }
    //    }
    //}
    //#endregion StoreSalesITHorizon
    //// end TT#1287 - JEllis - Inventory Minimum Maximum
    // end TT#4345 - MD - JEllis - GA VSW calculated incorrectly
    #endregion Obsolete Code

    #region Subtotal Allocated Base Bin
    /// <summary>
	/// Store Subtotal allocation basic tracking information.
	/// </summary>
	internal struct SubtotalAllocatedBaseBin
	{
		//=======
		// FIELDS
		//=======
		private int _qtyAllocated;
		private int _origQtyAllocated;
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        private int _itemQtyAllocated;
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1
		private MinMaxAllocationBin _minMax;
		private int _primaryMaximum;
//		private bool _out;
//		private bool _locked;
//		private bool _tempLock;

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets or sets QtyAllocated to store.
		/// </summary>
		/// <remarks>
		/// QtyAllocated must be a non-negative integer.
		/// </remarks>
		internal int QtyAllocated
		{
			get
			{
				return _qtyAllocated;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                        + ": QtyAllocated in " + GetType().Name);               // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                }
				else
				{
					_qtyAllocated = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets Original QtyAllocated to store.
		/// </summary>
		/// <remarks>
		/// Original QtyAllocated must be a non-negative integer.
		/// </remarks>
		internal int OriginalQtyAllocated
		{
			get
			{
				return _origQtyAllocated;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                        + ": OriginalQtyAllocated in " + GetType().Name);               // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                }
				else
				{
					_origQtyAllocated = value;
				}
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt1
        /// <summary>
        /// Gets or sets ItemQtyAllocated to store.
        /// </summary>
        /// <remarks>
        /// ItemQtyAllocated must be a non-negative integer.
        /// </remarks>
        internal int ItemQtyAllocated
        {
            get
            {
                return _itemQtyAllocated;
            }
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                        + ": ItemQtyAllocated in " + GetType().Name);               // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                }
                else
                {
                    _itemQtyAllocated = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets ImoQtyAllocated to store.
        /// </summary>
        /// <remarks>
        /// ImoQtyAllocated must be a non-negative integer.
        /// </remarks>
        internal int ImoQtyAllocated
        {
            get
            {
                // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
                int imoQtyAllocated =
                    _qtyAllocated
                    - ItemQtyAllocated;
                if (imoQtyAllocated < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)
                        + ": ImoQtyAllocated in " + GetType().Name);
                }
                return imoQtyAllocated;
            }
            //    return _imoQtyAllocated;
            //}
            //set
            //{
            //    if (value < 0)
            //    {
            //        throw new MIDException(eErrorLevel.severe,
            //            (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
            //            MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg));
            //    }
            //    else
            //    {
            //        _imoQtyAllocated = value;
            //    }
            //}
            // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt1

		/// <summary>
		/// Gets or sets the allocation maximum.
		/// </summary>
		internal int Maximum
		{
			get
			{
				return _minMax.Maximum;
			}
			set
			{
				_minMax.SetMaximum(value);
			}
		}

		/// <summary>
		/// Gets the largest possible maximum.
		/// </summary>
		internal int LargestMaximum
		{
			get
			{
				return _minMax.LargestMaximum;
			}
		}

		/// <summary>
		/// Gets or sets the allocation minimum.
		/// </summary>
		internal int Minimum
		{
			get
			{
				return _minMax.Minimum;
			}
			set
			{
				_minMax.SetMinimum(value);
			}
		}
		
		/// <summary>
		/// Gets or sets the primary maximum.
		/// </summary>
		internal int PrimaryMaximum
		{
			get
			{
				return _primaryMaximum;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_PMaxAllocationCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_PMaxAllocationCannotBeNeg));
				}
				else
				{
					_primaryMaximum = value;
				}
			}
		}

		//========
		// METHODS
		//========
	}
	#endregion Subtotal Allocated Base Bin

	#region Subtotal Total Allocated
	/// <summary>
	/// Fields to track a store's sutotal total allocations.
	/// </summary>
	internal struct SubtotalTotalAllocated
	{
		//=======
		// FIELDS
		//=======
		private SubtotalAllocatedBaseBin _subtotalTotal;
        private int _subtotalCapacityMaximum;
		private SubtotalAllocatedBaseBin _subtotalGenericTypeTotal;
		private int _subtotalTotalGenericUnitsAllocated;
        private int _subtotalTotalGenericItemUnitsAllocated; // TT#1401 - Jellis - Urban Reservation Stores pt 1
        private int _subtotalTotalGenericImoUnitsAllocated;  // TT#1401 - Jellis - Urban Reservation Stores pt 1
		private SubtotalAllocatedBaseBin _subtotalDetailTypeTotal;
		private int _subtotalTotalNonGenericUnitsAllocated;
        private int _subtotalTotalNonGenericItemUnitsAllocated; // TT#1401 - Jellis - Urban Reservation Stores pt 1
        private int _subtotalTotalNonGenericImoUnitsAllocated;  // TT#1401 - Jellis - Urban Reservation Stores pt 1
        private SubtotalAllocatedBaseBin _subtotalBulkTotal;
        //private int _subtotalBulkItemUnitsAllocated; // TT#1401 - Jellis - Urban Reservation Stores pt 1  // TT#246 - MD - Jellis - VSW Size In store Minimums pt 2
        //private int _subtotalBulkImoUnitsAllocated;  // TT#1401 - Jellis - Urban Reservation Stores pt 1  // TT#246 - MD - Jellis - VSW Size In store Minimums pt 2
        private int _subtotalBulkColorTotalUnitsAllocated;
        private int _subtotalBulkColorTotalItemUnitsAllocated; // TT#1401 - Jellis - Urban Reservation Stores pt 1
        private int _subtotalBulkColorTotalImoUnitsAllocated;  // TT#1401 - Jellis - Urban Reservation Stores pt 1
        //=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets or sets the subtotal's TotalUnitsAllocated.
		/// </summary>
		internal int SubtotalTotalUnitsAllocated
		{
			get
			{
				return _subtotalTotal.QtyAllocated;
			}
			set
			{
				_subtotalTotal.QtyAllocated = value;
			}
		}

		/// <summary>
		/// Gets or sets the subtotal's TotalOrigUnitsAllocated.
		/// </summary>
		internal int SubtotalTotalOrigUnitsAllocated
		{
			get
			{
				return _subtotalTotal.OriginalQtyAllocated;
			}
			set
			{
				_subtotalTotal.OriginalQtyAllocated = value;
			}
		}
		
        // begin TT#1401 - Jellis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets or sets the subtotal's TotalItemUnitsAllocated.
        /// </summary>
        internal int SubtotalTotalItemUnitsAllocated
        {
            get
            {
                return _subtotalTotal.ItemQtyAllocated;
            }
            set
            {
                _subtotalTotal.ItemQtyAllocated = value;
            }
        }

        /// <summary>
        /// Gets or sets the subtotal's TotalImoUnitsAllocated.
        /// </summary>
        internal int SubtotalTotalImoUnitsAllocated
        {
            get
            {
                return _subtotalTotal.ImoQtyAllocated;
            }
            // begin TT#1401 - JEllis - Virtual Store Warehouse pt 29
            //set
            //{
            //    _subtotalTotal.ImoQtyAllocated = value;
            //}
            // end TT#1401 - JEllis - Virtual Store Warehouse pt 29
        }
        // end TT31401 - Jellis - Urban Reservaton Stores pt 1

		/// <summary>
		/// Gets largest maximum value.
		/// </summary>
		internal int SubtotalLargestMaximum
		{
			get
			{
				return _subtotalTotal.LargestMaximum;
			}
		}

		/// <summary>
		/// Gets or sets the subtotal's Total allocation maximum.
		/// </summary>
		internal int SubtotalTotalMaximum
		{
			get
			{
				return _subtotalTotal.Maximum;
			}
			set
			{
				_subtotalTotal.Maximum = value;
			}
		}

		/// <summary>
		/// Gets or sets the subtotal's Total allocation minimum.
		/// </summary>
		internal int SubtotalTotalMinimum
		{
			get
			{
				return _subtotalTotal.Minimum;
			}
			set
			{
				_subtotalTotal.Minimum = value;
			}
		}

		/// <summary>
		/// Gets or sets the subtotal's total primary allocation maximum.
		/// </summary>
		internal int SubtotalTotalPrimaryMaximum
		{
			get
			{
				return _subtotalTotal.PrimaryMaximum;
			}
			set
			{
				_subtotalTotal.PrimaryMaximum = value;
			}
		}

		/// <summary>
		/// Gets or sets the subtotal's total Capacity allocation maximum.
		/// </summary>
		internal int SubtotalCapacityMaximum
		{
			get
			{
				return _subtotalCapacityMaximum;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_CapacityCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_CapacityCannotBeNeg));
				}
				_subtotalCapacityMaximum = value;
			}
		}

		/// <summary>
		/// Gets SubtotalTypeUnitsAllocated.
		/// </summary>
		/// <remarks>
		/// </remarks>
		internal int SubtotalTypeUnitsAllocated
		{
			get
			{
				return SubtotalGenericTypeUnitsAllocated + SubtotalDetailTypeUnitsAllocated;
			}
		}

		/// <summary>
		/// Gets SubtotalTypeOrigUnitsAllocated.
		/// </summary>
		/// <remarks>
		/// </remarks>
		internal int SubtotalTypeOrigUnitsAllocated
		{
			get
			{
				return SubtotalGenericTypeOrigUnitsAllocated + SubtotalDetailTypeOrigUnitsAllocated;
			}
		}

		/// <summary>
		/// Gets or sets the subtotal's GenericTypeUnitsAllocated.
		/// </summary>
		internal int SubtotalGenericTypeUnitsAllocated
		{
			get
			{
				return _subtotalGenericTypeTotal.QtyAllocated;
			}
			set
			{
				_subtotalGenericTypeTotal.QtyAllocated = value;
			}
		}

		/// <summary>
		/// Gets or sets the subtotal's GenericTypeOrigUnitsAllocated.
		/// </summary>
		internal int SubtotalGenericTypeOrigUnitsAllocated
		{
			get
			{
				return _subtotalGenericTypeTotal.OriginalQtyAllocated;
			}
			set
			{
				_subtotalGenericTypeTotal.OriginalQtyAllocated = value;
			}
		}

        // begin TT#1401 - Jellis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets SubtotalTypeItemUnitsAllocated.
        /// </summary>
        /// <remarks>
        /// </remarks>
        internal int SubtotalTypeItemUnitsAllocated
        {
            get
            {
                return SubtotalGenericTypeItemUnitsAllocated + SubtotalDetailTypeItemUnitsAllocated;
            }
        }

        /// <summary>
        /// Gets or sets the subtotal's GenericTypeItemUnitsAllocated.
        /// </summary>
        internal int SubtotalGenericTypeItemUnitsAllocated
        {
            get
            {
                return _subtotalGenericTypeTotal.ItemQtyAllocated;
            }
            set
            {
                _subtotalGenericTypeTotal.ItemQtyAllocated = value;
            }
        }
        /// <summary>
        /// Gets SubtotalTypeImoUnitsAllocated.
        /// </summary>
        /// <remarks>
        /// </remarks>
        internal int SubtotalTypeImoUnitsAllocated
        {
            get
            {
                return SubtotalGenericTypeImoUnitsAllocated + SubtotalDetailTypeImoUnitsAllocated;
            }
        }

        /// <summary>
        /// Gets or sets the subtotal's GenericTypeImoUnitsAllocated.
        /// </summary>
        internal int SubtotalGenericTypeImoUnitsAllocated
        {
            get
            {
                return _subtotalGenericTypeTotal.ImoQtyAllocated;
            }
            // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
            //set
            //{
            //    _subtotalGenericTypeTotal.ImoQtyAllocated = value;
            //}
            // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        }
        // end TT#1401 - Jellis - Urban Reservation Stores pt 1
		
		/// <summary>
		/// Gets or sets the subtotal's GenericType allocation maximum.
		/// </summary>
		internal int SubtotalGenericTypeMaximum
		{
			get
			{
				return _subtotalGenericTypeTotal.Maximum;
			}
			set
			{
				_subtotalGenericTypeTotal.Maximum = value;
			}
		}

		/// <summary>
		/// Gets or sets the subtotal's GenericType allocation minimum.
		/// </summary>
		internal int SubtotalGenericTypeMinimum
		{
			get
			{
				return _subtotalGenericTypeTotal.Minimum;
			}
			set
			{
				_subtotalGenericTypeTotal.Minimum = value;
			}
    	}

		/// <summary>
		/// Gets or sets the subtotal's DetailTypeUnitsAllocated.
		/// </summary>
		internal int SubtotalDetailTypeUnitsAllocated
		{
			get
			{
				return _subtotalDetailTypeTotal.QtyAllocated;
			}
			set
			{
				_subtotalDetailTypeTotal.QtyAllocated = value;
			}
		}

		/// <summary>
		/// Gets or sets the subtotal's DetailTypeOrigUnitsAllocated.
		/// </summary>
		internal int SubtotalDetailTypeOrigUnitsAllocated
		{
			get
			{
				return _subtotalDetailTypeTotal.OriginalQtyAllocated;
			}
			set
			{
				_subtotalDetailTypeTotal.OriginalQtyAllocated = value;
			}
		}
		
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets or sets the subtotal's DetailTypeItemUnitsAllocated.
        /// </summary>
        internal int SubtotalDetailTypeItemUnitsAllocated
        {
            get
            {
                return _subtotalDetailTypeTotal.ItemQtyAllocated;
            }
            set
            {
                _subtotalDetailTypeTotal.ItemQtyAllocated = value;
            }
        }
        /// <summary>
        /// Gets or sets the subtotal's DetailTypeImoUnitsAllocated.
        /// </summary>
        internal int SubtotalDetailTypeImoUnitsAllocated
        {
            get
            {
                return _subtotalDetailTypeTotal.ImoQtyAllocated;
            }
            // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
            //set
            //{
            //    _subtotalDetailTypeTotal.ImoQtyAllocated = value;
            //}
            // end TTT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1

		/// <summary>
		/// Gets or sets the subtotal's DetailType allocation maximum.
		/// </summary>
		internal int SubtotalDetailTypeMaximum
		{
			get
			{
				return _subtotalDetailTypeTotal.Maximum;
			}
			set
			{
				_subtotalDetailTypeTotal.Maximum = value;
			}
		}

		/// <summary>
		/// Gets or sets the subtotal's DetailType allocation minimum.
		/// </summary>
		internal int SubtotalDetailTypeMinimum
		{
			get
			{
				return _subtotalDetailTypeTotal.Minimum;
			}
			set
			{
				_subtotalDetailTypeTotal.Minimum = value;
			}
		}

		/// <summary>
		/// Gets or sets the subtotal's DetailType primary allocation maximum.
		/// </summary>
		internal int SubtotalDetailTypePrimaryMaximum
		{
			get
			{
				return _subtotalDetailTypeTotal.PrimaryMaximum;
			}
			set
			{
				_subtotalDetailTypeTotal.PrimaryMaximum = value;
			}
		}

		/// <summary>
		/// Gets or sets subtotal's total generic pack units allocated.
		/// </summary>
		/// <remarks>
		/// Accumulated generic pack units allocated.
		/// </remarks>
		internal int SubtotalTotalGenericUnitsAllocated
		{
			get
			{
				return _subtotalTotalGenericUnitsAllocated;
			}
			set
			{
				_subtotalTotalGenericUnitsAllocated = value;
			}
		}

		/// <summary>
		/// Gets or sets SubtotalTotalNonGenericUnitsAllocated.
		/// </summary>
		/// <remarks>
		/// Accumulated non-generic pack units allocated
		/// </remarks>
		internal int SubtotalTotalNonGenericUnitsAllocated
		{
			get
			{
				return _subtotalTotalNonGenericUnitsAllocated;
			}
			set
			{
				_subtotalTotalNonGenericUnitsAllocated = value;
			}
		}

		/// <summary>
		/// Gets or sets the subtotal's BulkUnitsAllocated.
		/// </summary>
		internal int SubtotalBulkUnitsAllocated
		{
			get
			{
				return _subtotalBulkTotal.QtyAllocated;
			}
			set
			{
				_subtotalBulkTotal.QtyAllocated = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the subtotal's BulkOrigUnitsAllocated.
		/// </summary>
		internal int SubtotalBulkOrigUnitsAllocated
		{
			get
			{
				return _subtotalBulkTotal.OriginalQtyAllocated;
			}
			set
			{
				_subtotalBulkTotal.OriginalQtyAllocated = value;
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets or sets subtotal's total generic pack Item units allocated.
        /// </summary>
        /// <remarks>
        /// Accumulated generic pack Item units allocated.
        /// </remarks>
        internal int SubtotalTotalGenericItemUnitsAllocated
        {
            get
            {
                return _subtotalTotalGenericItemUnitsAllocated;
            }
            set
            {
                _subtotalTotalGenericItemUnitsAllocated = value;
            }
        }

        /// <summary>
        /// Gets or sets SubtotalTotalNonGenericItemUnitsAllocated.
        /// </summary>
        /// <remarks>
        /// Accumulated non-generic pack item units allocated
        /// </remarks>
        internal int SubtotalTotalNonGenericItemUnitsAllocated
        {
            get
            {
                return _subtotalTotalNonGenericItemUnitsAllocated;
            }
            set
            {
                _subtotalTotalNonGenericItemUnitsAllocated = value;
            }
        }

        /// <summary>
        /// Gets or sets the subtotal's BulkItemUnitsAllocated.
        /// </summary>
        internal int SubtotalBulkItemUnitsAllocated
        {
            get
            {
                return _subtotalBulkTotal.ItemQtyAllocated;
            }
            set
            {
                _subtotalBulkTotal.ItemQtyAllocated = value;
            }
        }

        /// <summary>
        /// Gets or sets subtotal's total generic pack IMO units allocated.
        /// </summary>
        /// <remarks>
        /// Accumulated generic pack IMO units allocated.
        /// </remarks>
        internal int SubtotalTotalGenericImoUnitsAllocated
        {
            get
            {
                return _subtotalTotalGenericImoUnitsAllocated;
            }
            set
            {
                _subtotalTotalGenericImoUnitsAllocated = value;
            }
        }

        /// <summary>
        /// Gets or sets SubtotalTotalNonGenericImoUnitsAllocated.
        /// </summary>
        /// <remarks>
        /// Accumulated non-generic pack IMO units allocated
        /// </remarks>
        internal int SubtotalTotalNonGenericImoUnitsAllocated
        {
            get
            {
                return _subtotalTotalNonGenericImoUnitsAllocated;
            }
            set
            {
                _subtotalTotalNonGenericImoUnitsAllocated = value;
            }
        }

        /// <summary>
        /// Gets or sets the subtotal's BulkImoUnitsAllocated.
        /// </summary>
        internal int SubtotalBulkImoUnitsAllocated
        {
            get
            {
                return _subtotalBulkTotal.ImoQtyAllocated;
            }
            // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
            //set
            //{
            //    _subtotalBulkTotal.ImoQtyAllocated = value;
            //}
            // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 29
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1

		/// <summary>
		/// Gets or sets the subtotal's Bulk allocation maximum.
		/// </summary>
		internal int SubtotalBulkMaximum
		{
			get
			{
				return _subtotalBulkTotal.Maximum;
			}
			set
			{
				_subtotalBulkTotal.Maximum = value;
			}
		}

		/// <summary>
		/// Gets or sets the subtotal's Bulk allocation minimum.
		/// </summary>
		internal int SubtotalBulkMinimum
		{
			get
			{
				return _subtotalBulkTotal.Minimum;
			}
			set
			{
				_subtotalBulkTotal.Minimum = value;
			}
		}

		/// <summary>
		/// Gets or sets the subtotal's Bulk primary allocation maximum.
		/// </summary>
		internal int SubtotalBulkPrimaryMaximum
		{
			get
			{
				return _subtotalBulkTotal.PrimaryMaximum;
			}
			set
			{
				_subtotalBulkTotal.PrimaryMaximum = value;
			}
		}

		/// <summary>
		/// Gets or sets subtotal's bulk color total units allocated across all bulk colors.
		/// </summary>
		internal int SubtotalBulkColorTotalUnitsAllocated
		{
			get
			{
				return _subtotalBulkColorTotalUnitsAllocated;
			}
			set
			{
				_subtotalBulkColorTotalUnitsAllocated = value;
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 1
        /// <summary>
        /// Gets or sets subtotal's bulk color total item units allocated across all bulk colors.
        /// </summary>
        internal int SubtotalBulkColorTotalItemUnitsAllocated
        {
            get
            {
                return _subtotalBulkColorTotalItemUnitsAllocated;
            }
            set
            {
                _subtotalBulkColorTotalItemUnitsAllocated = value;
            }
        }
        /// <summary>
        /// Gets or sets subtotal's bulk color total IMO units allocated across all bulk colors.
        /// </summary>
        internal int SubtotalBulkColorTotalImoUnitsAllocated
        {
            get
            {
                return _subtotalBulkColorTotalImoUnitsAllocated;
            }
            set
            {
                _subtotalBulkColorTotalImoUnitsAllocated = value;
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 1


		//========
		// METHODS
		//========
	}
	#endregion Subtotal Total Allocated

	#region Ship Status
	/// <summary>
	/// Structure that identifies the quantity shipped to a store and whether the store is completely shipped.
	/// </summary>
	public struct ShipStore
	{
		//=======
		// FIELDS
		//=======
		private int _storeRID;
		private int _qtyShipped;
		private bool _shippingComplete;

		//=============
		// CONSTRUCTORS
		//=============
        /// <summary>
        /// Creates an instance of this structure
        /// </summary>
        /// <param name="aStoreRID">RID that identifies the store</param>
        /// <param name="aQtyShipped">Quantity shipped</param>
        /// <param name="aShippingComplete">True: store is completely shipped regardless of whether all units shipped; False: store is partially shipped.</param>
		public ShipStore(int aStoreRID, int aQtyShipped, bool aShippingComplete)
		{
			_storeRID = aStoreRID;
			_qtyShipped = aQtyShipped;
			_shippingComplete = aShippingComplete;
		}

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets Store RID.
		/// </summary>
		public int StoreRID
		{
			get
			{
				return _storeRID;
			}
		}

		/// <summary>
		/// Gets Quantity Shipped.
		/// </summary>
		public int QtyShipped
		{
			get
			{
				return _qtyShipped;
			}
		}
		
		/// <summary>
		/// Gets Shipping Complete Flag value
		/// </summary>
		public bool ShippingComplete
		{
			get
			{
				return _shippingComplete;
			}
		}
	}
	#endregion Ship Status

	// Begin MID Track 3326 Cannot Manually key size qty when no secondary size
	#region StoreHdrSizeBinProcesOrder
	/// <summary>
	/// Used to determine the order in which to process multiple HdrSizeBins
	/// </summary>
	public class StoreHdrSizeBinProcessOrder:IComparer
	{
		private int _storeIndex;
		internal StoreHdrSizeBinProcessOrder(int aStoreIndex)
		{
			_storeIndex = aStoreIndex;
		}
		public int Compare(object x, object y)
		{
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
			if (!(x is HdrSizeBin) || !(y is HdrSizeBin))
			{
				throw new MIDException(eErrorLevel.severe,
					(int)(eMIDTextCode.msg_al_CompareObjectMustBeHdrSizeBin),
					MIDText.GetText(eMIDTextCode.msg_al_CompareObjectMustBeHdrSizeBin));
			}

			int xUnits = ((HdrSizeBin)x).GetStoreSizeUnitsAllocated(_storeIndex);
			int yUnits = ((HdrSizeBin)y).GetStoreSizeUnitsAllocated(_storeIndex);
			if (xUnits < yUnits)
			{
				return -1;
			}
			if (xUnits > yUnits)
			{
				return +1;
			}
			xUnits = ((HdrSizeBin)x).SizeUnitsToAllocate;
			yUnits = ((HdrSizeBin)y).SizeUnitsToAllocate;
			if (xUnits < yUnits)
			{
				return -1;
			}
			if (xUnits > yUnits)
			{
				return +1;
			}
			xUnits = ((HdrSizeBin)x).SizeMultiple;
			yUnits = ((HdrSizeBin)y).SizeMultiple;

			if (xUnits < yUnits)
			{
				return -1;
			}
			if (xUnits > yUnits)
			{
				return +1;
			}
			return 0;
		}
	}
	#endregion StoreHdrSizeBinProcessOrder
	// End MID Track 3326 Cannot Manually key size qty when no secondary size

	// BEGIN TT#2225 - stodd - VSW ANF Enhancement (IMO)
	#region Store Ship Day
	/// <summary>
	/// Structure that identifies the ship day for a store.
	/// </summary>
	public struct StoreShipDay
	{
		//=======
		// FIELDS
		//=======
		private StoreProfile _store;
		private DateTime _shipDay;

		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of this structure
		/// </summary>
		/// <param name="aStore">a store profile</param>
		/// <param name="aShipDay">ship day</param>
		public StoreShipDay(StoreProfile aStore, DateTime aShipDay)
		{
			_store = aStore;
			_shipDay = aShipDay;
		}

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets Store RID.
		/// </summary>
		public StoreProfile Store
		{
			get
			{
				return _store;
			}
		}

		/// <summary>
		/// Gets Quantity Shipped.
		/// </summary>
		public DateTime ShipDay
		{
			get
			{
				return _shipDay;
			}
		}
	}
	#endregion Store Ship Day
	// END TT#2225 - stodd - VSW ANF Enhancement (IMO)
}
