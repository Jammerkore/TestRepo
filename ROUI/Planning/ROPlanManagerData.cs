using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Collections;

using C1.Win.C1FlexGrid;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using Logility.ROWebSharedTypes;

namespace Logility.ROUI
{
    /// <summary>
    /// Data that is defined at the manager level and used in the view
    /// This data is set only once per instance of the ladder screen
    /// </summary>
    abstract public class ROPlanManagerData : ROManagerData
    {
        private PlanOpenParms _openParms;
        private SessionAddressBlock _SAB;
        private PlanCubeGroup _planCubeGroup;
        private ApplicationSessionTransaction _transaction;
        private string _dollarScalingString = "1";
        private string _unitsScalingString = "1";

        public ROPlanManagerData(SessionAddressBlock aSAB, PlanOpenParms aOpenParms)
        {
            _openParms = aOpenParms;
            _SAB = aSAB;
        }

        public SessionAddressBlock SAB { get { return _SAB; } private set { } }

        public PlanOpenParms OpenParms { get { return _openParms; } private set { } }

        public PlanCubeGroup PlanCubeGroup { get { return _planCubeGroup; } set { _planCubeGroup = value; } }
 
        public ApplicationSessionTransaction Transaction { get { return _transaction; } set { _transaction = value;  } }

        public string DollarScalingString { get { return _dollarScalingString; } set { _dollarScalingString = value; } }

        public string UnitsScalingString { get { return _unitsScalingString; } set { _unitsScalingString = value; } }

    }

    /// <summary>
    /// Data that is defined per view
    /// </summary>
    abstract public class ROPlanViewData : ROViewData
    {
        private const bool THREADED_GRID_LOAD = false;
        private const int BIGCHANGE = 5;
        private const int SMALLCHANGE = 1;
        private const int ROWPAGESIZE = 99999;
        private const int COLPAGESIZE = 99999;
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
        private const int Grid10 = 9;
        private const int Grid11 = 10;
        private const int Grid12 = 11;

        private C1.Win.C1FlexGrid.C1FlexGrid g1;
        private C1.Win.C1FlexGrid.C1FlexGrid g2;
        private C1.Win.C1FlexGrid.C1FlexGrid g3;
        private C1.Win.C1FlexGrid.C1FlexGrid g4;
        private C1.Win.C1FlexGrid.C1FlexGrid g5;
        private C1.Win.C1FlexGrid.C1FlexGrid g6;
        private C1.Win.C1FlexGrid.C1FlexGrid g7;
        private C1.Win.C1FlexGrid.C1FlexGrid g8;
        private C1.Win.C1FlexGrid.C1FlexGrid g9;
        private C1.Win.C1FlexGrid.C1FlexGrid g10;
        private C1.Win.C1FlexGrid.C1FlexGrid g11;
        private C1.Win.C1FlexGrid.C1FlexGrid g12;

        private FunctionSecurityProfile _forecastBalanceSecurity;
        private ROPlanManagerData _managerData;
        private ProfileList _storeProfileList;
        private ProfileList _storeGroupListViewProfileList;
        private ProfileList _storeGroupLevelProfileList;
        private ProfileList _workingDetailProfileList;
        private ProfileList _variableProfileList;
        private ProfileList _quantityVariableProfileList;
        private ProfileList _versionProfileList;
        private ProfileList _periodProfileList;
        private IPlanComputationQuantityVariables _quantityVariables;
        private FunctionSecurityProfile _functionSecurity;
        private GroupedBy _columnGroupedBy;
        private RowColProfileHeader _adjustmentRowHeader;
        private RowColProfileHeader _originalRowHeader;
        private RowColProfileHeader _currentRowHeader;
        private ROCell[][,] _gridData;
        private object _holdValue;
        private structSort _currSortParms;
        private CubeWaferCoordinateList _commonWaferCoordinateList;
        private PlanViewData _planViewData;
        private DataTable _planViewDetail;
        private Hashtable _basisLabelList;
        private string _basisLabel = null;
        private ArrayList _selectableQuantityHeaders;
        private ArrayList _selectableStoreAttributeHeaders;
        private ArrayList _selectableVariableHeaders;
        private ArrayList _selectableTimeHeaders;
        private ArrayList _selectableBasisHeaders;
        private ArrayList _selectablePeriodHeaders;
        private ArrayList _cmiBasisList;
        private PlanViewData _viewDL;
        private DataTable _dtView;
        private ArrayList _filterUserRIDList;
        private ArrayList _viewUserRIDList;
        private SortedList _sortedVariableHeaders;
        private SortedList _sortedTimeHeaders;
        private Hashtable _periodHeaderHash;
        private bool _storeReadOnly;
        private bool _chainReadOnly;
        private bool _lowLevelStoreReadOnly;
        private bool _lowLevelChainReadOnly;
        private int _productDisplayCombination;
        private int _storeDisplayCombination;
        private int _lastFilterKey;
        private int _lastStoreAttributeKey;
        private int _lastStoreAttributeSetKey;
        private List<ColumnHeaderTag> _g2ColHeaderList = new List<ColumnHeaderTag>();
        private List<ColumnHeaderTag> _g3ColHeaderList = new List<ColumnHeaderTag>(); 
        private List<RowHeaderTag> _g4RowHeaderList = new List<RowHeaderTag>(); 
        private List<RowHeaderTag> _g10RowHeaderList = new List<RowHeaderTag>();
        
        private List<RowHeaderTag> _g7RowHeaderList = new List<RowHeaderTag>();
        private int _timeTotVarsPerGroup;
        private PlanProfile _currentStorePlanProfile;
        private PlanProfile _currentChainPlanProfile;



        public ROPlanViewData(ROPlanManagerData managerData)
        {
            _managerData = managerData;
        }

        public ROPlanManagerData ManagerData { get { return _managerData; } private set { } }

        public FunctionSecurityProfile FunctionSecurity { get { return _functionSecurity; } private set { } }

        public ApplicationSessionTransaction Transaction { get { return ManagerData.Transaction; } private set { } }

        public PlanCubeGroup PlanCubeGroup { get { return ManagerData.PlanCubeGroup; } private set { } }

        public ROCell[,] DataGrid1 { get { return _gridData[Grid1]; } }

        public ROCell[,] DataGrid2 { get { return _gridData[Grid2]; } }

        public ROCell[,] DataGrid3 { get { return _gridData[Grid3]; } }

        public ROCell[,] DataGrid4 { get { return _gridData[Grid4]; } }

        public ROCell[,] DataGrid5 { get { return _gridData[Grid5]; } }

        public ROCell[,] DataGrid6 { get { return _gridData[Grid6]; } }

        public ROCell[,] DataGrid7 { get { return _gridData[Grid7]; } }

        public ROCell[,] DataGrid8 { get { return _gridData[Grid8]; } }

        public ROCell[,] DataGrid9 { get { return _gridData[Grid9]; } }

        public ROCell[,] DataGrid10 { get { return _gridData[Grid10]; } }

        public ROCell[,] DataGrid11 { get { return _gridData[Grid11]; } }

        public ROCell[,] DataGrid12 { get { return _gridData[Grid12]; } }

        public List<ColumnHeaderTag> g2ColHeaders { get { return _g2ColHeaderList; } }
        public List<ColumnHeaderTag> g3ColHeaders { get { return _g3ColHeaderList; } }
        public List<RowHeaderTag> g4RowHeaders { get { return _g4RowHeaderList; } }
        public List<RowHeaderTag> g10RowHeaders { get { return _g10RowHeaderList; } }

        public List<RowHeaderTag> g7RowHeaders { get { return _g7RowHeaderList; } }

        public int TimeTotVarsPerGroup { get { return _timeTotVarsPerGroup; } }

        public int LastFilterKey { get { return _lastFilterKey; } set { _lastFilterKey = value; } }
        public int LastStoreAttributeSetKey { get { return _lastStoreAttributeSetKey; } set { _lastStoreAttributeSetKey = value; } }
        public int LastStoreAttributeKey { get { return _lastStoreAttributeKey; } set { _lastStoreAttributeKey = value; } }
        public void Initialize()
        {
            int i;
            int userRID;
            ProfileList basisProfList;

#if (DEBUG)
            DateTime startTime;
            DateTime startTime2;
#endif

            try
            {
#if (DEBUG)

                startTime = DateTime.Now;
                startTime2 = DateTime.Now;
#endif
                g1 = new C1.Win.C1FlexGrid.C1FlexGrid();
                g2 = new C1.Win.C1FlexGrid.C1FlexGrid();
                g3 = new C1.Win.C1FlexGrid.C1FlexGrid();
                g4 = new C1.Win.C1FlexGrid.C1FlexGrid();
                g5 = new C1.Win.C1FlexGrid.C1FlexGrid();
                g6 = new C1.Win.C1FlexGrid.C1FlexGrid();
                g7 = new C1.Win.C1FlexGrid.C1FlexGrid();
                g8 = new C1.Win.C1FlexGrid.C1FlexGrid();
                g9 = new C1.Win.C1FlexGrid.C1FlexGrid();
                g10 = new C1.Win.C1FlexGrid.C1FlexGrid();
                g11 = new C1.Win.C1FlexGrid.C1FlexGrid();
                g12 = new C1.Win.C1FlexGrid.C1FlexGrid();
                g1.Name = "g1";
                g2.Name = "g2";
                g3.Name = "g3";
                g4.Name = "g4";
                g5.Name = "g5";
                g6.Name = "g6";
                g7.Name = "g7";
                g8.Name = "g8";
                g9.Name = "g9";
                g10.Name = "g0";
                g11.Name = "g11";
                g12.Name = "g12";

                switch (ManagerData.OpenParms.PlanSessionType)
                {
                    case ePlanSessionType.StoreSingleLevel:
                        FunctionSecurity = ManagerData.SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastSingleLevelStore);
                        break;

                    case ePlanSessionType.StoreMultiLevel:
                        FunctionSecurity = ManagerData.SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMultiLevelStore);
                        break;

                    case ePlanSessionType.ChainSingleLevel:
                        FunctionSecurity = ManagerData.SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastSingleLevelChain);
                        break;

                    case ePlanSessionType.ChainMultiLevel:
                        FunctionSecurity = ManagerData.SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMultiLevelChain);
                        break;

                    default:
                        throw new Exception("Function not currently supported.");
                }

                _gridData = new ROCell[12][,];
                _commonWaferCoordinateList = new CubeWaferCoordinateList();
                _planViewData = new PlanViewData();

                if (ManagerData.OpenParms.GroupBy == eStorePlanSelectedGroupBy.ByVariable)
                //_columnGroupedBy = GroupedBy.GroupedByTime;
                {
                    _columnGroupedBy = GroupedBy.GroupedByVariable;
                }
                else
                {
                    _columnGroupedBy = GroupedBy.GroupedByTime;
                }

                // Create the PlanOpenParms object to open the Plan with

                _versionProfileList = ManagerData.SAB.ClientServerSession.GetUserForecastVersions();

                _forecastBalanceSecurity = ManagerData.SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastForecastBalance);
                FunctionSecurityProfile filterUserSecurity = ManagerData.SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreUser);
                FunctionSecurityProfile filterGlobalSecurity = ManagerData.SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreGlobal);
                FunctionSecurityProfile viewUserSecurity = ManagerData.SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastViewsUser);
                FunctionSecurityProfile viewGlobalSecurity = ManagerData.SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastViewsGlobal);

                _basisLabel = LoadBasisLabel();

                _filterUserRIDList = new ArrayList();

                _filterUserRIDList.Add(-1);

                if (filterUserSecurity.AllowView)
                {
                    _filterUserRIDList.Add(ManagerData.SAB.ClientServerSession.UserRID);
                }

                if (filterGlobalSecurity.AllowView)
                {
                    _filterUserRIDList.Add(Include.GlobalUserRID);
                }

                _viewUserRIDList = new ArrayList();

                _viewUserRIDList.Add(-1);

                if (viewUserSecurity.AllowView)
                {
                    _viewUserRIDList.Add(ManagerData.SAB.ClientServerSession.UserRID);
                }

                if (viewGlobalSecurity.AllowView)
                {
                    _viewUserRIDList.Add(Include.GlobalUserRID);
                }

                // Load the views

                _viewDL = new PlanViewData();
                _dtView = _viewDL.PlanView_Read(_viewUserRIDList);

                _dtView.Columns.Add(new DataColumn("DISPLAY_ID", typeof(string)));

                // Begin TT#1125 - JSmith - Global/User should be consistent
                //foreach (DataRow row in _dtView.Rows)
                //{
                //    if (Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture) != Include.GlobalUserRID)
                //    {
                //        row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture) + " (User)";
                //    }
                //    else
                //    {
                //        row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture);
                //    }
                //}
                foreach (DataRow row in _dtView.Rows)
                {
                    userRID = Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture);
                    if (userRID != Include.GlobalUserRID)
                    {
                        //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                        //row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture) + " (" + secAdmin.GetUserName(userRID) + ")";
                        row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture) + " (" + UserNameStorage.GetUserName(userRID) + ")";
                        //End TT#827-MD -jsobek -Allocation Reviews Performance
                    }
                    else
                    {
                        row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture);
                    }
                }

                // Create Basis Tooltips

                //_basisToolTipList = new Hashtable();
                _basisLabelList = new Hashtable();

                switch (ManagerData.OpenParms.PlanSessionType)
                {
                    case ePlanSessionType.StoreSingleLevel:

                        //basisProfList = ManagerData.OpenParms.GetBasisProfileList(_planCubeGroup, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key);

                        foreach (BasisProfile basisProfile in ManagerData.OpenParms.BasisProfileList)
                        {
                            BuildBasisLabelsAndToolTip(ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile, basisProfile);
                        }

                        //basisProfList = ManagerData.OpenParms.GetBasisProfileList(_planCubeGroup, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key);

                        foreach (BasisProfile basisProfile in ManagerData.OpenParms.BasisProfileList)
                        {
                            BuildBasisLabelsAndToolTip(ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile, basisProfile);
                        }
                        break;

                    case ePlanSessionType.StoreMultiLevel:

                        //basisProfList = ManagerData.OpenParms.GetBasisProfileList(_planCubeGroup, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key);

                        foreach (BasisProfile basisProfile in ManagerData.OpenParms.BasisProfileList)
                        {
                            BuildBasisLabelsAndToolTip(ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile, basisProfile);
                        }

                        foreach (PlanProfile planProf in ManagerData.OpenParms.LowLevelPlanProfileList)
                        {
                            basisProfList = ManagerData.OpenParms.GetBasisProfileList(ManagerData.PlanCubeGroup, planProf.NodeProfile.Key, planProf.VersionProfile.Key);

                            foreach (BasisProfile basisProfile in basisProfList)
                            {
                                BuildBasisLabelsAndToolTip(planProf.NodeProfile, basisProfile);
                            }
                        }
                        break;

                    case ePlanSessionType.ChainSingleLevel:

                        //basisProfList = ManagerData.OpenParms.GetBasisProfileList(_planCubeGroup, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key);

                        foreach (BasisProfile basisProfile in ManagerData.OpenParms.BasisProfileList)
                        {
                            BuildBasisLabelsAndToolTip(ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile, basisProfile);
                        }
                        break;

                    case ePlanSessionType.ChainMultiLevel:

                       // basisProfList = ManagerData.OpenParms.GetBasisProfileList(_planCubeGroup, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key);

                        foreach (BasisProfile basisProfile in ManagerData.OpenParms.BasisProfileList)
                        {
                            BuildBasisLabelsAndToolTip(ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile, basisProfile);
                        }

                        foreach (PlanProfile planProf in ManagerData.OpenParms.LowLevelPlanProfileList)
                        {
                            basisProfList = ManagerData.OpenParms.GetBasisProfileList(ManagerData.PlanCubeGroup, planProf.NodeProfile.Key, planProf.VersionProfile.Key);

                            foreach (BasisProfile basisProfile in basisProfList)
                            {
                                BuildBasisLabelsAndToolTip(planProf.NodeProfile, basisProfile);
                            }
                        }
                        break;
                }

                switch (ManagerData.OpenParms.PlanSessionType)
                {
                    case ePlanSessionType.StoreSingleLevel:
                    case ePlanSessionType.StoreMultiLevel:

                        //RO-3083 Data transport for Channel(Store) Review changing of Channel(Store) Attributes and Attribute Sets
                        if (_lastStoreAttributeKey == 0 || _lastStoreAttributeKey == -1)
                        {
                            _lastStoreAttributeKey = ManagerData.OpenParms.StoreGroupRID;
                        }
                        else
                        {
                            if (_lastStoreAttributeKey != ManagerData.OpenParms.StoreGroupRID)
                            {
                                //reset the store attribute key
                                ManagerData.OpenParms.StoreGroupRID = _lastStoreAttributeKey;
                                //and reset the store attribute set key
                                //_lastStoreAttributeSetKey = -1;
                                //reset store group profile
                                ((StorePlanMaintCubeGroup)PlanCubeGroup).SetStoreGroup(new StoreGroupProfile(_lastStoreAttributeKey));
                            }
                        }

                        if (ManagerData.OpenParms.PlanSessionType == ePlanSessionType.StoreSingleLevel)
                        {
                            ((StorePlanMaintCubeGroup)PlanCubeGroup).GetReadOnlyFlags(out _storeReadOnly, out _chainReadOnly);
                        }
                        else
                        {
                            ((StoreMultiLevelPlanMaintCubeGroup)PlanCubeGroup).GetReadOnlyFlags(out _storeReadOnly, out _chainReadOnly, out _lowLevelStoreReadOnly, out _lowLevelChainReadOnly);
                        }

                        //RO-3084 Data transport for Channel(Store) Review changing of Channel(Store) Filters
                        if (_lastFilterKey == 0 || _lastFilterKey == -1)
                        {
                            _lastFilterKey = ManagerData.OpenParms.FilterRID;
                        }
                        else
                        {
                            if (_lastFilterKey != ManagerData.OpenParms.FilterRID)
                            {
                                ManagerData.OpenParms.FilterRID = _lastFilterKey;
                                ProfileList _storeProfileList = new ProfileList(eProfileType.Store);
                            }
                        }

                        //((PlanCubeGroup)PlanCubeGroup).SetStoreFilter(ManagerData.OpenParms.FilterRID, PlanCubeGroup);
                        ((PlanCubeGroup)PlanCubeGroup).SetStoreFilter(_lastFilterKey, PlanCubeGroup);

                        //Retrieve StoreProfile list
                        _storeProfileList = PlanCubeGroup.GetFilteredProfileList(eProfileType.Store);

                        ProfileList _singleStoreProfileList = new ProfileList(eProfileType.Store);
                        if (!String.IsNullOrEmpty(ManagerData.OpenParms.StoreId) && ManagerData.OpenParms.StoreId != "(None)")
                        {
                            foreach (StoreProfile storeProf in _storeProfileList)
                            {
                                if (storeProf.StoreId == ManagerData.OpenParms.StoreId)
                                {
                                    _singleStoreProfileList.Add(storeProf);
                                }
                            }
                            _storeProfileList = _singleStoreProfileList;
                        }

                        Audit _audit;
                        _audit = ManagerData.SAB.ApplicationServerSession.Audit;

                        string message = "Plan Session Type: " + ManagerData.OpenParms.PlanSessionType;
                        _audit.Add_Msg(eMIDMessageLevel.Debug, message, this.GetType().Name);

                        message = "StoreProfileLIst count: " + _storeProfileList.Count.ToString();
                        _audit.Add_Msg(eMIDMessageLevel.Debug, message, this.GetType().Name);

                        message = "Store Group RID: " + ManagerData.OpenParms.StoreGroupRID;
                        _audit.Add_Msg(eMIDMessageLevel.Debug, message, this.GetType().Name);



                        if (_storeProfileList.Count == 0)  /// SMR HERE
                        {
                            MessageBox.Show("Applied filter(s) have resulted in no displayable Stores.", "Filter Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }

                        _workingDetailProfileList = _storeProfileList;

                        //Retrieve StoreGroupListViewProfile list

                        _storeGroupListViewProfileList = PlanCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupListView);

                        //Retrieve StoreGroupLevelProfile list

                        _storeGroupLevelProfileList = PlanCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupLevel);
                        _selectableStoreAttributeHeaders = new ArrayList();
                        bool storeAttributeSetFoundKey = false;

                        foreach (StoreGroupLevelProfile strGrpLvlProf in _storeGroupLevelProfileList)
                        {
                           _selectableStoreAttributeHeaders.Add(new RowColProfileHeader(strGrpLvlProf.Name, true, strGrpLvlProf.Sequence, strGrpLvlProf));
                            if (strGrpLvlProf.Key == _lastStoreAttributeSetKey)
                            {
                                storeAttributeSetFoundKey = true;
                            }
                        }

                        if (!storeAttributeSetFoundKey)
                        {
                            _lastStoreAttributeSetKey = -1;
                        }

                        //RO-3083 Data transport for Channel(Store) Review changing of Channel(Store) Attributes and Attribute Sets
                        if (_lastStoreAttributeSetKey == 0 || _lastStoreAttributeSetKey == -1)
                        {
                            _lastStoreAttributeSetKey = _storeGroupLevelProfileList[0].Key;
                        }
                       
                        _workingDetailProfileList = new ProfileList(eProfileType.Store);
                        BuildWorkingStoreList(_lastStoreAttributeSetKey, _workingDetailProfileList);

                        break;

                    case ePlanSessionType.ChainSingleLevel:

                        ((ChainPlanMaintCubeGroup)PlanCubeGroup).GetReadOnlyFlags(out _chainReadOnly);
                        _workingDetailProfileList = null;

                        break;

                    case ePlanSessionType.ChainMultiLevel:

                        ((ChainMultiLevelPlanMaintCubeGroup)PlanCubeGroup).GetReadOnlyFlags(out _chainReadOnly, out _lowLevelChainReadOnly);
                        _workingDetailProfileList = ManagerData.OpenParms.LowLevelPlanProfileList;

                        break;
                }

                _periodProfileList = ManagerData.SAB.ClientServerSession.Calendar.GetPeriodProfileList(ManagerData.OpenParms.DateRangeProfile.Key);

                _variableProfileList = PlanCubeGroup.GetFilteredProfileList(eProfileType.Variable);
                _quantityVariableProfileList = PlanCubeGroup.GetFilteredProfileList(eProfileType.QuantityVariable);

                // Load View

                LoadView();

                // Build Basis List

                _selectableBasisHeaders = new ArrayList();

                //_cmiBasisList = new ArrayList();

                if (ManagerData.OpenParms.BasisProfileList.Count > 0)
                {
                    //basisCmiSeparator = new ToolStripSeparator();
                    //cmsg4g7g10.Items.Add(basisCmiSeparator);
                    //_cmiBasisList.Add(basisCmiSeparator);

                    for (i = 0; i < ManagerData.OpenParms.BasisProfileList.Count; i++)
                    {
                        //Begin Track #5750 - KJohnson - Basis Labels Not Showing Correct Information
                        BasisProfile bp = (BasisProfile)ManagerData.OpenParms.BasisProfileList[i];
                        BasisDetailProfile bdp = (BasisDetailProfile)bp.BasisDetailProfileList[0];
                        //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                        string lblName = GetBasisLabel(bp.Key, bdp.HierarchyNodeProfile.Key);
                        if (lblName != null)
                        {
                            bp.Name = lblName;
                        }
                        //End Track #5782

                        //Begin Track #5779 - JScott - Right Click Menu for Labels
                        //_selectableBasisHeaders.Add(new RowColProfileHeader(bp.Name, true, i, bp));
                        //End Track #5779 - JScott - Right Click Menu for Labels

                        //basisCmiItem = new ToolStripMenuItem();
                        //Begin Track #5779 - JScott - Right Click Menu for Labels
                        _selectableBasisHeaders.Add(new RowColProfileHeader(bp.Name, true, i, bp));
                        //End Track #5779 - JScott - Right Click Menu for Labels
                        //basisCmiItem.Text = bp.Name;
                        //End Track #5750 - KJohnson
                       //basisCmiItem.Click += new System.EventHandler(this.cmiBasis_Click);
                        //basisCmiItem.Checked = true;

                        //cmsg4g7g10.Items.Add(basisCmiItem);
                        //_cmiBasisList.Add(basisCmiItem);
                    }
                }


                // Build DateProfile list

                BuildTimeHeaders();

                switch (ManagerData.OpenParms.PlanSessionType)
                {
                    case ePlanSessionType.ChainMultiLevel:
                    case ePlanSessionType.StoreMultiLevel:

                        _currentStorePlanProfile = null;
                        _currentChainPlanProfile = null;

                        break;

                    case ePlanSessionType.ChainSingleLevel:

                        _currentStorePlanProfile = null;
                        _currentChainPlanProfile = ManagerData.OpenParms.ChainHLPlanProfile;

                        break;

                    case ePlanSessionType.StoreSingleLevel:

                       _currentStorePlanProfile = ManagerData.OpenParms.StoreHLPlanProfile;
                        _currentChainPlanProfile = ManagerData.OpenParms.ChainHLPlanProfile;

                        break;
                }


                // Format and Fill grids




                //Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
                //Formatg2Grid(false, -1, SortEnum.none);
                //Formatg3Grid(false, -1, -1, SortEnum.none);
                Formatg2Grid(false, null, SortEnum.none);
                Formatg3Grid(false, null, SortEnum.none);
                //End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
                Formatg4Grid(false, g4, _workingDetailProfileList, true);
                Formatg5Grid(false);
                Formatg6Grid(false);
                Formatg7Grid(false);
                Formatg8Grid(false);
                Formatg9Grid(false);
                Formatg10Grid(false);
                Formatg11Grid(false);
                Formatg12Grid(false);
                SortToDefault();
                LoadCurrentPages();
                
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void LoadView()
        {
            int i;
            VariableProfile viewVarProf;
            QuantityVariableProfile viewQVarProf;
            DataRow viewRow;
            Hashtable varKeyHash;
            Hashtable perKeyHash;
            bool selectYear;
            bool selectSeason;
            bool selectQuarter;
            bool selectMonth;
            bool selectWeek;
            Hashtable qVarKeyHash;
            bool cont;

            try
            {
                //Read PlanViewDetail table

                _planViewDetail = _planViewData.PlanViewDetail_Read(ManagerData.OpenParms.ViewRID);

                //Load columns

                varKeyHash = new Hashtable();
                _selectableVariableHeaders = new ArrayList();

                foreach (DataRow row in _planViewDetail.Rows)
                {
                    if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eViewAxis.Variable)
                    {
                        if (Convert.ToInt32(row["PROFILE_TYPE"], CultureInfo.CurrentUICulture) == (int)eProfileType.Variable)
                        {
                            viewVarProf = (VariableProfile)_variableProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));

                            if (viewVarProf != null)
                            {
                                varKeyHash.Add(viewVarProf.Key, row);
                            }
                        }
                    }
                }

                foreach (VariableProfile varProf in _variableProfileList)
                {
                    viewRow = (DataRow)varKeyHash[varProf.Key];
                    if (viewRow != null)
                    {
                        _selectableVariableHeaders.Add(new RowColProfileHeader(varProf.VariableName, true, Convert.ToInt32(viewRow["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), varProf, varProf.Groupings));
                    }
                    else
                    {
                        _selectableVariableHeaders.Add(new RowColProfileHeader(varProf.VariableName, false, -1, varProf, varProf.Groupings));
                    }
                }

                CreateSortedList(_selectableVariableHeaders, out _sortedVariableHeaders);

                if (_sortedVariableHeaders.Count == 0)
                {
                    MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_pl_NoDisplayableVariables), "No Displayable Variables", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                //Load rows

                qVarKeyHash = new Hashtable();
                _selectableQuantityHeaders = new ArrayList();

                foreach (DataRow row in _planViewDetail.Rows)
                {
                    if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eViewAxis.Quantity)
                    {
                        if (Convert.ToInt32(row["PROFILE_TYPE"], CultureInfo.CurrentUICulture) == (int)eProfileType.QuantityVariable)
                        {
                            viewQVarProf = (QuantityVariableProfile)_quantityVariableProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));

                            if (viewQVarProf != null)
                            {
                                qVarKeyHash.Add(viewQVarProf.Key, row);
                            }
                        }
                    }
                }

                _quantityVariables = ManagerData.Transaction.PlanComputations.PlanQuantityVariables;

                _currentRowHeader = new RowColProfileHeader("Current Plan", true, 0, _quantityVariables.ValueQuantity);
                _originalRowHeader = new RowColProfileHeader("Original Plan", false, 1, _quantityVariables.ValueQuantity);

                viewRow = (DataRow)qVarKeyHash[_quantityVariables.ValueQuantity.Key];

                if (viewRow != null)
                {
                    _adjustmentRowHeader = new RowColProfileHeader("Adjusted Plan", true, 0, _quantityVariables.ValueQuantity);
                }
                else
                {
                    _adjustmentRowHeader = new RowColProfileHeader("Adjusted Plan", false, 0, _quantityVariables.ValueQuantity);
                }

                _selectableQuantityHeaders.Add(_adjustmentRowHeader);
                i = 2;

                foreach (QuantityVariableProfile qVarProf in _quantityVariableProfileList)
                {
                    cont = false;

                    switch (ManagerData.OpenParms.PlanSessionType)
                    {
                        case ePlanSessionType.ChainMultiLevel:
                            if (qVarProf.isChainMultiView &&
                                (qVarProf.isChainDetailCube))
                            {
                                cont = true;
                            }
                            break;

                        case ePlanSessionType.ChainSingleLevel:
                            if (qVarProf.isChainSingleView && qVarProf.isHighLevel &&
                                qVarProf.isChainDetailCube)
                            {
                                cont = true;
                            }
                            break;

                        case ePlanSessionType.StoreMultiLevel:
                            if (qVarProf.isStoreMultiView &&
                                (qVarProf.isChainDetailCube || qVarProf.isStoreDetailCube || qVarProf.isStoreSetCube || qVarProf.isStoreTotalCube))
                            {
                                cont = true;
                            }
                            break;

                        case ePlanSessionType.StoreSingleLevel:
                            if (qVarProf.isStoreSingleView && qVarProf.isHighLevel &&
                                (qVarProf.isChainDetailCube || qVarProf.isStoreDetailCube || qVarProf.isStoreSetCube || qVarProf.isStoreTotalCube))
                            {
                                cont = true;
                            }
                            break;
                    }

                    if (qVarProf.isSelectable && cont)
                    {
                        viewRow = (DataRow)qVarKeyHash[qVarProf.Key];
                        if (viewRow != null)
                        {
                            _selectableQuantityHeaders.Add(new RowColProfileHeader(qVarProf.VariableName, true, i, qVarProf));
                        }
                        else
                        {
                            _selectableQuantityHeaders.Add(new RowColProfileHeader(qVarProf.VariableName, false, i, qVarProf));
                        }
                        i++;
                    }
                }

                // Load Periods

                perKeyHash = new Hashtable();
                _selectablePeriodHeaders = new ArrayList();

                foreach (DataRow row in _planViewDetail.Rows)
                {
                    if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eViewAxis.Period)
                    {
                        if (Convert.ToInt32(row["PROFILE_TYPE"], CultureInfo.CurrentUICulture) == (int)eProfileType.Period)
                        {
                            perKeyHash.Add(Convert.ToInt32(row["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), row);
                        }
                    }
                }

                selectMonth = (DataRow)perKeyHash[(int)eProfileType.Month] != null;
                selectWeek = (DataRow)perKeyHash[(int)eProfileType.Week] != null;

                if (ManagerData.OpenParms.PlanSessionType == ePlanSessionType.ChainMultiLevel ||
                    ManagerData.OpenParms.PlanSessionType == ePlanSessionType.ChainSingleLevel)
                {
                    selectYear = (DataRow)perKeyHash[(int)eProfileType.Year] != null;
                    selectSeason = (DataRow)perKeyHash[(int)eProfileType.Season] != null;
                    selectQuarter = (DataRow)perKeyHash[(int)eProfileType.Quarter] != null;

                    if (selectYear)
                    {
                        _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Years", true, (int)eProfileType.Year, null));
                    }
                    else
                    {
                        _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Years", false, (int)eProfileType.Year, null));
                    }

                    if (selectSeason)
                    {
                        _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Seasons", true, (int)eProfileType.Season, null));
                    }
                    else
                    {
                        _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Seasons", false, (int)eProfileType.Season, null));
                    }

                    if (selectQuarter)
                    {
                        _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Quarters", true, (int)eProfileType.Quarter, null));
                    }
                    else
                    {
                        _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Quarters", false, (int)eProfileType.Quarter, null));
                    }

                    if (!selectYear && !selectSeason && !selectQuarter && !selectMonth && !selectWeek)
                    {
                        selectMonth = true;
                    }
                }
                else
                {
                    if (!selectMonth && !selectWeek)
                    {
                        selectMonth = true;
                    }
                }

                if (selectMonth)
                {
                    _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Months", true, (int)eProfileType.Month, null));
                }
                else
                {
                    _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Months", false, (int)eProfileType.Month, null));
                }

                if (selectWeek)
                {
                    _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Weeks", true, (int)eProfileType.Week, null));
                }
                else
                {
                    _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Weeks", false, (int)eProfileType.Week, null));
                }

                CreatePeriodHash();

            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void CreatePeriodHash()
        {
            int i;

            try
            {
                _periodHeaderHash = new Hashtable();

                for (i = 0; i < _selectablePeriodHeaders.Count; i++)
                {
                    if (((RowColProfileHeader)_selectablePeriodHeaders[i]).IsDisplayed)
                    {
                        _periodHeaderHash.Add(((RowColProfileHeader)_selectablePeriodHeaders[i]).Sequence, null);
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void BuildTimeHeaders()
        {
            int i;

            try
            {
                i = 0;

                _selectableTimeHeaders = new ArrayList();

                BuildPeriodHeaders(_periodProfileList, ManagerData.OpenParms.GetWeekProfileList(ManagerData.SAB.ClientServerSession), ref i);

                CreateSortedList(_selectableTimeHeaders, out _sortedTimeHeaders);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void BuildPeriodHeaders(ProfileList aPeriodList, ProfileList aWeekList, ref int aSeq)
        {
            try
            {
                if (aPeriodList.ProfileType == eProfileType.Period)
                {
                    foreach (PeriodProfile perProf in aPeriodList)
                    {
                        if (_periodHeaderHash.Contains((int)perProf.PeriodProfileType))
                        {
                            _selectableTimeHeaders.Add(new RowColProfileHeader(perProf.Text(), true, aSeq++, perProf));
                        }

                        if (perProf.ChildPeriodList.Count > 0)
                        {
                            BuildPeriodHeaders(perProf.ChildPeriodList, aWeekList, ref aSeq);
                        }
                        else
                        {
                            BuildPeriodHeaders(perProf.Weeks, aWeekList, ref aSeq);
                        }
                    }
                }
                else
                {
                    if (_periodHeaderHash.Contains((int)aPeriodList.ProfileType))
                    {
                        foreach (WeekProfile weekProf in aPeriodList)
                        {
                            if (aWeekList.Contains(weekProf.Key))
                            {
                                _selectableTimeHeaders.Add(new RowColProfileHeader(weekProf.Text(), true, aSeq++, weekProf));
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void BuildBasisLabelsAndToolTip(HierarchyNodeProfile aHierarchyNodeProfile, BasisProfile aBasisProfile)
        {
            //string toolTipStr;
            //string newLine;
            //int i;
            //System.Windows.Forms.ToolTip toolTip;

            try
            {
                //Begin Track #5782 - KJohnson - Basis Labels Not Showing Correct Information
                //foreach (BasisProfile basisProfile in _openParms.BasisProfileList)
                //{
                //End Track #5782 - KJohnson
                //-----------Fill In ToolTip---------------------------------------------
                //toolTipStr = "";
                //newLine = "";
                //i = 0;

                //foreach (BasisDetailProfile basisDetailProfile in aBasisProfile.BasisDetailProfileList)
                //{
                //    i++;
                //    toolTipStr += newLine + "Detail " + Convert.ToInt32(i, CultureInfo.CurrentUICulture) + ": " + basisDetailProfile.HierarchyNodeProfile.Text + " / " + basisDetailProfile.VersionProfile.Description + " / " + basisDetailProfile.DateRangeProfile.DisplayDate + " / " + Convert.ToString(basisDetailProfile.Weight, CultureInfo.CurrentUICulture);
                //    newLine = System.Environment.NewLine;
                //}

                //toolTip = new System.Windows.Forms.ToolTip(this.components);
                //toolTip.Active = false;
                //toolTip.SetToolTip(g4, toolTipStr);
                //toolTip.SetToolTip(g7, toolTipStr);
                //toolTip.SetToolTip(g10, toolTipStr);

                //_basisToolTipList[new HashKeyObject(aHierarchyNodeProfile.Key, aBasisProfile.Key)] = toolTip;


                //-----------Fill In Basis Labels---------------------------------------------
                string tmpLabel = _basisLabel;
                foreach (BasisDetailProfile basisDetailProfile in aBasisProfile.BasisDetailProfileList)
                {
                    tmpLabel = tmpLabel.Replace("Merchandise", basisDetailProfile.HierarchyNodeProfile.Text);
                    tmpLabel = tmpLabel.Replace("Version", basisDetailProfile.VersionProfile.Description);
                    tmpLabel = tmpLabel.Replace("Time_Period", basisDetailProfile.DateRangeProfile.DisplayDate);
                    //Begin Track #5782 - KJohnson - Basis Labels Not Showing Correct Information
                    if (tmpLabel == "")
                    {
                        tmpLabel = aBasisProfile.Name;
                    }
                    else
                    {
                        aBasisProfile.Name = tmpLabel;
                    }
                    //End Track #5782 - KJohnson
                    break;
                }
                _basisLabelList[new HashKeyObject(aHierarchyNodeProfile.Key, aBasisProfile.Key)] = tmpLabel;
                //Begin Track #5782 - KJohnson - Basis Labels Not Showing Correct Information
                //}
                //End Track #5782 - KJohnson
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void CreateSortedList(ArrayList aSelectableList, out SortedList aSortedList)
        {
            SortedList sortList;
            IDictionaryEnumerator enumerator;
            int i, j;
            int newCols;

            try
            {
                sortList = new SortedList();
                newCols = 0;

                for (i = 0; i < aSelectableList.Count; i++)
                {
                    if (((RowColProfileHeader)aSelectableList[i]).IsDisplayed)
                    {
                        if (((RowColProfileHeader)aSelectableList[i]).Sequence == -1)
                        {
                            newCols++;
                            ((RowColProfileHeader)aSelectableList[i]).Sequence = newCols * -1;
                        }
                        sortList.Add(((RowColProfileHeader)aSelectableList[i]).Sequence, i);
                    }
                    else
                    {
                        ((RowColProfileHeader)aSelectableList[i]).Sequence = -1;
                    }
                }

                enumerator = sortList.GetEnumerator();
                j = 0;

                while (enumerator.MoveNext())
                {
                    if (Convert.ToInt32(enumerator.Key, CultureInfo.CurrentUICulture) < 0)
                    {
                        ((RowColProfileHeader)aSelectableList[(int)enumerator.Value]).Sequence = sortList.Count - newCols + (Convert.ToInt32(enumerator.Key, CultureInfo.CurrentUICulture) * -1) - 1;
                    }
                    else
                    {
                        ((RowColProfileHeader)aSelectableList[(int)enumerator.Value]).Sequence = j;
                        j++;
                    }
                }

                aSortedList = new SortedList();

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

        private void Formatg2Grid(bool aClearGrid, CubeWaferCoordinateList aSortCoordinates, SortEnum aSortDirection)
        {
            int colsPerGroup = 1;
            int maxTimeTotVars = 0;
            ArrayList timeTotVars = null;
            int i;
            CubeWaferCoordinateList cubeWaferCoordinateList;
            RowColProfileHeader varHeader;
            string timeName;
            VariableProfile varProf;

            try
            {
                if (aClearGrid)
                {
                    g2.Clear();
                }

                if (g2.Tag == null)
                {
                    g2.Tag = new PagingGridTag(Grid2, g2, null, g2, null, 0, 0);
                }

                if (ManagerData.OpenParms.DateRangeProfile.Name == string.Empty)
                {
                    timeName = "Total";
                }
                else
                {
                    timeName = ManagerData.OpenParms.DateRangeProfile.Name;
                }

                if (ManagerData.OpenParms.GetSummaryDateProfile(ManagerData.SAB.ClientServerSession) != null)
                {
                    if (ManagerData.OpenParms.PlanSessionType == ePlanSessionType.ChainSingleLevel)
                    {
                        maxTimeTotVars = 0;

                        foreach (DictionaryEntry varEntry in _sortedVariableHeaders)
                        {
                            varHeader = (RowColProfileHeader)varEntry.Value;
                            varProf = (VariableProfile)varHeader.Profile;

                            maxTimeTotVars = Math.Max(maxTimeTotVars, varProf.TimeTotalChainVariables.Count);
                        }

                        colsPerGroup = 1;

                        ((PagingGridTag)g2.Tag).GroupsPerGrid = 1;

                        g2.Rows.Count = 2;
                        g2.Cols.Count = ((PagingGridTag)g2.Tag).GroupsPerGrid * colsPerGroup * maxTimeTotVars;
                        g2.Rows.Fixed = g2.Rows.Count;
                        g2.Cols.Fixed = 0;

                        ((PagingGridTag)g2.Tag).SortImageRow = -1;

                        for (i = 0; i < maxTimeTotVars; i++)
                        {
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(ManagerData.OpenParms.GetSummaryDateProfile(ManagerData.SAB.ClientServerSession).ProfileType, ManagerData.OpenParms.GetSummaryDateProfile(ManagerData.SAB.ClientServerSession).Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainTimeTotalIndex, i + 1));
                            g2.Cols[i].UserData = new ColumnHeaderTag(cubeWaferCoordinateList, null, null, i, string.Empty);

                            g2.SetData(0, i, " ");
                            g2.SetData(1, i, timeName + " ");
                        }

                        i--;
                    }
                    else
                    {
                        //Begin Track #5006 - JScott - Display Low-levels one at a time
                        //switch (ManagerData.OpenParms.PlanSessionType)
                        switch (ManagerData.OpenParms.PlanSessionType)
                        //End Track #5006 - JScott - Display Low-levels one at a time
                        {
                            case ePlanSessionType.ChainMultiLevel:
                                maxTimeTotVars = Transaction.PlanComputations.PlanVariables.MaxChainTimeTotalVariables;
                                break;

                            default:
                                maxTimeTotVars = Transaction.PlanComputations.PlanVariables.MaxStoreTimeTotalVariables;
                                break;
                        }


                        if (_columnGroupedBy == GroupedBy.GroupedByTime)
                        {
                            colsPerGroup = _sortedVariableHeaders.Count * maxTimeTotVars;

                            ((PagingGridTag)g2.Tag).GroupsPerGrid = 1;

                            g2.Rows.Count = FIXEDCOLHEADERS;
                            g2.Cols.Count = ((PagingGridTag)g2.Tag).GroupsPerGrid * colsPerGroup;
                            g2.Rows.Fixed = g2.Rows.Count;
                            g2.Cols.Fixed = 0;

                            ((PagingGridTag)g2.Tag).SortImageRow = 2;

                            i = -1;

                            foreach (DictionaryEntry varEntry in _sortedVariableHeaders)
                            {
                                varHeader = (RowColProfileHeader)varEntry.Value;

                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                //switch (ManagerData.OpenParms.PlanSessionType)
                                switch (ManagerData.OpenParms.PlanSessionType)
                                //End Track #5006 - JScott - Display Low-levels one at a time
                                {
                                    case ePlanSessionType.ChainMultiLevel:
                                        timeTotVars = ((VariableProfile)varHeader.Profile).TimeTotalChainVariables;
                                        break;

                                    default:
                                        timeTotVars = ((VariableProfile)varHeader.Profile).TimeTotalStoreVariables;
                                        break;
                                }

                                foreach (TimeTotalVariableProfile timeTotVarProf in timeTotVars)
                                {
                                    i++;
                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(ManagerData.OpenParms.GetSummaryDateProfile(ManagerData.SAB.ClientServerSession).ProfileType, ManagerData.OpenParms.GetSummaryDateProfile(ManagerData.SAB.ClientServerSession).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varHeader.Profile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.TimeTotalVariable, timeTotVarProf.Key));
                                    g2.Cols[i].UserData = new ColumnHeaderTag(cubeWaferCoordinateList, null, varHeader, i, timeTotVarProf.VariableName);

                                    //Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
                                    //if (varHeader.Profile.Key == aVariableSortKey)
                                    if (aSortCoordinates != null && aSortCoordinates.Equals(cubeWaferCoordinateList))
                                    //End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
                                    {
                                        ((ColumnHeaderTag)g2.Cols[i].UserData).Sort = aSortDirection;
                                        if (aSortDirection == SortEnum.asc)
                                        {
                                            //g2.SetCellImage(2, i, _upArrow);
                                        }
                                        else if (aSortDirection == SortEnum.desc)
                                        {
                                            //g2.SetCellImage(2, i, _downArrow);
                                        }
                                    }

                                    g2.SetData(0, i, " ");
                                    g2.SetData(1, i, timeName + " ");
                                    g2.SetData(2, i, timeTotVarProf.VariableName);
                                }
                            }
                        }
                        else
                        {
                            colsPerGroup = 1;

                            ((PagingGridTag)g2.Tag).GroupsPerGrid = _sortedVariableHeaders.Count;

                            g2.Rows.Count = FIXEDCOLHEADERS;
                            g2.Cols.Count = ((PagingGridTag)g2.Tag).GroupsPerGrid * colsPerGroup * maxTimeTotVars;
                            g2.Rows.Fixed = g2.Rows.Count;
                            g2.Cols.Fixed = 0;

                            ((PagingGridTag)g2.Tag).SortImageRow = 1;

                            i = -1;
                            foreach (DictionaryEntry varEntry in _sortedVariableHeaders)
                            {
                                varHeader = (RowColProfileHeader)varEntry.Value;

                                switch (ManagerData.OpenParms.PlanSessionType)
                                {
                                    case ePlanSessionType.ChainMultiLevel:
                                        timeTotVars = ((VariableProfile)varHeader.Profile).TimeTotalChainVariables;
                                        break;

                                    default:
                                        timeTotVars = ((VariableProfile)varHeader.Profile).TimeTotalStoreVariables;
                                        break;
                                }

                                foreach (TimeTotalVariableProfile timeTotVarProf in timeTotVars)
                                {
                                    i++;
                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(ManagerData.OpenParms.GetSummaryDateProfile(ManagerData.SAB.ClientServerSession).ProfileType, ManagerData.OpenParms.GetSummaryDateProfile(ManagerData.SAB.ClientServerSession).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varHeader.Profile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.TimeTotalVariable, timeTotVarProf.Key));
                                    g2.Cols[i].UserData = new ColumnHeaderTag(cubeWaferCoordinateList, varHeader, null, i, timeTotVarProf.VariableName);

                                    if (aSortCoordinates != null && aSortCoordinates.Equals(cubeWaferCoordinateList))
                                    {
                                        ((ColumnHeaderTag)g2.Cols[i].UserData).Sort = aSortDirection;
                                        if (aSortDirection == SortEnum.asc)
                                        {
                                            //g2.SetCellImage(1, i, _upArrow);
                                        }
                                        else if (aSortDirection == SortEnum.desc)
                                        {
                                            //g2.SetCellImage(1, i, _downArrow);
                                        }
                                    }

                                    g2.SetData(0, i, " ");
                                    g2.SetData(1, i, timeTotVarProf.VariableName);
                                    g2.SetData(2, i, timeName + " ");
                                }
                            }
                        }
                    }

                    if (i >= 0)
                    {
                        g2.Cols.Count = i + 1;
                        ((PagingGridTag)g2.Tag).Visible = true;
                        ((PagingGridTag)g2.Tag).DetailsPerGroup = colsPerGroup;
                    }
                    else
                    {
                        g2.Cols.Count = 0;
                        g2.Rows.Count = 0;
                        ((PagingGridTag)g2.Tag).Visible = false;
                        ((PagingGridTag)g2.Tag).DetailsPerGroup = 0;
                    }
                }
                else
                {
                    g2.Cols.Count = 0;
                    g2.Rows.Count = 0;
                    ((PagingGridTag)g2.Tag).Visible = false;
                    ((PagingGridTag)g2.Tag).DetailsPerGroup = 0;
                }

                ((PagingGridTag)g2.Tag).UnitsPerScroll = 1;

                foreach (Row row in g2.Rows)
                {
                    row.AllowMerging = true;
                }

                _gridData[Grid2] = new ROCell[g2.Rows.Count, g2.Cols.Count];
                //added for Metadata Per Group Count
                switch (ManagerData.OpenParms.PlanSessionType)
                {
                    case ePlanSessionType.ChainMultiLevel:
                        _timeTotVarsPerGroup =  maxTimeTotVars;
                        break;
                    case ePlanSessionType.StoreSingleLevel:
                        //number of stores
                        _timeTotVarsPerGroup =  _workingDetailProfileList.Count;
                        break;
                    default:
                        _timeTotVarsPerGroup = maxTimeTotVars;
                        break;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Formatg3Grid(bool aClearGrid, CubeWaferCoordinateList aSortCoordinates, SortEnum aSortDirection)
        {
            int colsPerGroup;
            int i;
            string headerDesc;
            CubeWaferCoordinateList cubeWaferCoordinateList;
            RowColProfileHeader varHeader;
            RowColProfileHeader timeHeader = null;

            try
            {
                if (aClearGrid)
                {
                    g3.Clear();
                }

                if (g3.Tag == null)
                {
                    g3.Tag = new PagingGridTag(Grid3, g3, null, g3, null, 0, 0);
                }

               

                //Begin Track #5006 - JScott - Display Low-levels one at a time
                //if (ManagerData.OpenParms.PlanSessionType == ePlanSessionType.ChainSingleLevel)
                if (ManagerData.OpenParms.PlanSessionType == ePlanSessionType.ChainSingleLevel)
                //End Track #5006 - JScott - Display Low-levels one at a time
                {
                    colsPerGroup = 1;

                    ((PagingGridTag)g3.Tag).GroupsPerGrid = _sortedTimeHeaders.Count;

                    g3.Rows.Count = 2;
                    g3.Cols.Count = ((PagingGridTag)g3.Tag).GroupsPerGrid * colsPerGroup;
                    g3.Rows.Fixed = g3.Rows.Count;
                    g3.Cols.Fixed = 0;
                    //Begin Track #6260 - JScott - Multi level>select cls 35018>results in Err
                    ((PagingGridTag)g3.Tag).SortImageRow = -1;
                    //End Track #6260 - JScott - Multi level>select cls 35018>results in Err

                    i = -1;

                    foreach (DictionaryEntry timeEntry in _sortedTimeHeaders)
                    {
                        timeHeader = (RowColProfileHeader)timeEntry.Value;

                        i++;
                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(timeHeader.Profile.ProfileType, timeHeader.Profile.Key));

                        g3.Cols[i].UserData = new ColumnHeaderTag(cubeWaferCoordinateList, timeHeader, null, i, timeHeader.Name);

                        //Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
                        //if (timeHeader.Profile.Key == aTimeSortKey)
                        if (aSortCoordinates != null && aSortCoordinates.Equals(cubeWaferCoordinateList))
                        //End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
                        {
                            ((ColumnHeaderTag)g3.Cols[i].UserData).Sort = aSortDirection;
                            if (aSortDirection == SortEnum.asc)
                            {
                                //g3.SetCellImage(2, i, _upArrow);
                            }
                            else if (aSortDirection == SortEnum.desc)
                            {
                                //g3.SetCellImage(2, i, _downArrow);
                            }
                        }

                        //g3.SetData(0, i, headerDesc);
                        g3.SetData(1, i, timeHeader.Name);
                    }
                }
                else if (_columnGroupedBy == GroupedBy.GroupedByTime)
                {
                    colsPerGroup = _sortedVariableHeaders.Count;

                    ((PagingGridTag)g3.Tag).GroupsPerGrid = _sortedTimeHeaders.Count;

                    g3.Rows.Count = FIXEDCOLHEADERS;
                    g3.Cols.Count = ((PagingGridTag)g3.Tag).GroupsPerGrid * colsPerGroup;
                    g3.Rows.Fixed = g3.Rows.Count;
                    g3.Cols.Fixed = 0;
                    //Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
                    ((PagingGridTag)g3.Tag).SortImageRow = 2;
                    //End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw

                    i = -1;

                    foreach (DictionaryEntry timeEntry in _sortedTimeHeaders)
                    {
                        timeHeader = (RowColProfileHeader)timeEntry.Value;

                        foreach (DictionaryEntry varEntry in _sortedVariableHeaders)
                        {
                            varHeader = (RowColProfileHeader)varEntry.Value;

                            i++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(timeHeader.Profile.ProfileType, timeHeader.Profile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varHeader.Profile.Key));

                            g3.Cols[i].UserData = new ColumnHeaderTag(cubeWaferCoordinateList, timeHeader, varHeader, i, timeHeader.Name + "|" + varHeader.Name);

                            if (aSortCoordinates != null && aSortCoordinates.Equals(cubeWaferCoordinateList))
                            {
                                ((ColumnHeaderTag)g3.Cols[i].UserData).Sort = aSortDirection;
                                if (aSortDirection == SortEnum.asc)
                                {
                                    //g3.SetCellImage(2, i, _upArrow);
                                }
                                else if (aSortDirection == SortEnum.desc)
                                {
                                    //g3.SetCellImage(2, i, _downArrow);
                                }
                            }

                            //g3.SetData(0, i, headerDesc);
                            g3.SetData(1, i, timeHeader.Name);
                            g3.SetData(2, i, varHeader.Name);
                        }
                    }
                }
                else
                {
                    colsPerGroup = _sortedTimeHeaders.Count;

                    ((PagingGridTag)g3.Tag).GroupsPerGrid = _sortedVariableHeaders.Count;

                    g3.Rows.Count = FIXEDCOLHEADERS;
                    g3.Cols.Count = ((PagingGridTag)g3.Tag).GroupsPerGrid * colsPerGroup;
                    g3.Rows.Fixed = g3.Rows.Count;
                    g3.Cols.Fixed = 0;
                    //Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
                    ((PagingGridTag)g3.Tag).SortImageRow = 2;
                    //End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw

                    i = -1;

                    foreach (DictionaryEntry varEntry in _sortedVariableHeaders)
                    {
                        varHeader = (RowColProfileHeader)varEntry.Value;

                        foreach (DictionaryEntry timeEntry in _sortedTimeHeaders)
                        {
                            timeHeader = (RowColProfileHeader)timeEntry.Value;

                            i++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(timeHeader.Profile.ProfileType, timeHeader.Profile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varHeader.Profile.Key));

                            g3.Cols[i].UserData = new ColumnHeaderTag(cubeWaferCoordinateList, varHeader, timeHeader, i, varHeader.Name + "|" + timeHeader.Name);

                            //Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
                            //if (timeHeader.Profile.Key == aTimeSortKey &&
                            //    varHeader.Profile.Key == aVariableSortKey)
                            if (aSortCoordinates != null && aSortCoordinates.Equals(cubeWaferCoordinateList))
                            //End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
                            {
                                ((ColumnHeaderTag)g3.Cols[i].UserData).Sort = aSortDirection;
                                if (aSortDirection == SortEnum.asc)
                                {
                                    //g3.SetCellImage(2, i, _upArrow);
                                }
                                else if (aSortDirection == SortEnum.desc)
                                {
                                    //g3.SetCellImage(2, i, _downArrow);
                                }
                            }

                            //g3.SetData(0, i, headerDesc);
                            g3.SetData(1, i, varHeader.Name);
                            g3.SetData(2, i, timeHeader.Name);
                        }
                    }
                }

                ((PagingGridTag)g3.Tag).Visible = true;
                ((PagingGridTag)g3.Tag).DetailsPerGroup = colsPerGroup;
                ((PagingGridTag)g3.Tag).UnitsPerScroll = 1;

                foreach (Row row in g3.Rows)
                {
                    row.AllowMerging = true;
                }

                _gridData[Grid3] = new ROCell[g3.Rows.Count, g3.Cols.Count];
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Formatg4Grid(bool aClearGrid, C1FlexGrid aGrid, ProfileList aWorkingDetailProfileList,
            bool aViewableGrid)
        {
            CubeWaferCoordinateList cubeWaferCoordinateList;
            ArrayList compList;
            GridRowList gridRowList;
            RowColProfileHeader groupHeader;
            RowColProfileHeader varHeader;
            VariableProfile varProf;

            try
            {
                if (aClearGrid)
                {
                    aGrid.Clear();
                }

                if (aGrid.Tag == null)
                {
                    aGrid.Tag = new PagingGridTag(Grid4, aGrid, aGrid, null, null, 0, 0);
                }

                compList = new ArrayList();
                gridRowList = new GridRowList();

                //Begin Track #5006 - JScott - Display Low-levels one at a time
                //switch (ManagerData.OpenParms.PlanSessionType)
                switch (ManagerData.OpenParms.PlanSessionType)
                //End Track #5006 - JScott - Display Low-levels one at a time
                {
                    case ePlanSessionType.ChainSingleLevel:

                        foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
                        {
                            if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                            {
                                if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube && ((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                                    {
                                        compList.Add(detailHeader);
                                    }
                                }
                            }
                        }

                        ((PagingGridTag)aGrid.Tag).GroupsPerGrid = _sortedVariableHeaders.Count;

                        foreach (DictionaryEntry varEntry in _sortedVariableHeaders)
                        {
                            varHeader = (RowColProfileHeader)varEntry.Value;
                            varProf = (VariableProfile)varHeader.Profile;

                            groupHeader = new RowColProfileHeader(varProf.VariableName, false, 0, varProf);

                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, new RowColProfileHeader("Original Plan", false, 2, null), gridRowList.Count, " ", varProf.VariableName, false), true);

                            if (_adjustmentRowHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
                                //End Track #5006 - JScott - Display Low-levels one at a time
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _originalRowHeader, gridRowList.Count, varProf.VariableName, varProf.VariableName));

                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
                                //End Track #5006 - JScott - Display Low-levels one at a time
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _adjustmentRowHeader, gridRowList.Count, "ADJ", varProf.VariableName + "|ADJ"));
                            }
                            else
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
                                //End Track #5006 - JScott - Display Low-levels one at a time
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, varProf.VariableName, varProf.VariableName));
                            }

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
                                    if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                    {
                                        //End Track #6010 - JScott - Bad % Change on Basis2
                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                        //Begin Track #5006 - JScott - Display Low-levels one at a time
                                        //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                                        //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
                                        //End Track #5006 - JScott - Display Low-levels one at a time
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, varProf.VariableName + "|" + detailHeader.Name));
                                        //Begin Track #6010 - JScott - Bad % Change on Basis2
                                    }
                                    //End Track #6010 - JScott - Bad % Change on Basis2
                                }
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    //Begin Track #5648 - JScott - Export Option from OTS Forecast Review Scrren
                                    //groupHeader = new RowColProfileHeader("Chain " + basisHeader.Name, true, 0, null);
                                    groupHeader = new RowColProfileHeader("Chain " + basisHeader.Name, true, 0, varProf);
                                    //End Track #5648 - JScott - Export Option from OTS Forecast Review Scrren

                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                    //Begin Track #5006 - JScott - Display Low-levels one at a time
                                    //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                                    //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
                                    //End Track #5006 - JScott - Display Low-levels one at a time
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                    //Begin Track #5006 - JScott - Display Low-levels one at a time
                                    ////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                    ////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, varProf.VariableName + "|" + ((BasisProfile)basisHeader.Profile).Name));
                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key), varProf.VariableName + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key)));
                                    ////End Track #5782
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentChainPlanProfile.NodeProfile.Key), varProf.VariableName + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentChainPlanProfile.NodeProfile.Key)));
                                    //End Track #5006 - JScott - Display Low-levels one at a time

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
                                                //End Track #5006 - JScott - Display Low-levels one at a time
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                                ////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                ////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, varProf.VariableName + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, varProf.VariableName + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key) + "|" + detailHeader.Name));
                                                ////End Track #5782
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, varProf.VariableName + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentChainPlanProfile.VersionProfile.Key) + "|" + detailHeader.Name));
                                                //End Track #5006 - JScott - Display Low-levels one at a time
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        break;

                    case ePlanSessionType.StoreSingleLevel:

                        foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
                        {
                            if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                            {
                                if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isStoreDetailCube && ((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                                    {
                                        compList.Add(detailHeader);
                                    }
                                }
                            }
                        }

                        ((PagingGridTag)aGrid.Tag).GroupsPerGrid = aWorkingDetailProfileList.Count;

                        foreach (StoreProfile storeProfile in aWorkingDetailProfileList)
                        {
                            groupHeader = new RowColProfileHeader("Store", true, 0, null);

                            if (_adjustmentRowHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
                                //End Track #5006 - JScott - Display Low-levels one at a time
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _originalRowHeader, gridRowList.Count, storeProfile.Text, storeProfile.Text), true);

                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
                                //End Track #5006 - JScott - Display Low-levels one at a time
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _adjustmentRowHeader, gridRowList.Count, "ADJ", storeProfile.Text + "|ADJ"));
                            }
                            else
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
                                //End Track #5006 - JScott - Display Low-levels one at a time
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, storeProfile.Text, storeProfile.Text), true);
                            }

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
                                    if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                    {
                                        //End Track #6010 - JScott - Bad % Change on Basis2
                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                        //Begin Track #5006 - JScott - Display Low-levels one at a time
                                        //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                        //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
                                        //End Track #5006 - JScott - Display Low-levels one at a time
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + detailHeader.Name));
                                        //Begin Track #6010 - JScott - Bad % Change on Basis2
                                    }
                                    //End Track #6010 - JScott - Bad % Change on Basis2
                                }
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    groupHeader = new RowColProfileHeader("Store " + basisHeader.Name, true, 0, null);

                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                    //Begin Track #5006 - JScott - Display Low-levels one at a time
                                    //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                    //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
                                    //End Track #5006 - JScott - Display Low-levels one at a time
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                    //Begin Track #5006 - JScott - Display Low-levels one at a time
                                    ////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                    ////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, storeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name));
                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key), storeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key)));
                                    ////End Track #5782
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key), storeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key)));
                                    //End Track #5006 - JScott - Display Low-levels one at a time

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
                                                //End Track #5006 - JScott - Display Low-levels one at a time
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                                ////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                ////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
                                                ////End Track #5782
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
                                                //End Track #5006 - JScott - Display Low-levels one at a time
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        break;

                    case ePlanSessionType.ChainMultiLevel:

                        foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
                        {
                            if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                            {
                                if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube &&
                                        ((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
                                    {
                                        compList.Add(detailHeader);
                                    }
                                }
                            }
                        }

                        ((PagingGridTag)aGrid.Tag).GroupsPerGrid = aWorkingDetailProfileList.Count;

                        foreach (PlanProfile planProf in aWorkingDetailProfileList)
                        {
                            groupHeader = new RowColProfileHeader("Low Level", true, 0, null);

                            if (_adjustmentRowHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _originalRowHeader, gridRowList.Count, planProf.NodeProfile.Text, planProf.NodeProfile.Text), true);

                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _adjustmentRowHeader, gridRowList.Count, "ADJ", planProf.NodeProfile.Text + "|ADJ"));
                            }
                            else
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, planProf.NodeProfile.Text, planProf.NodeProfile.Text), true);
                            }

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
                                    if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                    {
                                        //End Track #6010 - JScott - Bad % Change on Basis2
                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, planProf.NodeProfile.Text + "|" + detailHeader.Name));
                                        //Begin Track #6010 - JScott - Bad % Change on Basis2
                                    }
                                    //End Track #6010 - JScott - Bad % Change on Basis2
                                }
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    groupHeader = new RowColProfileHeader("Low Level " + basisHeader.Name, true, 0, null);

                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                    //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, planProf.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name));
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key), planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key)));
                                    //End Track #5782

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, planProf.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key) + "|" + detailHeader.Name));
                                                //End Track #5782
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        break;

                    case ePlanSessionType.StoreMultiLevel:

                        foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
                        {
                            if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                            {
                                if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isStoreDetailCube)
                                    {
                                        compList.Add(detailHeader);
                                    }
                                }
                            }
                        }

                        ((PagingGridTag)aGrid.Tag).GroupsPerGrid = aWorkingDetailProfileList.Count;

                        foreach (StoreProfile storeProfile in aWorkingDetailProfileList)
                        {
                            //=================
                            // Store High Level
                            //=================

                            groupHeader = new RowColProfileHeader("Store", true, 0, null);

                            if (_adjustmentRowHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _originalRowHeader, gridRowList.Count, storeProfile.Text, storeProfile.Text), true);

                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _adjustmentRowHeader, gridRowList.Count, "ADJ", storeProfile.Text + "|ADJ"));
                            }
                            else
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, storeProfile.Text, storeProfile.Text), true);
                            }

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
                                    if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                    {
                                        //End Track #6010 - JScott - Bad % Change on Basis2
                                        if (((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                                        {
                                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + detailHeader.Name));
                                        }
                                        //Begin Track #6010 - JScott - Bad % Change on Basis2
                                    }
                                    //End Track #6010 - JScott - Bad % Change on Basis2
                                }
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    groupHeader = new RowColProfileHeader("Store " + basisHeader.Name, true, 0, null);

                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                    //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, storeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name));
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key), storeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key)));
                                    //End Track #5782

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                if (((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                                                {
                                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                    //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
                                                    //End Track #5782
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //======================
                            // Store Low Level Total
                            //======================

                            groupHeader = new RowColProfileHeader("Low Level Total", true, 0, null);

                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, "Low Level Total", storeProfile.Text + "|Low Level Total"));

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
                                    if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                    {
                                        //End Track #6010 - JScott - Bad % Change on Basis2
                                        if (((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
                                        {
                                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|Low Level Total|" + detailHeader.Name));
                                        }
                                        //Begin Track #6010 - JScott - Bad % Change on Basis2
                                    }
                                    //End Track #6010 - JScott - Bad % Change on Basis2
                                }
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    groupHeader = new RowColProfileHeader("Low Level Total " + basisHeader.Name, true, 0, null);

                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, storeProfile.Text + "|Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name));

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                if (((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
                                                {
                                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //==============
                            // Store Balance
                            //==============

                            groupHeader = new RowColProfileHeader("Store Balance", true, 0, null);

                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, "Balance", storeProfile.Text + "|Balance"));

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    groupHeader = new RowColProfileHeader("Store Balance " + basisHeader.Name, true, 0, null);

                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, storeProfile.Text + "|Balance|" + ((BasisProfile)basisHeader.Profile).Name));
                                }
                            }

                            foreach (PlanProfile planProf in ManagerData.OpenParms.LowLevelPlanProfileList)
                            {
                                //================
                                // Store Low Level
                                //================

                                groupHeader = new RowColProfileHeader(planProf.NodeProfile.Text, true, 0, null);

                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, planProf.NodeProfile.Text, storeProfile.Text + "|" + planProf.NodeProfile.Text));

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
                                        //Begin Track #6010 - JScott - Bad % Change on Basis2
                                        if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                        {
                                            //End Track #6010 - JScott - Bad % Change on Basis2
                                            if (((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + planProf.NodeProfile.Text + "|" + detailHeader.Name));
                                            }
                                            //Begin Track #6010 - JScott - Bad % Change on Basis2
                                        }
                                        //End Track #6010 - JScott - Bad % Change on Basis2
                                    }
                                }

                                foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                                {
                                    if (basisHeader.IsDisplayed)
                                    {
                                        groupHeader = new RowColProfileHeader(planProf.NodeProfile.Text + basisHeader.Name, true, 0, null);

                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                        //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                        //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, storeProfile.Text + "|" + planProf.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name));
                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key), storeProfile.Text + "|" + planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key)));
                                        //End Track #5782

                                        foreach (RowColProfileHeader detailHeader in compList)
                                        {
                                            if (detailHeader.IsDisplayed)
                                            {
                                                if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                    detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                                {
                                                    if (((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
                                                    {
                                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                        //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                        //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + planProf.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key) + "|" + detailHeader.Name));
                                                        //End Track #5782
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        break;
                }

                aGrid.Cols.Count = FIXEDROWHEADERS;
                aGrid.Cols.Fixed = FIXEDROWHEADERS;

                gridRowList.BuildGridRows(aGrid, 0);

                if (aViewableGrid)
                {
                    ((PagingGridTag)aGrid.Tag).Visible = true;
                }
                ((PagingGridTag)aGrid.Tag).DetailsPerGroup = gridRowList.RowsPerGroup;
                ((PagingGridTag)aGrid.Tag).UnitsPerScroll = gridRowList.RowsPerGroup;

                if (aViewableGrid)
                {
                    _gridData[Grid4] = new ROCell[aGrid.Rows.Count, aGrid.Cols.Count];
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private string LoadBasisLabel()
        {
            try
            {
                //---Load Basis Label-----------
                string tmpBasisLabel = "";
                string concat = "";

                MIDRetail.Data.GlobalOptions opts = new MIDRetail.Data.GlobalOptions();
                DataTable dt = opts.GetGlobalOptions();
                DataRow dr = dt.Rows[0];

                _productDisplayCombination = Convert.ToInt32(dr["PRODUCT_LEVEL_DISPLAY_ID"], CultureInfo.CurrentUICulture);
                _storeDisplayCombination = Convert.ToInt32(dr["STORE_DISPLAY_OPTION_ID"], CultureInfo.CurrentUICulture);

                BasisLabelTypeProfile viewVarProf;
                ProfileList varProfList = GetBasisLabelProfList(Convert.ToInt32(dr["SYSTEM_OPTION_RID"], CultureInfo.CurrentUICulture));
                dt = opts.GetBasisLabelInfo(Convert.ToInt32(dr["SYSTEM_OPTION_RID"], CultureInfo.CurrentUICulture));
                foreach (DataRow releaseRow in dt.Rows)
                {
                    int basisLabelType = Convert.ToInt32(releaseRow["LABEL_TYPE"], CultureInfo.CurrentUICulture);
                    BasisLabelTypeProfile bltp = new BasisLabelTypeProfile(basisLabelType);
                    viewVarProf = (BasisLabelTypeProfile)varProfList.FindKey(basisLabelType);
                    bltp.BasisLabelSystemOptionRID = Convert.ToInt32(releaseRow["SYSTEM_OPTION_RID"], CultureInfo.CurrentUICulture);
                    bltp.BasisLabelName = Convert.ToString(viewVarProf.BasisLabelName);
                    bltp.BasisLabelType = basisLabelType;
                    bltp.BasisLabelSequence = Convert.ToInt32(releaseRow["LABEL_SEQ"], CultureInfo.CurrentUICulture);
                    tmpBasisLabel = tmpBasisLabel + concat + bltp.BasisLabelName;
                    concat = " / ";
                }
                return tmpBasisLabel;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }

        }


        private ProfileList GetBasisLabelProfList(int systemOptionRID)
        {
            ProfileList basisLabelList = new ProfileList(eProfileType.BasisLabelType);

            Array values;
            string[] names;

            values = System.Enum.GetValues(typeof(eBasisLabelType));
            names = System.Enum.GetNames(typeof(eBasisLabelType));

            for (int i = 0; i < names.Length; i++)
            {
                BasisLabelTypeProfile bltp = new BasisLabelTypeProfile(i);
                bltp.BasisLabelSystemOptionRID = systemOptionRID;
                bltp.BasisLabelName = names[i];
                bltp.BasisLabelType = i;
                bltp.BasisLabelSequence = -1;

                basisLabelList.Add(bltp);
            }
            return basisLabelList;
        }

        private string GetBasisLabel(int basisHeaderKey, int hierarchyNodeKey)
        {
            HashKeyObject activeKey = new HashKeyObject(hierarchyNodeKey, basisHeaderKey);
            return (string)_basisLabelList[activeKey];
        }

        private void Formatg5Grid(bool aClearGrid)
        {
            int i;
            int j;
            int rowsPerGroup;
            VariableProfile varProf;

            try
            {
                if (aClearGrid)
                {
                    g5.Clear();
                }

                if (g5.Tag == null)
                {
                    g5.Tag = new PagingGridTag(Grid5, g5, g4, g2, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE);
                }

                if (ManagerData.OpenParms.GetSummaryDateProfile(ManagerData.SAB.ClientServerSession) != null && ((PagingGridTag)g4.Tag).Visible)
                {
                    g5.Rows.Count = g4.Rows.Count;
                    g5.Cols.Count = g2.Cols.Count;
                    g5.Rows.Fixed = 0;
                    g5.Cols.Fixed = 0;

                    foreach (Row row in g5.Rows)
                    {
                        row.UserData = new RowTag();
                    }

                    foreach (Column col in g5.Cols)
                    {
                        col.UserData = new ColumnTag();
                    }

                    ((PagingGridTag)g5.Tag).Visible = true;

                    //Begin Track #5006 - JScott - Display Low-levels one at a time
                    //if (ManagerData.OpenParms.PlanSessionType == ePlanSessionType.ChainSingleLevel)
                    if (ManagerData.OpenParms.PlanSessionType == ePlanSessionType.ChainSingleLevel)
                    //End Track #5006 - JScott - Display Low-levels one at a time
                    {
                        if (g5.Rows.Count > 0 && g5.Cols.Count > 0)
                        {
                            rowsPerGroup = ((PagingGridTag)g4.Tag).DetailsPerGroup;

                            for (i = 0; i < g5.Rows.Count; i += rowsPerGroup)
                            {
                                varProf = (VariableProfile)((RowColProfileHeader)((RowHeaderTag)g4.Rows[i].UserData).GroupRowColHeader).Profile;

                                for (j = 0; j < varProf.TimeTotalChainVariables.Count; j++)
                                {
                                    g5.SetCellStyle(i, j, (CellStyle)null);
                                    g5.SetData(i, j, ((TimeTotalVariableProfile)varProf.GetChainTimeTotalVariable(j + 1)).VariableName);
                                }
                            }
                        }
                    }
                }
                else
                {
                    g5.Cols.Count = 0;
                    g5.Rows.Count = 0;
                    ((PagingGridTag)g5.Tag).Visible = false;
                }

                _gridData[Grid5] = new ROCell[g5.Rows.Count, g5.Cols.Count];
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Formatg6Grid(bool aClearGrid)
        {
            try
            {
                if (aClearGrid)
                {
                    g6.Clear();
                }

                if (g6.Tag == null)
                {
                    g6.Tag = new PagingGridTag(Grid6, g6, g4, g3, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE);
                }

                g6.Rows.Count = g4.Rows.Count;
                g6.Cols.Count = g3.Cols.Count;
                g6.Rows.Fixed = 0;
                g6.Cols.Fixed = 0;

                foreach (Row row in g6.Rows)
                {
                    row.UserData = new RowTag();
                }

                foreach (Column col in g6.Cols)
                {
                    col.UserData = new ColumnTag();
                }

                ((PagingGridTag)g6.Tag).Visible = true;

                _gridData[Grid6] = new ROCell[g6.Rows.Count, g6.Cols.Count];
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Formatg7Grid(bool aClearGrid)
        {
            CubeWaferCoordinateList cubeWaferCoordinateList;
            ArrayList compList;
            GridRowList gridRowList;
            int initVisibleRows = 0;

            try
            {
                if (aClearGrid)
                {
                    g7.Clear();
                }

                if (g7.Tag == null)
                {
                    g7.Tag = new PagingGridTag(Grid7, g7, g7, null, null, 0, 0);
                }

                compList = new ArrayList();
                gridRowList = new GridRowList();

                //Begin Track #5006 - JScott - Display Low-levels one at a time
                //switch (ManagerData.OpenParms.PlanSessionType)
                switch (ManagerData.OpenParms.PlanSessionType)
                //End Track #5006 - JScott - Display Low-levels one at a time
                {
                    case ePlanSessionType.ChainSingleLevel:
                    case ePlanSessionType.ChainMultiLevel:

                        g7.Cols.Count = 0;
                        g7.Rows.Count = 0;

                        ((PagingGridTag)g7.Tag).Visible = false;
                        ((PagingGridTag)g7.Tag).DetailsPerGroup = 0;
                        ((PagingGridTag)g7.Tag).UnitsPerScroll = 0;
                        break;

                    case ePlanSessionType.StoreSingleLevel:

                        foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
                        {
                            if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                            {
                                if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isStoreSetCube)
                                    {
                                        compList.Add(detailHeader);
                                    }
                                }
                            }
                        }

                        ((PagingGridTag)g7.Tag).GroupsPerGrid = _storeGroupLevelProfileList.Count;

                        foreach (RowColProfileHeader groupHeader in _selectableStoreAttributeHeaders)
                        {
                            if (_adjustmentRowHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
                                //End Track #5006 - JScott - Display Low-levels one at a time
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _originalRowHeader, gridRowList.Count, groupHeader.Name, groupHeader.Name), true);

                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
                                //End Track #5006 - JScott - Display Low-levels one at a time
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _adjustmentRowHeader, gridRowList.Count, "ADJ", groupHeader.Name + "|ADJ"));
                            }
                            else
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
                                //End Track #5006 - JScott - Display Low-levels one at a time
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, groupHeader.Name, groupHeader.Name), true);
                            }

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
                                    if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                    {
                                        //End Track #6010 - JScott - Bad % Change on Basis2
                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                        //Begin Track #5006 - JScott - Display Low-levels one at a time
                                        //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                        //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
                                        //End Track #5006 - JScott - Display Low-levels one at a time
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + detailHeader.Name));
                                        //Begin Track #6010 - JScott - Bad % Change on Basis2
                                    }
                                    //End Track #6010 - JScott - Bad % Change on Basis2
                                }
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                    //Begin Track #5006 - JScott - Display Low-levels one at a time
                                    //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                    //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
                                    //End Track #5006 - JScott - Display Low-levels one at a time
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                    //Begin Track #5006 - JScott - Display Low-levels one at a time
                                    ////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                    ////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, groupHeader.Name + "|" + ((BasisProfile)basisHeader.Profile).Name));
                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key), groupHeader.Name + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key)));
                                    ////End Track #5782
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key), groupHeader.Name + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key)));
                                    //End Track #5006 - JScott - Display Low-levels one at a time

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
                                                //End Track #5006 - JScott - Display Low-levels one at a time
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                                ////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                ////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
                                                ////End Track #5782
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
                                                //End Track #5006 - JScott - Display Low-levels one at a time
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        g7.Cols.Count = FIXEDROWHEADERS;
                        g7.Cols.Fixed = FIXEDROWHEADERS;

                        gridRowList.BuildGridRows(g7, 0);

                        ((PagingGridTag)g7.Tag).Visible = true;
                        ((PagingGridTag)g7.Tag).DetailsPerGroup = gridRowList.RowsPerGroup;
                        ((PagingGridTag)g7.Tag).UnitsPerScroll = gridRowList.RowsPerGroup;

                        break;

                    case ePlanSessionType.StoreMultiLevel:

                        foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
                        {
                            if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                            {
                                if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isStoreSetCube)
                                    {
                                        compList.Add(detailHeader);
                                    }
                                }
                            }
                        }

                        ((PagingGridTag)g7.Tag).GroupsPerGrid = _storeGroupLevelProfileList.Count;

                        foreach (RowColProfileHeader groupHeader in _selectableStoreAttributeHeaders)
                        {
                            //=================
                            // Store High Level
                            //=================

                            initVisibleRows = 0;

                            if (_adjustmentRowHeader.IsDisplayed)
                            {
                                initVisibleRows++;
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _originalRowHeader, gridRowList.Count, groupHeader.Name, groupHeader.Name), true);

                                initVisibleRows++;
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _adjustmentRowHeader, gridRowList.Count, "ADJ", groupHeader.Name + "|ADJ"));
                            }
                            else
                            {
                                initVisibleRows++;
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, groupHeader.Name, groupHeader.Name), true);
                            }

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
                                    if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                    {
                                        //End Track #6010 - JScott - Bad % Change on Basis2
                                        if (((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                                        {
                                            initVisibleRows++;
                                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + detailHeader.Name));
                                        }
                                        //Begin Track #6010 - JScott - Bad % Change on Basis2
                                    }
                                    //End Track #6010 - JScott - Bad % Change on Basis2
                                }
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    initVisibleRows++;
                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                    //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, groupHeader.Name + "|" + ((BasisProfile)basisHeader.Profile).Name));
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key), groupHeader.Name + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key)));
                                    //End Track #5782

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                if (((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                                                {
                                                    initVisibleRows++;
                                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                    //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
                                                    //End Track #5782
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //======================
                            // Store Low Level Total
                            //======================

                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, "Low Level Total", groupHeader.Name + "|Low Level Total"));

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
                                    if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                    {
                                        //End Track #6010 - JScott - Bad % Change on Basis2
                                        if (((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
                                        {
                                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|Low Level Total|" + detailHeader.Name));
                                        }
                                        //Begin Track #6010 - JScott - Bad % Change on Basis2
                                    }
                                    //End Track #6010 - JScott - Bad % Change on Basis2
                                }
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, groupHeader.Name + "|Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name));

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                if (((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
                                                {
                                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //==============
                            // Store Balance
                            //==============

                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, "Balance", groupHeader.Name + "|Balance"));

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, groupHeader.Name + "|Balance|" + ((BasisProfile)basisHeader.Profile).Name));
                                }
                            }

                            foreach (PlanProfile planProf in ManagerData.OpenParms.LowLevelPlanProfileList)
                            {
                                //================
                                // Store Low Level
                                //================

                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, planProf.NodeProfile.Text, groupHeader.Name + "|" + planProf.NodeProfile.Text));

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
                                        //Begin Track #6010 - JScott - Bad % Change on Basis2
                                        if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                        {
                                            //End Track #6010 - JScott - Bad % Change on Basis2
                                            if (((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + planProf.NodeProfile.Text + "|" + detailHeader.Name));
                                            }
                                            //Begin Track #6010 - JScott - Bad % Change on Basis2
                                        }
                                        //End Track #6010 - JScott - Bad % Change on Basis2
                                    }
                                }

                                foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                                {
                                    if (basisHeader.IsDisplayed)
                                    {
                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                        //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                        //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, groupHeader.Name + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + planProf.NodeProfile.Text));
                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key), groupHeader.Name + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key) + "|" + planProf.NodeProfile.Text));
                                        //End Track #5782

                                        foreach (RowColProfileHeader detailHeader in compList)
                                        {
                                            if (detailHeader.IsDisplayed)
                                            {
                                                if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                    detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                                {
                                                    if (((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
                                                    {
                                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                        //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                        //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + planProf.NodeProfile.Text + "|" + detailHeader.Name));
                                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key) + "|" + planProf.NodeProfile.Text + "|" + detailHeader.Name));
                                                        //End Track #5782
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        g7.Cols.Count = FIXEDROWHEADERS;
                        g7.Cols.Fixed = FIXEDROWHEADERS;

                        gridRowList.BuildGridRows(g7, 0);

                        ((PagingGridTag)g7.Tag).Visible = true;
                        ((PagingGridTag)g7.Tag).DetailsPerGroup = gridRowList.RowsPerGroup;
                        ((PagingGridTag)g7.Tag).UnitsPerScroll = gridRowList.RowsPerGroup;
                        ((PagingGridTag)g7.Tag).InitialVisibleRows = initVisibleRows;

                        break;
                }

                _gridData[Grid7] = new ROCell[g7.Rows.Count, g7.Cols.Count];
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
                    g8.Tag = new PagingGridTag(Grid8, g8, g7, g2, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE);
                }

                //Begin Track #5006 - JScott - Display Low-levels one at a time
                //if (ManagerData.OpenParms.GetSummaryDateProfile(ManagerData.SAB.ClientServerSession) != null && ((PagingGridTag)g7.Tag).Visible && (ManagerData.OpenParms.PlanSessionType == ePlanSessionType.StoreSingleLevel || ManagerData.OpenParms.PlanSessionType == ePlanSessionType.StoreMultiLevel))
                if (ManagerData.OpenParms.GetSummaryDateProfile(ManagerData.SAB.ClientServerSession) != null && ((PagingGridTag)g7.Tag).Visible && (ManagerData.OpenParms.PlanSessionType == ePlanSessionType.StoreSingleLevel || ManagerData.OpenParms.PlanSessionType == ePlanSessionType.StoreMultiLevel))
                //End Track #5006 - JScott - Display Low-levels one at a time
                {
                    g8.Rows.Count = g7.Rows.Count;
                    g8.Cols.Count = g2.Cols.Count;
                    g8.Rows.Fixed = 0;
                    g8.Cols.Fixed = 0;

                    foreach (Row row in g8.Rows)
                    {
                        row.UserData = new RowTag();
                    }

                    foreach (Column col in g8.Cols)
                    {
                        col.UserData = new ColumnTag();
                    }

                    ((PagingGridTag)g8.Tag).Visible = true;
                }
                else
                {
                    g8.Cols.Count = 0;
                    g8.Rows.Count = 0;
                    ((PagingGridTag)g8.Tag).Visible = false;
                }

                _gridData[Grid8] = new ROCell[g8.Rows.Count, g8.Cols.Count];
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
                    g9.Tag = new PagingGridTag(Grid9, g9, g7, g3, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE);
                }

                //Begin Track #5006 - JScott - Display Low-levels one at a time
                //if (ManagerData.OpenParms.PlanSessionType == ePlanSessionType.StoreSingleLevel || ManagerData.OpenParms.PlanSessionType == ePlanSessionType.StoreMultiLevel)
                if (ManagerData.OpenParms.PlanSessionType == ePlanSessionType.StoreSingleLevel || ManagerData.OpenParms.PlanSessionType == ePlanSessionType.StoreMultiLevel)
                //End Track #5006 - JScott - Display Low-levels one at a time
                {
                    g9.Rows.Count = g7.Rows.Count;
                    g9.Cols.Count = g3.Cols.Count;
                    g9.Rows.Fixed = 0;
                    g9.Cols.Fixed = 0;

                    foreach (Row row in g9.Rows)
                    {
                        row.UserData = new RowTag();
                    }

                    foreach (Column col in g9.Cols)
                    {
                        col.UserData = new ColumnTag();
                    }

                    ((PagingGridTag)g9.Tag).Visible = true;
                }
                else
                {
                    g9.Cols.Count = 0;
                    g9.Rows.Count = 0;
                    ((PagingGridTag)g9.Tag).Visible = false;
                }

                _gridData[Grid9] = new ROCell[g9.Rows.Count, g9.Cols.Count];
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Formatg10Grid(bool aClearGrid)
        {
            int i = 0;
            CubeWaferCoordinateList cubeWaferCoordinateList;
            ArrayList compList;
            GridRowList gridRowList;
            int initVisibleRows = 0;

            try
            {
                if (aClearGrid)
                {
                    g10.Clear();
                }

                if (g10.Tag == null)
                {
                    g10.Tag = new PagingGridTag(Grid10, g10, g10, null, null, 0, 0);
                }

                compList = new ArrayList();
                gridRowList = new GridRowList();

                //Begin Track #5006 - JScott - Display Low-levels one at a time
                //switch (ManagerData.OpenParms.PlanSessionType)
                switch (ManagerData.OpenParms.PlanSessionType)
                //End Track #5006 - JScott - Display Low-levels one at a time
                {
                    case ePlanSessionType.ChainSingleLevel:

                        g10.Cols.Count = 0;
                        g10.Rows.Count = 0;

                        ((PagingGridTag)g10.Tag).Visible = false;
                        ((PagingGridTag)g10.Tag).DetailsPerGroup = 0;
                        ((PagingGridTag)g10.Tag).UnitsPerScroll = 0;
                        break;

                    case ePlanSessionType.StoreSingleLevel:

                        foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
                        {
                            if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                            {
                                if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isHighLevel &&
                                        (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube ||
                                        ((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube))
                                    {
                                        compList.Add(detailHeader);
                                    }
                                }
                            }
                        }

                        ((PagingGridTag)g10.Tag).GroupsPerGrid = 2;

                        // All Store

                        i = 0;

                        if (_adjustmentRowHeader.IsDisplayed)
                        {
                            i++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                            //Begin Track #5006 - JScott - Display Low-levels one at a time
                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
                            //End Track #5006 - JScott - Display Low-levels one at a time
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _originalRowHeader, gridRowList.Count, "All Store", "All Store"), true);

                            i++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                            //Begin Track #5006 - JScott - Display Low-levels one at a time
                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
                            //End Track #5006 - JScott - Display Low-levels one at a time
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _adjustmentRowHeader, gridRowList.Count, "ADJ", "All Store|ADJ"));
                        }
                        else
                        {
                            i++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                            //Begin Track #5006 - JScott - Display Low-levels one at a time
                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
                            //End Track #5006 - JScott - Display Low-levels one at a time
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, "All Store", "All Store"), true);
                        }

                        foreach (RowColProfileHeader detailHeader in compList)
                        {
                            if (detailHeader.IsDisplayed)
                            {
                                //Begin Track #6010 - JScott - Bad % Change on Basis2
                                if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                {
                                    //End Track #6010 - JScott - Bad % Change on Basis2
                                    i++;

                                    if (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube)
                                    {
                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                        //Begin Track #5006 - JScott - Display Low-levels one at a time
                                        //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                        //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
                                        //End Track #5006 - JScott - Display Low-levels one at a time
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + detailHeader.Name));
                                    }
                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
                                }
                                //End Track #6010 - JScott - Bad % Change on Basis2
                            }
                        }

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                i++;
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
                                //End Track #5006 - JScott - Display Low-levels one at a time
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                ////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                ////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "All Store|" + ((BasisProfile)basisHeader.Profile).Name));
                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key), "All Store|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key)));
                                ////End Track #5782
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key), "All Store|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key)));
                                //End Track #5006 - JScott - Display Low-levels one at a time

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                        {
                                            i++;

                                            if (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
                                                //End Track #5006 - JScott - Display Low-levels one at a time
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                                ////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                ////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
                                                ////End Track #5782
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
                                                //End Track #5006 - JScott - Display Low-levels one at a time
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        gridRowList.RowsPerGroup = i;

                        // Chain

                        if (_adjustmentRowHeader.IsDisplayed)
                        {
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            //Begin Track #5006 - JScott - Display Low-levels one at a time
                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
                            //End Track #5006 - JScott - Display Low-levels one at a time
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _originalRowHeader, gridRowList.Count, "Chain", "Chain"), true);

                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            //Begin Track #5006 - JScott - Display Low-levels one at a time
                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
                            //End Track #5006 - JScott - Display Low-levels one at a time
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _adjustmentRowHeader, gridRowList.Count, "ADJ", "Chain|ADJ"));
                        }
                        else
                        {
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            //Begin Track #5006 - JScott - Display Low-levels one at a time
                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
                            //End Track #5006 - JScott - Display Low-levels one at a time
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, "Chain", "Chain"), true);
                        }

                        foreach (RowColProfileHeader detailHeader in compList)
                        {
                            if (detailHeader.IsDisplayed)
                            {
                                //Begin Track #6010 - JScott - Bad % Change on Basis2
                                if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                {
                                    //End Track #6010 - JScott - Bad % Change on Basis2
                                    if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube)
                                    {
                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                        //Begin Track #5006 - JScott - Display Low-levels one at a time
                                        //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                                        //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
                                        //End Track #5006 - JScott - Display Low-levels one at a time
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|" + detailHeader.Name));
                                    }
                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
                                }
                                //End Track #6010 - JScott - Bad % Change on Basis2
                            }
                        }

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
                                //End Track #5006 - JScott - Display Low-levels one at a time
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                ////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                ////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "Chain|" + ((BasisProfile)basisHeader.Profile).Name));
                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key), "Chain|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key)));
                                ////End Track #5782
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentChainPlanProfile.NodeProfile.Key), "Chain|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentChainPlanProfile.NodeProfile.Key)));
                                //End Track #5006 - JScott - Display Low-levels one at a time

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                        {
                                            if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
                                                //End Track #5006 - JScott - Display Low-levels one at a time
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                                ////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                ////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
                                                ////End Track #5782
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentChainPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
                                                //End Track #5006 - JScott - Display Low-levels one at a time
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        g10.Cols.Count = FIXEDROWHEADERS;
                        g10.Cols.Fixed = FIXEDROWHEADERS;

                        gridRowList.BuildGridRows(g10, 0);

                        ((PagingGridTag)g10.Tag).Visible = true;
                        ((PagingGridTag)g10.Tag).DetailsPerGroup = gridRowList.RowsPerGroup;
                        ((PagingGridTag)g10.Tag).UnitsPerScroll = gridRowList.RowsPerGroup;

                        break;

                    case ePlanSessionType.ChainMultiLevel:

                        foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
                        {
                            if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                            {
                                if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube)
                                    {
                                        compList.Add(detailHeader);
                                    }
                                }
                            }
                        }

                        ((PagingGridTag)g10.Tag).GroupsPerGrid = 3;

                        // Low Level Total

                        i = 0;

                        if (_adjustmentRowHeader.IsDisplayed)
                        {
                            i++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _originalRowHeader, gridRowList.Count, "Low Level Total", "Low Level Total"), true);

                            i++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _adjustmentRowHeader, gridRowList.Count, "ADJ", "Low Level Total|ADJ"));
                        }
                        else
                        {
                            i++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, "Low Level Total", "Low Level Total"), true);
                        }

                        foreach (RowColProfileHeader detailHeader in compList)
                        {
                            if (detailHeader.IsDisplayed)
                            {
                                //Begin Track #6010 - JScott - Bad % Change on Basis2
                                if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                {
                                    //End Track #6010 - JScott - Bad % Change on Basis2
                                    

                                    if (((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
                                    {
                                        i++;
                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Low Level Total|" + detailHeader.Name));
                                    }
                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
                                }
                                //End Track #6010 - JScott - Bad % Change on Basis2
                            }
                        }

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                i++;
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name));

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                        {
                                           

                                            if (((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
                                            {
                                                i++;
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        gridRowList.RowsPerGroup = i;

                        // High Level

                        if (_adjustmentRowHeader.IsDisplayed)
                        {
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _originalRowHeader, gridRowList.Count, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Text, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Text), true);

                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _adjustmentRowHeader, gridRowList.Count, "ADJ", ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Text + "|" + "ADJ"));
                        }
                        else
                        {
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Text, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Text), true);
                        }

                        foreach (RowColProfileHeader detailHeader in compList)
                        {
                            if (detailHeader.IsDisplayed)
                            {
                                //Begin Track #6010 - JScott - Bad % Change on Basis2
                                if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                {
                                    //End Track #6010 - JScott - Bad % Change on Basis2
                                    if (((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                                    {
                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Text + "|" + detailHeader.Name));
                                    }
                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
                                }
                                //End Track #6010 - JScott - Bad % Change on Basis2
                            }
                        }

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key), ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key)));
                                //End Track #5782

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                        {
                                            if (((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
                                                //End Track #5782
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Balance

                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, "Balance", "Balance"), true);

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "Balance|" + ((BasisProfile)basisHeader.Profile).Name));
                            }
                        }

                        g10.Cols.Count = FIXEDROWHEADERS;
                        g10.Cols.Fixed = FIXEDROWHEADERS;

                        gridRowList.BuildGridRows(g10, 0);

                        ((PagingGridTag)g10.Tag).Visible = true;
                        ((PagingGridTag)g10.Tag).DetailsPerGroup = gridRowList.RowsPerGroup;
                        ((PagingGridTag)g10.Tag).UnitsPerScroll = gridRowList.RowsPerGroup;

                        break;

                    case ePlanSessionType.StoreMultiLevel:

                        foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
                        {
                            if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                            {
                                if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube ||
                                        ((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube)
                                    {
                                        compList.Add(detailHeader);
                                    }
                                }
                            }
                        }

                        ((PagingGridTag)g10.Tag).GroupsPerGrid = 3;

                        // All Store High Level

                        i = 0;
                        initVisibleRows = 0;

                        if (_adjustmentRowHeader.IsDisplayed)
                        {
                            i++;
                            initVisibleRows++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _originalRowHeader, gridRowList.Count, "All Store", "All Store"), true);

                            i++;
                            initVisibleRows++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _adjustmentRowHeader, gridRowList.Count, "ADJ", "All Store|ADJ"));
                        }
                        else
                        {
                            i++;
                            initVisibleRows++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, "All Store", "All Store"), true);
                        }

                        foreach (RowColProfileHeader detailHeader in compList)
                        {
                            if (detailHeader.IsDisplayed)
                            {
                                //Begin Track #6010 - JScott - Bad % Change on Basis2
                                if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                {
                                    //End Track #6010 - JScott - Bad % Change on Basis2
                                    i++;
                                    initVisibleRows++;

                                    if (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube &&
                                        ((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                                    {
                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + detailHeader.Name));
                                    }
                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
                                }
                                //End Track #6010 - JScott - Bad % Change on Basis2
                            }
                        }

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                i++;
                                initVisibleRows++;
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "All Store|" + ((BasisProfile)basisHeader.Profile).Name));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key), "All Store|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key)));
                                //End Track #5782

                                

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                        {
                                            i++;
                                            initVisibleRows++;

                                            if (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube &&
                                                ((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.StoreHLPlanProfile.VersionProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.StoreHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
                                                //End Track #5782
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // All Store Low Level Total

                        i++;
                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, "Low Level Total", "All Store|Low Level Total"));

                        foreach (RowColProfileHeader detailHeader in compList)
                        {
                            if (detailHeader.IsDisplayed)
                            {
                                //Begin Track #6010 - JScott - Bad % Change on Basis2
                                if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                {
                                    //End Track #6010 - JScott - Bad % Change on Basis2
                                    i++;

                                    if (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube &&
                                        ((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
                                    {
                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|Low Level Total|" + detailHeader.Name));
                                    }
                                }
                                //Begin Track #6010 - JScott - Bad % Change on Basis2
                            }
                            //End Track #6010 - JScott - Bad % Change on Basis2
                        }

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                i++;
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "All Store|Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name));

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                        {
                                            i++;

                                            if (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube &&
                                                ((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // All Store Balance

                        i++;
                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, "Balance", "All Store|Balance"));

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                i++;
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "All Store|Balance|" + ((BasisProfile)basisHeader.Profile).Name));
                            }
                        }

                        foreach (PlanProfile planProf in ManagerData.OpenParms.LowLevelPlanProfileList)
                        {
                            // All Store Low Level

                            i++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, planProf.NodeProfile.Text, "All Store|" + planProf.NodeProfile.Text));

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
                                    if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                    {
                                        //End Track #6010 - JScott - Bad % Change on Basis2
                                        i++;

                                        if (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube &&
                                            ((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
                                        {
                                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + planProf.NodeProfile.Text + "|" + detailHeader.Name));
                                        }
                                        //Begin Track #6010 - JScott - Bad % Change on Basis2
                                    }
                                    //End Track #6010 - JScott - Bad % Change on Basis2
                                }
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    i++;

                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                    //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "All Store|" + planProf.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name));
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key), "All Store|" + planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key)));
                                    //End Track #5782

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                i++;

                                                if (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube &&
                                                    ((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
                                                {
                                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                    //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + planProf.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key) + "|" + detailHeader.Name));
                                                    //End Track #5782
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        gridRowList.RowsPerGroup = i;

                        // Chain High Level

                        if (_adjustmentRowHeader.IsDisplayed)
                        {
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _originalRowHeader, gridRowList.Count, "Chain", "Chain"), true);

                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _adjustmentRowHeader, gridRowList.Count, "ADJ", "Chain|ADJ"));
                        }
                        else
                        {
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, "Chain", "Chain"), true);
                        }

                        foreach (RowColProfileHeader detailHeader in compList)
                        {
                            if (detailHeader.IsDisplayed)
                            {
                                if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube &&
                                        ((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                                    {
                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|" + detailHeader.Name));
                                    }
                                }
                            }
                        }

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key), "Chain|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key)));

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                        {
                                            if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube &&
                                                ((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Chain Low Level Total

                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, "Low Level Total", "Chain|Low Level Total"));

                        foreach (RowColProfileHeader detailHeader in compList)
                        {
                            if (detailHeader.IsDisplayed)
                            {
                                if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube &&
                                        ((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
                                    {
                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|Low Level Total|" + detailHeader.Name));
                                    }
                                }
                            }
                        }

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "Chain|Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name));

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                        {
                                            if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube &&
                                                ((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Chain Balance

                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, "Balance", "Chain|Balance"));

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "Chain|Balance|" + ((BasisProfile)basisHeader.Profile).Name));
                            }
                        }

                        foreach (PlanProfile planProf in ManagerData.OpenParms.LowLevelPlanProfileList)
                        {
                            // Chain Low Level

                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, planProf.NodeProfile.Text, "Chain|" + planProf.NodeProfile.Text));

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
                                    if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                                    {
                                        if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube &&
                                            ((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
                                        {
                                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|" + planProf.NodeProfile.Text + "|" + detailHeader.Name));
                                        }
                                    }
                                }
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key), "Chain|" + planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key)));

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube &&
                                                    ((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
                                                {
                                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|" + planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key) + "|" + detailHeader.Name));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Difference High Level

                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.DifferenceQuantity.Key));
                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, "Difference", "Difference"), true);

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ManagerData.OpenParms.ChainHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.DifferenceQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key), "Difference|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, ManagerData.OpenParms.ChainHLPlanProfile.NodeProfile.Key)));
                            }
                        }

                        g10.Cols.Count = FIXEDROWHEADERS;
                        g10.Cols.Fixed = FIXEDROWHEADERS;

                        gridRowList.BuildGridRows(g10, 0);

                        ((PagingGridTag)g10.Tag).Visible = true;
                        ((PagingGridTag)g10.Tag).DetailsPerGroup = gridRowList.RowsPerGroup;
                        ((PagingGridTag)g10.Tag).UnitsPerScroll = gridRowList.RowsPerGroup;
                        ((PagingGridTag)g10.Tag).InitialVisibleRows = initVisibleRows;

                        break;
                }

                _gridData[Grid10] = new ROCell[g10.Rows.Count, g10.Cols.Count];
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Formatg11Grid(bool aClearGrid)
        {
            try
            {
                if (aClearGrid)
                {
                    g11.Clear();
                }

                if (g11.Tag == null)
                {
                    g11.Tag = new PagingGridTag(Grid11, g11, g10, g2, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE);
                }

                if (ManagerData.OpenParms.GetSummaryDateProfile(ManagerData.SAB.ClientServerSession) != null && ((PagingGridTag)g10.Tag).Visible)
                {
                    g11.Rows.Count = g10.Rows.Count;
                    g11.Cols.Count = g2.Cols.Count;
                    g11.Rows.Fixed = 0;
                    g11.Cols.Fixed = 0;

                    foreach (Row row in g11.Rows)
                    {
                        row.UserData = new RowTag();
                    }

                    foreach (Column col in g11.Cols)
                    {
                        col.UserData = new ColumnTag();
                    }

                    ((PagingGridTag)g11.Tag).Visible = true;
                }
                else
                {
                    g11.Cols.Count = 0;
                    g11.Rows.Count = 0;
                    ((PagingGridTag)g11.Tag).Visible = false;
                }

                _gridData[Grid11] = new ROCell[g11.Rows.Count, g11.Cols.Count];
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Formatg12Grid(bool aClearGrid)
        {
            try
            {
                if (aClearGrid)
                {
                    g12.Clear();
                }

                if (g12.Tag == null)
                {
                    g12.Tag = new PagingGridTag(Grid12, g12, g10, g3, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE);
                }

                if (((PagingGridTag)g10.Tag).Visible)
                {
                    g12.Rows.Count = g10.Rows.Count;
                    g12.Cols.Count = g3.Cols.Count;
                    g12.Rows.Fixed = 0;
                    g12.Cols.Fixed = 0;

                    foreach (Row row in g12.Rows)
                    {
                        row.UserData = new RowTag();
                    }

                    foreach (Column col in g12.Cols)
                    {
                        col.UserData = new ColumnTag();
                    }

                    ((PagingGridTag)g12.Tag).Visible = true;
                }
                else
                {
                    g12.Cols.Count = 0;
                    g12.Rows.Count = 0;
                    ((PagingGridTag)g12.Tag).Visible = false;
                }

                _gridData[Grid12] = new ROCell[g12.Rows.Count, g12.Cols.Count];
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        #region Loading actual data into grids, including threading-populate

        /// <summary>
        /// This method is called by each process/event to load the current visible grid pages.
        /// </summary>

        //private void LoadCurrentPages() 
        //changed to public ... in order to call this after cell changes are made without having to reinitialize
        public void LoadCurrentPages()
        {
            try
            {
                //StopPageLoadThreads();

                ((PagingGridTag)g5.Tag).AllocatePageArray();
                ((PagingGridTag)g6.Tag).AllocatePageArray();
                ((PagingGridTag)g8.Tag).AllocatePageArray();
                ((PagingGridTag)g9.Tag).AllocatePageArray();
                ((PagingGridTag)g11.Tag).AllocatePageArray();
                ((PagingGridTag)g12.Tag).AllocatePageArray();

                if (g5.Rows.Count > 0 && g5.Cols.Count > 0)
                {
                    LoadCurrentGridPages(g5);
                }
                if (g6.Rows.Count > 0 && g6.Cols.Count > 0)
                {
                    LoadCurrentGridPages(g6);
                }
                if (g8.Rows.Count > 0 && g8.Cols.Count > 0)
                {
                    LoadCurrentGridPages(g8);
                }
                if (g9.Rows.Count > 0 && g9.Cols.Count > 0)
                {
                    LoadCurrentGridPages(g9);
                }
                if (g11.Rows.Count > 0 && g11.Cols.Count > 0)
                {
                    LoadCurrentGridPages(g11);
                }
                if (g12.Rows.Count > 0 && g12.Cols.Count > 0)
                {
                    LoadCurrentGridPages(g12);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// This method is called by each process/event to load the surrounding grid pages.
        /// </summary>

        private void LoadSurroundingPages()
        {
            try
            {
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
                if (g11.Rows.Count > 0 && g11.Cols.Count > 0)
                {
                    LoadSurroundingGridPages(g11);
                }
                if (g12.Rows.Count > 0 && g12.Cols.Count > 0)
                {
                    LoadSurroundingGridPages(g12);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// This method is called by each the "export" process to load all grid pages.
        /// </summary>

        private void LoadAllPages()
        {
            try
            {
                //StopPageLoadThreads();

                if (g5.Rows.Count > 0 && g5.Cols.Count > 0)
                {
                    LoadAllGridPages(g5);
                }
                if (g6.Rows.Count > 0 && g6.Cols.Count > 0)
                {
                    LoadAllGridPages(g6);
                }
                if (g8.Rows.Count > 0 && g8.Cols.Count > 0)
                {
                    LoadAllGridPages(g8);
                }
                if (g9.Rows.Count > 0 && g9.Cols.Count > 0)
                {
                    LoadAllGridPages(g9);
                }
                if (g11.Rows.Count > 0 && g11.Cols.Count > 0)
                {
                    LoadAllGridPages(g11);
                }
                if (g12.Rows.Count > 0 && g12.Cols.Count > 0)
                {
                    LoadAllGridPages(g12);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// This method is called by each the scroll bar "scroll" events to force the reload the current visible page.
        /// </summary>

        public void LoadCurrentGridPage(C1FlexGrid aGrid)
        {
            try
            {
                if (aGrid.Rows.Count > 0 && aGrid.Cols.Count > 0)
                {
                    LoadCurrentGridPages(aGrid);
                    LoadSurroundingGridPages(aGrid);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// This method is called by the page load routines to do the actual work of loading a current page.
        /// </summary>

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
                            gridTag.LoadPage(page);
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

        /// <summary>
        /// This method is called by the page load routines to do the actual work of loading surrounding pages.
        /// </summary>

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

        /// <summary>
        /// This method is called by the page load routines to do the actual work of loading all pages.
        /// </summary>

        public void LoadAllGridPages(C1FlexGrid aGrid)
        {
            PagingGridTag gridTag;
            ArrayList pages;
            Cursor holdCursor;

            try
            {
                gridTag = (PagingGridTag)aGrid.Tag;
                pages = gridTag.GetPagesToLoad(0, 0, aGrid.Rows.Count - 1, aGrid.Cols.Count - 1);

                if (pages.Count > 0)
                {
                    holdCursor = Cursor.Current;
                    Cursor.Current = Cursors.WaitCursor;

                    try
                    {
                        foreach (Point page in pages)
                        {
                            gridTag.LoadPage(page);
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

        #endregion

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
            PlanWaferFlagCell[,] planWaferCellTable;
            CubeWafer cubeWafer;
            ColumnHeaderTag ColTag;
            RowHeaderTag RowTag;
            ComputationCellFlags planFlags = new ComputationCellFlags();
            //eCellDataType cellDataType = eCellDataType.Units;

            try
            {
                gridTag = (PagingGridTag)aGrid.Tag;

                //lock (_loadHash.SyncRoot)
                //{
                //    _loadHash.Add(System.Threading.Thread.CurrentThread.GetHashCode(), aPriority);
                //}

                wait = true;

                while (wait)
                {
                    wait = false;

                    //_pageLoadLock.AcquireWriterLock(-1);

                    try
                    {
                        //if (_stopPageLoadThread)
                        //{
                        //    throw new EndPageLoadThreadException();
                        //}

                        //lock (_loadHash.SyncRoot)
                        //{
                        //    foreach (int priority in _loadHash.Values)
                        //    {
                        //        if (priority < aPriority)
                        //        {
                        //            throw new WaitPageLoadException();
                        //        }
                        //    }

                        //    _loadHash.Remove(System.Threading.Thread.CurrentThread.GetHashCode());
                        //}

                        if (aStartRow <= aEndRow && aStartCol <= aEndCol)
                        {
                            rowHdrGrid = gridTag.RowHeaderGrid;
                            colHdrGrid = gridTag.ColHeaderGrid;

                            //Create the CubeWafer to request data
                            cubeWafer = new CubeWafer();

                            //Fill CommonWaferCoordinateListGroup
                            cubeWafer.CommonWaferCoordinateList = _commonWaferCoordinateList;

                            //Fill ColWaferCoordinateListGroup
                            cubeWafer.ColWaferCoordinateListGroup.Clear();

                            for (i = aStartCol; i <= aEndCol; i++)
                            {
                                ColTag = (ColumnHeaderTag)colHdrGrid.Cols[i].UserData;
                                if (ColTag != null)
                                {
                                    cubeWafer.ColWaferCoordinateListGroup.Add(ColTag.CubeWaferCoorList);
                                }
                            }

                            //Fill RowWaferCoordinateListGroup

                            cubeWafer.RowWaferCoordinateListGroup.Clear();
                            for (i = aStartRow; i <= aEndRow; i++)
                            {
                                RowTag = (RowHeaderTag)rowHdrGrid.Rows[i].UserData;
                                if (RowTag != null)
                                {
                                    cubeWafer.RowWaferCoordinateListGroup.Add(RowTag.CubeWaferCoorList);
                                }
                            }

                            if (cubeWafer.ColWaferCoordinateListGroup.Count > 0 && cubeWafer.RowWaferCoordinateListGroup.Count > 0)
                            {
                                // Retreive array of values

                                planWaferCellTable = ManagerData.PlanCubeGroup.GetPlanWaferCellValues(cubeWafer, ManagerData.UnitsScalingString, ManagerData.DollarScalingString);


                                // Load Grid with values

                                aGrid.Redraw = false;

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
                                                    //if (_stopPageLoadThread)
                                                    //{
                                                    //    throw new EndPageLoadThreadException();
                                                    //}

                                                    //Change for RO-3156 to Correct Store mismatch in "All Stores" view
                                                    //row = ((RowHeaderTag)rowHdrGrid.Rows[i].UserData).Order;
                                                    //col = ((ColumnHeaderTag)colHdrGrid.Cols[j].UserData).Order;
                                                    row = i;
                                                    col = j;

                                                    if (_gridData[gridTag.GridId][row, col] == null)
                                                    {
                                                        _gridData[gridTag.GridId][row, col] = new ROCell();
                                                    }
                                                    if (RowTag.LoadData)
                                                    {
                                                        if (planWaferCellTable[x, y] != null)
                                                        {
                                                            _gridData[gridTag.GridId][row, col].CellDataType = eCellDataType.Units;
                                                            switch (planWaferCellTable[x, y].VariableStyle)
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
                                                            aGrid[i, j] = planWaferCellTable[x, y].ValueAsString;
                                                            if (_gridData[gridTag.GridId][row, col].CellDataType == eCellDataType.Dollar
                                                                || _gridData[gridTag.GridId][row, col].CellDataType == eCellDataType.Percentage
                                                                || _gridData[gridTag.GridId][row, col].CellDataType == eCellDataType.Units)
                                                            {
                                                                _gridData[gridTag.GridId][row, col].Value = planWaferCellTable[x, y].Value;
                                                            }
                                                            else
                                                            {
                                                                _gridData[gridTag.GridId][row, col].Value = planWaferCellTable[x, y].ValueAsString;
                                                            }
                                                            planFlags = planWaferCellTable[x, y].Flags;

                                                            //if (!_gridData[gridTag.GridId][row, col].CellFlagsInited ||
                                                            //    planFlags.Flags != _gridData[gridTag.GridId][row, col].ComputationCellFlags.Flags ||
                                                            //    (planWaferCellTable[x, y] != null &&
                                                            //    planWaferCellTable[x, y].isValueNegative != _gridData[gridTag.GridId][row, col].isCellNegative))
                                                            //{
                                                            //    _gridData[gridTag.GridId][row, col].ComputationCellFlags = planFlags;
                                                            //    _gridData[gridTag.GridId][row, col].isCellNegative = planWaferCellTable[x, y].isValueNegative;
                                                            //    //ChangeCellStyles(aGrid, _gridData[gridTag.GridId][row, col], i, j);
                                                            //}
                                                            _gridData[gridTag.GridId][row, col].ROCellAttributes.DecimalPositions = planWaferCellTable[x, y].NumberOfDecimals;
                                                            _gridData[gridTag.GridId][row, col].ROCellAttributes.IsNegative = planWaferCellTable[x, y].isValueNegative;
                                                            _gridData[gridTag.GridId][row, col].ROCellAttributes.IsNumeric = planWaferCellTable[x, y].isValueNumeric;

                                                            ComputationCellFlags cellFlags = planWaferCellTable[x, y].Flags;

                                                            _gridData[gridTag.GridId][row, col].ROCellAttributes.IsIneligible = PlanCellFlagValues.isIneligible(cellFlags);
                                                            _gridData[gridTag.GridId][row, col].ROCellAttributes.IsProtected = PlanCellFlagValues.isProtected(cellFlags);

                                                            _gridData[gridTag.GridId][row, col].ROCellAttributes.IsLocked = ComputationCellFlagValues.isLocked(cellFlags);
                                                            _gridData[gridTag.GridId][row, col].ROCellAttributes.IsDisplayOnly = ComputationCellFlagValues.isDisplayOnly(cellFlags);
                                                            _gridData[gridTag.GridId][row, col].ROCellAttributes.IsModified = ComputationCellFlagValues.isChanged(cellFlags);
                                                            _gridData[gridTag.GridId][row, col].ROCellAttributes.IsClosed = PlanCellFlagValues.isClosed(cellFlags);
                                                            //RO-1170 Add Basis Flag from Header Record
                                                            if (RowTag.CubeWaferCoorList.FindCoordinateType(eProfileType.Basis) == null)
                                                            {
                                                                _gridData[gridTag.GridId][row, col].ROCellAttributes.IsBasis = false;
                                                            }
                                                            else
                                                            {
                                                                _gridData[gridTag.GridId][row, col].ROCellAttributes.IsBasis = true;
                                                            }

                                                            if (PlanCellFlagValues.isClosed(cellFlags) ||
                                                                ComputationCellFlagValues.isDisplayOnly(cellFlags) ||
                                                                ComputationCellFlagValues.isNull(cellFlags) ||
                                                                PlanCellFlagValues.isProtected(cellFlags) ||
                                                                ComputationCellFlagValues.isHidden(cellFlags) ||
                                                                ComputationCellFlagValues.isReadOnly(cellFlags))
                                                            {
                                                                _gridData[gridTag.GridId][row, col].ROCellAttributes.IsEditable = false;
                                                            }
                                                            else
                                                            {
                                                                _gridData[gridTag.GridId][row, col].ROCellAttributes.IsEditable = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            aGrid[i, j] = NULL_DATA_STRING;
                                                            planFlags.Clear();
                                                            ComputationCellFlagValues.isNull(ref planFlags, true);
                                                            _gridData[gridTag.GridId][row, col].CellDataType = eCellDataType.None;

                                                            //if (!_gridData[gridTag.GridId][row, col].CellFlagsInited ||
                                                            //    planFlags.Flags != _gridData[gridTag.GridId][row, col].ComputationCellFlags.Flags)
                                                            //{
                                                            //    _gridData[gridTag.GridId][row, col].ComputationCellFlags = planFlags;
                                                            //    //ChangeCellStyles(aGrid, _gridData[gridTag.GridId][row, col], i, j);
                                                            //}
                                                        }

                                                        //SetLockPicture(aGrid, planFlags, i, j);
                                                    }
                                                    else
                                                    {
                                                        planFlags.Clear();
                                                        ComputationCellFlagValues.isNull(ref planFlags, true);
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
                                    aGrid.Redraw = false;  // _currentRedrawState;
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
                        //_pageLoadLock.ReleaseWriterLock();
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void BuildWorkingStoreList(int aAttributeSetKey, ProfileList aWorkingDetailProfileList)
        {
            ProfileXRef storeSetXRef;
            ArrayList detailList;
            StoreProfile storeProf;

            try
            {
                storeSetXRef = (ProfileXRef)PlanCubeGroup.GetProfileXRef(new ProfileXRef(eProfileType.StoreGroupLevel, eProfileType.Store));
                detailList = storeSetXRef.GetDetailList(aAttributeSetKey);
                if (detailList != null)
                {
                    foreach (int storeId in detailList)
                    {
                        storeProf = (StoreProfile)_storeProfileList.FindKey(storeId);
                        if (storeProf != null)
                        {
                            aWorkingDetailProfileList.Add(storeProf);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void SortColumns(C1FlexGrid aGrid, ref structSort aSortParms)
        {
            PagingGridTag gridTag;
            SortValue valueData;
            ArrayList sortDirList;
            int i;
            int j;
            SortCriteria sortData;
            string cellValue;
            SortedList sortedList;
            int valueRow;
            ArrayList keyList;
            GridSortEntry sortEnt;
            ColumnHeaderTag colTag;
            C1FlexGrid colHdrGrid;
            ColumnHeaderTag colHdrTag;

            try
            {
                gridTag = (PagingGridTag)aGrid.Tag;

                sortDirList = new ArrayList();

                for (i = 0; !aSortParms.IsSortingByDefault && i < aSortParms.SortInfo.Count; i++)
                {
                    if (aSortParms.SortInfo[i] != null)
                    {
                        sortData = (SortCriteria)aSortParms.SortInfo[i];

                        if (sortData.Column1 != String.Empty)
                        {
                            GetCellRange(sortData.Column2GridPtr, 0, sortData.Column2Num, sortData.Column2GridPtr.Rows.Count - 1, sortData.Column2Num, 1);
                            sortDirList.Add(sortData.SortDirection);
                        }
                    }
                }

                sortedList = new SortedList(new SortComparer(sortDirList));

                for (i = 0; i < gridTag.GroupsPerGrid; i++)
                {
                    keyList = new ArrayList();

                    if (aSortParms.ValueInfo != null)
                    {
                        valueData = (SortValue)aSortParms.ValueInfo;
                        valueRow = (i * gridTag.DetailsPerGroup) + valueData.Row2Num;

                        for (j = 0; !aSortParms.IsSortingByDefault && j < aSortParms.SortInfo.Count; j++)
                        {
                            if (aSortParms.SortInfo[j] != null)
                            {
                                sortData = (SortCriteria)aSortParms.SortInfo[j];

                                if (sortData.Column1 != String.Empty)
                                {
                                    cellValue = Convert.ToString(sortData.Column2GridPtr[valueRow, sortData.Column2Num]).Trim();

                                    switch (valueData.Row2Format)
                                    {
                                        case eValueFormatType.None:

                                            switch (sortData.Column2Format)
                                            {
                                                case eValueFormatType.GenericNumeric:

                                                    if (cellValue.Length > 0)
                                                    {
                                                        keyList.Add(Convert.ToDouble(sortData.Column2GridPtr[valueRow, sortData.Column2Num]));
                                                    }
                                                    else
                                                    {
                                                        keyList.Add((double)0);
                                                    }
                                                    break;

                                                default:

                                                    keyList.Add(cellValue);
                                                    break;
                                            }

                                            break;

                                        case eValueFormatType.GenericNumeric:

                                            if (cellValue.Length > 0)
                                            {
                                                keyList.Add(Convert.ToDouble(sortData.Column2GridPtr[valueRow, sortData.Column2Num]));
                                            }
                                            else
                                            {
                                                keyList.Add((double)0);
                                            }
                                            break;

                                        default:

                                            keyList.Add(cellValue);
                                            break;
                                    }
                                }
                            }
                        }
                    }

                    //Begin Track #5006 - JScott - Display Low-levels one at a time
                    //if (_openParms.PlanSessionType == ePlanSessionType.ChainSingleLevel)
                    if (ManagerData.OpenParms.PlanSessionType == ePlanSessionType.ChainSingleLevel)
                    //End Track #5006 - JScott - Display Low-levels one at a time
                    {
                        sortedList.Add(new GridSortEntry(aGrid.Rows[i * gridTag.DetailsPerGroup], keyList, ((RowColProfileHeader)((RowHeaderTag)aGrid.Rows[i * gridTag.DetailsPerGroup].UserData).GroupRowColHeader).Profile.Key, i), null);
                    }
                    else
                    {
                        if (gridTag.GridId == Grid7)
                        {
                            sortedList.Add(new GridSortEntry(aGrid.Rows[i * gridTag.DetailsPerGroup], keyList, ((RowHeaderTag)aGrid.Rows[i * gridTag.DetailsPerGroup].UserData).GroupRowColHeader.Sequence, i), null);
                        }
                        else
                        {
                            sortedList.Add(new GridSortEntry(aGrid.Rows[i * gridTag.DetailsPerGroup], keyList, Convert.ToString(aGrid[i * gridTag.DetailsPerGroup, 0]), i), null);
                        }
                    }
                }

                for (i = 0; i < sortedList.Count; i++)
                {
                    sortEnt = (GridSortEntry)sortedList.GetKey(i);
                    MoveRows(aGrid, gridTag.DetailsPerGroup, sortEnt.RowIndex, i * gridTag.DetailsPerGroup);
                }

                for (i = 0; i < g2.Cols.Count; i++)
                {
                    colTag = (ColumnHeaderTag)g2.Cols[i].UserData;
                    colTag.Sort = SortEnum.none;
                    g2.Cols[i].UserData = colTag;
                }

                for (i = 0; i < g3.Cols.Count; i++)
                {
                    colTag = (ColumnHeaderTag)g3.Cols[i].UserData;
                    colTag.Sort = SortEnum.none;
                    g3.Cols[i].UserData = colTag;
                }

                for (i = 0; !aSortParms.IsSortingByDefault && i < aSortParms.SortInfo.Count; i++)
                {
                    if (aSortParms.SortInfo[i] != null)
                    {
                        sortData = (SortCriteria)aSortParms.SortInfo[i];

                        if (sortData.Column1 != String.Empty)
                        {
                            colHdrGrid = ((PagingGridTag)sortData.Column2GridPtr.Tag).ColHeaderGrid;
                            colHdrTag = (ColumnHeaderTag)colHdrGrid.Cols[sortData.Column2Num].UserData;

                            if (sortData.SortDirection == SortEnum.asc)
                            {
                                //colHdrGrid.SetCellImage(((PagingGridTag)colHdrGrid.Tag).SortImageRow, sortData.Column2Num, _upArrow);
                                colHdrTag.Sort = SortEnum.asc;
                            }
                            else
                            {
                                //colHdrGrid.SetCellImage(((PagingGridTag)colHdrGrid.Tag).SortImageRow, sortData.Column2Num, _downArrow);
                                colHdrTag.Sort = SortEnum.desc;
                            }

                            colHdrGrid.Cols[sortData.Column2Num].UserData = colHdrTag;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void SortToDefault()
        {
            try
            {
                if (ManagerData.OpenParms.PlanSessionType != ePlanSessionType.ChainSingleLevel)
                {
                    _currSortParms = new structSort();
                    _currSortParms.IsSortingByDefault = true;

                    SortColumns(g4, ref _currSortParms);
                    SortColumns(g7, ref _currSortParms);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void MoveRows(C1FlexGrid aGrid, int aRowsPerGroup, int OldIndex, int NewIndex)
        {
            Object detailProf;

            try
            {
                switch (((PagingGridTag)aGrid.Tag).GridId)
                {
                    case Grid4:

                        if (((PagingGridTag)g4.Tag).Visible)
                        {
                            g4.Rows.MoveRange(OldIndex, aRowsPerGroup, NewIndex);
                        }
                        if (((PagingGridTag)g5.Tag).Visible)
                        {
                            g5.Rows.MoveRange(OldIndex, aRowsPerGroup, NewIndex);
                        }
                        if (((PagingGridTag)g6.Tag).Visible)
                        {
                            g6.Rows.MoveRange(OldIndex, aRowsPerGroup, NewIndex);
                        }

                        detailProf = _workingDetailProfileList.ArrayList[OldIndex / aRowsPerGroup];
                        _workingDetailProfileList.ArrayList.RemoveAt(OldIndex / aRowsPerGroup);
                        _workingDetailProfileList.ArrayList.Insert(NewIndex / aRowsPerGroup, detailProf);

                        break;

                    case Grid7:

                        if (((PagingGridTag)g7.Tag).Visible)
                        {
                            g7.Rows.MoveRange(OldIndex, aRowsPerGroup, NewIndex);
                        }
                        if (((PagingGridTag)g8.Tag).Visible)
                        {
                            g8.Rows.MoveRange(OldIndex, aRowsPerGroup, NewIndex);
                        }
                        if (((PagingGridTag)g9.Tag).Visible)
                        {
                            g9.Rows.MoveRange(OldIndex, aRowsPerGroup, NewIndex);
                        }

                        break;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        internal void BuildGridsFromCubeData()
        {

            ShowGridValues(g2, 1);
            ShowGridValues(g3, 2);
            ShowGridValues(g4, 3);
            ShowGridValues(g5, 4);
            ShowGridValues(g6, 5);
            ShowGridValues(g7, 6);
            ShowGridValues(g8, 7);
            ShowGridValues(g9, 8);
            ShowGridValues(g10, 9);
            ShowGridValues(g11, 10);
            ShowGridValues(g12, 11);
        }

        private void ShowGridValues(C1.Win.C1FlexGrid.C1FlexGrid dataGrid, int gridTag)
        {
            if (dataGrid != null)
            {
#if (DEBUG)
                Debug.WriteLine("------ " + dataGrid.Name + "------ ");
                for (int r = 0; r < dataGrid.Rows.Count; r++)
                {
                    string line = string.Empty;
                    for (int c = 0; c < dataGrid.Cols.Count; c++)
                    {
                        line += dataGrid[r, c] + " , ";
                    }
                    Debug.WriteLine(line);
                }
#endif
                //create col and row headers
                switch (gridTag)
                {
                    case Grid2: //col headers g2
                        _g2ColHeaderList.Clear();
                        for (int c = 0; c < dataGrid.Cols.Count; c++)
                        {
                            _g2ColHeaderList.Add((ColumnHeaderTag)g2.Cols[c].UserData);
                        }
                        break;
                    case Grid3: //time tot col headers g3
                        _g3ColHeaderList.Clear();
                        for (int c = 0; c < dataGrid.Cols.Count; c++)
                        {
                            _g3ColHeaderList.Add((ColumnHeaderTag)g3.Cols[c].UserData);
                        }
                        break;
                    case Grid4: //row headers g4
                        _g4RowHeaderList.Clear();
                        for (int r = 0; r < dataGrid.Rows.Count; r++)
                        {
                            _g4RowHeaderList.Add((RowHeaderTag)g4.Rows[r].UserData);
                        }
                        break;
                    case Grid7: //row headers g7
                        _g7RowHeaderList.Clear();
                        for (int r = 0; r < dataGrid.Rows.Count; r++)
                        {
                            _g7RowHeaderList.Add((RowHeaderTag)g7.Rows[r].UserData);
                        }
                        break;
                    case Grid10: //row headers g10
                        _g10RowHeaderList.Clear();
                        for (int r = 0; r < dataGrid.Rows.Count; r++)
                        {
                            _g10RowHeaderList.Add((RowHeaderTag)g10.Rows[r].UserData);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

    }

    public enum GroupedBy
    {
        GroupedByTime,
        GroupedByVariable
    }

    public class SimpleRowHeader
    {
        public CubeWaferCoordinateList CubeWaferCoorList;
        public string RowHeading;
        public VariableProfile varProf;
        public string basisHeading;
        public string basisToolTip;
        public string basisHeaderName;
        public int basisHeaderSequence;

        public SimpleRowHeader(CubeWaferCoordinateList aCoorList, string aRowHeading, VariableProfile varProf)
        {
            CubeWaferCoorList = aCoorList;
            RowHeading = aRowHeading;
            this.varProf = varProf;
            this.basisHeading = string.Empty;
            this.basisToolTip = string.Empty;

        }
        public SimpleRowHeader(CubeWaferCoordinateList aCoorList, string aRowHeading, VariableProfile varProf, string basisHeading, string basisToolTip, string basisHeaderName, int basisHeaderSequence)
        {
            CubeWaferCoorList = aCoorList;
            RowHeading = aRowHeading;
            this.varProf = varProf;
            this.basisHeading = basisHeading;
            this.basisToolTip = basisToolTip;
            this.basisHeaderName = basisHeaderName;
            this.basisHeaderSequence = basisHeaderSequence;
        }
    }
    public class SimpleColumnHeader
    {
        public CubeWaferCoordinateList CubeWaferCoorList;


        public SimpleColumnHeader(CubeWaferCoordinateList aCoorList)
        {
            CubeWaferCoorList = aCoorList;

        }
    }

    public class chartColumnRef
    {
        public string columnName;
        public int cubeColumnIndex;
        public int cubeColumnADJ_Index;

        public chartColumnRef(string columnName, int cubeColumnIndex, int cubeColumnADJ_Index)
        {
            this.columnName = columnName;
            this.cubeColumnIndex = cubeColumnIndex;
            this.cubeColumnADJ_Index = cubeColumnADJ_Index;
        }
    }

    public class PagingCoordinates
    {
        private int _firstRowItem;
        private int _lastRowItem;
        private int _totalRowItems;
        private int _firstColItem;
        private int _lastColItem;
        private int _totalColItems;
        private int _countDistinctColumns;
        private int _columnsSelected;
        private int _firstColumnIndex;
        private int _lastColumnIndex;            

        public PagingCoordinates()
        {
        }

        public PagingCoordinates(
            int iFirstRowItem,
            int iLastRowItem,
            int iTotalRowItems,
            int iFirstColItem,
            int iLastColItem,
            int iTotalColItems,
            int iCountDistinctColumns,
            int iColumnsSelected,
            int iFirstColumnIndex,
            int iLastColumnIndex
            )
        {
            _firstRowItem = iFirstRowItem;
            _lastRowItem = iLastRowItem;
            _totalRowItems = iTotalRowItems;
            _firstColItem = iFirstColItem;
            _lastColItem = iLastColItem;
            _totalColItems = iTotalColItems;
            _countDistinctColumns = iCountDistinctColumns;
            _columnsSelected = iColumnsSelected;
            _firstColumnIndex = iFirstColumnIndex;
            _lastColumnIndex = iLastColumnIndex;
        }

        public int FirstRowItem { get { return _firstRowItem; } set { _firstRowItem = value; } }
        public int LastRowItem { get { return _lastRowItem; } set { _lastRowItem = value; } }
        public int TotalRowItems { get { return _totalRowItems; } set { _totalRowItems = value; } }
        public int FirstColItem { get { return _firstColItem; } set { _firstColItem = value; } }
        public int LastColItem { get { return _lastColItem; } set { _lastColItem = value; } }
        public int TotalColItems { get { return _totalColItems; } set { _totalColItems = value; } }
        public int CountDistinctColumns { get { return _countDistinctColumns; } set { _countDistinctColumns = value; } }
        public int ColumnsSelected { get { return _columnsSelected; } set { _columnsSelected = value; } }
        public int FirstColumnIndex { get { return _firstColumnIndex; } set { _firstColumnIndex = value; } }
        public int LastColumnIndex { get { return _lastColumnIndex; } set { _lastColumnIndex = value; } }

    }

    public struct SortCriteria
    {
        public string Column1;
        public string Column2;
        public int Column2Num;
        public FromGrid Column2Grid;
        public C1FlexGrid Column2GridPtr;
        public eValueFormatType Column2Format;
        public SortEnum SortDirection;
    }

    public struct SortValue
    {
        public string Row1;
        public string Row2;
        public int Row2Num;
        public eValueFormatType Row2Format;
    }

    public struct structSort
    {
        public bool IsSortingByDefault;
        public ArrayList SortInfo;
        public object ValueInfo;

        public structSort(object aSortVal, params SortCriteria[] aSortCrit)
        {
            int i;

            ValueInfo = aSortVal;
            SortInfo = new ArrayList();

            for (i = 0; i < 3; i++)
            {
                if (i < aSortCrit.Length)
                {
                    SortInfo.Add(aSortCrit[i]);
                }
                else
                {
                    SortInfo.Add(null);
                }
            }

            IsSortingByDefault = false;
        }
    }

    /// <summary>
    /// Class that is used to store the RowHeaderTag objects for a grid.
    /// </summary>

    public class GridRowList
    {
        //=======
        // FIELDS
        //=======

        private bool _rowsPerGroupInited;
        private int _rowsPerGroup;
        private ArrayList _gridRowList;

        //=============
        // CONSTRUCTORS
        //=============

        public GridRowList()
        {
            _rowsPerGroupInited = false;
            _rowsPerGroup = 0;
            _gridRowList = new ArrayList();
        }

        public GridRowList(int aRowsPerGroup)
        {
            _rowsPerGroupInited = true;
            _rowsPerGroup = aRowsPerGroup;
            _gridRowList = new ArrayList();
        }

        //===========
        // PROPERTIES
        //===========

        public int Count
        {
            get
            {
                return _gridRowList.Count;
            }
        }

        public int RowsPerGroup
        {
            get
            {
                return _rowsPerGroup;
            }
            set
            {
                _rowsPerGroupInited = true;
                _rowsPerGroup = value;
            }
        }
        // Begin TT#609 - RMatelic - OTS Forecast Chain Ladder View
        public ArrayList GridRowListPublic
        {
            get
            {
                return _gridRowList;
            }
        }
        // End TT#609
        //========
        // METHODS
        //========

        public void Add(RowHeaderTag aRowHeaderTag)
        {
            try
            {
                Add(aRowHeaderTag, false);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void Add(RowHeaderTag aRowHeaderTag, bool aFirstInGroup)
        {
            try
            {
                if (aFirstInGroup && _gridRowList.Count > 0)
                {
                    AddBorderToLastVisibleRow();
                    FillGridToGroupSize();
                }

                _gridRowList.Add(aRowHeaderTag);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void BuildGridRows(C1FlexGrid aGrid, int aFixedRows)
        {
            int i;
            int j;

            try
            {
                AddBorderToLastVisibleRow();
                FillGridToGroupSize();

                aGrid.Rows.Count = _gridRowList.Count;
                aGrid.Rows.Fixed = aFixedRows;

                i = 0;
                j = 0;

                foreach (RowHeaderTag rowHdrTag in _gridRowList)
                {
                    if (rowHdrTag != null)
                    {
                        aGrid.Rows[i].Visible = true;
                        aGrid.Rows[i].UserData = rowHdrTag;
                        aGrid.SetData(i, 0, rowHdrTag.RowHeading);

                        j++;
                    }
                    else
                    {
                        aGrid.Rows[i].Visible = false;
                    }

                    i++;
                }

                ((PagingGridTag)aGrid.Tag).VisibleRowCount = j;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void AddBorderToLastVisibleRow()
        {
            int i;

            try
            {
                for (i = _gridRowList.Count - 1; i > -1; i--)
                {
                    if (_gridRowList[i] != null)
                    {
                        ((RowHeaderTag)_gridRowList[i]).DrawBorder = true;
                        break;
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }


        private void FillGridToGroupSize()
        {
            try
            {
                if (_rowsPerGroupInited)
                {
                    //while (_gridRowList.Count % _rowsPerGroup != 0)
                    //{
                    //    _gridRowList.Add(null);
                    //}
                }
                else
                {
                    _rowsPerGroupInited = true;
                    _rowsPerGroup = _gridRowList.Count;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
    }

    /// <summary>
    /// Class that defines a sort object.
    /// </summary>

    public class GridSortEntry
    {
        //=======
        // FIELDS
        //=======

        private C1.Win.C1FlexGrid.Row _row;
        private IComparable[] _keys;
        private IComparable _defaultKey;
        private int _defaultSequence;

        //=============
        // CONSTRUCTORS
        //=============

        public GridSortEntry(C1.Win.C1FlexGrid.Row aRow, double[] aKeys, IComparable aDefaultKey, int aDefaultSequence)
        {
            _row = aRow;
            _keys = (IComparable[])aKeys.Clone();
            _defaultKey = aDefaultKey;
            _defaultSequence = aDefaultSequence;
        }

        public GridSortEntry(C1.Win.C1FlexGrid.Row aRow, string[] aKeys, IComparable aDefaultKey, int aDefaultSequence)
        {
            _row = aRow;
            _keys = (IComparable[])aKeys.Clone();
            _defaultKey = aDefaultKey;
            _defaultSequence = aDefaultSequence;
        }

        public GridSortEntry(C1.Win.C1FlexGrid.Row aRow, ArrayList aKeys, IComparable aDefaultKey, int aDefaultSequence)
        {
            _row = aRow;
            _keys = (IComparable[])aKeys.ToArray(typeof(IComparable));
            _defaultKey = aDefaultKey;
            _defaultSequence = aDefaultSequence;
        }

        //===========
        // PROPERTIES
        //===========

        public int RowIndex
        {
            get
            {
                return _row.Index;
            }
        }

        public IComparable[] Keys
        {
            get
            {
                return _keys;
            }
        }

        public IComparable DefaultKey
        {
            get
            {
                return _defaultKey;
            }
        }

        public int DefaultSequence
        {
            get
            {
                return _defaultSequence;
            }
        }

        //========
        // METHODS
        //========
    }

    public class SortComparer : IComparer
    {
        private int[] _sortDir;

        //=============
        // CONSTRUCTORS
        //=============

        public SortComparer(ArrayList aSortDirList)
        {
            int i;

            _sortDir = new int[aSortDirList.Count];

            for (i = 0; i < aSortDirList.Count; i++)
            {
                if ((SortEnum)aSortDirList[i] == SortEnum.desc)
                {
                    _sortDir[i] = -1;
                }
                else
                {
                    _sortDir[i] = 1;
                }
            }
        }

        //===========
        // PROPERTIES
        //===========

        //========
        // METHODS
        //========

        public int Compare(object x, object y)
        {
            GridSortEntry xObj;
            GridSortEntry yObj;
            int maxCount;
            int i;
            int compRes;

            try
            {
                xObj = (GridSortEntry)x;
                yObj = (GridSortEntry)y;
                maxCount = xObj.Keys.Length;

                for (i = 0; i < maxCount; i++)
                {
                    compRes = xObj.Keys[i].CompareTo(yObj.Keys[i]);

                    if (compRes != 0)
                    {
                        return compRes * _sortDir[i];
                    }
                }

                compRes = xObj.DefaultKey.CompareTo(yObj.DefaultKey);

                if (compRes != 0)
                {
                    return compRes;
                }
                else
                {
                    return xObj.DefaultSequence.CompareTo(yObj.DefaultSequence);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
    }

}
