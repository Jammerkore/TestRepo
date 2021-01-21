using MIDRetail.DataCommon;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Logility.ROWebSharedTypes
{

    public class ROPlanningReviewSelectionProperties
    {
        public KeyValuePair<int, string> StoreAttribute { get; set; }
        public KeyValuePair<int, string> Filter { get; set; }

        public eStorePlanSelectedGroupBy GroupBy { get; set; }

        public KeyValuePair<int, string> View { get; set; }
        public eDisplayTimeBy DisplayTimeBy { get; set; }

        public bool InEligibleStores { get; set; }

        public bool SimilarStores { get; set; }

        public eLowLevelsType LowLevelsType { get; set; }

        public int LowLevelsOffset { get; set; }

        public int LowLevelsSequence { get; set; }

        public string LowLevelsValue { get; set; }

        public bool IsLadder { get; set; }

        public bool IsMulti { get; set; }

        public ePlanSessionType PlanSessionType { get; set; }
        public string ComputationsMode { get; set; }
        public KeyValuePair<int, string> Version { get; set; }

        public KeyValuePair<int, string> StoreVersion { get; set; }

        public KeyValuePair<int, string> StoreNode { get; set; }

        public KeyValuePair<int, string> Node { get; set; }
        public KeyValuePair<int, string> DateRange { get; set; }
        public List<ROBasisProfile> ROBasisProfiles { get; set; }
        public KeyValuePair<int, string> LowLevelVersion { get; set; }

        public KeyValuePair<int, string> OverrideLowLevelsModel { get; set; }

    }

    [DataContract(Name = "ROPlanningForecastMethodProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROPlanningForecastMethodProperties : ROMethodProperties
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _merchandise;

        public KeyValuePair<int, string> Merchandise
        {
            get { return _merchandise; }
            set { _merchandise = value; }
        }
        [DataMember(IsRequired = true)]
        private bool _highLevel;

        public bool HighLevel
        {
            get { return _highLevel; }
            set { _highLevel = value; }
        }
        [DataMember(IsRequired = true)]
        private bool _lowLevels;

        public bool LowLevels
        {
            get { return _lowLevels; }
            set { _lowLevels = value; }
        }
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _version;

        public KeyValuePair<int, string> Version
        {
            get { return _version; }
            set { _version = value; }
        }

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _lowLevel;

        public KeyValuePair<int, string> LowLevel
        {
            get { return _lowLevel; }
            set { _lowLevel = value; }
        }

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _dateRange;

        public KeyValuePair<int, string> DateRange
        {
            get { return _dateRange; }
            set { _dateRange = value; }
        }

        [DataMember(IsRequired = true)]
        private ROOverrideLowLevel _overrideLowLevel;

        public ROOverrideLowLevel OverrideLowLevel
        {
            get { return _overrideLowLevel; }
            set { _overrideLowLevel = value; }
        }
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _chainForecastVersion;

        public KeyValuePair<int, string> ChainForcastVersion
        {
            get { return _chainForecastVersion; }
            set { _chainForecastVersion = value; }
        }
        [DataMember(IsRequired = true)]
        private bool _salesBalance;

        public bool SalesBalance
        {
            get { return _salesBalance; }
            set { _salesBalance = value; }
        }
        [DataMember(IsRequired = true)]
        private bool _stockBalance;

        public bool StockBalance
        {
            get { return _stockBalance; }
            set { _stockBalance = value; }
        }

        [DataMember(IsRequired = true)]
        private eApplyTrendOptions _applyTrendOptions;

        public eApplyTrendOptions ApplyTrendOptions
        {
            get { return _applyTrendOptions; }
            set { _applyTrendOptions = value; }
        }
        [DataMember(IsRequired = true)]
        private float _applyTrendOptionsValue;

        public float ApplyTrendOptionsValue
        {
            get { return _applyTrendOptionsValue; }
            set { _applyTrendOptionsValue = value; }
        }

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _storeAttribute;

        public KeyValuePair<int, string> StoreAttribute
        {
            get { return _storeAttribute; }
            set { _storeAttribute = value; }
        }


        [DataMember(IsRequired = true)]
        private List<ROPlanningForecastMethodAttributeSetProperties> _attributeSetProperties;

        public List<ROPlanningForecastMethodAttributeSetProperties> AttributeSetProperties
        {
            get { return _attributeSetProperties; }
            //set { _attributeSetProperties = value; }
        }

        public ROPlanningForecastMethodProperties(
            KeyValuePair<int, string> kvMethod, 
            string sDescription, int iUserKey,
            KeyValuePair<int, string> kvMerchandise, 
            bool bHighLevel, 
            bool bLowLevels, 
            KeyValuePair<int, string> kvVersion,
            KeyValuePair<int, string> kvLowLevel, 
            KeyValuePair<int, string> kvDateRange, 
            ROOverrideLowLevel overrideLowLevel,
            KeyValuePair<int, string> kvChainForecastVersion, 
            bool bSalesBalance, 
            bool bStockBalance, 
            eApplyTrendOptions applyTrendOptions,
            float fApplyTrendOptionsValue, 
            KeyValuePair<int, string> kvStoreAttribute,
            bool isTemplate = false
            ) 
            : base(
                eMethodType.OTSPlan, 
                kvMethod, 
                sDescription, 
                iUserKey,
                isTemplate)
        {
            _merchandise = kvMerchandise;
            _highLevel = bHighLevel;
            _lowLevels = bLowLevels;
            _version = kvVersion;
            _lowLevel = kvLowLevel;
            _dateRange = kvDateRange;
            _overrideLowLevel = overrideLowLevel;
            _chainForecastVersion = kvChainForecastVersion;
            _salesBalance = bSalesBalance;
            _stockBalance = bStockBalance;
            _applyTrendOptions = applyTrendOptions;
            _applyTrendOptionsValue = fApplyTrendOptionsValue;
            _storeAttribute = kvStoreAttribute;
            _attributeSetProperties = new List<ROPlanningForecastMethodAttributeSetProperties>();
        }
    }

    [DataContract(Name = "ROPlanningForecastMethodAttributeSetProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROPlanningForecastMethodAttributeSetProperties
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _attributeSet;

        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
            set { _attributeSet = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _isDefaultProperties;
        public bool IsDefaultProperties
        {
            get { return _isDefaultProperties; }
            set { _isDefaultProperties = value; }
        }
        [DataMember(IsRequired = true)]
        private bool _isAttributeSetForecast;

        public bool IsAttributeSetForecast
        {
            get { return _isAttributeSetForecast; }
            set { _isAttributeSetForecast = value; }
        }
        [DataMember(IsRequired = true)]
        private bool _isAttributeSetToUseDefault;

        public bool IsAttributeSetToUseDefault
        {
            get { return _isAttributeSetToUseDefault; }
            set { _isAttributeSetToUseDefault = value; }
        }


        [DataMember(IsRequired = true)]
        private KeyValuePair<eGroupLevelFunctionType, string> _forecastMethod;

        public KeyValuePair<eGroupLevelFunctionType, string> ForecastMethod
        {
            get { return _forecastMethod; }
            set { _forecastMethod = value; }
        }

        [DataMember(IsRequired = true)]
        private KeyValuePair<eGroupLevelSmoothBy, string> _smoothBy;

        public KeyValuePair<eGroupLevelSmoothBy, string> SmoothBy
        {
            get { return _smoothBy; }
            set { _smoothBy = value; }
        }
        [DataMember(IsRequired = true)]
        private List<ROForecastingBasisDetailsProfile> _roForecastBasisDetailProfiles;

        public List<ROForecastingBasisDetailsProfile> ROForecastBasisDetailProfiles
        {
            get { return _roForecastBasisDetailProfiles; }
            set { _roForecastBasisDetailProfiles = value; }
        }
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _stockMerchandise;

        public KeyValuePair<int, string> StockMerchandise
        {
            get { return _stockMerchandise; }
            set { _stockMerchandise = value; }
        }
        [DataMember(IsRequired = true)]
        private bool _applyMinMax;

        public bool ApplyMinMax
        {
            get { return _applyMinMax; }
            set { _applyMinMax = value; }
        }

        [DataMember(IsRequired = true)]
        private eMinMaxInheritType _minMaxInheritType;

        public eMinMaxInheritType MinMaxInheritType
        {
            get { return _minMaxInheritType; }
            set { _minMaxInheritType = value; }
        }
        [DataMember(IsRequired = true)]
        private List<ROPlanningStoreGrade> _storeGrades;

        public List<ROPlanningStoreGrade> StoreGrades
        {
            get { return _storeGrades; }
            set { _storeGrades = value; }
        }
        #region TY
        [DataMember(IsRequired = true)]
        private List<ROForecastingBasisDetailsProfile> _roForecastBasisDetailProfilesTY;

        public List<ROForecastingBasisDetailsProfile> ROForecastBasisDetailProfilesTY
        {
            get { return _roForecastBasisDetailProfilesTY; }
            set { _roForecastBasisDetailProfilesTY = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _equalizingWaitingTY;

        public bool EqualizingWaitingTY
        {
            get { return _equalizingWaitingTY; }
            set { _equalizingWaitingTY = value; }
        }


        #endregion

        #region LY
        [DataMember(IsRequired = true)]
        private List<ROForecastingBasisDetailsProfile> _roForecastBasisDetailProfilesLY;

        public List<ROForecastingBasisDetailsProfile> ROForecastBasisDetailProfilesLY
        {
            get { return _roForecastBasisDetailProfilesLY; }
            set { _roForecastBasisDetailProfilesLY = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _equalizingWaitingLY;

        public bool EqualizingWaitingLY
        {
            get { return _equalizingWaitingLY; }
            set { _equalizingWaitingLY = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _isAlternateLY;

        public bool IsAlternateLY
        {
            get { return _isAlternateLY; }
            set { _isAlternateLY = value; }
        }

        #endregion

        #region ApplyTrendTo
        [DataMember(IsRequired = true)]
        private List<ROForecastingBasisDetailsProfile> _roForecastBasisDetailProfilesApplyTrendTo;

        public List<ROForecastingBasisDetailsProfile> ROForecastBasisDetailProfilesApplyTrendTo
        {
            get { return _roForecastBasisDetailProfilesApplyTrendTo; }
            set { _roForecastBasisDetailProfilesApplyTrendTo = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _equalizingWaitingApplyTrendTo;

        public bool EqualizingWaitingApplyTrendTo
        {
            get { return _equalizingWaitingApplyTrendTo; }
            set { _equalizingWaitingApplyTrendTo = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _isAlternateApplyTrendTo;

        public bool IsAlternateApplyTrendTo
        {
            get { return _isAlternateApplyTrendTo; }
            set { _isAlternateApplyTrendTo = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _isProjectCurrentWeekSales;

        public bool IsProjectCurrentWeekSales
        {
            get { return _isProjectCurrentWeekSales; }
            set { _isProjectCurrentWeekSales = value; }
        }


        #endregion
        #region Trend Caps
        [DataMember(IsRequired = true)]
        private eTrendCapID _trendCapId;

        public eTrendCapID TrendCapId
        {
            get { return _trendCapId; }
            set { _trendCapId = value; }
        }

        [DataMember(IsRequired = true)]
        private int _trendCapsTolerance;

        public int TrendCapsTolerance
        {
            get { return _trendCapsTolerance; }
            set { _trendCapsTolerance = value; }
        }
        [DataMember(IsRequired = true)]
        private int _trendCapsLowLimit;


        public int TrendCapsLowLimit
        {
            get { return _trendCapsLowLimit; }
            set { _trendCapsLowLimit = value; }
        }

        private int _trendCapsHighLimit;

        public int TrendCapsHighLimit
        {
            get { return _trendCapsHighLimit; }
            set { _trendCapsHighLimit = value; }
        }

        #endregion
        public ROPlanningForecastMethodAttributeSetProperties(KeyValuePair<int, string> kvAttributeSet, bool bIsDefaultProperties, bool bIsAttributeSetForecast, bool bIsAttributeSetToUseDefault,
            KeyValuePair<eGroupLevelFunctionType, string> forecastMethod,
            KeyValuePair<eGroupLevelSmoothBy, string> smoothBy, List<ROForecastingBasisDetailsProfile> roForecastBasisDetailProfiles, KeyValuePair<int, string> kvStockMerchandise,
            bool bApplyMinMax, eMinMaxInheritType minMaxInheritType, List<ROPlanningStoreGrade> storeGrades, List<ROForecastingBasisDetailsProfile> roForecastBasisDetailProfilesTY,
            bool bEqualizingWaitingTY, List<ROForecastingBasisDetailsProfile> roForecastBasisDetailProfilesLY, bool bEqualizingWaitingLY, bool bIsAlternateLY,
            List<ROForecastingBasisDetailsProfile> roForcastBasisDetailProfilesApplyTrendTo, bool bEqualizingWaitingApplyTrendTo, bool bIsAlternateApplyTrendTo, bool bIsProjectCurrentWeekSales,
            eTrendCapID trendCapId, int iTrendCapsTolerance, int iTrendCapsLowLimit, int iTrendCapsHighLimit

            )
        {
            _attributeSet = kvAttributeSet;
            _isDefaultProperties = bIsDefaultProperties;
            _isAttributeSetForecast = bIsAttributeSetForecast;
            _isAttributeSetToUseDefault = bIsAttributeSetToUseDefault;
            _forecastMethod = forecastMethod;
            _smoothBy = smoothBy;
            _roForecastBasisDetailProfiles = roForecastBasisDetailProfiles;
            _stockMerchandise = kvStockMerchandise;
            _applyMinMax = bApplyMinMax;
            _minMaxInheritType = MinMaxInheritType;
            _storeGrades = storeGrades;
            _roForecastBasisDetailProfilesTY = roForecastBasisDetailProfilesTY;
            _equalizingWaitingTY = bEqualizingWaitingTY;
            _roForecastBasisDetailProfilesLY = roForecastBasisDetailProfilesLY;
            _equalizingWaitingLY = bEqualizingWaitingLY;
            _isAlternateLY = bIsAlternateLY;
            _roForecastBasisDetailProfilesApplyTrendTo = roForcastBasisDetailProfilesApplyTrendTo;
            _equalizingWaitingApplyTrendTo = bEqualizingWaitingTY;
            _isAlternateApplyTrendTo = bIsAlternateApplyTrendTo;
            _isProjectCurrentWeekSales = bIsProjectCurrentWeekSales;
            _trendCapId = trendCapId;
            _trendCapsTolerance = iTrendCapsTolerance;
            _trendCapsLowLimit = iTrendCapsLowLimit;
            _trendCapsHighLimit = iTrendCapsHighLimit;
        }
    }

    [DataContract(Name = "ROPlanningForecastExportProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROPlanningForecastExportProperties : ROMethodProperties
    {
        [DataMember(IsRequired = true)]
        private ePlanType _planType;

        public ePlanType PlanType
        {
            get { return _planType; }
            set { _planType = value; }
        }

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _merchandise;

        public KeyValuePair<int, string> Merchandise
        {
            get { return _merchandise; }
            set { _merchandise = value; }
        }
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _version;

        public KeyValuePair<int, string> Version
        {
            get { return _version; }
            set { _version = value; }
        }

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _timePeriod;

        public KeyValuePair<int, string> TimePeriod
        {
            get { return _timePeriod; }
            set { _timePeriod = value; }
        }
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _filter;

        public KeyValuePair<int, string> Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _isLowLevels;

        public bool IsLowLevels
        {
            get { return _isLowLevels; }
            set { _isLowLevels = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _isLowLevelsOnly;

        public bool IsLowLevelsOnly
        {
            get { return _isLowLevelsOnly; }
            set { _isLowLevelsOnly = value; }
        }



        [DataMember(IsRequired = true)]
        private ROLevelInformation _lowLevel;

        public ROLevelInformation LowLevel
        {
            get { return _lowLevel; }
            set { _lowLevel = value; }
        }

        [DataMember(IsRequired = true)]
        private ROOverrideLowLevel _overrideLowLevel;

        public ROOverrideLowLevel OverrideLowLevel
        {
            get { return _overrideLowLevel; }
            set { _overrideLowLevel = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _isExtractIneligibleStores;

        public bool IsExtractIneligibleStores
        {
            get { return _isExtractIneligibleStores; }
            set { _isExtractIneligibleStores = value; }
        }

        [DataMember(IsRequired = true)]
        private ROVariableGroupings _variableList;
        public ROVariableGroupings VariableList
        {
            get { return _variableList; }
            set { _variableList = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _isUseDefaultSettings;

        public bool UseDefaultSettings
        {
            get { return _isUseDefaultSettings; }
            set { _isUseDefaultSettings = value; }
        }

        [DataMember(IsRequired = true)]
        private eExportType _exportType;

        public eExportType ExportType
        {
            get { return _exportType; }
            set { _exportType = value; }
        }

        [DataMember(IsRequired = true)]
        private string _delimiter;

        public string Delimiter
        {
            get { return _delimiter; }
            set { _delimiter = value; }
        }

        [DataMember(IsRequired = true)]
        private string _csvFileExtension;

        public string CSVFileExtension
        {
            get { return _csvFileExtension; }
            set { _csvFileExtension = value; }
        }

        [DataMember(IsRequired = true)]
        private eExportDateType _exportDateType;

        public eExportDateType ExportDateType
        {
            get { return _exportDateType; }
            set { _exportDateType = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _isExtractPreInitValues;

        public bool IsExtractPreInitValues
        {
            get { return _isExtractPreInitValues; }
            set { _isExtractPreInitValues = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _isExcludeZeroValues;

        public bool IsExcludeZeroValues
        {
            get { return _isExcludeZeroValues; }
            set { _isExcludeZeroValues = value; }
        }

        [DataMember(IsRequired = true)]
        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _isDateStamp;

        public bool IsDateStamp
        {
            get { return _isDateStamp; }
            set { _isDateStamp = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _isTimeStamp;

        public bool IsTimeStamp
        {
            get { return _isTimeStamp; }
            set { _isTimeStamp = value; }
        }

        [DataMember(IsRequired = true)]
        private eExportSplitType _splitType;

        public eExportSplitType SplitType
        {
            get { return _splitType; }
            set { _splitType = value; }
        }

        [DataMember(IsRequired = true)]
        private int _splitNumEntries;

        public int SplitNumEntries
        {
            get { return _splitNumEntries; }
            set { _splitNumEntries = value; }
        }

        [DataMember(IsRequired = true)]
        private int _concurrentProcesses;

        public int ConcurrentProcesses
        {
            get { return _concurrentProcesses; }
            set { _concurrentProcesses = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _isCreateFlagfile;

        public bool IsCreateFlagfile
        {
            get { return _isCreateFlagfile; }
            set { _isCreateFlagfile = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _isCreateEndfile;

        public bool IsCreateEndfile
        {
            get { return _isCreateEndfile; }
            set { _isCreateEndfile = value; }
        }

        [DataMember(IsRequired = true)]
        private string _flagFileExtn;

        public string FlagFileExtension
        {
            get { return _flagFileExtn; }
            set { _flagFileExtn = value; }
        }

        [DataMember(IsRequired = true)]
        private string _endFileExtn;

        public string EndFileExtension
        {
            get { return _endFileExtn; }
            set { _endFileExtn = value; }
        }

        public ROPlanningForecastExportProperties(
            KeyValuePair<int, string> kvpMethod, 
            string sDescription, 
            int iUserKey,
            ePlanType planType, 
            KeyValuePair<int, string> kvpMerchandise, 
            KeyValuePair<int, string> kvpVersion, 
            KeyValuePair<int, string> kvpTimePeriod,
            KeyValuePair<int, string> kvpFilter, 
            bool bIsLowLevels, 
            bool bIsLowLevelsOnly, 
            ROLevelInformation lowLevel, 
            ROOverrideLowLevel overrideLowLevel,
            bool bIsExtractIneligibleStores, 
            ROVariableGroupings alVariableList, 
            bool bUseDefaultSettings, 
            eExportType exportType, 
            string sDelimiter,
            string sCSVFileExtension, 
            eExportDateType exportDateType, 
            bool bIsExtractPreInitValues, 
            bool bIsExcludeZeroValues,
            string sFilePath, 
            bool bIsDateStamp, 
            bool bIsTimeStamp, 
            eExportSplitType exportSplitType, 
            int iSplitNumEntries,
            int iConcurrentProcesses, 
            bool bIsCreateFlagfile, 
            bool bIsCreateEndfile, 
            string sFlagFileExtension, 
            string sEndFileExtension,
            bool isTemplate = false
            ) 
            : base(
                  eMethodType.Export, 
                  kvpMethod, 
                  sDescription, 
                  iUserKey,
                  isTemplate
                  )
        {
            _planType = planType;
            _merchandise = kvpMerchandise;
            _version = kvpVersion;
            _timePeriod = kvpTimePeriod;
            _filter = kvpFilter;
            _isLowLevels = bIsLowLevels;
            _isLowLevelsOnly = bIsLowLevelsOnly;
            _lowLevel = lowLevel;
            _overrideLowLevel = overrideLowLevel;
            _isExtractIneligibleStores = bIsExtractIneligibleStores;
            _variableList = alVariableList;
            _isUseDefaultSettings = bUseDefaultSettings;
            _exportType = exportType;
            _delimiter = sDelimiter;
            _csvFileExtension = sCSVFileExtension;
            _exportDateType = exportDateType;
            _isExtractPreInitValues = bIsExtractPreInitValues;
            _isExcludeZeroValues = bIsExcludeZeroValues;
            _filePath = sFilePath;
            _isDateStamp = bIsDateStamp;
            _isTimeStamp = bIsTimeStamp;
            _splitType = exportSplitType;
            _splitNumEntries = iSplitNumEntries;
            _concurrentProcesses = iConcurrentProcesses;
            _isCreateFlagfile = bIsCreateFlagfile;
            _isCreateEndfile = bIsCreateEndfile;
            _flagFileExtn = sFlagFileExtension;
            _endFileExtn = sEndFileExtension;

        }
    }

    

    [DataContract(Name = "ROPlanningGlobalUnlockProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROPlanningGlobalLockUnlockProperties : ROMethodProperties
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _merchandise;

        public KeyValuePair<int, string> Merchandise
        {
            get { return _merchandise; }
            set { _merchandise = value; }
        }
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _version;

        public KeyValuePair<int, string> Version
        {
            get { return _version; }
            set { _version = value; }
        }
        [DataMember(IsRequired = true)]
        private bool _isMuliLevel;

        public bool IsMultiLevel
        {
            get { return _isMuliLevel; }
            set { _isMuliLevel = value; }
        }


        [DataMember(IsRequired = true)]
        private ROLevelInformation _fromLevel;

        public ROLevelInformation FromLevel
        {
            get { return _fromLevel; }
            set { _fromLevel = value; }
        }

        [DataMember(IsRequired = true)]
        private ROLevelInformation _toLevel;

        public ROLevelInformation ToLevel
        {
            get { return _toLevel; }
            set { _toLevel = value; }
        }
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _timePeriod;

        public KeyValuePair<int, string> TimePeriod
        {
            get { return _timePeriod; }
            set { _timePeriod = value; }
        }
        [DataMember(IsRequired = true)]
        private ROOverrideLowLevel _overrideLowLevel;

        public ROOverrideLowLevel OverrideLowLevel
        {
            get { return _overrideLowLevel; }
            set { _overrideLowLevel = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _isStoresOptions;

        public bool IsStoreOptions
        {
            get { return _isStoresOptions; }
            set { _isStoresOptions = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _isChainOptions;

        public bool IsChainOptions
        {
            get { return _isChainOptions; }
            set { _isChainOptions = value; }
        }


        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _storeAttribute;

        public KeyValuePair<int, string> StoreAttribute
        {
            get { return _storeAttribute; }
            set { _storeAttribute = value; }
        }
        [DataMember(IsRequired = true)]
        private ArrayList _attributeSet;

        public ArrayList AttributeSet
        {
            get { return _attributeSet; }
            set { _attributeSet = value; }
        }
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _filter;

        public KeyValuePair<int, string> Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }
        [DataMember(IsRequired = true)]
        private string _lastProcessedDateTime;

        public string LastProcessedDateTime
        {
            get { return _lastProcessedDateTime; }
            set { _lastProcessedDateTime = value; }
        }
        [DataMember(IsRequired = true)]
        private string _lastProcessedUser;

        public string LastProcessedUser
        {
            get { return _lastProcessedUser; }
            set { _lastProcessedUser = value; }
        }


        public ROPlanningGlobalLockUnlockProperties(
            eMethodType methodType, 
            KeyValuePair<int, string> kvpMethod, 
            string sDescription, 
            int iUserKey,
            KeyValuePair<int, string> kvpMerchandise, 
            KeyValuePair<int, string> kvpVersion, 
            bool bIsMultilevel,
            ROLevelInformation FromLevel, 
            ROLevelInformation ToLevel, 
            KeyValuePair<int, string> kvpTimePeriod,
            ROOverrideLowLevel overrideLowLevel, 
            bool bStoreOptions, 
            bool bChainOptions,
            KeyValuePair<int, string> kvpStoreAttribute, 
            ArrayList attributeSet, 
            KeyValuePair<int, string> kvpFilter,
            string sLastProcessedDateTime, 
            string sLastProcessedUser,
            bool isTemplate = false
            ) 
            : base(
                  methodType, 
                  kvpMethod, 
                  sDescription, 
                  iUserKey,
                  isTemplate
                  )
        {
            _merchandise = kvpMerchandise;
            _version = kvpVersion;
            _isMuliLevel = bIsMultilevel;
            _fromLevel = FromLevel;
            _toLevel = ToLevel;
            _timePeriod = kvpTimePeriod;
            _overrideLowLevel = overrideLowLevel;
            _isStoresOptions = bStoreOptions;
            _isChainOptions = bChainOptions;
            _storeAttribute = kvpStoreAttribute;
            _attributeSet = attributeSet;
            _filter = kvpFilter;
            _lastProcessedDateTime = sLastProcessedDateTime;
            _lastProcessedUser = sLastProcessedUser;
        }
    }

    [DataContract(Name = "ROMethodRollupProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodRollupProperties : ROMethodProperties
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _merchandise;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _version;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _dateRange;
        [DataMember(IsRequired = true)]
        private List<ROMethodRollupOptionsBasis> _methodRollupBasisOptions;
        public KeyValuePair<int, string> Merchandise
        {
            get { return _merchandise; }
            set { _merchandise = value; }
        }

        public KeyValuePair<int, string> Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public KeyValuePair<int, string> DateRange
        {
            get { return _dateRange; }
            set { _dateRange = value; }
        }

        public List<ROMethodRollupOptionsBasis> MethodRollupBasisOptions
        {
            get { return _methodRollupBasisOptions; }
            set { _methodRollupBasisOptions = value; }
        }

        public ROMethodRollupProperties(
            KeyValuePair<int, string> method, 
            string description, 
            int userKey, 
            KeyValuePair<int, string> merchandise, 
            KeyValuePair<int, string> version,
            KeyValuePair<int, string> dateRange, 
            List<ROMethodRollupOptionsBasis> methodRollupBasisOptions,
            bool isTemplate = false
            ) 
            : base(
                  eMethodType.Rollup, 
                  method, 
                  description, 
                  userKey,
                  isTemplate
                  )
        {
            _merchandise = merchandise;
            _version = version;
            _dateRange = dateRange;
            _methodRollupBasisOptions = methodRollupBasisOptions;
        }
    }

    [DataContract(Name = "ROMethodRollupOptionsBasis", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodRollupOptionsBasis
    {
        [DataMember(IsRequired = true)]
        int _optionsDetailSeq;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _fromMerchandise;
        [DataMember(IsRequired = true)]
        ROLevelInformation _fromROLevelInformation;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _toMerchandise;
        [DataMember(IsRequired = true)]
        ROLevelInformation _toROLevelInformation;
        [DataMember(IsRequired = true)]
        bool _isStore;
        [DataMember(IsRequired = true)]
        bool _isChain;
        [DataMember(IsRequired = true)]
        bool _isStoreToChain;

        #region Public Properties
        public int OptionsDetailSeq
        {
            get { return _optionsDetailSeq; }
            set { _optionsDetailSeq = value; }
        }

        public KeyValuePair<int, string> FromMerchandise
        {
            get { return _fromMerchandise; }
            set { _fromMerchandise = value; }
        }

        public ROLevelInformation FromROLevelInformation
        {
            get { return _fromROLevelInformation; }
            set { _fromROLevelInformation = value; }
        }

        public KeyValuePair<int, string> ToMerchandise
        {
            get { return _toMerchandise; }
            set { _toMerchandise = value; }
        }

        public ROLevelInformation ToROLevelInformation
        {
            get { return _toROLevelInformation; }
            set { _toROLevelInformation = value; }
        }

        public bool IsStore
        {
            get { return _isStore; }
            set { _isStore = value; }
        }

        public bool IsChain
        {
            get { return _isChain; }
            set { _isChain = value; }
        }

        public bool IsStoreToChain
        {
            get { return _isStoreToChain; }
            set { _isStoreToChain = value; }
        }
        #endregion

        public ROMethodRollupOptionsBasis(int optionsDetailSeq, KeyValuePair<int, string> fromMerchandise, ROLevelInformation fromROLevelInformation, KeyValuePair<int, string> toMerchandise, ROLevelInformation toROLevelInformation,
                                          bool isStore, bool isChain, bool isStoreToChain)
        {

            _optionsDetailSeq = optionsDetailSeq;
            _fromMerchandise = fromMerchandise;
            _fromROLevelInformation = fromROLevelInformation;
            _toMerchandise = toMerchandise;
            _toROLevelInformation = toROLevelInformation;
            _isStore = isStore;
            _isChain = isChain;
            _isStoreToChain = isStoreToChain;
        }
    }

    [DataContract(Name = "ROMethodCopyForecastProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodCopyForecastProperties : ROMethodProperties
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _merchandise;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _version;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _timePeriod;
        [DataMember(IsRequired = true)]
        ePlanType _planType;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _storeFilter;
        [DataMember(IsRequired = true)]
        bool _multiLevel;
        [DataMember(IsRequired = true)]
        private ROLevelInformation _fromLevel;
        [DataMember(IsRequired = true)]
        private ROLevelInformation _toLevel;
        [DataMember(IsRequired = true)]
        private ROOverrideLowLevel _overrideLowLevel;
        bool _copyPreInitValues;
        [DataMember(IsRequired = true)]
        private List<ROBasisDetailProfile> _basisProfiles;


        #region Public Properties
        public KeyValuePair<int, string> Merchandise
        {
            get { return _merchandise; }
            set { _merchandise = value; }
        }
        public KeyValuePair<int, string> Version
        {
            get { return _version; }
            set { _version = value; }
        }
        public KeyValuePair<int, string> TimePeriod
        {
            get { return _timePeriod; }
            set { _timePeriod = value; }
        }
        public bool MultiLevel
        {
            get { return _multiLevel; }
            set { _multiLevel = value; }
        }
        public ePlanType PlanType
        {
            get { return _planType; }
            set { _planType = value; }
        }
        public KeyValuePair<int, string> StoreFilter
        {
            get { return _storeFilter; }
            set { _storeFilter = value; }
        }
        public ROLevelInformation FromLevel
        {
            get { return _fromLevel; }
            set { _fromLevel = value; }
        }
        public ROLevelInformation ToLevel
        {
            get { return _toLevel; }
            set { _toLevel = value; }
        }
        public ROOverrideLowLevel OverrideLowLevel
        {
            get { return _overrideLowLevel; }
            set { _overrideLowLevel = value; }
        }
        public bool CopyPreInitValues
        {
            get { return _copyPreInitValues; }
            set { _copyPreInitValues = value; }
        }
        public List<ROBasisDetailProfile> BasisProfiles
        {
            get { return _basisProfiles; }
            set { _basisProfiles = value; }
        }

        #endregion
        public ROMethodCopyForecastProperties(
            KeyValuePair<int, string> method, 
            string description, 
            eMethodType emethodType, 
            int userKey, 
            KeyValuePair<int, string> merchandise, 
            KeyValuePair<int, string> version,
            KeyValuePair<int, string> timePeriod, 
            bool multiLevel, 
            ePlanType planType, 
            KeyValuePair<int, string> storeFilter, 
            ROLevelInformation fromLevel, 
            ROLevelInformation toLevel,
            ROOverrideLowLevel overrideLowLevel, 
            bool copyPreInitValues, 
            List<ROBasisDetailProfile> basisProfile,
            bool isTemplate = false
            ) 
            : base(
                  emethodType,
                  method, 
                  description, 
                  userKey,
                  isTemplate
                  )

        {
            // fields specific to Copy Forecast method
            _merchandise = merchandise;
            _version = version;
            _timePeriod = timePeriod;
            _multiLevel = multiLevel;
            _planType = planType;
            _storeFilter = storeFilter;
            _fromLevel = fromLevel;
            _toLevel = toLevel;
            _overrideLowLevel = overrideLowLevel;
            _copyPreInitValues = copyPreInitValues;
            _basisProfiles = basisProfile;
        }
    }
    [DataContract(Name = "ROMethodMaxtrixBalanceProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodMatrixBalanceProperties : ROMethodProperties
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _filter;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _highLevelMerchandise;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _highLevelVersion;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _dateRange;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _lowLevelVersion;
        [DataMember(IsRequired = true)]
        private ROLevelInformation _lowLevel;
        [DataMember(IsRequired = true)]
        private bool _ineligibleStores;
        [DataMember(IsRequired = true)]
        private bool _similarStores;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _variable;
        [DataMember(IsRequired = true)]
        private eIterationType _iterationType;
        [DataMember(IsRequired = true)]
        private int _iterationsCount;
        [DataMember(IsRequired = true)]
        private eBalanceMode _balanceMode;
        [DataMember(IsRequired = true)]
        private string _computationMode;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _overrideLowLevel;
        [DataMember(IsRequired = true)]
        private eMatrixType _matrixType;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _model;
        [DataMember(IsRequired = true)]
        private List<ROBasisDetailProfile> _matrixBasis;

        public KeyValuePair<int, string> Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

        public KeyValuePair<int, string> HighLevelMerchandise
        {
            get { return _highLevelMerchandise; }
            set { _highLevelMerchandise = value; }
        }

        public KeyValuePair<int, string> HighLevelVersion
        {
            get { return _highLevelVersion; }
            set { _highLevelVersion = value; }
        }

        public KeyValuePair<int, string> DateRange
        {
            get { return _dateRange; }
            set { _dateRange = value; }
        }

        public KeyValuePair<int, string> LowLevelVersion
        {
            get { return _lowLevelVersion; }
            set { _lowLevelVersion = value; }
        }

        public ROLevelInformation LowLevel
        {
            get { return _lowLevel; }
            set { _lowLevel = value; }
        }

        public bool IneligibleStores
        {
            get { return _ineligibleStores; }
            set { _ineligibleStores = value; }
        }

        public bool SimilarStores
        {
            get { return _similarStores; }
            set { _similarStores = value; }
        }

        public KeyValuePair<int, string> Variable
        {
            get { return _variable; }
            set { _variable = value; }
        }

        public eIterationType IterationType
        {
            get { return _iterationType; }
            set { _iterationType = value; }
        }

        public int IterationsCount
        {
            get { return _iterationsCount; }
            set { _iterationsCount = value; }
        }

        public eBalanceMode BalanceMode
        {
            get { return _balanceMode; }
            set { _balanceMode = value; }
        }

        public string ComputationMode
        {
            get { return _computationMode; }
            set { _computationMode = value; }
        }

        public KeyValuePair<int, string> OverrideLowLevel
        {
            get { return _overrideLowLevel; }
            set { _overrideLowLevel = value; }
        }

        public eMatrixType MatrixType
        {
            get { return _matrixType; }
            set { _matrixType = value; }
        }

        public KeyValuePair<int, string> Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public List<ROBasisDetailProfile> MatrixBasis
        {
            get { return _matrixBasis; }
            set { _matrixBasis = value; }
        }

        public ROMethodMatrixBalanceProperties(
            KeyValuePair<int, string> method, 
            string description, 
            int userKey, 
            KeyValuePair<int, string> filter,
            KeyValuePair<int, string> highLevelMerchandise, 
            KeyValuePair<int, string> highLevelVersion, 
            KeyValuePair<int, string> dateRange,
            KeyValuePair<int, string> lowLevelVersion, 
            ROLevelInformation lowLevel, 
            bool ineligibleStores,
            bool similarStores, 
            KeyValuePair<int, string> variable,
            eIterationType iterationType, 
            int iterationsCount, 
            eBalanceMode balanceMode, 
            string computationMode, 
            KeyValuePair<int, string> overrideLowLevel,
            eMatrixType matrixType, 
            KeyValuePair<int, string> model, 
            List<ROBasisDetailProfile> matrixBasis,
            bool isTemplate = false
            ) 
            : base(
                  eMethodType.Rollup, 
                  method, 
                  description,
                  userKey,
                  isTemplate
                  )
        {
            _filter = filter;
            _highLevelMerchandise = highLevelMerchandise;
            _highLevelVersion = highLevelVersion;
            _dateRange = dateRange;
            _lowLevelVersion = lowLevelVersion;
            _lowLevel = lowLevel;
            _ineligibleStores = ineligibleStores;
            _similarStores = similarStores;
            _variable = variable;
            _iterationType = iterationType;
            _iterationsCount = iterationsCount;
            _balanceMode = balanceMode;
            _computationMode = computationMode;
            _overrideLowLevel = overrideLowLevel;
            _matrixType = matrixType;
            _model = model;
            _matrixBasis = matrixBasis;
        }
    }
    #region "Data transport for Forecast Spread Method #RO-742"
    [DataContract(Name = "ROMethodForecastSpreadProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodForecastSpreadProperties : ROMethodProperties
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _merchandise;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _version;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _timePeriod;
        [DataMember(IsRequired = true)]
        bool _multiLevel;
        [DataMember(IsRequired = true)]
        private ROLevelInformation _fromLevel;
        [DataMember(IsRequired = true)]
        private ROLevelInformation _toLevel;
        [DataMember(IsRequired = true)]
        private ROOverrideLowLevel _overrideLowLevel;
        [DataMember(IsRequired = true)]
        eSpreadOption _spreadOption;
        [DataMember(IsRequired = true)]
        bool _ignoreLocks;
        [DataMember(IsRequired = true)]
        bool _equalizeWeighting;
        [DataMember(IsRequired = true)]
        private List<ROBasisDetailProfile> _basisProfiles;


        #region Public Properties
        public KeyValuePair<int, string> Merchandise
        {
            get { return _merchandise; }
            set { _merchandise = value; }
        }
        public KeyValuePair<int, string> Version
        {
            get { return _version; }
            set { _version = value; }
        }
        public KeyValuePair<int, string> TimePeriod
        {
            get { return _timePeriod; }
            set { _timePeriod = value; }
        }
        public bool MultiLevel
        {
            get { return _multiLevel; }
            set { _multiLevel = value; }
        }

        public ROLevelInformation FromLevel
        {
            get { return _fromLevel; }
            set { _fromLevel = value; }
        }
        public ROLevelInformation ToLevel
        {
            get { return _toLevel; }
            set { _toLevel = value; }
        }
        public ROOverrideLowLevel OverrideLowLevel
        {
            get { return _overrideLowLevel; }
            set { _overrideLowLevel = value; }
        }
        public eSpreadOption SpreadOption
        {
            get { return _spreadOption; }
            set { _spreadOption = value; }
        }
        public bool IgnoreLocks
        {
            get { return _ignoreLocks; }
            set { _ignoreLocks = value; }
        }
        public bool EqualizeWeighting
        {
            get { return _equalizeWeighting; }
            set { _equalizeWeighting = value; }
        }
        public List<ROBasisDetailProfile> BasisProfiles
        {
            get { return _basisProfiles; }
            set { _basisProfiles = value; }
        }

        #endregion
        public ROMethodForecastSpreadProperties(
            KeyValuePair<int, string> method, 
            string description, 
            eMethodType emethodType, 
            int userKey, 
            KeyValuePair<int, string> merchandise, 
            KeyValuePair<int, string> version,
            KeyValuePair<int, string> timePeriod, 
            bool multiLevel, 
            ROLevelInformation fromLevel, 
            ROLevelInformation toLevel, 
            ROOverrideLowLevel overrideLowLevel,
            eSpreadOption spreadOption, 
            bool ignoreLocks,
            bool equalizeWeighting, 
            List<ROBasisDetailProfile> basisProfile,
            bool isTemplate = false
            ) 
            : base(
                  emethodType, 
                  method, 
                  description, 
                  userKey,
                  isTemplate
                  )

        {
            // fields specific to Forecast Spread method
            _merchandise = merchandise;
            _version = version;
            _timePeriod = timePeriod;
            _multiLevel = multiLevel;
            _spreadOption = spreadOption;
            _ignoreLocks = ignoreLocks;
            _fromLevel = fromLevel;
            _toLevel = toLevel;
            _overrideLowLevel = overrideLowLevel;
            _equalizeWeighting = equalizeWeighting;
            _basisProfiles = basisProfile;
        }
    }

    #endregion


    #region RO-741 -Data transport for Modify Sales Method"

    [DataContract(Name = "ROPlanningModifySalesProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROPlanningModifySalesProperties : ROMethodProperties
    {
        // fields specific to Modify Sales method 

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _dateRange;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _filter;

        [DataMember(IsRequired = true)]
        eStoreAverageBy _averageBy;

        [DataMember(IsRequired = true)]
        List<ROStoreGradeList> _storeGradeList;

        [DataMember(IsRequired = true)]
        List<ROSellThruList> _sellThruList;

        [DataMember(IsRequired = true)]
        List<ROMatrixRuleList> _matrixRulesList;


        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _merchandise;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attribute;


        #region Public Properties

        public KeyValuePair<int, string> DateRange
        {
            get { return _dateRange; }
            set { _dateRange = value; }
        }
        public KeyValuePair<int, string> Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

        public eStoreAverageBy AverageBy
        {
            get { return _averageBy; }
            set { _averageBy = value; }
        }

        public List<ROStoreGradeList> StoreGradeList
        {
            get { return _storeGradeList; }
            set { _storeGradeList = value; }
        }

        public List<ROSellThruList> SellThruList
        {
            get { return _sellThruList; }
            set { _sellThruList = value; }
        }

        public List<ROMatrixRuleList> SalesMatrixList
        {
            get { return _matrixRulesList; }
            set { _matrixRulesList = value; }
        }

        public KeyValuePair<int, string> Merchandise
        {
            get { return _merchandise; }
            set { _merchandise = value; }
        }

        public KeyValuePair<int, string> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        #endregion
        public ROPlanningModifySalesProperties(
            KeyValuePair<int, string> method, 
            string description, 
            int userKey,
            KeyValuePair<int, string> dateRange, 
            KeyValuePair<int, string> filter, 
            eStoreAverageBy averageBy,
            List<ROStoreGradeList> storeGradesList, 
            List<ROSellThruList> sellThruList,
            List<ROMatrixRuleList> matrixRulesList, 
            KeyValuePair<int, string> merchandise,
            KeyValuePair<int, string> attribute,
            bool isTemplate = false
            ) 
            : base(
                  eMethodType.ForecastModifySales, 
                  method, 
                  description, 
                  userKey,
                  isTemplate
                  )

        {
            // fields specific to Modify Sales method

            _dateRange = dateRange;
            _filter = filter;
            _averageBy = averageBy;
            _storeGradeList = storeGradesList;
            _sellThruList = sellThruList;
            _matrixRulesList = matrixRulesList;
            _merchandise = merchandise;
            _attribute = attribute;

        }
    }

    [DataContract(Name = "ROStoreGradeList", Namespace = "http://Logility.ROWeb/")]
    public class ROStoreGradeList
    {

        [DataMember(IsRequired = true)]
        private int _boundary;
        [DataMember(IsRequired = true)]
        private string _gradeCode;

        public ROStoreGradeList(int boundary, string gradeCode)
        {
            _boundary = boundary;
            _gradeCode = gradeCode;
        }

        public int Boundary
        {
            get { return _boundary; }
            set { _boundary = value; }
        }

        public string Grade_Code
        {
            get { return _gradeCode; }
            set { _gradeCode = value; }
        }
    }

    [DataContract(Name = "ROSellThruList", Namespace = "http://Logility.ROWeb/")]
    public class ROSellThruList
    {

        [DataMember(IsRequired = true)]
        private int _sellThru;
        [DataMember(IsRequired = true)]
        private string _sellThruHeading;

        public ROSellThruList(int sellThru, string sellThruHeading = null)
        {
            _sellThru = sellThru;
            _sellThruHeading = sellThruHeading;

        }

        public int Sell_Thru
        {
            get { return _sellThru; }
            set { _sellThru = value; }
        }

        public string SellThruHeading
        {
            get { return _sellThruHeading; }
            set { _sellThruHeading = value; }
        }

        public bool SellThruHeadingIsSet
        {
            get { return _sellThruHeading != null; }
        }
    }

    [DataContract(Name = "ROMatrixRuleList", Namespace = "http://Logility.ROWeb/")]
    public class ROMatrixRuleList
    {

        [DataMember(IsRequired = true)]
        private int _boundary;
        [DataMember(IsRequired = true)]
        private int? _sellThru;
        [DataMember(IsRequired = true)]
        private string _gradeCode;
        [DataMember(IsRequired = true)]
        private KeyValuePair<eModifySalesRuleType, string> _rule;
        [DataMember(IsRequired = true)]
        private double _quantity;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _attributeSet;

        public ROMatrixRuleList(int boundary, int? sellThru, string gradeCode, KeyValuePair<eModifySalesRuleType, string> rule, double quantity, KeyValuePair<int, string> attributeSet)
        {
            _boundary = boundary;
            _sellThru = sellThru;
            _gradeCode = gradeCode;
            _rule = rule;
            _quantity = quantity;
            _attributeSet = attributeSet;
        }

        public int Boundary
        {
            get { return _boundary; }
            set { _boundary = value; }
        }
        public int? Sell_Thru
        {
            get { return _sellThru; }
            set { _sellThru = value; }
        }

        public string Grade_Code
        {
            get { return _gradeCode; }
            set { _gradeCode = value; }
        }

        public KeyValuePair<eModifySalesRuleType, string> Matrix_Rule
        {
            get { return _rule; }
            set { _rule = value; }
        }

        public double Matrix_Rule_Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
            set { _attributeSet = value; }
        }

    }

    #endregion

    [DataContract(Name = "ROPlanningStoreGrade", Namespace = "http://Logility.ROWeb/")]
    public class ROPlanningStoreGrade : ROStoreGrade
    {
        [DataMember(IsRequired = true)]
        public KeyValuePair<int, string> StoreGroupLevel { get; set; }
        [DataMember(IsRequired = true)]
        public KeyValuePair<int, string> Merchandise { get; set; }
        [DataMember(IsRequired = true)]
        public string Picture { get; set; }
        [DataMember(IsRequired = true)]
        public KeyValuePair<int, string> DateRange { get; set; }
        [DataMember(IsRequired = true)]
        public List<ROStockMinMax> ROStockMinMaxList { get; set; }
    }

    [DataContract(Name = "ROMethodPlanningExtractProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodPlanningExtractProperties : ROMethodProperties
    {
        // fields specific Planning Extract method
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _merchandise;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _versionFilter;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _timePeriod_CDR;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _storeFilter;
        [DataMember(IsRequired = true)]
        bool _chainIndicator;
        [DataMember(IsRequired = true)]
        bool _storeIndicator;
        [DataMember(IsRequired = true)]
        bool _attributeSetIndicator;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attribute;
        [DataMember(IsRequired = true)]
        bool _lowLevelsIndicator;
        [DataMember(IsRequired = true)]
        bool _lowLevelsOnlyIndicator;
        [DataMember(IsRequired = true)]
        private ROLevelInformation _lowLevel;
        [DataMember(IsRequired = true)]
        bool _extractIneligibleStoresIndicator;
        [DataMember(IsRequired = true)]
        bool _excludeZeroValuesIndicator;
        [DataMember(IsRequired = true)]
        int _numberOfConcurrentProcesses;
        [DataMember(IsRequired = true)]
        private ROOverrideLowLevel _overrideLowLevel;
        [DataMember(IsRequired = true)]
        private ROVariableGroupings _variableList;
        [DataMember(IsRequired = true)]
        private ROVariableGroupings _totalVariableList;

        public KeyValuePair<int, string> Merchandise
        {
            get { return _merchandise; }
            set { _merchandise = value; }
        }

        public KeyValuePair<int, string> VersionFilter
        {
            get { return _versionFilter; }
            set { _versionFilter = value; }
        }

        public KeyValuePair<int, string> TimePeriod_CDR
        {
            get { return _timePeriod_CDR; }
            set { _timePeriod_CDR = value; }
        }

        public KeyValuePair<int, string> StoreFilter
        {
            get { return _storeFilter; }
            set { _storeFilter = value; }
        }

        public bool ChainIndicator
        {
            get { return _chainIndicator; }
            set { _chainIndicator = value; }
        }

        public bool StoreIndicator
        {
            get { return _storeIndicator; }
            set { _storeIndicator = value; }
        }

        public bool AttributeSetIndicator
        {
            get { return _attributeSetIndicator; }
            set { _attributeSetIndicator = value; }
        }

        public KeyValuePair<int, string> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        public bool LowLevelsIndicator
        {
            get { return _lowLevelsIndicator; }
            set { _lowLevelsIndicator = value; }
        }

        public bool LowLevelsOnlyIndicator
        {
            get { return _lowLevelsOnlyIndicator; }
            set { _lowLevelsOnlyIndicator = value; }
        }

        public ROLevelInformation LowLevel
        {
            get { return _lowLevel; }
            set { _lowLevel = value; }
        }

        public bool ExtractIneligibleStoresIndicator
        {
            get { return _extractIneligibleStoresIndicator; }
            set { _extractIneligibleStoresIndicator = value; }
        }

        public bool ExcludeZeroValuesIndicator
        {
            get { return _excludeZeroValuesIndicator; }
            set { _excludeZeroValuesIndicator = value; }
        }

        public int NumberOfConcurrentProcesses
        {
            get { return _numberOfConcurrentProcesses; }
            set { _numberOfConcurrentProcesses = value; }
        }

        public ROOverrideLowLevel OverrideLowLevel
        {
            get { return _overrideLowLevel; }
            set { _overrideLowLevel = value; }
        }

        public ROVariableGroupings VariableList
        {
            get { return _variableList; }
            set { _variableList = value; }
        }

        public ROVariableGroupings TotalVariableList
        {
            get { return _totalVariableList; }
            set { _totalVariableList = value; }
        }

        public ROMethodPlanningExtractProperties(
            KeyValuePair<int, string> kvpMethod, 
            string sDescription, 
            int iUserKey, 
            KeyValuePair<int, string> merchandise, 
            KeyValuePair<int, string> versionFilter,
            KeyValuePair<int, string> timePeriod_CDR, 
            KeyValuePair<int, string> storeFilter, 
            bool chainIndicator, 
            bool storeIndicator,
            bool attributeSetIndicator, 
            KeyValuePair<int, string> attribute,
            bool lowLevelsIndicator, 
            bool lowLevelsOnlyIndicator, 
            ROLevelInformation lowLevel, 
            bool extractIneligibleStoresIndicator,
            bool excludeZeroValuesIndicator, 
            int numberOfConcurrentProcesses, 
            ROOverrideLowLevel overrideLowLevel, 
            ROVariableGroupings variableList, 
            ROVariableGroupings totalVariableList,
            bool isTemplate = false
            ) 
            : base(
                  eMethodType.PlanningExtract, 
                  kvpMethod, 
                  sDescription, 
                  iUserKey,
                  isTemplate
                  )
        {
            _merchandise = merchandise;
            _versionFilter = versionFilter;
            _timePeriod_CDR = timePeriod_CDR;
            _storeFilter = storeFilter;
            _chainIndicator = chainIndicator;
            _storeIndicator = storeIndicator;
            _attributeSetIndicator = attributeSetIndicator;
            _attribute = attribute;
            _lowLevelsIndicator = lowLevelsIndicator;
            _lowLevelsOnlyIndicator = lowLevelsOnlyIndicator;
            _lowLevel = lowLevel;
            _extractIneligibleStoresIndicator = extractIneligibleStoresIndicator;
            _excludeZeroValuesIndicator = excludeZeroValuesIndicator;
            _numberOfConcurrentProcesses = numberOfConcurrentProcesses;
            _overrideLowLevel = overrideLowLevel;
            _variableList = variableList;
            _totalVariableList = totalVariableList;
        }
    }

    [DataContract(Name = "ROPlanningViewDetails", Namespace = "http://Logility.ROWeb/")]
    public class ROPlanningViewDetails : ROViewDetails
    {
        [DataMember(IsRequired = true)]
        private ROVariableGroupings _variableGroupings;

        [DataMember(IsRequired = true)]
        private List<ROSelectedField> _comparatives;

        [DataMember(IsRequired = true)]
        private List<ROSelectedField> _timePeriods;

        public ROPlanningViewDetails(KeyValuePair<int, string> view, ROVariableGroupings ROVariableGroupings) :
            base(view)
        {
            _variableGroupings = ROVariableGroupings;
            _comparatives = new List<ROSelectedField>();
            _timePeriods = new List<ROSelectedField>();
        }

        public ROVariableGroupings VariableGroupings { get { return _variableGroupings; } }
        public List<ROSelectedField> Comparatives { get { return _comparatives; } }
        public List<ROSelectedField> TimePeriods { get { return _timePeriods; } }

    }
}
