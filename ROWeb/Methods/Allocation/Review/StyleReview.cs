using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Logility.ROWeb
{
    public partial class ROAllocation : ROWebFunction
    {
        int _hdrRow = 0; int _compRow = 0;
        bool _columnAdded = false;
        string _lblStore;
        private AllocationHeaderProfileList _headerList;
        private int _viewRID;
        private eLayoutID _layoutID;

        #region AllocationStyleReviewViews
        public ROOut GetAllocationStyleReviewViewsInfo()
        {

            try
            {
                return new ROIListOut(eROReturnCode.Successful, null, ROInstanceID, BuildAllocationReviewViews(eLayoutID.styleReviewGrid));

            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "GetAllocationStyleReviewViewsInfo failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }

        }

        public ROOut GetAllocationStyleReviewVelocityViewsInfo()
        {

            try
            {
                return new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, BuildAllocationStyleReviewViews(layoutID: eLayoutID.velocityStoreDetailGrid));
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "GetAllocationStyleReviewVelocityViewsInfo failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }

        }

        internal List<ROWorklistViewOut> BuildAllocationReviewViews(eLayoutID layoutID)
        {
            DataTable _dtViews;
            GridViewData gridViewData = new GridViewData();
            List<ROWorklistViewOut> views = new List<ROWorklistViewOut>();
            UserRIDList();
            _dtViews = gridViewData.GridView_Read((int)layoutID, _userRIDList, true);
            _dtViews.Rows.Add(new object[] { Include.NoRID, SAB.ClientServerSession.UserRID, (int)layoutID, "  " });

            DataView dv = new DataView(_dtViews);

            foreach (DataRowView rowView in dv)
            {
                int viewRID = Convert.ToInt32(rowView.Row["VIEW_RID"]);
                string viewName = Convert.ToString(rowView.Row["VIEW_ID"]);
                int filterRID = (rowView["WORKSPACE_FILTER_RID"] != DBNull.Value) ? Convert.ToInt32(rowView["WORKSPACE_FILTER_RID"]) : Include.NoRID;
                var roWorklistViewOut = new ROWorklistViewOut(viewRID, viewName, filterRID);
                roWorklistViewOut.GroupBy = (rowView["GROUP_BY"] != DBNull.Value) ? Convert.ToInt32(rowView["GROUP_BY"]) : Include.NoRID;
                roWorklistViewOut.SecondaryGroupBy = (rowView["GROUP_BY_SECONDARY"] != DBNull.Value) ? Convert.ToInt32(rowView["GROUP_BY_SECONDARY"]) : Include.NoRID;
                roWorklistViewOut.IsSequential = (rowView["IS_SEQUENTIAL"] != DBNull.Value) ? Convert.ToBoolean(Convert.ToInt32(rowView["IS_SEQUENTIAL"])) : false;
                views.Add(roWorklistViewOut);
            }

            return views;
        }

        internal List<KeyValuePair<int, string>> BuildAllocationStyleReviewViews(eLayoutID layoutID)
        {

            DataTable dtViews;
            GridViewData gridViewData = new GridViewData();
            UserRIDList();
            dtViews = gridViewData.GridView_Read((int)layoutID, _userRIDList, true);

            dtViews.Rows.Add(new object[] { Include.NoRID, SAB.ClientServerSession.UserRID, (int)eLayoutID.styleReviewGrid, string.Empty });

            dtViews.PrimaryKey = new DataColumn[] { dtViews.Columns["VIEW_RID"] };

            return ApplicationUtilities.DataTableToKeyValues(dtViews, "VIEW_RID", "VIEW_ID");

        }
        #endregion

        #region AllocationStyleReviewVelocityRules
        public ROOut GetAllocationStyleReviewVelocityRules()
        {

            try
            {
                return new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, BuildAllocationStyleReviewVelocityRules());
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "GetAllocationStyleReviewVelocityRules failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }

        }

        internal List<KeyValuePair<int, string>> BuildAllocationStyleReviewVelocityRules()
        {
            DataTable dtRules = MIDText.GetLabels((int)eRuleType.None, (int)eRuleType.None);
            DataTable ruleTable = MIDText.GetLabels((int)eRuleType.WeeksOfSupply, (int)eRuleType.ForwardWeeksOfSupply);

            dtRules.Merge(ruleTable);

            dtRules.PrimaryKey = new DataColumn[] { dtRules.Columns["TEXT_CODE"] };

            return ApplicationUtilities.DataTableToKeyValues(dtRules, "TEXT_CODE", "TEXT_VALUE");

        }
        #endregion

        #region Get Allocation Style Review

        internal ROOut GetAllocationStyleReviewData(ROAllocationReviewOptionsParms reviewOptionsParms)
        {
            try
            {
                int iFirstRowItem = 0, iLastRowItem = 0, iTotalRowItems = 0, iNumberOfRows = 0;
                int iFirstColItem = 0, iLastColItem = 0, iTotalColItems = 0, iNumberOfColumns = 0;

                eAllocationSelectionViewType viewType = eAllocationSelectionViewType.Style;
                if (reviewOptionsParms.IsVelocity)
                {
                    if (_applicationSessionTransaction.VelocityCriteriaExists)
                    {
                        viewType = eAllocationSelectionViewType.Velocity;
                        if (!_wafersBuilt)
                        {
                            // override to the attribute used by velocity
                            if (reviewOptionsParms.StoreAttribute.Key != _applicationSessionTransaction.VelocityStoreGroupRID)
                            {
                                reviewOptionsParms.StoreAttribute = GetName.GetAttributeName(key: _applicationSessionTransaction.VelocityStoreGroupRID);
                                ProfileList storeGroupLevelsList = StoreMgmt.StoreGroup_GetLevelListFilled(_applicationSessionTransaction.VelocityStoreGroupRID);
                                reviewOptionsParms.AttributeSet = GetName.GetAttributeSetName(key: ((Profile)storeGroupLevelsList.ArrayList[0]).Key);
                            }
                        }
                        else if (_applicationSessionTransaction.AllocationStoreAttributeID != reviewOptionsParms.StoreAttribute.Key)
                        {
                            _applicationSessionTransaction.AllocationStoreAttributeID = reviewOptionsParms.StoreAttribute.Key;
                            _applicationSessionTransaction.CurrentStoreGroupProfile = StoreMgmt.StoreGroup_Get(_applicationSessionTransaction.AllocationStoreAttributeID);
                            _applicationSessionTransaction.VelocityStoreGroupRID = reviewOptionsParms.StoreAttribute.Key;
                        }
                        _applicationSessionTransaction.VelocityStyleReviewLastDisplayed = true;
                    }
                    else
                    {
                        viewType = eAllocationSelectionViewType.Style;
                        reviewOptionsParms.ViewType = eAllocationSelectionViewType.Style;
                        ROWebTools.LogMessage(eROMessageLevel.Warning, "Velocity information does not exist. Overridden to Style Review.", ROWebTools.ROUserID, ROWebTools.ROSessionID);
                    }
                }

                ROData roData = BuildAllocationReviewData(reviewOptionsParms, viewType, false);

                if (string.IsNullOrEmpty(_sROMessage))
                {
                    _returnCode = eROReturnCode.Successful;
                }
                else
                {
                    _returnCode = eROReturnCode.ChangedToReadOnly;
                }
                return new ROGridData(_returnCode, _sROMessage, ROInstanceID, roData, iFirstRowItem, iLastRowItem, iTotalRowItems, iNumberOfRows,
                    iFirstColItem, iLastColItem, iTotalColItems, iNumberOfColumns);

            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "GetAllocationStyleReviewData failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }

        #endregion


        #region "Method to Update StyleReview Change Data"

        private ROOut UpdateStyleReview(ROGridChangesParms gridChanges)
        {
            return UpdateAllocationReview(gridChanges);
        }
      
        #endregion

        #region Process Allocation Style Review Action
        private string _sROMessage;
        private eROReturnCode _returnCode;
        internal ROOut ProcessAllocationStyleReviewAction(ROAllocationReviewOptionsParms reviewOptionsParms)
        {
            try
            {
                int iFirstRowItem = 0, iLastRowItem = 0, iTotalRowItems = 0, iNumberOfRows = 0;
                int iFirstColItem = 0, iLastColItem = 0, iTotalColItems = 0, iNumberOfColumns = 0;

                ROData ROData = ProcessAllocationStyleReviewActionRequest(reviewOptionsParms);

                return new ROGridData(_returnCode, _sROMessage, ROInstanceID, ROData, iFirstRowItem, iLastRowItem, iTotalRowItems, iNumberOfRows,
                    iFirstColItem, iLastColItem, iTotalColItems, iNumberOfColumns);

            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "ProcessAllocationStyleReviewAction failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }

        private ROData ProcessAllocationStyleReviewActionRequest(ROAllocationReviewOptionsParms reviewOptionsParms)
        {
            bool aReviewFlag, aUseSystemTolerancePercent;
            double aTolerancePercent;
            int aStoreFilter, aWorkFlowStepKey;
            try
            {
                // see if an ACTION has been selected
                int action = Convert.ToInt32(reviewOptionsParms.AllocationActionType, CultureInfo.CurrentUICulture);
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
                if (reviewOptionsParms.IsVelocity)
                {
                    return FormatGridsWithApplyView(_wafers, reviewOptionsParms, eAllocationSelectionViewType.Velocity, false, reviewOptionsParms.View.Key);
                }
                else
                {
                    return FormatGridsWithApplyView(_wafers, reviewOptionsParms, eAllocationSelectionViewType.Style, false, reviewOptionsParms.View.Key);
                }
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


        #region "Code for Save AllocationStyleReview #RO-2906"
        private ROOut SaveAllocationStyleReview()
        {
            _applicationSessionTransaction.DataState = eDataState.Updatable;

            bool isAllocStyleReviewSaved = SaveAllocationStyleChanges();

            return new ROBoolOut(eROReturnCode.Successful, null, ROInstanceID, isAllocStyleReviewSaved);
        }

        internal bool SaveAllocationStyleChanges()
        {
            bool isHeaderSaved = true;

            try
            {
                _headerList = (AllocationHeaderProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AllocationHeader);
                FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewStyle);


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
                    UpdateAllocationReviewWorkspace();
                }

                _applicationSessionTransaction.SaveAllocationDefaults();

                SaveUserGridView();

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
        private void UpdateAllocationReviewWorkspace()
        {
            int[] hdrIdList;
            int[] mstrIdList;
            try
            {
                hdrIdList = new int[_headerList.Count];
                int i = 0;
                foreach (AllocationHeaderProfile ahp in _headerList)
                {
                    hdrIdList[i] = Convert.ToInt32(ahp.Key, CultureInfo.CurrentUICulture);
                    i++;
                }
                if (_masterKeyList != null)
                {
                    if (_masterKeyList.Count > 0)
                    {
                        i = 0;

                        mstrIdList = new int[_masterKeyList.Count];

                        foreach (int mstrKey in _masterKeyList)
                        {
                            mstrIdList[i++] = mstrKey;
                        }
                    }
                }

                AllocationProfileList apl = _applicationSessionTransaction.LinkedHeaderList;
                if (apl.Count > 0)
                {
                    i = 0;

                    hdrIdList = new int[apl.Count];

                    foreach (AllocationProfile ap in apl)
                    {
                        hdrIdList[i++] = ap.Key;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void SaveUserGridView()
        {

            _layoutID = eLayoutID.styleReviewGrid;

            _userGridView = new UserGridView();

            _viewRID = _userGridView.UserGridView_Read(SAB.ClientServerSession.UserRID, _layoutID);

            if (_userGridView != null && _viewRID != 0)
            {
                _userGridView.UserGridView_Update(SAB.ClientServerSession.UserRID, _layoutID, _viewRID);
            }
        }

        #endregion "#RO-2906"


    }




}
