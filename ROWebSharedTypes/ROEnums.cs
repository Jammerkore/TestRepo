namespace Logility.ROWebSharedTypes
{
    public enum eROReturnCode
    {
        Successful = 0,
        Failure = 2,
        ChangedToReadOnly = 3
    };

    public enum eAssortmentReviewTabType
    {
        AssortmentReviewMatrix,
        AssortmentReviewContent,
        AssortmentReviewCharacteristics
    }
    public enum eScreenID
    {
        ChainPlan = 1,
        PlanView = 2,
        PlanViewRT = 3,
        StyleView = 4,
		
        StorePlan = 5
		
    };

    // BEGIN TT#1156-MD CTeegarden add Chain Ladder functionality
    public enum eTimePeriod
    {
        Years = 1,
        Seasons = 2,
        Quarters = 3,
        Months = 4,
        Weeks = 5
    }
    // END TT#1156-MD CTeegarden add Chain Ladder functionality


    public enum eROCalendarModelPeriodType
    {
        None = 800905,
        Month = 800906,
        Quarter = 800907,
        Season = 800908,
        Year = 800909
    }

    ///// <summary>
    ///// Identifies the type of planning session to create.  
    ///// Must correspond to the eROClass.
    ///// </summary>
    //public enum ePlanSessionType
    //{
    //    None = 0,
    //    StoreSingleLevel = 1,
    //    StoreMultiLevel = 2,
    //    ChainSingleLevel = 3,
    //    ChainMultiLevel = 4
    //};

    public enum eAssortViewType
    {
        Selection,
        Properties
    };
    /// <summary>
    /// Identifes the class to instantiate for processing
    /// </summary>
    public enum eROClass
    {
        RONone,
        ROSystem,
        ROAdministration,
        ROCalendar,
        ROGlobalOptions,
        ROHierarchy,
        ROHierarchyProperties,
        ROStore,
        ROChainSingleLevel,
        ROChainMultiLevel,
        ROStoreSingleLevel,
        ROStoreMultiLevel,
        ROAllocation,
        ROGroupAllocation,
        ROAssortment,
        ROWorkflowMethodExplorer,
        ROStoreGroupExplorer,
        ROStoreFilterExplorer,
        ROHeaderFilterExplorer,
        ROAssortmentFilterExplorer,
        ROPlanning,
        ApplicationCommon,
        ROMerchandiseExplorer
    };

    /// <summary>
    /// Identifies the function requested
    /// </summary>
    /// <remarks>
    /// Used in GetData and ProcessData methods of the classes.
    /// </remarks>
    public enum eRORequest
    {
        Undefined,

        // System
        GetActiveClassesAndInstances,
        RemoveInstance,
        GetActiveUsers,

        // Administration
        GetUserInformation,
        SaveUser,
        GetCharacteristics,
        GetCharacteristic,
        SaveCharacteristics,
        DeleteCharacteristic,
        GetModels,
        GetModel,
        SaveModel,
        SaveAsModel,
        CopyModel,
        DeleteModel,
        ApplyModel,

        // Common
        GetFilters,
        GetVersions,
        GetViews,
        GetGlobalOptions,
        GetUserLastValues,
        SaveUserLastValues,
        UpdateGlobalOptions,
        GetSizeGroupList,
        GetSizeCurveGroupList,
        GetOtsPlanningOverrideLowLevelList,
        GetMethod,
        SaveMethod,
        ProcessMethod,
        CopyMethod,
        DeleteMethod,
        ApplyMethod,
        GetInUse,
        GetFunctionSecurity,
        ReleaseResources,
        //Calendar
        GetCalendarSelector,
        UpdateCalendarSelector,
        RenameDateRange,
        DeleteDateRange,
        GetCalendarModel,
        GetAbout,
        GetColors,
        DeleteWorkflow,
        Rename,
        Delete,
        Copy,
        AddShortCut,
        DeleteShortCut,
        AddFolder,
        DeleteFolder,
        SaveAs,

        //Hierarchy
        GetHierarchies,
        GetHierarchyChildren,
        NodePropertiesGet,
        NodePropertiesApply,
        NodePropertiesSave,
        NodePropertiesDelete,
        NodePropertiesDeleteDescendants,
        HierarchyPropertiesGet,
        HierarchyPropertiesApply,
        HierarchyPropertiesSave,
        HierarchyPropertiesDelete,

        //Stores
        GetStoreProfile,
        StoreComboFiltersData,
        GetStoreFilterExplorerData,
        StoreAttributeList,
        StoreGroupExplorerData,

        //Header Explorer
        GetHeaderFilterExplorerData,

        //Assortment Explorer
        GetAssortmentFilterExplorerData,

        //Merchandise Explorer
        GetMerchandiseExplorerData,

        //Workflow Method Explorer
        GetWorkflowMethodExplorerData,

        //All Explorers
        RefreshExplorerData,

        //Planning
        GetPlanningFilterData,
        OpenCube,
        CloseCube,
        GetROCubeMetadata,
        GetCubeData,
        HandlePeriodChange,
        CellChanged,
        RecomputeCubes,
        UndoLastRecompute,
        SaveCubeGroup,
        GetVariables,
        SelectVariables,
        GetComparatives,
        SelectComparatives,
        OTSForecastWorkflows,
        CellLock,
        CellUnlock,
        PlanningVersionsList,
        GetStoreAttributeSet,
        ProcessOTSForecastWorkflow,        
        GetOTSPlanSelectionData,
        OTSForecastWorkFlowDetails,
        SavePlanningWorkflow,
        GetPlanningWorkFlowMethodList,
        GetPlanningWorkFlowMethodActionsList,
        GetPlanningWorkFlowVariablesList,


        //Allocation
        GetAllocationHeaders,
        AllocationViews,
        AllocationActions,
        AllocationWorkflows,
        AllocationWorkFlowDetails,
        SaveAllocationWorkflow,
        
        SetAllocationSelectedHeaders,
        GetAllocationSelectedHeaderDetails,
        AllocationWorklistView,
        SetAllocationSelectedFilterHeaders,
        AllocationStyleReviewViews,
        AllocationStyleReviewVelocityViews,
        AllocationStyleReviewVelocityRules,
        UpdateStyleReviewChanges,
        UpdateSizeReviewChanges,
        UpdateSummaryReviewChanges,
        GetAllocationViewSelection,
        GetAllocationSizeReviewViews,
        GetAllocationStyleReview,
        GetAllocationSizeReview,
        GetAllocationSummaryReview,
		SaveAllocationStyleReview,
        SaveAllocationSizeReview,
        SaveAllocationSummaryReview,
        GetAllocationWorkFlowMethodList,
        GetAllocationWorkFlowMethodActionsList,
        GetAllocationWorklistColumns,
        GetVendorList,

        //Assortment 
        AssortmentActions,
        AssortmentFilters,
        AssortmentHeaderData,
        GetAssortmentWorklistViewDetails,
        AssortmentSelectedFilterHeaderData,
        GetAssortmentReviewViews,
        GetAssortmentProperties,
        GetAssortmentReviewSelection,
        GetAssortmentWorklistViews,
        UpdateAssortmentReview,
        UpdateAssortmentProperties,
        UpdateAssortmentSelection,
        SetAssortmentPropertiesToProfile,
        ApplyAssortmentReviewMatrixChanges,
        GetAssortmentReviewMatrixData,
        SaveAssortmentReviewChanges,
        GetAssortmentContentCharacteristics,
        ProcessAllocationStyleReviewAction,
        GetAssortmentUserLastValues,
        SaveAssortmentUserLastValues,
        ProcessAllocationSizeReviewAction,
        ProcessAllocationSummaryReviewAction,
        ProcessAllocationWorklistAction,
        ProcessAllocationWorkflow,
        ProcessAssortmentReviewAllocationAction,
        GetAssortmentReviewViewList,
        SetAssortmentSelectedHeaders,
        ProcessAssortmentAction,
		UpdateAssortmentContentCharacteristics,
        

        //Size Groups
        GetSizeGroupDetails,
        GetLowLevelList,

        

        

    };
    public enum eROTrendCaps
    {
        None = 0,
        Tolerance = 1,
        Limits = 2
    }
    public enum eApplyTrendOptions
    {
        ChainPlan = 0,
        ChainWOS = 1,
        PlugChainWOS = 2
    }

    public enum eROLevelsType
    {
        None = 0,
        HierarchyLevel = 1,
        Characteristic = 2,
        LevelOffset = 3
    }

    public enum eIconType
    {
        Plain = 0,
        Shortcut = 1,
        Shared = 2
    };

    public enum eIconColor
    {
        Yellow = 0,
        Silver = 1,
        MedBlue = 2,
        Orange = 3,
        Pink = 4,
        Purple = 5,
        Red = 6,
        Green = 7,
        Khaki = 8,
        LightBlue = 9,
        LightGreen = 10,
        Magenta = 11,
        Black = 12,
        Blue = 13,
        Brown = 14,
        Default = 15
    };

    public enum eGridOrientation
    {
        None,
        TimeOnColumn,
        TimeOnRow
    };
    public enum eCellDataType
    {
        None,
        Coordinate,
        Label,
        Units,
        Dollar,
        Percentage
    };

    public enum eCellValueType
    {
        None,
        String,
        Number,
        Integer,
        RuleType
    };

    public enum eDataType
    {
        None,
        ChainDetail,
        ChainDetail_Year,
        ChainDetail_Season,
        ChainDetail_Quarter,
        ChainDetail_Month,
        ChainDetail_Week,
        ChainTotals,
        ChainLowLevelTotals,
        ChainLowLevelDetail,
        ChainSummaryTotals,
        ChainSummaryDetail,
        StoreDetail,
        StoreTotals,
        StoreSummary,

        SetDetail,
        SetTotals,
        SetSummary,

        AllStoreDetail,
        AllStoreTotals,
        AllStoreSummary,

        AssortmentDetailLabels,

        AssortmentSummaryTotals,
        AssortmentDetailTotals,
        AssortmentTotalTotals,

        AssortmentSummaryDetail,
        AssortmentDetailDetail,
        AssortmentTotalDetail

    };

    public enum eROVersionsType
    {
        Chain = 1,
        Store = 2,
        Union = 3
    };

    public enum eROApplicationType
    {
        All,
        Allocation,
        Forecast,
        Assortment
    };

    public enum eMinMaxType
    {
        Allocation,
        Inventory
    };

    public enum eContentType
    {
        Undefined,
        Header,
        PlaceholderStyle,
        PlaceholderColor,
        BulkColor,
        BulkColorSize,
        Pack,
        PackColor,
		PackColorSize
    };

    public enum eROConnectionStatus
    {
        Failed,
        FailedBatchOnlyMode,
        FailedAlreadyLoggedIn,
        FailedInvalidUser,
        Successful,
        SuccessfulBatchOnlyMode
    };

    public enum eCharacteristicValueType
    {
        text = 0,
        date = 1,
        number = 2,
        dollar = 3,
        unknown = 4,
        list = 5,
        boolean = 6
    }

    public enum eVelocityCalculateAverageUsing
    {
        AllStores,
        AttributeSetAverage
    }

    public enum eVelocityDetermineShipQtyUsing
    {
        Basis,
        Header
    }

    public enum eVelocityApplyMinMaxType
    {
        None,
        StoreGrade,
        VelocityGrade
    }

    public enum eVelocityMatrixMode
    {
        None,
        Normal,
        Average
    }

    public enum eVelocitySpreadOption
    {
        None,
        Index,
        Smooth
    }

    public enum eVelocityAction
    {
        None,
        ProcessInteractive,
        ClearMatrix
    }

    public enum ePurgeDataType
    {
        None,
        DailyHistory,
        WeeklyHistory,
        ForecastPlans,
        HeaderASN,
        HeaderDropShip,
        HeaderDummy,
        HeaderReceipt,
        HeaderPurchaseOrder,
        HeaderReserve,
        HeaderVSW,
        HeaderWorkUpBuy
    }
}
