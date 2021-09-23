using System;
using System.Globalization;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using System.Collections;
using System.Collections.Generic;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	/// <summary>
	/// The Transaction class defines the "local" workspace for a series of functions in the client application.
	/// </summary>
	/// <remarks>
	/// This class gives the user the ability to store information that is "local", or unique, to a series of screens or functions.  This allows a Client
	/// application to open multiple functions of the same type, yet each has its own copy of information contained in this class.
	/// </remarks>

	public partial class HierarchySessionTransaction : Transaction
	{
		//=======
		// FIELDS
		//=======
        const int cStockVariable = 0;
        const int cSalesVariable = 1;
		private System.Collections.Hashtable _profileHash;
		private int _maxStoreRID = 0;
        // base cache tables
		private System.Collections.Hashtable _storeEligibilityHash;
        private System.Collections.Hashtable _allocationStockStoreEligibilityHash;
        private System.Collections.Hashtable _allocationSalesStoreEligibilityHash;
        private System.Collections.Hashtable _planningStockStoreEligibilityHash;
        private System.Collections.Hashtable _planningSalesStoreEligibilityHash;

        // color cache tables
        private System.Collections.Hashtable _allocationStockColorStoreEligibilityHash;
        private System.Collections.Hashtable _allocationSalesColorStoreEligibilityHash;
        private System.Collections.Hashtable _planningStockColorStoreEligibilityHash;
        private System.Collections.Hashtable _planningSalesColorStoreEligibilityHash;

        // pack cache tables
        private System.Collections.Hashtable _allocationStockPackStoreEligibilityHash;
        private System.Collections.Hashtable _allocationSalesPackStoreEligibilityHash;
        private System.Collections.Hashtable _planningStockPackStoreEligibilityHash;
        private System.Collections.Hashtable _planningSalesPackStoreEligibilityHash;

        private System.Collections.Hashtable _salesEligibilityModelHash;
		private System.Collections.Hashtable _salesEligibilityModelDateHash;
		private int _currentSalesEligibilityModelRID = Include.NoRID;
        // Begin TT#2307 - JSmith - Incorrect Stock Values
        private ModifierModel _salesEligibilityModifierModel = null;
        // End TT#2307
		private System.Collections.Hashtable _stockEligibilityModelHash;
		private System.Collections.Hashtable _stockEligibilityModelDateHash;
		private int _currentStockEligibilityModelRID = Include.NoRID;
        // Begin TT#2307 - JSmith - Incorrect Stock Values
        private ModifierModel _stockEligibilityModifierModel = null;
        // End TT#2307
		// BEGIN MID Track #4370 - John Smith - FWOS Models
		private System.Collections.Hashtable _FWOSEligibilityModelHash;
		private System.Collections.Hashtable _FWOSEligibilityModelDateHash;
        //private int _currentFWOSEligibilityModelRID = Include.NoRID;
		// END MID Track #4370
		private System.Collections.Hashtable _priorityShippingModelHash;
		private System.Collections.Hashtable _priorityShippingModelDateHash;
		private int _currentPriorityShippingModelRID = Include.NoRID;
        // Begin TT#2307 - JSmith - Incorrect Stock Values
        private ModifierModel _priorityShippingModifierModel = null;
        // End TT#2307
		private System.Collections.Hashtable _salesModifierModelHash;
		private System.Collections.Hashtable _salesModifierModelDateHash;
		private int _currentModifierModelRID = Include.NoRID;
		private ModifierModel _salesModifierModel = null;
		private System.Collections.Hashtable _stockModifierModelHash;
		private System.Collections.Hashtable _stockModifierModelDateHash;
		private int _currentStockModifierModelRID = Include.NoRID;
		private ModifierModel _stockModifierModel = null;
		// BEGIN MID Track #4370 - John Smith - FWOS Models
		private System.Collections.Hashtable _FWOSModifierModelHash;
		private System.Collections.Hashtable _FWOSModifierModelDateHash;
		private int _currentFWOSModifierModelRID = Include.NoRID;
		private ModifierModel _FWOSModifierModel = null;
		// END MID Track #4370
        // Begin TT#4988 - JSmith - Performance
        //private System.Collections.Hashtable _storeSalesStatusHash;
        //private System.Collections.Hashtable _storeStockStatusHash;
        private Dictionary<int, Dictionary<int, eStoreStatus>> _storeSalesStatusHash;
        private Dictionary<int, Dictionary<int, eStoreStatus>> _storeStockStatusHash;
        // End TT#4988 - JSmith - Performance

        private Dictionary<int, HierarchyNodeProfile> _nodeHash;
        private Dictionary<string, int> _storeHash;

        // Begin TT#1440 - JSmith - Memory Issues
        override public void Dispose()
        {
            if (_profileHash != null)
            {
                _profileHash.Clear();
                _profileHash = null;
            }
            if (_storeEligibilityHash != null)
            {
                _storeEligibilityHash.Clear();
                _storeEligibilityHash = null;
            }
            if (_allocationStockStoreEligibilityHash != null)
            {
                _allocationStockStoreEligibilityHash.Clear();
                _allocationStockStoreEligibilityHash = null;
            }
            if (_allocationSalesStoreEligibilityHash != null)
            {
                _allocationSalesStoreEligibilityHash.Clear();
                _allocationSalesStoreEligibilityHash = null;
            }
            if (_planningStockStoreEligibilityHash != null)
            {
                _planningStockStoreEligibilityHash.Clear();
                _planningStockStoreEligibilityHash = null;
            }
            if (_planningSalesStoreEligibilityHash != null)
            {
                _planningSalesStoreEligibilityHash.Clear();
                _planningSalesStoreEligibilityHash = null;
            }

            if (_allocationStockColorStoreEligibilityHash != null)
            {
                _allocationStockColorStoreEligibilityHash.Clear();
                _allocationStockColorStoreEligibilityHash = null;
            }
            if (_allocationSalesColorStoreEligibilityHash != null)
            {
                _allocationSalesColorStoreEligibilityHash.Clear();
                _allocationSalesColorStoreEligibilityHash = null;
            }
            if (_planningStockColorStoreEligibilityHash != null)
            {
                _planningStockColorStoreEligibilityHash.Clear();
                _planningStockColorStoreEligibilityHash = null;
            }
            if (_planningSalesColorStoreEligibilityHash != null)
            {
                _planningSalesColorStoreEligibilityHash.Clear();
                _planningSalesColorStoreEligibilityHash = null;
            }

            if (_allocationStockPackStoreEligibilityHash != null)
            {
                _allocationStockPackStoreEligibilityHash.Clear();
                _allocationStockPackStoreEligibilityHash = null;
            }
            if (_allocationSalesPackStoreEligibilityHash != null)
            {
                _allocationSalesPackStoreEligibilityHash.Clear();
                _allocationSalesPackStoreEligibilityHash = null;
            }
            if (_planningStockPackStoreEligibilityHash != null)
            {
                _planningStockPackStoreEligibilityHash.Clear();
                _planningStockPackStoreEligibilityHash = null;
            }
            if (_planningSalesPackStoreEligibilityHash != null)
            {
                _planningSalesPackStoreEligibilityHash.Clear();
                _planningSalesPackStoreEligibilityHash = null;
            }

            if (_salesEligibilityModelHash != null)
            {
                _salesEligibilityModelHash.Clear();
                _salesEligibilityModelHash = null;
            }
            if (_salesEligibilityModelDateHash != null)
            {
                _salesEligibilityModelDateHash.Clear();
                _salesEligibilityModelDateHash = null;
            }
            if (_stockEligibilityModelHash != null)
            {
                _stockEligibilityModelHash.Clear();
                _stockEligibilityModelHash = null;
            }
            if (_stockEligibilityModelDateHash != null)
            {
                _stockEligibilityModelDateHash.Clear();
                _stockEligibilityModelDateHash = null;
            }
            if (_FWOSEligibilityModelHash != null)
            {
                _FWOSEligibilityModelHash.Clear();
                _FWOSEligibilityModelHash = null;
            }
            if (_FWOSEligibilityModelDateHash != null)
            {
                _FWOSEligibilityModelDateHash.Clear();
                _FWOSEligibilityModelDateHash = null;
            }
            if (_priorityShippingModelHash != null)
            {
                _priorityShippingModelHash.Clear();
                _priorityShippingModelHash = null;
            }
            if (_priorityShippingModelDateHash != null)
            {
                _priorityShippingModelDateHash.Clear();
                _priorityShippingModelDateHash = null;
            }
            if (_salesModifierModelHash != null)
            {
                _salesModifierModelHash.Clear();
                _salesModifierModelHash = null;
            }
            if (_salesModifierModelDateHash != null)
            {
                _salesModifierModelDateHash.Clear();
                _salesModifierModelDateHash = null;
            }
            if (_stockModifierModelHash != null)
            {
                _stockModifierModelHash.Clear();
                _stockModifierModelHash = null;
            }
            if (_stockModifierModelDateHash != null)
            {
                _stockModifierModelDateHash.Clear();
                _stockModifierModelDateHash = null;
            }
            if (_FWOSModifierModelHash != null)
            {
                _FWOSModifierModelHash.Clear();
                _FWOSModifierModelHash = null;
            }
            if (_FWOSModifierModelDateHash != null)
            {
                _FWOSModifierModelDateHash.Clear();
                _FWOSModifierModelDateHash = null;
            }
            if (_storeSalesStatusHash != null)
            {
                _storeSalesStatusHash.Clear();
                _storeSalesStatusHash = null;
            }
            if (_storeStockStatusHash != null)
            {
                _storeStockStatusHash.Clear();
                _storeStockStatusHash = null;
            }
            if (_nodeHash != null)
            {
                _nodeHash.Clear();
                _nodeHash = null;
            }
            if (_storeHash != null)
            {
                _storeHash.Clear();
                _storeHash = null;
            }

            base.Dispose();
        }
        // End TT#1440

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of Transaction under the given SessionAddressBlock.
		/// </summary>
		/// <param name="aSAB">
		/// The SessionAddressBlock that owns this Transaction.
		/// </param>

		public HierarchySessionTransaction(SessionAddressBlock aSAB)
			: base(aSAB)
		{
            // begin MID Track 4607 and 4626 Eligibility and Priority Shipping Wrong
			_profileHash = null;
			_storeEligibilityHash = null;
            _allocationStockStoreEligibilityHash = null;
            _allocationSalesStoreEligibilityHash = null;
            _planningStockStoreEligibilityHash = null;
            _planningSalesStoreEligibilityHash = null;
            _allocationStockColorStoreEligibilityHash = null;
            _allocationSalesColorStoreEligibilityHash = null;
            _planningStockColorStoreEligibilityHash = null;
            _planningSalesColorStoreEligibilityHash = null;
            _allocationStockPackStoreEligibilityHash = null;
            _allocationSalesPackStoreEligibilityHash = null;
            _planningStockPackStoreEligibilityHash = null;
            _planningSalesPackStoreEligibilityHash = null;
			_salesEligibilityModelHash = null;
			_stockEligibilityModelHash = null;
			// BEGIN MID Track #4370 - John Smith - FWOS Models
			_FWOSEligibilityModelHash = null;
			// END MID Track #4370
			_priorityShippingModelHash = null;
			_salesModifierModelHash = null;
			_stockModifierModelHash = null;
			// BEGIN MID Track #4370 - John Smith - FWOS Models
			_FWOSModifierModelHash = null;
			// END MID Track #4370
			_salesEligibilityModelDateHash = null;
			_stockEligibilityModelDateHash = null;
			// BEGIN MID Track #4370 - John Smith - FWOS Models
			_FWOSEligibilityModelDateHash = null;
			// END MID Track #4370
			_priorityShippingModelDateHash = null;
			_salesModifierModelDateHash = null;
			_stockModifierModelDateHash = null;
			// BEGIN MID Track #4370 - John Smith - FWOS Models
			_FWOSModifierModelDateHash = null;
			// END MID Track #4370
			_storeSalesStatusHash = null;
			_storeStockStatusHash = null;
			//_profileHash = new System.Collections.Hashtable();
			//_storeEligibilityHash = new System.Collections.Hashtable();
			//_salesEligibilityModelHash = new System.Collections.Hashtable();
			//_stockEligibilityModelHash = new System.Collections.Hashtable();
			//_priorityShippingModelHash = new System.Collections.Hashtable();
			//_salesModifierModelHash = new System.Collections.Hashtable();
			//_stockModifierModelHash = new System.Collections.Hashtable();
			//_salesEligibilityModelDateHash = new System.Collections.Hashtable();
			//_stockEligibilityModelDateHash = new System.Collections.Hashtable();
			//_priorityShippingModelDateHash = new System.Collections.Hashtable();
			//_salesModifierModelDateHash = new System.Collections.Hashtable();
			//_stockModifierModelDateHash = new System.Collections.Hashtable();
			//_storeSalesStatusHash = new System.Collections.Hashtable();
			//_storeStockStatusHash = new System.Collections.Hashtable();
			// end MID Track 4607 and 4626 Eligibility and Priority Shipping Wrong
            _nodeHash = null;
            _storeHash = null;
		}

		//===========
		// PROPERTIES
		//===========

        // begin MID Track 4607 and 4626 Eligibility and Priority Shipping Not Working
		private Hashtable ProfileHash 
		{
			get 
			{ 
				if (_profileHash == null)
				{
					_profileHash = new System.Collections.Hashtable();
				}
				return _profileHash ; 
			}
			set 
			{ 
				_profileHash = value; 
			}
		}

		private Hashtable StoreEligibilityHash 
		{
			get 
			{ 
				if (_storeEligibilityHash == null)
				{
					_storeEligibilityHash = new System.Collections.Hashtable();
				}
				return _storeEligibilityHash ; 
			}
			set 
			{ 
				_storeEligibilityHash = value; 
			}
		}

		private Hashtable AllocationStockStoreEligibilityHash
        {
			get 
			{ 
				if (_allocationStockStoreEligibilityHash == null)
				{
                    _allocationStockStoreEligibilityHash = new System.Collections.Hashtable();
				}
				return _allocationStockStoreEligibilityHash; 
			}
			set 
			{
                _allocationStockStoreEligibilityHash = value; 
			}
		}

        private Hashtable AllocationSalesStoreEligibilityHash
        {
            get
            {
                if (_allocationSalesStoreEligibilityHash == null)
                {
                    _allocationSalesStoreEligibilityHash = new System.Collections.Hashtable();
                }
                return _allocationSalesStoreEligibilityHash;
            }
            set
            {
                _allocationSalesStoreEligibilityHash = value;
            }
        }

        private Hashtable PlanningStockStoreEligibilityHash
        {
            get
            {
                if (_planningStockStoreEligibilityHash == null)
                {
                    _planningStockStoreEligibilityHash = new System.Collections.Hashtable();
                }
                return _planningStockStoreEligibilityHash;
            }
            set
            {
                _planningStockStoreEligibilityHash = value;
            }
        }

        private Hashtable PlanningSalesStoreEligibilityHash
        {
            get
            {
                if (_planningSalesStoreEligibilityHash == null)
                {
                    _planningSalesStoreEligibilityHash = new System.Collections.Hashtable();
                }
                return _planningSalesStoreEligibilityHash;
            }
            set
            {
                _planningSalesStoreEligibilityHash = value;
            }
        }

        private Hashtable AllocationStockColorStoreEligibilityHash
        {
            get
            {
                if (_allocationStockColorStoreEligibilityHash == null)
                {
                    _allocationStockColorStoreEligibilityHash = new System.Collections.Hashtable();
                }
                return _allocationStockColorStoreEligibilityHash;
            }
            set
            {
                _allocationStockColorStoreEligibilityHash = value;
            }
        }

        private Hashtable AllocationSalesColorStoreEligibilityHash
        {
            get
            {
                if (_allocationSalesColorStoreEligibilityHash == null)
                {
                    _allocationSalesColorStoreEligibilityHash = new System.Collections.Hashtable();
                }
                return _allocationSalesColorStoreEligibilityHash;
            }
            set
            {
                _allocationSalesColorStoreEligibilityHash = value;
            }
        }

        private Hashtable PlanningStockColorStoreEligibilityHash
        {
            get
            {
                if (_planningStockColorStoreEligibilityHash == null)
                {
                    _planningStockColorStoreEligibilityHash = new System.Collections.Hashtable();
                }
                return _planningStockColorStoreEligibilityHash;
            }
            set
            {
                _planningStockColorStoreEligibilityHash = value;
            }
        }

        private Hashtable PlanningSalesColorStoreEligibilityHash
        {
            get
            {
                if (_planningSalesColorStoreEligibilityHash == null)
                {
                    _planningSalesColorStoreEligibilityHash = new System.Collections.Hashtable();
                }
                return _planningSalesColorStoreEligibilityHash;
            }
            set
            {
                _planningSalesColorStoreEligibilityHash = value;
            }
        }

        private Hashtable AllocationStockPackStoreEligibilityHash
        {
            get
            {
                if (_allocationStockPackStoreEligibilityHash == null)
                {
                    _allocationStockPackStoreEligibilityHash = new System.Collections.Hashtable();
                }
                return _allocationStockPackStoreEligibilityHash;
            }
            set
            {
                _allocationStockPackStoreEligibilityHash = value;
            }
        }

        private Hashtable AllocationSalesPackStoreEligibilityHash
        {
            get
            {
                if (_allocationSalesPackStoreEligibilityHash == null)
                {
                    _allocationSalesPackStoreEligibilityHash = new System.Collections.Hashtable();
                }
                return _allocationSalesPackStoreEligibilityHash;
            }
            set
            {
                _allocationSalesPackStoreEligibilityHash = value;
            }
        }

        private Hashtable PlanningStockPackStoreEligibilityHash
        {
            get
            {
                if (_planningStockPackStoreEligibilityHash == null)
                {
                    _planningStockPackStoreEligibilityHash = new System.Collections.Hashtable();
                }
                return _planningStockPackStoreEligibilityHash;
            }
            set
            {
                _planningStockPackStoreEligibilityHash = value;
            }
        }

        private Hashtable PlanningSalesPackStoreEligibilityHash
        {
            get
            {
                if (_planningSalesPackStoreEligibilityHash == null)
                {
                    _planningSalesPackStoreEligibilityHash = new System.Collections.Hashtable();
                }
                return _planningSalesPackStoreEligibilityHash;
            }
            set
            {
                _planningSalesPackStoreEligibilityHash = value;
            }
        }

        private Hashtable SalesEligibilityModelHash 
		{
			get 
			{ 
				if (_salesEligibilityModelHash == null)
				{
					_salesEligibilityModelHash = new System.Collections.Hashtable();
				}
				return _salesEligibilityModelHash ; 
			}
			set 
			{ 
				_salesEligibilityModelHash = value; 
			}
		}

		private Hashtable StockEligibilityModelHash 
		{
			get 
			{ 
				if (_stockEligibilityModelHash == null)
				{
					_stockEligibilityModelHash = new System.Collections.Hashtable();
				}
				return _stockEligibilityModelHash ; 
			}
			set 
			{ 
				_stockEligibilityModelHash = value; 
			}
		}

		// BEGIN MID Track #4370 - John Smith - FWOS Models
		private Hashtable FWOSEligibilityModelHash 
		{
			get 
			{ 
				if (_FWOSEligibilityModelHash == null)
				{
					_FWOSEligibilityModelHash = new System.Collections.Hashtable();
				}
				return _FWOSEligibilityModelHash ; 
			}
			set 
			{ 
				_FWOSEligibilityModelHash = value; 
			}
		}
		// END MID Track #4370

		private Hashtable PriorityShippingModelHash 
		{
			get 
			{ 
				if (_priorityShippingModelHash == null)
				{
					_priorityShippingModelHash = new System.Collections.Hashtable();
				}
				return _priorityShippingModelHash ; 
			}
			set 
			{ 
				_priorityShippingModelHash = value; 
			}
		}

		private Hashtable SalesModifierModelHash 
		{
			get 
			{ 
				if (_salesModifierModelHash == null)
				{
					_salesModifierModelHash = new System.Collections.Hashtable();
				}
				return _salesModifierModelHash ; 
			}
			set 
			{ 
				_salesModifierModelHash = value; 
			}
		}

		private Hashtable StockModifierModelHash 
		{
			get 
			{ 
				if (_stockModifierModelHash == null)
				{
					_stockModifierModelHash = new System.Collections.Hashtable();
				}
				return _stockModifierModelHash ; 
			}
			set 
			{ 
				_stockModifierModelHash = value; 
			}
		}

		// BEGIN MID Track #4370 - John Smith - FWOS Models
		private Hashtable FWOSModifierModelHash 
		{
			get 
			{ 
				if (_FWOSModifierModelHash == null)
				{
					_FWOSModifierModelHash = new System.Collections.Hashtable();
				}
				return _FWOSModifierModelHash ; 
			}
			set 
			{ 
				_FWOSModifierModelHash = value; 
			}
		}
		// END MID Track #4370

		private Hashtable SalesEligibilityModelDateHash 
		{
			get 
			{ 
				// begin MID Track 4607 and 4626 Eligibility and Priority Shipping not working
				// "null" value has meaning; when null, the logic is triggered to build this hash table
				//if (_salesEligibilityModelDateHash == null)
				//{
				//	_salesEligibilityModelDateHash = new System.Collections.Hashtable();
				//}
				// end MID Track 4607 and 4626 Eligibility and Priority Shipping not working
				return _salesEligibilityModelDateHash ; 
			}
			set 
			{ 
				_salesEligibilityModelDateHash = value; 
			}
		}

		private Hashtable StockEligibilityModelDateHash 
		{
			get 
			{ 
				// begin MID Track 4607 and 4626 Eligibility and Priority Shipping not working
				// "null" value has meaning; when null, the logic is triggered to build this hash table
				//if (_stockEligibilityModelDateHash == null)
				//{
				//	_stockEligibilityModelDateHash = new System.Collections.Hashtable();
				//}
				// end MID Track 4607 and 4626 Eligibility and Priority Shipping not working
				return _stockEligibilityModelDateHash ; 
			}
			set 
			{ 
				_stockEligibilityModelDateHash = value; 
			}
		}

		// BEGIN MID Track #4370 - John Smith - FWOS Models
		private Hashtable FWOSEligibilityModelDateHash 
		{
			get 
			{ 
				if (_FWOSEligibilityModelDateHash == null)
				{
					_FWOSEligibilityModelDateHash = new System.Collections.Hashtable();
				}
				return _FWOSEligibilityModelDateHash ; 
			}
			set 
			{ 
				_FWOSEligibilityModelDateHash = value; 
			}
		}
		// END MID Track #4370

		private Hashtable PriorityShippingModelDateHash 
		{
			get 
			{ 
				// begin MID Track 4607 and 4626 Eligibility and Priority Shipping not working
				// "null" value has meaning; when null, the logic is triggered to build this hash table
				//if (_priorityShippingModelDateHash == null)
				//{
				//	_priorityShippingModelDateHash = new System.Collections.Hashtable();
				//}
				// end MID Track 4607 and 4626 Eligibility and Priority Shipping not working
				return _priorityShippingModelDateHash ; 
			}
			set 
			{ 
				_priorityShippingModelDateHash = value; 
			}
		}

		private Hashtable SalesModifierModelDateHash 
		{
			get 
			{ 
				if (_salesModifierModelDateHash == null)
				{
					_salesModifierModelDateHash = new System.Collections.Hashtable();
				}
				return _salesModifierModelDateHash ; 
			}
			set 
			{ 
				_salesModifierModelDateHash = value; 
			}
		}

		private Hashtable StockModifierModelDateHash 
		{
			get 
			{ 
				if (_stockModifierModelDateHash == null)
				{
					_stockModifierModelDateHash = new System.Collections.Hashtable();
				}
				return _stockModifierModelDateHash ; 
			}
			set 
			{ 
				_stockModifierModelDateHash = value; 
			}
		}
		
		// BEGIN MID Track #4370 - John Smith - FWOS Models
		private Hashtable FWOSModifierModelDateHash 
		{
			get 
			{ 
				if (_FWOSModifierModelDateHash == null)
				{
					_FWOSModifierModelDateHash = new System.Collections.Hashtable();
				}
				return _FWOSModifierModelDateHash ; 
			}
			set 
			{ 
				_FWOSModifierModelDateHash = value; 
			}
		}
		// END MID Track #4370

        // Begin TT#4988 - JSmith - Performance
        //private Hashtable StoreSalesStatusHash
        //{
        //    get
        //    {
        //        if (_storeSalesStatusHash == null)
        //        {
        //            _storeSalesStatusHash = new System.Collections.Hashtable();
        //        }
        //        return _storeSalesStatusHash;
        //    }
        //    set
        //    {
        //        _storeSalesStatusHash = value;
        //    }
        //}
        private Dictionary<int, Dictionary<int, eStoreStatus>> StoreSalesStatusHash 
		{
			get 
			{ 
				if (_storeSalesStatusHash == null)
				{
					_storeSalesStatusHash = new Dictionary<int, Dictionary<int, eStoreStatus>>();
				}
				return _storeSalesStatusHash ; 
			}
			set 
			{ 
				_storeSalesStatusHash = value; 
			}
		}

        //private Hashtable StoreStockStatusHash
        //{
        //    get
        //    {
        //        if (_storeStockStatusHash == null)
        //        {
        //            _storeStockStatusHash = new System.Collections.Hashtable();
        //        }
        //        return _storeStockStatusHash;
        //    }
        //    set
        //    {
        //        _storeStockStatusHash = value;
        //    }
        //}
        private Dictionary<int, Dictionary<int, eStoreStatus>> StoreStockStatusHash 
		{
			get 
			{ 
				if (_storeStockStatusHash == null)
				{
					_storeStockStatusHash = new Dictionary<int, Dictionary<int, eStoreStatus>>();
				}
				return _storeStockStatusHash ; 
			}
			set 
			{ 
				_storeStockStatusHash = value; 
			}
		}
        // End TT#4988 - JSmith - Performance

        private Dictionary<int, HierarchyNodeProfile> NodeHash
        {
            get
            {
                if (_nodeHash == null)
                {
                    _nodeHash = new Dictionary<int, HierarchyNodeProfile>();
                }
                return _nodeHash;
            }
            set
            {
                _nodeHash = value;
            }
        }

        private Dictionary<string, int> StoreHash
        {
            get
            {
                if (_storeHash == null)
                {
                    _storeHash = new Dictionary<string, int>();
                }
                return _storeHash;
            }
            set
            {
                _storeHash = value;
            }
        }

		//========
		// METHODS
		//========

		/// <summary>
		/// This method will retrieve the current ProfileList stored in this transaction.  If the ProfileList has not yet been created, the
		/// values will be retrieved from other Sessions, if necessary.  If the information is not available in other sessions, an error will
		/// be thrown.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType of the ProfileList to retieve.
		/// </param>
		/// <returns>
		/// The ProfileList object for the given eProfileType.
		/// </returns>

		public ProfileList GetProfileList(eProfileType aProfileType)
		{
			try
			{
				ProfileList profileList;

                // begin MID Track 4607 and 4626 Eligibility and Priority SHipping not working
				//profileList = (ProfileList)_profileHash[aProfileType];
				profileList = (ProfileList)ProfileHash[aProfileType];
                // end MID Track 4607 and 4626 Eligibility and Priority SHipping not working


				if (profileList == null)
				{
					switch (aProfileType)
					{
						case eProfileType.Store:

							profileList = SAB.HierarchyServerSession.GetProfileList(aProfileType);

                            // begin MID Track 4607 and 4626 Eligibility and Priority SHipping not working
							//_profileHash.Add(profileList.ProfileType, profileList);
							ProfileHash.Add(profileList.ProfileType, profileList);
                            // end MID Track 4607 and 4626 Eligibility and Priority SHipping not working

							_maxStoreRID = profileList.MaxValue;

							break;

						default:

							profileList = SAB.HierarchyServerSession.GetProfileList(aProfileType);

                            // begin MID Track 4607 and 4626 Eligibility and Priority SHipping not working
							//_profileHash.Add(profileList.ProfileType, profileList);
							ProfileHash.Add(profileList.ProfileType, profileList);
                            // end MID Track 4607 and 4626 Eligibility and Priority SHipping not working

							break;
					}
				}

				return profileList;
			}
			catch
			{
				throw;
			}

		}

        // Begin TT#4988 - JSmith - Performance
        //private eStoreStatus GetStoreSalesStatus(int aStoreRID, int aFirstDayOfWeek)
        //{
        //    try
        //    {
        //        Hashtable yearWeekHash;
        //        // begin MID Track 4607 and 4626 Eligibility and Priority SHipping not working
        //        //yearWeekHash = (Hashtable) _storeSalesStatusHash[aFirstDayOfWeek];
        //        yearWeekHash = (Hashtable)StoreSalesStatusHash[aFirstDayOfWeek];
        //        // end MID Track 4607 and 4626 Eligibility and Priority SHipping not working
        //        if (yearWeekHash == null)
        //        {
        //            yearWeekHash = SAB.StoreServerSession.GetStoreSalesStatusHash(aFirstDayOfWeek);
        //            // begin MID Track 4607 and 4626 Eligibility and Priority SHipping not working
        //            //_storeSalesStatusHash.Add(aFirstDayOfWeek, yearWeekHash);
        //            StoreSalesStatusHash.Add(aFirstDayOfWeek, yearWeekHash);
        //            // end MID Track 4607 and 4626 Eligibility and Priority SHipping not working
        //        }
        //        return (eStoreStatus)yearWeekHash[aStoreRID];
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
		private eStoreStatus GetStoreSalesStatus(int aStoreRID, int aFirstDayOfWeek)
		{
			try
			{
                
                Dictionary<int, eStoreStatus> yearWeekHash;
                if (!StoreSalesStatusHash.TryGetValue(aFirstDayOfWeek, out yearWeekHash))
                {
                    yearWeekHash = SAB.HierarchyServerSession.GetStoreSalesStatusHash(aFirstDayOfWeek);
                    StoreSalesStatusHash.Add(aFirstDayOfWeek, yearWeekHash);
				}
				return (eStoreStatus)yearWeekHash[aStoreRID];
			}
			catch
			{
				throw;
			}
		}

        //private eStoreStatus GetStoreStockStatus(int aStoreRID, int aFirstDayOfWeek)
        //{
        //    try
        //    {
        //        Hashtable yearWeekHash;
        //        // begin MID Track 4607 and 4626 Eligibility and Priority SHipping not working
        //        //yearWeekHash = (Hashtable) _storeStockStatusHash[aFirstDayOfWeek];
        //        yearWeekHash = (Hashtable)StoreStockStatusHash[aFirstDayOfWeek];
        //        // end MID Track 4607 and 4626 Eligibility and Priority SHipping not working
        //        if (yearWeekHash == null)
        //        {
        //            yearWeekHash = SAB.StoreServerSession.GetStoreStockStatusHash(aFirstDayOfWeek);
        //            // begin MID Track 4607 and 4626 Eligibility and Priority SHipping not working
        //            //_storeStockStatusHash.Add(aFirstDayOfWeek, yearWeekHash);
        //            StoreStockStatusHash.Add(aFirstDayOfWeek, yearWeekHash);
        //            // end MID Track 4607 and 4626 Eligibility and Priority SHipping not working
        //        }
        //        return (eStoreStatus)yearWeekHash[aStoreRID];
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
		private eStoreStatus GetStoreStockStatus(int aStoreRID, int aFirstDayOfWeek)
		{
			try
			{
                Dictionary<int, eStoreStatus> yearWeekHash;
                if (!StoreStockStatusHash.TryGetValue(aFirstDayOfWeek, out yearWeekHash))
                {
                	yearWeekHash = SAB.HierarchyServerSession.GetStoreStockStatusHash(aFirstDayOfWeek);
                    StoreStockStatusHash.Add(aFirstDayOfWeek, yearWeekHash);
				}
				return (eStoreStatus)yearWeekHash[aStoreRID];
			}
			catch
			{
				throw;
			}
		}
        // End TT#4988 - JSmith - Performance

		/// <summary>
		/// Requests the transaction get the store eligibility settings for all stores for a node and range of weekw.
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aFirstDayOfBeginWeek">The first day of the Begin year/week for which eligibility is to be determined</param>
		/// <param name="aFirstDayOfEndWeek">The first day of the End year/week for which eligibility is to be determined</param>
		/// <returns>BitArray containing eligibility settings indexed by storeRID</returns>
		public System.Collections.BitArray GetStoreSalesEligibilityFlags(
            eRequestingApplication requestingApplication, 
            int aNodeRID, 
            int aFirstDayOfBeginWeek, 
            int aFirstDayOfEndWeek
            )
		{
			try
			{
				WeekProfile beginWeekProfile = this.SAB.HierarchyServerSession.Calendar.GetWeek(aFirstDayOfBeginWeek);
				WeekProfile endWeekProfile = this.SAB.HierarchyServerSession.Calendar.GetWeek(aFirstDayOfEndWeek);
				ProfileList weekList = this.SAB.HierarchyServerSession.Calendar.GetWeekRange(beginWeekProfile, endWeekProfile);
				System.Collections.BitArray rangeEligibilityBitArray = 
					this.GetStoreSalesEligibilityFlags(
                        requestingApplication,
                        aNodeRID, 
                        ((WeekProfile)weekList[0]).Key
                        );
				System.Collections.BitArray weekEligibilityBitArray;
				for (int i=1; i<weekList.Count; i++)
				{
					weekEligibilityBitArray = 
						this.GetStoreSalesEligibilityFlags(
                            requestingApplication,
                            aNodeRID, 
                            ((WeekProfile)weekList[i]).Key
                            );
					for (int j=1; j<rangeEligibilityBitArray.Count; j++)
					{
						if (weekEligibilityBitArray[j] == true)
						{
                           rangeEligibilityBitArray[j] = true;
						}
					}
				}

				return rangeEligibilityBitArray;
			}
			catch
			{
				throw;
			} 
		}

		/// <summary>
		/// Requests the transaction get the store eligibility settings for all stores for a node and week.
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aFirstDayOfWeek">The first day of year/week for which eligibility is to be determined</param>
		/// <returns>BitArray containing eligibility settings indexed by storeRID</returns>
		public System.Collections.BitArray GetStoreSalesEligibilityFlags(
            eRequestingApplication requestingApplication, 
            int aNodeRID, 
            int aFirstDayOfWeek
            )
		{
			try
			{
				StoreEligibilityList sel = null;
				ProfileList storeList = GetProfileList(eProfileType.Store);
				StoreEligibilityProfile sep = null;
				System.Collections.BitArray eligibilityBitArray = new System.Collections.BitArray(_maxStoreRID + 1,true);	// default to eligible
				// get date information to check against open and close dates
				WeekProfile weekProfile = this.SAB.HierarchyServerSession.Calendar.GetWeek(aFirstDayOfWeek);
				DayProfile dayProfile = (DayProfile)weekProfile.Days[0];
				DateTime firstDayOfWeek = dayProfile.Date;
				dayProfile = (DayProfile)weekProfile.Days[weekProfile.DaysInWeek - 1];
				DateTime lastDayOfWeek = dayProfile.Date;

                bool useExternalEligibility = false;
                Hashtable storeEligibilityHash;
                // select appropriate cache based on settings
                if (requestingApplication == eRequestingApplication.Forecast)
                {
                    useExternalEligibility = GlobalOptions.UseExternalEligibilityPlanning;
                    if (useExternalEligibility)
                    {
                        storeEligibilityHash = PlanningSalesStoreEligibilityHash;
                    }
                    else
                    {
                        storeEligibilityHash = StoreEligibilityHash;
                    }
                }
                else
                {
                    useExternalEligibility = GlobalOptions.UseExternalEligibilityAllocation;
                    if (useExternalEligibility)
                    {
                        storeEligibilityHash = AllocationSalesStoreEligibilityHash;
                    }
                    else
                    {
                        storeEligibilityHash = StoreEligibilityHash;
                    }
                }

                sel = (StoreEligibilityList)storeEligibilityHash[aNodeRID];

				if (sel == null)
				{
                    if (useExternalEligibility)
                    {
                        sel = CallExternalEligibility(
                             requestingApplication: requestingApplication,
                             variable: cSalesVariable,
                             merchandiseKey: aNodeRID,
                             packName: null,
                             yearWeek: weekProfile.YearWeek,
                             storeList: storeList
                            );
                    }
                    else
                    {
                        sel = SAB.HierarchyServerSession.GetStoreEligibilityList(storeList, aNodeRID, true, false);
                    }
                    storeEligibilityHash.Add(aNodeRID, sel);
				}

				foreach(StoreProfile storeProfile in storeList)
				{
					eStoreStatus storeStatus = GetStoreSalesStatus(storeProfile.Key, aFirstDayOfWeek); 
					if (storeStatus == eStoreStatus.Closed ||
						storeProfile.SellingOpenDt > lastDayOfWeek ||
						(storeProfile.SellingCloseDt != Include.UndefinedDate &&
						storeProfile.SellingCloseDt < firstDayOfWeek))
					{
						eligibilityBitArray[storeProfile.Key] = false;
					}
					else
					{
						sep = (StoreEligibilityProfile) sel.FindKey(storeProfile.Key);
						if (sep == null)	// if eligibility not set for store
						{
							if (storeStatus == eStoreStatus.Preopen)
							{
								eligibilityBitArray[storeProfile.Key] = false;
							}
						}
						else
						if (sep.StoreIneligible)
						{
							eligibilityBitArray[storeProfile.Key] = false;
						}
						else
						if (sep.EligModelRID != Include.NoRID)		// check against model dates
						{
							if (sep.EligModelRID != _currentSalesEligibilityModelRID)
							{
                                // Begin TT#2307 - JSmith - Incorrect Stock Values
                                //SalesEligibilityModelDateHash = (Hashtable)SalesEligibilityModelHash[sep.EligModelRID];
                                //if (SalesEligibilityModelDateHash == null)
                                //{
                                //    LoadEligibilityModel(sep.EligModelRID);
                                //    _currentStockEligibilityModelRID = sep.EligModelRID;	// set stock model since both are loaded
                                //}

                                if (!SalesEligibilityModelHash.ContainsKey(sep.EligModelRID))
								{
									LoadEligibilityModel(sep.EligModelRID);
									_currentStockEligibilityModelRID = sep.EligModelRID;	// set stock model since both are loaded
                                    _currentPriorityShippingModelRID = sep.EligModelRID;    // set priority shipping since loaded
								}

                                _salesEligibilityModifierModel = (ModifierModel)SalesEligibilityModelHash[sep.EligModelRID];

                                if (!_salesEligibilityModifierModel.ContainsValuesByStore)
                                {
                                    SalesEligibilityModelDateHash = _salesEligibilityModifierModel.ModelDateHash;
                                }
                                // End TT#2307
								_currentSalesEligibilityModelRID = sep.EligModelRID;
							}

                            // Begin TT#2307 - JSmith - Incorrect Stock Values
                            if (_salesEligibilityModifierModel.ContainsValuesByStore)
                            {
                                SalesEligibilityModelDateHash = (Hashtable)_salesEligibilityModifierModel.ModelDateHash[sep.Key];
                                if (SalesEligibilityModelDateHash == null)
                                {
                                    SalesEligibilityModelDateHash = new Hashtable();
                                }
                            }
                            // End TT#2307

                            if (SalesEligibilityModelDateHash.Count == 0)		// eligible if no dates
							{
								if (storeStatus == eStoreStatus.Preopen)
								{
									eligibilityBitArray[storeProfile.Key] = false;
								}
								else
								{
									eligibilityBitArray[storeProfile.Key] = true;
								}
							}
							else
							{
								if (SalesEligibilityModelDateHash.Contains(aFirstDayOfWeek))		
								{
									eligibilityBitArray[storeProfile.Key] = true;
								}
								else
								{	// check for reoccurring
                                    int weekInYear = weekProfile.WeekInYear;
                                    if (SalesEligibilityModelDateHash.Contains(weekInYear))
									{
										eligibilityBitArray[storeProfile.Key] = true;
									}
									else
									{
										eligibilityBitArray[storeProfile.Key] = false;
									}
								}
							}
						}
					}
				}
				return eligibilityBitArray;
			}
			catch
			{
				throw;
			} 
		}

		/// <summary>
		/// Requests the transaction get the store eligibility settings for all stores for a node and week.
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aFirstDayOfWeek">The first day of year/week for which eligibility is to be determined</param>
		/// <returns>BitArray containing eligibility settings indexed by storeRID</returns>
		public System.Collections.BitArray GetStoreStockEligibilityFlags(
            eRequestingApplication requestingApplication,
            int aNodeRID, 
            int aFirstDayOfWeek
            )
		{
			try
			{
				StoreEligibilityList sel = null;
				ProfileList storeList = GetProfileList(eProfileType.Store);
				StoreEligibilityProfile sep = null;
				System.Collections.BitArray eligibilityBitArray = new System.Collections.BitArray(_maxStoreRID + 1,true);	// default to eligible
				// get date information to check against open and close dates
				WeekProfile weekProfile = this.SAB.HierarchyServerSession.Calendar.GetWeek(aFirstDayOfWeek);
				DayProfile dayProfile = (DayProfile)weekProfile.Days[0];
				DateTime firstDayOfWeek = dayProfile.Date;
				dayProfile = (DayProfile)weekProfile.Days[weekProfile.DaysInWeek - 1];
				DateTime lastDayOfWeek = dayProfile.Date;

                Hashtable storeEligibilityHash;
                // select appropriate cache based on settings
                if (requestingApplication == eRequestingApplication.Forecast)
                {
                    if (GlobalOptions.UseExternalEligibilityPlanning)
                    {
                        storeEligibilityHash = PlanningStockStoreEligibilityHash;
                    }
                    else
                    {
                        storeEligibilityHash = StoreEligibilityHash;
                    }
                }
                else
                {
                    if (GlobalOptions.UseExternalEligibilityAllocation)
                    {
                        storeEligibilityHash = AllocationStockStoreEligibilityHash;
                    }
                    else
                    {
                        storeEligibilityHash = StoreEligibilityHash;
                    }
                }

                sel = (StoreEligibilityList)storeEligibilityHash[aNodeRID];

				if (sel == null)
				{
					sel = SAB.HierarchyServerSession.GetStoreEligibilityList(storeList, aNodeRID, true, false);
                    storeEligibilityHash.Add(aNodeRID, sel);
				}

				foreach(StoreProfile storeProfile in storeList)
				{
					eStoreStatus storeStatus = GetStoreStockStatus(storeProfile.Key, aFirstDayOfWeek);
					if (storeStatus == eStoreStatus.Closed ||
						storeProfile.StockOpenDt > lastDayOfWeek ||
						(storeProfile.StockCloseDt != Include.UndefinedDate &&
						storeProfile.StockCloseDt < firstDayOfWeek))
					{
						eligibilityBitArray[storeProfile.Key] = false;
					}
					else
					{
						sep = (StoreEligibilityProfile) sel.FindKey(storeProfile.Key);
						if (sep == null)	// if eligibility not defined for the store
						{
							if (storeStatus == eStoreStatus.Preopen)
							{
								eligibilityBitArray[storeProfile.Key] = false;
							}
						}
						else
							if (sep.StoreIneligible)
						{
							eligibilityBitArray[storeProfile.Key] = false;
						}
						else
						{
							if (sep.EligModelRID != Include.NoRID)		// check against model dates
							{
								if (sep.EligModelRID != _currentStockEligibilityModelRID)
                                {
                                    // Begin TT#2307 - JSmith - Incorrect Stock Values
                                    //StockEligibilityModelDateHash = (Hashtable)StockEligibilityModelHash[sep.EligModelRID];
                                    //if (StockEligibilityModelDateHash == null)
                                    //{
                                    //    LoadEligibilityModel(sep.EligModelRID);
                                    //    _currentSalesEligibilityModelRID = sep.EligModelRID;	// set sales model since all are loaded
                                    //}

                                    if (!StockEligibilityModelHash.ContainsKey(sep.EligModelRID))
                                    {
                                        LoadEligibilityModel(sep.EligModelRID);
                                        _currentSalesEligibilityModelRID = sep.EligModelRID;	// set sales model since both are loaded
                                        _currentPriorityShippingModelRID = sep.EligModelRID;    // set priority shipping since loaded
                                    }

                                    _stockEligibilityModifierModel = (ModifierModel)StockEligibilityModelHash[sep.EligModelRID];

                                    if (!_stockEligibilityModifierModel.ContainsValuesByStore)
                                    {
                                        StockEligibilityModelDateHash = _stockEligibilityModifierModel.ModelDateHash;
                                    }
                                    // End TT#2307
                                    _currentStockEligibilityModelRID = sep.EligModelRID;
                                }

                                // Begin TT#2307 - JSmith - Incorrect Stock Values
                                if (_stockEligibilityModifierModel.ContainsValuesByStore)
                                {
                                    StockEligibilityModelDateHash = (Hashtable)_stockEligibilityModifierModel.ModelDateHash[sep.Key];
                                    if (StockEligibilityModelDateHash == null)
                                    {
                                        StockEligibilityModelDateHash = new Hashtable();
                                    }
                                }
                                // End TT#2307

								if (StockEligibilityModelDateHash.Count == 0)		// eligible if no dates
								{
									if (storeStatus == eStoreStatus.Preopen)
									{
										eligibilityBitArray[storeProfile.Key] = false;
									}
									else
									{
										eligibilityBitArray[storeProfile.Key] = true;
									}
								}
								else
								{
									if (StockEligibilityModelDateHash.Contains(aFirstDayOfWeek))
									{
										eligibilityBitArray[storeProfile.Key] = true;
									}
									else
									{	// check for reoccurring
                                        int weekInYear = weekProfile.WeekInYear;
                                        if (StockEligibilityModelDateHash.Contains(weekInYear))
										{
											eligibilityBitArray[storeProfile.Key] = true;
										}
										else
										{
											eligibilityBitArray[storeProfile.Key] = false;
										}
									}
								}
							}
						}
					}
				}
				return eligibilityBitArray;
			}
			catch
			{
				throw;
			} 
		}

		/// <summary>
		/// Requests the transaction get the priority shipping settings for all stores for a node and week.
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aFirstDayOfWeek">The first day of year/week for which priority shipping is to be determined</param>
		/// <returns>BitArray containing priority shipping settings indexed by storeRID</returns>
		public System.Collections.BitArray GetStorePriorityShippingFlags(int aNodeRID, int aFirstDayOfWeek)
		{
			try
			{
				StoreEligibilityList sel = null;
				StoreEligibilityProfile sep = null;
				ProfileList storeList = GetProfileList(eProfileType.Store);
				System.Collections.BitArray priorityShippingBitArray = new System.Collections.BitArray(_maxStoreRID + 1,false);	// default to not priority shipper
				// get date information to check against open and close dates
				WeekProfile weekProfile = this.SAB.HierarchyServerSession.Calendar.GetWeek(aFirstDayOfWeek);
                sel = (StoreEligibilityList)StoreEligibilityHash[aNodeRID];
                
				if (sel == null)
				{
					sel = SAB.HierarchyServerSession.GetStoreEligibilityList(storeList, aNodeRID, true, false);
                    StoreEligibilityHash.Add(aNodeRID, sel);
				}

				foreach(StoreProfile storeProfile in storeList)
				{
					eStoreStatus storeStatus = GetStoreStockStatus(storeProfile.Key, aFirstDayOfWeek);
					if (storeStatus == eStoreStatus.Closed)
					{
						priorityShippingBitArray[storeProfile.Key] = false;
					}
					else
					{
						sep = (StoreEligibilityProfile) sel.FindKey(storeProfile.Key);
						if (sep == null)
						{
							priorityShippingBitArray[storeProfile.Key] = false;
						}
						else
							if (sep.StoreIneligible)
						{
							priorityShippingBitArray[storeProfile.Key] = false;
						}
						else
						{
							if (sep.EligModelRID != Include.NoRID)		// check against model dates
							{
								if (sep.EligModelRID != _currentPriorityShippingModelRID)
                                {
                                    // Begin TT#2307 - JSmith - Incorrect Stock Values
                                    //PriorityShippingModelDateHash = (Hashtable)PriorityShippingModelHash[sep.EligModelRID];
                                    //if (PriorityShippingModelDateHash == null)
                                    //{
                                    //    LoadEligibilityModel(sep.EligModelRID);
                                    //    _currentSalesEligibilityModelRID = sep.EligModelRID;	// set sales model since all are loaded
                                    //    _currentStockEligibilityModelRID = sep.EligModelRID;	// set stock model since all are loaded
                                    //}

                                    // Begin TT#4132 - JSmith - Priority Ship not working correctly
                                    //if (!PriorityShippingModelDateHash.ContainsKey(sep.EligModelRID))
									if (!PriorityShippingModelHash.ContainsKey(sep.EligModelRID))
                                    // End TT#4132 - JSmith - Priority Ship not working correctly
                                    {
                                        LoadEligibilityModel(sep.EligModelRID);
                                        _currentSalesEligibilityModelRID = sep.EligModelRID;	// set sales model since all are loaded
                                        _currentStockEligibilityModelRID = sep.EligModelRID;	// set stock model since all are loaded
                                    }

                                    // Begin TT#242-MD - JSmith - Receive an invalid cast exception with header type of work up buy>ran allocation override with minimums>ran the rule method with allocation minimum rule.
                                    //_priorityShippingModifierModel = (ModifierModel)PriorityShippingModelDateHash[sep.EligModelRID];
                                    _priorityShippingModifierModel = (ModifierModel)PriorityShippingModelHash[sep.EligModelRID];
                                    // End TT#242-MD - JSmith - Receive an invalid cast exception with header type of work up buy>ran allocation override with minimums>ran the rule method with allocation minimum rule.

                                    if (!_priorityShippingModifierModel.ContainsValuesByStore)
                                    {
                                        PriorityShippingModelDateHash = _priorityShippingModifierModel.ModelDateHash;
                                    }
                                    // End TT#2307
                                    // Begin TT#4132 - JSmith - Priority Ship not working correctly
                                    //_currentStockEligibilityModelRID = sep.EligModelRID;
                                    _currentPriorityShippingModelRID = sep.EligModelRID;
                                    // End TT#4132 - JSmith - Priority Ship not working correctly
                                }

                                // Begin TT#2307 - JSmith - Incorrect Stock Values
                                if (_priorityShippingModifierModel.ContainsValuesByStore)
                                {
                                    PriorityShippingModelDateHash = (Hashtable)_priorityShippingModifierModel.ModelDateHash[sep.Key];
                                    if (PriorityShippingModelDateHash == null)
                                    {
                                        PriorityShippingModelDateHash = new Hashtable();
                                    }
                                }
                                // End TT#2307

								if (PriorityShippingModelDateHash.Count == 0)		// not priority shipper if no dates
								{
									priorityShippingBitArray[storeProfile.Key] = false;
								}
								else
								{
									if (PriorityShippingModelDateHash.Contains(aFirstDayOfWeek))
									{
										priorityShippingBitArray[storeProfile.Key] = true;
									}
									else
									{	// check for reoccurring
                                        int weekInYear = weekProfile.WeekInYear;
                                        if (PriorityShippingModelDateHash.Contains(weekInYear))
										{
											priorityShippingBitArray[storeProfile.Key] = true;
										}
										else
										{
											priorityShippingBitArray[storeProfile.Key] = false;
										}
									}
								}
							}
						}
					}
				}
				return priorityShippingBitArray;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the transaction load date information associated with an eligibility model.
		/// </summary>
		/// <param name="aModelRID">The record id of the eligibility model</param>
		/// <remarks>
		/// This method loads all data for the model.  It loads sales eligibility, stock eligibility 
		/// and priority shipping information.
		/// </remarks>
		private void LoadEligibilityModel(int aModelRID)
		{
			try
			{
                if (SalesEligibilityModelHash.Contains(aModelRID))
				{
					SalesEligibilityModelHash.Remove(aModelRID);
				}
                
				
                // Begin TT#2307 - JSmith - Incorrect Stock Values
                //EligModelProfile emp = SAB.HierarchyServerSession.GetEligModelData(aModelRID);
                EligModelProfile emp = SAB.HierarchyServerSession.GetEligModelData(aModelRID, GetProfileList(eProfileType.Store));
                // End TT#2307

				// build sales eligibility dates
                // Begin TT#2307 - JSmith - Incorrect Stock Values
                //if (!emp.NeedsRebuilt)
				if (!emp.SalesEligibilityNeedsRebuilt)
                // End TT#2307
				{
                    SalesEligibilityModelDateHash = (Hashtable) emp.SalesEligibilityModelDateEntries.Clone();
				}
				else
				{
                    SalesEligibilityModelDateHash = new Hashtable();
                    foreach(EligModelEntry eme in emp.SalesEligibilityEntries)
					{
						ProfileList weekProfileList = SAB.HierarchyServerSession.Calendar.GetWeekRange(eme.DateRange, null);
						if (eme.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
                                if (!SalesEligibilityModelDateHash.Contains(weekProfile.WeekInYear))
								{
									SalesEligibilityModelDateHash.Add(weekProfile.WeekInYear, weekProfile);
								}
							}
						}
						else
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
                                if (!SalesEligibilityModelDateHash.Contains(weekProfile.Key))
								{
									SalesEligibilityModelDateHash.Add(weekProfile.Key, weekProfile);
								}
							}
						}
					}
				}

                // Begin TT#2307 - JSmith - Incorrect Stock Values
                //SalesEligibilityModelHash.Add(aModelRID, SalesEligibilityModelDateHash);
                _salesEligibilityModifierModel = new ModifierModel(0, SalesEligibilityModelDateHash, emp.SalesEligibilityContainsStoreDynamicDates);
                SalesEligibilityModelHash.Add(aModelRID, _salesEligibilityModifierModel);
                // End TT#2307

				// build stock eligibility dates
				if (StockEligibilityModelHash.Contains(aModelRID))
				{
					StockEligibilityModelHash.Remove(aModelRID);
				}
				if (!emp.NeedsRebuilt)
				{
					StockEligibilityModelDateHash = (Hashtable) emp.ModelDateEntries.Clone();
				}
				else
				{
					StockEligibilityModelDateHash = new Hashtable();
					foreach(EligModelEntry eme in emp.ModelEntries)
					{
						ProfileList weekProfileList = SAB.HierarchyServerSession.Calendar.GetWeekRange(eme.DateRange, null);
						foreach (WeekProfile weekProfile in weekProfileList)
						{
							if (eme.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
							{
								if (!StockEligibilityModelDateHash.Contains(weekProfile.WeekInYear))
								{
									StockEligibilityModelDateHash.Add(weekProfile.WeekInYear, weekProfile);
								}
							}
							else
							{
								if (!StockEligibilityModelDateHash.Contains(weekProfile.Key))
								{
									StockEligibilityModelDateHash.Add(weekProfile.Key, weekProfile);
								}
							}
						}
					}
				}
			
				
                // Begin TT#2307 - JSmith - Incorrect Stock Values
                //StockEligibilityModelHash.Add(aModelRID, StockEligibilityModelDateHash);
                _stockEligibilityModifierModel = new ModifierModel(0, StockEligibilityModelDateHash, emp.ContainsStoreDynamicDates);
                StockEligibilityModelHash.Add(aModelRID, _stockEligibilityModifierModel);
                // End TT#2307

				// build priority shipping dates
				if (PriorityShippingModelHash.Contains(aModelRID))
				{
					PriorityShippingModelHash.Remove(aModelRID);
				}
                // Begin TT#2307 - JSmith - Incorrect Stock Values
                //if (!emp.NeedsRebuilt)
				if (!emp.PriorityShippingNeedsRebuilt)
                // End TT#2307
				{
					PriorityShippingModelDateHash = (Hashtable) emp.PriorityShippingModelDateEntries.Clone();
				}
				else
				{
					PriorityShippingModelDateHash = new Hashtable();
					foreach(EligModelEntry eme in emp.PriorityShippingEntries)
					{
						ProfileList weekProfileList = SAB.HierarchyServerSession.Calendar.GetWeekRange(eme.DateRange, null);
						foreach (WeekProfile weekProfile in weekProfileList)
						{
							if (eme.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
							{
								if (!PriorityShippingModelDateHash.Contains(weekProfile.WeekInYear))
								{
									PriorityShippingModelDateHash.Add(weekProfile.WeekInYear, weekProfile);
								}
							}
							else
							{
								if (!PriorityShippingModelDateHash.Contains(weekProfile.Key))
								{
									PriorityShippingModelDateHash.Add(weekProfile.Key, weekProfile);
								}
							}
						}
					}
				}
			
                // Begin TT#2307 - JSmith - Incorrect Stock Values
                //PriorityShippingModelHash.Add(aModelRID, PriorityShippingModelDateHash);
                _priorityShippingModifierModel = new ModifierModel(0, PriorityShippingModelDateHash, emp.PriorityShippingContainsStoreDynamicDates);
                PriorityShippingModelHash.Add(aModelRID, _priorityShippingModifierModel);
                // End TT#2307
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the transaction get the store sales modifier percents for all stores for a node and week.
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aFirstDayOfWeek">The first day of year/week for which sale modifier percents is to be determined</param>
		/// <returns>Hashtable containing sales modifier percents indexed by storeRID</returns>
		public System.Collections.Hashtable GetStoreSalesModifierPercents(int aNodeRID, int aFirstDayOfWeek)
		{
			try
			{
				StoreEligibilityList sel = null;
				ProfileList storeList = GetProfileList(eProfileType.Store);
				System.Collections.Hashtable modifierHash = new System.Collections.Hashtable();

				sel = (StoreEligibilityList)StoreEligibilityHash[aNodeRID];
				if (sel == null)
				{
					sel = SAB.HierarchyServerSession.GetStoreEligibilityList(storeList, aNodeRID, true, false);
					StoreEligibilityHash.Add(aNodeRID, sel);
				}

				foreach(StoreEligibilityProfile sep in sel)
				{
					if (sep.SlsModType == eModifierType.Percent)	// if use specific percent
					{
						modifierHash[sep.Key] = sep.SlsModPct;
					}
					else
					{
						if (sep.SlsModModelRID != Include.NoRID)		// check against model dates
						{
							if (sep.SlsModModelRID != _currentModifierModelRID)
							{
								if (!SalesModifierModelHash.Contains(sep.SlsModModelRID))
								{
									LoadSalesModifierModel(sep.SlsModModelRID);	// loads model information into SalesModifierModelDateHash
								}
								_salesModifierModel = (ModifierModel)SalesModifierModelHash[sep.SlsModModelRID];
                                // Begin TT#2307 - JSmith - Incorrect Stock Values
                                //SalesModifierModelDateHash = _salesModifierModel.ModelDateHash;
                                if (!_salesModifierModel.ContainsValuesByStore)
                                {
                                    SalesModifierModelDateHash = _salesModifierModel.ModelDateHash;
                                }
                                // End TT#2307
								_currentModifierModelRID = sep.SlsModModelRID;
							}

							// Begin TT#2307 - JSmith - Incorrect Stock Values
                            if (_salesModifierModel.ContainsValuesByStore)
                            {
                                SalesModifierModelDateHash = (Hashtable)_salesModifierModel.ModelDateHash[sep.Key];
                            }
                            // End TT#2307

							if (SalesModifierModelDateHash.Contains(aFirstDayOfWeek))
							{
								modifierHash[sep.Key] = SalesModifierModelDateHash[aFirstDayOfWeek];
							}
							else
							{	// check for reoccurring
                                WeekProfile weekProfile = this.SAB.HierarchyServerSession.Calendar.GetWeek(aFirstDayOfWeek);
                                int weekInYear = weekProfile.WeekInYear;
								if (SalesModifierModelDateHash.Contains(weekInYear))
								{
									modifierHash[sep.Key] = SalesModifierModelDateHash[weekInYear];
								}
								else
								{
									modifierHash[sep.Key] = _salesModifierModel.ModelDefault;
								}
							}
						}
					}
				}
				return modifierHash;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the transaction get the store stock modifier percents for all stores for a node and week.
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aFirstDayOfWeek">The first day of year/week for which stock modifier percents is to be determined</param>
		/// <returns>Hashtable containing stock modifier percents indexed by storeRID</returns>
		public System.Collections.Hashtable GetStoreStockModifierPercents(int aNodeRID, int aFirstDayOfWeek)
		{
            try
            {
                StoreEligibilityList sel = null;
                ProfileList storeList = GetProfileList(eProfileType.Store);
                System.Collections.Hashtable modifierHash = new System.Collections.Hashtable();
                sel = (StoreEligibilityList)StoreEligibilityHash[aNodeRID];
                if (sel == null)
                {
                    sel = SAB.HierarchyServerSession.GetStoreEligibilityList(storeList, aNodeRID, true, false);
                    StoreEligibilityHash.Add(aNodeRID, sel);
                }
                foreach (StoreEligibilityProfile sep in sel)
                {
                    if (sep.StkModType == eModifierType.Percent)	// if use specific percent
                    {
                        modifierHash[sep.Key] = sep.StkModPct;
                    }
                    else
                    {
                        if (sep.StkModModelRID != Include.NoRID)		// check against model dates
                        {
                            if (sep.StkModModelRID != _currentStockModifierModelRID)
                            {
                                if (!StockModifierModelHash.Contains(sep.StkModModelRID))
                                {
                                    LoadStockModifierModel(sep.StkModModelRID);
                                }
                                _stockModifierModel = (ModifierModel)StockModifierModelHash[sep.StkModModelRID];
                                // Begin TT#2307 - JSmith - Incorrect Stock Values
                                //StockModifierModelDateHash = _stockModifierModel.ModelDateHash;
                                if (!_stockModifierModel.ContainsValuesByStore)
                                {
                                    StockModifierModelDateHash = _stockModifierModel.ModelDateHash;
                                }
                                // End TT#2307
                                _currentStockModifierModelRID = sep.StkModModelRID;
                            }

                            // Begin TT#2307 - JSmith - Incorrect Stock Values
                            if (_stockModifierModel.ContainsValuesByStore)
                            {
                                StockModifierModelDateHash = (Hashtable)_stockModifierModel.ModelDateHash[sep.Key];
                            }
                            // End TT#2307

                            if (StockModifierModelDateHash.Contains(aFirstDayOfWeek))
                            {
                                modifierHash[sep.Key] = StockModifierModelDateHash[aFirstDayOfWeek];
                            }
                            else
                            {	// check for reoccurring
                                WeekProfile weekProfile = this.SAB.HierarchyServerSession.Calendar.GetWeek(aFirstDayOfWeek);
                                int weekInYear = weekProfile.WeekInYear;
                                if (StockModifierModelDateHash.Contains(weekInYear))
                                {
                                    modifierHash[sep.Key] = StockModifierModelDateHash[weekInYear];
                                }
                                else
                                {
                                    modifierHash[sep.Key] = _stockModifierModel.ModelDefault;
                                }
                            }
                        }
                    }
                }
                return modifierHash;
            }
            catch
            {
                throw;
            }
		}

		/// <summary>
		/// Requests the transaction get the store FWOS modifier percents for all stores for a node and week.
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aFirstDayOfWeek">The first day of year/week for which sale modifier percents is to be determined</param>
		/// <returns>Hashtable containing FWOS modifier percents indexed by storeRID</returns>
		public System.Collections.Hashtable GetStoreFWOSModifierPercents(int aNodeRID, int aFirstDayOfWeek)
		{
			try
			{
				StoreEligibilityList sel = null;
				ProfileList storeList = GetProfileList(eProfileType.Store);
				System.Collections.Hashtable modifierHash = new System.Collections.Hashtable();
				sel = (StoreEligibilityList)StoreEligibilityHash[aNodeRID];
				
				if (sel == null)
				{
					sel = SAB.HierarchyServerSession.GetStoreEligibilityList(storeList, aNodeRID, true, false);
					StoreEligibilityHash.Add(aNodeRID, sel);
				}
				foreach(StoreEligibilityProfile sep in sel)
				{
					if (sep.FWOSModType == eModifierType.Percent)	// if use specific percent
					{
						modifierHash[sep.Key] = sep.FWOSModPct;
					}
					else
						if (sep.FWOSModModelRID != Include.NoRID)		// check against model dates
					{
						if (sep.FWOSModModelRID != _currentFWOSModifierModelRID)
						{
							if (!FWOSModifierModelHash.Contains(sep.FWOSModModelRID))
							{
								LoadFWOSModifierModel(sep.FWOSModModelRID);
							}
							_FWOSModifierModel = (ModifierModel)FWOSModifierModelHash[sep.FWOSModModelRID];
                            // Begin TT#2307 - JSmith - Incorrect Stock Values
                            //FWOSModifierModelDateHash = _FWOSModifierModel.ModelDateHash;
                            if (!_FWOSModifierModel.ContainsValuesByStore)
                            {
                                FWOSModifierModelDateHash = _FWOSModifierModel.ModelDateHash;
                            }
                            // End TT#2307
							_currentFWOSModifierModelRID = sep.FWOSModModelRID;
						}

                        // Begin TT#2307 - JSmith - Incorrect Stock Values
                        if (_FWOSModifierModel.ContainsValuesByStore)
                        {
                            FWOSModifierModelDateHash = (Hashtable)_FWOSModifierModel.ModelDateHash[sep.Key];
                        }
                        // End TT#2307

						if (FWOSModifierModelDateHash.Contains(aFirstDayOfWeek))
						{
							modifierHash[sep.Key] = FWOSModifierModelDateHash[aFirstDayOfWeek];
						}
						else
						{	// check for reoccurring
                            WeekProfile weekProfile = this.SAB.HierarchyServerSession.Calendar.GetWeek(aFirstDayOfWeek);
                            int weekInYear = weekProfile.WeekInYear;
							if (FWOSModifierModelDateHash.Contains(weekInYear))
							{
								modifierHash[sep.Key] = FWOSModifierModelDateHash[weekInYear];
							}
							else
							{
								modifierHash[sep.Key] = _FWOSModifierModel.ModelDefault;
							}
						}
					}
				}
				return modifierHash;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the transaction load date information associated with a sales modifier model.
		/// </summary>
		/// <param name="aModelRID">The record id of the sales modifier model</param>
		private void LoadSalesModifierModel(int aModelRID)
		{
			try
			{
                if (SalesModifierModelHash.Contains(aModelRID))
				{
					SalesModifierModelHash.Remove(aModelRID);
				}

                // Begin TT#2307 - JSmith - Incorrect Stock Values
                //SlsModModelProfile smmp = SAB.HierarchyServerSession.GetSlsModModelData(aModelRID);
                SlsModModelProfile smmp = SAB.HierarchyServerSession.GetSlsModModelData(aModelRID, GetProfileList(eProfileType.Store));
                // End TT#2307

				if (!smmp.NeedsRebuilt)
				{
                    SalesModifierModelDateHash = (Hashtable) smmp.ModelDateEntries.Clone();
				}
				else
				{
                    SalesModifierModelDateHash = new Hashtable();
					foreach(SlsModModelEntry smme in smmp.ModelEntries)
					{
						ProfileList weekProfileList = SAB.HierarchyServerSession.Calendar.GetWeekRange(smme.DateRange, null);
						if (smme.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
                                if (!SalesModifierModelDateHash.Contains(weekProfile.WeekInYear))
								{
									SalesModifierModelDateHash[weekProfile.WeekInYear] = smme.SlsModModelEntryValue;
								}
							}
						}
						else
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
                                if (!SalesModifierModelDateHash.Contains(weekProfile.YearWeek))
								{
									SalesModifierModelDateHash[weekProfile.YearWeek] = smme.SlsModModelEntryValue;
								}
							}
						}
					}
				}
                
                // Begin TT#2307 - JSmith - Incorrect Stock Values
                //_salesModifierModel = new ModifierModel(smmp.SlsModModelDefault, SalesModifierModelDateHash);
                _salesModifierModel = new ModifierModel(smmp.SlsModModelDefault, SalesModifierModelDateHash, smmp.ContainsStoreDynamicDates);
                // End TT#2307

				SalesModifierModelHash.Add(aModelRID, _salesModifierModel);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the transaction load date information associated with a stock modifier model.
		/// </summary>
		/// <param name="aModelRID">The record id of the stock modifier model</param>
		private void LoadStockModifierModel(int aModelRID)
		{
			try
			{
				if (StockModifierModelHash.Contains(aModelRID))
				{
					StockModifierModelHash.Remove(aModelRID);
				}

				// Begin TT#2307 - JSmith - Incorrect Stock Values
                //StkModModelProfile smmp = SAB.HierarchyServerSession.GetStkModModelData(aModelRID);
                StkModModelProfile smmp = SAB.HierarchyServerSession.GetStkModModelData(aModelRID, GetProfileList(eProfileType.Store));
                // End TT#2307
			
				if (!smmp.NeedsRebuilt)
				{
					StockModifierModelDateHash = (Hashtable) smmp.ModelDateEntries.Clone();
				}
				else
				{
					StockModifierModelDateHash = new Hashtable();
					foreach(StkModModelEntry smme in smmp.ModelEntries)
					{
						ProfileList weekProfileList = SAB.HierarchyServerSession.Calendar.GetWeekRange(smme.DateRange, null);
						if (smme.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
								if (!StockModifierModelDateHash.Contains(weekProfile.WeekInYear))
								{
									StockModifierModelDateHash[weekProfile.WeekInYear] = smme.StkModModelEntryValue;
								}
							}
						}
						else
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
								if (!StockModifierModelDateHash.Contains(weekProfile.YearWeek))
								{
									StockModifierModelDateHash[weekProfile.YearWeek] = smme.StkModModelEntryValue;
								}
							}
						}
					}
				}
			
				// Begin TT#2307 - JSmith - Incorrect Stock Values
                //_stockModifierModel = new ModifierModel(smmp.StkModModelDefault, StockModifierModelDateHash);
                _stockModifierModel = new ModifierModel(smmp.StkModModelDefault, StockModifierModelDateHash, smmp.ContainsStoreDynamicDates);
                // End TT#2307

				StockModifierModelHash.Add(aModelRID, _stockModifierModel);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the transaction load date information associated with a FWOS modifier model.
		/// </summary>
		/// <param name="aModelRID">The record id of the FWOS modifier model</param>
		private void LoadFWOSModifierModel(int aModelRID)
		{
			try
			{
				if (FWOSModifierModelHash.Contains(aModelRID))
				{
					FWOSModifierModelHash.Remove(aModelRID);
				}

				// Begin TT#2307 - JSmith - Incorrect Stock Values
                //FWOSModModelProfile smmp = SAB.HierarchyServerSession.GetFWOSModModelData(aModelRID);
                FWOSModModelProfile smmp = SAB.HierarchyServerSession.GetFWOSModModelData(aModelRID, GetProfileList(eProfileType.Store));
                // End TT#2307
			
				if (!smmp.NeedsRebuilt)
				{
					FWOSModifierModelDateHash = (Hashtable) smmp.ModelDateEntries.Clone();
				}
				else
				{
					FWOSModifierModelDateHash = new Hashtable();
					foreach(FWOSModModelEntry smme in smmp.ModelEntries)
					{
						ProfileList weekProfileList = SAB.HierarchyServerSession.Calendar.GetWeekRange(smme.DateRange, null);
						if (smme.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
								if (!FWOSModifierModelDateHash.Contains(weekProfile.WeekInYear))
								{
									FWOSModifierModelDateHash[weekProfile.WeekInYear] = smme.FWOSModModelEntryValue;
								}
							}
						}
						else
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
								if (!FWOSModifierModelDateHash.Contains(weekProfile.YearWeek))
								{
									FWOSModifierModelDateHash[weekProfile.YearWeek] = smme.FWOSModModelEntryValue;
								}
							}
						}
					}
				}

                // Begin TT#2307 - JSmith - Incorrect Stock Values
                //_FWOSModifierModel = new ModifierModel(smmp.FWOSModModelDefault, FWOSModifierModelDateHash);
                _FWOSModifierModel = new ModifierModel(smmp.FWOSModModelDefault, FWOSModifierModelDateHash, smmp.ContainsStoreDynamicDates);
                // End TT#2307

				FWOSModifierModelHash.Add(aModelRID, _FWOSModifierModel);
			}
			catch
			{
				throw;
			}
		}

		// BEGIN MID Track #4827 - John Smith - Presentation plus sales
		/// <summary>
		/// Requests the transaction get the store presentation minimum plus sales settings for all stores for a node.
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <returns>BitArray containing presentation minimum plus sales settings indexed by storeRID</returns>
		public System.Collections.BitArray GetStorePMPlusSales(int aNodeRID)
		{
			try
			{
				StoreEligibilityList sel = null;
				ProfileList storeList = GetProfileList(eProfileType.Store);
				StoreEligibilityProfile sep = null;
				System.Collections.BitArray PMPlusSalesBitArray = new System.Collections.BitArray(_maxStoreRID + 1,false);	// default to do not apply
				

				sel = (StoreEligibilityList)StoreEligibilityHash[aNodeRID];

				if (sel == null)
				{
					sel = SAB.HierarchyServerSession.GetStoreEligibilityList(storeList, aNodeRID, true, false);
					StoreEligibilityHash.Add(aNodeRID, sel);
				}

				foreach(StoreProfile storeProfile in storeList)
				{
					sep = (StoreEligibilityProfile) sel.FindKey(storeProfile.Key);
					if (sep == null)	// if eligibility not defined for the store
					{
						PMPlusSalesBitArray[storeProfile.Key] = false;
					}
					else
						if (sep.StoreIneligible)
					{
						PMPlusSalesBitArray[storeProfile.Key] = false;
					}
					else
					{
						PMPlusSalesBitArray[storeProfile.Key] = sep.PresPlusSalesInd;
					}
				}
				return PMPlusSalesBitArray;
			}
			catch
			{
				throw;
			} 
		}
		// END MID Track #4827

        // BEGIN Track #4985 - John Smith - Override Models
        public LowLevelVersionOverrideProfileList GetOverrideList(int aModelRID, int aVersionRID,
            int aLevelOffset, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool aIncludeUnknownColor)
        {
            try
            {
                return GetOverrideList(aModelRID, Include.NoRID, aVersionRID,
                                        aLevelOffset, aHighLevelNodeRID, aIncludeNodeProfile, aIncludeUnknownColor);
            }
            catch
            {
                throw;
            }
        }

        // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
        public LowLevelVersionOverrideProfileList GetOverrideList(int aModelRID, int aVersionRID,
            eHierarchyDescendantType aGetByType, int aLevel, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool aIncludeUnknownColor)
        {
            try
            {
                return GetOverrideList(aModelRID, Include.NoRID, aVersionRID,
                                        aGetByType, aLevel, aHighLevelNodeRID, aIncludeNodeProfile, aIncludeUnknownColor);
            }
            catch
            {
                throw;
            }
        }
        // END Track #6107

        public LowLevelVersionOverrideProfileList GetOverrideList(int aModelRID, int aNodeRID, int aVersionRID,
           int aLevelOffset, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool aIncludeUnknownColor)
        {
            try
            {
                return GetOverrideList(aModelRID, aNodeRID, aVersionRID,
                     aLevelOffset, aHighLevelNodeRID, aIncludeNodeProfile, aIncludeUnknownColor, false);
            }
            catch
            {
                throw;
            }
        }

        // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
        public LowLevelVersionOverrideProfileList GetOverrideList(int aModelRID, int aNodeRID, int aVersionRID,
           eHierarchyDescendantType aGetByType, int aLevel, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool aIncludeUnknownColor)
        {
            try
            {
                return GetOverrideList(aModelRID, aNodeRID, aVersionRID,
                     aGetByType, aLevel, aHighLevelNodeRID, aIncludeNodeProfile, aIncludeUnknownColor, false);
            }
            catch
            {
                throw;
            }
        }
        // END Track #6107

        public LowLevelVersionOverrideProfileList GetOverrideList(int aModelRID, int aNodeRID, int aVersionRID,
           int aLevelOffset, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool aIncludeUnknownColor,
            bool aMaintainingModels)
        {
            try
            {
                // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
                return SAB.HierarchyServerSession.GetOverrideList(aModelRID, aNodeRID, aVersionRID,
                     eHierarchyDescendantType.offset, aLevelOffset, aHighLevelNodeRID, aIncludeNodeProfile, aIncludeUnknownColor, aMaintainingModels);
                // END Track #6107
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
        public LowLevelVersionOverrideProfileList GetOverrideListWithIgnore(int aModelRID, int aNodeRID, int aVersionRID,
           int aLevelOffset, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool aIncludeUnknownColor, bool IgnoreDuplicates)
        {
            try
            {
                return GetOverrideList(aModelRID, aNodeRID, aVersionRID,
                     aLevelOffset, aHighLevelNodeRID, aIncludeNodeProfile, aIncludeUnknownColor, false, IgnoreDuplicates);
            }
            catch
            {
                throw;
            }
        }
        // End TT#2281

        // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
        public LowLevelVersionOverrideProfileList GetOverrideList(int aModelRID, int aNodeRID, int aVersionRID,
           eHierarchyDescendantType aGetByType, int aLevel, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool aIncludeUnknownColor,
            bool aMaintainingModels)
        {
            try
            {
                return SAB.HierarchyServerSession.GetOverrideList(aModelRID, aNodeRID, aVersionRID,
                     aGetByType, aLevel, aHighLevelNodeRID, aIncludeNodeProfile, aIncludeUnknownColor, aMaintainingModels);
            }
            catch
            {
                throw;
            }
        }
        // END Track #6107

        // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
        public LowLevelVersionOverrideProfileList GetOverrideList(int aModelRID, int aNodeRID, int aVersionRID,
           int aLevelOffset, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool aIncludeUnknownColor,
            bool aMaintainingModels, bool IgnoreDuplicates)
        {
            try
            {
                return SAB.HierarchyServerSession.GetOverrideList(aModelRID, aNodeRID, aVersionRID,
                     eHierarchyDescendantType.offset, aLevelOffset, aHighLevelNodeRID, aIncludeNodeProfile, aIncludeUnknownColor, aMaintainingModels, IgnoreDuplicates);
            }
            catch
            {
                throw;
            }
        }
        // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
		
		// Begin TT#2231 - JSmith - Size curve build failing
        public LowLevelVersionOverrideProfileList GetOverrideListOfSizes(int aModelRID, int aNodeRID, bool aGetSizeCodeRIDOnly)
        {
            return SAB.HierarchyServerSession.GetOverrideListOfSizes(aModelRID, aNodeRID, aGetSizeCodeRIDOnly);
        }
        // End TT#2231

        public void DeleteOverrideList(int aModelRID, int aNodeRID)
        {
            ModelsData modelsData = null;
            try
            {
                modelsData = new ModelsData();
                modelsData.OpenUpdateConnection();
                modelsData.DeleteOverrides(aModelRID, aNodeRID);
                modelsData.CommitData();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (modelsData != null &&
                    modelsData.ConnectionIsOpen)
                {
                    modelsData.CloseUpdateConnection();
                }
            }
        }
        // END Track #4985
	}

	#region ModifierModel Class
	/// <summary>
	/// Class that defines the contents of the cache area for a modifier model.
	/// </summary>

	public class ModifierModel
	{
		//=======
		// FIELDS
		//=======

		private double _modelDefault;
		private Hashtable _modelDateHash;
        // Begin TT#2307 - JSmith - Incorrect Stock Values
        private bool _containsValuesByStore;
        // End TT#2307
		
		//=============
		// CONSTRUCTORS
		//=============

        public ModifierModel(double aModelDefault, Hashtable aModelDateHash)
        {
            _modelDefault = aModelDefault;
            _modelDateHash = aModelDateHash;
            // Begin TT#2307 - JSmith - Incorrect Stock Values
            _containsValuesByStore = false;
            // End TT#2307
        }

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        public ModifierModel(double aModelDefault, Hashtable aModelDateHash, bool aContainsValuesByStore)
        {
            _modelDefault = aModelDefault;
            _modelDateHash = aModelDateHash;
            _containsValuesByStore = aContainsValuesByStore;
        }
        // End TT#2307

		//===========
		// PROPERTIES
		//===========

		public double ModelDefault
		{
			get
			{
				return _modelDefault;
			}
		}

		public Hashtable ModelDateHash
		{
			get
			{
				return _modelDateHash;
			}
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        public bool ContainsValuesByStore
        {
            get
            {
                return _containsValuesByStore;
            }
        }
        // End TT#2307

		//========
		// METHODS
		//========
	}
	#endregion
}
