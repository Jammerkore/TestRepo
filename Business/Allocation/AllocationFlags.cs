using System;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business.Allocation
{
	/// <summary>
	/// Allocation Store General Audit Flags track the general happenings to each store during the allocation process.
	/// </summary>
	/// <remarks>
	/// The Allocation Store General Audit Flags:
	/// <list type = "bullet">
	/// <item>PercentNeedLimitReached: true indicates that before or during allocation by need, the percent need limit was reached for this store</item>
	/// <item>CapacityMaximumReached: true indicates the capacity of the store was reached during allocation</item>
	/// <item>PrimaryMaximumReached: true indicates the primary maximum of the store was reached during allocation</item>
	/// <item>MayExceedMax: true indicates that the system was given permission to exceed grade maximums</item>
	/// <item>MayExceedPrimaryMax: true indicates that the system was given permission to exceed the primary allocation maximum</item>
	/// <item>MayExceedCapacityMax: true indicates that the system was given permission to exceed the capacity maximum for a store</item>
	/// <item>Eligible: true indicates that the store is/was eligible on its ship date.</item>
	/// <item>AllocationPriority: true indicates that the store has allocation (shipping) priority based on ship date and plan level</item>
	/// </list>
	/// </remarks>
	[Serializable]
	public struct AllocationStoreGeneralAuditFlags
	{
		//=======
		// FIELDS
		//=======
		ushort _flags;     // TT#1401 - JEllis - Urban Reservation Store pt 11

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets or Sets all flags
		/// </summary>
        internal ushort AllFlags // TT#1401 - JEllis - Urban Reservation Store pt 11
		{
			get
			{
				return _flags;
			}
			set
			{
				_flags = value;
			}
		}
		/// <summary>
		/// Gets or sets the PercentNeedLimitReached audit flag.
		/// </summary> 
		/// <remarks>
		/// When this flag is true, the store reached the percent need limit and was excluded from further need allocation decisions. Note: this flag is Set whenever the store is included in a need allocation calculation. So the flag represents the condition of the last time this store was included in a need allocation calculation.
		/// </remarks>
		public bool PercentNeedLimitReached
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationStoreGeneralAuditFlag.PercentNeedLimitReached);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationStoreGeneralAuditFlag.PercentNeedLimitReached, value);
			}
		}

		/// <summary>
		/// Gets or sets the Capacity maximum reached audit flag.
		/// </summary>
		public bool CapacityMaximumReached
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationStoreGeneralAuditFlag.CapacityMaximumReached);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationStoreGeneralAuditFlag.CapacityMaximumReached, value);
			}
		}

		/// <summary>
		/// Gets or sets PrimaryMaximumReached audit flag.
		/// </summary>
		public bool PrimaryMaximumReached
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationStoreGeneralAuditFlag.PrimaryMaximumReached);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationStoreGeneralAuditFlag.PrimaryMaximumReached, value);
			}
		}

		/// <summary>
		/// Gets or sets MayExceedMax flag
		/// </summary>
		/// <remarks>
		/// When true, this flag indicates the allocation process was permitted to exceed grade maximums.  Note: Once this flag is set to true, any allocation process will be allowed to exceed grade maximums.
		/// </remarks>
		public bool MayExceedMax
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreGeneralAuditFlag.MayExceedMax);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreGeneralAuditFlag.MayExceedMax, value);
			}
		}

		/// <summary>
		/// Gets or sets MayExceedPrimary
		/// </summary>
		/// <remarks>
		/// When true, this flag indicates the allocation process was permitted to exceed primary maximums. Note: Once this flag is set to true, any allocation process will be allowed to exceed primary maximums.
		///  </remarks>
		public bool MayExceedPrimary
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreGeneralAuditFlag.MayExceedPrimary);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreGeneralAuditFlag.MayExceedPrimary, value);
			}
		}
        
		/// <summary>
		/// Gets or sets MayExceedCapacity flag.
		/// </summary>
		/// <remarks>
		/// When true, this flag indicates the allocation process was permitted to exceed capacity maximums. Note: Once this flag is set to true, any allocation process will be allowed to exceed capacity maximums.
		/// </remarks>
		public bool MayExceedCapacity
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreGeneralAuditFlag.MayExceedCapacity);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreGeneralAuditFlag.MayExceedCapacity, value);

			}
		}

		/// <summary>
		/// Gets or sets StoreIsEligible flag value.
		/// </summary>
		public bool StoreIsEligible
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreGeneralAuditFlag.Eligible);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreGeneralAuditFlag.Eligible, value);
			}
		}

		/// <summary>
		/// Gets or sets StoreAllocationPriority flag value.
		/// </summary>
		public bool StoreAllocationPriority
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreGeneralAuditFlag.AllocationPriority);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreGeneralAuditFlag.AllocationPriority, value);
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Store pt 11
        /// <summary>
        /// Gets or sets IncludeStoreInAllocation flag value (True: indicates store is included in allocation of the Header)
        /// </summary>
        public bool IncludeStoreInAllocation
        {
            get
            {
                return !MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreGeneralAuditFlag.ExcludeStoreInAllocation);     // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 13
            }
            set
            {
                _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreGeneralAuditFlag.ExcludeStoreInAllocation, !value);   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 13
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Store pt 11
		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Allocation Store Detail Audit Flags track the detail happenings to each store during the allocation process.
	/// </summary>
	/// <remarks>
	/// The Allocation Store Detail Audit Flags:
	/// <list type = "bullet">
	/// <item>IsManuallyAllocated: true indicates the user manually specified the allocation </item>
	/// <item>WasAutoAllocated: true indicates units were allocated by an auto-allocation process</item>
	/// <item>ChosenRuleAccepted: true indicates that the "chosen" rule was used to determine a portion of the units allocated through an auto-allocation process</item>
	/// <item>Out: true indicates to exclude the allocation component from the allocation process</item>
	/// <item>HadNeed: true indicates units were allocated based on need</item>
	/// <item>FilledSizeHole: true indicates units were allocated to fill a gap in the desired size ownership curve</item>
	/// <item>Locked: true indicates units allocated are locked from changes; false indicates units allocated may be changed.</item>
	/// <item>TempLock: true indicates units allocated are temporarily locked from changes; false indicates units allocated may be changed</item>
	/// <item>RuleAllocationFromParentComponent: true indicates rule allocation is partially or completely from parent component; false indicates parent component did not participate in allocation.</item>
	/// <item>RuleAllocationFromChildComponent: true indicates rule allocation is partially or completely from children components; false indicates children components did not contribute to allocation.</item>
	/// <item>RuleAllocationFromChosenRule: true indicates rule allocationis partially or completely from the chosen rule for this component; false indicates chosen rule for componenent did not contribute to allocation.</item>
	/// <item>AllocationFromBottomUpSize: true indicates allocation is partially or completely from a bottom up size allocation; false indicates bottom up size allocation did not contribute to allocation</item>
	/// <item>AllocationFromPackContentBreakOut: true indicates allocation is partially or completely from an allocation by pack content; false indicates pack content not used to determine allocation.</item>
	/// <item>AllocationFromBulkSizeBreakOut: true indicates allocation is partially or completely from a breakout of the color by size; false indicates breakout of color by size not used to determine allocation.</item>
	/// <item>AllocationModifiedAfterMultiHeaderSplit: true indicates this store's allocation was modified after the header was split from a multi-header; false indicates no modification occurred after this header was split from a multi</item>
	/// </list>
	/// </remarks>
	[Serializable] 
	public struct AllocationStoreDetailAuditFlags
	{
		//======= 
		// FIELDS
		//=======
        //ushort _flags; // TT#488 - MD - Jellis  - Group Allocation 
        uint _flags; // TT#488 - MD - Jellis  - Group Allocation 

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets or Sets all flags
		/// </summary>
        //internal ushort AllFlags // TT#488 - MD - Jellis - Group Allocation
        internal uint AllFlags // TT#488 - MD - Jellis - Group Allocation
		{
			get
			{
				return _flags;
			}
			set
			{
				_flags = value;
			}
		}
		/// <summary>
		/// Gets or sets the value of IsManuallyAllocated Audit Flag
		/// </summary>
		/// <remarks>
		/// When this flag is true, the units allocated were manually set by the user. When true, this flag will inhibit the system from allocating additional units.
		/// </remarks>
		public bool IsManuallyAllocated
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationStoreDetailAuditFlag.ManuallyAllocated);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationStoreDetailAuditFlag.ManuallyAllocated,value);
			}
		}
         
        // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
        /// <summary>
        /// Gets or sets the value of ItemIsManuallyAllocatd Audit Flag
        /// </summary>
        /// <remarks>
        /// When this flag is true, the Item Value for the store was manually set by the user.
        /// </remarks>
        public bool ItemIsManuallyAllocated
        {
            get
            {
                return MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.ItemValueManuallyAllocated);
            }
            set
            {
                _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.ItemValueManuallyAllocated, value);
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 2

        // begin TT#1334 - Urban - Jellis - Balance to VSW Enhancement
        public bool ItemQtyIsLocked
        {
            get
            {
                return MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.ItemQtyIsLocked);
            }
            set
            {
                _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.ItemQtyIsLocked, value);
            }
        }
        // end TT#1334 - Urban - Jellis - Balance to VSW Enhancement

		/// <summary>
		/// Gets or sets the value of WasAutoAllocated Audit Flag
		/// </summary>
		/// <remarks>
		/// When this flag is true, the system allocated units.  If IsManuallyAllocated is also true, then override to the auto-allocated units was specified by the user.
		/// </remarks>
		public bool WasAutoAllocated
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationStoreDetailAuditFlag.AutoAllocated);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationStoreDetailAuditFlag.AutoAllocated,value);
			}
		}
		/// <summary>
		/// Gets or sets ChosenRuleAcceptedByHeader audit flag.
		/// </summary>
		/// <remarks>
		/// When this flag is true, the chosen rule was accepted. The action implied by the accepted chosen rule is ignored when IsManuallyAllocated is true.
		/// </remarks>
        public bool ChosenRuleAcceptedByHeader // TT#488 - MD - Jellis - Group Allocation
		{
			get
			{
                return MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.ChosenRuleAcceptedByHeader); // TT#488 - MD - Jellis - Group Allocation
			}
			set 
			{
                _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.ChosenRuleAcceptedByHeader, value); // TT#488 - MD - Jellis - Group Allocation
			}
		}

        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets ChosenRuleAcceptedByGroup audit flag.
        /// </summary>
        /// <remarks>
        /// When this flag is true, the chosen rule was accepted. The action implied by the accepted chosen rule is ignored when IsManuallyAllocated is true.
        /// This flag is ignored when a header does not belong to a Group Allocation
        /// </remarks>
        public bool ChosenRuleAcceptedByGroup 
        {
            get
            {
                return MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.ChosenRuleAcceptedByGroup);
            }
            set
            {
                _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.ChosenRuleAcceptedByGroup, value);
            }
        }
        // end TT#488 - MD - Jellis - Group Allocation

		/// <summary>
		/// Gets or sets the Out audit flag.
		/// </summary>
		/// <remarks>
		/// When this flag is true, no automatic allocation process will allocate units to the store for the affected allocation component.
		/// </remarks>
		public bool Out
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationStoreDetailAuditFlag.Out);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationStoreDetailAuditFlag.Out, value);
			}
		}

		/// <summary>
		/// Gets or sets HadNeed audit flag.
		/// </summary>
		/// <remarks>
		/// When true, this flag indicates that a store was allocated units based on need.
		/// </remarks>
		public bool HadNeed
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationStoreDetailAuditFlag.Need);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationStoreDetailAuditFlag.Need, value);
			}
		}

		/// <summary>
		/// Gets or sets FilledSizeHole audit flag.
		/// </summary>
		/// <remarks>
		/// When true, this flag indicates the system allocated units to the store to fill in the desired size ownership curve.
		/// </remarks>
		public bool FilledSizeHole
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationStoreDetailAuditFlag.FilledSizeHole);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationStoreDetailAuditFlag.FilledSizeHole, value);
			}
		}

        // begin TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
        ///// <summary>
        ///// Gets or sets Locked flag
        ///// </summary>
        ///// <remarks>
        ///// When true, this flag indicates the store detail allocation is locked.
        ///// </remarks>
        //public bool Locked
        //{
        //    get
        //    {
        //        return MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.Locked);
        //    }
        //    set
        //    {
        //        _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.Locked, value);
        //    }
        //}
        // end TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2

        // begin TT#59 Implement Temp Locks
		// /// <summary>
		// /// Gets or sets TempLock
		// /// </summary>
		// /// <remarks>
		// /// When true, this flag indicates the store detail allocation is temporarily locked.
		// ///  </remarks>
		//public bool TempLock
		//{
		//	get
		//	{
		//		return MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.TempLock);
		//	}
		//	set
		//	{
		//		_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.TempLock, value);
		//	}
		//}
        // end TT#59 Implement Tempm Locks

		/// <summary>
		/// Gets or sets RuleAllocationFromParentComponent
		/// </summary>
		/// <remarks>
		/// When true, this flag indicates the store rule allocation was partially or completely derived from the chosen rule of a parent component.
		/// </remarks>
		public bool RuleAllocationFromParentComponent
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.RuleAllocationFromParentComponent);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.RuleAllocationFromParentComponent, value);
			}
		}

        // begin TT#488 - MD - Jellis - Group Allocation
        /// <summary>
        /// Gets or sets RuleAllocationFromGroupComponent
        /// </summary>
        /// <remarks>
        /// When true, this flag indicates the store rule allocation was partially or completely derived from the chosen rule of a Group component.
        /// </remarks>
        public bool RuleAllocationFromGroupComponent
        {
            get
            {
                return MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.RuleAllocationFromGroupComponent);
            }
            set
            {
                _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.RuleAllocationFromGroupComponent, value);
            }
        }
        // end TT#488 - MD - Jellis - Group Allocation

		/// <summary>
		/// Gets or sets RuleAllocationFromChildComponent
		/// </summary>
		/// <remarks>
		/// When true, this flag indicates the store rule allocation was partially or completely derived from the chosen rule(s) of children components.
		/// </remarks>
		public bool RuleAllocationFromChildComponent
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.RuleAllocationFromChildComponent);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.RuleAllocationFromChildComponent, value);
			}
		}

		/// <summary>
		/// Gets or sets RuleAllocationFromChosenRule
		/// </summary>
		/// <remarks>
		/// When true, this flag indicates the store rule allocation was partially or completely derived from the chosen rule of this component.</remarks>
		public bool RuleAllocationFromChosenRule
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.RuleAllocationFromChosenRule);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.RuleAllocationFromChosenRule, value);
			}
		}
        
		/// <summary>
		/// Gets or sets AllocationFromBottomUpSize
		/// </summary>
		/// <remarks>
		/// When true, this flag indicates the store allocation was partially or completely derived from a bottom-up size function.</remarks>
		public bool AllocationFromBottomUpSize
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.AllocationFromBottomUpSize);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.AllocationFromBottomUpSize, value);
			}
		}

		/// <summary>
		/// Gets or sets AllocationFromPackContent
		/// </summary>
		/// <remarks>
		/// When true, this flag indicates the store allocation was partially or completely derived from a Pack Content decision.</remarks>
		public bool AllocationFromPackContentBreakOut
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.AllocationFromPackContentBreakOut);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.AllocationFromPackContentBreakOut, value);
			}
		}

		/// <summary>
		/// Gets or sets AllocationFromBulkSizeBreakOut
		/// </summary>
		/// <remarks>
		/// When true, this flag indicates the store allocation was partially or completely derived from a Bulk Size BreakOut function.</remarks>
		public bool AllocationFromBulkSizeBreakOut
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.AllocationFromBulkSizeBreakOut);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.AllocationFromBulkSizeBreakOut, value);
			}
		}

		// begin MID Track 4448 AnF Audit Enhancement
		/// <summary>
		/// Gets or sets AllocationModifiedAfterMultiHeaderSplit
		/// </summary>
		/// <remarks>When true, this flag indicats the store allocation was derived from a Multi Header Split and then subsequently modified. </remarks>
		public bool AllocationModifiedAfterMultiHeaderSplit
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.AllocationModifiedAfterMultiHeaderSplit);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStoreDetailAuditFlag.AllocationModifiedAfterMultiHeaderSplit, value);
			}
		}
		// end MID Track 4448 AnF Audit Enhancement
		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Tracks the progress of the allocation process. 
	/// </summary>
	/// <remarks>
	/// Allocation Status Flags:
	/// <list type="bullet">
	/// <item>ReceivedInBalance:  True indicates Allocation Header is ready to be allocated</item>
	/// <item>BottomUpSizePerformed:  True indicates a bottom-up size allocation process has occurred</item>
	/// <item>RulesDefinedAndProcessed:  True indicates Rules have been defined and processed</item>
	/// <item>NeedAllocationPerformed:  True indicates Style Need Allocation has occurred</item>
	/// <item>PackBreakoutByContent:  True indicates packs were broken out based on pack content need</item>
	/// <item>BulkSizeBreakoutPerformed:  True indicates a top-down size allocation process has occurred</item>
	/// <item>ReleaseApproved:  True indicates the Allocation Header and its store allocation ready to be released</item>
	/// <item>Released:  True indicates the Allocation Header and its store allocation ready to be picked and shipped to stores</item>
	/// <item>UnitsAllocated: True indicates at least one unit has been allocated; False indicates no units have been allocated. </item>
	/// <item>GradeDefinitionOverride: True indicates Grade Definition Overridden; False indicates no override occurred.</item>
	/// <item>AllocationFromMultiHeader: True indicates the allocation for this header was partially or wholly derived from a Multi-header split</item>
    /// <item>ReapplyTotalAllocationPerformed: True indicates a Reapply Total Allocation action occurred</item>
    /// <item>ImoCriteriaLoaded:  True indicates Imo (VSW) criteria has been loaded</item>
    /// <item>IdentifyIncludedStores: True indicates it is necessary to identify the stores to include in the allocation</item>
    /// <item>ImoCriteriaOverridden:  True indicates IMO (VSW) criteria was set using Allocation Override Method</item>
    /// <item>CalcVswSizeConstraints:  True indicates it is necessary to calculate the VSW Size constraints</item>
    /// <item>BalanceToVSW_Performed:  True indicates the Balance to VSW Action was successfully performed</item>

	/// </list>
	/// </remarks>
	[Serializable]
	public struct AllocationStatusFlags
	{
		//======= 
		// FIELDS
		//=======
        //ushort _flags;  // TT#246 - MD - Jellis - AnF VSW Size In Store minimums
        uint _flags;

		//=============
		// CONSTRUCTORS
		//=============
		// MID Track 5778 Scheduler 'Run Now' feature gets error in audit
        public AllocationStatusFlags(uint aFlags)   // TT#246 - MD - Jellis - AnF VSW Size In Store minimums
		{
			_flags = aFlags;
		}
        // MID Track 5778 Scheduler 'Run Now' feature gets error in audit
		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets or Sets all flags
		/// </summary>
        internal uint AllFlags // TT#246 - MD - Jellis - AnF VSW Size In Store minimums
		{
			get
			{
				return _flags;
			}
			set
			{
				_flags = value;
			}
		}
		/// <summary>
		/// Gets or sets the value of ReceivedInBalance Flag
		/// </summary>
		/// <remarks>
		/// When true, the allocation header is ready for the allocation process.
		/// </remarks>
		/// <value>
		/// When this flag is true, flags SizeReceiptsBalanceToColor<see cref="BalanceStatusFlags.SizeReceiptsBalanceToColor"/>
		/// , ColorReceiptsBalanceToBulk<see cref="BalanceStatusFlags.ColorReceiptsBalanceToBulk"/>
		/// , PackSizesBalanceToPackColor<see cref="BalanceStatusFlags.PackSizesBalanceToPackColor"/>
		///  and PackColorsBalanceToPack must be true <see cref="BalanceStatusFlags.PackColorsBalanceToPack"/>
		/// </value>
		public bool ReceivedInBalance
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationStatusFlag.ReceivedInBalance);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationStatusFlag.ReceivedInBalance,value);
			}
		}

		/// <summary>
		/// Gets or sets the value of GradeDefinitionOverride Flag
		/// </summary>
		/// <remarks>
		/// When true, grade definitions have been overridden.
		/// </remarks>
		/// <value>
		/// When this flag is true, ReceivedInBalance must be true <see cref="ReceivedInBalance"></see>.
		/// </value>
		public bool GradeDefinitionOverride
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationStatusFlag.GradeDefinitionOverride);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationStatusFlag.GradeDefinitionOverride,value);
			}
		}

		/// <summary>
		/// Gets or sets the value of UnitsAllocated Flag
		/// </summary>
		/// <remarks>
		/// When true, the at least one unit has been allocated to at least one store.
		/// </remarks>
		/// <value>
		/// When this flag is true, ReceivedInBalance must be true <see cref="ReceivedInBalance"></see>.
		/// </value>
		public bool UnitsAllocated
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationStatusFlag.UnitsAllocated);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationStatusFlag.UnitsAllocated,value);
			}
		}

		/// <summary>
		/// Gets or sets the value of BottomUpSizePerformed Flag
		/// </summary>
		/// <remarks>
		/// When true, the a Bottom Up Allocation process has occurred for at least one color.
		/// </remarks>
		/// <value>
		/// When this flag is true, ReceivedInBalance must be true <see cref="ReceivedInBalance"></see>.
		/// </value>
		public bool BottomUpSizePerformed
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationStatusFlag.BottomUpSizePerformed);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationStatusFlag.BottomUpSizePerformed,value);
			}
		}

		/// <summary>
		/// Gets or sets the value of RulesDefinedAndProcessed Flag
		/// </summary>
		/// <remarks>
		/// When true, at least one rule has been defined and processed for at least one store.
		/// </remarks>
		/// <value>
		/// When this flag is true, ReceivedInBalance must be true <see cref="ReceivedInBalance"></see>.
		/// </value>
		public bool RulesDefinedAndProcessed
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationStatusFlag.RulesDefinedAndProcessed);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationStatusFlag.RulesDefinedAndProcessed,value);
			}
		}

		/// <summary>
		/// Gets or sets the value of NeedAllocationPerformed Flag
		/// </summary>
		/// <remarks>
		/// When true, a style need allocation has occurred.
		/// </remarks>
		/// <value>
		/// When this flag is true, ReceivedInBalance must be true <see cref="ReceivedInBalance"></see>.
		/// </value>
		public bool NeedAllocationPerformed
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationStatusFlag.NeedAllocationPerformed);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationStatusFlag.NeedAllocationPerformed,value);
			}
		}


		/// <summary>
		/// Gets or sets the value of PackBreakoutByContent Flag
		/// </summary>
		/// <remarks>
		/// When true, packs have been allocated by pack content need.
		/// </remarks>
		/// <value>
		/// When this flag is true, ReceivedInBalance must be true <see cref="ReceivedInBalance"></see>.
		/// </value>
		public bool PackBreakoutByContent
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationStatusFlag.PackBreakoutByContent);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationStatusFlag.PackBreakoutByContent,value);
			}
		}


		/// <summary>
		/// Gets or sets the value of BulkSizeBreakoutPerformed Flag
		/// </summary>
		/// <remarks>
		/// When true, a Bulk Size Breakout has occurred.
		/// </remarks>
		/// <value>
		/// When this flag is true, ReceivedInBalance must be true <see cref="ReceivedInBalance"></see>.
		/// </value>
		public bool BulkSizeBreakoutPerformed 
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationStatusFlag.BulkSizeBreakoutPerformed);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationStatusFlag.BulkSizeBreakoutPerformed,value);
			}
		}

		/// <summary>
		/// Gets or sets the value of ReleaseApproved Flag
		/// </summary>
		/// <remarks>
		/// When true, allocation header has been approved for release.
		/// </remarks>
		/// <value>
		/// When this flag is true, all balance and intransit flags must be true.
		/// </value>
		public bool ReleaseApproved 
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationStatusFlag.ReleaseApproved);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationStatusFlag.ReleaseApproved,value);
			}
		}

		/// <summary>
		/// Gets or sets the value of Released Flag
		/// </summary>
		/// <remarks>
		/// When true, allocation header has been approved for release.
		/// </remarks>
		/// <value>
		/// When this flag is true, ReleaseApproved must be true <see cref="ReleaseApproved"></see>.
		/// </value>
		public bool Released 
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationStatusFlag.Released);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationStatusFlag.Released,value);
			}
		}

		// begin MID Track 4448 AnF Audit Enhancement
		/// <summary>
		/// Gets or sets the AllocationFromMultiHeader Flag
		/// </summary>
		/// <remarks>When this flag is true, then the allocation of this header was derived from a Multi-header split,</remarks>
		public bool AllocationFromMultiHeader
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationStatusFlag.AllocationFromMultiHeader);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationStatusFlag.AllocationFromMultiHeader, value);
			}
		}
		// end MID Track 4448 AnF Audit Enhancement

		// begin TT#785 - stodd - reapply total allocation
		/// <summary>
		/// Gets or sets the ReapplyTotalAllocationPerformed Flag
		/// </summary>
		/// <remarks>When this flag is true, then the Reapply Total Allocation action has been performed on the header.</remarks>
		public bool ReapplyTotalAllocationPerformed
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationStatusFlag.ReapplyTotalAllocationPerformed);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStatusFlag.ReapplyTotalAllocationPerformed, value);
			}
		}
		// end TT#785 - stodd - reapply total allocation

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 5
        // begin TT#327 - JEllis - Headers Allocated Before VSW Get VSW Allocation
        ///// <summary>
        ///// Gets or sets the IMO Criteria Loaded flag
        ///// </summary>
        ///// <remarks>When this flag is true, then the store IMO criteria (Max Value, Min Ship Qty and Pack Threshold) have been established.</remarks>
        //public bool ImoCriteriaLoaded
        //{
        //    get
        //    {
        //        return MIDFlag.GetFlagValue(_flags, (int)eAllocationStatusFlag.ImoCriteriaLoaded);
        //    }
        //    set
        //    {
        //        _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStatusFlag.ImoCriteriaLoaded, value);
        //    }
        //}
        /// <summary>
        /// Gets or sets the Load IMO Criteria flag
        /// </summary>
        /// <remarks>When this flag is true, then the store IMO criteria (Max Value, Min Ship Qty and Pack Threshold) need to be loaded.</remarks>
        public bool LoadImoCriteria
        {
            get
            {
                return MIDFlag.GetFlagValue(_flags, (int)eAllocationStatusFlag.LoadImoCriteria);
            }
            set
            {
                _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStatusFlag.LoadImoCriteria, value);
            }
        }
        // end TT#327 - JEllis - Headers Allocated Before VSW Get VSW Allocation
        // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 17
        /// <summary>
        /// Gets or sets the IdentifyIncludedStores flag
        /// </summary>
        /// <remarks>When this flag is true, then it is necessary to identify which stores to include in the allocation</remarks>
        public bool IdentifyIncludedStores
        {
            get
            {
                return MIDFlag.GetFlagValue(_flags, (int)eAllocationStatusFlag.IdentifyIncludedStores);
            }
            set
            {
                _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStatusFlag.IdentifyIncludedStores, value);
            }
        }
        // begin TT#246 - MD - Jellis - AnF VSW Size In Store Minimums
        ///// <summary>
        ///// Gets or sets CalcVswSizeConstraints Flag
        ///// </summary>
        //public bool CalcVswSizeConstraints
        //{
        //    get
        //    {
        //        return MIDFlag.GetFlagValue(_flags, (int)eAllocationStatusFlag.CalcVswSizeConstraints);
        //    }
        //    set
        //    {
        //        _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStatusFlag.CalcVswSizeConstraints, value);
        //    }
        //}
        //// end TT#246 - MD - Jellis - AnF VSW Size In Store Minimums
        // begin TT#246 - MD - Jellis - AnF VSW Size In Store Minimums pt 5
        /// <summary>
        /// Gets or sets CalcVswSizeConstraints Flag
        /// </summary>
        public bool CalcVswColorIdealItemMin
        {
            get
            {
                return MIDFlag.GetFlagValue(_flags, (int)eAllocationStatusFlag.CalcVswColorIdealItemMin);
            }
            set
            {
                _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStatusFlag.CalcVswColorIdealItemMin, value);
            }
        }
        // end TT#246 - MD - Jellis - AnF VSW Size In Store Minimums pt 5
        // begin TT#2083 - JEllis - Urban Virtual Store Warehouse Min Ship Qty
        /// <summary>
        /// Gets or sets the IMO Criteria Overridden flag
        /// </summary>
        /// <remarks>When this flag is true, then the store IMO criteria (Max Value, Min Ship Qty and Pack Threshold) have been established by an override method.</remarks>
        public bool ImoCriteriaOverridden
        {
            get
            {
                return MIDFlag.GetFlagValue(_flags, (int)eAllocationStatusFlag.ImoCriteriaOverridden);
            }
            set
            {
                _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStatusFlag.ImoCriteriaOverridden, value);
            }
        }
        // end TT32083 - JEllis - Urban Virtual STore Warehouse Min Ship Qty
        // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 17
        // end TT#1401 - JEllis - Urban Reservation Stores pt 5

        // begin TT#1334 - Urban - Jellis - Balance To VSW Enhancement
        /// <summary>
        /// Gets or sets BalanceToVSW_Performed
        /// </summary>
        public bool BalanceToVSW_Performed
        {
            get
            {
                return MIDFlag.GetFlagValue(_flags, (int)eAllocationStatusFlag.BalanceToVSW_Performed);
            }
            set
            {
                _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStatusFlag.BalanceToVSW_Performed, value);
            }
        }

        /// <summary>
        /// Gets or sets EligibilityLoaded
        /// </summary>
        public bool EligibilityLoaded
        {
            get
            {
                return MIDFlag.GetFlagValue(_flags, (int)eAllocationStatusFlag.EligibilityLoaded);
            }
            set
            {
                _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationStatusFlag.EligibilityLoaded, value);
            }
        }
    }

    // begin TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5
    /// <summary>
    /// Tracks the progress of the color allocation process. 
    /// </summary>
    /// <remarks>
    /// Allocation Status Flags:
    /// <list type="bullet">
    /// <item>CalcVswSizeConstraints:  True indicates it is necessary to calculate the VSW Size constraints</item>
    /// </list>
    /// </remarks>
    [Serializable]
    public struct ColorStatusFlags
    {
        //======= 
        // FIELDS
        //=======
        uint _flags;

        //=============
        // CONSTRUCTORS
        //=============
        public ColorStatusFlags(uint aFlags)   
        {
            _flags = aFlags;
        }
        //===========
        // PROPERTIES
        //===========
        /// <summary>
        /// Gets or Sets all flags
        /// </summary>
        internal uint AllFlags 
        {
            get
            {
                return _flags;
            }
            set
            {
                _flags = value;
            }
        }
        /// <summary>
        /// Gets or sets the value of CalcVswSizeConstraints Flag
        /// </summary>
        /// <remarks>
        /// When true, it is necessary to calculate the VSW Size constraints.
        /// </remarks>
        public bool CalcVswSizeConstraints
        {
            get
            {
                return MIDFlag.GetFlagValue(_flags, (int)eColorStatusFlag.CalcVswSizeConstraints);
            }
            set
            {
                _flags = MIDFlag.SetFlagValue(_flags, (int)eColorStatusFlag.CalcVswSizeConstraints, value);
            }
        }
    }
    // end TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5

	/// <summary>
	/// Tracks the shipping progress of an allocation. 
	/// </summary>
	/// <remarks>
	/// Shipping Status Flags:
	/// <list type="bullet">
	/// <item>ShippingStarted:  True indicates at least 1 unit has been shipped to a store and charged to its onhand</item>
	/// <item>ShippingComplete: True indicates all units have been shipped to stores and charged to each store's onhand</item>
	/// <item>ShippingOnHold: True indicates changes to allocation are on-hold.</item>
	/// </list>
	/// </remarks>
	[Serializable]
	public struct ShippingStatusFlags
	{
		//======= 
		// FIELDS
		//=======
		byte _flags;

		//=============
		// CONSTRUCTORS
		//=============
        // begin MID Track 4312 Size Intransit for "total" color wrong
		public ShippingStatusFlags(byte aShippingStatusFlags)
		{
			_flags = aShippingStatusFlags;
		}
		// end MID Track 4312 Size Intransit for "total" color wrong
		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets or Sets all flags
		/// </summary>
		internal byte AllFlags
		{
			get
			{
				return _flags;
			}
			set
			{
				_flags = value;
			}
		}

		/// <summary>
		/// Gets or sets the value of ShippingStarted Flag
		/// </summary>
		/// <remarks>
		/// When true, allocation header has been approved for release.
		/// </remarks>
		/// <value>
		/// When this flag is true, Released must be true <see cref="AllocationStatusFlags.Released"></see>.
		/// </value>
		public bool ShippingStarted 
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eShippingStatusFlag.ShippingStarted);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eShippingStatusFlag.ShippingStarted,value);
			}
		}

		/// <summary>
		/// Gets or sets the value of ShippingComplete Flag
		/// </summary>
		/// <remarks>
		/// When true, allocation header has been approved for release.
		/// </remarks>
		/// <value>
		/// When this flag is true, Released must be true <see cref="AllocationStatusFlags.Released"></see>.
		/// </value>
		public bool ShippingComplete 
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eShippingStatusFlag.ShippingComplete);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eShippingStatusFlag.ShippingComplete,value);
			}
		}

		/// <summary>
		/// Gets or sets ShppingOnHold audit flag.
		/// </summary>
		public bool ShippingOnHold
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eShippingStatusFlag.ShippingOnHold);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eShippingStatusFlag.ShippingOnHold, value);
			}
		}
	}

	/// <summary>
	/// Allocation Type Flags track the Allocation Header Type.
	/// </summary>
	/// <remarks>
	/// Allocation Type Flags:
	/// <list type="bullet">
	/// <item>WorkUpBulkColorBuy: true indicates the system will calculate units to buy(allocate) for each color.  Mutually exclusive with the Work Up Pack Buy types.</item>
	/// <item>WorkUpBulkSizeBuy: true indicates the system will calculate the units to buy(allocate) for each size in each color.  Mutually exclusive with Work Up Pack Buy types</item>
	/// <item>WorkUpPackColorBuy: true indicates the system will determine the color content of each pack.  Mutually exclusive with the Work Up Bulk Buy types.</item>
	/// <item>WorkUpPackSizeBuy: true indicates the system will determine the size content of each pack.  Mutually exclusive with Work Up Bulk Buy type.</item>
	/// <item>WorkUpBulkBuy: a consolidated summary flag for WorkUpBulkColorBuy and WorkUpBulkSizeBuy</item>
	/// <item>WorkUpPackBuy: a consolidated summary flag for WorkUpPackColorBuy and WorkUpPackSizeBuy</item>
	/// <item>IsDummy: true indicates a work allocation header that cannot be released.</item>
	/// <item>HasPrimaryAllocation: true indicates this allocation header is attached to a primary allocation.</item>
	/// <item>HasSecondaryAllocation: true indicates this allocation is the primary allocation for another (the secondary) allocation.</item>
	/// <item>Reserve: true indicates that all units are allocated from warehouse onhands</item>
	/// <item>MultiHeader: true indicates that this header is a collection of other headers.</item>
	/// <item>Receipt: true indicates that the merchandise described on this header is a receipt in a warehouse or DC</item>
	/// <item>PurchaseOrder: true indicates that this header represents a purchase order.</item>
	/// <item>ASN: true indicates this header is an Automated Ship Notice </item>
	/// <item>DropShip: true indicates this header decribes a drop shipment.</item>
	/// <item>InUseByMulti: true indicates this header is a member of a MultiHeader.</item>
	/// <item>BulkIsDetail: true indicates that bulk is a subordinate of the DetailType; false indicates bulk is a "type".</item>
	/// <item>WorkUpTotalBuy: true indicates the system will calculate units to buy(allocate) for the total stores.  </item>
	/// <item>IsInterfaced: true indicates the header was added via an interface; false indicates header was added manually</item>
	/// </list>
	/// </remarks>
	[Serializable] 
	public struct AllocationTypeFlags
	{
		//======= 
		// FIELDS
		//=======
//		ushort _flags;
		uint _flags;

		//=============
		// CONSTRUCTORS
		//=============
	
		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets or Sets all flags
		/// </summary>
//		internal ushort AllFlags
		public uint AllFlags
		{
			get
			{
				return _flags;
			}
			set
			{
				_flags = value;
			}
		}

		/// <summary>
		/// Gets or sets the value of WorkUp Bulk Color Buy Flag
		/// </summary>
		/// <remarks>
		/// When true, the system will calculate color units to allocate (ie. recommended color units to buy).
		/// </remarks>
		/// <value>
		/// When this flag is true, WorkUpPackColorBuy and WorkUpPackSizeBuy must be false.
		/// </value>
		public bool WorkUpBulkColorBuy
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationTypeFlag.WorkUpBulkColorBuy);
			}
			set
			{
                // begin TT#370 Build Packs Enhancement
                _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationTypeFlag.WorkUpBulkColorBuy, value);
                //if (WorkUpPackBuy)
                //{
                //    throw new MIDException (eErrorLevel.warning,
                //        (int)eMIDTextCode.msg_WrkUpBulkAndPackBuysMutuallyExcl ,
                //        MIDText.GetText(eMIDTextCode.msg_WrkUpBulkAndPackBuysMutuallyExcl));
                //}
                //else
                //{
                //    _flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationTypeFlag.WorkUpBulkColorBuy,value);
                //}
                // end TT#370 Build Packs Enhancement
			}
		}
         
		/// <summary>
		/// Gets or sets the value of WorkUp Bulk Size Flag
		/// </summary>
		/// <remarks>
		/// When true, the system will calculate size units to allocate (ie. recommended size buy) for each color.
		/// </remarks>
		/// <value>
		/// When true, WorkUpPackColorBuy and WorkUpPackSizeBuy must be false.
		/// </value>
		public bool WorkUpBulkSizeBuy
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationTypeFlag.WorkUpBulkSizeBuy);
			}
			set
			{
                // begin TT#370 Build Packs Enhancement
                _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationTypeFlag.WorkUpBulkSizeBuy, value);
                //if (WorkUpPackBuy)
                //{
                //    throw new MIDException (eErrorLevel.warning,
                //        (int)eMIDTextCode.msg_WrkUpBulkAndPackBuysMutuallyExcl ,
                //        MIDText.GetText(eMIDTextCode.msg_WrkUpBulkAndPackBuysMutuallyExcl));
                //}
                //else
                //{
                //    _flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationTypeFlag.WorkUpBulkSizeBuy,value);
                //}
                // end TT#370 Build Packs Enhancement
			}
		}
		
		/// <summary>
		/// Gets or sets the value of WorkUp Pack Color Buy Flag
		/// </summary>
		/// <remarks>
		/// When true, the system will calculate a recommended pack buy with a description of the color content for each pack.
		/// </remarks>
		/// <value>
		/// When true, WorkUpBulkColorBuy and WorkUpBulkSizeBuy must be false.
		/// </value>
		public bool WorkUpPackColorBuy
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationTypeFlag.WorkUpPackColorBuy);
			}
			set
			{
                // begin TT#370 Build Packs Enhancement
                _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationTypeFlag.WorkUpPackColorBuy, value);
                //if (WorkUpBulkBuy)
                //{
                //    throw new MIDException (eErrorLevel.warning,
                //        (int)eMIDTextCode.msg_WrkUpBulkAndPackBuysMutuallyExcl ,
                //        MIDText.GetText(eMIDTextCode.msg_WrkUpBulkAndPackBuysMutuallyExcl));
                //}
                //else
                //{
                //    _flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationTypeFlag.WorkUpPackColorBuy,value);
                //}
                // end TT#370 Build Packs Enhancement
			}
		}
         
		/// <summary>
		/// Gets or sets the value of WorkUp Pack Size Flag
		/// </summary>
		/// <remarks>
		/// When true, the system will calculate a recommended pack buy with a description of the size content for each pack.
		/// </remarks>
		/// When true, WorkUpBulkColorBuy and WorkUpBulkSizeBuy must be false.
		public bool WorkUpPackSizeBuy
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationTypeFlag.WorkUpPackSizeBuy);
			}
			set
			{
                // begin TT#370 Build Packs Enhancement
                _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationTypeFlag.WorkUpPackSizeBuy, value);
                //if (WorkUpBulkBuy)
                //{
                //    throw new MIDException (eErrorLevel.warning,
                //        (int)eMIDTextCode.msg_WrkUpBulkAndPackBuysMutuallyExcl ,
                //        MIDText.GetText(eMIDTextCode.msg_WrkUpBulkAndPackBuysMutuallyExcl));
                //}
                //else
                //{
                //    _flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationTypeFlag.WorkUpPackSizeBuy,value);
                //}
                // end TT#370 Build Packs Enhancement
			}
		}
		/// <summary>
		/// Returns true if the intent of allocation is to work up a bulk buy (color or size or both).
		/// </summary>
		public bool WorkUpBulkBuy
		{
			get
			{
				if (WorkUpBulkColorBuy | WorkUpBulkSizeBuy)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Returns true is the intent of allocation is to work up a pack buy (color or size or both).
		/// </summary>
		public bool WorkUpPackBuy
		{
			get 
			{
				if(WorkUpPackColorBuy | WorkUpPackSizeBuy)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		/// <summary>
		/// When IsDummy is true, the associated allocation header is a dummy allocation.
		/// </summary>
		/// <remarks>
		/// The allocator may define as many dummy allocations as necessary. Dummy allocations act like all other allocations except they cannot be release approved or released.
		/// </remarks>
		public bool IsDummy
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationTypeFlag.IsDummy);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationTypeFlag.IsDummy,value);
			}
		}

		/// <summary>
		/// Gets or sets HasPrimaryAllocation flag.
		/// </summary>
		/// <remarks>
		/// HasPrimaryAllocation indicates that this allocation header is attached to another,
		/// previously allocated header whose allocation is being used as absolute maximums for
		/// the stores to be allocated from this header.
		/// </remarks>
		public bool HasPrimaryAllocation
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationTypeFlag.HasPrimaryAllocation);
			}
			set
			{
				if (HasSecondaryAllocation)
				{
					throw new MIDException (eErrorLevel.warning,
						(int)eMIDTextCode.msg_HdrAlreadyPrimary,
						MIDText.GetText(eMIDTextCode.msg_HdrAlreadyPrimary));
				}
				else
				{
					_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationTypeFlag.HasPrimaryAllocation, value);
				}
			}
		}

		/// <summary>
		/// Gets or sets HasSecondaryAllocation flag.
		/// </summary>
		/// <remarks>
		/// HasSecondaryAllocation indicates that this allocation header is attached to another,
		/// allocation header.  This header and allocation is being used as absolute maximums for
		/// the stores to be allocated from the secondary header.
		/// </remarks>
		public bool HasSecondaryAllocation
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationTypeFlag.HasSecondaryAllocation);
			}
			set
			{
				if (HasPrimaryAllocation)
				{
					throw new MIDException (eErrorLevel.warning,
						(int)eMIDTextCode.msg_HdrAlreadySecondary,
						MIDText.GetText(eMIDTextCode.msg_HdrAlreadySecondary));
				}
				else
				{
					_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationTypeFlag.HasSecondaryAllocation,value);
				}
			}
		}

		/// <summary>
		/// Gets or sets Reserve audit flag
		/// </summary>
		/// <remarks>
		/// When true, this flag indicates the allocation header represented ownership within a warehouse
		/// </remarks>
		public bool Reserve
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationTypeFlag.Reserve);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationTypeFlag.Reserve, value);
			}
		}

		/// <summary>
		/// Gets or sets the MultiHeader flag value.
		/// </summary>
		public bool MultiHeader
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationTypeFlag.MultiHeader);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationTypeFlag.MultiHeader, value);
			}
		}

		/// <summary>
		/// Gets or sets the Receipt flag value.
		/// </summary>
		public bool Receipt
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationTypeFlag.Receipt);
			}
			set 
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationTypeFlag.Receipt, value);
			}
		}

		/// <summary>
		/// Gets or set the purchase order flag value.
		/// </summary>
		public bool PurchaseOrder
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationTypeFlag.PurchaseOrder);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationTypeFlag.PurchaseOrder, value);
			}
		}

		/// <summary>
		/// Gets or sets the ASN flag value
		/// </summary>
 		public bool ASN
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationTypeFlag.ASN);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationTypeFlag.ASN, value);
			}
		}

		/// <summary>
		/// Gets or sets the DropShip flag value.
		/// </summary>
		public bool DropShip
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationTypeFlag.DropShip);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationTypeFlag.DropShip, value);
			}
		}

		/// <summary>
		/// Gets or sets the InUseByMulti flag value.
		/// </summary>
		public bool InUseByMulti
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationTypeFlag.InUse);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationTypeFlag.InUse, value);
			}
		}

		/// <summary>
		/// Gets or sets the BulkIsDetail flag value.
		/// </summary>
		public bool BulkIsDetail
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationTypeFlag.BulkIsDetail);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationTypeFlag.BulkIsDetail, value);
			}
		}

		/// <summary>
		/// Gets or sets the WorkUpTotalBuy flag value.
		/// </summary>
		public bool WorkUpTotalBuy
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationTypeFlag.WorkUpTotalBuy);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationTypeFlag.WorkUpTotalBuy, value);
			}
		}

		/// <summary>
		/// Gets or sets the IsInterfaced flag value.
		/// </summary>
		public bool IsInterfaced
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationTypeFlag.IsInterfaced);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationTypeFlag.IsInterfaced, value);
			}
		}

		/// <summary>
		/// Gets or sets the Assortment flag value.
		/// </summary>
		public bool Assortment
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationTypeFlag.Assortment);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationTypeFlag.Assortment, value);
			}
		}
		/// <summary>
		/// Gets or sets the Placeholder flag value.
		/// </summary>
		public bool Placeholder
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationTypeFlag.Placeholder);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationTypeFlag.Placeholder, value);
			}
		}

        // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
        public bool ImoHeader
        {
            get
            {
                return MIDFlag.GetFlagValue(_flags, (int)eAllocationTypeFlag.ImoHeader);
            }
            set
            {
                _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationTypeFlag.ImoHeader, value);
            }
        }
        // end TT#1401 - JEllis - Urban Reservation Stores pt 2

        // begin TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
        /// <summary>
        /// Gets or sets the AdjustVSW_OnHand flag
        /// </summary>
        public bool AdjustVSW_OnHand
        {
            get
            {
                return MIDFlag.GetFlagValue(_flags, (int)eAllocationTypeFlag.AdjustVSW);
            }
            set
            {
                _flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationTypeFlag.AdjustVSW, value);
            }
        }
        // end TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1

		//BEGIN TT#562-MD - stodd -  Bulk is not getting properly updated upon an assortment save of placeholders
		public bool PlaceholderBulkColor
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationTypeFlag.PlaceholderBulkColor);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationTypeFlag.PlaceholderBulkColor, value);
			}
		}

		public bool PlaceholderBulkSize
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationTypeFlag.PlaceholderBulkSize);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationTypeFlag.PlaceholderBulkSize, value);
			}
		}

		public bool PlaceholderBulk
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationTypeFlag.PlaceholderBulk);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationTypeFlag.PlaceholderBulk, value);
			}
		}

		public bool PlaceholderPack
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationTypeFlag.PlaceholderPack);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationTypeFlag.PlaceholderPack, value);
			}
		}

		public bool PlaceholderPackColor
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationTypeFlag.PlaceholderPackColor);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationTypeFlag.PlaceholderPackColor, value);
			}
		}

		public bool PlaceholderPackSize
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationTypeFlag.PlaceholderPackSize);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eAllocationTypeFlag.PlaceholderPackSize, value);
			}
		}

		//END TT#562-MD - stodd -  Bulk is not getting properly updated upon an assortment save of placeholders

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Balance Status Flags track the balance conditions for an Allocation Header.
	/// </summary>
	/// <remarks>
	/// Balance Status Flags:
	/// <list type="bullet">
	/// <item>SizeReceiptsBalanceToColor: true indicates all size receipts(units to allocate) balance to color receipts. Note when WorkUpBulkBuy is true, this flag will also be true.</item>
	/// <item>ColorReceiptsBalanceToBulk: true indicates all color receipts(units to allocate) balance to the bulk receipts. Note when WorkUpBulkColorBuy is true, this flag will also be true.</item>
	/// <item>PackSizesBalanceToPackColor: true indicates that for every pack the size content within each color of the pack balances to the color content of the pack. Note when no pack has size content, this flag will be true.</item>
	/// <item>PackColorsBalanceToPack: true indicates that for every pack the color content of the pack balances to the pack multiple. Note when no pack has color content, this flag will be true.</item>
	/// <item>PackPlusBulkReceiptsBalanceToTotal: true indicates that the total receipts (units to allocate) by pack and bulk balance to the total units received (units to allocate) on the header.</item>
	/// <item>PackAllocationInBalance: true indicates that for every pack, the store allocation for that pack balances to the number of packs received (packs to allocate). Note this flag is true if there are no bulk units to allocate.</item>
	/// <item>BulkColorAllocationInBalance: true indicates that 1) for each store, the store's bulk allocation equals the sum of the store's color allocations; 2) Bulk units to allocate equals the sum of the store's bulk allocations; and 3) For each color, units to allocate in that color equals the sum of the store allocations in that color. Note this flag is true when there are no packs.</item>
	/// <item>BulkSizeAllocationInBalance: true indicates that 1) for each color and each store, the store's color allocation equals the sum of the store's size allocations in that color; 2) For each color the color units to allocate equals the sum of the store's size allocations in that color. Note this flag is true when there are no packs.</item>
	/// <item>StoreSizeAllocationsInBalanceToColor: true indicates that store color-size allocations balance to store color allocation; note when BulkSizeAllocationInBalance is true, it overrides this flags value</item>
	/// <item>BulkSizeAllocations_GT_SizeReceipts: true indicates at least one color-size over allocates its receipts; note when BulkSizeAllocationInBalance is true it overrides this flag's value</item>
	/// <item>BulkColorAllocations_GT_ColorReceipts: true indicates at least one color over allocates its receipts; note when BulkColorAllocationInBalance is true it overrides this flag's value</item>
	/// </list>
	/// </remarks>
	[Serializable] 
	public struct BalanceStatusFlags
	{
		//======= 
		// FIELDS
		//=======
		ushort _flags;

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets or Sets all flags
		/// </summary>
		internal ushort AllFlags
		{
			get
			{
				return _flags;
			}
			set
			{
				_flags = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the value of Size Receipts Balance to Color Flag
		/// </summary>
		/// <remarks>
		/// When this flag is true, ReceivedInBalance must be false. <see cref="AllocationStatusFlags.ReceivedInBalance"></see>> 
		/// </remarks>
		public bool SizeReceiptsBalanceToColor
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eBalanceStatusFlag.SizeReceiptsBalanceToColor);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eBalanceStatusFlag.SizeReceiptsBalanceToColor,value);
			}
		}
         
		/// <summary>
		/// Gets or sets the value of Color Receipts Balance to Bulk Flag
		/// </summary>
		/// <remarks>
		/// When this flag is true, ReceivedInBalance must be false. <see cref="AllocationStatusFlags.ReceivedInBalance"></see>> 
		/// </remarks>
		public bool ColorReceiptsBalanceToBulk
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eBalanceStatusFlag.ColorReceiptsBalanceToBulk);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eBalanceStatusFlag.ColorReceiptsBalanceToBulk,value);
			}
		}
		
		/// <summary>
		/// Gets or sets the value of Pack Sizes Balance to Pack Color
		/// </summary>
		/// <remarks>
		/// When this flag is true, ReceivedInBalance must be false. <see cref="AllocationStatusFlags.ReceivedInBalance"></see>> 
		/// </remarks> 
		public bool PackSizesBalanceToPackColor
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eBalanceStatusFlag.PackSizesBalanceToPackColor);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eBalanceStatusFlag.PackSizesBalanceToPackColor,value);
			}
		}
         
		/// <summary>
		/// Gets or sets the value of Pack Colors Balance to Pack Flag
		/// </summary>
		/// <remarks>
		/// When this flag is true, ReceivedInBalance must be false. <see cref="AllocationStatusFlags.ReceivedInBalance"></see>> 
		/// </remarks>
		public bool PackColorsBalanceToPack
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eBalanceStatusFlag.PackColorsBalanceToPack);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eBalanceStatusFlag.PackColorsBalanceToPack,value);
			}
		}
        
		/// <summary>
		/// Gets or sets the value of Pack plus Bulk Recepts Balance to Total Flag
		/// </summary>
		/// <remarks>
		/// When this flag is true, ReceivedInBalance must be false. <see cref="AllocationStatusFlags.ReceivedInBalance"></see>> 
		/// </remarks>
		public bool PackPlusBulkReceiptsBalanceToTotal
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eBalanceStatusFlag.PackBulkReceiptsBalanceToTotal);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eBalanceStatusFlag.PackBulkReceiptsBalanceToTotal,value);
			}
		}
        
		/// <summary>
		/// Gets or sets the value of Pack Allocation In Balance Flag
		/// </summary>
		/// <remarks>
		/// When this flag is false, all Intransit flags must be false. 
		/// </remarks>
		public bool PackAllocationInBalance
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eBalanceStatusFlag.PackAllocationInBalance);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eBalanceStatusFlag.PackAllocationInBalance,value);
			}
		}

        
		/// <summary>
		/// Gets or sets the value of Bulk Color Allocation In Balance Flag
		/// </summary>
		/// <remarks>
		/// When this flag is false, all Intransit flags must be false.<see cref="IntransitUpdateStatusFlags"></see>
		/// </remarks>
		public bool BulkColorAllocationInBalance
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eBalanceStatusFlag.BulkColorAllocationInBalance);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eBalanceStatusFlag.BulkColorAllocationInBalance,value);
			}
		}
		
        
		/// <summary>
		/// Gets or sets the value of BulkPlusPackAllocationInBalanceToTotal
		/// </summary>
		public bool BulkPlusPackAllocationInBalance
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eBalanceStatusFlag.BulkPlusPackAllocationInBalance);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eBalanceStatusFlag.BulkPlusPackAllocationInBalance, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of Bulk Size Allocation In Balance Flag
		/// </summary>
		/// <remarks>
		/// When this flag is false, BulkSizeIntransitUpdated flag must be false.<see cref="IntransitUpdateStatusFlags"></see>
		/// </remarks>
		public bool BulkSizeAllocationInBalance
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eBalanceStatusFlag.BulkSizeAllocationInBalance);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eBalanceStatusFlag.BulkSizeAllocationInBalance,value);
			}
		}

		// begin MID Tracks 3738, 3811, 3827 Status problems
		/// <summary>
		/// Gets or sets the value of Store Size Allocations In Balance to Color Flag
		/// </summary>
		public bool StoreSizeAllocationsInBalanceToColor
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eBalanceStatusFlag.StoreSizeAllocationsInBalanceToColor);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eBalanceStatusFlag.StoreSizeAllocationsInBalanceToColor,value);
			}
		}

		/// <summary>
		/// Gets or sets the value of Bulk Size Allocations GT Size Receipts flag
		/// </summary>
		public bool BulkSizeAllocations_GT_SizeReceipts
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eBalanceStatusFlag.BulkSizeAllocations_GT_SizeReceipts);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eBalanceStatusFlag.BulkSizeAllocations_GT_SizeReceipts, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of At Least One Size Allocated flag
		/// </summary>
		public bool AtLeastOneSizeAllocated
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eBalanceStatusFlag.AtLeastOneSizeAllocated);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eBalanceStatusFlag.AtLeastOneSizeAllocated, value);
			}
		}


		/// <summary>
		/// Gets or sets the value of Bulk Color Allocations GT Color Receipts flag
		/// </summary>
		public bool BulkColorAllocations_GT_ColorReceipts
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eBalanceStatusFlag.BulkColorAllocations_GT_ColorReceipts);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags, (int)eBalanceStatusFlag.BulkColorAllocations_GT_ColorReceipts, value);
			}
		}
		// end MID Tracks 3738, 3811, 3827 Status problems
		//========
		// METHODS
		//========
	}


	/// <summary>
	/// Intransit Update Status Flags track the intransit conditions for an Allocation Header.
	/// </summary>
	/// <remarks>
	/// Intransit Update Status Flags:
	/// <list type="bullet">
	/// <item>StyleIntransitUpdated: true indicates that the allocation by pack and bulk color total (not size) has been posted to intransit.  When pack content is known and this flag is true, the pack content has updated intransit appropriately (both color and size).</item>
	/// <item>BulkColorIntransitUpdated: true indicates that the allocation by bulk color total (not size) has been posted to the appropriate color intransit.</item>
	/// <item>BulkSizeIntransitUpdated: true indicates that the allocation for each bulk color and bulk size has been posted to the appropriate color and size intransit.</item></list></remarks>
	[Serializable]
	public struct IntransitUpdateStatusFlags
	{
		//======= 
		// FIELDS
		//=======
		byte _flags;

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets or Sets all flags
		/// </summary>
		internal byte AllFlags
		{
			get
			{
				return _flags;
			}
			set
			{
				_flags = value;
			}
		}

		/// <summary>
		/// Gets or sets the value of Style Intransit Updated Flag.
		/// </summary>
		/// <remarks>
		/// When this flag is true, PackAllocationInBalance and BulkColorAllocationInBalance must be true.<see cref="BalanceStatusFlags"></see>
		/// </remarks>
		public bool StyleIntransitUpdated
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eIntransitUpdateStatusFlag.StyleIntransitUpdated);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eIntransitUpdateStatusFlag.StyleIntransitUpdated,value);
			}
		}
         
		/// <summary>
		/// Gets or sets the value of Bulk Color Intransit Updated Flag
		/// </summary>
		/// <remarks>
		/// When this flag is true, BulkColorAllocationInBalance flag must be true.<see cref="BalanceStatusFlags"></see>
		/// </remarks>
		public bool BulkColorIntransitUpdated
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eIntransitUpdateStatusFlag.BulkColorIntransitUpdated);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eIntransitUpdateStatusFlag.BulkColorIntransitUpdated,value);
			}
		}
		
		/// <summary>
		/// Gets or sets the value of Bulk Size Intransit Updated Flag
		/// </summary>
		/// <remarks>
		/// When this flag is true, BulkSizeAllocationInBalance flag must be true.<see cref="BalanceStatusFlags"></see>
		/// </remarks>
		public bool BulkSizeIntransitUpdated
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eIntransitUpdateStatusFlag.BulkSizeIntransitUpdated);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eIntransitUpdateStatusFlag.BulkSizeIntransitUpdated,value);
			}
		}

		//========
		// METHODS
		//========

	}

}
