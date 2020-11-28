using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using System.Threading;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinMaskedEdit;
using Infragistics.Documents.Excel;
using Infragistics.Win.UltraWinGrid.ExcelExport;

using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
    public partial class AssortmentWorkspaceExplorer : MIDRetail.Windows.ExplorerBase
    {
        #region Variable Declarations

        private BindingSource _bindSourceHeader;

        private DataTable _dtHeader;
        private DataTable _dtView;
        private DataTable _anchorNodes;
        private DataTable _assortments;
        private DataTable _placeHolders;
       
        //private AllocationHeaderProfileList _headerList;  // TT#488 - MD - Jellis - Group Allocation
        private List<int> _headerRidEnqueueList;            // TT#488 - MD - Jellis - Group Allocation
        //private AssortmentWorkspaceFilterProfile _assrtWorkFilterProfile; //TT#1313-MD -jsobek -Header Filters -do not re-filter in the AWS
        private ApplicationSessionTransaction _trans;
        private frmUltraGridSearchReplace _frmUltraGridSearchReplace;

        //private AssortmentWorkspaceFilterData _assrtWorkFilterData; //TT#1313-MD -jsobek -Header Filters
        private GetMethods _getMethods;
        private GridViewData _gridViewData;
        //private HeaderCharGroupProfileList _headerCharGroupProfileList; //TT#440 - MD - RBeck _ Make Header Characteristics references by RID
		// BEGIN Stodd - 4.0 to 4.1 Manual merge
		private HeaderEnqueue _headerEnqueue = null;
		// END Stodd - 4.0 to 4.1 Manual merge
        private HierarchyLevelProfile _hlpStyle;
        private HierarchyLevelProfile _hlpProduct;
        private HierarchyMaintenance _hierMaint;
        private HierarchyNodeProfile _nodeDataHashLastValue;
        private HierarchyProfile _mainHp;
   
        private ExplorerAddressBlock _EAB;
        private SessionAddressBlock _SAB;
        private UserGridView _userGridView;
        private FilterData _filterData; //TT#1313-MD -jsobek -Header Filters
        private int _headerFilterRID = -1; //TT#1313-MD -jsobek -Header Filters

        private FunctionSecurityProfile _assortmentWorkspaceSecurity;
        //private FunctionSecurityProfile _assortmentWorkspaceViewsSecurity;  // TT#2014-MD - JSmith - Assortment Security
        //private FunctionSecurityProfile _assortmentWorkspaceViewsGlobalSecurity;
        //private FunctionSecurityProfile _assortmentWorkspaceViewsUserSecurity;
        // Begin TT#2014-MD - JSmith - Assortment Security
        private FunctionSecurityProfile _assortmentWorkspaceViewsGlobalSecurity;
        private FunctionSecurityProfile _assortmentWorkspaceViewsUserSecurity;
        // End TT#2014-MD - JSmith - Assortment Security

        private Image _dynamicToPlanImage = null;
        private Image _dynamicToCurrentImage = null;

        private UltraGridColumn _gridCol;
        private UltraGridBand _gridBand;
        private UltraGridRow _rClickRow = null;
      
        private ValueList _assortmentValueList;
        private ValueList _headerIntransitValueList;
        private ValueList _headerShipStatusValueList;
        private ValueList _headerStatusValueList;
        private ValueList _headerTypeValueList;
        private ValueList _packTypeValueList;
        private ValueList _placeHolderValueList;
        private ValueList _sizeGroupValueList;

        private ArrayList _hdrsInGroups = new ArrayList();
        private ArrayList _headerProfileArrayList;
        private ArrayList _headersInGroupBy;
        private ArrayList _masterKeyList;
        private ArrayList _removedAsrtHeaders = new ArrayList();
        private ArrayList _selectedHeaderKeyList;
        private ArrayList _selectedAssortmentKeyList; // TT#488 - MD - Jellis - Group Allocation
        private ArrayList _selectedRowsSequence = new ArrayList();
        private ArrayList _userRIDList;

        private Hashtable _addedColorSizeHash = new Hashtable();
        private Hashtable _assortmentGroups = new Hashtable();
        private Hashtable _charByGroupAndID = new Hashtable();
        //private Hashtable _charValueListsHash;   //TT#440 - MD - RBeck _ Make Header Characteristics references by RID
        private Hashtable _colorsForStyle = new Hashtable();
        private Hashtable _deletedAssortmentStyles = new Hashtable();
        private Hashtable _deletedHeaderRows = new Hashtable();
        private Hashtable _deletedPlaceholderStyles = new Hashtable();
        //private Hashtable _methodHash = new Hashtable(); //TT#1313-MD -jsobek -Header Filters -performance
        private Hashtable _multiHeaderColor = new Hashtable();
        private Hashtable _multiHeaderColorIds = new Hashtable();   // MID Track #6127, (#6164 - change from ArrayList to Hashtable)
        private Hashtable _multiHeaderGroups = new Hashtable();
        //private Hashtable _multiGroupsIncludeAfterDateFilter = new Hashtable();      // MID Track #6239 - Multi should not be split up //TT#1313-MD -jsobek -Header Filters -do not re-filter in the AWS
        //private Hashtable _multiGroupsIncludeAfterStyleFilter = new Hashtable();     // MID Track #6239 - Multi should not be split up //TT#1313-MD -jsobek -Header Filters -do not re-filter in the AWS
        private Hashtable _nodeDataHash = new Hashtable();
        private Hashtable _sizeGroupHash = new Hashtable();
        private Hashtable _workflowNameHash = new Hashtable();

        private bool _allowDeleteKey = false;
        private bool _bindingView;
        private bool _deleteKeyPressed = false;
        private bool _excludedHeadersRemoved = false;  
        //private bool _fromCellButton = false;
        private bool _fromFilterWindow = false;
        private bool _fromLoadEvent;
        //private bool _fromRightClick = false;
        //private bool _headerAdded = false;
        private bool _inEditMode = false;
        //private bool _skipAnchorEdit = false;
        //private bool _skipBeforeCellUpdate = false;
        //private bool _skipEdit = false;
        //private bool _skipHeaderEdit = false;
        private bool _skipInitialize = false;
        //private bool _skipProductEdit = false;
        //private bool _skipRowUpdate = false;
        //private bool _skipStyleEdit = false;
        private bool _viewSaved = false;         // MID Track #6407

        private string _graphicsDir;
        private string _lblAssortment;
        private string _lblColumnChooser;
        private string _lblDeleteRow;
        private string _lblHierarchyNode;
        private string _lblHeaderNotes;
        private string _lblPhStyle;
        private string _lblPlaceholder;
        private string _lblSelectAction;
        private string _lblSelected;
        private string _lblTotal;
        private string _lblQuantity;
        private string _lblUser;
        private string _noSizeDimensionLbl;
        private string _thisTitle;

        private int _lastAsrtSortSeq;
        private int _lastSelectedViewRID;
        private int _maxBandDepth = 1;
        private int _nodeDataHashLastKey = 0;
        private int _nonCharColCount = 0;
        //private int _sizeGroupHashLastKey = 0;
		private bool _checkForExportSelected = false;
		private UltraGridExcelExporter _ultraGridExcelExporter1 = null;

        #endregion

        #region Constructor
        public AssortmentWorkspaceExplorer(SessionAddressBlock aSAB, ExplorerAddressBlock aEAB, Form aMainMDIForm)
			: base(aSAB, aEAB, aMainMDIForm)
        {
			aEAB.AssortmentWorkspaceExplorer = this;

			this.AllowDrop = true;
            _SAB = aSAB;
            _EAB = aEAB;
            _mainHp = _SAB.HierarchyServerSession.GetMainHierarchyData();
            _trans = _SAB.ApplicationServerSession.CreateTransaction();
            //_headerList = new AllocationHeaderProfileList(eProfileType.AllocationHeader); // TT#488 - MD - Jellis - Group Allocation
            _headerRidEnqueueList = new List<int>();               // TT#488 - MD - Jellis - Group Allocation

            _getMethods = new GetMethods(_SAB);
            _gridViewData = new GridViewData();
            _userGridView = new UserGridView();
            _filterData = new FilterData();
            //_assrtWorkFilterData = new AssortmentWorkspaceFilterData(); //TT#1313-MD -jsobek -Header Filters

            // get hierarchy profiles for style and product (parent of style)
            _hlpStyle = null;
            _hlpProduct = null;
            for (int level = 1; level <= _mainHp.HierarchyLevels.Count; level++)
            {
                _hlpProduct = _hlpStyle;
                _hlpStyle = (HierarchyLevelProfile)_mainHp.HierarchyLevels[level];
                if (_hlpStyle.LevelType == eHierarchyLevelType.Style)
                {
                    break;
                }
            }
            if (_hlpStyle == null)
            {
                _hlpStyle = new HierarchyLevelProfile(0);
                _hlpStyle.LevelID = "Style";
            }
            if (_hlpProduct == null)
            {
                _hlpProduct = new HierarchyLevelProfile(1);
                _hlpProduct.LevelID = "Product";
            }

            // This call is required by the Windows Form Designer.
            InitializeComponent();

            SetText();
            // Begin TT#588-MD - JSmith - Remove ApplicationRoot from configuration
            //_graphicsDir = MIDConfigurationManager.AppSettings["ApplicationRoot"] + MIDGraphics.GraphicsDir;
            _graphicsDir = MIDGraphics.MIDGraphicsDir;
            // End TT#588-MD - JSmith - Remove ApplicationRoot from configuration
            string dynamicFileName = _graphicsDir + "\\" + MIDGraphics.DynamicToPlanImage;
            try
            {
                _dynamicToPlanImage = Image.FromFile(dynamicFileName);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message);
            }

            dynamicFileName = _graphicsDir + "\\" + MIDGraphics.DynamicToCurrentImage;
            try
            {
                _dynamicToCurrentImage = Image.FromFile(dynamicFileName);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message);
            }
        }

        
        #endregion

        #region TreevView Methods
        /// <summary>
        /// Virtual method that is called to initialize the ExplorerBase TreeView
        /// </summary>

        override protected void InitializeTreeView()
        {
            try
            {
                //TODO: Implement Base TreeView
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// Virtual method that is called to perform Form Load tasks
        /// </summary>

        override protected void ExplorerLoad()
        {
            try
            {
                //TODO: Implement Base TreeView
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// Virtual method that is called to build the ExplorerBase TreeView
        /// </summary>

        override protected void BuildTreeView()
        {
            try
            {
                //TODO: Implement Base TreeView
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        #endregion

        #region Load and Format
        private void AssortmentWorkspaceExplorer_Load(object sender, EventArgs e)
        {
            try
            {
                _assortmentWorkspaceSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAssortmentWorkspace);
                //_assortmentWorkspaceViewsSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAssortmentWorkspaceViews);  // TT#2014-MD - JSmith - Assortment Security
                //_assortmentWorkspaceViewsGlobalSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAssortmentWorkspaceViewsGlobal);
                //_assortmentWorkspaceViewsUserSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAssortmentWorkspaceViewsUser);
                // Begin TT#2014-MD - JSmith - Assortment Security
                _assortmentWorkspaceViewsGlobalSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAssortmentWorkspaceViewsGlobal);
                _assortmentWorkspaceViewsUserSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAssortmentWorkspaceViewsUser);
                // End TT#2014-MD - JSmith - Assortment Security

				// BEGIN TT#765-MD - Stodd - Add toolbars to Assortment Workspace
				// check for saved toolbar manager layout
				InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
				InfragisticsLayout toolbarManagerLayout = layoutData.InfragisticsLayout_Read(SAB.ClientServerSession.UserRID, eLayoutID.assortmentWorkspaceToolbars);
				if (toolbarManagerLayout.LayoutLength > 0)
				{
					this.headerToolbarsManager.LoadFromBinary(toolbarManagerLayout.LayoutStream);
					((Infragistics.Win.UltraWinToolbars.TextBoxTool)this.headerToolbarsManager.Tools["headerSelectedTextBox"]).Text = string.Empty;
					((Infragistics.Win.UltraWinToolbars.TextBoxTool)this.headerToolbarsManager.Tools["headerTotalTextBox"]).Text = string.Empty;
					((Infragistics.Win.UltraWinToolbars.TextBoxTool)this.headerToolbarsManager.Tools["quantityAllocateSelectedTextBox"]).Text = string.Empty;
					((Infragistics.Win.UltraWinToolbars.TextBoxTool)this.headerToolbarsManager.Tools["quantityAllocateTotalTextBox"]).Text = string.Empty;
				}
				//Infragistics.Win.UltraWinToolbars.ComboBoxTool cbo = (Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.headerToolbarsManager.Tools["viewComboBox"];  // TT#1386-MD - stodd - merge assortment into 5.4 - 
				//if (cbo.ValueList.ValueListItems.Count > 0)
				//{
				//    cbo.SelectedIndex = 0;
				//}
				// END TT#765-MD - Stodd - Add toolbars to Assortment Workspace

                if (_assortmentWorkspaceSecurity.AccessDenied) 
                {
					//gbxHeaderCount.Visible = false;
					//lblHeaderCount.Visible = false;
					//lblHeaderTotal.Visible = false;
					//gbxTotalQty.Visible = false;
					//lblSelQty.Visible = false;
					//lblTotQty.Visible = false;
					//cboView.Visible = false;
                    EAB.Explorer.RemoveMenuOption(Include.btRestoreLayout); 
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;

                    _fromLoadEvent = true;
					//gbxHeaderCount.Visible = true;
					//lblHeaderCount.Visible = true;
					//lblHeaderTotal.Visible = true;
					//gbxTotalQty.Visible = true;
					//lblSelQty.Visible = true;
					//lblTotQty.Visible = true;
					//cboView.Visible = true;
                    EAB.Explorer.AddMenuOption(Include.btRestoreLayout); 

                    _userRIDList = new ArrayList();
                    _userRIDList.Add(Include.GlobalUserRID);
                    _userRIDList.Add(_SAB.ClientServerSession.UserRID);


                    //Begin TT#1313-MD -jsobek -Header Filters -performance

                    int viewRID = _userGridView.UserGridView_Read(_SAB.ClientServerSession.UserRID, eLayoutID.assortmentWorkspaceGrid);
                    //Begin TT#1313-MD -jsobek -Header Filters
                    bool useViewWorkspaceFilter = false;
                    bool useFilterSorting = false;   // TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
                    if (viewRID != Include.NoRID && !_fromFilterWindow)
                    {
                        //bool useFilterSorting = false;   // TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
                        int workspaceFilterRID = _gridViewData.GridViewReadWorkspaceFilterRID(viewRID, ref useFilterSorting);
                        if (workspaceFilterRID != Include.NoRID)
                        {
                            useViewWorkspaceFilter = true;
                            this._headerFilterRID = workspaceFilterRID;
                        }
 
                        //if (_gridViewData.GridViewFilterExists(viewRID)) // apply View filter to User Workspace FIlter 
                        //{
                        //    ApplyViewToUserFilter(viewRID, _SAB.ClientServerSession.UserRID);
                        //}
                    }
                  
                    if (useViewWorkspaceFilter == false) // use the current user workspace filter
                    { 
                        this._headerFilterRID = _filterData.WorkspaceCurrentFilter_Read(_SAB.ClientServerSession.UserRID, eWorkspaceType.AssortmentWorkspace);
                    }

                    LoadHeadersOnGrid();
                    BindViewCombo();
                    BindFilterComboBox();
					// Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
                    //SetUserView(viewRID);
                    SetUserView(viewRID, useFilterSorting);
					// End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
                    SetViewComboEnabled();
                    SetHeaderFilter(_headerFilterRID);	// TT#1386-MD - stodd - merge assortment into 5.4
                    //End TT#1313-MD -jsobek -Header Filters
                    

                    if (_assortmentWorkspaceSecurity.IsReadOnly)
                    {
                        SetControlReadOnly(this, true);
                    }

                    // Begin TT#2014-MD - JSmith - Assortment Security
                    if (_assortmentWorkspaceViewsUserSecurity.AllowUpdate || _assortmentWorkspaceViewsGlobalSecurity.AllowUpdate)
                    {
                        this.headerToolbarsManager.Tools["saveView"].SharedProps.Enabled = true;
                    }
                    else
                    {
                        this.headerToolbarsManager.Tools["saveView"].SharedProps.Enabled = false;
                    }
                    // End TT#2014-MD - JSmith - Assortment Security

                    // Assign a CreationFilter to the grid. 
                    // The CreationFilter will trap the ColumnChooserButtonUIElement in the grid
                    // and set the tooltip information. 
                    this.ugAssortments.CreationFilter = new ToolTipItemCreationFilter();

                    //Begin TT#1313-MD -jsobek -Header Filters -performance
                    //if (ugAssortments.Rows.Count > 0)
                    //{
                    //    ugAssortments.ActiveRow = ugAssortments.Rows[0];
                    //}
                    //End TT#1313-MD -jsobek -Header Filters -performance
                    _fromLoadEvent = false;
                }
                // Begin TT#1389-MD - stodd - disable floating
                this.headerToolbarsManager.ToolbarSettings.AllowFloating = DefaultableBoolean.False;
                //this.headerToolbarsManager.ToolbarSettings.AllowHiding = DefaultableBoolean.False;
                // End TT#1389-MD - 
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            // Format for XP, if applicable
            if (Environment.OSVersion.Version.Major > 4 && Environment.OSVersion.Version.Minor > 0 &&
                System.IO.File.Exists(Application.ExecutablePath + ".manifest"))
            {
                FormatForXP(this);
            }
        }
        
        //Begin TT#1313-MD -jsobek -Header Filters -performance
        private void LoadHeadersOnGrid()
        {
            try
            {
                FilterHeaderOptions headerFilterOptions = new FilterHeaderOptions();
                headerFilterOptions.USE_WORKSPACE_FIELDS = true;
                headerFilterOptions.filterType = filterTypes.AssortmentFilter;
                _headerProfileArrayList = _SAB.HeaderServerSession.GetHeadersForWorkspace(this._headerFilterRID, headerFilterOptions);


                // BEGIN TT#488-MD - Stodd - Group Allocation
                // This removes any Group Allocation types from the list.
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
                // END TT#488-MD - Stodd - Group Allocation

                //_assrtWorkFilterProfile = new AssortmentWorkspaceFilterProfile(_SAB.ClientServerSession.UserRID);   // MID Track #5935 //TT#1313-MD -jsobek -Header Filters -do not re-filter in the AWS

                _hierMaint = new HierarchyMaintenance(_SAB);

                _masterKeyList = new ArrayList();

                //_headerCharGroupProfileList = _SAB.HeaderServerSession.GetHeaderCharGroups();

                _selectedHeaderKeyList = new ArrayList();
                _selectedAssortmentKeyList = new ArrayList(); // TT#488 - MD - Jellis - Group Allocation
                _selectedRowsSequence.Clear();

                BuildDataSets();
                LoadHeaders();
                LoadGridValueLists();

                ugAssortments.DataSource = _bindSourceHeader;
                ugAssortments.DisplayLayout.MaxBandDepth = _maxBandDepth;
                UpdateSelectedTotals();

                if (ugAssortments.Rows.Count > 0)
                {
                    ugAssortments.ActiveRow = ugAssortments.Rows[0];
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        //End TT#1313-MD -jsobek -Header Filters -performance

		// BEGIN TT#765-MD - Stodd - Add toolbars to Assortment Workspace
		private void SaveToolbarLayout()
		{
			InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
			System.IO.MemoryStream toolbarManagerMemoryStream = new System.IO.MemoryStream();
			this.headerToolbarsManager.SaveAsBinary(toolbarManagerMemoryStream, true);
			layoutData.InfragisticsLayout_Save(SAB.ClientServerSession.UserRID, MIDRetail.DataCommon.eLayoutID.assortmentWorkspaceToolbars, toolbarManagerMemoryStream);
		}
		// END TT#765-MD - Stodd - Add toolbars to Assortment Workspace

        protected void FormatForXP(Control ctl)
        {
            foreach (Control c in ctl.Controls)
            {
                FormatForXP(c);
            }

            if (ctl.GetType().BaseType == typeof(ButtonBase))
            {
                ((ButtonBase)ctl).FlatStyle = FlatStyle.System;
            }
        }

        //Begin TT#1313-MD -Header Filters
        //private void ApplyViewToUserFilter(int aViewRID, int aUserRID)
        //{
        //    try
        //    {
        //        _assrtWorkFilterData.OpenUpdateConnection();
        //        try
        //        {
        //            _assrtWorkFilterData.ApplyViewFilterToUserFilter(aViewRID, aUserRID);
        //            _assrtWorkFilterData.CommitData();
        //        }
        //        catch (Exception exc)
        //        {
        //            _assrtWorkFilterData.Rollback();
        //            string message = exc.ToString();
        //            throw;
        //        }
        //        finally
        //        {
        //            _assrtWorkFilterData.CloseUpdateConnection();
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        string message = exc.ToString();
        //        throw;
        //    }
        //}
        //End TT#1313-MD -Header Filters

     
        private void BindViewCombo()
        {
            try
            {
                _bindingView = true;

                _dtView = _gridViewData.GridView_Read((int)eLayoutID.assortmentWorkspaceGrid, _userRIDList,true);
				// BEGIN TT#765-MD - Stodd - Add toolbars to Assortment Workspace
				//+ Why the Spaces??
				// For some reason, when the control valuelist display value is a blank, the display shows you the value list key instead.
                //_dtView.Rows.Add(new object[] { Include.NoRID, _SAB.ClientServerSession.UserRID, (int)eLayoutID.assortmentWorkspaceGrid, string.Empty });
				_dtView.Rows.Add(new object[] { Include.NoRID, _SAB.ClientServerSession.UserRID, (int)eLayoutID.assortmentWorkspaceGrid, "     " });

				//cboView.ValueMember = "VIEW_RID";
				//cboView.DisplayMember = "VIEW_ID";
				//cboView.DataSource = _dtView;
				//cboView.SelectedValue = -1;
				
				LoadViewsOnToolbar(_dtView);
				// END TT#765-MD - Stodd - Add toolbars to Assortment Workspace

                _bindingView = false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        //Begin TT#1313-MD -jsobek -Header FiltersWF_AWS
        public void BindFilterComboBox()
        {
            try
            {
                _bindingView = true;
                ArrayList userRIDList = new ArrayList();
                userRIDList.Add(Include.GlobalUserRID);
                userRIDList.Add(SAB.ClientServerSession.UserRID);

                DataTable dtAssortmentFilters = _filterData.FilterRead(filterTypes.AssortmentFilter, eProfileType.FilterAssortment, userRIDList);
                
                LoadAssortmentFiltersOnToolbar(dtAssortmentFilters);
				// Begin TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns
                //((Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.headerToolbarsManager.Tools["headerFilterComboBox"]).Value = this._headerFilterRID;
                Infragistics.Win.UltraWinToolbars.ControlContainerTool cct = (Infragistics.Win.UltraWinToolbars.ControlContainerTool)this.headerToolbarsManager.Tools["ControlContainerAsrtFilter"];
                MIDComboBoxEnh.MyComboBox cmbFilter = (MIDComboBoxEnh.MyComboBox)cct.Control;
                cmbFilter.SelectedValue = this._headerFilterRID;
				// Begin TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns
                _bindingView = false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        //End TT#1313-MD -jsobek -Header Filters

        private void SetText()
        {
            try
            {
				// BEGIN TT#765-MD - Stodd - Add toolbars to Assortment Workspace
                //gbxHeaderCount.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Assortments);
                //gbxTotalQty.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AssortmentQtys);
				// END TT#765-MD - Stodd - Add toolbars to Assortment Workspace
                _lblSelected = MIDText.GetTextOnly(eMIDTextCode.lbl_Selected);
                _lblTotal = MIDText.GetTextOnly(eMIDTextCode.lbl_Total);
                _lblQuantity = MIDText.GetTextOnly(eMIDTextCode.lbl_Quantity);
                _lblColumnChooser = MIDText.GetTextOnly(eMIDTextCode.lbl_ColumnChooser); ;
                _lblDeleteRow = MIDText.GetTextOnly(eMIDTextCode.lbl_DeleteRow);
                _lblHeaderNotes = MIDText.GetTextOnly(eMIDTextCode.lbl_HeaderNotes);
                _lblHierarchyNode = MIDText.GetTextOnly(eMIDTextCode.lbl_HierarchyNode);
                _lblUser = MIDText.GetTextOnly(eMIDTextCode.lbl_User);
                _lblPhStyle = MIDText.GetTextOnly(eMIDTextCode.lbl_PhStyle);
                _lblSelectAction = MIDText.GetTextOnly(eMIDTextCode.lbl_SelectAction);
                _lblAssortment = MIDText.GetTextOnly(eMIDTextCode.lbl_Assortment);
                _noSizeDimensionLbl = MIDText.GetTextOnly((int)eMIDTextCode.lbl_NoSecondarySize);

                // Context Menu text
                this.cmsReview.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Review);
                this.cmsFilter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter);
                this.cmsSearch.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Search);
                this.cmsDelete.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Delete);
               
                this.cmsSaveView.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SaveView);
                
                //this.cmsSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Edit);
                _thisTitle = MIDText.GetTextOnly((int)eMIDTextCode.frm_WorkspaceExplorer);

                //this.toolTip1.SetToolTip(cboView, MIDText.GetTextOnly(eMIDTextCode.lbl_Views)); 	// TT#765-MD - Stodd - Add toolbars to Assortment Workspace
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Build DataSet data
        private void BuildDataSets()
        {
            _dtHeader = MIDEnvironment.CreateDataTable("Header");

            _colorsForStyle.Clear();
            _nodeDataHash.Clear();
            _nodeDataHashLastKey = 0;
            //_sizeGroupHashLastKey = 0;
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
                // START TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace
                //_dtHeader.Columns.Add("PO");
                //_dtHeader.Columns.Add("Vendor");
                //_dtHeader.Columns.Add("Workflow");
                //_dtHeader.Columns.Add("APIWorkflow");
                //_dtHeader.Columns.Add("DC");

                //_dtHeader.Columns.Add("Intransit", System.Type.GetType("System.Int32"));
                //_dtHeader.Columns.Add("ShipStatus", System.Type.GetType("System.Int32"));
                //_dtHeader.Columns.Add("Release", System.Type.GetType("System.String"));
                // END TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace

                _dtHeader.Columns.Add("Notes");
                _dtHeader.Columns.Add("Interfaced", System.Type.GetType("System.Boolean"));
                _dtHeader.Columns.Add("ChildTotal", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("MultiSortSeq", System.Type.GetType("System.Int32"));
                //_dtHeader.Columns.Add("Master");   // TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace
                _dtHeader.Columns.Add("AllocatedUnits", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("OrigAllocatedUnits", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("RsvAllocatedUnits", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("AsrtType", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("AsrtSortSeq", System.Type.GetType("System.Int32"));
                _dtHeader.Columns.Add("CharUpdated", System.Type.GetType("System.Boolean"));

                // save column count before characteristics are added for characteristic processing 
                _nonCharColCount = _dtHeader.Columns.Count;

                //foreach (HeaderCharGroupProfile hcgp in _headerCharGroupProfileList)
                //{
                //    _dtHeader.Columns.Add(hcgp.ID);
                //    _dtHeader.Columns[hcgp.ID].ExtendedProperties.Add("IsChar", hcgp);

                //    if (hcgp.ListInd)   // uses a drop down list
                //    {
                //        _dtHeader.Columns[hcgp.ID].DataType = System.Type.GetType("System.Int32");
                //    }
                //    else
                //    {
                //        switch (hcgp.Type)
                //        {
                //            case eHeaderCharType.date:
                //                _dtHeader.Columns[hcgp.ID].DataType = System.Type.GetType("System.DateTime");
                //                break;

                //            case eHeaderCharType.number:
                //            case eHeaderCharType.dollar:
                //                _dtHeader.Columns[hcgp.ID].DataType = System.Type.GetType("System.Double");
                //                break;
                //        }
                //    }

                //}

                // header defaults and constraints
                _dtHeader.Columns["HeaderID"].AllowDBNull = false;
                _dtHeader.Columns["HeaderID"].Unique = true;
                _dtHeader.Columns["Product"].AllowDBNull = true;            // allow nulls because of Assortment
                _dtHeader.Columns["Description"].AllowDBNull = true;        // allow nulls because of Assortment
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
                _dtHeader.Columns["UnitRetail"].DefaultValue = 0;  // TT#1975-MD - AGallagher  - Content Tab - Unit Retail and Cost are not editable
                _dtHeader.Columns["UnitCost"].DefaultValue = 0;  // TT#1975-MD - AGallagher  - Content Tab - Unit Retail and Cost are not editable
                _dtHeader.Columns["Multiple"].DefaultValue = Include.DefaultUnitMultiple;
                _dtHeader.Columns["SizeGroup"].DefaultValue = Include.UndefinedSizeGroupRID;
                //_dtHeader.Columns["Release"].DefaultValue = String.Empty;  // TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace
                _dtHeader.Columns["Interfaced"].DefaultValue = false;
                //_dtHeader.Columns["Master"].DefaultValue = String.Empty;  // TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace
                _dtHeader.Columns["CharUpdated"].DefaultValue = false;
                _dtHeader.PrimaryKey = new DataColumn[] { _dtHeader.Columns["KeyH"] };

                DataSet ds = MIDEnvironment.CreateDataSet();
                ds.Tables.Add(_dtHeader);
                //ds.Relations.Add("Placeholder", ds.Tables["Header"].Columns["KeyH"], ds.Tables["Header"].Columns["AsrtRID"], false);

				


                _bindSourceHeader = new BindingSource(ds, "Header");
            }
            catch (Exception ex)
            {
                HandleException(ex);
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
                HandleException(ex);
            }
        }

        private void LoadHeader(AllocationHeaderProfile ahp)
        {
            try
            {
                int headerType, headerStatus, intransitStatus, shipStatus;
                object groupRID, asrtRID, phRID, multiSortSeq, asrtSortSeq;

                HierarchyNodeSecurityProfile securityNode;
                eSecurityType styleSecurity;

                //WorkflowBaseData workflowData = new WorkflowBaseData(); //TT#1313-MD -jsobek -Header Filters -performance
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

                // Begin TT#2022-MD - JSmith - Invalid Cast Exception selecting Assortment Filter
                if ((eHeaderType)headerType != eHeaderType.Assortment)
                {
                    return;
                }
                // End TT#2022-MD - JSmith - Invalid Cast Exception selecting Assortment Filter

                switch ((eHeaderType)headerType)
                {
                    case eHeaderType.Assortment:
                    case eHeaderType.Placeholder:
                        canUpdate = _assortmentWorkspaceSecurity.AllowUpdate;
                        canView = _assortmentWorkspaceSecurity.AllowView;
                        break;

                    default:
                        securityNode = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(ahp.StyleHnRID, (int)eSecurityTypes.Allocation);
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

                    // check for size group in hash; if not found read it
                    SizeGroupProfile sgp = (SizeGroupProfile)_sizeGroupHash[ahp.SizeGroupRID];
                    if (sgp == null)
                    {
                        sgp = new SizeGroupProfile(ahp.SizeGroupRID);
                        _sizeGroupHash.Add(ahp.SizeGroupRID, sgp);
                    }
                    string workflowMethodStr = string.Empty;
                    // check for workflow name in hash; if not found read it
                    if (ahp.WorkflowRID > Include.UndefinedWorkflowRID)
                    {
                        workflowMethodStr = (string)_workflowNameHash[ahp.WorkflowRID];
                        if (workflowMethodStr == null)
                        {
                            workflowMethodStr = ahp.WorkflowName; // workflowData.GetWorkflowName(ahp.WorkflowRID); //TT#1313-MD -jsobek -Header Filters -performance
                            _workflowNameHash.Add(ahp.WorkflowRID, workflowMethodStr);
                        }
                    }
                    else if (ahp.MethodRID > Include.UndefinedMethodRID)
                    {
                        // check for method in hash; if not found read it
                        //Begin TT#1313-MD -jsobek -Header Filters -performance
                        //ApplicationBaseMethod abm = (ApplicationBaseMethod)_methodHash[ahp.MethodRID];
                        //if (abm == null)
                        //{
                        //    abm = (ApplicationBaseMethod)_getMethods.GetUnknownMethod(ahp.MethodRID, false);
                        //    _methodHash.Add(ahp.MethodRID, abm);
                        //}

                        workflowMethodStr = ahp.HeaderMethodName; //abm.Name;
                        //End TT#1313-MD -jsobek -Header Filters -performance
                    }

                    string APIworkflowMethodStr = string.Empty;
                    // check for workflow name in hash; if not found read it
                    if (ahp.API_WorkflowRID > Include.UndefinedWorkflowRID)
                    {
                        APIworkflowMethodStr = (string)_workflowNameHash[ahp.API_WorkflowRID];
                        if (APIworkflowMethodStr == null)
                        {
                            APIworkflowMethodStr = ahp.APIWorkflowName; // workflowData.GetWorkflowName(ahp.API_WorkflowRID); //TT#1313-MD -jsobek -Header Filters -performance
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

                    //use first parent ID if multiple parents
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
                            //_multiHeaders.Rows.Add(new object[] { ahp.Key, ahp.HeaderID });
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
                    else if (ahp.AsrtRID != Include.NoRID)  // headers attached to an assortment
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
                        HierarchyNodeProfile anchorHnp = _SAB.HierarchyServerSession.GetNodeData((int)anchorHnRID, false);
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
                                        // Start TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace
                                        //ahp.SizeGroupRID, ahp.AllocationMultipleDsply, ahp.PurchaseOrder, ahp.Vendor, // MID Track 5761 Allocation Multiple not saved on DB 
                                        //workflowMethodStr, APIworkflowMethodStr, ahp.DistributionCenter, 
                                        //intransitStatus, shipStatus, releaseDate, ahp.AllocationNotes,
                                        //ahp.IsInterfaced, 0, multiSortSeq, msgMasterSubord, 
                                        ahp.SizeGroupRID, ahp.AllocationMultipleDsply, 
                                        ahp.AllocationNotes,
                                        ahp.IsInterfaced, 0, multiSortSeq,  
                                        // End TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace
                                        ahp.AllocatedUnits, ahp.OrigAllocatedUnits, 
                                        ahp.RsvAllocatedUnits, ahp.AsrtType, asrtSortSeq });

                    //AddCharacteristicsToHeader(headerRow, ahp.Characteristics, false);

                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        //private void AddCharacteristicsToHeader(DataRow aHeaderRow, HeaderCharProfileList aCharList, bool aUseDetailData)
        //{
        //    try
        //    {
        //        for (int i = _nonCharColCount; i < _dtHeader.Columns.Count; i++)
        //        {
        //            DataColumn col = (DataColumn)_dtHeader.Columns[i];
        //            if (col.ExtendedProperties.ContainsKey("IsChar"))
        //            {
        //                HeaderCharGroupProfile hcgp = (HeaderCharGroupProfile)col.ExtendedProperties["IsChar"];
        //                HeaderCharProfile hcp = (HeaderCharProfile)aCharList.FindKey(hcgp.Key);

        //                aHeaderRow[col.ColumnName] = System.DBNull.Value;
        //                if (hcp.Text != null)
        //                {
        //                    if (hcgp.ListInd) // uses a drop down list
        //                    {
        //                        aHeaderRow[col.ColumnName] = hcp.CharRID;
        //                    }
        //                    else
        //                    {
        //                        aHeaderRow[col.ColumnName] = hcp.Text;
                                // need to save the char RID for update routine
                                 
        //                        if (hcp.HeaderCharType == eHeaderCharType.date)
        //                        {
        //                            charString = hcgp.Key.ToString() + "~" + hcp.DateValue.ToString();
        //                        }
        //                        else
        //                        {
        //                            charString = hcgp.Key.ToString() + "~" + hcp.Text;
        //                        }
        //                        if (!col.ExtendedProperties.ContainsKey(charString))
        //                        {
        //                            col.ExtendedProperties.Add(charString, hcp.CharRID);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}
//TT#440 - MD - RBeck _ Make Header Characteristics references by RID

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
                        _nodeDataHashLastValue = _SAB.HierarchyServerSession.GetNodeData(aHnRID, false);
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

        private void LoadGridValueLists()
        {
            try
            {
                // Assortments
                _assortmentValueList = new ValueList();
                _assortmentValueList.Key = "AssortmentID";

                foreach (DataRow dr in _assortments.Rows)
                {
                    _assortmentValueList.ValueListItems.Add(Convert.ToInt32(dr["AsrtRID"], CultureInfo.CurrentUICulture), dr["AssortmentID"].ToString());
                }

                // PlaceHolders
                _placeHolderValueList = new ValueList();
                _placeHolderValueList.Key = "PlaceHolderID";

                foreach (DataRow dr in _placeHolders.Rows)
                {
                    _placeHolderValueList.ValueListItems.Add(Convert.ToInt32(dr["PlaceHolderRID"], CultureInfo.CurrentUICulture), dr["PlaceHolderID"].ToString());
                }

                // Header Type
                _headerTypeValueList = LoadMIDTextValueList("HeaderType", eMIDTextType.eHeaderType, eMIDTextOrderBy.TextCode);
                if (!_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
                {
                    bool phRemoved = false, asrtRemoved = false;
                    // Begin TT#1966-MD - JSmith - DC Fulfillment
                    bool masterRemoved = false;
                    // End TT#1966-MD - JSmith - DC Fulfillment
                    for (int i = _headerTypeValueList.ValueListItems.Count - 1; i >= 0; i--)
                    {
                        ValueListItem vli = _headerTypeValueList.ValueListItems[i];
                        int value = Convert.ToInt32(vli.DataValue, CultureInfo.CurrentUICulture);
                        if (value == Convert.ToInt32(eHeaderType.Assortment, CultureInfo.CurrentUICulture))
                        {
                            _headerTypeValueList.ValueListItems.Remove(vli);
                            asrtRemoved = true;
                        }
                        else if (value == Convert.ToInt32(eHeaderType.Placeholder, CultureInfo.CurrentUICulture))
                        {
                            _headerTypeValueList.ValueListItems.Remove(vli);
                            phRemoved = true;
                        }
                        // Begin TT#1966-MD - JSmith - DC Fulfillment
                        else if (value == Convert.ToInt32(eHeaderType.Master, CultureInfo.CurrentUICulture))
                        {
                            _headerTypeValueList.ValueListItems.Remove(vli);
                            masterRemoved = true;
                        }

                        //if (asrtRemoved && phRemoved)
                        if (asrtRemoved && phRemoved && masterRemoved)
                        // End TT#1966-MD - JSmith - DC Fulfillment
                        {
                            break;
                        }
                    }
                }

                // Header Status
                _headerStatusValueList = LoadMIDTextValueList("Status", eMIDTextType.eHeaderAllocationStatus, eMIDTextOrderBy.TextCode);

                // Header Intransit
                _headerIntransitValueList = LoadMIDTextValueList("Intransit", eMIDTextType.eHeaderIntransitStatus, eMIDTextOrderBy.TextCode);

                // Header Ship Status
                _headerShipStatusValueList = LoadMIDTextValueList("ShipStatus", eMIDTextType.eHeaderShipStatus, eMIDTextOrderBy.TextCode);

                // Size Groups
                if (_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
                {
                    _sizeGroupValueList = new ValueList();
                    _sizeGroupValueList.Key = "SizeGroup";
                    SizeGroupList sgl = new SizeGroupList(eProfileType.SizeGroup);
                    //sgl.LoadAll(true);
                    sgl.LoadAll(IncludeUndefinedGroup: true, doReadSizeCodeListFromDatabase: false); //TT#1313-MD -jsobek -Header Filters -performance
                    foreach (SizeGroupProfile sgp in sgl.ArrayList)
                    {
                        _sizeGroupValueList.ValueListItems.Add(sgp.Key, sgp.SizeGroupName);
                    }
                }

                // Pack Type
                _packTypeValueList = LoadMIDTextValueList("PackType", eMIDTextType.ePackType, eMIDTextOrderBy.TextCode);

                // Characteristics
                //_charValueListsHash = new Hashtable();   //TT#440 - MD - RBeck _ Make Header Characteristics references by RID

                //foreach (HeaderCharGroupProfile hcgp in _headerCharGroupProfileList)
                //{
                //    if (hcgp.Characteristics.Count > 0)
                //    {
                //        ValueList charValueList = new ValueList();
                //        charValueList.Key = hcgp.ID;
                //        charValueList.ValueListItems.Add(Include.NoRID, Include.NoneText);
                //        foreach (HeaderCharInfo hci in hcgp.Characteristics.Values)
                //        {
                //            string value = GetCharacteristicValue(hci, hcgp.Type);
                //            charValueList.ValueListItems.Add(hci.RID, value);
                //        }
                //        _charValueListsHash.Add(hcgp.ID, charValueList);
                //    }
                //}

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private ValueList LoadMIDTextValueList(string aKey, eMIDTextType aMIDTextType, eMIDTextOrderBy aMIDTextOrderBy)
        {
            ValueList valueList = new ValueList();
            valueList.Key = aKey;
            try
            {
                DataTable dataTable = MIDText.GetTextType(aMIDTextType, aMIDTextOrderBy);

                foreach (DataRow dr in dataTable.Rows)
                {
                    valueList.ValueListItems.Add(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture), dr["TEXT_VALUE"].ToString());
                    if (Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture) == (int)eHeaderType.Placeholder)
                    {
                        _lblPlaceholder = dr["TEXT_VALUE"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return valueList;
        }

        private string GetCharacteristicValue(HeaderCharInfo aCharInfo, eHeaderCharType aCharType)
        {
            string value = null;
            try
            {
                switch (aCharType)
                {
                    case eHeaderCharType.text:
                        value = aCharInfo.TextValue;
                        break;

                    case eHeaderCharType.dollar:
                        value = Convert.ToString(aCharInfo.DollarValue, CultureInfo.CurrentUICulture);
                        break;

                    case eHeaderCharType.number:
                        value = Convert.ToString(aCharInfo.NumberValue, CultureInfo.CurrentUICulture);
                        break;

                    case eHeaderCharType.date:
                        value = Convert.ToString(aCharInfo.DateValue, CultureInfo.CurrentUICulture);
                        break;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return value;
        }


        #endregion

		#region Public Methods for External Access
		public int GetSelectedHeaders()
		{
			return this.ugAssortments.Selected.Rows.Count;
		}

        // BEGIN TT#2016-MD - AGallagher - Assortment Review Navigation
        public int GetSelectedAssortmentKey()
        {
            int AssortmentKeyRID = (int)_selectedAssortmentKeyList[0];
            return AssortmentKeyRID;
        }
        // END TT#2016-MD - AGallagher - Assortment Review Navigation

        public void ReloadUpdatedAssortments(int[] aHdrList)
        {
            int asrtRID;
            bool assortmentAdded = false;
            try
            {
                for (int i = 0; i < aHdrList.Length; i++)
                {
                    asrtRID = aHdrList[i];
                    AllocationHeaderProfile ahp = _SAB.HeaderServerSession.GetHeaderData(asrtRID, false, false, true);
                    DataRow dr = _dtHeader.Rows.Find(asrtRID);
                    if (dr != null)
                    {
                        UpdateAssortmentDataRow(dr, ahp);
                    }
                    else if (ahp.Key != Include.NoRID)
                    {
                        LoadHeader(ahp);
						// Begin TT#2 - stodd - assortment
                        //_assortments.Rows.Add(new object[] { ahp.Key, ahp.HeaderID });
						// End TT#2 - stodd - assortment
                        //ugAssortments.DisplayLayout.ValueLists["AssortmentID"].ValueListItems.Add(ahp.Key, ahp.HeaderID);
                        _assortmentValueList.ValueListItems.Add(ahp.Key, ahp.HeaderID);
                        assortmentAdded = true;
                    }
                }
				
                _assortments.AcceptChanges();
                _dtHeader.AcceptChanges();
                if (assortmentAdded)
                {
                    ugAssortments.DataSource = _bindSourceHeader;
                    ugAssortments.DisplayLayout.MaxBandDepth = _maxBandDepth;
                }
                UpdateSelectedTotals();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        
        private void UpdateAssortmentDataRow(DataRow aDataRow, AllocationHeaderProfile ahp)
        {
            try
            {
                int parentRID = 0, intransitStatus, shipStatus;
                string parentID = string.Empty;
                string anchorNode = null;
                string dateRange = null;
                string releaseDate = string.Empty;
                object anchorHnRID = System.DBNull.Value;
                object cdrRID = System.DBNull.Value;

                Header headerData = new Header();
                DataTable dtAssortProperties = headerData.GetAssortmentProperties(ahp.Key);
                
                if (dtAssortProperties.Rows.Count > 0)
                {
                    anchorHnRID = Convert.ToInt32(dtAssortProperties.Rows[0]["ANCHOR_HN_RID"], CultureInfo.CurrentUICulture);
                    HierarchyNodeProfile anchorHnp = _SAB.HierarchyServerSession.GetNodeData((int)anchorHnRID, false);
                    anchorNode = anchorHnp.LevelText;
                    cdrRID = Convert.ToInt32(dtAssortProperties.Rows[0]["CDR_RID"], CultureInfo.CurrentUICulture);
                    DateRangeProfile dr = SAB.ClientServerSession.Calendar.GetDateRange((int)cdrRID);
                    dateRange = dr.DisplayDate;
                }

                HierarchyNodeProfile hnp_style = this.GetNodeData(ahp.StyleHnRID);
                parentRID = Convert.ToInt32(hnp_style.Parents[0], CultureInfo.CurrentUICulture);
                HierarchyNodeProfile hnpProduct = this.GetNodeData(parentRID);
                parentID = hnpProduct.LevelText;

                intransitStatus = Convert.ToInt32(ahp.HeaderIntransitStatus, CultureInfo.CurrentUICulture);
                shipStatus = Convert.ToInt32(ahp.HeaderShipStatus, CultureInfo.CurrentUICulture);

                //WorkflowBaseData workflowData = new WorkflowBaseData(); //TT#1313-MD -jsobek -Header Filters -performance
                string workflowMethodStr = string.Empty;
                // check for workflow name in hash; if not found read it
                if (ahp.WorkflowRID > Include.UndefinedWorkflowRID)
                {
                    workflowMethodStr = (string)_workflowNameHash[ahp.WorkflowRID];
                    if (workflowMethodStr == null)
                    {
                        workflowMethodStr = ahp.WorkflowName; // workflowData.GetWorkflowName(ahp.WorkflowRID); //TT#1313-MD -jsobek -Header Filters -performance
                        _workflowNameHash.Add(ahp.WorkflowRID, workflowMethodStr);
                    }
                }
                else if (ahp.MethodRID > Include.UndefinedMethodRID)
                {
                    // check for method in hash; if not found read it
                    //Begin TT#1313-MD -jsobek -Header Filters -performance
                    //ApplicationBaseMethod abm = (ApplicationBaseMethod)_methodHash[ahp.MethodRID];
                    //if (abm == null)
                    //{
                    //    abm = (ApplicationBaseMethod)_getMethods.GetUnknownMethod(ahp.MethodRID, false);
                    //    _methodHash.Add(ahp.MethodRID, abm);
                    //}
                    workflowMethodStr = ahp.HeaderMethodName; // abm.Name;
                    //End TT#1313-MD -jsobek -Header Filters -performance
                }

                string APIworkflowMethodStr = string.Empty;
                // check for workflow name in hash; if not found read it
                if (ahp.API_WorkflowRID > Include.UndefinedWorkflowRID)
                {
                    APIworkflowMethodStr = (string)_workflowNameHash[ahp.API_WorkflowRID];
                    if (APIworkflowMethodStr == null)
                    {
                        APIworkflowMethodStr = ahp.APIWorkflowName; // workflowData.GetWorkflowName(ahp.API_WorkflowRID); //TT#1313-MD -jsobek -Header Filters -performance
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

                string oldID = Convert.ToString(aDataRow["HeaderID"], CultureInfo.CurrentUICulture);

                bool headerIDChanged = (oldID != ahp.HeaderID) ? true : false;
                aDataRow["HeaderID"] = ahp.HeaderID;
                aDataRow["Status"] = Convert.ToInt32(ahp.HeaderAllocationStatus, CultureInfo.CurrentUICulture);
                aDataRow["Date"] = ahp.HeaderDay;

                aDataRow["AnchorHnRID"] = anchorHnRID;
                aDataRow["AnchorNode"] =  anchorNode;
                aDataRow["CdrRID"] = cdrRID ;
                aDataRow["DateRange"] = dateRange;
                aDataRow["ProductRID"] = parentRID;
                aDataRow["Product"] = parentID;
                aDataRow["StyleHnRID"] = ahp.StyleHnRID;
                aDataRow["Style"] = hnp_style.LevelText;
                aDataRow["Description"] = ahp.HeaderDescription;
                aDataRow["HdrQuantity"] = ahp.TotalUnitsToAllocate;
                aDataRow["UnitRetail"] = ahp.UnitRetail;  // TT#1975-MD - AGallagher  - Content Tab - Unit Retail and Cost are not editable
                aDataRow["UnitCost"] = ahp.UnitCost;  // TT#1975-MD - AGallagher  - Content Tab - Unit Retail and Cost are not editable
                aDataRow["Multiple"] = ahp.AllocationMultipleDsply;
                // START TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace
                //aDataRow["PO"] = ahp.PurchaseOrder;
                //aDataRow["Vendor"] = ahp.Vendor;
                //aDataRow["Workflow"] = workflowMethodStr;
                //aDataRow["APIWorkflow"] = APIworkflowMethodStr;
                //aDataRow["DC"] = ahp.DistributionCenter;
                //aDataRow["Intransit"] = intransitStatus;
                //aDataRow["ShipStatus"] = shipStatus;
                //aDataRow["Release"] = releaseDate;
                //aDataRow["Master"] = msgMasterSubord;
                // END TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace
                aDataRow["AllocatedUnits"] = ahp.AllocatedUnits;
                aDataRow["OrigAllocatedUnits"] = ahp.OrigAllocatedUnits;
                aDataRow["RsvAllocatedUnits"] = ahp.RsvAllocatedUnits;
                aDataRow["AsrtType"] = ahp.AsrtType;

                if (headerIDChanged)
                {
                    DataRow dRow = _assortments.Rows.Find(ahp.Key);
                    if (dRow != null)
                    {
                        dRow["AssortmentID"] = ahp.HeaderID;
                    }
                    _EAB.AllocationWorkspaceExplorer.UpdateAssortmentID(ahp.Key, ahp.HeaderID, ahp.AsrtType);	// TT#921 - MD - replace IRefresh with ReloadUpdatedHeaders for workspaces
                }    
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void DeleteAssortmentRows(int[] aHdrList)
        {
            int asrtRID;
            try
            {
                for (int i = 0; i < aHdrList.Length; i++)
                {
                    asrtRID = aHdrList[i];
                    DataRow dr = _dtHeader.Rows.Find(asrtRID);
                    if (dr != null)
                    {
                        _dtHeader.Rows.Remove(dr);
                        _dtHeader.AcceptChanges();
                        DataRow dRow = _assortments.Rows.Find(asrtRID);
                        if (dRow != null)
                        {
                            _assortments.Rows.Remove(dRow);
                            _assortments.AcceptChanges();
                        }
                        for (int j = _assortmentValueList.ValueListItems.Count - 1; j >= 0; j--)
                        {
                            ValueListItem vli = _assortmentValueList.ValueListItems[j];
                            int value = Convert.ToInt32(vli.DataValue, CultureInfo.CurrentUICulture);
                            if (value == asrtRID)
                            {
                                _assortmentValueList.ValueListItems.Remove(vli);
                                break;
                            }
                        }
                    }
                }
                UpdateSelectedTotals();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
		#endregion

        #region ugAssortments grid events and methods
        private void ugAssortments_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                if (_skipInitialize || _assortmentWorkspaceSecurity.AccessDenied)
                {
                    return;
                }

                _multiHeaderGroups.Clear();
                _assortmentGroups.Clear();

                // check for saved layout
                InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
                InfragisticsLayout layout = layoutData.InfragisticsLayout_Read(_SAB.ClientServerSession.UserRID, eLayoutID.assortmentWorkspaceGrid);
                if (layout.LayoutLength > 0)
                {
                    ugAssortments.DisplayLayout.Load(layout.LayoutStream);
                    layoutData.InfragisticsLayout_Delete(_SAB.ClientServerSession.UserRID, eLayoutID.assortmentWorkspaceGrid);
                }

                e.Layout.MaxRowScrollRegions = 1;
                e.Layout.MaxBandDepth = 3;
                //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;	// TT#1512-MD - stodd - ASSORTMENT Workspace-> double click and opens Selection screen instead of Matrix.  
                e.Layout.Override.HeaderClickAction = HeaderClickAction.SortMulti;
				// Begin TT#1404-MD - stodd - Assortment Workspace is unformated until a filter is chosen
                //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                //ugld.ApplyDefaults(e, false);  // Stodd - merge issue
                ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, false, 0, false);
				// End TT#1404-MD - stodd - Assortment Workspace is unformated until a filter is chosen
                e.Layout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                e.Layout.Override.SelectTypeGroupByRow = SelectType.Extended;
                e.Layout.UseFixedHeaders = true;
                e.Layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;  // TT#1404-MD - stodd - Assortment Workspace is unformated until a filter is chosen

                e.Layout.Bands[0].Columns["HeaderID"].Header.Fixed = true;
                e.Layout.Bands[0].Columns["HeaderID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
                e.Layout.Bands[0].Columns["HeaderID"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;

                e.Layout.Bands[0].Columns["KeyH"].Hidden = true;
                e.Layout.Bands[0].Columns["KeyP"].Hidden = true;
                e.Layout.Bands[0].Columns["KeyC"].Hidden = true;
                e.Layout.Bands[0].Columns["HdrGroupRID"].Hidden = true;
                e.Layout.Bands[0].Columns["AsrtRID"].Hidden = true;
                e.Layout.Bands[0].Columns["PlaceHolderRID"].Hidden = true;
                e.Layout.Bands[0].Columns["FunctionSecurity"].Hidden = true;
                e.Layout.Bands[0].Columns["Type"].Hidden = true;
                e.Layout.Bands[0].Columns["StyleSecurity"].Hidden = true;
                e.Layout.Bands[0].Columns["Notes"].Hidden = true;
                e.Layout.Bands[0].Columns["Interfaced"].Hidden = true;
                e.Layout.Bands[0].Columns["AnchorHnRID"].Hidden = true;
                e.Layout.Bands[0].Columns["CdrRID"].Hidden = true;
                e.Layout.Bands[0].Columns["ProductRID"].Hidden = true;
                e.Layout.Bands[0].Columns["Product"].Hidden = true;
                e.Layout.Bands[0].Columns["StyleHnRID"].Hidden = true;
                e.Layout.Bands[0].Columns["Style"].Hidden = true;
                e.Layout.Bands[0].Columns["Balance"].Hidden = true;
                e.Layout.Bands[0].Columns["UnitRetail"].Hidden = true;
                e.Layout.Bands[0].Columns["UnitCost"].Hidden = true;
                e.Layout.Bands[0].Columns["SizeGroup"].Hidden = true;
                e.Layout.Bands[0].Columns["Multiple"].Hidden = true;

                e.Layout.Bands[0].Columns["ChildTotal"].Hidden = true;
                e.Layout.Bands[0].Columns["MultiSortSeq"].Hidden = true;
                e.Layout.Bands[0].Columns["AsrtType"].Hidden = true;
                e.Layout.Bands[0].Columns["AsrtSortSeq"].Hidden = true;
                e.Layout.Bands[0].Columns["CharUpdated"].Hidden = true;

                if (!this._SAB.ClientServerSession.GlobalOptions.AppConfig.MasterAllocationInstalled)
                {
                    //e.Layout.Bands[0].Columns["Master"].Hidden = true; // TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace
                    e.Layout.Bands[0].Columns["AllocatedUnits"].Hidden = true;
                    e.Layout.Bands[0].Columns["OrigAllocatedUnits"].Hidden = true;
                    e.Layout.Bands[0].Columns["RsvAllocatedUnits"].Hidden = true;
                    //e.Layout.Bands[0].Columns["Master"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;  // TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace
                    e.Layout.Bands[0].Columns["AllocatedUnits"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    e.Layout.Bands[0].Columns["OrigAllocatedUnits"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    e.Layout.Bands[0].Columns["RsvAllocatedUnits"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                }
                //if (!_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
                //{
                //    e.Layout.Bands[0].Columns["AsrtRID"].Hidden = true;
                //    e.Layout.Bands[0].Columns["AnchorNode"].Hidden = true;
                //    e.Layout.Bands[0].Columns["AsrtRID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //    e.Layout.Bands[0].Columns["AnchorNode"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //}
                e.Layout.Bands[0].Columns["KeyH"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["KeyP"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["KeyC"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["HdrGroupRID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;  
                e.Layout.Bands[0].Columns["AsrtRID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True; 
                e.Layout.Bands[0].Columns["FunctionSecurity"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["Type"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["HeaderID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["PlaceHolderRID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["StyleSecurity"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["Notes"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                e.Layout.Bands[0].Columns["Status"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["Style"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["HdrQuantity"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["Balance"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["UnitRetail"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["UnitCost"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["SizeGroup"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["Multiple"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                e.Layout.Bands[0].Columns["Interfaced"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["AnchorHnRID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["CdrRID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["ProductRID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["Product"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["StyleHnRID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["ChildTotal"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["MultiSortSeq"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["AsrtType"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["AsrtSortSeq"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                e.Layout.Bands[0].Columns["CharUpdated"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                // START TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace
                //e.Layout.Bands[0].Columns["PO"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //e.Layout.Bands[0].Columns["Vendor"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //e.Layout.Bands[0].Columns["Workflow"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //e.Layout.Bands[0].Columns["APIWorkflow"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //e.Layout.Bands[0].Columns["DC"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //e.Layout.Bands[0].Columns["Intransit"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //e.Layout.Bands[0].Columns["ShipStatus"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //e.Layout.Bands[0].Columns["Release"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //e.Layout.Bands[0].Columns["Master"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                // END TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace

                if (!_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
                {
                    e.Layout.Bands[0].Columns["SizeGroup"].Hidden = true;
                }
                //e.Layout.Bands[0].Columns["HeaderID"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_HeaderID);
                e.Layout.Bands[0].Columns["HeaderID"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_AssortmentID);
                e.Layout.Bands[0].Columns["HdrGroupRID"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_MultiHeaderID);
                e.Layout.Bands[0].Columns["AsrtRID"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_AssortmentID);
                //e.Layout.Bands[0].Columns["PlaceHolderRID"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_PlaceholderID);
                e.Layout.Bands[0].Columns["Type"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Type);
                e.Layout.Bands[0].Columns["Date"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Date);
                e.Layout.Bands[0].Columns["Status"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_HeaderStatus);
                //e.Layout.Bands[0].Columns["AnchorNode"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_AnchorNode);
                e.Layout.Bands[0].Columns["AnchorNode"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_ApplyTo);
                e.Layout.Bands[0].Columns["DateRange"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Delivery);
                e.Layout.Bands[0].Columns["Product"].Header.Caption = _hlpProduct.LevelID;
                e.Layout.Bands[0].Columns["Style"].Header.Caption = _hlpStyle.LevelID;
                e.Layout.Bands[0].Columns["Description"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_WorkspaceDescription);
                e.Layout.Bands[0].Columns["HdrQuantity"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Quantity);
                e.Layout.Bands[0].Columns["Balance"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Balance);
                e.Layout.Bands[0].Columns["UnitRetail"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_UnitRetail);
                e.Layout.Bands[0].Columns["UnitCost"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_UnitCost);
                e.Layout.Bands[0].Columns["SizeGroup"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_SizeGroup);
                e.Layout.Bands[0].Columns["Multiple"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Multiple);
                // START TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace
                //e.Layout.Bands[0].Columns["PO"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_PurchaseOrder);
                //e.Layout.Bands[0].Columns["Vendor"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Vendor);
                //e.Layout.Bands[0].Columns["Workflow"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Workflow);
                //e.Layout.Bands[0].Columns["APIWorkflow"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_APIWorkflow);
                //e.Layout.Bands[0].Columns["DC"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_DistCenter);
                //e.Layout.Bands[0].Columns["Intransit"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Intransit);
                //e.Layout.Bands[0].Columns["ShipStatus"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_ShipStatus);
                //e.Layout.Bands[0].Columns["Release"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Release);
                // END TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace
                e.Layout.Bands[0].Columns["ChildTotal"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_ChildTotal);
                //e.Layout.Bands[0].Columns["Master"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_MasterSubord);  // TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace
                e.Layout.Bands[0].Columns["AllocatedUnits"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_AllocatedUnits);
                e.Layout.Bands[0].Columns["OrigAllocatedUnits"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_OrigAllocatedUnits);
                e.Layout.Bands[0].Columns["RsvAllocatedUnits"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_RsvAllocatedUnits);

                e.Layout.Bands[0].Columns["HeaderID"].Width = 150;
                e.Layout.Bands[0].Columns["HdrGroupRID"].Width = 150;

                e.Layout.Bands[0].Columns["Type"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                e.Layout.Bands[0].Columns["SizeGroup"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                e.Layout.Bands[0].Columns["HdrGroupRID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;

                AssignValueLists(ugAssortments);
              
                FormatColumns(ugAssortments);

                CheckSortedColumn(e.Layout.Bands[0].SortedColumns, "HdrGroupRID", true);
                e.Layout.Bands[0].SortedColumns.Add("MultiSortSeq", false, false);
                CheckSortedColumn(e.Layout.Bands[0].SortedColumns, "AsrtRID", true);
                CheckSortedColumn(e.Layout.Bands[0].SortedColumns, "PlaceHolderRID", false);
                e.Layout.Bands[0].SortedColumns.Add("AsrtSortSeq", false, false);

				// Begin TT#1512-MD - stodd - ASSORTMENT Workspace-> double click and opens Selection screen instead of Matrix.  
                foreach (UltraGridRow row in ugAssortments.Rows)
                {
                    row.Activation = Activation.ActivateOnly;
                }

                e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
                // End TT#1512-MD - stodd - ASSORTMENT Workspace-> double click and opens Selection screen instead of Matrix.  

                _skipInitialize = true;		// TT#1404-MD - stodd - Assortment Workspace is unformated until a filter is chosen

            }
            catch
            {
                throw;
            }
        }

        private void CheckSortedColumn(SortedColumnsCollection sortedColumns, string colName, bool sortDescending)
        {
            try
            {
                if (!sortedColumns.Exists(colName))
                {
                    sortedColumns.Add(colName, sortDescending, false);
                }
                else if (!sortedColumns[colName].IsGroupByColumn)
                {
                    sortedColumns.Add(colName, sortDescending, false);
                }
            }
            catch
            {
                throw;
            }
        }

        private void ugAssortments_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.IsGroupByRow)
                {
                    LoopGroupByRow(e.Row);
                }
                else
                {
                    SetHeaderRowInfo(e.Row);
                }
            }
            catch
            {
                throw;
            }
        }

        private void LoopGroupByRow(UltraGridRow aGroupByRow)
        {
            try
            {
                UltraGridRow childRow = aGroupByRow.GetChild(ChildRow.First);
                while (childRow != null)
                {
                    if (childRow.IsGroupByRow)
                    {
                        LoopGroupByRow(childRow);
                    }
                    else
                    {
                        SetHeaderRowInfo(childRow);
                    }
                    childRow = childRow.GetSibling(SiblingRow.Next, false, false);
                }
            }
            catch
            {
                throw;
            }
        }

        private void SetHeaderRowInfo(UltraGridRow aRow)
        {
            try
            {
                aRow.Activation = Activation.ActivateOnly;
                SetNotesCellButton(aRow);
                CheckAssortHeaderHash(aRow);
                // Begin TT#1737 - RMatelic - Assortment Contents-Delete Placeholder, get Foreign Key violation .. unrelated to issue; change case of Cell name
                //DateRangeProfile dr = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(aRow.Cells["cdrRID"].Value, CultureInfo.CurrentUICulture));
                DateRangeProfile dr = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(aRow.Cells["CdrRID"].Value, CultureInfo.CurrentUICulture));
                // End TT#1737
                //aRow.Cells["DateRange"].Value = dr.DisplayDate;
                if (dr.DateRangeType == eCalendarRangeType.Dynamic)
                {
                    switch (dr.RelativeTo)
                    {
                        case eDateRangeRelativeTo.Current:
                            aRow.Cells["DateRange"].Appearance.Image = _dynamicToCurrentImage;
                            break;
                        case eDateRangeRelativeTo.Plan:
                            aRow.Cells["DateRange"].Appearance.Image = _dynamicToPlanImage;
                            break;
                        default:
                            aRow.Cells["DateRange"].Appearance.Image = null;
                            break;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void SetNotesCellButton(UltraGridRow aRow)
        {
            try
            {
                string notes = Convert.ToString(aRow.Cells["Notes"].Value, CultureInfo.CurrentUICulture);
                if (notes != null && notes.Trim() != string.Empty)
                {
                    aRow.Cells["HeaderID"].ButtonAppearance.Image = NotesImage;
                }
                else if (_inEditMode)
                {
                    if (Convert.ToInt32(aRow.Cells["Type"].Value, CultureInfo.CurrentUICulture) == (int)eHeaderType.Placeholder)
                    {
                        // change the cell style so the edit button doesn't show
                        aRow.Cells["HeaderID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                    }
                    else
                    {
                        aRow.Cells["HeaderID"].ButtonAppearance.Image = null;
                    }
                }
                else
                {   // change the cell style so the edit button doesn't show
                    aRow.Cells["HeaderID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                }
            }
            catch
            {
                throw;
            }
        }

        private void CheckAssortHeaderHash(UltraGridRow aRow)
        {
            int asrtRID, hdrRID;
            Hashtable asrtHeaderListHash;
            eHeaderType headerType;
            try
            {
                headerType = (eHeaderType)Convert.ToInt32(aRow.Cells["Type"].Value, CultureInfo.CurrentUICulture);
                hdrRID = Convert.ToInt32(aRow.Cells["KeyH"].Value, CultureInfo.CurrentUICulture);

                switch (headerType)
                {
                    case eHeaderType.Assortment:
                        if (!_assortmentGroups.ContainsKey(hdrRID))
                        {
                            asrtHeaderListHash = new Hashtable();
                            asrtHeaderListHash.Add(hdrRID, aRow);
                            _assortmentGroups.Add(hdrRID, asrtHeaderListHash);
                        }
                        else
                        {
                            asrtHeaderListHash = (Hashtable)_assortmentGroups[hdrRID];
                            UltraGridRow row = (UltraGridRow)asrtHeaderListHash[hdrRID];
                            if (row == null)
                            {
                                asrtHeaderListHash[hdrRID] = aRow;
                            }
                        }
                        break;

                    default: // the other headers
                        if (aRow.Cells["AsrtRID"].Value != DBNull.Value)
                        {
                            asrtRID = Convert.ToInt32(aRow.Cells["AsrtRID"].Value, CultureInfo.CurrentUICulture);
                            if (!_assortmentGroups.ContainsKey(asrtRID))
                            {
                                asrtHeaderListHash = new Hashtable();
                                asrtHeaderListHash.Add(asrtRID, null);
                                _assortmentGroups.Add(asrtRID, asrtHeaderListHash);
                            }
                            asrtHeaderListHash = (Hashtable)_assortmentGroups[asrtRID];

                            if (!asrtHeaderListHash.ContainsKey(hdrRID))
                            {
                                asrtHeaderListHash.Add(hdrRID, aRow);
                            }
                        }
                        break;
                }
            }
            catch
            {
                throw;
            }
        }

        private eAssortmentType GetAssortmentTypeForHeaderGrid(UltraGridRow aRow)
        {
            eAssortmentType asrtType = eAssortmentType.Undefined;
            try
            {
                int asrtRID = Convert.ToInt32(aRow.Cells["AsrtRID"].Value, CultureInfo.CurrentUICulture);
                DataRow dr = _dtHeader.Rows.Find(asrtRID);
                if (dr != null)
                {
                    asrtType = (eAssortmentType)dr["AsrtType"];
                }
            }
            catch
            {
                throw;
            }
            return asrtType;
        }

        private void ugAssortments_BeforeColumnChooserDisplayed(object sender, Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Dialog.Text = _lblColumnChooser;
                e.Dialog.ColumnChooserControl.ColumnDisplayOrder = ColumnDisplayOrder.SameAsGrid;
                e.Dialog.ColumnChooserControl.MultipleBandSupport = MultipleBandSupport.SingleBandOnly; // "Header" band only
                e.Dialog.ColumnChooserControl.Style = ColumnChooserStyle.AllColumnsWithCheckBoxes;      // this setting eliminates a "Header" caption at the top
                e.Dialog.ColumnChooserControl.ContextMenuStrip = cmsColumnChooser;
                e.Dialog.DisposeOnClose = DefaultableBoolean.True; // this causes the Chooser to match the grid order on subsequent opens

                UltraGrid grid = (UltraGrid)sender;
                grid.BeginInvoke(new PositionColumnChooserDialogDelegate(this.PositionColumnChooserDialog), new object[] { e.Dialog, grid });  
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public delegate void PositionColumnChooserDialogDelegate(ColumnChooserDialog dialog, UltraGrid grid);
        private void PositionColumnChooserDialog(ColumnChooserDialog dialog, UltraGrid grid)
        {
            try
            {
                dialog.Location = grid.PointToScreen(grid.Location);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugAssortments_BeforeSelectChange(object sender, BeforeSelectChangeEventArgs e)
        {
            try
            {
                if (typeof(UltraGridRow) == e.Type)
                {
                    if (ugAssortments.Selected.Rows.Count > 0)
                    {
                        foreach (UltraGridRow prevSelRow in ugAssortments.Selected.Rows)
                        {
                            if (!prevSelRow.IsGroupByRow && Convert.ToInt32(prevSelRow.Cells["Type"].Value, CultureInfo.CurrentUICulture) == (int)eHeaderType.Placeholder)
                            {
                                prevSelRow.Cells["HeaderID"].Appearance.ForeColor = prevSelRow.Band.Layout.Override.FixedCellAppearance.BackColor;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugAssortments_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
        {
            try
            {
                if (typeof(UltraGridRow) == e.Type || typeof(UltraGridGroupByRow) == e.Type)
                {
                    //if (_inEditMode)
                    //{
                    //    // disable event firing
                    //    ugAssortments.EventManager.SetEnabled(GridEventIds.AfterSelectChange, false);
                    //    if (typeof(UltraGridRow) == e.Type) //MID Track 4449, 4451, 4452
                    //    {
                    //        foreach (UltraGridRow selRow in ugAssortments.Selected.Rows)
                    //        {
                    //            if (selRow.Cells["HdrGroupRID"].Value != DBNull.Value)
                    //            {
                    //                int hdrGroupRID = Convert.ToInt32(selRow.Cells["HdrGroupRID"].Value, CultureInfo.CurrentUICulture);
                    //                if (_multiHeaderGroups.ContainsKey(hdrGroupRID))
                    //                {
                    //                    Hashtable multiHeaderListHash = (Hashtable)_multiHeaderGroups[hdrGroupRID];
                    //                    foreach (UltraGridRow row in multiHeaderListHash.Values)
                    //                    {
                    //                        row.Selected = true;
                    //                    }
                    //                }
                    //            }
                    //            else if (selRow.Cells["AsrtRID"].Value != DBNull.Value)
                    //            {
                    //                int asrtRID = Convert.ToInt32(selRow.Cells["AsrtRID"].Value, CultureInfo.CurrentUICulture);
                    //                if (_assortmentGroups.ContainsKey(asrtRID))
                    //                {
                    //                    Hashtable asrtHeaderListHash = (Hashtable)_assortmentGroups[asrtRID];
                    //                    foreach (UltraGridRow row in asrtHeaderListHash.Values)
                    //                    {
                    //                        row.Selected = true;
                    //                        if (Convert.ToInt32(row.Cells["Type"].Value, CultureInfo.CurrentUICulture) == (int)eHeaderType.Placeholder)
                    //                        {
                    //                            row.Cells["HeaderID"].SelectedAppearance.ForeColor = System.Drawing.SystemColors.Highlight;
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    // re-enable event firing
                    //    ugAssortments.EventManager.SetEnabled(GridEventIds.AfterSelectChange, true);
                    //    return;
                    //}

                    _selectedRowsSequence.Clear();
                    if (ugAssortments.Selected.Rows.Count > 0)
                    {
                        if (typeof(UltraGridRow) == e.Type)
                        {
                            foreach (UltraGridRow row in ugAssortments.Selected.Rows)
                            {
                                _selectedRowsSequence.Add(row);
                            }
                        }
                        else
                        {
                            ArrayList selrows = new ArrayList();
                            foreach (UltraGridRow row in ugAssortments.Selected.Rows)
                            {
                                selrows.Add(row);
                            }
                        
                            ugAssortments.EventManager.SetEnabled(GridEventIds.AfterSelectChange, false);
                            foreach (UltraGridRow row in selrows)
                            {
                                row.Selected = true;
                                GetRowsFromGroupByRow(row);
                            }    
                            ugAssortments.EventManager.SetEnabled(GridEventIds.AfterSelectChange, true);
                        }
                    }
                    UpdateSelectedTotals();
                    SetMenuItems();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally  //MID Track 4449, 4451, 4452 above 'return' statement left the screen suspended
            {
                //ugDetails.ResumeRowSynchronization();
                //ugDetails.EndUpdate();
            }
        }

        private void SetMenuItems()
        {
            try
            {
                //if (this.ugAssortments.Selected.Rows.Count > 0)
                //{
                //    cmsDelete.Enabled = true;
                //    _EAB.Explorer.EnableMenuItem(eMIDMenuItem.EditDelete);
                //    foreach (UltraGridRow row in ugAssortments.Selected.Rows)
                //    {
                //        if (row.IsGroupByRow)
                //        {
                //            cmsDelete.Enabled = false;
                //            _EAB.Explorer.DisableMenuItem(eMIDMenuItem.EditDelete);
                //            break;
                //        }
                //        else
                //        {
                //            eHeaderAllocationStatus headerStatus = (eHeaderAllocationStatus)Convert.ToInt32(row.Cells["Status"].Value, CultureInfo.CurrentUICulture);
                //            if (headerStatus != eHeaderAllocationStatus.ReceivedInBalance
                //             && headerStatus != eHeaderAllocationStatus.ReceivedOutOfBalance)
                //            {
                //                cmsDelete.Enabled = false;
                //                _EAB.Explorer.DisableMenuItem(eMIDMenuItem.EditDelete);
                //                break;
                //            }
                //            else if (!_assortmentWorkspaceSecurity.AllowDelete)
                //            {
                //                cmsDelete.Enabled = false;
                //                _EAB.Explorer.DisableMenuItem(eMIDMenuItem.EditDelete);
                //                break;
                //            }
                //        }
                //    }
                //}
            }
            catch
            {
                throw;
            }
        }
    
        private void GetRowsFromGroupByRow(UltraGridRow aGroupByRow)
        {
            try
            {
                UltraGridRow childRow = aGroupByRow.GetChild(ChildRow.First);
                while (childRow != null)
                {
                    if (childRow.IsGroupByRow)
                    {
                        GetRowsFromGroupByRow(childRow);
                    }
                    else
                    {
                        _selectedRowsSequence.Add(childRow);
                    }
                    childRow = childRow.GetSibling(SiblingRow.Next, false, false);
                }
            }
            catch
            {
                throw;
            }
        }

        private void UpdateSelectedTotals()
        {
            int selHeaders = 0, selUnits = 0, totalUnits = 0;
            string labelText = string.Empty;

            _selectedHeaderKeyList.Clear();
            _selectedAssortmentKeyList.Clear(); // TT#488 - MD - Jellis - Group Allocation
            _SAB.ClientServerSession.ClearSelectedHeaderList();

            try
            {
                if (ugAssortments.Selected.Rows.Count > 0)
                {
                    foreach (UltraGridRow row in ugAssortments.Selected.Rows)
                    {
                        UltraGridRow selRow = row;
                        
                        if (selRow.IsGroupByRow)
                        {
                            LoopGroupByRowForTotals(selRow, ref selHeaders, ref selUnits);
                        }
                        else
                        {
                            AccumulateSelectedRowTotals(selRow, ref selHeaders, ref selUnits);
                        }
                    }
                }

				// BEGIN TT#765-MD - Stodd - Add toolbars to Assortment Workspace
                //this.lblHeaderCount.Text = _lblSelected + ": " + selHeaders.ToString("#,###,##0", CultureInfo.CurrentUICulture);
                labelText = _lblTotal + ":      ";
                //this.lblHeaderTotal.Text = labelText + " " + _dtHeader.Rows.Count.ToString("#,###,##0", CultureInfo.CurrentUICulture);
				((Infragistics.Win.UltraWinToolbars.TextBoxTool)this.headerToolbarsManager.Tools["headerSelectedTextBox"]).Text = selHeaders.ToString("#,###,##0", CultureInfo.CurrentUICulture);
				((Infragistics.Win.UltraWinToolbars.TextBoxTool)this.headerToolbarsManager.Tools["headerTotalTextBox"]).Text = _dtHeader.Rows.Count.ToString("#,###,##0", CultureInfo.CurrentUICulture);
				// END TT#765-MD - Stodd - Add toolbars to Assortment Workspace

                if (_dtHeader.Rows.Count > 0)
                {
                    foreach (DataRow row in _dtHeader.Rows)
                    {
                        if (row["HdrQuantity"] != System.DBNull.Value)
                        {
                            totalUnits += Convert.ToInt32(row["HdrQuantity"], CultureInfo.CurrentUICulture);
                        }
                    }
                }

				// BEGIN TT#765-MD - Stodd - Add toolbars to Assortment Workspace
                //this.lblSelQty.Text = _lblSelected + ": " + selUnits.ToString("#,###,##0", CultureInfo.CurrentUICulture);
                //this.lblTotQty.Text = labelText + " " + totalUnits.ToString("#,###,##0", CultureInfo.CurrentUICulture);
				((Infragistics.Win.UltraWinToolbars.TextBoxTool)this.headerToolbarsManager.Tools["quantityAllocateSelectedTextBox"]).Text = selUnits.ToString("#,###,##0", CultureInfo.CurrentUICulture);
				((Infragistics.Win.UltraWinToolbars.TextBoxTool)this.headerToolbarsManager.Tools["quantityAllocateTotalTextBox"]).Text = totalUnits.ToString("#,###,##0", CultureInfo.CurrentUICulture);
				// END TT#765-MD - Stodd - Add toolbars to Assortment Workspace
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void LoopGroupByRowForTotals(UltraGridRow aGroupByRow, ref int aSelHeaders, ref int aSelUnits)
        {
            try
            {
                UltraGridRow childRow = aGroupByRow.GetChild(ChildRow.First);
                while (childRow != null)
                {
                    if (childRow.IsGroupByRow)
                    {
                        LoopGroupByRowForTotals(childRow, ref aSelHeaders, ref aSelUnits);
                    }
                    else
                    {
                        AccumulateSelectedRowTotals(childRow, ref aSelHeaders, ref aSelUnits);
                    }
                    childRow = childRow.GetSibling(SiblingRow.Next, false, false);
                }
            }
            catch
            {
                throw;
            }
        }

        private void AccumulateSelectedRowTotals(UltraGridRow aSelRow, ref int aSelHeaders, ref int aSelUnits)
        {
            try
            {
                int hdrRID = Convert.ToInt32(aSelRow.Cells["KeyH"].Value, CultureInfo.CurrentUICulture);
                if (!_selectedHeaderKeyList.Contains(hdrRID) // TT#488 - MD - Jellis - Group Allocation
                    && !_selectedAssortmentKeyList.Contains(hdrRID)) // TT#488 - MD - Jellis - Group Allocation
                {
                    string headerID = Convert.ToString(aSelRow.Cells["HeaderID"].Value, CultureInfo.CurrentUICulture);

                    eHeaderType headerType = (eHeaderType)Convert.ToInt32(aSelRow.Cells["Type"].Value, CultureInfo.CurrentUICulture);
                    //_selectedHeaderKeyList.Add(hdrRID);  // TT#488 - MD - Jellis - Group Allocation
                    int asrtRID;
                    if (aSelRow.Cells["AsrtRID"].Value == DBNull.Value)
                    {
                        asrtRID = Include.NoRID;
                    }
                    else
                    {
                        asrtRID = Convert.ToInt32(aSelRow.Cells["AsrtRID"].Value, CultureInfo.CurrentUICulture);
                    }
                    if (hdrRID > Include.DefaultHeaderRID)
                    {
                        int styleHnRID = Convert.ToInt32(aSelRow.Cells["StyleHnRID"].Value, CultureInfo.CurrentUICulture);
                        _SAB.ClientServerSession.AddSelectedHeaderList(hdrRID, headerID, headerType, asrtRID, styleHnRID);
                    }
                    // begin TT#488 - MD - Jellis - Group Allocation
                    if (headerType == eHeaderType.Assortment)
                    {
                        _selectedAssortmentKeyList.Add(hdrRID);
                    }
                    else
                    {
                        _selectedHeaderKeyList.Add(hdrRID);
                    }
                    // end TT3488 - MD - Jellis - Group Allocation

                    aSelHeaders++;
                    if (aSelRow.Cells["HdrQuantity"].Value != System.DBNull.Value)
                    {
                        aSelUnits += Convert.ToInt32(aSelRow.Cells["HdrQuantity"].Value, CultureInfo.CurrentUICulture);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void ugAssortments_BeforeSortChange(object sender, BeforeSortChangeEventArgs e)
        {
            try
            {
                if (_inEditMode)       
                {
                    e.Cancel = true;
                }
                else                 
                {
                    _headersInGroupBy = new ArrayList();
                   
                    foreach (UltraGridRow row in ugAssortments.Selected.Rows)
                    {
                        if (!row.IsGroupByRow)
                        {
                            if (!_headersInGroupBy.Contains(row))
                            {
                                _headersInGroupBy.Add(row);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugAssortments_AfterSortChange(object sender, Infragistics.Win.UltraWinGrid.BandEventArgs e)
        {
            UltraGrid grid = (UltraGrid)sender;
            grid.EventManager.SetEnabled(GridEventIds.AfterSortChange, false);
            try
            {
                grid.EventManager.SetEnabled(GridEventIds.AfterSelectChange, false);
                //UltraGridRow row = ugAssortments.GetRow(ChildRow.First);
                //if (row.IsGroupByRow)
                //{
                //    this.ugAssortments.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.None;
                //}
                //else
                //{
                //    this.ugAssortments.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                //}
                grid.EventManager.SetEnabled(GridEventIds.AfterSelectChange, true);
                for (int i = 0; i < e.Band.SortedColumns.Count; i++)
                {
                    UltraGridColumn sortColumn = e.Band.SortedColumns[i];

                    if (sortColumn.Key == "HdrGroupRID")
                    {
                        if (!e.Band.SortedColumns.Exists("MultiSortSeq"))
                        {
                            grid.EventManager.SetEnabled(GridEventIds.BeforeSortChange, false);
                            e.Band.SortedColumns.Add("MultiSortSeq", false);
                            grid.EventManager.SetEnabled(GridEventIds.BeforeSortChange, true);
                        }
                        break;
                    }
                }

                // after sort, reset active row to keep in view
                if (ugAssortments.ActiveCell != null)
                {
                    ugAssortments.ActiveRow = ugAssortments.ActiveCell.Row; // MID Track #5656 - null error after sort
                }
                else if (_headersInGroupBy != null && _headersInGroupBy.Count > 0)
                {
                    ugAssortments.Selected.Rows.Clear();
                    ReselectRows();
                }
                else
                {
                    ugAssortments.Selected.Rows.Clear();
                    UpdateSelectedTotals();
                    ugAssortments.ActiveRow = null;
                }
                _headersInGroupBy.Clear();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                grid.EventManager.SetEnabled(GridEventIds.AfterSortChange, true);
            }
        }

        private void ReselectRows()
        {
            try
            {
                for (int i = 0; i < _headersInGroupBy.Count; i++)
                {
                    UltraGridRow row = (UltraGridRow)_headersInGroupBy[i];
                    row.Activate();
                    row.Selected = true;
                }
            }
            catch
            {
                throw;
            }
        }

        private void ugAssortments_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            string title = _lblHeaderNotes + " ";
            try
            {
                switch (e.Cell.Column.Key)
                {
                    case "HeaderID":

                        title = title + e.Cell.Text;
                        TextDialog frm = new TextDialog(title, e.Cell.Row.Cells["Notes"].Text);
                        frm.ShowDialog();
                        frm.Dispose();
                        break;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugAssortments_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
			// Begin TT#1512-MD - stodd - ASSORTMENT Workspace-> double click and opens Selection screen instead of Matrix.  
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                EventArgs args = new EventArgs();
                cmsReviewAssortment_Click(cmsReviewAssortment, args);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
			// End TT#1512-MD - stodd - ASSORTMENT Workspace-> double click and opens Selection screen instead of Matrix.  
        }

        private void AssignValueLists(UltraGrid aGrid)
        {
            try
            {
                aGrid.DisplayLayout.ResetValueLists();

                // Assortment
                aGrid.DisplayLayout.ValueLists.Add(_assortmentValueList);
                aGrid.DisplayLayout.Bands[0].Columns["AsrtRID"].ValueList = _assortmentValueList;

                // PlaceHolder
                aGrid.DisplayLayout.ValueLists.Add(_placeHolderValueList);
                aGrid.DisplayLayout.Bands[0].Columns["PlaceHolderRID"].ValueList = _placeHolderValueList;

                // Header Type 
                aGrid.DisplayLayout.ValueLists.Add(_headerTypeValueList);
                aGrid.DisplayLayout.Bands[0].Columns["Type"].ValueList = _headerTypeValueList;

                // Header Status
                aGrid.DisplayLayout.ValueLists.Add(_headerStatusValueList);
                aGrid.DisplayLayout.Bands[0].Columns["Status"].ValueList = _headerStatusValueList;

                // Size Group
                if (_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
                {
                    aGrid.DisplayLayout.ValueLists.Add(_sizeGroupValueList);
                    aGrid.DisplayLayout.Bands[0].Columns["SizeGroup"].ValueList = _sizeGroupValueList;
                }

                // Header Intransit
                // START TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace
                //aGrid.DisplayLayout.ValueLists.Add(_headerIntransitValueList);
                //aGrid.DisplayLayout.Bands[0].Columns["Intransit"].ValueList = _headerIntransitValueList;
                // END TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace

                // Header Ship Status
                // START TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace
                //aGrid.DisplayLayout.ValueLists.Add(_headerShipStatusValueList);
                //aGrid.DisplayLayout.Bands[0].Columns["ShipStatus"].ValueList = _headerShipStatusValueList;
                // END TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace

                // Header Characteristics
                //if (_charValueListsHash.Count > 0)
                //{
                //    foreach (string groupID in _charValueListsHash.Keys)
                //    {
                //        ValueList valueList = (ValueList)_charValueListsHash[groupID];
                //        aGrid.DisplayLayout.ValueLists.Add(valueList);
                //        aGrid.DisplayLayout.Bands[0].Columns[groupID].ValueList = valueList;
                //        aGrid.DisplayLayout.Bands[0].Columns[groupID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                //    }
                //}

                //foreach (HeaderCharGroupProfile hcgp in _headerCharGroupProfileList)
                //{
                //    aGrid.DisplayLayout.Bands[0].Columns[hcgp.ID].Tag = hcgp;
                //}
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void FormatColumns(Infragistics.Win.UltraWinGrid.UltraGrid ultragrid)
        {
            try
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridBand band in ultragrid.DisplayLayout.Bands)
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in band.Columns)
                    {
                        switch (column.DataType.ToString())
                        {
                            case "System.Int32":
                                column.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                                column.Format = "#,###,##0";
                                break;
                            case "System.Double":
                                column.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                                column.Format = "#,###,###.00";
                                break;
                        }
                        if (!column.Hidden)
                        {
                            column.PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
                            column.CellActivation = Activation.NoEdit;	// TT#1512-MD - stodd - ASSORTMENT Workspace-> double click and opens Selection screen instead of Matrix.  
                        }
                    }

                    switch (band.Key)
                    {
                        case "Header":
                            //case "Placeholder":
                            band.Columns["HdrGroupRID"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
                            band.Columns["AsrtRID"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
                            band.Columns["PlaceHolderRID"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
                            band.Columns["Type"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
                            band.Columns["Status"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
                            band.Columns["SizeGroup"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
                            // START TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace
                            //band.Columns["Intransit"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left; 
                            //band.Columns["ShipStatus"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
                            // END TT#2029-MD - AGallagher - Characteristcs do not appear in the Allocation Workspace
                            //foreach (HeaderCharGroupProfile hcgp in _headerCharGroupProfileList)
                            //{
                            //    band.Columns[hcgp.ID].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
                            //}
                            break;

                        //case "Pack":
                        //    band.Columns["PackType"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
                        //    break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugAssortments_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                UltraGridCell mouseCell;
                Infragistics.Win.UIElement mouseUIElement;
                Infragistics.Win.UIElement headerUIElement;
                HeaderUIElement headerUI = null;
                Point point = new Point(e.X, e.Y);

                mouseUIElement = ugAssortments.DisplayLayout.UIElement.ElementFromPoint(point);
          
                if (mouseUIElement == null)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    // retrieve the UIElement from the location of the mouse 

                    headerUIElement = mouseUIElement.GetAncestor(typeof(HeaderUIElement));
                    if (null == headerUIElement)
                    {
                        // retrieve the Cell from the UIElement 
                        mouseCell = (Infragistics.Win.UltraWinGrid.UltraGridCell)mouseUIElement.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell));

                        // if there is a cell object reference, set to active cell and edit
                        if (mouseCell != null)
                        {
                            _gridCol = mouseCell.Column;
                            _gridBand = mouseCell.Band;
                        }
                    }
                    else if (headerUIElement.GetType() == typeof(HeaderUIElement))
                    {
                        headerUI = (HeaderUIElement)headerUIElement;
                        Infragistics.Win.UltraWinGrid.ColumnHeader colHeader = null;
                        _gridCol = null;
                        colHeader = (Infragistics.Win.UltraWinGrid.ColumnHeader)headerUI.SelectableItem;
                        _gridCol = colHeader.Column;
                        if (_gridCol == null)
                        {
                            return;
                        }
                        _gridBand = colHeader.Band;
                    }
                }
                _rClickRow = (UltraGridRow)mouseUIElement.GetContext(typeof(UltraGridRow));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugAssortments_SelectionDrag(object sender, CancelEventArgs e)
        {
            try
            {
                if (_inEditMode && ugAssortments.Selected.Rows.Count > 0 && !ugAssortments.Selected.Rows[0].IsGroupByRow)
                {
                    int xPos, yPos;
                    int imageHeight, imageWidth, Indent = 0, _spacing = 2;

                    string hdrIDs = string.Empty;
                    foreach (UltraGridRow selRow in ugAssortments.Selected.Rows)
                    {
                        if (hdrIDs == string.Empty)
                        {
                            hdrIDs = selRow.Cells["HeaderID"].Value.ToString();
                        }
                        else
                        {
                            hdrIDs += ", " + selRow.Cells["HeaderID"].Value.ToString();
                        }
                    }

                    MIDGraphics.BuildDragImage(hdrIDs, imageListDrag, Indent, _spacing,
                               Font, ForeColor, out imageHeight, out imageWidth);

                    xPos = imageWidth / 2;
                    yPos = imageHeight / 2;

                    if (DragHelper.ImageList_BeginDrag(this.imageListDrag.Handle, 0, xPos, yPos))
                    {
                        DragDropEffects dde = ugAssortments.DoDragDrop(ugAssortments.Selected.Rows, DragDropEffects.Move);

                        if (dde == DragDropEffects.None)
                        {   // for some reason the detail grid is in a semi-locked state if no row is dropped
                            // so the EndUpdate() seems to remedy that behavior
                            //ugDetails.EndUpdate();
                        }
                        DragHelper.ImageList_EndDrag();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugAssortments_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        private void ugAssortments_DragEnter(object sender, DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void ugAssortments_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            e.DisplayPromptMsg = false;
            string message;
            try
            {
                if (_deleteKeyPressed)
                {
                    _deleteKeyPressed = false;
                    if (!_allowDeleteKey)
                    {
                        e.Cancel = true;
                    }
                }

                message = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_DeleteRows), ugAssortments.Selected.Rows.Count.ToString());
                message += Environment.NewLine + MIDText.GetTextOnly((int)eMIDTextCode.msg_ContinueQuestion);

                DialogResult diagResult = MessageBox.Show(message, _lblDeleteRow, System.Windows.Forms.MessageBoxButtons.YesNo,
                    System.Windows.Forms.MessageBoxIcon.Question);

                if (diagResult == System.Windows.Forms.DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                
                if (!HeadersEnqueued())
                {
                    return;
                }

                //TODO: Database updates - probably thru AssortmentProfile
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugAssortments_AfterRowsDeleted(object sender, EventArgs e)
        {
            try
            {
                DequeueHeaders();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugAssortments_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Delete)
                {
                    _deleteKeyPressed = true;
                    _allowDeleteKey = cmsDelete.Enabled ? true : false;
                }
            }
            catch
            {
                throw;
            }
        }
         
        private void ugAssortments_DragLeave(object sender, EventArgs e)
        {
            Image_DragLeave(sender, e);
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void panel1_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        private void panel1_DragLeave(object sender, EventArgs e)
        {
            Image_DragLeave(sender, e);
        }

        #endregion

        #region Apply and Save View methods

		private void SetViewComboEnabled()
		{
			try
			{
				// Begin TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox - 
                Infragistics.Win.UltraWinToolbars.ControlContainerTool cct = (Infragistics.Win.UltraWinToolbars.ControlContainerTool)this.headerToolbarsManager.Tools["ControlContainerAsrtView"];	// TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns
                MIDComboBoxEnh.MyComboBox cmbView = (MIDComboBoxEnh.MyComboBox)cct.Control;
				// Begin TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox - 
				if (_inEditMode)
				{
					// BEGIN TT#765-MD - Stodd - Add toolbars to Assortment Workspace
					//cboView.Enabled = false;
					// End TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox - 
					//((Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.headerToolbarsManager.Tools["viewComboBox"]).SharedProps.Enabled = false;
					// END TT#765-MD - Stodd - Add toolbars to Assortment Workspace
                    cct.SharedProps.Enabled = false;
                    cmbView.Enabled = false;
					// End TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox - 
				}
				else
				{
					// BEGIN TT#765-MD - Stodd - Add toolbars to Assortment Workspace
					//cboView.Enabled = _assortmentWorkspaceViewsSecurity.AccessDenied ? false : true;
					// Begin TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox - 
					//((Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.headerToolbarsManager.Tools["viewComboBox"]).SharedProps.Enabled = _assortmentWorkspaceViewsSecurity.AccessDenied ? false : true;
					// END TT#765-MD - Stodd - Add toolbars to Assortment Workspace
                    // Begin TT#2014-MD - JSmith - Assortment Security
                    //cct.SharedProps.Enabled = _assortmentWorkspaceViewsSecurity.AccessDenied ? false : true; ;
                    //cmbView.Enabled = _assortmentWorkspaceViewsSecurity.AccessDenied ? false : true; ;
                     if ( _assortmentWorkspaceViewsGlobalSecurity.AccessDenied && _assortmentWorkspaceViewsUserSecurity.AccessDenied)
                     {
                         cct.SharedProps.Enabled = false;
                         cmbView.Enabled = false;
                     }
                     else
                     {
                         cct.SharedProps.Enabled = true;
                         cmbView.Enabled = true;
                     }
                    // End TT#2014-MD - JSmith - Assortment Security
					// End TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox - 
				}
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
		//private void SetUserView(int aViewRID)
		private void SetUserView(int aViewRID, bool useFilterSorting)
		// End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
		{
			try
			{
				if (aViewRID != Include.NoRID)
				{
					// BEGIN TT#765-MD - Stodd - Add toolbars to Assortment Workspace
					//cboView.SelectedValue = aViewRID;
                    // Begin TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox
					//((Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.headerToolbarsManager.Tools["viewComboBox"]).Value = aViewRID;
                    Infragistics.Win.UltraWinToolbars.ControlContainerTool cct = (Infragistics.Win.UltraWinToolbars.ControlContainerTool)this.headerToolbarsManager.Tools["ControlContainerAsrtView"];	// TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns
                    MIDComboBoxEnh.MyComboBox cmbView = (MIDComboBoxEnh.MyComboBox)cct.Control;
                    cmbView.SelectedValue = aViewRID;
                    // End TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox
					// END TT#765-MD - Stodd - Add toolbars to Assortment Workspace
                    // Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
					//ApplyViewToGridLayout(aViewRID);
					ApplyViewToGridLayout(aViewRID, useFilterSorting);
					// End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
				}
			}
			catch
			{
				throw;
			}
		}

		// BEGIN TT#765-MD - Stodd - Add toolbars to Assortment Workspace
		//private void cboView_MouseHover(object sender, EventArgs e)
		//{
		//    toolTip1.Active = true;
		//}

		//private void cboView_SelectionChangeCommitted(object sender, EventArgs e)
		//{
		//    try
		//    {
		//        int viewRID;
		//        if (!_bindingView && !_fromLoadEvent)
		//        {
		//            viewRID = Convert.ToInt32(cboView.SelectedValue, CultureInfo.CurrentUICulture);
		//            if (viewRID != _lastSelectedViewRID)
		//            {
		//                if (_gridViewData.GridViewFilterExists(viewRID)) // apply View filter to User Workspace FIlter 
		//                {
		//                    IRefresh();
		//                }
		//                else
		//                {
		//                    ApplyViewToGridLayout(viewRID);
		//                }
		//            }
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        HandleException(ex);
		//    }
		//}

        //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
		//void cboView_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
		//{
		//    this.cboView_SelectionChangeCommitted(source, new EventArgs());
		//}
        //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
		// END TT#765-MD - Stodd - Add toolbars to Assortment Workspace

        // Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
		//private void ApplyViewToGridLayout(int aViewRID)
		private void ApplyViewToGridLayout(int aViewRID, bool useFilterSorting)
		// End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
        {
            try
            {
                int visiblePosition, sortSequence, width;
                string bandKey, colKey, errMessage;
                bool isHidden, isGroupByCol;
                eSortDirection sortDirection;

                _lastSelectedViewRID = aViewRID;

                if (aViewRID == Include.NoRID)    // don't modify current grid appearance 
                {
                    return;
                }

                DataTable dtGridViewDetail = _gridViewData.GridViewDetail_Read(aViewRID);

                if (dtGridViewDetail == null || dtGridViewDetail.Rows.Count == 0)
                {
                    errMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_as_GridViewDoesNotExist);
                    MessageBox.Show(errMessage);
                    _lastSelectedViewRID = Include.NoRID;
                    BindViewCombo();
                    return;
                }

                SortedList sortedColumns = new SortedList();

                ugAssortments.ResetLayouts();
                ApplyAppearance(ugAssortments);
                ugAssortments.DisplayLayout.ClearGroupByColumns();
                foreach (UltraGridBand band in ugAssortments.DisplayLayout.Bands)
                {
                    band.SortedColumns.Clear();
                }

                foreach (DataRow row in dtGridViewDetail.Rows)
                {
                    bandKey = Convert.ToString(row["BAND_KEY"], CultureInfo.CurrentUICulture);
                    colKey = Convert.ToString(row["COLUMN_KEY"], CultureInfo.CurrentUICulture);
                    visiblePosition = Convert.ToInt32(row["VISIBLE_POSITION"], CultureInfo.CurrentUICulture);
                    isHidden = Include.ConvertCharToBool(Convert.ToChar(row["IS_HIDDEN"], CultureInfo.CurrentUICulture));
                    isGroupByCol = Include.ConvertCharToBool(Convert.ToChar(row["IS_GROUPBY_COL"], CultureInfo.CurrentUICulture));
                    sortDirection = (eSortDirection)Convert.ToInt32(row["SORT_DIRECTION"], CultureInfo.CurrentUICulture);
                    if (row["WIDTH"] != DBNull.Value)
                    {
                        width = Convert.ToInt32(row["WIDTH"], CultureInfo.CurrentUICulture);
                    }
                    else
                    {
                        width = -1;
                    }

                    // Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
					if (useFilterSorting)
                    {
                        sortSequence = -1;
                    }
                    else
                    {
					// End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
                        if (sortDirection == eSortDirection.Ascending || sortDirection == eSortDirection.Descending)
                        {
                            sortSequence = Convert.ToInt32(row["SORT_SEQUENCE"], CultureInfo.CurrentUICulture);
                            if (!sortedColumns.ContainsKey(sortSequence))
                            {
                                sortedColumns.Add(sortSequence, row);
                            }
                        }
                        else
                        {
                            sortSequence = -1;
                        }
                    }   // TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only

                    if (ugAssortments.DisplayLayout.Bands.Exists(bandKey))
                    {
                        UltraGridBand band = ugAssortments.DisplayLayout.Bands[bandKey];

                        if (band.Columns.Exists(colKey))
                        {
                            UltraGridColumn column = band.Columns[colKey];
                            column.Header.VisiblePosition = visiblePosition;
                            column.Hidden = isHidden;
                            if (width != -1)
                            {
                                column.Width = width;
                            }
                        }
                    }
                }
                if (sortedColumns.Count > 0)
                {
                    for (int i = 0; i < sortedColumns.Count; i++)
                    {
                        bool sortDescending;
                        DataRow sRow = (DataRow)sortedColumns[i];
                        bandKey = Convert.ToString(sRow["BAND_KEY"], CultureInfo.CurrentUICulture);
                        colKey = Convert.ToString(sRow["COLUMN_KEY"], CultureInfo.CurrentUICulture);
                        isGroupByCol = Include.ConvertCharToBool(Convert.ToChar(sRow["IS_GROUPBY_COL"], CultureInfo.CurrentUICulture));
                        sortDirection = (eSortDirection)Convert.ToInt32(sRow["SORT_DIRECTION"], CultureInfo.CurrentUICulture);
                        switch (sortDirection)
                        {
                            case eSortDirection.Descending:
                                sortDescending = true;
                                break;
                            default:
                                sortDescending = false;
                                break;
                        }

                        if (ugAssortments.DisplayLayout.Bands.Exists(bandKey))
                        {
                            UltraGridBand band = ugAssortments.DisplayLayout.Bands[bandKey];

                            if (!band.SortedColumns.Exists(colKey))
                            {
                                band.SortedColumns.Add(colKey, sortDescending, isGroupByCol);
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

        #endregion

        #region Display Context Menu Options
        private void cmsGrid_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                cmsCopy.Visible = false;
                cmsDelete.Visible = false;
                _EAB.Explorer.DisableMenuItem(eMIDMenuItem.EditCopy);
                _EAB.Explorer.DisableMenuItem(eMIDMenuItem.EditDelete);
                if (this.ugAssortments.Selected.Rows.Count == 1)
                {
                    cmsReviewProperties.Enabled = true;
                    cmsReviewAssortment.Enabled = true;
                }
                else
                {
                    cmsReviewProperties.Enabled = false;
                    cmsReviewAssortment.Enabled = false;
                }
               
                // Begin TT#2014-MD - JSmith - Assortment Security
                //cmsSaveView.Enabled = _assortmentWorkspaceViewsSecurity.AllowUpdate ? true : false;
                if (_assortmentWorkspaceViewsGlobalSecurity.AllowUpdate || _assortmentWorkspaceViewsUserSecurity.AllowUpdate)
                {
                    cmsSaveView.Enabled = true;
                }
                else
                {
                    cmsSaveView.Enabled = false;
                }
                // End TT#2014-MD - JSmith - Assortment Security
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        #endregion

        #region ContextMenu events and methods
        private void cmsReviewSelect_Click(object sender, EventArgs e)
        {
            DetermineWindow(eAllocationSelectionViewType.None);
        }

        private void cmsReviewProperties_Click(object sender, EventArgs e)
        {
			ReviewProperties();	// TT#765-MD - Stodd - Add toolbars to Assortment Workspace
        }

		private void ReviewProperties()		// TT#765-MD - Stodd - Add toolbars to Assortment Workspace
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            //if (_selectedHeaderKeyList.Count == 0)
            //{
            //    return;
            //}
            //int hdrRID = (int)_selectedHeaderKeyList[0]; 
            int hdrRID; 
            if (_selectedAssortmentKeyList.Count == 0)
            {
                return;
            }
            else
            {
                hdrRID = (int)_selectedAssortmentKeyList[0]; 
            }
            // end TT#488 - MD - Jellis - Group Allocation
			ApplicationSessionTransaction appTransaction = SAB.ApplicationServerSession.CreateTransaction();
			AssortmentProfile ap = new AssortmentProfile(appTransaction, null, hdrRID, _SAB.ClientServerSession);
			frmAssortmentProperties assortmentProperties = new frmAssortmentProperties(SAB, _EAB, null, ap, false);
			//closeHandler = new OnAssortmentPropertiesCloseClass(null);
			//assortmentProperties.OnAssortmentPropertiesChangeHandler += new frmAssortmentProperties.AssortmentPropertiesChangeEventHandler(OnAssortmentPropertiesChange);
			//assortmentProperties.OnAssortmentPropertiesCloseHandler += new frmAssortmentProperties.AssortmentPropertiesCloseEventHandler(closeHandler.OnClose);
			if (this.ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer")
			{
				assortmentProperties.MdiParent = this.ParentForm;
			}
			else
			{
				assortmentProperties.MdiParent = this.ParentForm.Owner;
			}

			assortmentProperties.Show();
		}

        private void cmsReviewAssortment_Click(object sender, EventArgs e)
        {
            // BEGIN TT#2116-MD - AGallagher - Assortment Review Navigation
            if (_selectedAssortmentKeyList.Count == 1)
            {
                ReviewProperties();
            }
            else
            {
            // END TT#2116-MD - AGallagher - Assortment Review Navigation
                DetermineWindow(eAllocationSelectionViewType.Assortment);
            }  // TT#2116-MD - AGallagher - Assortment Review Navigation
        }

        public void DetermineWindow(eAllocationSelectionViewType ViewType)
        {
            try
            {
               
                // begin TT#488 - MD - Jellis - Group Allocation
                if (_selectedAssortmentKeyList.Count > 0)
                {
                    foreach (int key in _selectedAssortmentKeyList)
                    {
                        if (key < 1)
                        {
                            MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HeadersChanged));
                            return;
                        }
                    }
                }
                // end TT#488 - MD - Jellis - Group Allocation
                if (_selectedHeaderKeyList.Count > 0)
                {
                    foreach (int key in _selectedHeaderKeyList)
                    {
                        if (key < 1)
                        {
                            MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HeadersChanged));
                            return;
                        }
                    }
                }

				// Begin TT#1212-MD - stodd - double-clicked on the assortment workspace to open an existing assortment receive an Argument Exception - 
                //ApplicationSessionTransaction processTransaction = NewTransFromSelectedHeaders(ViewType);	// TT#764-MD - Stodd - change Assortment Workspace to use AssortmentMember
                ApplicationSessionTransaction processTransaction = new ApplicationSessionTransaction(SAB);
                ArrayList asrtKeyList = new ArrayList();
                foreach (int key in _selectedAssortmentKeyList)
                {
                    asrtKeyList.Add(key);
                }
                AddSelectedHeadersToTrans(asrtKeyList, processTransaction, ref _selectedAssortmentKeyList, ref _selectedHeaderKeyList);

                //if (processTransaction == null)
                //{
                //    return;
                //}
				// End TT#1212-MD - stodd - double-clicked on the assortment workspace to open an existing assortment receive an Argument Exception - 
              
                AssortmentViewSelection avs = new AssortmentViewSelection(EAB, SAB, processTransaction, null, false);
                if (this.ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer")
                {
                    avs.MdiParent = this.ParentForm;
                }
                else
                {
                    avs.MdiParent = this.ParentForm.Owner;
                }
				avs.DetermineWindow(ViewType);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// Return a new transaction which is created on the application server and load the selected headers
        /// into its allocation profile list
        /// </summary>
        /// <returns></returns>
		private ApplicationSessionTransaction NewTransFromSelectedHeaders(eAllocationSelectionViewType viewType)	// TT#764-MD - Stodd - change Assortment Workspace to use AssortmentMember
        {
            try
            {
                ApplicationSessionTransaction newTrans = _SAB.ApplicationServerSession.CreateTransaction();
               
                if (_selectedRowsSequence.Count > 0)    
                {
                    newTrans.NewAllocationMasterProfileList();

                    BuildSelectedHeaderKeyList();
       
                    int[] selectedHeaderArray = new int[_selectedHeaderKeyList.Count];
                    _selectedHeaderKeyList.CopyTo(selectedHeaderArray);
                    // begin TT#488 - MD - Jellis - Group Allocation
                    int[] selectedAssortmentArray = new int[_selectedAssortmentKeyList.Count];
                    _selectedAssortmentKeyList.CopyTo(selectedHeaderArray);
                    // end TT#488 - MD - Jellis - Group Allocation

                    // load the selected headers in the Application session transaction
					// BEGIN TT#764-MD - Stodd - change Assortment Workspace to use AssortmentMember
					if (viewType == eAllocationSelectionViewType.Assortment)
					{
                        //newTrans.LoadAssortmentMemberHeaders(selectedHeaderArray);  // TT#488 - MD - Jellis - Group Allocation
                        newTrans.CreateMasterAssortmentMemberListFromSelectedHeaders(selectedAssortmentArray, selectedHeaderArray); // TT#488 - MD - Jellis - Group Allocation
					}
					else
					{
                        //newTrans.LoadHeaders(selectedHeaderArray); // TT#488 - MD - Jellis - Group Allocation
                        newTrans.CreateMasterAllocationProfileListFromSelectedHeaders(selectedAssortmentArray, selectedHeaderArray); // TT#488 - MD - Jellis - Group Allocation
					}
					// END TT#764-MD - Stodd - change Assortment Workspace to use AssortmentMember
                }
                return newTrans;
            }
            catch
            {
                throw;
            }
        }

        private void BuildSelectedHeaderKeyList()
        {
            try
            {
                _selectedHeaderKeyList.Clear();
                _selectedAssortmentKeyList.Clear(); // TT#488 - MD - Jellis - Group Allocation
                for (int i = 0; i < _selectedRowsSequence.Count; i++)
                {
                    UltraGridRow hdrRow = (UltraGridRow)_selectedRowsSequence[i];
                    int asrtRID = Convert.ToInt32(hdrRow.Cells["KeyH"].Value, CultureInfo.CurrentUICulture);
                    GetAllHeadersInAssortment(asrtRID);
                }
            }
            catch
            {
                throw;
            }
        }

        private void GetAllHeadersInAssortment(int aAsrtRID)
        {
            try
            {
                ArrayList al = _SAB.HeaderServerSession.GetHeadersInAssortment(aAsrtRID);
                for (int i = 0; i < al.Count; i++)
                {
                    int hdrRID = (int)al[i];
                    // begin TT#488 - MD - Jellis - Group Allocation
                    if (hdrRID == aAsrtRID)
                    {
                        _selectedAssortmentKeyList.Add(hdrRID);
                    }
                    else
                    {
                        // end TT#488 - MD - Jellis - Group Allocation
                        _selectedHeaderKeyList.Add(hdrRID);
                    }  // TT#488 - MD - Jellis - Group Allocation
                }
            }
            catch
            {
                throw;
            }
        }

        private void cmsFilter_Click(object sender, EventArgs e)
        {
			Filter();	// TT#765-MD - Stodd - Add toolbars to Assortment Workspace
        }

		private void Filter()		// TT#765-MD - Stodd - Add toolbars to Assortment Workspace
		{
			try
			{
                //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
                EAB.FilterExplorerAssortment.EditThisFilter(this._headerFilterRID);
                SetHeaderFilter(this._headerFilterRID); //Execute the filter after it is edited
                //AssortmentWorkspaceExplorerFilter awef = new AssortmentWorkspaceExplorerFilter(_SAB);
                //awef.OnAssrtWorkspaceFilterChangeHandler += new AssortmentWorkspaceExplorerFilter.AssrtWorkspaceFilterChangeEventHandler(OnAssrtWorkspaceFilterChangeHandler);

        

                //int defaultOwnerUserRID = _SAB.ClientServerSession.UserRID;  //From the workspace - just default to a user filter
                //frmFilterBuilder awef = SharedRoutines.GetFilterFormForNewFilters(filterTypes.AssortmentFilter, SAB, EAB, defaultOwnerUserRID);
                           
          

              
                //if (this.ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer")
                //{
                //    awef.MdiParent = this.ParentForm;
                //}
                //else
                //{
                //    awef.MdiParent = this.ParentForm.Owner;
                //}
                //awef.Show();
                //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
			}
			catch (Exception exception)
			{
				HandleException(exception);
			}

		}

        //private void OnAssrtWorkspaceFilterChangeHandler(object source, AssrtWorkspaceFilterChangeEventArgs e)
        //{
        //    try
        //    {
        //        _fromFilterWindow = true;
        //        IRefresh();
        //        _fromFilterWindow = false;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        private void cmsSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            try
            {
                if (this._gridCol == null)
                {
                    if (ugAssortments.ActiveCell == null)
                    {
                        _gridCol = ugAssortments.Rows[0].Band.Columns["HeaderID"];
                        _gridBand = ugAssortments.Rows[0].Band;
                    }
                    else
                    {
                        _gridCol = ugAssortments.ActiveCell.Column;
                        _gridBand = ugAssortments.ActiveCell.Band;
                    }
                }

                if (this._frmUltraGridSearchReplace == null)
                {
                    this._frmUltraGridSearchReplace = new frmUltraGridSearchReplace(_SAB, false);
                }

                if (this.ugAssortments.Visible == true)
                {
                    if (this.ugAssortments.ActiveRow == null)
                    {
                        ugAssortments.ActiveRow = ugAssortments.Rows[0];
                    }

                    // set the active row if not in band being searched
                    if (ugAssortments.ActiveRow.Band.Key != _gridBand.Key)
                    {
                        if (_gridBand.Key == "Header")
                        {
                            ugAssortments.ActiveRow = ugAssortments.Rows[0];
                        }
                    }

                    if (this.ugAssortments.ActiveRow == null)
                    {
                        MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NeedSearchRow),
                            _thisTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        if (this.ugAssortments.ActiveRow.Band.Columns.Exists(_gridCol.Key))
                        {
                            this._frmUltraGridSearchReplace.ShowSearchReplace(this.ugAssortments, _gridCol, _gridBand);
                        }
                        else
                        {
                            string message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SearchColumnNotFound);
                            message = message.Replace("{0}", _gridCol.Key);
                            MessageBox.Show(message, _thisTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void cmsDelete_Click(object sender, EventArgs e)
        {
            IDelete();
        }

		private void cmsSaveView_Click(object sender, EventArgs e)
		{
			SaveView();		// BEGIN TT#765-MD - Stodd - Add toolbars to Assortment Workspace
		}

		private void SaveView()		// BEGIN TT#765-MD - Stodd - Add toolbars to Assortment Workspace
		{
			try
			{
				this.Enabled = false;
				ViewParms viewParms = new ViewParms();
				viewParms.LayoutID = (int)eLayoutID.assortmentWorkspaceGrid;
				viewParms.ViewGrid = ugAssortments;
				// BEGIN TT#765-MD - Stodd - Add toolbars to Assortment Workspace
				// Begin TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox - 
				//viewParms.ViewName = ((Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.headerToolbarsManager.Tools["viewComboBox"]).Text;
				//viewParms.ViewRID = Convert.ToInt32(((Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.headerToolbarsManager.Tools["viewComboBox"]).Value);
                Infragistics.Win.UltraWinToolbars.ControlContainerTool cct = (Infragistics.Win.UltraWinToolbars.ControlContainerTool)this.headerToolbarsManager.Tools["ControlContainerAsrtView"];	// TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns
                MIDComboBoxEnh.MyComboBox cmbView = (MIDComboBoxEnh.MyComboBox)cct.Control;
                viewParms.ViewName = cmbView.Text;
                viewParms.ViewRID = int.Parse(cmbView.SelectedValue.ToString());
				// End TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox - 

				if (viewParms.ViewRID != Include.NoRID)
				{
					//viewParms.ViewUserRID = Convert.ToInt32(_dtView.Rows[(int)cboView.SelectedIndex]["USER_RID"], CultureInfo.CurrentUICulture);
                    // Begin TT#1411-MD - RMatelic - Move the deletion of the allocation workspace views and the assortment workspace views to the View Maintenance window
                    viewParms.ViewUserRID = Convert.ToInt32(_dtView.Rows[(int)cmbView.SelectedIndex]["USER_RID"], CultureInfo.CurrentUICulture);
                    // End TT#1411-MD
				}
				// END TT#765-MD - Stodd - Add toolbars to Assortment Workspace
				else
				{
					viewParms.ViewUserRID = _SAB.ClientServerSession.UserRID;
				}
				viewParms.FunctionSecurity = eSecurityFunctions.ExplorersAssortmentWorkspaceViews;
				viewParms.GlobalViewSecurity = eSecurityFunctions.ExplorersAssortmentWorkspaceViewsGlobal;
				viewParms.UserViewSecurity = eSecurityFunctions.ExplorersAssortmentWorkspaceViewsUser;
				viewParms.ShowDetailsCheckBox = false;

				// Begin TT#1390-MD - stodd - Assortment Workspace Save View lists headers filters instead of assortment header filters.
				ViewSave gridViewSaveForm = new ViewSave(_SAB, viewParms, true);
                //gridViewSaveForm.useAssortmentFilters = true; //TT#1313-MD -jsobek -Header Filters
				// End TT#1390-MD - stodd - Assortment Workspace Save View lists headers filters instead of assortment header filters.
				gridViewSaveForm.OnViewSaveClosingEventHandler += new ViewSave.ViewSaveClosingEventHandler(OnViewSaveClosing);
				gridViewSaveForm.MdiParent = this.ParentForm.Owner;
				gridViewSaveForm.Show();
			}
			catch (Exception ex)
			{
				HandleException(ex);
				this.Enabled = true;
			}
		}

        // Begin TT#454 - RMatelic - Add Views in Style Review - rewrite using aViewParms
        //void OnViewSaveClosing(object source, bool aViewSaved, int aViewRID, bool aViewDeleted)
		void OnViewSaveClosing(object source, ViewParms aViewParms)
		{
			try
			{
				if (aViewParms.ViewSaved || aViewParms.ViewDeleted)
				{
					BindViewCombo();
					// BEGIN MID Track #6407 - Grouping and Saving view takes several minutes  
					//if (aViewSaved || aViewDeleted)
					//{
					//    cboView.SelectedValue = aViewRID;
					//}
					if (aViewParms.ViewSaved)
					{
						_viewSaved = true;
					}
					// BEGIN TT#765-MD - Stodd - Add toolbars to Assortment Workspace
					//cboView.SelectedValue = aViewParms.ViewRID;
                    // Begin TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox
					//((Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.headerToolbarsManager.Tools["viewComboBox"]).Value = aViewParms.ViewRID;
                    Infragistics.Win.UltraWinToolbars.ControlContainerTool cct = (Infragistics.Win.UltraWinToolbars.ControlContainerTool)this.headerToolbarsManager.Tools["ControlContainerAsrtView"];	// Begin TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns
                    MIDComboBoxEnh.MyComboBox cmbView = (MIDComboBoxEnh.MyComboBox)cct.Control;
                    cmbView.SelectedValue = aViewParms.ViewRID;
                    // End TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox

					// END TT#765-MD - Stodd - Add toolbars to Assortment Workspace
					// End TT#454
					// END MID Track #6407
					// BEGIN Workspace Usability Enhancement - Ron Matelic
					//ShowHideDetails();
					// END Workspace Usability Enhancement
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
			finally
			{
				this.Enabled = true;
			}
		}

		// Begin TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns
        private void CheckSaveLayout()
        {
            if (FormLoaded)
            {
                SaveLayout();
            }
        }
		// End TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns
        private void cmsCopy_Click(object sender, EventArgs e)
        {
             
        }

        private void cmsColSelectAll_Click(object sender, EventArgs e)
        {
            HideColumns(false);
        }

        private void cmsColClearAll_Click(object sender, EventArgs e)
        {
            HideColumns(true);
        }

        private void HideColumns(bool aHideColumn)
        {
            try
            {
                foreach (UltraGridColumn hdrCol in this.ugAssortments.DisplayLayout.Bands["Header"].Columns)
                {
                    if (!hdrCol.IsChaptered && hdrCol.ExcludeFromColumnChooser != ExcludeFromColumnChooser.True)
                    {
                        hdrCol.Hidden = aHideColumn;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }    

        #endregion

        #region IFormBase Members
        public override void ICut()
        {

        }

        public override void ICopy()
        {

        }

        public override void IPaste()
        {

        }

        public override void ISave()
        {
            try
            {
                System.EventArgs args = new EventArgs();
               // cmsSave_Click(this.cmsSave, args);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }	

        public override void ISaveAs()
        {
            try
            {
                System.EventArgs args = new EventArgs();
               // cmsSaveAs_Click(this.cmsSaveAs, args);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }  

        public override void IRefresh()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                _workflowNameHash.Clear();
                //_methodHash.Clear(); //TT#1313-MD -jsobek -Header Filters -performance
                _sizeGroupHash.Clear();

                SaveLayout();
                System.EventArgs args = new System.EventArgs();

                if (_dtHeader != null)
                {
                    _dtHeader.Dispose();
                }

                //Begin TT#1313-MD -jsobek -Header Filters
                //NOT reloading headers since we are reloading the whole window

                //FilterHeaderOptions headerFilterOptions = new FilterHeaderOptions();
                //headerFilterOptions.USE_WORKSPACE_FIELDS = true;
                //headerFilterOptions.filterType = filterTypes.AssortmentFilter;
                //_SAB.HeaderServerSession.ReloadHeaders(this._headerFilterRID, headerFilterOptions, true);
                //End TT#1313-MD -jsobek -Header Filters
                AssortmentWorkspaceExplorer_Load(this, args);
                
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void SaveLayout()
        {
            try
            {
				// BEGIN TT#765-MD - Stodd - Add toolbars to Assortment Workspace
                //int viewRID = Convert.ToInt32(cboView.SelectedValue, CultureInfo.CurrentUICulture);
                // Begin TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox
				//int viewRID = Convert.ToInt32(((Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.headerToolbarsManager.Tools["viewComboBox"]).Value);
                Infragistics.Win.UltraWinToolbars.ControlContainerTool cct = (Infragistics.Win.UltraWinToolbars.ControlContainerTool)this.headerToolbarsManager.Tools["ControlContainerAsrtView"];	// TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns
                MIDComboBoxEnh.MyComboBox cmbView = (MIDComboBoxEnh.MyComboBox)cct.Control;
                // Begin TT#2023-MD - JSmith - Release Resouces and Cleared Layouts select to open an Asst and Receive a Null Reference Error
                //int viewRID = int.Parse(cmbView.SelectedValue.ToString());
                int viewRID = Include.NoRID;
                if (cmbView != null
                    && cmbView.SelectedValue != null)
                {
                    viewRID = int.Parse(cmbView.SelectedValue.ToString());
                }
                // End TT#2023-MD - JSmith - Release Resouces and Cleared Layouts select to open an Asst and Receive a Null Reference Error
                // End TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox

				// END TT#765-MD - Stodd - Add toolbars to Assortment Workspace
                _userGridView.UserGridView_Update(_SAB.ClientServerSession.UserRID, eLayoutID.assortmentWorkspaceGrid, viewRID);

                //Begin TT#1313-MD -jsobek -Header Filters
                _filterData.WorkspaceCurrentFilter_Update(_SAB.ClientServerSession.UserRID, eWorkspaceType.AssortmentWorkspace, this._headerFilterRID); //save the current header filter
                //End TT#1313-MD -jsobek -Header Filters
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public override void IDelete()
        {
            try
            {
                if (this.ugAssortments.Selected.Rows.Count > 0)
                {
                    ugAssortments.DeleteSelectedRows();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public override void IFind()
        {
            Search();
        }

        public override void IRestoreLayout()
        {
            try
            {
                InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
                layoutData.InfragisticsLayout_Delete(_SAB.ClientServerSession.UserRID, eLayoutID.assortmentWorkspaceGrid);
                ugAssortments.ResetLayouts();
                ApplyAppearance(ugAssortments);
                //ResetGrids();
				// BEGIN TT#765-MD - Stodd - Add toolbars to Assortment Workspace
                //cboView.SelectedValue = Include.NoRID;
                // Begin TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox
				//((Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.headerToolbarsManager.Tools["viewComboBox"]).Value = Include.NoRID;
                Infragistics.Win.UltraWinToolbars.ControlContainerTool cct = (Infragistics.Win.UltraWinToolbars.ControlContainerTool)this.headerToolbarsManager.Tools["ControlContainerAsrtView"];	// TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns
                MIDComboBoxEnh.MyComboBox cmbView = (MIDComboBoxEnh.MyComboBox)cct.Control;
                cmbView.SelectedValue = Include.NoRID;
                // End TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox

				// END TT#765-MD - Stodd - Add toolbars to Assortment Workspace
                SaveSelectedHeadersAndRebindGrid();
                UpdateSelectedTotals();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void SaveSelectedHeadersAndRebindGrid()
        {
            try
            {
                ArrayList hdrList = new ArrayList();
                if (_selectedRowsSequence.Count > 0)
                {
                    foreach (UltraGridRow row in _selectedRowsSequence)
                    {
                        hdrList.Add(Convert.ToInt32(row.Cells["KeyH"].Value, CultureInfo.CurrentUICulture));
                    }
                }
                this.ugAssortments.DataSource = null;
                this.ugAssortments.DataSource = _bindSourceHeader;
                if (hdrList.Count > 0)
                {
                    _selectedRowsSequence = new ArrayList();
                    IEnumerable enumerator = ugAssortments.DisplayLayout.Bands["Header"].GetRowEnumerator(GridRowType.DataRow);
                    foreach (UltraGridRow hRow in enumerator)
                    {
                        int hdrRID = Convert.ToInt32(hRow.Cells["KeyH"].Value, CultureInfo.CurrentUICulture);
                        if (hdrList.Contains(hdrRID))
                        {
                            _selectedRowsSequence.Add(hRow);
                            hdrList.Remove(hdrRID);
                        }
                        if (hdrList.Count == 0)
                        {
                            break;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        } 
        #endregion

        #region EnqueueDequeue Methods

        private bool HeadersEnqueued()
        {
            AllocationHeaderProfile ahp;
			// BEGIN Stodd - 4.0 to 4.1 Manual merge
			string enqMessage = string.Empty;
			// End Stodd - 4.0 to 4.1 Manual merge
            try
            {
				//_headerEnqueue = null;	//  Stodd - 4.0 to 4.1 Manual merge
                //_headerList.Clear();      // T#488 - MD - Jellis - Group Allocation
                _headerRidEnqueueList.Clear(); // TT#488 - MD - Jellis - Group Allocation

                BuildSelectedHeaderKeyList();
                // begin TT#488 - MD - Jellis - Group Allocation
                int[] selectedHeaderArray = new int[_selectedHeaderKeyList.Count];
                _selectedHeaderKeyList.CopyTo(selectedHeaderArray);
                int[] selectedAssortmentArray = new int[_selectedAssortmentKeyList.Count];
                _headerRidEnqueueList.AddRange(selectedAssortmentArray);
                _headerRidEnqueueList.AddRange(selectedHeaderArray);
                if (_headerRidEnqueueList.Count == 0)
                {
                    return false;
                }
                if (!_trans.EnqueueHeaders(_headerRidEnqueueList, out enqMessage))
                {
                    throw new HeaderConflictException();
                }
                return true;
                // end TT#488 - MD - Jellis - Group Allocation
                //foreach (int key in _selectedHeaderKeyList)
                //{
                //    ahp = new AllocationHeaderProfile(key);
                //    _headerList.Add(ahp);
                //}
               
                //if (_headerList.Count == 0)
                //{
                //    return false;
                //}
                //else
                //{
                //    // BEGIN Stodd - 4.0 to 4.1 Manual merge
                //    if (!_trans.EnqueueHeaders(_headerList, out enqMessage))
                //    {
                //        throw new HeaderConflictException();
                //    }
                //    //_headerEnqueue = new HeaderEnqueue(_trans, _headerList);
                //    //_headerEnqueue.EnqueueHeaders();
                //    // END Stodd - 4.0 to 4.1 Manual merge
                //    return true;
                //}
                // end TT#488 - MD - Jellis - Group Allocation
            }
            catch (HeaderConflictException)
            {
				// BEGIN Stodd - 4.0 to 4.1 Manual merge
				DisplayEnqueueConflict(enqMessage);
				// END Stodd - 4.0 to 4.1 Manual merge
                return false;
            }
        }

        private bool HeadersAddedEnqueued(ArrayList aHdrList)
        {
			string enqMessage = string.Empty;
            try
            {
                if (aHdrList.Count > 0)
                {
					// BEGIN Stodd - 4.0 to 4.1 Manual merge
					//if (_headerEnqueue != null)
					//{
					//    _headerEnqueue.EnqueueHeadersAdded(aHdrList);
					//}
					//else if (!HeadersEnqueued())
					//{
					//    return false;
					//}
					List<int> hdrKeyList = new List<int>();
					foreach (int key in aHdrList)
					{
						hdrKeyList.Add(key);
					}
					if (!_trans.EnqueueHeaders(hdrKeyList, out enqMessage))
					{
						throw new HeaderConflictException();
					}
					// END Stodd - 4.0 to 4.1 Manual merge
                }
                return true;
            }
            catch (HeaderConflictException)
            {
				// BEGIN Stodd - 4.0 to 4.1 Manual merge
				DisplayEnqueueConflict(enqMessage);
				// END Stodd - 4.0 to 4.1 Manual merge
                return false;
            }
        }

		// BEGIN Stodd - 4.0 to 4.1 Manual merge
        private void DisplayEnqueueConflict(string enqMessage)
        {
			//SecurityAdmin secAdmin = new SecurityAdmin();
			//string errMsg = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_HeadersInUse) + ":" + System.Environment.NewLine;

			//foreach (HeaderConflict hdrCon in _headerEnqueue.HeaderConflictList)
			//{
			//    SelectedHeaderList selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
			//    SelectedHeaderProfile shp = (SelectedHeaderProfile)selectedHeaderList.FindKey(System.Convert.ToInt32(hdrCon.HeaderRID, CultureInfo.CurrentUICulture));
			//    if (shp != null)
			//    {
			//        errMsg += System.Environment.NewLine + shp.HeaderID + ", User: " + secAdmin.GetUserName(hdrCon.UserRID);
			//    }
			//    else
			//    {
			//        foreach (AllocationHeaderProfile ahp in _headerProfileArrayList)
			//        {
			//            if (ahp.Key == hdrCon.HeaderRID)
			//            {
			//                errMsg += System.Environment.NewLine + ahp.HeaderID + ", User: " + secAdmin.GetUserName(hdrCon.UserRID);
			//                break;
			//            }
			//        }
			//    }
			//}
			//errMsg += System.Environment.NewLine + System.Environment.NewLine;
            DialogResult diagResult = _trans.SAB.MessageCallback.HandleMessage(
				enqMessage,
                "Header Lock Conflict",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
        }
		// END Stodd - 4.0 to 4.1 Manual merge

        public void DequeueHeaders()
        {
            try
            {
                if (_headerEnqueue != null)
                {
                    _headerEnqueue.DequeueHeaders();
                    _headerEnqueue = null;
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Enums
        public enum eBalanceAction
        {
            RowAdded,
            RowDeleted,
            ValueChanged
        }

        public enum eSecurityType
        {
            View,
            Update
        }
        #endregion

        #region Workspace Window Events
        private void AssortmentWorkspaceExplorer_Enter(object sender, EventArgs e)
        {
            try
            {
                EAB.Explorer.EnableMenuItem(eMIDMenuItem.EditFind);
                EAB.Explorer.ShowMenuItem(eMIDMenuItem.EditFind);
                EAB.Explorer.AddMenuOption(Include.btRestoreLayout);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void AssortmentWorkspaceExplorer_Leave(object sender, EventArgs e)
        {
            try
            {
				SaveToolbarLayout();	// TT#765-MD - Stodd - Add toolbars to Assortment Workspace
                EAB.Explorer.DisableMenuItem(eMIDMenuItem.EditFind);
                EAB.Explorer.HideMenuItem(eMIDMenuItem.EditFind);
                EAB.Explorer.RemoveMenuOption(Include.btRestoreLayout);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        #endregion

		#region excel export
		private void ExportToExcel()
		{
			if (_ultraGridExcelExporter1 == null)
			{
				_ultraGridExcelExporter1 = new UltraGridExcelExporter();
				_ultraGridExcelExporter1.RowExporting += new Infragistics.Win.UltraWinGrid.ExcelExport.RowExportingEventHandler(ultraGridExcelExporter1_RowExporting);
				//_ultraGridExcelExporter1.InitializeRow += new Infragistics.Win.UltraWinGrid.ExcelExport.InitializeRowEventHandler(ultraGridExcelExporter1_InitializeRow);
			}

			string caption = MIDText.GetTextOnly(eMIDTextCode.lbl_MB_Excel_Caption);
			caption = caption.Replace("{0}", "Assortment");
			string messageBoxText = MIDText.GetTextOnly(eMIDTextCode.lbl_MB_Excel_Text);
			string button1text = MIDText.GetTextOnly(eMIDTextCode.lbl_Asrt_Excel_But1);
			string button2text = MIDText.GetTextOnly(eMIDTextCode.lbl_Asrt_Excel_But2);
			string NoSelectError = MIDText.GetTextOnly(eMIDTextCode.lbl_MB_Excel_No_Sel);
			int answer = 0;

			MIDMessageBox mmb = new MIDMessageBox(caption, messageBoxText, new Color[2] { Color.Empty, Color.Empty, }, //button colors            
					new string[2] { button1text, button2text, }, 0, null, MIDGraphics.GetIcon(MIDGraphics.ExcelImage), true);

			answer = mmb.Show();

			string errorMsg = string.Empty;

			if (answer == 0)
			{ CreateExcelAllHeader(FindSavePath()); }

			if (answer == 1)
			{
				if (ugAssortments.Selected.Rows.Count > 0)
				{
					CreateExcelHeaderSelected(FindSavePath());
				}
				else
				{
					errorMsg = NoSelectError;
					MessageBox.Show(errorMsg, _thisTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

			if (answer == 2)
			{
				if (ugAssortments.Selected.Rows.Count > 0)
				{
					CreateExcelHeaderSelected(FindSavePath());
				}
				else
				{
					errorMsg = NoSelectError;
					MessageBox.Show(errorMsg, _thisTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void CreateExcelAllHeader(String myFilepath)
		{
			string MessBoxAlltext1 = MIDText.GetTextOnly(eMIDTextCode.lbl_MB_Excel_All);
			string MessBoxAlltext2 = MIDText.GetTextOnly(eMIDTextCode.lbl_MB_Excel_Exp_Lit);
			try
			{
				if (myFilepath != null)
				{
					_checkForExportSelected = false;
					_ultraGridExcelExporter1.Export(this.ugAssortments, myFilepath);
					MessageBox.Show(MessBoxAlltext1 + myFilepath + MessBoxAlltext2 + ugAssortments.Rows.Count);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void CreateExcelHeaderSelected(String myFilepath)
		{
			string MessBoxSHtext1 = MIDText.GetTextOnly(eMIDTextCode.lbl_MB_Excel_HS);
			string MessBoxSHtext2 = MIDText.GetTextOnly(eMIDTextCode.lbl_MB_Excel_Exp_Lit);
			try
			{
				if (myFilepath != null)
				{
					_checkForExportSelected = true;
					_ultraGridExcelExporter1.Export(this.ugAssortments, myFilepath);
					MessageBox.Show(MessBoxSHtext1 + myFilepath + MessBoxSHtext2 + ugAssortments.Selected.Rows.Count);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		private void ultraGridExcelExporter1_RowExporting(object sender, RowExportingEventArgs e)
		{
			// The GridRow property on the event args is a clone of the on-screen row, and it will not pick up the Selected State 
			UltraGridRow exportRow = e.GridRow;

			//  Get the grid
			UltraGrid grid = (UltraGrid)e.GridRow.Band.Layout.Grid;

			// Get the real, on-screen row, from the export row. 
			UltraGridRow onScreenRow = grid.GetRowFromPrintRow(exportRow);

			// If the on-screen row is not selected, do not export it. 
			if (onScreenRow.Selected == false && _checkForExportSelected == true)
				e.Cancel = true;
		}

		//private void ultraGridExcelExporter1_InitializeRow(object sender, ExcelExportInitializeRowEventArgs e)
		//{

		//    // If SUMMARY only and the on-screen row is not in the first band, do not export it. 
		//    if (!_exportDetails && e.CurrentOutlineLevel == 0)
		//    {
		//        //e.SkipRow = true;
		//        //e.SkipSiblings = true;
		//        e.SkipDescendants = true;
		//    }
		//}

		private String FindSavePath()
		{
			Stream myStream;
			string myFilepath = null;
			try
			{
				SaveFileDialog saveFileDialog1 = new SaveFileDialog();
				saveFileDialog1.Filter = "excel files (*.xls)|*.xls";
				saveFileDialog1.FilterIndex = 2;
				saveFileDialog1.RestoreDirectory = true;
				if (saveFileDialog1.ShowDialog() == DialogResult.OK)
				{
					if ((myStream = saveFileDialog1.OpenFile()) != null)
					{
						myFilepath = saveFileDialog1.FileName;
						myStream.Close();
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return myFilepath;
		}

		#endregion

		// BEGIN TT#765-MD - Stodd - Add toolbars to Assortment Workspace
		#region Toolbars
		private void headerToolbarsManager_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			switch (e.Tool.Key)
			{
				case "reviewSelectionScreen":
					DetermineWindow(eAllocationSelectionViewType.None);
					break;
				case "reviewAssortment":
					DetermineWindow(eAllocationSelectionViewType.Assortment);
					break;
				case "reviewProperties":
					ReviewProperties();
					break;
				case "searchButton":
					Search();
					break;
				case "filter":
					Filter();
					break;
				case "saveView":
					SaveView();
					break;
				case "gridChooseColumns":
					this.ugAssortments.ShowColumnChooser("Choose Columns");
					break;
				case "gridExport":
					ExportToExcel();
					break;
				case "gridEmailPopupMenu":
					UltraGridExcelExportWrapper exporter4 = new UltraGridExcelExportWrapper(this.ugAssortments);
					ShowEmailForm(exporter4.ExportAllRowsToExcelAsAttachment());
					break;

			}
		}

		private void headerToolbarsManager_ValueChanged(object sender, Infragistics.Win.UltraWinToolbars.ToolEventArgs e)
		{
			switch (e.Tool.Key)
			{
				case "viewComboBox":
					SetView((int)((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).Value);
					break;
                case "headerFilterComboBox":
                    SetHeaderFilter((int)((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).Value);
                    break;
			}
		}
		private void LoadViewsOnToolbar(DataTable dtView)
		{
            // Begin TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox
            //Infragistics.Win.UltraWinToolbars.ComboBoxTool cbo = (Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.headerToolbarsManager.Tools["viewComboBox"];
            //cbo.ValueList.ValueListItems.Clear();
            //foreach (DataRow row in dtView.Rows)
            //{
            //    //if ((int)row["VIEW_RID"] != -1)
            //    //{
            //    cbo.ValueList.ValueListItems.Add((int)row["VIEW_RID"], (string)row["VIEW_ID"]);
            //    //}
            //}

            _bindingView = true;

            Infragistics.Win.UltraWinToolbars.ControlContainerTool cct = (Infragistics.Win.UltraWinToolbars.ControlContainerTool)this.headerToolbarsManager.Tools["ControlContainerAsrtView"];	// TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns
            MIDComboBoxEnh.MyComboBox cmbView = (MIDComboBoxEnh.MyComboBox)cct.Control;

            dtView.PrimaryKey = new DataColumn[] { dtView.Columns["VIEW_RID"] };
            cmbView.ValueMember = "VIEW_RID";
            cmbView.DisplayMember = "VIEW_ID";
            cmbView.DataSource = dtView;
			// Begin TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns
            //if (dtView.Rows.Count > 0)
            //{
            //    cmbView.SelectedIndex = 0;
            //}
            cmbView.SelectedValue = Include.NoRID;
			// End TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns
            _bindingView = false;
            // End TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox
		}
		
        //Begin TT#1313-MD -jsobek -Header Filters
        private void LoadAssortmentFiltersOnToolbar(DataTable dtAssortmentFilters)
        {
			// Begin TT#1386-MD - stodd - merge assortment into 5.4
            //Infragistics.Win.UltraWinToolbars.ComboBoxTool cbo = (Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.headerToolbarsManager.Tools["headerFilterComboBox"];
            //cbo.ValueList.ValueListItems.Clear();
            Infragistics.Win.UltraWinToolbars.ControlContainerTool cct = (Infragistics.Win.UltraWinToolbars.ControlContainerTool)this.headerToolbarsManager.Tools["ControlContainerAsrtFilter"];	// TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns
            MIDComboBoxEnh.MyComboBox cmbFilter = (MIDComboBoxEnh.MyComboBox)cct.Control;

            DataView dv = new DataView(dtAssortmentFilters);
            dv.Sort = "FILTER_NAME";

            //foreach (DataRowView rowView in dv)
            //{
            //    cbo.ValueList.ValueListItems.Add((int)rowView.Row["FILTER_RID"], (string)rowView.Row["FILTER_NAME"]);
            //}
            cmbFilter.ValueMember = "FILTER_RID";
            cmbFilter.DisplayMember = "FILTER_NAME";
            cmbFilter.DataSource = dtAssortmentFilters;
            if (dtAssortmentFilters.Rows.Count > 0)
            {
                cmbFilter.SelectedIndex = 0;
            }
			// End TT#1386-MD - stodd - merge assortment into 5.4
        }
        //End TT#1313-MD -jsobek -Header Filters

		private void SetView(int viewRID)
		{
			if (!_bindingView && !_fromLoadEvent)
			{

				if (_viewSaved)
				{
					_viewSaved = false;
				}
				if (viewRID != _lastSelectedViewRID)
				{

					_lastSelectedViewRID = viewRID;

                    //Begin TT#1313-MD -jsobek -Header Filter
                    //if (_gridViewData.GridViewFilterExists(viewRID))
                    //{
                    //    //_cancelSelectEvent = true;
                    //    IRefresh();
                    //}
                    //else
                    //{
                    //    ApplyViewToGridLayout(viewRID);
                    //}
					
					bool useFilterSorting = false;
					
                    // Begin TT#1370-MD - stodd - change view in allocation workspace, the filter should change to match filter on view - 
                    Infragistics.Win.UltraWinToolbars.ControlContainerTool cct = (Infragistics.Win.UltraWinToolbars.ControlContainerTool)this.headerToolbarsManager.Tools["ControlContainerAsrtFilter"];
                    MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox cmbFilter = (MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox)cct.Control;
                    // End TT#1370-MD - stodd - change view in allocation workspace, the filter should change to match filter on view - 
                    int workspaceFilterRID = _gridViewData.GridViewReadWorkspaceFilterRID(viewRID, ref useFilterSorting);
                    if (workspaceFilterRID != Include.NoRID)
                    {
						// Begin TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns
                        //IRefresh();
                        cmbFilter.SelectedValue = workspaceFilterRID;
                        SetHeaderFilter(workspaceFilterRID);
                        //ApplyViewToGridLayout(viewRID);
                    }
                    //else
                    //{
                    //    ApplyViewToGridLayout(viewRID);
                    //}
                    //End TT#1313-MD -jsobek -Header Filter

                    // Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
					//ApplyViewToGridLayout(viewRID);
					ApplyViewToGridLayout(viewRID, useFilterSorting);
					// End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only	
					// End TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns
				}
			}
		}


        //Begin TT#1313-MD -jsobek -Header Filters
        public void SetHeaderFilter(int headerFilterRID)
        {
            if (!_bindingView && !_fromLoadEvent)
            {
                this._headerFilterRID = headerFilterRID;
          
                //IRefresh(); 
                LoadHeadersOnGrid();
            }

        }
        //End TT#1313-MD -jsobek -Header Filters

		private void ShowEmailForm(System.Net.Mail.Attachment a)
		{

			EmailMessageForm frm = new EmailMessageForm();
			frm.AddAttachment(a);
			string subject = "Assortment Workspace Headers";
			MIDRetail.Data.SecurityAdmin secAdmin = new MIDRetail.Data.SecurityAdmin();
			DataTable dt = secAdmin.GetUser(_SAB.ClientServerSession.UserRID);
			if (dt.Rows.Count > 0)
			{
				string userName = String.Empty;
				string userFullName = String.Empty;
				if (dt.Rows[0].IsNull("USER_NAME") == false)
				{
					userName = (string)dt.Rows[0]["USER_NAME"];
				}
				if (dt.Rows[0].IsNull("USER_FULLNAME") == false)
				{
					userFullName = (string)dt.Rows[0]["USER_FULLNAME"];
				}
				subject += " - " + userName + " (" + userFullName + ")";
			}

			frm.SetDefaults("", "", "", subject, "Please see attached file.");
			frm.ShowDialog();

		}
		#endregion

		#region "Grid Export"

		/// <summary>
		/// Used to provide Excel Export Functionality for the Infragistics UltraWinGrid
		/// Just create an instance of this class, pass in your grid and call Export function.
		/// Saves in xls format - does not support xlsx format
		/// </summary>
		private class UltraGridExcelExportWrapper
		{
			private Infragistics.Win.UltraWinGrid.UltraGrid _ug;
			private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
			private bool _checkForExportSelected = false;
			private string _objectDescriptor;
			public UltraGridExcelExportWrapper(Infragistics.Win.UltraWinGrid.UltraGrid ug, string objectDescriptor = "rows")
			{
				_ug = ug;
				_objectDescriptor = objectDescriptor;

				this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter();
				this.ultraGridExcelExporter1.RowExporting += new Infragistics.Win.UltraWinGrid.ExcelExport.RowExportingEventHandler(ultraGridExcelExporter1_RowExporting);
			}

			public void ExportAllRowsToExcel()
			{
				string myFilepath = FindSavePath();
				string MessBoxText1 = "All " + _objectDescriptor + " sucessfully exported to \r\n";
				string MessBoxText2 = "Number of " + _objectDescriptor + " exported: ";
				try
				{
					if (myFilepath != null)
					{
						_checkForExportSelected = false;
						this.ultraGridExcelExporter1.Export(_ug, myFilepath);
						MessageBox.Show(MessBoxText1 + myFilepath + "\r\n" + MessBoxText2 + _ug.Rows.Count);
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			public System.Net.Mail.Attachment ExportAllRowsToExcelAsAttachment()
			{
				try
				{
					_checkForExportSelected = false;
					return GetEmailAttachment();
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			public System.Net.Mail.Attachment ExportSelectedRowsToExcelAsAttachment()
			{
				try
				{

					_checkForExportSelected = true;
					return GetEmailAttachment();

				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			private System.Net.Mail.Attachment GetEmailAttachment()
			{
				Infragistics.Documents.Excel.Workbook wb = new Infragistics.Documents.Excel.Workbook();
				this.ultraGridExcelExporter1.Export(_ug, wb);

				//Infragistics does not save nicely directly to a memory stream, so saving as a file and reading it back into memory stream
				string fileName = System.IO.Path.GetTempPath() + "\\tempAssortmentWorkspaceHeaders_" + Data.EnvironmentInfo.MIDInfo.userName + "_" + System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".tmp";
				wb.Save(fileName);
				byte[] b = System.IO.File.ReadAllBytes(fileName);
				System.IO.File.Delete(fileName);
				System.IO.MemoryStream streamAttachment = new System.IO.MemoryStream(b);

				System.Net.Mail.Attachment attachmentWorkbook;
				attachmentWorkbook = new System.Net.Mail.Attachment(streamAttachment, "AssortmentWorkspaceHeaders.xls");
				return attachmentWorkbook;
			}

			public void ExportSelectedRowsToExcel()
			{
				string myFilepath = FindSavePath();
				string MessBoxText1 = "Selected " + _objectDescriptor + " sucessfully exported to \r\n";
				string MessBoxText2 = "Number of " + _objectDescriptor + " exported: ";
				try
				{
					if (myFilepath != null)
					{
						_checkForExportSelected = true;
						this.ultraGridExcelExporter1.Export(_ug, myFilepath);
						MessageBox.Show(MessBoxText1 + myFilepath + "\r\n" + MessBoxText2 + _ug.Selected.Rows.Count);
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			private void ultraGridExcelExporter1_RowExporting(object sender, Infragistics.Win.UltraWinGrid.ExcelExport.RowExportingEventArgs e)
			{
				// The GridRow property on the event args is a clone of the on-screen row, and it will not pick up the Selected State 
				//Infragistics.Win.UltraWinGrid.UltraGridRow exportRow = e.GridRow;

				//  Get the grid
				//Infragistics.Win.UltraWinGrid.UltraGrid grid = (Infragistics.Win.UltraWinGrid.UltraGrid)e.GridRow.Band.Layout.Grid;

				// Get the real, on-screen row, from the export row. 
				Infragistics.Win.UltraWinGrid.UltraGridRow onScreenRow = _ug.GetRowFromPrintRow(e.GridRow);

				// If the on-screen row is not selected, do not export it. 
				if (onScreenRow.Selected == false && _checkForExportSelected == true)
					e.Cancel = true;
			}
			private String FindSavePath()
			{
				System.IO.Stream myStream;
				string myFilepath = null;
				try
				{
					SaveFileDialog saveFileDialog1 = new SaveFileDialog();
					saveFileDialog1.Filter = "excel files (*.xls)|*.xls";
					saveFileDialog1.FilterIndex = 2;
					saveFileDialog1.RestoreDirectory = true;
					if (saveFileDialog1.ShowDialog() == DialogResult.OK)
					{
						if ((myStream = saveFileDialog1.OpenFile()) != null)
						{
							myFilepath = saveFileDialog1.FileName;
							myStream.Close();
						}
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return myFilepath;
			}
		}

		#endregion

		// Begin TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox - 
        private void assortmentMidComboBoxEnhView_SelectionChangeCommitted(object sender, EventArgs e)	// TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns

        {
            Infragistics.Win.UltraWinToolbars.ControlContainerTool cct = (Infragistics.Win.UltraWinToolbars.ControlContainerTool)this.headerToolbarsManager.Tools["ControlContainerAsrtView"];	// TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns

            MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox cmbView = (MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox)cct.Control;
            int viewRid = int.Parse(cmbView.SelectedValue.ToString());

            SetView(viewRid);
        }

        private void assortmentMidComboBoxFilter_SelectionChangeCommitted(object sender, EventArgs e) // TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns

        {
            Infragistics.Win.UltraWinToolbars.ControlContainerTool cct = (Infragistics.Win.UltraWinToolbars.ControlContainerTool)this.headerToolbarsManager.Tools["ControlContainerAsrtFilter"];	// TT#1360-MD - stodd - Convert Dropdowns on Assortment Workspace to be MID Enhanced Dropdowns

            MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox cmbFilter = (MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox)cct.Control;
            int filterRid = int.Parse(cmbFilter.SelectedValue.ToString());

            SetHeaderFilter(filterRid);
        }

		// End TT#1337-MD - stodd - On Assortment Explorer, replace toolbar combobox with MID Enhanced combobox - 

	}
	// END TT#765-MD - Stodd - Add toolbars to Assortment Workspace
}
