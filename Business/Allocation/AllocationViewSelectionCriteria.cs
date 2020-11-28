using System;
using System.Data;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace MIDRetail.Business.Allocation
{
	#region AllocationViewSelectionCriteriaList ProfileList
	/// <summary>
	/// Summary description for AllocationViewSelectionCriteriaList.
	/// </summary>
	public class AllocationViewSelectionCriteriaList : ProfileList
	{
		public AllocationViewSelectionCriteriaList(eProfileType aType)
			:base(aType)
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
	#endregion ALlocationViewSelectionCriteriaList ProfileList
	
	#region AllocationViewSelectionCriteria Profile
	/// <summary>
	/// Summary description for AllocationViewSelectionCriteria.
	/// </summary>
	public class AllocationViewSelectionCriteria : Profile
	{
		#region Fields
		//========//
		// Fields //
		//========//

		private bool _firstBuild;
		private bool _firstBuildSize; // MID Change j.ellis  performance enhancments
		private AllocationSelection _sel;
		private ApplicationSessionTransaction _trans;
		private AllocationWaferBuilderGroup _wafers;
		private AllocationProfileList _apl;
		private System.Windows.Forms.Form _styleView;
		private System.Windows.Forms.Form _summaryView;
		private System.Windows.Forms.Form _sizeView;
		private System.Windows.Forms.Form _assortmentView;
        private System.Windows.Forms.Form _headerInformation; // TT#59 Implement Store Temp Locks
        //private bool _headersEnqueued;  // TT#1185 - Verify ENQ before Update
		private eDataState _dataState;
		private string _qtyAllocatedLabel;
        // BEGIN TT#1401 - AGallagher - Reservation Stores
        private string _qtyStoreItemLabel;
        private string _qtyStoreIMOLabel;
        private string _qtyStoreIMOMaxLabel;
        private string _qtyStoreIMOHistMaxLabel;
        // END TT#1401 - AGallagher - Reservation Stores
		private string _ruleLabel;
		private string _ruleResultLabel;
		private string _velocityRuleLabel;
        private string _velocityRuleTypeQtyLabel;  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
		private string _velocityRuleQtyLabel;
		private string _velocityRuleResultLabel;
        private string _velocityInitialRuleQtyLabel; // tt#152 Velocity balance - apicchetti
        private string _velocityInitialRuleTypeLabel; // tt#152 Velocity balance - apicchetti
        private string _velocityInitialWillShipLabel; // tt#152 Velocity balance - apicchetti
        private string _velocityInitialRuleTypeQtyLabel;  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
		private string _transferLabel;
		private string _styleInTransitLabel;
		private string _styleOnHandLabel;
		private string _totalComponentLabel;
		private string _detailComponentLabel;
		private string _bulkComponentLabel;
		private string _headerTotalLabel;
		private string _allStoreLabel;
		private string _balanceLabel;
		private string _preSizeAllocatedLabel; // MID Track 4282 Velocity overlays Fill Size Holes Allocation
        // begin TT#59 Implement Store Temp Locks
        private string _shipStatusLabel;
        private string _qtyShippedLabel;
        private string _allocationFromBottomUpSizeLabel;
        private string _allocationModifiedAftMultiSplitLabel;
        private string _storeHadNeedLabel;
        private string _unitNeedBeforeLabel;
        private string _percentNeedBeforeLabel;
        private string _wasAutoAllocatedLabel;
        private string _qtyAllocatedByAutoLabel;
        private string _ruleAllocationFromChildLabel;
        private string _ruleAllocationFromParentLabel;
        private string _ruleAllocationFromChosenRuleLabel;
        private string _qtyAllocatedByRuleLabel;
        private string _storeManuallyAllocatedLabel;
        private string _allocationFromPackNeedLabel;
        private string _allocationFromSizeBreakoutLabel;
        private string _storeColorMaximumLabel;
        private string _storeColorMinimumLabel;
        private string _storeFilledSizeHolesLabel;
        // end TT#59 Implement Store Temp Locks
		private string _unassignedLabel;
		//HeaderEnqueue _headerEnqueue;   // TT#1185 - Verify ENQ before Update
		System.Windows.Forms.DialogResult diagResult;
        //private StoreServerSession _storeServerSession; //TT#1517-MD -jsobek -Store Service Optimization
		private int _sizeViewRowVariableCount;
		private bool _atLeast1WorkUpSizeBuy;
		private DataTable _quickFilterSizeDataTable; // MID Track 3611 Quick Filter not working in Size Review


		//Begin: Work Fields for Build
		private string [] _headerID;
		private SortedList _colorKeyList;
		private SortedList _sizeKeyList;
		private bool _detailTypeFound;
		private ArrayList _genericPackName;
		private ArrayList _nonGenericPackName;
		private ArrayList _subtotalGenericPackName;
		private ArrayList _subtotalNonGenericPackName;
        private ArrayList _genericPackDisplayName;    // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
        private ArrayList _nonGenericPackDisplayName;  // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review

		private string _lblNoSecondarySize;
		private string _noSizeDimensionLbl;
		private Hashtable _hdrSecSizeKeys;
		private string _sizeTotalAllocatedLabel; // MID Track 3611 Quick Filter not working in Size Review
        private string _allDimAllocatedLabel; // MID Track 3611 Quick Filter not working in Size Review
		//End:   Work Fields for Build
        private string _dupSizeNameSeparator;  // TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
        private Dictionary<int, int> _asrtAsrtType; // TT#2 - RMatelic - Assortment Planning
        private bool _isGAMode = false; // TT#1194
		#endregion Fields
		
		#region Constructor
		//=============//
		// Constructor //
		//=============//
        public AllocationViewSelectionCriteria(ApplicationSessionTransaction aTrans, bool useExistingAllocationProfileList = false) //TT#739-MD -jsobek -Delete Stores -Headers Loaded Multiple Times
			:base(Convert.ToInt32(eProfileType.AllocationViewSelectionCriteria, CultureInfo.CurrentUICulture))
		{
			_firstBuild = true;
			_firstBuildSize = true; // MID Change j.ellis Performance Enhancement
			_trans = aTrans;
			_storeGroupLevel = -1;
            //AllocationProfile ap; //TT#739-MD -jsobek -Delete Stores -Headers Loaded Multiple Times
			_atLeast1WorkUpSizeBuy = false;
			_sel = new MIDRetail.Data.AllocationSelection(_trans.SAB.ClientServerSession.UserRID);
			if (_trans.VelocityCriteriaExists  // TT#925 - MD - Jellis - Group Balance to Reserve Ignores Store Allocation
                || _trans.AssortmentView != null) // TT#925 - MD - Jellis - Group Balance to Reserve Ignores Store Allocation
				_apl = _trans.GetAllocationProfileList();
			else
			{
                //Begin TT#739-MD -jsobek -Delete Stores -Headers Loaded Multiple Times
                //_apl = new AllocationProfileList(eProfileType.Allocation);
                //foreach (int rid in _sel.AllocationHeaderRIDS)
                //{
                //    ap = new AllocationProfile(aTrans,"",rid, _trans.SAB.ApplicationServerSession);
                //    _apl.Add(ap);
                //    if (ap.WorkUpBulkSizeBuy)
                //    {
                //        this._atLeast1WorkUpSizeBuy = true;
                //    }
                //}


                if (useExistingAllocationProfileList)
                {
                    _apl = _trans.GetAllocationProfileList();
                }
                else
                {
                    _apl = new AllocationProfileList(eProfileType.Allocation);
                    // begin TT#916 - MD - Jellis - Invalid Attempt to instantiate an assortment
                    AssortmentProfile asrtP;
                    AllocationProfile ap;
                    //foreach (int rid in _sel.AllocationHeaderRIDS)                           // TT#916 - MD - Jellis - Invalid attempt to instantiate an assortment
                    foreach (HeaderRID_Type headerRID_Type in _sel.AllocationHeaderRIDsTypes)  // TT#916 - MD - Jellis - Invalid attempt to instantiate an assortment
                    {
                        // begin TT#916  - MD - Jellis - Invalid attempt to instantiate an assortment
                        if (headerRID_Type.HeaderIsAssortment)
                        {
                            asrtP =
                                new AssortmentProfile(
                                    _trans, "", headerRID_Type.HeaderRID, _trans.SAB.ApplicationServerSession);
                            _apl.Add(asrtP);
                        }
                        else
                        {
                            ap =
                                new AllocationProfile(
                                    _trans, "", headerRID_Type.HeaderRID, _trans.SAB.ApplicationServerSession);
                            //AllocationProfile ap = new AllocationProfile(aTrans, "", rid, _trans.SAB.ApplicationServerSession);
                            // end TT#916 - MD - Jellis _ Invalid attempt to instantiate an assortment
                            _apl.Add(ap);
						    //BEGIN TT#562-MD - stodd -  Bulk is not getting properly updated upon an assortment save of placeholders
						    //if (ap.WorkUpBulkSizeBuy)  
						    if (ap.WorkUpBulkSizeBuy
							    || ap.PlaceholderBulkSize)
						    {
							    this._atLeast1WorkUpSizeBuy = true;
						    }
						    //END TT#562-MD - stodd -  Bulk is not getting properly updated upon an assortment save of placeholders
                        }
                    }
                }

                //set the "atLeast1WorkUpSizeBuy" flag
                //foreach (int rid in _sel.AllocationHeaderRIDS)                            // TT#916 - MD - Jellis - Invalid attempt to instantiate an assortment 
                foreach (HeaderRID_Type headerRID_Type in _sel.AllocationHeaderRIDsTypes)  // TT#916 - MD - Jellis - Invalid attempt to instantiate an assortment
                {
                    //AllocationProfile ap = (AllocationProfile)_apl.FindKey(rid);  // TT#916 - MD - Jellis - Invalid attempt to instantiate an assortment
                    AllocationProfile ap = (AllocationProfile)_apl.FindKey(headerRID_Type.HeaderRID); // TT#916 - MD - Jellis - Invalid attempt to instantiate an assortment
                    if (ap != null && (ap.WorkUpBulkSizeBuy || ap.PlaceholderBulkSize))	// TT#1487-MD - stodd - ASST - Fill Size Holes does not work on a PH.
                    {
                        this._atLeast1WorkUpSizeBuy = true;
                    }
                }


                //End TT#739-MD -jsobek -Delete Stores -Headers Loaded Multiple Times
			}
			base.Key = UserID;
			_styleView = null;
			_summaryView = null;
			_sizeView = null;
			_assortmentView = null;
            _headerInformation = null; // TT#59 Implement Store Temp Locks
			//_headersEnqueued = false;  // TT#1185 - Verify ENQ before Update
			_dataState = eDataState.New;
			//_headerEnqueue = null; // TT#1185 - Verify ENQ before Update
			_qtyAllocatedLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.QuantityAllocated).DefaultLabel;
            // BEGIN TT#1401 - AGallagher - Reservation Stores
            _qtyStoreItemLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.StoreItemQuantityAllocated).DefaultLabel;
            _qtyStoreIMOLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.StoreIMOQuantityAllocated).DefaultLabel;
            _qtyStoreIMOMaxLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.StoreIMOMaxQuantityAllocated).DefaultLabel;
            // END TT#1401 - AGallagher - Reservation Stores
			_ruleLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.AppliedRule).DefaultLabel;
			_ruleResultLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.RuleResults).DefaultLabel;

			_velocityRuleLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.VelocityRuleType).DefaultLabel;
            _velocityRuleTypeQtyLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.VelocityRuleTypeQty).DefaultLabel;  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
            _velocityRuleQtyLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.VelocityRuleQty).DefaultLabel;
			_velocityRuleResultLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.VelocityRuleResult).DefaultLabel;

            _velocityInitialRuleTypeLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.VelocityInitialRuleType).DefaultLabel;// tt#152 Velocity balance
            _velocityInitialRuleTypeQtyLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.VelocityInitialRuleTypeQty).DefaultLabel; // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
            _velocityInitialRuleQtyLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.VelocityInitialRuleQty).DefaultLabel; // tt#152 Velocity balance
            _velocityInitialWillShipLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.VelocityInitialWillShip).DefaultLabel;// tt#152 Velocity balance
            
			_transferLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.Transfer).DefaultLabel;
		    _preSizeAllocatedLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.PreSizeAllocated).DefaultLabel; // MID Track 4282 Velocity overlays Fill Size Holes Allocation
			_styleOnHandLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.StyleOnHand).DefaultLabel;
			_styleInTransitLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.StyleInTransit).DefaultLabel;
            _qtyStoreIMOHistMaxLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.StoreIMOHistoryMaxQuantityAllocated).DefaultLabel;  // TT#1401 - AGallagher - Reservation Stores
			_totalComponentLabel = MIDText.GetTextOnly((int)eComponentType.Total);
			_detailComponentLabel = MIDText.GetTextOnly((int)eComponentType.DetailType);
			_bulkComponentLabel = MIDText.GetTextOnly((int)eComponentType.Bulk);
			_headerTotalLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_StoreViewHeaderTotals);
			_allStoreLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_AllStores);
			_balanceLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_BalanceLine);
                    // begin TT#59 Implement Store Temp Locks
            _shipStatusLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.ShippingStatus).DefaultLabel;
            _qtyShippedLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.QtyShipped).DefaultLabel;
            _allocationFromBottomUpSizeLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.AllocationFromBottomUpSize).DefaultLabel;
            _allocationModifiedAftMultiSplitLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.AllocationModifiedAftMultiSplit).DefaultLabel;
            _storeHadNeedLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.StoreHadNeed).DefaultLabel;
            _unitNeedBeforeLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.UnitNeedBefore).DefaultLabel;
            _percentNeedBeforeLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.PercentNeedBefore).DefaultLabel;
            _wasAutoAllocatedLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.WasAutoAllocated).DefaultLabel;
            _qtyAllocatedByAutoLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.QtyAllocatedByAuto).DefaultLabel;
            _ruleAllocationFromChildLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.RuleAllocationFromChild).DefaultLabel;
            _ruleAllocationFromParentLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.RuleAllocationFromParent).DefaultLabel;
            _ruleAllocationFromChosenRuleLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.RuleAllocationFromChosenRule).DefaultLabel;
            _qtyAllocatedByRuleLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.QtyAllocatedByRule).DefaultLabel;
            _storeManuallyAllocatedLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.StoreManuallyAllocated).DefaultLabel;
            _allocationFromPackNeedLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.AllocationFromPackNeed).DefaultLabel;
            _allocationFromSizeBreakoutLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.AllocationFromSizeBreakout).DefaultLabel;
            _storeColorMaximumLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.StoreColorMaximum).DefaultLabel;
            _storeColorMinimumLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.StoreColorMinimum).DefaultLabel;
            _storeFilledSizeHolesLabel = AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.StoreFilledSizeHoles).DefaultLabel;
            // end TT#59 Implement Store Temp Locks
			_unassignedLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_Unassigned);
            //_storeServerSession = _trans.SAB.StoreServerSession; //TT#1517-MD -jsobek -Store Service Optimization
			this._sizeViewRowVariableCount = 0;
			_lblNoSecondarySize = MIDText.GetTextOnly((int) eMIDTextCode.str_NoSecondarySize);
			// BEGIN MID Track #3942  'None' is now another name for NoSecondarySize 
			_noSizeDimensionLbl = MIDText.GetTextOnly((int)eMIDTextCode.lbl_NoSecondarySize);
			// END MID Track #3942
			_sizeTotalAllocatedLabel = string.Empty; // MID Track 3611 Quick Filter not working in Size Review
			_allDimAllocatedLabel = string.Empty; // MID Track 3611 Quick Filter not working in Size Review
            // Begin TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
            _dupSizeNameSeparator = MIDText.GetTextOnly((int)eMIDTextCode.lbl_DupSizeNameSeparator);
            // End TT#234  
        }
		#endregion Constructor
	
		#region Properties
		/// <summary>
		/// Gets the eProfilType of this Profile
		/// </summary>
		public override eProfileType ProfileType
		{
			get {return (eProfileType.AllocationViewSelectionCriteria);}
		}

		/// <summary>
		/// Gets the user ID associated with this Allocation View Selection Criteria
		/// </summary>
		public int UserID
		{
			get{ return _sel.UserID; }
		}

		/// <summary>
		/// Gets or sets the eAllocationSelectionViewType for this criteria
		/// </summary>
		public eAllocationSelectionViewType ViewType
		{
			get{ return _sel.ViewType;}
			set
			{
                // begin TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                if (_sel.ViewType != value)
                {
                    // end TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                    _sel.ViewType = value;
                    _wafers = null;
                } // TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
			}
		}
		
		
		/// <summary>
		/// Gets or sets the Store Attribute ID associated currently associated with this criteria
		/// </summary>			  
		public int StoreAttributeID
		{
			get{ return _sel.StoreAttributeID;}
			set
			{
                // begin TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                if (_sel.StoreAttributeID != value)
                {
                    // end TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                    _sel.StoreAttributeID = value;
                    _wafers = null;
                } // TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores 
			}
		}

		/// <summary>
		/// Gets or sets the store group level
		/// </summary>
		private int _storeGroupLevel;
		public int StoreGroupLevel
		{
			get{ return _storeGroupLevel;}
			set
			{
                // begin TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                if (_storeGroupLevel != value)
                {
                    // end TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                    _storeGroupLevel = value;
                    _wafers = null;
                } // TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
			}
		}

		/// <summary>
		/// Gets or sets the store filter
		/// </summary>
		public int FilterID
		{
			get{ return _sel.FilterID; }
			set
			{
                // begin TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                if (_sel.FilterID != value)
                {
                    // end TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                    _sel.FilterID = value;
                    _wafers = null;
                }  // TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
			}
		}
        
		/// <summary>
		/// Gets or sets the GroupBy value.
		/// </summary>
		public int GroupBy
		{
			get{ return _sel.GroupBy; }
			set
			{
                // begin TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                if (_sel.GroupBy != value)
                {
                    // end TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                    _sel.GroupBy = value;
                    _wafers = null;
                }  // TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
			}
		}


		/// <summary>
		/// Gets or sets the secondary Group By
		/// </summary>
		public int AllocationSecondaryGroupBy
		{
			get{ return _sel.SecondaryGroupBy; }
			set
			{
                // begin TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                if (_sel.SecondaryGroupBy != value)
                {
                    // end TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                    _sel.SecondaryGroupBy = value;
                    _wafers = null;
                } // TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
			}
		}

		/// <summary>
		/// Gets the number of variables associated with a size view row
		/// </summary>
		public int SizeViewRowVariableCount
		{
			get
			{
				return _sizeViewRowVariableCount;
			}
		}

		/// <summary>
		/// Gets or sets the ViewIsSequential display flag value
		/// </summary>
		/// <remarks>True: Display in a sequential manner; False: Display in a matrix format (For size: the primary size as the column and the secondary size as the row)</remarks>
		public bool ViewIsSequential
		{
			get
			{
				return _sel.ViewIsSequential;
			}
			set
			{
				//this.ViewIsSequential = value;
                // begin TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                if (_sel.ViewIsSequential != value)
                {
                    // end TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                    _sel.ViewIsSequential = value;
                    _wafers = null;
                } // TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
			}
		}

		/// <summary>
		/// Gets or sets the ViewID of this criteria
		/// </summary>
		public int ViewRID
		{
			get{ return _sel.ViewRID;}
			set
			{
                // begin TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                if (_sel.ViewRID != value)
                {
                    // end TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                    _sel.ViewRID = value;
                    _wafers = null;
                } // TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
			}
		}

		/// <summary>
		/// Gets or sets the Need Analysis Period Begin RID
		/// </summary>
		public int NeedAnalysisPeriodBeginRID
		{
			get{ return _sel.NeedAnalysisPeriodBeginRID; }
			set
			{
                // begin TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                if (_sel.NeedAnalysisPeriodBeginRID != value)
                {
                    // end TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                    _sel.NeedAnalysisPeriodBeginRID = value;
                    _trans.ResetAnalysisSettings(); // TT#1154 - MD - Jellis - Group Allocation Infinite Loop
                    _wafers = null;
                } // TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
			}
		}

		/// <summary>
		/// Gets or sets the Need Analysis Period End RID
		/// </summary>
		public int NeedAnalysisPeriodEndRID
		{
			get{ return _sel.NeedAnalysisPeriodEndRID; }
			set
			{
                // begin TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                if (_sel.NeedAnalysisPeriodEndRID != value)
                {
                    // end TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                    _sel.NeedAnalysisPeriodEndRID = value;
                    _trans.ResetAnalysisSettings(); // TT#1154 - MD - Jellis - Group Allocation Infinite Loop
                    _wafers = null;
                } // TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
			}
		}

		/// <summary>
		/// Gets or set the Need Analysis Hierarchy Node RID
		/// </summary>
		public int NeedAnalysisHNID
		{
			get{ return _sel.NeedAnalysisHNID; }
			set
			{
                // begin TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                if (_sel.NeedAnalysisHNID != value)
                {
                    // end TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                    _sel.NeedAnalysisHNID = value;
                    _trans.ResetAnalysisSettings(); // TT#1154 - MD - Jellis - Group Allocation Infinite Loop
                    _wafers = null;
                } // TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
			}
		}

		/// <summary>
		/// Gets or sets the IncludeIneligibleStores Flag
		/// </summary>
		public bool IncludeIneligibleStores
		{
			get{ return _sel.IncludeIneligibleStores; }
			set
			{
                // begin TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                if (_sel.IncludeIneligibleStores != value)
                {
                    // end TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores 
                    _sel.IncludeIneligibleStores = value;
                    _wafers = null;
                } // TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
			}
		}
		
		/// <summary>
		/// Gets or sets the store Filter data table
		/// </summary>
		public DataTable FilterTable
		{
			get{ return _sel.FilterTable; }
			set
			{
                // begin TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                if (_sel.FilterTable != value)
                {
                    // end TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                    _sel.FilterTable = value;
                    _wafers = null;
                } // TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
			}
		}

		/// <summary>
		/// Gets or sets the AllocationProfileList containing the list of headers.
		/// </summary>
		public AllocationProfileList HeaderList
		{
			get{ return _apl; }
			set
			{
                // begin TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                if (_apl != value)
                {
                    // end  TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                    _apl = value;
                    _trans.ResetAnalysisSettings(); // TT#1154 - MD - Jellis - Group Allocation Infinite Loop
                    _wafers = null;
                } // TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
			}
		}
		
		
		/// <summary>
		/// Gets or sets the AnalysisOnly Flag
		/// </summary>
		public bool AnalysisOnly
		{
			get{ return _sel.AnalysisOnly; }
			set
			{
                // begin TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                if (_sel.AnalysisOnly != value)
                {
                    // end TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                    _sel.AnalysisOnly = value;
                    _trans.ResetAnalysisSettings(); // TT#1154 - MD - Jellis - Group Allocation Infinite Loop
                    _wafers = null;
                } // TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
			}
		}

		// BEGIN MID Track #2959 - add Size Need Analysis
		/// <summary>
		/// Gets or set the Size Curve RID
		/// </summary>
		public int SizeCurveRID
		{
			get{ return _sel.SizeCurveRID; }
			set
			{
                // begin TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                if (_sel.SizeCurveRID != value)
                {
                    // end TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                    _sel.SizeCurveRID = value;
                    _wafers = null;
                }  // TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
			}
		}

		/// <summary>
		/// Gets or set the Size Constraint RID
		/// </summary>
		public int SizeConstraintRID
		{
			get{ return _sel.SizeConstraintRID; }
			set
			{
                // Begin TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                if (_sel.SizeConstraintRID != value)
                {
                    // end TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                    _sel.SizeConstraintRID = value;
                    _wafers = null;
                } // TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
			}
		}

		/// <summary>
		/// Gets or set the Size Alternate RID
		/// </summary>
		public int SizeAlternateRID
		{
			get{ return _sel.SizeAlternateRID; }
			set
			{
                // begin TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                if (_sel.SizeAlternateRID != value)
                {
                    // end TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores
                    _sel.SizeAlternateRID = value;
                    _wafers = null;
                } // TT#1185 - MD - Jellis - Group Allocation - Style Review not showing all eligible stores 
			}
		} 

		// END MID Track #2959  

        // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
        public DataTable DTUserAllocBasis
        {
            get { return _sel.DTUserAllocBasis; }
            set { _sel.DTUserAllocBasis = value; }
        }
        // End TT#638  

        // Begin TT#1282 - RMatelic - Assortment - Added colors to a style, the style originally had quantities in it.  When I added the colors the style went to 0
        public bool FirstBuild
        {
            get { return _firstBuild; }
            set { _firstBuild = value; }
        }
        // End TT#1282

		// BEGIN TT#543-MD - stodd - Style View/Size View assortment display
		public bool FirstBuildSize
		{
			get { return _firstBuildSize; }
			set { _firstBuildSize = value; }
		}
		// END TT#543-MD - stodd - Style View/Size View assortment display

		/// <summary>
		/// Gets the AllocationWaferBuilderGroup associated with this Allocation Selection Criteria.  The Wafers are built if they have not yet been built.
		/// </summary>
		public AllocationWaferBuilderGroup Wafers
		{
			get
			{
				if (_wafers == null)
				{
					BuildWafers();
				}
				return _wafers;
			}
			//			set
			//			{
			//				_wafers = value;
			//			}
		}

		/// <summary>
		/// Gets or sets the StyleView Windows Form
		/// </summary>
		public System.Windows.Forms.Form StyleView
		{
			get{ return _styleView; }
			set
			{
				_styleView = value;		
			}
		}

		/// <summary>
		/// Gets or sets the SummaryView Windows Form
		/// </summary>
		public System.Windows.Forms.Form SummaryView
		{
			get{ return _summaryView; }
			set
			{
				_summaryView = value;		
			}
		}

		/// <summary>
		/// Gets or sets the SizeView Windows Form
		/// </summary>
		public System.Windows.Forms.Form SizeView
		{
			get{ return _sizeView; }
			set
			{
				_sizeView = value;		
			}
		}
        
        // begin TT#59 Implement Store Temp Locks
        public System.Windows.Forms.Form HeaderInformation
        {
            get { return _headerInformation; }
            set
            {
                _headerInformation = value;
            }
        }
        // end TT#59 Implement Store Temp Locks

		// begin MID Track 3611 Quick Filters not working in Size Review
		/// <summary>
		/// Gets a datatable containing a description of the sizes for the quick filter drop down
		/// </summary>
		public DataTable QuickFilterSizeDropDown
		{
			get 
			{
				if (this._seqSizeColGroup == null)
				{
					this.BuildSeqSizeColumns(this._trans.GetAllocationProfileList());
				}
				return _quickFilterSizeDataTable;
			}
		}
		// end MID Track 3611 Quick Filters not working in Size Review

		/// <summary>
		/// Gets or sets the SizeView Windows Form
		/// </summary>
		public System.Windows.Forms.Form AssortmentView
		{
			get { return _assortmentView; }
			set
			{
				_assortmentView = value;
			}
		}

        // begin TT#1185 - Verify ENQ before Update
        ///// <summary>
        ///// Gets or sets a bool that indicatew whether the selected Headers are enqueued 
        ///// </summary>
        //public bool HeadersEnqueued 
        //{
        //    get{ return _headersEnqueued; }
        //    set
        //    {
        //        _headersEnqueued = value;		
        //    }
        //}
        // end TT#1185 - Verify ENQ before Update

		/// <summary>
		/// Gets or sets the data state of the selected headers.
		/// </summary>
		public eDataState DataState 
		{
			get{ return _dataState; }
			set
			{
				_dataState = value;		
			}
		}

		/// <summary>
		/// Gets or sets the AllocationSelection
		/// </summary>
		public AllocationSelection AllocSelection 
		{
			get{ return _sel; }
		}

		public int StoreGroupRID
		{
			get
			{
				if (this._trans.VelocityCriteriaExists)
				{
					return this._trans.Velocity.StoreGroupRID;
				}
				return this._sel.StoreAttributeID;
			}
		}

		/// <summary>
		/// Gets the primary size count (the Size View Wafers must exist in order for this count to be accurate)
		/// </summary>
		public int PrimarySizesCount
		{
			get
			{
				if (this._primarySizeKeys == null)
				{
					return 0;
				}
				return this._primarySizeKeys.Length;
			}
		}

        public bool IsGAMode
        {
            get
            {
                return _isGAMode;
            }
        }


		/// <summary>
		/// Gets the secondary size count (this Size View Wafers must exist in order for this count to be accurate)
		/// </summary>
		public int SecondarySizesCount
		{
			get
			{
				if (this._secondarySizeKeys == null)
				{
					return 0;
				}
				return this._secondarySizeKeys.Length;
			}
		}
		
		public class ColorData
		{
			public string ColorID;
			public string ColorName;
			public bool IsDuplicateName;
            public int ColorSequence;       //  Begin TT#213 - RMatelic - MID Track #6300-Multi header order in the workspace does not match order in style review
            public int ColorCodeRID;        //  End TT#213
		}   

        //// Begin TT#2 Ron Matelic - Assortment PLanning
        //public HeaderEnqueue HeaderEnqueue
        //{
            //get { return _headerEnqueue; }
        //}    
        //// End TT#2
		#endregion Properties

		#region Methods
		//=========//
		// Methods //
		//=========//
		#region SaveDefaults
		/// <summary>
		/// Saves the default selection criteria for this user to the database. This selection criteria will be the default selection for this user until the user changes the selection at some future date.
		/// </summary>
		public void SaveDefaults()
		{
			int i = 0;
			// BEGIN MID Track #3807 Size Review screen error on WorkupSiseBuy
			this._atLeast1WorkUpSizeBuy = false;
			// END MID Track #3807
            //_sel.AllocationHeaderRIDS = new int[_apl.Count]; // TT#916 - MD - Jellis - Invalid attempt to instantiate assortment
            _sel.AllocationHeaderRIDsTypes = new HeaderRID_Type[_apl.Count]; // TT#916 - MD - Jellis - Invalid attempt to instantiate assortment
			foreach(AllocationProfile ap in _apl)
			{
                //_sel.AllocationHeaderRIDS[i] = ap.Key; // TT#916 - MD - Jellis - Invalid attempt to instantiate assortment
                // Begin TT#4988 - BVaughan - Performance
                #if DEBUG
                if ((ap is AssortmentProfile && !ap.isAssortmentProfile) || (!(ap is AssortmentProfile) && ap.isAssortmentProfile))
                {
                    throw new Exception("Object does not match AssortmentProfile in SaveDefaults()");
                }
                #endif                
                //_sel.AllocationHeaderRIDsTypes[i] = new HeaderRID_Type(ap.Key, ap is AssortmentProfile); // TT#916 - MD - Jellis - Invalid attempt to instantiate assortment
                _sel.AllocationHeaderRIDsTypes[i] = new HeaderRID_Type(ap.Key, ap.isAssortmentProfile); // TT#916 - MD - Jellis - Invalid attempt to instantiate assortment
                // End TT#4988 - BVaughan - Performance

				// BEGIN MID Track #3807  Size Review screen error on WorkupSiseBuy
				//BEGIN TT#562-MD - stodd -  Bulk is not getting properly updated upon an assortment save of placeholders 
                //if (ap.WorkUpBulkSizeBuy)  
                if (ap.WorkUpBulkSizeBuy     
                    || ap.PlaceholderBulkSize)       
				{
					this._atLeast1WorkUpSizeBuy = true;
				}
				//END TT#562-MD - stodd -  Bulk is not getting properly updated upon an assortment save of placeholders 
				// END MID Track #3807
				i++;
			}
			_sel.SaveDefaultsToDatabase();
		}
		#endregion SaveDefaults

		#region BuildWafers
		#region ReBuild
		/// <summary>
		/// Rebuilds the wafers based on the current selection criteria.
		/// </summary>
		public void RebuildWafers()
		{
			_wafers = null;
			BuildWafers();
		}
		#endregion ReBuild

		#region Build
		#region BuildControl
		/// <summary>
		/// Builds Allocation Wafers.
		/// </summary>
		/// <remarks>An Allocation wafer describes the content of the cells within Style View, Style Summary and Velocity Detail.</remarks>
		private void BuildWafers()
		{
			//			this._trans.SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information,"   Begin:  BuildWafers()", this.GetType().Name);
			//			_detailTypeFound = false;
    		// _wafers = new AllocationWaferBuilderGroup(_trans,3,3);

			if (_firstBuild)
			{
				_detailTypeFound = false;
				_firstBuild = false;
                //_headerID = new String[_apl.Count];   // TT#2 - RMatelic - Assortment Planning >> unused variables
				_colorKeyList = new SortedList();
				_sizeKeyList = new SortedList();
				_genericPackName = new ArrayList();
				_nonGenericPackName = new ArrayList();
				_subtotalGenericPackName = new ArrayList();
				_subtotalNonGenericPackName = new ArrayList();
                _genericPackDisplayName = new ArrayList();    // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                _nonGenericPackDisplayName = new ArrayList();  // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                _asrtAsrtType = new Dictionary<int, int>();    // TT#2 - RMatelic - Assortment Planning - Allocation views: don't display Placeholders on Post Receipt assortments
				string subtotalName;
                //int headerCount = 0;                   //TT#2 - RMatelic - Assortment Planning >> unused variables
                _isGAMode = false;      // Begin TT#1194-MD - stodd - view ga header
				foreach (AllocationProfile ap in _apl)
				{
                    // Begin #TT2 - RMatelic - Assortment Planning
                    if (ap.HeaderType == eHeaderType.Assortment)
                    {
                        SetAssortmentType(ap.Key, ap.AsrtType); // TT#2 - RMatelic - Assortment Planning - Allocation views: don't display Placeholders on Post Receipt assortments 
						// Begin TT#1194-MD - stodd - view ga header
                        if (ap.AsrtType != (int)eAssortmentType.GroupAllocation)
                        {
                            continue;
                        }
                        else
                        {
                            _isGAMode = true;
                        }
						// End TT#1194-MD - stodd - view ga header
                    }
                    // End TT#2
                    //Begin TT#2 - RMatelic - Assortment Planning - Allocation views: don't display Placeholders on Post Receipt assortments 
                    else if (ap.HeaderType == eHeaderType.Placeholder)
                    {
						// Begin TT#964 - MD - stodd - style review end up with additional header and bulk components
                        if (GetAssortmentType(ap.AsrtRID) == eAssortmentType.PostReceipt || GetAssortmentType(ap.AsrtRID) == eAssortmentType.GroupAllocation)
		                // End TT#964 - MD - stodd - style review end up with additional header and bulk components
                        {
                            continue;
                        }
                    }
                    // End TT#2
					if (ap.HeaderRID == Include.DefaultHeaderRID)
					{
                        this._atLeast1WorkUpSizeBuy = true;
						ap.TotalUnitsToAllocate = 0;
                        // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28B
                        MIDException midException;
                        if (!ap.SetHeaderType(eHeaderType.WorkupTotalBuy, out midException))
                        {
                            throw midException;
                        }
                        //ap.WorkUpTotalBuy = true;
                        // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28B
						//ap.WorkUpBulkColorBuy = true; // MID Enhancement j.ellis allow workup size buy
						//ap.WorkUpBulkSizeBuy = true;  // MID Enhancement j.ellis allow workup size buy
						// begin MID Track 3815 Size Minimum and Maximum Rules not working
						if (this.NeedAnalysisHNID != Include.NoRID)
						{
							HierarchyNodeProfile hnp = this._trans.GetNodeData(this.NeedAnalysisHNID);
							if (hnp.LevelType == eHierarchyLevelType.Color)
							{
								ap.StyleHnRID = (int)hnp.Parents[0];
								ap.AddBulkColor(hnp.ColorOrSizeCodeRID, ap.TotalUnitsToAllocate, 0);
							}
							else
							{
								if (hnp.LevelType == eHierarchyLevelType.Size)
								{
									HierarchyNodeProfile colorHNP = this._trans.GetNodeData((int)hnp.Parents[0]);
									if (colorHNP.LevelType == eHierarchyLevelType.Color)
									{
										ap.StyleHnRID = (int)colorHNP.Parents[0];
										ap.AddBulkSizeToColor(colorHNP.ColorOrSizeCodeRID,hnp.ColorOrSizeCodeRID, ap.TotalUnitsToAllocate, 0);
									}
									else
									{
										ap.AddBulkSizeToColor(Include.DummyColorRID, hnp.ColorOrSizeCodeRID, ap.TotalUnitsToAllocate, 0);
									}
								}
								else
								{
									ap.AddBulkColor(Include.DummyColorRID, ap.TotalUnitsToAllocate, 0);
								}
							}
						}
						else
						{
							ap.AddBulkColor(Include.DummyColorRID, ap.TotalUnitsToAllocate,0);
						}
						// end MID Track 3815 Size Minimum and Maximum Rules not working

						// begin MID Track 3821 Size Analysis Gets Unhandled Exception
						//ap.AddBulkSizeToColor(Include.DummyColorRID, Include.NoRID, ap.TotalUnitsToAllocate, 0); // MID Enhancement j.ellis allow workup size buy
						// end MID Track 3821 Size Analysis Gets Unhandled Exception
					}
                    // Begin TT#2 - RMatelic - Assortment Planning >> unused variables
                    //_headerID[headerCount] = ap.HeaderID;
                    //headerCount++;
                    // End TT#2
                   
                    // Begin TT#213 - RMatelic - MID Track #6300-Multi header order in the workspace does not match order in style review
                    SortedList sortedPacks = new SortedList();
                    int k=0;  // TT#1206 - MD - Jellis -Group Allocation to Style Review: duplicate Key
                    foreach (PackHdr ph in ap.Packs.Values)
                    {
                        // begin TT#1206 - MD - Jellis -  Group Allocation to Style Review: duplicate key
                        //sortedPacks.Add(ph.Sequence, ph);
                        sortedPacks.Add(ph.Sequence * 1000 + k, ph);
                        k++;
                        // end TT#1206 - MD - Jellis -  Group Allocation to Style Review: duplicate key
                    }
                    //foreach (PackHdr ph in ap.Packs.Values)
                    // begin TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    // Begin TT#4988 - BVaughan - Performance
                    #if DEBUG
                    if ((ap is AssortmentProfile && !ap.isAssortmentProfile) || (!(ap is AssortmentProfile) && ap.isAssortmentProfile))
                    {
                        throw new Exception("Object does not match AssortmentProfile in BuildWafers()");
                    }
                    #endif	
                    //if (ap is AssortmentProfile)
                    if (ap.isAssortmentProfile)
                    // End TT#4988 - BVaughan - Performance
                    {
                        foreach (PackHdr ph in sortedPacks.Values)
                        {
                            subtotalName = ap.GetSubtotalPackName(ph);
                            if (ph.GenericPack)
                            {
                                if (!_genericPackName.Contains(ph.AssortmentPackName))
                                {
                                    _genericPackName.Add(ph.AssortmentPackName);
                                    _subtotalGenericPackName.Add(subtotalName);
                                    _genericPackDisplayName.Add(ph.AssortmentPackName + " (" + ph.PackMultiple.ToString(CultureInfo.CurrentUICulture) + ")");
                                }
                            }
                            else
                            {
                                _detailTypeFound = true;
                                if (!_nonGenericPackName.Contains(ph.AssortmentPackName))
                                {
                                    _nonGenericPackName.Add(ph.AssortmentPackName);
                                    _subtotalNonGenericPackName.Add(subtotalName);
                                    _nonGenericPackDisplayName.Add(ph.AssortmentPackName + "(" + ph.PackMultiple.ToString(CultureInfo.CurrentUICulture) + ")+");
                                }
                            }
                        }
                    }
                    else
                    {
                        // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                        foreach (PackHdr ph in sortedPacks.Values)
                        // End TT#213
                        {
                            subtotalName = ap.GetSubtotalPackName(ph);
                            if (ph.GenericPack)
                            {
                                // Begin TT#4632 - JSmith - Packs with same name/different multiple do not display Pack Allocation in Style Review (by Header) 
                                //if (!_subtotalGenericPackName.Contains(subtotalName)) // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
								//if (!_genericPackName.Contains(ph.PackName))            // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                                if (!_subtotalGenericPackName.Contains(subtotalName)) // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                                // End TT#4632 - JSmith - Packs with same name/different multiple do not display Pack Allocation in Style Review (by Header)
                                {
                                    _genericPackName.Add(ph.PackName);
                                    _subtotalGenericPackName.Add(subtotalName);
                                    _genericPackDisplayName.Add(subtotalName);
                                }
                            }
                            else
                            {
                                _detailTypeFound = true;
                                // Begin TT#4632 - JSmith - Packs with same name/different multiple do not display Pack Allocation in Style Review (by Header)
                                //if (!_subtotalNonGenericPackName.Contains(subtotalName)) // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
								//if (!_nonGenericPackName.Contains(ph.PackName)) // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                                if (!_subtotalNonGenericPackName.Contains(subtotalName)) // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                                // End TT#4632 - JSmith - Packs with same name/different multiple do not display Pack Allocation in Style Review (by Header)
                                {
                                    _nonGenericPackName.Add(ph.PackName);
                                    _subtotalNonGenericPackName.Add(subtotalName);
                                    _nonGenericPackDisplayName.Add(subtotalName);
                                }
                            }
                        }
                    }  // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
					//					if (ap.BulkIsDetail && ap.BulkColors.Count > 0)
					//					{
					//						_detailTypeFound = true;
					//					}
                    foreach (HdrColorBin ch in ap.BulkColors.Values)
					{
						if (!_colorKeyList.Contains(ch.ColorCodeRID))
						{
							// BEGIN MID Track #3231 - Color description change is not reflected in Review
							//_colorKeyList.Add(ch.ColorCodeRID, ch.ColorCodeRID);
							string colorName = string.Empty;
							// BEGIN MID Track #3258 - Size and Need analysis abend when opening	
							string colorCodeID = Include.DummyColorID;
							if (ch.ColorCodeRID != Include.DummyColorRID)
							{
								int styleHnRID = ap.StyleHnRID;
								int colorHnRID = Include.NoRID;
								colorCodeID = _trans.GetColorCodeProfile(ch.ColorCodeRID).ColorCodeID;
								HierarchyNodeProfile hnp_style = _trans.GetNodeData(styleHnRID);
				 
								if(_trans.SAB.HierarchyServerSession.ColorExistsForStyle(hnp_style.HomeHierarchyRID, hnp_style.Key, colorCodeID, hnp_style.QualifiedNodeID, ref colorHnRID))
								{
									HierarchyNodeProfile hnp_color = _trans.GetNodeData(colorHnRID);
									colorName  = hnp_color.NodeDescription;
								}
								else
									colorName  = colorCodeID;
							}
							else
							{
								colorName  = colorCodeID;
							}
							// END MID Track #3258
                            // Begin TT#1782 - RMatelic - Style View IndexOutOfRangeException with duplicate color descriptions >>> moved following code to after SortedList Add
                            //bool dupFound = false;
                            //foreach (ColorData listCD in _colorKeyList.Values)
                            //{
                            //    if (listCD.ColorName == colorName && listCD.ColorID != colorCodeID)
                            //    {
                            //        dupFound = true;
                            //        listCD.IsDuplicateName = true;
                            //    }
                            //}
                            // End TT#1782
							ColorData cd = new ColorData();
							cd.ColorID = colorCodeID;
							cd.ColorName = colorName;
                            //cd.IsDuplicateName = dupFound;         // TT#1782 - RMatelic - Style View IndexOutOfRangeException with duplicate color descriptions >>> moved down to after SortedList Add
                            cd.ColorSequence = ch.ColorSequence;        // Begin TT#213 - RMatelic - MID Track #6300-Multi header order in the workspace does not match order in style review
                            cd.ColorCodeRID = ch.ColorCodeRID;          // End TT#213
                            _colorKeyList.Add(ch.ColorCodeRID, cd);
							// END MID Track #3231
                            // Begin TT#1782 - RMatelic - Style View IndexOutOfRangeException with duplicate color descriptions
                            bool dupFound = false;
                            foreach (ColorData listCD in _colorKeyList.Values)
                            {
                                if (listCD.ColorName.ToUpper() == cd.ColorName.ToUpper() && listCD.ColorCodeRID != cd.ColorCodeRID)
                                {
                                    dupFound = true;
                                    listCD.IsDuplicateName = true;
                                }
                            }
                            cd.IsDuplicateName = dupFound;
                            // End TT#1782
						}
					}
				}
			}

			switch (_sel.ViewType)
			{
				case eAllocationSelectionViewType.Style:
				{
                    // BEGIN TT#2066-MD - AGallagher - Ship to Date validation.  Is this how it should be working
					//BuildStyleView(false);
                    if (_trans.AssortmentView != null)
                    {
					    BuildStyleView(false, true);
                    }
                    else
                    {
                        BuildStyleView(false, false);
                    }
                    // END TT#2066-MD - AGallagher - Ship to Date validation.  Is this how it should be working
					break;
				}
				case eAllocationSelectionViewType.Summary:
				{
					BuildSummaryView();
					break;
				}
				case eAllocationSelectionViewType.Velocity:
				{
                    // BEGIN TT#2066-MD - AGallagher - Ship to Date validation.  Is this how it should be working
					//BuildStyleView(true);
					if (_trans.AssortmentView != null)
                    {
					    BuildStyleView(true, true);
                    }
                    else
                    {
                        BuildStyleView(true, false);
                    }
                    // END TT#2066-MD - AGallagher - Ship to Date validation.  Is this how it should be working
					break;
				}
				case eAllocationSelectionViewType.Size:
				{
                    BuildSizeView();
                  	break;
				}
				default:
                    {
                        //System.Windows.Forms.MessageBox.Show("Selected View is not Implemented");
                        DialogResult diagResult = _trans.SAB.MessageCallback.HandleMessage(
                                            MIDText.GetText(eMIDTextCode.msg_al_SelectedViewNotValid),
                                            "",
                                            MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.None);
                        break;
                    }
			}
			//			this._trans.SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information,"     End:  BuildWafers()", this.GetType().Name);

		}

        // Begin TT#2 - RMatelic - Assortment Planning - Allocation views: don't display Placeholders on Post Receipt assortments 
        private void SetAssortmentType(int asrtRID, int asrtType)
        {
            try
            {
                if (!_asrtAsrtType.ContainsKey(asrtRID))
                {
                    _asrtAsrtType.Add(asrtRID, asrtType);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private eAssortmentType GetAssortmentType(int asrtRID)
        {
            eAssortmentType asrtType = eAssortmentType.Undefined;
            try
            {
                if (_asrtAsrtType.ContainsKey(asrtRID))
                {
                    asrtType = (eAssortmentType)_asrtAsrtType[asrtRID];
                }
                else
                {
					// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
					asrtType = eAssortmentType.Undefined;
                    AllocationProfile ap = (AllocationProfile)_apl.FindKey(asrtRID);
					if (ap != null)
					{
						SetAssortmentType(ap.Key, ap.AsrtType);
						asrtType = (eAssortmentType)(ap.AsrtType);
					}
					// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
                }
            }
            catch (Exception)
            {
                throw;
            }
            return asrtType;
        }
        // End TT#2 
		#endregion BuildControl
		
		#region StyleView
		/// <summary>
		/// Builds the Style Allocation View
		/// </summary>bool 
        // BEGIN TT#2066-MD - AGallagher - Ship to Date validation.  Is this how it should be working
		//private void BuildStyleView(bool aVelocityView)
		private void BuildStyleView(bool aVelocityView, bool aAssortmentView)
        // END TT#2066-MD - AGallagher - Ship to Date validation.  Is this how it should be working
		{
			_wafers = new AllocationWaferBuilderGroup(_trans,3,3);

			AllocationWaferCoordinateListGroup colGroup1 = new AllocationWaferCoordinateListGroup();
			AllocationWaferCoordinateListGroup colGroup2 = new AllocationWaferCoordinateListGroup();
			AllocationWaferCoordinateListGroup colGroup3 = new AllocationWaferCoordinateListGroup();

			AllocationWaferCoordinateListGroup rowGroup1 = new AllocationWaferCoordinateListGroup();
			AllocationWaferCoordinateListGroup rowGroup2 = new AllocationWaferCoordinateListGroup();
			AllocationWaferCoordinateListGroup rowGroup3 = new AllocationWaferCoordinateListGroup();
			AllocationWaferCoordinateList row;
			AllocationWaferCoordinateList col;

            // begin MID Track 6079 Zero Quantity not accepted after Sort
            col = new AllocationWaferCoordinateList(_trans);
            col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
            col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
            col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.SortSequence));
            colGroup1.Add(col);
            // end MID Track 6079 Zero Quantity not accepted after Sort

            // BEGIN TT#2066-MD - AGallagher - Ship to Date validation.  Is this how it should be working
            if (aAssortmentView)
            {
                col = new AllocationWaferCoordinateList(_trans);
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
                col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.AssortmentGrade));
                colGroup1.Add(col);
            }
            // END TT#2066-MD - AGallagher - Ship to Date validation.  Is this how it should be working

			if (aVelocityView)
			{
				col = new AllocationWaferCoordinateList(_trans);
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityGrade));
				colGroup1.Add(col);

                // BEGIN TT#948 - AGallagher - Add  "Grade" to Velocity Store Detail
                col = new AllocationWaferCoordinateList(_trans);
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
                col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreGrade));
                colGroup1.Add(col);
                // END TT#948 - AGallagher - Add  "Grade" to Velocity Store Detail

				col = new AllocationWaferCoordinateList(_trans);
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.PctSellThru));
				colGroup1.Add(col);

				col = new AllocationWaferCoordinateList(_trans);
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.PctSellThruIdx));
				colGroup1.Add(col);

				col = new AllocationWaferCoordinateList(_trans);
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.BasisSales));
				colGroup1.Add(col);

                // BEGIN TT#508 - AGallagher - Velocity - Add WOS to Store Detail (#66)
                col = new AllocationWaferCoordinateList(_trans);
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
                col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.AvgWeeksOfSupply));
                colGroup1.Add(col);
                // END TT#508 - AGallagher - Velocity - Add WOS to Store Detail (#66)

				col = new AllocationWaferCoordinateList(_trans);
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.AvgWeeklySales));
				colGroup1.Add(col);

				col = new AllocationWaferCoordinateList(_trans);
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.AvgWeeklyStock));
				colGroup1.Add(col);

				//				col = new AllocationWaferCoordinateList(_trans);
				//				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				//				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				//				col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.BasisStock));
				//				colGroup1.Add(col);

				col = new AllocationWaferCoordinateList(_trans);
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.BasisOnHand));
				colGroup1.Add(col);

				col = new AllocationWaferCoordinateList(_trans);
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.BasisInTransit));
				colGroup1.Add(col);

                //BEGIN TT#4262-VStuart-Velocity-VSW On Hand at the plan level is used when calculating ship up to, wos and fwos-MID
                col = new AllocationWaferCoordinateList(_trans);
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
                col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.BasisVSWOnHand));
                colGroup1.Add(col);
                //END TT#4262-VStuart-Velocity-VSW On Hand at the plan level is used when calculating ship up to, wos and fwos-MID
			}
			else
			{
				col = new AllocationWaferCoordinateList(_trans);
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreGrade));
				colGroup1.Add(col);

                // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
                col = new AllocationWaferCoordinateList(_trans);
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
                col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.BasisGrade));
                colGroup1.Add(col);

                col = new AllocationWaferCoordinateList(_trans);
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
                col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.BasisSales));
                colGroup1.Add(col);

                col = new AllocationWaferCoordinateList(_trans);
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
                col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.AvgWeeklySales));
                colGroup1.Add(col);

                col = new AllocationWaferCoordinateList(_trans);
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
                col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.AvgWeeklyStock));
                colGroup1.Add(col);
                // End TT#638  
			}

			col = new AllocationWaferCoordinateList(_trans);
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.OnHand));
			colGroup1.Add(col);

			col = new AllocationWaferCoordinateList(_trans);
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.InTransit));
			colGroup1.Add(col);

            // BEGIN TT#1401 - AGallagher - Reservation Stores
            col = new AllocationWaferCoordinateList(_trans);
            col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
            col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
            col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOHistoryMaxQuantityAllocated));
            colGroup1.Add(col);
            // END TT#1401 - AGallagher - Reservation Stores

			col = new AllocationWaferCoordinateList(_trans);
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.Sales));
			colGroup1.Add(col);

			col = new AllocationWaferCoordinateList(_trans);
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.Stock));
			colGroup1.Add(col);

			col = new AllocationWaferCoordinateList(_trans);
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.Need));
			colGroup1.Add(col);

			col = new AllocationWaferCoordinateList(_trans);
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.PercentNeed));
			colGroup1.Add(col);

			col = new AllocationWaferCoordinateList(_trans);
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.CurrentWeekToDaySales));
			colGroup1.Add(col);
			for (int i=0;i<_wafers.ColumnCount;i++)
			{
				_wafers[i,0].Columns = colGroup1;
			}

			AllocationWaferCoordinateList coordListA;		
			// begin TT#59 Implement Store Temp Locks
            //AllocationWaferCoordinateList coordListB = new AllocationWaferCoordinateList(_trans);  
			//AllocationWaferCoordinateList coordListC = new AllocationWaferCoordinateList(_trans);

            AllocationWaferCoordinateList totalCoordListB = new AllocationWaferCoordinateList(_trans);
            AllocationWaferCoordinateList headerCoordListB = new AllocationWaferCoordinateList(_trans);
            AllocationWaferCoordinateList totalCoordListC = new AllocationWaferCoordinateList(_trans);
            AllocationWaferCoordinateList headerCoordListC = new AllocationWaferCoordinateList(_trans);
            // end TT#59 Implement Store Temp Locks
                    
			AllocationWaferCoordinate aWaferCoordinate;
			if (aVelocityView)
			{
				aWaferCoordinate = new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, _styleOnHandLabel);
				headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
				aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StyleOnHand);
				headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

				aWaferCoordinate = new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, _styleInTransitLabel);
				headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
				aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StyleInTransit);
				headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                // BEGIN TT#1401 - AGallagher - Reservation Stores
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, _qtyStoreIMOHistMaxLabel);
                headerCoordListB.Add(aWaferCoordinate); 
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOHistoryMaxQuantityAllocated);
                headerCoordListC.Add(aWaferCoordinate); 
                // END TT#1401 - AGallagher - Reservation Stores

			}
			// begin MID Track 3880 Add Ship to day in Style and Size Review
			aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total,0,MIDText.GetTextOnly((int)eAllocationWaferVariable.ShipToDay));
			headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
			aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.ShipToDay);
			headerCoordListC.Add(aWaferCoordinate); // TT#59 Implemnent Store Temp Locks
			// end MID Track 3880 Add Ship to day in Style and Size Review
			
			// begin MID Track 4291 Add Fill Variables to Size Review
			aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total,0,MIDText.GetTextOnly((int)eAllocationWaferVariable.NeedDay));
			headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
			aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.NeedDay);
			headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
			// end MID Track 4291 Add Fill Variables to Size Review

			//			aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total,0,"Alloc Qty");
			aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total,0,_qtyAllocatedLabel);
			totalCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
            headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
			aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QuantityAllocated);
			totalCoordListC.Add(aWaferCoordinate);  // TT#59 Implement Store Temp Locks
            headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

            // BEGIN TT#1401 - AGallagher - Reservation Stores
            aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _qtyStoreItemLabel);
            totalCoordListB.Add(aWaferCoordinate);
            headerCoordListB.Add(aWaferCoordinate);
            aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreItemQuantityAllocated);
            totalCoordListC.Add(aWaferCoordinate);
            headerCoordListC.Add(aWaferCoordinate);

            aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _qtyStoreIMOLabel);
            totalCoordListB.Add(aWaferCoordinate);
            headerCoordListB.Add(aWaferCoordinate);
            aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOQuantityAllocated);
            totalCoordListC.Add(aWaferCoordinate);
            headerCoordListC.Add(aWaferCoordinate);

            aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _qtyStoreIMOMaxLabel);
            totalCoordListB.Add(aWaferCoordinate);
            headerCoordListB.Add(aWaferCoordinate);
            aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOMaxQuantityAllocated);
            totalCoordListC.Add(aWaferCoordinate);
            headerCoordListC.Add(aWaferCoordinate);
            // END TT#1401 - AGallagher - Reservation Stores
					
			aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total,0,_ruleLabel);
			headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
			aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AppliedRule);
			headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

			aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total,0,_ruleResultLabel);
			headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
			aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleResults);
			headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

            if (this._trans.Velocity == null)
            {
                aVelocityView = false;
            }

			if (aVelocityView && this._trans.Velocity.Component.ComponentType == eComponentType.Total)
			{
				aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total,0,this._velocityRuleLabel);
				headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
				aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleType);
				headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, this._velocityRuleTypeQtyLabel);
                //coordListB.Add(aWaferCoordinate);   // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
				headerCoordListB.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleTypeQty);
                //coordListC.Add(aWaferCoordinate);  // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
				headerCoordListC.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

				aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total,0,_velocityRuleQtyLabel);
				headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
				aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleQty);
				headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

				aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total,0,_velocityRuleResultLabel);
				headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
				aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleResult);
				headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

				aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total,0,_transferLabel);
				headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
				aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.Transfer);
				headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

				// begin MID Track 4282 Velocity overlays Fill Size Holes Allocation
				aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total,0,_preSizeAllocatedLabel);
				headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
				aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.PreSizeAllocated);
				headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
				// end MID Track 4282 Velocity overlays Fill Size Holes Allocation

                //begin tt#152 Velocity balance - apicchetti

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _velocityInitialRuleTypeLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialRuleType);
                headerCoordListC.Add(aWaferCoordinate);

                // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _velocityInitialRuleTypeQtyLabel);
                //coordListB.Add(aWaferCoordinate);   // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
				headerCoordListB.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialRuleTypeQty);
                //coordListC.Add(aWaferCoordinate);   // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
				headerCoordListC.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _velocityInitialRuleQtyLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialRuleQty);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _velocityInitialWillShipLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialWillShip);
                headerCoordListC.Add(aWaferCoordinate);
                //end tt#152 Velocity balance - apicchetti
			}

            // begin TT#59 Implement Store Temp Locks
            if (this._trans.SAB.AllowDebugging)
            {

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _shipStatusLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.ShippingStatus);
                headerCoordListC.Add(aWaferCoordinate);


                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _qtyShippedLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QtyShipped);
                headerCoordListC.Add(aWaferCoordinate);
                
                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.CapacityMaximum).DefaultLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.CapacityMaximum);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.CapacityExceedByPct).DefaultLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.CapacityExceedByPct);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.StoreUsedCapacity).DefaultLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreUsedCapacity);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.AvailableCapacity).DefaultLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AvailableCapacity);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.CapacityMaximumReached).DefaultLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.CapacityMaximumReached);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.StoreMaximum).DefaultLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreMaximum);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.StoreMayExceedMaximum).DefaultLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreMayExceedMaximum);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.StoreMinimum).DefaultLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreMinimum);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, AllocationWaferVariables.GetVariableProfile(eAllocationWaferVariable.StorePercentNeedLimitReached).DefaultLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StorePercentNeedLimitReached);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _allocationFromBottomUpSizeLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AllocationFromBottomUpSize);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _allocationModifiedAftMultiSplitLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AllocationModifiedAftMultiSplit);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _storeHadNeedLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreHadNeed);
                headerCoordListC.Add(aWaferCoordinate);

                // Unit Need Before is not being populated (uncomment lines below if it becomes populated)
                //aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _unitNeedBeforeLabel);
                //headerCoordListB.Add(aWaferCoordinate);
                //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.UnitNeedBefore);
                //headerCoordListC.Add(aWaferCoordinate);
                //
                //aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _percentNeedBeforeLabel);
                //headerCoordListB.Add(aWaferCoordinate);
                //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.PercentNeedBefore);
                //headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _wasAutoAllocatedLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.WasAutoAllocated);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _qtyAllocatedByAutoLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QtyAllocatedByAuto);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _ruleAllocationFromChildLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleAllocationFromChild);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _ruleAllocationFromChosenRuleLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleAllocationFromChosenRule);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _qtyAllocatedByRuleLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QtyAllocatedByRule);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total, 0, _storeManuallyAllocatedLabel);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreManuallyAllocated);
                headerCoordListC.Add(aWaferCoordinate);
            }
            // end TT#59 Implement Store Temp Locks


			AllocationPackComponent apc;
			if (aVelocityView 
				&& this._trans.Velocity.Component.ComponentType == eComponentType.SpecificPack)
			{
				apc = (AllocationPackComponent)this._trans.Velocity.Component;
			}
			else
			{
				apc = null;
			}
			string packNameDisplay;
			string subtotalPackNameDisplay;
            string packColumnNameDisplay; // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
            Dictionary<string, bool> subtotalExistDictionary = new Dictionary<string, bool>(); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
            bool subtotalExists; // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
			if (_genericPackName.Count > 0)
			{
                for (int iPack = 0; iPack < _subtotalGenericPackName.Count; iPack++)
				{
					packNameDisplay = (string)_genericPackName[iPack];
					subtotalPackNameDisplay = (string)_subtotalGenericPackName[iPack];
                    packColumnNameDisplay = (string)_genericPackDisplayName[iPack]; // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    // begin TT#59 Implement Store Temp Lock
					//coordListB.Add(new AllocationWaferCoordinate(  
					//	packNameDisplay, 
					//	subtotalPackNameDisplay, 
					//	subtotalPackNameDisplay)); // JAE
					//coordListC.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.QuantityAllocated));
                    // begin  TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    #region Obsolete CODE -- TT#1202
                    //aWaferCoordinate = new AllocationWaferCoordinate(
                    //    packNameDisplay,
                    //    subtotalPackNameDisplay,
                    //    subtotalPackNameDisplay);
                    //totalCoordListB.Add(aWaferCoordinate);
                    //headerCoordListB.Add(aWaferCoordinate);
                    //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QuantityAllocated);  
                    //totalCoordListC.Add(aWaferCoordinate);
                    //headerCoordListC.Add(aWaferCoordinate);
                    //// end TT#59 Implement Store Temp Lock

                    //// BEGIN TT#1401 - AGallagher - Reservation Stores
                    //aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, subtotalPackNameDisplay + " " + _qtyStoreItemLabel);
                    //totalCoordListB.Add(aWaferCoordinate); 
                    //headerCoordListB.Add(aWaferCoordinate); 
                    //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreItemQuantityAllocated);
                    //totalCoordListC.Add(aWaferCoordinate);  
                    //headerCoordListC.Add(aWaferCoordinate);

                    //aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, subtotalPackNameDisplay + " " + _qtyStoreIMOLabel);
                    //totalCoordListB.Add(aWaferCoordinate);
                    //headerCoordListB.Add(aWaferCoordinate);
                    //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOQuantityAllocated);
                    //totalCoordListC.Add(aWaferCoordinate);
                    //headerCoordListC.Add(aWaferCoordinate);

                    //// begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 27
                    ////aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, subtotalPackNameDisplay + " " + _qtyStoreIMOMaxLabel);
                    ////totalCoordListB.Add(aWaferCoordinate);
                    ////headerCoordListB.Add(aWaferCoordinate);
                    ////aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOMaxQuantityAllocated);
                    ////totalCoordListC.Add(aWaferCoordinate);
                    ////headerCoordListC.Add(aWaferCoordinate); 
                    //// end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 27
                    //// END TT#1401 - AGallagher - Reservation Stores
							
                    //aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay,subtotalPackNameDisplay,subtotalPackNameDisplay + " " + _ruleLabel);
                    //totalCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                    //headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                    //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AppliedRule);
                    //totalCoordListC.Add(aWaferCoordinate);  // TT#59 Implement Store Temp Locks
                    //headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                    //aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay,subtotalPackNameDisplay,subtotalPackNameDisplay + " " + _ruleResultLabel);
                    //totalCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                    //headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                    //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleResults);
                    //totalCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                    //headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                    #endregion Obsolete CODE -- TT#1202
                    if (!subtotalExistDictionary.TryGetValue(subtotalPackNameDisplay, out subtotalExists))
                    {
                        subtotalExistDictionary.Add(subtotalPackNameDisplay, true);
                        aWaferCoordinate = new AllocationWaferCoordinate(
                            packNameDisplay,
                            subtotalPackNameDisplay,
                            packColumnNameDisplay);
                        totalCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QuantityAllocated);
                        totalCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _qtyStoreItemLabel);
                        totalCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreItemQuantityAllocated);
                        totalCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _qtyStoreIMOLabel);
                        totalCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOQuantityAllocated);
                        totalCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _ruleLabel);
                        totalCoordListB.Add(aWaferCoordinate); 
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AppliedRule);
                        totalCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _ruleResultLabel);
                        totalCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleResults);
                        totalCoordListC.Add(aWaferCoordinate); 
                    }

                    aWaferCoordinate = new AllocationWaferCoordinate(
                        packNameDisplay,
                        subtotalPackNameDisplay,
                        packColumnNameDisplay);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QuantityAllocated);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _qtyStoreItemLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreItemQuantityAllocated);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _qtyStoreIMOLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOQuantityAllocated);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _ruleLabel);
                    headerCoordListB.Add(aWaferCoordinate); 
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AppliedRule);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _ruleResultLabel);
                    headerCoordListB.Add(aWaferCoordinate); 
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleResults);
                    headerCoordListC.Add(aWaferCoordinate);
                    // end  TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review

					if (apc != null && apc.PackName == packNameDisplay)
					{
                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _velocityRuleLabel);  // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
						headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
						aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleType);
						headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                        // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _velocityRuleTypeQtyLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                        //coordListB.Add(aWaferCoordinate);   // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
						headerCoordListB.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleTypeQty);
                        //coordListC.Add(aWaferCoordinate);   // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
						headerCoordListC.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                        // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _velocityRuleQtyLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                        headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
						aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleQty);
                        headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _velocityRuleResultLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                        headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
						aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleResult);
                        headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _transferLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                        headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks 
						aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.Transfer);
                        headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                        //begin tt#152 Velocity balance - apicchetti
                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _velocityInitialRuleTypeLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialRuleType);
                        headerCoordListC.Add(aWaferCoordinate); 

                        // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _velocityInitialRuleTypeQtyLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                        //coordListB.Add(aWaferCoordinate);   // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
						headerCoordListB.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialRuleTypeQty);
                        //coordListC.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
						headerCoordListC.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                        // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _velocityInitialRuleQtyLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialRuleQty);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _velocityInitialWillShipLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialWillShip);
                        headerCoordListC.Add(aWaferCoordinate);
                        //end tt#152 Velocity balance - apicchetti
					}

                    // begin TT#59 Implement Store Temp Locks
                    if (this._trans.SAB.AllowDebugging)
                    {
                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _shipStatusLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.ShippingStatus);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _qtyShippedLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QtyShipped);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _allocationModifiedAftMultiSplitLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AllocationModifiedAftMultiSplit);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _storeHadNeedLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreHadNeed);
                        headerCoordListC.Add(aWaferCoordinate);

                        // Unit Need Before is not being populated (uncomment lines below if it becomes populated)
                        //aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, subtotalPackNameDisplay + " " + _unitNeedBeforeLabel);
                        //headerCoordListB.Add(aWaferCoordinate);
                        //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.UnitNeedBefore);
                        //headerCoordListC.Add(aWaferCoordinate);
                        //
                        //aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, subtotalPackNameDisplay + " " + _percentNeedBeforeLabel);
                        //headerCoordListB.Add(aWaferCoordinate);
                        //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.PercentNeedBefore);
                        //headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _wasAutoAllocatedLabel);  // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.WasAutoAllocated);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _qtyAllocatedByAutoLabel);  // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QtyAllocatedByAuto);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _ruleAllocationFromParentLabel);  // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleAllocationFromParent);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _ruleAllocationFromChosenRuleLabel);  // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleAllocationFromChosenRule);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _qtyAllocatedByRuleLabel);  // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QtyAllocatedByRule);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _storeManuallyAllocatedLabel);  // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreManuallyAllocated);
                        headerCoordListC.Add(aWaferCoordinate);
                    }
                    // end TT#59 Implement Store Temp Locks

				}
			}

            if (_detailTypeFound)
            {
                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel);
                totalCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QuantityAllocated);
                totalCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                // BEGIN TT#1401 - AGallagher - Reservation Stores
                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _qtyStoreItemLabel);
                totalCoordListB.Add(aWaferCoordinate);
                headerCoordListB.Add(aWaferCoordinate); 
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreItemQuantityAllocated);
                totalCoordListC.Add(aWaferCoordinate);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _qtyStoreIMOLabel);
                totalCoordListB.Add(aWaferCoordinate);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOQuantityAllocated);
                totalCoordListC.Add(aWaferCoordinate);
                headerCoordListC.Add(aWaferCoordinate);

                // begin TT#1401 - JEllis - Virtual Store Warehouse pt 27
                //aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _qtyStoreIMOMaxLabel);
                //totalCoordListB.Add(aWaferCoordinate);
                //headerCoordListB.Add(aWaferCoordinate);
                //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOMaxQuantityAllocated);
                //totalCoordListC.Add(aWaferCoordinate);
                //headerCoordListC.Add(aWaferCoordinate); 
                // end TT#1401 - JEllis - Virtual Store Warehouse pt 27
                // END TT#1401 - AGallagher - Reservation Stores

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _ruleLabel);
                headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AppliedRule);
                headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _ruleResultLabel);
                headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleResults);
                headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                if (aVelocityView && this._trans.Velocity.Component.ComponentType == eComponentType.DetailType)
                {
                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _velocityRuleLabel);
                    headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleType);
                    headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                    // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _velocityRuleTypeQtyLabel);
                    //coordListB.Add(aWaferCoordinate);  // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
					headerCoordListB.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleTypeQty);
                    //coordListC.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                    headerCoordListC.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                    // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

					aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType,0,this._detailComponentLabel + " " + _velocityRuleQtyLabel);
                    headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
					aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleQty);
                    headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

					aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType,0,this._detailComponentLabel + " " + _velocityRuleResultLabel);
                    headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
					aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleResult);
                    headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

					aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType,0,this._detailComponentLabel + " " + _transferLabel);
                    headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
					aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.Transfer);
                    headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

					// begin MID Track 4282 Velocity overlays Fill Size Holes Allocation
					aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType,0,this._detailComponentLabel + " " + _preSizeAllocatedLabel);
                    headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
					aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.PreSizeAllocated);
                    headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
					// end MID Track 4282 Velocity overlays Fill Size Holes Allocation

                    //begin tt#152 Velocity balance - apicchetti
                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _velocityInitialRuleTypeLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialRuleType);
                    headerCoordListC.Add(aWaferCoordinate);

                    // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)    
                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _velocityInitialRuleTypeQtyLabel);
                    //coordListB.Add(aWaferCoordinate);  // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
					headerCoordListB.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialRuleTypeQty);
                    //coordListC.Add(aWaferCoordinate);  // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
					headerCoordListC.Add(aWaferCoordinate); // TT#59 Temp Lock -move from ver 3.1 to ver 4.0
                    // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _velocityInitialRuleQtyLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialRuleQty);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _velocityInitialWillShipLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialWillShip);
                    headerCoordListC.Add(aWaferCoordinate);
                    //end tt#152 Velocity balance - apicchetti
                }

                // begin TT#59 Implement Store Temp Locks
                if (this._trans.SAB.AllowDebugging)
                {
                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _allocationModifiedAftMultiSplitLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AllocationModifiedAftMultiSplit);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _storeHadNeedLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreHadNeed);
                    headerCoordListC.Add(aWaferCoordinate);

                    // Unit Need Before is not being populated (uncomment lines below if it becomes populated)
                    //aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _unitNeedBeforeLabel);
                    //headerCoordListB.Add(aWaferCoordinate);
                    //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.UnitNeedBefore);
                    //headerCoordListC.Add(aWaferCoordinate);
                    //
                    //aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _percentNeedBeforeLabel);
                    //headerCoordListB.Add(aWaferCoordinate);
                    //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.PercentNeedBefore);
                    //headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _wasAutoAllocatedLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.WasAutoAllocated);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _qtyAllocatedByAutoLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QtyAllocatedByAuto);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _ruleAllocationFromParentLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleAllocationFromParent);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _ruleAllocationFromChildLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleAllocationFromChild);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _ruleAllocationFromChosenRuleLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleAllocationFromChosenRule);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _qtyAllocatedByRuleLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QtyAllocatedByRule);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.DetailType, 0, this._detailComponentLabel + " " + _storeManuallyAllocatedLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreManuallyAllocated);
                    headerCoordListC.Add(aWaferCoordinate);
                }
                // end TT#59 Implement Store Temp Locks

            }


            for (int iPack = 0; iPack < _subtotalNonGenericPackName.Count; iPack++)
            {
                packNameDisplay = (string)_nonGenericPackName[iPack];
                subtotalPackNameDisplay = (string)_subtotalNonGenericPackName[iPack];
                packColumnNameDisplay = (string)_nonGenericPackDisplayName[iPack]; // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                // begin TT#59 Implement Store Temp Lock
                //coordListB.Add(new AllocationWaferCoordinate(  
                //	packNameDisplay, 
                //	subtotalPackNameDisplay, 
                //	subtotalPackNameDisplay)); // JAE
                //coordListC.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.QuantityAllocated));

                // begin TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                #region Obsolete CODE -- TT#1202
                //aWaferCoordinate = new AllocationWaferCoordinate(
                //    packNameDisplay,
                //    subtotalPackNameDisplay,
                //    subtotalPackNameDisplay);
                //totalCoordListB.Add(aWaferCoordinate);
                //headerCoordListB.Add(aWaferCoordinate);
                //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QuantityAllocated);    
                //totalCoordListC.Add(aWaferCoordinate);
                //headerCoordListC.Add(aWaferCoordinate);
                //// end TT#59 Implement Store Temp Lock	

                //// BEGIN TT#1401 - AGallagher - Reservation Stores
                //aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, subtotalPackNameDisplay + " " + _qtyStoreItemLabel);
                //totalCoordListB.Add(aWaferCoordinate);
                //headerCoordListB.Add(aWaferCoordinate);
                //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreItemQuantityAllocated);
                //totalCoordListC.Add(aWaferCoordinate);
                //headerCoordListC.Add(aWaferCoordinate);

                //aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, subtotalPackNameDisplay + " " + _qtyStoreIMOLabel);
                //totalCoordListB.Add(aWaferCoordinate);
                //headerCoordListB.Add(aWaferCoordinate);
                //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOQuantityAllocated);
                //totalCoordListC.Add(aWaferCoordinate);
                //headerCoordListC.Add(aWaferCoordinate);

                //// begin TT#1401 - JEllis - Virtual Store Warehouse pt 27
                ////aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, subtotalPackNameDisplay + " " + _qtyStoreIMOMaxLabel);
                ////totalCoordListB.Add(aWaferCoordinate);
                ////headerCoordListB.Add(aWaferCoordinate);
                ////aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOHistoryMaxQuantityAllocated);
                ////totalCoordListC.Add(aWaferCoordinate);
                ////headerCoordListC.Add(aWaferCoordinate);
                //// end TT#1401 - JEllis - Virtual Store Warehouse pt 27
                //// END TT#1401 - AGallagher - Reservation Stores

                //aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, subtotalPackNameDisplay + " " + _ruleLabel);
                //headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AppliedRule);
                //headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                //aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, subtotalPackNameDisplay + " " + _ruleResultLabel);
                //headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleResults);
                //headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                #endregion Obsolete CODE -- TT#1202

                if (!subtotalExistDictionary.TryGetValue(subtotalPackNameDisplay, out subtotalExists))
                {
                    subtotalExistDictionary.Add(subtotalPackNameDisplay, true);
                    aWaferCoordinate = new AllocationWaferCoordinate(
                         packNameDisplay,
                         subtotalPackNameDisplay,
                         packColumnNameDisplay);
                    totalCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QuantityAllocated);
                    totalCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _qtyStoreItemLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    totalCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreItemQuantityAllocated);
                    totalCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _qtyStoreIMOLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    totalCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOQuantityAllocated);
                    totalCoordListC.Add(aWaferCoordinate);
                }

                
                aWaferCoordinate = new AllocationWaferCoordinate(
                     packNameDisplay,
                     subtotalPackNameDisplay,
                     packColumnNameDisplay);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QuantityAllocated);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _qtyStoreItemLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreItemQuantityAllocated);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _qtyStoreIMOLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOQuantityAllocated);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _ruleLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AppliedRule);
                headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _ruleResultLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleResults);
                headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                // end TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review

                if (apc != null && apc.PackName == packNameDisplay)
                {
                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _velocityRuleLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks 
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleType);
                    headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                    // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _velocityRuleTypeQtyLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    //coordListB.Add(aWaferCoordinate);  // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
					headerCoordListB.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleTypeQty);
                    //coordListC.Add(aWaferCoordinate);  // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
					headerCoordListC.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                    // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _velocityRuleQtyLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
					aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleQty);
                    headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _velocityRuleResultLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
					aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleResult);
                    headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _transferLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
					aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.Transfer);
                    headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                    //begin tt#509 Velocity balance not working
                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _velocityInitialRuleTypeLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialRuleType);
                    headerCoordListC.Add(aWaferCoordinate);

                    // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _velocityInitialRuleTypeQtyLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    //coordListB.Add(aWaferCoordinate);  // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
					headerCoordListB.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialRuleTypeQty);
                    //coordListC.Add(aWaferCoordinate);  // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
					headerCoordListC.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                    // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _velocityInitialRuleQtyLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialRuleQty);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _velocityInitialWillShipLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialWillShip);
                    headerCoordListC.Add(aWaferCoordinate);
                    //end tt#509 Velocity balance not working
				}
                // begin TT#59 Implement Store Temp Locks
                if (this._trans.SAB.AllowDebugging)
                {
                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _shipStatusLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.ShippingStatus);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _qtyShippedLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QtyShipped);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _allocationModifiedAftMultiSplitLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AllocationModifiedAftMultiSplit);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _storeHadNeedLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreHadNeed);
                    headerCoordListC.Add(aWaferCoordinate);

                    // Unit Need Before is not being populated (uncomment lines below if it becomes populated)
                    //aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, subtotalPackNameDisplay + " " + _unitNeedBeforeLabel);
                    //headerCoordListB.Add(aWaferCoordinate);
                    //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.UnitNeedBefore);
                    //headerCoordListC.Add(aWaferCoordinate);
                    //
                    //aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, subtotalPackNameDisplay + " " + _percentNeedBeforeLabel);
                    //headerCoordListB.Add(aWaferCoordinate);
                    //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.PercentNeedBefore);
                    //headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _wasAutoAllocatedLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.WasAutoAllocated);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _qtyAllocatedByAutoLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QtyAllocatedByAuto);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _ruleAllocationFromParentLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleAllocationFromParent);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _ruleAllocationFromChosenRuleLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleAllocationFromChosenRule);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _qtyAllocatedByRuleLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QtyAllocatedByRule);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _allocationFromPackNeedLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AllocationFromPackNeed);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(packNameDisplay, subtotalPackNameDisplay, packColumnNameDisplay + " " + _storeManuallyAllocatedLabel); // TT#1202 - MD - Jellis - System Argument Except - cannot view Group with packs in Style Review
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreManuallyAllocated);
                    headerCoordListC.Add(aWaferCoordinate);
                }
                // end TT#59 Implement Store Temp Locks
            }

			if (_colorKeyList.Count > 0)
			{
				aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk,0,this._bulkComponentLabel);
                totalCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
				aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QuantityAllocated);
                totalCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                // BEGIN TT#1401 - AGallagher - Reservation Stores
                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _qtyStoreItemLabel);
                totalCoordListB.Add(aWaferCoordinate);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreItemQuantityAllocated);
                totalCoordListC.Add(aWaferCoordinate);
                headerCoordListC.Add(aWaferCoordinate);

                aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _qtyStoreIMOLabel);
                totalCoordListB.Add(aWaferCoordinate);
                headerCoordListB.Add(aWaferCoordinate);
                aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOQuantityAllocated);
                totalCoordListC.Add(aWaferCoordinate);
                headerCoordListC.Add(aWaferCoordinate);

                // begin TT#1401 - JEllis - Virtual Store Warehouse pt 27
                //aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _qtyStoreIMOMaxLabel);
                //totalCoordListB.Add(aWaferCoordinate);
                //headerCoordListB.Add(aWaferCoordinate);
                //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOMaxQuantityAllocated);
                //totalCoordListC.Add(aWaferCoordinate);
                //headerCoordListC.Add(aWaferCoordinate);
                // end TT#1401 - JEllis - Virtual Store Warehouse pt 27
                // END TT#1401 - AGallagher - Reservation Stores

				aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk,0,this._bulkComponentLabel + " " + _ruleLabel);
                headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
				aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AppliedRule);
                headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

				aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk,0,this._bulkComponentLabel + " " + _ruleResultLabel);
                headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
				aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleResults);
                headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                 
				if (aVelocityView && this._trans.Velocity.Component.ComponentType == eComponentType.Bulk)
				{
					aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk,0,this._bulkComponentLabel + " " + _velocityRuleLabel);
                    headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
					aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleType);
                    headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                    // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _velocityRuleTypeQtyLabel);
                    //coordListB.Add(aWaferCoordinate);  // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
					headerCoordListB.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleTypeQty);
                    //coordListC.Add(aWaferCoordinate);    // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
					headerCoordListC.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                    // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

					aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk,0,this._bulkComponentLabel + " " + _velocityRuleQtyLabel);
                    headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
					aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleQty);
                    headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

					aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk,0,this._bulkComponentLabel + " " + _velocityRuleResultLabel);
                    headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
					aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleResult);
                    headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

					aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk,0,this._bulkComponentLabel + " " + _transferLabel);
                    headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
					aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.Transfer);
                    headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

					// begin MID Track 4282 Velocity overlays Fill Size Holes Allocation
					aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk,0,this._bulkComponentLabel + " " + _preSizeAllocatedLabel);
                    headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
					aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.PreSizeAllocated);
                    headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
					// end MID Track 4282 Velocity overlays Fill Size Holes Allocation

                    //begin tt#152 Velocity balance - apicchetti
                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _velocityInitialRuleTypeLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialRuleType);
                    headerCoordListC.Add(aWaferCoordinate);

                    // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _velocityInitialRuleTypeQtyLabel);
                    //coordListB.Add(aWaferCoordinate);   // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
					headerCoordListB.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialRuleTypeQty);
                    //coordListC.Add(aWaferCoordinate);   // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
					headerCoordListC.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                    // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _velocityInitialRuleQtyLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialRuleQty);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _velocityInitialWillShipLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialWillShip);
                    headerCoordListC.Add(aWaferCoordinate);
                    //end tt#152 Velocity balance - apicchetti
                }

                // begin TT#59 Implement Store Temp Locks
                if (this._trans.SAB.AllowDebugging)
                {
                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _allocationModifiedAftMultiSplitLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AllocationModifiedAftMultiSplit);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _storeHadNeedLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreHadNeed);
                    headerCoordListC.Add(aWaferCoordinate);

                    // Unit Need Before is not being populated (uncomment lines below if it becomes populated)
                    //aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _unitNeedBeforeLabel);
                    //headerCoordListB.Add(aWaferCoordinate);
                    //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.UnitNeedBefore);
                    //headerCoordListC.Add(aWaferCoordinate);
                    //
                    //aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _percentNeedBeforeLabel);
                    //headerCoordListB.Add(aWaferCoordinate);
                    //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.PercentNeedBefore);
                    //headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _wasAutoAllocatedLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.WasAutoAllocated);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _qtyAllocatedByAutoLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QtyAllocatedByAuto);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _ruleAllocationFromParentLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleAllocationFromParent);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _ruleAllocationFromChildLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleAllocationFromChild);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _ruleAllocationFromChosenRuleLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleAllocationFromChosenRule);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _qtyAllocatedByRuleLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QtyAllocatedByRule);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _allocationFromPackNeedLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AllocationFromPackNeed);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _allocationFromBottomUpSizeLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AllocationFromBottomUpSize);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk, 0, this._bulkComponentLabel + " " + _storeManuallyAllocatedLabel);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreManuallyAllocated);
                    headerCoordListC.Add(aWaferCoordinate);
                }
                // end TT#59 Implement Store Temp Locks



                AllocationColorOrSizeComponent colorComponent;
				if (aVelocityView && this._trans.Velocity.Component.ComponentType == eComponentType.SpecificColor)
				{
					colorComponent = (AllocationColorOrSizeComponent)this._trans.Velocity.Component;
				}
				else
				{
					colorComponent = null;
				}
				// BEGIN MID Track #3231 - Color description change is not reflected in Review
				//foreach (int cKey in _colorKeyList.Values) 
                // Begin TT#213 - RMatelic - MID Track #6300-Multi header order in the workspace does not match order in style review
                SortedList sortedBulkColors = new SortedList();
                foreach (ColorData cd in _colorKeyList.Values)
                {
                    // Begin TT#326 - RMatelic - highlight 2 header; select style view;get system argument exception error message
                    //sortedBulkColors.Add(cd.ColorSequence, cd);
                    while (sortedBulkColors.ContainsKey(cd.ColorSequence))
                    {
                        cd.ColorSequence++;
                    }
                    sortedBulkColors.Add(cd.ColorSequence, cd);
                    // End TT#326
                }
                //foreach (int cKey in _colorKeyList.Keys) 
                foreach (ColorData cd in sortedBulkColors.Values)
                // End TT#213
                {
					//string colorName = _trans.GetColorCodeProfile(cKey).ColorCodeName;
					string colorName = string.Empty;
                    // Begin TT#213 - RMatelic - MID Track #6300-Multi header order in the workspace does not match order in style review
					//ColorData cd = (ColorData)_colorKeyList[cKey]; 
                    int cKey = cd.ColorCodeRID;
                    // End TT#213
					if (cd.IsDuplicateName)
						colorName = cd.ColorID + " " + cd.ColorName;
					else
						colorName = cd.ColorName;
				// END MID Track #3231

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName);
                    totalCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                    headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QuantityAllocated);    
                    totalCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                    headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                    // BEGIN TT#1401 - AGallagher - Reservation Stores
                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _qtyStoreItemLabel);
                    totalCoordListB.Add(aWaferCoordinate);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreItemQuantityAllocated);
                    totalCoordListC.Add(aWaferCoordinate);
                    headerCoordListC.Add(aWaferCoordinate);

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _qtyStoreIMOLabel);
                    totalCoordListB.Add(aWaferCoordinate);
                    headerCoordListB.Add(aWaferCoordinate);
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOQuantityAllocated);
                    totalCoordListC.Add(aWaferCoordinate);
                    headerCoordListC.Add(aWaferCoordinate);

                    // begin TT#1401 - JEllis - Virtual Store Warehouse pt 27
                    //aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _qtyStoreIMOMaxLabel);
                    //totalCoordListB.Add(aWaferCoordinate);
                    //headerCoordListB.Add(aWaferCoordinate);
                    //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOMaxQuantityAllocated);
                    //totalCoordListC.Add(aWaferCoordinate);
                    //headerCoordListC.Add(aWaferCoordinate); 
                    // end TT#1401 - JEllis - Virtual Store Warehouse pt 27
                    // END TT#1401 - AGallagher - Reservation Stores

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _ruleLabel);
                    headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AppliedRule);
                    headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

                    aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _ruleResultLabel);
                    headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                    aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleResults);
                    headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

					if (colorComponent != null && colorComponent.ColorRID == cKey)
					{
						aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor,cKey,colorName + " " + _velocityRuleLabel);
                        headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
						aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleType);
                        headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
                        
                        // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                        aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _velocityRuleTypeQtyLabel);
                        //coordListB.Add(aWaferCoordinate);   // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
						headerCoordListB.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleTypeQty);
                        //coordListC.Add(aWaferCoordinate);   // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
						headerCoordListC.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                        // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

						aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor,cKey,colorName + " " + _velocityRuleQtyLabel);
                        headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
						aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleQty);
                        headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

						aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor,cKey,colorName + " " + _velocityRuleResultLabel);
                        headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
						aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityRuleResult);
                        headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

						aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor,cKey,colorName + " " + _transferLabel);
                        headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
						aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.Transfer);
                        headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks

						// begin MID Track 4282 Velocity overlays Fill Size Holes Allocation
						aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor,cKey,colorName + " " + _preSizeAllocatedLabel);
                        headerCoordListB.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
						aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.PreSizeAllocated);
                        headerCoordListC.Add(aWaferCoordinate); // TT#59 Implement Store Temp Locks
						// end MID Track 4282 Velocity overlays Fill Size Holes Allocation

                        //begin tt#152 Velocity balance - apicchetti
                        aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _velocityInitialRuleTypeLabel);
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialRuleType);
                        headerCoordListC.Add(aWaferCoordinate);

                        // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                        aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _velocityInitialRuleTypeQtyLabel);
                        //coordListB.Add(aWaferCoordinate);   // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
						headerCoordListB.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialRuleTypeQty);
                        //coordListC.Add(aWaferCoordinate);   // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
						headerCoordListC.Add(aWaferCoordinate); // TT#59 Temp Lock - move from ver 3.1 to ver 4.0
                        // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

                        aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _velocityInitialRuleQtyLabel);
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialRuleQty);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _velocityInitialWillShipLabel);
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.VelocityInitialWillShip);
                        headerCoordListC.Add(aWaferCoordinate);
                        //end tt#152 Velocity balance - apicchetti
                    }

                    // begin TT#59 Implement Store Temp Locks
                    if (this._trans.SAB.AllowDebugging)
                    {
                        aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _storeColorMaximumLabel);
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreColorMaximum);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _storeColorMinimumLabel);
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreColorMinimum);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _shipStatusLabel);
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.ShippingStatus);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _qtyShippedLabel);
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QtyShipped);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _allocationModifiedAftMultiSplitLabel);
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AllocationModifiedAftMultiSplit);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _storeHadNeedLabel);
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreHadNeed);
                        headerCoordListC.Add(aWaferCoordinate);

                        // Unit Need Before is not being populated (uncomment lines below if it becomes populated)
                        //aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _unitNeedBeforeLabel);
                        //headerCoordListB.Add(aWaferCoordinate);
                        //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.UnitNeedBefore);
                        //headerCoordListC.Add(aWaferCoordinate);
                        //
                        //aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _percentNeedBeforeLabel);
                        //headerCoordListB.Add(aWaferCoordinate);
                        //aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.PercentNeedBefore);
                        //headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _wasAutoAllocatedLabel);
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.WasAutoAllocated);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _qtyAllocatedByAutoLabel);
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QtyAllocatedByAuto);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _ruleAllocationFromParentLabel);
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleAllocationFromParent);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _allocationFromBottomUpSizeLabel);
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AllocationFromBottomUpSize);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _ruleAllocationFromChosenRuleLabel);
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.RuleAllocationFromChosenRule);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _qtyAllocatedByRuleLabel);
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QtyAllocatedByRule);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _storeFilledSizeHolesLabel);
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreFilledSizeHoles);
                        headerCoordListC.Add(aWaferCoordinate);

                        aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor, cKey, colorName + " " + _storeManuallyAllocatedLabel);
                        headerCoordListB.Add(aWaferCoordinate);
                        aWaferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreManuallyAllocated);
                        headerCoordListC.Add(aWaferCoordinate);
                    }
                    // end TT#59 Implement Store Temp Locks

                }

            }

			coordListA = new AllocationWaferCoordinateList(_trans);
			coordListA.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.HeaderTotal,0,this._headerTotalLabel));

			switch ((eAllocationStyleViewGroupBy)_sel.GroupBy)
			{
				case  eAllocationStyleViewGroupBy.Header:
				{
					foreach (Object objA in coordListA)
					{
                        // begin TT#59 Implement Store Temp Locks
						// begin ver 4.0 code before merging Temp Lock code
						//for (int i=0;i<coordListB.Count;i++)
						//{	// RonM - don't display Rule column in Total grid
						//	// (CSMITH) - BEG MID Track #2410: Allow interactive processing with multiple headers
						//	//							if (coordListC[i].Label != _ruleLabel)
						//	//							{
						//	if (coordListC[i].Label != _ruleLabel &&
						//		coordListC[i].Label != _styleOnHandLabel &&
						//		coordListC[i].Label != _styleInTransitLabel &&
						//		coordListC[i].Label != _velocityRuleLabel &&
                        //      coordListC[i].Label != _velocityRuleTypeQtyLabel &&  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                        //       	coordListC[i].Label != _velocityRuleQtyLabel &&
						//		coordListC[i].Label != _velocityRuleResultLabel &&
                        //        coordListC[i].Label != _velocityInitialRuleQtyLabel && // tt#152 Velocity balance
                        //        coordListC[i].Label != _velocityInitialRuleTypeLabel && // tt#152 Velocity balance
                        //        coordListC[i].Label != _velocityInitialRuleTypeQtyLabel &&  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                        //        coordListC[i].Label != _velocityInitialWillShipLabel && // tt#152 Velocity balance
						//		coordListC[i].Label != _transferLabel // MID Track 4282 Velocity overlays Fill Size Holes Allocation
						//		&& coordListC[i].Label != _preSizeAllocatedLabel) // MID Track 4282 Velocity overlays Fill Size Holes Allocation
						// end ver 4.0 code before merging Temp Lock code
						// begin ver 3.1 commented code (before temp lock changes)
						//for (int i=0;i<coordListB.Count;i++)
						//{	// RonM - don't display Rule column in Total grid
						//	// (CSMITH) - BEG MID Track #2410: Allow interactive processing with multiple headers
						//	//							if (coordListC[i].Label != _ruleLabel)
						//	//							{
						//	if (coordListC[i].Label != _ruleLabel &&
						//		coordListC[i].Label != _styleOnHandLabel &&
						//		coordListC[i].Label != _styleInTransitLabel &&
						//		coordListC[i].Label != _velocityRuleLabel &&
						//		coordListC[i].Label != _velocityRuleQtyLabel &&
						//		coordListC[i].Label != _velocityRuleResultLabel &&
						//		coordListC[i].Label != _transferLabel // MID Track 4282 Velocity overlays Fill Size Holes Allocation
						//		&& coordListC[i].Label != _preSizeAllocatedLabel) // MID Track 4282 Velocity overlays Fill Size Holes Allocation
						//	{
						//		// (CSMITH) - END MID Track #2410
						//		col = new AllocationWaferCoordinateList(_trans);
						//		col.Add (objA);						
						//		col.Add (coordListB[i]);
						//		col.Add (coordListC[i]);
						//		colGroup2.Add(col);
						//	}
						//}
						// end ver 3.1 commented code (before temp lock changes)
                        for (int i = 0; i < totalCoordListB.Count; i++)
						{
                            if (totalCoordListC[i].Label != _qtyStoreIMOMaxLabel)  // TT#1401 - Jellis - Urban Virtual Store Warehouse pt 28C
                            {                                                      // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28C
                                col = new AllocationWaferCoordinateList(_trans);
                                col.Add(objA);
                                col.Add(totalCoordListB[i]);
                                col.Add(totalCoordListC[i]);
                                colGroup2.Add(col);
                            }                                                      // TT#1401 - Jellis - Urban Virtual Store Warehouse pt 28C
						}
                        // end TT#59 Implement Store Temp Locks
					}
					for (int i=0; i<_wafers.RowCount; i++)
					{
						_wafers[i,1].Columns = colGroup2;
					}
					break;
				}
				case eAllocationStyleViewGroupBy.Components:
				{
                    // begin TT#59 Implement Store Temp Locks
					//for (int i=0;i<coordListB.Count;i++)
					//{
					//	foreach (Object objA in coordListA)
					//	{	// RonM - don't display Rule column in Total grid
					//		// (CSMITH) - BEG MID Track #2410: Allow interactive processing with multiple headers
					//		//							if (coordListC[i].Label != _ruleLabel)
					//		//							{
					//		if (coordListC[i].Label != _ruleLabel &&
					//			coordListC[i].Label != _styleOnHandLabel &&
					//			coordListC[i].Label != _styleInTransitLabel &&
					//			coordListC[i].Label != _velocityRuleLabel &&
                    //          coordListC[i].Label != _velocityRuleTypeQtyLabel &&  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
					//			coordListC[i].Label != _velocityRuleQtyLabel &&
					//			coordListC[i].Label != _velocityRuleResultLabel &&
                    //            coordListC[i].Label != _velocityInitialRuleQtyLabel && // tt#152 Velocity balance
                    //           coordListC[i].Label != _velocityInitialRuleTypeLabel && // tt#152 Velocity balance
                    //          coordListC[i].Label != _velocityInitialRuleTypeQtyLabel &&  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                    //          coordListC[i].Label != _velocityInitialWillShipLabel && // tt#152 Velocity balance
					//			coordListC[i].Label != _transferLabel // MID Track 4282 Velocity overlays Fill Size Holes Allocation
					//			&& coordListC[i].Label != _preSizeAllocatedLabel) // MID Track 4282 Velocity overlays Fill Size Holes Allocation
					//		{
					//			// (CSMITH) - END MID Track #2410
					//			col = new AllocationWaferCoordinateList(_trans);
					//			col.Add (coordListB[i]);
					//			col.Add (objA);
					//			col.Add (coordListC[i]);
					//			colGroup2.Add(col);
					//		}
					//	}
					//}
                    for (int i=0; i < totalCoordListB.Count; i++)
                    {
                        foreach (Object objA in coordListA)
                        {
                            if (totalCoordListC[i].Label != _qtyStoreIMOMaxLabel)  // TT#1401 - Jellis - Urban Virtual Store Warehouse pt 28C
                            {                                                      // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28C
                                col = new AllocationWaferCoordinateList(_trans);
                                col.Add(totalCoordListB[i]);
                                col.Add(objA);
                                col.Add(totalCoordListC[i]);
                                colGroup2.Add(col);
                            }                                                      // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28C
                        }
                    }
					for (int i=0; i<_wafers.ColumnCount; i++)
					{
						_wafers[i,1].Columns = colGroup2;
					}
					break;
				}
				default:
                    {
                        //System.Windows.Forms.MessageBox.Show("Selected GroupBy is not implemented for Style View");
                        DialogResult diagResult = _trans.SAB.MessageCallback.HandleMessage(
                                            MIDText.GetText(eMIDTextCode.msg_al_InvalidStyleGroupBy),
                                            "",
                                            MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.None);
                        break;
                    }
			}

			coordListA = new AllocationWaferCoordinateList(_trans);
			foreach (AllocationProfile ap in _apl)
			{
                // Begin TT#2 - RMatelic - Assortment Planning 
                //coordListA.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.Header,ap.Key,ap.HeaderID));
				// Begin TT#1194-MD - stodd - view ga header
                string headerID = ap.HeaderID;
                if (ap.HeaderType == eHeaderType.Assortment)
                {
                    //SetAssortmentType(ap.Key, ap.AsrtType);
                    if (ap.AsrtType != (int)eAssortmentType.GroupAllocation)
                    {
                        continue;
                    }
					// End TT#1194-MD - stodd - view ga header
                    headerID = ap.HeaderID; // TT#1202 - temp temp temp
                }
                else if (ap.HeaderType == eHeaderType.Placeholder)
                {
                    // Begin TT#2 - RMatelic - Assortment Planning - Allocation views: don't display Placeholders on Post Receipt assortments 
                    if (GetAssortmentType(ap.AsrtRID) == eAssortmentType.PostReceipt || GetAssortmentType(ap.AsrtRID) == eAssortmentType.GroupAllocation)  // TT#1194-MD - stodd - view ga header
                    {
                        continue;
                    }
                    else
                    // End TT#2
                    {
                        HierarchyNodeProfile hnp_style = _trans.SAB.HierarchyServerSession.GetNodeData(ap.StyleHnRID, false);
                        if (hnp_style.IsVirtual && hnp_style.Purpose == ePurpose.Placeholder)
                        {
                            headerID = ap.HeaderID;
                        }
                        else
                        {
                            headerID = hnp_style.LevelText;
                        }
                    }
                }
                else
                {
                    headerID = ap.HeaderID;
                }
                coordListA.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.Header, ap.Key, headerID));
                // End TT#2
			}

			switch ((eAllocationStyleViewGroupBy)_sel.GroupBy)
			{
				case  eAllocationStyleViewGroupBy.Header:
				{
					foreach (Object objA in coordListA)
					{
						AllocationWaferCoordinate awc = (AllocationWaferCoordinate) objA;
						AllocationProfile ap = (AllocationProfile)_apl.FindKey(awc.Key);
						for (int i=0;i<headerCoordListB.Count;i++) // TT#59 Implement Store Temp Locks
						{
							if (IncludeCoordinate(ap, headerCoordListB[i]))  // jae , coordListC[i])) // TT#59 IMplement Store Temp Locks
							{
								col = new AllocationWaferCoordinateList(_trans);
								col.Add (objA);	
								col.Add (headerCoordListB[i]); // TT#59 IMplement Store Temp Locks
								col.Add (headerCoordListC[i]); // TT#59 Implement Store Temp Locks
								colGroup3.Add(col);
							}
						}
					}
					for (int i=0; i<_wafers.ColumnCount; i++)
					{
						_wafers[i,2].Columns = colGroup3;
					}
					break;
				}
				case eAllocationStyleViewGroupBy.Components:
				{
					for (int i=0;i<headerCoordListB.Count;i++)
					{
						foreach (Object objA in coordListA)
						{
							AllocationWaferCoordinate awc = (AllocationWaferCoordinate) objA;
							AllocationProfile ap = (AllocationProfile)_apl.FindKey(awc.Key);
							if (IncludeCoordinate(ap, headerCoordListB[i])) // jae, coordListC[i]))  // TT#59 Implement Stoer Temp Locks
							{
								col = new AllocationWaferCoordinateList(_trans);
                                col.Add(headerCoordListB[i]);  // TT#59 Implement Stoer Temp Locks
								col.Add (objA);
                                col.Add(headerCoordListC[i]);  // TT#59 Implement Stoer Temp Locks
								colGroup3.Add(col);
							}
						}
					}
					for (int i=0; i<_wafers.ColumnCount; i++)
					{
						_wafers[i,2].Columns = colGroup3;
					}
					break;
				}
				default:
                    {
                        //System.Windows.Forms.MessageBox.Show("Selected GroupBy is not implemented for Style View");
                        DialogResult diagResult = _trans.SAB.MessageCallback.HandleMessage(
                                            MIDText.GetText(eMIDTextCode.msg_al_InvalidStyleGroupBy),
                                            "",
                                            MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.None);
                        break;
                    }
			}

            ProfileList sgll = StoreMgmt.StoreGroup_GetLevelListFilled(this.StoreGroupRID); //_storeServerSession.GetStoreGroupLevelList(this.StoreGroupRID);
		
			//			ProfileList sgll = _storeServerSession.GetStoreGroupLevelList(_sel.StoreAttributeID);
			
			ProfileList spl = null;
			bool allSets = false;
			spl = new ProfileList(eProfileType.StoreGroupLevel);
			AllocationSubtotalProfile grandTotal = _trans.GetAllocationGrandTotalProfile();
			if (allSets)
			{
				foreach(StoreGroupLevelProfile sglp in sgll)
				{
					foreach(StoreProfile storeProfile in sglp.Stores)
					{
						if (grandTotal.StoreIsVisible(storeProfile.Key))
						{
							spl.Add(storeProfile);
						}
					}
				}
			}
			else
			{
				// BEGIN TT#553-MD - stodd - Size View null reference from attr set
				if (_storeGroupLevel == -1 || !sgll.Contains(_storeGroupLevel))
				// END TT#553-MD - stodd - Size View null reference from attr set
				{
					_storeGroupLevel = sgll[0].Key;
				}
				//				foreach (StoreProfile storeProfile in _trans.GetStoresInGroup(_sel.StoreAttributeID, _storeGroupLevel))	
				foreach (StoreProfile storeProfile in _trans.GetActiveStoresInGroup(this.StoreGroupRID, _storeGroupLevel))  //MID Track 5820 - Unhandled Exception After Store Activation	

				{
					if (grandTotal.StoreIsVisible(storeProfile.Key))
					{
						spl.Add(storeProfile);
					}
				}
			}
					
			coordListA = new AllocationWaferCoordinateList(_trans);
			AllocationWaferCoordinate coord;
			foreach (StoreProfile sp in spl)
			{
				coord = new AllocationWaferCoordinate(eStoreAllocationNode.Store,sp.Key,sp.Text);
				coordListA.Add(coord);
			}
			foreach (Object objA in coordListA)
			{
				row = new AllocationWaferCoordinateList(_trans);
				row.Add(objA);
				rowGroup1.Add(row);
			}
			for (int i=0; i<_wafers.RowCount; i++)
			{
				_wafers[0,i].Rows = rowGroup1;
			}
					
			coordListA = new AllocationWaferCoordinateList(_trans);
			foreach (StoreGroupLevelProfile sgp in sgll)
			{
				if (allSets)
				{
					spl = new ProfileList(eProfileType.StoreGroupLevel);
					foreach(StoreGroupLevelProfile sglp in sgll)
					{
						coord = new AllocationWaferCoordinate(eStoreAllocationNode.Set,sglp.Key,sglp.Name);
						coordListA.Add(coord);
					}
				}
				else
				{
					coord = new AllocationWaferCoordinate(eStoreAllocationNode.Set,sgp.Key,sgp.Name);
					coordListA.Add(coord);
				}
			}
			foreach (Object objA in coordListA)
			{
				row = new AllocationWaferCoordinateList(_trans);
				row.Add(objA);
				rowGroup2.Add(row);
			}
			for (int i=0; i<_wafers.RowCount; i++)
			{
				_wafers[1,i].Rows = rowGroup2;
			}

			coordListA = new AllocationWaferCoordinateList(_trans);
			coord = new AllocationWaferCoordinate(eStoreAllocationNode.All,0,this._allStoreLabel);
			coordListA.Add(coord);

			coord = new AllocationWaferCoordinate(eAllocationCoordinateType.BalanceChainToHeader,0,this._balanceLabel);
			coordListA.Add(coord);

			foreach (Object objA in coordListA)
			{
				row = new AllocationWaferCoordinateList(_trans);
				row.Add(objA);
				rowGroup3.Add(row);
			}
			for (int i=0; i<_wafers.RowCount; i++)
			{
				_wafers[2,i].Rows = rowGroup3;
			}

		}
		#endregion StyleView

		#region SummaryView
		/// <summary>
		/// Builds the Summary Allocation View
		/// </summary>
		private void BuildSummaryView()
		{
			_wafers = new AllocationWaferBuilderGroup(_trans,3,2);

			AllocationWaferCoordinateListGroup colGroup1 = new AllocationWaferCoordinateListGroup();
			AllocationWaferCoordinateListGroup colGroup2 = new AllocationWaferCoordinateListGroup();

			AllocationWaferCoordinateListGroup rowGroup1 = new AllocationWaferCoordinateListGroup();
			AllocationWaferCoordinateListGroup rowGroup2 = new AllocationWaferCoordinateListGroup();
			AllocationWaferCoordinateListGroup rowGroup3 = new AllocationWaferCoordinateListGroup();

			AllocationWaferCoordinateList col;
			AllocationWaferCoordinateList row = new AllocationWaferCoordinateList(_trans);

			row.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.OnHand));
			row.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			rowGroup1.Add(row);

			row = new AllocationWaferCoordinateList(_trans);
			row.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.InTransit));
			row.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));

            // BEGIN TT#1401 - AGallagher - Reservation Stores
            row = new AllocationWaferCoordinateList(_trans);
            row.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOHistoryMaxQuantityAllocated));
            row.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
            // END TT#1401 - AGallagher - Reservation Stores

			rowGroup1.Add(row);

			row = new AllocationWaferCoordinateList(_trans);
			row.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.OpenToShip));
			row.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			rowGroup1.Add(row);

			row = new AllocationWaferCoordinateList(_trans);
			row.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.QuantityAllocated));
			row.Add(new AllocationWaferCoordinate(eComponentType.Total,0,""));
			rowGroup1.Add(row);

            // BEGIN TT#1401 - AGallagher - Reservation Stores
            row = new AllocationWaferCoordinateList(_trans);
            row.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreItemQuantityAllocated));
            row.Add(new AllocationWaferCoordinate(eComponentType.Total, 0, ""));
            rowGroup1.Add(row);

            row = new AllocationWaferCoordinateList(_trans);
            row.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOQuantityAllocated));
            row.Add(new AllocationWaferCoordinate(eComponentType.Total, 0, ""));
            rowGroup1.Add(row);

            row = new AllocationWaferCoordinateList(_trans);
            row.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOMaxQuantityAllocated));
            row.Add(new AllocationWaferCoordinate(eComponentType.Total, 0, ""));
            rowGroup1.Add(row);
            // END TT#1401 - AGallagher - Reservation Stores

			row = new AllocationWaferCoordinateList(_trans);
			row.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.OTSVariance));
			row.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			rowGroup1.Add(row);

			for (int i=0;i<_wafers.ColumnCount;i++)
			{
				_wafers[0,i].Rows = rowGroup1;
			}

					
			AllocationWaferCoordinateList coordListB = new AllocationWaferCoordinateList(_trans);
			AllocationWaferCoordinateList coordListC;

			AllocationWaferCoordinate aWaferCoordinate;
			aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Total,0,this._totalComponentLabel);
			coordListB.Add(aWaferCoordinate);

			if (_genericPackName.Count > 0)
			{
				for (int iPack=0; iPack < _subtotalGenericPackName.Count; iPack++)
				{
					coordListB.Add(new AllocationWaferCoordinate(
						(string)_genericPackName[iPack], 
						(string)_subtotalGenericPackName[iPack], 
						(string)_subtotalGenericPackName[iPack])); 
				}
			}
			// begin MID Track #2483 - detail packs not appearing in Summary View
			if (_nonGenericPackName.Count > 0)
			{
				for (int iPack=0; iPack < _subtotalNonGenericPackName.Count; iPack++)
				{
					coordListB.Add(new AllocationWaferCoordinate(
						(string)_nonGenericPackName[iPack], 
						(string)_subtotalNonGenericPackName[iPack], 
						(string)_subtotalNonGenericPackName[iPack])); 
				}
			}
			// end MID Track #2483  
			
			if (_colorKeyList.Count > 0)
			{
				aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.Bulk,0,this._bulkComponentLabel);
				coordListB.Add(aWaferCoordinate);
				// BEGIN MID Track #3231 - Color description change is not reflected in Review
				//foreach (int cKey in _colorKeyList.Values)
                // Begin TT#213 - RMatelic - MID Track #6300-Multi header order in the workspace does not match order in style review
                SortedList sortedBulkColors = new SortedList();
                foreach (ColorData cd in _colorKeyList.Values)
                {
                    // Begin TT#326 - RMatelic - highlight 2 header; select style view;get system argument exception error message
                    //sortedBulkColors.Add(cd.ColorSequence, cd);
                    while (sortedBulkColors.ContainsKey(cd.ColorSequence))
                    {
                        cd.ColorSequence++;
                    }
                    sortedBulkColors.Add(cd.ColorSequence, cd);
                    // End TT#326
                }
                //foreach (int cKey in _colorKeyList.Keys) 
                foreach (ColorData cd in sortedBulkColors.Values)
                // End TT#213
				{
					string colorName = string.Empty;
                    // Begin TT#213 - RMatelic - MID Track #6300-Multi header order in the workspace does not match order in style review
                    //ColorData cd = (ColorData)_colorKeyList[cKey]; 
                    int cKey = cd.ColorCodeRID;
                    // End TT#213
					if (cd.IsDuplicateName)
						colorName = cd.ColorID + " " + cd.ColorName;
					else
						colorName = cd.ColorName;
					//aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor,cKey,_trans.GetColorCodeProfile(cKey).ColorCodeName);
					aWaferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor,cKey,colorName);
					// END MID Track #3231  
					coordListB.Add(aWaferCoordinate);
				}

			}


			AllocationWaferCoordinateList coordListA = new AllocationWaferCoordinateList(_trans);
			foreach (AllocationProfile ap in _apl)
			{
                // Begin TT#2 - RMatelic - Assortment Planning 
                //coordListA.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.Header,ap.Key,ap.HeaderID));
                string headerID;
                if (ap.HeaderType == eHeaderType.Assortment)
                {
                    continue;
                }
                else if (ap.HeaderType == eHeaderType.Placeholder)
                {
                    // Begin TT#2 - RMatelic - Assortment Planning - Allocation views: don't display Placeholders on Post Receipt assortments 
                    if (GetAssortmentType(ap.AsrtRID) == eAssortmentType.PostReceipt)
                    {
                        continue;
                    }
                    else
                    // End TT#2
                    {
                        HierarchyNodeProfile hnp_style = _trans.SAB.HierarchyServerSession.GetNodeData(ap.StyleHnRID, false);
                        if (hnp_style.IsVirtual && hnp_style.Purpose == ePurpose.Placeholder)
                        {
                            headerID = ap.HeaderID;
                        }
                        else
                        {
                            headerID = hnp_style.LevelText;
                        }
                    }
                }
                else
                {
                    headerID = ap.HeaderID;
                }
                coordListA.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.Header, ap.Key, headerID));
                // End TT#2
			}

			foreach (AllocationWaferCoordinate awc in coordListA)
			{
				AllocationProfile ap = (AllocationProfile)_apl.FindKey(awc.Key);
				for (int i=0;i<coordListB.Count;i++)
				{
					if (IncludeCoordinate(ap, coordListB[i]))
					{
						row = new AllocationWaferCoordinateList(_trans);
						row.Add (awc);	
						row.Add (coordListB[i]);
						rowGroup2.Add(row);
					}
				}
			}
			for (int i=0; i<_wafers.ColumnCount; i++)
			{
				_wafers[1,i].Rows = rowGroup2;
			}

			coordListA = new AllocationWaferCoordinateList(_trans);
			coordListA.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.HeaderTotal,0,this._headerTotalLabel));

			foreach (Object objA in coordListA)
			{
				for (int i=0;i<coordListB.Count;i++)
				{	
					row = new AllocationWaferCoordinateList(_trans);
					row.Add (objA);						
					row.Add (coordListB[i]);
					rowGroup3.Add(row);
				}
			}
			for (int i=0; i<_wafers.ColumnCount; i++)
			{
				_wafers[2,i].Rows = rowGroup3;
			}
			coordListA = new AllocationWaferCoordinateList(_trans);
			AllocationWaferCoordinate coord = new AllocationWaferCoordinate(eStoreAllocationNode.All,0,this._allStoreLabel);
			coordListA.Add(coord);
					
			coordListB = new AllocationWaferCoordinateList(_trans);
			coord = new AllocationWaferCoordinate(eAllocationWaferVariable.QtyReceived);
			coordListB.Add(coord);

			coord = new AllocationWaferCoordinate(eAllocationWaferVariable.Balance);
			coordListB.Add(coord);

			coord = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreCount);
			coordListB.Add(coord);

			coord = new AllocationWaferCoordinate(eAllocationWaferVariable.Total);
			coordListB.Add(coord);

			foreach (Object objA in coordListA)
			{
				foreach (Object objB in coordListB)
				{
					col = new AllocationWaferCoordinateList(_trans);
					col.Add(objA);
					col.Add(objB);
					colGroup1.Add(col);
				}
			}
			for (int i=0; i<_wafers.RowCount; i++)
			{
				_wafers[i,0].Columns = colGroup1;
			}


			//			ProfileList sgll = _storeServerSession.GetStoreGroupLevelList(_sel.StoreAttributeID);
            ProfileList sgll = StoreMgmt.StoreGroup_GetLevelListFilled(this.StoreGroupRID); //_storeServerSession.GetStoreGroupLevelList(this.StoreGroupRID);
			// BEGIN TT#553-MD - stodd - Size View null reference from attr set
			if (_storeGroupLevel == -1 || !sgll.Contains(_storeGroupLevel))
			// END TT#553-MD - stodd - Size View null reference from attr set
			{
				_storeGroupLevel = sgll[0].Key;
			}
			coordListA = new AllocationWaferCoordinateList(_trans);
			coordListB = new AllocationWaferCoordinateList(_trans);

			coordListC = new AllocationWaferCoordinateList(_trans);
			coord = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreCount);
			coordListC.Add(coord);
			coord = new AllocationWaferCoordinate(eAllocationWaferVariable.AverageStore);
			coordListC.Add(coord);
			coord = new AllocationWaferCoordinate(eAllocationWaferVariable.Total);
			coordListC.Add(coord);
			switch (_sel.GroupBy)
			{
				case((int)eAllocationSummaryViewGroupBy.Attribute):
				{
					foreach (StoreGroupLevelProfile sgp in sgll)
					{
						coord = new AllocationWaferCoordinate(eStoreAllocationNode.Set,sgp.Key,sgp.Name);
						coordListA.Add(coord);
					}
					foreach (object objA in coordListA)
					{
                        foreach (AllocationWaferCoordinate objC in coordListC) // TT#1401 - Jellis - Urban Virtual Store Warehouse pt 28C
						{
                            if (objC.Label != _qtyStoreIMOMaxLabel)  // TT#1401 - Jellis - Urban Virtual Store Warehouse pt 28C
                            {                                                      // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28C
                                col = new AllocationWaferCoordinateList(_trans);
                                col.Add(objA);
                                col.Add(objC);
                                colGroup2.Add(col);
                            }                                                      // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28C
						}
					}
					for (int i=0; i<_wafers.RowCount; i++)
					{
						_wafers[i,1].Columns = colGroup2;
					}
					break;
				}
				case((int)eAllocationSummaryViewGroupBy.StoreGrade):
				{
					StoreGroupLevelProfile sgp = (StoreGroupLevelProfile)sgll.FindKey(this._storeGroupLevel);
					coord = new AllocationWaferCoordinate(eStoreAllocationNode.Set,sgp.Key,sgp.Name);
					coordListA.Add(coord);
					coordListB.Add(coord);
							
					AllocationSubtotalProfile asp = _trans.GetAllocationGrandTotalProfile();
					ArrayList volumeGrade = asp.GradeList;
					foreach (AllocationGradeBin agb in volumeGrade)
					{
						coord = new AllocationWaferCoordinate(agb.Grade, agb.Grade);
						coordListB.Add(coord);
					}
					coord = new AllocationWaferCoordinate(" ", this._unassignedLabel);
					coordListB.Add(coord);
					foreach (object objA in coordListA)
					{
						foreach (object objB in coordListB)
						{
                            foreach (AllocationWaferCoordinate objC in coordListC) // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28C
							{
                                if (objC.Label != _qtyStoreIMOMaxLabel)  // TT#1401 - Jellis - Urban Virtual Store Warehouse pt 28C
                                {                                        // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28C
                                    col = new AllocationWaferCoordinateList(_trans);
                                    col.Add(objA);
                                    col.Add(objB);
                                    col.Add(objC);
                                    colGroup2.Add(col);
                                }                                        // TT#1401 - Jellis - Urban Virtual Store Warehouse pt 28C
							}
						}
					}
					for (int i=0; i<_wafers.RowCount; i++)
					{
						_wafers[i,1].Columns = colGroup2;
					}

					break;
				}
				default:
                    {
                        //System.Windows.Forms.MessageBox.Show("Selected GroupBy is not implemented for Summary View");
                        DialogResult diagResult = _trans.SAB.MessageCallback.HandleMessage(
                                            MIDText.GetText(eMIDTextCode.msg_al_InvalidSummaryGroupBy),
                                            "",
                                            MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.None);
                        break;
                    }
			}
		}
		#endregion SummaryView

		#region SizeView
		AllocationWaferCoordinateList _sizeColVariableCoordList;
		AllocationWaferCoordinateListGroup _seqSizeColGroup;
		AllocationWaferCoordinateListGroup _mtrxSizeColGroup;
		AllocationWaferCoordinateListGroup _sizeColGroup1;
		AllocationWaferCoordinateListGroup _sizeColGroup2;
		int[] _primarySizeKeys;
		int[] _secondarySizeKeys;
		Hashtable _sizesWithPosCurvePctHash;
//		Hashtable _sizeViewGroupByHash;

		// Row building blocks
		AllocationWaferCoordinateList _storeCoordList;	
		AllocationWaferCoordinateList _headerCoordList;
		AllocationWaferCoordinateList _colorCoordList;
		AllocationWaferCoordinateList _secondSizeCoordList;
		AllocationWaferCoordinateList _sizeRowVariableCoordList;

        // Begin TT#607-MD - RMatelic - Size Review not displaying new sizes added when header is dropped on an assortment placeholder
        public void ResetSizeViewGroups()
        {
            _seqSizeColGroup = null;
            _mtrxSizeColGroup = null;
        }
        // End TT#607-MD - 
		
        /// <summary>
		/// Builds the Size Allocation View
		/// </summary>
		private void BuildSizeView()
		{
			AllocationProfileList apl = this._trans.GetAllocationProfileList();
			// BEGIN MID Change j.ellis Performance Enhancement
			if (this._firstBuildSize)
			{
                BuildSizeViewInfrastructure(apl);
            }
			// END MID Change j.ellis Performance Enhancement
			_wafers = new AllocationWaferBuilderGroup(_trans,3,3);

			AllocationWaferCoordinateListGroup sizeColGroup = new AllocationWaferCoordinateListGroup();

			// Begin TT#1487-MD - stodd - ASST - Fill Size Holes does not work on a PH.
            foreach (AllocationProfile ap in apl.ArrayList)
            {
                if (ap.Placeholder)
                {
                    foreach (HdrColorBin hcb in ap.BulkColors.Values)
                    {
                        if (hcb.ColorSizes.Count > 0)
                        {
                            _atLeast1WorkUpSizeBuy = true;
                            break;
                        }

                    }
                }

                if (_atLeast1WorkUpSizeBuy)
                {
                    break;
                }
            }
			// End TT#1487-MD - stodd - ASST - Fill Size Holes does not work on a PH.

			if (this.ViewIsSequential )
			{
				if (this._seqSizeColGroup == null)
				{
					this.BuildSeqSizeColumns(apl);
				}
				sizeColGroup = _seqSizeColGroup;
			}
			else
			{
				if (this._mtrxSizeColGroup == null)
				{
					this.BuildMatrixSizeColumns(apl);
				}
				sizeColGroup = _mtrxSizeColGroup;
			}
			AllocationWaferCoordinateListGroup sizeColGroup3;
			// BEGIN MID Track #3152 - secondary sizes not showing in Sequential view; remove 'if...' 
//			if (this._sizeViewGroupByHash.Contains((int)_sel.SecondaryGroupBy))
//			{
//				sizeColGroup3 = (AllocationWaferCoordinateListGroup)this._sizeViewGroupByHash[(int)_sel.SecondaryGroupBy];
//			}
//			else
//			{
				sizeColGroup3 = new AllocationWaferCoordinateListGroup();
//				this._sizeViewGroupByHash.Add((int)_sel.SecondaryGroupBy, sizeColGroup3);
			// END MID Track #3152
				switch(_sel.SecondaryGroupBy) 
				{
					case ((int)eAllocationSizeView2ndGroupBy.Variable):
					{
						foreach (object variableObj in this._sizeColVariableCoordList)
						{
							AllocationWaferCoordinateList col;
							foreach (AllocationWaferCoordinateList sizeColumnCoordList in sizeColGroup)
							{
								// Beware: size column coordlist describes a single column!
								col = new AllocationWaferCoordinateList(_trans);
								col.Add(variableObj);
								foreach (object sizeColObj in sizeColumnCoordList)
								{
									col.Add(sizeColObj);
								}
								sizeColGroup3.Add(col);
							}
						}
						break;
					}
					case ((int)eAllocationSizeView2ndGroupBy.Size):
					{
						AllocationWaferCoordinateList col;
						foreach (AllocationWaferCoordinateList sizeColumnCoordList in sizeColGroup)
						{
							foreach (object variableObj in this._sizeColVariableCoordList)
							{
								col = new AllocationWaferCoordinateList(_trans);
								foreach (object sizeObj in sizeColumnCoordList)
								{
									col.Add(sizeObj);
								}
								col.Add(variableObj);
								sizeColGroup3.Add(col);
							}
						}
						break;
					}
					default:
					{
						//System.Windows.Forms.MessageBox.Show("Selected 2nd GroupBy is not implemented for Size View");
                        DialogResult diagResult = _trans.SAB.MessageCallback.HandleMessage(
                                            MIDText.GetText(eMIDTextCode.msg_al_InvalidSizeGroupBy),
                                            "",
                                            MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.None);
                        break;
					}
				}
//			}

			for (int i=0;i<_wafers.RowCount;i++)
			{
				_wafers[i,0].Columns = this._sizeColGroup1;
				_wafers[i,1].Columns = this._sizeColGroup2;
				_wafers[i,2].Columns = sizeColGroup3;
			}


			AllocationWaferCoordinateListGroup rowGroup1 = new AllocationWaferCoordinateListGroup();
			AllocationWaferCoordinateListGroup rowGroup2 = new AllocationWaferCoordinateListGroup();
			AllocationWaferCoordinateListGroup rowGroup3 = new AllocationWaferCoordinateListGroup();
    		AllocationWaferCoordinateList secondSizeCoordList = null;
			 
			// Build Size Dimension Coordinates
			if (!this.ViewIsSequential)			{
				secondSizeCoordList = this._secondSizeCoordList;
			}

			// Build selected store rows
            ProfileList sgll = StoreMgmt.StoreGroup_GetLevelListFilled(this.StoreGroupRID); //_storeServerSession.GetStoreGroupLevelList(this.StoreGroupRID);
			ProfileList spl ;
			spl = new ProfileList(eProfileType.StoreGroupLevel);
			AllocationSubtotalProfile grandTotal = _trans.GetAllocationGrandTotalProfile();
			// BEGIN TT#553-MD - stodd - Size View null reference from attr set
			if (_storeGroupLevel == -1 || !sgll.Contains(_storeGroupLevel))
			// END TT#553-MD - stodd - Size View null reference from attr set
			{
				// Begin TT#4964 - JSmith - Size Allocation Review Error - Color not defined for bulk
				// Make sure set has at least 1 active store
				//_storeGroupLevel = sgll[0].Key;
                foreach (StoreGroupLevelProfile sglp in sgll)
                {
                    if (sglp.Stores.Count > 0)
                    {
                        _storeGroupLevel = sglp.Key;
                        break;
                    }
                }
				// End TT#4964 - JSmith - Size Allocation Review Error - Color not defined for bulk
			}
			foreach (StoreProfile storeProfile in _trans.GetActiveStoresInGroup(this.StoreGroupRID, _storeGroupLevel)) //MID Track 5820 - Unhandled Exception After Store Activation	
			{
				if (grandTotal.StoreIsVisible(storeProfile.Key))
				{
					spl.Add(storeProfile);
				}
			}

			_storeCoordList = new AllocationWaferCoordinateList(_trans);
			AllocationWaferCoordinate coord;
			foreach (StoreProfile sp in spl)
			{
				coord = new AllocationWaferCoordinate(eStoreAllocationNode.Store,sp.Key,sp.Text);
				_storeCoordList.Add(coord);
			}
			rowGroup1 = BuildSizeRows(_storeCoordList, _headerCoordList,  _colorCoordList, secondSizeCoordList,  this._sizeRowVariableCoordList);
			
			// Build attribute set rows
			AllocationWaferCoordinateList setCoordList = new AllocationWaferCoordinateList(_trans);
			foreach (StoreGroupLevelProfile sgp in sgll)
			{
				coord = new AllocationWaferCoordinate(eStoreAllocationNode.Set,sgp.Key,sgp.Name);
				setCoordList.Add(coord);
			}
			rowGroup2 = BuildSizeRows(setCoordList, _headerCoordList, _colorCoordList, secondSizeCoordList, this._sizeRowVariableCoordList);
			// Build Balance rows
			setCoordList = new AllocationWaferCoordinateList(_trans);
			coord = new AllocationWaferCoordinate(eStoreAllocationNode.All,0,this._allStoreLabel);
			setCoordList.Add(coord);
			coord = new AllocationWaferCoordinate(eAllocationCoordinateType.BalanceChainToHeader,0,this._balanceLabel);
			setCoordList.Add(coord);
			rowGroup3 = BuildSizeRows(setCoordList, _headerCoordList, _colorCoordList, secondSizeCoordList, this._sizeRowVariableCoordList);

			for (int i=0; i<_wafers.ColumnCount; i++)
			{
				_wafers[0,i].Rows = rowGroup1;
				_wafers[1,i].Rows = rowGroup2;
				_wafers[2,i].Rows = rowGroup3;
			}
		}

        private void BuildSizeViewInfrastructure(AllocationProfileList aAllocationProfileList)
       	{
			bool addCurveVariablesToSizeVariables = true;
			this._firstBuildSize = false;
			_storeCoordList = new AllocationWaferCoordinateList(_trans);
			_headerCoordList = new AllocationWaferCoordinateList(_trans);
			_colorCoordList = new AllocationWaferCoordinateList(_trans);
			_secondSizeCoordList = new AllocationWaferCoordinateList(_trans);
			AllocationWaferCoordinate waferCoordinate;
			_allDimAllocatedLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_SecondarySizeTotal); // MID Track 3611 Quick Filter not working in Size Review
			string totalLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_Total_Percent);
			// Build header Total coordinates list
			if (aAllocationProfileList.Count > 1)
			{
				waferCoordinate = new AllocationWaferCoordinate(eAllocationCoordinateType.HeaderTotal,0,"Total");
				_headerCoordList.Add(waferCoordinate);
			}

			// Size Column Variables
			_sizeColVariableCoordList = new AllocationWaferCoordinateList(_trans);
			if (!this.AnalysisOnly)
			{
                _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.QuantityAllocated));
                _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreItemQuantityAllocated));   // TT#1401 - AGallagher - Reservation Stores
                _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOQuantityAllocated));   // TT#1401 - AGallagher - Reservation Stores
                //_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOMaxQuantityAllocated));   // TT#1401 - AGallagher - Reservation Stores
			}
			_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.SizeOnHand));
			_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.SizeInTransit));
            _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.SizeVSWOnHand));   // TT#1401 - AGallagher - VSW
			_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.SizeOnHandPlusIT));
			_sizeColGroup1 = new AllocationWaferCoordinateListGroup();
			_sizeColGroup2 = new AllocationWaferCoordinateListGroup();

			AllocationWaferCoordinateList col;

			// Build Size Column Group1
			col = new AllocationWaferCoordinateList(_trans);
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreGrade));
			_sizeColGroup1.Add(col);

            // Build Size Column Group2
			// if (!this.AnalysisOnly)
            // {	 // MID Track 4921 AnF#666 FIll to Size Plan Enhancement		
			// begin MID Track 3880 Add Ship Day to Style and Size Review
			col = new AllocationWaferCoordinateList(_trans);
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.ShipToDay));
			_sizeColGroup2.Add(col);
			// end MID Track 3880 Add Ship Day to Style and Size Review

            // begin MID Track 4291 Add Fill Variables to Size Review
			col = new AllocationWaferCoordinateList(_trans);
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
			col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.NeedDay));
			_sizeColGroup2.Add(col);
			// end MID Track 4291 Add Fill Variables to Size Review

            // Begin TT#3197 - JSmith - Size Review Analysis Mode in the Total Section Qty Allocated and Size Total appear. 
            if (!this.AnalysisOnly)
            {
            // End TT#3197 - JSmith - Size Review Analysis Mode in the Total Section Qty Allocated and Size Total appear. 
                col = new AllocationWaferCoordinateList(_trans);
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, "Color"));
                col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.QuantityAllocated));
                _sizeColGroup2.Add(col);

                // BEGIN TT#1401 - AGallagher - VSW
                col = new AllocationWaferCoordinateList(_trans);
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, "Color"));
                col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOMaxQuantityAllocated));
                _sizeColGroup2.Add(col);
                // END TT#1401 - AGallagher - VSW

                col = new AllocationWaferCoordinateList(_trans);
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
                col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, "Color"));
                // begin MID Track 3611 Quick Filter not working in Size Review
                //col.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.SizeTotalAllocated));
                waferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.SizeTotalAllocated);
                col.Add(waferCoordinate);
                _sizeTotalAllocatedLabel = waferCoordinate.Label;
                // end MID Track 3611 Quick Filter not working in Size Review
                _sizeColGroup2.Add(col);
            // Begin TT#3197 - JSmith - Size Review Analysis Mode in the Total Section Qty Allocated and Size Total appear. 
            }
            // End TT#3197 - JSmith - Size Review Analysis Mode in the Total Section Qty Allocated and Size Total appear. 

			// begin MID track 4921 AnF#666 Fill to Size Plan Enhancement
			if (addCurveVariablesToSizeVariables)
			{
				col = new AllocationWaferCoordinateList(_trans);
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,"Color"));
				waferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.FillSizeFwdForecastSales);
				col.Add(waferCoordinate);
				_sizeColGroup2.Add(col);
				col = new AllocationWaferCoordinateList(_trans);
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
				col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,"Color"));
				waferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.FillSizeFwdForecastStock);
				col.Add(waferCoordinate);
				_sizeColGroup2.Add(col);
			}
            // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28D
            // begin TT#246 - MD - JEllis - AnF VSW In Store Minimums pt 5 (unrelated issue: Allow Debug On gets "duplicate Column" error in Size Review)
            //// begin TT#230 Size Review Column Chooser missing columns
            //if (this._trans.SAB.AllowDebugging)
            //{
            //    col = new AllocationWaferCoordinateList(_trans);
            //    col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
            //    col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, "Color"));
            //    waferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreMaximum);
            //    col.Add(waferCoordinate);
            //    _sizeColGroup2.Add(col);
            //    col = new AllocationWaferCoordinateList(_trans);
            //    col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
            //    col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, "Color"));
            //    waferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreMinimum);
            //    col.Add(waferCoordinate);
            //    _sizeColGroup2.Add(col);
            //    col = new AllocationWaferCoordinateList(_trans);
            //    col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
            //    col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, "Color"));
            //    waferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.ShippingStatus);
            //    col.Add(waferCoordinate);
            //    _sizeColGroup2.Add(col);
            //    col = new AllocationWaferCoordinateList(_trans);
            //    col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
            //    col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, "Color"));
            //    waferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QtyShipped);
            //    col.Add(waferCoordinate);
            //    _sizeColGroup2.Add(col);
            //    col = new AllocationWaferCoordinateList(_trans);
            //    col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
            //    col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, "Color"));
            //    waferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.AllocationModifiedAftMultiSplit);
            //    col.Add(waferCoordinate);
            //    _sizeColGroup2.Add(col);
            //    col = new AllocationWaferCoordinateList(_trans);
            //    col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
            //    col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, "Color"));
            //    waferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.WasAutoAllocated);
            //    col.Add(waferCoordinate);
            //    _sizeColGroup2.Add(col);
            //    col = new AllocationWaferCoordinateList(_trans);
            //    col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
            //    col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, "Color"));
            //    waferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.QtyAllocatedByAuto);
            //    col.Add(waferCoordinate);
            //    _sizeColGroup2.Add(col);
            //    col = new AllocationWaferCoordinateList(_trans);
            //    col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
            //    col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, "Color"));
            //    waferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreFilledSizeHoles);
            //    col.Add(waferCoordinate);
            //    _sizeColGroup2.Add(col);
            //    col = new AllocationWaferCoordinateList(_trans);
            //    col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, ""));
            //    col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None, 0, "Color"));
            //    waferCoordinate = new AllocationWaferCoordinate(eAllocationWaferVariable.StoreManuallyAllocated);
            //    col.Add(waferCoordinate);
            //    _sizeColGroup2.Add(col); 
            //}
            //// end TT#230 Size Review Column Chooser missing columns
            // end TT#246 - MD - JEllis - AnF VSW In Store Minimums pt 5 (unrelated issue: Allow Debug On gets "duplicate Column" error in Size Review)

            // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28D
			// end MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            // } // MID Track 4921 AnF#666 Fill to Size Plan Enhancement 
			//int[] bcsKeys;  // MID Track 3611 Quick Filter not working in Size Review 			
			ArrayList primarySizeKeysAL = new ArrayList() ;
			ArrayList secondarySizeKeysAL = new ArrayList() ;
			_hdrSecSizeKeys = new Hashtable() ;
			
			SortedList primCodeSeqList = new SortedList();
			SortedList secCodeSeqList  = new SortedList();
			SortedList dupSeqList  = new SortedList();		// MID Track #4989 - Size out of sequence
			// BEGIN MID Change j.ellis show size curves
			SizeNeedMethod snm;  
			_sizesWithPosCurvePctHash = new Hashtable();
			Hashtable sizeNeedMethodHash = new Hashtable(); 
			// END MID Change j.ellis show size curves
			ProfileList scl;
			foreach (AllocationProfile ap in aAllocationProfileList)
			{
				// Build row header coordinate list
                // Begin TT#2 - RMatelic - Assortment Planning 
                //waferCoordinate = new AllocationWaferCoordinate(eAllocationCoordinateType.Header,ap.Key,ap.HeaderID);
                string headerID;
                if (ap.HeaderType == eHeaderType.Assortment // TT#1222-MD - stodd - When opening Size Review when Group Allocation is in "Group" mode and the matrix tab, size review gets a "No Displayable Sizes" error.
                    && GetAssortmentType(ap.AsrtRID) != eAssortmentType.GroupAllocation) // TT#1222-MD - stodd - When opening Size Review when Group Allocation is in "Group" mode and the matrix tab, size review gets a "No Displayable Sizes" error.
                {
                    continue;
                }
                if (ap.HeaderType == eHeaderType.Placeholder)
                {
                    // Begin TT#2 - RMatelic - Assortment Planning - Allocation views: don't display Placeholders on Post Receipt assortments 
                    if (GetAssortmentType(ap.AsrtRID) == eAssortmentType.PostReceipt)
                    {
                        continue;
                    }
                    else
                    // End TT#2
                    {
                        HierarchyNodeProfile hnp_style = _trans.SAB.HierarchyServerSession.GetNodeData(ap.StyleHnRID, false);
                        if (hnp_style.IsVirtual && hnp_style.Purpose == ePurpose.Placeholder)
                        {
                            headerID = ap.HeaderID;
                        }
                        else
                        {
                            headerID = hnp_style.LevelText;
                        }
                    }
                }
                else
                {
                    headerID = ap.HeaderID;
                }
                waferCoordinate = new AllocationWaferCoordinate(eAllocationCoordinateType.Header, ap.Key, headerID);
                // End TT#2
				_headerCoordList.Add(waferCoordinate);

				// column primary and secondary sizes
				primCodeSeqList.Clear();
				secCodeSeqList.Clear();
				Hashtable hdrSecondaryRIDs = new Hashtable();

                // begin TT#2040 - Jellis - GD - Sizes not in correct order on Size Review
                //if (ap.SizeGroupRID != Include.UndefinedSizeGroupRID)
                //{
                //    SizeGroupProfile sgp = new SizeGroupProfile(ap.SizeGroupRID);
                //    scl = (ProfileList)sgp.SizeCodeList;
                //}
                // end TT#2040 - Jellis - GD - Sizes not in correct order on Size Review

                // begin TT#368 Work up buy enhancement: work up buy does not require size group or curve
                //else if (ap.WorkUpBulkSizeBuy  // MID Track 4261 Size Need Analysis not working
                //         || ap.HeaderRID == Include.DefaultHeaderRID) // MID Track 4261 Size Need Analysis not working
                if (ap.HeaderRID == Include.DefaultHeaderRID
                    || SizeCurveRID > 0)
                    // end TT#368 Work up buy enhancement: work up buy does not require size group or curve
				{
					SizeCurveGroupProfile scg = new SizeCurveGroupProfile(this.SizeCurveRID);
					scl = scg.GetSizeCodeList();
				}
                // begin TT#2040 - Jellis - GD - Sizes not in correct order on Size Review
                else if (ap.SizeGroupRID != Include.UndefinedSizeGroupRID)
                {
                    SizeGroupProfile sgp = new SizeGroupProfile(ap.SizeGroupRID);
                    scl = (ProfileList)sgp.SizeCodeList;
                }
                else
                // end TT#2040 - Jellis - GD - Sizes not in correct order on Size Review
                {
					scl = new ProfileList(eProfileType.SizeCode);
					SizeCodeProfile sizeCodeProfile; // MID Track 4075 Sizes in wrong order on Size Review
					foreach (HdrColorBin hcb in ap.BulkColors.Values)
                    {   // Begin TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
                        SortedList sizeSL = new SortedList();
                        foreach (HdrSizeBin hsb in hcb.ColorSizes.Values)
                        {
                            sizeSL.Add(hsb.SizeSequence, hsb);
                        }
                        //foreach (HdrSizeBin hsb in hcb.ColorSizes.Values)
                        foreach (HdrSizeBin hsb in sizeSL.Values)
                        // End TT#234  
						{
                            if (!scl.Contains(hsb.SizeCodeRID))  // Assortment: color/size changes
							{
								// begin MID Track 4075 Size in wrong sequence on Size Review
                                sizeCodeProfile = (SizeCodeProfile)(this._trans.GetSizeCodeProfile(hsb.SizeCodeRID)).Clone(); // Assortment: color/size changes
								if (sizeCodeProfile.PrimarySequence == int.MaxValue)
								{
									sizeCodeProfile.PrimarySequence = hsb.SizeSequence;
								}
								if (sizeCodeProfile.SecondarySequence == int.MaxValue)
								{
									sizeCodeProfile.SecondarySequence = hsb.SizeSequence;
								}
							    scl.Add(sizeCodeProfile);
								//scl.Add((this._trans.GetSizeCodeProfile(hsb.SizeKey)));
								// end MID Track 4075 Size in wrong sequence on Size Review
							}
						}
					}
				}
				// These Size code profiles are in sequence but do not have ...PrimaryRID or ...SecondaryRID
				// so we also need to get the profiles that have the RIDs
				if (scl.Count > 0)
				{
					int primarySequence = 0;  // MID Track 4074 Missing Sizes in Size Review
					int secondarySequence = 0; // MID Track 4074 Missing Sizes in Size Review
					//long sequence = 0; // MID Track 4074 Missing Sizes in Size Review
					Hashtable primaryCodes = new Hashtable(); // MID Track 4074 Missing Sizes in Size Review
					Hashtable secondaryCodes = new Hashtable(); // MID Track 4074 Missing Sizes in Size Review
					 
					foreach (SizeCodeProfile scp in scl)
					{
						// begin MID Track 4074 Missing Sizes in Size Review
						//SizeCodeProfile scp2 = _trans.GetSizeCodeProfile(scp.Key); 
						// RonM - with the above line commented out by Track 4074, Size Review-Analysis Only,
						//        because it is designated as a WorkUpBulkSizeBuy and headers with  
						//        SizeGroups were getting an error because _trans didn't contain the
						//		  _secondarySizeCodeByRID Hashtable. So, putting the line back in for now
						SizeCodeProfile scp2 = _trans.GetSizeCodeProfile(scp.Key); 
					
						if (!primaryCodes.Contains(scp.SizeCodePrimaryRID))
						{
							primarySequence++;
							// BEGIN MID Track #4989 - Sizes out of sequence
							//sequence = (long)((long)scp.PrimarySequence << 32) + (long)primarySequence;
							primaryCodes.Add(scp.SizeCodePrimaryRID, scp.SizeCodePrimaryRID);
							//primCodeSeqList.Add(sequence, scp.SizeCodePrimaryRID);
							if (!primCodeSeqList.ContainsKey(scp.PrimarySequence))
							{
								primCodeSeqList.Add(scp.PrimarySequence, scp.SizeCodePrimaryRID);
							}
							else if (!dupSeqList.ContainsKey(scp.PrimarySequence))
							{
								dupSeqList.Add(scp.PrimarySequence, scp.SizeCodePrimaryRID);
							}
							// END MID Track #4989
						}
						if (!secondaryCodes.Contains(scp.SizeCodeSecondaryRID))
						{
							secondarySequence++;
							// BEGIN MID Track #4989 - Sizes out of sequence
							//sequence = (long)((long)scp.SecondarySequence << 32) + (long)secondarySequence;
							secondaryCodes.Add(scp.SizeCodeSecondaryRID, scp.SizeCodeSecondaryRID);
							//secCodeSeqList.Add(sequence, scp.SizeCodeSecondaryRID);
							// BEGIN MID Track #5306 - Size Review open error
							//secCodeSeqList.Add(scp.SecondarySequence, scp.SizeCodeSecondaryRID);
							if (!secCodeSeqList.ContainsKey(scp.SecondarySequence))
							{
								secCodeSeqList.Add(scp.SecondarySequence, scp.SizeCodeSecondaryRID);
							}
							// END MID Track #5306 
						}	// END MID Track #4989  
						if (!primCodeSeqList.ContainsKey(scp.PrimarySequence))
							primCodeSeqList.Add(scp.PrimarySequence,scp2.SizeCodePrimaryRID);
						
						if (!secCodeSeqList.ContainsKey( scp.SecondarySequence))
							secCodeSeqList.Add( scp.SecondarySequence, scp2.SizeCodeSecondaryRID);
						if (!hdrSecondaryRIDs.ContainsKey( scp.SizeCodeSecondaryRID))
							hdrSecondaryRIDs.Add(scp.SizeCodeSecondaryRID,ap.Key);
						// end MID Track 4074 Missing Sizes in Size Review 
					}
				}
				// re-activate this when size packs are being displayed on size review
				// snm = ap.GetSizeNeedMethod(new GeneralComponent(eGeneralComponentType.DetailType));
				// if (snm != null)
				// {
				// // BEGIN MID Track #2519 - Remove unused variables from the column chooser
				// //           Temporarily comment out the following lines  
				// if (addCurveVariablesToSizeVariables)
				// {
				// 	addCurveVariablesToSizeVariables = false;
				//	_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.SizeCurvePct));
				//	_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.SizeNeed));
				//	_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.SizePctNeed));
				//	_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.SizePositiveNeed));
				//	_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.SizePositivePctNeed));
				//	_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.SizePlan));
				// }
				// // END MID Track #2519
				// 	if (sizeNeedMethodHash.Contains(snm.Key))
				//	{
				//	}
				//	else
				//	{
				//		sizeNeedMethodHash.Add(snm.Key, snm);
				//		sizesWithPosCurvePctHash = DetermineSizesWithPosCurvePct(sizesWithPosCurvePctHash, snm.GetSizeCurve(Include.DummyColorRID));
				//	}
				// }
				foreach (HdrColorBin hcb in ap.BulkColors.Values)
				{
					// begin MID Track 4074 Missing sizes in size review
					//if (ap.SizeGroupRID == Include.UndefinedSizeGroupRID)
					//{
					//	bcsKeys = ap.GetBulkColorSizeKeys(hcb.ColorKey);
						//for (int i = 0; i < bcsKeys.Length; i++)
						//{
						//	SizeCodeProfile scp3 =  _trans.SAB.HierarchyServerSession.GetSizeCodeProfile(bcsKeys[i]);
						//	 
						//	if (!primCodeSeqList.ContainsKey(i))
						//		primCodeSeqList.Add(i,scp3.SizeCodePrimaryRID);
						//
						//	if (!secCodeSeqList.ContainsKey( i))
						//		secCodeSeqList.Add( i, scp3.SizeCodeSecondaryRID);
						//
						//	if (!hdrSecondaryRIDs.ContainsKey( scp3.SizeCodeSecondaryRID))
						//		hdrSecondaryRIDs.Add(scp3.SizeCodeSecondaryRID,ap.Key);
						//}
					//}
					// end MID Track 4074 Missing sizes in size review
					snm = ap.GetSizeNeedMethod(hcb);
					if (snm != null)
					{
						// BEGIN MID Track #2519 - Remove unused variables from the column chooser
						//           Temporarily comment out the following lines  
						if (addCurveVariablesToSizeVariables)
						{
							addCurveVariablesToSizeVariables = false;
							_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.SizeCurvePct));
							_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.SizeNeed));
							_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.SizePctNeed));
							_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.SizePositiveNeed));
							_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.SizePositivePctNeed));
							_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.SizePlan));
							// begin MID Track 4921 AnF#666 Fill to Size Plan ENhancement
							// begin MID track 4291 add fill variables to size review
							_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.FillSizeOwnPlan));
							_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.FillSizeOwnNeed));
							_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.FillSizeOwnPctNeed));
							// end MID Track 4291 add fill variables to size review
							_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.FillSizeFwdForecastPlan));
							_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.FillSizeFwdForecastNeed));
							_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.FillSizeFwdForecastPctNeed));
							// end MID Track 4921 AnF#666 Fill to Size Plan Enhancement
							// begin MID Track 3309 display actual onhand/intransit (not curve adjusted)
							_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.CurveAdjdSizeInTransit));
                            _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.CurveAdjdSizeOnHand));
							_sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.CurveAdjdSizeOnHandPlusIT));
							// end MID Track 3309 display actual onhand/intransit (not curve adjusted)
						}
						// END MID Track #2519
						if (sizeNeedMethodHash.Contains(snm.Key))
						{
						}
						else
						{
							sizeNeedMethodHash.Add(snm.Key, snm);
//							_sizesWithPosCurvePctHash = DetermineSizesWithPosCurvePct(_sizesWithPosCurvePctHash, snm.GetSizeCurve(hcb.ColorKey));
						}
					}
				}
                
                // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28D
                //// begin TT#230 Size Review Column Chooser missing columns
                //if (this._trans.SAB.AllowDebugging)
                //{
                //    _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreMaximum));
                //    _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreMinimum));
                //    _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.ShippingStatus));
                //    _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.QtyShipped));
                //    _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.AllocationModifiedAftMultiSplit));
                //    _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.WasAutoAllocated));
                //    _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.QtyAllocatedByAuto));
                //    _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreFilledSizeHoles));
                //    _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreManuallyAllocated));
                //}
                //// end TT#230 Size Review Column Chooser missing columns
                // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28D

				// add secondary size keys for each header to hashtable for later use
				_hdrSecSizeKeys.Add(ap.Key,hdrSecondaryRIDs);
				// BEGIN MID Track #4989 - Sizes out of sequence
				//foreach ( object obj in primCodeSeqList.Values)
				//{ 
				//	if (!primarySizeKeysAL.Contains(obj))
				//		primarySizeKeysAL.Add((int)obj);
				//}
				//foreach ( object obj in secCodeSeqList.Values)
				//{ 
				//	if (!secondarySizeKeysAL.Contains(obj))
				//		secondarySizeKeysAL.Add((int)obj);
				//}

                // Get last sequence number; increment and add RIDs from dupSeqList
                // BEGIN MID Track #5398 - error opening multiple headers -
                //			  occurs when headers with pack sizes are combined with headers with bulk sizes
                //int maxSeq = (int)primCodeSeqList.GetKey(primCodeSeqList.Count - 1); 
                int maxSeq = 0;
                if (primCodeSeqList.Count > 0)
                {
                    maxSeq = (int)primCodeSeqList.GetKey(primCodeSeqList.Count - 1);
                }
                // END MID Track #5398
				for (int i = 0; i <  dupSeqList.Count; i++)
				{
					object obj = (object)dupSeqList.GetByIndex(i);
					maxSeq++;
					primCodeSeqList.Add(maxSeq, obj);
				}


				for (int i = 0; i <  primCodeSeqList.Count; i++)
				{
					object obj = (object)primCodeSeqList.GetByIndex(i);
					if (!primarySizeKeysAL.Contains(obj))
						primarySizeKeysAL.Add((int)obj);
				}
				for (int i = 0; i <  secCodeSeqList.Count; i++)
				{ 
					object obj = (object)secCodeSeqList.GetByIndex(i);
					if (!secondarySizeKeysAL.Contains(obj))
						secondarySizeKeysAL.Add((int)obj);
				}
			}	// END MID Track #4989

            // begin TT#230 Size Review Column Chooser missing columns  >>>>> RMatelic - moved this code from above
            if (this._trans.SAB.AllowDebugging)
            {
                _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreMaximum));
                _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreMinimum));
                _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.ShippingStatus));
                _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.QtyShipped));
                _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.AllocationModifiedAftMultiSplit));
                _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.WasAutoAllocated));
                _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.QtyAllocatedByAuto));
                _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreFilledSizeHoles));
                _sizeColVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreManuallyAllocated));
            }
            // end TT#230 Size Review Column Chooser missing columns>  >>>> end move 

			_primarySizeKeys  = new int[primarySizeKeysAL.Count]; 
			primarySizeKeysAL.CopyTo(_primarySizeKeys,0);
			 
			_secondarySizeKeys  = new int[secondarySizeKeysAL.Count];
			secondarySizeKeysAL.CopyTo(_secondarySizeKeys,0);

			// BEGIN MID Track 3076 There are no summary variables in Size Review
			foreach (AllocationWaferCoordinate variable in this._sizeColVariableCoordList)
			{
                if (variable.Key != (int)eAllocationWaferVariable.QuantityAllocated)    
				{
					col = new AllocationWaferCoordinateList(_trans);
					col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,""));
					col.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.None,0,"Color"));
					col.Add(variable);
					_sizeColGroup2.Add(col);
                }
			}
			// END MID Track 3076

            // BEGIN TT#1401 - AGallagher - Reservation Stores
            //_sizeRowVariableCoordList = new AllocationWaferCoordinateList(_trans);
            //_sizeRowVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.Total));
            //_sizeRowVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreItemQuantityAllocated));
            //_sizeViewRowVariableCount = 2;

            //_sizeRowVariableCoordList = new AllocationWaferCoordinateList(_trans);
            //_sizeRowVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.Total));
            //_sizeRowVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOQuantityAllocated));
            //_sizeViewRowVariableCount = 2;

            //_sizeRowVariableCoordList = new AllocationWaferCoordinateList(_trans);
            //_sizeRowVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.Total));
            //_sizeRowVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.StoreIMOMaxQuantityAllocated));
            //_sizeViewRowVariableCount = 2;
            // END TT#1401 - AGallagher - Reservation Stores

			// BEGIN MID Track #3152 - secondary sizes not showing in Sequential view;
			// this._sizeViewGroupByHash = new Hashtable();
			// END MID Track #3152 

			// ROW Variable Coordinates 
			_sizeRowVariableCoordList = new AllocationWaferCoordinateList(_trans);
			_sizeRowVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.Total));
			_sizeRowVariableCoordList.Add(new AllocationWaferCoordinate(eAllocationWaferVariable.PctToTotal));
			_sizeViewRowVariableCount = 2;

			// Build Color Coordinates
			// begin MID Track #2463 don't display duplicate Total line 
			// only add Total Color line if more than 1 color
			if (_colorKeyList.Count > 1)
				// end MID Track #2463
			{
				waferCoordinate = new AllocationWaferCoordinate(eComponentType.AllColors,0,"Total Color");
				_colorCoordList.Add(waferCoordinate);
			}
			 
			// BEGIN MID Track #3231 - Color description change is not reflected in Review
			//foreach (int colorKey in this._colorKeyList.Values)
            // Begin TT#213 - RMatelic - MID Track #6300-Multi header order in the workspace does not match order in style review
            SortedList sortedBulkColors = new SortedList();
            foreach (ColorData cd in _colorKeyList.Values)
            {
                // Begin TT#326 - RMatelic - highlight 2 header; select style view;get system argument exception error message
                //sortedBulkColors.Add(cd.ColorSequence, cd);
                while (sortedBulkColors.ContainsKey(cd.ColorSequence))
                {
                    cd.ColorSequence++;
                }
                sortedBulkColors.Add(cd.ColorSequence, cd);
                // End TT#326
            }
            //foreach (int cKey in _colorKeyList.Keys) 
            foreach (ColorData cd in sortedBulkColors.Values)
            // End TT#213
			{
				//string colorName = _trans.GetColorCodeProfile(cKey).ColorCodeName;
				string colorName = string.Empty;
                // Begin TT#213 - RMatelic - MID Track #6300-Multi header order in the workspace does not match order in style review
                //ColorData cd = (ColorData)_colorKeyList[cKey]; 
                int cKey = cd.ColorCodeRID;
                // End TT#213
				if (cd.IsDuplicateName)
					colorName = cd.ColorID + " " + cd.ColorName;
				else
					colorName = cd.ColorName;
				waferCoordinate = new AllocationWaferCoordinate(eComponentType.SpecificColor,cKey,colorName);
				_colorCoordList.Add(waferCoordinate);
			}
			// END MID Track #3231

			if (_secondarySizeKeys.Length > 1)
			{
				// begin MID Track 3611 Quick Filter not working in Size Review
				//waferCoordinate = new AllocationWaferCoordinate(eAllocationCoordinateType.SecondarySizeTotal,0,"All Dim");
				waferCoordinate = new AllocationWaferCoordinate(eAllocationCoordinateType.SecondarySizeTotal,0,_allDimAllocatedLabel);
				// end MID Track 3611 Quick Filter not working in Size Review
				_secondSizeCoordList.Add(waferCoordinate);
			}
			eAllocationCoordinateType secondarySizeType;  // MID Track 4326 Cannot manually enter size in Size Review
			foreach (int secondarySizeKey in _secondarySizeKeys)
			{
				// Begin MID Track 3326 When no secondary size and matrix mode, keyed values do not stick
				// waferCoordinate = new AllocationWaferCoordinate(eAllocationCoordinateType.SecondarySize,secondarySizeKey,this._trans.GetSizeDimensionName(false, secondarySizeKey));
				secondarySizeType = eAllocationCoordinateType.SecondarySize; // MID Track 4326 cannot manually enter size in Size Review
				string label = this._trans.GetSizeDimensionName(false, secondarySizeKey);
				if (label == _lblNoSecondarySize ||
					label == string.Empty ||
					label == null || label == _noSizeDimensionLbl) 	// BEGIN/END MID Track #3942 - add  _noSizeDimensionLbl
				{
					label = " ";
					secondarySizeType = eAllocationCoordinateType.SecondarySizeNone; // MID Track 4326 cannot manually enter size in Size Review
				} 
				waferCoordinate = new AllocationWaferCoordinate(secondarySizeType, secondarySizeKey, label); // MID Track 4326 Cannot manually enter size in Size Review
				//waferCoordinate = new AllocationWaferCoordinate(eAllocationCoordinateType.SecondarySize,secondarySizeKey,label); // MID Track 4326 cannot manually enter size in Size Review
				// End MID Track 3326 When no secondary size and matrix mode, keyed values do not stick
				// Only add coordinate if it is a secondary size 
				if (waferCoordinate.Label != _lblNoSecondarySize && waferCoordinate.Label != string.Empty
					&& waferCoordinate.Label != null // MID Change j.ellis Duplicate rows when building size view
					&& waferCoordinate.Label !=  _noSizeDimensionLbl) 	// BEGIN/END MID Track #3942 - add  _noSizeDimensionLbl
				{
					_secondSizeCoordList.Add(waferCoordinate);
				}
			}
			if (_secondSizeCoordList.Count == 0)
			{
				// begin MID Track 3611 Quick Filter not working in Size Review
			    //waferCoordinate = new AllocationWaferCoordinate(eAllocationCoordinateType.SecondarySizeTotal,0,"All Dim"); 
				waferCoordinate = new AllocationWaferCoordinate(eAllocationCoordinateType.SecondarySizeTotal,0,_allDimAllocatedLabel);
				// end MID Track 3611 Quick Filter not working in Size Review
				_secondSizeCoordList.Add(waferCoordinate);
			}
		}

		private void BuildSeqSizeColumns (AllocationProfileList aAllocationProfileList)
		{
			bool showSize;
			this._seqSizeColGroup = new AllocationWaferCoordinateListGroup();
			ProfileList scpl;
			// begin MID Track 3611 Quick Filter not working in Size Review
			long primeSizeDataKey;
			long secondSizeDataKey;
			long primeSecondSizeDataKey;
			_quickFilterSizeDataTable = MIDEnvironment.CreateDataTable();
			_quickFilterSizeDataTable.Columns.Add("PrimeSecondSizeRIDs");
			_quickFilterSizeDataTable.Columns.Add("SizeName");
			_quickFilterSizeDataTable.Columns.Add("SizeType");
			Hashtable quickFilterSizeHash = new Hashtable();
			_lblNoSecondarySize = MIDText.GetTextOnly((int) eMIDTextCode.str_NoSecondarySize); 
			_noSizeDimensionLbl = MIDText.GetTextOnly((int)eMIDTextCode.lbl_NoSecondarySize);
			// end MID Track 3611 Quick Filter not working in Size Review

            Hashtable sizeHash = new Hashtable();    // TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
            int sizeSeq = 0;   // TT#2040 - JEllis - GD - Size Review Sizes display out of sequence
            foreach (int secondSizeKey in _secondarySizeKeys)
			{
				// begin MID Track #2462 - Size headings should display in primary/secondary 
				//string secondDimName = this._trans.GetSizeDimensionName(false, secondSizeKey) + "|";
				string secondDimName = this._trans.GetSizeDimensionName(false, secondSizeKey);
				// end MID Track #2462
				foreach (int primeSizeKey in _primarySizeKeys)
				{
                    sizeSeq++;  // TT#2040 - JEllis GD - Size Review Sizes display out of sequence
					showSize = false;
					if (this._atLeast1WorkUpSizeBuy)
					{
						showSize = true;
					}
					else
					{
						scpl = this._trans.GetSizeCodeByPrimarySecondary(primeSizeKey,secondSizeKey);

						foreach (AllocationProfile ap in aAllocationProfileList)
						{
							foreach (SizeCodeProfile scp in scpl)
							{
								// BEGIN MID Change j.ellis show size curves										// END MID Change j.ellis show size curves
								if (_sizesWithPosCurvePctHash.Contains(scp.Key))
								{
									showSize = true;
									break;
								}
								// END MID Change j.ellis show size curves	
                                // Begin TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
                                //int sizeSeq = 0;  // TT#2040 - JEllis GD - Size Review Sizes display out of sequence 
                                bool showColorSize = false;  
                                // End TT#234 
								foreach (HdrColorBin hcb in ap.BulkColors.Values)
								{
									if (hcb.SizeIsInColor (scp.Key)
										&& hcb.GetSizeUnitsToAllocate(scp.Key) > 0)
									{
                                        // Begin TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
                                        //showSize = true;
                                        showColorSize = true;
                                        // begin TT#2040 - JEllis - GD - Size Review Sizes display out of sequence
                                        //HdrSizeBin hsb = hcb.GetSizeBin(scp.Key);
                                        //sizeSeq = hsb.SizeSequence;
                                        // end TT#2040 - JEllis - GD - Size Review Sizes display out of sequence
                                        // End TT#234  
										break;
									}
								}
                                // Begin TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
                                //if (showSize)
                                //{
                                //    break;
                                //}
                                if (showColorSize)
                                {
                                    SortedList sizes = new SortedList();
                                    if (sizeHash.ContainsKey(ap.Key))
                                    {
                                        sizes = (SortedList)sizeHash[ap.Key];
                                        while (sizes.ContainsKey(sizeSeq))
                                        {
                                            sizeSeq++;
                                        }
                                        sizes.Add(sizeSeq, scp);
                                    }
                                    else
                                    {
                                        sizes.Add(sizeSeq, scp);
                                        sizeHash.Add(ap.Key, sizes);
                                    }

                                }
                            }   // End TT#234  
							if (showSize)
							{
								break;
							}
						}
					}

					if (showSize)
					{
						// begin MID Track #2462 - Size headings should display in primary/secondary
						//sizeLabel = secondDimName
						//	+ this._trans.GetSizeDimensionName(true, primeSizeKey);
						//sizeColCoordList = new AllocationWaferCoordinateList(_trans);
						//sizeColCoordList.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.SecondarySize, secondSizeKey,""));
						//sizeColCoordList.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.PrimarySize, primeSizeKey, sizeLabel));
						// begin MID Track 3611 Quick Filter not working in Size Review
						//string sizeLabel = this._trans.GetSizeDimensionName(true, primeSizeKey) 
						//	+   "|" + secondDimName;
						string sizeLabel;
						eAllocationCoordinateType secondarySizeType = eAllocationCoordinateType.SecondarySize; // MID Track 4326 Cannot manually enter size in Size Review
						if (secondDimName != _lblNoSecondarySize
							&& secondDimName != _noSizeDimensionLbl)
						{
							sizeLabel = 
								this._trans.GetSizeDimensionName(true, primeSizeKey) 
								+   " | " + secondDimName;
						}
						else
						{
							sizeLabel =
								this._trans.GetSizeDimensionName(true, primeSizeKey) ;
							secondarySizeType = eAllocationCoordinateType.SecondarySizeNone; // MID Track 4326 Cannot manually enter size in Size Review
						}
						// end MID Track 3611 Quick Filter not working in Size Review

						AllocationWaferCoordinateList sizeColCoordList = new AllocationWaferCoordinateList(_trans);
						sizeColCoordList.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.PrimarySize, primeSizeKey, sizeLabel));
						sizeColCoordList.Add(new AllocationWaferCoordinate(secondarySizeType, secondSizeKey,"")); // MID Track 4326 Cannot manually enter size in Size Review
						//sizeColCoordList.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.SecondarySize, secondSizeKey,"")); // MID Track 4326 cannot manually enter size in Size Review
						// end MID Track #2462						
						this._seqSizeColGroup.Add(sizeColCoordList);
						// begin MID Track 3611 Quick Filter not working in Size Review
						primeSizeDataKey = ((long)(long)primeSizeKey << 32);
                        // Begin TT#3006 - JSmith - Size REview Screen-> process a quick filter and get System Argument error message
                        //secondSizeDataKey = (long)secondSizeKey;
                        if (secondSizeKey == Include.NoRID)
                        {
                            secondSizeDataKey = ((long)1 << 32) - (long)1;

                        }
                        else
                        {
                            secondSizeDataKey = (long)secondSizeKey;
                        }
                        // End TT#3006 - JSmith - Size REview Screen-> process a quick filter and get System Argument error message
						primeSecondSizeDataKey = primeSizeDataKey + secondSizeDataKey;
						if (!quickFilterSizeHash.Contains(primeSizeDataKey))
						{
							quickFilterSizeHash.Add(primeSizeDataKey, primeSizeDataKey);
							if (secondDimName != _lblNoSecondarySize
								&& secondDimName != _noSizeDimensionLbl)
							{
								_quickFilterSizeDataTable.Rows.Add(new object[] {primeSizeDataKey, this._trans.GetSizeDimensionName(true, primeSizeKey) + " | " + _allDimAllocatedLabel, (int)eSpecificBulkType.SpecificSizePrimaryDim});
							}
						}
						if (!quickFilterSizeHash.Contains(secondSizeDataKey))
						{
							quickFilterSizeHash.Add(secondSizeDataKey, secondSizeDataKey);
							if (secondDimName != _lblNoSecondarySize
								&& secondDimName != _noSizeDimensionLbl)
							{
								_quickFilterSizeDataTable.Rows.Add(new object[] {secondSizeDataKey, _sizeTotalAllocatedLabel + " | " + secondDimName, (int)eSpecificBulkType.SpecificSizeSecondaryDim});
							}
						}
						if (!quickFilterSizeHash.Contains(primeSecondSizeDataKey))
						{
							quickFilterSizeHash.Add(primeSecondSizeDataKey, primeSecondSizeDataKey);
							_quickFilterSizeDataTable.Rows.Add(new object[] {primeSecondSizeDataKey, sizeLabel, (int)eSpecificBulkType.SpecificSize});
						}
						// end MID Track 3611 Quick Filter not working in Size Review
					}
				}
               
			}
            // Begin TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
            if (sizeHash.Count > 0)
            {
                SortedList SizeSL = new SortedList();
                foreach (SortedList sizes in sizeHash.Values)
                {
                    foreach (int seq in sizes.Keys)
                    {
                        SizeCodeProfile scp = (SizeCodeProfile)sizes[seq];
                        if (!SizeSL.ContainsValue(scp))
                        {
                            int seq2 = seq;
                            while (SizeSL.ContainsKey(seq2))
                            {
                                seq2++;
                            }
                            SizeSL.Add(seq2, scp);
                        }
                    }
                }
                if (SizeSL.Count > 0)
                {
                    ArrayList al = new ArrayList();
                    ArrayList primarySecondaryAL = new ArrayList();  // TT#817 - RMatelic - Header with dimensions: Select Size Review. Get incorrect size names on the columns. 4:ID=30404
                    foreach (int seq in SizeSL.Keys)
                    {
                        SizeCodeProfile scp = (SizeCodeProfile)SizeSL[seq];
                        string sizeLabel = scp.SizeCodePrimary;
                        // Begin TT#817 - RMatelic - Header with dimensions: Select Size Review. Get incorrect size names on the columns. 4:ID=30404
                        string primaryPlusSecondary = scp.SizeCodePrimaryRID.ToString() + "~" + scp.SizeCodeSecondaryRID.ToString();
                        eAllocationCoordinateType secondarySizeType = eAllocationCoordinateType.SecondarySize;
                        if (scp.SizeCodeSecondary != _lblNoSecondarySize
                            && scp.SizeCodeSecondary != _noSizeDimensionLbl)
                        {
                            sizeLabel += " | " + scp.SizeCodeSecondary;
                        }
                        else
                        {
                            secondarySizeType = eAllocationCoordinateType.SecondarySizeNone;
                        }
                        // End TT#817
                        if (al.Contains(sizeLabel))
                        {
                            // Begin TT#817 - RMatelic - Header with dimensions: Select Size Review. Get incorrect size names on the columns. 4:ID=30404
                            //sizeLabel += _dupSizeNameSeparator + scp.SizeCodeID;
                            if (primarySecondaryAL.Contains(primaryPlusSecondary))
                            {
                                sizeLabel += _dupSizeNameSeparator + scp.SizeCodeID;
                            }
                            else
                            {
                                primarySecondaryAL.Add(primaryPlusSecondary);
                            }
                            // End TT#817
                        }
                        if (!al.Contains(sizeLabel))
                        {
                            al.Add(sizeLabel);
                         
                            // Begin TT#817 - RMatelic - Header with dimensions: Select Size Review. Get incorrect size names on the columns. 4:ID=30404
                            if (!primarySecondaryAL.Contains(primaryPlusSecondary))
                            {
                                primarySecondaryAL.Add(primaryPlusSecondary);
                            }
                            //eAllocationCoordinateType secondarySizeType = eAllocationCoordinateType.SecondarySize;
                            //if (scp.SizeCodeSecondary != _lblNoSecondarySize
                            //    && scp.SizeCodeSecondary != _noSizeDimensionLbl)
                            //{
                            //    sizeLabel += " | " + scp.SizeCodeSecondary;
                            //}
                            //else
                            //{
                            //    secondarySizeType = eAllocationCoordinateType.SecondarySizeNone;
                            //}
                            // End TT#817  

                            AllocationWaferCoordinateList sizeColCoordList = new AllocationWaferCoordinateList(_trans);
                            sizeColCoordList.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.PrimarySize, scp.SizeCodePrimaryRID, sizeLabel));
                            sizeColCoordList.Add(new AllocationWaferCoordinate(secondarySizeType, scp.SizeCodeSecondaryRID, ""));
                            this._seqSizeColGroup.Add(sizeColCoordList);

                            primeSizeDataKey = ((long)(long)scp.SizeCodePrimaryRID << 32);
                            // Begin TT#3006 - JSmith - Size REview Screen-> process a quick filter and get System Argument error message
                            //secondSizeDataKey = (long)scp.SizeCodeSecondaryRID;
                            if (scp.SizeCodeSecondaryRID == Include.NoRID)
                            {
                                secondSizeDataKey = ((long)1 << 32) - (long)1;

                            }
                            else
                            {
                                secondSizeDataKey = (long)scp.SizeCodeSecondaryRID;
                            }
                            // End TT#3006 - JSmith - Size REview Screen-> process a quick filter and get System Argument error message
                            primeSecondSizeDataKey = primeSizeDataKey + secondSizeDataKey;
                            if (!quickFilterSizeHash.Contains(primeSizeDataKey))
                            {
                                quickFilterSizeHash.Add(primeSizeDataKey, primeSizeDataKey);
                                if (scp.SizeCodeSecondary != _lblNoSecondarySize
                                    && scp.SizeCodeSecondary != _noSizeDimensionLbl)
                                {
                                    _quickFilterSizeDataTable.Rows.Add(new object[] { primeSizeDataKey, this._trans.GetSizeDimensionName(true, scp.SizeCodePrimaryRID) + " | " + _allDimAllocatedLabel, (int)eSpecificBulkType.SpecificSizePrimaryDim });
                                }
                            }
                            if (!quickFilterSizeHash.Contains(secondSizeDataKey))
                            {
                                quickFilterSizeHash.Add(secondSizeDataKey, secondSizeDataKey);
                                if (scp.SizeCodeSecondary != _lblNoSecondarySize
                                    && scp.SizeCodeSecondary != _noSizeDimensionLbl)
                                {
                                    _quickFilterSizeDataTable.Rows.Add(new object[] { secondSizeDataKey, _sizeTotalAllocatedLabel + " | " + scp.SizeCodeSecondary, (int)eSpecificBulkType.SpecificSizeSecondaryDim });
                                }
                            }
                            if (!quickFilterSizeHash.Contains(primeSecondSizeDataKey))
                            {
                                quickFilterSizeHash.Add(primeSecondSizeDataKey, primeSecondSizeDataKey);
                                _quickFilterSizeDataTable.Rows.Add(new object[] { primeSecondSizeDataKey, sizeLabel, (int)eSpecificBulkType.SpecificSize });
                            }
                        }
                    }
                }
            }
            // End TT#234   
            // begin MID Track 3611 Quick Filter not working in Size Review
			if (_secondarySizeKeys.Length > 1)
			{
				_quickFilterSizeDataTable.Rows.Add(new object[] {(long)0, _sizeTotalAllocatedLabel + " | " + _allDimAllocatedLabel, 0});
			}
			else
			{
				_quickFilterSizeDataTable.Rows.Add(new object[] {(long)0, _sizeTotalAllocatedLabel, 0});
			}
			// end MID Track 3611 Quick Filter not working in Size Review
		}

		private void BuildMatrixSizeColumns (AllocationProfileList aAllocationProfileList)
		{
			bool showSize;
			this._mtrxSizeColGroup = new AllocationWaferCoordinateListGroup();
			ProfileList scpl;
            Hashtable sizeHash = new Hashtable();    // TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
            int sizeSeq = 0; // TT#2040 - JEllis GD - Size Review Sizes display out of sequence
            foreach (int primeSizeKey in _primarySizeKeys)
			{
                sizeSeq++;   // TT#2040 - JEllis - GD - Size Review Sizes display out of sequence
				showSize = false;
				if (this._atLeast1WorkUpSizeBuy)
				{
					showSize = true;
				}
				else
				{
					scpl = this._trans.GetSizeCodeByPrimaryDim(primeSizeKey);
					foreach (AllocationProfile ap in aAllocationProfileList)
					{
						foreach (SizeCodeProfile scp in scpl)
						{
                            // BEGIN MID Change j.ellis show size curves									
							if (_sizesWithPosCurvePctHash.Contains(scp.Key))
							{
								showSize = true;
								break;
							}
							// END MID Change j.ellis show size curves	
                            // Begin TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
                            //int sizeSeq = 0;  
                            bool showColorSize = false;  
                            // End TT#234  
							foreach (HdrColorBin hcb in ap.BulkColors.Values)
							{
								if (hcb.SizeIsInColor (scp.Key)
									&& hcb.GetSizeUnitsToAllocate(scp.Key) > 0)
								{
                                    // Begin TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
                                    //showSize = true;
                                    showColorSize = true;
                                    // begin TT#2040 - JEllis - GD - Size Review Sizes display out of sequence
                                    //HdrSizeBin hsb = hcb.GetSizeBin(scp.Key);
                                    //sizeSeq = hsb.SizeSequence;
                                    // end TT#2040 - JEllis - GD - Size Review Sizes display out of sequence
                                    // End TT#234  
									break;
								}
							}
                            // Begin TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
                            //if (showSize)
                            //{
                            //    break;
                            //}
                            if (showColorSize)
                            {
                                SortedList sizes = new SortedList();
                                if (sizeHash.ContainsKey(ap.Key))
                                {
                                    sizes = (SortedList)sizeHash[ap.Key];
                                    while (sizes.ContainsKey(sizeSeq))
                                    {
                                        sizeSeq++;
                                    }
                                    sizes.Add(sizeSeq, scp);
                                }
                                else
                                {
                                    sizes.Add(sizeSeq, scp);
                                    sizeHash.Add(ap.Key, sizes);
                                }
                            }
                        }   // End TT#234  
                        if (showSize)
                        {
                            break;
                        }
                    }  
				}
                if (showSize)
                {
                    string sizeLabel = this._trans.GetSizeDimensionName(true, primeSizeKey);
                    AllocationWaferCoordinateList sizeColCoordList = new AllocationWaferCoordinateList(_trans);
                    sizeColCoordList.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.PrimarySize, primeSizeKey, sizeLabel));
                    this._mtrxSizeColGroup.Add(sizeColCoordList);
                }
            }
            // Begin TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
            if (sizeHash.Count > 0)
            {
                SortedList SizeSL = new SortedList();
                foreach (SortedList sizes in sizeHash.Values)
                {
                    foreach (int seq in sizes.Keys)
                    {
                        SizeCodeProfile scp = (SizeCodeProfile)sizes[seq];
                        if (!SizeSL.ContainsValue(scp))
                        {
                            int seq2 = seq;
                            while (SizeSL.ContainsKey(seq2))
                            {
                                seq2++;
                            }
                            SizeSL.Add(seq2, scp);
                        }
                    }
                }
                if (SizeSL.Count > 0)
                {   
                    ArrayList al = new ArrayList();
                    ArrayList primarySecondaryAL = new ArrayList();  // TT#817 - RMatelic - Header with dimensions: Select Size Review. Get incorrect size names on the columns. 4:ID=30404
                    foreach (int seq in SizeSL.Keys)
                    {
                        SizeCodeProfile scp = (SizeCodeProfile)SizeSL[seq];
                        string sizeLabel = scp.SizeCodePrimary;

                        // Begin TT#817 - RMatelic - Header with dimensions: Select Size Review. Get incorrect size names on the columns. 4:ID=30404
                        string primaryPlusSecondary = scp.SizeCodePrimaryRID.ToString() + "~" + scp.SizeCodeSecondaryRID.ToString();
                        // End TT#817
                        if (al.Contains(sizeLabel))
                        {
                            // Begin TT#817 - RMatelic - Header with dimensions: Select Size Review. Get incorrect size names on the columns. 4:ID=30404
                            //sizeLabel += _dupSizeNameSeparator + scp.SizeCodeID;
                            if (primarySecondaryAL.Contains(primaryPlusSecondary))
                            {
                                sizeLabel += _dupSizeNameSeparator + scp.SizeCodeID;
                            }
                            else
                            {
                                primarySecondaryAL.Add(primaryPlusSecondary);
                            }
                            // End TT#817
                        }
                        if (!al.Contains(sizeLabel))
                        {
                            al.Add(sizeLabel);
                            // TT#817 - RMatelic - Header with dimensions: Select Size Review. Get incorrect size names on the columns. 4:ID=30404
                            if (!primarySecondaryAL.Contains(primaryPlusSecondary))
                            {
                                primarySecondaryAL.Add(primaryPlusSecondary);   
                            }
                            // End TT#817  
                            AllocationWaferCoordinateList sizeColCoordList = new AllocationWaferCoordinateList(_trans);
                            sizeColCoordList.Add(new AllocationWaferCoordinate(eAllocationCoordinateType.PrimarySize, scp.SizeCodePrimaryRID, sizeLabel));
                            this._mtrxSizeColGroup.Add(sizeColCoordList);
                        }
                    }
                }
            }
            // End TT#234 
        }   

		private Hashtable DetermineSizesWithPosCurvePct(Hashtable aSizesWithPosCurvePctHash, Hashtable aSizeCurves)
		{
			foreach (SizeCurveProfile curveProfile in aSizeCurves.Values)
			{
				foreach (SizeCodeProfile scp in curveProfile.SizeCodeList)
				{
					if (!aSizesWithPosCurvePctHash.Contains(scp.Key))
					{
						if (curveProfile.GetSizePercent(scp.Key) > 0)
						{
							aSizesWithPosCurvePctHash.Add(scp.Key, scp.Key);
						}
					}
				}
			}
			return aSizesWithPosCurvePctHash;
		}

		/// <summary>
		/// Builds the rows associated with a store (set) coordinate
		/// </summary>
		/// <param name="aStoreCoordList">Store (set) coordinate list</param>
		/// <param name="aHeaderCoordList">Header coordinate list</param>
		/// <param name="aColorCoordList">Color coordinate list</param>
		/// <param name="aSecondSizeCoordList">Second size coordinate list</param>
		/// <param name="aVariableCoordList">Allocation variable list</param>
		/// <returns></returns>
		private AllocationWaferCoordinateListGroup BuildSizeRows(
			AllocationWaferCoordinateList aStoreCoordList,
			AllocationWaferCoordinateList aHeaderCoordList,
			AllocationWaferCoordinateList aColorCoordList,
			AllocationWaferCoordinateList aSecondSizeCoordList,
			AllocationWaferCoordinateList aVariableCoordList)
		{
			AllocationWaferCoordinateListGroup rowGroup = new AllocationWaferCoordinateListGroup();
			AllocationWaferCoordinateList row;
			AllocationProfile ap;
			switch (_sel.GroupBy)
			{
				case ((int)eAllocationSizeViewGroupBy.Header):
				{
					foreach (Object storeObj in aStoreCoordList)
					{
						foreach (AllocationWaferCoordinate headerAWC in aHeaderCoordList)
						{
							if (headerAWC.CoordinateType == eAllocationCoordinateType.HeaderTotal)
							{
								foreach(AllocationWaferCoordinate colorAWC in aColorCoordList)
								{
									if (aSecondSizeCoordList != null
										&& aSecondSizeCoordList.Count > 0)
									{
										foreach (AllocationWaferCoordinate sizeAWC in aSecondSizeCoordList)
										{
											foreach (Object variableObj in aVariableCoordList)
											{
												row = new AllocationWaferCoordinateList(_trans);
												row.Add(storeObj);
												row.Add(headerAWC);
												row.Add(colorAWC);
												row.Add(sizeAWC);
												row.Add(variableObj);
												rowGroup.Add(row);
											}
										}
									}
									else
									{
										foreach (Object variableObj in aVariableCoordList)
										{
											row = new AllocationWaferCoordinateList(_trans);
											row.Add(storeObj);
											row.Add(headerAWC);
											row.Add(colorAWC);
											row.Add(variableObj);
											rowGroup.Add(row);
										}
									}
								}
							}
							else
							{
								ap = this._trans.GetAllocationProfile(headerAWC.Key);
                                // Begin TT#3805 - stodd - Group Allocation - Need Action against Headers Errors when Style and Size Review are open
                                // Sometimes we get into the situation (mostly in group allocation/Assortment) where the Allocation Profile list
                                // no longer contains the header that is currently viewed in Size Review. That header should always be in the 
                                // Assortment Member list.
                                if (ap == null)
                                {
                                    ap = _trans.GetAssortmentMemberProfile(headerAWC.Key);
                                }
                                // End TT#3805 - stodd - Group Allocation - Need Action against Headers Errors when Style and Size Review are open

								foreach(AllocationWaferCoordinate colorAWC in aColorCoordList)
								{
									if (ap.BulkColorIsOnHeader(colorAWC.Key))
									{
										HdrColorBin hcb = ap.GetHdrColorBin(colorAWC.Key);
										if (aSecondSizeCoordList != null
											&& aSecondSizeCoordList.Count > 0)
										{
											foreach (AllocationWaferCoordinate sizeAWC in aSecondSizeCoordList)
											{
												if (IncludeSecondarySizeRow(headerAWC.Key, sizeAWC))
													foreach (Object variableObj in aVariableCoordList)
													{
														row = new AllocationWaferCoordinateList(_trans);
														row.Add(storeObj);
														row.Add(headerAWC);
														row.Add(colorAWC);
														row.Add(sizeAWC);
														row.Add(variableObj);
														rowGroup.Add(row);
													}
											}
										}
										else
										{
											foreach (Object variableObj in aVariableCoordList)
											{
												row = new AllocationWaferCoordinateList(_trans);
												row.Add(storeObj);
												row.Add(headerAWC);
												row.Add(colorAWC);
												row.Add(variableObj);
												rowGroup.Add(row);
											}
										}
									}
                                    // BEGIN MID Change j.ellis Total Color not displaying when multiple colors
									else
									{
										if (colorAWC.CoordinateType == eAllocationCoordinateType.Component
											&& colorAWC.CoordinateSubType == (int)eComponentType.AllColors)
										{
											if (aSecondSizeCoordList != null
												&& aSecondSizeCoordList.Count > 0)
											{
												foreach (AllocationWaferCoordinate sizeAWC in aSecondSizeCoordList)
												{
													if (IncludeSecondarySizeRow(headerAWC.Key, sizeAWC))
														foreach (Object variableObj in aVariableCoordList)
														{
															row = new AllocationWaferCoordinateList(_trans);
															row.Add(storeObj);
															row.Add(headerAWC);
															row.Add(colorAWC);
															row.Add(sizeAWC);
															row.Add(variableObj);
															rowGroup.Add(row);
														}
												}
											}
											else
											{
												foreach (Object variableObj in aVariableCoordList)
												{
													row = new AllocationWaferCoordinateList(_trans);
													row.Add(storeObj);
													row.Add(headerAWC);
													row.Add(colorAWC);
													row.Add(variableObj);
													rowGroup.Add(row);
												}
											}
										}
									}
									// BEGIN MID Change j.ellis Total Color not displaying when multiple colors

								}
							}
						}
					}
					break;
				}
				case ((int)eAllocationSizeViewGroupBy.Color):
				{
					foreach (Object storeObj in aStoreCoordList)
					{
						foreach(AllocationWaferCoordinate colorAWC in aColorCoordList)
						{
							foreach (AllocationWaferCoordinate headerAWC in aHeaderCoordList)
							{
								if (headerAWC.CoordinateType == eAllocationCoordinateType.HeaderTotal)
								{
									if (aSecondSizeCoordList != null
										&& aSecondSizeCoordList.Count > 0)
									{
										foreach (AllocationWaferCoordinate sizeAWC in aSecondSizeCoordList)
										{
											foreach (Object variableObj in aVariableCoordList)
											{
												row = new AllocationWaferCoordinateList(_trans);
												row.Add(storeObj);
												row.Add(colorAWC);
												row.Add(headerAWC);
												row.Add(sizeAWC);
												row.Add(variableObj);
												rowGroup.Add(row);
											}
										}
									}
									else
									{
										foreach (Object variableObj in aVariableCoordList)
										{
											row = new AllocationWaferCoordinateList(_trans);
											row.Add(storeObj);
											row.Add(colorAWC);
											row.Add(headerAWC);
											row.Add(variableObj);
											rowGroup.Add(row);
										}
									}
								}
								else
								{
									ap = this._trans.GetAllocationProfile(headerAWC.Key);
                                    // Begin TT#3805 - stodd - Group Allocation - Need Action against Headers Errors when Style and Size Review are open
                                    // Sometimes we get into the situation (mostly in group allocation/Assortment) where the Allocation Profile list
                                    // no longer contains the header that is currently viewed in Size Review. That header should always be in the 
                                    // Assortment Member list.
                                    if (ap == null)
                                    {
                                        ap = _trans.GetAssortmentMemberProfile(headerAWC.Key);
                                    }
                                    // End TT#3805 - stodd - Group Allocation - Need Action against Headers Errors when Style and Size Review are open

									if (ap.BulkColorIsOnHeader(colorAWC.Key))
									{
										HdrColorBin hcb = ap.GetHdrColorBin(colorAWC.Key);
										if (aSecondSizeCoordList != null
											&& aSecondSizeCoordList.Count > 0)
										{
											foreach (AllocationWaferCoordinate sizeAWC in aSecondSizeCoordList)
											{
												if (IncludeSecondarySizeRow(headerAWC.Key, sizeAWC))
													foreach (Object variableObj in aVariableCoordList)
													{
														row = new AllocationWaferCoordinateList(_trans);
														row.Add(storeObj);
														row.Add(colorAWC);
														row.Add(headerAWC);
														row.Add(sizeAWC);
														row.Add(variableObj);
														rowGroup.Add(row);
													}
											}
										}
										else
										{
											foreach (Object variableObj in aVariableCoordList)
											{
												row = new AllocationWaferCoordinateList(_trans);
												row.Add(storeObj);
												row.Add(colorAWC);
												row.Add(headerAWC);
												row.Add(variableObj);
												rowGroup.Add(row);
											}
										}
									}
								}
							}
						}
					}
					break;
				}
				default:
                    {
                        //System.Windows.Forms.MessageBox.Show("Selected GroupBy is not implemented for Size View");
                        DialogResult diagResult = _trans.SAB.MessageCallback.HandleMessage(
                                            MIDText.GetText(eMIDTextCode.msg_al_InvalidSizeGroupBy),
                                            "",
                                            MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.None);
                        break;
                    }
			}
			return rowGroup;
		}

		private bool IncludeSecondarySizeRow(int aHeaderRID, AllocationWaferCoordinate aSizeAWC)
		{
			bool includeSize = true;  
			if (aSizeAWC.CoordinateType == eAllocationCoordinateType.SecondarySizeTotal)
			{
			}
			else if (_hdrSecSizeKeys.ContainsKey(aHeaderRID))
			{
				Hashtable secSizeKeys = (Hashtable)_hdrSecSizeKeys[aHeaderRID]; 
				if (!secSizeKeys.ContainsKey(aSizeAWC.Key)) 
					includeSize = false; 
			}
			return includeSize;
		}
		#endregion SizeView

		#region IncludeCoordinate
		/// <summary>
		/// Indicates if a coordinate is included in the view
		/// </summary>
		/// <param name="ap">Allocation Profile</param>
		/// <param name="coordB">AllocationWaferCoordinate</param>
		/// <returns>True: if the coordinate is included in the view; False: if the coordinate is not included in the view</returns>
		private bool IncludeCoordinate(AllocationProfile ap,
			AllocationWaferCoordinate coordB)
		{
			switch (coordB.CoordinateSubType)
			{
				case (int)eComponentType.AllPacks:
					if (ap.Packs.Count == 0)
					{
						return false;
					}
					break;
				case (int)eComponentType.SpecificPack:
					if (!ap.PackIsOnHeader(coordB.PackName)
						|| ap.GetSubtotalPackName(coordB.PackName) != coordB.SubtotalPackName)
					{
						return false;
					}
					break;
				case (int)eComponentType.SpecificColor:
					if (!ap.BulkColorIsOnHeader(coordB.Key))
					{
						return false;
					}
					break;
                // begin TT#1436 - MD - JEllis - GA allocates bulk before packs
                case (int)eComponentType.Bulk:
                    if (ap.BulkColors.Count == 0)
                    {
                        return false;
                    }
                    break;
                // end TT#1436 - MD - JEllis - GA allocates bulk before packs
			}
			
			return true;
		}
		#endregion IncludeCoordinate

		#endregion Build
		#endregion BuildWafers

		#region EnqDeqHeaders
		#region Enqueue
        // begin TT#1185 - Verify ENQ before Update
        /// <summary>
        /// Displays Enqueue Conflicts
        /// </summary>
        public void DisplayEnqueueConflict(HeaderConflict[] aHeaderConflictList)
        {
            string errMsg;
            //Begin TT#827-MD -jsobek -Allocation Reviews Performance
            //SecurityAdmin secAdmin;
            //secAdmin = new SecurityAdmin();
            errMsg = "The following headers are in use:" + System.Environment.NewLine;
            foreach (HeaderConflict hdrCon in aHeaderConflictList)
            {
                //errMsg += System.Environment.NewLine + hdrCon.HeaderID + ", User: " + secAdmin.GetUserName(hdrCon.UserRID) + ", Thread: " + hdrCon.ThreadID.ToString() + ", Transaction: " + hdrCon.TransactionID.ToString();
                errMsg += System.Environment.NewLine + hdrCon.HeaderID + ", User: " + UserNameStorage.GetUserName(hdrCon.UserRID) + ", Thread: " + hdrCon.ThreadID.ToString() + ", Transaction: " + hdrCon.TransactionID.ToString();           
            }
            //End TT#827-MD -jsobek -Allocation Reviews Performance
            errMsg += System.Environment.NewLine + System.Environment.NewLine;
            errMsg += "Do you wish to continue with the selected view as read-only?";

            diagResult = _trans.SAB.MessageCallback.HandleMessage(
                errMsg,
                "Header Lock Conflict",
                System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Asterisk);

            if (diagResult == System.Windows.Forms.DialogResult.Cancel)
            {
                throw new CancelProcessException();
            }
        }
        ///// <summary>
        ///// Enqueues Headers for update
        ///// </summary>
         //public void EnqueueHeaders()
        //{
        //    string errMsg;
        //    SecurityAdmin secAdmin;
        //    AllocationProfile ap;
        //    _headerEnqueue = new HeaderEnqueue(_trans);
        //    try
        //    {
        //        _headerEnqueue.EnqueueHeaders();
        //        _headersEnqueued = true; 
        //    }
        //    catch (HeaderConflictException)
        //    {
        //        secAdmin = new SecurityAdmin();
        //        errMsg = "The following headers are in use:" + System.Environment.NewLine;
        //        foreach (HeaderConflict hdrCon in _headerEnqueue.HeaderConflictList)
        //        {
        //            // errMsg += System.Environment.NewLine + hdrCon.HeaderRID.ToString(CultureInfo.CurrentUICulture) +  ", User: " + secAdmin.GetUserName(hdrCon.UserRID);
        //            ap = (AllocationProfile)_apl.FindKey(System.Convert.ToInt32(hdrCon.HeaderRID, CultureInfo.CurrentUICulture));
        //            errMsg += System.Environment.NewLine + ap.HeaderID +  ", User: " + secAdmin.GetUserName(hdrCon.UserRID);
        //        }
        //        errMsg += System.Environment.NewLine + System.Environment.NewLine;
        //        errMsg += "Do you wish to continue with the selected view as read-only?";

        //        diagResult = _trans.SAB.MessageCallback.HandleMessage(
        //            errMsg,
        //            "Header Lock Conflict",
        //            System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Asterisk);

        //        if (diagResult == System.Windows.Forms.DialogResult.Cancel)
        //        {
        //            throw new CancelProcessException();
        //        }
        //    }
        //}
        // end TT#1185 - Verfiry ENQ before Update
		#endregion Enqueue

		#region Dequeue
		/// <summary>
		/// Verifies that a header may be dequeued
		/// </summary>
        public void CheckForHeaderDequeue()
        {
            // begin TT#1185 - Verify ENQ before Update 
            if (_styleView == null
                && _sizeView == null
                && _summaryView == null
                && _assortmentView == null               
                && !_trans.VelocityCriteriaExists)
            {
                this._trans.DequeueHeaders();
            }
        }
            //// BEGIN MID Track #2515 - Object reference error when closing size review
            ////           Check to see that switch is set before trying to dequeue
            ////if (_styleView == null && _summaryView == null && _sizeView == null)
            //if (_styleView == null && _summaryView == null && _sizeView == null && _assortmentView == null && _headersEnqueued)
            //// END MID Track #2515
            //    DequeueHeaders();
        //}

        // begin TT#1185 - Verify ENQ before Update/// <summary>
        ///// Dequeues a header 
        ///// </summary>
        //public void DequeueHeaders()
        //{
        //    // begin TT#1185 - Verify ENQ before Update

            //try
            //{
            //    // BEGIN MID Track #2515 - Object reference error when closing size review
            //    //           Check to see that _headerEnqueue exists before trying to dequeue
            //    if (_headerEnqueue != null)
            //    // END MID Track #2515
            //        _headerEnqueue.DequeueHeaders();

            //    _headersEnqueued = false; 
            //}
            //catch ( Exception )
            //{
            //    throw;
            //}			
        //}
        // end TT#1185 - Verify ENQ before Update

		#endregion Dequeue
		#endregion EnqDeqHeaders

		#endregion Methods
	}
	#endregion AllocationViewSelectionCriteria Profile
}
