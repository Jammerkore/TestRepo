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
        private Dictionary<string, Dictionary<string, Tuple<string, bool, int>>> _viewColumnsTuple = new Dictionary<string, Dictionary<string, Tuple<string, bool, int>>>();

        AllocationWaferGroup _wafers;
        internal ROData allocationReviewData;
        internal SelectedHeaderList _selectedHeaderList;
        private int _currentViewRID = Include.NoRID;

        private void AddViewColumns(int viewRID, ArrayList builtVariables)
        {
            _columnAdded = false;
            if (viewRID == 0 || viewRID == Include.NoRID)
            {
                return;
            }

            DataTable dtGridViewDetail = _gridViewData.GridViewDetail_Read(viewRID);

            if (dtGridViewDetail == null || dtGridViewDetail.Rows.Count == 0)
            {
                return;
            }

            string colKey;
            int colKeyInt;
            bool isDisplayed = false;
            foreach (DataRow row in dtGridViewDetail.Rows)
            {
                colKey = Convert.ToString(row["COLUMN_KEY"], CultureInfo.CurrentUICulture);
                if (colKey == _lblStore)
                {
                    continue;
                }
                colKeyInt = Convert.ToInt32(colKey, CultureInfo.CurrentUICulture);
                isDisplayed = !Include.ConvertCharToBool(Convert.ToChar(row["IS_HIDDEN"], CultureInfo.CurrentUICulture));
                if (isDisplayed)
                {
                    CheckVariableBuiltArrayList(colKeyInt, builtVariables);
                }
            }
        }

        private void CheckVariableBuiltArrayList(int aVariable, ArrayList builtVariables)
        {
            try
            {
                if (!builtVariables.Contains(aVariable))
                {
                    _applicationSessionTransaction.BuildWaferColumnsAdd(0, (eAllocationWaferVariable)aVariable);
                    _applicationSessionTransaction.BuildWaferColumnsAdd(1, (eAllocationWaferVariable)aVariable);
                    _applicationSessionTransaction.BuildWaferColumnsAdd(2, (eAllocationWaferVariable)aVariable);
                    builtVariables.Add(aVariable);
                    _columnAdded = true;
                }
            }
            catch
            {
                throw;
            }
        }

        public int VisiblePosition { get; set; }

        private bool ChkIfMandatoryColumnsPresent(AllocationWaferCoordinate waferCoordinate)
        {
            bool blnDisplay = false;
            switch (_applicationSessionTransaction.AllocationViewType)
            {
                case eAllocationSelectionViewType.Style:
                    eAllocationStyleViewVariableDefault asd = (eAllocationStyleViewVariableDefault)waferCoordinate.Key;
                    if (Enum.IsDefined(typeof(eAllocationStyleViewVariableDefault), asd))
                    { blnDisplay = true; }
                    else { blnDisplay = false; }
                    break;

                case eAllocationSelectionViewType.Velocity:
                    eAllocationVelocityViewVariableDefault avd = (eAllocationVelocityViewVariableDefault)waferCoordinate.Key;
                    if (Enum.IsDefined(typeof(eAllocationVelocityViewVariableDefault), avd))
                    { blnDisplay = true; }
                    else { blnDisplay = false; }
                    break;
                case eAllocationSelectionViewType.Size:
                    eAllocationSizeViewVariableDefault asvvd = (eAllocationSizeViewVariableDefault)waferCoordinate.Key;
                    if (Enum.IsDefined(typeof(eAllocationSizeViewVariableDefault), asvvd))// &&
                                                                                          //_viewRID == Include.NoRID)
                    { blnDisplay = true; }
                    else { blnDisplay = false; }
                    break;
            }
            return blnDisplay;
        }


        private bool IsEligibleToDisplay(int iViewRid, int iWaferCol, AllocationWaferCoordinate waferCoordinate, string sLblStore, AllocationWaferCoordinateList waferCoordinateList)
        {
            bool displayCol = false;

            Tuple<string, bool, int> tuple = GetViewColumnIfExists(iWaferCol, waferCoordinate.Key.ToString());

            //Passing default values when the tuple is null.
            if (tuple == null) { tuple = new Tuple<string, bool, int>(string.Empty, false, 0); }

            VisiblePosition = SetColumnPositions(waferCoordinateList, tuple);

            displayCol = (iViewRid == Include.NoRID || waferCoordinate.Key.ToString() == sLblStore || tuple.Item2);

            //this should be executed only when the display column is false and there should be some view ( to handle StyleAnalsysis view)
            if ((!displayCol) && (iViewRid > 0))
            {
                if (waferCoordinateList[0].CoordinateSubType == (int)eComponentType.Bulk || waferCoordinateList[0].CoordinateSubType == (int)eComponentType.SpecificColor)
                {
                    if (waferCoordinate.Key == (int)eAllocationStyleViewVariableDefault.QuantityAllocated)
                    {
                        displayCol = ChkIfMandatoryColumnsPresent(waferCoordinate);
                        VisiblePosition = SetColumnPositions(waferCoordinateList, tuple);
                    }
                }
                else if (waferCoordinateList[1].CoordinateSubType == (int)eComponentType.Total || waferCoordinateList[1].CoordinateSubType == 0)
                {
                    displayCol = false;

                }
            }

            return displayCol;
        }

        private int SetColumnPositions(AllocationWaferCoordinateList waferCoordinateList, Tuple<string, bool, int> tuple)
        {
            int colPosition, colKey;

            colKey = waferCoordinateList[1].CoordinateSubType;

            if (colKey == (int)eComponentType.Total)
            { colPosition = tuple.Item3; }
            else
            { colPosition = colKey; }

            return colPosition;

        }

        private bool IsAllocationParamsChanged(ROAllocationReviewOptionsParms reviewOptionsParms, eAllocationSelectionViewType selectionViewType)
        {
            bool isParameterChanged = false;
            bool isStoreAttributeChanged = false, isAttributeSetChanged = false, isViewchanged = false, isFilterChanged = false, isViewSequentialChanged = false, isGroupByChanged = false, isSecondaryGroupByChanged = false;
            //Attribute 
            if (reviewOptionsParms.StoreAttribute.Key > 0)
            {
                if (_applicationSessionTransaction.AllocationStoreAttributeID != -1)
                { isStoreAttributeChanged = (_applicationSessionTransaction.AllocationStoreAttributeID != Convert.ToInt32(reviewOptionsParms.StoreAttribute.Key, CultureInfo.CurrentUICulture)); }
                else
                { isStoreAttributeChanged = false; }

                _applicationSessionTransaction.AllocationStoreAttributeID = Convert.ToInt32(reviewOptionsParms.StoreAttribute.Key, CultureInfo.CurrentUICulture);

                if (eAllocationSelectionViewType.Style == selectionViewType)
                {
                    if (_applicationSessionTransaction.AllocationViewType == eAllocationSelectionViewType.Velocity)
                    {
                        _applicationSessionTransaction.VelocityStoreGroupRID = Convert.ToInt32(reviewOptionsParms.StoreAttribute.Key, CultureInfo.CurrentUICulture);
                    }
                }
            }

            //AttributeSet
            if (reviewOptionsParms.AttributeSet.Key > 0)
            {
                if (_applicationSessionTransaction.AllocationStoreGroupLevel != -1)
                { isAttributeSetChanged = (_applicationSessionTransaction.AllocationStoreGroupLevel != Convert.ToInt32(reviewOptionsParms.AttributeSet.Key, CultureInfo.CurrentUICulture)); }
                else
                { isAttributeSetChanged = false; }

                _applicationSessionTransaction.AllocationStoreGroupLevel = Convert.ToInt32(reviewOptionsParms.AttributeSet.Key, CultureInfo.CurrentUICulture);
            }

            //View
            if (reviewOptionsParms.View.Key > 0)
            {
                if (_applicationSessionTransaction.AllocationViewRID != -1)
                { isViewchanged = (_applicationSessionTransaction.AllocationViewRID != Convert.ToInt32(reviewOptionsParms.View.Key, CultureInfo.CurrentUICulture)); }
                else
                { isViewchanged = false; }

                _applicationSessionTransaction.AllocationViewRID = Convert.ToInt32(reviewOptionsParms.View.Key, CultureInfo.CurrentUICulture);
            }

            //Filter
            //if (reviewOptionsParms.Filter.Key > 0)
            //{
            //    if (_applicationSessionTransaction.AllocationFilterID != -1)
            //    { isFilterChanged = (_applicationSessionTransaction.AllocationFilterID != Convert.ToInt32(reviewOptionsParms.Filter.Key, CultureInfo.CurrentUICulture)); }
            //    else
            //    { isFilterChanged = false; }

            //    _applicationSessionTransaction.AllocationFilterID = Convert.ToInt32(reviewOptionsParms.Filter.Key, CultureInfo.CurrentUICulture);
            //}
            isFilterChanged = (_applicationSessionTransaction.AllocationFilterID != Convert.ToInt32(reviewOptionsParms.Filter.Key, CultureInfo.CurrentUICulture));
            if (isFilterChanged)
            {
                _applicationSessionTransaction.AllocationFilterID = Convert.ToInt32(reviewOptionsParms.Filter.Key, CultureInfo.CurrentUICulture);
            }

            isGroupByChanged = (_applicationSessionTransaction.AllocationGroupBy != reviewOptionsParms.GroupBy);

            if (selectionViewType == eAllocationSelectionViewType.Size)
            {
                if (reviewOptionsParms.ViewIsSequential)
                { isViewSequentialChanged = (_applicationSessionTransaction.AllocationViewIsSequential != reviewOptionsParms.ViewIsSequential); }
                else
                { isViewSequentialChanged = (_applicationSessionTransaction.AllocationViewIsSequential != reviewOptionsParms.ViewIsSequential); }

                _applicationSessionTransaction.AllocationViewIsSequential = reviewOptionsParms.ViewIsSequential;

                if (reviewOptionsParms.SecondaryGroupBy > 0)
                {
                    isSecondaryGroupByChanged = (_applicationSessionTransaction.AllocationSecondaryGroupBy != reviewOptionsParms.SecondaryGroupBy);

                    _applicationSessionTransaction.AllocationSecondaryGroupBy = Convert.ToInt32(reviewOptionsParms.SecondaryGroupBy, CultureInfo.CurrentUICulture);
                }
            }

            isParameterChanged = isStoreAttributeChanged || isAttributeSetChanged || isViewchanged || isFilterChanged || isViewSequentialChanged || isGroupByChanged || isSecondaryGroupByChanged;

            return isParameterChanged;
        }

        private int GetReviewGridId(eAllocationSelectionViewType selectionViewType)
        {
            eLayoutID layoutID = eLayoutID.NotDefined;
            if (selectionViewType == eAllocationSelectionViewType.Style)
            { layoutID = eLayoutID.styleReviewGrid; }
            else if (selectionViewType == eAllocationSelectionViewType.Size)
            { layoutID = eLayoutID.sizeReviewGrid; }
            else if (selectionViewType == eAllocationSelectionViewType.Summary)
            { layoutID = eLayoutID.sizeReviewGrid; }

            int viewRID = _userGridView.UserGridView_Read(SAB.ClientServerSession.UserRID, layoutID);
            return viewRID;
        }

        private ROData BuildAllocationReviewData(ROAllocationReviewOptionsParms reviewOptionsParms, eAllocationSelectionViewType selectionViewType, bool rebuildWafers = false)
        {
            try
            {
                _fromAssortment = false;
                if (reviewOptionsParms.ROClass == eROClass.ROAssortment)
                {
                    // check for a list change
                    if (_allocationListFromAssortmentBuilt)
                    {
                        SelectedHeaderList selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
                        if (reviewOptionsParms.ListValues.Count != selectedHeaderList.Count)
                        {
                            _allocationListFromAssortmentBuilt = false;
                        }
                        else
                        {
                            foreach (int key in reviewOptionsParms.ListValues)
                            {
                                if (!selectedHeaderList.Contains(key))
                                {
                                    _allocationListFromAssortmentBuilt = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (!_allocationListFromAssortmentBuilt)
                    {
                        CreateAllocationListFromAssortment(reviewOptionsParms, selectionViewType);
                        _wafers = null;
                        _allocationListFromAssortmentBuilt = true;
                    }
                    _headersLocked = true;
                    _fromAssortment = true;
                }

                _gridViewData = new GridViewData();
                _userGridView = new UserGridView();

                ArrayList selectedAssortmentList = new ArrayList();
                if (!_fromAssortment)
                {
                    _applicationSessionTransaction.CreateAllocationViewSelectionCriteria();
                    _applicationSessionTransaction.NewCriteriaHeaderList();
                }
                _applicationSessionTransaction.AllocationViewType = selectionViewType;
                _applicationSessionTransaction.AllocationNeedAnalysisPeriodBeginRID = Include.NoRID;
                _applicationSessionTransaction.AllocationNeedAnalysisPeriodEndRID = Include.NoRID;
                _applicationSessionTransaction.AllocationNeedAnalysisHNID = Include.NoRID;

                if (reviewOptionsParms.View.Key == 0)
                {
                    reviewOptionsParms.View = new KeyValuePair<int, string>(_userGridView.UserGridView_Read(SAB.ClientServerSession.UserRID, eLayoutID.styleReviewGrid), "From DB");
                }

                _allocationHeaderProfileList = (AllocationHeaderProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AllocationHeader);
                _selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();

                if (_selectedHeaderList.Count == 0 && !_applicationSessionTransaction.ContainsGroupAllocationHeaders())
                {
                    _sROMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoHeaderSelectedOnWorkspace);
                }

                if (!_headersLocked)
                {
                    CheckSecurityEnqueue(selectionViewType);
                    _applicationSessionTransaction.SetCriteriaHeaderList(_allocationHeaderProfileList);
                    _headersLocked = true;
                }

                if (_fromAssortment
                    && _wafers == null)
                {
                    rebuildWafers = true;
                }
                else
                {
                    rebuildWafers = IsAllocationParamsChanged(reviewOptionsParms, selectionViewType);
                }

                // Set value to header if not valid
                SetAllocationGroupBy(reviewOptionsParms, selectionViewType);

                if (_applicationSessionTransaction.AllocationGroupBy == Convert.ToInt32(reviewOptionsParms.GroupBy, CultureInfo.CurrentUICulture))
                {
                    _hdrRow = 0;
                    _compRow = 1;
                }
                else
                {
                    _hdrRow = 1;
                    _compRow = 0;
                }

                _lblStore = MIDText.GetTextOnly(eMIDTextCode.lbl_StoreSingular);

                ArrayList builtVariables = new ArrayList();
                int viewRID = GetReviewGridId(selectionViewType);

                if (reviewOptionsParms.View.Key > 0)
                { viewRID = reviewOptionsParms.View.Key; }

                if (viewRID != _currentViewRID)
                {
                    _applicationSessionTransaction.BuildWaferColumns.Clear();
                    foreach (int variable in Enum.GetValues(typeof(eAllocationStyleViewVariableDefault)))
                    {
                        CheckVariableBuiltArrayList(variable, builtVariables);
                    }

                    AddViewColumns(viewRID, builtVariables);
                    _currentViewRID = viewRID;
                }

                if (reviewOptionsParms.ListValues.Count > 0
                    && _applicationSessionTransaction.GetSelectedHeaders().Count == 0
                    && !_fromAssortment)
                { _applicationSessionTransaction.LoadHeadersInTransaction(reviewOptionsParms.ListValues, selectedAssortmentList, true, false); }

                if (_applicationSessionTransaction.AllocationFilterTable == null
                    || rebuildWafers)
                {
                    FilterData filterDL = new FilterData();
                    DataTable _dtFilter = filterDL.FilterRead(filterTypes.StoreFilter, eProfileType.FilterStore, UserRIDList());
                    _applicationSessionTransaction.AllocationFilterTable = _dtFilter.Copy();
                }

                if (rebuildWafers)
                {
                    _applicationSessionTransaction.RebuildWafers();
                    _wafers = _applicationSessionTransaction.AllocationWafers;
                }
                else
                {
                    if (_wafers == null) { _wafers = _applicationSessionTransaction.AllocationWafers; }
                }

                if (eAllocationSelectionViewType.Size == selectionViewType)
                {
                    return FormatGridsWithApplyView(_wafers, reviewOptionsParms, selectionViewType, true, reviewOptionsParms.View.Key);
                }
                else if (eAllocationSelectionViewType.Style == selectionViewType)
                {
                    return FormatGridsWithApplyView(_wafers, reviewOptionsParms, selectionViewType, false, reviewOptionsParms.View.Key);
                }
                else //if (eAllocationSelectionViewType.Summary == selectionViewType)
                {
                    return FormatGridsWithApplyView(_wafers, reviewOptionsParms, selectionViewType);
                }
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "BuildAllocationReviewData failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }

        private void CreateAllocationListFromAssortment(ROAllocationReviewOptionsParms reviewOptionsParms, eAllocationSelectionViewType selectionViewType)
        {
            AssortmentProfile assortmentProfile = null;
            AllocationProfileList apl = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember);
            foreach (AllocationProfile ap in apl)
            {
                if (ap.HeaderType == eHeaderType.Assortment)
                {
                    //assortmentProfile = (AssortmentProfile)_applicationSessionTransaction.GetAssortmentMemberProfile(ap.Key);
                    assortmentProfile = (AssortmentProfile)ap;
                }
            }
            SelectedHeaderList selectedHeaderList = new SelectedHeaderList(eProfileType.SelectedHeader);
            SAB.ClientServerSession.ClearSelectedHeaderList();
            foreach (int hdrRID in reviewOptionsParms.ListValues)
            {
                SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(hdrRID);
                AllocationProfile ap = (AllocationProfile)apl.FindKey(hdrRID);
                selectedHeader.HeaderType = ap.HeaderType;
                selectedHeaderList.Add(selectedHeader);

                SAB.ClientServerSession.AddSelectedHeaderList(ap.Key, ap.HeaderID, ap.HeaderType, ap.AsrtRID, ap.StyleHnRID);
            }
            if (selectedHeaderList.Count > 0)
            {
                _applicationSessionTransaction.CreateAllocationProfileListFromAssortmentMaster(false, selectedHeaderList, false);
            }
            else
            {
                if (assortmentProfile != null
                    && (eAssortmentType)assortmentProfile.AsrtType == eAssortmentType.PostReceipt)
                {
                    _applicationSessionTransaction.CreateAllocationProfileListFromAssortmentMaster(false, false);
                }
                else
                {
                    _applicationSessionTransaction.CreateAllocationProfileListFromAssortmentMaster(false, true);
                }
            }

            _applicationSessionTransaction.UpdateAllocationViewSelectionHeaders();
            _applicationSessionTransaction.ResetFirstBuild(true);
            _applicationSessionTransaction.ResetFirstBuildSize(true);
            _applicationSessionTransaction.RebuildWafers();
            if (selectionViewType == eAllocationSelectionViewType.Style)
            {
                _applicationSessionTransaction.AllocationViewType = eAllocationSelectionViewType.Style;
                //_applicationSessionTransaction.AllocationGroupBy = Convert.ToInt32(eAllocationStyleViewGroupBy.Header);
            }
            else if (selectionViewType == eAllocationSelectionViewType.Size)
            {
                _applicationSessionTransaction.AllocationViewType = eAllocationSelectionViewType.Size;
                //_applicationSessionTransaction.AllocationGroupBy = Convert.ToInt32(eAllocationStyleViewGroupBy.Header);
            }
            else if (selectionViewType == eAllocationSelectionViewType.Summary)
            {
                _applicationSessionTransaction.AllocationViewType = eAllocationSelectionViewType.Summary;
                //_applicationSessionTransaction.AllocationGroupBy = Convert.ToInt32(eAllocationStyleViewGroupBy.Header);
            }
            _applicationSessionTransaction.AllocationStoreAttributeID = reviewOptionsParms.StoreAttribute.Key;

            //AttributeSet
            if (reviewOptionsParms.AttributeSet.Key > 0)
            {
                _applicationSessionTransaction.AllocationStoreGroupLevel = Convert.ToInt32(reviewOptionsParms.AttributeSet.Key, CultureInfo.CurrentUICulture);
            }
        }

        private void SetAllocationGroupBy(ROAllocationReviewOptionsParms reviewOptionsParms, eAllocationSelectionViewType selectionViewType)
        {
            if (selectionViewType == eAllocationSelectionViewType.Style)
            {
                if (Enum.IsDefined(typeof(eAllocationStyleViewGroupBy), reviewOptionsParms.GroupBy))
                { _applicationSessionTransaction.AllocationGroupBy = Convert.ToInt32(reviewOptionsParms.GroupBy); }
                else
                { _applicationSessionTransaction.AllocationGroupBy = Convert.ToInt32(eAllocationStyleViewGroupBy.Header); }
            }
            else if (selectionViewType == eAllocationSelectionViewType.Size)
            {
                if (Enum.IsDefined(typeof(eAllocationSizeViewGroupBy), reviewOptionsParms.GroupBy))
                { _applicationSessionTransaction.AllocationGroupBy = Convert.ToInt32(reviewOptionsParms.GroupBy); }
                else
                { _applicationSessionTransaction.AllocationGroupBy = Convert.ToInt32(eAllocationSizeViewGroupBy.Header); }

                if (Enum.IsDefined(typeof(eAllocationSizeView2ndGroupBy), reviewOptionsParms.SecondaryGroupBy))
                { _applicationSessionTransaction.AllocationSecondaryGroupBy = Convert.ToInt32(reviewOptionsParms.SecondaryGroupBy); }
                else
                { _applicationSessionTransaction.AllocationSecondaryGroupBy = Convert.ToInt32(eAllocationSizeView2ndGroupBy.Size); }

            }
            else if (selectionViewType == eAllocationSelectionViewType.Summary)
            {
                if (Enum.IsDefined(typeof(eAllocationSummaryViewGroupBy), reviewOptionsParms.GroupBy))
                { _applicationSessionTransaction.AllocationGroupBy = Convert.ToInt32(reviewOptionsParms.GroupBy); }
                else
                { _applicationSessionTransaction.AllocationGroupBy = Convert.ToInt32(eAllocationSummaryViewGroupBy.Attribute); }
            }
        }

        private void CheckSecurityEnqueue(eAllocationSelectionViewType selectionViewType)
        {
            try
            {
                if (!_applicationSessionTransaction.VelocityCriteriaExists)
                {
                    if ((selectionViewType == eAllocationSelectionViewType.Summary && AllocationReviewSummarySecurity.AllowUpdate) ||
                        (selectionViewType == eAllocationSelectionViewType.Size && AllocationReviewSizeSecurity.AllowUpdate) ||
                        (selectionViewType == eAllocationSelectionViewType.Style && AllocationReviewStyleSecurity.AllowUpdate))
                    {
                        try
                        {
                            bool OKToEnqueue = true;
                            FunctionSecurityProfile nodeFunctionSecurity;
                            eSecurityFunctions securityFunction;

                            if (selectionViewType == eAllocationSelectionViewType.Style)
                            { securityFunction = eSecurityFunctions.AllocationReviewStyle; }
                            else if (selectionViewType == eAllocationSelectionViewType.Summary)
                            { securityFunction = eSecurityFunctions.AllocationReviewSummary; }
                            else
                            { securityFunction = eSecurityFunctions.AllocationReviewSize; }

                            List<int> selectedHdrRIDs = new List<int>();
                            foreach (AllocationHeaderProfile ahp in _allocationHeaderProfileList)
                            {
                                nodeFunctionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(ahp.StyleHnRID, securityFunction, (int)eSecurityTypes.Allocation);
                                if (!nodeFunctionSecurity.AllowUpdate)
                                {
                                    OKToEnqueue = false;
                                    break;
                                }
                                selectedHdrRIDs.Add(ahp.Key);
                            }

                            if (OKToEnqueue)
                            {
                                string enqMsg;
                                if (_applicationSessionTransaction.EnqueueHeaders(_applicationSessionTransaction.GetHeadersToEnqueue(selectedHdrRIDs), out enqMsg))
                                    _applicationSessionTransaction.DataState = eDataState.Updatable;
                                else
                                {

                                    MIDEnvironment.isChangedToReadOnly = true;
                                    MIDEnvironment.Message = enqMsg + System.Environment.NewLine + MIDText.GetTextOnly(eMIDTextCode.msg_ReadOnlyMode);

                                    _applicationSessionTransaction.DataState = eDataState.ReadOnly;
                                }
                            }
                            else
                                _applicationSessionTransaction.DataState = eDataState.ReadOnly;
                        }
                        catch (CancelProcessException)
                        {
                            
                        }
                    }
                    else
                        _applicationSessionTransaction.DataState = eDataState.ReadOnly;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool IsColInWindows(AllocationWaferCoordinateList waferCoordList, int gridNo, AllocationWaferCoordinate waferCoordinate)
        {
            const string GRID_NAME = "g3";

            if (GRID_NAME == "g" + (gridNo + 1))
            {
                //filtration needs only for grid 3.
                bool bln1And2Same = false;
                bool blnBulkAndBlack = false;

                if (_applicationSessionTransaction.AllocationGroupBy == Convert.ToInt32(eAllocationStyleViewGroupBy.Header))
                { bln1And2Same = (waferCoordList[1].Label == waferCoordList[2].Label); }
                else if (_applicationSessionTransaction.AllocationGroupBy == Convert.ToInt32(eAllocationStyleViewGroupBy.Components))
                { bln1And2Same = (waferCoordList[0].Label == waferCoordList[2].Label); }

                blnBulkAndBlack = ChkIfMandatoryColumnsPresent(waferCoordinate);
                return bln1And2Same || blnBulkAndBlack;
            }
            else
            { return true; }
        }

        private int GetGroupByValueByAllocationGroupBy(AllocationWaferCoordinateList waferCoordlist, int allocationGroupBy, int allocation2GroupBy = 0)
        {
            int key = 0;

            if (allocationGroupBy == Convert.ToInt32(eAllocationStyleViewGroupBy.Header))
            { key = waferCoordlist[0].Key; }
            else if (allocationGroupBy == Convert.ToInt32(eAllocationStyleViewGroupBy.Components))
            { key = waferCoordlist[0].CoordinateSubType; }
            else if (allocationGroupBy == Convert.ToInt32(eAllocationSizeViewGroupBy.Header))
            {
                if (allocation2GroupBy == Convert.ToInt32(eAllocationSizeView2ndGroupBy.Size))
                { key = allocation2GroupBy; }
                else
                { key = (int)eAllocationSizeViewVariableDefault.QuantityAllocated; }

            }
            else if (allocationGroupBy == Convert.ToInt32(eAllocationSizeViewGroupBy.Color))
            {
                if (allocation2GroupBy == Convert.ToInt32(eAllocationSizeView2ndGroupBy.Size))
                {
                    key = allocation2GroupBy;
                }
                else
                { key = (int)eAllocationSizeViewVariableDefault.QuantityAllocated; }
            }
            return key;
        }

        private ROData FormatGridsWithApplyView(AllocationWaferGroup wafersin, ROAllocationReviewOptionsParms reviewOptionsParms,
            eAllocationSelectionViewType selectionViewType, bool filterDuplicatesForSizeReview = false, int aViewRID = Include.NoRID)
        {

            AllocationWaferCoordinateList waferCoordlist;
            AllocationWaferCoordinate waferCoord;
            AllocationWaferVariable varProf;
            int iToIncrement = 1;

            ROData roData = new ROData();
            eDataType dataType = eDataType.None;
            eDataType[,] dataTypeList = new eDataType[3, 3] {
                { eDataType.StoreSummary, eDataType.StoreTotals, eDataType.StoreDetail },
                { eDataType.SetSummary, eDataType.SetTotals, eDataType.SetDetail },
                { eDataType.AllStoreSummary, eDataType.AllStoreTotals, eDataType.AllStoreDetail }
            };


            if (aViewRID > 0) { ApplyView(aViewRID); }

            string lblStore = MIDText.GetTextOnly(eMIDTextCode.lbl_StoreSingular);
            _removedPctTotalRows = false;


            for (int waferRow = 0; waferRow < _wafers.RowCount; waferRow++)
            {
                for (int waferCol = 0; waferCol < _wafers.ColumnCount; waferCol++)
                {
                    AllocationWafer wafer = _wafers[waferRow, waferCol];
                    dataType = dataTypeList[waferRow, waferCol];
                    ROCells cells = new ROCells();

                    if (filterDuplicatesForSizeReview) iToIncrement = 2;

                    int rowIndex = wafer.RowLabels.GetLength(1);
                    for (int rowLblCntr = 0; rowLblCntr < wafer.RowLabels.GetLength(0); rowLblCntr = rowLblCntr + iToIncrement)
                    {
                        string rowLabel = null;
                        if (selectionViewType == eAllocationSelectionViewType.Summary
                             && (dataType == eDataType.SetSummary || dataType == eDataType.AllStoreSummary))
                        {
                            for (int r = 0; r < rowIndex; r++)
                            {
                                if (!string.IsNullOrEmpty(wafer.RowLabels[rowLblCntr, r]))
                                {
                                    if (rowLabel != null && rowLabel.Length > 0)
                                    {
                                        rowLabel += "|";
                                    }
                                    rowLabel += wafer.RowLabels[rowLblCntr, r] + " ";
                                }
                            }
                        }
                        else
                        {
                            rowLabel = wafer.RowLabels[rowLblCntr, 0];
                        }

                        cells.Rows.Add(new RORowAttributes(rowLabel));
                    }

                    int columnLength = wafer.ColumnLabels.GetLength(1);
                    int columnIndex = wafer.ColumnLabels.GetLength(0);
                    int indexValue = 0;

                    for (int k = 0; k < columnLength; k++)
                    {
                        string columnName = null;
                        waferCoordlist = (AllocationWaferCoordinateList)wafer.Columns[k];
                        waferCoord = GetAllocationCoordinate(waferCoordlist, eAllocationCoordinateType.Variable);
                        if (IsEligibleToDisplay(aViewRID, waferCol, waferCoord, lblStore, waferCoordlist))
                        {
                            for (int j = 0; j < columnIndex; j++)
                            {
                                if (!string.IsNullOrEmpty(wafer.ColumnLabels[j, k]))
                                {
                                    if (columnName != null && columnName.Length > 0)
                                    {
                                        columnName += "|";
                                    }
                                    columnName += wafer.ColumnLabels[j, k] + " ";
                                }
                            }

                            bool DisplayedInWindows = false;
                            if (filterDuplicatesForSizeReview)
                            { DisplayedInWindows = true; }
                            else { DisplayedInWindows = IsColInWindows(waferCoordlist, waferCol, waferCoord); }

                            cells.Columns.Add(new ROColumnAttributes(columnName.Trim(), indexValue, VisiblePosition,
                                GetGroupByValueByAllocationGroupBy(waferCoordlist, reviewOptionsParms.GroupBy, reviewOptionsParms.SecondaryGroupBy),
                                DisplayedInWindows, k));
                            indexValue = indexValue + 1;
                        }
                    }

                    if (selectionViewType == eAllocationSelectionViewType.Size
                        && (dataType == eDataType.StoreSummary || dataType == eDataType.SetSummary || dataType == eDataType.AllStoreSummary))
                    {
                        AddSizeLabelColumns(cells, reviewOptionsParms, dataType);
                    }

                    int iRows = wafer.Cells.GetLength(0);

                    if (selectionViewType == eAllocationSelectionViewType.Size)
                    {
                        // count needed rows
                        // skip all eAllocationWaferVariable.PctToTotal rows
                        // was enhancement that was not completed and hid in legacy
                        iRows = 0;
                        for (int row = 0; row < wafer.Cells.GetLength(0); row++)
                        {
                            AllocationWaferCoordinateList cubeWaferCoorList = (AllocationWaferCoordinateList)wafer.Rows[row];
                            waferCoord = GetAllocationCoordinate(cubeWaferCoorList, eAllocationCoordinateType.Variable);
                            if (waferCoord.Key != (int)eAllocationWaferVariable.PctToTotal)
                            {
                                ++iRows;
                            }
                            else
                            {
                                _removedPctTotalRows = true;
                            }
                        }
                    }

                    cells.AddCells(iRows, cells.Columns.Count);
                    int rowCntr = 0;

                    for (int row = 0; row < wafer.Cells.GetLength(0); row++)
                    {
                        if (filterDuplicatesForSizeReview)
                        {
                            TagForRow rowTag = new TagForRow();
                            rowTag.cellRow = row;
                            rowTag.CubeWaferCoorList = (AllocationWaferCoordinateList)wafer.Rows[row];
                            waferCoord = GetAllocationCoordinate(rowTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
                            if (waferCoord.Key == (int)eAllocationWaferVariable.PctToTotal)
                            {
                                rowTag.IsDisplayed = false;
                                continue;
                            }
                            else
                            {
                                rowTag.IsDisplayed = true;
                            }
                        }

                        int cellColumn = 0;
                        for (int col = 0; col < wafer.Cells.GetLength(1); col++)
                        {
                            waferCoordlist = (AllocationWaferCoordinateList)wafer.Columns[col];
                            waferCoord = GetAllocationCoordinate(waferCoordlist, eAllocationCoordinateType.Variable);
                            varProf = AllocationWaferVariables.GetVariableProfile((eAllocationWaferVariable)waferCoord.Key);
                            if (IsEligibleToDisplay(aViewRID, waferCol, waferCoord, lblStore, waferCoordlist))
                            {
                                ROCell cell = new ROCell();
                                switch (varProf.Format)
                                {
                                    case eAllocationWaferVariableFormat.String:
                                    case eAllocationWaferVariableFormat.None:
                                        cell.Value = wafer.Cells[row, col].ValueAsString;
                                        cell.CellValueType = eCellValueType.String;
                                        break;

                                    case eAllocationWaferVariableFormat.Number:
                                        cell.Value = wafer.Cells[row, col].Value;
                                        if (varProf.NumDecimals > 0)
                                        {
                                            cell.CellValueType = eCellValueType.Number;
                                            cell.ROCellAttributes.DecimalPositions = varProf.NumDecimals;
                                        }
                                        else
                                        {
                                            cell.CellValueType = eCellValueType.Integer;
                                        }
                                        if ((varProf.Key == Convert.ToInt32(eAllocationWaferVariable.StoreIMOMaxQuantityAllocated, CultureInfo.CurrentUICulture))
                                                && (Convert.ToInt32(cell.Value) == int.MaxValue))
                                        {
                                            cell.Value = null;
                                        }

                                        break;

                                    case eAllocationWaferVariableFormat.eRuleType:
                                        cell.Value = wafer.Cells[row, col].Value;
                                        cell.CellValueType = eCellValueType.RuleType;
                                        break;

                                    default:
                                        cell.Value = wafer.Cells[row, col].Value;
                                        cell.CellValueType = eCellValueType.String;
                                        break;
                                }

                                cell.ROCellAttributes.IsValid = wafer.Cells[row, col].CellIsValid;
                                cell.ColumnPosition = VisiblePosition;

                                cell.ColumnHeader = GetGroupByValueByAllocationGroupBy(waferCoordlist, reviewOptionsParms.GroupBy, reviewOptionsParms.SecondaryGroupBy);

                                if (filterDuplicatesForSizeReview)
                                { cell.DisplayedInWindows = true; }
                                else { cell.DisplayedInWindows = IsColInWindows(waferCoordlist, waferCol, waferCoord); }
                                cell.ROCellAttributes.MayExceedCapacityMaximum = wafer.Cells[row, col].MayExceedCapacityMaximum;
                                cell.ROCellAttributes.MayExceedGradeMaximum = wafer.Cells[row, col].MayExceedGradeMaximum;
                                cell.ROCellAttributes.MayExceedPrimaryMaximum = wafer.Cells[row, col].MayExceedPrimaryMaximum;
                                cell.ROCellAttributes.StoreAllocationOutOfBalance = wafer.Cells[row, col].StoreAllocationOutOfBalance;
                                cell.ROCellAttributes.StoreExceedsCapacity = wafer.Cells[row, col].StoreExceedsCapacity;
                                cell.ROCellAttributes.IsEditable = wafer.Cells[row, col].CellCanBeChanged;
                                cell.ROCellAttributes.GradeMaximumValue = wafer.Cells[row, col].GradeMaximumValue;
                                cell.ROCellAttributes.PrimaryMaximumValue = wafer.Cells[row, col].PrimaryMaximumValue;
                                cell.ROCellAttributes.MinimumValue = wafer.Cells[row, col].MinimumValue;
                                if ((cell.CellValueType == eCellValueType.Number || cell.CellValueType == eCellValueType.Integer)
                                    && Convert.ToDecimal(cell.Value) < 0)
                                {
                                    cell.ROCellAttributes.IsNegative = true;
                                }
                                cells.ROCell[rowCntr][cellColumn] = cell;
                                ++cellColumn;
                            }
                        }
                        rowCntr++;
                    }

                    if (selectionViewType == eAllocationSelectionViewType.Size
                        && (dataType == eDataType.StoreSummary || dataType == eDataType.SetSummary || dataType == eDataType.AllStoreSummary))
                    {
                        AddSizeLabelValues(cells, reviewOptionsParms, wafer, dataType);
                    }

                    roData.AddCells(dataType, cells);
                }
            }
            allocationReviewData = roData;
            return roData;
        }

        private void AddSizeLabelColumns(ROCells cells, ROAllocationReviewOptionsParms reviewOptionsParms, eDataType dataType)
        {
            string lblHeader = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Header);
            string lblColor = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Color);
            string lblVariable = MIDText.GetTextOnly(eMIDTextCode.lbl_Variable);
            string lblDimension = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Dimension);

            string[,] RowLabels = _wafers[0, 0].RowLabels;
            if (dataType == eDataType.AllStoreSummary)
            {
                RowLabels = _wafers[2, 0].RowLabels;
            }
            else if (dataType == eDataType.SetSummary)
            {
                RowLabels = _wafers[1, 0].RowLabels;
            }

            int colLabelCount = RowLabels.GetLength(1);
            if (colLabelCount == 0)
            {
                if (!reviewOptionsParms.ViewIsSequential)
                {
                    colLabelCount = 5;
                }
                else
                {
                    colLabelCount = 4;
                }
            }

            int k = cells.Columns.Count;

            // use max so sorts to the end column
            if ((eAllocationSizeViewGroupBy)reviewOptionsParms.GroupBy == eAllocationSizeViewGroupBy.Header)
            {
                cells.Columns.Add(new ROColumnAttributes(lblHeader, k, k, int.MaxValue, true, 0));
                k++;
                cells.Columns.Add(new ROColumnAttributes(lblColor, k, k, int.MaxValue, true, 0));
            }
            else
            {
                cells.Columns.Add(new ROColumnAttributes(lblColor, k, k, int.MaxValue, true, 0));
                k++;
                cells.Columns.Add(new ROColumnAttributes(lblHeader, k, k, int.MaxValue, true, 0));

            }
            if (!reviewOptionsParms.ViewIsSequential)
            {
                k++;
                cells.Columns.Add(new ROColumnAttributes(lblDimension, k, k, int.MaxValue, true, 0));
            }

        }

        private void AddSizeLabelValues(ROCells cells, ROAllocationReviewOptionsParms reviewOptionsParms, AllocationWafer wafer, eDataType dataType)
        {
            string[,] RowLabels = _wafers[0, 0].RowLabels;
            if (dataType == eDataType.AllStoreSummary)
            {
                RowLabels = _wafers[2, 0].RowLabels;
            }
            else if (dataType == eDataType.SetSummary)
            {
                RowLabels = _wafers[1, 0].RowLabels;
            }

            ROCell cell;
            int cellRow = 0;
            for (int row = 0; row < RowLabels.GetLength(0); row++)
            {
                // skip PctToTotal rows: was enhancement that was never finished
                AllocationWaferCoordinateList cubeWaferCoorList = (AllocationWaferCoordinateList)wafer.Rows[row];
                AllocationWaferCoordinate waferCoord = GetAllocationCoordinate(cubeWaferCoorList, eAllocationCoordinateType.Variable);
                if (waferCoord.Key != (int)eAllocationWaferVariable.PctToTotal)
                {
                    int rowLabelCol = 1;  // skip store column
                    int cellColumn = 0;

                    foreach (ROColumnAttributes col in cells.Columns)
                    {
                        if (col.ColumnHeader == int.MaxValue)
                        {
                            cell = new ROCell(eCellDataType.Label, RowLabels[row, rowLabelCol]);
                            cell.ColumnPosition = cellColumn;
                            cell.ColumnHeader = int.MaxValue;  // use max so sorts to the end column
                            cell.DisplayedInWindows = true;
                            cells.ROCell[cellRow][cellColumn] = cell;
                            ++rowLabelCol;
                        }

                        ++cellColumn;
                    }

                    ++cellRow;
                }
            }
        }

        /// <summary>
        /// This procedure puts the data into the memory used for Style/size/Summary
        /// </summary>
        /// <param name="gridChanges"></param>
        /// <returns></returns>
        private ROOut UpdateAllocationReview(ROGridChangesParms gridChanges)
        {
            AllocationWaferCellChangeList allocationWaferCellChangeList;
            allocationWaferCellChangeList = new AllocationWaferCellChangeList();

            int waferRow = 0, waferCol = 0;

            foreach (ROGridCellChange cellChange in gridChanges.CellChanges)
            {
                switch (cellChange.DataType)

                {
                    case eDataType.StoreSummary:
                        waferRow = 0;
                        waferCol = 0;
                        break;
                    case eDataType.StoreTotals:
                        waferRow = 0;
                        waferCol = 1;
                        break;
                    case eDataType.StoreDetail:
                        waferRow = 0;
                        waferCol = 2;
                        break;
                    case eDataType.SetSummary:
                        waferRow = 1;
                        waferCol = 0;
                        break;
                    case eDataType.SetTotals:
                        waferRow = 1;
                        waferCol = 1;
                        break;
                    case eDataType.SetDetail:
                        waferRow = 1;
                        waferCol = 2;
                        break;
                    case eDataType.AllStoreSummary:
                        waferRow = 2;
                        waferCol = 1;
                        break;
                    case eDataType.AllStoreTotals:
                        waferRow = 2;
                        waferCol = 1;
                        break;
                    case eDataType.AllStoreDetail:
                        waferRow = 2;
                        waferCol = 2;
                        break;

                }

                allocationWaferCellChangeList.AddAllocationWaferCellChange(
                    waferRow, waferCol, cellChange.RowIndex, GetWaferCoordinateNo(cellChange.ColumnIndex, cellChange.DataType), cellChange.dNewValue);
            }

            _applicationSessionTransaction.SetAllocationCellValue(allocationWaferCellChangeList);

            _wafers = _applicationSessionTransaction.AllocationWafers;

            return new RONoDataOut(eROReturnCode.Successful, null, ROInstanceID);
        }
        /// <summary>
        /// this function returns the wafer no based on the user selected column.
        /// </summary>
        /// <param name="colIndex"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        private int GetWaferCoordinateNo(int colIndex, eDataType dataType)
        {
            List<ROColumnAttributes> columnAttributes = allocationReviewData.GetCells(dataType).Columns;
            int intWaferNo = columnAttributes.Find(ix => ix.ColumnIndex == colIndex).WaferNo;
            return intWaferNo;
        }
        
        private AllocationWaferCoordinate GetAllocationCoordinate(AllocationWaferCoordinateList aCoordList, eAllocationCoordinateType aCoordType)
        {
            AllocationWaferCoordinate waferCoord = null;
            for (int i = 0; i < aCoordList.Count; i++)
            {
                waferCoord = aCoordList[i];
                if (waferCoord.CoordinateType == aCoordType)
                    break;
            }
            return waferCoord;
        }

        private Tuple<string, bool, int> GetViewColumnIfExists(int waferCol, string coordKey)
        {
            Tuple<string, bool, int> tuple2 = null; ;
            if (_viewColumnsTuple.TryGetValue("g" + (waferCol + 1), out Dictionary<string, Tuple<string, bool, int>> gridKeyEntry))
            {
                if (gridKeyEntry.TryGetValue(coordKey, out Tuple<string, bool, int> tuple))
                {
                    tuple2 = tuple;
                    tuple2 = new Tuple<string, bool, int>(tuple.Item1, tuple.Item2, tuple.Item3);
                }
            }
            return tuple2;
        }


        private void ApplyView(int aViewRID)
        {
            try
            {
                Dictionary<string, Tuple<string, bool, int>> gridKeyEntryTuple;
                int visiblePosition = 0;
                string gridKey, colKey, errMessage;
                bool isHidden;

                if (aViewRID == 0 || aViewRID == Include.NoRID)    // don't modify current grid appearance 
                {
                    return;
                }

                DataTable dtGridViewDetail = _gridViewData.GridViewDetail_Read_ByPosition(aViewRID);

                if (dtGridViewDetail == null || dtGridViewDetail.Rows.Count == 0)
                {
                    errMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_as_GridViewDoesNotExist);
                    return;
                }

                _viewColumnsTuple.Clear();
                string sTakeVisibleColumns = "IS_HIDDEN=0";
                foreach (DataRow row in dtGridViewDetail.Select(sTakeVisibleColumns))
                {
                    gridKey = Convert.ToString(row["BAND_KEY"], CultureInfo.CurrentUICulture);
                    colKey = Convert.ToString(row["COLUMN_KEY"], CultureInfo.CurrentUICulture);
                    visiblePosition = Convert.ToInt32(row["VISIBLE_POSITION"], CultureInfo.CurrentUICulture);
                    isHidden = Include.ConvertCharToBool(Convert.ToChar(row["IS_HIDDEN"], CultureInfo.CurrentUICulture));

                    if (!_viewColumnsTuple.TryGetValue(gridKey, out gridKeyEntryTuple))
                    {
                        gridKeyEntryTuple = new Dictionary<string, Tuple<string, bool, int>>();
                        _viewColumnsTuple.Add(gridKey, gridKeyEntryTuple);

                    }

                    var tuple = new Tuple<string, bool, int>(colKey, !isHidden, visiblePosition);
                    if (!gridKeyEntryTuple.Keys.Contains(colKey)) { gridKeyEntryTuple.Add(colKey, tuple); }
                }

            }
            catch
            {
                throw;
            }

        }

        private bool IsEligibleToProcessAssortmentActionType(eAllocationActionType aAction)
        {
            SelectedHeaderList allocHdrList = (SelectedHeaderList)_applicationSessionTransaction.AssortmentSelectedHdrList;
            string errorParm = string.Empty;
            bool isEligibleToProcess = true;
            AllocationProfileList alp = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember);

            foreach (SelectedHeaderProfile shp in allocHdrList)
            {
                AllocationProfile ap = (AllocationProfile)alp.FindKey(shp.Key);

                if (ap.HeaderType == eHeaderType.Assortment)
                {
                    continue;
                }

                switch (ap.HeaderAllocationStatus)
                {
                    case eHeaderAllocationStatus.ReceivedOutOfBalance:
                        if (aAction != eAllocationActionType.BackoutAllocation)
                        {
                            isEligibleToProcess = false;
                        }
                        break;
                    case eHeaderAllocationStatus.ReleaseApproved:
                        if (aAction != eAllocationActionType.Reset &&
                            aAction != eAllocationActionType.Release)
                        {
                            isEligibleToProcess = false;
                        }
                        break;
                    case eHeaderAllocationStatus.Released:
                        if (aAction != eAllocationActionType.Reset)
                        {
                            isEligibleToProcess = false;
                        }
                        break;
                    default:
                        if (aAction == eAllocationActionType.Reset)
                        {
                            isEligibleToProcess = false;
                        }
                        else if (aAction == eAllocationActionType.ChargeIntransit)
                        {
                            if (ap.HeaderAllocationStatus != eHeaderAllocationStatus.AllInBalance
                                && ap.HeaderAllocationStatus != eHeaderAllocationStatus.AllocatedInBalance)
                                isEligibleToProcess = false;
                        }
                        break;
                }

                if (!isEligibleToProcess)
                {
                    _sROMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction), MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));
                    _returnCode = eROReturnCode.Failure;
                    isEligibleToProcess = false;
                    break;
                }

                if (isEligibleToProcess)
                {
                    if (!SAB.ClientServerSession.GlobalOptions.IsReleaseable(ap.HeaderType)
                        && aAction == eAllocationActionType.Release)
                    {
                        if (ap.IsDummy)
                        {
                            errorParm = MIDText.GetTextOnly((int)eHeaderType.Dummy) + " " + MIDText.GetTextOnly(eMIDTextCode.lbl_Header);
                        }
                        else
                        {
                            errorParm = MIDText.GetTextOnly((int)ap.HeaderType);
                        }

                        _sROMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_ActionNotAllowed), errorParm);
                        _returnCode = eROReturnCode.Failure;
                        isEligibleToProcess = false;
                        break;
                    }
                }
            }
            if (isEligibleToProcess)
            {
                if (aAction == eAllocationActionType.BackoutAllocation
                    || aAction == eAllocationActionType.BackoutSizeAllocation
                    || aAction == eAllocationActionType.BackoutSizeIntransit
                    || aAction == eAllocationActionType.BackoutStyleIntransit
                    || aAction == eAllocationActionType.Reset)
                {
                    _sROMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ActionWarning),
                        MIDText.GetTextOnly((int)aAction));
                    _returnCode = eROReturnCode.Failure;
                    isEligibleToProcess = false;
                }
            }

            if (isEligibleToProcess)
            {
                isEligibleToProcess = VerifySecurity(allocHdrList);
            }

            return isEligibleToProcess;
        }

        private bool IsEligibleToProcessAllocationActionType(eAllocationActionType aAction)
        {
            string errorParm = string.Empty;
            bool isEligibleToProcess = true;
            eHeaderAllocationStatus headerStatus;

            if (_masterKeyList == null)
            {
                _masterKeyList = new ArrayList();
            }
            foreach (AllocationHeaderProfile ahp in _allocationHeaderProfileList)
            {
                if (!SAB.ClientServerSession.GlobalOptions.IsReleaseable(ahp.HeaderType) && aAction == eAllocationActionType.Release)
                {
                    isEligibleToProcess = false;
                    if (ahp.IsDummy)
                    {
                        _sROMessage = MIDText.GetText(eMIDTextCode.msg_al_CannotReleaseDummy) + ahp.HeaderID;
                    }
                    else
                    {
                        _sROMessage = MIDText.GetText(eMIDTextCode.msg_al_HeaderTypeCanNotBeReleased) + ": " + ahp.HeaderID;
                    }

                    _returnCode = eROReturnCode.Failure;
                    break;
                }
                else if (ahp.IsMasterHeader && aAction == eAllocationActionType.Release)
                {
                    isEligibleToProcess = false;
                    _sROMessage = MIDText.GetText(eMIDTextCode.msg_al_CannotReleaseMaster) + ahp.HeaderID;
                    _returnCode = eROReturnCode.Failure;
                    break;
                }
                else
                {
                    headerStatus = _applicationSessionTransaction.GetHeaderAllocationStatus(ahp.Key);
                    switch (headerStatus)
                    {
                        case eHeaderAllocationStatus.ReceivedOutOfBalance:
                            isEligibleToProcess = false;
                            break;
                        case eHeaderAllocationStatus.ReleaseApproved:
                        case eHeaderAllocationStatus.Released:
                            if (aAction != eAllocationActionType.Reset &&
                                aAction != eAllocationActionType.Release)
                            {
                                isEligibleToProcess = false;
                            }
                            break;
                        default:
                            if (aAction == eAllocationActionType.Reset)
                            {
                                isEligibleToProcess = false;
                            }
                            else if (aAction == eAllocationActionType.ChargeIntransit)
                            {
                                if (headerStatus != eHeaderAllocationStatus.AllInBalance
                                    && headerStatus != eHeaderAllocationStatus.AllocatedInBalance)
                                    isEligibleToProcess = false;
                            }
                            break;
                    }
                }
                if (!isEligibleToProcess)
                {
                    _sROMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction), MIDText.GetTextOnly((int)headerStatus));
                    _returnCode = eROReturnCode.Failure;
                    break;
                }

                if (isEligibleToProcess)
                {
                    if (ahp.IsMasterHeader
                            && ahp.DCFulfillmentProcessed)
                    {
                        errorParm = MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed);
                        errorParm = errorParm.Replace("{0}", ahp.HeaderID);

                        _sROMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_ActionNotAllowed), errorParm);
                        _returnCode = eROReturnCode.Failure;
                        isEligibleToProcess = false;

                        break;
                    }
                    else if (ahp.IsSubordinateHeader
                            && !ahp.DCFulfillmentProcessed)
                    {
                        errorParm = MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed);
                        errorParm = errorParm.Replace("{0}", ahp.HeaderID);

                        _sROMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_ActionNotAllowed), errorParm);
                        _returnCode = eROReturnCode.Failure;
                        isEligibleToProcess = false;

                        break;
                    }

                    if (aAction == eAllocationActionType.ReapplyTotalAllocation && ahp.HeaderType == eHeaderType.WorkupTotalBuy)
                    {
                        _sROMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_CannotPerformActionOnHeader),
                                        MIDText.GetTextOnly((int)aAction),
                                        ahp.HeaderID,
                                        MIDText.GetTextOnly((int)ahp.HeaderType));
                        _returnCode = eROReturnCode.Failure;
                        isEligibleToProcess = false;
                        break;
                    }
                }

                if (isEligibleToProcess)
                {
                    int masterRID = Include.NoRID;

                    Header header = new Header();

                    masterRID = header.GetMasterForSubord(ahp.Key);

                    if (masterRID != Include.NoRID)
                    {
                        if (!_masterKeyList.Contains(masterRID))
                        {
                            _masterKeyList.Add(masterRID);
                        }
                    }
                }
            }

            //if (isEligibleToProcess)
            //{
            //    if (aAction == eAllocationActionType.BackoutAllocation
            //        || aAction == eAllocationActionType.BackoutSizeAllocation
            //        || aAction == eAllocationActionType.BackoutSizeIntransit
            //        || aAction == eAllocationActionType.BackoutStyleIntransit
            //        || aAction == eAllocationActionType.Reset)
            //    {
            //        _sROMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ActionWarning),
            //            MIDText.GetTextOnly((int)aAction));
            //        _returnCode = eROReturnCode.Failure;
            //        isEligibleToProcess = false;
            //    }
            //}
            return isEligibleToProcess;
        }

        private void UpdateAllocationWorkspace()
        {
            int[] hdrIdList;
            int[] mstrIdList;

            try
            {
                hdrIdList = new int[_allocationHeaderProfileList.Count];
                int i = 0;
                foreach (AllocationHeaderProfile ahp in _allocationHeaderProfileList)
                {
                    hdrIdList[i] = Convert.ToInt32(ahp.Key, CultureInfo.CurrentUICulture);
                    i++;
                }
                //if (_awExplorer != null)
                //{
                //    _awExplorer.ReloadUpdatedHeaders(hdrIdList);
                //}

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

                        //if (_awExplorer != null)
                        //{
                        //    _awExplorer.ReloadUpdatedHeaders(mstrIdList);
                        //}
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

                    //if (_awExplorer != null)
                    //{
                    //    _awExplorer.ReloadUpdatedHeaders(hdrIdList);
                    //}
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        internal bool VerifySecurity(SelectedHeaderList selectedHeaderList)
        {
            HierarchyNodeSecurityProfile hierNodeSecProfile;
            try
            {
                bool allowUpdate = true;
                foreach (SelectedHeaderProfile shp in selectedHeaderList)
                {
                    AllocationProfile ap = _applicationSessionTransaction.GetAssortmentMemberProfile(shp.Key);
                    if (ap != null && ap.StyleHnRID > 0)
                    {
                        hierNodeSecProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(ap.StyleHnRID, (int)eSecurityTypes.Allocation);
                        if (!hierNodeSecProfile.AllowUpdate)
                        {
                            HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(shp.StyleHnRID, false, false);
                            allowUpdate = false;
                            string errorMessage = MIDText.GetText(eMIDTextCode.msg_NotAuthorizedForNode);
                            errorMessage = errorMessage + " Node: " + hnp.Text;
                            SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_NotAuthorizedForNode, errorMessage, "Assortment View");
                            break;
                        }
                    }
                }
                return allowUpdate;
            }
            catch
            {
                throw;
            }
        }
    }
}
