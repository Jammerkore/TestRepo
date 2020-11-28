using System;
using System.Data;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;
using MIDRetail.Business;
using System.Collections;
using System.Globalization;
using System.Windows.Forms;

namespace MIDRetail.Business
{
	#region AssortmentViewSelectionCriteriaList ProfileList
	/// <summary>
	/// Summary description for AssortmentViewSelectionCriteriaList.
	/// </summary>
	public class AssortmentViewSelectionCriteriaList : ProfileList
	{
		public AssortmentViewSelectionCriteriaList(eProfileType aType)
			:base(aType)
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
	#endregion AssortmentViewSelectionCriteriaList ProfileList

	#region AssortmentViewSelectionCriteria Profile
	/// <summary>
	/// Summary description for AssortmentViewSelectionCriteria.
	/// </summary>
	public class AssortmentViewSelectionCriteria : Profile
	{
		#region Fields
		//========//
		// Fields //
		//========//

		//private bool _firstBuild;
		//private bool _firstBuildSize; 
		private AssortmentSelection _sel;
		private ApplicationSessionTransaction _trans;
		//private AllocationWaferBuilderGroup _wafers;
		private AllocationProfileList _apl;
		private System.Windows.Forms.Form _assortmentView;
		//private bool _headersEnqueued;
		private eDataState _dataState;
		//private string _qtyAllocatedLabel;
		//private string _ruleLabel;
		//private string _ruleResultLabel;
		//private string _velocityRuleLabel;
		//private string _velocityRuleQtyLabel;
		//private string _velocityRuleResultLabel;
		//private string _transferLabel;
		//private string _styleInTransitLabel;
		//private string _styleOnHandLabel;
		//private string _totalComponentLabel;
		//private string _detailComponentLabel;
		//private string _bulkComponentLabel;
		//private string _headerTotalLabel;
		//private string _allStoreLabel;
		//private string _balanceLabel;
		//private string _preSizeAllocatedLabel; 
		//private string _unassignedLabel;
		//HeaderEnqueue _headerEnqueue;
		System.Windows.Forms.DialogResult diagResult;
        //private StoreServerSession _storeServerSession; //TT#1517-MD -jsobek -Store Service Optimization
		//private int _sizeViewRowVariableCount;
		//private bool _atLeast1WorkUpSizeBuy;


		//Begin: Work Fields for Build
		//private string [] _headerID;
		//private SortedList _colorKeyList;
		//private SortedList _sizeKeyList;
		//private bool _detailTypeFound;
		//private ArrayList _genericPackName;
		//private ArrayList _nonGenericPackName;
		//private ArrayList _subtotalGenericPackName;
		//private ArrayList _subtotalNonGenericPackName;
		//private string _lblNoSecondarySize;
		//private string _noSizeDimensionLbl;
		//private Hashtable _hdrSecSizeKeys;
		//private string _sizeTotalAllocatedLabel; // MID Track 3611 Quick Filter not working in Size Review
        //private string _allDimAllocatedLabel; // MID Track 3611 Quick Filter not working in Size Review
		//End:   Work Fields for Build

		#endregion Fields
		
		#region Constructor
		//=============//
		// Constructor //
		//=============//
		public AssortmentViewSelectionCriteria(ApplicationSessionTransaction aTrans)
			:base(Convert.ToInt32(eProfileType.AssortmentViewSelectionCriteria))
		{
			_trans = aTrans;
			//_storeGroupLevel = -1;
			AllocationProfile ap;
			_sel = new MIDRetail.Data.AssortmentSelection(_trans.SAB.ClientServerSession.UserRID);
				
			_apl = new AllocationProfileList(eProfileType.AssortmentMember);		// TT#856 - MD - stodd - total not spread error
			//foreach (int rid in _sel.AllocationHeaderRIDS)
			//{
			//    ap = new AllocationProfile(aTrans,"",rid, _trans.SAB.ApplicationServerSession);
			//    _apl.Add(ap);
			//}
			
			base.Key = UserRid;
			_assortmentView = null;
			//_headersEnqueued = false;
			_dataState = eDataState.New;
			//_headerEnqueue = null;

            //_storeServerSession = _trans.SAB.StoreServerSession; //TT#1517-MD -jsobek -Store Service Optimization
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

		public int UserRid
		{
			get{ return _sel.UserRid; }
		}	  

		public int StoreAttributeRid
		{
			get{ return _sel.StoreAttributeRid;}
			set { _sel.StoreAttributeRid = value; }
		}

		public int GroupBy
		{
			get{ return _sel.GroupBy; }
			set{_sel.GroupBy = value;}
		}

		public int ViewRid
		{
			get{ return _sel.ViewRid;}
			set{_sel.ViewRid = value;}
		}

		public eAssortmentVariableType VariableType
		{
			get { return _sel.VariableType; }
			set {_sel.VariableType = value;	}
		}

		public int VariableNumber
		{
			get { return _sel.VariableNumber; }
			set	{ _sel.VariableNumber = value; }
		}

		public bool IncludeCommitted
		{
			get{ return _sel.IncludeCommitted; }
			set {_sel.IncludeCommitted = value;	}
		}

		public bool IncludeIntransit
		{
			get { return _sel.IncludeIntransit; }
			set {_sel.IncludeIntransit = value;	}
		}

		public bool IncludeOnhand
		{
			get { return _sel.IncludeOnhand; }
			set {_sel.IncludeOnhand = value; }
		}

		public bool IncludeSimStore
		{
			get { return _sel.IncludeSimStore; }
			set	{ _sel.IncludeSimStore = value;	}
		}

		public eStoreAverageBy AverageBy
		{
			get { return _sel.AverageBy; }
			set { _sel.AverageBy = value; }
		}

		public eGradeBoundary GradeBoundary
		{
			get { return _sel.GradeBoundary; }
			set { _sel.GradeBoundary = value; }
		}

		public DataTable BasisDataTable
		{
			get { return _sel.BasisDataTable; }
			set { _sel.BasisDataTable = value; }
		}

		public DataTable StoreGradeDataTable
		{
			get { return _sel.StoreGradeDataTable; }
			set { _sel.StoreGradeDataTable = value; }
		}

		/// <summary>
		/// Gets or sets the AllocationProfileList containing the list of headers.
		/// </summary>
		public AllocationProfileList HeaderList
		{
			get{ return _apl; }
			set
			{
				_apl = value;		
			}
		}
		
		///// <summary>
		///// Gets or sets a bool that indicatew whether the selected Headers are enqueued 
		///// </summary>
		//public bool HeadersEnqueued 
		//{
		//    get { return _headersEnqueued; }
		//    set { _headersEnqueued = value;	}
		//}

		/// <summary>
		/// Gets or sets the data state of the selected headers.
		/// </summary>
		public eDataState DataState 
		{
			get { return _dataState; }
			set	{ _dataState = value; }
		}

		/// <summary>
		/// Gets or sets the AssortmentSelection
		/// </summary>
		public AssortmentSelection AssortmentSelection 
		{
			get{ return _sel; }
		}

		public int StoreGroupRID
		{
			get { return this._sel.StoreAttributeRid; }
            set { _sel.StoreAttributeRid = value; }	// TT#952 - MD - stodd - add matrix to Group Allocation Review

		}
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
			_sel.SaveUserAssortmentDefaults();
		}
		#endregion SaveDefaults

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
		#endregion Enqueue

		#region Dequeue

		public void CheckForHeaderDequeue()
		{
			// begin TT#1185 - Verify ENQ before Update 
			if ( //_styleView == null 	&& _sizeView == null && _summaryView == null &&
				_assortmentView == null
				&& !_trans.VelocityCriteriaExists)
			{
				this._trans.DequeueHeaders();
			}
		}

		///// <summary>
		///// Verifies that a header may be dequeued
		///// </summary>
		//public void CheckForHeaderDequeue()
		//{
		//    // BEGIN MID Track #2515 - Object reference error when closing size review
		//    //           Check to see that switch is set before trying to dequeue
		//    //if (_styleView == null && _summaryView == null && _sizeView == null)
		//    if (_assortmentView == null && _headersEnqueued)
		//    // END MID Track #2515
		//        DequeueHeaders();
		//}

		///// <summary>
		///// Dequeues a header 
		///// </summary>
		//public void DequeueHeaders()
		//{
		//    try
		//    {
		//        // BEGIN MID Track #2515 - Object reference error when closing size review
		//        //           Check to see that _headerEnqueue exists before trying to dequeue
		//        if (_headerEnqueue != null)
		//        // END MID Track #2515
		//            _headerEnqueue.DequeueHeaders();
				
		//        _headersEnqueued = false; 
		//    }
		//    catch ( Exception )
		//    {
		//        throw;
		//    }
			
		//}
		#endregion Dequeue
		#endregion EnqDeqHeaders
		#endregion Methods
	}
	#endregion AssortmentViewSelectionCriteria Profile
}
