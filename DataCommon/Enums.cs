using System;

namespace MIDRetail.DataCommon
{
    // Begin TT#2307 - JSmith - Incorrect Stock Values
    ///// <summary>
    ///// Do not reorder and change the values of the enumerations.  Results will be affected
    ///// </summary>

    //public enum eMIDMessageCode
    //{
    //    systemError
    //}
    public enum eMIDMessageCode
    {
        StoreOpenDateUpdated = 1,
    }

    public enum eMIDMessageSenderRecepient
    {
        unknown = 0,
        database = 1,
        hierarchyLoad = 2,
        storeLoad = 3,
        historyPlanLoad = 4,
        clientApplication = 5,
        storeGroupBuilder = 6,
        hierarchyWebService = 7,
        colorCodeLoad = 8,
        sizeCodeLoad = 9,
        headerLoad = 10,
        forecasting = 11,
        rollup = 12,
        controlService = 13,
        applicationService = 14,
        hierarchyService = 15,
        storeService = 16,
        relieveIntransit = 17,
        purge = 18,
        allocate = 19,
        schedulerService = 20,
        executeJob = 21,
        headerService = 22,
        sizeCurveLoad = 23,
        sqlScript = 24,
        computationsLoad = 25,
        forecastBalancing = 26,
        databaseConversionUtility = 27,
        reBuildIntransit = 28,
        sizeConstraintsLoad = 29,
        computationDriver = 30,
        specialRequest = 31,
        forecastExportThread = 32,
        sizeCurveGenerate = 33,
        SizeDayToWeekSummary = 34,
        buildPackCriteriaLoad = 35,
        StoreBinViewer = 36,
        generateRelieveIntransit = 37,
        sizeCurveGenerateThread = 38,
        determineHierarchyActivity = 39,
        ChainSetPercentCriteriaLoad = 40,
        hierarchyReclass = 41,
        pushToBackStock = 42,
        headerAllocationLoad = 43,
    }
    // End TT#2307

    public enum eMDIChildCloseAction
    {
        OK,
        Cancel
    }

    // Do not change the order of the values in this enumeration.  They are used on the database.
    public enum eViewAxis
    {
        //Begin Track #5121 - JScott - Add Year/Season/Quarter totals
        //Row				= 0,
        //Column			= 1,
        //Page			= 2
        Quantity = 0,
        Variable = 1,
        Period = 2
        //End Track #5121 - JScott - Add Year/Season/Quarter totals
    }

    // Do not change the order of the values in this enumeration.  They are used on the database.
    public enum eAssortmentViewAxis
    {
        Component = 0,
        SummaryColumn = 1,
        TotalColumn = 2,
        DetailColumn = 3,
        DetailRow = 4
    }

    public enum eScrollType
    {
        None,
        Line,
        Group
    }

    public enum eErrorLevel
    {
        information,
        warning,
        severe,
        //Begin TT#1020 - JScott - Add new Error level to the eErrorLevel enumerator
        //fatal
        fatal,
        error
        //End TT#1020 - JScott - Add new Error level to the eErrorLevel enumerator
    }

    public enum eReturnCode
    {
        successful = 0,
        inputFileNotFound = 1,
        editErrors = 2,
        warning = 3,
        severe = 4,
        fatal = 5
    }

    public enum eLockAction
    {
        AquireReadLock,
        AquireWriteLock,
        UpgradeWriteLock
    }

    // BEGIN TT#1766 - stodd - fifo file processing
    public enum eAPIFileProcessingDirection
    {
        // Begin TT#645-MD - JSmith - Add File Processing Direction Parameter to Tasks on Task Lists that Process Input Files
        //Default,
        //FIFO,
        //FILO
        Default = 802870,
        FIFO = 802871,
        FILO = 802872,
        Config = 802873
        // End TT#645-MD - JSmith - Add File Processing Direction Parameter to Tasks on Task Lists that Process Input Files
    }
    // END TT#1766 - stodd - fifo file processing

    public enum eExceptionLogging
    {
        logAllInnerExceptions,
        logOnlyInnerMostException
    }

    public enum ePlanViewSaveAsOption
    {
        Store,
        Chain,
        View,
        Theme
    }

    //Begin Track #5690 - JScott - Can not save low to high
    public enum ePlanViewSaveResult
    {
        Cancel,
        //Begin Track #5950 - JScott - Save Low Level to High may get warning message
        SaveViewOnly,
        //End Track #5950 - JScott - Save Low Level to High may get warning message
        Save
    }

    //End Track #5690 - JScott - Can not save low to high
    public enum eFilterDateType
    {
        none = 801690,
        today = 801691,
        between = 801692,
        specify = 801693,
        all = 801694
    }

    public enum eChangeType
    {
        none = 0,
        add = 1,
        update = 2,
        delete = 3,
        populate = 4,
        markedForDelete = 5     // TT#739-MD - STodd - delete stores
    }

    public enum eUpdateMode
    {
        Create,
        Update
    }

    public enum eGetSizes
    {
        SizeGroupRID,
        SizeCurveGroupRID
    }

    public enum eGetDimensions
    {
        SizeGroupRID,
        SizeCurveGroupRID
    }

    // rollup stored procedures must be changed if the values for this enum are changed
    // Values must match eRollType
    public enum eVariableDataType
    {
        none = 0,
        storeDailyHistory = 200,
        storeWeeklyHistory = 300,
        storeWeeklyForecast = 400,
        chainDailyHistory = 600,
        chainWeeklyHistory = 700,
        chainWeeklyForecast = 800,
        // Begin MID Track #4730 - JSmith - roll size intransit to dummy color
        //		storeIntransit			= 900
        //Begin TT#795 - JScott - STORE_INTRANSIT_REV table is not being purged
        //storeExternalIntransit = 900
        storeExternalIntransit = 900,
        storeIntransitReview = 1200,
        //End TT#795 - JScott - STORE_INTRANSIT_REV table is not being purged
        // End MID Track #4730
        storeIMOReview = 1300,  // TT#4352 - JSmith - VSW Review records not getting purged
    }

    // The base values must match eVariableDataType for rollup to work
    public enum eRollType
    {
        none = 0,
        storeDailyHistoryToWeeks = 100,
        storeDailyHistory = 200,
        storeWeeklyHistory = 300,
        storeWeeklyForecast = 400,
        storeToChain = 500,
        chainDailyHistory = 600,
        chainWeeklyHistory = 700,
        chainWeeklyForecast = 800,
        storeExternalIntransit = 900,
        // Begin MID Track #4730 - JSmith - roll size intransit to dummy color
        //		storeIntransit				= 1000
        storeIntransit = 1000,
        dummyColor = 1100
        // End MID Track #4730
    }

    public enum eLevelRollType
    {
        None,
        Sum,
        Average
    }

    public enum eDayToWeekRollType
    {
        None,
        Sum,
        Average,
        First,
        Last
    }

    public enum eStoreToChainRollType
    {
        None,
        Sum,
        Average
    }

    //	public enum eStockType
    //	{
    //		Beginning,
    //		Ending
    //	}

    // For General Assortment
    public enum eStoreAverageBy
    {
        None,
        AllStores,
        Set
    }

    public enum eGradeBoundary
    {
        Unknown,
        Index,
        Units
    }

    public enum eReserveType
    {
        Unknown,
        Percent,
        Units
    }
    // End for General Assorment

    // begin TT#2 - stodd - assortment 
    public enum eLastHeaderSelection
    {
        None,
        FromAllocationExplorer,
        FromAssortmentExplorer
    }
    // end TT#2 - stodd - assortment 

    // STORE_CHAR_GROUP, SCG_TYPE enum
    public enum eStoreCharType
    {
        text = 0,
        date = 1,
        number = 2,
        dollar = 3,
        unknown = 4,
        list = 5,
        boolean = 6
    }

    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //public enum eStoreNodeType
    //{
    //    group = 0,
    //    groupLevel = 1,
    //    store = 2,
    //    favorites = 3,
    //    // Begin Track #4872 - JSmith - Global/User Attributes
    //    //all = 4
    //    all = 4,
    //    user = 5
    //    // End Track #4872
    //}

    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    public enum eFavoritesType
    {
        Stores = 0,
        Workflows = 2,
        Methods = 3,
        TaskLists = 4,
    }

    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //public enum eFolderType
    //{
    //    None = -1,
    //    FilterFavorites = 1,
    //    FilterGlobal = 2,
    //    FilterUser = 3,
    //    // Begin Track #4872 - JSmith - Global/User Attributes
    //    StoreFavorites = 4,
    //    StoreGlobal = 5,
    //    StoreUser = 6,
    //    // End Track #4872
    //    //Begin Track #5005 - JSmith - Explorer Organization
    //    FilterShared = 7,
    //    MerchandiseFavorites = 8,
    //    MerchandiseUser = 9,
    //    MerchandiseOrganizational = 10,
    //    MerchandiseAlternates = 11,
    //    MerchandiseShared = 12,
    //    StoreShared = 13,
    //    WorflowMethodFavorites = 14,
    //    WorflowMethodUser = 15,
    //    WorflowMethodGlobal = 16,
    //    WorflowMethodShared = 17,
    //    TaskListFavorites = 18,
    //    TaskListShared = 19,
    //    //End Track #5005
    //}

    //public enum eFolderChildType
    //{
    //    None		= -1,
    //    Folder		= 1,
    //    Filter		= 2,
    //    //Begin Track #5005 - JSmith - Explorer Organization
    //    Hierarchy = 3,
    //    HierarchyNode = 4,
    //    //End Track #5005
    //}

    //public enum eFilterNodeType
    //{
    //    None,
    //    FavoriteFolder,
    //    GlobalFolder,
    //    UserFolder,
    //    Filter,
    //    FilterShortcut,
    //    ParentShortcut,
    //    ChildShortcut,
    //}
    public enum eTreeNodeType
    {
        None,
        //MainFolderNode,
        MainFavoriteFolderNode,
        MainSourceFolderNode,
        MainNonSourceFolderNode,
        SubFolderNode,
        ObjectNode,
        //RootShortcutNode,
        FolderShortcutNode,
        //ChildShortcutNode,
        ChildFolderShortcutNode,
        ChildObjectShortcutNode,
        ObjectShortcutNode
    }

    public enum eCutCopyOperation
    {
        None,
        Cut,
        Copy
    }

    public enum eExplorerActionMenuItem
    {
        None,
        New,
        NewFolder,
        NewItem,
        Cut,
        Copy,
        Paste,
        Delete,
        Rename,
        Process,
        HierarchyProperties,
        NodeProperties,
        Open,
        InUse,  //TT#110-MD-VStuart - In Use Tool
        Search,
        Schedule,
        Refresh,
        EditSeparator,
        ProcessSeparator,
        RefreshSeparator,
    }
    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

    public enum eDragStates
    {
        Idle,
        Move,
        Copy,
        Link
    }

    // Begin TT# - stodd assortment
    public enum eIndexToAverageReturnType
    {
        Total,
        SetTotal,
        Grades
    }
    // End TT# - stodd assortment

    // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
    public enum eDropStates
    {
        Allowed,
        Duplicate,
    }
    // End TT#2186

    /// <summary>
    /// Used to show the four main style groups.
    /// </summary>
    public enum StyleEnum
    {
        Chiseled,
        AlterColors,
        HighlightName,
        Plain
    }

    //Must be same int value as eStoreCharType (because I said so - Scott)
    public enum eStoreCharColumns
    {
        TEXT_VALUE = 0,
        DATE_VALUE = 1,
        NUMBER_VALUE = 2,
        DOLLAR_VALUE = 3
    }

    //Identifies what type of data is the size method constraint tables.
    public enum eSizeMethodRowType
    {
        AllSize = 0,
        Set = 1,
        AllColor = 2,
        Color = 3,
        AllColorSize = 4,
        ColorSize = 5,
        AllColorSizeDimension = 6,
        ColorSizeDimension = 7,
        Default = 8                       // MID Track 3619 Remove Fringe
                                          //SetFringeFilter = 9,            // MID Track 3619 Remove Fringe
                                          //AllColorFringeFilter = 10,      // MID Track 3619 Remove Fringe
                                          //ColorFringeFilter = 11          // MID Track 3619 Remove Fringe
    }

    // Designates the collection type used in the size collections 
    public enum eSizeCollectionType
    {
        MinMaxCollection,
        FringeCollection,
        RulesCollection,
        SizeOOSLookupCollection
    }


    // Used in the SizeNeedAlgorithm.
    // specifies whether the plan for a size should be constructed for 
    // the specific color being planned or from across all colors.
    public enum eSizeNeedColorPlan
    {
        PlanForSpecificColorOnly,
        PlanAcrossAllColors
    }

    // Begin TT#1581-MD - stodd - Header Reconcile API
    // Used in Header Reconcile/Header Load to note which key type is selected.
    public enum eHeaderMatchingKeyType
    {
        HeaderField,
        Color,
        Pack,
        Characteristic,
        Text,   // TT#1966-MD - JSmith- DC Fulfillment
        Sequence
    }
    // End TT#1581-MD - stodd - Header Reconcile API

    //Maps to actual Stores Table Column Names - soft labels have same enum
    //values in eMIDTextCode - need actual names for Store Group Level Builder
    //to create dynamic SQL Statements and show "Plain English" statements.
    public enum eStoreTableColumns
    {
        ST_ID = 900000,
        STORE_NAME = 900001,
        ACTIVE_IND = 900002,
        CITY = 900003,
        STATE = 900004,
        SELLING_SQ_FT = 900005,
        SELLING_OPEN_DATE = 900006,
        SELLING_CLOSE_DATE = 900007,
        STOCK_OPEN_DATE = 900008,
        STOCK_CLOSE_DATE = 900009,
        LEAD_TIME = 900010,
        SHIP_ON_MONDAY = 900011,
        SHIP_ON_TUESDAY = 900012,
        SHIP_ON_WEDNESDAY = 900013,
        SHIP_ON_THURSDAY = 900014,
        SHIP_ON_FRIDAY = 900015,
        SHIP_ON_SATURDAY = 900016,
        SHIP_ON_SUNDAY = 900017,
        STORE_DESC = 900024,
        // BEGIN TT#1401 - stodd - add resevation stores (IMO)
        SIMILAR_STORE_MODEL = 900352,
        IMO_ID = 900805,
        // END TT#1401 - stodd - add resevation stores (IMO)
        //BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement
        STOCK_LEAD_WEEKS = 900820,
        //END TT#44 - MD - DOConnell - New Store Forecasting Enhancement
        STORE_DELETE_IND = 900868       // TT#739-MD - STodd - delete stores
    }

    //Maps to lbl messages below - used in StoreGroupBuilder.cs
    public enum eENGSQLOperator
    {
        False = 900124,
        True = 900125,
        Equals = 900126,
        GThan = 900127,
        LThan = 900128,
        Between = 900129,
        NotEqual = 900130,
        Like = 900131,
        In = 900132,
        NotIn = 900133,
        And = 900134
    }

    public enum eAssortmentWindowType
    {
        AllocationSummary,
        Assortment,
        GroupAllocation     // TT#952 - add matrix to Group Allocation
    }

    public enum eDbType
    {
        Bit,
        Char,
        DateTime,
        Float,
        Int,
        SmallDateTime,
        VarChar,
        Decimal,
        Single,
        Image,
        Blob,
        Text,   // TT#173  Provide database container for large data collections
        smallint, // TT#173  Provide database container for large data collections
        tinyint,   // TT#173  Provide database container for large data collections
        VarBinary,  // TT#173  Provide database container for large data collections // TT#1185 - Verify ENQ before Update
        Int64,       // TT#1185 - Verify ENQ before update
        Structured //TT#827-MD -jsobek -Allocation Reviews Performance
    }

    public enum eParameterDirection
    {
        Input,
        InputOutput,
        Output
    }

    public enum ePlanDisplayType
    {
        Week,
        Period
    }

    public enum eUnitScaling
    {
        Ones = 801500,
        //Begin Modification - JScott - Add Scaling Decimals
        //UserScaling1					= 801501,
        //UserScaling2					= 801502,
        //UserScaling3					= 801503,
        //UserScaling4					= 801504,
        //UserScaling5					= 801505,
        //UserScaling6					= 801506,
        //UserScaling7					= 801507,
        //UserScaling8					= 801508,
        //UserScaling9					= 801509,
        ReservedForUserScaling1 = 801501,
        ReservedForUserScaling2 = 801502,
        ReservedForUserScaling3 = 801503,
        ReservedForUserScaling4 = 801504,
        ReservedForUserScaling5 = 801505,
        ReservedForUserScaling6 = 801506,
        ReservedForUserScaling7 = 801507,
        ReservedForUserScaling8 = 801508,
        ReservedForUserScaling9 = 801509,
        ReservedForUserScaling10 = 801510,
        ReservedForUserScaling11 = 801511,
        ReservedForUserScaling12 = 801512,
        ReservedForUserScaling13 = 801513,
        ReservedForUserScaling14 = 801514,
        ReservedForUserScaling15 = 801515,
        ReservedForUserScaling16 = 801516,
        ReservedForUserScaling17 = 801517,
        ReservedForUserScaling18 = 801518,
        ReservedForUserScaling19 = 801519,
        //End Modification - JScott - Add Scaling Decimals
    }

    public enum eDollarScaling
    {
        //Begin Modification - JScott - Add Scaling Decimals
        //Ones							= 801510,
        //UserScaling1					= 801511,
        //UserScaling2					= 801512,
        //UserScaling3					= 801513,
        //UserScaling4					= 801514,
        //UserScaling5					= 801515,
        //UserScaling6					= 801516,
        //UserScaling7					= 801517,
        //UserScaling8					= 801518,
        //UserScaling9					= 801519,
        Ones = 801520,
        ReservedForUserScaling1 = 801521,
        ReservedForUserScaling2 = 801522,
        ReservedForUserScaling3 = 801523,
        ReservedForUserScaling4 = 801524,
        ReservedForUserScaling5 = 801525,
        ReservedForUserScaling6 = 801526,
        ReservedForUserScaling7 = 801527,
        ReservedForUserScaling8 = 801528,
        ReservedForUserScaling9 = 801529,
        ReservedForUserScaling10 = 801530,
        ReservedForUserScaling11 = 801531,
        ReservedForUserScaling12 = 801532,
        ReservedForUserScaling13 = 801533,
        ReservedForUserScaling14 = 801534,
        ReservedForUserScaling15 = 801535,
        ReservedForUserScaling16 = 801536,
        ReservedForUserScaling17 = 801537,
        ReservedForUserScaling18 = 801538,
        ReservedForUserScaling19 = 801539,
        //End Modification - JScott - Add Scaling Decimals
    }


    public enum eVariableCategory
    {
        None,
        Store,
        Chain,
        Both
    }

    public enum eVariableType
    {
        None,
        Sales,
        BegStock,
        EndStock,
        Intransit,
        FWOS,
        ChainSetPercent,
        Other
    }

    // Begin TT#2131-MD - JSmith - Halo Integration
    // Do not change the order of the values in this enumeration.  They are used on the database.
    public enum eVariableTimeType
    {
        Weekly = 0,
        TimeTotal = 1
    }
    // End TT#2131-MD - JSmith - Halo Integration

    // BEGIN Issue 4827 stodd 10.23.2007
    public enum eClientCustomVariableType
    {
        None,
        PresentationMin
    }
    // END Issue 4827 

    public enum eVariableScope
    {
        None,
        Overridden,
        Static,
        Dynamic
    }

    public enum eVariableSpreadType
    {
        None,
        PctChange,
        PctContribution,
        Plug
    }

    public enum eVariableStyle
    {
        None,
        Overridden,
        Units,
        Dollar,
        Percentage
    }

    public enum eVariableAccess
    {
        None,
        Overridden,
        //Begin Track #5009 - JScott - Change Edits for XFER
        FollowDetail,
        //End Track #5009 - JScott - Change Edits for XFER
        DisplayOnly,
        Editable
    }

    public enum eVariableWeekType
    {
        None,
        Overridden,
        EOW,
        BOW
    }

    public enum eVariableTimeTotalType
    {
        None,
        First,
        Last,
        FirstAndLast,
        Next,
        All,
        AllPlusNext
    }

    //Begin Track #4616 - JSmith - SQL Server 2005
    public enum eDatabaseType
    {
        None,
        SQLServer2000,
        SQLServer2005,
        SQLServer2008,
        SQLServer2012,
        SQLServer2014,   // TT#1795-MD - JSmith - Support 2014
        SQLServer2016,   // TT#2130-MD - AGallagher - Support 2016
        SQLServer2017,   // TT#1952-MD - AGallagher - Support 2017
        SQLServer2019,   //  AGallagher - Support 2017
        Oracle
    }
    //End Track #4616

    //Begin Track #4637 - JSmith - Split variables by type
    public enum eVariableDatabaseModelType
    {
        Size = 1,
        Color = 2,
        Style = 3,
        //		Intermediate,
        //		PlanLevel,
        All = 4,
        None = 5
    }
    //End Track #4637

    public enum eVariableDatabaseType
    {
        None,
        Integer,
        Real,
        DateTime,
        String,
        //Begin Track #4846 - JSmith - add double datbase type
        //		Char
        Char,
        //Begin Track #4977 - JSmith - add big integer datbase type - split type by chain and store
        //		Float
        Float,
        BigInteger
        //End Track #4846
        //End Track #4977
    }

    public enum eVariableForecastType
    {
        None,
        Sales,
        Stock,
        Other
    }

    //Begin - Abercrombie & Fitch #4411 - JSmith - Chain Forecasting
    public enum eComputationType
    {
        None = 0,
        All = 1,
        Chain = 2,
        Store = 3
    }
    //End - Abercrombie & Fitch #4411

    public enum eEligibilityType
    {
        None,
        Stock,
        Sales,
        Either
    }

    public enum eSimilarStoreDateType
    {
        None,
        Stock,
        Sales
    }

    public enum eProfileListType
    {
        None,
        Master,
        Filtered
    }

    public enum eFilterListType
    {
        Attribute,
        Data,
        Characteristic
    }

    public enum eFilterType
    {
        Permanent,
        Temporary
    }

    public enum eFilterTimeModifyer
    {
        None,
        Any,
        All,
        //Begin Track #5111 - JScott - Add additional filter functionality
        Join,
        //End Track #5111 - JScott - Add additional filter functionality
        Average,
        Total,
        Corresponding
    }

    public enum eFilterCubeModifyer
    {
        None,
        StoreDetail,
        StoreAverage,
        StoreTotal,
        ChainDetail
    }

    public enum eFilterCompareTo
    {
        None,
        Constant,
        Variable,
        PctOf,
        PctChange
    }

    public enum eFilterComparisonType
    {
        Equal = 802360,
        Less = 802361,
        Greater = 802362,
        LessEqual = 802363,
        GreaterEqual = 802364,
        NotEqual = 802365,
        NotLess = 802366,
        NotGreater = 802367,
        NotLessEqual = 802368,
        NotGreaterEqual = 802369
    }

    public enum eFilterConjunctionType
    {
        And,
        Or
    }

    public enum eConjunctionType
    {
        None,
        And,
        Or
    }

    public enum eQuickFilterType
    {
        QuickFilter,
        Find
    }

    public enum eQuickFilterSelectionType
    {
        AllocationHeader = 802340,
        MerchandiseLevel = 802341,
        Color = 802342,
        ColorGroup = 802343,
        Size = 802344,
        Date = 802345,
        Component = 802346,
        Variable = 802347
    }

    public enum ePlanType
    {
        Chain,
        Store,
        None
    }

    //Begin Track #5858 - JSmith - Validating store security only
    public enum ePlanSelectType
    {
        All = 0xFFFF,
        Chain = 0x0001,
        Store = 0x0002
    }
    //End Track #5858

    public enum ePlanBasisType
    {
        Plan = 0,
        Basis = 1
    }

    //Begin Track #5871 - stodd 
    public enum eSecuritySelectType
    {
        None = 0x0000,
        All = 0xFFFF,
        View = 0x0001,
        Update = 0x0002,
        Delete = 0x0004
    }
    // End track #5871

    public enum eBasisIncludeExclude
    {
        Include,
        Exclude
    }

    public enum eValueFormatType
    {
        None,
        GenericNumeric,
        GenericString,
        StoreGrade,
        StoreStatus
    }

    public enum eGetCellMode
    {
        None,
        PreInit,
        PostInit,
        Previous,
        Current
    }

    public enum eUndoEntryType
    {
        None,
        Entry,
        Computation,
        SpreadResult,
        Lock
    }

    public enum eInheritedFrom
    {
        None,
        Node,
        //Begin Track #3863 - JScott - OTS Forecast Level Defaults
        //		HierarchyLevel
        HierarchyLevel,
        HierarchyDefaults,
        //End Track #3863 - JScott - OTS Forecast Level Defaults
        Method
    }

    public enum ePlanValueType
    {
        None,
        Current,
        PostInit,
        Adjusted
    }

    public enum eSpreadType
    {
        ChainToStore,
        HighLevelToLowLevel
    }

    // begin TT#2 - stodd - assortment
    public enum eGradesLoadedFrom
    {
        None,
        ApplyTo,
        Basis
    }

    public enum eAssortmentBasisLoadedFrom
    {
        None,
        AssortmentProperties,
        UserSelectionCriteria,
        GroupAllocation     // TT#952 - MD - stodd - add matrix to Group Allocation Review
    }
    // End TT#2

    // BEGIN TT#488-MD - Stodd - Group Allocation
    /// <summary>
    /// denotes where the action/method/workflow originated from.
    /// Used to help determine if Assortment or group allocation selected headers should used.
    /// </summary>
    public enum eActionOrigin
    {
        Unknown,
        AllocationWorkspace,
        Assortment,
        GroupAllocation,
        MethodOrWorkflow
    }
    // END TT#488-MD - Stodd - Group Allocation

    // Begin TT#4364 - stodd - processing a "Cancel Allocation" action against an individual header within a GA replaces the plan level and grades.
    public enum eGroupAllocationProcessAs
    {
        Unknown = 0,
        Group = 1,
        Headers = 2
    }
    // End TT#4364 - stodd - processing a "Cancel Allocation" action against an individual header within a GA replaces the plan level and grades.

    // Do not change the order of the values in this enumeration.  They are used on the database.
    public enum eLayoutID
    {
        NotDefined = -1, //TT#1443-MD -jsobek -Audit Filters
        explorerDock = 0,
        explorerToolbar = 1,
        eligibilityGrid = 2,
        storeGradeGrid = 3,
        velocityGradeGrid = 4,
        capacityGrid = 5,
        sellThruPctsGrid = 6,
        dailyPercentagesGrid = 7,
        eligibilityModelGrid = 8,
        stockModifierModelGrid = 9,
        salesModifierModelGrid = 10,
        auditViewerGrid = 11,
        similarStoresSelectGrid = 12,
        allocationWorkflowGrid = 13,
        storeMaintGrid = 14,
        allocationWorkspaceGrid = 15,
        allocationViewSelectionGrid = 16,
        textEditorGrid = 17,
        OTSForecastWorkflowGrid = 18,
        // BEGIN MID Track #4370 - John Smith - FWOS Models
        FWOSModifierModelGrid = 19,
        // END MID Track #4370
        productCharacteristicsGrid = 20,
        velocityMatrixGrid = 21,           // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        velocityStoreDetailGrid = 22,            // End TT#231  NOTE: velocityStoreDetailGrid is ComponentOne not Infragistics
        styleReviewGrid = 23,            // TT#454 - RMatelic - Add Views in Style Review 
        sizeReviewGrid = 24,            // TT#456 - RMatelic - Add Views to Size Review  
                                        //Begin TT#155 - JScott - Add Size Curve info to Node Properties
        sizeCurveInheritedCriteriaGrid = 25,
        sizeCurveCriteriaGrid = 26,
        sizeCurveToleranceGrid = 27,
        sizeCurveSimilarStoreGrid = 28,
        //End TT#155 - JScott - Add Size Curve info to Node Properties
        assortmentWorkspaceGrid = 29,    // TT#2 - Assortment Planning
                                         //Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
        sizeOutOfStockGrid = 30,
        //End TT#483 - JScott - Add Size Lost Sales criteria and processing
        //Begin TT#1498 -DOConnell - Chain Plan - Set Percentages - Phase 3
        chainSetPercentGrid = 31,
        //End TT#1498 -DOConnell - Chain Plan - Set Percentages - Phase 3
        FWOSMaxModelGrid = 32, //TT#108 - MD - DOConnell - FWOS Max Model Enhancment
        nodePropertyVSWGrid = 33, // TT#461-MD - JSmith - Node Properties VSW tab error
        //BEGIN TT#46-MD -jsobek -Develop My Activity Log
        userActivityGrid = 34,
        userActivityToolbars = 35,
        //END TT#46-MD -jsobek -Develop My Activity Log
        allocationWorkspaceToolbars = 36,
        // BEGIN TT#765-MD - Stodd - Add toolbars to Assortment Workspace
        assortmentWorkspaceToolbars = 37,
        assortmentReviewToolbars = 38,
        // END TT#765-MD - Stodd - Add toolbars to Assortment Workspace
        // Begin TT#984 - MD - stodd - save content grid format - 
        assortmentReviewContent = 39,
        groupAllocationReviewContent = 40,
        // End TT#984 - MD - stodd - save content grid format - 
        // BEGIN TT#1002-MD - DOConnell - Audit Enhancments
        auditViewerToolbars = 41,
        // END TT#1002-MD - DOConnell - Audit Enhancments
        groupAllocationReviewToolbars = 42,  // TT#1149-MD - RMatelic - GA Matrix - Content Tab - Headings should save based on user preference (like the Content Tab). 
        productFilterSearchResultsGrid = 43, //TT#1388-MD -jsobek -Product Filters
        productFilterSearchResultsMenu = 44, //TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
        auditFilterNestedSearchResultsGrid = 45, //TT#1443-MD -jsobek -Audit Filters
        auditFilterNestedSearchResultsMenu = 46, //TT#1443-MD -jsobek -Audit Filters
        auditFilterMergedSearchResultsGrid = 47, //TT#1443-MD -jsobek -Audit Filters
        auditFilterMergedSearchResultsMenu = 48, //TT#1443-MD -jsobek -Audit Filters
        storeFilterSearchResultsGrid = 49,
        storeFilterSearchResultsMenu = 50
    }

    public enum eProfileType
    {
        None = 0,
        Variable = 1,   // DO NOT CHANGE -- USED ON DATABASE
        QuantityVariable = 2,   // DO NOT CHANGE -- USED ON DATABASE
        PlanValue = 3,  // DO NOT CHANGE -- USED ON DATABASE
        AssortmentComponentVariable = 4,    // DO NOT CHANGE -- USED ON DATABASE
        AssortmentSummaryVariable = 5,  // DO NOT CHANGE -- USED ON DATABASE
        AssortmentTotalVariable = 6,    // DO NOT CHANGE -- USED ON DATABASE
        AssortmentDetailVariable = 7,   // DO NOT CHANGE -- USED ON DATABASE
        AssortmentQuantityVariable = 8, // DO NOT CHANGE -- USED ON DATABASE
                                        //Begin Track #5121 - JScott - Add Year/Season/Quarter totals
        Period = 9, // DO NOT CHANGE -- USED ON DATABASE
        Year = 10,  // DO NOT CHANGE -- USED ON DATABASE
        Season = 11,    // DO NOT CHANGE -- USED ON DATABASE
        Quarter = 12,   // DO NOT CHANGE -- USED ON DATABASE
        Month = 13, // DO NOT CHANGE -- USED ON DATABASE
        Week = 14,  // DO NOT CHANGE -- USED ON DATABASE
                    //End Track #5121 - JScott - Add Year/Season/Quarter totals
                    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        Folder = 15,    // DO NOT CHANGE -- USED ON DATABASE
        FilterStoreMainFavoritesFolder = 16,    // DO NOT CHANGE -- USED ON DATABASE
        FilterStoreMainUserFolder = 17, // DO NOT CHANGE -- USED ON DATABASE
        FilterStoreMainGlobalFolder = 18,   // DO NOT CHANGE -- USED ON DATABASE
        FilterStoreSubFolder = 19,  // DO NOT CHANGE -- USED ON DATABASE
        FilterStore = 20,   // DO NOT CHANGE -- USED ON DATABASE
        MerchandiseMainFavoritesFolder = 21,    // DO NOT CHANGE -- USED ON DATABASE
        MerchandiseMainUserFolder = 22, // DO NOT CHANGE -- USED ON DATABASE
        MerchandiseMainAlternatesFolder = 23,   // DO NOT CHANGE -- USED ON DATABASE
        MerchandiseMainOrganizationalFolder = 24,   // DO NOT CHANGE -- USED ON DATABASE
        MerchandiseSubFolder = 25,  // DO NOT CHANGE -- USED ON DATABASE
        Hierarchy = 26, // DO NOT CHANGE -- USED ON DATABASE
        HierarchyNode = 27,	// DO NOT CHANGE -- USED ON DATABASE
        MerchandiseMainColorFolder = 28,    // DO NOT CHANGE -- USED ON DATABASE
        ColorCode = 29, // DO NOT CHANGE -- USED ON DATABASE
        MerchandiseMainProductCharFolder = 30,  // DO NOT CHANGE -- USED ON DATABASE
        ProductCharacteristic = 31, // DO NOT CHANGE -- USED ON DATABASE
        ProductCharacteristicValue = 32,    // DO NOT CHANGE -- USED ON DATABASE
        StoreGroupMainFavoritesFolder = 33, // DO NOT CHANGE -- USED ON DATABASE
        StoreGroupMainUserFolder = 34,  // DO NOT CHANGE -- USED ON DATABASE
        StoreGroupMainGlobalFolder = 35,    // DO NOT CHANGE -- USED ON DATABASE
        StoreGroupSubFolder = 36,   // DO NOT CHANGE -- USED ON DATABASE
        Store = 37, // DO NOT CHANGE -- USED ON DATABASE
        StoreGroup = 38,    // DO NOT CHANGE -- USED ON DATABASE
        StoreGroupLevel = 39,   // DO NOT CHANGE -- USED ON DATABASE
        TaskListMainFavoritesFolder = 40,   // DO NOT CHANGE -- USED ON DATABASE
        TaskListTaskListMainFolder = 41,    // DO NOT CHANGE -- USED ON DATABASE
        TaskListTaskListMainUserFolder = 42,    // DO NOT CHANGE -- USED ON DATABASE
        TaskListTaskListMainGlobalFolder = 43,  // DO NOT CHANGE -- USED ON DATABASE
        TaskListTaskListMainSystemFolder = 44,  // DO NOT CHANGE -- USED ON DATABASE
        TaskListSubFolder = 45, // DO NOT CHANGE -- USED ON DATABASE
        TaskList = 46,  // DO NOT CHANGE -- USED ON DATABASE
        TaskListJobMainFolder = 47, // DO NOT CHANGE -- USED ON DATABASE
        Job = 48,   // DO NOT CHANGE -- USED ON DATABASE
        TaskListSpecialRequestMainFolder = 49,  // DO NOT CHANGE -- USED ON DATABASE
        SpecialRequest = 50,    // DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodMainFavoritesFolder = 51, // DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodMainUserFolder = 52,  // DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodMainGlobalFolder = 53,    // DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodSubFolder = 54,   // DO NOT CHANGE -- USED ON DATABASE
        Workflow = 55,  // DO NOT CHANGE -- USED ON DATABASE
                        //Begin TT#523 - JScott - Duplicate folder when new folder added
                        //Method								= 56,	// DO NOT CHANGE -- USED ON DATABASE
                        //End TT#523 - JScott - Duplicate folder when new folder added
        OverrideLowLevelModel = 57, // DO NOT CHANGE -- USED ON DATABASE
        PlanView = 58,  // DO NOT CHANGE -- USED ON DATABASE
        MethodOTSPlanFolder = 59,	// DO NOT CHANGE -- USED ON DATABASE
        MethodForecastBalanceFolder = 60,   // DO NOT CHANGE -- USED ON DATABASE
        MethodModifySalesFolder = 61,   // DO NOT CHANGE -- USED ON DATABASE
        MethodForecastSpreadFolder = 62,    // DO NOT CHANGE -- USED ON DATABASE
        MethodCopyChainForecastFolder = 63, // DO NOT CHANGE -- USED ON DATABASE
        MethodCopyStoreForecastFolder = 64, // DO NOT CHANGE -- USED ON DATABASE
        MethodExportFolder = 65,    // DO NOT CHANGE -- USED ON DATABASE
        MethodGlobalUnlockFolder = 66,  // DO NOT CHANGE -- USED ON DATABASE
        MethodRollupFolder = 67,    // DO NOT CHANGE -- USED ON DATABASE
        MethodGeneralAllocationFolder = 68, // DO NOT CHANGE -- USED ON DATABASE
        MethodAllocationOverrideFolder = 69,    // DO NOT CHANGE -- USED ON DATABASE
        MethodRuleFolder = 70,  // DO NOT CHANGE -- USED ON DATABASE
        MethodVelocityFolder = 71,  // DO NOT CHANGE -- USED ON DATABASE
        MethodSizeMethodFolder = 72,	// DO NOT CHANGE -- USED ON DATABASE
        MethodFillSizeHolesFolder = 73, // DO NOT CHANGE -- USED ON DATABASE
        MethodBasisSizeFolder = 74, // DO NOT CHANGE -- USED ON DATABASE
        MethodSizeNeedFolder = 75,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodOTSForcastFolder = 76,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodOTSForcastWorkflowsFolder = 77,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodOTSForcastMethodsFolder = 78,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodAllocationFolder = 79,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodAllocationWorkflowsFolder = 80,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodAllocationMethodsFolder = 81,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodAllocationSizeMethodsFolder = 82,	// DO NOT CHANGE -- USED ON DATABASE
        MethodOTSPlan = 83,	// DO NOT CHANGE -- USED ON DATABASE
        MethodForecastBalance = 84,	// DO NOT CHANGE -- USED ON DATABASE
        MethodModifySales = 85,	// DO NOT CHANGE -- USED ON DATABASE
        MethodForecastSpread = 86,	// DO NOT CHANGE -- USED ON DATABASE
        MethodCopyChainForecast = 87,	// DO NOT CHANGE -- USED ON DATABASE
        MethodCopyStoreForecast = 88,	// DO NOT CHANGE -- USED ON DATABASE
        MethodExport = 89,	// DO NOT CHANGE -- USED ON DATABASE
        MethodGlobalUnlock = 90,	// DO NOT CHANGE -- USED ON DATABASE
        MethodRollup = 91,	// DO NOT CHANGE -- USED ON DATABASE
        MethodGeneralAllocation = 92,	// DO NOT CHANGE -- USED ON DATABASE
        MethodAllocationOverride = 93,	// DO NOT CHANGE -- USED ON DATABASE
        MethodRule = 94,	// DO NOT CHANGE -- USED ON DATABASE
        MethodVelocity = 95,	// DO NOT CHANGE -- USED ON DATABASE
        MethodSizeNeedAllocation = 96,	// DO NOT CHANGE -- USED ON DATABASE
        MethodFillSizeHolesAllocation = 97,	// DO NOT CHANGE -- USED ON DATABASE
        MethodBasisSizeAllocation = 98,	// DO NOT CHANGE -- USED ON DATABASE
        MethodWarehouseSizeAllocation = 99,  // DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodOTSForcastWorkflowsSubFolder = 100,	// DO NOT CHANGE -- USED ON DATABASE
        MethodOTSPlanSubFolder = 101,	// DO NOT CHANGE -- USED ON DATABASE
        MethodForecastBalanceSubFolder = 102,	// DO NOT CHANGE -- USED ON DATABASE
        MethodModifySalesSubFolder = 103,	// DO NOT CHANGE -- USED ON DATABASE
        MethodForecastSpreadSubFolder = 104,	// DO NOT CHANGE -- USED ON DATABASE
        MethodCopyChainForecastSubFolder = 105,	// DO NOT CHANGE -- USED ON DATABASE
        MethodCopyStoreForecastSubFolder = 106,	// DO NOT CHANGE -- USED ON DATABASE
        MethodExportSubFolder = 107,	// DO NOT CHANGE -- USED ON DATABASE
        MethodGlobalUnlockSubFolder = 108,	// DO NOT CHANGE -- USED ON DATABASE
        MethodRollupSubFolder = 109,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodAllocationWorkflowsSubFolder = 110,	// DO NOT CHANGE -- USED ON DATABASE
        MethodGeneralAllocationSubFolder = 111,	// DO NOT CHANGE -- USED ON DATABASE
        MethodAllocationOverrideSubFolder = 112,	// DO NOT CHANGE -- USED ON DATABASE
        MethodRuleSubFolder = 113,	// DO NOT CHANGE -- USED ON DATABASE
        MethodVelocitySubFolder = 114,	// DO NOT CHANGE -- USED ON DATABASE
        MethodSizeMethodSubFolder = 115,	// DO NOT CHANGE -- USED ON DATABASE
        MethodFillSizeHolesSubFolder = 116,	// DO NOT CHANGE -- USED ON DATABASE
        MethodBasisSizeSubFolder = 117,	// DO NOT CHANGE -- USED ON DATABASE
        MethodSizeNeedSubFolder = 118,	// DO NOT CHANGE -- USED ON DATABASE
        // Begin TT#155 - JSmith - Size Curve Method
        MethodSizeCurve = 119,	// DO NOT CHANGE -- USED ON DATABASE
        MethodSizeCurveFolder = 120,	// DO NOT CHANGE -- USED ON DATABASE
        MethodSizeCurveSubFolder = 121,	// DO NOT CHANGE -- USED ON DATABASE
        // End TT#155
        // Begin TT#370 - APicchetti - Build Packs Method
        MethodBuildPacks = 122,	// DO NOT CHANGE -- USED ON DATABASE
        MethodBuildPacksFolder = 123,	// DO NOT CHANGE -- USED ON DATABASE
        MethodBuildPacksSubFolder = 124,    // DO NOT CHANGE -- USED ON DATABASE
                                            // End TT#370
                                            // Begin TT#2 - stodd - Assortment
        AssortmentMainFavoritesFolder = 125,    // DO NOT CHANGE -- USED ON DATABASE
        AssortmentMainFolder = 126, // DO NOT CHANGE -- USED ON DATABASE
        AssortmentSubFolder = 127,  // DO NOT CHANGE -- USED ON DATABASE
        Assortment = 128,   // DO NOT CHANGE -- USED ON DATABASE
                            // End TT#2 - stodd - Assortment
                            //Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
        ChainSetPercentValue = 129,	// DO NOT CHANGE -- USED ON DATABASE (was originally 125 - stodd)
        //End TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
        //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
        MethodGlobalLock = 130, // DO NOT CHANGE -- USED ON DATABASE
        MethodGlobalLockFolder = 131,   // DO NOT CHANGE -- USED ON DATABASE
        MethodGlobalLockSubFolder = 132,    // DO NOT CHANGE -- USED ON DATABASE
                                            //End TT#43 - MD - DOConnell - Projected Sales Enhancement
                                            //DO NOT DELETE OR REORDER ANY EXISTING ITEMS ABOVE THIS LINE -- THEY ARE USED ON DATABASE.  NEW ITEMS CAN BE ADDED AT END OF LIST.

        // Begin TT#110-MD - RMatelic - In Use Tool -  added explicit nubers to remaining profile types
        FilterMainSharedFolder = 133,
        MerchandiseMainSharedFolder = 134,
        StoreGroupMainSharedFolder = 135,
        TaskListTaskListMainSharedFolder = 136,
        WorkflowMethodMainSharedFolder = 137,
        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        ChainPlan = 138,
        StorePlan = 139,
        Plan = 140,
        Basis = 141,
        BasisDetail = 142,
        Version = 143,
        LowLevelTotalVersion = 144,
        Date = 145,
        Day = 146,
        //Begin Track #5121 - JScott - Add Year/Season/Quarter totals
        //Week,
        //End Track #5121 - JScott - Add Year/Season/Quarter totals
        BasisWeek = 147,
        //Begin Track #5121 - JScott - Add Year/Season/Quarter totals
        //Period,
        //Year,
        //End Track #5121 - JScott - Add Year/Season/Quarter totals
        Week53Year = 148,
        DateRange = 149,
        TimeTotalVariable = 150,
        ChainTimeTotalIndex = 151,
        StoreTimeTotalIndex = 152,
        PID = 153,
        LowLevelTotal = 154,
        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        //Store,
        //StoreGroup,
        //StoreGroupLevel,
        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        StoreTotal = 155,
        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        //Hierarchy,
        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        HierarchyLevel = 156,
        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        //HierarchyNode,
        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        HierarchyChild = 157,
        HierarchyChildren = 158,
        HierarchyJoin = 159,
        HierarchyNodeAncestor = 160,
        Formula = 161,
        Spread = 162,
        ChangeMethod = 163,
        EligibilityModel = 164,
        StockModifierModel = 165,
        SalesModifierModel = 166,
        DailyPctModel = 167,
        StoreEligibility = 168,
        StoreGrade = 169,
        StoreCapacity = 170,
        VelocityGrade = 171,
        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        //ColorCode,
        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        SizeCode = 172,
        Clipboard = 173,
        NotUsed12 = 174,
        AllocationVariables = 175,
        Allocation = 176,
        AllocationSubtotal = 177,
        AllocationAction = 178,
        AllocationWorkFlowStep = 179,
        RuleAllocation = 180,
        AllocationQuickFilter = 181,
        DailySalesRdr = 182,
        PlanLevel = 183,
        OTSPlan = 184,
        OTSPlanWorkFlowStep = 185,
        OnHand = 186,
        Intransit = 187,

        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        //Method,
        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        //MethodOTSPlan,
        //MethodForecastBalance,
        //MethodGeneralAllocation,
        //MethodAllocationOverride,
        //MethodVelocity,
        //MethodRule,
        //MethodFillSizeHolesAllocation,
        //MethodBasisSizeAllocation,
        //MethodWarehouseSizeAllocation,
        //MethodSizeNeedAllocation,
        //MethodOverrideAllocation,
        //MethodSizeOverrideAllocation,
        //MethodStyleNeed,
        MethodPackContentAllocation = 188,
        MethodKey = 189,
        MethodInventory = 190,
        //Begin Enhancement - JScott - Export Method - Part 2
        //MethodForecastExport,
        //End Enhancement - JScott - Export Method - Part 2

        GroupLevelFunction = 191,
        TrendCaps = 192,
        // BEGIN Issue 4818
        GroupLevelBasis = 193,
        BasisPlan = 194,
        BasisRange = 195,
        // END Isuue 4818
        StockMinMax = 196,

        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        //Workflow,
        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        WorkflowMethodExpStaticNode = 197,

        StoreValue = 198,
        GlobalOptions = 199,
        DailyPercentages = 200,
        StoreDailyPercentages = 201,
        SizeGroup = 202,
        SellThruPct = 203,

        AllocationViewSelectionCriteria = 204,
        SelectableRowHeader = 205,
        Task = 206,
        AssortmentHeader = 207,
        PlaceholderHeader = 208,
        AllocationHeader = 209,
        HeaderColor = 210,
        Style = 211,
        SimilarStore = 212,
        HierarchyNodeDescendant = 213,
        StoreGradeInventory = 214,
        StoreModifier = 215,
        SelectedHeader = 216,
        ModelName = 217,
        SelectedComponent = 218,
        StoreStatus = 219,
        SizeCurve = 220,
        SizeCurveGroup = 221,
        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        //TaskList,
        //Job,
        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        Schedule = 222,
        ScheduleJobJoin = 223,
        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        //StoreFilter,
        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        StoreGroupListView = 224,
        StoreGroupLevelListView = 225,
        StoreListView = 226,
        HeaderPack = 227,
        HeaderPackColor = 228,
        HeaderPackColorSize = 229,
        HeaderBulkColor = 230,
        HeaderBulkColorSize = 231,
        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        //Folder,
        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        HierarchyNodeParentIds = 232,
        HeaderCharGroup = 233,
        HeaderChar = 234,
        AllocationWorkspaceFilter = 235,
        Security = 236,
        SecurityHierarchyNode = 237,
        SecurityFunction = 238,
        SecurityVersion = 239,
        SecurityWorkflowMethod = 240,
        SizeConstraintModel = 241,
        SizeFringeModel = 242,
        SizeAlternateModel = 243,
        LowLevelVersionOverride = 244,
        OTSPlanAction = 245,
        OTSForecastVariable = 246,
        HeaderType = 247,
        ForecastingModel = 248,
        ForecastBalanceModel = 249,
        ModelVariable = 250,
        IntransitReadNode = 251,   //Track #4362 - JSmith - Intransit read performance
                                   //Begin Track #4457 - JSmith - Add forecast versions
                                   //		UserOptions
        UserOptions = 252,
        BasisHierarchyNode = 253,
        BasisVersion = 254,
        //End Track #4457
        // BEGIN MID Track #4370 - John Smith - FWOS Models
        FWOSModifierModel = 255,
        // FWOS MID Track
        FWOSMaxModel = 256, //TT#108 - MD - DOConnell - FWOS Max Model Enhancement
        LowLevelExclusions = 257,
        NodeStockMinMaxes = 258,
        NodeStockMinMaxSet = 259,
        NodeStockMinMaxBoundary = 260,
        // BEGIN MID Track #4827 - John Smith - Presentation plus sales
        //		NodeStockMinMax
        NodeStockMinMax = 261,
        PMPlusSales = 262,
        // END MID Track #4827
        AuditFilter = 263,
        HeaderComponent = 264,
        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        //OverrideLowLevelModel,
        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        OverrideLowLevelDetail = 265,
        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        //SpecialRequest,	// Issue 5117
        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        MethodGeneralAssortment = 266,
        NodeLockRequest = 267,
        NodeLockConflict = 268,
        // Begin TT#2 - stodd - assortment. Moved up higher in the list.
        //Assortment,
        // End TT#2 - stodd 
        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        //ProductCharacteristicValue,
        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        Placeholder = 269,
        AssortmentSummaryItem = 270,
        AssortmentAction = 271,
        AssortmentWorkFlowStep = 272,
        ProductSearchCol = 273,
        AssortmentSummary = 274,
        AssortmentSubTotal = 275,
        AllocationComponent = 276,
        AssortmentComponent = 277,
        // ProductCharacteristic should always be last
        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        //ProductCharacteristic,
        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        PlaceholderComponentLink = 278,
        // Begin Track #5005 - JSmith - Explorer Organization
        //BasisLabelType
        BasisLabelType = 279,
        ColorGroup = 280,
        // End Track #5005
        // Begin TT#155 - JSmith - Size Curve Method
        SizeCurveMerchBasis = 281,
        //Begin TT#155 - JScott - Add Size Curve info to Node Properties
        SizeCurveMethodToleranc = 282,
        //End TT#155 - JScott - Add Size Curve info to Node Properties
        SizeCurveCurveBasis = 283,
        SizeCurveCriteria = 284,
        SizeCurveDefaultCriteria = 285,
        SizeCurveTolerance = 286,
        SizeCurveSimilarStore = 287,
        // End TT#155
        OptionPackProfile = 288, // TT#370 Build Packs Enhancement
        AssortmentWorkspaceFilter = 289, // TT#2 - AssortmentPlanning
        AssortmentViewSelectionCriteria = 290, // TT#2 - AssortmentPlanning
        AssortmentSummaryStoreDetail = 291, // TT#2 - AssortmentPlanning
                                            // Begin TT#1227 - stodd - sort seq
        PlaceholderSeq = 292,
        HeaderSeq = 293,
        // End TT#1227 - stodd - sort seq
        //Begin TT#1498 -DOConnell - Chain Plan - Set Percentages - Phase 3
        ChainSetPercentListView = 294,
        ChainSetPercent = 295,
        //End TT#1498 -DOConnell - Chain Plan - Set Percentages - Phase 3
        //Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
        SizeOutOfStockHeader = 296,
        SizeOutOfStock = 297,
        SizeSellThru = 298,
        //End TT#483 - JScott - Add Size Lost Sales criteria and processing
        //Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
        SizeOutOfStockLookup = 299,
        //End TT#483 - JScott - Add Size Lost Sales criteria and processing
        IMO = 300,				// TT#1401 - reservation stores (IMO)
        IMOReadNode = 301,	// TT#1401 - reservation stores (IMO)
        StoreCharacteristics = 302,   // TT#2015 - gtaylor - apply changes to lower levels
        PurgeCriteria = 303,         // TT#2015 - gtaylor - apply changes to lower levels
        Service = 304,        // TT#195 MD - JSmith - Add environment authentication
        // End TT#110-MD 
        StoreCharacteristicValues = 305,    // TT#643-MD-VStuart-Need Queries for Store Characteristic Values
                                            // BEGIN TT#708-MD - Stodd - Group Allocation Prototype.
        MethodGroupAllocation = 306,
        MethodGroupAllocationFolder = 307,
        MethodGroupAllocationSubFolder = 308,
        AssortmentMember = 309,
        // END TT#708-MD - Stodd - Group Allocation Prototype.

        //Begin TT#1313-MD -jsobek -Header Filters
        FilterHeaderMainFavoritesFolder = 310,	// DO NOT CHANGE -- USED ON DATABASE
        FilterHeaderMainUserFolder = 311,	// DO NOT CHANGE -- USED ON DATABASE
        FilterHeaderMainGlobalFolder = 312,	// DO NOT CHANGE -- USED ON DATABASE
        FilterHeaderSubFolder = 313,	// DO NOT CHANGE -- USED ON DATABASE
        FilterHeader = 314,	// DO NOT CHANGE -- USED ON DATABASE

        FilterAssortmentMainFavoritesFolder = 315,	// DO NOT CHANGE -- USED ON DATABASE
        FilterAssortmentMainUserFolder = 316,	// DO NOT CHANGE -- USED ON DATABASE
        FilterAssortmentMainGlobalFolder = 317,	// DO NOT CHANGE -- USED ON DATABASE
        FilterAssortmentSubFolder = 318,	// DO NOT CHANGE -- USED ON DATABASE
        FilterAssortment = 319,	// DO NOT CHANGE -- USED ON DATABASE
        //End TT#1313-MD -jsobek -Header Filters
        // Begin TT#1652-MD - RMatelic - DC Carton Rounding
        MethodDCCartonRounding = 320,
        MethodDCCartonRoundingFolder = 321,
        MethodDCCartonRoundingSubFolder = 322,
        // End TT#1652-MD
        ColorSeq = 323, //TT#664 - MD - DOConnell - Placeholder colors display in reverse order on the Content Tab
        FilterStoreGroup = 330,
        // Begin TT#1966-MD - JSmith - DC Fulfillment
        MethodCreateMasterHeaders = 331,
        MethodCreateMasterHeadersFolder = 332,
        MethodCreateMasterHeadersSubFolder = 333,
        MethodDCFulfillment = 334,
        MethodDCFulfillmentFolder = 335,
        MethodDCFulfillmentSubFolder = 336,
        MasterHeaderSubordinates = 337,
        // End TT#1966-MD - JSmith - DC Fulfillment
        AssortmentGrade = 338,   // TT#2066-MD - AGallagher - Ship to Date validation.  Is this how it should be working
        // Begin TT#2131-MD - JSmith - Halo Integration
        MethodPlanningExtract = 339,
        MethodPlanningExtractFolder = 340,
        MethodPlanningExtractSubFolder = 341,
        // End TT#2131-MD - JSmith - Halo Integration
    }

    // Begin Track #6302 - JSmith - Assign user permanently to another, folders do not come over 
    public enum eDoNotAssignProfileTypes
    {
        FilterMainFavoritesFolder = 16,	// DO NOT CHANGE -- USED ON DATABASE
        FilterMainUserFolder = 17,	// DO NOT CHANGE -- USED ON DATABASE
        FilterMainGlobalFolder = 18,	// DO NOT CHANGE -- USED ON DATABASE
        MerchandiseMainFavoritesFolder = 21,	// DO NOT CHANGE -- USED ON DATABASE
        MerchandiseMainUserFolder = 22,	// DO NOT CHANGE -- USED ON DATABASE
        MerchandiseMainAlternatesFolder = 23,	// DO NOT CHANGE -- USED ON DATABASE
        MerchandiseMainOrganizationalFolder = 24,	// DO NOT CHANGE -- USED ON DATABASE
        MerchandiseMainColorFolder = 28,	// DO NOT CHANGE -- USED ON DATABASE
        MerchandiseMainProductCharFolder = 30,	// DO NOT CHANGE -- USED ON DATABASE
        StoreGroupMainFavoritesFolder = 33,	// DO NOT CHANGE -- USED ON DATABASE
        StoreGroupMainUserFolder = 34,	// DO NOT CHANGE -- USED ON DATABASE
        StoreGroupMainGlobalFolder = 35,	// DO NOT CHANGE -- USED ON DATABASE
        TaskListMainFavoritesFolder = 40,	// DO NOT CHANGE -- USED ON DATABASE
        TaskListTaskListMainFolder = 41,	// DO NOT CHANGE -- USED ON DATABASE
        TaskListTaskListMainUserFolder = 42,	// DO NOT CHANGE -- USED ON DATABASE
        TaskListTaskListMainGlobalFolder = 43,	// DO NOT CHANGE -- USED ON DATABASE
        TaskListTaskListMainSystemFolder = 44,	// DO NOT CHANGE -- USED ON DATABASE
        TaskListJobMainFolder = 47,	// DO NOT CHANGE -- USED ON DATABASE
        TaskListSpecialRequestMainFolder = 49,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodMainFavoritesFolder = 51,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodMainUserFolder = 52,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodMainGlobalFolder = 53,	// DO NOT CHANGE -- USED ON DATABASE
    }

    public enum eMainUserProfileType
    {
        FilterStoreMainUserFolder = 17,	// DO NOT CHANGE -- USED ON DATABASE
        FilterHeaderMainUserFolder = 311,	// DO NOT CHANGE -- USED ON DATABASE //TT#1313-MD -jsobek -Header Filters
        FilterAssortmentMainUserFolder = 316,	// DO NOT CHANGE -- USED ON DATABASE //TT#1313-MD -jsobek -Header Filters
        MerchandiseMainUserFolder = 22,	// DO NOT CHANGE -- USED ON DATABASE
        StoreGroupMainUserFolder = 34,	// DO NOT CHANGE -- USED ON DATABASE
        TaskListTaskListMainUserFolder = 42,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodMainUserFolder = 52,	// DO NOT CHANGE -- USED ON DATABASE
    }
    // End Track #6302

    public enum eMethodProfileType
    {
        Method = 56,	// DO NOT CHANGE -- USED ON DATABASE
        MethodOTSPlan = 83,	// DO NOT CHANGE -- USED ON DATABASE
        MethodForecastBalance = 84,	// DO NOT CHANGE -- USED ON DATABASE
        MethodModifySales = 85,	// DO NOT CHANGE -- USED ON DATABASE
        MethodForecastSpread = 86,	// DO NOT CHANGE -- USED ON DATABASE
        MethodCopyChainForecast = 87,	// DO NOT CHANGE -- USED ON DATABASE
        MethodCopyStoreForecast = 88,	// DO NOT CHANGE -- USED ON DATABASE
        MethodExport = 89,	// DO NOT CHANGE -- USED ON DATABASE
        MethodGlobalUnlock = 90,	// DO NOT CHANGE -- USED ON DATABASE
        MethodRollup = 91,	// DO NOT CHANGE -- USED ON DATABASE
        MethodGeneralAllocation = 92,	// DO NOT CHANGE -- USED ON DATABASE
        MethodAllocationOverride = 93,	// DO NOT CHANGE -- USED ON DATABASE
        MethodRule = 94,	// DO NOT CHANGE -- USED ON DATABASE
        MethodVelocity = 95,	// DO NOT CHANGE -- USED ON DATABASE
        MethodSizeNeedAllocation = 96,	// DO NOT CHANGE -- USED ON DATABASE
        MethodFillSizeHolesAllocation = 97,	// DO NOT CHANGE -- USED ON DATABASE
        MethodBasisSizeAllocation = 98,	// DO NOT CHANGE -- USED ON DATABASE
        MethodWarehouseSizeAllocation = 99,  // DO NOT CHANGE -- USED ON DATABASE
                                             // Begin TT#155 - JSmith - Size Curve Method
        MethodSizeCurve = 119,  // DO NOT CHANGE -- USED ON DATABASE
                                // End TT#155 - JSmith - Size Curve Method
                                //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
        MethodGlobalLock = 130, // DO NOT CHANGE -- USED ON DATABASE
                                //END TT#43 - MD - DOConnell - Projected Sales Enhancement
        MethodGroupAllocation = 306,	// DO NOT CHANGE -- USED ON DATABASE // TT#708-MD - Stodd - Group Allocation Prototype.
        MethodDCCartonRounding = 320,  // DO NOT CHANGE -- USED ON DATABASE // TT#1652-MD - RMatelic - DC Carton Rounding
        MethodCreateMasterHeaders = 331,   // TT#1966-MD - JSmith - DC Fulfillment
        MethodDCFulfillment = 334,   // TT#1966-MD - JSmith - DC Fulfillment
        MethodPlanningExtract = 339,  // TT#2131-MD - JSmith - Halo Integration
    }

    // Begin Track #6247 stodd
    public enum eMethodForecastProfileType
    {
        MethodOTSPlan = 83, // DO NOT CHANGE -- USED ON DATABASE
        MethodForecastBalance = 84, // DO NOT CHANGE -- USED ON DATABASE
        MethodModifySales = 85, // DO NOT CHANGE -- USED ON DATABASE
        MethodForecastSpread = 86,  // DO NOT CHANGE -- USED ON DATABASE
        MethodCopyChainForecast = 87,   // DO NOT CHANGE -- USED ON DATABASE
        MethodCopyStoreForecast = 88,   // DO NOT CHANGE -- USED ON DATABASE
        MethodExport = 89,  // DO NOT CHANGE -- USED ON DATABASE
        MethodGlobalUnlock = 90,    // DO NOT CHANGE -- USED ON DATABASE
        MethodRollup = 91,	// DO NOT CHANGE -- USED ON DATABASE
        //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
        MethodGlobalLock = 130, // DO NOT CHANGE -- USED ON DATABASE
        MethodGroupAllocation = 306,	// DO NOT CHANGE -- USED ON DATABASE // TT#708-MD - Stodd - Group Allocation Prototype.
        //END TT#43 - MD - DOConnell - Projected Sales Enhancement
        MethodPlanningExtract = 339,  // TT#2131-MD - JSmith - Halo Integration
    }

    public enum eMethodAllocationProfileType
    {
        MethodGeneralAllocation = 92,   // DO NOT CHANGE -- USED ON DATABASE
        MethodAllocationOverride = 93,  // DO NOT CHANGE -- USED ON DATABASE
        MethodRule = 94,    // DO NOT CHANGE -- USED ON DATABASE
        MethodVelocity = 95,    // DO NOT CHANGE -- USED ON DATABASE
        MethodSizeNeedAllocation = 96,  // DO NOT CHANGE -- USED ON DATABASE
        MethodFillSizeHolesAllocation = 97, // DO NOT CHANGE -- USED ON DATABASE
        MethodBasisSizeAllocation = 98, // DO NOT CHANGE -- USED ON DATABASE
        MethodWarehouseSizeAllocation = 99,  // DO NOT CHANGE -- USED ON DATABASE
                                             // Begin TT#155 - JSmith - Size Curve Method
        MethodSizeCurve = 119,  // DO NOT CHANGE -- USED ON DATABASE
                                // End TT#155 - JSmith - Size Curve Method
                                //Begin TT#479 - MD - Can not Drag/Drop a Build Packs Method to an Allocation Workflow new line
        MethodBuildPacks = 122, // DO NOT CHANGE -- USED ON DATABASE  
        MethodGroupAllocation = 306,    // DO NOT CHANGE -- USED ON DATABASE // TT#708-MD - Stodd - Group Allocation Prototype.
                                        //End   TT#479 - MD - Can not Drag/Drop a Build Packs Method to an Allocation Workflow new line
        MethodDCCartonRounding = 320, // DO NOT CHANGE -- USED ON DATABASE // TT#1652-MD - RMatelic - DC Carton Rounding
        MethodCreateMasterHeaders = 331,   // TT#1966-MD - JSmith - DC Fulfillment
        MethodDCFulfillment = 334,   // TT#1966-MD - JSmith - DC Fulfillment
    }
    // End Track #6247 stodd

    public enum eWorkflowProfileType
    {
        Workflow = 55,	// DO NOT CHANGE -- USED ON DATABASE
    }
    // Begin Track #5005 - JSmith - Explorer Organization
    //    public enum eClipboardDataType
    //    {
    //        HierarchyNode,
    //        // Begin Track #5005 - JSmith - Explorer Organization
    //        HierarchyFolder,
    //        // End Track #5005
    //        Store,
    //        Workflow,
    //        Method,
    //        Attribute,
    //        AttributeSet,
    //// Begin Assortment Planning - Multi select
    ////		Header
    //        Header,
    //        HierarchyNodeList,
    //// End Assortment Planning
    //        Color,
    //        ColorList,
    //        ProductChar,
    //        ProductCharList,
    //        StoreList,
    //        Filter,
    //        FilterList,
    //        Task,
    //        TaskList,
    //        TaskListList,
    //        Job
    //    }
    // End Track #5005

    public enum eComputationScheduleEntryType
    {
        AutoTotal,
        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        //ForcedReInit,
        //End TT#2 - JScott - Assortment Planning - Phase 2
        Spread,
        Formula
    }

    public enum eComputationFormulaReturnType
    {
        Successful,
        //Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
        //Pending
        Pending,
        SkippedAutoTotal
        //End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
    }

    public enum eSetCellMode
    {
        Load,
        Entry,
        Initialize,
        InitializeCurrent,
        Computation,
        AutoTotal,
        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        //ForcedReInit
        //End TT#2 - JScott - Assortment Planning - Phase 2
    }

    public enum eLastPendingSpreadCellType
    {
        SpreadFromCell,
        SpreadToCell
    }

    public enum eDateRangeRelativeTo
    {
        None = -1,
        Current = 0,
        Plan = 1,
        StoreOpen = 2
    }

    public enum eServerType
    {
        All = 0xFFFF,
        Client = 0x0001,
        Control = 0x0002,
        Store = 0x0004,
        Hierarchy = 0x0008,
        Application = 0x0010,
        Scheduler = 0x0020,
        Header = 0x0040
    }

    public enum eEligibilitySettingType
    {
        None = 0,
        SetIneligible = 1,
        Model = 2,
        SetEligible = 3
    }

    public enum eModifierType
    {
        None = 0,
        Percent = 1,
        Model = 2
    }

    public enum eSimilarStoreType
    {
        None = 0,
        Stores = 1
    }

    public enum eEligModelEntryType
    {
        StockEligibility = 0,
        SalesEligibility = 1,
        PriorityShipping = 2
    }

    public enum eModelType
    {
        None = 0,
        Eligibility = 1,
        StockModifier = 2,
        SalesModifier = 3,
        DailyPercentages = 4,
        Forecasting = 5,
        ForecastBalance = 6,
        SizeAlternates = 7,
        SizeConstraints = 8,
        SizeGroup = 9,
        SizeCurve = 10,
        // BEGIN MID Track #4370 - John Smith - FWOS Models
        FWOSModifier = 11,
        // END MID Track #4370
        //BEGIN TT#108 - MD - doconnell - FWOS Max Model
        FWOSMax = 12,
        //END TT#108 - MD - doconnell - FWOS Max Model
        OverrideLowLevel = 13
    }

    public enum eSearchDirection
    {
        Down = 0,
        Up = 1,
        All = 2
    }

    public enum eSearchContent
    {
        AnyPartOfField = 0,
        WholeField = 1,
        StartOfField = 2,
        EndOfField = 3,
    }

    public enum eBasisLabelType
    {
        Merchandise = 0,
        Version = 1,
        Time_Period = 2
    }

    // Begin TT#2 - stodd - assortment
    public enum eSpreadAverage
    {
        None = 0,
        SpreadByIndex = 1,
        Smooth = 2
    }
    // End TT#2 - stodd - assortment

    //Begin TT#1313-MD -jsobek -Header Filters
    public enum eWorkspaceType
    {
        AllocationWorkspace = 0, //used on DB - do not change
        AssortmentWorkspace = 1
    }
    //End TT#1313-MD -jsobek -Header Filters

    //Begin Track #5005 - JSmith - Explorer Organization
    //public enum eHierarchyNodeType
    //{
    //    MyHierarchyFolder				= 0, 
    //    OrganizationalHierarchyFolder	= 1,
    //    AlternateHierarchyFolder		= 2,
    //    MyHierarchyRoot					= 3, 
    //    OrganizationalHierarchyRoot		= 4,
    //    AlternateHierarchyRoot			= 5,
    //    //Begin Track #5005 - JSmith - Explorer Organization
    //    //TreeNode = 6
    //    TreeNode = 6,
    //    MyFavoritesFolder = 7,
    //    SharedFolder = 8,
    //    FavoriteFolder = 9,
    //    //End Track #5005
    //}
    //End Track #5005

    public enum eHierarchySelectType
    {
        MyHierarchyFolder = 0,
        OrganizationalHierarchyFolder = 1,
        AlternateHierarchyFolder = 2,
        FavoritesFolder = 3,
        MyHierarchyRoot = 4,
        OrganizationalHierarchyRoot = 5,
        AlternateHierarchyRoot = 6,
        HierarchyNode = 7,
    }

    public enum eHierarchySearchType
    {
        HomeHierarchyOnly = 0,
        AlternateHierarchiesOnly = 1,
        SpecificHierarchy = 2,
        AllHierarchies = 3
    }

    //Begin TT#155 - JScott - Add Size Curve info to Node Properties
    public enum eNodeChainSalesType
    {
        None = 0,
        IndexToAverage = 1,
        Units = 2,
    }

    //Begin Track #5005 - JSmith - Explorer Organization
    //public enum eColorNodeType
    //{
    //    GroupFolder = 0,
    //    ColorNode = 1
    //}
    //End Track #5005

    //Begin Track #5005 - JSmith - Explorer Organization
    //public enum eProductCharNodeType
    //{
    //    ProductCharFolder = 0,
    //    ProductChar = 1,
    //    ProductCharValue = 2
    //}

    //public enum eDropAction
    //{
    //    None							= 0, 
    //    Copy							= 1,
    //    Move							= 2,
    //    MakeShortCut					= 3
    //}
    //End Track #5005

    public enum eStoreStatus
    {
        None = -1,
        New = 804000,
        Comp = 804001,
        NonComp = 804002,
        Closed = 804003,
        Preopen = 804004
    }

    // MIDTextType is used to group like types of text for use in list boxes, etc
    // This must match the TEXT_TYPE column of the APPLICATION_TEXT
    public enum eMIDTextType
    {
        message = 0,
        label = 1,
        eHierarchyType = 2,
        eLevelLengthType = 3,
        eHierarchyLevelType = 4,
        eProductType = 5,
        eProcesses = 6,
        //		eProcessStatus					= 7,
        eProcessCompletionStatus = 7,
        eOTSPlanLevelType = 8,
        eStores = 9,
        eMethodType = 10,
        eGroupLevelFunctionType = 11,
        eGroupLevelSmoothBy = 12,
        eStoreDisplayOptions = 13,
        eTyLyType = 14,
        eCalendarDateType = 15,
        eMIDMessageLevel = 16,
        eMethod = 17,
        eGroupLevelRelativeTo = 18,
        eSecurityFunctions = 19,
        eSecurityViews = 20,
        eHierarchyDisplayOptions = 21,
        eStorePlanSelectedGroupBy = 22,
        eComponentType = 24,
        eHierarchyIDFormat = 25,
        eWorkflowMethodNodes = 26,
        eWorkflowType = 27,
        eGlobalUserType = 28,
        eWorkflowMethodIND = 29,
        ePurgeTimeframe = 30,
        eHeaderType = 31,
        ePackType = 32,
        eAllocationStyleGroupBy = 33,
        eAllocationSizeGroupBy = 34,
        eAllocationSummaryGroupBy = 35,
        eHeaderNode = 36,
        eStoreAllocationNode = 37,
        eRuleMethod = 38,
        eTaskType = 39,
        //		eTaskStatus						= 40,
        //		eScheduleTimeUnit				= 41,
        //		eSceduleType					= 42,
        //		eTaskPriority					= 43,
        //		eAllocationTaskType				= 44,
        eProcessExecutionStatus = 40,
        eScheduleByType = 41,
        eScheduleByMonthWeekType = 42,
        eScheduleRepeatIntervalType = 43,
        eScheduleConditionType = 44,
        eHeaderAllocationStatus = 45,
        eHeaderShipStatus = 46,
        eHeaderIntransitStatus = 47,
        eAllocationActionStatus = 48,
        eRuleType = 49,
        eQuickFilterSelectionType = 50,
        eAllocationVelocityGroupBy = 51,
        eFillSizeHolesRuleType = 52,
        eFillSizeHolesSort = 53,
        eStoreStatus = 54,
        eAllocationWaferVariable = 55,
        eBasisSizeSort = 56,
        eBasisSizeMethodRuleType = 57,
        eAllocationSizeView2ndGroupBy = 58,
        eSizeRuleType = 59,     // MID Track 3619 Remove Fringe
        eEquateOverrideSizeType = 60,
        //Begin Assortment Planning - JScott - Assortment Planning Changes
        eAllocationAssortmentGroupBy = 61,
        eAssortmentActionType = 62,
        //End Assortment Planning - JScott - Assortment Planning Changes
        //eFringeOverrideOperator			= 61, // MID Track 3619 Remove Fringe
        //eFringeOverrideUnitCriteria		= 62, // MID Track 3619 Remove Fringe
        //eFringeOverrideCondition		    = 63, // MID Track 3619 Remove Fringe
        //eFringeOverrideSort				= 64, // MID Track 3619 Remove Fringe
        eFilterDateType = 65,
        eSecurityAction = 66,
        eDatabaseSecurityTypes = 67,
        eSecurityOwnerType = 68,
        toolTip = 69,
        //eFringeFilterValueType			= 70, // MID Track 3619 Remove Fringe
        ePeriodName = 71,
        ePeriodAbbreviation = 72,
        eTrendCapID = 73,
        eUnitScaling = 74,
        //Begin Track #4457 - JSmith - Add forecast versions
        //		eDollarScaling					= 75
        eDollarScaling = 75,
        // BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
        //		eForecastBlendType				= 76
        eForecastBlendType = 76,
        //End Track #4457
        //Begin Assortment Planning - JScott - Assortment Planning Changes
        //eFillSizesToType				= 77
        // END MID Track #4921
        eFillSizesToType = 77,
        eAssortmentComponentVariables = 78,
        eAssortmentTotalVariables = 79,
        eAssortmentDetailVariables = 80,
        eAssortmentSummaryVariables = 81,
        //End Assortment Planning - JScott - Assortment Planning Changes
        eQuarterName = 82,
        eQuarterAbbreviation = 83,
        eSeasonName = 84,
        eSeasonAbbreviation = 85,       // TT#173  Provide database container for large data collections
        eAllocationDatabaseStoreVariables = 86, // TT#173  Provide database container for large data collections
        eForecastBaseDatabaseStoreVariables = 87, // TT#173  Provide database container for large data collections
        eForecastCustomDatabaseStoreVariables = 88,  // TT#173  Provide database container for large data collections
        eForecastFormulaType = 89,  // TT#248 - RMatelic - Global options Display options display extra Store Display options
        eGenerateSizeCurveUsing = 90,  // TT#391 - stodd - adding in stock sales to global 
        eVelocityMinMax = 91,  // TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
        eAdjustVSW = 92,	//  TT#2225 - stodd - VSW ANF Enhancement (IMO)
        eVSWSizeConstraints = 99,	//  TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
        eAPIFileProcessingDirection = 100,  // TT#645-MD - JSmith - Add File Processing Direction Parameter to Tasks on Task Lists that Process Input Files
        eGroupAllocationGroupBy = 101,
        eBatchComp = 102,				// TT#1595-MD - stodd - Batch Comp
        eAssortmentDisplayType = 103,  // TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only 
    }

    // Message level enums
    // must match code in text load 
    public enum eMIDMessageLevel
    {
        None = 0,
        Debug = 1,
        Information = 2,
        NothingToDo = 3,
        Edit = 4,
        Warning = 5,
        Error = 6,
        Severe = 7,
        //Begin TT#429 - JScott - Size Curve- Node Properties When requesting different size groups for the same level need a Conflict Warning.
        HandledEdit = 8,
        //End TT#429 - JScott - Size Curve- Node Properties When requesting different size groups for the same level need a Conflict Warning.
        // Begin TT#1127 - JSmith - Severe error didn't roll up to Scheduler Service
        Ignore = 10,
        // End TT#1127
        ListBoxItems = 12,
        WindowCaption = 13,
        Treeview = 14,
        ProcessUnavailable = 20,   // TT#1581-MD - stodd - Header Reconcile
        Cancelled = 99,
    }

    public enum eMIDTextValueType
    {
        String = 0,
        Boolean = 1,
        Date = 2,
        Numeric = 3,
        List = 4
    }

    public enum eMIDTextOrderBy
    {
        TextCode = 0,
        TextValue = 1
    }

    public enum eDataOrderBy
    {
        RID = 0,
        ID = 1
    }

    public enum eDataState
    {
        New = 0,
        ReadOnly = 1,
        Updatable = 2,
        None = 3
    }

    public enum ePurpose
    {
        Default = 0,
        Placeholder = 1,
        Connector = 2,
        Assortment = 3
    }


    // Enums 800000-899999
    // must match code in text load 
    public enum eHierarchyType
    {
        // Begin Track #5005 - JSmith - Explorer Organization
        None = 0,
        // End Track #5005
        organizational = 800000,
        alternate = 800001
    }

    public enum eMerchandiseType
    {
        HierarchyLevel = 0,
        Node = 1,
        OTSPlanLevel = 2,
        Undefined = 3,
        LevelOffset = 4,
        SameNode = 5
    }

    public enum eLevelLengthType
    {
        unrestricted = 800100,
        required = 800101,
        range = 800102
    }

    // BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement
    public enum eForecastFormulaType
    {
        Stock = 800661,
        Sales = 800662,
        PctContribution = 800663
    }
    // END MID Track #5773

    public enum eHierarchyLevelMasterType
    {
        Undefined = 800200,
        //Begin Track #3863 - JScott - OTS Forecast Level Defaults
        //		Planlevel				= 800201,
        //End Track #3863 - JScott - OTS Forecast Level Defaults
        Style = 800202,
        Color = 800203,
        Size = 800204,
        ParentOfStyle = 800205
    }

    public enum eHierarchyLevelType
    {
        Undefined = 800200,
        //Begin Track #3863 - JScott - OTS Forecast Level Defaults
        //		Planlevel				= 800201,
        //End Track #3863 - JScott - OTS Forecast Level Defaults
        Style = 800202,
        Color = 800203,
        Size = 800204
    }

    //Begin Track #4037 - JSmith - Optionally include dummy color in child list
    public enum eNodeSelectType
    {
        All,
        NoVirtual,
        VirtualOnly,
        Connectors
    }
    //End Track #4037

    // Begin Track #4872 - JSmith - Global/User Attributes
    public enum eStoreGroupSelectType
    {
        All,
        GlobalOnly,
        MyUserOnly,
        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        MySharedOnly,
        MyUserAndGlobal,
        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        MyUserAndShared
    }
    // End Track #4872

    //Begin - JScott - Add Rollup Type to Hierarchy Properties - Part 1
    public enum eHierarchyRollupOption
    {
        Undefined = 800250,
        RealTime = 800251,
        API = 800252,
    }

    //End - JScott - Add Rollup Type to Hierarchy Properties - Part 1
    public enum eProductType
    {
        Undefined = 800300,
        Hardline = 800301,
        Softline = 800302
    }

    public enum eHierarchyDescendantType
    {
        offset = 0,
        masterType = 1,
        levelType = 2
    }

    //Begin TT#155 - JScott - Add Size Curve info to Node Properties
    // Do not change the order of the values in this enumeration.  They are used on the database.
    public enum eSizeCurveGenerateType
    {
        None,
        Method,
        Node
    }

    //End TT#155 - JScott - Add Size Curve info to Node Properties
    //Begin TT#1076 - JScott - Size Curves by Set
    public enum eSizeCurvesByType
    {
        None = 801200,
        Store = 801201,
        AttributeSet = 801202
    }

    //End TT#1076 - JScott - Size Curves by Set
    public enum eProcesses
    {
        unknown = 800400,
        hierarchyLoad = 800401,
        storeLoad = 800402,
        historyPlanLoad = 800403,
        clientApplication = 800404,
        storeGroupBuilder = 800405,
        hierarchyWebService = 800406,
        colorCodeLoad = 800407,
        sizeCodeLoad = 800408,
        headerLoad = 800409,
        forecasting = 800410,
        rollup = 800411,
        controlService = 800412,
        applicationService = 800413,
        hierarchyService = 800414,
        storeService = 800415,
        relieveIntransit = 800416,
        purge = 800417,
        allocate = 800418,
        schedulerService = 800419,
        executeJob = 800420,
        headerService = 800421,
        sizeCurveLoad = 800422,
        //BEGIN TT#1435-VStuart-Attempt to Create Duplicate Size Curve Load Task List Causes Database Unique Index
        //sqlScript             = 800422,
        sqlScript = 800423,
        //computationsLoad		= 800423,
        //END TT#1435-VStuart-Attempt to Create Duplicate Size Curve Load Task List Causes Database Unique Index
        forecastBalancing = 800424,
        databaseConversionUtility = 800425,
        reBuildIntransit = 800426,  // TT#1137 (MID Track 4351) Rebuild Intransit Utility
                                    //Begin - Abercrombie & Fitch #4411 - JSmith - Chain Forecasting
                                    //		sizeConstraintsLoad	    = 800427
        sizeConstraintsLoad = 800427,
        //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
        //computationDriver = 800428
        computationDriver = 800428,
        //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
        //End - Abercrombie & Fitch #4411
        //End - Abercrombie & Fitch #4411
        // BEGIN Issue 5117 stodd 4.14.2008 bonton special request
        specialRequest = 800429,
        // END Issue 5117 stodd 4.14.2008 bonton special request
        forecastExportThread = 800430,
        // Begin TT#155 - JSmith - Size Curve Method
        assortmentAPI = 800431,
        sizeCurveGenerate = 800432,
        // End TT#155 - JSmith - Size Curve Method
        // Begin TT#391 - STodd - Size day to week aummary
        SizeDayToWeekSummary = 800435,
        //Begin MOD - JScott - Build Pack Criteria Load
        buildPackCriteriaLoad = 800436,
        //End MOD - JScott - Build Pack Criteria Load
        StoreBinViewer = 800437,
        // Begin TT#391 - STodd - Size day to week aummary
        // Begin TT#710 - JSmith - Generate relieve intransit
        generateRelieveIntransit = 800439,
        // End TT#710
        //Begin TT#707 - JScott - Size Curve process needs to multi-thread
        sizeCurveGenerateThread = 800438,
        //End TT#707 - JScott - Size Curve process needs to multi-thread
        //Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
        determineHierarchyActivity = 800440,
        //End TT#988
        //Begin TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2
        ChainSetPercentCriteriaLoad = 800441,
        //End TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2
        //Begin TT#1312 - JScott - Alternate Hierarchy Reclass
        hierarchyReclass = 800442,
        //End TT#1312 - JScott - Alternate Hierarchy Reclass
        // BEGIN TT#1401 - stodd - add resevation stores (IMO)
        pushToBackStock = 800444,
        // End TT#1401 - stodd - add resevation stores (IMO)
        // Begin TT#2249 - JSmith - Wrong name for process
        headerAllocationLoad = 800445,
        // End TT#2249
        //BEGIN TT#43 – MD – DOConnell – Projected Sales Enhancement
        DailyPercentagesCriteraLoad = 800446,
        //END TT#43 – MD – DOConnell – Projected Sales Enhancement
        // Begin TT#240 MD - JSmith - Change Scheduler Interface to have its own audit entry and not report as Scheduler Service
        scheduleInterface = 800447,
        // End TT#240 MD
        convertFilters = 800448, //TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)
        storeDelete = 800449,
        StoreEligibilityCriteraLoad = 800450, //TT#816 -MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
        VSWCriteriaLoad = 800451, //TT#817 -MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - VSW Load API
        HeaderReconcile = 800452,		// TT#1581-MD - stodd - API Header Reconcile
        //BEGIN TT#1644-VSuart-Process Control-MID
        AllocationTasklist = 800453,	// TT#1581-MD - stodd - API Header Reconcile
        //END TT#1644-VSuart-Process Control-MID
        BatchComp = 800454,				// TT#1595-MD - stodd - Batch Comp
        SchedulerJobManager = 800455,
        planningExtractThread = 800456,  // TT#2131-MD - JSmith - Halo Integration
    }

    public enum eTaskType
    {
        None = -1,
        HierarchyLoad = 800401,
        StoreLoad = 800402,
        HistoryPlanLoad = 800403,
        ColorCodeLoad = 800407,
        SizeCodeLoad = 800408,
        HeaderLoad = 800409,
        Forecasting = 800410,
        Rollup = 800411,
        RelieveIntransit = 800416,
        Purge = 800417,
        Allocate = 800418,
        ExternalProgram = 800420,
        SizeCurveLoad = 800422,
        //Begin Track #4382 - JSmith - Add Size Constraint Load to scheduler
        //		SQLScript				= 800423
        SQLScript = 800423,
        //Begin - Abercrombie & Fitch #4411 - JSmith - Chain Forecasting
        //		SizeConstraintsLoad		= 800427
        SizeConstraintsLoad = 800427,
        //End Track #4382
        computationDriver = 800428,
        //End - Abercrombie & Fitch #4411
        // Begin TT#155 - JSmith - Size Curve Method
        SizeCurveMethod = 800433,
        //Begin TT#155 - JScott - Add Size Curve info to Node Properties
        //SizeCurve				= 800434,
        SizeCurves = 800434,
        //End TT#155 - JScott - Add Size Curve info to Node Properties
        // End TT#155 - JSmith - Size Curve Method
        // Begin TT#391 - STodd - Size day to week aummary
        SizeDayToWeekSummary = 800435,
        // Begin TT#391 - STodd - Size day to week aummary
        //Begin MOD - JScott - Build Pack Criteria Load
        BuildPackCriteriaLoad = 800436,
        //End MOD - JScott - Build Pack Criteria Load
        // BEGIN TT#1501 - AGallagher - Chain Plan - Set Percentages - Phase 4
        ChainSetPercentCriteriaLoad = 800441,
        // END TT#1501 - AGallagher - Chain Plan - Set Percentages - Phase 4
        // BEGIN TT#1401 - AGallagher - VSW
        PushToBackStockLoad = 800444,
        // END TT#1401 - AGallagher - VSW
        //BEGIN TT#43 – MD – DOConnell – Projected Sales Enhancement
        DailyPercentagesCriteriaLoad = 800446, //TT#816 - MD - DOConnell - corrected misspelling
        //END TT#43 – MD – DOConnell – Projected Sales Enhancement
        //BEGIN TT#820 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Add Task List Options for VSW and Store Eligibility Load processes
        StoreEligibilityCriteriaLoad = 800450,
        VSWCriteriaLoad = 800451,
        //END TT#820 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Add Task List Options for VSW and Store Eligibility Load processes
        //Begin Track #4010 - JScott - Add all forecast methods and workflows to the scheduler.
        //		ForecastBalance			= 800424
        //End Track #4010 - JScott - Add all forecast methods and workflows to the scheduler.
        HeaderReconcile = 800452,       // TT#1581-MD - stodd - API Header Reconcile
        BatchComp = 800454,     // TT#1595-MD - stodd - batch comp
    }

    //	public enum eProcessStatus
    //	{
    //		none					= 800500,
    //		completed				= 800501,
    //		running					= 800502,
    //		failed					= 800503,
    //		scheduled				= 800504
    //	}

    public enum eProcessCompletionStatus
    {
        None = 800500,
        Successful = 800501,
        Failed = 800503,
        ConditionFailed = 800504,
        Cancelled = 800505,
        Unexpected = 800506
    }

    public enum eOTSPlanLevelType
    {
        Undefined = 800600,
        Regular = 800601,
        Total = 800602
    }

    public enum eStoreDisplayOptions
    {
        NameOnly = 800650,
        IdOnly = 800651,
        DescriptionOnly = 800652,
        IdAndName = 800653,
        IdAndDescription = 800654,
        NameAndDescription = 800655,
        IdAndNameAndDesc = 800656
    }

    public enum eExplorerDisplayOptions
    {
        NameOnly = 800693,
        IdOnly = 800694,
        DescriptionOnly = 800695,
        IdAndName = 800696,
        IdAndDescription = 800697,
        NameAndDescription = 800698,
        IdAndNameAndDesc = 800699
    }

    public enum eHierarchyDisplayOptions
    {
        NameOnly = 800700,
        IdOnly = 800701,
        DescriptionOnly = 800702,
        IdAndName = 800703,
        IdAndDescription = 800704,
        NameAndDescription = 800705,
        IdAndNameAndDesc = 800706,
        DoNotDisplay = 800707
    }

    public enum eSortDirection
    {
        Ascending = 800710,
        Descending = 800711,
        None = 800712
    }

    public enum eMaskField
    {
        Undefined = 0,
        Name = 1,
        Id = 2,
        Description = 3
    }

    public enum eStorePlanSelectedGroupBy
    {
        ByTimePeriod = 800720,
        ByVariable = 800721
    }
    public enum eDisplayTimeBy
    {
        ByWeek = 800851,
        ByPeriod = 800853
    }
    public enum ePlanSessionType
    {
        //Begin Track #6251 - JScott - Get System Null Ref Excp using filter
        None = 0,
        //End Track #6251 - JScott - Get System Null Ref Excp using filter
        StoreSingleLevel = 1,
        StoreMultiLevel = 2,
        ChainSingleLevel = 3,
        ChainMultiLevel = 4
    }
    public enum eHighLevelsType
    {
        None = 0,
        HierarchyLevel = 1,
        Characteristic = 2,
        LevelOffset = 3
    }
    public enum eLowLevelsType
    {
        None = 0,
        HierarchyLevel = 1,
        Characteristic = 2,
        LevelOffset = 3
    }
    public enum eFromLevelsType
    {
        None = 0,
        HierarchyLevel = 1,
        Characteristic = 2,
        LevelOffset = 3
    }
    public enum eToLevelsType
    {
        None = 0,
        HierarchyLevel = 1,
        Characteristic = 2,
        LevelOffset = 3
    }
    public enum eMinMaxInheritType
    {
        None = 0,
        Hierarchy = 1,
        // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
        //Method				= 2
        Method = 2,
        Default = 3
        // End TT#3
    }

    public enum ePlanLevelSelectType
    {
        Undefined = 0,
        HierarchyLevel = 1,
        Node = 2
    }
    public enum ePlanLevelLevelType
    {
        Undefined = 0,
        HierarchyLevel = 1,
        LevelOffset = 2
    }
    public enum eBalanceMode
    {
        Chain = 0,
        Store = 1
    }
    // BEGIN MID Track #5647 - KJohnson - Matrix Forecast
    public enum eMatrixType
    {
        Balance = 0,
        Forecast = 1
    }
    // END MID Track #5647
    public enum eIterationType
    {
        UseBase = 0,
        Custom = 1
    }

    // Begin TT#1077 - MD - stodd - cannot create GA views 
    public enum eAssortmentViewType
    {
        Assortment = 1,
        GroupAllocation = 2
    }
    // End TT#1077 - MD - stodd - cannot create GA views 

    public enum eHeaderCharType
    {
        text = 0,
        date = 1,
        number = 2,
        dollar = 3,
        unknown = 4,
        list = 5,
        boolean = 6
    }
    //Begin Track #4457 - JSmith - Add forecast versions
    public enum eForecastBlendType
    {
        None = 802830,
        Week = 802831,
        Month = 802832
    }
    //End Track #4457

    // BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
    public enum eFillSizesToType
    {
        Holes = 802850,
        SizePlan = 802851,
        SizePlanWithMins = 802852 //TT#848-MD -jsobek -Fill to Size Plan Presentation
    }
    // END MID Track #4921

    // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
    // Do not change, values used on database
    public enum eGenericSizeCurveNameType
    {
        NodePropertiesName = 0,
        HeaderCharacteristic = 1
    }
    // END TT#413

    // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
    public enum ePackRounding
    {
        Down = 900672,
        Up = 900673
    }
    // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)

    // begin TT#1185 - Verify ENQ before Update
    /// <summary>
    /// Enumeration of Enqueue Status
    /// </summary>
    /// <remarks>Enqueued: Enqueue was created by at a specified time by a given user on a given thread in a given transaction</remarks>
    /// <remarks>TentativelyEnqueued: Enqueue exists for a given user on a given thread in a given transaction</remarks>
    /// <remarks>NotEnqueued: No Enqueue exists</remarks>
    /// <remaarks>EnqueueConflict: An enqueue exists but for a different user, thread, transaction or time</remaarks>
    public enum eEnqueueStatus
    {
        Enqueued = 0,
        TentativelyEnqueued = 1,
        NotEnqueued = 2,
        EnqueueConflict = 3
    }
    // end TT#1185 - Verify ENQ before Update 



    /// <summary>
    /// Used to communicate the type of a header
    /// </summary>
    /// <remarks>
    /// Be sure to also maintain eSizeHeaderType and eNonSizeHeaderType
    /// </remarks>
    public enum eHeaderType
    {
        Receipt = 800730,
        ASN = 800731,
        Dummy = 800732,
        DropShip = 800733,
        MultiHeader = 800734,
        Reserve = 800735,
        //WorkupSizeBuy	= 800736,  // MID Track 3891 Remove WorkUpSizeBuy (now part of WorkUpTotalBuy)
        WorkupTotalBuy = 800737,
        PurchaseOrder = 800738,
        Assortment = 800739,  // New for Assortment
        Placeholder = 800740,
        IMO = 800741,  // TT#1401 - JEllis - Urban Reservation Stores pt 2
        Master = 800742,  // TT#1966-MD - JSmith - DC Fulfillment 
    }

    // begin MID Track 3891 Remove WorkUpSizeBuy (now part of WorkUpTotalBuy)
    //public enum eSizeHeaderType
    //{
    //	WorkupSizeBuy	= 800736
    //}
    // end MID Track 3891 Remove WorkUpSizeBuy (now part of WorkUpTotalBuy)

    public enum eNonSizeHeaderType
    {
        Receipt = 800730,
        ASN = 800731,
        Dummy = 800732,
        DropShip = 800733,
        MultiHeader = 800734,
        Reserve = 800735,
        WorkupTotalBuy = 800737,
        PurchaseOrder = 800738,  // TT#1401 - JEllis - Urban Reservation Stores pt 2
        IMO = 800741,  // TT#1401 - JEllis - Urban Reservation Stores pt 2
    }

    /// <summary>
    /// Used to communicate the allocation status of a header
    /// </summary>
    /// <remarks>
    /// Be sure to also maintain eSizeHeaderAllocationStatus and eNonSizeHeaderAllocationStatus
    /// </remarks>
    public enum eHeaderAllocationStatus
    {
        ReceivedOutOfBalance = 802700,
        ReceivedInBalance = 802701,
        InUseByMultiHeader = 802702,
        PartialSizeOutOfBalance = 802703,
        PartialSizeInBalance = 802704,
        AllocatedOutOfBalance = 802705,
        AllocatedInBalance = 802706,
        SizesOutOfBalance = 802707,
        AllInBalance = 802708,
        Released = 802709,
        ReleaseApproved = 802710,  // MID Tracks 3811, 3827 Status problems (added new status)
        AllocationStarted = 802711 // MID Tracks 3811, 3827 Status problems (added new status)
    }

    /// <summary>
    /// Used to communicate the allocation status of a header for size
    /// </summary>
    public enum eSizeHeaderAllocationStatus
    {
        PartialSizeOutOfBalance = 802703,
        PartialSizeInBalance = 802704,
        SizesOutOfBalance = 802707
    }

    /// <summary>
    /// Used to communicate the allocation status of a non-size header
    /// </summary>
    public enum eNonSizeHeaderAllocationStatus
    {
        ReceivedOutOfBalance = 802700,
        ReceivedInBalance = 802701,
        InUseByMultiHeader = 802702,
        AllocatedOutOfBalance = 802705,
        AllocatedInBalance = 802706,
        AllInBalance = 802708,
        Released = 802709,
        ReleaseApproved = 802710, // MID Tracks 3811, 3827 Status problems (added new status)
        AllocationStarted = 802711 // MID Tracks 3811, 3827 Status problmes (added new status)
    }

    /// <summary>
    /// Used to communicate the ship status of a header and its allocation
    /// </summary>
    public enum eHeaderShipStatus
    {
        NotShipped = 802730,
        Partial = 802731,
        OnHold = 802732,
        Shipped = 802733
    }

    /// <summary>
    /// Used to communicate the intransit status of a header.
    /// </summary>
    public enum eHeaderIntransitStatus
    {
        NotIntransit = 802750,
        IntransitBySKU = 802751,
        IntransitByStyle = 802752,
        IntransitByBulkSize = 802753
    }

    public enum eAllocationUpdateStatus
    {
        Successful = 0,
        Failed_BulkSizeIntransitIsUpdated = 1,
        Failed_StyleIntransitIsUpdated = 2,
        Failed_ColorNotOnHeader = 3,
        Failed_SizeNotInColor = 4,
        Failed_StoreLocked = 5,
        Failed_StoreTempLock = 6,
        Failed_SpreadFailed = 7,
        Failed_NotAllUnitsSpread = 8,  // TT#1773-MD - JSmith - GA -> Velocity-> Qty allocated column-> attempt to make manual change and get unhandled exception
        Failed_Other = 999
    }

    //	public enum ePackType
    //	{
    //		Generic			= 800745,
    //		Detail			= 800746
    //	}

    // Component enums
    public enum eComponentType
    {
        Total = 800750,
        GenericType = 800751,
        DetailType = 800752,
        AllPacks = 800753,
        AllGenericPacks = 800754,
        AllNonGenericPacks = 800755,
        SpecificPack = 800756,
        Bulk = 800757,
        AllColors = 800758,
        SpecificColor = 800759,
        AllSizes = 800760,
        SpecificSize = 800761,
        ColorAndSize = 800762,
        MatchingPack = 800763,
        MatchingColor = 800764,
        SpecificSizePrimaryDim = 800765,
        SpecificSizeSecondaryDim = 800766,
        DetailSubType = 800767
    }

    public enum eGeneralComponentType
    {
        Total = 800750,
        GenericType = 800751,
        DetailType = 800752,
        AllPacks = 800753,
        AllGenericPacks = 800754,
        AllNonGenericPacks = 800755,
        Bulk = 800757,
        AllColors = 800758,
        AllSizes = 800760,
        DetailSubType = 800767
    }

    public enum eRuleMethodComponentType
    {
        Total = 800750,
        SpecificPack = 800756,
        Bulk = 800757,
        SpecificColor = 800759,
        MatchingPack = 800763,
        MatchingColor = 800764
    }

    public enum eRuleMethodPackComponentType
    {
        SpecificPack = 800756,
        MatchingPack = 800763
    }

    public enum eRuleMethodColorComponentType
    {
        SpecificColor = 800759,
        MatchingColor = 800764
    }

    public enum eVelocityMethodComponentType
    {
        Total = 800750,
        Bulk = 800757
    }
    //Begin TT#855-MD -jsobek -Velocity Enhancements 
    public enum eVelocityMethodGradeVariableType
    {
        Sales = 848200,
        Stock = 848201
    }
    //End TT#855-MD -jsobek -Velocity Enhancements 


    // Component Category
    public enum eComponentCategory
    {
        General = 1,
        PackSpecific = 2,
        ColorOrSizeSpecific = 3,
        ColorAndSize = 4
    }

    // enum to distinguish bulk types.
    public enum eSpecificBulkType
    {
        SpecificColor = 800759,
        SpecificSize = 800761,
        SpecificSizePrimaryDim = 800765,
        SpecificSizeSecondaryDim = 800766
    }

    // enum to components that require specific details.
    public enum eSpecificComponentType
    {
        SpecificPack = 800756,
        SpecificColor = 800759,
        SpecificSize = 800761
    }

    //    // OTS Plan Method enums
    //	public enum eMethodType
    //	{
    //		OTSPlan						= 800800,
    //		ForecastBalance				= 800801,
    //		GeneralAllocation			= 800802,
    //		AllocationOverride			= 800803,
    //		Velocity					= 800804,
    //		Rule						= 800805,
    //		FillSizeHolesAllocation		= 800806,
    //		BasisSizeAllocation			= 800807,
    //		WarehouseSizeAllocation		= 800808,
    //		SizeNeedAllocation			= 800809,
    //		SizeOverrideAllocation		= 800810
    //	}

    public enum eGroupLevelFunctionType
    {
        PercentContribution = 800811,
        TyLyTrend = 800812,
        AverageSales = 800813,
        CurrentTrend = 800814
    }

    public enum eGroupLevelSmoothBy
    {
        StoreSet = 800820,
        StoreGrade = 800821,
        Both = 800822,
        None = 800823
    }

    //Begin TT#1517-MD -jsobek -Store Service Optimization
    public enum eGroupLevelTypes
    {
        Normal = 0,
        AvailableStoreSet = 1,
        DynamicSet = 2
    }
    //End TT#1517-MD -jsobek -Store Service Optimization

    //Begin Track #4010 - JScott - Add all forecast methods and workflows to the scheduler.
    public enum eWorkflowMethodType
    {
        None = 801700,
        Method = 801701,
        Workflow = 801702
    }

    //End Track #4010 - JScott - Add all forecast methods and workflows to the scheduler.

    // Begin TT#391 - stodd - adding In Stock Sales to global options
    public enum eGenerateSizeCurveUsing
    {
        Sales = 801720,
        InStockSales = 801721,
    }
    // End TT#391 - stodd - adding In Stock Sales to global options

    public enum eWorkflowType
    {
        None = 0,
        Forecast = 800825,
        Allocation = 800826,
        Assortment = 800827         // Stodd Assortment
    }

    public enum eWorkflowProcessOrder
    {
        AllStepsForHeader = 0,  // this option will process all steps for a header before going to the next header
        AllHeadersForStep = 1       // this option will process all headers for a stpe before going to the next step
    }

    public enum eGlobalUserType
    {
        User = 800830,
        Global = 800831
    }

    public enum eWorkflowMethodIND
    {
        Methods = 800835,
        Workflows = 800836,
        SizeMethods = 800837
    }

    public enum eTyLyType
    {
        NonTyLy = 800840,
        TyLy = 800841,
        AlternateLy = 800842,
        AlternateApplyTo = 800843,
        //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
        ProjectCurrWkSales = 800844
        //END TT#43 - MD - DOConnell - Projected Sales Enhancement
    }

    // begin MID Track 3619 Remove Fringe
    //public enum eFringeFilterValueType				
    //{
    //	Units							= 800848,
    //	Percent							= 800849
    //}
    // end MID Track 3619 Remove Fringe

    public enum eTrendCapID
    {
        None = 800870,
        Tolerance = 800871,
        Limits = 800872,
    }

    // Rule enums
    public enum eRuleMethod
    {
        None = 802400,
        Out = 802401,
        Quantity = 802402,
        StockMinimum = 802403,
        StockMaximum = 802404,
        ColorMinimum = 802405,
        ColorMaximum = 802406,
        AdMinimum = 802407,
        Exact = 802408,
        Fill = 802409,
        Proportional = 802410
    }

    public enum eRuleMethodRequiresQuantity
    {
        Quantity = 802402
    }

    // begin MID Track 4442 Min, Ad Min and Max only valid for Total Component
    /// <summary>
    /// Rules that are only valid on a total component (see also eRuleTypeOnlyValidOnTotalComponent)
    /// </summary>
    public enum eRuleMethodOnlyValidOnTotalComponent
    {
        StockMinimum = 802403,
        StockMaximum = 802404,
        AdMinimum = 802407
    }
    // end MID Track 4442 Min, Ad Min and Max only valid for Total Component
    // Calendar enums
    public enum eCalendarDateType
    {
        Day = 800850,
        Week = 800851,
        Period = 800853,
        Year = 800860
    }

    public enum eWeek53Offset
    {
        Not53WeekYear = 800853,
        DropWeek53 = 800854,
        Offset1Week = 800855
    }

    public enum eCalendarRangeType
    {
        Static = 800857,
        Dynamic = 800858,
        Reoccurring = 800859,
        DynamicSwitch = 800861

    }

    // BEGIN TT#2225 - stodd - VSW ANF Enhancement (IMO)	
    public enum eAdjustVSW
    {
        Replace = 800865,
        Adjust = 800866
    }
    // END TT#2225 - stodd - VSW ANF Enhancement (IMO)	

    /// <summary>
    /// Used to define the names of the default 4-5-4 periods on the calendar.
    /// </summary>
    public enum ePeriodName
    {
        Period1 = 800873,
        Period2 = 800874,
        Period3 = 800875,
        Period4 = 800876,
        Period5 = 800877,
        Period6 = 800878,
        Period7 = 800879,
        Period8 = 800880,
        Period9 = 800881,
        Period10 = 800882,
        Period11 = 800883,
        Period12 = 800884
    }

    /// <summary>
    /// Used to define the abbreviation of the default 4-5-4 periods on the calendar.
    /// </summary>
    public enum ePeriodAbbreviation
    {
        Period1 = 800886,
        Period2 = 800887,
        Period3 = 800888,
        Period4 = 800889,
        Period5 = 800890,
        Period6 = 800891,
        Period7 = 800892,
        Period8 = 800893,
        Period9 = 800894,
        Period10 = 800895,
        Period11 = 800896,
        Period12 = 800897
    }

    public enum eCalendarModelPeriodType
    {
        None = 800905,
        Month = 800906,
        Quarter = 800907,
        Season = 800908,
        Year = 800909
    }

    /// <summary>
    /// Used to define the names of the default Quarters on the calendar.
    /// </summary>
    public enum eQuarterName
    {
        Quarter1 = 800911,
        Quarter2 = 800912,
        Quarter3 = 800913,
        Quarter4 = 800914
    }

    /// <summary>
    /// Used to define the abbreviation of the default Quarters on the calendar.
    /// </summary>
    public enum eQuarterAbbreviation
    {
        QuarterAbbrev1 = 800915,
        QuarterAbbrev2 = 800916,
        QuarterAbbrev3 = 800917,
        QuarterAbbrev4 = 800918
    }

    /// <summary>
    /// Used to define the names of the default Quarters on the calendar.
    /// </summary>
    public enum eSeasonName
    {
        Season1 = 800919,
        Season2 = 800920
    }

    /// <summary>
    /// Used to define the abbreviation of the default Quarters on the calendar.
    /// </summary>
    public enum eSeasonAbbreviation
    {
        SeasonAbbrev1 = 800921,
        SeasonAbbrev2 = 800922
    }

    // Security enums 801000 - 801999
    public enum eSecurityLevel
    {
        NotSpecified = 0,
        //Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
        //		Initialize					= 1,
        PartialAllow = 1,
        //End Track #5091 - JScott - Secuirty Lights don't change when permission changes
        //		NoAccess					= 801001,
        //		ReadOnlyAccess				= 801002,
        //		FullAccess					= 801003,
        Allow = 801004,
        Deny = 801005
    }

    public enum eValidUsername
    {
        Valid = 801010,
        InvalidTooShort = 801011,
        InvalidIllegalChar = 801012,
        InvalidAlreadyExists = 801013
    }

    public enum eValidPassword
    {
        Valid = 801020,
        InvalidTooShort = 801021,
        InvalidIllegalChar = 801022
    }

    public enum eValidGroupname
    {
        Valid = 801030,
        InvalidTooShort = 801031,
        InvalidIllegalChar = 801032,
        InvalidAlreadyExists = 801033
    }

    public enum eSessionStatus
    {
        //Begin TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
        //Unknown = 802830,
        //LoggedIn = 802831,
        //LoggedOut = 802832
        Unknown = 802860,
        LoggedIn = 802861,
        LoggedOut = 802862
        //End TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
    }

    public enum eSecurityActivation
    {
        Activate,
        Deactivate,
        NoChange
    }

    public enum eSecurityAuthenticate
    {
        IncorrectPassword = 801150,
        UnknownUser = 801151,
        InactiveUser = 801152,
        ActiveUser = 801153,
        UserAuthenticated = 801154,
        PasswordChanged = 801155,
        Unavailable = 801156    // TT#1581-MD - stodd Header Reconcile
    }

    public enum eSecurityActions
    {
        NotSpecified = 0,
        FullControl = 801160,
        Maintain = 801161,
        Delete = 801162,
        View = 801163,
        Execute = 801164,
        Move = 801165,
        //AddSets					= 801166,
        Inactivate = 801167,
        // Begin Track #4961 - JSmith - Add security for apply to lower levels.  
        //Begin Track #4815 - JSmith - #283-User (Security) Maintenance
        //		Interactive					= 801168
        Interactive = 801168,
        ApplyToLowerLevels = 801169,
        InheritFromHigherLevel = 801170,
        // End Track #4961 
        // Begin TT#2015 - JSmith - Apply Changes to Lower Level
        //Assign                      = 801171
        Assign = 801171,
        //End Track #4815
        ApplyChangesToLowerLevels = 801173,
        // End TT#2015
    }

    public enum eSecurityOwnerType
    {
        NotSpecified = 0,
        Group = 802820,
        User = 802821
    }

    public enum eSecurityInheritanceTypes
    {
        NotSpecified = 0,
        HierarchyNode = 1,
        Function = 2,
        Group = 3,
        Version = 4
    }

    public enum eDatabaseSecurityTypes
    {
        NotSpecified = 0,
        Allocation = 802800,
        Chain = 802801,
        Store = 802802
    }

    public enum eSecurityTypes
    {
        None = 0x0000,
        All = 0xFFFF,
        Allocation = 0x0001,
        Chain = 0x0002,
        Store = 0x0004
    }

    public enum eVersionSecurityActions
    {
        NotSpecified = 0,
        FullControl = 801160,
        Maintain = 801161,
        Delete = 801162,
        View = 801163
    }

    public enum eNodeSecurityActions
    {
        NotSpecified = 0,
        FullControl = 801160,
        Maintain = 801161,
        Delete = 801162,
        View = 801163
    }

    public enum eSecurityFunctionTypes
    {
        ApplicationBase = 0,
        Size = 1,
        Assortment = 2,
        GroupAllocation = 3     // TT#1247-MD - stodd - Add Group Allocation as a License Key option
    }

    public enum eSecurityFunctions
    {
        NotSpecified = 0,
        Admin = 810000,
        AdminBatchOnlyMode = 810050, //TT#901-MD -jsobek -Batch Only Mode
        AdminCalendar = 810100,
        AdminCalendarDefine = 810110,
        AdminCalendarRange = 810120,
        AdminHeaders = 810200,
        AdminHeadersCharacteristics = 810210,
        //Begin Track #3785 - JScott - Remove Administration->Colors and Administration->Size->Size Definition securities.
        //		AdminColors								= 810300,
        //End Track #3785 - JScott - Remove Administration->Colors and Administration->Size->Size Definition securities.
        AdminGlobalOptions = 810400,
        AdminGlobalOptionsCompanyInfo = 810410,
        AdminGlobalOptionsDisplay = 810420,
        AdminGlobalOptionsOTSVersions = 810430,
        AdminGlobalOptionsAlDefaults = 810440,
        //Begin Track #3784 - JScott - Add security for Header Gloabl Options
        AdminGlobalOptionsAlHeaders = 810450,
        //End Track #3784 - JScott - Add security for Header Gloabl Options
        //Begin Track #6240 - stodd - Add security for basis labels and OTS Defaults
        AdminGlobalOptionsBasisLabels = 810460,
        AdminGlobalOptionsOTSDefaults = 810470,
        //End Track #6240 - stodd - Add security for basis labels and OTS Defaults
        AdminHierarchies = 810500,
        AdminHierarchiesAlt = 810510,
        AdminHierarchiesAltGlobal = 810520,
        AdminHierarchiesAltGlobalProperty = 810525,
        AdminHierarchiesAltGlobalNodeProperty = 810530,
        AdminHierarchiesAltGlobalNodePropertyCapacity = 810531,
        AdminHierarchiesAltGlobalNodePropertyDailyPcts = 810532,
        AdminHierarchiesAltGlobalNodePropertyEligibility = 810533,
        AdminHierarchiesAltGlobalNodePropertyProfile = 810534,
        AdminHierarchiesAltGlobalNodePropertyPurge = 810535,
        AdminHierarchiesAltGlobalNodePropertyStoreGrades = 810536,
        AdminHierarchiesAltGlobalNodePropertyVelocity = 810537,
        AdminHierarchiesAltGlobalNodePropertyStockMinMax = 810538,
        AdminHierarchiesAltGlobalNodePropertyCharacteristic = 810539,
        AdminHierarchiesAltNodes = 810540,
        //Begin TT#155 - JScott - Add Size Curve info to Node Properties
        AdminHierarchiesAltGlobalNodePropertySizeCurves = 810541,
        //End TT#155 - JScott - Add Size Curve info to Node Properties
        //Begin TT#1498 - DOConnell - Chain Plan - Set Percentages Phase 3
        AdminHierarchiesAltGlobalNodePropertyChainSetPcts = 810542,
        //End TT#1498 - DOConnell - Chain Plan - Set Percentages Phase 3
        //Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
        AdminHierarchiesAltGlobalNodePropertySizeOutOfStock = 810544,
        //End TT#483 - JScott - Add Size Lost Sales criteria and processing
        //Begin TT#2015 - gtaylor - apply changes to lower levels
        AdminHierarchiesAltGlobalNodePropertyVSW = 810543,
        //End TT#2015 - gtayor - apply changes to lower levels
        AdminHierarchiesAltUser = 810550,
        AdminHierarchiesOrg = 810560,
        AdminHierarchiesOrgProperty = 810565,
        AdminHierarchiesOrgNodeProperty = 810570,
        AdminHierarchiesOrgNodePropertyCapacity = 810571,
        AdminHierarchiesOrgNodePropertyDailyPcts = 810572,
        AdminHierarchiesOrgNodePropertyEligibility = 810573,
        AdminHierarchiesOrgNodePropertyProfile = 810574,
        AdminHierarchiesOrgNodePropertyPurge = 810575,
        AdminHierarchiesOrgNodePropertyStoreGrades = 810576,
        AdminHierarchiesOrgNodePropertyVelocity = 810577,
        AdminHierarchiesOrgNodePropertyStockMinMax = 810578,
        AdminHierarchiesOrgNodePropertyCharacteristic = 810579,
        AdminHierarchiesOrgNodes = 810580,
        //Begin TT#155 - JScott - Add Size Curve info to Node Properties
        AdminHierarchiesOrgNodePropertySizeCurves = 810581,
        //End TT#155 - JScott - Add Size Curve info to Node Properties
        //Begin TT#1498 - DOConnell - Chain Plan - Set Percentages Phase 3
        AdminHierarchiesOrgNodePropertyChainSetPcts = 810582,
        //End TT#1498 - DOConnell - Chain Plan - Set Percentages Phase 3
        //Begin TT#2015 - gtaylor - apply changes to lower levels
        AdminHierarchiesOrgNodePropertyVSW = 810583,
        //End TT#2015 - gtaylor - apply changes to lower levels
        //Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
        AdminHierarchiesOrgNodePropertySizeOutOfStock = 810584,
        //End TT#483 - JScott - Add Size Lost Sales criteria and processing
        //Begin Product Characteristics - JSmith
        AdminHierarchiesCharacteristics = 810590,
        //End Product Characteristics

        AdminModels = 810600,
        AdminModelsEligibility = 810610,
        AdminModelsSalesModifier = 810620,
        AdminModelsStockModifier = 810630,
        //Begin TT#108 - MD - DOConnell - FWOS Max Model Enhancement
        //	AdminModelsFWOSModifier					= 810660, // TT#203 - MD - Remove duplicate Enums entry 810660 - rbeck
        AdminModelsForecasting = 810640,
        AdminModelsForecastBalance = 810650,
        AdminModelsFWOSModifier = 810660,  // TT#203 - MD - Remove duplicate Enums entry 810660 - rbeck
        //Begin TT#2015 - gtaylor - apply changes to lower levels        
        // AdminModelsVSW     = 810660,   // TT#203 - MD - Remove duplicate Enums entry 810660 - rbeck
        // Begin TT#432-MD - JSmith - Login into new database with new user and received "Index was outside the bounds of the array" error
        //AdminModelsVSW                          = 810670,
        // End TT#432-MD - JSmith - Login into new database with new user and received "Index was outside the bounds of the array" error
        AdminFWOSMax = 810680,
        //End TT#2015 - gtaylor - apply changes to lower levels
        //End TT#108 - MD - DOConnell - FWOS Max Model Enhancement
        //      AdminNodeProperty						= 810700,
        //		AdminNodePropertyCapacity				= 810710,
        //		AdminNodePropertyDailyPcts				= 810720,
        //		AdminNodePropertyEligibility			= 810730,
        //		AdminNodePropertyProfile				= 810740,
        //		AdminNodePropertyPurge					= 810750,
        //		AdminNodePropertyStoreGrades			= 810760,
        //		AdminNodePropertyVelocity				= 810770,
        AdminSecurity = 810800,
        AdminSecurityUsers = 810810,
        AdminSecurityGroups = 810820,
        //		AdminSecurityOTSVersions				= 810830,
        //		AdminSecurityMerchandise				= 810840,
        //		AdminSecurityFunctions					= 810850,
        //		AdminSecurityStoresAttrSets				= 810860,
        //		AdminSecurityWorkflowsMethods			= 810870,
        AdminSize = 810900,
        //Begin Track #3785 - JScott - Remove Administration->Colors and Administration->Size->Size Definition securities.
        //		AdminSizeDefine							= 810910,
        //End Track #3785 - JScott - Remove Administration->Colors and Administration->Size->Size Definition securities.
        AdminSizeCurves = 810920,
        AdminSizeGroups = 810930,
        AdminSizeConstraints = 810940,
        //AdminSizeFringe							= 810950, // MID Track 3619 Remove Fringe
        AdminSizeAlternates = 810960,
        AdminStoreAttributes = 811000,
        // Begin Track #4872 - JSmith - Global/User Attributes
        AdminStoreAttributesGlobal = 811010,
        AdminStoreAttributesUser = 811020,
        // End Track #4872
        AdminStores = 811100,
        AdminStoresProfiles = 811110,
        AdminStoresCharacteristics = 811120,
        AdminModelsOverrideLowLevels = 811121,
        AdminModelsUserOverrideLowLevels = 811122,  // Override Low Level Enhancement
        AdminModelsGlobalOverrideLowLevels = 811123,    // Override Low Level Enhancement

        Allocation = 820000,
        AllocationActions = 821000,
        AllocationActionsNeed = 821001,
        AllocationActionsBalProportional = 821002,
        AllocationActionsBalReserve = 821003,
        AllocationActionsChargeInTransit = 821004,
        AllocationActionsRelease = 821005,
        AllocationActionsReset = 821006,
        AllocationActionsCancelInTransit = 821007,
        AllocationActionsCancelAllocation = 821008,
        AllocationActionsSizeProportional = 821009,
        AllocationActionsBalSizeProportional = 821010,
        AllocationActionsCancelSizeInTransit = 821011,
        AllocationActionsCancelSizeAllocation = 821012,
        AllocationActionsChargeSizeInTransit = 821013,
        AllocationActionsCancelAPIWorkflow = 821014,    // Issue 4554
        AllocationActionsApplyAPIWorkflow = 821015,	// Issue 4554
        AllocationActionsReapplyTotalAllocation = 821016,   // TT#785 - RMatelic - Header Load Interfacing a transaction  trying to Modify a WUB header with a PO type
        AllocationActionsBalSizeWithConstraints = 821017,   // TT#843 - New Size Constraint Balance
        AllocationActionBalSizeBilaterally = 821018,   // TT#794 - New Size Balance for Wet Seal
        BreakoutSizesAsReceivedWithContraints = 821019,   // TT#1391 - Balance Size With Constraints Other Option
        AllocationActionsBalSizeNoSubs = 821020,   // Development TT#22 - JSmith- Security -> Allocation Actions->Balance Size to Reserve
        AllocationActionsBalToVSW = 821022,   // TT#1334-MD - stodd - balance to VSW 

        AllocationMethods = 822000,
        AllocationMethodsGlobal = 822100,
        AllocationMethodsGlobalGeneralAllocation = 822101,
        AllocationMethodsGlobalAllocationOverride = 822102,
        AllocationMethodsGlobalRule = 822103,
        AllocationMethodsGlobalVelocity = 822104,
        AllocationMethodsGlobalFillSizeHoles = 822105,
        AllocationMethodsGlobalBasisSize = 822106,
        AllocationMethodsGlobalSizeNeed = 822107,
        AllocationMethodsGlobalSizeOverride = 822108,
        AllocationMethodsGlobalGeneralAssortment = 822109,
        // Begin TT#155 - JSmith - Size Curve Method
        AllocationMethodsGlobalSizeCurve = 822110,
        // End TT#155
        AllocationMethodsGlobalBuildPacks = 822111,   //tt#370 build packs - apicchetti
        AllocationMethodsGlobalGroupAllocation = 822112,	 // TT#708-MD - Stodd - Group Allocation
        AllocationMethodsGlobalDCCartonRounding = 822113,	 // TT#1652-MD - RMatelic- DC Carton Rounding
        AllocationMethodsGlobalCreateMasterHeaders = 822114,	 // TT#1966-MD - JSmith- DC Fulfillment
        AllocationMethodsGlobalDCFulfillment = 822115,   // TT#1966-MD - JSmith- DC Fulfillment
        AllocationMethodsUser = 822200,
        AllocationMethodsUserGeneralAllocation = 822201,
        AllocationMethodsUserAllocationOverride = 822202,
        AllocationMethodsUserRule = 822203,
        AllocationMethodsUserVelocity = 822204,
        AllocationMethodsUserFillSizeHoles = 822205,
        AllocationMethodsUserBasisSize = 822206,
        AllocationMethodsUserSizeNeed = 822207,
        AllocationMethodsUserSizeOverride = 822208,
        AllocationMethodsUserGeneralAssortment = 822209,
        // Begin TT#155 - JSmith - Size Curve Method
        AllocationMethodsUserSizeCurve = 822210,
        // End TT#155
        AllocationMethodsUserBuildPacks = 822211, //tt#370 build packs - apicchetti
        AllocationMethodsUserGroupAllocation = 822212, // TT#708-MD - Stodd - Group Allocation
        AllocationMethodsUserDCCartonRounding = 822213,	 // TT#1652-MD - RMatelic- DC Carton Rounding
        AllocationMethodsUserCreateMasterHeaders = 822214,	 // TT#1966-MD - JSmith- DC Fulfillment
        AllocationMethodsUserDCFulfillment = 822215,     // TT#1966-MD - JSmith- DC Fulfillment
        AllocationWorkflows = 823000,
        AllocationWorkflowsGlobal = 823100,
        AllocationWorkflowsUser = 823200,
        AllocationHeaders = 825000,
        // BEGIN MID Track #4357 - security for interfaced and non-interfaced headers
        AllocationHeadersNonInterfaced = 825100,
        AllocationHeadersInterfaced = 825200,
        // END MID Track #4357
        AllocationHeadersInterfacedHeader = 825210,   // Begin TT#254 - RMatelic - Add Header Component security to interfaced headers
        AllocationHeadersInterfacedComponent = 825220,   // End TT#254
        AllocationReview = 826000,
        AllocationReviewStyle = 826100,
        AllocationReviewSummary = 826200,
        AllocationReviewSize = 826300,
        //AllocationReviewAssortment				= 826400,  // TT#2 - JSmith - Assortment Security
        //AllocationReviewGroupAllocation			= 826500,		// TT#488-MD - Stodd - Group Allocation






        //		AllocationSummary						= 827000,
        //		AllocationVelocityMatrix				= 828000,
        AllocationViews = 827000, // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        AllocationViewsGlobal = 827100,
        AllocationViewsGlobalWorkspace = 827110,
        AllocationViewsGlobalVelocity = 827111,
        AllocationViewsGlobalVelocityDetail = 827112,
        AllocationViewsGlobalStyleReview = 827113, // TT#454 -Add Views in Style Review 
        AllocationViewsGlobalSizeReview = 827114, // TT#456 -Add Views to Size Review
        AllocationViewsGlobalGroupAllocation = 827115, // TT#708-MD - Stodd - Group Allocation
        AllocationViewsUser = 827200,
        AllocationViewsUserWorkspace = 827210,
        AllocationViewsUserVelocity = 827211,
        AllocationViewsUserVelocityDetail = 827212, // End TT#231
        AllocationViewsUserStyleReview = 827213, // TT#454 -Add Views in Style Review 
        AllocationViewsUserSizeReview = 827214, // TT#456 -Add Views to Size Review
        AllocationViewsUserGroupAllocation = 827215, // TT#708-MD - Stodd - Group Allocation

        Forecast = 830000,
        ForecastMethods = 831000,
        ForecastMethodsGlobal = 831100,
        ForecastMethodsGlobalOTSPlan = 831110,
        ForecastMethodsGlobalOTSBalance = 831111,
        ForecastMethodsGlobalOTSSpread = 831112,
        ForecastMethodsGlobalCopyChain = 831113,
        ForecastMethodsGlobalCopyStore = 831114,
        ForecastMethodsGlobalOTSModifySales = 831115,   // A&F enhancement
                                                        //Begin Enhancement - JScott - Export Method - Part 2
        ForecastMethodsGlobalExport = 831116,
        //End Enhancement - JScott - Export Method - Part 2
        //Begin Enhancement - KJohnson - Global Unlock
        ForecastMethodsGlobalGlobalUnlock = 831117,
        //End Enhancement - KJohnson - Global Unlock
        //Begin Enhancement - KJohnson - Rollup
        ForecastMethodsGlobalRollup = 831119,
        //End Enhancement - KJohnson - Rollup
        //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
        ForecastMethodsGlobalGlobalLock = 831120,
        //End TT#43 - MD - DOConnell - Projected Sales Enhancement
        ForecastMethodsGlobalPlanningExtract = 831122,  // TT#2131-MD - JSmith - Halo Integration
        ForecastMethodsUser = 831200,
        ForecastMethodsUserOTSPlan = 831210,
        ForecastMethodsUserOTSBalance = 831211,
        ForecastMethodsUserOTSSpread = 831212,
        ForecastMethodsUserCopyChain = 831213,
        ForecastMethodsUserCopyStore = 831214,
        ForecastMethodsUserOTSModifySales = 831215, // A&F enhancement
                                                    //Begin Enhancement - JScott - Export Method - Part 2
        ForecastMethodsUserExport = 831216,
        //End Enhancement - JScott - Export Method - Part 2
        //Begin Enhancement - KJohnson - Global Unlock
        ForecastMethodsUserGlobalUnlock = 831217,
        //End Enhancement - KJohnson - Global Unlock
        //Begin Enhancement - KJohnson - Rollup
        ForecastMethodsUserRollup = 831219,
        //End Enhancement - KJohnson - Rollup

        //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
        ForecastMethodsUserGlobalLock = 831220,
        //End TT#43 - MD - DOConnell - Projected Sales Enhancement
        ForecastMethodsUserPlanningExtract = 831222,  // TT#2131-MD - JSmith - Halo Integration

        ForecastViews = 832000,
        ForecastViewsGlobal = 832100,
        ForecastViewsUser = 832200,
        ForecastReview = 833000,
        ForecastSingleLevelChain = 833100,
        ForecastMultiLevelChain = 833200,
        ForecastSingleLevelStore = 833300,
        ForecastMultiLevelStore = 833400,
        //Begin Track Track #3735 - JScott - Add security for matrix balance
        ForecastForecastBalance = 833500,
        //End Track Track #3735 - JScott - Add security for matrix balance
        ForecastWorkflows = 834000,
        ForecastWorkflowsGlobal = 834100,
        ForecastWorkflowsUser = 834200,

        Tools = 840000,
        ToolsAuditViewer = 841000,

        ToolsFiltersStore = 842000,
        ToolsFiltersStoreGlobal = 842100,
        ToolsFiltersStoreUser = 842200,

        ToolsFiltersHeader = 842300, //TT#1313-MD -jsobek -Header Filters
        ToolsFiltersHeaderGlobal = 842310, //TT#1313-MD -jsobek -Header Filters
        ToolsFiltersHeaderUser = 842320, //TT#1313-MD -jsobek -Header Filters

        ToolsFiltersAssortment = 842400, //TT#1313-MD -jsobek -Header Filters
        ToolsFiltersAssortmentGlobal = 842410, //TT#1313-MD -jsobek -Header Filters
        ToolsFiltersAssortmentUser = 842420, //TT#1313-MD -jsobek -Header Filters

        //		ToolsFiltersSystem						= 842300,
        ToolsReleaseResources = 843000,
        ToolsReleaseResourcesPersonal = 843100,
        ToolsReleaseResourcesOthers = 843200,
        ToolsTextEditor = 844000,
        ToolsScheduler = 845000,
        ToolsSchedulerGlobal = 845100,
        ToolsSchedulerGlobalTaskLists = 845101,
        ToolsSchedulerSystem = 845200,
        ToolsSchedulerSystemTaskLists = 845201,
        ToolsSchedulerSystemJobs = 845202,
        ToolsSchedulerSystemSpecialReq = 845203,    // Issue 5117 stodd
        ToolsSchedulerUser = 845300,
        ToolsSchedulerUserTaskLists = 845301,
        ToolsSchedulerBrowser = 845400,
        ToolsProcessControl = 845401,   // TT#1581-MD - stodd - API Header Reconcile
                                        // BEGIN Track #3650 - JSmith - Login security
        ToolsShowLogin = 845500,
        // END Track #3650
        ToolsAuditReclassViewer = 846000,
        //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
        ToolsEmailMessage = 847000,
        //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application

        //Begin TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
        ToolsAllocationAnalysis = 848000,
        ToolsForecastAnalysis = 848100,
        //End TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis

        Assortment = 850000,
        AssortmentMethods = 851000,
        AssortmentMethodsGlobal = 851100,
        AssortmentMethodsGlobalGeneralAssortment = 851110,
        AssortmentMethodsUser = 851200,
        AssortmentMethodsUserGeneralAssortment = 851210,
        AssortmentViews = 852000,
        AssortmentViewsGlobal = 852100,
        //AssortmentViewsGlobalAssortmentReview       = 852110,   // TT#1409-md - stodd - assortment view security wrong  // TT#1995-MD - JSmith - Security - Assortment Views
        AssortmentViewsUser = 852200,
        //AssortmentViewsUserAssortmentReview         = 852210,   // TT#1409-md - stodd - assortment view security wrong  // TT#1995-MD - JSmith - Security - Assortment Views 
        AssortmentReview = 853000,
        // Begin TT#2 - JSmith - Assortment Security
        AssortmentReviewAssortment = 853100,
        AssortmentReviewContent = 853200,
        AssortmentReviewCharacteristic = 853300,
        // End TT#2
        AssortmentWorkflows = 854000,
        AssortmentWorkflowsGlobal = 854100,
        AssortmentWorkflowsUser = 854200,
        // Begin TT#2 - JSmith - Assortment Security
        AssortmentProperties = 855000,
        AssortmentActions = 856000,
        AssortmentActionsRedo = 856001,
        AssortmentActionsCancelAssortment = 856002,
        AssortmentActionsSpreadAverage = 856003,
        AssortmentActionsCreatePlaceholders = 856004,
        AssortmentActionsBalanceAssortment = 856005,
        AssortmentActionsChargeCommitted = 856006,
        AssortmentActionsCancelCommitted = 856007,
        AssortmentActionsChargeIntransit = 856008,
        AssortmentActionsCancelIntransit = 856009,
        AssortmentActionsCreatePlaceholdersBasedOnRevenue = 856010,
        // End TT#2

        Explorers = 860000,
        ExplorersMerchandise = 861000,

        ExplorersMerchandiseFolders = 861100,
        ExplorersMerchandiseFoldersGlobal = 861110,
        ExplorersMerchandiseFoldersUser = 861120,

        ExplorersStore = 862000,
        ExplorersStoreFolders = 862100,
        ExplorersStoreFoldersGlobal = 862110,
        ExplorersStoreFoldersUser = 862120,

        ExplorersTasklist = 863000,
        ExplorersTasklistFolders = 863100,
        ExplorersTasklistFoldersGlobal = 863110,
        ExplorersTasklistFoldersUser = 863120,
        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
        ExplorersTasklistFoldersSystem = 863130,
        //End Track #6321 - JScott - User has ability to to create folders when security is view

        ExplorersFilterStore = 864000,
        ExplorersFilterStoreFolders = 864100,
        ExplorersFilterStoreFoldersGlobal = 864110,
        ExplorersFilterStoreFoldersUser = 864120,

        ExplorersFilterHeader = 864200, //TT#1313-MD -jsobek -Header Filters
        ExplorersFilterHeaderFolders = 864210, //TT#1313-MD -jsobek -Header Filters
        ExplorersFilterHeaderFoldersGlobal = 864220, //TT#1313-MD -jsobek -Header Filters
        ExplorersFilterHeaderFoldersUser = 864230, //TT#1313-MD -jsobek -Header Filters

        ExplorersFilterAssortment = 864300, //TT#1313-MD -jsobek -Header Filters
        ExplorersFilterAssortmentFolders = 864310, //TT#1313-MD -jsobek -Header Filters
        ExplorersFilterAssortmentFoldersGlobal = 864320, //TT#1313-MD -jsobek -Header Filters
        ExplorersFilterAssortmentFoldersUser = 864330, //TT#1313-MD -jsobek -Header Filters

        ExplorersWorkflowMethod = 865000,
        ExplorersWorkflowMethodFolders = 865100,
        ExplorersWorkflowMethodFoldersGlobal = 865110,
        ExplorersWorkflowMethodFoldersUser = 865120,

        ExplorersAllocationWorkspace = 866000,
        //AllocationViews                             = 866100,     // Begin TT#231 - RMatelic - Replaced with 827xxxx above
        //AllocationViewsGlobal                       = 866110,
        //AllocationViewsUser                         = 866120,     // End TT#231

        // Begin TT#2 - JSmith - assortment
        ExplorersAssortment = 867000,
        ExplorersAssortmentExplorer = 867100,
        ExplorersAssortmentExplorerFolders = 867110,
        ExplorersAssortmentExplorerFoldersGlobal = 867111,
        ExplorersAssortmentExplorerFoldersUser = 867112,
        ExplorersAssortmentWorkspace = 867200,
        ExplorersAssortmentWorkspaceViews = 867210,
        ExplorersAssortmentWorkspaceViewsGlobal = 867201,
        ExplorersAssortmentWorkspaceViewsUser = 867202,
        // End TT#2 - JSmith

        Reports = 870000,       // Begin TT#209 - RMatelic - Audit Reports do not allow drag/drop of merchandise into selection
        ReportsAuditReclass = 870100,
        ReportsNodePropertiesOverrides = 870200,
        ReportsForecastAuditMerchandise = 870300,
        ReportsForecastAuditMethod = 870400,
        ReportsAllocationAudit = 870500,
        ReportsUserOptionsReview = 870550,       //TT#554-MD -jsobek -User Log Level Report
        ReportsAllocationByStore = 870560,       //TT#739-MD -jsobek -Delete Stores -Allocation by Store Report
        ReportsCustom = 870600,       // End TT#209 

        // BEgin TT#1007 - md - stodd - change group allocation security - 
        GroupAllocation = 880000,
        GroupAllocationReview = 881000,
        GroupAllocationMatrix = 881100,
        GroupAllocationContent = 881200,
        GroupAllocationCharacteristic = 881300,
        GroupAllocationActions = 882000,
        // End TT#1007 - md - stodd - change group allocation security - 

        // Begin TT#1077 - MD - stodd - cannot create GA views 
        GroupAllocationViews = 883000,
        GroupAllocationViewsGlobal = 883100,
        GroupAllocationViewsUser = 883200,
        // End TT#1077 - MD - stodd - cannot create GA views 

    }

    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    ////Begin Track #4815 - JSmith - #283-User (Security) Maintenance
    //public enum eSharedDataType
    //{
    //    NotSpecified							= 0,
    //    AllHierarchies							= 1,
    //    HierarchyFolder							= 2,
    //    Hierarchy								= 3,
    //    AllMethods								= 4,
    //    MethodFolder							= 5,
    //    Method									= 6,
    //    AllWorkflows							= 7,
    //    WorkflowFolder							= 8,
    //    Workflow								= 9,
    //    AllTasklists							= 10,
    //    TasklistFolder							= 11,
    //    Tasklist								= 12,
    //    AllFilters								= 13,
    //    FilterFolder							= 14,
    //    Filter									= 15,
    //    AllPlanViews							= 16,
    //    PlanViewFolder							= 17,
    //    // BEGIN Track #4985 - John Smith - Override Models
    //    //PlanView								= 18
    //    PlanView								= 18,
    //    // Begin Track #4872 - JSmith - Global/User Attributes
    //    //OverrideLowLevelModel                   = 19
    //    OverrideLowLevelModel                   = 19,
    //    AllStoreGroupsFolder = 20,
    //    UserStoreGroupsFolder = 21,
    //    FavoritesStoreGroupsFolder = 22,
    //    StoreGroupFolder = 23,
    //    StoreGroup = 24
    //    // End Track #4872
    //    // END Track #4985
    //}
    ////End Track #4815
    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

    // MID Track 3619 Remove Fringe
    //public enum eFringeOverrideOperator
    //{
    //	AND = 801400,
    //	OR  = 801401
    //}
    //
    //public enum eFringeOverrideUnitCriteria
    //{
    //	Received = 801410,
    //	RemainingToAllocate = 801411
    //}
    //
    //public enum eFringeOverrideCondition
    //{
    //	LessThan = 801420,
    //	LessThanEqualTo = 801421,
    //	GreaterThan = 801422,
    //	GreaterThanEqualTo = 801423
    //}
    // end MID Track 3619 Remove Fringe

    // begin MID Track 3619 Remove Fringe
    //public enum eFringeOverrideSort
    //{
    //	Ascending = 801440,
    //	Descending = 801441
    //}
    // end MID Track 3619 Remove Fringe

    public enum eMethodStatus
    {
        ValidMethod = 0,
        InvalidMethod = 1
    }

    // ForeCast and Allocation Methods and Actions enums 802100 - 802200
    /// <summary>
    /// A list of the valid workflow actions and methods. 
    /// </summary>
    /// <remarks>
    /// When updating this enum, also update either eAllocationMethodType or eForecastMethodType (whichever
    /// is the appropriate application).  If the new method type has a user interface, also update eMethodTypeUI.
    /// </remarks>
    public enum eMethodType
    {
        //Begin Track #4010 - JScott - Add all forecast methods and workflows to the scheduler.
        NotSpecified = 0,
        //End Track #4010 - JScott - Add all forecast methods and workflows to the scheduler.
        OTSPlan = 802100,
        ForecastBalance = 802101,
        GeneralAllocation = 802102,
        AllocationOverride = 802103,
        Velocity = 802104,
        Rule = 802105,
        FillSizeHolesAllocation = 802106,
        BasisSizeAllocation = 802107,
        WarehouseSizeAllocation = 802108,
        SizeNeedAllocation = 802109,
        SizeOverrideAllocation = 802110,
        StyleNeed = 802111,
        BreakoutSizesAsReceived = 802112,
        BalanceStyleProportional = 802113,
        BalanceToDC = 802114,
        BalanceSizeWithSubs = 802115,
        BalanceSizeNoSubs = 802116,
        ChargeIntransit = 802117,
        Release = 802118,
        Reset = 802119,
        BackoutSizeIntransit = 802120,
        BackoutStyleIntransit = 802121,
        BackoutSizeAllocation = 802122,
        BackoutAllocation = 802123,
        DeleteHeader = 802124,
        RedoRules = 802125,
        BackoutDetailPackAllocation = 802126,
        // BEGIN MID Track #2470 - Charge Size Intransit action is missing
        ChargeSizeIntransit = 802127,
        // END  MID Track #2470
        ForecastSpread = 802128,
        CopyChainForecast = 802129,
        CopyStoreForecast = 802130,
        GeneralAssortment = 802131,
        //Begin Enhancement - JScott - Export Method - Part 2
        //		ForecastModifySales			= 802132
        ForecastModifySales = 802132,
        RebuildIntransit = 802133,  // TT#1137 (MID Track 4351) Rebuild Intransit Utility
        Export = 802136,
        //End Enhancement - JScott - Export Method - Part 2
        // begin MID Track 4554 AnF Enhancement API Workflow
        RemoveAPI_Workflow = 802134,
        ApplyAPI_Workflow = 802135,
        // end MID Track 4554 AnF Enhancement API Workflo
        //Begin Enhancement #5004 - KJohnson - Global Unlock
        GlobalUnlock = 802137,
        //End Enhancement #5004 - KJohnson - Global Unlock
        //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
        GlobalLock = 802138,
        //End TT#43 - MD - DOConnell - Projected Sales Enhancement
        //Begin Enhancement #5196 - KJohnson - Rollup
        // Begin TT#155 - JSmith - Size Curve Method
        //Rollup                      = 802139
        Rollup = 802139,
        //End Enhancement #5196 - KJohnson - Rollup
        SizeCurve = 802140,
        /* Begin TT#370 - APicchetti - Build Packs Method */
        BuildPacks = 802141,
        /* End TT#370 */
        // begin TT#843 - New Size Constraint Balance
        ReapplyTotalAllocation = 802142,     // TT#785 - RMatelic - Header Load Interfacing a transaction  trying to Modify a WUB header with a PO type
        BalanceSizeWithConstraints = 802143,      // TT#1391 - JEllis - Balance Size With Contraints Other Options
        // end TT#843 - New Size Constraint Balance
        BalanceSizeBilaterally = 802144,      // TT#794 New Size Balance for Wet Seal
        BreakoutSizesAsReceivedWithConstraints = 802145,  // TT#1391 - JEllis - Balance Size With Contraints Other Options
        GroupAllocation = 802146,       // TT#708-MD - Stodd - Group Allocation Prototype.
        BalanceToVSW = 802147,
                                        // Begin TT#1224 - stodd - committed
        ChargeCommitted = 802161,
        CancelCommitted = 802162,
        // End TT#1224 - stodd - committed
        DCCartonRounding = 802166,  // TT#1652-MD - RMAtelic - DC Carton Rounding
        CreateMasterHeaders = 802167,  // TT#1966-MD - JSmith - DC Fulfillment
        DCFulfillment = 802168,  // TT#1966-MD - JSmith - DC Fulfillment
        PlanningExtract = 802169,  // TT#2131-MD - JSmith - Halo Integration
    }

    /// <summary>
    /// This enum is a subset of the eMethodType
    /// </summary>
    /// <remarks>
    /// The types in this enum represent the methods that have a user interface.  In other words, the actions
    /// associated with these methods have optional or required user inputs.
    /// </remarks>
    public enum eMethodTypeUI
    {
        None = 0,
        OTSPlan = 802100,
        ForecastBalance = 802101,
        GeneralAllocation = 802102,
        AllocationOverride = 802103,
        Velocity = 802104,
        Rule = 802105,
        FillSizeHolesAllocation = 802106,
        BasisSizeAllocation = 802107,
        WarehouseSizeAllocation = 802108,
        SizeNeedAllocation = 802109,
        SizeOverrideAllocation = 802110,
        //		PackContentAllocation       = 802111
        //		StyleNeed                   = 802112,
        ForecastSpread = 802128,
        CopyChainForecast = 802129,
        CopyStoreForecast = 802130,
        GeneralAssortment = 802131,
        //Begin Enhancement - JScott - Export Method - Part 2
        //		ForecastModifySales			= 802132
        ForecastModifySales = 802132,
        Export = 802136,
        //End Enhancement - JScott - Export Method - Part 2
        //Begin Enhancement #5004 - KJohnson - Global Unlock
        GlobalUnlock = 802137,
        //End Enhancement #5004 - KJohnson - Global Unlock
        //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
        GlobalLock = 802138,
        //End TT#43 - MD - DOConnell - Projected Sales Enhancement
        //Begin Enhancement #5196 - KJohnson - Rollup
        // Begin TT#155 - JSmith - Size Curve Method
        //Rollup                      = 802139
        Rollup = 802139,
        //End Enhancement #5196 - KJohnson - Rollup
        SizeCurve = 802140,
        // End TT#155

        // Begin TT#370 - APicchetti - Build Packs Method
        BuildPacks = 802141,
        //End TT#370
        GroupAllocation = 802146,	// TT#708-MD - Stodd - Group Allocation Prototype.
        DCCartonRounding = 802166,  // TT#1652-MD - RMAtelic - DC Carton Rounding
        CreateMasterHeaders = 802167,  // TT#1966-MD - JSmith - DC Fulfillment
        DCFulfillment = 802168,  // TT#1966-MD - JSmith - DC Fulfillment
        PlanningExtract = 802169,  // TT#2131-MD - JSmith - Halo Integration
    }

    /// <summary>
    /// This enum is a subset of the eMethodType
    /// </summary>
    /// <remarks>
    /// The types in this enum represent forecast methods and actions
    /// </remarks>
    public enum eForecastMethodType
    {
        OTSPlan = 802100,
        ForecastBalance = 802101,
        ForecastSpread = 802128,
        CopyChainForecast = 802129,
        CopyStoreForecast = 802130,
        //Begin Enhancement - JScott - Export Method - Part 2
        //		ForecastModifySales			= 802132
        ForecastModifySales = 802132,
        Export = 802136,
        //End Enhancement - JScott - Export Method - Part 2
        //Begin Enhancement #5004 - KJohnson - Global Unlock
        GlobalUnlock = 802137,
        //End Enhancement #5004 - KJohnson - Global Unlock
        //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
        GlobalLock = 802138,
        //End TT#43 - MD - DOConnell - Projected Sales Enhancement
        //Begin Enhancement #5196 - KJohnson - Rollup
        Rollup = 802139,
        //End Enhancement #5196 - KJohnson - Rollup
        PlanningExtract = 802169,  // TT#2131-MD - JSmith - Halo Integration
    }

    /// <summary>
    /// This enum is a subset of the eMethodType
    /// </summary>
    /// <remarks>
    /// The types in this enum represent allocation methods and actions.
    /// See also:  eAllocationMethodType_IT_Updt.
    /// See also:  eAllocationActionType.
    /// </remarks>
    public enum eAllocationMethodType
    {
        GeneralAllocation = 802102,
        AllocationOverride = 802103,
        Velocity = 802104,
        Rule = 802105,
        FillSizeHolesAllocation = 802106,
        BasisSizeAllocation = 802107,
        WarehouseSizeAllocation = 802108,
        SizeNeedAllocation = 802109,
        SizeOverrideAllocation = 802110,
        StyleNeed = 802111,
        BreakoutSizesAsReceived = 802112,
        BalanceStyleProportional = 802113,
        BalanceToDC = 802114,
        BalanceSizeWithSubs = 802115,
        BalanceSizeNoSubs = 802116,
        ChargeIntransit = 802117,
        Release = 802118,
        Reset = 802119,
        BackoutSizeIntransit = 802120,
        BackoutStyleIntransit = 802121,
        BackoutSizeAllocation = 802122,
        BackoutAllocation = 802123,
        DeleteHeader = 802124,
        RedoRules = 802125,
        BackoutDetailPackAllocation = 802126,
        // BEGIN MID Track #2470 - Charge Size Intransit action is missing
        ChargeSizeIntransit = 802127, // MID Track 4554 AnF Enhancement API Workflow
                                      // END  MID Track #2470
                                      // Added for method processing
        GeneralAssortment = 802131,
        RebuildIntransit = 802133,  // TT#1137 (MID Track 4351) Rebuild Intransit Utility
                                    // begin MID Track 4554 AnF Enhancement API Workflow
        RemoveAPI_Workflow = 802134,
        // Begin TT#155 - JSmith - Size Curve Method
        //ApplyAPI_Workflow           = 802135
        ApplyAPI_Workflow = 802135,
        // end MID Track 4554 AnF Enhancement API Workflo
        SizeCurve = 802140,
        // End TT#155
        /* Begin TT#370 - APicchetti - Build Packs Method */
        BuildPacks = 802141,
        /* End TT#370 */
        // Begin TT#843 - New Size Constraint Balance */
        ReapplyTotalAllocation = 802142,     // TT#785 - RMatelic - Header Load Interfacing a transaction  trying to Modify a WUB header with a PO type
        BalanceSizeWithConstraints = 802143,     // TT#1391 - JEllis - Balance Size With Contraints Other Options 
        // end TT#843 - New Size Constraint Balance
        BalanceSizeBilaterally = 802144,      // TT#794 - New Size Balance For Wet Seal
        BreakoutSizesAsReceivedWithConstraints = 802145,  // TT#1391 - JEllis - Balance Size With Contraints Other Options
        GroupAllocation = 802146,		// TT#708-MD - Stodd - Group Allocation Prototype.
        BalanceToVSW = 802147,               // TT#1334-MD - Stodd - Balance to VSW.
        DCCartonRounding = 802166,  // TT#1652-MD - RMAtelic - DC Carton Rounding
        CreateMasterHeaders = 802167,  // TT#1966-MD - JSmith - DC Fulfillment
        DCFulfillment = 802168  // TT#1966-MD - JSmith - DC Fulfillment
    }

    public enum eWorkflowMethodNodeType
    {
        WorkflowMethodMainFavoritesFolder = 51, // DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodMainUserFolder = 52,  // DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodMainGlobalFolder = 53,    // DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodSubFolder = 54,   // DO NOT CHANGE -- USED ON DATABASE
        Workflow = 55,  // DO NOT CHANGE -- USED ON DATABASE
        MethodOTSPlanFolder = 59,	// DO NOT CHANGE -- USED ON DATABASE
        MethodForecastBalanceFolder = 60,   // DO NOT CHANGE -- USED ON DATABASE
        MethodModifySalesFolder = 61,   // DO NOT CHANGE -- USED ON DATABASE
        MethodForecastSpreadFolder = 62,    // DO NOT CHANGE -- USED ON DATABASE
        MethodCopyChainForecastFolder = 63, // DO NOT CHANGE -- USED ON DATABASE
        MethodCopyStoreForecastFolder = 64, // DO NOT CHANGE -- USED ON DATABASE
        MethodExportFolder = 65,    // DO NOT CHANGE -- USED ON DATABASE
        MethodGlobalUnlockFolder = 66,  // DO NOT CHANGE -- USED ON DATABASE
        MethodRollupFolder = 67,    // DO NOT CHANGE -- USED ON DATABASE
        MethodGeneralAllocationFolder = 68, // DO NOT CHANGE -- USED ON DATABASE
        MethodAllocationOverrideFolder = 69,    // DO NOT CHANGE -- USED ON DATABASE
        MethodRuleFolder = 70,  // DO NOT CHANGE -- USED ON DATABASE
        MethodVelocityFolder = 71,  // DO NOT CHANGE -- USED ON DATABASE
        MethodSizeMethodFolder = 72,	// DO NOT CHANGE -- USED ON DATABASE
        MethodFillSizeHolesFolder = 73, // DO NOT CHANGE -- USED ON DATABASE
        MethodBasisSizeFolder = 74, // DO NOT CHANGE -- USED ON DATABASE
        MethodSizeNeedFolder = 75,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodOTSForcastFolder = 76,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodOTSForcastWorkflowsFolder = 77,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodOTSForcastMethodsFolder = 78,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodAllocationFolder = 79,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodAllocationWorkflowsFolder = 80,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodAllocationMethodsFolder = 81,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodAllocationSizeMethodsFolder = 82,	// DO NOT CHANGE -- USED ON DATABASE
        MethodOTSPlan = 83,	// DO NOT CHANGE -- USED ON DATABASE
        MethodForecastBalance = 84,	// DO NOT CHANGE -- USED ON DATABASE
        MethodModifySales = 85,	// DO NOT CHANGE -- USED ON DATABASE
        MethodForecastSpread = 86,	// DO NOT CHANGE -- USED ON DATABASE
        MethodCopyChainForecast = 87,	// DO NOT CHANGE -- USED ON DATABASE
        MethodCopyStoreForecast = 88,	// DO NOT CHANGE -- USED ON DATABASE
        MethodExport = 89,	// DO NOT CHANGE -- USED ON DATABASE
        MethodGlobalUnlock = 90,	// DO NOT CHANGE -- USED ON DATABASE
        MethodRollup = 91,	// DO NOT CHANGE -- USED ON DATABASE
        MethodGeneralAllocation = 92,	// DO NOT CHANGE -- USED ON DATABASE
        MethodAllocationOverride = 93,	// DO NOT CHANGE -- USED ON DATABASE
        MethodRule = 94,	// DO NOT CHANGE -- USED ON DATABASE
        MethodVelocity = 95,	// DO NOT CHANGE -- USED ON DATABASE
        MethodSizeNeedAllocation = 96,	// DO NOT CHANGE -- USED ON DATABASE
        MethodFillSizeHolesAllocation = 97,	// DO NOT CHANGE -- USED ON DATABASE
        MethodBasisSizeAllocation = 98,	// DO NOT CHANGE -- USED ON DATABASE
        MethodWarehouseSizeAllocation = 99,  // DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodOTSForcastWorkflowsSubFolder = 100,	// DO NOT CHANGE -- USED ON DATABASE
        MethodOTSPlanSubFolder = 101,	// DO NOT CHANGE -- USED ON DATABASE
        MethodForecastBalanceSubFolder = 102,	// DO NOT CHANGE -- USED ON DATABASE
        MethodModifySalesSubFolder = 103,	// DO NOT CHANGE -- USED ON DATABASE
        MethodForecastSpreadSubFolder = 104,	// DO NOT CHANGE -- USED ON DATABASE
        MethodCopyChainForecastSubFolder = 105,	// DO NOT CHANGE -- USED ON DATABASE
        MethodCopyStoreForecastSubFolder = 106,	// DO NOT CHANGE -- USED ON DATABASE
        MethodExportSubFolder = 107,	// DO NOT CHANGE -- USED ON DATABASE
        MethodGlobalUnlockSubFolder = 108,	// DO NOT CHANGE -- USED ON DATABASE
        MethodRollupSubFolder = 109,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodAllocationWorkflowsSubFolder = 110,	// DO NOT CHANGE -- USED ON DATABASE
        MethodGeneralAllocationSubFolder = 111,	// DO NOT CHANGE -- USED ON DATABASE
        MethodAllocationOverrideSubFolder = 112,	// DO NOT CHANGE -- USED ON DATABASE
        MethodRuleSubFolder = 113,	// DO NOT CHANGE -- USED ON DATABASE
        MethodVelocitySubFolder = 114,	// DO NOT CHANGE -- USED ON DATABASE
        MethodSizeMethodSubFolder = 115,	// DO NOT CHANGE -- USED ON DATABASE
        MethodFillSizeHolesSubFolder = 116,	// DO NOT CHANGE -- USED ON DATABASE
        MethodBasisSizeSubFolder = 117,	// DO NOT CHANGE -- USED ON DATABASE
        MethodSizeNeedSubFolder = 118,	// DO NOT CHANGE -- USED ON DATABASE
        MethodSizeCurve = 119,	// DO NOT CHANGE -- USED ON DATABASE
        MethodSizeCurveFolder = 120,	// DO NOT CHANGE -- USED ON DATABASE
        MethodSizeCurveSubFolder = 121,	// DO NOT CHANGE -- USED ON DATABASE
        MethodBuildPacks = 122,	// DO NOT CHANGE -- USED ON DATABASE
        MethodBuildPacksFolder = 123,	// DO NOT CHANGE -- USED ON DATABASE
        MethodBuildPacksSubFolder = 124,    // DO NOT CHANGE -- USED ON DATABASE
        MethodGlobalLock = 130, // DO NOT CHANGE -- USED ON DATABASE
        MethodGlobalLockFolder = 131,   // DO NOT CHANGE -- USED ON DATABASE
        MethodGlobalLockSubFolder = 132,    // DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodMainSharedFolder = 137,
        WorkflowMethodExpStaticNode = 197,
        MethodGroupAllocation = 306,
        MethodGroupAllocationFolder = 307,
        MethodGroupAllocationSubFolder = 308,
        MethodDCCartonRounding = 320,
        MethodDCCartonRoundingFolder = 321,
        MethodDCCartonRoundingSubFolder = 322,
        MethodCreateMasterHeaders = 331,
        MethodCreateMasterHeadersFolder = 332,
        MethodCreateMasterHeadersSubFolder = 333,
        MethodDCFulfillment = 334,
        MethodDCFulfillmentFolder = 335,
        MethodDCFulfillmentSubFolder = 336,
        MethodPlanningExtract = 339,
        MethodPlanningExtractFolder = 340,
        MethodPlanningExtractSubFolder = 341,
    }

    public enum eWorkflowMethodNodeAllocationType
    {
        MethodGeneralAllocationFolder = 68, // DO NOT CHANGE -- USED ON DATABASE
        MethodAllocationOverrideFolder = 69,    // DO NOT CHANGE -- USED ON DATABASE
        MethodRuleFolder = 70,  // DO NOT CHANGE -- USED ON DATABASE
        MethodVelocityFolder = 71,  // DO NOT CHANGE -- USED ON DATABASE
        MethodSizeMethodFolder = 72,	// DO NOT CHANGE -- USED ON DATABASE
        MethodFillSizeHolesFolder = 73, // DO NOT CHANGE -- USED ON DATABASE
        MethodBasisSizeFolder = 74, // DO NOT CHANGE -- USED ON DATABASE
        MethodSizeNeedFolder = 75,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodAllocationFolder = 79,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodAllocationWorkflowsFolder = 80,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodAllocationMethodsFolder = 81,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodAllocationSizeMethodsFolder = 82,	// DO NOT CHANGE -- USED ON DATABASE
        MethodGeneralAllocation = 92,	// DO NOT CHANGE -- USED ON DATABASE
        MethodAllocationOverride = 93,	// DO NOT CHANGE -- USED ON DATABASE
        MethodRule = 94,	// DO NOT CHANGE -- USED ON DATABASE
        MethodVelocity = 95,	// DO NOT CHANGE -- USED ON DATABASE
        MethodSizeNeedAllocation = 96,	// DO NOT CHANGE -- USED ON DATABASE
        MethodFillSizeHolesAllocation = 97,	// DO NOT CHANGE -- USED ON DATABASE
        MethodBasisSizeAllocation = 98,	// DO NOT CHANGE -- USED ON DATABASE
        MethodWarehouseSizeAllocation = 99,  // DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodAllocationWorkflowsSubFolder = 110,	// DO NOT CHANGE -- USED ON DATABASE
        MethodGeneralAllocationSubFolder = 111,	// DO NOT CHANGE -- USED ON DATABASE
        MethodAllocationOverrideSubFolder = 112,	// DO NOT CHANGE -- USED ON DATABASE
        MethodRuleSubFolder = 113,	// DO NOT CHANGE -- USED ON DATABASE
        MethodVelocitySubFolder = 114,	// DO NOT CHANGE -- USED ON DATABASE
        MethodSizeMethodSubFolder = 115,	// DO NOT CHANGE -- USED ON DATABASE
        MethodFillSizeHolesSubFolder = 116,	// DO NOT CHANGE -- USED ON DATABASE
        MethodBasisSizeSubFolder = 117,	// DO NOT CHANGE -- USED ON DATABASE
        MethodSizeNeedSubFolder = 118,	// DO NOT CHANGE -- USED ON DATABASE
        MethodSizeCurve = 119,	// DO NOT CHANGE -- USED ON DATABASE
        MethodSizeCurveFolder = 120,	// DO NOT CHANGE -- USED ON DATABASE
        MethodSizeCurveSubFolder = 121,	// DO NOT CHANGE -- USED ON DATABASE
        MethodBuildPacks = 122,	// DO NOT CHANGE -- USED ON DATABASE
        MethodBuildPacksFolder = 123,	// DO NOT CHANGE -- USED ON DATABASE
        MethodBuildPacksSubFolder = 124,    // DO NOT CHANGE -- USED ON DATABASE
        MethodGroupAllocation = 306,
        MethodGroupAllocationFolder = 307,
        MethodGroupAllocationSubFolder = 308,
        MethodDCCartonRounding = 320,
        MethodDCCartonRoundingFolder = 321,
        MethodDCCartonRoundingSubFolder = 322,
        MethodCreateMasterHeaders = 331,
        MethodCreateMasterHeadersFolder = 332,
        MethodCreateMasterHeadersSubFolder = 333,
        MethodDCFulfillment = 334,
        MethodDCFulfillmentFolder = 335,
        MethodDCFulfillmentSubFolder = 336,
    }

    public enum eWorkflowMethodNodePlanningType
    {
        MethodOTSPlanFolder = 59,	// DO NOT CHANGE -- USED ON DATABASE
        MethodForecastBalanceFolder = 60,   // DO NOT CHANGE -- USED ON DATABASE
        MethodModifySalesFolder = 61,   // DO NOT CHANGE -- USED ON DATABASE
        MethodForecastSpreadFolder = 62,    // DO NOT CHANGE -- USED ON DATABASE
        MethodCopyChainForecastFolder = 63, // DO NOT CHANGE -- USED ON DATABASE
        MethodCopyStoreForecastFolder = 64, // DO NOT CHANGE -- USED ON DATABASE
        MethodExportFolder = 65,    // DO NOT CHANGE -- USED ON DATABASE
        MethodGlobalUnlockFolder = 66,  // DO NOT CHANGE -- USED ON DATABASE
        MethodRollupFolder = 67,    // DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodOTSForcastFolder = 76,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodOTSForcastWorkflowsFolder = 77,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodOTSForcastMethodsFolder = 78,	// DO NOT CHANGE -- USED ON DATABASE
        MethodOTSPlan = 83,	// DO NOT CHANGE -- USED ON DATABASE
        MethodForecastBalance = 84,	// DO NOT CHANGE -- USED ON DATABASE
        MethodModifySales = 85,	// DO NOT CHANGE -- USED ON DATABASE
        MethodForecastSpread = 86,	// DO NOT CHANGE -- USED ON DATABASE
        MethodCopyChainForecast = 87,	// DO NOT CHANGE -- USED ON DATABASE
        MethodCopyStoreForecast = 88,	// DO NOT CHANGE -- USED ON DATABASE
        MethodExport = 89,	// DO NOT CHANGE -- USED ON DATABASE
        MethodGlobalUnlock = 90,	// DO NOT CHANGE -- USED ON DATABASE
        MethodRollup = 91,	// DO NOT CHANGE -- USED ON DATABASE
        WorkflowMethodOTSForcastWorkflowsSubFolder = 100,	// DO NOT CHANGE -- USED ON DATABASE
        MethodOTSPlanSubFolder = 101,	// DO NOT CHANGE -- USED ON DATABASE
        MethodForecastBalanceSubFolder = 102,	// DO NOT CHANGE -- USED ON DATABASE
        MethodModifySalesSubFolder = 103,	// DO NOT CHANGE -- USED ON DATABASE
        MethodForecastSpreadSubFolder = 104,	// DO NOT CHANGE -- USED ON DATABASE
        MethodCopyChainForecastSubFolder = 105,	// DO NOT CHANGE -- USED ON DATABASE
        MethodCopyStoreForecastSubFolder = 106,	// DO NOT CHANGE -- USED ON DATABASE
        MethodExportSubFolder = 107,	// DO NOT CHANGE -- USED ON DATABASE
        MethodGlobalUnlockSubFolder = 108,	// DO NOT CHANGE -- USED ON DATABASE
        MethodRollupSubFolder = 109,	// DO NOT CHANGE -- USED ON DATABASE
        MethodGlobalLock = 130, // DO NOT CHANGE -- USED ON DATABASE
        MethodGlobalLockFolder = 131,   // DO NOT CHANGE -- USED ON DATABASE
        MethodGlobalLockSubFolder = 132,    // DO NOT CHANGE -- USED ON DATABASE
        MethodPlanningExtract = 339,
        MethodPlanningExtractFolder = 340,
        MethodPlanningExtractSubFolder = 341,
    }

    // begin TT#488 - MD - Jellis - Group Allocation
    public enum eGroupAllocationActionType
    {
        StyleNeed = 802111
    }
    // end TT#488 - Md -Jellis - Group ALlocation

    // begin TT#891 - MD - Jellis - Group Allocation NEED action gets error
    public enum eGroupAllocationPlaceholderActionType
    {
        BackoutSizeAllocation = 802122,
        BackoutAllocation = 802123,
        BackoutDetailPackAllocation = 802126
    }
    // end TT#891 - MD - Jellis - Group Allocation NEED action gets error
    // begin TT#925 - MD - Jellis - Group Allocation Balance Ignores stores with allocation
    public enum eGroupAllocationActionRequiredType
    {
        BackoutSizeAllocation = 802122,
        BackoutAllocation = 802123,
        BackoutDetailPackAllocation = 802126
    }
    // end TT#925 - MD - Jellis - Group Allocation Balance ignores stores with allocation


    /// <summary>
    /// This enum is a subset of the eMethodType
    /// </summary>
    /// <remarks>
    /// The types in this enum represent assortment methods and actions.
    /// </remarks>
    public enum eAssortmentMethodType
    {
        GeneralAssortment = 802131
    }

    /// <summary>
    /// This enum is a subset of the eMethodType which contains all size methods
    /// </summary>
    public enum eSizeMethodType
    {
        FillSizeHolesAllocation = 802106,
        BasisSizeAllocation = 802107,
        WarehouseSizeAllocation = 802108,
        SizeNeedAllocation = 802109,
        SizeOverrideAllocation = 802110,
        BreakoutSizesAsReceived = 802112,
        BalanceSizeWithSubs = 802115,
        BalanceSizeNoSubs = 802116,
        BackoutSizeIntransit = 802120,
        // Begin TT#155 - JSmith - Size Curve Method
        //BackoutSizeAllocation       = 802122
        BackoutSizeAllocation = 802122,
        SizeCurve = 802140,
        // End TT#155
        BalanceSizeWithConstraints = 802143,   // TT#843 - New Size Constraint Balance  // TT#1391 - JEllis - Balance Size With Contraints Other Options
        BalanceSizeBilaterally = 802144,   // TT#794 - New Size Balance for Wet Seal
        BreakoutSizesAsReceivedWithConstraints = 802145  // TT#1391 - JEllis - Balance Size With Contraints Other Options
    }

    // Begin TT#155 - JSmith - Size Curve Method
    /// <summary>
    /// This enum is a subset of the eMethodType which contains methods that do not require headers
    /// </summary>
    public enum eNoHeaderMethodType
    {
        OTSPlan = 802100,
        ForecastBalance = 802101,
        ForecastSpread = 802128,
        CopyChainForecast = 802129,
        CopyStoreForecast = 802130,
        ForecastModifySales = 802132,
        Export = 802136,
        GlobalUnlock = 802137,
        //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
        GlobalLock = 802138,
        //End TT#43 - MD - DOConnell - Projected Sales Enhancement
        Rollup = 802139,
        SizeCurve = 802140,
        PlanningExtract = 802169,  // TT#2131-MD - JSmith - Halo Integration
    }
    // End TT#155

    // Begin TT#1966-MD - JSmith - DC Fulfillment
    public enum eOptionalHeaderMethodType
    {
        CreateMasterHeaders = 802167,
    }
    // End TT#1966-MD - JSmith - DC Fulfillment

    /// <summary>
    /// This enum is a subset of the eMethodType
    /// </summary>
    /// <remarks>
    /// The types in this enum represent the methods that have a filter.
    /// </remarks>
    public enum eFilteredMethodType
    {
        Rule = 802105
    }

    /// <summary>
    /// This enum is a subset of the eMethodType
    /// </summary>
    /// <remarks>
    /// The types in this enum represent the methods that allow a tolerance.
    /// </remarks>
    public enum eToleranceMethodType
    {
        BalanceStyleProportional = 802113,
        BalanceToDC = 802114,
        BalanceSizeWithSubs = 802115,
        BalanceSizeNoSubs = 802116,
        ForecastBalance = 802101, // TT#843 - New Size Constraint Balance  
        BalanceSizeWithConstraints = 802143,   // TT#843 - New Size Constraint Balance   // TT#794 - New Size Balance for Wet Seal 
        BalanceSizeBilaterally = 802144,   // TT#794 - New Size Balance for Wet Seal // TT#1334-MD - Stodd - Balance to VSW.
        BalanceToVSW = 802147    // TT#1334-MD - Stodd - Balance to VSW.
    }

    /// <summary>
    /// This enum is a subset of the eMethodType
    /// </summary>
    /// <remarks>
    /// The types in this enum represent the methods that allow a step to be balanced in a workflow.
    /// </remarks>
    public enum eBalanceStepMethodType
    {
        // Issue 4010 - commented out because OTS forecast doean't accept override
        //OTSPlan						= 802100
    }

    /// <summary>
    /// This enum is a subset of the eMethodType
    /// </summary>
    /// <remarks>
    /// The types in this enum represent forecast methods that can accept an override variable
    /// </remarks>
    public enum eForecastVariableMethodType
    {
        OTSPlan = 802100,       // Issue 4962 stodd
        ForecastBalance = 802101
    }

    /// <summary>
    /// This enum is a subset of the eMethodType
    /// </summary>
    /// <remarks>
    /// The types in this enum represent the methods that allow a component.
    /// </remarks>
    public enum eComponentMethodType
    {
        OTSPlan = 802100,
        ForecastBalance = 802101,
        GeneralAllocation = 802102,
        AllocationOverride = 802103,
        Velocity = 802104,
        Rule = 802105,
        FillSizeHolesAllocation = 802106,
        BasisSizeAllocation = 802107,
        WarehouseSizeAllocation = 802108,
        SizeNeedAllocation = 802109,
        SizeOverrideAllocation = 802110,
        StyleNeed = 802111,
        BreakoutSizesAsReceived = 802112,
        BalanceStyleProportional = 802113,
        BalanceToDC = 802114,
        BalanceSizeWithSubs = 802115,
        BalanceSizeNoSubs = 802116,
        ChargeIntransit = 802117,
        Release = 802118,
        Reset = 802119,
        BackoutSizeIntransit = 802120,
        BackoutStyleIntransit = 802121,
        BackoutSizeAllocation = 802122,
        BackoutAllocation = 802123,
        DeleteHeader = 802124,
        RedoRules = 802125,
        BackoutDetailPackAllocation = 802126,
        // BEGIN MID Track #2470 - Charge Size Intransit action is missing
        ChargeSizeIntransit = 802127,   // TT#843 - New Size Constraint Balance 
                                        // END  MID Track #2470
        BalanceSizeWithConstraints = 802143,     // TT#843 - New Size Constraint Balance // TT#1391 - JEllis - Balance Size With Contraints Other Options
        BalanceSizeBilaterally = 802144,     // TT#794 - New Size Balance for Wet Seal
        BreakoutSizesAsReceivedWithConstraints = 802145  // TT#1391 - JEllis - Balance Size With Contraints Other Options
    }

    /// <summary>
    /// This enum is a subset of the eAllocationMethodType
    /// </summary>
    /// <remarks>
    /// The types in this enum represent allocation actions.
    /// </remarks>
    public enum eAllocationActionType
    {
        None = -1,
        StyleNeed = 802111,
        BreakoutSizesAsReceived = 802112,
        BalanceStyleProportional = 802113,
        BalanceToDC = 802114,
        BalanceSizeWithSubs = 802115,
        BalanceSizeNoSubs = 802116,
        ChargeIntransit = 802117,
        Release = 802118,
        Reset = 802119,
        BackoutSizeIntransit = 802120,
        BackoutStyleIntransit = 802121,
        BackoutSizeAllocation = 802122,
        BackoutAllocation = 802123,
        DeleteHeader = 802124,
        BackoutDetailPackAllocation = 802126,
        // BEGIN MID Track #2470 - Charge Size Intransit action is missing
        ChargeSizeIntransit = 802127, // MID Track 4554 AnF Enhancement API Workflow
                                      // END  MID Track #2470
                                      // begin MID Track 4554 AnF Enhancement API Workflow
        RemoveAPI_Workflow = 802134,
        ApplyAPI_Workflow = 802135,
        // end MID Track 4554 AnF Enhancement API Workflo
        ReapplyTotalAllocation = 802142,     // TT#785 - RMatelic - Header Load Interfacing a transaction  trying to Modify a WUB header with a PO type // TT#843 - New Size Constraint Balance
        BalanceSizeWithConstraints = 802143,     // TT#843 - New Size Constraint Balance  // TT#1391 - JEllis - Balance Size With Constraints Other Option
        BalanceSizeBilaterally = 802144,     // TT#794 - New Size Balance for Wet Seal
        BreakoutSizesAsReceivedWithConstraints = 802145,  // TT#1391 - JEllis - Balance Size With Contraints Other Options
        BalanceToVSW = 802147           // TT#1334-MD - stodd - Balance to VSW Action
    }

    /// <summary>
    /// This enum is a list of Assortment Actions
    /// </summary>
    public enum eAssortmentAllocationActionType
    {
        StyleNeed = 802111,
        BreakoutSizesAsReceived = 802112,   // TT#531-MD - stodd -  Size Proportional is missing from the list pof Allocation Actions 
        BalanceStyleProportional = 802113,
        BalanceToDC = 802114,
        BalanceSizeWithSubs = 802115,
        BalanceSizeNoSubs = 802116,
        ChargeIntransit = 802117,
        Release = 802118,
        Reset = 802119,
        BackoutSizeIntransit = 802120,
        BackoutStyleIntransit = 802121,
        BackoutSizeAllocation = 802122,
        BackoutAllocation = 802123,
        BalanceSizeWithConstraints = 802143,     // TT#843 - New Size Constraint Balance // TT#1391 - JEllis - Balance Size With Contraints Other Options
        BalanceSizeBilaterally = 802144,     // TT#794 - New Size Balance for Wet Seal
        BreakoutSizesAsReceivedWithConstraints = 802145  // TT#1391 - JEllis - Balance Size With Contraints Other Options
    }

    /// <summary>
    /// This enum is a list of Assortment Actions
    /// </summary>
    public enum eAssortmentActionType
    {
        Redo = 802150,
        //RemoveHeader				= 802151,
        //DeleteAssortment			= 802152,
        CancelAssortment = 802153,
        SpreadAverage = 802154,
        //IndexToAverage			= 802155,	// TT#2 - stodd - replaced completely by spread average
        CreatePlaceholders = 802156,
        //Quantity					= 802157,
        BalanceAssortment = 802158,
        // Begin TT#2 - stodd - removed/obsolete
        //Insert					= 802159,
        //SizeNeed					= 802160,
        // End TT#2 - stodd - removed/obsolete
        ChargeCommitted = 802161,
        CancelCommitted = 802162,
        // Begin TT#1225 - stodd - e
        //ChargeIntransit				= 802163,
        //CancelIntransit				= 802164,
        // Begin TT#1225 - stodd - 
        // Begin TT#1487-MD - stodd - ASST - Fill Size Holes does not work on a PH.
        //OpenReview = 802165 //TT#793-MD-DOConnell - Ran balance size billaterally on a receipt header in an assortment and receive a null reference exception
        // End TT#1487-MD - stodd - ASST - Fill Size Holes does not work on a PH.
        CreatePlaceholdersBasedOnRevenue = 802170,
    }

    // Begin TT#1228 - stodd
    public enum eAssortmentSelectableActionType
    {
        StyleNeed = 802111,
        BreakoutSizesAsReceived = 802112,   // TT#1086 - MD - stodd - header selection ignored 
        BalanceStyleProportional = 802113,
        BalanceToDC = 802114,
        BalanceSizeWithSubs = 802115,
        BalanceSizeNoSubs = 802116,
        ChargeIntransit = 802117,
        Release = 802118,
        Reset = 802119,
        BackoutSizeIntransit = 802120,
        BackoutStyleIntransit = 802121,
        BackoutSizeAllocation = 802122,
        BackoutAllocation = 802123,
        // Begin TT#1086 - MD - stodd - header selection ignored 
        BackoutDetailPackAllocation = 802126,
        ChargeSizeIntransit = 802127,
        RemoveAPI_Workflow = 802134,
        ApplyAPI_Workflow = 802135,
        ReapplyTotalAllocation = 802142,
        BalanceSizeWithConstraints = 802143,
        BalanceSizeBilaterally = 802144,
        BreakoutSizesAsReceivedWithConstraints = 802145,
        // End TT#1086 - MD - stodd - header selection ignored 

        CancelAssortment = 802153,  // TT#1451-MD - stodd - On Assortment Content tab, cannot process Assortment actions unless a header/placeholder is selected. This is incorrect. 
        SpreadAverage = 802154,
        ChargeCommitted = 802161,
        CancelCommitted = 802162,
        OpenReview = 802165 //TT#793-MD-DOConnell - Ran balance size billaterally on a receipt header in an assortment and receive a null reference exception

    }

    public enum eAssortmentAllocHeaderOnlyActionType
    {
        //BEGIN TT#387 - MD - DOConnell - When processing actions the selected headers and placeholders are not being honored.
        //ChargeIntransit = 802117,
        //Release = 802118,
        //Reset = 802119,
        //BackoutSizeIntransit = 802120,
        //BackoutStyleIntransit = 802121,
        //// BEGIN TT#1936 - stodd - cancel allocation is for headers only
        //BackoutSizeAllocation = 802122,
        //BackoutAllocation = 802123
        //// END TT#1936 - stodd - cancel allocation is for headers only
        //END TT#387 - MD - DOConnell - When processing actions the selected headers and placeholders are not being honored.
    }
    // End TT#1228 - stodd


    /// <summary>
    /// This enum is a subset of the eMethodType and eAllocationMethodType
    /// </summary>
    /// <remarks>
    /// The types in this enum represent intransit update methods and actions.
    /// </remarks>
    public enum eAllocationMethodType_IT_Updt
    {
        ChargeIntransit = 802117,
        BackoutSizeIntransit = 802120,
        BackoutStyleIntransit = 802121,
        // BEGIN MID Track #2470 - Charge Size Intransit action is missing
        ChargeSizeIntransit = 802127, // TT#1137 (MID Track 4351) Rebuild Intransit Utility
                                      // END  MID Track #2470
        RebuildIntransit = 802133 // TT#1137 (MID Track 4351) Rebuild Intransit Utility
    }

    // Begin TT#1224 - stodd - assortment committed
    public enum eAssortmentMethodType_Committed_Updt
    {
        //BackoutSizeIntransit = 802120,
        //BackoutStyleIntransit = 802121,
        ChargeCommitted = 802161,
        CancelCommitted = 802162
    }
    // End TT#1224 - stodd - assortment committed


    //	/// <summary>
    //	/// This enum is a subset of the eMethodType
    //	/// </summary>
    //	/// <remarks>
    //	/// The types in this enum represent allocation methods and actions used in the Workflow.
    //	/// See also:  eAllocationMethodType.
    //	/// See also:  eAllocationActionType.
    //	/// </remarks>
    //	public enum eAllocationMethodActionType
    //	{
    //		GeneralAllocation			= 802102,
    //		AllocationOverride			= 802103,
    //		Velocity					= 802104,
    //		Rule					    = 802105,
    //		FillSizeHolesAllocation		= 802106,
    //		BasisSizeAllocation			= 802107,
    //		WarehouseSizeAllocation		= 802108,
    //		BreakoutSizesAsReceived     = 802125,
    //		SizeNeedAllocation			= 802109,
    //		SizeOverrideAllocation		= 802110,
    //		StyleNeed                   = 802112,
    //		BalanceStyleProportional    = 802113,
    //		BalanceToDC                 = 802114,
    //		BalanceSizeWithSubs         = 802115,
    //		BalanceSizeNoSubs           = 802116,
    //		ChargeIntransit             = 802117,
    //		Release                     = 802118,
    //	}

    public enum eAllocationActionStatus
    {
        NoActionPerformed = 802250,
        ActionCompletedSuccessfully = 802251,
        ActionFailed = 802252,
        VelocityBasisError = 802253, // TT#241 - MD - JEllis - Header Enqueue Process
        HeaderEnqueueFailed = 802254, // TT#241 - MD - Jellis - Header Enqueue Process
        NoHeaderResourceLocks = 802255,  // TT#241 - MD - JEllis - Header Enqueue Process // TT#2671 - JEllis - Release Action Fails Yet Headers are released
        NotAllLinkedHeadersRlseApproved = 802256,  // TT#2671 - JEllis - Release Action Fails Yet Headers are released
        AllLinkedHeadersReleased = 802257   // TT#2671 - Jellis - Release Action Fails Yet Headers are released

    }


    public enum eOTSPlanActionStatus
    {
        NoActionPerformed = 802260,
        ActionCompletedSuccessfully = 802261,
        ActionFailed = 802262
    }

    public enum eFillSizeHolesSort
    {
        Ascending = 802600,
        Descending = 802601
    }

    public enum eBasisSizeSort
    {
        Ascending = 802610,
        Descending = 802611
    }

    // Begin TT#1966-MD - JSmith- DC Fulfillment
    public enum eDCFulfillmentHeadersOrder
    {
        Ascending = 900976,
        Descending = 900977
    }

    public enum eDCFulfillmentStoresOrder
    {
        Ascending = 900981,
        Descending = 900982
    }

    public enum eDCFulfillmentSplitOption
    {
        DCFulfillment = 900972,
        Proportional = 900973
    }
    // End TT#1966-MD - JSmith- DC Fulfillment

    // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
    public enum eDCFulfillmentSplitByOption
    {
        SplitByStore = 900984,
        SplitByDC = 900985
    }
    public enum eDCFulfillmentReserve
    {
        ReservePreSplit = 900987,
        ReservePostSplit = 900988
    }
    public enum eDCFulfillmentMinimums
    {
        ApplyByQty = 900990,
        ApplyFirst = 900991
    }
    public enum eDCFulfillmentWithinDC
    {
        Fill = 900994,
        Proportional = 900995
    }
    // END TT#1966-MD - AGallagher - DC Fulfillment

    public enum eExternalEligibilityProductIdentifier
    {
        ID = 0,
        Name = 1,
        NameConcatColorName = 2
    }

    public enum eExternalEligibilityChannelIdentifier
    {
        ID = 0,
        Name = 1
    }

    //  Allocation Rule Types      802300 - 802399
    /// <summary>
    /// Enumerates the allocation rule types.
    /// </summary>
    /// <remarks>
    /// This is the master allocation rule type enum.  All rule types are represented here. The following enums are
    /// subsets of this master and indicate where enum value is valid:
    /// <list type="bullet">
    /// <item>eVelocityRuleType: Identifies the rules that may be assigned when using velocity.</item>
    /// <item>eBasisAllocationRuleType: Identifies the rules that may be assigned when using a basis allocation.</item>
    /// <item>eStoreFilterRuleType: Identifies the rules that may be assiged when using a store filter.</item>
    /// <item>eSizeStoreFilterRuleType: Identifies the size rules that may be assigned when using a store filter.</item>
    /// <item>eSizeNeedRuleType: Identifies the size rules that may be assigned when allocating by size need.</item>
    /// <item>eBasisSizeAllocationRuleType: Identifies the size rules that may be assigned when using a basis allocation to allocate sizes.</item>
    /// <item>eFillSizeHolesRuleType: Identifies the size rules that may be assigned when allocating sizes by Fill Size Holes.</item>
    /// </list></remarks>
    public enum eRuleType
    {
        WeeksOfSupply = 802300,
        ShipUpToQty = 802301,
        Exclude = 802302,
        AbsoluteQuantity = 802303,
        Minimum = 802304,
        AdMinimum = 802305,
        Maximum = 802306,
        ColorMinimum = 802307,
        ColorMaximum = 802308,
        ForwardWeeksOfSupply = 802309,
        ProportionalAllocated = 802310,
        Exact = 802311,
        Fill = 802312,
        SizeMinimum = 802313,
        SizeMinimumPlus1 = 802314,
        SizeMaximum = 802315,
        SizeFillUpTo = 802316,
        // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
        None = 802317
        //None                          = 802317,
        //MinimumBasis				    = 802318,
        //MaximumBasis				    = 802319,
        //AdMinimumBasis				= 802320
        // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
    }

    /// <summary>
    /// This is a subset of enumerations from eRuleType that are valid in Velocity.
    /// </summary>
    public enum eVelocityRuleType
    {
        WeeksOfSupply = 802300,
        ShipUpToQty = 802301,
        Out = 802302,
        AbsoluteQuantity = 802303,
        Minimum = 802304,
        AdMinimum = 802305,
        Maximum = 802306,
        ColorMinimum = 802307,
        ColorMaximum = 802308,
        ForwardWeeksOfSupply = 802309,
        // BEGIN TT#3433 - AGallagher - Velocity - Prior Header Rules do not display correctly in Velocity Store Detail with Reconcile
        ProportionalAllocated = 802310,
        Exact = 802311,
        Fill = 802312,
        // END TT#3433 - AGallagher - Velocity - Prior Header Rules do not display correctly in Velocity Store Detail with Reconcile
        // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
        None = 802317
        //None                          = 802317,
        //MinimumBasis				    = 802318,
        //MaximumBasis				    = 802319,
        //AdMinimumBasis				= 802320
        // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)

    }

    public enum eVelocityRuleRequiresQuantity
    {
        WeeksOfSupply = 802300,
        ShipUpToQty = 802301,
        AbsoluteQuantity = 802303,
        ForwardWeeksOfSupply = 802309
    }

    /// <summary>
    /// This is a subset of enumerations from eRuleType that are valid when using the Basis Allocation Method.
    /// </summary>
    public enum eRuleMethodType
    {
        Exclude = 802302,
        AbsoluteQuantity = 802303,
        Minimum = 802304,
        AdMinimum = 802305,
        Maximum = 802306,
        ColorMinimum = 802307,
        ColorMaximum = 802308,
        ProportionalAllocated = 802310,
        Exact = 802311,
        Fill = 802312,
        None = 802317

    }

    /// <summary>
    /// This is a subset of enumerations from eRuleType that are valid when using a store filter to allocate.
    /// </summary>
    public enum eStoreFilterRuleType
    {
        Exclude = 802302,
        AbsoluteQuantity = 802303,
        Minimum = 802304,
        AdMinimum = 802305,
        Maximum = 802306,
        ColorMinimum = 802307,
        ColorMaximum = 802308,
        None = 802317
    }

    /// <summary>
    /// This is a subset of enumerations from eRuleType that are valid when using a store filter to allocate a size.
    /// </summary>
    public enum eSizeStoreFilterRuleType
    {
        Exclude = 802302,
        AbsoluteQuantity = 802303,
        SizeMinimum = 802313,
        SizeMinimumPlus1 = 802314,
        SizeMaximum = 802315,
        None = 802317
    }

    // begin MID Track 4442 Min, Ad Min and Max only valid for Total Component
    /// <summary>
    /// Rules that are only valid on a total component  (see also eRuleMethodOnlyValidOnTotalComponent)
    /// </summary>
    public enum eRuleTypeOnlyValidOnTotalComponent
    {
        Minimum = 802304,
        AdMinimum = 802305,
        // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
        Maximum = 802306
        //Maximum                       = 802306,
        //MinimumBasis				    = 802318,
        //MaximumBasis			    	= 802319,
        //AdMinimumBasis				= 802320
        // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)

    }

    //	/// <summary>
    //	/// This is a subset of enumerations from eRuleType that are valid when using a basis allocation to allocate sizes.
    //	/// </summary>
    //	public enum eBasisSizeAllocationRuleType
    //	{
    //		Exclude                     = 802302,
    //		AbsoluteQuantity            = 802303,
    //		ProportionalAllocated       = 802310,
    //		Exact                       = 802311,
    //		Fill                        = 802312,
    //		SizeMinimum                 = 802313,
    //		SizeMaximum                 = 802315,
    //		None                        = 802317
    //	}

    /// <summary>
    /// This is a subset of enumerations from eRuleType that are valid when using the Fill Size Holes method.
    /// </summary>
    public enum eFillSizeHolesRuleType
    {
        SizeMinimum = 802313,
        SizeMaximum = 802315,
        SizeFillUpTo = 802316,
        Exclude = 802302
    }

    /// <summary>
    /// This is a subset of enumerations from eRuleType that are valid when using the Basis Size method.
    /// </summary>
    public enum eBasisSizeMethodRuleType
    {
        Exclude = 802302,
        ProportionalAllocated = 802310,
        Exact = 802311,
        Fill = 802312,
        AbsoluteQuantity = 802303
    }

    public enum eBasisSizeRuleRequiresQuantity
    {
        AbsoluteQuantity = 802303
    }


    /// <summary>
    /// This is a subset of enumerations from eRuleType that are valid when using the Size Override method.
    /// </summary>
    public enum eSizeRuleType     // MID Track Remove Fringe
    {
        Exclude = 802302,
        SizeMinimum = 802313,
        SizeMaximum = 802315,
        SizeMinimumPlus1 = 802314,
        AbsoluteQuantity = 802303,   // MID Track 3781 Size Curve not required
        None = 802317    // MID Track 3781 Size Curve not required

    }

    /// <summary>
    /// This is a subset of enumerations from eRuleType that are valid when allocating by size need.
    /// </summary>
    public enum eSizeNeedRuleType
    {
        SizeMinimum = 802313,
        SizeMinimumPlus1 = 802314,
        SizeMaximum = 802315,
        None = 802317
    }

    public enum eChosenRuleAllowsMoreAllocation
    {
        Minimum = 802304,
        AdMinimum = 802305,
        ColorMinimum = 802307,
        // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
        None = 802317
        // None                        = 802317,
        // MinimumBasis				   = 802318,
        // Begin TT # 91 - stodd
        //MaximumBasis				   = 802319,
        // End TT # 91 - stodd
        // AdMinimumBasis			   = 802320
        // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)

    }

    // End of the Rule Type enumerations

    public enum eModifySalesRuleType
    {
        None = 802430,
        SalesModifier = 802431,
        SalesIndex = 802432,
        PlugSales = 802433,
        StockToSalesRatio = 802434,
        StockToSalesIndex = 802435,
        StockToSalesMinmum = 802436,
        StockToSalesMaximum = 802437
    }

    public enum eModifySalesRuleRequiresQty
    {
        SalesModifier = 802431,
        SalesIndex = 802432,
        PlugSales = 802433,
        StockToSalesRatio = 802434,
        StockToSalesIndex = 802435,
        StockToSalesMinmum = 802436,
        StockToSalesMaximum = 802437
    }


    public enum eEquateOverrideSizeType
    {
        Dimensions = 802690,
        Size = 802691,
        DimensionSize = 802692
    }

    /// <summary>
    /// This lists the Types of Views for the Allocation Selection
    /// Note:  The values are also MIDTextType values
    /// </summary>
    public enum eAllocationSelectionViewType
    {
        None = 0,
        Style = eMIDTextType.eAllocationStyleGroupBy,
        Size = eMIDTextType.eAllocationSizeGroupBy,
        Summary = eMIDTextType.eAllocationSummaryGroupBy,
        //Begin Assortment Planning - JScott - Assortment Planning Changes
        //Velocity            = eMIDTextType.eAllocationVelocityGroupBy
        Velocity = eMIDTextType.eAllocationVelocityGroupBy,
        Assortment = eMIDTextType.eAllocationAssortmentGroupBy,
        //End Assortment Planning - JScott - Assortment Planning Changes
        GroupAllocation = eMIDTextType.eGroupAllocationGroupBy
    }
    /// <summary>
    /// This lists the GroupBy options for the allocation Style view
    /// </summary>
    public enum eAllocationStyleViewGroupBy
    {
        Header = 803001,
        Components = 803002
    }

    /// <summary>
    /// This lists the GroupBy options for the allocation Size view
    /// </summary>
    public enum eAllocationSizeViewGroupBy
    {
        Header = 803021,
        Color = 803022
    }

    /// <summary>
    /// This lists the 2nd GroupBy options for the allocation Size view
    /// </summary>
    public enum eAllocationSizeView2ndGroupBy
    {
        Variable = 803031,
        Size = 803032
    }

    /// <summary>
    /// This lists the GroupBy options for the allocation Summary view
    /// </summary>
    public enum eAllocationSummaryViewGroupBy
    {
        Attribute = 803041,
        StoreGrade = 803042
    }

    /// <summary>
    /// This lists the GroupBy options for the allocation Assortment view
    /// </summary>
    public enum eAllocationAssortmentViewGroupBy
    {
        None = 0,
        Attribute = 803051,
        StoreGrade = 803052
    }

    /// <summary>
    /// Enumeration of the valid allocation wafer variables
    /// </summary>
    /// <remarks>eAllocationVelocityViewVariable, eAllocationVelocityVariableDefault, eAllocationStyleViewVariable and eAllocationSummaryViewVariable are all subsets of this enumeration</remarks>
    public enum eAllocationWaferVariable
    {
        //begin tt#153 - Velocity balance - apicchetti
        VelocityInitialRuleQty = 804300,
        VelocityInitialRuleType = 804301,
        VelocityInitialWillShip = 804302,

        //end tt#153 - Velocity balance - apicchetti

        AvgWeeksOfSupply = 804303,   // TT#508 - AGallagher - Velocity - Add WOS to Store Detail (#66)
        VelocityRuleTypeQty = 804304,   // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
        VelocityInitialRuleTypeQty = 804305,   // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
        BasisGrade = 804306,   // TT#638 - RMatelic - Style Review - Add Basis Variables 
        StoreItemQuantityAllocated = 804307,   // TT#1401 - AGallagher - Reservation Stores
        StoreIMOQuantityAllocated = 804308,   // TT#1401 - AGallagher - Reservation Stores
        StoreIMOMaxQuantityAllocated = 804309,   // TT#1401 - AGallagher - Reservation Stores
        StoreIMOHistoryMaxQuantityAllocated = 804310,   // TT#1401 - AGallagher - Reservation Stores
        SizeVSWOnHand = 804313,  // TT#1401 - AGallagher - VSW

        None = 804500,
        OriginalQuantityAllocated = 804501,
        QuantityAllocated = 804502,
        Need = 804503,
        PercentNeed = 804504,
        OnHand = 804505,
        Sales = 804506,
        Stock = 804507,
        InTransit = 804508,
        StoreGrade = 804509,
        AppliedRule = 804510,
        RuleResults = 804511,
        OpenToShip = 804512,
        OTSVariance = 804513,
        QtyReceived = 804514,
        StoreCount = 804515,
        AverageStore = 804516,
        Balance = 804517,
        Total = 804518,
        AvgWeeklySales = 804519,
        AvgWeeklyStock = 804520,
        BasisInTransit = 804521,
        BasisOnHand = 804522,
        BasisSales = 804523,
        BasisStock = 804524,
        BasisVSWOnHand = 804321,    //TT#4262-VStuart-Velocity-VSW On Hand at the plan level is used when calculating ship up to, wos and fwos-MID
        PctSellThru = 804525,
        VelocityGrade = 804526,
        VelocityRuleType = 804527,
        VelocityRuleQty = 804528,
        VelocityRuleResult = 804529,
        PctSellThruIdx = 804530,
        StyleOnHand = 804531,
        StyleInTransit = 804532,
        Transfer = 804533,
        SizeTotalAllocated = 804534,
        SizeInTransit = 804535,
        SizeOnHand = 804536,
        SizeOnHandPlusIT = 804537,
        SizePlan = 804538,
        SizeCurvePct = 804539,
        SizeNeed = 804540,
        SizePositiveNeed = 804541,
        SizePctNeed = 804542,
        SizePositivePctNeed = 804543,
        PctToTotal = 804544,
        CurrentWeekToDaySales = 804545,
        SizeSales = 804546,
        CurveAdjdSizeOnHand = 804547, // MID Track 3209 Show "actual" IT and OH on Size Review
        CurveAdjdSizeInTransit = 804548, // MID Track 3209 Show "actual" IT and OH on Size Review
        CurveAdjdSizeOnHandPlusIT = 804549, // MID Track 3209 Show "actual" IT and OH on Size Review
        CurveAdjdSizeNeed = 804550, // MID Track 3209 Show "actual" IT and OH on Size Review
        CurveAdjdSizePctNeed = 804551, // MID Track 3209 Show "actual" IT and OH on Size Review
        CurveAdjdSizePosNeed = 804552, // MID Track 3209 Show "actual" IT and OH on Size Review
        CurveAdjdSizePosPctNeed = 804553, // MID Track 3209 Show "actual" IT and OH on Size Review
        CurveAdjdSizePlan = 804554, // MID Track 3209 Show "actual" OnHand and IT in Size Review
        CurveAdjdSizeCurvePct = 804555, // MID Track 3209 Show actual OH and IT on Size Review
        ShipToDay = 804556, // MID Track 3880 Add Ship To Day Variable to Style and Size Review
        NeedDay = 804557, // MID Track 4291 Add Fill Variables to Size Review
                          // begin MID track 4921 AnF#666 Fill to Size Plan Enhancement
        FillSizeOwnPlan = 804558, // MID Track 4291 Add Fill Variables to Size Review
        FillSizeOwnNeed = 804559, // MID Track 4291 Add Fill Variables to Size Review
        FillSizeOwnPctNeed = 804560,  // MID Track 4291 Add Fill Variables to Size Review
        PreSizeAllocated = 804561, // MID Traack 4282 Velocity overlays Fill Size Holes allocation
        FillSizeFwdForecastSales = 804562,
        FillSizeFwdForecastStock = 804563,
        FillSizeFwdForecastPlan = 804564,
        FillSizeFwdForecastNeed = 804565,
        FillSizeFwdForecastPctNeed = 804566, // MID Track 6079 Zero Quantity not accepted after Sort
                                             // end MID track 4921 AnF#666 Fill to Size Plan Enhancement
                                             // begin MID Track 6079 Zero Quantity not accepted after Sort
        SortSequence = 804567,  // TT#59 Implement Temp Locks
        // end MID Track 6079 Zero Quantity not accepted after Sort
        // begin TT#59 Implement Store Temp Locks
        StorePriority = 804568,
        AvailableCapacity = 804569,
        CapacityExceedByPct = 804570,
        CapacityMaximum = 804571,
        CapacityMaximumReached = 804572,
        StoreMayExceedCapacity = 804573,
        StoreMayExceedMaximum = 804574,
        StoreUsedCapacity = 804575,
        StorePercentNeedLimitReached = 804576,
        AllocationFromBottomUpSize = 804577,
        AllocationFromSizeBreakout = 804578,
        AllocationFromPackNeed = 804579,
        AllocationModifiedAftMultiSplit = 804580,
        StoreColorMaximum = 804581,
        StoreColorMinimum = 804582,
        StoreFilledSizeHoles = 804583,
        StoreHadNeed = 804584,
        StoreManuallyAllocated = 804585,
        StoreMaximum = 804586,
        StoreMinimum = 804587,
        RuleAllocationFromChild = 804588,
        RuleAllocationFromChosenRule = 804589,
        RuleAllocationFromParent = 804590,
        ShippingStatus = 804591,
        UnitNeedBefore = 804592,
        WasAutoAllocated = 804593,
        QtyAllocatedByAuto = 804594,
        QtyAllocatedByRule = 804595,
        QtyShipped = 804596,
        PercentNeedBefore = 804597,
        AssortmentGrade = 804598,  // BEGIN TT#2066-MD - AGallagher - Ship to Date validation.  Is this how it should be working
                                   // end TT#59 Implement Store Temp Locks
        Committed = 804599          // TT#1224 - stodd - add comitted
    }

    /// <summary>
    /// Enumeration of the secondary wafer variables
    /// </summary>
    public enum eAllocationSecondaryWaferVariable
    {
        None = 804500,
        StoreCount = 804515,
        AverageStore = 804516,
        Total = 804518,
        PctToTotal = 804544
    }

    /// <summary>
    /// Enumeration of the valid Velocity View Variables
    /// </summary>
    /// <remarks>This enumberation is a subset of the eAllocationWaferVariable. eAllocationVelocityViewVariableDefault is a subset of this enumeration.</remarks>
    public enum eAllocationVelocityViewVariable
    {
        AvgWeeksOfSupply = 804303,   // TT#508 - AGallagher - Velocity - Add WOS to Store Detail (#66)
        VelocityRuleTypeQty = 804304,   // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
        VelocityInitialRuleTypeQty = 804305,   // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
        StoreItemQuantityAllocated = 804307,   // TT#1401 - AGallagher - Reservation Stores
        StoreIMOQuantityAllocated = 804308,   // TT#1401 - AGallagher - Reservation Stores
        StoreIMOMaxQuantityAllocated = 804309,   // TT#1401 - AGallagher - Reservation Stores
        StoreIMOHistoryMaxQuantityAllocated = 804310,   // TT#1401 - AGallagher - Reservation Stores
        SizeVSWOnHand = 804313,  // TT#1401 - AGallagher - VSW
        None = 804500,
        QuantityAllocated = 804502,
        Need = 804503,
        PercentNeed = 804504,
        OnHand = 804505,
        Sales = 804506,
        Stock = 804507,
        InTransit = 804508,
        StoreGrade = 804509,
        AppliedRule = 804510,
        RuleResults = 804511,
        AvgWeeklySales = 804519,
        AvgWeeklyStock = 804520,
        BasisInTransit = 804521,
        BasisOnHand = 804522,
        BasisSales = 804523,
        PctSellThru = 804525,
        VelocityGrade = 804526,
        VelocityRuleType = 804527,
        VelocityRuleQty = 804528,
        VelocityRuleResult = 804529,
        PctSellThruIdx = 804530,
        StyleOnHand = 804531,
        StyleInTransit = 804532,
        // Begin MID Track #2446   Current Week to Day Sales values not showing in Velocity detail
        Transfer = 804533,
        CurrentWeekToDaySales = 804545,
        PreSizeAllocated = 804561,  // MID Traack 4282 Velocity overlays Fill Size Holes allocation
        AssortmentGrade = 804598   // BEGIN TT#2066-MD - AGallagher - Ship to Date validation.  Is this how it should be working
                                   // End MID Track #2446
    }

    /// <summary>
    /// Enumeration of the default velocity view variables
    /// </summary>
    /// <remarks>This enumeration is a subset of the eAllocationVelocityViewVariable.</remarks>
    public enum eAllocationVelocityViewVariableDefault
    {
        AvgWeeksOfSupply = 804303,  // TT#508 - AGallagher - Velocity - Add WOS to Store Detail (#66)
        VelocityRuleTypeQty = 804304,  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
        None = 804500,
        QuantityAllocated = 804502,
        Need = 804503,
        PercentNeed = 804504,  // MID track 6173 Add Percent Need to Velocity Default Columns
        AppliedRule = 804510,
        RuleResults = 804511,
        AvgWeeklySales = 804519,
        AvgWeeklyStock = 804520,
        BasisInTransit = 804521,
        BasisOnHand = 804522,
        BasisVSWOnHand = 804321,    //TT#4262-VStuart-Velocity-VSW On Hand at the plan level is used when calculating ship up to, wos and fwos-MID
        BasisSales = 804523,
        PctSellThru = 804525,
        VelocityGrade = 804526,
        VelocityRuleType = 804527,
        VelocityRuleQty = 804528,
        VelocityRuleResult = 804529,
        //PctSellThruIdx            = 804530, 
        StyleOnHand = 804531,
        StyleInTransit = 804532
    }

    /// <summary>
    /// Enumeration of the valid Style View Variables
    /// </summary>
    /// <remarks>
    /// This enumeration is a subset of the eAllocationWaferVariable.
    /// </remarks>
    public enum eAllocationStyleViewVariable
    {
        BasisGrade = 804306,   // TT#638 - RMatelic - Style Review - Add Basis Variables  
        None = 804500,
        QuantityAllocated = 804502,
        Need = 804503,
        PercentNeed = 804504,
        OnHand = 804505,
        Sales = 804506,
        Stock = 804507,
        InTransit = 804508,
        StoreGrade = 804509,
        AppliedRule = 804510,
        RuleResults = 804511,
        OpenToShip = 804512,
        QtyReceived = 804514,
        Balance = 804517,
        Total = 804518,
        AvgWeeklySales = 804519,    // Begin TT#638 - RMatelic - Style Review - Add Basis Variables 
        AvgWeeklyStock = 804520,
        BasisSales = 804523,    // End TT#638  
        ShipToDay = 804556, // MID Track 3880 Add Ship To Day Variable to Style and Size Review
        NeedDay = 804557, // MID Track 4291 Add Fill Variables to Size Review
        AssortmentGrade = 804598   // BEGIN TT#2066-MD - AGallagher - Ship to Date validation.  Is this how it should be working
    }

    public enum eAllocationStyleViewVariableDefault
    {
        None = 804500,
        QuantityAllocated = 804502,
        Need = 804503,
        PercentNeed = 804504,
        OnHand = 804505,
        Sales = 804506,
        Stock = 804507,
        InTransit = 804508,
        StoreGrade = 804509,
        OpenToShip = 804512,
        QtyReceived = 804514,
        Balance = 804517,
        Total = 804518
    }


    /// <summary>
    /// Enumeration of the valid Summary View Variables
    /// </summary>
    /// <remarks>This enumeration is a subset of the eAllocationWaferVariable</remarks>
    public enum eAllocationSummaryViewVariable
    {
        None = 804500,
        QuantityAllocated = 804502,
        OnHand = 804505,
        Sales = 804506,
        Stock = 804507,
        InTransit = 804508,
        OpenToShip = 804512,
        OTSVariance = 804513,
        QtyReceived = 804514,
        StoreCount = 804515,
        AverageStore = 804516,
        Balance = 804517,
        Total = 804518
    }

    /// <summary>
    /// Enumeration of the valid allocation quick filter variables
    /// </summary>
    /// <remarks>This enumeration is a subset of eAllocationWaferVariable</remarks>
    public enum eAllocationQuickFilterVariable
    {
        //OriginalQuantityAllocated = 804501,
        QuantityAllocated = 804502,
        Need = 804503,
        PercentNeed = 804504
        //OnHand                    = 804505,
        //Sales                     = 804506,
        //Stock                     = 804507,
        //InTransit                 = 804508,
        //StoreGrade                = 804509,
        //AppliedRule               = 804510,
        //RuleResults               = 804511,
        //OpenToShip                = 804512,
        //AvgWeeklySales            = 804519,
        //AvgWeeklyStock            = 804520,
        //BasisInTransit            = 804521,
        //BasisOnHand               = 804522,
        //BasisSales                = 804523,
        //BasisStock                = 804524, 
        //PctSellThru               = 804525,
        //VelocityGrade             = 804526,
        //VelocityRuleType          = 804527,
        //VelocityRuleQty           = 804528,
        //VelocityRuleResult        = 804529,
        //PctSellThruIdx            = 804530, 
        //StyleOnHand               = 804531,
        //StyleInTransit            = 804532,
        //Transfer                  = 804533,
        //SizeTotalAllocated        = 804534,
        //SizeInTransit             = 804535,
        //SizeOnHand                = 804536,
        //SizeOnHandPlusIT          = 804537,
        //SizePlan                  = 804538,
        //SizeCurvePct              = 804539,
        //SizeNeed                  = 804540,
        //SizePositiveNeed          = 804541,
        //SizePctNeed               = 804542,
        //SizePositivePctNeed       = 804543
    }

    /// <summary>
    /// Enumeration of the valid allocation size variables
    /// </summary>
    /// <remarks>This enumeration is a subset of eAllocationWaferVariable</remarks>
	public enum eAllocationSizeViewVariable
    {
        None = 804500,
        OriginalQuantityAllocated = 804501,
        QuantityAllocated = 804502,
        StoreGrade = 804509,
        SizeTotalAllocated = 804534,
        SizeInTransit = 804535,
        SizeOnHand = 804536,
        SizeOnHandPlusIT = 804537,
        SizePlan = 804538,
        SizeCurvePct = 804539,
        SizeNeed = 804540,
        SizePositiveNeed = 804541,
        SizePctNeed = 804542,
        SizePositivePctNeed = 804543,
        SizeSales = 804546,
        ShipToDay = 804556, // MID Track 3880 Add Ship To Day Variable to Style and Size Review
        NeedDay = 804557,  // MID Track 4291 Add Fill Variables to Size Review
                           // begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement
        FillSizeOwnPlan = 804558, // MID Track 4291 Add Fill Variables to Size Review
        FillSizeOwnNeed = 804559, // MID Track 4291 Add Fill Variables to Size Review
        FillSizeOwnPctNeed = 804560,  // MID Track 4291 Add Fill Variables to Size Review
        FillSizeFwdForecastSales = 804562,
        FillSizeFwdForecastStock = 804563,
        FillSizeFwdForecastPlan = 804564,
        FillSizeFwdForecastNeed = 804565,
        FillSizeFwdForecastPctNeed = 804566
        // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement
    }

    public enum eAllocationSizeViewVariableDefault
    {
        None = 804500,
        QuantityAllocated = 804502,
        SizeTotalAllocated = 804534,
        // BEGIN MID Track #2519 - Remove unused variables from the column chooser 
        // SizeOnHandPlusIT       = 804537  
        // END MID Track #2519

    }

    public enum eAllocationSizeNeedAnalysisVariableDefault
    {
        None = 804500,
        SizeOnHandPlusIT = 804537
    }

    // begin TT#173  Provide database container for large data collections
    /// <summary>
    /// Storage Type Codes 
    /// </summary>
    [Serializable]
    public enum eMIDStorageTypeCode
    {
        // Note:  these values DO NOT correspond to the System.TypeCode values
        // Int64 is largest supported MID numeric type within a Store Variable Container (StoreVariableVector
        // since Min & Max values are expressed as this type.
        // UInt64 is valid only when Int128 can be used to express these 2 values in which case it will 
        // be the largest supported numeric type within a Store Variable Container.
        Empty = 0,
        typeByte = 1,
        typeSByte = 2,
        typeUshort = 3,
        typeShort = 4,
        typeUint = 5,
        typeInt = 6,
        typeLong = 7
    }

    // begin TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4
    /// <summary>
    /// Enumeration of the Hierarchy Levels for a header
    /// </summary>
    public enum HeaderHierarchyLevel
    {
        Style = 0,
        Color = 1,
        Size = 2
    }
    // end TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4

    /// <summary>
    /// Enumeration of the valid Database variables for Store Allocation
    /// </summary>
    public enum eAllocationDatabaseStoreVariables
    {
        DetailAuditFlags = 805101,
        Minimum = 805102,
        Maximum = 805103,
        PrimaryMaximum = 805104,
        UnitsAllocated = 805105,
        PacksAllocated = 805106,
        UnitsAllocatedByAuto = 805107,
        PacksAllocatedByAuto = 805108,
        UnitsAllocatedByRule = 805109,
        PacksAllocatedByRule = 805110,
        ShipStatusFlags = 805111,
        UnitsShipped = 805112,
        PacksShipped = 805113,
        ChosenRuleType = 805114,
        ChosenRuleLayer = 805115,
        ChosenRuleUnits = 805116,
        ChosenRulePacks = 805117,
        NeedDay = 805118,
        UnitNeedBefore = 805119,
        UnitPlanBefore = 805120,
        GeneralAuditFlags = 805121,
        GradeIndex = 805122,
        ShipToDay = 805123,
        CapacityUnits = 805124,
        CapacityExceedByPercent = 805125, // TT#2225 - JEllis - AnF VSW FWOS Enhancement
        VswReverseOnhandUnits = 805126 // TT#2225 - JEllis - AnF VSW FWOS Enhancement
    }
    public enum eForecastBaseDatabaseStoreVariables
    {
        SalesTotal = 806101,
        SalesRegular = 806102,
        SalesPromo = 806103,
        SalesMarkdown = 806104,
        StockTotal = 806105,
        StockRegular = 806106,
        StockMarkdown = 806107,
        // begin TT#391 - stodd - size day to week summary
        InStockSales = 806108,
        InStockSalesReg = 806109,
        InStockSalesPromo = 806110,
        InStockSalesMkdn = 806111,
        AccumSellThruSales = 806112,
        AccumSellThruStock = 806113,
        DaysInStock = 806114,
        ReceivedStock = 806115
        // End TT#391 - stodd - size day to week summary
    }
    public enum eForecastCustomDatabaseStoreVariables
    {
        Not_A_Variable = 807100
    }
    /// <summary>
    /// Enumeration of the valid key types for a database bin table
    /// </summary>
    /// <remarks>See also:  DatabaseBinKey, its derived classes (HistoryDatabaseBinKey, HeaderDatabaseBinKey, etc.) and DatabaseBinTable[T]</remarks>
    public enum eDatabaseBinKeyType
    {
        None = 0,
        HistoryDatabaseBinKey = 1,
        HeaderTotalDatabaseBinKey = 2,
        HeaderDetailDatabaseBinKey = 3,
        HeaderBulkDatabaseBinKey = 4,
        HeaderPackDatabaseBinKey = 5,
        HeaderColorDatabaseBinKey = 6,
        HeaderColorSizeDatabaseBinKey = 7,      // TT#370 Build Pack Enhancement
        HeaderSummaryDatabaseBinKey = 8,       // TT#370 Build Pack Enhancement // TT#2225 - JEllis - AnF VSW FWOS Enhancement
        VswReverseOnhandDatabaseBinKey = 9       // TT#2225 - JEllis - AnF VSW FWOS Enhancement
    }
    /// <summary>
    /// Enumeration of the valid Header Key types for database bin tables where only the HdrRID is the key
    /// </summary>
    public enum eDatabaseHeaderBinKeyType
    {
        HeaderTotalDatabaseBinKey = 2,
        HeaderDetailDatabaseBinKey = 3,
        HeaderBulkDatabaseBinKey = 4
    }
    /// <summary>
    /// Variable Model Type Codes used as the key to identify the MID Variable Model definitions for variables
    /// </summary>
    [Serializable]
    public enum eMIDVariableModelType
    {
        TotalAllocationVariableModelType = 1,
        DetailAllocationVariableModelType = 2,
        BulkAllocationVariableModelType = 3,
        PackAllocationVariableModelType = 4,
        ColorAllocationVariableModelType = 5,
        SizeAllocationVariableModelType = 6,
        HistoryVariableModelType = 7,          // TT#370 Build Packs Enhancement
        AllocationArchiveModelType = 8,         // TT#370 Build Packs Enhancement // TT#2225 - JEllis - AnF VSW FWOS Enhancement
        VswReverseOnhandModelType = 9           // TT#2225 - JEllis - AnF VSW FWOS Enhancement

    }
    // end TT#173  Provide database container for large data collections

    //Begin Assortment Planning - JScott - Assortment Planning Changes
    public enum eAssortmentComponentVariables
    {
        Assortment = 804600,
        PlanLevel = 804601,
        Placeholder = 804602,
        // Begin TT#1489 - RMatelic - Add Hierarchy levels above style as default displayable items
        HierarchyLevel = 804603,
        // End TT#1489
        HeaderID = 804604,
        Color = 804605,
        Pack = 804606,
        Style = 804607,
        // Begin TT#1227 - stodd - sort seq
        PlaceholderSeq = 804608,
        HeaderSeq = 804609,
        // Characteristic should always be last
        Characteristic = 804610,
        // End TT#1227 - stodd - sort seq
        ColorSeq = 804611, //TT#664 - MD - DOConnell - Placeholder colors display in reverse order on the Content Tab
    }

    public enum eAssortmentTotalVariables
    {
        TotalPct = 804620,
        TotalUnits = 804621,
        HeaderUnits = 804622,
        ReserveUnits = 804623,
        AvgUnits = 804624,
        TotalRetail = 804625,
        TotalCost = 804626,
        UnitRetail = 804627,
        UnitCost = 804628,
        MUPct = 804629,
        Balance = 804630,
        OnHand = 804631,
        Intransit = 804632,
        Multiple = 804633,
        Minimum = 804634,
        Maximum = 804635,
        Committed = 804637,
        NumStoresAllocated = 804638,    // TT#4294 - stodd - Average Units in Matrix Enahancement
    }

    public enum eAssortmentDetailVariables
    {
        TotalPct = 804650,
        TotalUnits = 804651,
        TotalUnitsPctToSet = 804653,
        TotalUnitsPctToAll = 804654,
        AvgUnits = 804655,
        Index = 804656,
        UnitRetail = 804657,
        UnitCost = 804658,
        NumStoresAllocated = 804659,    // TT#4294 - stodd - Average Units in Matrix Enahancement
    }

    public enum eAssortmentSummaryVariables
    {
        Units = 804680,
        NumStores = 804681,
        AvgStore = 804682,
        AvgUnits = 804683,
        Index = 804684,
        Basis = 804685,
        Need = 804686,
        PctNeed = 804687,
        Intransit = 804688,
        Committed = 804689,
        OnHand = 804690,		// TT#845-MD - Stodd - add OnHand to Summary
        VSWOnHand = 804691,		// TT#952 - MD - stodd - add matrix to Group Allocation Review
        Balance = 804692,       // TT#3817 - stodd - Saving View with Balance checked -
    }

    //End Assortment Planning - JScott - Assortment Planning Changes

    //Start Assortment changes - stodd 
    public enum eAssortmentQuantityVariables
    {
        Value = 1,
        Balance = 2,
        // BEGIN TT#2148 - stodd - Assortment totals do not include header values
        Difference = 3,
        Total = 4
        // END TT#2148 - stodd - Assortment totals do not include header values
    }

    public enum eAssortmentVariableType
    {
        None,
        Sales,
        Stock,
        Receipts,
        Intransit,
        Onhand,
        Need
    }
    //End Assortment changes - stodd 

    //Begin Assortment changes - rmatelic
    public enum eAssortmentType
    {
        Undefined,
        PreReceipt,
        PostReceipt,
        GroupAllocation		// BEGIN TT#488-MD - Stodd - Group Allocation
    }
    //End Assortment changes - rmatelic 

    // Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only 
    public enum eAssortmentDisplayType
    {
        Buy = 802880,
        PostReceipt = 802881,

    }
    // End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only

    /// <summary>
    /// Enumeration of the valid allocation wafer variable formats
    /// </summary>
	public enum eAllocationWaferVariableFormat
    {
        None = 0,
        Number = 1,
        String = 2,
        eRuleType = 3
    }

    /// <summary>
    /// Enumeration of the positions where need parameters reside for ApplicationSessionTransation GetStoreOTS_Need method.
    /// </summary>
    public enum eStoreOTS_NeedParmPosition
    {
        InTransit = 0,
        OnHand = 1,
        Plan = 2,
        Need = 3,
        PctNeed = 4
    }

    public enum eHierarchyIDFormat
    {
        Unique = 800900,
        Combine = 800901
    }

    public enum ePurgeTimeframe
    {
        None = 800950,
        Days = 800951,
        Weeks = 800952
    }

    //	public enum eSecurityViews 
    //	{
    //		All							= 801190,
    //		Sales						= 801191,
    //		Stock						= 801192,
    //		Markdowns					= 801193,
    //		Intransit					= 801194,
    //		WeeksOfSupply				= 801195
    //	}

    public enum eForecastMonitorType
    {
        PercentContribution,
        TyLyTrend,
        CurrentTrend,
        AverageSales,
        Inventory
    }

    public enum eLoadType
    {
        TabDelimited,
        CommaDelimited,
        Xml
    }

    //Begin Enhancement - JScott - Export Method - Part 2
    public enum eExportType
    {
        CSV,
        XML,
        Excel
    }

    public enum eExportSplitType
    {
        None,
        Merchandise,
        NumEntries
    }

    //End Enhancement - JScott - Export Method - Part 2
    //Begin Track #4942 - JScott - Correct problems in Export Method
    public enum eExportDateType
    {
        Calendar,
        Fiscal
    }

    //End Track #4942 - JScott - Correct problems in Export Method

    public enum eMIDMenuItem
    {
        FileNew,
        FileSave,
        FileSaveAs,
        FileClose,
        FileExit,
        FileExport,
        EditCut,
        EditCopy,
        EditPaste,
        EditDelete,
        EditClear,
        EditFind,
        EditReplace,
        EditUndo,
        ToolsQuickFilter,
        ToolsTheme,              // MID Track #5006 - Add Theme to Tools menu 
        ToolsRestoreLayout       // Workspace Usability Enhancement
    }

    public enum eMIDMenuAction
    {
        Add,
        Enable,
        Disable,
        Hide,
        Show,
        // Begin TT#335 - JSmith - Menu should say Remove and not Delete
        //Remove
        Remove,
        Rename
        // End TT#335
    }

    // BEGIN TT#246-MD - AGallagher - VSW Size
    public enum eVSWSizeConstraints
    {
        None = 802370,
        InStoreSizeMinimum = 802371,
        ItemMaxIdealSize = 802372,
        CombinedIdealSizeMinimum = 802373
    }
    // END TT#246-MD - AGallagher - VSW Size

    // BEGIN TT#933-MD - AGallagher - Item Max vs. FWOS Max
    public enum eVSWItemFWOSMax
    {
        Default = 900885,
        Highest = 900886,
        Lowest = 900887
    }
    // END TT#933-MD - AGallagher - Item Max vs. FWOS Max

    public enum eMIDTextCode
    {
        Unassigned = 000000,
        // Summary text 000100-009999
        sum_Successful = 000100,
        sum_Failed = 000101,
        sum_Running = 000102,
        sum_InputFileNotFound = 000103,
        sum_InvalidNumberOfParameters = 000104,
        sum_UnexpectedTermination = 000105,

        //Database Messages 010000-019999

        //Services Messages 020000-024999
        systemError = 020000,
        cannotAccessAppServer = 020001,
        configFileError = 020002,
        loginFailed = 020003,
        msg_ControlServerNotFound = 020004,
        msg_ClientServerNotFound = 020005,
        msg_StoreServerNotFound = 020006,
        msg_HierarchyServerNotFound = 020007,
        msg_ApplicationServerNotFound = 020008,
        msg_UserRIDNotInitialized = 020009,
        msg_HierNameNotInitialized = 020010,
        msg_HierColorNotInitialized = 020011,
        msg_WrkflwMthdsNotInitialized = 020012,
        msg_StartedBuildingGlobal = 020013,
        msg_FinishedBuildingGlobal = 020014,
        msg_SchedulerServerNotFound = 020015,
        msg_DatabaseMismatch = 020016,
        msg_ContinueWithoutScheduler = 020017,
        msg_HeaderServerNotFound = 020018,
        msg_MustRestartServices = 020019,
        msg_ServicesMismatch = 020028,  // TT#195 MD - JSmith - Add environment authentication
        msg_ConfigurationMismatch = 020029,  // TT#195 MD - JSmith - Add environment authentication
        msg_ClientServiceMismatch = 020030,  // TT#195 MD - JSmith - Add environment authentication
        msg_ClientDatabaseMismatch = 020031,  // TT#195 MD - JSmith - Add environment authentication

        // MIDReaderWriterLock Messages 025000-029999

        msg_NoActionsPending = 025000,
        msg_InvalidWriterUnlock = 025001,
        msg_InvalidReaderUnlock = 025002,

        //Calendar Messages 030000-039999
        msg_CalendarYearNotFound = 030000,
        msg_CalendarPeriodNotFound = 030001,
        msg_CalendarWeekNotFound = 030002,
        msg_InvalidCalendarDateType = 030003,
        msg_NoRelativeDateRangeProfile = 030004,
        msg_CalendarDayNotFound = 030005,
        msg_InvalidCalendarDate = 030006,
        msg_PostingDateNotSet = 030007,
        msg_InvalidCalDateRangeProperty = 030008,
        msg_CalendarStartDateChange = 030009,
        msg_PeriodDefinitionBeingReplaced = 030010,
        msg_InvalidStartYear = 030011,
        msg_ExtremeStartYear = 030012,
        msg_InvalidNumberOfWeeks = 030013,
        msg_UndefinedCalendarDateRange = 030014,
        msg_DynamicToolTip = 030015,
        msg_StaticToolTip = 030016,
        msg_DynamicSwitchToolTip = 030017,
        msg_RecurringToolTip = 030018,
        msg_InvalidMethod2Parameters = 030019,
        msg_EnterDynamicSwtchDate = 030020,	// Issue 5171
        msg_MonthDefinitionBeingReplaced = 030021,
        msg_QuarterDefinitionBeingReplaced = 030022,
        msg_SeasonDefinitionBeingReplaced = 030023,
        msg_MissingSeasonDefinition = 030024,
        msg_MissingQuarterDefinition = 030025,
        msg_MismatchedSeasonQuarters = 030026,
        msg_MismatchedQuarterMonths = 030027,
        // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
        msg_DaysDynamicToCurrentDate = 030028,
        msg_DaysDynamicToPlanDate = 030029,
        msg_DaysDynamicToStoreOpenDate = 030030,
        msg_WeeksDynamicToCurrentDate = 030031,
        msg_WeeksDynamicToPlanDate = 030032,
        msg_WeeksDynamicToStoreOpenDate = 030033,
        msg_PeriodsDynamicToCurrentDate = 030034,
        msg_PeriodsDynamicToPlanDate = 030035,
        msg_PeriodsDynamicToStoreOpenDate = 030036,
        // End Track #5833
        msg_InvalidPeriodProfileKey = 030037,		/* Track #6018 stodd */
        msg_DateNotWithinMerchandiseCalendar = 030038,		/* Track #6018 stodd */
        msg_OnlyOneWeekForOperator = 030039,   // TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only


        // Size Group Messages 039000-039999 
        msg_LabelsNotUnique = 039000,
        msg_CantRetrieveSizeCode = 039001,
        msg_UnknownProductCategory = 039002,
        msg_PleaseSelectGroupToDelete = 039003,
        msg_DeleteSizeGroup = 039004,
        msg_SelectProductCategory = 039005,
        msg_SelectSizeGroup = 039006,
        msg_FillInSizeAndWidth = 039007,
        msg_AddSizeCodeQuery = 039008,
        msg_CorrectThisCode = 039009,
        msg_SelectSizeCode = 039010,
        msg_DuplicateSize = 039011,
        msg_DuplicateWidth = 039012,
        msg_SaveChanges = 039013,
        str_NoSecondarySize = 039014,
        msg_SizeExistsInCategory = 039015,
        msg_MustHaveAtLeaseOneSizeCode = 039016,
        msg_WantToFillInBlankCode = 039017,
        //Begin Track #3723 - JScott - Prevent blank size codes from being saved
        msg_NoSizeCodeFound = 039018,
        //End Track #3723 - JScott - Prevent blank size codes from being saved
        //Begin Track #3607 - JScott - Duplicate key during save of new Group
        msg_SizeGroupExists = 039019,
        //End Track #3607 - JScott - Duplicate key during save of new Group

        //General Edit Messages 040000-049999
        msg_DateRequired = 040000,
        msg_DateNotFound = 040001,
        msg_VersionRequired = 040002,
        msg_VersionNotFound = 040003,
        msg_InputFileNotFound = 040004,
        msg_NothingToPaste = 040005,
        msg_UnknownPasteFormat = 040006,
        msg_SavePendingChanges = 040007,
        msg_SearchComplete = 040008,
        msg_NameRequiredToSave = 040009,
        msg_SaveCanceled = 040010,
        msg_MustBeNumeric = 040011,
        msg_FieldIsRequired = 040012,
        msg_DuplicateName = 040013,
        msg_MustEqual100 = 040014,
        msg_FollowingErrors = 040015,
        msg_MustBeInteger = 040016,
        msg_BadDataInClipboard = 040017,
        msg_DeleteFailed = 040018,
        msg_CopyComplete = 040019,
        msg_DeleteComplete = 040020,
        msg_CopyStatus = 040021,
        msg_DeleteStatus = 040022,
        msg_Copying = 040023,
        msg_Deleting = 040024,
        msg_PercentComplete = 040025,
        msg_ErrorDeleting = 040026,
        msg_ContinueQuestion = 040027,
        msg_DeleteCancelled = 040028,
        msg_CopyCancelled = 040029,
        msg_PartialDelete = 040030,
        msg_PartialCopy = 040031,
        msg_FormNotFound = 040032,
        msg_ValueExceedsMaximum = 040033,
        msg_MustBeNonNegative = 040034,
        msg_NeedSearchRow = 040035,
        msg_SearchColumnNotFound = 040036,
        msg_ColumnCanNotBeApplied = 040037,
        msg_FormatInvalid = 040038,
        msg_NoActiveRow = 040039,
        msg_NewSizeGroup = 040040,
        msg_Applying = 040041,
        msg_ApplyStatus = 040042,
        msg_ApplyComplete = 040043,
        msg_MustBeUnique = 040044,
        msg_NotAllValuesSpread = 040045,
        msg_DataIsReadOnly = 040046,
        msg_ErrorsFoundReviewCorrect = 040047,
        msg_NotAuthorized = 040048,
        msg_NotAuthorizedForNode = 040049,
        msg_LockingForDelete = 040050,
        msg_EndNotGTBegin = 040051,
        msg_NonCompNotGTNew = 040052,
        msg_MustBePositiveInteger = 040053,
        msg_DataTypeMismatch = 040054,
        msg_InputInvalid = 040055,
        msg_ConfirmDeleteFilter = 040056,
        msg_OutdatedFilterInfo = 040057,
        msg_UnbalancedParenthesis = 040058,
        msg_SetOrStoreExpected = 040059,
        msg_UnmatchedParenthesis = 040060,
        msg_AndOrOrExpected = 040061,
        msg_OperatorExpected = 040062,
        msg_VQExpected = 040070,
        msg_VExpected = 040071,
        msg_UnexpectedEOL = 040072,
        msg_NoSetsAssigned = 040073,
        msg_NoStoresAssigned = 040074,
        msg_QuantityNotDefined = 040075,
        msg_HeaderIDRequired = 040076,
        msg_HeaderIDNotAllowed = 040077,
        msg_DateRangeNotDefined = 040078,
        msg_CorrespondingNotValid = 040079,
        msg_EntriesLessThanMinimum = 040080,
        msg_ConfirmDelete = 040081,
        msg_ConfirmRemove = 040082,
        msg_RemoveCancelled = 040083,
        msg_NoExcel = 040084,
        msg_MustBeBetween0And100 = 040085,
        msg_ValueCannotExceed100 = 040086,
        msg_FilterNotSaved = 040087,
        msg_ValueRequired = 040088,
        msg_ReplaceComplete = 040089,
        msg_SpecifyReplaceText = 040090,
        msg_GradeNotDefined = 040091,
        msg_StatusNotDefined = 040092,
        msg_QExpected = 040093,
        msg_VGExpected = 040094,
        msg_VSExpected = 040095,
        msg_DeleteFailedDataInUse = 040096,
        msg_ReestablishInheritance = 040097,
        msg_SmallerValuesFound = 040098,
        msg_BlankBeginMeansBlankEnd = 040099,
        msg_CannontBeBlank = 040100,
        msg_MerchandiseRequired = 040101,
        msg_MethodRequired = 040102,
        msg_MethodInvalid = 040103,
        msg_MethodInvalidMethodChanged = 040104,
        msg_MethodInvalidEdit = 040105,
        msg_ValueExceedsMultiple = 040106,
        msg_WorkflowRequired = 040107,
        msg_UnableToAddRow = 040108,
        msg_MerchandiseInvalid = 040109,
        msg_PerformanceWarning = 040110,
        msg_ConfirmMoveNode = 040111,
        msg_ConfirmCopyNode = 040112,
        msg_ConfirmShortCut = 040113,
        msg_SizeCatgPriSecNotUnique = 040114,
        msg_SizePrimaryRequired = 040115,
        msg_ProductLevelDelimiterEdit = 040116,
        msg_WorkflowFailedDueToTolerance = 040117,
        msg_IsInstalled = 040118,
        msg_IsNotInstalled = 040119,
        msg_IsTempInstalled = 040120,
        msg_SelectEntry = 040121,
        msg_CodeIsRequired = 040122,
        //Begin Track #3704 - JScott - To Length can be less than From Length on Hierarchy Properties
        msg_ToMustIsLessThanFrom = 040123,
        //End Track #3704 - JScott - To Length can be less than From Length on Hierarchy Properties
        //Begin Assortment Planning - Multi select
        msg_ShiftSelectMustHaveSameParent = 040124,
        //Begin Track #4010 - JScott - Add all forecast methods and workflows to the scheduler.
        msg_WorkflowOrMethodRequired = 040125,
        //End Track #4010 - JScott - Add all forecast methods and workflows to the scheduler.
        msg_RequiredBasisMissing = 040126,
        msg_MissingIndexUnitValues = 040127,
        msg_IndexOrUnitsNotBoth = 040128,
        msg_DeleteForUser = 040129,
        msg_DeleteFromWorkflow = 040130,
        msg_DeleteFromTasklist = 040131,

        msg_CleaningUpExplorer = 040132,
        msg_SearchInstructions = 040133,
        msg_SearchCriteria = 040134,
        msg_StoreGradesAlreadyExist = 040135,
        msg_UnitGradeBoundarySelected = 040136,
        msg_IndexGradeBoundarySelected = 040137,
        msg_CharacteristicValueExpected = 040138,
        msg_FileNameRequired = 040139,
        msg_DirectoryNotExists = 040140,
        msg_FileAlreadyExists = 040141,
        //End Assortment Planning
        msg_SellThruPctsAlreadyExist = 040142,
        // BEGIN MID Track #4370 - John Smith - FWOS Models
        msg_BalanceAttemptsExceeded = 040143,
        // END MID Track #4370
        msg_MaxGEMin = 040144,
        msg_StoreGradesDoNotMatch = 040145,
        msg_StoreGroupDoesNotMatch = 040146,
        msg_OnlyCopyCurrentSet = 040147,
        // BEGIN MID Track #4386 - Justin Bolles - Delete Views
        msg_ConfirmDeleteAllUserViews = 040148,
        // Begin - Abercrombie & Fitch #4411 - JSmith - Chain Forecasting
        msg_ComputationModelNotFound = 040149,
        // End - Abercrombie & Fitch #4411
        msg_ViewDeleteSuccess = 040150,
        // END MID Track #4386
        //Begin Track #4457 - JSmith - Add forecast versions
        msg_CannotDeleteCombinedVersion = 040151,
        msg_CannotDeleteStandardVersion = 040152,
        msg_CannotUseCombinedVersion = 040153,
        //End Track #4457
        //Begin Track #4815 - JSmith - #283-User (Security) Maintenance
        //Begin Track #4619 - JSmith - Add auto-upgrade
        msg_IncompatibleClient = 040154,
        //End Track #4619
        // Begin MID Track #4310 - JSmith - Store Grades not defined
        msg_StoreGradesNotFound = 040155,
        // End MID Track #4310
        msg_ConfirmRecover = 040156,
        msg_DeleteSuccessfulWithValue = 040157,
        msg_ConfirmAssign = 040158,
        msg_ConfirmUnassign = 040159,
        msg_SharedBy = 040160,
        //End Track #4815
        msg_FromToAreEqual = 040161,
        msg_AtLeastOneCheck = 040162,
        msg_EnterOrSelectValidItem = 040163,
        msg_NotAuthorizedForItem = 040164,
        msg_NameAlreadyExist = 040165,
        msg_CanNotSetOverrideModelInUse = 040166,
        // begin MID Track 5694 MA Enhancement Relieve IT by Header ID
        msg_RelieveAllStoresMustBeBoolean = 040167,
        msg_RelieveAllStoresDuplicatesNotAllowed = 040168,
        msg_RelieveAllStoresInvalidWhenNoHeaders = 040169,
        msg_RelieveAllStoresOverrideSetting = 040170,
        // end MID Track 5694 Enhancement Relieve IT by Header ID
        /* Track #5739 – KJohnson - Product Characteristic dragging problem */
        msg_MustClickApplyBeforeDrag = 040171,
        /* End Track #5739 */
        // Begin Track #4872 - JSmith - Global/User Attributes
        msg_AttributeNotAccessibleWarning = 040172,
        // End Track #4872
        // BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement
        msg_VariableAlreadyAdded = 040173,
        // END MID Track #5773
        /* Track #6047 – KJohnson – New message */
        msg_WeightCannotBeZeroOrNegative = 040174,
        /* End Track #6047 */
        /* Begin Track #5663 - JScott - Export to Excel when I type in the path to export to received and unauthorizedAccessException. */
        msg_FileNameIsDirectory = 040175,
        /* End Track #5663 - JScott - Export to Excel when I type in the path to export to received and unauthorizedAccessException. */
        // BEGIN MID Track #6336 - KJohnson - Header Load API Enhancement
        msg_hl_NoTypeSpecified = 040176,
        msg_hl_InvalidTypeSpecified = 040177,
        msg_hl_HeaderHasWorkflowAndMethods = 040178,
        msg_hl_HeaderHasInvalidMethodName = 040179,
        msg_hl_InvalidMethodName = 040180,
        msg_hl_InvalidActionSpecified = 040181,
        // END MID Track #6336
        msg_AtLeastOneOptionRequired = 040182, // TT#15 - Save allowed when required fields not specified
        // Begin TT#155 - JSmith - Size Curve Method
        msg_AtLeastOneBasisRequired = 040183,
        // End TT#155
        msg_InvalidCharAutoAddValue = 040184,	// TT# 166 - stodd
        msg_InvalidCharTypeDelimiter = 040185,	// TT# 166 - stodd
        msg_InvalidCharType = 040186,	// TT# 166 - stodd
        msg_AutoAddedCharacteristic = 040187,	// TT# 166 - stodd
        msg_InvalidAttributeAndSetEntry = 040188,   // Begin TT#209 - RMatelic - Unrelated to specific error  
        msg_InvalidAttributeName = 040189,
        msg_InvalidAttributeSetName = 040190,   // End TT#209
        msg_MustEnterValidValue = 040191,   // TT#274 - RMatelic - Unrelated to specific issue
        msg_AutoRefreshExplorer = 040192,   // TT#384 - JSmith - Error removing child from hierarchy
        msg_ErrorCreatingDirectory = 040193,	// TT#703 - Stodd - export method
        msg_NoItemsExceedsMaximum = 040194,  	// TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
        msg_InvalidUseCharacteristicTransaction = 040195,   // TT#1401 - stodd - reservation stores
        msg_InvalidHeaderCharacteristic = 040196,   // TT#1401 - stodd - reservation stores
        msg_DateConversionError = 040197,           // TT#1401 - stodd - reservation stores
        msg_HeaderAlreadyReleased = 040198,         // TT#1401 - stodd - reservation stores
        msg_HeaderNotInReceivedInBalance = 040199,          // TT#1401 - stodd - reservation stores

        msg_ViewSaveNameLength = 040201, // TT#1904 - DOConnell - Filter Error
        msg_DatabaseColumnSizeExceeded = 040202,  // TT#1921 - JSmith - Error during Hierarchy Reclass
        //Begin TT#1888 - DOConnell - Add feature to clear Infragistics Layouts
        msg_ResourcesReleased = 040203,
        msg_LayoutsDeleted = 040204,
        msg_LayoutDeleteScheduled = 040205,
        msg_CurrentLogon = 040206,
        msg_SchdLayoutClearProcessed = 040207,
        msg_ClearSuccess = 040208,
        //End TT#1888 - DOConnell - Add feature to clear Infragistics Layouts
        msg_PushToBackStockProcessed = 040209,          // TT#1401 - stodd - reservation stores
        msg_PushToBackStockSkipped = 040210,			// TT#1401 - stodd - reservation stores

        msg_NodeNotAffected = 040211,
        msg_ApplyPendingChanges = 040212,  // Development TT#8 - JSmith - Hold qty in last set entered or force Apply before changing Attribute set   
        msg_MessageReceived = 040213,  // TT#2307 - JSmith - Incorrect Stock Values
        msg_MessageListenerStarted = 040214,  // TT#2307 - JSmith - Incorrect Stock Values

        msg_TotAdjTo100 = 040216, //TT#280 - MD - DOConnell - Daily Percentages error messages are incorrect 
        msg_IsUseByTheFollowing = 040217, // TT#110-MD - RMatelic - In Use Tool 
        msg_ExplorerFolderBuildError = 040218, // TT#437-MD - JSmith - Client application will not start due to a missing item on an explorer (Null reference on Client App startup.) 
        msg_EmptyStoreSetsDeleted = 040222,		// TT#739-MD - STodd - delete stores

        msg_StoreQuantitiesNotEqual = 040219, //TT#1199 -MD- RBeck - NEED allocation # of stores is zero

        msg_OTS_Forecast_Method_LogInformation = 040220, //TT#753-754 Log informational message added to audit
        msg_Modify_Sales_Method_LogInformation = 040221, //TT#753-754 Log informational message added to audit
        msg_RefreshWarning = 040223,		// TT#4287 - JSmith - Do not allow refresh if windows are open
        // Begin TT#1646-MD - jsmith - Add or Remove Hierarchy Levels
        msg_ProcessCompletedSuccessfully = 040224,
        msg_ProcessCompletedWithErrors = 040225,
        msg_ProcessDidNotStart = 040226,
        msg_TransactionsProcessed = 040227,
        msg_ItemsAdded = 040228,
        msg_ItemsRemoved = 040229,
        msg_ItemsWithErrors = 040230,
        msg_ProcessNotRun = 040231,
        msg_FileSerializationError = 040232,
        msg_FileProcessingError = 040233,
        msg_DelimiterSetting = 040234,
        msg_UnknownAction = 040235,
        msg_ProcessingStep = 040236,
        msg_CompletedStep = 040237,
        msg_ProcessLocation = 040238,
        msg_ProcessArguments = 040239,
        msg_ProcessingErrorReturnCode = 040240,
        msg_FileLocation = 040241,
        msg_FileNotFound = 040242,
        msg_ConfigSettingAndValue = 040243,
        msg_ConfigSettingNotFound = 040244,
        msg_ProcessingFilesForTrigger = 040245,
        msg_NoFilesForTrigger = 040246,
        msg_Transaction = 040247,
        msg_EmptyFile = 040248,
        msg_InputFileNotSpecified = 040249,
        msg_MustBeApplicationServer = 040250,
        msg_MismatchedDelimiter = 040251,
        // End TT#1646-MD - jsmith - Add or Remove Hierarchy Levels
        msg_SaveGroupPendingChanges = 040252,  // TT#4811 - JSmith - Choose to not save changes in style review but changes save anyway
        msg_SaveAssortmentPendingChanges = 040253, // TT#1954-MD - AGallagher - Assortment
        msg_BeginProcessing = 040254,	// TT#1966-MD - JSmith - DC Fulfillment
        msg_EndProcessingSuccessfully = 040255,	// TT#1966-MD - JSmith - DC Fulfillment
        msg_EndProcessingFailed = 040256,	// TT#1966-MD - JSmith - DC Fulfillment
        msg_DC_Fulfillment_Method_LogInformation = 040257, // TT#1966-MD - AGallagher - DC Fulfillment
        msg_ReadOnlyMode = 040258, // RO-3444 - JSmith - RO Web Messaging
        msg_WorkflowCannotBeDeleted = 040259, // RO-3444 - JSmith - RO Web Messaging
        msg_WorkflowCannotBeUpdated = 040260, // RO-3444 - JSmith - RO Web Messaging
        msg_MethodCannotBeDeleted = 040261, // RO-3444 - JSmith - RO Web Messaging
        msg_MethodCannotBeUpdated = 040262, // RO-3444 - JSmith - RO Web Messaging
        msg_WorkflowRequested = 040263, // RO-3444 - JSmith - RO Web Messaging
        msg_MethodRequested = 040264, // RO-3444 - JSmith - RO Web Messaging
        msg_DeleteInUseWarning = 040265, // RO-3444 - JSmith - RO Web Messaging
        msg_CallParmsDoNotMatchInstance = 040266, // RO-3444 - JSmith - RO Web Messaging
        msg_DataNotLocked = 040267, // RO-3444 - JSmith - RO Web Messaging
		msg_ColorNotOnHeader = 040268, // RO-3444 - JSmith - RO Web Messaging

        //Hierarchy Messages 050000-059999
        msg_NotAddLevelOpenHierarchy = 050000,
        msg_HierarchyColorNotFound = 050001,
        msg_LevelColorNotFound = 050002,
        msg_LevelLengthTypeNotFound = 050003,
        msg_LevelNameRequired = 050004,
        msg_LevelRangeFromNotFound = 050005,
        msg_LevelRangeToNotFound = 050006,
        msg_LevelRequiredSizeNotFound = 050007,
        msg_LevelTypeNotFound = 050008,
        msg_HierarchyNotFound = 050009,
        msg_HierarchyRequired = 050010,
        msg_HierarchyTypeNotFound = 050011,
        msg_InvalidHierarchyType = 050012,
        msg_InvalidLevelLengthType = 050013,
        msg_InvalidLevelType = 050014,
        msg_InvalidProductType = 050015,
        msg_InvalidLevelRange = 050016,
        msg_InvalidLevelRequiredSize = 050017,
        msg_ParentRequired = 050018,
        msg_ParentNotFound = 050019,
        msg_ParentRequiredAutoadd = 050020,
        msg_ProductRequired = 050021,
        msg_RequiredSizeGTZero = 050022,
        msg_AddBeyondLevels = 050023,
        msg_FirstLevelNotUp = 050024,
        msg_LastLevelNotDown = 050025,
        msg_CanNotRemoveNestedLevel = 050026,
        msg_CanNotAddNestedLevel = 050027,
        msg_NodeInHierarchyTwice = 050028,
        msg_MustBeSameLevel = 050029,
        msg_CanNotDropInSharedPath = 050030,
        msg_CanNotMoveToPersonalFolder = 050031,
        msg_InvalidOTSPlanLeveltype = 050032,
        msg_MustAddAtHome = 050033,
        msg_DuplicateProductID = 050034,
        msg_MustBeNodeToDrop = 050035,
        msg_CanNotMoveLevel = 050036,
        msg_ColorAfterStyle = 050037,
        msg_SizeMustBeLowestLevel = 050038,
        msg_CanNotMoveLevelBelowStyle = 050039,
        msg_LevelOfTypeAlreadyDefined = 050040,
        msg_CanNotRenameComplexOption = 050041,
        msg_AutoAddColorCode = 050042,
        msg_ProductNotFound = 050043,
        msg_NoLevelBelowStyle = 050044,
        msg_HierarchyAlreadyExists = 050045,
        msg_NoDeleteWithChildren = 050046,
        msg_LevelsWillBeDeleted = 050047,
        msg_NodeLookupFormatInvalid = 050048,
        msg_FirstLevelMustBeStyle = 050049,
        msg_NodeCanNotBeMoved = 050050,
        msg_InvalidMyHierarchyCopy = 050051,
        msg_InvalidAlternateCopy = 050052,
        msg_CanNotRenameFolders = 050053,
        msg_CanNotAddNodesToFolders = 050054,
        msg_CanNotDropOnFolder = 050055,
        msg_CanNotCopyNode = 050056,
        msg_NodeIDAlreadyExists = 050057,
        msg_IDRequired = 050058,
        msg_DescriptionRequired = 050059,
        msg_NameRequired = 050060,
        msg_PropertiesNotAvailable = 050061,
        msg_CopyDescendants = 050062,
        msg_CanNotAddNodesBeyondDef = 050063,
        msg_OnlyOneOrganizational = 050064,
        msg_NodeAlreadyInParent = 050065,
        msg_LevelsDoNotMatch = 050066,
        msg_NextLevelHasNodes = 050067,
        msg_LevelHasNodesCanNotDelete = 050068,
        msg_CanNotDeleteSharedPath = 050069,
        msg_NumberOfDescendants = 050070,
        msg_ConfirmDeleteDescendants = 050071,
        msg_ConfirmDeleteItem = 050072,
        msg_CanNotCopyColorSizeToSelf = 050073,
        msg_NodeIDTooLarge = 050074,
        msg_AppErrorColorNotFound = 050075,
        msg_ColorAlreadyInStyle = 050076,
        msg_ColorCodeNotValid = 050077,
        msg_SizeCodeNotValid = 050078,
        msg_AppErrorColorLevelNotFound = 050079,
        msg_CanNotAddColorToHardlines = 050080,
        msg_DailyPercentDateOverlap = 050081,
        msg_DailyPercentageOverlapCount = 050082,
        msg_VelocityGradesNotUnique = 050083,
        msg_StoreGradesNotUnique = 050084,
        msg_GradeBoundariesNotUnique = 050085,
        msg_LastBoundaryNotZero = 050086,
        msg_MustBe100ToApply = 050087,
        msg_MinStockNotLessThanMax = 050088,
        msg_MinColorNotLessThanMax = 050089,
        msg_ApplyToDescendants = 050090,
        msg_LastSellThruPctNotZero = 050091,
        msg_CanNotRenameReferences = 050092,
        msg_DateRequiredToApply = 050093,
        msg_MinColorNotLessThanMinStock = 050094,
        msg_MaxColorNotLessThanMaxStock = 050095,
        msg_CircularNodeRelationship = 050096,
        msg_ConfirmRemoveItem = 050097,
        msg_CapacitySaveWarning = 050098,
        msg_EnterCapacityValues = 050099,
        msg_DeleteInheritedCapacityUseZero = 050100,
        msg_InvalidParentChild = 050101,
        msg_LevelNotFoundAddQuestion = 050102,
        msg_PlanViewNotAvailable = 050103,
        msg_NodeNotFoundAutoaddDisabled = 050104,
        msg_NotParentsHomeHierarchy = 050105,
        msg_NodeAlreadyInHierarchy = 050106,
        msg_SizeCodeNotAddedNotAddNode = 050107,
        msg_ConfirmInsertLevel = 050108,
        msg_AddColorLevel = 050109,
        msg_AddSizeLevel = 050110,
        msg_CanNotMoveLevelHasNodes = 050111,
        msg_AppErrorSizeLevelNotFound = 050112,
        msg_DeleteLowerLevels = 050113,
        //Begin Track #3948 - JSmith - add OTS Forecast Level interface
        msg_InvalidOTSForecastLevel = 050114,
        //End Track #3948
        msg_CanNotMoveFromSharedPath = 050115,
        msg_ConfirmRemoveItems = 050116,
        msg_ConfirmDeleteItems = 050117,
        msg_ConfirmDeleteRemoveItems = 050118,
        msg_NotAuthorizedToDeleteAllNodes = 050119,
        msg_NotAuthorizedToRemoveAllNodes = 050120,
        msg_NotAuthorizedToDeleteRemoveAllNodes = 050121,
        msg_ToParentRequired = 050122,
        msg_NewProductIDRequired = 050123,
        msg_ReclassPreview = 050124,
        msg_ReclassMove = 050125,
        msg_ReclassMoveConfirmed = 050126,
        msg_ReclassRename = 050127,
        msg_ReclassRenameConfirmed = 050128,
        msg_ReclassDelete = 050129,
        msg_ReclassDeleteConfirmed = 050130,
        msg_ReclassRemoveConfirmed = 050131,
        msg_ReclassDeleteFailed = 050132,
        msg_ReclassRemoveFailed = 050133,
        msg_ReclassRollup = 050134,
        msg_ReclassSecurityGroupDelete = 050135,
        msg_ReclassSecurityUserDelete = 050136,
        msg_ReclassDeleteProductForUser = 050137,
        msg_ReclassReplaceProductForUser = 050138,
        msg_ReclassInUseByHeader = 050139,
        msg_ReclassFailedShowPreview = 050140,
        msg_ReclassMoveFailed = 050141,
        msg_ReclassRenameFailed = 050142,
        msg_TransactionTypeInvalid = 050143,
        msg_ReclassRollupFlagInvalid = 050144,
        msg_ReclassActionInvalid = 050145,
        msg_ReclassPreviewFlagInvalid = 050146,
        msg_ReclassOptionTypeInvalid = 050147,
        msg_ReclassForceDeleteInvalid = 050148,
        msg_ReclassPreviewTurnedOff = 050149,
        msg_NodeLockConflictHeading = 050150,
        msg_NodeLockConflictUser = 050151,
        msg_AllNodesInUse = 050152,
        msg_ContinuePartialDelete = 050153,
        msg_ItemWillBeDeleted = 050154,
        msg_ItemWillNotBeDeleted = 050155,
        msg_ConfirmShortcuts = 050156,
        msg_ConfirmMoveNodes = 050157,
        msg_ConfirmCopyNodes = 050158,
        msg_DeleteFailedFromStyleHeaders = 050159,
        msg_MoveFailedFromStyleHeaders = 050160,
        // Begin TT#634 - JSmith - Color rename
        //msg_ReclassNotPermittedBelowStyle = 050161,
        msg_ReclassNotPermittedAtSize = 050161,
        // End TT#634
        msg_ReclassShortcutConfirmed = 050162,
        msg_ParentNotInHierarchy = 050163,
        msg_ProductCharAlreadyAssigned = 050164,
        msg_ProductCharNotFound = 050165,
        msg_ProductCharRequired = 050166,
        msg_ProductCharAdded = 050167,
        msg_ProductCharValueNotFound = 050168,
        msg_ProductCharValueAdded = 050169,
        msg_InvalidOTSHierarchy = 050170,
        msg_InvalidOTSSelectType = 050171,
        msg_InvalidOTSNodeSearchSelectType = 050172,
        msg_OTSLevelRequired = 050173,
        msg_OTSLevelStartsWithRequired = 050174,
        msg_StockMinMaxWarning = 050175,
        //Begin Track #4178 - JSmith - Only rename in home relationship
        msg_MustRenameInHome = 050176,
        //End Track #4178
        //Begin Track #4833 - JSmith - Adding size code without product category
        ProductCategoryRequired = 050177,
        //End Track #4833
        //Begin Track #4815 - JSmith - #283-User (Security) Maintenance
        msg_DeleteNodeInUse = 050178,
        //End Track #4815
        // Begin Track #5259 - JSmith - Add new reclass roll options
        msg_ReclassRollExtIntFlagInvalid = 050179,
        msg_ReclassRollAltHierFlagInvalid = 050180,
        // End Track #5259 - JSmith
        //Begin TT#155 - JScott - Add Size Curve info to Node Properties
        msg_CannotUncheckInheritedDefault = 050181,
        msg_CurveNameNotUnique = 050182,
        msg_InheritedFromHigherLevel = 050183,   // Begin TT#274 -RMatelic - Unrelated to specific issue
        msg_MustIncludeOneSubreport = 050184,   // End TT#274  
        msg_OnlyOneSimilarStoreAllowed = 050185,
        //End TT#155 - JScott - Add Size Curve info to Node Properties
        //Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
        msg_DetachSizeOutOfStockInheritance = 050186,
        //End TT#483 - JScott - Add Size Lost Sales criteria and processing
        // Begin TT#634 - JSmith - Color rename
        msg_ColorRenamed = 050187,
        msg_ExistingColorUsed = 050188,
        msg_ColorAdded = 050189,
        // End TT#634
        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
        msg_RestartingHierarchyService = 050190,
        msg_HierarchyServiceCouldNotBeStopped = 050191,
        msg_HierarchyServiceCouldNotBeStarted = 050192,
        msg_ServiceNotFound = 050193,
        msg_StartingHierarchyService = 050194,
        msg_HierarchyServiceStarted = 050195,
        msg_StoppingHierarchyService = 050196,
        msg_HierarchyServiceStopped = 050197,
        // End TT#988
        //Begin TT#1323 - JSmith - Rollup never completed and had to be cancelled
        msg_CircularHierarchiesHeader = 050198,
        msg_CircularHierarchiesDetail = 050199,
        //End TT#1323
        /*Begin TT#1159 – improving messaging*/
        msg_StandardInUseMsg = 050200,
        /*End TT#1159 – improving messaging*/
        /*Begin TT#1399 - GTaylor - Alternate Hierarchy Inherit Node Properties */
        msg_lblApplyNodePropsFrom = 050201,
        msg_gbxApplyNodeProperties = 050202,
        msg_MustBeAlternateForApplyNodeProperties = 050203,
        msg_ApplyNodeNotFound = 050204,
        msg_MustBeOrganizationslForApplyNodeProperties = 050205,
        /*End TT#1399 - GTaylor - Alternate Hierarchy Inherit Node Properties */
        // Begin TT#1873 - JSmith - Purge Failed with Severe Error
        msg_AllUserItemsNotDeleted = 050206,
        msg_HierarchyInUseByHeader = 050207,
        // End TT#1873
        //Begin TT#1754 - DOConnell - Transactions were incorrectly attempting to load different hierarchy level definitions to an existing hierarchy
        msg_OneOrganizationalHierarchyAllowed = 050208,
        //End TT#1754 - DOConnell - Transactions were incorrectly attempting to load different hierarchy level definitions to an existing hierarchy
        msg_DateRangesOverlap = 050209,  // TT#2621 - JSmith - Duplicate weeks in daily sales
        msg_CrossUserAlternateError = 050210,  // TT#2737 - JSmith - Brand Hierarchy Reclass
        msg_ToParentNotFound = 050211,  // TT#3366 - JSmith - Parent not found error in Audit is deceiving
        msg_InvalidReclassTriggerFileName = 050212,	// TT#3894-VStuart-Alternate Reclass Failure-Oakley 
        // Begin TT#1646-MD - jsmith - Add or Remove Hierarchy Levels
        msg_LevelNameInvalid = 050213,
        msg_CannotAddLevelAfterStyle = 050214,
        msg_DuplicateLevelName = 050215,
        msg_LevelAddTransactionIncorrect = 050216,
        msg_LevelSuccessfullyAdded = 050217,
        msg_LevelSuccessfullyRemoved = 050218,
        msg_HierarchyBranchSize = 050219,
        // End TT#1646-MD - jsmith - Add or Remove Hierarchy Levels
        msg_HierarchyInUseByUser = 050220,   //TT#5245-Purge error number 2-BonTon
        msg_HierarchyInUseByUserItem = 050221,   //TT#5245-Purge error number 2-BonTon
        msg_HierarchyNotAPIRollup = 050222,   // TT#5479 - JSmith - Rollup should not build requests for hierarchies not set as API
        msg_NodeCannotBeAlternate = 050223, // TT#2108-MD - JSmith - An Alternate Hierachy can be used in the Apply to for Merchandise.  This is not allowed because when charging intransit it would not know where to put it.  

        //Store Messages 060000-069999
        msg_StoreRequired = 060000,
        msg_StoreNotFound = 060001,
        msg_ActiveIndNotFound = 060002,
        msg_StoreGroupNameRequired = 060003,
        msg_AddTextToWhereClause = 060004,
        msg_RemoveTextFromWhereClause = 060005,
        msg_RadioButtonChangeError = 060006,
        msg_AllStoresPopError = 060007,
        msg_AllCharPopError = 060008,
        msg_AllENGSQLPopError = 060009,
        msg_SaveStoreGroupError = 060010,
        msg_AddAllError = 060011,
        msg_RemoveAllError = 060012,
        msg_AddToListError = 060013,
        msg_DetermineCharError = 060014,
        msg_DetermineBoolCharError = 060015,
        msg_DetermineStringCharError = 060016,
        msg_DetermineNumericCharError = 060017,
        msg_DetermineDateCharError = 060018,
        msg_DetermineUDCharError = 060019,
        msg_ClearValuesError = 060020,
        msg_DateFillError = 060021,
        msg_FromDateError = 060022,
        msg_ValidateDateError = 060023,
        msg_SQLOperatorError = 060024,
        msg_StoreGroupBuilderDBNullError = 060025,
        msg_StoreSellingDateError = 060026,
        msg_StoreStockDateError = 060027,
        msg_InvalidStoreCharListValue = 060028,
        msg_InvalidStoreCharateristic = 060029,
        msg_InvalidBetweenValue = 060030,
        msg_StoreCharacteristicInUse = 060031,
        msg_IncompleteCharacteristicGroup = 060032,
        msg_StoreStatusProtected = 060033,
        msg_ValuesExistForNewValueList = 060034,
        msg_ValuesExistForDelete = 060035,
        msg_DuplicateCharGroupName = 060036,
        msg_DuplicateCharValue = 060037,
        msg_CharNameAttrNameConflict = 060038,
        msg_NumericValuesOnly = 060039,
        msg_DateValuesOnly = 060040,
        msg_InvalidDollarValue = 060041,
        msg_InvalidDollarValues = 060042,
        msg_InvalidNumericValues = 060043,
        msg_InvalidNumericValue = 060044,
        msg_InvalidDateValues = 060045,
        msg_InvalidDateValue = 060046,
        msg_InvalidBooleanValue = 060047,
        msg_MissingTextValue = 060048,
        msg_ValuesExisitForCharHeaders = 060049,
        msg_ValuesDeleteForCharHeaders = 060050,
        msg_ValuesDeleteForCharGroups = 060051,
        msg_ErrorTryingToUpdateTable = 060052,
        msg_ErrorGettingMultipleRecords = 060053,
        msg_ErrorCantExitWhileEditing = 060054,
        msg_ValuesExisitForStoreChar = 060055,
        msg_InvalidCharNotPaired = 060056,
        msg_MismatchedParentheses = 060057,
        msg_InvalidWhereSql = 060058,
        msg_ConfirmRemoveWithRefresh = 060059,
        msg_MethodInUse = 060060,
        msg_StoreAttributeInUse = 060061,
        //Begin Track #4010 - JScott - Add all forecast methods and workflows to the scheduler.
        msg_WorkflowInUse = 060062,
        //End Track #4010 - JScott - Add all forecast methods and workflows to the scheduler.
        msg_MatchesReserveWord = 060063,
        msg_StoreNotActive = 060064,  // MID Track 4214 Identify stores in message
        msg_MustBePositive = 060065,
        msg_StoreActiveChanged = 060066,
        msg_InvalidStoreField = 060067, // Issue 4618
        msg_InvalidStoreFieldDetails = 060068,   // Issue 4618
        msg_UnableToDelete = 060069,   // Issue 4585
        msg_StoreMismatchedDelimiter = 060070,   // Issue 4667
        msg_DupHeaderColumnNameDefined = 060071,   // Issue 5743
        msg_StoreGroupInUse = 060072,  // Track #6368 – JSmith - Invalid object name 'dbo.FAVORITES'
        msg_InvalidStoreCharateristicValue = 060077, // Begin TT#237 - JSmith - after transaction gets error, subsequent transactions not processed.
        msg_DuplicateAttributeToCharacteristic = 060078, // Begin TT#502 - APicchetti - handling duplicate attributes/characteristics
        msg_CharacteristicAddedPriorToStore = 060079,
        msg_StoreServiceLooping = 060080,   // TT#190 - MD - stodd -
        msg_StoreNotInactiveForDelete = 060081,
        msg_CannotDeleteReserveStore = 060082,
        msg_CannotMarkReserveStoreInactive = 060083,
        msg_StoreDeleteInProgress = 060084,
        msg_NoStoresMarkedForDeletion = 060085,
        msg_StoreDeleteServicesMustBeDown = 060086,  // TT#1646-MD - jsmith - Add or Remove Hierarchy Levels
        msg_StoreDeleteAnalysisOutdated = 060087,
        msg_StoreDeleteToolTipRowPct = 060088,
        msg_StoreDeleteToolTipRowMax = 060089,
        msg_StoreDeleteToolTipRowMin = 060090,
        msg_StoreDeleteToolTipBatchSize = 060091,
        msg_StoreDeleteToolTipConcurrentDeletes = 060092,
        msg_StoreDeleteToolTipConcurrentCopies = 060093,
        msg_StoreDeleteDropConstraints = 060094,
        msg_InProgressReset = 060095,
        msg_SavingForcesSave = 060096,
        msg_DoNotAllowInactive = 060097, //TT#858-MD - DOConnell – Do not allow a store to be set to Inactive if it has allocation quantities or Intransit 
        msg_DoNotAllowRemoveVSW = 060098, //TT#4685 - JSmith – clear NODE_IMO table when VSW ID is removed from store 
        msg_PendingChanges = 060099, //TT#1517-MD -jsobek -Store Service Optimization
        msg_StoreProfileStoreIdRequired = 060100, //TT#1517-MD -jsobek -Store Service Optimization
        msg_StoreProfileStoreIdForReserve = 060101, //TT#1517-MD -jsobek -Store Service Optimization
        msg_StoreProfileInvalidDateFormat = 060102, //TT#1517-MD -jsobek -Store Service Optimization
        msg_StoreProfileDateNotInMerchandiseCalendar = 060103, //TT#1517-MD -jsobek -Store Service Optimization
        msg_StoreProfileInvalidSellingCloseDate = 060104, //TT#1517-MD -jsobek -Store Service Optimization
        msg_StoreProfileInvalidStockCloseDate = 060105, //TT#1517-MD -jsobek -Store Service Optimization
        msg_StoreProfileIdNotUnique = 060106, //TT#1517-MD -jsobek -Store Service Optimization
        msg_StoreProfileSellingSqFtPositive = 060107, //TT#1517-MD -jsobek -Store Service Optimization
        msg_StoreProfileLeadTimePositive = 060108, //TT#1517-MD -jsobek -Store Service Optimization
        msg_CellValueNotValid = 060109, //TT#1517-MD -jsobek -Store Service Optimization
        msg_StoreProfileStoreIdMaxLengthExceeded = 060110, //TT#1517-MD -jsobek -Store Service Optimization
        msg_AttributeNameExists = 060111,   // TT#1927-MD - JSmith - Able to Save a Dynamic and Manual Store Attribute with the same Name.  Would not think this would be allowed.
        msg_AttributeAlreadyExists = 060112,   // TT#1927-MD - JSmith - Able to Save a Dynamic and Manual Store Attribute with the same Name.  Would not think this would be allowed.
        msg_StoresOrGroupsChanged = 060113,   // TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail

        //Variable Messages 070000-074999
        msg_NoChainDailyForcast = 070000,
        msg_NoStoreDailyForcast = 070001,
        msg_VariableRequired = 070002,
        msg_VariableNotFound = 070003,
        msg_VariableNotSavedOnDatabase = 070004,
        msg_IntransitMustBeByDay = 070005,
        msg_InvalidAttributeForElement = 070006,
        msg_HistoryNotToTotal = 070007,
        msg_IntransitMustFuture = 070008,
        msg_NeedAtLeastOneVariable = 070009,
        msg_VariableCanNotBeRolled = 070010,
        msg_InvalidRollLevel = 070011,
        msg_RollLevelsNotFound = 070012,
        // Begin MID Track #4637 - JSmith - Split variables by type
        msg_InvalidForTypeOfData = 070013,
        // End MID Track #4637

        /* Plan Method Messages 075000-079999 */
        msg_DefaultExists = 075000,
        msg_OTSPlanMethodPopulateError = 075001,
        msg_ForecastMethodFailed = 075002,
        msg_ForecastMethodWeekNotPlanned = 075003,
        msg_MethodInsertMethodExists = 075004,
        msg_MethodInsertError = 075005,
        msg_MethodReserved2 = 075006,
        msg_MethodReserved3 = 075007,
        msg_MethodReserved4 = 075008,
        msg_MethodReserved5 = 075009,
        msg_MethodReserved6 = 075010,
        msg_MethodReserved7 = 075011,
        msg_MethodReserved8 = 075012,
        msg_MethodReserved9 = 075013,
        msg_MethodReserved10 = 075014,
        msg_MethodReserved11 = 075015,
        msg_InventoryCellIsProtected = 075016,
        msg_ChainWeekSpreadFailed = 075017,
        msg_HighLevelWeekSpreadFailed = 075018,
        msg_ChainPeriodSpreadFailed = 075019,
        msg_HighLevelPeriodSpreadFailed = 075020,
        msg_MustBeOTSPlanMethodToDrop = 075021,
        msg_ChainWeekLowLvlSpreadFailed = 075022,
        msg_ZeroChainTotalNoWos = 075023,
        msg_WosExceededBoundary = 075024,
        msg_ZeroChainTotalNoSpread = 075025,
        msg_WosExceededBoundaryQuestion = 075026,
        msg_UserCancelledWosExceeded = 075027,
        msg_VersionProtectedNoInventoryPlanned = 075028,
        msg_GlobalViews = 075029,
        msg_UserViews = 075030,
        msg_InvalidTrendPercentage = 075031,  // Issue 4124 - stodd
                                              //Begin Enhancement - JScott - Export Method - Part 2
        msg_MethodMerchandiseRequired = 075032,
        msg_MethodVersionRequired = 075033,
        msg_TimePeriodRequired = 075034,
        msg_DelimeterRequired = 075035,
        msg_FilePathRequired = 075036,
        msg_NumberOfEntriesRequired = 075037,
        msg_FlagSuffixRequired = 075038,
        msg_EndSuffixRequired = 075039,
        msg_ConcurrentProcessesRequired = 075040,
        msg_SuffixesMustBeUnique = 075041,
        msg_InvalidDefaultFileFormat = 075042,
        msg_InvalidDefaultDelimeter = 075043,
        msg_InvalidDefaultFilePath = 075044,
        msg_InvalidDefaultSplitType = 075045,
        msg_InvalidDefaultSplitNumEntries = 075046,
        msg_InvalidDefaultConcurrentProcesses = 075047,
        msg_InvalidDefaultFlagFileSuffix = 075048,
        msg_InvalidDefaultEndFileSuffix = 075049,
        msg_NoVarCustomTypeOfPresentationMin = 075050,
        //End Enhancement - JScott - Export Method - Part 2
        //Begin Track #4942 - JScott - Correct problems in Export Method
        msg_InvalidDefaultDateType = 075051,
        //End Track #4942 - JScott - Correct problems in Export Method
        msg_NoBalanceNeeded = 075052,	// Issue 5141
        msg_UnableToDeleteStoreLock = 075053,
        msg_UnableToDeleteChainLock = 075054,
        msg_MethodException = 075055,   // 5401 addl exception logic
        msg_MissingBasis = 075056,  // 5401 addl exception logic
        msg_MissingTrendCaps = 075057,	// 5401 addl exception logic
        msg_BasisItemsDeleteWarning = 075058,	// #5744 - JSmith - Rollup Method
        msg_KeepBasisItems = 075059,    // #5744 - JSmith - Rollup Method
        msg_StoreWeekLowLvlSpreadFailed = 075060,   // BEGIN MID Track #5647 - KJohnson - Matrix Forecast
        msg_MethodWarning = 075061, // Track #6187
        msg_LockedStoreTotalGreaterThanChain = 075062, // Track #6187
        msg_NegativeChainTotalNoSpread = 075063, // Track #6418
        msg_ConfirmReplaceMerchandise = 075064, // TT#1332 
        msg_DuplicateDescendantInMethodError = 075065, // TT#2281
                                                       //Begin TT#43 – MD – DOConnell – Projected Sales Enhancement */
                                                       //Begin TT#259 - MD - DOConnell - Cell Lock/Unlock does not work when cell is not selected
                                                       //msg_UnableToSetStoreLock                    = 075053,
                                                       //msg_UnableToSetChainLock                    = 075054,
        msg_UnableToSetStoreLock = 075066,
        msg_UnableToSetChainLock = 075067,
        //END TT#259 - MD - DOConnell - Cell Lock/Unlock does not work when cell is not selected
        //END TT#43 – MD – DOConnell – Projected Sales Enhancement */
        msg_NoWeeksToPlan = 075068,
        msg_updateOverriddenToRead = 075069,  // TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.

        //Allocation Messages 080000-089999
        msg_MaxLessThanRequestedTotal = 080000,
        msg_MinimumsBroken = 080001,
        msg_SpreadTotNotEqualToReqTot = 080002,
        msg_NoEligibleItems = 080003,
        msg_VGDefinitionsIncomplete = 080004,
        msg_NoPerHasPosSalesOrBegStock = 080005,
        msg_AverageBeginningStockIsZero = 080006,
        msg_QtyAllocatedCannotBeNeg = 080007,
        msg_MaxAllocationCannotBeNeg = 080008,
        msg_MinAllocationCannotBeNeg = 080009,
        msg_PMaxAllocationCannotBeNeg = 080010,
        msg_AutoAllocationCannotBeNeg = 080011,
        msg_RuleAllocationCannotBeNeg = 080012,
        msg_QtyShippedCannotBeNeg = 080013,
        msg_ShipYearDayNotDefined = 080014,
        msg_ShipYearWeekNotDefined = 080015,
        msg_NeedYearDayNotDefined = 080016,
        msg_NeedYearWeekNotDefined = 080017,
        msg_AccumGenAllocatedCannotBeNeg = 080018,
        msg_AccumBulkColorAllocatedCannotBeNeg = 080019,
        msg_ShipDtRequiredForNeedDt = 080020,
        msg_InvalidByteFlagGetRequest = 080021,
        msg_InvalidByteFlagSetRequest = 080022,
        msg_InvalidShortFlagGetRequest = 080023,
        msg_InvalidShortFlagSetRequest = 080024,
        msg_InvalidIntegerFlagGetRequest = 080025,
        msg_InvalidIntegerFlagSetRequest = 080026,
        msg_InvalidLongFlagGetRequest = 080027,
        msg_InvalidLongFlagSetRequest = 080028,
        msg_QtyToAllocateCannotBeNeg = 080029,
        msg_AccumBulkSizeAllocatedCannotBeNeg = 080030,
        msg_MultipleCannotBeLessThan1 = 080031,
        msg_UnitsInPackCannotBeNeg = 080032,
        msg_SizeNotDefinedInPackColor = 080033,
        msg_ColorNotDefinedInPack = 080034,
        msg_SizeNotDefinedInBulkColor = 080035,
        msg_ColorNotDefinedInBulk = 080036,
        msg_PackNotDefinedOnHeader = 080037,
        msg_PackSizeKeyOutOfSync = 080038,
        msg_NumberOfStoresCannotBeLessThan1 = 080039,
        msg_AccumColorUnitsInPackCannotBeNeg = 080040,
        msg_AccumSizeUnitsInPackColorCannotBeNeg = 080041,
        msg_PackColorCodeRIDOutOfSync = 080042,
        msg_AccumBulkSizeUnitsToAllocateCannotBeNeg = 080043,
        msg_AccumBulkColorUnitsToAllocateCannotBeNeg = 080044,
        msg_BulkSizeKeyOutOfSync = 080045,
        msg_BulkColorCodeRIDOutOfSync = 080046,
        msg_PackNameOutOfSync = 080047,
        msg_AccumGenToAllocateCannotBeNeg = 080048,
        msg_AccumDtlToAllocateCannotBeNeg = 080049,
        msg_HdrAlreadyPrimary = 080050,
        msg_HdrAlreadySecondary = 080051,
        msg_PlanCannotBeNeg = 080052,
        msg_DimMustBeGrThan0 = 080053,
        msg_IndexOutOfRange = 080054,
        msg_InTransitCannotBeNeg = 080055,
        msg_GradeWeekCountCannotBeLT1 = 080056,
        msg_WrkUpBulkAndPackBuysMutuallyExcl = 080057,
        msg_ReserveQtyCannotBeNeg = 080058,
        msg_CapacityPctCannotBeNeg = 080059,
        msg_GradeBoundaryCannotBeNeg = 080060,
        msg_GradeCalcFail_NoEligibleStores = 080061,
        msg_NoGradeDefinition = 080062,
        msg_ColorNotDefinedInMethod = 080063,
        msg_StrGroupLvlNotDefinedInMethod = 080064,
        msg_DuplicateGradeNameNotAllowed = 080065,
        msg_DuplicateColorNotAllowed = 080066,
        msg_DuplicateStrGroupLvlNotAllowed = 080067,
        msg_DuplicatePackNameNotAllowed = 080068,
        msg_GradeNotDefinedInMethod = 080069,
        msg_TolerancePctCannotBeNeg = 080070,
        msg_UnknownMethodInWorkFlow = 080071,
        msg_ActionCannotHaveUI = 080072,
        msg_PlanFactorCannotBeNeg = 080073,
        msg_MethodIgnored = 080074,   // MID Track 5778 - Scheduler "Run Now" feature gets Error in Audit
        msg_AllocationWorkFlowStart = 080075,
        msg_AllocationWorkFlowEnd = 080076,
        msg_AllocationWorkFlowError = 080077,
        msg_AllocationHeaderStart = 080078,
        msg_AllocationHeaderEnd = 080079,
        msg_CapacityCannotBeNeg = 080080,
        msg_BackoutAllocationInvalidWhenIntransit = 080081,
        msg_BackoutColorAllocationFailed = 080082,
        msg_BackoutColorAllocationSuccess = 080083,
        msg_BackoutPackAllocationFailed = 080084,
        msg_BackoutPackAllocationSuccess = 080085,
        msg_UnknownAllocationComponent = 080086,
        msg_ResetActionInvalidAfterShipping = 080087,
        msg_ReleaseActionSuccessful = 080088,
        msg_UnknownAllocationAction = 080089,
        msg_HeaderCreateSuccess = 080090,
        msg_HeaderDeleteSuccess = 080091,
        msg_HeaderNotFound = 080092,
        msg_HeaderUpdateSuccess = 080093,
        msg_NotColorComponent = 080094,
        msg_NotSpecificColorComponent = 080095,
        msg_NotSpecificSizeComponent = 080096,
        msg_BackoutPackSuccess = 080097,
        msg_ResetActionSuccess = 080098,
        msg_UnknownColorComponent = 080099,
        msg_UnknownSizeComponent = 080100,
        msg_BackoutBulkAllocationSuccess = 080101,
        msg_BackoutColorSizeAllocationSuccess = 080102,
        msg_BackoutDetailAllocationSuccess = 080103,
        msg_BackoutGenericAllocationSuccess = 080104,
        msg_BackoutTotalAllocationSuccess = 080105,
        msg_BackoutColorSizeAllocationFailed = 080106,
        msg_RestartWorkFlow = 080107,
        msg_RestartAtLineExceedsCount = 080108,
        msg_PrimarySecondaryRIDMustBeValid = 080109,
        msg_InvalidPrimaryAllocation = 080110,
        msg_PrimaryAttachSuccess = 080111,
        msg_SecondaryAttachSuccess = 080112,
        msg_CannotReAttachPrimary = 080113,
        msg_DuplicateColorSizeNotAllowed = 080114,
        msg_GeneralHeaderApplied = 080115,
        msg_NotAllocationProfile = 080116,
        msg_WorkflowTypeInvalid = 080117,
        msg_NotAllocationGradeBin = 080118,
        msg_CompareObjectMustBePackHdr = 080119,
        msg_StoreRIDNotFound = 080120,
        msg_SizeGroupNotFound = 080121,
        msg_MustBeStyleOrProductNode = 080122,
        msg_MethodsOutOfSync = 080123,
        msg_NotFillSizeHolesMethod = 080124,
        msg_al_NoReserveStore = 080125,
        msg_al_StoresNotLoaded = 080126,
        msg_al_LockTotalExceedsNew = 080127,
        msg_al_CannotChgBulkIsDetailAftAllocateStart = 080128,
        msg_al_NewPackAlreadyExists = 080129,
        msg_al_CannotChgPackTypeAftAllocateStart = 080130,
        msg_al_CannotChgPackMultAftAllocateStart = 080131,
        msg_al_CannotChgPackToAllocateAftAllocateStart = 080132,
        msg_al_QtyAloctdBulkColorTotalSummaryReadOnly = 080133,
        msg_al_QtyAloctdDetailSubTypeSummaryReadOnly = 080134,
        msg_al_DetailTypeOutOfSync = 080135,
        msg_al_QtyAloctdGenericTypeSummaryReadOnly = 080136,
        msg_al_QtyAloctdTypeSummaryReadOnly = 080137,
        msg_al_UnknownAllocationSummaryNode = 080138,
        msg_al_QtyShippedTypeSummaryReadOnly = 080139,
        msg_al_QtyShippedGenericTypeSummaryReadOnly = 080140,
        msg_al_QtyShippedDetailTypeSummaryReadOnly = 080141,
        msg_al_QtyShippedDetailSubTypeSummaryReadOnly = 080142,
        msg_al_QtyShippedBulkSummaryReadOnly = 080143,
        msg_al_QtyShippedBulkColorTotalSummaryReadOnly = 080144,
        msg_al_ShipStatusIsOnHoldCannotShip = 080145,
        msg_al_ShipFlagTypeSummaryReadOnly = 080146,
        msg_al_ShipFlagGenericTypeSummaryReadOnly = 080147,
        msg_al_ShipFlagDetailTypeSummaryReadOnly = 080148,
        msg_al_ShipFlagDetailSubTypeSummaryReadOnly = 080149,
        msg_al_ShipFlagBulkSummaryReadOnly = 080150,
        msg_al_ShipFlagBulkColorTotalSummaryReadOnly = 080151,
        msg_al_FilledSizeHoleGenericTypeInvalid = 080152,
        msg_al_FilledSizeHoleTypeInvalid = 080153,
        msg_al_FilledSizeHoleDetailSubTypeInvalid = 080154,
        msg_al_FilledSizeHoleBulkColorTotalInvalid = 080155,
        msg_DuplicateMethod = 080156,
        msg_GenericMethodInsertError = 080157,
        msg_al_NewBulkColorAlreadyExists = 080158,
        msg_al_DuplicateBulkColorNotAllowed = 080159,
        msg_al_DuplicateSizeInPackColorNotAllowed = 080160,
        msg_al_DuplicateColorInPackNotAllowed = 080161,
        msg_al_CannotRemovePackColorWhenPacksAloctd = 080162,
        msg_al_CompareObjectMustBeHdrSizeBin = 080163,
        msg_al_NewBulkColorSizeAlreadyExists = 080164,
        msg_al_CompareObjectMustBeHdrColorBin = 080165,
        msg_al_CompareObjectsMustBeAllocationGradeBin = 080166,
        msg_al_CompareObjMustBeAllocationProfile = 080167,
        msg_al_DuplicateSubtotalsNotAllowed = 080168,
        msg_al_ColorHierarchyNodeNotDefined = 080169,
        msg_al_SizeHierarchyNodeNotDefined = 080170,
        msg_al_ReleaseActionSuccess = 080171,
        msg_al_SizeIntransitBackoutSuccess = 080172,
        msg_al_SizeIntransitUpdateSuccess = 080173,
        // BEGIN MID Track # 2369  Wrong Message for Style Intransit Update/backout
        //		msg_al_StyleIntransitUpdateSuccess				= 080174,
        //		msg_al_StyleIntransitBackoutSuccess				= 080175,
        msg_al_StyleIntransitBackoutSuccess = 080174,
        msg_al_StyleIntransitUpdateSuccess = 080175,
        // END MID Track # 2369
        msg_al_ChargeIntransitFailed = 080176,
        msg_al_StyleIntransitBackoutFailed = 080177,
        msg_al_SizeIntransitBackoutFailed = 080178,
        msg_al_ShipFlagRequestInvalid = 080179,
        msg_NotRuleMethod = 080180,
        msg_al_CannotChgBegDtAftAloctnStarted = 080181,
        msg_al_CannotChgGradeBoundaryAftAloctnStarted = 080182,
        msg_al_CannotChgHeaderStyleAftAloctnStarted = 080183,
        msg_al_CannotChgInUseAftAloctnStarted = 080184,
        msg_al_CannotChgOnHandSrcAftAloctnStarted = 080185,
        msg_al_CannotChgPlanFactorAftAloctnStarted = 080186,
        msg_al_CannotChgPlanLvlAftAloctnStarted = 080187,
        msg_al_CannotChgShipDtAftAloctnStarted = 080188,
        msg_al_CannotChgWorkUpBuyAftAloctnStarted = 080189,
        msg_al_CannotDeleteHeaderWhenAllocationStarted = 080190,
        msg_al_UnitCostCannotBeNeg = 080191,
        msg_al_UnitRetailCannotBeNeg = 080192,
        msg_al_AllColorAllocatedReadOnly = 080193,
        msg_al_AllColorRuleReadOnly = 080194,
        msg_al_AllGenericPacksAllocatedReadOnly = 080195,
        msg_al_AllGenericPacksRuleReadOnly = 080196,
        msg_al_AllNonGenericPacksAllocatedReadOnly = 080197,
        msg_al_AllNonGenericPacksRuleReadOnly = 080198,
        msg_al_AllPacksAllocatedReadOnly = 080199,
        msg_al_AllPacksRuleReadOnly = 080200,
        msg_al_AllSizesAllocatedReadOnly = 080201,
        msg_al_LastNeedDayNotValidBySize = 080202,
        msg_al_MinimumNotTrackedForAllSizes = 080203,
        msg_al_MinimumNotTrackedForSizeAcrossColor = 080204,
        msg_al_RuleNotTrackedBySize = 080205,
        msg_al_SizeAllocatedAcrossColorReadOnly = 080206,
        msg_al_UnknownComponentType = 080207,
        msg_OTSPlanWorkFlowStart = 080208,
        msg_OTSPlanWorkFlowEnd = 080209,
        msg_OTSPlanWorkFlowError = 080210,
        msg_OTSPlanHeaderStart = 080211,
        msg_OTSPlanHeaderEnd = 080212,
        msg_al_GeneralComponentNodeNotImplemented = 080213,
        msg_al_PercentNeedBeforeNotValidBySize = 080214,
        msg_al_UnitNeedBeforeNotValidBySize = 080215,
        msg_al_FilledSizeHolePackInvalid = 080216,
        msg_al_AllNonGenericStyleByNeedNotImplemented = 080217,
        msg_al_AllPacksStyleByNeedNotImplemented = 080218,
        msg_al_AloctnByNeedNotValidForSize = 080219,
        msg_al_AllColorsTtlReadOnly = 080220,
        msg_al_DetailSubTypeTtlReadOnly = 080221,
        msg_al_TypeTtlReadOnly = 080222,
        msg_al_BreakoutSizeComponentInvalid = 080223,
        msg_al_StoresRequireApplicationSessionTransaction = 080224,
        msg_al_CannotChgTransactionAftProfileCreated = 080225,
        msg_al_DuplicateHeaderIgnored = 080226,
        msg_al_MustCreateSizeBeforeResetting = 080227,
        msg_al_MustCreateColorBeforeResetting = 080228,
        msg_al_ReserveNotImplemented = 080229,
        msg_al_SizeRequestRequiresColorAndSize = 080230,
        msg_al_SelectedHeadersMustTreatBulkSame = 080231,
        msg_al_SubtotalQtyAloctdGeneralComponent = 080232,
        msg_al_PackNotDefinedOnSubtotal = 080233,
        msg_MustBeMethodToDrop = 080234,
        msg_MethodRequiredForAction = 080235,
        msg_WrongTypeOfMethodForAction = 080236,
        msg_WorkflowHasNoSteps = 080237,
        msg_TolerancePctInvalid = 080238,
        msg_ComponentIsRequired = 080239,
        msg_ReviewFlagInvalid = 080240,
        msg_ValidColorIsRequired = 080241,
        msg_ValidSizeIsRequired = 080242,
        msg_ValidPackIsRequired = 080243,
        msg_ErrorProcessingStoreFilter = 080244,
        msg_al_UpdateIntransitFailed = 080245,
        msg_al_HeaderUpdateIntransitFailed = 080246,
        msg_al_PacksDoNotUpdateBulkIntransit = 080247,
        msg_ActionIsRequired = 080248,
        msg_MethodNameNotFound = 080249,
        msg_ActionNotAllowMethod = 080250,
        msg_al_ParentUpdateFailed = 080251,
        msg_al_SubtotalSpreadFailed = 080252,
        msg_al_SubtotalUpdateFailed = 080253,
        msg_al_AllocationWaferTrappedException = 080254,
        msg_al_ComponentMustBeSpecific = 080255,
        msg_al_MethodRIDCannotBeLessThan1 = 080256,
        msg_al_RuleAllocationAlreadyExists = 080257,
        msg_al_RuleAllocationDoesNotExist = 080258,
        msg_al_RuleLayerIDCannotBeLessThan1 = 080259,
        msg_al_MinimumValueExceeded = 080260,
        msg_al_MaximumValueExceeded = 080261,
        msg_al_MultipleValueIncorrect = 080262,
        msg_al_CapacityUsedCannotBeNeg = 080263,
        msg_al_RelieveComponentInvalid = 080264,
        msg_al_RelieveIntransitSuccess = 080265,
        msg_al_RelieveIntransitFailed = 080266,
        msg_DuplicateWorkflow = 080267,
        msg_DuplicateHeaderIdNotAllowed = 080268,
        msg_NoHeaderSelectedOnWorkspace = 080269,
        msg_MultHeadersSelectedOnWorkspace = 080270,
        msg_InclExclInstructionNotAllowed = 080271,
        msg_al_MultipleCannotExceedToAllocate = 080272,
        msg_al_ReleaseApprovedRequiresAllInBalanceStatus = 080273,
        msg_al_ReleaseRequiresReleaseApprovedStatus = 080274,
        msg_HeaderStatusDisallowsAction = 080275,
        msg_MustBeCorrectLevel = 080276,
        msg_Data1NotValidData2 = 080277,
        msg_al_ReleaseApprovedSuccess = 080278,
        msg_al_ReleaseActionFailed = 080279,
        msg_ActionWarning = 080280,
        msg_ActionNotAllowed = 080281,
        msg_al_StoreDateListsOutOfSync = 080282,
        msg_InvalidRule = 080283,
        msg_VelocityMatrixDeleteWarning = 080284,
        msg_al_ApplyChosenRuleFailed = 080285,
        msg_al_PacksNotCompatibleForExactRule = 080286,
        msg_MustBeAllocationMethodToDrop = 080287,
        msg_DateLessThanSalesWeek = 080288,
        msg_DatesMustBeSameType = 080289,
        msg_DateCannotBeGreater = 080290,
        msg_StoreGradePeriodExceeded = 080291,
        msg_SizeMustDivideColorEvenly = 080292,
        msg_al_BulkSizeAloctnsMustBalance = 080293,
        msg_al_ResetActionInvalidAfterShipping = 080294,
        msg_al_StyleAloctnMustBalance = 080295,
        msg_al_SubtotalHasNoMembers = 080296,
        msg_al_SzMultRequestRequiresColorSize = 080297,
        msg_al_ActionNotImplemented = 080298,
        msg_ComponentNotInHeader = 080299,
        msg_al_CannotApplyRulesAfterNeedOrIT = 080300,
        msg_InvalidTreeNode = 080301,
        msg_al_UnitsToAllocateCannotBeSetForWorkUpTotal = 080302,
        msg_al_OTS_PlanLvlNotDefined = 080303,
        msg_ComponentInvalidForInstruction = 080304,
        msg_ColorInstructionComponentMismatch = 080305,
        msg_TotalInstructionComponentMismatch = 080306,
        msg_UnableToDeleteHeaderNotCompletelyShipped = 080307,
        msg_al_NoStoresToProcess = 080308,
        msg_DeleteStoreDetailWarning = 080309,
        msg_TimePeriodCannotBeGreater = 080310,
        msg_VelocityMatrixReprocessWarning = 080311,
        msg_al_BalanceNotPerformedDueToTolerance = 080312,
        msg_al_NoUnitsAllocatedByNeedAction = 080313,
        msg_al_NoUnitsToAllocateByNeed = 080314,
        msg_al_NoWorkUpBuyNeed = 080315,
        msg_al_ThereWasNoPlan = 080316,
        msg_al_NeedHorizonUndefined = 080317,
        msg_al_HeaderReleaseFilePathNotFound = 080318,
        msg_al_ResetActionFailed = 080319,
        msg_al_CannotTriggerUndefinedWorkflow = 080320,
        msg_al_WorkflowTriggerFailed = 080321,
        msg_al_WorkflowTriggerSuccess = 080322,
        msg_al_NoHeadersForWorkflowProcess = 080323,
        msg_al_TriggerTransactionConflict = 080324,
        msg_al_BackoutSizeIntransit = 080325,
        msg_al_BackoutStyleIntransit = 080326,
        msg_al_CannotDeleteReleasedHeader = 080327,
        msg_al_StoreAllocationDeleted = 080328,
        msg_al_CannotReleaseDummy = 080329,
        msg_al_SizeNeedAlgorithmFailed = 080330,
        msg_NotBasisSizeMethod = 080331,
        msg_NotSizeNeedMethod = 080332,
        msg_al_TotalUnitsToAllocateChangedAftAllocateStart = 080333,
        msg_al_ColorUnitsToAllocateChangedAftAllocateStart = 080334,
        msg_al_PacksToAllocateChangedAftAllocateStart = 080335,
        msg_al_SizeUnitsToAllocateChangedAftAllocateStart = 080336,
        msg_al_CannotChgUnitsToAllocateWhenShippingStarted = 080337,
        msg_al_ResetDueToReceiptChg = 080338,
        msg_al_BackoutSizeIntransitDueToReceiptChg = 080339,
        msg_al_BackoutStyleIntransitDueToReceiptChg = 080340,
        msg_al_CannotChgPacksToAllocateWhenShippingStarted = 080341,
        msg_al_CannotChgUnitsToAllocateWhen = 080342,    // MID BL HotFix J.Ellis Do not allow changes after release on header load modify
        msg_al_CannotChgPacksToAllocateWhen = 080343,    // MID BL HotFix J.Ellis Do not allow changes after release on header load modify
        msg_al_ScheduleEnqueueFailure = 080344,
        msg_al_ScheduleHasNoHeadersToProcess = 080345,
        msg_al_SizeGroupRequiredForWorkUpSizeBuy = 080346,
        msg_NotSizeOverrideMethod = 080347,
        msg_al_StoreIndexRIDOutOfSync = 080348,
        msg_NotSpecificPrimarySizeComponent = 080349,
        msg_NotSpecificSecondarySizeComponent = 080350,
        msg_NotSizeComponent = 080351,
        msg_CurveSizeCodeNotOnSizeGroup = 080352,
        msg_RemovingInvalidSizeGroupFromCurve = 080353,
        msg_InvalidSizeCodeEncountered = 080354,
        msg_SizeReviewInvalid = 080355,
        msg_al_UnitsToAllocateCannotBeSetForWorkUpBulk = 080356,
        msg_al_UnitsToAllocateCannotBeSetForWorkUpBulkColor = 080357,
        msg_al_UnitsToAllocateCannotBeSetForWorkUpBulkSize = 080358,
        msg_al_StoreSizePctToColorTotalCannotBeNeg = 080359,
        msg_CannotReleasePurchaseOrder = 080360,
        msg_al_SizeComponentSelectionInvalid = 080361,
        msg_al_WokflowStoppedForReview = 080362,
        msg_al_NoMatchingPacks = 080363,
        msg_al_NoMatchingColors = 080364,
        msg_al_WorkflowTrigger = 080365,
        msg_al_ResetActionBegin = 080366,
        msg_al_BalanceSizeWithSubsBegin = 080367,
        msg_al_ReleaseActionBegin = 080368,
        msg_al_StyleNeedActionBegin = 080369,
        msg_al_BeginHeaderAction = 080370,
        msg_al_BalanceToDCBegin = 080371,
        msg_al_BalanceStyleProportionalBegin = 080372,
        msg_al_BalanceSizeNoSubsBegin = 080373,
        msg_al_DetermineChosenRule = 080374,
        msg_al_BasisHeaderNotDeclared = 080375,
        msg_al_UnsupportedTargetComponent = 080376,
        msg_al_RuleAllocationComponentsMismatched = 080377,
        msg_al_InvalidBasisComponent = 080378,
        msg_al_PacksNotCompatibleForFillRule = 080379,
        msg_al_NoPositivePercentNeed = 080380,
        msg_al_NoSizesFromCurveMatchSizesInColor = 080381,
        msg_al_RuleRequiredForBasisSize = 080382,
        msg_UngroupBeforeAdd = 080383,
        msg_al_SizeIntransitAlreadyUpdated = 080384,
        msg_al_SizeAllocationNotBalanced_SzITnotUpdated = 080385,
        msg_al_StyleAllocationNotBalanced_SzITnotUpdated = 080386,
        msg_al_NoColors_SzITnotUpdated = 080387,
        msg_SizeReviewInvalidForAnalysis = 080388,
        msg_HeaderMaximumWarning = 080389,
        msg_MaximumHeadersExceeded = 080390,
        msg_al_HoldColorComponentMustBeSpecific = 080391,
        msg_al_HoldSizeComponentMustBeSpecific = 080392,
        msg_al_DetermineSoftRule = 080393,
        msg_al_CaptureAllocationStateBeforeSoftRule = 080394,
        msg_al_RestoreAllocationStateBeforeSoftRule = 080395,
        msg_al_RemoveSoftRule = 080396,
        msg_al_SaveSoftRule = 080397,
        msg_al_SaveSoftRuleBindingSuccessful = 080398,
        msg_al_NoSoftRuleToSave = 080399,
        msg_al_SaveSoftRuleBindingFailed = 080400,
        msg_al_BeginSoftRule = 080401,
        msg_al_EndSoftRule = 080402,
        msg_al_BeginHardRule = 080403,
        msg_al_EndHardRule = 080404,
        msg_al_BeginProcessRule = 080405,
        msg_al_EndProcessRule = 080406,
        msg_al_BeginInteractiveRule = 080407,
        msg_al_EndInteractiveRule = 080408,
        msg_al_SizeNotOnBasis = 080409,
        msg_al_SizeNeedAlgorithmInvalidStoreAttr = 080410,
        msg_SelectSizeGroupBeforeDefiningCurve = 080411,
        msg_SizeCurveRequiredForAnalysisOnly = 080412,
        msg_InvalidPercent = 080413,
        msg_InvalidUnits = 080414,
        msg_al_MethodCompletedSuccessfully = 080415,
        msg_al_MethodFailed = 080416,
        msg_al_BasisSizeAlgorithmInvalidStoreAttr = 080417,
        msg_al_HeaderTypeCanNotBeReleased = 080418,
        msg_al_BasisAndTargetHeaderEqual = 080419,
        msg_al_BackoutDetailPackAllocationFailed = 080420,
        msg_al_DetailPackAllocationComponentInvalid = 080421,
        msg_CannotDeleteComponent = 080422,
        msg_al_CannotChangeColorAftAllocationStarted = 080423,
        msg_al_HeaderSizesDoNotMatchSizeCurve = 080424,
        msg_al_ITUpdateFailedDueToSizeAllocationOBAL = 080425,
        msg_al_ITUpdateFailedDueToStyleAllocationOBAL = 080426,
        msg_ChangeSelectionQuestion = 080427,

        msg_al_MustSelectAllDetailPacks = 080428,
        // (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
        msg_al_AloctdQtyCannotBeNeg = 080429,
        msg_al_OrigAloctdQtyCannotBeNeg = 080430,
        msg_al_MasterAlreadyAssigned = 080431,
        msg_al_MasterAlocNotStarted = 080432,
        msg_al_MasterStillAssigned = 080433,
        msg_al_PackMultipleGreaterThanSubordinate = 080434,
        msg_al_PackMultipleMustDivideSubordinateEvenly = 080435,
        msg_al_CannotProcessMasterTotalType = 080436,
        msg_al_NoPacksOnHeader = 080437,
        msg_al_NumberOfPacksExceeded = 080438,
        msg_al_NotAllPacksMatch = 080439,
        msg_al_RsvAloctdQtyCannotBeNeg = 080440,
        // (CSMITH) - END MID Track #3219
        msg_al_StyleItChargedActionNotValid = 080441, // MID Track 3958 bad message on balance to dc // MID Track 5778 (Part 2) - Schedule 'Run Now' feature gets error in audit
        msg_al_StoreSizeCurveDoesNotMatchHeader = 080442,
        msg_al_OTS_NeedFailed_ArraysNotInSync = 080443,
        msg_al_OTS_NeedFailed_StoreNeedNotCalc = 080444,
        msg_BasisDeleteWarning = 080445,
        msg_al_HeaderInUse = 080446,
        msg_al_CannotReleaseHeaderInUse = 080447,
        msg_al_HeaderInUseCannotProcess = 080448,
        msg_CantSelectExistingMulti = 080449,
        msg_InvalidStatusForMultiSelection = 080450,
        msg_OnlyOneMultiAllowed = 080451,
        msg_NoMultiHeaderSelected = 080452,
        msg_NoHeaderSelectedForMulti = 080453,
        msg_RemoveColumnGrouping = 080454,
        msg_MultiHeaderInstransitSplitComplete = 080455,
        msg_MultiHeaderNotMachingPack = 080456,
        msg_al_PackAddedAftAllocateStart = 080457,
        msg_al_CannotAddPacksWhenInUse = 080458,
        msg_al_CannotAddPacksWhenShippingStarted = 080459,
        msg_al_PackRemovedAftAllocateStart = 080460,
        msg_al_CannotRemovePacksWhenInUse = 080461,
        msg_al_CannotRemovePacksWhenShippingStarted = 080462,
        msg_al_PackColorAddedAftAllocateStart = 080463,
        msg_al_CannotAddPackColorWhenInUse = 080464,
        msg_al_CannotAddPackColorWhenShippingStarted = 080465,
        msg_al_PackColorRemovedAftAllocateStart = 080466,
        msg_al_CannotRemovePackColorWhenInUse = 080467,
        msg_al_CannotRemovePackColorWhenShippingStarted = 080468,
        msg_al_PackColorCodeRIDChangedAftAllocateStart = 080469,
        msg_al_CannotChangePackColorCodeRIDWhenInUse = 080470,
        msg_al_CannotChangePackColorCodeRIDWhenShippingStarted = 080471,
        msg_al_PackColorUnitsChangedAftAllocateStart = 080472,
        msg_al_CannotChangePackColorUnitsWhenInUse = 080473,
        msg_al_CannotChangePackColorUnitsWhenShippingStarted = 080474,
        msg_al_PackColorSizeAddedAftAllocateStart = 080475,
        msg_al_CannotAddPackColorSizeWhenInUse = 080476,
        msg_al_CannotAddPackColorSizeWhenShippingStarted = 080477,
        msg_al_PackColorSizeRemovedAftAllocateStart = 080478,
        msg_al_CannotRemovePackColorSizeWhenInUse = 080479,
        msg_al_CannotRemovePackColorSizeWhenShippingStarted = 080480,
        msg_al_PackColorSizeChangedAftAllocateStart = 080481,
        msg_al_CannotChangePackColorSizeWhenInUse = 080482,
        msg_al_CannotChangePackColorSizeWhenShippingStarted = 080483,
        msg_al_PackColorSizeUnitsChangedAftAllocateStart = 080484,
        msg_al_CannotChangePackColorSizeUnitsWhenInUse = 080485,
        msg_al_CannotChangePackColorSizeUnitsWhenShippingStarted = 080486,
        msg_al_BulkColorAddedAftAllocateStart = 080487,
        msg_al_CannotAddBulkColorWhenInUse = 080488,
        msg_al_CannotAddBulkColorWhenShippingStarted = 080489,
        msg_al_BulkColorRemovedAftAllocateStart = 080490,
        msg_al_CannotRemoveBulkColorWhenInUse = 080491,
        msg_al_CannotRemoveBulkColorWhenShippingStarted = 080492,
        msg_al_BulkSizeAddedAftAllocateStart = 080493,
        msg_al_CannotAddBulkSizeWhenInUse = 080494,
        msg_al_CannotAddBulkSizeWhenShippingStarted = 080495,
        msg_al_BulkSizeRemovedAftAllocateStart = 080496,
        msg_al_CannotRemoveBulkSizeWhenInUse = 080497,
        msg_al_CannotRemoveBulkSizeWhenShippingStarted = 080498,
        msg_al_BulkSizeChangedAftAllocateStart = 080499,
        msg_al_CannotChangeBulkSizeWhenInUse = 080500,
        msg_al_CannotChangeBulkSizeWhenShippingStarted = 080501,
        msg_al_MultiHeaderDupColorSizeMismatch = 080502,
        msg_al_MultiHeaderComponentMismatch = 080503,
        msg_al_CannotDetermineColorMinMaxUsingSpecifiedComponent = 080504,
        msg_InvalidMultiHeaderStatus = 080505,
        msg_InvalidTypeForHeadersWithPacks = 080506,
        msg_al_ConstraintModelRequiredForMinMaxRules = 080507, // MID Track 3813 Size Min and Max rules require Constraint model
        msg_al_ThereAreNoSizesInSelectedColor = 080508, // MID Tracks 3738, 3811, 3827 Status Issues
        msg_al_PlanLevelUndetermined = 080509,
        msg_al_SizeItChargedBalanceNotValid = 080510, // MID Track 3958 bad message on balance to dc
        msg_al_HeaderNameInvalid = 080511, // MID Track 4024 Header ID contains invalid characters for file name
        msg_al_Made_OBSOLETE_BY_TT1607 = 080512, // MID Track 4094 'color' not recognized in velocity  // TT#1607 - Inventory Basis wrong when header has mult color
        msg_al_PriorAndTargetHeaderEqual = 080513, // MID Track 3975 - stodd
        msg_al_MemberRemovedFromMultiOnReset = 080514, // MID Track 4033 allow updates to member header of multi-header
        msg_al_HeaderInConflict = 080515, // MID Track 4033 allow updates to member header of multi-header
        msg_HeaderDeleteFailedByStatus = 080516,
        msg_HeaderDeleteForced = 080517,
        msg_HeaderDelete = 080518,
        msg_al_MultiHeaderCannotBeWorkUpBuy = 080519, // MID Track 4033 allow updates to member header of multi-header
        msg_HeaderDeleteFailed = 080520,
        msg_al_AddToMultiHeaderRequiresMultiHeader = 080521, // MID Track 4033 allow updates to member header of multi-header
        msg_al_CannotAddHeaderToMultiWhen = 080522, // MID Track 4033 allow updates to member header of multi-header
        msg_al_CannotAddToMultiWhenMultiStatus = 080523, // MID Track 4033 allow updates to member header of multi-header
        msg_al_CreateMultiHeaderRequiresMultiHeader = 080524, // MID Track 4033 allow updates to member header of multi-header
        msg_al_HeaderAlreadyBelongsToMulti = 080525, // MID Track 4033 allow updates to member header of multi-header
        msg_al_HeaderMustBeSavedBeforeAddingToMulti = 080526, // MID Track 4033 allow updates to member header of multi-header
        msg_HeaderDeleteNotAllowedOnMultiOrMaster = 080527,
        msg_al_MultiHdrSizeGroupMustContainsAllSizesOfNewMember = 080528, // MID Track 4033 allow updates to member header of multi-header
        msg_al_MultiHeaderCreateFailed = 080529, // MID Track 4033 allow updates to member header of multi header
        msg_al_MultiHeaderAddToFailed = 080530, // MID Track 4033 allow updates to member header of multi header
        msg_al_MultiHeaderCreateSuccessful = 080531, // MID Track 4033 allow updates to member header of multi header
        msg_al_MultiHeaderAddToSuccessful = 080532, // MID Track 4033 allow updates to member header of multi header
        msg_al_DeleteExistingSizes = 080533,
        msg_al_AllSizeRowsToBeDeleted = 080534,
        msg_al_HeadersInUse = 080535,
        msg_al_SelectSingleComponent = 080536,
        msg_al_RemoveHeaderFromMultiBegin = 080537, // MID Track 4132 Workspace Blank after Remove
        msg_al_RemoveHeaderFromMultiEnd = 080538, // MID Track 4132 Workspace Blank after Remove
        msg_al_CannotForceHeaderRemoveWhenNoAppTran = 080539, // MID Track 4132 Workspace Blank after Remove
        msg_al_CannotRemoveHeaderFromMultiWhenShipStarted = 080540, // MID Track 4132 Workspace Blank after Remove
        msg_al_CannotRemoveHeaderFromMultiWhen = 080541, // MID Track 4132 Workspace Blank after Remove
        msg_al_HeaderIsNotMemberOfThisMulti = 080542, // MID Track 4132 Workspace Blank after Remove
        msg_al_HeaderIsNotMultiHeader = 080543, // MID Track 4132 Workspace Blank after Remove
        msg_al_ReleasedHeaderCannotBeReleased = 080544,
        msg_al_RemoveHeadersFromMultiHeaderSuccessful = 080545, // MID Track 4132 Workspace Blank after Remove
        msg_al_RemoveHeadersFromMultiHeaderFailed = 080546, // MID Track 4132 Workspace Blank after Remove
        msg_al_HeaderRemovedFromMultiHeader = 080547, // MID Track 4132 Workspace Blank after Remove
        msg_DeleteRows = 080548,
        msg_HeaderTypeNotAllowed = 080549,
        msg_HeadersChanged = 080550,
        msg_SizeTableExists = 080551,
        msg_SizeCodeRetrieveError = 080552,
        msg_SizeCodeNotInGroup = 080553,
        msg_al_LinkedHeadersNotApproved = 080554,
        msg_al_LinkedHeaderCharMultiMismatch = 080555,
        msg_al_LinkedHeadersNotChangeStatus = 080556,
        msg_al_NoSizeCurveForHeader = 080557,
        msg_al_NoGenericSizeCharacteristicForHeader = 080558,  // MID Track 4372 Generic Size Constraints
        msg_al_NoGenericSizeColorForHeader = 080559,  // MID Track 4372 Generic Size Constraints
        msg_al_MoreThanOneColorForHeader = 080560,
        msg_al_SizeGroupMissingSizesOnHeader = 080561, // MID Track 4242 Allow Size Group to change
        msg_al_CannotDetermineOTSColorWhenMultipleColors = 080562, // MID Track 4020 Plan level not reset on cancel
        msg_al_CannotDetermineOnHandColorWhenMultipleColors = 080563, // MID Track 4020 Plan level not reset on cancel
        msg_NotSizeCurveMethod = 080641, // TT#155 - JSmith - Size Curve Method
        msg_IT_BeginRebuildIntransit = 080564, // TT#1137 (MID Track 4351) Rebuild Intransit Utility
        msg_IT_EndRebuildIntransit = 080565, // TT#1137 (MID Track 4351) Rebuild Intransit Utility
        msg_IT_HierarchyNodeNotFound = 080566, // TT#1137 (MID Track 4351) Rebuild Intransit Utility
        msg_IT_NoParentOfStyleAssociatedWithHierarchyNode = 080567, // TT#1137 (MID Track 4351) Rebuild Intransit Utility
        msg_IT_RebuildIntransitParentOfStyleHeadersInUse = 080568, // TT#1137 (MID Track 4351) Rebuild Intransit Utility
        msg_IT_RebuildIntransitFailedForHierarchyNode = 080569, // TT#1137 (MID Track 4351) Rebuild Intransit Utility
        msg_IT_RebuildIntransitFailedForParentOfStyle = 080570, // TT#1137 (MID Track 4351) Rebuild Intransit Utility
        msg_IT_RebuildIntransitSuccessfulForHierarchyNode = 080571, // TT#1137 (MID Track 4351) Rebuild Intransit Utility
        msg_IT_RebuildIntransitSuccessfulForParentOfStyle = 080572, // TT#1137 (MID Track 4351) Rebuild Intransit Utility
        msg_IT_BeginParentOfStyleRebuildIntransit = 080573, // TT#1137 (MID Track 4351) Rebuild Intransit Utility
        msg_IT_EndParentOfStyleRebuildIntransit = 080574, // TT#1137 (MID Track 4351) Rebuild Intransit Utility
                                                          //Size Constraints Batch Load Messages 080575 - 080588
        msg_scl_NoStoreGroupsAreCurrentlyDefined = 080575,
        msg_scl_ModelNameNotDefined = 080576,
        msg_scl_SizeGroupAndSizeCurveNotDefined = 080577,
        msg_scl_ModelSetNotDefined = 080578,
        msg_scl_SizeConstraintsNotDefined = 080579,
        msg_scl_InvalidSizeMinAndMax = 080580,
        msg_scl_InvalidSizeGroupLevel = 080581,
        msg_scl_InvalidColorCode = 080582,
        msg_scl_InvalidDimensionCode = 080583,
        msg_scl_InvalidSizeCode = 080584,
        msg_scl_AllSetsNotDefined = 080585,
        msg_scl_InvalidStoreGroup = 080586,
        msg_scl_InvalidSizeGroup = 080587,
        msg_scl_InvalidSizeCurveGroup = 080588,
        msg_al_NoSizeConstraintForHeader = 080589, // MID Track 4372 Generic Size Constraints
        msg_al_FilterGlobalOptionMismatch = 080590,
        msg_al_RuleNotValidWhenComponentNotTotal = 080591,
        msg_al_FoundInvalidRuleForNonTotalComponent = 080592,

        msg_al_API_WorkflowRemoved = 080594, // MID Track 4554 AnF Enhancement API Workflow
        msg_al_NoAPI_WorkflowToApply = 080595, // MID Track 4554 AnF Enhancment API Workflow
        msg_al_StoreFilterAppliedToMethod = 080596,
        msg_al_ApplyAPIWorkflowFailed = 080597, // Issue 4728
        msg_al_RemoveAPIWorkflowFailed = 080598,    // Issue 4728
        msg_al_PreviousValidNodeQuestion = 080599,
        msg_al_OneAlternateRequired = 080600,
        msg_al_CannotShipHdrWhenStatusNotReleased = 080601,  // MID Track 5689 AnF Defect 1705 Header shipped when not released
        msg_al_IntransitUpdateStoredProcedureFailed = 080602,  // MID Track 5694 MA Enhancement Relieve IT by Header ID
        msg_al_StoreIntransitTableRowsUpdatedSuccessfully = 080603,  // MID Track 5694 MA Enhancement Relieve IT by Header ID
        msg_al_TargetHeaderColor_HasNoMatching_BasisColor = 080604, // MID Track 5778 Scheduler 'Run Now' feature gets error on Audit
        msg_al_RuleMethodPriorHeaderHasZeroAllocation = 080605, // MID Track 5759 Give Warning when Basis Header has zero units allocated 
        msg_al_HeadersExcludedByFilter = 080606, // MID Track 5935 - null reference after header add in workspace
        msg_al_InvalidMerchandiseLevel = 080607,	// MID Track #6101 - Workspace Filter issue
        msg_al_CannotReleaseWhenAllUnitsInReserve = 080630,   // MID Track 6335 Option to not Release Hdr with all units in reserve

        msg_al_CannotDeleteHeaderCharAsGlobalOption = 080637,   // TT#78 - Header Characteristic delete issue
        msg_al_RemoveHeaderCharAsGlobalOption = 080638,   // TT#78 - Header Characteristic delete issue
        msg_al_PlanBasisGradeMismatch = 080639,	// TT 91 - added edit
        msg_al_CannotPerformActionOnHeader = 080640,   // TT#179 Apply Track 6419 to 3.1  // MID Track 6065 Size Proportional Action not valid for Work Up Buy

        msg_al_HeaderCharGroupNotFound = 080642,   // TT#168 - Header Characteristic auto add

        // begin TT#370 Build Packs Enhancement -- J.Ellis
        msg_al_PackPatternNameRequired = 080645,
        msg_al_PackMultipleMustBeGT_0 = 080646,
        msg_al_MaxPacksMustBeGT_0 = 080647,
        msg_al_VendorPatternCannotContainSzRun = 080648,
        msg_al_PackPatternReadOnly = 080649,
        msg_al_CannotModifyMaxPackWhenSzRun = 080650,
        msg_al_VendorPatternCannotBeModified = 080651,
        msg_al_PackMultCannotBeModifiedWhenSzRun = 080652,
        msg_al_SzRunUnitsMustBeGT_0 = 080653,
        msg_al_ReplacingSzRunCausesInvalidPackMult = 080654,
        msg_al_CannotSetSizeValueWhenNoSzRun = 080655,
        msg_al_ComboMustContainAtLeast1Pattern = 080656,
        msg_al_ComboAndMemberPatternTypesMustMatch = 080657,
        msg_al_RemovingVendorCausesInvalidPackCombo = 080658,
        msg_al_VendorIsNotValid = 080659,
        msg_al_NoPatternsFoundForBuildPacksMethod = 080660,
        msg_al_CannotModifyPackMinWhenVendor = 080661,  // TT#787 Vendor Min Order applies only to packs 
        msg_al_PackMinOrderMustBeNonNegative = 080662,  // TT#787 Vendor Min Order applies only to packs
        msg_al_CannotModifySzMultWhenVendor = 080663,
        msg_al_SzMultMustBeGT_0 = 080664,
        msg_al_SizeGroupAndCurveGroupMutuallyExcl = 080665,
        msg_al_ReserveQtyMustBeNonNegative = 080666,
        msg_al_ComboMustBeBuildPacksMethodType = 080667,
        msg_al_BuildPacksRequiresWorkUpSizeBuy = 080668,
        msg_al_NoDefaultSizeCurveDefined = 080669, // TT#438 - Size method use default size curve
        msg_al_NoDefaultSizeCurveForHeader = 080670, // TT#438 - Size method use default size curve

        msg_al_BuildPacksAlreadyDoneOnHeader = 080671,
        msg_al_BuildPacksApplyPackOptionFailed = 080672,
        msg_al_BuildPacksApplyPackOptionSuccess = 080673,
        msg_al_WorkUpBuyReserveExceedsBuildPacksReserve = 080674,
        msg_al_BulkPacksApplyPackOptionPutAllInReserve = 080675,
        msg_al_PackUnitsLessThanMinimum = 080676,
        msg_al_NoDefaultSizeCurveNode = 080677,  // TT#487 - Default size curve issue
        msg_al_CannotProcessMultipleColors = 080678,  // TT#488 - Default size curve issue
        msg_al_WorkUpBuyBulkReserveExceedsBuildPacksReserve = 080679,
        msg_al_WorkUpBuyDesiredBulkExceedsDesiredTotalReserve = 080680,
        msg_al_WorkUpBuyDesiredPacksExceedsDesiredTotalBulkReserveDiff = 080681,
        msg_al_NoPackPatternsGenerated = 080682,
        msg_al_PackUnitsToReserveExceedsPackTotal = 080683,
        msg_al_BulkOrderLessThanMinimum = 080684,
        msg_al_SizeMultipleAssumedToBe1 = 080685,
        msg_al_SizeRID_HasNoIndex = 080686,
        msg_al_BuildPacksMethodHeaderIsChargedToIntransit = 080687,
        msg_al_ComboDoesNotExist = 080688,
        // end TT#370 Build Packs Enhancement -- J.Ellis

        msg_al_SizeWarningPrompt = 080689,  // TT#498 - Use Default warning and Rule Add size issue 
        msg_al_NoSizeCurveOrGroupSelected = 080690,  // TT#499 - Use Default warning and Rule Add size issue 
        msg_al_PackNotAllowedOnWorkupBuy = 080691,  // TT#368 - Workup buy - Allow sizes not in curve
        msg_al_AllUnitsInReserve = 080692, // TT#536 Build Packs Error when no packs generated_
        msg_al_NoPacksGeneratedForOption = 080693, // TT#535 Build Packs Error when no packs generated_
        msg_al_CannotChgWorkUpBuyWhenStatusIs = 080694,  // TT#368 - Work up buy enhancement
        msg_al_DupBuildPacksOptionRemoved = 080695, // TT#580 Build Packs Method generates dup solutions
        msg_al_BuildPacksAllUnitsPackaged = 080696, // TT#612 BP WUB not refreshed after Apply -- unrelated issue
        // Begin TT#634 - JSmith - Color rename
        msg_al_HeaderColorRenameFailed = 080697,
        msg_al_MultiHeaderNoChildrenDeleted = 080698,
        // End TT#634
        msg_al_StoreGradeChangeWarning = 080699,	// TT#618 - Stodd - Allocation Override - Add Attribute Sets (#35)
        msg_al_IncreaseBuyPctCannotBeNegative = 080700, // TT#669 Build Packs Variance Enhancement_
        msg_al_AsBulkAsPackNotEqualReserve = 080701, // TT#667 - Stodd - Pre-allocate Reserve

        msg_al_HeaderFieldLengthError = 080702,   // TT#687 - MultiHeader field exceeds max length
        msg_al_CanOnlyModifyNotesAndChars = 080703,   // TT#712 - Multi Header Release - In Use by Multi Header   
        msg_al_CurrentDateOutsideRange = 080704,   // TT#718-Tasklist for Allocation Workflow not working  
        msg_al_AllRemainingBulkSizeRemovedAfterPacksBuilt = 080705, // TT#744 - JEllis - Use Orig Pack Fitting Logic; Remove Bulk
        msg_al_WeeksToProcess = 080706,   // TT#763- SizeDayToWeekSummary  
                                          //Begin TT#793 - JScott - Size need successful -> get null reference error when selecting Size REview -> application goes into a loop size review never opens
        msg_al_NoSizesToDisplay = 080707,
        //End TT#793 - JScott - Size need successful -> get null reference error when selecting Size REview -> application goes into a loop size review never opens
        //Begin TT#785 - STodd - reapply total allocation
        msg_al_ApplyTotalAllocationFailed = 080708,
        //End TT#785 - Stodd - reapply total allocation
        msg_al_BP_SizeRunContainsSizesNotInHeader = 080709, // TT#886 - Distinct Packs have same size runs
        msg_al_BalanceSizeWithConstraintsBegin = 080710, // TT#843 New Action Size Balance with Constraints
        msg_al_BP_ApplyConfirmation = 080711, //tt#845 - Process and Apply buttons are confusing
        msg_al_BalanceSizeFailed = 080712, // TT#843 New Action Size Balance with Constraints  // TT#1002 - Size Proportional not working in Workflow
        msg_al_BalanceSizeSuccess = 080713, // TT#843 New Action Size Balance with Constraints  // TT#1002 - Size Proportional not working in Workflow
        msg_al_BalanceSizeWithConstraintsEnded = 080714, // TT#843 New Action Size Balance with Constraints
                                                         // Begin TT#967 - JSmith - Allows non style node to be associated with a header
        msg_al_MustBeValidStyle = 080715,
        // End TT#967
        msg_al_BalanceProportionalFailed = 080716, // TT#5494 - AGallagher - Balance Proportional Action Leaving Allocation Out of Balances
        msg_al_BalanceProportionalSuccess = 080717, // TT#5494 - AGallagher - Balance Proportional Action Leaving Allocation Out of Balances
        msg_IT_RebuildIntransitParentOfStyleAutoDeqHeaders = 080720, // TT#1137 (MID Track 4351) Rebuild Intransit Utility
        msg_al_HeaderUpdateFailedDueToRemovedLocks = 080721,  // TT#1185 - Verify ENQ before Update
        msg_al_HeaderCurrentlyLockedByUser = 080722,  // TT#1185 - Verify ENQ before Update
        msg_al_HeaderFailedDueToNoLocks = 080723,  // TT#1185 - Verify ENQ before Update
        msg_al_HeaderEnqFailed = 080724,  // TT#1185 - Verify ENQ before Update
        msg_al_HeaderUpdtFailedDueToEnqIssues = 080725,  // TT#1185 - Verify ENQ before Update
        msg_al_HeaderUpdateFailed = 080726,  // TT#1185 - Verify ENQ before Update
        msg_al_ValueTooLargeWhenSteppedActive = 080727,           // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement            
        msg_al_InventoryMinMaxRequiresBasis = 080729,  // TT#1287 - JEllis - Inventory Minimum/Maximum
        msg_al_AllocationMinMaxBasisInvalid = 080730,  // TT#1287 - JEllis - Inventory Minimum/Maximum
        msg_al_GradeMinMaxUpdatedSuccessfullyAsAllocation = 080731,  // TT#1287 - JEllis - Inventory Minimum/Maximum
        msg_al_GradeMinMaxUpdatedSuccessfullyAsInventory = 080732,  // TT#1287 - JEllis - Inventory Minimum/Maximum
        msg_al_AllocationNeedActionBegins = 080733,           // TT#488 - MD - JEllis - Group Allocation  
        msg_al_AllocationNeedActionEnds = 080734,           // TT#488 - MD - JEllis - Group Allocation  
        msg_al_DynamicColorBasisInvalid = 080735, // TT#1607 - JEllis - Inventory Basis wrong when header has mult color
        msg_al_MultiHeaderCharacteristicWarning = 080736,           // TT#1641 - RMatelic - Warning message should be sent when applying a Header Characteristic to a Multi-Header  
        msg_al_ValueCannotBeNeg = 080737, // TT#1401 - JEllis - Urban Reservation Stores pt 1
        msg_al_IMOUpdateStoredProcedureFailed = 080738,     // TT#1401 - stodd - add resevation stores (IMO)
        msg_al_StoreIMOTableRowsUpdatedSuccessfully = 080739,  // TT#1401 - stodd - add resevation stores (IMO)
        msg_al_HeaderUpdateIMOFailed = 080740,	// TT#1401 - stodd - add resevation stores (IMO)
        msg_al_ValueMustBeBetween0and1 = 080741,  // TT#1401 - JEllis - Urban Reservation Stores pt 5
        msg_al_IMOIdRequired = 080742,  // TT#1401 - stodd - add resevation stores (IMO)
        msg_al_IMOIdWarning = 080743,   // TT#1401 - stodd - add resevation stores (IMO)
        msg_al_IMOIdNotFound = 080744,	// TT#1401 - stodd - add resevation stores (IMO)
        msg_al_unknownHeaderType = 080745,   // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28B
        msg_al_ItemMaxRequired = 080746,   // TT#2083 - gtaylor - Item Max must have a value
        msg_al_FillSizeMthdInvalidWhenBeginDateAndNoCurves = 080747,  // TT#2155 - JEllis - Fill Size Holes Null Reference
        msg_al_SizeNeedMthdInvalidWhenBeginDateAndNoCurves = 080748,  // TT#2155 - JEllis - Fill Size Holes Null Reference
        msg_al_SizeNeedRulesSuccessfullyApplied = 080749,  // TT#2155 - JEllis - Fill Size Holes Null Reference
        msg_al_FillSizeRulesSuccessfullyApplied = 080750,  // TT#2155 - JEllis - Fill Size Holes Null Reference
        msg_al_SpreadFailed_NoRowsToProcess = 080761, // TT#586 - MD - Jellis - Assortment add 2 dimensional spread
        msg_al_SpreadFailed_NoColsToProcess = 080762, // TT#586 - MD - Jellis - Assortment add 2 dimensional spread
        msg_al_RowOrColVectorsIncapatilbeWithMatrix = 080763, // TT#586 - MD - Jellis - Assortment add 2 dimensional spread
        msg_al_NoHeadersToProcess = 080764,   // TT#488 - MD - Jellis - Group Allocation
        msg_NoHeaderSelectedInAssortment = 080765, // TT#696-MD - Stodd - add "active process"
        msg_al_HeaderBelongsToGroupAllocation = 080766, // TT#1019 - MD - stodd - prohibit allocation actions against GA - 
                                                        // Begin TT#1053 - MD - stodd - removing/adding headers to GA - 
        msg_al_HeadersCannotBeAddedToGroupAllocation = 080767,
        msg_al_HeadersCannotBeRemovedFromGroupAllocation = 080768,
        // End TT#1053 - MD - stodd - removing/adding headers to GA - 
        msg_al_NoHeadersSelectedInGroupAllocation = 080769,     // TT#1087 - MD - stodd - size review showing wrong headers 
        msg_al_TryGetSuccessful = 080770, // TT#1176 - MD - Jellis - Group Allocation Size Need not observing inventory min max
        // Begin TT#1222-MD - stodd - When opening Size Review when Group Allocation is in "Group" mode and the matrix tab, size review gets a "No Displayable Sizes" error.
        msg_al_GroupAllocationSizeViewProhibitedCaption = 080771,
        msg_al_GroupAllocationSizeViewProhibited = 080772,
        // End TT#1222-MD - stodd - When opening Size Review when Group Allocation is in "Group" mode and the matrix tab, size review gets a "No Displayable Sizes" error.
        msg_al_BalanceToVSWBegin = 080774,      // TT#1334-MD - stodd - Balance to VSW Action
        msg_al_BelongsToGroupAllocationOrAssortment = 080775,       // TT#1334-MD - stodd - Balance to VSW Action
        msg_al_ActionOnHeaderFailed = 080776, // TT#1334 - Urban - Jellis - Balance to VSW Enhancment
        msg_al_ActionOnHeaderSuccess = 080777, // TT#1334 - Urban - Jellis - Balance to VSW Enhancement
        msg_al_BalanceToVSWEnd = 080778,      // TT#1334-MD - stodd - Balance to VSW Action
        msg_al_BalanceToVSW_ChangedStrQty = 080779, // TT#1396 - MD - Urban - Balance to VSW - Cannot be Negative
        msg_al_HeaderCannotBeReset = 080780,     // TT#1649-MD - stodd - Change Header matching logic to look at "Shipping Started" indicator for a Reset transaction
        msg_al_UnitsPerCartonNotMultiple = 080781,      // TT#1652-MD - RMatelic - DC Carton Rounding 
        msg_al_UnitsPerCartonInvalidForStatus = 080782, // TT#1652-MD - RMatelic - DC Carton Rounding    
        msg_al_UnitsPerCartonInvalidForType = 080783,   // TT#1652-MD - RMatelic - DC Carton Rounding    

        // Begin TT#1652-MD - stodd - DC Carton Rounding
        msg_al_DCCartonRoundingIntransitCharged = 080784,
        msg_al_DCCartonRoundingUnsupportedHeaderType = 080785,
        msg_al_DCCartonRoundingSizesInvalid = 080786,
        msg_al_DCCartonRoundingMoreThanOnePack = 080787,
        msg_al_DCCartonRoundingMoreThanOneColor = 080788,
        msg_al_DCCartonRoundingPacksAndBulk = 080789,
        msg_al_DCCartonRoundingTotalNotDivisibleByMultiple = 080790,
        msg_al_DCCartonRoundingUnitsPerCartonNotDivisibleByMultiple = 080791,

        msg_al_DCCartonRoundingInvalidReceivingDC = 080792,
        msg_al_DCCartonRoundingReceivingDCHasNoStores = 080793,
        // End TT#1652-MD - stodd - DC Carton Rounding
        msg_al_DCCartonRoundingReceivingDCHasNoAllocation = 080798,	// TT#1718-MD - stodd - Processing DC Carton Rounding changed status to Allocated Out of Balance 

        msg_al_DCCartonRoundingNoUnitsPerCarton = 080794,	// TT#1692-md - stodd - DC Carton Rounding - process should not run if Units Per Carton has not been assigned
        msg_al_DCCartonRoundingVSWOnStores = 080795,    // TT#1693-MD - stodd - DC Carton Rounding - add edit to stop processing if any stores have a VSW ID defined.
        msg_al_DCCartonRoundingApplied = 080796,        // TT#1699-MD - stodd - DC Carton Rounding is missing an information message stating it was run against a specific header
        msg_al_UnitsPerCartonInvalid = 080797,			// TT#1703-MD - stodd - Error when Units Per Carton field is blank
        // Begin TT#1753-MD - stodd - Object reference error charging intransit on multi-header
        msg_al_MultiHeaderNewMemberBelongsToGroupAllocation = 080799,
        msg_al_MultiHeaderNewMemberBelongsToAssortment = 080800,
        // End TT#1753-MD - stodd - Object reference error charging intransit on multi-header

        msg_al_RemoveHeaderFromGroupBegin = 080801, // TT#4755 - JSmith - Cannot Remove header from Group
        msg_al_RemoveHeaderFromGroupEnd = 080802, // TT#4755 - JSmith - Cannot Remove header from Group
        msg_al_CannotForceGroupHeaderRemoveWhenNoAppTran = 080803, // TT#4755 - JSmith - Cannot Remove header from Group
        msg_al_CannotRemoveHeaderFromGroupWhenShipStarted = 080804, // TT#4755 - JSmith - Cannot Remove header from Group
        msg_al_CannotRemoveHeaderFromGroupWhen = 080805, // TT#4755 - JSmith - Cannot Remove header from Group
        msg_al_HeaderIsNotMemberOfThisGroup = 080806, // TT#4755 - JSmith - Cannot Remove header from Group
        msg_al_HeaderIsNotGroupHeader = 080807, // TT#4755 - JSmith - Cannot Remove header from Group
        msg_al_RemoveHeadersFromGroupHeaderSuccessful = 080808, // MID Track 4132 Workspace Blank after Remove
        msg_al_RemoveHeadersFromGroupHeaderFailed = 080809, // MID Track 4132 Workspace Blank after Remove
        msg_al_HeaderRemovedFromGroupHeader = 080810,
        msg_al_SummaryReviewNotValidForGroupHeaderMode = 080811,    // TT#4793 - stodd - GA- Heades are different styles-> Velocity basis is color-> get Vel basis error-> accidentally select Summary review-> get Index out of range-> select ok and attemt to cancel group-> get Null reference error.
        msg_al_DCCartonRoundingIntransitChargedMethodSkipped = 080812,   // TT#4911 - Auto Release Workflow Fail on Intransit Charge
        // Begin TT#1966-MD - JSmith- DC Fulfillment
        msg_al_DCFulfillmentProcessedActionNotAllowed = 080813,
        msg_al_DCFulfillmentNotProcessedActionNotAllowed = 080814,
        msg_al_CannotReleaseMaster = 080815,
        msg_al_MustBeMasterHeader = 080816,
        msg_al_DCFulfillmentComplete = 080817,
        msg_al_DCFulfillmentDCOrderNotFound = 080818,
        msg_al_DCFulfillmentDuplicateDCIgnored = 080819,
        msg_al_MasterHeaderNotMatchingPack = 080820,
        msg_al_MasterHeaderUnableToApplyMinimumToAllStores = 080821,
        msg_al_MasterHeaderUnableToApplyDCFulfillmentToAllStores = 080822,
        msg_al_MasterHeaderUnableToApplyDCFulfillmentFromPrimaryDC = 080823,
        msg_al_DCFulfillmentAlreadyProcessed = 080824,
        msg_al_MasterHeaderUnableToDeleteAfterDCFulfillmentProcessed = 080825,
        msg_al_MasterHeaderUnableToDeleteSubordinate = 080826,
        msg_al_MasterHeaderIntransitNotCharged = 080827,
        msg_al_MasterHeaderAtLeastOneEntryRequired = 080828,
        msg_al_HeaderMustHaveDCAssigned = 080829,
        msg_al_DCFulfillmentProcessed = 080830,
        msg_al_DCFulfillmentNotProcessed = 080831,
        msg_al_MasterHeaderCannotBeRelieved = 080832,
        msg_al_ActionNotValidForMasterOrSubordinate = 080833,
        msg_al_MultiHeaderNewMemberBelongsToMasterHeader = 080834,
        msg_al_SubordinateCannotBeAddedToGroupAllocation = 080835,
        // End TT#1966-MD - JSmith- DC Fulfillment
        msg_al_MethodDoesNotContainHeaderDC = 080836,   // TT#2112-MD - JSmith - Subordinate Headers are allocated out of balance
        msg_al_WorklistItemsInUse = 080837, // RO-3444 - JSmith - RO Web Messaging
        msg_al_InvalidStyleGroupBy = 080838, // RO-3444 - JSmith - RO Web Messaging
        msg_al_InvalidSizeGroupBy = 080839, // RO-3444 - JSmith - RO Web Messaging
        msg_al_InvalidSummaryGroupBy = 080840, // RO-3444 - JSmith - RO Web Messaging
        msg_al_SelectedViewNotValid = 080841, // RO-3444 - JSmith - RO Web Messaging
        msg_al_ColorAlreadyContainsSizes = 080842, // RO-3907 - JSmith - RO Web Messaging
        msg_al_PlaceholderCountGreaterThanZero = 080843, // RO-3907 - JSmith - RO Web Messaging
        msg_al_PlaceholderColorCountGreaterThanZero = 080844, // RO-3907 - JSmith - RO Web Messaging
        msg_CloseStyleReviewForInteractive = 080845, // RO-3907 - JSmith - RO Web Messaging

        //Assortment Messages 085000-089999
        msg_as_InvalidVariable = 085000,
        msg_as_NoGeneralAssortment = 085001,
        msg_as_AssrtAlreadyExists = 085002,
        msg_as_InvalidHeaderStatus = 085003,
        msg_as_OnlyOneAssrtAllowed = 085004,
        msg_as_InvalidAssrtStatus = 085005,
        msg_as_CantAddWhenPlaceholderExists = 085006,
        msg_as_NoAssrtRowSelected = 085007,
        msg_as_NoHeaderSelectedForAssrt = 085008,
        msg_as_PlaceholderStyleMismatch = 085009,
        msg_as_PlaceholderEntryNotAllowed = 085010,
        msg_as_GridViewDoesNotExist = 085011,
        msg_as_GridViewWillBeDeleted = 085012,
        msg_as_CannotCloseStyle = 085013,
        msg_as_GenericMessageBoxTitle = 085014,
        msg_as_DupPhStylesNotAllowed = 085015,
        msg_as_InvalideAnchorNodeLevel = 085016,
        msg_as_PhStyleWillChange = 085017,
        msg_as_AsrtNodePhNodeNotLinked = 085018,
        msg_as_SelectOneRowForSaveAs = 085019,
        msg_as_HeadersMustHaveSameStyle = 085020,
        msg_as_PlaceholderBalance = 085021,
        msg_as_NoAssortmentSelected = 085022,
        msg_as_AssortmentDupColorSizeMismatch = 085023,
        msg_as_AssortmentComponentMismatch = 085024,
        msg_as_HeadersMustBeInAssortment = 085025,
        msg_as_ApplyCharsToLowerLevels = 085026,
        msg_as_PlaceholderChangeInvalid = 085027,
        msg_as_NoHeadersSelected = 085028,
        msg_as_NoHeadersPlaceholdersSelected = 085029,

        msg_hl_HeaderTotalsUnitsGT0 = 085030, // TT2280
        msg_hl_HeaderColorUnitsGT0 = 085031, // TT2280
        msg_hl_HeaderSizeUnitsGT0 = 085032, // TT2280
        msg_hl_Header2ColorUnitsGT0 = 085033, // TT2280
        msg_hl_Header2SizeUnitsGT0 = 085034, // TT2280
        msg_hl_Header2ColorSizeUnitsGT0 = 085035, // TT2280
        msg_hl_Header3ColorSizeUnitsGT0 = 085036, // TT2280

        msg_sclp_HeaderNoSizeCurveGrpName = 085037, // TT780
        msg_sclp_HeaderBeginProcessing = 085038, // TT780
        msg_sclp_HeaderTransactionMessageSCNotDefined = 085039, // TT780
        msg_sclp_HeaderTransactionMessageSCNotDefinedMessage1 = 085040, // TT780
        msg_sclp_HeaderTransactionMessageSCNotDefinedMessage2 = 085041, // TT780
        msg_sclp_HeaderEndProcessing = 085042, // TT780
        msg_sclp_HeaderNoStores = 085043, // TT780
        msg_sclp_HeaderNoSizeCurve = 085044, // TT780
        msg_sclp_HeaderGroupInfo = 085045, // TT780
        msg_sclp_HeaderTransactionMessageH = 085046, // TT780
        msg_sclp_HeaderTransactionMessage1 = 085047, // TT780
        msg_sclp_HeaderTransactionMessage2 = 085048, // TT780
        msg_sclp_HeaderTransactionMessage3 = 085049, // TT780
        msg_sclp_HeaderTransactionMessage4 = 085050, // TT780
        msg_sclp_HeaderTransactionMessage5 = 085051, // TT780
        msg_sclp_ProcessGroupStoresFinal = 085052, // TT780
        msg_sclp_ProcessGroupStoresFinalError1 = 085053, // TT780
        msg_sclp_ProcessGroupStoresFinalError2 = 085054, // TT780
        msg_sclp_ProcessGroupStoresFinalError3 = 085055, // TT780
        msg_scglp_NoProduct = 085056, // TT780
        msg_scglp_NoSize = 085057, // TT780
        msg_scglp_ProcessGroupTransFinal = 085058, // TT780      
        msg_scglp_ProcessOneGroup = 085059, // TT780
        msg_scglp_BeginProcessing = 085060, // TT780
        msg_scglp_SizeCurveGroupAction = 085061, // TT780
        msg_scglp_ActionRequired = 085062, // TT780
        msg_scglp_ActionInvalid = 085063, // TT780
        msg_scglp_AlreadyDefined = 085064, // TT780
        msg_scglp_NoSizeCurveTransactions = 085065, // TT780
        msg_scglp_SizeCurveGroupNotDefined = 085066, // TT780
        msg_scglp_EndProcessing = 085067, // TT780
        msg_scglp_BeginProcessingCreateGroup = 085068, // TT780  
        msg_scglp_SizeCurveActionMissing = 085069, // TT780 
        msg_scglp_SizeCurveActionInvalid = 085070, // TT780 
        msg_scglp_MissingDefaultToCreate = 085071, // TT780 
        msg_scglp_InvalidDefaultToCreate = 085072, // TT780 
        msg_scglp_BeginProcessingCreateGroup1 = 085073,// TT780 
        msg_scglp_BeginProcessingCreateGroup2 = 085074,// TT780
        msg_scglp_BeginProcessingCreateGroup3 = 085075,// TT780
        msg_scglp_SizeProfiles = 085076,// TT780
        msg_scglp_InvalidCastException = 085077,// TT780
        msg_scglp_NoSizeTransactionsFound = 085078,// TT780
        msg_scglp_EqualizingSize = 085079,// TT780
        msg_scglp_NoSizeToProcess = 085080,// TT780
        msg_scglp_ErrorEqualizingDefault = 085081,// TT780
        msg_scglp_TransactionSuccessfullyCreated = 085082,// TT780
        msg_scglp_TransactionEncounteredEeditErrors = 085083,// TT780
        msg_scglp_SizeCurveActionRequired = 085084,// TT780
        msg_scglp_SizeCurveActionInvalid2 = 085085,// TT780
        msg_scglp_DefaultSizeCurveAlreadyDefined = 085086,// TT780
        msg_scglp_SizeCurveAlreadyDefined = 085087,// TT780
        msg_scglp_RestoredFromREMOVEList = 085088,// TT780
        msg_scglp_SizeProfiles2 = 085089,// TT780
        msg_scglp_AllSizesRemoved = 085090,// TT780
        msg_scglp_DefaultSizeCurveNOTRemoved = 085091,// TT780
        msg_scglp_SuccessfullyModified = 085092,// TT780
        msg_scglp_EncounteredEditErrors = 085093,// TT780
        msg_scglp_UpdateErrors = 085094,// TT780
        msg_scglp_UpdateErrors1 = 085095,// TT780
        msg_scglp_UpdateErrors2 = 085096,// TT780
        msg_scglp_CodeIDRequired = 085097,// TT780
        msg_scglp_CodeIDNOTDefined = 085098,// TT780
        msg_scglp_CategoryRequired = 085099,// TT780
        msg_scglp_CategoryNOTDefined = 085100,// TT780
        msg_scglp_PrimarySecondaryRequired = 085101,// TT780
        msg_scglp_PrimarySecondaryNOTDefined = 085102,// TT780
        msg_scglp_PrimarySecondaryNOTDefinedInCat = 085103,// TT780
        msg_scglp_SizesNOTDefinedInCat = 085104,// TT780
        msg_scglp_ValueNotNeg = 085105,// TT780
        msg_scglp_DefaultSizeCurveNotDefined = 085106,// TT780
        msg_scglp_SizeCurveNotDefined = 085107,// TT780
        msg_scglp_SizeActionRequired = 085108,// TT780
        msg_scglp_SizeActionInvalid = 085109,// TT780
        msg_as_InvalidPostReceiptView = 085110, // TT#490-MD - stodd -  post-receipts should not show placeholders
        msg_as_PlaceholderHeaderComponentMismatch = 085111, // TT#600-MD - stodd - Dragging a header with  pack(s) does not properly adjust the placeholder
        msg_ValueWasNotFound = 085112,
        msg_NoSelectedGroupAllocation = 085113,     // TT#488-MD - STodd - Group Allocation - 
                                                    // BEGIN TT#742-MD - Stodd - Assortment tooltips
        msg_as_HeaderBelongsToOtherAssortment = 085115,
        msg_as_HeaderNotReceivedInBalance = 085116,
        msg_as_HeaderDropInvalidSecurity = 085117,
        // END TT#742-MD - Stodd - Assortment tooltips
        msg_NoSelectedAssortment = 085114,			// TT#488-MD - STodd - Group Allocation - 
        //TT#691 - Validation issue on Administration-Models-FWOS Override  RBeck 
        msg_as_InvalidDateRange = 085118,
        msg_as_ClearAssortmentInvalidAccess = 085119,  // TT#488 - MD - Jellis - Group Allocation 
        msg_as_CannotClearAssortmentWhenUnitsAllocated = 085120,  // TT#488 - MD - Jellis - Group Allocation   
                                                                  //TT#806-MD-DOConnell-Assortment Placeholder on Content Tab-> ignores all criteria when a general method is processed - seems to use allocation defaults
        msg_as_GenAllocMethNotAllowed = 085121,
        msg_as_ConfirmEmptyGroupAllocationDelete = 085122,    // TT#936 - MD - stodd - Prevent the saving of empty Group Allocations 
        // Begin TT#941 - MD - stodd - When creating a new Group Allocation from headers selected on the allocation workspace, the selected headers are not getting placed into the Group Allocation.
        msg_as_ConfirmAddHeadersToGroupAllocation = 085123,    // TT#936 - MD - stodd - Prevent the saving of empty Group Allocations 
        msg_as_InvalidHeaderStatusGroupAllocation = 085124,    // TT#936 - MD - stodd - Prevent the saving of empty Group Allocations 
        msg_as_InvalidGroupTypeGroupAllocation = 085125,    // TT#936 - MD - stodd - Prevent the saving of empty Group Allocations 
        msg_as_AlreadyInGroupAllocation = 085126,    // TT#936 - MD - stodd - Prevent the saving of empty Group Allocations 
        msg_as_AddHeadersErrorGroupAllocation = 085127,    // TT#936 - MD - stodd - Prevent the saving of empty Group Allocations 
        msg_as_CreateGroupAllocation = 085128,    // TT#936 - MD - stodd - Prevent the saving of empty Group Allocations 
        // End TT#941 - MD - stodd - When creating a new Group Allocation from headers selected on the allocation workspace, the selected headers are not getting placed into the Group Allocation.
        msg_as_GroupAllocationMethodNotAllowed = 085129,    // TT#1030 - MD - stodd - GA method error message
        msg_as_NoProcessingMethodReadOnly = 085130,		// TT#1033 - md - stodd - Able to process Locked Group Allocations - 
        msg_as_NoAuthorityToProcessMethods = 085131,
        // Begin TT#1037 - MD - stodd - read only security -
        msg_as_GroupAllocationReviewScreenReadOnly = 085132,
        msg_as_GroupAllocationHeadersReadOnly = 085133,
        msg_as_AllocationOverrideMethodNotAllowed = 085134, // TT#1060 - md - stodd - allow Alloc Override to process against GA headers - 
                                                            // End TT#1037 - MD - stodd - read only security -
                                                            // Begin TT#1122 - md - stodd - calendar exception creating group allocation - 
        msg_as_HeaderRequiredForGroupAllocation = 085135,
        msg_as_NoValidHeadersSelectedForGroupAllocation = 085136,
        // End TT#1122 - md - stodd - calendar exception creating group allocation - 
        msg_as_GeneralAllocationMethodNotAllowed = 085137,  // TT#1139 - md - stodd - gen alloc can not be process against headers in GA-
        msg_as_GroupAllocationMatrixStoresOut = 085138,	// TT#1204-MD - stodd Provide a message within Matrix when there are no stores available to spread to -
        msg_as_LoadingSavedLayout = 085139,			// TT#1395-MD - stodd - Adding VSW Headers to a Group produces incorrect messages
        msg_as_LoadingDefaultLayout = 085140,		// TT#1395-MD - stodd - Adding VSW Headers to a Group produces incorrect messages
        msg_as_MissingAnchorDate = 085141,      // TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review 
        msg_as_InvalidActionAttributeChanged = 085142,	// TT#1530-MD - stodd - Changing Attribute in Matrix and processing Create Placeholders (& Spread Avg) should give message to user
        msg_as_ForecastLevelBelowStyleNotValidForPlaceholder = 085143, //TT#1560 - MD - DOConnell - Allocation Override method - Do not allow the forecast level to be Style or below if running against a placeholder with a placeholder style
        msg_bc_NoBatchCalcDefined = 085144,			// TT#1595-MD - stodd - Batch comp
        msg_bc_BatchCalcNotFound = 085145,			// TT#1595-MD - stodd - Batch comp
        msg_ValueMustEvenlyDivideInto = 085155,      // TT#1652-MD - RMatelic - DC Carton Rounding
        msg_as_GeneralAllocationMethodNotAllowed_On_Assortment = 085156,  // TT#1986-MD - AGallagher - Processing General Allocation Method on a Assortment Header
        msg_as_ActionNotAllowedOnPlaceholder = 085157,  // TT#2117-MD - JSmith - Placeholder on an Assortment should not allow Need to be processed
        msg_as_CannotRemoveColorFromPostReceipt = 085158,  // RO-4253 - JSmith - Data transport to remove Colors from an Initial Buy Assortment Style - Content

        //Planning Messages 090000-099999
        msg_pl_PlanInUse = 090000,
        msg_pl_PlanInUseSelectAnother = 090001,
        msg_pl_CubeNotDefined = 090002,
        msg_pl_CubeTypeNotDetermined = 090003,
        msg_pl_CellIsReadOnly = 090004,
        msg_pl_CellIsDisplayOnly = 090005,
        msg_pl_CellIsProtected = 090006,
        msg_pl_CellIsClosed = 090007,
        msg_pl_CellIsIneligible = 090008,
        msg_pl_CellIsLocked = 090009,
        msg_pl_InvalidCall = 090010,
        msg_pl_InvalidSetCellMode = 090011,
        msg_pl_CellNotAvailable = 090012,
        msg_pl_InvalidValueFormat = 090013,
        msg_pl_GenericMessageBoxTitle = 090014,
        msg_pl_FormulaConflict = 090015,
        msg_pl_CircularReference = 090016,
        msg_pl_CompChanged = 090017,
        msg_pl_InvalidDateType = 090018,
        msg_pl_InvalidNumberOfIndices = 090019,
        msg_pl_InvalidLogicalCoordinate = 090020,
        msg_pl_DimensionNotDefinedOnCube = 090021,
        msg_pl_InvalidTotalRelationship = 090022,
        msg_pl_InvalidChainStoreCode = 090023,
        msg_pl_StoreAttributeMissing = 090024,
        msg_pl_StoreHierarchyNodeMissing = 090025,
        msg_pl_StoreVersionMissing = 090026,
        msg_pl_PlanDateMissing = 090027,
        msg_pl_ChainHierarchyNodeMissing = 090028,
        msg_pl_ChainVersionMissing = 090029,
        msg_pl_UnfinishedBasisDetail = 090030,
        msg_pl_FillPreviousRow = 090031,
        msg_pl_ViewMissing = 090032,
        msg_pl_StoreDateMissing = 090033,
        msg_pl_ChainDateMissing = 090034,
        msg_pl_ViewNameMissing = 090035,
        msg_pl_ViewExists = 090036,
        msg_pl_ExceededWeekLimit = 090037,
        msg_pl_PlanInUseLine1 = 090038,
        msg_pl_PlanInUseOneWeek = 090039,
        msg_pl_PlanInUseMultipleWeeks = 090040,
        msg_pl_PlanInUseContinue = 090041,
        msg_pl_PlanInUseReselect = 090042,
        msg_pl_InvalidInput = 090043,
        msg_pl_NotLockable = 090044,
        msg_pl_ColumnValuesStillLoading = 090045,
        msg_pl_RowValuesStillLoading = 090046,
        msg_pl_SaveStoreAndChain = 090047,
        msg_pl_SaveStore = 090048,
        msg_pl_SaveChain = 090049,
        msg_pl_PlanHierarchyNodeMissing = 090050,
        msg_pl_BasisHierarchyNodeMissing = 090051,
        msg_pl_PlanVersionMissing = 090052,
        msg_pl_NoAttributeSetsToPlan = 090053,
        msg_pl_TooManyColumns = 090054,
        msg_pl_ForecastMonitoringProblem = 090055,
        msg_pl_StoreNodeNotAuthorized = 090056,
        msg_pl_ChainNodeNotAuthorized = 090057,
        msg_pl_ComputationsNotLoaded = 090058,
        msg_pl_LowLevelsNotDefined = 090059,
        msg_al_SizeNeedExpectedSizePlan = 090060,
        msg_al_MissingSizeNeedAlgorithm = 090061,
        msg_pl_StoreHighLevelHierarchyNodeMissing = 090062,
        msg_pl_ChainHighLevelHierarchyNodeMissing = 090063,
        msg_pl_StoreHighLevelVersionMissing = 090064,
        msg_pl_ChainHighLevelVersionMissing = 090065,
        msg_pl_StoreHighLevelDateMissing = 090066,
        msg_pl_ChainHighLevelDateMissing = 090067,
        msg_pl_StoreLowLevelVersionMissing = 090068,
        msg_pl_ChainLowLevelVersionMissing = 090069,
        msg_pl_StoreLowLevelDateMissing = 090070,
        msg_pl_ChainLowLevelDateMissing = 090071,
        msg_pl_Save = 090072,
        msg_pl_PlanBasisTimeNotSameLength = 090073,
        msg_pl_NumberOfValuesCopied = 090074,
        msg_pl_NoStoresToCopy = 090075,
        msg_pl_TotalNumberOfValuesCopied = 090076,
        msg_pl_MatrixMethodBasisRequired = 090077,
        msg_pl_InvalidComputationsRequested = 090078,
        //Begin Track #4168 - JScott - Error on Store Mulit-level screen when specifying no low-levels
        msg_pl_NoLowLevelsExist = 090079,
        //End Track #4168 - JScott - Error on Store Mulit-level screen when specifying no low-levels
        //Begin Abercrombie & Fitch - JScott - Base changes - Part 19
        msg_pl_NoCellToSpreadTo = 090080,
        //End Abercrombie & Fitch - JScott - Base changes - Part 19
        // BEGIN Issue 4535 - 8.1.07
        msg_pl_EmptyStoreList = 090081,
        msg_pl_EmptyStoreListFromFilter = 090082,
        // End Issue 4535
        msg_pl_NoNodeSelectedForMethod = 090083,
        msg_pl_ForecastVersionReadOnly = 090084,    // Issue 4562 stodd
        msg_pl_BasisRequired = 090085,  // Issue 4233 stodd
        msg_pl_ErrorsInLowLevels = 090086,  // Issue 4858 stodd
        msg_pl_NotAuthorizedToPlan = 090087,    // Issue 4858 stodd

        msg_pl_BasisWeightInvalid = 090088, // ANF - Weighting Multiple Basis
        msg_pl_BasisWeightTotalInvalid = 090089,    // ANF - Weighting Multiple Basis
        msg_pl_BasisWeeksExceedHighLevelWeeks = 090090, // ANF - Weighting Multiple Basis; issue 5149

        //Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
        msg_pl_ChainPlanAccessDenied = 090091,
        msg_pl_StorePlanAccessDenied = 090092,
        msg_pl_ChainBasisAccessDenied = 090093,
        msg_pl_StoreBasisAccessDenied = 090094,
        //End Track #5091 - JScott - Secuirty Lights don't change when permission changes
        //Begin Track #5239 - JScott - Error when opening Chain with Store-only view
        msg_pl_NoDisplayableVariables = 090095,
        //End Track #5239 - JScott - Error when opening Chain with Store-only view
        //Begin Track #5115 - KJohnson - Multi Level Spread
        msg_pl_FromLevelsNotDefined = 090096,
        msg_pl_ToLevelsNotDefined = 090097,
        //Begin Track #5115 - KJohnson - Multi Level Spread
        //Begin TT#824 - JScott - Add Toolbox methods to support external node references
        msg_pl_InvalidNodeIdSpecified = 090098,
        //End TT#824 - JScott - Add Toolbox methods to support external node references
        // Begin TT#83 MD - JSmith - Null reference error if Chain Set Percent values and no weeks to plan.
        msg_pl_NoWeeksSalesNotBalanced = 090099,
        msg_pl_NoWeeksStockNotBalanced = 090100,
        // End TT#83 MD 
        // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options; change edit
        msg_pl_ChartRequiresUnits = 090101,
        // End TT#1748-MD

        //Allocation Methods Messages 100000-109999
        msg_MethodNameRequired = 100000,
        msg_MethodDescriptionRequired = 100001,
        msg_UserOrGlobalRequired = 100002,

        //Filter Messages 110000-110999

        msg_FilterProfileDoesNotMatch = 110000,
        msg_FilterNameExists = 110001,
        msg_FilterNameRequired = 110002,
        //Begin Track #5111 - JScott - Add additional filter functionality
        msg_DateRangeNotValidWithJoin = 110003,
        //End Track #5111 - JScott - Add additional filter functionality
        //Begin Assortment Planning - JScott - Assortment Planning Changes
        msg_FilterContainsData = 110004,
        //End Assortment Planning - JScott - Assortment Planning Changes
        //Begin Track #5111 - JScott - Add additional filter functionality
        msg_FilterInvalid = 110005,
        //End Track #5111 - JScott - Add additional filter functionality
        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        msg_FolderNameExists = 110006,
        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

        // Begin TT#1348-MD - stodd - Store Filters - Change "Save Changes" prompt to be consistent with "Apply Changes" condition
        msg_ProvideValidFilterName = 110008,
        msg_FilterNameInvalid = 110009,
        msg_FilterAlreadyExists = 110010,
        msg_ApplyChangesCondition = 110011,
        msg_ApplyChanges = 110012,
        // End TT#1348-MD - stodd - Store Filters - Change "Save Changes" prompt to be consistent with "Apply Changes" condition
        //Begin TT#1388-MD -jsobek -Product Filter
        msg_DeleteSearchWarning = 110013,
        msg_DeleteSearchWarningCaption = 110014,
        msg_SelectFilterFirst = 110015,

        msg_DenyCopyingUserAllocateTaskToGlobalCaption = 110016, //TT#1446-MD -jsobek -Header Filter - Opening a Task List with a user Header Filter the filter name appears as a number
        msg_DenyCopyingUserAllocateTaskToGlobal = 110017, //TT#1446-MD -jsobek -Header Filter - Opening a Task List with a user Header Filter the filter name appears as a number
        //End TT#1388-MD -jsobek -Product Filter
        msg_StoreGroupFilterMustContainOneSet = 110018,
        msg_StoreGroupFilterMustContainOneSetCaption = 110019,
        msg_StoreGroupFilterCannotHaveDuplicateSetNames = 110020,
        msg_StoreGroupFilterCannotHaveDuplicateSetNamesCaption = 110021,
        msg_StoreGroupDynamicFilterCannotHaveDuplicateField = 110022,
        msg_StoreGroupDynamicFilterCannotHaveDuplicateFieldCaption = 110023,
        //End TT#1517-MD -jsobek -Store Service Optimization
        msg_StoreGroupFilterCannotHaveBlankSetNames = 110024,

        //Scheduler Messages 111000-111999

        msg_TaskListNameExists = 111000,
        msg_JobNameExists = 111001,
        msg_TaskListNameRequired = 111002,
        msg_JobNameRequired = 111003,
        msg_ScheduleNameRequired = 111004,
        msg_ShortcutExists = 111005,
        msg_AtLeastOneMethodRequired = 111006,
        msg_AtLeastOneMerchandiseRequired = 111007,
        msg_CurrentRowIsNotComplete = 111008,
        msg_InputDirectoryRequired = 111009,
        msg_FileSuffixRequired = 111010,
        msg_ScheduleSessionNotAvailable = 111011,
        msg_ScheduleNameExists = 111012,
        msg_RepeatTypeNotDefined = 111013,
        msg_DaysMustBeGreaterThanZero = 111014,
        msg_WeeksMustBeGreaterThanZero = 111015,
        msg_AtLeastOneDayOfWeekRequired = 111016,
        msg_MonthsMustBeGreaterThanZero = 111017,
        msg_MonthWeekTypeRequired = 111018,
        msg_HoursOrMinutesRequired = 111019,
        msg_ConditionDirectoryRequired = 111020,
        msg_ConditionSuffixRequired = 111021,
        msg_RunUntilSuffixRequired = 111022,
        msg_ConditionMustBeSpecified = 111023,
        msg_SchedulerThreadReceivedExceptionRestarting = 111024,
        msg_SchedulerThreadReceivedException = 111025,
        msg_InvalidStatusForAction = 111026,
        msg_HeaderTypeRequired = 111027,
        msg_AtLeastOneWorkflowRequired = 111028,
        msg_RollupDateRequired = 111029,
        msg_RollupFromLevelRequired = 111030,
        msg_ExecutableNotFound = 111031,
        msg_ScanIntervalMustBeLarger = 111032,
        msg_InvalidCharactersInSuffix = 111033,
        msg_DataFileNotFound = 111034,
        msg_SuffixIsInvalid = 111035,
        msg_TaskListHasBeenSubmitted = 111036,
        msg_JobHasBeenSubmitted = 111037,
        msg_AllocationTaskListNotFound = 111038,
        msg_CancelRunningJob = 111039,
        msg_JobDoesNotExist = 111040,
        msg_TaskListIsScheduled = 111041,
        msg_ProgramPathRequired = 111042,
        msg_ConditionSuffixTooLong = 111043,
        msg_FileSuffixTooLong = 111044,
        msg_RunUntilSuffixTooLong = 111045,
        msg_InputDirectoryTooLong = 111046,
        msg_ConditionDirectoryTooLong = 111047,
        msg_TaskListIsActive = 111048,
        msg_JobIsActive = 111049,
        msg_ExecutableDirectoryNotFound = 111050,
        msg_AtLeastOneNodeDescriptorRequired = 111051,
        msg_DefaultToMethodValue = 111052,
        msg_DefaultToWorkflowValue = 111053,
        msg_RollupToLevelRequired = 111054,
        msg_AtLeastOneTaskListRequired = 111055,
        msg_AtLeastOneTaskRequired = 111056,
        msg_RunNowConfirmation = 111057,
        // BEGIN MID Track #2423 - Restart run cycle after schedule is updated
        msg_TerminateJobCycle = 111058,
        msg_TerminateJobCycleAndRunNow = 111059,
        // END MID Track #2423 - Restart run cycle after schedule is updated
        // BEGIN MID Track #3048 - Force job to be held before changing schedule
        msg_HoldOrCancelBeforeModifying = 111060,
        // END MID Track #3048 - Force job to be held before changing schedule
        //Begin Track #4183 - JScott - Add Size Curve Load to scheduler
        msg_SizeCurvePanelNote1 = 111061,
        msg_SizeCurvePanelNote2 = 111062,
        //End Track #4183 - JScott - Add Size Curve Load to scheduler
        //Begin Track #4601 - JScott - System User being changed after Run Now
        msg_NoSecurityToSave = 111063,
        //End Track #4601 - JScott - System User being changed after Run Now
        //Begin Track #4639 - JScott - Invalid delete message from TaskList Explorer
        msg_CannotDeleteRunningTaskList = 111064,
        msg_CannotUpdateRunningTaskList = 111065,
        msg_CannotDeleteRunningJob = 111066,
        msg_CannotUpdateRunningJob = 111067,
        //End Track #4639 - JScott - Invalid delete message from TaskList Explorer
        msg_SpecialRequestNameExists = 111068,  // Issue 5117
        msg_AtLeastOneJobRequired = 111069, // Issue 5117
                                            //Begin Track #5973 - stodd - Long running forecast tasklist
        msg_TaskListBegin = 111070,
        msg_TaskListEnd = 111071,
        //End Track #5973 - stodd 
        msg_AssortmentNameExists = 111072,
        //Begin Track #6423 - JScott - Add Warning when saving task lists
        msg_AtLeastOneNodeDescriptorRecommended = 111073,
        //End Track #6423 - JScott - Add Warning when saving task lists
        //Begin TT#572 - JScott - Cannot view a running tasklist
        msg_OpenTasklistAsReadOnly = 111074,
        msg_OpenJobAsReadOnly = 111075,
        //End TT#572 - JScott - Cannot view a running tasklist
        msg_ConcurrentProcessesMustBe1 = 111076,  // TT#645-MD - JSmith - Add File Processing Direction Parameter to Tasks on Task Lists that Process Input Files 
        msg_ConcurrentProcessesMustBe1Override = 111077,  // TT#1314-MD - JSmith - Tasklist allows multiple concurrent files with FIFO/FILO processing




        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        //Explorer Messages 112000-112999

        msg_ConfirmDeleteExplorerChild = 112000,
        msg_ConfirmDeleteExplorerParent = 112001,
        msg_ConfirmCopyExplorerParent = 112002,
        msg_SharedFolderName = 112003,

        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

        msg_ConfirmApplicationSignOff = 112004,   // TT#4243 - RMatelic - Add a prompt when the user closes the MID app using the "x" in the top right corner.  
        msg_NoBatchCompsDefined = 112005,   // TT#1595-MD - stodd Batch Comp

        msg_StoreProfileChangeAuditLevel = 112010, //TT#1517-MD -jsobek -Store Service Optimization
        msg_StaticStoreGroupChangeAuditLevel = 112011, //TT#1517-MD -jsobek -Store Service Optimization
        msg_DynamicStoreGroupChangeAuditLevel = 112012, //TT#1517-MD -jsobek -Store Service Optimization
        msg_StoreCharacteristicChangeAuditLevel = 112013, //TT#1517-MD -jsobek -Store Service Optimization
        msg_HeaderCharacteristicChangeAuditLevel = 112014, //TT#1517-MD -jsobek -Store Service Optimization

        //Batch Messages 120000-129999

        msg_BatchInputFile = 120000,
        msg_BatchCommitLimitDefaulted = 120001,
        msg_BatchBulkInsertBatchSizeDefaulted = 120002,
        msg_BatchSerializeXMLFileDefaulted = 120003,
        msg_BatchAllowAutoAddsDefaulted = 120004,
        msg_BatchUseBulkInsertDefaulted = 120005,
        msg_BatchBulkInsertProcessReferenceRequired = 120006,
        msg_BatchBulkInsertDatabaseReferenceReuired = 120007,
        msg_InvalidStockTypeLoadCancelled = 120008,
        msg_BatchBulkInsertRetryCountDefaulted = 120009,
        msg_BatchBulkInsertCommandTimeoutDefaulted = 120010,
        msg_BatchRollupValuesDefaulted = 120011,
        msg_FromLevelRequired = 120012,
        msg_FromLevelInvalid = 120013,
        msg_ToLevelInvalid = 120014,
        msg_FromLessThanToLevel = 120015,
        msg_InvalidArgument = 120016,
        msg_ProcessStarting = 120017,
        msg_ProcessEnding = 120018,
        msg_InvalidMultipleSizeCode = 120019,
        msg_HeaderInUseByMultiHeader = 120020,
        msg_InvalidMultiHeaderProcess = 120021,
        msg_InvalidMethod = 120025,
        msg_InvalidTasklistStep = 120026,
        msg_RelieveAllStoresDefaultSetting = 120027,  // MID Track 5694 MA Enhancement Relieve IT by Header ID
                                                      //Begin MOD - JScott - Build Pack Criteria Load
        msg_InvalidBuildPackCriteriaRecord = 120028,
        msg_InvalidBuildPackCriteriaName = 120029,
        msg_InvalidBuildPackCriteriaCompMin = 120030,
        msg_InvalidBuildPackCriteriaSizeMult = 120031,
        msg_InvalidBuildPackCriteriaComboName = 120032,
        msg_InvalidBuildPackCriteriaPackMult = 120033,
        msg_InvalidBuildPackCriteriaMaxPacks = 120034,
        //End MOD - JScott - Build Pack Criteria Load
        // Begin TT#710 - JSmith - Generate relieve intransit
        msg_OutputFileLocation = 120035,
        msg_OutputFileName = 120036,
        msg_HeadersPerFile = 120037,
        msg_FlagFileSuffix = 120038,
        msg_RelieveIntransitHeaderCommand = 120039,
        msg_NoHeadersToRelieve = 120040,
        msg_GenerageRelieveForHeader = 120041,
        msg_ReturnCode = 120042,
        // End TT#710
        /* Begin TT#767 - JSmith - Purge Performance */
        msg_PurgeStarting = 120043,
        msg_PurgeDeterminingDates = 120044,
        msg_PurgeDeterminingDatesCompleted = 120045,
        msg_PurgeRemovingHistoryForecasts = 120046,
        msg_PurgeRemovingHistoryForecastsCompleted = 120047,
        msg_PurgeRemovingHeaders = 120048,
        msg_PurgeRemovingHeadersComplete = 120049,
        msg_PurgeRemovingAudits = 120050,
        msg_PurgeRemovingAuditsComplete = 120051,
        msg_PurgeRemovingSchedules = 120052,
        msg_PurgeRemovingSchedulesComplete = 120053,
        msg_PurgeRemovingDeletedUsers = 120054,
        msg_PurgeRemovingDeletedUsersComplete = 120055,
        msg_PurgeRemovingDeletedGroups = 120056,
        msg_PurgeRemovingDeletedUGroupsComplete = 120057,
        msg_PurgeComplete = 120058,
        /* End TT#767 */
        msg_PurgeRemovingDeletedHierarchyNodes = 120109,   // TT#3630 - JSmith - Delete My Hierarchy
        msg_PurgeRemovingDeletedHierarchyNodesComplete = 120110,   // TT#3630 - JSmith - Delete My Hierarchy
        msg_PurgeDeleteConfirmed = 120111,   // TT#3630 - JSmith - Delete My Hierarchy
        msg_CutoffTimeExceeded = 120112,  // TT#3822 - JSmith - Add Stop Time to Purge
        msg_CutoffTime = 120113,  // TT#3822 - JSmith - Add Stop Time to Purge
        msg_CutoffTimeInvalid = 120114,  // TT#3822 - JSmith - Add Stop Time to Purge

        //Begin TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2
        msg_InvalidChainSetPercentCriteriaRecord = 120059,
        msg_InvalidChainSetPercentCriteriaName = 120060,
        msg_InvalidChainSetPercentCriteriaMerchID = 120061,
        msg_InvalidChainSetPercentCriteriaStoreAtt = 120062,
        msg_InvalidChainSetPercentCriteriaStoreAttSet = 120063,
        msg_InvalidChainSetPercentCriteriaPctage = 120064,
        msg_InvalidChainSetPercentMerchID = 120065,
        msg_InvalidChainSetPercentStoreAtt = 120066,
        msg_InvalidChainSetPercentStoreAttSet = 120067,
        msg_InvalidChainSetPercentPlanWeek = 120068,
        //End TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2

        //Begin TT#1413 - DOConnell - Chain Plan - Set Percentages Phase 1
        msg_CSPNoBalanceNeeded = 120069,
        //End TT#1413 - DOConnell - Chain Plan - Set Percentages Phase 1
        // Begin TT#2198 - JSmith - Headers not purging
        msg_ErrorDeterminingPurgeDates = 120070,
        // End TT#2198

        //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
        msg_InvalidDailyPercentagesCriteriaRecord = 120071,

        //END TT#43 - MD - DOConnell - Projected Sales Enhancement
        // BEGIN TT#739-MD - STodd - delete stores
        msg_PurgeRemovingDeletedEmptyStoreSets = 120072,
        msg_PurgeRemovingDeletedEmptyStoreSetsComplete = 120073,
        msg_PurgeEmptyStoreSetsInUse = 120074,
        // END TT#739-MD - STodd - delete stores

        // BEGIN TT#3265 - DOConnell - performance issue with Store Eligibility Load
        msg_InvaidEligibilityModel = 120075,
        msg_InvaidFWOSModel = 120076,
        msg_InvaidStockModModel = 120077,
        msg_InvalidSalesModModel = 120078,
        msg_InvalidMinPlusSalesValue = 120079,
        msg_InvalidEligiblityValue = 120080,
        msg_InvalidFWOSValue = 120081,
        msg_InvalidStockModValue = 120082,
        msg_InvalidSalesModValue = 120083,
        msg_CannotHaveBoth = 120084,
        msg_InvalidMinShipQtyValue = 120085,
        msg_InvalidVSWStore = 120086,
        msg_InvalidPackThresholdValue = 120087,
        msg_InvalidItemMaxValue = 120088,
        msg_InvalidFWOSMaxModel = 120089,
        msg_InvalidFWOSMaxValue = 120090,
        msg_InvalidPushToBkStk = 120091,
        msg_InvalidType = 120092,
        // END TT#3265 - DOConnell - performance issue with Store Eligibility Load

        // Begin TT#1581-MD - stodd - Header Reconcile API
        msg_FoundMultipleHeaderMatches = 120093,
        msg_FoundMoreThanOneColorOnPack = 120094,
        msg_FoundMoreThanOneColor = 120095,
        msg_FoundInvalidHeaderType = 120096,
        msg_HeaderTranMissingStyle = 120097,
        msg_HeaderTranMissingDistCenter = 120098,
        msg_HeaderTranMissingPurchaseOrder = 120099,
        msg_FoundNoHeaderMatches = 120100,
        msg_HeaderTranContainsHeaderId = 120101,
        msg_IntermediateResetSuccessful = 120102,
        msg_IntermediateResetFailed = 120103,
        msg_InvalidHeaderAction = 120104,
        msg_FoundTwoHeaderMatches = 120105,
        // End TT#1581-MD - stodd - Header Reconcile API

        msg_mustUseItemMaxOrFWOSMax = 120106, //TT#3655 - DOConnell - Uploading Sales Modifier Value breaks inheritance for other Models
        msg_mustHaveTimePeriod = 120107, //TT#3661 - DOConnell - Eligibility Upload - Similar Store Date Range
        msg_StoreWeekPctContributionSpreadFailed = 120108, //TT#541 – MD - DOConnell - OTS Forecast receive a "nothing to spread to exception" when weeks are locked 

        msg_InvalidSimilarStore = 120115, //TT#4375 - DOConnell - Similar store error
                                          // Begin TT#1581-MD - stodd - Header Reconcile API
        msg_HeaderTranMissingKeyField = 120116,
        msg_HeaderTranMissingCharacteristicKeyField = 120117,
        msg_NoHeaderMatchKeys = 120118,
        msg_HeaderMatchKeysNotDefined = 120119,
        msg_NoHeaderIDKeys = 120120,
        msg_HeaderIDKeysNotDefined = 120121,
        msg_NoFieldSpecified = 120122,
        msg_InputDirectoryNotFound = 120123,
        msg_OutputDirectoryNotFound = 120124,
        msg_NoHeaderProcessingKeyFile = 120125,
        // End TT#1581-MD - stodd - Header Reconcile API
        //Begin TT#1386-MD - stodd - Scheduler Job Manager
        msg_InvalidUserName = 120126,
        msg_SchedulerJobManagerAPISuccessful = 120127,
        msg_MatchingJobsTableNull = 120128,
        msg_NoMatchingJobsFound = 120129,
        //End TT#1386-MD - stodd - Scheduler Job Manager
        msg_CannotProcessHeaderType = 120130,   // TT#5048 - JSmith - Allows header type to be changed when group allocation

        // Security Messages 130000-139999 
        msg_PasswordConfirmationFailed = 130000,
        msg_SecurityItemAlreadyExistsInFolder = 130001,
        msg_DropOnMerchandiseFolder = 130002,
        msg_CannotAssignSecurityAtColorOrSize = 130003,
        msg_CannotAssignSecurityToPersonalItems = 130004,
        msg_NotAuthorizedToPlan = 130005,
        //Begin Track #4815 - JSmith - #283-User (Security) Maintenance
        msg_ToBeDeletedDuringPurge = 130006,
        msg_CannotDeleteUserItemsRunning = 130007,
        msg_PermanentMoveWarning = 130008,
        msg_AssignConfirmation = 130009,
        msg_PermanentMoveConfirmation = 130010,
        msg_DeleteAssignedWarning = 130011,
        msg_ActivateUserConfirmation = 130012,
        msg_NotAuthorizedToPlanLowLevel = 130013,
        //Begin TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
        msg_CannotAssignLoggedOnUser = 130014,
        //End TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
        //End Track #4815
        msg_UnauthorizedFunctionAccess = 130015,       // TT#2 - RMatelic - Assortment Planning

        // Size Method Messages 140000-149999
        msg_ColorRequired = 140000,
        msg_SizeRequired = 140001,
        msg_SizeDimensionRequired = 140002,
        msg_FillZeroRequired = 140003,
        msg_InvalidFillZeroEntry = 140004,
        msg_FillUpToRuleQuantityRequired = 140005,
        msg_InvalidFillUpToRuleQuantity = 140006,
        msg_QuantityRuleQuantityRequired = 140007,
        msg_InvalidQuantityRuleQuantity = 140008,
        msg_InvalidRuleQuantity = 140009,
        msg_InvalidDataType = 140010,
        msg_MultipleBasisHeaders = 140011,
        msg_NoHeaderBulkColors = 140012,
        msg_MissingBasisHeader = 140013,
        msg_MissingUnitsPercent = 140014,
        msg_SizeCurveRequired = 140015,
        msg_SizeGroupRequired = 140016,
        msg_ConditionRequired = 140017,
        msg_FilterValueRequired = 140018,
        msg_OperatorRequired = 140019,
        msg_CurvePercentageMustEqual100 = 140020,
        //Begin TT#155 - JScott - Size Curve Method
        msg_NoCurveCreated = 140021,
        //End TT#155 - JScott - Size Curve Method
        //Begin TT#644 - JScott - Unable to allocate using Size Need with size curves at the Class level.
        msg_CurveGroupInUse = 140022,
        //End TT#644 - JScott - Unable to allocate using Size Need with size curves at the Class level.

        //Begin TT#684 - APicchetti - Build Packs Soft Text
        msg_OverrideSizeCurve = 140030,
        msg_OverrideSizeGroup = 140031,
        msg_ApplyHeaderSizes = 140032,
        msg_DeletePackCombo = 140033,
        msg_DeletePack = 140034,
        msg_ApplySizesToPackCombo = 140035,
        msg_SelectedOverrideSizeCurve = 140037,
        msg_SelectedOverrideSizeGroup = 140038,
        msg_PackOptionApplied = 140039,
        msg_InteractiveOneHeader = 140040,
        msg_NoSearchResults = 140041,
        msg_VendorAddAttempt = 140042,
        msg_VerdorDeleteAttempt = 140043,
        btn_Apply = 140044,
        mnu_PackCombinationAdd = 140045,
        mnu_PackCombinationAddPack = 140046,
        mnu_PackCombinationDelete = 140047,
        rdo_ReserveBulkPercent = 140048,
        rdo_ReserveBulkUnits = 140049,
        rdo_ReservePacksPercent = 140050,
        rdo_ReservePacksUnits = 140051,
        rdo_ReservePercent = 140052,
        rdo_ReserveUnits = 140053,
        tab_Method = 140054,
        Property_Names = 140055,

        //Begin tt#728 - Build Packs soft text fixes
        msg_NoMethodName = 140056,
        msg_NoMethodDesc = 140057,
        msg_NoPackQtyOrMax = 140058,
        msg_NoPackCombo = 140059,
        msg_BlankField = 140060,
        msg_NegNumber = 140061,
        msg_DecimalNotAllowed = 140062,
        msg_GreaterThanZeroVar = 140063,
        msg_NegNbrLessThanZero = 140064,
        msg_FieldMust100 = 140065,
        msg_FieldMustField = 140066,
        msg_InteractiveMultipeHeadersSelected = 140067,
        //End tt#728
        //Begin TT#1076 - JScott - Size Curves by Set
        msg_SizeCurveBySetRequired = 140068,
        //End TT#1076 - JScott - Size Curves by Set

        //End TT#684 - APicchetti - Build Packs Soft Text

        // Begin TT#2168 - DOConnell - Size Curve Method 20% min 50% max Issue
        msg_AllSizesLocked = 140069,
        // End TT#2168 - DOConnell - Size Curve Method 20% min 50% max Issue

        // Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
        msg_InvalidDate = 140070,
        //BEGIN TT#280 - MD - DOConnell - Daily Percentages error messages are incorrect
        msg_DailyPctMustEq100 = 140071,
        msg_DailyPctInvalidPercentage = 140072,
        msg_DailyPctDefaultMustEq100 = 140073,
        //END TT#280 - MD - DOConnell - Daily Percentages error messages are incorrect
        // End TT#43 - MD - DOConnell - Projected Sales Enhancement
        msg_EnqueueFailedForHeader = 140074,
        // Begin TT#1019 - MD - stodd - prohibit allocation actions against GA - 
        msg_ActionProtectedGroupAllocation = 140075,
        msg_ProcessProtectedGroupAllocation = 140076,
        // End TT#1019 - MD - stodd - prohibit allocation actions against GA - 

        // Custom User Exceptions 199000 - 199999
        // NOTE -- DO NOT LIST ANY ADDITIONAL USER EXCEPTIONS HERE. THEY SHOULD BE ADDED IN THE COMPUTATIONS.
        msg_CustomUserException = 199000,

        //Form Names 200000-209999
        frm_HierarchyExplorer = 200000,
        frm_StoreExplorer = 200001,
        frm_WorkspaceExplorer = 200002,
        frm_WorkflowMethodExplorer = 200003,
        frm_FilterExplorer = 200004,
        frm_HierarchyProperties = 200010,
        frm_NodeProperties = 200011,
        frm_EligibilityModel = 200012,
        frm_SalesModifierModel = 200013,
        frm_StockModifierModel = 200014,
        frm_GeneralAllocationMethod = 200015,
        frm_RuleMethod = 200016,
        frm_OTSPlanMethod = 200017,
        frm_FillSizeHolesMethod = 200018,
        frm_OverrideMethod = 200019,
        frm_VelocityMethod = 200020,
        frm_AllocationWorkflow = 200021,
        frm_ForecastWorkflow = 200022,
        //		frm_AllocationSchedule							= 200023,
        frm_SimilarStoresSelect = 200024,
        frm_GlobalOptions = 200025,
        frm_CalendarModelList = 200026,
        frm_CalendarModelMaint = 200027,
        frm_CalendarDisplay = 200028,
        frm_Calendar53Week = 200029,
        frm_Administration = 200030,
        frm_AuditViewer = 200031,
        frm_OTSPlanSelection = 200032,
        frm_OTSPlanReview = 200033,
        frm_StyleReview = 200034,
        frm_SummaryReview = 200035,
        frm_SizeReview = 200036,
        frm_StoreCharacteristicMaint = 200037,
        frm_StoreProfileMaint = 200038,
        frm_StoreAttributes = 200039,
        frm_HeaderCharacteristicMaint = 200040,
        frm_TextEditor = 200041,
        frm_BasisSizeMethod = 200042,
        frm_SizeOverrideMethod = 200043,
        frm_SizeWarehouseMethod = 200044,
        frm_SizeNeedMethod = 200045,
        frm_WorkspaceExplorerFilter = 200046,
        frm_SecurityInheritance = 200047,
        frm_SizeConstraintsModel = 200048,
        //frm_SizeFringeModel								= 200049, // MID Track 3619 Remove Fringe
        frm_SizeAlternativeModel = 200050,
        frm_OverrideLowLevelVersions = 200051,
        frm_OTSForecastBalanceMethod = 200052,
        frm_SizeCodeMaint = 200053,
        frm_OTSForecastBalance = 200054,
        frm_About = 200055,
        frm_Forecast_Model = 200056,
        frm_Forecast_Balance_Model = 200057,
        frm_CopyChainForecast = 200058,
        frm_CopyStoreForecast = 200059,
        frm_ForecastChainSpread = 200060,
        frm_GeneralAssortmentMethod = 200061,
        //Begin Assortment Planning - JScott - Assortment Planning Changes
        frm_AssortmentReview = 200062,
        //End Assortment Planning - JScott - Assortment Planning Changes
        frm_AllocationViewSelection = 200063,
        frm_ColorBrowser = 200064,
        frm_ProductCharacteristicMaint = 200065,
        frm_Properties = 200066,
        frm_Export = 200067,
        frm_ForecastModifySales = 200068,
        frm_FWOSModifierModel = 200069,
        frm_UserOptions = 200070,
        frm_ExcludeLowLevels = 200071,
        //Begin Track #4815 - JSmith - #283-User (Security) Maintenance
        //Begin Enhancement - JScott - Export Method - Part 2
        frm_ExportMethod = 200072,
        //End Enhancement - JScott - Export Method - Part 2
        frm_SizeCurves = 200073,
        frm_SizeGroups = 200074,
        frm_AssignUser = 200075,
        //End Track #4815
        //Begin Enhancement #5176 - JSmith - Audit Filter
        frm_AuditFilter = 200076,
        //End Enhancement #5176
        //Begin Enhancement #5004 - KJohnson - Global Unlock
        frm_GlobalUnlock = 200077,
        /* End Enhancement #5004 - KJohnson - Global Unlock */
        /* Begin Enhancement #5196 - KJohnson - Rollup  */
        frm_Rollup = 200079,
        /* End Enhancement #5196 - KJohnson - Rollup */
        // Begin TT#155 - JSmith - Size Curve Method
        frm_SizeCurveMethod = 200082,
        // End TT#155
        frm_NodePropertiesOverridesReport = 200083,  // TT#274 -Unrelated to specific issue
        // Begin TT#370 - APicchetti - Build Packs Method
        frm_BuildPacksMethod = 200084,
        // End TT#370

        frm_AssortmentWorkspace = 200080,   // TT#2 - Assortment Planning
        frm_AssortmentWorkspaceFilter = 200081,   // TT#2 - Assortment Planning
        frm_GlobalLock = 200085,   // TT#43 - MD - DOConnell - Projected Sales Enhancement

        frm_FWOSMaxModel = 200086,   // TT#108 - MD - DOConnell - FWOS Max Model Enhancement
        frm_InUse = 200087,   // TT#110 - MD - RMatleic - In Use Tool
        frm_GroupAllocationMethod = 200089,   // TT#488-MD - Stodd - Group Allocation
        frm_GroupAllocationReview = 200090,		// TT#488-MD - Stodd - Group Allocation
        frm_DCCartonRounding = 200091,	// TT#1652-MD - RMAtelic - DC Carton Rounding
        frm_CreateMasterHeaders = 200092,	// TT#1966-MD - JSmith - DC Fulfillment
        frm_DCFulfillment = 200093,	// TT#1966-MD - JSmith - DC Fulfillment
        frm_PlanningExtractMethod = 200094,  // TT#2131-MD - JSmith - Halo Integration

        //Form Names 300000-309999
        menu_File = 300000,
        menu_File_Close = 300010,
        menu_File_Save = 300020,
        menu_File_SaveAs = 300030,
        menu_File_Export = 300040,
        menu_File_LoginAsAdmin = 300045, //TT#1521-MD -jsobek -Active Directory Authentication
        menu_File_Exit = 300050,
        menu_Edit = 300100,
        //Begin Track #4815 - JSmith - #283-User (Security) Maintenance
        //		menu_Edit_Cut									= 300110,
        //		menu_Edit_Copy									= 300120,
        //		menu_Edit_Paste									= 300130,
        //		menu_Edit_Delete								= 300140,
        //		menu_Edit_Clear									= 300150,
        //		menu_Edit_Find									= 300160,
        menu_Edit_Cut = 300101,
        menu_Edit_Copy = 300102,
        menu_Edit_Paste = 300103,
        menu_Edit_Delete = 300104,
        menu_Edit_Clear = 300105,
        menu_Edit_Find = 300106,
        menu_Edit_Recover = 300107,
        menu_Edit_Assign = 300108,
        menu_Edit_Unassign = 300109,
        //End Track #4815
        menu_Edit_Undo = 300110,
        menu_Edit_Replace = 300111,
        // Begin TT#335 - JSmith - Menu should say Remove and not Delete
        menu_Edit_Remove = 300112,
        // End TT#335
        menu_View = 300200,
        menu_View_Allocation_Workspace = 300210,
        menu_View_Merchandise_Explorer = 300220,
        menu_View_Store_Explorer = 300230,
        menu_View_Workflow_Method_Explorer = 300240,
        menu_View_Filter_Explorer = 300250,
        menu_View_Header_Filter_Explorer = 300251, //TT#1313-MD -jsobek -Header Filters
        menu_View_Assortment_Filter_Explorer = 300252, //TT#1313-MD -jsobek -Header Filters
        menu_View_Tasklist_Explorer = 300260,
        menu_View_Assortment_Workspace = 300270,   // TT#2 Assortment Planning
        menu_Allocation = 300300,
        menu_Allocation_Scheduler = 300301,
        menu_Allocation_Review = 300302,
        menu_Allocation_Select = 300303,
        menu_Allocation_Style = 300304,
        menu_Allocation_Summary = 300305,
        menu_Allocation_Size = 300306,
        //Begin Assortment Planning - JScott - Assortment Planning Changes
        menu_Allocation_Assortment = 300307,
        //End Assortment Planning - JScott - Assortment Planning Changes
        menu_OTS_Plan = 300400,
        menu_OTS_Plan_Review = 300410,
        menu_OTS_Plan_View_Admin = 300420,
        menu_Admin = 300500,
        menu_Admin_Security = 300510,
        menu_Admin_Calendar = 300520,
        menu_Admin_Header_Chars = 300530,
        menu_Admin_Store = 300540,
        menu_Admin_Store_Profiles = 300541,
        menu_Admin_Store_Chars = 300542,
        menu_Admin_Options = 300550,
        menu_Admin_Models = 300560,
        menu_Admin_Models_Eligibility = 300561,
        menu_Admin_Models_Stock_Modifier = 300562,
        menu_Admin_Models_Sales_Modifier = 300563,
        menu_Admin_Models_Forecasting = 300564,
        menu_Admin_Models_Forecast_Balance = 300565,
        menu_Admin_Models_Size_Constraints = 300566,
        //menu_Admin_Models_Size_Fringe					= 300567, // MID Track 3619 Remove Fringe
        menu_Admin_Models_Size_Alternates = 300568,
        menu_Admin_Models_FWOS_Modifier = 300569,
        menu_Admin_Models_Matrix_Forcast = 300570, // BEGIN MID Track #5647 - KJohnson - Matrix Forecast
        menu_Admin_Size = 300580,
        menu_Admin_Size_Groups = 300581,
        menu_Admin_Size_Curves = 300582,
        menu_Admin_Product_Chars = 300590,
        menu_Tools = 300600,
        menu_Tools_Sort = 300610,
        menu_Tools_Filter = 300620,
        menu_Tools_Filter_Wizard = 300630,
        menu_Tools_Audit = 300640,
        menu_Tools_Audit_Reclass = 300645,
        menu_Tools_Release_Resources = 300650,
        menu_Tools_Refresh = 300660,
        //Begin TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
        menu_Tools_Allocation_Analysis = 300665,
        menu_Tools_Forecast_Analysis = 300666,
        //End TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
        menu_Tools_Text_Editor = 300670,
        menu_Tools_Email_Message = 300675,  //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
        menu_Tools_Schedule_Browser = 300680,
        menu_Tools_Theme = 300685,    // MID Track #5006 - Add Theme to Tools menu 
                                      //		menu_Tools_Show_Login							= 300690,
        menu_Tools_User_Options = 300690,
        menu_Tools_Quick_Filter = 300695,
        menu_Tools_Restore_Layout = 300696,    // Workspace Usability Enhancement
        menu_Tools_Activity_Monitor = 300697,    // TT#46 MD - JSmith - User Dashboard 
        menu_Window = 300700,
        menu_Help = 300800,
        menu_Help_About = 300810,
        menu_File_New = 300820,
        menu_Add_Characteristic = 301000,
        menu_Add_Characteristic_Value = 301001,
        menu_Reports = 301100,
        menu_Store = 301002,
        menu_Header = 301003,
        menu_Product = 301004,
        menu_Reports_Custom = 301110,
        menu_Admin_Models_Override_Low_Levels = 301111,
        //Begin Track #6232 - KJohnson - Incorporate Audit Reports in Version 3.0 Base
        menu_Tools_NodePropertiesOverrides = 301112,
        menu_Tools_ForecastAuditMerchandise = 301113,
        menu_Tools_ForecastAuditMethod = 301114,
        menu_Tools_AllocationAudit = 301115,
        //End Track #6232 - KJohnson
        // Begin TT#2 - stodd - assortment
        menu_View_Assortment_Explorer = 301116,
        // End TT#2 - stodd - assortment

        menu_Admin_Models_FWOS_Max = 301117, //TT#108 - MD - DOConnell - FWOS Max Model
        menu_Reports_User_Options_Review = 301118,  //TT#554-MD -jsobek -User Log Level Report
        menu_Group_Allocation_Review = 301119,  // TT#488-MD - Stodd - Group Allocation
        menu_Reports_Allocation_By_Store = 301120,  //TT#739-MD -jsobek -Delete Stores -Allocation by Store Report	// TT#950 - MD - stodd - dup numbers
        menu_Process_Control = 301121,	// TT#1581-MD - stodd - API Header Reconcile
        menu_Scheduler_Job_Manager = 301122,
        // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
        lbl_ApplyMinMaxNone = 806201,
        lbl_ApplyMinMaxStore = 806202,
        lbl_ApplyMinMaxVelocity = 806203,
        // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)

        // Labels 900000-999999
        lbl_ST_ID = 900000,
        lbl_STORE_NAME = 900001,
        lbl_ACTIVE_IND = 900002,
        lbl_CITY = 900003,
        lbl_STATE = 900004,
        lbl_SELLING_SQ_FT = 900005,
        lbl_SELLING_OPEN_DATE = 900006,
        lbl_SELLING_CLOSE_DATE = 900007,
        lbl_STOCK_OPEN_DATE = 900008,
        lbl_STOCK_CLOSE_DATE = 900009,
        lbl_LEAD_TIME = 900010,
        lbl_SHIP_ON_MONDAY = 900011,
        lbl_SHIP_ON_TUESDAY = 900012,
        lbl_SHIP_ON_WEDNESDAY = 900013,
        lbl_SHIP_ON_THURSDAY = 900014,
        lbl_SHIP_ON_FRIDAY = 900015,
        lbl_SHIP_ON_SATURDAY = 900016,
        lbl_SHIP_ON_SUNDAY = 900017,
        lbl_New_Store = 900018,
        lbl_Comp_Store = 900019,
        lbl_NonComp_Store = 900020,
        lbl_StoreGroupBuilder = 900021,
        lbl_StoreStatus = 900022,
        lbl_Method = 900023,
        lbl_STORE_DESC = 900024,
        lbl_Hier_Properties_Form = 900025,
        lbl_Hier_Name = 900026,
        lbl_Hier_Type = 900027,
        lbl_Hier_Type_Organizational = 900028,
        lbl_Hier_Type_Alternate = 900029,
        lbl_Hier_Lvl_Info_gbx = 900030,
        lbl_Hier_Lvls_Col_Name = 900031,
        lbl_Hier_Lvl_Name = 900032,
        lbl_Hier_Lvl_Prop_gbx = 900033,
        lbl_OTS_Info = 900034,
        lbl_OTS_Type = 900035,
        lbl_OTS_Regular = 900036,
        lbl_OTS_Total = 900037,
        lbl_Hier_Lvl_Len = 900038,
        lbl_Hier_Lvl_Len_Unrestricted = 900039,
        lbl_Hier_Lvl_Len_Req_Size = 900040,
        lbl_Hier_Lvl_Len_Range_From = 900041,
        lbl_Hier_Lvl_Len_Range_to = 900042,
        lbl_Button_New = 900043,
        lbl_Button_Delete = 900044,
        lbl_Button_Update = 900045,
        lbl_Button_OK = 900046,
        lbl_Button_Cancel = 900047,
        lbl_Node_Name = 900048,
        lbl_Profile = 900049,
        lbl_Basic_Replenishment = 900050,
        lbl_No = 900051,
        lbl_Yes = 900052,
        lbl_Daily_Percentages = 900053,
        lbl_OTS_Override_gbx = 900054,

        lbl_OTS_Override = 900055,
        lbl_Product_Type_gbx = 900056,
        lbl_Softline = 900057,
        lbl_Hardline = 900058,
        lbl_Undefined = 900059,
        lbl_Node_ID = 900060,
        lbl_Button_Security = 900061,
        lbl_Node_Description = 900062,
        lbl_Store_Eligibility = 900063,
        lbl_Button_Advanced = 900064,
        lbl_Store_Set = 900065,
        lbl_Store_Attribute = 900066,
        lbl_Store_Grades = 900067,
        lbl_Store_Capacity = 900068,
        lbl_Velocity_Grades = 900069,
        lbl_Purge_Criteria = 900070,
        lbl_Purge_Distros = 900071,
        lbl_Weeks = 900072,
        lbl_Days = 900073,
        lbl_Purge_Plans = 900074,
        lbl_Purge_Daily_History = 900075,
        lbl_No_Color = 900076,
        //		lbl_Forecast_Rule_Set			= 900076,
        //		lbl_Allocation_Rule_Set			= 900077,
        //		lbl_Allocation_Rules			= 900078,
        //		lbl_Forecast_Rules				= 900079,
        //		lbl_Post_Allocation_Rules		= 900080,
        //		lbl_Pre_Allocation_Rules		= 900081,
        lbl_Button_Help = 900082,
        lbl_Sort_Ascending = 900083,
        lbl_Sort_Descending = 900084,
        lbl_Search = 900085,
        lbl_Eligibility = 900086,
        lbl_Ineligible = 900087,
        lbl_Stock_Modifier = 900088,
        lbl_Sales_Modifier = 900089,
        lbl_Similar_Store = 900090,
        lbl_Similar_Store_Ratio = 900091,
        lbl_Similar_Store_Until = 900092,
        lbl_Grade = 900093,
        lbl_Boundary = 900094,
        lbl_WOS_Index = 900095,
        lbl_Min_Stock = 900096,
        lbl_Max_Stock = 900097,
        lbl_Min_Ad = 900098,
        lbl_Max_Ad = 900099,
        lbl_Color_Min = 900100,
        lbl_Color_Max = 900101,
        lbl_Sell_Thru_Pct = 900102,
        lbl_Max_Capacity = 900103,
        lbl_Cut = 900104,
        lbl_Copy = 900105,
        lbl_Paste = 900106,
        lbl_Delete = 900107,
        lbl_Insert = 900108,
        lbl_Insert_Before = 900109,
        lbl_Insert_after = 900110,
        lbl_Rules = 900111,
        lbl_Display_Option = 900112,
        lbl_ID_Option = 900113,
        lbl_NodeProperties = 900114,
        lbl_NewNode = 900115,
        lbl_ColorCode = 900116,
        lbl_NewHierarchy = 900117,
        lbl_NewPersonalHierarchy = 900118,
        lbl_NewLevel = 900119,
        lbl_Levels = 900120,
        lbl_HierarchyMaintenance = 900121,
        lbl_MerchandiseExplorer = 900122,
        lbl_Unassigned = 900123,

        //Labels used by Store Group Builder to display "English" SQL Statements
        lbl_False = 900124,
        lbl_True = 900125,
        lbl_Equals = 900126,
        lbl_GThan = 900127,
        lbl_LThan = 900128,
        lbl_Between = 900129,
        lbl_NotEqual = 900130,
        lbl_Like = 900131,
        lbl_In = 900132,
        lbl_NotIn = 900133,
        lbl_And = 900134,
        lbl_Purge_Weekly_History = 900135,
        lbl_Button_Save = 900136,
        lbl_Color_Group = 900137,
        lbl_Button_Apply = 900138,
        lbl_Date_Range = 900139,
        lbl_Day1 = 900140,
        lbl_Day2 = 900141,
        lbl_Day3 = 900142,
        lbl_Day4 = 900143,
        lbl_Day5 = 900144,
        lbl_Day6 = 900145,
        lbl_Day7 = 900146,
        lbl_Total_Percent = 900147,
        lbl_Apply = 900148,
        lbl_Clear_Values = 900149,
        lbl_Reset_Messages = 900150,
        lbl_ExpandAll = 900151,
        lbl_CollapseAll = 900152,
        lbl_Add_Date_Range = 900153,
        lbl_Delete_Date_Range = 900154,
        lbl_ApplyAll = 900155,
        lbl_ApplyColumn = 900156,
        lbl_ClearAll = 900157,
        lbl_ClearColumn = 900158,
        lbl_BalanceTo100 = 900159,
        lbl_Open_Store = 900160,
        lbl_Closed_Store = 900161,
        lbl_Apply_To_Lower_Levels = 900162,
        lbl_Selected = 900163,
        lbl_Multiple_Similar_Stores = 900164,
        lbl_DeleteAll = 900165,

        lbl_Header = 900166,
        lbl_Description = 900167,
        //		lbl_GenAllocMethod				= 900168,
        lbl_Inherited_From = 900169,
        lbl_Preopen_Store = 900170,
        lbl_Button_Process = 900171,
        lbl_Posting_Date = 900172,
        lbl_Date_Not_Set = 900173,
        lbl_Purge_Headers = 900174,
        lbl_OTSPlanLevel = 900175,
        lbl_Button_Close = 900176,
        lbl_Button_Preview = 900177,
        lbl_OTSPlan = 900178,
        lbl_Button_GetHeader = 900179,
        lbl_Name = 900180,
        lbl_Filter = 900181,
        lbl_Override = 900182,
        lbl_ActionOrMethod = 900183,
        lbl_MethodName = 900184,
        lbl_Component = 900185,
        lbl_Specific = 900186,
        lbl_Review = 900187,
        lbl_Tolerance = 900188,
        lbl_Button_Themes = 900189,
        lbl_Attribute = 900190,
        lbl_AttributeSet = 900191,
        lbl_Intransit = 900192,
        lbl_Stores = 900193,
        lbl_AllStores = 900194,
        lbl_AvgWOS = 900195,
        lbl_Rule = 900196,
        lbl_Qty = 900197,
        lbl_Set = 900198,
        lbl_Merchandise = 900199,
        lbl_Beginning = 900200,
        lbl_ShippingTo = 900201,
        lbl_ProcessInteractive = 900202,
        lbl_VelocityStoreDetail = 900203,
        lbl_SimilarStores = 900204,
        lbl_Average = 900205,
        lbl_Ship = 900206,
        lbl_Basis = 900207,
        lbl_HeaderStyle = 900208,
        lbl_Grades = 900209,
        lbl_Matrix = 900210,
        lbl_NoOnHandStores = 900211,
        lbl_StoreViewHeaderTotals = 900212,
        lbl_BalanceLine = 900213,
        lbl_TotalMatrix = 900214,
        lbl_Warehouse = 900215,
        lbl_Percent = 900216,
        lbl_Units = 900217,
        lbl_Reserve = 900218,
        lbl_Target = 900219,
        lbl_Settings = 900220,
        lbl_StoreGradeTime = 900221,
        lbl_PctNeedLimit = 900222,
        lbl_ExceedsMax = 900223,
        lbl_OnHand = 900224,
        lbl_FactorPct = 900225,
        lbl_ColorMultiple = 900226,
        lbl_SizeMultiple = 900227,
        lbl_ExceedCapacity = 900228,
        lbl_Constraints = 900229,
        lbl_ExceedByPct = 900230,
        lbl_Color = 900231,
        lbl_Minimum = 900232,
        lbl_Maximum = 900233,
        lbl_StoreSingular = 900234,
        lbl_StoreGradeSingular = 900235,
        lbl_ColorMinMax = 900236,
        lbl_Pack = 900237,
        lbl_Ascending = 900238,
        lbl_Descending = 900239,
        lbl_IncludedStores = 900240,
        lbl_ExcludedStores = 900241,
        lbl_Quantity = 900242,
        lbl_Prior = 900243,
        lbl_GroupBy = 900244,
        lbl_View = 900245,
        lbl_NeedAnalysis = 900246,
        lbl_TimePeriodBegin = 900247,
        lbl_TimePeriodEnd = 900248,
        lbl_AllocHeaders = 900249,
        lbl_Condition = 900250,
        lbl_Value = 900251,
        lbl_Variable = 900252,

        /* CONTEXT MENU'S FOR STORE EXPLORER*/
        lbl_RemoveStoreFromSet = 900253,
        lbl_NewAttribute = 900254,
        lbl_NewAttributeSet = 900255,
        lbl_NewStore = 900256,
        lbl_Rename = 900257,
        lbl_StoreProfile = 900258,
        lbl_RefreshStoreExplorer = 900259,
        lbl_Edit = 900260,

        lbl_TotalSales = 900261,
        lbl_AvgSales = 900262,
        lbl_PctTotalSales = 900263,
        lbl_AvgSalesIndex = 900264,
        lbl_Version = 900265,
        lbl_VelocityGrade = 900266,
        lbl_Total = 900267,
        lbl_Headers = 900268,
        lbl_QtyToAllocate = 900269,
        lbl_DimSize = 900270,
        lbl_Dimension = 900271,
        lbl_Forecast = 900272,
        lbl_Workflow = 900273,
        //Begin TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
        //lbl_AvailableStores				= 900274,
        //End TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
        lbl_APIWorkflow = 900275,
        lbl_Allocate = 900276,
        lbl_PlanTypeStore = 900277,
        lbl_PlanTypeChain = 900278,
        lbl_PlanTypeMultiLevel = 900279,
        lbl_IncludeIneligibleStore = 900280,
        lbl_IncludeSimilarStores = 900281,
        lbl_HighLevel = 900282,
        lbl_LowLevels = 900283,
        lbl_PlanTimePeriod = 900284,
        lbl_Button_OverrideLowVersion = 900285,
        lbl_Button_AddBasis = 900286,
        lbl_Button_AddBasisDetails = 900287,
        lbl_Weight = 900288,
        lbl_PlanType = 900289,
        lbl_DisplayOptions = 900290,
        lbl_Exclude = 900291,
        lbl_SizeCurve = 900292,
        lbl_ConstraintsModel = 900293,
        lbl_AlternatesModel = 900294,
        lbl_ComputationMode = 900295,
        lbl_Iterations = 900296,
        lbl_BalanceMode = 900297,
        lbl_Options = 900298,
        lbl_Properties = 900299,
        lbl_Using = 900300,
        lbl_Balance = 900301,
        lbl_Process = 900302,
        lbl_SelectHeaderRelease = 900303,
        lbl_SizeCode = 900304,
        lbl_New = 900305,
        lbl_SizeProductCategory = 900306,
        lbl_SizePrimary = 900307,
        lbl_SizeSecondary = 900308,
        lbl_Configuration = 900309,
        lbl_Assemblies = 900310,
        lbl_AddOns = 900311,
        lbl_Hide = 900312,
        lbl_Details = 900313,
        lbl_Allocation = 900314,
        lbl_Size = 900315,
        lbl_Planning = 900316,
        lbl_Assortment = 900317,
        // (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
        lbl_Master = 900318,
        lbl_MasterSubord = 900319,
        lbl_AllocatedUnits = 900320,
        lbl_OrigAllocatedUnits = 900321,
        lbl_ReserveUnits = 900322,
        lbl_Subordinate = 900323,
        lbl_RsvAllocatedUnits = 900324,
        // (CSMITH) - END MID Track #3219
        lbl_CopyTo = 900325,
        lbl_MultiHeaderID = 900326,
        lbl_HeaderID = 900327,

        lbl_Type = 900328,
        lbl_Date = 900329,
        lbl_UnitRetail = 900330,
        lbl_UnitCost = 900331,
        lbl_SizeGroup = 900332,
        lbl_Multiple = 900333,
        lbl_PurchaseOrder = 900334,
        lbl_Vendor = 900335,
        lbl_DistCenter = 900336,
        lbl_HeaderStatus = 900337,
        lbl_ShipStatus = 900338,
        lbl_Release = 900339,
        lbl_ChildTotal = 900340,
        lbl_PackType = 900341,
        lbl_TotalPacks = 900342,
        lbl_QtyPerPack = 900343,
        lbl_TotalQty = 900344,
        lbl_PackColor = 900345,
        lbl_PackColorDesc = 900346,
        lbl_BulkColor = 900347,
        lbl_BulkColorDesc = 900348,
        lbl_RestoreLayout = 900349,
        lbl_SpreadFrom = 900350,
        lbl_WorkspaceDescription = 900351,
        lbl_SimilarStoreModel = 900352,
        //Begin Track #3863 - JScott - OTS Forecast Level Defaults
        lbl_OTS_Level_Type = 900353,
        lbl_OTS_Node_Level = 900354,
        lbl_OTS_Node_Type = 900355,
        //End Track #3863 - JScott - OTS Forecast Level Defaults
        lbl_NoSecondarySize = 900356,  // MID Track 3914 Constraints not handling 'no secondary size'
        lbl_Receipts = 900357,
        lbl_Sales = 900358,
        lbl_Stock = 900359,
        lbl_CurrentOnHand = 900360,
        lbl_Index = 900361,
        lbl_Button_Add = 900362,
        lbl_Product = 900363,
        lbl_Transaction = 900364,
        lbl_Action_Move = 900365,
        lbl_Action_Delete = 900366,
        lbl_Action_Rename = 900367,
        lbl_Tasklist = 900368,
        lbl_Stop = 900369,
        lbl_SearchID = 900370,
        lbl_SearchName = 900371,
        lbl_SearchDescription = 900372,
        lbl_SearchLevels = 900373,
        lbl_Locate = 900374,
        lbl_SearchOptions = 900375,
        lbl_SearchMatchCase = 900376,
        lbl_SearchMatchWholeWord = 900377,
        lbl_SelectAllEntries = 900378,
        lbl_ClearAllEntries = 900379,
        lbl_ShowDetails = 900380,
        lbl_Remove = 900381,
        lbl_MultiHeader = 900382,
        lbl_Create = 900383,
        lbl_AddTo = 900384,
        lbl_RemoveFrom = 900385,
        lbl_PackSize = 900386,
        lbl_BulkSize = 900387,
        lbl_AssortmentID = 900388,
        lbl_ColumnChooser = 900389,
        lbl_HeaderNotes = 900390,
        lbl_DeleteRow = 900391,
        lbl_User = 900392,
        lbl_NumberOfPlaceholderColors = 900393,
        lbl_SearchGroupName = 900394,
        lbl_NewCharacteristic = 900395,
        lbl_NewCharacteristicValue = 900396,
        lbl_SearchValue = 900397,
        lbl_SearchCharacteristic = 900398,
        lbl_Characteristics = 900399,
        lbl_Characteristic = 900400,
        lbl_Assigned = 900401,
        lbl_Hierarchy = 900402,
        lbl_UseCharacteristics = 900403,
        lbl_PlaceholderID = 900404,
        lbl_Excel = 900405,
        lbl_File = 900406,
        lbl_IncludeAllSets = 900407,
        lbl_IncludeCurrentSet = 900408,
        lbl_IncludeFormatting = 900409,
        lbl_Location = 900410,
        lbl_Include = 900411,
        lbl_SelectAction = 900412,
        lbl_AddPlaceholderStyle = 900413,
        lbl_PhStyle = 900414,
        lbl_HeaderLinkCharacteristic = 900415,
        lbl_None = 900416,
        lbl_SecondarySizeTotal = 900417,  // MID Track 3611 Quick Filter not working in Size Review
        lbl_GenericSizeCurve = 900418,
        lbl_NoHeaderCharSelected = 900419,
        lbl_NoHierLevelSelected = 900420,
        lbl_SalesMatrix = 900421,
        lbl_PhStyleName = 900422,
        lbl_PhColorID = 900423,
        lbl_PhColorName = 900424,
        lbl_OTS_Hierarchy = 900425,
        lbl_OTS_Node = 900426,
        lbl_OTS_Starts_With = 900427,
        lbl_SaveView = 900428,
        lbl_Views = 900429,
        lbl_ViewName = 900430,
        lbl_DeleteView = 900431,
        lbl_SaveFilter = 900432,
        lbl_HierarchyNode = 900433,
        lbl_AnchorNode = 900434,
        lbl_ProtectInterfacedHeaders = 900435,  // MID Track 4357 Interfaced header security
        lbl_RequireSizeFilters = 900436,
        lbl_CharacterMask = 900437,
        lbl_Size_Group = 900438,
        lbl_SizeAlternates = 900439,
        lbl_SizeConstraints = 900440,
        lbl_Generic = 900441,
        lbl_FilterSelection = 900442,
        lbl_FilterSelectionText = 900443,
        lbl_FWOS_Modifier = 900444,  // MID Track #4370 - John Smith - FWOS Models
        lbl_On = 900445,
        lbl_Off = 900446,
        lbl_ForecastMonitor = 900447,
        lbl_ModifySalesMonitor = 900448,
        lbl_AuditLoggingLevel = 900449,
        lbl_Button_Directory = 900450,
        lbl_CurrentSalesWeek = 900451,
        lbl_DailySalesThru = 900452,
        lbl_Period = 900453,
        lbl_Week = 900454,
        lbl_Day = 900455,
        lbl_Dynamic = 900456,
        lbl_Static = 900457,
        lbl_Reoccurring = 900458,
        lbl_DynamicSwitch = 900459,
        lbl_RelativeToCurrent = 900460,
        lbl_RelativeToPlan = 900461,
        lbl_RelativeToStore = 900462,
        lbl_ForecastLowLevels = 900463,
        lbl_Button_ExcludeLowLvls = 900464,
        lbl_ForecastHighLevel = 900465,
        lbl_Stock_MinMax = 900466,
        lbl_Button_AddNewStockMinMax = 900467,
        lbl_ApplyMinMaxes = 900468,
        lbl_InheritFrom = 900469,
        lbl_LowLevelSetting = 900470,
        //lbl_Hierarchy					= 900471,
        lbl_NoneNoBrackets = 900472,
        lbl_Workflows = 900473,
        lbl_FV_Description = 900474,
        lbl_FV_ProtectHistory = 900475,
        lbl_FV_Active = 900476,
        lbl_FV_Combine = 900477,
        lbl_FV_CombineActual = 900478,
        lbl_FV_CombineForecast = 900479,
        lbl_FV_CombineCurrentMonth = 900480,
        //Begin Track #4547 - JSmith - Add similar stores by forecast versions
        lbl_FV_SimilarStore = 900481,
        //End Track #4547
        lbl_LowLevelDefault = 900483, // 4573 stodd
        lbl_ThisYear = 900485,
        lbl_LastYear = 900486,
        lbl_ApplyTo = 900487,
        lbl_PercentContribution = 900488,

        //Begin Track #4619 - JSmith - Add auto-upgrade
        lbl_IncompatibleClient = 900484,
        //End Track #4619
        // BEGIN Issue 4884
        lbl_TY = 900489,
        lbl_LY = 900490,
        lbl_ApplyTrendTo = 900491,
        lbl_TrendCaps = 900492,
        lbl_Criteria = 900493,
        lbl_StockMinMax = 900494,
        lbl_SetMethods = 900495,
        // END Issue 4884
        //Begin Track #4815 - JSmith - #283-User (Security) Maintenance
        lbl_PermanentlyMoveUser = 900496,
        lbl_AssignTo = 900497,
        //End Track #4815
        lbl_StoreGradesByBasis = 900498,    // Track #6074 stodd
        lbl_DefaultStoreGradesByBasis = 900499, // Track #6074 stodd



        //Begin Enhancement - JScott - Export Method - Part 2
        lbl_ExportMethod_Criteria = 900500,
        lbl_ExportMethod_LowLevels = 900501,
        lbl_ExportMethod_LowLevelsOnly = 900502,
        lbl_ExportMethod_ChainData = 900503,
        lbl_ExportMethod_StoreData = 900504,
        lbl_ExportMethod_Options = 900505,
        lbl_ExportMethod_Variables = 900506,
        lbl_ExportMethod_Format = 900507,
        lbl_ExportMethod_UseDefaults = 900508,
        lbl_ExportMethod_FileFormat = 900509,
        lbl_ExportMethod_CSV = 900510,
        lbl_ExportMethod_XML = 900511,
        lbl_ExportMethod_Delimeter = 900512,
        lbl_ExportMethod_OutputOptions = 900513,
        lbl_ExportMethod_FilePath = 900514,
        lbl_ExportMethod_BrowseFilePath = 900515,
        lbl_ExportMethod_FileNameInfo = 900516,
        lbl_ExportMethod_AddToFileName = 900517,
        lbl_ExportMethod_DateStamp = 900518,
        lbl_ExportMethod_TimeStamp = 900519,
        lbl_ExportMethod_SplitFiles = 900520,
        lbl_ExportMethod_SplitMerchandise = 900521,
        lbl_ExportMethod_SplitNone = 900522,
        lbl_ExportMethod_SplitNumEntries = 900523,
        lbl_ExportMethod_CreateFlagFile = 900524,
        lbl_ExportMethod_FlagSuffix = 900525,
        lbl_ExportMethod_CreateEndFile = 900526,
        lbl_ExportMethod_EndSuffix = 900527,
        lbl_ExportMethod_ConcurrentProcesses = 900528,
        //Begin Track #4942 - JScott - Correct problems in Export Method
        //		lbl_ExportMethod_ValueType		= 900529,
        lbl_ExportMethod_OutputFormat = 900529,
        //End Track #4942 - JScott - Correct problems in Export Method
        lbl_ExportMethod_PreinitValues = 900530,
        lbl_ExportMethod_ShowIneligible = 900531,
        // BEGIN MID Track #4827 - John Smith - Presentation plus sales
        lbl_PMPlusSales = 900532,
        // END MID Track #4827
        //Begin Track #4547 - JSmith - Add similar stores by forecast versions
        lbl_NormalizeSizeCurves = 900533,
        lbl_OverrideDefault = 900534,
        //End Track #4547
        //Begin Track #4924 - JSmith - Add inherit from higher level
        lbl_Inherit_From_Higher_Level = 900535,
        //End Track #4924
        //End Enhancement - JScott - Export Method - Part 2
        //Begin Track #4942 - JScott - Correct problems in Export Method
        lbl_ExportMethod_DateType = 900536,
        lbl_ExportMethod_Calendar = 900537,
        lbl_ExportMethod_Fiscal = 900538,
        lbl_ExportMethod_CSVSuffix = 900539,
        //End Track #4942 - JScott - Correct problems in Export Method
        lbl_Button_SaveAs = 900540, // MID Track #4970 - add button text
        lbl_ModelName = 900541,
        lbl_PrimarySizeCurve = 900542,
        lbl_AlternateSizeCurve = 900543,
        lbl_Clear = 900544,
        lbl_Verify = 900545,
        // BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
        lbl_Fill_SizesTo = 900546,
        // End MID Track #4921
        // BEGIN MID Track #5170 - JSmith - Model enhancements
        lbl_Case_Sensitive = 900547,
        // End MID Track #5170
        //Begin Enhancement #5176 - JSmith - Audit Filter
        lbl_AF_HighestProcessLevel = 900548,
        lbl_AF_RunDate = 900549,
        lbl_AF_RunDateAll = 900550,
        lbl_AF_RunDateBetweenAnd = 900551,
        lbl_AF_RunDateBetweenDays = 900552,
        lbl_AF_RunDateSpecifyTo = 900554,
        lbl_AF_RunDateSpecifyFrom = 900555,
        lbl_AF_RunDateSpecify = 900556,
        lbl_AF_RunDateBetween = 900557,
        lbl_AF_RunDateToday = 900558,
        lbl_AF_HighestDetailLevel = 900559,
        lbl_AF_Duration = 900560,
        lbl_AF_DurationMinutes = 900561,
        lbl_AF_Status = 900562,
        lbl_AF_StatusRunning = 900563,
        lbl_AF_StatusCompleted = 900564,
        lbl_AF_MyTasksOnly = 900565,
        //End Enhancement #5176
        //Begin Enhancement #5004 - KJohnson - Global Unlock
        lbl_UnlockForm = 900566,
        lbl_cbMultiLevel = 900567,
        lbl_FromLevel = 900568,
        lbl_ToLevel = 900569,
        lbl_btnOverride = 900570,
        lbl_OptionsForm = 900571,
        lbl_cbStores = 900572,
        lbl_cbChain = 900573,
        lbl_StoreOptionsForm = 900574,
        lbl_Attribute2 = 900575,
        lbl_Filter2 = 900576,
        lbl_AttributeSet2 = 900577,
        lbl_LastProcessed = 900578,
        lbl_DateTime = 900579,
        lbl_ByUser = 900580,
        //End Enhancement #5004
        // BEGIN Issue 5117
        lbl_SpecialRequest = 900581,
        lbl_JobConcurrency = 900582,
        // END Issue 5117
        lbl_PhStyleID = 900583,
        //Begin Enhancement #5196 - KJohnson - Rollup
        lbl_Button_AddNewRollup = 900584,
        //End Enhancement #5196
        //Begin Track #5395 - JScott - Add ability to discard zero values in Export
        lbl_ExportMethod_ExcludeZeroValues = 900585,
        //End Track #5395 - JScott - Add ability to discard zero values in Export
        lbl_OverrideModel_Custom = 900586,
        lbl_OverrideModel_Model = 900587,
        lbl_OverrideModel_Low_Level = 900588,
        lbl_Rollup = 900589,
        // BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement
        lbl_Button_AddVariable = 900590,
        lbl_ApplyModifiers = 900591,
        lbl_ForecastFormula = 900592,
        lbl_AssociatedSalesVar = 900593,
        lbl_StockMin = 900594,
        lbl_StockMax = 900595,
        lbl_FWOS_Sales = 900596,
        lbl_FWOS_Stock = 900597,
        lbl_PctContribution = 900598,
        // END MID Track #5773
        // BEGIN MID Track #6187 stodd
        lbl_UsePlanAsBasis = 900600,
        lbl_BasisOverride = 900601,

        // END MID Track #6187 stodd
        // BEGIN MID Track #6271 stodd
        lbl_AllowChainNegatives = 900603,
        // End MID Track #6271 stodd
        lbl_HideDetails = 900604,   // Workpsace Usability
        lbl_Assortments = 900605,
        lbl_AssortmentQtys = 900606,

        // Begin TT#2 - stodd - assortment
        lbl_Delivery = 900607,
        lbl_Committed = 900608,
        // End TT#2 - stodd - assortment
        lbl_NumPacks = 900610,   // Workpsace Usability
        lbl_NumBulkColors = 900611,   // Workpsace Usability
        lbl_NumBulkSizes = 900612,   // Workpsace Usability

        //BEGIN Test Track #153 apicchetti
        lbl_TotalNumStores = 900613,
        lbl_StockPercentOfTotal = 900614,
        lbl_AvgStock = 900615,
        lbl_AllocationPercentOfTotal = 900616,
        //END Test Track #153 apicchetti

        //BEGIN TT#154 – Add variables to Allocation workspace - apicchetti
        lbl_NumberOfStores = 900617,
        //END TT#154 – Add variables to Allocation workspace - apicchetti

        //BEGIN TT#152 – Velocity balance - apicchetti
        lbl_Checkbox_VelocityBalance = 900618,
        //END TT#152 – Velocity balance - apicchetti
        lbl_VelocityMatrix = 900619,   // TT#231 - Add Velocity Matrix  & Store Detail views
                                       // Begin TT#155 - JSmith - Size Curve Method
        lbl_MerchandiseBasis = 900620,
        lbl_CurveBasis = 900621,
        lbl_EqualizeWeighting = 900622,
        // End TT#155 - JSmith - Size Curve Method
        // Begin TT#356 - RMatelic- Allocation Options->detail pack settings - will not override to blank within the method 
        //Begin TT#155 - JScott - Add Size Curve info to Node Properties
        lbl_Size_Curves = 900623,
        lbl_Size_Curves_Tolerance = 900624,
        lbl_Size_Curves_Inherited_Criteria = 900625,
        lbl_Size_Curves_Criteria = 900626,
        lbl_Size_Curves_Similar_Store = 900627,
        lbl_Default = 900628,
        lbl_Lost_Sales = 900629,
        lbl_Include_Exclude = 900630,
        lbl_Curve_Name = 900631,
        lbl_Add = 900632,
        // Begin TT#274 - RMatelic - Purge Criteria Node Override Report issues 
        lbl_Modifiers = 900633,
        lbl_AllocationMinMax = 900634,
        lbl_AllocationAudit = 900635,
        lbl_Asterisks = 900636,
        lbl_Capacity = 900637,
        lbl_DailyPcts = 900638,
        lbl_PurgeCriteria = 900639,
        lbl_ForecastLevel = 900640,
        lbl_OR = 900641,
        // End TT#274
        lbl_AvgPackDevTolerance = 900642,
        lbl_MaxPackAllocNeedTolerance = 900643,
        // End TT#356
        // Begin TT#384 - JSmith - Error removing child from hierarchy
        lbl_AutoRefresh = 900644,
        // End TT#384

        // Begin TT#391 - STodd - Size Day to Week Summary
        lbl_SizeDayToWeekSummary = 900645,
        lbl_DateRangeToProcess = 900646,
        // End TT#391 - STodd - Size Day to Week Summary
        //Begin TT#155 - JScott - Add Size Curve info to Node Properties
        lbl_HigherLevelSalesTolerance = 900647,
        lbl_MinimumAvgPerSize = 900648,
        lbl_HighestLevel = 900649,
        lbl_ApplyChainSales = 900650,
        lbl_SalesTolerance = 900651,
        lbl_IndexToAverage = 900652,
        lbl_SalesToleranceUnits = 900653,
        lbl_MinMaxTolerancePct = 900654,
        lbl_MinTolerancePct = 900655,
        lbl_MaxTolerancePct = 900656,
        //End TT#155 - JScott - Add Size Curve info to Node Properties
        // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
        lbl_GenericSizeCurveName = 900657,
        lbl_Use = 900658,
        lbl_NodePropertiesName = 900659,
        lbl_HeaderCharacteristics = 900660,
        lbl_UseDefaultCurve = 900661,
        // End TT#413
        // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
        lbl_NoNameExtensionSelected = 900662,
        // End TT#413
        // Begin TT#391 - stodd - adding in stock sales to global options.
        lbl_GenerateSizeCurvesUsing = 900663,
        // End TT#391
        //Begin TT#450 - JScott - SZ CURVE- node properties->tolerance->minimum average per size - store 1101 should get style curve, instead getting style/color chain curve - not correct
        lbl_NoHigherLevel = 900664,
        //End TT#450 - JScott - SZ CURVE- node properties->tolerance->minimum average per size - store 1101 should get style curve, instead getting style/color chain curve - not correct
        // Begin TT#391 - STodd - Size Day to Week Summary
        lbl_OverrideMerchandise = 900665,
        // End TT#391 - STodd - Size Day to Week Summary
        lbl_DupSizeNameSeparator = 900666,   // TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
        lbl_AndBasisSubstitute = 900667,    // TT#498 - Size Need Use Default warning message
                                            //Begin TT#483 - JScott - Add Size Lost Sales criteria and processing */
        lbl_OutOfStock = 900668,
        lbl_SellThru = 900669,
        lbl_SellThruLimit = 900670,
        //End TT#483 - JScott - Add Size Lost Sales criteria and processing */
        // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        lbl_Generic_Pack_Rounding = 900671,
        lbl_Generic_Pack_1st_Pack_Rounding_Up_From = 900672,
        lbl_Generic_Pack_Nth_Pack_Rounding_Up_From = 900673,
        lbl_Override_Pack_Multiple = 900674,
        lbl_Override_Pack_Rounding = 900675,
        lbl_Override_Pack_1st_Pack = 900676,
        lbl_Override_Pack_Nth_Pack = 900677,
        lbl_Override_All_Generic_Packs = 900678,
        lbl_Override_Pack_Rounding_Info = 900693,
        // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        lbl_Override_Ship_Up_To = 900679,  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)


        // BEGIN TT#619- AGallagher - OTS Forecast - Chain Plan not required (#46)
        lbl_TrendOptions = 900680,
        lbl_TrendOptionsChainPlans = 900681,
        lbl_TrendOptionsChainWOS = 900682,
        lbl_TrendOptionsPlugChainWOS = 900683,
        // END TT#619- AGallagher - OTS Forecast - Chain Plan not required (#46) 

        // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) 
        lbl_MatrixMode = 900684,
        lbl_MatrixModeNormal = 900685,
        lbl_MatrixModeAverage = 900686,
        lbl_MatrixSpreadOption = 900687,
        lbl_MatrixSpreadOptionIndex = 900688,
        lbl_MatrixSpreadOptionSmooth = 900689,
        // END TT#637 - AGallagher - Velocity - Spread Average (#7) 

        //BEGIN TT#667 - Stodd - Pre-allocate Reserve
        lbl_ReserveAsBulk = 900690, // TT#667 - Stodd - Pre-allocate Reserve
        lbl_ReserveAsPacks = 900691,    // TT#667 - Stodd - Pre-allocate Reserve
                                        // END TT#667 - Stodd - Pre-allocate Reserve
                                        //Begin TT#707 - JScott - Size Curve process needs to multi-thread
        lbl_ConcurrentSizeCurveProcesses = 900692,
        //End TT#707 - JScott - Size Curve process needs to multi-thread
        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        lbl_AllocMinMax = 900694,
        lbl_InventoryMinMax = 900695,
        lbl_InventoryBasis = 900696,
        lbl_MinMaxOptions = 900697,
        lbl_VelocityGradesInactive = 804311,
        lbl_ItemMaxOverride = 804312,
        // END TT#1287 - AGallagher - Inventory Min/Max
        // begin TT#2155 - JEllis - Fill Size Holes Null Reference
        lbl_ApplyRulesOnly = 900698,
        // end TT#2155 - JEllis - Fill Size Holes Null Reference
        lbl_RIExpandInd = 900699,   // TT#628-MD - JSmith -  Add Option To Not Expand Allocation Records Once Relieved

        // BEGIN TT#246-MD - AGallagher - VSW Size
        lbl_VSWSizeConstraints = 900835,
        lbl_VSWOverrideDefault = 900836,
        // END TT#246-MD - AGallagher - VSW Size

        // BEGIN TT#933-MD - AGallagher - Item Max vs. FWOS Max
        lbl_ItemFWOSMax = 900884,
        lbl_ItemFWOSMaxDefault = 900885,
        lbl_ItemFWOSMaxHighest = 900886,
        lbl_ItemFWOSMaxLowest = 900887,
        // END TT#933-MD - AGallagher - Item Max vs. FWOS Max

        // BEGIN TT#933-MD - AGallagher - Add Header Min/Max Options and Inventory Basis
        lbl_GrpMinMaxOptions = 900889,
        lbl_HdrMinMaxOptions = 900890,
        // END TT#933-MD - AGallagher - Add Header Min/Max Options and Inventory Basis
        lbl_Grid = 900891,    //  TT#1126-MD - RMatelic - Show Details needs consistent “controls” as the main Workspace Explorer

        //Begin TT#370 - APicchetti - Build Packs
        tab_BuildPacks = 900700,
        tab_Criteria = 900701,
        tab_Constraints = 900702,
        tab_Summary = 900703,
        tab_Evaluation = 900704,
        grp_Criteria = 900705,
        lbl_BuildPackHeader = 900706,
        btn_GetHeader = 900707,
        lbl_VendorPackOrderMin = 900708, // TT#787 Vendor Min Order applies only to packs
        lbl_BuildPackVendor = 900709,
        lbl_BuildPackSizeMultiple = 900710,
        gb_SizeGroup = 900711,
        gb_SizeCurve = 900712,
        lbl_CandidatePacks = 900713,
        lbl_PercentToReserve = 900714,
        lbl_PercentToReserveBulk = 900715,
        lbl_PercentToReservePacks = 900716,
        grp_PackErrorOptions = 900717,
        lbl_AvgPackDeviationTolerance = 900718,
        lbl_MaxPackAllocationNeedTolerance = 900719,
        lbl_EvaluationPackOptions = 900720,

        //property names
        pnFromPackPatternComboName = 900721,
        pnShipVariance = 900722,
        pnMaxPackNeedTolerance = 900723,
        pnAvgPackDevTolerance = 900724,
        pnAllStoreTotalBuy = 900725,
        pnAllStoreBulkBuy = 900726,
        pnNonReserveTotalBuy = 900727,
        pnNonReserveBulkBuy = 900728,
        pnReserveTotalBuy = 900729,
        pnReserveBulkBuy = 900730,
        pnAllStoreTotalPackUnits = 900731,
        pnNonReserveTotalPackUnits = 900732,
        pnReserveTotalPackUnits = 900733,
        pnAllStoreTotalNumberOfPacks = 900734,
        pnNonReserveTotalNumberOfPacks = 900735,
        pnReserveTotalNumberOfPacks = 900736,
        pnCountSizesWithUnits = 900737,
        pnCountSizesWithAtLeast1Error = 900738,
        pnTotalSizeUnitError = 900739,
        pnAvgErrorPerSizeWithUnits = 900740,
        pnAvgErrorPerSizeInError = 900741,
        pnAvgErrorPerPack = 900742,
        pnAvgErrorPerPackWithError = 900743,
        pnAvgErrorPerStoreWithError = 900744,
        pnAvgErrorPerStoreWithPacks = 900745,
        pnAvgErrorPerStoreWithUnits = 900746,
        pnPercentAllStoreUnitsInPacks = 900747,
        pnPercentNonReserveUnitsInPacks = 900748,
        pnPercentReserveUnitsInPacks = 900749,
        pnPercentReserveInTotal = 900750,
        pnPercentBulkToTotal = 900751,
        pnPercentBulkInReserve = 900752,
        pnPercentBulkTotalInReserve = 900753,
        pnPercentReserveInBulk = 900754,
        pnCountOfAllStoresWithPacks = 900755,
        pnCountOfNonReserveStoresWithPacks = 900756,
        pnCountOfAllStoresWithUnits = 900757,
        pnCountOfNonReserveStoresWithUnits = 900758,
        pnCountOfAllStoresWithBulk = 900759,
        pnCountOfNonReserveStoresWithBulk = 900760,
        pnStoresWithErrorCount = 900761,
        pnPercentNonReserveWithUnitsInError = 900762,
        pnKey = 900763,

        //End TT#370 - APicchetti - Build Packs

        // BEGIN TT#669 - AGallagher – Build Pack Method – Variance Options
        lbl_VarianceOptions = 900764,
        lbl_DepleteReserve = 900765,
        lbl_IncreaseBuyQty = 900766,
        lbl_PercentSign = 900767,
        // END TT#669 - AGallagher – Build Pack Method – Variance Options
        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
        lbl_Active_Only = 900783,
        lbl_Active = 900784,
        lbl_Inactive = 900785,
        // End TT#988
        // Begin TT#698 - JSmith - Enhance environment information
        lbl_EnvUser = 900768,
        lbl_EnvVersion = 900769,
        lbl_EnvEnvironment = 900770,
        lbl_EnvUpdateDate = 900771,
        lbl_EnvOperatingSystem = 900772,
        lbl_EnvEdition = 900773,
        lbl_EnvServicePack = 900774,
        lbl_EnvFramework = 900775,
        lbl_EnvUnavailable = 900776,
        // End TT#698
        lbl_BP_RemoveBulkFromHeader = 900777,   // TT#744 - JEllis - Use Orig Pack Fit Logic; Option No Bulk

        // begin TT#801 Build Packs Add Stats for Best Pack Solution Select
        pnOriginalBuyPackUnits = 900778,
        pnPercentOriginalBuyPackaged = 900779,
        // end TT#801 Build Packs Add Stats for Best Pack Solution Select


        // Begin TT#2 - RMatelic - assortment
        lbl_EmptyRow = 900780,
        lbl_ColorBrowser = 900781,
        // End TT#2
        // Begin TT#2 - stodd - assortment
        lbl_Database = 900782,
        // End TT#2 - stodd - assortment

        // Begin TT#2 - RMatelic - assortment
        lbl_PhStyleID_2 = 900786,
        // End TT#2 
        //Begin TT#1076 - JScott - Size Curves by Set
        lbl_SizeCurvesByStore = 900787,
        lbl_SizeCurvesBySet = 900788,
        //End TT#1076 - JScott - Size Curves by Set

        // BEGIN TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
        lbl_Checkbox_VelocityReconcile = 900789,
        // END TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
        // BEGIN TT#1135 - AGallagher - AExport headers from allocation workspace
        lbl_Button_Excel = 900790,
        lbl_MB_Excel_Caption = 900791,
        lbl_MB_Excel_Text = 900792,
        lbl_MB_Excel_But1 = 900793,
        lbl_MB_Excel_But2 = 900794,
        lbl_MB_Excel_But3 = 900795,
        lbl_MB_Excel_No_Sel = 900796,
        lbl_MB_Excel_All = 900797,
        lbl_MB_Excel_HS = 900798,
        lbl_MB_Excel_HSD = 900799,
        lbl_MB_Excel_Exp_Lit = 900800,
        // END TT#1135 - AGallagher - AExport headers from allocation workspace
        lbl_Content = 900801,
        lbl_Button_Refresh = 900802,
        // Begin TT#1227 - stodd - sort seq
        lbl_PlaceholderSeq = 900803,
        lbl_HeaderSeq = 900804,
        // End TT#1227 - stodd - sort seq
        // BEGIN TT#1401 - stodd - add resevation stores (IMO)
        lbl_IMO_ID = 900805,
        lbl_IMO_MIN_SHIP_QTY = 900806,
        lbl_IMO_PCT_PK_THRSHLD = 900807,
        lbl_IMO_MAX_VALUE = 900808,
        lbl_PSH_BK_STK_CDR_RID = 900809,
        // END TT#1401 - stodd - add resevation stores (IMO)
        // Begin TT#1401 - RMatelic - Reservation Stores
        lbl_ItemUnitsAllocated = 900810,
        lbl_ItemOrigUnitsAllocated = 900811,
        lbl_IMO_Tab = 900812,
        lbl_VSWGridDisabled = 804314,
        lbl_ApplyVSWCheckBox_Popup = 804315,
        // End TT#1401 

        // Begin TT#2015 - gtaylor - apply changes to lower levels
        lbl_ACLL_ActionCompletedSuccessfully = 804316,
        lbl_ACLL_NoChangesHaveBeenMade = 804317,
        lbl_ACLL_LockAttemptFailed = 804318,
        lbl_ACLL_NoLowLevelProcessingRequired = 804319,
        lbl_ACLL_DefaultAction = 804320,
        // End TT#2015 - gtaylor - apply changes to lower levels
        // Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
        lbl_ApplyMinToZeroTolerance = 900813,
        // End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
        lbl_ChainSetPercent = 900814,  // TT#2138 - JSmith - Open Node Properties Read Only

        //BEGIN TT#3 -MD- DOConnell - Export does not produce output
        lbl_MB_Excel_Assortment_Caption = 900815,
        //END TT#3 -MD- DOConnell - Export does not produce output
        lbl_AdjustVSW_OnHand = 900816, // TT#2225 - JEllis - AnF VSW Fwos Enhancement
        lbl_IMO_FWOS_MAX = 900817, // TT#2225 - gtaylor - ANF VSW
        lbl_IMO_FWOS_MAX_REQUIRED = 900818, // TT#2225 - gtaylor - ANF VSW
        // Begin TT#46 MD - JSmith - User Dashboard
        lbl_Button_Clear = 900821,
        lbl_Time = 900822,
        lbl_Module = 900823,
        lbl_Message_Level = 900824,
        lbl_Message = 900825,
        lbl_Message_Details = 900826,
        lbl_Message_Breaddown = 900827,
        lbl_Show_Chart = 900828,
        // End TT#46 MD

        //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement 
        lbl_Project_Curr_WK_Sls = 900819,
        //END TT#43 - MD - DOConnell - Projected Sales Enhancement 

        //BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement 
        lbl_Stock_Lead_Weeks = 900820,
        //BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement 

        //BEGIN TT#2576 - DOConnell - Global Lock Create - Title Bar Name Changes
        lbl_glbLock = 900832,
        //END TT#2576 - DOConnell - Global Lock Create - Title Bar Name Changes
        // Begin TT#292-MD - JSmith - Enhanced Help - About
        lbl_WebPage = 900833,
        lbl_SupportWebPage = 900834,
        // End TT#292-MD - JSmith - Enhanced Help - About
        //BEGIN TT#46 - -jsobek –Develop My Activity Log
        msg_MyActivity_ClearWarningPrompt = 900837,
        msg_MyActivity_ClearWarningPromptTitle = 900838,
        msg_MyActivity_MessageLimitHigh = 900839,
        msg_MyActivity_MessageLimitInvalid = 900840,
        msg_MyActivity_MessageLimitInvalidTitle = 900841,
        msg_MyActivity_MessageLimitLow = 900842,
        msg_MyActivity_MessageLimitRestoringOriginal = 900843,
        //END TT#46 -jsobek –Develop My Activity Log
        //BEGIN TT#110-MD-VStuart - In Use Tool
        lbl_In_Use = 200087,
        //END TT#110-MD-VStuart - In Use Tool
        lbl_File_Processing_Direction = 200088,  // TT#645-MD - JSmith - Add File Processing Direction Parameter to Tasks on Task Lists that Process Input Files

        /*BEGIN TT#506 - -jsobek –Add infrastructure to allow Email to be sent from the application */
        msg_Email_From_Address_Required = 900844,
        msg_Email_From_Address_Invalid = 900845,
        msg_Email_To_Address_Required = 900846,
        msg_Email_To_Address_Invalid = 900847,
        msg_Email_CC_Address_Invalid = 900848,
        msg_Email_BCC_Address_Invalid = 900849,
        msg_Email_Subject_Required = 900850,
        msg_Email_Body_Required = 900851,
        msg_Email_Attachments_Invalid = 900852,
        msg_Email_SMTP_SettingsNotSetup = 900853,
        msg_Email_SMTP_ConnectionIssue = 900854,
        msg_Email_SMTP_SendingEmailIssue = 900855,
        msg_Email_Test_Email_Successful = 900856,
        /*END TT#506 -jsobek –Add infrastructure to allow Email to be sent from the application */
        lbl_PlanTypeChainLadder = 900857,  /*TT#609-MD -jsobek –OTS Forecast Chain Ladder View */
        lbl_TotalsRight = 900858,  /*TT#639-MD -agallagher – OTS Forecast Totals Right??? */
        lbl_StoreDeleteInd = 900859,
        // BEGIN TT#708-MD - Stodd - Group Allocation Prototype.
        // lbl_Group_Min = 900882,      // TT#937 - MD - stodd - GA method missing 'group min' column'
        lbl_Group_Max = 900860,
        lbl_Header_Min = 900861,
        lbl_Header_Max = 900862,
        lbl_Line_Item_Min_Override = 900863,
        lbl_GroupAllocation = 900864,
        lbl_Group = 900865,
        // END TT#708-MD - Stodd - Group Allocation Prototype.
        lbl_Asrt_Excel_But1 = 900866,
        lbl_Asrt_Excel_But2 = 900867,
        lbl_StoreDeleteSettings = 900869,
        lbl_NumConcurrentForCopy = 900870,
        lbl_NumConcurrentForDelete = 900871,
        // END TT#708-MD - Stodd - Group Allocation Prototype.
        lbl_BatchSize = 900872,
        lbl_MinimumRowCount = 900873,
        lbl_MaximumRowCount = 900874,
        lbl_RowPctMaximum = 900875,
        lbl_Analysis = 900876,
        lbl_ProcessAnalysis = 900877,
        lbl_Messages = 900878,
        lbl_ProcessStoreDelete = 900879,
        lbl_ResetForRestart = 900880,
        lbl_StoreDelete = 900881,

        //Begin TT#855-MD -jsobek -Velocity Enhancements
        lbl_MatrixModeHeaderBalance = 900882,
        //End TT#855-MD -jsobek -Velocity Enhancements
        lbl_GroupAllocationID = 900883,		// TT#893 - MD - stodd - Add "Group Allocation ID" Column to Allocation Workspace
        lbl_Group_Min = 900888,     // TT#937 - MD - stodd - GA method missing 'group min' column'
        // Begin TT#3859 - stodd - Balance message text
        lbl_Header_Total = 900896,
        lbl_Placeholder_Total = 900897,
        // End TT#3859 - stodd - Balance message text
        lbl_Unspecified = 900898,   // TT#1378-MD - RMatelic- Add soft text label for Unspecified Size Group
        lbl_ShowSignOffPrompt = 900899,   // TT#4243 - RMatelic - Add a prompt when the user closes the MID app using the "x" in the top right corner  

        //Begin TT#901-MD -jsobek -Batch Only Mode
        msg_RemoteSystemOptions_ShutdownDisplayMessagePrefix = 900900,
        msg_RemoteSystemOptions_MessageForClientTitle = 900901,
        msg_RemoteSystemOptions_BatchOnlyModeOnLastChangedByPrefix = 900902,
        msg_RemoteSystemOptions_BatchOnlyModeOffLastChangedByPrefix = 900903,
        msg_RemoteSystemOptions_NotAvailableRunningLocal = 900904,
        msg_RemoteSystemOptions_NotAvailableNotConnected = 900905,
        msg_RemoteSystemOptions_LogOutCommandSent = 900906,
        msg_RemoteSystemOptions_BatchOnlyModeTurnedOn = 900907,
        msg_RemoteSystemOptions_BatchOnlyModeTurnedOff = 900908,
        msg_RemoteSystemOptions_BatchOnlyModeAlreadyOnPrefix = 900909,
        msg_RemoteSystemOptions_BatchOnlyModeAlreadyOffPrefix = 900910,
        msg_RemoteSystemOptions_BatchOnlyModeWillTurnOn = 900911,
        msg_RemoteSystemOptions_BatchOnlyModeWillTurnOff = 900912,
        msg_RemoteSystemOptions_MessageSent = 900913,
        msg_RemoteSystemOptions_UsersLoggedInSuffix = 900914,
        lbl_RemoteSystemOptions_ShowCurrentUserGrid_UserColumn = 900915,
        lbl_RemoteSystemOptions_ShowCurrentUserGrid_MachineColumn = 900916,
        lbl_RemoteSystemOptions_ShowCurrentUserGrid_IPAddressColumn = 900917,
        lbl_RemoteSystemOptions_gbLoginOptions = 900918,
        lbl_RemoteSystemOptions_ckbUseWindowsLogin = 900919,
        lbl_RemoteSystemOptions_cbxForceSingleClientInstance = 900920,
        lbl_RemoteSystemOptions_cbxForceSingleUserInstance = 900921,
        lbl_RemoteSystemOptions_cbxEnableRemoteSystemOptions = 900922,
        lbl_RemoteSystemOptions_cbxControlServiceDefaultBatchOnlyModeOn = 900923,
        lbl_RemoteSystemOptions_btnBatchModeTurnOn = 900924,
        lbl_RemoteSystemOptions_btnBatchModeTurnOff = 900925,
        lbl_RemoteSystemOptions_btnSendMsg = 900926,
        lbl_RemoteSystemOptions_lblMessageForClients = 900927,
        lbl_RemoteSystemOptions_lblLastChangedBy = 900928,
        lbl_RemoteSystemOptions_gbCurrentUsers = 900929,
        lbl_RemoteSystemOptions_btnShowCurrentUsers = 900930,
        lbl_RemoteSystemOptions_btnLogOutSelected = 900931,
        msg_RemoteSystemOptions_SelectOneOrMoreUsers = 900932,
        msg_RemoteSystemOptions_ProvideMessageToSend = 900933,
        //End TT#901-MD -jsobek -Batch Only Mode
        //Begin TT#1349-MD -jsobek -Store Filters - Merchandise Selector - add new "No Size" option to prevent selection sizes
        msg_SelectSingleHierarchyNode_InvalidMerchandiseNode = 900934,
        msg_SelectSingleHierarchyNode_OnlyStyle = 900935,
        msg_SelectSingleHierarchyNode_OnlyColor = 900936,
        msg_SelectSingleHierarchyNode_OnlySize = 900937,
        msg_SelectSingleHierarchyNode_NoSize = 900938,
        //End TT#1349-MD -jsobek -Store Filters - Merchandise Selector - add new "No Size" option to prevent selection sizes
        lbl_CompanyName = 900939,  // TT#4287 - JSmith - Do not allow refresh if windows are open
        // Begin TT#1581-MD - stodd - header reconcile
        lbl_TransactionFilesRead = 900940,
        lbl_RecordsRead = 900941,
        lbl_DuplicateRecordsFound = 900942,
        lbl_RecordsSkipped = 900943,
        lbl_RecordsWritten = 900944,
        lbl_TransactionFilesWritten = 900945,
        lbl_RemoveRecordsWritten = 900946,
        lbl_RemoveFilesWritten = 900947,
        // End TT#1581-MD - stodd - header reconcile
        lbl_BatchCompAll = 900948,	// TT#1595-MD - stodd - Batch comp
        lbl_PriorHeaderIncludeReserve = 900951,	// TT#1609-MD - stodd - prior header
        lbl_IncludeReserve = 900952,	// TT#1609-MD - stodd - prior header


        lbl_DCCartonRoundDfltAttribute = 900953,   // TT#1652-MD - RMatelic - DC Carton Rounding
        lbl_UnitsPerCarton = 900954,               // TT#1652-MD - RMatelic - DC Carton Rounding
        lbl_DCCartonRounding = 900955,             // TT#1652-MD - RMatelic - DC Carton Rounding
        lbl_ApplyOverageTo = 900956,               // TT#1652-MD - RMatelic - DC Carton Rounding
        lbl_AllocatedStores = 900957,              // TT#1652-MD - RMatelic - DC Carton Rounding
        lbl_DCCartonRoundingTabText = 900958,      // TT#1652-MD - RMatelic - DC Carton Rounding
        lbl_RefreshToSeeChanges = 900959,          // TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields   */
        lbl_CreateMasterHeadersTabText = 900960,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentTabText = 900961,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_CreateMasterHeadersUseSelectedText = 900962,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_CreateMasterHeadersMerchandiseGrid = 900963,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_CreateMasterHeadersMerchandise = 900964,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_CreateMasterHeadersFilter = 900965,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_CreateMasterHeadersAddMerchandise = 900966,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_CreateMasterHeadersOverrideGrid = 900967,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_CreateMasterHeadersOverrideHeaderChar = 900968,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_CreateMasterHeadersOverrideValue = 900969,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_CreateMasterHeadersAddOverride = 900970,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentMasterSplitOptions = 900971,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillment = 900972,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentProportional = 900973,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentApplyMinimums = 900974,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentPrioritizeHeadersBy = 900975,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentHeadersAscending = 900976,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentHeadersDescending = 900977,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentOrderStoresByGrid = 900978,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentDC = 900979,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentStoreCharacteristic = 900980,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentStoresAscending = 900981,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentStoresDescending = 900982,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentSplitBy = 900983,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentSplitByStore = 900984,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentSplitByDC = 900985,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentReserve = 900986,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentReservePreSplit = 900987,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentReservePostSplit = 900988,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentMinimumsApply = 900989,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentMinimumsApplyByQuantity = 900990,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentMinimumsApplyFirst = 900991,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentOptions = 900992,   // TT#1966-MD - JSmith- DC Fulfillment
        lbl_DCFulfillmentWithinDC = 900993,   // TT#1966-MD - AGallagher- DC Fulfillment
        lbl_DCFulfillmentWithinDCFill = 900994,   // TT#1966-MD - AGallagher- DC Fulfillment
        lbl_DCFulfillmentWithinDCProportional = 900995,   // TT#1966-MD - AGallagher- DC Fulfillment
        lbl_DCFulfillmentMonitor = 900996,   // TT#1966-MD - AGallagher- DC Fulfillment
        lbl_ProcessControl = 900997,   // TT#1644-MD - JSmith - Process Control
        lbl_ProcessID = 900998,   // TT#1644-MD - JSmith - Process Control
        lbl_IsRunning = 900999,   // TT#1644-MD - JSmith - Process Control
        lbl_LastModifiedOn = 901000,   // TT#1644-MD - JSmith - Process Control
        lbl_SaveChanges = 901001,   // TT#1644-MD - JSmith - Process Control
        lbl_MustBeRunning = 901002,   // TT#1644-MD - JSmith - Process Control
        lbl_CannotBeRunning = 901003,   // TT#1644-MD - JSmith - Process Control
        // Begin TT#2131-MD - JSmith - Halo Integration			
        lbl_PlanningExtractMethod_LowLevels = 901006,
        lbl_PlanningExtractMethod_LowLevelsOnly = 901007,
        lbl_PlanningExtractMethod_ChainData = 901008,
        lbl_PlanningExtractMethod_StoreData = 901009,
        lbl_PlanningExtractMethod_AttributeSetData = 901010,
        lbl_PlanningExtractMethod_Attribute = 901011,
        lbl_PlanningExtractMethod_Variables = 901012,
        lbl_PlanningExtractMethod_TotalVariables = 901013,
        lbl_PlanningExtractMethod_ConcurrentProcesses = 901014,
        lbl_PlanningExtractMethod_ExcludeZeroValues = 901015,
        lbl_PlanningExtractMethod_ShowIneligible = 901016,
        lbl_Analytics = 901017,
        // End TT#2131-MD - JSmith - Halo Integration,
        lbl_ServicesRestartRequired = 901004,   // TT#1644-MD - JSmith - Process Control
        lbl_LastModifiedBy = 901005,   // TT#1644-MD - JSmith - Process Control
        lbl_WorklistItem = 901018, // RO-3444 - JSmith - RO Web Messaging
        lbl_RemoteSystemOptions_ShowCurrentUserGrid_ClientType = 901019,
        lbl_RemoteSystemOptions_ShowCurrentUserGrid_ClientType_Windows = 901020,
        lbl_RemoteSystemOptions_ShowCurrentUserGrid_ClientType_Web = 901021,

        // ToolTips 1000000-1099999
        tt_Button_AddBasis = 1000000,
        tt_Button_AddBasisDetails = 1000001,
        //Begin Track #4547 - JSmith - Add similar stores by forecast versions
        //		tt_DoubleClickToChangeColor		= 1000002
        tt_DoubleClickToChangeColor = 1000002,
        tt_ClickToShowHideColumns = 1000003,
        tt_ClickForHeaderNotes = 1000004,
        tt_ClickForColorBrowser = 1000005,
        tt_ClickToFilterDropDown = 1000006,
        tt_CheckToForecast = 1000007,
        tt_UncheckToNotForecast = 1000008,
        tt_CheckToForecastLowLevels = 1000009,
        tt_UncheckToNotForecastLowLevels = 1000010,
        tt_AddNewStockMinMax = 1000011,
        tt_SizeFilterMaskValues = 1000012,
        //Begin Track #4547 - JSmith - Add similar stores by forecast versions
        tt_FV_Description = 1000013,
        tt_FV_Active = 1000014,
        tt_FV_ProtectHistory = 1000015,
        tt_FV_SimilarStores = 1000016,
        tt_FV_BlendType = 1000017,
        tt_FV_ActualVersion = 1000018,
        tt_FV_ForecastVersion = 1000019,
        // BEGIN MID Track #4827 - John Smith - Presentation plus sales
        //Begin Enhancement - JScott - Export Method - Part 8
        //		tt_FV_PeriodHistory				= 1000020
        tt_FV_PeriodHistory = 1000020,
        //End Track #4547
        tt_ExportMethod_PreinitValues = 1000021,
        tt_PMPlusSales_On = 1000022,
        tt_PMPlusSales_Off = 1000023,
        tt_PMPlusSales_Inherit = 1000024,
        // END MID Track #4827
        //End Enhancement - JScott - Export Method - Part 8
        //End Track #4547
        //Begin Track #5395 - JScott - Add ability to discard zero values in Export
        //tt_Button_AddNewRollup          = 1000025
        tt_Button_AddNewRollup = 1000025,
        tt_ExportMethod_ExcludeZeroValues = 1000026,
        //End Track #5395 - JScott - Add ability to discard zero values in Export
    }



    //Begin Track #5858 - JSmith - Validating store security only
    public enum eMIDControlCode
    {
        Unassigned = 000000,
        form_HierarchyExplorer = 100000,
        form_StoreExplorer = 100001,
        form_WorkspaceExplorer = 100002,
        form_WorkflowMethodExplorer = 100003,
        form_FilterExplorer = 100004,
        form_HierarchyProperties = 100010,
        form_NodeProperties = 100011,
        form_EligibilityModel = 100012,
        form_SalesModifierModel = 100013,
        form_StockModifierModel = 100014,
        form_GeneralAllocationMethod = 100015,
        form_RuleMethod = 100016,
        form_OTSPlanMethod = 100017,
        form_FillSizeHolesMethod = 100018,
        form_OverrideMethod = 100019,
        form_VelocityMethod = 100020,
        form_AllocationWorkflow = 100021,
        form_ForecastWorkflow = 100022,
        form_SimilarStoresSelect = 100024,
        form_GlobalOptions = 100025,
        form_CalendarModelList = 100026,
        form_CalendarModelMaint = 100027,
        form_CalendarDisplay = 100028,
        form_Calendar53Week = 100029,
        form_Administration = 100030,
        form_AuditViewer = 100031,
        form_OTSPlanSelection = 100032,
        form_OTSPlanReview = 100033,
        form_StyleReview = 100034,
        form_SummaryReview = 100035,
        form_SizeReview = 100036,
        form_StoreCharacteristicMaint = 100037,
        form_StoreProfileMaint = 100038,
        form_StoreAttributes = 100039,
        form_HeaderCharacteristicMaint = 100040,
        form_TextEditor = 100041,
        form_BasisSizeMethod = 100042,
        form_SizeOverrideMethod = 100043,
        form_SizeWarehouseMethod = 100044,
        form_SizeNeedMethod = 100045,
        form_WorkspaceExplorerFilter = 100046,
        form_SecurityInheritance = 100047,
        form_SizeConstraintsModel = 100048,
        form_SizeAlternativeModel = 100050,
        form_OverrideLowLevelVersions = 100051,
        form_OTSForecastBalanceMethod = 100052,
        form_SizeCodeMaint = 100053,
        form_OTSForecastBalance = 100054,
        form_About = 100055,
        form_Forecast_Model = 100056,
        form_Forecast_Balance_Model = 100057,
        form_CopyChainForecast = 100058,
        form_CopyStoreForecast = 100059,
        form_ForecastChainSpread = 100060,
        form_GeneralAssortmentMethod = 100061,
        form_AssortmentReview = 100062,
        form_AllocationViewSelection = 100063,
        form_ColorBrowser = 100064,
        form_ProductCharacteristicMaint = 100065,
        form_Properties = 100066,
        form_Export = 100067,
        form_ForecastModifySales = 100068,
        form_FWOSModifierModel = 100069,
        form_UserOptions = 100070,
        form_ExcludeLowLevels = 100071,
        form_ExportMethod = 100072,
        form_SizeCurves = 100073,
        form_SizeGroups = 100074,
        form_AssignUser = 100075,
        form_AuditFilter = 100076,
        form_Rollup = 100077,
        // Begin TT#155 - JSmith - Size Curve Method
        form_SizeCurveMethod = 100078,
        // End TT#155
        form_AssortmentProperties = 100079,
        form_AssortmentViewSelection = 100080,
        form_FWOSMaxModel = 100081,
        form_PlanningExtractMethod = 100082,  // TT#2131-MD - JSmith - Halo Integration

        field_Merchandise = 200000,
        field_Version = 200001,
        field_DateRange = 200002,
        field_Filter = 200003,
        field_Attribute = 200004,
        field_AttributeSet = 200005,
    }
    //End Track #5858

    // begin MID Track 3810 Size Allocation GT Style Allocation
    //	// The eWorkUpBuyType is used to identify the type of allocation header.
    //	public enum eWorkUpBuyType
    //	{
    //		NotWorkUpBuy,
    //		WorkUpBulkColorBuy,
    //		WorkUpBulkSizeBuy,
    //		WorkUpBulkColorSizeBuy,
    //		WorkUpPackColorBuy,
    //		WorkUpPackSizeBuy,
    //		WorkUpPackColorSizeBuy
    //	}
    /// <summary>
    /// Identifies buy allocation type
    /// </summary>
    public enum eWorkUpBuyAllocationType
    {
        NotWorkUpAllocationBuy = 0,
        WorkUpTotalAllocationBuy = 1,
        //WorkUpBulkColorAllocationBuy  = 2,
        WorkUpBulkSizeAllocationBuy = 3
    }
    // end MID Track 3810 Size Allocation GT Style Allocation

    /// <summary>
    /// eAllocationStatusFlag tracks the progress of the allocation process. 
    /// </summary>
    /// <remarks>
    /// These flags track the progress of the allocation:
    /// <list type="bullet">
    /// <item>ReceivedInBalance:  True indicates Allocation Header is ready to be allocated</item>
    /// <item>BottomUpSizePerformed:  True indicates a bottom-up size allocation process has occurred</item>
    /// <item>RulesDefinedAndProcessed:  True indicates Rules have been defined and processed</item>
    /// <item>NeedAllocationPerformed:  True indicates Style Need Allocation has occurred</item>
    /// <item>PackBreakoutByContent:  True indicates packs were broken out based on pack content</item>
    /// <item>BulkSizeBreakoutPerformed:  True indicates a top-down size allocation process has occurred</item>
    /// <item>ReleaseApproved:  True indicates the Allocation Header and its store allocation ready to be released</item>
    /// <item>Released:  True indicates the Allocation Header and its store allocation ready to be picked and shipped to stores</item>
    /// <item>UnitsAllocated:  True indicates units have been allocated; False indicates no units have been allocated</item>
    /// <item>GradeDefinitionOverride:  True indicates the Grade Definitions were overridden (are not the default); False indicates the Grade definitions are the default definitions</item>
    /// <item>AllocationFromMultiHeader:  True indicates this header's allocation was split from the allocation of a multi header; false indicates this header's allocation was not developed on a multi-header.</item>
    /// <item>ReapplyTotalAllocationPerformed: True indicates Reapply Total Allocation Has been performed for this header.</item>
    /// <item>IdentifyIncludeStores: True indicates to identify stores that may be included in the allocation.</item>
    /// <item>ImoCriteriaOverridden: True indicates VSW/IMO criteria was overridden by Override Method</item>
    /// <item>LoadImoCriteria:  True indicates IMO Criteria must be loaded</item>
    /// <item>CalcVswColorIdealItemMin: True indicates to calculate the color ideal item minimum</item>
    /// <item>BalanceToVSW_Performed:  True indicates the Balance to VSW Action was successfully performed</item>
    /// <uten>StoreItemQtyIsLocked:  True indicates all store item quantities have been LOCKED globally by a Balance to VSW Action; set to false whenever store allocations change after the Balance to VSW occurs</uten>
    /// </list> 
    /// </remarks>
    //  Warning
    //  Warning  The values associated with this flag correspond to bit positions in a ushort field.
    //  Warning
    public enum eAllocationStatusFlag
    {
        ReceivedInBalance = 0,
        BottomUpSizePerformed = 1,
        RulesDefinedAndProcessed = 2,
        NeedAllocationPerformed = 3,
        PackBreakoutByContent = 4,
        BulkSizeBreakoutPerformed = 5,
        ReleaseApproved = 6,
        Released = 7,
        UnitsAllocated = 8,
        GradeDefinitionOverride = 9,   // MID Track 4448 AnF Audit Enhancement
        AllocationFromMultiHeader = 10, // MID Track 4448 AnF Audit Enhancement
        ReapplyTotalAllocationPerformed = 11, // TT#785 - Reapply total allocation // TT#1401 - JEllis - Urban Reservation Stores pt 5
        Obsolet = 12,                                                               // TT#1401 - JEllis - Urban Reservation Stores pt 5     // TT#327 - Header Allocated befor VSW Gets VSW Allocation
        IdentifyIncludedStores = 13,                                                // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 17
        ImoCriteriaOverridden = 14,                                                    // TT#2083 - Jellis - Urban Virtual Store Warehouse Min Ship Qty // TT#327 - Header Allocated before VSW Gets VSW Allocation
        LoadImoCriteria = 15,
        NotUsed = 16,                                                         // TT#327 - Header Allocated befor VSW Gets VSW Allocation // TT#246 - MD - JEllis - AnF VSW In Store Minimums pt 5
        CalcVswColorIdealItemMin = 17,                                                        // TT#246 - MD - JEllis - AnF VSW In STore Minimums pt 5  // TT#1334 - Urban - Jellis - Balance to VSW Enhancement
        BalanceToVSW_Performed = 18
    }

    // begin TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5
    /// <summary>
    /// eColorFlag tracks the progress of the color allocation process. 
    /// </summary>
    /// <remarks>
    /// These flags track the progress of the color allocation:
    /// <list type="bullet">
    /// <item>CalcVswSizeConstraints:  True indicates VSW Size Constraints must be calculated</item>
    /// </list> 
    /// </remarks>
    //  Warning
    //  Warning  The values associated with this flag correspond to bit positions in a uint field.
    //  Warning
    public enum eColorStatusFlag
    {
        CalcVswSizeConstraints = 0
    }
    // end TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5

    /// <summary>
    /// eShippingStatusFlag tracks the shipping status of an allocation. 
    /// </summary>
    /// <remarks>
    /// These flags track the shipping status:
    /// <list type="bullet">
    /// <item>ShippingStarted:  True indicates at least 1 unit has been shipped to a store and charged to its onhand</item>
    /// <item>ShippingComplete: True indicates all units have been shipped to stores and charged to each store's onhand</item>
    /// <item>ShippingOnHold: True indicates no changes can occur.</item>
    /// </list> 
    /// </remarks>
    //  Warning
    //  Warning  The values associated with this flag correspond to bit positions in a byte field.
    //  Warning
    public enum eShippingStatusFlag
    {
        ShippingStarted = 0,
        ShippingComplete = 1,
        ShippingOnHold = 2
    }
    /// <summary>
    /// eBalanceStatusFlag tracks the allocation balance. 
    /// </summary>
    /// <remarks>
    /// These flags track the balance status of the allocation:
    /// <list type="bullet">
    /// <item>SizeReceiptsBalanceToColor: True indicates for all colors, the color receipts balance to the total of size receipts within that color</item>
    /// <item>ColorReceiptsBalanceToBulk: True indicates that bulk receipts balance to total of all color receipts</item>
    /// <item>PackSizesBalanceToPackColor: True indicates for all packs and for each color in that pack the size content of the pack balances to the color content</item>
    /// <item>PackColorsBalanceToPack: True indicates for all packs, the color content balances to the total pack content</item>
    /// <item>PackBulkReceiptsBalanceToTotal: True indicates sum of all pack receipts plus total bulk receipts balance to total receipts</item>
    /// <item>PackAllocationInBalance:  True indicates that packs to allocate = packs allocated for all packs</item>
    /// <item>BulkColorAllocationInBalance:  True indicates that units to allocate = units allocated for all bulk colors</item>
    /// <item>BulkPlusPackAllocationInBalance: True indicates for all stores bulk plus pack allocation equals store's total allocation</item>
    /// <item>BulkSizeAllocationInBalance:  True indicates units to allocate = units allocated for all width-sizes in every bulk color</item>
    /// <item>StoreSizeAllocationsInBalanceToColor: True indicates units allocated to store-color-size balance to units allocated to store-color</item>
    /// <item>BulkSizeAllocations_GT_SizeReceipts: True indicates Size Units allocated GT Size Receipts for some color-size; note BulkSizeAllocationInBalance overrides this flag when it is true</item>
    /// <item>BulkColorAllocations_GT_ColorReciepts: True indicates Color Units allocated GT Color Receipts for some color; note BulkColorAllocationInBalance overrides this flag when it is true</item>
    /// <item>AtLeastOneSizeAllocated: True indicates at least one size in at least one color has units allocated</item>
    /// </list>
    /// </remarks>
    //  Warning
    //  Warning  The values associated with this flag correspond to bit positions in a ushort field.
    //  Warning
    public enum eBalanceStatusFlag
    {
        SizeReceiptsBalanceToColor = 0,
        ColorReceiptsBalanceToBulk = 1,
        PackSizesBalanceToPackColor = 2,
        PackColorsBalanceToPack = 3,
        PackBulkReceiptsBalanceToTotal = 4,
        PackAllocationInBalance = 5,
        BulkColorAllocationInBalance = 6,
        BulkPlusPackAllocationInBalance = 7,
        BulkSizeAllocationInBalance = 8,
        StoreSizeAllocationsInBalanceToColor = 9, // MID Tracks 3738, 3811, 3827 Status Problems
        BulkSizeAllocations_GT_SizeReceipts = 10, // MID Tracks 3738, 3811, 3827 Status Problems
        BulkColorAllocations_GT_ColorReceipts = 11, // MID Tracks 3738, 3811, 3827 Status Problems
        AtLeastOneSizeAllocated = 12 // MID Tracks 3738, 3811, 3827 Status Problems
    }

    /// <summary>
    /// eAllocationStoreGeneralAuditFlag audits each store's total allocation resulting from an allocation process. 
    /// </summary>
    /// <remarks>
    /// These flags provide a general audit of each store's allocation:
    /// <list type="bullet">
    /// <item>PercentNeedLimitReached:  True indicates store reached the user specified Percent Need Limit during a need allocation process.</item>
    /// <item>CapacityMaximumReached:  True indicates store reached its capacity maximum</item>
    /// <item>PrimaryMaximumReached:  True indicates store reached its primary maximum allocation</item>
    /// <item>MayExceedMax:  True indicates that store maximum allocation may be exceeded if necessary</item>
    /// <item>MayExceedPrimary:  True indicates that the primary allocation is not an absolute maximum</item>
    /// <item>MaxExceedCapacity:  True indicates that the capacity of a store may be exceeded</item>
    /// <item>AllocationPriority: True indicates store has allocation priority (ie. shipping priority)</item>
    /// <item>Eligible: True indicates store is eligible</item>
    /// <item>IncludeStoreInAllocation: True indicates store is included in IMO header allocation</item>
    /// </list>
    /// </remarks>
    //  Warning
    //  Warning  The values associated with this flag correspond to bit positions in a ushort field.
    //  Warning
    public enum eAllocationStoreGeneralAuditFlag
    {
        PercentNeedLimitReached = 0,
        CapacityMaximumReached = 1,
        PrimaryMaximumReached = 2,
        AllocationPriority = 3,
        MayExceedMax = 4,
        MayExceedPrimary = 5,
        MayExceedCapacity = 6,
        Eligible = 7,          // TT#1401 - JEllis - Urban Reservation Store pt 11
        ExcludeStoreInAllocation = 8    // TT#1401 - JEllis - Urban Reservation Store pt 11  // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 13
    }

    /// <summary>
    /// eAllocationStoreDetailAuditFlag audits each store's detail component allocation resulting from an allocation process. 
    /// </summary>
    /// <remarks>
    /// These flags provide a detail audit of each store's allocation:
    /// <list type="bullet">
    /// <item>ManuallyAllocated:  True indicates user manually allocated some units.</item>             
    /// <item>AutoAllocated:  True indicates some units were allocated by an automatic allocation process</item>
    /// <item>ChosenRuleAcceptedByHeader:  True indicates chosen rule was accepted on the Header.</item>
    /// <item>Out:  True indicates store and allocation component were "outted" by special rules</item>
    /// <item>Need:  True indicates that units were allocated based on need.</item>
    /// <item>FilledSizeHole:  True indicates size holes were filled based on size need</item>
    /// <item>Locked: true indicates units allocated are locked from changes; false indicates units allocated may be changed.</item>
    /// <item>TempLock: true indicates units allocated are temporarily locked from changes; false indicates units allocated may be changed</item>
    /// <item>RuleAllocationFromParentComponent: true indicates rule allocation is partially or completely from parent component; false indicates parent component did not participate in allocation.</item>
    /// <item>RuleAllocationFromChildComponent: true indicates rule allocation is partially or completely from children components; false indicates children components did not contribute to allocation.</item>
    /// <item>RuleAllocationFromChosenRule: true indicates rule allocationis partially or completely from the chosen rule for this component; false indicates chosen rule for componenent did not contribute to allocation.</item>
    /// <item>AllocationFromBottomUpSize: true indicates allocation is partially or completely from a bottom up size allocation; false indicates bottom up size allocation did not contribute to allocation</item>
    /// <item>AllocationFromPackContentBreakOut: true indicates allocation is partially or completely from an allocation by pack content; false indicates pack content not used to determine allocation.</item>
    /// <item>AllocationFromBulkSizeBreakOut: true indicates allocation is partially or completely from a breakout of the color by size; false indicates breakout of color by size not used to determine allocation.</item>
    /// <item>AllocationModifiedAfterMultiHeaderSplit: true indicates header allocation for this store was modified after this header was split off of a Multi-Header; false indicates no modifications occurred after this header was split from a multi-header</item>
    /// <item>ItemValueManuallyAllocation: true indicates a manual change to the store quantity (Item) was made</item>
    /// <item>RuleAllocationFromGroup: true indicates rule allocation is partially or compoletely from the the Group Allocation Component</item>
    /// <item>ChosenRuleAcceptedByGroup: true indicates chosen rule was accepted by Group Allocation (when Header is part of Group Allocation)</item>
    /// <item>ItemQtyIsLocked:  True indicates VSW Balance occurred and this store's item quantity was locked; False: item qty is not locked (NOTE: only applies for total store, cannot lock a component's item qty independent of other components)</item>
    /// </list>
    /// </remarks>
    //  Warning
    //  Warning  The values associated with this flag correspond to bit positions in a uint field.
    //  Warning
    public enum eAllocationStoreDetailAuditFlag
    {
        ManuallyAllocated = 0,
        AutoAllocated = 1,
        ChosenRuleAcceptedByHeader = 2,   // TT#488 - MD - Jellis - Group Allocation
        Out = 3,
        Need = 4,
        FilledSizeHole = 5,
        //TempLock = 6,        // TT#59 Implement Temp lock   (flag position available)
        //Locked = 7,          // TT#1189 - MD - Jellis- Group Allocation - Implement Lock that does not persist to database pt 2
        RuleAllocationFromParentComponent = 8,
        RuleAllocationFromChildComponent = 9,
        RuleAllocationFromChosenRule = 10,
        AllocationFromBottomUpSize = 11,
        AllocationFromPackContentBreakOut = 12,
        AllocationFromBulkSizeBreakOut = 13,    // MID Track 4448 AnF Audit Enhancement
        AllocationModifiedAfterMultiHeaderSplit = 14,  // MID Track 4448 AnF Audit Enhancement // TT#1401 - JEllis - Urban Reservation Stores pt 2
        ItemValueManuallyAllocated = 15,                // TT#1401 - JEllis - Urban Reservation Stores pt 2 // TT#488 - Jellis - Group Allocation
        RuleAllocationFromGroupComponent = 16,               // TT#488 - MD - Jellis - Group Allocation
        ChosenRuleAcceptedByGroup = 17,              // TT#488 - MD - Jellis - Group Allocation // TT#1334 - Urban - Jellis - Balance to VSW Enhancement
        ItemQtyIsLocked = 18                   // TT#1334 - Urban - Jellis - Balance to VSW Enhancement
    }

    /// <summary>
    /// eAllocationTypeFlag indicates the allocation header type.
    /// </summary>
    /// <remarks>
    /// These flags identify the allocation type:
    /// <list type="bullet">
    /// <item>IsDummy: True indicates this header as a "dummy"</item>
    /// <item>HasPrimaryAllocation:  True indicates this allocation header is subordinate to a primary allocation</item>
    /// <item>HasSecondaryAllocation:  True indicates this allocation is the primary allocation for a secondary allocation</item>
    /// <item>WorkUpBulkColorBuy: True indicates system to calculate a recommended color buy based on color needs.</item>             
    /// <item>WorkUPBulkSizeBuy: True indicates system to calculate a recommended size buy based on size needs.</item>
    /// <item>WorkUpPackColorBuy:  True indicates system to calculate a recommended pack-color buy based on color needs.</item>
    /// <item>WorkUPPackSizeBuy: True indicates system to calculate a recommended pack-size buy based on size needs</item>
    /// <item>Reserve:  True indicates this was a basic replenishment allocation</item>
    /// <item>MultiHeader: True indicates this Header describes a multi-header allocation</item>
    /// <item>Receipt: True indicates this header describes a receipt in the warehouse</item>
    /// <item>PurchaseOrder: True indicates this header is associated with a purchase order</item>
    /// <item>ASN: True indicates this header describes an Automatic Ship Notice</item>
    /// <item>DropShip: True indicates this header describes a drop shipment.</item>
    /// <item>InUse: True indicates this header belongs to a MultiHeader</item>
    /// <item>BulkIsDetail: True indicates that bulk is subordinate to the DetailType; false indicates bulk is an equal of Detail type.  True indicates bulk is processed after all packs; false, indicates that bulk may be processed before packs that are allocated by content.</item>
    /// <item>WorkUpTotalBuy: True indicates the system to calculate a recommended Total store buy based on store need.</item>
    /// <item>IsInterfaced: True indicates header was added via an interface; false indicates header was added manually</item>
    /// </list>
    /// </remarks>
    //  Warning
    //  Warning  The values associated with this flag correspond to bit positions in a uint field.
    //  Warning
    public enum eAllocationTypeFlag
    {
        WorkUpBulkColorBuy = 0,
        WorkUpBulkSizeBuy = 1,
        WorkUpPackColorBuy = 2,
        WorkUpPackSizeBuy = 3,
        IsDummy = 4,
        HasPrimaryAllocation = 5,
        HasSecondaryAllocation = 6,
        Reserve = 7,
        MultiHeader = 8,
        Receipt = 9,
        PurchaseOrder = 10,
        ASN = 11,
        DropShip = 12,
        InUse = 13,
        BulkIsDetail = 14,
        WorkUpTotalBuy = 15,
        IsInterfaced = 16,
        Assortment = 17,
        Placeholder = 18, // TT#1401 - JEllis - Urban Reservation Stores pt 2
        ImoHeader = 19,   // TT#1401 - JEllis - Urban Reservation Stores pt 2
        AdjustVSW = 20,   // TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
                          //BEGIN TT#562-MD - stodd -  Bulk is not getting properly updated upon an assortment save of placeholders
        PlaceholderBulkColor = 21,
        PlaceholderBulkSize = 22,
        PlaceholderBulk = 23,
        PlaceholderPackColor = 24,
        PlaceholderPackSize = 25,
        PlaceholderPack = 26,
        //END TT#562-MD - stodd -  Bulk is not getting properly updated upon an assortment save of placeholders
    }

    /// <summary>
    /// Flag identifies wafer "cell" conditions.  Note:  Flag values correspond to the bits in a uiny
    /// </summary>
    public enum eAllocationWaferCellFlag
    {
        CellCanBeChanged = 0,
        CellIsValid = 1,
        MayExceedGradeMaximum = 2,
        MayExceedPrimaryMaximum = 3,
        MayExceedCapacityMaximum = 4,
        StoreExceedsCapacity = 5,
        StoreAllocationOutOfBalance = 6   // MID Track #1511 Highlight stores whose allocation is out of balance
    }

    // begin MID Track 3994 Performance
    /// <summary>
    /// Enumeration of the SQL Structure types used to hold an allocation
    /// </summary>
    public enum eSQL_StructureType
    {
        ShipToDayStructure = 0,
        GradeStructure = 1,
        CapacityStructure = 2,
        GeneralAuditStructure = 3,
        AllocatedStructure = 4,
        RuleStructure = 5,
        AllocatedAuditStructure = 6,
        NeedAuditStructure = 7,
        MinimumStructure = 8,
        MaximumStructure = 9,
        PrimaryMaxStructure = 10,
        DetailAuditStructure = 11,
        ShippingStructure = 12, // TT#1401 - JEllis - Urban Reservation Stores pt 2
        ImoStructure = 13, // TT#1401 - JEllis - Urban Reservation Stores pt 2
        ItemAllocatedStructure = 14,  // TT#1401 - JEllis - Urban Reservation Stores pt 2 // TT#246 - MD - JEllis - AnF VSW In-Store Minimum
        ItemMinimumStructure = 15  // TT#246 - MD - JEllis - AnF VSW In-Store Minimum
    }
    // end MID Track 3994 Performance

    // begin TT#173  Provide database container for large data collections
    public enum eSQLTimeIdType
    {
        TimeIdIsDaily = 0,
        TimeIdIsWeekly = 1
    }
    // end TT#173  Provide database container for large data collections

    public enum eAssignAllocationHdr
    {
        Primary = 0,
        Secondary = 1
    }

    /// <summary>
    /// eIntransitUpdateStatusFlag tracks the status of intransit updates
    /// </summary>
    /// <remarks>
    /// These flags track the intransit status:
    /// <list type="bullet">
    /// <item>StyleIntransitUpdated:  True indicates each store's total allocation has updated total intransit</item>
    /// <item>BulkColorIntransitUpdated:  True indicates each store's bulk color allocations have updated the appropriate color intransit</item>
    /// <item>BulkSizeIntransitUpdated:  True indicates each store's bulk color-width-size allocations have updated the appropriate color-width-size intransit</item>
    /// </list>
    /// </remarks>

    public enum eIntransitUpdateStatusFlag
    {
        StyleIntransitUpdated = 0,
        BulkColorIntransitUpdated = 1,
        BulkSizeIntransitUpdated = 2
    }


    /// <summary>
    /// Describes the ways in which to read intransit.
    /// </summary>
    /// <remarks>
    /// Intransit can be read and summarized as follows:
    /// <list type="bullet">
    /// <item>Total:  Read intransit for a given hierarchy node and summarize as a single total for each store within each day.</item>
    /// <item>Colors:  Read intransit for a given hierarchy node and summarize by color for each store within each day.</item>
    /// <item>SizesWithinColors: Read intransit for a given hierarchy node and summarize by size witin color for each store within each day.</item>
    /// <item>Sizes: Read intransit for a given hierarchy node and summarize by size for each store within each day.</item>
    /// </list></remarks>
    public enum eIntransitBy
    {
        Total = 1,
        Color = 2,
        SizeWithinColors = 3,
        Size = 4
    }

    // Begin TT#1224 - stodd - assortment committed
    /// <summary>
    /// Describes the ways in which to read committed.
    /// </summary>
    /// <remarks>
    /// Committed can be read and summarized as follows:
    /// <list type="bullet">
    /// <item>Total:  Read committed for a given hierarchy node and summarize as a single total for each store within each day.</item>
    /// <item>Colors:  Read committed for a given hierarchy node and summarize by color for each store within each day.</item>
    /// <item>SizesWithinColors: Read committed for a given hierarchy node and summarize by size witin color for each store within each day.</item>
    /// <item>Sizes: Read committed for a given hierarchy node and summarize by size for each store within each day.</item>
    /// </list></remarks>
    public enum eCommittedBy
    {
        Total = 1,
        Color = 2,
        SizeWithinColors = 3,
        Size = 4
    }
    // End TT#1224 - stodd - assortment committed

    // begin TT#488 - MD - JEllis - Urban Group Allocation
    public enum eNeedAllocationNode
    {
        None = 0,
        Group = 10,
        Style = 20,
        Total = 30,
        Type = 40,
        GenericType = 50,
        GenericPack = 60,
        DetailType = 70,
        DetailSubType = 80,
        NonGenericPack = 90,
        Bulk = 100,
        BulkColorTotal = 110,
        BulkColor = 120,
        ColorSizeTotal = 130,
        BulkColorSize = 140
    }
    // end TT#488 - MD - JEllis - Urban Group Allocation

    /// <summary>
    /// eAllocationNode enumerates allocation detail tracking levels within a single header.
    /// </summary>
    /// <remarks>
    /// This enumeration identifies all of the possible allocation tracking levels within a single header.
    /// eAllocationSummaryNode is a subset of this enumeration that identifies summary tracking levels
    /// within a header.  The two enumerations must be kept in sync.
    /// </remarks>
    public enum eAllocationNode
    {
        None = 0,
        Total = 1,
        Type = 2,
        GenericType = 3,
        DetailType = 4,
        DetailSubType = 5,
        GenericPack = 6,
        NonGenericPack = 7,
        Bulk = 8,
        BulkColorTotal = 9,
        BulkColor = 10,
        ColorSizeTotal = 11,
        BulkColorSize = 12
    }
    /// <summary>
    /// eAllocationSummaryNode enumerates allocation summary tracking levels within a single header.
    /// </summary>
    /// <remarks>
    /// This enumeration is a subset of the eAllocationNode enumeration.
    /// </remarks>
    public enum eAllocationSummaryNode
    {
        Total = 1,
        Type = 2,
        GenericType = 3,
        DetailType = 4,
        DetailSubType = 5,
        Bulk = 8,
        BulkColorTotal = 9

    }

    /// <summary>
    /// eHeaderNode enumerates allocation header summary tracking levels
    /// </summary>
    public enum eHeaderNode
    {
        //		None = 800790,
        Detail = 800791,
        Subtotal = 800792,
        GrandTotal = 800793
    }

    /// <summary>
    /// eStoreAllocationNode enumerates store allocation summary tracking levels
    /// </summary>
    public enum eStoreAllocationNode
    {
        //		None = 800780,
        Store = 800781,
        SetFilter = 800782,
        Set = 800783,
        Filter = 800784,
        All = 800785
    }

    public enum eStoreCountType
    {
        StoreCountNeed,
        StoreCountOnHand,
        StoreCountInTransit,
        StoreCountNeedBeforeAllocation
    }

    //	public enum eStoreSummaryType
    //	{
    //		Detail = 800770,
    //		Received = 800771,
    //		Balance = 800772,
    //		Count = 800773,
    //		Average = 800774,
    //		Total = 800775
    //	}

    //	/// <summary>
    //	/// eNeedAllocationMode enumerates the ways in which to allocate by need.
    //	/// </summary>
    //	public enum eNeedAllocationMode
    //	{
    //		AllocateTotalToStore,
    //		AllocateGenericToStore,
    //		AllocateDetailToStore,
    //		AllocateBulkToStore,
    //		AllocatePackToStore,
    //		AllocateColorToStore,
    //		AllocateColorSizeToStore,
    //		BreakoutStoreColorToSize,
    //		BreakoutStoreBulkToColor,
    //		FillSizeHoles,
    //		FillColorSizeHoles,
    //		FillBulkColorHoles
    //	}

    /// <summary>
    /// eAllocationType enumerates the high-level breakdown of an allocation. 
    /// </summary>
    public enum eAllocationType
    {
        GenericType = 800745,
        DetailType = 800746
    }

    /// <summary>
    /// eDistributeChange identifies how a change to an allocation is to be distributed to other components. 
    /// </summary>
    public enum eDistributeChange
    {
        ToNone = 0,
        ToParent = 1,
        ToAll = 2,
        ToChildren = 3
    }

    public enum eAllocationCoordinateType
    {
        None = 0,  // MID Track 4326 Cannot manually enter size in Size Review
        Header = 1,  // MID Track 4326 Cannot manually enter size in Size Review
        HeaderTotal = 2,  // MID Track 4326 Cannot manually enter size in Size Review
        Component = 3,  // MID Track 4326 Cannot manually enter size in Size Review
        Size = 4,  // MID Track 4326 Cannot manually enter size in Size Review
        SizesTotal = 5,  // MID Track 4326 Cannot manually enter size in Size Review
        StoreAllocationNode = 6,  // MID Track 4326 Cannot manually enter size in Size Review
        Variable = 7,  // MID Track 4326 Cannot manually enter size in Size Review
        PackName = 8,  // MID Track 4326 Cannot manually enter size in Size Review
        BalanceChainToHeader = 9,  // MID Track 4326 Cannot manually enter size in Size Review
        VolumeGrade = 10,  // MID Track 4326 Cannot manually enter size in Size Review
        PrimarySize = 11,  // MID Track 4326 Cannot manually enter size in Size Review
        SecondarySize = 12,  // MID Track 4326 Cannot manually enter size in Size Review
        SecondarySizeTotal = 13,  // MID Track 4326 Cannot manually enter size in Size Review
        SecondarySizeNone = 14   // MID Track 4326 Cannot manually enter size in Size Review
                                 //		Received,
                                 //		StoreCount,
                                 //		Average,
                                 //		Total
    }

    public struct structWMExpStaticNode
    {
        public int WM_Static_Node_RID;
        public string Text_Value;
        public int Node_Enum_ID;
        public int Level_1;
        public int Level_2;
        public int Level_3;
        public int Level_4;
    }

    public struct structMethodAltKey
    {
        public eMethodTypeUI Method_Type_ID;
        public int User_RID;
        public string Method_Name;
    }

    // Enqueue/Dequeue Lock Types
    public enum eLockType
    {
        Intransit = 1,
        StoreWeek = 2,
        ChainWeek = 3,
        Header = 4,
        Hierarchy = 5,
        HierarchyNode = 6,
        Store = 7,
        StoreGrade = 8,
        StoreGradeLevel = 9,
        HierarchyBranch = 10,
        EligibilityModel = 11,
        StockModifierModel = 12,
        SalesModifierModel = 13,
        VersionWeek = 14,
        Method = 15,
        Workflow = 16,
        Text = 17,
        Forecasting = 18,
        ForecastBalance = 19,
        TaskList = 20,
        Job = 21,
        Schedule = 22,
        ColorCode = 23,
        SizeCode = 24,
        // BEGIN MID Track #4370 - John Smith - FWOS Models
        //Begin Track #4052 - JScott - Filters not being enqueued
        //		RollupItem			= 25
        RollupItem = 25,
        // END MID Track #4370
        Filter = 26,
        //End Track #4052 - JScott - Filters not being enqueued
        //Begin Track #4970 - RonM - Size models not being enqueued
        SizeAlternates = 27,
        SizeConstraints = 28,
        SizeGroup = 29,
        SizeCurve = 30,
        //End Track #4970 
        //Begin - Abercrombie & Fitch #4411 - JSmith - Chain Forecasting
        FWOSModifier = 31,
        ComputationItem = 32,
        //End - Abercrombie & Fitch #4411
        // Begin Track #6368 – JSmith - Invalid object name 'dbo.FAVORITES'
        //SpecialRequest		= 33
        SpecialRequest = 33,
        Assortment = 34,
        //StoreGroup = 35
        StoreGroup = 35,
        ProductCharacteristic = 36,
        // End TT#287
        // End Track #6368
        StyleColorSize = 37,             // TT#173 Create Color-Size Blob Structure
        StoreVariableHistory = 38,       // TT#173  Provide database container for large data collections
        HeaderCharacteristicGroup = 39,  // TT#721  Duplicate header characteristics
        HeaderCharacteristicValue = 40,   // TT#721  Duplicate header characteristics
                                          //Begin TT#644 - JScott - Unable to allocate using Size Need with size curves at the Class level.
        SizeCurveGroup = 41,
        //End TT#644 - JScott - Unable to allocate using Size Need with size curves at the Class level.
        //Begin TT#1312 - JScott - Alternate Hierarchy Reclass
        HierarchyReclass = 42,
        //End TT#1312 - JScott - Alternate Hierarchy Reclass
        // Begin TT#1753 - JSmith - New transaction is not allowed because there are other threads running in the session
        Audit = 43,
        // End TT#1753
        // Begin TT#2015 - gtaylor - apply changes to lower levels
        NodeMasterLock = 44,
        // End TT#2015 - gtaylor - apply changes to lower levels
        // Begin TT#108 - MD - doconnell - FWOS Max Model
        FWOSMaxModel = 45,
        // End TT#108 - MD - doconnell - FWOS Max Model

        StoreCharacteristicGroup = 46, //TT#1517-MD -jsobek -Store Service Optimization
        StoreCharacteristicValue = 47, //TT#1517-MD -jsobek -Store Service Optimization
        StoreFields = 48, //TT#1517-MD -jsobek -Store Service Optimization
        StoreAddNew = 49, //TT#1517-MD -jsobek -Store Service Optimization
        StoreLoadRunning = 50, //TT#1517-MD -jsobek -Store Service Optimization
                               //Begin TT#1312 - JScott - Alternate Hierarchy Reclass
        OverrideLowLevelModel = 51,
        StoreEligibility = 52,
        AlternateHierarchyReclass = 9000,
        //End TT#1312 - JScott - Alternate Hierarchy Reclass
    }

    public enum eLockStatus
    {
        Undefined = 0,
        Locked = 1,
        ReadOnly = 2,
        Cancel = 3
    }

    public enum eOnHandVariable
    {
        InventoryTotal = 1,
        InventoryRegular = 2
    }

    public enum eSizeProcessControl
    {
        SizeCurvesOnly = 1,
        SizeCurvesAndPlanOnly = 2,
        ProcessAll = 3
    }

    // begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement
    /// <summary>
    /// Identifies size plan type
    /// </summary>
    public enum eSizePlanType
    {
        SizeNeed = 0,
        FillToOwn = 1,
        FillToPlan = 2
    }
    // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement

    //Generic Database Errors
    public enum eGenericDBError
    {
        DuplicateKey = -1,
        InvalidDataType = -2,
        DeleteErrorConstraint = -3,
        GenericDBError = -4
    }

    public enum eMethodErrors
    {
        DuplicateMethod = 080156,
        GenericMethodInsertError = 080157
    }

    // scheduler
    //	public enum eTaskType
    //	{
    //		Allocation	 		= 801500,
    //		OTSPlan				= 801501,
    //		Rollup				= 801502
    //	}
    //	public enum eTaskStatus
    //	{
    //		Submitted			= 801510,
    //		Started				= 801511,
    //		StoppedForReview	= 801512,
    //		Completed			= 801513,
    //		Failed				= 801514
    //	}
    //	public enum eAllocationTaskType
    //	{
    //		Reserve				= 801550,
    //		Header				= 801551,
    //		PO					= 801552
    //	}
    //	public enum eRollupType
    //	{
    //		RollToNodeFromLevel	= 801560,				// roll node values from requested level to top of hierarchy
    //		RollFromNodeToTop	= 801561,				// roll node values to the top of the hierarchy 
    //		RollToAlternate		= 801562,				// roll values into alternate hierarchy
    //	}
    //	public enum eScheduleTimeUnit
    //	{
    //		Hour				= 801520,
    //		Day					= 801521,
    //		Week				= 801522,
    //		Month				= 801523
    //	}
    //	public enum eScheduleType
    //	{
    //		ImmediateLocal		= 801530,				// run immediately, if possible on user's PC
    //		ImmediateRemote		= 801531,				// run immediately, but not on user's PC
    //		ScheduleOneTime		= 801532,				// scheduled (not immediate), non-recurring
    //		ScheduleRecurring	= 801533				// scheduled (not immediate), recurring
    //	}
    //	public enum eTaskPriority
    //	{
    //		Low					= 801540,
    //		Medium				= 801541,
    //		High				= 801542
    //	}

    // Schedule enums

    public enum eProcessExecutionStatus
    {
        None = 801600,
        Waiting = 801601,
        Running = 801602,
        OnHold = 801603,
        Completed = 801604,
        Cancelled = 801605,
        Executed = 801606,
        Failed = 801607,
        InError = 801608,
        //Begin Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
        //Unexpected		= 801609
        Unexpected = 801609,
        Queued = 801610
        //Begin Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
    }
    public enum eScheduleByType
    {
        Once = 801620,
        Day = 801621,
        Week = 801622,
        Month = 801623
    }

    public enum eScheduleByMonthWeekType
    {
        None = 801640,
        Every = 801641,
        First = 801642,
        Second = 801643,
        Third = 801644,
        Fourth = 801645,
        Last = 801646
    }

    public enum eScheduleRepeatIntervalType
    {
        None = 801660,
        Seconds = 801661,
        Minutes = 801662,
        Hours = 801663
    }

    public enum eScheduleConditionType
    {
        None = 801680,
        ByFileExtension = 801681
    }

    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //public enum eTaskListNodeType
    //{
    //    None,
    //    MainTaskListFolder,
    //    FavoriteTaskListFolder,
    //    GlobalTaskListFolder,
    //    SystemTaskListFolder,
    //    UserTaskListFolder,
    //    JobFolder,
    //    TaskList,
    //    Shortcut,
    //    Job,
    //    SpecialRequestFolder,		// Issue 5117
    //    SpecialRequest		// Issue 5117
    //}
    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

    public enum eScheduleUpdateStatus
    {
        CreateSchedule,
        CreateScheduleAndJob,
        UpdateSchedule
    }

    public enum eSpreadOption
    {
        Basis,
        Plan
    }

    public enum eForecastMonitorMode
    {
        Default = 0,
        Verbose = 1
    }

    // begin TT#552 Build Packs provide pack properties for TAB 4
    /// <summary>
    /// Describes the available pack properties within Build Packs
    /// </summary>
    public enum eBuildPackProperty
    {
        AllStoreTotalPackUnits = 900731,
        NonReserveTotalPackUnits = 900732,
        ReserveTotalPackUnits = 900733,
        AllStoreTotalNumberOfPacks = 900734,
        ReserveTotalNumberOfPacks = 900735,
        NonReserveTotalNumberOfPacks = 900736,
        AverageErrorPerSizeWithUnits = 900740,
        AverageErrorPerStoreWithPacks = 900745,
        CountOfAllStoresWithPacks = 900755,
        CountOfNonReserveStoresWithPacks = 900756
    }
    // end TT#552 Build Packs provide pack properties for TAB 4

    public enum eVelocityMinMax
    {
        None = 806201,
        StoreGradeMinMax = 806202,
        VelocityGradeMinMax = 806203,
    }


    //	#region MethodStructures
    //
    //	public struct structOTSPlan
    //	{
    //		public int Method_RID;
    //		public int Plan_HN_RID;
    //		public int Plan_FV_RID;
    //		public int CDR_RID;
    //		public int Chain_FV_RID;
    //		public char Bal_Sales_IND;
    //		public char Bal_Stock_IND;
    //	}
    //
    //	#endregion
    // BEGIN TT#739-MD - STodd - delete stores
    public enum eStoreDeleteTableStatus
    {
        NotStarted = 0,
        InProgress = 1,
        Completed = 2,
    }
    // END TT#739-MD - STodd - delete stores

    // Begin TT#1595-MD - stodd - Batch comp
    /// <summary>
    /// Used in Batch Comp processing. More values will be added as needed.
    /// </summary>
    public enum eBatchComp
    {
        All = 900948
    }
    // End TT#1595-MD - stodd - Batch comp

    // Begin TT#1652-MD - RMatelic - DC Carton Rounding
    /// <summary>
    /// Used in DC Carton Rounding Method
    /// </summary>
    public enum eAllocateOverageTo
    {
        AllocatedStores = 0,
        Reserve = 1,
    }
    // End TT#1652-MD

    /// <summary>
    /// Specifies which message is to be displayed.
    /// </summary>
    public enum eMessageRequest
    {
        None = 0,
        CreatePlaceholderContinue = 1
    }
    
    /// <summary>
    /// Specifies identifiers to indicate the return value of a message.
    /// </summary>
    public enum eMessageResponse
    {
        None = 0,
        OK = 1,
        Cancel = 2,
        Abort = 3,
        Retry = 4,
        Ignore = 5,
        Yes = 6,
        No = 7
    }

    /// <summary>
    /// Specifies identifiers to indicate the status of a message.
    /// </summary>
    public enum eMessagingStatus
    {
        None = 0,
        MessageSent = 1,
        WaitingForResponse = 2,
        ResponseReceived = 3
    }

    public enum eRequestingApplication
    {
        Allocation,
        Forecast,
        Assortment
    };
}
