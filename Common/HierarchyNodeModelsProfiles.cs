using System;
using System.Collections;

using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
	/// <summary>
	/// Used to retrieve and update information about an  model
	/// </summary>
	[Serializable()]
	abstract public class NodeModelProfile : ModelProfile
	{
//		private eChangeType					_modelChangeType;
//		private eLockStatus					_modelLockStatus;
//		private int							_modelRID;
//		private string						_modelID;
		private ArrayList					_modelEntries;
//		private DateTime					_updateDateTime;
		private Hashtable					_modelDateEntries;
		private bool						_containsDynamicDates;
		private bool						_containsStoreDynamicDates;
		private bool						_containsPlanDynamicDates;
		private bool						_containsReoccurringDates;
//		private bool						_needsRebuilt;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeModelProfile(int aKey)
			: base(aKey)
		{
//			_modelChangeType = eChangeType.none;
			_modelEntries = new ArrayList();
			_modelDateEntries = new Hashtable();
		}

//		/// <summary>
//		/// Gets or sets the type of change for the model.
//		/// </summary>
//		public eChangeType ModelChangeType 
//		{
//			get { return _modelChangeType ; }
//			set { _modelChangeType = value; }
//		}
//		/// <summary>
//		/// Gets or sets the status of lock for the model.
//		/// </summary>
//		public eLockStatus ModelLockStatus 
//		{
//			get { return _modelLockStatus ; }
//			set { _modelLockStatus = value; }
//		}
//		/// <summary>
//		/// Gets or sets the record id for the model.
//		/// </summary>
//		public int ModelRID 
//		{
//			get { return _modelRID ; }
//			set { _modelRID = value; }
//		}
//		/// <summary>
//		/// Gets or sets the id of the  model.
//		/// </summary>
//		public string ModelID 
//		{
//			get { return _modelID ; }
//			set { _modelID = value; }
//		}
		/// <summary>
		/// Gets or sets the list of model entries.
		/// </summary>
		/// <remarks>
		/// This is an ArrayList of ModelEntry
		/// </remarks>
		public ArrayList ModelEntries 
		{
			get { return _modelEntries ; }
			set { _modelEntries = value; }
		}
//		/// <summary>
//		/// Gets or sets the date and time stamp of the last time this model was updated.
//		/// </summary>
//		public DateTime	UpdateDateTime 
//		{
//			get { return _updateDateTime ; }
//			set { _updateDateTime = value; }
//		}
//		/// <summary>
//		/// Gets the string value for date and time stamp of the last time this model was updated.
//		/// </summary>
//		public string UpdateDateTimeString 
//		{
//			get 
//			{ 
//				return _updateDateTime.ToShortDateString() + " " + _updateDateTime.ToShortTimeString() ; 
//			}
//		}
		/// <summary>
		/// Gets or sets the list of model date entries.
		/// </summary>
		/// <remarks>
		/// This is a Hashtable of date information for the model.  The key to the entry is the date, the value 
		/// is the information for the date.
		/// </remarks>
		public Hashtable ModelDateEntries 
		{
			get { return _modelDateEntries ; }
			set { _modelDateEntries = value; }
		}
//		/// <summary>
//		/// Gets or sets a flag indentifying if the model date information needs rebuilt before it can be used.
//		/// </summary>
//		public bool NeedsRebuilt 
//		{
//			get { return _needsRebuilt ; }
//			set { _needsRebuilt = value; }
//		}
		/// <summary>
		/// Gets or sets a flag indentifying if the model contains dynamic date information.
		/// </summary>
		/// <remarks>
		/// If the model contains dynamic date information, it must be rebuilt before it can be used if the 
		/// needs rebuilt flag is set.
		/// </remarks>
		public bool ContainsDynamicDates 
		{
			get { return _containsDynamicDates ; }
			set { _containsDynamicDates = value; }
		}
		/// <summary>
		/// Gets or sets a flag indentifying if the model contains dynamic date information relative to the store 
		/// open dates.
		/// </summary>
		/// <remarks>
		/// If the model contains dynamic date information relative to the store open date,
		/// it must be rebuilt each time before it can be used because a change to the store information
		/// is not known 
		/// </remarks>
		public bool ContainsStoreDynamicDates 
		{
			get { return _containsStoreDynamicDates ; }
			set { _containsStoreDynamicDates = value; }
		}
		/// <summary>
		/// Gets or sets a flag indentifying if the model contains dynamic date information relative to the plan dates. 
		/// </summary>
		/// <remarks>
		/// If the model contains dynamic date information relative to the plan date,
		/// it must be rebuilt each time before it can be used.
		/// </remarks>
		public bool ContainsPlanDynamicDates 
		{
			get { return _containsPlanDynamicDates ; }
			set { _containsPlanDynamicDates = value; }
		}
		/// <summary>
		/// Gets or sets a flag indentifying if the model contains reoccurring date information.
		/// </summary>
		/// <remarks>
		/// If the model contains reoccurring date information, it requires special procesing for the reoccurring 
		/// dates.
		/// </remarks>
		public bool ContainsReoccurringDates 
		{
			get { return _containsReoccurringDates ; }
			set { _containsReoccurringDates = value; }
		}
	}

	/// <summary>
	/// Used to retrieve and update information about the entries in a model
	/// </summary>
	[Serializable()]
	abstract public class ModelEntry
	{
		private eChangeType					_modelEntryChangeType;
		private DateRangeProfile			_dateRange;
		private int							_modelEntrySeq;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ModelEntry()
		{
			_modelEntryChangeType = eChangeType.none;
		}

		/// <summary>
		/// Gets or sets the type of change for the model entry.
		/// </summary>
		public eChangeType ModelEntryChangeType 
		{
			get { return _modelEntryChangeType ; }
			set { _modelEntryChangeType = value; }
		}
		/// <summary>
		/// Gets or sets the date range information for the model entry.
		/// </summary>
		public DateRangeProfile DateRange 
		{
			get { return _dateRange ; }
			set { _dateRange = value; }
		}
		public int ModelEntrySeq 
		{
			get { return _modelEntrySeq ; }
			set { _modelEntrySeq = value; }
		}
	}

	/// <summary>
	/// Used to retrieve a list of eligibility models
	/// </summary>
	[Serializable()]
	public class EligModelList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public EligModelList(eProfileType aProfileType)
			: base(aProfileType)
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}

	/// <summary>
	/// Used to retrieve and update information about an eligibility model
	/// </summary>
	[Serializable()]
	public class EligModelProfile : NodeModelProfile
	{
		private ArrayList					_salesEligibilityEntries;
		private ArrayList					_priorityShippingEntries;
		private Hashtable					_salesEligibilityModelDateEntries;
		private Hashtable					_priorityShippingModelDateEntries;
        // Begin TT#2307 - JSmith - Incorrect Stock Values
        private bool _salesEligibilityModelDateEntriesLoadedByStore;
        private bool _salesEligibilityContainsDynamicDates;
        private bool _salesEligibilityContainsStoreDynamicDates;
        private bool _salesEligibilityContainsPlanDynamicDates;
        private bool _salesEligibilityContainsReoccurringDates;
        private bool _salesEligibilityNeedsRebuilt;
        private bool _priorityShippingModelDateEntriesLoadedByStore;
        private bool _priorityShippingContainsDynamicDates;
        private bool _priorityShippingContainsStoreDynamicDates;
        private bool _priorityShippingContainsPlanDynamicDates;
        private bool _priorityShippingContainsReoccurringDates;
        private bool _priorityShippingNeedsRebuilt;
        // End TT#2307
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public EligModelProfile(int aKey)
			: base(aKey)
		{
			_salesEligibilityEntries = new ArrayList();
			_priorityShippingEntries = new ArrayList();
			_salesEligibilityModelDateEntries = new Hashtable();
			_priorityShippingModelDateEntries = new Hashtable();
            // Begin TT#2307 - JSmith - Incorrect Stock Values
            _salesEligibilityModelDateEntriesLoadedByStore = false;
            _salesEligibilityContainsDynamicDates = false;
            _salesEligibilityContainsStoreDynamicDates = false;
            _salesEligibilityContainsPlanDynamicDates = false;
            _salesEligibilityContainsReoccurringDates = false;
            _salesEligibilityNeedsRebuilt = true;
            _priorityShippingModelDateEntriesLoadedByStore = false;
            _priorityShippingContainsDynamicDates = false;
            _priorityShippingContainsStoreDynamicDates = false;
            _priorityShippingContainsPlanDynamicDates = false;
            _priorityShippingContainsReoccurringDates = false;
            _priorityShippingNeedsRebuilt = true;
            // End TT#2307
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.EligibilityModel;
			}
		}

		/// <summary>
		/// Gets or sets the list of stock eligibility model entries.
		/// </summary>
		/// <remarks>
		/// This is an ArrayList of inforamation for the type of model
		/// </remarks>
		public ArrayList SalesEligibilityEntries 
		{
			get { return _salesEligibilityEntries ; }
			set { _salesEligibilityEntries = value; }
		}

		/// <summary>
		/// Gets or sets the list of priority shipping model entries.
		/// </summary>
		/// <remarks>
		/// This is an ArrayList of inforamation for the type of model
		/// </remarks>
		public ArrayList PriorityShippingEntries 
		{
			get { return _priorityShippingEntries ; }
			set { _priorityShippingEntries = value; }
		}
		/// <summary>
		/// Gets or sets the list of model date entries for sales eligibility.
		/// </summary>
		/// <remarks>
		/// This is a Hashtable of date information for the model.  The key to the entry is the date, the value 
		/// is the information for the date.
		/// </remarks>
		public Hashtable SalesEligibilityModelDateEntries 
		{
			get { return _salesEligibilityModelDateEntries ; }
			set { _salesEligibilityModelDateEntries = value; }
		}
		/// <summary>
		/// Gets or sets the list of model date entries for priority shipping.
		/// </summary>
		/// <remarks>
		/// This is a Hashtable of date information for the model.  The key to the entry is the date, the value 
		/// is the information for the date.
		/// </remarks>
		public Hashtable PriorityShippingModelDateEntries 
		{
			get { return _priorityShippingModelDateEntries ; }
			set {_priorityShippingModelDateEntries = value; }
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        /// <summary>
        /// Gets or sets the flag identifying if model date entries for sales eligibility have been loaded by store.
        /// </summary>
        public bool SalesEligibilityModelDateEntriesLoadedByStore
        {
            get { return _salesEligibilityModelDateEntriesLoadedByStore; }
            set { _salesEligibilityModelDateEntriesLoadedByStore = value; }
        }

        /// <summary>
        /// Gets or sets the flag identifying if model date entries for sales eligibility contain dynamic dates.
        /// </summary>
        public bool SalesEligibilityContainsDynamicDates
        {
            get { return _salesEligibilityContainsDynamicDates; }
            set { _salesEligibilityContainsDynamicDates = value; }
        }

        /// <summary>
        /// Gets or sets the flag identifying if model date entries for sales eligibility contain dynamic dates by store.
        /// </summary>
        public bool SalesEligibilityContainsStoreDynamicDates
        {
            get { return _salesEligibilityContainsStoreDynamicDates; }
            set { _salesEligibilityContainsStoreDynamicDates = value; }
        }

        /// <summary>
        /// Gets or sets the flag identifying if model date entries for sales eligibility contain dynamic dates by plan.
        /// </summary>
        public bool SalesEligibilityContainsPlanDynamicDates
        {
            get { return _salesEligibilityContainsPlanDynamicDates; }
            set { _salesEligibilityContainsPlanDynamicDates = value; }
        }

        /// <summary>
        /// Gets or sets the flag identifying if model date entries for sales eligibility contain reoccurring dates.
        /// </summary>
        public bool SalesEligibilityContainsReoccurringDates
        {
            get { return _salesEligibilityContainsReoccurringDates; }
            set { _salesEligibilityContainsReoccurringDates = value; }
        }

        /// <summary>
        /// Gets or sets the flag identifying if model date entries for sales eligibility need rebuild.
        /// </summary>
        public bool SalesEligibilityNeedsRebuilt
        {
            get { return _salesEligibilityNeedsRebuilt; }
            set { _salesEligibilityNeedsRebuilt = value; }
        }

        /// <summary>
        /// Gets or sets the flag identifying if model date entries for priority shipping have been loaded by store.
        /// </summary>
        public bool PriorityShippingModelDateEntriesLoadedByStore
        {
            get { return _priorityShippingModelDateEntriesLoadedByStore; }
            set { _priorityShippingModelDateEntriesLoadedByStore = value; }
        }

        /// <summary>
        /// Gets or sets the flag identifying if model date entries for priority shipping contain dynamic dates.
        /// </summary>
        public bool PriorityShippingContainsDynamicDates
        {
            get { return _priorityShippingContainsDynamicDates; }
            set { _priorityShippingContainsDynamicDates = value; }
        }

        /// <summary>
        /// Gets or sets the flag identifying if model date entries for priority shipping contain dynamic dates by store.
        /// </summary>
        public bool PriorityShippingContainsStoreDynamicDates
        {
            get { return _priorityShippingContainsStoreDynamicDates; }
            set { _priorityShippingContainsStoreDynamicDates = value; }
        }

        /// <summary>
        /// Gets or sets the flag identifying if model date entries for priority shipping contain dynamic dates by plan.
        /// </summary>
        public bool PriorityShippingContainsPlanDynamicDates
        {
            get { return _priorityShippingContainsPlanDynamicDates; }
            set { _priorityShippingContainsPlanDynamicDates = value; }
        }

        /// <summary>
        /// Gets or sets the flag identifying if model date entries for priority shipping contain reoccurring dates.
        /// </summary>
        public bool PriorityShippingContainsReoccurringDates
        {
            get { return _priorityShippingContainsReoccurringDates; }
            set { _priorityShippingContainsReoccurringDates = value; }
        }

        /// <summary>
        /// Gets or sets the flag identifying if model date entries for priority shipping need rebuild.
        /// </summary>
        public bool PriorityShippingNeedsRebuilt
        {
            get { return _priorityShippingNeedsRebuilt; }
            set { _priorityShippingNeedsRebuilt = value; }
        }
        // End TT#2307
	}

	/// <summary>
	/// Used to retrieve and update information about the entries in an eligibility model
	/// </summary>
	[Serializable()]
	public class EligModelEntry : ModelEntry
	{
		private eEligModelEntryType			_eligModelEntryType;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public EligModelEntry()
		{
		}

		/// <summary>
		/// Gets or sets the type of item for the eligibile model entry.
		/// </summary>
		public eEligModelEntryType EligModelEntryType 
		{
			get { return _eligModelEntryType ; }
			set { _eligModelEntryType = value; }
		}
	}

	/// <summary>
	/// Used to retrieve a list of stock modifier models
	/// </summary>
	[Serializable()]
	public class StkModModelList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StkModModelList(eProfileType aProfileType)
			: base(aProfileType)
		{
		}
	}
	
	/// <summary>
	/// Used to retrieve and update information about the entries in a stock modifier model
	/// </summary>
	[Serializable()]
	public class StkModModelProfile : NodeModelProfile
	{
		private double				_stkModModelDefault;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StkModModelProfile(int aKey)
			: base(aKey)
		{
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.StockModifierModel;
			}
		}

		/// <summary>
		/// Gets or sets the default value of the stock modifier model.
		/// </summary>
		public double StkModModelDefault 
		{
			get { return _stkModModelDefault ; }
			set { _stkModModelDefault = value; }
		}
	}

	/// <summary>
	/// Used to retrieve and update information about the entries in a stock modifier model entry
	/// </summary>
	[Serializable()]
	public class StkModModelEntry : ModelEntry
	{
		private double						_stkModModelEntryValue;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StkModModelEntry()
		{
		}
		/// <summary>
		/// Gets or sets the sales modifier entry value of the sales modifier model.
		/// </summary>
		public double StkModModelEntryValue 
		{
			get { return _stkModModelEntryValue ; }
			set { _stkModModelEntryValue = value; }
		}
	}

	/// <summary>
	/// Used to retrieve a list of sales modifier models
	/// </summary>
	[Serializable()]
	public class SlsModModelList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SlsModModelList(eProfileType aProfileType)
			: base(aProfileType)
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}

	/// <summary>
	/// Used to retrieve and update information about the entries in a sales modifier model
	/// </summary>
	[Serializable()]
	public class SlsModModelProfile : NodeModelProfile
	{
		private double						_slsModModelDefault;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SlsModModelProfile(int aKey)
			: base(aKey)
		{
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SalesModifierModel;
			}
		}

		/// <summary>
		/// Gets or sets the default value of the sales modifier model.
		/// </summary>
		public double SlsModModelDefault 
		{
			get { return _slsModModelDefault ; }
			set { _slsModModelDefault = value; }
		}
	}

	/// <summary>
	/// Used to retrieve and update information about the entries in a sales modifier model entry
	/// </summary>
	[Serializable()]
	public class SlsModModelEntry : ModelEntry
	{
		private double						_slsModModelEntryValue;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SlsModModelEntry()
		{
		}

		/// <summary>
		/// Gets or sets the sales modifier entry value of the sales modifier model.
		/// </summary>
		public double SlsModModelEntryValue 
		{
			get { return _slsModModelEntryValue ; }
			set { _slsModModelEntryValue = value; }
		}
		
	}

	/// <summary>
	/// Used to retrieve and update information about the entries in a daily percentages model
	/// </summary>
	[Serializable()]
	public class DailyPctModelProfile : NodeModelProfile
	{

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public DailyPctModelProfile(int aKey)
			: base(aKey)
		{
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.DailyPctModel;
			}
		}
	}

	/// <summary>
	/// Used to retrieve and update information about the entries in a daily percentages model entry
	/// </summary>
	[Serializable()]
	public class DailyPctModelEntry : ModelEntry
	{
		private double						_dailyValue;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public DailyPctModelEntry()
		{
		}
		/// <summary>
		/// Gets or sets the daily value of the entry in the daily percentages model.
		/// </summary>
		public double DailyValue 
		{
			get { return _dailyValue ; }
			set { _dailyValue = value; }
		}
	}

	// BEGIN MID Track #4370 - John Smith - FWOS Models
	/// <summary>
	/// Used to retrieve a list of FWOS modifier models
	/// </summary>
	[Serializable()]
	public class FWOSModModelList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public FWOSModModelList(eProfileType aProfileType)
			: base(aProfileType)
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}

	/// <summary>
	/// Used to retrieve and update information about the entries in a sales modifier model
	/// </summary>
	[Serializable()]
	public class FWOSModModelProfile : NodeModelProfile
	{
		private double						_FWOSModModelDefault;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public FWOSModModelProfile(int aKey)
			: base(aKey)
		{
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.FWOSModifierModel;
			}
		}

		/// <summary>
		/// Gets or sets the default value of the FWOS modifier model.
		/// </summary>
		public double FWOSModModelDefault 
		{
			get { return _FWOSModModelDefault ; }
			set { _FWOSModModelDefault = value; }
		}
	}

	/// <summary>
	/// Used to retrieve and update information about the entries in a FWOS modifier model entry
	/// </summary>
	[Serializable()]
	public class FWOSModModelEntry : ModelEntry
	{
		private double						_FWOSModModelEntryValue;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public FWOSModModelEntry()
		{
		}

		/// <summary>
		/// Gets or sets the FWOS modifier entry value of the FWOS modifier model.
		/// </summary>
		public double FWOSModModelEntryValue 
		{
			get { return _FWOSModModelEntryValue ; }
			set { _FWOSModModelEntryValue = value; }
		}
		
	}
	// END MID Track #4370

    // BEGIN TT#108 - MD - DOConnell - FWOS Max Model Enhancement
    /// <summary>
    /// Used to retrieve a list of FWOS modifier models
    /// </summary>
    [Serializable()]
    public class FWOSMaxModelList : ProfileList
    {
        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public FWOSMaxModelList(eProfileType aProfileType)
            : base(aProfileType)
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }

    /// <summary>
    /// Used to retrieve and update information about the entries in a sales modifier model
    /// </summary>
    [Serializable()]
    public class FWOSMaxModelProfile : NodeModelProfile
    {
        private double _FWOSMaxModelDefault;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public FWOSMaxModelProfile(int aKey)
            : base(aKey)
        {
        }

        /// <summary>
        /// Returns the eProfileType of this profile.
        /// </summary>
        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.FWOSMaxModel;
            }
        }

        /// <summary>
        /// Gets or sets the default value of the FWOS modifier model.
        /// </summary>
        public double FWOSMaxModelDefault
        {
            get { return _FWOSMaxModelDefault; }
            set { _FWOSMaxModelDefault = value; }
        }
    }

    /// <summary>
    /// Used to retrieve and update information about the entries in a FWOS modifier model entry
    /// </summary>
    [Serializable()]
    public class FWOSMaxModelEntry : ModelEntry
    {
        private double _FWOSMaxModelEntryValue;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public FWOSMaxModelEntry()
        {
        }

        /// <summary>
        /// Gets or sets the FWOS modifier entry value of the FWOS modifier model.
        /// </summary>
        public double FWOSMaxModelEntryValue
        {
            get { return _FWOSMaxModelEntryValue; }
            set { _FWOSMaxModelEntryValue = value; }
        }

    }
    // END TT#108 - MD - DOConnell - FWOS Max Model Enhancement

	/// <summary>
	/// Used to retrieve model name
	/// </summary>
	[Serializable()]
	public class ModelName : Profile
	{
		private string						_modelID;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ModelName(int aKey)
			: base(aKey)
		{
			
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.ModelName;
			}
		}

		/// <summary>
		/// Gets or sets the id of the  model.
		/// </summary>
		public string ModelID 
		{
			get { return _modelID ; }
			set { _modelID = value; }
		}
		
	}

	/// <summary>
	/// Used to retrieve a list of model names
	/// </summary>
	[Serializable()]
	public class ModelNameList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ModelNameList(eProfileType aProfileType)
			: base(aProfileType)
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}

	public class ModelNameSort:IComparer
	{
		public ModelNameSort()
		{
		}

		public int Compare(object x,object y)
		{	
			return ((ModelName)x).ModelID.CompareTo(((ModelName)y).ModelID);
				
		}
	}

	#region ModelNameCombo Class
	/// <summary>
	/// Class that defines the contents of the ModelName combo box.
	/// </summary>

	public class ModelNameCombo
	{
		//=======
		// FIELDS
		//=======

		private int _modelRID;
		private string _modelName;
		private object _tag;
		
		//=============
		// CONSTRUCTORS
		//=============

		public ModelNameCombo(int aModelRID, string aModelName)
		{
			_modelRID = aModelRID;
			_modelName = aModelName;
			_tag = null;
		}

		public ModelNameCombo(int aModelRID, string aModelName, object aTag)
		{
			_modelRID = aModelRID;
			_modelName = aModelName;
			_tag = aTag;
		}

		//===========
		// PROPERTIES
		//===========

		public int ModelRID
		{
			get
			{
				return _modelRID;
			}
		}

		public string ModelName
		{
			get
			{
				return _modelName;
			}
		}

		public object Tag
		{
			get
			{
				return _tag;
			}
		}

		//========
		// METHODS
		//========

		override public string ToString()
		{
			return _modelName;
		}

		override public bool Equals(object obj)
		{
			if (((ModelNameCombo)obj).ModelRID == _modelRID)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		override public int GetHashCode()
		{
			return _modelRID;
		}
	}
	#endregion


}
