using System.Runtime.Serialization;
using MIDRetail.DataCommon;


namespace Logility.ROWebSharedTypes
{
    [DataContract]

    [KnownType(typeof(eROClass))]
    [KnownType(typeof(eStorePlanSelectedGroupBy))]
    [KnownType(typeof(eLowLevelsType))]
    [KnownType(typeof(eDisplayTimeBy))]
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
    [KnownType(typeof(eExternalEligibilityProductIdentifier))]
    [KnownType(typeof(eExternalEligibilityChannelIdentifier))]

    [KnownType(typeof(ROMerchandiseListEntry))]

    [KnownType(typeof(ROActiveClass))]
    [KnownType(typeof(RONoDataOut))]
    [KnownType(typeof(RODataTableOut))]
    [KnownType(typeof(RODataSetOut))]
    [KnownType(typeof(ROGridData))]
    [KnownType(typeof(ROLongOut))]
    [KnownType(typeof(ROIntOut))]
    [KnownType(typeof(ROCubeMetadata))]
    [KnownType(typeof(ROCalendarTimePeriodModels))]
    [KnownType(typeof(ROCalendarDate))]
    [KnownType(typeof(ROListOut))]
    [KnownType(typeof(ROIntStringPairListOut))]
    [KnownType(typeof(ROCalendarDateInfo))]
    [KnownType(typeof(ROActiveUserOut))]

    [KnownType(typeof(ROColorGroup))]
    [KnownType(typeof(ROColor))]
    [KnownType(typeof(ROMessageRequest))]
    [KnownType(typeof(ROMessageDetails))]
    [KnownType(typeof(ROMessageDetailsCreatePlaceholders))]

    [KnownType(typeof(ROIListOut))]
    [KnownType(typeof(ROAllocationHeaderSummary))]
    [KnownType(typeof(ROAllocationHeaderCharacteristic))]

    [KnownType(typeof(ROAllocationWorklistValues))]
    [KnownType(typeof(ROAllocationWorklistValuesOut))]

    [KnownType(typeof(ROAssortmentView))]
    [KnownType(typeof(ROAssortmentProperties))]
    [KnownType(typeof(AssortmentPropertiesBasis))]
    [KnownType(typeof(AssortmentPropertiesStoreGrades))]

    [KnownType(typeof(ROExplorerView))]
    [KnownType(typeof(ROSubFolder))]
    [KnownType(typeof(ROFilterNode))]
    [KnownType(typeof(ROListCustomOut<ROExplorerView>))]

    [KnownType(typeof(ROHeaderBulkColorDetails))]
    [KnownType(typeof(BulkColorSize))]

    [KnownType(typeof(ROHeaderPackProfile))]
    [KnownType(typeof(HeaderPackColor))]
    [KnownType(typeof(HeaderPackColorSize))]
    [KnownType(typeof(ROCharacteristic))]

    [KnownType(typeof(ROOTSWorkflowStepOut))]

    [KnownType(typeof(ROAllocationWorklistOut))]
    [KnownType(typeof(ROAllocationWorklistEntry))]
    [KnownType(typeof(StoreGradeProfile))]
    [KnownType(typeof(SecurityProfile))]
    [KnownType(typeof(VersionSecurityProfile))]
    [KnownType(typeof(VersionProfile))]
    [KnownType(typeof(Profile))]
    [KnownType (typeof(ROTreeNodeOut))]
    [KnownType(typeof(ROTreeNodeData))]
    [KnownType(typeof(ROTreeNodeDataHierarchy))]

    [KnownType(typeof(ROModelProfile))]
    [KnownType(typeof(SizeCodeProfile))]
    [KnownType(typeof(ROSizeGroupProfileOut))]

    [KnownType(typeof(ROAllocationReviewSelectionProperties))]
    [KnownType(typeof(ROAllocationReviewSelectionGridData))]
    [KnownType(typeof(ROAllocationReviewSelectionBasis))]
    [KnownType(typeof(ROAllocationReviewSelectionPropertiesOut))]

    [KnownType(typeof(ROMethodGeneralAllocationProperties))]
    [KnownType(typeof(ROMethodRuleProperties))]
    [KnownType(typeof(ROMethodFillSizeHolesProperties))]
    [KnownType(typeof(ROMethodSizeNeedProperties))]
    [KnownType(typeof(ROMethodBasisSizeProperties))]
    [KnownType(typeof(ROSizeConstraintProperties))]
    [KnownType(typeof(ROSizeCurveProperties))]
    [KnownType(typeof(ROMethodSizeRuleAttributeSet))]
    [KnownType(typeof(ROMethodSizeRuleProperties))]
    [KnownType(typeof(ROAssortmentPropertiesOut))]
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
    [KnownType(typeof(ROBoolOut))]
    [KnownType(typeof(ROWorklistViewOut))]

    [KnownType(typeof(ROWorkflow))]
    [KnownType(typeof(ROPlanningWorkflowStep))]
    [KnownType(typeof(ROAllocationWorkflowStep))]
    [KnownType(typeof(ROWorkflowStep))]
    [KnownType(typeof(ROWorkflowOut))]
    [KnownType(typeof(ROSecurityProfile))]
    [KnownType(typeof(ROFunctionSecurityOut))]
    [KnownType(typeof(eSecurityActions))]
    [KnownType(typeof(eSecurityFunctions))]
    [KnownType(typeof(ROBaseProperties))]
    [KnownType(typeof(ROMethodProperties))]
    [KnownType(typeof(ROMethodPropertiesOut))]
    [KnownType(typeof(ROMethodOverrideModelListOut))]
    [KnownType(typeof(ROAboutProperties))]
    [KnownType(typeof(ROAboutOut))]

    [KnownType(typeof(ROBasisProfile))]
    [KnownType(typeof(ROBasisDetailProfile))]
    [KnownType(typeof(ROPlanningReviewSelectionProperties))]
    [KnownType(typeof(ROPlanningReviewSelectionPropertiesOut))]

    [KnownType(typeof(LowLevelCombo))]
    [KnownType(typeof(ROLowLevelsOut))]

    [KnownType(typeof(ROInUseEntry))]
    [KnownType(typeof(ROInUse))]
    [KnownType(typeof(ROInUseOut))]

    [KnownType(typeof(ROAssortmentActionsOut))]
    [KnownType(typeof(ROAssortmentReviewOptionsParms))]
    [KnownType(typeof(eApplyTrendOptions))]
    [KnownType(typeof(ROPlanningForecastMethodProperties))]
    [KnownType(typeof(eGroupLevelFunctionType))]
    [KnownType(typeof(eGroupLevelSmoothBy))]
    [KnownType(typeof(ROOverrideLowLevel))]
    [KnownType(typeof(ROPlanningGlobalLockUnlockProperties))]
    [KnownType(typeof(ROLevelInformation))]
    [KnownType(typeof(eROLevelsType))]


    [KnownType(typeof(ROMethodCopyForecastProperties))]
    [KnownType(typeof(ROMethodCopyChainForecastProperties))]
    [KnownType(typeof(ROMethodCopyStoreForecastProperties))]
    [KnownType(typeof(ROMethodCopyStoreForecastAttributeSetProperties))]
    [KnownType(typeof(ROMethodRollupProperties))]
    [KnownType(typeof(ROMethodRollupOptionsBasis))]
    [KnownType(typeof(ROSelectedField))]
    [KnownType(typeof(ROMethodForecastSpreadProperties))]
    [KnownType(typeof(eTrendCapID))]
    [KnownType(typeof(ROPlanningForecastExportProperties))]
    [KnownType(typeof(ROVariable))]
    [KnownType(typeof(ROVariableGrouping))]
    [KnownType(typeof(ROVariableGroupings))]
    [KnownType(typeof(ROPlanningForecastMethodAttributeSetProperties))]
    [KnownType(typeof(ROForecastingBasisDetailsProfile))]
    [KnownType(typeof(ROMethodMatrixBalanceProperties))]
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
    [KnownType(typeof(ROAttributeSetAllocationStoreGrade))]
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

    [KnownType(typeof(ROUserInformation))]
    [KnownType(typeof(ROUserInformationOut))]

    [KnownType(typeof(ROCharacteristicsProperties))]
    [KnownType(typeof(ROCharacteristicDefinition))]
    [KnownType(typeof(ROCharacteristicValue))]
    [KnownType(typeof(ROCharacteristicsOut))]

    [KnownType(typeof(ROModelProperties))]
    [KnownType(typeof(ROModelEligibilityProperties))]
    [KnownType(typeof(ROModelValue))]
    [KnownType(typeof(ROModelValuesProperties))]
    [KnownType(typeof(ROModelStockModifierProperties))]
    [KnownType(typeof(ROModelSalesModifierProperties))]
    [KnownType(typeof(ROModelOverrideLowLevelsProperties))]
    [KnownType(typeof(ROModelOverrideLowLevel))]
    [KnownType(typeof(ROModelFWOSModifierProperties))]
    [KnownType(typeof(ROModelFWOSMaxProperties))]

    [KnownType(typeof(ROSizeConstraintValues))]
    [KnownType(typeof(ROSizeConstraintSize))]
    [KnownType(typeof(ROSizeConstraintDimension))]
    [KnownType(typeof(ROSizeConstraintColor))]
    [KnownType(typeof(ROSizeConstraints))]
    [KnownType(typeof(ROSizeConstraintAttributeSet))]
    [KnownType(typeof(ROSizeDimension))]
    [KnownType(typeof(ROModelSizeConstraintProperties))]

    [KnownType(typeof(ROModelSizeAlternateProperties))]
    [KnownType(typeof(ROSizeAlternatePrimarySize))]
    [KnownType(typeof(ROSizeAlternateAlternateSize))]
    [KnownType(typeof(ROModelSizeGroupProperties))]

    [KnownType(typeof(ROSize))]
    [KnownType(typeof(ROSizeCurveEntry))]
    [KnownType(typeof(ROSizeCurve))]
    [KnownType(typeof(ROSizeCurveStore))]
    [KnownType(typeof(ROSizeCurveAttributeSet))]
    [KnownType(typeof(ROModelSizeCurveProperties))]

    [KnownType(typeof(ROModelPropertiesOut))]

    [KnownType(typeof(ROHierarchyPropertiesOut))]
    [KnownType(typeof(ROHierarchyPropertiesProfile))]
    [KnownType(typeof(ROHierarchyLevel))]

    [KnownType(typeof(RONodePropertiesOut))]
    [KnownType(typeof(RONodeProperties))]
    [KnownType(typeof(RONodePropertiesProfile))]

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

    [KnownType(typeof(ROViewDetails))]
	[KnownType(typeof(ROPlanningViewDetails))]
	[KnownType(typeof(ROPlanningViewDetailsOut))]
    [KnownType(typeof(ROAllocationReviewViewDetails))]
    [KnownType(typeof(ROAllocationReviewViewDetailsOut))]

    [KnownType(typeof(ROStoreProfile))]
    [KnownType(typeof(ROStoreProfileOut))]
    [KnownType(typeof(ROAllStoresProfilesOut))]

    [KnownType(typeof(ROTaskListProperties))]
    [KnownType(typeof(ROTaskListPropertiesOut))]
    [KnownType(typeof(ROTaskProperties))]
    [KnownType(typeof(ROTaskPropertiesOut))]
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
    [KnownType(typeof(ROAuditFilterOption))]
    [KnownType(typeof(ROAuditResult))]
    [KnownType(typeof(ROTaskJobOut))]
    [KnownType(typeof(ROTaskJobs))]

    [KnownType(typeof(ROAllocationEligibilityWorklistStore))]
    [KnownType(typeof(ROAllocationEligibilityWorklistItem))]
    [KnownType(typeof(ROAllocationEligibilityOut))]

    [KnownType(typeof(ROGlobalOptions))]
    [KnownType(typeof(ROGlobalOptionsOut))]

    public class ROOut
    {
        public static readonly long lInvalidInstanceID = -1;

        [DataMember(IsRequired = true)]
        private eROReturnCode _ROReturnCode;

        [DataMember(IsRequired = true)]
        private string _sROMessage;

        [DataMember(IsRequired = true)]
        private long _ROInstanceID;

        public ROOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID)
        {
            if (ROReturnCode != eROReturnCode.Failure
                && MIDEnvironment.isChangedToReadOnly)
            {
                _ROReturnCode = eROReturnCode.ChangedToReadOnly;
            }
            else if (MIDEnvironment.requestFailed)
            {
                _ROReturnCode = eROReturnCode.Failure;
            }
            else
            {
                _ROReturnCode = ROReturnCode;
            }
            if (!string.IsNullOrEmpty(sROMessage))
            {
                _sROMessage = sROMessage;
            }
            else
            {
                _sROMessage = MIDEnvironment.Message;
            }
            _ROInstanceID = ROInstanceID;
        }

        public eROReturnCode ROReturnCode { get { return _ROReturnCode; } }

        public string ROMessage { get { return _sROMessage; } }

        public long ROInstanceID { get { return _ROInstanceID; } }
    }
}
