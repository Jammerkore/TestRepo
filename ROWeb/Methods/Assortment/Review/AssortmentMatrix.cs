using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using System.Reflection;
using C1.Win.C1FlexGrid;
using System.Windows.Forms;
using System.Drawing;
using MIDRetail.Windows.Controls;
using System.Diagnostics;

namespace Logility.ROWeb
{
    public partial class ROAssortment : ROAllocation 
    {
        //=======
        // FIELDS
        //=======
        private readonly object gridLock = new object();

        private bool _formLoading;
        private bool _formLoaded = false;
        private bool _asrtTypeChanged = false;
        private bool _groupByChanged = false;
        private bool _criteriaChanged = false;
        private bool _isAsrtPropertyChanged = false;
        FunctionSecurityProfile filterGlobalSecurity;
        FunctionSecurityProfile filterUserSecurity;
        private const bool THREADED_GRID_LOAD = false;
        private const int BIGCHANGE = 5;
        private const int SMALLCHANGE = 1;
        private const int ROWPAGESIZE = 90;
        private const int COLPAGESIZE = 30;
        private const int MINCOLSIZE = 6;
        private const int FIXEDCOLHEADERS = 3;
        private const int FIXEDROWHEADERS = 1;
        private const string NULL_DATA_STRING = " ";
        private const int HIGHNODECOMBOKEY = -2;
        private const int LOWLEVELTOTALCOMBOKEY = -1;

        private const int Grid1 = 0;
        private const int Grid2 = 1;
        private const int Grid3 = 2;
        private const int Grid4 = 3;
        private const int Grid5 = 4;
        private const int Grid6 = 5;
        private const int Grid7 = 6;
        private const int Grid8 = 7;
        private const int Grid9 = 8;        

        private C1.Win.C1FlexGrid.C1FlexGrid g1;
        private C1.Win.C1FlexGrid.C1FlexGrid g2;
        private C1.Win.C1FlexGrid.C1FlexGrid g3;
        private MIDRetail.Windows.Controls.MIDFlexGrid g4;
        private C1.Win.C1FlexGrid.C1FlexGrid g5;
        private C1.Win.C1FlexGrid.C1FlexGrid g6;
        private C1.Win.C1FlexGrid.C1FlexGrid g7;
        private C1.Win.C1FlexGrid.C1FlexGrid g8;
        private C1.Win.C1FlexGrid.C1FlexGrid g9;        

        private ROCell[][,] _gridData;
        private ROData _asrtMatrixROData;

        private List<RowHeaderTag> _g1RowHeaderList = new List<RowHeaderTag>();
        private List<RowHeaderTag> _g4RowHeaderList = new List<RowHeaderTag>();
        private List<RowHeaderTag> _g7RowHeaderList = new List<RowHeaderTag>();

        public ROCell[,] _dataGrid1;
        public ROCell[,] _dataGrid2;
        public ROCell[,] _dataGrid3;
        public ROCell[,] _dataGrid4;
        public ROCell[,] _dataGrid5;
        public ROCell[,] _dataGrid6;
        public ROCell[,] _dataGrid7;
        public ROCell[,] _dataGrid8;
        public ROCell[,] _dataGrid9;

        // low level row headers (g2)
        protected List<RowHeaderTag> rowHeaderList = new List<RowHeaderTag>();
        // low level row headers (g4)
        protected List<RowHeaderTag> rowHeaderDetailList = new List<RowHeaderTag>();        
        // low level totals row headers (g7)
        protected List<RowHeaderTag> rowHeaderTotalList = new List<RowHeaderTag>();
        // current headers for Grid Mapping
        protected List<ColumnHeaderTag> currentColumnHeaderList = new List<ColumnHeaderTag>();
        protected List<RowHeaderTag> currentRowHeaderList = new List<RowHeaderTag>();


        #region Properties

        public ROCell[,] DataGrid1 { get { return _gridData[Grid1]; } }
        public ROCell[,] DataGrid2 { get { return _gridData[Grid2]; } }
        public ROCell[,] DataGrid3 { get { return _gridData[Grid3]; } }
        public ROCell[,] DataGrid4 { get { return _gridData[Grid4]; } }
        public ROCell[,] DataGrid5 { get { return _gridData[Grid5]; } }
        public ROCell[,] DataGrid6 { get { return _gridData[Grid6]; } }
        public ROCell[,] DataGrid7 { get { return _gridData[Grid7]; } }
        public ROCell[,] DataGrid8 { get { return _gridData[Grid8]; } }
        public ROCell[,] DataGrid9 { get { return _gridData[Grid9]; } }               

        private GetMergeData _getMergeData;        

        public object g4GetMergeData(int row, int col)
        {
            string data;
            int i;

            try
            {
                data = "";

                for (i = 0; i < col; i++)
                {
                    data += g4.GetDataDisplay(row, i);
                }

                data += g4.GetDataDisplay(row, col);

                return data;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private eAllocationAssortmentViewGroupBy GroupBy { get; set; }

        private KeyValuePair<int, string> Attribute { get; set; }

        private KeyValuePair<int, string> AttributeSet { get; set; }

        private bool FormLoaded { get { return _formLoaded; } }

        #endregion

        #region "Get Assortment Review Matrix Data"

        public ROOut GetAssortmentReviewMatrixData(ROAssortmentReviewOptionsParms parms)
        {
            if (_assortmentViewData == null)
            {
                _assortmentViewData = new AssortmentViewData();
            }

            _criteriaChanged = false;
            if (_currStoreGroupLevelProfile != null)
            {
                _formLoaded = true;
                _groupByChanged = false;
                if (Attribute.Key != parms.ROAssortmentReviewOptions.StoreAttribute.Key)
                {
                    _criteriaChanged = true;
                    AttributeSelectionChange(attributeRid: parms.ROAssortmentReviewOptions.StoreAttribute.Key);
                }

                if (AttributeSet.Key != parms.ROAssortmentReviewOptions.AttributeSet.Key)
                {
                    _criteriaChanged = true;
                    AttributeSetSelectionChange(setRid: parms.ROAssortmentReviewOptions.AttributeSet.Key);
                }
                if (GroupBy != (eAllocationAssortmentViewGroupBy)parms.ROAssortmentReviewOptions.GroupBy)
                {
                    _groupByChanged = true;
                    if ((eAllocationAssortmentViewGroupBy)parms.ROAssortmentReviewOptions.GroupBy == eAllocationAssortmentViewGroupBy.Attribute)
                    {
                        GroupByAttribute();
                    }
                    else if ((eAllocationAssortmentViewGroupBy)parms.ROAssortmentReviewOptions.GroupBy == eAllocationAssortmentViewGroupBy.StoreGrade)
                    {
                        GroupByStoreGrade();
                    }

                }
            }
            else if (_assortmentProfile.AssortmentStoreGroupRID != parms.ROAssortmentReviewOptions.StoreAttribute.Key)
            {
                _assortmentProfile.AssortmentStoreGroupRID = parms.ROAssortmentReviewOptions.StoreAttribute.Key;
            }

            GroupBy = (eAllocationAssortmentViewGroupBy)parms.ROAssortmentReviewOptions.GroupBy;
            // Always use the group by from the parameters and not from the view.  This may change once views are being saved.
            if (_openParms != null)
            {
                _openParms.GroupBy = GroupBy;
            }
            Attribute = parms.ROAssortmentReviewOptions.StoreAttribute;
            AttributeSet = parms.ROAssortmentReviewOptions.AttributeSet;

            //if(parms.ROAssortmentReviewOptions.View.Key == Include.Undefined)
            if (!FormLoaded)
            {
                return GetAssortmentReviewMatrixDisplay(_isAsrtPropertyChanged, false, parms);
            }
            else
            {
                // Initialize the data if needed
                if (_asrtMatrixROData == null)
                {
                    GetAssortmentReviewMatrixDisplay(false, false, parms);
                }

                return GetAssortmentReviewMatrixViewData(parms.ROAssortmentReviewOptions.View.Key);
            }

        }
        internal ROOut GetAssortmentReviewMatrixDisplay(bool isAsrtPropertyChanged, bool isAsrtBasisChanged, ROAssortmentReviewOptionsParms parms)
        {

            g1 = new C1.Win.C1FlexGrid.C1FlexGrid();
            g2 = new C1.Win.C1FlexGrid.C1FlexGrid();
            g3 = new C1.Win.C1FlexGrid.C1FlexGrid();
            g4 = new MIDRetail.Windows.Controls.MIDFlexGrid();
            g5 = new C1.Win.C1FlexGrid.C1FlexGrid();
            g6 = new C1.Win.C1FlexGrid.C1FlexGrid();
            g7 = new C1.Win.C1FlexGrid.C1FlexGrid();
            g8 = new C1.Win.C1FlexGrid.C1FlexGrid();
            g9 = new C1.Win.C1FlexGrid.C1FlexGrid();

            g1.Name = "g1";
            g2.Name = "g2";
            g3.Name = "g3";
            g4.Name = "g4";
            g5.Name = "g5";
            g6.Name = "g6";
            g7.Name = "g7";
            g8.Name = "g8";
            g9.Name = "g9";

            _gridData = new ROCell[9][,];
            _commonWaferCoordinateList = new CubeWaferCoordinateList();
            _assortmentViewData = new AssortmentViewData();

            _selectableComponentColumnHeaders = new ArrayList();
            _selectableSummaryRowHeaders = new ArrayList();
            _selectableTotalColumnHeaders = new ArrayList();
            _selectableTotalRowHeaders = new ArrayList();
            _selectableDetailColumnHeaders = new ArrayList();
            _selectableDetailRowHeaders = new ArrayList();
            _sortedComponentColumnHeaders = new SortedList();
            _sortedSummaryRowHeaders = new SortedList();
            _sortedTotalColumnHeaders = new SortedList();
            _sortedDetailColumnHeaders = new SortedList();
            _sortedDetailRowHeaders = new SortedList();
         
            SetGridRedraws(false);

            g1.DrawMode = DrawModeEnum.OwnerDraw;
            g2.DrawMode = DrawModeEnum.OwnerDraw;
            g3.DrawMode = DrawModeEnum.OwnerDraw;
            g4.DrawMode = DrawModeEnum.OwnerDraw;
            g5.DrawMode = DrawModeEnum.OwnerDraw;
            g6.DrawMode = DrawModeEnum.OwnerDraw;
            g7.DrawMode = DrawModeEnum.OwnerDraw;
            g8.DrawMode = DrawModeEnum.OwnerDraw;
            g9.DrawMode = DrawModeEnum.OwnerDraw;
                        
            //_assortmentMemberListbuilt = true;
            //ROData roData = null;

            if (_assortmentProfile == null)
            {
                _assortmentProfile = new AssortmentProfile(_applicationSessionTransaction, string.Empty, _headerRID, SAB.ClientServerSession);
            }

            AddSelectedHeadersToTrans(_applicationSessionTransaction);

            Process(isAsrtPropertyChanged, isAsrtBasisChanged);

            _applicationSessionTransaction.CreateAssortmentViewSelectionCriteria();
            if (parms.ROAssortmentReviewOptions.View.Key != Include.NoRID)
            {
                _applicationSessionTransaction.AssortmentViewRid = parms.ROAssortmentReviewOptions.View.Key;
            }

            _applicationSessionTransaction.CreateAllocationViewSelectionCriteria(true);

            _applicationSessionTransaction.NewAssortmentCriteriaHeaderList();

            AllocationHeaderProfileList headerList = (AllocationHeaderProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AllocationHeader);

            if (_applicationSessionTransaction.AssortmentStoreAttributeRid == Include.NoRID)
            {
                _applicationSessionTransaction.AssortmentStoreAttributeRid = SAB.ClientServerSession.GlobalOptions.AllocationStoreGroupRID;
            }
            if (_applicationSessionTransaction.AllocationStoreAttributeID == Include.NoRID)
            {
                _applicationSessionTransaction.AllocationStoreAttributeID = SAB.ClientServerSession.GlobalOptions.AllocationStoreGroupRID;
            }

            _assortmentReviewAssortment = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentReviewAssortment);

            if (CheckSecurityEnqueue(_applicationSessionTransaction, headerList))
            {
                int i;
                AllocationHeaderProfileList asrtList;
                
                _applicationSessionTransaction.AssortmentViewLoadedFrom = eAssortmentBasisLoadedFrom.AssortmentProperties;
                _applicationSessionTransaction.AssortmentStoreAttributeRid = _assortmentProfile.AssortmentStoreGroupRID;
                                                                                                      
                _windowType = eAssortmentWindowType.Assortment;

                // Set up Security

                if (IsGroupAllocation)
                {
                    FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.GroupAllocationReview);
                    _assortmentReviewAssortment = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.GroupAllocationMatrix);

                    _userViewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.GroupAllocationViewsUser);
                    _globalViewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.GroupAllocationViewsGlobal);

                }
                else
                {
                    FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentReview);
                    _assortmentReviewAssortment = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentReviewAssortment);

                    _userViewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentViewsUser);
                    _globalViewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentViewsGlobal);

                }

                _allocationReviewSummarySecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewSummary);
                _allocationReviewStyleSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewStyle);
                _allocationReviewSizeSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewSize);


                FunctionSecurityProfile filterUserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreUser);
                FunctionSecurityProfile filterGlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreGlobal);

                _userRIDList = new ArrayList();

                _userRIDList.Add(-1);

                if (filterUserSecurity.AllowUpdate || filterUserSecurity.AllowView)
                {
                    _userRIDList.Add(SAB.ClientServerSession.UserRID);
                }

                if (filterGlobalSecurity.AllowUpdate || filterGlobalSecurity.AllowView)
                {
                    _userRIDList.Add(Include.GlobalUserRID);
                }

                if (_applicationSessionTransaction.DataState != eDataState.ReadOnly)
                {
                    if (!_assortmentReviewAssortment.AllowUpdate)
                    {
                        _applicationSessionTransaction.DataState = eDataState.ReadOnly;
                    }
                }

                _asrtCubeGroup = new AssortmentCubeGroup(this.SAB, _applicationSessionTransaction, _windowType);
                
                //Get Header information

                if (_applicationSessionTransaction.AllocationCriteriaExists)
                {
                    _applicationSessionTransaction.UpdateAllocationViewSelectionHeaders();

                    _headerList = (AllocationHeaderProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AllocationHeader);

                    foreach (AllocationHeaderProfile ahp in _headerList)
                    {
                        if (ahp.HeaderType == eHeaderType.Assortment)
                        {
                            _assortmentProfile = (AssortmentProfile)_applicationSessionTransaction.GetAssortmentMemberProfile(ahp.Key);
                        }
                    }
                }

                // default to index since it's set by radio button that is hidden in legacy.
                if (_assortmentProfile.AssortmentGradeBoundary == eGradeBoundary.Unknown)
                {
                    _assortmentProfile.AssortmentGradeBoundary = eGradeBoundary.Index;
                }

                //Get Component Information for all selected headers
                _dtHeaders = _asrtCubeGroup.GetAssortmentComponents();
                if (_dtHeaders.Rows.Count == 0)
                {
                    _disableMatrix = true;
                }

                // Get UserAssortment values                       
                _openParms = LoadParmsFromTransaction(_applicationSessionTransaction.AssortmentViewLoadedFrom); // ??

                // If opened from Assortment Properties, use the GroupBy in the View
                int storeGroupOnView = Include.UndefinedStoreGroupRID;
                if (IsAssortment || IsGroupAllocation)
                {
                    if (_applicationSessionTransaction.AssortmentViewLoadedFrom == eAssortmentBasisLoadedFrom.AssortmentProperties
                        || _applicationSessionTransaction.AssortmentViewLoadedFrom == eAssortmentBasisLoadedFrom.GroupAllocation)
                    {
                        if (GroupBy == eAllocationAssortmentViewGroupBy.None)
                        {
                            DataRow viewRow = _assortmentViewData.AssortmentView_Read(_openParms.ViewRID);
                            eAllocationAssortmentViewGroupBy ViewGroupBy = (eAllocationAssortmentViewGroupBy)int.Parse(viewRow["GROUP_BY_ID"].ToString());

                            if (IsGroupAllocation)
                            {
                                storeGroupOnView = int.Parse(viewRow["SG_RID"].ToString());
                            }

                            _openParms.GroupBy = ViewGroupBy;
                        }
                        else
                        {
                            _openParms.GroupBy = GroupBy;
                        }
                    }
                }
                
                if (_openParms.GroupBy == eAllocationAssortmentViewGroupBy.Attribute)
                {
                    if(Attribute.Key != Include.Undefined
                       && Attribute.Key != _lastStoreGroupValue)
                    {
                        AttributeSelectionChange(Attribute.Key);

                    }
                    GroupByAttribute();
                }
                else
                {
                    GroupByStoreGrade();
                }


                //Open the CubeGroup
                _asrtCubeGroup.OpenCubeGroup(_openParms); 
               
                //Retrieve Variable Lists
                _componentVariables = _asrtCubeGroup.AssortmentComponentVariables;
                _totalVariables = _asrtCubeGroup.AssortmentComputations.AssortmentTotalVariables;
                _detailVariables = _asrtCubeGroup.AssortmentComputations.AssortmentDetailVariables;
                _summaryVariables = _asrtCubeGroup.AssortmentComputations.AssortmentSummaryVariables;
                _quantityVariables = _asrtCubeGroup.AssortmentComputations.AssortmentQuantityVariables;

                //Retrieve Variable ProfileLists
                _componentColumnProfileList = (ProfileList)_componentVariables.VariableProfileList.Clone();
                _totalColumnProfileList = _totalVariables.VariableProfileList;
                _planRowProfileList = _applicationSessionTransaction.PlanComputations.PlanVariables.GetAssortmentPlanningVariableList();
                _detailColumnProfileList = _detailVariables.VariableProfileList;
                _detailRowProfileList = _quantityVariables.VariableProfileList;
                _summaryRowProfileList = _summaryVariables.VariableProfileList;


                //Retrieve StoreGradeProfile list
                _storeGradeProfileList = _asrtCubeGroup.GetFilteredProfileList(eProfileType.StoreGrade);

                SetSelectedView(_openParms.ViewRID);

                ComponentsChanged(false);

                // Set current Store Group
                if (IsAssortment || IsGroupAllocation)
                {

                    if (_applicationSessionTransaction.AssortmentViewLoadedFrom == eAssortmentBasisLoadedFrom.AssortmentProperties)
                    {
                        _asrtCubeGroup.SetStoreGroup(StoreMgmt.StoreGroup_Get(_asrtCubeGroup.AssortmentStoreGroupRID));
                        _lastStoreGroupValue = _asrtCubeGroup.AssortmentStoreGroupRID;
                    }
                    else if (_applicationSessionTransaction.AssortmentViewLoadedFrom == eAssortmentBasisLoadedFrom.UserSelectionCriteria)
                    {
                        _asrtCubeGroup.SetStoreGroup(StoreMgmt.StoreGroup_Get(_applicationSessionTransaction.AssortmentStoreAttributeRid));
                        _lastStoreGroupValue = _applicationSessionTransaction.AssortmentStoreAttributeRid;
                    }

                    else if (_applicationSessionTransaction.AssortmentViewLoadedFrom == eAssortmentBasisLoadedFrom.GroupAllocation)
                    {

                        _asrtCubeGroup.SetStoreGroup(StoreMgmt.StoreGroup_Get(storeGroupOnView));
                        _lastStoreGroupValue = storeGroupOnView;

                    }

                    else
                    {
                        _asrtCubeGroup.SetStoreGroup(StoreMgmt.StoreGroup_Get(Include.AllStoreGroupRID));
                        _lastStoreGroupValue = Include.AllStoreGroupRID;
                    }

                }
                else
                {
                    _asrtCubeGroup.SetStoreGroup(StoreMgmt.StoreGroup_Get(Include.AllStoreGroupRID));
                    _lastStoreGroupValue = Include.AllStoreGroupRID;
                }                

                if (_applicationSessionTransaction.AssortmentViewLoadedFrom == eAssortmentBasisLoadedFrom.UserSelectionCriteria || IsGroupAllocation)
                {
                    RebuildAssortmentSummary();
                }
                else
                {
                    BuildAssortmentSummary();
                }

                AssortmentProfile asp1 = AssortProfile;
                _assortmentRid = asp1.Key;

                GroupName = asp1.HeaderID;
                _asrtCubeGroup.SetStoreFilter(Include.NoRID, null);
                _asrtCubeGroup.ReadData();

                //Retrieve StoreProfile list

                if (IsGroupAllocation)
                {
                    _storeProfileList = _applicationSessionTransaction.GetMasterProfileList(eProfileType.Store);
                }
                else
                {
                    _storeProfileList = _asrtCubeGroup.GetFilteredProfileList(eProfileType.Store);
                }

                _workingDetailProfileList = _storeProfileList;

                //Retrieve StoreGroupListViewProfile list

                _storeGroupListViewProfileList = _asrtCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupListView);

                //Retrieve StoreGroupLevelProfile list

                _storeGroupLevelProfileList = _asrtCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupLevel);
                if (_currStoreGroupLevelProfile == null)
                {
                    _currStoreGroupLevelProfile = (StoreGroupLevelProfile)_storeGroupLevelProfileList.FindKey(AttributeSet.Key);
                    if (_currStoreGroupLevelProfile == null)
                    {
                        _currStoreGroupLevelProfile = (StoreGroupLevelProfile)_storeGroupLevelProfileList[0];
                    }
                }

                //Retrieve StoreGradeProfile list

                _storeGradeProfileList = _asrtCubeGroup.GetFilteredProfileList(eProfileType.StoreGrade);

                _selectableStoreGradeHeaders = new ArrayList();

                i = 0;

                foreach (StoreGradeProfile strGrdProf in _storeGradeProfileList)
                {
                    _selectableStoreGradeHeaders.Add(new RowColProfileHeader(strGrdProf.StoreGrade, true, i, strGrdProf));
                    i++;
                }

                asrtList = _asrtCubeGroup.GetAssortmentList();

                _lastStoreGroupValue = Attribute.Key;

               _lastStoreGroupLevelValue = AttributeSet.Key;

            }

            FormatCol1Grids(false);
            FormatCol2Grids(false, -1, SortEnum.none);
            FormatCol3Grids(false, -1, SortEnum.none);
           
            LoadCurrentPages();

            return new ROGridData(eROReturnCode.Successful, null, ROInstanceID, _asrtMatrixROData, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        #region "Reference Methods"

        
        private void BuildAssortmentSummary()
        {
            AssortProfile.BuildAssortmentSummary();
        }
        private void RebuildAssortmentSummary()
        {
            int i;

            AssortmentProfile asp = AssortProfile;

            if (_applicationSessionTransaction.AssortmentViewLoadedFrom == eAssortmentBasisLoadedFrom.UserSelectionCriteria)
            {
                asp.SetupSummaryfromSelection(_applicationSessionTransaction);
            }
            else if (_applicationSessionTransaction.AssortmentViewLoadedFrom == eAssortmentBasisLoadedFrom.GroupAllocation)
            {
                asp.SetupSummaryfromGroupAllocation(_applicationSessionTransaction);
            }

            // Instantiates the Assortment Summary profile
            if (asp.AssortmentSummaryProfile == null)
            {
                asp.BuildAssortmentSummary();
            }

            asp.AssortmentSummaryProfile.AnchorNodeRid = asp.AssortmentAnchorNodeRid;

            int selHeadCount = 0;
            for (i = 0; i < _applicationSessionTransaction.AllocationCriteria.HeaderList.Count; i++)
            {
                if (_applicationSessionTransaction.AllocationCriteria.HeaderList[i].ProfileType == eProfileType.Assortment)
                {
                    selHeadCount++;
                }
            }


            if (!_applicationSessionTransaction.AssortmentViewSelectionBypass)
            {
                if (IsGroupAllocation)
                {
                    asp.AssortmentSummaryProfile.BasisLoadedFrom = eAssortmentBasisLoadedFrom.GroupAllocation;
                }
                else
                {
                    if (selHeadCount > 1)
                    {
                        asp.AssortmentSummaryProfile.BasisLoadedFrom = eAssortmentBasisLoadedFrom.UserSelectionCriteria;
                    }
                    else
                    {
                        asp.AssortmentSummaryProfile.BasisLoadedFrom = eAssortmentBasisLoadedFrom.AssortmentProperties;
                    }
                }
            }
            else if (IsGroupAllocation)
            {
                asp.AssortmentSummaryProfile.BasisLoadedFrom = eAssortmentBasisLoadedFrom.GroupAllocation;
            }

            else
            {
                asp.AssortmentSummaryProfile.BasisLoadedFrom = eAssortmentBasisLoadedFrom.AssortmentProperties;
            }


            List<int> hierNodeList = new List<int>();
            List<int> versionList = new List<int>();
            List<int> dateRangeList = new List<int>();
            List<double> weightList = new List<double>();

            foreach (AssortmentBasis ab in asp.AssortmentBasisList)
            {
                hierNodeList.Add(ab.HierarchyNodeProfile.Key);
                versionList.Add(ab.VersionProfile.Key);
                dateRangeList.Add(ab.HorizonDate.Key);
                weightList.Add(ab.Weight);
            }
            asp.AssortmentSummaryProfile.ClearAssortmentSummaryTable();
            asp.BasisReader.AssortmentPlanCubeGroup.OpenGradeCubes(asp.AssortmentStoreGradeList, asp.AssortmentSummaryProfile.SetGradeStoreXRef);

            // Reads variable data from basis data reader
            asp.AssortmentSummaryProfile.Process(_applicationSessionTransaction, asp.AssortmentAnchorNodeRid, asp.AssortmentVariableType, hierNodeList,
                   versionList, dateRangeList, weightList, asp.AssortmentIncludeSimilarStores, asp.AssortmentIncludeIntransit,
                   asp.AssortmentIncludeOnhand, asp.AssortmentIncludeCommitted, asp.AssortmentAverageBy, true, true);

            //   ?? need to check whether this commit is for what and needed?
            if (IsGroupAllocation)
            {
                bool doCommit = false;
                try
                {
                    if (!asp.HeaderDataRecord.ConnectionIsOpen)
                    {
                        asp.HeaderDataRecord.OpenUpdateConnection();
                        doCommit = true;
                    }

                    asp.AssortmentSummaryProfile.WriteAssortmentStoreSummary(asp.HeaderDataRecord);

                    if (doCommit)
                    {
                        asp.HeaderDataRecord.CommitData();
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (doCommit)
                    {
                        asp.HeaderDataRecord.CloseUpdateConnection();
                    }
                }
            }

            asp.AssortmentSummaryProfile.BuildSummary(_lastStoreGroupValue);
        }
        private ApplicationSessionTransaction NewTransFromSelectedHeaders()
        {
            try
            {
                ApplicationSessionTransaction newTrans = SAB.ApplicationServerSession.CreateTransaction();                
                AddSelectedHeadersToTrans(newTrans);
                return newTrans;                
            }
            catch
            {
                throw;
            }
        }
        private void AddSelectedHeadersToTrans(ApplicationSessionTransaction aTrans)
        {
            try
            {
                if (_assortmentMemberListbuilt)
                {
                    return;
                }

                aTrans.NewAllocationMasterProfileList();

                aTrans.DequeueHeaders();
                _selectedHeaderKeyList.Clear();
                _selectedAssortmentKeyList.Clear(); 

                GetAllHeadersInAssortment(_assortmentProfile.Key);

                int[] selectedHeaderArray = new int[_selectedHeaderKeyList.Count];
                _selectedHeaderKeyList.CopyTo(selectedHeaderArray);
                int[] selectedAssortmentArray = new int[_selectedAssortmentKeyList.Count]; 
                _selectedAssortmentKeyList.CopyTo(selectedAssortmentArray);     
               
                string enqMessage = string.Empty;
                List<int> hdrList = new List<int>(selectedAssortmentArray);
                hdrList.AddRange(selectedHeaderArray);                      
                                                                           
                bool success = aTrans.EnqueueHeaders(hdrList, out enqMessage);

                // load the selected headers in the Application session transaction                
                aTrans.CreateMasterAssortmentMemberListFromSelectedHeaders(selectedAssortmentArray, selectedHeaderArray);

                _assortmentMemberListbuilt = true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Fills in _selectedHeaderKeyList from aAsrtRID.
        /// </summary>
        /// <param name="aAsrtRID"></param>
        private void GetAllHeadersInAssortment(int aAsrtRID)
        {
            try
            {
                ArrayList al = SAB.HeaderServerSession.GetHeadersInAssortment(aAsrtRID);
                for (int i = 0; i < al.Count; i++)
                {
                    int hdrRID = (int)al[i];
                    if (hdrRID != aAsrtRID)  
                    {
                        _selectedHeaderKeyList.Add(hdrRID);
                        
                    }
                    else
                    {
                        _selectedAssortmentKeyList.Add(hdrRID);
                    }
                    
                }
            }
            catch
            {
                throw;
            }
        }
        private bool Process(bool isAsrtPropertyChanged, bool isAsrtBasisChanged)
        {
            try
            {
                bool doCommit = false;
                bool processed = false;

                try
                {
                    if (!_assortmentProfile.HeaderDataRecord.ConnectionIsOpen)
                    {
                        _assortmentProfile.HeaderDataRecord.OpenUpdateConnection();
                        doCommit = true;
                    }

                    List<int> hierNodeList = new List<int>();
                    List<int> versionList = new List<int>();
                    List<int> dateRangeList = new List<int>();
                    List<double> weightList = new List<double>();

                    foreach (AssortmentBasis ab in _assortmentProfile.AssortmentBasisList)
                    {
                        hierNodeList.Add(ab.HierarchyNodeProfile.Key);
                        versionList.Add(ab.VersionProfile.Key);
                        dateRangeList.Add(ab.HorizonDate.Key);
                        weightList.Add(ab.Weight);

                    }
                    _assortmentProfile.SetAssortmentBasis(_assortmentProfile.HeaderRID, hierNodeList, versionList, dateRangeList, weightList);
                                       
                    if (isAsrtPropertyChanged)
                    {
                        if (_assortmentProfile.IsAssortment
                        && _assortmentProfile.BeginDay != _assortmentProfile.AssortmentBeginDay.Date)
                        {
                            _assortmentProfile.BeginDayIsSet = false;
                            _assortmentProfile.BeginDay = Include.UndefinedDate;
                            _assortmentProfile.BeginDay = _assortmentProfile.AssortmentBeginDay.Date;
                        }

                        if (_assortmentProfile.IsAssortment
                            && _assortmentProfile.ShipToDay != _assortmentProfile.AssortmentApplyToDate.Date)
                        {
                            _assortmentProfile.ResetAssortmentStoreShipDates();
                            _assortmentProfile.ShipToDay = Include.UndefinedDate;
                            _assortmentProfile.ShipToDay = _assortmentProfile.AssortmentApplyToDate.Date;
                        }
                    }

                    
                    if (_assortmentProfile.AssortmentSummaryProfile == null || isAsrtPropertyChanged)                    

                    {
                        _assortmentProfile.BuildAssortmentSummary();                       
                    }
                    else
                    {
                        StoreGroupProfile sgp = StoreMgmt.StoreGroup_GetFilled(_assortmentProfile.AssortmentStoreGroupRID); //SAB.StoreServerSession.GetStoreGroupFilled(_assortmentProfile.AssortmentStoreGroupRID);
                        _assortmentProfile.AssortmentSummaryProfile.ReinitializeSummary(sgp, _assortmentProfile.AssortmentStoreGradeList);
                    }
                    bool refreshBasisData = false;
                    
                    if (isAsrtPropertyChanged)
                    {
                        _assortmentProfile.AssortmentSummaryProfile.ClearAssortmentSummaryTable();
                        _reprocess = true;
                        refreshBasisData = isAsrtBasisChanged;                        
                    }

                    _assortmentProfile.AssortmentSummaryProfile.Process(_applicationSessionTransaction, _assortmentProfile.AssortmentAnchorNodeRid, _assortmentProfile.AssortmentVariableType, hierNodeList, versionList, dateRangeList, weightList, _assortmentProfile.AssortmentIncludeSimilarStores,
                        _assortmentProfile.AssortmentIncludeIntransit, _assortmentProfile.AssortmentIncludeOnhand, _assortmentProfile.AssortmentIncludeCommitted, _assortmentProfile.AssortmentAverageBy, _reprocess, refreshBasisData);

                    if (_reprocess)
                    {
                        _assortmentProfile.AssortmentSummaryProfile.WriteAssortmentStoreSummary(_assortmentProfile.HeaderDataRecord);
                        if (doCommit)
                        {
                            _assortmentProfile.HeaderDataRecord.CommitData();
                        }

                    }
                    
                    if (isAsrtPropertyChanged)
                    {
                        _assortmentProfile.AssortmentSummaryProfile.RereadStoreSummaryData();
                        _assortmentProfile.AssortmentSummaryProfile.BuildSummary(_assortmentProfile.LastSglRidUsedInSummary);
                    }

                    processed = true;                    
                }
                finally
                {
                    if (doCommit)
                    {
                        _assortmentProfile.HeaderDataRecord.CloseUpdateConnection();
                    }

                    _isAsrtPropertyChanged = false;
                }

                return processed;

            }
            catch (Exception exc)
            {
                throw;
            }
        }
        protected bool CheckSecurityEnqueue(ApplicationSessionTransaction processTransaction, AllocationHeaderProfileList headerList)
        {
            try
            {
                bool OKToEnqueue = true;
                FunctionSecurityProfile nodeFunctionSecurity;
                // Check Function security
                if (_assortmentReviewAssortment.AccessDenied)
                {
                    string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UnauthorizedFunctionAccess);                    
                    return false;
                }

                foreach (AllocationHeaderProfile ahp in headerList)
                {
                    nodeFunctionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(ahp.StyleHnRID, 
                                                                                                            eSecurityFunctions.AssortmentReview, 
                                                                                                            (int)eSecurityTypes.Allocation);
                    if (!nodeFunctionSecurity.AllowUpdate)
                    {
                        OKToEnqueue = false;
                        break;
                    }
                }

                if (OKToEnqueue)
                {
                    string enqMessage = string.Empty;

                    List<int> selectedHdrs = new List<int>();
                    foreach (AllocationHeaderProfile ahp in headerList)
                    {
                        selectedHdrs.Add(ahp.Key);
                    }

                    if (processTransaction.EnqueueHeaders(processTransaction.GetHeadersToEnqueue(selectedHdrs), out enqMessage))
                    {
                        processTransaction.DataState = eDataState.Updatable;
                    }
                    else
                    {
                        processTransaction.DataState = eDataState.ReadOnly;
                    }
                }
                else
                {
                    processTransaction.DataState = eDataState.ReadOnly;
                }
                return true;
            }
            catch (CancelProcessException)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void GroupByAttribute()     
        {

            try
            {
                if (_columnGroupedBy != eAllocationAssortmentViewGroupBy.Attribute)
                {
                    _columnGroupedBy = eAllocationAssortmentViewGroupBy.Attribute;                   
                }
                
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        private void GroupByStoreGrade()
        {

            try
            {
                if (_columnGroupedBy != eAllocationAssortmentViewGroupBy.StoreGrade)
                {
                    _columnGroupedBy = eAllocationAssortmentViewGroupBy.StoreGrade;
                    
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        private void SetSelectedView(int selectedView)
        {

            int assortView = 0;

            if (GetAssortmentType() == eAssortmentType.GroupAllocation)
            {
                assortView = Include.DefaultGroupAllocationViewRID;
            }
            else if (GetAssortmentType() == eAssortmentType.PostReceipt)
            {
                assortView = Include.DefaultPostReceiptViewRID;
            }
            else
            {
                assortView = Include.DefaultAssortmentViewRID;
            }
            if (selectedView == Include.Undefined
                && assortView != 0)
            {
                ViewSelectionChange((int)assortView);
            }
            else if (!FormLoaded)
            {
                ViewSelectionChange(selectedView);
            }
        }
        private void ViewSelectionChange(int viewRid)   
        {
            int selectedValue;

            try
            {               
                DataRow viewRow = _assortmentViewData.AssortmentView_Read(viewRid);
                if (viewRow == null)
                {
                    string errMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_as_GridViewDoesNotExist);
                    
                    _viewDeleted = true;
                    SetSelectedView(-1);
                    _viewDeleted = false;
                    return;
                }
               
                selectedValue = viewRid;

                if ((selectedValue != _lastViewValue)
                    || _groupByChanged
                    || _criteriaChanged
                    )
                {
                    SetCurrentView(selectedValue);
                    LoadView(selectedValue);
                    if (FormLoaded)
                    {
                        ComponentsChanged();
                        ReformatRowsChanged(true);
                    }
                    _lastViewValue = selectedValue;
                }
            }
            catch (Exception exc)
            {
                throw;
            }
        }
        private void SetCurrentView(int aViewID)
        {
            DataRow drCurrView;

            try
            {
                _openParms.ViewRID = aViewID;
                drCurrView = _assortmentViewData.AssortmentView_Read(_openParms.ViewRID);
                _openParms.ViewName = Convert.ToString(drCurrView["VIEW_ID"], CultureInfo.CurrentUICulture);
                _openParms.ViewUserID = Convert.ToInt32(drCurrView["USER_RID"], CultureInfo.CurrentUICulture);
                
                // Always use the group by from the parameters and not from the view.  This may change once views are being saved.
                //if (FormLoaded
                //    && !_groupByChanged)
                //{
                //    _openParms.GroupBy = (eAllocationAssortmentViewGroupBy)Convert.ToInt32(drCurrView["GROUP_BY_ID"]);
                //}  
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void SetGridRedraws(bool aValue)
        {
            try
            {
                _currentRedrawState = aValue;

                g2.Redraw = aValue;
                g3.Redraw = aValue;
                g4.Redraw = aValue;
                g5.Redraw = aValue;
                g6.Redraw = aValue;
                g7.Redraw = aValue;
                g8.Redraw = aValue;
                g9.Redraw = aValue;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void ReformatStoreGroupChanged(bool aClearGrid)
        {
            try
            {
                FormatCol1Grids(aClearGrid);
                FormatCol2Grids(aClearGrid, -1, SortEnum.none);
                FormatCol3Grids(aClearGrid, -1, SortEnum.none);

                LoadCurrentPages();
                
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void ReformatGroupingChanged(bool aClearGrid, int ag2VarSortKey, SortEnum ag2SortDir, int ag3VarSortKey, SortEnum ag3SortDir)
        {
            try
            {
                FormatCol1Grids(aClearGrid);
                FormatCol2Grids(aClearGrid, ag2VarSortKey, ag2SortDir);
                FormatCol3Grids(aClearGrid, ag3VarSortKey, ag3SortDir);
                
                LoadCurrentPages();
                
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void LoadCurrentPages()
        {
            try
            {
                _asrtMatrixROData = new ROData();
                ((PagingGridTag)g2.Tag).AllocatePageArray();
                ((PagingGridTag)g3.Tag).AllocatePageArray();
                ((PagingGridTag)g5.Tag).AllocatePageArray();
                ((PagingGridTag)g6.Tag).AllocatePageArray();
                ((PagingGridTag)g8.Tag).AllocatePageArray();
                ((PagingGridTag)g9.Tag).AllocatePageArray();

                if (g2.Rows.Count > 0 && g2.Cols.Count > 0)
                {
                    LoadCurrentGridPages(g2);
                    ROCells roCells = GetAssortmentMatrixData(Grid2, g2.Rows.Count, g2.Cols.Count, Grid1, g1.Rows.Count, 1);
                    _asrtMatrixROData.AddCells(eDataType.AssortmentSummaryTotals, roCells);
                }
                if (g3.Rows.Count > 0 && g3.Cols.Count > 0)
                {
                    LoadCurrentGridPages(g3);
                    ROCells roCells = GetAssortmentMatrixData(Grid3, g3.Rows.Count, g3.Cols.Count, Grid1, g1.Rows.Count, 1);
                    _asrtMatrixROData.AddCells(eDataType.AssortmentSummaryDetail, roCells);
                }
                if (g5.Rows.Count > 0 && g5.Cols.Count > 0)
                {
                    LoadCurrentGridPages(g5);
                    ROCells roCells = GetAssortmentMatrixData(Grid5, g5.Rows.Count, g5.Cols.Count, Grid5, g5.Rows.Count, 1);
                    _asrtMatrixROData.AddCells(eDataType.AssortmentDetailTotals, roCells);
                }
                if (g6.Rows.Count > 0 && g6.Cols.Count > 0)
                {
                    LoadCurrentGridPages(g6);
                    ROCells roCells = GetAssortmentMatrixData(Grid6, g6.Rows.Count, g6.Cols.Count, Grid5, g5.Rows.Count, 1);
                    _asrtMatrixROData.AddCells(eDataType.AssortmentDetailDetail, roCells);
                }
                if (g8.Rows.Count > 0 && g8.Cols.Count > 0)
                {
                    LoadCurrentGridPages(g8);
                    ROCells roCells = GetAssortmentMatrixData(Grid8, g8.Rows.Count, g8.Cols.Count, Grid7, g7.Rows.Count, 1);
                    roCells.Columns = _asrtMatrixROData.Cells[eDataType.AssortmentDetailTotals].Columns;
                    _asrtMatrixROData.AddCells(eDataType.AssortmentTotalTotals, roCells);
                }
                if (g9.Rows.Count > 0 && g9.Cols.Count > 0)
                {
                    LoadCurrentGridPages(g9);
                    ROCells roCells = GetAssortmentMatrixData(Grid9, g9.Rows.Count, g9.Cols.Count, Grid7, g7.Rows.Count, 1);
                    roCells.Columns = _asrtMatrixROData.Cells[eDataType.AssortmentDetailDetail].Columns;
                    _asrtMatrixROData.AddCells(eDataType.AssortmentTotalDetail, roCells);
                }

                //Column4 grids                
                if (g4.Rows.Count > 0 && g4.Cols.Count > 0)
                {
                    ROCells rocells = MapRowColumnAttributesForGrid4(Grid4, _gridData[Grid4].GetLength(0), _sortedComponentColumnHeaders.Count);
                    _asrtMatrixROData.AddCells(eDataType.AssortmentDetailLabels, rocells);
                }                
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public void LoadCurrentGridPages(C1FlexGrid aGrid)
        {
            PagingGridTag gridTag;
            ArrayList pages;
            Cursor holdCursor;

            try
            {
                gridTag = (PagingGridTag)aGrid.Tag;
                pages = gridTag.GetPagesToLoad(aGrid.TopRow, aGrid.LeftCol, Math.Min(aGrid.TopRow + ROWPAGESIZE - 1, aGrid.Rows.Count), Math.Min(aGrid.LeftCol + COLPAGESIZE - 1, aGrid.Cols.Count));

                if (pages.Count > 0)
                {
                    holdCursor = Cursor.Current;
                    Cursor.Current = Cursors.WaitCursor;

                    try
                    {
                        foreach (Point page in pages)
                        {
                            bool loadPage = true;
                            int count = 0;
                            // try multiple times to handle thread invoke issues
                            while (loadPage)
                            {
                                try
                                {
                                    loadPage = false;
                                    // BEGIN RO-2960 RDewey - Added check for InvokeRequired to get around threading issue with C1FlexGrid
                                    if (aGrid.InvokeRequired)
                                    {
                                        gridTag.LoadPageForMatrix(page);
                                    }
                                    else
                                    {
                                        gridTag.LoadPage(page);
                                    }
                                    // END RO-2960 RDewey
                                }
                                catch (System.ComponentModel.InvalidAsynchronousStateException)
                                {
                                    if (count < 3)
                                    {
                                        loadPage = true;
                                    }
                                    else
                                    {
                                        System.Threading.Thread.Sleep(100);
                                    }
                                    ++count;
                                }
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        string message = exc.ToString();
                        throw;
                    }
                    finally
                    {
                        Cursor.Current = holdCursor;
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void ReloadGridData(bool reload = true, bool rebuildComponents = true, bool reloadBlockedCells = true, bool reloadHeaders = true, bool reformatRows = false)
        {
            CloseAndReOpenCubeGroup();
            //UpdateData(true);
            UpdateData(reload: reload, rebuildComponents: rebuildComponents, reloadBlockedCells: reloadBlockedCells, reloadHeaders: reloadHeaders, reformatRows: reformatRows);
            LoadCurrentPages();
        }

        private void LoadView(int aViewID)
        {
            int i;
            AssortmentComponentVariableProfile compVarProf;
            AssortmentTotalVariableProfile totVarProf;
            AssortmentSummaryVariableProfile summVarProf;
            
            VariableProfile planVarProf;
           
            AssortmentDetailVariableProfile detVarProf;
            QuantityVariableProfile quantVarProf;
            
            DataRow viewRow;
            Hashtable varKeyHash;           

            try
            {
                
                if (FormLoaded)
                {
                    viewRow = _assortmentViewData.AssortmentView_Read(aViewID);
                    // Always use the group by from the parameters and not from the view.  This may change once views are being saved.
                    //eAllocationAssortmentViewGroupBy ViewGroupBy = (eAllocationAssortmentViewGroupBy)int.Parse(viewRow["GROUP_BY_ID"].ToString());

                    int storeGroupOnView = Include.UndefinedStoreGroupRID;
                    if (IsGroupAllocation)
                    {
                        storeGroupOnView = int.Parse(viewRow["SG_RID"].ToString());
                    }

                    // Always use the group by from the parameters and not from the view.  This may change once views are being saved.
                    //if (ViewGroupBy == eAllocationAssortmentViewGroupBy.Attribute)
                    //{                        
                    //    GroupByAttribute();
                    //}

                    //if (ViewGroupBy == eAllocationAssortmentViewGroupBy.StoreGrade)
                    //{                        
                    //    GroupByStoreGrade();
                    //}

                    if (storeGroupOnView != Include.UndefinedStoreGroupRID)
                    {
                        AttributeSelectionChange(storeGroupOnView);
                    }
                }

                _assrtViewDetail = _assortmentViewData.AssortmentViewDetail_Read(aViewID);

                //Load Component columns

                varKeyHash = new Hashtable();
                _selectableComponentColumnHeaders.Clear();

                foreach (DataRow row in _assrtViewDetail.Rows)
                {
                    if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eAssortmentViewAxis.Component)
                    {
                        compVarProf = (AssortmentComponentVariableProfile)_componentColumnProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));
                        if (compVarProf != null)
                        {
                            varKeyHash.Add(compVarProf.Key, row);
                        }
                    }
                }

                _placeholderSelected = false;

                foreach (AssortmentComponentVariableProfile varProf in _componentColumnProfileList)
                {
                    viewRow = (DataRow)varKeyHash[varProf.Key];
                    if (viewRow != null)
                    {
                        _selectableComponentColumnHeaders.Add(new RowColProfileHeader(varProf.VariableName, true, Include.ConvertCharToBool(Convert.ToChar(viewRow["SUMMARIZED_IND"], CultureInfo.CurrentUICulture)), Convert.ToInt32(viewRow["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), varProf));
                    }
                    else
                    {
                        _selectableComponentColumnHeaders.Add(new RowColProfileHeader(varProf.VariableName, false, false, -1, varProf));
                    }
                }

                _sortedComponentColumnHeaders.Clear();

                CreateSortedList(_selectableComponentColumnHeaders, _sortedComponentColumnHeaders);

                //Load Summary columns

                varKeyHash = new Hashtable();
                _selectableSummaryRowHeaders.Clear();

                foreach (DataRow row in _assrtViewDetail.Rows)
                {
                    if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eAssortmentViewAxis.SummaryColumn)
                    {
                        summVarProf = (AssortmentSummaryVariableProfile)_summaryRowProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));
                        if (summVarProf != null)
                        {
                            varKeyHash.Add(summVarProf.Key, row);
                        }                        
                        else
                        {
                            planVarProf = (VariableProfile)_planRowProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));
                            if (planVarProf != null)
                            {
                                varKeyHash.Add(planVarProf.Key, row);
                            }
                        }
                    }
                }

                foreach (AssortmentSummaryVariableProfile varProf in _summaryRowProfileList)
                {
                    if (!IsGroupAllocation && varProf.VariableName == "Average Units")
                    {
                        
                    }
                    // Skip "Balance" if not Group Allocation
                    if (!IsGroupAllocation && varProf.VariableName == "Balance")
                    {
                        continue;
                    }
                    if (IsGroupAllocation && varProf.VariableName == "Average Units")
                    {
                        continue;
                    }
                    
                    string varName = varProf.VariableName;
                    if (!IsGroupAllocation && varProf.VariableName == "Basis")
                    {
                        varName = "Remove Basis";
                    }
                    
                    if (IsGroupAllocation && varProf.VariableName == "Basis")
                    {
                        varName = "Stock";
                    }
                    if (IsGroupAllocation && varProf.VariableName == "Units")
                    {
                        varName = "Sales";
                    }

                    viewRow = (DataRow)varKeyHash[varProf.Key];
                    if (varName == "Remove Basis")
                    {
                        viewRow = null;
                    }

                    if (viewRow != null)
                    {
                        _selectableSummaryRowHeaders.Add(new RowColProfileHeader(varName, true, false, Convert.ToInt32(viewRow["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), varProf));
                    }
                    else
                    {                        
                        if (varName != "Remove Basis")
                        {
                            _selectableSummaryRowHeaders.Add(new RowColProfileHeader(varName, false, false, -1, varProf));
                        }
                    }
                }
                foreach (VariableProfile varProf in _planRowProfileList)
                {
                    viewRow = (DataRow)varKeyHash[varProf.Key];
                    if (viewRow != null)
                    {
                        _selectableSummaryRowHeaders.Add(new RowColProfileHeader(varProf.VariableName, true, false, Convert.ToInt32(viewRow["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), varProf));
                    }
                    else
                    {
                        _selectableSummaryRowHeaders.Add(new RowColProfileHeader(varProf.VariableName, false, false, -1, varProf));
                    }
                }
                _sortedSummaryRowHeaders.Clear();

                CreateSortedList(_selectableSummaryRowHeaders, _sortedSummaryRowHeaders);

                //Load Total columns

                varKeyHash = new Hashtable();
                _selectableTotalColumnHeaders.Clear();

                foreach (DataRow row in _assrtViewDetail.Rows)
                {
                    if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eAssortmentViewAxis.TotalColumn)
                    {
                        totVarProf = (AssortmentTotalVariableProfile)_totalColumnProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));
                        if (totVarProf != null)
                        {
                            varKeyHash.Add(totVarProf.Key, row);
                        }
                    }
                }

                foreach (AssortmentTotalVariableProfile varProf in _totalColumnProfileList)
                {                    
                    if (IsGroupAllocation && (varProf.VariableName == "On Hand" || varProf.VariableName == "Intransit"))
                    {
                        continue;
                    }
                    if (!IsGroupAllocation && varProf.Key == (int)eAssortmentTotalVariables.NumStoresAllocated)
                    {
                        continue;
                    }
                    viewRow = (DataRow)varKeyHash[varProf.Key];
                    if (viewRow != null)
                    {
                        _selectableTotalColumnHeaders.Add(new RowColProfileHeader(varProf.VariableName, true, false, Convert.ToInt32(viewRow["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), varProf));
                    }
                    else
                    {
                        _selectableTotalColumnHeaders.Add(new RowColProfileHeader(varProf.VariableName, false, false, -1, varProf));
                    }
                }

                _sortedTotalColumnHeaders.Clear();

                CreateSortedList(_selectableTotalColumnHeaders, _sortedTotalColumnHeaders);

                //Load Detail columns

                varKeyHash = new Hashtable();
                _selectableDetailColumnHeaders.Clear();

                foreach (DataRow row in _assrtViewDetail.Rows)
                {
                    if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eAssortmentViewAxis.DetailColumn)
                    {
                        detVarProf = (AssortmentDetailVariableProfile)_detailColumnProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));
                        if (detVarProf != null)
                        {
                            varKeyHash.Add(detVarProf.Key, row);
                        }
                    }
                }

                foreach (AssortmentDetailVariableProfile varProf in _detailColumnProfileList)
                {
                    if (!IsGroupAllocation && varProf.Key == (int)eAssortmentDetailVariables.NumStoresAllocated)
                    {
                        continue;
                    }
                    viewRow = (DataRow)varKeyHash[varProf.Key];
                    if (viewRow != null)
                    {
                        _selectableDetailColumnHeaders.Add(new RowColProfileHeader(varProf.VariableName, true, false, Convert.ToInt32(viewRow["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), varProf));
                    }
                    else
                    {
                        _selectableDetailColumnHeaders.Add(new RowColProfileHeader(varProf.VariableName, false, false, -1, varProf));
                    }
                }

                _sortedDetailColumnHeaders.Clear();

                CreateSortedList(_selectableDetailColumnHeaders, _sortedDetailColumnHeaders);

                //Load Detail rows

                varKeyHash = new Hashtable();
                _selectableDetailRowHeaders.Clear();

                foreach (DataRow row in _assrtViewDetail.Rows)
                {
                    if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eAssortmentViewAxis.DetailRow)
                    {
                        quantVarProf = (QuantityVariableProfile)_detailRowProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));
                        if (quantVarProf != null)
                        {
                            varKeyHash.Add(quantVarProf.Key, row);
                        }
                    }
                }

                foreach (QuantityVariableProfile varProf in _detailRowProfileList)
                {
                    viewRow = (DataRow)varKeyHash[varProf.Key];
                    if (viewRow != null)
                    {
                        _selectableDetailRowHeaders.Add(new RowColProfileHeader(varProf.VariableName, true, false, Convert.ToInt32(viewRow["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), varProf));
                    }
                    else
                    {
                        _selectableDetailRowHeaders.Add(new RowColProfileHeader(varProf.VariableName, false, false, -1, varProf));
                    }
                }

                _sortedDetailRowHeaders.Clear();

                CreateSortedList(_selectableDetailRowHeaders, _sortedDetailRowHeaders);

                // Build Grades
                BuildGrades();  
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public void LoadSurroundingPages()  // TT#795-MD - stodd - Build Packs not working on a Placeholder in an assortment.
        {
            try
            {
                if (g2.Rows.Count > 0 && g2.Cols.Count > 0)
                {
                    LoadSurroundingGridPages(g2);
                }
                if (g3.Rows.Count > 0 && g3.Cols.Count > 0)
                {
                    LoadSurroundingGridPages(g3);
                }
                if (g5.Rows.Count > 0 && g5.Cols.Count > 0)
                {
                    LoadSurroundingGridPages(g5);
                }
                if (g6.Rows.Count > 0 && g6.Cols.Count > 0)
                {
                    LoadSurroundingGridPages(g6);
                }
                if (g8.Rows.Count > 0 && g8.Cols.Count > 0)
                {
                    LoadSurroundingGridPages(g8);
                }
                if (g9.Rows.Count > 0 && g9.Cols.Count > 0)
                {
                    LoadSurroundingGridPages(g9);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void LoadSurroundingGridPages(C1FlexGrid aGrid)
        {
            PagingGridTag gridTag;
            ArrayList pages;

            try
            {
                if (THREADED_GRID_LOAD)
                {
                    gridTag = (PagingGridTag)aGrid.Tag;
                    pages = gridTag.GetSurroundingPagesToLoad(aGrid.TopRow, aGrid.LeftCol, Math.Min(aGrid.TopRow + ROWPAGESIZE - 1, aGrid.Rows.Count), Math.Min(aGrid.LeftCol + COLPAGESIZE - 1, aGrid.Cols.Count));

                    foreach (Point page in pages)
                    {
                        gridTag.LoadPageInBackground(page);
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void BuildGrades()
        {
            int i;
            _selectableStoreGradeHeaders = new ArrayList();
            i = 0;

            foreach (StoreGradeProfile strGrdProf in _storeGradeProfileList)
            {
                _selectableStoreGradeHeaders.Add(new RowColProfileHeader(strGrdProf.StoreGrade, true, i, strGrdProf));
                i++;
            }

            _sortedStoreGradeHeaders = new SortedList();

            CreateSortedList(_selectableStoreGradeHeaders, _sortedStoreGradeHeaders);
        }
        private void AttributeSelectionChange(int attributeRid) 
        {
            try
            {
                int selectedItem = attributeRid;

                {
                    //if (FormLoaded
                    //    && selectedItem != _lastStoreGroupValue)
                    if (selectedItem != _lastStoreGroupValue)
                    {
                        if (_storeGroupListViewProfileList == null)
                        {
                            _storeGroupListViewProfileList = _asrtCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupListView);
                        }
                        _asrtCubeGroup.SetStoreGroup(new StoreGroupProfile(((StoreGroupListViewProfile)_storeGroupListViewProfileList.FindKey(selectedItem)).Key));
                        _asrtCubeGroup.ReadData(true);
                    }
					
                    _storeGroupLevelProfileList = _asrtCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupLevel);

                    //_lastStoreGroupLevelValue = AttributeSet.Key;

                    _lastStoreGroupValue = selectedItem;
                    _applicationSessionTransaction.CurrentStoreGroupProfile = StoreMgmt.StoreGroup_Get(_applicationSessionTransaction.AllocationStoreAttributeID);

                    _applicationSessionTransaction.AllocationStoreAttributeID = Convert.ToInt32(selectedItem);
                    _applicationSessionTransaction.CurrentStoreGroupProfile = StoreMgmt.StoreGroup_Get(_applicationSessionTransaction.AllocationStoreAttributeID);

                }
            }
            catch (Exception exc)
            {

            }
        }

        private void AttributeSetSelectionChange(int setRid)    
        {
            ProfileXRef storeSetXRef;
            ArrayList detailList;
            StoreProfile storeProf;

            _workingDetailProfileList = new ProfileList(eProfileType.Store);

            storeSetXRef = (ProfileXRef)_asrtCubeGroup.GetProfileXRef(new ProfileXRef(eProfileType.StoreGroupLevel, eProfileType.Store));
            detailList = storeSetXRef.GetDetailList(setRid);  
            if (detailList != null)
            {
                foreach (int storeId in detailList)
                {
                    storeProf = (StoreProfile)_storeProfileList.FindKey(storeId);
                    if (storeProf != null)
                    {
                        _workingDetailProfileList.Add(storeProf);
                    }
                }
            }

            _currStoreGroupLevelProfile = (StoreGroupLevelProfile)_storeGroupLevelProfileList.FindKey(setRid);

            //ReformatStoreGroupChanged(false);
        }

        private void CreateSortedList(ArrayList aSelectableList, SortedList aSortedList)
        {            
            SortedList summaryList;
            SortedList selectedList;
            IDictionaryEnumerator enumerator;
            int i, j;
            int newCols;

            try
            {
                summaryList = new SortedList();
                selectedList = new SortedList();
                newCols = 0;
                
                for (i = 0; i < aSelectableList.Count; i++)
                {

                    if (((RowColProfileHeader)aSelectableList[i]).IsDisplayed)
                    {   

                        if (((RowColProfileHeader)aSelectableList[i]).Sequence == -1)
                        {
                            ((RowColProfileHeader)aSelectableList[i]).Sequence = aSelectableList.Count + newCols;
                            newCols++;
                        }

                        if (((RowColProfileHeader)aSelectableList[i]).IsSummarized)
                        {
                            summaryList.Add(((RowColProfileHeader)aSelectableList[i]).Sequence, i);
                        }
                        else
                        {
                            selectedList.Add(((RowColProfileHeader)aSelectableList[i]).Sequence, i);
                        }
                    }
                    else
                    {
                        ((RowColProfileHeader)aSelectableList[i]).Sequence = -1;
                    }
                }

                j = 0;
                enumerator = summaryList.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    ((RowColProfileHeader)aSelectableList[(int)enumerator.Value]).Sequence = j;
                    j++;
                }

                enumerator = selectedList.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    ((RowColProfileHeader)aSelectableList[(int)enumerator.Value]).Sequence = j;
                    j++;
                }

                aSortedList.Clear();

                foreach (RowColProfileHeader rowColHeader in aSelectableList)
                {
                    if (rowColHeader.IsDisplayed)
                    {
                        aSortedList.Add(rowColHeader.Sequence, rowColHeader);
                     
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void FormatCol1Grids(bool aClearGrid)
        {
            try
            {
                Formatg4Grid(aClearGrid);
                Formatg1Grid(aClearGrid);
                Formatg7Grid(aClearGrid);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void FormatCol2Grids(bool aClearGrid, int aVariableSortKey, SortEnum aSortDirection)
        {
            try
            {
                Formatg2Grid(aClearGrid, aVariableSortKey, aSortDirection);
                Formatg5Grid(aClearGrid);
                Formatg8Grid(aClearGrid);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void FormatCol3Grids(bool aClearGrid, int aVariableSortKey, SortEnum aSortDirection)
        {
            try
            {
                Formatg3Grid(aClearGrid, aVariableSortKey, aSortDirection);
                Formatg6Grid(aClearGrid);
                Formatg9Grid(aClearGrid);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        
        private void Formatg1Grid(bool aClearGrid)
        {
            int i;
            int j;
            CubeWaferCoordinateList asrtWaferCoordinateList;
            RowColProfileHeader varHeader;
           
            //AssortmentSummaryVariableProfile varProf;
            ComputationVariableProfile varProf;
            
            string varName;

            try
            {
                if (aClearGrid)
                {
                    g1.Clear();
                }

                if (g1.Tag == null)
                {
                    g1.Tag = new PagingGridTag(Grid1, g1, g4, g1, null, 0, 0, null, _selectableSummaryRowHeaders, null, _sortedSummaryRowHeaders, false, false);
                } 
                if (IsBalanceSelectedInSummary())
                {
                    g1.Rows.Count = _sortedSummaryRowHeaders.Count;
                }
                else
                {
                    g1.Rows.Count = _sortedSummaryRowHeaders.Count + 1;
                }                 

                g1.Cols.Count = g4.Cols.Count;
                g1.Rows.Fixed = 1;
                g1.Cols.Fixed = g4.Cols.Count;

                g1.AllowDragging = AllowDraggingEnum.None;
                g1.AllowMerging = AllowMergingEnum.Free;

                //initilaze here to avoid null reference exception
                _gridData[Grid1] = new ROCell[g1.Rows.Count, g1.Cols.Count]; //_sortedComponentColumnHeaders.Count];

                for (j = 0; j < g1.Cols.Count; j++)
                {
                    g1.SetData(0, j, " ");
                }

                g1.Rows[0].UserData = new RowHeaderTag(new CubeWaferCoordinateList(), null, null, 0, string.Empty, string.Empty, false);

                i = 0;
                _includeBalance = false;

                foreach (DictionaryEntry varEntry in _sortedSummaryRowHeaders)
                {
                    varHeader = (RowColProfileHeader)varEntry.Value;
                    
                    varProf = (ComputationVariableProfile)varHeader.Profile;
                    
                    // If the varProf is null, then this is the dummy variable for showing and hiding "balance" in the total area 
                    // at the bottom of the matrix screen. 
                    if (varProf.Key == (int)eAssortmentSummaryVariables.Balance)	 
                    {
                        _includeBalance = true;
                        continue;
                    }

                    if (IsAssortment || IsGroupAllocation)  
                    {
                        if (varProf.Key == ((AssortmentViewSummaryVariables)_summaryVariables).Units.Key)
                        {
                            switch (_asrtCubeGroup.AssortmentVariableType)
                            {
                                case eAssortmentVariableType.Receipts:
                                    varName = MIDText.GetTextOnly(eMIDTextCode.lbl_Receipts);
                                    break;

                                case eAssortmentVariableType.Sales:
                                    varName = MIDText.GetTextOnly(eMIDTextCode.lbl_Sales);
                                    break;

                                case eAssortmentVariableType.Stock:
                                    varName = MIDText.GetTextOnly(eMIDTextCode.lbl_Stock);
                                    break;

                                default:
                                    varName = varHeader.Name;
                                    break;
                            }
                        }
                        else
                        {
                            varName = varHeader.Name;
                        }
                    }
                    else
                    {
                        varName = varHeader.Name;
                    }

                    if (i == 0)
                    {
                        _gridData[Grid1][i, 0] = new ROCell();
                        _gridData[Grid1][i, 0].Value = varName;
                    }

                    i++;
                    asrtWaferCoordinateList = new CubeWaferCoordinateList();
                   
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(varHeader.Profile.ProfileType, varHeader.Profile.Key));

                    if (varHeader.Profile.ProfileType == eProfileType.Variable)
                    {
                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, AssortProfile.AssortmentBasisList[0].VersionProfile.Key));
                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, AssortProfile.AssortmentBasisList[0].HierarchyNodeProfile.Key));
                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, 1));
                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.TimeTotalVariable, ((VariableProfile)varHeader.Profile).TotalTimeTotalVariableProfile.Key));
                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _applicationSessionTransaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key));
                    }                    

                    g1.Rows[i].UserData = new RowHeaderTag(asrtWaferCoordinateList, null, varHeader, i, varName, varName);

                    for (j = 0; j < g1.Cols.Count; j++)
                    {
                        g1.SetData(i, j, varName);
                    }
                    _gridData[Grid1][i, 0] = new ROCell();
                    _gridData[Grid1][i, 0].Value = varHeader.Name;
                }

                for (i = 0; i < g4.Cols.Count; i++)
                {
                    g1.Cols[i].Visible = g4.Cols[i].Visible;
                }

                ((PagingGridTag)g1.Tag).Visible = true;
                ((PagingGridTag)g1.Tag).RowsPerRowGroup = 1;
                ((PagingGridTag)g1.Tag).RowGroupsPerGrid = g1.Rows.Count - g1.Rows.Fixed;
                ((PagingGridTag)g1.Tag).RowsPerScroll = 1;
                ((PagingGridTag)g1.Tag).ColsPerColGroup = 1;
                ((PagingGridTag)g1.Tag).ColGroupsPerGrid = g1.Cols.Count;
                ((PagingGridTag)g1.Tag).ColsPerScroll = 1;

                foreach (Row row in g1.Rows)
                {
                    row.AllowMerging = true;
                }

                g1.Visible = ((PagingGridTag)g1.Tag).Visible;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// CHeks to see if "Balance" is checked for summary rows.
        /// </summary>
        /// <returns></returns>
        private bool IsBalanceSelectedInSummary()
        {
            bool isSelected = false;
            RowColProfileHeader varHeader;
            ComputationVariableProfile varProf;

            foreach (DictionaryEntry varEntry in _sortedSummaryRowHeaders)
            {
                varHeader = (RowColProfileHeader)varEntry.Value;
                varProf = (ComputationVariableProfile)varHeader.Profile;
                if (varProf.Key == (int)eAssortmentSummaryVariables.Balance)
                {
                    isSelected = true;
                    continue;
                }
            }

            return isSelected;
        }
        private void Formatg2Grid(bool aClearGrid, int aVariableSortKey, SortEnum aSortDirection)
        {
            int i;
            int j;
            CubeWaferCoordinateList asrtWaferCoordinateList;
            RowColProfileHeader varHeader;
            RowColProfileHeader totalVarHeader;
            AssortmentTotalVariableProfile totVarProf;

            try
            {
                if (aClearGrid)
                {
                    g2.Clear();
                }

                if (g2.Tag == null)
                {
                    g2.Tag = new PagingGridTag(Grid2, g2, g1, g2, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE, _selectableTotalColumnHeaders, null, _sortedTotalColumnHeaders, null, false, false);
                }

                totVarProf = (AssortmentTotalVariableProfile)_totalColumnProfileList.FindKey((int)eAssortmentTotalVariables.TotalUnits);
                totalVarHeader = new RowColProfileHeader(totVarProf.VariableName, true, 0, totVarProf);

                g2.Rows.Count = g1.Rows.Count;
                g2.Cols.Count = _sortedTotalColumnHeaders.Count;
                g2.Rows.Fixed = g1.Rows.Fixed;
                g2.Cols.Fixed = 0;

                g2.AllowDragging = AllowDraggingEnum.None;
                g2.AllowMerging = AllowMergingEnum.RestrictCols;

                for (j = 0; j < g2.Cols.Count; j++)
                {
                    g2.SetData(0, j, " ");
                }

                i = -1;

                //initilaze here to avoid null reference exception
                _gridData[Grid2] = new ROCell[g2.Rows.Count, g2.Cols.Count];

                foreach (DictionaryEntry varEntry in _sortedTotalColumnHeaders)
                {
                    varHeader = (RowColProfileHeader)varEntry.Value;

                    i++;
                    asrtWaferCoordinateList = new CubeWaferCoordinateList();
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroup, _asrtCubeGroup.CurrentStoreGroupProfile.Key));
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentTotalVariable, totalVarHeader.Profile.Key));
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));

                    g2.Cols[i].UserData = new ColumnHeaderTag(asrtWaferCoordinateList, new RowColProfileHeader(_asrtCubeGroup.CurrentStoreGroupProfile.Name, true, 0, _asrtCubeGroup.CurrentStoreGroupProfile), totalVarHeader, i, _asrtCubeGroup.CurrentStoreGroupProfile.Name + "|" + varHeader.Name);
                    g2.SetData(0, i, "Total");
                    if (i > 0)
                    {
                        _gridData[Grid2][0, i] = new ROCell();
                        _gridData[Grid2][0, i].Value = string.Empty;
                    }
                    else
                    {
                        _gridData[Grid2][0, i] = new ROCell();
                        _gridData[Grid2][0, i].Value = "Total";
                    }
                }

                ((PagingGridTag)g2.Tag).Visible = true;
                ((PagingGridTag)g2.Tag).RowsPerRowGroup = 1;
                ((PagingGridTag)g2.Tag).RowGroupsPerGrid = g2.Rows.Count - g2.Rows.Fixed;
                ((PagingGridTag)g2.Tag).RowsPerScroll = 1;
                ((PagingGridTag)g2.Tag).ColsPerColGroup = 1;
                ((PagingGridTag)g2.Tag).ColGroupsPerGrid = g2.Cols.Count;
                ((PagingGridTag)g2.Tag).ColsPerScroll = 1;

                for (i = 0; i < g2.Rows.Fixed; i++)
                {
                    g2.Rows[i].AllowMerging = true;
                }
               
                g2.Visible = ((PagingGridTag)g2.Tag).Visible;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void Formatg3Grid(bool aClearGrid, int aVariableSortKey, SortEnum aSortDirection)
        {
            int i;
            CubeWaferCoordinateList asrtWaferCoordinateList;
            RowColProfileHeader gradeHeader;
            RowColProfileHeader groupLevelHeader;
            RowColProfileHeader varHeader;
            RowColProfileHeader totalVarHeader;
            AssortmentDetailVariableProfile totVarProf;

            try
            {
                if (aClearGrid)
                {
                    g3.Clear();
                }

                if (g3.Tag == null)
                {
                    g3.Tag = new PagingGridTag(Grid3, g3, g1, g3, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE, _selectableStoreGradeHeaders, null, _sortedStoreGradeHeaders, null, false, true);
                }

                totVarProf = (AssortmentDetailVariableProfile)_detailColumnProfileList.FindKey((int)eAssortmentDetailVariables.TotalUnits);
                totalVarHeader = new RowColProfileHeader(totVarProf.VariableName, true, 0, totVarProf);

                if (_columnGroupedBy == eAllocationAssortmentViewGroupBy.StoreGrade)
                {
                    g3.Rows.Count = g1.Rows.Count;
                    g3.Cols.Count = (_sortedStoreGradeHeaders.Count + 1) * _sortedDetailColumnHeaders.Count;
                    g3.Rows.Fixed = g1.Rows.Fixed;
                    g3.Cols.Fixed = 0;

                    g3.AllowDragging = AllowDraggingEnum.None;
                    g2.AllowMerging = AllowMergingEnum.RestrictRows;

                    i = -1;
                    //Initialize to avoid null reference exception.
                    _gridData[Grid3] = new ROCell[g3.Rows.Count, g3.Cols.Count];

                    foreach (DictionaryEntry varEntry in _sortedDetailColumnHeaders)
                    {
                        varHeader = (RowColProfileHeader)varEntry.Value;

                        i++;
                        asrtWaferCoordinateList = new CubeWaferCoordinateList();
                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroup, _asrtCubeGroup.CurrentStoreGroupProfile.Key));
                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, _currStoreGroupLevelProfile.Key));
                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentDetailVariable, totalVarHeader.Profile.Key));
                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));

                        g3.Cols[i].UserData = new ColumnHeaderTag(asrtWaferCoordinateList, new RowColProfileHeader(_currStoreGroupLevelProfile.Name, true, 0, _currStoreGroupLevelProfile), totalVarHeader, i, _currStoreGroupLevelProfile.Name + "|" + varHeader.Name);
                        g3.SetData(0, i, _currStoreGroupLevelProfile.Name);
                        if (i == 0)
                        {
                            _gridData[Grid3][0, i] = new ROCell();
                            _gridData[Grid3][0, i].Value = _currStoreGroupLevelProfile.Name;
                        }
                        else
                        {
                            _gridData[Grid3][0, i] = new ROCell();
                            _gridData[Grid3][0, i].Value = string.Empty;
                        }
                    }

                    bool bNewGrade = false;
                    
                    foreach (DictionaryEntry gradeEntry in _sortedStoreGradeHeaders)
                    {
                        bNewGrade = true;
                        int counter = -1;
                        gradeHeader = (RowColProfileHeader)gradeEntry.Value;

                        foreach (DictionaryEntry varEntry in _sortedDetailColumnHeaders)
                        {
                            varHeader = (RowColProfileHeader)varEntry.Value;

                            i++;
                            counter++;
                            asrtWaferCoordinateList = new CubeWaferCoordinateList();
                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroup, _asrtCubeGroup.CurrentStoreGroupProfile.Key));
                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, _currStoreGroupLevelProfile.Key));
                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGrade, gradeHeader.Profile.Key));
                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentDetailVariable, totalVarHeader.Profile.Key));
                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));

                            g3.Cols[i].UserData = new ColumnHeaderTag(asrtWaferCoordinateList, gradeHeader, totalVarHeader, i, gradeHeader.Name + "|" + varHeader.Name);
                            g3.SetData(0, i, gradeHeader.Name);
                            if (bNewGrade && counter == 0)
                            {
                                _gridData[Grid3][0, i] = new ROCell();
                                _gridData[Grid3][0, i].Value = gradeHeader.Name;
                                bNewGrade = false;
                            }
                            else
                            {
                                _gridData[Grid3][0, i] = new ROCell();
                                _gridData[Grid3][0, i].Value = string.Empty;
                            }
                        }
                    }
                }
                else
                {
                    g3.Rows.Count = g1.Rows.Count;
                    g3.Cols.Count = _storeGroupLevelProfileList.Count * _sortedDetailColumnHeaders.Count;
                    g3.Rows.Fixed = g1.Rows.Fixed;
                    g3.Cols.Fixed = 0;

                    g3.AllowDragging = AllowDraggingEnum.None;
                    g2.AllowMerging = AllowMergingEnum.RestrictRows;

                    i = -1;
                    _gridData[Grid3] = new ROCell[g3.Rows.Count, g3.Cols.Count];

                    foreach (StoreGroupLevelProfile groupLevelProf in _storeGroupLevelProfileList)
                    {
                        groupLevelHeader = new RowColProfileHeader(groupLevelProf.Name, true, 0, groupLevelProf);

                        foreach (DictionaryEntry varEntry in _sortedDetailColumnHeaders)
                        {
                            varHeader = (RowColProfileHeader)varEntry.Value;

                            i++;
                            asrtWaferCoordinateList = new CubeWaferCoordinateList();
                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroup, _asrtCubeGroup.CurrentStoreGroupProfile.Key));
                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupLevelHeader.Profile.Key));
                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentDetailVariable, totalVarHeader.Profile.Key));
                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));

                            g3.Cols[i].UserData = new ColumnHeaderTag(asrtWaferCoordinateList, groupLevelHeader, totalVarHeader, i, groupLevelHeader.Name + "|" + varHeader.Name);
                            g3.SetData(0, i, groupLevelHeader.Name);
                            _gridData[Grid3][0, i] = new ROCell();
                            _gridData[Grid3][0, i].Value = groupLevelHeader.Name;
                        }
                    }
                }
                    
                ((PagingGridTag)g3.Tag).Visible = true;
                ((PagingGridTag)g3.Tag).RowsPerRowGroup = 1;
                ((PagingGridTag)g3.Tag).RowGroupsPerGrid = g3.Rows.Count - g3.Rows.Fixed;
                ((PagingGridTag)g3.Tag).RowsPerScroll = 1;
                ((PagingGridTag)g3.Tag).ColsPerColGroup = _sortedDetailColumnHeaders.Count;
                ((PagingGridTag)g3.Tag).ColGroupsPerGrid = _sortedStoreGradeHeaders.Count + 1;
                ((PagingGridTag)g3.Tag).ColsPerScroll = 1;

                for (i = 0; i < g3.Rows.Fixed; i++)
                {
                    g3.Rows[i].AllowMerging = true;
                }

                g3.Visible = ((PagingGridTag)g3.Tag).Visible;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void Formatg4Grid(bool aClearGrid)
        {
            int i;
            int j;
            int treeCols;
            int summCols;
            CubeWaferCoordinateList asrtWaferCoordinateList;
            RowColProfileHeader varHeader;
            AssortmentComponentVariableProfile varProf;
            string firstSumCol = string.Empty;
            int RIDValue;
            int headerRID;
            string varStr;
            string summTitle;
            string tabStr;
            string[] colArray;
            string[] textColArray;
            string[] keyColArray;
            DataTable dtGrid;
            DataView dvHeaders;
            SortedList gridRowList;
            GridRow gridRow;


            lock (gridLock)
            {
                try
                {
                    int seqColumns = 4;
                    if (aClearGrid)
                    {
                        g4.Clear();
                    }

                    g4.Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Clear);

                    if (g4.Tag == null)
                    {
                        g4.Tag = new PagingGridTag(Grid4, g4, g4, g4, null, 0, 0, _selectableComponentColumnHeaders, null, _sortedComponentColumnHeaders, null, false, false);
                    }

                    i = 0;
                    _headerCol = int.MaxValue;
                    _placeholderCol = int.MaxValue;
                    _packCol = int.MaxValue;

                    _highestPlaceholderHeaderCol = int.MaxValue;
                    _containsBothPlaceholderAndHeader = false;
                    _noPlaceholderOrHeaderSelected = false;

                    foreach (DictionaryEntry varEntry in _sortedComponentColumnHeaders)
                    {
                        varHeader = (RowColProfileHeader)varEntry.Value;
                        varProf = (AssortmentComponentVariableProfile)varHeader.Profile;

                        switch (varProf.Key)
                        {
                            case (int)eAssortmentComponentVariables.HeaderID:
                                _headerCol = i;

                                // Note: Highest col in the tree has the lowest index
                                if (_headerCol > -1 && _headerCol != int.MaxValue)
                                {
                                    if (_headerCol < _highestPlaceholderHeaderCol)
                                    {
                                        _highestPlaceholderHeaderCol = _headerCol;
                                    }
                                }

                                if (varHeader.IsSummarized)
                                {
                                    //_isHeaderSummarized = true;
                                }

                                break;

                            case (int)eAssortmentComponentVariables.Placeholder:
                                _placeholderCol = i;

                                if (_placeholderCol > -1 && _placeholderCol != int.MaxValue)
                                {
                                    if (_placeholderCol < _highestPlaceholderHeaderCol)
                                    {
                                        _highestPlaceholderHeaderCol = _placeholderCol;
                                    }
                                }

                                break;

                            case (int)eAssortmentComponentVariables.Pack:
                                _packCol = i;
                                break;
                            case (int)eAssortmentComponentVariables.Color:
                                seqColumns = 5;
                                break;
                        }

                        i++;
                    }

                    if ((_headerCol > -1 && _headerCol != int.MaxValue) &&
                        (_placeholderCol > -1 && _placeholderCol != int.MaxValue))
                    {
                        _containsBothPlaceholderAndHeader = true;
                    }
                    else if (_headerCol == int.MaxValue && _placeholderCol == int.MaxValue)
                    {
                        _noPlaceholderOrHeaderSelected = true;
                    }

                ((AssortmentComponentVariableProfile)_componentVariables.VariableProfileList.FindKey((int)eAssortmentComponentVariables.Pack)).UseAlternateTextColumnName =
                    (_packCol < _headerCol && _packCol < _placeholderCol);

                    i = 0;
                    varStr = "";
                    summTitle = "";

                    colArray = new string[(_sortedComponentColumnHeaders.Count * 2) + seqColumns];
                    textColArray = new string[_sortedComponentColumnHeaders.Count];
                    keyColArray = new string[_sortedComponentColumnHeaders.Count];
                    treeCols = 0;
                    summCols = 0;
                    bool hasPlaceholder = false;
                    bool hasHeader = false;
                    bool hasColor = false;

                    foreach (DictionaryEntry varEntry in _sortedComponentColumnHeaders)
                    {
                        varHeader = (RowColProfileHeader)varEntry.Value;
                        varProf = (AssortmentComponentVariableProfile)varHeader.Profile;

                        if (varHeader.IsSummarized)
                        {
                            {
                                treeCols = 1;
                                summCols++;

                                tabStr = "";

                                for (j = 0; j < summCols; j++)
                                {
                                    if (j == 0)
                                    {
                                        tabStr += "     ";
                                    }
                                    else
                                    {
                                        tabStr += "    ";
                                    }
                                }

                                if (summTitle.Length != 0)
                                {
                                    summTitle += System.Environment.NewLine;
                                }

                                summTitle += tabStr + varHeader.Name;
                            }
                        }

                        if (varStr != "")
                        {
                            varStr += ", ";
                        }

                        varStr += varProf.DisplayTextColumnName;

                        textColArray[i / 2] = varProf.DisplayTextColumnName;
                        keyColArray[i / 2] = varProf.RIDColumnName;

                        colArray[i] = varProf.DisplayTextColumnName;
                        i++;
                        colArray[i] = varProf.RIDColumnName;
                        i++;
                        if (varProf.ProfileListType == eProfileType.PlaceholderHeader)
                        {
                            hasPlaceholder = true;
                        }
                        else if (varProf.ProfileListType == eProfileType.AllocationHeader)
                        {
                            hasHeader = true;
                        }

                        if (varProf.ProfileListType == eProfileType.HeaderPackColor)
                        {
                            hasColor = true;
                        }

                    }

                    // Add these additianl fields to the GridRow.DataRow                
                    //if (hasPlaceholder)					
                    {
                        colArray[i] = "PLACEHOLDERSEQ";
                        i++;
                        colArray[i] = "PLACEHOLDERSEQ_RID";
                        i++;
                    }
                    //if (hasHeader)
                    {
                        colArray[i] = "HEADERSEQ";
                        i++;
                        colArray[i] = "HEADERSEQ_RID";
                        i++;
                    }

                    if (hasColor)
                    {
                        colArray[i] = "COLORSEQ";
                    }

                    i++;

                    dvHeaders = _dtHeaders.DefaultView;

                    if (_headerCol == int.MaxValue)
                    {
                        if (IsGroupAllocation)
                        {
                            dvHeaders.RowFilter = "ASSORTMENT_IND = " + bool.FalseString;
                        }
                        else
                        {
                            dvHeaders.RowFilter = "ASSORTMENT_IND = " + bool.TrueString;
                        }
                    }
                    else if (_placeholderCol == int.MaxValue)
                    {
                        dvHeaders.RowFilter = "ASSORTMENT_IND = " + bool.FalseString;
                    }
                    else
                    {
                        dvHeaders.RowFilter = "";
                    }

                    dtGrid = dvHeaders.ToTable("Grid Table", true, colArray);

                    //===============================================================================
                    // No header. No placeholder
                    // If the view doesn't contain header ID or Placeholder ID AND
                    // the headers have the same color(s), duplicate "Bulk" rows are created.
                    // RemoveDuplicateBulkRows() attepts to remove any duplicate "Bulk" rows.
                    //===============================================================================
                    if (!hasHeader && !hasPlaceholder)
                    {
                        dtGrid = RemoveDuplicateBulkRows(dtGrid, keyColArray);
                    }

                    g4.Rows.Count = dtGrid.Rows.Count + 1;

                    // If hasCOlor, then ColorSeq is on dtGrid.
                    if (hasColor)
                    {
                        g4.Cols.Count = dtGrid.Columns.Count - 5 + treeCols;
                    }
                    else
                    {
                        g4.Cols.Count = dtGrid.Columns.Count - 4 + treeCols;
                    }

                    g4.Rows.Fixed = 1;
                    g4.Cols.Fixed = treeCols;

                    _gridData[Grid4] = new ROCell[g4.Rows.Count, _sortedComponentColumnHeaders.Count];

                    g4.GetMergeData = new GetMergeData(g4GetMergeData);

                    if (treeCols == 1)
                    {
                        g4.Cols[0].Name = " ";
                    }

                    gridRowList = new SortedList();

                    foreach (DataRow dtRow in dtGrid.Rows)
                    {
                        gridRowList.Add(new GridRow(dtRow, textColArray, keyColArray), null);
                    }

                    for (i = 0; i < textColArray.Length; i++)
                    {
                        g4.Cols[(i * 2) + treeCols].Name = textColArray[i];
                        g4.Cols[(i * 2) + treeCols + 1].Name = keyColArray[i];
                    }

                    i = g4.Rows.Fixed;

                    IDictionaryEnumerator dictEnum = gridRowList.GetEnumerator();

                    while (dictEnum.MoveNext())
                    {
                        gridRow = (GridRow)dictEnum.Key;

                        for (j = 0; j < gridRow.TextArray.Length; j++)
                        {
                            g4.SetData(i, (j * 2) + treeCols, gridRow.TextArray[j]);
                            _gridData[Grid4][i, j] = new ROCell();
                            _gridData[Grid4][i, j].Value = gridRow.TextArray[j];
                            g4.SetData(i, (j * 2) + treeCols + 1, gridRow.KeyArray[j]);
                        }

                        i++;
                    }

                    if (treeCols == 1)
                    {
                        g4.SetData(0, 0, summTitle);
                        //_gridData[Grid4][0, 0] = new ROCell();
                        //_gridData[Grid4][0, 0].Value = summTitle;
                        g4.Tree.Style = C1.Win.C1FlexGrid.TreeStyleFlags.Simple;
                        g4.Tree.Column = 0;
                    }

                    g4.AllowDragging = AllowDraggingEnum.None;
                    g4.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictCols;

                    foreach (DictionaryEntry varEntry in _sortedComponentColumnHeaders)
                    {
                        varHeader = (RowColProfileHeader)varEntry.Value;
                        varProf = (AssortmentComponentVariableProfile)varHeader.Profile;

                        g4.Cols[varProf.RIDColumnName].Visible = false;

                        if (varHeader.IsSummarized)
                        {
                            g4.SetData(0, varProf.DisplayTextColumnName, " ");
                            g4.Cols[varProf.DisplayTextColumnName].Visible = false;
                            _gridData[Grid4][0, varHeader.Sequence] = new ROCell();
                            _gridData[Grid4][0, varHeader.Sequence].Value = varHeader.Name;
                        }
                        else
                        {
                            g4.SetData(0, varProf.DisplayTextColumnName, varHeader.Name);
                            _gridData[Grid4][0, varHeader.Sequence] = new ROCell();
                            _gridData[Grid4][0, varHeader.Sequence].Value = varHeader.Name;
                        }
                    }

                    if (treeCols == 1)
                    {
                        i = 1;
                        foreach (DictionaryEntry varEntry in _sortedComponentColumnHeaders)
                        {
                            varHeader = (RowColProfileHeader)varEntry.Value;
                            varProf = (AssortmentComponentVariableProfile)varHeader.Profile;

                            if (((RowColProfileHeader)varEntry.Value).IsSummarized)
                            {
                                if (i == 1)
                                {
                                    firstSumCol = varProf.DisplayTextColumnName;
                                }
                                g4.Subtotal(C1.Win.C1FlexGrid.AggregateEnum.None, i, firstSumCol, varProf.DisplayTextColumnName, varProf.DisplayTextColumnName, "{0}");
                                i++;
                            }
                            else
                            {
                                varHeader = (RowColProfileHeader)varEntry.Value;

                                asrtWaferCoordinateList = new CubeWaferCoordinateList();
                                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentComponentVariable, varHeader.Profile.Key));

                                g4.Cols[varProf.DisplayTextColumnName].UserData = new ColumnHeaderTag(asrtWaferCoordinateList, null, varHeader, g4.Cols[varProf.DisplayTextColumnName].Index, varHeader.Name);
                            }
                        }
                    }

                    foreach (C1.Win.C1FlexGrid.Row row in g4.Rows)
                    {
                        if (row.Index < g4.Rows.Fixed)
                        {
                            row.UserData = new RowHeaderTag(new CubeWaferCoordinateList(), null, null, row.Index, string.Empty, string.Empty, false);
                        }
                        else if (row.IsNode)
                        {
                            asrtWaferCoordinateList = new CubeWaferCoordinateList();
                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentSubTotal, row.Node.Level));

                            headerRID = -1;

                            for (i = 0; i <= row.Node.Level; i++)
                            {
                                varHeader = (RowColProfileHeader)_sortedComponentColumnHeaders.GetByIndex(i);
                                varProf = (AssortmentComponentVariableProfile)varHeader.Profile;
                                RIDValue = Convert.ToInt32(GetFirstDetailRow(g4, row.Node)[varProf.RIDColumnName]);

                                if (i == row.Node.Level)
                                {
                                    RIDValue = int.MaxValue;
                                }
                                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(varProf.ProfileListType, RIDValue));

                                if (varProf.HeaderComponent)
                                {
                                    headerRID = RIDValue;
                                }
                            }

                            if (_containsBothPlaceholderAndHeader && row.Node.Level < _highestPlaceholderHeaderCol + 1)
                            {

                                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.TotalVariableProfile.Key));
                                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));

                            }
                            else if (_containsBothPlaceholderAndHeader && (_placeholderCol + 1) == _headerCol)
                            {

                                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.TotalVariableProfile.Key));
                                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));

                            }
                            // Level is above header 
                            else if (row.Node.Level < _headerCol + 1 && !_containsBothPlaceholderAndHeader)
                            {
                                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));
                                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlaceholderHeader, 0));
                            }
                            else if (row.Node.Level < _placeholderCol + 1 && !_containsBothPlaceholderAndHeader)
                            {
                                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));
                                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));
                            }
                            else
                            {
                                if (headerRID == int.MaxValue)
                                {
                                    if (row.Caption == MIDText.GetTextOnly(eMIDTextCode.msg_as_PlaceholderBalance))
                                    {
                                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.DifferenceVariableProfile.Key));
                                    }
                                    else
                                    {
                                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));
                                    }
                                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));
                                }
                                else
                                {
                                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));

                                    if (headerRID == -1)
                                    {
                                        if (GetAssortmentType() == eAssortmentType.PostReceipt || GetAssortmentType() == eAssortmentType.GroupAllocation)
                                        {
                                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AllocationHeader, 0));
                                        }
                                        else
                                        {
                                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));
                                        }
                                    }
                                }
                            }

                            row.UserData = new RowHeaderTag(asrtWaferCoordinateList, null, null, row.Index, string.Empty, string.Empty);
                        }
                        else
                        {
                            asrtWaferCoordinateList = new CubeWaferCoordinateList();
                            headerRID = -1;

                            foreach (DictionaryEntry varEntry in _sortedComponentColumnHeaders)
                            {
                                varHeader = (RowColProfileHeader)varEntry.Value;
                                varProf = (AssortmentComponentVariableProfile)varHeader.Profile;
                                RIDValue = Convert.ToInt32(row[varProf.RIDColumnName]);
                                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(varProf.ProfileListType, RIDValue));

                                if (varProf.HeaderComponent)
                                {
                                    headerRID = RIDValue;
                                }
                            }

                            if (headerRID == int.MaxValue)
                            {
                                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));

                                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));
                            }
                            else
                            {
                                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));

                                if (headerRID == -1)
                                {
                                    if (GetAssortmentType() == eAssortmentType.PostReceipt || GetAssortmentType() == eAssortmentType.GroupAllocation)
                                    {
                                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AllocationHeader, 0));
                                    }
                                    else
                                    {
                                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));
                                    }
                                }
                            }

                            row.UserData = new RowHeaderTag(asrtWaferCoordinateList, null, null, row.Index, string.Empty, string.Empty);
                        }
                    }

                    g4.Rows[0].AllowMerging = true;

                    for (i = treeCols; i < g4.Cols.Count; i++)
                    {
                        g4.Cols[i].AllowMerging = true;
                    }

                    //g4.AutoSizeCols();

                    ((PagingGridTag)g4.Tag).Visible = true;
                    ((PagingGridTag)g4.Tag).RowsPerRowGroup = 1;
                    ((PagingGridTag)g4.Tag).RowGroupsPerGrid = g4.Rows.Count - g4.Rows.Fixed;
                    ((PagingGridTag)g4.Tag).RowsPerScroll = 1;
                    ((PagingGridTag)g4.Tag).ColsPerColGroup = 1;
                    ((PagingGridTag)g4.Tag).ColGroupsPerGrid = g4.Cols.Count;
                    ((PagingGridTag)g4.Tag).ColsPerScroll = 1;

                    g4.Visible = true;

                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }

        
        /// <summary>
        /// If the view doesn't contain header ID or Placeholder ID AND
        /// the headers have the same color(s), duplicate "Bulk" rows are created.
        /// RemoveDuplicateBulkRows() attepts to remove any duplicate "Bulk" rows.
        /// </summary>
        /// <param name="dtGrid"></param>
        /// <returns></returns>        
        private static DataTable RemoveDuplicateBulkRows(DataTable dtGrid, string[] keyColArray)        
        {
            DataTable dtTempGrid = dtGrid.Clone();
            List<int> bulkColors = new List<int>();
            bool includesPackAlternate = false;		
            List<string> lstKeys = new List<string>();
            string sKeys;

            foreach (DataRow aRow in dtGrid.Rows)
            {
                sKeys = null;
                foreach (string key in keyColArray)
                {
                    sKeys += Convert.ToString(aRow[key]);
                }
                // If Pack not selected, no special logic needed
                
                if (aRow.Table.Columns.Contains("PACK_ALTERNATE"))
                {
                    includesPackAlternate = true;                   
                }
               
                // If Color not selected, no special logic needed
                if (!aRow.Table.Columns.Contains("COLOR_RID"))
                {
                    dtTempGrid = dtGrid;    
                    break;
                }
                int colorRid = -1;
                if (aRow["COLOR_RID"] != DBNull.Value)
                {
                    colorRid = int.Parse(aRow["COLOR_RID"].ToString());
                }

                if (includesPackAlternate)
                {
                    // If we find a "Bulk" and the color is in the list, skip it.
                    if (aRow["PACK_ALTERNATE"].ToString() == "Bulk" && bulkColors.Contains(colorRid))
                    {
                        continue;
                    }

                    if (aRow["PACK_ALTERNATE"].ToString() == "Bulk")
                    {
                        bulkColors.Add(colorRid);
                    }
                }
                else
                {
                    // If we find a "Bulk" and the color is in the list, skip it.                   
                    if (bulkColors.Contains(colorRid)
                        && lstKeys.Contains(sKeys))
                    {
                        continue;
                    }
                    if (!bulkColors.Contains(colorRid))
                    {
                        bulkColors.Add(colorRid);
                    }
                    if (!lstKeys.Contains(sKeys))
                    {
                        lstKeys.Add(sKeys);
                    }
                    
                }
                dtTempGrid.Rows.Add(aRow.ItemArray);
            }

            return dtTempGrid;
        }
        
        /// <summary>
        /// if a PH has colors, but which are not displayed, this causes dupilcate placeholder rows to appear.
        /// This method removes those rows.
        /// </summary>
        /// <param name="dtGrid"></param>
        /// <returns></returns>
        private static DataTable RemoveDuplicatePlaceholderRows(DataTable dtGrid)
        {
            DataTable dtTempGrid = dtGrid.Clone();
            List<int> placeholderSeqList = new List<int>();

            foreach (DataRow aRow in dtGrid.Rows)
            {
                int phSeq = -1;
                if (aRow["PLACEHOLDERSEQ"] != DBNull.Value)
                {
                    phSeq = int.Parse(aRow["PLACEHOLDERSEQ"].ToString());
                }

                if (placeholderSeqList.Contains(phSeq))
                {
                    continue;
                }

                placeholderSeqList.Add(phSeq);

                dtTempGrid.Rows.Add(aRow.ItemArray);
            }

            return dtTempGrid;
        }        
        private static DataTable RemoveDuplicatePlaceholderColorRows(DataTable dtGrid)
        {
            DataTable dtTempGrid = dtGrid.Clone();
            List<int> placeholderSeqList = new List<int>();
            List<int> placeholderClrSeqList = new List<int>();
            int oldPhSeq = -1;
            foreach (DataRow aRow in dtGrid.Rows)
            {
                int phSeq = -1;
                if (aRow["PLACEHOLDERSEQ"] != DBNull.Value)
                {
                    phSeq = int.Parse(aRow["PLACEHOLDERSEQ"].ToString());
                }

                if (oldPhSeq == -1 || oldPhSeq != phSeq)
                {
                    placeholderClrSeqList.Clear();
                    oldPhSeq = phSeq;
                }

                int clrRid = -1;
                if (aRow["COLOR_RID"] != DBNull.Value || Convert.ToInt32(aRow["COLOR_RID"]) != int.MaxValue)
                {
                    clrRid = int.Parse(aRow["COLOR_RID"].ToString());
                }

                if (placeholderClrSeqList.Contains(clrRid))
                {
                    continue;
                }
                placeholderClrSeqList.Add(clrRid);


                dtTempGrid.Rows.Add(aRow.ItemArray);
            }

            return dtTempGrid;
        }
        private static DataTable RemoveDuplicatePlaceholderHeaderColorRows(DataTable dtGrid)
        {
            DataTable dtTempGrid = dtGrid.Clone();
            List<int> placeholderSeqList = new List<int>();
            List<int> placeholderClrSeqList = new List<int>();
            List<int> HdrSeqList = new List<int>();

            Dictionary<HashSet<int>, string> keyHashList = new Dictionary<HashSet<int>, string>(HashSet<int>.CreateSetComparer());



            foreach (DataRow aRow in dtGrid.Rows)
            {
                HashSet<int> keyHash = new HashSet<int>();

                int phSeq = -1;
                int hdrSeq = -1;
                int colorSeq = -1;

                if (aRow["PLACEHOLDERSEQ"] != DBNull.Value)
                {
                    phSeq = int.Parse(aRow["PLACEHOLDERSEQ"].ToString());
                }

                if (aRow["HEADER_RID"] != DBNull.Value)
                {
                    hdrSeq = int.Parse(aRow["HEADER_RID"].ToString());
                }

                if (aRow["COLORSEQ"] != DBNull.Value)
                {
                    colorSeq = int.Parse(aRow["COLORSEQ"].ToString());
                }

                keyHash.Add(phSeq);
                keyHash.Add(hdrSeq);
                keyHash.Add(colorSeq);

                if (keyHashList.ContainsKey(keyHash))
                {
                    continue;
                }

                keyHashList.Add(keyHash, null);
                dtTempGrid.Rows.Add(aRow.ItemArray);
            }

            return dtTempGrid;
        }
        
        ///// <summary>
        ///// debugs the values of the cells in the grid so you can see them.
        ///// </summary>
        ///// <param name="aGrid"></param>
        //private void DebugGridValues(MIDFlexGrid aGrid)
        //{
        //    string gVal = "";
        //    string line;
        //    for (int r = 1; r < aGrid.Rows.Count; r++)
        //    {
        //        line = string.Empty;
        //        for (int c = 1; c < aGrid.Cols.Count; c++)
        //        {
        //            if (aGrid[r, c] == null)
        //            {
        //                gVal = "null";
        //            }
        //            else
        //            {
        //                gVal = aGrid[r, c].ToString();
        //            }
        //            line += "(" + r + "," + c + ") " + gVal + "\t";
        //        }
        //        Debug.WriteLine(line);
        //    }
        //}
        
        private Row GetFirstDetailRow(C1FlexGrid aGrid, Node aNode)
        {
            try
            {
                if (aNode.Children == 0)
                {
                    return aGrid.Rows[aNode.GetCellRange().TopRow + 1];
                }
                else
                {
                    return GetFirstDetailRow(aGrid, aNode.GetNode(NodeTypeEnum.FirstChild));
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Formatg5Grid(bool aClearGrid)
        {
            int i;
            CubeWaferCoordinateList asrtWaferCoordinateList;
            RowColProfileHeader varHeader;

            try
            {
                if (aClearGrid)
                {
                    g5.Clear();
                }

                if (g5.Tag == null)
                {
                    g5.Tag = new PagingGridTag(Grid5, g5, g4, g5, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE, _selectableTotalColumnHeaders, null, _sortedTotalColumnHeaders, null, false, false);
                }

                g5.Rows.Count = g4.Rows.Count;
                g5.Cols.Count = g2.Cols.Count;
                g5.Rows.Fixed = g4.Rows.Fixed;
                g5.Cols.Fixed = g2.Cols.Fixed;

                g5.AllowDragging = AllowDraggingEnum.None;

                i = -1;
                //Initialize here to avoid null reference exceptoin
                _gridData[Grid5] = new ROCell[g5.Rows.Count, g5.Cols.Count];

                foreach (DictionaryEntry varEntry in _sortedTotalColumnHeaders)
                {
                    varHeader = (RowColProfileHeader)varEntry.Value;

                    i++;
                    asrtWaferCoordinateList = new CubeWaferCoordinateList();
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentTotalVariable, varHeader.Profile.Key));
                    g5.Cols[i].UserData = new ColumnHeaderTag(asrtWaferCoordinateList, null, varHeader, i, ((AssortmentTotalVariableProfile)varHeader.Profile).VariableName);
                    g5.SetData(0, i, ((AssortmentTotalVariableProfile)varHeader.Profile).VariableName);
                    _gridData[Grid5][0, i] = new ROCell();
                    _gridData[Grid5][0, i].Value = ((AssortmentTotalVariableProfile)varHeader.Profile).VariableName;
                }

                ((PagingGridTag)g5.Tag).Visible = true;
                ((PagingGridTag)g5.Tag).RowsPerRowGroup = 1;
                ((PagingGridTag)g5.Tag).RowGroupsPerGrid = g5.Rows.Count - g5.Rows.Fixed;
                ((PagingGridTag)g5.Tag).RowsPerScroll = 1;
                ((PagingGridTag)g5.Tag).ColsPerColGroup = 1;
                ((PagingGridTag)g5.Tag).ColGroupsPerGrid = g5.Cols.Count;
                ((PagingGridTag)g5.Tag).ColsPerScroll = 1;

                g5.Visible = ((PagingGridTag)g5.Tag).Visible;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void Formatg6Grid(bool aClearGrid)
        {
            int i;
            CubeWaferCoordinateList asrtWaferCoordinateList;
            RowColProfileHeader gradeHeader;
            RowColProfileHeader groupLevelHeader;
            RowColProfileHeader varHeader;

            try
            {
                if (aClearGrid)
                {
                    g6.Clear();
                }

                if (g6.Tag == null)
                {
                    g6.Tag = new PagingGridTag(Grid6, g6, g4, g6, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE, _selectableDetailColumnHeaders, null, _sortedDetailColumnHeaders, null, false, true);
                }

                if (_columnGroupedBy == eAllocationAssortmentViewGroupBy.StoreGrade)
                {
                    g6.Rows.Count = g4.Rows.Count;
                    g6.Cols.Count = g3.Cols.Count;
                    g6.Rows.Fixed = g4.Rows.Fixed;
                    g6.Cols.Fixed = g3.Cols.Fixed;

                    g6.AllowDragging = AllowDraggingEnum.None;

                    i = -1;
                    //Initialize here to avoid null reference exception
                    _gridData[Grid6] = new ROCell[g6.Rows.Count, g6.Cols.Count];

                    foreach (DictionaryEntry varEntry in _sortedDetailColumnHeaders)
                    {
                        varHeader = (RowColProfileHeader)varEntry.Value;

                        i++;
                        asrtWaferCoordinateList = new CubeWaferCoordinateList();
                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroup, _asrtCubeGroup.CurrentStoreGroupProfile.Key));
                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, _currStoreGroupLevelProfile.Key));
                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentDetailVariable, varHeader.Profile.Key));

                        g6.Cols[i].UserData = new ColumnHeaderTag(asrtWaferCoordinateList, new RowColProfileHeader(_currStoreGroupLevelProfile.Name, true, 0, _currStoreGroupLevelProfile), varHeader, i, varHeader.Name);
                        g6.SetData(0, i, varHeader.Name);
                        _gridData[Grid6][0, i] = new ROCell();
                        _gridData[Grid6][0, i].Value = varHeader.Name;
                    }

                    foreach (DictionaryEntry gradeEntry in _sortedStoreGradeHeaders)
                    {
                        gradeHeader = (RowColProfileHeader)gradeEntry.Value;

                        foreach (DictionaryEntry varEntry in _sortedDetailColumnHeaders)
                        {
                            varHeader = (RowColProfileHeader)varEntry.Value;

                            i++;
                            asrtWaferCoordinateList = new CubeWaferCoordinateList();
                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroup, _asrtCubeGroup.CurrentStoreGroupProfile.Key));
                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, _currStoreGroupLevelProfile.Key));
                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGrade, gradeHeader.Profile.Key));
                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentDetailVariable, varHeader.Profile.Key));

                            g6.Cols[i].UserData = new ColumnHeaderTag(asrtWaferCoordinateList, gradeHeader, varHeader, i, varHeader.Name);
                            g6.SetData(0, i, varHeader.Name);
                            _gridData[Grid6][0, i] = new ROCell();
                            _gridData[Grid6][0, i].Value = varHeader.Name;
                        }
                    }
                }
                else
                {
                    g6.Rows.Count = g4.Rows.Count;
                    g6.Cols.Count = g3.Cols.Count;
                    g6.Rows.Fixed = g4.Rows.Fixed;
                    g6.Cols.Fixed = g3.Cols.Fixed;

                    g6.AllowDragging = AllowDraggingEnum.None;

                    i = -1;
                    //Initialize here to avoid null reference exception
                    _gridData[Grid6] = new ROCell[g6.Rows.Count, g6.Cols.Count];

                    foreach (StoreGroupLevelProfile groupLevelProf in _storeGroupLevelProfileList)
                    {
                        groupLevelHeader = new RowColProfileHeader(groupLevelProf.Name, true, 0, groupLevelProf);

                        foreach (DictionaryEntry varEntry in _sortedDetailColumnHeaders)
                        {
                            varHeader = (RowColProfileHeader)varEntry.Value;

                            i++;
                            asrtWaferCoordinateList = new CubeWaferCoordinateList();
                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroup, _asrtCubeGroup.CurrentStoreGroupProfile.Key));
                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupLevelHeader.Profile.Key));
                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentDetailVariable, varHeader.Profile.Key));

                            g6.Cols[i].UserData = new ColumnHeaderTag(asrtWaferCoordinateList, groupLevelHeader, varHeader, i, varHeader.Name);
                            g6.SetData(0, i, varHeader.Name);
                            _gridData[Grid6][0, i] = new ROCell();
                            _gridData[Grid6][0, i].Value = varHeader.Name;
                        }
                    }
                }

                ((PagingGridTag)g6.Tag).Visible = true;
                ((PagingGridTag)g6.Tag).RowsPerRowGroup = 1;
                ((PagingGridTag)g6.Tag).RowGroupsPerGrid = g6.Rows.Count - g6.Rows.Fixed;
                ((PagingGridTag)g6.Tag).RowsPerScroll = 1;
                ((PagingGridTag)g6.Tag).ColsPerColGroup = _sortedDetailColumnHeaders.Count;
                ((PagingGridTag)g6.Tag).ColGroupsPerGrid = _sortedStoreGradeHeaders.Count + 1;
                ((PagingGridTag)g6.Tag).ColsPerScroll = 1;
               
                g6.Visible = ((PagingGridTag)g6.Tag).Visible;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Formatg7Grid(bool aClearGrid)
        {
            CubeWaferCoordinateList asrtWaferCoordinateList;
            int i;

            try
            {
                if (aClearGrid)
                {
                    g7.Clear();
                }

                if (g7.Tag == null)
                {
                    g7.Tag = new PagingGridTag(Grid7, g7, g4, null, null, 0, 0);
                }
               
                if (IsPostReceiptAssortment() || IsGroupAllocation)
                {                    
                    if (_includeBalance)
                    {
                        g7.Rows.Count = 2;
                    }
                    else
                    {
                        g7.Rows.Count = 1;
                    }
                }
                else
                {
                    g7.Rows.Count = 4;
                }

                g7.Cols.Count = g1.Cols.Count;
                g7.Rows.Fixed = 0;
                g7.Cols.Fixed = g1.Cols.Fixed;

                g7.AllowDragging = AllowDraggingEnum.None;
                g7.AllowMerging = AllowMergingEnum.Free;

                _gridData[Grid7] = new ROCell[g7.Rows.Count, g7.Cols.Count];//_sortedComponentColumnHeaders.Count];

                if (IsPostReceiptAssortment() || IsGroupAllocation)
                {
                    // Add Header Total Row
                    asrtWaferCoordinateList = new CubeWaferCoordinateList();
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AllocationHeader, 0));
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentSubTotal, 0));
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));
                    g7.Rows[0].UserData = new RowHeaderTag(asrtWaferCoordinateList, null, null, 0, string.Empty, string.Empty);
 
                    if (_includeBalance)
                    {
                        // Add Balance Row
                        asrtWaferCoordinateList = new CubeWaferCoordinateList();
                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));
                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentSubTotal, 0));
                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, ((AssortmentViewQuantityVariables)_quantityVariables).Balance.Key));

                        g7.Rows[1].UserData = new RowHeaderTag(asrtWaferCoordinateList, null, null, 1, string.Empty, string.Empty);
                    } 
                }
                else
                {

                    // Add Placeholder Total Row
                    asrtWaferCoordinateList = new CubeWaferCoordinateList();
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentSubTotal, 0));
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));
                    g7.Rows[0].UserData = new RowHeaderTag(asrtWaferCoordinateList, null, null, 0, string.Empty, string.Empty);

                    // Add Header Total Row
                    asrtWaferCoordinateList = new CubeWaferCoordinateList();
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AllocationHeader, 0));
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentSubTotal, 0));
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));
                    g7.Rows[1].UserData = new RowHeaderTag(asrtWaferCoordinateList, null, null, 1, string.Empty, string.Empty);

                    // Add Total Row
                    asrtWaferCoordinateList = new CubeWaferCoordinateList();
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentSubTotal, 0));
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, ((AssortmentViewQuantityVariables)_quantityVariables).Total.Key));

                    g7.Rows[2].UserData = new RowHeaderTag(asrtWaferCoordinateList, null, null, 2, string.Empty, string.Empty);

                    // Add Balance Row
                    asrtWaferCoordinateList = new CubeWaferCoordinateList();
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentSubTotal, 0));
                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, ((AssortmentViewQuantityVariables)_quantityVariables).Balance.Key));

                    g7.Rows[3].UserData = new RowHeaderTag(asrtWaferCoordinateList, null, null, 3, string.Empty, string.Empty);
                }


                for (i = 0; i < g7.Cols.Count; i++)
                {
                    if (IsPostReceiptAssortment() || IsGroupAllocation)
                    {
                        g7.SetData(0, i, MIDText.GetTextOnly(eMIDTextCode.lbl_Header_Total));
                        if (i < _sortedComponentColumnHeaders.Count)
                        {
                            _gridData[Grid7][0, i] = new ROCell();
                            _gridData[Grid7][0, i].Value = MIDText.GetTextOnly(eMIDTextCode.lbl_Header_Total);
                        }
                        if (_includeBalance)
                        {
                            g7.SetData(1, i, MIDText.GetTextOnly((int)eAssortmentSummaryVariables.Balance));
                            if (i < _sortedComponentColumnHeaders.Count)
                            {
                                _gridData[Grid7][1, i] = new ROCell();
                                _gridData[Grid7][1, i].Value = MIDText.GetTextOnly((int)eAssortmentSummaryVariables.Balance);
                            }
                        }
                    }
                    else
                    {
                        g7.SetData(0, i, MIDText.GetTextOnly(eMIDTextCode.lbl_Placeholder_Total));
                        g7.SetData(1, i, MIDText.GetTextOnly(eMIDTextCode.lbl_Header_Total));
                        g7.SetData(2, i, MIDText.GetTextOnly(eMIDTextCode.lbl_Total));
                        g7.SetData(3, i, MIDText.GetTextOnly((int)eAssortmentSummaryVariables.Balance));

                        if (i < _sortedComponentColumnHeaders.Count)
                        {
                            _gridData[Grid7][0, i] = new ROCell();
                            _gridData[Grid7][0, i].Value = MIDText.GetTextOnly(eMIDTextCode.lbl_Placeholder_Total);
                            _gridData[Grid7][1, i] = new ROCell();
                            _gridData[Grid7][1, i].Value = MIDText.GetTextOnly(eMIDTextCode.lbl_Header_Total);
                            _gridData[Grid7][2, i] = new ROCell();
                            _gridData[Grid7][2, i].Value = MIDText.GetTextOnly(eMIDTextCode.lbl_Total);
                            _gridData[Grid7][3, i] = new ROCell();
                            _gridData[Grid7][3, i].Value = MIDText.GetTextOnly((int)eAssortmentSummaryVariables.Balance);
                        }
                    }
                }
                
                for (i = 0; i < g4.Cols.Count; i++)
                {
                    g7.Cols[i].Visible = g4.Cols[i].Visible;
                }

                ((PagingGridTag)g7.Tag).Visible = true;
                ((PagingGridTag)g7.Tag).RowsPerRowGroup = 1;
                ((PagingGridTag)g7.Tag).RowGroupsPerGrid = 1;
                ((PagingGridTag)g7.Tag).RowsPerScroll = 1;
                ((PagingGridTag)g7.Tag).ColsPerColGroup = 1;
                ((PagingGridTag)g7.Tag).ColGroupsPerGrid = g7.Cols.Count;
                ((PagingGridTag)g7.Tag).ColsPerScroll = 1;

                foreach (Row row in g7.Rows)
                {
                    row.AllowMerging = true;
                }
                
                g7.Visible = ((PagingGridTag)g7.Tag).Visible;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Formatg8Grid(bool aClearGrid)
        {
            try
            {
                if (aClearGrid)
                {
                    g8.Clear();
                }

                if (g8.Tag == null)
                {
                    g8.Tag = new PagingGridTag(Grid8, g8, g7, g5, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE);
                }

                g8.Rows.Count = g7.Rows.Count;
                g8.Cols.Count = g2.Cols.Count;
                g8.Rows.Fixed = g7.Rows.Fixed;
                g8.Cols.Fixed = g2.Cols.Fixed;

                ((PagingGridTag)g8.Tag).Visible = true;
                ((PagingGridTag)g8.Tag).RowsPerRowGroup = 1;
                ((PagingGridTag)g8.Tag).RowGroupsPerGrid = g8.Rows.Count - g8.Rows.Fixed;
                ((PagingGridTag)g8.Tag).RowsPerScroll = 1;
                ((PagingGridTag)g8.Tag).ColsPerColGroup = 1;
                ((PagingGridTag)g8.Tag).ColGroupsPerGrid = g8.Cols.Count;
                ((PagingGridTag)g8.Tag).ColsPerScroll = 1;

                _gridData[Grid8] = new ROCell[g8.Rows.Count, g8.Cols.Count];
                g8.Visible = true;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Formatg9Grid(bool aClearGrid)
        {
            try
            {
                if (aClearGrid)
                {
                    g9.Clear();
                }

                if (g9.Tag == null)
                {
                    g9.Tag = new PagingGridTag(Grid9, g9, g7, g6, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE, null, null, null, null, false, true);
                }

                g9.Rows.Count = g7.Rows.Count;
                g9.Cols.Count = g3.Cols.Count;
                g9.Rows.Fixed = g7.Rows.Fixed;
                g9.Cols.Fixed = g3.Cols.Fixed;

                ((PagingGridTag)g9.Tag).Visible = true;
                ((PagingGridTag)g9.Tag).RowsPerRowGroup = 1;
                ((PagingGridTag)g9.Tag).RowGroupsPerGrid = g9.Rows.Count - g9.Rows.Fixed;
                ((PagingGridTag)g9.Tag).RowsPerScroll = 1;
                ((PagingGridTag)g9.Tag).ColsPerColGroup = _sortedDetailColumnHeaders.Count;
                ((PagingGridTag)g9.Tag).ColGroupsPerGrid = _sortedStoreGradeHeaders.Count + 1;
                ((PagingGridTag)g9.Tag).ColsPerScroll = 1;

                _gridData[Grid9] = new ROCell[g9.Rows.Count, g9.Cols.Count];
                g9.Visible = true;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private eAssortmentType GetAssortmentType()
        {
            eAssortmentType asrtType = _asrtCubeGroup.AssortmentType;

            if (asrtType == eAssortmentType.GroupAllocation)
                return asrtType;

            if (_buildDetailsGrid)
            {
               asrtType = (eAssortmentType)AssortProfile.AsrtType;
            }
            if (_asrtCubeGroup.AssortmentType != asrtType && !_formLoading)
            {
                _asrtTypeChanged = true;
            }
            _asrtCubeGroup.AssortmentType = asrtType;

            return asrtType;
        }

        private bool IsPostReceiptAssortment()
        {
            bool isPostReceipt = false;

            try
            {
                int assortmentCount = 0;
                int placeholderCount = 0;	
                int headerCount = 0;        
                _headerList = (AllocationHeaderProfileList)_asrtCubeGroup.GetHeaderList();

                foreach (AllocationHeaderProfile ahp in _headerList)
                {
                    if (ahp.HeaderType == eHeaderType.Assortment)
                    {
                        assortmentCount++;
                        if (ahp.AsrtType == (int)eAssortmentType.PostReceipt)
                        {
                            isPostReceipt = true;
                        }
                    }
                    
                    else if (ahp.HeaderType == eHeaderType.Placeholder)
                    {
                        placeholderCount++;
                    }
                    else
                    {
                        headerCount++;
                    }
                    
                }

                if (assortmentCount > 1)
                {
                    isPostReceipt = false;
                    
                }
                else
                {
                    // This catches the initial drop of header(s) onto an assortment to make it a post receipt.
                    if (!isPostReceipt && placeholderCount == 0 && headerCount > 0)
                    {
                        isPostReceipt = true;
                    }
                }                

                return isPostReceipt;
            }
            catch
            {
                throw;
            }
        }

        /*private void SetGroupBy()
        {
            try
            {
                if (_openParms.GroupBy == eAllocationAssortmentViewGroupBy.StoreGrade)
                {
                    GroupByStoreGrade();
                }
                else
                {
                    GroupByAttribute();
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }        */

        public void GetCellRange(
        C1FlexGrid aGrid,
        int aStartRow,
        int aStartCol,
        int aEndRow,
        int aEndCol,
        int aPriority)
        {
            bool wait;
            PagingGridTag gridTag;
            C1FlexGrid rowHdrGrid;
            C1FlexGrid colHdrGrid;
            int i, j, x, y;
            int row, col;
           
            WaferCell[,] waferCellTable;
           
            CubeWafer asrtWafer;
            ColumnHeaderTag ColTag;
            RowHeaderTag RowTag;
            ComputationCellFlags cellFlags = new ComputationCellFlags();

            try
            {
                gridTag = (PagingGridTag)aGrid.Tag;

                wait = true;

                while (wait)
                {
                    wait = false;

                    try
                    {                        

                        if (aStartRow <= aEndRow && aStartCol <= aEndCol)
                        {
                            rowHdrGrid = gridTag.RowHeaderGrid;
                            colHdrGrid = gridTag.ColHeaderGrid;

                            //Create the AssortmentWafer to request data
                            asrtWafer = new CubeWafer();

                            //Fill CommonWaferCoordinateListGroup
                            asrtWafer.CommonWaferCoordinateList = _commonWaferCoordinateList;

                            //Fill ColWaferCoordinateListGroup
                            asrtWafer.ColWaferCoordinateListGroup.Clear();

                            for (i = aStartCol; i <= aEndCol; i++)
                            {
                                ColTag = (ColumnHeaderTag)colHdrGrid.Cols[i].UserData;
                                if (ColTag != null)
                                {
                                    asrtWafer.ColWaferCoordinateListGroup.Add(ColTag.CubeWaferCoorList);
                                }
                            }

                            //Fill RowWaferCoordinateListGroup

                            asrtWafer.RowWaferCoordinateListGroup.Clear();
                            for (i = aStartRow; i <= aEndRow; i++)
                            {
                                RowTag = (RowHeaderTag)rowHdrGrid.Rows[i].UserData;
                                if (RowTag != null)
                                {
                                    asrtWafer.RowWaferCoordinateListGroup.Add(RowTag.CubeWaferCoorList);
                                }
                            }

                            if (asrtWafer.ColWaferCoordinateListGroup.Count > 0 && asrtWafer.RowWaferCoordinateListGroup.Count > 0)
                            {
                                // Retreive array of values

                                waferCellTable = _asrtCubeGroup.GetAssortmentWaferCellValues(asrtWafer);

                                //// Load Grid with values

                                //aGrid.Redraw = false;

                                try
                                {
                                    x = 0;

                                    for (i = aStartRow; i <= aEndRow; i++)
                                    {
                                        RowTag = (RowHeaderTag)rowHdrGrid.Rows[i].UserData;

                                        if (RowTag != null)
                                        {
                                            y = 0;

                                            for (j = aStartCol; j <= aEndCol; j++)
                                            {
                                                ColTag = (ColumnHeaderTag)colHdrGrid.Cols[j].UserData;

                                                if (ColTag != null)
                                                {

                                                    row = ((RowHeaderTag)rowHdrGrid.Rows[i].UserData).Order;
                                                    col = ((ColumnHeaderTag)colHdrGrid.Cols[j].UserData).Order;
                                                    if (_gridData[gridTag.GridId][row, col] == null)
                                                    {
                                                        _gridData[gridTag.GridId][row, col] = new ROCell();
                                                    }
                                                    _gridData[gridTag.GridId][row, col].ROCellAttributes.RowOrder = row;
                                                    _gridData[gridTag.GridId][row, col].ROCellAttributes.ColOrder = col;
                                                   

                                                    if (RowTag.LoadData)
                                                    {
                                                        if (waferCellTable[x, y] != null)
                                                        {
                                                            _gridData[gridTag.GridId][row, col].ROCellAttributes.VariableStyle = waferCellTable[x, y].VariableStyle;
                                                            _gridData[gridTag.GridId][row, col].CellDataType = eCellDataType.Units;

                                                            switch (waferCellTable[x, y].VariableStyle)
                                                            {
                                                                case eVariableStyle.Dollar:
                                                                    _gridData[gridTag.GridId][row, col].CellDataType = eCellDataType.Dollar;
                                                                    _gridData[gridTag.GridId][row, col].CellValueType = eCellValueType.Number;
                                                                    break;
                                                                case eVariableStyle.Percentage:
                                                                    _gridData[gridTag.GridId][row, col].CellDataType = eCellDataType.Percentage;
                                                                    _gridData[gridTag.GridId][row, col].CellValueType = eCellValueType.Number;
                                                                    break;
                                                                case eVariableStyle.Units:
                                                                    _gridData[gridTag.GridId][row, col].CellDataType = eCellDataType.Units;
                                                                    _gridData[gridTag.GridId][row, col].CellValueType = eCellValueType.Integer;
                                                                    break;
                                                                default:
                                                                    _gridData[gridTag.GridId][row, col].CellDataType = eCellDataType.None;
                                                                    _gridData[gridTag.GridId][row, col].CellValueType = eCellValueType.None;
                                                                    break;
                                                            }

                                                            cellFlags = waferCellTable[x, y].Flags;
                                                            
                                                            ApplyCellFlagOverrides(gridTag, x, y, ref cellFlags);
                                                            
                                                            if (!AssortmentCellFlagValues.isBlocked(cellFlags))
                                                            {
                                                                aGrid[i, j] = waferCellTable[x, y].ValueAsString;
                                                            }
                                                            else
                                                            {
                                                                aGrid[i, j] = NULL_DATA_STRING;
                                                            }

                                                            if (_gridData[gridTag.GridId][row, col].CellDataType == eCellDataType.Dollar
                                                                || _gridData[gridTag.GridId][row, col].CellDataType == eCellDataType.Percentage
                                                                || _gridData[gridTag.GridId][row, col].CellDataType == eCellDataType.Units)
                                                            {
                                                                _gridData[gridTag.GridId][row, col].Value = waferCellTable[x, y].Value;
                                                            }
                                                            else
                                                            {
                                                                _gridData[gridTag.GridId][row, col].Value = waferCellTable[x, y].ValueAsString;
                                                            }

                                                            _gridData[gridTag.GridId][row, col].ROCellAttributes.DecimalPositions = waferCellTable[x, y].NumberOfDecimals;
                                                            _gridData[gridTag.GridId][row, col].ROCellAttributes.IsNegative = waferCellTable[x, y].isValueNegative;
                                                            _gridData[gridTag.GridId][row, col].ROCellAttributes.IsNumeric = waferCellTable[x, y].isValueNumeric;

                                                            _gridData[gridTag.GridId][row, col].ROCellAttributes.IsLocked = ComputationCellFlagValues.isLocked(cellFlags);
                                                            _gridData[gridTag.GridId][row, col].ROCellAttributes.IsDisplayOnly = ComputationCellFlagValues.isDisplayOnly(cellFlags);
                                                            _gridData[gridTag.GridId][row, col].ROCellAttributes.IsEditable = !ComputationCellFlagValues.isDisplayOnly(cellFlags);
                                                            _gridData[gridTag.GridId][row, col].ROCellAttributes.IsModified = ComputationCellFlagValues.isChanged(cellFlags);
                                                            _gridData[gridTag.GridId][row, col].ROCellAttributes.IsClosed = AssortmentCellFlagValues.isBlocked(cellFlags);
                                                        }
                                                        else
                                                        {
                                                            aGrid[i, j] = NULL_DATA_STRING;
                                                            cellFlags.Clear();
                                                            ComputationCellFlagValues.isNull(ref cellFlags, true);
                                                            _gridData[gridTag.GridId][row, col].CellDataType = eCellDataType.None;

                                                        }
                                                    }
                                                    else
                                                    {
                                                        cellFlags.Clear();
                                                        ComputationCellFlagValues.isNull(ref cellFlags, true);

                                                        ApplyCellFlagOverrides(gridTag, x, y, ref cellFlags);
                                                        
                                                        _gridData[gridTag.GridId][row, col].CellDataType = eCellDataType.None;
                                                    }

                                                    y++;
                                                }
                                            }

                                            x++;
                                        }
                                    }
                                }
                                catch (Exception exc)
                                {
                                    string message = exc.ToString();
                                    throw;
                                }
                                finally
                                {
                                    aGrid.Redraw = _currentRedrawState;
                                }
                            }
                        }
                    }
                    catch (WaitPageLoadException)
                    {
                        wait = true;
                    }
                    catch (Exception exc)
                    {
                        string message = exc.ToString();
                        throw;
                    }
                    finally
                    {
                       // _pageLoadLock.ReleaseWriterLock();
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        void ApplyCellFlagOverrides(PagingGridTag gridTag, int row, int col, ref ComputationCellFlags cellFlags)
        {
            C1FlexGrid rowGrid;
            C1FlexGrid colGrid;
            CubeWaferCoordinateList rowCoorList;
            CubeWaferCoordinateList colCoorList;
            eAssortmentDetailVariables detVar = 0;
            eAssortmentTotalVariables totVar = 0;
            eAssortmentQuantityVariables qtyVar = 0;

            //BEGIN TT#681 - MD - DOConnell - Receive "Nothing to Spread" exception when applying changes to total units for a header.
            RowColProfileHeader varHeader1;
            AssortmentComponentVariableProfile varProf1;
            RowColProfileHeader varHeader2;
            AssortmentComponentVariableProfile varProf2;
            bool disable = false;
            bool noPh = false;		// TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
            bool noHdr = false;     // TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
                                    //END TT#681 - MD - DOConnell - Receive "Nothing to Spread" exception when applying changes to total units for a header.

            try
            {
                rowGrid = gridTag.RowHeaderGrid;
                colGrid = gridTag.ColHeaderGrid;
                rowCoorList = ((RowHeaderTag)rowGrid.Rows[row].UserData).CubeWaferCoorList;
                colCoorList = ((ColumnHeaderTag)colGrid.Cols[col].UserData).CubeWaferCoorList;

                // Detail Grids
                //BEGIN TT#681 - MD - DOConnell - Receive "Nothing to Spread" exception when applying changes to total units for a header.
                if (gridTag.GridId == 4 || gridTag.GridId == 5 || gridTag.GridId == 6)
                {
                    varHeader1 = (RowColProfileHeader)_sortedComponentColumnHeaders.GetByIndex(0);
                    varProf1 = (AssortmentComponentVariableProfile)varHeader1.Profile;
                    if (varProf1.Key == Convert.ToInt32(eAssortmentComponentVariables.Placeholder))
                    {
                        foreach (DictionaryEntry varEntry in _sortedComponentColumnHeaders)
                        {
                            varHeader2 = (RowColProfileHeader)varEntry.Value;
                            varProf2 = (AssortmentComponentVariableProfile)varHeader2.Profile;
                            if (varProf2.Key == Convert.ToInt32(eAssortmentComponentVariables.HeaderID))
                            {
                                foreach (CubeWaferCoordinate coor in rowCoorList)
                                {
                                    if (coor.WaferCoordinateType == eProfileType.PlaceholderHeader)
                                    {
                                        qtyVar = (eAssortmentQuantityVariables)coor.Key;
                                        if (Convert.ToInt32(qtyVar) != int.MaxValue)
                                        {
                                            noPh = true;	// TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
                                        }
                                    }
                                    if (coor.WaferCoordinateType == eProfileType.AllocationHeader)
                                    {
                                        qtyVar = (eAssortmentQuantityVariables)coor.Key;
                                        if (Convert.ToInt32(qtyVar) != int.MaxValue)
                                        {
                                            noHdr = true;	// TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
                                        }
                                    }
                                }

                                if (noHdr == true && noPh == true)		// TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
                                {
                                    disable = true;
                                }
                                foreach (CubeWaferCoordinate coor in colCoorList)
                                {
                                    if (coor.WaferCoordinateType == eProfileType.AssortmentDetailVariable)
                                    {
                                        detVar = (eAssortmentDetailVariables)coor.Key;
                                    }
                                    else if (coor.WaferCoordinateType == eProfileType.AssortmentTotalVariable)
                                    {
                                        totVar = (eAssortmentTotalVariables)coor.Key;
                                    }
                                    //Debug.WriteLine("COL Grid " + gridTag.GridId + " " + coor.WaferCoordinateType + " " + coor.Key);
                                }

                                if (detVar == eAssortmentDetailVariables.TotalUnits || totVar == eAssortmentTotalVariables.TotalUnits && disable == true)
                                {
                                    //ComputationCellFlagValues.isDisplayOnly(ref cellFlags, true); //TT#1229 - MD - DOConnell - Cannot change values in the Total Units column on Assortment Matrix
                                }
                                // Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                                if (IsGroupAllocation)
                                {
                                    if (detVar == eAssortmentDetailVariables.AvgUnits || totVar == eAssortmentTotalVariables.AvgUnits && disable == true)
                                    {
                                        ComputationCellFlagValues.isDisplayOnly(ref cellFlags, true);
                                    }
                                }
                                // End TT#4294 - stodd - Average Units in Matrix Enahancement
                            }
                        }
                    }
                    //END TT#681 - MD - DOConnell - Receive "Nothing to Spread" exception when applying changes to total units for a header.
                    // Begin TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
                    disable = true;
                    foreach (DictionaryEntry varEntry in _sortedComponentColumnHeaders)
                    {
                        varHeader2 = (RowColProfileHeader)varEntry.Value;
                        varProf2 = (AssortmentComponentVariableProfile)varHeader2.Profile;
                        if (varProf2.Key == Convert.ToInt32(eAssortmentComponentVariables.HeaderID))
                        {
                            disable = false;
                            break;
                        }
                    }
                    foreach (CubeWaferCoordinate coor in colCoorList)
                    {
                        if (coor.WaferCoordinateType == eProfileType.AssortmentDetailVariable)
                        {
                            detVar = (eAssortmentDetailVariables)coor.Key;
                        }
                        else if (coor.WaferCoordinateType == eProfileType.AssortmentTotalVariable)
                        {
                            totVar = (eAssortmentTotalVariables)coor.Key;
                        }
                        //Debug.WriteLine("COL Grid " + gridTag.GridId + " " + coor.WaferCoordinateType + " " + coor.Key);

                        // Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                        if (coor.WaferCoordinateType == eProfileType.PlaceholderHeader)
                        {
                            qtyVar = (eAssortmentQuantityVariables)coor.Key;
                            if (Convert.ToInt32(qtyVar) != int.MaxValue)
                            {
                                noPh = true;
                            }
                        }
                        if (coor.WaferCoordinateType == eProfileType.AllocationHeader)
                        {
                            qtyVar = (eAssortmentQuantityVariables)coor.Key;
                            if (Convert.ToInt32(qtyVar) != int.MaxValue)
                            {
                                noHdr = true;
                            }
                        }
                        // End TT#4294 - stodd - Average Units in Matrix Enahancement
                    }

                    if (totVar == eAssortmentTotalVariables.ReserveUnits && disable == true)
                    {
                        ComputationCellFlagValues.isDisplayOnly(ref cellFlags, true);
                    }
                    // End TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix

                    // Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    if (IsGroupAllocation)
                    {
                        if ((detVar == eAssortmentDetailVariables.AvgUnits || totVar == eAssortmentTotalVariables.AvgUnits)
                            && noPh && noHdr)
                        {
                            ComputationCellFlagValues.isDisplayOnly(ref cellFlags, true);
                        }
                    }
                    // End TT#4294 - stodd - Average Units in Matrix Enahancement
                }

                // Total Grids
                if (gridTag.GridId == 7 || gridTag.GridId == 8 || gridTag.GridId == 9)
                {
                    //rowGrid = gridTag.RowHeaderGrid;
                    //colGrid = gridTag.ColHeaderGrid;
                    //rowCoorList = ((RowHeaderTag)rowGrid.Rows[row].UserData).CubeWaferCoorList;
                    //colCoorList = ((ColumnHeaderTag)colGrid.Cols[col].UserData).CubeWaferCoorList;

                    foreach (CubeWaferCoordinate coor in rowCoorList)
                    {
                        if (coor.WaferCoordinateType == eProfileType.AssortmentQuantityVariable)
                        {
                            qtyVar = (eAssortmentQuantityVariables)coor.Key;
                        }
                        //Debug.WriteLine("ROW Grid " + gridTag.GridId + " " + coor.WaferCoordinateType + " " + coor.Key);
                    }
                    foreach (CubeWaferCoordinate coor in colCoorList)
                    {
                        if (coor.WaferCoordinateType == eProfileType.AssortmentDetailVariable)
                        {
                            detVar = (eAssortmentDetailVariables)coor.Key;
                        }
                        else if (coor.WaferCoordinateType == eProfileType.AssortmentTotalVariable)
                        {
                            totVar = (eAssortmentTotalVariables)coor.Key;
                        }
                        //Debug.WriteLine("COL Grid " + gridTag.GridId + " " + coor.WaferCoordinateType + " " + coor.Key);
                    }

                    if (detVar == eAssortmentDetailVariables.TotalPct || totVar == eAssortmentTotalVariables.TotalPct)
                    {
                        ComputationCellFlagValues.isDisplayOnly(ref cellFlags, true);
                    }
                    // Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    if (IsGroupAllocation)
                    {
                        if (detVar == eAssortmentDetailVariables.AvgUnits || totVar == eAssortmentTotalVariables.AvgUnits)
                        {
                            ComputationCellFlagValues.isDisplayOnly(ref cellFlags, true);
                        }
                    }
                    // End TT#4294 - stodd - Average Units in Matrix Enahancement

                    // Begin TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
                    disable = true;
                    foreach (DictionaryEntry varEntry in _sortedComponentColumnHeaders)
                    {
                        varHeader2 = (RowColProfileHeader)varEntry.Value;
                        varProf2 = (AssortmentComponentVariableProfile)varHeader2.Profile;
                        if (varProf2.Key == Convert.ToInt32(eAssortmentComponentVariables.HeaderID))
                        {
                            disable = false;
                            break;
                        }
                    }
                    if (totVar == eAssortmentTotalVariables.ReserveUnits && disable == true)
                    {
                        ComputationCellFlagValues.isDisplayOnly(ref cellFlags, true);
                    }
                    // End TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
                }
            }
            catch
            {
                throw;
            }

        }


        internal ROCells GetAssortmentMatrixData(int gridId, int rowCount, int colCount, int mapGridId, int mapGridRowCount, int mapgridColCount)
        {
            ROCells asrtMatrixCells = new ROCells(rowCount - 1, colCount);
            asrtMatrixCells.AddCells(rowCount, colCount);

            int counter = _sortedDetailColumnHeaders.Count;

            // since grid 3 will have columns based on view and the details selected,
            // using _sortedStoreGradeHeaders and _sortedDetailColumnHeaders counts 
            if (gridId == Grid3)
            {
                for (int row = 0; row < rowCount; row++)
                {                
                    for (int col = 0; col < colCount; col++)
                    {                        
                        if (row == 0)
                        {
                            asrtMatrixCells.Columns.Add(new ROColumnAttributes(_gridData[gridId][row, col].ValueAsString, 0, Include.DefaultColumnWidth));
                        }
                        else
                        {                            
                            if (col % counter == 0)
                            {
                                asrtMatrixCells.ROCell[row][col] = _gridData[gridId][row, col];
                            }
                            else
                            {
                                asrtMatrixCells.ROCell[row][col] = new ROCell();
                            }
                        }
                    }
                }
            }
            else
            {
                //will work for grid 2,3,5,6 column and grid data
                for (int r = 0; r < rowCount; r++)
                {
                    for (int c = 0; c < colCount; c++)
                    {
                        if (_gridData[gridId][r, c] != null)
                        {
                            if (r == 0)//&& c == 0)
                            {
                                if (gridId == Grid8 || gridId == Grid9)
                                {
                                    asrtMatrixCells.ROCell[r][c] = _gridData[gridId][r, c];
                                }
                                else
                                {
                                    asrtMatrixCells.Columns.Add(new ROColumnAttributes(_gridData[gridId][r, c].ValueAsString, 0, Include.DefaultColumnWidth));
                                }
                            }
                            else
                            {
                                asrtMatrixCells.ROCell[r][c] = _gridData[gridId][r, c];
                            }
                        }
                    }
                }
            }

            //will work for grid 1 and 7
            for (int mapRow = 0; mapRow < mapGridRowCount; mapRow++)
            {
                for (int mapCol = 0; mapCol < mapgridColCount; mapCol++)
                {
                    if (_gridData[mapGridId][mapRow, mapCol] != null)
                    {
                        //Add RORowAttribute for each row, so the front end can use it to loop through
                        if (mapGridId == Grid5 || mapGridId == Grid6)
                        {
                            asrtMatrixCells.Rows.Add(new RORowAttributes(string.Empty));
                        }
                        else
                        {
                            asrtMatrixCells.Rows.Add(new RORowAttributes(_gridData[mapGridId][mapRow, mapCol].ValueAsString));
                        }
                    }
                }
            }

            return asrtMatrixCells;
        }

        internal ROCells MapRowColumnAttributesForGrid4(int gridId, int rowCount, int colCount)
        {
            ROCells asrtMatrixCells = new ROCells(rowCount, 2);
            asrtMatrixCells.AddCells(rowCount, 2);

            string sCol1 = string.Empty;
            int colIndex = -1;        

            //column attributes
            for (colIndex = 0; colIndex < colCount - 1; colIndex++)
            {
                if (string.IsNullOrEmpty(sCol1))
                {
                    sCol1 = _gridData[gridId][0, colIndex].ValueAsString;
                }
                else
                {
                    sCol1 = sCol1 + "|" + _gridData[gridId][0, colIndex].ValueAsString;
                }
            }                      

            asrtMatrixCells.Columns.Add(new ROColumnAttributes(sCol1, 0, Include.DefaultColumnWidth));

            string sCol2 = _gridData[gridId][0, colIndex].ValueAsString;
            asrtMatrixCells.Columns.Add(new ROColumnAttributes(sCol2, 1, Include.DefaultColumnWidth));

            //To add row attributes, get the grid 5 row count and loopin 
            ROCells gridCells = _asrtMatrixROData.GetCells(eDataType.AssortmentDetailTotals);
            int rCount = gridCells.Rows.Count;

            for (int a = 0; a < rCount; a++)
            {
                asrtMatrixCells.Rows.Add(new RORowAttributes(string.Empty));
            }

            //Row attributes
            int leftDim = _gridData[gridId].GetLength(0);
            int rightDim = _gridData[gridId].GetLength(1);            

            for (int i = 1; i < leftDim; i++)
            {
                int counter = 0;
                string sTreeData = string.Empty;

                for (int j = 0; j < rightDim - 1; j++)
                {
                    if (string.IsNullOrEmpty(sTreeData))
                    {
                        sTreeData = _gridData[gridId][i, j].ValueAsString + "|";
                    }
                    else
                    {
                        sTreeData = string.Concat(sTreeData + ",|" + _gridData[gridId][i, j].ValueAsString);
                    }
                    
                    counter++;                    
                }
                asrtMatrixCells.ROCell[i][0] = new ROCell(eCellDataType.None, sTreeData);
                asrtMatrixCells.ROCell[i][1] = new ROCell(eCellDataType.None, _gridData[gridId][i, counter].ValueAsString);

                sTreeData = string.Empty;
            }

            return asrtMatrixCells;
        }

        private void ReformatRowsChanged(bool aClearGrid)
        {
            try
            {
                FormatCol1Grids(aClearGrid);
                FormatCol2Grids(aClearGrid, -1, SortEnum.none);
                FormatCol3Grids(aClearGrid, -1, SortEnum.none);
                
                LoadCurrentPages();                
                
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void StopPageLoadThreads()
        {
            try
            {
                _stopPageLoadThread = true;
                ((PagingGridTag)g5.Tag).WaitForPageLoads();
                ((PagingGridTag)g6.Tag).WaitForPageLoads();
                ((PagingGridTag)g8.Tag).WaitForPageLoads();
                ((PagingGridTag)g9.Tag).WaitForPageLoads();
                _stopPageLoadThread = false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        #endregion

        #endregion

        #region Get Assortment Review View names list
        internal ROOut GetAssortmentReviewViewList()
        {
            try
            {
                List<KeyValuePair<int, string>> defaultViewDetails = GetAssortmentReviewNamesList();

                return new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, defaultViewDetails);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal List<KeyValuePair<int, string>> GetAssortmentReviewNamesList()
        {
            DataTable dtView;
            int userRid;
            int viewRid;
            string viewId;
            _bindingView = true;

            filterUserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreUser);
            filterGlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreGlobal);

            _userRIDList = new ArrayList();
            _userRIDList.Add(-1);

            if (filterUserSecurity.AllowUpdate || filterUserSecurity.AllowView)
            {
                _userRIDList.Add(SAB.ClientServerSession.UserRID);
            }

            if (filterGlobalSecurity.AllowUpdate || filterGlobalSecurity.AllowView)
            {
                _userRIDList.Add(Include.GlobalUserRID);
            }

            _asrtCubeGroup = new AssortmentCubeGroup(this.SAB, _applicationSessionTransaction, eAssortmentWindowType.Assortment);
            _assortmentViewData = new AssortmentViewData();

            if (GetAssortmentType() == eAssortmentType.PostReceipt)
            {
                dtView = _assortmentViewData.AssortmentView_ReadPostReceipt(_userRIDList);
            }
            else if (GetAssortmentType() == eAssortmentType.GroupAllocation)
            {
                dtView = _assortmentViewData.AssortmentView_Read(_userRIDList, eAssortmentViewType.GroupAllocation);
            }
            else
            {
                dtView = _assortmentViewData.AssortmentView_Read(_userRIDList, eAssortmentViewType.Assortment);
            }
            foreach (DataRow row in dtView.Rows)
            {
                userRid = Convert.ToInt32(row["USER_RID"]);
                viewId = row["VIEW_ID"].ToString();
                viewRid = Convert.ToInt32(row["VIEW_RID"]);
                if (userRid != Include.GlobalUserRID)
                {
                    viewId = viewId + " (" + UserNameStorage.GetUserName(userRid) + ")";
                    row["VIEW_ID"] = viewId;
                }
            }
            _bindingView = false;
            return DataTableTools.DataTableToKeyValues(dtView, "VIEW_RID", "VIEW_ID");
        }

        #endregion

        #region Apply Assortment Review Matrix Changes
        public ROOut ApplyAssortmentReviewMatrixCellChanges(ROGridChangesParms gridChanges)
        {
            PagingGridTag gridTag;
            C1FlexGrid grid = null;
            C1FlexGrid rowGrid;
            C1FlexGrid colGrid;
            string message = null;
            eROReturnCode returnCode = eROReturnCode.Successful;

            try
            {

                foreach (ROGridCellChange cellChange in gridChanges.CellChanges)
                {
                    switch (cellChange.DataType)
                    {
                        case eDataType.AssortmentSummaryTotals:
                            grid = g2;
                            break;
                        case eDataType.AssortmentSummaryDetail:
                            grid = g3;
                            break;
                        case eDataType.AssortmentDetailTotals:
                            grid = g5;
                            break;
                        case eDataType.AssortmentDetailDetail:
                            grid = g6;
                            break;
                        case eDataType.AssortmentTotalTotals:
                            grid = g8;
                            break;
                        case eDataType.AssortmentTotalDetail:
                            grid = g9;
                            break;
                    }

                    gridTag = (PagingGridTag)grid.Tag;
                    rowGrid = gridTag.RowHeaderGrid;
                    colGrid = gridTag.ColHeaderGrid;

                    switch (cellChange.CellAction)
                    {
                        case ROGridCellChange.eROCellAction.CellChanged:
                            SetCellValue(
                                aCommonWaferList: _commonWaferCoordinateList,
                                aRowWaferList: ((RowHeaderTag)rowGrid.Rows[cellChange.RowIndex].UserData).CubeWaferCoorList,
                                aColWaferList: ((ColumnHeaderTag)colGrid.Cols[cellChange.ColumnIndex].UserData).CubeWaferCoorList,
                                aValue: System.Convert.ToDouble(cellChange.dNewValue, CultureInfo.CurrentUICulture));
                            break;
                        case ROGridCellChange.eROCellAction.Lock:
                            SetCellLockStatus(
                                aCommonWaferList: _commonWaferCoordinateList,
                                aRowWaferList: ((RowHeaderTag)rowGrid.Rows[cellChange.RowIndex].UserData).CubeWaferCoorList,
                                aColWaferList: ((ColumnHeaderTag)colGrid.Cols[cellChange.ColumnIndex].UserData).CubeWaferCoorList,
                                aLockStatus: true);
                            break;
                        case ROGridCellChange.eROCellAction.Unlock:
                            SetCellLockStatus(
                                aCommonWaferList: _commonWaferCoordinateList,
                                aRowWaferList: ((RowHeaderTag)rowGrid.Rows[cellChange.RowIndex].UserData).CubeWaferCoorList,
                                aColWaferList: ((ColumnHeaderTag)colGrid.Cols[cellChange.ColumnIndex].UserData).CubeWaferCoorList,
                                aLockStatus: false);
                            break;
                        case ROGridCellChange.eROCellAction.CascadeLock:
                            SetCellRecursiveLockStatus(
                                aCommonWaferList: _commonWaferCoordinateList,
                                aRowWaferList: ((RowHeaderTag)rowGrid.Rows[cellChange.RowIndex].UserData).CubeWaferCoorList,
                                aColWaferList: ((ColumnHeaderTag)colGrid.Cols[cellChange.ColumnIndex].UserData).CubeWaferCoorList,
                                aLockStatus: true,
                                varRid: ((ColumnHeaderTag)colGrid.Cols[cellChange.ColumnIndex].UserData).DetailRowColHeader.Profile.Key);
                            break;
                        case ROGridCellChange.eROCellAction.CascadeUnlock:
                            SetCellRecursiveLockStatus(
                               aCommonWaferList: _commonWaferCoordinateList,
                               aRowWaferList: ((RowHeaderTag)rowGrid.Rows[cellChange.RowIndex].UserData).CubeWaferCoorList,
                               aColWaferList: ((ColumnHeaderTag)colGrid.Cols[cellChange.ColumnIndex].UserData).CubeWaferCoorList,
                               aLockStatus: false,
                               varRid: ((ColumnHeaderTag)colGrid.Cols[cellChange.ColumnIndex].UserData).DetailRowColHeader.Profile.Key);
                            break;
                        case ROGridCellChange.eROCellAction.Open:
                            SetCellBlockStatus(
                                aCommonWaferList: _commonWaferCoordinateList,
                                aRowWaferList: ((RowHeaderTag)rowGrid.Rows[cellChange.RowIndex].UserData).CubeWaferCoorList,
                                aColWaferList: ((ColumnHeaderTag)colGrid.Cols[cellChange.ColumnIndex].UserData).CubeWaferCoorList,
                                aBlockStatus: false);
                            break;
                        case ROGridCellChange.eROCellAction.Close:
                            SetCellBlockStatus(
                                aCommonWaferList: _commonWaferCoordinateList,
                                aRowWaferList: ((RowHeaderTag)rowGrid.Rows[cellChange.RowIndex].UserData).CubeWaferCoorList,
                                aColWaferList: ((ColumnHeaderTag)colGrid.Cols[cellChange.ColumnIndex].UserData).CubeWaferCoorList,
                                aBlockStatus: true);
                            break;
                    }

                }
            }
            catch (MIDException MIDEx)
            {
                message = MIDEx.ErrorMessage;
                returnCode = eROReturnCode.Failure;
            }
            catch
            {
                throw;
            }

            return new ROGridData(returnCode, message, ROInstanceID, _asrtMatrixROData, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        private void SetCellValue(CubeWaferCoordinateList aCommonWaferList, CubeWaferCoordinateList aRowWaferList, CubeWaferCoordinateList aColWaferList, double aValue)
        {
            Hashtable blockedHash;

            _asrtCubeGroup.SetCellValue(
                           aCommonWaferList: aCommonWaferList,
                           aRowWaferList: aRowWaferList,
                           aColWaferList: aColWaferList,
                           aValue: aValue,
                           aUnitScaling: "1",
                           aDollarScaling: "1",
                           ignoreDisplayOnly: false);

            _asrtCubeGroup.RecomputeCubes(true);
            SetActivateAssortmentOnHeaders(true);
            SaveDetailCubeGroup();
            SetActivateAssortmentOnHeaders(false);
            blockedHash = (Hashtable)_asrtCubeGroup.BlockedList.Clone();
            CloseAndReOpenCubeGroup();
            _asrtCubeGroup.BlockedList = blockedHash;
            if (IsAssortment)
            {
                UpdateData(reload: true, rebuildComponents: true, reloadBlockedCells: false, reloadHeaders: false);
            }

            LoadCurrentPages();
        }

        private void SetCellLockStatus(CubeWaferCoordinateList aCommonWaferList, CubeWaferCoordinateList aRowWaferList, CubeWaferCoordinateList aColWaferList, bool aLockStatus)
        {
            _asrtCubeGroup.SetCellLockStatus(
                            aCommonWaferList: aCommonWaferList,
                            aRowWaferList: aRowWaferList,
                            aColWaferList: aColWaferList,
                            aLockStatus: aLockStatus);

            LoadCurrentPages();
        }

        private void SetCellRecursiveLockStatus(CubeWaferCoordinateList aCommonWaferList, CubeWaferCoordinateList aRowWaferList, CubeWaferCoordinateList aColWaferList, bool aLockStatus, int varRid)
        {
            ComputationCellReference cellRef;

            cellRef = _asrtCubeGroup.SetCellRecursiveLockStatus(
                            aCommonWaferList: aCommonWaferList,
                            aRowWaferList: aRowWaferList,
                            aColWaferList: aColWaferList,
                            aLockStatus: aLockStatus);

            if (!aLockStatus)
            {
                if (Enum.IsDefined(typeof(eAssortmentDetailVariables), varRid))
                {
                    CascadeUnlockDetailVariables((AssortmentCellReference)cellRef, varRid);
                }
                else
                {
                    CascadeUnlockTotalVariables((AssortmentCellReference)cellRef, varRid);
                }
            }

            LoadCurrentPages();
        }

        private void CascadeUnlockDetailVariables(AssortmentCellReference cellRef, int varRid)
        {
            ComputationCellReference copyCellRef = null;
            foreach (AssortmentDetailVariableProfile varProf in _asrtCubeGroup.AssortmentComputations.AssortmentDetailVariables.VariableProfileList)
            {
                if (varProf.Key != varRid)
                {
                    copyCellRef = (AssortmentCellReference)cellRef.Copy();
                    copyCellRef[eProfileType.AssortmentDetailVariable] = varProf.Key;

                    if (!copyCellRef.isCellReadOnly && !copyCellRef.isCellDisplayOnly && !copyCellRef.isCellProtected && !copyCellRef.isCellClosed && !copyCellRef.isCellIneligible && copyCellRef.isCellLocked)
                    {
                        _asrtCubeGroup.SetCellRecursiveLockStatus(copyCellRef, false);
                    }
                }
            }
        }

        private void SetCellBlockStatus(CubeWaferCoordinateList aCommonWaferList, CubeWaferCoordinateList aRowWaferList, CubeWaferCoordinateList aColWaferList, bool aBlockStatus)
        {
            Hashtable blockedHash;

            _asrtCubeGroup.SetCellBlockStatus(
                            aCommonWaferList: aCommonWaferList,
                            aRowWaferList: aRowWaferList,
                            aColWaferList: aColWaferList,
                            aBlockStatus: aBlockStatus);

            _asrtCubeGroup.RecomputeCubes(true);
            SaveDetailCubeGroup();
            blockedHash = (Hashtable)_asrtCubeGroup.BlockedList.Clone();
            CloseAndReOpenCubeGroup();
            _asrtCubeGroup.BlockedList = blockedHash;
            UpdateData(reload: true, rebuildComponents: false, reloadBlockedCells: false);
            LoadCurrentPages();
        }

        private void CascadeUnlockTotalVariables(AssortmentCellReference cellRef, int varRid)
        {
            ComputationCellReference copyCellRef = null;
            foreach (AssortmentTotalVariableProfile varProf in _asrtCubeGroup.AssortmentComputations.AssortmentTotalVariables.VariableProfileList)
            {
                if (varProf.Key != varRid)
                {
                    copyCellRef = (AssortmentCellReference)cellRef.Copy();
                    copyCellRef[eProfileType.AssortmentTotalVariable] = varProf.Key;

                    if (!copyCellRef.isCellReadOnly && !copyCellRef.isCellDisplayOnly && !copyCellRef.isCellProtected && !copyCellRef.isCellClosed && !copyCellRef.isCellIneligible && copyCellRef.isCellLocked)    // TT#1770-MD - JSmith - GA - Matrix Tab-Detail Section - Cascade Unlock Column Total Units - Unlocked the Total Units column and the 1st row in the adjacent column for average units.  Only expect the Total Unit colum to be Unlocked.
                    {
                        _asrtCubeGroup.SetCellRecursiveLockStatus(copyCellRef, false);
                    }
                }
            }

        }

        #endregion

        #region Get Assortment Review Matrix - Change View

        internal ROOut GetAssortmentReviewMatrixViewData(int newViewId)
        {

            ViewSelectionChange(newViewId);


            return new ROGridData(eROReturnCode.Successful, null, ROInstanceID, _asrtMatrixROData, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        #endregion

        #region Save Assortment Review Matrix Changes

        private ROOut SaveAssortmentReviewChanges(RONoParms savOptionsParms)
        {
            bool headerChanged;
            bool placeholderChanged;

            try
            {
                headerChanged = ((AssortmentCubeGroup)_asrtCubeGroup).hasHeaderCubeChanged();
                placeholderChanged = ((AssortmentCubeGroup)_asrtCubeGroup).hasPlaceholderCubeChanged();

                if (headerChanged || placeholderChanged)
                {
                    _asrtCubeGroup.SaveCubeGroup();
                }

                _asrtCubeGroup.SaveBlockedStyles();

                OnAssortmentSaveHeaderData(null);
            }
            catch (Exception exc)
            {
                //HandleExceptions(exc);
            }
            return new ROBoolOut(eROReturnCode.Successful, null, ROInstanceID, true);

        }

        #region Reference Methods

        void OnAssortmentSaveHeaderData(ROAssortmentReviewSaveOptionsParms parms)
        {
            try
            {
                
                _userGridView = new UserGridView();

                SaveGridChanges();

                int viewRid = Include.NoRID;

            }
            catch (Exception ex)
            {
                throw;
                //HandleExceptions(ex);
            }
        }

        private void SaveGridChanges()
        {
            int hdrRID;
            try
            {

                bool success = true;

                AllocationHeaderProfileList updateHeaderList = (AllocationHeaderProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AllocationHeader);
                List<int> hdrRIDs = new List<int>();

                if (success)
                {
                    
                    foreach (AllocationHeaderProfile ahp in updateHeaderList)
                    {
                        success = WriteHeader(ahp.Key);
                        hdrRIDs.Add(ahp.Key);
                        if (!success)
                        {
                            break;
                        }
                    }
                }

                if (_allocProfileList != null)
                {
                    foreach (AllocationProfile ap in _allocProfileList)
                    {
                        if (!hdrRIDs.Contains(ap.Key))
                        {
                            success = WriteHeader(ap.Key);
                            hdrRIDs.Add(ap.Key);
                            if (!success)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }


        private bool WriteHeader(int aKey)
        {
            AllocationProfile ap;
            DataRow dr = null;
            try
            {
                ap = _applicationSessionTransaction.GetAssortmentMemberProfile(aKey);

                if (ap == null)
                {
                    ap = GetAllocationProfile(aKey);
                }

                if (ap != null && ap.HeaderID != null)
                {
                    AssortmentProfile asp;
                    if (!ap.Placeholder && !ap.Assortment && ap.AsrtRID != Include.NoRID && !ap.AllocationStarted)
                    {
                        asp = (AssortmentProfile)_allocProfileList.FindKey(ap.AsrtRID);

                        if (IsGroupAllocation)
                        {
                            ap.ShipToDay = asp.ShipToDay;
                        }
                        else
                        {

                            ap.ShipToDay = asp.AssortmentApplyToDate.Date;  // apply to all headers
                            ap.BeginDay = asp.AssortmentBeginDay.Date;
                        }
                    }

                    if (!ap.WriteHeader())
                    {
                        EnqueueError(ap);
                        return false;
                    }

                    if (aKey != ap.Key)
                    {   // this was a new header

                        _allocProfileList.HashRebuild();

                        //if (ap.Assortment)
                        //{
                        //    UpdateAssortmentKeys(aKey, ap.Key);
                        //}
                        //else if (ap.Placeholder)
                        //{
                        //    UpdatePlaceholderKeys(aKey, ap.Key);
                        //}

                        //if ((ap.Packs.Count > 0 || ap.BulkColors.Count > 0))
                        //{
                        //    UpdateDataRowSizeKeys(aKey, ap);	
                        //}

                        if (aKey < 0 && _allocProfileList.Contains(aKey))
                        {
                            _allocProfileList.Remove(GetAllocationProfile(aKey));
                        }

                        asp = (AssortmentProfile)_allocProfileList.FindKey(ap.AsrtRID);


                        if (IsGroupAllocation)
                        {

                            if (ap.HeaderType != eHeaderType.Placeholder)
                            {
                                ap.ShipToDay = asp.ShipToDay;

                            }
                        }
                        else
                        {
                            ap.ShipToDay = asp.AssortmentApplyToDate.Date;  // apply to all headers
                            ap.BeginDay = asp.AssortmentBeginDay.Date;
                        }

                        if (ap.Placeholder)
                        {
                            ap.PlaceHolderRID = Include.NoRID;
                        }

                        if (!ap.WriteHeader())
                        {
                            EnqueueError(ap);
                            return false;
                        }
                    }

                    if (ap.HeaderType != eHeaderType.Assortment)
                    {
                        ReloadProfileToGrid(ap.Key);
                    }

                }
                return true;
            }
            catch (MIDException)
            {
                //HandleMIDException(MIDexc);
                return false;
            }
            catch (Exception)
            {
                //HandleException(ex);
                return false;
            }
        }
        
        private void ReloadProfileToGrid(int aHdrRID)
        {
            try
            {

                AllocationProfile ap = (AllocationProfile)this._allocProfileList.FindKey(aHdrRID);
                if (ap == null)
                {
                    AllocationHeaderProfile ahp = SAB.HeaderServerSession.GetHeaderData(aHdrRID, true, true, true);
                    ap = new AllocationProfile(_applicationSessionTransaction, ahp.HeaderID, ahp.Key, SAB.ClientServerSession);
                    _allocProfileList.Add(ap);
                }


                AllocationHeaderProfile ahp2 = (AllocationHeaderProfile)_headerList.FindKey(aHdrRID);
                if (ahp2 != null)
                {
                    _headerList.Remove(ahp2);
                }
                ahp2 = SAB.HeaderServerSession.GetHeaderData(aHdrRID, true, true, true);
                if (ahp2.AsrtRID != Include.NoRID || ahp2.Assortment)
                {
                    _headerList.Add(ahp2);
                }

                WorkflowBaseData workflowData = new WorkflowBaseData();
                string workflowMethodStr = string.Empty;
                int masterRID = Include.NoRID;
                int subordRID = Include.NoRID;

                string masterID = String.Empty;
                string subordID = String.Empty;
                string msgMasterSubord = String.Empty;

                if (ap.WorkflowRID > Include.UndefinedWorkflowRID)
                {
                    workflowMethodStr = workflowData.GetWorkflowName(ap.WorkflowRID);
                }
                else
                {
                    ApplicationBaseMethod abm = (ApplicationBaseMethod)GetAsrtMethods.GetUnknownMethod(ap.MethodRID, false);
                    if (abm.Key > Include.UndefinedMethodRID)
                    {
                        workflowMethodStr = abm.Name;
                    }
                }

                string APIworkflowMethodStr = string.Empty;
                if (ap.API_WorkflowRID > Include.UndefinedWorkflowRID)
                {
                    APIworkflowMethodStr = (string)_workflowNameHash[ap.API_WorkflowRID];
                    if (APIworkflowMethodStr == null)
                    {
                        APIworkflowMethodStr = workflowData.GetWorkflowName(ap.API_WorkflowRID);
                        _workflowNameHash.Add(ap.API_WorkflowRID, APIworkflowMethodStr);
                    }
                }

                subordRID = ap.SubordinateRID;

                if (subordRID != Include.NoRID)
                {
                    subordID = ap.SubordinateID;
                    if (subordID != null && subordID != string.Empty)
                    {
                        msgMasterSubord = ap.HeaderID + " / " + subordID;
                    }
                }
                else
                {
                    masterRID = ap.MasterRID;
                    if (masterRID != Include.NoRID)
                    {
                        masterID = ap.MasterID;
                        if (masterID != null && masterID != string.Empty)
                        {
                            msgMasterSubord = masterID + " / " + ap.HeaderID;
                        }
                    }
                }

                //DataRow dr = _dsMain.Tables["Header"].Rows.Find(aHdrRID);
                ////DataRow dr = _dtHeaders.Rows.Find(aHdrRID); //HB re-visit this ?????
                //if (dr != null)
                //{
                //    dr["Type"] = Convert.ToInt32(ap.HeaderType, CultureInfo.CurrentUICulture);
                //    dr["Status"] = Convert.ToInt32(ap.HeaderAllocationStatus, CultureInfo.CurrentUICulture);
                //    dr["Intransit"] = Convert.ToInt32(ap.HeaderIntransitStatus, CultureInfo.CurrentUICulture);
                //    dr["ShipStatus"] = Convert.ToInt32(ap.HeaderShipStatus, CultureInfo.CurrentUICulture);
                //    dr["HdrQuantity"] = ap.TotalUnitsToAllocate;
                //    dr["UnitRetail"] = ap.UnitRetail;
                //    dr["UnitCost"] = ap.UnitCost;
                //    dr["Multiple"] = ap.AllocationMultiple;
                //    dr["PO"] = ap.PurchaseOrder;
                //    dr["Vendor"] = ap.Vendor;
                //    dr["Workflow"] = workflowMethodStr;
                //    dr["APIWorkflow"] = APIworkflowMethodStr;
                //    dr["DC"] = ap.DistributionCenter;
                //    if (ap.ReleaseDate == Include.UndefinedDate)
                //    {
                //        dr["Release"] = string.Empty;
                //    }
                //    else
                //    {
                //        dr["Release"] = ap.ReleaseDate.ToShortDateString();
                //    }
                //    dr["Master"] = msgMasterSubord;


                //    dr["AllocatedUnits"] = ap.AllocatedUnits; //TotalUnitsAllocated(ap); this method has just one line so replaced it here.

                //    dr["OrigAllocatedUnits"] = ap.OrigAllocatedUnits;
                //    dr["RsvAllocatedUnits"] = ap.RsvAllocatedUnits;

                //    if ((!ap.MultiHeader) && (ap.Packs.Count > 0 || ap.BulkColors.Count > 0))
                //    {
                //        UpdateDataRowSizeKeys(ap.Key, ap);
                //    }

                //    if (ap.WorkUpTotalBuy || ap.Placeholder)
                //    {
                //        int hdrChildTotal = 0;

                //        if ((ap.BulkColors != null && ap.BulkColors.Count > 0) || (ap.Packs != null && ap.Packs.Count > 0))
                //        {
                //            ReloadWorkUpTotalBuy(ap, ref hdrChildTotal);
                //            dr["ChildTotal"] = hdrChildTotal;
                //            int hdrQty = Convert.ToInt32(dr["HdrQuantity"]);
                //            dr["Balance"] = Convert.ToInt32(dr["HdrQuantity"], CultureInfo.CurrentUICulture) - hdrChildTotal;
                //        }
                //    }

                //}

                //_dsMain.Tables["Header"].AcceptChanges();	//????? re-visit this later
                //_dtHeaders.AcceptChanges();
            }
            catch (Exception ex)
            {
                // HandleException(ex);
            }
            finally
            {

            }

        }

        private void UpdateDataRowSizeKeys(int aOldHdrKey, AllocationProfile ap)
        {
            try
            {
                string oldSizeTableName = string.Empty;
                string newSizeTableName = string.Empty;
                int oldPackKey = 0, oldColorCodeRID = 0;
                if (ap.Packs.Count > 0)
                {
                    object[] Keys = new object[2];
                    Keys[0] = ap.Key;
                    foreach (PackHdr aPack in ap.Packs.Values)
                    {
                        Keys[1] = aPack.PackName;
                        DataRow drPack = _dsMain.Tables["Pack"].Rows.Find(Keys);
                        if (drPack != null)
                        {
                            oldPackKey = (int)drPack["KeyP"];
                            if (oldPackKey < 0)
                            {
                                oldSizeTableName = "PackSize" + "~" + aOldHdrKey.ToString(CultureInfo.CurrentUICulture)
                                                       + "~" + oldPackKey.ToString(CultureInfo.CurrentUICulture)
                                                       + "~" + Include.DummyColorRID.ToString(CultureInfo.CurrentUICulture);
                                newSizeTableName = "PackSize" + "~" + ap.Key.ToString(CultureInfo.CurrentUICulture)
                                                       + "~" + aPack.PackRID.ToString(CultureInfo.CurrentUICulture)
                                                       + "~" + Include.DummyColorRID.ToString(CultureInfo.CurrentUICulture);
                                drPack["KeyP"] = aPack.PackRID;
                                drPack["Sequence"] = aPack.Sequence;
                                //_dsDetails.Tables["Pack"].AcceptChanges(); ?????Re-visit this 

                                if (ap.Placeholder)
                                {
                                    UpdateAssortmentLinkedPacks(ap.Key, oldPackKey, aPack.PackRID);
                                }
                            }
                            if (aPack.PackColors != null && aPack.PackColors.Count > 0)
                            {
                                UpdatePackColorCodeRIDs(aOldHdrKey, ap.Key, oldPackKey, aPack);
                            }
                        }
                    }
                }

                if (ap.BulkColors.Count > 0)
                {
                    object[] Keys = new object[2];
                    Keys[0] = ap.Key;
                    foreach (HdrColorBin aColor in ap.BulkColors.Values)
                    {
                        Keys[1] = aColor.ColorCodeRID;
                        DataRow drColor = _dsMain.Tables["BulkColor"].Rows.Find(Keys);
                        if (drColor != null)
                        {
                            if ((int)drColor["KeyC"] < 0)
                            {
                                oldColorCodeRID = (int)drColor["KeyC"];
                                oldSizeTableName = "BulkSize" + "~" + aOldHdrKey.ToString(CultureInfo.CurrentUICulture)
                                                       + "~" + oldColorCodeRID.ToString(CultureInfo.CurrentUICulture);
                                newSizeTableName = "BulkSize" + "~" + ap.Key.ToString(CultureInfo.CurrentUICulture)
                                                       + "~" + aColor.HdrBCRID.ToString(CultureInfo.CurrentUICulture);

                                drColor["KeyC"] = aColor.HdrBCRID;
                                drColor["Sequence"] = aColor.ColorSequence;
                                ReplaceSizeTable("BulkSize", oldSizeTableName, newSizeTableName);
                                if (ap.Placeholder)
                                {
                                    UpdateAssortmentLinkedColors(ap.Key, oldColorCodeRID, aColor.HdrBCRID);
                                }

                            }
                        }
                    }
                }

            }
            catch
            {
                throw;
            }
        }

        private void ReloadWorkUpTotalBuy(AllocationProfile aAP, ref int hdrChildTotal)
        {
            int aHeaderRID = aAP.HeaderRID;
            hdrChildTotal = 0;
            try
            {
                object[] Keys = new object[2];
                if (aAP.Packs != null && aAP.Packs.Count > 0)
                {
                    Hashtable packs = aAP.Packs;
                    Keys[0] = aHeaderRID;
                    int packColorTotal;
                    int packSizeTotal;
                    DataTable packTable = _dsMain.Tables["Pack"];
                    DataTable packColorTable;
                    DataTable packSizeTable;
                    object[] packColorKeys = new object[3];
                    packColorKeys[0] = aHeaderRID;
                    foreach (PackHdr aPack in packs.Values)
                    {
                        Keys[1] = aPack.PackName;
                        packColorKeys[1] = aPack.PackRID;
                        hdrChildTotal += aPack.UnitsToAllocate;
                        packColorTotal = 0;
                        DataRow packRow = packTable.Rows.Find(Keys);

                        if (packRow == null)
                        {
                            int packType;
                            if (aPack.GenericPack)
                            {
                                packType = (int)eAllocationType.GenericType;
                            }
                            else
                            {
                                packType = (int)eAllocationType.DetailType;
                            }
                            packRow = packTable.Rows.Add(new object[] { aAP.HeaderRID, aPack.PackRID, Include.NoRID,
                                                          aPack.PackName, packType, aPack.PacksToAllocate,
                                                          aPack.PackMultiple, aPack.UnitsToAllocate,
                                                          aPack.AssociatedPackRID, aPack.Sequence});
                        }
                        if (aPack.PackColors != null && aPack.PackColors.Count > 0)
                        {
                            packColorTable = _dsMain.Tables["PackColor"];
                            foreach (PackColorSize aColor in aPack.PackColors.Values)
                            {
                                packColorTotal += aColor.ColorUnitsInPack;
                                packSizeTotal = 0;
                                packColorKeys[2] = aColor.ColorCodeRID;
                                DataRow colorRow = packColorTable.Rows.Find(packColorKeys);
                                ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(aColor.ColorCodeRID);
                                if (colorRow == null)
                                {
                                    string packColorDescription = string.Empty;
                                    string colorID = string.Empty;

                                    if (ccp.VirtualInd)
                                    {
                                        colorID = aColor.ColorName;

                                        if (colorID == null)
                                        {
                                            colorID = ccp.ColorCodeID;
                                        }
                                        packColorDescription = aColor.ColorDescription;
                                        if (packColorDescription == null)
                                        {
                                            packColorDescription = GetColorDescription(GetNodeData(aAP.StyleHnRID), ccp);
                                        }

                                    }
                                    else
                                    {
                                        colorID = ccp.ColorCodeID;
                                        packColorDescription = GetColorDescription(GetNodeData(aAP.StyleHnRID), ccp);
                                    }
                                    if (aColor.ColorCodeRID != Include.DummyColorRID)
                                    {
                                        colorRow = packColorTable.Rows.Add(new object[] { aAP.HeaderRID, aPack.PackRID,
                                                                      aColor.HdrPCRID, aColor.ColorCodeRID,
                                                                      colorID, packColorDescription,
                                                                      aColor.ColorUnitsInPack, aColor.ColorSequenceInPack, aColor.ColorName });
                                    }
                                }
                                if (aColor.ColorSizes != null && aColor.ColorSizes.Count > 0)
                                {
                                    string dtSizeNamePrefix = aColor.ColorCodeRID == Include.DummyColorRID ? "PackSize" : "PackColorSize";
                                    string sizeTableName = dtSizeNamePrefix
                                        + "~" + aAP.HeaderRID.ToString(CultureInfo.CurrentUICulture)
                                        + "~" + aPack.PackRID.ToString(CultureInfo.CurrentUICulture)
                                        + "~" + aColor.HdrPCRID.ToString(CultureInfo.CurrentUICulture);
                                    packSizeTable = _dsMain.Tables[sizeTableName];
                                    if (packSizeTable == null)
                                    {
                                        DataTable dtSizes = FormatPackSizeTable(aColor, aAP.SizeGroupRID, aAP.HeaderRID, aPack.PackRID, aColor.HdrPCRID, ccp.ColorCodeID, sizeTableName, ref packSizeTotal);
                                        if (dtSizes != null)
                                        {
                                            _dsMain.Tables.Add(dtSizes);
                                            if (aColor.ColorCodeRID == Include.DummyColorRID)
                                            {
                                                _dsMain.Relations.Add(sizeTableName,
                                                    new DataColumn[] { _dsMain.Tables["Pack"].Columns["KeyH"], _dsMain.Tables["Pack"].Columns["KeyP"] },
                                                    new DataColumn[] { _dsMain.Tables[sizeTableName].Columns["KeyH"], _dsMain.Tables[sizeTableName].Columns["KeyP"] }, true);
                                                packRow["ChildTotal"] = packSizeTotal;
                                                packRow["Balance"] = Convert.ToInt32(packRow["QuantityPerPack"], CultureInfo.CurrentUICulture) - packSizeTotal;

                                            }
                                            else
                                            {
                                                _dsMain.Relations.Add(sizeTableName,
                                                    new DataColumn[] { _dsMain.Tables["PackColor"].Columns["KeyH"], _dsMain.Tables["PackColor"].Columns["KeyP"], _dsMain.Tables["PackColor"].Columns["KeyC"] },
                                                    new DataColumn[] { _dsMain.Tables[sizeTableName].Columns["KeyH"], _dsMain.Tables[sizeTableName].Columns["KeyP"], _dsMain.Tables[sizeTableName].Columns["KeyC"] }, true);

                                                if (colorRow != null)
                                                {
                                                    colorRow["ChildTotal"] = packSizeTotal;
                                                    colorRow["Balance"] = Convert.ToInt32(colorRow["QuantityPerPack"], CultureInfo.CurrentUICulture) - packSizeTotal;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (PackContentBin aSize in aColor.ColorSizes.Values)
                                        {
                                            packSizeTotal += aSize.ContentUnits;
                                        }
                                    }
                                }

                                if (colorRow != null)
                                {
                                    colorRow["ChildTotal"] = packSizeTotal;
                                    colorRow["Balance"] = Convert.ToInt32(colorRow["QuantityPerPack"], CultureInfo.CurrentCulture) - packSizeTotal;
                                }

                                if (aColor.ColorCodeRID == Include.DummyColorRID)
                                {
                                    packColorTotal = packSizeTotal;
                                }
                            }
                            packRow["ChildTotal"] = packColorTotal;
                            packRow["Balance"] = Convert.ToInt32(packRow["QuantityPerPack"], CultureInfo.CurrentUICulture) - packColorTotal;
                        }

                    }
                    _dsMain.AcceptChanges();
                }
                if (aAP.BulkColors != null && aAP.BulkColors.Count > 0)
                {
                    Hashtable aBulkColors = aAP.BulkColors;
                    foreach (HdrColorBin aColor in aBulkColors.Values)
                    {
                        Keys[0] = aHeaderRID;
                        Keys[1] = aColor.ColorCodeRID;

                        DataRow drColor = _dsMain.Tables["BulkColor"].Rows.Find(Keys);
                        drColor["Quantity"] = aColor.ColorUnitsToAllocate;
                        hdrChildTotal += aColor.ColorUnitsToAllocate;

                        int sizeKey, rowTotal, childTotal = 0;
                        string primary, secondary;

                        if (aColor.ColorSizes != null && aColor.ColorSizes.Count > 0)
                        {
                            string sizeTableName = "BulkSize" + "~"
                                + aHeaderRID.ToString(CultureInfo.CurrentUICulture) + "~"
                                + aColor.HdrBCRID.ToString(CultureInfo.CurrentUICulture);

                            DataTable dtBulkSize = _dsMain.Tables[sizeTableName];

                            if (dtBulkSize == null)
                            {
                                bool addSizeTable = false;
                                
                                if (addSizeTable)
                                {
                                    dtBulkSize = _dsMain.Tables[sizeTableName];
                                }
                            }
                            foreach (DataRow dRow in dtBulkSize.Rows)
                            {
                                rowTotal = 0;
                                secondary = Convert.ToString(dRow[3], CultureInfo.CurrentUICulture);
                                for (int i = 7; i < dtBulkSize.Columns.Count; i++)
                                {
                                    DataColumn dCol = dtBulkSize.Columns[i];
                                    primary = dCol.ColumnName;
                                    sizeKey = Convert.ToInt32(dCol.ExtendedProperties[primary + "~" + secondary], CultureInfo.CurrentUICulture);

                                    if (aColor.ColorSizes.ContainsKey(sizeKey))
                                    {
                                        HdrSizeBin aSize = (HdrSizeBin)aColor.ColorSizes[sizeKey];
                                        dRow[i] = aSize.SizeUnitsToAllocate;
                                        rowTotal += aSize.SizeUnitsToAllocate;
                                    }
                                }
                                dRow["TotalQuantity"] = rowTotal;
                                childTotal += rowTotal;
                            }
                            drColor["ChildTotal"] = childTotal;
                            int hdrQty = Convert.ToInt32(drColor["Quantity"]);
                            drColor["Balance"] = Convert.ToInt32(drColor["Quantity"], CultureInfo.CurrentUICulture) - childTotal;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //HandleException(ex);
            }
        }

        private void UpdateAssortmentLinkedColors(int aPhKey, int aOldValue, int aNewValue)
        {
            try
            {
                
            }
            catch
            {
                throw;
            }
        }

        private void UpdateAssortmentLinkedPacks(int aPhKey, int aOldValue, int aNewValue)
        {
            try
            {
                
            }
            catch
            {
                throw;
            }
        }

        private void UpdatePackColorCodeRIDs(int aOldHdrKey, int aNewHdrRID, int aOldPackKey, PackHdr aPack)
        {
            try
            {
                string oldSizeTableName = string.Empty;
                string newSizeTableName = string.Empty;
                object[] pcKeys = new object[3];
                pcKeys[0] = aNewHdrRID;
                pcKeys[1] = aPack.PackRID;

                foreach (PackColorSize aColor in aPack.PackColors.Values)
                {
                    if (aColor.ColorCodeRID != Include.DummyColorRID)
                    {
                        pcKeys[2] = aColor.ColorCodeRID;
                        DataRow drPackColor = _dsMain.Tables["PackColor"].Rows.Find(pcKeys);
                        int oldPackColorRID = (int)drPackColor["KeyC"];

                        if (oldPackColorRID < 0)
                        {
                            oldSizeTableName = "PackColorSize" + "~" + aOldHdrKey.ToString(CultureInfo.CurrentUICulture)
                                                + "~" + aOldPackKey.ToString(CultureInfo.CurrentUICulture)
                                                + "~" + oldPackColorRID.ToString(CultureInfo.CurrentUICulture);
                            newSizeTableName = "PackColorSize" + "~" + aNewHdrRID.ToString(CultureInfo.CurrentUICulture)
                                                + "~" + aPack.PackRID.ToString(CultureInfo.CurrentUICulture)
                                                + "~" + aColor.HdrPCRID.ToString(CultureInfo.CurrentUICulture);
                            drPackColor["KeyC"] = aColor.HdrPCRID;
                            drPackColor["Sequence"] = aColor.ColorSequenceInPack;
                            ReplaceSizeTable("PackColorSize", oldSizeTableName, newSizeTableName);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void ReplaceSizeTable(string aBandKey, string aOldTableName, string aNewTableName)
        {
            try
            {
                if (_dsMain.Relations.Contains(aOldTableName))
                {
                    DataTable dtSize = _dsMain.Relations[aOldTableName].ChildTable.Copy();

                    _dsMain.Tables[aOldTableName].Constraints.Remove(aOldTableName);
                    _dsMain.Relations.Remove(aOldTableName);
                    _dsMain.Tables.Remove(aOldTableName);
                    _dsMain.AcceptChanges();

                    dtSize.TableName = aNewTableName;
                    _dsMain.Tables.Add(dtSize);

                    switch (aBandKey)
                    {
                        case "BulkSize":
                            _dsMain.Relations.Add(aNewTableName,
                                new DataColumn[] { _dsMain.Tables["BulkColor"].Columns["KeyH"], _dsMain.Tables["BulkColor"].Columns["KeyC"] },
                                new DataColumn[] { _dsMain.Tables[aNewTableName].Columns["KeyH"], _dsMain.Tables[aNewTableName].Columns["KeyC"] }, true);
                            break;

                        case "PackSize":
                            _dsMain.Relations.Add(aNewTableName,
                                new DataColumn[] { _dsMain.Tables["Pack"].Columns["KeyH"], _dsMain.Tables["Pack"].Columns["KeyP"] },
                                new DataColumn[] { _dsMain.Tables[aNewTableName].Columns["KeyH"], _dsMain.Tables[aNewTableName].Columns["KeyP"] }, true);
                            break;

                        case "PackColorSize":
                            _dsMain.Relations.Add(aNewTableName,
                              new DataColumn[] { _dsMain.Tables["PackColor"].Columns["KeyH"], _dsMain.Tables["PackColor"].Columns["KeyP"], _dsMain.Tables["PackColor"].Columns["KeyC"] },
                              new DataColumn[] { _dsMain.Tables[aNewTableName].Columns["KeyH"], _dsMain.Tables[aNewTableName].Columns["KeyP"], _dsMain.Tables[aNewTableName].Columns["KeyC"] }, true);
                            break;
                    }

                    _dsMain.AcceptChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        
        #endregion  


        #endregion
    }

    public class GridRow : IComparable
    {
        string[] _textCols;
        string[] _keyCols;
        string[] _textArray;
        int[] _keyArray;
        int[] _seqArray;
        DataRow _dataRow;
        // Placeholder Seq concatenated with Header Seq
        string _phSeqHdrSeq;

        int _compSeqCount;
        int i;

        public GridRow(DataRow aDataRow, string[] aTextCols, string[] aKeyCols)
        {
            int i;

            try
            {
                _textCols = aTextCols;
                _keyCols = aKeyCols;
                _dataRow = aDataRow;
                _compSeqCount = 0;

                _textArray = new string[aTextCols.Length];
                _keyArray = new int[aKeyCols.Length];
                _seqArray = new int[aKeyCols.Length];

                _phSeqHdrSeq = Convert.ToString(aDataRow["HEADERSEQ"]);

                for (i = 0; i < aTextCols.Length; i++)
                {
                    if (aDataRow[aTextCols[i]] == DBNull.Value)
                    {
                        _textArray[i] = " ";
                    }
                    else
                    {
                        _textArray[i] = Convert.ToString(aDataRow[aTextCols[i]]);
                    }

                    _keyArray[i] = Convert.ToInt32(aDataRow[aKeyCols[i]]);
                }
                //BEGIN TT#2502 - DOConnell - Nested Characteristics in Assortment not correct
                for (i = 0; i < _keyArray.Length; i++)
                {
                    // BEGIN TT#2503 - stodd - Cannot key in Avg Str Units in Assortment Tab
                    // This change here was actually to fix a sorting issue when no characteristics were present
                    if (_textCols[i] != "PLACEHOLDER" && _textCols[i] != "HEADER" && _textCols[i] != "PACK" && _textCols[i] != "PACK_ALTERNATE"
                        && _textCols[i] != "COLOR" && !_textCols[i].StartsWith("HIERARCHYLEVEL")
                        && _textCols[i] != "ASSORTMENT" && _textCols[i] != "PLANLEVEL")
                    {
                        _compSeqCount = _compSeqCount + 1;
                    }
                    // END TT#2503 - stodd - Cannot key in Avg Str Units in Assortment Tab
                }
                //END TT#2502 - DOConnell - Nested Characteristics in Assortment not correct
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public string[] TextCols
        {
            get
            {
                return _textCols;
            }
        }

        public string[] KeyCols
        {
            get
            {
                return _keyCols;
            }
        }

        public string[] TextArray
        {
            get
            {
                return _textArray;
            }
        }

        public int[] KeyArray
        {
            get
            {
                return _keyArray;
            }
        }

        // Begin TT#1438 - stodd - sorting issue
        /// <summary>
        /// Placeholder Seq concatenated with Header Seq.
        /// </summary>
        public string PlaceholderSeqHeaderSeq
        {
            get
            {
                return _phSeqHdrSeq;
            }
        }
        // End TT#1438 - stodd - sorting issue

        public DataRow DataRow
        {
            get
            {
                return _dataRow;
            }
        }

        public int CompareTo(object obj)
        {
            GridRow gridRow;
            int retCode;

            int drCSeq;
            int grCSeq;

            bool componentfirst = false;

            try
            {
                gridRow = (GridRow)obj;
                retCode = 0;

                for (i = 0; i < _textArray.Length && retCode == 0; i++)
                {
                    if (_keyArray[i] == int.MaxValue && gridRow._keyArray[i] != int.MaxValue)
                    {
                        retCode = -1;
                    }
                    else if (_keyArray[i] != int.MaxValue && gridRow._keyArray[i] == int.MaxValue)
                    {
                        retCode = 1;
                    }
                    else if (_keyArray[i] == int.MaxValue && gridRow._keyArray[i] == int.MaxValue)
                    {
                        //retCode = 0;
                        string data = _textArray[i] + _phSeqHdrSeq;
                        string gridRowData = gridRow._textArray[i] + gridRow.PlaceholderSeqHeaderSeq;
                        retCode = data.CompareTo(gridRowData);
                    }
                    else
                    {

                        if (_textCols[i] == "PLACEHOLDER")
                        {
                            componentfirst = true;
                            int iSeq = int.Parse(_dataRow["PLACEHOLDERSEQ_RID"].ToString());
                            string seq = iSeq.ToString("0000") + _phSeqHdrSeq;
                            int iGridSeq = int.Parse(gridRow.DataRow["PLACEHOLDERSEQ_RID"].ToString());
                            string gridRowSeq = iGridSeq.ToString("0000") + gridRow.PlaceholderSeqHeaderSeq;
                            // End TT#1335 - stodd more sorting issues
                            //int seq = Convert.ToInt32(_dataRow["PLACEHOLDERSEQ_RID"]);
                            //int gridRowSeq = Convert.ToInt32(gridRow.DataRow["PLACEHOLDERSEQ_RID"]);
                            // End TT#1461 - stodd - sorting issue
                            retCode = seq.CompareTo(gridRowSeq);
                        }
                        else if (_textCols[i] == "HEADER")
                        {
                            componentfirst = true;
                            int iSeq = int.Parse(_dataRow["HEADERSEQ_RID"].ToString());
                            string seq = iSeq.ToString("0000") + _phSeqHdrSeq;
                            int iGridSeq = int.Parse(gridRow.DataRow["HEADERSEQ_RID"].ToString());
                            string gridRowSeq = iGridSeq.ToString("0000") + gridRow.PlaceholderSeqHeaderSeq;
                            // End TT#1335 - stodd more sorting issues
                            //int seq = Convert.ToInt32(_dataRow["HEADERSEQ_RID"]);
                            //int gridRowSeq = Convert.ToInt32(gridRow.DataRow["HEADERSEQ_RID"]);
                            // End TT#1461 - stodd - sorting issue
                            retCode = seq.CompareTo(gridRowSeq);
                        }
                        else
                        {
                            //BEGIN TT#2502 - DOConnell - Nested Characteristics in Assortment not correct
                            string gridRowData = null;
                            string data = null;

                            if (_textCols[i] == "COLOR")
                            {
                                //BEGIN TT#1537 - MD - DOConnell - ASST - Added 2 detail ppks to a PH and received null reference errors. 
                                if (_dataRow["COLORSEQ"] == null || _dataRow["COLORSEQ"].ToString() == "")
                                {
                                    drCSeq = -1;    // TT#1544-MD - stodd - When the header column is selected in the column chooser it is not appearing on the Matrix Tab.
                                }
                                else
                                {
                                    drCSeq = int.Parse(_dataRow["COLORSEQ"].ToString());
                                }

                                if (gridRow.DataRow["COLORSEQ"] == null || gridRow.DataRow["COLORSEQ"].ToString() == "")
                                {
                                    grCSeq = -1;    // TT#1544-MD - stodd - When the header column is selected in the column chooser it is not appearing on the Matrix Tab.
                                }
                                else
                                {
                                    grCSeq = int.Parse(gridRow.DataRow["COLORSEQ"].ToString());
                                }


                                if (i > 0)
                                {
                                    data = drCSeq.ToString() + _textArray[i];
                                    gridRowData = grCSeq.ToString() + gridRow._textArray[i];
                                }
                                else
                                {
                                    data = _textArray[i];
                                    gridRowData = gridRow._textArray[i];
                                }

                            }
                            else
                            {

                                data = _textArray[i];
                                gridRowData = gridRow._textArray[i];
                                // End TT#1992-MD - JSmith - Received system argument exception when rordering components
                            }



                            retCode = data.CompareTo(gridRowData);
                            if (retCode == 0
                                && i == _textArray.Length - 1)
                            {
                                retCode = _phSeqHdrSeq.CompareTo(gridRow.PlaceholderSeqHeaderSeq);
                            }

                        }
                        // End TT#1322 - stodd - sorting
                    }
                }

                return retCode;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
    }

    public class CellStyleUserData
    {
        private CellStyle _blockedStyle;
        private CellStyle _lockedStyle;
        private CellStyle _negativeStyle;
        private CellStyle _editableStyle;
        private CellStyle _negativeEditableStyle;

        public CellStyleUserData(CellStyle aBlockedStyle, CellStyle aLockedStyle, CellStyle aNegativeStyle, CellStyle aEditableStyle, CellStyle aNegativeEditableStyle)
        {
            _blockedStyle = aBlockedStyle;
            _lockedStyle = aLockedStyle;
            _negativeStyle = aNegativeStyle;
            _editableStyle = aEditableStyle;
            _negativeEditableStyle = aNegativeEditableStyle;
        }

        public CellStyle BlockedStyle
        {
            get
            {
                return _blockedStyle;
            }
        }

        public CellStyle LockedStyle
        {
            get
            {
                return _lockedStyle;
            }
        }

        public CellStyle NegativeStyle
        {
            get
            {
                return _negativeStyle;
            }
        }

        public CellStyle EditableStyle
        {
            get
            {
                return _editableStyle;
            }
        }

        public CellStyle NegativeEditableStyle
        {
            get
            {
                return _negativeEditableStyle;
            }
        }
    }


}
