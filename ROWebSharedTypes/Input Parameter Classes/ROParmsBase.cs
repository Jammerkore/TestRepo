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
    [KnownType(typeof(RONodePropertyEligibilityKeyParms))]

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
    [KnownType(typeof(RONodePropertiesStoreGrades))]
    [KnownType(typeof(RONodePropertiesStockMinMax))]
    [KnownType(typeof(RONodePropertiesStoreCapacity))]
    [KnownType(typeof(RONodePropertiesDailyPercentages))]
    [KnownType(typeof(RONodePropertiesPurgeCriteria))]
    [KnownType(typeof(RONodePropertiesSizeCurves))]
    [KnownType(typeof(RONodePropertiesChainSetPercent))]
    [KnownType(typeof(RONodePropertiesVSW))]
    [KnownType(typeof(RONodePropertiesParms))]

    [KnownType(typeof(ROUserInformation))]
    [KnownType(typeof(RUserInformationParms))]

    public abstract class ROParms
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
