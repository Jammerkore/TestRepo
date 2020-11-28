using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using System.Collections;
using System.Globalization;
using MIDRetail.Business.Allocation;

namespace Logility.ROWeb
{
    public partial class ROAllocation : ROWebFunction
    {

        #region "Get Allocation Size Review Views"
        internal ROOut GetAllocationSizeReviewViews()
        {
            try
            {
                List<KeyValuePair<int, string>> defaultViewDetails = GetDefaultViewDetails();

                return new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, defaultViewDetails);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal List<KeyValuePair<int, string>> GetDefaultViewDetails()
        {
            DataTable dtViews = new DataTable();

            try
            {
                FunctionSecurityProfile globalViewSecurity;
                FunctionSecurityProfile userViewSecurity;
                eLayoutID layoutID;
                ArrayList userRIDList = new ArrayList();
                GridViewData gridViewData = new GridViewData();
                UserGridView userGridView = new UserGridView();

                globalViewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsGlobalSizeReview);
                userViewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsUserSizeReview);
                layoutID = eLayoutID.sizeReviewGrid;

                if (globalViewSecurity.AllowView)
                {
                    userRIDList.Add(Include.GlobalUserRID);
                }
                if (userViewSecurity.AllowView)
                {
                    userRIDList.Add(SAB.ClientServerSession.UserRID);
                }

                if (userRIDList.Count > 0)
                {
                    _bindingView = true;
                    dtViews = gridViewData.GridView_Read((int)layoutID, userRIDList, true);

                    dtViews.Rows.Add(new object[] { Include.NoRID, SAB.ClientServerSession.UserRID, layoutID, string.Empty });
                    dtViews.PrimaryKey = new DataColumn[] { dtViews.Columns["VIEW_RID"] };
                    _bindingView = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ApplicationUtilities.DataTableToKeyValues(dtViews, "VIEW_RID", "VIEW_ID");
        }
        #endregion

        #region Get Allocation Size Review
        internal ROOut GetAllocationSizeReviewData(ROAllocationReviewOptionsParms reviewOptionsParms)
        {
            try
            {
                int iFirstRowItem = 0, iLastRowItem = 0, iTotalRowItems = 0, iNumberOfRows = 0;
                int iFirstColItem = 0, iLastColItem = 0, iTotalColItems = 0, iNumberOfColumns = 0;

                ROData roData = BuildAllocationReviewData(reviewOptionsParms, eAllocationSelectionViewType.Size, false);
                if (string.IsNullOrEmpty(_sROMessage))
                {
                    _returnCode = eROReturnCode.Successful;
                }
                return new ROGridData(_returnCode, _sROMessage, ROInstanceID, roData, iFirstRowItem, iLastRowItem, iTotalRowItems, iNumberOfRows,
                     iFirstColItem, iLastColItem, iTotalColItems, iNumberOfColumns);


            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "GetAllocationSizeReviewData failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }

        #endregion

        #region "Method to Update SizeReview Change Data"

        private ROOut UpdateSizeReview(ROGridChangesParms gridChanges)
        {
            // adjust row index for removed % to total rows
            if (_removedPctTotalRows)
            {
                foreach (ROGridCellChange cellChange in gridChanges.CellChanges)
                {
                    cellChange.RowIndex = cellChange.RowIndex * 2;
                }
            }

            return UpdateAllocationReview(gridChanges);
        }

        #endregion

        #region "Process SizeReview Action"
        private ROOut ProcessAllocationSizeReviewAction(ROAllocationReviewOptionsParms reviewOptionsParms)
        {

            try
            {
                int iFirstRowItem = 0, iLastRowItem = 0, iTotalRowItems = 0, iNumberOfRows = 0;
                int iFirstColItem = 0, iLastColItem = 0, iTotalColItems = 0, iNumberOfColumns = 0;

                ROData ROData = ProcessAllocationSizeReviewActionRequest(reviewOptionsParms);

                return new ROGridData(_returnCode, _sROMessage, ROInstanceID, ROData, iFirstRowItem, iLastRowItem, iTotalRowItems, iNumberOfRows,
                    iFirstColItem, iLastColItem, iTotalColItems, iNumberOfColumns);

            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "ProcessAllocationSizeReviewAction Failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }

        }

        internal ROData ProcessAllocationSizeReviewActionRequest(ROAllocationReviewOptionsParms reviewOptionsParms)
        {

            bool aReviewFlag, aUseSystemTolerancePercent;
            double aTolerancePercent;
            int aStoreFilter, aWorkFlowStepKey;
            try
            {
                int action = Convert.ToInt32(reviewOptionsParms.AllocationActionType, CultureInfo.CurrentUICulture);

                if (action != -1)
                {
                    if (_applicationSessionTransaction.AssortmentViewSelectionCriteria == null)
                    {
                        if (!IsEligibleToProcessAllocationActionType((eAllocationActionType)action))
                        {
                            _returnCode = eROReturnCode.Failure;
                            _sROMessage = "Allocation Action is not possible to apply.";
                            return new ROData();

                        }
                    }
                    else
                    {
                        if (!IsEligibleToProcessAssortmentActionType((eAllocationActionType)action))
                        {
                            _returnCode = eROReturnCode.Failure;
                            _sROMessage = "Assortment Action is not possible to apply.";
                            return new ROData();
                        }
                    }


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

                return FormatGridsWithApplyView(_wafers, reviewOptionsParms, eAllocationSelectionViewType.Size, true, reviewOptionsParms.View.Key);
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

        #region "Data transport for Allocation Review Save - Size #RO-3026"

        private ROOut SaveAllocationSizeReview()
        {
            _applicationSessionTransaction.DataState = eDataState.Updatable;

            bool isAllocSizeReviewSaved = SaveAllocationSizeChanges();

            return new ROBoolOut(eROReturnCode.Successful, null, ROInstanceID, isAllocSizeReviewSaved);
        }

        internal bool SaveAllocationSizeChanges()
        {
            bool isHeaderSaved = true;

            try
            {
                _headerList = (AllocationHeaderProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AllocationHeader);
                FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewSize);


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

                SaveSizeUserGridView();

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
        
        private void SaveSizeUserGridView()
        {

            _layoutID = eLayoutID.sizeReviewGrid;

            _userGridView = new UserGridView();

            _viewRID = _userGridView.UserGridView_Read(SAB.ClientServerSession.UserRID, _layoutID);

            if (_userGridView != null && _viewRID != 0)
            {
                _userGridView.UserGridView_Update(SAB.ClientServerSession.UserRID, _layoutID, _viewRID);
            }
        }
        #endregion
    }
}
