using System;
using System.Collections;

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business.Allocation
{

#region AllocationHeader
//	/// <summary>
//	/// The AllocationHeader describes the merchandise to be allocated.
//	/// </summary>
//	public class AllocationHeader
//	{
//		string _id;								// Required - Allocation Header id; Key
//		string _description;					// Optional - Allocation Header description
//		// Header Configuration
//		/// <remarks>
//		/// An AllocationHeader is typically configured to allocate merchandise received (or to be received) at a distribution center.  However, an AllocationHeader is not limited to this typical configuration.  The typical configuration is the default.
//		/// <para>
//		/// A distributor may configure an AllocationHeader as a Dummy.  A Dummy is typically used to reserve in-transit space in the stores in anticipation of a future receipt.  It can also be used to configure a primary distribution that will drive future receipts. 
//		/// </para><para>
//		/// An AllocationHeader may be configured to allocate basic merchandise from a warehouse.  In this case, the warehouse on-hands are interfaced to the allocation system as an AllocationHeader. 
//		/// </para><para>
//		/// An AllocationHeader may be configured to work up a buy.  In this case, the system will calculate recommended units to buy.  Four basic configurations are supported:  WorkUpBulkColorBuy, WorkUpBulkSizeBuy, WorkUpPackColorBuy and WorkUpPackSizeBuy.
//		/// </para><para>
//		/// The WorkUpBulk and WorkUpPack buys are mutually exclusive configurations.  However, the color and size options for within each configuration are compatible with each other
//		/// </para><para>
//		/// For the WorkUpBulkColorBuy, the configuration consists of: UnitsReceived and the Bulk-Colors to include.  No receipts by Bulk-Color are permitted at definition time--the system will calculate these values as the allocation proceeds.  The PackReceipts are not valid for a WorkUpBulkColorBuy.  
//		/// </para><para>
//		/// For the WorkUpBulkSizeBuy, the configuration consists of: UnitsReceived, the Bulk-Colors to include and the Bulk-Sizes to include.  If it is not a WorkUpBulkColorBuy, then the Bulk-Color Receipts must be described. If it is a WorkUpColorBuy, the Bulk-Color Receipts must not be defined.  In any case, the Bulk-Size Receipts must not be specified--these values are to be calculated during the allocation process.
//		/// </para><para>
//		/// For the WorkUpPackColorBuy, the configuration consists of: Units Received, a list of colors to include and Generic Packs that describe the desired pack multiples.  The allocation process will calculate the pack content and number of each pack to receive (buy).
//		/// </para><para>
//		/// For the WorkUpPackSizeBuy, the configuration consists of: Units Received, a list of sizes to include and Generic Packs that describe the desired pack multiples (and color content if known).  The allocation process will calculate the pack size content and the number of each pack to receive (buy).
//		/// </para></remarks>
//		bool _isDummy;                          // default = false 
//		bool _allocateFromWarehouse;            // default = false 
//		bool _workUpBulkColorBuy;               // default = false, Mutually Exclusive with _workUpPack*
//		bool _workUpBulkSizeBuy;                // default = false, Mutually Exclusive with _workUpPack* 
//		bool _workUpPackColorBuy;               // default = false, Mutually Exclusive with _workUpBulk*
//		bool _workUpPackSizeBuy;                // default = false, Mutually Exclusive with _workUpBulk*
//		// Allocation Header Status (internally controlled)
//		/// <summary>
//		/// An AllocationHeader has several status flags that measure the progress of the allocation.
//		/// </summary>
//		/// <para>
//		/// Status ReceivedInBalance indicates that the AllocationHeader is correctly defined and that detail pack, Bulk-Color and Bulk-Size receipts foot to appropriate totals.  Allocation cannot begin until ReceivedInBalance is true.  When ReceivedInBalance is false, then no allocation process may begin--in this case, the status is ReceivedOutOfBalance. 
//		/// </para>
//		bool _receivedInBalance;                // default = false
//		/// <para>
//		/// Status AllocationStarted indicates that at least 1 unit on this AllocationHeader has been allocated to the stores.
//		/// </para>
//		bool _allocationStarted;                // default = false
//		/// <para>
//		/// Status BottomUpSizeStarted indicates that at least 1 bottom-up size allocation process has occurred.
//		/// </para>
//		bool _bottomUpSizeStarted;              // default = false
//		/// <para>
//		/// Status SpecialRulesDefined indicates that at least 1 Special Rule has been defined on at least 1 component and all accepted rules have been applied.
//		/// </para>
//		bool _specialRulesDefined;              // default = false
//		/// <para>
//		/// Status NeedAllocationPerformed indicates that a need allocation has occurred.
//		/// </para>
//		bool _needAllocationPerformed;          // default = false
//		/// <para>
//		/// Status PacksAndBulkColorsInBalance indicates that the allocation is in-balance at the style level.
//		/// </para>
//		bool _packsAndBulkColorsInBalance;      // default = false
//		/// <para>
//		/// Status BulkSizesInBalance indicates that the allocation is in-balance at the size level.  When this condition is true, it implies that the style level is also in-balance.
//		/// </para>
//		bool _bulkSizesInBalance;               // default = false
//		/// <para>
//		/// Status StyleIntransitUpdated indicates that the store allocation for this AllocationHeader has updated the style intransit for each store's charge intransit date.
//		/// </para>
//		bool _styleIntransitUpdated;            // default = false
//		/// <para>
//		/// Status BulkColorIntransitUpdated indicates that the store allocation for this AllocationHeader has updated Color Intransit.  This status is only valid when Bulk Colors exist on the header. 
//		/// </para>
//		bool _bulkColorIntransitUpdated;        // default = false, implies _styleIntransitUpdated=true
//		/// <para>
//		/// Status BulkSizeIntransitUpdated indicates that the store allocation for this AllocationHeader has updated Size Intransit.  This status is only valid when Bulk Sizes exist on the header. 
//		/// </para>
//		bool _bulkSizeIntransitUpdated;         // default = false, implies _bulkColorIntransitUpdated=true
//		/// <para>
//		/// Status releaseApproved indicates that this AllocationHeader is ready to be released.
//		/// </para>
//		bool _releaseApproved;                  // default = false
//		/// <para>
//		/// Status Released indicates that this AllocationHeader is ready to be picked and shipped to the stores.
//		/// </para>
//		bool _released;                         // default = false, implies _releaseApproved=true
//		/// <para>
//		/// Status ShippingStarted indicates that the intransit for at least one store has been partially or completely relieved.
//		/// </para>
//		bool _shippingStarted;                  // default = false, implies _released=true
//		/// <para>
//		/// Status ShippingCompleted indicates that the intransit for all stores has been relieved.
//		/// </para>
//		bool _shippingComplete;                 // default = false, implies _shippingStarted=true
//		// Description
//		int _stylePId;							// Req - maps to MHieracrchy
//		char _vendor;                           // Optional
//		char _PurchaseOrder;                    // Optional 
//		double _unitRetail;						// Req; must be > 0; 2-decimals 
//		DateTime _receiptDate;					// default = Today, must be valid date
//		int _unitsReceived;						// Req; default = 0, must be >= 0
//		NameId[] _packCode;
//		NameId[] _colorCode;
//		NameId[] _widthCode;
//		NameId[] _sizeCode;
//		Component _total;
//		Component _genericPackSubTotal;
//		Component _colorSizeSubTotal;
//		PackReceipt[] _pack;
//		BulkReceipt _bulk;
//		DateTime _beginDate;                    // Optional; when specified, future mdse calendar date
//		DateTime _shipDate;                     // Optional; when specified, future mdse calendar date
//		DateTime _releaseApprovedDate;          // Date when _releaseApproved first set to true
//		DateTime _releaseDate;                  // Date when _released first set to true
//		/// <param name="AllocationHeaderID" uniquely identifies the allocation header. If the first character of the allocation header is a "D", then the allocation header defines a Dummy allocation header.
//		/// <param name="aWorkUpBuyType" designates whether the allocation header is a work up buy or not a work up buy.  A null value for this parameter indicates that the allocation header is not a work up buy. If it is a work up buy, this parameter designates what type of work up buy:  WorkUpColorBuy, WorkUpSizeBuy, WorkUpColorSizeBuy, WorkUpPackColorBuy, WorkUpPackSizeBuy or WorkUpPackColorSizeBuy. 
//		public AllocationHeader(string AllocationHeaderID, eWorkUpBuyType aWorkUpBuyType )
//		{
//			_id = AllocationHeaderID;
//			if ( Equals(_id [0],"D") )
//			{
//				_isDummy = true;
//			}
//			else
//			{
//				_isDummy = false;
//			}
//			_allocateFromWarehouse = false;
//			switch ( aWorkUpBuyType )
//			{
//       			case eWorkUpBuyType.NotWorkUpBuy:
//				{
//					_workUpBulkColorBuy = false;
//					_workUpBulkSizeBuy = false;
//					_workUpPackColorBuy = false;
//					_workUpPackSizeBuy = false;
//					break;
//				}
//				case eWorkUpBuyType.WorkUpBulkColorBuy:
//				{
//					_workUpBulkColorBuy = true;
//					_workUpBulkSizeBuy = false;
//					_workUpPackColorBuy = false;
//					_workUpPackSizeBuy = false;
//					break;
//				}
//				case eWorkUpBuyType.WorkUpBulkSizeBuy:
//				{
//					_workUpBulkColorBuy = false;
//					_workUpBulkSizeBuy = true;
//					_workUpPackColorBuy = false;
//					_workUpPackSizeBuy = false;
//					break;
//				}
//				case eWorkUpBuyType.WorkUpBulkColorSizeBuy:
//				{
//					_workUpBulkColorBuy = true;
//					_workUpBulkSizeBuy = true;
//					_workUpPackColorBuy = false;
//					_workUpPackSizeBuy = false;
//					break;
//				}
//				case eWorkUpBuyType.WorkUpPackColorBuy:
//				{
//					_workUpBulkColorBuy = false;
//					_workUpBulkSizeBuy = false;
//					_workUpPackColorBuy = true;
//					_workUpPackSizeBuy = false;
//					break;
//				}
//				case eWorkUpBuyType.WorkUpPackSizeBuy:
//				{
//					_workUpBulkColorBuy = false;
//					_workUpBulkSizeBuy = false;
//					_workUpPackColorBuy = false;
//					_workUpPackSizeBuy = true;
//					break;
//				}
//				case eWorkUpBuyType.WorkUpPackColorSizeBuy:
//				{
//					_workUpBulkColorBuy = false;
//					_workUpBulkSizeBuy = false;
//					_workUpPackColorBuy = true;
//					_workUpPackSizeBuy = true;
//					break;
//				}		
//				default:
//				{
//                    // To Do
//					// Issue error: Invalid Work Up Buy designation.
//					break;
//				}
//			}
//			_receivedInBalance = false;              
//			_allocationStarted = false;              
//			_bottomUpSizeStarted = false;            
//			_specialRulesDefined = false;            
//			_needAllocationPerformed = false;        
//			_packsAndBulkColorsInBalance = false;    
//			_bulkSizesInBalance = false;             
//			_styleIntransitUpdated = false;          
//			_bulkColorIntransitUpdated = false;      
//			_bulkSizeIntransitUpdated = false;       
//			_releaseApproved = false;                
//			_released = false;                  
//			_shippingStarted = false;           
//			_shippingComplete = false;          			
//			_unitRetail = 0.0;
//			_unitsReceived = 0;
//			_total = new Component();
//			_genericPackSubTotal = new Component();
//			_colorSizeSubTotal = new Component();
//			_bulk = new BulkReceipt();
//		}
//		#region AllocationHeader Properties
//		/// <summary>
//		/// The Allocation Header ID is a unique identifier for the allocation header.
//		/// </summary>
//		public string AllocationHeaderID
//		{
//			get { return _id; }
//		}
//		public bool IsDummy
//		{
//			get 
//			{ 
//				if (_isDummy)
//				{
//					return true;
//				}
//				else
//				{
//					return false;
//				}
//			}
//		}
//		public bool AllocateFromWarehouse
//		{
//			get { return _allocateFromWarehouse; }
//			set { _allocateFromWarehouse = value; }
//		}
//		public bool WorkUpBulkColorBuy
//		{
//			get { return _workUpBulkColorBuy; }
//		}
//		public bool WorkUpBulkSizeBuy
//		{
//			get 
//			{
//				return _workUpBulkSizeBuy; 
//			}
//		}
//		public bool WorkUpPackColorBuy
//		{
//			get
//			{
//				return _workUpPackColorBuy;
//			}
//		}
//		public bool WorkUpPackSizeBuy
//		{
//			get
//			{
//				return _workUpPackSizeBuy;
//			}
//		}
//		public bool ReceivedInBalance
//		{
//			get { return _receivedInBalance; }
//		}
//		public bool AllocationStarted
//		{
//			get { return _allocationStarted; }
//		}
//		public bool BottomUpSizeStarted
//		{
//			get { return _bottomUpSizeStarted;}
//		}
//		public bool SpecialRulesDefined
//		{
//			get { return _specialRulesDefined; }
//		}
//		public bool NeedAllocationPerformed
//		{
//			get { return _needAllocationPerformed; }
//		}
//		public bool PacksAndBulkColorsInBalance
//		{
//			get { return _packsAndBulkColorsInBalance; }
//		}
//		public bool BulkSizesInBalance
//		{
//			get { return _bulkSizesInBalance; }
//		}
//		public bool StyleIntransitUpdated
//		{
//			get { return _styleIntransitUpdated; }
//		}
//		public bool BulkColorIntransitUpdated
//		{
//			get { return _bulkColorIntransitUpdated; }
//		}  
//		public bool BulkSizeIntransitUpdated
//		{
//			get { return _bulkSizeIntransitUpdated; }
//		}       
//		public bool ReleaseApproved
//		{
//			get { return _releaseApproved; }
//		}    
//		public bool Released
//		{
//			get { return _released; }
//		}      
//		public bool ShippingStarted
//		{
//			get { return _shippingStarted; }
//		}
//		public bool ShippingComplete
//		{
//			get { return _shippingComplete; }
//		}
//		public int UnitsReceived
//        {
//	        get { return _unitsReceived; }
//         	set { _unitsReceived = value; }
//        }
//		#endregion
//}
//	#endregion
//	#region NameId
//	/// <summary>
//	/// Receiver internal class NameId.
//	/// </summary>	internal class NameId;
//	internal class NameId
//	{
//		string _name;
//		int _id;
//		public NameId()
//		{
//			_name = null;
//			_id = 0;
//		}
//		#region NameId Properties
//		public string Name
//		{
//			get { return _name; }	
//			set { _name = value; }
//		}
//		public int Id
//		{
//			get { return _id; }
//			set { _id = value; }
//		}
//		#endregion
//	}
//	#endregion
//	#region Component
//	public enum ComponentType
//	{
//		Total, Pack, Bulk, Color, Width, Size
//	}
//	internal class Component
//	{
//		int _id;
//		ComponentType _type;
//		int _units;
//		Component[] _subComponent;
//		public Component()
//		{
//			_units = 0;
//		}
//		#region Properties
//		public int Id
//		{
//			get { return _id; }
//			set { _id = value; }
//		}
//		public int Units
//		{
//			get { return _units; }
//			set { _units = value; }
//		}
//		public ComponentType Type
//		{
//			get { return _type; }
//			set { _type = value; }
//		}
//		#endregion
//	}
//	#endregion
//	#region Bulk Receipt Components
//	internal class BulkReceipt:BulkBaseComponent
//	{
//	}
//	internal class BulkBaseComponent:Component
//	{
//		int _multiple;
//		public BulkBaseComponent()
//		{
//			_multiple = 1;
//		}
//		#region Properties
//		public int Multiple
//		{
//			get { return _multiple; }
//			set { _multiple = value; }
//		}
//		#endregion
//	}
//	internal class BulkColorComponent:BulkBaseComponent
//	{
//	}
//	internal class BulkWidthComponent:BulkBaseComponent
//	{
//	}
//	internal class BulkSizeComponent:BulkBaseComponent
//	{
//	}
//	#endregion
//	#region Pack Receipt Components
//	internal class PackBaseComponent:Component
//	{
//	}
//	internal class PackReceipt:PackBaseComponent
//	{
//		int _multiple;
//		int _numberOfPacks;
//		bool _knowColor;
//		bool _knowWidthSize;
//		public PackReceipt ()
//		{
//			_multiple = 1;
//			_numberOfPacks = 0;
//			_knowColor = false;
//			_knowWidthSize = false;
//		}
//	#region Properties
//		public int Multiple
//		{
//			get { return _multiple; }
//			set { _multiple = value; }
//		}
//		public int NumberOfPacks
//		{
//			get { return _numberOfPacks; }
//			set { _numberOfPacks = value; }
//		}
//		public bool KnowColor
//		{
//			get { return _knowColor; }
//			set { _knowColor = value; }
//		}
//		public bool KnowWidthSize
//		{
//			get { return _knowWidthSize; }
//			set { _knowWidthSize = value; }
//		}
//	#endregion
//	}
//	internal class PackColorComponent:PackBaseComponent
//	{
//	}
//	internal class PackWidthComponent:PackBaseComponent
//	{
//	}
//	internal class PackSizeComponent:PackBaseComponent
//	{
//	}
//	#endregion
//	#region Bulk Distribution Components
//	internal class BulkDistribution:Component
//	{
//	}
//	internal class BulkColorDistribution:Component
//	{
//	}
//	internal class BulkWidthDistribution:Component
//	{
//	}
//	internal class BulkSizeDistribution:Component
//	{
//	}
//	#endregion
//	#region Pack Distribution Components
//	internal class PackDistribution:Component
//	{
//		int _packsAllocated;
//		public PackDistribution()
//		{
//			_packsAllocated = 0;
//		}
//	}
//	#endregion
//	#region Store Distribution
//	internal class StoreDistribution
//	{
//		int _store;
//		Component _total;
//		Component _totalPacksUnKnownContent;
//		Component _totalPacksBulkKnownContent;
//		Component _totalPacksBulkKnownColor;
//		Component _totalPacksBulkKnownSize;
//		PackDistribution[] _pack;
//		BulkDistribution _bulk;
//		public StoreDistribution()
//		{
//		}
//	}
//	#endregion
//	#region Distribution
//	public class Distribution
//	{
//		string _id;
//		double _stylePctOfPlan;
//		int _onhandSourceId; 
//		double _pctNeedThreshold;
//		bool _mayExceedMax;
//		Component _total;
//		Component _colorPackTotal;
//		Component _colorSizePackTotal;
//		Component _sizePackTotal;
//		PackDistribution [] _pack;
//		BulkDistribution _bulk;
//		StoreDistribution [] _store;
//		public Distribution()
//		{
//	
//		}
//	}
#endregion
}
