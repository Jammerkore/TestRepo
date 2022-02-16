using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using Logility.ROWebSharedTypes;

namespace MIDRetail.Business
{
    /// <summary>
    /// 
    /// </summary>
    public class OTSPlanMethod : OTSPlanBaseMethod
    {
        private OTSPlanMethodData _OTSPlanData;
        private int _Orig_Plan_HN_RID;
        private int _Plan_HN_RID;
        private int _Plan_FV_RID;
        private int _CDR_RID;
        private int _Chain_FV_RID;
        private bool _Bal_Sales_Ind;
        private bool _Bal_Stock_Ind;
        private double[,] _Entered_Weight_Values;
        // BEGIN TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 
        // _ApplyTrendOptionsInd values: Chain Plans = 'C', Chain WOS = 'W', Plug Chain WOS = 'S'
        private char _ApplyTrendOptionsInd;
        private float _ApplyTrendOptionsWOSValue;
        // END TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 
        private ProfileList _GLFProfileList;
        private int _originalSGRid;
        private string _computationMode;
        private int _forecastModelRid;

        private ForecastModelProfile _forecastingModel;
        //Begin Track #4371 - JSmith - Multi-level forecasting.
        private bool _highLevelInd;
        private bool _lowLevelsInd;
        private eLowLevelsType _lowLevelsType;
        private int _lowLevelsOffset;
        private int _lowLevelsSequence;
        //private ProfileList			_lowlevelOverrideList = null; // Override Low Level Enhancement
        //End Track #4371
        private LowLevelVersionOverrideProfileList _lowlevelOverrideList = null; // Override Low Level Enhancement

        private ModelVariableProfile _currentVariable;
        private ModelVariableProfile _salesVariable;
        private ModelVariableProfile _stockVariable;
        private ModelVariableProfile _overrideVariable;     // Issue 4962 stodd


        string _sourceModule = "OTSPlanMethod.cs";
        private SessionAddressBlock _SAB;
        private PlanCubeGroup _cubeGroup;
        private PlanOpenParms _openParms;
        private ApplicationSessionTransaction _applicationTransaction;

        private Hashtable _storeGroupLevelHash;
        private ProfileList _allStoreList;
        private ProfileList _weeksToPlan;
        private bool _firstSetToPlan;
        private int _reserveStoreRid;
        // BEGIN MID Track #6043 - KJohnson - Forcast Chain WOS Calculations
        private int _numberOfWeeksWithZeroSales;
        private int _maximumChainWOS;
        private bool _prorateChainStock;
        // END MID Track #6043 - KJohnson

        // BEGIN Issue 4818
        private DataTable _dtGroupLevelBasis;
        // END Issue 4818
        private string _errorMsg;
        private ForecastMonitor _forecastMonitor;
        private bool _monitor;
        private string _monitorFilePath;
        private eForecastMonitorMode _monitorMode;
        private double _planFactor;
        private ForecastStockMinMax _stockMinMax;
        private HierarchyNodeProfile _hnp;
        private WeekProfile _firstWeekOfPlan = null;
        private WeekProfile _firstWeekOfBasis = null;  // 4025
        private PeriodProfile _planPeriod;

        private int _nodeOverride = Include.NoRID;
        private int _versionOverride = Include.NoRID;
        private string _infoMsg;
        private ArrayList _setLists;
        // BEGIN MID Track #4370 - John Smith - FWOS Models
        Hashtable _inventoryForecastSummandHash = null;
        int _maxBalanceAttempts = 5;
        // END MID Track #4370
        private WeekProfile _weekBeingPlanned = null;

        private eInheritedFrom _minMaxInheritedFrom = eInheritedFrom.None;
        private int _minMaxInheritedFromNodeRID;
        private bool _minMaxAttributeMismatch;

        private Hashtable _stockMinMaxUsedHash;
        private int _overrideLLRid;

        // BEGIN TT#279-MD - stodd - Projected Sales 
        private Dictionary<int, double> _applyToWeights = new Dictionary<int, double>();
        private bool _applyToWeightsFilled;
        // END TT#279-MD - stodd - Projected Sales 

        // BEGIN Issue 4817 stodd
        struct WeightedBasisValue
        {
            private double _value;
            // Begin Track #6171
            private int _index;
            private int _basisIndex;
            private int _basisDetailIndex;
            // End Track #6171

            public double Value
            {
                get { return _value; }
                set { _value = value; }
            }
            public int Index
            {
                get { return _index; }
                set { _index = value; }
            }
            // Begin Track #6171
            public int BasisIndex
            {
                get { return _basisIndex; }
                set { _basisIndex = value; }
            }
            public int BasisDetailIndex
            {
                get { return _basisDetailIndex; }
                set { _basisDetailIndex = value; }
            }

            public WeightedBasisValue(int basisIndex, int basisDetailIndex, int index, double aValue)
            {
                _index = index;
                _basisIndex = basisIndex;
                _basisDetailIndex = basisDetailIndex;
                _value = aValue;
            }
            // End track #6171
        }
        // END Issue 4817

        // Begin TT#1413 - DOConnell - Chain Set Percent
        private DataTable _ChainSetPercentValues;
        private ChainSetPercentList _chainSetPercentList;
        private ProfileList _storeList;
        private ProfileList _weekList;
        public class ChainSetPercentValues : IComparable<ChainSetPercentValues>
        {
            private int _index;
            private double _value;
            private int _week;

            public int CompareTo(ChainSetPercentValues other)
            {

                return _index.CompareTo(other._index);
            }

            public int Index
            {
                get { return _index; }
                set { _index = value; }
            }

            public double Value
            {
                get { return _value; }
                set { _value = value; }
            }

            public int Week
            {
                get { return _week; }
                set { _week = value; }
            }

            public ChainSetPercentValues(int index, double aValue, int bValue)
            {
                _index = index;
                _value = aValue;
                _week = bValue;

            }

        }

        public class ChainSetKeyValues : IComparable<ChainSetKeyValues>
        {
            private int _index;
            private int _value;
            private double _Pct;
            private int _Week;

            public int CompareTo(ChainSetKeyValues other)
            {

                return _index.CompareTo(other._index);
            }

            public int Index
            {
                get { return _index; }
                set { _index = value; }
            }

            public int Value
            {
                get { return _value; }
                set { _value = value; }
            }

            public Double Pct
            {
                get { return _Pct; }
                set { _Pct = value; }
            }

            public int Week
            {
                get { return _Week; }
                set { _Week = value; }
            }

            public ChainSetKeyValues(int index, int aValue, double bValue, int cValue)
            {
                _index = index;
                _value = aValue;
                _Pct = bValue;
                _Week = cValue;

            }

        }
        // End TT#1413 - DOConnell - Chain Set Percent

        #region Properties
        // BEGIN MID Track #4370 - John Smith - FWOS Models
        //		public ProfileList WeeksToPlan
        //		{
        //			get	{return _weeksToPlan;}
        //			set	{_weeksToPlan = value;	}
        //		}
        // END MID Track #4370

        public ProfileList AllStoreList
        {
            get { return _allStoreList; }
            set { _allStoreList = value; }
        }
        public Hashtable StoreGroupLevelHash
        {
            get { return _storeGroupLevelHash; }
            set { _storeGroupLevelHash = value; }
        }

        public int Plan_HN_RID
        {
            get { return _Plan_HN_RID; }
            set { _Plan_HN_RID = value; }
        }

        public int Orig_Plan_HN_RID
        {
            get { return _Orig_Plan_HN_RID; }
            set { _Orig_Plan_HN_RID = value; }
        }

        public int Plan_FV_RID
        {
            get { return _Plan_FV_RID; }
            set { _Plan_FV_RID = value; }
        }
        public int CDR_RID
        {
            get { return _CDR_RID; }
            set { _CDR_RID = value; }
        }
        public int Chain_FV_RID
        {
            get { return _Chain_FV_RID; }
            set { _Chain_FV_RID = value; }
        }

        public bool Bal_Sales_Ind
        {
            get { return _Bal_Sales_Ind; }
            set { _Bal_Sales_Ind = value; }
        }

        public bool Bal_Stock_Ind
        {
            get { return _Bal_Stock_Ind; }
            set { _Bal_Stock_Ind = value; }
        }

        // BEGIN TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 
        public char ApplyTrendOptionsInd
        {
            get { return _ApplyTrendOptionsInd; }
            set { _ApplyTrendOptionsInd = value; }
        }

        public float ApplyTrendOptionsWOSValue
        {
            get { return _ApplyTrendOptionsWOSValue; }
            set { _ApplyTrendOptionsWOSValue = value; }
        }
        // END TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 

        //Begin Track #4371 - JSmith - Multi-level forecasting.
        public bool HighLevelInd
        {
            get { return _highLevelInd; }
            set { _highLevelInd = value; }
        }

        public bool LowLevelsInd
        {
            get { return _lowLevelsInd; }
            set { _lowLevelsInd = value; }
        }

        public eLowLevelsType LowLevelsType
        {
            get { return _lowLevelsType; }
            set { _lowLevelsType = value; }
        }

        public int LowLevelsOffset
        {
            get { return _lowLevelsOffset; }
            set { _lowLevelsOffset = value; }
        }

        public int LowLevelsSequence
        {
            get { return _lowLevelsSequence; }
            set { _lowLevelsSequence = value; }
        }

        // BEGIN Override Low Level Enhancement
        public LowLevelVersionOverrideProfileList LowlevelOverrideList
        {
            get
            {
                if (_lowlevelOverrideList == null)
                {
                    //Begin TT#764 - JSmith - Application "hourglassing" when opening OTS Forecast Methods.
                    //_lowlevelOverrideList = new LowLevelVersionOverrideProfileList(eProfileType.LowLevelVersionOverride);
                    PopulateOverrideList();
                    //End TT#764
                }
                return _lowlevelOverrideList;
            }
            set { _lowlevelOverrideList = value; }
        }
        //End Track #4371
        // END Override Low Level Enhancement

        public ProfileList GLFProfileList
        {
            get { return _GLFProfileList; }
            set { _GLFProfileList = value; }
        }

        public ForecastMonitor ForecastMonitor
        {
            get { return _forecastMonitor; }
            set { _forecastMonitor = value; }
        }

        public bool MONITOR
        {
            get { return _monitor; }
            set { _monitor = value; }
        }
        public string MonitorFilePath
        {
            get { return _monitorFilePath; }
            set { _monitorFilePath = value; }
        }
        public eForecastMonitorMode MonitorMode
        {
            get { return _monitorMode; }
            set { _monitorMode = value; }
        }

        public ApplicationSessionTransaction ApplicationTransaction
        {
            get { return _applicationTransaction; }
            set { _applicationTransaction = value; }
        }

        public ForecastStockMinMax StockMinMax
        {
            get { return _stockMinMax; }
            set { _stockMinMax = value; }
        }

        public WeekProfile FirstWeekOfPlan
        {
            get { return _firstWeekOfPlan; }
            set { _firstWeekOfPlan = value; }
        }

        public PeriodProfile PlanPeriod
        {
            get { return _planPeriod; }
            set { _planPeriod = value; }
        }

        public int ForecastModelRid
        {
            get { return _forecastModelRid; }
            set { _forecastModelRid = value; }
        }

        /// <summary>
        /// Variable currently being processed by processAction.
        /// </summary>
        public ModelVariableProfile CurrentVariable
        {
            get { return _currentVariable; }
            set { _currentVariable = value; }
        }

        /// <summary>
        /// Gets the Sales variable of the current model being processed--could be null if no sales variable was defined.
        /// </summary>
        public ModelVariableProfile SalesVariable
        {
            get { return _salesVariable; }
            //set { _forecastModelRid = value; }
        }

        /// <summary>
        /// Gets the Stock variable of the current model being processed--could be null if no Stock variable was defined.
        /// </summary>
        public ModelVariableProfile StockVariable
        {
            get { return _stockVariable; }
            //set { _forecastModelRid = value; }
        }

        // BEGIN Issue 4962 stodd 11.27.2007 Forecast Stock Only
        /// <summary>
        /// Gets the override variable of the current model being processed--can be null if no override variable was defined.
        /// </summary>
        public ModelVariableProfile OverrideVariable
        {
            get { return _overrideVariable; }
        }

        /// <summary>
        /// Gets the forecasting Model Profile
        /// </summary>
        public ForecastModelProfile ForecastingModel
        {
            get { return _forecastingModel; }
            set { _forecastingModel = value; }
        }
        // END Issue 4962 

        /// <summary>
        /// Gets the ProfileType of this profile.
        /// </summary>
        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.MethodOTSPlan;
            }
        }

        // BEGIN MID Track #4370 - John Smith - FWOS Models
        /// <summary>
        /// Gets the Hashtable of inventory store objects
        /// </summary>
        public Hashtable InventoryForecastSummandHash
        {
            get
            {
                if (_inventoryForecastSummandHash == null)
                {
                    _inventoryForecastSummandHash = new Hashtable();
                }
                return _inventoryForecastSummandHash;
            }
        }
        // END MID Track #4370

        /// <summary>
        /// Gets or sets the location where the min/maxes is inherited from.
        /// </summary>
        public eInheritedFrom MinMaxInheritedFrom
        {
            get { return _minMaxInheritedFrom; }
            set { _minMaxInheritedFrom = value; }
        }

        /// <summary>
        /// Gets or sets the record ID of the node where the min/maxes is inherited from.
        /// </summary>
        public int MinMaxInheritedFromNodeRID
        {
            get { return _minMaxInheritedFromNodeRID; }
            set { _minMaxInheritedFromNodeRID = value; }
        }

        /// <summary>
        /// Gets or sets the flag identifying if the attribute on the method is different.
        /// </summary>
        public bool MinMaxAttributeMismatch
        {
            get { return _minMaxAttributeMismatch; }
            set { _minMaxAttributeMismatch = value; }
        }

        /// <summary>
        /// Gets or Sets the hash table that holds by set whether a stock min/max was defined.
        /// </summary>
        public Hashtable StockMinMaxUsedHash
        {
            get { return _stockMinMaxUsedHash; }
            set { _stockMinMaxUsedHash = value; }
        }

        /// <summary>
        /// Gets or Sets the override low level rid.
        /// </summary>
        public int OverrideLowLevelRid
        {
            get { return _overrideLLRid; }
            set { _overrideLLRid = value; }
        }

        public WeekProfile WeekBeingPlanned
        {
            get { return _weekBeingPlanned; }
            set { _weekBeingPlanned = value; }
        }


        #endregion

        public OTSPlanMethod(SessionAddressBlock SAB, int aMethodRID) : base(SAB,
            //Begin TT#523 - JScott - Duplicate folder when new folder added
            //aMethodRID, eMethodType.OTSPlan)
            aMethodRID, eMethodType.OTSPlan, eProfileType.MethodOTSPlan)
        //End TT#523 - JScott - Duplicate folder when new folder added
        {
            _GLFProfileList = new ProfileList(eProfileType.GroupLevelFunction);
            _SAB = SAB;
            _planFactor = 100.0d;
            //needed during maintenance Update()
            _originalSGRid = this.SG_RID;

            _monitor = _SAB.ClientServerSession.UserOptions.ForecastMonitorIsActive;
            _monitorFilePath = _SAB.ClientServerSession.UserOptions.ForecastMonitorDirectory;
            _monitorMode = _SAB.ClientServerSession.UserOptions.ForecastMonitorMode;

            // BEGIN MID Track #4370 - John Smith - FWOS Models
            string maxAttempts = MIDConfigurationManager.AppSettings["MaximumBalanceAttempts"];
            if (maxAttempts != null)
            {
                try
                {
                    _maxBalanceAttempts = Convert.ToInt32(maxAttempts);
                }
                catch
                {
                }
            }
            // END MID Track #4370
            _stockMinMaxUsedHash = new Hashtable();
            if (base.Filled)
            {
                _OTSPlanData = new OTSPlanMethodData(aMethodRID, eChangeType.populate);
                _Plan_HN_RID = _OTSPlanData.Plan_HN_RID;
                _Orig_Plan_HN_RID = _Plan_HN_RID;
                _Plan_FV_RID = _OTSPlanData.Plan_FV_RID;
                _CDR_RID = _OTSPlanData.CDR_RID;
                _Chain_FV_RID = _OTSPlanData.Chain_FV_RID;
                _Bal_Sales_Ind = Include.ConvertCharToBool(_OTSPlanData.Bal_Sales_Ind);
                _Bal_Stock_Ind = Include.ConvertCharToBool(_OTSPlanData.Bal_Stock_Ind);
                _forecastModelRid = _OTSPlanData.ForecastModelRid;
                _highLevelInd = Include.ConvertCharToBool(_OTSPlanData.High_Level_Ind);
                _lowLevelsInd = Include.ConvertCharToBool(_OTSPlanData.Low_Levels_Ind);
                _lowLevelsType = (eLowLevelsType)_OTSPlanData.Low_Level_Type;
                _lowLevelsSequence = _OTSPlanData.Low_Level_Seq;
                _lowLevelsOffset = _OTSPlanData.Low_Level_Offset;
                _overrideLLRid = _OTSPlanData.OverrideLowLevelRid;
                // BEGIN TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 
                _ApplyTrendOptionsInd = _OTSPlanData.ApplyTrendOptionsInd;
                _ApplyTrendOptionsWOSValue = _OTSPlanData.ApplyTrendOptionsWOSValue;
                // END TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 

                // BEGIN Override Low Level Enhancement
                //Begin TT#764 - JSmith - Application "hourglassing" when opening OTS Forecast Methods.
                //PopulateOverrideList();
                //End TT#764
                // END Override Low Level Enhancement

                if (_OTSPlanData.ForecastModelRid != Include.NoRID)
                {
                    _forecastingModel = new ForecastModelProfile(_OTSPlanData.ForecastModelRid);
                    // BEGIN Issue 5476 - stodd 5.20.2008
                    ApplicationSessionTransaction aApplicationTransaction = new ApplicationSessionTransaction(_SAB);
                    _cubeGroup = (PlanCubeGroup)aApplicationTransaction.GetForecastCubeGroup();
                    // END Issue 5476
                    ProfileList variables = _cubeGroup.Transaction.PlanComputations.PlanVariables.VariableProfileList;
                    // add variable profiles to variables in forcast model
                    foreach (ModelVariableProfile mvp in _forecastingModel.Variables)
                    {
                        VariableProfile vp = (VariableProfile)variables.FindKey(mvp.Key);
                        mvp.VariableProfile = vp;
                    }
                }
                else
                    _forecastingModel = null;

                SetGroupLevelFunctionProfileList();
            }
            else
            //Defaults
            {
                _Plan_HN_RID = Include.NoRID;
                _Plan_FV_RID = Include.NoRID;
                _CDR_RID = Include.NoRID;
                _Chain_FV_RID = Include.NoRID;
                _Bal_Sales_Ind = false;
                _Bal_Stock_Ind = false;
                _forecastModelRid = Include.NoRID;
                _forecastingModel = null;
                _highLevelInd = true;
                _lowLevelsInd = false;
                _overrideLLRid = Include.NoRID;
                // BEGIN TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 
                _ApplyTrendOptionsInd = 'C';
                _ApplyTrendOptionsWOSValue = 0;
                // END TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 

            }
        }

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        override internal bool CheckForUserData()
        {
            if (IsHierarchyNodeUser(Plan_HN_RID))
            {
                return true;
            }

            if (IsStoreGroupUser(SG_RID))
            {
                return true;
            }

            foreach (GroupLevelFunctionProfile GLFProfile in GLFProfileList)
            {
                if (IsHierarchyNodeUser(GLFProfile.Season_HN_RID))
                {
                    return true;
                }

                foreach (GroupLevelBasisProfile groupLevelBasisProfile in GLFProfile.GroupLevelBasis)
                {
                    if (IsHierarchyNodeUser(groupLevelBasisProfile.Basis_HN_RID))
                    {
                        return true;
                    }
                }

                foreach (int Key in GLFProfile.Group_Level_Nodes.Keys)
                {
                    GroupLevelNodeFunction glnf = (GroupLevelNodeFunction)GLFProfile.Group_Level_Nodes[Key];

                    foreach (StockMinMaxProfile smmp in glnf.Stock_MinMax)
                    {
                        if (IsHierarchyNodeUser(glnf.HN_RID))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

        /// <summary>
        /// Populates the class's _lowlevelOverrideList.
        /// </summary>
        public void PopulateOverrideList()
        {
            try
            {
                // BEGIN Override Low Level Enhancement
                HierarchySessionTransaction hTran = new HierarchySessionTransaction(_SAB);
                if (_lowLevelsType == eLowLevelsType.LevelOffset)
                {
                    _lowlevelOverrideList = hTran.GetOverrideList(_overrideLLRid, this._Plan_HN_RID, _Plan_FV_RID,
                                                                               _lowLevelsOffset, Include.NoRID, true, false);
                }
                else if (_lowLevelsType == eLowLevelsType.HierarchyLevel)
                {
                    HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(Plan_HN_RID);

                    // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
                    //int offset = _lowLevelsSequence - hnp.NodeLevel;
                    //_lowlevelOverrideList = hTran.GetOverrideList(_overrideLLRid, _Plan_HN_RID, _Plan_FV_RID,
                    //                                                           offset, Include.NoRID, true, false);
                    _lowlevelOverrideList = hTran.GetOverrideList(_overrideLLRid, _Plan_HN_RID, _Plan_FV_RID,
                                                                   eHierarchyDescendantType.levelType, _lowLevelsSequence, Include.NoRID, true, false);
                    // END Track #6107
                }
                // END Override Low Level Enhancement
                //Begin TT#764 - JSmith - Application "hourglassing" when opening OTS Forecast Methods.
                else
                {
                    _lowlevelOverrideList = new LowLevelVersionOverrideProfileList(eProfileType.LowLevelVersionOverride);
                }
                //End TT#764

                //        HierarchyNodeList hnl = null;
                //        if (this.LowLevelsType == eLowLevelsType.LevelOffset)
                //        {
                //            //Begin Track #4037 - JSmith - Optionally include dummy color in child list
                //            //Begin Track #5378 - color and size not qualified
                //            //					hnl = _SAB.HierarchyServerSession.GetDescendantData(this.Plan_HN_RID, this.LowLevelsOffset, true, eNodeSelectType.NoVirtual);
                //            hnl = _SAB.HierarchyServerSession.GetDescendantData(this.Plan_HN_RID, this.LowLevelsOffset, true, eNodeSelectType.NoVirtual, true);
                //            //End Track #5378
                //            //End Track #4037
                //        }
                //        else
                //        {
                //            //Begin Track #4037 - JSmith - Optionally include dummy color in child list
                //            //Begin Track #5378 - color and size not qualified
                //            //					hnl = _SAB.HierarchyServerSession.GetDescendantDataByLevel(this.Plan_HN_RID, this.LowLevelsSequence, true, eNodeSelectType.NoVirtual);
                //            hnl = _SAB.HierarchyServerSession.GetDescendantDataByLevel(this.Plan_HN_RID, this.LowLevelsSequence, true, eNodeSelectType.NoVirtual, true);
                //            //End Track #5378
                //            //End Track #4037
                //        }

                //        // BEGIN Override Low Level Enhancement
                //        LowlevelExcludeList.Clear();
                //        foreach (HierarchyNodeProfile hnp in hnl)
                //        {
                //            LowLevelVersionOverrideProfile llvop = new LowLevelVersionOverrideProfile(hnp.Key, hnp, false,
                //                new VersionProfile(this._Plan_FV_RID), false);
                //            this.LowlevelExcludeList.Add(llvop);
                //        }
                //        // END Override Low Level Enhancement

                //        if (aMethodRID != Include.NoRID)
                //        {
                //            _OTSPlanData.LowlevelExcludeList = this.LowlevelExcludeList;
                //            //_OTSPlanData.PopulateExcludes(aMethodRID);
                //        }
            }
            catch
            {
                throw;
            }
        }

        public int GetDefaultGLFRid()
        {
            int defaultRid = Include.NoRID;
            foreach (GroupLevelFunctionProfile glfp in this._GLFProfileList.ArrayList)
            {
                if (glfp.Default_IND == true)
                {
                    defaultRid = glfp.Key;
                    break;
                }
            }

            return defaultRid;
        }

        // begin MID Track 2375 

        /// <summary>
        /// This method is called when the default indicator on a set is set from true to false.
        /// Since there is no longer a default, all sets that had Use Default set to true must be set to false also.
        /// </summary>
        public void SetAllUseDefaultToFalse()
        {
            foreach (GroupLevelFunctionProfile glfp in this._GLFProfileList.ArrayList)
            {
                glfp.Use_Default_IND = false;
            }
        }
        // end MID Track 2375 


        public override void ProcessMethod(
            ApplicationSessionTransaction aApplicationTransaction,
            int aStoreFilter, Profile methodProfile)
        {
            MIDTimer processTimer = new MIDTimer(); // TT#241 - stodd
            processTimer.Start(); // TT#241 - stodd
                                  //Begin TT#1517 - DOConnell - Recieve Null Ref error when processing forecast with low levels selected
            this._applicationTransaction = aApplicationTransaction;
            //End TT#1517 - DOConnell - Recieve Null Ref error when processing forecast with low levels selected
            // Begin MID Track #5210 - JSmith - Out of memory
            try
            {
                // Begin TT#241 - stodd - add node messages
                _infoMsg = "OTS Forecast Started: " + this.Name;
                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, _sourceModule);
                // End TT#241 - stodd - add node messages

                // End MID Track #5210
                ArrayList forecastingOverrideList = aApplicationTransaction.ForecastingOverrideList;

                // Begin Issue 4558 stodd 08.28.07
                //==============================================================================
                // This gets rid of any "empty" overrides that do not effect the  OTS Forecast
                //==============================================================================
                ArrayList tempList = new ArrayList();
                if (forecastingOverrideList != null && forecastingOverrideList.Count > 0)  // Issue 4558
                {
                    foreach (ForecastingOverride fo in forecastingOverrideList)
                    {
                        if (fo.HasOverrides())
                            tempList.Add(fo);
                    }
                    forecastingOverrideList = tempList;
                }
                // End Issue 4558

                if (LowLevelsInd)
                    GetLowLevels(ref forecastingOverrideList);  // Issue 4558
                                                                // BEGIN Issue 5068
                else if (HighLevelInd)
                    GetHighLevel(ref forecastingOverrideList);

                BlendForecastOverrides(forecastingOverrideList);
                //END Issue 5068

                // BEGIN Override Low LEvel Enhancement
                //MergeGroupLevelFunctionGroupLevelNodesList();
                // END Override Low LEvel Enhancement

                if (forecastingOverrideList != null && forecastingOverrideList.Count > 0)  // Issue 4558
                {
                    foreach (ForecastingOverride fo in forecastingOverrideList)
                    {
                        if (fo.HierarchyNodeRid != Include.NoRID)
                        {
                            this.Plan_HN_RID = fo.HierarchyNodeRid;
                            this._nodeOverride = fo.HierarchyNodeRid;
                        }
                        if (fo.ForecastVersionRid != Include.NoRID)
                        {
                            this.Plan_FV_RID = fo.ForecastVersionRid;
                            this.Chain_FV_RID = fo.ForecastVersionRid;
                            this._versionOverride = fo.ForecastVersionRid;
                        }

                        if (Plan_HN_RID == Include.NoRID)
                        {
                            _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_pl_PlanHierarchyNodeMissing, _sourceModule);
                            // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                            //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                            // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                            throw new MIDException(eErrorLevel.severe,
                                (int)eMIDTextCode.msg_pl_PlanHierarchyNodeMissing,
                                MIDText.GetText(eMIDTextCode.msg_pl_PlanHierarchyNodeMissing));
                        }
                        if (Plan_FV_RID == Include.NoRID)
                        {
                            _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_pl_PlanVersionMissing, _sourceModule);
                            // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                            //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                            // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                            throw new MIDException(eErrorLevel.severe,
                                (int)eMIDTextCode.msg_pl_PlanVersionMissing,
                                MIDText.GetText(eMIDTextCode.msg_pl_PlanVersionMissing));
                        }
                        if (Chain_FV_RID == Include.NoRID)
                        {
                            _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_pl_ChainVersionMissing, _sourceModule);
                            // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                            //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                            // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                            throw new MIDException(eErrorLevel.severe,
                                (int)eMIDTextCode.msg_pl_ChainVersionMissing,
                                MIDText.GetText(eMIDTextCode.msg_pl_ChainVersionMissing));
                        }

                        // BEGIN Issue 4962 stodd 11.27.2007
                        //_hnp = _SAB.HierarchyServerSession.GetNodeData(this.Plan_HN_RID); //TT#339 - MD - Modify Forecast audit message - RBeck
                        // Begin TT#737-MD - JSmith - Store Forecasting-> sales are not being forecasted and log data does not match OTS forecast review
                        //_hnp = _SAB.HierarchyServerSession.GetNodeData(this.Plan_HN_RID, false, true);
                        _hnp = _SAB.HierarchyServerSession.GetNodeData(this.Plan_HN_RID, true, true);
                        // End TT#737-MD - JSmith - Store Forecasting-> sales are not being forecasted and log data does not match OTS forecast review
                        // Begin Track #6137 stodd
                        if (fo.VariableNumber != 0 && fo.VariableNumber != Include.NoRID)
                        // End Track # 6137
                        {
                            // BEGIN TT#790 - stodd - forecasting null reference
                            _cubeGroup = (PlanCubeGroup)aApplicationTransaction.GetForecastCubeGroup();
                            // END TT#790
                            ProfileList variables = _cubeGroup.Transaction.PlanComputations.PlanVariables.VariableProfileList;
                            VariableProfile vp = (VariableProfile)variables.FindKey(fo.VariableNumber);
                            _overrideVariable = new ModelVariableProfile(vp);
                            //====================================================================================
                            // Overrides the Forecasting model to only include the override variable.
                            // If no Forecasting Model exists, it creates one using only the override variable.
                            //====================================================================================
                            OverrideForecastingModel(aApplicationTransaction, fo);
                        }
                        // END Issue 4962

                        this.ProcessAction(
                            aApplicationTransaction.SAB,
                            aApplicationTransaction,
                            null,
                            methodProfile,
                            true,
                            //Begin Track #5188 - JScott - Method Filters not being over overriden from Workflow
                            aStoreFilter);
                        //End Track #5188 - JScott - Method Filters not being over overriden from Workflow
                    }
                }
                else
                {
                    //==============================================================================
                    // This code is only processed for the High Level node. But we only want to
                    // process it if the HighLevelInd is true.
                    //==============================================================================
                    if (HighLevelInd)
                    {
                        // BEGIN Issue 4962 stodd 11.27.2007
                        //_hnp = _SAB.HierarchyServerSession.GetNodeData(this.Plan_HN_RID); //TT#339 - MD - Modify Forecast audit message - RBeck
                        // Begin TT#737-MD - JSmith - Store Forecasting-> sales are not being forecasted and log data does not match OTS forecast review
                        //_hnp = _SAB.HierarchyServerSession.GetNodeData(this.Plan_HN_RID, false, true);
                        _hnp = _SAB.HierarchyServerSession.GetNodeData(this.Plan_HN_RID, true, true);
                        // End TT#737-MD - JSmith - Store Forecasting-> sales are not being forecasted and log data does not match OTS forecast review
                        // END Issue 4962 stodd 11.27.2007
                        this.ProcessAction(
                            aApplicationTransaction.SAB,
                            aApplicationTransaction,
                            null,
                            methodProfile,
                            true,
                            //Begin Track #5188 - JScott - Method Filters not being over overriden from Workflow
                            aStoreFilter);
                        //End Track #5188 - JScott - Method Filters not being over overriden from Workflow
                    }
                    else
                    {
                        string msg = MIDText.GetText(eMIDTextCode.msg_pl_NoNodeSelectedForMethod);
                        msg = msg.Replace("{0}", this.Name);
                        _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msg, _sourceModule);
                        aApplicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.NoActionPerformed;

                    }
                }
                // BEGIN TT#241 - stodd - add node messaging
                processTimer.Stop();
                _infoMsg = "OTS Forecast Completed: " + this.Name + "  Run Time: " + processTimer.ElaspedTimeString;
                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, _sourceModule);
                // BEGIN TT#3166 - stodd -error running forecast
                //_applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionCompletedSuccessfully;
                // END TT#3166 - stodd -error running forecast
                // End TT#241 - stodd - add node messaging
                // Begin MID Track #5210 - JSmith - Out of memory
            }
            catch
            {
                throw;
            }
            finally
            {
                _OTSPlanData = null;
                _GLFProfileList = null;
                _forecastingModel = null;
                _lowlevelOverrideList = null;
                _cubeGroup = null;
                _openParms = null;
                _currentVariable = null;
                _salesVariable = null;
                _stockVariable = null;
                _overrideVariable = null;
                _storeGroupLevelHash = null;
                _allStoreList = null;
                _weeksToPlan = null;
                _dtGroupLevelBasis = null;
                _forecastMonitor = null;
                _stockMinMax = null;
                _hnp = null;
                _firstWeekOfPlan = null;
                _firstWeekOfBasis = null;
                _planPeriod = null;
                _setLists = null;
                _stockMinMaxUsedHash = null;
                _inventoryForecastSummandHash = null;
            }
            // End MID Track #5210
        }

        private void GetLowLevels(ref ArrayList forecastingOverrideList) // Begin Issue 4558 stodd 08.28.07
        {
            // Begin Issue 4435 stodd - processing low levels in from workflow
            //===============================================================================================
            // The Workflow always send a ForecastingOVerride object, even if for all practical purposes
            // it is empty of data--hierarchy node is -1. This checks for this and skips the logic, otherwise it
            // abends looking for the relationship of -1 in the hierarchy.
            //===============================================================================================
            bool listIsEmpty = true;

            // BEGIN Issue 4441 - stodd 06.08.2007
            if (forecastingOverrideList != null)
            {
                if (forecastingOverrideList.Count > 1)
                {
                    listIsEmpty = false;
                }
                else if (forecastingOverrideList.Count == 1)
                {
                    ForecastingOverride fo = (ForecastingOverride)forecastingOverrideList[0];
                    if (fo.HierarchyNodeRid == Include.NoRID)
                        listIsEmpty = true;
                    else
                        listIsEmpty = false;
                }
            }
            // END Issue 4441 - stodd 06.08.2007


            if (forecastingOverrideList == null || listIsEmpty)
            // End Issue 4435 
            {
                if (HighLevelInd)
                {
                    // Begin Issue 4558
                    ForecastingOverride fo = new ForecastingOverride(this.Plan_HN_RID, Include.NoRID);
                    if (forecastingOverrideList == null)
                        forecastingOverrideList = new ArrayList();
                    forecastingOverrideList.Add(fo);
                    // End Issue 4558
                }

                foreach (LowLevelVersionOverrideProfile profile in LowlevelOverrideList)
                {
                    if (!profile.Exclude)
                    {
                        // Begin Issue 4558
                        ForecastingOverride fo = new ForecastingOverride(profile.Key, Include.NoRID);
                        if (forecastingOverrideList == null)
                            forecastingOverrideList = new ArrayList();
                        forecastingOverrideList.Add(fo);
                        // End Issue 4558
                    }
                }
            }
            else
            {
                int loopMax = forecastingOverrideList.Count;

                for (int i = 0; i < loopMax; i++)
                {
                    ForecastingOverride fo = (ForecastingOverride)forecastingOverrideList[i];

                    HierarchyNodeList hnl = null;
                    if (this.LowLevelsType == eLowLevelsType.LevelOffset)
                        //Begin Track #4037 - JSmith - Optionally include dummy color in child list
                        hnl = _SAB.HierarchyServerSession.GetDescendantData(fo.HierarchyNodeRid, this.LowLevelsOffset, true, eNodeSelectType.NoVirtual);
                    //End Track #4037
                    else
                        //Begin Track #4037 - JSmith - Optionally include dummy color in child list
                        hnl = _SAB.HierarchyServerSession.GetDescendantDataByLevel(fo.HierarchyNodeRid, this.LowLevelsSequence, true, eNodeSelectType.NoVirtual);
                    //End Track #4037

                    // Begin Issue 4558
                    foreach (HierarchyNodeProfile hnp in hnl)
                    {
                        ForecastingOverride newFo = new ForecastingOverride(hnp.Key, Include.NoRID);
                        forecastingOverrideList.Add(newFo);
                    }
                    // End Issue 4558
                }
            }
        }

        // BEGIN Issue 5058 stodd 01.02.2008
        /// <summary>
        /// Overrides, such as variable overrides affect the entire list of nodes being process. This method blends
        /// the override with each of the node overrides and then removes the blended override.
        /// </summary>
        /// <param name="forecastingOverrideList"></param>
        private void BlendForecastOverrides(ArrayList forecastingOverrideList)
        {
            ArrayList blendedOverrides = new ArrayList();
            try
            {
                if (forecastingOverrideList != null)
                {
                    int loopMax = forecastingOverrideList.Count;
                    for (int i = 0; i < loopMax; i++)
                    {
                        ForecastingOverride fo = (ForecastingOverride)forecastingOverrideList[i];
                        if (fo.HierarchyNodeRid == Include.NoRID)
                        {
                            blendedOverrides.Add(fo);
                            foreach (ForecastingOverride blendFo in forecastingOverrideList)
                            {
                                blendFo.Balance = fo.Balance;
                                if (fo.ComputationMode != null && fo.ComputationMode != string.Empty)
                                    blendFo.ComputationMode = fo.ComputationMode;
                                if (fo.ForecastVersionRid != Include.NoRID)
                                    blendFo.ForecastVersionRid = fo.ForecastVersionRid;
                                if (fo.VariableNumber != Include.NoRID)
                                    blendFo.VariableNumber = fo.VariableNumber;
                            }
                        }
                    }

                    // Remove blended overrides from main list
                    foreach (ForecastingOverride blendFo in blendedOverrides)
                    {
                        forecastingOverrideList.Remove(blendFo);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void GetHighLevel(ref ArrayList forecastingOverrideList)
        {
            //===============================================================================================
            // The Workflow always send a ForecastingOVerride object, even if for all practical purposes
            // it is empty of data--hierarchy node is -1. This checks for this and skips the logic, otherwise it
            // abends looking for the relationship of -1 in the hierarchy.
            //===============================================================================================
            bool listIsEmpty = true;

            if (forecastingOverrideList != null)
            {
                if (forecastingOverrideList.Count > 1)
                {
                    listIsEmpty = false;
                }
                else if (forecastingOverrideList.Count == 1)
                {
                    ForecastingOverride fo = (ForecastingOverride)forecastingOverrideList[0];
                    if (fo.HierarchyNodeRid == Include.NoRID)
                        listIsEmpty = true;
                    else
                        listIsEmpty = false;
                }
            }

            if (forecastingOverrideList == null || listIsEmpty)
            {
                if (HighLevelInd)
                {
                    // Begin Issue 4558
                    ForecastingOverride fo = new ForecastingOverride(this.Plan_HN_RID, Include.NoRID);
                    if (forecastingOverrideList == null)
                        forecastingOverrideList = new ArrayList();
                    forecastingOverrideList.Add(fo);
                    // End Issue 4558
                }
            }
        }
        // END Issue 5068 

        /// <summary>
        /// Overrides the Forecasting Model to only include the override variable.
        /// If no Forecasting Model exists, it creates one using only the override variable.
        /// </summary>
        private void OverrideForecastingModel(ApplicationSessionTransaction trans, ForecastingOverride fo)
        {
            if (_forecastingModel == null)
            {
                _forecastingModel = new ForecastModelProfile(Include.NoRID);
                _forecastingModel.ComputationMode = fo.ComputationMode;
                SetDefaultSalesStockVariables(trans);
                _forecastingModel.Variables.Add(_salesVariable);
                _forecastingModel.Variables.Add(_stockVariable);
            }
            else
            {
                _forecastingModel.ModelID = "Variable Override";
                _forecastingModel.ComputationMode = fo.ComputationMode;
            }

            //=========================================================
            // Set the override variable to be process to IsSelected.
            //=========================================================
            foreach (ModelVariableProfile mvp in _forecastingModel.Variables)
            {
                if (mvp.VariableProfile.Key == OverrideVariable.VariableProfile.Key)
                    mvp.IsSelected = true;
                else
                    mvp.IsSelected = false;
            }
        }

        public override void ProcessAction(SessionAddressBlock aSAB, ApplicationSessionTransaction aApplicationTransaction, ApplicationWorkFlowStep aWorkFlowStep, Profile aProfile, bool WriteToDB, int aStoreFilterRID)
        {
            try
            {
                MIDTimer recomputeTimer = new MIDTimer();
                MIDTimer processTimer = new MIDTimer(); // TT#241 - stodd
                processTimer.Start(); // TT#241 - stodd

                // Begin MID Track #5210 - JSmith - Out of memory
                _cubeGroup = (PlanCubeGroup)aApplicationTransaction.GetForecastCubeGroup();
                // End MID Track #5210
                _applicationTransaction = aApplicationTransaction;
                _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.NoActionPerformed;

                // Begin MID Track 4858 - JSmith - Security changes
                // Need to check here incase part of workflow
                if (!AuthorizedToUpdate(_SAB.ApplicationServerSession, _SAB.ClientServerSession.UserRID))
                {
                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_NotAuthorizedForNode, this.ToString());
                    // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                    //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                    // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                    _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
                    return;
                }
                // End MID Track 4858

                // Init these for later compares
                _currentVariable = new ModelVariableProfile(Include.NoRID);
                _salesVariable = new ModelVariableProfile(Include.NoRID);
                _stockVariable = new ModelVariableProfile(Include.NoRID);
                // Begin Issue 3752 - stodd 
                _reserveStoreRid = _SAB.ApplicationServerSession.GlobalOptions.ReserveStoreRID;
                // End Issue 3752 - stodd 			
                try
                {
                    // Begin TT#241 - stodd - add messaging for each node processed
                    ForecastVersion fv = new ForecastVersion();

                    //Begin TT#339 - MD - Modify Forecast audit message - RBeck 
                    //_infoMsg = "Processing Hierarchy Node ID: " +
                    //            _hnp.NodeID + " DESC: " +
                    //            _hnp.NodeDescription + ". Version: " + fv.GetVersionText(Plan_FV_RID) + ".";
                    _infoMsg = "Processing Hierarchy Node: " +
                            _hnp.Text + " - Version: " + fv.GetVersionText(Plan_FV_RID) + ".";
                    //End TT#339 - MD - Modify Forecast audit message - RBeck

                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, _sourceModule);
                    // End TT#241 - stodd - add messaging for each node processed

                    _infoMsg = "Forecasting Sales...";
                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, _sourceModule);

                    bool foundSomethingToPlan = false;

                    // get the plan info once for the data cube.
                    FillOpenParmForPlan();
                    // BEGIN Issue 4818
                    // get all of the basis info into a datatable
                    _dtGroupLevelBasis = GetGroupLevelBasis(this.Key);  //Issue 4818
                                                                        // some processing needs them split up
                                                                        // END Issue 4818
                                                                        // get store to group level cross reference (hash).
                    _storeGroupLevelHash = StoreMgmt.GetStoreGroupLevelHashTable(this.SG_RID); //_SAB.StoreServerSession.GetStoreGroupLevelHashTable(this.SG_RID);
                                                                                               // construct Stock Min Max class
                                                                                               // Begin TT#5028 - JSmith - Node Property - Sku Level Min/Max setup
                                                                                               //_stockMinMax = new ForecastStockMinMax(_SAB.ApplicationServerSession.Calendar, this.Key);
                    _stockMinMax = new ForecastStockMinMax(_SAB.ApplicationServerSession.Calendar, this.Key, this);
                    // End TT#5028 - JSmith - Node Property - Sku Level Min/Max setup
                    // get all stores (and remove inactive stores from the list)		
                    ProfileList tempStoreList = StoreMgmt.StoreProfiles_GetActiveStoresList(); //_SAB.StoreServerSession.GetActiveStoresList();
                    _allStoreList = new ProfileList(eProfileType.Store);
                    foreach (StoreProfile sp in tempStoreList.ArrayList)
                    {
                        if (sp.ActiveInd)
                        {
                            _allStoreList.Add(sp);
                        }
                    }

                    // get weeks to plan
                    _weeksToPlan = GetWeeksToPlan(this.CDR_RID);

                    // BEGIN TT#3166 - stodd - error while running forecasting
                    if (_weeksToPlan.Count == 0)
                    {
                        string msg = MIDText.GetText(eMIDTextCode.msg_NoWeeksToPlan);
                        msg = msg.Replace("{0}", this.Name);
                        _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msg, _sourceModule);
                        throw new EndProcessingException(msg);
                    }
                    // END TT#3166 - stodd - error while running forecasting

                    // BEGIN MID Track #6043 - KJohnson - Forcast Chain WOS Calculations
                    _numberOfWeeksWithZeroSales = _SAB.ApplicationServerSession.GlobalOptions.NumberOfWeeksWithZeroSales;
                    _maximumChainWOS = _SAB.ApplicationServerSession.GlobalOptions.MaximumChainWOS;
                    _prorateChainStock = _SAB.ApplicationServerSession.GlobalOptions.ProrateChainStock;
                    // END MID Track #6043 - KJohnson

                    if (MONITOR)
                    {
                        try
                        {

                            //Begin TT#339 - MD - Duplicate colors from previous hierarchy are over written - RBeck
                            string qualifiedNodeID = _hnp.QualifiedNodeID;
                            string fileName = "Forecast_";
                            //  string fileName = "Forecast" + this.Key.ToString(CultureInfo.CurrentUICulture) + "_" +
                            //                    _hnp.HomeHierarchyParentRID  + "_" + _hnp.NodeID;      
                            //  _forecastMonitor = new ForecastMonitor(this, _SAB, fileName, _monitorFilePath, _SAB.ClientServerSession.UserRID, this.Name, this.Key);
                            //End   TT#339 - MD - Duplicate colors from previous hierarchy are over written - RBeck

                            _forecastMonitor = new ForecastMonitor(this, _SAB, fileName, _monitorFilePath, _SAB.ClientServerSession.UserRID, this.Name, qualifiedNodeID);
                            //StoreGroupProfile sgp = StoreMgmt.GetStoreGroup(SG_RID); //_SAB.StoreServerSession.GetStoreGroup(SG_RID);
                            string groupName = StoreMgmt.StoreGroup_GetName(SG_RID); //TT#1517-MD -jsobek -Store Service Optimization
                            _forecastMonitor.GroupSets = StoreMgmt.StoreGroup_GetLevelListFilled(SG_RID); //_SAB.StoreServerSession.GetStoreGroupLevelList(SG_RID);
                            _forecastMonitor.WriteLine("Hierarchy Node: " +
                                //_hnp.NodeID + "   DESC: " + //TT#339 - MD - Duplicate colors from previous hierarchy are over written - RBeck
                                _hnp.Text + "(" +
                                _hnp.Key.ToString(CultureInfo.CurrentUICulture) + ")");
                            // BEGIN stodd 09.24.2007
                            _forecastMonitor.WriteLine("Process High Level? " + _highLevelInd.ToString(CultureInfo.CurrentUICulture) +
                                "    Process Low Levels? " + _lowLevelsInd.ToString(CultureInfo.CurrentUICulture));
                            if (_lowLevelsInd)
                            {
                                //Begin TT#764 - JSmith - Application "hourglassing" when opening OTS Forecast Methods.
                                //foreach (LowLevelVersionOverrideProfile llep in _lowlevelOverrideList.ArrayList)
                                foreach (LowLevelVersionOverrideProfile llep in LowlevelOverrideList.ArrayList)
                                //End TT#764
                                {
                                    if (llep.Exclude)
                                    {
                                        _forecastMonitor.WriteLine("  Excluded Low Level: " + llep.NodeProfile.NodeID + "   DESC: " +
                                            llep.NodeProfile.NodeDescription + "(" +
                                            llep.NodeProfile.Key.ToString(CultureInfo.CurrentUICulture) + ")");
                                    }
                                    else
                                    {
                                        _forecastMonitor.WriteLine("  Processed Low Level: " + llep.NodeProfile.NodeID + "   DESC: " +
                                            llep.NodeProfile.NodeDescription + "(" +
                                            llep.NodeProfile.Key.ToString(CultureInfo.CurrentUICulture) + ")");
                                    }
                                }
                            }
                            // END stodd 09.24.2007
                            //ForecastVersion fv = new ForecastVersion();	removed for TT#241 stodd
                            _forecastMonitor.WriteLine("Forecast Version: " + fv.GetVersionText(Plan_FV_RID) + "(" +
                                Plan_FV_RID.ToString(CultureInfo.CurrentUICulture) + ")");
                            DateRangeProfile drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(_CDR_RID);
                            string displaydate = _SAB.ApplicationServerSession.Calendar.GetDisplayDate(drp);
                            _forecastMonitor.WriteLine("Forecast Dates:   " + displaydate + "(" +
                                _CDR_RID.ToString(CultureInfo.CurrentUICulture) + ")");
                            _forecastMonitor.WriteLine("Attribute:        " + groupName + "(" +
                                SG_RID.ToString(CultureInfo.CurrentUICulture) + ")");
                            _forecastMonitor.WriteLine("Balance Sales? " + _Bal_Sales_Ind.ToString(CultureInfo.CurrentUICulture) +
                                "    Balance Inventory? " + _Bal_Stock_Ind.ToString(CultureInfo.CurrentUICulture));
                            // BEGIN MID Track #6043 - KJohnson - Forcast Chain WOS Calculations
                            _forecastMonitor.WriteLine("  Number Of Weeks With Zero Sales:     " + _numberOfWeeksWithZeroSales.ToString(CultureInfo.CurrentUICulture));
                            _forecastMonitor.WriteLine("  Maximum Chain WOS:     " + _maximumChainWOS.ToString(CultureInfo.CurrentUICulture));
                            _forecastMonitor.WriteLine("  Prorate Chain Stock:     " + _prorateChainStock.ToString(CultureInfo.CurrentUICulture));
                            // BEGIN TT#619 - STodd - OTS Forecast - Chain Plan not required (#46)
                            switch (_ApplyTrendOptionsInd)
                            {
                                case 'C':
                                    _forecastMonitor.WriteLine("WOS Trend Option:   Chain Plans (default)");
                                    break;
                                case 'W':
                                    _forecastMonitor.WriteLine("WOS Trend Option:   Chain WOS");
                                    break;
                                case 'S':
                                    _forecastMonitor.WriteLine("WOS Trend Option:   Plug Chain WOS. Plug Value = " + _ApplyTrendOptionsWOSValue.ToString());
                                    break;
                                default:
                                    break;
                            }
                            // END TT#619 - STodd - OTS Forecast - Chain Plan not required (#46)

                            // END MID Track #6043 - KJohnson
                            // Begin Track #5773 stodd
                            if (_forecastingModel != null)
                                _forecastMonitor.WriteLine("Forecast Model: " + _forecastingModel.ModelID);
                            _forecastMonitor.WriteLine("");
                            // End Track #5773 stodd
                            // BEGIN stodd 09.24.2007
                            // May be needed in the future...
                            //							foreach(GroupLevelFunctionProfile glfp in this._GLFProfileList.ArrayList)
                            //							{
                            //								foreach(int Key in glfp.Group_Level_Nodes.Keys)
                            //								{
                            //									GroupLevelNodeFunction glnf = (GroupLevelNodeFunction)glfp.Group_Level_Nodes[Key];
                            //
                            //									_forecastMonitor.WriteLine(glnf.SglRID.ToString() + " " +
                            //											glnf.HN_RID + " " + glnf.MinMaxInheritType.ToString());
                            //
                            //									foreach(StockMinMaxProfile smmp in glnf.Stock_MinMax)
                            //									{
                            //										_forecastMonitor.WriteLine("  " + smmp.Boundary.ToString() + " " + smmp.DateRangeRid.ToString() + " " +
                            //											smmp.MinimumStock.ToString() + " " + smmp.MaximumStock.ToString());						
                            //									}
                            //									
                            //								}
                            //							}
                            // END stodd 09.24.2007
                        }
                        catch (Exception ex)
                        {
                            // BEGIN Issue 4962
                            _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, ex.ToString(), _sourceModule);
                            // END Issue 4962
                            MONITOR = false;
                            string errMsg = String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_pl_ForecastMonitoringProblem));

                            System.Windows.Forms.DialogResult diagResult = SAB.MessageCallback.HandleMessage(
                                errMsg,
                                "Problem Creating Forecast Monitor Log File",
                                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        }
                    }

                    _firstSetToPlan = true; // init this
                                            // BEGIN MID Track #4370 - John Smith - FWOS Models
                    bool processOneWeekAtATime = false;
                    // determine if plan is used as basis
                    // if so, process one week at a time
                    foreach (GroupLevelFunctionProfile glfp in this._GLFProfileList.ArrayList)
                    {
                        if (processOneWeekAtATime)
                        {
                            break;
                        }
                        else
                        {
                            foreach (GroupLevelBasisProfile glbp in glfp.GroupLevelBasis)
                            {
                                if (!glbp.Basis_ExcludeInd &&
                                    glbp.Basis_FV_RID == Plan_FV_RID &&
                                    glbp.Basis_HN_RID == Plan_HN_RID)
                                {
                                    processOneWeekAtATime = true;
                                    break;
                                }
                            }
                        }
                    }

                    bool moreWeeks = true;
                    int currentWeekIndex = 0;
                    ArrayList weeksToPlan = new ArrayList();
                    if (!processOneWeekAtATime)
                    {
                        foreach (WeekProfile planWeek in _weeksToPlan.ArrayList)
                        {
                            weeksToPlan.Add(planWeek);
                        }
                    }
                    else
                    {
                        weeksToPlan.Add(_weeksToPlan.ArrayList[currentWeekIndex]);
                    }

                    //while (moreWeeks)
                    //{
                    // END MID Track #4370
                    if (_forecastingModel != null)
                    {
                        _openParms.ComputationsMode = _forecastingModel.ComputationMode.Trim();
                        foreach (ModelVariableProfile aVariable in _forecastingModel.Variables)
                        {
                            // BEGIN MID Track #4370 - John Smith - FWOS Models
                            InventoryForecastSummandHash.Clear();
                            // END MID Track #4370
                            _currentVariable = aVariable;
                            //if (aVariable.Key == _forecastingModel.SalesVariable)
                            if (aVariable.ForecastFormula == (int)eForecastFormulaType.Sales)   // MID Track #5773 - KJohnson - Planned FWOS Enhancement
                            {
                                _salesVariable = aVariable;

                                if (aVariable.IsSelected)   // Issue 4962
                                {
                                    while (moreWeeks)
                                    {
                                        // BEGIN MID Track #4370 - John Smith - FWOS Models
                                        foundSomethingToPlan = ProcessSales(aVariable, weeksToPlan);
                                        //								foundSomethingToPlan = ProcessSales(aVariable);
                                        // END MID Track #4370
                                        if (this.Bal_Sales_Ind)
                                        {
                                            // BEGIN MID Track #4370 - John Smith - FWOS Models
                                            BalanceSalesMain(aVariable, weeksToPlan);
                                            //									BalanceSalesMain(aVariable);
                                            // END MID Track #4370
                                        }

                                        recomputeTimer.Start();
                                        _cubeGroup.RecomputeCubes(true);
                                        recomputeTimer.Stop();
                                        if (weeksToPlan.Count > 0)
                                        {
                                            recomputeTimer.Write("Variable: " + aVariable.VariableProfile.VariableName + " Recompute Plan Cubes for " +
                                                ((WeekProfile)weeksToPlan[0]).Text() + " thru " + ((WeekProfile)weeksToPlan[weeksToPlan.Count - 1]).Text());
                                        }

                                        Save();

                                        if (processOneWeekAtATime)
                                        {
                                            weeksToPlan.Clear();
                                            ++currentWeekIndex;
                                            if (currentWeekIndex < _weeksToPlan.ArrayList.Count)
                                            {
                                                weeksToPlan.Add(_weeksToPlan.ArrayList[currentWeekIndex]);
                                            }
                                            else
                                            {
                                                moreWeeks = false;
                                            }
                                        }
                                        else
                                        {
                                            moreWeeks = false;
                                        }
                                    }
                                }
                            }
                            //else if (aVariable.Key == _forecastingModel.StockVariable)   
                            else if (aVariable.ForecastFormula == (int)eForecastFormulaType.Stock)   // MID Track #5773 - KJohnson - Planned FWOS Enhancement   
                            {
                                _stockVariable = aVariable;

                                // BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement
                                //---Set The Associated Sales Valiable-------------------
                                if (aVariable.AssocVariable != Include.NoRID)
                                {
                                    ProfileList variables = _cubeGroup.Transaction.PlanComputations.PlanVariables.VariableProfileList;
                                    //ProfileList variables = _SAB.ApplicationServerSession.Variables.VariableProfileList;
                                    VariableProfile vp = (VariableProfile)variables.FindKey(aVariable.AssocVariable);
                                    _salesVariable = new ModelVariableProfile(vp);
                                }
                                // END MID Track #5773

                                if (aVariable.IsSelected)   // Issue 4962
                                {
                                    // BEGIN MID Track #4370 - John Smith - FWOS Models
                                    ProcessInventory(aVariable, _weeksToPlan.ArrayList);
                                    //								ProcessInventory(aVariable);
                                    // END MID Track #4370
                                    if (this.Bal_Stock_Ind)
                                    {
                                        // BEGIN MID Track #4370 - John Smith - FWOS Models
                                        BalanceInventoryMain(aVariable, _weeksToPlan.ArrayList);
                                        //									BalanceInventoryMain(aVariable);
                                        // END MID Track #4370
                                    }
                                    else if (_prorateChainStock)
                                    {
                                        // BEGIN MID Track #6043 - KJohnson - Forcast Chain WOS Calculations
                                        BalanceInventoryMain(aVariable, _weeksToPlan.ArrayList);
                                        // END MID Track #6043 - KJohnson
                                    }

                                    recomputeTimer.Start();
                                    _cubeGroup.RecomputeCubes(true);
                                    recomputeTimer.Stop();
                                    if (weeksToPlan.Count > 0)
                                    {
                                        recomputeTimer.Write("Variable: " + aVariable.VariableProfile.VariableName + " Recompute Plan Cubes for " +
                                            ((WeekProfile)weeksToPlan[0]).Text() + " thru " + ((WeekProfile)weeksToPlan[weeksToPlan.Count - 1]).Text());
                                    }
                                }
                            }
                            //else
                            else if (aVariable.ForecastFormula == (int)eForecastFormulaType.PctContribution)   // MID Track #5773 - KJohnson - Planned FWOS Enhancement
                            {
                                ModelVariableProfile assocVar = null;   // Track #6187
                                if (aVariable.IsSelected)   // Issue 4962
                                {
                                    // Begin Track #6187 stodd - add assoc var
                                    if (aVariable.AssocVariable != Include.NoRID)
                                    {
                                        ProfileList variables = _cubeGroup.Transaction.PlanComputations.PlanVariables.VariableProfileList;
                                        VariableProfile vp = (VariableProfile)variables.FindKey(aVariable.AssocVariable);
                                        assocVar = new ModelVariableProfile(vp);
                                    }
                                    // End track #6187

                                    // BEGIN MID Track #4370 - John Smith - FWOS Models
                                    ProcessVariable(aVariable, _weeksToPlan.ArrayList, assocVar, aVariable.UsePlan);
                                    //								ProcessVariable(aVariable);
                                    // END MID Track #4370

                                    recomputeTimer.Start();
                                    _cubeGroup.RecomputeCubes(true);
                                    recomputeTimer.Stop();
                                    if (weeksToPlan.Count > 0)
                                    {
                                        recomputeTimer.Write("Variable: " + aVariable.VariableProfile.VariableName + " Recompute Plan Cubes for " +
                                            ((WeekProfile)weeksToPlan[0]).Text() + " thru " + ((WeekProfile)weeksToPlan[weeksToPlan.Count - 1]).Text());
                                    }
                                }
                            }
                        }

                        //===================
                        // Save cube values
                        //===================
                        if (_forecastingModel.Variables.Count > 0)
                            Save();
                    }
                    else
                    {
                        //=======================================================
                        // Determine which variables to use for sales and stock
                        //=======================================================
                        SetDefaultSalesStockVariables(aApplicationTransaction);  // Issue 4962

                        _openParms.ComputationsMode = _SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name;

                        while (moreWeeks)
                        {
                            //==========================
                            // Process Sales
                            //==========================
                            // BEGIN MID Track #4370 - John Smith - FWOS Models
                            foundSomethingToPlan = ProcessSales(_salesVariable, weeksToPlan);
                            //						foundSomethingToPlan = ProcessSales(_salesVariable);
                            // END MID Track #4370

                            //*******************************
                            // no functions to plan? error.
                            //*******************************
                            if (!foundSomethingToPlan)
                            {
                                string groupName = StoreMgmt.StoreGroup_GetName(this.SG_RID); //_SAB.StoreServerSession.GetStoreGroupName(this.SG_RID);
                                _errorMsg = "Method: " + this.Name + ", " + groupName + ".  No Store Sets are designated to be forecasted.";
                                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, _errorMsg, _sourceModule);
                                // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                                //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                                // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                                moreWeeks = false;
                            }
                            else
                            {
                                //******************
                                // BALANCE Sales
                                //******************
                                if (this.Bal_Sales_Ind)
                                {
                                    // BEGIN MID Track #4370 - John Smith - FWOS Models
                                    BalanceSalesMain(_salesVariable, weeksToPlan);
                                    //								BalanceSalesMain(_salesVariable);
                                    // END MID Track #4370
                                }

                                //******************
                                // Save Sales data
                                //******************
                                _cubeGroup.RecomputeCubes(true);
                                Save();

                                if (processOneWeekAtATime)
                                {
                                    weeksToPlan.Clear();
                                    ++currentWeekIndex;
                                    if (currentWeekIndex < _weeksToPlan.ArrayList.Count)
                                    {
                                        weeksToPlan.Add(_weeksToPlan.ArrayList[currentWeekIndex]);
                                    }
                                    else
                                    {
                                        moreWeeks = false;
                                    }
                                }
                                else
                                {
                                    moreWeeks = false;
                                }
                            }
                        }

                        if (foundSomethingToPlan)
                        {
                            //***********************
                            // Inventory
                            //***********************
                            // BEGIN MID Track #4370 - John Smith - FWOS Models
                            ProcessInventory(_stockVariable, _weeksToPlan.ArrayList);
                            //							ProcessInventory(_stockVariable);
                            // END MID Track #4370

                            //******************
                            // BALANCE Inventory
                            //******************
                            if (this.Bal_Stock_Ind)
                            {
                                // BEGIN MID Track #4370 - John Smith - FWOS Models
                                BalanceInventoryMain(_stockVariable, _weeksToPlan.ArrayList);
                                //								BalanceInventoryMain(_stockVariable);
                                // END MID Track #4370
                            }
                            // Begin Track # 6289 stodd
                            else if (_prorateChainStock)
                            {
                                BalanceInventoryMain(_stockVariable, _weeksToPlan.ArrayList);
                            }
                            // End track #6289
                            //******************
                            // Save Inventory data
                            //******************
                            _cubeGroup.RecomputeCubes(true);
                            Save();
                        }
                    }
                    // END MID Track #4370

                    // Begin TT#241 - stodd - add messaging for each node processed
                    processTimer.Stop();
                    //Begin TT#339 - MD - Modify Forecast audit message - RBeck 
                    //_infoMsg = "Completed Hierarchy Node: " + _hnp.NodeID + " DESC: " + _hnp.NodeDescription + ". Processing time: " + processTimer.ElaspedTimeString;
                    _infoMsg = "Completed Hierarchy Node: " + _hnp.Text + ". Processing time: " + processTimer.ElaspedTimeString;
                    //End TT#339 - MD - Modify Forecast audit message - RBeck
                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, _sourceModule);
                    // End TT#241 - stodd - add messaging for each node processed

                    WriteAuditInfo();
                    // BEGIN TT#3166 - stodd - error while running forecasting
                    _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionCompletedSuccessfully;
                    // END TT#3166 - stodd - error while running forecasting
                }
                catch (UserCancelledException)
                {
                    _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.NoActionPerformed;
                }
                catch (PlanInUseException)
                {
                    // BEGIN TT#3166 - stodd - error while running forecasting
                    _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
                    // END TT#3166 - stodd - error while running forecasting
                }
                catch (NoAttributeSetsToPlan)
                {
                    // BEGIN TT#3166 - stodd - error while running forecasting
                    _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.NoActionPerformed;
                    // END TT#3166 - stodd - error while running forecasting
                }
                catch (EndProcessingException)  // Issue 4827
                {
                    _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
                }
                catch (Exception err)
                {
                    // BEGIN Issue 5401 stodd
                    string msg = MIDText.GetText(eMIDTextCode.msg_MethodException);
                    msg = msg.Replace("{0}", this.Name);
                    msg = msg.Replace("{1}", err.ToString());
                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msg, _sourceModule);
                    // END Issue 5401
                    // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                    //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                    // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                    _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;

                    throw;
                }
                finally
                {
                    // Cleanup & dequeue
                    if (_cubeGroup != null)
                    {
                        ((ForecastCubeGroup)_cubeGroup).CloseCubeGroup();
                        // Begin MID Track #5210 - JSmith - Out of memory
                        _cubeGroup.Dispose();
                        _cubeGroup = null;
                        // End MID Track #5210
                    }

                    // Begin TT#1731 - JSmith - Forecast Job Errors in MID 4.0 - Out of Memory
                    _applicationTransaction.ClearSalesModifierCache();
                    _applicationTransaction.ClearStockModifierCache();
                    _applicationTransaction.ClearFWOSModifierCache();
                    _applicationTransaction.ClearPMPlusSalesCache();
                    _applicationTransaction.ClearSalesEligibilityCache();
                    _applicationTransaction.ClearStockEligibilityCache();
                    _applicationTransaction.ClearPriorityShippingCache();
                    _applicationTransaction.ClearSalesWkRangeEligibilityCache();
                    _applicationTransaction.ClearDailyPercentagesCache();
                    _applicationTransaction.ClearStoreGradesCache();
                    _applicationTransaction.ClearVelocityGradesCache();
                    _applicationTransaction.ClearSellThruPctsCache();
                    _applicationTransaction.ClearSimilarStoreCache();
                    _applicationTransaction.ClearStoreCapacityCache();

                    GC.Collect();
                    // End TT#1731

                    if (MONITOR)
                    {
                        // BEGIN TT#3166 - stodd - error while running forecasting
                        if (_forecastMonitor != null)
                        {
                            _forecastMonitor.CloseLogFile();
                        }
                        // END TT#3166 - stodd - error while running forecasting
                    }
                }
            }
            catch (Exception err)
            {
                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, err.Message, _sourceModule);
                // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                _applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;

                throw;
            }
            // Begin MID Track #5210 - JSmith - Out of memory
            finally
            {
                if (_storeGroupLevelHash != null)
                {
                    _storeGroupLevelHash.Clear();
                }
                if (_allStoreList != null)
                {
                    _allStoreList.Clear();
                }
                if (_stockMinMaxUsedHash != null)
                {
                    _stockMinMaxUsedHash.Clear();
                }
                if (_inventoryForecastSummandHash != null)
                {
                    _inventoryForecastSummandHash.Clear();
                }
            }
            // End MID Track #5210
        }

        // BEGIN Issue 4962
        public void SetDefaultSalesStockVariables(ApplicationSessionTransaction trans)
        {
            try
            {
                bool resetCubeGroup = false;
                if (_cubeGroup == null)
                {
                    // Begin MID Track #5210 - JSmith - Out of memory
                    _cubeGroup = (PlanCubeGroup)trans.GetForecastCubeGroup();
                    // End MID Track #5210
                    resetCubeGroup = true;
                }

                if (_hnp == null)
                    _hnp = _SAB.HierarchyServerSession.GetNodeData(this.Plan_HN_RID);

                // Begin TT#737-MD - JSmith - Store Forecasting-> sales are not being forecasted and log data does not match OTS forecast review
                if (MONITOR)
                {
                    if (_hnp.OTSPlanLevelType == eOTSPlanLevelType.Undefined)
                    {
                        _forecastMonitor.WriteLine("OTS Plan Level Type is Undefined");
                    }
                }
                // End TT#737-MD - JSmith - Store Forecasting-> sales are not being forecasted and log data does not match OTS forecast review

                if (_hnp.OTSPlanLevelType == eOTSPlanLevelType.Regular)
                {
                    // BEGIN TT#790 - stodd - ost forecast null ref
                    _salesVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.SalesRegularUnitsVariable.Key);
                    _salesVariable.ForecastFormula = (int)eForecastFormulaType.Sales;
                    _stockVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.InventoryRegularUnitsVariable.Key);
                    _stockVariable.ForecastFormula = (int)eForecastFormulaType.Stock;
                    // END TT#790 - stodd - ost forecast null ref
                }
                else
                {
                    // BEGIN TT#790 - stodd - ost forecast null ref
                    _salesVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.SalesTotalUnitsVariable.Key);
                    _salesVariable.ForecastFormula = (int)eForecastFormulaType.Sales;
                    _stockVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.InventoryTotalUnitsVariable.Key);
                    _stockVariable.ForecastFormula = (int)eForecastFormulaType.Stock;
                    // END TT#790 - stodd - ost forecast null ref
                }
                ProfileList variables = _cubeGroup.Transaction.PlanComputations.PlanVariables.VariableProfileList;
                VariableProfile vp = (VariableProfile)variables.FindKey(_salesVariable.Key);
                _salesVariable.VariableProfile = vp;
                vp = (VariableProfile)variables.FindKey(_stockVariable.Key);
                _stockVariable.VariableProfile = vp;

                if (resetCubeGroup)
                    _cubeGroup = null;
            }
            catch
            {
                throw;
            }
        }
        // END Issue 4962

        // BEGIN MID Track #4370 - John Smith - FWOS Models
        //		private bool ProcessSales(ModelVariableProfile aVariable)
        private bool ProcessSales(ModelVariableProfile aVariable, ArrayList aWeeksToProcess)
        // END MID Track #4370
        {
            try
            {
                _currentVariable = aVariable;
                bool foundSomethingToPlan = false;

                if (_setLists == null)
                    _setLists = GetSetLists();

                if (_setLists.Count == 0)
                {
                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_pl_NoAttributeSetsToPlan, _sourceModule);
                    // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                    //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                    // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                    throw new NoAttributeSetsToPlan();
                }

                // process each set list (a group of sets that are planned a like)
                foreach (ArrayList setList in _setLists)
                {
                    if (MONITOR)
                    {
                        _forecastMonitor.ClearSetDataOnly();
                        _forecastMonitor.IsDefault = ((GroupLevelFunctionProfile)setList[0]).Default_IND;
                        _forecastMonitor.ForecastType = ((GroupLevelFunctionProfile)setList[0]).GLFT_ID;
                        _forecastMonitor.SmoothBy = ((GroupLevelFunctionProfile)setList[0]).GLSB_ID;
                    }

                    foundSomethingToPlan = true;
                    //_firstReadForBasis = true;  // used during basis read

                    GroupLevelFunctionProfile groupLevelFunction = (GroupLevelFunctionProfile)setList[0];

                    // BEGIN TT#279-MD - stodd - Projected Sales
                    _applyToWeights.Clear();
                    _applyToWeightsFilled = false;
                    // BEGIN TT#279-MD - stodd - Projected Sales

                    // Track #4817 - JBolles - Weighting Multiple Basis
                    if (groupLevelFunction.TY_Weight_Multiple_Basis_Ind || groupLevelFunction.LY_Weight_Multiple_Basis_Ind || groupLevelFunction.Apply_Weight_Multiple_Basis_Ind)
                        WeightMultipleBasis(groupLevelFunction);

                    if (_firstSetToPlan)
                    {
                        // Begin MID Track #5210 - JSmith - Out of memory
                        _cubeGroup = (PlanCubeGroup)_applicationTransaction.GetForecastCubeGroup();
                        // End MID Track #5210
                        _openParms.BasisProfileList.Clear();
                        FillOpenParmForBasis(groupLevelFunction);
                        DateTime beginTimeO = System.DateTime.Now;
                        ((ForecastCubeGroup)_cubeGroup).OpenCubeGroup(_openParms);
                        DateTime endTimeO = System.DateTime.Now;
                        Debug.WriteLine("OpenCubeGroup: " + System.Convert.ToString(endTimeO.Subtract(beginTimeO)));
                        _firstSetToPlan = false;
                    }
                    else
                    {
                        // If this is NOT the default function AND
                        // this one does not use the default, then we must refresh
                        // the basis cube data
                        // Begin Issue 3814 - stodd
                        _openParms.BasisProfileList.Clear();
                        // End Issue 3814
                        FillOpenParmForBasis(groupLevelFunction);
                        DateTime beginTimeR = System.DateTime.Now;
                        ((ForecastCubeGroup)_cubeGroup).RefreshStoreBasis(_openParms);
                        DateTime endTimeR = System.DateTime.Now;
                        Debug.WriteLine("RefreshCubeGroupBasis: " + System.Convert.ToString(endTimeR.Subtract(beginTimeR)));
                    }

                    //					beginTime2 = System.DateTime.Now;
                    switch (groupLevelFunction.GLFT_ID)
                    {
                        case eGroupLevelFunctionType.PercentContribution:
                            {
                                Proportional forecastProportional = new Proportional(_SAB, this);
                                // BEGIN MID Track #4370 - John Smith - FWOS Models
                                // Begin Track #6187 stodd
                                forecastProportional.ProcessSets(setList, aWeeksToProcess, null, false);  // sends list of GLFs that are alike
                                                                                                          // End track #6187
                                                                                                          //							forecastProportional.ProcessSets(setList);  // sends list of GLFs that are alike
                                                                                                          // END MID Track #4370
                                                                                                          // Begin MID Track #5210 - JSmith - Out of memory
                                forecastProportional.Dispose();
                                // End MID Track #5210
                                break;
                            }
                        case eGroupLevelFunctionType.AverageSales:
                            {
                                _errorMsg = "The Average Sales forecast method is not available at this time.";
                                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, _errorMsg, _sourceModule);
                                // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                                //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                                // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running	
                                break;
                            }
                        case eGroupLevelFunctionType.CurrentTrend:
                            {
                                _errorMsg = "The Current Trend forecast method is not available at this time.";
                                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, _errorMsg, _sourceModule);
                                // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                                //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                                // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                                break;
                            }
                        case eGroupLevelFunctionType.TyLyTrend:
                            {
                                TyLyTrend forecastTyLyTrend = new TyLyTrend(_SAB, this);
                                // BEGIN MID Track #4370 - John Smith - FWOS Models
                                forecastTyLyTrend.ProcessSets(setList, aWeeksToProcess);  // sends list of GLFs that are alike
                                                                                          //							forecastTyLyTrend.ProcessSets(setList);  // sends list of GLFs that are alike
                                                                                          // END MID Track #4370
                                                                                          // Begin MID Track #5210 - JSmith - Out of memory
                                forecastTyLyTrend.Dispose();
                                // End MID Track #5210
                                break;
                            }
                        default:
                            {
                                _errorMsg = "The forecast method code of " + groupLevelFunction.GLFT_ID.ToString() + "is not valid.";
                                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, _errorMsg, _sourceModule);
                                // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                                //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                                // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                                break;
                            }
                    }
                }

                _infoMsg = "Forecasting Sales Complete";
                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, _sourceModule);

                return foundSomethingToPlan;

            }
            catch
            {
                throw;
            }
        }

        // Begin MID Track #4817 - JBolles - Weighting Multiple Basis
        private void WeightMultipleBasis(GroupLevelFunctionProfile groupLevelFunction)
        {
            PlanCubeGroup cubeGroup = null;

            try
            {
                // Begin MID Track #5210 - JSmith - Out of memory
                cubeGroup = (PlanCubeGroup)_applicationTransaction.GetForecastCubeGroup();
                // End MID Track #5210
                _openParms.BasisProfileList.Clear();
                FillOpenParmForBasis(groupLevelFunction);
                // Begin Track #5940 stodd
                ((ForecastCubeGroup)cubeGroup).OpenCubeGroup(_openParms, true);
                // End Track #5940
                // BEGIN TT#2634 - stodd - Weighted basis not producing expected results
                Cube totalCube = cubeGroup.GetCube(eCubeType.StoreBasisStoreTotalWeekDetail);

                if (MONITOR)
                {
                    _forecastMonitor.WriteLine("Weighted Basis Info: NEW WGT = (BASIS TOTAL * WGT) / BASIS WGT (may be blank)");
                }
                if (groupLevelFunction.TY_Weight_Multiple_Basis_Ind)
                {
                    WeightBasisByType(eTyLyType.TyLy, totalCube, null, groupLevelFunction);//  Track #5945 stodd
                }
                if (groupLevelFunction.LY_Weight_Multiple_Basis_Ind)
                {
                    WeightBasisByType(eTyLyType.AlternateLy, totalCube, null, groupLevelFunction);//  Track #5945 stodd
                }
                if (groupLevelFunction.Apply_Weight_Multiple_Basis_Ind)
                {
                    // BEGIN TT#279-MD - stodd - Projected Sales 
                    if (groupLevelFunction.Proj_Curr_Wk_Sales_IND)
                    {
                        // Changing cube so we can get store values back to apply projected sales percentages when accumming basis
                        Cube storeCube = cubeGroup.GetCube(eCubeType.StoreBasisWeekDetail);
                        WeightBasisByType(eTyLyType.ProjectCurrWkSales, totalCube, storeCube, groupLevelFunction);
                    }
                    else
                    {
                        WeightBasisByType(eTyLyType.AlternateApplyTo, totalCube, null, groupLevelFunction);//  Track #5945 stodd
                    }
                    // END TT#279-MD - stodd - Projected Sales 
                }
                // END TT#2634 - stodd - Weighted basis not producing expected results
                groupLevelFunction.TY_Weight_Multiple_Basis_Ind = false;
                groupLevelFunction.LY_Weight_Multiple_Basis_Ind = false;
                groupLevelFunction.Apply_Weight_Multiple_Basis_Ind = false;
            }
            catch
            {
                throw;
            }
            finally
            {
                ((ForecastCubeGroup)cubeGroup).CloseCubeGroup(true);
                // Begin MID Track #5399 - JSmith - Plan In Use error
                cubeGroup.Dispose();
                cubeGroup = null;
                // End MID Track #5399
            }
        }

        // Begin Track #5945 stodd
        private void WeightBasisByType(eTyLyType basisType, Cube totalCube, Cube storeCube, GroupLevelFunctionProfile groupLevelFunction)
        // End Track #5945 stodd
        {

            PlanCellReference planCellRef = null;
            double totalValue = 0;
            ArrayList rawValues = new ArrayList();
            int bIndex = 0;

            // Begin Track #5945 stodd
            _dtGroupLevelBasis.DefaultView.RowFilter = "SGL_RID = " + groupLevelFunction.Key.ToString();
            DataView dv = _dtGroupLevelBasis.DefaultView;
            // End Track #5945 stodd

            try
            {
                foreach (BasisProfile profile in _openParms.BasisProfileList)
                {
                    if (profile.BasisType == basisType)
                    {
                        BasisDetailProfile basisDetail = (BasisDetailProfile)profile.BasisDetailProfileList[0];
                        ProfileList basisWeeks = basisDetail.GetWeekProfileList(SAB.ApplicationServerSession);
                        //int basisWeekKey = basisDetail.GetPlanWeekIdFromBasisWeekId(SAB.ApplicationServerSession, basisWeeks[0].Key);
                        if (profile.BasisType == eTyLyType.ProjectCurrWkSales && basisWeeks[0].Key == SAB.ApplicationServerSession.Calendar.CurrentWeek.Key)
                        {
                            //BasisDetailProfile basisDetail = (BasisDetailProfile)profile.BasisDetailProfileList[0];
                            DateRangeProfile planRange = SAB.ApplicationServerSession.Calendar.GetDateRange(this.CDR_RID);
                            //ProfileList basisWeeks = basisDetail.GetWeekProfileList(SAB.ApplicationServerSession);
                            double cellValue = 0;

                            Dictionary<int, double> totpcthash = new Dictionary<int, double>();
                            foreach (Profile thisProfile in basisWeeks)
                            {
                                int dateKey = basisDetail.GetPlanWeekIdFromBasisWeekId(SAB.ApplicationServerSession, thisProfile.Key);

                                planCellRef = new PlanCellReference((PlanCube)storeCube);

                                planCellRef[eProfileType.Version] = this.Plan_FV_RID;
                                planCellRef[eProfileType.HierarchyNode] = this.Plan_HN_RID;
                                planCellRef[eProfileType.Basis] = profile.Key; ;
                                planCellRef[eProfileType.Week] = dateKey;
                                planCellRef[eProfileType.QuantityVariable] = _cubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
                                planCellRef[eProfileType.Variable] = this.CurrentVariable.VariableProfile.Key;
                                ArrayList storeBasisValues = planCellRef.GetCellRefArray(_allStoreList);

                                int storeRID;

                                totpcthash = AccumDailyPercentagesByStore(((WeekProfile)thisProfile).YearWeek);


                                string msg = "  START of weighted basis logic that applies projected sales to values...";
                                // BEGIN TT#344-MD - stodd - Forecast abending 
                                if (MONITOR)
                                {
                                    _forecastMonitor.WriteLine(msg);
                                }
                                // END TT#344-MD - stodd - Forecast abending 
                                for (int i = 0; i < storeBasisValues.Count; i++)
                                {
                                    PlanCellReference pcr = (PlanCellReference)storeBasisValues[i];
                                    storeRID = pcr[eProfileType.Store];
                                    double totpct = GetStoreAccumDailyPercentage(totpcthash, storeRID);
                                    double storeValue = 0;
                                    if (totpct != 0.0)
                                    {
                                        storeValue = (double)(decimal)(pcr.HiddenCurrentCellValue / totpct);
                                        cellValue += storeValue;
                                    }
                                    else
                                    {
                                        cellValue += pcr.HiddenCurrentCellValue;
                                    }

                                    StoreProfile sp = (StoreProfile)_allStoreList[i];
                                    msg = "    " + profile.Key.ToString() + ": " + sp.StoreId + "(" + storeRID + ")  Orig Val: " + pcr.HiddenCurrentCellValue +
                                        "  Proj Sales Pct: " + totpct + "  Proj Sales Value: " + storeValue;
                                    // BEGIN TT#344-MD - stodd - Forecast abending 
                                    if (MONITOR)
                                    {
                                        _forecastMonitor.WriteLine(msg);
                                    }
                                    // END TT#344-MD - stodd - Forecast abending 
                                }
                                msg = "  END of weighted basis logic that applies projected sales to values. Total = " + cellValue;
                                // BEGIN TT#344-MD - stodd - Forecast abending 
                                if (MONITOR)
                                {
                                    _forecastMonitor.WriteLine(msg);
                                }
                                // BEGIN TT#344-MD - stodd - Forecast abending 
                            }
                            // Begin Track #6171 stodd

                            cellValue = (Double)(int)(cellValue + .5);
                            WeightedBasisValue wbv = new WeightedBasisValue(profile.Key, basisDetail.Key, bIndex, cellValue);
                            // End Track #6171 stodd
                            rawValues.Add(wbv);
                            totalValue += cellValue;
                        }
                        else // Using Total cube
                        {
                            //BasisDetailProfile basisDetail = (BasisDetailProfile)profile.BasisDetailProfileList[0];
                            DateRangeProfile planRange = SAB.ApplicationServerSession.Calendar.GetDateRange(this.CDR_RID);
                            //ProfileList basisWeeks = basisDetail.GetWeekProfileList(SAB.ApplicationServerSession);
                            double cellValue = 0;

                            foreach (Profile thisProfile in basisWeeks)
                            {
                                int dateKey = basisDetail.GetPlanWeekIdFromBasisWeekId(SAB.ApplicationServerSession, thisProfile.Key);

                                planCellRef = new PlanCellReference((PlanCube)totalCube);
                                planCellRef[eProfileType.Version] = this.Plan_FV_RID;
                                planCellRef[eProfileType.HierarchyNode] = this.Plan_HN_RID;
                                planCellRef[eProfileType.Basis] = profile.Key;
                                planCellRef[eProfileType.Week] = dateKey;
                                planCellRef[eProfileType.QuantityVariable] = _cubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
                                planCellRef[eProfileType.Variable] = this.CurrentVariable.VariableProfile.Key;

                                cellValue += planCellRef.HiddenCurrentCellValue;
                            }
                            // Begin Track #6171 stodd
                            WeightedBasisValue wbv = new WeightedBasisValue(profile.Key, basisDetail.Key, bIndex, cellValue);
                            // End Track #6171 stodd
                            rawValues.Add(wbv);
                            totalValue += cellValue;
                        }
                    }
                    bIndex++;
                }

                foreach (WeightedBasisValue wbv in rawValues)
                {
                    if (wbv.Value != 0)
                    {
                        //Begin Track #6293 - KJohnson - Process Frcst Trend using Equalize weighting blows up with index out of bounds.
                        // Begin Track #6171 stodd
                        double rawWeight = Convert.ToDouble(_Entered_Weight_Values[wbv.BasisIndex - 1, wbv.BasisDetailIndex - 1]);
                        // End Track #6171
                        //End Track #6293
                        double newWeight = (totalValue * rawWeight) / wbv.Value;
                        if (MONITOR)
                        {
                            string msg = "  " + basisType.ToString() + ":  " + newWeight.ToString() + " = " + " (" + totalValue.ToString() + " * " +
                                rawWeight.ToString() + ") / " + wbv.Value.ToString();
                            _forecastMonitor.WriteLine(msg);
                        }

                        //// BEGIN MID Track #5954 - KJohnson - Basis weighting, exclusions, and equalizer issues
                        //if (newWeight == 0)
                        //{
                        //    newWeight = 1;
                        //}
                        //// END MID Track #5954

                        // Begin Track #5945 stodd
                        // BEGIN TT#279-MD - stodd - Projected Sales 
                        if (basisType == eTyLyType.ProjectCurrWkSales && groupLevelFunction.Proj_Curr_Wk_Sales_IND)
                        {
                            _applyToWeights.Add(wbv.BasisIndex, newWeight);
                            _applyToWeightsFilled = true;
                            dv[wbv.Index]["WEIGHT"] = 1;
                        }
                        else
                        {
                            dv[wbv.Index]["WEIGHT"] = newWeight;
                        }
                        // End Track #5945 stodd
                    }
                    else if (basisType == eTyLyType.ProjectCurrWkSales && groupLevelFunction.Proj_Curr_Wk_Sales_IND)
                    {
                        double wgt = 1;
                        if (dv[wbv.Index]["WEIGHT"] != DBNull.Value)
                        {
                            wgt = double.Parse(dv[wbv.Index]["WEIGHT"].ToString());
                        }
                        _applyToWeights.Add(wbv.BasisIndex, wgt);
                        _applyToWeightsFilled = true;
                        // END TT#279-MD - stodd - Projected Sales 
                    }
                }
                // Begin Track #5945 stodd
                _dtGroupLevelBasis.DefaultView.RowFilter = "";
                // End Track #5945 stodd


            }
            catch
            {
                throw;
            }

        }



        /// <summary>
        /// This builds an array list of the Default set, plus any sets that use the default, and adds it to the 
        /// setLists array.  Other sets are then added to the SetLists array, but these are really just array lists 
        /// of a the single set.  (The attempt here is to combine like Sets, so we can do them all at once.)
        /// </summary>
        /// <returns></returns>
        private ArrayList GetSetLists()
        {
            ArrayList setLists = new ArrayList();
            // BEGIN TRack #6145 stodd
            ArrayList tyLySetLists = new ArrayList();
            ArrayList pctConSetLists = new ArrayList();
            // END TRack #6145 stodd
            ArrayList defaultList = new ArrayList();
            foreach (GroupLevelFunctionProfile glf in GLFProfileList.ArrayList)
            {
                if (glf.Plan_IND)
                {
                    if (glf.Default_IND || glf.Use_Default_IND)
                    {
                        defaultList.Add(glf);
                    }
                    else
                    {
                        // BEGIN Issue 5553	stodd
                        //if (!defaultWasAdded)
                        //{
                        //    if (defaultList.Count > 0)
                        //        setLists.Add(defaultList);
                        //    defaultWasAdded = true;
                        //}
                        // END Issue 5553	stodd
                        ArrayList singleSetList = new ArrayList();
                        singleSetList.Add(glf);
                        // BEGIN TRack #6145 stodd
                        if (glf.GLFT_ID == eGroupLevelFunctionType.PercentContribution)
                            pctConSetLists.Add(singleSetList);
                        else if (glf.GLFT_ID == eGroupLevelFunctionType.TyLyTrend)
                            tyLySetLists.Add(singleSetList);
                        // END TRack #6145 stodd
                    }
                }
            }
            // Catch if defaults haven't been added yet to the setLists
            // BEGIN Issue 5553	stodd
            //if (!defaultWasAdded)
            //{
            // BEGIN TRack #6145 stodd
            // There seems to be an issue with the refresh basis. Doing the TY/LY sets first corrects the problem.  
            if (defaultList.Count > 0)
            {
                GroupLevelFunctionProfile firstGlfp = (GroupLevelFunctionProfile)defaultList[0];
                if (firstGlfp.GLFT_ID == eGroupLevelFunctionType.TyLyTrend)
                {
                    setLists.Add(defaultList);
                }
            }
            if (tyLySetLists.Count > 0)
            {
                setLists.AddRange(tyLySetLists);
            }
            if (defaultList.Count > 0)
            {
                GroupLevelFunctionProfile firstGlfp = (GroupLevelFunctionProfile)defaultList[0];
                if (firstGlfp.GLFT_ID == eGroupLevelFunctionType.PercentContribution)
                {
                    setLists.Add(defaultList);
                }
            }
            if (pctConSetLists.Count > 0)
            {
                setLists.AddRange(pctConSetLists);
            }
            // END TRack #6145 stodd
            //	defaultWasAdded = true;
            //}
            // END Issue 5553	stodd

            return setLists;
        }


        /// <summary>
        ///  controls the overall Balancing of Sales
        /// </summary>
        // BEGIN MID Track #4370 - John Smith - FWOS Models
        private void BalanceSalesMain(ModelVariableProfile aVariable, ArrayList aWeeksToPlan)
        // END MID Track #4370
        {
            try
            {
                _infoMsg = "Balancing Sales...";
                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, _sourceModule);

                if (MONITOR)
                {
                    //							_forecastMonitor.ReopenLogFile();
                    _forecastMonitor.WriteLine("INTERNAL - BALANCE SALES");
                    //							_forecastMonitor.CloseLogFile();
                }

                // Begin TT#83 MD - JSmith - Null reference error if Chain Set Percent values and no weeks to plan.
                if (aWeeksToPlan.Count == 0)
                {
                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_pl_NoWeeksSalesNotBalanced, _sourceModule);
                    if (MONITOR)
                    {
                        _infoMsg = MIDText.GetTextOnly(eMIDTextCode.msg_pl_NoWeeksSalesNotBalanced);
                        _forecastMonitor.WriteLine(_infoMsg);
                    }
                    return;
                }
                // End TT#83 MD

                // BEGIN MID Track #4370 - John Smith - FWOS Models
                BalanceSales(aVariable, aWeeksToPlan);
                //				BalanceSales(aVariable);
                // END MID Track #4370
                _infoMsg = "Balancing Sales Completed";
                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, _sourceModule);


                if (MONITOR)
                {
                    // this is set only to get the right format for the monitor log
                    _forecastMonitor.MonitorType = eForecastMonitorType.PercentContribution;

                    ForecastMonitorStoreData fmStoreData;
                    // BEGIN MID Track #4370 - John Smith - FWOS Models
                    foreach (WeekProfile planWeek in aWeeksToPlan)
                    // END MID Track #4370
                    {
                        _forecastMonitor.WriteLine(" ");
                        _forecastMonitor.WriteLine("Week: " + planWeek.Text());

                        ArrayList storePlanValues = ReadStoreValues(planWeek, _salesVariable);

                        for (int s = 0; s < storePlanValues.Count; s++)
                        {
                            PlanCellReference cr = (PlanCellReference)storePlanValues[s];
                            StoreProfile sp = (StoreProfile)_allStoreList[s];
                            fmStoreData = _forecastMonitor.CreateStoreData(sp.Key);
                            fmStoreData.StoreName = sp.StoreId;
                            fmStoreData.IsEligible = !cr.isCellIneligible;
                            fmStoreData.ResultValue = cr.CurrentCellValue;
                            if (cr.isCellLocked)
                                fmStoreData.IsLocked = true;
                            fmStoreData.Set = (int)StoreGroupLevelHash[fmStoreData.StoreRID];

                        }
                        _forecastMonitor.DumpToFile();
                        _forecastMonitor.ClearWeeklyDataOnly();
                    }
                }
            }
            catch
            {


            }
        }

        // BEGIN MID Track #4370 - John Smith - FWOS Models
        private void ProcessInventory(ModelVariableProfile aVariable, ArrayList aWeeksToProcess)
        // END MID Track #4370
        {
            try
            {
                _infoMsg = "Calculating Inventory...";
                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, _sourceModule);

                // BEGIN Issue 4962 stodd 11.27.2007
                // Added 12.27.2007 stodd
                // Discovered that during low level processing of a workflow using the forecast stock variable,
                // that the cube was not getting refreshed after the first node came through. Comparing the nodes
                // insures we refresh the cube when needed.
                if (_cubeGroup == null || _cubeGroup.OpenParms == null // Issue 5276 stodd
                    || _openParms.StoreHLPlanProfile.NodeProfile.Key != _cubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.Key)
                {
                    // Begin MID Track #5210 - JSmith - Out of memory
                    _cubeGroup = (PlanCubeGroup)_applicationTransaction.GetForecastCubeGroup();
                    // End MID Track #5210
                    _openParms.BasisProfileList.Clear();
                    DateTime beginTimeO = System.DateTime.Now;
                    ((ForecastCubeGroup)_cubeGroup).OpenCubeGroup(_openParms);
                    DateTime endTimeO = System.DateTime.Now;
                    Debug.WriteLine("OpenCubeGroup: " + System.Convert.ToString(endTimeO.Subtract(beginTimeO)));
                }
                // END Issue 4962

                _currentVariable = aVariable;
                InventoryForecast inventory = new InventoryForecast(_SAB, this);
                // BEGIN MID Track #4370 - John Smith - FWOS Models
                inventory.Process(aVariable, aWeeksToProcess);
                // END MID Track #4370

                // Begin MID Track #5210 - JSmith - Out of memory
                inventory.Dispose();
                // End MID Track #5210

                _infoMsg = "Calculating Inventory Completed";
                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, _sourceModule);
            }
            catch
            {
                throw;
            }
        }


        // BEGIN MID Track #4370 - John Smith - FWOS Models
        private void BalanceInventoryMain(ModelVariableProfile aVariable, ArrayList aWeeksToPlan)
        // END MID Track #4370
        {
            try
            {
                _infoMsg = "Balancing Inventory...";
                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, _sourceModule);

                // Begin Track #6289 stodd
                //if (MONITOR)
                //{
                //    _forecastMonitor.WriteLine(" ");
                //    _forecastMonitor.WriteLine("INTERNAL - BALANCE INVENTORY");
                //    _forecastMonitor.WriteLine(" ");
                //}

                // Begin TT#83 MD - JSmith - Null reference error if Chain Set Percent values and no weeks to plan.
                if (aWeeksToPlan.Count == 0)
                {
                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_pl_NoWeeksStockNotBalanced, _sourceModule);
                    if (MONITOR)
                    {
                        _infoMsg = MIDText.GetTextOnly(eMIDTextCode.msg_pl_NoWeeksStockNotBalanced);
                        _forecastMonitor.WriteLine(_infoMsg);
                    }
                    return;
                }
                // End TT#83 MD

                // BEGIN MID Track #4370 - John Smith - FWOS Models
                bool showBalanceLog = BalanceInventory(aVariable, aWeeksToPlan);
                // END MID Track #4370

                if (MONITOR && showBalanceLog)
                {

                    _forecastMonitor.WriteLine(" ");
                    _forecastMonitor.WriteLine("BALANCE INVENTORY");
                    _forecastMonitor.WriteLine(" ");
                    // End Track #6289

                    // this is set only to get the right format for the monitor log
                    _forecastMonitor.MonitorType = eForecastMonitorType.PercentContribution;

                    ForecastMonitorStoreData fmStoreData;
                    // BEGIN MID Track #4370 - John Smith - FWOS Models
                    foreach (WeekProfile planWeek in aWeeksToPlan)
                    // END MID Track #4370
                    {
                        _forecastMonitor.WriteLine(" ");
                        _forecastMonitor.WriteLine("Week: " + planWeek.Text());

                        ArrayList storePlanValues = ReadStoreValues(planWeek, _stockVariable);

                        for (int s = 0; s < storePlanValues.Count; s++)
                        {
                            PlanCellReference cr = (PlanCellReference)storePlanValues[s];
                            StoreProfile sp = (StoreProfile)_allStoreList[s];
                            fmStoreData = _forecastMonitor.CreateStoreData(sp.Key);
                            fmStoreData.StoreName = sp.StoreId;
                            fmStoreData.IsEligible = !cr.isCellIneligible;
                            fmStoreData.Inventory = cr.CurrentCellValue;
                            fmStoreData.ResultValue = cr.CurrentCellValue;
                            if (cr.isCellLocked)
                                fmStoreData.IsLocked = true;
                        }
                        _forecastMonitor.WriteAllStoreData();
                        _forecastMonitor.ClearWeeklyDataOnly();
                    }
                }
                _infoMsg = "Balancing Inventory Completed";
                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, _sourceModule);
            }
            catch
            {

            }
        }

        // BEGIN MID Track #4370 - John Smith - FWOS Models
        private bool ProcessVariable(ModelVariableProfile aVariable, ArrayList aWeeksToProcess, ModelVariableProfile assocVariable, bool usePlanForBasis)   // Track #6187
                                                                                                                                                            // END MID Track #4370
        {
            try
            {
                _infoMsg = "Processing " + aVariable.VariableProfile.VariableName + "...";
                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, _sourceModule);

                bool foundSomethingToPlan = false;

                if (_setLists == null)
                    _setLists = GetSetLists();

                if (_setLists.Count == 0)
                {
                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_pl_NoAttributeSetsToPlan, _sourceModule);
                    // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                    //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                    // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                    throw new NoAttributeSetsToPlan();
                }

                // Begin TT#5115 - JSmith - Error Message when running store forecast
                // Reset _firstWeekOfBasis for next variable
                _firstWeekOfBasis = (WeekProfile)aWeeksToProcess[0];
                // End TT#5115 - JSmith - Error Message when running store forecast

                // process each set list (a group of sets that are planned a like)
                foreach (ArrayList setList in _setLists)
                {
                    if (MONITOR)
                    {
                        _forecastMonitor.ClearSetDataOnly();
                        _forecastMonitor.IsDefault = ((GroupLevelFunctionProfile)setList[0]).Default_IND;
                        _forecastMonitor.ForecastType = ((GroupLevelFunctionProfile)setList[0]).GLFT_ID;
                        _forecastMonitor.SmoothBy = ((GroupLevelFunctionProfile)setList[0]).GLSB_ID;
                    }

                    foundSomethingToPlan = true;
                    //_firstReadForBasis = true;  // used during basis read

                    GroupLevelFunctionProfile groupLevelFunction = (GroupLevelFunctionProfile)setList[0];

                    if (_firstSetToPlan)
                    {
                        // Begin MID Track #5210 - JSmith - Out of memory
                        _cubeGroup = (PlanCubeGroup)_applicationTransaction.GetForecastCubeGroup();
                        // End MID Track #5210
                        // Begin MID Track #5601
                        _openParms.BasisProfileList.Clear();
                        // END MID Track #5601
                        FillOpenParmForBasis(groupLevelFunction);
                        DateTime beginTimeO = System.DateTime.Now;
                        ((ForecastCubeGroup)_cubeGroup).OpenCubeGroup(_openParms);
                        DateTime endTimeO = System.DateTime.Now;
                        Debug.WriteLine("OpenCubeGroup: " + System.Convert.ToString(endTimeO.Subtract(beginTimeO)));
                        _firstSetToPlan = false;
                    }
                    else
                    {
                        // If this is NOT the default function AND
                        // this one does not use the default, then we must refresh
                        // the basis cube data
                        // Begin MID Track #5601
                        _openParms.BasisProfileList.Clear();
                        // END MID Track #5601
                        FillOpenParmForBasis(groupLevelFunction);
                        DateTime beginTimeR = System.DateTime.Now;
                        ((ForecastCubeGroup)_cubeGroup).RefreshStoreBasis(_openParms);
                        DateTime endTimeR = System.DateTime.Now;
                        Debug.WriteLine("RefreshCubeGroupBasis: " + System.Convert.ToString(endTimeR.Subtract(beginTimeR)));
                    }

                    Proportional forecastProportional = new Proportional(_SAB, this);
                    // BEGIN MID Track #4370 - John Smith - FWOS Models
                    // Begin Track #6187
                    forecastProportional.ProcessSets(setList, aWeeksToProcess, assocVariable, usePlanForBasis);  // sends list of GLFs that are alike
                                                                                                                 // End Track #6187
                                                                                                                 // END MID Track #4370


                }

                _infoMsg = "Processing " + aVariable.VariableProfile.VariableName + " Complete";
                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, _sourceModule);
                return foundSomethingToPlan;
            }
            catch
            {
                throw;
            }
        }

        public void SetStockMinMaxTrue(int sglRid, int storeRid)
        {
            try
            {
                if (_stockMinMaxUsedHash == null)
                {
                    _stockMinMaxUsedHash = new Hashtable();
                }
                if (_stockMinMaxUsedHash.ContainsKey(sglRid))
                {
                    _stockMinMaxUsedHash[sglRid] = true;
                }
                else
                {
                    _stockMinMaxUsedHash.Add(sglRid, true);
                }
            }
            catch
            {
                throw;
            }
        }

        private void Save()
        {
            try
            {
                if (_cubeGroup.GetType() == typeof(ForecastCubeGroup))
                {
                    _infoMsg = "Saving forecasting values...";
                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, _sourceModule);

                    // BEGIN MID Track #4370 - John Smith - FWOS Models
                    PlanSaveParms planSaveParms = new PlanSaveParms();
                    planSaveParms.SaveLocks = false;
                    _cubeGroup.SaveCubeGroup(planSaveParms);
                    // END MID Track #4370

                    _infoMsg = "Saving forecasting Values Completed";
                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, _infoMsg, _sourceModule);
                }
            }
            catch
            {
                throw;
            }
        }

        private void WriteAuditInfo()
        {
            //Begin - Abercrombie & Fitch #4448 - JSmith - Audit
            int auditRID;
            AuditData auditData = new AuditData();
            try
            {
                auditData.OpenUpdateConnection();
                DateRangeProfile drp = null;
                DateRangeProfile planDRP = _SAB.ApplicationServerSession.Calendar.GetDateRange(this.CDR_RID);
                ProfileList weekRange = _SAB.ApplicationServerSession.Calendar.GetWeekRange(planDRP, null);
                //StoreGroupProfile sgp = StoreMgmt.GetStoreGroup(SG_RID); //_SAB.StoreServerSession.GetStoreGroup(SG_RID);
                string groupName = StoreMgmt.StoreGroup_GetName(SG_RID); //TT#1517-MD -jsobek -Store Service Optimization
                ProfileList sgll = StoreMgmt.StoreGroup_GetLevelListViewList(SG_RID); //_SAB.StoreServerSession.GetStoreGroupLevelListViewList(SG_RID);
                StoreGroupLevelListViewProfile sgllvp;
                GroupLevelNodeFunction glnf = null;
                string forecastType = null;
                bool stockMinMax = false;
                int sequence = 0;

                auditRID = auditData.ForecastAuditForecast_Add(DateTime.Now,
                    _SAB.ApplicationServerSession.Audit.ProcessRID,
                    _SAB.ClientServerSession.UserRID,
                    this.Plan_HN_RID,
                    _hnp.Text,                  //tt#1049 - Forecast Audit and Trend Tab Apply to do not match on the merchandise levels - apicchetti - 1/12/2010
                    this.Key,
                    this.Name,
                    this.MethodType,
                    this.Plan_FV_RID,
                    this.Chain_FV_RID,
                    planDRP.DateRangeType,
                    planDRP.Name,
                    planDRP.DisplayDate,
                    ((WeekProfile)weekRange[0]).YearWeek,
                    ((WeekProfile)weekRange[weekRange.Count - 1]).YearWeek,
                    this.SG_RID,
                    groupName);
                // write details for each set
                foreach (GroupLevelFunctionProfile glfp in this._GLFProfileList.ArrayList)
                {
                    sgllvp = (StoreGroupLevelListViewProfile)sgll.FindKey(glfp.Key);
                    glnf = (GroupLevelNodeFunction)glfp.Group_Level_Nodes[this.Plan_HN_RID];

                    stockMinMax = false;
                    if (glnf != null)
                    {
                        if (_stockMinMaxUsedHash.ContainsKey(glnf.SglRID))
                        {
                            stockMinMax = (bool)_stockMinMaxUsedHash[glnf.SglRID];
                        }
                    }

                    forecastType = MIDText.GetTextOnly((int)glfp.GLFT_ID);
                    auditData.ForecastAuditSet_Insert(auditRID, glfp.Key, sgllvp.Name,
                        forecastType, stockMinMax);
                    // write set basis details
                    sequence = 0;
                    switch (glfp.GLFT_ID)
                    {
                        // BEGIN Issue 4818
                        case eGroupLevelFunctionType.PercentContribution:
                            foreach (GroupLevelBasisProfile basis in glfp.GroupLevelBasis.ArrayList)
                            {
                                drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(basis.Basis_CDR_RID);
                                if (drp.DisplayDate == null || drp.DisplayDate == string.Empty)
                                {
                                    drp.InternalAnchorDate = _SAB.ApplicationServerSession.Calendar.GetFirstWeekOfRange(planDRP);
                                    drp.DisplayDate = _SAB.ApplicationServerSession.Calendar.GetDisplayDate(drp);
                                }
                                // If the basis node is a heirarchy level, then we replace it with the plan node.
                                int basisNodeRid = 0;
                                if (basis.Basis_HN_RID == 0)
                                    basisNodeRid = Plan_HN_RID;
                                else
                                    basisNodeRid = basis.Basis_HN_RID;

                                //BEGIN tt#1049 - Forecast Audit and Trend Tab Apply to do not match on the merchandise levels - apicchetti - 01/12/2011
                                HierarchyNodeProfile basisHNP = _SAB.HierarchyServerSession.GetNodeData(basisNodeRid);

                                auditData.ForecastAuditSetBasis_Insert(auditRID, glfp.Key, sequence,
                                    basisNodeRid, basisHNP.Text, basis.Basis_FV_RID, drp.DisplayDate, basis.Basis_Weight, 0, string.Empty);
                                //END tt#1049 - Forecast Audit and Trend Tab Apply to do not match on the merchandise levels - apicchetti - 01/12/2011

                                ++sequence;
                            }
                            break;
                        case eGroupLevelFunctionType.TyLyTrend:
                            foreach (GroupLevelBasisProfile basis in glfp.GroupLevelBasis.ArrayList)
                            {
                                eTyLyType basisType = (eTyLyType)(Convert.ToInt32(basis.Basis_TyLyType));

                                drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(basis.Basis_CDR_RID);
                                if (drp.DisplayDate == null || drp.DisplayDate == string.Empty)
                                {
                                    drp.InternalAnchorDate = _SAB.ApplicationServerSession.Calendar.GetFirstWeekOfRange(planDRP);
                                    drp.DisplayDate = _SAB.ApplicationServerSession.Calendar.GetDisplayDate(drp);
                                }

                                string basisTypeString = string.Empty;
                                int basisSortCode = 0;
                                switch (basisType)
                                {
                                    case eTyLyType.TyLy:
                                        basisSortCode = 1;
                                        basisTypeString = MIDText.GetTextOnly(eMIDTextCode.lbl_ThisYear);
                                        break;
                                    case eTyLyType.AlternateLy:
                                        basisSortCode = 2;
                                        basisTypeString = MIDText.GetTextOnly(eMIDTextCode.lbl_LastYear);
                                        break;
                                    case eTyLyType.AlternateApplyTo:
                                        basisSortCode = 3;
                                        basisTypeString = MIDText.GetTextOnly(eMIDTextCode.lbl_ApplyTo);
                                        break;
                                    //begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                                    case eTyLyType.ProjectCurrWkSales:
                                        basisSortCode = 3;
                                        basisTypeString = MIDText.GetTextOnly(eMIDTextCode.lbl_Project_Curr_WK_Sls);
                                        break;
                                    //End TT#43 - MD - DOConnell - Projected Sales Enhancement
                                    default:
                                        basisSortCode = 0;
                                        basisTypeString = MIDText.GetTextOnly(eMIDTextCode.lbl_PercentContribution);
                                        break;
                                }
                                // If the basis node is a heirarchy level, then we replace it with the plan node.
                                int basisNodeRid = 0;
                                if (basis.Basis_HN_RID == 0)
                                    basisNodeRid = Plan_HN_RID;
                                else
                                    basisNodeRid = basis.Basis_HN_RID;

                                //BEGIN tt#1049 - Forecast Audit and Trend Tab Apply to do not match on the merchandise levels - apicchetti - 01/12/2011
                                HierarchyNodeProfile basisHNP = _SAB.HierarchyServerSession.GetNodeData(basisNodeRid);

                                auditData.ForecastAuditSetBasis_Insert(auditRID, glfp.Key, sequence,
                                    basisNodeRid, basisHNP.Text, basis.Basis_FV_RID, drp.DisplayDate, basis.Basis_Weight, basisSortCode, basisTypeString);
                                //END tt#1049 - Forecast Audit and Trend Tab Apply to do not match on the merchandise levels - apicchetti - 01/12/2011

                                ++sequence;


                            }
                            break;
                            // END Issue 4818
                    }
                }

                auditData.CommitData();
            }
            catch
            {
                throw;
            }
            finally
            {
                auditData.CloseUpdateConnection();
            }
            //End - Abercrombie & Fitch #4448





        }

        private void SetGroupLevelFunctionProfileList()
        {
            GroupLevelFunction glf = new GroupLevelFunction();
            DataTable dt = glf.GetAllGroupLevelFunctions(this.Key);
            // Begin TT#1891-MD - JSmith - OTS Forecast Method - Set Methods Tab - Forecast selection keeps changing.
            //foreach(DataRow dr in dt.Rows)
            //{
            DataRow dr;
            ProfileList pl = StoreMgmt.StoreGroup_GetLevelListViewList(SG_RID);
            foreach (StoreGroupLevelListViewProfile sgllvp in pl)
            {
                DataRow[] drows = dt.Select("SGL_RID=" + sgllvp.Key);
                if (drows.Length == 0)
                {
                    continue;
                }
                dr = drows[0];
                // End TT#1891-MD - JSmith - OTS Forecast Method - Set Methods Tab - Forecast selection keeps changing.
                GroupLevelFunctionProfile GLFP = new GroupLevelFunctionProfile(Convert.ToInt32(dr["SGL_RID"], CultureInfo.CurrentUICulture));
                GLFP.Default_IND = Include.ConvertCharToBool(Convert.ToChar(dr["DEFAULT_IND"], CultureInfo.CurrentUICulture));
                GLFP.Plan_IND = Include.ConvertCharToBool(Convert.ToChar(dr["PLAN_IND"], CultureInfo.CurrentUICulture));
                GLFP.Use_Default_IND = Include.ConvertCharToBool(Convert.ToChar(dr["USE_DEFAULT_IND"], CultureInfo.CurrentUICulture));
                GLFP.Clear_IND = Include.ConvertCharToBool(Convert.ToChar(dr["CLEAR_IND"], CultureInfo.CurrentUICulture));
                GLFP.Season_IND = Include.ConvertCharToBool(Convert.ToChar(dr["SEASON_IND"], CultureInfo.CurrentUICulture));
                GLFP.Season_HN_RID = Convert.ToInt32(dr["SEASON_HN_RID"], CultureInfo.CurrentUICulture);
                GLFP.GLFT_ID = (eGroupLevelFunctionType)Convert.ToInt32(dr["GLFT_ID"], CultureInfo.CurrentUICulture);
                GLFP.GLSB_ID = (eGroupLevelSmoothBy)Convert.ToInt32(dr["GLSB_ID"], CultureInfo.CurrentUICulture);
                GLFP.LY_Alt_IND = Include.ConvertCharToBool(Convert.ToChar(dr["LY_ALT_IND"], CultureInfo.CurrentUICulture));
                GLFP.Trend_Alt_IND = Include.ConvertCharToBool(Convert.ToChar(dr["TREND_ALT_IND"], CultureInfo.CurrentUICulture));
                GLFP.TY_Weight_Multiple_Basis_Ind = Include.ConvertCharToBool(Convert.ToChar(dr["TY_EQUALIZE_WEIGHT_IND"], CultureInfo.CurrentUICulture));
                GLFP.LY_Weight_Multiple_Basis_Ind = Include.ConvertCharToBool(Convert.ToChar(dr["LY_EQUALIZE_WEIGHT_IND"], CultureInfo.CurrentUICulture));
                GLFP.Apply_Weight_Multiple_Basis_Ind = Include.ConvertCharToBool(Convert.ToChar(dr["APPLY_EQUALIZE_WEIGHT_IND"], CultureInfo.CurrentUICulture));
                GLFP.GLF_Change_Type = eChangeType.none;
                //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
                GLFP.Proj_Curr_Wk_Sales_IND = Include.ConvertCharToBool(Convert.ToChar(dr["PROJECT_CURR_WEEK_SALES_IND"], CultureInfo.CurrentUICulture));
                //END TT#43 - MD - DOConnell - Projected Sales Enhancement
                // Fill in Basis info AND trend caps
                SetGroupLevelFunctionBasisProfileList(GLFP);
                SetGroupLevelFunctionGroupLevelNodesList(GLFP);


                GLFP.Filled = true;

                _GLFProfileList.Add(GLFP);
            }
        }

        private void SetGroupLevelFunctionGroupLevelNodesList(GroupLevelFunctionProfile GLFP)
        {
            TransactionData td = new TransactionData();
            td.OpenUpdateConnection();
            DataTable dtGLNF = GroupLevelNodeFunction.GetAllGroupLevelNodeFunctions(this.Key, GLFP.Key, td);

            foreach (DataRow dr in dtGLNF.Rows)
            {
                GroupLevelNodeFunction glfn = new GroupLevelNodeFunction();
                glfn.MethodRID = this.Key;
                glfn.SglRID = GLFP.Key;
                glfn.HN_RID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
                glfn.ApplyMinMaxesInd = Include.ConvertCharToBool(Convert.ToChar(dr["APPLY_MIN_MAXES_IND"], CultureInfo.CurrentUICulture));
                glfn.MinMaxInheritType = (eMinMaxInheritType)Convert.ToInt32(dr["MIN_MAXES_INHERIT_TYPE"], CultureInfo.CurrentUICulture);
                SetGroupLevelFunctionStockMinMaxProfileList(glfn);
                GLFP.Group_Level_Nodes.Add(glfn.HN_RID, glfn);
            }

            if (!GLFP.Group_Level_Nodes.Contains(Plan_HN_RID))
            {
                GroupLevelNodeFunction glfn = new GroupLevelNodeFunction();
                glfn.MethodRID = this.Key;
                glfn.SglRID = GLFP.Key;
                glfn.HN_RID = Plan_HN_RID;
                glfn.ApplyMinMaxesInd = true;
                glfn.MinMaxInheritType = eMinMaxInheritType.None;
                SetGroupLevelFunctionStockMinMaxProfileList(glfn);
                GLFP.Group_Level_Nodes.Add(glfn.HN_RID, glfn);
            }

            td.CloseUpdateConnection();
        }

        // BEGIN Override Low level enhancement
        //public void MergeGroupLevelFunctionGroupLevelNodesList()
        //{
        //    foreach (GroupLevelFunctionProfile GLFP in GLFProfileList.ArrayList)
        //    {
        //        // Merge in any missing low levels that default from the high level setting.
        //        GroupLevelNodeFunction HLglfn = null;
        //        HLglfn = (GroupLevelNodeFunction)GLFP.Group_Level_Nodes[Plan_HN_RID];
        //        foreach(LowLevelExcludeProfile llep in LowlevelExcludeList)
        //        {
        //            if(!llep.Exclude)
        //            {
        //                if (!GLFP.Group_Level_Nodes.ContainsKey(llep.NodeProfile.Key))
        //                {
        //                    GroupLevelNodeFunction glfn = new GroupLevelNodeFunction();
        //                    glfn.MethodRID = this.Key;
        //                    glfn.SglRID = GLFP.Key;
        //                    glfn.HN_RID = llep.NodeProfile.Key;
        //                    glfn.ApplyMinMaxesInd = HLglfn.ApplyMinMaxesInd;
        //                    glfn.MinMaxInheritType = HLglfn.MinMaxInheritType;
        //                    SetGroupLevelFunctionStockMinMaxProfileList(glfn);
        //                    GLFP.Group_Level_Nodes.Add(glfn.HN_RID, glfn);
        //                }
        //            }
        //        }
        //    }
        //}
        // END Override Low level enhancement

        private void SetGroupLevelFunctionStockMinMaxProfileList(GroupLevelNodeFunction GLNF)
        {
            switch (GLNF.MinMaxInheritType)
            {
                case eMinMaxInheritType.None:
                    LoadStockMinMaxProfileListFromNode(GLNF, GLNF.HN_RID);
                    break;
                case eMinMaxInheritType.Method:
                    _minMaxInheritedFrom = eInheritedFrom.Method;
                    LoadStockMinMaxProfileListFromNode(GLNF, Orig_Plan_HN_RID);
                    break;
                case eMinMaxInheritType.Hierarchy:
                    LoadStockMinMaxProfileListFromHierarchy(GLNF, GLNF.HN_RID);
                    break;
                // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                case eMinMaxInheritType.Default:
                    LoadStockMinMaxProfileListFromNode(GLNF, GLNF.HN_RID);
                    break;
                    // End TT#3
            }
        }

        private void LoadStockMinMaxProfileListFromNode(GroupLevelNodeFunction GLNF, int aNodeRID)
        {
            int key = -1;
            StockMinMax smm = new StockMinMax();
            DataTable dt = smm.GetStockMinMax(GLNF.MethodRID, GLNF.SglRID, aNodeRID);
            foreach (DataRow dr in dt.Rows)
            {
                StockMinMaxProfile smmp = new StockMinMaxProfile(key);
                smmp.MethodRid = Convert.ToInt32(dr["METHOD_RID"], CultureInfo.CurrentUICulture);
                smmp.StoreGroupLevelRid = Convert.ToInt32(dr["SGL_RID"], CultureInfo.CurrentUICulture);
                smmp.HN_RID = GLNF.HN_RID;
                smmp.DateRangeRid = Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture);
                smmp.Boundary = Convert.ToInt32(dr["BOUNDARY"], CultureInfo.CurrentUICulture);
                if (dr["MIN_STOCK"] == System.DBNull.Value)
                    smmp.MinimumStock = (int)Include.UndefinedDouble;
                else
                    smmp.MinimumStock = Convert.ToInt32(dr["MIN_STOCK"], CultureInfo.CurrentUICulture);
                if (dr["MAX_STOCK"] == System.DBNull.Value)
                    smmp.MaximumStock = (int)Include.UndefinedDouble;
                else
                    smmp.MaximumStock = Convert.ToInt32(dr["MAX_STOCK"], CultureInfo.CurrentUICulture);

                GLNF.Stock_MinMax.Add(smmp);
                key--;
            }
        }

        public void ReloadNodeStockMinMax(GroupLevelNodeFunction GLNF)
        {
            GLNF.Stock_MinMax.Clear();
            SetGroupLevelFunctionStockMinMaxProfileList(GLNF);
        }

        // Begin TT#5028 - JSmith - Node Property - Sku Level Min/Max setup
        //private void LoadStockMinMaxProfileListFromHierarchy(GroupLevelNodeFunction GLNF, int aNodeRID)
        internal void LoadStockMinMaxProfileListFromHierarchy(GroupLevelNodeFunction GLNF, int aNodeRID)
        // End TT#5028 - JSmith - Node Property - Sku Level Min/Max setup
        {
            int key = -1;
            NodeStockMinMaxesProfile nodeStockMinMaxesProfile = _SAB.HierarchyServerSession.GetStockMinMaxes(aNodeRID);
            _minMaxInheritedFrom = eInheritedFrom.Node;
            if (nodeStockMinMaxesProfile.NodeStockMinMaxsIsInherited)
            {
                _minMaxInheritedFromNodeRID = nodeStockMinMaxesProfile.NodeStockMinMaxsInheritedFromNodeRID;
            }
            else
            {
                _minMaxInheritedFromNodeRID = nodeStockMinMaxesProfile.Key;
            }
            if (nodeStockMinMaxesProfile.NodeStockStoreGroupRID != SG_RID)
            {
                _minMaxAttributeMismatch = true;
                return;
            }

            _minMaxAttributeMismatch = false;
            NodeStockMinMaxSetProfile minMaxSetProfile = (NodeStockMinMaxSetProfile)nodeStockMinMaxesProfile.NodeSetList.FindKey(GLNF.SglRID);
            if (minMaxSetProfile != null)
            {
                foreach (NodeStockMinMaxProfile minMaxProfile in minMaxSetProfile.Defaults.MinMaxList)
                {
                    StockMinMaxProfile smmp = new StockMinMaxProfile(key);
                    smmp.MethodRid = Key;
                    smmp.StoreGroupLevelRid = minMaxSetProfile.Key;
                    smmp.DateRangeRid = minMaxProfile.Key;
                    smmp.HN_RID = aNodeRID;
                    smmp.Boundary = Include.NoRID;
                    if (minMaxProfile.Minimum == int.MinValue)
                    {
                        smmp.MinimumStock = (int)Include.UndefinedDouble;
                    }
                    else
                    {
                        smmp.MinimumStock = minMaxProfile.Minimum;
                    }
                    if (minMaxProfile.Maximum == int.MaxValue)
                        smmp.MaximumStock = (int)Include.UndefinedDouble;
                    else
                        smmp.MaximumStock = minMaxProfile.Maximum;

                    GLNF.Stock_MinMax.Add(smmp);
                    key--;
                }

                foreach (NodeStockMinMaxBoundaryProfile minMaxBoundaryProfile in minMaxSetProfile.BoundaryList)
                {
                    foreach (NodeStockMinMaxProfile minMaxProfile in minMaxBoundaryProfile.MinMaxList)
                    {
                        StockMinMaxProfile smmp = new StockMinMaxProfile(key);
                        smmp.MethodRid = Key;
                        smmp.StoreGroupLevelRid = minMaxSetProfile.Key;
                        smmp.DateRangeRid = minMaxProfile.Key;
                        smmp.HN_RID = aNodeRID;
                        smmp.Boundary = minMaxBoundaryProfile.Key;
                        if (minMaxProfile.Minimum == int.MinValue)
                        {
                            smmp.MinimumStock = (int)Include.UndefinedDouble;
                        }
                        else
                        {
                            smmp.MinimumStock = minMaxProfile.Minimum;
                        }
                        if (minMaxProfile.Maximum == int.MaxValue)
                            smmp.MaximumStock = (int)Include.UndefinedDouble;
                        else
                            smmp.MaximumStock = minMaxProfile.Maximum;

                        GLNF.Stock_MinMax.Add(smmp);
                        key--;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves and loads the basis_plan and basis_range data for the Group Level Function
        /// </summary>
        /// <param name="GLFP"></param>
        private void SetGroupLevelFunctionBasisProfileList(GroupLevelFunctionProfile GLFP)
        {
            // BEGIN Issue 4818
            GroupLevelBasis glb = new GroupLevelBasis();
            DataTable dt = glb.GetGroupLevelBasis(this.Key, GLFP.Key);
            foreach (DataRow dr in dt.Rows)
            {
                int seq = Convert.ToInt32(dr["BASIS_SEQ"]);
                GroupLevelBasisProfile glbp = new GroupLevelBasisProfile(seq);

                if (dr["HN_RID"] == DBNull.Value)
                    glbp.Basis_HN_RID = Include.NoRID;
                else
                    glbp.Basis_HN_RID = Convert.ToInt32(dr["HN_RID"]);
                glbp.Basis_FV_RID = Convert.ToInt32(dr["FV_RID"]);
                glbp.Basis_Weight = Convert.ToDouble(dr["WEIGHT"]);
                glbp.Basis_CDR_RID = Convert.ToInt32(dr["CDR_RID"]);
                glbp.Basis_ExcludeInd = Include.ConvertCharToBool(Convert.ToChar(dr["INC_EXC_IND"]));
                glbp.Basis_TyLyType = (eTyLyType)Convert.ToInt32(dr["TYLY_TYPE_ID"]);
                glbp.MerchType = (eMerchandiseType)Convert.ToInt32(dr["MERCH_TYPE"]);

                GLFP.GroupLevelBasis.Add(glbp);
            }
            // END Issue 4818

            TrendCaps tc = new TrendCaps();
            dt = tc.GetTrendCaps(this.Key, GLFP.Key);
            foreach (DataRow dr in dt.Rows)
            {
                TrendCapsProfile tcp = new TrendCapsProfile(GLFP.Key);
                tcp.TrendCapID = (eTrendCapID)dr["TREND_CAP_ID"];
                if (dr["TOL_PCT"] == System.DBNull.Value)
                    tcp.TolPct = Include.UndefinedDouble;
                else
                    tcp.TolPct = Convert.ToDouble(dr["TOL_PCT"]);
                if (dr["HIGH_LIMIT"] == System.DBNull.Value)
                    tcp.HighLimit = Include.UndefinedDouble;
                else
                    tcp.HighLimit = Convert.ToDouble(dr["HIGH_LIMIT"]);
                if (dr["LOW_LIMIT"] == System.DBNull.Value)
                    tcp.LowLimit = Include.UndefinedDouble;
                else
                    tcp.LowLimit = Convert.ToDouble(dr["LOW_LIMIT"]);

                GLFP.Trend_Caps.Add(tcp);
            }
        }

        /// <summary>
        /// Fills in the plan part of the CubeGroup open parms
        /// </summary>
        private void FillOpenParmForPlan()
        {
            ProfileList versProfList;

            versProfList = SAB.ClientServerSession.GetUserForecastVersions();

            _openParms = new PlanOpenParms(ePlanSessionType.StoreSingleLevel, _computationMode);
            _openParms.SimilarStores = true;
            _openParms.ChainHLPlanProfile.VersionProfile = (VersionProfile)versProfList.FindKey(this.Chain_FV_RID);
            _openParms.ChainHLPlanProfile.NodeProfile = _SAB.HierarchyServerSession.GetNodeData(this.Plan_HN_RID);
            _openParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(this.Plan_HN_RID, (int)eSecurityTypes.Chain);
            _openParms.StoreHLPlanProfile.VersionProfile = (VersionProfile)versProfList.FindKey(this.Plan_FV_RID);
            _openParms.StoreHLPlanProfile.NodeProfile = _SAB.HierarchyServerSession.GetNodeData(this.Plan_HN_RID);
            _openParms.StoreHLPlanProfile.NodeProfile.StoreSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(this.Plan_HN_RID, (int)eSecurityTypes.Store);
            DateRangeProfile drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(this.CDR_RID);
            _openParms.DateRangeProfile = drp;
            _openParms.StoreGroupRID = this.SG_RID;
        }

        /// <summary>
        /// Fills in the basis part of the CubeGroup open parms.
        /// Differenct forecasting function type have different ways of building basis data.
        /// </summary>
        private void FillOpenParmForBasis(GroupLevelFunctionProfile groupLevelFunction)
        {
            BasisProfile basisProfile;
            BasisDetailProfile basisDetailProfile;
            int bdpKey = 1;
            int maxRows = 0;
            int sgl_rid = groupLevelFunction.Key;
            eGroupLevelFunctionType aFunctionType = groupLevelFunction.GLFT_ID;
            //==============================================================================
            // Part of Multi-level forecasting changes
            // Changed this to a local variable so it will start at 1 each time we
            // refresh the basis. Otherwise no values were being returned.
            //==============================================================================
            int bpKey = 1;
            HierarchyNodeProfile hnp = null;
            HierarchyNodeProfile overrideHnp = null;

            maxRows = _dtGroupLevelBasis.Rows.Count;
            // Begin Track #6171
            _Entered_Weight_Values = new double[maxRows, maxRows];
            // END Track #6171
            if (_nodeOverride != Include.NoRID)
            {
                overrideHnp = _SAB.HierarchyServerSession.GetNodeData(_nodeOverride);
            }


            switch (aFunctionType)
            {
                case eGroupLevelFunctionType.PercentContribution:
                    //maxRows = _dtGroupLevelBasis.Rows.Count;	// Track #6171
                    for (int row = 0; row < maxRows; row++)
                    {
                        int basis_sgl_rid = Convert.ToInt32(_dtGroupLevelBasis.Rows[row]["SGL_RID"], CultureInfo.CurrentUICulture);
                        if (basis_sgl_rid == sgl_rid)
                        {
                            // Apply override node to basis, if basis node is NULL
                            if (_nodeOverride != Include.NoRID)
                            {
                                eMerchandiseType merchType = (eMerchandiseType)(Convert.ToInt32(_dtGroupLevelBasis.Rows[row]["MERCH_TYPE"], CultureInfo.CurrentUICulture));
                                if (_dtGroupLevelBasis.Rows[row]["HN_RID"] == DBNull.Value
                                    || merchType == eMerchandiseType.SameNode)
                                    hnp = overrideHnp;
                                else
                                    hnp = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(_dtGroupLevelBasis.Rows[row]["HN_RID"], CultureInfo.CurrentUICulture));
                            }
                            else
                            {
                                if (_dtGroupLevelBasis.Rows[row]["HN_RID"] == DBNull.Value)
                                {
                                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_pl_BasisHierarchyNodeMissing, _sourceModule);
                                    // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                                    //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                                    // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                                    throw new MIDException(eErrorLevel.severe,
                                        (int)eMIDTextCode.msg_pl_BasisHierarchyNodeMissing,
                                        MIDText.GetText(eMIDTextCode.msg_pl_BasisHierarchyNodeMissing));
                                }
                                else
                                {
                                    eMerchandiseType merchType = (eMerchandiseType)(Convert.ToInt32(_dtGroupLevelBasis.Rows[row]["MERCH_TYPE"], CultureInfo.CurrentUICulture));
                                    if (merchType == eMerchandiseType.SameNode)
                                    {
                                        hnp = _SAB.HierarchyServerSession.GetNodeData(this._Plan_HN_RID);
                                    }
                                    else
                                    {
                                        hnp = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(_dtGroupLevelBasis.Rows[row]["HN_RID"], CultureInfo.CurrentUICulture));
                                    }
                                }
                            }

                            basisProfile = new BasisProfile(bpKey++, null, _openParms);
                            basisProfile.BasisType = eTyLyType.NonTyLy;
                            basisDetailProfile = new BasisDetailProfile(bdpKey, _openParms);
                            //Begin Track #4457 - JSmith - Add forecast versions
                            ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
                            basisDetailProfile.VersionProfile = fvpb.Build(Convert.ToInt32(_dtGroupLevelBasis.Rows[row]["FV_RID"]));
                            //End Track #4457
                            basisDetailProfile.HierarchyNodeProfile = hnp;
                            DateRangeProfile drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(Convert.ToInt32(_dtGroupLevelBasis.Rows[row]["CDR_RID"], CultureInfo.CurrentUICulture));
                            basisDetailProfile.DateRangeProfile = drp;

                            // MID End Issue 2390
                            basisDetailProfile.ForecastingInfo = new BasisDetailForecastInfo();
                            basisDetailProfile.ForecastingInfo.PlanWeek = _firstWeekOfBasis; //Issue 4025
                            basisDetailProfile.ForecastingInfo.OrigWeekListCount = basisDetailProfile.GetWeekProfileList(_SAB.ApplicationServerSession).Count;
                            basisDetailProfile.ForecastingInfo.BasisPeriodList = _SAB.ApplicationServerSession.Calendar.GetDateRangePeriods(drp, _firstWeekOfBasis); //Issue 4025
                            basisDetailProfile.IncludeExclude = (eBasisIncludeExclude)Convert.ToInt32(_dtGroupLevelBasis.Rows[row]["INC_EXC_IND"], CultureInfo.CurrentUICulture);
                            if (_dtGroupLevelBasis.Rows[row]["WEIGHT"] == System.DBNull.Value)
                                basisDetailProfile.Weight = 1;
                            else
                                basisDetailProfile.Weight = (float)Convert.ToDouble(_dtGroupLevelBasis.Rows[row]["WEIGHT"], CultureInfo.CurrentUICulture);

                            //// BEGIN MID Track #5954 - KJohnson - Basis weighting, exclusions, and equalizer issues
                            //if (basisDetailProfile.Weight == 0)
                            //{
                            //    basisDetailProfile.Weight = 1;
                            //}
                            //// END MID Track #5954

                            basisProfile.BasisDetailProfileList.Add(basisDetailProfile);
                            _openParms.BasisProfileList.Add(basisProfile);
                        }
                    }
                    break;

                case eGroupLevelFunctionType.TyLyTrend:
                    //maxRows = _dtGroupLevelBasis.Rows.Count;	// Track #6171
                    for (int row = 0; row < maxRows; row++)
                    {
                        int basisSglRid = Convert.ToInt32(_dtGroupLevelBasis.Rows[row]["SGL_RID"], CultureInfo.CurrentUICulture);
                        eTyLyType basisType = (eTyLyType)Convert.ToInt32(_dtGroupLevelBasis.Rows[row]["TYLY_TYPE_ID"], CultureInfo.CurrentUICulture);
                        if (basisSglRid == sgl_rid)
                        {
                            // Apply override node to basis, if basis node is NULL
                            if (_nodeOverride != Include.NoRID)
                            {
                                eMerchandiseType merchType = (eMerchandiseType)(Convert.ToInt32(_dtGroupLevelBasis.Rows[row]["MERCH_TYPE"], CultureInfo.CurrentUICulture));
                                if (_dtGroupLevelBasis.Rows[row]["HN_RID"] == DBNull.Value
                                    || merchType == eMerchandiseType.SameNode)
                                    hnp = overrideHnp;
                                else
                                    hnp = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(_dtGroupLevelBasis.Rows[row]["HN_RID"], CultureInfo.CurrentUICulture));
                            }
                            else
                            {
                                if (_dtGroupLevelBasis.Rows[row]["HN_RID"] == DBNull.Value)
                                {
                                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_pl_BasisHierarchyNodeMissing, _sourceModule);
                                    // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                                    //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                                    // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                                    throw new MIDException(eErrorLevel.severe,
                                        (int)eMIDTextCode.msg_pl_BasisHierarchyNodeMissing,
                                        MIDText.GetText(eMIDTextCode.msg_pl_BasisHierarchyNodeMissing));
                                }
                                else
                                {
                                    eMerchandiseType merchType = (eMerchandiseType)(Convert.ToInt32(_dtGroupLevelBasis.Rows[row]["MERCH_TYPE"], CultureInfo.CurrentUICulture));
                                    if (merchType == eMerchandiseType.SameNode)
                                    {
                                        hnp = _SAB.HierarchyServerSession.GetNodeData(this._Plan_HN_RID);
                                    }
                                    else
                                    {
                                        hnp = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(_dtGroupLevelBasis.Rows[row]["HN_RID"], CultureInfo.CurrentUICulture));
                                    }
                                }
                            }

                            basisProfile = new BasisProfile(bpKey++, null, _openParms);
                            basisProfile.BasisType = basisType;
                            basisDetailProfile = new BasisDetailProfile(bdpKey, _openParms);
                            //Begin Track #4457 - JSmith - Add forecast versions
                            ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
                            basisDetailProfile.VersionProfile = fvpb.Build(Convert.ToInt32(_dtGroupLevelBasis.Rows[row]["FV_RID"]));
                            //End Track #4457
                            basisDetailProfile.HierarchyNodeProfile = hnp;
                            DateRangeProfile drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(Convert.ToInt32(_dtGroupLevelBasis.Rows[row]["CDR_RID"], CultureInfo.CurrentUICulture));
                            basisDetailProfile.DateRangeProfile = drp;

                            // MID End Issue 2390
                            basisDetailProfile.ForecastingInfo = new BasisDetailForecastInfo();
                            basisDetailProfile.ForecastingInfo.PlanWeek = _firstWeekOfBasis; //Issue 4025
                            basisDetailProfile.ForecastingInfo.OrigWeekListCount = basisDetailProfile.GetWeekProfileList(_SAB.ApplicationServerSession).Count;
                            basisDetailProfile.ForecastingInfo.BasisPeriodList = _SAB.ApplicationServerSession.Calendar.GetDateRangePeriods(drp, _firstWeekOfBasis); //Issue 4025
                            basisDetailProfile.IncludeExclude = (eBasisIncludeExclude)Convert.ToInt32(_dtGroupLevelBasis.Rows[row]["INC_EXC_IND"], CultureInfo.CurrentUICulture);
                            //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                            if (basisProfile.BasisType == eTyLyType.ProjectCurrWkSales && groupLevelFunction.Proj_Curr_Wk_Sales_IND)
                            {

                            }
                            //End TT#43 - MD - DOConnell - Projected Sales Enhancement
                            if ((basisProfile.BasisType == eTyLyType.TyLy && groupLevelFunction.TY_Weight_Multiple_Basis_Ind) ||
                                (basisProfile.BasisType == eTyLyType.AlternateLy && groupLevelFunction.LY_Weight_Multiple_Basis_Ind) ||
                                (basisProfile.BasisType == eTyLyType.AlternateApplyTo && groupLevelFunction.Apply_Weight_Multiple_Basis_Ind) ||
                                //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                                (basisProfile.BasisType == eTyLyType.ProjectCurrWkSales && groupLevelFunction.Apply_Weight_Multiple_Basis_Ind))
                            //End TT#43 - MD - DOConnell - Projected Sales Enhancement
                            {
                                basisDetailProfile.Weight = 1;
                                // BEgin Track #6171 - wgted basis issue
                                double wgt = Convert.ToDouble(_dtGroupLevelBasis.Rows[row]["WEIGHT"]);
                                //Begin Track #6293 - KJohnson - Process Frcst Trend using Equalize weighting blows up with index out of bounds.
                                _Entered_Weight_Values[basisProfile.Key - 1, basisDetailProfile.Key - 1] = wgt;
                                //End Track #6293
                                //_Entered_Weight_Values.Add(row, _dtGroupLevelBasis.Rows[row]["WEIGHT"]);
                                // END Track #6171
                            }
                            else if (_dtGroupLevelBasis.Rows[row]["WEIGHT"] == System.DBNull.Value)
                            // BEGIN TT#279-MD - stodd - Projected Sales 
                            {
                                basisDetailProfile.Weight = 1;
                            }
                            else
                            {
                                if (basisProfile.BasisType == eTyLyType.ProjectCurrWkSales && !_applyToWeightsFilled)
                                {
                                    _applyToWeights.Add(basisProfile.Key, Convert.ToDouble(_dtGroupLevelBasis.Rows[row]["WEIGHT"]));
                                    basisDetailProfile.Weight = 1;
                                }
                                else
                                {
                                    // BEGIN TT#2634 - stodd - Weighted basis not producing expected results
                                    if (_applyToWeights.ContainsKey(basisProfile.Key))
                                    {
                                        basisDetailProfile.Weight = 1;
                                    }
                                    else
                                    {
                                        basisDetailProfile.Weight = (float)Convert.ToDouble(_dtGroupLevelBasis.Rows[row]["WEIGHT"], CultureInfo.CurrentUICulture);
                                    }
                                    // END TT#2634 - stodd - Weighted basis not producing expected results
                                }
                            }
                            // END TT#279-MD - stodd - Projected Sales 
                            //// BEGIN MID Track #5954 - KJohnson - Basis weighting, exclusions, and equalizer issues
                            //if (basisDetailProfile.Weight == 0)
                            //{
                            //    basisDetailProfile.Weight = 1;
                            //}
                            //// END MID Track #5954

                            basisProfile.BasisDetailProfileList.Add(basisDetailProfile);
                            _openParms.BasisProfileList.Add(basisProfile);
                        }
                    }

                    // Begin TT#2588 - JSmith - Equalize weighting set to "no" produces an error with proj sales checked
                    if (_applyToWeights.Count > 0)
                    {
                        _applyToWeightsFilled = true;
                    }
                    // End TT#2588 - JSmith - Equalize weighting set to "no" produces an error with proj sales checked

                    break;

                default:

                    break;
            } // end switch
        }


        /// <summary>
        /// Returns the aggregate store values of the all the basis defined 
        /// </summary>
        /// <param name="weekInPlan"></param>
        /// <param name="planWeek"></param>
        /// <param name="aBasisType"></param>
        /// <returns></returns>
        public ArrayList ReadStoreBasisValues(int weekInPlan, WeekProfile planWeek, eTyLyType aBasisType, ModelVariableProfile aVariable)
        {
            WeekProfile fromWeek = planWeek;
            ArrayList masterValueList = new ArrayList();
            if (MONITOR)
            {
                _forecastMonitor.WriteLine("Basis Information -- " +
                    "Basis Type: " + aBasisType.ToString());
            }

            // BEGIN issue 4406 - stodd 5/14/2007
            //===================================================================================
            // Sales Reg variable substitution logic
            //-----------------------------------------------------------------------------------
            // If the variable is Sales Reg, we actually read the Sales Reg Promo variable 
            // to get the basis values.
            //===================================================================================
            if (aVariable.Key == _cubeGroup.Transaction.PlanComputations.PlanVariables.SalesRegularUnitsVariable.Key)
            {
                aVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.SalesRegPromoUnitsVariable.Key);
            }
            // END issue 4406

            foreach (BasisProfile bp in _openParms.BasisProfileList.ArrayList)
            {
                if (bp.BasisType != aBasisType)
                    continue;
                //***************************************************************************
                // For each basis line, create a temp basis list.  
                // The temp basis lists are then accumulated.
                // Dynamic:  if a basis date range is Dynamic AND relative to plan,
                // The weeks must move as the week to plan moves.
                //***************************************************************************
                for (int i = 0; i < bp.BasisDetailProfileList.Count; i++)
                {
                    BasisDetailProfile bdp = (BasisDetailProfile)bp.BasisDetailProfileList[i];

                    if (MONITOR)
                    {
                        ForecastVersion fv = new ForecastVersion();
                        bdp.DateRangeProfile.InternalAnchorDate = planWeek;
                        _SAB.ApplicationServerSession.Calendar.GetDisplayDate(bdp.DateRangeProfile);
                        string dateRange = bdp.DateRangeProfile.DisplayDate + "(" + bdp.DateRangeProfile.Key.ToString(CultureInfo.CurrentUICulture);
                        if (bdp.DateRangeProfile.DateRangeType == eCalendarRangeType.Static)
                            dateRange += ",Static)";
                        else if (bdp.DateRangeProfile.DateRangeType == eCalendarRangeType.Dynamic)
                            dateRange += ",Dynamic," + bdp.DateRangeProfile.RelativeTo.ToString() + ")";
                        else if (bdp.DateRangeProfile.DateRangeType == eCalendarRangeType.Reoccurring)
                            dateRange += ",Reoccurring)";

                        // BEGIN TT#279-MD - stodd - Projected Sales 
                        double basisWgt = 0;
                        if (bp.BasisType == eTyLyType.ProjectCurrWkSales)
                        {
                            basisWgt = _applyToWeights[bp.Key];
                        }
                        else
                        {
                            basisWgt = bdp.Weight;
                        }
                        // END TT#279-MD - stodd - Projected Sales 
                        _forecastMonitor.WriteLine("  " +
                            bdp.HierarchyNodeProfile.NodeID + "(" + bdp.HierarchyNodeProfile.Key.ToString(CultureInfo.CurrentUICulture) + ")  " +
                            fv.GetVersionText(bdp.VersionProfile.Key) + "(" +
                            bdp.VersionProfile.Key.ToString(CultureInfo.CurrentUICulture) + ")" + "  " +
                            dateRange + "  wgt: " +
                            // BEGIN TT#279-MD - stodd - Projected Sales 
                            basisWgt + "  " + bdp.IncludeExclude.ToString());
                        // END TT#279-MD - stodd - Projected Sales 
                    }

                    //==========================================================================================================
                    // Begin MID Issue 2476
                    //==========================================================================================================
                    //					if (bdp.DateRangeProfile.DateRangeType == eCalendarRangeType.Dynamic 
                    //						&& bdp.DateRangeProfile.RelativeTo == DataCommon.eDateRangeRelativeTo.Plan)
                    //					{	

                    ProfileList weekList = GetWeekList(weekInPlan, fromWeek, bdp);

                    //====================================================================================
                    // IF the apply to date is a period, we only want to use the first we of the period...
                    //====================================================================================
                    if (aBasisType == eTyLyType.AlternateApplyTo)
                    {
                        WeekProfile firstWeek = (WeekProfile)weekList[0];
                        weekList.Clear();
                        weekList.Add(firstWeek);
                    }

                    //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
                    if (aBasisType == eTyLyType.ProjectCurrWkSales)
                    {
                        int currweekRID = _SAB.ApplicationServerSession.Calendar.CurrentWeek.Key;
                        WeekProfile procWeek = (WeekProfile)weekList[0];
                        if (procWeek.Key == currweekRID)
                        {
                            weekList.Clear();
                            weekList.Add(procWeek);
                        }
                    }
                    //END TT#43 - MD - DOConnell - Projected Sales Enhancement
                    if (MONITOR)
                    {
                        // Begin TT#1332-MD - JSmith - OTS Forecast - Basis is Month dynamic to Plan
                        //int fromDt = ((WeekProfile)weekList[0]).YearWeek;
                        //int toDt = ((WeekProfile)weekList[weekList.Count - 1]).YearWeek;
                        int basisFromKey = bdp.GetBasisWeekIdFromPlanWeekId(_SAB.ApplicationServerSession, ((WeekProfile)weekList[0]).Key);
                        int basistoKey = bdp.GetBasisWeekIdFromPlanWeekId(_SAB.ApplicationServerSession, ((WeekProfile)weekList[weekList.Count - 1]).Key);
                        int fromDt = _SAB.ApplicationServerSession.Calendar.GetWeek(basisFromKey).YearWeek;
                        int toDt = _SAB.ApplicationServerSession.Calendar.GetWeek(basistoKey).YearWeek;
                        // End TT#1332-MD - JSmith - OTS Forecast - Basis is Month dynamic to Plan
                        string msg = "    " + bp.Key.ToString() + "-" + bdp.Key.ToString() + " " + fromDt.ToString() + " - " + toDt.ToString();
                        _forecastMonitor.WriteLine(msg);
                    }
                    //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                    //ArrayList tempBasisList = ReadBasisDetailValues(bp.Key, bdp, weekList, aVariable);
                    // BEGIN TT#279-MD - stodd - Projected Sales 
                    ArrayList tempBasisList = ReadBasisDetailValues(bp.Key, bp, bdp, weekList, aVariable, aBasisType);
                    if (MONITOR)
                    {
                        if (aBasisType == eTyLyType.ProjectCurrWkSales)
                        {
                            // for formatting
                            _forecastMonitor.WriteLine("  ");
                        }
                    }
                    // END TT#279-MD - stodd - Projected Sales 
                    //End TT#43 - MD - DOConnell - Projected Sales Enhancement
                    AddArrayLists(masterValueList, tempBasisList);
                    //===========================================
                    // End MID Issue 2476
                    //===========================================
                }
            }

            return masterValueList;

        }

        // BEGIN Track #5874 stodd 
        // When forecasting other variables, sometimes the Sales variable was done 
        // using TY/LY Trend, so no proportional basis existed. This determines what
        // type of basis is present.
        internal eTyLyType GetBasisType()
        {
            eTyLyType basisType = eTyLyType.NonTyLy;
            foreach (BasisProfile bp in _openParms.BasisProfileList.ArrayList)
            {
                if (bp.BasisType == eTyLyType.TyLy)
                {
                    basisType = eTyLyType.TyLy;
                }
            }
            return basisType;
        }
        // End Track #5874 stodd 
        // Begin Issue #4286 - Stodd 

        /// <summary>
        /// Only used by Proportional to keep period alignment correct when weeks are skipped due
        /// to zero chain amounts.
        /// </summary>
        /// <param name="weekInPlan"></param>
        /// <param name="planWeek"></param>
        /// <param name="aBasisType"></param>
        public void HandleBasisPeriodShift(int weekInPlan, WeekProfile planWeek, eTyLyType aBasisType)
        {
            WeekProfile fromWeek = planWeek;
            foreach (BasisProfile bp in _openParms.BasisProfileList.ArrayList)
            {
                if (bp.BasisType != aBasisType)
                    continue;
                for (int i = 0; i < bp.BasisDetailProfileList.Count; i++)
                {
                    BasisDetailProfile bdp = (BasisDetailProfile)bp.BasisDetailProfileList[i];
                    //=====================================================
                    // The GetWeekList takes care of adjusting periods
                    //=====================================================
                    ProfileList weekList = GetWeekList(weekInPlan, fromWeek, bdp);

                    if (MONITOR)
                    {
                        int fromDt = ((WeekProfile)weekList[0]).YearWeek;
                        int toDt = ((WeekProfile)weekList[weekList.Count - 1]).YearWeek;
                        string msg = "    " + bp.Key.ToString() + "-" + bdp.Key.ToString() + " " + fromDt.ToString() + " - " + toDt.ToString();
                        _forecastMonitor.WriteLine(msg);
                    }
                }
            }
        }

        // End Issue #4286 - Stodd 

        internal bool GetDoAnyStoresOpenOrCloseDuringThisPlanWeek(WeekProfile planWeek)
        {
            int storeCount = _allStoreList.Count;
            bool anyChanges = false;

            for (int i = 0; i < storeCount; i++)
            {
                StoreProfile sp = (StoreProfile)_allStoreList[i];
                if (sp.SellingOpenDt != Include.UndefinedDate)
                {
                    WeekProfile SellingOpenWeek = this._SAB.ApplicationServerSession.Calendar.GetWeek(sp.SellingOpenDt);
                    if (SellingOpenWeek.Key == planWeek.Key)
                    {
                        anyChanges = true;
                        break;
                    }
                }
                if (sp.SellingCloseDt != Include.UndefinedDate)
                {
                    WeekProfile SellingCloseWeek = this._SAB.ApplicationServerSession.Calendar.GetWeek(sp.SellingCloseDt);
                    if (SellingCloseWeek.Key == planWeek.Key)
                    {
                        anyChanges = true;
                        break;
                    }
                }
            }

            return anyChanges;
        }

        internal void DisplayPlanWeekAlignment()
        {
            if (MONITOR)
            {
                ProfileList PlanWeeksList = _openParms.GetWeekProfileList(_SAB.ApplicationServerSession);
                MonitorWeekList("Plan", PlanWeeksList);
                foreach (BasisProfile bp in _openParms.BasisProfileList.ArrayList)
                {
                    for (int i = 0; i < bp.BasisDetailProfileList.Count; i++)
                    {
                        ProfileList basisWeekList = new ProfileList(eProfileType.Week);
                        BasisDetailProfile bdp = (BasisDetailProfile)bp.BasisDetailProfileList[i];
                        foreach (WeekProfile wp in PlanWeeksList.ArrayList)
                        {
                            int basisWeekRid = bdp.GetBasisWeekIdFromPlanWeekId(_SAB.ApplicationServerSession, wp.Key);
                            WeekProfile basisWeek = _SAB.ApplicationServerSession.Calendar.GetWeek(basisWeekRid);
                            basisWeekList.Add(basisWeek);
                        }
                        MonitorWeekList("Basis " + bp.Key + ":" + bdp.Key + " ", basisWeekList);
                    }
                }
            }
        }


        private void MonitorWeekList(string caption, ProfileList weekList)
        {
            if (MONITOR)
            {
                string printLine;
                if (caption.Length > 12)
                    caption = caption.Substring(0, 12);
                else if (caption.Length < 12)
                    caption = caption.PadRight(12, ' ');
                printLine = caption;

                foreach (WeekProfile wp in weekList.ArrayList)
                {
                    printLine += " " + wp.YearWeek.ToString();
                }
                _forecastMonitor.WriteLine(printLine);
            }
        }

        /// <summary>
        /// resolves what the weeklist for the data will be
        /// </summary>
        /// <param name="fromWeek"></param>
        /// <param name="bdp"></param>
        /// <returns></returns>
        private ProfileList GetWeekList(int weekInPlan, WeekProfile fromWeek, BasisDetailProfile bdp)
        {
            //ProfileList weekList = null;
            // Begin TT#184 - JSmith - WHen processing 1 wk at a time, the basis period logic to bump the basis ahead is not working.
            int WeekListCount = bdp.ForecastingInfo.OrigWeekListCount;
            // End TT#184 - JSmith - WHen processing 1 wk at a time, the basis period logic to bump the basis ahead is not working.

            if (bdp.DateRangeProfile.DateRangeType == eCalendarRangeType.Dynamic
                && bdp.DateRangeProfile.RelativeTo == DataCommon.eDateRangeRelativeTo.Plan)
            {
                if (bdp.DateRangeProfile.SelectedDateType == eCalendarDateType.Period)
                {
                    WeekProfile PriorWeek = _SAB.ApplicationServerSession.Calendar.Add(fromWeek, -1);

                    if (fromWeek.Period != PriorWeek.Period && fromWeek != this.FirstWeekOfPlan)
                    {
                        fromWeek = bdp.ForecastingInfo.ShiftDateRange(_SAB.ApplicationServerSession.Calendar);
                        // Begin TT#184 - JSmith - WHen processing 1 wk at a time, the basis period logic to bump the basis ahead is not working.
                        _firstWeekOfBasis = fromWeek;
                        // End TT#184 - JSmith - WHen processing 1 wk at a time, the basis period logic to bump the basis ahead is not working.
                    }
                    else
                    {
                        // Begin TT#184 - JSmith - WHen processing 1 wk at a time, the basis period logic to bump the basis ahead is not working.
                        // Begin TT#1332-MD - JSmith - OTS Forecast - Basis is Month dynamic to Plan
                        fromWeek = bdp.ForecastingInfo.PlanWeek;
                        //fromWeek = (WeekProfile)fromWeek.Period.Weeks[0];
                        // End TT#1332-MD - JSmith - OTS Forecast - Basis is Month dynamic to Plan
                        // End TT#184 - JSmith - WHen processing 1 wk at a time, the basis period logic to bump the basis ahead is not working.
                    }
                    // Begin TT#184 - JSmith - WHen processing 1 wk at a time, the basis period logic to bump the basis ahead is not working.
                    ProfileList periodProfileList = _SAB.ApplicationServerSession.Calendar.GetPeriodRange(bdp.DateRangeProfile, fromWeek);
                    WeekListCount = 0;
                    foreach (MonthProfile monthProfile in periodProfileList)
                    {
                        WeekListCount += monthProfile.Weeks.Count;
                    }
                    // End TT#184 - JSmith - WHen processing 1 wk at a time, the basis period logic to bump the basis ahead is not working.
                }
            }
            else
            {
                fromWeek = this._firstWeekOfBasis; //Issue 4025
            }

            // Begin TT#184 - JSmith - WHen processing 1 wk at a time, the basis period logic to bump the basis ahead is not working.
            //WeekProfile toWeek = _SAB.ApplicationServerSession.Calendar.Add(fromWeek, bdp.ForecastingInfo.OrigWeekListCount - 1);
            WeekProfile toWeek = _SAB.ApplicationServerSession.Calendar.Add(fromWeek, WeekListCount - 1);
            // End TT#184 - JSmith - WHen processing 1 wk at a time, the basis period logic to bump the basis ahead is not working.
            _weekList = _SAB.ApplicationServerSession.Calendar.GetWeekRange(fromWeek, toWeek);

            return _weekList;
        }

        // BEGIN MID Track #4370 - John Smith - FWOS Models
        private void BalanceSales(ModelVariableProfile aVariable, ArrayList aWeeksToPlan)
        // END MID Track #4370
        {
            // BEGIN TT#1413 - DOConnell - Chain Plan - Set Percentages 
            // Get product Chain Set Percentages attribute values
            int MaxWeek = 0;
            int MinWeek = 0;
            int aYearWeekId = 0;
            int bYearWeekId = 0;
            int zeroCells = 0;
            bool chainSetPercent = false;
            bool zeroOrLockedPlanValues = false;
            double Percent;
            foreach (WeekProfile planWeek in aWeeksToPlan)
            {
                if (Convert.ToInt32(planWeek.YearWeek) < 1000000)  // YYYYWW format
                {
                    // convert to YYYYDDD
                    aYearWeekId = this.SAB.ApplicationServerSession.Calendar.GetWeekKey(Convert.ToInt32(planWeek.YearWeek));

                }
                int CurWeek = aYearWeekId;

                if (MinWeek == 0) MinWeek = aYearWeekId;

                if (CurWeek < MinWeek)
                {
                    MinWeek = CurWeek;
                }
                else if (CurWeek > MaxWeek)
                {
                    MaxWeek = CurWeek;
                }
            }

            //get the list of Sets within the Chain and the Percent for each
            //BEGIN TT#1628 - DOConnell - Chain Set Percentage (API) - CSP/API does not insert zero percentages nor balance out on the OTS Forecast Review.
            if ((_ChainSetPercentValues == null) || (_ChainSetPercentValues.Rows.Count <= 0))
            {
                //Begin TT#1693 - DOConnell - Chain Set Percent - OTS Forecast not using Node Hierarchy to access data
                //_ChainSetPercentValues = _OTSPlanData.GetChainSetPercentByDate(MinWeek, MaxWeek, _Plan_HN_RID, _originalSGRid);
                _storeList = StoreMgmt.StoreGroup_GetLevelListFilled(_originalSGRid); //_SAB.StoreServerSession.GetStoreGroupLevelList(_originalSGRid);
                _weekList = _SAB.ApplicationServerSession.Calendar.GetWeekRange(MinWeek, MaxWeek);
                _chainSetPercentList = _SAB.HierarchyServerSession.GetChainSetPercentList(_storeList, _Plan_HN_RID, false, false, false, _weekList);
            }
            ArrayList pctList = new ArrayList();
            var ChainList = new List<ChainSetPercentValues>();
            var KeyList = new List<ChainSetKeyValues>();
            //var KeyList2 = new List<ChainSetKeyValues>();
            var WeekList = new List<ChainSetKeyValues>();

            foreach (ChainSetPercentProfiles cspp in _chainSetPercentList)
            {
                //if (cspp.StoreGroupRID == _originalSGRid)
                //{
                Percent = Convert.ToDouble(cspp.ChainSetPercent);
                ChainSetKeyValues StoreKeyList = new ChainSetKeyValues(cspp.StoreGroupLevelRID, cspp.StoreGroupRID, Percent, cspp.TimeID);

                KeyList.Add(StoreKeyList);
                //}                       
            }

            //foreach (DataRow dr in _ChainSetPercentValues.Rows)
            //{
            //    try
            //    {
            //            Percent = Convert.ToDouble(dr["PERCENTAGE"]);
            //    }
            //    catch
            //    {
            //        Percent = 0;
            //    }
            //        pctList.Add(Percent);
            //        ChainSetKeyValues StoreKeyList = new ChainSetKeyValues(Convert.ToInt32(dr["STOREGROUPLEVELRID"]),
            //                                                                Convert.ToInt32(dr["STOREGROUPRID"]),
            //                                                                Percent,
            //                                                               Convert.ToInt32(dr["TIMEID"]));
            //        KeyList.Add(StoreKeyList);

            //}
            //End TT#1628 - DOConnell - Chain Set Percentage (API) - CSP/API does not insert zero percentages nor balance out on the OTS Forecast Review.
            //End TT#1693 - DOConnell - Chain Set Percent - OTS Forecast not using Node Hierarchy to access data
            KeyList.Sort();
            foreach (WeekProfile planWeek in aWeeksToPlan)
            {
                if (planWeek.YearWeek < 1000000)  // YYYYWW format
                {
                    // convert to YYYYDDD
                    aYearWeekId = this.SAB.ApplicationServerSession.Calendar.GetWeekKey(planWeek.YearWeek);
                }
                if (KeyList != null && KeyList.Count > 0)
                {
                    for (int a = 0; a < KeyList.Count; ++a)
                    {
                        //Begin TT#1693 - DOConnell - Chain Set Percent - OTS Forecast not using Node Hierarchy to access data
                        if (KeyList[a].Week < 1000000)  // YYYYWW format
                        {
                            // convert to YYYYDDD
                            bYearWeekId = this.SAB.ApplicationServerSession.Calendar.GetWeekKey(KeyList[a].Week);
                        }
                        if (bYearWeekId == aYearWeekId)
                            //if (KeyList[a].Week == aYearWeekId)
                            //End TT#1693 - DOConnell - Chain Set Percent - OTS Forecast not using Node Hierarchy to access data
                            WeekList.Add(KeyList[a]);
                    }
                    if (WeekList != null && WeekList.Count > 0)
                    {
                        chainSetPercent = true;
                    }
                    else
                    {
                        chainSetPercent = false;
                    }
                }

                if (chainSetPercent)
                {
                    if (MONITOR)
                    {
                        _forecastMonitor.WriteLine(" ");
                        _forecastMonitor.WriteLine("Chain Set Week: " + planWeek.Text());
                    }
                    //*************************************
                    // Get chain plan value
                    //*************************************
                    double chainValue = ReadChainValue(planWeek, _salesVariable);

                    //Balance the Sets by Chain % and return the stock amount for each Set
                    WeekList.Sort();
                    BalanceChainSetPercent(WeekList, chainValue, planWeek, eVariableType.ChainSetPercent, ChainList);
                    ChainList.Sort();

                    //loop through Sets and get the inventory values for each store in the set
                    for (int s = 0; s < WeekList.Count; s++)
                    {

                        ProfileList storeList = StoreMgmt.StoreGroupLevel_GetStoreProfileList(WeekList[s].Value, ChainList[s].Index); //_SAB.StoreServerSession.GetStoresInGroup(WeekList[s].Value, ChainList[s].Index);
                        double storeValue = ChainList[s].Value;
                        string StoreSetName = Convert.ToString(StoreMgmt.StoreGroupLevel_GetName(WeekList[s].Value, WeekList[s].Index)); //_SAB.StoreServerSession.GetGroupSetName(WeekList[s].Value, WeekList[s].Index));
                        bool lockedCells;
                        bool nonZeroCells;
                        ArrayList storePlanValues = ReadStoreValues(planWeek, _salesVariable);
                        ArrayList CellReferenceList = new ArrayList();
                        for (int spv = 0; spv < storeList.Count; spv++)
                        {
                            lockedCells = false;
                            nonZeroCells = false;
                            StoreProfile sp = (StoreProfile)_allStoreList.FindKey(storeList[spv].Key);
                            int storeIdx = _allStoreList.ArrayList.IndexOf(sp);

                            PlanCellReference cr = (PlanCellReference)storePlanValues[storeIdx];
                            if (cr.isCellLocked)
                                lockedCells = true;
                            if (cr.CurrentCellValue != 0.0)
                                nonZeroCells = true;


                            if (lockedCells || nonZeroCells)
                            {
                                CellReferenceList.Add(cr);
                            }

                            else
                            {
                                zeroCells = ++zeroCells;
                                if (zeroCells == storeList.Count)
                                {
                                    zeroOrLockedPlanValues = true;
                                    string msg = MIDText.GetText(eMIDTextCode.msg_CSPNoBalanceNeeded);
                                    msg = msg.Replace("{0}", planWeek.YearWeek.ToString());
                                    msg = msg.Replace("{1}", StoreSetName);
                                    msg = msg.Replace("{2}", _salesVariable.VariableProfile.VariableName);
                                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msg, this.ToString());
                                    if (MONITOR)
                                    {
                                        _forecastMonitor.WriteLine(msg);
                                    }
                                }
                            }
                        }
                        // Balance each store in the Set with the inventory value for the Set
                        if (!zeroOrLockedPlanValues)
                            Balance(CellReferenceList, storeValue, planWeek, eVariableType.Sales);
                        zeroCells = 0;
                        zeroOrLockedPlanValues = false;
                    }
                    ChainList.Clear();
                    WeekList.Clear();
                }

                else
                {
                    // END TT#1413 - DOConnell - Chain Plan - Set Percentages
                    //*************************************
                    // Get chain plan value
                    //*************************************
                    double chainValue = ReadChainValue(planWeek, _salesVariable);

                    _weekBeingPlanned = planWeek; // Track #6187
                                                  //*************************************
                                                  // Get store sales values
                                                  //*************************************
                    ArrayList storePlanValues = ReadStoreValues(planWeek, _salesVariable);

                    // BEGIN Issue 5141 stodd 1.28.2008
                    bool lockedCells = false;
                    bool nonZeroCells = false;
                    for (int s = 0; s < storePlanValues.Count; s++)
                    {
                        PlanCellReference cr = (PlanCellReference)storePlanValues[s];
                        if (cr.isCellLocked)
                            lockedCells = true;
                        if (cr.CurrentCellValue != 0.0)
                            nonZeroCells = true;
                    }

                    if (lockedCells || nonZeroCells)
                    {
                        Balance(storePlanValues, chainValue, planWeek, eVariableType.Sales);
                    }
                    else
                    {
                        string msg = MIDText.GetText(eMIDTextCode.msg_NoBalanceNeeded);
                        msg = msg.Replace("{0}", planWeek.YearWeek.ToString());
                        msg = msg.Replace("{1}", _salesVariable.VariableProfile.VariableName);
                        _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msg, this.ToString());
                        if (MONITOR)
                        {
                            _forecastMonitor.WriteLine(msg);
                        }
                    }
                    // END Issue 5141
                }
            }
        }
        // BEGIN MID Track #4370 - John Smith - FWOS Models
        private bool BalanceInventory(ModelVariableProfile aVariable, ArrayList aWeeksToPlan)
        // END MID Track #4370
        {
            // Begin Track #6289
            bool showBalanceLog = false;
            // End Track #6289


            // BEGIN TT#1413 - DOConnell - Chain Plan - Set Percentages 
            // Get product Chain Set Percentages attribute values
            bool chainSetPercent = false;
            int MaxWeek = 0;
            int MinWeek = 0;
            int aYearWeekId = 0;
            int bYearWeekId = 0;
            int zeroCells = 0;
            double Percent;
            bool zeroOrLockedPlanValues = false;
            //get the max and min week 
            foreach (WeekProfile planWeek in aWeeksToPlan)
            {
                if (planWeek.YearWeek < 1000000)  // YYYYWW format
                {
                    // convert to YYYYDDD
                    aYearWeekId = this.SAB.ApplicationServerSession.Calendar.GetWeekKey(planWeek.YearWeek);
                }

                int CurWeek = aYearWeekId;
                if (MinWeek == 0) MinWeek = aYearWeekId;
                if (CurWeek < MinWeek)
                {
                    MinWeek = CurWeek;
                }
                else if (CurWeek > MaxWeek)
                {
                    MaxWeek = CurWeek;
                }
            }

            //get the list of Sets within the Chain and the Percent for each
            //BEGIN TT#1628 - DOConnell - Chain Set Percentage (API) - CSP/API does not insert zero percentages nor balance out on the OTS Forecast Review.
            if ((_ChainSetPercentValues == null) || (_ChainSetPercentValues.Rows.Count <= 0))
            {
                //Begin TT#1693 - DOConnell - Chain Set Percent - OTS Forecast not using Node Hierarchy to access data
                //_ChainSetPercentValues = _OTSPlanData.GetChainSetPercentByDate(MinWeek, MaxWeek, _Plan_HN_RID, _originalSGRid);
                _storeList = StoreMgmt.StoreGroup_GetLevelListFilled(_originalSGRid); //_SAB.StoreServerSession.GetStoreGroupLevelList(_originalSGRid);
                _weekList = _SAB.ApplicationServerSession.Calendar.GetWeekRange(MinWeek, MaxWeek);
                _chainSetPercentList = _SAB.HierarchyServerSession.GetChainSetPercentList(_storeList, _Plan_HN_RID, false, false, false, _weekList);
            }
            ArrayList pctList = new ArrayList();
            var ChainList = new List<ChainSetPercentValues>();
            var KeyList = new List<ChainSetKeyValues>();
            var WeekList = new List<ChainSetKeyValues>();

            foreach (ChainSetPercentProfiles cspp in _chainSetPercentList)
            {
                Percent = Convert.ToDouble(cspp.ChainSetPercent);
                ChainSetKeyValues StoreKeyList = new ChainSetKeyValues(cspp.StoreGroupLevelRID, cspp.StoreGroupRID, Percent, cspp.TimeID);

                KeyList.Add(StoreKeyList);
            }

            //foreach (DataRow dr in _ChainSetPercentValues.Rows)
            //{
            //    try
            //    {

            //        Percent = Convert.ToDouble(dr["PERCENTAGE"]);
            //    }
            //    catch
            //    {
            //        Percent = 0;
            //    }
            //    pctList.Add(Percent);
            //    ChainSetKeyValues StoreKeyList = new ChainSetKeyValues(Convert.ToInt32(dr["STOREGROUPLEVELRID"]),
            //                                                            Convert.ToInt32(dr["STOREGROUPRID"]),
            //                                                            Percent,
            //                                                           Convert.ToInt32(dr["TIMEID"]));

            //    KeyList.Add(StoreKeyList);
            //}
            //End TT#1628 - DOConnell - Chain Set Percentage (API) - CSP/API does not insert zero percentages nor balance out on the OTS Forecast Review.
            //End TT#1693 - DOConnell - Chain Set Percent - OTS Forecast not using Node Hierarchy to access data
            KeyList.Sort();
            foreach (WeekProfile planWeek in aWeeksToPlan)
            {
                if (planWeek.YearWeek < 1000000)  // YYYYWW format
                {
                    // convert to YYYYDDD
                    aYearWeekId = this.SAB.ApplicationServerSession.Calendar.GetWeekKey(planWeek.YearWeek);
                }
                if (KeyList != null && KeyList.Count > 0)
                {
                    for (int a = 0; a < KeyList.Count; ++a)
                    {
                        //Begin TT#1693 - DOConnell - Chain Set Percent - OTS Forecast not using Node Hierarchy to access data
                        if (KeyList[a].Week < 1000000)  // YYYYWW format
                        {
                            // convert to YYYYDDD
                            bYearWeekId = this.SAB.ApplicationServerSession.Calendar.GetWeekKey(KeyList[a].Week);
                        }
                        //if (KeyList[a].Week == aYearWeekId)
                        if (bYearWeekId == aYearWeekId)
                            //End TT#1693 - DOConnell - Chain Set Percent - OTS Forecast not using Node Hierarchy to access data
                            WeekList.Add(KeyList[a]);
                    }
                    if (WeekList != null && WeekList.Count > 0)
                    {
                        chainSetPercent = true;
                    }
                    else
                    {
                        chainSetPercent = false;
                    }
                }


                if (chainSetPercent)
                {
                    if (MONITOR)
                    {
                        _forecastMonitor.WriteLine(" ");
                        _forecastMonitor.WriteLine("Chain Set Week: " + planWeek.Text());
                    }
                    //Get the Chain Inventory Value
                    double chainValue = ReadChainValue(planWeek, _stockVariable);

                    //Balance the Sets by Chain % and return the stock amount for each Set
                    WeekList.Sort();
                    BalanceChainSetPercent(WeekList, chainValue, planWeek, eVariableType.ChainSetPercent, ChainList);
                    ChainList.Sort();

                    //loop through Sets and get the inventory values for each store in the set
                    for (int s = 0; s < WeekList.Count; s++)
                    {
                        ProfileList storeList = StoreMgmt.StoreGroupLevel_GetStoreProfileList(WeekList[s].Value, ChainList[s].Index); //_SAB.StoreServerSession.GetStoresInGroup(WeekList[s].Value, ChainList[s].Index);
                        double storeValue = ChainList[s].Value;
                        string StoreSetName = Convert.ToString(StoreMgmt.StoreGroupLevel_GetName(WeekList[s].Value, WeekList[s].Index)); //_SAB.StoreServerSession.GetGroupSetName(WeekList[s].Value, WeekList[s].Index));
                        bool lockedCells;
                        bool nonZeroCells;
                        ArrayList storePlanValues = ReadStoreValues(planWeek, _stockVariable);
                        ArrayList CellReferenceList = new ArrayList();
                        for (int spv = 0; spv < storeList.Count; spv++)
                        {
                            lockedCells = false;
                            nonZeroCells = false;
                            StoreProfile sp = (StoreProfile)_allStoreList.FindKey(storeList[spv].Key);

                            int storeIdx = _allStoreList.ArrayList.IndexOf(sp);

                            PlanCellReference cr = (PlanCellReference)storePlanValues[storeIdx];
                            if (cr.isCellLocked)
                                lockedCells = true;
                            if (cr.CurrentCellValue != 0.0)
                                nonZeroCells = true;

                            if ((Bal_Stock_Ind && (lockedCells || nonZeroCells))
                                ||
                               (!nonZeroCells && _prorateChainStock))
                            {
                                // Balance each store in the Set to the Chain Set Percent
                                CellReferenceList.Add(cr);
                            }

                            else
                            {
                                zeroCells = ++zeroCells;
                                if (zeroCells == storeList.Count)
                                {
                                    zeroOrLockedPlanValues = true;
                                    string msg = MIDText.GetText(eMIDTextCode.msg_CSPNoBalanceNeeded);
                                    msg = msg.Replace("{0}", planWeek.YearWeek.ToString());
                                    msg = msg.Replace("{1}", StoreSetName);
                                    msg = msg.Replace("{2}", _stockVariable.VariableProfile.VariableName);
                                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msg, this.ToString());
                                    if (MONITOR)
                                    {

                                        _forecastMonitor.WriteLine(msg);
                                    }
                                }
                            }
                        }
                        // Balance each store in the Set with the inventory value for the Set
                        bool balanced = false;
                        int attempts = 0;
                        while (!balanced && !zeroOrLockedPlanValues)
                        {
                            ++attempts;
                            balanced = Balance(CellReferenceList, storeValue, planWeek, eVariableType.BegStock);
                            if (!balanced &&
                                attempts > _maxBalanceAttempts)
                            {
                                balanced = true;
                                string msg = MIDText.GetText(eMIDTextCode.msg_BalanceAttemptsExceeded);
                                msg = msg.Replace("{0}", _maxBalanceAttempts.ToString());
                                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msg, this.ToString());
                                if (MONITOR)
                                {
                                    _forecastMonitor.WriteLine(msg);
                                }
                            }
                            zeroCells = 0;
                            zeroOrLockedPlanValues = false;
                        }
                    }

                    ChainList.Clear();
                    WeekList.Clear();
                    showBalanceLog = true;
                }

                //if Chain Set Percent data does not exist process as normal
                else
                {
                    // Begin Track 6280 stodd - don't balance current inventory week
                    ForecastVersion fv = new ForecastVersion();
                    bool versionProtected = fv.GetIsVersionProtected(Plan_FV_RID);
                    WeekProfile currentWeek = _SAB.ApplicationServerSession.Calendar.CurrentWeek;
                    if (planWeek == currentWeek && versionProtected)  // compares keys
                    {
                        string errorMsg = MIDText.GetText(eMIDTextCode.msg_VersionProtectedNoInventoryPlanned);
                        errorMsg = errorMsg.Replace("{0}", planWeek.YearWeek.ToString(CultureInfo.CurrentUICulture));
                        errorMsg = errorMsg.Replace("{1}", aVariable.VariableProfile.VariableName);
                        errorMsg = errorMsg.Replace("{2}", "balanced");
                        _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, errorMsg, _sourceModule);
                        if (MONITOR)
                        {
                            _forecastMonitor.WriteLine("WARNING - " + errorMsg);
                        }
                        continue;
                    }
                    // Begin Track 6280 stodd - don't balance current inventory week
                    _weekBeingPlanned = planWeek; // Track #6187
                    //*************************************
                    // Get store stock values
                    //*************************************
                    ArrayList storePlanValues = ReadStoreValues(planWeek, _stockVariable);

                    // BEGIN Issue 5141 stodd 1.28.2008
                    bool lockedCells = false;
                    bool nonZeroCells = false;
                    for (int s = 0; s < storePlanValues.Count; s++)
                    {
                        PlanCellReference cr = (PlanCellReference)storePlanValues[s];
                        if (cr.isCellLocked)
                            lockedCells = true;
                        if (cr.CurrentCellValue != 0.0)
                            nonZeroCells = true;
                    }

                    // Begin Track #6289 stodd
                    if ((Bal_Stock_Ind && (lockedCells || nonZeroCells))
                        ||
                       (!nonZeroCells && _prorateChainStock)) // MID Track #6043 - KJohnson - Forcast Chain WOS Calculations
                    {
                        // End Track 6289
                        //*************************************
                        // Get chain plan value
                        //*************************************
                        double chainValue = ReadChainValue(planWeek, _stockVariable);
                        showBalanceLog = true;	// track # 6289 stodd
                        // BEGIN MID Track #4370 - John Smith - FWOS Models
                        bool balanced = false;
                        int attempts = 0;
                        while (!balanced)
                        {
                            ++attempts;
                            balanced = Balance(storePlanValues, chainValue, planWeek, eVariableType.BegStock);
                            if (!balanced &&
                                attempts > _maxBalanceAttempts)
                            {
                                balanced = true;
                                string msg = MIDText.GetText(eMIDTextCode.msg_BalanceAttemptsExceeded);
                                msg = msg.Replace("{0}", _maxBalanceAttempts.ToString());
                                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msg, this.ToString());
                                if (MONITOR)
                                {
                                    _forecastMonitor.WriteLine(msg);
                                }
                            }
                        }
                        // END MID Track #4370
                    }
                    // Begin Track #6289 stodd
                    else if (Bal_Stock_Ind)
                    // End Track #6289 stodd
                    {
                        string msg = MIDText.GetText(eMIDTextCode.msg_NoBalanceNeeded);
                        msg = msg.Replace("{0}", planWeek.YearWeek.ToString());
                        msg = msg.Replace("{1}", _stockVariable.VariableProfile.VariableName);
                        _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msg, this.ToString());
                        if (MONITOR)
                        {
                            _forecastMonitor.WriteLine(msg);
                        }
                    }
                    // END Issue 5141
                }
            }
            return showBalanceLog;
        }

        /// <summary>
        /// Balance the list of values to the total
        /// </summary>
        /// <remarks>
        /// planWeek and variableType are not currently used.
        /// They will be needed if stock mins and maxes are need to be kept.
        /// </remarks>
        /// <param name="valueList"></param>
        /// <param name="totalValue"></param>
        /// <param name="planWeek"></param>
        /// <param name="variableType"></param>
        /// <returns>A boolean flag identifying if the balance was successful</returns>
        // BEGIN MID Track #4370 - John Smith - FWOS Models
        private bool Balance(ArrayList valueList, double totalValue, WeekProfile planWeek, eVariableType variableType)
        // END MID Track #4370 
        {
            _weekBeingPlanned = planWeek; // Track #6187
                                          // BEGIN MID Track #4370 - John Smith - FWOS Models
            bool balanceSuccessful = true;
            //Begin TT#1703 - DOConnell - Missing Inventory
            bool shouldBeLocked = false;
            double summandQuantityTotal = 0;
            //End TT#1703 - DOConnell - Missing Inventory
            Hashtable summandHash = null;
            if (variableType == eVariableType.BegStock) summandHash = (Hashtable)InventoryForecastSummandHash[planWeek.Key];
            else if (variableType == eVariableType.ChainSetPercent) summandHash = (Hashtable)InventoryForecastSummandHash[planWeek.Key];
            else summandHash = (Hashtable)InventoryForecastSummandHash[planWeek.Key];
            // END MID Track #4370 

            ProportionalSpread spread = new ProportionalSpread(_SAB);

            double[] adjustedValueArray;

            adjustedValueArray = new double[valueList.Count];

            //*******************************************************
            // Load up summand array list getting ready to do spread
            //*******************************************************
            Hashtable storeCellRefHash = new Hashtable();
            ArrayList summandList = new ArrayList();
            for (int s = 0; s < valueList.Count; s++)
            {
                PlanCellReference cr = (PlanCellReference)valueList[s];
                // this Hash is used later (after sorting) to rematch stores with the Cell Reference

                Summand summand = new Summand();
                if (variableType == eVariableType.ChainSetPercent)
                {
                    summand.Item = cr[eProfileType.Store];
                    storeCellRefHash.Add(cr[eProfileType.Store], cr);
                }
                else
                {
                    summand.Item = _allStoreList[s].Key;
                    storeCellRefHash.Add(_allStoreList[s].Key, cr);
                }
                summand.Eligible = !cr.isCellIneligible;
                // Begin Issue 3752 - stodd 
                // The RESERVE STORE is ALWAYS ineligible
                if (summand.Item == _reserveStoreRid)
                    summand.Eligible = false;
                // End Issue 3752 - stodd 
                summand.Quantity = cr.CurrentCellValue;
                adjustedValueArray[s] = cr.CurrentCellValue;

                // if cell is locked quantity becomes locked cell value, otherwise we use the
                // basis value.
                if (cr.isCellLocked)
                    summand.Locked = true;
                else
                    summand.Locked = false;


                // this partial code for getting stock min and max is currently commented out.
                // Right now we will balance without keeping the min and max.
                // but the code is left here incase someone changes their mind.
                if (variableType == eVariableType.BegStock)
                {
                    // BEGIN MID Track #4370 - John Smith - FWOS Models
                    if (summandHash != null)
                    {
                        Summand invSummand = (Summand)summandHash[summand.Item];

                        if (invSummand != null)
                        {
                            if (invSummand.Min == Include.Undefined)
                                summand.Min = 0;                //no min	
                            else
                                summand.Min = invSummand.Min;

                            if (invSummand.Max == Include.Undefined)
                                summand.Max = double.MaxValue;  //no max	
                            else
                                summand.Max = invSummand.Max;
                        }
                        else
                        {
                            summand.Min = 0;                //no min
                            summand.Max = double.MaxValue;  //no max
                        }
                    }
                    else
                    {
                        summand.Min = 0;                //no min
                        summand.Max = double.MaxValue;  //no max
                    }
                    // END MID Track #4370
                }
                else
                {
                    summand.Min = 0;                //no min
                    summand.Max = double.MaxValue;  //no max
                }

                summandList.Add(summand);

            }

            spread.SummandList = summandList;
            spread.RequestedTotal = totalValue;
            spread.Precision = 0;
            // Begin Track #6187 stodd
            int rc = spread.Calculate();
            if (rc != 0)
            {
                string msg = MIDText.GetText(eMIDTextCode.msg_MethodWarning);
                msg = msg.Replace("{0}", this.Name);
                msg = msg.Replace("{1}", "");   // Variable
                msg = msg.Replace("{2}", ""); // week
                string warnText = MIDText.GetText((eMIDTextCode)rc);
                msg = msg.Replace("{3}", warnText);
                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msg, _sourceModule);
            }
            // End track #6187

            //****************************************************************************
            // Take values in summandList and copy them to the corresponding PlanCellRef
            // for only the stores we are planning this time 
            //****************************************************************************
            StoreProfile tempStoreProfile = new StoreProfile(0);
            //Begin TT#1703 - DOConnell - Missing Inventory
            foreach (Summand summand in summandList)
            {
                if (summand.Eligible)
                {
                    summandQuantityTotal = summandQuantityTotal + summand.Quantity;
                }
            }
            //End TT#1703 - DOConnell - Missing Inventory
            foreach (Summand summand in summandList)
            {
                try
                {
                    tempStoreProfile.Key = summand.Item;
                    // is it Eligible?  is the cell Locked?
                    if (summand.Eligible && !summand.Locked)
                    {

                        PlanCellReference cr = (PlanCellReference)storeCellRefHash[summand.Item];
                        cr.SetEntryCellValue(summand.Result);
                        if (variableType == eVariableType.BegStock)
                        {
                            // if exceed max, set to max and lock
                            if (summand.Result > summand.Max)
                            {
                                summand.Result = summand.Max;
                                //Begin TT#1703 - DOConnell - Missing Inventory
                                //cr.SetCellLock(true);
                                shouldBeLocked = true;
                                //End TT#1703 - DOConnell - Missing Inventory
                                balanceSuccessful = false;
                            }
                            //Begin TT#1703 - DOConnell - Missing Inventory
                            else if (summand.Result == summand.Max && summandQuantityTotal < totalValue)
                            {
                                shouldBeLocked = true;
                            }
                            //End TT#1703 - DOConnell - Missing Inventory
                            // if less than min, set to min and lock
                            else if (summand.Result < summand.Min)
                            {
                                summand.Result = summand.Min;
                                //Begin TT#1703 - DOConnell - Missing Inventory
                                shouldBeLocked = true;
                                //cr.SetCellLock(true);
                                //End TT#1703 - DOConnell - Missing Inventory
                                balanceSuccessful = false;
                            }
                            //Begin TT#1703 - DOConnell - Missing Inventory
                            else if (summand.Result == summand.Min && summandQuantityTotal > totalValue)
                            {
                                shouldBeLocked = true;
                            }
                            //End TT#1703 - DOConnell - Missing Inventory
                        }
                        // END MID Track #4370
                        cr.SetCompCellValue(eSetCellMode.Computation, summand.Result);
                        //Begin TT#1703 - DOConnell - Missing Inventory
                        if (shouldBeLocked)
                        {
                            cr.SetCellLock(true);
                            shouldBeLocked = false;
                        }
                        //End TT#1703 - DOConnell - Missing Inventory
                    }
                }
                catch (CellNotAvailableException)
                {
                    string msg = MIDText.GetText(eMIDTextCode.msg_InventoryCellIsProtected);
                    msg = msg.Replace("{0}", StoreMgmt.GetStoreDisplayText(summand.Item)); //_SAB.StoreServerSession.GetStoreDisplayText(summand.Item));
                    msg = msg.Replace("{1}", planWeek.YearWeek.ToString());
                    ForecastVersion fv = new ForecastVersion();
                    msg = msg.Replace("{2}", fv.GetVersionText(Plan_FV_RID));
                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msg, this.ToString());
                    if (MONITOR)
                    {
                        _forecastMonitor.WriteLine(msg);
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.ToString();
                    throw;
                }
            }
            // BEGIN MID Track #4370 - John Smith - FWOS Models
            return balanceSuccessful;
            // END MID Track #4370

        }
        // BEGIN TT#1413 - dOConnell - Chain Plan - Set Percentages 
        private bool BalanceChainSetPercent(List<ChainSetKeyValues> WeekList, double totalValue, WeekProfile planWeek, eVariableType variableType, List<ChainSetPercentValues> ChainList)
        {
            _weekBeingPlanned = planWeek; // Track #6187
            // BEGIN MID Track #4370 - John Smith - FWOS Models
            bool balanceSuccessful = true;
            Hashtable summandHash = null;

            if (variableType == eVariableType.ChainSetPercent)
            {
                summandHash = (Hashtable)InventoryForecastSummandHash[planWeek.Key];
            }
            // END MID Track #4370 

            ProportionalSpread spread = new ProportionalSpread(_SAB);

            //double[] adjustedValueArray;
            //adjustedValueArray = new double[valueList.Count];

            //*******************************************************
            // Load up summand array list getting ready to do spread
            //*******************************************************
            Hashtable SetCellRefHash = new Hashtable();
            ArrayList summandList = new ArrayList();
            for (int s = 0; s < WeekList.Count; s++)
            {
                //double cr = (double)valueList[s];
                // this Hash is used later (after sorting) to rematch stores with the Cell Reference
                SetCellRefHash.Add(_allStoreList[s].Key, (WeekList[s].Index));

                Summand summand = new Summand();
                summand.Item = WeekList[s].Index;
                summand.ItemIdx = WeekList[s].Value;
                summand.Eligible = true;
                summand.Quantity = WeekList[s].Pct;
                // Begin Issue 3752 - stodd 
                // The RESERVE STORE is ALWAYS ineligible
                if (summand.Item == _reserveStoreRid)
                    summand.Eligible = false;
                // End Issue 3752 - stodd 
                summand.Locked = false;
                summand.Min = 0;				//no min
                summand.Max = double.MaxValue;  //no max

                summandList.Add(summand);
            }

            spread.SummandList = summandList;
            spread.RequestedTotal = totalValue;
            spread.Precision = 0;
            // Begin Track #6187 stodd
            int rc = spread.Calculate();
            if (rc != 0)
            {
                string msg = MIDText.GetText(eMIDTextCode.msg_MethodWarning);
                msg = msg.Replace("{0}", this.Name);
                msg = msg.Replace("{1}", "");	// Variable
                msg = msg.Replace("{2}", ""); // week
                string warnText = MIDText.GetText((eMIDTextCode)rc);
                msg = msg.Replace("{3}", warnText);
                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msg, _sourceModule);
            }
            // End track #6187

            //****************************************************************************
            // Take values in summandList and copy them to the corresponding PlanCellRef
            // for only the stores we are planning this time 
            //****************************************************************************
            StoreProfile tempStoreProfile = new StoreProfile(0);
            if (MONITOR)
            {
                _forecastMonitor.WriteLine(" ");
                _forecastMonitor.WriteLine("Chain Value " + totalValue);
            }
            foreach (Summand summand in summandList)
            {
                try
                {
                    tempStoreProfile.Key = summand.Item;
                    // is it Eligible?  is the cell Locked?

                    int cr = Convert.ToInt32(SetCellRefHash[summand.Quantity]);

                    ChainSetPercentValues StoreValueList = new ChainSetPercentValues(summand.Item, summand.Result, planWeek.YearWeek);

                    string StoreSetName = Convert.ToString(StoreMgmt.StoreGroupLevel_GetName(summand.ItemIdx, summand.Item)); //_SAB.StoreServerSession.GetGroupSetName(summand.ItemIdx, summand.Item));

                    ChainList.Add(StoreValueList);
                    summand.ItemIdx = 0;
                    if (MONITOR)
                    {
                        _forecastMonitor.WriteLine("ChainSet: " + StoreSetName + "(" + summand.Item + ") Percentage: " + summand.Quantity + " Result: " + summand.Result);
                    }
                }
                catch (CellNotAvailableException)
                {
                    string msg = MIDText.GetText(eMIDTextCode.msg_InventoryCellIsProtected);
                    msg = msg.Replace("{0}", StoreMgmt.GetStoreDisplayText(summand.Item)); //_SAB.StoreServerSession.GetStoreDisplayText(summand.Item));
                    msg = msg.Replace("{1}", planWeek.YearWeek.ToString());
                    ForecastVersion fv = new ForecastVersion();
                    msg = msg.Replace("{2}", fv.GetVersionText(Plan_FV_RID));
                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msg, this.ToString());
                    if (MONITOR)
                    {
                        _forecastMonitor.WriteLine(msg);
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.ToString();
                    throw;
                }
            }

            return balanceSuccessful;
        }
        // END TT#1413 - dOConnell - Chain Plan - Set Percentages 


        public class ForecastSpread : Spread
        {
            //=======
            // FIELDS
            //=======

            //			System.Collections.ArrayList _spreadToCellRefList;

            //=============
            // CONSTRUCTORS
            //=============

            /// <summary>
            /// Creates a new instance of PlanSpread.
            /// </summary>

            public ForecastSpread()
            {
            }

            override protected bool ExcludeValue(int aIndex)
            {
                return false;
            }

            override protected bool ExcludeValue(int aIndex, bool ignoreLocks)
            {
                return false;
            }

            public void ExecuteForecastSpread(double aSpreadFromValue, double[] aBasisValueArray, double[] aSpreadToValueArray, int aDecimals)
            {
                ExecutePctContributionSpread(aSpreadFromValue, aBasisValueArray, aSpreadToValueArray, aDecimals);
            }

        }

        public ProfileList GetWeeksToPlan(int planDrp_rid)
        {

            // Get full week range
            DateRangeProfile drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(planDrp_rid);
            ProfileList weekRange = _SAB.ApplicationServerSession.Calendar.GetWeekRange(drp, null);
            ProfileList weekRangeMirror = _SAB.ApplicationServerSession.Calendar.GetWeekRange(drp, null);
            // get posting week
            WeekProfile currentWeek = _SAB.ApplicationServerSession.Calendar.CurrentWeek;

            // Begin Issue 4025 - stodd
            if (weekRange.Count > 0)
            {
                _firstWeekOfBasis = (WeekProfile)weekRange[0];
            }
            // End Issue 4025

            // Begin 
            ForecastVersion fv = new ForecastVersion();
            bool forecastProtected = fv.GetIsVersionProtected(this.Plan_FV_RID);
            // End
            // Throw out weeks that are less than current week
            foreach (WeekProfile wp in weekRangeMirror.ArrayList)
            {
                // capture first week of plan range
                if (_firstWeekOfPlan == null)
                    _firstWeekOfPlan = wp;

                // Begin
                if (forecastProtected)
                {
                    if (wp.Key < currentWeek.Key)
                    {
                        weekRange.Remove(wp);
                        _errorMsg = "Week " + wp.WeekInYear.ToString("00", CultureInfo.CurrentUICulture) + "/" + wp.FiscalYear.ToString("0000", CultureInfo.CurrentUICulture) + " not planned.";
                        _errorMsg += "  Week prior to Current week.";
                        _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, _errorMsg, _sourceModule);
                    }
                }
                // End
            }

            // capture first week of plan range
            if (weekRange.Count > 0)
            {
                _firstWeekOfPlan = (WeekProfile)weekRange[0];
            }

            return weekRange;
        }

        // BEGIN Issue 4827 stodd 10.24.2007
        public ArrayList ReadStoreValues(WeekProfile planWeek, ModelVariableProfile aVariable)
        {
            ArrayList storeValues = new ArrayList();
            storeValues = ReadStoreValues(planWeek, aVariable.VariableProfile);
            return storeValues;
        }

        public ArrayList ReadStoreValues(WeekProfile planWeek, VariableProfile aVariable)
        {
            ArrayList storeValues = new ArrayList();

            Cube myCube = _cubeGroup.GetCube(eCubeType.StorePlanWeekDetail);

            PlanCellReference planCellRef = new PlanCellReference((PlanCube)myCube);
            planCellRef[eProfileType.Version] = this.Plan_FV_RID;
            planCellRef[eProfileType.HierarchyNode] = this.Plan_HN_RID;
            planCellRef[eProfileType.Week] = planWeek.Key;
            planCellRef[eProfileType.QuantityVariable] = _cubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
            planCellRef[eProfileType.Variable] = aVariable.Key;

            storeValues = planCellRef.GetCellRefArray(_allStoreList);

            return storeValues;
        }
        // END Issue 4827 

        public ArrayList GetStoreWeeklyValues(ArrayList weekList, int aStoreRID, ModelVariableProfile aVariable)
        {
            ArrayList storeWeeklyValues = new ArrayList();

            Cube myCube = _cubeGroup.GetCube(eCubeType.StorePlanWeekDetail);

            PlanCellReference planCellRef = new PlanCellReference((PlanCube)myCube);
            planCellRef[eProfileType.Version] = this.Plan_FV_RID;
            planCellRef[eProfileType.HierarchyNode] = this.Plan_HN_RID;
            planCellRef[eProfileType.QuantityVariable] = _cubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
            planCellRef[eProfileType.Variable] = aVariable.Key;

            planCellRef[eProfileType.Store] = aStoreRID;

            ProfileList weekProfileList = new ProfileList(eProfileType.Week, weekList);

            storeWeeklyValues = planCellRef.GetCellRefArray(weekProfileList);

            return storeWeeklyValues;
        }

        // BEGIN TT#619 - STodd - OTS Forecast - Chain Plan not required (#46)
        public ModelVariableProfile GetChainPlanWOSVariable()
        {
            ModelVariableProfile chainWOSVariable = null;
            if (_hnp.OTSPlanLevelType == eOTSPlanLevelType.Regular)
            {
                chainWOSVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.WOSRegPromoVariable.Key);
            }
            else
            {
                chainWOSVariable = new ModelVariableProfile(_cubeGroup.Transaction.PlanComputations.PlanVariables.WOSTotalVariable.Key);
            }
            return chainWOSVariable;
        }
        // End TT#619 - STodd - OTS Forecast - Chain Plan not required (#46)

        public int ReadChainValue(WeekProfile planWeek, ModelVariableProfile aVariable)
        {
            int chainValue;

            Cube myCube = _cubeGroup.GetCube(eCubeType.ChainPlanWeekDetail);

            PlanCellReference planCellRef = new PlanCellReference((PlanCube)myCube);
            planCellRef[eProfileType.Version] = this.Chain_FV_RID;
            planCellRef[eProfileType.HierarchyNode] = this.Plan_HN_RID;
            planCellRef[eProfileType.Week] = planWeek.Key;
            planCellRef[eProfileType.QuantityVariable] = _cubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
            planCellRef[eProfileType.Variable] = aVariable.Key;

            // Begin Track #6271 stodd
            // The opriginal rounding code of + .5 was changing the negative by one, making a -325 into -324.
            if (planCellRef.CurrentCellValue >= 0)
            {
                chainValue = (int)((((double)planCellRef.CurrentCellValue * _planFactor) / 100.0d) + 0.5d);
            }
            else
            {
                chainValue = (int)((((double)planCellRef.CurrentCellValue * _planFactor) / 100.0d) - 0.5d);
            }
            // End track #6271
            return chainValue;
        }

        // BEGIN TT#619 - STodd - OTS Forecast - Chain Plan not required (#46)
        public double ReadChainValueDouble(WeekProfile planWeek, ModelVariableProfile aVariable)
        {
            double chainValue;

            Cube myCube = _cubeGroup.GetCube(eCubeType.ChainPlanWeekDetail);

            PlanCellReference planCellRef = new PlanCellReference((PlanCube)myCube);
            planCellRef[eProfileType.Version] = this.Chain_FV_RID;
            planCellRef[eProfileType.HierarchyNode] = this.Plan_HN_RID;
            planCellRef[eProfileType.Week] = planWeek.Key;
            planCellRef[eProfileType.QuantityVariable] = _cubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
            planCellRef[eProfileType.Variable] = aVariable.Key;

            chainValue = planCellRef.CurrentCellValue;

            return chainValue;
        }
        // END TT#619 - STodd - OTS Forecast - Chain Plan not required (#46)

        #region Read Store Basis Detail Values

        /// <summary>
        /// Reads the Store Basis values for a particular Store Basis Detail
        /// </summary>
        /// <param name="basisDetailProfile"></param>
        /// <param name="weekList"></param>
        /// <returns></returns>
        //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
        //private ArrayList ReadBasisDetailValues(int basisProfileKey, BasisDetailProfile basisDetailProfile, ProfileList weekList, ModelVariableProfile aVariable)
        // BEGIN TT#279-MD - stodd - Projected Sales 
        private ArrayList ReadBasisDetailValues(int basisProfileKey, BasisProfile basisProfile, BasisDetailProfile basisDetailProfile, ProfileList weekList, ModelVariableProfile aVariable, eTyLyType aBasisType)
        // END TT#279-MD - stodd - Projected Sales 
        //End TT#43 - MD - DOConnell - Projected Sales Enhancement
        {
            ArrayList MasterStoreValueList = new ArrayList();

            Cube myCube = _cubeGroup.GetCube(eCubeType.StoreBasisWeekDetail);

            // read data

            // BEGIN TT#279-MD - stodd - Projected Sales 
            foreach (WeekProfile planWeek in weekList.ArrayList)
            // END TT#279-MD - stodd - Projected Sales 
            {
                PlanCellReference planCellRef = new PlanCellReference((PlanCube)myCube);
                planCellRef[eProfileType.Version] = this.Plan_FV_RID;
                planCellRef[eProfileType.HierarchyNode] = this.Plan_HN_RID;
                planCellRef[eProfileType.Basis] = basisProfileKey;
                // BEGIN TT#279-MD - stodd - Projected Sales 
                planCellRef[eProfileType.Week] = planWeek.Key;
                // END TT#279-MD - stodd - Projected Sales 
                planCellRef[eProfileType.QuantityVariable] = _cubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

                //				// If REG/PROMO or TOTAL
                //				if (basisDetailProfile.HierarchyNodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Regular)
                //					planCellRef[eProfileType.Variable] = _cubeGroup.Variables.SalesRegPromoUnitsVariable.Key;
                //				else
                //					planCellRef[eProfileType.Variable] = _cubeGroup.Variables.SalesTotalUnitsVariable.Key;
                //====================================================================================================
                // The above check was removed because the variable is now determine within the ProcessAction method.
                //====================================================================================================
                planCellRef[eProfileType.Variable] = aVariable.Key;

                ArrayList storeBasisValues = planCellRef.GetCellRefArray(_allStoreList);

                // For testing purposes only
                //				PlanCellReference pcr2 = (PlanCellReference)storeBasisValues[100];
                //				double storeValue2 = pcr2.HiddenCurrentCellValue;
                //				Debug.WriteLine(basisProfileKey.ToString() + " " + basisWeek.YearWeek.ToString() + " " + storeValue2.ToString());

                //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
                // BEGIN TT#279-MD - stodd - Projected Sales 
                int basisWeekKey = basisDetailProfile.GetBasisWeekIdFromPlanWeekId(_SAB.ApplicationServerSession, planWeek.Key);

                // Begin TT#2614 - JSmith - Applying non default sales percents
                WeekProfile basisWeekProfile = _SAB.ApplicationServerSession.Calendar.GetWeek(basisWeekKey);
                // End TT#2614 - JSmith - Applying non default sales percents

                Dictionary<int, double> totpcthash = new Dictionary<int, double>();
                if (aBasisType == eTyLyType.ProjectCurrWkSales)
                {
                    if (basisWeekKey == SAB.ApplicationServerSession.Calendar.CurrentWeek.Key)
                    {
                        int storeRID;
                        // Begin TT#2614 - JSmith - Applying non default sales percents
                        //totpcthash = AccumDailyPercentagesByStore(basisWeekKey);
                        totpcthash = AccumDailyPercentagesByStore(basisWeekProfile.YearWeek);
                        // End TT#2614 - JSmith - Applying non default sales percents

                        if (MasterStoreValueList.Count == 0)
                        {
                            for (int i = 0; i < storeBasisValues.Count; i++)
                            {
                                PlanCellReference pcr = (PlanCellReference)storeBasisValues[i];
                                storeRID = pcr[eProfileType.Store];
                                double totpct = GetStoreAccumDailyPercentage(totpcthash, storeRID);
                                double storeValue = 0;
                                double wgt = 0;
                                double wgtedStoreValue = 0;
                                if (totpct != 0.0)
                                {
                                    storeValue = (double)(decimal)(pcr.HiddenCurrentCellValue / totpct);
                                    wgt = _applyToWeights[basisProfile.Key];
                                    //storeValue = Math.Round((storeValue * wgt), 6);
                                    wgtedStoreValue = storeValue * wgt;
                                    MasterStoreValueList.Add(wgtedStoreValue);
                                }
                                else
                                {
                                    wgtedStoreValue = pcr.HiddenCurrentCellValue;
                                    MasterStoreValueList.Add(wgtedStoreValue);
                                }

                                if (MONITOR)
                                {
                                    StoreProfile sp = (StoreProfile)_allStoreList[i];
                                    string msg = "    " + basisProfile.Key.ToString() + ": " + sp.StoreId + "(" + storeRID + ")  Orig Val: " + pcr.HiddenCurrentCellValue +
                                        "  Proj Sales Pct: " + totpct + "  Proj Sales Interm Value: " + storeValue + "  Wgt: " + wgt + "  Wgted Val: " + wgtedStoreValue;
                                    _forecastMonitor.WriteLine(msg);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < storeBasisValues.Count; i++)
                            {
                                double totpct = 0;
                                PlanCellReference pcr = (PlanCellReference)storeBasisValues[i];
                                double masterValue = (double)MasterStoreValueList[i];
                                double wgt = _applyToWeights[basisProfile.Key];
                                //double storeValue = Math.Round((pcr.HiddenCurrentCellValue * wgt), 6);
                                double storeValue = (pcr.HiddenCurrentCellValue * wgt);
                                MasterStoreValueList[i] = (double)MasterStoreValueList[i] + storeValue;
                                if (MONITOR)
                                {
                                    StoreProfile sp = (StoreProfile)_allStoreList[i];
                                    storeRID = pcr[eProfileType.Store];
                                    string msg = "    " + basisProfile.Key.ToString() + ": " + sp.StoreId + "(" + storeRID + ")  Orig Val: " + pcr.HiddenCurrentCellValue +
                                        "  Proj Sales Pct: " + totpct + "  Proj Sales Interm Value: " + storeValue + "  Wgt: " + wgt + "  Wgted Val: " + storeValue;
                                    _forecastMonitor.WriteLine(msg);
                                }
                            }
                        }

                    }
                    else
                    {

                        if (MasterStoreValueList.Count == 0)
                        {
                            for (int i = 0; i < storeBasisValues.Count; i++)
                            {
                                PlanCellReference pcr = (PlanCellReference)storeBasisValues[i];
                                //double storeValue = pcr.HiddenCurrentCellValue;
                                double wgt = _applyToWeights[basisProfile.Key];
                                //double storeValue = Math.Round((pcr.HiddenCurrentCellValue * wgt), 6);
                                double storeValue = (pcr.HiddenCurrentCellValue * wgt);
                                MasterStoreValueList.Add(storeValue);
                                if (MONITOR)
                                {
                                    double totpct = 0;
                                    StoreProfile sp = (StoreProfile)_allStoreList[i];
                                    int storeRID = pcr[eProfileType.Store];
                                    string msg = "    " + basisProfile.Key.ToString() + ": " + sp.StoreId + "(" + storeRID + ")  Orig Val: " + pcr.HiddenCurrentCellValue +
                                        "  Proj Sales Pct: " + totpct + "  Proj Sales Interm Value: " + pcr.HiddenCurrentCellValue + "  Wgt: " + wgt + "  Wgted Val: " + storeValue;
                                    _forecastMonitor.WriteLine(msg);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < storeBasisValues.Count; i++)
                            {
                                PlanCellReference pcr = (PlanCellReference)storeBasisValues[i];
                                double masterValue = (double)MasterStoreValueList[i];
                                double wgt = _applyToWeights[basisProfile.Key];
                                //double storeValue = Math.Round((pcr.HiddenCurrentCellValue * wgt), 6);
                                double storeValue = (pcr.HiddenCurrentCellValue * wgt);
                                MasterStoreValueList[i] = (double)MasterStoreValueList[i] + storeValue;
                                if (MONITOR)
                                {
                                    double totpct = 0;
                                    StoreProfile sp = (StoreProfile)_allStoreList[i];
                                    int storeRID = pcr[eProfileType.Store];
                                    string msg = "    " + basisProfile.Key.ToString() + ": " + sp.StoreId + "(" + storeRID + ")  Orig Val: " + pcr.HiddenCurrentCellValue +
                                        "  Proj Sales Pct: " + totpct + "  Proj Sales Interm Value: " + pcr.HiddenCurrentCellValue + "  Wgt: " + wgt + "  Wgted Val: " + storeValue;
                                    _forecastMonitor.WriteLine(msg);
                                }
                            }
                        }
                    }
                }
                else
                {

                    if (MasterStoreValueList.Count == 0)
                    {
                        for (int i = 0; i < storeBasisValues.Count; i++)
                        {
                            PlanCellReference pcr = (PlanCellReference)storeBasisValues[i];
                            double storeValue = pcr.HiddenCurrentCellValue;
                            MasterStoreValueList.Add(storeValue);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < storeBasisValues.Count; i++)
                        {
                            PlanCellReference pcr = (PlanCellReference)storeBasisValues[i];
                            double masterValue = (double)MasterStoreValueList[i];
                            MasterStoreValueList[i] = (double)MasterStoreValueList[i] + pcr.HiddenCurrentCellValue;
                        }
                    }
                }

            }

            return MasterStoreValueList;
        }



        private Dictionary<int, double> AccumDailyPercentagesByStore(int basisWeekKey)
        {
            double pct = 0.0;
            double totpct = 0.0;

            Dictionary<int, double> totpcthash = new Dictionary<int, double>();
            DayProfile currentday = SAB.ApplicationServerSession.Calendar.CurrentDate;
            // BEGIN TT#279-MD - stodd - Projected Sales 
            StoreWeekDailyPercentagesList sdpl = SAB.HierarchyServerSession.GetStoreDailyPercentages(this.Plan_HN_RID, basisWeekKey);
            // END TT#279-MD - stodd - Projected Sales 
            foreach (StoreWeekDailyPercentagesProfile swdpp in sdpl)
            {
                totpct = 0.0;
                if (currentday.DayInWeek > 1)
                {
                    for (int d = 0; d < currentday.DayInWeek - 1; d++)
                    {
                        pct = swdpp.DailyPercentages[d];
                        totpct = totpct + pct;
                    }
                }
                totpct = (double)(decimal)Math.Round((totpct / 100), 4);
                totpcthash.Add(swdpp.Key, totpct);
            }
            return totpcthash;
        }

        private static double GetStoreAccumDailyPercentage(Dictionary<int, double> totpcthash, int storeRID)
        {
            double totpct = 1;
            foreach (int a in totpcthash.Keys)
            {
                if (a == storeRID)
                {
                    totpct = (double)(totpcthash[a]);
                }
            }
            return totpct;
        }
        // END TT#279-MD - stodd - Projected Sales 

        public void AddArrayLists(ArrayList masterList, ArrayList aValueList)
        {
            if (masterList.Count == 0)
            {
                for (int i = 0; i < aValueList.Count; i++)
                {
                    masterList.Add(aValueList[i]);
                }
            }
            else
            {
                for (int i = 0; i < aValueList.Count; i++)
                {
                    double masterValue = (double)masterList[i];
                    masterList[i] = masterValue + (double)aValueList[i];
                }
            }
        }

        public void SubtractArrayLists(ArrayList masterList, ArrayList aValueList)
        {
            if (masterList.Count == 0)
            {
                for (int i = 0; i < aValueList.Count; i++)
                {
                    double newValue = 0 - (double)aValueList[i];
                    masterList.Add(newValue);
                }
            }
            else
            {
                for (int i = 0; i < aValueList.Count; i++)
                {
                    double masterValue = (double)masterList[i];
                    masterList[i] = masterValue - (double)aValueList[i];
                }
            }
        }

        public void ApplyWeight(ArrayList aValueList, float weight)
        {
            for (int i = 0; i < aValueList.Count; i++)
            {
                double storeValue = (double)aValueList[i];
                aValueList[i] = storeValue * weight;
            }
        }
        #endregion

        /// <summary>
        /// Updates the OTS Plan method
        /// </summary>
        /// <param name="td">An instance of the TransactionData class which contains the database connection</param>
        //		new public void Update(TransactionData td)
        override public void Update(TransactionData td)

        {
            // Begin TT#1147 - JSmith -  Foreacst Audit Report not accurate for New Store Set
            GroupLevelFunctionProfile defaultSetGLFProfile = null;
            ProfileList groupLevelBasis;
            // End TT#1147

            if (_OTSPlanData == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
            {
                _OTSPlanData = new OTSPlanMethodData();
            }

            _OTSPlanData.Method_RID = this.Key;
            _OTSPlanData.Plan_HN_RID = this.Plan_HN_RID;
            _OTSPlanData.Plan_FV_RID = this.Plan_FV_RID;
            _OTSPlanData.CDR_RID = this.CDR_RID;
            _OTSPlanData.Chain_FV_RID = this.Chain_FV_RID;
            _OTSPlanData.Bal_Sales_Ind = Include.ConvertBoolToChar(this.Bal_Sales_Ind);
            _OTSPlanData.Bal_Stock_Ind = Include.ConvertBoolToChar(this.Bal_Stock_Ind);
            // BEGIN TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 
            _OTSPlanData.ApplyTrendOptionsInd = _ApplyTrendOptionsInd;
            _OTSPlanData.ApplyTrendOptionsWOSValue = _ApplyTrendOptionsWOSValue;
            // END TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 
            _OTSPlanData.ForecastModelRid = this._forecastModelRid;
            // BEGIN MID Track #4371 - Justin Bolles - Low Level Forecast
            _OTSPlanData.High_Level_Ind = Include.ConvertBoolToChar(this.HighLevelInd);
            _OTSPlanData.Low_Levels_Ind = Include.ConvertBoolToChar(this.LowLevelsInd);
            _OTSPlanData.Low_Level_Type = (int)this.LowLevelsType;
            _OTSPlanData.Low_Level_Seq = this.LowLevelsSequence;
            _OTSPlanData.Low_Level_Offset = this.LowLevelsOffset;
            _OTSPlanData.LowlevelExcludeList = this.LowlevelOverrideList;
            // END MID Track #4371
            _OTSPlanData.OverrideLowLevelRid = this.OverrideLowLevelRid;

            try
            {
                GroupLevelFunction GLFData = null;

                switch (this.Method_Change_Type)
                {
                    case eChangeType.add:
                        base.Update(td);    // creates rec on METHOD table
                        _OTSPlanData.InsertOTSPlan(base.Key, td);  // adds OTS_PLAN table

                        foreach (GroupLevelFunctionProfile GLFProfile in GLFProfileList)
                        {
                            GLFData = new GroupLevelFunction();
                            GLFData.Method_RID = this.Key;
                            GLFData.SGL_RID = GLFProfile.Key;
                            GLFData.Default_Ind = Include.ConvertBoolToChar(GLFProfile.Default_IND);
                            GLFData.Plan_Ind = Include.ConvertBoolToChar(GLFProfile.Plan_IND);
                            GLFData.Use_Default_Ind = Include.ConvertBoolToChar(GLFProfile.Use_Default_IND);
                            GLFData.Clear_Ind = Include.ConvertBoolToChar(GLFProfile.Clear_IND);
                            GLFData.Season_Ind = Include.ConvertBoolToChar(GLFProfile.Season_IND);
                            GLFData.Season_HN_RID = GLFProfile.Season_HN_RID;
                            GLFData.GLFT_ID = Convert.ToInt32(GLFProfile.GLFT_ID, CultureInfo.CurrentUICulture);
                            GLFData.GLSB_ID = Convert.ToInt32(GLFProfile.GLSB_ID, CultureInfo.CurrentUICulture);
                            GLFData.LY_Alt_Ind = Include.ConvertBoolToChar(GLFProfile.LY_Alt_IND);
                            GLFData.Trend_Alt_Ind = Include.ConvertBoolToChar(GLFProfile.Trend_Alt_IND);
                            GLFData.TY_Equalize_Weight = Include.ConvertBoolToChar(GLFProfile.TY_Weight_Multiple_Basis_Ind);
                            GLFData.LY_Equalize_Weight = Include.ConvertBoolToChar(GLFProfile.LY_Weight_Multiple_Basis_Ind);
                            GLFData.Apply_Equalize_Weight = Include.ConvertBoolToChar(GLFProfile.Apply_Weight_Multiple_Basis_Ind);
                            //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
                            GLFData.Proj_Curr_Wk_Sales_Ind = Include.ConvertBoolToChar(GLFProfile.Proj_Curr_Wk_Sales_IND);
                            //END TT#43 - MD - DOConnell - Projected Sales Enhancement
                            if (!GLFData.InsertGLF(td))  // adds GROUP_LEVEL_FUNCTION table
                            {
                                // TODO ERROR!!
                                //return (int)eGenericDBError.GenericDBError;
                            }


                            int seq = 1;
                            // BEGIN Issue 4818
                            GroupLevelBasis groupLevelBasisData = new GroupLevelBasis();
                            // remove any previous Group Level Basis records for this key
                            groupLevelBasisData.DeleteGroupLevelBasis(this.Key, GLFProfile.Key, td);
                            // build a new Group Level Basis for each basis row
                            foreach (GroupLevelBasisProfile groupLevelBasisProfile in GLFProfile.GroupLevelBasis)
                            {
                                groupLevelBasisData.MethodRid = this.Key;
                                groupLevelBasisData.SglRid = GLFProfile.Key;
                                groupLevelBasisData.HierNodeRid = groupLevelBasisProfile.Basis_HN_RID;
                                groupLevelBasisData.VersionRid = groupLevelBasisProfile.Basis_FV_RID;
                                groupLevelBasisData.DateRangeRid = groupLevelBasisProfile.Basis_CDR_RID;
                                groupLevelBasisData.Weight = groupLevelBasisProfile.Basis_Weight;
                                groupLevelBasisData.Sequence = seq;
                                groupLevelBasisData.ExcludeInd = groupLevelBasisProfile.Basis_ExcludeInd;
                                groupLevelBasisData.TyLyType = groupLevelBasisProfile.Basis_TyLyType;
                                // Begin Issue 4422 - stodd
                                groupLevelBasisData.MerchType = groupLevelBasisProfile.MerchType;
                                groupLevelBasisData.MerchOffset = groupLevelBasisProfile.MerchOffset;
                                groupLevelBasisData.MerchPhlSequence = groupLevelBasisProfile.MerchPhlSequence;
                                groupLevelBasisData.MerchPhRid = groupLevelBasisProfile.MerchPhRid;
                                // End issue 4422
                                groupLevelBasisData.InsertGroupLevelBasis(td);

                                seq++;
                            }
                            // END Issue 4818

                            TrendCaps trendCapsData = new TrendCaps();
                            // There is only 1 trend cap per set
                            foreach (TrendCapsProfile tcp in GLFProfile.Trend_Caps)
                            {
                                trendCapsData.MethodRid = this.Key;
                                trendCapsData.SglRid = GLFProfile.Key;
                                trendCapsData.TrendCapID = tcp.TrendCapID;
                                trendCapsData.TolPct = tcp.TolPct;
                                trendCapsData.HighLimit = tcp.HighLimit;
                                trendCapsData.LowLimit = tcp.LowLimit;

                                trendCapsData.InsertTrendCaps(td);
                            }

                            foreach (int Key in GLFProfile.Group_Level_Nodes.Keys)
                            {
                                GroupLevelNodeFunction glnf = (GroupLevelNodeFunction)GLFProfile.Group_Level_Nodes[Key];
                                glnf.SglRID = GLFProfile.Key;
                                glnf.MethodRID = this.Key;
                                glnf.InsertGroupLevelNodeFunction(td);

                                StockMinMax stockMinMaxData = new StockMinMax();

                                // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                                //if(glnf.MinMaxInheritType == eMinMaxInheritType.None)
                                // Begin TT#3420 - JSmith - Store Forecast Stock Min-Max at Low Level not being observed.
                                //if (glnf.MinMaxInheritType == eMinMaxInheritType.None ||
                                //    glnf.MinMaxInheritType == eMinMaxInheritType.Default)
                                if (glnf.MinMaxInheritType == eMinMaxInheritType.None ||
                                    glnf.MinMaxInheritType == eMinMaxInheritType.Default ||
                                    (glnf.isHighLevel && glnf.MinMaxInheritType == eMinMaxInheritType.Method))
                                // End TT#3420 - JSmith - Store Forecast Stock Min-Max at Low Level not being observed.
                                // End TT#3
                                {
                                    foreach (StockMinMaxProfile smmp in glnf.Stock_MinMax)
                                    {
                                        stockMinMaxData.MethodRid = this.Key;
                                        stockMinMaxData.StoreGroupLevelRid = GLFProfile.Key;
                                        stockMinMaxData.HN_RID = glnf.HN_RID;
                                        stockMinMaxData.Boundary = smmp.Boundary;
                                        stockMinMaxData.DateRangeRid = smmp.DateRangeRid;
                                        stockMinMaxData.MinimumStock = smmp.MinimumStock;
                                        stockMinMaxData.MaximumStock = smmp.MaximumStock;

                                        stockMinMaxData.InsertStockMinMax(td);
                                    }
                                }
                            }
                        }
                        // BEGIN TT691 - stodd - old store group level information not getting cleaned up.
                        this._originalSGRid = SG_RID;
                        // END TT691 - stodd - old store group level information not getting cleaned up.
                        break;


                    case eChangeType.update:
                        base.Update(td);    // updates rec on METHOD table
                                            //Update OTS Plan Method
                        _OTSPlanData.UpdateOTSPlan(this.Key, td);  // Updates OTS_PLAN table

                        GLFData = new GroupLevelFunction();
                        // delete all group level functions/basis plan/basis range records
                        GLFData.DeleteGLFCascade(this.Key, this.SG_RID, td);
                        // During Maintenance the Store Group (Attr) can change.
                        // those original records need to be removed too.
                        if (this._originalSGRid != SG_RID && this._originalSGRid != Include.NoRID)
                            GLFData.DeleteGLFCascade(this.Key, this._originalSGRid, td);
                        // BEGIN TT691 - stodd - old store group level information not getting cleaned up.
                        _originalSGRid = SG_RID;
                        // END TT691 - stodd - old store group level information not getting cleaned up.

                        // Begin TT#1147 - JSmith -  Foreacst Audit Report not accurate for New Store Set
                        foreach (GroupLevelFunctionProfile GLFProfile in GLFProfileList)
                        {
                            if (GLFProfile.Default_IND)
                            {
                                defaultSetGLFProfile = GLFProfile;
                                break;
                            }
                        }
                        // End TT#1147

                        foreach (GroupLevelFunctionProfile GLFProfile in GLFProfileList)
                        {
                            GLFData.Method_RID = this.Key;
                            GLFData.SGL_RID = GLFProfile.Key;
                            GLFData.Default_Ind = Include.ConvertBoolToChar(GLFProfile.Default_IND);
                            GLFData.Plan_Ind = Include.ConvertBoolToChar(GLFProfile.Plan_IND);
                            GLFData.Use_Default_Ind = Include.ConvertBoolToChar(GLFProfile.Use_Default_IND);
                            GLFData.Clear_Ind = Include.ConvertBoolToChar(GLFProfile.Clear_IND);
                            GLFData.Season_Ind = Include.ConvertBoolToChar(GLFProfile.Season_IND);
                            GLFData.Season_HN_RID = GLFProfile.Season_HN_RID;
                            GLFData.GLFT_ID = Convert.ToInt32(GLFProfile.GLFT_ID, CultureInfo.CurrentUICulture);
                            GLFData.GLSB_ID = Convert.ToInt32(GLFProfile.GLSB_ID, CultureInfo.CurrentUICulture);
                            GLFData.LY_Alt_Ind = Include.ConvertBoolToChar(GLFProfile.LY_Alt_IND);
                            GLFData.Trend_Alt_Ind = Include.ConvertBoolToChar(GLFProfile.Trend_Alt_IND);
                            GLFData.TY_Equalize_Weight = Include.ConvertBoolToChar(GLFProfile.TY_Weight_Multiple_Basis_Ind);
                            GLFData.LY_Equalize_Weight = Include.ConvertBoolToChar(GLFProfile.LY_Weight_Multiple_Basis_Ind);
                            GLFData.Apply_Equalize_Weight = Include.ConvertBoolToChar(GLFProfile.Apply_Weight_Multiple_Basis_Ind);
                            //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
                            GLFData.Proj_Curr_Wk_Sales_Ind = Include.ConvertBoolToChar(GLFProfile.Proj_Curr_Wk_Sales_IND);
                            //END TT#43 - MD - DOConnell - Projected Sales Enhancement
                            if (!GLFData.InsertGLF(td))  // adds GROUP_LEVEL_FUNCTION table
                            {
                                // TODO ERROR!!
                                //return (int)eGenericDBError.GenericDBError;
                            }

                            int seq = 1;
                            // BEGIN Issue 4818
                            GroupLevelBasis groupLevelBasisData = new GroupLevelBasis();
                            // remove any previous Basis Plan records for this key
                            groupLevelBasisData.DeleteGroupLevelBasis(this.Key, GLFProfile.Key, td);
                            // build a new basis plan for each basis row

                            // Begin TT#1147 - JSmith -  Foreacst Audit Report not accurate for New Store Set
                            if (GLFProfile.Use_Default_IND &&
                                defaultSetGLFProfile != null)
                            {
                                groupLevelBasis = defaultSetGLFProfile.GroupLevelBasis;
                            }
                            else
                            {
                                groupLevelBasis = GLFProfile.GroupLevelBasis;
                            }

                            //foreach(GroupLevelBasisProfile groupLevelBasisProfile in GLFProfile.GroupLevelBasis)
                            foreach (GroupLevelBasisProfile groupLevelBasisProfile in groupLevelBasis)
                            // End TT#1147
                            {
                                groupLevelBasisData.MethodRid = this.Key;
                                groupLevelBasisData.SglRid = GLFProfile.Key;
                                groupLevelBasisData.HierNodeRid = groupLevelBasisProfile.Basis_HN_RID;
                                groupLevelBasisData.VersionRid = groupLevelBasisProfile.Basis_FV_RID;
                                groupLevelBasisData.DateRangeRid = groupLevelBasisProfile.Basis_CDR_RID;
                                groupLevelBasisData.Weight = groupLevelBasisProfile.Basis_Weight;
                                groupLevelBasisData.Sequence = seq;
                                groupLevelBasisData.ExcludeInd = groupLevelBasisProfile.Basis_ExcludeInd;
                                groupLevelBasisData.TyLyType = groupLevelBasisProfile.Basis_TyLyType;
                                // Begin Issue 4422 - stodd
                                groupLevelBasisData.MerchType = groupLevelBasisProfile.MerchType;
                                groupLevelBasisData.MerchOffset = groupLevelBasisProfile.MerchOffset;
                                groupLevelBasisData.MerchPhlSequence = groupLevelBasisProfile.MerchPhlSequence;
                                groupLevelBasisData.MerchPhRid = groupLevelBasisProfile.MerchPhRid;
                                // End issue 4422

                                groupLevelBasisData.InsertGroupLevelBasis(td);

                                seq++;
                            }
                            // END Issue 4818

                            TrendCaps trendCapsData = new TrendCaps();
                            // There is only 1 trend cap per set
                            foreach (TrendCapsProfile tcp in GLFProfile.Trend_Caps)
                            {
                                trendCapsData.MethodRid = this.Key;
                                trendCapsData.SglRid = GLFProfile.Key;
                                trendCapsData.TrendCapID = tcp.TrendCapID;
                                trendCapsData.TolPct = tcp.TolPct;
                                trendCapsData.HighLimit = tcp.HighLimit;
                                trendCapsData.LowLimit = tcp.LowLimit;

                                trendCapsData.InsertTrendCaps(td);
                            }

                            GroupLevelNodeFunction.DeleteGroupLevelNodeFunction(this.Key, GLFProfile.Key, td);
                            foreach (int Key in GLFProfile.Group_Level_Nodes.Keys)
                            {
                                GroupLevelNodeFunction glnf = (GroupLevelNodeFunction)GLFProfile.Group_Level_Nodes[Key];
                                glnf.SglRID = GLFProfile.Key;
                                glnf.MethodRID = this.Key;
                                glnf.InsertGroupLevelNodeFunction(td);

                                StockMinMax stockMinMaxData = new StockMinMax();
                                stockMinMaxData.DeleteStockMinMax(glnf.MethodRID, glnf.SglRID, glnf.HN_RID, glnf.isHighLevel, td);
                                foreach (StockMinMaxProfile smmp in glnf.Stock_MinMax)
                                {
                                    stockMinMaxData.MethodRid = this.Key;
                                    stockMinMaxData.StoreGroupLevelRid = GLFProfile.Key;
                                    stockMinMaxData.HN_RID = glnf.HN_RID;
                                    stockMinMaxData.Boundary = smmp.Boundary;
                                    stockMinMaxData.DateRangeRid = smmp.DateRangeRid;
                                    stockMinMaxData.MinimumStock = smmp.MinimumStock;
                                    stockMinMaxData.MaximumStock = smmp.MaximumStock;

                                    stockMinMaxData.InsertStockMinMax(td);
                                }
                            }
                        }
                        break;

                    case eChangeType.delete:
                        GLFData = new GroupLevelFunction();
                        // delete all group level functions/basis plan/basis range records
                        GLFData.DeleteGLFCascade(this.Key, this.SG_RID, td);
                        // delete OTD_PLAN record
                        _OTSPlanData.DeleteOTSPlan(this.Key, td);
                        // delete METHOD record
                        base.Update(td);

                        break;
                }
            }
            catch (Exception e)
            {
                string message = e.ToString();
                throw;
            }
            finally
            {

                //TO DO:  whatever has to be done after an update or exception.
            }
        }

        #region Basis
        //		/// <summary>
        //		/// Save Basis Plan and Basis Range (delete current DB records & insert new) 
        //		/// as two seperate DataTables (as they are in the DB)
        //		/// based on METHOD_RID and SGL_RID
        //		/// </summary>
        //		/// <param name="method_RID">method_RID - PK of Group Level Function</param>
        //		/// <param name="sgl_RID">sgl_RID - PK of Group Level Function</param>
        //		/// <param name="dtBasisPlan">DataTable containing Basis_Plan data</param>
        //		/// <param name="dtBasisRange">DataTable containing Basis_Range data</param>
        //		/// <param name="eTyLy">eTyLyType enum</param>
        //		/// <returns>boolean; true if successful, false if failed</returns>
        //		public bool InsertBasisDetails(int method_RID, int sgl_RID, DataTable dtBasisPlan, DataTable dtBasisRange, eTyLyType eTyLy, TransactionData td)
        //		{
        //			try
        //			{
        //				BasisData bd = new BasisData();
        //				bd.InsertBasis(method_RID, sgl_RID, dtBasisPlan,dtBasisRange,eTyLy, td);
        //				return true;
        //			}
        //			catch
        //			{
        //				return false;
        //			}
        //		}

        // BEGIN Issue 4818 stodd 1.9.2008 Discrete Time Periods
        /// <summary>
        /// Get GROUP_LEVEL_BASIS DataTable based on METHOD_RID
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetGroupLevelBasis(int method_RID)
        {
            GroupLevelBasis glb = new GroupLevelBasis();
            return glb.GetGroupLevelBasis(method_RID);
        }

        // Begin TT#1044 - JSmith - Apply Trend to TAb is taking an unusual amount of time to apply a basis- this iwill be a problem for ANF
        /// <summary>
        /// Get GROUP_LEVEL_BASIS DataTable based on METHOD_RID
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetGroupLevelBasis(int method_RID, eTyLyType tyLyType)
        {
            GroupLevelBasis glb = new GroupLevelBasis();
            return glb.GetGroupLevelBasis(method_RID, tyLyType);
        }
        // End TT#1044

        /// <summary>
        /// Get GROUP_LEVEL_BASIS DataTable based on METHOD_RID and SGL_RID
        /// </summary>
        /// <param name="method_RID"></param>
        /// <param name="sgl_RID"></param>
        /// <returns>DataTable</returns>
        public DataTable GetGroupLevelBasis(int method_RID, int sgl_RID)
        {
            GroupLevelBasis glb = new GroupLevelBasis();
            return glb.GetGroupLevelBasis(method_RID, sgl_RID);
        }
        // END Issue 4818


        //		/// <summary>
        //		/// Get Basis_Plan DataTable based on METHOD_RID - Should have GLF.SGL_RID??
        //		/// </summary>
        //		/// <returns>DataTable</returns>
        //		public DataTable GetBasisPlan(int method_RID)
        //		{
        //			BasisPlan bp = new  BasisPlan();
        //			return bp.GetBasisPlan(method_RID);
        //		}
        //
        //		/// <summary>
        //		/// Get Basis_Plan DataTable based on METHOD_RID and SGL_RID
        //		/// </summary>
        //		/// <param name="method_RID"></param>
        //		/// <param name="sgl_RID"></param>
        //		/// <returns>DataTable</returns>
        //		public DataTable GetBasisPlan(int method_RID, int sgl_RID)
        //		{
        //			BasisPlan bp = new  BasisPlan();
        //			return bp.GetBasisPlan(method_RID, sgl_RID);
        //		}
        //
        //		/// <summary>
        //		/// Get Basis_Range DataTable based on METHOD_RID - Should have GLF.SGL_RID??
        //		/// </summary>
        //		/// <returns>DataTable</returns>
        //		public DataTable GetBasisRange(int method_RID)
        //		{
        //			BasisRange br = new BasisRange();
        //			return br.GetBasisRange(method_RID);
        //		}
        //
        //		/// <summary>
        //		/// Get Basis_Range DataTable based on METHOD_RID and SGL_RID
        //		/// </summary>
        //		/// <param name="method_RID"></param>
        //		/// <param name="sgl_RID"></param>
        //		/// <returns>DataTable</returns>
        //		public DataTable GetBasisRange(int method_RID, int sgl_RID)
        //		{
        //			BasisRange br = new BasisRange();
        //			return br.GetBasisRange(method_RID, sgl_RID);
        //		}
        //
        //		/// <summary>
        //		/// Gets Both the Basis_Plan table and the Basis_Range DataTable 
        //		/// as one combined table based on METHOD_RID - Should have GLF.SGL_RID??
        //		/// </summary>
        //		/// <returns></returns>
        //		public DataTable GetBasisPlanAndBasisRange(int method_RID)
        //		{
        //			BasisRange br = new BasisRange();
        //			return br.GetBasisPlanAndBasisRange(method_RID);
        //		}
        //
        //		/// <summary>
        //		/// Gets Both the Basis_Plan table and the Basis_Range DataTable 
        //		/// as one combined table based on METHOD_RID and SGL_RID
        //		/// </summary>
        //		/// <param name="method_RID"></param>
        //		/// <param name="sgl_RID"></param>
        //		/// <returns>DataTable</returns>
        //		public DataTable GetBasisPlanAndBasisRange(int method_RID, int sgl_RID)
        //		{
        //			BasisRange br = new BasisRange();
        //			return br.GetBasisPlanAndBasisRange(method_RID, sgl_RID);
        //		}

        /// <summary>
        /// Get Stock Min Max DataTable based on METHOD_RID
        /// </summary>
        /// <param name="method_RID"></param>
        /// <returns></returns>
        public DataTable GetStockMinMax(int method_RID)
        {
            StockMinMax stkMinMax = new StockMinMax();
            return stkMinMax.GetStockMinMax(method_RID);
        }

        /// <summary>
        /// Get Stock Min Max DataTable based on METHOD_RID and SGL_RID
        /// </summary>
        /// <param name="method_RID"></param>
        /// <param name="sgl_RID"></param>
        /// <returns>DataTable</returns>
        public DataTable GetStockMinMax(int method_RID, int sgl_RID)
        {
            StockMinMax stkMinMax = new StockMinMax();
            return stkMinMax.GetStockMinMax(method_RID, sgl_RID);
        }
        #endregion

        public override bool WithinTolerance(double aTolerancePercent)
        {
            return true;
        }

        /// <summary>
        /// Returns a copy of this object.
        /// </summary>
        /// <param name="aSession">
        /// The current session
        /// </param>
        /// <param name="aCloneDateRanges">
        /// A flag identifying if date ranges are to be cloned or use the original</param>
        /// <param name="aCloneCustomOverrideModels">
        /// A flag identifying if custom override models are to be cloned or use the original
        /// </param>
        /// <returns>
        /// A copy of the object.
        /// </returns>
        // Begin Track #5912 - JSmith - Save As needs to clone custom override models
        //override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges)
        override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges, bool aCloneCustomOverrideModels)
        // End Track #5912
        {
            OTSPlanMethod newOTSPlanMethod = null;
            // Begin Track #5912 - JSmith - Save As needs to clone custom override models
            OverrideLowLevelProfile ollp;
            int customUserRID;
            // End Track #5912

            try
            {
                newOTSPlanMethod = (OTSPlanMethod)this.MemberwiseClone();
                newOTSPlanMethod.AllStoreList = AllStoreList;
                newOTSPlanMethod.Bal_Sales_Ind = Bal_Sales_Ind;
                newOTSPlanMethod.Bal_Stock_Ind = Bal_Stock_Ind;
                if (aCloneDateRanges &&
                    CDR_RID != Include.UndefinedCalendarDateRange)
                {
                    newOTSPlanMethod.CDR_RID = aSession.Calendar.GetDateRangeClone(CDR_RID).Key;
                }
                else
                {
                    newOTSPlanMethod.CDR_RID = CDR_RID;
                }
                newOTSPlanMethod.Chain_FV_RID = Chain_FV_RID;
                newOTSPlanMethod.CurrentVariable = CurrentVariable;
                newOTSPlanMethod.FirstWeekOfPlan = FirstWeekOfPlan;
                newOTSPlanMethod.ForecastModelRid = ForecastModelRid;
                newOTSPlanMethod.Method_Change_Type = eChangeType.none;
                newOTSPlanMethod.Method_Description = Method_Description;
                newOTSPlanMethod.MethodStatus = MethodStatus;
                newOTSPlanMethod.MONITOR = MONITOR;
                newOTSPlanMethod.MonitorFilePath = MonitorFilePath;
                newOTSPlanMethod.MonitorMode = MonitorMode;
                newOTSPlanMethod.Name = Name;
                newOTSPlanMethod.Plan_FV_RID = Plan_FV_RID;
                newOTSPlanMethod.Plan_HN_RID = Plan_HN_RID;
                newOTSPlanMethod.PlanPeriod = PlanPeriod;
                newOTSPlanMethod.SG_RID = SG_RID;
                newOTSPlanMethod.StockMinMax = StockMinMax;
                newOTSPlanMethod.User_RID = User_RID;
                newOTSPlanMethod.Virtual_IND = Virtual_IND;
                newOTSPlanMethod.Template_IND = Template_IND;
                // Begin Track #5912 - JSmith - Save As needs to clone custom override models
                if (aCloneCustomOverrideModels &&
                    CustomOLL_RID != Include.NoRID)
                {
                    ollp = new OverrideLowLevelProfile(CustomOLL_RID);
                    ollp.Key = Include.NoRID;
                    ollp.ModelChangeType = eChangeType.add;
                    customUserRID = Include.NoRID;
                    customUserRID = ollp.WriteProfile(ref customUserRID, SAB.ClientServerSession.UserRID);
                    if (CustomOLL_RID == OverrideLowLevelRid)
                    {
                        OverrideLowLevelRid = customUserRID;
                    }
                    newOTSPlanMethod.CustomOLL_RID = customUserRID;
                }
                // End Track #5912
                newOTSPlanMethod.OverrideLowLevelRid = OverrideLowLevelRid;

                newOTSPlanMethod.GLFProfileList = new ProfileList(eProfileType.GroupLevelFunction);
                foreach (GroupLevelFunctionProfile glfp in this._GLFProfileList.ArrayList)
                {
                    GroupLevelFunctionProfile newGLFP = new GroupLevelFunctionProfile(glfp.Key);
                    newGLFP.Default_IND = glfp.Default_IND;
                    newGLFP.Plan_IND = glfp.Plan_IND;
                    newGLFP.Use_Default_IND = glfp.Use_Default_IND;
                    newGLFP.Clear_IND = glfp.Clear_IND;
                    newGLFP.Season_IND = glfp.Season_IND;
                    newGLFP.Season_HN_RID = glfp.Season_HN_RID;
                    newGLFP.GLFT_ID = glfp.GLFT_ID;
                    newGLFP.GLSB_ID = glfp.GLSB_ID;
                    newGLFP.LY_Alt_IND = glfp.LY_Alt_IND;
                    newGLFP.Trend_Alt_IND = glfp.Trend_Alt_IND;
                    newGLFP.TY_Weight_Multiple_Basis_Ind = glfp.TY_Weight_Multiple_Basis_Ind;
                    newGLFP.LY_Weight_Multiple_Basis_Ind = glfp.LY_Weight_Multiple_Basis_Ind;
                    newGLFP.Apply_Weight_Multiple_Basis_Ind = glfp.Apply_Weight_Multiple_Basis_Ind;
                    newGLFP.GLF_Change_Type = eChangeType.none;
                    //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
                    newGLFP.Proj_Curr_Wk_Sales_IND = glfp.Proj_Curr_Wk_Sales_IND;
                    //END TT#43 - MD - DOConnell - Projected Sales Enhancement
                    // Copies in Basis info AND trend caps
                    // BEGIN Issue 4818
                    foreach (GroupLevelBasisProfile glbp in glfp.GroupLevelBasis)
                    {
                        newGLFP.GroupLevelBasis.Add(glbp.Copy());
                    }
                    // END Issue 4818
                    foreach (TrendCapsProfile tcp in glfp.Trend_Caps)
                    {
                        newGLFP.Trend_Caps.Add(tcp.Copy());
                    }
                    foreach (int Key in glfp.Group_Level_Nodes.Keys)
                    {
                        newGLFP.Group_Level_Nodes.Add(Key, glfp.Group_Level_Nodes[Key]);
                    }

                    newOTSPlanMethod.GLFProfileList.Add(newGLFP);
                }

                return newOTSPlanMethod;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // Begin MID Track 4858 - JSmith - Security changes
        /// <summary>
        /// Returns a flag identifying if the user can update the data on the method.
        /// </summary>
        /// <param name="aSession">
        /// The current session
        /// </param>
        /// <param name="aUserRID">
        /// The internal key of the user</param>
        /// <returns>
        /// A flag.
        /// </returns>
        override public bool AuthorizedToUpdate(Session aSession, int aUserRID)
        {
            try
            {
                if (Plan_FV_RID == Include.NoRID ||
                    Plan_HN_RID == Include.NoRID)
                {
                    return false;
                }

                VersionSecurityProfile versionSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(Plan_FV_RID, (int)eSecurityTypes.Store);
                if (!versionSecurity.AllowUpdate)
                {
                    return false;
                }
                HierarchyNodeSecurityProfile hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(Plan_HN_RID, (int)eSecurityTypes.Store);
                if (!hierNodeSecurity.AllowUpdate)
                {
                    return false;
                }
                return true;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End MID Track 4858

        // Begin TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.
        /// <summary>
        /// Returns a flag identifying if the user can view the data on the method.
        /// </summary>
        /// <param name="aSession">
        /// The current session
        /// </param>
        /// <param name="aUserRID">
        /// The internal key of the user</param>
        /// <returns>
        /// A flag.
        /// </returns>
        override public bool AuthorizedToView(Session aSession, int aUserRID)
        {
            HierarchyNodeSecurityProfile hierNodeSecurity;
            VersionSecurityProfile versionSecurity;

            int basisHnRID, basisFVRID;
            foreach (GroupLevelFunctionProfile glfp in this._GLFProfileList.ArrayList)
            {
                foreach (GroupLevelBasisProfile glbp in glfp.GroupLevelBasis)
                {
                    basisFVRID = glbp.Basis_FV_RID;
                    if (basisFVRID != Include.NoRID)
                    {
                        versionSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(basisFVRID, (int)eSecurityTypes.Store);
                        if (!versionSecurity.AllowView)
                        {
                            return false;
                        }
                    }
                    //BEGIN TT#3988 - DOConnell - Low Levels incorectly reports "75069: You are not authorized to view all data referenced by this method" message
                    if (glbp.MerchType == eMerchandiseType.Node)
                    //END TT#3988 - DOConnell - Low Levels incorectly reports "75069: You are not authorized to view all data referenced by this method" message
                    {
                        basisHnRID = glbp.Basis_HN_RID;
                        if (basisHnRID != Include.NoRID)
                        {
                            hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(basisHnRID, (int)eSecurityTypes.Store);
                            if (!hierNodeSecurity.AllowView)
                            {
                                return false;
                            }
                        }
                    } //TT#3988 - DOConnell - Low Levels incorectly reports "75069: You are not authorized to view all data referenced by this method" message
                }
            }

            return true;
        }
        // End TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.

        // BEGIN RO-642 - RDewey
        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            if (this.GlobalUserType == eGlobalUserType.Global)
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalOTSPlan);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserOTSPlan);
            }

        }

        int _attributeSetKey = Include.NoRID;
        bool _attributeChanged = false;
        bool _needToFixLowLevelData = true;

        override public ROMethodProperties MethodGetData(out bool successful, ref string message, bool processingApply = false)
        {
            successful = true;
            ProfileList attributeSetList;

            KeyValuePair<int, string> thisMethod = GetName.GetMethod(method: this);

            ROLevelInformation lowLevel = new ROLevelInformation();
            lowLevel.LevelType = (eROLevelsType)LowLevelsType;
            lowLevel.LevelOffset = LowLevelsOffset;
            lowLevel.LevelSequence = LowLevelsSequence;
            lowLevel.LevelValue = GetName.GetLevelName(
               levelType: (eROLevelsType)LowLevelsType,
               levelSequence: LowLevelsSequence,
               levelOffset: LowLevelsOffset,
               SAB: SAB
               );

            ROOverrideLowLevel roOverrideLowLevel = new ROOverrideLowLevel();
            roOverrideLowLevel.LowLevel = lowLevel;
            roOverrideLowLevel.OverrideLowLevelsModel = GetName.GetOverrideLowLevelsModel(OverrideLowLevelRid, SAB);
            roOverrideLowLevel.OverrideLowLevelsModelList = BuildOverrideLowLevelList(
                overrideLowLevelRid: OverrideLowLevelRid,
                customOverrideLowLevelRid: CustomOLL_RID
                );

            if (CustomOLL_RID > Include.NoRID
                && CustomOLL_RID == OverrideLowLevelRid)
            {
                roOverrideLowLevel.IsCustomModel = true;
            }

            // get key of first set in attribute
            if (_attributeSetKey == Include.NoRID)
            {
                attributeSetList = StoreMgmt.StoreGroup_GetLevelListViewList(SG_RID);
                if (attributeSetList.Count > 0)
                {
                    _attributeSetKey = attributeSetList[0].Key;
                }
            }

            ROPlanningForecastMethodProperties method = new ROPlanningForecastMethodProperties(
               method: thisMethod,
               description: Method_Description,
               userKey: User_RID,
               merchandise: GetName.GetMerchandiseName(Plan_HN_RID, SAB),
               highLevel: HighLevelInd,
               lowLevels: LowLevelsInd,
               version: GetName.GetVersion(Plan_FV_RID, SAB),
               lowLevel: GetName.GetOverrideLowLevelsModel(CustomOLL_RID, SAB),
               dateRange: GetName.GetCalendarDateRange(CDR_RID, SAB),
               overrideLowLevel: roOverrideLowLevel,
               chainVersion: GetName.GetVersion(Chain_FV_RID, SAB),
               salesBalance: Bal_Sales_Ind,
               stockBalance: Bal_Stock_Ind,
               applyTrendOptions: GetApplyTrendOptions(ApplyTrendOptionsInd),
               applyTrendOptionsValue: ApplyTrendOptionsWOSValue,
               attribute: GetName.GetAttributeName(SG_RID),
               attributeSet: GetName.GetAttributeSetName(key: _attributeSetKey),
               isTemplate: Template_IND
               );

            method.DefaultAttributeSetKey = GetDefaultGLFRid();

            foreach (GroupLevelFunctionProfile GLFProfile in _GLFProfileList)
            {
                // adjust values for basis that were set as SameNode if low level is turned off
                if (!method.LowLevels
                    && _needToFixLowLevelData)
                {
                    foreach (GroupLevelBasisProfile basis in GLFProfile.GroupLevelBasis)
                    {
                        if (basis.MerchType != eMerchandiseType.Node)
                        {
                            basis.MerchType = eMerchandiseType.Node;
                            basis.Basis_HN_RID = Plan_HN_RID;
                        }
                    }
                }

                if (GLFProfile.Key != _attributeSetKey)
                {
                    continue;
                }

                method.AttributeSetValues = BuildAttributeSetProperties(
                    GLFProfile: GLFProfile,
                    lowLevel: roOverrideLowLevel.LowLevel
                    );
            }

            _needToFixLowLevelData = false;

            if (method.AttributeSetValues == null)
            {
                GroupLevelFunctionProfile GLFProfile = new GroupLevelFunctionProfile(aKey: _attributeSetKey);
                GroupLevelNodeFunction groupLevelNodeFunction = new GroupLevelNodeFunction();
                groupLevelNodeFunction.MethodRID = this.Key;
                groupLevelNodeFunction.SglRID = GLFProfile.Key;
                groupLevelNodeFunction.HN_RID = Plan_HN_RID;
                groupLevelNodeFunction.ApplyMinMaxesInd = true;
                groupLevelNodeFunction.MinMaxInheritType = eMinMaxInheritType.None;
                SetGroupLevelFunctionStockMinMaxProfileList(groupLevelNodeFunction);
                GLFProfile.Group_Level_Nodes.Add(groupLevelNodeFunction.HN_RID, groupLevelNodeFunction);
                method.AttributeSetValues = BuildAttributeSetProperties(
                    GLFProfile: GLFProfile,
                    lowLevel: roOverrideLowLevel.LowLevel);
            }

            BuildVersionLists(method: method);

            if (Plan_HN_RID > 0)
            {
                BuildLowLevelList(method: method);
            }

            return method;
        }

        override public ROOverrideLowLevel MethodGetOverrideModelList(
            ROOverrideLowLevel overrideLowLevel,
            out bool successful,
            ref string message
            )
        {
            successful = true;

            OverrideLowLevelRid = overrideLowLevel.OverrideLowLevelsModel.Key;
            if (overrideLowLevel.IsCustomModel)
            {
                CustomOLL_RID = overrideLowLevel.OverrideLowLevelsModel.Key;
            }
            else
            {
                CustomOLL_RID = Include.NoRID;
            }

            overrideLowLevel.OverrideLowLevelsModel = GetName.GetOverrideLowLevelsModel(OverrideLowLevelRid, SAB);
            overrideLowLevel.OverrideLowLevelsModelList = BuildOverrideLowLevelList(
                overrideLowLevelRid: OverrideLowLevelRid,
                customOverrideLowLevelRid: CustomOLL_RID
                );

            if (CustomOLL_RID > Include.NoRID
                && CustomOLL_RID == OverrideLowLevelRid)
            {
                overrideLowLevel.IsCustomModel = true;
            }

            return overrideLowLevel;
        }

        private void BuildVersionLists(ROPlanningForecastMethodProperties method)
        {
            ProfileList storeVersionList = GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Store, true, Plan_FV_RID, true);
            foreach (VersionProfile versionProfile in storeVersionList)
            {
                method.Versions.Add(new KeyValuePair<int, string>(versionProfile.Key, versionProfile.Description));
            }

            ProfileList chainVersionList = GetForecastVersionList(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Chain, true, Chain_FV_RID);
            foreach (VersionProfile versionProfile in chainVersionList)
            {
                method.ChainVersions.Add(new KeyValuePair<int, string>(versionProfile.Key, versionProfile.Description));
            }

            ProfileList basisVersionList = base.GetForecastVersionList(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Store);
            foreach (VersionProfile versionProfile in basisVersionList)
            {
                method.BasisVersions.Add(new KeyValuePair<int, string>(versionProfile.Key, versionProfile.Description));
            }
        }

        private void BuildLowLevelList(ROPlanningForecastMethodProperties method)
        {
            eMerchandiseType merchandiseType;
            int homeHierarchyKey;
            List<HierarchyLevelComboObject> levelList = HierarchyTools.GetLevelsList(
                sessionAddressBlock: SAB,
                nodeKey: Plan_HN_RID,
                includeHomeLevel: false,
                includeLowestLevel: true,
                includeOrganizationLevelsForAlternate: false,
                merchandiseType: out merchandiseType,
                homeHierarchyKey: out homeHierarchyKey
                );

            method.HierarchyLevelsType = merchandiseType;
            foreach (HierarchyLevelComboObject level in levelList)
            {
                if (merchandiseType == eMerchandiseType.LevelOffset)
                {
                    method.HierarchyLevels.Add(new KeyValuePair<int, string>(level.Level, level.ToString()));
                }
                else
                {
                    method.HierarchyLevels.Add(new KeyValuePair<int, string>(level.Level, level.LevelName));
                }
            }

            // set selected as first level if not set
            if (method.LowLevels
                    && method.OverrideLowLevel.LowLevel.LevelType == eROLevelsType.None
                    && method.HierarchyLevels.Count > 0)
            {
                if (method.HierarchyLevelsType == eMerchandiseType.HierarchyLevel)
                {
                    method.OverrideLowLevel.LowLevel.LevelType = eROLevelsType.HierarchyLevel;
                    method.OverrideLowLevel.LowLevel.LevelSequence = method.HierarchyLevels[0].Key;
                    method.OverrideLowLevel.LowLevel.LevelValue = method.HierarchyLevels[0].Value;
                }
                else
                {
                    method.OverrideLowLevel.LowLevel.LevelType = eROLevelsType.LevelOffset;
                    method.OverrideLowLevel.LowLevel.LevelOffset = method.HierarchyLevels[0].Key;
                    method.OverrideLowLevel.LowLevel.LevelValue = method.HierarchyLevels[0].Value;
                }
            }

        }
         
        private ROPlanningForecastMethodAttributeSetProperties BuildAttributeSetProperties(
            GroupLevelFunctionProfile GLFProfile,
            ROLevelInformation lowLevel
            )
        {

            GroupLevelNodeFunction GLNFunction = (GroupLevelNodeFunction)GLFProfile.Group_Level_Nodes[Plan_HN_RID];

            if (GLNFunction == null)
            {
                GLNFunction = new GroupLevelNodeFunction();
                GLNFunction.MethodRID = this.Key;
                GLNFunction.SglRID = GLFProfile.Key;
                GLNFunction.HN_RID = Plan_HN_RID;
                GLNFunction.ApplyMinMaxesInd = true;
                GLNFunction.MinMaxInheritType = eMinMaxInheritType.None;
                SetGroupLevelFunctionStockMinMaxProfileList(GLNFunction);
                GLFProfile.Group_Level_Nodes.Add(GLNFunction.HN_RID, GLNFunction);
            }

            ProfileList pl = GLFProfile.Trend_Caps;
            TrendCapsProfile trendCapsProfile = null;

            foreach (TrendCapsProfile tcp in pl)
            {
                trendCapsProfile = tcp;
            }

            eTrendCapID trendCapId = eTrendCapID.None;
            double trendCapsTolerance = 0;
            double trendCapsLowLimit = 0;
            double trendCapsHighLimit = 0;

            if (trendCapsProfile != null)
            {
                trendCapId = trendCapsProfile.TrendCapID;
                trendCapsTolerance = trendCapsProfile.TolPct;
                trendCapsLowLimit = trendCapsProfile.LowLimit;
                trendCapsHighLimit = trendCapsProfile.HighLimit;
            }

            ProfileList groupLevelBasis = GLFProfile.GroupLevelBasis;
            List<ROForecastingBasisDetailsProfile> forecastBasisDetailProfiles = ConvertBasisDataToList(
                groupBasisProfiles: groupLevelBasis, 
                tyLyType: eTyLyType.NonTyLy,
                lowLevel: lowLevel
                );
            List<ROForecastingBasisDetailsProfile> forecastBasisDetailProfilesTY = ConvertBasisDataToList(
                groupBasisProfiles: groupLevelBasis,
                tyLyType: eTyLyType.TyLy,
                lowLevel: lowLevel
                );
            List<ROForecastingBasisDetailsProfile> forecastBasisDetailProfilesLY = ConvertBasisDataToList(
                groupBasisProfiles: groupLevelBasis,
                tyLyType: eTyLyType.AlternateLy,
                lowLevel: lowLevel
                );
            List<ROForecastingBasisDetailsProfile> forecastBasisDetailProfilesTrend = ConvertBasisDataToList(
                groupBasisProfiles: groupLevelBasis,
                tyLyType: eTyLyType.AlternateApplyTo,
                lowLevel: lowLevel
                );

            List<ROPlanningStoreGrade> storeGrades;
            List<ROStockMinMax> stockMinMaxes;
            // Only supports None and Default stock min/max inheritance types
            if (GLNFunction.MinMaxInheritType != eMinMaxInheritType.Default)
            {
                GLNFunction.MinMaxInheritType = eMinMaxInheritType.None;
            }
            storeGrades = BuildStoreGrades(GLNFunction);
            stockMinMaxes = BuildDataStockMinMax(GLNFunction, ref storeGrades);
            storeGrades = MergeStoreGradesWithStockMinMax(storeGrades, stockMinMaxes);

            ROPlanningForecastMethodAttributeSetProperties attributeSetProperties = new ROPlanningForecastMethodAttributeSetProperties(
                   attributeSet: GetName.GetAttributeSetName(GLFProfile.Key),
                   isDefaultProperties: GLFProfile.Default_IND,
                   isAttributeSetForecast: GLFProfile.Plan_IND,
                   isAttributeSetToUseDefault: GLFProfile.Use_Default_IND,
                   forecastMethod: GetName.GetForecastMethodType(GLFProfile.GLFT_ID),
                   smoothBy: GetName.GetSmoothByType(GLFProfile.GLSB_ID),
                   forecastBasisDetailProfiles: forecastBasisDetailProfiles,
                   stockMerchandise: GetName.GetMerchandiseName(GLNFunction.HN_RID, SAB),
                   applyMinMax: GLNFunction.ApplyMinMaxesInd,
                   minMaxInheritType: GLNFunction.MinMaxInheritType,
                   storeGrades: storeGrades,
                   forecastBasisDetailProfilesTY: forecastBasisDetailProfilesTY,
                   equalizingWaitingTY: GLFProfile.TY_Weight_Multiple_Basis_Ind,
                   forecastBasisDetailProfilesLY: forecastBasisDetailProfilesLY,
                   equalizingWaitingLY: GLFProfile.LY_Weight_Multiple_Basis_Ind,
                   isAlternateLY: GLFProfile.LY_Alt_IND,
                   forcastBasisDetailProfilesApplyTrendTo: forecastBasisDetailProfilesTrend,
                   equalizingWaitingApplyTrendTo: GLFProfile.Apply_Weight_Multiple_Basis_Ind,
                   isAlternateApplyTrendTo: GLFProfile.Trend_Alt_IND,
                   isProjectCurrentWeekSales: GLFProfile.Proj_Curr_Wk_Sales_IND,
                   trendCapId: trendCapId,
                   trendCapsTolerance: trendCapsTolerance,
                   trendCapsLowLimit: trendCapsLowLimit,
                   trendCapsHighLimit: trendCapsHighLimit
                );

            return attributeSetProperties;
        }

        #region MethodGetData Private Functions
        const string STOREGRADE_DEFAULT = "(Default)";
        private List<ROForecastingBasisDetailsProfile> ConvertBasisDataToList(
            ProfileList groupBasisProfiles, 
            eTyLyType tyLyType,
            ROLevelInformation lowLevel
            )
        {
            KeyValuePair<int, string> workKVP;
            List<ROForecastingBasisDetailsProfile> basisDetailProfiles = new List<ROForecastingBasisDetailsProfile>();

            foreach (GroupLevelBasisProfile basis in groupBasisProfiles)
            {
                if (basis.Basis_TyLyType == tyLyType)
                {
                    int iMerchandiseId = Convert.ToInt32(basis.Basis_HN_RID.ToString());
                    workKVP = GetName.GetMerchandiseName(iMerchandiseId, SAB);
                    string sMerchandise = workKVP.Value;
                    if (basis.MerchType == eMerchandiseType.SameNode)
                    {
                        sMerchandise = lowLevel.LevelValue;
                        basis.MerchPhlSequence = lowLevel.LevelSequence;
                        basis.MerchOffset = lowLevel.LevelOffset;
                    }
                    int iVersionId = Convert.ToInt32(basis.Basis_FV_RID.ToString());
                    workKVP = GetName.GetVersion(iVersionId, SAB);
                    string sVersion = workKVP.Value;
                    int iDateRangeID = Convert.ToInt32(basis.Basis_CDR_RID.ToString());

                    if (_CDR_RID != Include.UndefinedCalendarDateRange)
                    { workKVP = GetName.GetCalendarDateRange(iDateRangeID, SAB, _CDR_RID); }
                    else
                    { workKVP = GetName.GetCalendarDateRange(iDateRangeID, SAB, SAB.ClientServerSession.Calendar.CurrentDate); }

                    string sDateRange = workKVP.Value;
                    string sPicture = string.Empty;
                    float fWeight = Convert.ToSingle(basis.Basis_Weight.ToString());
                    bool bIsIncluded = !basis.Basis_ExcludeInd;

                    string sIncludeButton = bIsIncluded.ToString();
                    ROForecastingBasisDetailsProfile forecastBasisDetailProfile = new ROForecastingBasisDetailsProfile(basis.Key, iMerchandiseId, sMerchandise, iVersionId, sVersion,
                        iDateRangeID, sDateRange, sPicture, fWeight, bIsIncluded, sIncludeButton, tyLyType, basis.MerchType, basis.MerchPhRid, basis.MerchPhlSequence, basis.MerchOffset);

                    basisDetailProfiles.Add(forecastBasisDetailProfile);
                }
            }

            return basisDetailProfiles;
        }

        private List<ROStockMinMax> BuildDataStockMinMax(GroupLevelNodeFunction GLNF, ref List<ROPlanningStoreGrade> storeGrades)
        {
            List<ROStockMinMax> stockMinMaxes = new List<ROStockMinMax>();

            foreach (StockMinMaxProfile smmp in GLNF.Stock_MinMax)
            {
                ROStockMinMax stockMinMax = new ROStockMinMax();

                stockMinMax.StoreGrade = new KeyValuePair<int, string>(0, STOREGRADE_DEFAULT);
                stockMinMax.StoreGrouplevel = GetName.GetAttributeSetName(smmp.StoreGroupLevelRid);
                stockMinMax.Boundary = smmp.Boundary;
                stockMinMax.Merchandise = GetName.GetMerchandiseName(smmp.HN_RID, SAB);
                stockMinMax.DateRange = GetName.GetCalendarDateRange(smmp.DateRangeRid, SAB);
                stockMinMax.MinimumStock = smmp.MinimumStock != (int)Include.UndefinedDouble ? smmp.MinimumStock : (int?)null;
                stockMinMax.MaximumStock = smmp.MaximumStock != (int)Include.UndefinedDouble ? smmp.MaximumStock : (int?)null;
                stockMinMaxes.Add(stockMinMax);

                int dateRange = Convert.ToInt32(smmp.DateRangeRid, CultureInfo.CurrentUICulture);
                if (dateRange == Include.UndefinedCalendarDateRange)
                {
                    int sglRid = Convert.ToInt32(smmp.StoreGroupLevelRid, CultureInfo.CurrentUICulture);
                    int hnRid = Convert.ToInt32(smmp.HN_RID, CultureInfo.CurrentUICulture);
                    int boundary = Convert.ToInt32(smmp.Boundary, CultureInfo.CurrentUICulture);
                    foreach (ROPlanningStoreGrade gradeRecord in storeGrades)
                    {
                        int gradeBoundary = Convert.ToInt32(gradeRecord.StoreGrade.Key, CultureInfo.CurrentUICulture);
                        int gradeSglRid = Convert.ToInt32(gradeRecord.StoreGroupLevel.Key, CultureInfo.CurrentUICulture);
                        int gradeHnRid = Convert.ToInt32(gradeRecord.Merchandise.Key, CultureInfo.CurrentUICulture);

                        if (boundary == gradeBoundary && sglRid == gradeSglRid && gradeHnRid == hnRid)
                        {
                            gradeRecord.Minimum = smmp.MinimumStock != (int)Include.UndefinedDouble ? smmp.MinimumStock : (int?)null;
                            gradeRecord.Maximum = smmp.MaximumStock != (int)Include.UndefinedDouble ? smmp.MaximumStock : (int?)null;
                        }
                    }
                }
            }

            return stockMinMaxes;
        }

        private List<ROPlanningStoreGrade> MergeStoreGradesWithStockMinMax(List<ROPlanningStoreGrade> storeGrades, List<ROStockMinMax> stockMinMaxes)
        {
            foreach (ROPlanningStoreGrade grade in storeGrades)
            {
                List<ROStockMinMax> data = stockMinMaxes.FindAll(x => (x.Boundary == grade.StoreGrade.Key) &&
                      (x.StoreGrouplevel.Key == grade.StoreGroupLevel.Key) &&
                      (x.Merchandise.Key == grade.Merchandise.Key) &&
                      (x.DateRange.Key != Include.UndefinedCalendarDateRange)
                      );

                if (data != null) grade.ROStockMinMaxList = data;
            }

            return storeGrades;
        }

        private List<ROPlanningStoreGrade> BuildStoreGrades(GroupLevelNodeFunction glnf)
        {
            List<ROPlanningStoreGrade> storeGrades = new List<ROPlanningStoreGrade>();

            StoreGradeList storeGradeList = SAB.HierarchyServerSession.GetStoreGradeList(glnf.HN_RID, false, true);

            ProfileList storeGroupLevelList = StoreMgmt.StoreGroup_GetLevelListViewList(SG_RID);

            foreach (StoreGroupLevelListViewProfile sglp in storeGroupLevelList.ArrayList)
            {
                if (sglp.Key != glnf.SglRID) continue;
                ROPlanningStoreGrade storeGrade = new ROPlanningStoreGrade();

                storeGrade.StoreGrade = new KeyValuePair<int, string>(-1, "Default");
                storeGrade.DateRange = new KeyValuePair<int, string>(Include.UndefinedCalendarDateRange, "(Default)");
                storeGrade.StoreGroupLevel = new KeyValuePair<int, string>(sglp.Key, sglp.Name);
                storeGrade.Merchandise = GetName.GetMerchandiseName(glnf.HN_RID, SAB);
                storeGrade.Minimum = null;
                storeGrade.Maximum = null;
                storeGrades.Add(storeGrade);

                foreach (StoreGradeProfile sgp in storeGradeList.ArrayList)
                {
                    ROPlanningStoreGrade storeGrade2 = new ROPlanningStoreGrade();
                    storeGrade2.StoreGrade = new KeyValuePair<int, string>(sgp.Key, sgp.StoreGrade);
                    storeGrade2.DateRange = new KeyValuePair<int, string>(Include.UndefinedCalendarDateRange, "(Default)");
                    storeGrade2.StoreGroupLevel = new KeyValuePair<int, string>(sglp.Key, sglp.Name);
                    storeGrade2.Merchandise = GetName.GetMerchandiseName(glnf.HN_RID, SAB);
                    storeGrade2.Minimum = null;
                    storeGrade2.Maximum = null;
                    storeGrades.Add(storeGrade2);

                }
            }
            return storeGrades;
        }

        private eApplyTrendOptions GetApplyTrendOptions(char applyTrendOption)
        {
            if (applyTrendOption == 'C')
            {
                return eApplyTrendOptions.ChainPlan;
            }
            else if (applyTrendOption == 'W')
            {
                return eApplyTrendOptions.ChainWOS;
            }
            else //if (applyTrendOption == 'S')
            { return eApplyTrendOptions.PlugChainWOS; }
        }

        private char SetApplyTrendOptions(eApplyTrendOptions applyTrendOption)
        {
            if (applyTrendOption == eApplyTrendOptions.ChainPlan)
            {
                return 'C';
            }
            else if (applyTrendOption == eApplyTrendOptions.ChainWOS)
            {
                return 'W';
            }
            else //eApplyTrendOptions.PlugChainWOS;
            { return 'S'; }
        }
        #endregion

        public void SetStockMinMax(
            List<ROPlanningStoreGrade> storeGrades, 
            ref GroupLevelNodeFunction glnf,
            bool stockMinMaxInheritedChanged,
            GroupLevelFunctionProfile defaultAttributeSetProfile
            )
        {
            // if changed to inherit from default, copy values from default
            if (glnf.MinMaxInheritType == eMinMaxInheritType.Default
                && defaultAttributeSetProfile != null)
            {
                if (stockMinMaxInheritedChanged)
                {
                    glnf.Stock_MinMax.Clear();
                    GroupLevelNodeFunction default_groupLevelNodeFunction;
                    StockMinMaxProfile stockMinMax;
                    default_groupLevelNodeFunction = (GroupLevelNodeFunction)defaultAttributeSetProfile.Group_Level_Nodes[glnf.HN_RID];
                    if (default_groupLevelNodeFunction != null)
                    {
                        foreach (StockMinMaxProfile stockMinMaxProfile in default_groupLevelNodeFunction.Stock_MinMax)
                        {
                            stockMinMax = stockMinMaxProfile.Copy(SAB.ApplicationServerSession, true);
                            stockMinMax.StoreGroupLevelRid = glnf.SglRID;
                            glnf.Stock_MinMax.Add(stockMinMax);
                        }
                    }
                }
            }
            else
            {
                glnf.Stock_MinMax.Clear();
                foreach (ROPlanningStoreGrade sg in storeGrades)
                {
                    if (sg.MinimumIsSet
                        || sg.MaximumIsSet)
                    {
                        StockMinMaxProfile smmp = new StockMinMaxProfile(glnf.Stock_MinMax.MinValue - 1);
                        smmp.Boundary = sg.StoreGrade.Key;
                        smmp.StoreGroupLevelRid = sg.StoreGroupLevel.Key;
                        smmp.HN_RID = sg.Merchandise.Key;
                        if (sg.MinimumIsSet)
                        {
                            smmp.MinimumStock = (int)sg.Minimum;
                        }
                        else
                        {
                            smmp.MinimumStock = (int)Include.UndefinedDouble;
                        }
                        if (sg.MaximumIsSet)
                        {
                            smmp.MaximumStock = (int)sg.Maximum;
                        }
                        else
                        {
                            smmp.MaximumStock = (int)Include.UndefinedDouble;
                        }
                        smmp.DateRangeRid = sg.DateRange.Key;
                        glnf.Stock_MinMax.Add(smmp);
                    }

                    if (sg.ROStockMinMaxList != null && sg.ROStockMinMaxList.Count > 0)
                    {
                        foreach (ROStockMinMax roStockMinMax in sg.ROStockMinMaxList)
                        {
                            if (roStockMinMax.MinimumStockIsSet
                                || roStockMinMax.MaximumStockIsSet)
                            {
                                StockMinMaxProfile smmp2 = new StockMinMaxProfile(glnf.Stock_MinMax.MinValue - 1);
                                smmp2.Boundary = roStockMinMax.Boundary;
                                smmp2.StoreGroupLevelRid = roStockMinMax.StoreGrouplevel.Key;
                                smmp2.HN_RID = roStockMinMax.Merchandise.Key;
                                if (roStockMinMax.MinimumStockIsSet)
                                {
                                    smmp2.MinimumStock = (int)roStockMinMax.MinimumStock;
                                }
                                else
                                {
                                    smmp2.MinimumStock = (int)Include.UndefinedDouble;
                                }
                                if (roStockMinMax.MaximumStockIsSet)
                                {
                                    smmp2.MaximumStock = (int)roStockMinMax.MaximumStock;
                                }
                                else
                                {
                                    smmp2.MaximumStock = (int)Include.UndefinedDouble;
                                }
                                smmp2.DateRangeRid = roStockMinMax.DateRange.Key;

                                glnf.Stock_MinMax.Add(smmp2);
                            }
                        }
                    }
                }
            }
        }

        int index = -1;
        private ProfileList GetGroupBasisProfileList(List<ROForecastingBasisDetailsProfile> roBasisDetailProfiles)
        {

            ProfileList groupBasisList = new ProfileList(eProfileType.GroupLevelBasis);

            foreach (ROForecastingBasisDetailsProfile forcastBasisDetailProfile in roBasisDetailProfiles)
            {
                GroupLevelBasisProfile glbProfile = new GroupLevelBasisProfile(index);
                if (forcastBasisDetailProfile.MerchandiseType == eMerchandiseType.SameNode
                    || forcastBasisDetailProfile.MerchandiseId < 0)
                {
                    glbProfile.Basis_HN_RID = 0;
                }
                else
                {
                    glbProfile.Basis_HN_RID = forcastBasisDetailProfile.MerchandiseId;
                }

                glbProfile.Basis_FV_RID = forcastBasisDetailProfile.VersionId;
                glbProfile.Basis_Weight = forcastBasisDetailProfile.Weight;
                glbProfile.Basis_CDR_RID = forcastBasisDetailProfile.DateRangeId;
                glbProfile.Basis_ExcludeInd = !forcastBasisDetailProfile.IsIncluded;
                glbProfile.Basis_TyLyType = forcastBasisDetailProfile.TyLyType;
                glbProfile.MerchType = forcastBasisDetailProfile.MerchandiseType;
                glbProfile.MerchPhRid = forcastBasisDetailProfile.MerchPhRId;
                glbProfile.MerchPhlSequence = forcastBasisDetailProfile.MerchPhlSequence;

                if (forcastBasisDetailProfile.MerchandiseType == eMerchandiseType.SameNode
                    || forcastBasisDetailProfile.MerchOffset < 0)
                {
                    glbProfile.MerchOffset = 0;
                }
                else
                {
                    glbProfile.MerchOffset = forcastBasisDetailProfile.MerchOffset;
                }
                groupBasisList.Add(glbProfile);
                index--;
            }

            return groupBasisList;
        }
        override public bool MethodSetData(ROMethodProperties methodProperties, ref string message, bool processingApply)
        {
            ROPlanningForecastMethodProperties properties = (ROPlanningForecastMethodProperties)methodProperties;

            try
            {
                Template_IND = methodProperties.IsTemplate;
                Plan_HN_RID = properties.Merchandise.Key;
                HighLevelInd = properties.HighLevel;
                LowLevelsInd = properties.LowLevels;
                Plan_FV_RID = properties.Version.Key;
                CDR_RID = properties.DateRange.Key;

                LowLevelsOffset = properties.OverrideLowLevel.LowLevel.LevelOffset;
                LowLevelsSequence = properties.OverrideLowLevel.LowLevel.LevelSequence;
                LowLevelsType = (eLowLevelsType)properties.OverrideLowLevel.LowLevel.LevelType;

                OverrideLowLevelRid = properties.OverrideLowLevel.OverrideLowLevelsModel.Key;
                if (properties.OverrideLowLevel.IsCustomModel)
                {
                    CustomOLL_RID = properties.OverrideLowLevel.OverrideLowLevelsModel.Key;
                }
                else
                {
                    CustomOLL_RID = Include.NoRID; 
                }
                Chain_FV_RID = properties.ChainVersion.Key;
                Bal_Sales_Ind = properties.SalesBalance;
                Bal_Stock_Ind = properties.StockBalance;
                ApplyTrendOptionsInd = SetApplyTrendOptions(properties.ApplyTrendOptions);
                ApplyTrendOptionsWOSValue = properties.ApplyTrendOptionsValue;
                _attributeChanged = SG_RID != properties.Attribute.Key;
                SG_RID = properties.Attribute.Key;

                if (properties.AttributeSetIsSet)
                {
                    _attributeSetKey = properties.AttributeSet.Key;
                    // check to determine if attribute set is part of new attribute
                    // if not, set so will use first set in the attribute
                    if (_attributeChanged)
                    {
                        // delete all settings
						GLFProfileList.Clear();
                        ProfileList attributeSetList = StoreMgmt.StoreGroup_GetLevelListViewList(SG_RID);
                        if (attributeSetList.FindKey(aKey: _attributeSetKey) == null)
                        {
                            _attributeSetKey = Include.NoRID;
                        }
                    }
                }

                int defaultAttributeSetKey = GetDefaultGLFRid();
                bool stockMinMaxInheritedChanged = false;

                // only update attribute set values if attribute did not change
                if (!_attributeChanged)
                {
                    GroupLevelFunctionProfile defaultAttributeSetProfile = (GroupLevelFunctionProfile)_GLFProfileList.FindKey(defaultAttributeSetKey);

                    ROPlanningForecastMethodAttributeSetProperties item = properties.AttributeSetValues;

                    GroupLevelFunctionProfile newGLFP = (GroupLevelFunctionProfile)_GLFProfileList.FindKey(item.AttributeSet.Key);
                    // if use default attribute set, sync to default set values
                    if (item.IsAttributeSetToUseDefault
                        && defaultAttributeSetKey > 0)
                    {
                        bool addedAttributeSet = false;
                        if (newGLFP == null)
                        {
                            newGLFP = new GroupLevelFunctionProfile(item.AttributeSet.Key);
                            GLFProfileList.Add(newGLFP);
                            addedAttributeSet = true;
                        }
                        
                        // If not already set to use default, copy default values to the set
                        if (defaultAttributeSetProfile != null
                            && !newGLFP.Use_Default_IND)
                        {
                            newGLFP = defaultAttributeSetProfile.CopyTo(newGLFP, SAB.ApplicationServerSession, false, true, true);
                            newGLFP.Use_Default_IND = item.IsAttributeSetToUseDefault;
                            newGLFP.Plan_IND = true;  // set to plan if changed to use default
                            if (addedAttributeSet)  // copy stock min/max if new values
                            {
                                foreach (int Key in defaultAttributeSetProfile.Group_Level_Nodes.Keys)
                                {
                                    GroupLevelNodeFunction glnf = (GroupLevelNodeFunction)defaultAttributeSetProfile.Group_Level_Nodes[Key];
                                    glnf = glnf.Copy();
                                    glnf.SglRID = item.AttributeSet.Key;
                                    newGLFP.Group_Level_Nodes.Add(glnf.HN_RID, glnf);
                                }
                            }
                        }
						
                        newGLFP.Default_IND = false;

                        GroupLevelNodeFunction GLNFunction;

                        if (newGLFP.Group_Level_Nodes.ContainsKey(Plan_HN_RID))
                        {
                            GLNFunction = (GroupLevelNodeFunction)newGLFP.Group_Level_Nodes[Plan_HN_RID];
                        }
                        else
                        {
                            GLNFunction = new GroupLevelNodeFunction();
                        }

                        if (item.StockMerchandise.Key > 0)
                        {
                            GLNFunction.HN_RID = item.StockMerchandise.Key;
                        }
                        else
                        {
                            GLNFunction.HN_RID = Plan_HN_RID;
                        }
                        GLNFunction.SglRID = item.AttributeSet.Key;
                        GLNFunction.ApplyMinMaxesInd = item.ApplyMinMax;
                        stockMinMaxInheritedChanged = GLNFunction.MinMaxInheritType != item.MinMaxInheritType;
                        GLNFunction.MinMaxInheritType = item.MinMaxInheritType;

                        SetStockMinMax(item.StoreGrades, ref GLNFunction, stockMinMaxInheritedChanged, defaultAttributeSetProfile);
                        if (!newGLFP.Group_Level_Nodes.ContainsKey(GLNFunction.HN_RID))
                        {
                            newGLFP.Group_Level_Nodes.Add(GLNFunction.HN_RID, GLNFunction);
                        }
                    }
                    else
                    {
                        if (newGLFP == null)
                        {
                            newGLFP = new GroupLevelFunctionProfile(item.AttributeSet.Key);
                            GLFProfileList.Add(newGLFP);
                        }

                        bool isDefaultSet = newGLFP.Default_IND;

                        GroupLevelNodeFunction GLNFunction;

                        if (newGLFP.Group_Level_Nodes.ContainsKey(Plan_HN_RID))
                        {
                            GLNFunction = (GroupLevelNodeFunction)newGLFP.Group_Level_Nodes[Plan_HN_RID];
                        }
                        else
                        {
                            GLNFunction = new GroupLevelNodeFunction();
                        }

                        // no longer the default set, so remove all use default settings
                        if (isDefaultSet
                            && !item.IsDefaultProperties)
                        {
                            SetAllUseDefaultToFalse();
                        }
                        newGLFP.Default_IND = item.IsDefaultProperties;
                        newGLFP.Plan_IND = item.IsAttributeSetForecast;
                        newGLFP.Use_Default_IND = item.IsAttributeSetToUseDefault;
                        newGLFP.GLFT_ID = item.ForecastMethod.Key;
                        newGLFP.GLSB_ID = item.SmoothBy.Key;

                        if (item.StockMerchandise.Key > 0)
                        {
                            GLNFunction.HN_RID = item.StockMerchandise.Key;
                        }
                        else
                        {
                            GLNFunction.HN_RID = Plan_HN_RID;
                        }
                        GLNFunction.SglRID = item.AttributeSet.Key;
                        GLNFunction.ApplyMinMaxesInd = item.ApplyMinMax;
                        stockMinMaxInheritedChanged = GLNFunction.MinMaxInheritType != item.MinMaxInheritType;
                        GLNFunction.MinMaxInheritType = item.MinMaxInheritType;
                        //TO DO:: Stock Min Max to be assigned here.
                        //GLNFunction.Stock_MinMax
                        SetStockMinMax(item.StoreGrades, ref GLNFunction, stockMinMaxInheritedChanged, defaultAttributeSetProfile);
                        if (!newGLFP.Group_Level_Nodes.ContainsKey(GLNFunction.HN_RID))
                        {
                            newGLFP.Group_Level_Nodes.Add(GLNFunction.HN_RID, GLNFunction);
                        }
                        newGLFP.TY_Weight_Multiple_Basis_Ind = item.EqualizingWaitingTY;
                        newGLFP.LY_Weight_Multiple_Basis_Ind = item.EqualizingWaitingLY;
                        newGLFP.LY_Alt_IND = item.IsAlternateLY;
                        newGLFP.Trend_Alt_IND = item.IsAlternateApplyTrendTo;
                        newGLFP.Apply_Weight_Multiple_Basis_Ind = item.EqualizingWaitingApplyTrendTo;
                        newGLFP.Proj_Curr_Wk_Sales_IND = item.IsProjectCurrentWeekSales;
                        //TO DO:: All Trend Caps data need to assign here.
						// Update object if exists, otherwise create new one
                        TrendCapsProfile tcp = (TrendCapsProfile)newGLFP.Trend_Caps.FindKey(newGLFP.Key);
                        if (tcp == null)
                        {
                            tcp = new TrendCapsProfile(newGLFP.Key);
                            newGLFP.Trend_Caps.Add(tcp);
                        }
                        tcp.TrendCapID = item.TrendCapId;
                        tcp.LowLimit = item.TrendCapsLowLimit;
                        tcp.HighLimit = item.TrendCapsHighLimit;
                        tcp.TolPct = item.TrendCapsTolerance;

                        if (item.ForecastMethod.Key == eGroupLevelFunctionType.PercentContribution)
                        {
                            ProfileList groupBasisProfileList = GetGroupBasisProfileList(item.ROForecastBasisDetailProfiles);
                            newGLFP.GroupLevelBasis = groupBasisProfileList;
                        }
                        else if (item.ForecastMethod.Key == eGroupLevelFunctionType.TyLyTrend)
                        {
                            ProfileList groupBasisProfileListTY = GetGroupBasisProfileList(item.ROForecastBasisDetailProfilesTY);
                            ProfileList groupBasisProfileListLY = GetGroupBasisProfileList(item.ROForecastBasisDetailProfilesLY);
                            ProfileList groupBasisProfileListTrend = GetGroupBasisProfileList(item.ROForecastBasisDetailProfilesApplyTrendTo);
                            newGLFP.GroupLevelBasis.Clear();
                            newGLFP.GroupLevelBasis.AddRange(groupBasisProfileListTY);
                            newGLFP.GroupLevelBasis.AddRange(groupBasisProfileListLY);
                            newGLFP.GroupLevelBasis.AddRange(groupBasisProfileListTrend);
                            //To DO:: Need to check how basis objects to be assigned.
                        }

                       

                        // Default set has changed, so copy values to all sets that use default
                        if (newGLFP.Default_IND)
                        {
                            UpdateSetsUsingDefault(
                                defaultAttributeSetProfile: newGLFP
                                );
                        }
                    }
                }

                // If performing a save and low levels enabled
                // copy high level stock min/maxes to all low levels
                if (!processingApply
                    && LowLevelsInd)
                {
                    CopyHighLevelStockMinimumsMaximumsToLowLevels();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void UpdateSetsUsingDefault(GroupLevelFunctionProfile defaultAttributeSetProfile)
        {
            GroupLevelFunctionProfile attributeSetProfile;

            for (int i = 0; i < _GLFProfileList.Count; i++)
            {
                attributeSetProfile = (GroupLevelFunctionProfile)_GLFProfileList[i];
                if (attributeSetProfile.Use_Default_IND)
                {
                    attributeSetProfile = defaultAttributeSetProfile.CopyTo(attributeSetProfile, SAB.ApplicationServerSession, false, true, true);
                    attributeSetProfile.Default_IND = false;
                    attributeSetProfile.Plan_IND = true;
                    attributeSetProfile.Use_Default_IND = true;
                    // replace the attribute set key
                    // need to clear and rebuild Trend_Caps list since cannot directly replace key in profile
                    ArrayList trendCaps = new ArrayList();
                    foreach (TrendCapsProfile trendCapsProfile in attributeSetProfile.Trend_Caps)
                    {
                        trendCapsProfile.Key = attributeSetProfile.Key;
                        trendCaps.Add(trendCapsProfile);
                    }
                    attributeSetProfile.Trend_Caps.Clear();
                    foreach (TrendCapsProfile trendCapsProfile in trendCaps)
                    {
                        attributeSetProfile.Trend_Caps.Add(trendCapsProfile);
                    }
                    _GLFProfileList.Update(attributeSetProfile);
                }
            }
        }

        private void CopyHighLevelStockMinimumsMaximumsToLowLevels()
        {
            GroupLevelFunctionProfile attributeSetProfile;
            GroupLevelNodeFunction highLevelGroupLevelNodeFunction;
            GroupLevelNodeFunction lowLevelGroupLevelNodeFunction;
            StockMinMaxProfile stockMinimumMaximumProfile;

            // process all attribute sets
            for (int i = 0; i < _GLFProfileList.Count; i++)
            {
                attributeSetProfile = (GroupLevelFunctionProfile)_GLFProfileList[i];
                if (attributeSetProfile.Group_Level_Nodes.ContainsKey(Plan_HN_RID))
                {
                    // get the values for the high level node
                    highLevelGroupLevelNodeFunction = (GroupLevelNodeFunction)attributeSetProfile.Group_Level_Nodes[Plan_HN_RID];
                    // clear all node values and put the high level back
                    attributeSetProfile.Group_Level_Nodes.Clear();
                    attributeSetProfile.Group_Level_Nodes.Add(highLevelGroupLevelNodeFunction.HN_RID, highLevelGroupLevelNodeFunction);
                    // build the list of all low levels
                    PopulateOverrideList();
                    // copy high level settings to each low level
                    foreach (LowLevelVersionOverrideProfile lowLevel in LowlevelOverrideList)
                    {
                        // copy the node settings
                        lowLevelGroupLevelNodeFunction = highLevelGroupLevelNodeFunction.Copy();
                        // update node to low level key
                        lowLevelGroupLevelNodeFunction.HN_RID = lowLevel.NodeProfile.Key;
                        // copy all stock settings
                        lowLevelGroupLevelNodeFunction.Stock_MinMax.Clear();
                        foreach (StockMinMaxProfile stockMinMax in highLevelGroupLevelNodeFunction.Stock_MinMax)
                        {
                            stockMinimumMaximumProfile = new StockMinMaxProfile(stockMinMax.Key);
                            stockMinimumMaximumProfile.HN_RID = lowLevel.NodeProfile.Key;
                            stockMinimumMaximumProfile.MethodRid = stockMinMax.MethodRid;
                            stockMinimumMaximumProfile.StoreGroupLevelRid = stockMinMax.StoreGroupLevelRid;
                            stockMinimumMaximumProfile.Boundary = stockMinMax.Boundary;
                            stockMinimumMaximumProfile.DateRangeRid = stockMinMax.DateRangeRid;
                            stockMinimumMaximumProfile.MaximumStock = stockMinMax.MaximumStock;
                            stockMinimumMaximumProfile.MinimumStock = stockMinMax.MinimumStock;
                            lowLevelGroupLevelNodeFunction.Stock_MinMax.Add(stockMinimumMaximumProfile);
                        }
                        // add low level to attribute set node list
                        attributeSetProfile.Group_Level_Nodes.Add(lowLevelGroupLevelNodeFunction.HN_RID, lowLevelGroupLevelNodeFunction);
                    }
                }
                else
                {
                    // clear all node values
                    attributeSetProfile.Group_Level_Nodes.Clear();
                }
            }
        }



        override public ROMethodProperties MethodCopyData()
        {
            throw new NotImplementedException("MethodCopyData is not implemented");
        }
        // END RO-642 - RDewey
    }
}
