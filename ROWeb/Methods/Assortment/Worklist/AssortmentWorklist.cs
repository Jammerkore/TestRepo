using Logility.ROWebSharedTypes;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace Logility.ROWeb
{
    public partial class ROAssortment : ROAllocation 
    {
        //=======
        // FIELDS
        //=======
        private UserGridView _userGridView;
        private GridViewData _gridViewData;
        private FilterData _filterData;
        private bool _fromFilterWindow = false;
        private int _headerFilterRID = Include.NoRID;
        private ArrayList _headerProfileArrayList;


        private ArrayList _masterKeyList;
        private ArrayList _selectedHeaderKeyList;
        private ArrayList _selectedAssortmentKeyList;
        private ArrayList _selectedRowsSequence = new ArrayList();
        private DataTable _dtHeader = null;
        private Hashtable _colorsForStyle = new Hashtable();
        private Hashtable _nodeDataHash = new Hashtable();
        private int _nodeDataHashLastKey = 0;
        private DataTable _assortments;
        private DataTable _placeHolders;
        private DataTable _anchorNodes;
        private int _nonCharColCount = 0;
        private Hashtable _charByGroupAndID = new Hashtable();
        private int _lastAsrtSortSeq;
        private FunctionSecurityProfile _assortmentWorkspaceSecurity;
        private Hashtable _sizeGroupHash = new Hashtable();
        private Hashtable _workflowNameHash = new Hashtable();
        private HierarchyNodeProfile _nodeDataHashLastValue;

        private KeyValueList _assortmentValueList;
        private KeyValueList _placeHolderValueList;
        private KeyValueList _headerStatusValueList;
        private KeyValueList _packTypeValueList;
        private KeyValueList _headerTypeValueList;
        private KeyValueList _sizeGroupValueList;
        private KeyValueList _headerIntransitValueList;
        private KeyValueList _headerShipStatusValueList;
        private DataSet _dsMain;
        private bool _bindingView;
        private DataTable dtView;
        private FunctionSecurityProfile _assortmentWorkspaceViewsGlobalSecurity;
        private FunctionSecurityProfile _assortmentWorkspaceViewsUserSecurity;
        private MIDRetail.Data.Header _headerDataRecord;
        private int dateRangeRid;
        private DayProfile _assortApplyToDay;

        protected AssortmentCubeGroup _asrtCubeGroup;
        private AssortmentOpenParms _openParms;
        private eAssortmentWindowType _windowType;
        private AllocationHeaderProfileList _headerList;
        private AssortmentProfile _assortmentProfile = null;
        private DataTable _dtHeaders;
        private SortedList _sortedComponentColumnHeaders;
        private ArrayList _selectableComponentColumnHeaders;
        private bool _placeholderSelected;
        private AssortmentComponentVariables _componentVariables;
        private FunctionSecurityProfile _assortmentSecurity;
        private HeaderCharGroupProfileList _headerCharGroupProfileList;
        private DataTable _dtHeaderViewFieldMapping = null;
                
        public GetMethods _getAsrtMethods;
        

        public Header HeaderDataRecord
        {
            get
            {
                if (_headerDataRecord == null)
                {
                    _headerDataRecord = new Header();
                }

                return _headerDataRecord;
            }
            set { _headerDataRecord = value; }
        }

        public DayProfile AssortmentApplyToDate
        {
            get
            {
                if (_assortApplyToDay == null)
                {
                    _assortApplyToDay = ((DayProfile)(SAB.ApplicationServerSession.Calendar.GetFirstDayOfRange(dateRangeRid)));

                }
                return _assortApplyToDay;
            }
            set { _assortApplyToDay = value; }
        }

        public GetMethods GetAsrtMethods
        {
            get
            {
                if (_getAsrtMethods == null)
                {
                    _getAsrtMethods = new GetMethods(SAB);
                }

                return _getAsrtMethods;
            }
        }

        #region AssortmentActions
        public ROOut GetAssortmentActionsInfo()
        {

            try
            {
                return new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, BuildAssortmentActions());
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        internal List<KeyValuePair<int, string>> BuildAssortmentActions()
        {
            DataTable dtActions = MIDText.GetLabels((int)eAssortmentActionType.Redo, (int)eAssortmentActionType.CreatePlaceholdersBasedOnRevenue);

            List<DataRow> rowsToDelete = new List<DataRow>();
            foreach (DataRow aRow in dtActions.Rows)
            {
                int action = int.Parse(aRow["TEXT_CODE"].ToString());
                if (!AllowAction(action, true)
                    || (!Enum.IsDefined(typeof(eAssortmentActionType), (eAssortmentActionType)action))
                    || action == (int)eAssortmentActionType.ChargeCommitted
                    || action == (int)eAssortmentActionType.CancelCommitted)
                {
                    rowsToDelete.Add(aRow);
                }
            }

            foreach (DataRow aRow in rowsToDelete)
            {
                dtActions.Rows.Remove(aRow);
            }

            DataRow selectRow = dtActions.NewRow();
            selectRow["TEXT_CODE"] = Include.NoRID;
            selectRow["TEXT_VALUE"] = "Select action...";
            dtActions.Rows.InsertAt(selectRow, 0);
            dtActions.PrimaryKey = new DataColumn[] { dtActions.Columns["TEXT_CODE"] };

            dtActions = DataTableTools.SortDataTable(dataTable: dtActions, sColName: "TEXT_ORDER", bAscending: true);

            return DataTableTools.DataTableToKeyValues(dtActions, "TEXT_CODE", "TEXT_VALUE"); ;
        }

        internal bool AllowAction(int aAction, bool bAssortmentAction)
        {
            bool allowAction = true;

            try
            {
                FunctionSecurityProfile actionSecurity;
                if (bAssortmentAction)
                {
                    actionSecurity = SAB.ClientServerSession.GetMyUserActionSecurityAssignment((eAssortmentActionType)aAction);
                    if (actionSecurity.AccessDenied)
                    {
                        allowAction = false;
                    }
                }
                else
                {
                    actionSecurity = SAB.ClientServerSession.GetMyUserActionSecurityAssignment((eAllocationActionType)aAction, true);
                    if (actionSecurity.AccessDenied)
                    {
                        allowAction = false;
                    }
                    else
                    {
                        actionSecurity = SAB.ClientServerSession.GetMyUserActionSecurityAssignment((eAllocationActionType)aAction, false);
                        if (actionSecurity.AccessDenied)
                        {
                            allowAction = false;
                        }
                    }
                }
            }
            catch
            {
                allowAction = false;
                throw;
            }
            return allowAction;
        }
        #endregion

        #region AssortmentFilters
        public ROOut GetAssortmentFiltersInfo()
        {
            try
            {
                return new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, BindAssortmentFilters());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal List<KeyValuePair<int, string>> BindAssortmentFilters()
        {
            FilterData _filterData = new FilterData();
            ArrayList userRIDList = new ArrayList();

            userRIDList.Add(Include.GlobalUserRID);
            userRIDList.Add(SAB.ClientServerSession.UserRID);

            DataTable dtAssortmentFilters = _filterData.FilterRead(filterTypes.AssortmentFilter, eProfileType.FilterAssortment, userRIDList);

            return DataTableTools.DataTableToKeyValues(dtAssortmentFilters, "FILTER_RID", "FILTER_NAME");
        }
        #endregion

        #region Worklist Headers

        public ROOut GetAssortmentHeaderData()
        {
            try
            {
                GetHeaderFilters();
                return LoadHeaderData();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return new RONoDataOut(eROReturnCode.Successful, null, ROInstanceID);
        }

        internal void GetHeaderFilters()
        {
            _userGridView = new UserGridView();
            _gridViewData = new GridViewData();
            _filterData = new FilterData();
            FilterHeaderOptions headerFilterOptions = new FilterHeaderOptions();
            headerFilterOptions.USE_WORKSPACE_FIELDS = true;
            headerFilterOptions.filterType = filterTypes.HeaderFilter;

            int viewRID = _userGridView.UserGridView_Read(SAB.ClientServerSession.UserRID, eLayoutID.assortmentWorkspaceGrid);
            bool useViewWorkspaceFilter = false;
            bool useFilterSorting = false;
            if (viewRID != Include.NoRID && !_fromFilterWindow)
            {
                int workspaceFilterRID = _gridViewData.GridViewReadWorkspaceFilterRID(viewRID, ref useFilterSorting);
                if (workspaceFilterRID != Include.NoRID)
                {
                    useViewWorkspaceFilter = true;
                    this._headerFilterRID = workspaceFilterRID;
                }
            }
            if (useViewWorkspaceFilter == false)
            {
                this._headerFilterRID = _filterData.WorkspaceCurrentFilter_Read(SAB.ClientServerSession.UserRID, eWorkspaceType.AssortmentWorkspace);
            }
        }

        internal ROOut LoadHeaderData()
        {
            try
            {
                FilterHeaderOptions headerFilterOptions = new FilterHeaderOptions();
                headerFilterOptions.USE_WORKSPACE_FIELDS = true;
                headerFilterOptions.filterType = filterTypes.AssortmentFilter;
                _headerProfileArrayList = SAB.HeaderServerSession.GetHeadersForWorkspace(this._headerFilterRID, headerFilterOptions);

                ArrayList groupAllocHdrList = new ArrayList();
                foreach (AllocationHeaderProfile ahp in _headerProfileArrayList)
                {
                    if (ahp.AsrtType == (int)eAssortmentType.GroupAllocation)
                    {
                        groupAllocHdrList.Add(ahp);
                    }
                }
                foreach (AllocationHeaderProfile ahp in groupAllocHdrList)
                {
                    _headerProfileArrayList.Remove(ahp);
                }

                _headerCharGroupProfileList = SAB.HeaderServerSession.GetHeaderCharGroups();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            ROIListOut ROHeaderSummaryList = new ROIListOut(eROReturnCode.Successful, null, ROInstanceID, BuildHeaderSummaryList(_headerProfileArrayList));
            // Add digital asset key
            Header dlAssortment = new Header();
            foreach (ROAllocationHeaderSummary ahs in ROHeaderSummaryList.ROIListOutput)
            {
                if (ahs.DigitalAssetKey <= 0)
                {
                    DataTable dtAssrtComponents = dlAssortment.GetAssormentComponents(ahs.Key);
                    foreach (DataRow assrtRow in dtAssrtComponents.Rows)
                    {
                        int hnRid = Convert.ToInt32(assrtRow["STYLE_HNRID"]);
                        HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(hnRid);
                        if (hnp.DigitalAssetKey > 0)
                        {
                            ahs.DigitalAssetKey = hnp.DigitalAssetKey;
                            break;
                        }
                    }
                }
            }
            return ROHeaderSummaryList;
        }

        internal List<ROAllocationHeaderSummary> BuildHeaderSummaryList(ArrayList headerProfileList)
        {
            List<ROAllocationHeaderSummary> headerList = new List<ROAllocationHeaderSummary>();

            _assortmentSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.Assortment);
            Hashtable sizeGroupHash = new Hashtable();
            Hashtable workflowNameHash = new Hashtable();
            Header headerData = new Header();

            foreach (AllocationHeaderProfile lstItem in _headerProfileArrayList)
            {
                string headerID = lstItem.HeaderID;
                string headerDescription = lstItem.HeaderDescription;
                DateTime headerReceiptDay = lstItem.HeaderDay;
                DateTime originalReceiptDay = lstItem.OriginalReceiptDay;
                double unitRetail = lstItem.UnitRetail;
                double unitCost = lstItem.UnitCost;
                int styleHnRID = lstItem.StyleHnRID;
                int planHnRID = lstItem.PlanHnRID;
                int onHandHnRID = lstItem.OnHandHnRID;
                string vendor = lstItem.Vendor;
                string purchaseOrder = lstItem.PurchaseOrder;
                DateTime beginDay = lstItem.BeginDay;
                DateTime shipDay = lstItem.ShipToDay;
                DateTime lastNeedDay = lstItem.LastNeedDay;
                DateTime releaseApprovedDate = lstItem.ReleaseApprovedDate;
                DateTime releaseDate = lstItem.ReleaseDate;
                DateTime earliestShipDay = lstItem.EarliestShipDay;
                int headerGroupRID = lstItem.HeaderGroupRID;
                int sizeGroupRID = lstItem.SizeGroupRID;
                int workflowRID = lstItem.WorkflowRID;
                bool workflowTrigger = lstItem.WorkflowTrigger;
                int apiWorkflowRID = lstItem.API_WorkflowRID;
                bool apiWorkflowTrigger = lstItem.API_WorkflowTrigger;
                int methodRID = lstItem.MethodRID;
                bool allocationStarted = lstItem.AllocationStarted;
                double percentNeedLimit = lstItem.PercentNeedLimit;
                double planPercentFactor = lstItem.PlanFactor;
                int reserveUnits = lstItem.ReserveUnits;
                int allocatedUnits = lstItem.AllocatedUnits;
                int origAllocatedUnits = lstItem.OrigAllocatedUnits;
                int rsvAllocatedUnits = lstItem.RsvAllocatedUnits;
                int releaseCount = lstItem.ReleaseCount;
                int gradeWeekCount = lstItem.GradeWeekCount;
                int primarySecondaryRID = lstItem.PrimarySecondaryRID;
                string distributionCenter = lstItem.DistributionCenter;
                string allocationNotes = lstItem.AllocationNotes;
                int headerTotalQtyToAllocate = lstItem.TotalUnitsToAllocate;
                int headerTotalQtyAllocated = lstItem.TotalUnitsAllocated;
                int headerTotalUnitMultiple = lstItem.AllocationMultiple;
                int headerTotalUnitMultipleDefault = lstItem.AllocationMultipleDefault;
                int unitsShipped = lstItem.UnitsShipped;
                int genericTotalQtyToAllocate = lstItem.GenericUnitsToAllocate;
                int genericTotalQtyAllocated = lstItem.GenericUnitsAllocated;
                int genericTotalUnitMultiple = lstItem.GenericMultiple;
                int detailTotalQtyToAllocate = lstItem.DetailTypeUnitsToAllocate;
                int detailTotalQtyAllocated = lstItem.DetailTypeUnitsAllocated;
                int detailTotalUnitMultiple = lstItem.DetailTypeMultiple;
                int bulkTotalQtyToAllocate = lstItem.BulkUnitsToAllocate;
                int bulkTotalQtyAllocated = lstItem.BulkUnitsAllocated;
                int bulkTotalUnitMultiple = lstItem.BulkMultiple;
                int masterRID = lstItem.MasterRID;
                string masterID = lstItem.MasterID;
                int subordinateRID = lstItem.SubordinateRID;
                List<int> subordinateRIDs = lstItem.SubordinateRIDs;
                string subordinateID = lstItem.SubordinateID;
                int storeStyleAllocationManuallyChgdCnt = lstItem.StoreStyleAllocationManuallyChangedCount;
                int storeSizeAllocationManuallyChgdCnt = lstItem.StoreSizeAllocationManuallyChangedCount;
                int storeStyleManualAllocationTotal = lstItem.StoreStyleManualAllocationTotal;
                int storeSizeManualAllocationTotal = lstItem.StoreSizeManualAllocationTotal;
                int storesWithAllocationCnt = lstItem.StoresWithAllocationCount;
                bool horizonOverride = lstItem.HorizonOverride;
                int asrtRID = lstItem.AsrtRID;
                int placeHolderRID = lstItem.PlaceHolderRID;
                int asrtType = lstItem.AsrtType;
                string nodeDisplayForOtsForecast = lstItem.NodeDisplayForOtsForecast;
                string nodeDisplayForOnHand = lstItem.NodeDisplayForOnHand;
                string nodeDisplayForGradeInvBasis = lstItem.NodeDisplayForGradeInvBasis;
                string workflowName = lstItem.WorkflowName;
                string headerMethodName = lstItem.HeaderMethodName;
                string apiWorkflowName = lstItem.APIWorkflowName;
                int gradeSG_RID = lstItem.GradeSG_RID;
                bool gradeInventoryMinMax = lstItem.GradeInventoryMinimumMaximum;
                int gradeInventoryHnRID = lstItem.GradeInventoryBasisHnRID;
                string imoID = lstItem.ImoID;
                int itemUnitsAllocated = lstItem.TotalItemUnitsAllocated;
                int itemOrigUnitsAllocated = lstItem.TotalItemOrigUnitsAllocated;
                int asrtPlaceholderSeq = lstItem.AsrtPlaceholderSeq;
                int asrtHeaderSeq = lstItem.AsrtHeaderSeq;
                int asrtUserRid = lstItem.AsrtUserRid;
                string asrtID = lstItem.AssortmentID;
                int asrtTypeForParentAsrt = lstItem.AsrtTypeForParentAsrt;
                int unitsPerCarton = lstItem.UnitsPerCarton;
                bool DCFulfillmentProcessed = lstItem.DCFulfillmentProcessed;
                int aKey = lstItem.Key;

                //Added new fields based on the request by John and Vickie
                eHeaderType headerType = lstItem.HeaderType;
                string headerTypeText = MIDText.GetTextOnly(Convert.ToInt32(lstItem.HeaderType));
                bool canView = false;
                bool canUpdate = false;
                switch (headerType)
                {
                    case eHeaderType.Assortment:
                    case eHeaderType.Placeholder:
                        canUpdate = _assortmentSecurity.AllowUpdate;
                        canView = _assortmentSecurity.AllowView;
                        break;

                    default:
                        HierarchyNodeSecurityProfile securityNode = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(lstItem.StyleHnRID, (int)eSecurityTypes.Allocation);
                        canUpdate = securityNode.AllowUpdate;
                        canView = securityNode.AllowView;
                        break;
                }


                eHeaderAllocationStatus headerStatus = lstItem.HeaderAllocationStatus;
                string headerStatusText = MIDText.GetTextOnly(Convert.ToInt32(lstItem.HeaderAllocationStatus));
                eHeaderIntransitStatus intransitStatus = lstItem.HeaderIntransitStatus;
                string intransitStatusText = MIDText.GetTextOnly(Convert.ToInt32(lstItem.HeaderIntransitStatus));
                eHeaderShipStatus shipStatus = lstItem.HeaderShipStatus;
                string shipStatusText = MIDText.GetTextOnly(Convert.ToInt32(lstItem.HeaderShipStatus));
                HierarchyNodeProfile hnp_style = GetNodeData(lstItem.StyleHnRID);
                string parentID = GetNodeData(Convert.ToInt32(hnp_style.Parents[0])).LevelText;
                string styleID = hnp_style.LevelText;

                int numberOfStores = lstItem.StoresWithAllocationCount;
                int packCount = lstItem.PackCount;
                int bulkColorCount = lstItem.BulkColorCount;
                int bulkColorSizeCount = lstItem.BulkColorSizeCount;
                string sizeGroupName = null;
                if (lstItem.SizeGroupRID > Include.UndefinedSizeGroupRID)
                {
                    // check for size group in hash; if not found read it
                    SizeGroupProfile sgp = (SizeGroupProfile)sizeGroupHash[lstItem.SizeGroupRID];
                    if (sgp == null)
                    {
                        sgp = new SizeGroupProfile(lstItem.SizeGroupRID);
                        sizeGroupHash.Add(lstItem.SizeGroupRID, sgp);
                    }
                    sizeGroupName = sgp.SizeGroupName;
                }

                string subordID = String.Empty;
                string masterSubord = String.Empty;

                if (lstItem.IsMasterHeader)
                {
                    masterSubord = lstItem.HeaderID;
                }
                else if (lstItem.IsSubordinateHeader)
                {
                    masterSubord = lstItem.MasterID;
                }
                int adjustVSW = Include.NoRID;
                if (lstItem.AdjustVSW_OnHand)
                {
                    adjustVSW = (int)eAdjustVSW.Adjust;
                }
                else
                {
                    if (headerType == eHeaderType.IMO)
                    {
                        adjustVSW = (int)eAdjustVSW.Replace;
                    }
                }
                int groupAllocRid = Include.NoRID;
                int asrtSortSeq = 0;
                if (headerType == eHeaderType.Assortment)
                {
                    if ((eAssortmentType)asrtType == eAssortmentType.GroupAllocation)
                    {
                        groupAllocRid = lstItem.Key;
                    }
                    else
                    {
                        asrtRID = lstItem.Key;
                        asrtSortSeq = 0;
                    }
                }

                int storetot = lstItem.TotalItemUnitsAllocated - lstItem.RsvAllocatedUnits;
                int vSWtot = lstItem.AllocatedUnits - storetot;

                int balance = 0;
                if ((lstItem.Packs != null && lstItem.Packs.Count > 0)
                    || (lstItem.BulkColors != null && lstItem.BulkColors.Count > 0))
                {
                    balance = lstItem.TotalUnitsToAllocate;
                    if (lstItem.Packs != null && lstItem.Packs.Count > 0)
                    {
                        foreach (PackHdr pack in lstItem.Packs.Values)
                        {
                            balance -= pack.PacksToAllocate * pack.PackMultiple;
                        }
                    }
                    if (lstItem.BulkColors != null && lstItem.BulkColors.Count > 0)
                    {
                        foreach (HdrColorBin color in lstItem.BulkColors.Values)
                        {
                            balance -= color.ColorUnitsToAllocate;
                        }
                    }
                }

                string otsForecast = string.Empty;
                if (lstItem.PlanHnRID != Include.DefaultPlanHnRID)
                {
                    otsForecast = headerData.GetNodeDisplay(lstItem.PlanHnRID);
                }

                string onHand = string.Empty;
                if (lstItem.OnHandHnRID != Include.DefaultOnHandHnRID)
                {
                    onHand = headerData.GetNodeDisplay(lstItem.OnHandHnRID);
                }

                string gradeInvBasis = string.Empty;
                if (lstItem.GradeInventoryBasisHnRID > 0)
                {
                    gradeInvBasis = headerData.GetNodeDisplay(lstItem.GradeInventoryBasisHnRID);
                }

                // need characteristics
                // create list in summary profile of a new class of header characteristics
                List<ROAllocationHeaderCharacteristic> headerCharacteristics = new List<ROAllocationHeaderCharacteristic>();

                foreach (HeaderCharProfile hcp in lstItem.Characteristics)
                {
                    ROAllocationHeaderCharacteristic headerCharacteristic = new ROAllocationHeaderCharacteristic();

                    HeaderCharGroupProfile hcgp = (HeaderCharGroupProfile)_headerCharGroupProfileList.FindKey(hcp.Key);
                    int headerCharGroupKey = hcp.Key;
                    headerCharacteristic.HeaderCharGroupKey = headerCharGroupKey;

                    string headerCharGroupName = string.Empty;
                    if (hcgp != null)
                    {
                        headerCharGroupName = hcgp.ID;
                    }

                    headerCharacteristic.HeaderCharGroupName = headerCharGroupName;

                    int headerCharKey = hcp.CharRID;
                    headerCharacteristic.HeaderCharKey = headerCharKey;

                    string headerCharValue = string.Empty;
                    if (hcp.Text != null)
                    {
                        if (hcp.HeaderCharType == eHeaderCharType.date)
                        {
                            headerCharValue = hcp.DateValue.ToString();
                        }
                        else
                        {
                            headerCharValue = hcp.Text;
                        }
                    }
                    headerCharacteristic.HeaderCharValue = headerCharValue;

                    headerCharacteristics.Add(headerCharacteristic);
                }
                //Ends Here

                int anchorHnRID = Include.NoRID, cdrRID = Include.NoRID;
                string anchorNode = string.Empty, dateRange = string.Empty;
                DataTable dtAssortProperties = headerData.GetAssortmentProperties(aKey);

                if (dtAssortProperties.Rows.Count > 0)
                {
                    anchorHnRID = Convert.ToInt32(dtAssortProperties.Rows[0]["ANCHOR_HN_RID"], CultureInfo.CurrentUICulture);
                    HierarchyNodeProfile anchorHnp = SAB.HierarchyServerSession.GetNodeData((int)anchorHnRID, false);
                    anchorNode = anchorHnp.LevelText;
                    cdrRID = Convert.ToInt32(dtAssortProperties.Rows[0]["CDR_RID"], CultureInfo.CurrentUICulture);
                    DateRangeProfile dr = SAB.ClientServerSession.Calendar.GetDateRange((int)cdrRID);
                    dateRange = dr.DisplayDate;
                }
                int digitalAssetKey = hnp_style.DigitalAssetKey;
                headerList.Add(new ROAllocationHeaderSummary(aKey, headerID, headerDescription, headerReceiptDay, originalReceiptDay, unitRetail, unitCost, styleHnRID,
                                                                planHnRID, onHandHnRID, vendor, purchaseOrder, beginDay, shipDay, lastNeedDay, releaseApprovedDate,
                                                                releaseDate, earliestShipDay, headerGroupRID,
                                                                sizeGroupRID, workflowRID, workflowTrigger, apiWorkflowRID, apiWorkflowTrigger, methodRID, allocationStarted, percentNeedLimit,
                                                                planPercentFactor,
                                                                reserveUnits, allocatedUnits, origAllocatedUnits, rsvAllocatedUnits, releaseCount, gradeWeekCount,
                                                                primarySecondaryRID, distributionCenter, allocationNotes, headerTotalQtyToAllocate,
                                                                headerTotalQtyAllocated,
                                                                headerTotalUnitMultiple, headerTotalUnitMultipleDefault, unitsShipped,
                                                                genericTotalQtyToAllocate, genericTotalQtyAllocated, genericTotalUnitMultiple, detailTotalQtyToAllocate,
                                                                detailTotalQtyAllocated, detailTotalUnitMultiple,
                                                                bulkTotalQtyToAllocate, bulkTotalQtyAllocated, bulkTotalUnitMultiple,
                                                                masterRID, masterID, subordinateRID, subordinateRIDs, subordinateID,
                                                                storeStyleAllocationManuallyChgdCnt,
                                                                storeSizeAllocationManuallyChgdCnt, storeStyleManualAllocationTotal,
                                                                storeSizeManualAllocationTotal, storesWithAllocationCnt, horizonOverride, asrtRID, placeHolderRID,
                                                                asrtType, nodeDisplayForOtsForecast, nodeDisplayForOnHand, nodeDisplayForGradeInvBasis,
                                                                workflowName, headerMethodName, apiWorkflowName, gradeSG_RID, gradeInventoryMinMax,
                                                                gradeInventoryHnRID, imoID, itemUnitsAllocated, itemOrigUnitsAllocated,
                                                                asrtPlaceholderSeq, asrtHeaderSeq, asrtUserRid, asrtID, asrtTypeForParentAsrt,
                                                                unitsPerCarton, DCFulfillmentProcessed, headerType, headerTypeText, canView, canUpdate,
                                                                headerStatus, headerStatusText, intransitStatus, 
                                                                intransitStatusText, shipStatus, shipStatusText, hnp_style, parentID, styleID, numberOfStores, packCount,
                                                                bulkColorCount, bulkColorSizeCount, sizeGroupName, subordID, masterSubord, digitalAssetKey,
                                                                adjustVSW, groupAllocRid, asrtSortSeq, storetot, vSWtot, balance, otsForecast, onHand, gradeInvBasis,
                                                                headerCharacteristics, anchorHnRID, anchorNode, cdrRID, dateRange));

            }

            return headerList;
        }

        public ROOut GetAssortmentFilterHeaderData(ROIntParms headerFilterRID)
        {
            try
            {
                this._headerFilterRID = headerFilterRID.ROInt;
                return LoadHeaderData();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        internal void LoadAssortmentFilterHeaders()
        {
            try
            {
                _assortmentWorkspaceSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAssortmentWorkspace);
                FilterHeaderOptions headerFilterOptions = new FilterHeaderOptions();
                headerFilterOptions.USE_WORKSPACE_FIELDS = true;
                headerFilterOptions.filterType = filterTypes.AssortmentFilter;
                _headerProfileArrayList = SAB.HeaderServerSession.GetHeadersForWorkspace(this._headerFilterRID, headerFilterOptions);

                ArrayList groupAllocHdrList = new ArrayList();
                foreach (AllocationHeaderProfile ahp in _headerProfileArrayList)
                {
                    if (ahp.AsrtType == (int)eAssortmentType.GroupAllocation)
                    {
                        groupAllocHdrList.Add(ahp);
                    }
                }
                foreach (AllocationHeaderProfile ahp in groupAllocHdrList)
                {
                    _headerProfileArrayList.Remove(ahp);
                }
                _masterKeyList = new ArrayList();
                _selectedHeaderKeyList = new ArrayList();
                _selectedAssortmentKeyList = new ArrayList();
                _selectedRowsSequence.Clear();

                BuildDataSets();
                LoadHeaders();
                LoadGridValueLists();

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void BuildDataSets()
        {
            _dtHeader = MIDEnvironment.CreateDataTable("Header");
            _colorsForStyle.Clear();
            _nodeDataHash.Clear();
            _nodeDataHashLastKey = 0;
            try
            {
                // Assortment
                _assortments = MIDEnvironment.CreateDataTable("Assortments");
                _assortments.Columns.Add("AsrtRID", System.Type.GetType("System.Int32"));
                _assortments.Columns.Add("AssortmentID");
                _assortments.PrimaryKey = new DataColumn[] { _assortments.Columns["AsrtRID"] };

                // Placeholders
                _placeHolders = MIDEnvironment.CreateDataTable("PlaceHolders");
                _placeHolders.Columns.Add("PlaceHolderRID", System.Type.GetType("System.Int32"));
                _placeHolders.Columns.Add("PlaceHolderID");

                // AnchorNodes
                _anchorNodes = MIDEnvironment.CreateDataTable("AnchorNodes");
                _anchorNodes.Columns.Add("PhStyleRID", System.Type.GetType("System.Int32"));
                _anchorNodes.Columns.Add("PhOldAnchorRID", System.Type.GetType("System.Int32"));
                _anchorNodes.Columns.Add("PhNewAnchorRID", System.Type.GetType("System.Int32"));
                _anchorNodes.PrimaryKey = new DataColumn[] { _anchorNodes.Columns["PhStyleRID"] };

                _dtHeader.Columns.Add("KeyH", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("KeyP", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("KeyC", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("HeaderID");

                _dtHeader.Columns.Add("HdrGroupRID", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("AsrtRID", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("PlaceHolderRID", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("FunctionSecurity");

                _dtHeader.Columns.Add("Type", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("Date", System.Type.GetType("System.DateTime"));
                _dtHeader.Columns.Add("Status", System.Type.GetType("System.Int32"));

                _dtHeader.Columns.Add("AnchorHnRID", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("AnchorNode");
                _dtHeader.Columns.Add("CdrRID", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("DateRange");
                _dtHeader.Columns.Add("ProductRID", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("Product");
                _dtHeader.Columns.Add("StyleHnRID", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("Style");

                _dtHeader.Columns.Add("StyleSecurity", System.Type.GetType("System.Int32"));

                _dtHeader.Columns.Add("Description");
                _dtHeader.Columns.Add("HdrQuantity", System.Type.GetType("System.Int32"));

                _dtHeader.Columns.Add("Balance", System.Type.GetType("System.Int32"));

                _dtHeader.Columns.Add("UnitRetail", System.Type.GetType("System.Double"));
                _dtHeader.Columns.Add("UnitCost", System.Type.GetType("System.Double"));
                _dtHeader.Columns.Add("SizeGroup", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("Multiple", System.Type.GetType("System.Int32"));

                _dtHeader.Columns.Add("Notes");
                _dtHeader.Columns.Add("Interfaced", System.Type.GetType("System.Boolean"));
                _dtHeader.Columns.Add("ChildTotal", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("MultiSortSeq", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("AllocatedUnits", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("OrigAllocatedUnits", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("RsvAllocatedUnits", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("AsrtType", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("AsrtSortSeq", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("CharUpdated", System.Type.GetType("System.Boolean"));

                _nonCharColCount = _dtHeader.Columns.Count;

                // header defaults and constraints
                _dtHeader.Columns["HeaderID"].AllowDBNull = false;
                _dtHeader.Columns["HeaderID"].Unique = true;
                _dtHeader.Columns["Product"].AllowDBNull = true;
                _dtHeader.Columns["Description"].AllowDBNull = true;
                _dtHeader.Columns["HdrGroupRID"].AllowDBNull = true;
                _dtHeader.Columns["AsrtRID"].AllowDBNull = true;
                _dtHeader.Columns["PlaceHolderRID"].AllowDBNull = true;
                _dtHeader.Columns["MultiSortSeq"].AllowDBNull = true;
                _dtHeader.Columns["AsrtSortSeq"].AllowDBNull = true;
                _dtHeader.Columns["KeyH"].DefaultValue = 0;
                _dtHeader.Columns["KeyP"].DefaultValue = 0;
                _dtHeader.Columns["KeyC"].DefaultValue = 0;
                _dtHeader.Columns["Type"].DefaultValue = (int)eHeaderType.Assortment;
                _dtHeader.Columns["Date"].DefaultValue = DateTime.Today;
                _dtHeader.Columns["UnitRetail"].DefaultValue = 0;
                _dtHeader.Columns["UnitCost"].DefaultValue = 0;
                _dtHeader.Columns["Multiple"].DefaultValue = Include.DefaultUnitMultiple;
                _dtHeader.Columns["SizeGroup"].DefaultValue = Include.UndefinedSizeGroupRID;
                _dtHeader.Columns["Interfaced"].DefaultValue = false;
                _dtHeader.Columns["CharUpdated"].DefaultValue = false;
                _dtHeader.PrimaryKey = new DataColumn[] { _dtHeader.Columns["KeyH"] };

                _dsMain = MIDEnvironment.CreateDataSet();
                _dsMain.Tables.Add(_dtHeader);

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void LoadAssortmentHeaders()
        {
            try
            {
                foreach (AllocationHeaderProfile ahp in _headerProfileArrayList)
                {
                    LoadHeader(ahp);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void LoadHeader(AllocationHeaderProfile ahp, bool assortmentHeadersOnly = true)
        {
            try
            {
                int headerType, headerStatus, intransitStatus, shipStatus;
                object groupRID, asrtRID, phRID, multiSortSeq, asrtSortSeq;
                HierarchyNodeSecurityProfile securityNode;
                eSecurityType styleSecurity;
                eSecurityLevel securityLevel = eSecurityLevel.Allow;
                string releaseDate = string.Empty;
                string headerDay = string.Empty;
                bool canView, canUpdate;
                _charByGroupAndID.Clear();
                _lastAsrtSortSeq = 0;

                headerType = Convert.ToInt32(ahp.HeaderType, CultureInfo.CurrentUICulture);
                headerStatus = Convert.ToInt32(ahp.HeaderAllocationStatus, CultureInfo.CurrentUICulture);
                intransitStatus = Convert.ToInt32(ahp.HeaderIntransitStatus, CultureInfo.CurrentUICulture);
                shipStatus = Convert.ToInt32(ahp.HeaderShipStatus, CultureInfo.CurrentUICulture);
                canView = false;
                canUpdate = false;

                if ((eHeaderType)headerType != eHeaderType.Assortment
                    && assortmentHeadersOnly)
                {
                    return;
                }

                switch ((eHeaderType)headerType)
                {
                    case eHeaderType.Assortment:
                    case eHeaderType.Placeholder:
                        canUpdate = _assortmentWorkspaceSecurity.AllowUpdate;
                        canView = _assortmentWorkspaceSecurity.AllowView;
                        break;

                    default:
                        securityNode = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(ahp.StyleHnRID, (int)eSecurityTypes.Allocation);
                        canUpdate = securityNode.AllowUpdate;
                        canView = securityNode.AllowView;
                        break;
                }
                if (canUpdate || canView)
                {
                    if (canUpdate)
                    {
                        styleSecurity = eSecurityType.Update;
                    }
                    else
                    {
                        styleSecurity = eSecurityType.View;
                    }


                    SizeGroupProfile sgp = (SizeGroupProfile)_sizeGroupHash[ahp.SizeGroupRID];
                    if (sgp == null)
                    {
                        sgp = new SizeGroupProfile(ahp.SizeGroupRID);
                        _sizeGroupHash.Add(ahp.SizeGroupRID, sgp);
                    }
                    string workflowMethodStr = string.Empty;

                    if (ahp.WorkflowRID > Include.UndefinedWorkflowRID)
                    {
                        workflowMethodStr = (string)_workflowNameHash[ahp.WorkflowRID];
                        if (workflowMethodStr == null)
                        {
                            workflowMethodStr = ahp.WorkflowName;
                            _workflowNameHash.Add(ahp.WorkflowRID, workflowMethodStr);
                        }
                    }
                    else if (ahp.MethodRID > Include.UndefinedMethodRID)
                    {
                        workflowMethodStr = ahp.HeaderMethodName;
                    }

                    string APIworkflowMethodStr = string.Empty;

                    if (ahp.API_WorkflowRID > Include.UndefinedWorkflowRID)
                    {
                        APIworkflowMethodStr = (string)_workflowNameHash[ahp.API_WorkflowRID];
                        if (APIworkflowMethodStr == null)
                        {
                            APIworkflowMethodStr = ahp.APIWorkflowName;
                            _workflowNameHash.Add(ahp.API_WorkflowRID, APIworkflowMethodStr);
                        }
                    }

                    int masterRID = Include.NoRID;
                    int subordRID = Include.NoRID;

                    string masterID = String.Empty;
                    string subordID = String.Empty;
                    string msgMasterSubord = String.Empty;

                    subordRID = ahp.SubordinateRID;
                    if (subordRID != Include.NoRID)
                    {
                        subordID = ahp.SubordinateID;
                        if (subordID != null && subordID != string.Empty)
                        {
                            msgMasterSubord = ahp.HeaderID + " / " + subordID;
                        }
                    }
                    else
                    {
                        masterRID = ahp.MasterRID;
                        if (masterRID != Include.NoRID)
                        {
                            masterID = ahp.MasterID;
                            if (masterID != null && masterID != string.Empty)
                            {
                                msgMasterSubord = masterID + " / " + ahp.HeaderID;
                            }
                        }
                    }

                    if (ahp.ReleaseDate == Include.UndefinedDate)
                    {
                        releaseDate = string.Empty;
                    }
                    else
                    {
                        releaseDate = ahp.ReleaseDate.ToShortDateString();
                    }

                    string parentID = string.Empty;
                    int parentRID = 0;

                    HierarchyNodeProfile hnp_style = this.GetNodeData(ahp.StyleHnRID);
                    parentRID = Convert.ToInt32(hnp_style.Parents[0], CultureInfo.CurrentUICulture);

                    HierarchyNodeProfile hnpProduct = this.GetNodeData(parentRID);
                    parentID = hnpProduct.LevelText;

                    // MultiHeader && Assortment
                    switch (headerType)
                    {
                        case (int)eHeaderType.MultiHeader:
                            break;

                        case (int)eHeaderType.Assortment:
                            _assortments.Rows.Add(new object[] { ahp.Key, ahp.HeaderID });
                            break;

                        case (int)eHeaderType.Placeholder:
                            _placeHolders.Rows.Add(new object[] { ahp.Key, ahp.HeaderID });
                            break;
                    }

                    groupRID = System.DBNull.Value;
                    multiSortSeq = System.DBNull.Value;

                    phRID = System.DBNull.Value;
                    asrtRID = System.DBNull.Value;
                    asrtSortSeq = System.DBNull.Value;
                    if (headerType == (int)eHeaderType.Assortment)
                    {
                        asrtRID = ahp.Key;
                        asrtSortSeq = 0;
                    }
                    else if (headerType == (int)eHeaderType.Placeholder)
                    {
                        phRID = ahp.Key;
                        asrtRID = ahp.AsrtRID;
                        asrtSortSeq = 0;
                    }
                    else if (ahp.AsrtRID != Include.NoRID)
                    {
                        asrtRID = ahp.AsrtRID;
                        _lastAsrtSortSeq++;
                        asrtSortSeq = _lastAsrtSortSeq;
                        if (ahp.PlaceHolderRID != Include.NoRID)
                        {
                            phRID = ahp.PlaceHolderRID;
                        }
                    }

                    object anchorHnRID = System.DBNull.Value;
                    string anchorNode = null;

                    object cdrRID = System.DBNull.Value;
                    Header headerData = new Header();
                    DataTable dtAssortProperties = headerData.GetAssortmentProperties(ahp.Key);
                    string dateRange = null;
                    if (dtAssortProperties.Rows.Count > 0)
                    {
                        anchorHnRID = Convert.ToInt32(dtAssortProperties.Rows[0]["ANCHOR_HN_RID"], CultureInfo.CurrentUICulture);
                        HierarchyNodeProfile anchorHnp = SAB.HierarchyServerSession.GetNodeData((int)anchorHnRID, false);
                        anchorNode = anchorHnp.LevelText;
                        cdrRID = Convert.ToInt32(dtAssortProperties.Rows[0]["CDR_RID"], CultureInfo.CurrentUICulture);
                        DateRangeProfile dr = SAB.ClientServerSession.Calendar.GetDateRange((int)cdrRID);
                        dateRange = dr.DisplayDate;
                    }

                    DataRow headerRow;
                    headerRow = _dtHeader.Rows.Add(new object[]
                                      { ahp.Key, Include.NoRID, Include.NoRID, ahp.HeaderID, groupRID,
                                        asrtRID, phRID, (int)securityLevel, headerType, ahp.HeaderDay,
                                        headerStatus, anchorHnRID, anchorNode, cdrRID, dateRange, parentRID, parentID, ahp.StyleHnRID,
                                        hnp_style.LevelText, (int)styleSecurity, ahp.HeaderDescription,
                                        ahp.TotalUnitsToAllocate, System.DBNull.Value, ahp.UnitRetail, ahp.UnitCost,
                                        ahp.SizeGroupRID, ahp.AllocationMultipleDsply,
                                        ahp.AllocationNotes,
                                        ahp.IsInterfaced, 0, multiSortSeq,
                                        ahp.AllocatedUnits, ahp.OrigAllocatedUnits,
                                        ahp.RsvAllocatedUnits, ahp.AsrtType, asrtSortSeq});

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private HierarchyNodeProfile GetNodeData(int aHnRID)
        {
            try
            {
                if (_nodeDataHashLastKey != aHnRID)
                {
                    _nodeDataHashLastKey = aHnRID;
                    if (_nodeDataHash == null)
                    {
                        _nodeDataHash = new Hashtable();
                    }
                    if (_nodeDataHash.Contains(aHnRID))
                    {
                        _nodeDataHashLastValue = (HierarchyNodeProfile)_nodeDataHash[aHnRID];
                    }
                    else
                    {
                        _nodeDataHashLastValue = SAB.HierarchyServerSession.GetNodeData(aHnRID, false);
                        _nodeDataHash.Add(aHnRID, _nodeDataHashLastValue);
                    }
                }
                return _nodeDataHashLastValue;
            }
            catch
            {
                throw;
            }
        }

        private void LoadHeaders()
        {
            try
            {
                foreach (AllocationHeaderProfile ahp in _headerProfileArrayList)
                {
                    LoadHeader(ahp);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void LoadGridValueLists()
        {
            try
            {
                // Assortments
                _assortmentValueList = new KeyValueList();
                _assortmentValueList.TKey = "AssortmentID";

                var rtn = (from item in _assortments.AsEnumerable()
                           select new ValueListItem
                           {
                               id = Convert.ToInt32(item["AsrtRID"], CultureInfo.CurrentUICulture),
                               value = item["AssortmentID"].ToString()
                           }).ToList();

                _assortmentValueList.TValues = rtn;

                // PlaceHolders
                _placeHolderValueList = new KeyValueList();
                _placeHolderValueList.TKey = "PlaceHolderID";

                var plch = (from item in _placeHolders.AsEnumerable()
                            select new ValueListItem
                            {
                                id = Convert.ToInt32(item["PlaceHolderRID"], CultureInfo.CurrentUICulture),
                                value = item["PlaceHolderID"].ToString()
                            }).ToList();

                _placeHolderValueList.TValues = plch;

                // Header Type
                _headerTypeValueList = new KeyValueList();
                _headerTypeValueList.TKey = "HeaderType";
                if (!SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
                {
                    bool phRemoved = false, asrtRemoved = false;
                    bool masterRemoved = false;

                    for (int i = _headerTypeValueList.TValues.Count - 1; i >= 0; i--)
                    {
                        ValueListItem vli = _headerTypeValueList.TValues[i];
                        int value = Convert.ToInt32(vli.value, CultureInfo.CurrentUICulture);
                        if (value == Convert.ToInt32(eHeaderType.Assortment, CultureInfo.CurrentUICulture))
                        {
                            _headerTypeValueList.TValues.Remove(vli);
                            asrtRemoved = true;
                        }
                        else if (value == Convert.ToInt32(eHeaderType.Placeholder, CultureInfo.CurrentUICulture))
                        {
                            _headerTypeValueList.TValues.Remove(vli);
                            phRemoved = true;
                        }

                        else if (value == Convert.ToInt32(eHeaderType.Master, CultureInfo.CurrentUICulture))
                        {
                            _headerTypeValueList.TValues.Remove(vli);
                            masterRemoved = true;
                        }

                        if (asrtRemoved && phRemoved && masterRemoved)
                        {
                            break;
                        }
                    }
                }

                _headerStatusValueList = LoadMIDTextValueList("Status", eMIDTextType.eHeaderAllocationStatus, eMIDTextOrderBy.TextCode);
                _headerIntransitValueList = LoadMIDTextValueList("Intransit", eMIDTextType.eHeaderIntransitStatus, eMIDTextOrderBy.TextCode);
                _headerShipStatusValueList = LoadMIDTextValueList("ShipStatus", eMIDTextType.eHeaderShipStatus, eMIDTextOrderBy.TextCode);

                if (SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
                {
                    _sizeGroupValueList = new KeyValueList();
                    _sizeGroupValueList.TKey = "SizeGroup";
                    SizeGroupList sgl = new SizeGroupList(eProfileType.SizeGroup);

                    sgl.LoadAll(IncludeUndefinedGroup: true, doReadSizeCodeListFromDatabase: false);

                    var szgrplst = (from SizeGroupProfile szgrp in sgl.ArrayList
                                    select new ValueListItem
                                    {
                                        id = szgrp.Key,
                                        value = szgrp.SizeGroupName
                                    }).ToList();

                    _sizeGroupValueList.TValues = szgrplst;
                }

                // Pack Type
                _packTypeValueList = LoadMIDTextValueList("PackType", eMIDTextType.ePackType, eMIDTextOrderBy.TextCode);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private KeyValueList LoadMIDTextValueList(string aKey, eMIDTextType aMIDTextType, eMIDTextOrderBy aMIDTextOrderBy)
        {
            KeyValueList valueList = new KeyValueList();
            valueList.TKey = aKey;
            try
            {
                DataTable dataTable = MIDText.GetTextType(aMIDTextType, aMIDTextOrderBy);

                var valueLstItem = (from vallst in dataTable.AsEnumerable()
                                    select new ValueListItem
                                    {
                                        id = Convert.ToInt32(vallst["TEXT_CODE"], CultureInfo.CurrentUICulture),
                                        value = vallst["TEXT_VALUE"].ToString()
                                    }).ToList();
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return valueList;
        }

        public enum eSecurityType
        {
            View,
            Update
        }

        #endregion

        #region Populate Explorer View Name List

        internal ROOut GetAssortmentWorklistViews()
        {
            try
            {
                List<ROWorklistViewOut> assortmentViewDetails = BindAssortmentView();
                return new ROIListOut(eROReturnCode.Successful, null, ROInstanceID, assortmentViewDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal List<ROWorklistViewOut> BindAssortmentView()
        {
            _userGridView = new UserGridView();
            _gridViewData = new GridViewData();

            _assortmentWorkspaceSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAssortmentWorkspace);
            _assortmentWorkspaceViewsGlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAssortmentWorkspaceViewsGlobal);
            _assortmentWorkspaceViewsUserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAssortmentWorkspaceViewsUser);

            FilterData _filterData = new FilterData();
            ArrayList userRIDList = new ArrayList();

            if (_assortmentWorkspaceViewsGlobalSecurity.AllowUpdate || _assortmentWorkspaceViewsGlobalSecurity.AllowView)
            {
                userRIDList.Add(Include.GlobalUserRID);
            }
            if (_assortmentWorkspaceViewsUserSecurity.AllowUpdate || _assortmentWorkspaceViewsUserSecurity.AllowView)
            {
                userRIDList.Add(SAB.ClientServerSession.UserRID);
            }

            int viewRID = _userGridView.UserGridView_Read(SAB.ClientServerSession.UserRID, eLayoutID.assortmentWorkspaceGrid);

            bool useViewWorkspaceFilter = false;
            bool useFilterSorting = false;
            if (viewRID != Include.NoRID && !_fromFilterWindow)
            {

                int workspaceFilterRID = _gridViewData.GridViewReadWorkspaceFilterRID(viewRID, ref useFilterSorting);
                if (workspaceFilterRID != Include.NoRID)
                {
                    useViewWorkspaceFilter = true;
                    this._headerFilterRID = workspaceFilterRID;
                }
            }
            if (useViewWorkspaceFilter == false)
            {
                this._headerFilterRID = _filterData.WorkspaceCurrentFilter_Read(SAB.ClientServerSession.UserRID, eWorkspaceType.AssortmentWorkspace);
            }
            DataTable dtAssortmentFilters = _filterData.FilterRead(filterTypes.AssortmentFilter, eProfileType.FilterAssortment, userRIDList);

            _bindingView = true;
            dtView = _gridViewData.GridView_Read((int)eLayoutID.assortmentWorkspaceGrid, userRIDList, true);
            dtView.Rows.Add(new object[] { Include.NoRID, SAB.ClientServerSession.UserRID, (int)eLayoutID.assortmentWorkspaceGrid, "     " });
            //_dtView;                

            _bindingView = false;

            List<ROWorklistViewOut> views = new List<ROWorklistViewOut>();

            DataView dv = new DataView(dtView);

            foreach (DataRowView rowView in dv)
            {
                int viewKey = Convert.ToInt32(rowView.Row["VIEW_RID"]);
                string viewName = Convert.ToString(rowView.Row["VIEW_ID"]);
                int filterRID = (rowView["WORKSPACE_FILTER_RID"] != DBNull.Value) ? Convert.ToInt32(rowView["WORKSPACE_FILTER_RID"]) : Include.NoRID;
                views.Add(new ROWorklistViewOut(viewKey, viewName, filterRID));
            }

            return views;
        }


        #endregion

        #region "Update Assortment Review List"
        private ROOut UpdateAssortmentReviewSelection(ROCubeOpenParms roCubeParams)
        {
            try
            {
                SetActivateAssortmentOnHeaders(true);
                SaveDetailCubeGroup();
                SetActivateAssortmentOnHeaders(false);

                Hashtable blockedHash = (Hashtable)_asrtCubeGroup.BlockedList.Clone();

                CloseAndReOpenCubeGroup();

                _asrtCubeGroup.BlockedList = blockedHash;

                if (IsAssortment)
                {

                    UpdateData(reload:true, rebuildComponents:true, reloadBlockedCells:false, reloadHeaders:false);
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new RONoDataOut(eROReturnCode.Successful, null, ROInstanceID);
        }

        #region "reference methods"
        internal void UpdateData(bool reload, bool rebuildComponents, bool reloadBlockedCells, bool reloadHeaders = true, bool reformatRows = false)
        {
            try
            {
                if (rebuildComponents)
                {
                    _asrtCubeGroup.NullHeaderObjects();
                }

                if (rebuildComponents)
                {
                    _dtHeaders = _asrtCubeGroup.GetAssortmentComponents();
                    ComponentsChanged(reloadHeaders);
                }
                _asrtCubeGroup.ReadData(reload, reloadBlockedCells);
                if (reformatRows)
                {
                    ReformatRowsChanged(true);
                }
                ProfileList apl = _applicationSessionTransaction.GetMasterProfileList(eProfileType.Allocation);
                foreach (AllocationProfile ap in apl.ArrayList)
                {
                    UpdateStatusColumn(ap.Key, ap.GetHeaderAllocationStatus(true));
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {

            }
        }
        internal void UpdateStatusColumn(int aHeaderRID, eHeaderAllocationStatus aStatus)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal void ComponentsChanged(bool reloadHeaders = true)
        {
            AssortmentComponentVariableProfile varProf;
            try
            {
                _asrtCubeGroup.ComponentsChanged(_sortedComponentColumnHeaders, reloadHeaders);
                if (IsAssortment || IsGroupAllocation)
                {
                    _placeholderSelected = false;

                    foreach (RowColProfileHeader rowColHdr in _selectableComponentColumnHeaders)
                    {
                        varProf = (AssortmentComponentVariableProfile)rowColHdr.Profile;

                        if (rowColHdr.IsDisplayed && varProf.Key == ((AssortmentViewComponentVariables)_componentVariables).Placeholder.Key)
                        {
                            _placeholderSelected = true;
                        }
                    }
                }
                else
                {
                    _placeholderSelected = false;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        internal void SetActivateAssortmentOnHeaders(bool isActive)
        {
            AllocationProfileList apl = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember);
            foreach (AllocationProfile ap in apl.ArrayList)
            {
                if (ap != null)
                {
                    if (ap.HeaderType != eHeaderType.Placeholder && ap.HeaderType != eHeaderType.Assortment)
                    {
                        ap.ActivateAssortment = isActive;
                    }
                }
            }
        }

        internal bool SaveDetailCubeGroup()
        {
            try
            {
                _asrtCubeGroup.SaveCubeGroup();
                return true;
            }
            catch (MIDException MIDexc)
            {

                return false;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        internal void CloseAndReOpenCubeGroup()
        {
            try
            {
                _asrtCubeGroup.CloseCubeGroup();

                //Create an AssortmentCubeGroup
                _asrtCubeGroup = new AssortmentCubeGroup(SAB, _applicationSessionTransaction, _windowType);
                _openParms = LoadParmsFromTransaction(_applicationSessionTransaction.AssortmentViewLoadedFrom);   // TT#857 - MD - stodd - assortment not honoring view
                _asrtCubeGroup.OpenCubeGroup(_openParms);

                if (IsAssortment || IsGroupAllocation)
                {
                    if (_lastattributeValue != Include.Undefined)
                    {
                        _asrtCubeGroup.SetStoreGroup(StoreMgmt.StoreGroup_Get(_lastattributeValue));
                    }
                    else if (_applicationSessionTransaction.AssortmentViewLoadedFrom == eAssortmentBasisLoadedFrom.AssortmentProperties)
                    {
                        _asrtCubeGroup.SetStoreGroup(StoreMgmt.StoreGroup_Get(_asrtCubeGroup.AssortmentStoreGroupRID)); 
                    }
                    else if (_applicationSessionTransaction.AssortmentViewLoadedFrom == eAssortmentBasisLoadedFrom.UserSelectionCriteria)
                    {
                        _asrtCubeGroup.SetStoreGroup(StoreMgmt.StoreGroup_Get(_applicationSessionTransaction.AssortmentStoreAttributeRid)); 
                    }
                 }
                else
                {
                    _asrtCubeGroup.SetStoreGroup(StoreMgmt.StoreGroup_Get(_lastattributeValue)); 
                }
                _asrtCubeGroup.SetStoreFilter(Include.NoRID, null);
                ComponentsChanged(false);
                if (_asrtCubeGroup.AssortmentType == eAssortmentType.Undefined)
                {
                    GetAssortmentType();
                }
            }
            catch
            {
                throw;
            }
        }

        internal AssortmentOpenParms LoadParmsFromTransaction(eAssortmentBasisLoadedFrom loadedFrom)
        {
            AssortmentOpenParms openParms;
            try
            {
                openParms = new AssortmentOpenParms();
                openParms.StoreGroupRID = _applicationSessionTransaction.AssortmentStoreAttributeRid;
                openParms.FilterRID = Include.NoRID;
                openParms.GroupBy = (eAllocationAssortmentViewGroupBy)_applicationSessionTransaction.AssortmentGroupBy;
                openParms.ViewRID = _applicationSessionTransaction.AssortmentViewRid;
                if (loadedFrom == eAssortmentBasisLoadedFrom.AssortmentProperties
                    || loadedFrom == eAssortmentBasisLoadedFrom.GroupAllocation)
                {
                    Header hd = new Header();
                    // Get first assortment Rid
                    int asrtRid = -1;
                    foreach (AllocationHeaderProfile ahp in _headerList)
                    {
                        if (ahp.HeaderType == eHeaderType.Assortment)
                        {
                            asrtRid = ahp.Key;
                            break;
                        }
                    }
                    int viewRid = hd.GetAssortmentUserView(asrtRid, SAB.ClientServerSession.UserRID);
                    if (openParms.ViewRID == Include.NoRID
                        && viewRid != Include.NoRID)
                    {
                        openParms.ViewRID = viewRid;
                    }
                }

                if (openParms.ViewRID == Include.NoRID)
                {
                    openParms.ViewRID = Include.DefaultAssortmentViewRID;
                }
                if (loadedFrom == eAssortmentBasisLoadedFrom.AssortmentProperties
                    || loadedFrom == eAssortmentBasisLoadedFrom.GroupAllocation)
                {
                    openParms.VariableType = (eAssortmentVariableType)_assortmentProfile.AssortmentVariableType;
                    openParms.VariableNumber = _assortmentProfile.AssortmentVariableNumber;
                    openParms.InclOnhand = _assortmentProfile.AssortmentIncludeOnhand;
                    openParms.InclIntransit = _assortmentProfile.AssortmentIncludeIntransit;
                    openParms.InclSimStores = _assortmentProfile.AssortmentIncludeSimilarStores;
                    openParms.InclCommitted = _assortmentProfile.AssortmentIncludeCommitted;
                    openParms.AverageBy = (eStoreAverageBy)_assortmentProfile.AssortmentAverageBy;
                    openParms.GradeBoundary = (eGradeBoundary)_assortmentProfile.AssortmentGradeBoundary;
                }
                else
                {
                    openParms.VariableType = (eAssortmentVariableType)_applicationSessionTransaction.AssortmentVariableType;
                    openParms.VariableNumber = _applicationSessionTransaction.AssortmentVariableNumber;
                    openParms.InclOnhand = _applicationSessionTransaction.AssortmentIncludeOnhand;
                    openParms.InclIntransit = _applicationSessionTransaction.AssortmentIncludeIntransit;
                    openParms.InclSimStores = _applicationSessionTransaction.AssortmentIncludeSimStore;
                    openParms.InclCommitted = _applicationSessionTransaction.AssortmentIncludeCommitted;
                    openParms.AverageBy = (eStoreAverageBy)_applicationSessionTransaction.AssortmentAverageBy;
                    openParms.GradeBoundary = (eGradeBoundary)_applicationSessionTransaction.AssortmentGradeBoundary;
                }
                return openParms;
            }
            catch
            {
                throw;
            }
        }
        internal bool IsAssortment
        {
            get
            {
                bool isAsrt = false;
                if (_windowType == eAssortmentWindowType.Assortment)
                {
                    isAsrt = true;
                }
                return isAsrt;
            }
        }
        internal bool IsGroupAllocation
        {
            get
            {
                bool isGa = false;
                if (_windowType == eAssortmentWindowType.GroupAllocation)
                {
                    isGa = true;
                }
                return isGa;
            }
        }

        #endregion
        #endregion

        #region "Method to get the Assortment User Last Data"
        private ROOut GetAssortmentUserLastValues()
        {
            ROAllocationWorklistValues userLastValues = GetAssortmentUserValues();
            ROAllocationWorklistValuesOut userLastValuesOut = new ROAllocationWorklistValuesOut(eROReturnCode.Successful, null, ROInstanceID, userLastValues);

            return userLastValuesOut;
        }
        #endregion

        #region "Method to save the Assortment User Last Data"
        private ROOut SaveAssortmentUserLastValues(ROAllocationWorklistLastDataParms rOAllocationWorklistLastDataParms)
        {
            try
            {
                SaveAssortmentUserValues(rOAllocationWorklistLastDataParms);

                RONoDataOut rONoDataOut = new RONoDataOut(eROReturnCode.Successful, null, ROInstanceID);
                return rONoDataOut;
            }
            catch
            {
                RONoDataOut rONoDataOut = new RONoDataOut(eROReturnCode.Failure, null, ROInstanceID);
                return rONoDataOut;
            }

        }
        #endregion

        #region "Get User Last Values"
        internal ROAllocationWorklistValues GetAssortmentUserValues()
        {
            ROAllocationWorklistValues userLastValuesOut = new ROAllocationWorklistValues();
            _userGridView = new UserGridView();
            _gridViewData = new GridViewData();
            _filterData = new FilterData();

            int viewRID = _userGridView.UserGridView_Read(SAB.ClientServerSession.UserRID, eLayoutID.assortmentWorkspaceGrid);
            userLastValuesOut.UserRID = SAB.ClientServerSession.UserRID;
            userLastValuesOut.ViewRID = viewRID;
            bool useViewWorkspaceFilter = false;
            bool useFilterSorting = false;
            if (viewRID != Include.NoRID && !_fromFilterWindow)
            {
                int workspaceFilterRID = _gridViewData.GridViewReadWorkspaceFilterRID(viewRID, ref useFilterSorting);
                if (workspaceFilterRID != Include.NoRID)
                {
                    useViewWorkspaceFilter = true;
                    userLastValuesOut.FilterRID = workspaceFilterRID;
                }
            }
            if (useViewWorkspaceFilter == false) // use the current user workspace filter
            {
                userLastValuesOut.FilterRID = _filterData.WorkspaceCurrentFilter_Read(SAB.ClientServerSession.UserRID, eWorkspaceType.AssortmentWorkspace);
            }

            // Get the names associated with the RID's
            FilterData filterData = new FilterData();
            userLastValuesOut.FilterName = filterData.FilterGetName(userLastValuesOut.FilterRID);

            DataRow row = _gridViewData.GridView_Read(userLastValuesOut.ViewRID);
            if (row != null)
            {
                userLastValuesOut.ViewName = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture);
            }

            return userLastValuesOut;
        }
        #endregion

        #region "Save User Last Values"
        internal void SaveAssortmentUserValues(ROAllocationWorklistLastDataParms rOAllocationWorklistLastDataParms)
        {
            _userGridView = new UserGridView();
            _filterData = new FilterData();
            _userGridView.UserGridView_Update(SAB.ClientServerSession.UserRID, eLayoutID.assortmentWorkspaceGrid, rOAllocationWorklistLastDataParms.iViewRID);
            _filterData.WorkspaceCurrentFilter_Update(SAB.ClientServerSession.UserRID, eWorkspaceType.AssortmentWorkspace, rOAllocationWorklistLastDataParms.iFilterRID);
        }
        #endregion

        #region "Rename Worklist items"
        public ROOut RenameWorklistItems(ROBaseUpdateParms renameParms)
        {
            string message;
            try
            {
                if (!RenameWorklistItem(renameParms, out message))
                {
                    MIDEnvironment.Message = message;
                    MIDEnvironment.requestFailed = true;
                }
                return LoadHeaderData();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private bool RenameWorklistItem(ROBaseUpdateParms renameParms, out string message)
        {
            message = null;
            int key;
            FolderDataLayer dlFolder;
            Header dlAssortment;
            GenericEnqueue objEnqueue;
            List<GenericEnqueue> enqueueList = new List<GenericEnqueue>();

            try
            {
                switch (renameParms.ProfileType)
                {
                    case eProfileType.AssortmentHeader:
                        dlAssortment = new Header();
                        objEnqueue = EnqueueObject("Rename", renameParms.Name, eLockType.Header, renameParms.Key, ref message);
                        if (objEnqueue == null)
                        {
                            return false;
                        }

                        enqueueList.Add(objEnqueue);

                        try
                        {
                            key = dlAssortment.HeaderAssortment_GetKey(renameParms.NewName);

                            if (key != -1)
                            {
                                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AssortmentNameExists);
                                return false;
                            }

                            // Enqueue placeholders
                            DataTable dtPlaceholders = dlAssortment.GetPlaceholdersForAssortment(renameParms.Key);
                            foreach (DataRow row in dtPlaceholders.Rows)
                            {
                                int phKey = int.Parse(row["HDR_RID"].ToString());
                                AllocationHeaderProfile apPlaceholder = (AllocationHeaderProfile)SAB.HeaderServerSession.GetHeaderData(phKey, false, false, true);
                                AssortmentHeaderProfile asrtPlaceholder = new AssortmentHeaderProfile(apPlaceholder.HeaderID, apPlaceholder.Key);
                                GenericEnqueue objEnqueuePh = EnqueueObject("Rename", apPlaceholder.HeaderID, eLockType.Assortment, apPlaceholder.Key, ref message);
                                if (objEnqueue == null)
                                {
                                    return false;
                                }
                                enqueueList.Add(objEnqueuePh);
                            }

                            dlAssortment.OpenUpdateConnection();

                            try
                            {
                                
                                dlAssortment.AssortmentHeader_Update(renameParms.Key, renameParms.NewName);

                                foreach (DataRow row in dtPlaceholders.Rows)
                                {
                                    int phKey = int.Parse(row["HDR_RID"].ToString());
                                    string oldId = row["HDR_ID"].ToString();
                                    int index = oldId.IndexOf("PhStyle");
                                    string phNewName = renameParms.NewName + " " + oldId.Substring(index);
                                    dlAssortment.AssortmentHeader_Update(phKey, phNewName);
                                }

                                dlAssortment.CommitData();
                            }
                            catch (Exception exc)
                            {
                                message = exc.ToString();
                                throw;
                            }
                            finally
                            {
                                dlAssortment.CloseUpdateConnection();
                            }
                        }
                        catch (Exception error)
                        {
                            message = error.ToString();
                            throw;
                        }
                        finally
                        {
                            foreach (GenericEnqueue ge in enqueueList)
                            {
                                ge.DequeueGeneric();
                            }
                        }

                        break;

                    case eProfileType.AssortmentMainFavoritesFolder:
                    case eProfileType.AssortmentMainFolder:
                    case eProfileType.AssortmentSubFolder:
                        dlFolder = new FolderDataLayer();

                        key = dlFolder.Folder_GetKey(renameParms.UserKey, renameParms.NewName, renameParms.ParentKey, renameParms.ProfileType);

                        if (key != -1)
                        {
                            message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FolderNameExists);
                            return false;
                        }

                        dlFolder.OpenUpdateConnection();

                        try
                        {
                            dlFolder.Folder_Rename(renameParms.Key, renameParms.NewName);
                            dlFolder.CommitData();
                        }
                        catch (Exception exc)
                        {
                            message = exc.ToString();
                            throw;
                        }
                        finally
                        {
                            dlFolder.CloseUpdateConnection();
                        }

                        break;
                }

                return true;
            }
            catch (Exception exc)
            {
                message = exc.ToString();
                throw;
            }
        }
        #endregion "Rename Worklist items"

        #region "Delete Assortment"
        public ROOut DeleteWorklistItems(ROBaseUpdateParms deleteParms)
        {
            string message;
            try
            {
                if (!DeleteWorklistItem(deleteParms, out message))
                {
                    MIDEnvironment.Message = message;
                    MIDEnvironment.requestFailed = true;
                }
                return LoadHeaderData();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private bool DeleteWorklistItem(ROBaseUpdateParms deleteParms, out string message)
        {
            message = null;
            FolderDataLayer dlFolder;
            Header dlAssortment;
            GenericEnqueue objEnqueue;
            object[] deleteArray;
            ApplicationSessionTransaction enqueueTransaction = null;

            try
            {
                dlAssortment = new Header();
                dlFolder = new FolderDataLayer();

                if (deleteParms.ProfileType == eProfileType.AssortmentHeader)
                {
                    objEnqueue = EnqueueObject("Delete", deleteParms.Name, eLockType.Header, deleteParms.Key, ref message);

                    if (objEnqueue == null)
                    {
                        return false;
                    }
                    if (!HeadersEnqueued(deleteParms.Key, out enqueueTransaction, ref message))
                    {
                        objEnqueue.DequeueGeneric();
                        return false;
                    }

                    try
                    {
                        ArrayList styleAL = new ArrayList();
                        DataTable dtAssortment = dlAssortment.GetHeader(deleteParms.Key);
                        foreach (DataRow row in dtAssortment.Rows)
                        {
                            styleAL.Add(Convert.ToInt32(row["STYLE_HNRID"], CultureInfo.CurrentUICulture));
                        }
                        DataTable dtPlaceholders = dlAssortment.GetPlaceholdersForAssortment(deleteParms.Key);
                        foreach (DataRow row in dtPlaceholders.Rows)
                        {
                            styleAL.Add(Convert.ToInt32(row["STYLE_HNRID"], CultureInfo.CurrentUICulture));
                        }
                        dlAssortment.OpenUpdateConnection();
                        dlFolder.OpenUpdateConnection();

                        try
                        {
                            dlFolder.Folder_Item_Delete(deleteParms.Key, eProfileType.AssortmentHeader);
                            dlFolder.Folder_Shortcut_DeleteAll(deleteParms.Key, eProfileType.AssortmentHeader);
                            
                            dlAssortment.DeleteAssortment(deleteParms.Key);

                            dlAssortment.CommitData();
                            dlFolder.CommitData();

                            if (styleAL.Count > 0)
                            {
                                foreach (int styleRID in styleAL)
                                {
                                    if (!DeletePlaceholderStyle(styleRID, ref message))
                                    {
                                        break;
                                    }
                                }
                            }

                        }
                        catch (DatabaseForeignKeyViolation)
                        {
                            message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse);
                            return false;
                        }
                        catch (Exception exc)
                        {
                            message = exc.ToString();
                            throw;
                        }
                        finally
                        {
                            dlAssortment.CloseUpdateConnection();
                            dlFolder.CloseUpdateConnection();
                        }
                    }
                    catch (Exception error)
                    {
                        message = error.ToString();
                        throw;
                    }
                    finally
                    {
                        objEnqueue.DequeueGeneric();
                        DequeueHeaders(enqueueTransaction);
                    }
                }
                //else if (aNode.isSubFolder)
                //{
                //    deleteArray = new object[aNode.Nodes.Count];
                //    aNode.Nodes.CopyTo(deleteArray, 0);

                //    foreach (MIDAssortmentNode node in deleteArray)
                //    {
                //        DeleteAssortmentNode(node);
                //    }

                //    if (aNode.Nodes.Count == 0)
                //    {
                //        dlFolder.OpenUpdateConnection();

                //        try
                //        {
                //            dlFolder.Folder_Item_Delete(deleteParms.Key, eProfileType.AssortmentSubFolder);
                //            dlFolder.Folder_Delete(deleteParms.Key, eProfileType.AssortmentSubFolder);
                //            dlFolder.CommitData();
                //        }
                //        catch (Exception exc)
                //        {
                //            message = exc.ToString();
                //            throw;
                //        }
                //        finally
                //        {
                //            dlFolder.CloseUpdateConnection();
                //        }
                //    }
                //}
                //else if (aNode.isObjectShortcut)
                //{
                //    dlFolder.OpenUpdateConnection();

                //    try
                //    {
                //        dlFolder.Folder_Shortcut_Delete(deleteParms.ParentKey, deleteParms.Key, eProfileType.AssortmentHeader);
                //        dlFolder.CommitData();
                //    }
                //    catch (DatabaseForeignKeyViolation)
                //    {
                //        message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse);
                //        return false;
                //    }
                //    catch (Exception exc)
                //    {
                //        message = exc.ToString();
                //        throw;
                //    }
                //    finally
                //    {
                //        dlFolder.CloseUpdateConnection();
                //    }

                //}
                //else if (aNode.isFolderShortcut)
                //{
                //    dlFolder.OpenUpdateConnection();

                //    try
                //    {
                //        dlFolder.Folder_Shortcut_Delete(deleteParms.ParentKey, deleteParms.Key, eProfileType.AssortmentSubFolder);
                //        dlFolder.CommitData();
                //    }
                //    catch (DatabaseForeignKeyViolation)
                //    {
                //        message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse);
                //        return false;
                //    }
                //    catch (Exception exc)
                //    {
                //        message = exc.ToString();
                //        throw;
                //    }
                //    finally
                //    {
                //        dlFolder.CloseUpdateConnection();
                //    }

                //}

                return true;
            }
            catch (Exception exc)
            {
                message = exc.ToString();
                throw;
            }
        }

        private bool DeletePlaceholderStyle(int aStyleRID, ref string message)
        {
            try
            {
                EditMsgs em = new EditMsgs();
                HierarchyNodeProfile styleHnp = SAB.HierarchyServerSession.GetNodeData(aStyleRID);
                if (styleHnp.IsVirtual && (styleHnp.Purpose == ePurpose.Placeholder || styleHnp.Purpose == ePurpose.Assortment))
                {
                    HierMaint.DeletePlaceholderStyleAnchorNode(aStyleRID, ref em);
                }
                if (em.ErrorFound)
                {
                    message = FormatMessage(em);
                    SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, message, this.GetType().Name);
                    return false;
                }

                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion "Delete Assortment"

        #region "Copy Assortment"
        public ROOut CopyWorklistItems(ROBaseUpdateParms deleteParms)
        {
            string message;
            try
            {
                if (!CopyWorklistItem(deleteParms, out message))
                {
                    MIDEnvironment.Message = message;
                    MIDEnvironment.requestFailed = true;
                }
                return LoadHeaderData();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private bool CopyWorklistItem(ROBaseUpdateParms copyParms, out string message)
        {
            message = null;

            try
            {
                try
                {
                    return CopyAssortmentNode(copyParms, ref message);
                }
                catch (Exception exc)
                {
                    message = exc.ToString();
                    throw;
                }
            }
            catch (Exception exc)
            {
                message = exc.ToString();
                throw;
            }
        }

        private bool CopyAssortmentNode(ROBaseUpdateParms copyParms, ref string message)
        {
            FolderProfile folderProf;
            AssortmentProfile assortmentProf;
            FolderDataLayer dlFolder;
            Header dlAssortment;
            AssortmentHeaderProfile ahp;


            try
            {
                dlAssortment = new Header();
                dlFolder = new FolderDataLayer();
                //if (aFromNode.isSubFolder)
                //{
                //    folderProf = (FolderProfile)((FolderProfile)aFromNode.Profile).Clone();
                //    folderProf.UserRID = copyParms.NewUserKey;
                //    folderProf.OwnerUserRID = copyParms.NewUserKey;

                //    folderProf.Name = WorklistUtilities.FindNewFolderName(copyParms.Name, copyParms.NewUserKey, copyParms.NewParentKey, eProfileType.AssortmentSubFolder);

                //    dlFolder.OpenUpdateConnection();

                //    try
                //    {
                //        folderProf.Key = dlFolder.Folder_Create(folderProf.UserRID, folderProf.Name, eProfileType.AssortmentSubFolder);
                //        dlFolder.Folder_Item_Insert(copyParms.NewParentKey, folderProf.Key, eProfileType.AssortmentSubFolder);

                //        dlFolder.CommitData();
                //    }
                //    catch (Exception exc)
                //    {
                //        message = exc.ToString();
                //        throw;
                //    }
                //    finally
                //    {
                //        dlFolder.CloseUpdateConnection();
                //    }

                //    newNode = BuildSubFolderNode(folderProf, aToNode);

                //    foreach (MIDAssortmentNode node in aFromNode.Nodes)
                //    {
                //        CopyAssortmentNode(node, newNode, aFindUniqueName);
                //    }

                //    return true;
                //}
                //else if (aFromNode.isFolderShortcut)
                //{
                //    dlFolder.OpenUpdateConnection();

                //    try
                //    {
                //        dlFolder.Folder_Shortcut_Insert(copyParms.NewParentKey, copyParms.ParentKey, copyParms.ParentProfileType);
                //        dlFolder.CommitData();
                //    }
                //    catch (Exception exc)
                //    {
                //        message = exc.ToString();
                //        throw;
                //    }
                //    finally
                //    {
                //        dlFolder.CloseUpdateConnection();
                //    }

                //    return true;
                //}
                //else if (aFromNode.isObjectShortcut)
                //{
                //    dlFolder.OpenUpdateConnection();

                //    try
                //    {
                //        dlFolder.Folder_Shortcut_Insert(copyParms.NewParentKey, copyParms.ParentKey, copyParms.ParentProfileType);
                //        dlFolder.CommitData();
                //    }
                //    catch (Exception exc)
                //    {
                //        message = exc.ToString();
                //        throw;
                //    }
                //    finally
                //    {
                //        dlFolder.CloseUpdateConnection();
                //    }

                //    return true;
                //}
                //else if (copyParms.ProfileType == eProfileType.AssortmentHeader)
                if (copyParms.ProfileType == eProfileType.AssortmentHeader)
                {
                    try
                    {
                        //============================
                        // Copy Assortment header
                        //============================
                        ApplicationSessionTransaction aTrans = SAB.ApplicationServerSession.CreateTransaction();
                        AssortmentProfile origAsrtProfile = new AssortmentProfile(aTrans, copyParms.Name, copyParms.Key, SAB.ApplicationServerSession);
                        assortmentProf = (AssortmentProfile)(origAsrtProfile).Clone();

                        assortmentProf.Key = Include.NoRID;
                        assortmentProf.AssortmentUserRid = Include.GlobalUserRID;
                        DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRangeClone(assortmentProf.AssortmentCalendarDateRangeRid);
                        assortmentProf.AssortmentCalendarDateRangeRid = drp.Key;
                        assortmentProf.AsrtRID = Include.NoRID;   
                        if (assortmentProf.AssortmentBeginDayCalendarDateRangeRid != Include.UndefinedCalendarDateRange)
                        {
                            DateRangeProfile drpBeginDay = SAB.ClientServerSession.Calendar.GetDateRangeClone(assortmentProf.AssortmentBeginDayCalendarDateRangeRid);
                            assortmentProf.AssortmentBeginDayCalendarDateRangeRid = drpBeginDay.Key;
                        }

                        assortmentProf.HeaderID = FindNewAssortmentName(assortmentProf.HeaderID, copyParms.NewUserKey);

                        dlAssortment.OpenUpdateConnection();
                        dlFolder.OpenUpdateConnection();
                        assortmentProf.AppSessionTransaction.NewAllocationMasterProfileList();
                        assortmentProf.WriteHeader();
                        assortmentProf.BuildAssortmentSummary();
                        assortmentProf.AssortmentSummaryProfile.Key = assortmentProf.Key;


                        //=====================================
                        // Copy Attached Placeholder headers
                        //=====================================
                        Hashtable placeholderHash = new Hashtable();
                        DataTable dtPlaceholders = dlAssortment.GetPlaceholdersForAssortment(copyParms.Key);
                        HierarchyNodeList hierarchyNodeList = SAB.HierarchyServerSession.GetPlaceholderStyles(assortmentProf.AssortmentAnchorNodeRid,
                                dtPlaceholders.Rows.Count, 0, assortmentProf.Key);

                        for (int i = 0; i < dtPlaceholders.Rows.Count; i++)
                        {
                            DataRow phRow = dtPlaceholders.Rows[i];
                            int phRid = int.Parse(phRow["HDR_RID"].ToString());
                            string phId = phRow["HDR_ID"].ToString();
                            ApplicationSessionTransaction aPHTrans = SAB.ApplicationServerSession.CreateTransaction();
                            aPHTrans.AddAssortmentMemberProfile(assortmentProf);
                                                                                    
                            AllocationProfile phOrigProfile = new AllocationProfile(aPHTrans, phId, phRid, SAB.ApplicationServerSession);
                            AllocationProfile phProfile = (AllocationProfile)(phOrigProfile).Clone();

                            phProfile.Key = Include.NoRID;
                            phProfile.AsrtRID = assortmentProf.Key;
                            phProfile.AppSessionTransaction.NewAllocationMasterProfileList();
                            int[] selectedHeaderArray = new int[1];
                            selectedHeaderArray[0] = phProfile.Key;
                            phProfile.AppSessionTransaction.AddAllocationProfile(phProfile);
                            phProfile.HeaderID = FindNewAssortmentName(phProfile.HeaderID, copyParms.NewUserKey);
                            phProfile.WriteHeader();

                            if (phProfile.BulkColors != null)
                            {
                                foreach (HdrColorBin hcb in phProfile.BulkColors.Values)
                                {
                                    phProfile.SetColorUnitsToAllocate(hcb.ColorCodeRID, 0);
                                    if (hcb.ColorSizes != null)
                                    {
                                        foreach (HdrSizeBin hsb in hcb.ColorSizes.Values)
                                        {
                                            phProfile.SetSizeUnitsToAllocate(hcb.ColorCodeRID, hsb.SizeCodeRID, 0);
                                        }
                                    }
                                }
                            }


                            ProfileList stores = StoreMgmt.StoreProfiles_GetActiveStoresList(); //SAB.StoreServerSession.GetActiveStoresList();
                            phProfile.SetAllocatedUnits(stores, 0);

                            // Remove Packs
                            if (phProfile.Packs != null)
                            {
                                List<string> packsToRemove = new List<string>();
                                foreach (PackHdr pack in phProfile.Packs.Values)
                                {
                                    packsToRemove.Add(pack.PackName);
                                }
                                foreach (string packName in packsToRemove)
                                {
                                    phProfile.RemovePack(packName);
                                }
                            }

                            //=========================================
                            // Cancel any allocation on Cloned Header
                            //=========================================
                            GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);
                            bool aReviewFlag = false;
                            bool aUseSystemTolerancePercent = false;
                            double aTolerancePercent = Include.DefaultBalanceTolerancePercent;
                            int aStoreFilter = Include.AllStoreFilterRID;
                            int aWorkFlowStepKey = -1;
                            phProfile.StyleHnRID = ((HierarchyNodeProfile)hierarchyNodeList[i]).Key; 
                            ApplicationBaseAction aMethod = phProfile.AppSessionTransaction.CreateNewMethodAction(eMethodType.BackoutAllocation);
                            AllocationWorkFlowStep aAllocationWorkFlowStep
                                    = new AllocationWorkFlowStep(aMethod, aComponent, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
                            phProfile.AppSessionTransaction.DoAllocationAction(aAllocationWorkFlowStep);
                            phProfile.WriteHeader();
                            aPHTrans.Dispose();
                            placeholderHash.Add(phRid, phProfile.Key);
                        }

                        //=========================================
                        // Build Summary data for copied assortment
                        //=========================================
                        List<int> hierNodeList = new List<int>();
                        List<int> versionList = new List<int>();
                        List<int> dateRangeList = new List<int>();
                        List<double> weightList = new List<double>();
                        foreach (AssortmentBasis ab in assortmentProf.AssortmentBasisList)
                        {
                            hierNodeList.Add(ab.HierarchyNodeProfile.Key);
                            versionList.Add(ab.VersionProfile.Key);
                            dateRangeList.Add(ab.HorizonDate.Key);
                            weightList.Add(ab.Weight);
                        }
                        assortmentProf.AssortmentSummaryProfile.ClearAssortmentSummaryTable();
                        assortmentProf.AssortmentSummaryProfile.Process(aTrans, assortmentProf.AssortmentAnchorNodeRid, assortmentProf.AssortmentVariableType, hierNodeList,
                               versionList, dateRangeList, weightList, assortmentProf.AssortmentIncludeSimilarStores, assortmentProf.AssortmentIncludeIntransit,
                               assortmentProf.AssortmentIncludeOnhand, assortmentProf.AssortmentIncludeCommitted, assortmentProf.AssortmentAverageBy, true, true);

                        //===============================
                        // Copy Assortment Properties
                        //===============================
                        assortmentProf.HeaderDataRecord = dlAssortment;
                        assortmentProf.WriteAssortment();
                        assortmentProf.AssortmentSummaryProfile.WriteAssortmentStoreEligibility();
                        assortmentProf.AssortmentSummaryProfile.WriteAssortmentStoreSummary(assortmentProf.HeaderDataRecord);
                        CopyAssortmentStyleClosedList(placeholderHash);

                        dlFolder.Folder_Item_Insert(copyParms.ToParentKey, assortmentProf.Key, eProfileType.AssortmentHeader);

                        dlAssortment.CommitData();
                        dlFolder.CommitData();

                        aTrans.Dispose();

                    }
                    catch (Exception exc)
                    {
                        message = exc.ToString();
                        throw;
                    }
                    finally
                    {
                        dlAssortment.CloseUpdateConnection();
                        dlFolder.CloseUpdateConnection();
                    }


                    return true;
                }
                else
                {
                    throw new Exception("Unexpected Node encountered");
                }
            }
            catch (Exception exc)
            {
                message = exc.ToString();
                throw;
            }
        }

        #endregion "Copy Assortment"

        #region HeadersEnqueueDequeue
        private bool HeadersEnqueued(int nodeKey, out ApplicationSessionTransaction enqueueTransaction, ref string message)
        {
            bool processOK = false;
            enqueueTransaction = null;
            AllocationHeaderProfileList headerList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
            string enqMessage = string.Empty;
            try
            {
                ArrayList selectedNodeAL = new ArrayList();
                selectedNodeAL.Add(nodeKey);

                enqueueTransaction = NewTransFromSelectedHeaders(selectedNodeAL);

                ArrayList headerListAL = GetAllHeadersInAssortment(selectedNodeAL);

                foreach (int hdrRID in headerListAL)
                {
                    AllocationHeaderProfile headerProfile = SAB.HeaderServerSession.GetHeaderData(hdrRID, false, false, true);
                    headerList.Add(headerProfile);
                }

                if (headerList.Count > 0)
                {
                    if (!enqueueTransaction.EnqueueHeaders(headerList, out enqMessage))
                    {
                        throw new HeaderConflictException();
                    }
                }

                processOK = true;
            }
            catch (HeaderConflictException)
            {
                message = enqMessage;
            }
            catch (Exception exc)
            {
                message = exc.ToString();
                throw;
            }
            return processOK;
        }

        public void DequeueHeaders(ApplicationSessionTransaction enqueueTransaction)
        {
            try
            {
                enqueueTransaction.DequeueHeaders();
            }
            catch
            {
                throw;
            }
        }

        #endregion HeadersEnqueueDequeue

        #region Worklist Utilities

        private ApplicationSessionTransaction NewTransFromSelectedHeaders(ArrayList selectedNodes)
        {
            try
            {
                ApplicationSessionTransaction newTrans = SAB.ApplicationServerSession.CreateTransaction();


                newTrans.NewAllocationMasterProfileList();

                ArrayList selectedHeaderKeyList = GetAllHeadersInAssortment(selectedNodes);

                int[] selectedHeaderArray = new int[selectedHeaderKeyList.Count];
                selectedHeaderKeyList.CopyTo(selectedHeaderArray);

                int[] selectedAssortmentArray = new int[selectedNodes.Count];

                ArrayList selectedAssortmentKeyList = new ArrayList();
                foreach (int nodeRID in selectedNodes)
                {
                    selectedAssortmentKeyList.Add(nodeRID);
                }
                selectedAssortmentKeyList.CopyTo(selectedAssortmentArray);

                newTrans.LoadHeaders(selectedAssortmentArray, selectedHeaderArray);
                return newTrans;
            }
            catch
            {
                throw;
            }
        }

        private ArrayList GetAllHeadersInAssortment(ArrayList selectedNodes)
        {
            ArrayList selectedHeaderKeyList = new ArrayList();
            try
            {
                foreach (int nodeRID in selectedNodes)
                {
                    ArrayList al = SAB.HeaderServerSession.GetHeadersInAssortment(nodeRID);
                    for (int i = 0; i < al.Count; i++)
                    {
                        int hdrRID = (int)al[i];
                        if (hdrRID != nodeRID)
                        {
                            selectedHeaderKeyList.Add(hdrRID);
                        }
                    }
                }
                return selectedHeaderKeyList;
            }
            catch
            {
                throw;
            }
        }

        public string FindNewAssortmentName(string aAssortmentName, int aUserRID)
        {
            int index;
            string newName;
            int key;
            Header dlAssortment;

            try
            {
                dlAssortment = new Header();
                index = 0;
                newName = aAssortmentName;
                key = dlAssortment.HeaderAssortment_GetKey(newName);

                while (key != -1)
                {
                    index++;

                    //if (index > 1)
                    //{
                    //    newName = "Copy (" + index + ") of " + aAssortmentName;
                    //}
                    //else
                    //{
                    //    newName = "Copy of " + aAssortmentName;
                    //}
                    newName = Include.GetNewName(name: aAssortmentName, index: index);

                    key = dlAssortment.HeaderAssortment_GetKey(newName);
                }

                return newName;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void CopyAssortmentStyleClosedList(Hashtable placeholderHash)
        {

            AssortmentDetailData assortDetailData = new AssortmentDetailData();
            try
            {
                if (!assortDetailData.ConnectionIsOpen)
                    assortDetailData.OpenUpdateConnection();

                assortDetailData.AssortmentStyleClosed_Copy(placeholderHash);
                assortDetailData.CommitData();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (assortDetailData.ConnectionIsOpen)
                {
                    assortDetailData.CloseUpdateConnection();
                }
            }
        }
        #endregion Worklist Utilities
    }


}
