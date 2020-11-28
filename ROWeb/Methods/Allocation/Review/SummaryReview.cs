using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using System;
using System.Collections;
using System.Data;
using System.Globalization;

namespace Logility.ROWeb
{
    public partial class ROAllocation : ROWebFunction
    {
        #region Get Allocation Summary Review
        internal ROOut GetAllocationSummaryReviewData(ROAllocationReviewOptionsParms reviewOptionsParms)
        {
            try
            {
                int iFirstRowItem = 0, iLastRowItem = 0, iTotalRowItems = 0, iNumberOfRows = 0;
                int iFirstColItem = 0, iLastColItem = 0, iTotalColItems = 0, iNumberOfColumns = 0;

                ROData roData = BuildAllocationReviewData(reviewOptionsParms, eAllocationSelectionViewType.Summary, false);
                if (string.IsNullOrEmpty(_sROMessage))
                {
                    _returnCode = eROReturnCode.Successful;
                }
                return new ROGridData(_returnCode, _sROMessage, ROInstanceID, roData, iFirstRowItem, iLastRowItem, iTotalRowItems, iNumberOfRows,
                     iFirstColItem, iLastColItem, iTotalColItems, iNumberOfColumns);

            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "GetAllocationSummaryReviewData failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }
 
        #endregion

        #region "Method to Update Summary Review Change Data"
        private ROOut UpdateSummaryReview(ROGridChangesParms gridChanges)
        {
            return UpdateAllocationReview(gridChanges);
        }

        #endregion

        #region Process Allocation Summary Review Action
        internal ROOut ProcessAllocationSummaryReviewAction(ROAllocationReviewOptionsParms reviewOptionsParms)
        {
            try
            {
                int iFirstRowItem = 0, iLastRowItem = 0, iTotalRowItems = 0, iNumberOfRows = 0;
                int iFirstColItem = 0, iLastColItem = 0, iTotalColItems = 0, iNumberOfColumns = 0;

                ROData ROData = ProcessAllocationSummaryReviewActionRequest(reviewOptionsParms);

                return new ROGridData(_returnCode, _sROMessage, ROInstanceID, ROData, iFirstRowItem, iLastRowItem, iTotalRowItems, iNumberOfRows,
                    iFirstColItem, iLastColItem, iTotalColItems, iNumberOfColumns);

            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "ProcessAllocationStyleReviewAction failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }

        private ROData ProcessAllocationSummaryReviewActionRequest(ROAllocationReviewOptionsParms reviewOptionsParms)
        {
            bool aReviewFlag, aUseSystemTolerancePercent;
            double aTolerancePercent;
            int aStoreFilter, aWorkFlowStepKey;
            try
            {
                // see if an ACTION has been selected
                int action = Convert.ToInt32(reviewOptionsParms.AllocationActionType, CultureInfo.CurrentUICulture);
                int viewRID = _userGridView.UserGridView_Read(SAB.ClientServerSession.UserRID, eLayoutID.sizeReviewGrid);

                if (action != -1)
                {
                    if (_applicationSessionTransaction.AssortmentViewSelectionCriteria == null)
                    {//This is to execute Allocation part
                        if (!IsEligibleToProcessAllocationActionType((eAllocationActionType)action))
                        {
                            _returnCode = eROReturnCode.Failure;
                            _sROMessage = "Allocation Action is not possible to apply.";
                            return new ROData();

                        }
                    }
                    else
                    {//This is to exeucte Assortment Part
                        if (!IsEligibleToProcessAssortmentActionType((eAllocationActionType)action))
                        {
                            _returnCode = eROReturnCode.Failure;
                            _sROMessage = "Assortment Action is not possible to apply.";
                            return new ROData();
                        }
                    }

                    // set ACTION in transaction
                    ApplicationBaseAction aMethod = _applicationSessionTransaction.CreateNewMethodAction((eMethodType)action);
                    GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);
                    aReviewFlag = false;
                    aUseSystemTolerancePercent = true;
                    aTolerancePercent = Include.DefaultBalanceTolerancePercent;

                    if (_applicationSessionTransaction.AllocationFilterID != Include.NoRID)
                    {
                        aStoreFilter = _applicationSessionTransaction.AllocationFilterID;
                    }
                    else
                    {
                        aStoreFilter = Include.AllStoreFilterRID;
                    }

                    aWorkFlowStepKey = -1;
                    AllocationWorkFlowStep aAllocationWorkFlowStep
                        = new AllocationWorkFlowStep(aMethod, aComponent, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
                    _applicationSessionTransaction.DoAllocationAction(aAllocationWorkFlowStep);

                    eAllocationActionStatus actionStatus = _applicationSessionTransaction.AllocationActionAllHeaderStatus;
                    _sROMessage = MIDText.GetTextOnly((int)actionStatus);
                    _returnCode = eROReturnCode.Successful;
                    if (actionStatus == eAllocationActionStatus.HeaderEnqueueFailed
                        || actionStatus == eAllocationActionStatus.NoHeaderResourceLocks)
                    {
                        _returnCode = eROReturnCode.Failure;
                        _sROMessage = "Header Enqueue Failed.";
                        return new ROData();
                    }
                    else
                    {
                        if (!_applicationSessionTransaction.UseAssortmentSelectedHeaders)
                        {
                            _applicationSessionTransaction.UpdateAllocationViewSelectionHeaders();
                            _applicationSessionTransaction.NewCriteriaHeaderList();
                            _allocationHeaderProfileList = (AllocationHeaderProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AllocationHeader);
                        }

                        UpdateAllocationWorkspace();
                        if (action == (int)eAllocationActionType.BackoutAllocation)
                        {
                            _applicationSessionTransaction.RebuildWafers();
                        }
                    }
                }

                if (reviewOptionsParms.AllocationActionType == eAllocationActionType.BackoutAllocation)
                {
                    _applicationSessionTransaction.RebuildWafers();
                }
                _wafers = _applicationSessionTransaction.AllocationWafers;
                return FormatGridsWithApplyView(_wafers, reviewOptionsParms, eAllocationSelectionViewType.Summary);
            }
            catch (MIDException MIDexc)
            {
                _returnCode = eROReturnCode.Failure;
                _sROMessage = MIDexc.ToString();
                throw;
            }

            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

        #region "#RO3027 - Data transport for Allocation Review Save - Summary"

        private ROOut SaveAllocationSummaryReview()
        {
            _applicationSessionTransaction.DataState = eDataState.Updatable;

            bool isAllocSummaryReviewSaved = SaveAllocationSummaryChanges();

            return new ROBoolOut(eROReturnCode.Successful, null, ROInstanceID, isAllocSummaryReviewSaved);
        }

        internal bool SaveAllocationSummaryChanges()
        {
            bool isHeaderSaved = true;

            try
            {
                _headerList = (AllocationHeaderProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AllocationHeader);
                FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewSummary);


                if (_applicationSessionTransaction.DataState == eDataState.Updatable
                  && FunctionSecurity.AllowUpdate
                  && _headerList != null
                  && _applicationSessionTransaction.AreHeadersEnqueued(_headerList))
                {
                    isHeaderSaved = _applicationSessionTransaction.SaveHeaders();

                    if (!isHeaderSaved)
                    {
                        string message = MIDText.GetTextOnly((int)eMIDTextCode.msg_al_HeaderUpdateFailed);
                    }

                }

                _applicationSessionTransaction.SaveAllocationDefaults();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {

            }

            return isHeaderSaved;
        }
        
        #endregion
    }
}
