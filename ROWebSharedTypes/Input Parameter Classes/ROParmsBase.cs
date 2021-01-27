using MIDRetail.DataCommon;
using System;
using System.Runtime.Serialization;

namespace Logility.ROWebSharedTypes
{
    [DataContract]

    [KnownType(typeof(eROClass))]
    [KnownType(typeof(eSecurityActions))]
    [KnownType(typeof(eSecurityFunctions))]
    [KnownType(typeof(eProductType))]
    [KnownType(typeof(eHierarchyLevelType))]
    [KnownType(typeof(eOTSPlanLevelType))]
    [KnownType(typeof(ePlanLevelSelectType))]
    [KnownType(typeof(eMaskField))]
    [KnownType(typeof(eHierarchyType))]
    [KnownType(typeof(eHierarchyRollupOption))]
    [KnownType(typeof(eLevelLengthType))]
    [KnownType(typeof(eHierarchyDisplayOptions))]
    [KnownType(typeof(eHierarchyIDFormat))]
    [KnownType(typeof(ePurgeTimeframe))]
    [KnownType(typeof(eMerchandiseType))]
    [KnownType(typeof(eMinMaxType))]
    [KnownType(typeof(eVelocityApplyMinMaxType))]
    [KnownType(typeof(eVelocityMethodGradeVariableType))]
    [KnownType(typeof(eVelocityMatrixMode))]
    [KnownType(typeof(eVelocitySpreadOption))]
    [KnownType(typeof(eVelocityRuleRequiresQuantity))]
    [KnownType(typeof(eVelocityRuleType))]
    [KnownType(typeof(eVelocityCalculateAverageUsing))]
    [KnownType(typeof(eVelocityDetermineShipQtyUsing))]
    [KnownType(typeof(eVelocityAction))]
    [KnownType(typeof(ePurgeDataType))]

    [KnownType(typeof(ROMerchandiseListEntry))]

    [KnownType(typeof(SecurityPermission))]
    [KnownType(typeof(RONoParms))]
    [KnownType(typeof(ROKeyParms))]
    [KnownType(typeof(ROStringParms))]
    [KnownType(typeof(RODataTableParms))]
    [KnownType(typeof(RODataSetParms))]
    [KnownType(typeof(ROCubeOpenParms))]
    [KnownType(typeof(ROCubeGetMetadataParams))]
    [KnownType(typeof(ROCubeGetDataParams))]
    [KnownType(typeof(ROCubePeriodChangeParams))]
    [KnownType(typeof(ROGridCellChange))]
    [KnownType(typeof(ROGridChangesParms))]
    [KnownType(typeof(ROColumnFormat))]
    [KnownType(typeof(ROPagingParms))]
    [KnownType(typeof(ROCalendarSelectorParms))]
    [KnownType(typeof(ROCalendarDeleteParams))]
    [KnownType(typeof(ROCalendarSaveParms))]
    [KnownType(typeof(ROAllocationWorklistLastDataParms))]
    [KnownType(typeof(ROListParms))]
    [KnownType(typeof(ROIntParms))]
    [KnownType(typeof(ROTreeNodeParms))]
	[KnownType(typeof(ROPlanningVersionsParms))]
    [KnownType(typeof(ROAssortmentPropertiesParms))]
    [KnownType(typeof(ROAllocationReviewOptionsParms))]
    [KnownType(typeof(ROAssortmentAllocationActionParms))]

    [KnownType(typeof(ROWorkflow))]
    [KnownType(typeof(ROPlanningWorkflowStep))]
    [KnownType(typeof(ROAllocationWorkflowStep))]
    [KnownType(typeof(ROWorkflowStep))]
    [KnownType(typeof(ROWorkflowPropertiesParms))]

    [KnownType(typeof(ROBaseProperties))]
    [KnownType(typeof(ROMethodParms))]
    [KnownType(typeof(ROMethodPropertiesParms))]
    [KnownType(typeof(ROMethodGeneralAllocationProperties))]
    [KnownType(typeof(ROMethodRuleProperties))]
    [KnownType(typeof(ROMethodFillSizeHolesProperties))]
    [KnownType(typeof(ROMethodSizeNeedProperties))]
    [KnownType(typeof(ROMethodBasisSizeProperties))]
    [KnownType(typeof(ROSizeConstraintDimensionSizes))]
    [KnownType(typeof(ROSizeConstraintProperties))]
    [KnownType(typeof(ROSizeCurveProperties))]
    [KnownType(typeof(ROMethodSizeRuleAttributeSet))]
    [KnownType(typeof(ROMethodSizeRuleProperties))]
    [KnownType(typeof(ROMethodDCFulfillmentProperties))]
    [KnownType(typeof(ROMethodDCStoreCharacteristicSet))]
    [KnownType(typeof(ROMethodDCStoreCharacteristicProperties))]
    [KnownType(typeof(ROMethodBuildPacksProperties))]
    [KnownType(typeof(PackPatternCombo))]
    [KnownType(typeof(BuildPacksMethod_Combo))]
    [KnownType(typeof(Vendor_Combo))]
    [KnownType(typeof(PackPatternList))]
    [KnownType(typeof(OptionPackList))]
    [KnownType(typeof(PackPattern))]
    [KnownType(typeof(BuildPacksMethod_PackPattern))]
    [KnownType(typeof(Vendor_PackPattern))]
    [KnownType(typeof(OptionPack_PackPattern))]
    [KnownType(typeof(ROMethodSizeCurveProperties))]
    [KnownType(typeof(ROMethodSizeCurveMerchBasisSet))]
    [KnownType(typeof(ROMethodSizeCurveMerchBasisProperties))]
    [KnownType(typeof(ROLowLevelModelParms))]
    [KnownType(typeof(ROInUseParms))]
    [KnownType(typeof(ROProfileKeyParms))]
    [KnownType(typeof(ROProfileKey))]
    [KnownType(typeof(ROCalendarDateInfo))]
    [KnownType(typeof(ROWorkflowMethodParms))]

    [KnownType(typeof(ROAssortmentActionParms))]
    [KnownType(typeof(ROAssortmentAction))]
    [KnownType(typeof(ROAssortmentActionsSpreadAverage))]
    [KnownType(typeof(ROAssortmentActionsCreateplaceHolders))]
    [KnownType(typeof(ROAssortmentActionsGradeDetails))]
    [KnownType(typeof(ROBaseAssortmentActionDetails))]
    [KnownType(typeof(ROAssortmentReviewOptionsParms))]
	[KnownType(typeof(ROAssortmentReviewOptions))]
    [KnownType(typeof(ROAssortmentUpdateContentCharacteristicsParms))]
    [KnownType(typeof(ROUpdateContent))]
    [KnownType(typeof(ROReleaseResourceParms))]
    [KnownType(typeof(ROBaseUpdateParms))]
    [KnownType(typeof(ROWorklistUpdateParms))]
    [KnownType(typeof(ROWorklistRenameParms))]
    [KnownType(typeof(ROWorklistDeleteParms))]
    [KnownType(typeof(ROWorklistCopyParms))]
    [KnownType(typeof(RODataExplorerRenameParms))]
    [KnownType(typeof(RODataExplorerCopyParms))]
    [KnownType(typeof(RODataExplorerSaveAsParms))]
    [KnownType(typeof(RODataExplorerShortcutParms))]
    [KnownType(typeof(RODataExplorerFolderParms))]
    [KnownType(typeof(ROPlanningForecastMethodProperties))]
    [KnownType(typeof(eApplyTrendOptions))]
    [KnownType(typeof(eGroupLevelFunctionType))]
    [KnownType(typeof(eGroupLevelSmoothBy))]
    [KnownType(typeof(ROOverrideLowLevel))]
    [KnownType(typeof(ROPlanningGlobalLockUnlockProperties))]
    [KnownType(typeof(ROLevelInformation))]
    [KnownType(typeof(eROLevelsType))]
    [KnownType(typeof(ROAssortmentReviewSaveOptionsParms))]
    [KnownType(typeof(ROMethodCopyForecastProperties))]
    [KnownType(typeof(ROMethodRollupProperties))]
    [KnownType(typeof(ROMethodRollupOptionsBasis))]
    [KnownType(typeof(ROSelectedField))]
    [KnownType(typeof(ROMethodForecastSpreadProperties))]
    [KnownType(typeof(ROPlanningModifySalesProperties))]

    [KnownType(typeof(eTrendCapID))]
    [KnownType(typeof(ROPlanningForecastExportProperties))]
    [KnownType(typeof(ROVariable))]
    [KnownType(typeof(ROVariableGrouping))]
    [KnownType(typeof(ROVariableGroupings))]
    [KnownType(typeof(ROMethodMatrixBalanceProperties))]
    [KnownType(typeof(ROPlanningForecastMethodAttributeSetProperties))]
    [KnownType(typeof(ROForecastingBasisDetailsProfile))]
    [KnownType(typeof(eExportType))]
    [KnownType(typeof(ePlanType))]
    [KnownType(typeof(eExportDateType))]
    [KnownType(typeof(eExportSplitType))]
    [KnownType(typeof(eAllocationAssortmentViewGroupBy))]
    [KnownType(typeof(ROMethodAllocationOverrideProperties))]
    [KnownType(typeof(ROStoreGrade))]
    [KnownType(typeof(ROPlanningStoreGrade))]
    [KnownType(typeof(ROAllocationStoreGrade))]
    [KnownType(typeof(ROAttributeSetStoreGrade))]
    [KnownType(typeof(ROAllocationVelocityGrade))]
    [KnownType(typeof(ROMethodOverrideCapacityProperties))]
    [KnownType(typeof(ROMethodOverrideColorProperties))]
    [KnownType(typeof(ROMethodOverridePackRoundingProperties))]
    [KnownType(typeof(ROMethodOverrideVSW))]
    [KnownType(typeof(ROMethodOverrideVSWAttributeSet))]
    [KnownType(typeof(ROMethodCreateMasterHeadersProperties))]
    [KnownType(typeof(ROMethodCreateMasterHeadersMerchandise))]
    [KnownType(typeof(ROMethodDCCartonRoundingProperties))]
    [KnownType(typeof(ROMethodPlanningExtractProperties))]
    [KnownType(typeof(ROMethodAllocationVelocityProperties))]
    [KnownType(typeof(ROMethodAllocationVelocityAttributeSet))]
    [KnownType(typeof(ROMethodAllocationVelocityMatrixVelocityGrade))]
    [KnownType(typeof(ROMethodAllocationVelocityMatrixCell))]
    [KnownType(typeof(ROBasisWithLevelDetailProfile))]

    [KnownType(typeof(ROCharacteristicsPropertiesParms))]

    [KnownType(typeof(ROModelParms))]
    [KnownType(typeof(ROSizeCurveModelParms))]
    [KnownType(typeof(ROSizeConstraintModelParms))]
    [KnownType(typeof(ROModelProperties))]
    [KnownType(typeof(ROModelPropertiesParms))]
    [KnownType(typeof(ROModelEligibilityProperties))]
    [KnownType(typeof(ROModelValue))]
    [KnownType(typeof(ROModelValuesProperties))]
    [KnownType(typeof(ROModelStockModifierProperties))]
    [KnownType(typeof(ROModelSalesModifierProperties))]
    [KnownType(typeof(ROModelOverrideLowLevelsProperties))]
    [KnownType(typeof(ROModelFWOSModifierProperties))]
    [KnownType(typeof(ROModelFWOSMaxProperties))]

    [KnownType(typeof(ROSizeConstraintValues))]
    [KnownType(typeof(ROSizeConstraintSize))]
    [KnownType(typeof(ROSizeConstraintDimension))]
    [KnownType(typeof(ROSizeConstraintColor))]
    [KnownType(typeof(ROSizeConstraints))]
    [KnownType(typeof(ROSizeConstraintAttributeSet))]
    [KnownType(typeof(ROSizeConstraintDimensionSizes))]
    [KnownType(typeof(ROModelSizeConstraintProperties))]

    [KnownType(typeof(ROModelSizeAlternateProperties))]
    [KnownType(typeof(ROSizeAlternatePrimarySet))]
    [KnownType(typeof(ROSizeAlternateSecondarySets))]
    [KnownType(typeof(ROSizeAlternateSecondaryValues))]
    [KnownType(typeof(ROModelSizeGroupProperties))]

    [KnownType(typeof(ROSize))]
    [KnownType(typeof(ROSizeCurveEntry))]
    [KnownType(typeof(ROSizeCurve))]
    [KnownType(typeof(ROSizeCurveStore))]
    [KnownType(typeof(ROSizeCurveAttributeSet))]
    [KnownType(typeof(ROModelSizeCurveProperties))]

    [KnownType(typeof(RONodePropertyKeyParms))]
    [KnownType(typeof(RONodePropertyAttributeKeyParms))]
    [KnownType(typeof(RONodePropertyAttributeDateKeyParms))]

    [KnownType(typeof(RONodeProperties))]
    [KnownType(typeof(RONodePropertiesProfile))]

    [KnownType(typeof(ROHierarchyPropertyKeyParms))]
    [KnownType(typeof(ROHierarchyPropertiesProfile))]
    [KnownType(typeof(ROHierarchyLevel))]
    [KnownType(typeof(ROHierarchyPropertiesParms))]

    [KnownType(typeof(RONodePropertiesEligibilityValues))]
    [KnownType(typeof(RONodePropertiesEligibilityStore))]
    [KnownType(typeof(RONodePropertiesEligibilityAttributeSet))]
    [KnownType(typeof(RONodePropertiesEligibility))]

    [KnownType(typeof(RONodePropertiesVelocityGrades))]
    [KnownType(typeof(RONodePropertiesVelocityGrade))]
    [KnownType(typeof(RONodePropertiesStoreGrades))]
    [KnownType(typeof(RONodePropertiesStoreGrade))]

    [KnownType(typeof(RONodePropertiesStockMinMax))]
    [KnownType(typeof(RONodePropertiesStockMinMaxAttributeSet))]
    [KnownType(typeof(RONodePropertiesStockMinMaxStoreGrade))]
    [KnownType(typeof(RONodePropertiesStockMinMaxStoreGradeEntry))]

    [KnownType(typeof(RONodePropertiesStoreCapacity))]
    [KnownType(typeof(RONodePropertiesStoreCapacityAttributeSet))]
    [KnownType(typeof(RONodePropertiesStoreCapacityStore))]
    [KnownType(typeof(RONodePropertiesStoreCapacityValues))]

    [KnownType(typeof(RONodePropertiesDailyPercentagesValues))]
    [KnownType(typeof(RONodePropertiesDailyPercentagesStore))]
    [KnownType(typeof(RONodePropertiesDailyPercentagesAttributeSet))]
    [KnownType(typeof(RONodePropertiesDailyPercentages))]

    [KnownType(typeof(RONodePropertiesPurgeCriteriaSettings))]
    [KnownType(typeof(RONodePropertiesPurgeCriteria))]

    [KnownType(typeof(RONodePropertiesSizeCurveCriteria))]
    [KnownType(typeof(RONodePropertiesSizeCurvesSimilarStoreValues))]
    [KnownType(typeof(RONodePropertiesSizeCurvesSimilarStoresAttributeSet))]
    [KnownType(typeof(RONodePropertiesSizeCurvesSimilarStore))]
    [KnownType(typeof(RONodePropertiesSizeCurves))]

    [KnownType(typeof(RONodePropertiesChainSetPercentWeek))]
    [KnownType(typeof(RONodePropertiesChainSetPercentAttributeSet))]
    [KnownType(typeof(RONodePropertiesChainSetPercent))]

    [KnownType(typeof(RONodePropertiesVSWValues))]
    [KnownType(typeof(RONodePropertiesVSWStore))]
    [KnownType(typeof(RONodePropertiesVSWAttributeSet))]
    [KnownType(typeof(RONodePropertiesVSW))]

    [KnownType(typeof(RONodePropertiesCharacteristicsValue))]
    [KnownType(typeof(RONodePropertiesCharacteristics))]

    [KnownType(typeof(RONodePropertiesParms))]

    [KnownType(typeof(ROUserInformation))]
    [KnownType(typeof(RUserInformationParms))]

    [KnownType(typeof(ROViewDetails))]
    [KnownType(typeof(ROPlanningViewDetailsParms))]
    [KnownType(typeof(ROPlanningViewDetails))]
    [KnownType(typeof(ROAllocationReviewViewDetailsParms))]
    [KnownType(typeof(ROAllocationReviewViewDetails))]
    [KnownType(typeof(ROViewFormatParms))]

    [KnownType(typeof(ROMessageResponseParms))]

    [KnownType(typeof(ROAllocationWorklistEntry))]

    [KnownType(typeof(ROAllocationWorklistViewDetails))]
    [KnownType(typeof(ROAllocationWorklistViewDetailsParms))]

    [KnownType(typeof(ROTaskListPropertiesParms))]
    [KnownType(typeof(ROTaskParms))]
    [KnownType(typeof(ROTaskPropertiesParms))]
    [KnownType(typeof(ROTaskListProperties))]
    [KnownType(typeof(ROTaskProperties))]
    [KnownType(typeof(ROTaskAllocateMerchandiseWorkflowMethod))]
    [KnownType(typeof(ROTaskAllocateMerchandise))]
    [KnownType(typeof(ROTaskAllocate))]
    [KnownType(typeof(ROTaskHierarchyLoad))]
    [KnownType(typeof(ROTaskStoreLoad))]
    [KnownType(typeof(ROTaskHistoryPlanLoad))]
    [KnownType(typeof(ROTaskColorCodeLoad))]
    [KnownType(typeof(ROTaskSizeCodeLoad))]
    [KnownType(typeof(ROTaskHeaderLoad))]
    [KnownType(typeof(ROTaskForecasting))]
    [KnownType(typeof(ROTaskRollupMerchandise))]
    [KnownType(typeof(ROTaskRollup))]
    [KnownType(typeof(ROTaskRelieveIntransit))]
    [KnownType(typeof(ROTaskPurge))]
    [KnownType(typeof(ROTaskExternalProgram))]
    [KnownType(typeof(ROTaskSizeCurveLoad))]
    [KnownType(typeof(ROTaskSQLScript))]
    [KnownType(typeof(ROTaskSizeConstraintsLoad))]
    [KnownType(typeof(ROTaskSizeCurveMethod))]
    [KnownType(typeof(ROTaskSizeCurves))]
    [KnownType(typeof(ROTaskSizeDayToWeekSummary))]
    [KnownType(typeof(ROTaskBuildPackCriteriaLoad))]
    [KnownType(typeof(ROTaskChainSetPercentCriteriaLoad))]
    [KnownType(typeof(ROTaskPushToBackStockLoad))]
    [KnownType(typeof(ROTaskDailyPercentagesCriteriaLoad))]
    [KnownType(typeof(ROTaskStoreEligibilityCriteriaLoad))]
    [KnownType(typeof(ROTaskVSWCriteriaLoad))]
    [KnownType(typeof(ROTaskHeaderReconcile))]
    [KnownType(typeof(ROTaskBatchCompute))]
    [KnownType(typeof(ROTaskChainForecasting))]


    public class ROParms
    {
        [DataMember(IsRequired = true)]
        protected string _sROUserID;

        [DataMember(IsRequired = true)]
        protected string _sROSessionID;

        [DataMember(IsRequired = true)]
        protected eROClass _ROClass;

        [DataMember(IsRequired = true)]
        protected eRORequest _RORequest;

        [DataMember(IsRequired = true)]
        protected long _ROInstanceID;

        public static eROClass GetClassForScreen(eScreenID eScreenID)
        {
            switch (eScreenID)
            {
                case eScreenID.ChainPlan:
                    return eROClass.ROChainSingleLevel;
                default:
                    throw new NotImplementedException("Need to add code for this screen: " + eScreenID.ToString());
            }
        }

        public ROParms()
        {
        }

        public ROParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID)
        {
            _sROUserID = sROUserID;
            _sROSessionID = sROSessionID;
            _ROClass = ROClass;
            _RORequest = RORequest;
            _ROInstanceID = ROInstanceID;
        }

        public string ROUserID { get { return _sROUserID; } }

        public string ROSessionID { get { return _sROSessionID; } }

        public eROClass ROClass { get { return _ROClass; } }

        public eRORequest RORequest { get { return _RORequest; } }

        public long ROInstanceID { get { return _ROInstanceID; } }
    }
}
