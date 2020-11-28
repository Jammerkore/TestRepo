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

namespace Logility.ROWeb
{
    public partial class ROAssortment : ROAllocation 
    {
        //=======
        // FIELDS
        //=======
        private ApplicationSessionTransaction _processTransaction;
        private string reserveAmount = string.Empty;
        private string AnchorNodeText = string.Empty;
        private long _instanceid;
        private string CalendarDateRangeText = string.Empty;
        private string BeginDayCalendarDateRangeText = string.Empty;
        private double _assortReserveAmount;
        private int _headerRID;
        private int _assortStoreGroupRid;
        private int _assortGroupBy;
        private int _assortViewRid;
        private eReserveType _assortReserveType;
        private eAssortmentVariableType _assortVariableType;
        private string _assortHeaderDesc = string.Empty;
        private string _assortHeaderID = string.Empty;
        private string _assortStoreAttributeText = string.Empty;
        private int _assortVariableNumber;
        private bool _assortInclOnhand = false;
        private bool _assortInclIntransit = false;
        private bool _assortInclSimStores = false;
        private bool _assortInclCommitted = false;
        private DateTime _assortLastProcessDt;
        private int _assortUserRid;
        private bool bPercent = false;
        private bool bUnits = false;
        private eStoreAverageBy _assortAverageBy;
        private eGradeBoundary _assortGradeBoundary;
        private int _assortCdrRid;
        private int _assortAnchorNodeRid;
        private int _assortBeginDayCdrRid;
        private DataTable _dtBasis;
        //private AssortmentProfile _updatedAssortmentProfile;
        private List<AssortmentBasis> _basisList = new List<AssortmentBasis>();
        private DataTable _dtStoreGrades;
        private ProfileList _assortmentStoreGradeList;
        private ROAssortmentProperties _rOAssortmentProperties = null;
        List<AssortmentPropertiesBasis> _assortmentPropertiesBasis = null;
        List<AssortmentPropertiesStoreGrades> _assortmentPropertiesStoreGrades = null;

        //private CellTag[][,] _gridData;
        private CubeWaferCoordinateList _commonWaferCoordinateList;
        private AssortmentViewData _assortmentViewData;

        private AssortmentVariables _summaryVariables;
        private AssortmentVariables _detailVariables;
        private AssortmentVariables _totalVariables;
        private AssortmentQuantityVariables _quantityVariables;
        private ProfileList _componentColumnProfileList;
        private ProfileList _totalColumnProfileList;
        private ProfileList _planRowProfileList;
        private ProfileList _detailColumnProfileList;
        private ProfileList _detailRowProfileList;
        private ProfileList _summaryRowProfileList;
        private ProfileList _storeGradeProfileList;
        private int _lastStoreGroupValue;
        private int _lastStoreGroupLevelValue;
        private int _assortmentRid;
        private ProfileList _storeProfileList;
        private ProfileList _workingDetailProfileList;
        private ProfileList _storeGroupListViewProfileList;
        private ProfileList _storeGroupLevelProfileList;
        private StoreGroupLevelProfile _currStoreGroupLevelProfile;
        private ArrayList _selectableStoreGradeHeaders;
        //private long _instanceid;
        private bool _assortmentMemberListbuilt = false;
        //private bool _assortmentPropertiesChanged = false;
        private bool _reprocess = false;
        private FunctionSecurityProfile _assortmentReviewAssortment;
        private FunctionSecurityProfile _userViewSecurity;
        private FunctionSecurityProfile _globalViewSecurity;
        private FunctionSecurityProfile _allocationReviewSummarySecurity;
        private FunctionSecurityProfile _allocationReviewStyleSecurity;
        private FunctionSecurityProfile _allocationReviewSizeSecurity;
        private FunctionSecurityProfile _assortReviewAssortmentSecurity;

        private bool _includeBalance = false;
        //private ArrayList _selectableComponentColumnHeaders;
        private ArrayList _selectableSummaryRowHeaders;
        private ArrayList _selectableTotalColumnHeaders;
        private ArrayList _selectableTotalRowHeaders;   // TT#1224 - stodd - committed
        private ArrayList _selectableDetailColumnHeaders;
        private ArrayList _selectableDetailRowHeaders;
        //private ArrayList _selectableStoreGradeHeaders;

        //private SortedList _sortedComponentColumnHeaders;
        private SortedList _sortedSummaryRowHeaders;
        private SortedList _sortedTotalColumnHeaders;
        private SortedList _sortedDetailColumnHeaders;
        private SortedList _sortedDetailRowHeaders;
        private SortedList _sortedStoreGradeHeaders;

        private eAllocationAssortmentViewGroupBy _columnGroupedBy;

        private bool _currentRedrawState;

        private int _headerCol;
        private int _placeholderCol;
        private int _packCol;
        // BEGIN TT#2150 - stodd - totals not showing in main matrix grid
        private int _highestPlaceholderHeaderCol;
        private bool _containsBothPlaceholderAndHeader;
        private bool _noPlaceholderOrHeaderSelected;
        private int _lastFilterValue;

        private ArrayList _userRIDList;

        private bool _buildDetailsGrid = true;
        private bool _disableMatrix = false;
        private bool _viewDeleted = false;
        private int _lastViewValue;
        private int _lastattributeValue = Include.Undefined;

        private DataTable _assrtViewDetail;
        private MIDReaderWriterLock _pageLoadLock;
        private Hashtable _loadHash;
        private bool _stopPageLoadThread;

        //private AssortmentProfile _asp;

        private string _groupName;
        public string GroupName
        {
            get
            {
                return _groupName;
            }
            set
            {
                _groupName = value;
            }
        }

        public AllocationProfileList AssortmentMemberList
        {
            get
            {
                if (_allocProfileList == null)
                {
                    _allocProfileList = (AllocationProfileList)GetApplicationSessionTransaction().GetMasterProfileList(eProfileType.AssortmentMember);
                }
                return _allocProfileList;
            }
        }

        public AssortmentProfile AssortProfile
        {
            get
            {
                return (AssortmentProfile)_asrtCubeGroup.DefaultAllocationProfile;
            }
        }

        public string ProcessName
        {
            get
            {
                string processName = MIDText.GetTextOnly(eMIDTextCode.lbl_Assortment);
                if (IsGroupAllocation)
                {
                    processName = MIDText.GetTextOnly(eMIDTextCode.lbl_GroupAllocation);
                }
                return processName;
            }
        }

        #region AssortmentReview
        private ROOut GetAssortmentReviewViews()
        {
            DataTable dtView;
            AssortmentViewData assortmentViewData = new AssortmentViewData();
            ArrayList userRIDList = new ArrayList();

            userRIDList.Add(Include.GlobalUserRID);
            userRIDList.Add(SAB.ClientServerSession.UserRID);

            dtView = assortmentViewData.AssortmentView_Read(userRIDList, eAssortmentViewType.Assortment);

            DataView dv = new DataView(dtView);
            dv.Sort = "VIEW_ID";

            List<KeyValuePair<int, string>> viewsList = new List<KeyValuePair<int, string>>();

            foreach (DataRowView rowView in dv)
            {
                int viewRid = Convert.ToInt32(rowView.Row["VIEW_RID"]);
                int userRid = Convert.ToInt32(rowView.Row["USER_RID"]);
                string viewId = Convert.ToString(rowView.Row["VIEW_ID"]);
                if (userRid != Include.GlobalUserRID)
                {
                    viewId = viewId + " (" + UserNameStorage.GetUserName(userRid) + ")";
                }
                viewsList.Add(new KeyValuePair<int, string>(viewRid, viewId));
            }

            return new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, viewsList);

        }
        #endregion

        #region Assortment Properties

        #region Load Assortment Properties Data
        private ROOut GetAssortmentPropertiesData(ROKeyParms rOKeyParams)
        {
            try
            {
                ROAssortmentProperties assortmentProperties = BuildAssortmentPropertiesAndViewSelectionList(rOKeyParams.Key);
                return new ROAssortmentPropertiesOut(eROReturnCode.Successful, null, ROInstanceID, assortmentProperties);
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "GetAssortmentPropertiesData failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }

        internal ROAssortmentProperties BuildAssortmentPropertiesAndViewSelectionList(int asrtID)
        {
            try
            {
                _selectedHeaderKeyList = new ArrayList();
                _selectedAssortmentKeyList = new ArrayList();
                FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentProperties);

                _applicationSessionTransaction = GetApplicationSessionTransaction();

                ROAssortmentProperties rOAssortmentProperties = new ROAssortmentProperties();

                _assortStoreGroupRid = SAB.ClientServerSession.GlobalOptions.AllocationStoreGroupRID;

                if (asrtID > Include.NoRID)
                {
                    _headerRID = asrtID;
                   _assortmentProfile = new AssortmentProfile(_applicationSessionTransaction, string.Empty, asrtID, SAB.ClientServerSession);

                    _assortStoreGroupRid = _assortmentProfile.AssortmentStoreGroupRID;
                    KeyValuePair<int, string> _assortStoreAttributeText = GetName.GetAttributeName(_assortStoreGroupRid);
                    _assortGroupBy = (int)eAllocationAssortmentViewGroupBy.Attribute;
                    _assortViewRid = Include.DefaultAssortmentViewRID;

                    _assortHeaderDesc = _assortmentProfile.HeaderDescription;
                    _assortHeaderID = _assortmentProfile.HeaderID;
                    _assortVariableNumber = _assortmentProfile.AssortmentVariableNumber;
                    _assortInclOnhand = _assortmentProfile.AssortmentIncludeOnhand;
                    _assortInclIntransit = _assortmentProfile.AssortmentIncludeIntransit;
                    _assortInclSimStores = _assortmentProfile.AssortmentIncludeSimilarStores;
                    _assortInclCommitted = false;
                    _assortAverageBy = _assortmentProfile.AssortmentAverageBy;
                    _assortGradeBoundary = _assortmentProfile.AssortmentGradeBoundary;
                    _assortVariableType = _assortmentProfile.AssortmentVariableType;

                    _assortReserveAmount = _assortmentProfile.AssortmentReserveAmount;
                    if (_assortReserveAmount != Include.UndefinedReserve)
                    {
                        reserveAmount = Convert.ToString(_assortReserveAmount, CultureInfo.CurrentUICulture);
                        if (_assortmentProfile.AssortmentReserveType == eReserveType.Percent)
                        {
                            bPercent = true;
                        }
                        else if (_assortmentProfile.AssortmentReserveType == eReserveType.Units)
                        {
                            bUnits = true;
                        }
                    }
                    _assortReserveType = _assortmentProfile.AssortmentReserveType;

                    _assortAnchorNodeRid = _assortmentProfile.AssortmentAnchorNodeRid;
                    if (_assortAnchorNodeRid > 0)
                    {
                        HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_assortAnchorNodeRid, true, true);
                        AnchorNodeText = hnp.Text;
                    }

                    _assortUserRid = _assortmentProfile.AssortmentUserRid;
                    _assortLastProcessDt = _assortmentProfile.AssortmentLastProcessDt;

                    _assortCdrRid = _assortmentProfile.AssortmentCalendarDateRangeRid;
                    if (_assortCdrRid > 0 && _assortCdrRid != Include.UndefinedCalendarDateRange)
                    {
                        DateRangeProfile drpCalendarDateRangeRid = SAB.ClientServerSession.Calendar.GetDateRange(_assortCdrRid);
                        CalendarDateRangeText = drpCalendarDateRangeRid.DisplayDate;
                    }

                    _instanceid = _assortmentProfile.InstanceID;

                    _assortBeginDayCdrRid = _assortmentProfile.AssortmentBeginDayCalendarDateRangeRid;
                    if (_assortBeginDayCdrRid > Include.UndefinedCalendarDateRange)
                    {
                        DateRangeProfile drpBeginDayCalendarDateRangeRid = SAB.ClientServerSession.Calendar.GetDateRange(_assortBeginDayCdrRid);
                        BeginDayCalendarDateRangeText = drpBeginDayCalendarDateRangeRid.DisplayDate;
                    }

                    DataTable dtBasis = BuildBasisGridData(asrtID);
                    DataTable dtStoreGrade = BuildStoreGradesGridData(asrtID);

                    var assortmentPropertiesBasis = (from dtB in dtBasis.AsEnumerable()
                                                     select new AssortmentPropertiesBasis
                                                     {
                                                         HeaderRID = dtB.Field<int>("HDR_RID"),
                                                         BasicSequence = dtB.Field<int>("BASIS_SEQUENCE"),
                                                         HN_RID = dtB.Field<int>("HN_RID"),
                                                         FV_RID = dtB.Field<int>("FV_RID"),
                                                         CDR_RID = dtB.Field<int>("CDR_RID"),
                                                         Weight = dtB.Field<double>("WEIGHT"),
                                                         Merchandise = dtB.Field<string>("Merchandise"),
                                                         Version = dtB.Field<string>("Version"),
                                                         HorizonDateRange = dtB.Field<string>("HorizonDateRange"),

                                                     }).ToList();

                    var assortmentPropertiesStoreGrades = (from dtSG in dtStoreGrade.AsEnumerable()
                                                           select new AssortmentPropertiesStoreGrades
                                                           {
                                                               HeaderRid = dtSG.Field<int>("HDR_RID"),
                                                               StoreGradeSeq = dtSG.Field<int>("STORE_GRADE_SEQ"),
                                                               BoundaryIndex = dtSG.Field<int>("BOUNDARY_INDEX"),
                                                               BoundaryUnits = dtSG.Field<int>("BOUNDARY_UNITS"),
                                                               GradeCode = dtSG.Field<string>("GRADE_CODE"),
                                                           }).ToList();

                    rOAssortmentProperties = new ROAssortmentProperties
                    {
                        HeaderDesc = _assortHeaderDesc,
                        HeaderID = _assortHeaderID,
                        HeaderRid = _headerRID,
                        InstanceId = _instanceid,
                        StoreAttributeRid = _assortStoreGroupRid,
                        StoreAttributeText = _assortStoreAttributeText,
                        GroupBy=_assortGroupBy,
                        ViewRid=_assortViewRid,
                        VariableNumber = _assortVariableNumber,
                        InclOnhand = _assortInclOnhand,
                        InclIntransit = _assortInclIntransit,
                        InclSimStores = _assortInclSimStores,
                        InclCommitted = _assortInclCommitted,
                        PercentReserveType = bPercent,
                        UnitsReserveType = bUnits,
                        AssortReserveType = _assortReserveType,
                        AverageBy = _assortAverageBy,
                        GradeBoundary = _assortGradeBoundary,
                        VariableType = _assortVariableType,
                        ReserveAmount = reserveAmount,
                        AnchorNodeRid = _assortAnchorNodeRid,
                        AnchorNodeText = AnchorNodeText,
                        CalendarDateRangeRid = _assortCdrRid,
                        CalendarDateRangeText = CalendarDateRangeText,
                        BeginDayCalendarDateRangeRid = _assortBeginDayCdrRid,
                        BeginDayCalendarDateRangeText = BeginDayCalendarDateRangeText,
                        AssortLastProcessDt = _assortLastProcessDt,
                        AssortUserRid = _assortUserRid,
                        AssortmentPropertiesBasis = assortmentPropertiesBasis,
                        AssortmentPropertiesStoreGrades = assortmentPropertiesStoreGrades,
                    };

                    //_assortmentProfile = assortmentProfile;
                    AddSelectedHeadersToTrans(aTrans: _applicationSessionTransaction);

                    //just check if this adds any value to fetch grid data
                    //AddSelectedHeadersToTrans(aTrans: _applicationSessionTransaction);
                    _assortmentProfile = _applicationSessionTransaction.GetAssortmentProfileFromList();
                    _applicationSessionTransaction.AssortmentProfile = _assortmentProfile;
                }
                else
                {
                    _assortmentProfile = new AssortmentProfile(_applicationSessionTransaction, string.Empty, Include.NoRID, SAB.ApplicationServerSession);
                    rOAssortmentProperties = new ROAssortmentProperties { StoreAttributeRid = _assortStoreGroupRid };
                }


                return rOAssortmentProperties;
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "BuildAssortmentPropertiesList failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }

        #region  Store Grades Grid Data
        internal DataTable BuildStoreGradesGridData(int asrtRid)
        {
            try
            {
                FillStoreGradeList(asrtRid);

                _dtStoreGrades = MIDEnvironment.CreateDataTable("Store Grades");

                _dtStoreGrades.Columns.Add("HDR_RID", System.Type.GetType("System.Int32"));
                _dtStoreGrades.Columns.Add("STORE_GRADE_SEQ", System.Type.GetType("System.Int32"));
                _dtStoreGrades.Columns.Add("BOUNDARY_INDEX", System.Type.GetType("System.Int32"));
                _dtStoreGrades.Columns.Add("BOUNDARY_UNITS", System.Type.GetType("System.Int32"));
                _dtStoreGrades.Columns.Add("GRADE_CODE", System.Type.GetType("System.String"));

                int seq = 0;
                foreach (StoreGradeProfile sgp in _assortmentStoreGradeList.ArrayList)
                {
                    DataRow aRow = _dtStoreGrades.NewRow();
                    if (_assortmentProfile == null)
                    {
                        aRow["HDR_RID"] = Include.NoRID;
                    }
                    else
                    {
                        aRow["HDR_RID"] = _assortmentProfile.Key;
                    }
                    aRow["STORE_GRADE_SEQ"] = seq++;
                    aRow["BOUNDARY_INDEX"] = sgp.Boundary;
                    aRow["BOUNDARY_UNITS"] = sgp.BoundaryUnits;
                    aRow["GRADE_CODE"] = sgp.StoreGrade;

                    _dtStoreGrades.Rows.Add(aRow);
                }

                _dtStoreGrades.AcceptChanges();

                return _dtStoreGrades;
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "BuildStoreGradesGridData failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }

        internal void FillStoreGradeList(int asrtRid)
        {
            try
            {
                DataTable dtAssortGrades = HeaderDataRecord.GetAssortmentPropertiesStoreGrades(asrtRid);

                _assortmentStoreGradeList = new ProfileList(eProfileType.StoreGrade);
                foreach (DataRow aRow in dtAssortGrades.Rows)
                {
                    int seq = Convert.ToInt32(aRow["STORE_GRADE_SEQ"], CultureInfo.CurrentUICulture);
                    int boundary = Convert.ToInt32(aRow["BOUNDARY_INDEX"], CultureInfo.CurrentUICulture);
                    int boundaryUnits = Convert.ToInt32(aRow["BOUNDARY_UNITS"], CultureInfo.CurrentUICulture);
                    string gradeCode = aRow["GRADE_CODE"].ToString().Trim();
                    StoreGradeProfile sgp = new StoreGradeProfile(boundary);
                    sgp.Boundary = boundary;
                    sgp.StoreGrade = gradeCode;
                    sgp.BoundaryUnits = boundaryUnits;
                    _assortmentStoreGradeList.Add(sgp);
                }
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "FillStoreGradeList failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }
        #endregion

        #region  Basis Grid Data
        internal DataTable BuildBasisGridData(int asrtRID)
        {
            try
            {
                _basisList = new List<AssortmentBasis>();

                FillAssortBasis(asrtRID);

                _dtBasis = MIDEnvironment.CreateDataTable("Assortment Basis");

                _dtBasis.Columns.Add("HDR_RID", System.Type.GetType("System.Int32"));
                _dtBasis.Columns.Add("BASIS_SEQUENCE", System.Type.GetType("System.Int32"));
                _dtBasis.Columns.Add("HN_RID", System.Type.GetType("System.Int32"));
                _dtBasis.Columns.Add("FV_RID", System.Type.GetType("System.Int32"));
                _dtBasis.Columns.Add("CDR_RID", System.Type.GetType("System.Int32"));
                _dtBasis.Columns.Add("WEIGHT", System.Type.GetType("System.Double"));
                _dtBasis.Columns.Add("Merchandise", System.Type.GetType("System.String"));
                _dtBasis.Columns.Add("Version", System.Type.GetType("System.String"));
                _dtBasis.Columns.Add("HorizonDateRange", System.Type.GetType("System.String"));

                int seq = 0;
                // Loads in display versions of the RIDs
                foreach (AssortmentBasis ab in _basisList)
                {
                    DataRow aRow = _dtBasis.NewRow();
                    if (_assortmentProfile == null)
                    {
                        aRow["HDR_RID"] = Include.NoRID;
                    }
                    else
                    {
                        aRow["HDR_RID"] = _assortmentProfile.Key;
                    }
                    aRow["BASIS_SEQUENCE"] = seq++;
                    aRow["HN_RID"] = ab.HierarchyNodeProfile.Key;
                    aRow["FV_RID"] = ab.VersionProfile.Key;
                    aRow["CDR_RID"] = ab.HorizonDate.Key;
                    aRow["WEIGHT"] = ab.Weight;
                    aRow["Merchandise"] = ab.HierarchyNodeProfile.Text;
                    aRow["Version"] = ab.VersionProfile.Description;
                    aRow["HorizonDateRange"] = ab.HorizonDate.DisplayDate;
                    _dtBasis.Rows.Add(aRow);
                }

                _dtBasis.AcceptChanges();

                return _dtBasis;
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "BuildBasisGridData failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }

        internal void FillAssortBasis(int assortmentID)
        {
            try
            {
                //==========================
                // Header Assortment Basis
                //==========================
                DataTable dtAssortBasis = HeaderDataRecord.GetAssortmentPropertiesBasis(assortmentID);

                ApplicationSessionTransaction aTrans = SAB.ApplicationServerSession.CreateTransaction();

                foreach (DataRow aRow in dtAssortBasis.Rows)
                {
                    int hierNodeRid = Convert.ToInt32(aRow["HN_RID"], CultureInfo.CurrentUICulture);
                    int versionRid = Convert.ToInt32(aRow["FV_RID"], CultureInfo.CurrentUICulture);
                    dateRangeRid = Convert.ToInt32(aRow["CDR_RID"], CultureInfo.CurrentUICulture);
                    float weight = (float)Convert.ToDouble(aRow["WEIGHT"], CultureInfo.CurrentUICulture);

                    AssortmentBasis ab = new AssortmentBasis(SAB, aTrans, hierNodeRid, versionRid, dateRangeRid, weight, AssortmentApplyToDate);
                    _basisList.Add(ab);
                }
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "FillAssortBasis failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }
        #endregion

        #endregion

        #region Create/Modify Assortment Properties
        private ROOut UpdateAssortmentPropertiesData(ROAssortmentPropertiesParms rOAssortmentPropertiesParms)
        {
            ROAssortmentProperties rOAssortmentProperties = new ROAssortmentProperties();
            try
            {
                rOAssortmentProperties = rOAssortmentPropertiesParms.ROAssortmentProperties;

                if (rOAssortmentProperties.HeaderRid == Include.NoRID)
                {
                    _isAsrtPropertyChanged = true;
					_assortmentMemberListbuilt = false;
                }

                if (_assortmentProfile == null)
                {
                    _assortmentProfile = new AssortmentProfile(GetApplicationSessionTransaction(), string.Empty, rOAssortmentProperties.HeaderRid, SAB.ClientServerSession);
                }

                SetSpecificFields(rOAssortmentProperties);

                SetBaseValues(rOAssortmentProperties);

                bool success = SaveAssortmentPropertiesData();

                // set flag to recalculate summary
                _isAsrtPropertyChanged = true;

                return new ROIntOut(eROReturnCode.Successful, null, ROInstanceID, _assortmentProfile.Key);
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "UpdateAssortmentPropertiesData failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }

        internal ApplicationSessionTransaction GetApplicationSessionTransaction()
        {
            if (_applicationSessionTransaction != null)
            {
                return _applicationSessionTransaction;
            }
            else
            {
                return SAB.ApplicationServerSession.CreateTransaction();
            }
        }

        private bool SaveAssortmentPropertiesData()
        {
            try
            {
                if (!_assortmentProfile.HeaderDataRecord.ConnectionIsOpen)
                {
                    _assortmentProfile.HeaderDataRecord.OpenUpdateConnection();
                }

                if (_assortmentProfile.HeaderRID == Include.NoRID)
                {
                    _assortmentProfile.WriteHeader();
                    if (!_assortmentProfile.HeaderDataRecord.ConnectionIsOpen)
                    {
                        _assortmentProfile.HeaderDataRecord.OpenUpdateConnection();
                    }
                    AddAssortmentToFolder(_assortmentProfile.HeaderRID);
                }

                _assortmentProfile.WriteAssortment();

                _assortmentProfile.HeaderDataRecord.CommitData();

                return true;
            }
            catch (Exception exc)
            {
                throw;
            }
            finally
            {
                _assortmentProfile.HeaderDataRecord.CloseUpdateConnection();
                _assortmentProfile.AppSessionTransaction.RemoveAllocationProfile(_assortmentProfile);
            }
        }

        private void AddAssortmentToFolder(int assortmentKey)
        {
            FolderDataLayer dlFolder = new FolderDataLayer();
            DataTable dtFolders = dlFolder.Folder_Read(Include.GlobalUserRID, eProfileType.AssortmentMainFolder);
            if (dtFolders.Rows.Count > 0)
            {
                int mainAssortmentFolderKey = Convert.ToInt32(dtFolders.Rows[0]["FOLDER_RID"]);

                dlFolder.OpenUpdateConnection();

                try
                {

                    dlFolder.Folder_Item_Insert(mainAssortmentFolderKey, assortmentKey, eProfileType.AssortmentHeader);

                    dlFolder.CommitData();
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    dlFolder.CloseUpdateConnection();
                }
            }
        }

        private void SetBaseValues(ROAssortmentProperties rOAssortmentProperties)
        {
            try
            {
                DateRangeProfile drpCalendarDateRangeRid = SAB.ClientServerSession.Calendar.GetDateRange(rOAssortmentProperties.CalendarDateRangeRid);
                DayProfile anchoreDay = SAB.ApplicationServerSession.Calendar.GetFirstDayOfRange(drpCalendarDateRangeRid);

                _headerRID = rOAssortmentProperties.HeaderRid;
                // Store Attribute
                if (rOAssortmentProperties.StoreAttributeRid > -1)
                {
                    _assortStoreGroupRid = Convert.ToInt32(rOAssortmentProperties.StoreAttributeRid, CultureInfo.CurrentUICulture);
                }
                else
                {
                    _assortStoreGroupRid = Include.NoRID;
                }


                // Average By
                _assortAverageBy = rOAssortmentProperties.AverageBy;

                // Variable
                _assortVariableType = rOAssortmentProperties.VariableType;

                _assortInclOnhand = rOAssortmentProperties.InclOnhand;
                _assortInclIntransit = rOAssortmentProperties.InclIntransit;
                _assortInclSimStores = rOAssortmentProperties.InclSimStores;
                _assortInclCommitted = rOAssortmentProperties.InclCommitted;

                // Grade Boundary
                _assortGradeBoundary = rOAssortmentProperties.GradeBoundary;

                List<int> hierNodeList = new List<int>();
                List<int> versionList = new List<int>();
                List<int> dateRangeList = new List<int>();
                List<double> weightList = new List<double>();
                _basisList.Clear();
                bool filled = false;
                foreach (var item in rOAssortmentProperties.AssortmentPropertiesBasis)
                {
                    int hnRid = Convert.ToInt32(item.HN_RID);
                    int verRid = Convert.ToInt32(item.FV_RID);
                    int cdrRid = Convert.ToInt32(item.CDR_RID);
                    float weight = (float)Convert.ToDouble(item.Weight);

                    AssortmentBasis ab = new AssortmentBasis(SAB, _applicationSessionTransaction, hnRid, verRid, cdrRid, weight, anchoreDay);
                    _basisList.Add(ab);

                    // Used in Variable number read below. Only need first basis value.
                    if (!filled)
                    {
                        hierNodeList.Add(hnRid);
                        versionList.Add(verRid);
                        dateRangeList.Add(cdrRid);
                        weightList.Add(weight);
                        filled = true;
                    }
                }

                if (_applicationSessionTransaction == null)
                {
                    _applicationSessionTransaction = _assortmentProfile.AppSessionTransaction;
                }

                if (rOAssortmentProperties.AnchorNodeRid < 1)
                {
                    _assortAnchorNodeRid = (int)hierNodeList[0];
                }

                AllocationProfileList alp = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.Allocation);
                if (_assortmentProfile == null)
                {

                    for (int i = 0; i < alp.ArrayList.Count; i++)
                    {
                        if ((alp.ArrayList[i] is AssortmentProfile && !((AllocationProfile)alp.ArrayList[i]).isAssortmentProfile) || (!(alp.ArrayList[i] is AssortmentProfile) && ((AllocationProfile)alp.ArrayList[i]).isAssortmentProfile))
                        {
                            throw new Exception("Object does not match AssortmentProfile in SetBaseValues()");
                        }

                        if (((AllocationProfile)alp.ArrayList[i]).isAssortmentProfile)
                        {
                            _assortmentProfile = (AssortmentProfile)alp.ArrayList[i];
                            break;
                        }
                    }
                }

                _assortmentProfile.AssortmentIncludeSimilarStores = _assortInclSimStores;
                _assortmentProfile.AssortmentIncludeOnhand = _assortInclOnhand;
                _assortmentProfile.AssortmentIncludeIntransit = _assortInclIntransit;

                _assortmentProfile.AssortmentBasisList = _basisList;

                _assortVariableNumber = _assortmentProfile.BasisReader.GetVariableNumber(_assortVariableType);


                _assortmentStoreGradeList = new ProfileList(eProfileType.StoreGrade);
                _assortmentStoreGradeList.Clear();
                foreach (var items in rOAssortmentProperties.AssortmentPropertiesStoreGrades.OrderByDescending(x => x.BoundaryIndex))
                {
                    int boundary = 0;
                    if (items.BoundaryIndex > -1)
                    {
                        boundary = Convert.ToInt32(items.BoundaryIndex, CultureInfo.CurrentUICulture);
                    }

                    int boundaryUnits = 0;
                    if (items.BoundaryUnits > -1)
                    {
                        boundaryUnits = Convert.ToInt32(items.BoundaryUnits, CultureInfo.CurrentUICulture);
                    }

                    string gradeCode = items.GradeCode.ToString().Trim();
                    StoreGradeProfile sgp = new StoreGradeProfile(boundary);
                    sgp.Boundary = boundary;
                    sgp.StoreGrade = gradeCode;
                    sgp.BoundaryUnits = boundaryUnits;
                    _assortmentStoreGradeList.Add(sgp);
                }

                _assortmentProfile.Key = _headerRID;
                _assortmentProfile.AssortmentStoreGroupRID = _assortStoreGroupRid;
                _assortmentProfile.AssortmentVariableType = _assortVariableType;
                _assortmentProfile.AssortmentVariableNumber = _assortVariableNumber;
                _assortmentProfile.AssortmentIncludeOnhand = _assortInclOnhand;
                _assortmentProfile.AssortmentIncludeIntransit = _assortInclIntransit;
                _assortmentProfile.AssortmentIncludeSimilarStores = _assortInclSimStores;
                _assortmentProfile.AssortmentIncludeCommitted = _assortInclCommitted;
                _assortmentProfile.AssortmentAverageBy = _assortAverageBy;
                _assortmentProfile.AssortmentGradeBoundary = _assortGradeBoundary;
                _assortmentProfile.AssortmentBasisList = _basisList;
                _assortmentProfile.AssortmentStoreGradeList = _assortmentStoreGradeList;

            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "SetBaseValues failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }

        private void SetSpecificFields(ROAssortmentProperties rOAssortmentProperties)
        {
            try
            {

                _assortmentProfile.SaveAssortmentMembers = false;
                //_assortmentProfile.HeaderID = rOAssortmentProperties.HeaderRid.ToString();
                _assortmentProfile.HeaderID = rOAssortmentProperties.HeaderID;
                _assortmentProfile.HeaderDescription = rOAssortmentProperties.HeaderDesc;

                HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(rOAssortmentProperties.AnchorNodeRid, true, true);

                if (_assortmentProfile.AssortmentAnchorNodeRid != hnp.Key)
                {
                    _assortmentProfile.AssortmentAnchorNodeRid = hnp.Key;
                    _assortmentProfile.PlanHnRID = Include.DefaultPlanHnRID;
                    _assortmentProfile.SaveAssortmentMembers = true;
                }
                else
                {
                    _assortmentProfile.AssortmentAnchorNodeRid = hnp.Key;
                }

                if (_assortmentProfile.AssortmentCalendarDateRangeRid != rOAssortmentProperties.CalendarDateRangeRid)
                {
                    _assortmentProfile.AssortmentCalendarDateRangeRid = rOAssortmentProperties.CalendarDateRangeRid;
                    if (_assortmentProfile.Key != Include.NoRID)
                    {
                        _assortmentProfile.ResetShipDates();
                    }
                    _assortmentProfile.AssortmentApplyToDate = null;
                    _assortmentProfile.SaveAssortmentMembers = true;
                }

                if (rOAssortmentProperties.BeginDayCalendarDateRangeRid >= Include.UndefinedCalendarDateRange)
                {
                    if (_assortmentProfile.AssortmentBeginDayCalendarDateRangeRid != rOAssortmentProperties.BeginDayCalendarDateRangeRid)
                    {
                        _assortmentProfile.AssortmentBeginDayCalendarDateRangeRid = rOAssortmentProperties.BeginDayCalendarDateRangeRid;
                        _assortmentProfile.BeginDay = Include.UndefinedDate;
                        _assortmentProfile.AssortmentBeginDay = null;
                        _assortmentProfile.SaveAssortmentMembers = true;
                    }
                }

                // Reserve amount
                if (rOAssortmentProperties.ReserveAmount != null && rOAssortmentProperties.ReserveAmount.Trim() != string.Empty)
                    _assortmentProfile.AssortmentReserveAmount = Convert.ToDouble(rOAssortmentProperties.ReserveAmount, CultureInfo.CurrentUICulture);
                else
                    _assortmentProfile.AssortmentReserveAmount = Include.UndefinedReserve;

                //Reserve Ind
                _assortmentProfile.AssortmentReserveType = eReserveType.Unknown;
                if (rOAssortmentProperties.PercentReserveType)
                    _assortmentProfile.AssortmentReserveType = eReserveType.Percent;
                else if (rOAssortmentProperties.UnitsReserveType)
                    _assortmentProfile.AssortmentReserveType = eReserveType.Units;


                _assortmentProfile.AssortmentUserRid = SAB.ClientServerSession.UserRID;
                _assortmentProfile.AssortmentLastProcessDt = DateTime.Now;

                if (_assortmentProfile.Key < 0)
                {
                    _assortmentProfile.StyleHnRID = hnp.Key;
                }
                _assortmentProfile.AsrtAnchorNodeRid = hnp.Key;

                MIDException midException;
                if (!_assortmentProfile.SetHeaderType(eHeaderType.Assortment, out midException))
                {
                    throw midException;
                }
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "SetSpecificFields failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }
        #endregion


        #endregion

        #region Assortment Review Selection
        private ROOut GetAssortmentReviewSelectionData(ROKeyParms rOKeyParams)
        {
            try
            {
                ROAssortmentProperties assortmentSelections = BuildAssortmentPropertiesAndViewSelectionList(rOKeyParams.Key);
                return new ROAssortmentPropertiesOut(eROReturnCode.Successful, null, ROInstanceID, assortmentSelections);
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "GetAssortmentReviewSelectionData failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }

        #endregion

        #region "Get Selected Header list"

        private ROOut SetAssortmentSelectedHeaders(ROListParms selectedHeaders)
        {
            string message = string.Empty;
            ArrayList _selectedHeaderKeyList = new ArrayList();
            GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);
            ArrayList selectedAssortmentList = new ArrayList(); //to pass an empty seleted assortement list 
            try
            {
                var selectedHeaderList4mSession = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();

                if (selectedHeaderList4mSession.ArrayList.Count > 0)
                {
                    SAB.ClientServerSession.ClearSelectedHeaderList();
                    SAB.ClientServerSession.ClearSelectedComponentList();
                }

                _applicationSessionTransaction = GetApplicationSessionTransaction();

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

        #region "Method to set the AssortmentProperties to AssortmentProfile"
        private ROOut UpdateAssortmentSelection(ROAssortmentPropertiesParms rOAssortmentPropertiesParms)
        {
            bool isPropertiesAssigned = false;
            try
            {
                isPropertiesAssigned = UpdateToAssortmentProfile(rOAssortmentPropertiesParms.ROAssortmentProperties);
            }
            catch (Exception ex)
            {
                isPropertiesAssigned = false;
            }

            return new ROBoolOut(eROReturnCode.Successful, null, ROInstanceID, isPropertiesAssigned);
        }

        #endregion

        #region "Internal method to Set Values to AssortmentProfile" 
        internal bool UpdateToAssortmentProfile(ROAssortmentProperties rOAssortmentProperties)
        {
            bool returnValue = false;
            try
            {

                if (_assortmentProfile == null)
                {
                    _assortmentProfile = new AssortmentProfile(_applicationSessionTransaction, string.Empty, rOAssortmentProperties.HeaderRid, SAB.ClientServerSession);
                }

                _applicationSessionTransaction.CreateAssortmentViewSelectionCriteria();

                var basisData = ApplicationUtilities.ToDataTable(rOAssortmentProperties.AssortmentPropertiesBasis);
                var storeGradeData = ApplicationUtilities.ToDataTable(rOAssortmentProperties.AssortmentPropertiesStoreGrades);

                if (rOAssortmentProperties.viewType == eAssortViewType.Selection)
                {
                    _applicationSessionTransaction.AssortmentStoreAttributeRid = rOAssortmentProperties.StoreAttributeRid;
                    _applicationSessionTransaction.AssortmentGroupBy = rOAssortmentProperties.GroupBy;
                    _applicationSessionTransaction.AssortmentViewRid = rOAssortmentProperties.ViewRid;
                    _applicationSessionTransaction.AssortmentVariableType = rOAssortmentProperties.VariableType;
                    _applicationSessionTransaction.AssortmentVariableNumber = rOAssortmentProperties.VariableNumber;
                    _applicationSessionTransaction.AssortmentIncludeOnhand = rOAssortmentProperties.InclOnhand;
                    _applicationSessionTransaction.AssortmentIncludeIntransit = rOAssortmentProperties.InclIntransit;
                    _applicationSessionTransaction.AssortmentIncludeSimStore = rOAssortmentProperties.InclSimStores;
                    _applicationSessionTransaction.AssortmentIncludeCommitted = rOAssortmentProperties.InclCommitted;
                    _applicationSessionTransaction.AssortmentAverageBy = rOAssortmentProperties.AverageBy;
                    _applicationSessionTransaction.AssortmentGradeBoundary = rOAssortmentProperties.GradeBoundary;
                    _applicationSessionTransaction.AssortmentBasisDataTable = basisData;
                    _applicationSessionTransaction.AssortmentStoreGradeDataTable = storeGradeData;
                }
                else if (rOAssortmentProperties.viewType == eAssortViewType.Properties)
                {
                    //_assortmentProfile = new AssortmentProfile(GetApplicationSessionTransaction(), string.Empty, rOAssortmentProperties.HeaderRid, SAB.ClientServerSession);

                    AssortmentSelection assortmentSelection = new AssortmentSelection
                    {
                        UserRid = rOAssortmentProperties.AssortUserRid,
                        StoreAttributeRid = rOAssortmentProperties.StoreAttributeRid,
                        GroupBy=rOAssortmentProperties.GroupBy,
                        ViewRid=rOAssortmentProperties.ViewRid,
                        VariableType = rOAssortmentProperties.VariableType,
                        VariableNumber = rOAssortmentProperties.VariableNumber,
                        IncludeOnhand = rOAssortmentProperties.InclOnhand,
                        IncludeIntransit = rOAssortmentProperties.InclIntransit,
                        IncludeSimStore = rOAssortmentProperties.InclSimStores,
                        IncludeCommitted = rOAssortmentProperties.InclCommitted,
                        AverageBy = rOAssortmentProperties.AverageBy,
                        GradeBoundary = rOAssortmentProperties.GradeBoundary,
                        BasisDataTable = basisData,
                        StoreGradeDataTable = storeGradeData

                    };
                }

                SetSpecificFields(rOAssortmentProperties);

                SetBaseValues(rOAssortmentProperties);

                bool success = SaveAssortmentPropertiesData();

                returnValue = true;
            }
            catch (Exception ex)
            {

                returnValue = false;
            }

            return returnValue;
        }

        #endregion

        private AllocationHeaderProfile GetHeader(int aHeaderRID)
        {
            foreach (AllocationHeaderProfile ho in _asrtCubeGroup.GetHeaderList().ArrayList)
            {
                if (ho.Key == aHeaderRID)
                {
                    return ho;
                }
            }

            return null;
        }

        private eHeaderType GetHeaderType(int aHeaderRID)
        {
            AllocationHeaderProfile ho = GetHeader(aHeaderRID);
            if (ho != null)
            {
                return ho.HeaderType;
            }

            return eHeaderType.Receipt;
        }
    }
}
