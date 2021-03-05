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
    public partial class ROAllocation : ROWebFunction
    {
        //=======
        // FIELDS
        //=======
        private FunctionSecurityProfile _allocationNonInterfacedHeadersSecurity;
        private FunctionSecurityProfile _allocationInterfacedHeadersSecurity;
        private ArrayList _hdrsInGroups = new ArrayList();
        private ArrayList _headerProfileArrayList;
        private HeaderCharGroupProfileList _headerCharGroupProfileList = null;
        private int _headerFilterRID = Include.NoRID;
        private int _currentWorklistViewRID = Include.NoRID;
        private Hashtable _colorsForStyle = new Hashtable();
        private Hashtable _nodeDataHash = new Hashtable();
        internal AllocationProfileList _allocProfileList;
        private ArrayList _masterKeyList;
        private Hashtable _headersAddedToMulti;
        private Hashtable _headersRemovedFromMulti;
        private UserGridView _userGridView;
        private GridViewData _gridViewData;
        private bool _fromFilterWindow = false;
        private bool _filterChangedAfterView = false;
        private FilterData _filterData;
        private bool _allowHeaderDelete = true;
        private ArrayList _userRIDList;
        private DataTable _dtView;

        private DataSet _dsDetails;
        private DataSet _dsDetailsSaved;
        private DataTable _dtBulkColor;
        private DataTable _dtPack;
        private DataTable _dtPackColor;
        private DataTable _dtDetailHeader;
        private bool _inClearDetails = false;
        private int _nodeDataHashLastKey = 0;
        private HierarchyNodeProfile _nodeDataHashLastValue;
        private string _dupSizeNameSeparator;
        private int _sizeGroupHashLastKey = 0;
        private Hashtable _sizeGroupHash = new Hashtable();
        private SizeGroupProfile _sizeGroupHashLastValue;
        private bool _addingSizes = false;
        private Hashtable _addedColorSizeHash = new Hashtable();

        private FunctionSecurityProfile _assortmentSecurity;
        private Hashtable _workflowNameHash = new Hashtable();
        private ArrayList _selectedFilterHeaderList;
        private int _selectedFilterId = Include.NoRID;
        private DataTable _dtHeaderViewFieldMapping = null;

        private ArrayList _selectedHeaderKeyList;

        private List<ROAllocationWorklistOut> _allocationWorklistOut;
        //=================
        // PRIVATE METHODS
        //=================

        #region "Get Header Data"
        public ROOut GetAllocationHeaderData()
        {


            try
            {
                if (SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAllocationWorkspace).AccessDenied)
                {
                    return null;
                }

                _allocationNonInterfacedHeadersSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationHeadersNonInterfaced);
                _allocationInterfacedHeadersSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationHeadersInterfacedHeader);

                if (!_allocationNonInterfacedHeadersSecurity.AllowDelete &&
                    !_allocationInterfacedHeadersSecurity.AllowDelete)
                {
                    _allowHeaderDelete = false;
                }

                GetHeaderFilters();

                return LoadHeaders();


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        #endregion

        #region "Get Header Filters"
        internal void GetHeaderFilters()
        {
            _userGridView = new UserGridView();
            _gridViewData = new GridViewData();
            _filterData = new FilterData();

            FilterHeaderOptions headerFilterOptions = new FilterHeaderOptions();
            headerFilterOptions.USE_WORKSPACE_FIELDS = true;
            headerFilterOptions.filterType = filterTypes.HeaderFilter;

            int viewRID = _userGridView.UserGridView_Read(SAB.ClientServerSession.UserRID, eLayoutID.allocationWorkspaceGrid);
            bool useViewWorkspaceFilter = false;
            bool useFilterSorting = false;

            if (viewRID != Include.NoRID && !(_fromFilterWindow || _filterChangedAfterView))
            {
                int workspaceFilterRID = _gridViewData.GridViewReadWorkspaceFilterRID(viewRID, ref useFilterSorting);
                if (workspaceFilterRID != Include.NoRID)
                {
                    useViewWorkspaceFilter = true;
                    this._headerFilterRID = workspaceFilterRID;
                }
            }

            if (useViewWorkspaceFilter == false) // use the current user workspace filter
            {
                this._headerFilterRID = _filterData.WorkspaceCurrentFilter_Read(SAB.ClientServerSession.UserRID, eWorkspaceType.AllocationWorkspace);
            }
        }

        #endregion

        #region "Load Allocation Header Lists"
        internal ROOut LoadHeaders()
        {
            ClearAllocationHeaders();

            _allocProfileList = new AllocationProfileList(eProfileType.Allocation);

            FilterHeaderOptions headerFilterOptions = new FilterHeaderOptions();
            headerFilterOptions.USE_WORKSPACE_FIELDS = true;
            headerFilterOptions.filterType = filterTypes.HeaderFilter;

            _headerProfileArrayList = SAB.HeaderServerSession.GetHeadersForWorkspace(this._headerFilterRID, headerFilterOptions);

            _masterKeyList = new ArrayList();
            _headersAddedToMulti = new Hashtable();
            _headersRemovedFromMulti = new Hashtable();

            ROIListOut ROHeaderSummaryList = new ROIListOut(eROReturnCode.Successful, null, ROInstanceID, BuildHeaderSummaryList(_headerProfileArrayList));

            return ROHeaderSummaryList;

        }

        #endregion

        #region "Clear Allocation Header List"
        internal void ClearAllocationHeaders()
        {
            if (_allocProfileList != null)
            {
                _allocProfileList.Clear();
                _allocProfileList = null;
            }

            if (_headerProfileArrayList != null)
            {
                _headerProfileArrayList.Clear();
                _headerProfileArrayList = null;
            }
            if (_headerCharGroupProfileList != null)
            {
                _headerCharGroupProfileList.Clear();
                _headerCharGroupProfileList = null;
            }
        }
        #endregion

        #region "Method to get the Allocation Views"
        private ROOut GetAllocationViewsData()
        {
            List<ROWorklistViewOut> views = GetViews();

            _bindingView = false;

            return new ROIListOut(eROReturnCode.Successful, null, ROInstanceID, views);

        }
        #endregion

        #region Method to get the Allocation Action
        private ROOut GetAllocationActionsData()
        {
            try
            {
                return new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, GetAllocationActions());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region "Method to get the Allocation User Last Data"
        private ROOut GetAllocationUserLastValues()
        {
            ROAllocationWorklistValues userLastValues = GetUserLastValues();
            ROAllocationWorklistValuesOut userLastValuesOut = new ROAllocationWorklistValuesOut(eROReturnCode.Successful, null, ROInstanceID, userLastValues);

            return userLastValuesOut;
        }
        #endregion

        #region "Method to save the Allocation User Last Data"
        private ROOut SaveUserLastValues(ROAllocationWorklistLastDataParms rOAllocationWorklistLastDataParms)
        {
            try
            {
                SaveAllocationUserLastValues(rOAllocationWorklistLastDataParms);

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

        #region "Reference methods"
        internal List<ROAllocationHeaderSummary> BuildHeaderSummaryList(ArrayList headerProfileList, bool includeDetails = false, bool includeCharacteristics = false, bool excludeAssortmentTypeHeaders = true, bool excludePlaceholderTypeHeaders = true)
        {
            List<ROAllocationHeaderSummary> headerList = new List<ROAllocationHeaderSummary>();

            if (_headerCharGroupProfileList == null)
            {
                _headerCharGroupProfileList = SAB.HeaderServerSession.GetHeaderCharGroups();
            }

            _assortmentSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.Assortment);

            Hashtable sizeGroupHash = new Hashtable();
            Hashtable workflowNameHash = new Hashtable();
            Header headerData = new Header();

            foreach (AllocationHeaderProfile lstItem in headerProfileList)
            {
                if (lstItem.AsrtType == (int)eAssortmentType.GroupAllocation
                    || lstItem.AsrtTypeForParentAsrt == (int)eAssortmentType.GroupAllocation)
                {
                    continue;
                }
                if (excludeAssortmentTypeHeaders)
                {
                    if (lstItem.AsrtType == (int)eAssortmentType.GroupAllocation
                        || lstItem.HeaderType == eHeaderType.Assortment
                        || (lstItem.HeaderType == eHeaderType.Placeholder && excludePlaceholderTypeHeaders)
                        )
                    {
                        continue;
                    }
                }
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
                        foreach (HeaderPackProfile pack in lstItem.Packs.Values)
                        {
                            balance -= pack.Packs * pack.Multiple;
                        }
                    }
                    if (lstItem.BulkColors != null && lstItem.BulkColors.Count > 0)
                    {
                        foreach (HeaderBulkColorProfile color in lstItem.BulkColors.Values)
                        {
                            balance -= color.Units;
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
                var digitalAssetKey = hnp_style.DigitalAssetKey;

                ROAllocationHeaderSummary headerSummary = new ROAllocationHeaderSummary(aKey, headerID, headerDescription, headerReceiptDay,
                                                                    originalReceiptDay, unitRetail, unitCost, styleHnRID,
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
                                                                    unitsPerCarton, DCFulfillmentProcessed,
                                                                    headerType, headerTypeText, canView, canUpdate,
                                                                    headerStatus, headerStatusText, intransitStatus,
                                                                    intransitStatusText, shipStatus, shipStatusText, hnp_style, parentID, styleID, numberOfStores, packCount,
                                                                    bulkColorCount, bulkColorSizeCount, sizeGroupName, subordID, masterSubord,
                                                                    digitalAssetKey, adjustVSW, groupAllocRid,
                                                                    asrtSortSeq, storetot, vSWtot, balance, otsForecast, onHand, gradeInvBasis,
                                                                    headerCharacteristics);

                if (includeDetails)
                {
                    HeaderDetails(hdrProf: lstItem, headerSummary: headerSummary);
                }

                if (includeCharacteristics
                   || includeDetails)
                {
                    LoadProductCharacteristicsAndDigitalAssetKeys(headerSummary: headerSummary, includeDetails: includeDetails, includeCharacteristics: includeCharacteristics);
                }

                headerList.Add(headerSummary);

            }

            return headerList;
        }

        private void LoadProductCharacteristicsAndDigitalAssetKeys(ROAllocationHeaderSummary headerSummary, bool includeDetails, bool includeCharacteristics)
        {
            string inheritedFromBase = MIDText.GetTextOnly(eMIDTextCode.lbl_Inherited_From);

            ProductCharProfileList productCharProfileList = null;
            if (includeCharacteristics)
            {
                productCharProfileList = SAB.HierarchyServerSession.GetProductCharacteristics();

                headerSummary.ProductCharacteristics = new List<ROCharacteristic>();
                headerSummary.ProductCharacteristics.AddRange(BuildProductCharacteristics(productCharProfileList, headerSummary.StyleHnRID, inheritedFromBase));
            }

            if (includeDetails)
            {
                HierarchyNodeProfile hnp_style = SAB.HierarchyServerSession.GetNodeData(headerSummary.StyleHnRID);
                int colorHnRID = Include.NoRID;
                foreach (ROHeaderBulkColorDetails color in headerSummary.BulkColorDetails)
                {
                    ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(color.Color.Key);

                    if (SAB.HierarchyServerSession.ColorExistsForStyle(hnp_style.HomeHierarchyRID, hnp_style.Key, ccp.ColorCodeID, ref colorHnRID))
                    {
                        if (includeCharacteristics)
                        {
                            color.ProductCharacteristics = new List<ROCharacteristic>();
                            color.ProductCharacteristics.AddRange(BuildProductCharacteristics(productCharProfileList, colorHnRID, inheritedFromBase));
                        }

                        HierarchyNodeProfile colorHierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeData(colorHnRID, true);
                        color.DigitalAssetKey = colorHierarchyNodeProfile.DigitalAssetKey;
                    }
                }

                foreach (ROHeaderPackProfile pack in headerSummary.PackDetails)
                {
                    foreach (HeaderPackColor color in pack.ColorsInfo)
                    {
                        ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(color.Color.Key);

                        if (SAB.HierarchyServerSession.ColorExistsForStyle(hnp_style.HomeHierarchyRID, hnp_style.Key, ccp.ColorCodeID, ref colorHnRID))
                        {
                            if (includeCharacteristics)
                            {
                                color.ProductCharacteristics = new List<ROCharacteristic>();
                                color.ProductCharacteristics.AddRange(BuildProductCharacteristics(productCharProfileList, colorHnRID, inheritedFromBase));
                            }

                            HierarchyNodeProfile colorHierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeData(colorHnRID, true);
                            color.DigitalAssetKey = colorHierarchyNodeProfile.DigitalAssetKey;
                        }
                    }
                }
            }
        }
        private List<ROCharacteristic> BuildProductCharacteristics(ProductCharProfileList productCharProfileList, int nodeKey, string inheritedFromBase)
        {
            string inheritedFrom;
            List<ROCharacteristic> characteristics = new List<ROCharacteristic>();

            NodeCharProfileList nodeCharProfileList = SAB.HierarchyServerSession.GetProductCharacteristics(nodeKey, true);
            if (nodeCharProfileList.Count > 0)
            {
                foreach (ProductCharProfile productCharProfile in productCharProfileList)
                {
                    NodeCharProfile nodeCharProfile = (NodeCharProfile)nodeCharProfileList.FindKey(productCharProfile.Key);

                    inheritedFrom = null;
                    if (nodeCharProfile != null)
                    {
                        if (nodeCharProfile.InheritedFrom != nodeKey
                            && nodeCharProfile.TypeInherited == eInheritedFrom.Node)
                        {
                            HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(nodeCharProfile.InheritedFrom);
                            inheritedFrom = inheritedFromBase + hnp.Text;
                        }

                        ROCharacteristic characteristic = new ROCharacteristic(characteristicGroupKey: productCharProfile.Key, characteristicGroupName: productCharProfile.ProductCharID,
                            characteristicValueKey: nodeCharProfile == null ? Include.NoRID : nodeCharProfile.ProductCharValueRID,
                            characteristicValueName: nodeCharProfile == null ? string.Empty : nodeCharProfile.ProductCharValue,
                            inheritedFrom: inheritedFrom);

                        characteristics.Add(characteristic);
                    }
                }
            }

            return characteristics;
        }


        internal ArrayList UserRIDList()
        {
            if (_userRIDList == null)
            {
                _userRIDList = new ArrayList();
                _userRIDList.Add(Include.GlobalUserRID);
                _userRIDList.Add(SAB.ClientServerSession.UserRID);
            }
            return _userRIDList;
        }

        internal List<ROWorklistViewOut> GetViews()
        {
            _gridViewData = new GridViewData();
            _bindingView = true;

            List<ROWorklistViewOut> views = new List<ROWorklistViewOut>();

            _dtView = _gridViewData.GridView_Read((int)eLayoutID.allocationWorkspaceGrid, UserRIDList(), true);
            _dtView.Rows.Add(new object[] { Include.NoRID, SAB.ClientServerSession.UserRID, (int)eLayoutID.allocationWorkspaceGrid, "  " });


            DataView dv = new DataView(_dtView);

            foreach (DataRowView rowView in dv)
            {
                int viewRID = Convert.ToInt32(rowView.Row["VIEW_RID"]);
                string viewName = Convert.ToString(rowView.Row["VIEW_ID"]);
                int filterRID = (rowView["WORKSPACE_FILTER_RID"] != DBNull.Value) ? Convert.ToInt32(rowView["WORKSPACE_FILTER_RID"]) : Include.NoRID;
                views.Add(new ROWorklistViewOut(viewRID, viewName, filterRID));
            }


            return views;
        }

        internal List<KeyValuePair<int, string>> GetAllocationActions()
        {
            DataTable dtAction = new DataTable();
            List<KeyValuePair<int, string>> allocationActionsList = new List<KeyValuePair<int, string>>();

            dtAction = MIDText.GetLabels((int)eAllocationActionType.StyleNeed, (int)eAllocationActionType.BalanceToVSW);
            DataRow dr;
            Hashtable removeEntry = new Hashtable();
            removeEntry.Add(Convert.ToInt32(eAllocationActionType.BackoutDetailPackAllocation), eAllocationActionType.BackoutDetailPackAllocation);
            removeEntry.Add(Convert.ToInt32(eAllocationActionType.ChargeSizeIntransit), eAllocationActionType.ChargeSizeIntransit);
            removeEntry.Add(Convert.ToInt32(eAllocationActionType.DeleteHeader), eAllocationActionType.DeleteHeader);

            if (!SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
            {
                removeEntry.Add(Convert.ToInt32(eAllocationActionType.BackoutSizeAllocation), eAllocationActionType.BackoutSizeAllocation);
                removeEntry.Add(Convert.ToInt32(eAllocationActionType.BackoutSizeIntransit), eAllocationActionType.BackoutSizeIntransit);
                removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeNoSubs), eAllocationActionType.BalanceSizeNoSubs);
                removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeWithSubs), eAllocationActionType.BalanceSizeWithSubs);
                removeEntry.Add(Convert.ToInt32(eAllocationActionType.BreakoutSizesAsReceived), eAllocationActionType.BreakoutSizesAsReceived);
                removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeWithConstraints), eAllocationActionType.BalanceSizeWithConstraints); // TT#843 - New Size Constraint Balance
                removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeWithConstraints), eAllocationActionType.BalanceSizeBilaterally); // TT#794 - New Size Balance for Wet Seal
                removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeWithConstraints), eAllocationActionType.BreakoutSizesAsReceivedWithConstraints); // TT#1391 - JEllis - Balance Size With Constraint Other Options
            }

            int codeValue;
            for (int i = dtAction.Rows.Count - 1; i >= 0; i--)
            {
                dr = dtAction.Rows[i];
                codeValue = Convert.ToInt32(dr["TEXT_CODE"]);
                if (removeEntry.Contains(codeValue))
                {
                    dtAction.Rows.Remove(dr);
                }
                else if (!Enum.IsDefined(typeof(eAllocationActionType), (eAllocationActionType)codeValue))
                {
                    dtAction.Rows.Remove(dr);
                }
            }

            dtAction.Rows.Add(new object[] { Include.NoRID, MIDText.GetTextOnly(eMIDTextCode.lbl_SelectAction), 0, 0, 0, 0 });

            CheckSecurityForActions(dtAction);

            DataView dvAction = new DataView(dtAction);
            dvAction.Sort = "TEXT_ORDER";
            foreach (DataRowView rowView in dvAction)
            {
                int actionID = Convert.ToInt32(rowView.Row["TEXT_CODE"]);
                string actionName = Convert.ToString(rowView.Row["TEXT_VALUE"]);
                allocationActionsList.Add(new KeyValuePair<int, string>(actionID, actionName));
            }
            return allocationActionsList;
        }

        internal void CheckSecurityForActions(DataTable dtAction)
        {
            bool allowAction = true;
            FunctionSecurityProfile actionSecurity = null;

            foreach (int action in Enum.GetValues(typeof(eAllocationActionType)))
            {
                allowAction = true;
                actionSecurity = SAB.ClientServerSession.GetMyUserActionSecurityAssignment((eAllocationActionType)action, true);
                if (actionSecurity.AccessDenied)
                {
                    allowAction = false;
                }
                else
                {
                    actionSecurity = SAB.ClientServerSession.GetMyUserActionSecurityAssignment((eAllocationActionType)action, false);
                    if (actionSecurity.AccessDenied)
                    {
                        allowAction = false;
                    }
                }

                if (!allowAction)
                {
                    for (int i = dtAction.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dtAction.Rows[i];
                        if (Convert.ToInt32(dr["TEXT_CODE"]) == action)
                        {
                            dtAction.Rows.Remove(dr);
                        }
                    }
                }
            }
        }

        #region Application Session Transaction

        internal ApplicationSessionTransaction GetApplicationSessionTransaction(bool getNewTransaction = false)
        {
            //ApplicationSessionTransaction trans = SAB.ApplicationServerSession.CreateTransaction();
            //trans.NewAllocationMasterProfileList();
            //return trans;
            if (_applicationSessionTransaction == null
                || getNewTransaction)
            {
                if (_applicationSessionTransaction != null)
                {
                    _applicationSessionTransaction.Dispose();
                }
                _applicationSessionTransaction = SAB.ApplicationServerSession.CreateTransaction();
                _applicationSessionTransaction.NewAllocationMasterProfileList();
            }
            return _applicationSessionTransaction;
        }

        #endregion

        #endregion

        #region "Get Selected Header list"

        private ROOut SetAllocationSelectedHeaders(ROListParms selectedHeaders)
        {
            string message = string.Empty;
            ArrayList _selectedHeaderKeyList = new ArrayList();
            GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);
            ArrayList selectedAssortmentList = new ArrayList(); //to pass an empty seleted assortement list 
            try
            {
                bool getNewTransaction = false;

                var selectedHeaderList4mSession = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();

                if (selectedHeaderList4mSession.ArrayList.Count > 0)
                {
                    // get a new ApplicationSessionTransaction if header list is different
                    if (selectedHeaders.ListValues.Count != selectedHeaderList4mSession.ArrayList.Count)
                    {
                        getNewTransaction = true;
                    }
                    else
                    {
                        foreach (int headerKey in selectedHeaders.ListValues)
                        {
                            bool foundHeader = false;
                            foreach (SelectedHeaderProfile selectedHeaderProfile in selectedHeaderList4mSession.ArrayList)
                            {
                                if (selectedHeaderProfile.Key == headerKey)
                                {
                                    foundHeader = true;
                                    break;
                                }
                            }
                            if (!foundHeader)
                            {
                                getNewTransaction = true;
                            }
                        }
                    }

                    SAB.ClientServerSession.ClearSelectedHeaderList();
                    SAB.ClientServerSession.ClearSelectedComponentList();
                    // if have a transaction and not getting new transaction, remove subtotals to rebuild
                    if (_applicationSessionTransaction != null
                        && !getNewTransaction)
                    {
                        AllocationSubtotalProfileList subtotalList;
                        eProfileType profileType = eProfileType.AllocationSubtotal;
                        subtotalList = (AllocationSubtotalProfileList)_applicationSessionTransaction.GetMasterProfileList(profileType);
                        if (subtotalList != null)
                        {
                            foreach (AllocationSubtotalProfile allocationSubtotalProfile in subtotalList)
                            {
                                allocationSubtotalProfile.RemoveAllSubtotalMembers();
                            }
                            _applicationSessionTransaction.RemoveAllocationSubtotalProfileList();
                        }
                    }
                }

                // if getting new transaction, clear the ROWorkflowMethodManager so new one is built for different headers
                if (getNewTransaction
                    && _ROWorkflowMethodManager != null)
                {
                    _ROWorkflowMethodManager.CleanUp();
                    _ROWorkflowMethodManager = null;
                }

                _applicationSessionTransaction = GetApplicationSessionTransaction(getNewTransaction: getNewTransaction);

                _applicationSessionTransaction.LoadHeadersInTransaction(selectedHeaders.ListValues, selectedAssortmentList, true, false);

                for (int i = 0; i < selectedHeaders.ListValues.Count; i++)
                {
                    int hdrRid = Convert.ToInt32(selectedHeaders.ListValues[i].ToString());
                    AllocationHeaderProfile ahp = SAB.HeaderServerSession.GetHeaderData(hdrRid, false, false, true);
                    SAB.ClientServerSession.AddSelectedHeaderList(ahp.Key, ahp.HeaderID, ahp.HeaderType, ahp.AsrtRID, ahp.StyleHnRID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return new RONoDataOut(eROReturnCode.Successful, null, ROInstanceID);
        }

        #endregion

        #region Allocation Workflow Steps
        /// <summary>
        /// Get the Allocation Worklist view
        /// </summary>
        /// <param name="rOKeyParams"></param>
        /// <returns>List of ROAllocationWorklistOut containing information for each column in the view</returns>
        public ROOut GetAllocationWorklistViewDetails(ROKeyParms rOKeyParams)
        {
            try
            {
                _allocationWorklistOut = DataTableToWorklist(BuildAllocationWorklistData(rOKeyParams));
                return new ROIListOut(eROReturnCode.Successful, null, ROInstanceID, _allocationWorklistOut);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal List<ROAllocationWorklistOut> DataTableToWorklist(DataTable dataTable)
        {
            if (_dtHeaderViewFieldMapping == null)
            {
                BuildHeaderViewFieldMapping();
            }

            List<ROAllocationWorklistOut> allocationWorklistOuts = new List<ROAllocationWorklistOut>();

            if (dataTable.Rows.Count > 0)
            {
                for (int iCounter = 0; iCounter < dataTable.Rows.Count; iCounter++)
                {
                    int viewRID = Convert.ToInt32(dataTable.Rows[iCounter]["VIEW_RID"].ToString());
                    string bandKey = dataTable.Rows[iCounter]["BAND_KEY"].ToString();
                    string columnKey = dataTable.Rows[iCounter]["COLUMN_KEY"].ToString();
                    int visiblePos = Convert.ToInt32(dataTable.Rows[iCounter]["VISIBLE_POSITION"].ToString());
                    bool isHidden = Include.ConvertCharToBool(Convert.ToChar(dataTable.Rows[iCounter]["IS_HIDDEN"], CultureInfo.CurrentUICulture));
                    bool isGroupByCol = Include.ConvertCharToBool(Convert.ToChar(dataTable.Rows[iCounter]["IS_GROUPBY_COL"], CultureInfo.CurrentUICulture));
                    int sortDir = Convert.ToInt32(dataTable.Rows[iCounter]["SORT_DIRECTION"].ToString());
                    int sortSeq = 0;
                    if (!string.IsNullOrEmpty(dataTable.Rows[iCounter]["SORT_SEQUENCE"].ToString()))
                    {
                        sortSeq = Convert.ToInt32(dataTable.Rows[iCounter]["SORT_SEQUENCE"].ToString());

                    }
                    int width = Convert.ToInt32(dataTable.Rows[iCounter]["WIDTH"].ToString());
                    string columnType = dataTable.Rows[iCounter]["COLUMN_TYPE"].ToString();
                    string hcgRid = dataTable.Rows[iCounter]["HCG_RID"].ToString();
                    string label = null;
                    string itemField = null;
                    DataRow[] dr;
                    if (hcgRid.Trim().Length > 0)
                    {
                        dr = _dtHeaderViewFieldMapping.Select("HCG_RID='" + hcgRid + "'");
                    }
                    else
                    {
                        dr = _dtHeaderViewFieldMapping.Select("ViewKey='" + columnKey + "'");
                    }
                    if (dr.Length > 0)
                    {
                        label = Convert.ToString(dr[0]["Label"], CultureInfo.CurrentUICulture);
                        itemField = Convert.ToString(dr[0]["ItemField"], CultureInfo.CurrentUICulture);
                    }
                    else if (!isHidden)
                    {
                        ROWebTools.LogMessage(ROWebCommon.eROMessageLevel.Error, "Could not find mapping for " + columnKey);
                    }

                    allocationWorklistOuts.Add(new ROAllocationWorklistOut(
                         viewRID, bandKey, columnKey, visiblePos, isHidden, isGroupByCol, sortDir, sortSeq, width, columnType, hcgRid, label, itemField));
                }
            }

            return allocationWorklistOuts;
        }
        internal DataTable BuildAllocationWorklistData(ROKeyParms roKeyParams)
        {
            GridViewData gridViewData = new GridViewData();
            _currentWorklistViewRID = roKeyParams.Key;
            DataTable dtWorklistViewDetail = gridViewData.GridViewDetail_Read_ByPosition(roKeyParams.Key);

            return dtWorklistViewDetail;
        }
        #endregion

        /// <summary>
        /// Saves the Allocation Worklist view to the database
        /// </summary>
        /// <param name="viewDetails"></param>
        /// <returns>List of ROAllocationWorklistOut containing information for each column in the view</returns>
        public ROOut SaveAllocationWorklistViewDetails(ROAllocationWorklistViewDetailsParms viewDetails)
        {
            string message = null;
            int viewKey, viewUserKey;
            // columnType indicates the type of data.  "B" is base header value; "C" is header characteristic value.
            string columnType;
            int headerCharacteristicGroupKey;
            GridViewData gridViewData = new GridViewData();
            eLayoutID layoutID;
            string columnKey;
            int result;

            try
            {
                // make sure at least one variable is selected
                bool variableIsSelected = false;
                foreach (ROAllocationWorklistEntry entry in viewDetails.ROAllocationWorklistViewDetails.ViewDetails)
                {
                    if (!entry.IsHidden)
                    {
                        variableIsSelected = true;
                        break;
                    }
                }

                if (!variableIsSelected)
                {
                    return new ROIListOut(
                        ROReturnCode: eROReturnCode.Failure,
                        sROMessage: SAB.ClientServerSession.Audit.GetText(messageCode: eMIDTextCode.msg_NeedAtLeastOneVariable, addToAuditReport: true),
                        ROInstanceID: ROInstanceID,
                        ROIListOutput: viewDetails.ROAllocationWorklistViewDetails.ViewDetails);
                }

                if (viewDetails.ROAllocationWorklistViewDetails.IsUserView)
                {
                    viewUserKey = SAB.ClientServerSession.UserRID;
                }
                else
                {
                    viewUserKey = Include.GlobalUserRID;
                }

                layoutID = eLayoutID.allocationWorkspaceGrid;
                int workspaceFilterRID = Include.NoRID;
                if (viewDetails.ROAllocationWorklistViewDetails.FilterIsSet)
                {
                    workspaceFilterRID = viewDetails.ROAllocationWorklistViewDetails.Filter.Key;
                }
                bool useFilterSorting = false;

                gridViewData.OpenUpdateConnection();

                try
                {
                    viewKey = gridViewData.GridView_GetKey(viewUserKey, (int)layoutID, viewDetails.ROAllocationWorklistViewDetails.View.Value);
                    if (viewKey != Include.NoRID)
                    {
                        gridViewData.GridViewDetail_Delete(viewKey);
                        gridViewData.GridView_Update(viewKey, false, Include.NoRID, Include.NoRID, false, workspaceFilterRID, useFilterSorting);
                    }
                    else
                    {
                        viewKey = gridViewData.GridView_Insert(viewUserKey, (int)layoutID, viewDetails.ROAllocationWorklistViewDetails.View.Value, false, Include.NoRID, Include.NoRID, false, workspaceFilterRID, useFilterSorting);
                        string viewName = viewDetails.ROAllocationWorklistViewDetails.View.Value;
                        viewDetails.ROAllocationWorklistViewDetails.View = new KeyValuePair<int, string>(viewKey, viewName);
                    }

                    columnType = "B";
                    headerCharacteristicGroupKey = Include.NoRID;

                    HeaderCharacteristicsData headerCharacteristicsData = new HeaderCharacteristicsData();

                    foreach (ROAllocationWorklistEntry entry in viewDetails.ROAllocationWorklistViewDetails.ViewDetails)
                    {
                        columnKey = entry.ColumnKey;

                        headerCharacteristicGroupKey = Include.NoRID;
                        columnType = "B";
                        if (!string.IsNullOrEmpty(entry.HeaderCharacteristicGroupKey))
                        {
                            if (int.TryParse(entry.HeaderCharacteristicGroupKey, out result))
                            {
                                headerCharacteristicGroupKey = result;
                                DataTable HCGdt = headerCharacteristicsData.HeaderCharGroup_Read(headerCharacteristicGroupKey);
                                if (HCGdt.Rows.Count > 0)
                                {
                                    DataRow HCGdr = HCGdt.Rows[0];
                                    columnKey = (string)HCGdr["HCG_ID"];
                                    columnType = "C";
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }

                        gridViewData.GridViewDetail_Insert(viewKey, "Header", columnKey, entry.VisiblePosition, entry.IsHidden,
                                                  entry.IsGroupByColumn, entry.SortDirection, entry.SortSequence, entry.Width,
                                                  columnType, headerCharacteristicGroupKey);

                    }

                    gridViewData.CommitData();
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
                }
            }
            catch (Exception exc)
            {
                message = exc.ToString();
                throw;
            }

            ROKeyParms rOKeyParams = new ROKeyParms(
                sROUserID: viewDetails.ROUserID,
                sROSessionID: viewDetails.ROSessionID,
                ROClass: viewDetails.ROClass,
                RORequest: eRORequest.AllocationWorklistView,
                ROInstanceID: viewDetails.ROInstanceID,
                iKey: viewDetails.ROAllocationWorklistViewDetails.View.Key
                );

            return GetAllocationWorklistViewDetails(rOKeyParams);
        }

        public ROOut DeleteAllocationWorklistViewDetails()
        {
            return DeleteViewDetails(viewKey: _currentWorklistViewRID);
        }

        #region Build Header View Field Mapping
        private void BuildHeaderViewFieldMapping()
        {
            HierarchyProfile mainHp = SAB.HierarchyServerSession.GetMainHierarchyData();
            string styleLevelID = null;
            string productLevelID = null;
            for (int level = 1; level <= mainHp.HierarchyLevels.Count; level++)
            {
                productLevelID = styleLevelID;
                styleLevelID = ((HierarchyLevelProfile)mainHp.HierarchyLevels[level]).LevelID;

                if (((HierarchyLevelProfile)mainHp.HierarchyLevels[level]).LevelType == eHierarchyLevelType.Style)
                {
                    break;
                }
            }

            _dtHeaderViewFieldMapping = new DataTable("HeaderViewFieldMapping");
            _dtHeaderViewFieldMapping.Columns.Add("ViewKey");
            _dtHeaderViewFieldMapping.Columns.Add("Label");
            _dtHeaderViewFieldMapping.Columns.Add("ItemField");
            _dtHeaderViewFieldMapping.Columns.Add("HCG_RID");

            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "HeaderID", "ID", "HeaderID" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "HdrGroupRID", MIDText.GetTextOnly(eMIDTextCode.lbl_MultiHeaderID), "HeaderGroupRID" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "AsrtRID", MIDText.GetTextOnly(eMIDTextCode.lbl_AssortmentID), "AsrtRID" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "GroupAllocRID", MIDText.GetTextOnly(eMIDTextCode.lbl_GroupAllocationID), "GroupAllocRid" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "Type", MIDText.GetTextOnly(eMIDTextCode.lbl_Type), "HeaderTypeText" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "Date", MIDText.GetTextOnly(eMIDTextCode.lbl_Date), "HeaderDay" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "Status", MIDText.GetTextOnly(eMIDTextCode.lbl_HeaderStatus), "HeaderStatusText" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "AnchorNode", MIDText.GetTextOnly(eMIDTextCode.lbl_ApplyTo), "AnchorNode" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "DateRange", MIDText.GetTextOnly(eMIDTextCode.lbl_Delivery), "DateRange" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "Product", productLevelID, "ParentID" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "Style", styleLevelID, "StyleID" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "Description", MIDText.GetTextOnly(eMIDTextCode.lbl_WorkspaceDescription), "HeaderDescription" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "HdrQuantity", MIDText.GetTextOnly(eMIDTextCode.lbl_Quantity), "TotalUnitsToAllocate" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "Balance", MIDText.GetTextOnly(eMIDTextCode.lbl_Balance), "Balance" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "UnitRetail", MIDText.GetTextOnly(eMIDTextCode.lbl_UnitRetail), "UnitRetail" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "UnitCost", MIDText.GetTextOnly(eMIDTextCode.lbl_UnitCost), "UnitCost" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "SizeGroup", MIDText.GetTextOnly(eMIDTextCode.lbl_SizeGroup), "SizeGroupName" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "Multiple", MIDText.GetTextOnly(eMIDTextCode.lbl_Multiple), "AllocationMultiple" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "PO", MIDText.GetTextOnly(eMIDTextCode.lbl_PurchaseOrder), "PurchaseOrder" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "Vendor", MIDText.GetTextOnly(eMIDTextCode.lbl_Vendor), "Vendor" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "Workflow", MIDText.GetTextOnly(eMIDTextCode.lbl_Workflow), "WorkflowName" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "APIWorkflow", MIDText.GetTextOnly(eMIDTextCode.lbl_APIWorkflow), "APIWorkflowName" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "DC", MIDText.GetTextOnly(eMIDTextCode.lbl_DistCenter), "DistributionCenter" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "Intransit", MIDText.GetTextOnly(eMIDTextCode.lbl_Intransit), "IntransitStatusText" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "ShipStatus", MIDText.GetTextOnly(eMIDTextCode.lbl_ShipStatus), "ShipStatusText" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "Release", MIDText.GetTextOnly(eMIDTextCode.lbl_Release), "ReleaseDate" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "ChildTotal", MIDText.GetTextOnly(eMIDTextCode.lbl_ChildTotal), "" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "Master", MIDText.GetTextOnly(eMIDTextCode.lbl_MasterSubord), "MasterID" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "AllocatedUnits", MIDText.GetTextOnly(eMIDTextCode.lbl_AllocatedUnits), "AllocatedUnits" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "OrigAllocatedUnits", MIDText.GetTextOnly(eMIDTextCode.lbl_OrigAllocatedUnits), "OrigAllocatedUnits" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "RsvAllocatedUnits", MIDText.GetTextOnly(eMIDTextCode.lbl_RsvAllocatedUnits), "RsvAllocatedUnits" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "NumberOfStores", MIDText.GetTextOnly(eMIDTextCode.lbl_NumberOfStores), "NumberOfStores" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "NumPacks", MIDText.GetTextOnly(eMIDTextCode.lbl_NumPacks), "PackCount" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "NumBulkColors", MIDText.GetTextOnly(eMIDTextCode.lbl_NumBulkColors), "BulkColorCount" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "NumBulkSizes", MIDText.GetTextOnly(eMIDTextCode.lbl_NumBulkSizes), "BulkColorSizeCount" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "ImoId", MIDText.GetTextOnly(eMIDTextCode.lbl_IMO_ID), "ImoID" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "ItemUnitsAllocated", MIDText.GetTextOnly(eMIDTextCode.lbl_ItemUnitsAllocated), "StoreTot" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "ItemOrigUnitsAllocated", MIDText.GetTextOnly(eMIDTextCode.lbl_ItemOrigUnitsAllocated), "VSWtot" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "AdjustVSW", MIDText.GetTextOnly(eMIDTextCode.lbl_AdjustVSW_OnHand), "AdjustVSW" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "PlanHnRID", MIDText.GetTextOnly(eMIDTextCode.lbl_OTSPlan), "PlanHnText" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "OnHandHnRID", MIDText.GetTextOnly(eMIDTextCode.lbl_OnHand), "OnHandHnText" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "GradeInvHnRID", MIDText.GetTextOnly(eMIDTextCode.lbl_InventoryBasis), "GradeInventoryBasisHnText" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "UnitsPerCarton", MIDText.GetTextOnly(eMIDTextCode.lbl_UnitsPerCarton), "UnitsPerCarton" });
            _dtHeaderViewFieldMapping.Rows.Add(new object[] { "Notes", MIDText.GetTextOnly(eMIDTextCode.lbl_HeaderNotes), "AllocationNotes" });

            // Get all header characteristic groups
            HeaderCharacteristicsData headerCharacteristicsData = new HeaderCharacteristicsData();
            DataTable dt = headerCharacteristicsData.HeaderCharGroup_Read();

            foreach (DataRow dr in dt.Rows)
            {
                string ID = Convert.ToString(dr["HCG_ID"], CultureInfo.CurrentUICulture);
                string HCG_RID = Convert.ToString(dr["HCG_RID"], CultureInfo.CurrentUICulture);
                _dtHeaderViewFieldMapping.Rows.Add(new object[] { ID, ID, ID, HCG_RID });
            }
        }
        #endregion

        #region "Get User Last Values"
        internal ROAllocationWorklistValues GetUserLastValues()
        {
            ROAllocationWorklistValues userLastValuesOut = new ROAllocationWorklistValues();
            _userGridView = new UserGridView();
            _gridViewData = new GridViewData();
            _filterData = new FilterData();

            int viewRID = _userGridView.UserGridView_Read(SAB.ClientServerSession.UserRID, eLayoutID.allocationWorkspaceGrid);
            userLastValuesOut.UserRID = SAB.ClientServerSession.UserRID;
            userLastValuesOut.ViewRID = viewRID;
            bool useViewWorkspaceFilter = false;
            bool useFilterSorting = false;
            if (viewRID != Include.NoRID && !(_fromFilterWindow || _filterChangedAfterView))
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
                userLastValuesOut.FilterRID = _filterData.WorkspaceCurrentFilter_Read(SAB.ClientServerSession.UserRID, eWorkspaceType.AllocationWorkspace);
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
        internal void SaveAllocationUserLastValues(ROAllocationWorklistLastDataParms rOAllocationWorklistLastDataParms)
        {
            _userGridView = new UserGridView();
            _filterData = new FilterData();
            _userGridView.UserGridView_Update(SAB.ClientServerSession.UserRID, eLayoutID.allocationWorkspaceGrid, rOAllocationWorklistLastDataParms.iViewRID);
            _filterData.WorkspaceCurrentFilter_Update(SAB.ClientServerSession.UserRID, eWorkspaceType.AllocationWorkspace, rOAllocationWorklistLastDataParms.iFilterRID);
        }
        #endregion

        #region "Get Selected Header Details"
        private ROOut GetSelectedHeaderDetails()
        {

            try
            {
                ClearDetails();

                var selectedHeaders = (SelectedHeaderList)_applicationSessionTransaction.GetSelectedHeaders();

                var headerDetails = BuildHeaderDetails(selectedHeaders);

                ROIListOut rOIListOut = new ROIListOut(eROReturnCode.Successful, null, ROInstanceID, headerDetails);

                return rOIListOut;

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        #region "internal methods for Get Allocation Header Details"
        internal List<ROAllocationHeaderSummary> BuildHeaderDetails(SelectedHeaderList selectedHeaders)
        {
            ArrayList headerProfileList = new ArrayList();
            if (selectedHeaders.Count > 0)
            {
                foreach (SelectedHeaderProfile item in selectedHeaders)
                {
                    headerProfileList.Add(SAB.HeaderServerSession.GetHeaderData(item.Key, true, true, true));
                }
            }

            return BuildHeaderSummaryList(headerProfileList: headerProfileList, includeDetails: true);
        }

        internal ROAllocationHeaderSummary HeaderDetails(AllocationHeaderProfile hdrProf, ROAllocationHeaderSummary headerSummary)
        {
            Hashtable sizeGroupHash = new Hashtable();
            Hashtable workflowNameHash = new Hashtable();
            Header headerData = new Header();

            if (_headerCharGroupProfileList == null)
            {
                _headerCharGroupProfileList = SAB.HeaderServerSession.GetHeaderCharGroups();
            }

            _assortmentSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.Assortment);

            HierarchyNodeProfile hnp_style = SAB.HierarchyServerSession.GetNodeData(hdrProf.StyleHnRID, false);

            #region "Code for BulkColorDetails #RO-3031"

            List<HeaderBulkColorProfile> bulkColorValues = hdrProf.BulkColors.Values.OfType<HeaderBulkColorProfile>().ToList();

            List<ROHeaderBulkColorDetails> bulkColorDetails = new List<ROHeaderBulkColorDetails>();

            foreach (var blkValues in bulkColorValues)
            {
                //bulkColorDetails.Clear();
                int balance = blkValues.Units;

                var bulkColorSizes = blkValues.Sizes;
                if (bulkColorSizes.Count == 0)
                {
                    balance = 0;
                }

                List<BulkColorSize> sizeProfile = new List<BulkColorSize>();
                if (bulkColorSizes.Count > 0)
                {
                    sizeProfile.Clear();

                    foreach (DictionaryEntry bulkSizeEntry in bulkColorSizes)
                    {
                        var blkSize = ((HeaderBulkColorSizeProfile)bulkSizeEntry.Value);

                        SizeCodeProfile scp = SAB.HierarchyServerSession.GetSizeCodeProfile(blkSize.Key);

                        BulkColorSize bulkClrSize = new BulkColorSize
                        {
                            HDR_BCSZ_Key = blkSize.HDR_BCSZ_Key,
                            HeaderBulkColorSize = blkSize.ProfileType,
                            InstanceID = blkSize.InstanceID,
                            //Size = new KeyValuePair<int, string>(blkSize.Key, SAB.HierarchyServerSession.GetSizeCodeProfile(blkSize.Key).SizeCodeName),
                            Size = new KeyValuePair<int, string>(blkSize.Key, scp.SizeCodeName),
                            SizePrimary = new KeyValuePair<int, string>(scp.SizeCodePrimaryRID, scp.SizeCodePrimary),
                            SizeSecondary = new KeyValuePair<int, string>(scp.SizeCodeSecondaryRID, scp.SizeCodeSecondary),
                            Maximum = blkSize.Maximum,
                            Minimum = blkSize.Minimum,
                            Multiple = blkSize.Multiple,
                            ReserveUnits = blkSize.ReserveUnits,
                            Sequence = blkSize.Sequence,
                            Units = blkSize.Units
                        };

                        balance -= blkSize.Units;

                        sizeProfile.Add(bulkClrSize);
                    }
                }

                ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(blkValues.Key);
                string colorID = string.Empty;
                string description = string.Empty;

                if (ccp.VirtualInd)
                {
                    colorID = blkValues.Name;
                    description = blkValues.Description;
                }
                else
                {
                    colorID = ccp.ColorCodeID;
                    description = GetColorDescription(hnp_style, ccp);
                }

                ROHeaderBulkColorDetails rOHeaderBulkColorDetails = new ROHeaderBulkColorDetails
                {
                    AsrtBCRID = blkValues.AsrtBCRID,
                    Description = description,
                    InstanceID = blkValues.InstanceID,
                    Color = new KeyValuePair<int, string>(blkValues.Key, colorID),
                    Last_BCSZ_Key_Used = blkValues.Last_BCSZ_Key_Used,
                    Maximum = blkValues.Maximum,
                    Minimum = blkValues.Minimum,
                    HeaderBulkColor = blkValues.ProfileType,
                    ReserveUnits = blkValues.ReserveUnits,
                    Sequence = blkValues.Sequence,
                    BulkColorSizeProfile = sizeProfile,
                    Multiple = blkValues.Multiple,
                    Name = ccp.ColorCodeName,
                    Units = blkValues.Units,
                    Balance = balance
                };

                bulkColorDetails.Add(rOHeaderBulkColorDetails);
            }

            headerSummary.BulkColorDetails = bulkColorDetails;

            #endregion

            #region"Code for PackDetails, PackColor, PackColorSize #RO-3030"

            List<HeaderPackProfile> packValues = hdrProf.Packs.Values.OfType<HeaderPackProfile>().ToList();   //Pack Information

            List<ROHeaderPackProfile> packDetails = new List<ROHeaderPackProfile>();


            foreach (var packValue in packValues)
            {
                var packColors = packValue.Colors;
                int packBalance = packValue.Packs * packValue.Multiple;
                if (packColors.Count == 0)
                {
                    packBalance = 0;
                }

                List<HeaderPackColor> lstHeaderPackColors = new List<HeaderPackColor>();

                foreach (HeaderPackColorProfile colorInfo in packColors.Values)
                {
                    
                    lstHeaderPackColors.Clear();

                    int balance = colorInfo.Units;
                    
                    packBalance -= (colorInfo.Units * packValue.Packs);

                    var packColorSizes = colorInfo.Sizes;
                    if (packColorSizes.Count == 0)
                    {
                        balance = 0;
                    }

                    List<HeaderPackColorSize> lstheaderPackColorSizes = new List<HeaderPackColorSize>();

                    if (packColorSizes.Count > 0)
                    {

                        lstheaderPackColorSizes.Clear();

                        foreach (DictionaryEntry colorSizeEntry in packColorSizes)
                        {
                            var sizeInfo = ((HeaderPackColorSizeProfile)colorSizeEntry.Value);

                            SizeCodeProfile scp = SAB.HierarchyServerSession.GetSizeCodeProfile(sizeInfo.Key);

                            HeaderPackColorSize headerPackColorSize = new HeaderPackColorSize
                            {
                                Hdr_PCSZ_Key = sizeInfo.HDR_PCSZ_Key,
                                Sequence = sizeInfo.Sequence,
                                Units = sizeInfo.Units,
                                InstanceId = sizeInfo.InstanceID,
                                isFound = sizeInfo.isFound,
                                ProfileType = sizeInfo.ProfileType,
                                //Size = new KeyValuePair<int, string>(sizeInfo.Key, SAB.HierarchyServerSession.GetSizeCodeProfile(sizeInfo.Key).SizeCodeName)
                                Size = new KeyValuePair<int, string>(sizeInfo.Key,scp.SizeCodeName),
                                SizePrimary = new KeyValuePair<int, string>(scp.SizeCodePrimaryRID, scp.SizeCodePrimary),
                                SizeSecondary = new KeyValuePair<int, string>(scp.SizeCodeSecondaryRID, scp.SizeCodeSecondary)
                            };

                            balance -= sizeInfo.Units;

                            lstheaderPackColorSizes.Add(headerPackColorSize);
                        }

                    }

                    ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(colorInfo.Key);
                    string colorID = string.Empty;
                    string description = string.Empty;

                    if (ccp.VirtualInd)
                    {
                        colorID = colorInfo.ColorName;
                        description = colorInfo.ColorDescription;
                    }
                    else
                    {
                        colorID = ccp.ColorCodeID;
                        description = GetColorDescription(hnp_style, ccp);
                    }

                    HeaderPackColor headerPackColor = new HeaderPackColor
                    {
                        Color = new KeyValuePair<int, string>(colorInfo.Key, colorID),
                        ColorCodeRID = colorInfo.ColorCodeRID,
                        ColorDescription = description,
                        ColorName = ccp.ColorCodeName,
                        HdrPCRID = colorInfo.HdrPCRID,
                        InstanceID = colorInfo.InstanceID,
                        Last_PCSZ_Key_Used = colorInfo.Last_PCSZ_Key_Used,
                        Sequence = colorInfo.Sequence,
                        ProfileType = colorInfo.ProfileType,
                        Units = colorInfo.Units,
                        Balance = balance,
                        Sizes = lstheaderPackColorSizes
                    };

                    lstHeaderPackColors.Add(headerPackColor);

                }

                ROHeaderPackProfile rOHeaderPackProfile = new ROHeaderPackProfile
                {
                    AssociatedPackRIDs = packValue.AssociatedPackRIDs,
                    GenericInd = packValue.GenericInd,
                    HeaderRID = packValue.HeaderRID,
                    InstanceId = packValue.InstanceID,
                    Pack = new KeyValuePair<int, string>(packValue.Key, packValue.HeaderPackName),
                    Multiple = packValue.Multiple,
                    Packs = packValue.Packs,
                    ProfileType = packValue.ProfileType,
                    ReservePacks = packValue.ReservePacks,
                    Sequence = packValue.Sequence,
                    Balance = packBalance,
                    ColorsInfo = lstHeaderPackColors
                };

                packDetails.Add(rOHeaderPackProfile);
            }


            headerSummary.PackDetails = packDetails;

            #endregion


            return headerSummary;
        }

        internal void ClearDetails()
        {
            try
            {
                _inClearDetails = true;
                ArrayList alDataTables = null;

                if (_dsDetails != null)
                {
                    alDataTables = new ArrayList();
                    _dsDetails.Clear();
                    _dsDetails.Relations.Clear();
                    RemoveTablesFromDataset(_dsDetails, GetTablesFromDataset(_dsDetails, "PackSize"));
                    RemoveTablesFromDataset(_dsDetails, GetTablesFromDataset(_dsDetails, "PackColor"));
                    RemoveTablesFromDataset(_dsDetails, GetTablesFromDataset(_dsDetails, "Pack"));
                    RemoveTablesFromDataset(_dsDetails, GetTablesFromDataset(_dsDetails, "BulkSize"));
                    RemoveTablesFromDataset(_dsDetails, GetTablesFromDataset(_dsDetails, "BulkColor"));
                    RemoveTablesFromDataset(_dsDetails, GetTablesFromDataset(_dsDetails, "Header"));

                    _dtBulkColor.Dispose();
                    _dtBulkColor = null;
                    _dtPackColor.Dispose();
                    _dtPackColor = null;
                    _dtPack.Dispose();
                    _dtPack = null;
                    _dtDetailHeader.Dispose();
                    _dtDetailHeader = null;

                    _dsDetails.Dispose();
                    _dsDetails = null;

                    ClearSavedDetails();

                }

            }
            catch
            {
                throw;
            }
            finally
            {
                _inClearDetails = false;
            }
        }
        internal ArrayList GetTablesFromDataset(DataSet aDataSet, string aTableNameMask)
        {
            ArrayList al = new ArrayList();
            foreach (DataTable dt in aDataSet.Tables)
            {
                if (dt.TableName.Contains(aTableNameMask))
                {
                    dt.Clear();
                    al.Add(dt);
                }
            }
            return al;
        }

        internal void RemoveTablesFromDataset(DataSet aDataSet, ArrayList alDataTables)
        {
            foreach (DataTable dt in alDataTables)
            {
                dt.PrimaryKey = null;
                dt.Clear();
                dt.Rows.Clear();
                RemoveConstraintsFromDataTable(dt, GetConstraintFromDataTable(dt));
                if (aDataSet.Tables.CanRemove(dt))
                {
                    aDataSet.Tables.Remove(dt);
                }
            }
        }
        internal ArrayList GetConstraintFromDataTable(DataTable aDataTable)
        {
            ArrayList al = new ArrayList();
            foreach (Constraint constraint in aDataTable.Constraints)
            {
                al.Add(constraint);
            }
            return al;
        }

        internal void RemoveConstraintsFromDataTable(DataTable aDataTable, ArrayList alConstraints)
        {
            foreach (Constraint constraint in alConstraints)
            {
                if (aDataTable.Constraints.CanRemove(constraint))
                {
                    aDataTable.Constraints.Remove(constraint);
                }
            }
        }

        internal void ClearSavedDetails()
        {
            try
            {
                ArrayList alDataTables = null;

                if (_dsDetailsSaved != null)
                {
                    alDataTables = new ArrayList();
                    _dsDetailsSaved.Clear();
                    _dsDetailsSaved.Relations.Clear();
                    RemoveTablesFromDataset(_dsDetailsSaved, GetTablesFromDataset(_dsDetailsSaved, "PackSize"));
                    RemoveTablesFromDataset(_dsDetailsSaved, GetTablesFromDataset(_dsDetailsSaved, "PackColor"));
                    RemoveTablesFromDataset(_dsDetailsSaved, GetTablesFromDataset(_dsDetailsSaved, "Pack"));
                    RemoveTablesFromDataset(_dsDetailsSaved, GetTablesFromDataset(_dsDetailsSaved, "BulkSize"));
                    RemoveTablesFromDataset(_dsDetailsSaved, GetTablesFromDataset(_dsDetailsSaved, "BulkColor"));
                    RemoveTablesFromDataset(_dsDetailsSaved, GetTablesFromDataset(_dsDetailsSaved, "Header"));

                    _dsDetailsSaved.Dispose();
                    _dsDetailsSaved = null;
                }
            }
            catch
            {
                throw;
            }
        }

        internal HierarchyNodeProfile GetNodeData(int aHnRID)
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

        internal string GetColorDescription(HierarchyNodeProfile hnp_style, ColorCodeProfile ccp)
        {
            int colorHnRID = Include.NoRID;
            string colorDescription = null;
            Hashtable colorHash = null;
            try
            {
                if (_colorsForStyle.ContainsKey(hnp_style.Key))
                {
                    colorHash = (Hashtable)_colorsForStyle[hnp_style.Key];
                    if (colorHash.ContainsKey(ccp.Key))
                    {
                        colorDescription = colorHash[ccp.Key].ToString();
                    }
                    else
                    {
                        if (SAB.HierarchyServerSession.ColorExistsForStyle(hnp_style.HomeHierarchyRID, hnp_style.Key, ccp.ColorCodeID, ref colorHnRID))
                        {
                            HierarchyNodeProfile hnp_color = SAB.HierarchyServerSession.GetNodeData(colorHnRID, false);
                            colorDescription = hnp_color.NodeDescription;
                        }
                        else
                        {
                            colorDescription = ccp.ColorCodeName;
                        }
                        colorHash.Add(ccp.Key, colorDescription);
                    }
                }
                else
                {
                    colorHash = new Hashtable();
                    _colorsForStyle.Add(hnp_style.Key, colorHash);
                    colorDescription = GetColorDescription(hnp_style, ccp);
                }
            }
            catch (Exception ex)
            {

            }
            return colorDescription;
        }

        internal DataTable FormatPackSizeTable(PackColorSize aColor, int sizeGroupRID, int headerRID, int packRID, int colorRID, string aColorCodeID, string tableName, ref int sizeTotal)
        {
            ArrayList sizeID = new ArrayList();
            SortedList primarySL = new SortedList();
            ArrayList secondaryAL = new ArrayList();
            Hashtable bothHash = new Hashtable();
            SizeGroupProfile sgp = null;
            sizeTotal = 0;

            AllocationProfile ap = (AllocationProfile)_allocProfileList.FindKey(headerRID);
            SizeCodeList scl = null;

            if (sizeGroupRID > Include.UndefinedSizeGroupRID)
            {
                sgp = GetSizeGroup(sizeGroupRID);
                scl = sgp.SizeCodeList;

                LoadSizeArraysFromGroup(scl, ref sizeID, ref primarySL, ref secondaryAL);

                if (aColor != null)
                {
                    foreach (PackContentBin aSize in aColor.ColorSizes.Values)
                    {
                        if (scl.Contains(aSize.ContentCodeRID))
                        {
                            SizeCodeProfile scp = (SizeCodeProfile)scl.FindKey(aSize.ContentCodeRID);
                            if (!bothHash.ContainsKey(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID))
                            {
                                bothHash.Add(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID, aSize);
                            }
                            else
                            {
                                throw new MIDException(eErrorLevel.severe, 0, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_LabelsNotUnique));
                            }
                        }
                    }

                    foreach (PackContentBin aSize in aColor.ColorSizes.Values)
                    {

                        SizeCodeProfile scp = (SizeCodeProfile)scl.FindKey(aSize.ContentCodeRID);
                        if (scp == null)
                        {
                            if (!ap.MultiHeader && !ap.Placeholder)
                            {
                                scp = SAB.HierarchyServerSession.GetSizeCodeProfile(aSize.ContentCodeRID);
                                if (scp.Key == Include.NoRID)
                                {
                                    throw new MIDException(eErrorLevel.severe, 0,
                                          string.Format(MIDText.GetText(eMIDTextCode.msg_SizeCodeRetrieveError), aSize.ContentCodeRID.ToString(CultureInfo.CurrentUICulture)));
                                }

                                return null;
                            }
                            else
                            {
                                AddSizesNotInGroup(aSize.ContentCodeRID, ref scp, ref sizeID, ref primarySL, ref secondaryAL, bothHash);
                            }


                            if (!bothHash.ContainsKey(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID))
                            {
                                bothHash.Add(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID, aSize);
                            }
                            else
                            {
                                string newPrimaryBoth = scp.SizeCodePrimary + _dupSizeNameSeparator + scp.SizeCodeID;
                                if (!bothHash.ContainsKey(newPrimaryBoth + "~" + scp.SizeCodeSecondaryRID))
                                {
                                    bothHash.Add(newPrimaryBoth + "~" + scp.SizeCodeSecondaryRID, aSize);
                                }
                                else
                                {
                                    throw new MIDException(eErrorLevel.severe, 0, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_LabelsNotUnique));
                                }
                            }
                        }
                    }
                }
            }
            else
            {   // no size group
                SortedList sl = new SortedList();
                foreach (PackContentBin aSize in aColor.ColorSizes.Values)
                {
                    sl.Add(aSize.Sequence, aSize);
                }

                foreach (PackContentBin aSize in sl.Values)
                {
                    SizeCodeProfile scp = SAB.HierarchyServerSession.GetSizeCodeProfile(aSize.ContentCodeRID);
                    if (scp.Key == Include.NoRID)
                    {
                        throw new MIDException(eErrorLevel.severe, 0,
                            string.Format(MIDText.GetText(eMIDTextCode.msg_SizeCodeRetrieveError), aSize.ContentCodeRID.ToString(CultureInfo.CurrentUICulture)));
                    }
                    LoadSizeArraysFromHeader(aSize.ContentCodeRID, ref sizeID, ref primarySL, ref secondaryAL);

                    if (!bothHash.ContainsKey(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID))
                    {
                        bothHash.Add(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID, aSize);
                    }
                    else
                    {
                        string newPrimaryBoth = scp.SizeCodePrimary + _dupSizeNameSeparator + scp.SizeCodeID;
                        if (!bothHash.ContainsKey(newPrimaryBoth + "~" + scp.SizeCodeSecondaryRID))
                        {
                            bothHash.Add(newPrimaryBoth + "~" + scp.SizeCodeSecondaryRID, aSize);
                        }
                        else
                        {
                            throw new MIDException(eErrorLevel.severe, 0, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_LabelsNotUnique));
                        }

                    }
                }
            }

            if (_addingSizes)
            {
                UpdateColorSizeHash(ap.StyleHnRID, aColorCodeID, sizeID);
            }
            DataTable dtSizes = InitializeSizeTable(tableName, primarySL);
            BuildSizeTableFromArrays(ref dtSizes, true, scl, headerRID, packRID, colorRID, secondaryAL, bothHash, ref sizeTotal);
            return (dtSizes);
        }

        internal SizeGroupProfile GetSizeGroup(int aSizeGroupRID)
        {
            if (_sizeGroupHashLastKey != aSizeGroupRID)
            {
                _sizeGroupHashLastKey = aSizeGroupRID;
                if (_sizeGroupHash == null)
                {
                    _sizeGroupHash = new Hashtable();
                }
                if (_sizeGroupHash.Contains(aSizeGroupRID))
                {
                    _sizeGroupHashLastValue = (SizeGroupProfile)_sizeGroupHash[aSizeGroupRID];
                }
                else
                {
                    _sizeGroupHashLastValue = new SizeGroupProfile(aSizeGroupRID);
                    _sizeGroupHash.Add(aSizeGroupRID, _sizeGroupHashLastValue);
                }
            }
            return _sizeGroupHashLastValue;
        }


        internal void LoadSizeArraysFromGroup(SizeCodeList aScl, ref ArrayList aSizeID, ref SortedList aPrimarySL, ref ArrayList aSecondaryAL)
        {
            try
            {
                foreach (SizeCodeProfile scp in aScl.ArrayList)
                {
                    if (scp.Key == Include.NoRID)
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_CantRetrieveSizeCode,
                            MIDText.GetText(eMIDTextCode.msg_CantRetrieveSizeCode));
                    }

                    if (!aSizeID.Contains(scp.SizeCodeID))
                    {
                        aSizeID.Add(scp.SizeCodeID);
                    }
                    if (!aPrimarySL.ContainsValue(scp.SizeCodePrimary + "~" + scp.SizeCodePrimaryRID))
                    {
                        aPrimarySL.Add(scp.PrimarySequence, scp.SizeCodePrimary + "~" + scp.SizeCodePrimaryRID);
                    }

                    if (!aSecondaryAL.Contains(scp.SizeCodeSecondary + "~" + scp.SizeCodeSecondaryRID))
                    {
                        aSecondaryAL.Add(scp.SizeCodeSecondary + "~" + scp.SizeCodeSecondaryRID);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        internal DataTable FormatBulkSizeTable(HdrColorBin aColor, int aSizeGroupRID, int headerRID, int colorRID, string aColorCodeID, string tableName, ref int sizeTotal)
        {
            ArrayList sizeID = new ArrayList();
            SortedList primarySL = new SortedList();
            ArrayList secondaryAL = new ArrayList();
            Hashtable bothHash = new Hashtable();
            SortedList sortedList = new SortedList();
            SizeGroupProfile sgp = null;
            sizeTotal = 0;

            AllocationProfile ap = (AllocationProfile)_allocProfileList.FindKey(headerRID);
            SizeCodeList scl = null;
            // is there a size group?
            if (aSizeGroupRID > Include.UndefinedSizeGroupRID)
            {
                // load existing group
                sgp = GetSizeGroup(aSizeGroupRID);
                scl = sgp.SizeCodeList;

                LoadSizeArraysFromGroup(scl, ref sizeID, ref primarySL, ref secondaryAL);

                if (aColor != null)
                {
                    foreach (HdrSizeBin aSize in aColor.ColorSizes.Values)
                    {
                        sortedList.Add(aSize.SizeSequence, aSize);
                    }

                    foreach (HdrSizeBin aSize in sortedList.Values)
                    {
                        if (scl.Contains(aSize.SizeKey))
                        {
                            SizeCodeProfile scp = (SizeCodeProfile)scl.FindKey(aSize.SizeKey);
                            if (!bothHash.ContainsKey(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID))
                            {
                                bothHash.Add(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID, aSize);
                            }
                            else
                            {
                                throw new MIDException(eErrorLevel.severe, 0, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_LabelsNotUnique));
                            }
                        }
                    }
                    foreach (HdrSizeBin aSize in sortedList.Values)
                    {
                        SizeCodeProfile scp = (SizeCodeProfile)scl.FindKey(aSize.SizeKey);
                        if (scp == null)
                        {
                            if (!ap.MultiHeader && !ap.Placeholder)
                            {
                                scp = SAB.HierarchyServerSession.GetSizeCodeProfile(aSize.SizeKey);
                                if (scp.Key == Include.NoRID)
                                {
                                    throw new MIDException(eErrorLevel.severe, 0, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CantRetrieveSizeCode) + " " + aSize.SizeKey.ToString(CultureInfo.CurrentUICulture));
                                }

                                return null;
                            }
                            else
                            {
                                AddSizesNotInGroup(aSize.SizeKey, ref scp, ref sizeID, ref primarySL, ref secondaryAL, bothHash);
                            }
                        }
                        else
                        {
                            continue;
                        }

                        if (!bothHash.ContainsKey(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID))
                        {
                            bothHash.Add(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID, aSize);
                        }
                        else
                        {
                            string newPrimaryBoth = scp.SizeCodePrimary + _dupSizeNameSeparator + scp.SizeCodeID;
                            if (!bothHash.ContainsKey(newPrimaryBoth + "~" + scp.SizeCodeSecondaryRID))
                            {
                                bothHash.Add(newPrimaryBoth + "~" + scp.SizeCodeSecondaryRID, aSize);
                            }
                            else
                            {
                                throw new MIDException(eErrorLevel.severe, 0, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_LabelsNotUnique));
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (HdrSizeBin aSize in aColor.ColorSizes.Values)
                {
                    sortedList.Add(aSize.SizeSequence, aSize);
                }
                scl = new SizeCodeList(eProfileType.SizeCode);
                foreach (HdrSizeBin aSize in sortedList.Values)
                {
                    SizeCodeProfile scp = SAB.HierarchyServerSession.GetSizeCodeProfile(aSize.SizeKey);
                    if (scp.Key == Include.NoRID)
                    {
                        throw new MIDException(eErrorLevel.severe, 0,
                            string.Format(MIDText.GetText(eMIDTextCode.msg_SizeCodeRetrieveError), aSize.SizeKey.ToString(CultureInfo.CurrentUICulture)));
                    }
                    scl.Add(scp);
                    LoadSizeArraysFromHeader(aSize.SizeKey, ref sizeID, ref primarySL, ref secondaryAL);

                    if (!bothHash.ContainsKey(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID))
                    {
                        bothHash.Add(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID, aSize);
                    }
                    else
                    {
                        AddSizesNotInGroup(aSize.SizeKey, ref scp, ref sizeID, ref primarySL, ref secondaryAL, bothHash);
                        string newPrimaryBoth = scp.SizeCodePrimary + _dupSizeNameSeparator + scp.SizeCodeID;
                        if (!bothHash.ContainsKey(newPrimaryBoth + "~" + scp.SizeCodeSecondaryRID))
                        {
                            bothHash.Add(newPrimaryBoth + "~" + scp.SizeCodeSecondaryRID, aSize);
                        }
                        else
                        {
                            throw new MIDException(eErrorLevel.severe, 0, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_LabelsNotUnique));
                        }
                    }
                }
            }

            if (_addingSizes)
            {
                UpdateColorSizeHash(ap.StyleHnRID, aColorCodeID, sizeID);
            }

            DataTable dtSizes = InitializeSizeTable(tableName, primarySL);
            BuildSizeTableFromArrays(ref dtSizes, false, scl, headerRID, Include.NoRID, colorRID, secondaryAL, bothHash, ref sizeTotal);
            return (dtSizes);
        }

        internal void AddSizesNotInGroup(int aSizeKey, ref SizeCodeProfile aScp, ref ArrayList aSizeID, ref SortedList aPrimarySL, ref ArrayList aSecondaryAL, Hashtable aBothHash)
        {
            try
            {
                aScp = SAB.HierarchyServerSession.GetSizeCodeProfile(aSizeKey);
                if (aScp.Key == Include.NoRID)
                {
                    throw new MIDException(eErrorLevel.severe, 0,
                            string.Format(MIDText.GetText(eMIDTextCode.msg_SizeCodeRetrieveError), aSizeKey.ToString(CultureInfo.CurrentUICulture)));
                }
                if (!aSizeID.Contains(aScp.SizeCodeID))
                {
                    aSizeID.Add(aScp.SizeCodeID);
                }

                int seq = aPrimarySL.Count;
                if (!aPrimarySL.ContainsValue(aScp.SizeCodePrimary + "~" + aScp.SizeCodePrimaryRID))
                {
                    seq++;
                    aPrimarySL.Add(seq, aScp.SizeCodePrimary + "~" + aScp.SizeCodePrimaryRID);
                }
                else if (aBothHash.Contains(aScp.SizeCodePrimary + "~" + aScp.SizeCodeSecondaryRID))
                {
                    if (!aPrimarySL.ContainsValue(aScp.SizeCodePrimary + "~" + aScp.SizeCodePrimaryRID + "~" + aScp.SizeCodeID))
                    {
                        seq++;
                        aPrimarySL.Add(seq, aScp.SizeCodePrimary + "~" + aScp.SizeCodePrimaryRID + "~" + aScp.SizeCodeID);
                    }
                }
                if (!aSecondaryAL.Contains(aScp.SizeCodeSecondary + "~" + aScp.SizeCodeSecondaryRID))
                {
                    aSecondaryAL.Add(aScp.SizeCodeSecondary + "~" + aScp.SizeCodeSecondaryRID);
                }
            }
            catch (Exception ex)
            {

            }
        }

        internal void LoadSizeArraysFromHeader(int aSizeKey, ref ArrayList aSizeID, ref SortedList aPrimarySL, ref ArrayList aSecondaryAL)
        {
            try
            {
                SizeCodeProfile scp = SAB.HierarchyServerSession.GetSizeCodeProfile(aSizeKey);
                if (scp.Key == Include.NoRID)
                {
                    throw new MIDException(eErrorLevel.severe, 0,
                            string.Format(MIDText.GetText(eMIDTextCode.msg_SizeCodeRetrieveError), aSizeKey.ToString(CultureInfo.CurrentUICulture)));
                }
                if (!aSizeID.Contains(scp.SizeCodeID))
                {
                    aSizeID.Add(scp.SizeCodeID);
                }
                int seq = aPrimarySL.Count;
                if (!aPrimarySL.ContainsValue(scp.SizeCodePrimary + "~" + scp.SizeCodePrimaryRID))
                {
                    seq++;
                    aPrimarySL.Add(seq, scp.SizeCodePrimary + "~" + scp.SizeCodePrimaryRID);
                }

                if (!aSecondaryAL.Contains(scp.SizeCodeSecondary + "~" + scp.SizeCodeSecondaryRID))
                {
                    aSecondaryAL.Add(scp.SizeCodeSecondary + "~" + scp.SizeCodeSecondaryRID);
                }
            }
            catch (Exception ex)
            {

            }
        }

        internal void UpdateColorSizeHash(int aStyleHnRID, string aColorCodeID, ArrayList aSizeID)
        {
            try
            {
                ArrayList colorSizeAL;
                Hashtable styleColorHash;
                string addSizeID;
                for (int i = 0; i < aSizeID.Count; i++)
                {
                    addSizeID = Convert.ToString(aSizeID[i], CultureInfo.CurrentUICulture);
                    if (!_addedColorSizeHash.ContainsKey(aStyleHnRID))
                    {
                        styleColorHash = new Hashtable();
                        colorSizeAL = new ArrayList();
                        colorSizeAL.Add(addSizeID);
                        styleColorHash.Add(aColorCodeID, colorSizeAL);
                        _addedColorSizeHash.Add(aStyleHnRID, styleColorHash);
                    }
                    else
                    {
                        styleColorHash = (Hashtable)_addedColorSizeHash[aStyleHnRID];
                        if (!styleColorHash.ContainsKey(aColorCodeID))
                        {
                            colorSizeAL = new ArrayList();
                            colorSizeAL.Add(addSizeID);
                            styleColorHash.Add(aColorCodeID, colorSizeAL);
                        }
                        else
                        {
                            colorSizeAL = (ArrayList)styleColorHash[aColorCodeID];
                            if (!colorSizeAL.Contains(addSizeID))
                            {
                                colorSizeAL.Add(addSizeID);
                            }
                        }
                    }
                }
                _addingSizes = false;
            }
            catch
            {
                throw;
            }
        }

        internal DataTable InitializeSizeTable(string aTableName, SortedList aPrimarySL)
        {
            try
            {
                DataTable dtSizes = MIDEnvironment.CreateDataTable(aTableName);
                dtSizes.Columns.Add("KeyH", System.Type.GetType("System.Int32"));
                dtSizes.Columns.Add("KeyP", System.Type.GetType("System.Int32"));
                dtSizes.Columns.Add("KeyC", System.Type.GetType("System.Int32"));
                dtSizes.Columns.Add("SecondaryRID", System.Type.GetType("System.Int32"));
                dtSizes.Columns.Add("Secondary");
                dtSizes.Columns.Add(" - ");
                dtSizes.Columns.Add("TotalQuantity", System.Type.GetType("System.Int32"));

                foreach (int seq in aPrimarySL.Keys)
                {
                    string[] sizeParts = aPrimarySL[seq].ToString().Split(new char[] { '~' });
                    string colName = sizeParts[0];
                    int primaryRID = Convert.ToInt32(sizeParts[1], CultureInfo.CurrentUICulture);
                    if (dtSizes.Columns.Contains(colName))
                    {
                        colName += _dupSizeNameSeparator + Convert.ToString(sizeParts[2], CultureInfo.CurrentUICulture);
                    }
                    dtSizes.Columns.Add(colName);
                    dtSizes.Columns[colName].DataType = System.Type.GetType("System.Int32");
                    dtSizes.Columns[colName].ExtendedProperties.Add("PrimaryRID", primaryRID);
                }
                return dtSizes;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        internal void BuildSizeTableFromArrays(ref DataTable dtSizes, bool aPackSizes, SizeCodeList aSizeCodeList, int aHeaderRID, int aPackRID, int aColorRID, ArrayList aSecondaryAL, Hashtable aBothHash, ref int aSizeTotal)
        {
            try
            {
                foreach (string secondary in aSecondaryAL)
                {
                    string[] secondaryParts = secondary.ToString().Split(new char[] { '~' });
                    string secondaryName = secondaryParts[0];
                    int secondaryRID = Convert.ToInt32(secondaryParts[1], CultureInfo.CurrentUICulture);
                    int total = 0;
                    DataRow dRow = dtSizes.NewRow();
                    dRow["KeyH"] = aHeaderRID;
                    dRow["KeyP"] = aPackRID;
                    dRow["KeyC"] = aColorRID;
                    dRow["SecondaryRID"] = secondaryRID;
                    dRow["Secondary"] = secondaryName;

                    if (secondaryName == Include.NoSecondarySize || secondaryName.Trim() == string.Empty || secondaryName.Trim() == _noSizeDimensionLbl)
                    {
                        dRow[" - "] = _lblQuantity;
                    }
                    else
                    {
                        dRow[" - "] = secondaryName;
                    }
                    dRow["TotalQuantity"] = 0;
                    for (int i = 7; i < dtSizes.Columns.Count; i++)
                    {
                        DataColumn dCol = dtSizes.Columns[i];
                        string primary = dCol.ColumnName;

                        if (aBothHash.Contains(primary + "~" + secondaryRID))
                        {
                            if (aPackSizes)
                            {
                                total += ((PackContentBin)aBothHash[primary + "~" + secondaryRID]).ContentUnits;
                                dRow[dCol] = ((PackContentBin)aBothHash[primary + "~" + secondaryRID]).ContentUnits;
                                dCol.ExtendedProperties.Add(primary + "~" + secondaryRID, ((PackContentBin)aBothHash[primary + "~" + secondaryRID]).ContentCodeRID);
                            }
                            else
                            {
                                total += ((HdrSizeBin)aBothHash[primary + "~" + secondaryRID]).SizeUnitsToAllocate;
                                dRow[dCol] = ((HdrSizeBin)aBothHash[primary + "~" + secondaryRID]).SizeUnitsToAllocate;
                                dCol.ExtendedProperties.Add(primary + "~" + secondaryRID, ((HdrSizeBin)aBothHash[primary + "~" + secondaryRID]).SizeKey);
                            }
                        }
                        else
                        {
                            dRow[dCol] = 0;
                            if (aSizeCodeList == null)
                            {
                            }
                            else
                            {
                                SizeCodeProfile scp = null;
                                bool foundSize = false;
                                for (int j = 0; j < aSizeCodeList.ArrayList.Count; j++)
                                {
                                    scp = (SizeCodeProfile)aSizeCodeList.ArrayList[j];
                                    if (scp.SizeCodePrimary == primary && scp.SizeCodeSecondaryRID == secondaryRID)
                                    {
                                        dCol.ExtendedProperties.Add(primary + "~" + secondaryRID, scp.Key);
                                        foundSize = true;
                                        break;
                                    }
                                }
                                if (scp == null || !foundSize)
                                {
                                    SizeGroup sizeData = new SizeGroup();
                                    DataTable dtGetSize = sizeData.GetSizeForPrimarySecondary(primary, secondaryName);
                                    if (dtGetSize != null && dtGetSize.Rows.Count > 0)
                                    {
                                        DataRow row = dtGetSize.Rows[0];
                                        dCol.ExtendedProperties.Add(primary + "~" + secondaryRID, row["SIZE_CODE_RID"]);
                                        foundSize = true;
                                    }
                                    else
                                    {
                                        dtGetSize = sizeData.GetSizeForPrimarySecondary(primary, Include.NoSecondarySize);
                                        if (dtGetSize != null && dtGetSize.Rows.Count > 0)
                                        {
                                            DataRow row = dtGetSize.Rows[0];
                                            dCol.ExtendedProperties.Add(primary + "~" + secondaryRID, row["SIZE_CODE_RID"]);
                                            foundSize = true;
                                        }
                                    }
                                    if (!foundSize)
                                    {
                                        dCol.ExtendedProperties.Add(primary + "~" + secondaryRID, Include.NoRID);
                                    }
                                }   // End TT#558  
                            }
                        }
                    }
                    dRow["TotalQuantity"] = total;
                    aSizeTotal += total;
                    dtSizes.Rows.Add(dRow);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private enum eSecurityType
        {
            View,
            Update
        }

        internal string setGeneratedWorkflowName(string aHeaderID, string workflowName)
        {
            try
            {
                if ((workflowName != null) && (workflowName == aHeaderID + "(generated)"))
                {
                    return "\"Generated\"";
                }
                else
                {
                    return workflowName;
                }
            }
            catch
            {
                return workflowName;
            }
        }

        #endregion

        #endregion

        #region "Get Selected Filter Header List"

        private ROOut GetSelectedFilterHeaderList(ROKeyParms headerFilterRID)
        {
            if (_selectedFilterHeaderList != null)
            {
                _selectedFilterHeaderList.Clear();
                _selectedFilterHeaderList = null;
            }

            this._selectedFilterId = headerFilterRID.Key;

            SAB.AllocationWorkspaceCurrentHeaderFilter = headerFilterRID.Key;

            FilterHeaderOptions headerFilterOptions = new FilterHeaderOptions();
            headerFilterOptions.USE_WORKSPACE_FIELDS = true;
            headerFilterOptions.filterType = filterTypes.HeaderFilter;
            _selectedFilterHeaderList = SAB.HeaderServerSession.GetHeadersForWorkspace(this._selectedFilterId, headerFilterOptions);

            ROIListOut ROAllocSelectedFilterHeaders = new ROIListOut(eROReturnCode.Successful, null, ROInstanceID, BuildHeaderSummaryList(_selectedFilterHeaderList));

            return ROAllocSelectedFilterHeaders;
        }

        #endregion

        #region "Method to Process AllocationWorklist Action Processing #2524"
        private ROOut ProcessAllocationWorklistAction(ROIntParms actionType)
        {
            string message = null;
            eROReturnCode returnCode = eROReturnCode.Successful;

            bool isProcessCompleted = ProcessAction(actionType.ROInt, ref message);
            if (!isProcessCompleted)
            {
                returnCode = eROReturnCode.Failure;
            }

            return new ROBoolOut(returnCode, message, ROInstanceID, isProcessCompleted);
        }

        internal bool ProcessAction(int action, ref string message)
        {

            bool isActionCompleted = false;

            ApplicationSessionTransaction processTransaction = null;
            GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);
            bool blActionSelected = false;
            bool isMasterOrSubordinate = false;
            _selectedHeaderKeyList = new ArrayList();

            try
            {
                if (action == Include.NoRID)
                {
                    return false;
                }

                blActionSelected = true;
                processTransaction = _applicationSessionTransaction;

                if (!EnqueueHeadersForAction(processTransaction, ref message))
                {
                    return false;
                }


                _allocProfileList = (AllocationProfileList)processTransaction.GetMasterProfileList(eProfileType.Allocation);

                AllocationHeaderProfileList ahpl = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
                ahpl.LoadAll(_allocProfileList);

                processTransaction.SetMasterProfileList(ahpl);

                _allocationHeaderProfileList = (AllocationHeaderProfileList)processTransaction.GetMasterProfileList(eProfileType.AllocationHeader);

                if (!isEligibleToProcessAllocationWorklist(((eAllocationActionType)action)))
                {
                    return false;
                }

                ApplicationBaseAction aMethod = processTransaction.CreateNewMethodAction((eMethodType)action);

                processTransaction.ActionOrigin = eActionOrigin.AllocationWorkspace;
                bool aReviewFlag = false;
                bool aUseSystemTolerancePercent = true;
                double aTolerancePercent = Include.DefaultBalanceTolerancePercent;
                int aStoreFilter = Include.AllStoreFilterRID;
                int aWorkFlowStepKey = -1;
                AllocationWorkFlowStep aAllocationWorkFlowStep
                    = new AllocationWorkFlowStep(aMethod, aComponent, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);

                foreach (int hdrRID in _selectedHeaderKeyList)
                {
                    AllocationHeaderProfile ahp = SAB.HeaderServerSession.GetHeaderData(hdrRID, false, false, true);
                    if (ahp.IsSubordinateHeader)
                    {
                        isMasterOrSubordinate = true;
                    }
                    if (ahp.IsMasterHeader)
                    {
                        isMasterOrSubordinate = true;
                    }
                }

                processTransaction.DoAllocationAction(aAllocationWorkFlowStep);
                eAllocationActionStatus actionStatus = processTransaction.AllocationActionAllHeaderStatus;

                if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
                {
                    isActionCompleted = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (processTransaction != null)
                {
                    processTransaction.DequeueHeaders();
                }
            }

            return isActionCompleted;
        }

        internal bool EnqueueHeadersForAction(ApplicationSessionTransaction aTrans, ref string message)
        {
            string enqMessage;
            List<int> hdrRidList = new List<int>();

            _selectedHeaderKeyList = GetHeadersInAllocation();

            foreach (int key in _selectedHeaderKeyList)
            {
                hdrRidList.Add(key);
            }

            if (hdrRidList.Count == 0)
            {
                message = "There are no worklist items";
                return false;
            }

            if (aTrans.EnqueueHeaders(aTrans.GetHeadersToEnqueue(hdrRidList), out enqMessage))
            {
                return true;
            }
            else
            {
                message = enqMessage;
            }

            return false;
        }

        internal ArrayList GetHeadersInAllocation()
        {
            try
            {
                if (_applicationSessionTransaction != null)
                {
                    var selectedHeaders = (SelectedHeaderList)_applicationSessionTransaction.GetSelectedHeaders();

                    for (int i = 0; i < selectedHeaders.Count; i++)
                    {
                        _selectedHeaderKeyList.Add(selectedHeaders[i].Key);
                    }
                }
            }
            catch
            {
                throw;
            }

            return _selectedHeaderKeyList;
        }

        internal bool isEligibleToProcessAllocationWorklist(eAllocationActionType aAction)
        {
            string errorMessage = string.Empty;
            string errorParm = string.Empty;
            bool okToProcess = true;

            try
            {
                foreach (int key in _selectedHeaderKeyList)
                {
                    AllocationProfile ap = (AllocationProfile)_allocProfileList.FindKey(key);
                    if (ap != null)
                    {
                        ap.ReReadHeader();
                        switch (ap.HeaderAllocationStatus)
                        {
                            case eHeaderAllocationStatus.ReceivedOutOfBalance:
                                if (aAction != eAllocationActionType.BackoutAllocation)
                                {
                                    okToProcess = false;
                                }
                                break;
                            case eHeaderAllocationStatus.ReleaseApproved:
                                if (aAction != eAllocationActionType.Reset &&
                                    aAction != eAllocationActionType.Release)
                                {
                                    okToProcess = false;
                                }
                                break;
                            case eHeaderAllocationStatus.Released:
                                if (aAction != eAllocationActionType.Reset)
                                {
                                    okToProcess = false;
                                }
                                break;
                            default:
                                if (aAction == eAllocationActionType.Reset
                                    && ap.HeaderAllocationStatus != eHeaderAllocationStatus.Released)  // TT#1966-MD - JSmith- DC Fulfillment
                                {
                                    okToProcess = false;
                                }
                                else if (aAction == eAllocationActionType.ChargeIntransit)
                                {
                                    if (ap.HeaderAllocationStatus != eHeaderAllocationStatus.AllInBalance
                                        && ap.HeaderAllocationStatus != eHeaderAllocationStatus.AllocatedInBalance)
                                    {
                                        okToProcess = false;
                                    }
                                }
                                break;
                        }
                        if (!okToProcess)
                        {
                            errorMessage = string.Format
                                (MIDText.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction),
                                MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));

                            okToProcess = false;
                            break;
                        }
                        if (okToProcess)
                        {
                            if (!SAB.ClientServerSession.GlobalOptions.IsReleaseable(ap.HeaderType)
                                && aAction == eAllocationActionType.Release)
                            {
                                if (ap.IsDummy)
                                {
                                    errorParm = MIDText.GetTextOnly((int)eHeaderType.Dummy) + " "
                                        + MIDText.GetTextOnly(eMIDTextCode.lbl_Header);
                                }
                                else
                                {
                                    errorParm = MIDText.GetTextOnly((int)ap.HeaderType);
                                }
                                errorMessage = string.Format
                                    (MIDText.GetText(eMIDTextCode.msg_ActionNotAllowed),
                                    errorParm);

                                okToProcess = false;
                                break;
                            }
                            else if (ap.IsMasterHeader && ap.DCFulfillmentProcessed)
                            {
                                errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed), ap.HeaderID);

                                okToProcess = false;
                            }
                            else if (ap.IsSubordinateHeader && !ap.DCFulfillmentProcessed)
                            {
                                errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed), ap.HeaderID);

                                okToProcess = false;
                            }
                        }

                        if (okToProcess)
                        {
                            if (aAction == eAllocationActionType.BackoutAllocation)
                            {
                                if (ap.IsMasterHeader && ap.DCFulfillmentProcessed)
                                {
                                    errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed), ap.HeaderID);

                                    okToProcess = false;
                                }
                                else if (ap.IsSubordinateHeader && !ap.DCFulfillmentProcessed)
                                {
                                    errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed), ap.HeaderID);

                                    okToProcess = false;
                                }
                            }
                            else if (aAction == eAllocationActionType.ReapplyTotalAllocation && ap.HeaderType == eHeaderType.WorkupTotalBuy)
                            {
                                errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_CannotPerformActionOnHeader),
                                                MIDText.GetTextOnly((int)aAction),
                                                ap.HeaderID,
                                                MIDText.GetTextOnly((int)ap.HeaderType));
                                okToProcess = false;
                                break;
                            }
                        }

                        if (okToProcess)
                        {
                            int masterRID = Include.NoRID;
                            masterRID = ap.MasterRID;
                            if (masterRID != Include.NoRID)
                            {
                                if (!_masterKeyList.Contains(masterRID))
                                {
                                    _masterKeyList.Add(masterRID);
                                }
                            }
                        }

                    }
                }

                if (okToProcess)
                {
                    if (aAction == eAllocationActionType.BackoutAllocation
                        || aAction == eAllocationActionType.BackoutSizeAllocation
                        || aAction == eAllocationActionType.BackoutSizeIntransit
                        || aAction == eAllocationActionType.BackoutStyleIntransit
                        || aAction == eAllocationActionType.Reset)
                    {
                        errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_ActionWarning),
                                                     MIDText.GetTextOnly((int)aAction));
                    }
                }

            }
            catch (Exception ex)
            {
                okToProcess = false;
            }

            return okToProcess;
        }

        #endregion


        #region "Method to Get Columns List to Bind ColumnChooser #RO-2772"
        private ROOut GetAllocationWorklistColumns()
        {
            try
            {                
                BuildHeaderViewFieldMapping();
                DataTable dtColumns = _dtHeaderViewFieldMapping;

                var result = BuildColumnChooserList(dtColumns);
                return new ROIListOut(eROReturnCode.Successful, null, ROInstanceID, result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal List<ROSelectedField> BuildColumnChooserList(DataTable dtHeadersMapper)
        {
            var lstSelectedFields = new List<ROSelectedField>();
            bool isSelected = false;
            try
            {
                for (int i = 0; i < dtHeadersMapper.Rows.Count; i++)
                {                    
                    var allocationWorklist = _allocationWorklistOut;

                    foreach (var item in allocationWorklist)
                    {
                        if (item.ColumnKey.ToString()== dtHeadersMapper.Rows[i].ItemArray[0].ToString())
                        {
                            if (item.IsHidden==false)
                            {
                                isSelected = true;
                                break;
                            }
                            else
                            {
                                isSelected = false;
                                break;
                            }
                           
                        }
                    }

                    lstSelectedFields.Add(new ROSelectedField(dtHeadersMapper.Rows[i].ItemArray[0].ToString(), 
                        dtHeadersMapper.Rows[i].ItemArray[1].ToString(), isSelected));
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return lstSelectedFields;
        }

        #endregion        
    }

}