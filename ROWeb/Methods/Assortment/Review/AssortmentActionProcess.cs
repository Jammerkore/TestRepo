using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logility.ROWeb
{
    public partial class ROAssortment : ROAllocation
    {
        private eAssortmentReviewTabType assortmentTabType = eAssortmentReviewTabType.AssortmentReviewMatrix;

        #region Assortment action process
        private ROOut ProcessAssortmentAction(ROAssortmentActionParms parms)
        {
            int assortmentActionType = (int)(parms.ROAssortmentProperties.AssortmentActionType);
            assortmentTabType = parms.ROAssortmentProperties.AssortmentTabType;

            try
            {

                if (assortmentActionType == Include.NoRID)
                {
                    _sROMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ActionIsRequired);
                    return new RONoDataOut(eROReturnCode.Successful, "Action is required", ROInstanceID); ;
                }

                if (assortmentActionType == (int)eAssortmentActionType.CreatePlaceholders
                    && _asrtCubeGroup.GetHeaderList().Count > 1)
                {
                    _sROMessage = string.Format
                                    (MIDText.GetText(eMIDTextCode.msg_ActionNotAllowed),
                                    MIDText.GetTextOnly(eHeaderType.Assortment.GetHashCode())
                                    );

                    return new ROGridData(eROReturnCode.Failure, "Create Placeholders not valid", ROInstanceID, _asrtMatrixROData, 0, 0, 0, 0, 0, 0, 0, 0);
                }

                ApplicationBaseAction aMethod = _applicationSessionTransaction.CreateNewMethodAction((eMethodType)assortmentActionType);
                GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);

                bool aReviewFlag = false;
                bool aUseSystemTolerancePercent = true;
                double aTolerancePercent = Include.DefaultBalanceTolerancePercent;
                int aStoreFilter = Include.AllStoreFilterRID;
                int aWorkFlowStepKey = -1;
                int selectedHdrCount = 0;
                SelectedHeaderList shl = null;

                if (!IsGroupAllocation)
                {
                    _applicationSessionTransaction.ActionOrigin = eActionOrigin.Assortment;
                }

                if (assortmentActionType != (int)eAssortmentActionType.CreatePlaceholders && assortmentActionType != (int)eAssortmentActionType.SpreadAverage)
                {
                    SaveChanges();
                }

                string actionText = string.Empty;
                string messageSuccessfull = string.Empty;


                eAssortmentActionType enumActionText = (eAssortmentActionType)assortmentActionType;
                actionText = enumActionText.ToString();

                _allocProfileList = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember);

                if (Enum.IsDefined(typeof(eAssortmentActionType), (eAssortmentActionType)assortmentActionType))
                {
                    bool noActionPerformed = false;
                    shl = ProcessAssortmentMethod(ref _applicationSessionTransaction, assortmentActionType, aComponent, aMethod, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey, ref noActionPerformed, parms);

                    selectedHdrCount = shl.Count;

                    messageSuccessfull = actionText + eAllocationActionStatus.ActionCompletedSuccessfully.ToString();


                    if (noActionPerformed)
                    {
                        foreach (SelectedHeaderProfile shp in shl.ArrayList)
                        {
                            _applicationSessionTransaction.SetAllocationActionStatus(shp.Key, eAllocationActionStatus.NoActionPerformed);
                            messageSuccessfull = eAllocationActionStatus.NoActionPerformed.ToString();
                        }
                    }
                }


                if (selectedHdrCount == 0)
                {
                    eAllocationActionStatus actionStatus = eAllocationActionStatus.NoActionPerformed;

                    messageSuccessfull = actionStatus.ToString();

                }
                else if (_applicationSessionTransaction != null)
                {
                    eAllocationActionStatus actionStatus = _applicationSessionTransaction.AllocationActionAllHeaderStatus;

                    messageSuccessfull = actionStatus.ToString();
                }

                if (assortmentTabType == eAssortmentReviewTabType.AssortmentReviewMatrix)
                {
                    return new ROGridData(eROReturnCode.Successful, messageSuccessfull, ROInstanceID, _asrtMatrixROData, 0, 0, 0, 0, 0, 0, 0, 0);
                }
                else
                {
                    return new ROIListOut(eROReturnCode.Successful, null, ROInstanceID, BuildAssortmentContentCharacteristics());
                }

            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "ProcessAssortmentAction failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
            finally
            {

                eAllocationActionStatus actionStatus = _applicationSessionTransaction.AllocationActionAllHeaderStatus;

                if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
                {
                    // No reason to do the UpdateData for CreatePlaceholders
                    if ((eAssortmentActionType)assortmentActionType != eAssortmentActionType.CreatePlaceholders && (eAssortmentActionType)assortmentActionType != eAssortmentActionType.BalanceAssortment)
                    {
                        SelectedHeaderList allocHdrList = GetSelectableHeaderList(assortmentActionType, _applicationSessionTransaction, false, parms);

                        int[] hdrList = new int[allocHdrList.Count];
                        int i = 0;

                        foreach (SelectedHeaderProfile shp in allocHdrList)
                        {
                            hdrList[i] = shp.Key;
                            i++;
                        }
                    }


                    if (IsGroupAllocation && (eAllocationActionType)assortmentActionType == eAllocationActionType.BackoutAllocation && IsProcessAsGroup)
                    {
                        if (IsProcessAsGroup)
                        {
                            AssortProfile.FillAssortGradesFromGroupAllocation(_applicationSessionTransaction);
                            _storeGradeProfileList = _asrtCubeGroup.GetFilteredProfileList(eProfileType.StoreGrade);
                            BuildGrades();
                        }

                        RebuildAssortmentSummary();
                        _asrtCubeGroup.ClearTotalCubes(true);
                    }
                }
            }
        }


        private SelectedHeaderList ProcessAssortmentMethod(ref ApplicationSessionTransaction actionTransaction, int action,
            GeneralComponent aComponent, ApplicationBaseAction aMethod, bool aReviewFlag, bool aUseSystemTolerancePercent,
            double aTolerancePercent, int aStoreFilter, int aWorkFlowStepKey, ref bool noActionPerformed, ROAssortmentActionParms parms)

        {
            SelectedHeaderList selectedHdrList = GetSelectableHeaderList(action, actionTransaction, true, parms);
            try
            {

                noActionPerformed = false;
                if (!OKToProcessAssortment((eAssortmentActionType)action, selectedHdrList))
                {
                    noActionPerformed = true;
                    return selectedHdrList;
                }
                else
                {
                    actionTransaction.AssortmentSelectedHdrList = selectedHdrList;
                }

                AssortmentWorkFlowStep aAssortmentWorkFlowStep = null;
                ProfileList gradeProfList = AssortProfile.GetAssortmentStoreGrades();
                ProfileList sgll = _asrtCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupLevel);
                eAllocationActionStatus actionStatus;
                int _lastStoreGroupLevelValue = 0;
                switch ((eAssortmentActionType)action)
                {
                    case eAssortmentActionType.CreatePlaceholders:

                        //if (ChangePending)
                        //{
                        //    SaveChanges();
                        //}

                        ((AssortmentAction)aMethod).AverageUnitList = ((ROAssortmentActionsCreateplaceHolders)parms.ROAssortmentProperties.AssortmentActionDetails).AverageUnit;
                        ((AssortmentAction)aMethod).ViewGroupBy = eAllocationAssortmentViewGroupBy.StoreGrade;
                        ((AssortmentAction)aMethod).StoreGroupRid = _lastStoreGroupValue;
                        aAssortmentWorkFlowStep
                            = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
                        actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);

                        actionStatus = actionTransaction.AllocationActionAllHeaderStatus;
                        if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
                        {
                            FilterHeaderOptions headerFilterOptions = new FilterHeaderOptions();
                            headerFilterOptions.USE_WORKSPACE_FIELDS = true;
                            headerFilterOptions.filterType = filterTypes.AssortmentFilter;
                            SAB.HeaderServerSession.RebuildHeaderCharacteristicData(SharedRoutines.GetHeaderFilterForAssortmentView(), headerFilterOptions);

                            AllocationProfileList apl = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.Allocation);

                            AllocationHeaderProfileList newHeaderList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
                            if (apl != null)
                            {
                                foreach (object asrtObj in apl)
                                {
                                    AllocationProfile ap = (AllocationProfile)asrtObj;
                                    AllocationHeaderProfile ahp = SAB.HeaderServerSession.GetHeaderData(ap.Key, true, true, true);
                                    if (!newHeaderList.Contains(ap.Key))
                                    {
                                        newHeaderList.Add(ahp);
                                    }
                                }
                            }

                            _applicationSessionTransaction.RemoveMasterProfileList(newHeaderList);
                            _applicationSessionTransaction.SetMasterProfileList(newHeaderList);

                            _headerList = (AllocationHeaderProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AllocationHeader);

                            _asrtCubeGroup.SaveCubeGroup();
                            ReloadGridData(reformatRows: true);
                            ComponentsChanged();
                        }
                        break;

                    case eAssortmentActionType.Redo:
                        ((AssortmentAction)aMethod).StoreGroupRid = _lastStoreGroupValue;
                        aAssortmentWorkFlowStep
                            = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
                        actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);
                        RebuildAssortmentSummary(AssortProfile);
                        ReloadGridData();
                        break;


                    case eAssortmentActionType.SpreadAverage:

                        eIndexToAverageReturnType returnType = ((Logility.ROWebSharedTypes.ROAssortmentActionsSpreadAverage)parms.ROAssortmentProperties.AssortmentActionDetails).SpreadType;

                        ((AssortmentAction)aMethod).IndexToAverageReturnType = ((ROAssortmentActionsSpreadAverage)parms.ROAssortmentProperties.AssortmentActionDetails).SpreadType;

                        //((AssortmentAction)aMethod).CurrSglRid = _lastStoreGroupLevelValue;
                        ((AssortmentAction)aMethod).CurrSglRid = AttributeSet.Key;
                        ((AssortmentAction)aMethod).SpreadAverageOption = ((ROAssortmentActionsSpreadAverage)parms.ROAssortmentProperties.AssortmentActionDetails).SpreadOption;

                        if (returnType == eIndexToAverageReturnType.Total ||
                            returnType == eIndexToAverageReturnType.SetTotal)
                        {
                            double dbReturn = Convert.ToDouble(((ROAssortmentActionsSpreadAverage)parms.ROAssortmentProperties.AssortmentActionDetails).Average);

                            ((AssortmentAction)aMethod).AverageUnits = dbReturn;
                        }
                        else if (returnType == eIndexToAverageReturnType.Grades)
                        {

                            List<ROAssortmentActionsGradeDetails> dtReturn =
                                (((ROAssortmentActionsSpreadAverage)parms.ROAssortmentProperties.AssortmentActionDetails).ActionsGradeDetails);

                            Hashtable gradeAverageUnitsHash = new Hashtable();

                            foreach (var dt in dtReturn)
                            {
                                try
                                {
                                    double gradeValue = double.Parse(dt.GradeAverage.ToString());
                                    gradeAverageUnitsHash.Add(dt.GradeCode.ToString(), gradeValue);
                                }
                                catch
                                {
                                    // The value cannot be parsed into a double,
                                    // skip the grade.
                                }
                            }
                            ((AssortmentAction)aMethod).GradeAverageUnitsHash = gradeAverageUnitsHash;
                        }


                        SetActivateAssortmentOnHeaders(selectedHdrList, true);

                        //==================================
                        // Process Index to Average action
                        //==================================
                        aAssortmentWorkFlowStep
                        = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
                        actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);


                        SetActivateAssortmentOnHeaders(selectedHdrList, false);

                        ReloadGridData();

                        break;

                    case eAssortmentActionType.CancelAssortment:
                        ((AssortmentAction)aMethod).StoreGroupRid = _lastStoreGroupValue;
                        aAssortmentWorkFlowStep
                            = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
                        actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);

                        ReloadGridData();

                        break;

                    case eAssortmentActionType.BalanceAssortment:
                        // Balance Set / Grade
                        foreach (StoreGroupLevelProfile sglp in sgll.ArrayList)
                        {
                            foreach (StoreGradeProfile sgp in gradeProfList.ArrayList)
                            {
                                AssortmentCellReference asrtCellRef9 = (AssortmentCellReference)_asrtCubeGroup.GetCube(new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, 0)).CreateCellReference();
                                asrtCellRef9[eProfileType.StoreGroupLevel] = sglp.Key;
                                asrtCellRef9[eProfileType.StoreGrade] = sgp.Key;
                                asrtCellRef9[eProfileType.AssortmentDetailVariable] = ((AssortmentViewDetailVariables)_asrtCubeGroup.AssortmentComputations.AssortmentDetailVariables).TotalUnits.Key;
                                asrtCellRef9[eProfileType.AssortmentQuantityVariable] = ((AssortmentViewQuantityVariables)_asrtCubeGroup.AssortmentComputations.AssortmentQuantityVariables).Balance.Key;
                                double balCellValue = asrtCellRef9.CurrentCellValue;
                                asrtCellRef9.SetEntryCellValue(0);
                            }

                        }

                        SaveDetailCubeGroup();

                        AllocationProfileList alp = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember);

                        foreach (SelectedHeaderProfile shp in selectedHdrList)
                        {
                            AllocationProfile ap = (AllocationProfile)alp.FindKey(shp.Key);
                            if (ap != null)
                            {

                                if (!ap.WriteHeader())
                                {
                                    EnqueueError(ap);
                                    return selectedHdrList;
                                }
                            }
                        }

                        foreach (SelectedHeaderProfile shp in selectedHdrList)
                        {
                            AllocationProfile ap = (AllocationProfile)alp.FindKey(shp.Key);
                            //ReloadProfileToGrid(ap.Key);
                        }

                        //CloseAndReOpenCubeGroup();

                        //UpdateData(true);

                        //LoadSurroundingPages();
                        ReloadGridData();

                        aAssortmentWorkFlowStep
                            = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
                        actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);

                        break;

                    default:
                        ((AssortmentAction)aMethod).StoreGroupRid = _lastStoreGroupValue;
                        aAssortmentWorkFlowStep
                            = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
                        actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);
                        break;
                }
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "ProcessAssortmentMethod failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
            return selectedHdrList;
        }

        private SelectedHeaderList GetSelectableHeaderList(int actionRid, ApplicationSessionTransaction aTrans,
            bool createTransProfileList, ROAssortmentActionParms parms)
        {
            return GetSelectableHeaderList(actionRid, aTrans, createTransProfileList, parms.ROAssortmentProperties.AssortmentTabType);
        }




        private bool OKToProcessAssortment(eAssortmentActionType aAction, SelectedHeaderList selectedHdrList)

        {
            string errorMessage = string.Empty;
            string errorParm = string.Empty;
            bool okToProcess = true;


            if (selectedHdrList.Count == 0
                && (aAction != eAssortmentActionType.CreatePlaceholders || IsActionSelectable((int)aAction)))

            {
                errorMessage = MIDText.GetText(eMIDTextCode.msg_as_NoHeadersPlaceholdersSelected);
                SAB.ClientServerSession.Audit.Add_Msg(
                    eMIDMessageLevel.Warning,
                    eMIDTextCode.msg_as_NoHeadersPlaceholdersSelected,
                    errorMessage,
                    "Assortment View");

                okToProcess = false;
            }

            if (aAction == eAssortmentActionType.CreatePlaceholders || aAction == eAssortmentActionType.SpreadAverage || aAction == eAssortmentActionType.BalanceAssortment)    // TT#1533-MD - stodd - Balance Asst after switching attribute needs message
            {
                if (_lastStoreGroupValue != _assortmentProfile.AssortmentStoreGroupRID)
                {
                    errorMessage = MIDText.GetText(eMIDTextCode.msg_as_InvalidActionAttributeChanged);
                    SAB.ClientServerSession.Audit.Add_Msg(
                        eMIDMessageLevel.Error,
                        eMIDTextCode.msg_as_InvalidActionAttributeChanged,
                        errorMessage,
                        "Assortment View");

                    okToProcess = false;
                }

            }

            return okToProcess;
        }


        private void RebuildAssortmentSummary(AssortmentProfile Assp)
        {
            RebuildAssortmentSummary(Assp, true);
        }

        private void RebuildAssortmentSummary(AssortmentProfile Assp, bool rereadStoreSummaryData)
        {
            try
            {
                ClearStoresInBlockedStyles();
                if (rereadStoreSummaryData)
                {
                    Assp.AssortmentSummaryProfile.RereadStoreSummaryData();
                }
                Assp.AssortmentSummaryProfile.BuildSummary(_lastStoreGroupValue);

                _asrtCubeGroup.ClearTotalCubes(true);
            }
            catch
            {

                throw;
            }
        }
        private void ClearStoresInBlockedStyles()
        {
            AllocationProfileList apl = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember);

            ArrayList blockedKeyList = new ArrayList();
            IDictionaryEnumerator iEnum = null;
            BlockedListHashKey blockedKey = null;
            bool didUpdateHdr = false;
            foreach (AllocationProfile ap in apl.ArrayList)
            {
                iEnum = _asrtCubeGroup.BlockedList.GetEnumerator();

                while (iEnum.MoveNext())
                {
                    blockedKey = (BlockedListHashKey)iEnum.Key;
                    if (blockedKey.PlaceholderRID == ap.Key)
                    {
                        ProfileList gradeStoreList = _asrtCubeGroup.GetStoresInSetGrade(blockedKey.StrGrpLvlRID, blockedKey.GradeRID);
                        ProfileList storeList = new ProfileList(eProfileType.Store);
                        foreach (StoreProfile sp in gradeStoreList)
                        {
                            if (sp.Key != _applicationSessionTransaction.ReserveStore.RID)
                            {
                                storeList.Add(sp);
                            }
                        }
                        if (ap.SetNotManuallyAllocated(aStoreList: storeList, bOnlySetForZeroValues: true))
                        {
                            didUpdateHdr = true;
                        }
                    }
                }

                if (didUpdateHdr)
                {
                    if (!ap.WriteHeader())
                    {
                        EnqueueError(ap);
                        return;
                    }
                }
            }
        }

        #endregion Assortment action process

    }


}
