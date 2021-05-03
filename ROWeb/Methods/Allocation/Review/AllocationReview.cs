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
        private Dictionary<string, Dictionary<string, AllocationViewColumn>> _viewColumns = new Dictionary<string, Dictionary<string, AllocationViewColumn>>();

        AllocationWaferGroup _wafers;
        private bool _wafersBuilt = false;
        internal ROData allocationReviewData;
        internal SelectedHeaderList _selectedHeaderList;
        private int _currentViewRID = Include.NoRID;
        private bool _viewHasBeenUpdated = false;
        private int _currentViewUserRID = Include.NoRID;
        internal Dictionary<eDataType, List<ROSelectedField>> _columnsList;
        private List<int> _selectedFieldKeys;
        private int _currentVisiblePosition = 0;

        private void AddViewColumns(int viewRID, ArrayList builtVariables)
        {
            _columnAdded = false;
            if (viewRID == 0 || viewRID == Include.NoRID)
            {
                return;
            }

            DataTable dtGridViewDetail = _gridViewData.GridViewDetail_Read_ByPosition(viewRID);

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


        private bool IsEligibleToDisplay(int iViewRid, int iWaferCol, AllocationWaferCoordinate waferCoordinate, string sLblStore, AllocationWaferCoordinateList waferCoordinateList, eDataType dataType, 
            bool buildColumns, eAllocationSelectionViewType selectionViewType, out AllocationViewColumn viewColumn)
        {
            bool displayCol = false;
            int width;

            viewColumn = GetViewColumnIfExists(iWaferCol, waferCoordinate.Key.ToString(), out width);

            //Passing default values when the tuple is null.
            if (viewColumn == null) { viewColumn = new AllocationViewColumn(string.Empty, false, int.MaxValue, eSortDirection.None, 0); }

            VisiblePosition = SetColumnPositions(waferCoordinateList, viewColumn);

            displayCol = (iViewRid == Include.NoRID || waferCoordinate.Key.ToString() == sLblStore || viewColumn.IsVisible);

            AllocationWaferCoordinate variableCoord = GetAllocationCoordinate(waferCoordinateList, eAllocationCoordinateType.Variable);
            if (selectionViewType == eAllocationSelectionViewType.Style)
            {
                if (dataType == eDataType.StoreDetail || dataType == eDataType.SetDetail || dataType == eDataType.AllStoreDetail)
                {
                    if (displayCol
                        && !_selectedFieldKeys.Contains(variableCoord.Key))
                    {
                        _selectedFieldKeys.Add(variableCoord.Key);
                    }
                }
            }

            //this should be executed only when the display column is false and there should be some view ( to handle StyleAnalsysis view)
            if ((!displayCol) && (iViewRid > 0))
            {
                if (waferCoordinateList[0].CoordinateSubType == (int)eComponentType.Bulk || waferCoordinateList[0].CoordinateSubType == (int)eComponentType.SpecificColor)
                {
                    if (waferCoordinate.Key == (int)eAllocationStyleViewVariableDefault.QuantityAllocated)
                    {
                        displayCol = ChkIfMandatoryColumnsPresent(waferCoordinate);
                        VisiblePosition = SetColumnPositions(waferCoordinateList, viewColumn);
                    }
                }
                else if (waferCoordinateList[1].CoordinateSubType == (int)eComponentType.Total || waferCoordinateList[1].CoordinateSubType == 0)
                {
                    displayCol = false;
                }
            }

            if (selectionViewType == eAllocationSelectionViewType.Style
                || selectionViewType == eAllocationSelectionViewType.Velocity)
            {
                // check if total field selected to display then display corresponding component field
                if (waferCoordinateList[1].CoordinateSubType != (int)eComponentType.Total)
                {
                    if (_selectedFieldKeys.Contains(variableCoord.Key))
                    {
                        if (buildColumns)  // if field is not selected to display by the view, it is not calculated, so calculate values
                        {
                            _applicationSessionTransaction.BuildWaferColumnsAdd(0, (eAllocationWaferVariable)variableCoord.Key);
                            _applicationSessionTransaction.BuildWaferColumnsAdd(1, (eAllocationWaferVariable)variableCoord.Key);
                            _applicationSessionTransaction.BuildWaferColumnsAdd(2, (eAllocationWaferVariable)variableCoord.Key);
                        }
                        displayCol = true;
                    }
                }
            }
            else if (selectionViewType == eAllocationSelectionViewType.Summary)
            {
                if (displayCol)
                {
                    VisiblePosition = _currentVisiblePosition;
                    ++_currentVisiblePosition;
                }
            }

            return displayCol;
        }

        private int SetColumnPositions(AllocationWaferCoordinateList waferCoordinateList, AllocationViewColumn viewColumn)
        {
            int colPosition, colKey;

            colKey = waferCoordinateList[1].CoordinateSubType;

            if (viewColumn != null)
            { colPosition = viewColumn.VisiblePosition; }
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

                if (eAllocationSelectionViewType.Style == selectionViewType
                    || eAllocationSelectionViewType.Velocity == selectionViewType)
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

                if (_viewHasBeenUpdated)
                {
                    isViewchanged = true;
                }
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
            eLayoutID layoutID = GetLayoutID(allocationSelectionType: selectionViewType);

            int viewRID = _userGridView.UserGridView_Read(SAB.ClientServerSession.UserRID, layoutID);

            if (viewRID == Include.NoRID)
            {
                if (selectionViewType == eAllocationSelectionViewType.Velocity)
                {
                    viewRID = Include.DefaultVelocityDetailViewRID;
                }
                else if (selectionViewType == eAllocationSelectionViewType.Style)
                {
                    viewRID = Include.DefaultStyleViewRID;
                }
                else if (selectionViewType == eAllocationSelectionViewType.Size)
                {
                    viewRID = Include.DefaultSizeViewRID;
                }
            }

            return viewRID;
        }

        public ROOut GetViewDetails()
        {
            List<ROSelectedField> columns = null;
            List<string> columnList = new List<string>();

            ROAllocationReviewViewDetails viewDetails = new ROAllocationReviewViewDetails(view: GetName.GetAllocationViewName(key: _currentViewRID));
            if (_currentViewRID > 0)
            {
                DataRow row = _gridViewData.GridView_Read(_currentViewRID);
                if (row != null)
                {
                    int userRID = Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture);
                    if (userRID != Include.GlobalUserRID)
                    {
                        viewDetails.IsUserView = true;
                    }
                    int groupBy = Convert.ToInt32(row["GROUP_BY"], CultureInfo.CurrentUICulture);
                    if (groupBy != 0)
                    {
                        viewDetails.GroupBy = groupBy;
                    }
                }
            }

            if (!_columnsList.TryGetValue(eDataType.StoreSummary, out columns))
            {
                columns = new List<ROSelectedField>();
            }
            foreach (ROSelectedField field in columns)
            {
                if (columnList.Contains(field.Field.Key))
                {
                    continue;
                }
                columnList.Add(field.Field.Key);
                viewDetails.SummaryColumns.Add(field);
            }

            columnList.Clear();
            if (!_columnsList.TryGetValue(eDataType.StoreTotals, out columns))
            {
                columns = new List<ROSelectedField>();
            }
            foreach (ROSelectedField field in columns)
            {
                if (columnList.Contains(field.Field.Key))
                {
                    continue;
                }
                columnList.Add(field.Field.Key);
                viewDetails.TotalColumns.Add(field);
            }

            columnList.Clear();
            if (!_columnsList.TryGetValue(eDataType.StoreDetail, out columns))
            {
                columns = new List<ROSelectedField>();
            }
            foreach (ROSelectedField field in columns)
            {
                if (columnList.Contains(field.Field.Key))
                {
                    continue;
                }
                columnList.Add(field.Field.Key);
                viewDetails.DetailColumns.Add(field);
            }

            // Get splitter percentages for the view
            List<double> verticalSplitterPercentages, horizontalSplitterPercentages;

            GetSplitterPercentages(
                viewKey: _currentViewRID,
                userKey: SAB.ClientServerSession.UserRID,
                layoutID: GetLayoutID(allocationSelectionType: _applicationSessionTransaction.AllocationViewType),
                verticalSplitterPercentages: out verticalSplitterPercentages,
                horizontalSplitterPercentages: out horizontalSplitterPercentages
                );

            foreach (double splitterPercentage in verticalSplitterPercentages)
            {
                viewDetails.VerticalSplitterPercentages.Add(splitterPercentage);
            }

            foreach (double splitterPercentage in horizontalSplitterPercentages)
            {
                viewDetails.HorizontalSplitterPercentages.Add(splitterPercentage);
            }

            return new ROAllocationReviewViewDetailsOut(eROReturnCode.Successful, null, ROInstanceID, viewDetails);
        }

        private void GetSplitterPercentages(
            int viewKey,
            int userKey,
            eLayoutID layoutID,
            out List<double> verticalSplitterPercentages,
            out List<double> horizontalSplitterPercentages
            )
        {
            char splitterTypeIndicator;
            double splitterPercentage;
            verticalSplitterPercentages = new List<double>();
            horizontalSplitterPercentages = new List<double>();

            DataTable _viewSplitter = _gridViewData.GridViewSplitter_Read(
                viewRID: viewKey,
                userRID: userKey,
                layoutID: layoutID);

            if (_viewSplitter.Rows.Count == 0)  // set defaults
            {
                switch (layoutID)
                {
                    case eLayoutID.styleReviewGrid:
                        horizontalSplitterPercentages.Add(80);
                        horizontalSplitterPercentages.Add(80);
                        verticalSplitterPercentages.Add(30);
                        verticalSplitterPercentages.Add(30);

                        break;

                    case eLayoutID.sizeReviewGrid:
                        horizontalSplitterPercentages.Add(80);
                        horizontalSplitterPercentages.Add(80);
                        verticalSplitterPercentages.Add(30);
                        verticalSplitterPercentages.Add(30);

                        break;

                }
            }
            else
            {
                foreach (DataRow row in _viewSplitter.Rows)
                {
                    splitterTypeIndicator = Convert.ToChar(row["SPLITTER_TYPE_IND"], CultureInfo.CurrentUICulture);
                    splitterPercentage = Convert.ToDouble(row["SPLITTER_PERCENTAGE"], CultureInfo.CurrentUICulture);
                    if (splitterTypeIndicator == 'V')
                    {
                        verticalSplitterPercentages.Add(splitterPercentage);
                    }
                    else
                    {
                        horizontalSplitterPercentages.Add(splitterPercentage);
                    }
                }
            }
        }

        public ROOut SaveViewDetails(ROAllocationReviewViewDetailsParms viewDetails)
        {
            eROReturnCode returnCode = eROReturnCode.Successful;
            bool success = true;
            string message = null;
            int viewRID = Include.NoRID, viewUserRID;
            int visiblePosition, sortDirection, sortSequence, width;
            bool isHidden, isGroupByCol;
            string columnType;
            int hcg_RID;
            GridViewData gridViewData = new GridViewData();
            eLayoutID layoutID;
            List<string> columnList = new List<string>();

            try
            {
                // make sure at least one variable is selected
                bool variableIsSelected = false;
                foreach (ROSelectedField field in viewDetails.ROAllocationReviewViewDetails.SummaryColumnsByPosition)
                {
                    if (field.IsSelected)
                    {
                        variableIsSelected = true;
                        break;
                    }
                }

                if (!variableIsSelected)
                {
                    return new ROAllocationReviewViewDetailsOut(
                        ROReturnCode: eROReturnCode.Failure,
                        sROMessage: SAB.ClientServerSession.Audit.GetText(messageCode: eMIDTextCode.msg_NeedAtLeastOneVariable, addToAuditReport: true),
                        ROInstanceID: ROInstanceID,
                        ROAllocationReviewViewDetails: viewDetails.ROAllocationReviewViewDetails);
                }

                if (viewDetails.ROAllocationReviewViewDetails.IsUserView)
                {
                    viewUserRID = SAB.ClientServerSession.UserRID;
                }
                else
                {
                    viewUserRID = Include.GlobalUserRID;
                }

                layoutID = eLayoutID.NotDefined;
                bool isSequential = false;
                int secondaryGroupBy = Include.NoRID;
                if (_applicationSessionTransaction.AllocationViewType == eAllocationSelectionViewType.Velocity)
                {
                    layoutID = eLayoutID.velocityStoreDetailGrid;
                }
                else if (_applicationSessionTransaction.AllocationViewType == eAllocationSelectionViewType.Style)
                {
                    layoutID = eLayoutID.styleReviewGrid;
                }
                else if (_applicationSessionTransaction.AllocationViewType == eAllocationSelectionViewType.Size)
                {
                    layoutID = eLayoutID.sizeReviewGrid;
                    isSequential = viewDetails.ROAllocationReviewViewDetails.IsSequential;
                    secondaryGroupBy = viewDetails.ROAllocationReviewViewDetails.SecondaryGroupBy;
                }

                gridViewData.OpenUpdateConnection();

                try
                {
                    viewRID = gridViewData.GridView_GetKey(viewUserRID, (int)layoutID, viewDetails.ROAllocationReviewViewDetails.View.Value);
                    if (viewRID != Include.NoRID)
                    {
                        gridViewData.GridViewDetail_Delete(viewRID);
                        gridViewData.GridView_Update(viewRID, true, viewDetails.ROAllocationReviewViewDetails.GroupBy, secondaryGroupBy, isSequential, Include.NoRID, false);
                    }
                    else
                    {
                        viewRID = gridViewData.GridView_Insert(viewUserRID, (int)layoutID, viewDetails.ROAllocationReviewViewDetails.View.Value, false, viewDetails.ROAllocationReviewViewDetails.GroupBy, secondaryGroupBy, isSequential, Include.NoRID, false);
                        string viewName = viewDetails.ROAllocationReviewViewDetails.View.Value;
                        viewDetails.ROAllocationReviewViewDetails.View = new KeyValuePair<int, string>(viewRID, viewName);
                    }

                    columnType = "B";
                    hcg_RID = Include.NoRID;
                    
                    isGroupByCol = false;
                    sortSequence = -1;
                    visiblePosition = 0;

                    foreach (ROSelectedField field in viewDetails.ROAllocationReviewViewDetails.SummaryColumnsByPosition)
                    {
                        if (field.Field.Key == "Store")
                        {
                            isHidden = false;  // Store field is always displayed
                        }
                        else
                        {
                            isHidden = !field.IsSelected;
                        }
                        sortDirection = (int)field.SortDirection;
                        // if key is already in the view for this grid, go to the next field
                        if (columnList.Contains(field.Field.Key))
                        {
                            continue;
                        }
                        columnList.Add(field.Field.Key);
                        gridViewData.GridViewDetail_Insert(viewRID, "g1", field.Field.Key, visiblePosition, isHidden,
                                                  isGroupByCol, sortDirection, sortSequence, field.Width,
                                                  columnType, hcg_RID);
                        ++visiblePosition;
                    }

                    visiblePosition = 0;
                    columnList.Clear();
                    foreach (ROSelectedField field in viewDetails.ROAllocationReviewViewDetails.TotalColumnsByPosition)
                    {
                        isHidden = !field.IsSelected;
                        sortDirection = (int)field.SortDirection;
                        // if key is already in the view for this grid, go to the next field
                        if (columnList.Contains(field.Field.Key))
                        {
                            continue;
                        }
                        columnList.Add(field.Field.Key);
                        gridViewData.GridViewDetail_Insert(viewRID, "g2", field.Field.Key, visiblePosition, isHidden,
                                                  isGroupByCol, sortDirection, sortSequence, field.Width,
                                                  columnType, hcg_RID);
                        ++visiblePosition;
                    }

                    visiblePosition = 0;
                    columnList.Clear();
                    foreach (ROSelectedField field in viewDetails.ROAllocationReviewViewDetails.DetailColumnsByPosition)
                    {
                        isHidden = !field.IsSelected;
                        sortDirection = (int)field.SortDirection;
                        // if key is already in the view for this grid, go to the next field
                        if (columnList.Contains(field.Field.Key))
                        {
                            continue;
                        }
                        columnList.Add(field.Field.Key);
                        gridViewData.GridViewDetail_Insert(viewRID, "g3", field.Field.Key, visiblePosition, isHidden,
                                                  isGroupByCol, sortDirection, sortSequence, field.Width,
                                                  columnType, hcg_RID);
                        ++visiblePosition;
                    }

                    // save splitter settings
                    if (!SaveViewSplitters(
                       userKey: SAB.ClientServerSession.UserRID,
                       viewKey: viewRID,
                       layoutID: layoutID,
                       viewDetails: viewDetails,
                       gridViewData: gridViewData,
                       message: out message
                       ))
                    {
                        success = false;
                        returnCode = eROReturnCode.Failure;
                    }

                    // commit the new values to the database
                    if (success)
                    {
                        gridViewData.CommitData();
                    }
                }
                catch (Exception exc)
                {
                    gridViewData.Rollback();
                    message = exc.ToString();
                    throw;
                }
                finally
                {
                    gridViewData.CloseUpdateConnection();
                    if (viewRID == _currentViewRID)
                    {
                        _viewHasBeenUpdated = true;
                    }
                }
            }
            catch (Exception exc)
            {
                message = exc.ToString();
                throw;
            }


            return new ROAllocationReviewViewDetailsOut(returnCode, message, ROInstanceID, viewDetails.ROAllocationReviewViewDetails);
        }

        /// <summary>
        /// Saves splitter percentages to the database
        /// </summary>
        /// <param name="userKey">The key of the user for the view</param>
        /// <param name="viewKey">The key of the view</param>
        /// <param name="layoutID">The layout ID for splitters</param>
        /// <param name="viewDetails">An instance of the ROAllocationReviewViewDetailsParms containing view formatting</param>
        /// <param name="gridViewData">The data layer for views</param>
        /// <param name="message">A message during processing</param>
        /// <returns></returns>
        protected bool SaveViewSplitters(
            int userKey,
            int viewKey,
            eLayoutID layoutID,
            ROAllocationReviewViewDetailsParms viewDetails,
            GridViewData gridViewData,
            out string message
            )
        {
            bool success = true;
            int splitterSequence = 0;
            message = null;

            try
            {
                // delete current settings
                gridViewData.GridViewSplitter_Delete(
                    viewRID: viewKey,
                    userRID: userKey,
                    layoutID: layoutID
                    );

                // add each horizontal splitter
                foreach (double splitterPercentage in viewDetails.ROAllocationReviewViewDetails.HorizontalSplitterPercentages)
                {
                    // if row exists, splitterPercentage will be updated
                    gridViewData.GridViewSplitter_Insert(
                        viewRID: viewKey,
                        userRID: userKey,
                        layoutID: layoutID,
                        splitterTypeIndicator: 'H',
                        splitterSequence: splitterSequence,
                        splitterPercentage: splitterPercentage
                        );

                    ++splitterSequence;
                }

                // add each vertical splitter
                splitterSequence = 0;
                foreach (double splitterPercentage in viewDetails.ROAllocationReviewViewDetails.VerticalSplitterPercentages)
                {
                    // if row exists, splitterPercentage will be updated
                    gridViewData.GridViewSplitter_Insert(
                        viewRID: viewKey,
                        userRID: userKey,
                        layoutID: layoutID,
                        splitterTypeIndicator: 'V',
                        splitterSequence: splitterSequence,
                        splitterPercentage: splitterPercentage
                        );

                    ++splitterSequence;
                }
            }
            catch (Exception exc)
            {
                success = false;
                message = exc.ToString();
            }

            return success;
        }

        public ROOut DeleteReviewViewDetails()
        {
            return DeleteViewDetails(viewKey: _currentViewRID);
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
                if (!_fromAssortment
                    && !_applicationSessionTransaction.AllocationCriteriaExists)
                {
                    _applicationSessionTransaction.CreateAllocationViewSelectionCriteria();
                    _applicationSessionTransaction.NewCriteriaHeaderList();
                }

                if (reviewOptionsParms.IsVelocity)
                {
                    _applicationSessionTransaction.AllocationStoreAttributeID = _applicationSessionTransaction.VelocityStoreGroupRID;
                }

                _applicationSessionTransaction.AllocationViewType = selectionViewType;
                _applicationSessionTransaction.AllocationNeedAnalysisPeriodBeginRID = Include.NoRID;
                _applicationSessionTransaction.AllocationNeedAnalysisPeriodEndRID = Include.NoRID;
                _applicationSessionTransaction.AllocationNeedAnalysisHNID = Include.NoRID;

                if (reviewOptionsParms.View.Key == 0)
                {
                    if (reviewOptionsParms.IsVelocity)
                    {
                        reviewOptionsParms.View = new KeyValuePair<int, string>(_userGridView.UserGridView_Read(SAB.ClientServerSession.UserRID, eLayoutID.velocityStoreDetailGrid), "From DB");
                    }
                    else
                    {
                        reviewOptionsParms.View = new KeyValuePair<int, string>(_userGridView.UserGridView_Read(SAB.ClientServerSession.UserRID, eLayoutID.styleReviewGrid), "From DB");
                    }
                }

                _allocationHeaderProfileList = (AllocationHeaderProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AllocationHeader);
                _selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();

                if (_selectedHeaderList.Count == 0 && !_applicationSessionTransaction.ContainsGroupAllocationHeaders())
                {
                    _sROMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoHeaderSelectedOnWorkspace);
                }

                if (!_headersLocked)
                {
                    if (!_applicationSessionTransaction.VelocityCriteriaExists)
                    {
                        CheckSecurityEnqueue(selectionViewType);
                        _applicationSessionTransaction.SetCriteriaHeaderList(_allocationHeaderProfileList);
                    }
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

                if (viewRID != _currentViewRID
                    || _viewHasBeenUpdated)
                {
                    _applicationSessionTransaction.BuildWaferColumns.Clear();
                    if (reviewOptionsParms.IsVelocity)
                    {
                        foreach (int variable in Enum.GetValues(typeof(eAllocationVelocityViewVariableDefault)))
                        {
                            CheckVariableBuiltArrayList(variable, builtVariables);
                        }
                    }
                    else
                    {
                        foreach (int variable in Enum.GetValues(typeof(eAllocationStyleViewVariableDefault)))
                        {
                            CheckVariableBuiltArrayList(variable, builtVariables);
                        }
                    }

                    AddViewColumns(viewRID, builtVariables);
                    _currentViewRID = viewRID;
                    _viewHasBeenUpdated = false;
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

                if (rebuildWafers
                    && _wafersBuilt)
                {
                    _applicationSessionTransaction.RebuildWafers();
                    _wafers = _applicationSessionTransaction.AllocationWafers;
                }
                else
                {
                    if (_wafers == null)
                    {
                        _wafers = _applicationSessionTransaction.AllocationWafers;
                        _wafersBuilt = true;
                    }
                }

                if (eAllocationSelectionViewType.Size == selectionViewType)
                {
                    return FormatGridsWithApplyView(_wafers, reviewOptionsParms, selectionViewType, true, reviewOptionsParms.View.Key);
                }
                else if (eAllocationSelectionViewType.Style == selectionViewType
                    || eAllocationSelectionViewType.Velocity == selectionViewType)
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
            if (selectionViewType == eAllocationSelectionViewType.Style
                || selectionViewType == eAllocationSelectionViewType.Velocity)
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

                // if variable is selected fields, show the column
                AllocationWaferCoordinate variableCoord = GetAllocationCoordinate(waferCoordList, eAllocationCoordinateType.Variable);
                if (_selectedFieldKeys.Contains(variableCoord.Key))
                {
                    return true;
                }

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
            List<ROSelectedField> columns = null;
            bool isDisplayed;
            bool compFound;
            AllocationViewColumn viewColumn;

            ROData roData = new ROData();
            eDataType dataType = eDataType.None;
            eDataType[,] dataTypeList = new eDataType[3, 3] {
                { eDataType.StoreSummary, eDataType.StoreTotals, eDataType.StoreDetail },
                { eDataType.SetSummary, eDataType.SetTotals, eDataType.SetDetail },
                { eDataType.AllStoreSummary, eDataType.AllStoreTotals, eDataType.AllStoreDetail }
            };

            _columnsList = new Dictionary<eDataType, List<ROSelectedField>>();

            if (aViewRID > 0) { ApplyView(aViewRID); }

            string lblStore = MIDText.GetTextOnly(eMIDTextCode.lbl_StoreSingular);
            _removedPctTotalRows = false;


            for (int waferRow = 0; waferRow < _wafers.RowCount; waferRow++)
            {
                for (int waferCol = 0; waferCol < _wafers.ColumnCount; waferCol++)
                {
                    _selectedFieldKeys = new List<int>();
                    AllocationWafer wafer = _wafers[waferRow, waferCol];
                    dataType = dataTypeList[waferRow, waferCol];
                    ROCells cells = new ROCells();

                    if (dataType == eDataType.StoreSummary
                        || dataType == eDataType.StoreTotals
                        || dataType == eDataType.StoreDetail
                        )
                    {
                        columns = new List<ROSelectedField>();
                        _columnsList.Add(dataType, columns);

                        // Add store column
                        if (dataType == eDataType.StoreSummary)
                        {
                            int width;
                            string fieldKey;
                            // view keys are held differently for Size Review and Style Review
                            if (selectionViewType == eAllocationSelectionViewType.Size)
                            {
                                viewColumn = GetViewColumnIfExists(0, eMIDTextCode.lbl_StoreSingular.GetHashCode().ToString(), out width);
                                fieldKey = eMIDTextCode.lbl_StoreSingular.GetHashCode().ToString();
                            }
                            else
                            {
                                viewColumn = GetViewColumnIfExists(0, _lblStore, out width);
                                fieldKey = _lblStore;
                            }
                            if (viewColumn == null) { viewColumn = new AllocationViewColumn(string.Empty, false, 0, eSortDirection.None, 0); }
                            columns.Add(new ROSelectedField(fieldkey: fieldKey, field: "Channel", selected: true, sortDirection: viewColumn.SortDirection, width: viewColumn.Width, visiblePosition: viewColumn.VisiblePosition));
                        }
                    }

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
                    _currentVisiblePosition = 0;

                    for (int k = 0; k < columnLength; k++)
                    {
                        string columnName = null;
                        waferCoordlist = (AllocationWaferCoordinateList)wafer.Columns[k];
                        waferCoord = GetAllocationCoordinate(waferCoordlist, eAllocationCoordinateType.Variable);
                        isDisplayed = false;
                        if (IsEligibleToDisplay(aViewRID, waferCol, waferCoord, lblStore, waferCoordlist, dataType, true, selectionViewType, out viewColumn))
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
                                DisplayedInWindows, k, viewColumn.Width));
                            isDisplayed = true;
                            indexValue = indexValue + 1;
                        }

                        // add column objects for view definition
                        if (dataType == eDataType.StoreSummary)
                        {
                            if (waferCoord.Label == _lblStore)
                            {
                                columns.Add(new ROSelectedField(fieldkey: waferCoord.Label, field: waferCoord.Label, selected: true, sortDirection: viewColumn.SortDirection, width: viewColumn.Width, visiblePosition: viewColumn.VisiblePosition));
                            }
                            else
                            {
                                columns.Add(new ROSelectedField(fieldkey: waferCoord.Key.ToString(), field: waferCoord.Label, selected: isDisplayed, sortDirection: viewColumn.SortDirection, width: viewColumn.Width, visiblePosition: viewColumn.VisiblePosition));
                            }
                        }
                        else if (dataType == eDataType.StoreTotals)
                        {
                            if (selectionViewType == eAllocationSelectionViewType.Size)
                            {
                                columns.Add(new ROSelectedField(fieldkey: waferCoord.Key.ToString(), field: waferCoord.Label, selected: isDisplayed, sortDirection: viewColumn.SortDirection, width: viewColumn.Width, visiblePosition: viewColumn.VisiblePosition));
                            }
                            else
                            {
                                int waferCoordKey = waferCoord.Key;
                                waferCoord = waferCoordlist[_compRow];
                                if (waferCoord.CoordinateSubType != (int)eComponentType.Bulk
                                    && waferCoord.CoordinateSubType != (int)eComponentType.SpecificPack
                                    && waferCoord.CoordinateSubType != (int)eComponentType.SpecificColor
                                    && waferCoord.CoordinateSubType != (int)eComponentType.SpecificSize)
                                {
                                    compFound = false;
                                    foreach (ROSelectedField field in columns)
                                    {
                                        if (field.Field.Value == waferCoord.Label)
                                        {
                                            compFound = true;
                                            break;
                                        }
                                    }
                                    if (!compFound)
                                    {
                                        columns.Add(new ROSelectedField(fieldkey: waferCoordKey.ToString(), field: waferCoord.Label, selected: isDisplayed, sortDirection: viewColumn.SortDirection, width: viewColumn.Width, visiblePosition: viewColumn.VisiblePosition));
                                    }
                                }
                            }
                        }
                        else if (dataType == eDataType.StoreDetail)
                        {
                            int waferCoordKey = waferCoord.Key;
                            //if (selectionViewType == eAllocationSelectionViewType.Style
                            //    || selectionViewType == eAllocationSelectionViewType.Velocity)
                            //{
                            //    waferCoord = waferCoordlist[_compRow];
                            //}
                            if (waferCoord.CoordinateSubType != (int)eComponentType.Bulk
                                && waferCoord.CoordinateSubType != (int)eComponentType.SpecificPack
                                && waferCoord.CoordinateSubType != (int)eComponentType.SpecificColor
                                && waferCoord.CoordinateSubType != (int)eComponentType.SpecificSize)
                            {
                                compFound = false;
                                foreach (ROSelectedField field in columns)
                                {
                                    if (field.Field.Value == waferCoord.Label)
                                    {
                                        compFound = true;
                                        break;
                                    }
                                }
                                if (!compFound)
                                {
                                    columns.Add(new ROSelectedField(fieldkey: waferCoordKey.ToString(), field: waferCoord.Label, selected: isDisplayed, sortDirection: viewColumn.SortDirection, width: viewColumn.Width, visiblePosition: viewColumn.VisiblePosition));
                                }
                            }
                        }
                    }

                    if (selectionViewType == eAllocationSelectionViewType.Size
                        && (dataType == eDataType.StoreSummary || dataType == eDataType.SetSummary || dataType == eDataType.AllStoreSummary))
                    {
                        AddSizeLabelColumns(cells, reviewOptionsParms, dataType);
                        if (dataType == eDataType.StoreSummary)
                        {
                            // add size columns
                            int variableKey = Include.Undefined;
                            string lblHeader = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Header);
                            string lblColor = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Color);
                            string lblVariable = MIDText.GetTextOnly(eMIDTextCode.lbl_Variable);
                            string lblDimension = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Dimension);
                            foreach (ROColumnAttributes columnAttributes in cells.Columns)
                            {
                                variableKey = Include.Undefined;
                                if (columnAttributes.Label == lblHeader)
                                {
                                    variableKey = (int)eMIDTextCode.lbl_Header;
                                }
                                else if (columnAttributes.Label == lblColor)
                                {
                                    variableKey = (int)eMIDTextCode.lbl_Color;
                                }
                                else if (columnAttributes.Label == lblVariable)
                                {
                                    variableKey = (int)eMIDTextCode.lbl_Variable;
                                }
                                else if (columnAttributes.Label == lblDimension)
                                {
                                    variableKey = (int)eMIDTextCode.lbl_Dimension;
                                }

                                if (variableKey != Include.Undefined)
                                {
                                    int width;
                                    viewColumn = GetViewColumnIfExists(0, variableKey.ToString(), out width);
                                    if (viewColumn == null)
                                    {
                                        viewColumn = new AllocationViewColumn(colKey: variableKey.ToString(),
                                            isVisible: true,
                                            sortDirection: eSortDirection.None,
                                            width: width,
                                            visiblePosition: int.MaxValue
                                            );
                                    }
                                    columns.Add(new ROSelectedField(fieldkey: variableKey.ToString(), field: columnAttributes.Label, selected: viewColumn.IsVisible, sortDirection: viewColumn.SortDirection, width: viewColumn.Width, visiblePosition: viewColumn.VisiblePosition));
                                }
                            }
                        }
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
                            if (IsEligibleToDisplay(aViewRID, waferCol, waferCoord, lblStore, waferCoordlist, dataType, false, selectionViewType, out viewColumn))
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

            // Get splitter percentages for the view
            List<double> verticalSplitterPercentages, horizontalSplitterPercentages;

            GetSplitterPercentages(
                viewKey: _currentViewRID,
                userKey: SAB.ClientServerSession.UserRID,
                layoutID: GetLayoutID(allocationSelectionType: selectionViewType),
                verticalSplitterPercentages: out verticalSplitterPercentages,
                horizontalSplitterPercentages: out horizontalSplitterPercentages
                );

            foreach (double splitterPercentage in verticalSplitterPercentages)
            {
                roData.VerticalSplitterPercentages.Add(splitterPercentage);
            }

            foreach (double splitterPercentage in horizontalSplitterPercentages)
            {
                roData.HorizontalSplitterPercentages.Add(splitterPercentage);
            }

            allocationReviewData = roData;
            return roData;
        }

        private eLayoutID GetLayoutID (eAllocationSelectionViewType allocationSelectionType)
        {
            eLayoutID layoutID = eLayoutID.NotDefined;
            if (allocationSelectionType == eAllocationSelectionViewType.Style)
            { layoutID = eLayoutID.styleReviewGrid; }
            else if (allocationSelectionType == eAllocationSelectionViewType.Velocity)
            { layoutID = eLayoutID.velocityStoreDetailGrid; }
            else if (allocationSelectionType == eAllocationSelectionViewType.Size)
            { layoutID = eLayoutID.sizeReviewGrid; }
            else if (allocationSelectionType == eAllocationSelectionViewType.Summary)
            { layoutID = eLayoutID.sizeReviewGrid; }

            return layoutID;
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

            AllocationViewColumn viewColumn;
            int width = 100;
            // use max so sorts to the end column
            // Header is always displayed
            if ((eAllocationSizeViewGroupBy)reviewOptionsParms.GroupBy == eAllocationSizeViewGroupBy.Header)
            {
                viewColumn = GetViewColumnIfExists(0, eMIDTextCode.lbl_Header.GetHashCode().ToString(), out width);
                cells.Columns.Add(new ROColumnAttributes(lblHeader, viewColumn.VisiblePosition, viewColumn.VisiblePosition, int.MaxValue, true, 0, width));
                k++;
                viewColumn = GetViewColumnIfExists(0, eMIDTextCode.lbl_Color.GetHashCode().ToString(), out width);
                cells.Columns.Add(new ROColumnAttributes(lblColor, viewColumn.VisiblePosition, viewColumn.VisiblePosition, int.MaxValue, true, 0, width));
            }
            else
            {
                viewColumn = GetViewColumnIfExists(0, eMIDTextCode.lbl_Color.GetHashCode().ToString(), out width);
                cells.Columns.Add(new ROColumnAttributes(lblColor, viewColumn.VisiblePosition, viewColumn.VisiblePosition, int.MaxValue, true, 0, width));
                k++;
                viewColumn = GetViewColumnIfExists(0, eMIDTextCode.lbl_Header.GetHashCode().ToString(), out width);
                cells.Columns.Add(new ROColumnAttributes(lblHeader, viewColumn.VisiblePosition, viewColumn.VisiblePosition, int.MaxValue, true, 0, width));

            }
            if (!reviewOptionsParms.ViewIsSequential)
            {
                k++;
                viewColumn = GetViewColumnIfExists(0, eMIDTextCode.lbl_Dimension.GetHashCode().ToString(), out width);
                cells.Columns.Add(new ROColumnAttributes(lblDimension, viewColumn.VisiblePosition, viewColumn.VisiblePosition, int.MaxValue, true, 0, width));
            }
            k++;
            viewColumn = GetViewColumnIfExists(0, eMIDTextCode.lbl_Variable.GetHashCode().ToString(), out width);
            cells.Columns.Add(new ROColumnAttributes(lblVariable, viewColumn.VisiblePosition, viewColumn.VisiblePosition, int.MaxValue, true, 0, width));
        }

        private void AddSizeLabelValues(ROCells cells, ROAllocationReviewOptionsParms reviewOptionsParms, AllocationWafer wafer, eDataType dataType)
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

            // RowLabels always contains all columns even if not to be displayed
            // The order is different based on the Group By
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
                            if (col.Label == lblHeader)
                            {
                                if ((eAllocationSizeViewGroupBy)reviewOptionsParms.GroupBy == eAllocationSizeViewGroupBy.Header)
                                {
                                    rowLabelCol = 1;
                                }
                                else
                                {
                                    rowLabelCol = 2;
                                }
                            }
                            else if (col.Label == lblColor)
                            {
                                if ((eAllocationSizeViewGroupBy)reviewOptionsParms.GroupBy == eAllocationSizeViewGroupBy.Header)
                                {
                                    rowLabelCol = 2;
                                }
                                else
                                {
                                    rowLabelCol = 1;
                                }
                            }
                            else if (col.Label == lblDimension)
                            {
                                // only include dimension if not sequential
                                if (reviewOptionsParms.ViewIsSequential)
                                {
                                    rowLabelCol = 999;
                                }
                                else
                                {
                                    rowLabelCol = 3;
                                }
                            }
                            else if (col.Label == lblVariable)
                            {
                                if (reviewOptionsParms.ViewIsSequential)
                                {
                                    rowLabelCol = 3;
                                }
                                else
                                {
                                    rowLabelCol = 4;
                                }
                            }

                            string rowLabel = string.Empty;
                            if (rowLabelCol < RowLabels.GetLength(1))
                            {
                                rowLabel = RowLabels[row, rowLabelCol];
                            }

                            cell = new ROCell(eCellDataType.Label, rowLabel);
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

        private AllocationViewColumn GetViewColumnIfExists(int waferCol, string coordKey, out int width)
        {
            width = Include.DefaultColumnWidth;
            AllocationViewColumn viewColumn2 = null;
            if (_viewColumns.TryGetValue("g" + (waferCol + 1), out Dictionary<string, AllocationViewColumn> gridKeyEntry))
            {
                if (gridKeyEntry.TryGetValue(coordKey, out AllocationViewColumn columnView))
                {
                    viewColumn2 = columnView;
                    viewColumn2 = new AllocationViewColumn(columnView.ColKey, columnView.IsVisible, columnView.VisiblePosition, columnView.SortDirection, columnView.Width);
                }
            }
            if (viewColumn2 != null)
            {
                width = viewColumn2.Width;
            }
            return viewColumn2;
        }


        private void ApplyView(int aViewRID)
        {
            try
            {
                Dictionary<string, AllocationViewColumn> gridKeyEntry;
                int visiblePosition = 0, width = 0;
                string gridKey, colKey, errMessage;
                bool isHidden;
                eSortDirection sortDirection;

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

                _viewColumns.Clear();
                //string sTakeVisibleColumns = "IS_HIDDEN=0";
                //foreach (DataRow row in dtGridViewDetail.Select(sTakeVisibleColumns))
                foreach (DataRow row in dtGridViewDetail.Rows)
                {
                    gridKey = Convert.ToString(row["BAND_KEY"], CultureInfo.CurrentUICulture);
                    colKey = Convert.ToString(row["COLUMN_KEY"], CultureInfo.CurrentUICulture);
                    visiblePosition = Convert.ToInt32(row["VISIBLE_POSITION"], CultureInfo.CurrentUICulture);
                    isHidden = Include.ConvertCharToBool(Convert.ToChar(row["IS_HIDDEN"], CultureInfo.CurrentUICulture));
                    sortDirection = (eSortDirection)Convert.ToInt32(row["SORT_DIRECTION"], CultureInfo.CurrentUICulture);
                    width = Convert.ToInt32(row["WIDTH"], CultureInfo.CurrentUICulture);

                    if (!_viewColumns.TryGetValue(gridKey, out gridKeyEntry))
                    {
                        gridKeyEntry = new Dictionary<string, AllocationViewColumn>();
                        _viewColumns.Add(gridKey, gridKeyEntry);

                    }

                    var viewColumn = new AllocationViewColumn(colKey, !isHidden, visiblePosition, sortDirection, width);
                    if (!gridKeyEntry.Keys.Contains(colKey)) { gridKeyEntry.Add(colKey, viewColumn); }
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
