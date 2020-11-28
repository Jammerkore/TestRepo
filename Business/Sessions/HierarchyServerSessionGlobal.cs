// Begin TT#1239 - JSmith - Overall Performance
// Converted all Hashtable to Dictionary
// Too many lines to mark
// End TT#1239
using System; 
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;


namespace MIDRetail.Business
{
	/// <summary>
	/// HierarchyServerGlobal is a static class that contains fields that are global to all HierarchyServerSession objects.
	/// </summary>
	/// <remarks>
	/// The HierarchyServerGlobal class is used to store information that is global to all HierarchyServerSession objects
	/// within the process.  A common use for this class would be to cache static information from the database in order to
	/// reduce accesses to the database.
	/// </remarks>

	public class HierarchyServerGlobal : Global
	{
		// Fields that are global to the Hierarchy Service

		//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		static private ArrayList _loadLock;
		static private bool _loaded;
		static private Audit _audit;

		//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		static private MIDReaderWriterLock hierarchy_rwl;
		static private ArrayList _colorCode_lock;
		static private ArrayList _sizeCode_lock;
		static private ArrayList _cache_lock;
        static private ArrayList _load_lock;  // TT#4270 - JSmith - History/Forecast Load Failing
		static private ArrayList _colorSizeIDcache_lock;
		static private ArrayList _productChar_lock;
        static private ArrayList _assortment_lock;
		static private MIDReaderWriterLock enqueue_rwl;
		static private MIDReaderWriterLock eligibility_rwl;
		static private MIDReaderWriterLock storeGrades_rwl;
		static private MIDReaderWriterLock stockMinMaxes_rwl;
		static private MIDReaderWriterLock storeCapacity_rwl;
		static private MIDReaderWriterLock velocityGrades_rwl;
		static private MIDReaderWriterLock sellThruPcts_rwl;
		static private MIDReaderWriterLock dailyPercentages_rwl;
		static private MIDReaderWriterLock models_rwl;
		static private MIDReaderWriterLock characteristic_rwl;
		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		static private MIDReaderWriterLock sizeCurveCriteria_rwl;
		static private MIDReaderWriterLock sizeCurveDefaultCriteria_rwl;
		static private MIDReaderWriterLock sizeCurveTolerance_rwl;
		static private MIDReaderWriterLock sizeCurveSimilarStore_rwl;
		//End TT#155 - JScott - Add Size Curve info to Node Properties
		//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
		static private MIDReaderWriterLock sizeOutOfStock_rwl;
		static private MIDReaderWriterLock sizeSellThru_rwl;
		//End TT#483 - JScott - Add Size Lost Sales criteria and processing
        //Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
        static private MIDReaderWriterLock chainSetPercent_rwl;
        private SessionAddressBlock _SAB;
        //End TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
        //BEGIN TT#1401 - Reservation (IMO) Stores - gtaylor
        static private MIDReaderWriterLock imo_rwl;        
        //END TT#1401 - Reservation (IMO) Stores - gtaylor
        static private GlobalOptionsProfile _globalOptions;
        static private Dictionary<int, HierarchyInfo> _hierarchiesByRID = new Dictionary<int, HierarchyInfo>();  //  contains all info by record id
        static private Dictionary<string, int> _hierarchiesByID = new Dictionary<string,int>();  //  xref from name to record id
		static private MIDCache<int, NodeInfo> _nodesByRID;
		static private MIDCache<string, int> _nodesByID;   //  xref from name to record id
		static private MIDCache<string, int> _colorSizesByID;   //  color and size xref from name to record id
		static private bool _colorSizesCacheUsed;
        static private Dictionary<int, NodeEligibilityInfo> _eligibilityByRID = new Dictionary<int, NodeEligibilityInfo>();   //  contains all info by record id 
        static private Dictionary<int, NodeStoreGradesInfo> _storeGradesByRID = new Dictionary<int, NodeStoreGradesInfo>();   //  contains all info by record id 
        static private Dictionary<int, NodeStockMinMaxesInfo> _stockMinMaxesByRID = new Dictionary<int, NodeStockMinMaxesInfo>();   //  contains all info by record id 
        static private Dictionary<int, NodeStoreCapacityInfo> _storeCapacityByRID = new Dictionary<int, NodeStoreCapacityInfo>();   //  contains all info by record id 
        static private Dictionary<int, NodeVelocityGradesInfo> _velocityGradesByRID = new Dictionary<int, NodeVelocityGradesInfo>();   //  contains all info by record id 
        static private Dictionary<int, NodeSellThruPctsInfo> _sellThruPctsByRID = new Dictionary<int, NodeSellThruPctsInfo>();   //  contains all info by record id 
        static private Dictionary<int, NodeDailyPercentagesInfo> _dailyPercentagesByRID = new Dictionary<int, NodeDailyPercentagesInfo>();   //  contains all info by record id 
        static private Dictionary<int, EligModelInfo> _eligModelsByRID = new Dictionary<int, EligModelInfo>();   //  contains all info by record id 
        static private Dictionary<string, int> _eligModelsByID = new Dictionary<string, int>();   //  xref from name to record id
        static private Dictionary<int, SlsModModelInfo> _slsModModelsByRID = new Dictionary<int, SlsModModelInfo>();   //  contains all info by record id 
        static private Dictionary<string, int> _slsModModelsByID = new Dictionary<string, int>();   //  xref from name to record id
        static private Dictionary<int, StkModModelInfo> _stkModModelsByRID = new Dictionary<int, StkModModelInfo>();   //  contains all info by record id 
        static private Dictionary<string, int> _stkModModelsByID = new Dictionary<string, int>();   //  xref from name to record id
        static private Dictionary<int, FWOSModModelInfo> _FWOSModModelsByRID = new Dictionary<int, FWOSModModelInfo>();   //  contains all info by record id 
        static private Dictionary<string, int> _FWOSModModelsByID = new Dictionary<string, int>();   //  xref from name to record id
        //BEGIN TT#108 - MD - DOConnell - FWOS Max Model
        static private Dictionary<int, FWOSMaxModelInfo> _FWOSMaxModelsByRID = new Dictionary<int, FWOSMaxModelInfo>();   //  contains all info by record id 
        static private Dictionary<string, int> _FWOSMaxModelsByID = new Dictionary<string, int>();   //  xref from name to record id
        //End TT#108 - MD - DOConnell - FWOS Max Model
        static private Dictionary<int, ColorCodeInfo> _colorCodesByRID = new Dictionary<int, ColorCodeInfo>();   //  contains all info by record id 
        static private Dictionary<string, int> _colorCodesByID = new Dictionary<string, int>();   //  xref from name to record id
        static private SortedList _colorCodePlaceholders = new SortedList();   //  contains placeholder color codes
        static private Dictionary<int, SizeCodeInfo> _sizeCodesByRID = new Dictionary<int, SizeCodeInfo>();   //  contains all info by record id 
        static private Dictionary<string, int> _sizeCodesByID = new Dictionary<string, int>();   //  xref from name to record id
        static private Dictionary<string, int> _sizeCodesByPriSec = new Dictionary<string, int>();   // xref from Primary/Secondary code to record id
        static private Dictionary<int, ProductCharInfo> _productCharByRID = new Dictionary<int, ProductCharInfo>();
        static private Dictionary<string, int> _productCharByID = new Dictionary<string, int>();
        static private Dictionary<int, ProductCharValueInfo> _productCharValueByRID = new Dictionary<int, ProductCharValueInfo>();
		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
        static private Dictionary<int, NodeSizeCurveCriteriaInfo> _sizeCurveCriteriaByRID = new Dictionary<int, NodeSizeCurveCriteriaInfo>();
        static private Dictionary<int, NodeSizeCurveDefaultCriteriaInfo> _sizeCurveDefaultCriteriaByRID = new Dictionary<int, NodeSizeCurveDefaultCriteriaInfo>();
        static private Dictionary<int, NodeSizeCurveToleranceInfo> _sizeCurveToleranceByRID = new Dictionary<int, NodeSizeCurveToleranceInfo>();
        static private Dictionary<int, NodeSizeCurveSimilarStoreInfo> _sizeCurveSimilarStoreByRID = new Dictionary<int, NodeSizeCurveSimilarStoreInfo>();
		//End TT#155 - JScott - Add Size Curve info to Node Properties
		//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
        //static private Hashtable _sizeOutOfStockByRID = new Hashtable();
        //static private Hashtable _sizeSellThruByRID = new Hashtable();
        static private Dictionary<int, NodeSizeSellThruInfo> _sizeSellThruByRID = new Dictionary<int, NodeSizeSellThruInfo>();
		//End TT#483 - JScott - Add Size Lost Sales criteria and processing
        static private Dictionary<int, int> _includedHierarchies = new Dictionary<int, int>();
		static string[] _parsedIncludeHierarchyName = null;
		static string[] _parsedExcludeHierarchyName = null;
		static private int _mainHierarchyRID = Include.NoRID;
		static private int _lowestLoadLevel;
		static private bool _colorSizeIDsNeedBuilt = true;
		static private eHierarchyLevelType _currentCacheType = eHierarchyLevelType.Undefined;
        static private Dictionary<int, List<int>> _userHierarchies = new Dictionary<int, List<int>>();   //  contains hierarchies for user 
        static string _placeholderColorLabel;
        // Begin TT#2 - RMatelic - Assortment Planning  
        //static string _placeholderStyleLabel;
        static string _placeholderStyleLabel_1;
        static string _placeholderStyleLabel_2;
        // End TT#2  
		//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
		static int _cacheSize;
		static int _cacheGroupSize;
		//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
        //Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
        static private Hashtable _chainSetPercentByRID = new Hashtable();   //  contains all info by record id 
        //static private Hashtable _chainSetPercentByID = new Hashtable();
        //static private Hashtable _chainSetPercentValueByRID = new Hashtable();
        static private WeekProfile _firstWeekOfBasis = null;
        static private WeekProfile _firstWeekOfPlan = null;
        //End TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
        //BEGIN TT#1401 - Reservation (IMO) Stores - gtaylor
        //static private Hashtable _imoByRID = new Hashtable();   //  contains all info by record id 
        static private Dictionary<int, NodeIMOInfo> _imoByRID = new Dictionary<int, NodeIMOInfo>();
        //static private Hashtable _imoByID = new Hashtable();
        //static private Hashtable _imoValueByRID = new Hashtable();
        //END TT#1401 - Reservation (IMO) Stores - gtaylor
        // Begin TT#3523 - JSmith - Performance of Anthro morning processing jobs
        static private DataTable _dtHierarchyCharacteristics;
        // End TT#3523 - JSmith - Performance of Anthro morning processing jobs
        static private Hashtable _htProductCharacteristicsLoaded;  // TT#3558 - JSmith - Perf of Hierarchy Load
        static private Dictionary<string, ColorNodeInfo> _dctColorNodeLookup = new Dictionary<string, ColorNodeInfo>();  // TT#3573 - JSmith - Improve performance loading color level

        /// <summary>
		/// Creates a new instance of HierarchyServerGlobal
		/// </summary>

		static HierarchyServerGlobal()
		{
			string debugSizeStr;
			int debugSize;

			try
			{
				//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				_loadLock = new ArrayList();
				_loaded = false;

				//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				if (!EventLog.SourceExists("MIDHierarchyService"))
				{
					EventLog.CreateEventSource("MIDHierarchyService", null);
				}

				debugSizeStr = MIDConfigurationManager.AppSettings["LockDebugSize"];

				if (debugSizeStr != null)
				{
					debugSize = Convert.ToInt32(debugSizeStr);
					debugSize = Math.Max(100, debugSize);

					hierarchy_rwl = new MIDReaderWriterLock("Hierarchy", debugSize);
					enqueue_rwl = new MIDReaderWriterLock("Enqueue", debugSize);
					eligibility_rwl = new MIDReaderWriterLock("Eligibility", debugSize);
					storeGrades_rwl = new MIDReaderWriterLock("StoreGrades", debugSize);
					stockMinMaxes_rwl = new MIDReaderWriterLock("StockMinMaxes", debugSize);
					storeCapacity_rwl = new MIDReaderWriterLock("StoreCapacity", debugSize);
					velocityGrades_rwl = new MIDReaderWriterLock("VelocityGrades", debugSize);
					sellThruPcts_rwl = new MIDReaderWriterLock("SellThruPcts", debugSize);
					dailyPercentages_rwl = new MIDReaderWriterLock("DailyPercentages", debugSize);
					models_rwl = new MIDReaderWriterLock("Models", debugSize);
					//Begin TT#155 - JScott - Add Size Curve info to Node Properties
					//characteristic_rwl = new MIDReaderWriterLock("Models", debugSize);
					characteristic_rwl = new MIDReaderWriterLock("Characteristic", debugSize);
					sizeCurveCriteria_rwl = new MIDReaderWriterLock("SizeCurveCriteria", debugSize);
					sizeCurveDefaultCriteria_rwl = new MIDReaderWriterLock("SizeCurveDefaultCriteria", debugSize);
					sizeCurveTolerance_rwl = new MIDReaderWriterLock("SizeCurveTolerance", debugSize);
					sizeCurveSimilarStore_rwl = new MIDReaderWriterLock("SizeCurveSimStore", debugSize);
					//End TT#155 - JScott - Add Size Curve info to Node Properties
                    //Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
                    chainSetPercent_rwl = new MIDReaderWriterLock("ChainSetPercent", debugSize);
                    //End TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
                    //BEGIN TT#1401 - Reservation (IMO) Stores - gtaylor
                    imo_rwl = new MIDReaderWriterLock("IMO", debugSize);
                    //END TT#1401 - Reservation (IMO) Stores - gtaylor
					//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
					sizeOutOfStock_rwl = new MIDReaderWriterLock("SizeOutOfStock", debugSize);
					sizeSellThru_rwl = new MIDReaderWriterLock("SizeSellThru", debugSize);
					//End TT#483 - JScott - Add Size Lost Sales criteria and processing
				}
				else
				{
					hierarchy_rwl = new MIDReaderWriterLock("Hierarchy");
					enqueue_rwl = new MIDReaderWriterLock("Enqueue");
					eligibility_rwl = new MIDReaderWriterLock("Eligibility");
					storeGrades_rwl = new MIDReaderWriterLock("StoreGrades");
					stockMinMaxes_rwl = new MIDReaderWriterLock("StockMinMaxes");
					storeCapacity_rwl = new MIDReaderWriterLock("StoreCapacity");
					velocityGrades_rwl = new MIDReaderWriterLock("VelocityGrades");
					sellThruPcts_rwl = new MIDReaderWriterLock("SellThruPcts");
					dailyPercentages_rwl = new MIDReaderWriterLock("DailyPercentages");
					models_rwl = new MIDReaderWriterLock("Models");
					//Begin TT#155 - JScott - Add Size Curve info to Node Properties
					//characteristic_rwl = new MIDReaderWriterLock("Models");
					characteristic_rwl = new MIDReaderWriterLock("Characteristic");
					sizeCurveCriteria_rwl = new MIDReaderWriterLock("SizeCurveCriteria");
					sizeCurveDefaultCriteria_rwl = new MIDReaderWriterLock("SizeCurveDefaultCriteria");
					sizeCurveTolerance_rwl = new MIDReaderWriterLock("SizeCurveTolerance");
					sizeCurveSimilarStore_rwl = new MIDReaderWriterLock("SizeCurveSimStore");
					//End TT#155 - JScott - Add Size Curve info to Node Properties
                    //Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
                    chainSetPercent_rwl = new MIDReaderWriterLock("ChainSetPercent");
                    //End TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
                    //BEGIN TT#1401 - Reservation (IMO) Stores - gtaylor
                    imo_rwl = new MIDReaderWriterLock("IMO");
                    //END TT#1401 - Reservation (IMO) Stores - gtaylor
					//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
					sizeOutOfStock_rwl = new MIDReaderWriterLock("SizeOutOfStock");
					sizeSellThru_rwl = new MIDReaderWriterLock("SizeSellThru");
					//End TT#483 - JScott - Add Size Lost Sales criteria and processing
                }
				// create objects to use for locking
				_colorCode_lock = new ArrayList(1);
				_sizeCode_lock = new ArrayList(1);
                _cache_lock = new ArrayList(1);
                _load_lock = new ArrayList(1);   // TT#4270 - JSmith - History/Forecast Load Failing
				_colorSizeIDcache_lock = new ArrayList(1);
				_productChar_lock = new ArrayList(1);
                _assortment_lock = new ArrayList(1);
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry("MIDHierarchyService", ex.Message, EventLogEntryType.Error);
			}
		}

		/// <summary>
		/// The Load method is called by the service or client to trigger the instantiation of the static HierarchyServerGlobal
		/// object.  
		/// </summary>

        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
        //static public void Load()
        static public void Load(bool aLocal)
        // End TT#189
		{
			try
			{
				//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				//LoadBase(eProcesses.hierarchyService);
				lock (_loadLock.SyncRoot)
				{
					if (!_loaded)
					{
                        //Begin TT#5320-VStuart-deadlock issues-FinishLine
                        if (!aLocal)
                        {
                            MarkRunningProcesses(eProcesses.hierarchyService);  // TT#4483 - JSmith - Modify Control Service start to not update other running services as Unexpected Termination
                        }
                        //End TT#5320-VStuart-deadlock issues-FinishLine

                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
                        //_audit = new Audit(eProcesses.hierarchyService, Include.AdministratorUserRID);
                        if (!aLocal)
                        {
                            _audit = new Audit(eProcesses.hierarchyService, Include.AdministratorUserRID);
                        }
                        // End TT#189

                        // Begin TT#2307 - JSmith - Incorrect Stock Values
                        int messagingInterval = Include.Undefined;
                        object parm = MIDConfigurationManager.AppSettings["MessagingInterval"];
                        if (parm != null)
                        {
                            messagingInterval = Convert.ToInt32(parm);
                        }
                        //LoadBase();
                        LoadBase(eMIDMessageSenderRecepient.hierarchyService, messagingInterval, aLocal, eProcesses.hierarchyService);
                        // End TT#2307
				//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

						string includedHierarchies = MIDConfigurationManager.AppSettings["IncludedHierarchies"];
						if (includedHierarchies != null)
						{
							_parsedIncludeHierarchyName = includedHierarchies.Split(';');

						}
						else
						{
							string excludedHierarchies = MIDConfigurationManager.AppSettings["ExcludedHierarchies"];
							if (excludedHierarchies != null)
							{
								_parsedExcludeHierarchyName = excludedHierarchies.Split(';');
							}
						}

						//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
						//int cacheSize = 500000;
						//string appStr = MIDConfigurationManager.AppSettings["CacheSize"];
						//if (appStr != null)
						//{
						//    cacheSize = Convert.ToInt32(appStr);
						//}

						//int cacheGroupSize = 5000;
						//appStr = MIDConfigurationManager.AppSettings["CacheGroupSize"];
						//if (appStr != null)
						//{
						//    cacheGroupSize = Convert.ToInt32(appStr);
						//}
						_cacheSize = 500000;
						string appStr = MIDConfigurationManager.AppSettings["CacheSize"];
						if (appStr != null)
						{
							_cacheSize = Convert.ToInt32(appStr);
						}

						_cacheGroupSize = 5000;
						appStr = MIDConfigurationManager.AppSettings["CacheGroupSize"];
						if (appStr != null)
						{
							_cacheGroupSize = Convert.ToInt32(appStr);
						}
						//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model

						int colorSizeIDCacheSize = 2000000;
						appStr = MIDConfigurationManager.AppSettings["ColorSizeIDCacheSize"];
						if (appStr != null)
						{
							colorSizeIDCacheSize = Convert.ToInt32(appStr);
						}

						//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
						//_nodesByRID = new MIDCache(cacheSize, cacheGroupSize);
						//_nodesByID = new MIDCache(cacheSize, cacheGroupSize);
						//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
						// Begin TT#189 - RMatelic - Remove excessive entries from the Audit
                        //Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_StartedBuildingGlobal, System.Reflection.MethodBase.GetCurrentMethod().Name);
                        if (Audit != null)
                        {
                            Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_StartedBuildingGlobal, System.Reflection.MethodBase.GetCurrentMethod().Name);
                        } 
                        // End TT#189

                        EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService begin building hierarchies", EventLogEntryType.Information);
						// Get global options
						_globalOptions = new GlobalOptionsProfile(Include.NoRID);
						_globalOptions.LoadOptions();

                        _htProductCharacteristicsLoaded = new Hashtable();  // TT#3558 - JSmith - Perf of Hierarchy Load
                        // Begin TT#3523 - JSmith - Performance of Anthro morning processing jobs
                        MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                        _dtHierarchyCharacteristics = mhd.Hier_Char_Join_Read_All();
                       // End TT#3523 - JSmith - Performance of Anthro morning processing jobs

						BuildColors();
						BuildSizes();
						BuildHierarchies();
						//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
						//BuildBaseNodes(cacheSize);
						BuildBaseNodes(_cacheSize, _cacheGroupSize);
						//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
						HierarchyProfile hp = GetMainHierarchyData();
						Calendar.SetPostingDate(hp.PostingDate);
						BuildEligModels();
						BuildStkModModels();
						BuildSlsModModels();
						BuildFWOSModModels();
						BuildProductChars();
                        BuildFWOSMaxModels();   //TT#108 - MD - DOConnell - FWOS Max Models

						if (colorSizeIDCacheSize == 0)
						{
							_colorSizesCacheUsed = false;
						}
						else
						{
							if (ColorOrSizeLevelDefined())
							{
								_colorSizesCacheUsed = true;
							}
						}
						if (_colorSizesCacheUsed)
						{
							//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
							//_colorSizesByID = new MIDCache(colorSizeIDCacheSize, cacheGroupSize);
							_colorSizesByID = new MIDCache<string,int>(colorSizeIDCacheSize, _cacheGroupSize);
							//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
						}
						BuildColorSizeIDs(eHierarchyLevelType.Undefined);
						_placeholderColorLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_PhColorID);
                       
                        // Begin TT#2 - RMatelic - Assortment Planning - change Placeholder Style ID 
                        //_placeholderStyleLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_PhStyleID);
                        _placeholderStyleLabel_1 = MIDText.GetTextOnly(eMIDTextCode.lbl_PhStyleID);
                        _placeholderStyleLabel_2 = MIDText.GetTextOnly(eMIDTextCode.lbl_PhStyleID_2);
                        // End TT#2

						SetInitialMemoryCounts();
						EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService completed building hierarchies", EventLogEntryType.Information);
                       
                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
                        //Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_FinishedBuildingGlobal, System.Reflection.MethodBase.GetCurrentMethod().Name);
                        if (Audit != null)
                        {
                            Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_FinishedBuildingGlobal, System.Reflection.MethodBase.GetCurrentMethod().Name);
                        }
                        // End TT#189

				//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

                        // Begin TT#2307 - JSmith - Incorrect Stock Values
                        MessageProcessor.Messaging.OnMessageSentHandler += new MessageEvent.MessageEventHandler(Messaging_OnMessageSentHandler);
                        if (Audit != null)
                        {
                            string message = MIDText.GetText(eMIDTextCode.msg_MessageListenerStarted, MessageProcessor.Recepient.ToString());
                            Audit.Add_Msg(eMIDMessageLevel.Information, message, System.Reflection.MethodBase.GetCurrentMethod().Name, true);
                        }
                        StartMessageListener();
                        // End TT#2307

                        // Begin TT#195 MD - JSmith - Add environment authentication
                        if (!aLocal)
                        {
                            RegisterServiceStart();
                        }
                        // End TT#195 MD

						_loaded = true;
					}
				}
				//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry("MIDHierarchyService", ex.Message, EventLogEntryType.Information);
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}
				throw;
			}
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        static void Messaging_OnMessageSentHandler(object source, MessageEventArgs e)
        {
            try
            {
                bool successful = false;

                switch (e.MessageCode)
                {
                    case eMIDMessageCode.StoreOpenDateUpdated:
                        successful = StoreOpenDateChanged();
                        break;

                    default:
                        break;
                }

                if (Audit != null)
                {
                    string message = MIDText.GetText(eMIDTextCode.msg_MessageReceived, e.MessageCode.ToString(), e.MessageFrom.ToString(), e.MessageProcessingPriority.ToString(), successful.ToString());
                    Audit.Add_Msg(eMIDMessageLevel.Information, message, System.Reflection.MethodBase.GetCurrentMethod().Name, true);
                }

                RemoveMessage(e.MessageRID);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Store Open Date has changed.
        /// </summary>
        static public bool StoreOpenDateChanged()
        {
            bool successful = false;
            try
            {
               models_rwl.AcquireWriterLock(WriterLockTimeOut);
                try
                {
                    // reset all models that reference a store open date
                    foreach (EligModelInfo eligModel in _eligModelsByRID.Values)
                    {
                        if (eligModel.ContainsStoreDynamicDates)
                        {
                            eligModel.ModelDateEntriesLoadedByStore = false;
                        }
                        if (eligModel.SalesEligibilityContainsStoreDynamicDates)
                        {
                            eligModel.SalesEligibilityModelDateEntriesLoadedByStore = false;
                        }
                        if (eligModel.PriorityShippingContainsStoreDynamicDates)
                        {
                            eligModel.PriorityShippingModelDateEntriesLoadedByStore = false;
                        }
                    }
                    foreach (SlsModModelInfo slsModModel in _slsModModelsByRID.Values)
                    {
                        if (slsModModel.ContainsStoreDynamicDates)
                        {
                            slsModModel.ModelDateEntriesLoadedByStore = false;
                        }
                    }
                    foreach (StkModModelInfo stkModModel in _stkModModelsByRID.Values)
                    {
                        if (stkModModel.ContainsStoreDynamicDates)
                        {
                            stkModModel.ModelDateEntriesLoadedByStore = false;
                        }
                    }
                    foreach (FWOSModModelInfo fwoskModModel in _FWOSModModelsByRID.Values)
                    {
                        if (fwoskModModel.ContainsStoreDynamicDates)
                        {
                            fwoskModModel.ModelDateEntriesLoadedByStore = false;
                        }
                    }

                    //BEGIN TT#3828 - DOConnell - FWOS Max Model should allow dates dynamic to Store Open dates.
                    foreach (FWOSMaxModelInfo fwosMaxModel in _FWOSMaxModelsByRID.Values)
                    {
                        if (fwosMaxModel.ContainsStoreDynamicDates)
                        {
                            fwosMaxModel.ModelDateEntriesLoadedByStore = false;
                        }
                    }
                    //END TT#3828 - DOConnell - FWOS Max Model should allow dates dynamic to Store Open dates.

                    successful = true;
                }
                catch (Exception ex)
                {
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    EventLog.WriteEntry("MIDHierarchyService", "StoreOpenDateChanged error:" + ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    // Ensure that the lock is released.
                    models_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:StoreOpenDateChanged writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:StoreOpenDateChanged writer lock has timed out");
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }

            return successful;
        }
        // End TT#2307 

        // Begin TT#1243 - JSmith - Audit Performance
        /// <summary>
        /// Clean up the global resources
        /// </summary>
        static public void CloseAudit()
        {
            try
            {
				// Begin TT#1303 - stodd - null ref
				if (Audit != null)
				{
					Audit.CloseUpdateConnection();
				}
				// End TT#1303 - stodd - null ref
            }
            catch
            {
                throw;
            }
        }
        // End TT#1243

		//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		/// <summary>
		/// Cleans up all resources for the service
		/// </summary>

		static public void CleanUp()
		{
			try
			{
                // Begin TT#2307 - JSmith - Incorrect Stock Values
                if (isExecutingLocal &&
                    MessageProcessor.isListeningForMessages)
                {
                    StopMessageListener();
                }
                // End TT#2307

				if (Audit != null)
				{
					Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", Audit.HighestMessageLevel);
                    // Begin TT#1243 - JSmith - Audit Performance
                    Audit.CloseUpdateConnection();
                    // End TT#1243
				}
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}
			}
		}

		//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

        //  BEGIN TT#2015 - gtaylor - apply changes to lower levels
        //  clear the caches specified and re-instantiate
        static public void ClearCache(Dictionary<int, Dictionary<long, object>> nodeChanges)
        {
            try
            {
                if (nodeChanges.ContainsKey((int)eProfileType.IMO))
                {
                    //  clear IMO caches
                    _imoByRID.Clear();
                    //_imoByRID = new Hashtable();
                    _imoByRID = new Dictionary<int, NodeIMOInfo>();
                }
                if (nodeChanges.ContainsKey((int)eProfileType.StoreEligibility))
                {
                    //  clear Sotre Eligibility caches
                    _eligibilityByRID.Clear();
                    //_eligibilityByRID = new Hashtable();
                    _eligibilityByRID = new Dictionary<int, NodeEligibilityInfo>(); 
                }
                if (nodeChanges.ContainsKey((int)eProfileType.StoreCharacteristics))
                {
                    //  clear Store Characteristics caches
                    //  this is wrapped up in
                    //static private MIDCache _nodesByRID;
                    //static private MIDCache _nodesByID;   //  xref from name to record id

                }
                if (nodeChanges.ContainsKey((int)eProfileType.StoreCapacity))
                {
                    //  clear Store Capacity caches
                    _storeCapacityByRID.Clear();
                    //_storeCapacityByRID = new Hashtable();
                    _storeCapacityByRID = new Dictionary<int, NodeStoreCapacityInfo>();
                }
                if (nodeChanges.ContainsKey((int)eProfileType.DailyPercentages))
                {
                    //  clear Daily Percentages caches
                    _dailyPercentagesByRID.Clear();
                    //_dailyPercentagesByRID = new Hashtable();
					_dailyPercentagesByRID = new Dictionary<int, NodeDailyPercentagesInfo>();
                }
                if (nodeChanges.ContainsKey((int)eProfileType.PurgeCriteria))
                {
                    //  clear Purge Criteria caches
                    //  this is wrapped up in
                    //static private MIDCache _nodesByRID;
		            //static private MIDCache _nodesByID;   //  xref from name to record id
                }
                if (nodeChanges.ContainsKey((int)eProfileType.ChainSetPercent))
                {
                    //  clear Chain Set Percent caches
                    _chainSetPercentByRID.Clear();
                    _chainSetPercentByRID = new Hashtable();
                }
            }
            catch
            {
                throw;
            }
        }
        //  END TT#2015 - gtaylor - apply changes to lower levels

		//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
		static public void ClearNodeCache()
		{
			try
			{
				BuildBaseNodes(_cacheSize, _cacheGroupSize);
			}
			catch
			{
				throw;
			}
		}

		//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
		static public bool ColorSizesCacheUsed()
		{
			return _colorSizesCacheUsed;
		}

		static private bool ColorOrSizeLevelDefined()
		{
			try
			{
				bool colorOrSizeDefined = false;
				HierarchyProfile hp = GetMainHierarchyData();
				// if hierarchy not defined, return true incase they are loading colors or sizes
				if (hp.Key == Include.NoRID)
				{
					return true;
				}
				else
				{
					for (int level = 1; level <= hp.HierarchyLevels.Count; level++) // levels begin at 1 since the hierarchy occupies level 0
					{
						HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[level];
						if (hlp.LevelType == eHierarchyLevelType.Color ||
							hlp.LevelType == eHierarchyLevelType.Size)
						{
							colorOrSizeDefined = true;
							break;
						}
					}
				}
				return colorOrSizeDefined;
			}
			catch
			{
				throw;
			}
		}

		static public void BuildColorSizeIDs(eHierarchyLevelType aHierarchyLevelType)
		{
			try
			{
				bool colorSizeIDsNeedLoaded = false; 
				lock (_colorSizeIDcache_lock.SyncRoot)
				{
					if (GetColorSizeRIDByIDCount() >= GetColorSizeRIDByIDNumEntries() &&
						_currentCacheType != aHierarchyLevelType)
					{
						colorSizeIDsNeedLoaded = true;
					}

					_currentCacheType = aHierarchyLevelType;
					
					if (_colorSizeIDsNeedBuilt)
					{
						colorSizeIDsNeedLoaded = true;
						bool rebuildIDs = false;
						string appStr = MIDConfigurationManager.AppSettings["RebuildIDs"];
						if (appStr != null)
						{
							try
							{
								rebuildIDs = Convert.ToBoolean(appStr);
							}
							catch
							{
							}
						}

						if (rebuildIDs)
						{
							MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
							try
							{
								mhd.OpenUpdateConnection();
								mhd.DropColorSizeIndexes();
								mhd.CommitData();
							}
							catch (DatabaseNotInCatalog)
							{
								// this error is ok so swallow the exception
							}
							catch
							{
								throw;
							}
							finally
							{
								if (mhd != null &&
									mhd.ConnectionIsOpen)
								{
									mhd.CloseUpdateConnection();
								}
							}
							try
							{
								mhd.OpenUpdateConnection();
								mhd.BuildColorSizeIDs();
								mhd.CommitData();
							}
							catch
							{
								throw;
							}
							finally
							{
								if (mhd != null &&
									mhd.ConnectionIsOpen)
								{
									mhd.CloseUpdateConnection();
								}
							}
							try
							{
								mhd.OpenUpdateConnection();
								mhd.BuildColorSizeIndexes();
								mhd.CommitData();
							}
							catch
							{
								throw;
							}
							finally
							{
								if (mhd != null &&
									mhd.ConnectionIsOpen)
								{
									mhd.CloseUpdateConnection();
								}
							}
						}
						_colorSizeIDsNeedBuilt = false;
					}
					if (_colorSizesCacheUsed &&
						colorSizeIDsNeedLoaded)
					{
						ClearColorSizeRIDByID();
						switch (aHierarchyLevelType)
						{
							case eHierarchyLevelType.Color:
								BuildColorIDs(GetColorSizeRIDByIDNumEntries());
								if (GetColorSizeRIDByIDCount() < GetColorSizeRIDByIDNumEntries())
								{
									BuildSizeIDs(GetColorSizeRIDByIDNumEntries() - GetColorSizeRIDByIDCount());
								}
								break;
							default:
								BuildSizeIDs(GetColorSizeRIDByIDNumEntries());
								if (GetColorSizeRIDByIDCount() < GetColorSizeRIDByIDNumEntries())
								{
									BuildColorIDs(GetColorSizeRIDByIDNumEntries() - GetColorSizeRIDByIDCount());
								}
								break;
						}
						System.GC.Collect();
					}
				}
			}
			catch
			{
				throw;
			}
		}

		// Properties to access the global fields

		//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		static private Audit Audit
		{
			get
			{
				return _audit;
			}
		}

		static public bool Loaded
		{
			get
			{
				return _loaded;
			}
		}

		//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		/// <summary>
		/// Gets the HierarchyInfo object for the requested index.
		/// </summary>
		/// 
		public HierarchyInfo this[int HiIdx]
		{
			get
			{
				return (HierarchyInfo)_hierarchiesByRID[HiIdx];
			}
		}

		/// <summary>
		/// Requests a lock on the enqueue resource
		/// </summary>
		static public void AcquireEnqueueWriterLock()
		{
			try
			{
				enqueue_rwl.AcquireWriterLock(WriterLockTimeOut);		
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:AcquireEnqueueWriterLock writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:AcquireEnqueueWriterLock writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
              	throw;
			}
		}

		/// <summary>
		/// Releases a lock on the enqueue resource
		/// </summary>
		static public void ReleaseEnqueueWriterLock()
		{
			try
			{
				enqueue_rwl.ReleaseWriterLock();		
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
               	// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:ReleaseEnqueueWriterLock writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:ReleaseEnqueueWriterLock writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Reads the base nodes from the database and builds nodeInfo objects for each node.  
		/// </summary>
		/// <remarks>
		/// Also constructs two cross-references.  
		/// One from node record id to a nodeInfo object and the other from the node ID to the node record id. 
		/// These nodes do not include color and size.  They are built on demand.
		/// </remarks>
		//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
		//static private void BuildBaseNodes(int aCacheSize)
		static private void BuildBaseNodes(int aCacheSize, int aCacheGroupSize)
		//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
		{
			NodeInfo ni;
			bool errorFound = false;

			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = null;
				int numNodes = 0;
				if (_includedHierarchies.Count == 0)
				{
					// SQL Server can read all rows faster than a set of rows
					dt = mhd.Hierarchy_Node_Read();
				}
				else
				{
					dt = mhd.Hierarchy_Node_Read(_includedHierarchies);
				}

				//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
				lock (_cache_lock.SyncRoot)
				{
					_nodesByRID = new MIDCache<int, NodeInfo>(aCacheSize, aCacheGroupSize);
					_nodesByID = new MIDCache<string,int>(aCacheSize, aCacheGroupSize);
				}

				//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
				foreach (DataRow dr in dt.Rows)
				{
					errorFound = false;
					int NodeRID				= Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
					string NodeID				= (string)dr["HN_ID"];
					int HomeHierarchyRID		= Convert.ToInt32(dr["HOME_PH_RID"], CultureInfo.CurrentUICulture);
					int HomeHierarchyLevel	= Convert.ToInt32(dr["HOME_LEVEL"], CultureInfo.CurrentUICulture);

					ni = null;
					// build node if room left in cache area.  Leave space for some nodes
					if (_nodesByRID.Count < _nodesByRID.NumEntries - _nodesByRID.ExpireGroupSize)
					{
						++numNodes;
                        ni = new NodeInfo();
                        BuildNodeInfoFromDataRow(ref ni, dr, nodeInfoTypeEnum.Base); //TT#827-MD -jsobek -Allocation Reviews Performance
					}
					
					// update that the level has nodes
					if (HomeHierarchyLevel > 0)
					{
						HierarchyInfo hi = (HierarchyInfo)_hierarchiesByRID[HomeHierarchyRID];
						if (hi == null)
						{
							errorFound = true;
						}
						else
						if (hi.HierarchyType == eHierarchyType.organizational)
						{
							HierarchyLevelInfo hli = (HierarchyLevelInfo)hi.HierarchyLevels[HomeHierarchyLevel];
							//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
							//++hli.LevelNodeCount;
							hli.LevelNodesExist = true;
							//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
							hi.HierarchyLevels[HomeHierarchyLevel] = hli;
						}
					}

					if (errorFound)
					{
                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                        if (Audit != null)
                        {
                            Audit.Add_Msg(eMIDMessageLevel.Warning, "Error loading node " + NodeRID.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                        }
                        else
                        {
                            string message = "Error loading node " + NodeRID.ToString();
                            EventLog.WriteEntry("MIDHierarchyService", message, EventLogEntryType.Warning);
                        }
                        // End TT#189
                        continue;
					}

					if (ni != null)
					{
						SetNodeCacheByRID(NodeRID, ni);
					}
					SetNodeRIDByID(NodeID, NodeRID);
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
               	throw;
			}
		}

		/// <summary>
		/// Reads the styles nodes for a parent from the database and builds nodeInfo objects for each node.
		/// </summary>
		/// <param name="hierarchyRID">The record ID of the hierarchy</param>
		/// <param name="nodeRID">The record ID of the node</param>
		/// <param name="aLoadSiblings">This flag identifies if the siblings of the style should also be loaded</param>
		static private void BuildStyles(int hierarchyRID, int nodeRID, bool aLoadSiblings)
		{
			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = null;
				if (aLoadSiblings)
				{
					dt = mhd.Hierarchy_ChildNodes_Read(hierarchyRID, nodeRID);
				}
				else
				{
					dt = mhd.Hierarchy_Node_Read(nodeRID);
				}

                AddToNodeInfoCacheFromDataTable(dt, nodeInfoTypeEnum.Base);  //TT#827-MD -jsobek -Allocation Reviews Performance
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
              	throw;
			}
		}

		/// <summary>
		/// Adds all color node IDs to the hashtable.
		/// </summary>
		/// <param name="aAvailableEntries">The number of unused entries in the cache</param>
		static private void BuildColorIDs(int aAvailableEntries)
		{
			try
			{
				char nodeDelimiter = GetProductLevelDelimiter();
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.GetColorIDs(aAvailableEntries);
				foreach(DataRow dr in dt.Rows)
				{
					// ID not built so exit.
					if (dr["STYLE_NODE_ID"] == DBNull.Value ||
						dr["COLOR_CODE_ID"] == DBNull.Value)
					{
						break;
					}
					int nodeRID				= Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
					string styleID			= (string)dr["STYLE_NODE_ID"];
					string colorID			= (string)dr["COLOR_CODE_ID"];
					string ID = styleID + nodeDelimiter + colorID;
					
					if (GetColorSizeRIDByIDCount() < GetColorSizeRIDByIDNumEntries())
					{
						SetColorSizeRIDByID(ID, nodeRID);
					}
					else
					{
						break;
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Adds all size node IDs to the hashtable.
		/// </summary>
		/// <param name="aAvailableEntries">The number of unused entries in the cache</param>
		static private void BuildSizeIDs(int aAvailableEntries)
		{
			try
			{
				char nodeDelimiter = GetProductLevelDelimiter();
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.GetSizeIDs(aAvailableEntries);
				foreach(DataRow dr in dt.Rows)
				{
					// ID not built so exit.
					if (dr["STYLE_NODE_ID"] == DBNull.Value ||
						dr["COLOR_NODE_ID"] == DBNull.Value ||
						dr["SIZE_CODE_ID"] == DBNull.Value)
					{
						break;
					}
					int nodeRID				= Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
					string styleID			= (string)dr["STYLE_NODE_ID"];
					string colorID			= (string)dr["COLOR_NODE_ID"];
					string sizeID			= (string)dr["SIZE_CODE_ID"];
					string ID = styleID + nodeDelimiter + colorID + nodeDelimiter + sizeID;
					
					if (GetColorSizeRIDByIDCount() < GetColorSizeRIDByIDNumEntries())
					{
						SetColorSizeRIDByID(ID, nodeRID);
					}
					else
					{
						break;
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
                //Audit.Log_Exception(ex);
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Adds characteristics to the NodeInfo object.
		/// </summary>
		/// <param name="aNodeInfo">An instance of the NodeInfo class</param>
		static private void LoadCharacteristics(NodeInfo aNodeInfo)
		{
			try
			{
				if (aNodeInfo.ProductCharsLoaded)
				{
					return;
				}
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.Hier_Char_Join_Read(aNodeInfo.NodeRID);
				aNodeInfo.ClearCharValues();
				foreach(DataRow dr in dt.Rows)
				{
					aNodeInfo.AddCharValue( Convert.ToInt32(dr["HC_RID"], CultureInfo.CurrentUICulture));
				}
				aNodeInfo.ProductCharsLoaded = true;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
            	throw;
			}
		}

        // Begin TT#3523 - JSmith - Performance of Anthro morning processing jobs
        /// <summary>
        /// Adds characteristics to the NodeInfo object.
        /// </summary>
        /// <param name="aNodeInfo">An instance of the NodeInfo class</param>
        static private void LoadCharacteristicsFromDataTable(NodeInfo aNodeInfo)
        {
            try
            {
                if (aNodeInfo.ProductCharsLoaded)
                {
                    return;
                }
                aNodeInfo.ClearCharValues();
                // Begin TT#3558 - JSmith - Perf of Hierarchy Load
                //DataRow[] rows = _dtHierarchyCharacteristics.Select("HN_RID=" + aNodeInfo.NodeRID);
                //foreach (DataRow dr in rows)
                //{
                //    aNodeInfo.AddCharValue(Convert.ToInt32(dr["HC_RID"], CultureInfo.CurrentUICulture));
                //}
                // check to see if already loaded key once and was swapped out of cache.  If so, load from database.
                if (_htProductCharacteristicsLoaded.ContainsKey(aNodeInfo.NodeRID))
                {
                    LoadCharacteristics(aNodeInfo);
                }
                else
                {
                    DataRow[] rows = _dtHierarchyCharacteristics.Select("HN_RID=" + aNodeInfo.NodeRID);
                    foreach (DataRow dr in rows)
                    {
                        aNodeInfo.AddCharValue(Convert.ToInt32(dr["HC_RID"], CultureInfo.CurrentUICulture));
                    }
                    _htProductCharacteristicsLoaded.Add(aNodeInfo.NodeRID, null);
                }
                // End TT#3558 - JSmith - Perf of Hierarchy Load
                aNodeInfo.ProductCharsLoaded = true;
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }
        // End TT#3523 - JSmith - Performance of Anthro morning processing jobs

		/// <summary>
		/// Reads the node and builds nodeInfo object for the node.
		/// </summary>
		/// <param name="aNodeRID">The record ID of the node</param>
		static private NodeInfo LoadNode(int aNodeRID)
		{
			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				switch (mhd.Hierarchy_Node_Type(aNodeRID))
				{
					case eHierarchyLevelType.Color:
						CheckLoadChildren(aNodeRID, true);
						break;
					case eHierarchyLevelType.Size:
						CheckLoadChildren(aNodeRID, true);
						break;
					default:
						LoadBaseNode(aNodeRID);
						break;
				}

				return GetNodeCacheByRID(aNodeRID);
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Reads the node and builds nodeInfo object for the node.
		/// </summary>
		/// <param name="nodeRID">The record ID of the node</param>
		static private NodeInfo LoadBaseNode(int nodeRID)
		{			
			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				NodeInfo ni = null;
				// make sure the node is not already loaded
				ni = GetNodeCacheByRID(nodeRID);

				// node is not loaded, so load it
				if (ni == null ||
					ni.NodeRID == Include.NoRID)
				{
					DataTable dt = mhd.Hierarchy_Node_Read(nodeRID);

                    AddToNodeInfoCacheFromDataTable(dt, nodeInfoTypeEnum.Base); //TT#827-MD -jsobek -Allocation Reviews Performance
				}
				return ni;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        private enum nodeInfoTypeEnum
        {
            Base=0,
            Color=1,
            Size=2,
            PurgeInfo=3
        }

        //Begin TT#827-MD -jsobek -Allocation Reviews Performance
        private static void AddToNodeInfoCacheFromDataTable(DataTable dt, nodeInfoTypeEnum nodeInfoType)
        {
            foreach(DataRow dr in dt.Rows)
			{
				NodeInfo ni = new NodeInfo();
                BuildNodeInfoFromDataRow(ref ni, dr, nodeInfoType);		            
				SetNodeCacheByRID(ni.NodeRID, ni);
			}
        }

        private static void BuildNodeInfoFromDataRow(ref NodeInfo ni, DataRow dr, nodeInfoTypeEnum nodeInfoType)
        {
            ni.NodeRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
            ni.HomeHierarchyRID = Convert.ToInt32(dr["HOME_PH_RID"], CultureInfo.CurrentUICulture);
            ni.HomeHierarchyLevel = Convert.ToInt32(dr["HOME_LEVEL"], CultureInfo.CurrentUICulture);
            ni.LevelType = (eHierarchyLevelType)(Convert.ToInt32(dr["LEVEL_TYPE"], CultureInfo.CurrentUICulture));

            if (nodeInfoType == nodeInfoTypeEnum.Base)
            {
                ni.NodeID = (string)dr["HN_ID"];
                ni.NodeName = (string)dr["HN_NAME"];
                ni.NodeDescription = (string)dr["DESCRIPTION"];
                ni.ProductType = (eProductType)(Convert.ToInt32(dr["PRODUCT_TYPE"], CultureInfo.CurrentUICulture));
            }
            else if (nodeInfoType == nodeInfoTypeEnum.Color)
            {
                ni.ColorOrSizeCodeRID = Convert.ToInt32(dr["COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
                ni.NodeID = (string)dr["COLOR_CODE_ID"];
                ni.NodeName = (string)dr["COLOR_CODE_NAME"];
                ni.NodeDescription = (string)dr["COLOR_DESCRIPTION"];
                ni.ProductType = eProductType.Undefined;
            }
            else if (nodeInfoType == nodeInfoTypeEnum.Size)
            {
                ni.ColorOrSizeCodeRID = Convert.ToInt32(dr["SIZE_CODE_RID"], CultureInfo.CurrentUICulture);
                ni.NodeID = (string)dr["SIZE_CODE_ID"];
                int sizeCodeRID = (int)_sizeCodesByID[ni.NodeID];
                SizeCodeInfo sci = GetSizeCodeInfoByRID(sizeCodeRID);
                ni.NodeName = sci.SizeCodeName;
                ni.NodeDescription = sci.SizeCodeName;

                ni.ProductType = eProductType.Undefined;
            }
            else if (nodeInfoType == nodeInfoTypeEnum.PurgeInfo)
            {
                ni.ProductType = eProductType.Undefined;
            }

            if (ni.ProductType == eProductType.Undefined)
            {
                ni.ProductTypeIsOverridden = false;
            }
            else
            {
                ni.ProductTypeIsOverridden = true;
            }

            ni.OTSPlanLevelType = (eOTSPlanLevelType)(Convert.ToInt32(dr["OTS_PLANLEVEL_TYPE"], CultureInfo.CurrentUICulture));

            if (ni.OTSPlanLevelType == eOTSPlanLevelType.Undefined)
            {
                ni.OTSPlanLevelTypeIsOverridden = false;
            }
            else
            {
                ni.OTSPlanLevelTypeIsOverridden = true;
            }

            ni.OTSPlanLevelSelectType = (ePlanLevelSelectType)Convert.ToInt32(dr["OTS_FORECAST_LEVEL_SELECT_TYPE"], CultureInfo.CurrentUICulture);
            ni.OTSPlanLevelLevelType = (ePlanLevelLevelType)Convert.ToInt32(dr["OTS_FORECAST_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
            ni.OTSPlanLevelHierarchyRID = Convert.ToInt32(dr["OTS_FORECAST_LEVEL_PH_RID"], CultureInfo.CurrentUICulture);
            ni.OTSPlanLevelHierarchyLevelSequence = Convert.ToInt32(dr["OTS_FORECAST_LEVEL_PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
            ni.OTSPlanLevelAnchorNode = Convert.ToInt32(dr["OTS_FORECAST_LEVEL_ANCHOR_NODE"], CultureInfo.CurrentUICulture);
            if (dr["OTS_FORECAST_LEVEL_MASK"] == DBNull.Value)
            {
                ni.OTSPlanLevelMaskField = eMaskField.Undefined;
                ni.OTSPlanLevelMask = null;
            }
            else
            {
                ni.OTSPlanLevelMaskField = (eMaskField)Convert.ToInt32(dr["OTS_FORECAST_LEVEL_MASK_FIELD"], CultureInfo.CurrentUICulture);
                ni.OTSPlanLevelMask = Convert.ToString(dr["OTS_FORECAST_LEVEL_MASK"], CultureInfo.CurrentUICulture);
            }

            if (ni.OTSPlanLevelHierarchyRID == Include.NoRID || ni.OTSPlanLevelSelectType == ePlanLevelSelectType.Undefined)
            {
                ni.OTSPlanLevelIsOverridden = false;
            }
            else
            {
                ni.OTSPlanLevelIsOverridden = true;

                if (ni.OTSPlanLevelHierarchyLevelSequence == Include.Undefined)
                {
                    ni.OTSPlanLevelHierarchyLevelSequence = 0;
                }
            }

            if (dr["USE_BASIC_REPLENISHMENT"] == DBNull.Value)
            {
                ni.UseBasicReplenishment = false;
            }
            else
                if (Convert.ToInt32(dr["USE_BASIC_REPLENISHMENT"], CultureInfo.CurrentUICulture) == 0)
                {
                    ni.UseBasicReplenishment = false;
                }
                else
                {
                    ni.UseBasicReplenishment = true;
                }

            ni.PurgeDailyHistoryAfter = Convert.ToInt32(dr["PURGE_DAILY_HISTORY"], CultureInfo.CurrentUICulture);
            ni.PurgeWeeklyHistoryAfter = Convert.ToInt32(dr["PURGE_WEEKLY_HISTORY"], CultureInfo.CurrentUICulture);
            ni.PurgeOTSPlansAfter = Convert.ToInt32(dr["PURGE_PLANS"], CultureInfo.CurrentUICulture);
            // Begin TT#400-MD - JSmith - Add Header Purge criteria by Header Type
            //ni.PurgeHeadersAfter	= Convert.ToInt32(dr["PURGE_HEADERS"], CultureInfo.CurrentUICulture);
            //ni = AddNodeHeaderPurge(ni);
            ni.PurgeHtASNAfter = Convert.ToInt32(dr["PURGE_HEADERS_WEEKS_ASN"]);
            ni.PurgeHtDropShipAfter = Convert.ToInt32(dr["PURGE_HEADERS_WEEKS_DROPSHIP"]);
            ni.PurgeHtDummyAfter = Convert.ToInt32(dr["PURGE_HEADERS_WEEKS_DUMMY"]);
            ni.PurgeHtPurchaseOrderAfter = Convert.ToInt32(dr["PURGE_HEADERS_WEEKS_PO"]);
            ni.PurgeHtReceiptAfter = Convert.ToInt32(dr["PURGE_HEADERS_WEEKS_RECEIPT"]);
            ni.PurgeHtReserveAfter = Convert.ToInt32(dr["PURGE_HEADERS_WEEKS_RESERVE"]);
            ni.PurgeHtVSWAfter = Convert.ToInt32(dr["PURGE_HEADERS_WEEKS_VSW"]);
            ni.PurgeHtWorkUpTotAfter = Convert.ToInt32(dr["PURGE_HEADERS_WEEKS_WORKUPTOTALBUY"]);
            // End TT#400-MD - JSmith - Add Header Purge criteria by Header Type

            ni.StockMinMaxSgRID = Convert.ToInt32(dr["STOCK_MIN_MAX_SG_RID"], CultureInfo.CurrentUICulture);
            ni.IsVirtual = Include.ConvertCharToBool(Convert.ToChar(dr["VIRTUAL_IND"], CultureInfo.CurrentUICulture));
            ni.Purpose = (ePurpose)(Convert.ToInt32(dr["PURPOSE"]));
            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
            ni.Active = Include.ConvertCharToBool(Convert.ToChar(dr["ACTIVE_IND"], CultureInfo.CurrentUICulture));
            // End TT#988
            // BEGIN TT#1399 - GRT - Alternate Hierarchy Inherit Node Properties
            ni.ApplyHNRIDFrom = Convert.ToInt32(dr["APPLY_HN_RID_FROM"], CultureInfo.CurrentUICulture);
            // END TT#1399
            ni.DeleteNode = Include.ConvertCharToBool(Convert.ToChar(dr["NODE_DELETE_IND"], CultureInfo.CurrentUICulture));  // TT#3630 - JSmith - Delete My Hierarchy
        }
        

        // Begin TT#400-MD - JSmith - Add Header Purge criteria by Header Type
        //static private NodeInfo AddNodeHeaderPurge(NodeInfo ni)
        //{
        //    try
        //    {
        //        eHeaderType headerType;
        //        //ePurgeTimeframe purgeTimeFrame; //TT#827-MD -jsobek -Allocation Reviews Performance
        //        int purgeHeaders;

        //        MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
        //        DataTable ldt = mhd.HierarchyNode_Read_HeaderPurge(ni.NodeRID);
        //        foreach (DataRow ldr in ldt.Rows)
        //        {
        //            headerType = (eHeaderType)(Convert.ToInt32(ldr["HEADER_TYPE"], CultureInfo.CurrentUICulture));
        //            //Begin TT#827-MD -jsobek -Allocation Reviews Performance
        //            //if (ldr["PURGE_HEADERS_TIMEFRAME"] == DBNull.Value)
        //            //{
        //            //    purgeTimeFrame = ePurgeTimeframe.None;
        //            //}
        //            //else
        //            //{
        //            //    purgeTimeFrame = (ePurgeTimeframe)(Convert.ToInt32(ldr["PURGE_HEADERS_TIMEFRAME"], CultureInfo.CurrentUICulture));
        //            //}
        //            //End TT#827-MD -jsobek -Allocation Reviews Performance
        //            if (ldr["PURGE_HEADERS"] == DBNull.Value)
        //            {
        //                purgeHeaders = Include.Undefined;
        //            }
        //            else
        //            {
        //                purgeHeaders = (Convert.ToInt32(ldr["PURGE_HEADERS"], CultureInfo.CurrentUICulture));
        //            }

        //            switch (headerType)
        //            {
        //                case eHeaderType.ASN:
        //                    ni.PurgeHtASNAfter = purgeHeaders;
        //                    break;
        //                case eHeaderType.DropShip:
        //                    ni.PurgeHtDropShipAfter = purgeHeaders;
        //                    break;
        //                case eHeaderType.Dummy:
        //                    ni.PurgeHtDummyAfter = purgeHeaders;
        //                    break;
        //                case eHeaderType.PurchaseOrder:
        //                    ni.PurgeHtPurchaseOrderAfter = purgeHeaders;
        //                    break;
        //                case eHeaderType.Receipt:
        //                    ni.PurgeHtReceiptAfter = purgeHeaders;
        //                    break;
        //                case eHeaderType.Reserve:
        //                    ni.PurgeHtReserveAfter = purgeHeaders;
        //                    break;
        //                case eHeaderType.IMO:
        //                    ni.PurgeHtVSWAfter = purgeHeaders;
        //                    break;
        //                case eHeaderType.WorkupTotalBuy:
        //                    ni.PurgeHtWorkUpTotAfter = purgeHeaders;
        //                    break;

        //            }
        //        }

        //        return ni;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        // End TT#400-MD - JSmith - Add Header Purge criteria by Header Type
        //End TT#827-MD -jsobek -Allocation Reviews Performance

		/// <summary>
		/// Reads a base node ID and loads it into the cache.
		/// </summary>
		static private void LoadBaseNodeID(string[] fields)
		{			
			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.Hierarchy_NodeRID_Read(fields[0]);

					foreach(DataRow dr in dt.Rows)
					{
						int nodeRID	= Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
						SetNodeRIDByID(fields[0], nodeRID);
					}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}


		/// <summary>
		/// Reads the color nodes for a style from the database and builds nodeInfo objects for each node.
		/// </summary>
		/// <param name="hierarchyRID">The record ID of the hierarchy</param>
		/// <param name="nodeRID">The record ID of the node</param>
		/// <remarks>
		/// Does not build ID cross reference since color IDs are not unique
		/// </remarks>
		static private void BuildColorsForStyle(int hierarchyRID, int nodeRID)
		{			
			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = null;
				dt = mhd.Hierarchy_ColorNode_Read(hierarchyRID, nodeRID);

                AddToNodeInfoCacheFromDataTable(dt, nodeInfoTypeEnum.Color); //TT#827-MD -jsobek -Allocation Reviews Performance
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Reads all color node IDs for a style node into the cache.
		/// </summary>
		static private void LoadColorsIDsForStyle(string[] fields)
		{			
			try
			{
				// assume that if there are empty entries in the cache the ID does not exist or
				// it would already be loaded
				if (_colorSizesByID.Count + _colorSizesByID.ExpireGroupSize > _colorSizesByID.NumEntries)
				{
					char nodeDelimiter = GetProductLevelDelimiter();
					MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
					DataTable dt = mhd.GetColorIDs(fields[0]);

					foreach(DataRow dr in dt.Rows)
					{
						// ID not built so exit.
						if (dr["STYLE_NODE_ID"] == DBNull.Value ||
							dr["COLOR_CODE_ID"] == DBNull.Value)
						{
							break;
						}
						int nodeRID				= Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
						string styleID			= (string)dr["STYLE_NODE_ID"];
						string colorID			= (string)dr["COLOR_CODE_ID"];
						string ID = styleID + nodeDelimiter + colorID;
						SetColorSizeRIDByID(ID, nodeRID);
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}


		/// <summary>
		/// Reads a color node from the database and builds nodeInfo object for the node.
		/// </summary>
		/// <param name="aNodeRID">The record ID of the node</param>
		/// <remarks>
		/// Does not build ID cross reference since color IDs are not unique
		/// </remarks>
		static private void LoadColorNode(int aNodeRID)
		{
			try
			{
				NodeInfo ni = null;
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				// make sure the node is not already loaded
				ni = GetNodeCacheByRID(aNodeRID);

				// node is not loaded, so load it
				if (ni == null ||
					ni.NodeRID == Include.NoRID)
				{
					DataTable dt = null;
					dt = mhd.Hierarchy_ColorNode_Read(aNodeRID);

                    AddToNodeInfoCacheFromDataTable(dt, nodeInfoTypeEnum.Color); //TT#827-MD -jsobek -Allocation Reviews Performance
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}
	
		/// <summary>
		/// Reads the size nodes for a color from the database and builds nodeInfo objects for each node.
		/// </summary>
		/// <param name="hierarchyRID">The record ID of the hierarchy</param>
		/// <param name="nodeRID">The record ID of the node</param>
		/// <remarks>
		/// Does not build ID cross reference since size IDs are not unique
		/// </remarks>
		static private void BuildSizesForColor(int hierarchyRID, int nodeRID)
		{
			//NodeInfo ni;
			
			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				//SizeCodeInfo sci;
				//int sizeCodeRID;
				DataTable dt = null;
				dt = mhd.Hierarchy_SizeNode_Read(hierarchyRID, nodeRID);

                AddToNodeInfoCacheFromDataTable(dt, nodeInfoTypeEnum.Size);
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Reads all size node IDs for a color node into the cache.
		/// </summary>
		static private void LoadSizesIDsForColor(string[] fields)
		{			
			try
			{
				// assume that if there are empty entries in the cache the ID does not exist or
				// it would already be loaded
				if (_colorSizesByID.Count + _colorSizesByID.ExpireGroupSize > _colorSizesByID.NumEntries)
				{
					char nodeDelimiter = GetProductLevelDelimiter();
					MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
					DataTable dt = mhd.GetSizeIDs(fields[0], fields[1]);

					foreach(DataRow dr in dt.Rows)
					{
						// ID not built so exit.
						if (dr["STYLE_NODE_ID"] == DBNull.Value ||
							dr["COLOR_NODE_ID"] == DBNull.Value ||
							dr["SIZE_CODE_ID"] == DBNull.Value)
						{
							break;
						}
						int nodeRID				= Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
						string styleID			= (string)dr["STYLE_NODE_ID"];
						string colorID			= (string)dr["COLOR_NODE_ID"];
						string sizeID			= (string)dr["SIZE_CODE_ID"];
						string ID = styleID + nodeDelimiter + colorID + nodeDelimiter + sizeID;
						SetColorSizeRIDByID(ID, nodeRID);
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Returns an instance of a ColorCodeInfo object associated with the color code record ID.
		/// </summary>
		/// <param name="aColorCodeRID">The record id of the color code.</param>
		/// <returns>An instance of the ColorCodeInfo class containing the information for the color code</returns>
		/// <remarks>
		/// Return Include.NoRID in the record id if the color code is not found.
		/// </remarks>
		static private ColorCodeInfo GetColorCodeInfoByRID(int aColorCodeRID)
		{
			try
			{
				ColorCodeInfo cci = null;
				lock (_colorCode_lock.SyncRoot)
				{
                    if (_colorCodesByRID.ContainsKey(aColorCodeRID))
                    {
                        cci = (ColorCodeInfo)_colorCodesByRID[aColorCodeRID];
                    }
				}
				if (cci == null)
				{
					cci = new ColorCodeInfo();
					cci.ColorCodeRID = Include.NoRID;
				}
				return cci;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Returns an instance of a SizeCodeInfo object associated with the size code record ID.
		/// </summary>
		/// <param name="aSizeCodeRID">The record id of the size code.</param>
		/// <returns>An instance of the SizeCodeInfo class containing the information for the size code</returns>
		/// <remarks>
		/// Return Include.NoRID in the record id if the size code is not found.
		/// </remarks>
		static private SizeCodeInfo GetSizeCodeInfoByRID(int aSizeCodeRID)
		{
			try
			{
				SizeCodeInfo sci = null;
				lock (_sizeCode_lock.SyncRoot)
				{
                    if (_sizeCodesByRID.ContainsKey(aSizeCodeRID))
                    {
                        sci = (SizeCodeInfo)_sizeCodesByRID[aSizeCodeRID];
                    }
				}
				if (sci == null)
				{
					sci = new SizeCodeInfo();
					sci.SizeCodeRID = Include.NoRID;
				}
				return sci;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Reads the size node the database and builds nodeInfo object for the node.
		/// </summary>
		/// <param name="aNodeRID">The record ID of the node</param>
		/// <remarks>
		/// Does not build ID cross reference since size IDs are not unique
		/// </remarks>
		static private void LoadSizeNode(int aNodeRID)
		{
			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				NodeInfo ni = null;
				// make sure the node is not already loaded
				ni = GetNodeCacheByRID(aNodeRID);

				// node is not loaded, so load it
				if (ni == null ||
					ni.NodeRID == Include.NoRID)
				{
					DataTable dt = null;
					dt = mhd.Hierarchy_SizeNode_Read(aNodeRID);

                    AddToNodeInfoCacheFromDataTable(dt, nodeInfoTypeEnum.Size); //TT#827-MD -jsobek -Allocation Reviews Performance
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Reads the node and builds purge criteria in the nodeInfo object for the node.
		/// </summary>
		/// <param name="nodeRID">The record ID of the node</param>
		/// <remarks>This object is not added to the cache</remarks>
		static private NodeInfo GetPurgeCriteria(int nodeRID)
		{			
			try
			{
				NodeInfo ni = null;
				// make sure the node is not already loaded
				ni = GetNodeCacheByRID(nodeRID);

				// node is not loaded, so load it
				if (ni == null ||
					ni.NodeRID == Include.NoRID)
				{
					MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
					DataTable dt = null;
					dt = mhd.Hierarchy_Node_PurgeCriteria_Read(nodeRID);

					foreach(DataRow dr in dt.Rows)
					{
						ni = new NodeInfo();

                        BuildNodeInfoFromDataRow(ref ni, dr, nodeInfoTypeEnum.PurgeInfo); //TT#827-MD -jsobek -Allocation Reviews Performance
					}
				}
				return ni;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}
	
		/// <summary>
		/// Reads the hierarchies from the database and builds HierarchyInfo objects for each hierarchy.  
		/// </summary>
		/// <remarks>
		/// Also constructs two cross-references.  
		/// One from hierarchy record id to a HierarchyInfo object and the other from the hierarchy ID 
		/// to the hierarchy record id. 
		/// </remarks>
		static private void BuildHierarchies()
		{
			HierarchyInfo hi;
            List<int> hierarchies;
			int userRID;

			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.Hierarchy_Read();

				foreach(DataRow dr in dt.Rows)
				{
					//Begin Track #6269 - JScott - Error logging on after auto upgrade
					//hi = new HierarchyInfo();
					hi = new HierarchyInfo(Audit);
					//End Track #6269 - JScott - Error logging on after auto upgrade
					hi.HierarchyRID = Convert.ToInt32(dr["PH_RID"], CultureInfo.CurrentUICulture);
					hi.HierarchyID	= (string)dr["PH_ID"];
					hi.HierarchyType	= (eHierarchyType)(Convert.ToInt32(dr["PH_Type"], CultureInfo.CurrentUICulture));
					hi.HierarchyRollupOption = (eHierarchyRollupOption)(Convert.ToInt32(dr["HISTORY_ROLL_OPTION"], CultureInfo.CurrentUICulture));
					hi.HierarchyColor = (string)dr["PH_COLOR"];
					hi.Owner = Convert.ToInt32(dr["OWNER_USER_RID"], CultureInfo.CurrentUICulture);
					userRID = Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture);
					hi.OTSPlanLevelType = (eOTSPlanLevelType)(Convert.ToInt32(dr["OTS_PLANLEVEL_TYPE"], CultureInfo.CurrentUICulture));
					if (dr["POSTING_DATE"] == DBNull.Value)
					{
						hi.PostingDate = Calendar.PostDate.Date;
					}
					else
					{
						hi.PostingDate = Convert.ToDateTime(dr["POSTING_DATE"], CultureInfo.CurrentUICulture);
					}

                    if (!_userHierarchies.TryGetValue(userRID, out hierarchies))
					{
                        hierarchies = new List<int>();
					}
					hierarchies.Add(hi.HierarchyRID);
					_userHierarchies[userRID] = hierarchies;
                    // Begin Track #6269 - JSmith - Error logging on
					// only add hierarchy to cache if owner
                    //if (hi.Owner != userRID)
                    //{
                    //    continue;
                    //}
                    if (_hierarchiesByRID.ContainsKey(hi.HierarchyRID))
                    {
                        continue;
                    }
                    // End Track #6269
					
					bool buildHierarchy = true;
					if (_parsedIncludeHierarchyName != null)
					{
						buildHierarchy = false;
						for (int i=0; i<_parsedIncludeHierarchyName.Length; i++)
						{
							if (hi.HierarchyID == _parsedIncludeHierarchyName[i])
							{
								buildHierarchy = true;
								_includedHierarchies.Add(hi.HierarchyRID, hi.HierarchyRID);
								break;
							}
						}
					}
					else
						if (_parsedExcludeHierarchyName != null)
					{
						buildHierarchy = true;
						for (int i=0; i<_parsedExcludeHierarchyName.Length; i++)
						{
							if (hi.HierarchyID == _parsedExcludeHierarchyName[i])
							{
								buildHierarchy = false;
								break;
							}
						}
						if (buildHierarchy == true)
						{
							_includedHierarchies.Add(hi.HierarchyRID, hi.HierarchyRID);
						}
					}
						
					if (buildHierarchy == true)
					{
						DataTable ldt = mhd.HierarchyLevels_Read(hi.HierarchyRID);
                        // Begin TT#3985 - JSmith - Up and down arrows allow user to move levels after nodes are defined
                        //foreach (DataRow ldr in ldt.Rows)
                        //{
                        //    AddLevel(hi, ldr);
                        //}
                        int maxNodeLevel = 0;
                        if (ldt.Rows.Count > 0)
                        {
                            maxNodeLevel = mhd.GetHierarchyMaxNodeLevel(hi.HierarchyRID);
                        }
						foreach(DataRow ldr in ldt.Rows)
						{
                            AddLevel(hi, ldr, maxNodeLevel);
						}
                        // End TT#3985 - JSmith - Up and down arrows allow user to move levels after nodes are defined
						hi.BuildParentChild(mhd, hi.HierarchyRID);
						_hierarchiesByRID.Add(hi.HierarchyRID, hi);
						_hierarchiesByID.Add(hi.HierarchyID, hi.HierarchyRID);

						if (hi.HierarchyType == eHierarchyType.organizational)
						{
							_mainHierarchyRID = hi.HierarchyRID;
						}
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Adds a level to a hierarchy.
		/// </summary>
		/// <param name="hi">The hierarchy</param>
		/// <param name="ldr">DataRow containing level information</param>
        // Begin TT#3985 - JSmith - Up and down arrows allow user to move levels after nodes are defined
        //static private void AddLevel(HierarchyInfo hi, DataRow ldr)
        static private void AddLevel(HierarchyInfo hi, DataRow ldr, int maxNodeLevel)
        // End TT#3985 - JSmith - Up and down arrows allow user to move levels after nodes are defined
		{
			try
			{
				HierarchyLevelInfo hli = new HierarchyLevelInfo();
				hli.Level = Convert.ToInt32(ldr["PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
				hli.LevelID = (string)ldr["PHL_ID"];
				hli.LevelColor = (string)ldr["PHL_COLOR"];
				hli.LevelType = (eHierarchyLevelType)(Convert.ToInt32(ldr["PHL_TYPE"], CultureInfo.CurrentUICulture));
				hli.LevelLengthType = (eLevelLengthType)(Convert.ToInt32(ldr["LENGTH_TYPE"], CultureInfo.CurrentUICulture));
				hli.LevelRequiredSize = Convert.ToInt32(ldr["REQUIRED_SIZE"], CultureInfo.CurrentUICulture);
				hli.LevelSizeRangeFrom = Convert.ToInt32(ldr["SIZE_RANGE_FROM"], CultureInfo.CurrentUICulture);
				hli.LevelSizeRangeTo = Convert.ToInt32(ldr["SIZE_RANGE_TO"], CultureInfo.CurrentUICulture);
				hli.LevelOTSPlanLevelType = (eOTSPlanLevelType)(Convert.ToInt32(ldr["OTS_PLANLEVEL_TYPE"], CultureInfo.CurrentUICulture));
				if (ldr["PHL_DISPLAY_OPTION_ID"] == DBNull.Value)
				{
					hli.LevelDisplayOption = _globalOptions.ProductLevelDisplay;
				}
				else
				{
					hli.LevelDisplayOption = (eHierarchyDisplayOptions)(Convert.ToInt32(ldr["PHL_DISPLAY_OPTION_ID"], CultureInfo.CurrentUICulture));
				}

				if (ldr["PHL_ID_FORMAT"] == DBNull.Value)
				{
					hli.LevelIDFormat = eHierarchyIDFormat.Unique;
				}
				else
				{
					hli.LevelIDFormat = (eHierarchyIDFormat)(Convert.ToInt32(ldr["PHL_ID_FORMAT"], CultureInfo.CurrentUICulture));
				}

				if (ldr["PURGE_DAILY_HISTORY_TIMEFRAME"] == DBNull.Value)
				{
					hli.PurgeDailyHistoryTimeframe = ePurgeTimeframe.None;
				}
				else
				{
					hli.PurgeDailyHistoryTimeframe = (ePurgeTimeframe)(Convert.ToInt32(ldr["PURGE_DAILY_HISTORY_TIMEFRAME"], CultureInfo.CurrentUICulture));
				}
				if (ldr["PURGE_DAILY_HISTORY"] == DBNull.Value)
				{
					hli.PurgeDailyHistory = Include.Undefined;
				}
				else
				{
					hli.PurgeDailyHistory = (Convert.ToInt32(ldr["PURGE_DAILY_HISTORY"], CultureInfo.CurrentUICulture));
				}

				if (ldr["PURGE_WEEKLY_HISTORY_TIMEFRAME"] == DBNull.Value)
				{
					hli.PurgeWeeklyHistoryTimeframe = ePurgeTimeframe.None;
				}
				else
				{
					hli.PurgeWeeklyHistoryTimeframe = (ePurgeTimeframe)(Convert.ToInt32(ldr["PURGE_WEEKLY_HISTORY_TIMEFRAME"], CultureInfo.CurrentUICulture));
				}
				if (ldr["PURGE_WEEKLY_HISTORY"] == DBNull.Value)
				{
					hli.PurgeWeeklyHistory = Include.Undefined;
				}
				else
				{
					hli.PurgeWeeklyHistory = (Convert.ToInt32(ldr["PURGE_WEEKLY_HISTORY"], CultureInfo.CurrentUICulture));
				}

				if (ldr["PURGE_PLANS_TIMEFRAME"] == DBNull.Value)
				{
					hli.PurgePlansTimeframe = ePurgeTimeframe.None;
				}
				else
				{
					hli.PurgePlansTimeframe = (ePurgeTimeframe)(Convert.ToInt32(ldr["PURGE_PLANS_TIMEFRAME"], CultureInfo.CurrentUICulture));
				}
				if (ldr["PURGE_PLANS"] == DBNull.Value)
				{
					hli.PurgePlans = Include.Undefined;
				}
				else
				{
					hli.PurgePlans = (Convert.ToInt32(ldr["PURGE_PLANS"], CultureInfo.CurrentUICulture));
				}

                //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                //if (ldr["PURGE_HEADERS_TIMEFRAME"] == DBNull.Value)
                //{
                    //hli.PurgeHeadersTimeframe = ePurgeTimeframe.None;
                //}
                //else
                //{
                //    hli.PurgeHeadersTimeframe = (ePurgeTimeframe)(Convert.ToInt32(ldr["PURGE_HEADERS_TIMEFRAME"], CultureInfo.CurrentUICulture));
                //}
                //if (ldr["PURGE_HEADERS"] == DBNull.Value)
                //{
                    //hli.PurgeHeaders = Include.Undefined;
                //}
                //else
                //{
                //    hli.PurgeHeaders = (Convert.ToInt32(ldr["PURGE_HEADERS"], CultureInfo.CurrentUICulture));
                //}
                //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type

                // Begin TT#400-MD - JSmith - Add Header Purge criteria by Header Type
                hli = AddLevelHeaderPurge(hi, hli);
                // End TT#400-MD - JSmith - Add Header Purge criteria by Header Type

				// keep track of lowest level above style
				if (hli.LevelType != eHierarchyLevelType.Style &&
					hli.LevelType != eHierarchyLevelType.Color &&
					hli.LevelType != eHierarchyLevelType.Size)
				{
					if (hli.Level > _lowestLoadLevel)
					{
						_lowestLoadLevel = hli.Level;
					}
				}

                // Begin TT#3985 - JSmith - Up and down arrows allow user to move levels after nodes are defined
                if (hli.Level <= maxNodeLevel)
                {
                    hli.LevelNodesExist = true;
                }
                // End TT#3985 - JSmith - Up and down arrows allow user to move levels after nodes are defined

				hi.HierarchyLevels.Add(hli.Level, hli);
				++hi.HierarchyDBLevelsCount;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}

		}

        // Begin TT#400-MD - JSmith - Add Header Purge criteria by Header Type
        static private HierarchyLevelInfo AddLevelHeaderPurge(HierarchyInfo hi, HierarchyLevelInfo hli)
        {
            try
            {
                eHeaderType headerType;
                ePurgeTimeframe purgeTimeFrame;
                int purgeHeaders;

                MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                DataTable ldt = mhd.HierarchyLevels_Read_HeaderPurge(hi.HierarchyRID, hli.Level);
                foreach (DataRow ldr in ldt.Rows)
                {
                    headerType = (eHeaderType)(Convert.ToInt32(ldr["HEADER_TYPE"], CultureInfo.CurrentUICulture));
                    if (ldr["PURGE_HEADERS_TIMEFRAME"] == DBNull.Value)
                    {
                        purgeTimeFrame = ePurgeTimeframe.None;
                    }
                    else
                    {
                        purgeTimeFrame = (ePurgeTimeframe)(Convert.ToInt32(ldr["PURGE_HEADERS_TIMEFRAME"], CultureInfo.CurrentUICulture));
                    }
                    if (ldr["PURGE_HEADERS"] == DBNull.Value)
                    {
                        purgeHeaders = Include.Undefined;
                    }
                    else
                    {
                        purgeHeaders = (Convert.ToInt32(ldr["PURGE_HEADERS"], CultureInfo.CurrentUICulture));
                    }

                    switch (headerType)
                    {

                        case eHeaderType.ASN:
                            hli.PurgeHtASNTimeframe = purgeTimeFrame;
                            hli.PurgeHtASN = purgeHeaders;
                            break;
                        case eHeaderType.DropShip:
                            hli.PurgeHtDropShipTimeframe = purgeTimeFrame;
                            hli.PurgeHtDropShip = purgeHeaders;
                            break;
                        case eHeaderType.Dummy:
                            hli.PurgeHtDummyTimeframe = purgeTimeFrame;
                            hli.PurgeHtDummy = purgeHeaders;
                            break;
                        case eHeaderType.PurchaseOrder:
                            hli.PurgeHtPurchaseOrderTimeframe = purgeTimeFrame;
                            hli.PurgeHtPurchaseOrder = purgeHeaders;
                            break;
                        case eHeaderType.Receipt:
                            hli.PurgeHtReceiptTimeframe = purgeTimeFrame;
                            hli.PurgeHtReceipt = purgeHeaders;
                            break;
                        case eHeaderType.Reserve:
                            hli.PurgeHtReserveTimeframe = purgeTimeFrame;
                            hli.PurgeHtReserve = purgeHeaders;
                            break;
                        case eHeaderType.IMO:
                            hli.PurgeHtVSWTimeframe = purgeTimeFrame;
                            hli.PurgeHtVSW = purgeHeaders;
                            break;
                        case eHeaderType.WorkupTotalBuy:
                            hli.PurgeHtWorkUpTotTimeframe = purgeTimeFrame;
                            hli.PurgeHtWorkUpTot = purgeHeaders;
                            break;
                    }
                }

                return hli;
            }
            catch
            {
                throw;
            }
        }
        // End TT#400-MD - JSmith - Add Header Purge criteria by Header Type

		/// <summary>
		/// Reads the hierarchies from the database and rebuilds the hierarchies by user.  
		/// </summary>
		static public void ReBuildUserHierarchies()
		{
            List<int> hierarchies;
			int userRID;
			int hierarchyRID;

			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.Hierarchy_Read();

				hierarchy_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					_userHierarchies.Clear();

					foreach(DataRow dr in dt.Rows)
					{
						hierarchyRID	= Convert.ToInt32(dr["PH_RID"], CultureInfo.CurrentUICulture);
						userRID = Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture);
					
                        if (!_userHierarchies.TryGetValue(userRID, out hierarchies))
						{
                            hierarchies = new List<int>();
						}
						hierarchies.Add(hierarchyRID);
						_userHierarchies[userRID] = hierarchies;
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					hierarchy_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:ReBuildUserHierarchies writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:ReBuildUserHierarchies writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Reads the eligibility models from the database and builds EligModelInfo objects model.  
		/// </summary>
		static private void BuildEligModels()
		{
			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.EligModel_Read();

				foreach(DataRow dr in dt.Rows)
				{
					int entrySeq;
					EligModelInfo emi = new EligModelInfo();	
					emi.ModelRID	= Convert.ToInt32(dr["EM_RID"], CultureInfo.CurrentUICulture);
					emi.ModelID	= (string)dr["EM_ID"];
					//				emi.ModelEntries = new ArrayList();
					DataTable edt = mhd.EligModelStockEntry_Read(emi.ModelRID);
					entrySeq = 0;
					foreach(DataRow edr in edt.Rows)
					{
						AddEligModelEntry(emi, edr, entrySeq, eEligModelEntryType.StockEligibility);
						++entrySeq;
					}
					edt.Clear();
					edt = mhd.EligModelSalesEntry_Read(emi.ModelRID);
					entrySeq = 0;
					foreach(DataRow edr in edt.Rows)
					{
						AddEligModelEntry(emi, edr, entrySeq, eEligModelEntryType.SalesEligibility);
						++entrySeq;
					}
					edt.Clear();
					edt = mhd.EligModelPriShipEntry_Read(emi.ModelRID);
					entrySeq = 0;
					foreach(DataRow edr in edt.Rows)
					{
						AddEligModelEntry(emi, edr, entrySeq, eEligModelEntryType.PriorityShipping);
						++entrySeq;
					}
					LoadEligibilityModelDates(emi);
					_eligModelsByRID.Add(emi.ModelRID, emi);
					_eligModelsByID.Add(emi.ModelID, emi.ModelRID);
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Reads the stock modifier models from the database and builds EligModelInfo objects model.
		/// </summary>
		/// <param name="emi">An instance of the EligModelInfo class containing eligibility model information</param>
		/// <param name="edr">A row from the data table containing eligibility fields</param>
		/// <param name="entrySeq">The sequence of the entry in the eligibility model</param>
		/// <param name="entryType">The type of eligibility model entry</param>
		static private void AddEligModelEntry(EligModelInfo emi, DataRow edr, int entrySeq, eEligModelEntryType entryType)
		{
			try
			{
				EligModelEntryInfo emei = new EligModelEntryInfo();
				emei.EligModelEntryType = entryType;
				emei.DateRange = new DateRangeProfile(0);
				emei.ModelEntrySeq = entrySeq;
				emei.DateRange.Key = Convert.ToInt32(edr["CDR_RID"], CultureInfo.CurrentUICulture);
				emei.DateRange.StartDateKey = Convert.ToInt32(edr["CDR_START"], CultureInfo.CurrentUICulture);
				emei.DateRange.EndDateKey = Convert.ToInt32(edr["CDR_END"], CultureInfo.CurrentUICulture);
				emei.DateRange.DateRangeType = (eCalendarRangeType)Convert.ToInt32(edr["CDR_RANGE_TYPE_ID"], CultureInfo.CurrentUICulture); //stodd
				emei.DateRange.RelativeTo = (eDateRangeRelativeTo)Convert.ToInt32(edr["CDR_RELATIVE_TO"], CultureInfo.CurrentUICulture);

				emei.DateRange.SelectedDateType = eCalendarDateType.Week;
				emei.DateRange.DisplayDate = Calendar.GetDisplayDate(emei.DateRange);
				++entrySeq;
					
				switch (entryType)
				{
					case eEligModelEntryType.StockEligibility:
						emi.ModelEntries.Add(emei);
						break;
					case eEligModelEntryType.SalesEligibility:
						emi.SalesEligibilityEntries.Add(emei);
						break;
					case eEligModelEntryType.PriorityShipping:
						emi.PriorityShippingEntries.Add(emei);
						break;
				}
			}
			catch 
			{
				throw;
			}
		}

		/// <summary>
		/// Reads the stock modifier models from the database and builds EligModelInfo objects model.  
		/// </summary>
		static private void BuildStkModModels()
		{
			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.StkModModel_Read();

				foreach(DataRow dr in dt.Rows)
				{
					StkModModelInfo smmi = new StkModModelInfo();	
					smmi.ModelRID	= Convert.ToInt32(dr["STKMOD_RID"], CultureInfo.CurrentUICulture);
					smmi.ModelID	= (string)dr["STKMOD_ID"];
					smmi.StkModModelDefault	= ((Convert.ToDouble(dr["STKMOD_DEFAULT"], CultureInfo.CurrentUICulture)));
					DataTable edt = mhd.StkModModelEntry_Read(smmi.ModelRID);
					int stkModEntrySequence = 0;
					foreach(DataRow edr in edt.Rows)
					{
						StkModModelEntryInfo smmei = new StkModModelEntryInfo();
						smmei.DateRange = new DateRangeProfile(0);
						smmei.ModelEntrySeq = stkModEntrySequence;
						smmei.StkModModelEntryValue = Convert.ToDouble(edr["STKMOD_VALUE"], CultureInfo.CurrentUICulture);
						smmei.DateRange.Key = Convert.ToInt32(edr["CDR_RID"], CultureInfo.CurrentUICulture);
						smmei.DateRange.StartDateKey = Convert.ToInt32(edr["CDR_START"], CultureInfo.CurrentUICulture);
						smmei.DateRange.EndDateKey = Convert.ToInt32(edr["CDR_END"], CultureInfo.CurrentUICulture);
						smmei.DateRange.DateRangeType = (eCalendarRangeType)Convert.ToInt32(edr["CDR_RANGE_TYPE_ID"], CultureInfo.CurrentUICulture); //stodd

						smmei.DateRange.RelativeTo = (eDateRangeRelativeTo)Convert.ToInt32(edr["CDR_RELATIVE_TO"], CultureInfo.CurrentUICulture);

						smmei.DateRange.SelectedDateType = eCalendarDateType.Week;
                        //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                        //smmei.DateRange.DisplayDate = Calendar.GetDisplayDate(smmei.DateRange);
                        if (smmei.DateRange.DateRangeType == eCalendarRangeType.Dynamic)
                        {
                            smmei.DateRange.DisplayDate = Calendar.GetDisplayDate(smmei.DateRange, true);
                        }
                        else
                        {
                            smmei.DateRange.DisplayDate = Calendar.GetDisplayDate(smmei.DateRange);
                        }
                        //End TT#169
						++stkModEntrySequence;
					
						smmi.ModelEntries.Add(smmei);
					}
					LoadStockModifierModelDates(smmi);
					_stkModModelsByRID.Add(smmi.ModelRID, smmi);
					_stkModModelsByID.Add(smmi.ModelID, smmi.ModelRID);
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Reads the sales modifier models from the database and builds EligModelInfo objects model.  
		/// </summary>
		static private void BuildSlsModModels()
		{
			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.SlsModModel_Read();

				foreach(DataRow dr in dt.Rows)
				{
					SlsModModelInfo smmi = new SlsModModelInfo();	
					smmi.ModelRID	= Convert.ToInt32(dr["SLSMOD_RID"], CultureInfo.CurrentUICulture);
					smmi.ModelID	= (string)dr["SLSMOD_ID"];
					smmi.SlsModModelDefault	= ((Convert.ToDouble(dr["SLSMOD_DEFAULT"], CultureInfo.CurrentUICulture)));
					//				smmi.ModelEntries = new ArrayList();
					DataTable edt = mhd.SlsModModelEntry_Read(smmi.ModelRID);
					int slsModEntrySequence = 0;
					foreach(DataRow edr in edt.Rows)
					{
						SlsModModelEntryInfo smmei = new SlsModModelEntryInfo();
						smmei.DateRange = new DateRangeProfile(0);
						smmei.ModelEntrySeq = slsModEntrySequence;
						smmei.SlsModModelEntryValue = Convert.ToDouble(edr["SLSMOD_VALUE"], CultureInfo.CurrentUICulture);
						smmei.DateRange.Key = Convert.ToInt32(edr["CDR_RID"], CultureInfo.CurrentUICulture);
						smmei.DateRange.StartDateKey = Convert.ToInt32(edr["CDR_START"], CultureInfo.CurrentUICulture);
						smmei.DateRange.EndDateKey = Convert.ToInt32(edr["CDR_END"], CultureInfo.CurrentUICulture);
						smmei.DateRange.DateRangeType = (eCalendarRangeType)Convert.ToInt32(edr["CDR_RANGE_TYPE_ID"], CultureInfo.CurrentUICulture); //stodd
						smmei.DateRange.RelativeTo = (eDateRangeRelativeTo)Convert.ToInt32(edr["CDR_RELATIVE_TO"], CultureInfo.CurrentUICulture);

						smmei.DateRange.SelectedDateType = eCalendarDateType.Week;
                        //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                        //smmei.DateRange.DisplayDate = Calendar.GetDisplayDate(smmei.DateRange);
                        if (smmei.DateRange.DateRangeType == eCalendarRangeType.Dynamic)
                        {
                            smmei.DateRange.DisplayDate = Calendar.GetDisplayDate(smmei.DateRange, true);
                        }
                        else
                        {
                            smmei.DateRange.DisplayDate = Calendar.GetDisplayDate(smmei.DateRange);
                        }
                        //End TT#169
						++slsModEntrySequence;
					
						smmi.ModelEntries.Add(smmei);
					}
					LoadSalesModifierModelDates(smmi);
					_slsModModelsByRID.Add(smmi.ModelRID, smmi);
					_slsModModelsByID.Add(smmi.ModelID, smmi.ModelRID);
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Reads the FWOS modifier models from the database and builds EligModelInfo objects model.  
		/// </summary>
		static private void BuildFWOSModModels()
		{
			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.FWOSModModel_Read();

				foreach(DataRow dr in dt.Rows)
				{
					FWOSModModelInfo mmi = new FWOSModModelInfo();	
					mmi.ModelRID	= Convert.ToInt32(dr["FWOSMOD_RID"], CultureInfo.CurrentUICulture);
					mmi.ModelID	= (string)dr["FWOSMOD_ID"];
					mmi.FWOSModModelDefault	= ((Convert.ToDouble(dr["FWOSMOD_DEFAULT"], CultureInfo.CurrentUICulture)));
					//				mmi.ModelEntries = new ArrayList();
					DataTable edt = mhd.FWOSModModelEntry_Read(mmi.ModelRID);
					int FWOSModEntrySequence = 0;
					foreach(DataRow edr in edt.Rows)
					{
						FWOSModModelEntryInfo mmei = new FWOSModModelEntryInfo();
						mmei.DateRange = new DateRangeProfile(0);
						mmei.ModelEntrySeq = FWOSModEntrySequence;
						mmei.FWOSModModelEntryValue = Convert.ToDouble(edr["FWOSMOD_VALUE"], CultureInfo.CurrentUICulture);
						mmei.DateRange.Key = Convert.ToInt32(edr["CDR_RID"], CultureInfo.CurrentUICulture);
						mmei.DateRange.StartDateKey = Convert.ToInt32(edr["CDR_START"], CultureInfo.CurrentUICulture);
						mmei.DateRange.EndDateKey = Convert.ToInt32(edr["CDR_END"], CultureInfo.CurrentUICulture);
						mmei.DateRange.DateRangeType = (eCalendarRangeType)Convert.ToInt32(edr["CDR_RANGE_TYPE_ID"], CultureInfo.CurrentUICulture); //stodd
						mmei.DateRange.RelativeTo = (eDateRangeRelativeTo)Convert.ToInt32(edr["CDR_RELATIVE_TO"], CultureInfo.CurrentUICulture);

						mmei.DateRange.SelectedDateType = eCalendarDateType.Week;
                        //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                        //mmei.DateRange.DisplayDate = Calendar.GetDisplayDate(smmei.DateRange);
                        if (mmei.DateRange.DateRangeType == eCalendarRangeType.Dynamic)
                        {
                            mmei.DateRange.DisplayDate = Calendar.GetDisplayDate(mmei.DateRange, true);
                        }
                        else
                        {
                            mmei.DateRange.DisplayDate = Calendar.GetDisplayDate(mmei.DateRange);
                        }
                        //End TT#169
						++FWOSModEntrySequence;
					
						mmi.ModelEntries.Add(mmei);
					}
					LoadFWOSModifierModelDates(mmi);
					_FWOSModModelsByRID.Add(mmi.ModelRID, mmi);
					_FWOSModModelsByID.Add(mmi.ModelID, mmi.ModelRID);
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        //BEGIN TT#108 - MD - DOConnell - FWOS Max Model
        /// <summary>
        /// Reads the FWOS Max models from the database and builds EligModelInfo objects model.  
        /// </summary>
        static private void BuildFWOSMaxModels()
        {
            try
            {
                MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                DataTable dt = mhd.FWOSMaxModel_Read();

                foreach (DataRow dr in dt.Rows)
                {
                    FWOSMaxModelInfo mmi = new FWOSMaxModelInfo();
                    mmi.ModelRID = Convert.ToInt32(dr["FWOSMAX_RID"], CultureInfo.CurrentUICulture);
                    mmi.ModelID = (string)dr["FWOSMAX_ID"];
                    mmi.FWOSMaxModelDefault = ((Convert.ToDouble(dr["FWOSMAX_DEFAULT"], CultureInfo.CurrentUICulture)));
                    //				mmi.ModelEntries = new ArrayList();
                    DataTable edt = mhd.FWOSMaxModelEntry_Read(mmi.ModelRID);
                    int FWOSMaxEntrySequence = 0;
                    foreach (DataRow edr in edt.Rows)
                    {
                        FWOSMaxModelEntryInfo mmei = new FWOSMaxModelEntryInfo();
                        mmei.DateRange = new DateRangeProfile(0);
                        mmei.ModelEntrySeq = FWOSMaxEntrySequence;
                        mmei.FWOSMaxModelEntryValue = Convert.ToDouble(edr["FWOSMAX_VALUE"], CultureInfo.CurrentUICulture);
                        mmei.DateRange.Key = Convert.ToInt32(edr["CDR_RID"], CultureInfo.CurrentUICulture);
                        mmei.DateRange.StartDateKey = Convert.ToInt32(edr["CDR_START"], CultureInfo.CurrentUICulture);
                        mmei.DateRange.EndDateKey = Convert.ToInt32(edr["CDR_END"], CultureInfo.CurrentUICulture);
                        mmei.DateRange.DateRangeType = (eCalendarRangeType)Convert.ToInt32(edr["CDR_RANGE_TYPE_ID"], CultureInfo.CurrentUICulture); //stodd
                        mmei.DateRange.RelativeTo = (eDateRangeRelativeTo)Convert.ToInt32(edr["CDR_RELATIVE_TO"], CultureInfo.CurrentUICulture);

                        mmei.DateRange.SelectedDateType = eCalendarDateType.Week;
                        //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                        //mmei.DateRange.DisplayDate = Calendar.GetDisplayDate(smmei.DateRange);
                        if (mmei.DateRange.DateRangeType == eCalendarRangeType.Dynamic)
                        {
                            mmei.DateRange.DisplayDate = Calendar.GetDisplayDate(mmei.DateRange, true);
                        }
                        else
                        {
                            mmei.DateRange.DisplayDate = Calendar.GetDisplayDate(mmei.DateRange);
                        }
                        //End TT#169
                        ++FWOSMaxEntrySequence;

                        mmi.ModelEntries.Add(mmei);
                    }
                    LoadFWOSMaxModelDates(mmi);
                    _FWOSMaxModelsByRID.Add(mmi.ModelRID, mmi);
                    _FWOSMaxModelsByID.Add(mmi.ModelID, mmi.ModelRID);
                }
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }
        //END TT#108 - MD - DOConnell - FWOS Max Model

		/// <summary>
		/// Reads the sales modifier models from the database and builds EligModelInfo objects model.  
		/// </summary>
		static private void BuildProductChars()
		{
			try
			{
				BuildProductCharGroups();
				BuildProductCharValues();
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Reads the product characteristic groups from the database and builds ProductCharInfo objects model.  
		/// </summary>
		static private void BuildProductCharGroups()
		{
			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.Hierarchy_CharGroup_Read();

				foreach (DataRow dr in dt.Rows)
				{
					ProductCharInfo pci = new ProductCharInfo();
					pci.ProductCharRID = Convert.ToInt32(dr["HCG_RID"], CultureInfo.CurrentUICulture);
					pci.ProductCharID = (string)dr["HCG_ID"];
					_productCharByRID.Add(pci.ProductCharRID, pci);
					_productCharByID.Add(pci.ProductCharID, pci.ProductCharRID);
				}
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Reads the product characteristic values from the database and builds ProductCharValueInfo objects model.  
		/// </summary>
		static private void BuildProductCharValues()
		{
			try
			{
				ProductCharInfo pci = new ProductCharInfo();
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.Hierarchy_Char_Read();

				foreach (DataRow dr in dt.Rows)
				{
					ProductCharValueInfo pcvi = new ProductCharValueInfo();
					pcvi.ProductCharValueRID = Convert.ToInt32(dr["HC_RID"], CultureInfo.CurrentUICulture);
					pcvi.ProductCharValue = (string)dr["HC_ID"];
					pcvi.ProductCharRID = Convert.ToInt32(dr["HCG_RID"], CultureInfo.CurrentUICulture);

					_productCharValueByRID.Add(pcvi.ProductCharValueRID, pcvi);

					// add value to product char
					if (pcvi.ProductCharRID != pci.ProductCharRID)
					{
						pci = GetProductCharInfo(pcvi.ProductCharRID);
					}
					if (pci.ProductCharRID > Include.NoRID)
					{
						pci.AddCharValue(pcvi.ProductCharValueRID);
					}
				}
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Reads the colors from the database and builds ColorCodeInfo objects.  
		/// </summary>
		static private void BuildColors()
		{
			try
			{
				ColorData cd = new ColorData();
				DataTable dt = cd.Colors_Read();

				foreach(DataRow dr in dt.Rows)
				{
					ColorCodeInfo cci	= new ColorCodeInfo();	
					cci.ColorCodeRID	= Convert.ToInt32(dr["COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
					cci.ColorCodeID		= (string)dr["COLOR_CODE_ID"];
					cci.ColorCodeName	= (string)dr["COLOR_CODE_NAME"];
					cci.ColorCodeGroup	= (string)dr["COLOR_CODE_GROUP"];
                    cci.VirtualInd = Include.ConvertCharToBool(Convert.ToChar(dr["VIRTUAL_IND"], CultureInfo.CurrentUICulture));
                    cci.Purpose = (ePurpose)(Convert.ToInt32(dr["PURPOSE"], CultureInfo.CurrentUICulture));
                    _colorCodesByRID.Add(cci.ColorCodeRID, cci);
                    _colorCodesByID.Add(cci.ColorCodeID, cci.ColorCodeRID);
                    if (cci.Purpose == ePurpose.Placeholder)
                    {
                        _colorCodePlaceholders.Add(cci.ColorCodeID, cci);
                    }
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Reads the sizes from the database and builds SizeCodeInfo objects.  
		/// </summary>
		static private void BuildSizes()
		{
            int RID;
			try
			{
				SizeGroup sgd = new SizeGroup();
                Dictionary<string, int> primarySize = new Dictionary<string, int>();
                Dictionary<string, int> secondarySize = new Dictionary<string, int>();
				DataTable dt;
				dt = sgd.PrimarySizes_Read();
				foreach(DataRow dr in dt.Rows)
				{
					primarySize.Add((string)dr["SIZE_CODE_PRIMARY"], Convert.ToInt32(dr["SIZES_RID"], CultureInfo.CurrentUICulture));
				}
				dt = sgd.SecondarySizes_Read();
				foreach(DataRow dr in dt.Rows)
				{
					secondarySize.Add((string)dr["SIZE_CODE_SECONDARY"], Convert.ToInt32(dr["DIMENSIONS_RID"], CultureInfo.CurrentUICulture));
				}


				dt = sgd.Sizes_Read();

				foreach(DataRow dr in dt.Rows)
				{
					SizeCodeInfo sci = new SizeCodeInfo();	
					sci.SizeCodeRID	= Convert.ToInt32(dr["SIZE_CODE_RID"], CultureInfo.CurrentUICulture);
					sci.SizeCodeID	= (string)dr["SIZE_CODE_ID"];
					sci.SizeCodePrimary = (string)dr["SIZE_CODE_PRIMARY"];
					sci.SizeCodeSecondary = (string)dr["SIZE_CODE_SECONDARY"];
					// set the secondary to null if there is no value
					if (sci.SizeCodeSecondary.Trim().Length == 0)
					{
						sci.SizeCodeSecondary = null;
					}

					sci.SizeCodeProductCategory = (string)dr["SIZE_CODE_PRODUCT_CATEGORY"];
					sci.SizeCodeName = Include.GetSizeName(sci.SizeCodePrimary, sci.SizeCodeSecondary, sci.SizeCodeID);
					sci.SizeCodePrimaryRID = Include.NoRID;
					if (primarySize.TryGetValue(sci.SizeCodePrimary, out RID))
					{
                        sci.SizeCodePrimaryRID = RID;
					}
					sci.SizeCodeSecondaryRID = Include.NoRID;
					if (sci.SizeCodeSecondary == null ||
						sci.SizeCodeSecondary.Trim().Length ==0)
					{
					}
					else
					{
                        if (secondarySize.TryGetValue(sci.SizeCodeSecondary, out RID))
						{
                            sci.SizeCodeSecondaryRID = RID;
						}
					}
					_sizeCodesByRID.Add(sci.SizeCodeRID, sci);
					_sizeCodesByID.Add(sci.SizeCodeID, sci.SizeCodeRID);

					string hashKey = Include.GetSizeKey(sci.SizeCodeProductCategory, sci.SizeCodePrimary, sci.SizeCodeSecondary);
			
					if (!_sizeCodesByPriSec.ContainsKey(hashKey))
					{
						_sizeCodesByPriSec.Add(hashKey, sci.SizeCodeRID);
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}


		// Methods

		static public char GetProductLevelDelimiter()
		{
			try
			{
				return _globalOptions.ProductLevelDelimiter;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns an instance of a HierarchyInfo object associated with the hierarchy record ID.
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy.</param>
		/// <param name="aIncludeParentChild">A flag identifying if the parent/child relationship Hashtable is needed</param>
		/// <returns>An instance of the HierarchyInfo class containing the information for the hierarchy</returns>
		/// <remarks>
		/// Return Include.NoRID in the record id if the hierarchy is not found.
		/// </remarks>
		static public HierarchyInfo GetHierarchyInfoByRID(int hierarchyRID, bool aIncludeParentChild)
		{
            HierarchyInfo hi;
			try
			{
				hierarchy_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
                    if (_hierarchiesByRID.TryGetValue(hierarchyRID, out hi))
					{
                        if (aIncludeParentChild)
						{
							return (HierarchyInfo)hi.Clone();
						}
						else
						{
							return (HierarchyInfo)hi.NewObject();
						}
					}
					else
					{
						//Begin Track #6269 - JScott - Error logging on after auto upgrade
						//HierarchyInfo hi = new HierarchyInfo();
						hi = new HierarchyInfo(Audit);
						//End Track #6269 - JScott - Error logging on after auto upgrade
						hi.HierarchyRID = Include.NoRID;
						return hi;
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					hierarchy_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetHierarchyInfoByRID reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetHierarchyInfoByRID reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Returns an instance of a HierarchyInfo object associated with the hierarchy  ID.
		/// </summary>
		/// <param name="hierarchyID">The id of the hierarchy.</param>
		/// <returns>An instance of the HierarchyInfo class containing the information for the hierarchy</returns>
		/// <remarks>
		/// Return Include.NoRID in the record id if the hierarchy is not found.
		/// </remarks>
		static public HierarchyInfo GetHierarchyInfoByID(string hierarchyID)
		{
			try
			{
				int hierarchyRID;
				hierarchy_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
                    if (!_hierarchiesByID.TryGetValue(hierarchyID, out hierarchyRID))
					{
						//Begin Track #6269 - JScott - Error logging on after auto upgrade
						//HierarchyInfo hi = new HierarchyInfo();
						HierarchyInfo hi = new HierarchyInfo(Audit);
						//End Track #6269 - JScott - Error logging on after auto upgrade
						hi.HierarchyRID = Include.NoRID;
						return hi;
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					hierarchy_rwl.ReleaseReaderLock();
				}

				return GetHierarchyInfoByRID(hierarchyRID, true);
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetHierarchyInfoByID reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetHierarchyInfoByID reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        //Begin TT#1313-MD -jsobek -Header Filters
        public static string GetHierarchyIdByRID(int hierarchyRID)
        {
            try
            { 
                HierarchyInfo hi = GetHierarchyInfoByRID(hierarchyRID, false);
                return hi.HierarchyID;
                //MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                //return mhd.Hierarchy_ID_Read(hierarchyRID);
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }
        //End TT#1313-MD -jsobek -Header Filters

        // Begin TT#3985 - JSmith - Up and down arrows allow user to move levels after nodes are defined
        /// <summary>
        /// Updates a HierarchyInfo object to identify nodes exist at a specific level.
        /// </summary>
        /// <param name="hierarchyRID">The record id of the hierarchy.</param>
        /// <param name="nodeLevel">The level to update that nodes exist</param>
        /// <returns>An instance of the HierarchyInfo class containing the information for the hierarchy</returns>
        /// <remarks>
        /// Return Include.NoRID in the record id if the hierarchy is not found.
        /// </remarks>
        static public void SetLevelNodesExist(int hierarchyRID, int nodeLevel)
        {
            HierarchyInfo hi;
 
            try
            {
                hierarchy_rwl.AcquireWriterLock(WriterLockTimeOut);
                try
                {
                    hi = (HierarchyInfo)_hierarchiesByRID[hierarchyRID];
                    HierarchyLevelInfo hli = (HierarchyLevelInfo)hi.HierarchyLevels[nodeLevel];
                    hli.LevelNodesExist = true;
                }
                catch (Exception ex)
                {
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    throw;
                }
                finally
                {
                    // Ensure that the lock is released.
                    hierarchy_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:JoinUpdate writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:JoinUpdate writer lock has timed out");
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }
        // End TT#3985 - JSmith - Up and down arrows allow user to move levels after nodes are defined

		/// <summary>
		/// Returns a list of node record IDs that are descendants for the node in the hierarchy
		/// </summary>
		/// <param name="nodeRID">The record id of the node.</param>
		/// <param name="hierarchyRID">The record ID of the hierarchy</param>
		/// <returns>An instance of the NodeDescendantList class containing descendant node record IDs</returns>
		static public NodeDescendantList GetNodeDescendantList(int nodeRID, int hierarchyRID,
			eNodeSelectType aNodeSelectType)
		{
			try
			{
				HierarchyInfo hi = null;
				NodeDescendantList descendantList = new NodeDescendantList(eProfileType.HierarchyNodeDescendant);
				hi = GetHierarchyInfoByRID(hierarchyRID, true);
				if (hi != null)
				{
					GetDescendants(ref descendantList, hi, nodeRID, aNodeSelectType);
				}
				return descendantList;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Recursively navigates hierarchy and returns the record ids of all descendants
		/// </summary>
		/// <param name="descendantList">An instance of the NodeDescendantList class containing descendant node record IDs</param>
		/// <param name="hi">An instance of the HierarchyInfo class containing information for the hierarchy</param>
		/// <param name="nodeRID">The record ID of the node</param>
		/// <param name="aNodeSelectType">The type of nodes to select</param>
		static private void GetDescendants(ref NodeDescendantList descendantList, HierarchyInfo hi, int nodeRID,
			eNodeSelectType aNodeSelectType)
		{
			try
			{
				HierarchyInfo.NodeRelationship nr = hi.GetChildRelationship(nodeRID);
				if (nr.ChildrenCount() > 0)
				{
					foreach (int childRID in nr.children)  //  Get the children of the children
					{
						if (!descendantList.Contains(childRID))
						{
							NodeDescendantProfile ndp = new NodeDescendantProfile(childRID);
							descendantList.Add(ndp);
							GetDescendants(ref descendantList, hi, childRID, aNodeSelectType);
						}
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Returns a list of node record IDs that are descendants for the node in the hierarchy for a specified
		/// level of the hierarchy
		/// </summary>
		/// <param name="nodeRID">The record id of the node.</param>
		/// <param name="hierarchyRID">The record ID of the hierarchy</param>
		/// <param name="aHierarchyLevelType">The eHierarchyLevelType of descendants to retrieve</param>
		/// <param name="aNodeSelectType">The type of nodes to select</param>
		/// <returns>An instance of the NodeDescendantList class containing descendant node record IDs</returns>
		static public NodeDescendantList GetNodeDescendantList(int nodeRID, int hierarchyRID, eHierarchyLevelType aHierarchyLevelType,
			eNodeSelectType aNodeSelectType)
		{
			try
			{
				HierarchyInfo hi = null;
				NodeDescendantList descendantList = new NodeDescendantList(eProfileType.HierarchyNodeDescendant);
				hi = GetHierarchyInfoByRID(hierarchyRID, true);
				if (hi != null)
				{
					GetDescendants(ref descendantList, hi, nodeRID, aHierarchyLevelType, aNodeSelectType);
				}
				return descendantList;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Recursively navigates hierarchy and returns the record ids of all descendants
		/// </summary>
		/// <param name="descendantList">An instance of the NodeDescendantList class containing descendant node record IDs</param>
		/// <param name="hi">An instance of the HierarchyInfo class containing information for the hierarchy</param>
		/// <param name="nodeRID">The record ID of the node</param>
		/// <param name="aHierarchyLevelType">The eHierarchyLevelType of descendants to retrieve</param>
		/// <param name="aNodeSelectType">The type of nodes to select</param>
		static private void GetDescendants(ref NodeDescendantList descendantList, HierarchyInfo hi, int nodeRID, eHierarchyLevelType aHierarchyLevelType,
			eNodeSelectType aNodeSelectType)
		{
			try
			{
				NodeInfo ni = GetNodeInfoByRID(nodeRID);
				bool selectNode = false;
				switch (aNodeSelectType)
				{
					case eNodeSelectType.All:
						selectNode = true;
						break;
					case eNodeSelectType.NoVirtual:
						if (!ni.IsVirtual)
						{
							selectNode = true;
						}
						break;
					case eNodeSelectType.VirtualOnly:
						if (ni.IsVirtual)
						{
							selectNode = true;
						}
						break;
				}
                // Begin TT#3630 - JSmith - Delete My Hierarchy
                if (ni.DeleteNode)
                {
                    selectNode = false;
                }
                // End TT#3630 - JSmith - Delete My Hierarchy
				if (ni.LevelType == aHierarchyLevelType)
				{
					if (!descendantList.Contains(nodeRID) &&
						selectNode)
					{
						NodeDescendantProfile ndp = new NodeDescendantProfile(nodeRID);
						descendantList.Add(ndp);
					}
				}
				else
				{
					HierarchyInfo.NodeRelationship nr = hi.GetChildRelationship(nodeRID);	
					if (nr.ChildrenCount() > 0)
					{
						foreach (int childRID in nr.children)  //  Get the children of the children
						{
							if (!descendantList.Contains(childRID))
							{
								ni = GetNodeInfoByRID(childRID);
								if (ni.HomeHierarchyRID != hi.HierarchyRID)
								{
									hi = GetHierarchyInfoByRID(ni.HomeHierarchyRID, true);
								}
								GetDescendants(ref descendantList, hi, childRID, aHierarchyLevelType, aNodeSelectType);
							}
						}
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        //Begin Track #5004 - JSmith - Global Unlock
        /// <summary>
        /// Returns a list of node record IDs that are descendants for the node in the hierarchy for a specified
        /// level of the hierarchy
        /// </summary>
        /// <param name="nodeRID">The record id of the node.</param>
        /// <param name="hierarchyRID">The record ID of the hierarchy</param>
        /// <param name="aNodeSelectType">The type of nodes to select</param>
        /// <param name="aFromLevelOffset">The offset of the level to begin retrieving descendants</param>
        /// <param name="aToLevelOffset">The offset of the level to end retrieving descendants</param>
        /// <returns>An instance of the NodeDescendantList class containing descendant node record IDs</returns>
        static public NodeDescendantList GetNodeDescendantList(int aNodeRID, int hierarchyRID, 
            eNodeSelectType aNodeSelectType, int aFromLevelOffset, int aToLevelOffset)
        {
            try
            {
                int nodeRID;
                bool IsVirtual = false;
                NodeDescendantList descendantList = new NodeDescendantList(eProfileType.HierarchyNodeDescendant);

                MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                DataTable dt = mhd.GetDescendantsByRange(aNodeRID, aFromLevelOffset, aToLevelOffset);
                        
                foreach (DataRow dr in dt.Rows)
                {
                    nodeRID = Convert.ToInt32(dr["HN_RID"]);
                    if (descendantList.Contains(nodeRID))
                    {
                        continue;
                    }
                    bool selectNode = false;
                    switch (aNodeSelectType)
                    {
                        case eNodeSelectType.All:
                            selectNode = true;
                            break;
                        case eNodeSelectType.NoVirtual:
                            IsVirtual = Include.ConvertCharToBool(Convert.ToChar(dr["VIRTUAL_IND"], CultureInfo.CurrentUICulture));
                            if (!IsVirtual)
                            {
                                selectNode = true;
                            }
                            break;
                        case eNodeSelectType.VirtualOnly:
                            IsVirtual = Include.ConvertCharToBool(Convert.ToChar(dr["VIRTUAL_IND"], CultureInfo.CurrentUICulture));
                            if (IsVirtual)
                            {
                                selectNode = true;
                            }
                            break;
                    }
                    // Begin TT#3630 - JSmith - Delete My Hierarchy
					//BEGIN TT#4694 - DOConnell - Forecast Spread Method with alternate hierarchy recieves an Argument Exception error
                    //if (Include.ConvertCharToBool(Convert.ToChar(dr["NODE_DELETE_IND"], CultureInfo.CurrentUICulture)))
                    //{
                    //    selectNode = false;
                    //}
					//END TT#4694 - DOConnell - Forecast Spread Method with alternate hierarchy recieves an Argument Exception error
                    // End TT#3630 - JSmith - Delete My Hierarchy
                    if (selectNode)
                    {
                        NodeDescendantProfile ndp = new NodeDescendantProfile(nodeRID);
                        descendantList.Add(ndp);
                    }
                }
                return descendantList;
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }
        //End Track #5004

        // Begin TT#155 - JSmith - Size Curve Method
        /// <summary>
        /// Retrieves the descendant node list
        /// </summary>
        /// <param name="hierarchyRID">The record id of the hierarchy</param>
        /// <param name="aNodeRID">The record id of the node</param>
        /// <param name="aGetByType">Identifies the type check to use to determine the descendant</param>
        /// <param name="level">The level or number of levels offset of the node's level to retrieve</param>
        /// <param name="aLevelType">The level master type of the node to retrieve</param>
        /// <param name="aNodeSelectType">
        /// The indicator of type eNodeSelectType that identifies which type of nodes to select
        /// </param>
        /// <returns>
        /// An instance of the NodeDescendantList object containing NodeDescendantProfile objects for each descendant.
        /// </returns>
        static public NodeDescendantList GetNodeDescendantList(int hierarchyRID, int aNodeRID,
            eHierarchyDescendantType aGetByType, int level, eHierarchyLevelMasterType aLevelType,
            eNodeSelectType aNodeSelectType)
        {
            try
            {
               int nodeRID;
               bool IsVirtual = false;
               // Begin TT#2231 - JSmith - Size curve build failing
               int colorOrSizeCodeRID = Include.NoRID;
               // End TT#2231

                NodeDescendantList descendantList = new NodeDescendantList(eProfileType.HierarchyNodeDescendant);
                MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                DataTable dt = null;
                switch (aGetByType)
                {
                    case eHierarchyDescendantType.offset:
                        dt = mhd.GetDescendantsByOffset(aNodeRID, level);
                        break;
                    case eHierarchyDescendantType.masterType:
                        dt = mhd.GetDescendantsByType(aNodeRID, aLevelType);
                        break;
                    default:
                        dt = mhd.GetDescendantsByLevel(aNodeRID, level);
                        break;
                }
                foreach (DataRow dr in dt.Rows)
                {
                    nodeRID = Convert.ToInt32(dr["HN_RID"]);
                    // Begin TT#2231 - JSmith - Size curve build failing
                    colorOrSizeCodeRID = Convert.ToInt32(dr["ColorOrSizeCodeRID"]);
                    // End TT#2231
                    if (descendantList.Contains(nodeRID))
                    {
                        continue;
                    }
                    bool selectNode = false;
                    switch (aNodeSelectType)
                    {
                        case eNodeSelectType.All:
                            selectNode = true;
                            break;
                        case eNodeSelectType.NoVirtual:
                            if (dr["VIRTUAL_IND"] == DBNull.Value)
                            {
                                IsVirtual = false;
                            }
                            else
                            {
                                IsVirtual = Include.ConvertCharToBool(Convert.ToChar(dr["VIRTUAL_IND"], CultureInfo.CurrentUICulture));
                            }
                            if (!IsVirtual)
                            {
                                selectNode = true;
                            }
                            break;
                        case eNodeSelectType.VirtualOnly:
                            if (dr["VIRTUAL_IND"] == DBNull.Value)
                            {
                                IsVirtual = false;
                            }
                            else
                            {
                                IsVirtual = Include.ConvertCharToBool(Convert.ToChar(dr["VIRTUAL_IND"], CultureInfo.CurrentUICulture));
                            }
                            if (IsVirtual)
                            {
                                selectNode = true;
                            }
                            break;
                    }
                    // Begin TT#4310 - JSmith - Severe Errors in Size Curve Generation
                    // Begin TT#3630 - JSmith - Delete My Hierarchy
                    //if (Include.ConvertCharToBool(Convert.ToChar(dr["NODE_DELETE_IND"], CultureInfo.CurrentUICulture)))
                    //{
                    //    selectNode = false;
                    //}
                    // End TT#3630 - JSmith - Delete My Hierarchy
                    // End TT#4310 - JSmith - Severe Errors in Size Curve Generation
                    if (selectNode)
                    {
                        NodeDescendantProfile ndp = new NodeDescendantProfile(nodeRID);
                        // Begin TT#2231 - JSmith - Size curve build failing
                        ndp.ColorOrSizeCodeRID = colorOrSizeCodeRID;
                        // End TT#2231
                        descendantList.Add(ndp);
                    }
                }
                return descendantList;
            }
            catch (Exception ex)
            {
                Audit.Log_Exception(ex);
                throw;
            }
        }

        // End TT#155

		/// <summary>
		/// Returns an instance of a NodeInfo object associated with the node record ID.
		/// </summary>
		/// <param name="nodeRID">The record id of the node.</param>
		/// <param name="aLoadSiblings">This flag identifies if the siblings of the node should also be loaded</param>
		/// <returns>An instance of the NodeInfo class containing the information for the node</returns>
		/// <remarks>
		/// Return Include.NoRID in the record id if the node is not found.
		/// </remarks>
		static public NodeInfo GetNodeInfoByRID(int nodeRID, bool aLoadSiblings)
		{
			try
			{
				NodeInfo ni = GetNodeInfoByRID(nodeRID);
				if (ni == null ||
					ni.NodeRID == Include.NoRID)
				{
					// see if node is style, color or size that has not been loaded
					if (CheckLoadChildren(nodeRID, aLoadSiblings))
					{
						// try again
						ni = GetNodeInfoByRID(nodeRID);
					}
					else if (ni == null)
					{
						ni = new NodeInfo();
						ni.NodeRID = Include.NoRID;
					}
				}
				return ni;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Returns an instance of a NodeInfo object associated with the node record ID.
		/// </summary>
		/// <param name="nodeRID">The record id of the node.</param>
		/// <returns>An instance of the NodeInfo class containing the information for the node</returns>
		/// <remarks>
		/// Does not load node if not found, returns Include.NoRID in the record id if the node is not found.
		/// </remarks>
		static public NodeInfo GetNodeInfoByRID(int nodeRID)
		{
			try
			{
				NodeInfo ni = GetNodeCacheByRID(nodeRID);
				if (ni == null)
				{
                    // Begin TT#4270 - JSmith - History/Forecast Load Failing
                    //ni = LoadNode(nodeRID);
                    //// still no node, send back empty node
                    //if (ni == null)
                    //{
                    //    ni = new NodeInfo();
                    //    ni.NodeRID = Include.NoRID;
                    //}
                    lock (_load_lock.SyncRoot)
                    {
                        ni = LoadNode(nodeRID);
                        // still no node, send back empty node
                        if (ni == null)
                        {
                            ni = new NodeInfo();
                            ni.NodeRID = Include.NoRID;
                        }
                    }
                    // End TT#4270 - JSmith - History/Forecast Load Failing
				}
				return (NodeInfo)ni.Clone();
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static private NodeInfo GetNodeCacheByRID(int aNodeRID)
		{
			try
			{
				lock (_cache_lock.SyncRoot)
				{
                    if (_nodesByRID.ContainsKey(aNodeRID))
                    {
                        return (NodeInfo)_nodesByRID[aNodeRID];
                    }
                    else
                    {
                        return null;
                    }
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static private void SetNodeCacheByRID(int aKey, NodeInfo aNodeInfo)
		{
			try
			{
				lock (_cache_lock.SyncRoot)
				{
                    // Begin TT#3523 - JSmith - Performance of Anthro morning processing jobs
                    LoadCharacteristicsFromDataTable(aNodeInfo);
                    // End TT#3523 - JSmith - Performance of Anthro morning processing jobs
					_nodesByRID[aKey] = aNodeInfo;
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static private void RemoveNodeCacheByRID(int aNodeRID)
		{
			try
			{
				lock (_cache_lock.SyncRoot)
				{
					_nodesByRID.Remove(aNodeRID);
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Returns an instance of a NodeInfo object associated with the node record ID.
		/// </summary>
		/// <param name="nodeID">The id of the node.</param>
		/// <returns>An instance of the NodeInfo class containing the information for the node</returns>
		/// <remarks>
		/// Return Include.NoRID in the record id if the node is not found.
		/// </remarks>
		static public NodeInfo GetNodeInfoByID(string nodeID)
		{
			try
			{
				object RID = GetNodeRIDByID(nodeID);
				if (RID != null)
				{
					return GetNodeInfoByRID((int)RID, false);
				}
				else
				{
					NodeInfo ni = new NodeInfo();
					ni.NodeRID = Include.NoRID;
					return ni;
				}
			}       	
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Adds a new entry to the node ID to node RID cache.
		/// </summary>
		/// <param name="aNodeID">The id of the node.</param>
		/// <param name="aNodeRID">The record ID of the node</param>
		static public void AddNodeRIDByID(string aNodeID, int aNodeRID)
		{
			try
			{
				int index = aNodeID.IndexOf(GetProductLevelDelimiter());
				if (index > -1) // has delimiter so must be color or size      
				{
					SetColorSizeRIDByID(aNodeID, aNodeRID);
				}
				else
				{
					SetNodeRIDByID(aNodeID, aNodeRID);
				}
			}       	
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static public object GetNodeRIDByID(string aNodeID)
		{
			try
			{
                object RID = null;
				lock (_cache_lock.SyncRoot)
				{
                    if (_nodesByID.ContainsKey(aNodeID))
                    {
                        RID = _nodesByID[aNodeID];
                    }
				}
				// if not found, check color/size
                if (RID == null &&
					_colorSizesCacheUsed)
				{
					lock (_colorSizeIDcache_lock.SyncRoot)
					{

                        if (_colorSizesByID.ContainsKey(aNodeID))
                        {
                            RID = _colorSizesByID[aNodeID];
                        }
					}
				}
				// still not found, try to load ID
				if (RID == null)
				{
					string[] fields = MIDstringTools.Split(aNodeID, GetProductLevelDelimiter(), true);
					switch (fields.Length)
					{
						case 2:		// color node
							if (_colorSizesCacheUsed)
							{
								LoadColorsIDsForStyle(fields);
								lock (_colorSizeIDcache_lock.SyncRoot)
								{
                                    if (_colorSizesByID.ContainsKey(aNodeID))
                                    {
                                        RID = _colorSizesByID[aNodeID];
                                    }
								}
							}
							break;
						case 3:		// size node
							if (_colorSizesCacheUsed)
							{
								LoadSizesIDsForColor(fields);
								lock (_colorSizeIDcache_lock.SyncRoot)
								{
                                    if (_colorSizesByID.ContainsKey(aNodeID))
                                    {
                                        RID = _colorSizesByID[aNodeID];
                                    }
								}
							}
							break;
						default:	// base node
							LoadBaseNodeID(fields);
							lock (_cache_lock.SyncRoot)
							{
                                if (_nodesByID.ContainsKey(aNodeID))
                                {
                                    RID = _nodesByID[aNodeID];
                                }
							}
							break;
					}
				}
				return RID;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static private void SetNodeRIDByID(string aNodeID, int aKey)
		{
			try
			{
				if (aNodeID != null &&
					aKey > Include.NoRID)
				{
					lock (_cache_lock.SyncRoot)
					{
						_nodesByID[aNodeID] = aKey;
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static private void SetColorSizeRIDByID(string aNodeID, int aKey)
		{
			try
			{
				if (_colorSizesCacheUsed)
				{
					if (aNodeID != null &&
						aKey > Include.NoRID)
					{
						lock (_colorSizeIDcache_lock.SyncRoot)
						{
							_colorSizesByID[aNodeID] = aKey;
						}
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static private void ClearColorSizeRIDByID()
		{
			try
			{
				if (_colorSizesCacheUsed)
				{
					lock (_colorSizeIDcache_lock.SyncRoot)
					{
						_colorSizesByID.Clear();
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static private int GetColorSizeRIDByIDCount()
		{
			try
			{
				if (_colorSizesCacheUsed)
				{
					lock (_colorSizeIDcache_lock.SyncRoot)
					{
						return _colorSizesByID.Count;
					}
				}
				else
				{
					return 0;
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static private int GetColorSizeRIDByIDNumEntries()
		{
			try
			{
				if (_colorSizesCacheUsed)
				{
					lock (_colorSizeIDcache_lock.SyncRoot)
					{
						return _colorSizesByID.NumEntries;
					}
				}
				else
				{
					return 0;
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static private void RemoveNodeRIDByID(string aNodeID)
		{
			try
			{
				if (aNodeID != null)	// 03-16-06 Ron Matelic added if... 
				{
					lock (_cache_lock.SyncRoot)
					{
						_nodesByID.Remove(aNodeID);
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static private void RemoveColorSizeRIDByID(string aNodeID)
		{
			try
			{
				if (_colorSizesCacheUsed)
				{
					lock (_colorSizeIDcache_lock.SyncRoot)
					{
						_colorSizesByID.Remove(aNodeID);
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}


		/// <summary>
		/// Gets the type of the hierarchy.
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <returns></returns>
		static public eHierarchyType GetHierarchyType(int hierarchyRID)
		{
			try
			{
				if (hierarchyRID == 0)
				{
					return eHierarchyType.alternate;
				}
				else
				{
					HierarchyInfo hi = GetHierarchyInfoByRID(hierarchyRID, true);
					return hi.HierarchyType;
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Updates the posting date.
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="postingDate">The posting date</param>
		static public void PostingDateUpdate(int hierarchyRID, DateTime postingDate)
		{
			try
			{
				bool postingDateChanged = false;
				hierarchy_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					HierarchyInfo hi = (HierarchyInfo)_hierarchiesByRID[hierarchyRID];
					if (hi.PostingDate != postingDate)
					{
						hi.PostingDate = postingDate;
						_hierarchiesByRID[hierarchyRID] = hi;
						postingDateChanged = true;
					}
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					hierarchy_rwl.ReleaseWriterLock();
				}

				if (postingDateChanged)
				{
					// rebuild all models that use a current date reference
					foreach (EligModelInfo eligModel in _eligModelsByRID.Values)
					{
						if (eligModel.ContainsDynamicDates)
						{
							LoadEligibilityModelDates(eligModel);
						}
					}
					foreach (SlsModModelInfo slsModModel in _slsModModelsByRID.Values)
					{
						if (slsModModel.ContainsDynamicDates)
						{
							LoadSalesModifierModelDates(slsModModel);
						}
					}
					foreach (StkModModelInfo stkModModel in _stkModModelsByRID.Values)
					{
						if (stkModModel.ContainsDynamicDates)
						{
							LoadStockModifierModelDates(stkModModel);
						}
					}
                    foreach (FWOSModModelInfo fwoskModModel in _FWOSModModelsByRID.Values)
                    {
                        if (fwoskModModel.ContainsDynamicDates)
                        {
                            LoadFWOSModifierModelDates(fwoskModModel);
                        }
                    }
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:PostingDateUpdate writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:PostingDateUpdate writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Retrieves information about the main hierarchy
		/// </summary>
		/// <remarks>
		/// This call will not work if multiple organizational hierarchies are allowed.
		/// </remarks>
		/// <returns>An instance of the HierarchyProfile class containing the information for the hierarchy</returns>
		static public HierarchyProfile GetMainHierarchyData()
		{
			try
			{
				if (_mainHierarchyRID != Include.NoRID)
				{
					return GetHierarchyData(_mainHierarchyRID);
				}
				else
				{
					return new HierarchyProfile(Include.NoRID);
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets all information associated with a hierarchy's definition using the hierarchy name.
		/// </summary>
		/// <param name="hierarchyID">The id of the hierarchy</param>
		/// <returns>An instance of the HierarchyProfile class containing the information for the hierarchy</returns>
		static public HierarchyProfile GetHierarchyData(string hierarchyID)
		{
			try
			{
				int hierarchyRID;
				hierarchy_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
                    if (!_hierarchiesByID.TryGetValue(hierarchyID, out hierarchyRID))
					{
						HierarchyProfile hp = new HierarchyProfile(Include.NoRID);
						return hp;
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					hierarchy_rwl.ReleaseReaderLock();
				}

				return GetHierarchyData(hierarchyRID);
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetHierarchyData reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetHierarchyData reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets all information associated with a hierarchy's definition using the hierarchy record id.
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <returns>An instance of the HierarchyProfile class containing the information for the hierarchy</returns>
		static public HierarchyProfile GetHierarchyData(int hierarchyRID)
		{
			try
			{
					HierarchyLevelInfo hli;
					HierarchyInfo hi = GetHierarchyInfoByRID(hierarchyRID, false);
					HierarchyProfile hp = new HierarchyProfile(hi.HierarchyRID);
					hp.HierarchyChangeType = eChangeType.none;
					hp.HierarchyColor = hi.HierarchyColor;
					hp.HierarchyID = hi.HierarchyID;
					hp.HierarchyLevels.Clear();
					for (int level = 1; level <= hi.HierarchyLevels.Count; level++)
					{
						hli = (HierarchyLevelInfo)hi.HierarchyLevels[level];
						HierarchyLevelProfile hlp = new HierarchyLevelProfile(level);
						hlp.Level = hli.Level;
						hlp.LevelID = hli.LevelID;
						hlp.LevelColor = hli.LevelColor;
						hlp.LevelType = hli.LevelType;
						hlp.LevelLengthType = hli.LevelLengthType;
						hlp.LevelRequiredSize = hli.LevelRequiredSize;
						hlp.LevelSizeRangeFrom = hli.LevelSizeRangeFrom;
						hlp.LevelSizeRangeTo = hli.LevelSizeRangeTo;
						hlp.LevelOTSPlanLevelType = hli.LevelOTSPlanLevelType;
						hlp.LevelDisplayOption = hli.LevelDisplayOption;
						hlp.LevelIDFormat = hli.LevelIDFormat;
						//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
						//hlp.LevelNodeCount = hli.LevelNodeCount;
						hlp.LevelNodesExist = hli.LevelNodesExist;
						//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
						hlp.PurgeDailyHistoryTimeframe = hli.PurgeDailyHistoryTimeframe;
						hlp.PurgeDailyHistory = hli.PurgeDailyHistory;
						hlp.PurgeWeeklyHistoryTimeframe = hli.PurgeWeeklyHistoryTimeframe;
						hlp.PurgeWeeklyHistory = hli.PurgeWeeklyHistory;
						hlp.PurgePlansTimeframe = hli.PurgePlansTimeframe;
						hlp.PurgePlans = hli.PurgePlans;
                        //hlp.PurgeHeadersTimeframe = hli.PurgeHeadersTimeframe;
                        //hlp.PurgeHeaders = hli.PurgeHeaders;
						hp.HierarchyLevels.Add(hlp.Level, hlp);
                        // update profile from info
                        //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                        hlp.PurgeHtASNTimeframe = hli.PurgeHtASNTimeframe;
                        hlp.PurgeHtASN = hli.PurgeHtASN;
                        hlp.PurgeHtDropShipTimeframe = hli.PurgeHtDropShipTimeframe;
                        hlp.PurgeHtDropShip = hli.PurgeHtDropShip;
                        hlp.PurgeHtDummyTimeframe = hli.PurgeHtDummyTimeframe;
                        hlp.PurgeHtDummy = hli.PurgeHtDummy;
                        hlp.PurgeHtPurchaseOrderTimeframe = hli.PurgeHtPurchaseOrderTimeframe;
                        hlp.PurgeHtPurchaseOrder = hli.PurgeHtPurchaseOrder;
                        hlp.PurgeHtReceiptTimeframe = hli.PurgeHtReceiptTimeframe;
                        hlp.PurgeHtReceipt = hli.PurgeHtReceipt;
                        hlp.PurgeHtReserveTimeframe = hli.PurgeHtReserveTimeframe;
                        hlp.PurgeHtReserve = hli.PurgeHtReserve;
                        hlp.PurgeHtVSWTimeframe = hli.PurgeHtVSWTimeframe;
                        hlp.PurgeHtVSW = hli.PurgeHtVSW;
                        hlp.PurgeHtWorkUpTotTimeframe = hli.PurgeHtWorkUpTotTimeframe;
                        hlp.PurgeHtWorkUpTot = hli.PurgeHtWorkUpTot;
                        //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                    }
					hp.HierarchyDBLevelsCount = hi.HierarchyDBLevelsCount;
					hp.HierarchyRootNodeRID = hi.HierarchyRootNodeRID;
					hp.Owner = hi.Owner;
					hp.PostingDate = hi.PostingDate;
					hp.HierarchyType = hi.HierarchyType;
					hp.HierarchyRollupOption = hi.HierarchyRollupOption;
					hp.OTSPlanLevelType = hi.OTSPlanLevelType;
				if (hp.HierarchyType == eHierarchyType.organizational)
				{
					NodeInfo ni = GetNodeCacheByRID(hp.HierarchyRootNodeRID);
					hp.OTSPlanLevelHierarchyLevelSequence = ni.OTSPlanLevelHierarchyLevelSequence;
				}
					return hp;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Deletes the requested hierarchy.
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		static public void DeleteHierarchy(int hierarchyRID)
		{
			try
			{
				hierarchy_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					HierarchyInfo hi = (HierarchyInfo)_hierarchiesByRID[hierarchyRID];
					_hierarchiesByRID.Remove(hierarchyRID);
					_hierarchiesByID.Remove(hi.HierarchyID);
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					hierarchy_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:DeleteHierarchy writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:DeleteHierarchy writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Adds or updates information relating to a hierarchy
		/// </summary>
		/// <param name="hp">Data used to update a hierarchy's definition</param>
		/// <returns></returns>
		static public HierarchyProfile HierarchyUpdate(HierarchyProfile hp)
		{
			try
			{
				hierarchy_rwl.AcquireWriterLock(WriterLockTimeOut);
				HierarchyLevelProfile hlp;
				try
				{
					if (_mainHierarchyRID == Include.NoRID &&
						hp.HierarchyType == eHierarchyType.organizational)
					{
						_mainHierarchyRID = hp.Key;
					}
					switch (hp.HierarchyChangeType)
					{
						case eChangeType.none: 
						{
							break;
						}
						case eChangeType.add: 
						{
							// build hierarchy instance
							//Begin Track #6269 - JScott - Error logging on after auto upgrade
							//HierarchyInfo hi = new HierarchyInfo();
							HierarchyInfo hi = new HierarchyInfo(Audit);
							//End Track #6269 - JScott - Error logging on after auto upgrade
							hi.HierarchyColor = hp.HierarchyColor;
							hi.HierarchyID = hp.HierarchyID;
							//						hi.HierarchyLevels = hp.HierarchyLevels;
							hi.HierarchyLevels.Clear();
							for (int level = 1; level <= hp.HierarchyLevels.Count; level++) // levels begin at 1 since the hierarchy occupies level 0
							{
								hlp = (HierarchyLevelProfile)hp.HierarchyLevels[level];
								if (hlp.LevelChangeType != eChangeType.delete)
								{
									HierarchyLevelInfo hli = new HierarchyLevelInfo();
									hli.Level = hlp.Level;
									hli.LevelID = hlp.LevelID;
									hli.LevelColor = hlp.LevelColor;
									hli.LevelType = hlp.LevelType;
									hli.LevelLengthType = hlp.LevelLengthType;
									hli.LevelRequiredSize = hlp.LevelRequiredSize;
									hli.LevelSizeRangeFrom = hlp.LevelSizeRangeFrom;
									hli.LevelSizeRangeTo = hlp.LevelSizeRangeTo;
									hli.LevelOTSPlanLevelType = hlp.LevelOTSPlanLevelType;
									//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
									//hli.LevelNodeCount = hlp.LevelNodeCount;
									hli.LevelNodesExist = hlp.LevelNodesExist;
									//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
									hli.LevelDisplayOption = hlp.LevelDisplayOption;
									hli.LevelIDFormat = hlp.LevelIDFormat;
									hli.PurgeDailyHistoryTimeframe = hlp.PurgeDailyHistoryTimeframe;
									hli.PurgeDailyHistory = hlp.PurgeDailyHistory;
									hli.PurgeWeeklyHistoryTimeframe = hlp.PurgeWeeklyHistoryTimeframe;
									hli.PurgeWeeklyHistory = hlp.PurgeWeeklyHistory;
									hli.PurgePlansTimeframe = hlp.PurgePlansTimeframe;
									hli.PurgePlans = hlp.PurgePlans;
                                    //hli.PurgeHeadersTimeframe = hlp.PurgeHeadersTimeframe;
                                    //hli.PurgeHeaders = hlp.PurgeHeaders;
                                    //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                                    // move profile to info
                                    hli.PurgeHtASNTimeframe = hlp.PurgeHtASNTimeframe;
                                    hli.PurgeHtASN = hlp.PurgeHtASN;
                                    hli.PurgeHtDropShipTimeframe = hlp.PurgeHtDropShipTimeframe;
                                    hli.PurgeHtDropShip = hlp.PurgeHtDropShip;
                                    hli.PurgeHtDummyTimeframe = hlp.PurgeHtDummyTimeframe;
                                    hli.PurgeHtDummy = hlp.PurgeHtDummy;
                                    hli.PurgeHtPurchaseOrderTimeframe = hlp.PurgeHtPurchaseOrderTimeframe;
                                    hli.PurgeHtPurchaseOrder = hlp.PurgeHtPurchaseOrder;
                                    hli.PurgeHtReceiptTimeframe = hlp.PurgeHtReceiptTimeframe;
                                    hli.PurgeHtReceipt = hlp.PurgeHtReceipt;
                                    hli.PurgeHtReserveTimeframe = hlp.PurgeHtReserveTimeframe;
                                    hli.PurgeHtReserve = hlp.PurgeHtReserve;
                                    hli.PurgeHtVSWTimeframe = hlp.PurgeHtVSWTimeframe;
                                    hli.PurgeHtVSW = hlp.PurgeHtVSW;
                                    hli.PurgeHtWorkUpTotTimeframe = hlp.PurgeHtWorkUpTotTimeframe;
                                    hli.PurgeHtWorkUpTot = hlp.PurgeHtWorkUpTot;
                                    //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                                    hi.HierarchyLevels.Add(hli.Level, hli);
								}
							}
							hi.HierarchyDBLevelsCount = hp.HierarchyLevels.Count;
							hi.HierarchyRootNodeRID = hp.HierarchyRootNodeRID;
							hi.HierarchyRID = hp.Key;
							hi.HierarchyType = hp.HierarchyType;
							hi.HierarchyRollupOption = hp.HierarchyRollupOption;
							hi.Owner = hp.Owner;
							hi.OTSPlanLevelType = hp.OTSPlanLevelType;

							// add relationship for new hierarchy root node
							HierarchyInfo.NodeRelationship nodeRelationship = new HierarchyInfo.NodeRelationship();
							// add main root as parent
							nodeRelationship.AddParent(0);
							hi.ParentChildRelationship.Add(hp.HierarchyRootNodeRID, nodeRelationship);

							nodeRelationship = new HierarchyInfo.NodeRelationship();
							nodeRelationship.AddChild(hp.HierarchyRootNodeRID);
							hi.ParentChildRelationship.Add(0, nodeRelationship);

							// add hierarchy
							_hierarchiesByRID.Add(hp.Key, hi);
							_hierarchiesByID.Add(hi.HierarchyID, hp.Key);

							//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
                            List<int> hierarchies;
                            if (!_userHierarchies.TryGetValue(hp.Owner, out hierarchies))
							{
                                hierarchies = new List<int>();
							}
							hierarchies.Add(hp.Key);
							_userHierarchies[hp.Owner] = hierarchies;
							//End Track #4815

							NodeInfo ni = new NodeInfo();
							ni.NodeRID = hi.HierarchyRootNodeRID;
							ni.NodeID = hi.HierarchyID;
							ni.NodeName = hi.HierarchyID;
							ni.NodeDescription = hi.HierarchyID;
							ni.HomeHierarchyRID = hi.HierarchyRID;
							ni.HomeHierarchyLevel = 0;
							ni.ProductTypeIsOverridden = false;
							ni.ProductType = eProductType.Undefined;
							ni.OTSPlanLevelIsOverridden = false;
							ePlanLevelSelectType OTSPlanLevelSelectType = ePlanLevelSelectType.Undefined;
							ePlanLevelLevelType OTSPlanLevelLevelType = ePlanLevelLevelType.Undefined;
							int	OTSPlanLevelHierarchyRID = Include.NoRID;
							int	OTSPlanLevelHierarchyLevelSequence = Include.Undefined;
							int	OTSPlanLevelAnchorNode = Include.Undefined;
							if (hp.OTSPlanLevelHierarchyLevelSequence != Include.Undefined)
							{
								OTSPlanLevelHierarchyRID = hp.Key;
								OTSPlanLevelHierarchyLevelSequence = hp.OTSPlanLevelHierarchyLevelSequence;
								OTSPlanLevelAnchorNode = hp.HierarchyRootNodeRID;
							}
							ni.OTSPlanLevelSelectType = OTSPlanLevelSelectType;
							ni.OTSPlanLevelLevelType = OTSPlanLevelLevelType;
							ni.OTSPlanLevelHierarchyRID = OTSPlanLevelHierarchyRID;
							ni.OTSPlanLevelHierarchyLevelSequence = OTSPlanLevelHierarchyLevelSequence;
							ni.OTSPlanLevelAnchorNode = OTSPlanLevelAnchorNode;
							ni.OTSPlanLevelMaskField = eMaskField.Undefined;
							ni.OTSPlanLevelMask = null;
							ni.OTSPlanLevelTypeIsOverridden = false;
							ni.OTSPlanLevelType = eOTSPlanLevelType.Undefined;
							ni.IsVirtual = false;
                            ni.Purpose = ePurpose.Default;
                            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                            ni.Active = true;
                            // End TT#988
							// BEGIN TT#1399 - GRT - Alternate Hierarchy Inherit Node Properties
                            ni.ApplyHNRIDFrom = Include.NoRID;
                            // END TT#1399 
							SetNodeCacheByRID(ni.NodeRID, ni);
							SetNodeRIDByID(ni.NodeID, ni.NodeRID);
							break;
						}
						case eChangeType.update: 
						{
							HierarchyInfo hi = (HierarchyInfo)_hierarchiesByRID[hp.Key];
							NodeInfo ni = GetNodeCacheByRID(hp.HierarchyRootNodeRID);
							_hierarchiesByID.Remove(hi.HierarchyID);
							RemoveNodeRIDByID(ni.NodeID);

                            // Begin Track #6302 - JSmith - Assign user permanently to another, folders do not come over
                            bool ownerChanged = false;
                            // remove hierarchy from prior owner
                            if (hi.Owner != hp.Owner)
                            {
                                List<int> hierarchies;
                                if (!_userHierarchies.TryGetValue(hp.Owner, out hierarchies))
                                {
                                    hierarchies = new List<int>();
                                }
                                hierarchies.Remove(hp.Key);
                                _userHierarchies[hi.Owner] = hierarchies;
                                hi.Owner = hp.Owner;
                                ownerChanged = true;
                            }
                            //End Track #6302

							hi.HierarchyColor = hp.HierarchyColor;
							hi.HierarchyID = hp.HierarchyID;
							hi.HierarchyLevels.Clear();
							for (int level = 1; level <= hp.HierarchyLevels.Count; level++)  // levels begin at 1 since the hierarchy occupies level 0
							{
								hlp = (HierarchyLevelProfile)hp.HierarchyLevels[level];
								if (hlp.LevelChangeType != eChangeType.delete)
								{
									HierarchyLevelInfo hli = new HierarchyLevelInfo();
									hli.Level = hlp.Level;
									hli.LevelID = hlp.LevelID;
									hli.LevelColor = hlp.LevelColor;
									hli.LevelType = hlp.LevelType;
									hli.LevelLengthType = hlp.LevelLengthType;
									hli.LevelRequiredSize = hlp.LevelRequiredSize;
									hli.LevelSizeRangeFrom = hlp.LevelSizeRangeFrom;
									hli.LevelSizeRangeTo = hlp.LevelSizeRangeTo;
									hli.LevelOTSPlanLevelType = hlp.LevelOTSPlanLevelType;
									//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
									//hli.LevelNodeCount = hlp.LevelNodeCount;
									hli.LevelNodesExist = hlp.LevelNodesExist;
									//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
									hli.LevelDisplayOption = hlp.LevelDisplayOption;
									hli.LevelIDFormat = hlp.LevelIDFormat;
									hli.PurgeDailyHistoryTimeframe = hlp.PurgeDailyHistoryTimeframe;
									hli.PurgeDailyHistory = hlp.PurgeDailyHistory;
									hli.PurgeWeeklyHistoryTimeframe = hlp.PurgeWeeklyHistoryTimeframe;
									hli.PurgeWeeklyHistory = hlp.PurgeWeeklyHistory;
									hli.PurgePlansTimeframe = hlp.PurgePlansTimeframe;
									hli.PurgePlans = hlp.PurgePlans;
                                    //hli.PurgeHeadersTimeframe = hlp.PurgeHeadersTimeframe;
                                    //hli.PurgeHeaders = hlp.PurgeHeaders;
                                    //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                                    // move profile to info
                                    hli.PurgeHtASNTimeframe = hlp.PurgeHtASNTimeframe;
                                    hli.PurgeHtASN = hlp.PurgeHtASN;
                                    hli.PurgeHtDropShipTimeframe = hlp.PurgeHtDropShipTimeframe;
                                    hli.PurgeHtDropShip = hlp.PurgeHtDropShip;
                                    hli.PurgeHtDummyTimeframe = hlp.PurgeHtDummyTimeframe;
                                    hli.PurgeHtDummy = hlp.PurgeHtDummy;
                                    hli.PurgeHtPurchaseOrderTimeframe = hlp.PurgeHtPurchaseOrderTimeframe;
                                    hli.PurgeHtPurchaseOrder = hlp.PurgeHtPurchaseOrder;
                                    hli.PurgeHtReceiptTimeframe = hlp.PurgeHtReceiptTimeframe;
                                    hli.PurgeHtReceipt = hlp.PurgeHtReceipt;
                                    hli.PurgeHtReserveTimeframe = hlp.PurgeHtReserveTimeframe;
                                    hli.PurgeHtReserve = hlp.PurgeHtReserve;
                                    hli.PurgeHtVSWTimeframe = hlp.PurgeHtVSWTimeframe;
                                    hli.PurgeHtVSW = hlp.PurgeHtVSW;
                                    hli.PurgeHtWorkUpTotTimeframe = hlp.PurgeHtWorkUpTotTimeframe;
                                    hli.PurgeHtWorkUpTot = hlp.PurgeHtWorkUpTot;
                                    //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                                    hi.HierarchyLevels.Add(hli.Level, hli);
								}
							}
							hi.HierarchyDBLevelsCount = hp.HierarchyLevels.Count;
							hi.HierarchyRootNodeRID = hp.HierarchyRootNodeRID;
							hi.HierarchyRID = hp.Key;
							hi.HierarchyType = hp.HierarchyType;
							hi.HierarchyRollupOption = hp.HierarchyRollupOption;
							hi.OTSPlanLevelType = hp.OTSPlanLevelType;
							_hierarchiesByRID[hp.Key] = hi;
							_hierarchiesByID.Add(hi.HierarchyID, hp.Key);

							ni.NodeID = hp.HierarchyID;
							ni.NodeName = hp.HierarchyID;
							ni.NodeDescription = hp.HierarchyID;
							ni.IsVirtual = false;
							SetNodeCacheByRID(hi.HierarchyRootNodeRID, ni);
							SetNodeRIDByID(hp.HierarchyID, hi.HierarchyRootNodeRID);
                            // Begin Track #6302 - JSmith - Assign user permanently to another, folders do not come over
                            if (ownerChanged)
                            {
                                List<int> hierarchies;
                                if (!_userHierarchies.TryGetValue(hp.Owner, out hierarchies))
                                {
                                    hierarchies = new List<int>();
                                }
                                hierarchies.Add(hp.Key);
                                _userHierarchies[hp.Owner] = hierarchies;
                            }
                            //End Track #6302
							break;
						}
						case eChangeType.delete: 
						{
							RemoveNodeCacheByRID(hp.HierarchyRootNodeRID);
							RemoveNodeRIDByID(hp.HierarchyID);
							_hierarchiesByRID.Remove(hp.Key);
							_hierarchiesByID.Remove(hp.HierarchyID);

                            List<int> hierarchies;
                            if (!_userHierarchies.TryGetValue(hp.Owner, out hierarchies))
                            {
                                hierarchies = new List<int>();
                            }
							hierarchies.Remove(hp.Key);
							_userHierarchies[hp.Owner] = hierarchies;
							break;
						}
                        // Begin TT#3630 - JSmith - Delete My Hierarchy
                        case eChangeType.markedForDelete:
                        {
                            HierarchyInfo hi = (HierarchyInfo)_hierarchiesByRID[hp.Key];
                            NodeInfo ni = GetNodeCacheByRID(hp.HierarchyRootNodeRID);
                            _hierarchiesByID.Remove(hi.HierarchyID);
                            //RemoveNodeRIDByID(ni.NodeID);
                            hi.HierarchyID = hp.HierarchyID;
                            //ni.DeleteNode = true;
                            //ni.NodeID = hp.HierarchyID;
                            _hierarchiesByRID[hp.Key] = hi;
                            _hierarchiesByID.Add(hi.HierarchyID, hp.Key);
                            //SetNodeCacheByRID(hi.HierarchyRootNodeRID, ni);
                            //SetNodeRIDByID(hp.HierarchyID, hi.HierarchyRootNodeRID);
                            break;
                        }
                        // End TT#3630 - JSmith - Delete My Hierarchy
					}
					return hp;
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					hierarchy_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:HierarchyUpdate writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:HierarchyUpdate writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Requests the session get all hierarchies in dependency order.  This means that a hierarchy in
		/// the list does not contain a reference to any other hierarchy later in the list. 
		/// </summary>
		/// <returns></returns>
        // Begin TT#2064 - JSmith - Add message to Rollup when hierarchy dependency build fails
        //static public HierarchyProfileList GetHierarchiesByDependency()
        // Begin TT#746-MD - JSmith - Unable to override message level for Circular Hierarchy Relationship
        //static public HierarchyProfileList GetHierarchiesByDependency(ref string aReturnMessage)
        static public HierarchyProfileList GetHierarchiesByDependency(ref string aReturnMessage, ref eMIDMessageLevel aMessageLevel)
        // End TT#746-MD - JSmith - Unable to override message level for Circular Hierarchy Relationship
        // End TT#2064
		{
			try
			{
                //static public HierarchyProfileList GetHierarchiesByDependency()
                aReturnMessage = null;
                // End TT#2064
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				int [] hierarchyRIDs;
				HierarchyProfileList hpl = new HierarchyProfileList(eProfileType.Hierarchy);
				// add main hierarchy because it can not reference any other hierarchy
				HierarchyProfile hp = GetMainHierarchyData();
				hpl.Add(hp);
				// get list of all alternate hierarchy RIDs to loop through
                //Begin TT#1323 - JSmith - Rollup never completed and had to be cancelled
                int hierarchiesAdded = 0;
                Hashtable htHierarchyXref = new Hashtable();
                ArrayList hierarchyXref = null;
                //End TT#1323
				try
				{
					hierarchyRIDs = new int[_hierarchiesByRID.Count - 1];
					hierarchy_rwl.AcquireReaderLock(ReaderLockTimeOut);
					try
					{
						int i = 0;
						foreach (HierarchyInfo hi in _hierarchiesByRID.Values)
						{
							if (hi.HierarchyRID != hp.Key)
							{
								hierarchyRIDs[i] = hi.HierarchyRID;
								++i;
							}
						}
					}
					catch ( Exception ex )
					{
                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                        if (Audit != null)
                        {
                            Audit.Log_Exception(ex);
                        }
                        // End TT#189
					}
					finally
					{
						// Ensure that the lock is released.
						hierarchy_rwl.ReleaseReaderLock();
					}
				}
				catch (ApplicationException ex)
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					// The reader lock request timed out.
					EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetHierarchiesByDependency reader lock has timed out", EventLogEntryType.Error);
					throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetHierarchiesByDependency reader lock has timed out");
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}

				// loop through each hierarchy to build dependency list
				bool done = false;
                // Begin TT#2978 - JSmith - Hierarchy Circular Reference Warning message is logged without any Circular References
                // Do not check if no alternate hierarchies
                if (hierarchyRIDs.Length == 0)
                {
                    done = true;
                }
                // End TT#2978 - JSmith - Hierarchy Circular Reference Warning message is logged without any Circular References
				bool canAddHierarchy = false;
				while (!done)
				{
                    //Begin TT#1323 - JSmith - Rollup never completed and had to be cancelled
                    hierarchiesAdded = 0;
                    //End TT#1323
					for (int i = 0; i < hierarchyRIDs.Length; i++)
					{
						if (hierarchyRIDs[i] != 0)
						{
                            //Begin TT#1323 - JSmith - Rollup never completed and had to be cancelled
                            //ArrayList hierarchyXref = mhd.GetHierarchyXref(hierarchyRIDs[i]);
                            hierarchyXref = (ArrayList)htHierarchyXref[hierarchyRIDs[i]];
                            if (hierarchyXref == null)
                            {
                                hierarchyXref = mhd.GetHierarchyXref(hierarchyRIDs[i]);
                            }
                            //End TT#1323
							canAddHierarchy = true;
							// if hierarchy reference other hierarchies, make sure they are already on the list
							if (hierarchyXref.Count > 0)
							{
								foreach (int hierarchyRID in hierarchyXref)
								{
									if (!hpl.Contains(hierarchyRID))
									{
										canAddHierarchy = false;
										break;
									}
								}
							}
                            if (canAddHierarchy)
                            {
                                hp = GetHierarchyData(hierarchyRIDs[i]);
                                hpl.Add(hp);
                                hierarchyRIDs[i] = 0;
                                //Begin TT#1323 - JSmith - Rollup never completed and had to be cancelled
                                ++hierarchiesAdded;
                                //End TT#1323
                            }
                            //Begin TT#1323 - JSmith - Rollup never completed and had to be cancelled
                            else if (!htHierarchyXref.ContainsKey(hierarchyRIDs[i])) 
                            {
                                htHierarchyXref.Add(hierarchyRIDs[i], hierarchyXref);
                            }
                            //End TT#1323
						}
					}

					if (hpl.Count == hierarchyRIDs.Length + 1)
					{
						done = true;
					}

                    //Begin TT#1323 - JSmith - Rollup never completed and had to be cancelled
                    if (hierarchiesAdded == 0)
                    {
                        // Begin TT#868-MD - JSmith - Rollup reports a circular relationship warning when there is not a circular relationship
                        bool blCircularAdded = false;
                        // End TT#868-MD - JSmith - Rollup reports a circular relationship warning when there is not a circular relationship
                        done = true;
                        string message = MIDText.GetText(eMIDTextCode.msg_CircularHierarchiesHeader);
                        // Begin TT#746-MD - JSmith - Unable to override message level for Circular Hierarchy Relationship
                        aMessageLevel = MIDText.GetMessageLevel((int)eMIDTextCode.msg_CircularHierarchiesHeader);
                        // End TT#746-MD - JSmith - Unable to override message level for Circular Hierarchy Relationship
                        for (int i = 0; i < hierarchyRIDs.Length; i++)
                        {
                            if (hierarchyRIDs[i] != 0)
                            {
                                hp = GetHierarchyData(hierarchyRIDs[i]);
                                string msgHierarchies = string.Empty;
                                hierarchyXref = (ArrayList)htHierarchyXref[hierarchyRIDs[i]];
                                if (hierarchyXref != null)
                                {
                                    foreach (int hierarchyRID in hierarchyXref)
                                    {
                                        if (msgHierarchies.Length > 0)
                                        {
                                            msgHierarchies += ",";
                                        }
                                        msgHierarchies += GetHierarchyData(hierarchyRID).HierarchyID;
                                    }
                                    message += Environment.NewLine + MIDText.GetTextOnly(eMIDTextCode.msg_CircularHierarchiesDetail, hp.HierarchyID, msgHierarchies);
                                    // Begin TT#868-MD - JSmith - Rollup reports a circular relationship warning when there is not a circular relationship
                                    blCircularAdded = true;
                                    // End TT#868-MD - JSmith - Rollup reports a circular relationship warning when there is not a circular relationship
                                }
                            }
                        }
                        //static public HierarchyProfileList GetHierarchiesByDependency()
                        // Begin TT#868-MD - JSmith - Rollup reports a circular relationship warning when there is not a circular relationship
                        //aReturnMessage = message;
                        // End TT#2064
                        //if (Audit != null)
                        //{
                        //    Audit.Add_Msg(eMIDMessageLevel.Warning, message, System.Reflection.MethodBase.GetCurrentMethod().Name);
                        //}
                        if (blCircularAdded)
                        {
                            aReturnMessage = message;
                            if (Audit != null)
                            {
                                Audit.Add_Msg(eMIDMessageLevel.Warning, message, System.Reflection.MethodBase.GetCurrentMethod().Name);
                            }
                        }
                        // End TT#868-MD - JSmith - Rollup reports a circular relationship warning when there is not a circular relationship
                    }
                    //End TT#1323
				}
				return hpl;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        /// <summary>
        /// Retrieves all hierarchies in for a user. 
        /// </summary>
        /// <returns></returns>
        static public HierarchyProfileList GetHierarchiesForUser(int aUserRID)
        {
            HierarchyProfile hp;
            try
            {
                MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                int[] hierarchyRIDs;
                HierarchyProfileList hpl = new HierarchyProfileList(eProfileType.Hierarchy);
                try
                {
                    hierarchyRIDs = new int[_hierarchiesByRID.Count];
                    hierarchy_rwl.AcquireReaderLock(ReaderLockTimeOut);
                    try
                    {
                        int i = 0;
                        foreach (HierarchyInfo hi in _hierarchiesByRID.Values)
                        {
                            hierarchyRIDs[i] = hi.HierarchyRID;
                            ++i;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                        if (Audit != null)
                        {
                            Audit.Log_Exception(ex);
                        }
                        // End TT#189
                    }
                    finally
                    {
                        // Ensure that the lock is released.
                        hierarchy_rwl.ReleaseReaderLock();
                    }
                }
                catch (ApplicationException ex)
                {
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
                    // The reader lock request timed out.
                    EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetHierarchiesByDependency reader lock has timed out", EventLogEntryType.Error);
                    throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:GetHierarchiesByDependency reader lock has timed out");
                }
                catch (Exception ex)
                {
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
                    throw;
                }

                // loop through each hierarchy to check owner
                for (int i = 0; i < hierarchyRIDs.Length; i++)
                {
                    hp = GetHierarchyData(hierarchyRIDs[i]);
                    if (hp.Owner == aUserRID)
                    {
                        hpl.Add(hp);
                    }
                }

                return hpl;
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

		static public bool AlternateAPIRollupExists()
		{
			try
			{
				hierarchy_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
					foreach (HierarchyInfo hi in _hierarchiesByRID.Values)
					{
						if (hi.HierarchyType == eHierarchyType.alternate &&
							hi.HierarchyRollupOption == eHierarchyRollupOption.API)
						{
							return true;
						}
					}
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
				}
				finally
				{
					// Ensure that the lock is released.
					hierarchy_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:AlternateAPIRollupExists reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:AlternateAPIRollupExists reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
			return false;
		}

        // Begin TT#222 - JSmith - Apply to lower levels in alternate affects guest nodes
        ///// <summary>
        ///// Counts to number of descendants under a node.
        ///// </summary>
        ///// <param name="HierarchyRID">The record ID of the hierarchy to be counted</param>
        ///// <param name="nodeRID">The record ID of the node to be counted</param>
        ///// <returns>An integer containing the number of descendants for the node </returns>
        //static public int GetDescendantCount(int HierarchyRID, int nodeRID)
        //{
        //    try
        //    {
        //            int count = 0;
        //            HierarchyInfo hi = GetHierarchyInfoByRID(HierarchyRID, true);
        //            CountDescendants(ref count, hi, nodeRID);
			
        //            return count;
        //    }
        //    catch ( Exception ex )
        //    {
        //        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
        //        if (Audit != null)
        //        {
        //            Audit.Log_Exception(ex);
        //        }
        //        // End TT#189
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Recursively navigates hierarchy and counts all descendants
        ///// </summary>
        ///// <param name="count">The count of the descendants</param>
        ///// <param name="hi">An instance of the HierarchyInfo class containing information for the hierarchy</param>
        ///// <param name="nodeRID">The record ID of the node</param>
        //static private void CountDescendants(ref int count, HierarchyInfo hi, int nodeRID)
        //{
        //    // Begin TT#222 - JSmith - Apply to lower levels in alternate affects guest nodes
        //    NodeInfo ni;
        //    // End TT#222
        //    try
        //    {
        //        HierarchyInfo.NodeRelationship nr = hi.GetChildRelationship(nodeRID);
        //        // Begin TT#222 - JSmith - Apply to lower levels in alternate affects guest nodes
        //        //count += nr.ChildrenCount();
        //        // End TT#222
        //        if (nr.ChildrenCount() > 0)
        //        {
        //            foreach (int childRID in nr.children)  //  Get the children of the children
        //            {
        //                // Begin TT#222 - JSmith - Apply to lower levels in alternate affects guest nodes
        //                //CountDescendants(ref count, hi, childRID);
        //                ni = GetNodeInfoByRID(childRID);
        //                if (ni.HomeHierarchyRID == hi.HierarchyRID)
        //                {
        //                    count += 1;
        //                    CountDescendants(ref count, hi, childRID);
        //                }
        //                // End TT#222
        //            }
        //        }
        //    }
        //    catch ( Exception ex )
        //    {
        //        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
        //        if (Audit != null)
        //        {
        //            Audit.Log_Exception(ex);
        //        }
        //        // End TT#189
        //        throw;
        //    }
        //}
        /// <summary>
        /// Counts to number of descendants under a node.
        /// </summary>
        /// <param name="HierarchyRID">The record ID of the hierarchy to be counted</param>
        /// <param name="nodeRID">The record ID of the node to be counted</param>
        /// <returns>An integer containing the number of descendants for the node </returns>
        static public int GetDescendantCount(int HierarchyRID, int nodeRID, bool aHomeHierarchyOnly)
        {
            try
            {
                int count = 0;
                HierarchyInfo hi = GetHierarchyInfoByRID(HierarchyRID, true);
                CountDescendants(ref count, hi, nodeRID, aHomeHierarchyOnly);

                return count;
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }

        /// <summary>
        /// Recursively navigates hierarchy and counts all descendants
        /// </summary>
        /// <param name="count">The count of the descendants</param>
        /// <param name="hi">An instance of the HierarchyInfo class containing information for the hierarchy</param>
        /// <param name="nodeRID">The record ID of the node</param>
        static private void CountDescendants(ref int count, HierarchyInfo hi, int nodeRID, bool aHomeHierarchyOnly)
        {
            NodeInfo ni;
            try
            {
                HierarchyInfo.NodeRelationship nr = hi.GetChildRelationship(nodeRID);
                if (nr.ChildrenCount() > 0)
                {
                    foreach (int childRID in nr.children)  //  Get the children of the children
                    {
                        ni = GetNodeInfoByRID(childRID);
                        if ((aHomeHierarchyOnly && ni.HomeHierarchyRID == hi.HierarchyRID) ||
                            !aHomeHierarchyOnly)
                        {
                            count += 1;
                            CountDescendants(ref count, hi, childRID, aHomeHierarchyOnly);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }
        // End TT#222

		//Begin Track #5378 - color and size not qualified
		/// <summary>
		/// Gets information about a node using the nodes id
		/// </summary>
		/// <param name="nodeID">The id of the node</param>
		/// <param name="chaseHierarchy">
		/// This switch identifies if the routine is to chase the hierarchy
		/// for settings if they are not set on the requested node
		/// </param>
		/// <remarks>
		/// This will not return a qualified node ID for colors and sizes
		/// </remarks>
		/// <returns>An instance of the HierarchyNodeProfile class containing the information for the node</returns>
		static public HierarchyNodeProfile GetNodeData(string nodeID, bool chaseHierarchy)
		{
			try
			{
				return GetNodeData(nodeID, chaseHierarchy, false);
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}
		//End Track #5378

        //Begin TT#1373-MD -jsobek Key in Style\Color for Merchandise and only the Color displays in the Details 
        static public HierarchyNodeProfile GetNodeDataWithQualifiedID(string nodeID, bool chaseHierarchy)
        {
            try
            {
                return GetNodeData(nodeID, chaseHierarchy, true);
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }
        //End TT#1373-MD -jsobek Key in Style\Color for Merchandise and only the Color displays in the Details 


        //Begin TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
        static public HierarchyNodeProfile GetNodeDataFromBaseSearchString(string searchString)
		{
            try
            {
                MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                DataTable dt = mhd.Hierarchy_NodeRID_Read_From_Base_Search_String(searchString);

                int nodeRID = Include.NoRID;
                if (dt.Rows.Count > 0)
                {
                    nodeRID = Convert.ToInt32(dt.Rows[0]["HN_RID"], CultureInfo.CurrentUICulture);
                }
               

                return GetNodeData(nodeRID, false, true); //TT#1373-MD -jsobek Key in Style\Color for Merchandise and only the Color displays in the Details 


            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
		}
        
       
        //End TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis

		/// <summary>
		/// Gets information about a node using the nodes id
		/// </summary>
		/// <param name="nodeID">The id of the node</param>
		/// <param name="chaseHierarchy">
		/// This switch identifies if the routine is to chase the hierarchy
		/// for settings if they are not set on the requested node
		/// </param>
		/// <param name="aBuildQualifiedID">
		/// This switch identifies that the node is being looked up should
		/// have the ID include parents when necessary to identify
		/// </param>
		/// <returns>An instance of the HierarchyNodeProfile class containing the information for the node</returns>
		//Begin Track #5378 - color and size not qualified
//		static public HierarchyNodeProfile GetNodeData(string nodeID, bool chaseHierarchy)
		static public HierarchyNodeProfile GetNodeData(string nodeID, bool chaseHierarchy, bool aBuildQualifiedID)
		//End Track #5378
		{
			try
			{
				int nodeRID;
				object RID = GetNodeRIDByID(nodeID);
				if (RID != null)
				{
					nodeRID = (int)RID;
				}
				else
				{
					HierarchyNodeProfile hnp = new HierarchyNodeProfile(Include.NoRID);
					hnp.Key = Include.NoRID;  // not found
					return hnp;
				}

				//Begin Track #5378 - color and size not qualified
//				return GetNodeData(nodeRID, chaseHierarchy);
				return GetNodeData(nodeRID, chaseHierarchy, aBuildQualifiedID);
				//End Track #5378
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the record id of the node
		/// </summary>
		/// <param name="nodeID">The id of the node</param>
		/// <returns>The record ID of the node or -1 if the node ID is not found</returns>
		static public int GetNodeRID(string nodeID)
		{
			try
			{
				object RID = GetNodeRIDByID(nodeID);
				int nodeRID = Include.NoRID;
				if (RID != null)
				{
					nodeRID = (int)RID;
				}
				return nodeRID;
				
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the record id of the node.
		/// </summary>
		/// <param name="aHnp">The record id of the node</param>
		/// <returns>The record ID of the node or -1 if the node ID is not found</returns>
		static public int GetNodeRID(HierarchyNodeProfile aHnp)
		{
			try
			{
				int nodeRID = Include.NoRID;
				if (aHnp.LevelType == eHierarchyLevelType.Color)
				{
					ColorExistsForStyle(aHnp.HomeHierarchyRID, aHnp.HomeHierarchyParentRID, aHnp.NodeID, aHnp.QualifiedNodeID, ref nodeRID);
				}
				else if (aHnp.LevelType == eHierarchyLevelType.Size)
				{
					SizeExistsForColor(aHnp.HierarchyRID, aHnp.HomeHierarchyParentRID, aHnp.NodeID, aHnp.QualifiedNodeID, ref nodeRID);
				}
				else
				{
					object RID = GetNodeRIDByID(aHnp.NodeID);
					if (RID != null)
					{
						nodeRID = Convert.ToInt32(RID);
					}
				}

				return nodeRID;
				
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the id of the node
		/// </summary>
		/// <param name="aNodeRID">The record ID of the node</param>
		static public string GetNodeID(int aNodeRID)
		{
			try
			{
				NodeInfo ni = GetNodeInfoByRID(aNodeRID);
				if (ni == null)
				{
					throw new MIDException(eErrorLevel.severe, 0, "Node record ID=" + aNodeRID.ToString() + " not found");
				}
				return ni.NodeID;
				
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the text of the node
		/// </summary>
		/// <param name="aNodeRID">The record ID of the node</param>
		static public string GetNodeText(int aNodeRID)
		{
			try
			{
				HierarchyNodeProfile hnp = GetNodeData(aNodeRID, false);
				return hnp.Text;
				
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the id of the node and the parent node
		/// </summary>
		/// <param name="aNodeRID">The record ID of the node</param>
		static public HierarchyNodeAndParentIdsProfile GetNodeIDAndParentID(int aNodeRID)
		{
			try
			{
				HierarchyNodeAndParentIdsProfile hnpnp = new HierarchyNodeAndParentIdsProfile(aNodeRID);
				NodeInfo ni = GetNodeInfoByRID(aNodeRID, false);
				if (ni == null)
				{
					throw new MIDException(eErrorLevel.severe, 0, "Node record ID=" + aNodeRID.ToString() + " not found");
				}
				hnpnp.NodeID = ni.NodeID;
					
				ArrayList parents = GetParentRIDs(ni.NodeRID, ni.HomeHierarchyRID);
				foreach (int parentRID in parents)
				{
					if (parentRID > 0)
					{
						ni = GetNodeInfoByRID(parentRID, false);
						if (ni == null)
						{
							throw new MIDException(eErrorLevel.severe, 0, "Node record ID=" + aNodeRID.ToString() + " not found");
						}
						hnpnp.Parents.Add(parentRID);
						hnpnp.ParentNodeIDs.Add(ni.NodeID);
					}
				}
				
				return hnpnp;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Returns the Hashtable of the nodes in the system keyed by ID.
		/// </summary>
		static public Hashtable GetNodeListByID()
		{
			try
			{
				lock (_cache_lock.SyncRoot)
				{
					return _nodesByID.GetHashCopy();
				}        
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		//Begin Track #5378 - color and size not qualified
		/// <summary>
		/// Gets information about a node using the nodes record id
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <returns>An instance of the HierarchyNodeProfile class containing the information for the node</returns>
		/// <remarks>
		/// For performance reasons, NodeLevel will only be set for guest nodes if aChaseHierarchy is set to true
		/// This will not return a qualified node ID for colors and sizes
		/// </remarks>
		static public HierarchyNodeProfile GetNodeData(int hierarchyRID, int nodeRID, bool aChaseHierarchy)
		{
			return GetNodeData(hierarchyRID, nodeRID, aChaseHierarchy, false);
		}

		//End Track #5378

		/// <summary>
		/// Gets information about a node using the nodes record id
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <returns>An instance of the HierarchyNodeProfile class containing the information for the node</returns>
		/// <remarks>
		/// For performance reasons, NodeLevel will only be set for guest nodes if aChaseHierarchy is set to true
		/// </remarks>
		//Begin Track #5378 - color and size not qualified
//		static public HierarchyNodeProfile GetNodeData(int hierarchyRID, int nodeRID, bool aChaseHierarchy)
		static public HierarchyNodeProfile GetNodeData(int hierarchyRID, int nodeRID, bool aChaseHierarchy, bool aBuildQualifiedID)
		//End Track #5378
		{
			try
			{
				//Begin Track #5378 - color and size not qualified
//				HierarchyNodeProfile hnp =  GetNodeData(nodeRID, aChaseHierarchy);
				HierarchyNodeProfile hnp =  GetNodeData(nodeRID, aChaseHierarchy, aBuildQualifiedID);
				//End Track #5378
				if (hnp.Key != Include.NoRID)
				{
					//						hnp.ParentRID = GetParentRIDs(hnp.Key, hierarchyRID);
					hnp.Parents = GetParentRIDs(hnp.Key, hierarchyRID);
					ArrayList homeParents;
					if (hierarchyRID == hnp.HomeHierarchyRID)
					{
						homeParents = hnp.Parents;
					}
					else
					{
						homeParents = GetParentRIDs(hnp.Key, hnp.HomeHierarchyRID);
					}
					if (homeParents.Count == 1)
					{
						hnp.HomeHierarchyParentRID = Convert.ToInt32(homeParents[0], CultureInfo.CurrentCulture);
					}

					hnp.HierarchyRID = hierarchyRID;
					if (hnp.HomeHierarchyRID != hierarchyRID &&
						aChaseHierarchy)
					{
						NodeAncestorList nal = GetNodeAncestorList(nodeRID, hierarchyRID);
						hnp.NodeLevel = nal.Count - 1;	// ancestor list includes yourself so subtract out
					}
				}

				return hnp;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets information about a node using the nodes record id
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <remarks>
		/// This will not return a qualified node ID for colors and sizes
		/// </remarks>
		/// <returns>An instance of the HierarchyNodeProfile class containing the information for the node</returns>
		static public HierarchyNodeProfile GetNodeData(int nodeRID)
		{
			return GetNodeData(nodeRID, true);
		}

		//Begin Track #5378 - color and size not qualified
		/// <summary>
		/// Gets information about a node using the nodes record id
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="chaseHierarchy">
		/// This switch identifies if the routine is to chase the hierarchy
		/// for settings if they are not set on the requested node
		/// </param>
		/// <remarks>
		/// This will not return a qualified node ID for colors and sizes
		/// </remarks>
		/// <returns>An instance of the HierarchyNodeProfile class containing the information for the node</returns>
		static public HierarchyNodeProfile GetNodeData(int nodeRID, bool chaseHierarchy)
		{
			return GetNodeData(nodeRID, chaseHierarchy, false);
		}
		//End Track #5378

		/// <summary>
		/// Gets information about a node using the nodes record id
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="chaseHierarchy">
		/// This switch identifies if the routine is to chase the hierarchy
		/// for settings if they are not set on the requested node
		/// </param>
		/// <param name="aBuildQualifiedID">
		/// This switch identifies that the node is being looked up should
		/// have the ID include parents when necessary to identify
		/// </param>
		/// <returns>An instance of the HierarchyNodeProfile class containing the information for the node</returns>
		//Begin Track #5378 - color and size not qualified
        //static public HierarchyNodeProfile GetNodeData(int nodeRID, bool chaseHierarchy)
        static public HierarchyNodeProfile GetNodeData(int nodeRID, bool chaseHierarchy, bool aBuildQualifiedID) 
		//End Track #5378
		{
			try
			{
				NodeInfo ni;
				try
				{
					ni = GetNodeInfoByRID(nodeRID, false);
					HierarchyNodeProfile hnp = new HierarchyNodeProfile(ni.NodeRID);
					if (ni.NodeRID <= 0)
					{
						return hnp;
					}
					hnp.NodeChangeType = eChangeType.none;
					hnp.NodeID = ni.NodeID;
                    // Begin TT#5185 - JSmith - Error creating Group
                    //hnp.NodeName = ni.NodeName;
                    //hnp.NodeDescription = ni.NodeDescription;
                    if (string.IsNullOrEmpty(ni.NodeName))
                    {
                        hnp.NodeName = hnp.NodeID;
                    }
                    else
                    {
                        hnp.NodeName = ni.NodeName;
                    }
                    if (string.IsNullOrEmpty(ni.NodeDescription))
                    {
                        hnp.NodeDescription = hnp.NodeName;
                    }
                    else
                    {
                        hnp.NodeDescription = ni.NodeDescription;
                    }
                    // Begin TT#5185 - JSmith - Error creating Group
					// BEGIN TT#1399
                    hnp.ApplyHNRIDFrom = ni.ApplyHNRIDFrom;
                  // END TT#1399
					hnp.Parents = GetParentRIDs(ni.NodeRID, ni.HomeHierarchyRID);
					if (hnp.Parents.Count > 0)
					{
						hnp.HomeHierarchyParentRID = Convert.ToInt32(hnp.Parents[0], CultureInfo.CurrentCulture);
					}

					hnp.HierarchyRID = ni.HomeHierarchyRID;
					hnp.HomeHierarchyRID = ni.HomeHierarchyRID;
					hnp.HomeHierarchyLevel = ni.HomeHierarchyLevel;
					hnp.NodeLevel = ni.HomeHierarchyLevel;   
					hnp.ProductTypeIsOverridden = ni.ProductTypeIsOverridden;
					// if the Product Type is not overridden, set it to undefined so that it can inherit
					if (hnp.ProductTypeIsOverridden)
					{
						hnp.ProductType = ni.ProductType;
					}
					else
					{
						hnp.ProductType = eProductType.Undefined;
					}

					hnp.OTSPlanLevelIsOverridden = ni.OTSPlanLevelIsOverridden;
					// if the OTS Plan Level is not overridden, set it to undefined so that it can inherit
					if (hnp.OTSPlanLevelIsOverridden)
					{
						hnp.OTSPlanLevelSelectType = ni.OTSPlanLevelSelectType;
						hnp.OTSPlanLevelLevelType = ni.OTSPlanLevelLevelType;
						hnp.OTSPlanLevelHierarchyRID = ni.OTSPlanLevelHierarchyRID;
						hnp.OTSPlanLevelHierarchyLevelSequence = ni.OTSPlanLevelHierarchyLevelSequence;
						hnp.OTSPlanLevelAnchorNode = ni.OTSPlanLevelAnchorNode;
						hnp.OTSPlanLevelMaskField = ni.OTSPlanLevelMaskField;
						hnp.OTSPlanLevelMask = ni.OTSPlanLevelMask;
					}
					else
					{
						hnp.OTSPlanLevelSelectType = ePlanLevelSelectType.Undefined;
						hnp.OTSPlanLevelLevelType = ePlanLevelLevelType.Undefined;
						hnp.OTSPlanLevelHierarchyRID = Include.NoRID;
						hnp.OTSPlanLevelHierarchyLevelSequence = Include.Undefined;
						hnp.OTSPlanLevelAnchorNode = Include.NoRID;
						hnp.OTSPlanLevelMaskField = eMaskField.Undefined;
						hnp.OTSPlanLevelMask = null;
					}

					hnp.OTSPlanLevelTypeIsOverridden = ni.OTSPlanLevelTypeIsOverridden;
					// if the OTS Plan Level Type is not overridden, set it to undefined so that it can inherit
					if (hnp.OTSPlanLevelTypeIsOverridden)
					{
						hnp.OTSPlanLevelType = ni.OTSPlanLevelType;
					}
					else
					{
						hnp.OTSPlanLevelType = eOTSPlanLevelType.Undefined;
					}

					hnp.UseBasicReplenishment = ni.UseBasicReplenishment;
					hnp.ColorOrSizeCodeRID = ni.ColorOrSizeCodeRID;
					hnp.PurgeDailyHistoryAfter = ni.PurgeDailyHistoryAfter;
					hnp.PurgeWeeklyHistoryAfter = ni.PurgeWeeklyHistoryAfter;
					hnp.PurgeOTSPlansAfter = ni.PurgeOTSPlansAfter;
                    //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                    //hnp.PurgeHeadersAfter = ni.PurgeHeadersAfter;
                    hnp.PurgeHtASNAfter = ni.PurgeHtASNAfter;
                    hnp.PurgeHtDropShipAfter = ni.PurgeHtDropShipAfter;
                    hnp.PurgeHtDummyAfter = ni.PurgeHtDummyAfter;
                    hnp.PurgeHtReceiptAfter = ni.PurgeHtReceiptAfter;
                    hnp.PurgeHtPurchaseOrderAfter = ni.PurgeHtPurchaseOrderAfter;
                    hnp.PurgeHtReserveAfter = ni.PurgeHtReserveAfter;
                    hnp.PurgeHtVSWAfter = ni.PurgeHtVSWAfter;
                    hnp.PurgeHtWorkUpTotAfter = ni.PurgeHtWorkUpTotAfter;
                    //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                    hnp.IsVirtual = ni.IsVirtual;
                    hnp.Purpose = ni.Purpose;
                    hnp.DeleteNode = ni.DeleteNode;  // Begin TT#3630 - JSmith - Delete My Hierarchy
                    // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                    hnp.Active = ni.Active;
                    // End TT#988
					HierarchyInfo hi = GetHierarchyInfoByRID(ni.HomeHierarchyRID, true);
					hnp.HomeHierarchyType = hi.HierarchyType;
                    // Begin Track #5005 - JSmith - Explorer Organization
                    hnp.HomeHierarchyOwner = hi.Owner;
                    // End Track #5005
					hnp.RollupOption = hi.HierarchyRollupOption;
					if (ni.HomeHierarchyLevel == 0)
					{
						hnp.LevelType = eHierarchyLevelType.Undefined;
						hnp.DisplayOption = _globalOptions.ProductLevelDisplay;
						hnp.NodeColor = hi.HierarchyColor;
					}
					else
						if (hi.HierarchyType == eHierarchyType.organizational)
					{
						HierarchyLevelInfo hli = (HierarchyLevelInfo)hi.HierarchyLevels[ni.HomeHierarchyLevel]; 
						hnp.LevelType = hli.LevelType;
						hnp.DisplayOption = hli.LevelDisplayOption;
						if (hi.HierarchyType == eHierarchyType.organizational)
						{
							hnp.NodeColor = hli.LevelColor;
						}
						else
						{
							hnp.NodeColor = Include.MIDDefaultColor;
						}
					}
					else
					{
						hnp.NodeColor = Include.MIDDefaultColor;
						hnp.DisplayOption = _globalOptions.ProductLevelDisplay;
					}

					//  set true if next level in hierarchy is the style level
					if (ni.HomeHierarchyLevel + 1 <= hi.HierarchyLevels.Count)
					{
						HierarchyLevelInfo childhli = (HierarchyLevelInfo)hi.HierarchyLevels[ni.HomeHierarchyLevel + 1];
						if (childhli.LevelType == eHierarchyLevelType.Style)
						{
							hnp.IsParentOfStyle = true;
						}
					}

					HierarchyInfo.NodeRelationship nr = hi.GetChildRelationship(nodeRID);

					hnp.HasChildren = nr.ChildrenCount() > 0;

					HierarchyLevelInfo nextLevel = null;
					if (hi.HierarchyType == eHierarchyType.organizational)
					{
						if (hnp.HomeHierarchyLevel < hi.HierarchyLevels.Count)
						{
							nextLevel = (HierarchyLevelInfo)hi.HierarchyLevels[hnp.HomeHierarchyLevel + 1];
						}

						if (nextLevel != null &&
							nextLevel.LevelDisplayOption == eHierarchyDisplayOptions.DoNotDisplay)  // Do not show level on the explorer
						{
							hnp.DisplayChildren = false;
						}
						else
						{
							hnp.DisplayChildren = true;
						}
					}
					else
					{
						hnp.DisplayChildren = true;
					}

					// format the Text based on the level type.
					NodeAncestorList nal = null;
					NodeAncestorProfile nap;
					hnp.LevelText = Include.GetNodeDisplay(hnp.DisplayOption, hnp.NodeID, hnp.NodeName, hnp.NodeDescription);
					hnp.Text = hnp.LevelText;
					//Begin Track #5378 - color and size not qualified
					if (aBuildQualifiedID)
					{
                        if (hnp.LevelType == eHierarchyLevelType.Color ||
                            hnp.LevelType == eHierarchyLevelType.Size)
                        {
                            // concatenate up to style level
                            nal = GetNodeAncestorList(nodeRID, hnp.HomeHierarchyRID);
                            for (int ancestor = 1; ancestor < nal.Count; ancestor++)
                            {
                                // check ancestor
                                nap = (NodeAncestorProfile)nal[ancestor];	// get nodeRID from ancestor list
                                ni = GetNodeInfoByRID(nap.Key, false);
                                //Begin Track #5378 - color and size not qualified

                                //HierarchyNodeProfile textHnp = GetNodeData(ni.NodeRID, false);    //TT#339 - MD - Modify Forecast audit message - RBeck
                                HierarchyNodeProfile textHnp = GetNodeData(ni.NodeRID, false, aBuildQualifiedID);
                                //End Track #5378
                                hnp.Text = textHnp.LevelText + _globalOptions.ProductLevelDelimiter + hnp.Text;

                                if (ni.LevelType == eHierarchyLevelType.Style)
                                {                                                              
                                    break;
                                }
                            }
                        }
                        hnp.QualifiedNodeID = BuildQualifiedNode( hnp ); //TT#339 - MD - Modify Forecast audit message - RBeck
					}
					//End MID Track #xxxx

                    if (chaseHierarchy)
                    {
                        // Begin TT#400-MD - JSmith - Add Header Purge Criteria by Header Type
                        // Restructured criteria chase.  Too many changes to mark.
                        // Use compare tool to see differences.

                        ArrayList alNodeInfo = new ArrayList();
                        HierarchyLevelInfo hli;
                        // if OTSPlanLevelType, ProductType, or purge criteria not set on the node, chase up hierarchy
                        // OTSPlanLevelType is chased up the hierarchy ancestor list then up the
                        // hierarchy level definitions.  Purge Criteria is chased differently.  It is chased
                        // by node then level then parent then parent level and so on.
                        if (!hnp.ProductTypeIsOverridden ||
                            hnp.ProductType == eProductType.Undefined ||
                            !hnp.OTSPlanLevelIsOverridden ||
                            ((hnp.OTSPlanLevelHierarchyRID == Include.NoRID ||
                            hnp.OTSPlanLevelHierarchyLevelSequence == Include.Undefined) &&
                            hnp.OTSPlanLevelMask == null) ||
                            !hnp.OTSPlanLevelTypeIsOverridden ||
                            hnp.OTSPlanLevelType == eOTSPlanLevelType.Undefined ||
                            hnp.PurgeDailyHistoryAfter == Include.Undefined ||
                            hnp.PurgeWeeklyHistoryAfter == Include.Undefined ||
                            hnp.PurgeOTSPlansAfter == Include.Undefined ||
                            hnp.PurgeHtASNAfter == Include.Undefined ||
                            hnp.PurgeHtDropShipAfter == Include.Undefined ||
                            hnp.PurgeHtDummyAfter == Include.Undefined ||
                            hnp.PurgeHtReceiptAfter == Include.Undefined ||
                            hnp.PurgeHtPurchaseOrderAfter == Include.Undefined ||
                            hnp.PurgeHtReserveAfter == Include.Undefined ||
                            hnp.PurgeHtVSWAfter == Include.Undefined ||
                            hnp.PurgeHtWorkUpTotAfter == Include.Undefined ||
                            hnp.ApplyHNRIDFrom == Include.Undefined // TT#1401                            
                            )
                        {
                            // look up hierarchy by checking ancestor list
                            if (nal == null)
                            {
                                nal = GetNodeAncestorList(nodeRID, hnp.HomeHierarchyRID);
                            }
                           
                            for (int ancestor = 0; ancestor < nal.Count; ancestor++)
                            {
                                // check ancestor
                                nap = (NodeAncestorProfile)nal[ancestor];	// get nodeRID from ancestor list
                                ni = GetNodeInfoByRID(nap.Key, false);

                                alNodeInfo.Add(ni);

                                HierarchyInfo homehi = GetHierarchyInfoByRID(ni.HomeHierarchyRID, false);

                                // Get the level information of the node
                                if (homehi.HierarchyLevels != null &&
                                    homehi.HierarchyLevels.Count > 0 &&
                                    ni.HomeHierarchyLevel > 0)
                                {
                                    hli = (HierarchyLevelInfo)homehi.HierarchyLevels[ni.HomeHierarchyLevel];
                                }
                                else
                                {
                                    hli = null;
                                }

                                // check for OTSPlanLevel
                                if (hnp.HomeHierarchyType == eHierarchyType.organizational &&
                                    hnp.OTSPlanLevelSelectType == ePlanLevelSelectType.Undefined &&
                                    ni.OTSPlanLevelSelectType != ePlanLevelSelectType.Undefined)
                                {
                                    hnp.OTSPlanLevelSelectType = ni.OTSPlanLevelSelectType;
                                    hnp.OTSPlanLevelLevelType = ni.OTSPlanLevelLevelType;
                                    hnp.OTSPlanLevelHierarchyRID = ni.OTSPlanLevelHierarchyRID;
                                    hnp.OTSPlanLevelHierarchyLevelSequence = ni.OTSPlanLevelHierarchyLevelSequence;
                                    hnp.OTSPlanLevelAnchorNode = ni.OTSPlanLevelAnchorNode;
                                    hnp.OTSPlanLevelMaskField = ni.OTSPlanLevelMaskField;
                                    hnp.OTSPlanLevelMask = ni.OTSPlanLevelMask;
                                    hnp.OTSPlanLevelInherited = eInheritedFrom.Node;
                                    hnp.OTSPlanLevelInheritedFrom = ni.NodeRID;
                                }

                                // check for OTSPlanLevelType
                                if (hnp.OTSPlanLevelType == eOTSPlanLevelType.Undefined)
                                {
                                    if (ni.OTSPlanLevelTypeIsOverridden &&
                                        ni.OTSPlanLevelType != eOTSPlanLevelType.Undefined)
                                    {
                                        hnp.OTSPlanLevelType = ni.OTSPlanLevelType;
                                        hnp.OTSPlanLevelTypeInherited = eInheritedFrom.Node;
                                        hnp.OTSPlanLevelTypeInheritedFrom = ni.NodeRID;
                                    }
                                }

                                // check for ProductType
                                if (hnp.ProductType == eProductType.Undefined &&
                                    ni.ProductTypeIsOverridden &&
                                    ni.ProductType != eProductType.Undefined)
                                {
                                    hnp.ProductType = ni.ProductType;
                                    hnp.ProductTypeInherited = eInheritedFrom.Node;
                                    hnp.ProductTypeInheritedFrom = ni.NodeRID;
                                }

                                // BEGIN TT#1399
                                if (hnp.ApplyHNRIDFrom == Include.Undefined && ni.ApplyHNRIDFrom != Include.Undefined)
                                {
                                    hnp.ApplyHNRIDFrom = ni.ApplyHNRIDFrom;
                                    hnp.ApplyFromInheritedFrom = ni.NodeRID;
                                    hnp.ApplyFromInherited = eInheritedFrom.Node;
                                }
                                // END TT#1399

                                // check for Daily history purge criteria
                                if (hnp.PurgeDailyHistoryAfter == Include.Undefined)
                                {
                                    if (ni.PurgeDailyHistoryAfter != Include.Undefined)
                                    {
                                        hnp.PurgeDailyHistoryAfter = ni.PurgeDailyHistoryAfter;
                                        hnp.PurgeDailyCriteriaInherited = eInheritedFrom.Node;
                                        hnp.PurgeDailyCriteriaInheritedFrom = ni.NodeRID;
                                    }
                                    else if (hli != null && 
                                             hli.PurgeDailyHistory != Include.Undefined)
                                    {
                                        hnp.PurgeDailyHistoryAfter = hli.PurgeDailyHistory;
                                        hnp.PurgeDailyCriteriaInherited = eInheritedFrom.HierarchyLevel;
                                        hnp.PurgeDailyCriteriaInheritedFrom = ni.HomeHierarchyLevel;
                                    }
                                }

                                // check for Weekly history purge criteria
                                if (hnp.PurgeWeeklyHistoryAfter == Include.Undefined)
                                {
                                    if (ni.PurgeWeeklyHistoryAfter != Include.Undefined)
                                    {
                                        hnp.PurgeWeeklyHistoryAfter = ni.PurgeWeeklyHistoryAfter;
                                        hnp.PurgeWeeklyCriteriaInherited = eInheritedFrom.Node;
                                        hnp.PurgeWeeklyCriteriaInheritedFrom = ni.NodeRID;
                                    }
                                    else if (hli != null && 
                                             hli.PurgeWeeklyHistory != Include.Undefined)
                                    {
                                        hnp.PurgeWeeklyHistoryAfter = hli.PurgeWeeklyHistory;
                                        hnp.PurgeWeeklyCriteriaInherited = eInheritedFrom.HierarchyLevel;
                                        hnp.PurgeWeeklyCriteriaInheritedFrom = ni.HomeHierarchyLevel;
                                    }
                                }

                                // check for OTS plans purge criteria
                                if (hnp.PurgeOTSPlansAfter == Include.Undefined)
                                {
                                    if (ni.PurgeOTSPlansAfter != Include.Undefined)
                                    {
                                        hnp.PurgeOTSPlansAfter = ni.PurgeOTSPlansAfter;
                                        hnp.PurgeOTSCriteriaInherited = eInheritedFrom.Node;
                                        hnp.PurgeOTSCriteriaInheritedFrom = ni.NodeRID;
                                    }
                                    else if (hli != null &&
                                             hli.PurgePlans != Include.Undefined)
                                    {
                                        hnp.PurgeOTSPlansAfter = hli.PurgePlans;
                                        hnp.PurgeOTSCriteriaInherited = eInheritedFrom.HierarchyLevel;
                                        hnp.PurgeOTSCriteriaInheritedFrom = ni.HomeHierarchyLevel;
                                    }
                                }

                                // check for header type purge criteria
                                if (hnp.PurgeHtASNAfter == Include.Undefined)
                                {
                                    if (ni.PurgeHtASNAfter != Include.Undefined)
                                    {
                                        hnp.PurgeHtASNAfter = ni.PurgeHtASNAfter;
                                        hnp.PurgeHtASNCriteriaInherited = eInheritedFrom.Node;
                                        hnp.PurgeHtASNCriteriaInheritedFrom = ni.NodeRID;
                                    }
                                    else if (hli != null &&
                                             hli.PurgeHtASN != Include.Undefined)
                                    {
                                        hnp.PurgeHtASNAfter = hli.PurgeHtASN;
                                        hnp.PurgeHtASNCriteriaInherited = eInheritedFrom.HierarchyLevel;
                                        hnp.PurgeHtASNCriteriaInheritedFrom = ni.HomeHierarchyLevel;
                                    }
                                }

                                if (hnp.PurgeHtDropShipAfter == Include.Undefined)
                                {
                                    if (ni.PurgeHtDropShipAfter != Include.Undefined)
                                    {
                                        hnp.PurgeHtDropShipAfter = ni.PurgeHtDropShipAfter;
                                        hnp.PurgeHtDropShipCriteriaInherited = eInheritedFrom.Node;
                                        hnp.PurgeHtDropShipCriteriaInheritedFrom = ni.NodeRID;
                                    }
                                    else if (hli != null &&
                                             hli.PurgeHtDropShip != Include.Undefined)
                                    {
                                        hnp.PurgeHtDropShipAfter = hli.PurgeHtDropShip;
                                        hnp.PurgeHtDropShipCriteriaInherited = eInheritedFrom.HierarchyLevel;
                                        hnp.PurgeHtDropShipCriteriaInheritedFrom = ni.HomeHierarchyLevel;
                                    }
                                }

                                if (hnp.PurgeHtDummyAfter == Include.Undefined)
                                {
                                    if (ni.PurgeHtDummyAfter != Include.Undefined)
                                    {
                                        hnp.PurgeHtDummyAfter = ni.PurgeHtDummyAfter;
                                        hnp.PurgeHtDummyCriteriaInherited = eInheritedFrom.Node;
                                        hnp.PurgeHtDummyCriteriaInheritedFrom = ni.NodeRID;
                                    }
                                    else if (hli != null &&
                                             hli.PurgeHtDummy != Include.Undefined)
                                    {
                                        hnp.PurgeHtDummyAfter = hli.PurgeHtDummy;
                                        hnp.PurgeHtDummyCriteriaInherited = eInheritedFrom.HierarchyLevel;
                                        hnp.PurgeHtDummyCriteriaInheritedFrom = ni.HomeHierarchyLevel;
                                    }
                                }

                                if (hnp.PurgeHtReceiptAfter == Include.Undefined)
                                {
                                    if (ni.PurgeHtReceiptAfter != Include.Undefined)
                                    {
                                        hnp.PurgeHtReceiptAfter = ni.PurgeHtReceiptAfter;
                                        hnp.PurgeHtReceiptCriteriaInherited = eInheritedFrom.Node;
                                        hnp.PurgeHtReceiptCriteriaInheritedFrom = ni.NodeRID;
                                    }
                                    else if (hli != null &&
                                             hli.PurgeHtReceipt != Include.Undefined)
                                    {
                                        hnp.PurgeHtReceiptAfter = hli.PurgeHtReceipt;
                                        hnp.PurgeHtReceiptCriteriaInherited = eInheritedFrom.HierarchyLevel;
                                        hnp.PurgeHtReceiptCriteriaInheritedFrom = ni.HomeHierarchyLevel;
                                    }
                                }

                                if (hnp.PurgeHtPurchaseOrderAfter == Include.Undefined)
                                {
                                    if (ni.PurgeHtPurchaseOrderAfter != Include.Undefined)
                                    {
                                        hnp.PurgeHtPurchaseOrderAfter = ni.PurgeHtPurchaseOrderAfter;
                                        hnp.PurgeHtPurchaseOrderCriteriaInherited = eInheritedFrom.Node;
                                        hnp.PurgeHtPurchaseOrderCriteriaInheritedFrom = ni.NodeRID;
                                    }
                                    else if (hli != null &&
                                             hli.PurgeHtPurchaseOrder != Include.Undefined)
                                    {
                                        hnp.PurgeHtPurchaseOrderAfter = hli.PurgeHtPurchaseOrder;
                                        hnp.PurgeHtPurchaseOrderCriteriaInherited = eInheritedFrom.HierarchyLevel;
                                        hnp.PurgeHtPurchaseOrderCriteriaInheritedFrom = ni.HomeHierarchyLevel;
                                    }
                                }

                                if (hnp.PurgeHtReserveAfter == Include.Undefined)
                                {
                                    if (ni.PurgeHtReserveAfter != Include.Undefined)
                                    {
                                        hnp.PurgeHtReserveAfter = ni.PurgeHtReserveAfter;
                                        hnp.PurgeHtReserveCriteriaInherited = eInheritedFrom.Node;
                                        hnp.PurgeHtReserveCriteriaInheritedFrom = ni.NodeRID;
                                    }
                                    else if (hli != null &&
                                             hli.PurgeHtReserve != Include.Undefined)
                                    {
                                        hnp.PurgeHtReserveAfter = hli.PurgeHtReserve;
                                        hnp.PurgeHtReserveCriteriaInherited = eInheritedFrom.HierarchyLevel;
                                        hnp.PurgeHtReserveCriteriaInheritedFrom = ni.HomeHierarchyLevel;
                                    }
                                }

                                if (hnp.PurgeHtVSWAfter == Include.Undefined)
                                {
                                    if (ni.PurgeHtVSWAfter != Include.Undefined)
                                    {
                                        hnp.PurgeHtVSWAfter = ni.PurgeHtVSWAfter;
                                        hnp.PurgeHtVSWCriteriaInherited = eInheritedFrom.Node;
                                        hnp.PurgeHtVSWCriteriaInheritedFrom = ni.NodeRID;
                                    }
                                    else if (hli != null &&
                                             hli.PurgeHtVSW != Include.Undefined)
                                    {
                                        hnp.PurgeHtVSWAfter = hli.PurgeHtVSW;
                                        hnp.PurgeHtVSWCriteriaInherited = eInheritedFrom.HierarchyLevel;
                                        hnp.PurgeHtVSWCriteriaInheritedFrom = ni.HomeHierarchyLevel;
                                    }
                                }

                                if (hnp.PurgeHtWorkUpTotAfter == Include.Undefined)
                                {
                                    if (ni.PurgeHtWorkUpTotAfter != Include.Undefined)
                                    {
                                        hnp.PurgeHtWorkUpTotAfter = ni.PurgeHtWorkUpTotAfter;
                                        hnp.PurgeHtWorkUpTotCriteriaInherited = eInheritedFrom.Node;
                                        hnp.PurgeHtWorkUpTotCriteriaInheritedFrom = ni.NodeRID;
                                    }
                                    else if (hli != null &&
                                             hli.PurgeHtWorkUpTot != Include.Undefined)
                                    {
                                        hnp.PurgeHtWorkUpTotAfter = hli.PurgeHtWorkUpTot;
                                        hnp.PurgeHtWorkUpTotCriteriaInherited = eInheritedFrom.HierarchyLevel;
                                        hnp.PurgeHtWorkUpTotCriteriaInheritedFrom = ni.HomeHierarchyLevel;
                                    }
                                }

                                // stop if everything is found
                                if (hnp.OTSPlanLevelType != eOTSPlanLevelType.Undefined &&
                                    ((hnp.OTSPlanLevelHierarchyRID != Include.NoRID &&
                                    hnp.OTSPlanLevelHierarchyLevelSequence != Include.Undefined) ||
                                    (hnp.OTSPlanLevelMask != null)) &&
                                    hnp.ProductType != eProductType.Undefined &&
                                    hnp.PurgeDailyHistoryAfter != Include.Undefined &&
                                    hnp.PurgeWeeklyHistoryAfter != Include.Undefined &&
                                    hnp.PurgeOTSPlansAfter != Include.Undefined &&
                                    hnp.PurgeHtASNAfter != Include.Undefined &&
                                    hnp.PurgeHtDropShipAfter != Include.Undefined &&
                                    hnp.PurgeHtDummyAfter != Include.Undefined &&
                                    hnp.PurgeHtReceiptAfter != Include.Undefined &&
                                    hnp.PurgeHtPurchaseOrderAfter != Include.Undefined &&
                                    hnp.PurgeHtReserveAfter != Include.Undefined &&
                                    hnp.PurgeHtVSWAfter != Include.Undefined &&
                                    hnp.PurgeHtWorkUpTotAfter != Include.Undefined &&
                                    hnp.ApplyHNRIDFrom != Include.Undefined // TT#1399
                                    )
                                {
                                    break;
                                }

                            }

                            // if still still not set, chase up home hierarchy level definitions
                            if (hnp.OTSPlanLevelType == eOTSPlanLevelType.Undefined)
                            {
                                foreach (NodeInfo ancestorNodeInfo in alNodeInfo)
                                {
                                    if (ancestorNodeInfo.HomeHierarchyRID != hi.HierarchyRID)
                                    {
                                        hi = GetHierarchyInfoByRID(ancestorNodeInfo.HomeHierarchyRID, false);
                                    }

                                    if (hi.HierarchyType == eHierarchyType.organizational &&
                                        ancestorNodeInfo.HomeHierarchyLevel > 0)
                                    {
                                        hli = (HierarchyLevelInfo)hi.HierarchyLevels[ancestorNodeInfo.HomeHierarchyLevel];
                                        if (hnp.OTSPlanLevelType == eOTSPlanLevelType.Undefined &&
                                            hli.LevelOTSPlanLevelType != eOTSPlanLevelType.Undefined)
                                        {
                                            hnp.OTSPlanLevelType = hli.LevelOTSPlanLevelType;
                                            hnp.OTSPlanLevelTypeInherited = eInheritedFrom.HierarchyLevel;
                                            hnp.OTSPlanLevelTypeInheritedFrom = ancestorNodeInfo.HomeHierarchyLevel;
                                        }
                                    }

                                    // stop if everything is found
                                    if (hnp.OTSPlanLevelType != eOTSPlanLevelType.Undefined)
                                    {
                                        break;
                                    }
                                }

                                // if still not set, use Hierarchy Default
                                if (hnp.OTSPlanLevelType == eOTSPlanLevelType.Undefined)
                                {
                                    hnp.OTSPlanLevelType = hi.OTSPlanLevelType;
                                    hnp.OTSPlanLevelTypeInherited = eInheritedFrom.HierarchyDefaults;
                                    hnp.OTSPlanLevelTypeInheritedFrom = Include.NoRID;
                                }
                            }
                        }

                        // End TT#400-MD - JSmith - Add Header Purge Criteria by Header Type
                    }
 

					return hnp;
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static public NodeAncestorList DetermineOTSPlanLevelPath(HierarchyNodeProfile hnp, out HierarchyNodeProfile anchorNode)
		{
			try
			{
				int OTSPlanLevelHierarchyRID;
				anchorNode = null;
                // Begin TT#2857 - JSmith - Using Alternate Hierarchy Plans as Default OTS Forecasts for Org Nodes
                eHierarchySearchType hierarchySearchType = eHierarchySearchType.HomeHierarchyOnly;
                bool UseApplyFrom = true;
                // End TT#2857 - JSmith - Using Alternate Hierarchy Plans as Default OTS Forecasts for Org Nodes
				
				if (hnp.OTSPlanLevelAnchorNode != Include.NoRID)
				{
					anchorNode = GetNodeData(hnp.OTSPlanLevelAnchorNode, false);
					OTSPlanLevelHierarchyRID = anchorNode.HomeHierarchyRID;
                    // Begin TT#2857 - JSmith - Using Alternate Hierarchy Plans as Default OTS Forecasts for Org Nodes
                    if (anchorNode.HomeHierarchyType == eHierarchyType.alternate)
                    {
                        hierarchySearchType = eHierarchySearchType.SpecificHierarchy;
                        UseApplyFrom = false;
                    }
                    // End TT#2857 - JSmith - Using Alternate Hierarchy Plans as Default OTS Forecasts for Org Nodes
				}
				else
				{
					OTSPlanLevelHierarchyRID = hnp.HomeHierarchyRID;
				}
                // Begin TT#2857 - JSmith - Using Alternate Hierarchy Plans as Default OTS Forecasts for Org Nodes
                //return GetNodeAncestorList(hnp.Key, OTSPlanLevelHierarchyRID);
                return GetNodeAncestorList(hnp.Key, OTSPlanLevelHierarchyRID, hierarchySearchType, UseApplyFrom);
                // End TT#2857 - JSmith - Using Alternate Hierarchy Plans as Default OTS Forecasts for Org Nodes
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}


		static public int DetermineOTSPlanLevelRid(HierarchyNodeProfile hnp)
		{
			try
			{
				NodeAncestorList nal;
				HierarchyNodeProfile anchorNode = null;

				nal = DetermineOTSPlanLevelPath(hnp, out anchorNode);

				// Begin Track #5332 - JSmith - Plan Level not found from alternate
//				//Begin Track #4376 - JSmith - Incorrect OTS plan level returned when referencing alt hier
//				// check for multiple paths in alternate hierarchies
//				for (int i = 0; i < nal.Count; i++)
//				{
//					NodeAncestorProfile nap1 = (NodeAncestorProfile)nal[i];
//					for (int j = i + 1; j < nal.Count; j++)
//					{
//						NodeAncestorProfile nap2 = (NodeAncestorProfile)nal[j];
//						if (nap1.HomeHierarchyRID == nap2.HomeHierarchyRID &&
//							nap1.HomeHierarchyLevel == nap2.HomeHierarchyLevel)
//						{
//							return Include.NoRID;
//						}
//					}
//				}
//				//End Track #4376
				// End Track #5332

				switch (hnp.OTSPlanLevelSelectType)
				{
					case ePlanLevelSelectType.HierarchyLevel:
						return DetermineOTSPlanLevelRidByLevel(hnp, nal, anchorNode);
					case ePlanLevelSelectType.Node:
						return DetermineOTSPlanLevelRidByNode(hnp, nal);
				}
				return Include.NoRID;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static private int DetermineOTSPlanLevelRidByLevel(HierarchyNodeProfile hnp, NodeAncestorList nal,
			HierarchyNodeProfile anchorNode)
		{
			try
			{
				int OTSLevel = 0;
				HierarchyNodeProfile ancestor;

				// if main hierarchy, must be hierarchy level
				if (anchorNode == null || anchorNode.HomeHierarchyType == eHierarchyType.organizational)
				{
					if (hnp.OTSPlanLevelInherited == eInheritedFrom.None)
					{
						if (hnp.OTSPlanLevelHierarchyLevelSequence != Include.Undefined)
						{
							if (hnp.OTSPlanLevelHierarchyLevelSequence <= hnp.HomeHierarchyLevel)
							{
								OTSLevel = hnp.HomeHierarchyLevel - hnp.OTSPlanLevelHierarchyLevelSequence;
							}
							else 
							{
								return Include.Undefined;
							}
						}
						else
						{
							return hnp.Key;
						}
					}
					else
					{
						OTSLevel = hnp.HomeHierarchyLevel - hnp.OTSPlanLevelHierarchyLevelSequence;
					}
				
					if (OTSLevel < 0)
					{
						return Include.NoRID;
					}
					else
					{
						return ((NodeAncestorProfile)nal[OTSLevel]).Key;
					}
				}
				// if offset in alternate hierarchy, calculate ancestor location
				else if (hnp.OTSPlanLevelLevelType == ePlanLevelLevelType.LevelOffset)
				{
					// Begin Track #5332 - JSmith - Plan Level not found from alternate
//					OTSLevel = nal.Count - hnp.OTSPlanLevelHierarchyLevelSequence + anchorNode.HomeHierarchyLevel - 1;
					foreach (NodeAncestorProfile nap in nal)
					{
						if (nap.Key == anchorNode.Key)
						{
							break;
						}
						++OTSLevel;
					}
					OTSLevel -= hnp.OTSPlanLevelHierarchyLevelSequence;
					// End Track #5332
					if (OTSLevel < 0 ||
						OTSLevel > nal.Count)
					{
						return Include.NoRID;
					}
					else
					{
						return ((NodeAncestorProfile)nal[OTSLevel]).Key;
					}
				}
				else  // if hierarchy level in alternate hierarchy, must search
				{
					foreach (NodeAncestorProfile nap in nal)
					{
						ancestor = GetNodeData(nap.Key, false);
						if (ancestor.HomeHierarchyLevel == hnp.OTSPlanLevelHierarchyLevelSequence)
						{
							return nap.Key;
						}
					}
					return Include.NoRID;
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static private int DetermineOTSPlanLevelRidByNode(HierarchyNodeProfile hnp, NodeAncestorList nal)
		{
			try
			{
				int OTSPlanLevelRID = Include.NoRID;
				HierarchyNodeProfile ancestor;

				foreach (NodeAncestorProfile nap in nal)
				{
					ancestor = GetNodeData(nap.Key, false);
					switch (hnp.OTSPlanLevelMaskField)
					{
						case eMaskField.Id:
							if (ancestor.NodeID.ToLower().StartsWith(hnp.OTSPlanLevelMask.ToLower()))
							{
								OTSPlanLevelRID = nap.Key;
							}
							break;
							case eMaskField.Name:
							if (ancestor.NodeName.ToLower().StartsWith(hnp.OTSPlanLevelMask.ToLower()))
							{
								OTSPlanLevelRID = nap.Key;
							}
							break;
							case eMaskField.Description:
							if (ancestor.NodeDescription.ToLower().StartsWith(hnp.OTSPlanLevelMask.ToLower()))
							{
								OTSPlanLevelRID = nap.Key;
							}
							break;
					}

					if (OTSPlanLevelRID != Include.NoRID)
					{
						break;
					}
				}
				
				return OTSPlanLevelRID;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        /// <summary>
        /// Retrieve assortment node
        /// </summary>
        /// <returns>HierarchyNodeProfile of the assortment node</returns>
        static public HierarchyNodeProfile GetAssortmentNode()
        {
            MerchandiseHierarchyData mhd;
            HierarchyNodeProfile assortmentNode = null;
            try
            {
                lock (_assortment_lock.SyncRoot)
                {
                    HierarchyProfile mainHierarchy = GetMainHierarchyData();
                    mhd = new MerchandiseHierarchyData();

                    try
                    {
                        int styleLevel = 0;
                        HierarchyLevelProfile hlp;
                        if (mainHierarchy.HierarchyLevels != null)
                        {
                            for (int i = 1; i < mainHierarchy.HierarchyLevels.Count; i++)
                            {
                                hlp = (HierarchyLevelProfile)mainHierarchy.HierarchyLevels[i];
                                if (hlp.LevelType == eHierarchyLevelType.Style)
                                {
                                    styleLevel = i;
                                }
                            }
                        }
                        HierarchyNodeProfile parentConnector = BuildConnectorNodes(mainHierarchy.HierarchyRootNodeRID, styleLevel);

                        mhd.OpenUpdateConnection();

                        assortmentNode = parentConnector;
                        assortmentNode.Purpose = ePurpose.Assortment;
                        assortmentNode.NodeChangeType = eChangeType.add;
                        assortmentNode.HomeHierarchyParentRID = assortmentNode.Key;
                        assortmentNode.Key = Include.NoRID;
                        assortmentNode.HomeHierarchyLevel += 1;
                        assortmentNode.LevelText = Include.GetNodeDisplay(_globalOptions.ProductLevelDisplay, assortmentNode.NodeID, assortmentNode.NodeName, assortmentNode.NodeDescription);
                        assortmentNode.Text = assortmentNode.LevelText;

                        // Begin TT#1453 - JSmith - Multiple Loads -> Foreign Key Errors -> Missing Size Node
                        //assortmentNode.Key = mhd.Hierarchy_Node_Add(assortmentNode.HierarchyRID, assortmentNode.HomeHierarchyParentRID,
                        //    assortmentNode.HomeHierarchyRID, assortmentNode.HomeHierarchyLevel,
                        //    assortmentNode.LevelType, assortmentNode.OTSPlanLevelTypeIsOverridden, 
                        //    assortmentNode.OTSPlanLevelType, assortmentNode.UseBasicReplenishment,
                        //    assortmentNode.OTSPlanLevelIsOverridden, assortmentNode.OTSPlanLevelSelectType, 
                        //    assortmentNode.OTSPlanLevelLevelType, assortmentNode.OTSPlanLevelHierarchyRID, 
                        //    assortmentNode.OTSPlanLevelHierarchyLevelSequence, assortmentNode.OTSPlanLevelAnchorNode, 
                        //    assortmentNode.OTSPlanLevelMaskField, assortmentNode.OTSPlanLevelMask, 
                        //    assortmentNode.IsVirtual, assortmentNode.Purpose);

                        //assortmentNode.NodeID = "ASSORTMENT" + assortmentNode.Key.ToString("d9");
                        //assortmentNode.NodeName = assortmentNode.NodeID;
                        //assortmentNode.NodeDescription = assortmentNode.NodeID;
                        //mhd.Hierarchy_BasicNode_Add(assortmentNode.Key, assortmentNode.NodeID, assortmentNode.NodeName,
                        //            assortmentNode.NodeDescription, assortmentNode.ProductTypeIsOverridden, assortmentNode.ProductType);

                        assortmentNode.NodeID = "ASSORTMENT" + assortmentNode.Key.ToString("d9");
                        assortmentNode.NodeName = assortmentNode.NodeID;
                        assortmentNode.NodeDescription = assortmentNode.NodeID;
                        // Begin TT#1785 - RMatelic - ASST receive system argument exception when creating a placeholder >>>> re-added 'assortmentNode.Key = '
                        //mhd.Hierarchy_BasicNode_Add(assortmentNode.HierarchyRID, assortmentNode.HomeHierarchyParentRID,
                        assortmentNode.Key = mhd.Hierarchy_BasicNode_Add(assortmentNode.HierarchyRID, assortmentNode.HomeHierarchyParentRID,  // End TT#1785
                                                    assortmentNode.HomeHierarchyRID, assortmentNode.HomeHierarchyLevel,
                                                    assortmentNode.LevelType, assortmentNode.OTSPlanLevelTypeIsOverridden,
                                                    assortmentNode.OTSPlanLevelType, assortmentNode.UseBasicReplenishment,
                                                    assortmentNode.OTSPlanLevelIsOverridden, assortmentNode.OTSPlanLevelSelectType,
                                                    assortmentNode.OTSPlanLevelLevelType, assortmentNode.OTSPlanLevelHierarchyRID,
                                                    assortmentNode.OTSPlanLevelHierarchyLevelSequence, assortmentNode.OTSPlanLevelAnchorNode,
                                                    assortmentNode.OTSPlanLevelMaskField, assortmentNode.OTSPlanLevelMask,
                                                    assortmentNode.IsVirtual, assortmentNode.Purpose, assortmentNode.NodeID, assortmentNode.NodeName,
                                                    assortmentNode.NodeDescription, assortmentNode.ProductTypeIsOverridden, assortmentNode.ProductType);
                        // Begin TT#1453
                        mhd.CommitData();
                        NodeUpdateProfileInfo(assortmentNode);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        mhd.CloseUpdateConnection();
                    }
                }
                return assortmentNode;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieve placeholder style node
        /// </summary>
        /// <param name="aAnchorNode">
        /// The key of the anchor node for the placeholder style.
        /// </param>
        /// <param name="aNumberOfPlaceholderStyles">
        /// The number of placeholder styles to create
        /// </param>
        /// <param name="aCurrentNumberOfPlaceholderStyles">
        /// The current number of placeholder styles already defined to the assortment
        /// </param>
        /// <param name="aAssortmentID">
        /// The RID of the assortment to use in the placeholder styles ID
        /// </param>
        /// <returns>HierarchyNodeList of HierarchyNodeProfiles with the placeholder style nodes</returns>
        static public HierarchyNodeList GetPlaceholderStyles(int aAnchorNode, int aNumberOfPlaceholderStyles,
            int aCurrentNumberOfPlaceholderStyles, int aAssortmentRID)
        {
            try
            {
                int currentPlaceholderStyles = aNumberOfPlaceholderStyles;
                MerchandiseHierarchyData mhd;
                HierarchyNodeList placeholderStyles = new HierarchyNodeList(eProfileType.HierarchyNode);
                HierarchyNodeProfile placeholderStyle = null;
                string placeholderStyleID = null;
                char nodeDelimiter = GetProductLevelDelimiter();
                // anchor to hierarchy root if no anchor node
                if (aAnchorNode == Include.NoRID)
                {
                    aAnchorNode = GetMainHierarchyData().HierarchyRootNodeRID;
                }

                //List<int> currentPlaceHolders = new List<int>();

                HierarchyProfile mainHierarchy = GetMainHierarchyData();
                int styleLevel = 0;
                HierarchyLevelProfile hlp;
                if (mainHierarchy.HierarchyLevels != null)
                {
                    for (int i = 1; i < mainHierarchy.HierarchyLevels.Count; i++)
                    {
                        hlp = (HierarchyLevelProfile)mainHierarchy.HierarchyLevels[i];
                        if (hlp.LevelType == eHierarchyLevelType.Style)
                        {
                            styleLevel = i;
                        }
                    }
                }

                lock (_assortment_lock.SyncRoot)
                {
                    HierarchyNodeProfile parentConnector = BuildConnectorNodes(aAnchorNode, styleLevel);

                    mhd = new MerchandiseHierarchyData();
                    try
                    {
                        NodeInfo anchorNodeInfo = GetNodeInfoByRID(aAnchorNode);
                        mhd.OpenUpdateConnection();

                        for (int i = 0; i < aNumberOfPlaceholderStyles; i++)
                        {
                            placeholderStyle = (HierarchyNodeProfile)parentConnector.Clone();
                            placeholderStyle.Purpose = ePurpose.Placeholder;
                           
                            // TT#1599 - RMatelic - Removing a Placeholder (style) from the Contents tab, does not remove it from the Merch Explorer.  Deleting an Assortment also does not delete the Placeholders from the Merch Explorer.
                            placeholderStyle.IsVirtual = true;
                            // End TT#1599
                           
                            placeholderStyle.NodeChangeType = eChangeType.add;
                            placeholderStyle.HomeHierarchyParentRID = placeholderStyle.Key;
                            placeholderStyle.Key = Include.NoRID;
                            placeholderStyle.HomeHierarchyLevel += 1;
                            placeholderStyle.NodeID = string.Empty;
                            // Begin TT#2 - RMatelic - Assortment Planning - change Placeholder Style ID 
                            //int nextPHSID = GetNextPlaceholderStyleID(currentPlaceholderStyles, aAssortmentID + nodeDelimiter + _placeholderStyleLabel);
                            //placeholderStyleID = _placeholderStyleLabel + (nextPHSID).ToString("d4");
                            //placeholderStyle.NodeID += aAssortmentID + nodeDelimiter + placeholderStyleID;
                            int nextPHSID = GetNextPlaceholderStyleID(currentPlaceholderStyles,_placeholderStyleLabel_1 + aAssortmentRID.ToString() + _placeholderStyleLabel_2);
                            placeholderStyleID = _placeholderStyleLabel_2 + (nextPHSID).ToString("d4");
                            placeholderStyle.NodeID += _placeholderStyleLabel_1 + aAssortmentRID.ToString() + placeholderStyleID;
                            // End TT#2 

                            placeholderStyle.NodeName = placeholderStyleID;
                            placeholderStyle.NodeDescription = placeholderStyleID;
                            placeholderStyle.LevelText = Include.GetNodeDisplay(_globalOptions.ProductLevelDisplay, placeholderStyle.NodeID, placeholderStyle.NodeName, placeholderStyle.NodeDescription);
                            placeholderStyle.Text = placeholderStyle.LevelText;

                            // Begin TT#1453 - JSmith - Multiple Loads -> Foreign Key Errors -> Missing Size Node
                            //placeholderStyle.Key = mhd.Hierarchy_Node_Add(placeholderStyle.HierarchyRID, placeholderStyle.HomeHierarchyParentRID,
                            //    placeholderStyle.HomeHierarchyRID, placeholderStyle.HomeHierarchyLevel,
                            //    placeholderStyle.LevelType, placeholderStyle.OTSPlanLevelTypeIsOverridden, placeholderStyle.OTSPlanLevelType, placeholderStyle.UseBasicReplenishment,
                            //    placeholderStyle.OTSPlanLevelIsOverridden, placeholderStyle.OTSPlanLevelSelectType,
                            //    placeholderStyle.OTSPlanLevelLevelType, placeholderStyle.OTSPlanLevelHierarchyRID,
                            //    placeholderStyle.OTSPlanLevelHierarchyLevelSequence, placeholderStyle.OTSPlanLevelAnchorNode,
                            //    placeholderStyle.OTSPlanLevelMaskField, placeholderStyle.OTSPlanLevelMask,
                            //    placeholderStyle.IsVirtual, placeholderStyle.Purpose);
                            //mhd.Hierarchy_BasicNode_Add(placeholderStyle.Key, placeholderStyle.NodeID, placeholderStyle.NodeName,
                            //            placeholderStyle.NodeDescription, placeholderStyle.ProductTypeIsOverridden, placeholderStyle.ProductType);
                            // Begin TT#1785 - RMatelic - ASST receive system argument exception when creating a placeholder >>>> re-added 'placeholderStyle.Key = '
                            //mhd.Hierarchy_BasicNode_Add(placeholderStyle.HierarchyRID, placeholderStyle.HomeHierarchyParentRID,
                            placeholderStyle.Key = mhd.Hierarchy_BasicNode_Add(placeholderStyle.HierarchyRID, placeholderStyle.HomeHierarchyParentRID,  // End TT#1785
                                                        placeholderStyle.HomeHierarchyRID, placeholderStyle.HomeHierarchyLevel,
                                                        placeholderStyle.LevelType, placeholderStyle.OTSPlanLevelTypeIsOverridden, placeholderStyle.OTSPlanLevelType, placeholderStyle.UseBasicReplenishment,
                                                        placeholderStyle.OTSPlanLevelIsOverridden, placeholderStyle.OTSPlanLevelSelectType,
                                                        placeholderStyle.OTSPlanLevelLevelType, placeholderStyle.OTSPlanLevelHierarchyRID,
                                                        placeholderStyle.OTSPlanLevelHierarchyLevelSequence, placeholderStyle.OTSPlanLevelAnchorNode,
                                                        placeholderStyle.OTSPlanLevelMaskField, placeholderStyle.OTSPlanLevelMask,
                                                        placeholderStyle.IsVirtual, placeholderStyle.Purpose, placeholderStyle.NodeID, placeholderStyle.NodeName,
                                                        placeholderStyle.NodeDescription, placeholderStyle.ProductTypeIsOverridden, placeholderStyle.ProductType);
                            // End TT#1453
                            NodeUpdateProfileInfo(placeholderStyle);
                            placeholderStyles.Add(placeholderStyle);
                            currentPlaceholderStyles++;
                        }
                        mhd.CommitData();
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        mhd.CloseUpdateConnection();
                    }
                }

                return placeholderStyles;
            }
            catch
            {
                throw;
            }
        }

        static private int GetNextPlaceholderStyleID(int aCurrentNumberOfPlaceholderStyles, string aIDPrefix)
        {
            MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
            int nextID = ++aCurrentNumberOfPlaceholderStyles;
            bool notFoundID = true;
            while (notFoundID)
            {
                string nodeID = aIDPrefix + (nextID).ToString("d4");
                DataTable dt = mhd.Hierarchy_NodeRID_Read(nodeID);
                if (dt.Rows.Count == 0)
                {
                    notFoundID = false;
                }
                else
                {
                    ++nextID;
                }
            }
            return nextID;
        }
        
        /// <summary>
        /// Gets the lowest connector node for the anchor node
        /// </summary>
        /// <param name="aAnchorNode">
        /// The key of the anchor node.
        /// </param>
        ///<returns>
        ///An instance of the HierarchyNodeProfile with the connector node.
        ///</returns>
        static public HierarchyNodeProfile GetLowestConnectorNode(int aAnchorNode)
        {
            try
            {
                HierarchyNodeProfile parentConnector = null;
                // anchor to hierarchy root if no anchor node
                if (aAnchorNode == Include.NoRID)
                {
                    aAnchorNode = GetMainHierarchyData().HierarchyRootNodeRID;
                }

                HierarchyProfile mainHierarchy = GetMainHierarchyData();
                int styleLevel = 0;
                HierarchyLevelProfile hlp;
                if (mainHierarchy.HierarchyLevels != null)
                {
                    for (int i = 1; i < mainHierarchy.HierarchyLevels.Count; i++)
                    {
                        hlp = (HierarchyLevelProfile)mainHierarchy.HierarchyLevels[i];
                        if (hlp.LevelType == eHierarchyLevelType.Style)
                        {
                            styleLevel = i;
                        }
                    }
                }

                lock (_assortment_lock.SyncRoot)
                {
                    parentConnector = BuildConnectorNodes(aAnchorNode, styleLevel);
                }

                return parentConnector;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieve the anchor node for a node
        /// </summary>
        /// <param name="aNodeRID">
        /// The key of the node for which the anchor node is to be retrieved.
        /// </param>
        /// <returns>HierarchyNodeProfile with the anchor node. Key has -1 if no anchor node is found.</returns>
        static public HierarchyNodeProfile GetAnchorNode(int aNodeRID)
        {
            try
            {
                HierarchyNodeProfile anchorNode = new HierarchyNodeProfile(Include.NoRID); 
                HierarchyNodeProfile hnp = GetNodeData(aNodeRID);
                ArrayList ancestors = GetNodeAncestorRIDs(aNodeRID, hnp.HomeHierarchyRID);
                foreach (int ancestorRID in ancestors)
                { 
                    if (ancestorRID != aNodeRID)
                    {
                        hnp = GetNodeData(ancestorRID);
                        if (hnp.Purpose != ePurpose.Connector)
                        {
                            anchorNode = hnp;
                            break;
                        }
                    }
                }

                return anchorNode;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Builds connector nodes from the anchor node to the identified level
        /// </summary>
        /// <param name="aAnchorNodeRID">The key of the node to use as the anchor</param>
        /// <param name="aNodeLevel">The level of the node to build connectors to</param>
        /// <returns>The lowest connector node</returns>
        static private HierarchyNodeProfile BuildConnectorNodes(int aAnchorNodeRID, int aNodeLevel)
        {
            MerchandiseHierarchyData mhd;
            try
            {
                int connectorNodeCount = 0;
                //Hashtable emptyHash = new Hashtable();
                Dictionary<int, HierarchyNodeProfile> emptyNodeDictionary = new Dictionary<int, HierarchyNodeProfile>();
                Dictionary<int, HierarchyNodeSecurityProfile> emptySecurityDictionary = new Dictionary<int, HierarchyNodeSecurityProfile>();
                HierarchyNodeProfile connectorNode = GetNodeData(aAnchorNodeRID, false);
                string anchorNodeID = connectorNode.NodeID;
                char nodeDelimiter = GetProductLevelDelimiter();
                if (connectorNode.HomeHierarchyLevel + 1 < aNodeLevel)
                {
                    mhd = new MerchandiseHierarchyData();
                    try
                    {
                        mhd.OpenUpdateConnection();
                        while (connectorNode.HomeHierarchyLevel + 1 < aNodeLevel)
                        {
                            HierarchyNodeList children = GetHierarchyChildren(connectorNode.HomeHierarchyLevel,
                                connectorNode.HomeHierarchyRID, connectorNode.HomeHierarchyRID,
                                connectorNode.Key, true, emptyNodeDictionary, emptySecurityDictionary, false, eNodeSelectType.Connectors, false);
                            if (children.Count == 0)
                            {
                                connectorNode.Purpose = ePurpose.Connector;
                                connectorNode.NodeChangeType = eChangeType.add;
                                connectorNode.LevelType = eHierarchyLevelType.Undefined;
                                connectorNode.OTSPlanLevelTypeIsOverridden = false;
                                connectorNode.OTSPlanLevelType = eOTSPlanLevelType.Undefined;
                                connectorNode.UseBasicReplenishment = true;
                                connectorNode.OTSPlanLevelIsOverridden = false;
                                connectorNode.OTSPlanLevelHierarchyRID = Include.NoRID;
                                connectorNode.OTSPlanLevelHierarchyLevelSequence = 0;
                                connectorNode.IsVirtual = true;
                                connectorNode.HomeHierarchyParentRID = connectorNode.Key;
                                connectorNode.HomeHierarchyLevel += 1;
                                connectorNode.Key = Include.NoRID;
                                ++connectorNodeCount;
                                string nodeID = "CN" + connectorNodeCount.ToString("d2");
                                connectorNode.NodeID = anchorNodeID + nodeDelimiter + nodeID;
                                connectorNode.NodeName = nodeID;
                                connectorNode.NodeDescription = nodeID;

                                // Begin TT#1453 - JSmith - Multiple Loads -> Foreign Key Errors -> Missing Size Node
                                //connectorNode.Key = mhd.Hierarchy_Node_Add(connectorNode.HierarchyRID, connectorNode.HomeHierarchyParentRID,
                                //    connectorNode.HomeHierarchyRID, connectorNode.HomeHierarchyLevel,
                                //    connectorNode.LevelType, connectorNode.OTSPlanLevelTypeIsOverridden, connectorNode.OTSPlanLevelType, connectorNode.UseBasicReplenishment,
                                //    connectorNode.OTSPlanLevelIsOverridden, connectorNode.OTSPlanLevelSelectType,
                                //    connectorNode.OTSPlanLevelLevelType, connectorNode.OTSPlanLevelHierarchyRID,
                                //    connectorNode.OTSPlanLevelHierarchyLevelSequence, connectorNode.OTSPlanLevelAnchorNode,
                                //    connectorNode.OTSPlanLevelMaskField, connectorNode.OTSPlanLevelMask,
                                //    connectorNode.IsVirtual, connectorNode.Purpose);
                                //mhd.Hierarchy_BasicNode_Add(connectorNode.Key, connectorNode.NodeID, connectorNode.NodeName,
                                //            connectorNode.NodeDescription, connectorNode.ProductTypeIsOverridden, connectorNode.ProductType);
                                // Begin TT#1785 - RMatelic - ASST receive system argument exception when creating a placeholder >>>> re-added 'connectorNode.Key = '
                                //mhd.Hierarchy_BasicNode_Add(connectorNode.HierarchyRID, connectorNode.HomeHierarchyParentRID,
                                connectorNode.Key = mhd.Hierarchy_BasicNode_Add(connectorNode.HierarchyRID, connectorNode.HomeHierarchyParentRID,  // End TT#1785
                                                            connectorNode.HomeHierarchyRID, connectorNode.HomeHierarchyLevel,
                                                            connectorNode.LevelType, connectorNode.OTSPlanLevelTypeIsOverridden, connectorNode.OTSPlanLevelType, connectorNode.UseBasicReplenishment,
                                                            connectorNode.OTSPlanLevelIsOverridden, connectorNode.OTSPlanLevelSelectType,
                                                            connectorNode.OTSPlanLevelLevelType, connectorNode.OTSPlanLevelHierarchyRID,
                                                            connectorNode.OTSPlanLevelHierarchyLevelSequence, connectorNode.OTSPlanLevelAnchorNode,
                                                            connectorNode.OTSPlanLevelMaskField, connectorNode.OTSPlanLevelMask,
                                                            connectorNode.IsVirtual, connectorNode.Purpose, connectorNode.NodeID, connectorNode.NodeName,
                                                            connectorNode.NodeDescription, connectorNode.ProductTypeIsOverridden, connectorNode.ProductType);
                                // End TT#1453
                                NodeUpdateProfileInfo(connectorNode);
                            }
                            else
                            {
                                connectorNode = (HierarchyNodeProfile)children[0];
                            }
                        }
                        mhd.CommitData();
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        mhd.CloseUpdateConnection();
                    }
                }
                return connectorNode;
            }
            catch
            {
                throw;
            }
        }

		static private bool CheckLoadChildren(int nodeRID, bool aLoadSiblings)
		{
			bool nodeLoaded = false;
			try
			{
				int parentRID = Include.NoRID;
				HierarchyLevelInfo hli;
				HierarchyInfo hi = GetHierarchyInfoByRID(_mainHierarchyRID, true);
				HierarchyInfo.NodeRelationship child_nr = hi.GetChildRelationship(nodeRID);
				if (child_nr.ParentsCount() > 0)
				{
					parentRID = Convert.ToInt32(child_nr.parents[0], CultureInfo.CurrentCulture);
					
						NodeInfo ni = null;
								ni = GetNodeCacheByRID(parentRID);
						if (ni == null)
						{
							ni = LoadNode(parentRID);
						}
						HierarchyInfo.NodeRelationship nr = hi.GetChildRelationship(parentRID);

						if (hi.HierarchyType == eHierarchyType.organizational)
						{
					
							hli = (HierarchyLevelInfo)hi.HierarchyLevels[ni.HomeHierarchyLevel + 1];
							if (hli.LevelType == eHierarchyLevelType.Style)
							{
								if (nr.childrenBuilt == false)
								{
									if (aLoadSiblings)
									{
										BuildStyles(ni.HomeHierarchyRID, parentRID, aLoadSiblings);
										nr.childrenBuilt = true;
										hi.ParentChildRelationship[parentRID] = nr;
									}
									else
									{
										LoadNode(nodeRID);
									}
								}
								else
								{
									LoadNode(nodeRID);
								}
								nodeLoaded = true;
							}
							else
								if (hli.LevelType == eHierarchyLevelType.Color)
							{
								if (nr.childrenBuilt == false)
								{
									BuildColorsForStyle(ni.HomeHierarchyRID, parentRID);
									nr.childrenBuilt = true;
									hi.ParentChildRelationship[parentRID] = nr;
								}
								else
								{
									LoadColorNode(nodeRID);
								}
								nodeLoaded = true;
							}
							else
								if (hli.LevelType == eHierarchyLevelType.Size)
							{
								if (nr.childrenBuilt == false)
								{
									BuildSizesForColor(ni.HomeHierarchyRID, parentRID);
									nr.childrenBuilt = true;
									hi.ParentChildRelationship[parentRID] = nr;
								}
								else
								{
									LoadSizeNode(nodeRID);
								}
								nodeLoaded = true;
							}
						}
//					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}

			return nodeLoaded;
		}

		/// <summary>
		/// Retrieves the ancestor node information
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="levelOffset">The number of levels offset of the node's level to retrieve</param>
		/// <returns>
		/// An instance of the HierarchyNodeProfile object containing information about the ancestor node.
		/// Key contains Include.Undefined if ancestor is not found
		/// </returns>
		static public HierarchyNodeProfile GetAncestorData(int hierarchyRID, int nodeRID, int levelOffset)
		{
            try
            {
                if (hierarchyRID == Include.NoRID ||
                    nodeRID == Include.NoRID)
                {
                    return new HierarchyNodeProfile(Include.NoRID);
                }
                int ancestorOffset = 0;
                HierarchyNodeProfile hnp;
                NodeAncestorProfile nap;

                NodeAncestorList nal = GetNodeAncestorList(nodeRID, hierarchyRID);
                ancestorOffset = Math.Abs(levelOffset);
                if (ancestorOffset > nal.Count)
                {
                    hnp = new HierarchyNodeProfile(Include.Undefined);
                }
                else
                {
                    nap = (NodeAncestorProfile)nal[ancestorOffset];
                    hnp = GetNodeData(hierarchyRID, nap.Key, true);
                }
                return hnp;
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
		}

		/// <summary>
		/// Retrieves the ancestor node information
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="idPrefix">
		/// The prefix of the Node ID that will be selected. The first node encountered in the ancestor 
		/// list whose ID matches this string will be selected.
		/// </param>
		/// <returns>
		/// An instance of the HierarchyNodeProfile object containing information about the ancestor node.
		/// Key contains Include.Undefined if ancestor is not found
		/// </returns>
		static public HierarchyNodeProfile GetAncestorData(int hierarchyRID, int nodeRID, string idPrefix)
		{
			HierarchyNodeProfile hnp;
			HierarchyNodeProfile matchHnp;

			try
			{
				if (hierarchyRID == Include.NoRID ||
					nodeRID == Include.NoRID)
				{
					return new HierarchyNodeProfile(Include.NoRID);
				}

				NodeAncestorList nal =  GetNodeAncestorList(nodeRID, hierarchyRID);
				matchHnp = null;

				foreach (NodeAncestorProfile nap in nal)
				{
					hnp = GetNodeData(hierarchyRID, nap.Key, true);
					if (hnp.NodeID.ToLower().IndexOf(idPrefix.ToLower()) == 0)
					{
						matchHnp = hnp;
						break;
					}
				}

				if (matchHnp == null)
				{
					matchHnp = new HierarchyNodeProfile(Include.Undefined);
				}
				
				return matchHnp;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}


		//Begin Track #5378 - color and size not qualified
		/// <summary>
		/// Retrieves the descendant node information
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aGetByType">Identifies the type check to use to determine the descendant</param>
		/// <param name="level">The level or number of levels offset of the node's level to retrieve</param>
		/// <param name="aLevelType">The level master type of the node to retrieve</param>
		/// <param name="aChaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
		/// <param name="aNodeSelectType">
		/// The indicator of type eNodeSelectType that identifies which type of nodes to select
		/// </param>
		/// <returns>
		/// An instance of the HierarchyNodeList object containing HierarchyNodeProfile objects for each descendant.
		/// </returns>
		static public HierarchyNodeList GetDescendantData(int hierarchyRID, int aNodeRID, 
			eHierarchyDescendantType aGetByType, int level, eHierarchyLevelMasterType aLevelType,
			bool aChaseHierarchy, eNodeSelectType aNodeSelectType)
		{
			return GetDescendantData(hierarchyRID, aNodeRID, aGetByType, level, aLevelType,
				aChaseHierarchy, aNodeSelectType, false);
		}
		//End Track #5378

		/// <summary>
		/// Retrieves the descendant node information
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aGetByType">Identifies the type check to use to determine the descendant</param>
		/// <param name="level">The level or number of levels offset of the node's level to retrieve</param>
		/// <param name="aLevelType">The level master type of the node to retrieve</param>
		/// <param name="aChaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
		/// <param name="aNodeSelectType">
		/// The indicator of type eNodeSelectType that identifies which type of nodes to select
		/// </param>
		/// <param name="aBuildQualifiedID">
		/// This switch identifies that the node is being looked up should
		/// have the ID include parents when necessary to identify
		/// </param>
		/// <returns>
		/// An instance of the HierarchyNodeList object containing HierarchyNodeProfile objects for each descendant.
		/// </returns>
		static public HierarchyNodeList GetDescendantData(int hierarchyRID, int aNodeRID, 
			eHierarchyDescendantType aGetByType, int level, eHierarchyLevelMasterType aLevelType,
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
//			bool aChaseHierarchy)
//Begin Track #5378 - color and size not qualified
//			bool aChaseHierarchy, eNodeSelectType aNodeSelectType)
			bool aChaseHierarchy, eNodeSelectType aNodeSelectType, bool aBuildQualifiedID)
//End Track #5378
//End Track #4037
		{
			try
			{
				HierarchyNodeProfile hnp;
				int nodeRID;
				HierarchyNodeList hnl = new HierarchyNodeList(eProfileType.HierarchyNode);
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = null;
				switch (aGetByType)
				{
					case eHierarchyDescendantType.offset:
						dt = mhd.GetDescendantsByOffset(aNodeRID, level);
						break;
					case eHierarchyDescendantType.masterType:
						dt = mhd.GetDescendantsByType(aNodeRID, aLevelType);
						break;
					default:
						dt = mhd.GetDescendantsByLevel(aNodeRID, level);
						break;
				}
				foreach(DataRow dr in dt.Rows)
				{
					nodeRID = Convert.ToInt32(dr["HN_RID"]);
					if (hnl.Contains(nodeRID))
					{
						continue;
					}
//Begin Track #5378 - color and size not qualified
//					hnp = GetNodeData(nodeRID, aChaseHierarchy);
					hnp = GetNodeData(nodeRID, aChaseHierarchy, aBuildQualifiedID);
//End Track #5378
					bool selectNode = false;
					switch (aNodeSelectType)
					{
						case eNodeSelectType.All:
							selectNode = true;
							break;
						case eNodeSelectType.NoVirtual:
							if (!hnp.IsVirtual)
							{
								selectNode = true;
							}
							break;
						case eNodeSelectType.VirtualOnly:
							if (hnp.IsVirtual)
							{
								selectNode = true;
							}
							break;
					}
                    // Begin TT#3630 - JSmith - Delete My Hierarchy
                    if (hnp.DeleteNode)
                    {
                        selectNode = false;
                    }
                    // End TT#3630 - JSmith - Delete My Hierarchy
					if (selectNode)
					{
						hnl.Add(hnp);
					}
				}

				return hnl;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Recursively retrieves all descendants at a given level
		/// </summary>
		/// <param name="hnl">A reference to an instance of the HierarchyNodeList class that will contain the HierarchyNodeProfile objects for the descendants</param>
		/// <param name="hi">An instance of the HierarchyInfo class containing information for the hierarchy</param>
		/// <param name="nodeRID">The record ID of the node</param>
		/// <param name="aGetByType">Identifies the type check to use to determine the descendant</param>
		/// <param name="descendantLevel">The level at which nodes are to be found</param>
		/// <param name="aLevelType">The type of the node to retrieve</param>
		/// <param name="aMainHierarchyRID">The record ID of the main hierarchy</param>
		/// <param name="currentLevel">The level of the nodes to select</param>
		/// <param name="aNodeSelectType">The type of nodes to select</param>
		static private void GetDescendants(ref HierarchyNodeList hnl, HierarchyInfo hi, int nodeRID, eHierarchyDescendantType aGetByType, int currentLevel, int descendantLevel, eHierarchyLevelMasterType aLevelType, int aMainHierarchyRID,
			eNodeSelectType aNodeSelectType)
		{
			try
			{
				HierarchyNodeProfile hnp = GetNodeData(hi.HierarchyRID, nodeRID, true);
				bool selectNode = false;
				switch (aNodeSelectType)
				{
					case eNodeSelectType.All:
						selectNode = true;
						break;
					case eNodeSelectType.NoVirtual:
						if (!hnp.IsVirtual)
						{
							selectNode = true;
						}
						break;
					case eNodeSelectType.VirtualOnly:
						if (hnp.IsVirtual)
						{
							selectNode = true;
						}
						break;
				}
                // Begin TT#3630 - JSmith - Delete My Hierarchy
                if (hnp.DeleteNode)
                {
                    selectNode = false;
                }
                // End TT#3630 - JSmith - Delete My Hierarchy
				// retrieve nodes by offset
				if (aGetByType == eHierarchyDescendantType.offset
					&& selectNode
					&& currentLevel == descendantLevel)	// if correct level, add to list
				{
					if (hnl.Contains(hnp.Key))
					{
						hnp = (HierarchyNodeProfile)hnl.FindKey(hnp.Key);
						++hnp.WeightingFactor;
					}
					else
					{
						hnl.Add(hnp);
					}
				}
					// retrieve node by level of the main hierarchy
				else if (aGetByType == eHierarchyDescendantType.levelType &&
					hnp.HomeHierarchyRID == aMainHierarchyRID &&
					selectNode &&
					hnp.NodeLevel == descendantLevel)
				{
					if (hnl.Contains(hnp.Key))
					{
						hnp = (HierarchyNodeProfile)hnl.FindKey(hnp.Key);
						++hnp.WeightingFactor;
					}
					else
					{
						hnl.Add(hnp);
					}
				}
				else if (aGetByType == eHierarchyDescendantType.masterType &&
					aLevelType == eHierarchyLevelMasterType.ParentOfStyle &&
					selectNode &&
					(hnp.IsParentOfStyle ||
					hnp.LevelType == eHierarchyLevelType.Style ||
					hnp.LevelType == eHierarchyLevelType.Color ||
					hnp.LevelType == eHierarchyLevelType.Size))
				{
					if (hnl.Contains(hnp.Key))
					{
						hnp = (HierarchyNodeProfile)hnl.FindKey(hnp.Key);
						++hnp.WeightingFactor;
					}
					else
					{
						hnl.Add(hnp);
					}
				}
				else
					if (aGetByType == eHierarchyDescendantType.masterType && 
					selectNode &&
					(eHierarchyLevelMasterType)hnp.LevelType == aLevelType)	// if correct level, add to list
				{
					if (hnl.Contains(hnp.Key))
					{
						hnp = (HierarchyNodeProfile)hnl.FindKey(hnp.Key);
						++hnp.WeightingFactor;
					}
					else
					{
						hnl.Add(hnp);
					}
				}
				else									// else go down another level
				{
					HierarchyInfo.NodeRelationship nr = hi.GetChildRelationship(nodeRID);
					if (nr.ChildrenCount() > 0)
					{
						if (hi.HierarchyType == eHierarchyType.organizational)
						{
							HierarchyLevelInfo hli = (HierarchyLevelInfo)hi.HierarchyLevels[hnp.NodeLevel + 1];
							if (hli.LevelType == eHierarchyLevelType.Color && nr.childrenBuilt == false)
							{
								BuildColorsForStyle(hi.HierarchyRID, nodeRID);
								nr.childrenBuilt = true;
								hi.ParentChildRelationship[nodeRID] = nr;
							}
							else
								if (hli.LevelType == eHierarchyLevelType.Size && nr.childrenBuilt == false)
							{
								BuildSizesForColor(hi.HierarchyRID, nodeRID);
								nr.childrenBuilt = true;
								hi.ParentChildRelationship[nodeRID] = nr;
							}
						}
						if (nr.ChildrenCount() > 0)
						{
							foreach (int childRID in nr.children)  //  Get the children of the children
							{
								// get the hierarchy of the child
								HierarchyNodeProfile child_hnp = GetNodeData(childRID);
								hi = GetHierarchyInfoByRID(child_hnp.HomeHierarchyRID, true);
								GetDescendants(ref hnl, hi, childRID, aGetByType, currentLevel + 1, descendantLevel, aLevelType, aMainHierarchyRID, aNodeSelectType);
							}
						}
					}
				}
			}        
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Retrieves a list of keys to read to get intransit for the node
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aLevelType">The master level type of the node to retrieve</param>
		/// <returns>
		/// An instance of the ArrayList object containing IntransitReadNodeProfile objects for each key.
		/// </returns>
		/// <remarks>The node's home hierarchy will be used</remarks>
		static public ArrayList GetIntransitReadNodes(int aNodeRID, eHierarchyLevelMasterType aLevelType)
		{
			try
			{
				int nodeRID, colorCodeRID, sizeCodeRID;
				eHierarchyLevelMasterType levelType;
				bool styleDefinedInHierarchy = false;
				ArrayList intransitReadNodes = new ArrayList();
				IntransitReadNodeProfile irnp;
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = null;
				dt = mhd.GetReadIntransitNodes(aNodeRID, aLevelType);
				foreach(DataRow dr in dt.Rows)
				{
					nodeRID = Convert.ToInt32(dr["HN_RID"]);
					colorCodeRID = Convert.ToInt32(dr["COLOR_CODE_RID"]);
					sizeCodeRID = Convert.ToInt32(dr["SIZE_CODE_RID"]);
					levelType = (eHierarchyLevelMasterType)Convert.ToInt32(dr["LEVEL_TYPE"]);
					styleDefinedInHierarchy = Include.ConvertCharToBool(Convert.ToChar(dr["STYLE_IN_PATH"]));
					switch (levelType)
					{
						case eHierarchyLevelMasterType.Color:
							irnp = new IntransitReadNodeProfile(nodeRID, colorCodeRID);
							irnp.StyleDefinedInHierarchy = styleDefinedInHierarchy;
							break;
						case eHierarchyLevelMasterType.Size:
							irnp = new IntransitReadNodeProfile(nodeRID, colorCodeRID, sizeCodeRID);
							irnp.StyleDefinedInHierarchy = styleDefinedInHierarchy;
							break;
						default:
							irnp = new IntransitReadNodeProfile(nodeRID, levelType);
							break;
					}
					intransitReadNodes.Add(irnp);
				}
				return intransitReadNodes;
			}
			catch
			{
				throw;
			}
		}

        static public IMOProfileList GetMethodOverrideIMOList(ProfileList storeList, bool forCopy)
        {
            try
            {
                IMOProfileList imopl = new IMOProfileList(eProfileType.IMO);
                imopl = GetMethodOverrideIMO(storeList, imopl, forCopy);
                return imopl;
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }

        // this blends the database with the changes the user makes
        static public IMOProfileList GetMethodOverrideIMO(ProfileList storeList, IMOProfileList imopl, bool forCopy)
        {
            try
            {
                IMOProfile imop;
                MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                DataTable dt = null;
                int storeRID;

                dt = mhd.Method_Override_IMO_ReadAll();
                foreach (DataRow dr in dt.Rows)
                {
                    if (!dr.IsNull("ST_RID"))
                    {
                        storeRID = Convert.ToInt32(dr["ST_RID"]);
                        if (storeList.Contains(storeRID))
                        {
                            if (!imopl.Contains(storeRID) && !imopl.Contains(storeRID))
                            // Do you already have this store and hierarchy node?
                            {
                                imop = new IMOProfile(storeRID);
                                imop.IMOStoreRID = dr.IsNull("ST_RID") ? Include.NoRID : Convert.ToInt32(dr["ST_RID"]);
                                imop.IMOMinShipQty = dr.IsNull("IMO_MIN_SHIP_QTY") ? 0 : Convert.ToInt32(dr["IMO_MIN_SHIP_QTY"]);
                                imop.IMOPackQty = dr.IsNull("IMO_PCT_PK_THRSHLD") ? Include.PercentPackThresholdDefault : (Convert.ToInt32(dr["IMO_PCT_PK_THRSHLD"]));
                                imop.IMOMaxValue = dr.IsNull("IMO_MAX_VALUE") ? int.MaxValue : Convert.ToInt32(dr["IMO_MAX_VALUE"]);
                                imop.IMOPshToBackStock = 0;
                                imopl.Add(imop);
                            }
                        }
                    }
                }
                return imopl;
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }

        /// <summary>
        /// retrieves a list of imo data for a node
        /// </summary>
        /// <param name="storeList"></param>
        /// <param name="aNodeRID"></param>
        /// <param name="chaseHierarchy"></param>
        /// <param name="forCopy"></param>
        /// <returns></returns>
        static public IMOProfileList GetNodeIMOList(ProfileList storeList, int aNodeRID, bool chaseHierarchy, bool forCopy)
        {
            try
            {
                bool setInheritance = false;
                int storeCount = 0;
                NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(aNodeRID, false);
                NodeAncestorList nal = GetNodeAncestorList(aNodeRID, ni.HomeHierarchyRID);
                IMOProfileList imopl = new IMOProfileList(eProfileType.IMO);
                foreach (NodeAncestorProfile nap in nal)
                {
                    imopl = GetNodeIMO(storeList, nap.Key, imopl, forCopy, setInheritance);
                    if (storeCount == storeList.Count ||	//  stop lookup if have all settings for each store
                        !chaseHierarchy)					//  or stop on first node if copy
                    {
                        break;
                    }
                    setInheritance = true;
                }
                //  append the missing stores to the ProfileList
                foreach (Profile store in storeList)
                {
                    IMOProfile imop;
                    if (!imopl.Contains(store.Key))
                    {
                        imop = new IMOProfile(store.Key);
                        imop.IMOMaxValue = int.MaxValue;
                        imop.IMONodeRID = Include.NoRID;
                        imop.IMOPackQty = Include.PercentPackThresholdDefault;
                        imop.IMOMinShipQty = 0;
                        imop.IMOFWOS_Max = int.MaxValue; // TT#2225 - gtaylor - ANF VSW
                        imop.IMOPshToBackStock = Include.NoRID;
                        imop.IMOStoreRID = store.Key;
                        if (forCopy)
                        {
                            imop.IMOChangeType = eChangeType.none;
                        }

                        if (setInheritance)
                        {
                            imop.IMOIsInherited = false;
                            imop.IMOInheritedFromNodeRID = Include.NoRID;
                        }
                        imopl.Add(imop);
                    }
                }
                return imopl;
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }

        /// <summary>
        /// retrieves a node's IMO data
        /// </summary>
        /// <param name="aNodeRID">The record id of the node</param>
        /// <returns>
        /// a datatable containing the rows of IMO data
        /// </returns>
        /// <remarks></remarks>
        static public IMOProfileList GetNodeIMO(ProfileList storeList, int NodeRID, IMOProfileList imopl, bool forCopy, bool setInheritance)
        {
            NodeIMOInfo nimoi = null;
            bool setSomething = false; //TT#108 - MD - DOConnell - FWOS Max Model
            FWOSMaxModelInfo FWOSmaxmi = new FWOSMaxModelInfo(); //TT#108 - MD - DOConnell - FWOS Max Model
            try
            {
                imo_rwl.AcquireReaderLock(ReaderLockTimeOut);
                try
                {
                    if (_imoByRID.ContainsKey(NodeRID))
                    {
                        nimoi = (NodeIMOInfo)_imoByRID[NodeRID];
                    }
                }
                finally
                {
                    // Ensure that the lock is released.
                    imo_rwl.ReleaseReaderLock();
                }
            }
            catch (ApplicationException ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetNodeIMO reader lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:GetNodeIMO reader lock has timed out");
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
            try
            {
				// BEGIN TT#1401 - stodd - add resevation stores (IMO)
				// Begin TT#4988 - JSmith - Performance
                //if (nimoi == null || nimoi.IMO.Count == 0)
                if (nimoi == null)
				// End TT#4988 - JSmith - Performance
				// END TT#1401 - stodd - add resevation stores (IMO)
                {
                    nimoi = new NodeIMOInfo();
                    LoadNodeIMO(NodeRID, nimoi);
                    UpdateIMOHash(NodeRID, nimoi);
                }

                IMOInfo imoi;
                foreach (DictionaryEntry val in nimoi.IMO)
                {
                    imoi = (IMOInfo)val.Value;
                    IMOProfile imop;
                    if (storeList.Contains(imoi.IMOStoreRID))
                    {
                        if (!imopl.Contains(imoi.IMOStoreRID) && !imopl.Contains(imoi.IMONodeRID))  
                            // Do you already have this store and hierarchy node?
                        {
                            imop = new IMOProfile(imoi.IMOStoreRID);
                            imop.IMOMaxValue = imoi.IMOMaxValue;
                            imop.IMONodeRID = NodeRID;
                            imop.IMOPackQty = imoi.IMOPackQty;
                            imop.IMOMinShipQty = imoi.IMOMinShipQty;
							//BEGIN TT#108 - MD - DOConnell - FWOS Max Model
                            if (imop.IMOFWOS_MaxType == eModifierType.None &&
                                imoi.IMOFWOSMaxType != eModifierType.None)
                            {
                                setSomething = true;
                                imop.IMOFWOS_MaxType = imoi.IMOFWOSMaxType;
                                imop.IMOFWOS_MaxModelRID = imoi.IMOFWOSMaxRID;
                                if (imop.IMOFWOS_MaxType == eModifierType.Model)
                                {
                                    FWOSmaxmi = (FWOSMaxModelInfo)_FWOSMaxModelsByRID[imoi.IMOFWOSMaxRID];
                                    imop.IMOFWOS_MaxModelName = FWOSmaxmi.ModelID;
                                }
                                imop.IMOFWOS_Max = imoi.IMOFWOSMax;

                            }

							//END TT#108 - MD - DOConnell - FWOS Max Model
                            imop.IMOPshToBackStock = imoi.IMOPshToBackStock;
                            imop.IMOStoreRID = imoi.IMOStoreRID;
                            if (forCopy)
                            {
                                imop.IMOChangeType = eChangeType.add;
                            }

                            //if ((setInheritance) && (!imop.IsDefaultValues))
                            if (setInheritance)
                            {
                                imop.IMOIsInherited = true;
                                imop.IMOInheritedFromNodeRID = NodeRID;
                            }
                            imopl.Add(imop);
                        }
                    }
                }
                return imopl;
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aNodeRID"></param>
        /// <param name="aNodeIMOInfo"></param>
        static private void UpdateIMOHash(int aNodeRID, NodeIMOInfo aNodeIMOInfo)
        {
            try
            {
                imo_rwl.AcquireWriterLock(WriterLockTimeOut);
                try
                {
                    if (!_imoByRID.ContainsKey(aNodeRID))
                    {
                        _imoByRID.Add(aNodeRID, aNodeIMOInfo);
                    }
                }
                finally
                {
                    // Ensure that the lock is released.
                    imo_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:UpdateIMOHash writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:UpdateIMOHash writer lock has timed out");
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }

        /// <summary>
        /// load node imo information into hash
        /// </summary>
        /// <param name="aNodeRID">the node rid to load</param>
        /// <param name="nimoi">the node imo info class to load it into</param>
        static private void LoadNodeIMO(int aNodeRID, NodeIMOInfo nimoi)
        {
            try
            {
                IMOInfo imoi;
                MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                DataTable dt = null;
                //dt = mhd.Node_IMO_Read();
                dt = mhd.Node_IMO_Read(aNodeRID);
                foreach (DataRow dr in dt.Rows)
                {
                    if (!dr.IsNull("ST_RID"))
                    {
                        imoi = new IMOInfo();
                        imoi.IMOStoreRID = Convert.ToInt32(dr["ST_RID"]);
                        imoi.IMONodeRID = dr.IsNull("HN_RID") ? Include.NoRID : Convert.ToInt32(dr["HN_RID"]);
                        imoi.IMOMinShipQty = dr.IsNull("IMO_MIN_SHIP_QTY") ? 0 : Convert.ToInt32(dr["IMO_MIN_SHIP_QTY"]);
                        imoi.IMOMaxValue = dr.IsNull("IMO_MAX_VALUE") ? int.MaxValue : Convert.ToInt32(dr["IMO_MAX_VALUE"]);                        
                        imoi.IMOPackQty = dr.IsNull("IMO_PCT_PK_THRSHLD") ? .5 : (Convert.ToDouble(dr["IMO_PCT_PK_THRSHLD"]));
                        //BEGIN TT#108 - MD - DOConnell - FWOS Max Model
                        imoi.IMOFWOSMaxRID = dr.IsNull("IMO_FWOS_MAX_RID") ? int.MaxValue : (Convert.ToInt32(dr["IMO_FWOS_MAX_RID"]));
                        imoi.IMOFWOSMaxType = dr.IsNull("IMO_FWOS_MAX_TYPE") ? eModifierType.None : (eModifierType)(Convert.ToInt32(dr["IMO_FWOS_MAX_TYPE"]));
                        
                        // Begin TT#2381 - JSmith - VSW FWOS MAX values of zero do not hold when hierarch service is restarted
                        //imoi.IMOFWOSMax = dr.IsNull("IMO_FWOS_MAX") ? 0.0 : (Convert.ToDouble(dr["IMO_FWOS_MAX"])); // TT#2225 - gtaylor - ANF VSW
                        if (imoi.IMOFWOSMaxType == eModifierType.Percent)
                        {
                            imoi.IMOFWOSMax = dr.IsNull("IMO_FWOS_MAX") ? int.MaxValue : (Convert.ToDouble(dr["IMO_FWOS_MAX"]));
                        }
                        // End TT#2381
						//END TT#108 - MD - DOConnell - FWOS Max Model
                        imoi.IMOPshToBackStock = dr.IsNull("IMO_PSH_BK_STK") ? 0 : Convert.ToInt32(dr["IMO_PSH_BK_STK"]);
                        nimoi.IMO.Add(imoi.IMOStoreRID, imoi);
                    }
                }
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }

        /// <summary>
        /// updates a nodes imo data
        /// </summary>
        /// <param name="imopl">the imo profile list</param>
        static public void UpdateNodeIMO(int NodeRID, IMOProfileList imopl)
        {
            NodeIMOInfo nimoi = null;
            try
            {
                imo_rwl.AcquireWriterLock(WriterLockTimeOut);
                try
                {
                    if (_imoByRID.ContainsKey(NodeRID))
                    {
                        nimoi = (NodeIMOInfo)_imoByRID[NodeRID];
                    }
                    if (nimoi == null)
                    {
                        nimoi = new NodeIMOInfo();
                    }

                    IMOInfo imoi;
                    bool found = false;

                    foreach (IMOProfile imop in imopl)
                    {
                        //if (imop.IsDefaultValues) imop.IMOChangeType = eChangeType.delete;
                        if (imop.IMOChangeType != eChangeType.none)
                        {
                            if (nimoi.IMO.Contains(imop.Key))
                            {
                                imoi = (IMOInfo)nimoi.IMO[imop.Key];
                                found = true;
                            }
                            else
                            {
                                imoi = new IMOInfo();
                                found = false;
                            }

                            if (imop.IMOChangeType == eChangeType.delete)
                            {
                                if (found)
                                {
                                    nimoi.IMO.Remove(imoi.IMOStoreRID);
                                }
                            }
                            else
                            {
                                imoi.IMOStoreRID = imop.Key;
                                imoi.IMOMaxValue = imop.IMOMaxValue;
                                imoi.IMOMinShipQty = imop.IMOMinShipQty;
                                imoi.IMONodeRID = imop.IMONodeRID;
                                imoi.IMOPackQty = imop.IMOPackQty;
                                imoi.IMOFWOSMax = imop.IMOFWOS_Max; // TT#2225 - gtaylor - ANF VSW
                                imoi.IMOFWOSMaxRID = imop.IMOFWOS_MaxModelRID; //TT#108 - MD - DOConnell - FWOS Max Model
                                imoi.IMOFWOSMaxType = imop.IMOFWOS_MaxType; //TT#108 - MD - DOConnell - FWOS Max Model
                                imoi.IMOPshToBackStock = imop.IMOPshToBackStock;
                                if (!found)
                                {
                                    nimoi.IMO.Add(imoi.IMOStoreRID, imoi);
                                }
                                else
                                {
                                    nimoi.IMO[imoi.IMOStoreRID] = imoi;
                                }
                            }
                        }
                        _imoByRID[NodeRID] = nimoi;
                    }
                }
                catch (Exception ex)
                {
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    EventLog.WriteEntry("MIDHierarchyService", "IMO Node Update error:" + ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    // Ensure that the lock is released.
                    imo_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:IMONodeUpdate writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:IMONodeUpdate writer lock has timed out");
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }

        /// <summary>
        /// this method will update hashes and lists based on a change to the Store's IMO_ID
        /// </summary>
        /// <param name="storeRID"></param>
        /// <returns></returns>
        static public bool UpdateNodeIMOStore(int storeRID)
        {
            return true;
        }
        // end TT#1401 - gtaylor - reservation stores - get node imo rows

		// BEGIN TT#1401 - stodd - add resevation stores (IMO)
		/// <summary>
		/// Retrieves a list of keys to read to get intransit for the node
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aLevelType">The master level type of the node to retrieve</param>
		/// <returns>
		/// An instance of the ArrayList object containing IntransitReadNodeProfile objects for each key.
		/// </returns>
		/// <remarks>The node's home hierarchy will be used</remarks>
		static public ArrayList GetIMOReadNodes(int aNodeRID, eHierarchyLevelMasterType aLevelType)
		{
			try
			{
				int nodeRID, colorCodeRID, sizeCodeRID;
				eHierarchyLevelMasterType levelType;
				bool styleDefinedInHierarchy = false;
				ArrayList IMOReadNodes = new ArrayList();
				IMOReadNodeProfile irnp;
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = null;
				dt = mhd.GetReadIMONodes(aNodeRID, aLevelType);
				foreach (DataRow dr in dt.Rows)
				{
					nodeRID = Convert.ToInt32(dr["HN_RID"]);
					colorCodeRID = Convert.ToInt32(dr["COLOR_CODE_RID"]);
					sizeCodeRID = Convert.ToInt32(dr["SIZE_CODE_RID"]);
					levelType = (eHierarchyLevelMasterType)Convert.ToInt32(dr["LEVEL_TYPE"]);
					styleDefinedInHierarchy = Include.ConvertCharToBool(Convert.ToChar(dr["STYLE_IN_PATH"]));
					switch (levelType)
					{
						case eHierarchyLevelMasterType.Color:
							irnp = new IMOReadNodeProfile(nodeRID, colorCodeRID);
							irnp.StyleDefinedInHierarchy = styleDefinedInHierarchy;
							break;
						case eHierarchyLevelMasterType.Size:
							irnp = new IMOReadNodeProfile(nodeRID, colorCodeRID, sizeCodeRID);
							irnp.StyleDefinedInHierarchy = styleDefinedInHierarchy;
							break;
						default:
							irnp = new IMOReadNodeProfile(nodeRID, levelType);
							break;
					}
					IMOReadNodes.Add(irnp);
				}
				return IMOReadNodes;
			}
			catch
			{
				throw;
			}
		}
		// END TT#1401 - stodd - add resevation stores (IMO)

		/// <summary>
		/// Determines if the node is a child of the parent RID
		/// </summary>
		/// <param name="aHierarchyRID">The record ID of the hierarchy</param>
		/// <param name="aParentRID">The record ID of the parent</param>
		/// <param name="aNodeRID">The record ID of the child</param>
		/// <returns>
		/// A boolean indicating if the node is a child of the parent in any hierarchy
		/// </returns>
		static public bool IsParentChild(int aHierarchyRID, int aParentRID, int aNodeRID)
		{
			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.Hierarchy_Join_Read(aHierarchyRID, aParentRID, aNodeRID);
				if (dt.Rows.Count > 0)
				{
					return true;
				}
				return false;

			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Returns an indicator if the record id exists in a hierarchy.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <returns></returns>
		static public bool NodeExists(int nodeRID)
		{
			try
			{
				hierarchy_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
					// make sure node is loaded in cache
					NodeInfo ni = GetNodeInfoByRID(nodeRID);
					if (ni != null &&
						ni.NodeRID != Include.NoRID)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					hierarchy_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:NodeExists reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:NodeExists reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Adds or updates information about a node
		/// </summary>
		/// <param name="hnp">An instance of the HierarchyNodeProfile class containing the information for the node</param>
		static public void NodeUpdateProfileInfo(HierarchyNodeProfile hnp)
		{
			try
			{
				hierarchy_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					switch (hnp.NodeChangeType)
					{
						case eChangeType.none: 
						{
							break;
						}
						case eChangeType.add: 
						{
							NodeInfo ni = new NodeInfo();
							ni.NodeRID = hnp.Key;
							ni.NodeID = hnp.NodeID;
							ni.NodeName = hnp.NodeName;
							ni.NodeDescription = hnp.NodeDescription;
                            // Begin TT#626-MD - JSmith - Size Name and Description Missing after Auto-Add
                            if (hnp.LevelType == eHierarchyLevelType.Color)
                            {
                                if (string.IsNullOrEmpty(ni.NodeName))
                                {
                                    ColorCodeInfo cci = GetColorCodeInfoByRID(hnp.ColorOrSizeCodeRID);
                                    ni.NodeName = cci.ColorCodeName;
                                }
                                // Begin TT#3573 - JSmith - Improve performance loading color level
                                _dctColorNodeLookup.Add(hnp.HomeHierarchyParentRID + ":" + hnp.NodeID, new ColorNodeInfo(hnp.ColorOrSizeCodeRID, hnp.NodeDescription));
                                // End TT#3573 - JSmith - Improve performance loading color level
                            }
                            else if (hnp.LevelType == eHierarchyLevelType.Size)
                            {
                                SizeCodeInfo sci = GetSizeCodeInfoByRID(hnp.ColorOrSizeCodeRID);
                                if (string.IsNullOrEmpty(ni.NodeName))
                                {
                                    ni.NodeName = sci.SizeCodeName;
                                }
                                if (string.IsNullOrEmpty(ni.NodeDescription))
                                {
                                    ni.NodeDescription = sci.SizeCodeName;
                                }
                            }
                            // End TT#626-MD - JSmith - Size Name and Description Missing after Auto-Add
							ni.HomeHierarchyRID = hnp.HomeHierarchyRID;
							ni.HomeHierarchyLevel = hnp.HomeHierarchyLevel;
							ni.LevelType = hnp.LevelType;
							ni.ProductTypeIsOverridden = hnp.ProductTypeIsOverridden;
							ni.ProductType = hnp.ProductType;
							ni.OTSPlanLevelIsOverridden = hnp.OTSPlanLevelIsOverridden;
							ni.OTSPlanLevelSelectType = hnp.OTSPlanLevelSelectType;
							ni.OTSPlanLevelLevelType = hnp.OTSPlanLevelLevelType;
							ni.OTSPlanLevelHierarchyRID = hnp.OTSPlanLevelHierarchyRID;
							ni.OTSPlanLevelHierarchyLevelSequence = hnp.OTSPlanLevelHierarchyLevelSequence;
							ni.OTSPlanLevelAnchorNode = hnp.OTSPlanLevelAnchorNode;
							ni.OTSPlanLevelMaskField = hnp.OTSPlanLevelMaskField;
							ni.OTSPlanLevelMask = hnp.OTSPlanLevelMask;
							ni.OTSPlanLevelTypeIsOverridden = hnp.OTSPlanLevelTypeIsOverridden;
							ni.OTSPlanLevelType = hnp.OTSPlanLevelType;
							ni.UseBasicReplenishment = hnp.UseBasicReplenishment;
							ni.ColorOrSizeCodeRID = hnp.ColorOrSizeCodeRID;
							ni.PurgeDailyHistoryAfter = hnp.PurgeDailyHistoryAfter;
							ni.PurgeWeeklyHistoryAfter = hnp.PurgeWeeklyHistoryAfter;
							ni.PurgeOTSPlansAfter = hnp.PurgeOTSPlansAfter;
                            //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                            //ni.PurgeHeadersAfter = hnp.PurgeHeadersAfter;
                            ni.PurgeHtASNAfter = hnp.PurgeHtASNAfter;
                            ni.PurgeHtDropShipAfter = hnp.PurgeHtDropShipAfter;
                            ni.PurgeHtDummyAfter = hnp.PurgeHtDummyAfter;
                            ni.PurgeHtReceiptAfter = hnp.PurgeHtReceiptAfter;
                            ni.PurgeHtPurchaseOrderAfter = hnp.PurgeHtPurchaseOrderAfter;
                            ni.PurgeHtReserveAfter = hnp.PurgeHtReserveAfter;
                            ni.PurgeHtVSWAfter = hnp.PurgeHtVSWAfter;
                            ni.PurgeHtWorkUpTotAfter = hnp.PurgeHtWorkUpTotAfter;
                            //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
							ni.IsVirtual = hnp.IsVirtual;
                            ni.Purpose = hnp.Purpose;
                            ni.DeleteNode = hnp.DeleteNode;  // Begin TT#3630 - JSmith - Delete My Hierarchy
                            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                            hnp.Active = ni.Active;
                            // End TT#988
                            // BEGIN TT#1399
                            ni.ApplyHNRIDFrom = hnp.ApplyHNRIDFrom;
                            // END TT#1399
							SetNodeCacheByRID(ni.NodeRID, ni);
							if (hnp.LevelType == eHierarchyLevelType.Color ||   
								hnp.LevelType == eHierarchyLevelType.Size)      
							{
								SetColorSizeRIDByID(hnp.QualifiedNodeID, ni.NodeRID);
							}
							else if (ni.NodeID != null)
							{
								SetNodeRIDByID(ni.NodeID, ni.NodeRID);
							}
							HierarchyJoinProfile hjp = new HierarchyJoinProfile(hnp.Key);
							hjp.JoinChangeType = eChangeType.add;
							hjp.NewHierarchyRID = hnp.HierarchyRID;
							hjp.NewParentRID = hnp.HomeHierarchyParentRID;
							hjp.LevelType = hnp.LevelType;
							JoinUpdate(hjp);
							// update that the level has nodes
							if (ni.HomeHierarchyLevel > 0)
							{
								HierarchyInfo hi = GetHierarchyInfoByRID(ni.HomeHierarchyRID, false);
								if (hi.HierarchyType == eHierarchyType.organizational)
								{
									HierarchyLevelInfo hli = (HierarchyLevelInfo)hi.HierarchyLevels[ni.HomeHierarchyLevel];
									//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
									//++hli.LevelNodeCount;
                                    // Begin TT#3985 - JSmith - Up and down arrows allow user to move levels after nodes are defined
                                    //hli.LevelNodesExist = true;
                                    ////End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
                                    //hi.HierarchyLevels[ni.HomeHierarchyLevel] = hli;
                                    if (!hli.LevelNodesExist)
                                    {
                                        SetLevelNodesExist(ni.HomeHierarchyRID, ni.HomeHierarchyLevel);
                                    }
                                    // End TT#3985 - JSmith - Up and down arrows allow user to move levels after nodes are defined
								}
							}
							break;
						}
						case eChangeType.update: 
						{
							NodeInfo ni = GetNodeInfoByRID(hnp.Key);
							
							if (hnp.LevelType == eHierarchyLevelType.Color ||   
								hnp.LevelType == eHierarchyLevelType.Size)      
							{
                                // Begin TT#634 - JSmith - Color rename
                                //RemoveNodeRIDByID(hnp.QualifiedNodeID);
                                //RemoveColorSizeRIDByID(hnp.QualifiedNodeID);
                                string qualifiedNodeID = BuildCurrentQualifiedNode(hnp);
                                RemoveNodeRIDByID(qualifiedNodeID);
                                RemoveColorSizeRIDByID(qualifiedNodeID);
                                // End TT#634
                                // Begin TT#3573 - JSmith - Improve performance loading color level
                                if (hnp.LevelType == eHierarchyLevelType.Color)
                                {
                                    if (ni.NodeID != hnp.NodeID)
                                    {
                                        _dctColorNodeLookup.Remove(hnp.HomeHierarchyParentRID + ":" + ni.NodeID);
                                        _dctColorNodeLookup.Add(hnp.HomeHierarchyParentRID + ":" + hnp.NodeID, new ColorNodeInfo(hnp.ColorOrSizeCodeRID, hnp.NodeDescription));
                                    }
                                    
                                }
                                // End TT#3573 - JSmith - Improve performance loading color level
							}
							else
							{
								RemoveNodeRIDByID(ni.NodeID);
							}
							ni.NodeID = hnp.NodeID;
							ni.NodeName = hnp.NodeName;
							ni.NodeDescription = hnp.NodeDescription;
							ni.HomeHierarchyRID = hnp.HomeHierarchyRID;
							ni.HomeHierarchyLevel = hnp.HomeHierarchyLevel;
							ni.LevelType = hnp.LevelType;
							ni.ProductTypeIsOverridden = hnp.ProductTypeIsOverridden;
							ni.ProductType = hnp.ProductType;
							ni.OTSPlanLevelIsOverridden = hnp.OTSPlanLevelIsOverridden;
                            // Begin TT#903 - JSmith -  changed plan level back to undefined and get error when select style review
                            //ni.OTSPlanLevelSelectType = hnp.OTSPlanLevelSelectType;
                            //ni.OTSPlanLevelLevelType = hnp.OTSPlanLevelLevelType;
                            //ni.OTSPlanLevelHierarchyRID = hnp.OTSPlanLevelHierarchyRID;
                            //ni.OTSPlanLevelHierarchyLevelSequence = hnp.OTSPlanLevelHierarchyLevelSequence;
                            //ni.OTSPlanLevelAnchorNode = hnp.OTSPlanLevelAnchorNode;
                            //ni.OTSPlanLevelMaskField = hnp.OTSPlanLevelMaskField;
                            //ni.OTSPlanLevelMask = hnp.OTSPlanLevelMask;
                            //ni.OTSPlanLevelTypeIsOverridden = hnp.OTSPlanLevelTypeIsOverridden;
                            //ni.OTSPlanLevelType = hnp.OTSPlanLevelType;
                            if (ni.OTSPlanLevelIsOverridden)
                            {
                                ni.OTSPlanLevelSelectType = hnp.OTSPlanLevelSelectType;
                                ni.OTSPlanLevelLevelType = hnp.OTSPlanLevelLevelType;
                                ni.OTSPlanLevelHierarchyRID = hnp.OTSPlanLevelHierarchyRID;
                                ni.OTSPlanLevelHierarchyLevelSequence = hnp.OTSPlanLevelHierarchyLevelSequence;
                                ni.OTSPlanLevelAnchorNode = hnp.OTSPlanLevelAnchorNode;
                                ni.OTSPlanLevelMaskField = hnp.OTSPlanLevelMaskField;
                                ni.OTSPlanLevelMask = hnp.OTSPlanLevelMask;
								//Begin TT#1371 - JScott - Cannot change Node Properties Type from Total to Regular
								//ni.OTSPlanLevelTypeIsOverridden = hnp.OTSPlanLevelTypeIsOverridden;
								//ni.OTSPlanLevelType = hnp.OTSPlanLevelType;
								//End TT#1371 - JScott - Cannot change Node Properties Type from Total to Regular
							}
                            else
                            {
                                ni.OTSPlanLevelSelectType = ePlanLevelSelectType.Undefined;
                                ni.OTSPlanLevelLevelType = ePlanLevelLevelType.Undefined;
                                ni.OTSPlanLevelHierarchyRID = Include.Undefined;
                                ni.OTSPlanLevelHierarchyLevelSequence = Include.Undefined;
                                ni.OTSPlanLevelAnchorNode = Include.Undefined;
                                ni.OTSPlanLevelMaskField = eMaskField.Undefined;
                                ni.OTSPlanLevelMask = null;
								//Begin TT#1371 - JScott - Cannot change Node Properties Type from Total to Regular
								//ni.OTSPlanLevelTypeIsOverridden = false;
								//ni.OTSPlanLevelType = eOTSPlanLevelType.Undefined;
								//End TT#1371 - JScott - Cannot change Node Properties Type from Total to Regular
							}
                            // End TT#903
							//Begin TT#1371 - JScott - Cannot change Node Properties Type from Total to Regular
							ni.OTSPlanLevelTypeIsOverridden = hnp.OTSPlanLevelTypeIsOverridden;
							if (ni.OTSPlanLevelTypeIsOverridden)
							{
								ni.OTSPlanLevelType = hnp.OTSPlanLevelType;
							}
							else
							{
								ni.OTSPlanLevelType = eOTSPlanLevelType.Undefined;
							}
							//End TT#1371 - JScott - Cannot change Node Properties Type from Total to Regular
							ni.UseBasicReplenishment = hnp.UseBasicReplenishment;
                            // Begin TT#1005 - JSmith - PO Changes after Color Rename are in "release approved"
                            ni.ColorOrSizeCodeRID = hnp.ColorOrSizeCodeRID;
                            // End TT#1005
							if (hnp.PurgeDailyCriteriaInherited == eInheritedFrom.None)
							{
								ni.PurgeDailyHistoryAfter = hnp.PurgeDailyHistoryAfter;
							}
							if (hnp.PurgeWeeklyCriteriaInherited == eInheritedFrom.None)
							{
								ni.PurgeWeeklyHistoryAfter = hnp.PurgeWeeklyHistoryAfter;
							}
							if (hnp.PurgeOTSCriteriaInherited == eInheritedFrom.None)
							{
								ni.PurgeOTSPlansAfter = hnp.PurgeOTSPlansAfter;
							}
                            //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                            //if (hnp.PurgeHeadersCriteriaInherited == eInheritedFrom.None)
                            //{
                            //    ni.PurgeHeadersAfter = hnp.PurgeHeadersAfter;
                            //}
                            if (hnp.PurgeHtASNCriteriaInherited == eInheritedFrom.None)
                            {
                                ni.PurgeHtASNAfter = hnp.PurgeHtASNAfter;
                            }

                            if (hnp.PurgeHtDropShipCriteriaInherited == eInheritedFrom.None)
                            {
                                ni.PurgeHtDropShipAfter = hnp.PurgeHtDropShipAfter;
                            }

                            if (hnp.PurgeHtDummyCriteriaInherited == eInheritedFrom.None)
                            {
                                ni.PurgeHtDummyAfter = hnp.PurgeHtDummyAfter;
                            }

                            if (hnp.PurgeHtReceiptCriteriaInherited == eInheritedFrom.None)
                            {
                                ni.PurgeHtReceiptAfter = hnp.PurgeHtReceiptAfter;
                            }

                            if (hnp.PurgeHtPurchaseOrderCriteriaInherited == eInheritedFrom.None)
                            {
                                ni.PurgeHtPurchaseOrderAfter = hnp.PurgeHtPurchaseOrderAfter;
                            }

                            if (hnp.PurgeHtReserveCriteriaInherited == eInheritedFrom.None)
                            {
                                ni.PurgeHtReserveAfter = hnp.PurgeHtReserveAfter;
                            }

                            if (hnp.PurgeHtVSWCriteriaInherited == eInheritedFrom.None)
                            {
                                ni.PurgeHtVSWAfter = hnp.PurgeHtVSWAfter;
                            }

                            if (hnp.PurgeHtWorkUpTotCriteriaInherited == eInheritedFrom.None)
                            {
                                ni.PurgeHtWorkUpTotAfter = hnp.PurgeHtWorkUpTotAfter;
                            }
                            //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                            ni.IsVirtual = hnp.IsVirtual;
                            ni.Purpose = hnp.Purpose;
                            ni.DeleteNode = hnp.DeleteNode;  // Begin TT#3630 - JSmith - Delete My Hierarchy
                            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                            hnp.Active = ni.Active;
                            // End TT#988
							// BEGIN TT#1399
                            ni.ApplyHNRIDFrom = hnp.ApplyHNRIDFrom;
                            // END TT#1399
							SetNodeCacheByRID(hnp.Key, ni);
							if (hnp.LevelType == eHierarchyLevelType.Color ||   
								hnp.LevelType == eHierarchyLevelType.Size)      
							{
                                // Begin TT#1955-MD - JSmith - Cache not updated correctly after a color rename
                                //SetColorSizeRIDByID(hnp.QualifiedNodeID, ni.NodeRID);
                                string qualifiedNodeID = BuildCurrentQualifiedNode(hnp);
                                SetColorSizeRIDByID(qualifiedNodeID, ni.NodeRID);
                                // End TT#1955-MD - JSmith - Cache not updated correctly after a color rename 
							}
                            else if (ni.NodeID != null)
							{
								SetNodeRIDByID(ni.NodeID, ni.NodeRID);
							}
							break;
						}
						case eChangeType.delete: 
						{
							//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
							//Begin TT#155 - JScott - Add Size Curve info to Node Properties
							//DeleteNodeData(hnp.Key, true, false, false, false, false, false, false, hnp.QualifiedNodeID);
							//DeleteNodeData(hnp.Key, true, false, false, false, false, false, false, false, false, false, hnp.QualifiedNodeID);
							//End TT#155 - JScott - Add Size Curve info to Node Properties
                            // Begin TT#3982 - JSmith - Node Properties – Stock Min/Max Apply to lower levels not working
                            //DeleteNodeData(hnp.Key, true, false, false, false, false, false, false, false, false, false, false, false, false, false, hnp.QualifiedNodeID);
                            DeleteNodeData(hnp.Key, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, hnp.QualifiedNodeID);
                            // End TT#3982 - JSmith - Node Properties – Stock Min/Max Apply to lower levels not working
							//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
                            // Begin TT#3573 - JSmith - Improve performance loading color level
                            if (hnp.LevelType == eHierarchyLevelType.Color)
                            {
                                _dctColorNodeLookup.Remove(hnp.HomeHierarchyParentRID + ":" + hnp.NodeID);
                            }
                            // End TT#3573 - JSmith - Improve performance loading color level
							break;
						}
						// Begin TT#3630 - JSmith - Delete My Hierarchy
                        case eChangeType.markedForDelete:
                        {
                            NodeInfo ni = GetNodeInfoByRID(hnp.Key);

                            RemoveNodeRIDByID(ni.NodeID);

                            ni.NodeID = hnp.NodeID;
                            ni.DeleteNode = hnp.DeleteNode;

                            SetNodeCacheByRID(hnp.Key, ni);
                            SetNodeRIDByID(ni.NodeID, ni.NodeRID);

                            break;
                        }
						// End TT#3630 - JSmith - Delete My Hierarchy
					}
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					EventLog.WriteEntry("MIDHierarchyService", "NodeUpdateProfileInfo error:" + ex.Message, EventLogEntryType.Error);
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					hierarchy_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:NodeUpdateProfileInfo writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:NodeUpdateProfileInfo writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Buids the qualified node from the profile for color and size nodes
		/// </summary>
		/// <param name="hnp">An instance of the HierarchyNodeProfile class containing the information for the node</param>
		/// <remarks>The qualified node is only built if the color/size ID cache is used</remarks>
		/// <returns>A string with the fully qualified node ID</returns>
		static public string BuildQualifiedNode(HierarchyNodeProfile hnp)
		{
			try
			{
				string qualifiedNode = null;
				// only build qualified node if cache is used.
				if (_colorSizesCacheUsed)
				{
					if (hnp.LevelType == eHierarchyLevelType.Color ||
						hnp.LevelType == eHierarchyLevelType.Size)
					{
						HierarchyNodeProfile parent_hnp = GetNodeData(hnp.HomeHierarchyParentRID, false);
						qualifiedNode = parent_hnp.NodeID + _globalOptions.ProductLevelDelimiter + hnp.NodeID;
						if (hnp.LevelType == eHierarchyLevelType.Size)
						{
							HierarchyNodeProfile grandparent_hnp = GetAncestorData(parent_hnp.HomeHierarchyRID, parent_hnp.Key, 1);
							qualifiedNode = grandparent_hnp.NodeID + _globalOptions.ProductLevelDelimiter + qualifiedNode;
						}
					}
					//Begin TT#417 - JScott - Size Curve - Generated a Size Curve the name is too long to read in Admini>Size Curve.  Need a Tool Tip so user can read the name of the Curve created.
					else
					{
						qualifiedNode = hnp.NodeID;
					}
					//End TT#417 - JScott - Size Curve - Generated a Size Curve the name is too long to read in Admini>Size Curve.  Need a Tool Tip so user can read the name of the Curve created.
				}
				return qualifiedNode;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        // Begin TT#634 - JSmith - Color rename
        /// <summary>
        /// Buids the current qualified node from the profile for color and size nodes
        /// </summary>
        /// <param name="hnp">An instance of the HierarchyNodeProfile class containing the information for the node</param>
        /// <remarks>The qualified node is only built if the color/size ID cache is used</remarks>
        /// <returns>A string with the fully qualified node ID</returns>
        static public string BuildCurrentQualifiedNode(HierarchyNodeProfile hnp)
        {
            try
            {
                NodeInfo ni = GetNodeInfoByRID(hnp.Key, false);
                string qualifiedNode = null;
                // only build qualified node if cache is used.
                if (_colorSizesCacheUsed)
                {
                    if (hnp.LevelType == eHierarchyLevelType.Color ||
                        hnp.LevelType == eHierarchyLevelType.Size)
                    {
                        HierarchyNodeProfile parent_hnp = GetNodeData(hnp.HomeHierarchyParentRID, false);
                        qualifiedNode = parent_hnp.NodeID + _globalOptions.ProductLevelDelimiter + ni.NodeID;
                        if (hnp.LevelType == eHierarchyLevelType.Size)
                        {
                            HierarchyNodeProfile grandparent_hnp = GetAncestorData(parent_hnp.HomeHierarchyRID, parent_hnp.Key, 1);
                            qualifiedNode = grandparent_hnp.NodeID + _globalOptions.ProductLevelDelimiter + qualifiedNode;
                        }
                    }
                    else
                    {
                        qualifiedNode = ni.NodeID;
                    }
                }
                return qualifiedNode;
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }
        // End TT#634

		/// <summary>
		/// Selectively deletes information associated with a node from the database
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="deleteNode">A flag to identify if the node is to be deleted.  This will automatically delete all other data.</param>
		/// <param name="deleteEligibility">A flag to identify if the eligibility is to be deleted.</param>
		/// <param name="deleteStoreGrades">A flag to identify if the store grages are to be deleted.</param>
		/// <param name="deleteVelocityGrades">A flag to identify if the velocity grades are to be deleted.</param>
		/// <param name="deleteStoreCapacity">A flag to identify if the store capacities are to be deleted.</param>
		/// <param name="deletePurgeCriteria">A flag to identify if the purge criteria is to be deleted.</param>
		/// <param name="deleteDailyPercentages">A flag to identify if the daily percentages are to be deleted.</param>
		static public void DeleteNodeData(int nodeRID, bool deleteNode, bool deleteEligibility, bool deleteStoreGrades,
			bool deleteVelocityGrades, bool deleteStoreCapacity, bool deletePurgeCriteria,
			//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
			//Begin TT#155 - JScott - Add Size Curve info to Node Properties
			//bool deleteDailyPercentages, string aQualifiedNodeID)
			//bool deleteDailyPercentages, bool deleteSizeCurveCriteria, bool deleteSizeCurveTolerance,
			//bool deleteSizeCurveSimilarStore, string aQualifiedNodeID)
			//End TT#155 - JScott - Add Size Curve info to Node Properties
			bool deleteDailyPercentages, bool deleteSizeCurveCriteria, bool deleteSizeCurveTolerance,
			bool deleteSizeCurveSimilarStore, bool deleteSizeOutOfStock, bool deleteSizeSellThru,
            bool deleteCharacteristics, 
            bool deleteVSW,  //  TT#2015 - gtaylor - apply changes to lower levels
            bool deleteSellThruPcts, bool deleteStockMinMaxes, // TT#3982 - JSmith - Node Properties – Stock Min/Max Apply to lower levels not working
			string aQualifiedNodeID)
			//End TT#483 - JScott - Add Size Lost Sales criteria and processing
		{
			if (deleteStoreGrades || deleteNode)
			{
				try
				{
					storeGrades_rwl.AcquireWriterLock(WriterLockTimeOut);
					try
					{
						_storeGradesByRID.Remove(nodeRID);
					}
					catch ( Exception ex )
					{
                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                        if (Audit != null)
                        {
                            Audit.Log_Exception(ex);
                        }
                        // End TT#189
						throw;
					}
					finally
					{
						// Ensure that the lock is released.
						storeGrades_rwl.ReleaseWriterLock();
					}
				}
				catch (ApplicationException ex)
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					// The writer lock request timed out.
					EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:DeleteNodeData writer lock has timed out", EventLogEntryType.Error);
					throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:DeleteNodeData writer lock has timed out");
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
			}

			if (deleteVelocityGrades || deleteNode)
			{
				try
				{
					velocityGrades_rwl.AcquireWriterLock(WriterLockTimeOut);
					try
					{
						_velocityGradesByRID.Remove(nodeRID);
					}
					catch ( Exception ex )
					{
                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                        if (Audit != null)
                        {
                            Audit.Log_Exception(ex);
                        }
                        // End TT#189
						throw;
					}
					finally
					{
						// Ensure that the lock is released.
						velocityGrades_rwl.ReleaseWriterLock();
					}
				}
				catch (ApplicationException ex)
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					// The writer lock request timed out.
					EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:DeleteNodeData writer lock has timed out", EventLogEntryType.Error);
					throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:DeleteNodeData writer lock has timed out");
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
			}

			if (deleteStoreCapacity || deleteNode)
			{
				try
				{
					storeCapacity_rwl.AcquireWriterLock(WriterLockTimeOut);
					try
					{
						_storeCapacityByRID.Remove(nodeRID);
					}
					catch ( Exception ex )
					{
                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                        if (Audit != null)
                        {
                            Audit.Log_Exception(ex);
                        }
                        // End TT#189
						throw;
					}
					finally
					{
						// Ensure that the lock is released.
						storeCapacity_rwl.ReleaseWriterLock();
					}
				}
				catch (ApplicationException ex)
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					// The writer lock request timed out.
					EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:DeleteNodeData writer lock has timed out", EventLogEntryType.Error);
					throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:DeleteNodeData writer lock has timed out");
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
			}

			if (deleteEligibility || deleteNode)
			{
				try
				{
					eligibility_rwl.AcquireWriterLock(WriterLockTimeOut);
					try
					{
						_eligibilityByRID.Remove(nodeRID);
					}
					catch ( Exception ex )
					{
                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                        if (Audit != null)
                        {
                            Audit.Log_Exception(ex);
                        }
                        // End TT#189
						throw;
					}
					finally
					{
						// Ensure that the lock is released.
						eligibility_rwl.ReleaseWriterLock();
					}
				}
				catch (ApplicationException ex)
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					// The writer lock request timed out.
					EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:DeleteNodeData writer lock has timed out", EventLogEntryType.Error);
					throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:DeleteNodeData writer lock has timed out");
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
			}

			if (deleteDailyPercentages || deleteNode)
			{
				try
				{
					dailyPercentages_rwl.AcquireWriterLock(WriterLockTimeOut);
					try
					{
						_dailyPercentagesByRID.Remove(nodeRID);
					}
					catch ( Exception ex )
					{
                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                        if (Audit != null)
                        {
                            Audit.Log_Exception(ex);
                        }
                        // End TT#189
						throw;
					}
					finally
					{
						// Ensure that the lock is released.
						dailyPercentages_rwl.ReleaseWriterLock();
					}
				}
				catch (ApplicationException ex)
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					// The writer lock request timed out.
					EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:DeleteNodeData writer lock has timed out", EventLogEntryType.Error);
					throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:DeleteNodeData writer lock has timed out");
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
			}

            if (deletePurgeCriteria || deleteCharacteristics || deleteNode) //  TT#2015 - gtaylor - apply changes to lower levels
			{
				try
				{
					hierarchy_rwl.AcquireWriterLock(WriterLockTimeOut);
					try
					{
						NodeInfo ni = GetNodeInfoByRID(nodeRID, false);

                        if (deleteNode)   //  TT#2015 - gtaylor - apply changes to lower levels
                        {
                            RemoveNodeCacheByRID(nodeRID);
                            if (ni.LevelType == eHierarchyLevelType.Color ||
                                ni.LevelType == eHierarchyLevelType.Size)
                            {
                                if (aQualifiedNodeID != null)
                                {
                                    RemoveNodeRIDByID(aQualifiedNodeID);
                                    RemoveColorSizeRIDByID(aQualifiedNodeID);
                                }
                            }
                            else
                            {
                                RemoveNodeRIDByID(ni.NodeID);
                            }
							foreach (KeyValuePair<int, HierarchyInfo>  hierarchy in _hierarchiesByRID) // remove relationships from hierarchies
							{
								int hierarchyRID = hierarchy.Key;
								HierarchyInfo hi = hierarchy.Value;
                                HierarchyInfo.NodeRelationship nr;
                                if (hi.ParentChildRelationship.TryGetValue(nodeRID, out nr))
								{
                                    //HierarchyInfo.NodeRelationship nr = hi.GetChildRelationship(nodeRID);			// get as parent
                                    hi.ParentChildRelationship.Remove(nodeRID);			// remove as parent

                                    // update all parents in hierarchy
                                    if (nr.ParentsCount() > 0)
                                    {
                                        foreach (int parentRID in nr.parents)
                                        {
                                            HierarchyInfo.NodeRelationship parent_nr = hi.GetChildRelationship(parentRID);	// get as child
                                            parent_nr.ChildrenRemove(nodeRID);					// remove as child from parent
                                            hi.ParentChildRelationship[parentRID] = parent_nr;	// update parent relationships
                                        }
                                    }
                                }
                            }
                            // remove node from level node count
                            if (ni.HomeHierarchyLevel > 0)
                            {
                                HierarchyInfo hi = (HierarchyInfo)_hierarchiesByRID[ni.HomeHierarchyRID];
                                if (hi.HierarchyType == eHierarchyType.organizational)
                                {
                                    HierarchyLevelInfo hli = (HierarchyLevelInfo)hi.HierarchyLevels[ni.HomeHierarchyLevel];
                                    //Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
                                    //--hli.LevelNodeCount;
                                    //End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
                                    hi.HierarchyLevels[ni.HomeHierarchyLevel] = hli;
                                }
                            }
                        }
                        else
                        {
                            if (deletePurgeCriteria)
                            {
                                ni.PurgeDailyHistoryAfter = Include.Undefined;
                                ni.PurgeOTSPlansAfter = Include.Undefined;
                                ni.PurgeWeeklyHistoryAfter = Include.Undefined;
                                //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                                //ni.PurgeHeadersAfter = Include.Undefined;
                                ni.PurgeHtASNAfter = Include.Undefined;
                                ni.PurgeHtDropShipAfter = Include.Undefined;
                                ni.PurgeHtDummyAfter = Include.Undefined;
                                ni.PurgeHtReceiptAfter = Include.Undefined;
                                ni.PurgeHtPurchaseOrderAfter = Include.Undefined;
                                ni.PurgeHtReserveAfter = Include.Undefined;
                                ni.PurgeHtVSWAfter = Include.Undefined;
                                ni.PurgeHtWorkUpTotAfter = Include.Undefined;
                                //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                            }
                            if (deleteCharacteristics)
                            {
                                ni.ClearCharValues();
                            }
                            SetNodeCacheByRID(nodeRID, ni);
                        }
					}
					catch ( Exception ex )
					{
                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                        if (Audit != null)
                        {
                            Audit.Log_Exception(ex);
                        }
                        // End TT#189
						string exceptionMessage = ex.Message;
						throw;
					}
					finally
					{
						// Ensure that the lock is released.
						hierarchy_rwl.ReleaseWriterLock();
					}
				}
				catch (ApplicationException ex)
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					// The writer lock request timed out.
					EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:DeleteNodeData writer lock has timed out", EventLogEntryType.Error);
					throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:DeleteNodeData writer lock has timed out");
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
			}
			//Begin TT#155 - JScott - Add Size Curve info to Node Properties

            // BEGIN TT#2015 - gtaylor - apply changes to lower levels
            if (deleteVSW || deleteNode)
            {
                try
                {
                    imo_rwl.AcquireWriterLock(WriterLockTimeOut);
                    try
                    {
                        _imoByRID.Remove(nodeRID);
                    }
                    catch (Exception ex)
                    {
                        if (Audit != null)
                        {
                            Audit.Log_Exception(ex);
                        }
                        throw;
                    }
                    finally
                    {
                        imo_rwl.ReleaseWriterLock();
                    }

                }
                catch (ApplicationException ex)
                {
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:DeleteNodeData writer lock has timed out", EventLogEntryType.Error);
                    throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:DeleteNodeData writer lock has timed out");
                }
                catch (Exception ex)
                {
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    throw;
                }
            }
            // END TT#2015 - gtaylor - apply changes to lower levels

			if (deleteSizeCurveCriteria || deleteNode)
			{
				try
				{
					sizeCurveCriteria_rwl.AcquireWriterLock(WriterLockTimeOut);
					try
					{
						_sizeCurveCriteriaByRID.Remove(nodeRID);
					}
					catch (Exception ex)
					{
						if (Audit != null)
						{
							Audit.Log_Exception(ex);
						}
						throw;
					}
					finally
					{
						sizeCurveCriteria_rwl.ReleaseWriterLock();
					}

					sizeCurveDefaultCriteria_rwl.AcquireWriterLock(WriterLockTimeOut);
					try
					{
						_sizeCurveDefaultCriteriaByRID.Remove(nodeRID);
					}
					catch (Exception ex)
					{
						if (Audit != null)
						{
							Audit.Log_Exception(ex);
						}
						throw;
					}
					finally
					{
						sizeCurveDefaultCriteria_rwl.ReleaseWriterLock();
					}
				}
				catch (ApplicationException ex)
				{
					if (Audit != null)
					{
						Audit.Log_Exception(ex);
					}
					EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:DeleteNodeData writer lock has timed out", EventLogEntryType.Error);
					throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:DeleteNodeData writer lock has timed out");
				}
				catch (Exception ex)
				{
					if (Audit != null)
					{
						Audit.Log_Exception(ex);
					}
					throw;
				}
			}

			if (deleteSizeCurveTolerance || deleteNode)
			{
				try
				{
					sizeCurveTolerance_rwl.AcquireWriterLock(WriterLockTimeOut);
					try
					{
						_sizeCurveToleranceByRID.Remove(nodeRID);
					}
					catch (Exception ex)
					{
						if (Audit != null)
						{
							Audit.Log_Exception(ex);
						}
						throw;
					}
					finally
					{
						sizeCurveTolerance_rwl.ReleaseWriterLock();
					}
				}
				catch (ApplicationException ex)
				{
					if (Audit != null)
					{
						Audit.Log_Exception(ex);
					}
					EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:DeleteNodeData writer lock has timed out", EventLogEntryType.Error);
					throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:DeleteNodeData writer lock has timed out");
				}
				catch (Exception ex)
				{
					if (Audit != null)
					{
						Audit.Log_Exception(ex);
					}
					throw;
				}
			}

			if (deleteSizeCurveSimilarStore || deleteNode)
			{
				try
				{
					sizeCurveSimilarStore_rwl.AcquireWriterLock(WriterLockTimeOut);
					try
					{
						_sizeCurveSimilarStoreByRID.Remove(nodeRID);
					}
					catch (Exception ex)
					{
						if (Audit != null)
						{
							Audit.Log_Exception(ex);
						}
						throw;
					}
					finally
					{
						sizeCurveSimilarStore_rwl.ReleaseWriterLock();
					}
				}
				catch (ApplicationException ex)
				{
					if (Audit != null)
					{
						Audit.Log_Exception(ex);
					}
					EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:DeleteNodeData writer lock has timed out", EventLogEntryType.Error);
					throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:DeleteNodeData writer lock has timed out");
				}
				catch (Exception ex)
				{
					if (Audit != null)
					{
						Audit.Log_Exception(ex);
					}
					throw;
				}
			}
			//End TT#155 - JScott - Add Size Curve info to Node Properties
			//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing

			if (deleteSizeOutOfStock || deleteNode)
			{
				try
				{
					sizeOutOfStock_rwl.AcquireWriterLock(WriterLockTimeOut);
					try
					{
                        //_sizeOutOfStockByRID.Remove(nodeRID);
					}
					catch (Exception ex)
					{
						if (Audit != null)
						{
							Audit.Log_Exception(ex);
						}
						throw;
					}
					finally
					{
						sizeOutOfStock_rwl.ReleaseWriterLock();
					}
				}
				catch (ApplicationException ex)
				{
					if (Audit != null)
					{
						Audit.Log_Exception(ex);
					}
					EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:DeleteNodeData writer lock has timed out", EventLogEntryType.Error);
					throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:DeleteNodeData writer lock has timed out");
				}
				catch (Exception ex)
				{
					if (Audit != null)
					{
						Audit.Log_Exception(ex);
					}
					throw;
				}
			}

			if (deleteSizeSellThru || deleteNode)
			{
				try
				{
					sizeSellThru_rwl.AcquireWriterLock(WriterLockTimeOut);
					try
					{
						_sizeSellThruByRID.Remove(nodeRID);
					}
					catch (Exception ex)
					{
						if (Audit != null)
						{
							Audit.Log_Exception(ex);
						}
						throw;
					}
					finally
					{
						sizeSellThru_rwl.ReleaseWriterLock();
					}
				}
				catch (ApplicationException ex)
				{
					if (Audit != null)
					{
						Audit.Log_Exception(ex);
					}
					EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:DeleteNodeData writer lock has timed out", EventLogEntryType.Error);
					throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:DeleteNodeData writer lock has timed out");
				}
				catch (Exception ex)
				{
					if (Audit != null)
					{
						Audit.Log_Exception(ex);
					}
					throw;
				}
			}
			//End TT#483 - JScott - Add Size Lost Sales criteria and processing

            // Begin TT#3982 - JSmith - Node Properties – Stock Min/Max Apply to lower levels not working
            if (deleteSellThruPcts || deleteNode)
            {
                try
                {
                    sellThruPcts_rwl.AcquireWriterLock(WriterLockTimeOut);
                    try
                    {
                        _sellThruPctsByRID.Remove(nodeRID);
                    }
                    catch (Exception ex)
                    {
                        if (Audit != null)
                        {
                            Audit.Log_Exception(ex);
                        }
                        throw;
                    }
                    finally
                    {
                        sellThruPcts_rwl.ReleaseWriterLock();
                    }
                }
                catch (ApplicationException ex)
                {
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:DeleteNodeData writer lock has timed out", EventLogEntryType.Error);
                    throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:DeleteNodeData writer lock has timed out");
                }
                catch (Exception ex)
                {
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    throw;
                }
            }

            if (deleteStockMinMaxes || deleteNode)
            {
                try
                {
                    stockMinMaxes_rwl.AcquireWriterLock(WriterLockTimeOut);
                    try
                    {
                        _stockMinMaxesByRID.Remove(nodeRID);
                    }
                    catch (Exception ex)
                    {
                        if (Audit != null)
                        {
                            Audit.Log_Exception(ex);
                        }
                        throw;
                    }
                    finally
                    {
                        stockMinMaxes_rwl.ReleaseWriterLock();
                    }
                }
                catch (ApplicationException ex)
                {
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:DeleteNodeData writer lock has timed out", EventLogEntryType.Error);
                    throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:DeleteNodeData writer lock has timed out");
                }
                catch (Exception ex)
                {
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    throw;
                }
            }
            // End TT#3982 - JSmith - Node Properties – Stock Min/Max Apply to lower levels not working
		}


		/// <summary>
		/// Checks to determine if a node is already a child for a specified parent within a hierarchy
		/// </summary>
		/// <param name="hjp">An instance of the HierarchyJoinProfile which contains node relationship information</param>
		/// <returns></returns>
		static public bool JoinExists(HierarchyJoinProfile hjp)
		{
			try
			{
				hierarchy_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
					bool joinExists = false;
					HierarchyInfo.NodeRelationship nodeRelationship;
					try
					{
						HierarchyInfo hi = GetHierarchyInfoByRID(hjp.NewHierarchyRID, true);
							
                        //if (hi.ParentChildRelationship.Contains(hjp.NewParentRID))
                        if (hi.ParentChildRelationship.TryGetValue(hjp.NewParentRID, out nodeRelationship))
						{
							if (nodeRelationship.ChildrenContains(hjp.Key))
							{
								joinExists = true;
							}
						}
					}
					catch ( Exception ex )
					{
                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                        if (Audit != null)
                        {
                            Audit.Log_Exception(ex);
                        }
                        // End TT#189
						throw;
					}
					return joinExists;
				}        
				finally
				{
					// Ensure that the lock is released.
					hierarchy_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:JoinExists reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:JoinExists reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Adds or updates relationship information for parents and children in a hierarchy.
		/// </summary>
		/// <param name="hjp">An instance of the HierarchyJoinProfile which contains node relationship information</param>
		static public void JoinUpdate(HierarchyJoinProfile hjp)
		{
			try
			{
				hierarchy_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					switch (hjp.JoinChangeType)
					{
						case eChangeType.none: 
						{
							break;
						}
						case eChangeType.add: 
						{
							AddRelationship(hjp);
							break;
						}
						case eChangeType.update: 
						{
							UpdateRelationship(hjp);
							
							break;
						}
						case eChangeType.delete: 
						{
							DeleteRelationship(hjp);
						
							break;
						}
					}
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					hierarchy_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:JoinUpdate writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:JoinUpdate writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Adds a child node to a parent
		/// </summary>
		/// <param name="hjp">An instance of the HierarchyJoinProfile which contains node relationship information</param>
		static private void AddRelationship(HierarchyJoinProfile hjp)
		{
			HierarchyInfo.NodeRelationship nodeRelationship;
			HierarchyInfo.NodeRelationship child;
			try
			{
				//  add child to parent
				HierarchyInfo hi = (HierarchyInfo)_hierarchiesByRID[hjp.NewHierarchyRID];
							
                //if (hi.ParentChildRelationship.ContainsKey(hjp.NewParentRID))
                if (hi.ParentChildRelationship.TryGetValue(hjp.NewParentRID, out nodeRelationship))
				{
                    //nodeRelationship = (HierarchyInfo.NodeRelationship )hi.ParentChildRelationship[hjp.NewParentRID];
					nodeRelationship.AddChild(hjp.Key);
					if (hjp.LevelType == eHierarchyLevelType.Color ||
						hjp.LevelType == eHierarchyLevelType.Size)
					{
						nodeRelationship.childrenBuilt = true;
					}
					hi.ParentChildRelationship[hjp.NewParentRID] = nodeRelationship;
				}
				else
				{
					nodeRelationship = new HierarchyInfo.NodeRelationship();
					nodeRelationship.AddChild(hjp.Key);
					hi.ParentChildRelationship.Add(hjp.NewParentRID, nodeRelationship);
				}
								

				//  update child record with parent
                //if (hi.ParentChildRelationship.Contains(hjp.Key))
                if (hi.ParentChildRelationship.TryGetValue(hjp.Key, out child))
				{
                    //child = (HierarchyInfo.NodeRelationship)hi.ParentChildRelationship[hjp.Key];
					//					child.parentRID = hjp.NewParentRID;
					if (!child.ParentsContains(hjp.NewParentRID))
					{
						child.AddParent(hjp.NewParentRID);
					}
					hi.ParentChildRelationship[hjp.Key] = child;
				}
				else
				{
					child = new HierarchyInfo.NodeRelationship();
					// default childrenBuilt to true since new record and no children
					child.childrenBuilt = true;
					child.AddParent(hjp.NewParentRID);
					hi.ParentChildRelationship.Add(hjp.Key, child);
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}	
		}

		/// <summary>
		/// Moves a child node to a new parent
		/// </summary>
		/// <param name="hjp">An instance of the HierarchyJoinProfile which contains node relationship information</param>
		static private void UpdateRelationship(HierarchyJoinProfile hjp)
		{
			HierarchyInfo.NodeRelationship nodeRelationship;
			HierarchyInfo.NodeRelationship child;
			try
			{
				//  remove child from old parent
				HierarchyInfo hi = (HierarchyInfo)_hierarchiesByRID[hjp.OldHierarchyRID];

                if (hi.ParentChildRelationship.TryGetValue(hjp.OldParentRID, out nodeRelationship))
				{
                    //nodeRelationship = (HierarchyInfo.NodeRelationship )hi.ParentChildRelationship[hjp.OldParentRID];
					nodeRelationship.ChildrenRemove(hjp.Key);
					hi.ParentChildRelationship[hjp.OldParentRID] = nodeRelationship;
				}

				//  add child to new parent
				hi = (HierarchyInfo)_hierarchiesByRID[hjp.NewHierarchyRID];

                if (hi.ParentChildRelationship.TryGetValue(hjp.NewParentRID, out nodeRelationship))
				{
                    //nodeRelationship = (HierarchyInfo.NodeRelationship )hi.ParentChildRelationship[hjp.NewParentRID];
					nodeRelationship.AddChild(hjp.Key);
					if (hjp.LevelType == eHierarchyLevelType.Color ||
						hjp.LevelType == eHierarchyLevelType.Size)
					{
						nodeRelationship.childrenBuilt = true;
					}
					hi.ParentChildRelationship[hjp.NewParentRID] = nodeRelationship;
				}
				else
				{
					nodeRelationship = new HierarchyInfo.NodeRelationship();
					nodeRelationship.AddChild(hjp.Key);
					hi.ParentChildRelationship.Add(hjp.NewParentRID, nodeRelationship);
				}
								

				//  update child record with parent
                if (hi.ParentChildRelationship.TryGetValue(hjp.Key, out child))
				{
                    //child = (HierarchyInfo.NodeRelationship)hi.ParentChildRelationship[hjp.Key];
					//delete old parent
					if (child.ParentsContains(hjp.OldParentRID))
					{
						child.ParentsRemove(hjp.OldParentRID);
					}
					// add new parent
					if (!child.ParentsContains(hjp.NewParentRID))
					{
						child.AddParent(hjp.NewParentRID);
					}
					hi.ParentChildRelationship[hjp.Key] = child;
				}
				else
				{
					child = new HierarchyInfo.NodeRelationship();
					if (!child.ParentsContains(hjp.NewParentRID))
					{
						child.AddParent(hjp.NewParentRID);
					}
					hi.ParentChildRelationship.Add(hjp.Key, child);
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}	
		}

		/// <summary>
		/// Deletes a child from a parent in a hierarchy.
		/// </summary>
		/// <param name="hjp">An instance of the HierarchyJoinProfile which contains node relationship information</param>
		static private void DeleteRelationship(HierarchyJoinProfile hjp)
		{
			
			HierarchyInfo.NodeRelationship nodeRelationship;

			try
			{
				HierarchyInfo hi = (HierarchyInfo)_hierarchiesByRID[hjp.OldHierarchyRID];

                if (hi.ParentChildRelationship.TryGetValue(hjp.OldParentRID, out nodeRelationship))
				{
                    //nodeRelationship = (HierarchyInfo.NodeRelationship )hi.ParentChildRelationship[hjp.OldParentRID];
					nodeRelationship.ChildrenRemove(hjp.Key);
					hi.ParentChildRelationship[hjp.OldParentRID] = nodeRelationship;
				}

				//  remove child record 
                if (hi.ParentChildRelationship.TryGetValue(hjp.Key, out nodeRelationship))
				{
                    //nodeRelationship = (HierarchyInfo.NodeRelationship )hi.ParentChildRelationship[hjp.Key];
					nodeRelationship.ParentsRemove(hjp.OldParentRID);
					if (nodeRelationship.ParentsCount() == 0)
					{
						hi.ParentChildRelationship.Remove(hjp.Key);
					}
					hi.ParentChildRelationship[hjp.Key] = nodeRelationship; // MID Track #4389 - Justin Bolles - Hierarchy Copy Error
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the root nodes for all hierarchies system hierarchies and personal hierarchies for requested user.
		/// </summary>
		/// <param name="ownerRID">The user's record ID.</param>
		/// <param name="isHierarchyAdministrator">A switch identifying if the user is a hierarchy administrator</param>
		/// <param name="availableNodeList">A Hashtable containing nodes that are available for the user to view</param>
		/// <returns></returns>
        static public HierarchyNodeList GetRootNodes(int ownerRID, bool isHierarchyAdministrator,
            Dictionary<int, HierarchyNodeProfile> availableNodeList)
		{
			List<int> hierarchies;
            bool foundHierarchy = false;
			try
			{
				hierarchy_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
					int hierarchyRID;
					HierarchyInfo hi;
					HierarchyInfo.NodeRelationship child_nr;
					HierarchyNodeList rootNodes = new HierarchyNodeList(eProfileType.HierarchyNode);

					if (ownerRID == Include.AdministratorUserRID)
					{
                        foundHierarchy = _userHierarchies.TryGetValue(Include.GlobalUserRID, out hierarchies);
					}
					else
					{
                        foundHierarchy = _userHierarchies.TryGetValue(ownerRID, out hierarchies);
					}

                    if (!foundHierarchy)
                    {
                        hierarchies = new List<int>();
                    }

					foreach (KeyValuePair<int, HierarchyInfo>  hierarchy in _hierarchiesByRID)
					{
						hierarchyRID = hierarchy.Key;
						hi = hierarchy.Value;
						if ((hi.Owner == Include.GlobalUserRID && (isHierarchyAdministrator || availableNodeList.ContainsKey(hi.HierarchyRootNodeRID))) ||   //Issue 3806
							hi.Owner == ownerRID || hierarchies.Contains(hi.HierarchyRID))
						{
							HierarchyNodeProfile hnp = GetNodeData(hi.HierarchyRID, hi.HierarchyRootNodeRID, true);
							child_nr = hi.GetChildRelationship(hnp.Key);
							HierarchyInfo homehi = GetHierarchyInfoByRID(hnp.HomeHierarchyRID, true);

							hnp.HasChildren = child_nr.ChildrenCount() > 0;

							HierarchyLevelInfo nextLevel = null;
							if (homehi.HierarchyType == eHierarchyType.organizational)
							{
								if (hnp.HomeHierarchyLevel < homehi.HierarchyLevels.Count)
								{
									nextLevel = (HierarchyLevelInfo)homehi.HierarchyLevels[hnp.HomeHierarchyLevel + 1];
								}

								if (nextLevel != null &&
									nextLevel.LevelDisplayOption == eHierarchyDisplayOptions.DoNotDisplay)  // Do not show level on the explorer
								{
									hnp.DisplayChildren = false;
								}
								else
								{
									hnp.DisplayChildren = true;
								}
							}
							else
							{
								hnp.DisplayChildren = true;
							}

                            // Begin TT#3630 - JSmith - Delete My Hierarchy
                            //rootNodes.Add(hnp);
                            if (!hnp.DeleteNode)
                            {
                                rootNodes.Add(hnp);
                            }
                            // End TT#3630 - JSmith - Delete My Hierarchy
						}
					}

					return rootNodes;
				}        
				finally
				{
					// Ensure that the lock is released.
					hierarchy_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetRootNodes reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetRootNodes reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the root nodes for all hierarchies system hierarchies and personal hierarchies for requested user.
		/// </summary>
		/// <param name="ownerRID">The user's record ID.</param>
		/// <param name="isHierarchyAdministrator">A switch identifying if the user is a hierarchy administrator</param>
		/// <param name="availableNodeList">A Hashtable containing nodes that are available for the user to view</param>
		/// <param name="aHierarchyNodeType">The type of root nodes to retrieve</param>
		/// <returns></returns>
        // Begin Track #5005 - JSmith - Explorer Organization
        //static public HierarchyNodeList GetRootNodes(int ownerRID, bool isHierarchyAdministrator,
        //    Hashtable availableNodeList, eHierarchyNodeType aHierarchyNodeType)
        static public HierarchyNodeList GetRootNodes(int ownerRID, bool isHierarchyAdministrator,
            Dictionary<int, HierarchyNodeProfile> availableNodeList, eHierarchySelectType aHierarchyNodeType)
        // End Track #5005
		{
			try
			{
				hierarchy_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
					int hierarchyRID;
					HierarchyInfo hi;
					HierarchyInfo.NodeRelationship child_nr;
					HierarchyNodeList rootNodes = new HierarchyNodeList(eProfileType.HierarchyNode);

					foreach (KeyValuePair<int, HierarchyInfo>  hierarchy in _hierarchiesByRID)
					{
						hierarchyRID = hierarchy.Key;
						hi = hierarchy.Value;
						if ((hi.Owner == Include.GlobalUserRID && (isHierarchyAdministrator || availableNodeList.ContainsKey(hi.HierarchyRootNodeRID))) || 
							hi.Owner == ownerRID)
						{
							HierarchyNodeProfile hnp = GetNodeData(hi.HierarchyRID, hi.HierarchyRootNodeRID, false);
							child_nr = hi.GetChildRelationship(hnp.Key);
							HierarchyInfo homehi = GetHierarchyInfoByRID(hnp.HomeHierarchyRID, true);

							hnp.HasChildren = child_nr.ChildrenCount() > 0;

							HierarchyLevelInfo nextLevel = null;
							if (homehi.HierarchyType == eHierarchyType.organizational)
							{
								if (hnp.HomeHierarchyLevel < homehi.HierarchyLevels.Count)
								{
									nextLevel = (HierarchyLevelInfo)homehi.HierarchyLevels[hnp.HomeHierarchyLevel + 1];
								}

								if (nextLevel != null &&
									nextLevel.LevelDisplayOption == eHierarchyDisplayOptions.DoNotDisplay)  // Do not show level on the explorer
								{
									hnp.DisplayChildren = false;
								}
								else
								{
									hnp.DisplayChildren = true;
								}
							}
							else
							{
								hnp.DisplayChildren = true;
							}
							bool selectNode = false;
							switch (aHierarchyNodeType)
							{
                                // Begin Track #5005 - JSmith - Explorer Organization
                                //case eHierarchyNodeType.MyHierarchyRoot:
                                case eHierarchySelectType.MyHierarchyRoot:
                                // End Track #5005
									if (homehi.HierarchyType == eHierarchyType.alternate &&
										hi.Owner == ownerRID)
									{
										selectNode = true;
									}
									break;
                                // Begin Track #5005 - JSmith - Explorer Organization
                                //case eHierarchyNodeType.AlternateHierarchyRoot:
                                case eHierarchySelectType.AlternateHierarchyRoot:
                                // End Track #5005
									if (homehi.HierarchyType == eHierarchyType.alternate &&
										hi.Owner == Include.GlobalUserRID)
									{
										selectNode = true;
									}
									break;
                                // Begin Track #5005 - JSmith - Explorer Organization
                                //case eHierarchyNodeType.OrganizationalHierarchyRoot:
                                case eHierarchySelectType.OrganizationalHierarchyRoot:
                                // End Track #5005
									if (homehi.HierarchyType == eHierarchyType.organizational)
									{
										selectNode = true;
									}
									break;
							}

							if (selectNode)
							{
								rootNodes.Add(hnp);
							}
						}
					}

					return rootNodes;
				}        
				finally
				{
					// Ensure that the lock is released.
					hierarchy_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetRootNodes reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetRootNodes reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the information for all the children of a parent in a hierarchy.
		/// </summary>
		/// <param name="parentNodeLevel">The relative level of the parent node in the hierarchy</param>
		/// <param name="currentHierarchyRID">The record id of the hierarchy where the children are currently found</param>
		/// <param name="homeHierarchyRID">The record id of the home hierarchy of the parent node</param>
		/// <param name="nodeRID">The record id of the parent</param>
		/// <param name="isHierarchyAdministrator">A switch identifying if the user is a hierarchy administrator</param>
		/// <param name="availableNodeList">A Hashtable containing nodes that are available for the user to view</param>
		/// <param name="nodeSecurityAssignments">An Hashtable containing node records where a users security is assigned</param>
		/// <param name="aNodeSelectType">The type of nodes to select</param>
		/// <returns></returns>
		/// <remarks>
		/// For performance reasons, NodeLevel will only be set for guest nodes if aChaseHierarchy is set to true
		/// </remarks>
		static public HierarchyNodeList GetHierarchyChildren(int parentNodeLevel, int currentHierarchyRID, 
			int homeHierarchyRID, int nodeRID, bool isHierarchyAdministrator,
            Dictionary<int, HierarchyNodeProfile> availableNodeList, Dictionary<int, HierarchyNodeSecurityProfile> nodeSecurityAssignments, bool aChaseHierarchy, eNodeSelectType aNodeSelectType,
            bool aAccessDenied)
		{
			try
			{
				NodeInfo ni;
				HierarchyInfo.NodeRelationship child_nr;
				HierarchyLevelInfo hli = null;
				HierarchyNodeList children = new HierarchyNodeList(eProfileType.HierarchyNode);
				HierarchyInfo hi = GetHierarchyInfoByRID(homeHierarchyRID, true);
				HierarchyInfo.NodeRelationship nr = hi.GetChildRelationship(nodeRID);
				ni = GetNodeInfoByRID(nodeRID, true);
				HierarchyInfo homehi;
				if (ni.HomeHierarchyRID == homeHierarchyRID)
				{
					homehi = hi;
				}
				else
				{
					homehi = GetHierarchyInfoByRID(ni.HomeHierarchyRID, true);
				}
				HierarchyInfo currentHierarchy;
				if (currentHierarchyRID == homeHierarchyRID)
				{
					currentHierarchy = hi;
				}
				else if (currentHierarchyRID == ni.HomeHierarchyRID)
				{
					currentHierarchy = homehi;
				}
				else
				{
					currentHierarchy = GetHierarchyInfoByRID(currentHierarchyRID, true);
				}
				if (homehi.HierarchyType == eHierarchyType.organizational)
				{
					if (ni.HomeHierarchyLevel == homehi.HierarchyLevels.Count)	// lowest level so no children
					{
						return children;
					}
					else
					{
						hli = (HierarchyLevelInfo)homehi.HierarchyLevels[ni.HomeHierarchyLevel + 1];
						if (hli.LevelType == eHierarchyLevelType.Style && nr.childrenBuilt == false)
						{
							BuildStyles(homeHierarchyRID, nodeRID, true);
							nr.childrenBuilt = true;
							hi.ParentChildRelationship[nodeRID] = nr;
						}
						else
							if (hli.LevelType == eHierarchyLevelType.Color && nr.childrenBuilt == false)
						{
							BuildColorsForStyle(homeHierarchyRID, nodeRID);
							nr.childrenBuilt = true;
							hi.ParentChildRelationship[nodeRID] = nr;
						}
						else
							if (hli.LevelType == eHierarchyLevelType.Size && nr.childrenBuilt == false)
						{
							BuildSizesForColor(homeHierarchyRID, nodeRID);
							nr.childrenBuilt = true;
							hi.ParentChildRelationship[nodeRID] = nr;
						}
					}
				}

				bool addChild = false;

				if (nr.ChildrenCount() > 0)
				{
					foreach (int childRID in nr.children)
					{
						//Begin Track #5378 - color and size not qualified
//						HierarchyNodeProfile hnp = GetNodeData(homeHierarchyRID, childRID, aChaseHierarchy);
						HierarchyNodeProfile hnp = GetNodeData(homeHierarchyRID, childRID, aChaseHierarchy, currentHierarchy.HierarchyType == eHierarchyType.alternate);
						//End Track #5378
						switch (aNodeSelectType)
						{
							case eNodeSelectType.Connectors:
								if (hnp.Purpose != ePurpose.Connector)
								{
									continue;
								}
								break;
							case eNodeSelectType.NoVirtual:
								if (hnp.IsVirtual)
								{
									continue;
								}
								break;
							case eNodeSelectType.VirtualOnly:
								if (!hnp.IsVirtual)
								{
									continue;
								}
								break;
						}
						addChild = false;
						if (isHierarchyAdministrator || availableNodeList.ContainsKey(childRID))
						{
							addChild = true;
						}
						else
						{
                            //Begin Track #5279 - JSmith - Showing nodes with access denied
                            // if access isn't denied to parent and child not a guest, assume you can use the parent's security
                            if (!aAccessDenied &&
                                currentHierarchy.HierarchyRID == hnp.HomeHierarchyRID)
                            {
                                HierarchyNodeSecurityProfile hnsp = null;
                                if (nodeSecurityAssignments.ContainsKey(childRID))
                                {
                                    hnsp = (HierarchyNodeSecurityProfile)nodeSecurityAssignments[childRID];
                                }
                                if (hnsp == null ||		// assume if can see parent then can see child
                                    hnsp.AllowView ||
                                    hnsp.AllowView)
                                {
                                    addChild = true;
                                }
                            }
                            else
                            {
                                // see if ancestors in home hierarchy in security list
                                NodeAncestorList nal = GetNodeAncestorList(childRID, hnp.HomeHierarchyRID);
                                foreach (NodeAncestorProfile nap in nal)
                                {
                                    if (nodeSecurityAssignments.ContainsKey(nap.Key))
                                    {
                                        addChild = true;
                                        break;
                                    }
                                }

                            }
                            //End Track #5279

							if (!addChild && currentHierarchyRID != homeHierarchyRID)
							{
								// see if hierarchy root in security list
								if (currentHierarchy.HierarchyType == eHierarchyType.alternate)
								{
									if (nodeSecurityAssignments.ContainsKey(currentHierarchy.HierarchyRootNodeRID))							
									{
										addChild = true;
									}
								}
							}
						}

                        // Begin TT#3630 - JSmith - Delete My Hierarchy
                        if (hnp.DeleteNode)
                        {
                            addChild = false;
                        }
                        // End TT#3630 - JSmith - Delete My Hierarchy

						if (addChild)
						{
							// get color of level in home hierarchy
							if (homehi == null || homehi.HierarchyRID != hnp.HomeHierarchyRID)
							{
								homehi = GetHierarchyInfoByRID(hnp.HomeHierarchyRID, true);
							}
							// check home hierarchy to see if children have children to add expand symbol
							child_nr = homehi.GetChildRelationship(hnp.Key);


							hnp.HasChildren = child_nr.ChildrenCount() > 0;

							HierarchyLevelInfo nextLevel = null;
							if (homehi.HierarchyType == eHierarchyType.organizational)
							{
								if (hnp.HomeHierarchyLevel < homehi.HierarchyLevels.Count)
								{
									nextLevel = (HierarchyLevelInfo)homehi.HierarchyLevels[hnp.HomeHierarchyLevel + 1];
								}

								if (nextLevel != null &&
									nextLevel.LevelDisplayOption == eHierarchyDisplayOptions.DoNotDisplay)  // Do not show level on the explorer
								{
									hnp.DisplayChildren = false;
								}
								else
								{
									hnp.DisplayChildren = true;
								}
							}
							else
							{
								hnp.DisplayChildren = true;
							}

							children.Add(hnp);
						}
					}
				}

				return children;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the a list of record IDs of the ancestors of the node in the hierarchy. 
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aHierarchyRID">The record id of the hierarchy</param>
		/// <remarks>use Include.NoRID to reference the main heierarchy</remarks>
		/// <returns>An instance of the ArrayList class containing a list of ancestor nodes record IDs</returns>
		static public ArrayList GetNodeAncestorRIDs(int aNodeRID, int aHierarchyRID)
		{
			try
			{
				if (aHierarchyRID == Include.NoRID)
				{
					aHierarchyRID = ((HierarchyProfile)GetMainHierarchyData()).Key;
				}
				ArrayList ancestors = new ArrayList();
				HierarchyInfo hi = GetHierarchyInfoByRID(aHierarchyRID, true);
				ancestors.Add(aNodeRID);
					
				ArrayList parents = GetParentRIDs(aNodeRID, aHierarchyRID);
				foreach (int parentRID in parents)
				{
					if (parentRID > 0)
					{
						AddAncestorRID(ref ancestors, hi, parentRID);
					}
				}

				return ancestors;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the a list of record IDs for the ancestors of the node. 
		/// </summary>
		/// <param name="ancestors">The list of ancestors</param>
		/// <param name="hi">The hierarchy information</param>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <returns>An instance of the ArrayList class containing a list of ancestor nodes record IDs</returns>
		static private ArrayList AddAncestorRID(ref ArrayList ancestors, HierarchyInfo hi, int aNodeRID)
		{
			try
			{
				ancestors.Add(aNodeRID);

				ArrayList parents = GetParentRIDs(aNodeRID, hi.HierarchyRID);
				foreach (int parentRID in parents)
				{
					if (parentRID > 0)
					{
						AddAncestorRID(ref ancestors, hi, parentRID);
					}
				}

				return ancestors;
			}        
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static private ArrayList GetParentRIDs(int nodeRID, int hierarchyRID)
		{
			ArrayList parents = new ArrayList();
			try
			{
                // Begin TT#340 - RMatelic - Original enhancement not working; with the 6269 comment out below, the GetChildRelationship was taking a hard exception
                //  when the nodeRID was an altenate and the hierarchyRID is passed in as -1 
                if (hierarchyRID == Include.NoRID)
                {
                    HierarchyNodeProfile hnp = GetNodeData(nodeRID);
                    hierarchyRID = hnp.HomeHierarchyRID;
                }
                // End TT#340
				HierarchyInfo hi = GetHierarchyInfoByRID(hierarchyRID, true);
                // Begin TT#374 - JSmith - Adding new guests to alternate hierarchy fails with Orphaned hierarchy error
                //HierarchyInfo.NodeRelationship nr = hi.GetChildRelationship(nodeRID);

                //if (nr.ParentsCount() > 0)
                //{
                //    foreach (int parentRID in nr.parents)
                //    {
                //        parents.Add(parentRID);
                //    }
                //}
                if (hi.ChildRelationshipExists(nodeRID))
                {
                    HierarchyInfo.NodeRelationship nr = hi.GetChildRelationship(nodeRID);

                    if (nr.ParentsCount() > 0)
                    {
                        foreach (int parentRID in nr.parents)
                        {
                            parents.Add(parentRID);
                        }
                    }
                }
                // End TT#374

				return parents;
			}
			//Begin Track #6269 - JScott - Error logging on after auto upgrade
			//catch (RelationshipNotFoundException)
			//{
			//    return parents;
			//}
			//End Track #6269 - JScott - Error logging on after auto upgrade
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		static private ArrayList GetAllParentRIDs(int nodeRID, int hierarchyRID, NodeAncestorList aHomeAncestorList)
		{
			ArrayList parents = new ArrayList();
            List<int> parentList = new List<int>();
			try
			{
				HierarchyInfo hi = GetHierarchyInfoByRID(hierarchyRID, true);

				foreach (NodeAncestorProfile nap in aHomeAncestorList)
				{
                    // Begin TT#1589 - JSmith - Orphan Node Messages
                    //if (hi.ChildAnyRelationshipExists(nap.Key, aHomeAncestorList))
                    if (hi.ChildRelationshipExists(nap.Key))
                    // End TT#1589
					{
						HierarchyInfo.NodeRelationship nr = hi.GetChildRelationship(nap.Key);

						if (nr.ParentsCount() > 0)
						{
							foreach (int parentRID in nr.parents)
							{
                                if (!parentList.Contains(parentRID))
								{
									parents.Add(parentRID);
                                    parentList.Add(parentRID);
								}
							}
						}
					}
				}

				return parents;
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}
				throw;
			}
		}

		//End TT#155 - JScott - Add Size Curve info to Node Properties
		/// <summary>
		/// Gets the sequence of the highest guest level below the node. 
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <returns>An integer containing the sequence of the highest level</returns>
		static public int GetHighestGuestLevel(int aNodeRID)
		{
			try
			{
				int highestGuestLevel = int.MaxValue;
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				highestGuestLevel = mhd.GetHighestGuestLevel(aNodeRID);
				if (highestGuestLevel == -1)
				{
					return int.MaxValue;
				}
				else
				{
					return highestGuestLevel;
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        // Begin Track #5960 - JSmith - Get "No Low Levels Found" when using hierarchy level name 
        /// <summary>
        /// Gets a list of all guest levels 
        /// </summary>
        /// <param name="aNodeRID">The record id of the node</param>
        /// <returns>An ArrayList containing HierarchyLevelInfo objects for all guest levels</returns>
        static public ArrayList GetAllGuestLevels(int aNodeRID)
        {
            DataTable dt;
            ArrayList alGuests;
            HierarchyProfile hp;
            int homeLevel;
            try
            {
                alGuests = new ArrayList();
                hp = GetMainHierarchyData();
                MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                dt = mhd.GetAllGuestLevels(aNodeRID);
                foreach (DataRow dr in dt.Rows)
                {
                    homeLevel = Convert.ToInt32(dr["HOME_LEVEL"]);
                    alGuests.Add(hp.HierarchyLevels[homeLevel]);
                }
                return alGuests;
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }
        // End Track #5960

		//BEGIN TT#4650 - DOConnell - Changes do not hold
        /// <summary>
        /// Gets a count of all levels under an alternate hierarchy 
        /// </summary>
        /// <param name="aNodeRID">The record id of the node</param>
        /// <returns>An Int containing the count of all guest levels</returns>
        static public DataTable GetHierarchyDescendantLevels(int aNodeRID)
        {
            HierarchyProfile hp;
            int Levels = 0;
            try
            {
                MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                DataTable HierarchyDescendantLevels = mhd.GetHierarchyDescendantLevels(aNodeRID);

                return HierarchyDescendantLevels;
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }
		//END TT#4650 - DOConnell - Changes do not hold

		/// <summary>
		/// Recursively checks all descendant paths below the node to determine the highest guest level 
		/// </summary>
		/// <param name="aHighestGuestLevel">The highest guest level of all descendants</param>
        /// <param name="aOrigHierarchyInfo">The HierarchyInfo object of the original hierarchy</param>
        /// <param name="aCurrHierarchyInfo">The HierarchyInfo object of the current</param>
        /// <param name="aNodeInfo">The NodeInfo object that contains information about the node</param>
		/// <returns>The highest level sequence of the guests below the node</returns>
		static private int CheckNextLevel(ref int aHighestGuestLevel, HierarchyInfo aOrigHierarchyInfo, HierarchyInfo aCurrHierarchyInfo, NodeInfo aNodeInfo)
		{
			try
			{
				if (aNodeInfo.HomeHierarchyRID != aOrigHierarchyInfo.HierarchyRID && // if different hierarchy, check the level
					aCurrHierarchyInfo.HierarchyType == eHierarchyType.organizational) // and organizational, check the level
				{
					if (aNodeInfo.HomeHierarchyLevel < aHighestGuestLevel)	
					{
						aHighestGuestLevel = aNodeInfo.HomeHierarchyLevel;
					}
				}
				else  // otherwise, go to the next level
				{
					HierarchyInfo.NodeRelationship nr = aCurrHierarchyInfo.GetChildRelationship(aNodeInfo.NodeRID);
					if (nr.ChildrenCount() > 0)
					{
						foreach (int childRID in nr.children)  //  check the children
						{
							NodeInfo ni = GetNodeInfoByRID(childRID, false);
							if (ni.HomeHierarchyRID != aCurrHierarchyInfo.HierarchyRID)
							{
								aCurrHierarchyInfo = GetHierarchyInfoByRID(ni.HomeHierarchyRID, true);
							}
							CheckNextLevel(ref aHighestGuestLevel, aOrigHierarchyInfo, aCurrHierarchyInfo, ni);
						}
					}
				}
				return aHighestGuestLevel;
			}        
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the count of the largest branch below the node in the hierarchy
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
        /// <param name="aHomeHierarchyOnly">A flag indicating that only the home hierarchy should be searched</param>
		/// <returns>An integer containing the longest branch size</returns>
        // Begin Track #5960 - JSmith - Get "No Low Levels Found" when using hierarchy level name
        //static public int GetLongestBranch(int aNodeRID)
        static public int GetLongestBranch(int aNodeRID, bool aHomeHierarchyOnly)
        // End Track #5960
		{
			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                // Begin Track #5960 - JSmith - Get "No Low Levels Found" when using hierarchy level name
                //return mhd.GetBranchSize(aNodeRID);
                return mhd.GetBranchSize(aNodeRID, aHomeHierarchyOnly);
                // End Track #5960
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Recursively checks all descendant paths below the node to determine the highest guest level 
		/// </summary>
		/// <param name="aLargestBranchSize">The largest branch size</param>
		/// <param name="aBranchSize">The size of the current branch</param>
		/// <param name="aNodeInfo">The current node</param>
		/// <param name="aHierarchyInfo">The HierarchyInfo object of the home hierarchy of the node</param>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <returns>An integer containing the maximum number of levels between the node and the first guest level</returns>
		static private void DetermineBranchSize(ref int aLargestBranchSize, int aBranchSize, NodeInfo aNodeInfo, HierarchyInfo aHierarchyInfo, int aNodeRID)
		{
			try
			{
				NodeInfo ni = GetNodeInfoByRID(aNodeRID, false);
				
				if (ni.HomeHierarchyRID != aHierarchyInfo.HierarchyRID) // change hierarchies
				{
					HierarchyInfo hi = GetHierarchyInfoByRID(ni.HomeHierarchyRID, true);
					// if main hierarchy, estimate by number of levels rather than add.  Close enough
					if (hi.HierarchyType == eHierarchyType.organizational) 
					{
						// do not add 1 since the first guest level has already been acounted for
						int mainHierarchyLevels = hi.HierarchyLevels.Count - ni.HomeHierarchyLevel;
						aBranchSize += mainHierarchyLevels;
					}
					else  // otherwise switch hierarchies
					{
						DetermineBranchSize(ref aLargestBranchSize, aBranchSize, ni, hi, ni.NodeRID);
					}
				}
				else  // go down a level
				{
					HierarchyInfo.NodeRelationship nr = aHierarchyInfo.GetChildRelationship(aNodeRID);
					if (nr.ChildrenCount() > 0)
					{
						++aBranchSize;
						foreach (int childRID in nr.children)  //  check the children
						{
							DetermineBranchSize(ref aLargestBranchSize, aBranchSize, aNodeInfo, aHierarchyInfo, childRID);
						}
					}
				}

				if (aBranchSize > aLargestBranchSize)
				{
					aLargestBranchSize = aBranchSize;
				}
			}        
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        //  BEGIN TT#1572 - GRT - Urban Alternate Hierarchy
        /// <summary>
        /// Gets the a list of NodeAncestorProfiles which contain information about
        /// the ancestors of the node. 
        /// </summary>
        /// <param name="nodeRID">The record id of the node</param>
        /// <param name="hierarchyRID">The record id of the hierarchy</param>
        /// <param name="UseApplyFrom">This paramater identifies whether this request came SizeCurve</param>
        /// <remarks>Includes node in list</remarks>
        /// <returns>An instance of the NodeAncestorList class containing a list of ancestor nodes</returns>
        static public NodeAncestorList GetNodeAncestorList(int nodeRID, int hierarchyRID, bool UseApplyFrom)
        {
            try
            {
                //  if this is for size curve then we want the parameter to be false so that it doesn't apply the hierarchy
                return GetNodeAncestorList(nodeRID, hierarchyRID, eHierarchySearchType.HomeHierarchyOnly, UseApplyFrom);
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }
        //  END TT#1572 - GRT - Urban Alternate Hierarchy

		/// <summary>
		/// Gets the a list of NodeAncestorProfiles which contain information about
		/// the ancestors of the node. 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <remarks>Includes node in list</remarks>
		/// <returns>An instance of the NodeAncestorList class containing a list of ancestor nodes</returns>
		static public  NodeAncestorList GetNodeAncestorList(int nodeRID, int hierarchyRID)
		{
			try
			{
				return GetNodeAncestorList(nodeRID, hierarchyRID, eHierarchySearchType.HomeHierarchyOnly, true);
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the a list of NodeAncestorProfiles which contain information about
		/// the ancestors of the node. 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="aHierarchySearchType">The type of hierarchies to search for ancestors</param>
		/// <remarks>Includes node in list</remarks>
		/// <returns>An instance of the NodeAncestorList class containing a list of ancestor nodes</returns>
		static public NodeAncestorList GetNodeAncestorList(int nodeRID, int hierarchyRID, eHierarchySearchType aHierarchySearchType, bool UseApplyFrom)
		{
			try
			{
				NodeAncestorProfile nap = null;
				NodeAncestorList ancestors = new NodeAncestorList(eProfileType.HierarchyNodeAncestor);
				NodeInfo ni = GetNodeInfoByRID(nodeRID, false);

                //  Begin TT#1549 - Urban Hierarchy - GRT
                //      if the Hierarchy Type is Alternate and Apply From is True
                if ((GetHierarchyType(hierarchyRID) == eHierarchyType.alternate) && (UseApplyFrom))
                {
                    MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                    DataTable dt = mhd.GetAncestors(nodeRID, hierarchyRID, aHierarchySearchType, true);
                    foreach (DataRow dr in dt.Rows)
                    {
                        nap = new NodeAncestorProfile(Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture));
                        nap.HomeHierarchyRID = Convert.ToInt32(dr["HOME_PH_RID"], CultureInfo.CurrentUICulture);
                        nap.HomeHierarchyLevel = Convert.ToInt32(dr["HOME_LEVEL"], CultureInfo.CurrentUICulture);
                        ancestors.Add(nap);
                    }                
                }
                // End TT#1549 - Urban Hierarchy - GRT
                else
                {
                    // Begin Track #5277 - JSmith - Reclass not rolling alternate hierarchies
                    if (aHierarchySearchType == eHierarchySearchType.AllHierarchies ||
                        aHierarchySearchType == eHierarchySearchType.AlternateHierarchiesOnly)
                    {
                        MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                        // override search type if not home hierarchy
                        if (ni.HomeHierarchyRID != hierarchyRID &&
                            aHierarchySearchType == eHierarchySearchType.HomeHierarchyOnly)
                        {
                            aHierarchySearchType = eHierarchySearchType.SpecificHierarchy;
                        }
                        DataTable dt = mhd.GetAncestors(nodeRID, hierarchyRID, aHierarchySearchType, false);
                        foreach (DataRow dr in dt.Rows)
                        {
                            nap = new NodeAncestorProfile(Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture));
                            nap.HomeHierarchyRID = Convert.ToInt32(dr["HOME_PH_RID"], CultureInfo.CurrentUICulture);
                            nap.HomeHierarchyLevel = Convert.ToInt32(dr["HOME_LEVEL"], CultureInfo.CurrentUICulture);
                            ancestors.Add(nap);
                        }
                    }
                    else
                    {
                        // End Track #5277
                        HierarchyInfo hi = GetHierarchyInfoByRID(hierarchyRID, true);
                        nap = new NodeAncestorProfile(nodeRID);
                        nap.HomeHierarchyRID = ni.HomeHierarchyRID;
                        nap.HomeHierarchyLevel = ni.HomeHierarchyLevel;
                        ancestors.Add(nap);
                        // Begin TT#303-MD - JSmith - Hierarchy Service crash when select over 20 Headers for Style Review
                        if (hi.HierarchyRID == Include.NoRID)
                        {
                            if (Audit != null)
                            {
                                Audit.Add_Msg(eMIDMessageLevel.Warning, "Orphaned Hierarchy Node found: HN_RID = " + nodeRID, System.Reflection.MethodBase.GetCurrentMethod().Name);
                            }
                        }
                        else
                        {
                        // End TT#303-MD - JSmith - Hierarchy Service crash when select over 20 Headers for Style Review
                            ArrayList parents = GetParentRIDs(nodeRID, hi.HierarchyRID);
                            // if there are no parents in the alternate hierarchy, check the home path to
                            // see if its home ancestor is in alternate
                            if (parents.Count == 0 &&
                                hi.HierarchyType == eHierarchyType.alternate &&
                                ni.HomeHierarchyRID != hierarchyRID)
                            {
                                parents = GetParentRIDs(nodeRID, ni.HomeHierarchyRID);
                            }
                            foreach (int parentRID in parents)
                            {
                                if (parentRID > 0)
                                {
                                    AddAncestor(ref ancestors, hi, parentRID);
                                }
                            }

                            // make sure last node is in the alternate
                            // otherwise, clear all ancestors
                            if (ancestors.Count > 0 &&
                                hi.HierarchyType == eHierarchyType.alternate &&
                                ni.HomeHierarchyRID != hierarchyRID)
                            {
                                nap = (NodeAncestorProfile)ancestors[ancestors.Count - 1];
                                if (nap.HomeHierarchyRID != hi.HierarchyRID)
                                {
                                    nap = (NodeAncestorProfile)ancestors[0];
                                    ancestors.Clear();
                                    ancestors.Add(nap);
                                }
                            }
                            // Begin Track #5277 - JSmith - Reclass not rolling alternate hierarchies
                        }
                    // Begin TT#303-MD - JSmith - Hierarchy Service crash when select over 20 Headers for Style Review
                    }
                    // End TT#303-MD - JSmith - Hierarchy Service crash when select over 20 Headers for Style Review
                    // End Track #5277
                }
//				Debug.WriteLine("Ancestors for " + nodeRID.ToString());
//				for (int i=0; i<ancestors.Count; i++)
//				{
//					nap = (NodeAncestorProfile)ancestors[i];
//					NodeAncestorProfile nap2 = (NodeAncestorProfile)ancestors2[i];
//					Debug.WriteLine("  Ancestor " + nap.Key.ToString() + ":" + nap2.Key.ToString());
//					Debug.WriteLine("  Home " + nap.HomeHierarchyRID.ToString() + ":" + nap2.HomeHierarchyRID.ToString());
//					Debug.WriteLine("  Level " + nap.HomeHierarchyLevel.ToString() + ":" + nap2.HomeHierarchyLevel.ToString());
//					if (nap.Key != nap2.Key ||
//						nap.HomeHierarchyRID != nap2.HomeHierarchyRID ||
//						nap.HomeHierarchyLevel != nap2.HomeHierarchyLevel)
//					{
//						int xxx = 0;
//					}
//				}

				return ancestors;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}
		
		/// <summary>
		/// Gets the a list of NodeAncestorProfiles which contain information about
		/// the ancestors of the node. 
		/// </summary>
		/// <param name="ancestors">The list of ancestors</param>
		/// <param name="hi">The hierarchy information</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <returns>An instance of the NodeAncestorList class containing a list of ancestor nodes</returns>
		static private NodeAncestorList AddAncestor(ref NodeAncestorList ancestors, HierarchyInfo hi, int nodeRID)
		{
			try
			{
				if (!ancestors.Contains(nodeRID))
				{
					NodeAncestorProfile nap = new NodeAncestorProfile(nodeRID);
					NodeInfo ni = GetNodeInfoByRID(nodeRID, false);
					nap.HomeHierarchyRID = ni.HomeHierarchyRID;
					nap.HomeHierarchyLevel = ni.HomeHierarchyLevel;
					ancestors.Add(nap);

					ArrayList parents = GetParentRIDs(nodeRID, hi.HierarchyRID);
					// if there are no parents in the alternate hierarchy, check the home path to
					// see if its home ancestor is in alternate
					if (parents.Count == 0 &&
						hi.HierarchyType == eHierarchyType.alternate &&
						ni.HomeHierarchyRID != hi.HierarchyRID)
					{
						parents = GetParentRIDs(nodeRID, ni.HomeHierarchyRID);
					}
					foreach (int parentRID in parents)
					{
						if (parentRID > 0)
						{
							AddAncestor(ref ancestors, hi, parentRID);
						}
					}
				}

				return ancestors;
			}        
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the a list of all NodeAncestorList ancestor lists for each reference
		/// of the node in all hierarchies. 
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <returns>An instance of an ArrayList containing a NodeAncestorList class for each reference.</returns>
		static public ArrayList GetAllNodeAncestorLists(int aNodeRID)
		{
			try
			{
				ArrayList ancestorLists = new ArrayList();
                //Hashtable ancestorPaths = new Hashtable();
                Dictionary<int, NodeAncestorList> ancestorPaths = new Dictionary<int, NodeAncestorList>();
				bool errorFound = false;
				int[] hierarchyRIDs;
				// get list of all hierarchy RIDs to loop through
				try
				{
					hierarchyRIDs = new int[_hierarchiesByRID.Count];
					hierarchy_rwl.AcquireReaderLock(ReaderLockTimeOut);
					try
					{
						int i = 0;
						foreach (HierarchyInfo hi in _hierarchiesByRID.Values)
						{
							hierarchyRIDs[i] = hi.HierarchyRID;
							++i;
						}
					}
					catch (Exception ex)
					{
                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                        if (Audit != null)
                        {
                            Audit.Log_Exception(ex);
                        }
                        // End TT#189
						errorFound = true;
					}
					finally
					{
						// Ensure that the lock is released.
						hierarchy_rwl.ReleaseReaderLock();
					}
				}
				catch (ApplicationException ex)
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					// The reader lock request timed out.
                    EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetAllNodeAncestorLists reader lock has timed out", EventLogEntryType.Error);
                    throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:GetAllNodeAncestorLists reader lock has timed out");
				}
				catch (Exception ex)
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}

				if (!errorFound)
				{
					// loop through each hierarchy to find if exists in hierarchy and get parents
					try
					{
						NodeInfo ni = GetNodeInfoByRID(aNodeRID, false);
						NodeAncestorProfile nap = new NodeAncestorProfile(aNodeRID);
						nap.HomeHierarchyRID = ni.HomeHierarchyRID;
						nap.HomeHierarchyLevel = ni.HomeHierarchyLevel;
						
						foreach (int hierarchyRID in hierarchyRIDs)
						{
							ArrayList parents = GetParentRIDs(aNodeRID, hierarchyRID);
							foreach (int parentRID in parents)
                            {

                                NodeAncestorList ancestors = new NodeAncestorList(eProfileType.HierarchyNodeAncestor);
                                ancestors.Add(nap);
                                int pathID = ancestorPaths.Count + 1;
                                ancestorPaths.Add(pathID, ancestors);
                                if (parentRID != 0)
                                {
                                    AddAllAncestors(ref ancestorPaths, pathID, parentRID);
                                }
                            }
						}
					}
					catch (Exception ex)
					{
                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                        if (Audit != null)
                        {
                            Audit.Log_Exception(ex);
                        }
                        // End TT#189
						errorFound = true;
					}
				}
				foreach (NodeAncestorList ancestorList in ancestorPaths.Values)
				{
					ancestorLists.Add(ancestorList);
				}

				return ancestorLists;
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        // BEGIN TT#1572 - GRT - added overload to allow for bool var
        /// <summary>
        /// Requests the session get the a list of all NodeAncestorList ancestor lists for each reference
        /// of the node in all hierarchies. 
        /// </summary>
        /// <param name="aNodeRID">The record id of the node</param>
        /// <returns>An instance of an ArrayList containing a NodeAncestorList class for each reference.</returns>
        static public SortedList GetAllNodeAncestors(int aNodeRID)
        {
            return GetAllNodeAncestors(aNodeRID, true);
        }
        // END TT#1572 - GRT - added overload to allow for bool var

		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		/// <summary>
		/// Requests the session get the a list of all NodeAncestorList ancestor lists for each reference
		/// of the node in all hierarchies. 
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <returns>An instance of an ArrayList containing a NodeAncestorList class for each reference.</returns>
        static public SortedList GetAllNodeAncestors(int aNodeRID, bool UseApplyFrom)
		{
            Dictionary<int, NodeAncestorList> ancestorPaths;
			bool errorFound;
			int[] hierarchyRIDs;
			int i;
			NodeInfo ni;
			NodeAncestorList nal;
			ArrayList parents;
			NodeAncestorList ancestors;
			SortedList outList;
			IDictionaryEnumerator iEnum;

			try
			{
				errorFound = false;

				try
				{
					hierarchyRIDs = new int[_hierarchiesByRID.Count];
					hierarchy_rwl.AcquireReaderLock(ReaderLockTimeOut);

					try
					{
						i = 0;

						foreach (HierarchyInfo hi in _hierarchiesByRID.Values)
						{
							hierarchyRIDs[i] = hi.HierarchyRID;
							++i;
						}
					}
					catch (Exception ex)
					{
						if (Audit != null)
						{
							Audit.Log_Exception(ex);
						}

						errorFound = true;
					}
					finally
					{
						hierarchy_rwl.ReleaseReaderLock();
					}
				}
				catch (ApplicationException ex)
				{
					if (Audit != null)
					{
						Audit.Log_Exception(ex);
					}

                    EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetAllNodeAncestorLists reader lock has timed out", EventLogEntryType.Error);
                    throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:GetAllNodeAncestorLists reader lock has timed out");
				}
				catch (Exception ex)
				{
					if (Audit != null)
					{
						Audit.Log_Exception(ex);
					}

					throw;
				}

                ancestorPaths = new Dictionary<int, NodeAncestorList>();

				if (!errorFound)
				{
					try
					{
						ni = GetNodeInfoByRID(aNodeRID, false);
                        nal = GetNodeAncestorList(aNodeRID, ni.HomeHierarchyRID, UseApplyFrom);
					
						foreach (int hierarchyRID in hierarchyRIDs)
						{
							parents = GetAllParentRIDs(aNodeRID, hierarchyRID, nal);

							if (parents.Count > 0)
							{
								ancestors = new NodeAncestorList(eProfileType.HierarchyNodeAncestor);
								ancestorPaths.Add(hierarchyRID, ancestors);

								foreach (int parentRID in parents)
								{
									if (parentRID != 0)
									{
										AddAllAncestors(ref ancestorPaths, hierarchyRID, parentRID);
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						if (Audit != null)
						{
							Audit.Log_Exception(ex);
						}

						errorFound = true;
					}
				}

				outList = new SortedList();
				iEnum = ancestorPaths.GetEnumerator();

				while (iEnum.MoveNext())
				{
					outList.Add(iEnum.Key, iEnum.Value);
				}

				return outList;
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		static public ProfileList GetAncestorPath(int aNodeRID, int aToHierarchyRID, eLowLevelsType aToLowLevelType, int aToLevelRID, int aToOffset)
		{
			try
			{
				return RecurseAncestorPath(aNodeRID, aToHierarchyRID, aToLowLevelType, aToLevelRID, aToOffset, null);
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		static private ProfileList RecurseAncestorPath(int aNodeRID, int aToHierarchyRID, eLowLevelsType aToLowLevelType, int aToLevelRID, int aToOffset, ProfileList aNodeList)
		{
			ProfileList nodeList;
			ArrayList parentList;
			HierarchyNodeProfile nodeProf;
			ProfileList retNodeList;

			try
			{
				if (aNodeList != null)
				{
					nodeList = (ProfileList)aNodeList.Clone();
				}
				else
				{
					nodeList = new ProfileList(eProfileType.HierarchyNode);
				}

				nodeProf = GetNodeData(aNodeRID, false, true);
				nodeList.Add(nodeProf);

				if (nodeProf.HomeHierarchyRID == aToHierarchyRID &&
						((nodeProf.HomeHierarchyType == eHierarchyType.organizational &&
						aToLowLevelType == eLowLevelsType.HierarchyLevel &&
						nodeProf.HomeHierarchyLevel == aToLevelRID) ||
						(nodeProf.HomeHierarchyType == eHierarchyType.alternate &&
						aToLowLevelType == eLowLevelsType.LevelOffset &&
						nodeProf.HomeHierarchyLevel == aToOffset)))
				{
					return nodeList;
				}
				else
				{
					parentList = GetParentRIDs(aNodeRID, aToHierarchyRID);

					if (parentList.Count > 0)
					{
						foreach (int parentRID in parentList)
						{
							retNodeList = RecurseAncestorPath(parentRID, aToHierarchyRID, aToLowLevelType, aToLevelRID, aToOffset, nodeList);

							if (retNodeList != null)
							{
								return retNodeList;
							}
						}
					}
				}

				return null;
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		//End TT#155 - JScott - Add Size Curve info to Node Properties
		/// <summary>
		/// Gets the a list of NodeAncestorProfiles which contain information about
		/// the ancestors of the node. 
		/// </summary>
		/// <param name="aAncestorPaths">The list of all ancestor paths</param>
		/// <param name="aPathID">The ID for the current path</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <returns>A reference to the instance of the Hashtable class containing all NodeAncestorList</returns>
        static private void AddAllAncestors(ref Dictionary<int, NodeAncestorList> aAncestorPaths, int aPathID, int nodeRID)
		{
			try
			{
				NodeAncestorList ancestors = (NodeAncestorList)aAncestorPaths[aPathID];
				if (!ancestors.Contains(nodeRID))
				{
					NodeAncestorProfile nap = new NodeAncestorProfile(nodeRID);
					//Begin TT#427 - JScott - Size Curve Tab,>Tolerance Tab- Node-Properties -In the Highest Level drop down do not include Personal Hierarchies.
					//NodeInfo ni = GetNodeInfoByRID(nodeRID, false);
					//nap.HomeHierarchyRID = ni.HomeHierarchyRID;
					//nap.HomeHierarchyLevel = ni.HomeHierarchyLevel;
					HierarchyNodeProfile np = GetNodeData(nodeRID, false, false);
					nap.HomeHierarchyRID = np.HomeHierarchyRID;
					nap.HomeHierarchyLevel = np.HomeHierarchyLevel;
					nap.HomeHierarchyOwner = np.HomeHierarchyOwner;
					//End TT#427 - JScott - Size Curve Tab,>Tolerance Tab- Node-Properties -In the Highest Level drop down do not include Personal Hierarchies.
					ancestors.Add(nap);

					//Begin TT#427 - JScott - Size Curve Tab,>Tolerance Tab- Node-Properties -In the Highest Level drop down do not include Personal Hierarchies.
					//ArrayList parents = GetParentRIDs(nodeRID, ni.HomeHierarchyRID);
					ArrayList parents = GetParentRIDs(nodeRID, np.HomeHierarchyRID);
					//End TT#427 - JScott - Size Curve Tab,>Tolerance Tab- Node-Properties -In the Highest Level drop down do not include Personal Hierarchies.
					int parentCount = 0;
					int pathID = aPathID;
					foreach (int parentRID in parents)
					{
						if (parentRID > 0)
						{
							++parentCount;
							if (parentCount > 1)
							{
								NodeAncestorList newAncestors = (NodeAncestorList)ancestors.Clone();
								pathID = newAncestors.Count + 1;
								aAncestorPaths.Add(pathID, newAncestors);
							}
							AddAllAncestors(ref aAncestorPaths, pathID, parentRID);
						}
					}
				}

				//return ancestors;
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Adds or updates information about an eligibility model
		/// </summary>
		/// <param name="emp">Information about the eligibility model</param>
		static public void EligModelUpdate(EligModelProfile emp)
		{
			try
			{
				models_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					switch (emp.ModelChangeType)
					{
						case eChangeType.none: 
						{
							break;
						}
						case eChangeType.add: 
						{
							EligModelInfo emi = new EligModelInfo();
							emi.ModelRID = emp.Key;
							emi.ModelID = emp.ModelID;
							foreach (EligModelEntry eme in emp.ModelEntries)
							{
								EligModelEntryInfo modelEntry = new EligModelEntryInfo();
								modelEntry.EligModelEntryType = eme.EligModelEntryType;
								modelEntry.ModelEntrySeq = eme.ModelEntrySeq;
								modelEntry.DateRange = eme.DateRange;
								emi.ModelEntries.Add(modelEntry);
							}
							foreach (EligModelEntry eme in emp.SalesEligibilityEntries)
							{
								EligModelEntryInfo modelEntry = new EligModelEntryInfo(); 
								modelEntry.EligModelEntryType = eme.EligModelEntryType;
								modelEntry.ModelEntrySeq = eme.ModelEntrySeq;
								modelEntry.DateRange = eme.DateRange;
								emi.SalesEligibilityEntries.Add(modelEntry);
							}
							foreach (EligModelEntry eme in emp.PriorityShippingEntries)
							{
								EligModelEntryInfo modelEntry = new EligModelEntryInfo(); 
								modelEntry.EligModelEntryType = eme.EligModelEntryType;
								modelEntry.ModelEntrySeq = eme.ModelEntrySeq;
								modelEntry.DateRange = eme.DateRange;
								emi.PriorityShippingEntries.Add(modelEntry);
							}
							emi.UpdateDateTime = DateTime.Now;
							LoadEligibilityModelDates(emi);
							_eligModelsByRID.Add(emi.ModelRID, emi);
							_eligModelsByID.Add(emi.ModelID, emi.ModelRID);
							break;
						}
						case eChangeType.update: 
						{
							EligModelInfo emi = (EligModelInfo)_eligModelsByRID[emp.Key];
							_eligModelsByID.Remove(emi.ModelID);
							emi.ModelRID = emp.Key;
							emi.ModelID = emp.ModelID;
							emi.ModelEntries.Clear();
							foreach (EligModelEntry eme in emp.ModelEntries)
							{
								EligModelEntryInfo modelEntry = new EligModelEntryInfo();
								modelEntry.EligModelEntryType = eme.EligModelEntryType;
								modelEntry.ModelEntrySeq = eme.ModelEntrySeq;
								modelEntry.DateRange = eme.DateRange;
								emi.ModelEntries.Add(modelEntry);
							}
							emi.SalesEligibilityEntries.Clear();
							foreach (EligModelEntry eme in emp.SalesEligibilityEntries)
							{
								EligModelEntryInfo modelEntry = new EligModelEntryInfo(); 
								modelEntry.EligModelEntryType = eme.EligModelEntryType;
								modelEntry.ModelEntrySeq = eme.ModelEntrySeq;
								modelEntry.DateRange = eme.DateRange;
								emi.SalesEligibilityEntries.Add(modelEntry);
							}
							emi.PriorityShippingEntries.Clear();
							foreach (EligModelEntry eme in emp.PriorityShippingEntries)
							{
								EligModelEntryInfo modelEntry = new EligModelEntryInfo(); 
								modelEntry.EligModelEntryType = eme.EligModelEntryType;
								modelEntry.ModelEntrySeq = eme.ModelEntrySeq;
								modelEntry.DateRange = eme.DateRange;
								emi.PriorityShippingEntries.Add(modelEntry);
							}
							emi.UpdateDateTime = DateTime.Now;
							LoadEligibilityModelDates(emi);
							_eligModelsByRID[emp.Key] = emi;
							_eligModelsByID.Add(emi.ModelID, emi.ModelRID);
							break;
						}
						case eChangeType.delete: 
						{
							EligModelInfo emi = (EligModelInfo)_eligModelsByRID[emp.Key];
							_eligModelsByRID.Remove(emp.Key);
							_eligModelsByID.Remove(emi.ModelID);
							break;
						}
					}
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					EventLog.WriteEntry("MIDHierarchyService", "EligModelUpdate error:" + ex.Message, EventLogEntryType.Error);
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:EligModelUpdate writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:EligModelUpdate writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets all information associated with an eligibility model using the model name.
		/// </summary>
		/// <param name="ModelID">The id of the eligibility model</param>
		/// <returns>An instance of the EligModelProfile class containing eligibility model information</returns>
		static public EligModelProfile GetEligModelData(string ModelID)
		{
            int emRID;
			try
			{
				models_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
                    if (_eligModelsByID.TryGetValue(ModelID, out emRID))
					{
						return GetEligModelData(emRID);
					}
					else
					{
						EligModelProfile emp = new EligModelProfile(Include.NoRID);
						emp.Key = Include.NoRID;  // not found
						return emp;
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetEligModelData reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetEligModelData reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        static public EligModelProfile GetEligModelData(int ModelRID)
		{
			try
			{
                return GetEligModelData(ModelRID, null);
            }
            catch
            {
                throw;
            }
        }
        // Begin TT#2307 - JSmith - Incorrect Stock Values

		/// <summary>
		/// Gets the information about an eligibility model.
		/// </summary>
		/// <param name="ModelRID">The record id of the eligibility model</param>
		/// <returns>An instance of the EligModelProfile class containing eligibility model information</returns>
        // Begin TT#2307 - JSmith - Incorrect Stock Values
        //static public EligModelProfile GetEligModelData(int ModelRID)
        static public EligModelProfile GetEligModelData(int ModelRID, ProfileList aStoreList)
        // Begin TT#2307 - JSmith - Incorrect Stock Values
		{
			try
			{
                // Begin TT#2307 - JSmith - Incorrect Stock Values
                DateTime storeUpdateDate = DateTime.Now;
                // End TT#2307

				models_rwl.AcquireReaderLock(ReaderLockTimeOut);
				EligModelInfo emi;
				try
				{
                    if (!_eligModelsByRID.TryGetValue(ModelRID, out emi))
					{
						emi = new EligModelInfo();
						emi.ModelRID = Include.NoRID;
					}
					EligModelProfile emp = new EligModelProfile(emi.ModelRID);
					emp.ModelChangeType = eChangeType.none;
					emp.Key = emi.ModelRID;
					emp.ModelID = emi.ModelID;
					emp.UpdateDateTime = emi.UpdateDateTime;
					foreach (EligModelEntryInfo emei in emi.ModelEntries)
					{
						EligModelEntry modelEntry = new EligModelEntry(); 
						modelEntry.EligModelEntryType = emei.EligModelEntryType;
						modelEntry.ModelEntryChangeType = eChangeType.none;
						modelEntry.ModelEntrySeq = emei.ModelEntrySeq;
						modelEntry.DateRange = emei.DateRange;
						emp.ModelEntries.Add(modelEntry);
					}

                    // Begin TT#2307 - JSmith - Incorrect Stock Values
                    emp.ContainsDynamicDates = emi.ContainsDynamicDates;
                    emp.ContainsPlanDynamicDates = emi.ContainsPlanDynamicDates;
                    emp.ContainsReoccurringDates = emi.ContainsReoccurringDates;
                    emp.ContainsStoreDynamicDates = emi.ContainsStoreDynamicDates;
                    if (aStoreList != null &&
                        emi.ContainsStoreDynamicDates &&
                        !emi.ModelDateEntriesLoadedByStore)
                    {
                        LoadStockEligibilityModelDatesByStore(emi, aStoreList);
                    }
                    emp.NeedsRebuilt = emi.NeedsRebuilt;
                    // End TT#2307

					foreach (EligModelEntryInfo emei in emi.SalesEligibilityEntries)
					{
						EligModelEntry modelEntry = new EligModelEntry(); 
						modelEntry.EligModelEntryType = emei.EligModelEntryType;
						modelEntry.ModelEntryChangeType = eChangeType.none;
						modelEntry.ModelEntrySeq = emei.ModelEntrySeq;
						modelEntry.DateRange = emei.DateRange;
						emp.SalesEligibilityEntries.Add(modelEntry);
					}

                    // Begin TT#2307 - JSmith - Incorrect Stock Values
                    emp.SalesEligibilityContainsDynamicDates = emi.SalesEligibilityContainsDynamicDates;
                    emp.SalesEligibilityContainsPlanDynamicDates = emi.SalesEligibilityContainsPlanDynamicDates;
                    emp.SalesEligibilityContainsReoccurringDates = emi.SalesEligibilityContainsReoccurringDates;
                    emp.SalesEligibilityContainsStoreDynamicDates = emi.SalesEligibilityContainsStoreDynamicDates;
                    if (aStoreList != null &&
                        emi.SalesEligibilityContainsStoreDynamicDates &&
                        !emi.SalesEligibilityModelDateEntriesLoadedByStore)
                    {
                        LoadSalesEligibilityModelDatesByStore(emi, aStoreList);
                    }
                    emp.SalesEligibilityNeedsRebuilt = emi.SalesEligibilityNeedsRebuilt;
                    // End TT#2307

					foreach (EligModelEntryInfo emei in emi.PriorityShippingEntries)
					{
						EligModelEntry modelEntry = new EligModelEntry(); 
						modelEntry.EligModelEntryType = emei.EligModelEntryType;
						modelEntry.ModelEntryChangeType = eChangeType.none;
						modelEntry.ModelEntrySeq = emei.ModelEntrySeq;
						modelEntry.DateRange = emei.DateRange;
						emp.PriorityShippingEntries.Add(modelEntry);
					}

                    // Begin TT#2307 - JSmith - Incorrect Stock Values
                    emp.PriorityShippingContainsDynamicDates = emi.PriorityShippingContainsDynamicDates;
                    emp.PriorityShippingContainsPlanDynamicDates = emi.PriorityShippingContainsPlanDynamicDates;
                    emp.PriorityShippingContainsReoccurringDates = emi.PriorityShippingContainsReoccurringDates;
                    emp.PriorityShippingContainsStoreDynamicDates = emi.PriorityShippingContainsStoreDynamicDates;
                    if (aStoreList != null &&
                        emi.PriorityShippingContainsStoreDynamicDates &&
                        !emi.PriorityShippingModelDateEntriesLoadedByStore)
                    {
                        LoadPriorityShippingModelDatesByStore(emi, aStoreList);
                    }
                    emp.PriorityShippingNeedsRebuilt = emi.PriorityShippingNeedsRebuilt;
                    // End TT#2307

					emp.ModelDateEntries = (Hashtable)emi.ModelDateEntries.Clone();
					emp.SalesEligibilityModelDateEntries = (Hashtable)emi.SalesEligibilityModelDateEntries.Clone();
					emp.PriorityShippingModelDateEntries = (Hashtable)emi.PriorityShippingModelDateEntries.Clone();
					return emp;
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetEligModelData reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetEligModelData reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the date and time the eligibility model was last updated.
		/// </summary>
		/// <param name="ModelRID">The record id of the eligibility model</param>
		/// <returns>A string containing the date and time the model was last updated </returns>
		static public string GetEligModelUpdateDateString(int ModelRID)
		{
			try
			{
				models_rwl.AcquireReaderLock(ReaderLockTimeOut);
				EligModelInfo emi;
				try
				{
                    if (!_eligModelsByRID.TryGetValue(ModelRID, out emi))
					{
						emi = new EligModelInfo();
						emi.ModelRID = Include.NoRID;
					}
					
					return emi.UpdateDateTimeString;
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetEligModelUpdateDateString reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetEligModelUpdateDateString reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Populates the model DataTable with eligibility models information
		/// </summary>
		/// <returns></returns>
		static public ProfileList PopulateEligModels()
		{
			try
			{
				models_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
					ModelNameList modelList = new ModelNameList(eProfileType.ModelName);
					foreach (KeyValuePair<int, EligModelInfo>  eligModel in _eligModelsByRID)
					{
						EligModelInfo emi = (EligModelInfo)eligModel.Value;
						ModelName mn = new ModelName(emi.ModelRID);
						mn.ModelID = emi.ModelID;
						modelList.Add(mn);
					}
					if (modelList.Count == 0)
					{
						ModelName mn = new ModelName(Include.NoRID);
						mn.ModelID = " ";
						modelList.Add(mn);
					}
					modelList.ArrayList.Sort(new ModelNameSort());
					return modelList;
				}        
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:PopulateEligModels reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:PopulateEligModels reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Requests the date information associated with an eligibility model is loaded.
		/// </summary>
		/// <param name="emi">The eligibility model object</param>
		static private void LoadEligibilityModelDates(EligModelInfo emi)
		{
			try
			{
				models_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					//clear out any old date entries
					emi.ModelDateEntries.Clear();
					emi.SalesEligibilityModelDateEntries.Clear();
					emi.PriorityShippingModelDateEntries.Clear();
					emi.NeedsRebuilt = false;

					// load sales eligibility dates
					foreach(EligModelEntryInfo emei in emi.SalesEligibilityEntries)
					{
                        // Begin TT#2307 - JSmith - Incorrect Stock Values
                        //if (emei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
                        //{
                        //    emi.ContainsReoccurringDates = true;
                        //}
                        //if (emei.DateRange.DateRangeType == eCalendarRangeType.Dynamic)
                        //{
                        //    emi.ContainsDynamicDates = true;
                        //    if (emei.DateRange.RelativeTo == eDateRangeRelativeTo.StoreOpen)
                        //    {
                        //        emi.ContainsStoreDynamicDates = true;
                        //        emi.NeedsRebuilt = true;
                        //    }
                        //    if (emei.DateRange.RelativeTo == eDateRangeRelativeTo.Plan)
                        //    {
                        //        emi.ContainsPlanDynamicDates = true;
                        //        emi.NeedsRebuilt = true;
                        //    }
                        //}
                        if (emei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
                        {
                            emi.SalesEligibilityContainsReoccurringDates = true;
                        }
                        if (emei.DateRange.DateRangeType == eCalendarRangeType.Dynamic)
                        {
                            emi.SalesEligibilityContainsDynamicDates = true;
                            if (emei.DateRange.RelativeTo == eDateRangeRelativeTo.StoreOpen)
                            {
                                emi.SalesEligibilityContainsStoreDynamicDates = true;
                                emi.SalesEligibilityNeedsRebuilt = true;
                            }
                            if (emei.DateRange.RelativeTo == eDateRangeRelativeTo.Plan)
                            {
                                emi.SalesEligibilityContainsPlanDynamicDates = true;
                                emi.SalesEligibilityNeedsRebuilt = true;
                            }
                        }
                        // End TT#2307
				
						ProfileList weekProfileList = Calendar.GetWeekRange(emei.DateRange, null);
						if (emei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
								if (!emi.SalesEligibilityModelDateEntries.Contains(weekProfile.WeekInYear))
								{
									emi.SalesEligibilityModelDateEntries.Add(weekProfile.WeekInYear,null);
								}
							}
						}
						else
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
								if (!emi.SalesEligibilityModelDateEntries.Contains(weekProfile.Key))
								{
									emi.SalesEligibilityModelDateEntries.Add(weekProfile.Key, null);
								}
							}
						}
					}

					// load stock eligibility dates
					foreach(EligModelEntryInfo emei in emi.ModelEntries)
					{
						if (emei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
						{
							emi.ContainsReoccurringDates = true;
						}
						if (emei.DateRange.DateRangeType == eCalendarRangeType.Dynamic)
						{
							emi.ContainsDynamicDates = true;
							if (emei.DateRange.RelativeTo == eDateRangeRelativeTo.StoreOpen)
							{
								emi.ContainsStoreDynamicDates = true;
								emi.NeedsRebuilt = true;
							}
							if (emei.DateRange.RelativeTo == eDateRangeRelativeTo.Plan)
							{
								emi.ContainsPlanDynamicDates = true;
								emi.NeedsRebuilt = true;
							}
						}
				
						ProfileList weekProfileList = Calendar.GetWeekRange(emei.DateRange, null);
						if (emei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
								if (!emi.ModelDateEntries.Contains(weekProfile.WeekInYear))
								{
									emi.ModelDateEntries.Add(weekProfile.WeekInYear,null);
								}
							}
						}
						else
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
								if (!emi.ModelDateEntries.Contains(weekProfile.Key))
								{
									emi.ModelDateEntries.Add(weekProfile.Key, null);
								}
							}
						}
					}

					// load priority shipping dates
					foreach(EligModelEntryInfo emei in emi.PriorityShippingEntries)
					{
                        // Begin TT#2307 - JSmith - Incorrect Stock Values
                        //if (emei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
                        //{
                        //    emi.ContainsReoccurringDates = true;
                        //}
                        //if (emei.DateRange.DateRangeType == eCalendarRangeType.Dynamic)
                        //{
                        //    emi.ContainsDynamicDates = true;
                        //    if (emei.DateRange.RelativeTo == eDateRangeRelativeTo.StoreOpen)
                        //    {
                        //        emi.ContainsStoreDynamicDates = true;
                        //        emi.NeedsRebuilt = true;
                        //    }
                        //    if (emei.DateRange.RelativeTo == eDateRangeRelativeTo.Plan)
                        //    {
                        //        emi.ContainsPlanDynamicDates = true;
                        //        emi.NeedsRebuilt = true;
                        //    }
                        //}
                        if (emei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
                        {
                            emi.PriorityShippingContainsReoccurringDates = true;
                        }
                        if (emei.DateRange.DateRangeType == eCalendarRangeType.Dynamic)
                        {
                            emi.PriorityShippingContainsDynamicDates = true;
                            if (emei.DateRange.RelativeTo == eDateRangeRelativeTo.StoreOpen)
                            {
                                emi.PriorityShippingContainsStoreDynamicDates = true;
                                emi.PriorityShippingNeedsRebuilt = true;
                            }
                            if (emei.DateRange.RelativeTo == eDateRangeRelativeTo.Plan)
                            {
                                emi.PriorityShippingContainsPlanDynamicDates = true;
                                emi.PriorityShippingNeedsRebuilt = true;
                            }
                        }
                        // End TT#2307
				
						ProfileList weekProfileList = Calendar.GetWeekRange(emei.DateRange, null);
						if (emei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
								if (!emi.PriorityShippingModelDateEntries.Contains(weekProfile.WeekInYear))
								{
									emi.PriorityShippingModelDateEntries.Add(weekProfile.WeekInYear,null);
								}
							}
						}
						else
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
								if (!emi.PriorityShippingModelDateEntries.Contains(weekProfile.Key))
								{
									emi.PriorityShippingModelDateEntries.Add(weekProfile.Key, null);
								}
							}
						}
					}

                    // Begin TT#2307 - JSmith - Incorrect Stock Values
                    emi.ModelDateEntriesLoadedByStore = false;
                    emi.SalesEligibilityModelDateEntriesLoadedByStore = false;
                    emi.PriorityShippingModelDateEntriesLoadedByStore = false;
                    // End TT#2307
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					EventLog.WriteEntry("MIDHierarchyService", "LoadEligibilityModelDates error:" + ex.Message, EventLogEntryType.Error);
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:LoadEligibilityModelDates writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:LoadEligibilityModelDates writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        /// <summary>
        /// Requests the date information associated with a stock modifier model by store is loaded.
        /// </summary>
        /// <param name="emi">The Eligibility model object</param>
        static private void LoadSalesEligibilityModelDatesByStore(EligModelInfo emi, ProfileList aStoreList)
        {
            try
            {
                Hashtable modelDateEntries;
                ProfileList weekProfileList = null;
                models_rwl.AcquireWriterLock(WriterLockTimeOut);
                try
                {
                    //clear out any old date entries
                    emi.SalesEligibilityModelDateEntries = new Hashtable();

                    foreach (EligModelEntryInfo emei in emi.SalesEligibilityEntries)
                    {
                        if (emei.DateRange.RelativeTo != eDateRangeRelativeTo.StoreOpen)
                        {
                            weekProfileList = Calendar.GetWeekRange(emei.DateRange, null);
                        }

                        foreach (StoreProfile storeProfile in aStoreList)
                        {
                            modelDateEntries = (Hashtable)emi.SalesEligibilityModelDateEntries[storeProfile.Key];
                            if (modelDateEntries == null)
                            {
                                modelDateEntries = new Hashtable();
                                emi.SalesEligibilityModelDateEntries.Add(storeProfile.Key, modelDateEntries);
                            }

                            if (emei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
                            {
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!modelDateEntries.Contains(weekProfile.WeekInYear))
                                    {
                                        modelDateEntries.Add(weekProfile.WeekInYear, null);
                                    }
                                }
                            }
                            else if (emei.DateRange.RelativeTo == eDateRangeRelativeTo.StoreOpen)
                            {
                                if (storeProfile.SellingOpenDt != Include.UndefinedDate)
                                {
                                    WeekProfile mrsWeek = Calendar.GetWeek(storeProfile.SellingOpenDt);
                                    emei.DateRange.InternalAnchorDate = mrsWeek;
                                }

                                weekProfileList = Calendar.GetWeekRange(emei.DateRange, emei.DateRange.InternalAnchorDate);
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!modelDateEntries.Contains(weekProfile.Key))
                                    {
                                        modelDateEntries.Add(weekProfile.Key, null);
                                    }
                                }
                            }
                            else
                            {
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!emi.ModelDateEntries.Contains(weekProfile.Key))
                                    {
                                        modelDateEntries.Add(weekProfile.Key, null);
                                    }
                                }
                            }
                            //emi.SalesEligibilityModelDateEntries.Add(storeProfile.Key, modelDateEntries);
                        }
                    }
                    emi.SalesEligibilityNeedsRebuilt = false;
                    emi.SalesEligibilityModelDateEntriesLoadedByStore = true;
                }
                catch (Exception ex)
                {
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
                    EventLog.WriteEntry("MIDHierarchyService", "LoadSalesEligibilityModelDatesByStore error:" + ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    // Ensure that the lock is released.
                    models_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:LoadSalesEligibilityModelDatesByStore writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:LoadSalesEligibilityModelDatesByStore writer lock has timed out");
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

        /// <summary>
        /// Requests the date information associated with a stock modifier model by store is loaded.
        /// </summary>
        /// <param name="emi">The Eligibility model object</param>
        static private void LoadStockEligibilityModelDatesByStore(EligModelInfo emi, ProfileList aStoreList)
        {
            try
            {
                Hashtable modelDateEntries;
                ProfileList weekProfileList = null;
                models_rwl.AcquireWriterLock(WriterLockTimeOut);
                try
                {
                    //clear out any old date entries
                    emi.ModelDateEntries = new Hashtable();

                    foreach (EligModelEntryInfo emei in emi.ModelEntries)
                    {
                        if (emei.DateRange.RelativeTo != eDateRangeRelativeTo.StoreOpen)
                        {
                            weekProfileList = Calendar.GetWeekRange(emei.DateRange, null);
                        }

                        foreach (StoreProfile storeProfile in aStoreList)
                        {
                            modelDateEntries = (Hashtable)emi.ModelDateEntries[storeProfile.Key];
                            if (modelDateEntries == null)
                            {
                                modelDateEntries = new Hashtable();
                                emi.ModelDateEntries.Add(storeProfile.Key, modelDateEntries);
                            }

                            if (emei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
                            {
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!modelDateEntries.Contains(weekProfile.WeekInYear))
                                    {
                                        modelDateEntries.Add(weekProfile.WeekInYear, null);
                                    }
                                }
                            }
                            else if (emei.DateRange.RelativeTo == eDateRangeRelativeTo.StoreOpen)
                            {
                                // Begin TT#91 MD - JSmith - Issue loading models from Hierarchy when date range is relative to store open
								//if (storeProfile.StockCloseDt != Include.UndefinedDate)
                                if (storeProfile.SellingOpenDt != Include.UndefinedDate)
                                // End TT#91 MD
                                {
                                    WeekProfile mrsWeek = Calendar.GetWeek(storeProfile.SellingOpenDt);
                                    emei.DateRange.InternalAnchorDate = mrsWeek;
                                }

                                weekProfileList = Calendar.GetWeekRange(emei.DateRange, emei.DateRange.InternalAnchorDate);
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!modelDateEntries.Contains(weekProfile.Key))
                                    {
                                        modelDateEntries.Add(weekProfile.Key, null);
                                    }
                                }
                            }
                            else
                            {
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!emi.ModelDateEntries.Contains(weekProfile.Key))
                                    {
                                        modelDateEntries.Add(weekProfile.Key, null);
                                    }
                                }
                            }
                            //emi.ModelDateEntries.Add(storeProfile.Key, modelDateEntries);
                        }
                    }

                    emi.NeedsRebuilt = false;
                    emi.ModelDateEntriesLoadedByStore = true;
                }
                catch (Exception ex)
                {
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
                    EventLog.WriteEntry("MIDHierarchyService", "LoadStockEligibilityModelDatesByStore error:" + ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    // Ensure that the lock is released.
                    models_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:LoadStockEligibilityModelDatesByStore writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:LoadStockEligibilityModelDatesByStore writer lock has timed out");
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

        /// <summary>
        /// Requests the date information associated with a stock modifier model by store is loaded.
        /// </summary>
        /// <param name="emi">The Eligibility model object</param>
        static private void LoadPriorityShippingModelDatesByStore(EligModelInfo emi, ProfileList aStoreList)
        {
            try
            {
                Hashtable modelDateEntries;
                ProfileList weekProfileList = null;
                models_rwl.AcquireWriterLock(WriterLockTimeOut);
                try
                {
                    //clear out any old date entries
                    emi.PriorityShippingModelDateEntries = new Hashtable();

                    foreach (EligModelEntryInfo emei in emi.PriorityShippingEntries)
                    {
                        if (emei.DateRange.RelativeTo != eDateRangeRelativeTo.StoreOpen)
                        {
                            weekProfileList = Calendar.GetWeekRange(emei.DateRange, null);
                        }

                        foreach (StoreProfile storeProfile in aStoreList)
                        {
                            modelDateEntries = (Hashtable)emi.PriorityShippingModelDateEntries[storeProfile.Key];
                            if (modelDateEntries == null)
                            {
                                modelDateEntries = new Hashtable();
                                emi.PriorityShippingModelDateEntries.Add(storeProfile.Key, modelDateEntries);
                            }

                            if (emei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
                            {
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!modelDateEntries.Contains(weekProfile.WeekInYear))
                                    {
                                        modelDateEntries.Add(weekProfile.WeekInYear, null);
                                    }
                                }
                            }
                            else if (emei.DateRange.RelativeTo == eDateRangeRelativeTo.StoreOpen)
                            {
                                // Begin TT#91 MD - JSmith - Issue loading models from Hierarchy when date range is relative to store open
								//if (storeProfile.StockCloseDt != Include.UndefinedDate)
                                if (storeProfile.SellingOpenDt != Include.UndefinedDate)
                                // End TT#91 MD
                                {
                                    WeekProfile mrsWeek = Calendar.GetWeek(storeProfile.SellingOpenDt);
                                    emei.DateRange.InternalAnchorDate = mrsWeek;
                                }

                                weekProfileList = Calendar.GetWeekRange(emei.DateRange, emei.DateRange.InternalAnchorDate);
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!modelDateEntries.Contains(weekProfile.Key))
                                    {
                                       modelDateEntries.Add(weekProfile.Key, null);
                                    }
                                }
                            }
                            else
                            {
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!emi.ModelDateEntries.Contains(weekProfile.Key))
                                    {
                                        modelDateEntries.Add(weekProfile.Key, null);
                                    }
                                }
                            }
                            //emi.PriorityShippingModelDateEntries.Add(storeProfile.Key, modelDateEntries);
                        }
                    }

                    emi.PriorityShippingNeedsRebuilt = false;
                    emi.PriorityShippingModelDateEntriesLoadedByStore = true;
                }
                catch (Exception ex)
                {
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
                    EventLog.WriteEntry("MIDHierarchyService", "LoadPriorityShippingModelDatesByStore error:" + ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    // Ensure that the lock is released.
                    models_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:LoadPriorityShippingModelDatesByStore writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:LoadPriorityShippingModelDatesByStore writer lock has timed out");
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }
        // End TT#2307

		/// <summary>
		/// Adds or updates information about an stock modifier model
		/// </summary>
		/// <param name="smmp">Information about the stock modifier model</param>
		static public void StkModModelUpdate(StkModModelProfile smmp)
		{
			try
			{
				models_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					switch (smmp.ModelChangeType)
					{
						case eChangeType.none: 
						{
							break;
						}
						case eChangeType.add: 
						{
							StkModModelInfo smmi = new StkModModelInfo();
							smmi.ModelRID = smmp.Key;
							smmi.ModelID = smmp.ModelID;
							smmi.StkModModelDefault = smmp.StkModModelDefault;
							foreach (StkModModelEntry smme in smmp.ModelEntries)
							{
								StkModModelEntryInfo modelEntry = new StkModModelEntryInfo(); 
								modelEntry.ModelEntrySeq = smme.ModelEntrySeq;
								modelEntry.StkModModelEntryValue = smme.StkModModelEntryValue;
								modelEntry.DateRange = smme.DateRange;
								smmi.ModelEntries.Add(modelEntry);
							}
							smmi.UpdateDateTime = DateTime.Now;
							LoadStockModifierModelDates(smmi);
							_stkModModelsByRID.Add(smmi.ModelRID, smmi);
							_stkModModelsByID.Add(smmi.ModelID, smmi.ModelRID);
							break;
						}
						case eChangeType.update: 
						{
							StkModModelInfo smmi = (StkModModelInfo)_stkModModelsByRID[smmp.Key];
							_stkModModelsByID.Remove(smmp.ModelID);
							smmi.ModelRID = smmp.Key;
							smmi.ModelID = smmp.ModelID;
							smmi.StkModModelDefault = smmp.StkModModelDefault;
							smmi.ModelEntries.Clear();
							foreach (StkModModelEntry smme in smmp.ModelEntries)
							{
								StkModModelEntryInfo modelEntry = new StkModModelEntryInfo(); 
								modelEntry.ModelEntrySeq = smme.ModelEntrySeq;
								modelEntry.StkModModelEntryValue = smme.StkModModelEntryValue;
								modelEntry.DateRange = smme.DateRange;
								smmi.ModelEntries.Add(modelEntry);
							}
							smmi.UpdateDateTime = DateTime.Now;
							LoadStockModifierModelDates(smmi);
							_stkModModelsByRID[smmp.Key] = smmi;
							_stkModModelsByID.Add(smmi.ModelID, smmi.ModelRID);
							break;
						}
						case eChangeType.delete: 
						{
							StkModModelInfo smmi = (StkModModelInfo)_stkModModelsByRID[smmp.Key];
							_stkModModelsByRID.Remove(smmp.Key);
							_stkModModelsByID.Remove(smmi.ModelID);
							break;
						}
					}
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					EventLog.WriteEntry("MIDHierarchyService", "StkModModelUpdate error:" + ex.Message, EventLogEntryType.Error);
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:StkModModelUpdate writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:StkModModelUpdate writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets all information associated with an stock modifier model using the model name.
		/// </summary>
		/// <param name="ModelID">The model id of the stock modifier model</param>
		/// <returns>An instance of the StkModModelProfile class containing stock modifier model information</returns>
		static public StkModModelProfile GetStkModModelData(string ModelID)
		{
            int smmRID;
			try
			{
				models_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
                    if (_stkModModelsByID.TryGetValue(ModelID, out smmRID))
					{
						return GetStkModModelData(smmRID);
					}
					else
					{
						StkModModelProfile smmp = new StkModModelProfile(Include.NoRID);
						smmp.Key = Include.NoRID;  // not found
						return smmp;
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetStkModModelData reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetStkModModelData reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        static public StkModModelProfile GetStkModModelData(int ModelRID)
        {
            try
            {
                return GetStkModModelData(ModelRID, null);
            }
            catch
            {
                throw;
            }
        }
        // End TT#2307

		/// <summary>
		/// Gets the information about a stock modifier model.
		/// </summary>
		/// <param name="ModelRID">The record id of the stock modifier model</param>
		/// <returns>An instance of the StkModModelProfile class containing stock modifier model information</returns>
        // Begin TT#2307 - JSmith - Incorrect Stock Values
        //static public StkModModelProfile GetStkModModelData(int ModelRID)
        static public StkModModelProfile GetStkModModelData(int ModelRID, ProfileList aStoreList)
        // End TT#2307
		{
			try
			{
                // Begin TT#2307 - JSmith - Incorrect Stock Values
                DateTime storeUpdateDate = DateTime.Now;
                // End TT#2307

				models_rwl.AcquireReaderLock(ReaderLockTimeOut);
				StkModModelInfo smmi;
				try
				{
					if (!_stkModModelsByRID.TryGetValue(ModelRID, out smmi))
					{
						smmi = new StkModModelInfo();
						smmi.ModelRID = Include.NoRID;
					}
					StkModModelProfile smmp = new StkModModelProfile(smmi.ModelRID);
					smmp.ModelChangeType = eChangeType.none;
					smmp.Key = smmi.ModelRID;
					smmp.ModelID = smmi.ModelID;
					smmp.StkModModelDefault = smmi.StkModModelDefault;
					smmp.UpdateDateTime = smmi.UpdateDateTime;
					foreach (StkModModelEntryInfo smmei in smmi.ModelEntries)
					{
						StkModModelEntry modelEntry = new StkModModelEntry(); 
						modelEntry.ModelEntryChangeType = eChangeType.none;
						modelEntry.ModelEntrySeq = smmei.ModelEntrySeq;
						modelEntry.StkModModelEntryValue = smmei.StkModModelEntryValue;
						modelEntry.DateRange = smmei.DateRange;
						smmp.ModelEntries.Add(modelEntry);
					}
					smmp.ContainsDynamicDates = smmi.ContainsDynamicDates;
					smmp.ContainsPlanDynamicDates = smmi.ContainsPlanDynamicDates;
					smmp.ContainsReoccurringDates = smmi.ContainsReoccurringDates;
					smmp.ContainsStoreDynamicDates = smmi.ContainsStoreDynamicDates;

                    // Begin TT#2307 - JSmith - Incorrect Stock Values
                    if (aStoreList != null &&
                        smmi.ContainsStoreDynamicDates &&
                        !smmi.ModelDateEntriesLoadedByStore)
                    {
                        LoadStockModifierModelDatesByStore(smmi, aStoreList);
                    }
                    smmp.NeedsRebuilt = smmi.NeedsRebuilt;
                    // End TT#2307
                    if (!smmp.NeedsRebuilt)
					{
						smmp.ModelDateEntries = (Hashtable) smmi.ModelDateEntries.Clone();
					}
					return smmp;
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetStkModModelData reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetStkModModelData reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the date and time the stock modifier model was last updated.
		/// </summary>
		/// <param name="ModelRID">The record id of the stock modifier model</param>
		/// <returns>A string containing the date and time the model was last updated </returns>
		static public string GetStkModModelUpdateDateString(int ModelRID)
		{
			try
			{
				models_rwl.AcquireReaderLock(ReaderLockTimeOut);
				StkModModelInfo smmi;
				try
				{
                    if (!_stkModModelsByRID.TryGetValue(ModelRID, out smmi))
					{
						smmi = new StkModModelInfo();
						smmi.ModelRID = Include.NoRID;
					}
					
					return smmi.UpdateDateTimeString;
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetStkModModelUpdateDateString reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetStkModModelUpdateDateString reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Populates the model DataTable with stock modifier models information
		/// </summary>
		/// <returns></returns>
		static public ProfileList PopulateStkModModels()
		{
			try
			{
				models_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
					ModelNameList modelList = new ModelNameList(eProfileType.ModelName);
					foreach (KeyValuePair<int, StkModModelInfo>  stkModModel in _stkModModelsByRID)
					{
						StkModModelInfo smmi = stkModModel.Value;
						ModelName mn = new ModelName(smmi.ModelRID);
						mn.ModelID = smmi.ModelID;
						modelList.Add(mn);
					}
					if (modelList.Count == 0)
					{
						ModelName mn = new ModelName(Include.NoRID);
						mn.ModelID = " ";
						modelList.Add(mn);
					}
					modelList.ArrayList.Sort(new ModelNameSort());
					return modelList;
				}        
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:PopulateStkModModels reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:PopulateStkModModels reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Requests the date information associated with a stock modifier model is loaded.
		/// </summary>
		/// <param name="smmi">The stock modifier model object</param>
		static private void LoadStockModifierModelDates(StkModModelInfo smmi)
		{
			try
			{
				models_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					//clear out any old date entries
					smmi.ModelDateEntries.Clear();
					smmi.NeedsRebuilt = false;
					smmi.ModelDateEntries.Add(Include.UndefinedDay,smmi.StkModModelDefault);
					foreach (StkModModelEntryInfo smmei in smmi.ModelEntries)
					{
						if (smmei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
						{
							smmi.ContainsReoccurringDates = true;
						}
						if (smmei.DateRange.DateRangeType == eCalendarRangeType.Dynamic)
						{
							smmi.ContainsDynamicDates = true;
							if (smmei.DateRange.RelativeTo == eDateRangeRelativeTo.StoreOpen)
							{
								smmi.ContainsStoreDynamicDates = true;
								smmi.NeedsRebuilt = true;
							}
							if (smmei.DateRange.RelativeTo == eDateRangeRelativeTo.Plan)
							{
								smmi.ContainsPlanDynamicDates = true;
								smmi.NeedsRebuilt = true;
							}
						}
				
						ProfileList weekProfileList = Calendar.GetWeekRange(smmei.DateRange, null);
						if (smmei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
								if (!smmi.ModelDateEntries.Contains(weekProfile.WeekInYear))
								{
									smmi.ModelDateEntries.Add(weekProfile.WeekInYear,smmei.StkModModelEntryValue);
								}
							}
						}
						else
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
								if (!smmi.ModelDateEntries.Contains(weekProfile.Key))
								{
									smmi.ModelDateEntries[weekProfile.Key] = smmei.StkModModelEntryValue;
								}
							}
						}
					}

                    // Begin TT#2307 - JSmith - Incorrect Stock Values
                    smmi.ModelDateEntriesLoadedByStore = false;
                    // End TT#2307
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					EventLog.WriteEntry("MIDHierarchyService", "LoadStockModifierModelDates error:" + ex.Message, EventLogEntryType.Error);
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:LoadStockModifierModelDates writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:LoadStockModifierModelDates writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        /// <summary>
        /// Requests the date information associated with a stock modifier model by store is loaded.
        /// </summary>
        /// <param name="smmi">The stock modifier model object</param>
        static private void LoadStockModifierModelDatesByStore(StkModModelInfo smmi, ProfileList aStoreList)
        {
            try
            {
                Hashtable modelDateEntries;
                ProfileList weekProfileList = null;
                models_rwl.AcquireWriterLock(WriterLockTimeOut);
                try
                {
                    //clear out any old date entries
                    smmi.ModelDateEntries = new Hashtable();

                    foreach (StkModModelEntryInfo smmei in smmi.ModelEntries)
                    {
                        if (smmei.DateRange.RelativeTo != eDateRangeRelativeTo.StoreOpen)
                        {
                            weekProfileList = Calendar.GetWeekRange(smmei.DateRange, null);
                        }

                        foreach (StoreProfile storeProfile in aStoreList)
                        {
                            modelDateEntries = (Hashtable)smmi.ModelDateEntries[storeProfile.Key];
                            if (modelDateEntries == null)
                            {
                                modelDateEntries = new Hashtable();
                                smmi.ModelDateEntries.Add(storeProfile.Key, modelDateEntries);
                                modelDateEntries.Add(Include.UndefinedDay, smmi.StkModModelDefault);
                            }

                            if (smmei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
                            {
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!modelDateEntries.Contains(weekProfile.WeekInYear))
                                    {
                                        modelDateEntries.Add(weekProfile.WeekInYear, smmei.StkModModelEntryValue);
                                    }
                                }
                            }
                            else if (smmei.DateRange.RelativeTo == eDateRangeRelativeTo.StoreOpen)
                            {
                                // Begin TT#91 MD - JSmith - Issue loading models from Hierarchy when date range is relative to store open
								//if (storeProfile.StockCloseDt != Include.UndefinedDate)
                                if (storeProfile.SellingOpenDt != Include.UndefinedDate)
                                // End TT#91 MD
                                {
                                    WeekProfile mrsWeek = Calendar.GetWeek(storeProfile.SellingOpenDt);
                                    smmei.DateRange.InternalAnchorDate = mrsWeek;
                                }

                                weekProfileList = Calendar.GetWeekRange(smmei.DateRange, smmei.DateRange.InternalAnchorDate);
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!modelDateEntries.Contains(weekProfile.Key))
                                    {
                                        modelDateEntries[weekProfile.Key] = smmei.StkModModelEntryValue;
                                    }
                                }
                            }
                            else
                            {
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!smmi.ModelDateEntries.Contains(weekProfile.Key))
                                    {
                                        smmi.ModelDateEntries[weekProfile.Key] = smmei.StkModModelEntryValue;
                                    }
                                }
                            }
                            //smmi.ModelDateEntries.Add(storeProfile.Key, modelDateEntries);
                        }
                    }

                    smmi.NeedsRebuilt = false;
                    smmi.ModelDateEntriesLoadedByStore = true;
                }
                catch (Exception ex)
                {
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
                    EventLog.WriteEntry("MIDHierarchyService", "LoadStockModifierModelDatesByStore error:" + ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    // Ensure that the lock is released.
                    models_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:LoadStockModifierModelDatesByStore writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:LoadStockModifierModelDatesByStore writer lock has timed out");
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }
        // End TT#2307

		/// <summary>
		/// Adds or updates information about a sales modifier model
		/// </summary>
		/// <param name="smmp">Information about the sales modifier model</param>
		static public void SlsModModelUpdate(SlsModModelProfile smmp)
		{
			try
			{
				models_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					switch (smmp.ModelChangeType)
					{
						case eChangeType.none: 
						{
							break;
						}
						case eChangeType.add: 
						{
							SlsModModelInfo smmi = new SlsModModelInfo();
							smmi.ModelRID = smmp.Key;
							smmi.ModelID = smmp.ModelID;
							smmi.SlsModModelDefault = smmp.SlsModModelDefault;
							foreach (SlsModModelEntry smme in smmp.ModelEntries)
							{
								SlsModModelEntryInfo modelEntry = new SlsModModelEntryInfo(); 
								modelEntry.ModelEntrySeq = smme.ModelEntrySeq;
								modelEntry.SlsModModelEntryValue = smme.SlsModModelEntryValue;
								modelEntry.DateRange = smme.DateRange;
								smmi.ModelEntries.Add(modelEntry);
							}
							smmi.UpdateDateTime = DateTime.Now;
							LoadSalesModifierModelDates(smmi);
							_slsModModelsByRID.Add(smmi.ModelRID, smmi);
							_slsModModelsByID.Add(smmi.ModelID, smmi.ModelRID);
							break;
						}
						case eChangeType.update: 
						{
							SlsModModelInfo smmi = (SlsModModelInfo)_slsModModelsByRID[smmp.Key];
							_slsModModelsByID.Remove(smmp.ModelID);
							smmi.ModelRID = smmp.Key;
							smmi.ModelID = smmp.ModelID;
							smmi.SlsModModelDefault = smmp.SlsModModelDefault;
							smmi.ModelEntries.Clear();
							foreach (SlsModModelEntry smme in smmp.ModelEntries)
							{
								SlsModModelEntryInfo modelEntry = new SlsModModelEntryInfo(); 
								modelEntry.ModelEntrySeq = smme.ModelEntrySeq;
								modelEntry.SlsModModelEntryValue = smme.SlsModModelEntryValue;
								modelEntry.DateRange = smme.DateRange;
								smmi.ModelEntries.Add(modelEntry);
							}
							smmi.UpdateDateTime = DateTime.Now;
							LoadSalesModifierModelDates(smmi);
							_slsModModelsByRID[smmp.Key] = smmi;
							_slsModModelsByID.Add(smmi.ModelID, smmi.ModelRID);
							break;
						}
						case eChangeType.delete: 
						{
							SlsModModelInfo smmi = (SlsModModelInfo)_slsModModelsByRID[smmp.Key];
							_slsModModelsByRID.Remove(smmp.Key);
							_slsModModelsByID.Remove(smmi.ModelID);
							break;
						}
					}
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					EventLog.WriteEntry("MIDHierarchyService", "StkModModelUpdate error:" + ex.Message, EventLogEntryType.Error);
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:SlsModModelUpdate writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:SlsModModelUpdate writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets all information associated with an sales modifier model using the model name.
		/// </summary>
		/// <param name="ModelID">The model id of the sales modifier model</param>
		/// <returns>An instance of the SlsModModelProfile class containing sales modifier model information</returns>
		static public SlsModModelProfile GetSlsModModelData(string ModelID)
		{
            int smmRID;
			try
			{
				models_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
					if (_slsModModelsByID.TryGetValue(ModelID, out smmRID))
					{
						return GetSlsModModelData(smmRID);
					}
					else
					{
						SlsModModelProfile smmp = new SlsModModelProfile(Include.NoRID);
						smmp.Key = Include.NoRID;  // not found
						return smmp;
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetSlsModModelData reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetSlsModModelData reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}		

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        static public SlsModModelProfile GetSlsModModelData(int ModelRID)
        {
            try
            {
                return GetSlsModModelData(ModelRID, null);
            }
            catch
            {
                throw;
            }
        }
        // End TT#2307

		/// <summary>
		/// Gets the information about a sales modifier model.
		/// </summary>
		/// <param name="ModelRID">The record id of the sales modifier model</param>
		/// <returns>An instance of the SlsModModelProfile class containing sales modifier model information</returns>
        // Begin TT#2307 - JSmith - Incorrect Stock Values
        //static public SlsModModelProfile GetSlsModModelData(int ModelRID)
        static public SlsModModelProfile GetSlsModModelData(int ModelRID, ProfileList aStoreList)
        // End TT#2307
		{
			try
			{
                // Begin TT#2307 - JSmith - Incorrect Stock Values
                DateTime storeUpdateDate = DateTime.Now;
                // End TT#2307

				models_rwl.AcquireReaderLock(ReaderLockTimeOut);
				SlsModModelInfo smmi;
				try
				{
                    if (!_slsModModelsByRID.TryGetValue(ModelRID, out smmi))
					{
						smmi = new SlsModModelInfo();
						smmi.ModelRID = Include.NoRID;
					}
					SlsModModelProfile smmp = new SlsModModelProfile(smmi.ModelRID);
					smmp.ModelChangeType = eChangeType.none;
					smmp.Key = smmi.ModelRID;
					smmp.ModelID = smmi.ModelID;
					smmp.SlsModModelDefault = smmi.SlsModModelDefault;
					smmp.UpdateDateTime = smmi.UpdateDateTime;
					foreach (SlsModModelEntryInfo smmei in smmi.ModelEntries)
					{
						SlsModModelEntry modelEntry = new SlsModModelEntry(); 
						modelEntry.ModelEntryChangeType = eChangeType.none;
						modelEntry.ModelEntrySeq = smmei.ModelEntrySeq;
						modelEntry.SlsModModelEntryValue = smmei.SlsModModelEntryValue;
						modelEntry.DateRange = smmei.DateRange;
						smmp.ModelEntries.Add(modelEntry);
					}
					smmp.ContainsDynamicDates = smmi.ContainsDynamicDates;
					smmp.ContainsPlanDynamicDates = smmi.ContainsPlanDynamicDates;
					smmp.ContainsReoccurringDates = smmi.ContainsReoccurringDates;
					smmp.ContainsStoreDynamicDates = smmi.ContainsStoreDynamicDates;
                    // Begin TT#2307 - JSmith - Incorrect Stock Values
                    if (aStoreList != null &&
                        smmi.ContainsStoreDynamicDates &&
                        !smmi.ModelDateEntriesLoadedByStore)
                    {
                        LoadSalesModifierModelDatesByStore(smmi, aStoreList);
                    }
                    smmp.NeedsRebuilt = smmi.NeedsRebuilt;
                    // End TT#2307

                    if (!smmp.NeedsRebuilt)
					{
						smmp.ModelDateEntries = (Hashtable) smmi.ModelDateEntries.Clone();
					}
					return smmp;
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetSlsModModelData reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetSlsModModelData reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the date and time sales modifier model was last updated.
		/// </summary>
		/// <param name="ModelRID">The record id of the sales modifier model</param>
		/// <returns>A string containing the date and time the model was last updated </returns>
		static public string GetSlsModModelUpdateDateString(int ModelRID)
		{
			try
			{
				models_rwl.AcquireReaderLock(ReaderLockTimeOut);
				SlsModModelInfo smmi;
				try
				{
                    if (!_slsModModelsByRID.TryGetValue(ModelRID, out smmi))
					{
						smmi = new SlsModModelInfo();
						smmi.ModelRID = Include.NoRID;
					}
					
					return smmi.UpdateDateTimeString;
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetSlsModModelUpdateDateString reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetSlsModModelUpdateDateString reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Populates the model name list with sales modifier models information
		/// </summary>
		/// <returns></returns>
		static public ProfileList PopulateSlsModModels()
		{
			try
			{
				models_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
					ModelNameList modelList = new ModelNameList(eProfileType.ModelName);
					foreach (KeyValuePair<int, SlsModModelInfo> slsModModel in _slsModModelsByRID)
					{
						SlsModModelInfo smmi = slsModModel.Value;
						ModelName mn = new ModelName(smmi.ModelRID);
						mn.ModelID = smmi.ModelID;
						modelList.Add(mn);
					}
					if (modelList.Count == 0)
					{
						ModelName mn = new ModelName(Include.NoRID);
						mn.ModelID = " ";
						modelList.Add(mn);
					}
					modelList.ArrayList.Sort(new ModelNameSort());
					return modelList;
				}        
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:PopulateSlsModModels reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:PopulateSlsModModels reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Requests the date information associated with a sales modifier model is loaded.
		/// </summary>
		/// <param name="smmi">The sales modifier model object</param>
		static private void LoadSalesModifierModelDates(SlsModModelInfo smmi)
		{
			try
			{
				models_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					//clear out any old date entries
					smmi.ModelDateEntries.Clear();
					smmi.NeedsRebuilt = false;
					smmi.ModelDateEntries.Add(Include.UndefinedDay,smmi.SlsModModelDefault);
					foreach (SlsModModelEntryInfo smmei in smmi.ModelEntries)
					{
						if (smmei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
						{
							smmi.ContainsReoccurringDates = true;
						}
						if (smmei.DateRange.DateRangeType == eCalendarRangeType.Dynamic)
						{
							smmi.ContainsDynamicDates = true;
							if (smmei.DateRange.RelativeTo == eDateRangeRelativeTo.StoreOpen)
							{
								smmi.ContainsStoreDynamicDates = true;
								smmi.NeedsRebuilt = true;
							}
							if (smmei.DateRange.RelativeTo == eDateRangeRelativeTo.Plan)
							{
								smmi.ContainsPlanDynamicDates = true;
								smmi.NeedsRebuilt = true;
							}
						}
				
						ProfileList weekProfileList = Calendar.GetWeekRange(smmei.DateRange, null);
						if (smmei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
								if (!smmi.ModelDateEntries.Contains(weekProfile.WeekInYear))
								{
									smmi.ModelDateEntries.Add(weekProfile.WeekInYear,smmei.SlsModModelEntryValue);
								}
							}
						}
						else
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
								if (!smmi.ModelDateEntries.Contains(weekProfile.Key))
								{
									smmi.ModelDateEntries[weekProfile.Key] = smmei.SlsModModelEntryValue;
								}
							}
						}
					}

                    // Begin TT#2307 - JSmith - Incorrect Stock Values
                    smmi.ModelDateEntriesLoadedByStore = false;
                    // End TT#2307
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					EventLog.WriteEntry("MIDHierarchyService", "LoadSalesModifierModelDates error:" + ex.Message, EventLogEntryType.Error);
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:LoadSalesModifierModelDates writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:LoadSalesModifierModelDates writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        /// <summary>
        /// Requests the date information associated with a stock modifier model by store is loaded.
        /// </summary>
        /// <param name="smmi">The stock modifier model object</param>
        static private void LoadSalesModifierModelDatesByStore(SlsModModelInfo smmi, ProfileList aStoreList)
        {
            try
            {
                Hashtable modelDateEntries;
                ProfileList weekProfileList = null;
                models_rwl.AcquireWriterLock(WriterLockTimeOut);
                try
                {
                    //clear out any old date entries
                    smmi.ModelDateEntries = new Hashtable();

                    foreach (SlsModModelEntryInfo smmei in smmi.ModelEntries)
                    {
                        if (smmei.DateRange.RelativeTo != eDateRangeRelativeTo.StoreOpen)
                        {
                            weekProfileList = Calendar.GetWeekRange(smmei.DateRange, null);
                        }

                        foreach (StoreProfile storeProfile in aStoreList)
                        {
                            modelDateEntries = (Hashtable)smmi.ModelDateEntries[storeProfile.Key];
                            if (modelDateEntries == null)
                            {
                                modelDateEntries = new Hashtable();
                                smmi.ModelDateEntries.Add(storeProfile.Key, modelDateEntries);
                                modelDateEntries.Add(Include.UndefinedDay, smmi.SlsModModelDefault);
                            }

                            if (smmei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
                            {
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!modelDateEntries.Contains(weekProfile.WeekInYear))
                                    {
                                        modelDateEntries.Add(weekProfile.WeekInYear, smmei.SlsModModelEntryValue);
                                    }
                                }
                            }
                            else if (smmei.DateRange.RelativeTo == eDateRangeRelativeTo.StoreOpen)
                            {
                                if (storeProfile.SellingOpenDt != Include.UndefinedDate)
                                {
                                    WeekProfile mrsWeek = Calendar.GetWeek(storeProfile.SellingOpenDt);
                                    smmei.DateRange.InternalAnchorDate = mrsWeek;
                                }

                                weekProfileList = Calendar.GetWeekRange(smmei.DateRange, smmei.DateRange.InternalAnchorDate);
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!modelDateEntries.Contains(weekProfile.Key))
                                    {
                                        modelDateEntries[weekProfile.Key] = smmei.SlsModModelEntryValue;
                                    }
                                }
                            }
                            else
                            {
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!smmi.ModelDateEntries.Contains(weekProfile.Key))
                                    {
                                        smmi.ModelDateEntries[weekProfile.Key] = smmei.SlsModModelEntryValue;
                                    }
                                }
                            }
                            //smmi.ModelDateEntries.Add(storeProfile.Key, modelDateEntries);
                        }
                    }

                    smmi.NeedsRebuilt = false;
                    smmi.ModelDateEntriesLoadedByStore = true;
                }
                catch (Exception ex)
                {
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
                    EventLog.WriteEntry("MIDHierarchyService", "LoadSalesModifierModelDatesByStore error:" + ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    // Ensure that the lock is released.
                    models_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:LoadSalesModifierModelDatesByStore writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:LoadSalesModifierModelDatesByStore writer lock has timed out");
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }
        // End TT#2307

		static public SizeAltModelProfile GetSizeAltModelData(int ModelRID)
		{
			try
			{
				models_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
					 
					SizeAltModelProfile samp = new SizeAltModelProfile(ModelRID);
					samp.ModelChangeType = eChangeType.none;
					samp.Key = ModelRID;
		
					return samp;
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetStkModModelData reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetStkModModelData reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}
		
		static public SizeConstraintModelProfile GetSizeConstraintModelData(int ModelRID)
			{
				try
				{
					models_rwl.AcquireReaderLock(ReaderLockTimeOut);
					try
					{
					 
						SizeConstraintModelProfile scmp = new SizeConstraintModelProfile(ModelRID);
						scmp.ModelChangeType = eChangeType.none;
						scmp.Key = ModelRID;
		
						return scmp;
					}
					catch ( Exception ex )
					{
                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                        if (Audit != null)
                        {
                            Audit.Log_Exception(ex);
                        }
                        // End TT#189
						throw;
					}
					finally
					{
						// Ensure that the lock is released.
						models_rwl.ReleaseReaderLock();
					}
				}
				catch (ApplicationException ex)
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					// The reader lock request timed out.
					EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetStkModModelData reader lock has timed out", EventLogEntryType.Error);
					throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetStkModModelData reader lock has timed out");
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
			}

		static public SizeCurveGroupProfile GetSizeCurveGroupData(int ModelRID)
		{
			try
			{
				models_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
					 
					SizeCurveGroupProfile scgp = new SizeCurveGroupProfile(ModelRID);
					scgp.ModelChangeType = eChangeType.none;
					scgp.Key = ModelRID;
		
					return scgp;
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetStkModModelData reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetStkModModelData reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static public SizeGroupProfile GetSizeGroupData(int ModelRID)
		{
			try
			{
				models_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
					 
					SizeGroupProfile sgp = new SizeGroupProfile(ModelRID);
					sgp.ModelChangeType = eChangeType.none;
					sgp.Key = ModelRID;
		
					return sgp;
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetStkModModelData reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetStkModModelData reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Adds or updates information about a FWOS modifier model
		/// </summary>
        /// <param name="mmp">Information about the FWOS modifier model</param>
		static public void FWOSModModelUpdate(FWOSModModelProfile mmp)
		{
			try
			{
				models_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					switch (mmp.ModelChangeType)
					{
						case eChangeType.none: 
						{
							break;
						}
						case eChangeType.add: 
						{
							FWOSModModelInfo mmi = new FWOSModModelInfo();
							mmi.ModelRID = mmp.Key;
							mmi.ModelID = mmp.ModelID;
							mmi.FWOSModModelDefault = mmp.FWOSModModelDefault;
							foreach (FWOSModModelEntry smme in mmp.ModelEntries)
							{
								FWOSModModelEntryInfo modelEntry = new FWOSModModelEntryInfo(); 
								modelEntry.ModelEntrySeq = smme.ModelEntrySeq;
								modelEntry.FWOSModModelEntryValue = smme.FWOSModModelEntryValue;
								modelEntry.DateRange = smme.DateRange;
								mmi.ModelEntries.Add(modelEntry);
							}
							mmi.UpdateDateTime = DateTime.Now;
							LoadFWOSModifierModelDates(mmi);
							_FWOSModModelsByRID.Add(mmi.ModelRID, mmi);
							_FWOSModModelsByID.Add(mmi.ModelID, mmi.ModelRID);
							break;
						}
						case eChangeType.update: 
						{
							FWOSModModelInfo mmi = (FWOSModModelInfo)_FWOSModModelsByRID[mmp.Key];
							_FWOSModModelsByID.Remove(mmp.ModelID);
							mmi.ModelRID = mmp.Key;
							mmi.ModelID = mmp.ModelID;
							mmi.FWOSModModelDefault = mmp.FWOSModModelDefault;
							mmi.ModelEntries.Clear();
							foreach (FWOSModModelEntry smme in mmp.ModelEntries)
							{
								FWOSModModelEntryInfo modelEntry = new FWOSModModelEntryInfo(); 
								modelEntry.ModelEntrySeq = smme.ModelEntrySeq;
								modelEntry.FWOSModModelEntryValue = smme.FWOSModModelEntryValue;
								modelEntry.DateRange = smme.DateRange;
								mmi.ModelEntries.Add(modelEntry);
							}
							mmi.UpdateDateTime = DateTime.Now;
							LoadFWOSModifierModelDates(mmi);
                            _FWOSModModelsByRID[mmp.Key] = mmi;
							_FWOSModModelsByID.Add(mmi.ModelID, mmi.ModelRID);
							break;
						}
						case eChangeType.delete: 
						{
							FWOSModModelInfo mmi = (FWOSModModelInfo)_FWOSModModelsByRID[mmp.Key];
							_FWOSModModelsByRID.Remove(mmp.Key);
							_FWOSModModelsByID.Remove(mmi.ModelID);
							break;
						}
					}
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					EventLog.WriteEntry("MIDHierarchyService", "StkModModelUpdate error:" + ex.Message, EventLogEntryType.Error);
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:FWOSModModelUpdate writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:FWOSModModelUpdate writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets all information associated with a FWOS modifier model using the model name.
		/// </summary>
		/// <param name="ModelID">The model id of the FWOS modifier model</param>
		/// <returns>An instance of the FWOSModModelProfile class containing FWOS modifier model information</returns>
		static public FWOSModModelProfile GetFWOSModModelData(string ModelID)
		{
            int smmRID;
			try
			{
				models_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
					if (_FWOSModModelsByID.TryGetValue(ModelID, out smmRID))
					{
						return GetFWOSModModelData(smmRID);
					}
					else
					{
						FWOSModModelProfile mmp = new FWOSModModelProfile(Include.NoRID);
						mmp.Key = Include.NoRID;  // not found
						return mmp;
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetFWOSModModelData reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetFWOSModModelData reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        static public FWOSModModelProfile GetFWOSModModelData(int ModelRID)
		{
			try
			{
                return GetFWOSModModelData(ModelRID, null);
            }
            catch 
            {
                throw;
            }
        }
        // End TT#2307

		/// <summary>
		/// Gets the information about a FWOS modifier model.
		/// </summary>
		/// <param name="ModelRID">The record id of the FWOS modifier model</param>
		/// <returns>An instance of the FWOSModModelProfile class containing FWOS modifier model information</returns>
        // Begin TT#2307 - JSmith - Incorrect Stock Values
        //static public FWOSModModelProfile GetFWOSModModelData(int ModelRID)
        static public FWOSModModelProfile GetFWOSModModelData(int ModelRID, ProfileList aStoreList)
        // End TT#2307
		{
			try
			{
                // Begin TT#2307 - JSmith - Incorrect Stock Values
                DateTime storeUpdateDate = DateTime.Now;
                // End TT#2307

				models_rwl.AcquireReaderLock(ReaderLockTimeOut);
				FWOSModModelInfo mmi;
				try
				{
                    if (!_FWOSModModelsByRID.TryGetValue(ModelRID, out mmi))
					{
						mmi = new FWOSModModelInfo();
						mmi.ModelRID = Include.NoRID;
					}
					FWOSModModelProfile mmp = new FWOSModModelProfile(mmi.ModelRID);
					mmp.ModelChangeType = eChangeType.none;
					mmp.Key = mmi.ModelRID;
					mmp.ModelID = mmi.ModelID;
					mmp.FWOSModModelDefault = mmi.FWOSModModelDefault;
					mmp.UpdateDateTime = mmi.UpdateDateTime;
					foreach (FWOSModModelEntryInfo smmei in mmi.ModelEntries)
					{
						FWOSModModelEntry modelEntry = new FWOSModModelEntry(); 
						modelEntry.ModelEntryChangeType = eChangeType.none;
						modelEntry.ModelEntrySeq = smmei.ModelEntrySeq;
						modelEntry.FWOSModModelEntryValue = smmei.FWOSModModelEntryValue;
						modelEntry.DateRange = smmei.DateRange;
						mmp.ModelEntries.Add(modelEntry);
					}
					mmp.ContainsDynamicDates = mmi.ContainsDynamicDates;
					mmp.ContainsPlanDynamicDates = mmi.ContainsPlanDynamicDates;
					mmp.ContainsReoccurringDates = mmi.ContainsReoccurringDates;
					mmp.ContainsStoreDynamicDates = mmi.ContainsStoreDynamicDates;
                    // Begin TT#2307 - JSmith - Incorrect Stock Values
                    if (aStoreList != null &&
                        mmp.ContainsStoreDynamicDates &&
                        !mmi.ModelDateEntriesLoadedByStore)
                    {
                        LoadFWOSModifierModelDatesByStore(mmi, aStoreList);
                    }
                    mmp.NeedsRebuilt = mmi.NeedsRebuilt;
                    // End TT#2307

                    if (!mmp.NeedsRebuilt)
					{
						mmp.ModelDateEntries = (Hashtable) mmi.ModelDateEntries.Clone();
					}
					return mmp;
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetFWOSModModelData reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetFWOSModModelData reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the date and time FWOS modifier model was last updated.
		/// </summary>
		/// <param name="ModelRID">The record id of the FWOS modifier model</param>
		/// <returns>A string containing the date and time the model was last updated </returns>
		static public string GetFWOSModModelUpdateDateString(int ModelRID)
		{
			try
			{
				models_rwl.AcquireReaderLock(ReaderLockTimeOut);
				FWOSModModelInfo mmi;
				try
				{
                    if (!_FWOSModModelsByRID.TryGetValue(ModelRID, out mmi))
					{
						mmi = new FWOSModModelInfo();
						mmi.ModelRID = Include.NoRID;
					}
					
					return mmi.UpdateDateTimeString;
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetFWOSModModelUpdateDateString reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetFWOSModModelUpdateDateString reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Populates the model name list with FWOS modifier models information
		/// </summary>
		/// <returns></returns>
		static public ProfileList PopulateFWOSModModels()
		{
			try
			{
				models_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
					ModelNameList modelList = new ModelNameList(eProfileType.ModelName);
					foreach (KeyValuePair<int, FWOSModModelInfo>  FWOSModModel in _FWOSModModelsByRID)
					{
						FWOSModModelInfo mmi = FWOSModModel.Value;
						ModelName mn = new ModelName(mmi.ModelRID);
						mn.ModelID = mmi.ModelID;
						modelList.Add(mn);
					}
					if (modelList.Count == 0)
					{
						ModelName mn = new ModelName(Include.NoRID);
						mn.ModelID = " ";
						modelList.Add(mn);
					}
					modelList.ArrayList.Sort(new ModelNameSort());
					return modelList;
				}        
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:PopulateFWOSModModels reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:PopulateFWOSModModels reader lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Requests the date information associated with a FWOS modifier model is loaded.
		/// </summary>
		/// <param name="mmi">The FWOS modifier model object</param>
		static private void LoadFWOSModifierModelDates(FWOSModModelInfo mmi)
		{
			try
			{
				models_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					//clear out any old date entries
					mmi.ModelDateEntries.Clear();
					mmi.NeedsRebuilt = false;
					mmi.ModelDateEntries.Add(Include.UndefinedDay,mmi.FWOSModModelDefault);
					foreach (FWOSModModelEntryInfo mmei in mmi.ModelEntries)
					{
						if (mmei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
						{
							mmi.ContainsReoccurringDates = true;
						}
						if (mmei.DateRange.DateRangeType == eCalendarRangeType.Dynamic)
						{
							mmi.ContainsDynamicDates = true;
							if (mmei.DateRange.RelativeTo == eDateRangeRelativeTo.StoreOpen)
							{
								mmi.ContainsStoreDynamicDates = true;
								mmi.NeedsRebuilt = true;
							}
							if (mmei.DateRange.RelativeTo == eDateRangeRelativeTo.Plan)
							{
								mmi.ContainsPlanDynamicDates = true;
								mmi.NeedsRebuilt = true;
							}
						}
				
						ProfileList weekProfileList = Calendar.GetWeekRange(mmei.DateRange, null);
						if (mmei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
								if (!mmi.ModelDateEntries.Contains(weekProfile.WeekInYear))
								{
									mmi.ModelDateEntries.Add(weekProfile.WeekInYear,mmei.FWOSModModelEntryValue);
								}
							}
						}
						else
						{
							foreach (WeekProfile weekProfile in weekProfileList)
							{
								if (!mmi.ModelDateEntries.Contains(weekProfile.Key))
								{
									mmi.ModelDateEntries[weekProfile.Key] = mmei.FWOSModModelEntryValue;
								}
							}
						}
					}

                    // Begin TT#2307 - JSmith - Incorrect Stock Values
                    mmi.ModelDateEntriesLoadedByStore = false;
                    // End TT#2307
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					EventLog.WriteEntry("MIDHierarchyService", "LoadFWOSModifierModelDates error:" + ex.Message, EventLogEntryType.Error);
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					models_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:LoadFWOSModifierModelDates writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:LoadFWOSModifierModelDates writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        /// <summary>
        /// Requests the date information associated with a stock modifier model by store is loaded.
        /// </summary>
        /// <param name="smmi">The stock modifier model object</param>
        static private void LoadFWOSModifierModelDatesByStore(FWOSModModelInfo mmi, ProfileList aStoreList)
        {
            try
            {
                Hashtable modelDateEntries;
                ProfileList weekProfileList = null;
                models_rwl.AcquireWriterLock(WriterLockTimeOut);
                try
                {
                    //clear out any old date entries
                    mmi.ModelDateEntries = new Hashtable();

                    foreach (FWOSModModelEntryInfo mmei in mmi.ModelEntries)
                    {
                        if (mmei.DateRange.RelativeTo != eDateRangeRelativeTo.StoreOpen)
                        {
                            weekProfileList = Calendar.GetWeekRange(mmei.DateRange, null);
                        }

                        foreach (StoreProfile storeProfile in aStoreList)
                        {
                            modelDateEntries = (Hashtable)mmi.ModelDateEntries[storeProfile.Key];
                            if (modelDateEntries == null)
                            {
                                modelDateEntries = new Hashtable();
                                mmi.ModelDateEntries.Add(storeProfile.Key, modelDateEntries);
                                modelDateEntries.Add(Include.UndefinedDay, mmi.FWOSModModelDefault);
                            }

                            if (mmei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
                            {
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!modelDateEntries.Contains(weekProfile.WeekInYear))
                                    {
                                        modelDateEntries.Add(weekProfile.WeekInYear, mmei.FWOSModModelEntryValue);
                                    }
                                }
                            }
                            else if (mmei.DateRange.RelativeTo == eDateRangeRelativeTo.StoreOpen)
                            {
                                // Begin TT#91 MD - JSmith - Issue loading models from Hierarchy when date range is relative to store open
								//if (storeProfile.StockOpenDt != Include.UndefinedDate)
                                if (storeProfile.SellingOpenDt != Include.UndefinedDate)
                                // End TT#91 MD 
                                {
                                    WeekProfile mrsWeek = Calendar.GetWeek(storeProfile.SellingOpenDt);
                                    mmei.DateRange.InternalAnchorDate = mrsWeek;
                                }

                                weekProfileList = Calendar.GetWeekRange(mmei.DateRange, mmei.DateRange.InternalAnchorDate);
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!modelDateEntries.Contains(weekProfile.Key))
                                    {
                                        modelDateEntries[weekProfile.Key] = mmei.FWOSModModelEntryValue;
                                    }
                                }
                            }
                            else
                            {
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!mmi.ModelDateEntries.Contains(weekProfile.Key))
                                    {
                                        mmi.ModelDateEntries[weekProfile.Key] = mmei.FWOSModModelEntryValue;
                                    }
                                }
                            }
                            //mmi.ModelDateEntries.Add(storeProfile.Key, modelDateEntries);
                        }
                    }

                    mmi.NeedsRebuilt = false;
                    mmi.ModelDateEntriesLoadedByStore = true;
                }
                catch (Exception ex)
                {
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    EventLog.WriteEntry("MIDHierarchyService", "LoadFWOSModifierModelDatesByStore error:" + ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    // Ensure that the lock is released.
                    models_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:LoadFWOSModifierModelDatesByStore writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:LoadFWOSModifierModelDatesByStore writer lock has timed out");
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }
        // End TT#2307

        //BEGIN TT#108 - MD - DOConnell - FWOS Max Model Enhancment
        /// <summary>
        /// Adds or updates information about a FWOS modifier model
        /// </summary>
        /// <param name="mmp">Information about the FWOS modifier model</param>
        static public void FWOSMaxModelUpdate(FWOSMaxModelProfile mmp)
        {
            try
            {
                models_rwl.AcquireWriterLock(WriterLockTimeOut);
                try
                {
                    switch (mmp.ModelChangeType)
                    {
                        case eChangeType.none:
                            {
                                break;
                            }
                        case eChangeType.add:
                            {
                                FWOSMaxModelInfo mmi = new FWOSMaxModelInfo();
                                mmi.ModelRID = mmp.Key;
                                mmi.ModelID = mmp.ModelID;
                                mmi.FWOSMaxModelDefault = mmp.FWOSMaxModelDefault;
                                foreach (FWOSMaxModelEntry smme in mmp.ModelEntries)
                                {
                                    FWOSMaxModelEntryInfo modelEntry = new FWOSMaxModelEntryInfo();
                                    modelEntry.ModelEntrySeq = smme.ModelEntrySeq;
                                    modelEntry.FWOSMaxModelEntryValue = smme.FWOSMaxModelEntryValue;
                                    modelEntry.DateRange = smme.DateRange;
                                    mmi.ModelEntries.Add(modelEntry);
                                }
                                mmi.UpdateDateTime = DateTime.Now;
                                LoadFWOSMaxModelDates(mmi);
                                _FWOSMaxModelsByRID.Add(mmi.ModelRID, mmi);
                                _FWOSMaxModelsByID.Add(mmi.ModelID, mmi.ModelRID);
                                break;
                            }
                        case eChangeType.update:
                            {
                                FWOSMaxModelInfo mmi = (FWOSMaxModelInfo)_FWOSMaxModelsByRID[mmp.Key];
                                _FWOSMaxModelsByID.Remove(mmp.ModelID);
                                mmi.ModelRID = mmp.Key;
                                mmi.ModelID = mmp.ModelID;
                                mmi.FWOSMaxModelDefault = mmp.FWOSMaxModelDefault;
                                mmi.ModelEntries.Clear();
                                foreach (FWOSMaxModelEntry smme in mmp.ModelEntries)
                                {
                                    FWOSMaxModelEntryInfo modelEntry = new FWOSMaxModelEntryInfo();
                                    modelEntry.ModelEntrySeq = smme.ModelEntrySeq;
                                    modelEntry.FWOSMaxModelEntryValue = smme.FWOSMaxModelEntryValue;
                                    modelEntry.DateRange = smme.DateRange;
                                    mmi.ModelEntries.Add(modelEntry);
                                }
                                mmi.UpdateDateTime = DateTime.Now;
                                LoadFWOSMaxModelDates(mmi);
                                _FWOSMaxModelsByRID[mmp.Key] = mmi;
                                _FWOSMaxModelsByID.Add(mmi.ModelID, mmi.ModelRID);
                                break;
                            }
                        case eChangeType.delete:
                            {
                                FWOSMaxModelInfo mmi = (FWOSMaxModelInfo)_FWOSMaxModelsByRID[mmp.Key];
                                _FWOSMaxModelsByRID.Remove(mmp.Key);
                                _FWOSMaxModelsByID.Remove(mmi.ModelID);
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
                    EventLog.WriteEntry("MIDHierarchyService", "FWOSMaxModelUpdate error:" + ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    // Ensure that the lock is released.
                    models_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:FWOSMaxModelUpdate writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:FWOSMaxModelUpdate writer lock has timed out");
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

        /// <summary>
        /// Gets all information associated with a FWOS modifier model using the model name.
        /// </summary>
        /// <param name="ModelID">The model id of the FWOS modifier model</param>
        /// <returns>An instance of the FWOSModModelProfile class containing FWOS modifier model information</returns>
        static public FWOSMaxModelProfile GetFWOSMaxModelData(string ModelID)
        {
            int smmRID;
            try
            {
                models_rwl.AcquireReaderLock(ReaderLockTimeOut);
                try
                {
                    if (_FWOSMaxModelsByID.TryGetValue(ModelID, out smmRID))
                    {
                        return GetFWOSMaxModelData(smmRID);
                    }
                    else
                    {
                        FWOSMaxModelProfile mmp = new FWOSMaxModelProfile(Include.NoRID);
                        mmp.Key = Include.NoRID;  // not found
                        return mmp;
                    }
                }
                finally
                {
                    // Ensure that the lock is released.
                    models_rwl.ReleaseReaderLock();
                }
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                // The reader lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetFWOSMaxModelData reader lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:GetFWOSMaxModelData reader lock has timed out");
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        static public FWOSMaxModelProfile GetFWOSMaxModelData(int ModelRID)
        {
            try
            {
                return GetFWOSMaxModelData(ModelRID, null);
            }
            catch
            {
                throw;
            }
        }
        // End TT#2307

        /// <summary>
        /// Gets the information about a FWOS modifier model.
        /// </summary>
        /// <param name="ModelRID">The record id of the FWOS modifier model</param>
        /// <returns>An instance of the FWOSMaxModelProfile class containing FWOS modifier model information</returns>
        static public FWOSMaxModelProfile GetFWOSMaxModelData(int ModelRID, ProfileList aStoreList)
        {
            try
            {
                // Begin TT#2307 - JSmith - Incorrect Stock Values
                DateTime storeUpdateDate = DateTime.Now;
                // End TT#2307

                models_rwl.AcquireReaderLock(ReaderLockTimeOut);
                FWOSMaxModelInfo mmi;
                try
                {
                    if (!_FWOSMaxModelsByRID.TryGetValue(ModelRID, out mmi))
                    {
                        mmi = new FWOSMaxModelInfo();
                        mmi.ModelRID = Include.NoRID;
                    }
                    FWOSMaxModelProfile mmp = new FWOSMaxModelProfile(mmi.ModelRID);
                    mmp.ModelChangeType = eChangeType.none;
                    mmp.Key = mmi.ModelRID;
                    mmp.ModelID = mmi.ModelID;
                    mmp.FWOSMaxModelDefault = mmi.FWOSMaxModelDefault;
                    mmp.UpdateDateTime = mmi.UpdateDateTime;
                    foreach (FWOSMaxModelEntryInfo smmei in mmi.ModelEntries)
                    {
                        FWOSMaxModelEntry modelEntry = new FWOSMaxModelEntry();
                        modelEntry.ModelEntryChangeType = eChangeType.none;
                        modelEntry.ModelEntrySeq = smmei.ModelEntrySeq;
                        modelEntry.FWOSMaxModelEntryValue = smmei.FWOSMaxModelEntryValue;
                        modelEntry.DateRange = smmei.DateRange;
                        mmp.ModelEntries.Add(modelEntry);
                    }
                    mmp.ContainsDynamicDates = mmi.ContainsDynamicDates;
                    mmp.ContainsPlanDynamicDates = mmi.ContainsPlanDynamicDates;
                    mmp.ContainsReoccurringDates = mmi.ContainsReoccurringDates;
                    mmp.ContainsStoreDynamicDates = mmi.ContainsStoreDynamicDates;
                    // Begin TT#2307 - JSmith - Incorrect Stock Values
                    if (aStoreList != null &&
                        mmp.ContainsStoreDynamicDates &&
                        !mmi.ModelDateEntriesLoadedByStore)
                    {
                        LoadFWOSMaxModelDatesByStore(mmi, aStoreList);
                    }
                    mmp.NeedsRebuilt = mmi.NeedsRebuilt;
                    // End TT#2307

                    if (!mmp.NeedsRebuilt)
                    {
                        mmp.ModelDateEntries = (Hashtable)mmi.ModelDateEntries.Clone();
                    }
                    return mmp;
                }
                catch (Exception ex)
                {
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
                    throw;
                }
                finally
                {
                    // Ensure that the lock is released.
                    models_rwl.ReleaseReaderLock();
                }
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                // The reader lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetFWOSMaxModelData reader lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:GetFWOSMaxModelData reader lock has timed out");
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

        /// <summary>
        /// Gets the date and time FWOS modifier model was last updated.
        /// </summary>
        /// <param name="ModelRID">The record id of the FWOS modifier model</param>
        /// <returns>A string containing the date and time the model was last updated </returns>
        static public string GetFWOSMaxModelUpdateDateString(int ModelRID)
        {
            try
            {
                models_rwl.AcquireReaderLock(ReaderLockTimeOut);
                FWOSMaxModelInfo mmi;
                try
                {
                    if (!_FWOSMaxModelsByRID.TryGetValue(ModelRID, out mmi))
                    {
                        mmi = new FWOSMaxModelInfo();
                        mmi.ModelRID = Include.NoRID;
                    }

                    return mmi.UpdateDateTimeString;
                }
                catch (Exception ex)
                {
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
                    throw;
                }
                finally
                {
                    // Ensure that the lock is released.
                    models_rwl.ReleaseReaderLock();
                }
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                // The reader lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetFWOSMaxModelUpdateDateString reader lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:GetFWOSMaxModelUpdateDateString reader lock has timed out");
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

        /// <summary>
        /// Populates the model name list with FWOS modifier models information
        /// </summary>
        /// <returns></returns>
        static public ProfileList PopulateFWOSMaxModels()
        {
            try
            {
                models_rwl.AcquireReaderLock(ReaderLockTimeOut);
                try
                {
                    ModelNameList modelList = new ModelNameList(eProfileType.ModelName);
                    foreach (KeyValuePair<int, FWOSMaxModelInfo> FWOSMaxModel in _FWOSMaxModelsByRID)
                    {
                        FWOSMaxModelInfo mmi = FWOSMaxModel.Value;
                        ModelName mn = new ModelName(mmi.ModelRID);
                        mn.ModelID = mmi.ModelID;
                        modelList.Add(mn);
                    }
                    if (modelList.Count == 0)
                    {
                        ModelName mn = new ModelName(Include.NoRID);
                        mn.ModelID = " ";
                        modelList.Add(mn);
                    }
                    modelList.ArrayList.Sort(new ModelNameSort());
                    return modelList;
                }
                finally
                {
                    // Ensure that the lock is released.
                    models_rwl.ReleaseReaderLock();
                }
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                // The reader lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:PopulateFWOSMaxModels reader lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:PopulateFWOSMaxModels reader lock has timed out");
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

        /// <summary>
        /// Requests the date information associated with a FWOS modifier model is loaded.
        /// </summary>
        /// <param name="mmi">The FWOS modifier model object</param>
        static private void LoadFWOSMaxModelDates(FWOSMaxModelInfo mmi)
        {
            try
            {
                models_rwl.AcquireWriterLock(WriterLockTimeOut);
                try
                {
                    //clear out any old date entries
                    mmi.ModelDateEntries.Clear();
                    mmi.NeedsRebuilt = false;
                    mmi.ModelDateEntries.Add(Include.UndefinedDay, mmi.FWOSMaxModelDefault);
                    foreach (FWOSMaxModelEntryInfo mmei in mmi.ModelEntries)
                    {
                        if (mmei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
                        {
                            mmi.ContainsReoccurringDates = true;
                        }
                        if (mmei.DateRange.DateRangeType == eCalendarRangeType.Dynamic)
                        {
                            mmi.ContainsDynamicDates = true;
                            if (mmei.DateRange.RelativeTo == eDateRangeRelativeTo.StoreOpen)
                            {
                                mmi.ContainsStoreDynamicDates = true;
                                mmi.NeedsRebuilt = true;
                            }
                            if (mmei.DateRange.RelativeTo == eDateRangeRelativeTo.Plan)
                            {
                                mmi.ContainsPlanDynamicDates = true;
                                mmi.NeedsRebuilt = true;
                            }
                        }

                        ProfileList weekProfileList = Calendar.GetWeekRange(mmei.DateRange, null);
                        if (mmei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
                        {
                            foreach (WeekProfile weekProfile in weekProfileList)
                            {
                                if (!mmi.ModelDateEntries.Contains(weekProfile.WeekInYear))
                                {
                                    mmi.ModelDateEntries.Add(weekProfile.WeekInYear, mmei.FWOSMaxModelEntryValue);
                                }
                            }
                        }
                        else
                        {
                            foreach (WeekProfile weekProfile in weekProfileList)
                            {
                                if (!mmi.ModelDateEntries.Contains(weekProfile.Key))
                                {
                                    mmi.ModelDateEntries[weekProfile.Key] = mmei.FWOSMaxModelEntryValue;
                                }
                            }
                        }
                    }

                    // Begin TT#2307 - JSmith - Incorrect Stock Values
                    mmi.ModelDateEntriesLoadedByStore = false;
                    // End TT#2307
                }
                catch (Exception ex)
                {
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
                    EventLog.WriteEntry("MIDHierarchyService", "LoadFWOSMaxModelDates error:" + ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    // Ensure that the lock is released.
                    models_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:LoadFWOSMaxModelDates writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:LoadFWOSMaxModelDates writer lock has timed out");
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        /// <summary>
        /// Requests the date information associated with a stock modifier model by store is loaded.
        /// </summary>
        /// <param name="smmi">The stock modifier model object</param>
        static private void LoadFWOSMaxModelDatesByStore(FWOSMaxModelInfo mmi, ProfileList aStoreList)
        {
            try
            {
                Hashtable modelDateEntries;
                ProfileList weekProfileList = null;
                models_rwl.AcquireWriterLock(WriterLockTimeOut);
                try
                {
                    //clear out any old date entries
                    mmi.ModelDateEntries = new Hashtable();

                    foreach (FWOSMaxModelEntryInfo mmei in mmi.ModelEntries)
                    {
                        if (mmei.DateRange.RelativeTo != eDateRangeRelativeTo.StoreOpen)
                        {
                            weekProfileList = Calendar.GetWeekRange(mmei.DateRange, null);
                        }

                        foreach (StoreProfile storeProfile in aStoreList)
                        {
                            modelDateEntries = (Hashtable)mmi.ModelDateEntries[storeProfile.Key];
                            if (modelDateEntries == null)
                            {
                                modelDateEntries = new Hashtable();
                                mmi.ModelDateEntries.Add(storeProfile.Key, modelDateEntries);
                                modelDateEntries.Add(Include.UndefinedDay, mmi.FWOSMaxModelDefault);
                            }

                            if (mmei.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
                            {
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!modelDateEntries.Contains(weekProfile.WeekInYear))
                                    {
                                        modelDateEntries.Add(weekProfile.WeekInYear, mmei.FWOSMaxModelEntryValue);
                                    }
                                }
                            }
                            else if (mmei.DateRange.RelativeTo == eDateRangeRelativeTo.StoreOpen)
                            {
                                // Begin TT#91 MD - JSmith - Issue loading models from Hierarchy when date range is relative to store open
                                //if (storeProfile.StockOpenDt != Include.UndefinedDate)
                                if (storeProfile.SellingOpenDt != Include.UndefinedDate)
                                // End TT#91 MD 
                                {
                                    WeekProfile mrsWeek = Calendar.GetWeek(storeProfile.SellingOpenDt);
                                    mmei.DateRange.InternalAnchorDate = mrsWeek;
                                }

                                weekProfileList = Calendar.GetWeekRange(mmei.DateRange, mmei.DateRange.InternalAnchorDate);
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!modelDateEntries.Contains(weekProfile.Key))
                                    {
                                        modelDateEntries[weekProfile.Key] = mmei.FWOSMaxModelEntryValue;
                                    }
                                }
                            }
                            else
                            {
                                foreach (WeekProfile weekProfile in weekProfileList)
                                {
                                    if (!mmi.ModelDateEntries.Contains(weekProfile.Key))
                                    {
                                        mmi.ModelDateEntries[weekProfile.Key] = mmei.FWOSMaxModelEntryValue;
                                    }
                                }
                            }
                            //mmi.ModelDateEntries.Add(storeProfile.Key, modelDateEntries);
                        }
                    }

                    mmi.NeedsRebuilt = false;
                    mmi.ModelDateEntriesLoadedByStore = true;
                }
                catch (Exception ex)
                {
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    EventLog.WriteEntry("MIDHierarchyService", "LoadFWOSMaxModelDatesByStore error:" + ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    // Ensure that the lock is released.
                    models_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:LoadFWOSMaxModelDatesByStore writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:LoadFWOSMaxModelDatesByStore writer lock has timed out");
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }
        // End TT#2307

        //END TT#108 - MD - DOConnell - FWOS Max Model Enahncment


		/// <summary>
		/// Gets the information about a product characteristic.
		/// </summary>
		/// <param name="aProductCharRID">The record id of the product characteristic</param>
		/// <returns>An instance of the ProductCharInfo class</returns>
		static public ProductCharInfo GetProductCharInfo(int aProductCharRID)
		{
			try
			{
				characteristic_rwl.AcquireReaderLock(ReaderLockTimeOut);
				ProductCharInfo pci = null;
				try
				{
                    if (_productCharByRID.ContainsKey(aProductCharRID))
                    {
                        pci = (ProductCharInfo)_productCharByRID[aProductCharRID];
                    }
					if (pci == null)
					{
						pci = new ProductCharInfo();
					}

					return pci;
				}
				catch (Exception ex)
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					characteristic_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetProductCharInfo reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:GetProductCharInfo reader lock has timed out");
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static public int GetProductCharRIDByID(string aProductCharID)
		{
			try
			{
				object RID = null;
				lock (_productChar_lock.SyncRoot)
				{
                    if (_productCharByID.ContainsKey(aProductCharID))
                    {
                        RID = _productCharByID[aProductCharID];
                    }
				}

				if (RID != null)
				{
					return Convert.ToInt32(RID);
				}
				return Include.NoRID;
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the information about a product characteristic.
		/// </summary>
		/// <param name="aProductCharValueRID">The record id of the product characteristic value</param>
		/// <returns>An instance of the ProductCharInfo class</returns>
		static public ProductCharValueInfo GetProductCharValueInfo(int aProductCharValueRID)
		{
			try
			{
				characteristic_rwl.AcquireReaderLock(ReaderLockTimeOut);
				ProductCharValueInfo pcvi = null;
				try
				{
                    if (_productCharValueByRID.ContainsKey(aProductCharValueRID))
                    {
                        pcvi = (ProductCharValueInfo)_productCharValueByRID[aProductCharValueRID];
                    }
					if (pcvi == null)
					{
						pcvi = new ProductCharValueInfo();
					}

					return pcvi;
				}
				catch (Exception ex)
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					characteristic_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetProductCharValueInfo reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:GetProductCharValueInfo reader lock has timed out");
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Adds or updates information about a stores eligibility
		/// </summary>
		/// <param name="sel">An instance of the StoreEligibilityList class containing instances of the StoreEligibilityProfile class</param>
		/// <param name="nodeRID">The record id of the node</param>
		static public void StoreEligibilityUpdate(int nodeRID, StoreEligibilityList sel)
		{
            NodeEligibilityInfo nei = null;
			try
			{
				eligibility_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
                    if (_eligibilityByRID.ContainsKey(nodeRID))
                    {
                        nei = (NodeEligibilityInfo)_eligibilityByRID[nodeRID];
                    }
					if (nei == null)
					{
						nei = new NodeEligibilityInfo();
					}
					
					StoreEligibilityInfo sei;
					bool found = false;
					foreach (StoreEligibilityProfile sep in sel)
					{
						if (sep.StoreEligChangeType != eChangeType.none)
						{

							if (nei.StoreEligibility.Contains(sep.Key))
							{
								sei = (StoreEligibilityInfo)nei.StoreEligibility[sep.Key];
								found = true;
							}
							else
							{
								sei = new StoreEligibilityInfo();
								found = false;
							}

							if (sep.StoreEligChangeType == eChangeType.delete)
							{
								if (found)
								{
									nei.StoreEligibility.Remove(sei.StoreRID);
								}
							}
							else
							{
								sei.StoreRID = sep.Key;
								sei.EligModelRID = sep.EligModelRID;
								sei.StoreIneligible = sep.StoreIneligible;
								sei.StkModType = sep.StkModType;
								sei.StkModModelRID = sep.StkModModelRID;
								sei.StkModPct = sep.StkModPct;
								sei.SlsModType = sep.SlsModType;
								sei.SlsModModelRID = sep.SlsModModelRID;
								sei.SlsModPct = sep.SlsModPct;
								sei.FWOSModType = sep.FWOSModType;
								sei.FWOSModModelRID = sep.FWOSModModelRID;
								sei.FWOSModPct = sep.FWOSModPct;
								sei.SimStoreType = sep.SimStoreType;
								if (sep.SimStoresChanged)
								{
									sei.SimStores.Clear();
									if (sep.SimStores == null)
									{
										sei.SimStoreType = eSimilarStoreType.None;
									}
									else
									{
										foreach (int simStore in sep.SimStores)
										{
											sei.SimStores.Add(simStore);
										}
									}
								}
								if (sei.SimStores.Count == 0)
								{
									sei.SimStoreType = eSimilarStoreType.None;
								}
								sei.SimStoreRatio = sep.SimStoreRatio;
								sei.SimStoreUntilDateRangeRID = sep.SimStoreUntilDateRangeRID;
								if (sep.SimStoreUntilDateRangeRID != Include.NoRID)
								{
									sei.DateRangeProfile = Calendar.GetDateRange(sep.SimStoreUntilDateRangeRID);
								}

								if (sep.EligIsInherited)
								{
									sei.UseStoreEligibility = false;
								}
								else
								{
									sei.UseStoreEligibility = true;
								}

								// temporarily set flag to false if eligibility not explicitly set so will be chased
								if (sei.EligModelRID == Include.NoRID && 
									!sei.StoreIneligible)
								{
									sei.UseStoreEligibility = false;
								}

								sei.PresPlusSalesIsSet = sep.PresPlusSalesIsSet;
								sei.PresPlusSalesInd = sep.PresPlusSalesInd;

								if (!found)
								{
									nei.StoreEligibility.Add(sei.StoreRID, sei);
								}
								else
								{
									nei.StoreEligibility[sei.StoreRID] = sei;
								}
								//BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement
                                if (sei.StkLeadWeeks != Include.NoRID)
                                {
                                    sei.StkLeadWeeks = sep.StkLeadWeeks;
                                }
								//END TT#44 - MD - DOConnell - New Store Forecasting Enhancement
							}
						}
					}
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					EventLog.WriteEntry("MIDHierarchyService", "StoreEligibilityUpdate error:" + ex.Message, EventLogEntryType.Error);
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					eligibility_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:StoreEligibilityUpdate writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:StoreEligibilityUpdate writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the store eligibility for the node.
		/// </summary>
		/// <param name="storeList">The list of stores requiring eligibility</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="chaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns>An instance of the StoreEligibilityList class containing instances of the StoreEligibilityProfile class</returns>
        static public StoreEligibilityList GetStoreEligibilityList(ProfileList storeList, int nodeRID, bool chaseHierarchy, bool forCopy)
        {
            try
            {
                bool setInheritance = false;
                int storeCount = 0;
                NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
                NodeAncestorList nal = GetNodeAncestorList(nodeRID, ni.HomeHierarchyRID);
                StoreEligibilityList sel = new StoreEligibilityList(eProfileType.StoreEligibility);
                foreach (NodeAncestorProfile nap in nal)
                {
                    sel = GetStoreEligibility(storeList, nap.Key, sel, forCopy, setInheritance, ref storeCount);
                    if (storeCount == storeList.Count ||	//  stop lookup if have all settings for each store
                        !chaseHierarchy)					//  or stop on first node if copy
                    {
                        break;
                    }
                    setInheritance = true;
                }
                return sel;
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

		/// <summary>
		/// Gets the store eligibility for the node.
		/// </summary>
		/// <param name="storeList">The list of stores requiring eligibility</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <param name="sel">The list containing the eligibilities for the stores</param>
		/// <param name="setInheritance">Identifies if inheritance information should be set</param>
		/// <param name="storeCount">The number of stores that have all fields set</param>
		/// <returns></returns>
		static private StoreEligibilityList GetStoreEligibility(ProfileList storeList, int nodeRID, 
			StoreEligibilityList sel, bool forCopy, bool setInheritance, ref int storeCount)
		 {
			NodeEligibilityInfo nei = null;
			try
			{
				eligibility_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
                    if (_eligibilityByRID.ContainsKey(nodeRID))
                    {
                        nei = (NodeEligibilityInfo)_eligibilityByRID[nodeRID];
                    }
				}        
				finally
				{
					// Ensure that the lock is released.
					eligibility_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetStoreEligibility reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetStoreEligibility reader lock has timed out");
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}

			try
			{
				if (nei == null)
				{
					nei = new NodeEligibilityInfo();
					LoadStoreEligibility(nodeRID, nei, storeList);
					UpdateEligibilityHash(nodeRID, nei);
				}

				StoreEligibilityInfo sei;
				StoreProfile storeProfile;
				EligModelInfo emi = new EligModelInfo();
				StkModModelInfo stkmodmi = new StkModModelInfo();
				SlsModModelInfo slsmodmi = new SlsModModelInfo();
				FWOSModModelInfo FWOSmodmi = new FWOSModModelInfo();
				foreach (DictionaryEntry val in nei.StoreEligibility)
				{
					sei = (StoreEligibilityInfo)val.Value;
					if (storeList.Contains(sei.StoreRID))  // Do you need this store
					{
						StoreEligibilityProfile sep;
						bool newRecord = true;
						bool setSomething = false;
						if (!sel.Contains(sei.StoreRID))  // Do you already have this store
						{
							sep = new StoreEligibilityProfile(sei.StoreRID);
						}
						else
						{
							sep = (StoreEligibilityProfile)sel.FindKey(sei.StoreRID);
							newRecord = false;
						}
						storeProfile = (StoreProfile)storeList.FindKey(sei.StoreRID);

						// check eligibility
						if (!sep.EligIsSet)
						{
							if ((sep.EligType == eEligibilitySettingType.None && sei.EligModelRID > 0) ||
								(sep.EligType == eEligibilitySettingType.None && sei.StoreIneligible) ||
								sei.UseStoreEligibility)
							{
								setSomething = true;
								if (sei.StoreIneligible)
								{
									sep.StoreIneligible = sei.StoreIneligible;
									sep.EligType = eEligibilitySettingType.SetIneligible;
									sep.EligIsSet = true;
								}
								else if (sei.EligModelRID > Include.NoRID)
								{
									sep.EligModelRID = sei.EligModelRID;
									emi = (EligModelInfo)_eligModelsByRID[sei.EligModelRID];
									sep.EligModelName = emi.ModelID; 
									sep.EligType = eEligibilitySettingType.Model;
									sep.EligIsSet = true;
								}
								else if (sei.UseStoreEligibility)
								{
									sep.StoreIneligible = sei.StoreIneligible;
									sep.EligType = eEligibilitySettingType.SetEligible;
									sep.EligIsSet = true;
								}
							
								if (!forCopy)
								{
									if (setInheritance)
									{
										sep.EligIsInherited = true;
										sep.EligInheritedFromNodeRID = nodeRID;
									}
									else
									{
										sep.RecordExists = true;
									}
								}
							}
						}

						// check stock modifier
						if (sep.StkModType == eModifierType.None &&
							sei.StkModType != eModifierType.None)
						{
							setSomething = true;
							sep.StkModType = sei.StkModType;
							sep.StkModModelRID = sei.StkModModelRID;
							if (sep.StkModType == eModifierType.Model)
							{
								stkmodmi = (StkModModelInfo)_stkModModelsByRID[sei.StkModModelRID];
								sep.StkModModelName = stkmodmi.ModelID;
							}
							sep.StkModPct = sei.StkModPct;
							if (!forCopy)
							{
								if (setInheritance)
								{
									sep.StkModIsInherited = true;
									sep.StkModInheritedFromNodeRID = nodeRID;
								}
								else
								{
									sep.RecordExists = true;
								}
							}
						}

						// check sales modifier
						if (sep.SlsModType == eModifierType.None &&
							sei.SlsModType != eModifierType.None)
						{
							setSomething = true;
							sep.SlsModType = sei.SlsModType;
							sep.SlsModModelRID = sei.SlsModModelRID;
							if (sep.SlsModType == eModifierType.Model)
							{
								slsmodmi = (SlsModModelInfo)_slsModModelsByRID[sei.SlsModModelRID];
								sep.SlsModModelName = slsmodmi.ModelID;
							}
							sep.SlsModPct = sei.SlsModPct;
							if (!forCopy)
							{
								if (setInheritance)
								{
									sep.SlsModIsInherited = true;
									sep.SlsModInheritedFromNodeRID = nodeRID;
								}
								else
								{
									sep.RecordExists = true;
								}
							}
						}

						// check FWOS modifier
						if (sep.FWOSModType == eModifierType.None &&
							sei.FWOSModType != eModifierType.None)
						{
							setSomething = true;
							sep.FWOSModType = sei.FWOSModType;
							sep.FWOSModModelRID = sei.FWOSModModelRID;
							if (sep.FWOSModType == eModifierType.Model)
							{
								FWOSmodmi = (FWOSModModelInfo)_FWOSModModelsByRID[sei.FWOSModModelRID];
								sep.FWOSModModelName = FWOSmodmi.ModelID;
							}
							sep.FWOSModPct = sei.FWOSModPct;
							if (!forCopy)
							{
								if (setInheritance)
								{
									sep.FWOSModIsInherited = true;
									sep.FWOSModInheritedFromNodeRID = nodeRID;
								}
								else
								{
									sep.RecordExists = true;
								}
							}
						}

						if (sei.SimStoreType == eSimilarStoreType.Stores &&
							sei.SimStores.Count == 0)
						{
							sei.SimStoreType = eSimilarStoreType.None;
						}

						// check similar store
						if (sep.SimStoreType == eSimilarStoreType.None &&
							sei.SimStoreType != eSimilarStoreType.None)
						{
							setSomething = true;
							sep.SimStoreType = sei.SimStoreType;
							switch (sei.SimStoreType)
							{
								case eSimilarStoreType.Stores:
									foreach (int simStore in sei.SimStores)
									{
										sep.SimStores.Add(simStore);
									}
									break;
								default:
									break;
							}
							sep.SimStoreRatio = sei.SimStoreRatio;
							sep.SimStoreUntilDateRangeRID = sei.SimStoreUntilDateRangeRID;
							if (sep.SimStoreUntilDateRangeRID != Include.NoRID)
							{
                                string dateRangeFrom = "sei.DateRangeProfile";
								DateRangeProfile drp = null;
								if (sei.SimStoreUntilDateRangeRID != sei.OrigSimStoreUntilDateRangeRID)
								{
                                    dateRangeFrom = "sep.SimStoreUntilDateRangeRID";
									drp = Calendar.GetDateRange(sep.SimStoreUntilDateRangeRID);
									sei.DateRangeProfile = drp;
								}
								else
								{
									drp = sei.DateRangeProfile;
								}
								if (storeProfile.SellingOpenDt != Include.UndefinedDate)
								{
									WeekProfile mrsWeek = Calendar.GetWeek(storeProfile.SellingOpenDt);
									drp.InternalAnchorDate = mrsWeek;
								}
								sep.SimStoreWeekList = Calendar.GetDateRangeWeeks(drp, drp.InternalAnchorDate);
                                // temp code for debugging
                                if (sep.SimStoreWeekList.Count == 0)
                                {
                                    bool retry = true;
                                    int retryCount = 0;
                                    while (retry)
                                    {
                                        Thread.Sleep(2000);
                                        ++retryCount;
                                        sep.SimStoreWeekList = Calendar.GetDateRangeWeeks(drp, drp.InternalAnchorDate);
                                        if (sep.SimStoreWeekList.Count != 0)
                                        {
                                            retry = false;
                                        }
                                        else if (retryCount > 4)
                                        {
                                            retry = false;
                                        }
                                    }
                                    if (sep.SimStoreWeekList.Count == 0)
                                    {
                                        Audit.Add_Msg(eMIDMessageLevel.Error, "storeProfile.StoreId:" + storeProfile.StoreId, "HierarchyServerGlobal.GetStoreEligibility");
                                        Audit.Add_Msg(eMIDMessageLevel.Error, "dateRangeFrom:" + dateRangeFrom, "HierarchyServerGlobal.GetStoreEligibility");
                                        Audit.Add_Msg(eMIDMessageLevel.Error, "storeProfile.SellingOpenDt:" + storeProfile.SellingOpenDt.ToString(), "HierarchyServerGlobal.GetStoreEligibility");
                                        Audit.Add_Msg(eMIDMessageLevel.Error, "drp.DateRangeType.ToString():" + drp.DateRangeType.ToString(), "HierarchyServerGlobal.GetStoreEligibility");
                                        Audit.Add_Msg(eMIDMessageLevel.Error, "drp.RelativeTo.ToString():" + drp.RelativeTo.ToString(), "HierarchyServerGlobal.GetStoreEligibility");
                                        Audit.Add_Msg(eMIDMessageLevel.Error, "drp.SelectedDateType.ToString():" + drp.SelectedDateType.ToString(), "HierarchyServerGlobal.GetStoreEligibility");
                                        Audit.Add_Msg(eMIDMessageLevel.Error, "drp.StartDateKey.ToString():" + drp.StartDateKey.ToString(), "HierarchyServerGlobal.GetStoreEligibility");
                                        Audit.Add_Msg(eMIDMessageLevel.Error, "drp.InternalAnchorDate:" + drp.InternalAnchorDate.ToString(), "HierarchyServerGlobal.GetStoreEligibility");
                                    }
                                }
                                // temp code for debugging
								sep.SimStoreDisplayDate = Calendar.GetDisplayDate(drp);
							}
							if (!forCopy)
							{
								if (setInheritance)
								{
									sep.SimStoreIsInherited = true;
									sep.SimStoreInheritedFromNodeRID = nodeRID;
								}
								else
								{
									sep.RecordExists = true;
								}
							}
						}

						// check presentation plus sales
						if (!sep.PresPlusSalesIsSet)
						{
							if (sei.PresPlusSalesIsSet)
							{
								setSomething = true;
								sep.PresPlusSalesInd = sei.PresPlusSalesInd;
								sep.PresPlusSalesIsSet = true;
							
								if (!forCopy)
								{
									if (setInheritance)
									{
										sep.PresPlusSalesIsInherited = true;
										sep.PresPlusSalesInheritedFromNodeRID = nodeRID;
									}
									else
									{
										sep.RecordExists = true;
									}
								}
							}
						}

                        //BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement
                        
                        if (sei.StkLeadWeeks != Include.NoRID)
                        {
                            sep.StkLeadWeeks = sei.StkLeadWeeks;
                            if (!forCopy)
                            {
                                if (setInheritance)
                                {
                                    sep.StkLeadWeeksInherited = true;
                                    sep.StkLeadWeeksInheritedRid = nodeRID;
                                }
                                else
                                {
                                    sep.RecordExists = true;
                                }
                            }
                        }
                        //END TT#44 - MD - DOConnell - New Store Forecasting Enhancement

						if (forCopy)
						{
							sep.StoreEligChangeType = eChangeType.add;
						}

						if (setSomething &&
							sep.EligType != eEligibilitySettingType.None &&
							sep.StkModType != eModifierType.None &&
							sep.SlsModType != eModifierType.None &&
							sep.FWOSModType != eModifierType.None &&
							sep.SimStoreType != eSimilarStoreType.None)
						{
							++storeCount;
						}

						if (!forCopy)
						{
							if (!setInheritance)
							{
								sep.RecordExists = true;
							}
						}

						if (newRecord)
						{
							sel.Add(sep);
						}
						else
						{
							sel.Update(sep);
						}
					}
				}
				return sel;
			}        
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}        
		}

		/// <summary>
		/// Loads the store eligibility for the node.
		/// </summary>
		///<remarks>
		///Eligibility is loaded as needed for performance.
		///</remarks>
		static private void LoadStoreEligibility(int aNodeRID, NodeEligibilityInfo nei, ProfileList aStoreList)
		{
			try
			{
				int simStoreRID;
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.StoreEligibility_Read(aNodeRID);
                //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                //Get similiar store data in one database call
                foreach (DataRow dr in dt.Rows)
                {
                    StoreEligibilityInfo sei = new StoreEligibilityInfo();

                    sei.StoreRID = Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture);
                }
                //DataTable ssdt = mhd.SimilarStore_Read(aNodeRID, sei.StoreRID);
                //DataTable ssdt = mhd.SimilarStore_GetStoreRIDs(aNodeRID, mhd.MakeStoreRidParameterDataTable(dt, "ST_RID"));
                DataTable ssdt = mhd.SimilarStore_GetStoreRIDs(aNodeRID);
                //End TT#827-MD -jsobek -Allocation Reviews Performance

				foreach(DataRow dr in dt.Rows)
				{
					StoreEligibilityInfo sei = new StoreEligibilityInfo();

					sei.StoreRID = Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture);
					sei.EligModelRID = Convert.ToInt32(dr["EM_RID"], CultureInfo.CurrentUICulture);
					if (dr["USE_ELIGIBILITY"] != DBNull.Value)
					{
						sei.UseStoreEligibility = Include.ConvertCharToBool(Convert.ToChar(dr["USE_ELIGIBILITY"], CultureInfo.CurrentUICulture));
					}
					if (Convert.ToInt32(dr["INELIGIBLE"], CultureInfo.CurrentUICulture) == 0)
					{
						sei.StoreIneligible = false;
					}
					else
					{
						sei.StoreIneligible = true;
					}

					// temporarily set flag to false if eligibility not explicitly set so will be chased
					if (sei.EligModelRID == Include.NoRID && 
						!sei.StoreIneligible)
					{
						sei.UseStoreEligibility = false;
					}

					sei.StkModType = (eModifierType)(Convert.ToInt32(dr["STKMOD_TYPE"], CultureInfo.CurrentUICulture));
					sei.StkModModelRID = Convert.ToInt32(dr["STKMOD_RID"], CultureInfo.CurrentUICulture);
					if (sei.StkModType == eModifierType.Percent)
					{
						sei.StkModPct = Convert.ToDouble(dr["STKMOD_PCT"], CultureInfo.CurrentUICulture);
					}
					
					sei.SlsModType = (eModifierType)(Convert.ToInt32(dr["SLSMOD_TYPE"], CultureInfo.CurrentUICulture));
					sei.SlsModModelRID = Convert.ToInt32(dr["SLSMOD_RID"], CultureInfo.CurrentUICulture);
					if (sei.SlsModType == eModifierType.Percent)
					{
						sei.SlsModPct = Convert.ToDouble(dr["SLSMOD_PCT"], CultureInfo.CurrentUICulture);
					}
				
					sei.FWOSModType = (eModifierType)(Convert.ToInt32(dr["FWOSMOD_TYPE"], CultureInfo.CurrentUICulture));
					sei.FWOSModModelRID = Convert.ToInt32(dr["FWOSMOD_RID"], CultureInfo.CurrentUICulture);
					if (sei.FWOSModType == eModifierType.Percent)
					{
						sei.FWOSModPct = Convert.ToDouble(dr["FWOSMOD_PCT"], CultureInfo.CurrentUICulture);
					}

					sei.SimStoreType = (eSimilarStoreType)(Convert.ToInt32(dr["SIMILAR_STORE_TYPE"], CultureInfo.CurrentUICulture));
					switch (sei.SimStoreType)
					{
						case eSimilarStoreType.Stores:
                            //DataTable ssdt = mhd.SimilarStore_Read(aNodeRID, sei.StoreRID); //TT#827-MD -jsobek -Allocation Reviews Performance
                            DataRow[] drssFind = ssdt.Select("ST_RID=" + sei.StoreRID); //TT#827-MD -jsobek -Allocation Reviews Performance
                            foreach (DataRow ssdr in drssFind) //TT#827-MD -jsobek -Allocation Reviews Performance
							{
								simStoreRID = Convert.ToInt32(ssdr["SS_RID"], CultureInfo.CurrentUICulture);
								if (aStoreList.Contains(simStoreRID))
								{
									sei.SimStores.Add(simStoreRID);
								}
							}
							if (sei.SimStores.Count == 0)
							{
								sei.SimStoreType = eSimilarStoreType.None;
							}
							break;
						default:
							break;
					}
					sei.SimStoreRatio = Convert.ToDouble(dr["SIMILAR_STORE_RATIO"], CultureInfo.CurrentUICulture);
					sei.SimStoreUntilDateRangeRID = Convert.ToInt32(dr["UNTIL_DATE"], CultureInfo.CurrentUICulture);
					sei.OrigSimStoreUntilDateRangeRID = sei.SimStoreUntilDateRangeRID;
					if (sei.SimStoreUntilDateRangeRID != Include.NoRID)
					{
						sei.DateRangeProfile.Key = Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture);
						sei.DateRangeProfile.StartDateKey = Convert.ToInt32(dr["CDR_START"], CultureInfo.CurrentUICulture);
						sei.DateRangeProfile.EndDateKey = Convert.ToInt32(dr["CDR_END"], CultureInfo.CurrentUICulture);
						sei.DateRangeProfile.DateRangeType = (eCalendarRangeType)Convert.ToInt32(dr["CDR_RANGE_TYPE_ID"], CultureInfo.CurrentUICulture);
						sei.DateRangeProfile.SelectedDateType = (eCalendarDateType)Convert.ToInt32(dr["CDR_DATE_TYPE_ID"], CultureInfo.CurrentUICulture);
						sei.DateRangeProfile.RelativeTo = (eDateRangeRelativeTo)Convert.ToInt32(dr["CDR_RELATIVE_TO"], CultureInfo.CurrentUICulture);
						if (dr["CDR_NAME"] != DBNull.Value)
							sei.DateRangeProfile.Name	= (string)dr["CDR_NAME"];
					}
					if (dr["PRESENTATION_PLUS_SALES_IND"] != DBNull.Value)
					{
						sei.PresPlusSalesInd = Include.ConvertCharToBool(Convert.ToChar(dr["PRESENTATION_PLUS_SALES_IND"], CultureInfo.CurrentUICulture));
						sei.PresPlusSalesIsSet = true;
					}
                    //BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement
                    if (dr["STOCK_LEAD_WEEKS"] != DBNull.Value)
                    {
                      sei.StkLeadWeeks = Convert.ToInt32(dr["STOCK_LEAD_WEEKS"], CultureInfo.CurrentUICulture);
                    }
                    //END TT#44 - MD - DOConnell - New Store Forecasting Enhancement
					nei.StoreEligibility.Add(sei.StoreRID, sei);
				}
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static private void UpdateEligibilityHash(int aNodeRID, NodeEligibilityInfo aNodeEligibilityInfo)
		{
			try
			{
				eligibility_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					if (!_eligibilityByRID.ContainsKey(aNodeRID))
					{
						_eligibilityByRID.Add(aNodeRID, aNodeEligibilityInfo);
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					eligibility_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:UpdateEligibilityHash writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:UpdateEligibilityHash writer lock has timed out");
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		/// <summary>
		/// Adds or updates information about a stores size curve similar store
		/// </summary>
		/// <param name="sccl">An instance of the SizeCurveCriteriaList class containing instances of the SizeCurveCriteriaProfile class</param>
		/// <param name="nodeRID">The record id of the node</param>
		static public void SizeCurveCriteriaUpdate(int nodeRID, SizeCurveCriteriaList sccl)
		{
			NodeSizeCurveCriteriaInfo nscci = null;
			SizeCurveCriteriaInfo scci = null;
			bool found = false;

			try
			{
				sizeCurveCriteria_rwl.AcquireWriterLock(WriterLockTimeOut);

				try
				{
                    if (_sizeCurveCriteriaByRID.ContainsKey(nodeRID))
                    {
                        nscci = (NodeSizeCurveCriteriaInfo)_sizeCurveCriteriaByRID[nodeRID];
                    }

					if (nscci == null)
					{
						nscci = new NodeSizeCurveCriteriaInfo();
					}

					foreach (SizeCurveCriteriaProfile sccp in sccl)
					{
						if (sccp.CriteriaChangeType != eChangeType.none)
						{
							if (nscci.SizeCurveCriteria.Contains(sccp.Key))
							{
								scci = (SizeCurveCriteriaInfo)nscci.SizeCurveCriteria[sccp.Key];
								found = true;
							}
							else
							{
								scci = new SizeCurveCriteriaInfo(sccp.Key);
								found = false;
							}

							if (sccp.CriteriaChangeType == eChangeType.delete)
							{
								if (found)
								{
									nscci.SizeCurveCriteria.Remove(sccp.Key);
								}
							}
							else
							{
								scci.LevelType = sccp.CriteriaLevelType;
								scci.LevelRID = sccp.CriteriaLevelRID;
								scci.LevelSequence = sccp.CriteriaLevelSequence;
								scci.LevelOffset = sccp.CriteriaLevelOffset;
								scci.DateRID = sccp.CriteriaDateRID;
								scci.ApplyLostSalesInd = sccp.CriteriaApplyLostSalesInd;
								scci.OLLRID = sccp.CriteriaOLLRID;
								scci.CustomOLLRID = sccp.CriteriaCustomOLLRID;
								scci.SizeGroupRID = sccp.CriteriaSizeGroupRID;
								scci.CurveName = sccp.CriteriaCurveName;
								//Begin TT#1076 - JScott - Size Curves by Set
								scci.SgRID = sccp.CriteriaSgRID;
								//End TT#1076 - JScott - Size Curves by Set

								if (!found)
								{
									nscci.SizeCurveCriteria.Add(sccp.Key, scci);
								}
								else
								{
									nscci.SizeCurveCriteria[sccp.Key] = scci;
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					if (Audit != null)
					{
						Audit.Log_Exception(ex);
					}

					EventLog.WriteEntry("MIDHierarchyService", "SizeCurveCriteriaUpdate error:" + ex.Message, EventLogEntryType.Error);
					throw;
				}
				finally
				{
					sizeCurveCriteria_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:SizeCurveCriteriaUpdate writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:SizeCurveCriteriaUpdate writer lock has timed out");
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		static public SizeCurveCriteriaList GetSizeCurveCriteriaList(int nodeRID, bool forCopy, bool useApplyFrom)
		{
			NodeInfo ni;
			NodeAncestorList homeNal;
			SortedList allAncestorList;
			NodeAncestorList hierNal;
			SizeCurveCriteriaList sccl;
			SizeCurveCriteriaList tempSccl;
			int sequence;
            List<int> processedNodeList;

			try
			{
				sccl = new SizeCurveCriteriaList();
				processedNodeList = new List<int>();

				// Process the Home hierarchy first

				tempSccl = new SizeCurveCriteriaList();
				ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
                homeNal = GetNodeAncestorList(nodeRID, ni.HomeHierarchyRID, useApplyFrom);

				foreach (NodeAncestorProfile nap in homeNal)
				{
					tempSccl = GetSizeCurveCriteria(nap.Key, tempSccl, forCopy, nap.Key != nodeRID);
                    processedNodeList.Add(nap.Key);
				}

				sequence = tempSccl.Count;

				foreach (SizeCurveCriteriaProfile sccp in tempSccl)
				{
					sccp.CriteriaSequence = sequence--;
					sccl.Add(sccp);
				}

				// Process Alternate hierarchies

				tempSccl.Clear();
				allAncestorList = GetAllNodeAncestors(nodeRID, false);

				foreach (DictionaryEntry dictEnt in allAncestorList)
				{
					hierNal = (NodeAncestorList)dictEnt.Value;

					foreach (NodeAncestorProfile nap in hierNal)
					{
                        if (ni.HomeHierarchyRID != nap.HomeHierarchyRID && !processedNodeList.Contains(nap.Key))
						{
							tempSccl = GetSizeCurveCriteria(nap.Key, tempSccl, forCopy, nap.Key != nodeRID);
                            processedNodeList.Add(nap.Key);
						}
					}
				}

				sequence = sccl.Count + tempSccl.Count;

				foreach (SizeCurveCriteriaProfile sccp in tempSccl)
				{
					sccp.CriteriaSequence = sequence--;
					sccl.Add(sccp);
				}

				return sccl;
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// <summary>
		/// Gets the size curve criteria for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="sccl">An instance of the SizeCurveCriteriaList class containing instances of the SizeCurveCriteriaProfile class</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns></returns>
		static private SizeCurveCriteriaList GetSizeCurveCriteria(int nodeRID, SizeCurveCriteriaList sccl, bool forCopy, bool setInheritance)
		{
			NodeSizeCurveCriteriaInfo nscci = null;
			SizeCurveCriteriaProfile sccp;
			IDictionaryEnumerator iEnum;
			SizeCurveCriteriaInfo scci;

			try
			{
				sizeCurveCriteria_rwl.AcquireReaderLock(ReaderLockTimeOut);

				try
				{
                    if (_sizeCurveCriteriaByRID.ContainsKey(nodeRID))
                    {
                        nscci = (NodeSizeCurveCriteriaInfo)_sizeCurveCriteriaByRID[nodeRID];
                    }
				}
				finally
				{
					sizeCurveCriteria_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetSizeCurveCriteria reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:GetSizeCurveCriteria reader lock has timed out");
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}

			try
			{
				if (nscci == null)
				{
					nscci = new NodeSizeCurveCriteriaInfo();
					LoadSizeCurveCriterias(nodeRID, nscci);
					UpdateSizeCurveCriteriaHash(nodeRID, nscci);
				}

				iEnum = nscci.SizeCurveCriteria.GetEnumerator();

				while (iEnum.MoveNext())
				{
					scci = (SizeCurveCriteriaInfo)iEnum.Value;
					sccp = new SizeCurveCriteriaProfile(scci.Key);

					sccp.CriteriaIsInherited = setInheritance;

					if (setInheritance)
					{
						sccp.CriteriaInheritedFromNodeRID = nodeRID;
					}
					else
					{
						sccp.CriteriaInheritedFromNodeRID = Include.NoRID;
					}

					sccp.CriteriaLevelType = scci.LevelType;
					sccp.CriteriaLevelRID = scci.LevelRID;
					sccp.CriteriaLevelSequence = scci.LevelSequence;
					sccp.CriteriaLevelOffset = scci.LevelOffset;
					sccp.CriteriaDateRID = scci.DateRID;
					sccp.CriteriaApplyLostSalesInd = scci.ApplyLostSalesInd;
					sccp.CriteriaOLLRID = scci.OLLRID;
					sccp.CriteriaCustomOLLRID = scci.CustomOLLRID;
					sccp.CriteriaSizeGroupRID = scci.SizeGroupRID;
					sccp.CriteriaCurveName = scci.CurveName;
					//Begin TT#1076 - JScott - Size Curves by Set
					sccp.CriteriaSgRID = scci.SgRID;
					//End TT#1076 - JScott - Size Curves by Set

					if (forCopy)
					{
						sccp.CriteriaChangeType = eChangeType.update;
					}

					sccl.Add(sccp);
				}

				return sccl;
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// <summary>
		/// Loads the size curve criterias for the node.
		/// </summary>
		///<remarks>
		///Size curve criterias are loaded as needed for performance.
		///</remarks>
		static private void LoadSizeCurveCriterias(int aNodeRID, NodeSizeCurveCriteriaInfo aNodeSizeCurveCriteriaInfo)
		{
			MerchandiseHierarchyData mhd;
			DataTable dt;
			SizeCurveCriteriaInfo scci;

			try
			{
				mhd = new MerchandiseHierarchyData();
				dt = mhd.SizeCurveCriteria_Read(aNodeRID);

				foreach (DataRow dr in dt.Rows)
				{
					scci = new SizeCurveCriteriaInfo(Convert.ToInt32(dr["NSCCD_RID"], CultureInfo.CurrentUICulture));

					scci.LevelType = (eLowLevelsType)Convert.ToInt32(dr["PH_OFFSET_IND"], CultureInfo.CurrentUICulture);
					scci.LevelRID = Convert.ToInt32(dr["PH_RID"], CultureInfo.CurrentUICulture);
					scci.LevelSequence = Convert.ToInt32(dr["PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
					scci.LevelOffset = Convert.ToInt32(dr["PHL_OFFSET"], CultureInfo.CurrentUICulture);
					scci.DateRID = Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture);
					scci.ApplyLostSalesInd = (Convert.ToString(dr["APPLY_LOST_SALES_IND"], CultureInfo.CurrentUICulture) == "1" ? true : false);
					scci.OLLRID = Convert.ToInt32(dr["OLL_RID"], CultureInfo.CurrentUICulture);
					scci.CustomOLLRID = Convert.ToInt32(dr["CUSTOM_OLL_RID"], CultureInfo.CurrentUICulture);
					scci.SizeGroupRID = Convert.ToInt32(dr["SIZE_GROUP_RID"], CultureInfo.CurrentUICulture);
					scci.CurveName = Convert.ToString(dr["CURVE_NAME"], CultureInfo.CurrentUICulture);
					//Begin TT#1076 - JScott - Size Curves by Set
					scci.SgRID = Convert.ToInt32(dr["SG_RID"], CultureInfo.CurrentUICulture);
					//End TT#1076 - JScott - Size Curves by Set

					aNodeSizeCurveCriteriaInfo.SizeCurveCriteria.Add(scci.Key, scci);
				}
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		static private void UpdateSizeCurveCriteriaHash(int aNodeRID, NodeSizeCurveCriteriaInfo aNodeSizeCurveCriteriaInfo)
		{
			try
			{
				sizeCurveCriteria_rwl.AcquireWriterLock(WriterLockTimeOut);

				try
				{
					if (!_sizeCurveCriteriaByRID.ContainsKey(aNodeRID))
					{
						_sizeCurveCriteriaByRID.Add(aNodeRID, aNodeSizeCurveCriteriaInfo);
					}
				}
				finally
				{
					sizeCurveCriteria_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:UpdateSizeCurveCriteriaHash writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:UpdateSizeCurveCriteriaHash writer lock has timed out");
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// <summary>
		/// Adds or updates information about a stores size curve similar store
		/// </summary>
		/// <param name="scdcp">An instance of the SizeCurveDefaultCriteriaProfile class</param>
		/// <param name="nodeRID">The record id of the node</param>
		static public void SizeCurveDefaultCriteriaUpdate(int nodeRID, SizeCurveDefaultCriteriaProfile scdcp)
		{
			NodeSizeCurveDefaultCriteriaInfo nscdci = null;
			SizeCurveDefaultCriteriaInfo scdci = null;

			try
			{
				sizeCurveDefaultCriteria_rwl.AcquireWriterLock(WriterLockTimeOut);

				try
				{
                    if (_sizeCurveDefaultCriteriaByRID.ContainsKey(nodeRID))
                    {
                        nscdci = (NodeSizeCurveDefaultCriteriaInfo)_sizeCurveDefaultCriteriaByRID[nodeRID];
                    }

					if (nscdci == null)
					{
						nscdci = new NodeSizeCurveDefaultCriteriaInfo();
						_sizeCurveDefaultCriteriaByRID[nodeRID] = nscdci;
					}

					if (scdcp.DefaultChangeType != eChangeType.none)
					{
						scdci = nscdci.SizeCurveDefaultCriteriaInfo;

						scdci.DefaultCriteriaRID = scdcp.DefaultRID;
					}
				}
				catch (Exception ex)
				{
					if (Audit != null)
					{
						Audit.Log_Exception(ex);
					}

					EventLog.WriteEntry("MIDHierarchyService", "SizeCurveDefaultCriteriaUpdate error:" + ex.Message, EventLogEntryType.Error);
					throw;
				}
				finally
				{
					sizeCurveDefaultCriteria_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:SizeCurveDefaultCriteriaUpdate writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:SizeCurveDefaultCriteriaUpdate writer lock has timed out");
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		static public SizeCurveDefaultCriteriaProfile GetSizeCurveDefaultCriteriaProfile(int nodeRID, SizeCurveCriteriaList sccl, bool forCopy)
		{
			bool foundDefault;
			NodeInfo ni;
			NodeAncestorList homeNal;
			SortedList allAncestorList;
			NodeAncestorList hierNal;
			SizeCurveDefaultCriteriaProfile scdcp;

			try
			{
				foundDefault = false;
				scdcp = new SizeCurveDefaultCriteriaProfile();

				// Process the Home hierarchy first

				ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
				homeNal = GetNodeAncestorList(nodeRID, ni.HomeHierarchyRID, false);

				foreach (NodeAncestorProfile nap in homeNal)
				{
					GetSizeCurveDefaultCriteria(nap.Key, sccl, scdcp, forCopy, nap.Key != nodeRID, ref foundDefault);

					if (foundDefault)
					{
						break;
					}
				}

				// Process Alternate hierarchies

				if (!foundDefault)
				{
					allAncestorList = GetAllNodeAncestors(nodeRID, false);

					foreach (DictionaryEntry dictEnt in allAncestorList)
					{
						hierNal = (NodeAncestorList)dictEnt.Value;

						foreach (NodeAncestorProfile nap in hierNal)
						{
							if (ni.HomeHierarchyRID != nap.HomeHierarchyRID)
							{
								GetSizeCurveDefaultCriteria(nap.Key, sccl, scdcp, forCopy, nap.Key != nodeRID, ref foundDefault);
							}

							if (foundDefault)
							{
								break;
							}
						}

						if (foundDefault)
						{
							break;
						}
					}
				}

				return scdcp;
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// <summary>
		/// Gets the size curve criteria for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="scdcp">An instance of the SizeCurveDefaultCriteria class</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns></returns>
		static private void GetSizeCurveDefaultCriteria(int nodeRID, SizeCurveCriteriaList sccl, SizeCurveDefaultCriteriaProfile scdcp, bool forCopy, bool setInheritance, ref bool foundDefault)
		{
			NodeSizeCurveDefaultCriteriaInfo nscdci = null;
			SizeCurveDefaultCriteriaInfo scdci = null;

			try
			{
				sizeCurveDefaultCriteria_rwl.AcquireReaderLock(ReaderLockTimeOut);

				try
				{
                    if (_sizeCurveDefaultCriteriaByRID.ContainsKey(nodeRID))
                    {
                        nscdci = (NodeSizeCurveDefaultCriteriaInfo)_sizeCurveDefaultCriteriaByRID[nodeRID];
                    }
				}
				finally
				{
					sizeCurveDefaultCriteria_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetSizeCurveDefaultCriteria reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:GetSizeCurveDefaultCriteria reader lock has timed out");
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}

			try
			{
				if (nscdci == null)
				{
					nscdci = new NodeSizeCurveDefaultCriteriaInfo();
					LoadSizeCurveDefaultCriteria(nodeRID, nscdci);
					UpdateSizeCurveDefaultCriteriaHash(nodeRID, nscdci);
				}

				scdci = nscdci.SizeCurveDefaultCriteriaInfo;

				if (scdci.DefaultCriteriaRID != Include.NoRID && sccl.Contains(scdci.DefaultCriteriaRID))
				{
					scdcp.DefaultRID = scdci.DefaultCriteriaRID;
					foundDefault = true;

					if (setInheritance)
					{
						scdcp.DefaultRIDIsInherited = true;
						scdcp.DefaultRIDIsInheritedFromRID = nodeRID;
					}
				}

				if (forCopy)
				{
					scdcp.DefaultChangeType = eChangeType.update;
				}
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// <summary>
		/// Loads the size curve criterias for the node.
		/// </summary>
		///<remarks>
		///Size curve criterias are loaded as needed for performance.
		///</remarks>
		static private void LoadSizeCurveDefaultCriteria(int aNodeRID, NodeSizeCurveDefaultCriteriaInfo aNodeSizeCurveDefaultCriteriaInfo)
		{
			MerchandiseHierarchyData mhd;
			DataTable dt;
			DataRow dr;
			SizeCurveDefaultCriteriaInfo scdci;

			try
			{
				mhd = new MerchandiseHierarchyData();
				dt = mhd.SizeCurveDefaultCriteria_Read(aNodeRID);

				if (dt.Rows.Count > 0)
				{
					dr = dt.Rows[0];

					scdci = aNodeSizeCurveDefaultCriteriaInfo.SizeCurveDefaultCriteriaInfo;

					scdci.DefaultCriteriaRID = Convert.ToInt32(dr["NSCCD_RID"], CultureInfo.CurrentUICulture);
				}
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		static private void UpdateSizeCurveDefaultCriteriaHash(int aNodeRID, NodeSizeCurveDefaultCriteriaInfo aNodeSizeCurveDefaultCriteriaInfo)
		{
			try
			{
				sizeCurveDefaultCriteria_rwl.AcquireWriterLock(WriterLockTimeOut);

				try
				{
					if (!_sizeCurveDefaultCriteriaByRID.ContainsKey(aNodeRID))
					{
						_sizeCurveDefaultCriteriaByRID.Add(aNodeRID, aNodeSizeCurveDefaultCriteriaInfo);
					}
				}
				finally
				{
					sizeCurveDefaultCriteria_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:UpdateSizeCurveDefaultCriteriaHash writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:UpdateSizeCurveDefaultCriteriaHash writer lock has timed out");
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// <summary>
		/// Adds or updates information about a stores size curve similar store
		/// </summary>
		/// <param name="sccl">An instance of the SizeCurveToleranceProfile class containing instances of the SizeCurveToleranceProfile class</param>
		/// <param name="nodeRID">The record id of the node</param>
		static public void SizeCurveToleranceUpdate(int nodeRID, SizeCurveToleranceProfile sctp)
		{
			NodeSizeCurveToleranceInfo nscti = null;
			SizeCurveToleranceInfo scti = null;

			try
			{
				sizeCurveTolerance_rwl.AcquireWriterLock(WriterLockTimeOut);

				try
				{
                    if (_sizeCurveToleranceByRID.ContainsKey(nodeRID))
                    {
                        nscti = (NodeSizeCurveToleranceInfo)_sizeCurveToleranceByRID[nodeRID];
                    }

					if (nscti == null)
					{
						nscti = new NodeSizeCurveToleranceInfo();
						_sizeCurveToleranceByRID[nodeRID] = nscti;
					}

					if (sctp.ToleranceChangeType != eChangeType.none)
					{
						scti = nscti.SizeCurveToleranceInfo;

						if (sctp.ToleranceChangeType == eChangeType.delete)
						{
							_sizeCurveToleranceByRID[nodeRID] = null;
						}
						else
						{
							scti.MinimumAvg = sctp.ToleranceMinAvg;
							scti.LevelType = sctp.ToleranceLevelType;
							scti.LevelRID = sctp.ToleranceLevelRID;
							scti.LevelSeq = sctp.ToleranceLevelSeq;
							scti.LevelOffset = sctp.ToleranceLevelOffset;
							scti.SalesTolerance = sctp.ToleranceSalesTolerance;
							scti.IndexUnitsInd = sctp.ToleranceIdxUnitsInd;
							scti.MinimumTolerance = sctp.ToleranceMinTolerance;
                            //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
                            scti.ApplyMinToZeroTolerance = sctp.ApplyMinToZeroTolerance;
                            //End TT#2079
							scti.MaximumTolerance = sctp.ToleranceMaxTolerance;
						}
					}
				}
				catch (Exception ex)
				{
					if (Audit != null)
					{
						Audit.Log_Exception(ex);
					}

					EventLog.WriteEntry("MIDHierarchyService", "SizeCurveToleranceUpdate error:" + ex.Message, EventLogEntryType.Error);
					throw;
				}
				finally
				{
					sizeCurveTolerance_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:SizeCurveToleranceUpdate writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:SizeCurveToleranceUpdate writer lock has timed out");
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// <summary>
		/// Gets the size curve tolerance for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns></returns>
		static public SizeCurveToleranceProfile GetSizeCurveToleranceProfile(int nodeRID, bool forCopy)
		{
			bool foundMinAvg;
			bool foundLevel;
			bool foundSalesToler;
			bool foundIndexUnits;
			bool foundMinToler;
			bool foundMaxToler;
			bool setInheritance;
			NodeInfo ni;
			NodeAncestorList nal;
			SizeCurveToleranceProfile sctp;

			try
			{
				foundMinAvg = false;
				foundLevel = false;
				foundSalesToler = false;
				foundIndexUnits = false;
				foundMinToler = false;
				foundMaxToler = false;
				setInheritance = false;

				ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
				nal = GetNodeAncestorList(nodeRID, ni.HomeHierarchyRID, false);
				sctp = new SizeCurveToleranceProfile();

				foreach (NodeAncestorProfile nap in nal)
				{
					GetSizeCurveTolerance(nap.Key, sctp, forCopy, setInheritance,
						ref foundMinAvg, ref foundLevel, ref foundSalesToler, ref foundIndexUnits, ref foundMinToler, ref foundMaxToler);

					if (foundMinAvg && foundLevel && foundSalesToler && foundIndexUnits && foundMinToler && foundMaxToler)
					{
						break;
					}

					setInheritance = true;
				}

				return sctp;
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// <summary>
		/// Gets the size curve tolerance for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="sctl">An instance of the SizeCurveToleranceProfile class containing instances of the SizeCurveToleranceProfile class</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns></returns>
		static private void GetSizeCurveTolerance(int nodeRID, SizeCurveToleranceProfile sctp, bool forCopy, bool setInheritance,
			ref bool foundMinAvg, ref bool foundLevel, ref bool foundSalesToler, ref bool foundIndexUnits, ref bool foundMinToler, ref bool foundMaxToler)
		{
			NodeSizeCurveToleranceInfo nscti = null;
			SizeCurveToleranceInfo scti = null;

			try
			{
				sizeCurveTolerance_rwl.AcquireReaderLock(ReaderLockTimeOut);

				try
				{
                    if (_sizeCurveToleranceByRID.ContainsKey(nodeRID))
                    {
                        nscti = (NodeSizeCurveToleranceInfo)_sizeCurveToleranceByRID[nodeRID];
                    }
				}
				finally
				{
					sizeCurveTolerance_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetSizeCurveTolerance reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:GetSizeCurveTolerance reader lock has timed out");
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}

			try
			{
				if (nscti == null)
				{
					nscti = new NodeSizeCurveToleranceInfo();
					LoadSizeCurveTolerances(nodeRID, nscti);
					UpdateSizeCurveToleranceHash(nodeRID, nscti);
				}

				scti = nscti.SizeCurveToleranceInfo;

				if (scti.MinimumAvg != Include.Undefined && !foundMinAvg)
				{
					foundMinAvg = true;
					sctp.ToleranceMinAvg = scti.MinimumAvg;

					if (forCopy)
					{
						sctp.ToleranceChangeType = eChangeType.update;
					}
					if (setInheritance)
					{
						sctp.ToleranceMinAvgIsInherited = true;
						sctp.ToleranceMinAvgInheritedFromNodeRID = nodeRID;
					}
				}

				if (scti.LevelType != eLowLevelsType.None && !foundLevel)
				{
					foundLevel = true;
					sctp.ToleranceLevelType = scti.LevelType;
					sctp.ToleranceLevelRID = scti.LevelRID;
					sctp.ToleranceLevelSeq = scti.LevelSeq;
					sctp.ToleranceLevelOffset = scti.LevelOffset;

					if (forCopy)
					{
						sctp.ToleranceChangeType = eChangeType.update;
					}
					if (setInheritance)
					{
						sctp.ToleranceLevelIsInherited = true;
						sctp.ToleranceLevelInheritedFromNodeRID = nodeRID;
					}
				}

				if (scti.SalesTolerance != Include.Undefined && !foundSalesToler)
				{
					foundSalesToler = true;
					sctp.ToleranceSalesTolerance = scti.SalesTolerance;

					if (forCopy)
					{
						sctp.ToleranceChangeType = eChangeType.update;
					}
					if (setInheritance)
					{
						sctp.ToleranceSalesToleranceIsInherited = true;
						sctp.ToleranceSalesToleranceInheritedFromNodeRID = nodeRID;
					}
				}

				if (scti.IndexUnitsInd != eNodeChainSalesType.None && !foundIndexUnits)
				{
					foundIndexUnits = true;
					sctp.ToleranceIdxUnitsInd = scti.IndexUnitsInd;

					if (forCopy)
					{
						sctp.ToleranceChangeType = eChangeType.update;
					}
					if (setInheritance)
					{
						sctp.ToleranceIdxUnitsIndIsInherited = true;
						sctp.ToleranceIdxUnitsIndInheritedFromNodeRID = nodeRID;
					}
				}

				if (scti.MinimumTolerance != Include.Undefined && !foundMinToler)
				{
					foundMinToler = true;
					sctp.ToleranceMinTolerance = scti.MinimumTolerance;
                    //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
                    sctp.ApplyMinToZeroTolerance = scti.ApplyMinToZeroTolerance;
                    //End TT#2079

					if (forCopy)
					{
						sctp.ToleranceChangeType = eChangeType.update;
					}
					if (setInheritance)
					{
						sctp.ToleranceMinToleranceIsInherited = true;
						sctp.ToleranceMinToleranceInheritedFromNodeRID = nodeRID;
					}
				}

				if (scti.MaximumTolerance != Include.Undefined && !foundMaxToler)
				{
					foundMaxToler = true;
					sctp.ToleranceMaxTolerance = scti.MaximumTolerance;

					if (forCopy)
					{
						sctp.ToleranceChangeType = eChangeType.update;
					}
					if (setInheritance)
					{
						sctp.ToleranceMaxToleranceIsInherited = true;
						sctp.ToleranceMaxToleranceInheritedFromNodeRID = nodeRID;
					}
				}
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// <summary>
		/// Loads the size curve tolerances for the node.
		/// </summary>
		///<remarks>
		///Size curve tolerances are loaded as needed for performance.
		///</remarks>
		static private void LoadSizeCurveTolerances(int aNodeRID, NodeSizeCurveToleranceInfo aNodeSizeCurveToleranceInfo)
		{
			MerchandiseHierarchyData mhd;
			DataTable dt;
			DataRow dr;
			SizeCurveToleranceInfo scti;

			try
			{
				mhd = new MerchandiseHierarchyData();
				dt = mhd.SizeCurveTolerance_Read(aNodeRID);

				if (dt.Rows.Count > 0)
				{
					dr = dt.Rows[0];

					scti = aNodeSizeCurveToleranceInfo.SizeCurveToleranceInfo;

					scti.MinimumAvg = Convert.ToDouble(dr["MINIMUM_AVERAGE"], CultureInfo.CurrentUICulture);
					scti.LevelType = (eLowLevelsType)Convert.ToInt32(dr["PH_OFFSET_IND"], CultureInfo.CurrentUICulture);
					scti.LevelRID = Convert.ToInt32(dr["PH_RID"], CultureInfo.CurrentUICulture);
					scti.LevelSeq = Convert.ToInt32(dr["PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
					scti.LevelOffset = Convert.ToInt32(dr["PHL_OFFSET"], CultureInfo.CurrentUICulture);
					scti.SalesTolerance = Convert.ToDouble(dr["SALES_TOLERANCE"], CultureInfo.CurrentUICulture);
					scti.IndexUnitsInd = (eNodeChainSalesType)Convert.ToInt32(dr["INDEX_UNITS_TYPE"], CultureInfo.CurrentUICulture);
                    scti.MinimumTolerance = Convert.ToDouble(dr["MIN_TOLERANCE"], CultureInfo.CurrentUICulture);
                    scti.MaximumTolerance = Convert.ToDouble(dr["MAX_TOLERANCE"], CultureInfo.CurrentUICulture);
                    //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
                    scti.ApplyMinToZeroTolerance = Include.ConvertCharToBool(Convert.ToChar(dr["APPLY_MIN_TO_ZERO_TOLERANCE_IND"], CultureInfo.CurrentUICulture)); ;
                    //End TT#2079
				}
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}
				throw;
			}
		}

		static private void UpdateSizeCurveToleranceHash(int aNodeRID, NodeSizeCurveToleranceInfo aNodeSizeCurveToleranceInfo)
		{
			try
			{
				sizeCurveTolerance_rwl.AcquireWriterLock(WriterLockTimeOut);

				try
				{
					if (!_sizeCurveToleranceByRID.ContainsKey(aNodeRID))
					{
						_sizeCurveToleranceByRID.Add(aNodeRID, aNodeSizeCurveToleranceInfo);
					}
				}
				finally
				{
					sizeCurveTolerance_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:UpdateSizeCurveToleranceHash writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:UpdateSizeCurveToleranceHash writer lock has timed out");
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// <summary>
		/// Adds or updates information about a nodes size curve similar store
		/// </summary>
		/// <param name="scssl">An instance of the SizeCurveSimilarStoreList class containing instances of the SizeCurveSimilarStoreProfile class</param>
		/// <param name="nodeRID">The record id of the node</param>
		static public void SizeCurveSimilarStoreUpdate(int nodeRID, SizeCurveSimilarStoreList scssl)
		{
			NodeSizeCurveSimilarStoreInfo nscssi = null;
			SizeCurveSimilarStoreInfo scssi = null;
			bool found;

			try
			{
				sizeCurveSimilarStore_rwl.AcquireWriterLock(WriterLockTimeOut);

				try
				{
                    if (_sizeCurveSimilarStoreByRID.ContainsKey(nodeRID))
                    {
                        nscssi = (NodeSizeCurveSimilarStoreInfo)_sizeCurveSimilarStoreByRID[nodeRID];
                    }
		
					if (nscssi == null)
					{
						nscssi = new NodeSizeCurveSimilarStoreInfo();
					}

					found = false;

					foreach (SizeCurveSimilarStoreProfile scssp in scssl)
					{
						if (scssp.SimilarStoreChangeType != eChangeType.none)
						{
							if (nscssi.StoreSizeCurveSimilarStore.Contains(scssp.Key))
							{
								scssi = (SizeCurveSimilarStoreInfo)nscssi.StoreSizeCurveSimilarStore[scssp.Key];
								found = true;
							}
							else
							{
								scssi = new SizeCurveSimilarStoreInfo();
								found = false;
							}

							if (scssp.SimilarStoreChangeType == eChangeType.delete)
							{
								if (found)
								{
									nscssi.StoreSizeCurveSimilarStore.Remove(scssi.StoreRID);
								}
							}
							else
							{
								scssi.StoreRID = scssp.Key;
								scssi.SimilarStoreRID = scssp.SimStoreRID;
								scssi.UntilDateRID = scssp.SimStoreUntilDateRangeRID;

								if (scssp.SimStoreUntilDateRangeRID != Include.NoRID)
								{
									scssi.DateRangeProfile = Calendar.GetDateRange(scssp.SimStoreUntilDateRangeRID);
								}

								if (!found)
								{
									nscssi.StoreSizeCurveSimilarStore.Add(scssi.StoreRID, scssi);
								}
								else
								{
									nscssi.StoreSizeCurveSimilarStore[scssi.StoreRID] = scssi;
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					if (Audit != null)
					{
						Audit.Log_Exception(ex);
					}

					EventLog.WriteEntry("MIDHierarchyService", "SizeCurveSimilarStoreUpdate error:" + ex.Message, EventLogEntryType.Error);
					throw;
				}
				finally
				{
					sizeCurveSimilarStore_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:SizeCurveSimilarStoreUpdate writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:SizeCurveSimilarStoreUpdate writer lock has timed out");
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// <summary>
		/// Gets the size curve similar store for the node.
		/// </summary>
		/// <param name="storeList">The list of stores requiring size curve similar store</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="chaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns>An instance of the SizeCurveSimilarStoreList class containing instances of the SizeCurveSimilarStoreProfile class</returns>
		static public SizeCurveSimilarStoreList GetSizeCurveSimilarStoreList(ProfileList storeList, int nodeRID, bool chaseHierarchy, bool forCopy)
		{
			bool setInheritance;
			int storeCount;
			NodeInfo ni;
			NodeAncestorList nal;
			SizeCurveSimilarStoreList scssl;

			try
			{
				setInheritance = false;
				storeCount = 0;
				ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
				nal = GetNodeAncestorList(nodeRID, ni.HomeHierarchyRID, false);
				scssl = new SizeCurveSimilarStoreList();

				foreach (NodeAncestorProfile nap in nal)
				{
					scssl = GetSizeCurveSimilarStore(storeList, nap.Key, scssl, forCopy, setInheritance, ref storeCount);

					if (storeCount == storeList.Count ||	//  stop lookup if have all settings for each store
						!chaseHierarchy)					//  or stop on first node if copy
					{
						break;
					}

					setInheritance = true;
				}

				return scssl;
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// <summary>
		/// Gets the size curve similar store for the node.
		/// </summary>
		/// <param name="storeList">The list of stores requiring size curve similar store</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <param name="sel">The list containing the eligibilities for the stores</param>
		/// <param name="setInheritance">Identifies if inheritance information should be set</param>
		/// <param name="storeCount">The number of stores that have all fields set</param>
		/// <returns></returns>
		static private SizeCurveSimilarStoreList GetSizeCurveSimilarStore(ProfileList storeList, int nodeRID,
			SizeCurveSimilarStoreList sel, bool forCopy, bool setInheritance, ref int storeCount)
		{
			NodeSizeCurveSimilarStoreInfo nscssi = null;
			SizeCurveSimilarStoreInfo scssi;
			StoreProfile storeProfile;
			SizeCurveSimilarStoreProfile scssp;
			bool newRecord;
			bool setSomething;
			DateRangeProfile drp;

			try
			{
				sizeCurveSimilarStore_rwl.AcquireReaderLock(ReaderLockTimeOut);

				try
				{
                    if (_sizeCurveSimilarStoreByRID.ContainsKey(nodeRID))
                    {
                        nscssi = (NodeSizeCurveSimilarStoreInfo)_sizeCurveSimilarStoreByRID[nodeRID];
                    }
				}
				finally
				{
					sizeCurveSimilarStore_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetSizeCurveSimilarStore reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:GetSizeCurveSimilarStore reader lock has timed out");
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}

			try
			{
				if (nscssi == null)
				{
					nscssi = new NodeSizeCurveSimilarStoreInfo();
					LoadSizeCurveSimilarStore(nodeRID, nscssi, storeList);
					UpdateSizeCurveSimilarStoreHash(nodeRID, nscssi);
				}

				foreach (DictionaryEntry val in nscssi.StoreSizeCurveSimilarStore)
				{
					scssi = (SizeCurveSimilarStoreInfo)val.Value;

					if (storeList.Contains(scssi.StoreRID))  // Do you need this store
					{
						if (!sel.Contains(scssi.StoreRID))  // Do you already have this store
						{
							scssp = new SizeCurveSimilarStoreProfile(scssi.StoreRID);
							newRecord = true;
						}
						else
						{
							scssp = (SizeCurveSimilarStoreProfile)sel.FindKey(scssi.StoreRID);
							newRecord = false;
						}

						setSomething = false;
						storeProfile = (StoreProfile)storeList.FindKey(scssi.StoreRID);

						if (scssp.SimStoreRID == Include.NoRID &&
							scssi.SimilarStoreRID != Include.NoRID)
						{
							setSomething = true;

							scssp.SimStoreRID = scssi.SimilarStoreRID;
							scssp.SimStoreUntilDateRangeRID = scssi.UntilDateRID;

							if (scssp.SimStoreUntilDateRangeRID != Include.NoRID)
							{
								if (scssi.UntilDateRID != scssi.OrigUntilDateRID)
								{
									drp = Calendar.GetDateRange(scssp.SimStoreUntilDateRangeRID);
									scssi.DateRangeProfile = drp;
								}
								else
								{
									drp = scssi.DateRangeProfile;
								}

								if (storeProfile.SellingOpenDt != Include.UndefinedDate)
								{
									drp.InternalAnchorDate = Calendar.GetWeek(storeProfile.SellingOpenDt);
								}

								scssp.SimStoreWeekList = Calendar.GetDateRangeWeeks(drp, drp.InternalAnchorDate);
								scssp.SimStoreDisplayDate = Calendar.GetDisplayDate(drp);
							}

							if (!forCopy)
							{
								if (setInheritance)
								{
									scssp.SimStoreIsInherited = true;
									scssp.SimStoreInheritedFromNodeRID = nodeRID;
								}
								else
								{
									scssp.RecordExists = true;
								}
							}
						}

						if (forCopy)
						{
							scssp.SimilarStoreChangeType = eChangeType.add;
						}

						if (setSomething && scssp.SimStoreRID != Include.NoRID)
						{
							++storeCount;
						}

						if (!forCopy)
						{
							if (!setInheritance)
							{
								scssp.RecordExists = true;
							}
						}

						if (newRecord)
						{
							sel.Add(scssp);
						}
						else
						{
							sel.Update(scssp);
						}
					}
				}

				return sel;
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// <summary>
		/// Loads the size curve similar store for the node.
		/// </summary>
		///<remarks>
		///Size curve similar store is loaded as needed for performance.
		///</remarks>
		static private void LoadSizeCurveSimilarStore(int aNodeRID, NodeSizeCurveSimilarStoreInfo nei, ProfileList aStoreList)
		{
			MerchandiseHierarchyData mhd;
			DataTable dt;
			SizeCurveSimilarStoreInfo scssi;

			try
			{
				mhd = new MerchandiseHierarchyData();
				dt = mhd.SizeCurveSimilarStore_Read(aNodeRID);

				foreach (DataRow dr in dt.Rows)
				{
					scssi = new SizeCurveSimilarStoreInfo();

					scssi.StoreRID = Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture);
					scssi.SimilarStoreRID = Convert.ToInt32(dr["SS_RID"], CultureInfo.CurrentUICulture);
					scssi.UntilDateRID = Convert.ToInt32(dr["UNTIL_DATE"], CultureInfo.CurrentUICulture);
					scssi.OrigUntilDateRID = scssi.UntilDateRID;

					if (scssi.UntilDateRID != Include.NoRID)
					{
						scssi.DateRangeProfile.Key = Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture);
						scssi.DateRangeProfile.StartDateKey = Convert.ToInt32(dr["CDR_START"], CultureInfo.CurrentUICulture);
						scssi.DateRangeProfile.EndDateKey = Convert.ToInt32(dr["CDR_END"], CultureInfo.CurrentUICulture);
						scssi.DateRangeProfile.DateRangeType = (eCalendarRangeType)Convert.ToInt32(dr["CDR_RANGE_TYPE_ID"], CultureInfo.CurrentUICulture);
						scssi.DateRangeProfile.SelectedDateType = (eCalendarDateType)Convert.ToInt32(dr["CDR_DATE_TYPE_ID"], CultureInfo.CurrentUICulture);
						scssi.DateRangeProfile.RelativeTo = (eDateRangeRelativeTo)Convert.ToInt32(dr["CDR_RELATIVE_TO"], CultureInfo.CurrentUICulture);

						if (dr["CDR_NAME"] != DBNull.Value)
						{
							scssi.DateRangeProfile.Name = (string)dr["CDR_NAME"];
						}
					}

					nei.StoreSizeCurveSimilarStore.Add(scssi.StoreRID, scssi);
				}
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		static private void UpdateSizeCurveSimilarStoreHash(int aNodeRID, NodeSizeCurveSimilarStoreInfo aNodeSizeCurveSimilarStoreInfo)
		{
			try
			{
				sizeCurveSimilarStore_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					if (!_sizeCurveSimilarStoreByRID.ContainsKey(aNodeRID))
					{
						_sizeCurveSimilarStoreByRID.Add(aNodeRID, aNodeSizeCurveSimilarStoreInfo);
					}
				}
				finally
				{
					sizeCurveSimilarStore_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:UpdateSizeCurveSimilarStoreHash writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:UpdateSizeCurveSimilarStoreHash writer lock has timed out");
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		//End TT#155 - JScott - Add Size Curve info to Node Properties
		//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
		/// <summary>
		/// Gets the size out of stock for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns></returns>
		static public SizeOutOfStockHeaderProfile GetSizeOutOfStockHeaderProfile(int nodeRID, bool forCopy)
		{
			try
			{
				return GetSizeOutOfStockHeaderProfile(nodeRID, Include.NoRID, Include.NoRID, forCopy);
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// Gets the size out of stock for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns></returns>
		static public SizeOutOfStockHeaderProfile GetSizeOutOfStockHeaderProfile(int nodeRID, int storeGrpRID, int sizeGrpRID, bool forCopy)
		{
			SizeOutOfStockHeaderProfile sooshp;

			try
			{
				sooshp = new SizeOutOfStockHeaderProfile();

				if (storeGrpRID != Include.NoRID)
				{
					sooshp.StoreGrpRID = storeGrpRID;
				}

				if (sizeGrpRID != Include.NoRID)
				{
					sooshp.SizeGrpRID = sizeGrpRID;
				}

				if (sooshp.StoreGrpRID == Include.NoRID || sooshp.SizeGrpRID == Include.NoRID)
				{
					GetSizeOutOfStockHeader(nodeRID, sooshp, forCopy);
				}

				if (sooshp.StoreGrpRID == Include.NoRID)
				{
					sooshp.StoreGrpRID = _globalOptions.AllocationStoreGroupRID;
				}

				return sooshp;
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// <summary>
		/// Gets the size out of stock for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns></returns>
        // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
        //static public SizeOutOfStockProfile GetSizeOutOfStockProfile(int nodeRID, bool forCopy)
        static public SizeOutOfStockProfile GetSizeOutOfStockProfile(int nodeRID, bool forCopy, int sg_Version)
        // End TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
		{
			try
			{
                // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                //return GetSizeOutOfStockProfile(nodeRID, Include.NoRID, Include.NoRID, forCopy);
                return GetSizeOutOfStockProfile(nodeRID, Include.NoRID, Include.NoRID, forCopy, sg_Version);
                // End TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// Gets the size out of stock for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns></returns>
        // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
        //static public SizeOutOfStockProfile GetSizeOutOfStockProfile(int nodeRID, int storeGrpRID, int sizeGrpRID, bool forCopy)
        static public SizeOutOfStockProfile GetSizeOutOfStockProfile(int nodeRID, int storeGrpRID, int sizeGrpRID, bool forCopy, int sg_Version)
        // End TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
		{
			SizeOutOfStockProfile soosp;

			try
			{
				soosp = new SizeOutOfStockProfile();

				if (storeGrpRID != Include.NoRID)
				{
					soosp.StoreGrpRID = storeGrpRID;
				}

				if (sizeGrpRID != Include.NoRID)
				{
					soosp.SizeGrpRID = sizeGrpRID;
				}

				if (soosp.StoreGrpRID == Include.NoRID || soosp.SizeGrpRID == Include.NoRID)
				{
					GetSizeOutOfStockHeader(nodeRID, soosp, forCopy);
				}

				if (soosp.StoreGrpRID == Include.NoRID)
				{
					soosp.StoreGrpRID = _globalOptions.AllocationStoreGroupRID;
				}

                // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                //GetSizeOutOfStockValues(nodeRID, soosp.StoreGrpRID, soosp.SizeGrpRID, soosp);
                GetSizeOutOfStockValues(nodeRID, soosp.StoreGrpRID, soosp.SizeGrpRID, soosp, sg_Version);
                // End TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.

				return soosp;
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// <summary>
		/// Gets the size curve tolerance for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="sooshp">An instance of the SizeOutOfStockHeaderProfile class containing instances of the SizeOutOfStockProfile class</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns></returns>
		static private void GetSizeOutOfStockHeader(int nodeRID, SizeOutOfStockHeaderProfile sooshp, bool forCopy)
		{
			MerchandiseHierarchyData mhd;
			DataTable dt;
			DataRow dr;
			int strGrpRID;
			int szGrpRID;

			try
			{
				mhd = new MerchandiseHierarchyData();
				dt = mhd.SizeOutOfStockHeader_Get(nodeRID);

				if (dt.Rows.Count > 0)
				{
					dr = dt.Rows[0];

					strGrpRID = Convert.ToInt32(dr["SG_RID"], CultureInfo.CurrentUICulture);

					if (strGrpRID != Include.Undefined && sooshp.StoreGrpRID == Include.NoRID)
					{
						sooshp.StoreGrpRID = strGrpRID;
						sooshp.StrGrpIsInherited = Convert.ToBoolean(dr["SG_RID_IS_INHERITED"], CultureInfo.CurrentUICulture);
						sooshp.StrGrpIsInheritedFromNodeRID = Convert.ToInt32(dr["SG_RID_IS_INHERITED_FROM"], CultureInfo.CurrentUICulture);

						if (forCopy)
						{
							sooshp.ChangeType = eChangeType.update;
						}
					}

					szGrpRID = Convert.ToInt32(dr["SIZE_GROUP_RID"], CultureInfo.CurrentUICulture);

					if (szGrpRID != Include.Undefined && sooshp.SizeGrpRID == Include.NoRID)
					{
						sooshp.SizeGrpRID = szGrpRID;
						sooshp.SizeGrpIsInherited = Convert.ToBoolean(dr["SIZE_GROUP_RID_IS_INHERITED"], CultureInfo.CurrentUICulture);
						sooshp.SizeGrpIsInheritedFromNodeRID = Convert.ToInt32(dr["SIZE_GROUP_RID_IS_INHERITED_FROM"], CultureInfo.CurrentUICulture);

						if (forCopy)
						{
							sooshp.ChangeType = eChangeType.update;
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}
				throw;
			}
		}

		/// <summary>
		/// Gets the size curve tolerance for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="sctl">An instance of the SizeOutOfStockProfile class containing instances of the SizeOutOfStockProfile class</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns></returns>
        // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
        //static private void GetSizeOutOfStockValues(int nodeRID, int strGrpRID, int sizeGrpRID, SizeOutOfStockProfile soosp)
        static private void GetSizeOutOfStockValues(int nodeRID, int strGrpRID, int sizeGrpRID, SizeOutOfStockProfile soosp, int sg_Version)
        // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
		{
			MerchandiseHierarchyData mhd;
			DataSet dsValues;

			try
			{
				mhd = new MerchandiseHierarchyData();
                // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                //dsValues = mhd.SizeOutOfStockValuesForNodeProperties_Get(nodeRID, strGrpRID, sizeGrpRID);
                dsValues = mhd.SizeOutOfStockValuesForNodeProperties_Get(nodeRID, strGrpRID, sizeGrpRID, sg_Version);
                // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.

				soosp.dsValues = dsValues;
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}
				throw;
			}
		}

        //Begin TT#739-MD -jsobek -Delete Stores
		// Begin TT#483 - STODD - add out of stock processing
		/// <summary>
		/// Returns a DataTable of Color Rids and Color Code Rids that
		/// are affected by Out of Stock values.
		/// </summary>
		/// <returns></returns>
        //static public DataTable GetSizeOutOfStockColorNodes()
        //{
        //    MerchandiseHierarchyData mhd;
        //    DataSet dsValues;

        //    try
        //    {
        //        mhd = new MerchandiseHierarchyData();

        //        sizeOutOfStock_rwl.AcquireReaderLock(ReaderLockTimeOut);

        //        dsValues = mhd.SizeOutOfStockValues_GetColorNodes();

        //        sizeOutOfStock_rwl.ReleaseReaderLock();

        //        return dsValues.Tables[0];
        //    }
        //    catch (Exception ex)
        //    {
        //        if (Audit != null)
        //        {
        //            Audit.Log_Exception(ex);
        //        }
        //        throw;
        //    }
        //}

        //static public DataTable GetSizeSellThruLimitColorNodes()
        //{
        //    MerchandiseHierarchyData mhd;
        //    DataSet dsValues;

        //    try
        //    {
        //        mhd = new MerchandiseHierarchyData();

        //        sizeSellThru_rwl.AcquireReaderLock(ReaderLockTimeOut);

        //        dsValues = mhd.SizeSellThru_GetColorNodes();

        //        sizeSellThru_rwl.ReleaseReaderLock();

        //        return dsValues.Tables[0];
        //    }
        //    catch (Exception ex)
        //    {
        //        if (Audit != null)
        //        {
        //            Audit.Log_Exception(ex);
        //        }
        //        throw;
        //    }
        //}
		// End TT#483 - STODD - add out of stock processing

        
        static public DataTable ExecuteSizeDayToWeekSummary(int aNodeRID, int startSQLTimeID, int endSQLTimeID, int storeRID = -1)
        {
            MerchandiseHierarchyData mhd;
            DataSet dsValues;

            try
            {
                mhd = new MerchandiseHierarchyData();

                //sizeSellThru_rwl.AcquireReaderLock(ReaderLockTimeOut);

                dsValues = mhd.SizeDayToWeekSummary(aNodeRID, startSQLTimeID, endSQLTimeID, storeRID);

                //sizeSellThru_rwl.ReleaseReaderLock();

                return dsValues.Tables[0];
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }
        //End TT#739-MD -jsobek -Delete Stores





		/// <summary>
		/// Adds or updates information about a nodes size curve similar store
		/// </summary>
		/// <param name="sstp">An instance of the SizeSellThruProfile class</param>
		/// <param name="nodeRID">The record id of the node</param>
		static public void SizeSellThruUpdate(int nodeRID, SizeSellThruProfile sstp)
		{
			NodeSizeSellThruInfo nssti = null;
			SizeSellThruInfo ssti = null;

			try
			{
				sizeSellThru_rwl.AcquireWriterLock(WriterLockTimeOut);

				try
				{
                    if (_sizeSellThruByRID.ContainsKey(nodeRID))
                    {
                        nssti = (NodeSizeSellThruInfo)_sizeSellThruByRID[nodeRID];
                    }

					if (nssti == null)
					{
						nssti = new NodeSizeSellThruInfo();
						_sizeSellThruByRID[nodeRID] = nssti;
					}

					if (sstp.ChangeType != eChangeType.none)
					{
						ssti = nssti.SizeSellThruInfo;

						if (sstp.ChangeType == eChangeType.delete)
						{
							_sizeSellThruByRID[nodeRID] = null;
						}
						else
						{
							ssti.SellThruLimit = sstp.SellThruLimit;
						}
					}
				}
				catch (Exception ex)
				{
					if (Audit != null)
					{
						Audit.Log_Exception(ex);
					}

					EventLog.WriteEntry("MIDHierarchyService", "SizeSellThruUpdate error:" + ex.Message, EventLogEntryType.Error);
					throw;
				}
				finally
				{
					sizeSellThru_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:SizeSellThruUpdate writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:SizeSellThruUpdate writer lock has timed out");
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// Gets the size sell thru for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns></returns>
		static public SizeSellThruProfile GetSizeSellThruProfile(int nodeRID, bool forCopy)
		{
			bool foundSellThru;
			bool setInheritance;
			NodeInfo ni;
			NodeAncestorList nal;
			SizeSellThruProfile sstp;

			try
			{
				foundSellThru = false;
				setInheritance = false;

				ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
				nal = GetNodeAncestorList(nodeRID, ni.HomeHierarchyRID);
				sstp = new SizeSellThruProfile();

				if (!foundSellThru)
				{
					foreach (NodeAncestorProfile nap in nal)
					{
						GetSizeSellThru(nap.Key, sstp, forCopy, setInheritance, ref foundSellThru);

						if (foundSellThru)
						{
							break;
						}

						setInheritance = true;
					}
				}

				return sstp;
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// <summary>
		/// Gets the size sell thru for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="sstp">An instance of the SizeSellThruProfile class containing instances of the SizeSellThruProfile class</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns></returns>
		static private void GetSizeSellThru(int nodeRID, SizeSellThruProfile sstp, bool forCopy, bool setInheritance,
			ref bool foundSellThru)
		{
			NodeSizeSellThruInfo nssti = null;
			SizeSellThruInfo ssti = null;

			try
			{
				sizeSellThru_rwl.AcquireReaderLock(ReaderLockTimeOut);

				try
				{
                    if (_sizeSellThruByRID.ContainsKey(nodeRID))
                    {
                        nssti = (NodeSizeSellThruInfo)_sizeSellThruByRID[nodeRID];
                    }
				}
				finally
				{
					sizeSellThru_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetSizeSellThru reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:GetSizeSellThru reader lock has timed out");
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}

			try
			{
				if (nssti == null)
				{
					nssti = new NodeSizeSellThruInfo();
					LoadSizeSellThru(nodeRID, nssti);
					UpdateSizeSellThruHash(nodeRID, nssti);
				}

				ssti = nssti.SizeSellThruInfo;

				if (ssti.SellThruLimit != Include.Undefined && !foundSellThru)
				{
					foundSellThru = true;
					sstp.SellThruLimit = ssti.SellThruLimit;

					if (forCopy)
					{
						sstp.ChangeType = eChangeType.update;
					}
					if (setInheritance)
					{
						sstp.SellThruLimitIsInherited = true;
						sstp.SellThruLimitInheritedFromNodeRID = nodeRID;
					}
				}
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		/// <summary>
		/// Loads the size sell thru for the node.
		/// </summary>
		///<remarks>
		///Size sell thru is loaded as needed for performance.
		///</remarks>
		static private void LoadSizeSellThru(int aNodeRID, NodeSizeSellThruInfo aNodeSizeSellThruInfo)
		{
			MerchandiseHierarchyData mhd;
			DataTable dt;
			DataRow dr;
			SizeSellThruInfo ssti;

			try
			{
				mhd = new MerchandiseHierarchyData();
				dt = mhd.SizeSellThru_Get(aNodeRID);

				if (dt.Rows.Count > 0)
				{
					dr = dt.Rows[0];

					ssti = aNodeSizeSellThruInfo.SizeSellThruInfo;

					ssti.SellThruLimit = Convert.ToSingle(dr["SELLTHRU_LIMIT"], CultureInfo.CurrentUICulture);
				}
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}
				throw;
			}
		}

		static private void UpdateSizeSellThruHash(int aNodeRID, NodeSizeSellThruInfo aNodeSizeSellThruInfo)
		{
			try
			{
				sizeSellThru_rwl.AcquireWriterLock(WriterLockTimeOut);

				try
				{
					if (!_sizeSellThruByRID.ContainsKey(aNodeRID))
					{
						_sizeSellThruByRID.Add(aNodeRID, aNodeSizeSellThruInfo);
					}
				}
				finally
				{
					sizeSellThru_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:UpdateSizeSellThruHash writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:UpdateSizeSellThruHash writer lock has timed out");
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}

				throw;
			}
		}

		//End TT#483 - JScott - Add Size Lost Sales criteria and processing
		/// <summary>
		/// Adds or updates information about a stores grades
		/// </summary>
		/// <param name="sgl">An instance of the StoreGradeList class containing instances of the StoreGradeProfile class</param>
		/// <param name="nodeRID">The record id of the node</param>
        static public void StoreGradesUpdate(int nodeRID, StoreGradeList sgl, bool aBoundaryChanged)  // TT#2404 - JSmith - Questions on Store Grades in OTS Forecast Review
		{
            NodeStoreGradesInfo nsgi = null;
			try
			{
				storeGrades_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
                    // Begin TT#2404 - JSmith - Questions on Store Grades in OTS Forecast Review
                    // clear all cached grades if boundary has changed so all will be reloaded
                    if (aBoundaryChanged)
                    {
                        _storeGradesByRID.Clear();
                        return;
                    }
                    // End TT#2404 - JSmith - Questions on Store Grades in OTS Forecast Review
					bool newEntry = false;
                    if (_storeGradesByRID.ContainsKey(nodeRID))
                    {
                        nsgi = (NodeStoreGradesInfo)_storeGradesByRID[nodeRID];
                    }
					if (nsgi == null)
					{
						nsgi = new NodeStoreGradesInfo();
						newEntry = true;
					}
 
					if (!newEntry && nsgi.StoreGradesLoaded)
					{
						nsgi.StoreGrades.Clear();
					}
					else
					{
						nsgi.StoreGrades = new ArrayList();
					}
					foreach (StoreGradeProfile sgp in sgl)
					{
						if (sgp.StoreGradeChangeType != eChangeType.delete ||
							sgp.MinMaxChangeType != eChangeType.delete)
						{
							StoreGradeInfo sgi = new StoreGradeInfo();
							sgi.Boundary = sgp.Boundary;
							if (sgp.StoreGradeChangeType == eChangeType.add ||
								sgp.StoreGradeChangeType == eChangeType.update ||
								(sgp.StoreGradeChangeType == eChangeType.none &&	// handle a change to min/maxes only
								sgp.StoreGradesIsInherited == false))				// to put code back in StoreGradeInfo
							{
								nsgi.StoreGradesFound = true;
								sgi.StoreGrade = sgp.StoreGrade;
								sgi.WosIndex = sgp.WosIndex;
							}
							if (sgp.MinMaxChangeType == eChangeType.add ||
								sgp.MinMaxChangeType == eChangeType.update)
							{
								sgi.MinStock = sgp.MinStock;
								sgi.MaxStock = sgp.MaxStock;
								sgi.MinAd = sgp.MinAd;
								sgi.MinColor = sgp.MinColor;
								sgi.MaxColor = sgp.MaxColor;
                                sgi.ShipUpTo = sgp.ShipUpTo;    // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
								if (sgi.MinStock != Include.Undefined ||
									sgi.MaxStock != Include.Undefined ||
									sgi.MinAd != Include.Undefined ||
									sgi.MinColor != Include.Undefined ||
									sgi.MaxColor != Include.Undefined ||
                                    sgi.ShipUpTo != Include.Undefined)  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
								{
									nsgi.MinMaxesFound = true;
								}
							}
						
							nsgi.StoreGrades.Add(sgi);
						}
					}

					if (newEntry)
					{
						_storeGradesByRID.Add(nodeRID, nsgi);
					}
					else
					{
						_storeGradesByRID[nodeRID] = nsgi;
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					storeGrades_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:StoreGradesUpdate writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:StoreGradesUpdate writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the store grades for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <param name="forAdmin">A flag identifying if the list is being retrieved for use by the admin app</param>
		/// <returns></returns>
		static public StoreGradeList GetStoreGradeList(int nodeRID, bool forCopy, bool forAdmin)
		{
            try
            {
                bool foundStoreGrades = false;
                bool foundMinMaxes = false;
                bool setInheritance = false;
                NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
                NodeAncestorList nal = GetNodeAncestorList(nodeRID, ni.HomeHierarchyRID);
                StoreGradeList sgl = new StoreGradeList(eProfileType.StoreGrade);
                foreach (NodeAncestorProfile nap in nal)
                {
                    sgl = GetStoreGrade(nap.Key, sgl, forCopy, forAdmin, setInheritance, ref foundStoreGrades, ref foundMinMaxes);
                    if (foundStoreGrades && foundMinMaxes) 	//  stop lookup if found store grades and min/maxes
                    {
                        break;
                    }
                    setInheritance = true;
                }
                // remove unmatched grades (do not use enumerator; delete impacts positioning)
                int gradeCount = sgl.Count;
                int currentIndex = 0;
                for (int i = 0; i < gradeCount; i++)
                {
                    StoreGradeProfile sgp = (StoreGradeProfile)sgl[currentIndex];
                    if (sgp.StoreGrade == null)
                    {
                        sgl.RemoveAt(currentIndex);
                    }
                    else
                    {
                        ++currentIndex;
                    }
                }
                return sgl;
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
		}

		/// <summary>
		/// Gets the store grades for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="sgl">An instance of the StoreGradeList class containing instances of the StoreGradeProfile class</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <param name="forAdmin">A flag identifying if the list is being retrieved for use by the admin app</param>
		/// <param name="setInheritance">A flag identifying if inheritance information should be set</param>
		/// <param name="foundStoreGrades">A reference to a flag that identifies if store grade information has been found</param>
		/// <param name="foundMinMaxes">A reference to a flag that identifies if min/max information has been found</param>
		/// <returns></returns>
		static private StoreGradeList GetStoreGrade(int nodeRID, StoreGradeList sgl, bool forCopy, bool forAdmin, 
			bool setInheritance, ref bool foundStoreGrades, ref bool foundMinMaxes)
		{
			NodeStoreGradesInfo nsgi = null;
			try
			{
				storeGrades_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
                    if (_storeGradesByRID.ContainsKey(nodeRID))
                    {
                        nsgi = (NodeStoreGradesInfo)_storeGradesByRID[nodeRID];
                    }
				}        
				finally
				{
					// Ensure that the lock is released.
					storeGrades_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetStoreGrade reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetStoreGrade reader lock has timed out");
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}

			try
			{
				if (nsgi == null)
				{
					nsgi = new NodeStoreGradesInfo();
					LoadStoreGrades(nodeRID, nsgi);
					UpdateStoreGradeHash(nodeRID, nsgi);
				}

				bool foundProfile = false;
				StoreGradeProfile sgp;
				bool wrkStoreGradesFound = false;
				bool wrkMinMaxesFound = false;

				if (!foundStoreGrades || !foundMinMaxes)
				{
					foreach (StoreGradeInfo sgi in nsgi.StoreGrades)
					{
						if (sgl.Contains(sgi.Boundary))
						{
							sgp = (StoreGradeProfile)sgl.FindKey(sgi.Boundary);
							foundProfile = true;
						}
						else
						{
							sgp = new StoreGradeProfile(sgi.Boundary);
							foundProfile = false;
						}
						sgp.Boundary = sgi.Boundary;
                        sgp.OriginalBoundary = sgi.Boundary;  // TT#2404 - JSmith - Questions on Store Grades in OTS Forecast Review

						// add store grade information if not already found
						if (nsgi.StoreGradesFound && !foundStoreGrades)
						{
							wrkStoreGradesFound = true;
							sgp.StoreGradeFound = true;
							sgp.StoreGrade = sgi.StoreGrade;
							sgp.WosIndex = sgi.WosIndex;
							if (forCopy)
							{
								sgp.StoreGradeChangeType = eChangeType.update;
							}
							if (setInheritance)
							{
								sgp.StoreGradesIsInherited = true;
								sgp.StoreGradesInheritedFromNodeRID = nodeRID;
							}
						}

						// add min/max information if not already found
						if (nsgi.MinMaxesFound && !foundMinMaxes)
						{
							wrkMinMaxesFound = true;
							sgp.MinMaxFound = true;
							sgp.MinStock = sgi.MinStock;
							sgp.MaxStock = sgi.MaxStock;
							sgp.MinAd = sgi.MinAd;
							sgp.MinColor = sgi.MinColor;
							sgp.MaxColor = sgi.MaxColor;
                            sgp.ShipUpTo = sgi.ShipUpTo;  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)

							if (forCopy)
							{
								// If we're adding store grades, then this is an update.
								// otherwise it's an add.
								if (sgp.StoreGradeChangeType == eChangeType.add)
									sgp.MinMaxChangeType = eChangeType.update;
								else
									sgp.MinMaxChangeType = eChangeType.add;
							}


							if (setInheritance)
							{
								sgp.MinMaxesIsInherited = true;
								sgp.MinMaxesInheritedFromNodeRID = nodeRID;
							}
						}

						// override min/max values if not for admin
						if (!forAdmin && !forCopy)	
						{
							if (sgp.MinStock == Include.Undefined)
							{
								sgp.MinStock = 0;
							}
							if (sgp.MaxStock == Include.Undefined)
							{
								sgp.MaxStock = int.MaxValue;
							}
							if (sgp.MinAd == Include.Undefined)
							{
								sgp.MinAd = 0;
							}
							if (sgp.MinColor == Include.Undefined)
							{
								sgp.MinColor = 0;
							}
							if (sgp.MaxColor == Include.Undefined)
							{
								sgp.MaxColor = int.MaxValue;
							}
                            // BEGIN TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
                            if (sgp.ShipUpTo == Include.Undefined)
                            {
                                // Begin TT#617 - RMatelic - Allocation Override - Ship Up To Rule (#36, #39)
                                //sgp.ShipUpTo = int.MaxValue;
                                sgp.ShipUpTo = 0;
                                // End TT#617
                            }
                            // END TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)

						}

						if (foundProfile)
						{
							sgl.Update(sgp);
						}
						else
						{
							sgl.Add(sgp);
						}
					}
					if (wrkStoreGradesFound)
					{
						foundStoreGrades = true;
						foundMinMaxes = true;
					}
					if (wrkMinMaxesFound)
					{
						foundMinMaxes = true;
					}
				}
				return sgl;
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Loads the store grades for the node.
		/// </summary>
		///<remarks>
		///Store grades are loaded as needed for performance.
		///</remarks>
		static private void LoadStoreGrades(int aNodeRID, NodeStoreGradesInfo aNodeStoreGradesInfo)
		{
			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.StoreGrades_Read(aNodeRID);

				foreach(DataRow dr in dt.Rows)
				{
					StoreGradeInfo sgi = new StoreGradeInfo();

					sgi.StoreGrade = Convert.ToString(dr["GRADE_CODE"], CultureInfo.CurrentUICulture);
					sgi.Boundary = Convert.ToInt32(dr["BOUNDARY"], CultureInfo.CurrentUICulture);
					sgi.WosIndex = Convert.ToInt32(dr["WOS_INDEX"], CultureInfo.CurrentUICulture);
					sgi.MinStock = Convert.ToInt32(dr["MINIMUM_STOCK"], CultureInfo.CurrentUICulture);
					sgi.MaxStock = Convert.ToInt32(dr["MAXIMUM_STOCK"], CultureInfo.CurrentUICulture);
					sgi.MinAd = Convert.ToInt32(dr["MINIMUM_AD"], CultureInfo.CurrentUICulture);
					//				sgi.MaxAd = Convert.ToInt32(dr["MAXIMUM_AD"]);
					sgi.MinColor = Convert.ToInt32(dr["MINIMUM_COLOR"], CultureInfo.CurrentUICulture);
					sgi.MaxColor = Convert.ToInt32(dr["MAXIMUM_COLOR"], CultureInfo.CurrentUICulture);
                    sgi.ShipUpTo = Convert.ToInt32(dr["SHIP_UP_TO"], CultureInfo.CurrentUICulture);  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
					if (sgi.StoreGrade != "" ||
						sgi.WosIndex != Include.Undefined)
					{
						aNodeStoreGradesInfo.StoreGradesFound = true;
					}

					if (sgi.MinStock != Include.Undefined || 
						sgi.MaxStock != Include.Undefined ||
						sgi.MinAd != Include.Undefined ||
						sgi.MinColor != Include.Undefined ||
						sgi.MaxColor != Include.Undefined ||
                        sgi.ShipUpTo != Include.Undefined)  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
					{
						aNodeStoreGradesInfo.MinMaxesFound = true;
					}

					aNodeStoreGradesInfo.StoreGrades.Add(sgi);
				}
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}


		static private void UpdateStoreGradeHash(int aNodeRID, NodeStoreGradesInfo aNodeStoreGradesInfo)
		{
			try
			{
				storeGrades_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					if (!_storeGradesByRID.ContainsKey(aNodeRID))
					{
						_storeGradesByRID.Add(aNodeRID, aNodeStoreGradesInfo);
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					storeGrades_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:UpdateStoreGradeHash writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:UpdateStoreGradeHash writer lock has timed out");
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Requests the session add or update store grade information 
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aMinMaxesProfile">A instance of NodeStockMinMaxesProfile class which contains stock min/max information</param>
		static public void StockMinMaxUpdate(int aNodeRID, NodeStockMinMaxesProfile aMinMaxesProfile)
		{
			try
			{
				NodeStockMinMaxesInfo minMaxesInfo = null;
				NodeStockMinMaxSetInfo minMaxSetInfo = null;
				NodeStockMinMaxBoundaryInfo minMaxBoundaryInfo;
				NodeStockMinMaxInfo minMaxInfo = null;
				stockMinMaxes_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					switch (aMinMaxesProfile.NodeStockMinMaxChangeType)
					{
						case eChangeType.none: 
						{
							break;
						}
						case eChangeType.add: 
						case eChangeType.update: 
						{
							bool dataFound = false;
							NodeInfo ni = GetNodeInfoByRID(aNodeRID, false);
                            if (_stockMinMaxesByRID.ContainsKey(aNodeRID))
                            {
                                minMaxesInfo = (NodeStockMinMaxesInfo)_stockMinMaxesByRID[aNodeRID];
                            }
							if (minMaxesInfo == null)
							{
								minMaxesInfo = new NodeStockMinMaxesInfo();
							}
							else
							{
								minMaxesInfo.NodeSetList.Clear();
							}
 
							foreach (NodeStockMinMaxSetProfile minMaxSetProfile in aMinMaxesProfile.NodeSetList)
							{
								minMaxSetInfo = new NodeStockMinMaxSetInfo(minMaxSetProfile.Key);
								foreach (NodeStockMinMaxProfile minMaxProfile in minMaxSetProfile.Defaults.MinMaxList.ArrayList)
								{
									minMaxInfo = new NodeStockMinMaxInfo(minMaxProfile.Key, minMaxProfile.Minimum, minMaxProfile.Maximum);
									minMaxSetInfo.Defaults.MinMaxList.Add(minMaxInfo);
									dataFound = true;
								}

								foreach (NodeStockMinMaxBoundaryProfile minMaxBoundaryProfile in minMaxSetProfile.BoundaryList.ArrayList)
								{
									minMaxBoundaryInfo = new NodeStockMinMaxBoundaryInfo(minMaxBoundaryProfile.Key);
									foreach (NodeStockMinMaxProfile minMaxProfile in minMaxBoundaryProfile.MinMaxList.ArrayList)
									{
										minMaxInfo = new NodeStockMinMaxInfo(minMaxProfile.Key, minMaxProfile.Minimum, minMaxProfile.Maximum);
										minMaxBoundaryInfo.MinMaxList.Add(minMaxInfo);
									}
									minMaxSetInfo.BoundaryList.Add(minMaxBoundaryInfo);
									dataFound = true;
								}
								minMaxesInfo.NodeSetList.Add(minMaxSetInfo);
							}

							if (dataFound)
							{
								_stockMinMaxesByRID[aNodeRID] = minMaxesInfo;
								ni.StockMinMaxSgRID = aMinMaxesProfile.NodeStockStoreGroupRID;
							}
							else
							{
								_stockMinMaxesByRID.Remove(aNodeRID);
								ni.StockMinMaxSgRID = Include.NoRID;
							}
							SetNodeCacheByRID(aNodeRID, ni);

							break;
						}
						case eChangeType.delete: 
						{
							_stockMinMaxesByRID.Remove(aNodeRID);
							NodeInfo ni = GetNodeInfoByRID(aNodeRID, false);
							ni.StockMinMaxSgRID = Include.NoRID;
							SetNodeCacheByRID(aNodeRID, ni);
							break;
						}
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					stockMinMaxes_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:StockMinMaxUpdate writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:StockMinMaxUpdate writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the stock min/maxes for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <returns></returns>
		static public NodeStockMinMaxesProfile GetStockMinMaxes(int nodeRID)
		{
			try
			{
				NodeStockMinMaxesProfile minMaxesProfile = null;
				NodeInfo ni =  HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
				NodeAncestorList nal =  GetNodeAncestorList(nodeRID, ni.HomeHierarchyRID);
				foreach (NodeAncestorProfile nap in nal)
				{
					minMaxesProfile = GetStockMinMax(nap.Key);
					if (minMaxesProfile != null &&
						minMaxesProfile.NodeSetList.Count > 0)				
					{
						if (nap.Key != nodeRID)
						{
							minMaxesProfile.NodeStockMinMaxsIsInherited = true;
							minMaxesProfile.NodeStockMinMaxsInheritedFromNodeRID = nap.Key;
							ni =  HierarchyServerGlobal.GetNodeInfoByRID(nap.Key, false);
						}
						minMaxesProfile.NodeStockMinMaxFound = true;
						minMaxesProfile.NodeStockStoreGroupRID = ni.StockMinMaxSgRID;
						break;
					}
				}

				if (minMaxesProfile == null)
				{
					minMaxesProfile = new NodeStockMinMaxesProfile(nodeRID);
				}
				
				return minMaxesProfile;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the stock min/maxes for the node.
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <returns></returns>
		static private NodeStockMinMaxesProfile GetStockMinMax(int aNodeRID)
		{
			NodeStockMinMaxesInfo minMaxesInfo = null;
			NodeStockMinMaxesProfile minMaxesProfile = null;
			NodeStockMinMaxSetProfile minMaxSetProfile;
			NodeStockMinMaxBoundaryProfile minMaxBoundaryProfile;
			NodeStockMinMaxProfile minMaxProfile;
			try
			{
				stockMinMaxes_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
                    if (_stockMinMaxesByRID.ContainsKey(aNodeRID))
                    {
                        minMaxesInfo = (NodeStockMinMaxesInfo)_stockMinMaxesByRID[aNodeRID];
                    }
				}        
				finally
				{
					// Ensure that the lock is released.
					stockMinMaxes_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetStockMinMax reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetStockMinMax reader lock has timed out");
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}

			try
			{
				if (minMaxesInfo == null)
				{
					minMaxesInfo = new NodeStockMinMaxesInfo();
					LoadStockMinMaxes(aNodeRID, minMaxesInfo);
					UpdateStockMinMaxesHash(aNodeRID, minMaxesInfo);
				}

				if (minMaxesInfo.NodeSetList.Count > 0)
				{
					minMaxesProfile = new NodeStockMinMaxesProfile(aNodeRID);
					foreach (NodeStockMinMaxSetInfo minMaxSetInfo in minMaxesInfo.NodeSetList)
					{
						minMaxSetProfile = new NodeStockMinMaxSetProfile(minMaxSetInfo.SetRID);

						foreach (NodeStockMinMaxInfo minMaxInfo in minMaxSetInfo.Defaults.MinMaxList)
						{
							minMaxProfile = new NodeStockMinMaxProfile(minMaxInfo.DateRangeKey,
								minMaxInfo.Minimum,
								minMaxInfo.Maximum);
							minMaxSetProfile.Defaults.MinMaxList.Add(minMaxProfile);
						}

						foreach (NodeStockMinMaxBoundaryInfo minMaxBoundaryInfo in minMaxSetInfo.BoundaryList)
						{
							minMaxBoundaryProfile = new NodeStockMinMaxBoundaryProfile(minMaxBoundaryInfo.Boundary);
							foreach (NodeStockMinMaxInfo minMaxInfo in minMaxBoundaryInfo.MinMaxList)
							{
								minMaxProfile = new NodeStockMinMaxProfile(minMaxInfo.DateRangeKey,
									minMaxInfo.Minimum,
									minMaxInfo.Maximum);
								minMaxBoundaryProfile.MinMaxList.Add(minMaxProfile);
							}
							minMaxSetProfile.BoundaryList.Add(minMaxBoundaryProfile);
						}

						minMaxesProfile.NodeSetList.Add(minMaxSetProfile);
					}
				}
				return minMaxesProfile;
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Loads the stock min/maxes for the node.
		/// </summary>
		///<remarks>
		///Stock min/maxes are loaded as needed for performance.
		///</remarks>
		static private void LoadStockMinMaxes(int aNodeRID, NodeStockMinMaxesInfo aNodeStockMinMaxesInfo)
		{
			try
			{
				const int INIT_VALUE = -22;
				int currentSglRid = INIT_VALUE, currentBoundary = INIT_VALUE;
				int boundary, sglRid;
				int cdrRid;
				int min, max;
				NodeStockMinMaxSetInfo minMaxSetInfo = null;
				NodeStockMinMaxBoundaryInfo minMaxBoundaryInfo = null;
				NodeStockMinMaxInfo minMaxInfo = null;
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.StockMinMaxes_Read(aNodeRID);

				foreach(DataRow dr in dt.Rows)
				{
					sglRid = Convert.ToInt32(dr["SGL_RID"], CultureInfo.CurrentUICulture);
					boundary = Convert.ToInt32(dr["BOUNDARY"], CultureInfo.CurrentUICulture);
					cdrRid = Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture);
					if (dr["MIN_STOCK"] == DBNull.Value)
					{
						min = int.MinValue;
					}
					else
					{
						min = Convert.ToInt32(dr["MIN_STOCK"], CultureInfo.CurrentUICulture);
					}
					if (dr["MAX_STOCK"] == DBNull.Value)
					{
						max = int.MaxValue;
					}
					else
					{
						max = Convert.ToInt32(dr["MAX_STOCK"], CultureInfo.CurrentUICulture);
					}

					minMaxInfo = new NodeStockMinMaxInfo(cdrRid, min, max);

					if (currentSglRid == INIT_VALUE)
					{
						minMaxSetInfo = new NodeStockMinMaxSetInfo(sglRid);
					}
					else if (sglRid != currentSglRid)
					{
						aNodeStockMinMaxesInfo.NodeSetList.Add(minMaxSetInfo);
						minMaxSetInfo = new NodeStockMinMaxSetInfo(sglRid);
					}

					if (boundary == Include.Undefined)
					{
						minMaxSetInfo.Defaults.MinMaxList.Add(minMaxInfo);
					}
					else if (currentBoundary == INIT_VALUE ||
						boundary != currentBoundary)
					{
						minMaxBoundaryInfo = new NodeStockMinMaxBoundaryInfo(boundary);
						minMaxBoundaryInfo.MinMaxList.Add(minMaxInfo);
						minMaxSetInfo.BoundaryList.Add(minMaxBoundaryInfo);
					}
					else
					{
						minMaxBoundaryInfo.MinMaxList.Add(minMaxInfo);
					}

					currentSglRid = sglRid;
					currentBoundary = boundary;
				}

				// catch the last set
				if (minMaxSetInfo != null &&
					(minMaxSetInfo.BoundaryList.Count > 0 ||
					minMaxSetInfo.Defaults.MinMaxList.Count > 0))
				{
					aNodeStockMinMaxesInfo.NodeSetList.Add(minMaxSetInfo);
				}
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static private void UpdateStockMinMaxesHash(int aNodeRID, NodeStockMinMaxesInfo aNodeStockMinMaxesInfo)
		{
			try
			{
				stockMinMaxes_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					if (!_stockMinMaxesByRID.ContainsKey(aNodeRID))
					{
						_stockMinMaxesByRID.Add(aNodeRID, aNodeStockMinMaxesInfo);
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					stockMinMaxes_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:UpdateStockMinMaxesHash writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:UpdateStockMinMaxesHash writer lock has timed out");
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Adds or updates information about a store capacities
		/// </summary>
		/// <param name="scl">An instance of the StoreCapacityList class containing StoreCapacityProfile objects</param>
		/// <param name="nodeRID">The record id of the node</param>
        static public void StoreCapacityUpdate(int nodeRID, StoreCapacityList scl)
        {
            NodeStoreCapacityInfo nsci = null;
            try
            {
                storeCapacity_rwl.AcquireWriterLock(WriterLockTimeOut);
                try
                {
                    if (_storeCapacityByRID.ContainsKey(nodeRID))
                    {
                        nsci = (NodeStoreCapacityInfo)_storeCapacityByRID[nodeRID];
                    }
                    if (nsci == null)
                    {
                        nsci = new NodeStoreCapacityInfo();
                    }
                    StoreCapacityInfo sci;

                    foreach (StoreCapacityProfile scp in scl)
                    {
						// Begin Track #5963 - JSmith - Capacity values not clearing
						if (scp.StoreCapacityChangeType != eChangeType.none)
						{
						// End Track #5963
                        sci = (StoreCapacityInfo)nsci.StoreCapacity[scp.Key];
                        if (sci == null)
                        {
                            sci = new StoreCapacityInfo();
                        }

                        if (scp.StoreCapacityChangeType == eChangeType.delete)
                        {
                            nsci.StoreCapacity.Remove(scp.Key);
                        }
                        else
                        {
                            sci.StoreRID = scp.Key;
                            sci.StoreCapacity = scp.StoreCapacity;
                            nsci.StoreCapacity[sci.StoreRID] = sci;
                        }
						// Begin Track #5963 - JSmith - Capacity values not clearing
						}
						// End Track #5963
                    }

                    _storeCapacityByRID[nodeRID] = nsci;
                }
                finally
                {
                    // Ensure that the lock is released.
                    storeCapacity_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:StoreCapacityUpdate writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:StoreCapacityUpdate writer lock has timed out");
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

		/// <summary>
		/// Gets the store capacities for the node.
		/// </summary>
		/// <param name="storeList">The ProfileList of store for which capacities are to be determined</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="stopOnFind">This switch tells the routine to stop when the first node with capacities is found</param>
		/// <param name="forCopy">This node tells the routine that the lookup is done for copy purposes</param>
		/// <returns></returns>
        static public StoreCapacityList GetStoreCapacityList(ProfileList storeList, int nodeRID, bool stopOnFind, bool forCopy)
        {
            try
            {
                bool setInheritance = false;
                int lowestNodeRID = Include.NoRID;
                NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
                NodeAncestorList nal = GetNodeAncestorList(nodeRID, ni.HomeHierarchyRID);
                StoreCapacityList scl = new StoreCapacityList(eProfileType.StoreCapacity);
                foreach (NodeAncestorProfile nap in nal)
                {
                    scl = GetStoreCapacity(storeList, nap.Key, scl, forCopy, setInheritance);
                    if (lowestNodeRID == Include.NoRID && scl.Count > 0)
                    {
                        lowestNodeRID = nap.Key;
                    }

                    if (scl.Count == storeList.Count ||		//  stop lookup if have capacity for each store
                        forCopy)							//  or stop on first node if copy
                    {
                        break;
                    }
                    else
                        if (stopOnFind && scl.Count > 0)	//  stop if you find capacities for any store
                        {
                            break;
                        }
                    setInheritance = true;
                }
                if (stopOnFind && !forCopy)
                {
                    foreach (StoreProfile sp in storeList)	// set all store not found to int.max
                    {
                        if (!scl.Contains(sp.Key))
                        {
                            StoreCapacityProfile scp = new StoreCapacityProfile(sp.Key);
                            scp.StoreCapacity = int.MaxValue;
                            scp.NodeRID = lowestNodeRID;

                            scl.Add(scp);
                        }
                    }
                }

                return scl;
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

		/// <summary>
		/// Gets the store capacities for a node.
		/// </summary>
		/// <param name="storeList">The ProfileList of store for which capacities are to be determined</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <param name="scl">An instance of the StoreCapacityList class containing StoreCapacityProfile objects</param>
		/// <param name="setInheritance">Identifies if inheritance information should be set</param>
		/// <returns></returns>
		static private StoreCapacityList GetStoreCapacity(ProfileList storeList, int nodeRID, 
			StoreCapacityList scl, bool forCopy, bool setInheritance)
		{
			NodeStoreCapacityInfo nsci = null;
			try
			{
				storeCapacity_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
                    if (_storeCapacityByRID.ContainsKey(nodeRID))
                    {
                        nsci = (NodeStoreCapacityInfo)_storeCapacityByRID[nodeRID];
                    }
				}        
				finally
				{
					// Ensure that the lock is released.
					storeCapacity_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetStoreCapacity reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetStoreCapacity reader lock has timed out");
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}

			try
			{
				if (nsci == null)
				{
					nsci = new NodeStoreCapacityInfo();
					LoadStoreCapacity(nodeRID, nsci);
					UpdateStoreCapacityHash(nodeRID, nsci);
				}

				StoreCapacityInfo sci;
				foreach (DictionaryEntry val in nsci.StoreCapacity)
				{
					sci = (StoreCapacityInfo)val.Value;
					if (storeList.Contains(sci.StoreRID))  // Do you need this store
					{
						if (!scl.Contains(sci.StoreRID))  // Do you already have this store
						{
							StoreCapacityProfile scp = new StoreCapacityProfile(sci.StoreRID);
							scp.StoreCapacity = sci.StoreCapacity;
							scp.NodeRID = nodeRID;
							scp.NewRecord = false;
							if (forCopy)
							{
								scp.StoreCapacityChangeType = eChangeType.add;
							}

							if (setInheritance)
							{
								scp.StoreCapacityIsInherited = true;
								scp.StoreCapacityInheritedFromNodeRID = nodeRID;
							}

							scl.Add(scp);
						}
					}
				}
				return scl;
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Loads the store capacities for the node.
		/// </summary>
		///<remarks>
		///Store capacity is loaded as needed for performance.
		///</remarks>
		static private void LoadStoreCapacity(int aNodeRID, NodeStoreCapacityInfo aNodeStoreCapacityInfo)
		{
			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.StoreCapacity_Read(aNodeRID);

				foreach(DataRow dr in dt.Rows)
				{
					StoreCapacityInfo sci = new StoreCapacityInfo();

					sci.StoreRID = Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture);
					sci.StoreCapacity = Convert.ToInt32(dr["ST_CAPACITY"], CultureInfo.CurrentUICulture);
				
					aNodeStoreCapacityInfo.StoreCapacity.Add(sci.StoreRID, sci);
				}
			}        
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static private void UpdateStoreCapacityHash(int aNodeRID, NodeStoreCapacityInfo aNodeStoreCapacityInfo)
		{
			try
			{
				storeCapacity_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					if (!_storeCapacityByRID.ContainsKey(aNodeRID))
					{
						_storeCapacityByRID.Add(aNodeRID, aNodeStoreCapacityInfo);
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					storeCapacity_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:UpdateStoreCapacityHash writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:UpdateStoreCapacityHash writer lock has timed out");
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Adds or updates information about a velocity grades
		/// </summary>
		/// <param name="vgl">An instance of the VelocityGradeList class containing VelocityGradeProfile objects</param>
		/// <param name="nodeRID">The record id of the node</param>
		static public void VelocityGradesUpdate(int nodeRID, VelocityGradeList vgl)
		{
            NodeVelocityGradesInfo nvgi = null;
			try
			{
				velocityGrades_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
                    if (_velocityGradesByRID.ContainsKey(nodeRID))
                    {
                        nvgi = (NodeVelocityGradesInfo)_velocityGradesByRID[nodeRID];
                    }
					if (nvgi == null)
					{
						nvgi = new NodeVelocityGradesInfo();
					}
					else
					{
						nvgi.VelocityGrades.Clear();
					}
					
					foreach (VelocityGradeProfile vgp in vgl)
					{
						//Begin TT#505 - JScott - Velocity - Apply Min/Max
						//if (vgp.VelocityGradeChangeType == eChangeType.add ||
						//    vgp.VelocityGradeChangeType == eChangeType.update)
						//{
						//    VelocityGradeInfo vgi = new VelocityGradeInfo();
						//    vgi.VelocityGrade = vgp.VelocityGrade;
						//    vgi.Boundary = vgp.Boundary;
						//    //					vgi.SellThruPct = vgp.SellThruPct;
						
						//    nvgi.VelocityGrades.Add(vgi);
						//}
						if (vgp.VelocityGradeChangeType != eChangeType.delete ||
							vgp.VelocityMinMaxChangeType != eChangeType.delete)
						{
							VelocityGradeInfo vgi = new VelocityGradeInfo();
							vgi.Boundary = vgp.Boundary;
							if (vgp.VelocityGradeChangeType == eChangeType.add ||
								vgp.VelocityGradeChangeType == eChangeType.update ||
								(vgp.VelocityGradeChangeType == eChangeType.none &&	// handle a change to min/maxes only
								vgp.VelocityGradeIsInherited == false))				// to put code back in StoreGradeInfo
							{
								nvgi.VelocityGradesFound = true;
								vgi.VelocityGrade = vgp.VelocityGrade;
							}
							if (vgp.VelocityMinMaxChangeType == eChangeType.add ||
								vgp.VelocityMinMaxChangeType == eChangeType.update)
							{
								vgi.VelocityMinStock = vgp.VelocityMinStock;
								vgi.VelocityMaxStock = vgp.VelocityMaxStock;
								vgi.VelocityMinAd = vgp.VelocityMinAd;
								if (vgi.VelocityMinStock != Include.Undefined ||
									vgi.VelocityMaxStock != Include.Undefined ||
									vgi.VelocityMinAd != Include.Undefined)
								{
									nvgi.VelocityMinMaxesFound = true;
								}
							}

							nvgi.VelocityGrades.Add(vgi);
						}
						//End TT#505 - JScott - Velocity - Apply Min/Max
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					velocityGrades_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:VelocityGradesUpdate writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:VelocityGradesUpdate writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the velocity grades for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns></returns>
		static public VelocityGradeList GetVelocityGradeList(int nodeRID, bool forCopy, bool forAdmin)
        {
            try
            {
				//Begin TT#505 - JScott - Velocity - Apply Min/Max
				//bool setInheritance = false;
				//NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
				//NodeAncestorList nal = GetNodeAncestorList(nodeRID, ni.HomeHierarchyRID);
				//VelocityGradeList vgl = new VelocityGradeList(eProfileType.VelocityGrade);
				//foreach (NodeAncestorProfile nap in nal)
				//{
				//    vgl = GetVelocityGrade(nap.Key, vgl, forCopy, setInheritance);
				//    if (vgl.Count > 0 ||		//  stop lookup if found velocity grades for any node
				//        forCopy)				//  or stop on first node if copy
				//    {
				//        break;
				//    }
				//    setInheritance = true;
				//}
				//return vgl;
				bool foundVelocityGrades = false;
				bool foundVelocityMinMaxes = false;
				bool setInheritance = false;
				NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
				NodeAncestorList nal = GetNodeAncestorList(nodeRID, ni.HomeHierarchyRID);
				VelocityGradeList vgl = new VelocityGradeList(eProfileType.VelocityGrade);
				foreach (NodeAncestorProfile nap in nal)
				{
					vgl = GetVelocityGrade(nap.Key, vgl, forCopy, forAdmin, setInheritance, ref foundVelocityGrades, ref foundVelocityMinMaxes);
					if (foundVelocityGrades && foundVelocityMinMaxes) 	//  stop lookup if found velocity grades and min/maxes
					{
						break;
					}
					setInheritance = true;
				}
				// remove unmatched grades (do not use enumerator; delete impacts positioning)
				int gradeCount = vgl.Count;
				int currentIndex = 0;
				for (int i = 0; i < gradeCount; i++)
				{
					VelocityGradeProfile vgp = (VelocityGradeProfile)vgl[currentIndex];
					if (vgp.VelocityGrade == null)
					{
						vgl.RemoveAt(currentIndex);
					}
					else
					{
						++currentIndex;
					}
				}
				return vgl;
				//End TT#505 - JScott - Velocity - Apply Min/Max
			}
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

		/// <summary>
		/// Gets the velocity grades for a node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="vgl">An instance of the VelocityGradeList class containing VelocityGradeProfile objects</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <param name="setInheritance">Identifies if inheritance information should be set</param>
		/// <returns></returns>
		//Begin TT#505 - JScott - Velocity - Apply Min/Max
		//static private VelocityGradeList GetVelocityGrade(int nodeRID, VelocityGradeList vgl, 
		//    bool forCopy, bool setInheritance)
		static private VelocityGradeList GetVelocityGrade(int nodeRID, VelocityGradeList vgl,
			bool forCopy, bool forAdmin, bool setInheritance, ref bool foundVelocityGrades, ref bool foundVelocityMinMaxes)
		//End TT#505 - JScott - Velocity - Apply Min/Max
		{
			NodeVelocityGradesInfo nvgi = null;
			try
			{
				velocityGrades_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
                    if (_velocityGradesByRID.ContainsKey(nodeRID))
                    {
                        nvgi = (NodeVelocityGradesInfo)_velocityGradesByRID[nodeRID];
                    }
				}        
				finally
				{
					// Ensure that the lock is released.
					velocityGrades_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetVelocityGrade reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetVelocityGrade reader lock has timed out");
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}

			// read data from cache

			try
			{
				if (nvgi == null)
				{
					nvgi = new NodeVelocityGradesInfo();
					LoadVelocityGrades(nodeRID, nvgi);
					UpdateVelocityGradeHash(nodeRID, nvgi);
				}

				//Begin TT#505 - JScott - Velocity - Apply Min/Max
				//int index=0;
				//foreach (VelocityGradeInfo vgi in nvgi.VelocityGrades)
				//{
				//    VelocityGradeProfile vgp = new VelocityGradeProfile(index);
				//    vgp.VelocityGradeChangeType = eChangeType.none;
				//    vgp.VelocityGrade = vgi.VelocityGrade;
				//    vgp.Boundary = vgi.Boundary;
				//    if (forCopy)
				//    {
				//        vgp.VelocityGradeChangeType = eChangeType.add;
				//    }

				//    if (setInheritance)
				//    {
				//        vgp.VelocityGradeIsInherited = true;
				//        vgp.VelocityGradeInheritedFromNodeRID = nodeRID;
				//    }
						
				//    vgl.Add(vgp);
				//    ++index;
				//}
				//return vgl;
				bool foundProfile = false;
				VelocityGradeProfile vgp;
				bool wrkVelocityGradesFound = false;
				bool wrkVelocityMinMaxesFound = false;

				if (!foundVelocityGrades || !foundVelocityMinMaxes)
				{
					foreach (VelocityGradeInfo vgi in nvgi.VelocityGrades)
					{
						if (vgl.Contains(vgi.Boundary))
						{
							vgp = (VelocityGradeProfile)vgl.FindKey(vgi.Boundary);
							foundProfile = true;
						}
						else
						{
							vgp = new VelocityGradeProfile(vgi.Boundary);
							foundProfile = false;
						}
						vgp.Boundary = vgi.Boundary;

						// add store grade information if not already found
						if (nvgi.VelocityGradesFound && !foundVelocityGrades)
						{
							wrkVelocityGradesFound = true;
							vgp.VelocityGradeFound = true;
							vgp.VelocityGrade = vgi.VelocityGrade;
							if (forCopy)
							{
								vgp.VelocityGradeChangeType = eChangeType.update;
							}
							if (setInheritance)
							{
								vgp.VelocityGradeIsInherited = true;
								vgp.VelocityGradeInheritedFromNodeRID = nodeRID;
							}
						}

						// add min/max information if not already found
						if (nvgi.VelocityMinMaxesFound && !foundVelocityMinMaxes)
						{
							wrkVelocityMinMaxesFound = true;
							vgp.VelocityMinMaxFound = true;
							vgp.VelocityMinStock = vgi.VelocityMinStock;
							vgp.VelocityMaxStock = vgi.VelocityMaxStock;
							vgp.VelocityMinAd = vgi.VelocityMinAd;

							if (forCopy)
							{
								// If we're adding store grades, then this is an update.
								// otherwise it's an add.
								if (vgp.VelocityGradeChangeType == eChangeType.add)
									vgp.VelocityMinMaxChangeType = eChangeType.update;
								else
									vgp.VelocityMinMaxChangeType = eChangeType.add;
							}

							if (setInheritance)
							{
								vgp.VelocityMinMaxesIsInherited = true;
								vgp.VelocityMinMaxesInheritedFromNodeRID = nodeRID;
							}
						}

						// override min/max values if not for admin
						if (!forAdmin && !forCopy)
						{
							if (vgp.VelocityMinStock == Include.Undefined)
							{
								vgp.VelocityMinStock = 0;
							}
							if (vgp.VelocityMaxStock == Include.Undefined)
							{
								vgp.VelocityMaxStock = 0;
							}
							if (vgp.VelocityMinAd == Include.Undefined)
							{
								vgp.VelocityMinAd = 0;
							}
						}

						if (foundProfile)
						{
							vgl.Update(vgp);
						}
						else
						{
							vgl.Add(vgp);
						}
					}
					if (wrkVelocityGradesFound)
					{
						foundVelocityGrades = true;
						foundVelocityMinMaxes = true;
					}
					if (wrkVelocityMinMaxesFound)
					{
						foundVelocityMinMaxes = true;
					}
				}
				return vgl;
				//End TT#505 - JScott - Velocity - Apply Min/Max
			}        
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Loads the velocity grades for the node.
		/// </summary>
		///<remarks>
		///Store grades are loaded as needed for performance.
		///</remarks>
		static private void LoadVelocityGrades(int aNodeRID, NodeVelocityGradesInfo nvgi)
		{
			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.VelocityGrades_Read(aNodeRID);

				foreach(DataRow dr in dt.Rows)
				{
					VelocityGradeInfo vgi = new VelocityGradeInfo();

					vgi.VelocityGrade = Convert.ToString(dr["GRADE_CODE"], CultureInfo.CurrentUICulture);
					vgi.Boundary = Convert.ToInt32(dr["BOUNDARY"], CultureInfo.CurrentUICulture);
					//Begin TT#505 - JScott - Velocity - Apply Min/Max
					vgi.VelocityMinStock = Convert.ToInt32(dr["MINIMUM_STOCK"], CultureInfo.CurrentUICulture);
					vgi.VelocityMaxStock = Convert.ToInt32(dr["MAXIMUM_STOCK"], CultureInfo.CurrentUICulture);
					vgi.VelocityMinAd = Convert.ToInt32(dr["MINIMUM_AD"], CultureInfo.CurrentUICulture);
					if (vgi.VelocityGrade != "")
					{
						nvgi.VelocityGradesFound = true;
					}

					if (vgi.VelocityMinStock != Include.Undefined ||
						vgi.VelocityMaxStock != Include.Undefined ||
						vgi.VelocityMinAd != Include.Undefined)
					{
						nvgi.VelocityMinMaxesFound = true;
					}

					//End TT#505 - JScott - Velocity - Apply Min/Max
					nvgi.VelocityGrades.Add(vgi);
				}
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static private void UpdateVelocityGradeHash(int aNodeRID, NodeVelocityGradesInfo aNodeVelocityGradesInfo)
		{
			try
			{
				velocityGrades_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					if (!_velocityGradesByRID.ContainsKey(aNodeRID))
					{
						_velocityGradesByRID.Add(aNodeRID, aNodeVelocityGradesInfo);
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					velocityGrades_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:UpdateVelocityGradeHash writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:UpdateVelocityGradeHash writer lock has timed out");
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Adds or updates information about a sell thru percents
		/// </summary>
		/// <param name="stpl">An instance of the SellThruPctList class containing SellThruPctProfile objects </param>
		/// <param name="nodeRID">The record id of the node</param>
		static public void SellThruPctsUpdate(int nodeRID, SellThruPctList stpl)
		{
            NodeSellThruPctsInfo nstpi = null;
			try
			{
				sellThruPcts_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
                    if (_sellThruPctsByRID.ContainsKey(nodeRID))
                    {
                        nstpi = (NodeSellThruPctsInfo)_sellThruPctsByRID[nodeRID];
                    }
					if (nstpi == null)
					{
						nstpi = new NodeSellThruPctsInfo();
					}
					else
					{
						nstpi.SellThruPcts.Clear();
					}
					
					foreach (SellThruPctProfile stpp in stpl)
					{
						if (stpp.SellThruPctChangeType == eChangeType.add ||
							stpp.SellThruPctChangeType == eChangeType.update)
						{
							SellThruPctInfo stpi = new SellThruPctInfo();
							stpi.SellThruPct = stpp.SellThruPct;
						
							nstpi.SellThruPcts.Add(stpi);
						}
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					sellThruPcts_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:SellThruPctsUpdate writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:SellThruPctsUpdate writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the sell thru percents for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns></returns>
        static public SellThruPctList GetSellThruPctList(int nodeRID, bool forCopy)
        {
            try
            {
                bool setInheritance = false;
                NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
                NodeAncestorList nal = GetNodeAncestorList(nodeRID, ni.HomeHierarchyRID);
                SellThruPctList stpl = new SellThruPctList(eProfileType.SellThruPct);
                foreach (NodeAncestorProfile nap in nal)
                {
                    stpl = GetSellThruPcts(nap.Key, stpl, forCopy, setInheritance);
                    if (stpl.Count > 0 ||		//  stop lookup if found sell thru percents for any node
                        forCopy)				//  or stop on first node if copy
                    {
                        break;
                    }
                    setInheritance = true;
                }
                return stpl;
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

		/// <summary>
		/// Gets the sell thru percents for a node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="stpl">An instance of the SellThruPctList class containing SellThruPctProfile objects</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <param name="setInheritance">Identifies if inheritance information should be set</param>
		/// <returns></returns>
		static private SellThruPctList GetSellThruPcts(int nodeRID, SellThruPctList stpl, 
			bool forCopy, bool setInheritance)
		{
			NodeSellThruPctsInfo nstpi = null;
			try
			{
				sellThruPcts_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
                    if (_sellThruPctsByRID.ContainsKey(nodeRID))
                    {
                        nstpi = (NodeSellThruPctsInfo)_sellThruPctsByRID[nodeRID];
                    }
				}        
				finally
				{
					// Ensure that the lock is released.
					sellThruPcts_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetSellThruPcts reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetSellThruPcts reader lock has timed out");
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}

			try
			{
				if (nstpi == null)
				{
					nstpi = new NodeSellThruPctsInfo();
					LoadSellThruPcts(nodeRID, nstpi);
					UpdateSellThruPctsHash(nodeRID, nstpi);
				}
				int index=0;
				foreach (SellThruPctInfo stpi in nstpi.SellThruPcts)
				{
					SellThruPctProfile stpp = new SellThruPctProfile(index);
					if (forCopy)
					{
						stpp.SellThruPctChangeType = eChangeType.add;
					}
					else
					{
						stpp.SellThruPctChangeType = eChangeType.none;
					}
					stpp.SellThruPct = stpi.SellThruPct;
					
					if (setInheritance)
					{
						stpp.SellThruPctIsInherited = true;
						stpp.SellThruPctInheritedFromNodeRID = nodeRID;
					}

					stpl.Add(stpp);
					++index;
				}
				return stpl;
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Loads the sell thru percentages for the node.
		/// </summary>
		///<remarks>
		///sell thru percentages are loaded as needed for performance.
		///</remarks>
		static private void LoadSellThruPcts(int aNodeRID, NodeSellThruPctsInfo aNodeSellThruPctsInfo)
		{
			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.SellThruPcts_Read(aNodeRID);

				foreach(DataRow dr in dt.Rows)
				{
					SellThruPctInfo stpi = new SellThruPctInfo();

					stpi.SellThruPct = Convert.ToInt32(dr["SELL_THRU_PCT"], CultureInfo.CurrentUICulture);
					aNodeSellThruPctsInfo.SellThruPcts.Add(stpi);
				}
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static private void UpdateSellThruPctsHash(int aNodeRID, NodeSellThruPctsInfo aNodeSellThruPctsInfo)
		{
			try
			{
				sellThruPcts_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					if (!_sellThruPctsByRID.ContainsKey(aNodeRID))
					{
						_sellThruPctsByRID.Add(aNodeRID, aNodeSellThruPctsInfo);
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					sellThruPcts_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:UpdateSellThruPctsHash writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:UpdateSellThruPctsHash writer lock has timed out");
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Adds or updates information about a daily percentages
		/// </summary>
		/// <param name="sdpl">An instance of the StoreDailyPercentagesList class containing StoreDailyPercentagesProfile objects</param>
		/// <param name="nodeRID">The record id of the node</param>
		static public void StoreDailyPercentagesUpdate(int nodeRID, StoreDailyPercentagesList sdpl)
		{
            NodeDailyPercentagesInfo ndpi = null;
			try
			{
				dailyPercentages_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					bool dailyPercentagesFound = false;
					bool newEntry = false;
                    if (_dailyPercentagesByRID.ContainsKey(nodeRID))
                    {
                        ndpi = (NodeDailyPercentagesInfo)_dailyPercentagesByRID[nodeRID];
                    }
 
					if (ndpi == null)
					{
						ndpi = new NodeDailyPercentagesInfo();
						newEntry = true;
					}
					else
					{
						ndpi.StoreDailyPercentages.Clear();
					}
					
					foreach (StoreDailyPercentagesProfile sdpp in sdpl)
					{
                        // Begin TT#2610 - JSmith - Daily %s delete date range
                        if (sdpp.StoreDailyPercentagesIsInherited)
                        {
                            continue;
                        }
                        // End TT#2610 - JSmith - Daily %s delete date range

                        // Begin TT#2192 - JSmith - Daily Percentages reported as inherited with no values
                        dailyPercentagesFound = false;
                        // End TT#2192
						StoreDailyPercentagesInfo sdpi = new StoreDailyPercentagesInfo();
						sdpi.StoreRID = sdpp.Key;
                        // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                        //sdpi.DailyPercentagesList = new Hashtable();
                        sdpi.DailyPercentagesList = new SortedList( new DailyPercentagesComparer());
                        // End TT#2621 - JSmith - Duplicate weeks in daily sales
						if (sdpp.StoreDailyPercentagesDefaultChangeType != eChangeType.delete &&
							!sdpp.StoreDailyPercentagesIsInherited)
						{
							if (sdpp.HasDefaultValues &&
								(sdpp.Day1Default != 0 ||
								sdpp.Day2Default != 0 ||
								sdpp.Day3Default != 0 ||
								sdpp.Day4Default != 0 ||
								sdpp.Day5Default != 0 ||
								sdpp.Day6Default != 0 ||
								sdpp.Day7Default != 0))
							{
								dailyPercentagesFound = true;
							}
							sdpi.HasDefaultValues = sdpp.HasDefaultValues;
							sdpi.Day1Default = sdpp.Day1Default;
							sdpi.Day2Default = sdpp.Day2Default;
							sdpi.Day3Default = sdpp.Day3Default;
							sdpi.Day4Default = sdpp.Day4Default;
							sdpi.Day5Default = sdpp.Day5Default;
							sdpi.Day6Default = sdpp.Day6Default;
							sdpi.Day7Default = sdpp.Day7Default;
						}
						foreach (DailyPercentagesProfile dpp in sdpp.DailyPercentagesList)
						{
                            // Begin TT#305-MD - JSmith - Daily Percentage Tab is not holding inheritance when one store is changed and will not inherit from higher level
                            if (dpp.DailyPercentagesChangeType == eChangeType.delete)
                            {
                                // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                                //sdpi.DailyPercentagesList.Remove(dpp.DateRange);
                                // End TT#2621 - JSmith - Duplicate weeks in daily sales
                                continue;
                            }
                            // End TT#305-MD - JSmith - Daily Percentage Tab is not holding inheritance when one store is changed and will not inherit from higher level
							DailyPercentagesInfo dpi = new DailyPercentagesInfo();
							dpi.DateRange = dpp.DateRange;
							dpi.Day1 = dpp.Day1;
							dpi.Day2 = dpp.Day2;
							dpi.Day3 = dpp.Day3;
							dpi.Day4 = dpp.Day4;
							dpi.Day5 = dpp.Day5;
							dpi.Day6 = dpp.Day6;
							dpi.Day7 = dpp.Day7;
			
							if (dpi.Day1 != 0 ||
								dpi.Day2 != 0 ||
								dpi.Day3 != 0 ||
								dpi.Day4 != 0 ||
								dpi.Day5 != 0 ||
								dpi.Day6 != 0 ||
								dpi.Day7 != 0)
							{
								dailyPercentagesFound = true;
                                // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                                //sdpi.DailyPercentagesList.Add(dpi.DateRange.Key, dpi);
                                sdpi.DailyPercentagesList.Add(dpi.DateRange,dpi);
                                // End TT#2621 - JSmith - Duplicate weeks in daily sales
							}
						}
						if (dailyPercentagesFound)
						{
							ndpi.StoreDailyPercentages.Add(sdpi.StoreRID, sdpi);
						}
                        // Begin TT#305-MD - JSmith - Daily Percentage Tab is not holding inheritance when one store is changed and will not inherit from higher level
                        else if (sdpp.StoreDailyPercentagesDefaultChangeType == eChangeType.delete &&
                            sdpi.DailyPercentagesList.Count == 0)
                        {
                            ndpi.StoreDailyPercentages.Remove(sdpp.Key);
                        }
                        // End TT#305-MD - JSmith - Daily Percentage Tab is not holding inheritance when one store is changed and will not inherit from higher level
					}

					if (newEntry)
					{
						_dailyPercentagesByRID.Add(nodeRID, ndpi);
					}
					else
					{
						_dailyPercentagesByRID[nodeRID] = ndpi;
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					dailyPercentages_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:StoreDailyPercentagesUpdate writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:StoreDailyPercentagesUpdate writer lock has timed out");
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the daily percentages for the node.
		/// </summary>
		/// <param name="storeList">The ProfileList of store for which daily percentages are to be determined</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns></returns>
        static public StoreDailyPercentagesList GetStoreDailyPercentagesList(ProfileList storeList, int nodeRID, bool forCopy)
        {
            try
            {
                bool setInheritance = false;
                NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
                NodeAncestorList nal = GetNodeAncestorList(nodeRID, ni.HomeHierarchyRID);
                StoreDailyPercentagesList sdpl = new StoreDailyPercentagesList(eProfileType.StoreDailyPercentages);
                foreach (NodeAncestorProfile nap in nal)
                {
                    sdpl = GetStoreDailyPercentages(storeList, nap.Key, sdpl, forCopy, setInheritance);
                    if (sdpl.Count > storeList.Count ||	//  stop lookup if found daily percentages for all stores
                        forCopy)						//  or stop on first node if copy
                    {
                        break;
                    }
                    setInheritance = true;
                }
                return sdpl;
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

        //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
        /// <summary>
        /// Gets the daily percentages for the node.
        /// </summary>
        /// <param name="storeList">The ProfileList of store for which daily percentages are to be determined</param>
        /// <param name="nodeRID">The record id of the node</param>
        /// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
        /// <returns></returns>
        static public StoreDailyPercentagesList GetStoreDailyPercentagesList(StoreProfile storeProf, int nodeRID, bool forCopy)
        {
            try
            {
                bool setInheritance = false;
                NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
                NodeAncestorList nal = GetNodeAncestorList(nodeRID, ni.HomeHierarchyRID);
                StoreDailyPercentagesList sdpl = new StoreDailyPercentagesList(eProfileType.StoreDailyPercentages);
                foreach (NodeAncestorProfile nap in nal)
                {
                    sdpl = GetStoreDailyPercentages(storeProf, nap.Key, sdpl, forCopy, setInheritance);

                    setInheritance = true;
                }
                return sdpl;
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

        /// <summary>
        /// Gets the daily percentages for a node.
        /// </summary>
        /// <param name="storeList">The ProfileList of store for which daily percentages are to be determined</param>
        /// <param name="nodeRID">The record id of the node</param>
        /// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
        /// <param name="setInheritance">Identifies if inheritance information should be set</param>
        /// <returns></returns>
        static private StoreDailyPercentagesList GetStoreDailyPercentages(StoreProfile storeProf, int nodeRID,
            StoreDailyPercentagesList sdpl, bool forCopy, bool setInheritance)
        {
            NodeDailyPercentagesInfo ndpi = null;
            try
            {
                dailyPercentages_rwl.AcquireReaderLock(ReaderLockTimeOut);
                try
                {
                    if (_dailyPercentagesByRID.ContainsKey(nodeRID))
                    {
                        ndpi = (NodeDailyPercentagesInfo)_dailyPercentagesByRID[nodeRID];
                    }
                }
                finally
                {
                    // Ensure that the lock is released.
                    dailyPercentages_rwl.ReleaseReaderLock();
                }
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetStoreDailyPercentages reader lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:GetStoreDailyPercentages reader lock has timed out");
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }

            try
            {
                if (ndpi == null)
                {
                    ndpi = new NodeDailyPercentagesInfo();
                    LoadDailyPercentages(nodeRID, ndpi);
                    UpdateDailyPercentagesHash(nodeRID, ndpi);
                }

                StoreDailyPercentagesInfo sdpi;
                foreach (DictionaryEntry val in ndpi.StoreDailyPercentages)
                {
                    sdpi = (StoreDailyPercentagesInfo)val.Value;
                    if (storeProf.Key == sdpi.StoreRID)  // Do you need this store
                    {
                        if (!sdpl.Contains(sdpi.StoreRID))  // Do you already have this store
                        {
                            StoreDailyPercentagesProfile sdpp = new StoreDailyPercentagesProfile(sdpi.StoreRID);
                            if (forCopy)
                            {
                                sdpp.StoreDailyPercentagesDefaultChangeType = eChangeType.add;
                            }
                            else
                            {
                                sdpp.StoreDailyPercentagesDefaultChangeType = eChangeType.none;
                            }
                            sdpp.HasDefaultValues = sdpi.HasDefaultValues;
                            if (sdpp.HasDefaultValues)
                            {
                                sdpp.Day1Default = sdpi.Day1Default;
                                sdpp.Day2Default = sdpi.Day2Default;
                                sdpp.Day3Default = sdpi.Day3Default;
                                sdpp.Day4Default = sdpi.Day4Default;
                                sdpp.Day5Default = sdpi.Day5Default;
                                sdpp.Day6Default = sdpi.Day6Default;
                                sdpp.Day7Default = sdpi.Day7Default;
                            }
                            sdpp.DailyPercentagesList = new DailyPercentagesList(eProfileType.DailyPercentages);
                            if (sdpi.DailyPercentagesList.Count > 0)
                            {
                                DailyPercentagesInfo dpi;
                                foreach (DictionaryEntry dpval in sdpi.DailyPercentagesList)
                                {
                                    dpi = (DailyPercentagesInfo)dpval.Value;
                                    DailyPercentagesProfile dpp = new DailyPercentagesProfile(dpi.DateRange.Key);
                                    dpp.DailyPercentagesChangeType = eChangeType.none;
                                    dpp.DateRange = dpi.DateRange;
                                    dpp.Day1 = dpi.Day1;
                                    dpp.Day2 = dpi.Day2;
                                    dpp.Day3 = dpi.Day3;
                                    dpp.Day4 = dpi.Day4;
                                    dpp.Day5 = dpi.Day5;
                                    dpp.Day6 = dpi.Day6;
                                    dpp.Day7 = dpi.Day7;

                                    sdpp.DailyPercentagesList.Add(dpp);
                                }
                            }
                            if (setInheritance)
                            {
                                sdpp.StoreDailyPercentagesIsInherited = true;
                                sdpp.StoreDailyPercentagesInheritedFromNodeRID = nodeRID;
                            }

                            sdpl.Add(sdpp);
                        }
                    }
                }
                return sdpl;
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

        //END TT#43 - MD - DOConnell - Projected Sales Enhancement

		/// <summary>
		/// Gets the daily percentages for a node.
		/// </summary>
		/// <param name="storeList">The ProfileList of store for which daily percentages are to be determined</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <param name="setInheritance">Identifies if inheritance information should be set</param>
		/// <returns></returns>
		static private StoreDailyPercentagesList GetStoreDailyPercentages(ProfileList storeList, int nodeRID, 
			StoreDailyPercentagesList sdpl, bool forCopy, bool setInheritance)
		{
			NodeDailyPercentagesInfo ndpi = null;
			try
			{
				dailyPercentages_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
                    if (_dailyPercentagesByRID.ContainsKey(nodeRID))
                    {
                        ndpi = (NodeDailyPercentagesInfo)_dailyPercentagesByRID[nodeRID];
                    }
				}        
				finally
				{
					// Ensure that the lock is released.
					dailyPercentages_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:GetStoreDailyPercentages reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:GetStoreDailyPercentages reader lock has timed out");
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}

			try
			{
				if (ndpi == null)
				{
					ndpi = new NodeDailyPercentagesInfo();
					LoadDailyPercentages(nodeRID, ndpi);
					UpdateDailyPercentagesHash(nodeRID, ndpi);
				}

				StoreDailyPercentagesInfo sdpi;
				foreach (DictionaryEntry val in ndpi.StoreDailyPercentages)
				{
					sdpi = (StoreDailyPercentagesInfo)val.Value;
					if (storeList.Contains(sdpi.StoreRID))  // Do you need this store
					{
						if (!sdpl.Contains(sdpi.StoreRID))  // Do you already have this store
						{
							StoreDailyPercentagesProfile sdpp = new StoreDailyPercentagesProfile(sdpi.StoreRID);
							if (forCopy)
							{
								sdpp.StoreDailyPercentagesDefaultChangeType = eChangeType.add;
							}
							else
							{
								sdpp.StoreDailyPercentagesDefaultChangeType = eChangeType.none;
							}
							sdpp.HasDefaultValues = sdpi.HasDefaultValues;
							if (sdpp.HasDefaultValues)
							{
								sdpp.Day1Default = sdpi.Day1Default;
								sdpp.Day2Default = sdpi.Day2Default;
								sdpp.Day3Default = sdpi.Day3Default;
								sdpp.Day4Default = sdpi.Day4Default;
								sdpp.Day5Default = sdpi.Day5Default;
								sdpp.Day6Default = sdpi.Day6Default;
								sdpp.Day7Default = sdpi.Day7Default;
							}
							sdpp.DailyPercentagesList = new DailyPercentagesList(eProfileType.DailyPercentages);
							if (sdpi.DailyPercentagesList.Count > 0)
							{
                                DailyPercentagesInfo dpi;
                                foreach (DictionaryEntry dpval in sdpi.DailyPercentagesList)
                                {
                                    dpi = (DailyPercentagesInfo)dpval.Value;
                                    DailyPercentagesProfile dpp = new DailyPercentagesProfile(dpi.DateRange.Key);
                                    dpp.DailyPercentagesChangeType = eChangeType.none;
                                    dpp.DateRange = dpi.DateRange;
                                    dpp.Day1 = dpi.Day1;
                                    dpp.Day2 = dpi.Day2;
                                    dpp.Day3 = dpi.Day3;
                                    dpp.Day4 = dpi.Day4;
                                    dpp.Day5 = dpi.Day5;
                                    dpp.Day6 = dpi.Day6;
                                    dpp.Day7 = dpi.Day7;


                                    sdpp.DailyPercentagesList.Add(dpp);
                                }
							}
							if (setInheritance)
							{
								sdpp.StoreDailyPercentagesIsInherited = true;
								sdpp.StoreDailyPercentagesInheritedFromNodeRID = nodeRID;
							}

							sdpl.Add(sdpp);
						}
					}
				}
				return sdpl;
			}    
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Loads the daily percentages for the node.
		/// </summary>
		///<remarks>
		///Daily percentages are loaded as needed for performance.
		///</remarks>
		static private void LoadDailyPercentages(int aNodeRID, NodeDailyPercentagesInfo aNodeDailyPercentagesInfo)
		{
			try
			{
				StoreDailyPercentagesInfo spdi = null;
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();

				// load the default values for the stores
				DataTable default_dt = mhd.DailyPercentagesDefaults_Read(aNodeRID);

				foreach(DataRow dr in default_dt.Rows)
				{
					spdi = new StoreDailyPercentagesInfo();
                    // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                    //spdi.DailyPercentagesList = new Hashtable();
                    spdi.DailyPercentagesList = new SortedList(new DailyPercentagesComparer());
                    // End TT#2621 - JSmith - Duplicate weeks in daily sales
					spdi.HasDefaultValues = true;
				
					spdi.StoreRID = Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture);
					spdi.Day1Default = Convert.ToDouble(dr["DAY1"], CultureInfo.CurrentUICulture);
					spdi.Day2Default = Convert.ToDouble(dr["DAY2"], CultureInfo.CurrentUICulture);
					spdi.Day3Default = Convert.ToDouble(dr["DAY3"], CultureInfo.CurrentUICulture);
					spdi.Day4Default = Convert.ToDouble(dr["DAY4"], CultureInfo.CurrentUICulture);
					spdi.Day5Default = Convert.ToDouble(dr["DAY5"], CultureInfo.CurrentUICulture);
					spdi.Day6Default = Convert.ToDouble(dr["DAY6"], CultureInfo.CurrentUICulture);
					spdi.Day7Default = Convert.ToDouble(dr["DAY7"], CultureInfo.CurrentUICulture);
				
					aNodeDailyPercentagesInfo.StoreDailyPercentages.Add(spdi.StoreRID, spdi);
				}

				// load the daily percentages for time ranges for the stores
				DataTable dt = mhd.DailyPercentages_Read(aNodeRID);

				int currentStoreRID = Include.NoRID;
				bool storeRecordExists = false;
				foreach(DataRow dr in dt.Rows)
				{
					int storeRID = Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture);
					if (storeRID != currentStoreRID)
					{
						if (currentStoreRID != Include.NoRID)
						{
							if (storeRecordExists)
							{
								aNodeDailyPercentagesInfo.StoreDailyPercentages[currentStoreRID] = spdi;
							}
							else
							{
								aNodeDailyPercentagesInfo.StoreDailyPercentages.Add(spdi.StoreRID, spdi);
							}
						}
						currentStoreRID = storeRID;
						if (aNodeDailyPercentagesInfo.StoreDailyPercentages.Contains(storeRID))
						{
							storeRecordExists = true;
							spdi = (StoreDailyPercentagesInfo)aNodeDailyPercentagesInfo.StoreDailyPercentages[storeRID];
                            // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                            //spdi.DailyPercentagesList = new Hashtable();
                            spdi.DailyPercentagesList = new SortedList(new DailyPercentagesComparer());
                            // End TT#2621 - JSmith - Duplicate weeks in daily sales
						}
						else
						{
							storeRecordExists = false;
							spdi = new StoreDailyPercentagesInfo();
							spdi.HasDefaultValues = false;
				
							spdi.StoreRID = storeRID;
                            // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                            //spdi.DailyPercentagesList = new Hashtable();
                            spdi.DailyPercentagesList = new SortedList(new DailyPercentagesComparer());
                            // End TT#2621 - JSmith - Duplicate weeks in daily sales
						}
					}

					DailyPercentagesInfo dpi = new DailyPercentagesInfo();
					dpi.DateRange = new DateRangeProfile(Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture));
					dpi.DateRange.Key = Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture);
					dpi.DateRange.StartDateKey = Convert.ToInt32(dr["CDR_START"], CultureInfo.CurrentUICulture);
					dpi.DateRange.EndDateKey = Convert.ToInt32(dr["CDR_END"], CultureInfo.CurrentUICulture);
					dpi.DateRange.DateRangeType = (eCalendarRangeType)Convert.ToInt32(dr["CDR_RANGE_TYPE_ID"], CultureInfo.CurrentUICulture); //stodd
					dpi.DateRange.RelativeTo = (eDateRangeRelativeTo)Convert.ToInt32(dr["CDR_RELATIVE_TO"], CultureInfo.CurrentUICulture);
					dpi.DateRange.SelectedDateType = eCalendarDateType.Week;
					dpi.DateRange.DisplayDate = Calendar.GetDisplayDate(dpi.DateRange);
					dpi.Day1 = Convert.ToDouble(dr["DAY1"], CultureInfo.CurrentUICulture);
					dpi.Day2 = Convert.ToDouble(dr["DAY2"], CultureInfo.CurrentUICulture);
					dpi.Day3 = Convert.ToDouble(dr["DAY3"], CultureInfo.CurrentUICulture);
					dpi.Day4 = Convert.ToDouble(dr["DAY4"], CultureInfo.CurrentUICulture);
					dpi.Day5 = Convert.ToDouble(dr["DAY5"], CultureInfo.CurrentUICulture);
					dpi.Day6 = Convert.ToDouble(dr["DAY6"], CultureInfo.CurrentUICulture);
					dpi.Day7 = Convert.ToDouble(dr["DAY7"], CultureInfo.CurrentUICulture);

                    // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                    //spdi.DailyPercentagesList.Add(dpi.DateRange.Key, dpi);
                    spdi.DailyPercentagesList.Add(dpi.DateRange, dpi);
                    // End TT#2621 - JSmith - Duplicate weeks in daily sales
				}

				if (currentStoreRID != Include.NoRID)
				{
					if (storeRecordExists)
					{
						aNodeDailyPercentagesInfo.StoreDailyPercentages[currentStoreRID] = spdi;
					}
					else
					{
						aNodeDailyPercentagesInfo.StoreDailyPercentages.Add(spdi.StoreRID, spdi);
					}
				}
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		static private void UpdateDailyPercentagesHash(int aNodeRID, NodeDailyPercentagesInfo aNodeDailyPercentagesInfo)
		{
			try
			{
				dailyPercentages_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					if (!_dailyPercentagesByRID.ContainsKey(aNodeRID))
					{
						_dailyPercentagesByRID.Add(aNodeRID, aNodeDailyPercentagesInfo);
					}
				}        
				finally
				{
					// Ensure that the lock is released.
					dailyPercentages_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:UpdateStoreGradeHash writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:UpdateStoreGradeHash writer lock has timed out");
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        // Begin TT#634 - JSmith - Color rename
        /// <summary>
        /// Requests the session get all styles that contain a color code.
        /// </summary>
        /// <param name="colorCodeRID">The record id of the color code</param>
        /// <returns>An instance of the HierarchyNodeList class which contains a HierarchyNodeProfile class for each style</returns>
        static public HierarchyNodeList GetStylesForColor(int colorCodeRID)
        {
            MerchandiseHierarchyData mhd;
            DataTable dt;
            try
            {
                HierarchyNodeList hnl = new HierarchyNodeList(eProfileType.HierarchyNode);
                mhd = new MerchandiseHierarchyData();
                dt = mhd.GetStylesForColor(colorCodeRID);
                foreach (DataRow dr in dt.Rows)
                {
                    // Begin TT#634 - JSmith - Color rename
                    //hnl.Add(GetNodeData(Convert.ToInt32(dr["SYTLE_HN_RID"]), false));
                    int styleRID = Convert.ToInt32(dr["SYTLE_HN_RID"]);
                    if (!hnl.Contains(styleRID))
                    {
                        hnl.Add(GetNodeData(styleRID, false));
                    }
                    // End TT#634
                }

                return hnl;
            }
            catch
            {
                throw;
            }
        }
        // End TT#634

		/// <summary>
		/// Returns an instance of a ColorCodeProfile object associated with the color code.
		/// </summary>
		/// <param name="colorCodeRID">The record id of the color.</param>
		/// <returns>An instance of the ColorCodeProfile containing information about the color.</returns>
		/// <remarks>
		/// Return Include.NoRID in the record id if the color is not found.
		/// </remarks>
		static public ColorCodeProfile GetColorCodeProfile(int colorCodeRID)
		{
			try
			{
				ColorCodeProfile ccp = new ColorCodeProfile(Include.NoRID);
				if (_colorCodesByRID.ContainsKey(colorCodeRID))
				{
					ColorCodeInfo cci = GetColorCodeInfoByRID(colorCodeRID);
					ccp.ColorCodeChangeType = eChangeType.none;
					ccp.Key = cci.ColorCodeRID;
					ccp.ColorCodeID = cci.ColorCodeID;
					ccp.ColorCodeName = cci.ColorCodeName;
					ccp.ColorCodeGroup = cci.ColorCodeGroup;
					ccp.Text = Include.GetColorDisplay(cci.ColorCodeID, cci.ColorCodeName);
                    ccp.VirtualInd = cci.VirtualInd;        // Assortment
				}
				return ccp;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Returns an instance of a ColorCodeProfile object associated with the color ID.
		/// </summary>
		/// <param name="colorID">The ID of the color.</param>
		/// <returns>An instance of the ColorCodeProfile containing information about the color.</returns>
		/// <remarks>
		/// Return Include.NoRID in the record id if the color is not found.
		/// </remarks>
		static public ColorCodeProfile GetColorCodeProfile(string colorID)
		{
			try
			{
				ColorCodeProfile ccp;
				object hashEntry = null;
				lock (_colorCode_lock.SyncRoot)
				{
                    if (_colorCodesByID.ContainsKey(colorID))
                    {
                        hashEntry = _colorCodesByID[colorID];
                    }
				}
				if (hashEntry != null)
				{
					int colorCodeRID = Convert.ToInt32(hashEntry);
					ccp = GetColorCodeProfile(colorCodeRID);
				}
				else
				{
					ccp = new ColorCodeProfile(Include.NoRID);
				}
				return ccp;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Adds or updates information about a color code
		/// </summary>
		/// <param name="ccp">An instance of the ColorCodeProfile class containing information for a color code</param>
		static public void ColorCodeUpdate(ColorCodeProfile ccp)
		{
			try
			{
				switch (ccp.ColorCodeChangeType)
				{
					case eChangeType.none: 
					{
						break;
					}
					case eChangeType.add: 
					{
						ColorCodeInfo cci = new ColorCodeInfo();
						cci.ColorCodeRID = ccp.Key;
						cci.ColorCodeID = ccp.ColorCodeID;
						cci.ColorCodeName = ccp.ColorCodeName;
						cci.ColorCodeGroup = ccp.ColorCodeGroup;
                        cci.VirtualInd = ccp.VirtualInd;        // Assortment
                        cci.Purpose = ccp.Purpose;        // Assortment
						lock (_colorCode_lock.SyncRoot)
						{
							_colorCodesByRID.Add(cci.ColorCodeRID, cci);
							_colorCodesByID.Add(cci.ColorCodeID, cci.ColorCodeRID);
                            if (ccp.Purpose == ePurpose.Placeholder)
                            {
                                _colorCodePlaceholders.Add(cci.ColorCodeID, cci);
                            }
						}
					
						break;
					}
					case eChangeType.update: 
					{
						ColorCodeInfo cci = GetColorCodeInfoByRID(ccp.Key);
						cci.ColorCodeID = ccp.ColorCodeID;
						cci.ColorCodeName = ccp.ColorCodeName;
						cci.ColorCodeGroup = ccp.ColorCodeGroup;
                        cci.VirtualInd = ccp.VirtualInd;        // Assortment
                        cci.Purpose = ccp.Purpose;        // Assortment
						lock (_colorCode_lock.SyncRoot)
						{
							_colorCodesByID.Remove(cci.ColorCodeID);
							_colorCodesByRID[ccp.Key] = cci;
							_colorCodesByID.Add(cci.ColorCodeID, cci.ColorCodeRID);
						}
						break;
					}
					case eChangeType.delete: 
					{
						ColorCodeInfo cci = GetColorCodeInfoByRID(ccp.Key);
						lock (_colorCode_lock.SyncRoot)
						{
							_colorCodesByRID.Remove(ccp.Key);
							_colorCodesByID.Remove(cci.ColorCodeID);
                            _colorCodePlaceholders.Remove(ccp.ColorCodeID);
						}
                        break;
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the list of color codes in the system.
		/// </summary>
		/// <returns>An instance of the ColorCodeList class containing ColorCodeProfile objects</returns>
		static public ColorCodeList GetColorCodeList()
		{
			try
			{
				// clone hashtable to reduce lock time
                Dictionary<int, ColorCodeInfo> colorCodesByRID = null;
				lock (_colorCode_lock.SyncRoot)
				{
                    //colorCodesByRID = HierarchyServerGlobal._colorCodesByRID.Clone();
                    colorCodesByRID = CloneDictionary<int, ColorCodeInfo>(_colorCodesByRID);
				}

				ColorCodeInfo cci;
				ColorCodeList ccl = new ColorCodeList(eProfileType.ColorCode);
				foreach (KeyValuePair<int, ColorCodeInfo>  val in colorCodesByRID)
				{
					cci = val.Value;
					ColorCodeProfile ccp = new ColorCodeProfile(cci.ColorCodeRID);
					ccp.ColorCodeChangeType = eChangeType.none;
					ccp.ColorCodeID = cci.ColorCodeID;
					ccp.ColorCodeName = cci.ColorCodeName;
					ccp.ColorCodeGroup = cci.ColorCodeGroup;
					ccp.Text = Include.GetColorDisplay(cci.ColorCodeID, cci.ColorCodeName);
                    ccp.VirtualInd = cci.VirtualInd;        // Assortment
										
					ccl.Add(ccp);
				}
				return ccl;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Returns the Hashtable of the colors in the system keyed by ID.
		/// </summary>
		static public Dictionary<string, int> GetColorCodeListByID()
		{
			try
			{
                Dictionary<string, int> colorCodesByID;
				lock (_colorCode_lock.SyncRoot)
				{
                    //colorCodesByID = (Hashtable)HierarchyServerGlobal._colorCodesByID.Clone();
                    colorCodesByID = CloneDictionary<string, int>(_colorCodesByID);
				}
				return colorCodesByID;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        /// <summary>
        /// Retrieve a list of place holder color profiles
        /// </summary>
        /// <param name="aNumberOfPlaceholderColors">
        /// The number of placeholder colors to retrieve
        /// </param>
        /// <param name="aCurrentPlaceholderColors">
        /// The list of current placeholder color keys.  This is used to filter the list
        /// </param>
        /// <returns>ColorCodeList of placeholder color code profiles</returns>
        static public ColorCodeList GetPlaceholderColors(int aNumberOfPlaceholderColors, ArrayList aCurrentPlaceholderColors)
        {
            try
            {
                List<int> currentPlaceHolders = new List<int>();
                ColorCodeList ccl = new ColorCodeList(eProfileType.ColorCode);
                bool needPlaceholders = true;

                // copy current placeholder keys to Hashtable for easy lookup
                foreach (int colorRID in aCurrentPlaceholderColors)
                {
                    currentPlaceHolders.Add(colorRID);
                }

                // get placeholders out of cache
                lock (_colorCode_lock.SyncRoot)
                {
                    foreach (ColorCodeInfo cci in _colorCodePlaceholders.Values)
                    {
                        if (!currentPlaceHolders.Contains(cci.ColorCodeRID))
                        {
                            ColorCodeProfile ccp = new ColorCodeProfile(cci.ColorCodeRID);
                            ccp.ColorCodeChangeType = eChangeType.none;
                            ccp.ColorCodeID = cci.ColorCodeID;
                            ccp.ColorCodeName = cci.ColorCodeName;
                            ccp.ColorCodeGroup = cci.ColorCodeGroup;
                            ccp.Text = Include.GetColorDisplay(cci.ColorCodeID, cci.ColorCodeName);
                            ccp.VirtualInd = cci.VirtualInd;        // Assortment
                            ccp.Purpose = cci.Purpose;

                            ccl.Add(ccp);
                        }

                        if (ccl.Count == aNumberOfPlaceholderColors)
                        {
                            needPlaceholders = false;
                            break;
                        }
                    }

                    // build more placeholders if necessary
                    if (ccl.Count < aNumberOfPlaceholderColors)
                    {
                        int numberPlaceholdersToBuild = aNumberOfPlaceholderColors - ccl.Count > 50 ? aNumberOfPlaceholderColors - ccl.Count : 50;
                        while (needPlaceholders)
                        {
                            ColorCodeList placeHolders = BuildPlaceholderColors(numberPlaceholdersToBuild);
                            foreach (ColorCodeProfile ccp in placeHolders)
                            {
                                ccl.Add(ccp);
                                if (ccl.Count == aNumberOfPlaceholderColors)
                                {
                                    needPlaceholders = false;
                                    break;
                                }
                            }
                        }
                    }
                }

                return ccl;
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

        /// <summary>
        /// Build placeholder colors
        /// </summary>
        /// <param name="aNumberOfPlaceholderColors">
        /// The number of placeholder colors to build
        /// </param>
        static private ColorCodeList BuildPlaceholderColors(int aNumberOfPlaceholderColors)
        {
            ColorData cd = new ColorData();
            ColorCodeList ccl = new ColorCodeList(eProfileType.ColorCode);
            try
            {
                ColorCodeProfile ccp;
                cd.OpenUpdateConnection(eLockType.ColorCode, "AddPlaceholders");
                for (int i = 0; i < aNumberOfPlaceholderColors; i++)
                {
                    ccp = new ColorCodeProfile(-1);
                    ccp.ColorCodeChangeType = eChangeType.add;
                    ccp.ColorCodeID = _placeholderColorLabel + (_colorCodePlaceholders.Count + 1).ToString("d3");
                    ccp.ColorCodeName = ccp.ColorCodeID;
                    ccp.ColorCodeGroup = _placeholderColorLabel;
                    ccp.VirtualInd = true;
                    ccp.Purpose = ePurpose.Placeholder;
                    ccp.Key = cd.Color_Add(ccp.ColorCodeID, ccp.ColorCodeName, ccp.ColorCodeGroup, ccp.VirtualInd, ccp.Purpose);

                    ccl.Add(ccp);
                    ColorCodeUpdate(ccp);
                }

                cd.CommitData();
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
            finally
            {
                cd.CloseUpdateConnection();
            }

            return ccl;
        }

		/// <summary>
		/// Returns an instance of a SizeCodeProfile object associated with the size code.
		/// </summary>
		/// <param name="sizeCodeRID">The record id of the size.</param>
		/// <returns></returns>
		/// <remarks>
		/// Return Include.NoRID in the record id if the size is not found.
		/// </remarks>
		static public SizeCodeProfile GetSizeCodeProfile(int sizeCodeRID)
		{
			try
			{
				SizeCodeInfo sci = GetSizeCodeInfoByRID(sizeCodeRID);

				SizeCodeProfile scp = new SizeCodeProfile(Include.NoRID);
				if (sci != null)
				{
					scp.SizeCodeChangeType = eChangeType.none;
					scp.Key = sci.SizeCodeRID;
					scp.SizeCodeID = sci.SizeCodeID;
					if (sci.SizeCodeName != null &&
						sci.SizeCodeName.Trim().Length > 0)
					{
						scp.SizeCodeName = sci.SizeCodeName;
					}
					else if (sci.SizeCodePrimary != null &&
						sci.SizeCodePrimary.Trim().Length > 0)
					{
						scp.SizeCodeName = sci.SizeCodePrimary;
					}
					else if (sci.SizeCodeSecondary != null &&
						sci.SizeCodeSecondary.Trim().Length > 0)
					{
						scp.SizeCodeName = sci.SizeCodeSecondary;
					}
					else 
					{
						scp.SizeCodeName = sci.SizeCodeID;
					}
					scp.SizeCodePrimary = sci.SizeCodePrimary;
					if (sci.SizeCodeSecondary != null)
					{
						scp.SizeCodeSecondary = sci.SizeCodeSecondary;
					}
					else
					{
						scp.SizeCodeSecondary = string.Empty;
					}
					scp.SizeCodeProductCategory = sci.SizeCodeProductCategory;
					scp.SizeCodePrimaryRID = sci.SizeCodePrimaryRID;
					scp.SizeCodeSecondaryRID = sci.SizeCodeSecondaryRID;
				}
				return scp;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Returns an instance of a SizeCodeProfile object associated with the size ID.
		/// </summary>
		/// <param name="sizeID">The ID of the size.</param>
		/// <returns></returns>
		/// <remarks>
		/// Return Include.NoRID in the record id if the size is not found.
		/// </remarks>
		static public SizeCodeProfile GetSizeCodeProfile(string sizeID)
		{
			try
			{
				object hashValue = null;
				lock (_sizeCode_lock.SyncRoot)
				{
                    if (_sizeCodesByID.ContainsKey(sizeID))
                    {
                        hashValue = _sizeCodesByID[sizeID];
                    }
				}
				if (hashValue != null)
				{
					return GetSizeCodeProfile(Convert.ToInt32(hashValue));
				}
				else
				{
					SizeCodeProfile scp = new SizeCodeProfile(Include.NoRID);
					return scp;
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Returns the RID of the size code with the given primary and secondary size codes.  If it does not exist, Include.NoRID is returned.
		/// </summary>
		/// <param name="aSizeCodePrimary">The primary size code.</param>
		/// <param name="aSizeCodeSecondary">The secondary size code.</param>
		/// <returns>The RID of the size code or Include.NoRID if it doesn't exist.</returns>
		static public int GetSizeCodeRID(string aProductCategory, string aSizeCodePrimary, string aSizeCodeSecondary)
		{
			object hashValue = null;

			try
			{
				lock (_sizeCode_lock.SyncRoot)
				{
					string hashKey = Include.GetSizeKey(aProductCategory, aSizeCodePrimary, aSizeCodeSecondary);
                    if (_sizeCodesByPriSec.ContainsKey(hashKey))
                    {
                        hashValue = _sizeCodesByPriSec[hashKey];
                    }
				}
				if (hashValue != null)
				{
					return (int)hashValue;
				}
				else
				{
					return Include.NoRID;
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Adds or updates information about a size code
		/// </summary>
		/// <param name="scp">An instance of the SizeCodeProfile class containing information for a size code</param>
		static public void SizeCodeUpdate(SizeCodeProfile scp)
		{
			try
			{
                // Begin TT#2735 - JSmith - Saving Size Group in Never-ending Loop
                string noSizeDimensionLbl = MIDText.GetTextOnly((int)eMIDTextCode.lbl_NoSecondarySize);
                // End TT#2735 - JSmith - Saving Size Group in Never-ending Loop

				switch (scp.SizeCodeChangeType)
				{
					case eChangeType.none: 
					{
						break;
					}
					case eChangeType.add: 
					{
						SizeGroup sg = new SizeGroup();
						SizeCodeInfo sci = new SizeCodeInfo();
						sci.SizeCodeRID = scp.Key;
						sci.SizeCodeID = scp.SizeCodeID;
                        // Begin TT#626-MD - JSmith - Size Name and Description Missing after Auto-Add
                        //sci.SizeCodeName = scp.SizeCodeName;
                        sci.SizeCodeName = Include.GetSizeName(scp.SizeCodePrimary, scp.SizeCodeSecondary, scp.SizeCodeID);
                        // End TT#626-MD - JSmith - Size Name and Description Missing after Auto-Add
						sci.SizeCodePrimary = scp.SizeCodePrimary;
						// Begin TT#61 - JSmith -  Size Group not recognizing primary sizes when secondary does not exist
//						sci.SizeCodeSecondary = scp.SizeCodeSecondary;
						// Begin Track #6417 - JSmith - object reference error
						//						if (scp.SizeCodeSecondary.Trim().Length == 0)
						if (scp.SizeCodeSecondary == null ||
							scp.SizeCodeSecondary.Trim().Length == 0)
						// End Track #6417
						{
                            // Begin TT#2735 - JSmith - Saving Size Group in Never-ending Loop
                            //sci.SizeCodeSecondary = null;
                            sci.SizeCodeSecondary = noSizeDimensionLbl;
                            // End TT#2735 - JSmith - Saving Size Group in Never-ending Loop
						}
						else
						{
							sci.SizeCodeSecondary = scp.SizeCodeSecondary;
						}
						// End TT#61
						sci.SizeCodeProductCategory = scp.SizeCodeProductCategory;
						sci.SizeCodePrimaryRID = sg.GetPrimarySizeRID(scp.SizeCodePrimary);
						sci.SizeCodeSecondaryRID = sg.GetSecondarySizeRID(scp.SizeCodeSecondary);
						
						lock (_sizeCode_lock.SyncRoot)
						{
							_sizeCodesByRID.Add(sci.SizeCodeRID, sci);
							_sizeCodesByID.Add(sci.SizeCodeID, sci.SizeCodeRID);
							string hashKey = Include.GetSizeKey(sci.SizeCodeProductCategory, sci.SizeCodePrimary, sci.SizeCodeSecondary);
							_sizeCodesByPriSec.Add(hashKey, sci.SizeCodeRID);
						}

						break;
					}
					case eChangeType.update: 
					{
						SizeGroup sg = new SizeGroup();
						SizeCodeInfo sci = GetSizeCodeInfoByRID(scp.Key);
						string oldHashKey = Include.GetSizeKey(sci.SizeCodeProductCategory, sci.SizeCodePrimary, sci.SizeCodeSecondary);
						sci.SizeCodeID = scp.SizeCodeID;
                        // Begin TT#626-MD - JSmith - Size Name and Description Missing after Auto-Add
                        //sci.SizeCodeName = scp.SizeCodeName;
                        sci.SizeCodeName = Include.GetSizeName(scp.SizeCodePrimary, scp.SizeCodeSecondary, scp.SizeCodeID);
                        // End TT#626-MD - JSmith - Size Name and Description Missing after Auto-Add
						sci.SizeCodePrimary = scp.SizeCodePrimary;
						// Begin TT#61 - JSmith -  Size Group not recognizing primary sizes when secondary does not exist
//						sci.SizeCodeSecondary = scp.SizeCodeSecondary;
						// Begin Track #6417 - JSmith - object reference error
//						if (scp.SizeCodeSecondary.Trim().Length == 0)
						if (scp.SizeCodeSecondary == null ||
							scp.SizeCodeSecondary.Trim().Length == 0)
						// End Track #6417
						{
                            // Begin TT#2735 - JSmith - Saving Size Group in Never-ending Loop
                            //sci.SizeCodeSecondary = null;
                            sci.SizeCodeSecondary = noSizeDimensionLbl;
                            // End TT#2735 - JSmith - Saving Size Group in Never-ending Loop
						}
						else
						{
							sci.SizeCodeSecondary = scp.SizeCodeSecondary;
						}
						// End TT#61
						sci.SizeCodeProductCategory = scp.SizeCodeProductCategory;
						sci.SizeCodePrimaryRID = sg.GetPrimarySizeRID(scp.SizeCodePrimary);
						sci.SizeCodeSecondaryRID = sg.GetSecondarySizeRID(scp.SizeCodeSecondary);
						string newHashKey = Include.GetSizeKey(scp.SizeCodeProductCategory, scp.SizeCodePrimary, scp.SizeCodeSecondary);
						lock (_sizeCode_lock.SyncRoot)
						{
							// remove old
							_sizeCodesByID.Remove(sci.SizeCodeID);
							_sizeCodesByPriSec.Remove(oldHashKey);
							// add new
							_sizeCodesByRID[scp.Key] = sci;
							_sizeCodesByID.Add(sci.SizeCodeID, sci.SizeCodeRID);
							_sizeCodesByPriSec.Add(newHashKey, sci.SizeCodeRID);
						}
						break;
					}
					case eChangeType.delete: 
					{
						SizeCodeInfo sci = GetSizeCodeInfoByRID(scp.Key);
						lock (_sizeCode_lock.SyncRoot)
						{
							_sizeCodesByRID.Remove(scp.Key);
							_sizeCodesByID.Remove(sci.SizeCodeID);
							string hashKey = Include.GetSizeKey(sci.SizeCodeProductCategory, sci.SizeCodePrimary, sci.SizeCodeSecondary);
							_sizeCodesByPriSec.Remove(hashKey);
						}
						break;
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the list of the product categories for the size codes in the system.
		/// </summary>
		/// <returns>An ArrayList containing a unique list of product categories for the sizes</returns>
		static public ArrayList GetSizeProductCategoryList()
		{
			try
			{
				// clone hashtable to reduce lock time
                Dictionary<int, SizeCodeInfo> sizeCodesByRID = null;
				lock (_sizeCode_lock.SyncRoot)
				{
                    //sizeCodesByRID = (Hashtable)HierarchyServerGlobal._sizeCodesByRID.Clone();
                    sizeCodesByRID = CloneDictionary<int, SizeCodeInfo>(_sizeCodesByRID);
				}        

				SizeCodeInfo sci;
				ArrayList productCategoryList = new ArrayList();
                foreach (KeyValuePair<int, SizeCodeInfo> val in sizeCodesByRID)
				{
					sci = (SizeCodeInfo)val.Value;
					if (!productCategoryList.Contains(sci.SizeCodeProductCategory))
					{
						productCategoryList.Add(sci.SizeCodeProductCategory);
					}
				}
				return productCategoryList;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the complete list of size codes in the system.
		/// </summary>
		/// <returns>An instance of the SizeCodeList containing SizeCodeProfile objects</returns>
		static public SizeCodeList GetSizeCodeList()
		{
			try
			{
				return GetSizeCodeList(null, eSearchContent.WholeField, null, eSearchContent.WholeField, null, eSearchContent.WholeField);
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Returns an instance of a SizeCodeList object with the requested productCategory, primary, secondary.
		/// </summary>
		/// <param name="productCategory">The product category of the size.</param>
		/// <param name="primary">The primary description of the size.</param>
		/// <param name="secondary">The secondary description of the size.</param>
		/// <returns>An instance of the SizeCodeList containing SizeCodeProfile objects that match the query</returns>
		/// <remarks>
		/// Returns empty SizeCodeList if no size codes match the query.
		/// </remarks>
		static public SizeCodeList GetSizeCodeList(string productCategory, string primary, string secondary)
		{
			try
			{
				return GetSizeCodeList(productCategory, eSearchContent.WholeField, primary, eSearchContent.AnyPartOfField, secondary, eSearchContent.AnyPartOfField);
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Returns an instance of a SizeCodeList object with the requested productCategory, primary, secondary.
		/// </summary>
		/// <param name="productCategory">The product category of the size.</param>
		/// <param name="primary">The primary description of the size.</param>
		/// <param name="secondary">The secondary description of the size.</param>
		/// <returns>An instance of the SizeCodeList containing SizeCodeProfile objects that match the query</returns>
		/// <remarks>
		/// Returns empty SizeCodeList if no size codes match the query.
		/// </remarks>
		static public SizeCodeList GetExactSizeCodeList(string productCategory, string primary, string secondary)
		{
			try
			{
				return GetSizeCodeList(productCategory, eSearchContent.WholeField, primary, eSearchContent.WholeField, secondary, eSearchContent.WholeField);
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets the list of size codes in the system for the provided product category.
		/// </summary>
		/// <param name="productCategory">An instance of the SizeCodeList containing SizeCodeProfile objects that match the product category</param>
		/// <returns></returns>
		static public SizeCodeList GetSizeCodeList(string productCategory)
		{
			try
			{
				return GetSizeCodeList(productCategory, eSearchContent.WholeField, null, eSearchContent.WholeField, null, eSearchContent.WholeField);
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Returns an instance of a SizeCodeList object with the requested productCategory, primary, secondary.
		/// </summary>
		/// <param name="productCategory">The product category of the size.</param>
		/// <param name="productCategorySearchContent">The type of search for product category.</param>
		/// <param name="primary">The primary description of the size.</param>
		/// <param name="primarySearchContent">The type of search for primary.</param>
		/// <param name="secondary">The secondary description of the size.</param>
		/// <param name="secondarySearchContent">The type of search secondary.</param>
		/// <returns>An instance of the SizeCodeList containing SizeCodeProfile objects that match the query</returns>
		/// <remarks>
		/// Returns empty SizeCodeList if no size codes match the query.
		/// </remarks>
		static public SizeCodeList GetSizeCodeList(string productCategory, eSearchContent productCategorySearchContent,
			string primary, eSearchContent primarySearchContent, 
			string secondary, eSearchContent secondarySearchContent)
		{
			try
			{
				string upperProductCategory = null;
				if (productCategory != null)
				{
					upperProductCategory = productCategory.Trim().ToUpper(CultureInfo.CurrentUICulture);
				}
				string upperPrimary = null;
				if (primary != null)
				{
					upperPrimary = primary.Trim().ToUpper(CultureInfo.CurrentUICulture);
				}
				string upperSecondary = null;
				if (secondary != null)
				{
					if (secondary.Trim().Length > 0)
					{
						upperSecondary = secondary.Trim().ToUpper(CultureInfo.CurrentUICulture);
					}
				}

				// clone hashtable to reduce lock time
                Dictionary<int, SizeCodeInfo> sizeCodesByRID = null;
				try
				{
					lock (_sizeCode_lock.SyncRoot)
					{
                        //sizeCodesByRID = (Hashtable)HierarchyServerGlobal._sizeCodesByRID.Clone();
                        sizeCodesByRID = CloneDictionary<int, SizeCodeInfo>(_sizeCodesByRID);
					}
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}

				bool foundMatch = false;
				SizeCodeInfo sci;
				SizeCodeList scl = new SizeCodeList(eProfileType.SizeCode);
				foreach (KeyValuePair<int, SizeCodeInfo>  val in sizeCodesByRID)
				{
					foundMatch = true;
					sci = val.Value;
					if (productCategory != null)
					{
						switch (productCategorySearchContent)
						{
							case eSearchContent.WholeField:
							{
								if (sci.SizeCodeProductCategoryUPPER!= upperProductCategory)
								{
									foundMatch = false;
								}
								break;
							}
							case eSearchContent.StartOfField:
							{
								if ((sci.SizeCodeProductCategoryUPPER).IndexOf(upperProductCategory) != 0)
								{
									foundMatch = false;
								}
								break;
							}
							case eSearchContent.AnyPartOfField:
							{
								if ((sci.SizeCodeProductCategoryUPPER).IndexOf(upperProductCategory) == -1)
								{
									foundMatch = false;
								}
								break;
							}
							case eSearchContent.EndOfField:
							{
								if ((((sci.SizeCodeProductCategoryUPPER).IndexOf(upperProductCategory)) + productCategory.Length) != sci.SizeCodeProductCategory.Length)
								{
									foundMatch = false;
								}
								break;
							}
						}
					}

					if (foundMatch && primary != null)
					{
						switch (primarySearchContent)
						{
							case eSearchContent.WholeField:
							{
								if (sci.SizeCodePrimaryUPPER != upperPrimary)
								{
									foundMatch = false;
								}
								break;
							}
							case eSearchContent.StartOfField:
							{
								if ((sci.SizeCodePrimaryUPPER).IndexOf(upperPrimary) != 0)
								{
									foundMatch = false;
								}
								break;
							}
							case eSearchContent.AnyPartOfField:
							{
								if ((sci.SizeCodePrimaryUPPER).IndexOf(upperPrimary) == -1)
								{
									foundMatch = false;
								}
								break;
							}
							case eSearchContent.EndOfField:
							{
								if ((((sci.SizeCodePrimaryUPPER).IndexOf(upperPrimary)) + primary.Length) != sci.SizeCodePrimary.Length)
								{
									foundMatch = false;
								}
								break;
							}
						}
					}

					if (foundMatch)
					{
						if (upperSecondary != null && sci.SizeCodeSecondaryUPPER != null)
						{
							switch (secondarySearchContent)
							{
								case eSearchContent.WholeField:
								{
									if (sci.SizeCodeSecondaryUPPER != upperSecondary)
									{
										foundMatch = false;
									}
									break;
								}
								case eSearchContent.StartOfField:
								{
									if ((sci.SizeCodeSecondaryUPPER).IndexOf(upperSecondary) != 0)
									{
										foundMatch = false;
									}
									break;
								}
								case eSearchContent.AnyPartOfField:
								{
									if ((sci.SizeCodeSecondaryUPPER).IndexOf(upperSecondary) == Include.Undefined)
									{
										foundMatch = false;
									}
									break;
								}
								case eSearchContent.EndOfField:
								{
									if ((((sci.SizeCodeSecondaryUPPER).IndexOf(upperSecondary)) + secondary.Length) != sci.SizeCodeSecondary.Length)
									{
										foundMatch = false;
									}
									break;
								}
							}
						}
						else if (upperSecondary == null && sci.SizeCodeSecondaryUPPER != null)
						{
							if (sci.SizeCodeSecondaryUPPER.Trim().Length > 0)
							{
								foundMatch = false;
							}
						}
						else if (upperSecondary != null && sci.SizeCodeSecondaryUPPER == null)
						{
							if (upperSecondary.Trim().Length > 0)
							{
								foundMatch = false;
							}
						}
					}
				
					if (foundMatch)
					{
						SizeCodeProfile scp = new SizeCodeProfile(sci.SizeCodeRID);
						scp.SizeCodeChangeType = eChangeType.none;
						scp.SizeCodeID = sci.SizeCodeID;
						scp.SizeCodeName = sci.SizeCodeName;
						scp.SizeCodePrimary = sci.SizeCodePrimary;
						scp.SizeCodeSecondary = sci.SizeCodeSecondary;
						scp.SizeCodeProductCategory = sci.SizeCodeProductCategory;
									
						scl.Add(scp);
					}
				}
				return scl;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}
		
		/// <summary>
		/// Returns the Hashtable of the sizes in the system keyed by ID.
		/// </summary>
        static public Dictionary<string, int> GetSizeCodeListByID()
		{
			try
			{
				lock (_sizeCode_lock.SyncRoot)
				{
                    //return (Hashtable)HierarchyServerGlobal._sizeCodesByID.Clone();
                    return CloneDictionary<string, int>(_sizeCodesByID);
					
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Returns the Hashtable of the sizes in the system keyed by category, primary and secondary code.
		/// </summary>
        static public Dictionary<string, int> GetSizeCodeListByPriSec()
		{
			try
			{
				lock (_sizeCode_lock.SyncRoot)
				{
                    //return (Hashtable)HierarchyServerGlobal._sizeCodesByPriSec.Clone();
                    return CloneDictionary<string, int>(_sizeCodesByPriSec);
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Checks to determine if a color is already assigned to a style
		/// </summary>
		/// <param name="hierarchyRID">The record ID of the hierarchy</param>
		/// <param name="nodeRID">The record id of the style</param>
		/// <param name="colorCode">The code associated with the color</param>
		/// <param name="aQualifiedNodeID">The fully qualified node ID including parent</param>
		/// <param name="colorNodeRID">a reference to an integer that is used to return the record id of the node if 
		/// the color is already defined to the style</param>
		/// <returns>A flag identifying if the color is defined to the style</returns>
		static public bool ColorExistsForStyle(int hierarchyRID, int nodeRID, string colorCode, string aQualifiedNodeID, ref int colorNodeRID)
		{
			try
			{
				if (colorCode != null)
				{
					colorCode = colorCode.Trim();
				}
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				colorNodeRID = mhd.Hierarchy_ColorNodeRID_For_Style(hierarchyRID, nodeRID, colorCode);
				if (colorNodeRID == Include.NoRID)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        // Begin TT#2763 - JSmith - Hierarchy Color descriptions not updating
        /// <summary>
        /// Checks to determine if a color is already assigned to a style
        /// </summary>
        /// <param name="hierarchyRID">The record ID of the hierarchy</param>
        /// <param name="nodeRID">The record id of the style</param>
        /// <param name="colorCode">The code associated with the color</param>
        /// <param name="aQualifiedNodeID">The fully qualified node ID including parent</param>
        /// <param name="colorNodeRID">a reference to an integer that is used to return the record id of the node if 
		/// <param name="aColorDescription">Output parameter containing the color description</param>
        /// the color is already defined to the style</param>
        /// <returns>A flag identifying if the color is defined to the style</returns>
        static public bool ColorExistsForStyle(int hierarchyRID, int nodeRID, string colorCode, string aQualifiedNodeID, ref int colorNodeRID,
            out string aColorDescription)
        {
            ColorNodeInfo cni;  // TT#3573 - JSmith - Improve performance loading color level
            try
            {
                if (colorCode != null)
                {
                    colorCode = colorCode.Trim();
                }
                // Begin TT#3573 - JSmith - Improve performance loading color level
                //MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                //colorNodeRID = mhd.Hierarchy_ColorNodeRID_For_Style(hierarchyRID, nodeRID, colorCode, out  aColorDescription);
                //if (colorNodeRID == Include.NoRID)
                //{
                //    return false;
                //}
                //else
                //{
                //    return true;
                //}
                if (_dctColorNodeLookup.TryGetValue(nodeRID + ":" + colorCode, out cni))
                {
                    colorNodeRID = cni.NodeRID;
                    aColorDescription = cni.Description;
                    return true;
                }
                else
                {
                    MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                    colorNodeRID = mhd.Hierarchy_ColorNodeRID_For_Style(hierarchyRID, nodeRID, colorCode, out  aColorDescription);
                    if (colorNodeRID == Include.NoRID)
                    {
                        return false;
                    }
                    else
                    {
                        _dctColorNodeLookup.Add(nodeRID + ":" + colorCode, new ColorNodeInfo(colorNodeRID, aColorDescription));
                        return true;
                    }
                }
                // End TT#3573 - JSmith - Improve performance loading color level
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }
        // End TT#2763 - JSmith - Hierarchy Color descriptions not updating

		/// <summary>
		/// Checks to determine if a size is already assigned to a color
		/// </summary>
		/// <param name="hierarchyRID">The record ID of the hierarchy</param>
		/// <param name="nodeRID">The record ID of the color</param>
		/// <param name="sizeCode">The code associated with the size</param>
		/// <param name="aQualifiedNodeID">The fully qualified node ID including parent</param>
		/// <param name="sizeNodeRID">a reference to an integer that is used to return the record id of the node if 
		/// the size is already defined to the color</param>
		/// <returns>A flag identifying if the size is defined to the color</returns>
		static public bool SizeExistsForColor(int hierarchyRID, int nodeRID, string sizeCode, string aQualifiedNodeID, ref int sizeNodeRID)
		{
			try
			{
				if (sizeCode != null)
				{
					sizeCode = sizeCode.Trim();
				}
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				sizeNodeRID = mhd.Hierarchy_SizeNodeRID_For_Color(hierarchyRID, nodeRID, sizeCode);
				if (sizeNodeRID == Include.NoRID)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Builds a database table containing purge dates for all nodes
		/// </summary>
		/// <returns>A flag identifying if the purge dates were built</returns>
		/// <remarks>
		/// Purge criteria is chased differently than other node information.  The node is checked followed by
		/// the level definition followed by the parent.
		/// </remarks>
        // Begin TT#460 - JSmith - Add size daily blob tables to Purge
        //static public bool BuildPurgeDates()
        static public bool BuildPurgeDates(int aWeeksToKeepDailySizeOnhand)
        // End TT#460
		{
			bool errorFound = false;
            int[] hierarchyRIDs = null;  // TT#5210 - JSmith - Purge Performance
			HierarchyProfile hp = null;
			PurgeData pd = new PurgeData();

			// delete old purge dates
			try
			{
				pd.OpenUpdateConnection();
				pd.DeletePurgeDates();
				pd.CommitData();
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				errorFound = true;
			}
			finally
			{
				if (pd.ConnectionIsOpen)
				{
					pd.CloseUpdateConnection();
				}
			}

			// get posting date
			try
			{
				hp = HierarchyServerGlobal.GetMainHierarchyData();
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				errorFound = true;
			}

			if (!errorFound)
			{
				// get list of all hierarchy RIDs to loop through
                // Begin TT#5210 - JSmith - Purge Performance
                //try
                //{
                //    hierarchyRIDs = new int[_hierarchiesByRID.Count];
                //    hierarchy_rwl.AcquireReaderLock(ReaderLockTimeOut);
                //    try
                //    {
                //        int i = 1;
                //        hierarchyRIDs[0] = hp.Key; // TT#1549 - GRT - Urban Alternate Inheritence
                //        foreach (HierarchyInfo hi in _hierarchiesByRID.Values)
                //        {
                //            if (hi.HierarchyRID != hp.Key)
                //            {
                //                hierarchyRIDs[i] = hi.HierarchyRID;  // TT#1549 - GRT - Urban Alternate Inheritence
                //                ++i;
                //            }
                //        }
                //    }
                //    catch ( Exception ex )
                //    {
                //        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                //        if (Audit != null)
                //        {
                //            Audit.Log_Exception(ex);
                //        }
                //        // End TT#189
                //        errorFound = true;
                //    }
                //    finally
                //    {
                //        // Ensure that the lock is released.
                //        hierarchy_rwl.ReleaseReaderLock();
                //    }
                //}
                //catch (ApplicationException ex)
                //{
                //    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                //    if (Audit != null)
                //    {
                //        Audit.Log_Exception(ex);
                //    }
                //    // End TT#189
                //    // The reader lock request timed out.
                //    EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:BuildPurgeDates reader lock has timed out", EventLogEntryType.Error);
                //    throw new MIDException (eErrorLevel.severe,	0, "MIDHierarchyService:BuildPurgeDates reader lock has timed out");
                //}
                //catch ( Exception ex )
                //{
                //    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                //    if (Audit != null)
                //    {
                //        Audit.Log_Exception(ex);
                //    }
                //    // End TT#189
                //    throw;
                //}

                DataTable dt = pd.DistinctHierarchiesWithPurgeCriteria_Read();
                if (dt.Rows.Count > 0)
                {
                    hierarchyRIDs = new int[dt.Rows.Count];
                    int i = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        hierarchyRIDs[i] = Convert.ToInt32(dr["HOME_PH_RID"]);
                        ++i;
                    }
                }
                else
                {
                    return false; // stop process
                }
                // End TT#5210 - JSmith - Purge Performance

				// loop through each hierarchy to build purge dates
				try
				{
					if (!errorFound)
					{
						foreach (int hierarchyRID in hierarchyRIDs)
						{
							BuildHierarchyPurgeDates(hierarchyRID, Calendar.CurrentWeek, pd);
						}
                        // Begin TT#460 - JSmith - Add size daily blob tables to Purge
                        //UpdatePurgeTimeIDs(Calendar.CurrentWeek, pd);
                        UpdatePurgeTimeIDs(Calendar.CurrentWeek, pd, aWeeksToKeepDailySizeOnhand);
                        // End TT#460
					}
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					errorFound = true;
				}
			}

			return errorFound;
		}

		/// <summary>
		/// Builds purge dates for a hierarchy
		/// </summary>
		/// <param name="aHierarchyRID">The record ID of the hierarchy</param>
		/// <param name="aCurrentWeekProfile">The WeekProfile of the posting date</param>
		static private void BuildHierarchyPurgeDates(int aHierarchyRID, WeekProfile aCurrentWeekProfile,
			PurgeData aPurgeData)
		{
			try
			{
				if (!aPurgeData.ConnectionIsOpen)
				{
					aPurgeData.OpenUpdateConnection();
				}
				try
				{
					aPurgeData.BuildPurgeDates(aHierarchyRID);
					aPurgeData.CommitData();
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					if (aPurgeData.ConnectionIsOpen)
					{
						aPurgeData.CloseUpdateConnection();
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        // Begin TT#460 - JSmith - Add size daily blob tables to Purge
        //static private void UpdatePurgeTimeIDs(WeekProfile aCurrentWeekProfile, PurgeData aPurgeData)
        static private void UpdatePurgeTimeIDs(WeekProfile aCurrentWeekProfile, PurgeData aPurgeData, int aWeeksToKeepDailySizeOnhand)
        // End TT#460 
		{
			try
			{
				if (!aPurgeData.ConnectionIsOpen)
				{
					aPurgeData.OpenUpdateConnection();
				}
				DataTable dt = aPurgeData.DistinctDailyHistoryWeeks_Read();
				foreach (DataRow dr in dt.Rows)
				{
					if (dr["PURGE_DAILY_HISTORY_WEEKS"] != System.DBNull.Value)
					{
						int weeks = Convert.ToInt32(dr["PURGE_DAILY_HISTORY_WEEKS"], CultureInfo.CurrentUICulture);
						int timeID = DeterminePurgeTimeID(aCurrentWeekProfile, weeks);
                        // Begin TT#460 - JSmith - Add size daily blob tables to Purge
                        // use the purge criteria to determine the number of blob weeks.
                        // But, use configuration for daily size history
                        // Begin TT#739-MD -JSmith - Delete Stores
                        aPurgeData.DailyHistoryDates_Update(weeks, timeID);
                        //SQL_TimeID sqlDate = new SQL_TimeID(eSQLTimeIdType.TimeIdIsDaily, timeID);
                        //aPurgeData.DailyHistoryDates_Update(weeks, timeID, sqlDate.SqlTimeID);
                        //timeID = DeterminePurgeTimeID(aCurrentWeekProfile, aWeeksToKeepDailySizeOnhand);
                        //aPurgeData.DailySizeHistoryDates_Update(timeID);
                        // End TT#739-MD -JSmith - Delete Stores
                        // End TT#460
					}
				}

                // Begin TT#739-MD -JSmith - Delete Stores
                // update all non-size daily purge criteria to 2 weeks
                int timeID2 = DeterminePurgeTimeID(aCurrentWeekProfile, 2);
                aPurgeData.DailyNonSizeHistoryDates_Update(timeID2);
                // Begin TT#739-MD -JSmith - Delete Stores

				dt = aPurgeData.DistinctWeeklyHistoryWeeks_Read();
				foreach (DataRow dr in dt.Rows)
				{
					if (dr["PURGE_WEEKLY_HISTORY_WEEKS"] != System.DBNull.Value)
					{
						int weeks = Convert.ToInt32(dr["PURGE_WEEKLY_HISTORY_WEEKS"], CultureInfo.CurrentUICulture);
						int timeID = DeterminePurgeTimeID(aCurrentWeekProfile, weeks);
                        // Begin TT#460 - JSmith - Add size daily blob tables to Purge
                        // Begin TT#739-MD -JSmith - Delete Stores
                        aPurgeData.WeeklyHistoryDates_Update(weeks, timeID);
                        //SQL_TimeID sqlDate = new SQL_TimeID(eSQLTimeIdType.TimeIdIsDaily, timeID);
                        //aPurgeData.WeeklyHistoryDates_Update(weeks, timeID, sqlDate.SqlTimeID);
                        // End TT#739-MD -JSmith - Delete Stores
                        // End TT#460
					}
				}

				dt = aPurgeData.DistinctPlanWeeks_Read();
				foreach (DataRow dr in dt.Rows)
				{
					if (dr["PURGE_PLANS_WEEKS"] != System.DBNull.Value)
					{
						int weeks = Convert.ToInt32(dr["PURGE_PLANS_WEEKS"], CultureInfo.CurrentUICulture);
						int timeID = DeterminePurgeTimeID(aCurrentWeekProfile, weeks);
						aPurgeData.PlanDates_Update(weeks, timeID);
					}
				}

                //Begin TT#400-MD - JSmith - Add Header Purge Criteria by Header Type
                //dt = aPurgeData.DistinctHeaderWeeks_Read();
                //foreach (DataRow dr in dt.Rows)
                //{
                //    if (dr["PURGE_HEADERS_WEEKS"] != System.DBNull.Value)
                //    {
                //        int weeks = Convert.ToInt32(dr["PURGE_HEADERS_WEEKS"], CultureInfo.CurrentUICulture);
                //        DateTime dateTime = DeterminePurgeDate(aCurrentWeekProfile, weeks);
                //        // Begin TT#269-MD - JSmith - Modify Purge to use header weeks when purging daily percentages
                //        int timeID = DeterminePurgeTimeID(aCurrentWeekProfile, weeks);
                //        //aPurgeData.HeaderDates_Update(weeks, dateTime);
                //        aPurgeData.HeaderDates_Update(weeks, dateTime, timeID);
                //        // End TT#269-MD - JSmith - Modify Purge to use header weeks when purging daily percentages
                //    }
                //}

                dt = aPurgeData.DistinctReceiptHeaderWeeks_Read();
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["PURGE_HEADERS_WEEKS_RECEIPT"] != System.DBNull.Value)
                    {
                        int weeks = Convert.ToInt32(dr["PURGE_HEADERS_WEEKS_RECEIPT"], CultureInfo.CurrentUICulture);
                        DateTime dateTime = DeterminePurgeDate(aCurrentWeekProfile, weeks);
                        int timeID = DeterminePurgeTimeID(aCurrentWeekProfile, weeks);
                        aPurgeData.HeaderReceiptDates_Update(weeks, dateTime, timeID);
                    }
                }

                dt = aPurgeData.DistinctASNHeaderWeeks_Read();
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["PURGE_HEADERS_WEEKS_ASN"] != System.DBNull.Value)
                    {
                        int weeks = Convert.ToInt32(dr["PURGE_HEADERS_WEEKS_ASN"], CultureInfo.CurrentUICulture);
                        DateTime dateTime = DeterminePurgeDate(aCurrentWeekProfile, weeks);
                        int timeID = DeterminePurgeTimeID(aCurrentWeekProfile, weeks);
                        aPurgeData.HeaderASNDates_Update(weeks, dateTime, timeID);
                    }
                }

                dt = aPurgeData.DistinctDummyHeaderWeeks_Read();
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["PURGE_HEADERS_WEEKS_DUMMY"] != System.DBNull.Value)
                    {
                        int weeks = Convert.ToInt32(dr["PURGE_HEADERS_WEEKS_DUMMY"], CultureInfo.CurrentUICulture);
                        DateTime dateTime = DeterminePurgeDate(aCurrentWeekProfile, weeks);
                        int timeID = DeterminePurgeTimeID(aCurrentWeekProfile, weeks);
                        aPurgeData.HeaderDummyDates_Update(weeks, dateTime, timeID);
                    }
                }

                dt = aPurgeData.DistinctDropShipHeaderWeeks_Read();
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["PURGE_HEADERS_WEEKS_DROPSHIP"] != System.DBNull.Value)
                    {
                        int weeks = Convert.ToInt32(dr["PURGE_HEADERS_WEEKS_DROPSHIP"], CultureInfo.CurrentUICulture);
                        DateTime dateTime = DeterminePurgeDate(aCurrentWeekProfile, weeks);
                        int timeID = DeterminePurgeTimeID(aCurrentWeekProfile, weeks);
                        aPurgeData.HeaderDropShipDates_Update(weeks, dateTime, timeID);
                    }
                }

                dt = aPurgeData.DistinctReserveHeaderWeeks_Read();
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["PURGE_HEADERS_WEEKS_RESERVE"] != System.DBNull.Value)
                    {
                        int weeks = Convert.ToInt32(dr["PURGE_HEADERS_WEEKS_RESERVE"], CultureInfo.CurrentUICulture);
                        DateTime dateTime = DeterminePurgeDate(aCurrentWeekProfile, weeks);
                        int timeID = DeterminePurgeTimeID(aCurrentWeekProfile, weeks);
                        aPurgeData.HeaderReserveDates_Update(weeks, dateTime, timeID);
                    }
                }

                dt = aPurgeData.DistinctWorkupTotalBuyHeaderWeeks_Read();
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["PURGE_HEADERS_WEEKS_WORKUPTOTALBUY"] != System.DBNull.Value)
                    {
                        int weeks = Convert.ToInt32(dr["PURGE_HEADERS_WEEKS_WORKUPTOTALBUY"], CultureInfo.CurrentUICulture);
                        DateTime dateTime = DeterminePurgeDate(aCurrentWeekProfile, weeks);
                        int timeID = DeterminePurgeTimeID(aCurrentWeekProfile, weeks);
                        aPurgeData.HeaderWorkupTotalBuyDates_Update(weeks, dateTime, timeID);
                    }
                }

                dt = aPurgeData.DistinctPOHeaderWeeks_Read();
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["PURGE_HEADERS_WEEKS_PO"] != System.DBNull.Value)
                    {
                        int weeks = Convert.ToInt32(dr["PURGE_HEADERS_WEEKS_PO"], CultureInfo.CurrentUICulture);
                        DateTime dateTime = DeterminePurgeDate(aCurrentWeekProfile, weeks);
                        int timeID = DeterminePurgeTimeID(aCurrentWeekProfile, weeks);
                        aPurgeData.HeaderPODates_Update(weeks, dateTime, timeID);
                    }
                }

                dt = aPurgeData.DistinctVSWHeaderWeeks_Read();
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["PURGE_HEADERS_WEEKS_VSW"] != System.DBNull.Value)
                    {
                        int weeks = Convert.ToInt32(dr["PURGE_HEADERS_WEEKS_VSW"], CultureInfo.CurrentUICulture);
                        DateTime dateTime = DeterminePurgeDate(aCurrentWeekProfile, weeks);
                        int timeID = DeterminePurgeTimeID(aCurrentWeekProfile, weeks);
                        aPurgeData.HeaderVSWDates_Update(weeks, dateTime, timeID);
                    }
                }
                //End TT#400-MD - JSmith - Add Header Purge Criteria by Header Type
				aPurgeData.CommitData();
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
			finally
			{
				if (aPurgeData.ConnectionIsOpen)
				{
					aPurgeData.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Determines the purge time ID from a given number of weeks
		/// </summary>
		/// <param name="aCurrentWeekProfile">The WeekProfile of the current date</param>
		/// <param name="aPurgeWeeks">Weeks to purge</param>
		static private int DeterminePurgeTimeID(WeekProfile aCurrentWeekProfile, int aPurgeWeeks)
		{
			try
			{
				int purgeTimeID;
				WeekProfile purgeDate = null;

				//determine purge dates
				switch (aPurgeWeeks)
				{
					case -1:		// do not purge anything
						purgeTimeID = 0;
						break;
					case 0:			// purge all records (format yyyyddd)
						purgeTimeID = 9999999;
						break;
					default:		// determine date
						purgeDate = Calendar.Add(aCurrentWeekProfile, aPurgeWeeks * -1);
						purgeTimeID = purgeDate.Key;
						break;
				}
				
				return purgeTimeID;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Determines the purge date from a given number of weeks
		/// </summary>
		/// <param name="aPostingWeekProfile">The WeekProfile of the posting date</param>
		/// <param name="aPurgeWeeks">Weeks to purge</param>
		static private DateTime DeterminePurgeDate(WeekProfile aPostingWeekProfile, int aPurgeWeeks)
		{
			try
			{
				DateTime purgeDateTime;
				WeekProfile purgeDate = null;

				switch (aPurgeWeeks)
				{
					case -1:		// do not purge anything
						purgeDateTime = Include.PurgeMinDate;
						break;
					case 0:			// purge all records
						purgeDateTime = Include.PurgeMaxDate;
						break;
					default:		// determine date
						purgeDate = Calendar.Add(aPostingWeekProfile, (aPurgeWeeks - 1) * -1);
						purgeDateTime = purgeDate.Date;
						break;
				}
				
				return purgeDateTime;
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Requests a list of product characteristics.
		/// </summary>
		/// <returns>
		/// A ProductCharProfileList of ProductCharProfile
		/// </returns>
		static public ProductCharProfileList GetProductCharacteristics()
		{
			try
			{
				ProductCharProfileList pcpl = new ProductCharProfileList(eProfileType.ProductCharacteristic);

				// clone hashtable to reduce lock time
                Dictionary<int, ProductCharInfo> productCharByRID = null;
				try
				{
					lock (_productChar_lock.SyncRoot)
					{
                        //productCharByRID = (Hashtable)HierarchyServerGlobal._productCharByRID.Clone();
                        productCharByRID = CloneDictionary<int, ProductCharInfo>(_productCharByRID);
					}
				}
				catch (Exception ex)
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				foreach (ProductCharInfo pci in productCharByRID.Values)
				{
					pcpl.Add(GetProductCharProfile(pci.ProductCharRID));
				}

				return pcpl;
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Requests the product characteristics assigned to the node.
		/// </summary>
		/// <returns>
		/// A NodeCharProfileList of NodeCharProfile
		/// </returns>
		static public NodeCharProfileList GetProductCharacteristics(int aNodeRID, bool aChaseHierarchy)
		{
			try
			{
				NodeCharProfileList ncpl = GetProductCharacteristics(aNodeRID);
				if (aChaseHierarchy)
				{
					NodeInfo ni =  GetNodeInfoByRID(aNodeRID, false);
					NodeAncestorList nal = GetNodeAncestorList(aNodeRID, ni.HomeHierarchyRID);
					foreach (NodeAncestorProfile nap in nal)
					{
						// skip the node
						if (nap.Key != aNodeRID)
						{
							NodeCharProfileList charList = GetProductCharacteristics(nap.Key);
							foreach (NodeCharProfile ncp in charList)
							{
								if (ncpl.FindKey(ncp.Key) == null)
								{
									ncp.InheritedFrom = nap.Key;
									ncp.TypeInherited = eInheritedFrom.Node;
									ncpl.Add(ncp);
								}
							}
						}
					}
				}
				

				return ncpl;
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Requests the product characteristics assigned to the node.
		/// </summary>
		/// <returns>
		/// A NodeCharProfileList of NodeCharProfile
		/// </returns>
		static private NodeCharProfileList GetProductCharacteristics(int aNodeRID)
		{
			try
			{
				NodeCharProfileList ncpl = new NodeCharProfileList(eProfileType.ProductCharacteristic);
				NodeInfo ni = GetNodeInfoByRID(aNodeRID);
				if (!ni.ProductCharsLoaded)
				{
					LoadCharacteristics(ni);
					SetNodeCacheByRID(aNodeRID, ni);
				}

				// needs to check for inherited values
				for (int i = 0; i < ni.CharValueCount(); i++)
				{
					int productCharValueRID = (int)ni.ProductCharValues[i];
					ProductCharValueInfo pcvi = GetProductCharValueInfo(productCharValueRID);
					ProductCharInfo pci = GetProductCharInfo(pcvi.ProductCharRID);
					NodeCharProfile ncp = new NodeCharProfile(pcvi.ProductCharRID);
					ncp.ProductCharID = pci.ProductCharID;
					ncp.ProductCharValueRID = productCharValueRID;
					ncp.ProductCharValue = pcvi.ProductCharValue;
					ncpl.Add(ncp);
				}

				return ncpl;
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Updates the product characteristics assigned to the node.
		/// </summary>
		/// <param name="aNodeRID">The key of the node to be updated</param>
		/// <param name="aNodeCharProfileList">
		/// An instance of the NodeCharProfileList class containing instances of the NodeCharProfile class.
		/// </param>
		static public void UpdateProductCharacteristics(int aNodeRID, NodeCharProfileList aNodeCharProfileList)
		{
			try
			{
				NodeInfo ni = GetNodeInfoByRID(aNodeRID);
				ni.ClearCharValues();

				foreach (NodeCharProfile ncp in aNodeCharProfileList)
				{
					switch (ncp.ProductCharChangeType)
					{
						case eChangeType.none:
						case eChangeType.add:
						case eChangeType.update:
							{
								ni.AddCharValue(ncp.ProductCharValueRID);
								break;
							}
						case eChangeType.delete:
							{
								break;
							}
					}
				}
				SetNodeCacheByRID(ni.NodeRID, ni);
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Requests a product characteristic profile for the given characteristic name.
		/// </summary>
		/// <returns>
		/// A ProductCharValueProfile
		/// </returns>
		static public ProductCharProfile GetProductCharProfile(string aProductCharID)
		{
			try
			{
				return GetProductCharProfile(GetProductCharRIDByID(aProductCharID));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests a product characteristic profile for the given characteristic key.
		/// </summary>
		/// <returns>
		/// A ProductCharValueProfile
		/// </returns>
		static public ProductCharProfile GetProductCharProfile(int aProductCharRID)
		{
			try
			{
				ProductCharInfo pci = GetProductCharInfo(aProductCharRID);
				ProductCharProfile pcp = new ProductCharProfile(aProductCharRID);
				pcp.ProductCharID = pci.ProductCharID;
				for (int i = 0; i < pci.CharValueCount(); i++)
				{
					int productCharValueRID = (int)pci.ProductCharValues[i];
					ProductCharValueInfo pcvi = GetProductCharValueInfo(productCharValueRID);
					ProductCharValueProfile pcvp = new ProductCharValueProfile(pcvi.ProductCharValueRID);
					pcvp.ProductCharRID = pcvi.ProductCharRID;
					pcvp.ProductCharValue = pcvi.ProductCharValue;
					pcp.ProductCharValues.Add(pcvp);
				}
				return pcp;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Adds or updates information about a product characteristic
		/// </summary>
		/// <param name="aProductCharProfile">Information about the product characteristic</param>
        static public void ProductCharUpdate(ProductCharProfile aProductCharProfile, bool isReloadAllValues)  // TT#3558 - JSmith - Perf of Hierarchy Load
		{
			try
			{
				characteristic_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					switch (aProductCharProfile.ProductCharChangeType)
					{
						case eChangeType.none:
							{
								break;
							}
						case eChangeType.add:
							{
								ProductCharInfo pci = new ProductCharInfo();
								pci.ProductCharRID = aProductCharProfile.Key;
								pci.ProductCharID = aProductCharProfile.ProductCharID;
								foreach (ProductCharValueProfile pcvp in aProductCharProfile.ProductCharValues)
								{
									if (pcvp.ProductCharValueChangeType != eChangeType.delete)
									{
										ProductCharValueInfo pcvi = new ProductCharValueInfo();
										pcvi.ProductCharValueRID = pcvp.Key;
										pcvi.ProductCharRID = aProductCharProfile.Key;
										pcvi.ProductCharValue = pcvp.ProductCharValue;
                                        // Begin TT#256 - JSmith - Error auto adding hierarchy characteristic value
                                        //_productCharValueByRID.Add(pcvi.ProductCharValueRID, pcvi);
                                        _productCharValueByRID[pcvi.ProductCharValueRID] = pcvi;
                                        // End TT#256
										pci.AddCharValue(pcvi.ProductCharValueRID);
									}
								}
                                // Begin TT#256 - JSmith - Error auto adding hierarchy characteristic value
                                //_productCharByRID.Add(pci.ProductCharRID, pci);
                                //_productCharByID.Add(pci.ProductCharID, pci.ProductCharRID);
                                _productCharByRID[pci.ProductCharRID] = pci;
                                _productCharByID[pci.ProductCharID] = pci.ProductCharRID;
                                // End TT#256
								break;
							}
						case eChangeType.update:
							{
								ProductCharInfo pci = (ProductCharInfo)_productCharByRID[aProductCharProfile.Key];
								_productCharByID.Remove(aProductCharProfile.ProductCharID);
								pci.ProductCharID = aProductCharProfile.ProductCharID;
                                // Begin TT#2056 - JSmith - Auto added characteristic and values not showing
                                // Begin TT#715-MD - JSmith - Product Characteristics does not delete 
                                // Begin TT#3558 - JSmith - Perf of Hierarchy Load
                                //pci.ClearCharValues();
                                if (isReloadAllValues)
                                {
                                    pci.ClearCharValues();
                                }
                                // End TT#3558 - JSmith - Perf of Hierarchy Load
                                // End TT#715-MD - JSmith - Product Characteristics does not delete 
                                // End TT#2056
								foreach (ProductCharValueProfile pcvp in aProductCharProfile.ProductCharValues)
								{
									if (pcvp.ProductCharValueChangeType != eChangeType.delete)
									{
										ProductCharValueInfo pcvi = new ProductCharValueInfo();
										pcvi.ProductCharValueRID = pcvp.Key;
										pcvi.ProductCharRID = aProductCharProfile.Key;
										pcvi.ProductCharValue = pcvp.ProductCharValue;
										_productCharValueByRID[pcvi.ProductCharValueRID] = pcvi;
										pci.AddCharValue(pcvi.ProductCharValueRID);
									}
									//BEGIN TT#3962-VStuart-Dragged Values never allowed to drop-MID
                                    // Begin Track #5729 - JSmith - Characteristic not deleted
                                    else if (!pcvp.HasBeenMoved)
                                    {
                                        _productCharValueByRID.Remove(pcvp.Key);
                                    }
                                    // End Track #5729
									//END TT#3962-VStuart-Dragged Values never allowed to drop-MID
								}
								_productCharByRID[pci.ProductCharRID] = pci;
                                // Begin TT#256 - JSmith - Error auto adding hierarchy characteristic value
                                //_productCharByID.Add(pci.ProductCharID, pci.ProductCharRID);
                                _productCharByID[pci.ProductCharID] = pci.ProductCharRID;
                                // End TT#256
								break;
							}
						case eChangeType.delete:
							{
								ProductCharInfo pci = (ProductCharInfo)_productCharByRID[aProductCharProfile.Key];
                                _productCharByRID.Remove(pci.ProductCharRID);
                                _productCharByID.Remove(pci.ProductCharID);

								break;
							}
					}
				}
				catch (Exception ex)
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					EventLog.WriteEntry("MIDHierarchyService", "ProductCharUpdate error:" + ex.Message, EventLogEntryType.Error);
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					characteristic_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:ProductCharUpdate writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:ProductCharUpdate writer lock has timed out");
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Requests a product characteristic profile for the given key.
		/// </summary>
		/// <returns>
		/// A ProductCharValueProfile
		/// </returns>
		static public ProductCharValueProfile GetProductCharValueProfile(int aProductCharValueRID)
		{
			try
			{
				ProductCharValueProfile pcvp = new ProductCharValueProfile(aProductCharValueRID);
				ProductCharValueInfo pcvi = GetProductCharValueInfo(aProductCharValueRID);
				pcvp.ProductCharValue = pcvi.ProductCharValue;
				pcvp.ProductCharRID = pcvi.ProductCharRID;
				return pcvp;
			}
			catch
			{
				throw;
			}
		}

        public static Dictionary<K, V> CloneDictionary<K, V>(Dictionary<K, V> dict)
        {
            Dictionary<K, V> newDict = null;

            // The clone method is immune to the source dictionary being null.
            if (dict != null)
            {
                // If the key and value are value types, clone without serialization.
                if (((typeof(K).IsValueType || typeof(K) == typeof(string)) &&
                     (typeof(V).IsValueType) || typeof(V) == typeof(string)))
                {
                    newDict = new Dictionary<K, V>();
                    // Clone by copying the value types.
                    foreach (KeyValuePair<K, V> kvp in dict)
                    {
                        newDict[kvp.Key] = kvp.Value;
                    }
                }
                else
                {
                    // Clone by serializing to a memory stream, then deserializing.
                    // Don't use this method if you've got a large objects, as the
                    // BinaryFormatter produces bloat, bloat, and more bloat.
                    BinaryFormatter bf = new BinaryFormatter();
                    MemoryStream ms = new MemoryStream();
                    bf.Serialize(ms, dict);
                    ms.Position = 0;
                    newDict = (Dictionary<K, V>)bf.Deserialize(ms);
                }
            }

            return newDict;
        }

#region Chain Set Percent
        //Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3

        static public void ChainSetPercentUpdate(int nodeRID, ChainSetPercentList scl)
        {
            try
            {
                chainSetPercent_rwl.AcquireWriterLock(WriterLockTimeOut);
                try
                {
                    NodeChainSetPercentInfo ncssp = (NodeChainSetPercentInfo)_chainSetPercentByRID[nodeRID];
                    if (ncssp == null)
                    {
                        ncssp = new NodeChainSetPercentInfo();
                    }
                    ChainSetPercentInfo cspi;

                    foreach (ChainSetPercentProfiles cspp in scl)
                    {
                        if (cspp.ChainSetPercentChangeType != eChangeType.none)
                        {
                            cspi = (ChainSetPercentInfo)ncssp.ChainSetPercent[cspp.Key];
                            if (cspi == null)
                            {
                                cspi = new ChainSetPercentInfo();
                            }

                            if (cspp.ChainSetPercentChangeType == eChangeType.delete)
                            {
                                ncssp.ChainSetPercent.Remove(cspp.Key);
                            }
                            else
                            {
                                cspi.StoreGroupLevelRID = cspp.StoreGroupLevelRID;
                                cspi.storeGroupLevelId = cspp.NodeID;
                                cspi.storeGroupId = cspp.StoreGroupID;
                                cspi.StoreGroupRID = cspp.StoreGroupRID;
                                cspi.TimeID = cspp.TimeID;
                                cspi.ChainSetPercent = cspp.ChainSetPercent;
                                ncssp.ChainSetPercent[cspi.StoreGroupLevelRID] = cspi;
                            }
                        }
                    }

                    _chainSetPercentByRID[nodeRID] = ncssp;
                }
                finally
                {
                    // Ensure that the lock is released.
                    chainSetPercent_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:ChainSetPercentUpdate writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:ChainSetPercentUpdate writer lock has timed out");
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
        }

        static public ChainSetPercentList GetChainSetPercentList(ProfileList storeList, int nodeRID, bool stopOnFind, bool forCopy, bool forAdmin, SessionAddressBlock _SAB, ProfileList WeekList)
        {
            try
            {
                
                bool setInheritance = false;
                int lowestNodeRID = Include.NoRID;
                NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
                NodeAncestorList nal = GetNodeAncestorList(nodeRID, ni.HomeHierarchyRID);
                ChainSetPercentList cspl = new ChainSetPercentList(eProfileType.ChainSetPercent);
                foreach (NodeAncestorProfile nap in nal)
                {
                    cspl = GetChainSetPercent(storeList, nap.Key, cspl, forCopy, forAdmin, setInheritance, _SAB, WeekList);
                    if (lowestNodeRID == Include.NoRID && cspl.Count > 0)
                    {
                        lowestNodeRID = nap.Key;
                    }

                    if (cspl.Count == (storeList.Count * WeekList.Count) ||		//  stop lookup if have Chain Set Percent for each store
                        forCopy)							//  or stop on first node if copy
                    {
                        //break;
                    }
                    else
                        if (stopOnFind && cspl.Count > 0)	//  stop if you find Chain Set Percent for any store
                        {
                            break;
                        }
                    setInheritance = true;
                }
                if (stopOnFind && !forCopy)
                {
                    
                    foreach (StoreProfile sp in storeList)	// set all store not found to int.max
                    {
                        if (!cspl.Contains(sp.Key))
                        {
                            
                            ChainSetPercentProfiles scp = new ChainSetPercentProfiles(sp.Key);
                            scp.ChainSetPercent = int.MaxValue;
                            scp.NodeRID = lowestNodeRID;

                            cspl.Add(scp);
                        }
                    }
                }

                return cspl;
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

        static private ChainSetPercentList GetChainSetPercent(ProfileList storeList, int nodeRID,
            ChainSetPercentList scl, bool forCopy, bool forAdmin, bool setInheritance, SessionAddressBlock _SAB, ProfileList WeekList)
        {
            NodeChainSetPercentInfo nsci = null;
            NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
            int beg_Week;
            int end_Week;
            int storeKey;
            Hashtable AttributeSetHash = new Hashtable();
            int storeGroupRID = 0;
            int currStoreGroupRID = Include.NoRID;
            ProfileList strGrpLvlProf = null;
            
            try
            {
                if (nsci == null)
                {
                    beg_Week = WeekList.MinValue;
                    end_Week = WeekList.MaxValue;
                    nsci = new NodeChainSetPercentInfo();
                    
                    LoadChainSetPercent(nodeRID, beg_Week, end_Week, nsci);
                    if (nsci.ChainSetPercent.Count > 0)

                    UpdateChainSetPercentHash(nodeRID, nsci);
                }



                ChainSetPercentInfo sci;
                Hashtable weekKeyHash = new Hashtable();
                foreach (DictionaryEntry val in nsci.ChainSetPercent)
                {
                    sci = (ChainSetPercentInfo)val.Value;

                        if (!scl.Contains(Convert.ToInt32(val.Key)))  // Do you already have this store
                        {
                            ChainSetPercentProfiles scp = new ChainSetPercentProfiles(Convert.ToInt32(val.Key));
                            scp.StoreWeekID = Convert.ToInt32(val.Key);
                            scp.StoreGroupRID = sci.StoreGroupRID;
                            scp.StoreGroupID = sci.storeGroupId;
                            scp.StoreGroupVersion = sci.StoreGroupVersion;  // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                            scp.StoreGroupLevelRID = sci.StoreGroupLevelRID;
                            scp.StoreGroupLevelID = sci.storeGroupLevelId;
                            scp.ChainSetPercent = sci.ChainSetPercent;
                            scp.TimeID = sci.TimeID;
                            scp.NodeRID = nodeRID;
                            scp.NodeID = ni.NodeName;
                            scp.NewRecord = false;
                            if (forCopy)
                            {
                                scp.ChainSetPercentChangeType = eChangeType.add;
                            }

                            if (setInheritance)
                            {
                                scp.ChainSetPercentIsInherited = true;
                                scp.ChainSetPercentInheritedFromNodeRID = nodeRID;
                            }
                            weekKeyHash[sci.TimeID] = scp.StoreGroupRID;
                            scl.Add(scp);
                        }
                }


                if (scl.Count > 0)
                {
                    if (forAdmin)
                    {
                        foreach (StoreGroupLevelProfile sglp in storeList)
                        {
                            foreach (WeekProfile wp in WeekList)
                            {
                                if (weekKeyHash.Contains(wp.YearWeek))
                                {
                                    // Begin TT#1719 - JSmith - Chain Set Pct:  System Overflow exception in Node Properties
                                    //storeKey = Convert.ToInt32(Convert.ToString(wp.YearWeek) + Convert.ToString(sglp.Key));
                                    storeKey = Convert.ToInt32(Convert.ToString(wp.YearWeek - 200000) + Convert.ToString(sglp.Key));
                                    // End TT#1719
                                    if (!scl.Contains(storeKey))
                                    {
                                        ChainSetPercentProfiles scp = new ChainSetPercentProfiles(storeKey);
                                        scp.StoreWeekID = Convert.ToInt32(storeKey);
                                        scp.StoreGroupRID = sglp.GroupRid;
                                        //scp.StoreGroupID = sglp.;
                                        scp.StoreGroupLevelRID = sglp.Key;
                                        scp.StoreGroupLevelID = sglp.Name;
                                        //scp.ChainSetPercent = null;
                                        scp.TimeID = wp.YearWeek;
                                        scp.NodeRID = nodeRID;
                                        scp.NodeID = ni.NodeName;
                                        scp.NewRecord = false;
                                        if (forCopy)
                                        {
                                            scp.ChainSetPercentChangeType = eChangeType.add;
                                        }

                                        if (setInheritance)
                                        {
                                            scp.ChainSetPercentIsInherited = true;
                                            scp.ChainSetPercentInheritedFromNodeRID = nodeRID;
                                        }

                                        scl.Add(scp);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (WeekProfile wp in WeekList)
                        {
                            if (weekKeyHash.Contains(wp.YearWeek))
                            {
                                storeGroupRID = Convert.ToInt32(weekKeyHash[wp.YearWeek]);
                                if (currStoreGroupRID != storeGroupRID)
                                {
                                    strGrpLvlProf = StoreMgmt.StoreGroup_GetLevelListViewList(storeGroupRID, false); //_SAB.StoreServerSession.GetStoreGroupLevelListViewList(storeGroupRID, false);
                                    currStoreGroupRID = storeGroupRID;
                                }

                                foreach (StoreGroupLevelListViewProfile sglp in strGrpLvlProf)
                                {
                                    // Begin TT#1719 - JSmith - Chain Set Pct:  System Overflow exception in Node Properties
                                    //storeKey = Convert.ToInt32(Convert.ToString(wp.YearWeek) + Convert.ToString(sglp.Key));
                                    storeKey = Convert.ToInt32(Convert.ToString(wp.YearWeek - 200000) + Convert.ToString(sglp.Key));
                                    // End TT#1719
                                    if (!scl.Contains(storeKey))
                                    {
                                        ChainSetPercentProfiles scp = new ChainSetPercentProfiles(storeKey);
                                        scp.StoreWeekID = Convert.ToInt32(storeKey);
                                        scp.StoreGroupRID = sglp.GroupRid;
                                        //scp.StoreGroupID = sglp.;
                                        scp.StoreGroupLevelRID = sglp.Key;
                                        scp.StoreGroupLevelID = sglp.Name;
                                        //scp.ChainSetPercent = null;
                                        scp.TimeID = wp.YearWeek;
                                        scp.NodeRID = nodeRID;
                                        scp.NodeID = ni.NodeName;
                                        scp.NewRecord = false;
                                        if (forCopy)
                                        {
                                            scp.ChainSetPercentChangeType = eChangeType.add;
                                        }

                                        if (setInheritance)
                                        {
                                            scp.ChainSetPercentIsInherited = true;
                                            scp.ChainSetPercentInheritedFromNodeRID = nodeRID;
                                        }

                                        scl.Add(scp);
                                    }
                                }
                            }
                        }
                    }
                }
                
                return scl;
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

        //Begin TT#1708 - DOConnell - Chain Set % Node Properties: Recieved a Serialization Exception Error
        static public ProfileList GetWeeks(int planDrp_rid, SessionAddressBlock _SAB)
        {

            // Get full week range
            DateRangeProfile drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(planDrp_rid);
            ProfileList weekRange = _SAB.ApplicationServerSession.Calendar.GetWeekRange(drp, null);
            ProfileList weekRangeMirror = _SAB.ApplicationServerSession.Calendar.GetWeekRange(drp, null);
            // get posting week
            WeekProfile currentWeek = _SAB.ApplicationServerSession.Calendar.CurrentWeek;

            return weekRange;
        }
        //End TT#1708 - DOConnell - Chain Set % Node Properties: Recieved a Serialization Exception Error

        static private void LoadChainSetPercent(int aNodeRID, int beg_Week, int end_Week, NodeChainSetPercentInfo aNodeChainSetPercentInfo)
        {
            int comboStWk;
            try
            {
                
                MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                DataTable dt = mhd.ChainSetPercentSet_Read(aNodeRID, beg_Week, end_Week);
                aNodeChainSetPercentInfo.ChainSetPercent.Clear();
                foreach (DataRow dr in dt.Rows)
                {

                    ChainSetPercentInfo cspi = new ChainSetPercentInfo();

                    cspi.StoreGroupLevelRID = Convert.ToInt32(dr["SGL_RID"], CultureInfo.CurrentUICulture);
                    cspi.storeGroupLevelId = Convert.ToString(dr["SGL_ID"], CultureInfo.CurrentUICulture);
                    cspi.StoreGroupRID = Convert.ToInt32(dr["SG_RID"], CultureInfo.CurrentUICulture);
                    cspi.storeGroupId = Convert.ToString(dr["SG_ID"], CultureInfo.CurrentUICulture);
                    cspi.StoreGroupVersion = Convert.ToInt32(dr["SG_VERSION"], CultureInfo.CurrentUICulture);  // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error. 
                    decimal pctValue;
                    decimal.TryParse(Convert.ToString(dr["Percentage"]), out pctValue);
                    cspi.ChainSetPercent = Convert.ToDecimal(pctValue, CultureInfo.CurrentUICulture);
                    cspi.TimeID = Convert.ToInt32(dr["TIME_ID"], CultureInfo.CurrentUICulture);

                    // Begin TT#1719 - JSmith - Chain Set Pct:  System Overflow exception in Node Properties
                    //comboStWk = Convert.ToInt32(Convert.ToString(cspi.TimeID) 
                    //            + Convert.ToString(cspi.StoreGroupLevelRID), CultureInfo.CurrentUICulture);
                    comboStWk = Convert.ToInt32(Convert.ToString(cspi.TimeID - 200000)
                                + Convert.ToString(cspi.StoreGroupLevelRID), CultureInfo.CurrentUICulture);
                    // End TT#1719
                    

                    //IF percentage <> 0
                    //if (!aNodeChainSetPercentInfo.ChainSetPercent.Contains(cspi.StoreGroupLevelRID))
                    aNodeChainSetPercentInfo.ChainSetPercent.Add(comboStWk, cspi);
                }
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

        static private void UpdateChainSetPercentHash(int aNodeRID, NodeChainSetPercentInfo aNodeChainSetPercentInfo)
        {
            try
            {
                chainSetPercent_rwl.AcquireWriterLock(WriterLockTimeOut);
                try
                {
                    if (!_chainSetPercentByRID.Contains(aNodeRID))
                    {
                        _chainSetPercentByRID.Add(aNodeRID, aNodeChainSetPercentInfo);
                    }
                }
                finally
                {
                    // Ensure that the lock is released.
                    chainSetPercent_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDHierarchyService", "MIDHierarchyService:UpdateChainSetPercentHash writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHierarchyService:UpdateChainSetPercentHash writer lock has timed out");
            }
            catch (Exception ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                throw;
            }
        }

    #endregion
	}

}
