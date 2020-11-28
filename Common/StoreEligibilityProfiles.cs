using System;
using System.Collections;
using System.Data;

using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;
using MIDRetail.Common;

namespace MIDRetail.Common
{
	///// <summary>
	///// Contains the information about the eligibility for a store for a node in a hierarchy
	///// </summary>
	//[Serializable()]
	//public class StoreEligibilityProfile : Profile
	//{
	//	// Fields

	//	private eChangeType			_storeEligChangeType;
	//	private bool				_recordExists;

	//	private bool				_eligIsSet;
	//	private bool				_eligIsInherited;
	//	private int					_eligInheritedFromNodeRID;
	//	private eEligibilitySettingType	_eligType;
	//	private int					_eligModelRID;
	//	private string				_eligModelName;
	//	private bool				_storeIneligible;
 //       private DateTime _updateDate;

	//	private bool				_stkModIsInherited;
	//	private int					_stkModInheritedFromNodeRID;
	//	private eModifierType		_stkModType;
	//	private int					_stkModModelRID;
	//	private string				_stkModModelName;
	//	private double				_stkModPct;
 //       //BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement
 //       private bool                _stkLeadWeeksInherited;
 //       private int                 _stkLeadWeeksInheritedRid;
 //       private int                 _stkLeadWeeks;
 //       //END TT#44 - MD - DOConnell - New Store Forecasting Enhancement
	//	private bool				_slsModIsInherited;
	//	private int					_slsModInheritedFromNodeRID;
	//	private eModifierType		_slsModType;
	//	private int					_slsModModelRID;
	//	private string				_slsModModelName;
	//	private double				_slsModPct;

	//	// BEGIN MID Track #4370 - John Smith - FWOS Models
	//	private bool				_FWOSModIsInherited;
	//	private int					_FWOSModInheritedFromNodeRID;
	//	private eModifierType		_FWOSModType;
	//	private int					_FWOSModModelRID;
	//	private string				_FWOSModModelName;
	//	private double				_FWOSModPct;
	//	// END MID Track #4370

	//	private bool				_simStoreIsInherited;
	//	private int					_simStoreInheritedFromNodeRID;
	//	private eSimilarStoreType	_simStoreType;
	//	private bool				_simStoresChanged;
	//	private ArrayList			_simStores;
	//	private double				_simStoreRatio;
	//	private int					_simStoreUntilDateRangeRID;
	//	private string				_simStoreDisplayDate;
	//	private ProfileList			_simStoreWeekList;
	//	// BEGIN MID Track #4827 - John Smith - Presentation plus sales
	//	private bool				_presPlusSalesIsSet;
	//	private bool				_presPlusSalesIsInherited;
	//	private int					_presPlusSalesInheritedFromNodeRID;
	//	private bool				_presPlusSalesInd;
	//	// END MID Track #4827

	//	/// <summary>
	//	/// Used to construct an instance of the class.
	//	/// </summary>
	//	public StoreEligibilityProfile(int aKey)
	//		: base(aKey)
	//	{
	//		_storeEligChangeType		= eChangeType.none;
	//		_recordExists				= false;

	//		_eligIsSet					= false;
	//		_eligIsInherited			= false;
	//		_eligInheritedFromNodeRID	= Include.NoRID;
	//		_eligType					= eEligibilitySettingType.None;
	//		_eligModelRID				= Include.NoRID;
	//		_eligModelName				= "";
	//		_storeIneligible			= false;
 //           _updateDate = DateTime.MinValue;

	//		_stkModIsInherited			= false;
	//		_stkModInheritedFromNodeRID	= Include.NoRID;
	//		_stkModType					= eModifierType.None;
	//		_stkModModelRID				= Include.NoRID;
	//		_stkModModelName			= "";
	//		_stkModPct					= 0;
 //           //BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement
 //           _stkLeadWeeksInherited      = false;
 //           _stkLeadWeeksInheritedRid   = Include.NoRID;
 //           _stkLeadWeeks               = Include.NoRID;
 //           //END TT#44 - MD - DOConnell - New Store Forecasting Enhancement
	//		_slsModIsInherited			= false;
	//		_slsModInheritedFromNodeRID	= Include.NoRID;
	//		_slsModType					= eModifierType.None;
	//		_slsModModelRID				= Include.NoRID;
	//		_slsModModelName			= "";
	//		_slsModPct					= 0;

	//		// BEGIN MID Track #4370 - John Smith - FWOS Models
	//		_FWOSModIsInherited			= false;
	//		_FWOSModInheritedFromNodeRID	= Include.NoRID;
	//		_FWOSModType					= eModifierType.None;
	//		_FWOSModModelRID				= Include.NoRID;
	//		_FWOSModModelName			= "";
	//		_FWOSModPct					= 0;
	//		// END MID Track #4370

	//		_simStoreIsInherited			= false;
	//		_simStoreInheritedFromNodeRID	= Include.NoRID;
	//		_simStoreType				= eSimilarStoreType.None;
	//		_simStoresChanged			= false;
	//		_simStores					= new ArrayList();
	//		_simStoreRatio				= 0;
	//		_simStoreUntilDateRangeRID	= Include.NoRID;
	//		_eligIsInherited			= false;
	//		_eligInheritedFromNodeRID	= Include.NoRID;

	//		// BEGIN MID Track #4827 - John Smith - Presentation plus sales
	//		_presPlusSalesIsSet			= false;
	//		_presPlusSalesIsInherited	= false;
	//		_presPlusSalesInheritedFromNodeRID = Include.NoRID;
	//		_presPlusSalesInd			= false;
	//		// END MID Track #4827
	//	}

	//	// Properties

	//	/// <summary>
	//	/// Returns the eProfileType of this profile.
	//	/// </summary>
	//	override public eProfileType ProfileType
	//	{
	//		get
	//		{
	//			return eProfileType.StoreEligibility;
	//		}
	//	}

	//	/// <summary>
	//	/// Gets or sets the change type of a store's eligibility information.
	//	/// </summary>
	//	public eChangeType StoreEligChangeType 
	//	{
	//		get { return _storeEligChangeType ; }
	//		set { _storeEligChangeType = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets a flag identifying if a store's eligibility information is inherited.
	//	/// </summary>
	//	public bool RecordExists 
	//	{
	//		get { return _recordExists ; }
	//		set { _recordExists = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets a flag identifying if a store's eligibility information is inherited.
	//	/// </summary>
	//	public bool EligIsSet 
	//	{
	//		get { return _eligIsSet ; }
	//		set { _eligIsSet = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets a flag identifying if a store's eligibility information is inherited.
	//	/// </summary>
	//	public bool EligIsInherited 
	//	{
	//		get { return _eligIsInherited ; }
	//		set { _eligIsInherited = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the record ID of the node where the store's eligibility information is inherited from.
	//	/// </summary>
	//	public int EligInheritedFromNodeRID 
	//	{
	//		get { return _eligInheritedFromNodeRID ; }
	//		set { _eligInheritedFromNodeRID = value; }
	//	}

	//	/// <summary>
	//	/// Gets or sets the type of eligibility.
	//	/// </summary>
	//	public eEligibilitySettingType EligType
	//	{
	//		get { return _eligType ; }
	//		set { _eligType = value; }
	//	}

	//	/// <summary>
	//	/// Gets or sets the record id of the elig model for the store.
	//	/// </summary>
	//	public int EligModelRID 
	//	{
	//		get { return _eligModelRID ; }
	//		set { _eligModelRID = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the name of the elig model for the store.
	//	/// </summary>
	//	public string EligModelName 
	//	{
	//		get { return _eligModelName ; }
	//		set { _eligModelName = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the flag identifying that a store is ineligible.
	//	/// </summary>
	//	public bool StoreIneligible 
	//	{
	//		get { return _storeIneligible ; }
	//		set { _storeIneligible = value; }
	//	}

 //       /// <summary>
	//	/// Gets or sets the date that the store is updated.
	//	/// </summary>
 //       public DateTime UpdateDate
 //       {
 //           get { return _updateDate; }
 //           set { _updateDate = value; }
 //       }

	//	/// <summary>
	//	/// Gets or sets a flag identifying if a store's stock modifier information is inherited.
	//	/// </summary>
	//	public bool StkModIsInherited 
	//	{
	//		get { return _stkModIsInherited ; }
	//		set { _stkModIsInherited = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the record ID of the node where the store's stock modifier information is inherited from.
	//	/// </summary>
	//	public int StkModInheritedFromNodeRID 
	//	{
	//		get { return _stkModInheritedFromNodeRID ; }
	//		set { _stkModInheritedFromNodeRID = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the type of stock modifier.
	//	/// </summary>
	//	public eModifierType StkModType 
	//	{
	//		get { return _stkModType ; }
	//		set { _stkModType = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the record id for the stock modifier model.
	//	/// </summary>
	//	public int StkModModelRID 
	//	{
	//		get { return _stkModModelRID ; }
	//		set { _stkModModelRID = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the name of the stock modifier.
	//	/// </summary>
	//	public string StkModModelName 
	//	{
	//		get { return _stkModModelName ; }
	//		set { _stkModModelName = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the stock modifier percent for the store if a model is not used.
	//	/// </summary>
	//	public double StkModPct 
	//	{
	//		get { return _stkModPct ; }
	//		set { _stkModPct = value; }
	//	}

 //       //BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement
 //       /// <summary>
 //       /// Gets or sets a flag identifying if a store's Stock Lead Weeks information is inherited.
 //       /// </summary>
 //       public bool StkLeadWeeksInherited
 //       {
 //           get { return _stkLeadWeeksInherited; }
 //           set { _stkLeadWeeksInherited = value; }
 //       }
 //       /// <summary>
 //       /// Gets or sets the record ID of the node where the store's Stock Lead Weeks information is inherited from.
 //       /// </summary>
 //       public int StkLeadWeeksInheritedRid
 //       {
 //           get { return _stkLeadWeeksInheritedRid; }
 //           set { _stkLeadWeeksInheritedRid = value; }
 //       }

 //       /// <summary>
 //       /// Gets or sets the stock lead weeks a new store will recieve in a new store inventory plan.
 //       /// </summary>
 //       public int StkLeadWeeks
 //       {
 //           get { return _stkLeadWeeks; }
 //           set { _stkLeadWeeks = value; }
 //       }
 //       //END TT#44 - MD - DOConnell - New Store Forecasting Enhancement

	//	/// <summary>
	//	/// Gets or sets a flag identifying if a store's sales modifier information is inherited.
	//	/// </summary>
	//	public bool SlsModIsInherited 
	//	{
	//		get { return _slsModIsInherited ; }
	//		set { _slsModIsInherited = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the record ID of the node where the store's sales modifier information is inherited from.
	//	/// </summary>
	//	public int SlsModInheritedFromNodeRID 
	//	{
	//		get { return _slsModInheritedFromNodeRID ; }
	//		set { _slsModInheritedFromNodeRID = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the sales modifier.
	//	/// </summary>
	//	public eModifierType SlsModType 
	//	{
	//		get { return _slsModType ; }
	//		set { _slsModType = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the record id for the sales modifier model.
	//	/// </summary>
	//	public int SlsModModelRID 
	//	{
	//		get { return _slsModModelRID ; }
	//		set { _slsModModelRID = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the name of the sales modifier.
	//	/// </summary>
	//	public string SlsModModelName 
	//	{
	//		get { return _slsModModelName ; }
	//		set { _slsModModelName = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the sales modifier percent for the store if a model is not used.
	//	/// </summary>
	//	public double SlsModPct 
	//	{
	//		get { return _slsModPct ; }
	//		set { _slsModPct = value; }
	//	}

	//	// BEGIN MID Track #4370 - John Smith - FWOS Models
	//	/// <summary>
	//	/// Gets or sets a flag identifying if a store's FWOS modifier information is inherited.
	//	/// </summary>
	//	public bool FWOSModIsInherited 
	//	{
	//		get { return _FWOSModIsInherited ; }
	//		set { _FWOSModIsInherited = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the record ID of the node where the store's FWOS modifier information is inherited from.
	//	/// </summary>
	//	public int FWOSModInheritedFromNodeRID 
	//	{
	//		get { return _FWOSModInheritedFromNodeRID ; }
	//		set { _FWOSModInheritedFromNodeRID = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the FWOS modifier.
	//	/// </summary>
	//	public eModifierType FWOSModType 
	//	{
	//		get { return _FWOSModType ; }
	//		set { _FWOSModType = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the record id for the FWOS modifier model.
	//	/// </summary>
	//	public int FWOSModModelRID 
	//	{
	//		get { return _FWOSModModelRID ; }
	//		set { _FWOSModModelRID = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the name of the FWOS modifier.
	//	/// </summary>
	//	public string FWOSModModelName 
	//	{
	//		get { return _FWOSModModelName ; }
	//		set { _FWOSModModelName = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the FWOS modifier percent for the store if a model is not used.
	//	/// </summary>
	//	public double FWOSModPct 
	//	{
	//		get { return _FWOSModPct ; }
	//		set { _FWOSModPct = value; }
	//	}
	//	// END MID Track #4370

	//	/// <summary>
	//	/// Gets or sets a flag identifying if a store's similar store information is inherited.
	//	/// </summary>
	//	public bool SimStoreIsInherited 
	//	{
	//		get { return _simStoreIsInherited ; }
	//		set { _simStoreIsInherited = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the record ID of the node where the store's similar store information is inherited from.
	//	/// </summary>
	//	public int SimStoreInheritedFromNodeRID 
	//	{
	//		get { return _simStoreInheritedFromNodeRID ; }
	//		set { _simStoreInheritedFromNodeRID = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the type of simlar store identified for the store.
	//	/// </summary>
	//	public eSimilarStoreType SimStoreType 
	//	{
	//		get { return _simStoreType ; }
	//		set { _simStoreType = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the switch to identify if the similar stores have been changed.
	//	/// </summary>
	//	public bool SimStoresChanged 
	//	{
	//		get { return _simStoresChanged ; }
	//		set { _simStoresChanged = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the list of simlar stores identified for the store.
	//	/// </summary>
	//	public ArrayList SimStores 
	//	{
	//		get { return _simStores ; }
	//		set { _simStores = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the ratio of simlar store for the store.
	//	/// </summary>
	//	public double SimStoreRatio 
	//	{
	//		get { return _simStoreRatio ; }
	//		set { _simStoreRatio = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the record id of the date range to use for the similar store.
	//	/// </summary>
	//	public int SimStoreUntilDateRangeRID 
	//	{
	//		get { return _simStoreUntilDateRangeRID ; }
	//		set { _simStoreUntilDateRangeRID = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the display date to use for the similar store.
	//	/// </summary>
	//	public string SimStoreDisplayDate 
	//	{
	//		get { return _simStoreDisplayDate ; }
	//		set { _simStoreDisplayDate = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the week list of the date range to use for the similar store.
	//	/// </summary>
	//	public ProfileList SimStoreWeekList 
	//	{
	//		get { return _simStoreWeekList ; }
	//		set { _simStoreWeekList = value; }
	//	}

	//	// BEGIN MID Track #4827 - John Smith - Presentation plus sales
	//	/// <summary>
	//	/// Gets or sets the flag identifying if the presentation plus sales indicator was set.
	//	/// </summary>
	//	public bool PresPlusSalesIsSet 
	//	{
	//		get { return _presPlusSalesIsSet ; }
	//		set { _presPlusSalesIsSet = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the flag identifying if the presentation plus sales indicator was inherited.
	//	/// </summary>
	//	public bool PresPlusSalesIsInherited 
	//	{
	//		get { return _presPlusSalesIsInherited ; }
	//		set { _presPlusSalesIsInherited = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the flag identifying if the presentation plus sales indicator was inherited.
	//	/// </summary>
	//	public int PresPlusSalesInheritedFromNodeRID 
	//	{
	//		get { return _presPlusSalesInheritedFromNodeRID ; }
	//		set { _presPlusSalesInheritedFromNodeRID = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the flag identifying if the presentation plus sales option is to be used.
	//	/// </summary>
	//	public bool PresPlusSalesInd 
	//	{
	//		get { return _presPlusSalesInd ; }
	//		set { _presPlusSalesInd = value; }
	//	}
	//	// END MID Track #4827
	//}

	///// <summary>
	///// Used to retrieve a list of eligibility information
	///// </summary>
	//[Serializable()]
	//public class StoreEligibilityList : ProfileList
	//{
	//	/// <summary>
	//	/// Used to construct an instance of the class.
	//	/// </summary>
	//	public StoreEligibilityList(eProfileType aProfileType)
	//		: base(aProfileType)
	//	{
	//		//
	//		// TODO: Add constructor logic here
	//		//
	//	}
	//}


	///// <summary>
	///// Contains the information about the eligibility for a store for a node in a hierarchy
	///// </summary>
	//[Serializable()]
	//public class StoreWeekEligibilityProfile : Profile
	//{
	//	// Fields

	//	private int					_yearWeek;
	//	private bool				_storeIsEligible;
	//	private bool				_storeIsPriorityShipper;
		
	//	/// <summary>
	//	/// Used to construct an instance of the class.
	//	/// </summary>
	//	public StoreWeekEligibilityProfile(int aKey)
	//		: base(aKey)
	//	{
	//		_yearWeek = -1;
	//		_storeIsEligible = true;
	//		_storeIsPriorityShipper = false;
	//	}

	//	// Properties

	//	/// <summary>
	//	/// Returns the eProfileType of this profile.
	//	/// </summary>
	//	override public eProfileType ProfileType
	//	{
	//		get
	//		{
	//			return eProfileType.StoreEligibility;
	//		}
	//	}

	//	/// <summary>
	//	/// Gets or sets the year/week for which eligibility is to be determined.
	//	/// </summary>
	//	public int YearWeek 
	//	{
	//		get { return _yearWeek ; }
	//		set { _yearWeek = value; }
	//	}
	//	/// <summary>
	//	/// Gets or sets the flag identifying that a store is eligible.
	//	/// </summary>
	//	public bool StoreIsEligible 
	//	{
	//		get { return _storeIsEligible ; }
	//		set { _storeIsEligible = value; }
	//	}

	//	/// <summary>
	//	/// Gets or sets the flag identifying that a store is a priority shipper.
	//	/// </summary>
	//	public bool StoreIsPriorityShipper 
	//	{
	//		get { return _storeIsPriorityShipper ; }
	//		set { _storeIsPriorityShipper = value; }
	//	}
	//}

	///// <summary>
	///// Used to retrieve a list of eligibility information for the store/week combinations
	///// </summary>
	//[Serializable()]
	//public class StoreWeekEligibilityList : ProfileList
	//{
	//	/// <summary>
	//	/// Used to construct an instance of the class.
	//	/// </summary>
	//	public StoreWeekEligibilityList(eProfileType aProfileType)
	//		: base(aProfileType)
	//	{
	//		//
	//		// TODO: Add constructor logic here
	//		//
	//	}
	//}

    public class storeEligibilityDataSet : MIDObject
    {
        public DataSet StoreEligibility_Define(System.Data.DataSet _storeEligibilityDataSet)
        {
            try
            {
                _storeEligibilityDataSet = MIDEnvironment.CreateDataSet("storeEligibilityDataSet");

                DataColumn dataColumn;

                DataTable setTable = _storeEligibilityDataSet.Tables.Add("Sets");

                //Create Columns and rows for datatable
                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "SetID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Eligibility RID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Eligibility";
                dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Eligibility);
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "Ineligible";
                dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Ineligible);
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "PMPlusSales";
                dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_PMPlusSales);
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Stock Modifier RID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Stock Modifier";
                dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Stock_Modifier);
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Sales Modifier RID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Sales Modifier";
                dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Sales_Modifier);
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                //BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement
                //dataColumn = new DataColumn();
                //dataColumn.DataType = System.Type.GetType("System.String");
                //dataColumn.ColumnName = "Stock Lead Weeks";
                //dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Stock_Lead_Weeks);
                //dataColumn.ReadOnly = false;
                //dataColumn.Unique = false;
                //setTable.Columns.Add(dataColumn);
                //END TT#44 - MD - DOConnell - New Store Forecasting Enhancement

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "FWOS Modifier RID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "FWOS Modifier";
                dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_FWOS_Modifier);
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "Used Store Selector";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "Similar Store Changed";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Object");
                dataColumn.ColumnName = "SimilarStoresArrayList";
                dataColumn.Caption = "SimilarStoresArrayList";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Similar Store";
                dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Similar_Store);
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Ratio";
                dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Similar_Store_Ratio);
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Until RID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Until";
                dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Similar_Store_Until);
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                //make set ID the primary key
                DataColumn[] PrimaryKeyColumn = new DataColumn[1];
                PrimaryKeyColumn[0] = setTable.Columns["SetID"];
                setTable.PrimaryKey = PrimaryKeyColumn;

                DataTable storeTable = _storeEligibilityDataSet.Tables.Add("Stores");

                //Create Columns and rows for datatable
                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "SetID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "Inherited";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Inherited RID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "Updated";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Store RID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = true;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Store";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Eligibility RID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Eligibility";
                dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Eligibility);
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "Ineligible";
                dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Ineligible);
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "PMPlusSales";
                dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_PMPlusSales);
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "PMPlusSales Inherited";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "PMPlusSales Inherited RID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "Stk Mod Inherited";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Stock Modifier Inherited RID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Stock Modifier RID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Stock Modifier";
                dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Stock_Modifier);
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "Sls Mod Inherited";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Sales Modifier Inherited RID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Sales Modifier RID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Sales Modifier";
                dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Sales_Modifier);
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                //BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement
                //dataColumn = new DataColumn();
                //dataColumn.DataType = System.Type.GetType("System.Boolean");
                //dataColumn.ColumnName = "Stock Lead Weeks Inherited";
                //dataColumn.ReadOnly = false;
                //dataColumn.Unique = false;
                //storeTable.Columns.Add(dataColumn);

                //dataColumn = new DataColumn();
                //dataColumn.DataType = System.Type.GetType("System.Int32");
                //dataColumn.ColumnName = "Stock Lead Weeks Inherited RID";
                //dataColumn.ReadOnly = false;
                //dataColumn.Unique = false;
                //storeTable.Columns.Add(dataColumn);

                //dataColumn = new DataColumn();
                //dataColumn.DataType = System.Type.GetType("System.String");
                //dataColumn.ColumnName = "Stock Lead Weeks";
                //dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Stock_Lead_Weeks);
                //dataColumn.ReadOnly = false;
                //dataColumn.Unique = false;
                //dataColumn.MaxLength = 2;
                //storeTable.Columns.Add(dataColumn);
                //END TT#44 - MD - DOConnell - New Store Forecasting Enhancement

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "FWOS Mod Inherited";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "FWOS Modifier Inherited RID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "FWOS Modifier RID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "FWOS Modifier";
                dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_FWOS_Modifier);
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "Sim Str Inherited";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Similar Store Inherited RID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "Used Store Selector";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "Similar Store Changed";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Object");
                dataColumn.ColumnName = "SimilarStoresArrayList";
                dataColumn.Caption = "SimilarStoresArrayList";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Similar Store";
                dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Similar_Store);
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Ratio";
                dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Similar_Store_Ratio);
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Until RID";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Until";
                dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Similar_Store_Until);
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                _storeEligibilityDataSet.Relations.Add("Stores",
                    _storeEligibilityDataSet.Tables["Sets"].Columns["SetID"],
                    _storeEligibilityDataSet.Tables["Stores"].Columns["SetID"]);

                return _storeEligibilityDataSet;
            }
            catch
            {
                throw;
            }
        }
    }
}
