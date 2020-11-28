// Begin Track #4872 - JSmith - Global/User Attributes
// Renamed cboAttributeSet to cbxAttributeSet so it would not get protected in read only mode.
// End Track #4872
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlTypes;
using System.Windows.Forms;
using System.Diagnostics;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for VelocityMethod.
	/// </summary>
    public class frmVelocityMethod : WorkflowMethodFormBase
    {
        private int _nodeRID = -1;
        private VelocityMethod _velocityMethod = null;
        //		private string _strMethodType;
        ApplicationSessionTransaction _trans = null;
        AllocationViewSelection _avs;
        private System.Data.DataTable _basisDataTable;
        //private System.Data.DataTable _salesPeriodDataTable;
        private System.Data.DataTable _velocityGradesDataTable;
        private System.Data.DataTable _sellThruPctsDataTable;
        private System.Data.DataTable _groupLevelDataTable;
        private System.Data.DataTable _dbMatrixDataTable;
        private System.Data.DataTable _screenMatrixDataTable;
        private System.Data.DataTable _merchDataTable2;
        private System.Data.DataTable _defOnHandRules;
        private System.Data.DataTable _noOnHandRules;
        private System.Data.DataTable _compDataTable;
        private System.Data.DataTable _groupDataTable;
        private System.Data.DataTable _dtMatrixView;   // TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail

        private TabPage _currentTabPage = null;
        private TabPage _currentMethodTabPage = null;
        private DataSet _dsVelocity = null;
        //		private CalendarDateSelector _frm;
        private const string strBegin = "begin";
        private const string strShip = "ship";
        private Bitmap picRelToPlan;
        private Bitmap picRelToCurrent;
        //		private UltraGridRow _dateRow;
        private bool _skipAfterCellUpdate = false;
        private bool _basisChangesMade;
        private bool _velocityGradesChangesMade;
        private bool _sellThruPctsChangesMade;
        private bool _matrixChangesMade;
        //		private bool _FormLoadError = false;
        private bool _velocityGradesIsPopulated = false;
        private bool _sellThruPctsIsPopulated = false;
        private bool _basisIsPopulated = false;
        //private bool _salesPeriodIsPopulated = false;
        private bool _matrixIsPopulated = false;
        private bool _rebuildMatrix = false;
        private bool _setRowPosition = true;
        private bool _setReset = false;
        private bool _attributeReset = false;
        //		private bool _showMessageBox = true;
        private bool _compReset = false;
        private bool _cbxReset = false;
        //		private bool _merchReset = false;
        private bool _simStoresReset = false;
        // Begin Track #6074 stodd
        private bool _gradesByBasisReset = false;
        // End Track #6074 stodd
        private bool _shipReset = false;
        private bool _attributeChanged = false;
        private bool _attributeSetChanged = false;  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
       // private bool _matrixModeChanged = false;  // TT#960 - AGallagher - Velocity - Spread Average (#7) - Screen Processing Oversites 
        private bool _matrixProcessed = false;
        private bool _matrixEverProcessed = false;  // TT#3617 - JSmith - Velocity throws Null Ref error when a change is made to the Boundaries
        private bool _addBandGroups = true;
        private bool _statsCalculated = false;
        private bool _attrSetChanged = false;
        private bool _basisNodeInList = false;
        private bool _averageReset = false; // TT#587 Matrix Totals Wrong
        private bool _updateFromStoreDetail = false;  // BEGIN END MID Track #2761
        // BEGIN MID Track #3878 - JSmith - Receive Weight has changed message 
        private bool _changedByCode = false;
        // END MID Track #3300
        private bool _bindingView = false;               // TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        private bool _textChanged = false; 
        private bool _priorError = false;  
        private int _lastMerchIndex = -1;
        private char _InventoryInd;  
        // END TT#1287 - AGallagher - Inventory Min/Max
        // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
        private char _ApplyMinMaxInd;
        // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
        
        private ArrayList _errorCells = null;
        private ArrayList _matrixColumnHeaders = null;
        private ArrayList _userRIDList = null;  // TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        private VelocityGradeList _velocityGradeList;
        private SellThruPctList _sellThruPctList;
        private string _thisTitle;
        private string _noOnHandLabel;
        private string _lblTotalSales;
        private string _lblAvgSales;
        private string _lblPctTotalSales;
        private string _lblAvgSalesIdx;

        //BEGIN TT#153 – add variables to velocity matrix - apicchetti
        private string _lblTotalNumStores;
        private string _lblStockPercentOfTotal;
        private string _lblAvgStock;
        private string _lblAllocationPercentOfTotal;
        //END TT#153 – add variables to velocity matrix - apicchetti

        private bool _balancechanged = false; //tt#290 - velocity values update on balance issue - apicchetti
        private int _processintctr = 0;  //tt#290 - velocity values update on balance issue - apicchetti

        private string _lblAllStores;
        private string _lblSet;
        private int _prevSetValue;
        private int _prevSetValueTest;  // TT#5792 - AGallagher - Velocity Method with Total Matrix and Average Mode not giving expected results
        private int _prevAttributeValue;
        //		private int _prevMerchIndex;
        private int _prevCompIndex;
        private int _lastSelectedViewRID;       // TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        private UltraGridCell _returnToCell = null;
        //		private FunctionSecurityProfile _VelocityMatrixSecurityLevel;
        private SelectedHeaderList _selectedHeaderList;
        private StoreGroupLevelListViewProfile _totalMatrixSet;
        private System.Windows.Forms.TabPage tabMethod;
        private System.Windows.Forms.TabPage tabProperties;
        private System.Windows.Forms.CheckBox cbxSimilarStores;
        private System.Windows.Forms.RadioButton radAvgChain;
        private System.Windows.Forms.RadioButton radAvgSet;
        private System.Windows.Forms.GroupBox gbxAverage;
        private System.Windows.Forms.GroupBox gbxShip;
        private System.Windows.Forms.RadioButton radShipBasis;
        private System.Windows.Forms.RadioButton radHeaderStyle;
        private System.Windows.Forms.TabControl tabVelocity;
        private System.Windows.Forms.TabPage tabBasis;
        private System.Windows.Forms.TabPage tabGrades;
        private System.Windows.Forms.ContextMenu mnuVelocityGradesGrid;
        private System.Windows.Forms.ContextMenu mnuSellThruPctsGrid;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugVelocityGrades;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugSellThruPcts;
        private System.Windows.Forms.ContextMenu mnuMatrixGrid;
        private System.Windows.Forms.TabControl tabVelocityMethod;
        private System.Windows.Forms.CheckBox cbxTrendPct;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugBasisNodeVersion;
        private System.Windows.Forms.GroupBox gbxCriteria;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugWorkflows;
        private System.Windows.Forms.ContextMenu mnuBasisGrid;
        private System.Windows.Forms.ContextMenu mnuSalesGrid;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnChanges;
        private System.Windows.Forms.Label lblSet;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.TextBox txtOHQuantity;
        private System.Windows.Forms.TextBox txtNoOHQuantity;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugMatrix;
        private System.Windows.Forms.TabPage tabMatrix;
        private System.Windows.Forms.Label lblAttribute;
        // Begin Track #4872 - JSmith - Global/User Attributes
        //private System.Windows.Forms.ComboBox cboStoreAttribute;
        private MIDAttributeComboBox cboStoreAttribute;
        // End Track #4872
        private System.Windows.Forms.GroupBox gbxActiveMatrix;
        private System.Windows.Forms.Label lblComponent;
        private System.Windows.Forms.CheckBox cbxInteractive;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugGroupData;
        private System.Windows.Forms.GroupBox gbxNoOH;
        private System.Windows.Forms.GroupBox gbxMatrixDefault;

        //TT#152 Velocity Balance - apicchetti
        private CheckBox cbxBalance;
        //TT#152 Velocity Balance - apicchetti
                
        //Begin TT#299 - stodd - null ref changing attribute set
        private bool _afterProcess = false;
        //End TT#299 - stodd - null ref changing attribute set
		private bool _isProcessingAssortment = false;	// TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member

        private System.ComponentModel.IContainer components;
        private GroupBox gbxApplyMinMax;
        private RadioButton radApplyMinMaxVelocity;
        private RadioButton radApplyMinMaxStore;
        private RadioButton radApplyMinMaxNone;
        private GroupBox gbxSpreadOption;
        private GroupBox gbxMatrixMode;
        private RadioButton rdoSpreadOptionSmooth;
        private RadioButton rdoSpreadOptionIdx;
        private TextBox txtMatrixModeAvgRule;
        private RadioButton rdoMatrixModeAverage;
        private RadioButton rdoMatrixModeNormal;
        private CheckBox cbxReconcile;
        private GroupBox gbxMinMaxOpt;
        private Label lblInventoryBasis;
        private RadioButton radInventoryMinMax;
        private RadioButton radAllocationMinMax;
        private MIDComboBoxEnh cboInventoryBasis;
        private MIDComboBoxEnh cbxAttributeSet;
        private MIDComboBoxEnh cboNoOHRule;
        private MIDComboBoxEnh cboComponent;
        private MIDComboBoxEnh cboMatrixModeAvgRule;
        private MIDComboBoxEnh cboMatrixView;
        private GroupBox gbxGrade;
        private RadioButton radGradeVariableSales;
        private RadioButton radGradeVariableStock;
        private CheckBox cbxBalanceToHeader;
        private MIDComboBoxEnh cboOHRule;
        private Label lblGA;	// TT#1194-MD - stodd - view GA header 
        private bool _windowIsMaximized = false; // TT#3138 - RMatelic - Velocity Detail when you go in to the screen the scroll bar is the length of the screen and you can't scroll

        //TT#328 - Adding interactive counter so that security enqueue functionality will be bypassed except at initialization - apicchetti
   //     private int _interactiveCtr = 0;  // never used

        /// <summary>
        /// Gets or sets a boolean identifying if changes have been made to the basis data.
        /// </summary>
        public bool BasisChangesMade
        {
            get { return _basisChangesMade; }
            set
            {
                _basisChangesMade = value;
                // if true, turn on master flag
                if (_basisChangesMade)
                {
                    if (FormLoaded)
                    {
                        // BEGIN TT#3114 - AGallagher - Velocity change basis and have to deselect and reselect Activate for data to appear. 
                        //ChangePending = true;
                        CheckInteractive();
                        // END TT#3114 - AGallagher - Velocity change basis and have to deselect and reselect Activate for data to appear. 
                    }
                }
            }
        }
        /// <summary>
        /// Gets or sets a boolean identifying if changes have been made to the velocity grades data.
        /// </summary>
        public bool VelocityGradesChangesMade
        {
            get { return _velocityGradesChangesMade; }
            set
            {
                _velocityGradesChangesMade = value;
                // if true, turn on master flag
                if (_velocityGradesChangesMade)
                {
                    if (FormLoaded)
                    {
                        ChangePending = true;
                    }
                }
            }
        }
        /// <summary>
        /// Gets or sets a boolean identifying if changes have been made to the percent sell thru data.
        /// </summary>
        public bool SellThruPctsChangesMade
        {
            get { return _sellThruPctsChangesMade; }
            set
            {
                _sellThruPctsChangesMade = value;
                // if true, turn on master flag
                if (_sellThruPctsChangesMade)
                {
                    if (FormLoaded)
                    {
                        ChangePending = true;
                    }
                }
            }
        }
        /// <summary>
        /// Gets or sets a boolean identifying if changes have been made to the matrix data.
        /// </summary>
        public bool MatrixChangesMade
        {
            get { return _matrixChangesMade; }
            set
            {
                _matrixChangesMade = value;
                // if true, turn on master flag
                if (_matrixChangesMade)
                {
                    if (FormLoaded)
                    {
                        ChangePending = true;
                    }
                }
            }
        }

        #region Initialize & Dispose

        public frmVelocityMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB)
            : base(SAB, aEAB, eMIDTextCode.frm_VelocityMethod, eWorkflowMethodType.Method)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserVelocity);
            GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalVelocity);

            // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
            GridViewData = new GridViewData();
            UserGridView = new UserGridView();
            MethodViewUserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsUserVelocity);
            MethodViewGlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsGlobalVelocity);
            MethodDetailViewUserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsUserVelocityDetail);
            MethodDetailViewGlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsGlobalVelocityDetail);
            // End TT#231 -
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                // Begin MID Track 4858 - JSmith - Security changes
                //				this.txtDesc.TextChanged -= new System.EventHandler(this.txtDesc_TextChanged);
                //				this.txtName.TextChanged -= new System.EventHandler(this.txtName_TextChanged);
                // End MID Track 4858
                this.cboComponent.SelectionChangeCommitted -= new System.EventHandler(this.cboComponent_SelectionChangeCommitted);
                this.cbxInteractive.CheckedChanged -= new System.EventHandler(this.cbxInteractive_CheckedChanged);
                this.tabVelocityMethod.SelectedIndexChanged -= new System.EventHandler(this.tabVelocityMethod_SelectedIndexChanged);
                this.tabVelocity.SelectedIndexChanged -= new System.EventHandler(this.tabVelocity_SelectedIndexChanged);
                this.ugBasisNodeVersion.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugBasisNodeVersion_CellChange);
                this.ugBasisNodeVersion.AfterRowsDeleted -= new System.EventHandler(this.ugBasisNodeVersion_AfterRowsDeleted);
                this.ugBasisNodeVersion.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugBasisNodeVersion_MouseEnterElement);
                this.ugBasisNodeVersion.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugBasisNodeVersion_DragDrop);
                this.ugBasisNodeVersion.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugBasisNodeVersion_DragEnter);
                this.ugBasisNodeVersion.BeforeCellUpdate -= new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ugBasisNodeVersion_BeforeCellUpdate);
                this.ugBasisNodeVersion.DragOver -= new System.Windows.Forms.DragEventHandler(this.ugBasisNodeVersion_DragOver);
                this.ugBasisNodeVersion.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugBasisNodeVersion_AfterCellUpdate);
                this.ugBasisNodeVersion.BeforeCellDeactivate -= new System.ComponentModel.CancelEventHandler(this.ugGrid_BeforeCellDeactivate);
                this.ugBasisNodeVersion.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugBasisNodeVersion_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(ugBasisNodeVersion);
                //End TT#169
                this.ugWorkflows.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugWorkflows_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ugWorkflows);
                //End TT#169
                this.ugSellThruPcts.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSellThruPcts_CellChange);
                this.ugSellThruPcts.AfterRowsDeleted -= new System.EventHandler(this.ugSellThruPcts_AfterRowsDeleted);
                this.ugSellThruPcts.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugSellThruPcts_MouseEnterElement);
                this.ugSellThruPcts.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugSellThruPcts_DragDrop);
                this.ugSellThruPcts.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugSellThruPcts_AfterRowInsert);
                this.ugSellThruPcts.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugSellThruPcts_DragEnter);
                // Begin TT#325 - JSmith - drag/drop method/workflow into merch field
                this.ugSellThruPcts.DragOver += new DragEventHandler(ugSellThruPcts_DragOver);
                // End TT#325 - JSmith - drag/drop method/workflow into merch field
                this.ugSellThruPcts.BeforeCellUpdate -= new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ugSellThruPcts_BeforeCellUpdate);
                this.ugSellThruPcts.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSellThruPcts_AfterCellUpdate);
                this.ugSellThruPcts.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugSellThruPcts_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ugSellThruPcts);
                //End TT#169
                this.ugSellThruPcts.AfterSortChange -= new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugSellThruPcts_AfterSortChange);
                this.ugSellThruPcts.BeforeCellDeactivate -= new System.ComponentModel.CancelEventHandler(this.ugGrid_BeforeCellDeactivate);
                this.ugVelocityGrades.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugVelocityGrades_CellChange);
                this.ugVelocityGrades.AfterRowsDeleted -= new System.EventHandler(this.ugVelocityGrades_AfterRowsDeleted);
                this.ugVelocityGrades.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugVelocityGrades_MouseEnterElement);
                this.ugVelocityGrades.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugVelocityGrades_DragDrop);
                this.ugVelocityGrades.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugVelocityGrades_AfterRowInsert);
                this.ugVelocityGrades.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugVelocityGrades_DragEnter);
                // Begin TT#325 - JSmith - drag/drop method/workflow into merch field
                this.ugVelocityGrades.DragOver -= new DragEventHandler(ugVelocityGrades_DragOver);
                // End TT#325 - JSmith - drag/drop method/workflow into merch field
                this.ugVelocityGrades.BeforeCellUpdate -= new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ugVelocityGrades_BeforeCellUpdate);
                this.ugVelocityGrades.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugVelocityGrades_AfterCellUpdate);
                this.ugVelocityGrades.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugVelocityGrades_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ugVelocityGrades);
                //End TT#169
                this.ugVelocityGrades.AfterSortChange -= new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugVelocityGrades_AfterSortChange);
                this.ugVelocityGrades.BeforeCellDeactivate -= new System.ComponentModel.CancelEventHandler(this.ugGrid_BeforeCellDeactivate);
                this.ugVelocityGrades.AfterExitEditMode -= new EventHandler(ugVelocityGrades_AfterExitEditMode);  // TT#3617 - JSmith - Velocity throws Null Ref error when a change is made to the Boundaries
                this.btnApply.Click -= new System.EventHandler(this.btnApply_Click);
                this.txtOHQuantity.TextChanged -= new System.EventHandler(this.txtOHQuantity_TextChanged);
                this.cboOHRule.SelectionChangeCommitted -= new System.EventHandler(this.cboOHRule_SelectionChangeCommitted);
                this.txtNoOHQuantity.TextChanged -= new System.EventHandler(this.txtNoOHQuantity_TextChanged);
                this.txtNoOHQuantity.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.txtNoOHQuantity_KeyUp);
                this.cboNoOHRule.SelectionChangeCommitted -= new System.EventHandler(this.cboNoOHRule_SelectionChangeCommitted);
                this.cbxAttributeSet.SelectionChangeCommitted -= new System.EventHandler(this.cbxAttributeSet_SelectionChangeCommitted);
                this.ugMatrix.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugMatrix_CellChange);
                this.ugMatrix.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugMatrix_MouseEnterElement);
                this.ugMatrix.AfterCellListCloseUp -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugMatrix_AfterCellListCloseUp);
                this.ugMatrix.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugMatrix_AfterCellUpdate);
                this.ugMatrix.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugMatrix_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ugMatrix);
                //End TT#169
                // BEGIN MID Track #3102 - Remove OTS Plan Section 
                //this.cboOTSPlan.Validating -= new System.ComponentModel.CancelEventHandler(this.cboOTSPlan_Validating);
                //this.cboOTSPlan.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboOTSPlan_DragDrop);
                //this.cboOTSPlan.SelectionChangeCommitted -= new System.EventHandler(this.cboOTSPlan_SelectionChangeCommitted);
                //this.cboOTSPlan.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboOTSPlan_DragEnter);
                //this.midDateRangeSelectorBeg.Load -= new System.EventHandler(this.midDateRangeSelectorBeg_Load);
                //this.midDateRangeSelectorBeg.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.midDateRangeSelectorBeg_ClickCellButton);
                //this.midDateRangeSelectorBeg.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.midDateRangeSelectorBeg_OnSelection);
                //this.midDateRangeSelectorShip.Load -= new System.EventHandler(this.midDateRangeSelectorShip_Load);
                //this.midDateRangeSelectorShip.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.midDateRangeSelectorShip_ClickCellButton);
                //this.midDateRangeSelectorShip.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.midDateRangeSelectorShip_OnSelection);
                // END MID Track #3102
                this.radShipBasis.CheckedChanged -= new System.EventHandler(this.radShipBasis_CheckedChanged);
                this.radHeaderStyle.CheckedChanged -= new System.EventHandler(this.radHeaderStyle_CheckedChanged);
                this.radAvgSet.CheckedChanged -= new System.EventHandler(this.radAvgSet_CheckedChanged);
                this.radAvgChain.CheckedChanged -= new System.EventHandler(this.radAvgChain_CheckedChanged);
                // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                this.radApplyMinMaxNone.CheckedChanged -= new System.EventHandler(this.radApplyMinMaxNone_CheckedChanged);
                this.radApplyMinMaxStore.CheckedChanged -= new System.EventHandler(this.radApplyMinMaxStore_CheckedChanged);
                this.radApplyMinMaxVelocity.CheckedChanged -= new System.EventHandler(this.radApplyMinMaxVelocity_CheckedChanged);
                // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                this.radAllocationMinMax.CheckedChanged -= new System.EventHandler(this.radAllocationMinMax_CheckedChanged);
                this.radInventoryMinMax.CheckedChanged -= new System.EventHandler(this.radInventoryMinMax_CheckedChanged);
                this.cboInventoryBasis.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.cboInventoryBasis_KeyDown);
                this.cboInventoryBasis.Validating -= new System.ComponentModel.CancelEventHandler(this.cboInventoryBasis_Validating);
                this.cboInventoryBasis.Validated -= new System.EventHandler(this.cboInventoryBasis_Validated);
                this.cboInventoryBasis.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragDrop);
                this.cboInventoryBasis.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragEnter);
                this.cboInventoryBasis.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragOver);
                // END TT#1287 - AGallagher - Inventory Min/Max
                // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) 
                this.cboMatrixModeAvgRule.SelectionChangeCommitted -= new System.EventHandler(this.cboMatrixModeAvgRule_SelectionChangeCommitted);
                // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                //this.rdoMatrixModeAverage.CheckedChanged -= new System.EventHandler(this.rdoMatrixModeAverage_CheckedChanged);
                //this.rdoMatrixModeNormal.CheckedChanged -= new System.EventHandler(this.rdoMatrixModeNormal_CheckedChanged);
                this.rdoMatrixModeAverage.CheckedChanged -= new System.EventHandler(this.rdoMatrixModeAverage_CheckedChanged);
                this.rdoMatrixModeNormal.CheckedChanged -= new System.EventHandler(this.rdoMatrixModeNormal_CheckedChanged);
                this.cbxBalanceToHeader.CheckedChanged -= new System.EventHandler(this.cbxBalanceToHeader_CheckedChanged); //TT#855-MD -jsobek -Velocity Enhancements
                this.radGradeVariableStock.CheckedChanged -= new System.EventHandler(this.rdoGradeVariableStock_CheckedChanged); //TT#855-MD -jsobek -Velocity Enhancements
                this.radGradeVariableSales.CheckedChanged -= new System.EventHandler(this.rdoGradeVariableSales_CheckedChanged); //TT#855-MD -jsobek -Velocity Enhancements
              
                // END TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
                // END TT#637 - AGallagher - Velocity - Spread Average (#7) 
                // BEGIN TT#960 - AGallagher - Velocity - Spread Average (#7) - Screen Processing Oversites
                this.txtMatrixModeAvgRule.TextChanged -= new System.EventHandler(this.txtMatrixModeAvgRule_TextChanged);
                this.rdoSpreadOptionIdx.CheckedChanged -= new System.EventHandler(this.rdoSpreadOptionIdx_CheckedChanged);
                this.rdoSpreadOptionSmooth.CheckedChanged -= new System.EventHandler(this.rdoSpreadOptionSmooth_CheckedChanged);
                // END TT#960 - AGallagher - Velocity - Spread Average (#7) - Screen Processing Oversites 
                this.cbxSimilarStores.CheckedChanged -= new System.EventHandler(this.cbxSimilarStores_CheckedChanged);
                this.cboStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
                this.cboStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
                this.cboStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
                // Begin MID Track 4858 - JSmith - Security changes
                //				this.btnProcess.Click -= new System.EventHandler(this.btnProcess_Click);
                //				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
                //				this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
                // End MID Track 4858
                this.mnuBasisGrid.Popup -= new System.EventHandler(this.mnuBasisGridItemInsert_Click);
                this.btnView.Click -= new System.EventHandler(this.btnView_Click);
                this.btnChanges.Click -= new System.EventHandler(this.btnChanges_Click);
                // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
                this.cboMatrixView.SelectionChangeCommitted -= new System.EventHandler(this.cboMatrixView_SelectionChangeCommitted);
                // End TT#231 
                // BEGIN TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
                this.cbxReconcile.CheckedChanged -= new System.EventHandler(this.cbxReconcile_CheckedChanged);
                this.cbxBalance.CheckedChanged -= new System.EventHandler(this.cbxBalance_CheckedChanged); 
                // END TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance

                this.cboInventoryBasis.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboInventoryBasis_MIDComboBoxPropertiesChangedEvent);
                this.cboMatrixModeAvgRule.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboMatrixModeAvgRule_MIDComboBoxPropertiesChangedEvent);
                this.cboOHRule.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboOHRule_MIDComboBoxPropertiesChangedEvent);
                this.cboNoOHRule.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboNoOHRule_MIDComboBoxPropertiesChangedEvent);
                this.cboComponent.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboComponent_MIDComboBoxPropertiesChangedEvent);
                this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);
                this.cbxAttributeSet.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cbxAttributeSet_MIDComboBoxPropertiesChangedEvent);
                this.cboMatrixView.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboMatrixView_MIDComboBoxPropertiesChangedEvent);
                if (_trans != null && _trans.AllocationCriteriaExists)
                {
                    if (_trans.StyleView == null &&
                        _trans.SummaryView == null &&
                        _trans.SizeView == null &&
                        _trans.AssortmentView == null &&
                        _trans.VelocityWindow == null)
                    {
                        _trans.DequeueHeaders(); // TT#1185 - Verify ENQ before Update
                        _trans.Dispose();
                        _trans = null;            // TT#1185 - Verify ENQ before Update
                    }
                }

                // begom TT#1195 - Verify ENQ before Update
                //if (ApplicationTransaction != null)
                //{
                //    ApplicationTransaction.Dispose();
                //}
                // end TT#1185 - Verify ENQ before Update
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            this.tabVelocityMethod = new System.Windows.Forms.TabControl();
            this.tabMethod = new System.Windows.Forms.TabPage();
            this.cbxBalanceToHeader = new System.Windows.Forms.CheckBox();
            this.gbxGrade = new System.Windows.Forms.GroupBox();
            this.radGradeVariableSales = new System.Windows.Forms.RadioButton();
            this.radGradeVariableStock = new System.Windows.Forms.RadioButton();
            this.cbxReconcile = new System.Windows.Forms.CheckBox();
            this.gbxApplyMinMax = new System.Windows.Forms.GroupBox();
            this.radApplyMinMaxVelocity = new System.Windows.Forms.RadioButton();
            this.radApplyMinMaxStore = new System.Windows.Forms.RadioButton();
            this.radApplyMinMaxNone = new System.Windows.Forms.RadioButton();
            this.cbxBalance = new System.Windows.Forms.CheckBox();
            this.gbxCriteria = new System.Windows.Forms.GroupBox();
            this.tabVelocity = new System.Windows.Forms.TabControl();
            this.tabBasis = new System.Windows.Forms.TabPage();
            this.ugBasisNodeVersion = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tabGrades = new System.Windows.Forms.TabPage();
            this.gbxMinMaxOpt = new System.Windows.Forms.GroupBox();
            this.lblInventoryBasis = new System.Windows.Forms.Label();
            this.cboInventoryBasis = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.radInventoryMinMax = new System.Windows.Forms.RadioButton();
            this.radAllocationMinMax = new System.Windows.Forms.RadioButton();
            this.ugSellThruPcts = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ugVelocityGrades = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.cbxTrendPct = new System.Windows.Forms.CheckBox();
            this.gbxShip = new System.Windows.Forms.GroupBox();
            this.radShipBasis = new System.Windows.Forms.RadioButton();
            this.radHeaderStyle = new System.Windows.Forms.RadioButton();
            this.gbxAverage = new System.Windows.Forms.GroupBox();
            this.radAvgChain = new System.Windows.Forms.RadioButton();
            this.radAvgSet = new System.Windows.Forms.RadioButton();
            this.cbxSimilarStores = new System.Windows.Forms.CheckBox();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tabMatrix = new System.Windows.Forms.TabPage();
            this.lblGA = new System.Windows.Forms.Label();
            this.gbxSpreadOption = new System.Windows.Forms.GroupBox();
            this.rdoSpreadOptionSmooth = new System.Windows.Forms.RadioButton();
            this.rdoSpreadOptionIdx = new System.Windows.Forms.RadioButton();
            this.gbxMatrixMode = new System.Windows.Forms.GroupBox();
            this.cboMatrixModeAvgRule = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.txtMatrixModeAvgRule = new System.Windows.Forms.TextBox();
            this.rdoMatrixModeAverage = new System.Windows.Forms.RadioButton();
            this.rdoMatrixModeNormal = new System.Windows.Forms.RadioButton();
            this.cboMatrixView = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.gbxMatrixDefault = new System.Windows.Forms.GroupBox();
            this.cboOHRule = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.btnApply = new System.Windows.Forms.Button();
            this.txtOHQuantity = new System.Windows.Forms.TextBox();
            this.gbxNoOH = new System.Windows.Forms.GroupBox();
            this.cboNoOHRule = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.txtNoOHQuantity = new System.Windows.Forms.TextBox();
            this.gbxActiveMatrix = new System.Windows.Forms.GroupBox();
            this.cboComponent = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblComponent = new System.Windows.Forms.Label();
            this.cbxInteractive = new System.Windows.Forms.CheckBox();
            this.ugGroupData = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.lblAttribute = new System.Windows.Forms.Label();
            this.cboStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.ugMatrix = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.lblSet = new System.Windows.Forms.Label();
            this.cbxAttributeSet = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.mnuVelocityGradesGrid = new System.Windows.Forms.ContextMenu();
            this.mnuSellThruPctsGrid = new System.Windows.Forms.ContextMenu();
            this.mnuBasisGrid = new System.Windows.Forms.ContextMenu();
            this.mnuMatrixGrid = new System.Windows.Forms.ContextMenu();
            this.mnuSalesGrid = new System.Windows.Forms.ContextMenu();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnView = new System.Windows.Forms.Button();
            this.btnChanges = new System.Windows.Forms.Button();
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.tabVelocityMethod.SuspendLayout();
            this.tabMethod.SuspendLayout();
            this.gbxGrade.SuspendLayout();
            this.gbxApplyMinMax.SuspendLayout();
            this.gbxCriteria.SuspendLayout();
            this.tabVelocity.SuspendLayout();
            this.tabBasis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugBasisNodeVersion)).BeginInit();
            this.tabGrades.SuspendLayout();
            this.gbxMinMaxOpt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugSellThruPcts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugVelocityGrades)).BeginInit();
            this.gbxShip.SuspendLayout();
            this.gbxAverage.SuspendLayout();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
            this.tabMatrix.SuspendLayout();
            this.gbxSpreadOption.SuspendLayout();
            this.gbxMatrixMode.SuspendLayout();
            this.gbxMatrixDefault.SuspendLayout();
            this.gbxNoOH.SuspendLayout();
            this.gbxActiveMatrix.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugGroupData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugMatrix)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(632, 668);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(544, 668);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(8, 668);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // tabVelocityMethod
            // 
            this.tabVelocityMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabVelocityMethod.Controls.Add(this.tabMethod);
            this.tabVelocityMethod.Controls.Add(this.tabProperties);
            this.tabVelocityMethod.Controls.Add(this.tabMatrix);
            this.tabVelocityMethod.Location = new System.Drawing.Point(8, 48);
            this.tabVelocityMethod.Name = "tabVelocityMethod";
            this.tabVelocityMethod.SelectedIndex = 0;
            this.tabVelocityMethod.Size = new System.Drawing.Size(702, 616);
            this.tabVelocityMethod.TabIndex = 19;
            this.tabVelocityMethod.SelectedIndexChanged += new System.EventHandler(this.tabVelocityMethod_SelectedIndexChanged);
            // 
            // tabMethod
            // 
            this.tabMethod.Controls.Add(this.cbxBalanceToHeader);
            this.tabMethod.Controls.Add(this.gbxGrade);
            this.tabMethod.Controls.Add(this.cbxReconcile);
            this.tabMethod.Controls.Add(this.gbxApplyMinMax);
            this.tabMethod.Controls.Add(this.cbxBalance);
            this.tabMethod.Controls.Add(this.gbxCriteria);
            this.tabMethod.Controls.Add(this.cbxTrendPct);
            this.tabMethod.Controls.Add(this.gbxShip);
            this.tabMethod.Controls.Add(this.gbxAverage);
            this.tabMethod.Controls.Add(this.cbxSimilarStores);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(694, 590);
            this.tabMethod.TabIndex = 1;
            this.tabMethod.Text = "Method";
            // 
            // cbxBalanceToHeader
            // 
            this.cbxBalanceToHeader.Location = new System.Drawing.Point(551, 53);
            this.cbxBalanceToHeader.Name = "cbxBalanceToHeader";
            this.cbxBalanceToHeader.Size = new System.Drawing.Size(137, 16);
            this.cbxBalanceToHeader.TabIndex = 54;
            this.cbxBalanceToHeader.Text = "Header Balance";
            this.cbxBalanceToHeader.CheckedChanged += new System.EventHandler(this.cbxBalanceToHeader_CheckedChanged);
            // 
            // gbxGrade
            // 
            this.gbxGrade.Controls.Add(this.radGradeVariableSales);
            this.gbxGrade.Controls.Add(this.radGradeVariableStock);
            this.gbxGrade.Location = new System.Drawing.Point(257, 55);
            this.gbxGrade.Name = "gbxGrade";
            this.gbxGrade.Size = new System.Drawing.Size(180, 42);
            this.gbxGrade.TabIndex = 53;
            this.gbxGrade.TabStop = false;
            this.gbxGrade.Text = "Grade Variable";
            // 
            // radGradeVariableSales
            // 
            this.radGradeVariableSales.Checked = true;
            this.radGradeVariableSales.Location = new System.Drawing.Point(16, 16);
            this.radGradeVariableSales.Name = "radGradeVariableSales";
            this.radGradeVariableSales.Size = new System.Drawing.Size(64, 16);
            this.radGradeVariableSales.TabIndex = 0;
            this.radGradeVariableSales.TabStop = true;
            this.radGradeVariableSales.Text = "Sales";
            this.radGradeVariableSales.CheckedChanged += new System.EventHandler(this.rdoGradeVariableSales_CheckedChanged);
            // 
            // radGradeVariableStock
            // 
            this.radGradeVariableStock.Location = new System.Drawing.Point(84, 16);
            this.radGradeVariableStock.Name = "radGradeVariableStock";
            this.radGradeVariableStock.Size = new System.Drawing.Size(88, 16);
            this.radGradeVariableStock.TabIndex = 1;
            this.radGradeVariableStock.Text = "Stock";
            this.radGradeVariableStock.CheckedChanged += new System.EventHandler(this.rdoGradeVariableStock_CheckedChanged);
            // 
            // cbxReconcile
            // 
            this.cbxReconcile.AutoSize = true;
            this.cbxReconcile.Location = new System.Drawing.Point(551, 73);
            this.cbxReconcile.Name = "cbxReconcile";
            this.cbxReconcile.Size = new System.Drawing.Size(77, 17);
            this.cbxReconcile.TabIndex = 52;
            this.cbxReconcile.Text = "Reconcile:";
            this.cbxReconcile.UseVisualStyleBackColor = true;
            this.cbxReconcile.CheckedChanged += new System.EventHandler(this.cbxReconcile_CheckedChanged);
            // 
            // gbxApplyMinMax
            // 
            this.gbxApplyMinMax.Controls.Add(this.radApplyMinMaxVelocity);
            this.gbxApplyMinMax.Controls.Add(this.radApplyMinMaxStore);
            this.gbxApplyMinMax.Controls.Add(this.radApplyMinMaxNone);
            this.gbxApplyMinMax.Location = new System.Drawing.Point(257, 5);
            this.gbxApplyMinMax.Name = "gbxApplyMinMax";
            this.gbxApplyMinMax.Size = new System.Drawing.Size(278, 45);
            this.gbxApplyMinMax.TabIndex = 51;
            this.gbxApplyMinMax.TabStop = false;
            this.gbxApplyMinMax.Text = "Apply Min/Max:";
            // 
            // radApplyMinMaxVelocity
            // 
            this.radApplyMinMaxVelocity.AutoSize = true;
            this.radApplyMinMaxVelocity.Location = new System.Drawing.Point(174, 19);
            this.radApplyMinMaxVelocity.Name = "radApplyMinMaxVelocity";
            this.radApplyMinMaxVelocity.Size = new System.Drawing.Size(97, 17);
            this.radApplyMinMaxVelocity.TabIndex = 2;
            this.radApplyMinMaxVelocity.TabStop = true;
            this.radApplyMinMaxVelocity.Text = "Velocity Grade:";
            this.radApplyMinMaxVelocity.UseVisualStyleBackColor = true;
            this.radApplyMinMaxVelocity.CheckedChanged += new System.EventHandler(this.radApplyMinMaxVelocity_CheckedChanged);
            // 
            // radApplyMinMaxStore
            // 
            this.radApplyMinMaxStore.AutoSize = true;
            this.radApplyMinMaxStore.Location = new System.Drawing.Point(74, 19);
            this.radApplyMinMaxStore.Name = "radApplyMinMaxStore";
            this.radApplyMinMaxStore.Size = new System.Drawing.Size(85, 17);
            this.radApplyMinMaxStore.TabIndex = 1;
            this.radApplyMinMaxStore.TabStop = true;
            this.radApplyMinMaxStore.Text = "Store Grade:";
            this.radApplyMinMaxStore.UseVisualStyleBackColor = true;
            this.radApplyMinMaxStore.CheckedChanged += new System.EventHandler(this.radApplyMinMaxStore_CheckedChanged);
            // 
            // radApplyMinMaxNone
            // 
            this.radApplyMinMaxNone.AutoSize = true;
            this.radApplyMinMaxNone.Location = new System.Drawing.Point(6, 19);
            this.radApplyMinMaxNone.Name = "radApplyMinMaxNone";
            this.radApplyMinMaxNone.Size = new System.Drawing.Size(54, 17);
            this.radApplyMinMaxNone.TabIndex = 0;
            this.radApplyMinMaxNone.TabStop = true;
            this.radApplyMinMaxNone.Text = "None:";
            this.radApplyMinMaxNone.UseVisualStyleBackColor = true;
            this.radApplyMinMaxNone.CheckedChanged += new System.EventHandler(this.radApplyMinMaxNone_CheckedChanged);
            // 
            // cbxBalance
            // 
            this.cbxBalance.Location = new System.Drawing.Point(551, 33);
            this.cbxBalance.Name = "cbxBalance";
            this.cbxBalance.Size = new System.Drawing.Size(104, 16);
            this.cbxBalance.TabIndex = 35;
            this.cbxBalance.Text = "Balance";
            this.cbxBalance.CheckedChanged += new System.EventHandler(this.cbxBalance_CheckedChanged);
            // 
            // gbxCriteria
            // 
            this.gbxCriteria.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxCriteria.Controls.Add(this.tabVelocity);
            this.gbxCriteria.Location = new System.Drawing.Point(12, 104);
            this.gbxCriteria.Name = "gbxCriteria";
            this.gbxCriteria.Size = new System.Drawing.Size(676, 480);
            this.gbxCriteria.TabIndex = 34;
            this.gbxCriteria.TabStop = false;
            this.gbxCriteria.Text = "Criteria";
            // 
            // tabVelocity
            // 
            this.tabVelocity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabVelocity.Controls.Add(this.tabBasis);
            this.tabVelocity.Controls.Add(this.tabGrades);
            this.tabVelocity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabVelocity.ItemSize = new System.Drawing.Size(46, 18);
            this.tabVelocity.Location = new System.Drawing.Point(8, 16);
            this.tabVelocity.Name = "tabVelocity";
            this.tabVelocity.SelectedIndex = 0;
            this.tabVelocity.Size = new System.Drawing.Size(658, 456);
            this.tabVelocity.TabIndex = 9;
            this.tabVelocity.SelectedIndexChanged += new System.EventHandler(this.tabVelocity_SelectedIndexChanged);
            this.tabVelocity.DragEnter += new System.Windows.Forms.DragEventHandler(this.tabVelocity_DragEnter);
            this.tabVelocity.DragOver += new System.Windows.Forms.DragEventHandler(this.tabVelocity_DragOver);
            // 
            // tabBasis
            // 
            this.tabBasis.Controls.Add(this.ugBasisNodeVersion);
            this.tabBasis.Location = new System.Drawing.Point(4, 22);
            this.tabBasis.Name = "tabBasis";
            this.tabBasis.Size = new System.Drawing.Size(650, 430);
            this.tabBasis.TabIndex = 0;
            this.tabBasis.Text = "Basis";
            // 
            // ugBasisNodeVersion
            // 
            this.ugBasisNodeVersion.AllowDrop = true;
            this.ugBasisNodeVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugBasisNodeVersion.DisplayLayout.Appearance = appearance1;
            this.ugBasisNodeVersion.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ugBasisNodeVersion.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugBasisNodeVersion.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugBasisNodeVersion.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugBasisNodeVersion.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ugBasisNodeVersion.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugBasisNodeVersion.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ugBasisNodeVersion.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ugBasisNodeVersion.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugBasisNodeVersion.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugBasisNodeVersion.Location = new System.Drawing.Point(27, 16);
            this.ugBasisNodeVersion.Name = "ugBasisNodeVersion";
            this.ugBasisNodeVersion.Size = new System.Drawing.Size(600, 396);
            this.ugBasisNodeVersion.TabIndex = 1;
            this.ugBasisNodeVersion.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugBasisNodeVersion_AfterCellUpdate);
            this.ugBasisNodeVersion.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugBasisNodeVersion_InitializeLayout);
            this.ugBasisNodeVersion.AfterRowsDeleted += new System.EventHandler(this.ugBasisNodeVersion_AfterRowsDeleted);
            this.ugBasisNodeVersion.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugBasisNodeVersion_CellChange);
            this.ugBasisNodeVersion.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugBasisNodeVersion_ClickCellButton);
            this.ugBasisNodeVersion.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.ugGrid_BeforeCellDeactivate);
            this.ugBasisNodeVersion.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ugBasisNodeVersion_BeforeCellUpdate);
            this.ugBasisNodeVersion.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugBasisNodeVersion_MouseEnterElement);
            this.ugBasisNodeVersion.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugBasisNodeVersion_DragDrop);
            this.ugBasisNodeVersion.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugBasisNodeVersion_DragEnter);
            this.ugBasisNodeVersion.DragOver += new System.Windows.Forms.DragEventHandler(this.ugBasisNodeVersion_DragOver);
            // 
            // tabGrades
            // 
            this.tabGrades.Controls.Add(this.gbxMinMaxOpt);
            this.tabGrades.Controls.Add(this.ugSellThruPcts);
            this.tabGrades.Controls.Add(this.ugVelocityGrades);
            this.tabGrades.Location = new System.Drawing.Point(4, 22);
            this.tabGrades.Name = "tabGrades";
            this.tabGrades.Size = new System.Drawing.Size(650, 430);
            this.tabGrades.TabIndex = 1;
            this.tabGrades.Text = "Grades";
            this.tabGrades.Visible = false;
            // 
            // gbxMinMaxOpt
            // 
            this.gbxMinMaxOpt.Controls.Add(this.lblInventoryBasis);
            this.gbxMinMaxOpt.Controls.Add(this.cboInventoryBasis);
            this.gbxMinMaxOpt.Controls.Add(this.radInventoryMinMax);
            this.gbxMinMaxOpt.Controls.Add(this.radAllocationMinMax);
            this.gbxMinMaxOpt.Location = new System.Drawing.Point(64, 6);
            this.gbxMinMaxOpt.Name = "gbxMinMaxOpt";
            this.gbxMinMaxOpt.Size = new System.Drawing.Size(578, 63);
            this.gbxMinMaxOpt.TabIndex = 41;
            this.gbxMinMaxOpt.TabStop = false;
            this.gbxMinMaxOpt.Text = "Min/Max Options:";
            // 
            // lblInventoryBasis
            // 
            this.lblInventoryBasis.AutoSize = true;
            this.lblInventoryBasis.Location = new System.Drawing.Point(207, 39);
            this.lblInventoryBasis.Name = "lblInventoryBasis";
            this.lblInventoryBasis.Size = new System.Drawing.Size(82, 13);
            this.lblInventoryBasis.TabIndex = 3;
            this.lblInventoryBasis.Text = "Inventory Basis:";
            // 
            // cboInventoryBasis
            // 
            this.cboInventoryBasis.AllowDrop = true;
            this.cboInventoryBasis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboInventoryBasis.AutoAdjust = true;
            this.cboInventoryBasis.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboInventoryBasis.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboInventoryBasis.DataSource = null;
            this.cboInventoryBasis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cboInventoryBasis.DropDownWidth = 179;
            this.cboInventoryBasis.FormattingEnabled = false;
            this.cboInventoryBasis.IgnoreFocusLost = true;
            this.cboInventoryBasis.ItemHeight = 13;
            this.cboInventoryBasis.Location = new System.Drawing.Point(295, 36);
            this.cboInventoryBasis.Margin = new System.Windows.Forms.Padding(0);
            this.cboInventoryBasis.MaxDropDownItems = 25;
            this.cboInventoryBasis.Name = "cboInventoryBasis";
            this.cboInventoryBasis.SetToolTip = "";
            this.cboInventoryBasis.Size = new System.Drawing.Size(179, 21);
            this.cboInventoryBasis.TabIndex = 2;
            this.cboInventoryBasis.Tag = null;
            this.cboInventoryBasis.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboInventoryBasis_MIDComboBoxPropertiesChangedEvent);
            this.cboInventoryBasis.SelectionChangeCommitted += new System.EventHandler(this.cboInventoryBasis_SelectionChangeCommitted);
            this.cboInventoryBasis.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragDrop);
            this.cboInventoryBasis.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragEnter);
            this.cboInventoryBasis.DragOver += new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragOver);
            this.cboInventoryBasis.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboInventoryBasis_KeyDown);
            this.cboInventoryBasis.Validating += new System.ComponentModel.CancelEventHandler(this.cboInventoryBasis_Validating);
            this.cboInventoryBasis.Validated += new System.EventHandler(this.cboInventoryBasis_Validated);
            // 
            // radInventoryMinMax
            // 
            this.radInventoryMinMax.AutoSize = true;
            this.radInventoryMinMax.Location = new System.Drawing.Point(60, 38);
            this.radInventoryMinMax.Name = "radInventoryMinMax";
            this.radInventoryMinMax.Size = new System.Drawing.Size(117, 17);
            this.radInventoryMinMax.TabIndex = 1;
            this.radInventoryMinMax.TabStop = true;
            this.radInventoryMinMax.Text = "Inventory Min/Max:";
            this.radInventoryMinMax.UseVisualStyleBackColor = true;
            this.radInventoryMinMax.CheckedChanged += new System.EventHandler(this.radInventoryMinMax_CheckedChanged);
            // 
            // radAllocationMinMax
            // 
            this.radAllocationMinMax.AutoSize = true;
            this.radAllocationMinMax.Location = new System.Drawing.Point(60, 14);
            this.radAllocationMinMax.Name = "radAllocationMinMax";
            this.radAllocationMinMax.Size = new System.Drawing.Size(119, 17);
            this.radAllocationMinMax.TabIndex = 0;
            this.radAllocationMinMax.TabStop = true;
            this.radAllocationMinMax.Text = "Allocation Min/Max:";
            this.radAllocationMinMax.UseVisualStyleBackColor = true;
            this.radAllocationMinMax.CheckedChanged += new System.EventHandler(this.radAllocationMinMax_CheckedChanged);
            // 
            // ugSellThruPcts
            // 
            this.ugSellThruPcts.AllowDrop = true;
            this.ugSellThruPcts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugSellThruPcts.DisplayLayout.Appearance = appearance7;
            this.ugSellThruPcts.DisplayLayout.InterBandSpacing = 10;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.ugSellThruPcts.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.TextHAlignAsString = "Left";
            appearance9.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugSellThruPcts.DisplayLayout.Override.HeaderAppearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugSellThruPcts.DisplayLayout.Override.RowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugSellThruPcts.DisplayLayout.Override.RowSelectorAppearance = appearance11;
            this.ugSellThruPcts.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugSellThruPcts.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.ugSellThruPcts.DisplayLayout.Override.SelectedRowAppearance = appearance12;
            this.ugSellThruPcts.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugSellThruPcts.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugSellThruPcts.Location = new System.Drawing.Point(482, 75);
            this.ugSellThruPcts.Name = "ugSellThruPcts";
            this.ugSellThruPcts.Size = new System.Drawing.Size(160, 349);
            this.ugSellThruPcts.TabIndex = 40;
            this.ugSellThruPcts.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSellThruPcts_AfterCellUpdate);
            this.ugSellThruPcts.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugSellThruPcts_InitializeLayout);
            this.ugSellThruPcts.AfterRowsDeleted += new System.EventHandler(this.ugSellThruPcts_AfterRowsDeleted);
            this.ugSellThruPcts.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugSellThruPcts_AfterRowInsert);
            this.ugSellThruPcts.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSellThruPcts_CellChange);
            this.ugSellThruPcts.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.ugGrid_BeforeCellDeactivate);
            this.ugSellThruPcts.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ugSellThruPcts_BeforeCellUpdate);
            this.ugSellThruPcts.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ugSellThruPcts_BeforeRowsDeleted);
            this.ugSellThruPcts.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugSellThruPcts_AfterSortChange);
            this.ugSellThruPcts.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugSellThruPcts_MouseEnterElement);
            this.ugSellThruPcts.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugSellThruPcts_DragDrop);
            this.ugSellThruPcts.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugSellThruPcts_DragEnter);
            this.ugSellThruPcts.DragOver += new System.Windows.Forms.DragEventHandler(this.ugSellThruPcts_DragOver);
            // 
            // ugVelocityGrades
            // 
            this.ugVelocityGrades.AllowDrop = true;
            this.ugVelocityGrades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugVelocityGrades.DisplayLayout.Appearance = appearance13;
            this.ugVelocityGrades.DisplayLayout.InterBandSpacing = 10;
            appearance14.BackColor = System.Drawing.Color.Transparent;
            this.ugVelocityGrades.DisplayLayout.Override.CardAreaAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.ForeColor = System.Drawing.Color.Black;
            appearance15.TextHAlignAsString = "Left";
            appearance15.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugVelocityGrades.DisplayLayout.Override.HeaderAppearance = appearance15;
            appearance16.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugVelocityGrades.DisplayLayout.Override.RowAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugVelocityGrades.DisplayLayout.Override.RowSelectorAppearance = appearance17;
            this.ugVelocityGrades.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugVelocityGrades.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.ForeColor = System.Drawing.Color.Black;
            this.ugVelocityGrades.DisplayLayout.Override.SelectedRowAppearance = appearance18;
            this.ugVelocityGrades.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugVelocityGrades.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugVelocityGrades.Location = new System.Drawing.Point(64, 75);
            this.ugVelocityGrades.Name = "ugVelocityGrades";
            this.ugVelocityGrades.Size = new System.Drawing.Size(403, 349);
            this.ugVelocityGrades.TabIndex = 39;
            this.ugVelocityGrades.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugVelocityGrades_AfterCellUpdate);
            this.ugVelocityGrades.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugVelocityGrades_InitializeLayout);
            this.ugVelocityGrades.AfterExitEditMode += new System.EventHandler(this.ugVelocityGrades_AfterExitEditMode);
            this.ugVelocityGrades.AfterRowsDeleted += new System.EventHandler(this.ugVelocityGrades_AfterRowsDeleted);
            this.ugVelocityGrades.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugVelocityGrades_AfterRowInsert);
            this.ugVelocityGrades.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugVelocityGrades_CellChange);
            this.ugVelocityGrades.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.ugGrid_BeforeCellDeactivate);
            this.ugVelocityGrades.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ugVelocityGrades_BeforeCellUpdate);
            this.ugVelocityGrades.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ugVelocityGrades_BeforeRowsDeleted);
            this.ugVelocityGrades.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugVelocityGrades_AfterSortChange);
            this.ugVelocityGrades.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugVelocityGrades_MouseEnterElement);
            this.ugVelocityGrades.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugVelocityGrades_DragDrop);
            this.ugVelocityGrades.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugVelocityGrades_DragEnter);
            this.ugVelocityGrades.DragOver += new System.Windows.Forms.DragEventHandler(this.ugVelocityGrades_DragOver);
            // 
            // cbxTrendPct
            // 
            this.cbxTrendPct.Location = new System.Drawing.Point(463, 71);
            this.cbxTrendPct.Name = "cbxTrendPct";
            this.cbxTrendPct.Size = new System.Drawing.Size(72, 16);
            this.cbxTrendPct.TabIndex = 30;
            this.cbxTrendPct.Text = "Trend %";
            // 
            // gbxShip
            // 
            this.gbxShip.Controls.Add(this.radShipBasis);
            this.gbxShip.Controls.Add(this.radHeaderStyle);
            this.gbxShip.Location = new System.Drawing.Point(51, 55);
            this.gbxShip.Name = "gbxShip";
            this.gbxShip.Size = new System.Drawing.Size(178, 42);
            this.gbxShip.TabIndex = 29;
            this.gbxShip.TabStop = false;
            this.gbxShip.Text = "Ship ";
            // 
            // radShipBasis
            // 
            this.radShipBasis.Location = new System.Drawing.Point(16, 16);
            this.radShipBasis.Name = "radShipBasis";
            this.radShipBasis.Size = new System.Drawing.Size(64, 16);
            this.radShipBasis.TabIndex = 0;
            this.radShipBasis.Text = "Basis";
            this.radShipBasis.CheckedChanged += new System.EventHandler(this.radShipBasis_CheckedChanged);
            // 
            // radHeaderStyle
            // 
            this.radHeaderStyle.Location = new System.Drawing.Point(84, 16);
            this.radHeaderStyle.Name = "radHeaderStyle";
            this.radHeaderStyle.Size = new System.Drawing.Size(88, 16);
            this.radHeaderStyle.TabIndex = 1;
            this.radHeaderStyle.Text = "Header Style";
            this.radHeaderStyle.CheckedChanged += new System.EventHandler(this.radHeaderStyle_CheckedChanged);
            // 
            // gbxAverage
            // 
            this.gbxAverage.Controls.Add(this.radAvgChain);
            this.gbxAverage.Controls.Add(this.radAvgSet);
            this.gbxAverage.Location = new System.Drawing.Point(53, 5);
            this.gbxAverage.Name = "gbxAverage";
            this.gbxAverage.Size = new System.Drawing.Size(176, 45);
            this.gbxAverage.TabIndex = 28;
            this.gbxAverage.TabStop = false;
            this.gbxAverage.Text = "Average";
            this.gbxAverage.Enter += new System.EventHandler(this.gbxAverage_Enter);
            // 
            // radAvgChain
            // 
            this.radAvgChain.Location = new System.Drawing.Point(16, 16);
            this.radAvgChain.Name = "radAvgChain";
            this.radAvgChain.Size = new System.Drawing.Size(72, 16);
            this.radAvgChain.TabIndex = 26;
            this.radAvgChain.Text = "All Stores";
            this.radAvgChain.CheckedChanged += new System.EventHandler(this.radAvgChain_CheckedChanged);
            // 
            // radAvgSet
            // 
            this.radAvgSet.Location = new System.Drawing.Point(96, 16);
            this.radAvgSet.Name = "radAvgSet";
            this.radAvgSet.Size = new System.Drawing.Size(50, 16);
            this.radAvgSet.TabIndex = 27;
            this.radAvgSet.Text = "Set";
            this.radAvgSet.CheckedChanged += new System.EventHandler(this.radAvgSet_CheckedChanged);
            // 
            // cbxSimilarStores
            // 
            this.cbxSimilarStores.Location = new System.Drawing.Point(551, 13);
            this.cbxSimilarStores.Name = "cbxSimilarStores";
            this.cbxSimilarStores.Size = new System.Drawing.Size(104, 16);
            this.cbxSimilarStores.TabIndex = 25;
            this.cbxSimilarStores.Text = "Similar Stores";
            this.cbxSimilarStores.CheckedChanged += new System.EventHandler(this.cbxSimilarStores_CheckedChanged);
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.ugWorkflows);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(694, 590);
            this.tabProperties.TabIndex = 0;
            this.tabProperties.Text = "Properties";
            this.tabProperties.Visible = false;
            // 
            // ugWorkflows
            // 
            this.ugWorkflows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance19.BackColor = System.Drawing.Color.White;
            appearance19.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugWorkflows.DisplayLayout.Appearance = appearance19;
            this.ugWorkflows.DisplayLayout.InterBandSpacing = 10;
            appearance20.BackColor = System.Drawing.Color.Transparent;
            this.ugWorkflows.DisplayLayout.Override.CardAreaAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance21.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance21.ForeColor = System.Drawing.Color.Black;
            appearance21.TextHAlignAsString = "Left";
            appearance21.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugWorkflows.DisplayLayout.Override.HeaderAppearance = appearance21;
            appearance22.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.Override.RowAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance23.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorAppearance = appearance23;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugWorkflows.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance24.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance24.ForeColor = System.Drawing.Color.Black;
            this.ugWorkflows.DisplayLayout.Override.SelectedRowAppearance = appearance24;
            this.ugWorkflows.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugWorkflows.Location = new System.Drawing.Point(23, 19);
            this.ugWorkflows.Name = "ugWorkflows";
            this.ugWorkflows.Size = new System.Drawing.Size(616, 536);
            this.ugWorkflows.TabIndex = 3;
            this.ugWorkflows.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugWorkflows_InitializeLayout);
            // 
            // tabMatrix
            // 
            this.tabMatrix.Controls.Add(this.lblGA);
            this.tabMatrix.Controls.Add(this.gbxSpreadOption);
            this.tabMatrix.Controls.Add(this.gbxMatrixMode);
            this.tabMatrix.Controls.Add(this.cboMatrixView);
            this.tabMatrix.Controls.Add(this.gbxMatrixDefault);
            this.tabMatrix.Controls.Add(this.gbxNoOH);
            this.tabMatrix.Controls.Add(this.gbxActiveMatrix);
            this.tabMatrix.Controls.Add(this.lblAttribute);
            this.tabMatrix.Controls.Add(this.cboStoreAttribute);
            this.tabMatrix.Controls.Add(this.ugMatrix);
            this.tabMatrix.Controls.Add(this.lblSet);
            this.tabMatrix.Controls.Add(this.cbxAttributeSet);
            this.tabMatrix.Location = new System.Drawing.Point(4, 22);
            this.tabMatrix.Name = "tabMatrix";
            this.tabMatrix.Size = new System.Drawing.Size(694, 590);
            this.tabMatrix.TabIndex = 2;
            this.tabMatrix.Text = "Matrix";
            // 
            // lblGA
            // 
            this.lblGA.AutoSize = true;
            this.lblGA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGA.ForeColor = System.Drawing.Color.Red;
            this.lblGA.Location = new System.Drawing.Point(284, 11);
            this.lblGA.Name = "lblGA";
            this.lblGA.Size = new System.Drawing.Size(59, 13);
            this.lblGA.TabIndex = 52;
            this.lblGA.Text = "GA Mode";
            // 
            // gbxSpreadOption
            // 
            this.gbxSpreadOption.Controls.Add(this.rdoSpreadOptionSmooth);
            this.gbxSpreadOption.Controls.Add(this.rdoSpreadOptionIdx);
            this.gbxSpreadOption.Enabled = false;
            this.gbxSpreadOption.Location = new System.Drawing.Point(407, 147);
            this.gbxSpreadOption.Name = "gbxSpreadOption";
            this.gbxSpreadOption.Size = new System.Drawing.Size(233, 49);
            this.gbxSpreadOption.TabIndex = 51;
            this.gbxSpreadOption.TabStop = false;
            this.gbxSpreadOption.Text = "Spread Option:";
            // 
            // rdoSpreadOptionSmooth
            // 
            this.rdoSpreadOptionSmooth.AutoSize = true;
            this.rdoSpreadOptionSmooth.Checked = true;
            this.rdoSpreadOptionSmooth.Location = new System.Drawing.Point(123, 18);
            this.rdoSpreadOptionSmooth.Name = "rdoSpreadOptionSmooth";
            this.rdoSpreadOptionSmooth.Size = new System.Drawing.Size(64, 17);
            this.rdoSpreadOptionSmooth.TabIndex = 3;
            this.rdoSpreadOptionSmooth.TabStop = true;
            this.rdoSpreadOptionSmooth.Text = "Smooth:";
            this.rdoSpreadOptionSmooth.UseVisualStyleBackColor = true;
            this.rdoSpreadOptionSmooth.CheckedChanged += new System.EventHandler(this.rdoSpreadOptionSmooth_CheckedChanged);
            // 
            // rdoSpreadOptionIdx
            // 
            this.rdoSpreadOptionIdx.AutoSize = true;
            this.rdoSpreadOptionIdx.Location = new System.Drawing.Point(13, 18);
            this.rdoSpreadOptionIdx.Name = "rdoSpreadOptionIdx";
            this.rdoSpreadOptionIdx.Size = new System.Drawing.Size(104, 17);
            this.rdoSpreadOptionIdx.TabIndex = 2;
            this.rdoSpreadOptionIdx.Text = "Spread by index:";
            this.rdoSpreadOptionIdx.UseVisualStyleBackColor = true;
            this.rdoSpreadOptionIdx.CheckedChanged += new System.EventHandler(this.rdoSpreadOptionIdx_CheckedChanged);
            // 
            // gbxMatrixMode
            // 
            this.gbxMatrixMode.Controls.Add(this.cboMatrixModeAvgRule);
            this.gbxMatrixMode.Controls.Add(this.txtMatrixModeAvgRule);
            this.gbxMatrixMode.Controls.Add(this.rdoMatrixModeAverage);
            this.gbxMatrixMode.Controls.Add(this.rdoMatrixModeNormal);
            this.gbxMatrixMode.Location = new System.Drawing.Point(16, 147);
            this.gbxMatrixMode.Name = "gbxMatrixMode";
            this.gbxMatrixMode.Size = new System.Drawing.Size(388, 49);
            this.gbxMatrixMode.TabIndex = 50;
            this.gbxMatrixMode.TabStop = false;
            this.gbxMatrixMode.Text = "Matrix Mode:";
            // 
            // cboMatrixModeAvgRule
            // 
            this.cboMatrixModeAvgRule.AutoAdjust = true;
            this.cboMatrixModeAvgRule.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboMatrixModeAvgRule.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboMatrixModeAvgRule.DataSource = null;
            this.cboMatrixModeAvgRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMatrixModeAvgRule.DropDownWidth = 152;
            this.cboMatrixModeAvgRule.FormattingEnabled = false;
            this.cboMatrixModeAvgRule.IgnoreFocusLost = false;
            this.cboMatrixModeAvgRule.ItemHeight = 13;
            this.cboMatrixModeAvgRule.Location = new System.Drawing.Point(157, 16);
            this.cboMatrixModeAvgRule.Margin = new System.Windows.Forms.Padding(0);
            this.cboMatrixModeAvgRule.MaxDropDownItems = 25;
            this.cboMatrixModeAvgRule.Name = "cboMatrixModeAvgRule";
            this.cboMatrixModeAvgRule.SetToolTip = "";
            this.cboMatrixModeAvgRule.Size = new System.Drawing.Size(152, 21);
            this.cboMatrixModeAvgRule.TabIndex = 43;
            this.cboMatrixModeAvgRule.Tag = null;
            this.cboMatrixModeAvgRule.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboMatrixModeAvgRule_MIDComboBoxPropertiesChangedEvent);
            this.cboMatrixModeAvgRule.SelectionChangeCommitted += new System.EventHandler(this.cboMatrixModeAvgRule_SelectionChangeCommitted);
            // 
            // txtMatrixModeAvgRule
            // 
            this.txtMatrixModeAvgRule.Enabled = false;
            this.txtMatrixModeAvgRule.Location = new System.Drawing.Point(313, 16);
            this.txtMatrixModeAvgRule.MaxLength = 6;
            this.txtMatrixModeAvgRule.Name = "txtMatrixModeAvgRule";
            this.txtMatrixModeAvgRule.Size = new System.Drawing.Size(64, 20);
            this.txtMatrixModeAvgRule.TabIndex = 44;
            this.txtMatrixModeAvgRule.TextChanged += new System.EventHandler(this.txtMatrixModeAvgRule_TextChanged);
            // 
            // rdoMatrixModeAverage
            // 
            this.rdoMatrixModeAverage.AutoSize = true;
            this.rdoMatrixModeAverage.Location = new System.Drawing.Point(84, 18);
            this.rdoMatrixModeAverage.Name = "rdoMatrixModeAverage";
            this.rdoMatrixModeAverage.Size = new System.Drawing.Size(68, 17);
            this.rdoMatrixModeAverage.TabIndex = 1;
            this.rdoMatrixModeAverage.Text = "Average:";
            this.rdoMatrixModeAverage.UseVisualStyleBackColor = true;
            this.rdoMatrixModeAverage.CheckedChanged += new System.EventHandler(this.rdoMatrixModeAverage_CheckedChanged);
            // 
            // rdoMatrixModeNormal
            // 
            this.rdoMatrixModeNormal.AutoSize = true;
            this.rdoMatrixModeNormal.Checked = true;
            this.rdoMatrixModeNormal.Location = new System.Drawing.Point(13, 18);
            this.rdoMatrixModeNormal.Name = "rdoMatrixModeNormal";
            this.rdoMatrixModeNormal.Size = new System.Drawing.Size(61, 17);
            this.rdoMatrixModeNormal.TabIndex = 0;
            this.rdoMatrixModeNormal.TabStop = true;
            this.rdoMatrixModeNormal.Text = "Normal:";
            this.rdoMatrixModeNormal.UseVisualStyleBackColor = true;
            this.rdoMatrixModeNormal.CheckedChanged += new System.EventHandler(this.rdoMatrixModeNormal_CheckedChanged);
            // 
            // cboMatrixView
            // 
            this.cboMatrixView.AutoAdjust = true;
            this.cboMatrixView.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboMatrixView.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboMatrixView.DataSource = null;
            this.cboMatrixView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMatrixView.DropDownWidth = 184;
            this.cboMatrixView.FormattingEnabled = false;
            this.cboMatrixView.IgnoreFocusLost = false;
            this.cboMatrixView.ItemHeight = 13;
            this.cboMatrixView.Location = new System.Drawing.Point(16, 213);
            this.cboMatrixView.Margin = new System.Windows.Forms.Padding(0);
            this.cboMatrixView.MaxDropDownItems = 25;
            this.cboMatrixView.Name = "cboMatrixView";
            this.cboMatrixView.SetToolTip = "";
            this.cboMatrixView.Size = new System.Drawing.Size(184, 21);
            this.cboMatrixView.TabIndex = 49;
            this.cboMatrixView.Tag = null;
            this.cboMatrixView.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboMatrixView_MIDComboBoxPropertiesChangedEvent);
            this.cboMatrixView.SelectionChangeCommitted += new System.EventHandler(this.cboMatrixView_SelectionChangeCommitted);
            // 
            // gbxMatrixDefault
            // 
            this.gbxMatrixDefault.Controls.Add(this.cboOHRule);
            this.gbxMatrixDefault.Controls.Add(this.btnApply);
            this.gbxMatrixDefault.Controls.Add(this.txtOHQuantity);
            this.gbxMatrixDefault.Location = new System.Drawing.Point(16, 90);
            this.gbxMatrixDefault.Name = "gbxMatrixDefault";
            this.gbxMatrixDefault.Size = new System.Drawing.Size(302, 52);
            this.gbxMatrixDefault.TabIndex = 48;
            this.gbxMatrixDefault.TabStop = false;
            this.gbxMatrixDefault.Text = "Matrix Default";
            // 
            // cboOHRule
            // 
            this.cboOHRule.AutoAdjust = true;
            this.cboOHRule.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboOHRule.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboOHRule.DataSource = null;
            this.cboOHRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOHRule.DropDownWidth = 152;
            this.cboOHRule.FormattingEnabled = false;
            this.cboOHRule.IgnoreFocusLost = false;
            this.cboOHRule.ItemHeight = 13;
            this.cboOHRule.Location = new System.Drawing.Point(64, 18);
            this.cboOHRule.Margin = new System.Windows.Forms.Padding(0);
            this.cboOHRule.MaxDropDownItems = 25;
            this.cboOHRule.Name = "cboOHRule";
            this.cboOHRule.SetToolTip = "";
            this.cboOHRule.Size = new System.Drawing.Size(152, 21);
            this.cboOHRule.TabIndex = 37;
            this.cboOHRule.Tag = null;
            this.cboOHRule.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboOHRule_MIDComboBoxPropertiesChangedEvent);
            this.cboOHRule.SelectionChangeCommitted += new System.EventHandler(this.cboOHRule_SelectionChangeCommitted);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(8, 18);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(48, 21);
            this.btnApply.TabIndex = 39;
            this.btnApply.Text = "Apply";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // txtOHQuantity
            // 
            this.txtOHQuantity.Location = new System.Drawing.Point(220, 18);
            this.txtOHQuantity.MaxLength = 6;
            this.txtOHQuantity.Name = "txtOHQuantity";
            this.txtOHQuantity.Size = new System.Drawing.Size(64, 20);
            this.txtOHQuantity.TabIndex = 38;
            this.txtOHQuantity.TextChanged += new System.EventHandler(this.txtOHQuantity_TextChanged);
            // 
            // gbxNoOH
            // 
            this.gbxNoOH.Controls.Add(this.cboNoOHRule);
            this.gbxNoOH.Controls.Add(this.txtNoOHQuantity);
            this.gbxNoOH.Location = new System.Drawing.Point(16, 34);
            this.gbxNoOH.Name = "gbxNoOH";
            this.gbxNoOH.Size = new System.Drawing.Size(302, 52);
            this.gbxNoOH.TabIndex = 47;
            this.gbxNoOH.TabStop = false;
            this.gbxNoOH.Text = "Stores with No On Hand";
            // 
            // cboNoOHRule
            // 
            this.cboNoOHRule.AutoAdjust = true;
            this.cboNoOHRule.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboNoOHRule.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboNoOHRule.DataSource = null;
            this.cboNoOHRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNoOHRule.DropDownWidth = 152;
            this.cboNoOHRule.FormattingEnabled = false;
            this.cboNoOHRule.IgnoreFocusLost = false;
            this.cboNoOHRule.ItemHeight = 13;
            this.cboNoOHRule.Location = new System.Drawing.Point(64, 18);
            this.cboNoOHRule.Margin = new System.Windows.Forms.Padding(0);
            this.cboNoOHRule.MaxDropDownItems = 25;
            this.cboNoOHRule.Name = "cboNoOHRule";
            this.cboNoOHRule.SetToolTip = "";
            this.cboNoOHRule.Size = new System.Drawing.Size(152, 21);
            this.cboNoOHRule.TabIndex = 41;
            this.cboNoOHRule.Tag = null;
            this.cboNoOHRule.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboNoOHRule_MIDComboBoxPropertiesChangedEvent);
            this.cboNoOHRule.SelectionChangeCommitted += new System.EventHandler(this.cboNoOHRule_SelectionChangeCommitted);
            // 
            // txtNoOHQuantity
            // 
            this.txtNoOHQuantity.Location = new System.Drawing.Point(220, 18);
            this.txtNoOHQuantity.MaxLength = 6;
            this.txtNoOHQuantity.Name = "txtNoOHQuantity";
            this.txtNoOHQuantity.Size = new System.Drawing.Size(64, 20);
            this.txtNoOHQuantity.TabIndex = 42;
            this.txtNoOHQuantity.TextChanged += new System.EventHandler(this.txtNoOHQuantity_TextChanged);
            this.txtNoOHQuantity.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtNoOHQuantity_KeyUp);
            // 
            // gbxActiveMatrix
            // 
            this.gbxActiveMatrix.Controls.Add(this.cboComponent);
            this.gbxActiveMatrix.Controls.Add(this.lblComponent);
            this.gbxActiveMatrix.Controls.Add(this.cbxInteractive);
            this.gbxActiveMatrix.Controls.Add(this.ugGroupData);
            this.gbxActiveMatrix.Location = new System.Drawing.Point(344, 35);
            this.gbxActiveMatrix.Name = "gbxActiveMatrix";
            this.gbxActiveMatrix.Size = new System.Drawing.Size(296, 106);
            this.gbxActiveMatrix.TabIndex = 46;
            this.gbxActiveMatrix.TabStop = false;
            this.gbxActiveMatrix.Text = "Active Matrix";
            // 
            // cboComponent
            // 
            this.cboComponent.AutoAdjust = true;
            this.cboComponent.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboComponent.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboComponent.DataSource = null;
            this.cboComponent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboComponent.DropDownWidth = 144;
            this.cboComponent.FormattingEnabled = false;
            this.cboComponent.IgnoreFocusLost = false;
            this.cboComponent.ItemHeight = 13;
            this.cboComponent.Location = new System.Drawing.Point(150, 12);
            this.cboComponent.Margin = new System.Windows.Forms.Padding(0);
            this.cboComponent.MaxDropDownItems = 25;
            this.cboComponent.Name = "cboComponent";
            this.cboComponent.SetToolTip = "";
            this.cboComponent.Size = new System.Drawing.Size(144, 21);
            this.cboComponent.TabIndex = 39;
            this.cboComponent.Tag = null;
            this.cboComponent.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboComponent_MIDComboBoxPropertiesChangedEvent);
            this.cboComponent.SelectionChangeCommitted += new System.EventHandler(this.cboComponent_SelectionChangeCommitted);
            // 
            // lblComponent
            // 
            this.lblComponent.Location = new System.Drawing.Point(86, 16);
            this.lblComponent.Name = "lblComponent";
            this.lblComponent.Size = new System.Drawing.Size(64, 16);
            this.lblComponent.TabIndex = 38;
            this.lblComponent.Text = "Component";
            // 
            // cbxInteractive
            // 
            this.cbxInteractive.Location = new System.Drawing.Point(16, 16);
            this.cbxInteractive.Name = "cbxInteractive";
            this.cbxInteractive.Size = new System.Drawing.Size(64, 16);
            this.cbxInteractive.TabIndex = 37;
            this.cbxInteractive.Text = "Activate";
            this.cbxInteractive.CheckedChanged += new System.EventHandler(this.cbxInteractive_CheckedChanged);
            // 
            // ugGroupData
            // 
            appearance25.BackColor = System.Drawing.Color.White;
            appearance25.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugGroupData.DisplayLayout.Appearance = appearance25;
            this.ugGroupData.DisplayLayout.InterBandSpacing = 10;
            appearance26.BackColor = System.Drawing.Color.Transparent;
            this.ugGroupData.DisplayLayout.Override.CardAreaAppearance = appearance26;
            appearance27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance27.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance27.ForeColor = System.Drawing.Color.Black;
            appearance27.TextHAlignAsString = "Left";
            appearance27.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugGroupData.DisplayLayout.Override.HeaderAppearance = appearance27;
            appearance28.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugGroupData.DisplayLayout.Override.RowAppearance = appearance28;
            appearance29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance29.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugGroupData.DisplayLayout.Override.RowSelectorAppearance = appearance29;
            this.ugGroupData.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugGroupData.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance30.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance30.ForeColor = System.Drawing.Color.Black;
            this.ugGroupData.DisplayLayout.Override.SelectedRowAppearance = appearance30;
            this.ugGroupData.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugGroupData.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugGroupData.Location = new System.Drawing.Point(16, 42);
            this.ugGroupData.Name = "ugGroupData";
            this.ugGroupData.Size = new System.Drawing.Size(262, 106);
            this.ugGroupData.TabIndex = 47;
            this.ugGroupData.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugGroupData_InitializeLayout);
            // 
            // lblAttribute
            // 
            this.lblAttribute.Location = new System.Drawing.Point(26, 10);
            this.lblAttribute.Name = "lblAttribute";
            this.lblAttribute.Size = new System.Drawing.Size(48, 16);
            this.lblAttribute.TabIndex = 44;
            this.lblAttribute.Text = "Attribute";
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.AllowDrop = true;
            this.cboStoreAttribute.AllowUserAttributes = false;
            this.cboStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStoreAttribute.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStoreAttribute.Location = new System.Drawing.Point(80, 8);
            this.cboStoreAttribute.Name = "cboStoreAttribute";
            this.cboStoreAttribute.Size = new System.Drawing.Size(184, 21);
            this.cboStoreAttribute.TabIndex = 45;
            this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cboStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
            this.cboStoreAttribute.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
            this.cboStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
            this.cboStoreAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragOver);
            // 
            // ugMatrix
            // 
            this.ugMatrix.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugMatrix.DisplayLayout.Appearance = appearance31;
            this.ugMatrix.DisplayLayout.InterBandSpacing = 10;
            appearance32.BackColor = System.Drawing.Color.Transparent;
            this.ugMatrix.DisplayLayout.Override.CardAreaAppearance = appearance32;
            appearance33.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance33.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance33.ForeColor = System.Drawing.Color.Black;
            appearance33.TextHAlignAsString = "Left";
            appearance33.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugMatrix.DisplayLayout.Override.HeaderAppearance = appearance33;
            appearance34.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugMatrix.DisplayLayout.Override.RowAppearance = appearance34;
            appearance35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance35.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance35.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugMatrix.DisplayLayout.Override.RowSelectorAppearance = appearance35;
            this.ugMatrix.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugMatrix.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance36.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance36.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance36.ForeColor = System.Drawing.Color.Black;
            this.ugMatrix.DisplayLayout.Override.SelectedRowAppearance = appearance36;
            this.ugMatrix.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugMatrix.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugMatrix.Location = new System.Drawing.Point(16, 213);
            this.ugMatrix.Name = "ugMatrix";
            this.ugMatrix.Size = new System.Drawing.Size(664, 365);
            this.ugMatrix.TabIndex = 43;
            this.ugMatrix.Text = "Velocity Matrix";
            this.ugMatrix.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugMatrix_AfterCellUpdate);
            this.ugMatrix.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugMatrix_InitializeLayout);
            this.ugMatrix.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugMatrix_CellChange);
            this.ugMatrix.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugMatrix_AfterCellListCloseUp);
            this.ugMatrix.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugMatrix_MouseEnterElement);
            // 
            // lblSet
            // 
            this.lblSet.Location = new System.Drawing.Point(352, 10);
            this.lblSet.Name = "lblSet";
            this.lblSet.Size = new System.Drawing.Size(72, 17);
            this.lblSet.TabIndex = 18;
            this.lblSet.Text = "Attribute Set";
            // 
            // cbxAttributeSet
            // 
            this.cbxAttributeSet.AutoAdjust = true;
            this.cbxAttributeSet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbxAttributeSet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxAttributeSet.DataSource = null;
            this.cbxAttributeSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAttributeSet.DropDownWidth = 185;
            this.cbxAttributeSet.FormattingEnabled = false;
            this.cbxAttributeSet.IgnoreFocusLost = false;
            this.cbxAttributeSet.ItemHeight = 13;
            this.cbxAttributeSet.Location = new System.Drawing.Point(430, 8);
            this.cbxAttributeSet.Margin = new System.Windows.Forms.Padding(0);
            this.cbxAttributeSet.MaxDropDownItems = 25;
            this.cbxAttributeSet.Name = "cbxAttributeSet";
            this.cbxAttributeSet.SetToolTip = "";
            this.cbxAttributeSet.Size = new System.Drawing.Size(185, 21);
            this.cbxAttributeSet.TabIndex = 19;
            this.cbxAttributeSet.Tag = null;
            this.cbxAttributeSet.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cbxAttributeSet_MIDComboBoxPropertiesChangedEvent);
            this.cbxAttributeSet.SelectionChangeCommitted += new System.EventHandler(this.cbxAttributeSet_SelectionChangeCommitted);
            // 
            // mnuBasisGrid
            // 
            this.mnuBasisGrid.Popup += new System.EventHandler(this.mnuBasisGridItemInsert_Click);
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnView.Location = new System.Drawing.Point(120, 668);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(114, 23);
            this.btnView.TabIndex = 24;
            this.btnView.Text = "Velocity Store Detail";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnChanges
            // 
            this.btnChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnChanges.Location = new System.Drawing.Point(254, 668);
            this.btnChanges.Name = "btnChanges";
            this.btnChanges.Size = new System.Drawing.Size(96, 23);
            this.btnChanges.TabIndex = 25;
            this.btnChanges.Text = "Apply Changes";
            this.btnChanges.Click += new System.EventHandler(this.btnChanges_Click);
            // 
            // frmVelocityMethod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(720, 698);
            this.Controls.Add(this.btnChanges);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.tabVelocityMethod);
            this.Name = "frmVelocityMethod";
            this.Text = "Velocity Method";
            this.Activated += new System.EventHandler(this.FormActivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmVelocityMethod_FormClosing);
            this.Load += new System.EventHandler(this.frmVelocityMethod_Load);
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
            this.Controls.SetChildIndex(this.tabVelocityMethod, 0);
            this.Controls.SetChildIndex(this.btnView, 0);
            this.Controls.SetChildIndex(this.btnChanges, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.pnlGlobalUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.tabVelocityMethod.ResumeLayout(false);
            this.tabMethod.ResumeLayout(false);
            this.tabMethod.PerformLayout();
            this.gbxGrade.ResumeLayout(false);
            this.gbxApplyMinMax.ResumeLayout(false);
            this.gbxApplyMinMax.PerformLayout();
            this.gbxCriteria.ResumeLayout(false);
            this.tabVelocity.ResumeLayout(false);
            this.tabBasis.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugBasisNodeVersion)).EndInit();
            this.tabGrades.ResumeLayout(false);
            this.gbxMinMaxOpt.ResumeLayout(false);
            this.gbxMinMaxOpt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugSellThruPcts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugVelocityGrades)).EndInit();
            this.gbxShip.ResumeLayout(false);
            this.gbxAverage.ResumeLayout(false);
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            this.tabMatrix.ResumeLayout(false);
            this.tabMatrix.PerformLayout();
            this.gbxSpreadOption.ResumeLayout(false);
            this.gbxSpreadOption.PerformLayout();
            this.gbxMatrixMode.ResumeLayout(false);
            this.gbxMatrixMode.PerformLayout();
            this.gbxMatrixDefault.ResumeLayout(false);
            this.gbxMatrixDefault.PerformLayout();
            this.gbxNoOH.ResumeLayout(false);
            this.gbxNoOH.PerformLayout();
            this.gbxActiveMatrix.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugGroupData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugMatrix)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       
        #endregion

        /// <summary>
        /// Opens a new Velocity Method.
        /// </summary>
        override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
        {
            try
            {
                _velocityMethod = new VelocityMethod(SAB, Include.NoRID);
                ABM = _velocityMethod;
                base.NewWorkflowMethod(aParentNode, eSecurityFunctions.AllocationMethodsUserVelocity, eSecurityFunctions.AllocationMethodsGlobalVelocity);

                Common_Load(aParentNode.GlobalUserType);

            }
            catch (Exception ex)
            {
                HandleException(ex);
                FormLoadError = true;
            }
        }
        /// <summary>
        /// Opens an existing Velocity Method.
        /// </summary>
        /// <param name="aMethodRID">method_RID</param>
        /// <param name="aLockStatus">The lock status of the data to be displayed</param>
        override public void UpdateWorkflowMethod(int aMethodRID, int aNodeRID, MIDWorkflowMethodTreeNode aNode, eLockStatus aLockStatus)
        {
            try
            {
                _nodeRID = aNodeRID;
                _velocityMethod = new VelocityMethod(SAB, aMethodRID);
                base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.AllocationMethodsUserVelocity, eSecurityFunctions.AllocationMethodsGlobalVelocity);

                Common_Load(aNode.GlobalUserType);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                FormLoadError = true;
            }
        }

        /// <summary>
        /// Deletes a Velocity Method.
        /// </summary>
        /// <param name="aMethodRID">The record ID of the method</param>
        override public bool DeleteWorkflowMethod(int aMethodRID)
        {
            try
            {
                _velocityMethod = new VelocityMethod(SAB, aMethodRID);
                return Delete();
            }
            catch (DatabaseForeignKeyViolation keyVio)
            {
                throw keyVio;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                FormLoadError = true;
            }

            return true;
        }

        /// <summary>
        /// Renames a Velocity Method.
        /// </summary>
        /// <param name="aMethodRID">The record ID of the method</param>
        /// <param name="aNewName">The new name of the workflow or method</param>
        override public bool RenameWorkflowMethod(int aMethodRID, string aNewName)
        {
            try
            {
                _velocityMethod = new VelocityMethod(SAB, aMethodRID);
                return Rename(aNewName);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                FormLoadError = true;
            }
            return false;
        }

        /// <summary>
        /// Processes a method.
        /// </summary>
        /// <param name="aWorkflowRID">The record ID of the method</param>
        override public void ProcessWorkflowMethod(int aMethodRID)
        {
            try
            {
                _velocityMethod = new VelocityMethod(SAB, aMethodRID);
                ProcessAction(eMethodType.Velocity, true);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                FormLoadError = true;
            }
        }

        // Begin TT#3513 - JSmith - Clean Up Memory Leaks
        public void RemoveReferenceToAllocationViewSelection()
        {
            _avs = null;
        }
        // End TT#3513 - JSmith - Clean Up Memory Leaks

        private void Common_Load(eGlobalUserType aGlobalUserType)
        {
            try
            {
                SetText();
                Name = MIDText.GetTextOnly((int)eMethodType.Velocity);


                _thisTitle = _velocityMethod.Name;

                //setup images to be used in the grid.
                picRelToPlan = new Bitmap(GraphicsDirectory + "\\RelToPlan.bmp");
                picRelToCurrent = new Bitmap(GraphicsDirectory + "\\RelToCurrent.bmp");

                _currentTabPage = this.tabBasis;
                _currentMethodTabPage = this.tabMethod;
                _totalMatrixSet = new StoreGroupLevelListViewProfile(Include.TotalMatrixLevelRID);
                _totalMatrixSet.Name = MIDText.GetTextOnly(eMIDTextCode.lbl_TotalMatrix);


            

                //Begin Track #5858 - KJohnson - Validating store security only
                // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                //cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute);
                cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute, FunctionSecurity, aGlobalUserType == eGlobalUserType.User);
                // End TT#44
                //End Track #5858
                StoreAttributes_Populate();
                BuildDataTables();
                // BEGIN MID Track #3102 - Remove OTS Plan Section 
                //BindMerchandiseCombo();
                // END MID Track #3102  
                BindRuleComboBoxes();

				// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
				// BEGIN TT#696-MD - Stodd - add "active process"
				//bool isProcessingInAssortment = false;
				//bool isProcessingInAssortment = false;
				//bool useAssortmentHeaders = false;

				//useAssortmentHeaders = UseAssortmentSelectedHeaders();
                lblGA.Visible = false;	// TT#1194-MD - stodd - view GA header 
				if (AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID != Include.NoRID)	// use assortment
				{
					_selectedHeaderList = SAB.AssortmentSelectedHeaderEvent.GetSelectedHeaders(this, AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID, eMethodType.Velocity);
					// Begin TT#1194-MD - stodd - view GA header 
                    foreach (SelectedHeaderProfile shp in _selectedHeaderList.ArrayList)
                    {
                        if (shp.HeaderType == eHeaderType.Assortment)
                        {
                            lblGA.Visible = true;
                        }
                    }
					// End TT#1194-MD - stodd - view GA header 
				}
				else
				{
					_selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
				}
				// END TT#696-MD - Stodd - add "active process"
				// END TT#371-MD - stodd -  Velocity Interactive on Assortment

                BindComponentCombo();

                // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail 
                _userRIDList = new ArrayList();
                if (MethodViewGlobalSecurity.AllowView || MethodViewUserSecurity.AllowView)
                {
                    if (MethodViewGlobalSecurity.AllowView)
                    {
                        _userRIDList.Add(Include.GlobalUserRID);
                    }
                    if (MethodViewUserSecurity.AllowView)
                    {
                        _userRIDList.Add(SAB.ClientServerSession.UserRID);
                    }
                    BindMatrixViewCombo();
                }
                else
                {
                    cboMatrixView.Visible = false;
                }
                // End TT#231

                LoadValues();
                GetDataSource();
                BuildGroupDataTable();

                if (FunctionSecurity.AllowInteractive)
                {
                    cbxInteractive.Enabled = true;
                }
                else
                {
                    cbxInteractive.Enabled = false;
                }

                btnView.Enabled = cbxInteractive.Checked;
                btnChanges.Enabled = cbxInteractive.Checked;
                cboComponent.Enabled = cbxInteractive.Checked;

                // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
                int viewRID = UserGridView.UserGridView_Read(SAB.ClientServerSession.UserRID, eLayoutID.velocityMatrixGrid);
                if (viewRID != Include.NoRID)
                {
                    cboMatrixView.SelectedValue = viewRID;
                }
                // End TT#231  

                AdjustTextWidthComboBox_DropDown(cboStoreAttribute); //TT#7 - MD - RBeck _ Dynamic download
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void GetDataSource()
        {
            try
            {
                _dsVelocity = _velocityMethod.DSVelocity;
                BasisTab_Load();
                VelocityGradesTab_Load();
                _sellThruPctsDataTable = _dsVelocity.Tables["SellThru"];
                _sellThruPctsDataTable.PrimaryKey = new DataColumn[] { _sellThruPctsDataTable.Columns["SellThruIndex"] }; //TT#3963 - DOConnell - Delete Sell Thru %'s  causes Foreign Key Violation on update
                _velocityGradesDataTable = _dsVelocity.Tables["VelocityGrade"];
                _groupLevelDataTable = _dsVelocity.Tables["GroupLevel"];
                _dbMatrixDataTable = _dsVelocity.Tables["VelocityMatrix"];

                int setValue = Convert.ToInt32(cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
                LoadAttributeSetValues(setValue);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void BuildGroupDataTable()
        {
            try
            {
                _groupDataTable = MIDEnvironment.CreateDataTable();
                DataColumn dCol;
                DataRow dRow;

                dCol = new DataColumn();
                dCol.DataType = System.Type.GetType("System.String");
                dCol.ColumnName = "RowHeader";
                dCol.ReadOnly = false;
                _groupDataTable.Columns.Add(dCol);

                dCol = new DataColumn();
                dCol.DataType = System.Type.GetType("System.Double");
                dCol.ColumnName = "AvgWOS";
                dCol.ReadOnly = false;
                _groupDataTable.Columns.Add(dCol);

                dCol = new DataColumn();
                dCol.DataType = System.Type.GetType("System.Double");
                dCol.ColumnName = "PctSellThru";
                dCol.ReadOnly = false;
                _groupDataTable.Columns.Add(dCol);

                dRow = _groupDataTable.NewRow();
                dRow["RowHeader"] = _lblAllStores;
                _groupDataTable.Rows.Add(dRow);

                dRow = _groupDataTable.NewRow();
                dRow["RowHeader"] = _lblSet;
                _groupDataTable.Rows.Add(dRow);

                ugGroupData.DataSource = _groupDataTable;

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void LoadValues()
        {
            try
            {
                if (_velocityMethod.Method_Change_Type != eChangeType.add)
                {

                    this.txtName.Text = _velocityMethod.Name;
                    this.txtDesc.Text = _velocityMethod.Method_Description;

                    if (_velocityMethod.User_RID == Include.GetGlobalUserRID())
                        radGlobal.Checked = true;
                    else
                        radUser.Checked = true;
                }

                GetWorkflows(_velocityMethod.Key, ugWorkflows);
                // BEGIN MID Track #3102 - Remove OTS Plan Section 
                //Load Merchandise Node or Level Text to combo box
                //				HierarchyNodeProfile hnp;
                //				if (_velocityMethod.OTSPlanHNRID == Include.DefaultPlanHnRID)	
                //					cboOTSPlan.SelectedIndex = 0;
                //				else if (_velocityMethod.OTSPlanHNRID != Include.NoRID)
                //				{
                //					hnp = SAB.HierarchyServerSession.GetNodeData(_velocityMethod.OTSPlanHNRID);
                //					AddNodeToMerchandiseCombo ( hnp );
                //				}
                //				else
                //				{ 
                //					if (_velocityMethod.OTSPlanPHRID != Include.NoRID)
                //						SetComboToLevel(_velocityMethod.OTSPlanPHLSeq);
                //					else
                //						cboOTSPlan.SelectedIndex = 0;
                //				}
                //Load Calendar Data
                //				DateRangeProfile dr;
                //				if (   _velocityMethod.OTS_Begin_CDR_RID != Include.UndefinedCalendarDateRange
                //					&& _velocityMethod.OTS_Begin_CDR_RID != Include.NoRID
                //					&& _velocityMethod.OTS_Begin_CDR_RID != 0)
                //				{
                //					dr = SAB.ClientServerSession.Calendar.GetDateRange(_velocityMethod.OTS_Begin_CDR_RID);
                //					LoadDateRangeText(dr, midDateRangeSelectorBeg, strBegin);
                //				}
                //				if (   _velocityMethod.OTS_Ship_To_CDR_RID != Include.UndefinedCalendarDateRange
                //					&& _velocityMethod.OTS_Ship_To_CDR_RID != Include.NoRID
                //					&& _velocityMethod.OTS_Ship_To_CDR_RID != 0)
                //				{
                //					dr = SAB.ClientServerSession.Calendar.GetDateRange(_velocityMethod.OTS_Ship_To_CDR_RID);
                //					LoadDateRangeText(dr, midDateRangeSelectorShip, strShip);
                //				}
                // END MID Track #3102 
                this.cboStoreAttribute.SelectedValue = _velocityMethod.SG_RID;
               
                // Begin TT#1530 - RMatelic - Record locking violations from Store Attribute change
                // Begin Track #4872 - JSmith - Global/User Attributes
                //if (cboStoreAttribute.ContinueReadOnly)
                //{
                //    SetMethodReadOnly();
                //}
                // End Track #4872
                if (FunctionSecurity.AllowUpdate)
                {
                    if (cboStoreAttribute.ContinueReadOnly)
                    {
                        SetMethodReadOnly();
                    }
                }
                else
                {
                    cboStoreAttribute.Enabled = false;
                }
                // End TT#1530

                _prevAttributeValue = _velocityMethod.SG_RID;
                radAvgChain.Checked = _velocityMethod.CalculateAverageUsingChain;
                radAvgSet.Checked = !_velocityMethod.CalculateAverageUsingChain;

                radShipBasis.Checked = _velocityMethod.DetermineShipQtyUsingBasis;
                radHeaderStyle.Checked = !_velocityMethod.DetermineShipQtyUsingBasis;

                cbxSimilarStores.Checked = _velocityMethod.UseSimilarStoreHistory;
                // Begin Track #6074 stodd
                // Begin TT # 91 - stodd
                //cbxBasisGrades.Checked = _velocityMethod.GradesByBasisInd;
                // Begin TT # 91 - stodd
                // End Track #6074
                cbxTrendPct.Checked = _velocityMethod.TrendPctContribution;
                // cbxTrendPct is not currently used so make it invisible
                // for now

                // Begin TT#313 - JSmith -  balance does not remain checked
                cbxBalance.Checked = _velocityMethod.Balance;
                // End TT#313

                // BEGIN TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
                cbxReconcile.Checked = _velocityMethod.Reconcile;
                // END TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance



                //Begin TT#855-MD -jsobek -Velocity Enhancements
                if (_velocityMethod.GradeVariableType == eVelocityMethodGradeVariableType.Stock)
                {
                    radGradeVariableStock.Checked = true;
                }
                else
                {
                    radGradeVariableSales.Checked = true;
                }
                if (_velocityMethod.BalanceToHeaderInd == '1')
                {
                    cbxBalanceToHeader.Checked = true;
                }
                else
                {
                    cbxBalanceToHeader.Checked = false;
                }
                //End TT#855-MD -jsobek -Velocity Enhancements
                

                // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                switch (_velocityMethod.ApplyMinMaxInd)
                {
                    case 'N':
                        this.radApplyMinMaxNone.Checked = true;
                        gbxMinMaxOpt.Enabled = false; // TT#1287 - AGallagher - Inventory Min/Max
                        _ApplyMinMaxInd = 'N';
                        break;
                    case 'S':
                        this.radApplyMinMaxStore.Checked = true;
                        gbxMinMaxOpt.Enabled = false; // TT#1287 - AGallagher - Inventory Min/Max
                        _ApplyMinMaxInd = 'S';
                        break;
                    case 'V':
                        this.radApplyMinMaxVelocity.Checked = true;
                        gbxMinMaxOpt.Enabled = true; // TT#1287 - AGallagher - Inventory Min/Max
                        _ApplyMinMaxInd = 'V';
                        break;
                }
                // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                switch (_velocityMethod.InventoryInd)
                {
                    case 'A':
                        this.radAllocationMinMax.Checked = true;
                        cboInventoryBasis.Enabled = false;
                        cboInventoryBasis.Text = null;
                        _InventoryInd = 'A';
                        break;
                    case 'I':
                        this.radInventoryMinMax.Checked = true;
                        cboInventoryBasis.Enabled = true;
                        _InventoryInd = 'I';
                        break;
                }
                // END TT#1287 - AGallagher - Inventory Min/Max


                cbxTrendPct.Visible = false;

                //BEGIN TT#110-MD-VStuart - In Use Tool
                tabVelocityMethod.Controls.Remove(tabProperties);
                //END TT#110-MD-VStuart - In Use Tool

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // BEGIN MID Track #3102 - Remove OTS Plan Section 
        //		private void BindMerchandiseCombo()
        //		{
        //			try
        //			{
        //				cboOTSPlan.DataSource = MerchandiseDataTable;
        //				cboOTSPlan.DisplayMember = "text";
        //				cboOTSPlan.ValueMember = "seqno";
        //			}
        //			catch (Exception ex)
        //			{
        //				HandleException(ex);
        //			}
        //		}
        // END MID Track #3102  
        private void BindRuleComboBoxes()
        {
            _defOnHandRules = MIDText.GetLabels((int)eVelocityRuleType.None, (int)eVelocityRuleType.None);

            DataTable velocityRules2 = MIDText.GetLabels((int)eVelocityRuleType.WeeksOfSupply, (int)eVelocityRuleType.ForwardWeeksOfSupply);

            //Begin TT#637 - Velocity Spread Average - APicchetti
            DataTable MatrixModeVelocityRules = MIDText.GetLabels((int)eVelocityRuleRequiresQuantity.WeeksOfSupply,
                // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) 
                //    (int)eVelocityRuleRequiresQuantity.ForwardWeeksOfSupply);
                (int)eVelocityRuleRequiresQuantity.ShipUpToQty);

            DataTable MatrixModeVelocityRules2 = MIDText.GetLabels((int)eVelocityRuleRequiresQuantity.AbsoluteQuantity,
            (int)eVelocityRuleRequiresQuantity.AbsoluteQuantity);

            DataTable MatrixModeVelocityRules3 = MIDText.GetLabels((int)eVelocityRuleRequiresQuantity.ForwardWeeksOfSupply,
            (int)eVelocityRuleRequiresQuantity.ForwardWeeksOfSupply);

            foreach (DataRow row in MatrixModeVelocityRules2.Rows)
            {
                MatrixModeVelocityRules.ImportRow(row);
            }

            foreach (DataRow row in MatrixModeVelocityRules3.Rows)
            {
                MatrixModeVelocityRules.ImportRow(row);
            }

            MatrixModeVelocityRules2.Clear();
            MatrixModeVelocityRules3.Clear();
            // END TT#637 - AGallagher - Velocity - Spread Average (#7) 
            //End TT#637 - Velocity Spread Average - APicchetti

            // For now do not include eVelocityRuleType.ForwardWeeksOfSupply
            //DataTable velocityRules2 = MIDText.GetLabels((int) eVelocityRuleType.WeeksOfSupply, (int)eVelocityRuleType.ColorMaximum);

            foreach (DataRow row in velocityRules2.Rows)
            {
                DataRow nRow = _defOnHandRules.NewRow();
                nRow["TEXT_CODE"] = row["TEXT_CODE"];
                nRow["TEXT_VALUE"] = row["TEXT_VALUE"];
                _defOnHandRules.Rows.Add(nRow);
            }
            // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
            //DataTable velocityRules3 = MIDText.GetLabels((int)eVelocityRuleType.MinimumBasis, (int)eVelocityRuleType.AdMinimumBasis);
            //foreach (DataRow row in velocityRules3.Rows)
            //{
            //    DataRow nRow = _defOnHandRules.NewRow();
            //    nRow["TEXT_CODE"] = row["TEXT_CODE"];
            //    nRow["TEXT_VALUE"] = row["TEXT_VALUE"];
            //    _defOnHandRules.Rows.Add(nRow);
            //}
            // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
            _noOnHandRules = _defOnHandRules.Copy();

            this.cboOHRule.DataSource = _defOnHandRules;
            this.cboOHRule.DisplayMember = "TEXT_VALUE";
            this.cboOHRule.ValueMember = "TEXT_CODE";
            this.cboOHRule.SelectedValue = (int)eVelocityRuleType.None;

            this.cboNoOHRule.DataSource = _noOnHandRules;
            this.cboNoOHRule.DisplayMember = "TEXT_VALUE";
            this.cboNoOHRule.ValueMember = "TEXT_CODE";
            this.cboNoOHRule.SelectedValue = (int)eVelocityRuleType.None;

            //Begin TT#637 - Velocity Spread Average - APicchetti
            this.cboMatrixModeAvgRule.DataSource = MatrixModeVelocityRules;
            this.cboMatrixModeAvgRule.DisplayMember = "TEXT_VALUE";
            this.cboMatrixModeAvgRule.ValueMember = "TEXT_CODE";
            this.cboMatrixModeAvgRule.SelectedValue = (int)eVelocityRuleType.None;
            //End TT#637 - Velocity Spread Average - APicchetti 

        }

        private void BindComponentCombo()
        {
            _compDataTable = MIDText.GetLabels((int)eVelocityMethodComponentType.Total, (int)eVelocityMethodComponentType.Bulk);
            for (int i = _compDataTable.Columns.Count - 1; i >= 0; i--)
            {
                DataColumn dc = _compDataTable.Columns[i];
                if (dc.ColumnName != "TEXT_CODE" && dc.ColumnName != "TEXT_VALUE")
                {
                    _compDataTable.Columns.Remove(dc);
                }
            }
            foreach (DataRow dr in _compDataTable.Rows)
            {
                eVelocityMethodComponentType vmct = (eVelocityMethodComponentType)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
                if (!Enum.IsDefined(typeof(eVelocityMethodComponentType), vmct))
                {
                    dr.Delete();
                }
            }
            _compDataTable.AcceptChanges();
            DataColumn col = new DataColumn();
            col.DataType = System.Type.GetType("System.Int32");
            col.ColumnName = "CompType";
            _compDataTable.Columns.Add(col);
            foreach (DataRow dr in _compDataTable.Rows)
            {
                dr["CompType"] = dr["TEXT_CODE"];
            }


            DataColumn[] PrimaryKeyColumn = new DataColumn[1];
            PrimaryKeyColumn[0] = _compDataTable.Columns["TEXT_CODE"];
            _compDataTable.PrimaryKey = PrimaryKeyColumn;
            this.cboComponent.DataSource = _compDataTable;
            this.cboComponent.DisplayMember = "TEXT_VALUE";
            this.cboComponent.ValueMember = "TEXT_CODE";
            this.cboComponent.SelectedIndex = -1;
        }

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail 
        private void BindMatrixViewCombo()
        {
            try
            {
                _bindingView = true;
                // Begin TT#1117 - JSmith - Global & User Views w/ the same names do not have indicators
                //_dtMatrixView = GridViewData.GridView_Read((int)eLayoutID.velocityMatrixGrid, _userRIDList);
                _dtMatrixView = GridViewData.GridView_Read((int)eLayoutID.velocityMatrixGrid, _userRIDList, true);
                // End TT#1117
                _dtMatrixView.Rows.Add(new object[] { Include.NoRID, SAB.ClientServerSession.UserRID, (int)eLayoutID.velocityMatrixGrid, string.Empty });
                _dtMatrixView.PrimaryKey = new DataColumn[] { _dtMatrixView.Columns["VIEW_RID"] };

                cboMatrixView.ValueMember = "VIEW_RID";
                cboMatrixView.DisplayMember = "VIEW_ID";
                cboMatrixView.DataSource = _dtMatrixView;
                cboMatrixView.SelectedValue = -1;

                _bindingView = false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#231  

        private void GetGradesPstFromHeader(int aHeaderRID)
        {
            Header header;
            int nodeRID;
            try
            {
                //this.Cursor = Cursors.WaitCursor;
                header = new Header();
                DataTable dtHeader = header.GetHeader(aHeaderRID);
                if (dtHeader.Rows.Count == 0)
                    return;
                DataRow row = dtHeader.Rows[0];
                nodeRID = Convert.ToInt32(row["STYLE_HNRID"], CultureInfo.CurrentUICulture);

                if (_dsVelocity.Tables["VelocityGrade"].Rows.Count == 0)
                {
                    VelocityGrades_Populate(nodeRID);
                }

                if (_dsVelocity.Tables["SellThru"].Rows.Count == 0)
                {
                    SellThruPcts_Populate(nodeRID);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            // this.Cursor = Cursors.Default;
        }
        // BEGIN TT#673 - AGallagher - Velocity - Disable Balance option on WUB header  
        private void GetBalanceInd()
        {
            try
            {
                cbxBalance.Enabled = true;
                foreach (SelectedHeaderProfile shp in _selectedHeaderList)
                {
                    if (shp.HeaderType == eHeaderType.WorkupTotalBuy)
                    {
                        cbxBalance.Enabled = false;
                        cbxBalance.Checked = false;
                        _velocityMethod.Balance = cbxBalance.Checked;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }
        // END TT#673 - AGallagher - Velocity - Disable Balance option on WUB header  
        private void GetPacksAndColors()
        {
            Header header;
            try
            {
                //header = new Header();
                bool colorsExist = false;
                bool packFound;
                BindComponentCombo();

                foreach (SelectedHeaderProfile shp in _selectedHeaderList)
                {
                    header = new Header();
                    DataTable dtPacks = header.GetPacks(shp.Key);
                    DataTable dtBulkColors = header.GetBulkColors(shp.Key);
                    if (dtPacks.Rows.Count > 0)
                    {
                        foreach (DataRow pRow in dtPacks.Rows)
                        {
                            packFound = false;
                            string dtPackName = pRow["HDR_PACK_NAME"].ToString();

                            foreach (DataRow compRow in _compDataTable.Rows)
                            {
                                string compPackName = compRow["TEXT_VALUE"].ToString();
                                eComponentType compType = (eComponentType)compRow["CompType"];
                                if (compPackName == dtPackName && compType == eComponentType.SpecificPack)
                                {
                                    packFound = true;
                                    break;
                                }
                            }
                            if (!packFound)
                                _compDataTable.Rows.Add(new object[] { (int) pRow["HDR_PACK_RID"], pRow["HDR_PACK_NAME"], 
																	  (int) eComponentType.SpecificPack});
                        }
                    }
                    if (dtBulkColors.Rows.Count > 0)
                    {
                        colorsExist = true;
                        foreach (DataRow cRow in dtBulkColors.Rows)
                        {
                            int colorKey = Convert.ToInt32(cRow["COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
                            if (!_compDataTable.Rows.Contains(colorKey))
                            {
                                ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(colorKey);
                                _compDataTable.Rows.Add(new object[] {colorKey, ccp.ColorCodeName,
																		  (int) eComponentType.SpecificColor});
                            }
                        }
                    }
                }
                if (!colorsExist)
                    RemoveBulk();

                _compDataTable.AcceptChanges();

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        private void RemoveBulk()
        {
            try
            {
                foreach (DataRow dr in _compDataTable.Rows)
                {
                    eVelocityMethodComponentType vmct = (eVelocityMethodComponentType)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
                    if (vmct == eVelocityMethodComponentType.Bulk)
                    {
                        dr.Delete();
                        break;
                    }
                }
                _compDataTable.AcceptChanges();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // BEGIN MID Track #3102 - Remove OTS Plan Section 
        //		private void cboOTSPlan_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        //		{
        //			try
        //			{
        //				ClipboardProfile cbp = GetClipboardData(eClipboardDataType.HierarchyNode);
        //				HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbp.Key);
        //				AddNodeToMerchandiseCombo ( hnp );
        //				AddNodeToMerchandiseCombo2 (hnp);
        //				if (!_basisNodeInList)
        //				{
        //					Infragistics.Win.ValueListItem vli = new Infragistics.Win.ValueListItem();
        //					vli.DataValue = ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Count;
        //					vli.DisplayText = hnp.Text;
        //					vli.Tag = Convert.ToString(hnp.Key, CultureInfo.CurrentUICulture);
        //					ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Add(vli);
        //				}
        //			}
        //			catch (BadDataInClipboardException)
        //			{
        //				MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
        //					this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //			}
        //			catch (Exception ex)
        //			{
        //				HandleException(ex);
        //			}
        //		}
        // END MID Track #3102  
        private void AddNodeToMerchandiseCombo(HierarchyNodeProfile hnp)
        {
            try
            {
                DataRow myDataRow;
                bool nodeFound = false;
                int nodeRID = Include.NoRID;
                for (int levIndex = 0;
                    levIndex < MerchandiseDataTable.Rows.Count; levIndex++)
                {
                    myDataRow = MerchandiseDataTable.Rows[levIndex];
                    if ((eMerchandiseType)(Convert.ToInt32(myDataRow["leveltypename"], CultureInfo.CurrentUICulture)) == eMerchandiseType.Node)
                    {
                        nodeRID = (Convert.ToInt32(myDataRow["key"], CultureInfo.CurrentUICulture));
                        if (hnp.Key == nodeRID)
                        {
                            nodeFound = true;
                            break;
                        }
                    }
                }
                if (!nodeFound)
                {
                    myDataRow = MerchandiseDataTable.NewRow();
                    myDataRow["seqno"] = MerchandiseDataTable.Rows.Count;
                    myDataRow["leveltypename"] = eMerchandiseType.Node;
                    myDataRow["text"] = hnp.Text;
                    myDataRow["key"] = hnp.Key;
                    MerchandiseDataTable.Rows.Add(myDataRow);
                    // BEGIN MID Track #3102 - Remove OTS Plan Section 
                    //cboOTSPlan.SelectedIndex = MerchandiseDataTable.Rows.Count - 1;
                    // END MID Track #3102  
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void AddNodeToMerchandiseCombo2(HierarchyNodeProfile hnp)
        {
            try
            {
                DataRow row;
                _basisNodeInList = false;
                int nodeRID = Include.NoRID;
                for (int levIndex = 0;
                    levIndex < _merchDataTable2.Rows.Count; levIndex++)
                {
                    row = _merchDataTable2.Rows[levIndex];
                    if ((eMerchandiseType)(Convert.ToInt32(row["leveltypename"], CultureInfo.CurrentUICulture)) == eMerchandiseType.Node)
                    {
                        nodeRID = (Convert.ToInt32(row["key"], CultureInfo.CurrentUICulture));
                        if (hnp.Key == nodeRID)
                        {
                            _basisNodeInList = true;
                            break;
                        }
                    }
                }
                if (!_basisNodeInList)
                {
                    row = _merchDataTable2.NewRow();
                    row["seqno"] = _merchDataTable2.Rows.Count;
                    row["leveltypename"] = eMerchandiseType.Node;
                    row["text"] = hnp.Text;
                    row["key"] = hnp.Key;
                    _merchDataTable2.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // BEGIN MID Track #3102 - Remove OTS Plan Section 	
        //		private void SetComboToLevel(int seq)
        //		{
        //			try
        //			{
        //				DataRow myDataRow;
        //				for (int levIndex = 0;
        //					levIndex < MerchandiseDataTable.Rows.Count; levIndex++)
        //				{	
        //					myDataRow = MerchandiseDataTable.Rows[levIndex];
        //					if (Convert.ToInt32(myDataRow["seqno"], CultureInfo.CurrentUICulture) == seq)
        //					{
        //						cboOTSPlan.SelectedIndex = levIndex;
        //						break;
        //					}
        //				}
        //			}
        //			catch (Exception ex)
        //			{
        //				HandleException(ex);
        //			}
        //
        //		}
        //		private void cboOTSPlan_SelectionChangeCommitted(object sender, System.EventArgs e)
        //		{
        //			int idx;
        //			try
        //			{ 
        //				if (FormLoaded)
        //				{	
        //					if (_matrixProcessed)	
        //					{	 
        //						if (_merchReset)
        //						{
        //							_merchReset = false;
        //							return;
        //						}	
        //						idx = this.cboOTSPlan.SelectedIndex;
        //						if (!ReprocessWarningOK(this.lblMerch.Text))
        //						{
        //							_merchReset = true;
        //							this.cboOTSPlan.SelectedIndex = _prevMerchIndex;
        //							return;
        //						}
        //						else
        //							this.cboOTSPlan.SelectedIndex = idx;
        //					}
        //					ChangePending = true;
        //				}
        //				if (this.cboOTSPlan.SelectedValue != null)
        //				{
        //					_prevMerchIndex = Convert.ToInt32(this.cboOTSPlan.SelectedIndex,CultureInfo.CurrentUICulture);
        //				}
        //			}
        //			catch (Exception ex)
        //			{
        //				HandleException(ex);
        //			}
        //		
        //		}

        //		private void cboOTSPlan_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        //		{
        //			try
        //			{
        //				ObjectDragEnter(e);
        //			}
        //			catch (Exception ex)
        //			{
        //				HandleException(ex);
        //			}
        //		}
        //		private void cboOTSPlan_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        //		{
        //			string productID, productID2, errorMessage; 
        //			int key;
        //			ErrorProvider.SetError(cboOTSPlan,string.Empty);
        //			if (cboOTSPlan.Text.Trim().Length > 0)
        //			{
        //				if (cboOTSPlan.SelectedIndex > -1)
        //				{
        //					DataRow row = MerchandiseDataTable.Rows[cboOTSPlan.SelectedIndex];
        //					if (row != null)
        //						return;
        //				}
        //			
        //				productID = cboOTSPlan.Text.Trim();
        //				key = GetNodeText(ref productID);
        //				if (key == Include.NoRID)
        //				{
        //					errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
        //						productID );	
        //					ErrorProvider.SetError(cboOTSPlan,errorMessage);
        //					if (_showMessageBox)
        //					{
        //						MessageBox.Show( errorMessage, _thisTitle);
        //						e.Cancel = true;
        //					}
        //				}
        //				else 
        //				{
        //					cboOTSPlan.Text = productID;
        //					cboOTSPlan.Tag = key;
        //					string[] pArray = productID.Split(new char[] {'['});
        //					productID2 = pArray[0].Trim(); 
        //					HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(productID2);
        //					AddNodeToMerchandiseCombo ( hnp );
        //					AddNodeToMerchandiseCombo2( hnp );
        //					if (!_basisNodeInList)
        //					{
        //						Infragistics.Win.ValueListItem vli = new Infragistics.Win.ValueListItem();
        //						vli.DataValue = ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Count;
        //						vli.DisplayText = hnp.Text;
        //						vli.Tag = Convert.ToString(hnp.Key, CultureInfo.CurrentUICulture);
        //						ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Add(vli);
        //					}
        //				}
        //			}
        //			else
        //			{
        //				errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
        //				cboOTSPlan.Tag = null;
        //				ErrorProvider.SetError(cboOTSPlan,errorMessage);
        //				if (_showMessageBox)
        //				{
        //					MessageBox.Show( errorMessage, _thisTitle);
        //					e.Cancel = true;
        //				}
        //			}
        //		}
        // END MID Track #3102 
        private HierarchyNodeProfile GetNodeProfile(string aProductID)
        {
            string desc = string.Empty;
            try
            {
                string productID = aProductID;
                string[] pArray = productID.Split(new char[] { '[' });
                productID = pArray[0].Trim();
                //				HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(productID);
                HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
                EditMsgs em = new EditMsgs();
                return hm.NodeLookup(ref em, productID, false);
                //				if (hnp.Key == Include.NoRID)
                //					return Include.NoRID;
                //				else 
                //				{
                //					aProductID =  hnp.Text;
                //					return hnp.Key;
                //				}
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return new HierarchyNodeProfile(Include.NoRID);
            }
        }
        // BEGIN MID Track #3102 - Remove OTS Plan Section 	
        //		private void midDateRangeSelectorBeg_ClickCellButton(object sender, CellEventArgs e)
        //		{
        //			//Don't allow change if Interactive Velocity
        //			if (_matrixProcessed)
        //			{
        //				if (!ReprocessWarningOK(this.lblBegin.Text))
        //				{
        //					return;
        //				}
        //			}
        //					
        //			// tells the date range selector the currently selected date range RID
        //			if (midDateRangeSelectorBeg.Tag != null)
        //				((CalendarDateSelector)midDateRangeSelectorBeg.DateRangeForm).DateRangeRID = (int)midDateRangeSelectorBeg.Tag;
        //
        //			((CalendarDateSelector)midDateRangeSelectorBeg.DateRangeForm).AllowDynamicToStoreOpen = false;
        //			((CalendarDateSelector)midDateRangeSelectorBeg.DateRangeForm).AllowDynamicToPlan = false;
        //			((CalendarDateSelector)midDateRangeSelectorBeg.DateRangeForm).RestrictToOnlyWeeks = true;
        //			
        //			// tells the control to show the date range selector form
        //			midDateRangeSelectorBeg.ShowSelector();
        //		}

        //		private void midDateRangeSelectorShip_ClickCellButton(object sender, CellEventArgs e)
        //		{
        //			//Don't allow change if Interactive Velocity
        //			if (_matrixProcessed)
        //			{
        //				if (!ReprocessWarningOK(this.lblShip.Text))
        //				{
        //					return;
        //				}
        //			}
        //			
        //			// tells the date range selector the currently selected date range RID
        //			if (midDateRangeSelectorShip.Tag != null)
        //				((CalendarDateSelector)midDateRangeSelectorShip.DateRangeForm).DateRangeRID = (int)midDateRangeSelectorShip.Tag;
        //
        //			((CalendarDateSelector)midDateRangeSelectorShip.DateRangeForm).AllowDynamicToStoreOpen = false;
        //			((CalendarDateSelector)midDateRangeSelectorShip.DateRangeForm).AllowDynamicToPlan = false;
        //			((CalendarDateSelector)midDateRangeSelectorShip.DateRangeForm).RestrictToOnlyWeeks = true;
        //
        //			
        //			// tells the control to show the date range selector form
        //			midDateRangeSelectorShip.ShowSelector();
        //		}
        // END MID Track #3102  	
        private void cbxSimilarStores_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (FormLoaded)
                {
                    if (_matrixProcessed)
                    {
                        if (_simStoresReset)
                        {
                            _simStoresReset = false;
                            return;
                        }
                        if (!ReprocessWarningOK(this.cbxSimilarStores.Text))
                        {
                            _simStoresReset = true;
                            this.cbxSimilarStores.Checked = !this.cbxSimilarStores.Checked;
                            return;
                        }
                    }
                    ChangePending = true;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }


        #region Basis Tab

        private void ugBasis_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

            DefaultBasisGridLayout();

            //			this.ugVelocityGrades.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            //			this.ugVelocityGrades.DisplayLayout.Bands[0].Columns["RowPosition"].Hidden = true;
            //_basisGridBuilt = true;
        }

        private void DefaultBasisGridLayout()
        {
            //			this.ugBasis.DisplayLayout.Appearance.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
            //			this.ugBasis.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Default;
            //			this.ugBasis.DisplayLayout.Bands[0].Columns["Grade"].Width = 75;
            //			this.ugBasis.DisplayLayout.Bands[0].Columns["Boundary"].Width = 75;
        }


        private void BasisTab_Load()
        {

            //			this.ugBasis.DisplayLayout.Bands[0].Override.TipStyleRowConnector = TipStyle.Show;
            //			this.ugBasis.DisplayLayout.Bands[0].Override.TipStyleScroll = TipStyle.Show;
            //			this.ugBasis.DisplayLayout.Bands[0].Override.TipStyleCell = TipStyle.Show;

            try
            {
                _basisDataTable = _dsVelocity.Tables["Basis"];
                _basisDataTable.Columns.Add("Merchandise");
                _basisDataTable.AcceptChanges();

                CreateComboLists();

                this.ugBasisNodeVersion.DataSource = _basisDataTable;

                this.ugBasisNodeVersion.DisplayLayout.AddNewBox.Hidden = true;
                this.ugBasisNodeVersion.DisplayLayout.GroupByBox.Hidden = true;
                this.ugBasisNodeVersion.DisplayLayout.GroupByBox.Prompt = "";
                this.ugBasisNodeVersion.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
                //ugBasisNodeVersion.DisplayLayout.Bands[0].AddButtonCaption = "Velocity Grade";

                //e.Layout.AutoFitColumns = true;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].ColHeadersVisible = true;
                //Prevent the user from re-arranging columns.
                this.ugBasisNodeVersion.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;

                //hide the db columns.
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisSequence"].Hidden = true;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisHNRID"].Hidden = true;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisPHRID"].Hidden = true;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisPHLSequence"].Hidden = true;

                //Set the header captions.
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["Merchandise"].Header.VisiblePosition = 1;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["Merchandise"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["Merchandise"].Width = 200;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisFVRID"].Header.VisiblePosition = 2;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisFVRID"].Width = 120;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisFVRID"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Version);


                //make the "Merchandise" & "Version" columns drop down lists.

                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["Merchandise"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["Merchandise"].ValueList = ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"];
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisFVRID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisFVRID"].ValueList = ugBasisNodeVersion.DisplayLayout.ValueLists["Version"];

                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //FormatColumns(this.ugBasisNodeVersion);
                //End TT#169
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisFVRID"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;

                if (!FunctionSecurity.AllowUpdate)
                {
                    foreach (UltraGridBand ugb in this.ugBasisNodeVersion.DisplayLayout.Bands)
                    {
                        ugb.Override.AllowDelete = DefaultableBoolean.False;
                    }
                }

                if (FunctionSecurity.AllowUpdate)
                {
                    BuildBasisContextMenu();
                    this.ugBasisNodeVersion.ContextMenu = mnuBasisGrid;
                }

                //				HierarchyNodeProfile hnp;
                foreach (DataRow row in _basisDataTable.Rows)
                {
                    if ((int)row["BasisHNRID"] != Include.NoRID)
                    {
                        //hnp = SAB.HierarchyServerSession.GetNodeData((int)row["BasisHNRID"] );
                        //row["Merchandise"] = hnp.Text;
                        for (int i = 0; i < _merchDataTable2.Rows.Count; i++)
                        {
                            DataRow listRow = _merchDataTable2.Rows[i];
                            if ((int)listRow["key"] == (int)row["BasisHNRID"])
                            {
                                SetGridComboToLevel(row, (int)listRow["seqno"]);
                            }
                        }
                    }
                    else if ((int)row["BasisPHRID"] != Include.NoRID)
                    {
                        SetGridComboToLevel(row, (int)row["BasisPHLSequence"]);
                    }
                    else
                        SetGridComboToLevel(row, 0);
                }

                _basisIsPopulated = true;

                // BEGIN Issue 4818
                //_salesPeriodDataTable = _dsVelocity.Tables["SalesPeriod"];
                _basisDataTable.Columns.Add("DateRange");
                _basisDataTable.Columns.Add("Picture");
                _basisDataTable.Columns["Weight"].DefaultValue = 1;
                _basisDataTable.AcceptChanges();
                //this.ugBasisSalesPeriod.DataSource = _salesPeriodDataTable;
                //this.ugBasisSalesPeriod.DisplayLayout.AddNewBox.Hidden = true;
                //this.ugBasisSalesPeriod.DisplayLayout.GroupByBox.Hidden = true;
                //this.ugBasisSalesPeriod.DisplayLayout.GroupByBox.Prompt = "";
                //this.ugBasisSalesPeriod.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;

                //this.ugBasisSalesPeriod.DisplayLayout.Bands[0].ColHeadersVisible = true;
                //Prevent the user from re-arranging columns.
                //this.ugBasisSalesPeriod.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;

                //hide the db columns.
                //this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["PeriodSequence"].Hidden = true;
                // END Issue 4818
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["cdrRID"].Hidden = true;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["DateRange"].Header.VisiblePosition = 3;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["DateRange"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Date_Range);
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["DateRange"].Width = 180;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["DateRange"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["DateRange"].CellActivation = Activation.ActivateOnly;

                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["Picture"].Hidden = true;

                // BEGIN Issue 4818
                //				this.ugBasisSalesPeriod.DisplayLayout.Bands[0].Columns["Picture"].Header.VisiblePosition = 2;
                //				this.ugBasisSalesPeriod.DisplayLayout.Bands[0].Columns["Picture"].Header.Caption = " ";
                //				this.ugBasisSalesPeriod.DisplayLayout.Bands[0].Columns["Picture"].CellActivation = Activation.NoEdit;
                //				this.ugBasisSalesPeriod.DisplayLayout.Bands[0].Columns["Picture"].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
                //				this.ugBasisSalesPeriod.DisplayLayout.Bands[0].Columns["Picture"].CellAppearance.ImageVAlign = Infragistics.Win.VAlign.Middle;
                //				this.ugBasisSalesPeriod.DisplayLayout.Bands[0].Columns["Picture"].Width = 20;
                // END Issue 4818
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["Weight"].Header.VisiblePosition = 4;
                // BEGIN MID Track #3640 - get label text
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["Weight"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Weight);
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["Weight"].Width = 50;

                // END MID Track #3640

                // BEGIN Issue 4818
                //				FormatColumns(this.ugBasisSalesPeriod);
                //
                //				if (!FunctionSecurity.AllowUpdate)
                //				{
                //					foreach (UltraGridBand ugb in this.ugBasisSalesPeriod.DisplayLayout.Bands)
                //					{
                //						ugb.Override.AllowDelete = DefaultableBoolean.False;
                //					}
                //				}
                //
                //				if (FunctionSecurity.AllowUpdate)
                //				{
                //					BuildSalesPeriodContextMenu();
                //					this.ugBasisSalesPeriod.ContextMenu = mnuSalesGrid;
                //				}
                // END Issue 4818

                foreach (UltraGridRow row in ugBasisNodeVersion.Rows)
                {
                    DateRangeProfile dr = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(row.Cells["cdrRID"].Value, CultureInfo.CurrentUICulture));
                    row.Cells["DateRange"].Value = dr.DisplayDate;
                    if (dr.DateRangeType == eCalendarRangeType.Dynamic)
                    {
                        switch (dr.RelativeTo)
                        {
                            case eDateRangeRelativeTo.Current:
                                row.Cells["DateRange"].Appearance.Image = this.DynamicToCurrentImage;
                                break;
                            case eDateRangeRelativeTo.Plan:
                                row.Cells["DateRange"].Appearance.Image = this.DynamicToPlanImage;
                                break;
                            default:
                                row.Cells["DateRange"].Appearance.Image = null;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        /// <summary>
        /// Creates a list for use on the "Version" column, which is a dropdown.
        /// </summary>
        private void CreateComboLists()
        {
            //Add a list to the grid, and name it "Merchandise".
            ugBasisNodeVersion.DisplayLayout.ValueLists.Add("Merchandise");
            _merchDataTable2 = MerchandiseDataTable.Copy();
            HierarchyNodeProfile hnp;
            foreach (DataRow row in _basisDataTable.Rows)
            {
                if ((int)row["BasisHNRID"] != Include.NoRID)
                {
                    //Begin Track #5378 - color and size not qualified
                    //					hnp = SAB.HierarchyServerSession.GetNodeData((int)row["BasisHNRID"] );
                    hnp = SAB.HierarchyServerSession.GetNodeData((int)row["BasisHNRID"], true, true);
                    //End Track #5378
                    AddNodeToMerchandiseCombo2(hnp);

                }
            }
            foreach (DataRow row in _merchDataTable2.Rows)
            {
                Infragistics.Win.ValueListItem vli = new Infragistics.Win.ValueListItem();
                vli.DataValue = row["seqno"];
                vli.DisplayText = Convert.ToString(row["text"], CultureInfo.CurrentUICulture);
                vli.Tag = Convert.ToString(row["key"], CultureInfo.CurrentUICulture);
                ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Add(vli);
            }

            //Add a list to the grid, and name it "Version".
            ugBasisNodeVersion.DisplayLayout.ValueLists.Add("Version");

            //Get the versions from the database.
            ForecastVersion fv = new ForecastVersion();
            DataTable VersionSource;
            VersionSource = fv.GetForecastVersions();

            //Loop through the datatable and manually add value and text to the lists.
            for (int i = 0; i < VersionSource.Rows.Count; i++)
            {
                Infragistics.Win.ValueListItem vli = new Infragistics.Win.ValueListItem();

                vli.DataValue = VersionSource.Rows[i]["FV_RID"];
                vli.DisplayText = Convert.ToString(VersionSource.Rows[i]["DESCRIPTION"], CultureInfo.CurrentUICulture);
                ugBasisNodeVersion.DisplayLayout.ValueLists["Version"].ValueListItems.Add(vli);
                vli.Dispose();
            }

            //no longer need this temporary datatable.
            VersionSource.Dispose();
        }

        private void SetGridComboToLevel(DataRow aRow, int seq)
        {
            try
            {
                DataRow myDataRow;
                for (int levIndex = 0;
                    levIndex < _merchDataTable2.Rows.Count; levIndex++)
                {
                    myDataRow = _merchDataTable2.Rows[levIndex];
                    if (Convert.ToInt32(myDataRow["seqno"], CultureInfo.CurrentUICulture) == seq)
                    {
                        //aRow["Merchandise"] = myDataRow["text"];
                        aRow["Merchandise"] = myDataRow["seqno"];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        private void BuildBasisContextMenu()
        {
            MenuItem mnuItemInsert = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert));
            MenuItem mnuItemInsertBefore = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_Before));
            MenuItem mnuItemInsertAfter = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_after));
            MenuItem mnuItemDelete = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Delete));
            MenuItem mnuItemDeleteAll = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_DeleteAll));
            mnuBasisGrid.MenuItems.Add(mnuItemInsert);
            mnuItemInsert.MenuItems.Add(mnuItemInsertBefore);
            mnuItemInsert.MenuItems.Add(mnuItemInsertAfter);
            mnuBasisGrid.MenuItems.Add(mnuItemDelete);
            mnuBasisGrid.MenuItems.Add(mnuItemDeleteAll);
            mnuItemInsert.Click += new System.EventHandler(this.mnuBasisGridItemInsert_Click);
            mnuItemInsertBefore.Click += new System.EventHandler(this.mnuBasisGridItemInsertBefore_Click);
            mnuItemInsertAfter.Click += new System.EventHandler(this.mnuBasisGridItemInsertAfter_Click);
            mnuItemDelete.Click += new System.EventHandler(this.mnuBasisGridItemDelete_Click);
            mnuItemDeleteAll.Click += new System.EventHandler(this.mnuBasisGridItemDeleteAll_Click);
        }
        private void mnuBasisGridItemInsert_Click(object sender, System.EventArgs e)
        {
        }

        // BEGIN Issue 4391/4393 stodd 10.4.2007
        private void mnuBasisGridItemInsertBefore_Click(object sender, System.EventArgs e)
        {
            if (_matrixProcessed && !_changedByCode)
            {
                if (!ReprocessWarningOK("the Basis"))
                {
                    // Do not insert
                }
                else
                {
                    InsertBasisNode(true);
                }
            }
            else
            {
                InsertBasisNode(true);
            }
            _changedByCode = false;
        }

        private void mnuBasisGridItemInsertAfter_Click(object sender, System.EventArgs e)
        {
            if (_matrixProcessed && !_changedByCode)
            {
                if (!ReprocessWarningOK("the Basis"))
                {
                    // Do not insert
                }
                else
                {
                    InsertBasisNode(false);
                }
            }
            else
            {
                InsertBasisNode(false);
            }
            _changedByCode = false;
        }
        // END Issue 4391/4393

        private void InsertBasisNode(bool InsertBeforeRow)
        {
            _setRowPosition = false;
            int rowPosition = 0;
            try
            {
                if (this.ugBasisNodeVersion.Rows.Count > 0)
                {
                    if (this.ugBasisNodeVersion.ActiveRow == null) return;
                    rowPosition = Convert.ToInt32(this.ugBasisNodeVersion.ActiveRow.Cells["BasisSequence"].Value, CultureInfo.CurrentUICulture);
                    int seq;
                    foreach (DataRow row in _basisDataTable.Rows)
                    {
                        seq = (int)row["BasisSequence"];
                        if (InsertBeforeRow)
                        {
                            if (seq >= rowPosition)
                                row["BasisSequence"] = seq + 1;
                        }
                        else
                        {
                            if (seq > rowPosition)
                                row["BasisSequence"] = seq + 1;
                        }
                    }
                }
                DataRow addedRow = _basisDataTable.NewRow();
                addedRow["BasisSequence"] = rowPosition;
                addedRow["BasisHNRID"] = Include.NoRID;
                addedRow["BasisPHRID"] = Include.NoRID;
                addedRow["BasisPHLSequence"] = 0;
                // BEGIN Issue 4818
                addedRow["cdrRID"] = Include.NoRID;
                addedRow["Weight"] = 1;
                // END Issue 4818
                // Copy OTS Plan Merchandise to 1st inserted row only 
                if (this.ugBasisNodeVersion.Rows.Count == 0)
                {
                    // BEGIN MID Track #3102 - Remove OTS Plan Section 					 
                    //int rowseq = cboOTSPlan.SelectedIndex;
                    int rowseq = 0;
                    // END MID Track #3102  
                    SetGridComboToLevel(addedRow, rowseq);
                    DataRow row = _merchDataTable2.Rows[rowseq];
                    if (row != null)
                    {
                        eMerchandiseType MerchandiseType = (eMerchandiseType)(Convert.ToInt32(row["leveltypename"], CultureInfo.CurrentUICulture));

                        switch (MerchandiseType)
                        {
                            case eMerchandiseType.Node:
                                addedRow["BasisHNRID"] = Convert.ToInt32(row["key"], CultureInfo.CurrentUICulture);
                                addedRow["BasisPHRID"] = Include.NoRID;
                                addedRow["BasisPHLSequence"] = 0;
                                break;
                            case eMerchandiseType.HierarchyLevel:
                                addedRow["BasisPHRID"] = HP.Key;
                                addedRow["BasisPHLSequence"] = Convert.ToInt32(row["key"], CultureInfo.CurrentUICulture);
                                addedRow["BasisHNRID"] = Include.NoRID;
                                break;
                            case eMerchandiseType.OTSPlanLevel:
                                addedRow["BasisHNRID"] = Include.NoRID;
                                addedRow["BasisPHRID"] = Include.NoRID;
                                addedRow["BasisPHLSequence"] = 0;
                                break;
                        }
                    }
                }
                _basisDataTable.Rows.Add(addedRow);
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].SortedColumns.Clear();
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].SortedColumns.Add("BasisSequence", false);
                _setRowPosition = true;
                BasisChangesMade = true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        // BEGIN Issue 4391/4393 stodd 10.4.2007
        private void mnuBasisGridItemDelete_Click(object sender, System.EventArgs e)
        {
            if (ugBasisNodeVersion.Selected.Rows.Count > 0)
            {
                if (_matrixProcessed && !_changedByCode)
                {
                    if (!ReprocessWarningOK("the Basis"))
                    {
                        // Do not insert
                    }
                    else
                    {
                        ugBasisNodeVersion.DeleteSelectedRows();
                        _basisDataTable.AcceptChanges();
                        BasisChangesMade = true;
                    }
                }
                else
                {
                    ugBasisNodeVersion.DeleteSelectedRows();
                    _basisDataTable.AcceptChanges();
                    BasisChangesMade = true;
                }
                _changedByCode = false;
            }
        }

        //TT#262 - Added to refresh the Allocation Percentage of Total values in the matrix after an allocation is performed - apicchetti
        private void FormActivate(object sender, System.EventArgs e)
        {
            // Begin TT# 4730 - stodd - NullReferenceException in Velocity after opening group as Headers in Style Review
            // Form is loaded and "GA Mode" is visible
            if (FormLoaded && lblGA.Visible)
            {
                if (AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID != Include.NoRID)
                {
                    bool groupHeaderFound = false;

                    ProfileList apl = _trans.GetMasterProfileList(eProfileType.Allocation);
                    foreach (AllocationProfile ap in apl.ArrayList)
                    {
                        if (ap.HeaderType == eHeaderType.Assortment)
                        {
                            groupHeaderFound = true;
                            break;
                        }
                    }

                    if (!groupHeaderFound)
                    {
                        _selectedHeaderList = SAB.AssortmentSelectedHeaderEvent.GetSelectedHeaders(this, AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID, eMethodType.Velocity);
                        CheckInteractive(false);
                    }
                }
            }
            // End TT# 4730 - stodd - NullReferenceException in Velocity after opening group as Headers in Style Review

            //check the activated from flag
            if (ActivatedFromStyleView == true)
            {
                //get the allocation subtotal profile object
                AllocationSubtotalProfile asp = _trans.GetAllocationGrandTotalProfile();
                //Begin tt#321 - JSmith - Velocity Interactive-> severe error when layering
                //asp.RebuildSubtotalsNeeded();
                //End tt#321

                //running total of allocation percent of total values
                double APT_Total = 0;

                //loop thru the data source and set the allocation percentage of total values
                foreach (DataRow row in _screenMatrixDataTable.Rows)
                {
                    string strGrade = row["Grade"].ToString().Trim();
                    if (strGrade != "Total:")
                    {
                        double dbValue = asp.GetStoreListVelocityAllocationPercentOfTotal(_prevAttributeValue, _prevSetValue, strGrade);
                        APT_Total += dbValue;
                        row["AllocationPercentOfTotal"] = dbValue.ToString().Trim();
                    }
                    else
                    {
                        row["AllocationPercentOfTotal"] = APT_Total;
                    }
                }

                //reinitialize data source
                this.ugMatrix.BeginUpdate();
                this.ugMatrix.DataSource = null;
                this.ugMatrix.DataSource = _screenMatrixDataTable;
                _addBandGroups = true;
                FormatScreenMatrix();
                gbxMatrixDefault.Enabled = (_screenMatrixDataTable.Rows.Count > 0);
                this.ugMatrix.EndUpdate();

                //reset activated location flag
                ActivatedFromStyleView = false;
            }
            // Begin TT#3138 - RMatelic - Velocity Detail when you go in to the screen the scroll bar is the length of the screen and you can't scroll
            if (_windowIsMaximized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            // End TT#3138
        }

        private void mnuBasisGridItemDeleteAll_Click(object sender, System.EventArgs e)
        {
            if (ugBasisNodeVersion.Selected.Rows.Count > 0)
            {
                if (_matrixProcessed && !_changedByCode)
                {
                    if (!ReprocessWarningOK("the Basis"))
                    {
                        // Do not insert
                    }
                    else
                    {
                        _basisDataTable.Rows.Clear();
                        _basisDataTable.AcceptChanges();
                        BasisChangesMade = true;
                    }
                }
                else
                {
                    _basisDataTable.Rows.Clear();
                    _basisDataTable.AcceptChanges();
                    BasisChangesMade = true;
                }
                _changedByCode = false;
            }
        }
        // END Issue 4391/4393


        #endregion Basis Tab

        #region Velocity Grades Tab

        private void ugVelocityGrades_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

            DefaultVelocityGradesGridLayout();

            //this.ugVelocityGrades.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.ugVelocityGrades.DisplayLayout.Bands[0].Columns["RowPosition"].Hidden = true;
            //_velocityGradesGridBuilt = true;
        }

        private void DefaultVelocityGradesGridLayout()
        {
            this.ugVelocityGrades.DisplayLayout.Appearance.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
            this.ugVelocityGrades.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Default;
            this.ugVelocityGrades.DisplayLayout.Bands[0].Columns["Grade"].Width = 75;
            this.ugVelocityGrades.DisplayLayout.Bands[0].Columns["Boundary"].Width = 75;
            // BEGIN MID Track #3640 - get label text
            this.ugVelocityGrades.DisplayLayout.Bands[0].Columns["Grade"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Grade);
            this.ugVelocityGrades.DisplayLayout.Bands[0].Columns["Boundary"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Boundary);
            // END MID Track #3640
            // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
            this.ugVelocityGrades.DisplayLayout.Bands[0].Columns["MinStock"].Width = 80;
            this.ugVelocityGrades.DisplayLayout.Bands[0].Columns["MinStock"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Min_Stock);
            this.ugVelocityGrades.DisplayLayout.Bands[0].Columns["MaxStock"].Width = 80;
            this.ugVelocityGrades.DisplayLayout.Bands[0].Columns["MaxStock"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Max_Stock);
            this.ugVelocityGrades.DisplayLayout.Bands[0].Columns["MinAd"].Width = 75;
            this.ugVelocityGrades.DisplayLayout.Bands[0].Columns["MinAd"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Min_Ad);
            // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
        }

        private void ugSellThruPcts_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            DefaultSellThruPctsGridLayout();

            //this.ugSellThruPcts.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.ugSellThruPcts.DisplayLayout.Bands[0].Columns["RowPosition"].Hidden = true;
            //_sellThruPctsGridBuilt = true;
        }

        private void DefaultSellThruPctsGridLayout()
        {
            this.ugSellThruPcts.DisplayLayout.Appearance.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
            this.ugSellThruPcts.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Default;

            this.ugSellThruPcts.DisplayLayout.Bands[0].Columns["SellThruIndex"].Header.VisiblePosition = 0;
            this.ugSellThruPcts.DisplayLayout.Bands[0].Columns["SellThruIndex"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Sell_Thru_Pct);
            this.ugSellThruPcts.DisplayLayout.Bands[0].Columns["SellThruIndex"].Width = this.ugSellThruPcts.DisplayLayout.Bands[0].Columns["SellThruIndex"].CalculateAutoResizeWidth(1, true);
            this.ugSellThruPcts.Width = this.ugSellThruPcts.DisplayLayout.Bands[0].Columns["SellThruIndex"].Width + 85;

        }

        private void VelocityGradesTab_Load()
        {
            this.ugVelocityGrades.DataSource = _dsVelocity.Tables["VelocityGrade"];
            this.ugSellThruPcts.DataSource = _dsVelocity.Tables["SellThru"];

            this.ugVelocityGrades.DisplayLayout.AddNewBox.Hidden = false;
            this.ugVelocityGrades.DisplayLayout.GroupByBox.Hidden = true;
            this.ugVelocityGrades.DisplayLayout.GroupByBox.Prompt = "";
            this.ugVelocityGrades.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugVelocityGrades.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_VelocityGrade);

            this.ugSellThruPcts.DisplayLayout.AddNewBox.Hidden = false;
            this.ugSellThruPcts.DisplayLayout.GroupByBox.Hidden = true;
            this.ugSellThruPcts.DisplayLayout.GroupByBox.Prompt = "";
            this.ugSellThruPcts.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugSellThruPcts.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_Sell_Thru_Pct);
            this.ugSellThruPcts.DisplayLayout.Bands[0].AddButtonToolTipText = MIDText.GetTextOnly(eMIDTextCode.lbl_Sell_Thru_Pct);

            //this.ugVelocityGrades.DisplayLayout.Bands[0].Override.TipStyleRowConnector = TipStyle.Show;
            //this.ugVelocityGrades.DisplayLayout.Bands[0].Override.TipStyleScroll = TipStyle.Show;
            //this.ugVelocityGrades.DisplayLayout.Bands[0].Override.TipStyleCell = TipStyle.Show;
            if (!FunctionSecurity.AllowUpdate)
            {
                foreach (UltraGridBand ugb in this.ugVelocityGrades.DisplayLayout.Bands)
                {
                    ugb.Override.AllowDelete = DefaultableBoolean.False;
                }

                foreach (UltraGridBand ugb in this.ugSellThruPcts.DisplayLayout.Bands)
                {
                    ugb.Override.AllowDelete = DefaultableBoolean.False;
                }
            }

            if (FunctionSecurity.AllowUpdate)
            {
                BuildVelocityGradesContextmenu();
                this.ugVelocityGrades.ContextMenu = mnuVelocityGradesGrid;

                BuildSellThruPctsContextmenu();
                this.ugSellThruPcts.ContextMenu = mnuSellThruPctsGrid;
            }

            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //FormatColumns(this.ugVelocityGrades);
            //FormatColumns(this.ugSellThruPcts);
            //End TT#169
            _velocityGradesIsPopulated = true;
            _sellThruPctsIsPopulated = true;
        }
        private void VelocityGrades_Populate(int nodeRID)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int count = 0;

                _dsVelocity.Tables["VelocityGrade"].Clear();
                // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                // _velocityGradeList = SAB.HierarchyServerSession.GetVelocityGradeList(nodeRID, false);
                _velocityGradeList = SAB.HierarchyServerSession.GetVelocityGradeList(nodeRID, true);
                // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                foreach (VelocityGradeProfile vgp in _velocityGradeList)
                {
                    // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                    //_dsVelocity.Tables["VelocityGrade"].Rows.Add(new object[] { count, vgp.VelocityGrade, vgp.Boundary}); 
                    _dsVelocity.Tables["VelocityGrade"].Rows.Add(new object[] { count, vgp.VelocityGrade, vgp.Boundary, vgp.VelocityMinStock, vgp.VelocityMaxStock, vgp.VelocityMinAd });
                    // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                    ++count;
                }

                // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                foreach (DataRow vgRow in _dsVelocity.Tables["VelocityGrade"].Rows)
                {
                    if (Convert.ToInt32(vgRow["MinStock"]) == -1)
                    { vgRow["MinStock"] = DBNull.Value; }
                    if (Convert.ToInt32(vgRow["MaxStock"]) == -1)
                    { vgRow["MaxStock"] = DBNull.Value; }
                    if (Convert.ToInt32(vgRow["MinAd"]) == -1)
                    { vgRow["MinAd"] = DBNull.Value; }
                }
                // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)

                this.ugVelocityGrades.DataSource = _dsVelocity.Tables["VelocityGrade"];
                FunctionSecurityProfile securityLevel = FunctionSecurity;

                if (securityLevel.AllowUpdate)
                {
                    this.ugVelocityGrades.DisplayLayout.AddNewBox.Hidden = false;
                }
                else
                {
                    this.ugVelocityGrades.DisplayLayout.AddNewBox.Hidden = true;
                }
                _rebuildMatrix = true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void SellThruPcts_Populate(int nodeRID)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int count = 0;

                _dsVelocity.Tables["SellThru"].Clear();
                _sellThruPctList = SAB.HierarchyServerSession.GetSellThruPctList(nodeRID, false);
                foreach (SellThruPctProfile stpp in _sellThruPctList)
                {
                    _dsVelocity.Tables["SellThru"].Rows.Add(new object[] { count, stpp.SellThruPct });
                    ++count;
                }

                this.ugSellThruPcts.DataSource = _dsVelocity.Tables["SellThru"];

                FunctionSecurityProfile securityLevel = FunctionSecurity;

                if (securityLevel.AllowUpdate)
                {
                    this.ugSellThruPcts.DisplayLayout.AddNewBox.Hidden = false;
                }
                else
                {
                    this.ugSellThruPcts.DisplayLayout.AddNewBox.Hidden = true;
                }
                _rebuildMatrix = true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void BuildVelocityGradesContextmenu()
        {
            MenuItem mnuItemInsert = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert));
            MenuItem mnuItemInsertBefore = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_Before));
            MenuItem mnuItemInsertAfter = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_after));
            MenuItem mnuItemDelete = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Delete));
            MenuItem mnuItemDeleteAll = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_DeleteAll));
            mnuVelocityGradesGrid.MenuItems.Add(mnuItemInsert);
            mnuItemInsert.MenuItems.Add(mnuItemInsertBefore);
            mnuItemInsert.MenuItems.Add(mnuItemInsertAfter);
            mnuVelocityGradesGrid.MenuItems.Add(mnuItemDelete);
            mnuVelocityGradesGrid.MenuItems.Add(mnuItemDeleteAll);
            mnuItemInsert.Click += new System.EventHandler(this.mnuVGItemInsert_Click);
            mnuItemInsertBefore.Click += new System.EventHandler(this.mnuVGItemInsertBefore_Click);
            mnuItemInsertAfter.Click += new System.EventHandler(this.mnuVGItemInsertAfter_Click);
            mnuItemDelete.Click += new System.EventHandler(this.mnuVGItemDelete_Click);
            mnuItemDeleteAll.Click += new System.EventHandler(this.mnuVGItemDeleteAll_Click);
        }

        private void mnuVGItemInsert_Click(object sender, System.EventArgs e)
        {

        }

        private void mnuVGItemInsertBefore_Click(object sender, System.EventArgs e)
        {
            _setRowPosition = false;
            int rowPosition = 0;
            if (this.ugVelocityGrades.Rows.Count > 0)
            {
                if (this.ugVelocityGrades.ActiveRow == null) return;
                rowPosition = Convert.ToInt32(this.ugVelocityGrades.ActiveRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture);
                // increment the position of the active row to end of grid
                foreach (UltraGridRow gridRow in ugVelocityGrades.Rows)
                {
                    if (Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) >= rowPosition)
                    {
                        gridRow.Cells["RowPosition"].Value = Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) + 1;
                    }
                }
            }
            UltraGridRow addedRow = this.ugVelocityGrades.DisplayLayout.Bands[0].AddNew();
            addedRow.Cells["RowPosition"].Value = rowPosition;
            this.ugVelocityGrades.DisplayLayout.Bands[0].SortedColumns.Clear();
            this.ugVelocityGrades.DisplayLayout.Bands[0].SortedColumns.Add("RowPosition", false);
            _setRowPosition = true;
        }

        private void mnuVGItemInsertAfter_Click(object sender, System.EventArgs e)
        {
            _setRowPosition = false;
            int rowPosition = 0;
            if (this.ugVelocityGrades.Rows.Count > 0)
            {
                if (this.ugVelocityGrades.ActiveRow == null) return;
                rowPosition = Convert.ToInt32(this.ugVelocityGrades.ActiveRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture);
                // increment the position of the active row to end of grid
                foreach (UltraGridRow gridRow in ugVelocityGrades.Rows)
                {
                    if (Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) > rowPosition)
                    {
                        gridRow.Cells["RowPosition"].Value = Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) + 1;
                    }
                }
            }
            UltraGridRow addedRow = this.ugVelocityGrades.DisplayLayout.Bands[0].AddNew();
            addedRow.Cells["RowPosition"].Value = rowPosition + 1;
            this.ugVelocityGrades.DisplayLayout.Bands[0].SortedColumns.Clear();
            this.ugVelocityGrades.DisplayLayout.Bands[0].SortedColumns.Add("RowPosition", false);
            _setRowPosition = true;
        }

        private void mnuVGItemDelete_Click(object sender, System.EventArgs e)
        {
            if (this.ugVelocityGrades.Selected.Rows.Count > 0)
                this.ugVelocityGrades.DeleteSelectedRows();
        }

        private void mnuVGItemDeleteAll_Click(object sender, System.EventArgs e)
        {
            _dsVelocity.Tables["VelocityGrade"].Rows.Clear();
            _dsVelocity.Tables["VelocityGrade"].AcceptChanges();
            VelocityGradesChangesMade = true;
            _rebuildMatrix = true;
        }
        private void BuildSellThruPctsContextmenu()
        {
            MenuItem mnuItemInsert = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert));
            MenuItem mnuItemInsertBefore = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_Before));
            MenuItem mnuItemInsertAfter = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_after));
            MenuItem mnuItemDelete = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Delete));
            MenuItem mnuItemDeleteAll = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_DeleteAll));
            mnuSellThruPctsGrid.MenuItems.Add(mnuItemInsert);
            mnuItemInsert.MenuItems.Add(mnuItemInsertBefore);
            mnuItemInsert.MenuItems.Add(mnuItemInsertAfter);
            mnuSellThruPctsGrid.MenuItems.Add(mnuItemDelete);
            mnuSellThruPctsGrid.MenuItems.Add(mnuItemDeleteAll);
            mnuItemInsert.Click += new System.EventHandler(this.mnuSTItemInsert_Click);
            mnuItemInsertBefore.Click += new System.EventHandler(this.mnuSTItemInsertBefore_Click);
            mnuItemInsertAfter.Click += new System.EventHandler(this.mnuSTItemInsertAfter_Click);
            mnuItemDelete.Click += new System.EventHandler(this.mnuSTItemDelete_Click);
            mnuItemDeleteAll.Click += new System.EventHandler(this.mnuSTItemDeleteAll_Click);
        }

        private void mnuSTItemInsert_Click(object sender, System.EventArgs e)
        {
        }

        private void mnuSTItemInsertBefore_Click(object sender, System.EventArgs e)
        {
            _setRowPosition = false;
            int rowPosition = 0;
            if (this.ugSellThruPcts.Rows.Count > 0)
            {
                if (this.ugSellThruPcts.ActiveRow == null) return;
                rowPosition = Convert.ToInt32(this.ugSellThruPcts.ActiveRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture);
                // increment the position of the active row to end of grid
                foreach (UltraGridRow gridRow in ugSellThruPcts.Rows)
                {
                    if (Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) >= rowPosition)
                    {
                        gridRow.Cells["RowPosition"].Value = Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) + 1;
                    }
                }
            }
            UltraGridRow addedRow = this.ugSellThruPcts.DisplayLayout.Bands[0].AddNew();
            addedRow.Cells["RowPosition"].Value = rowPosition;
            this.ugSellThruPcts.DisplayLayout.Bands[0].SortedColumns.Clear();
            this.ugSellThruPcts.DisplayLayout.Bands[0].SortedColumns.Add("RowPosition", false);
            _setRowPosition = true;
        }

        private void mnuSTItemInsertAfter_Click(object sender, System.EventArgs e)
        {
            _setRowPosition = false;
            int rowPosition = 0;
            if (this.ugSellThruPcts.Rows.Count > 0)
            {
                if (this.ugSellThruPcts.ActiveRow == null) return;
                rowPosition = Convert.ToInt32(this.ugSellThruPcts.ActiveRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture);
                // increment the position of the active row to end of grid
                foreach (UltraGridRow gridRow in ugSellThruPcts.Rows)
                {
                    if (Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) > rowPosition)
                    {
                        gridRow.Cells["RowPosition"].Value = Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) + 1;
                    }
                }
            }
            UltraGridRow addedRow = this.ugSellThruPcts.DisplayLayout.Bands[0].AddNew();
            addedRow.Cells["RowPosition"].Value = rowPosition + 1;
            this.ugSellThruPcts.DisplayLayout.Bands[0].SortedColumns.Clear();
            this.ugSellThruPcts.DisplayLayout.Bands[0].SortedColumns.Add("RowPosition", false);
            _setRowPosition = true;
        }

        private void mnuSTItemDelete_Click(object sender, System.EventArgs e)
        {
            if (this.ugSellThruPcts.Selected.Rows.Count > 0)
                this.ugSellThruPcts.DeleteSelectedRows();
        }

        private void mnuSTItemDeleteAll_Click(object sender, System.EventArgs e)
        {
            this.ugSellThruPcts.BeginUpdate();
            if (_dsVelocity.Tables["SellThru"].Rows.Count > 0)
            {
                _dsVelocity.Tables["SellThru"].Rows.Clear();
            }
            _dsVelocity.Tables["SellThru"].AcceptChanges();
            this.ugSellThruPcts.EndUpdate();
            VelocityGradesChangesMade = true;
            _rebuildMatrix = true;
        }

        private void mnuSTItemExpandAll_Click(object sender, System.EventArgs e)
        {
            this.ugSellThruPcts.Rows.ExpandAll(true);
        }

        private void mnuSTItemCollapseAll_Click(object sender, System.EventArgs e)
        {
            this.ugSellThruPcts.Rows.CollapseAll(true);
        }

        private void ugVelocityGrades_AfterSortChange(object sender, Infragistics.Win.UltraWinGrid.BandEventArgs e)
        {
            int count = 0;
            if (_setRowPosition)
            {
                foreach (UltraGridRow gridRow in ugVelocityGrades.Rows)
                {
                    gridRow.Cells["RowPosition"].Value = count;
                    ++count;
                }
            }
        }

        private void ugSellThruPcts_AfterSortChange(object sender, Infragistics.Win.UltraWinGrid.BandEventArgs e)
        {
            int count = 0;
            if (_setRowPosition)
            {
                foreach (DataRow row in _sellThruPctsDataTable.Rows)
                {
                    row["RowPosition"] = count;
                    count++;
                }
            }
        }
        #endregion Velocity Grades Tab

        #region Matrix Tab


        private void ugGroupData_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
            //ugld.ApplyDefaults(e);
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
            //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
            // End TT#1164
            //End TT#169
            this.ugGroupData.DisplayLayout.GroupByBox.Hidden = true;

            this.ugGroupData.DisplayLayout.Bands[0].Override.RowSelectors = DefaultableBoolean.False;
            this.ugGroupData.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;

            this.ugGroupData.DisplayLayout.Bands[0].Columns["RowHeader"].Width = 60;
            this.ugGroupData.DisplayLayout.Bands[0].Columns["RowHeader"].Header.Caption = " ";

            this.ugGroupData.DisplayLayout.Bands[0].Columns["AvgWOS"].Width = 60;
            this.ugGroupData.DisplayLayout.Bands[0].Columns["AvgWOS"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_AvgWOS);

            this.ugGroupData.DisplayLayout.Bands[0].Columns["PctSellThru"].Header.VisiblePosition = 2;
            this.ugGroupData.DisplayLayout.Bands[0].Columns["PctSellThru"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Sell_Thru_Pct);
            this.ugGroupData.DisplayLayout.Bands[0].Columns["PctSellThru"].Width = this.ugGroupData.DisplayLayout.Bands[0].Columns["PctSellThru"].CalculateAutoResizeWidth(1, true);

            foreach (UltraGridRow row in ugGroupData.Rows)
            {
                row.Cells["RowHeader"].Activation = Activation.Disabled;
                row.Cells["AvgWOS"].Activation = Activation.Disabled;
                row.Cells["PctSellThru"].Activation = Activation.Disabled;
            }
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //FormatColumns(this.ugGroupData);
            //End TT#169
        }

        private void ugMatrix_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
            //ugld.ApplyDefaults(e);
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
            //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 1, true);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 1, false);
            // End TT#1164
            //End TT#169
            DefaultMatrixGridLayout();

            //			this.ugMatrix.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            //			this.ugMatrix.DisplayLayout.Bands[0].Columns["RowPosition"].Hidden = true;
            //_matrixGridBuilt = true;
        }

        private void DefaultMatrixGridLayout()
        {
            this.ugMatrix.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_VelocityMatrix);  // TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
            this.ugMatrix.DisplayLayout.Appearance.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
            this.ugMatrix.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Default;
        }

        public void ugMatrix_AddTotalLine(bool Exception) //TT#299 - stodd - null ref changing attribute set
        {
            if (Exception == false)
            {
                //subtotal variable
                // begin TT#533 Velocity variables not calculated correctly
                //double totalsales_set = 0;
                //double totalstores_set = 0;
                //double totalsales_all = 0;
                //double totalstores_all = 0;
                //double avgsalesidx_set = 0;
                //double totalstock_all = 0;
                //double totalstock_set = 0;
                // end TT#533 Velocity variables not calculated correctly
                // begin TT#586 Velocity variables not calculated correctly
                //double totalalloc_set = 0;
                //double totalalloc_all = 0;
                //Hashtable sellThruList_totalStores = null;
                //Hashtable sellThruAvgWOSList = null;
                // end TT#586 Velocity variables not calculated correctly

                //DataTable dtSellThruStuff = _dsVelocity.Tables["SellThru"];  // TT#586 velocity variables not calculated correctly

                //create a new row for totals
                DataRow summaryRow = _screenMatrixDataTable.NewRow();
                summaryRow["Grade"] = "Total:";


                //loop thru the table and delete the totals row if it exists
                foreach (DataRow row in _screenMatrixDataTable.Rows)
                {
                    if (row["Grade"].ToString().Trim() == "Total:")  //TT#319 - added this check so that only the total line would be inactivated - apicchetti
                    {
                        _screenMatrixDataTable.Rows.Remove(row);
                        break;
                    }
                }

                // begin TT#586 Velocity variables not calculated correctly
                int sellThruIdx;

        //Begin TT#2670 - RBeck - Error in the Velocity Interactive Method 
                bool setSummary = false;

                if (_trans != null)
                {
                    if (_trans.VelocityCriteriaExists == true)
                    {
                        setSummary = true;
                    }
                }

                // if (_trans != null)
                if (setSummary == true)
        //End   TT#2670 - RBeck - Error in the Velocity Interactive Method 
                {
                    summaryRow["TotalSales"] = _trans.VelocityGetMatrixGradeTotBasisSales(_prevSetValue, null);
                    summaryRow["AvgSales"] = _trans.VelocityGetMatrixGradeAvgBasisSales(_prevSetValue, null);
                    summaryRow["PctTotalSales"] = _trans.VelocityGetMatrixGradeAvgBasisSalesPctTot(_prevSetValue, null);
                    summaryRow["AvgSalesIdx"] = _trans.VelocityGetMatrixGradeAvgBasisSalesIdx(_prevSetValue, null);
                    summaryRow["TotalNumStores"] = _trans.VelocityGetMatrixGradeTotalNumberOfStores(_prevSetValue, null);
                    summaryRow["StockPercentOfTotal"] = _trans.VelocityGetMatrixGradeStockPercentOfTotal(_prevSetValue, null);
                    summaryRow["AvgStock"] = _trans.VelocityGetMatrixGradeAvgStock(_prevSetValue, null);
                    summaryRow["AllocationPercentOfTotal"] = _trans.Velocity.GetSetAllocatedPctOfTotal(_prevAttributeValue, _prevSetValue, null);
                    for (int sellThruRow = 0; sellThruRow < _sellThruPctsDataTable.Rows.Count; sellThruRow++)
                    {
                        sellThruIdx = Convert.ToInt32(_sellThruPctsDataTable.Rows[sellThruRow]["SellThruIndex"]);
                        summaryRow["Stores" + sellThruIdx.ToString()] = _trans.VelocityGetSellThruTotalStores(_prevSetValue, sellThruIdx);
                        summaryRow["AvgWOS" + sellThruIdx.ToString()] = _trans.VelocityGetSellThruAvgWOS(_prevSetValue, sellThruIdx);
                    }
                }
                else
                {
                    summaryRow["TotalSales"] = 0;
                    summaryRow["AvgSales"] = 0;
                    summaryRow["PctTotalSales"] = 0;
                    summaryRow["AvgSalesIdx"] = 0;
                    summaryRow["TotalNumStores"] = 0;
                    summaryRow["StockPercentOfTotal"] = 0;
                    summaryRow["AvgStock"] = 0;
                    summaryRow["AllocationPercentOfTotal"] = 0;
                    for (int sellThruRow = 0; sellThruRow < _sellThruPctsDataTable.Rows.Count; sellThruRow++)
                    {
                        sellThruIdx = Convert.ToInt32(_sellThruPctsDataTable.Rows[sellThruRow]["SellThruIndex"]);
                        summaryRow["Stores" + sellThruIdx.ToString()] = 0;
                        summaryRow["AvgWOS" + sellThruIdx.ToString()] = 0;
                    }
                }
                // end TT#586 Velocity variables not calculated correctly



                // begin TT#586 velocity variables not calculated correctly
                //if(_trans != null)              /*TT#332 - apicchetti - I dropped all the afterprocess flags
                //                                 instead I am looking at the transactions to be present or not*/
                ////if (_afterProcess == true)	//TT#299 - stodd - null ref changing attribute set
                //{
                //    DataTable store_sell_thrus_set = (DataTable)_trans.StoresSellThru[0];
                //    DataTable store_sell_thrus = (DataTable)_trans.StoresSellThru[1];

                //    //get subtotals
                //    // begin TT#533 Velocity variables not calculated correctly
                //    //totalsales_set = _velocityMethod.GetTotalSales_Set(_trans, _prevAttributeValue, _prevSetValue);  
                //    //totalstores_set = _velocityMethod.GetTotalStores_Set(_trans, _prevAttributeValue, _prevSetValue);
                //    //totalstores_all = _velocityMethod.GetTotalStores_All(_trans);
                //    //totalsales_all = _velocityMethod.GetTotalSales_All(_trans);
                //    //avgsalesidx_set = _velocityMethod.GetAvgSalesIndex_Set(_trans, _prevAttributeValue, _prevSetValue);
                //    //totalstock_all = _velocityMethod.GetTotalStock_All(_trans);
                //    //totalstock_set = _velocityMethod.GetTotalStock_Set(_trans, _prevAttributeValue, _prevSetValue);
                //    // begin TT#586 Velocity variables not calculated correctly
                //    //totalalloc_set = _velocityMethod.GetAllocated_Set(_trans, _prevAttributeValue, _prevSetValue);
                //    //totalalloc_all = _velocityMethod.GetAllocated_All(_trans);
                //    //// end TT#533 Velocity variables not calculated correctly
                //    //sellThruList_totalStores = _velocityMethod.GetTotalNumberStores_SellThru(_trans, _prevAttributeValue, _prevSetValue, _trans.StoresSellThru);
                //    //sellThruAvgWOSList = _velocityMethod.GetAvgWOS_SellThru(_trans, _prevAttributeValue, _prevSetValue, _trans.StoresSellThru);
                //    // TT#586 velocity variables not calculated correctly
                //}
                // end TT#586 velocity variables not calculated correctly

                // begin TT#586 velocity variables not calculated correctly
                //if (_trans != null)                        // TT#533 Velocity variables not calculated correctly
                //{                                          // TT#533 Velocity variable not calculated correctly           
                //    foreach (string strColumn in alSumCols)
                //    {
                //        switch (strColumn)
                //        {
                //            case "TotalSales":
                //                //summaryRow[strColumn] = totalsales_set;  // TT#533 Velocity variables not calculated correctly
                //                summaryRow[strColumn] = _trans.VelocityGetMatrixGradeTotBasisSales(_prevSetValue, null); // TT#533 Velocity variables not calculated correctly
                //                break;
                //            case "AvgSales":
                //                // begin TT#533 Velocity variables not calculated correctly
                //                ////TT#299 - stodd - null ref changing attribute set
                //                //if (_trans != null && totalstores_set > 0)              /*TT#332 - apicchetti - I dropped all the afterprocess flags
                //                //                                                        instead I am looking at the transactions to be present or not*/
                //                ////if (_afterProcess == true && totalstores_set > 0)
                //                //{
                //                //    summaryRow[strColumn] = totalsales_set / totalstores_set; 
                //                //}
                //                //else
                //                //{
                //                //    summaryRow[strColumn] = 0;
                //                //}

                //                summaryRow[strColumn] = _trans.VelocityGetMatrixGradeAvgBasisSales(_prevSetValue, null);  // TT#533 Velocity variables not calculated correctly
                //                break;
                //            case "PctTotalSales":
                //                // begin TT#533 Velocity variables not calculated correctly
                //                ////TT#299 - stodd - null ref changing attribute set
                //                //if (_trans != null && totalsales_all > 0)   /*TT#332 - apicchetti - I dropped all the afterprocess flags
                //                //                                            instead I am looking at the transactions to be present or not*/
                //                ////if (_afterProcess == true && totalsales_all > 0)
                //                //{
                //                //    summaryRow[strColumn] = (totalsales_set / totalsales_all) * 100;
                //                //}
                //                //else
                //                //{
                //                //    summaryRow[strColumn] = 0;
                //                //}
                //                //summaryRow[strColumn] = _velocityMethod.GetMatrixGradeSalesPctOfTotal(_prevSetValue, null); 
                //                summaryRow[strColumn] = _trans.VelocityGetMatrixGradeAvgBasisSalesPctTot(_prevSetValue, null);
                //                // end TT#533 Velocity variables not calculated correctly
                //                break;
                //            case "AvgSalesIdx":
                //                // begin TT#533 Velocity variables not calculated correctly
                //                //summaryRow[strColumn] = avgsalesidx_set;
                //                summaryRow[strColumn] = _trans.VelocityGetMatrixGradeAvgBasisSalesIdx(_prevSetValue, null);
                //                // end TT#533 Velocity variables not calculatd correctly
                //                break;
                //            case "TotalNumStores":
                //                // begin TT#533 Velocity variables not calculated correctly
                //                //summaryRow[strColumn] = totalstores_set;
                //                summaryRow[strColumn] = _trans.VelocityGetMatrixGradeTotalNumberOfStores(_prevSetValue, null);
                //                // end TT#533 Velocity variables not calculated correctly
                //                break;
                //            case "StockPercentOfTotal":
                //                // begin TT#533 Velocity variables not calculated correctly
                //                ////TT#299 - stodd - null ref changing attribute set
                //                //if (_trans != null)              /*TT#332 - apicchetti - I dropped all the afterprocess flags
                //                //                                instead I am looking at the transactions to be present or not*/
                //                ////if (_afterProcess == true)
                //                //{
                //                //    summaryRow[strColumn] = _velocityMethod.GetStockPercentOfTotal(_trans, _prevAttributeValue, _prevSetValue);
                //                //}
                //                //else
                //                //{
                //                //    summaryRow[strColumn] = 0;
                //                //}
                //                summaryRow[strColumn] = _trans.VelocityGetMatrixGradeStockPercentOfTotal(_prevSetValue, null);
                //                // end TT#533 Velocity variables not calculated correctly
                //                break;
                //            case "AvgStock":
                //                // begin TT#533 Velocity variable not calculated correctly
                //                ////TT#299 - stodd - null ref changing attribute set
                //                //if (_trans != null && totalstores_set > 0)          /*TT#332 - apicchetti - I dropped all the afterprocess flags
                //                //                                                    instead I am looking at the transactions to be present or not*/
                //                ////if (_afterProcess == true && totalstores_set > 0)
                //                //{
                //                //    summaryRow[strColumn] = totalstock_set / totalstores_set;
                //                //}
                //                //else
                //                //{
                //                //    summaryRow[strColumn] = 0;
                //                //}
                //                summaryRow[strColumn] = _trans.VelocityGetMatrixGradeAvgStock(_prevSetValue, null);
                //                // end TT#533 Velocity variable not calculated correctly
                //                break;
                //            case "AllocationPercentOfTotal":
                //                // begin TT#586 Velocity Variables not calculated correctly
                //                //TT#299 - stodd - null ref changing attribute set
                //                //if (_trans != null && totalalloc_all > 0)   /*TT#332 - apicchetti - I dropped all the afterprocess flags
                //                //                                        instead I am looking at the transactions to be present or not*/
                //                ////if (_afterProcess == true && totalalloc_all > 0)
                //                //{
                //                //    summaryRow[strColumn] = (totalalloc_set / totalalloc_all) * 100;
                //                //}
                //                //else
                //                //{
                //                //    summaryRow[strColumn] = 0;
                //                //}
                //                summaryRow[strColumn] = _trans.Velocity.GetSetAllocatedPctOfTotal(_prevAttributeValue, _prevSetValue, null);
                //                // end TT#586 velocity variables not calculated correctly
                //                break;
                //        }
                //    } 
                //    // begin TT#533 Velocity variable not calculated correctly
                //}  
                //else  
                //{     
                //    foreach (string strColumn in alSumCols) 
                //    {
                //        summaryRow[strColumn] = 0;          
                //    }
                //}
                // end TT#533 Velcity variable not calculated correctly
                // end TT#586 Velocity variables not calculatd correcly

                // begin TT#586 Velocity Variables not calculated correctly
                ////initialize the sell thru totals to 0
                //foreach (string strColumn in alSumCols)
                //{
                //    if (strColumn.StartsWith("Stores") == true)
                //    {
                //        summaryRow[strColumn] = "0";
                //    }
                //    else if (strColumn.StartsWith("AvgWOS") == true)
                //    {
                //        summaryRow[strColumn] = "0.0";
                //    }
                //}

                //foreach (string strColumn in alSumCols)
                //{
                //    //TT#299 - stodd - null ref changing attribute set
                //    if (_trans != null)              /*TT#332 - apicchetti - I dropped all the afterprocess flags
                //                                        instead I am looking at the transactions to be present or not*/
                //    //if (_afterProcess == true)
                //    {
                //        for (int intSellThruData = 0; intSellThruData < _sellThruPctsDataTable.Rows.Count; intSellThruData++)
                //        {
                //            int SellThruDataKey = Convert.ToInt32(_sellThruPctsDataTable.Rows[intSellThruData]["RowPosition"]);
                //            int SellThruDataIdx = Convert.ToInt32(_sellThruPctsDataTable.Rows[intSellThruData]["SellThruIndex"]);

                //foreach (DictionaryEntry sellThru_totalStores in sellThruList_totalStores)
                //{
                //    int SellThruKey = Convert.ToInt32(sellThru_totalStores.Key);
                //    int SellThruStoreCount = Convert.ToInt32(sellThru_totalStores.Value);

                //    if (SellThruDataIdx == SellThruKey)
                //    {
                //        if (strColumn == "Stores" + SellThruKey)
                //        {
                //            summaryRow[strColumn] = SellThruStoreCount;
                //        }
                //    }
                //    else
                //    {
                //        if (strColumn == "Stores" + SellThruKey)
                //        {
                //            if (summaryRow[strColumn].ToString() == "")
                //            {
                //                summaryRow[strColumn] = "0";
                //            }
                //        }
                //    }
                //}
                //        }

                //        for (int intSellThruData = 0; intSellThruData < _sellThruPctsDataTable.Rows.Count; intSellThruData++)
                //        {
                //            int SellThruDataKey = Convert.ToInt32(_sellThruPctsDataTable.Rows[intSellThruData]["RowPosition"]);
                //            int SellThruDataIdx = Convert.ToInt32(_sellThruPctsDataTable.Rows[intSellThruData]["SellThruIndex"]);

                //            foreach (DictionaryEntry sellThruAvgWOS in sellThruAvgWOSList)
                //            {
                //                int SellThruKey = Convert.ToInt32(sellThruAvgWOS.Key);
                //                double SellThruAvgWOS = Convert.ToDouble(sellThruAvgWOS.Value);

                //                if (SellThruDataIdx == SellThruKey)
                //                {
                //                    if (strColumn == "AvgWOS" + SellThruKey)
                //                    {
                //                        summaryRow[strColumn] = SellThruAvgWOS;
                //                    }

                //                }
                //                else
                //                {
                //                    if (strColumn == "AvgWOS" + SellThruKey)
                //                    {
                //                        if (summaryRow[strColumn].ToString() == "")
                //                        {
                //                            summaryRow[strColumn] = "0.0";
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //    else
                //    {
                //        if (strColumn.StartsWith("Stores") == true)
                //        {
                //            summaryRow[strColumn] = 0;
                //        }

                //        if (strColumn.StartsWith("AvgWOS") == true)
                //        {
                //            summaryRow[strColumn] = 0;
                //        }
                //    }

                //}
                // end TT#586 Velocity Variables not calculated correctly

                //add the subtotal row
                _screenMatrixDataTable.Rows.Add(summaryRow);
                //BEGIN TT#299 – stodd
                if (FormLoaded)
                {
                    int lastIdx = ugMatrix.Rows.Count - 1;
                    if (lastIdx > 0)
                    {
                        UltraGridRow lastRow = ugMatrix.Rows[lastIdx];
                        lastRow.Activation = Activation.Disabled;
                    }
                }
                //END TT#299 – stodd
            }
        }
        //tt#153 (add a summary row to the matrix)

        private void MatrixTab_Load()
        {
            Matrix_Define();
            this.ugMatrix.DataSource = null;

            //begin tt#153 - Velocity Matrix Variable
            //ugMatrix_AddTotalLine(false);     //removing total line here because it is causing null reference TT#319
            //end tt#153 - Velocity Matrix Variable

            this.ugMatrix.DataSource = _screenMatrixDataTable;
            _addBandGroups = true;
            FormatScreenMatrix();
            gbxMatrixDefault.Enabled = (_screenMatrixDataTable.Rows.Count > 0);
        }

        private void FormatScreenMatrix()
        {
            string label;
            this.ugMatrix.DisplayLayout.AddNewBox.Hidden = true;
            this.ugMatrix.DisplayLayout.GroupByBox.Hidden = true;
            this.ugMatrix.DisplayLayout.GroupByBox.Prompt = "";
            this.ugMatrix.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugMatrix.DisplayLayout.Bands[0].AddButtonCaption = "";

            this.ugMatrix.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;
            //this.ugMatrix.DisplayLayout.Bands[0].Override.TipStyleRowConnector = TipStyle.Show;
            //this.ugMatrix.DisplayLayout.Bands[0].Override.TipStyleScroll = TipStyle.Show;
            //this.ugMatrix.DisplayLayout.Bands[0].Override.TipStyleCell = TipStyle.Show;
            if (!FormLoaded)
            {
                _matrixColumnHeaders = new ArrayList();

                RowColHeader rch = new RowColHeader();
                rch.Name = _lblTotalSales;
                rch.IsDisplayed = false;
                _matrixColumnHeaders.Add(rch);

                rch = new RowColHeader();
                rch.Name = _lblAvgSales;
                rch.IsDisplayed = false;
                _matrixColumnHeaders.Add(rch);

                rch = new RowColHeader();
                rch.Name = _lblPctTotalSales;
                rch.IsDisplayed = false;
                _matrixColumnHeaders.Add(rch);

                rch = new RowColHeader();
                rch.Name = _lblAvgSalesIdx;
                rch.IsDisplayed = false;
                _matrixColumnHeaders.Add(rch);

                //BEGIN TT#153 – Additional variables need to be added to the Velocity Matrix by Grade - apicchetti
                rch = new RowColHeader();
                //rch.Name = _lblTotalNumStores;
                rch.Name = _lblTotalNumStores.Replace("_", " ");
                rch.IsDisplayed = false;
                _matrixColumnHeaders.Add(rch);

                rch = new RowColHeader();
                rch.Name = _lblStockPercentOfTotal;
                rch.Name = _lblStockPercentOfTotal.Replace("_", " ");
                //rch.IsDisplayed = false;
                _matrixColumnHeaders.Add(rch);

                rch = new RowColHeader();
                rch.Name = _lblAvgStock;
                rch.IsDisplayed = false;
                _matrixColumnHeaders.Add(rch);

                rch = new RowColHeader();
                //rch.Name = _lblAllocationPercentOfTotal;
                rch.Name = _lblAllocationPercentOfTotal.Replace("_", " ");
                rch.IsDisplayed = false;
                _matrixColumnHeaders.Add(rch);
                //END TT#153 – Additional variables need to be added to the Velocity Matrix by Grade - apicchetti
            }

            this.ugMatrix.DisplayLayout.Bands[0].Columns["Grade"].Width = 40;
            this.ugMatrix.DisplayLayout.Bands[0].Columns["Grade"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Grade);

            this.ugMatrix.DisplayLayout.Bands[0].Columns["Boundary"].Width = 68;
            this.ugMatrix.DisplayLayout.Bands[0].Columns["Boundary"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Boundary);

            this.ugMatrix.DisplayLayout.Bands[0].Columns["TotalSales"].Width = 56;
            this.ugMatrix.DisplayLayout.Bands[0].Columns["TotalSales"].Tag = _lblTotalSales;
            label = _lblTotalSales;
            ParseColumnHeading(ref label);
            this.ugMatrix.DisplayLayout.Bands[0].Columns["TotalSales"].Header.Caption = label;

            this.ugMatrix.DisplayLayout.Bands[0].Columns["AvgSales"].Width = 56;
            this.ugMatrix.DisplayLayout.Bands[0].Columns["AvgSales"].Tag = _lblAvgSales;
            label = _lblAvgSales;
            ParseColumnHeading(ref label);
            this.ugMatrix.DisplayLayout.Bands[0].Columns["AvgSales"].Header.Caption = label;

            this.ugMatrix.DisplayLayout.Bands[0].Columns["PctTotalSales"].Width = 40;
            this.ugMatrix.DisplayLayout.Bands[0].Columns["PctTotalSales"].Tag = _lblPctTotalSales;
            label = _lblPctTotalSales;
            ParseColumnHeading(ref label);
            this.ugMatrix.DisplayLayout.Bands[0].Columns["PctTotalSales"].Header.Caption = label;

            this.ugMatrix.DisplayLayout.Bands[0].Columns["AvgSalesIdx"].Width = 40;
            this.ugMatrix.DisplayLayout.Bands[0].Columns["AvgSalesIdx"].Tag = _lblAvgSalesIdx;
            label = _lblAvgSalesIdx;
            ParseColumnHeading(ref label);
            this.ugMatrix.DisplayLayout.Bands[0].Columns["AvgSalesIdx"].Header.Caption = label;

            //BEGIN TT#153 – Additional variables need to be added to the Velocity Matrix by Grade - apicchetti
            this.ugMatrix.DisplayLayout.Bands[0].Columns["TotalNumStores"].Width = 60;
            this.ugMatrix.DisplayLayout.Bands[0].Columns["TotalNumStores"].Tag = _lblTotalNumStores.Replace("_", " ");
            label = _lblTotalNumStores;
            ParseColumnHeading(ref label, true);
            this.ugMatrix.DisplayLayout.Bands[0].Columns["TotalNumStores"].Header.Caption = label;

            this.ugMatrix.DisplayLayout.Bands[0].Columns["StockPercentOfTotal"].Width = 60;
            this.ugMatrix.DisplayLayout.Bands[0].Columns["StockPercentOfTotal"].Tag = _lblStockPercentOfTotal.Replace("_", " ");
            label = _lblStockPercentOfTotal;
            ParseColumnHeading(ref label, true);
            this.ugMatrix.DisplayLayout.Bands[0].Columns["StockPercentOfTotal"].Header.Caption = label;

            this.ugMatrix.DisplayLayout.Bands[0].Columns["AvgStock"].Width = 60;
            this.ugMatrix.DisplayLayout.Bands[0].Columns["AvgStock"].Tag = _lblAvgStock;
            label = _lblAvgStock;
            ParseColumnHeading(ref label);
            this.ugMatrix.DisplayLayout.Bands[0].Columns["AvgStock"].Header.Caption = label;

            this.ugMatrix.DisplayLayout.Bands[0].Columns["AllocationPercentOfTotal"].Width = 75;
            this.ugMatrix.DisplayLayout.Bands[0].Columns["AllocationPercentOfTotal"].Tag = _lblAllocationPercentOfTotal.Replace("_", " ");
            label = _lblAllocationPercentOfTotal;
            ParseColumnHeading(ref label, true);
            this.ugMatrix.DisplayLayout.Bands[0].Columns["AllocationPercentOfTotal"].Header.Caption = label;
            //END TT#153 – Additional variables need to be added to the Velocity Matrix by Grade - apicchetti

            //this.ugMatrix.DisplayLayout.AutoFitColumns = true;
            if (!FormLoaded)
            {
                //Add a list to the grid, and name it "Rule".
                this.ugMatrix.DisplayLayout.ValueLists.Add("Rule");

                foreach (DataRow row in _defOnHandRules.Rows)
                {
                    Infragistics.Win.ValueListItem vli = new Infragistics.Win.ValueListItem();
                    vli.DataValue = row["TEXT_CODE"];
                    vli.DisplayText = row["TEXT_VALUE"].ToString();
                    this.ugMatrix.DisplayLayout.ValueLists["Rule"].ValueListItems.Add(vli);
                    vli.Dispose();
                }
                BuildMatrixContextmenu();
                this.ugMatrix.ContextMenu = mnuMatrixGrid;
            }
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //FormatColumns(this.ugMatrix);
            //End TT#169

            for (int i = 0; i < this.ugMatrix.DisplayLayout.Bands[0].Columns.Count; i++)
            {
                this.ugMatrix.DisplayLayout.Bands[0].Columns[i].SortIndicator = SortIndicator.Disabled;

                if (i >= 10)
                {
                    if (SqlInt32.Mod(i, 4) == 2)
                    {
                        this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Stores);
                        this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Tag = "Stores";
                        this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Width = 45;
                        // BEGIN MID Track #3792 - replace obsolete property 
                        //this.ugMatrix.DisplayLayout.Bands[0].Columns[i].FieldLen = 6;
                        this.ugMatrix.DisplayLayout.Bands[0].Columns[i].MaxLength = 6;
                        // END MID Track #3792
                        if (!cbxInteractive.Checked)
                            this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Hidden = true;
                    }
                    else if (SqlInt32.Mod(i, 4) == 3)
                    {
                        label = MIDText.GetTextOnly(eMIDTextCode.lbl_AvgWOS);
                        ParseColumnHeading(ref label);
                        this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Header.Caption = label;
                        this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Tag = "AvgWOS";
                        this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Width = 40;
                        // BEGIN MID Track #3792 - replace obsolete property 
                        //this.ugMatrix.DisplayLayout.Bands[0].Columns[i].FieldLen = 6;
                        this.ugMatrix.DisplayLayout.Bands[0].Columns[i].MaxLength = 6;
                        // END MID Track #3792
                        if (!cbxInteractive.Checked)
                            this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Hidden = true;
                    }
                    else if (SqlInt32.Mod(i, 4) == 0)
                    {
                        this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Rule);
                        this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        this.ugMatrix.DisplayLayout.Bands[0].Columns[i].ValueList = ugMatrix.DisplayLayout.ValueLists["Rule"];
                        this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Tag = "Rule";
                        this.ugMatrix.DisplayLayout.Bands[0].Columns[i].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
                    }
                    else
                    {
                        this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Qty);
                        this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Tag = "Qty";
                        this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Width = 55;
                        // BEGIN MID Track #3792 - replace obsolete property 
                        //this.ugMatrix.DisplayLayout.Bands[0].Columns[i].FieldLen = 6;
                        this.ugMatrix.DisplayLayout.Bands[0].Columns[i].MaxLength = 6;
                        // END MID Track #3792
                    }
                }
            }
            // for some reason, the column setting for CellActivation isn't working
            // so use the Row setting
            bool activateQty;
            foreach (UltraGridRow row in ugMatrix.Rows)
            {
                row.Cells["Grade"].Activation = Activation.Disabled;
                row.Cells["Boundary"].Activation = Activation.Disabled;
                row.Cells["TotalSales"].Activation = Activation.Disabled;
                row.Cells["AvgSales"].Activation = Activation.Disabled;
                row.Cells["PctTotalSales"].Activation = Activation.Disabled;
                row.Cells["AvgSalesIdx"].Activation = Activation.Disabled;

                //Begin TT#153 – add variables to velocity matrix - apicchetti
                row.Cells["TotalNumStores"].Activation = Activation.Disabled;
                row.Cells["StockPercentOfTotal"].Activation = Activation.Disabled;
                row.Cells["AvgStock"].Activation = Activation.Disabled;
                row.Cells["AllocationPercentOfTotal"].Activation = Activation.Disabled;
                //END TT#153 – add variables to velocity matrix - apicchetti

                for (int j = 10; j < row.Band.Columns.Count; j++)
                {
                    activateQty = false;
                    if (row.Cells[j].Column.Tag.ToString() == "Rule")
                    {
                        if (row.Cells[j].Value != System.DBNull.Value)
                        {
                            eVelocityRuleRequiresQuantity vrq = (eVelocityRuleRequiresQuantity)(Convert.ToInt32(row.Cells[j].Value, CultureInfo.CurrentUICulture));
                            if (Enum.IsDefined(typeof(eVelocityRuleRequiresQuantity), vrq))
                            {
                                activateQty = true;
                            }
                            else
                            {
                                activateQty = false;
                            }
                        }
                        j++; // Enable/disable Qty column
                        if (activateQty)
                            row.Cells[j].Activation = Activation.AllowEdit;
                        else
                            row.Cells[j].Activation = Activation.Disabled;
                    }
                    else if (row.Cells[j].Column.Tag.ToString() == "Stores"
                        || row.Cells[j].Column.Tag.ToString() == "AvgWOS")
                    {
                        row.Cells[j].Activation = Activation.Disabled;
                    }
                }
            }
            // sort in descending order by boundary
            //this.ugMatrix.DisplayLayout.Bands[0].SortedColumns.Clear();
            //this.ugMatrix.DisplayLayout.Bands[0].SortedColumns.Add("Boundary", true);

            if (_addBandGroups)
            {
                UltraGridBand band = this.ugMatrix.DisplayLayout.Bands[0];
                band.LevelCount = 1;
                band.Override.AllowGroupMoving = AllowGroupMoving.NotAllowed;

                UltraGridGroup group0 = new UltraGridGroup();
                group0 = band.Groups.Add("Group1", "   ");
                group0.Columns.Add(band.Columns["Grade"], 0, 0);
                group0.Columns.Add(band.Columns["Boundary"], 1, 0);
                group0.Columns.Add(band.Columns["TotalSales"], 2, 0);
                group0.Columns.Add(band.Columns["AvgSales"], 3, 0);
                group0.Columns.Add(band.Columns["PctTotalSales"], 4, 0);
                group0.Columns.Add(band.Columns["AvgSalesIdx"], 5, 0);

                //BEGIN TT#153 – add variables to velocity matrix - apicchetti
                group0.Columns.Add(band.Columns["TotalNumStores"], 6, 0);
                group0.Columns.Add(band.Columns["StockPercentOfTotal"], 7, 0);
                group0.Columns.Add(band.Columns["AvgStock"], 8, 0);
                group0.Columns.Add(band.Columns["AllocationPercentOfTotal"], 9, 0);
                //END TT#153 – add variables to velocity matrix - apicchetti

                for (int i = 10; i < band.Columns.Count; i += 4)
                {
                    UltraGridGroup group = new UltraGridGroup();
                    group = band.Groups.Add("Group" + i.ToString(), _screenMatrixDataTable.Columns[i].Caption);
                    group.Columns.Add(band.Columns[i], 0, 0);
                    group.Columns.Add(band.Columns[i + 1], 1, 0);
                    group.Columns.Add(band.Columns[i + 2], 2, 0);
                    group.Columns.Add(band.Columns[i + 3], 3, 0);
                    //group.Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
                    group.CellAppearance.BorderColor = System.Drawing.Color.Black;

                }
            }

            if (!FunctionSecurity.AllowUpdate)
            {
                foreach (UltraGridBand ugb in this.ugMatrix.DisplayLayout.Bands)
                {
                    ugb.Override.AllowDelete = DefaultableBoolean.False;
                }

                this.ugMatrix.ContextMenu = null;
            }
            //BEGIN TT#299 – stodd
            int lastIdx = ugMatrix.Rows.Count - 1;

            //TT#319 and 320 - added these checks because of null reference issues
            if (lastIdx >= 0)
            {
                if ((string)ugMatrix.Rows[lastIdx].Cells[0].Value == "Total:")
                {
                    UltraGridRow lastRow = ugMatrix.Rows[lastIdx];
                    lastRow.Activation = Activation.Disabled;
                }
            }
            //END TT#299 – stodd

            ShowHideColHeaders(_matrixColumnHeaders);
            _matrixIsPopulated = true;
        }

        private void ParseColumnHeading(ref string aColHeading)
        {
            string newstring = string.Empty;
            newstring = aColHeading.Replace(" ", Environment.NewLine);
            aColHeading = newstring;
        }

        //begin tt#260 - new Column headings in velocity being cut off
        private void ParseColumnHeading(ref string aColHeading, bool UnderscoreToSpace)
        {
            string newstring = string.Empty;
            newstring = aColHeading.Replace(" ", Environment.NewLine);
            if (UnderscoreToSpace == true)
            {
                newstring = newstring.Replace("_", " ");
            }
            aColHeading = newstring;
        }
        //end tt#260 - new Column headings in velocity being cut off

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        private void UnParseColumnHeading(ref string aColHeading)
        {
            string newstring = string.Empty;
            newstring = aColHeading.Replace(Environment.NewLine, " ");
            aColHeading = newstring;
        }
        // End TT#231 

        //ArrayList alSumCols = new ArrayList();  // TT#586 Velocity Variables not calculated correctly
        private void Matrix_Define()
        {
            //alSumCols.Clear();  // TT#586 Velocity Variables not calculated correctly

            string grade;
            int setValue, index = 0, prevIndex = 0, boundary;

            _screenMatrixDataTable = MIDEnvironment.CreateDataTable("ScreenMatrix");

            DataColumn dataColumn;

            //Create Columns and rows for datatable
            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.String");
            dataColumn.ColumnName = "Grade";
            dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Grade);
            dataColumn.ReadOnly = false;
            dataColumn.Unique = true;
            _screenMatrixDataTable.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.Int32");
            dataColumn.ColumnName = "Boundary";
            dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Boundary);
            dataColumn.ReadOnly = false;
            dataColumn.Unique = true;
            _screenMatrixDataTable.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.Int32");
            dataColumn.ColumnName = "TotalSales";
            // begin TT#586 velocity variables not calculated correctly
            ////tt#153 - Velocity Matrix Variables - apicchetti
            //alSumCols.Add(dataColumn.ColumnName);
            ////tt#153 (add the column to the array for summation)
            // end TT#586 velocity variables not calculatd correctly
            dataColumn.Caption = _lblTotalSales;
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            _screenMatrixDataTable.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.Double");
            dataColumn.ColumnName = "AvgSales";
            // begin TT#586 velocity variables not calculated correctly
            ////tt#153 - Velocity Matrix Variables - apicchetti
            //alSumCols.Add(dataColumn.ColumnName);
            ////tt#153 (add the column to the array for summation)
            // end TT#586 velocity variables not calculated correctly
            dataColumn.Caption = _lblAvgSales;
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            _screenMatrixDataTable.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.Double");
            dataColumn.ColumnName = "PctTotalSales";
            // begin TT#586 Velocity variables not calculated correctly
            ////tt#153 - Velocity Matrix Variables - apicchetti
            //alSumCols.Add(dataColumn.ColumnName);
            ////tt#153 (add the column to the array for summation)
            // end TT#586 Velocity variables not calculated correctly
            dataColumn.Caption = _lblPctTotalSales;
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            _screenMatrixDataTable.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.Double");
            dataColumn.ColumnName = "AvgSalesIdx";
            // begin TT#586 Velocity variables not calculated correctly
            ////tt#153 - Velocity Matrix Variables - apicchetti
            //alSumCols.Add(dataColumn.ColumnName);
            ////tt#153 (add the column to the array for summation)
            // end TT#586 Velocity variables not calculated correctly
            dataColumn.Caption = _lblAvgSalesIdx;
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            _screenMatrixDataTable.Columns.Add(dataColumn);

            //BEGIN TT#153 – add variables to velocity matrix - apicchetti
            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.Int32");
            dataColumn.ColumnName = "TotalNumStores";
            // begin TT#586 Velocity variables not calculated correctly
            ////tt#153 - Velocity Matrix Variables - apicchetti
            //alSumCols.Add(dataColumn.ColumnName);
            ////tt#153 (add the column to the array for summation)
            // end TT#586 velocity variables not calculated correctly

            dataColumn.Caption = _lblTotalNumStores;
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            _screenMatrixDataTable.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.Double");
            dataColumn.ColumnName = "StockPercentOfTotal";
            // begin TT#586 Velocity variables not calculated correctly
            ////tt#153 - Velocity Matrix Variables - apicchetti
            //alSumCols.Add(dataColumn.ColumnName);
            ////tt#153 (add the column to the array for summation)
            // end TT#586 velocity variables not calculated correctly
            dataColumn.Caption = _lblStockPercentOfTotal;
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            _screenMatrixDataTable.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.Double");
            dataColumn.ColumnName = "AvgStock";
            // begin TT#586 Velocity variables not calculated correctly
            ////tt#153 - Velocity Matrix Variables - apicchetti
            //alSumCols.Add(dataColumn.ColumnName);
            ////tt#153 (add the column to the array for summation)
            // end TT#586 velocity variables not calculated correctly
            dataColumn.Caption = _lblAvgStock;
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            _screenMatrixDataTable.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.Double");
            dataColumn.ColumnName = "AllocationPercentOfTotal";
            // begin TT#586 Velocity variables not calculated correctly
            ////tt#153 - Velocity Matrix Variables - apicchetti
            //alSumCols.Add(dataColumn.ColumnName);
            ////tt#153 (add the column to the array for summation)
            // end TT#586 velocity variables not calculated correctly
            dataColumn.Caption = _lblAllocationPercentOfTotal;
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            _screenMatrixDataTable.Columns.Add(dataColumn);
            //BEGIN TT#153 – add variables to velocity matrix - apicchetti

            string colCaption = string.Empty;

            _sellThruPctsDataTable = _dsVelocity.Tables["SellThru"];
            _sellThruPctsDataTable.DefaultView.Sort = "SellThruIndex DESC";

            for (int i = 0; i < _sellThruPctsDataTable.DefaultView.Count; i++)
            {
                DataRowView dr = _sellThruPctsDataTable.DefaultView[i];
                index = (int)dr["SellThruIndex"];
                if (i == 0)
                {
                    colCaption = ">" + Convert.ToString(index, CultureInfo.CurrentUICulture);
                }
                else if (i == _sellThruPctsDataTable.DefaultView.Count - 1)
                {
                    colCaption = Convert.ToString(index, CultureInfo.CurrentUICulture) + "-"
                        + Convert.ToString(prevIndex, CultureInfo.CurrentUICulture);
                }
                else
                {
                    colCaption = Convert.ToString(index + 1, CultureInfo.CurrentUICulture) + "-"
                        + Convert.ToString(prevIndex, CultureInfo.CurrentUICulture);
                }

                prevIndex = index;

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Stores" + index.ToString();
                // begin TT#586 Velocity variables not calculated correctly
                ////tt#153 - Velocity Matrix Variables - apicchetti
                //alSumCols.Add(dataColumn.ColumnName);
                ////tt#153 (add the column to the array for summation)
                // end TT#586 velocity variables not calculated correctly

                dataColumn.Caption = colCaption;
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                _screenMatrixDataTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Double");
                dataColumn.ColumnName = "AvgWOS" + index.ToString();
                // begin TT#586 Velocity variables not calculated correctly
                ////tt#153 - Velocity Matrix Variables - apicchetti
                //alSumCols.Add(dataColumn.ColumnName);
                ////tt#153 (add the column to the array for summation)
                // end TT#586 velocity variables not calculated correctly

                dataColumn.Caption = colCaption;
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                _screenMatrixDataTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Rule" + index.ToString();
                dataColumn.Caption = colCaption;
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                _screenMatrixDataTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Double");
                dataColumn.ColumnName = "Qty" + index.ToString();

                dataColumn.Caption = colCaption;
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                _screenMatrixDataTable.Columns.Add(dataColumn);
            }

            setValue = Convert.ToInt32(cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
            foreach (DataRow row in _velocityGradesDataTable.Rows)
            {
                grade = Convert.ToString(row["Grade"], CultureInfo.CurrentUICulture);
                boundary = Convert.ToInt32(row["Boundary"], CultureInfo.CurrentUICulture);
                _screenMatrixDataTable.Rows.Add(new object[] { grade, boundary });
            }
            LoadDBTableToScreenTable();
            // BEGIN TT#1287 - AGallagher - Inventory Min/Max
            if (_InventoryInd == 'I')
            {
                LoadMerchandiseCombo();
                LoadGenAllocValues();
            }
            // END TT#1287 - AGallagher - Inventory Min/Max

            
        }
        private void LoadDBTableToScreenTable()
        {
            try
            {
                int setValue = Convert.ToInt32(cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
                int idx = 0;
                if (_dbMatrixDataTable.Rows.Count > 0)
                {
                    _dbMatrixDataTable.DefaultView.RowFilter = "SglRID = " + setValue.ToString();
                    _dbMatrixDataTable.DefaultView.Sort = "Boundary DESC, SellThruIndex DESC";
                    if (_dbMatrixDataTable.DefaultView.Count > 0)
                    {
                        for (int i = 0; i < _screenMatrixDataTable.Rows.Count; i++)
                        {
                            DataRow row = _screenMatrixDataTable.Rows[i];
                            int boundary = Convert.ToInt32(row["Boundary"], CultureInfo.CurrentUICulture);
                            object[] keys = new object[2];
                            keys[0] = boundary.ToString();
                            for (int j = 6; j < _screenMatrixDataTable.Columns.Count; j++)
                            {
                                int foundRowIdx = -1;
                                DataColumn col = _screenMatrixDataTable.Columns[j];

                                if (col.ColumnName.Substring(0, 3) == "Qty")
                                    idx = Convert.ToInt32(col.ColumnName.Substring(3), CultureInfo.CurrentUICulture);
                                else if (col.ColumnName.Substring(0, 4) == "Rule")
                                    idx = Convert.ToInt32(col.ColumnName.Substring(4), CultureInfo.CurrentUICulture);
                                else if (col.ColumnName.Substring(0, 6) == "Stores"
                                    || col.ColumnName.Substring(0, 6) == "AvgWOS")
                                    idx = Convert.ToInt32(col.ColumnName.Substring(6), CultureInfo.CurrentUICulture);

                                keys[1] = idx.ToString();
                                foundRowIdx = _dbMatrixDataTable.DefaultView.Find(keys);
                                if (foundRowIdx >= 0)
                                {
                                    DataRowView dvRow = _dbMatrixDataTable.DefaultView[foundRowIdx];
                                    if (col.ColumnName.Substring(0, 3) == "Qty")
                                    {
                                        if (dvRow["VelocityQty"] == System.DBNull.Value
                                            || Convert.ToDouble(dvRow["VelocityQty"], CultureInfo.CurrentUICulture) == Include.UndefinedDouble)
                                            row[col] = System.DBNull.Value;
                                        else
                                            row[col] = dvRow["VelocityQty"];
                                    }
                                    else if (col.ColumnName.Substring(0, 4) == "Rule")
                                    {
                                        row[col] = dvRow["VelocityRule"];
                                    }
                                    else if (col.ColumnName.Substring(0, 6) == "Stores")
                                    {
                                        if (dvRow["Stores"] == System.DBNull.Value
                                            || Convert.ToInt32(dvRow["Stores"], CultureInfo.CurrentUICulture) == 0)
                                            row[col] = System.DBNull.Value;
                                        else
                                            row[col] = dvRow["Stores"];
                                    }
                                    else if (col.ColumnName.Substring(0, 6) == "AvgWOS")
                                    {
                                        if (dvRow["AvgWOS"] == System.DBNull.Value
                                            || Convert.ToInt32(dvRow["AvgWOS"], CultureInfo.CurrentUICulture) == 0)
                                            row[col] = System.DBNull.Value;
                                        else
                                            row[col] = dvRow["AvgWOS"];
                                    }
                                }
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
        private void BuildMatrixContextmenu()
        {
            MenuItem mnuItemChooseCol = new MenuItem("Choose Column...");
            mnuMatrixGrid.MenuItems.Add(mnuItemChooseCol);
            mnuItemChooseCol.Click += new System.EventHandler(this.mnuMatrixChooseColumn_Click);
        }

        private void mnuMatrixChooseColumn_Click(object sender, System.EventArgs e)
        {
            bool needsAtLeastOneCol = false;
            try
            {
                // Begin Track #4868 - JSmith - Variable Groupings
                //RowColChooser frm = new RowColChooser(_matrixColumnHeaders, needsAtLeastOneCol, "Column Chooser");
                RowColChooser frm = new RowColChooser(_matrixColumnHeaders, needsAtLeastOneCol, "Column Chooser", null);
                // End Track #4868
                if (frm.ShowDialog() == DialogResult.OK)
                    ShowHideColHeaders(frm.Headers);

                frm.Dispose();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ShowHideColHeaders(ArrayList aColHeaders)
        {
            string colName;
            int hdrLines;
            try
            {
                if (cbxInteractive.Checked)
                    hdrLines = 2;
                else
                    hdrLines = 1;

                foreach (RowColHeader rch in aColHeaders)
                {
                    colName = rch.Name;

                    //for (int i = 2; i <= 9; i++)                      // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
                    for (int i = 2; i <= (aColHeaders.Count + 1); i++)  // End TT#231
                    {
                        Debug.Print(this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Tag.ToString() +
                            " : " + colName);
                        if (this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Tag.ToString() == colName)
                        {
                            this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Hidden = !rch.IsDisplayed;

                            if (rch.Name == _lblTotalSales || rch.Name == _lblAvgSales || rch.Name == _lblTotalNumStores.Replace("_", " ") ||
                                rch.Name == _lblStockPercentOfTotal.Replace("_", " ") || rch.Name == _lblAllocationPercentOfTotal.Replace("_", " ")
                                || rch.Name == _lblAvgStock) //TT#260 - Added control for matrix header height - apicchetti
                            {
                                if (rch.IsDisplayed && hdrLines < 2)
                                    hdrLines = 2;
                            }
                            else if (rch.Name == _lblPctTotalSales || rch.Name == _lblAvgSalesIdx)
                            {
                                if (rch.IsDisplayed && hdrLines < 3)
                                    hdrLines = 3;
                            }
                            break;
                        }
                    }
                }
                this.ugMatrix.DisplayLayout.Bands[0].ColHeaderLines = hdrLines;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        private void ApplyViewToGridLayout(int aViewRID)
        {
            try
            {
                int visiblePosition, sortSequence, width;
                string bandKey, colKey, errMessage;
                bool isHidden, isGroupByCol;
                eSortDirection sortDirection;

                _lastSelectedViewRID = aViewRID;

                if (aViewRID == 0 || aViewRID == Include.NoRID)    // don't modify current grid appearance 
                {
                    return;
                }

                DataTable dtGridViewDetail = GridViewData.GridViewDetail_Read(aViewRID);

                if (dtGridViewDetail == null || dtGridViewDetail.Rows.Count == 0)
                {
                    errMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_as_GridViewDoesNotExist);
                    MessageBox.Show(errMessage);
                    _lastSelectedViewRID = Include.NoRID;
                    BindMatrixViewCombo();
                    return;
                }

                SortedList sortedColumns = new SortedList();

                ugMatrix.ResetLayouts();
                ApplyAppearance(ugMatrix);
                ugMatrix.DisplayLayout.ClearGroupByColumns();
                foreach (UltraGridBand band in ugMatrix.DisplayLayout.Bands)
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

                    if (ugMatrix.DisplayLayout.Bands.Exists(bandKey))
                    {
                        UltraGridBand band = ugMatrix.DisplayLayout.Bands[bandKey];

                        if (band.Columns.Exists(colKey))
                        {
                            UltraGridColumn column = band.Columns[colKey];
                            column.Header.VisiblePosition = visiblePosition;
                            column.Hidden = isHidden;
                            if (width != -1)
                            {
                                column.Width = width;
                            }

                            string label = column.Header.Caption;
                            UnParseColumnHeading(ref label);

                            foreach (RowColHeader rch in _matrixColumnHeaders)
                            {
                                if (rch.Name == label)
                                {
                                    rch.IsDisplayed = !column.Hidden;
                                    break;
                                }
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

                        if (ugMatrix.DisplayLayout.Bands.Exists(bandKey))
                        {
                            UltraGridBand band = ugMatrix.DisplayLayout.Bands[bandKey];

                            if (!band.SortedColumns.Exists(colKey))
                            {
                                //_cancelSelectEvent = true;   
                                band.SortedColumns.Add(colKey, sortDescending, isGroupByCol);
                            }
                        }
                    }
                }
                ShowHideColHeaders(_matrixColumnHeaders);
            }
            catch
            {
                throw;
            }
        }
        // End TT#231  
        #endregion Matrix Tab

        #region Attribute and Attribute Set Combo Boxes
        private void StoreAttributes_Populate()
        {
            try
            {
                // Begin Track #4872 - JSmith - Global/User Attributes
                //ProfileList profileList = SAB.StoreServerSession.GetStoreGroupListViewList();
                BuildAttributeList();

                //this.cboStoreAttribute.ValueMember = "Key";
                //this.cboStoreAttribute.DisplayMember = "Name";
                //this.cboStoreAttribute.DataSource = profileList.ArrayList;
                // End Track #4872

                //StoreGroupListViewProfile storeGroup = (StoreGroupListViewProfile)profileList[0];
                //this.cboStoreAttribute.SelectedValue = storeGroup.Key;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                FormLoadError = true;
            }
        }

        private void cboStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            int idx;
            // Begin TT#3626 - JSmith - Velocity - Change the Attribute after opening the method and receive a Null Reference
            bool dequeuedHeaders = false;
            // End TT#3626 - JSmith - Velocity - Change the Attribute after opening the method and receive a Null Reference
            try
            {
                if (FormLoaded)
                {
                    if (_attributeReset)
                    {
                        _attributeReset = false;
                        return;
                    }
                    idx = this.cboStoreAttribute.SelectedIndex;
                    if (!MatrixWarningOK(this.lblAttribute.Text))
                    {
                        _attributeReset = true;
                        cboStoreAttribute.SelectedValue = _prevAttributeValue;
                        return;
                    }
                    else
                    {
                        this.cboStoreAttribute.SelectedIndex = idx;
                    }
                    _groupLevelDataTable.Clear();
                    _attributeChanged = true;
                    _matrixEverProcessed = false;
                    // Begin TT#1759-MD - JSmith - GA- Velocity-> Velocity Store Detail-> Allocate and Velocity Matrix buttons gray out when selecting attribute in Matrix
                    if (AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID != Include.NoRID)	// Reuse transaction if assortment or group
                    {
                        if (_trans != null)
                        {
                            _trans.ReInitializeForAssortGroup();
                        }
                    }
                    else
                    {
                    // End TT#1759-MD - JSmith - GA- Velocity-> Velocity Store Detail-> Allocate and Velocity Matrix buttons gray out when selecting attribute in Matrix
                        // Begin TT#3619 - JSmith - In Velocity-> active matrix-> change the attribute-> enque error occurs
                        // release locks before loosing transaction
                        // Begin TT#3626 - JSmith - Velocity - Change the Attribute after opening the method and receive a Null Reference
                        if (_trans != null)
                        {
                            // End TT#3626 - JSmith - Velocity - Change the Attribute after opening the method and receive a Null Reference
                            dequeuedHeaders = true;
                            _trans.DequeueHeaders();
                            _trans.Dispose();
                        }
                        // End TT#3619 - JSmith - In Velocity-> active matrix-> change the attribute-> enque error occurs
                        _trans = null;   // TT#694 - AGallagher - Velocity - received an Unhandled Exception when chanign Attribute Sets
                    // Begin TT#1759-MD - JSmith - GA- Velocity-> Velocity Store Detail-> Allocate and Velocity Matrix buttons gray out when selecting attribute in Matrix
                    }
                    // End TT#1759-MD - JSmith - GA- Velocity-> Velocity Store Detail-> Allocate and Velocity Matrix buttons gray out when selecting attribute in Matrix
                    ResetMatrix();
                }
                if (this.cboStoreAttribute.SelectedValue != null)
                {
                    _prevAttributeValue = Convert.ToInt32(this.cboStoreAttribute.SelectedValue, CultureInfo.CurrentUICulture);
                    PopulateStoreAttributeSet(this.cboStoreAttribute.SelectedValue.ToString()); //moved for bug found when working on tt#290 - stodd
                }

                // Begin TT#3619 - JSmith - In Velocity-> active matrix-> change the attribute-> enque error occurs
                // re-establish locks in new transaction
                if (cbxInteractive.Checked &&
                    dequeuedHeaders)
                {
                    _trans = SAB.ApplicationServerSession.CreateTransaction();
                    string enqMessage = string.Empty;
                    if (!_trans.EnqueueSelectedHeaders(out enqMessage))
                    {
                        // Begin TT#4515 - stodd - enqueue message
                        //enqMessage =
                        //    MIDText.GetTextOnly(eMIDTextCode.msg_al_HeaderEnqFailed)
                        //    + System.Environment.NewLine
                        //    + enqMessage;
                        // End TT#4515 - stodd - enqueue message
                        SAB.MessageCallback.HandleMessage
                            (
                                enqMessage,
                                "Header Lock Conflict",
                                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk
                            );
                    }
                }
                // End TT#3619 - JSmith - In Velocity-> active matrix-> change the attribute-> enque error occurs
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void cboStoreAttribute_DragDrop(object sender, DragEventArgs e)
        {
            //Begin Track #5858 - Kjohnson - Validating store security only
            try
            {
                bool isSuccessfull = ((MIDComboBoxTag)(((ComboBox)sender).Tag)).ComboBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
            //End Track #5858
        }
        /// <summary>
        /// Populate all values of the Store_Group_Levels (Attribute Sets)
        /// based on the key parameter.
        /// </summary>
        /// <param name="key">SGL_RID</param>
        private void PopulateStoreAttributeSet(string key)
        {
            try
            {
                ProfileList pl = StoreMgmt.StoreGroup_GetLevelListViewList(Convert.ToInt32(key, CultureInfo.CurrentUICulture)); //SAB.StoreServerSession.GetStoreGroupLevelListViewList(Convert.ToInt32(key, CultureInfo.CurrentUICulture));
                pl.ArrayList.Add(_totalMatrixSet);

                this.cbxAttributeSet.ValueMember = "Key";
                this.cbxAttributeSet.DisplayMember = "Name";
                this.cbxAttributeSet.DataSource = pl.ArrayList;

                if (this.cbxAttributeSet.Items.Count > 0)
                {
                    this.cbxAttributeSet.SelectedIndex = 0;
                    _prevSetValue = Convert.ToInt32(cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cbxAttributeSet_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            if (FormLoaded)
            {
                // BEGIN TT#5792 - AGallagher - Velocity Method with Total Matrix and Average Mode not giving expected results
                _prevSetValueTest = Convert.ToInt32(cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
                gbxMatrixMode.Enabled = true;
                if (_prevSetValueTest == 0)
                {
                    //DisableAverageRuleUI();
                    cboMatrixModeAvgRule.Enabled = false;
                    txtMatrixModeAvgRule.Enabled = false;
                    gbxSpreadOption.Enabled = false;

                    //rdoSpreadOptionSmooth.Checked = true;
                    txtMatrixModeAvgRule.Text = string.Empty;
                    cboMatrixModeAvgRule.Text = string.Empty;
                    cboMatrixModeAvgRule.Text = null;
                    txtMatrixModeAvgRule.Text = null;
                    gbxMatrixMode.Enabled = false;
                    //rdoMatrixModeNormal.Checked = true;
                }
                // END TT#5792 - AGallagher - Velocity Method with Total Matrix and Average Mode not giving expected results
                _attributeSetChanged = true;
                if (_setReset)
                {
                    _setReset = false;
                    return;
                }
                if (!ValidTabMatrix())
                {
                    string text = MIDText.GetTextOnly(eMIDTextCode.msg_ErrorsFoundReviewCorrect);
                    MessageBox.Show(text);
                    _setReset = true;
                    cbxAttributeSet.SelectedValue = _prevSetValue;
                }
                else
                {
                    // begin TT#587 Matrix Totals WRong
                    //if (_attributeChanged)
                    //	_attributeChanged = false; 
                    //else
                    //SetAttributeSetValues(_prevSetValue);
                    if (!_attributeChanged)
                    {
                        SetAttributeSetValues(_prevSetValue);
                    }
                    // end TT#587 Matrix Totals Wrong
                    _attrSetChanged = true;
                    LoadAttributeSetValues((int)cbxAttributeSet.SelectedValue);
                    _prevSetValue = Convert.ToInt32(cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
                    _attrSetChanged = false;
                }

                _afterProcess = false; // resetting after process to avoid null reference in adding total line TT#319
                // begin TT#587 Matrix Totals Wrong
                //ugMatrix_AddTotalLine(false); 
                if (_attributeChanged)
                {
                    _attributeChanged = false;
                }
                else
                {
                    ugMatrix_AddTotalLine(false);
                }
                // end TT#587 Matrix Totals Wrong
                _attributeSetChanged = false;
            }
        }
        private void SetAttributeSetValues(int aSetValue)
        {
            DataRow setRow = null;
            foreach (DataRow row in _groupLevelDataTable.Rows)
            {
                if ((int)row["SglRID"] == aSetValue)
                {
                    setRow = row;
                    break;
                }
            }
            if (setRow == null)
            {
                setRow = _groupLevelDataTable.NewRow();
                _groupLevelDataTable.Rows.Add(setRow);
            }

            setRow["SglRID"] = aSetValue;
            setRow["NoOnHandRule"] = Convert.ToInt32(cboNoOHRule.SelectedValue, CultureInfo.CurrentUICulture);
            if (txtNoOHQuantity.Text.Trim().Length > 0)
                setRow["NoOnHandQty"] = Convert.ToDouble(txtNoOHQuantity.Text, CultureInfo.CurrentUICulture);
            else
                setRow["NoOnHandQty"] = System.DBNull.Value;
            // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) 
            //Begin TT#855-MD -jsobek -Velocity Enhancements
            //if (rdoMatrixModeNormal.Checked == true)
            //    setRow["ModeInd"] = 'N';
            //else
            //    setRow["ModeInd"] = 'A';
            
            if (rdoMatrixModeNormal.Checked == true)
            {
                setRow["ModeInd"] = 'N';
            }
            else
            {
                setRow["ModeInd"] = 'A';
            }
            //End TT#855-MD -jsobek -Velocity Enhancements

            if (cboMatrixModeAvgRule.Text.Trim().Length > 0)
                setRow["AverageRule"] = Convert.ToInt32(cboMatrixModeAvgRule.SelectedValue, CultureInfo.CurrentUICulture);
            else
                setRow["AverageRule"] = System.DBNull.Value;
            if (txtMatrixModeAvgRule.Text.Trim().Length > 0)
                setRow["AverageQty"] = Convert.ToDouble(txtMatrixModeAvgRule.Text, CultureInfo.CurrentUICulture);
            else
                setRow["AverageQty"] = System.DBNull.Value;
            if (rdoSpreadOptionSmooth.Checked == true)
                setRow["SpreadInd"] = 'S';
            else
                setRow["SpreadInd"] = 'I';
            // END TT#637 - AGallagher - Velocity - Spread Average (#7) 

            _groupLevelDataTable.AcceptChanges();

            _screenMatrixDataTable.AcceptChanges();

            _dbMatrixDataTable.DefaultView.RowFilter = "SglRID = " + aSetValue.ToString();
            for (int i = _dbMatrixDataTable.DefaultView.Count - 1; i >= 0; i--)
            {
                _dbMatrixDataTable.DefaultView.Delete(i);
            }
            _dbMatrixDataTable.AcceptChanges();

            foreach (DataRow row in _screenMatrixDataTable.Rows)
            {
                if (row["Grade"].ToString().Trim() != "Total:")
                {
                    int boundary = Convert.ToInt32(row["Boundary"], CultureInfo.CurrentUICulture);
                    int sellThruIndex = 0;
                    int rule = 0;
                    double qty = 0;

                    for (int j = 10; j < _screenMatrixDataTable.Columns.Count; j++)
                    {
                        DataColumn col = _screenMatrixDataTable.Columns[j];
                        if (col.ColumnName.Substring(0, 4) == "Rule")
                        {
                            sellThruIndex = Convert.ToInt32(col.ColumnName.Substring(4), CultureInfo.CurrentUICulture);
                            if (row[col] != System.DBNull.Value)
                            {
                                rule = Convert.ToInt32(row[col], CultureInfo.CurrentUICulture);
                                j++;
                                col = _screenMatrixDataTable.Columns[j];
                                if (row[col] != System.DBNull.Value)
                                    qty = Convert.ToDouble(row[col], CultureInfo.CurrentUICulture);
                                else
                                    qty = Include.UndefinedDouble;
                                _dbMatrixDataTable.Rows.Add(new object[] { aSetValue, boundary, sellThruIndex, rule, qty });
                            }
                        }
                    }
                }
            }
            _dbMatrixDataTable.AcceptChanges();
        }
        private void LoadAttributeSetValues(int SetValue)
        {
            //if (_groupLevelDataTable != null)
            //	_groupLevelDataTable.DefaultView.RowFilter = "SglRID = " +  SetValue.ToString();
            DataRow row = null;
            if (_groupLevelDataTable.Rows.Count > 0)
            {
                foreach (DataRow dRow in _groupLevelDataTable.Rows)
                {
                    if ((int)dRow["SglRID"] == SetValue)
                    {
                        row = dRow;
                        break;
                    }
                }
                if (row != null)
                {
                    if (row["NoOnHandRule"] != System.DBNull.Value)
                        cboNoOHRule.SelectedValue = (int)row["NoOnHandRule"];
                    else
                        cboNoOHRule.SelectedIndex = 0;
                    if (row["NoOnHandQty"] != System.DBNull.Value)
                        txtNoOHQuantity.Text = row["NoOnHandQty"].ToString();
                    else
                        txtNoOHQuantity.Text = string.Empty;
                    // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) 
                    if (row["AverageRule"] != System.DBNull.Value)
                        cboMatrixModeAvgRule.SelectedValue = (int)row["AverageRule"];
                    else
                        cboMatrixModeAvgRule.Text = string.Empty;
                    if (row["AverageQty"] != System.DBNull.Value)
                        txtMatrixModeAvgRule.Text = row["AverageQty"].ToString();
                    else
                        txtMatrixModeAvgRule.Text = string.Empty;
                    if (row["ModeInd"] == System.DBNull.Value)
                    {
                        rdoMatrixModeNormal.Checked = true;
                        cboMatrixModeAvgRule.Enabled = false;
                        txtMatrixModeAvgRule.Enabled = false;
                        // Begin TT#3033/TT#671-MD - RMatelic Velocity Method - Avg Matrix Mode - Greyed Out
                        //ugMatrix.Enabled = true;
                        // End TT#3033/TT#671-MD  
                        gbxSpreadOption.Enabled = false;
                        rdoSpreadOptionSmooth.Checked = true;
                    }
                    else
                        if (row["ModeInd"].ToString().Trim() == "N")
                        {
                            rdoMatrixModeNormal.Checked = true;
                            cboMatrixModeAvgRule.Enabled = false;
                            txtMatrixModeAvgRule.Enabled = false;
                            // Begin TT#3033/TT#671-MD - RMatelic Velocity Method - Avg Matrix Mode - Greyed Out
                            //ugMatrix.Enabled = true;
                            // End TT#3033/TT#671-MD  
                            gbxSpreadOption.Enabled = false;
                            rdoSpreadOptionSmooth.Checked = true;
                        }
                        else
                        {
                            rdoMatrixModeAverage.Checked = true;
                            cboMatrixModeAvgRule.Enabled = true;
                            txtMatrixModeAvgRule.Enabled = true;
                            // Begin TT#3033/TT#671-MD - RMatelic Velocity Method - Avg Matrix Mode - Greyed Out
                            //ugMatrix.Enabled = false;
                            // End TT#3033/TT#671-MD  
                            gbxSpreadOption.Enabled = true;
                            rdoSpreadOptionSmooth.Checked = true;
                        }
                    if (row["SpreadInd"] == System.DBNull.Value)
                    {
                        rdoSpreadOptionSmooth.Checked = true;
                        rdoSpreadOptionIdx.Checked = false;
                    }
                    else
                        if (row["SpreadInd"].ToString().Trim() == "S")
                        {
                            rdoSpreadOptionSmooth.Checked = true;
                            rdoSpreadOptionIdx.Checked = false;
                        }
                        else
                        {
                            rdoSpreadOptionSmooth.Checked = false;
                            rdoSpreadOptionIdx.Checked = true;
                        }
                    // END TT#637 - AGallagher - Velocity - Spread Average (#7) 
                }
            }
            if (row == null)
            {
                cboNoOHRule.SelectedIndex = 0;
                txtNoOHQuantity.Text = string.Empty;
                // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                rdoMatrixModeNormal.Checked = true;
                cboMatrixModeAvgRule.Enabled = false;
                txtMatrixModeAvgRule.Enabled = false;
                // Begin TT#3033/TT#671-MD - RMatelic Velocity Method - Avg Matrix Mode - Greyed Out
                //ugMatrix.Enabled = true;
                // End TT#3033/TT#671-MD  
                gbxSpreadOption.Enabled = false;
                rdoSpreadOptionSmooth.Checked = true;
                // END TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
            }
            cboOHRule.SelectedIndex = 0;
            txtOHQuantity.Text = string.Empty;
            MatrixTab_Load();
            if (_statsCalculated)
            {
                GetStoreData();
            }
            // Begin TT#3627 - JSmith - Velocity - using Attribute and sets- grid not populating all sets when process interactive.
            else if (_matrixEverProcessed)
            {
                btnView_Click(this, new System.EventArgs());
            }
            // End TT#3627 - JSmith - Velocity - using Attribute and sets- grid not populating all sets when process interactive.
            // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
            ApplyViewToGridLayout(Convert.ToInt32(cboMatrixView.SelectedValue, CultureInfo.CurrentUICulture));
            // End TT#231  
            // Begin TT#3033/TT#671-MD - RMatelic Velocity Method - Avg Matrix Mode - Greyed Out
            SetMatrixActivation();
            // End TT#3033/TT#671-MD 
        }
        #endregion Attribute and Attribute Set Combo Boxes

        // Begin TT#3033/TT#671-MD - RMatelic Velocity Method - Avg Matrix Mode - Greyed Out
        private void SetMatrixActivation()
        {
            try
            {
                Activation activation = (rdoMatrixModeAverage.Checked ? Activation.Disabled : Activation.AllowEdit);
                foreach (UltraGridBand band in ugMatrix.DisplayLayout.Bands)
                {
                    for (int i = 10; i < band.Columns.Count; i++)
                    {
                        if (band.Columns[i].Tag.ToString() == "Rule" || band.Columns[i].Tag.ToString() == "Qty")
                        {
                            band.Columns[i].CellActivation = activation;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

        }
        // End TT#3033/TT#671-MD

        private void tabVelocity_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            bool errorsFound = false;
            EditMsgs em = new EditMsgs();
            try
            {
                //VelocityGradesTab_Load();
                if (this.tabVelocity.SelectedTab.Name != _currentTabPage.Name)
                {
                    switch (_currentTabPage.Name)
                    {
                        case "tabBasis":
                            if (BasisChangesMade)
                            {
                                if (!ValidTabBasis(ref em))
                                {
                                    errorsFound = true;
                                    this.tabVelocity.SelectedTab = this.tabBasis;
                                }
                            }
                            break;
                        case "tabGrades":
                            if (VelocityGradesChangesMade)
                            {
                                if (!ValidTabVelocityGrades(ref em))
                                {
                                    errorsFound = true;
                                    this.tabVelocity.SelectedTab = this.tabGrades;
                                }
                            }
                            break;
                    }
                }
                if (errorsFound)
                {
                    string text = MIDText.GetTextOnly(eMIDTextCode.msg_ErrorsFoundReviewCorrect);
                    MessageBox.Show(text);
                }
                else
                {
                    _currentTabPage = this.tabVelocity.SelectedTab;
                    switch (_currentTabPage.Name)
                    {

                        case "tabBasis":
                            if (!_basisIsPopulated)
                            {
                                BasisTab_Load();
                            }
                            this.tabVelocity.SelectedTab = this.tabBasis;
                            break;
                        case "tabGrades":
                            if (!_velocityGradesIsPopulated)
                            {
                                VelocityGradesTab_Load();
                            }
                            this.tabVelocity.SelectedTab = this.tabGrades;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void tabVelocityMethod_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            bool errorsFound = false;
            EditMsgs em = new EditMsgs();
            try
            {
                if (this.tabVelocityMethod.SelectedTab.Name != _currentMethodTabPage.Name)
                {
                    switch (_currentMethodTabPage.Name)
                    {
                        case "tabMatrix":
                            if (MatrixChangesMade)
                            {
                                if (!ValidTabMatrix())
                                {
                                    errorsFound = true;
                                    this.tabVelocityMethod.SelectedTab = this.tabMatrix;
                                }
                            }
                            break;
                        // Begin Track #6326 - JSmith - Receive error when Sell Thru % is blank
                        case "tabMethod":
                            switch (_currentTabPage.Name)
                            {
                                case "tabBasis":
                                    if (BasisChangesMade)
                                    {
                                        if (!ValidTabBasis(ref em))
                                        {
                                            errorsFound = true;
                                            this.tabVelocity.SelectedTab = this.tabBasis;
                                        }
                                    }
                                    break;
                                case "tabGrades":
                                    if (VelocityGradesChangesMade)
                                    {
                                        if (!ValidTabVelocityGrades(ref em))
                                        {
                                            errorsFound = true;
                                            this.tabVelocity.SelectedTab = this.tabGrades;
                                        }
                                    }
                                    break;
                            }
                            if (errorsFound)
                            {
                                this.tabVelocityMethod.SelectedTab = this.tabMethod;
                            }
                            break;
                        // End Track #6326
                    }
                }
                if (errorsFound)
                {
                    string text = MIDText.GetTextOnly(eMIDTextCode.msg_ErrorsFoundReviewCorrect);
                    MessageBox.Show(text);
                }
                else
                {
                    _currentMethodTabPage = this.tabVelocityMethod.SelectedTab;
                    switch (_currentMethodTabPage.Name)
                    {
                        case "tabMatrix":
                            if (_rebuildMatrix)
                            {
                                MatrixTab_Load();
                                _rebuildMatrix = false;
                            }
                            this.tabVelocityMethod.SelectedTab = this.tabMatrix;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cboOHRule_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            try
            {
                if (cboOHRule.SelectedIndex > -1)
                {
                    DataRow dr = _defOnHandRules.Rows[cboOHRule.SelectedIndex];
                    eVelocityRuleRequiresQuantity vrq = (eVelocityRuleRequiresQuantity)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
                    if (Enum.IsDefined(typeof(eVelocityRuleRequiresQuantity), vrq))
                    {
                        txtOHQuantity.Enabled = true;
                        switch (vrq)
                        {
                            case eVelocityRuleRequiresQuantity.WeeksOfSupply:
                            case eVelocityRuleRequiresQuantity.ShipUpToQty:
                                txtOHQuantity.MaxLength = 4;
                                break;
                            default:
                                txtOHQuantity.MaxLength = 6;
                                break;
                        }
                    }
                    else
                    {
                        ErrorProvider.SetError(txtOHQuantity, string.Empty);
                        txtOHQuantity.Text = string.Empty;
                        txtOHQuantity.Enabled = false;
                    }

                    if (FormLoaded)
                    {
                        MatrixChangesMade = true;
                    }
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
        private void cboNoOHRule_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            try
            {
                if (cboNoOHRule.SelectedIndex > -1)
                {
                    DataRow dr = _noOnHandRules.Rows[cboNoOHRule.SelectedIndex];
                    eVelocityRuleRequiresQuantity vrq = (eVelocityRuleRequiresQuantity)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
                    if (Enum.IsDefined(typeof(eVelocityRuleRequiresQuantity), vrq))
                    {
                        txtNoOHQuantity.Enabled = true;
                        switch (vrq)
                        {
                            case eVelocityRuleRequiresQuantity.WeeksOfSupply:
                            case eVelocityRuleRequiresQuantity.ShipUpToQty:
                                txtNoOHQuantity.MaxLength = 4;
                                break;
                            default:
                                txtNoOHQuantity.MaxLength = 6;
                                break;
                        }
                    }
                    else
                    {
                        txtNoOHQuantity.Text = string.Empty;
                        txtNoOHQuantity.Enabled = false;
                    }

                    if (FormLoaded && _attrSetChanged == false)
                    {
                        MatrixChangesMade = true;
                        if (_matrixProcessed)
                        {
                            eVelocityRuleType ruleType = (eVelocityRuleType)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
                            int setValue = Convert.ToInt32(this.cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
                            _trans.VelocitySetMatrixNoOnHandRuleType(setValue, ruleType);
                            btnChanges.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
        //private void FormatColumns(Infragistics.Win.UltraWinGrid.UltraGrid ultragrid)
        //{
        //    foreach ( Infragistics.Win.UltraWinGrid.UltraGridBand oBand in ultragrid.DisplayLayout.Bands )
        //    {
        //        foreach ( Infragistics.Win.UltraWinGrid.UltraGridColumn oColumn in oBand.Columns )
        //        {
        //            switch (oColumn.DataType.ToString())
        //            {
        //                case "System.Int32":
        //                    oColumn.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
        //                    oColumn.Format = "#,###,##0";
        //                    break;
        //                case "System.Double":
        //                    oColumn.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
        //                    if (ultragrid == ugMatrix )
        //                        oColumn.Format = "###,###.0";
        //                    else if (ultragrid == ugBasisNodeVersion) // MID Track #3300
        //                        oColumn.Format = "#,###,###.000";
        //                    else
        //                        oColumn.Format = "#,###,###.00";
        //                    break;
        //            }
        //        }
        //    }
        //}
        //End TT#169

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cancel_Click();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        // Begin MID Track 4858 - JSmith - Security changes

        protected override void Call_btnSave_Click()
        {
            try
            {
                // begin MID Track 6471 Error on Update in Velocity after Grade change
                if (_rebuildMatrix)
                {
                    MatrixTab_Load();
                    _rebuildMatrix = false;
                }
                // end MID Track 6471 Error on Update in Velocity after Grade change
                if (cbxInteractive.Checked)
                    if (InInteractiveMode())
                    {
                        ShowSaveDialog();
                        base.btnSave_Click();  // Added by stodd 12.5.2007
                    }
                    else
                    {
                        base.btnSave_Click();
                    }
                else
                {
                    base.btnSave_Click();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            // Removed by stodd 12.5.2007. Added code up a few lines to replace it.
            // worked great in interactive mode, but broke non-interactive mode
            //			try
            //			{
            //				base.btnSave_Click();
            //			}
            //			catch( Exception exception )
            //			{
            //				HandleException(exception);
            //			}	
        }
        // End MID Track 4858

        private void ShowSaveDialog()
        {
            string title;
            WindowSaveItem wsi;
            MIDRetail.Windows.StyleView frmStyleView;
            try
            {
                ArrayList WindowNames = new ArrayList();
                wsi = new WindowSaveItem();
                wsi.Name = this.Text;
                wsi.SaveData = false;
                WindowNames.Add(wsi);
                wsi = new WindowSaveItem();
                wsi.Name = _trans.StyleView.Text;
                wsi.SaveData = false;
                WindowNames.Add(wsi);
                title = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save) + " " + MIDText.GetTextOnly((int)eMethodType.Velocity);
                SaveDialog frm = new SaveDialog(WindowNames, title);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    foreach (WindowSaveItem wsi2 in frm.SaveList)
                    {
                        if (wsi2.SaveData)
                        {
                            if (wsi2.Name == this.Text)
                            {
                                Save_Click(false);
                                if (!ErrorFound && _velocityMethod.Method_Change_Type == eChangeType.add)
                                {
                                    _velocityMethod.Method_Change_Type = eChangeType.update;
                                    btnSave.Text = "&Update";
                                }
                            }
                            else
                            {
                                frmStyleView = (MIDRetail.Windows.StyleView)_trans.StyleView;
                                frmStyleView.SaveFromExternalSource();
                            }
                        }
                    }
                }

                frm.Dispose();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void SaveFromExternalSource()
        {
            Save_Click(false);
            if (!ErrorFound && _velocityMethod.Method_Change_Type == eChangeType.add)
            {
                _velocityMethod.Method_Change_Type = eChangeType.update;
                btnSave.Text = "&Update";
            }
        }
        // BEGIN MID Track #2761 - sync Velocity & Store Detail windows
        public void UpdateFromStoreDetail()
        {
            _velocityMethod.CalculateAverageUsingChain = _trans.VelocityCalculateAverageUsingChain;
            _updateFromStoreDetail = true;
            radAvgChain.Checked = _velocityMethod.CalculateAverageUsingChain;
            radAvgSet.Checked = !_velocityMethod.CalculateAverageUsingChain;
            _updateFromStoreDetail = false;
        }
        // END MID Track #2761  

        private void SetText()
        {
            try
            {
                this.ugVelocityGrades.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Velocity_Grades);
                this.ugSellThruPcts.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Sell_Thru_Pct);
                if (_velocityMethod.Method_Change_Type == eChangeType.update)
                    this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
                else
                    this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);

                this.btnClose.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
                this.btnProcess.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Process);
                // BEGIN MID Track #3102 - Remove OTS Plan Section 
                //this.lblMerch.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);	
                //this.lblBegin.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Beginning);	
                //this.lblShip.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ShippingTo);
                // END MID Track #3102  
                this.lblAttribute.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Attribute);
                this.lblSet.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AttributeSet);
                this.radAvgChain.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AllStores);
                this.radAvgSet.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Set);
                this.lblComponent.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Component);
                // BEGIN MID Track #3102 - Remove OTS Plan Section 
                //this.gbxOTSPlan.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OTSPlan);
                // END MID Track #3102  
                this.gbxAverage.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Average);
                this.gbxShip.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Ship);
                this.radShipBasis.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Basis);
                this.radHeaderStyle.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_HeaderStyle);
                this.cbxSimilarStores.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SimilarStores);

                //TT#152 - Velocity Balance - apicchetti
                this.cbxBalance.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Checkbox_VelocityBalance);
                //TT#152 -Velocity Balance - apicchetti

                // BEGIN TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
                this.cbxReconcile.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Checkbox_VelocityReconcile);
                // END TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance

                // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                this.gbxApplyMinMax.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ApplyMinMaxes);
                this.radApplyMinMaxNone.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ApplyMinMaxNone);
                this.radApplyMinMaxStore.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ApplyMinMaxStore);
                this.radApplyMinMaxVelocity.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ApplyMinMaxVelocity);
                // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)

                // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) 
                this.gbxMatrixMode.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_MatrixMode);
                this.rdoMatrixModeNormal.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_MatrixModeNormal);
                this.rdoMatrixModeAverage.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_MatrixModeAverage);
                this.cbxBalanceToHeader.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_MatrixModeHeaderBalance); //TT#855-MD -jsobek -Velocity Enhancements

                this.gbxSpreadOption.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_MatrixSpreadOption);
                this.rdoSpreadOptionIdx.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_MatrixSpreadOptionIndex);
                this.rdoSpreadOptionSmooth.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_MatrixSpreadOptionSmooth);
                // END TT#637 - AGallagher - Velocity - Spread Average (#7) 

                //Begin TT#855-MD -jsobek -Velocity Enhancements
                this.radGradeVariableSales.Text = MIDText.GetTextOnly((int)eVelocityMethodGradeVariableType.Sales);
                this.radGradeVariableStock.Text = MIDText.GetTextOnly((int)eVelocityMethodGradeVariableType.Stock);
                //End TT#855-MD -jsobek -Velocity Enhancements

                this.tabMethod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Method);
                this.tabBasis.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Basis);
                this.tabGrades.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Grades);
                this.tabMatrix.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Matrix);
                this.btnView.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ProcessInteractive);
                this.btnApply.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Apply);
                _noOnHandLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_NoOnHandStores);

                _lblTotalSales = MIDText.GetTextOnly(eMIDTextCode.lbl_TotalSales);
                _lblAvgSales = MIDText.GetTextOnly(eMIDTextCode.lbl_AvgSales);
                _lblPctTotalSales = MIDText.GetTextOnly(eMIDTextCode.lbl_PctTotalSales);
                _lblAvgSalesIdx = MIDText.GetTextOnly(eMIDTextCode.lbl_AvgSalesIndex);
                _lblAllStores = MIDText.GetTextOnly(eMIDTextCode.lbl_AllStores);
                _lblSet = MIDText.GetTextOnly(eMIDTextCode.lbl_Set);
                // Begin Track #6074
                // Begin TT # 91 - stodd
                //this.cbxBasisGrades.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_StoreGradesByBasis);
                // ENd TT # 91 - stodd
                // End Track #6074
                //BEGIN TT#153 – add variables to velocity matrix - apicchetti
                _lblTotalNumStores = MIDText.GetTextOnly(eMIDTextCode.lbl_TotalNumStores);
                _lblStockPercentOfTotal = MIDText.GetTextOnly(eMIDTextCode.lbl_StockPercentOfTotal);
                _lblAvgStock = MIDText.GetTextOnly(eMIDTextCode.lbl_AvgStock);
                _lblAllocationPercentOfTotal = MIDText.GetTextOnly(eMIDTextCode.lbl_AllocationPercentOfTotal);
                //END TT#153 – add variables to velocity matrix - apicchetti

                // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                gbxApplyMinMax.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ApplyMinMaxes);
                // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)

                // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                gbxMinMaxOpt.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_MinMaxOptions);
                radAllocationMinMax.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AllocMinMax);
                radInventoryMinMax.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_InventoryMinMax);
                lblInventoryBasis.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_InventoryBasis);
                EnhancedToolTip.SetToolTipWhenDisabled(gbxMinMaxOpt, MIDText.GetTextOnly(eMIDTextCode.lbl_VelocityGradesInactive)); 
                // END TT#1287 - AGallagher - Inventory Min/Max

                SetNoOnHandStores(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }


        #region WorkflowMethodFormBase Overrides

        /// <summary>
        /// Gets if workflow or method.
        /// </summary>
        override protected eWorkflowMethodIND WorkflowMethodInd()
        {
            return eWorkflowMethodIND.Methods;
        }
        /// <summary>
        /// Use to set the method name, description, user and global radio buttons
        /// </summary>
        override protected void SetCommonFields()
        {
            WorkflowMethodName = txtName.Text;
            WorkflowMethodDescription = txtDesc.Text;
            GlobalRadioButton = radGlobal;
            UserRadioButton = radUser;
        }

        /// <summary>
        /// Use to set the specific fields in method object before updating
        /// </summary>
        override protected void SetSpecificFields()
        {
            // BEGIN TT#1287 - AGallagher - Inventory Min/Max
            //Merchandise Level
            if (_InventoryInd == 'I')
            {
                DataRow myDataRow = MerchandiseDataTable.Rows[cboInventoryBasis.SelectedIndex];
                eMerchandiseType MerchandiseType = (eMerchandiseType)(Convert.ToInt32(myDataRow["leveltypename"], CultureInfo.CurrentUICulture));
                _velocityMethod.MerchandiseType = MerchandiseType;

                switch (MerchandiseType)
                {
                    case eMerchandiseType.Node:
                        _velocityMethod.MERCH_HN_RID = Convert.ToInt32(myDataRow["key"], CultureInfo.CurrentUICulture);
                        break;
                    case eMerchandiseType.HierarchyLevel:
                        _velocityMethod.MERCH_PHL_SEQ = Convert.ToInt32(myDataRow["key"], CultureInfo.CurrentUICulture);
                        _velocityMethod.MERCH_PH_RID = HP.Key;
                        _velocityMethod.MERCH_HN_RID = Include.NoRID;
                        break;
                    case eMerchandiseType.OTSPlanLevel:
                        _velocityMethod.MERCH_HN_RID = Include.NoRID;
                        _velocityMethod.MERCH_PH_RID = Include.NoRID;
                        _velocityMethod.MERCH_PHL_SEQ = 0;
                        break;
                }}
                else
                {_velocityMethod.MERCH_HN_RID = Include.NoRID;
                        _velocityMethod.MERCH_PH_RID = Include.NoRID;
                        _velocityMethod.MERCH_PHL_SEQ = 0;}
            
            // END TT#1287 - AGallagher - Inventory Min/Max
            //			int lAddTag;
            // BEGIN MID Track #3102 - Remove OTS Plan Section 
            //OTS Plan Level
            //DataRow myDataRow = MerchandiseDataTable.Rows[cboOTSPlan.SelectedIndex];
            //eMerchandiseType MerchandiseType = (eMerchandiseType)(Convert.ToInt32(myDataRow["leveltypename"], CultureInfo.CurrentUICulture)); 

            //switch(MerchandiseType)
            //{
            //	case eMerchandiseType.Node:
            //		_velocityMethod.OTSPlanHNRID = Convert.ToInt32(myDataRow["key"], CultureInfo.CurrentUICulture);
            //		_velocityMethod.OTSPlanPHRID = Include.NoRID;
            //		_velocityMethod.OTSPlanPHLSeq  = 0;
            //		break;
            //	case eMerchandiseType.HierarchyLevel:
            //		_velocityMethod.OTSPlanPHRID = HP.Key;
            //		_velocityMethod.OTSPlanPHLSeq = Convert.ToInt32(myDataRow["key"], CultureInfo.CurrentUICulture);
            //		_velocityMethod.OTSPlanHNRID = Include.NoRID;
            //		break;
            //	case eMerchandiseType.OTSPlanLevel:
            _velocityMethod.OTSPlanHNRID = Include.NoRID;
            _velocityMethod.OTSPlanPHRID = Include.NoRID;
            _velocityMethod.OTSPlanPHLSeq = 0;
            //		break;
            //}
            //Beginning Target Time Period  
            //if (midDateRangeSelectorBeg.Tag !=null)
            //{
            //	lAddTag = (int)midDateRangeSelectorBeg.Tag;
            //	_velocityMethod.OTS_Begin_CDR_RID = lAddTag;
            //}               
            //else
            //{
            _velocityMethod.OTS_Begin_CDR_RID = Include.UndefinedCalendarDateRange;
            //}
            //Ship To Target Time Period  
            //if (midDateRangeSelectorShip.Tag !=null)
            //{
            //	lAddTag = (int)midDateRangeSelectorShip.Tag;
            //	_velocityMethod.OTS_Ship_To_CDR_RID = lAddTag;
            //}               
            //else
            //{
            _velocityMethod.OTS_Ship_To_CDR_RID = Include.UndefinedCalendarDateRange;
            //}
            // END MID Track #3102 
            _velocityMethod.SG_RID = Convert.ToInt32(this.cboStoreAttribute.SelectedValue, CultureInfo.CurrentUICulture);
            _velocityMethod.StoreGroupRID = Convert.ToInt32(this.cboStoreAttribute.SelectedValue, CultureInfo.CurrentUICulture);
            _velocityMethod.CalculateAverageUsingChain = radAvgChain.Checked;
            _velocityMethod.DetermineShipQtyUsingBasis = radShipBasis.Checked;
            _velocityMethod.UseSimilarStoreHistory = cbxSimilarStores.Checked;
            // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
            _velocityMethod.ApplyMinMaxInd = _ApplyMinMaxInd;
            // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
            _velocityMethod.InventoryInd = _InventoryInd;   // TT#1287 - AGallagher - Inventory Min/Max


            //Begin TT#855-MD -jsobek -Velocity Enhancements
            if (radGradeVariableSales.Checked == true)
            {
                _velocityMethod.GradeVariableType = eVelocityMethodGradeVariableType.Sales;
            }
            else if (radGradeVariableStock.Checked == true)
            {
                _velocityMethod.GradeVariableType = eVelocityMethodGradeVariableType.Stock;
            }

            if (cbxBalanceToHeader.Checked == true)
            {
                _velocityMethod.BalanceToHeaderInd = '1';
            }
            else
            {
                _velocityMethod.BalanceToHeaderInd = '0';
            }
            //End TT#855-MD -jsobek -Velocity Enhancements


            // Begin Track #6074
            // Begin TT # 91 - stodd
            //_velocityMethod.GradesByBasisInd = cbxBasisGrades.Checked;
            // End TT # 91 - stodd
            // End Track #6074
            _velocityMethod.TrendPctContribution = cbxTrendPct.Checked;
            SetBasisTabData();
            _velocityGradesDataTable.AcceptChanges();
            _sellThruPctsDataTable.AcceptChanges();

            SetAttributeSetValues(Convert.ToInt32(this.cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture));
            _dsVelocity.Tables["VelocityMatrix"].AcceptChanges();
            //CheckForSetRemoval(); // TT#637 - AGallagher - Velocity - Spread Average (#7) 
            CheckForSellThruRemoval(); //TT#3963 - DOConnell - Delete Sell Thru %'s  causes Foreign Key Violation on update
            // if screen table already exists, remove it
            if (_dsVelocity.Tables.Contains("ScreenMatrix"))
                _dsVelocity.Tables.Remove("ScreenMatrix");
            _dsVelocity.Tables.Add(_screenMatrixDataTable);

            _velocityMethod.DSVelocity = _dsVelocity;

            SetComponent();

            // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail 
            SaveUserGridView = true;
            LayoutID = eLayoutID.velocityMatrixGrid;
            SaveViewGrid = CreateGridFromColumnList();
            ViewRID = Convert.ToInt32(cboMatrixView.SelectedValue, CultureInfo.CurrentUICulture);
            if (ViewRID != 0 && ViewRID != Include.NoRID)
            {
                SaveAsViewName = cboMatrixView.Text;
                DataRow viewRow = _dtMatrixView.Rows.Find(ViewRID);
                if (viewRow != null)
                {
                    SaveAsViewUserRID = Convert.ToInt32(viewRow["USER_RID"], CultureInfo.CurrentUICulture);
                }
                else if (MethodViewGlobalSecurity.AllowUpdate)
                {
                    SaveAsViewUserRID = Include.GlobalUserRID;
                }
                else if (MethodViewUserSecurity.AllowUpdate)
                {
                    SaveAsViewUserRID = SAB.ClientServerSession.UserRID;
                }
            }
            else
            {
                SaveAsViewName = null;
                if (MethodViewGlobalSecurity.AllowUpdate)
                {
                    SaveAsViewUserRID = Include.GlobalUserRID;
                }
                else if (MethodViewUserSecurity.AllowUpdate)
                {
                    SaveAsViewUserRID = SAB.ClientServerSession.UserRID;
                }
            }
            GetStoreDetailView();
            // End TT#231  
        }

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        private UltraGrid CreateGridFromColumnList()
        {
            Infragistics.Win.UltraWinGrid.UltraGrid ugMatrixViewSave = new UltraGrid();
            try
            {
                string colKey;
                int sortSequence;
                SortedList sortedColumns = new SortedList();

                ugMatrixViewSave.DisplayLayout.Bands[0].Key = "ScreenMatrix";
                // need to save first 2 columns;  these are not in _matrixColumnHeaders  
                for (int i = 0; i < 2; i++)
                {
                    colKey = this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Key;
                    ugMatrixViewSave.DisplayLayout.Bands[0].Columns.Add(colKey);
                    ugMatrixViewSave.DisplayLayout.Bands[0].Columns[colKey].Hidden = this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Hidden;
                    ugMatrixViewSave.DisplayLayout.Bands[0].Columns[colKey].Width = this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Width;
                    ugMatrixViewSave.DisplayLayout.Bands[0].Columns[colKey].Header.VisiblePosition
                                                                                  = this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Header.VisiblePosition;
                    if (ugMatrix.DisplayLayout.Bands[0].SortedColumns.Exists(colKey))
                    {
                        sortSequence = -1;
                        for (int j = 0; j < ugMatrix.DisplayLayout.Bands[0].SortedColumns.Count; j++)
                        {
                            if (ugMatrix.DisplayLayout.Bands[0].SortedColumns[j].Key == colKey)
                            {
                                sortSequence = j;
                                break;
                            }
                        }
                        sortedColumns.Add(sortSequence, colKey);
                    }
                }

                foreach (RowColHeader rch in _matrixColumnHeaders)
                {
                    for (int i = 2; i <= (_matrixColumnHeaders.Count + 1); i++)
                    {
                        if (this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Tag.ToString() == rch.Name)
                        {
                            colKey = this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Key;
                            ugMatrixViewSave.DisplayLayout.Bands[0].Columns.Add(colKey);
                            ugMatrixViewSave.DisplayLayout.Bands[0].Columns[colKey].Hidden = !rch.IsDisplayed;
                            ugMatrixViewSave.DisplayLayout.Bands[0].Columns[colKey].Width = this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Width;
                            ugMatrixViewSave.DisplayLayout.Bands[0].Columns[colKey].Header.VisiblePosition
                                            = this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Header.VisiblePosition;

                            if (ugMatrix.DisplayLayout.Bands[0].SortedColumns.Exists(colKey))
                            {
                                sortSequence = -1;
                                for (int j = 0; j < ugMatrix.DisplayLayout.Bands[0].SortedColumns.Count; j++)
                                {
                                    if (ugMatrix.DisplayLayout.Bands[0].SortedColumns[j].Key == colKey)
                                    {
                                        sortSequence = j;
                                        break;
                                    }
                                }
                                sortedColumns.Add(sortSequence, colKey);
                            }
                            break;
                        }
                    }
                }
                if (sortedColumns.Count > 0)
                {
                    for (int i = 0; i < sortedColumns.Count; i++)
                    {
                        colKey = sortedColumns[i].ToString();
                        if (ugMatrix.DisplayLayout.Bands[0].SortedColumns.Exists(colKey))
                        {
                            bool sortDescending = false;
                            if (ugMatrix.DisplayLayout.Bands[0].SortedColumns[colKey].SortIndicator == SortIndicator.Descending)
                            {
                                sortDescending = true;
                            }
                            ugMatrixViewSave.DisplayLayout.Bands[0].SortedColumns.Add(colKey, sortDescending);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return ugMatrixViewSave;
        }

        public void GetStoreDetailView()
        {
            if (InInteractiveMode())
            {
                ShowDetailViewOption = true;
                FrmStyleView = (MIDRetail.Windows.StyleView)_trans.StyleView;
                DataRow detailViewRow = FrmStyleView.GetStoreDetailViewRow();
                if (detailViewRow != null)
                {
                    DetailViewRID = Convert.ToInt32(detailViewRow["VIEW_RID"], CultureInfo.CurrentUICulture);
                    if (DetailViewRID != Include.NoRID)
                    {
                        SaveAsDetailViewName = Convert.ToString(detailViewRow["VIEW_ID"], CultureInfo.CurrentUICulture);
                        SaveAsDetailViewUserRID = Convert.ToInt32(detailViewRow["USER_RID"], CultureInfo.CurrentUICulture);
                    }
                    else
                    {
                        SaveAsDetailViewName = null;
                        if (MethodDetailViewGlobalSecurity.AllowUpdate)
                        {
                            SaveAsDetailViewUserRID = Include.GlobalUserRID;
                        }
                        else if (MethodDetailViewUserSecurity.AllowUpdate)
                        {
                            SaveAsDetailViewUserRID = SAB.ClientServerSession.UserRID;
                        }
                    }
                }
                else
                {
                    SaveAsDetailViewName = null;
                    if (MethodDetailViewGlobalSecurity.AllowUpdate)
                    {
                        SaveAsDetailViewUserRID = Include.GlobalUserRID;
                    }
                    else if (MethodDetailViewUserSecurity.AllowUpdate)
                    {
                        SaveAsDetailViewUserRID = SAB.ClientServerSession.UserRID;
                    }
                }
            }
            else
            {
                ShowDetailViewOption = false;
            }
        }
        // End TT#231 

		//BEGIN TT#3963 - DOConnell - Delete Sell Thru %'s  causes Foreign Key Violation on update
        private void CheckForSellThruRemoval()
        {
            DataRow setRow;
            
            for (int i = _dbMatrixDataTable.Rows.Count - 1; i >= 0; i--)
            {
                setRow = _dbMatrixDataTable.Rows[i];

                int mSellThru = (int)setRow["SellThruIndex"];

                if (!_sellThruPctsDataTable.Rows.Contains(mSellThru))
                {
                    setRow.Delete();
                }

            }
            _dbMatrixDataTable.AcceptChanges();
        }
		//END TT#3963 - DOConnell - Delete Sell Thru %'s  causes Foreign Key Violation on update
		
        private void CheckForSetRemoval()
        {
            bool deleteRow;
            DataRow setRow;
            int nohRule;
            for (int i = _groupLevelDataTable.Rows.Count - 1; i >= 0; i--)
            {
                deleteRow = false;
                setRow = _groupLevelDataTable.Rows[i];
                nohRule = Convert.ToInt32(setRow["NoOnHandRule"], CultureInfo.CurrentUICulture);
                if (nohRule == (int)eVelocityRuleType.None)
                {
                    if (_dbMatrixDataTable.Rows.Count > 0)
                    {
                        bool setFound = false;
                        foreach (DataRow row in _dbMatrixDataTable.Rows)
                        {
                            int mSetRid = (int)row["SglRID"];
                            int gSetRid = (int)setRow["SglRID"];
                            if (mSetRid == gSetRid)
                            {
                                setFound = true;
                                break;
                            }
                        }
                        if (!setFound)
                            deleteRow = true;
                    }
                    else
                        deleteRow = true;
                }
                if (deleteRow)
                    setRow.Delete();
            }
            _groupLevelDataTable.AcceptChanges();
        }
        private void SetComponent()
        {
            int compValue;
            eComponentType compType;
            try
            {
                if (cbxInteractive.Checked)
                {
                    compValue = Convert.ToInt32(cboComponent.SelectedValue, CultureInfo.CurrentUICulture);

                    foreach (DataRow row in _compDataTable.Rows)
                    {
                        if ((int)row["TEXT_CODE"] == compValue)
                        {
                            compType = (eComponentType)row["CompType"];
                            switch (compType)
                            {
                                case eComponentType.SpecificPack:
                                    _velocityMethod.Component = new AllocationPackComponent(Convert.ToString(row["TEXT_VALUE"], CultureInfo.CurrentUICulture));
                                    break;
                                case eComponentType.SpecificColor:
                                    _velocityMethod.Component = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, compValue);
                                    break;
                                case eComponentType.Total:
                                    _velocityMethod.Component = new GeneralComponent(eGeneralComponentType.Total);
                                    break;
                                case eComponentType.Bulk:
                                    _velocityMethod.Component = new GeneralComponent(eGeneralComponentType.Bulk);
                                    break;
                            }
                            break;
                        }
                    }
                }
                else
                    _velocityMethod.Component = new GeneralComponent(eGeneralComponentType.Total);

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        /// <summary>
        /// Use to validate the fields that are specific to this method type
        /// </summary>
        override protected bool ValidateSpecificFields()
        {
            bool methodFieldsValid = true;
            //			string errorMessage;
            // BEGIN MID Track #3102 - Remove OTS Plan Section 
            //initialize all fields to not having an error
            //ErrorProvider.SetError (midDateRangeSelectorBeg,string.Empty);
            //ErrorProvider.SetError (midDateRangeSelectorShip,string.Empty);
            //DateRangeProfile Begdr = null;
            //DateRangeProfile Shipdr = null;
            //System.ComponentModel.CancelEventArgs args = new CancelEventArgs();

            // _showMessageBox = false;
            //cboOTSPlan_Validating(cboOTSPlan, args);
            //if (ErrorProvider.GetError(cboOTSPlan) != string.Empty)
            //	methodFieldsValid = false;
            //_showMessageBox = true;

            // get posting week
            //WeekProfile currentWeek = SAB.HierarchyServerSession.GetCurrentDate().Week;
            //			if (midDateRangeSelectorBeg.Text.Trim() != string.Empty)
            //			{
            //				Begdr = SAB.ClientServerSession.Calendar.GetDateRange(midDateRangeSelectorBeg.DateRangeRID);
            //			}
            //			if (midDateRangeSelectorShip.Text.Trim() != string.Empty)
            //			{
            //				Shipdr = SAB.ClientServerSession.Calendar.GetDateRange(midDateRangeSelectorShip.DateRangeRID);
            //			}
            //			if (midDateRangeSelectorBeg.Text.Trim() != string.Empty)
            //			{
            //				if (Begdr.DateRangeType == eCalendarRangeType.Dynamic) 
            //				{
            //					if (Begdr.StartDateKey < 0) 
            //					{
            //						methodFieldsValid = false;
            //						ErrorProvider.SetError (midDateRangeSelectorBeg,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DateLessThanSalesWeek));
            //						this.tabVelocityMethod.SelectedTab = this.tabMethod;
            //						return methodFieldsValid;
            //					}	
            //				}
            //				else if (Begdr.StartDateKey < currentWeek.Key)
            //				{
            //					methodFieldsValid = false;
            //					ErrorProvider.SetError (midDateRangeSelectorBeg,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DateLessThanSalesWeek));
            //					this.tabVelocityMethod.SelectedTab = this.tabMethod;
            //					return methodFieldsValid;
            //				}
            //			} 
            //			// Shipping  date is required if begginning date is present 
            //			if (midDateRangeSelectorBeg.Text.Trim() != string.Empty 
            //				&& midDateRangeSelectorShip.Text.Trim() == string.Empty)
            //			{
            //				methodFieldsValid = false;
            //				ErrorProvider.SetError (midDateRangeSelectorShip,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
            //				this.tabVelocityMethod.SelectedTab = this.tabMethod;
            //				return methodFieldsValid;
            //			}
            //			
            //			if (midDateRangeSelectorBeg.Text.Trim() != string.Empty 
            //				&& midDateRangeSelectorShip.Text.Trim() != string.Empty
            //				&& Begdr.DateRangeType != Shipdr.DateRangeType)
            //			{
            //				methodFieldsValid = false;
            //				errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DatesMustBeSameType);
            //				ErrorProvider.SetError (midDateRangeSelectorBeg,errorMessage);
            //				ErrorProvider.SetError (midDateRangeSelectorShip,errorMessage);
            //				this.tabVelocityMethod.SelectedTab = this.tabMethod;
            //				return methodFieldsValid;
            //			}
            //			if (midDateRangeSelectorShip.Text.Trim() != string.Empty)
            //			{
            //				if (Shipdr.DateRangeType == eCalendarRangeType.Dynamic) 
            //				{
            //					if (Shipdr.StartDateKey < 0) 
            //					{
            //						methodFieldsValid = false;
            //						ErrorProvider.SetError (midDateRangeSelectorShip,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DateLessThanSalesWeek));
            //						this.tabVelocityMethod.SelectedTab = this.tabMethod;
            //						return methodFieldsValid;
            //					}	
            //				}
            //				else if (Shipdr.StartDateKey < currentWeek.Key)
            //				{
            //					methodFieldsValid = false;
            //					ErrorProvider.SetError (midDateRangeSelectorShip,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DateLessThanSalesWeek));
            //					this.tabVelocityMethod.SelectedTab = this.tabMethod;
            //					return methodFieldsValid;
            //				}
            //			} 
            //			if (midDateRangeSelectorBeg.Text.Trim() != string.Empty
            //				&& midDateRangeSelectorShip.Text.Trim() != string.Empty)
            //			{
            //				if ( Begdr.StartDateKey >  Shipdr.StartDateKey)
            //				{
            //					methodFieldsValid = false;
            //					ErrorProvider.SetError (midDateRangeSelectorBeg,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DateCannotBeGreater));
            //					this.tabVelocityMethod.SelectedTab = this.tabMethod;
            //					return methodFieldsValid;
            //				}	
            //			}
            // END MID Track #3102  
            // Begin Track #4872 - JSmith - Global/User Attributes
            if (cboStoreAttribute.SelectedIndex == Include.Undefined)
            {
                methodFieldsValid = false;
                ErrorProvider.SetError(cboStoreAttribute, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
            }
            else
            {
                ErrorProvider.SetError(cboStoreAttribute, string.Empty);
            }
            // End Track #4872

            EditMsgs em = new EditMsgs();
            if (!ValidTabBasis(ref em))
            {
                methodFieldsValid = false;
                this.tabVelocity.SelectedTab = this.tabBasis;
            }

            if (methodFieldsValid)
            {
                if (!ValidTabVelocityGrades(ref em))
                {
                    methodFieldsValid = false;
                    this.tabVelocity.SelectedTab = this.tabGrades;
                }
            }

            if (methodFieldsValid)
            {
                if (!ValidTabMatrix())
                {
                    methodFieldsValid = false;
                    this.tabVelocityMethod.SelectedTab = this.tabMatrix;
                }
            }
            return methodFieldsValid;
        }

        /// <summary>
        /// Use to set the errors to the screen
        /// </summary>
        override protected void HandleErrors()
        {
            if (!WorkflowMethodNameValid)
            {
                ErrorProvider.SetError(txtName, WorkflowMethodNameMessage);
            }
            else
            {
                ErrorProvider.SetError(txtName, "");
            }
            if (!WorkflowMethodDescriptionValid)
            {
                ErrorProvider.SetError(txtDesc, WorkflowMethodDescriptionMessage);
            }
            else
            {
                ErrorProvider.SetError(txtDesc, "");
            }
            if (!UserGlobalValid)
            {
                ErrorProvider.SetError(pnlGlobalUser, UserGlobalMessage);
            }
            else
            {
                ErrorProvider.SetError(pnlGlobalUser, "");
            }
        }

        /// <summary>
        /// Use to set the specific method object before updating
        /// </summary>
        override protected void SetObject()
        {
            try
            {
                ABM = _velocityMethod;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Use to return the explorer node selected when form was opened
        /// </summary>
        override protected MIDWorkflowMethodTreeNode GetExplorerNode()
        {
            return ExplorerNode;
        }

        // Begin TT#330 - RMatelic -Velocity Method View when do a Save As of a view after the Save the method closes
        /// <summary>
        /// Use to set any specific window data
        /// </summary>
        override protected void UpdateAdditionalData()
        {
            try
            {
                BindMatrixViewCombo();
                cboMatrixView.SelectedValue = ViewRID;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // End TT#330  


        #endregion WorkflowMethodFormBase Overrides

        #region IFormBase Members
        //		override public void ICut()
        //		{
        //			
        //		}
        //
        //		override public void ICopy()
        //		{
        //			
        //		}
        //
        //		override public void IPaste()
        //		{
        //			
        //		}	

        //		override public void IClose()
        //		{
        //			try
        //			{
        //				this.Close();
        //
        //			}		
        //			catch (Exception ex)
        //			{
        //				HandleException(ex);
        //			}
        //			
        //		}

        override public void ISave()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SaveChanges();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        //		override public void ISaveAs()
        //		{
        //			
        //		}
        //
        //		override public void IDelete()
        //		{
        //			
        //		}
        //
        //		override public void IRefresh()
        //		{
        //			
        //		}

        #endregion

        #region Validate Basis Tab
        private bool ValidTabBasis(ref EditMsgs em)
        {
            string errorMessage = null;
            bool errorFound = false;
            _errorCells = new ArrayList();
            ErrorProvider.SetError(ugBasisNodeVersion, string.Empty);
            try
            {
                if (ugBasisNodeVersion.Rows.Count == 0)
                {
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                    ugBasisNodeVersion.Tag = errorMessage;
                    ErrorProvider.SetError(ugBasisNodeVersion, errorMessage);
                    errorFound = true;
                }
                else
                {
                    foreach (UltraGridRow gridRow in ugBasisNodeVersion.Rows)
                    {
                        if (!ValidMerchandise(gridRow.Cells["Merchandise"]))
                        {
                            //em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
                            errorFound = true;
                        }
                        if (!ValidVersion(gridRow.Cells["BasisFVRID"]))
                        {
                            //em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
                            errorFound = true;
                        }
                        if (!DateRangeValid(gridRow.Cells["DateRange"]))
                        {
                            //em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
                            errorFound = true;
                        }
                        if (!WeightValid(gridRow.Cells["Weight"]))
                        {
                            //em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
                            errorFound = true;
                        }
                    }
                }

                if (errorFound)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }
        private bool ValidMerchandise(UltraGridCell gridCell)
        {
            bool errorFound = false;
            string errorMessage = string.Empty;
            int rowseq = -1;
            try
            {
                string productID = gridCell.Value.ToString().Trim();
                if (productID == string.Empty)
                {
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                    errorFound = true;
                }
                else
                {
                    foreach (Infragistics.Win.ValueListItem vli in ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems)
                    {
                        if (Convert.ToInt32(vli.DataValue, CultureInfo.CurrentUICulture) == Convert.ToInt32(gridCell.Value, CultureInfo.CurrentUICulture))
                        {
                            rowseq = Convert.ToInt32(vli.DataValue, CultureInfo.CurrentUICulture);
                            break;
                        }

                    }
                    if (rowseq != -1)
                    {
                        DataRow row = _merchDataTable2.Rows[rowseq];
                        if (row != null)
                            return true;
                    }
                    //					key = GetNodeText(ref productID);
                    HierarchyNodeProfile hnp = GetNodeProfile(productID);
                    if (hnp.Key == Include.NoRID)
                    {
                        errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
                            productID);
                        errorFound = true;
                    }
                }
            }
            catch (Exception error)
            {
                string exceptionMessage = error.Message;
                errorMessage = error.Message;
                errorFound = true;
            }

            if (errorFound)
            {
                gridCell.Appearance.Image = ErrorImage;
                gridCell.Tag = errorMessage;
                return false;
            }
            else
            {
                gridCell.Appearance.Image = null;
                gridCell.Tag = null;
                return true;
            }
        }
        private bool ValidVersion(UltraGridCell gridCell)
        {
            bool errorFound = false;
            string errorMessage = string.Empty;
            try
            {
                if (gridCell.Text.Length == 0)	// cell is empty - use default
                {
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                    errorFound = true;
                }
                else
                {
                    //int cellValue = Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture);
                    //if (cellValue < 0)
                    //{
                    //	errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
                    //	errorFound = true;
                    //}
                }
            }
            catch (Exception error)
            {
                string exceptionMessage = error.Message;
                errorMessage = error.Message;
                errorFound = true;
            }

            if (errorFound)
            {
                gridCell.Appearance.Image = ErrorImage;
                gridCell.Tag = errorMessage;
                return false;
            }
            else
            {
                gridCell.Appearance.Image = null;
                gridCell.Tag = null;
                return true;
            }
        }
        private bool DateRangeValid(UltraGridCell gridCell)
        {
            bool errorFound = false;
            string errorMessage = string.Empty;
            try
            {
                if (gridCell.Text.Length == 0)	// cell is empty - use default
                {
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                    errorFound = true;
                }
                else
                {
                    //int cellValue = Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture);
                    //if (cellValue < 0)
                    //{
                    //	errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
                    //	errorFound = true;
                    //}
                }
            }
            catch (Exception error)
            {
                string exceptionMessage = error.Message;
                errorMessage = error.Message;
                errorFound = true;
            }

            if (errorFound)
            {
                gridCell.Appearance.Image = ErrorImage;
                gridCell.Tag = errorMessage;
                _errorCells.Add(gridCell);
                return false;
            }
            else
            {
                // Issue 4393 stodd 10.17.2007
                // This was inadverantly removing the dynamic type indicator for the date range.
                if (gridCell.Appearance.Image == ErrorImage)
                    gridCell.Appearance.Image = null;
                gridCell.Tag = null;
                return true;
            }
        }
        private bool WeightValid(UltraGridCell gridCell)
        {
            bool errorFound = false;
            string errorMessage = string.Empty;
            double dblValue;
            try
            {
                if (gridCell.Text.Length == 0)	// cell is empty - use default
                {
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                    errorFound = true;
                }
                else
                {
                    dblValue = Convert.ToDouble(gridCell.Text, CultureInfo.CurrentUICulture);
                    // BEGIN MID Track #3300 - Weight can be less than 1 and up to 3 decimal positions 
                    //if (dblValue < 1)
                    dblValue = Math.Round(dblValue, 3);
                    // BEGIN MID Track #3878 - JSmith - Receive Weight has changed message
                    _changedByCode = true;
                    // END MID Track #3878
                    gridCell.Value = dblValue.ToString(CultureInfo.CurrentUICulture);
                    // BEGIN Issue 4393 - stodd 10.4.2007
                    _changedByCode = false;
                    // END Issue 4393 - stodd 10.4.2007
                    if (dblValue < .001)
                    {
                        //errorMessage = string.Format
                        //	(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MinimumValueExceeded),dblValue, "1");
                        errorMessage = string.Format
                            (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MinimumValueExceeded), dblValue, ".001");
                        errorFound = true;
                    }
                    else if (dblValue > 9999)
                    {
                        errorMessage = string.Format
                            (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MaximumValueExceeded), dblValue, "9999");
                        errorFound = true;
                    }
                    // END MID Track #3300
                }
            }
            catch (Exception error)
            {
                string exceptionMessage = error.Message;
                errorMessage = error.Message;
                errorFound = true;
            }

            if (errorFound)
            {
                gridCell.Appearance.Image = ErrorImage;
                gridCell.Tag = errorMessage;
                return false;
            }
            else
            {
                gridCell.Appearance.Image = null;
                gridCell.Tag = null;
                return true;
            }
        }
        #endregion Validate Basis Tab

        #region Set Basis Tab
        private void SetBasisTabData()
        {
            try
            {
                int seq = 0;
                _basisDataTable.DefaultView.Sort = "BasisSequence";

                foreach (DataRowView row in _basisDataTable.DefaultView)
                {
                    row["BasisSequence"] = seq;
                    seq++;
                }
                _basisDataTable.AcceptChanges();

                //				seq = 0;
                //				_salesPeriodDataTable.DefaultView.Sort = "PeriodSequence";
                //				
                //				foreach (DataRowView row in _salesPeriodDataTable.DefaultView)
                //				{
                //					row["PeriodSequence"] = seq;
                //					seq++;
                //				}
                //				_salesPeriodDataTable.AcceptChanges();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        #endregion Set Basis Tab

        #region Validate Velocity Grades Tab
        private bool ValidTabVelocityGrades(ref EditMsgs em)
        {
            string errorMessage = string.Empty;
            bool errorFound = false;
            ErrorProvider.SetError(ugVelocityGrades, string.Empty);
            ErrorProvider.SetError(ugSellThruPcts, string.Empty);

            try
            {
                if (ugVelocityGrades.Rows.Count < 2)
                {
                    //errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                    errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_EntriesLessThanMinimum),
                        Include.MinVelocityGrades.ToString(CultureInfo.CurrentUICulture),
                        MIDText.GetTextOnly(eMIDTextCode.lbl_Velocity_Grades));
                    ugVelocityGrades.Tag = errorMessage;
                    ErrorProvider.SetError(ugVelocityGrades, errorMessage);
                    errorFound = true;
                }
                else
                {
                    ValidVelocityGrades(ref em);
                }

                if (ugSellThruPcts.Rows.Count == 0)
                {
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                    ugSellThruPcts.Tag = errorMessage;
                    ErrorProvider.SetError(ugSellThruPcts, errorMessage);
                    errorFound = true;
                }
                else
                {
                    ValidSellThruPcts(ref em);
                }

                if (em.ErrorFound || errorFound)
                {
                    return false;
                }
                else
                {
                    _dsVelocity.Tables["VelocityGrade"].AcceptChanges();
                    _dsVelocity.Tables["SellThru"].AcceptChanges();
                    return true;
                }

            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }

        private bool ValidVelocityGrades(ref EditMsgs em)
        {
            int lastBoundary = 0;
            string errorMessage = null;
            // sort in descending order by boundary
            this.ugVelocityGrades.DisplayLayout.Bands[0].SortedColumns.Clear();
            this.ugVelocityGrades.DisplayLayout.Bands[0].SortedColumns.Add("Boundary", true);
            UltraGridRow lastRow = null;

            foreach (UltraGridRow gridRow in ugVelocityGrades.Rows)
            {
                lastRow = gridRow;
                if (!VelocityGradeValid(gridRow.Cells["Grade"], ref errorMessage))
                {
                    em.AddMsg(eMIDMessageLevel.Edit, errorMessage, this.ToString());
                }

                if (!VelocityBoundaryValid(gridRow.Cells["Boundary"], ref errorMessage))
                {
                    em.AddMsg(eMIDMessageLevel.Edit, errorMessage, this.ToString());

                }
                else
                {
                    lastBoundary = Convert.ToInt32(gridRow.Cells["Boundary"].Value, CultureInfo.CurrentUICulture);
                }
            }

            if (lastBoundary != 0)
            {
                errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_LastBoundaryNotZero);
                em.AddMsg(eMIDMessageLevel.Edit, errorMessage, this.ToString());
                lastRow.Cells["Boundary"].Appearance.Image = ErrorImage;
                lastRow.Cells["Boundary"].Tag = errorMessage;
            }

            if (em.ErrorFound)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool VelocityGradeValid(UltraGridCell gridCell, ref string errorMessage)
        {
            bool errorFound = false;
            try
            {
                //				if (Convert.IsDBNull(gridCell.Value) ||
                //					(string)gridCell.Value == string.Empty)	// cell is empty
                if (gridCell.Text.Length == 0)	// cell is empty
                {
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                    errorFound = true;
                }
                else
                {
                    // make sure grades are unique
                    foreach (UltraGridRow gridRow in this.ugVelocityGrades.Rows)
                    {
                        //						if (!gridRow.IsActiveRow)
                        if (gridCell.Row != gridRow)
                        {
                            if ((string)gridCell.Text == (string)gridRow.Cells["Grade"].Text)
                            {
                                errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_VelocityGradesNotUnique);
                                errorFound = true;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception error)
            {
                string exceptionMessage = error.Message;
                errorMessage = error.Message;
                errorFound = true;
            }

            if (errorFound)
            {
                gridCell.Appearance.Image = ErrorImage;
                gridCell.Tag = errorMessage;
                return false;
            }
            else
            {
                gridCell.Appearance.Image = null;
                gridCell.Tag = null;
                return true;
            }
        }

        private bool VelocityBoundaryValid(UltraGridCell gridCell, ref string errorMessage)
        {
            bool errorFound = false;
            try
            {
                //				if (Convert.IsDBNull(gridCell.Value)) // cell is empty
                if (gridCell.Text.Length == 0)	// cell is empty
                {
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                    errorFound = true;
                }
                else
                {
                    int cellValue = Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture);
                    if (cellValue < 0)
                    {
                        errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
                        errorFound = true;
                    }
                    else
                    {
                        // make sure boundaries are unique
                        foreach (UltraGridRow gridRow in this.ugVelocityGrades.Rows)
                        {
                            if (gridCell.Row != gridRow)	// Don't check yourself
                            {
                                if (Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture) == Convert.ToInt32(gridRow.Cells["Boundary"].Text, CultureInfo.CurrentUICulture))
                                {
                                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_GradeBoundariesNotUnique);
                                    errorFound = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception error)
            {
                string exceptionMessage = error.Message;
                errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeInteger);
                errorFound = true;
            }

            if (errorFound)
            {
                gridCell.Appearance.Image = ErrorImage;
                gridCell.Tag = errorMessage;
                return false;
            }
            else
            {
                gridCell.Appearance.Image = null;
                gridCell.Tag = null;
                return true;
            }
        }


        private bool ValidSellThruPcts(ref EditMsgs em)
        {
            try
            {
                int lastPct = 0;
                UltraGridRow lastRow = null;
                string errorMessage = null;
                // sort in descending order by sell thru %
                this.ugSellThruPcts.DisplayLayout.Bands[0].SortedColumns.Clear();
                this.ugSellThruPcts.DisplayLayout.Bands[0].SortedColumns.Add("SellThruIndex", true);

                foreach (UltraGridRow gridRow in ugSellThruPcts.Rows)
                {
                    lastRow = gridRow;
                    if (!VelocitySellThruValid(gridRow.Cells["SellThruIndex"], ref errorMessage))
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, errorMessage, this.ToString());
                    }
                    else
                    {
                        lastPct = Convert.ToInt32(gridRow.Cells["SellThruIndex"].Value, CultureInfo.CurrentUICulture);
                    }
                }

                if (lastPct != 0)
                {
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_LastSellThruPctNotZero);
                    em.AddMsg(eMIDMessageLevel.Edit, errorMessage, this.ToString());
                    lastRow.Cells["SellThruIndex"].Appearance.Image = ErrorImage;
                    lastRow.Cells["SellThruIndex"].Tag = errorMessage;
                }

                if (em.ErrorFound)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                em.AddMsg(eMIDMessageLevel.Error, ex.Message, this.ToString());
                HandleException(ex);
            }

            return false;
        }
        private bool VelocitySellThruValid(UltraGridCell gridCell, ref string errorMessage)
        {
            bool errorFound = false;
            try
            {
                if (gridCell.Text.Length == 0)	// cell is empty
                {
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                    errorFound = true;
                }
                else
                {
                    int cellValue = Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture);
                    if (cellValue < 0)
                    {
                        errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
                        errorFound = true;
                    }
                    else
                    {
                        // make sure sell thrus are unique
                        foreach (UltraGridRow gridRow in this.ugSellThruPcts.Rows)
                        {
                            if (gridCell.Row != gridRow)	// Don't check yourself
                            {
                                if (gridRow.Cells["SellThruIndex"].Text.Trim().Length > 0)
                                {
                                    if (Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture) == Convert.ToInt32(gridRow.Cells["SellThruIndex"].Text, CultureInfo.CurrentUICulture))
                                    {
                                        errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeUnique);
                                        errorFound = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception error)
            {
                string exceptionMessage = error.Message;
                errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeInteger);
                errorFound = true;
            }

            if (errorFound)
            {
                gridCell.Appearance.Image = ErrorImage;
                gridCell.Tag = errorMessage;
                return false;
            }
            else
            {
                gridCell.Appearance.Image = null;
                gridCell.Tag = null;
                return true;
            }
        }
        #endregion Validate Velocity Grades Tab

        #region Validate Matrix Tab
        private bool ValidTabMatrix()
        {
            string errorMessage = string.Empty;
            bool errorFound = false;
            ErrorProvider.SetError(ugMatrix, string.Empty);
            ErrorProvider.SetError(txtOHQuantity, string.Empty);
            ErrorProvider.SetError(txtNoOHQuantity, string.Empty);
            try
            {
                if (!ValidQuantity(txtOHQuantity))
                    errorFound = true;
                if (!ValidQuantity(txtNoOHQuantity))
                    errorFound = true;
                if (!ValidMatrix())
                    errorFound = true;
                if (errorFound)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }
        private bool ValidQuantity(TextBox aTextBox)
        {
            double quantity;
            bool errorFound = false;
            string errorMessage = string.Empty;
            DataRow dr = null;
            try
            {
                switch (aTextBox.Name)
                {
                    case "txtOHQuantity":
                        dr = _defOnHandRules.Rows[cboOHRule.SelectedIndex];
                        break;
                    case "txtNoOHQuantity":
                        dr = _noOnHandRules.Rows[cboNoOHRule.SelectedIndex];
                        break;
                }
                eVelocityRuleRequiresQuantity vrq = (eVelocityRuleRequiresQuantity)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
                if (Enum.IsDefined(typeof(eVelocityRuleRequiresQuantity), vrq))
                {
                    if (aTextBox.Text.Trim().Length == 0)
                    {
                        errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                        ErrorProvider.SetError(aTextBox, errorMessage);
                        errorFound = true;
                    }
                    else
                    {
                        try
                        {
                            quantity = Convert.ToDouble(aTextBox.Text, CultureInfo.CurrentUICulture);

                            switch (vrq)
                            {
                                case eVelocityRuleRequiresQuantity.WeeksOfSupply:
                                    if (quantity < 0)
                                    {
                                        errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
                                        ErrorProvider.SetError(aTextBox, errorMessage);
                                        errorFound = true;
                                    }
                                    else if (quantity > Include.MaxWeeksOfSupply)
                                    {
                                        errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MaximumValueExceeded),
                                            quantity.ToString(CultureInfo.CurrentUICulture), Include.MaxWeeksOfSupply.ToString(CultureInfo.CurrentUICulture));
                                        ErrorProvider.SetError(aTextBox, errorMessage);
                                        errorFound = true;
                                    }
                                    else
                                        aTextBox.Text = Convert.ToString(Math.Round(quantity, 1), CultureInfo.CurrentUICulture);
                                    break;
                                case eVelocityRuleRequiresQuantity.ShipUpToQty:
                                    if (quantity < 1)
                                    {
                                        errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
                                        ErrorProvider.SetError(aTextBox, errorMessage);
                                        errorFound = true;
                                    }
                                    else if (quantity > Include.MaxShipUpToQty)
                                    {
                                        errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MaximumValueExceeded),
                                                                    quantity.ToString(CultureInfo.CurrentUICulture), Include.MaxShipUpToQty.ToString(CultureInfo.CurrentUICulture));
                                        ErrorProvider.SetError(aTextBox, errorMessage);
                                        errorFound = true;
                                    }
                                    else
                                        aTextBox.Text = Convert.ToString(Math.Round(quantity, 0), CultureInfo.CurrentUICulture);
                                    break;
                                case eVelocityRuleRequiresQuantity.AbsoluteQuantity:
                                    if (quantity < 1)
                                    {
                                        errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
                                        ErrorProvider.SetError(aTextBox, errorMessage);
                                        errorFound = true;
                                    }
                                    else
                                        aTextBox.Text = Convert.ToString(Math.Round(quantity, 0), CultureInfo.CurrentUICulture);
                                    break;
                            }
                        }
                        catch
                        {
                            errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
                            ErrorProvider.SetError(aTextBox, errorMessage);
                            errorFound = true;
                        }
                    }
                }
                if (errorFound)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }
        private bool ValidMatrix()
        {
            bool errorFound = false;
            try
            {
                foreach (UltraGridRow gridRow in ugMatrix.Rows)
                {
                    foreach (UltraGridCell cell in gridRow.Cells)
                    {
                        if (cell.Column.Tag != null && cell.Column.Tag.ToString() == "Rule"
                            && cell.Text.TrimEnd() != string.Empty)
                        {
                            if (!ValidMatrixQuantity(cell))
                                errorFound = true;
                        }
                    }
                }
                if (errorFound)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }
        private bool ValidMatrixQuantity(UltraGridCell gridCell)
        {
            bool errorFound = false;
            string errorMessage = string.Empty;
            double quantity;
            try
            {
                int i = gridCell.Column.Index;
                eVelocityRuleRequiresQuantity vrq = (eVelocityRuleRequiresQuantity)Convert.ToInt32(gridCell.Value, CultureInfo.CurrentUICulture);
                if (Enum.IsDefined(typeof(eVelocityRuleRequiresQuantity), vrq))
                {
                    if (gridCell.Row.Cells[i + 1].Text.Trim() == string.Empty)
                    {
                        errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                        errorFound = true;
                    }
                    else
                    {
                        quantity = Convert.ToDouble(gridCell.Row.Cells[i + 1].Text, CultureInfo.CurrentUICulture);
                        switch (vrq)
                        {
                            case eVelocityRuleRequiresQuantity.WeeksOfSupply:
                                if (quantity < 0)
                                {
                                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
                                    errorFound = true;
                                }
                                else if (quantity > Include.MaxWeeksOfSupply)
                                {
                                    errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MaximumValueExceeded),
                                        quantity.ToString(CultureInfo.CurrentUICulture), Include.MaxWeeksOfSupply.ToString(CultureInfo.CurrentUICulture));
                                    errorFound = true;
                                }
                                break;
                            case eVelocityRuleRequiresQuantity.ShipUpToQty:
                                if (quantity < 1)
                                {
                                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
                                    errorFound = true;
                                }
                                else if (quantity > Include.MaxShipUpToQty)
                                {
                                    errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MaximumValueExceeded),
                                        quantity.ToString(CultureInfo.CurrentUICulture), Include.MaxShipUpToQty.ToString(CultureInfo.CurrentUICulture));
                                    errorFound = true;
                                }
                                break;
                            case eVelocityRuleRequiresQuantity.AbsoluteQuantity:
                                if (quantity < 1)
                                {
                                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
                                    errorFound = true;
                                }
                                break;
                        }
                    }
                }
                if (errorFound)
                {
                    gridCell.Row.Cells[i + 1].Appearance.Image = ErrorImage;
                    gridCell.Row.Cells[i + 1].Tag = errorMessage;
                    return false;
                }
                else
                {
                    gridCell.Row.Cells[i + 1].Appearance.Image = null;
                    gridCell.Row.Cells[i + 1].Tag = null;
                    return true;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }
        #endregion Validate Matrix Tab
        private void txtOHQuantity_TextChanged(object sender, System.EventArgs e)
        {
            MatrixChangesMade = true;
        }

        //		private void midDateRangeSelectorBeg_OnSelection(object source, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
        //		{
        //			try
        //			{
        //				DateRangeProfile dr = e.SelectedDateRange;
        //				if (dr != null)
        //				{
        //					LoadDateRangeText(dr, midDateRangeSelectorBeg, strBegin);
        //					ChangePending = true;
        //				}
        //			}
        //			catch(Exception ex)
        //			{
        //				HandleException(ex);
        //			}
        //		}
        private void LoadDateRangeText(DateRangeProfile dr, Controls.MIDDateRangeSelector midDRSel, string whichSel)
        {
            try
            {
                if (dr.DisplayDate != null)
                {
                    midDRSel.Text = dr.DisplayDate;
                }
                else
                {
                    midDRSel.Text = string.Empty;
                }

                //Add RID to Control's Tag (for later use)
                int lAddTag = dr.Key;

                midDRSel.Tag = lAddTag;
                midDRSel.DateRangeRID = lAddTag;

                switch (whichSel)
                {
                    case strBegin:
                        //Display Dynamic picture or not
                        if (dr.DateRangeType == eCalendarRangeType.Dynamic)
                            midDRSel.SetImage(this.DynamicToCurrentImage);
                        else
                            midDRSel.SetImage(null);
                        break;
                    case strShip:
                        //Display Dynamic picture or not
                        if (dr.DateRangeType == eCalendarRangeType.Dynamic)
                            midDRSel.SetImage(this.DynamicToCurrentImage);
                        else
                            midDRSel.SetImage(null);
                        break;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        //		private void midDateRangeSelectorBeg_Load(object sender, System.EventArgs e)
        //		{
        //			try
        //			{
        //				CalendarDateSelector frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{SAB});
        //				midDateRangeSelectorBeg.DateRangeForm = frm;
        //				frm.RestrictToSingleDate = true;
        //			}
        //			catch(Exception ex)
        //			{
        //				HandleException(ex);
        //			}
        //		}

        //		private void midDateRangeSelectorShip_Load(object sender, System.EventArgs e)
        //		{
        //			try
        //			{
        //				CalendarDateSelector frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{SAB});
        //				midDateRangeSelectorShip.DateRangeForm = frm;
        //				frm.RestrictToSingleDate = true;
        //			}
        //			catch(Exception ex)
        //			{
        //				HandleException(ex);
        //			}
        //		}
        //
        //		private void midDateRangeSelectorShip_OnSelection(object source, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
        //		{
        //			try
        //			{
        //				DateRangeProfile dr = e.SelectedDateRange;
        //				if (dr != null)
        //				{
        //					LoadDateRangeText(dr, midDateRangeSelectorShip, strShip);
        //					ChangePending = true;
        //				}
        //			}
        //			catch(Exception ex)
        //			{
        //				HandleException(ex);
        //			}
        //		}

        private void ugBasisNodeVersion_DragEnter(object sender, DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void ugBasisNodeVersion_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            try
            {
                Image_DragOver(sender, e);
                Infragistics.Win.UIElement aUIElement;
                aUIElement = ugBasisNodeVersion.DisplayLayout.UIElement.ElementFromPoint(ugBasisNodeVersion.PointToClient(new Point(e.X, e.Y)));

                if (aUIElement == null)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                UltraGridRow aRow;
                aRow = (UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow));

                if (aRow == null)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                UltraGridCell aCell = (UltraGridCell)aUIElement.GetContext(typeof(UltraGridCell));
                if (aCell == null)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                if (aCell == aRow.Cells["Merchandise"])
                {
                    // Begin TT#325 - JSmith - drag/drop method/workflow into merch field
                    //e.Effect = DragDropEffects.All;
                    e.Effect = DragDropEffects.None;
                    if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                    {
                        TreeNodeClipboardList cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                        if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
                        {
                            e.Effect = DragDropEffects.All;
                        }
                    }
                    // End TT#325 - JSmith - drag/drop method/workflow into merch field
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void ugBasisNodeVersion_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            HierarchyNodeProfile hnp;
            TreeNodeClipboardList cbList = null;
            try
            {
                Infragistics.Win.UIElement aUIElement;

                aUIElement = ugBasisNodeVersion.DisplayLayout.UIElement.ElementFromPoint(ugBasisNodeVersion.PointToClient(new Point(e.X, e.Y)));

                if (aUIElement == null)
                {
                    return;
                }

                UltraGridRow aRow;
                aRow = (UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow));

                if (aRow == null)
                {
                    return;
                }

                UltraGridCell aCell = (UltraGridCell)aUIElement.GetContext(typeof(UltraGridCell));
                if (aCell == null)
                {
                    return;
                }
                if (aCell == aRow.Cells["Merchandise"])
                {
                    try
                    {
                        //// Create a new instance of the DataObject interface.
                        //IDataObject data = Clipboard.GetDataObject();

                        ////If the data is ClipboardProfile, then retrieve the data
                        //ClipboardProfile cbp;
                        //HierarchyClipboardData MIDTreeNode_cbd;
                        object cellValue = null;
                        if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                        {
                            cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                            //if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode)
                            if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
                            {
                                //if (cbp.ClipboardData.GetType() == typeof(HierarchyClipboardData))
                                //{
                                //MIDTreeNode_cbd = (HierarchyClipboardData)cbp.ClipboardData;

                                //Begin Track #5378 - color and size not qualified
                                //									hnp = SAB.HierarchyServerSession.GetNodeData(cbp.Key);
                                hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);
                                //End Track #5378

                                aRow.Cells["BasisHNRID"].Value = hnp.Key;
                                aRow.Cells["Merchandise"].Appearance.Image = null;
                                aRow.Cells["Merchandise"].Tag = null;
                                // Issue stodd 10.4.2007
                                // Used to tell if user nixed changing the merchandise node. 
                                if (_changedByCode)
                                {
                                    _changedByCode = false;
                                }
                                else
                                {
                                    aRow.Cells["BasisPHRID"].Value = Include.NoRID;
                                    aRow.Cells["BasisPHLSequence"].Value = 0;
                                    AddNodeToMerchandiseCombo2(hnp);
                                    if (!_basisNodeInList)
                                    {
                                        Infragistics.Win.ValueListItem vli = new Infragistics.Win.ValueListItem();
                                        vli.DataValue = ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Count;
                                        vli.DisplayText = hnp.Text;
                                        vli.Tag = Convert.ToString(hnp.Key, CultureInfo.CurrentUICulture);
                                        ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Add(vli);
                                        cellValue = vli.DataValue;
                                    }
                                    else
                                    {
                                        foreach (Infragistics.Win.ValueListItem vli in ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems)
                                        {
                                            if (vli.DisplayText == hnp.Text)
                                            {
                                                cellValue = vli.DataValue;
                                                break;
                                            }
                                        }
                                    }
                                    _skipAfterCellUpdate = true;
                                    aCell.Value = cellValue;
                                    _skipAfterCellUpdate = false;
                                    _changedByCode = false;
                                    ugBasisNodeVersion.UpdateData();
                                }
                                //}
                                //else
                                //{
                                //    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard));
                                //}
                            }
                            else
                            {
                                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
                            }
                        }
                        else
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
                        }
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        // BEGIN Issue 4391/4393 stodd 10.4.2007
        private void ugBasisNodeVersion_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
            //ugld.ApplyDefaults(e);
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
            //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 3, true);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 3, false);
            // End TT#1164
            //End TT#169

            ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisHNRID"].Header.Caption = "Merchandise";
        }
        // END Issue 4391/4393 stodd 10.4.2007

        private void ugBasisNodeVersion_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
        {
            try
            {
                ShowUltraGridToolTip(ugBasisNodeVersion, e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugBasisNodeVersion_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                if (FunctionSecurity.AllowUpdate)
                {
                    CalendarDateSelector frmCalDtSelector = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[] { SAB });
                    frmCalDtSelector.AllowReoccurring = false;
                    frmCalDtSelector.AllowDynamic = true;
                    frmCalDtSelector.AllowDynamicToStoreOpen = false;
                    frmCalDtSelector.AllowDynamicToPlan = false;

                    if (e.Cell.Row.Cells["DateRange"].Value != null &&
                        e.Cell.Row.Cells["DateRange"].Value != System.DBNull.Value &&
                        e.Cell.Row.Cells["DateRange"].Text.Length > 0)
                    {
                        frmCalDtSelector.DateRangeRID = Convert.ToInt32(e.Cell.Row.Cells["cdrRid"].Value, CultureInfo.CurrentUICulture);
                    }


                    frmCalDtSelector.AnchorDate = SAB.ClientServerSession.Calendar.CurrentDate;

                    DialogResult DateRangeResult = frmCalDtSelector.ShowDialog();
                    if (DateRangeResult == DialogResult.OK)
                    {
                        DateRangeProfile SelectedDateRange = (DateRangeProfile)frmCalDtSelector.Tag;
                        e.Cell.Value = SelectedDateRange.DisplayDate;
                        if (!_changedByCode)  // Issue 4393 stodd 10.17.2007
                        {
                            //				e.Cell.Tag = SelectedDateRange;
                            // for some reason have to clear the cell before it can be updated??
                            if (e.Cell.Row.Cells["cdrRid"].Value != System.DBNull.Value)
                            {
                                e.Cell.Row.Cells["cdrRid"].Value = System.DBNull.Value;
                            }
                            e.Cell.Row.Cells["cdrRid"].Value = SelectedDateRange.Key;
                            if (SelectedDateRange.DateRangeType == eCalendarRangeType.Dynamic)
                            {
                                if (SelectedDateRange.RelativeTo == eDateRangeRelativeTo.Plan)
                                    e.Cell.Appearance.Image = DynamicToPlanImage;
                                else
                                    e.Cell.Appearance.Image = DynamicToCurrentImage;
                            }
                            else
                            {
                                e.Cell.Appearance.Image = null;
                            }
                        }
                        else
                        {
                            ChangePending = true;
                            btnSave.Enabled = true;
                        }
                        _changedByCode = false;

                    }
                    //					frmCalDtSelector.Remove();
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        // BEGIN Issue 4818
        //		private void ugBasisSalesPeriod_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
        //		{
        //			try
        //			{
        //				ShowUltraGridToolTip(ugBasisSalesPeriod, e);
        //			}
        //			catch(Exception ex)
        //			{
        //				HandleException(ex);
        //			}
        //		}
        // END Issue 4818

        private void ugVelocityGrades_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
        {
            try
            {
                ShowUltraGridToolTip(ugVelocityGrades, e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugSellThruPcts_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
        {
            try
            {
                ShowUltraGridToolTip(ugSellThruPcts, e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugMatrix_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
        {
            try
            {
                ShowUltraGridToolTip(ugMatrix, e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        // BEGIN Issue 4818
        //		private void ugBasisSalesPeriod_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        //		{
        //			try
        //			{
        //				//Get the GUI element where the mouse cursor is. (so that later on
        //				//we can retrieve the row and the cell based on the mouse location.)
        //				Infragistics.Win.UIElement aUIElement;
        //				Point pt = new Point(e.X, e.Y);
        //				aUIElement = ugBasisSalesPeriod.DisplayLayout.UIElement.ElementFromPoint(pt);
        //
        //				if (aUIElement == null) 
        //				{					
        //					return;
        //				}
        //
        //				//Retrieve the row where the mouse is.
        //				UltraGridRow aRow;
        //				aRow = (UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow)); 
        //
        //				if (aRow == null) 
        //				{
        //					return;
        //				}
        //
        //				//if ((ugBaslsSalesPeriod.DisplayLayout.Bands.Count == 2 && aRow.Band.Index == 1)
        //				//	|| (aGrid.DisplayLayout.Bands.Count == 1 && aRow.Band.Index == 0))
        //				//{
        //					//Retrieve the cell where the mouse is.
        //					UltraGridCell aCell = (UltraGridCell)aUIElement.GetContext(typeof(UltraGridCell)); 
        //					if (aCell == null) 
        //					{
        //						return;
        //					}
        //
        ////					//If the cell is the "DateRange" cell or the include/exclude button cell,
        ////					//put a DateRangeSelector or a button in it.
        ////					if (aCell == aRow.Cells["DateRange"])
        ////					{
        ////						 _dateRow = aRow; //m_aRow is a form-level variable and the DateRangeSelector's click event needs it.
        ////
        ////						//We need to get the size and location of the cell, and we can only
        ////						//get that by retrieving the UIElement associated with that cell.
        ////						CellUIElement objCellUIElement = (CellUIElement)aCell.GetUIElement(ugBasisSalesPeriod.ActiveRowScrollRegion, ugBasisSalesPeriod.ActiveColScrollRegion);
        ////						if ( objCellUIElement == null ) { return; }
        ////
        ////						//   Get the size and location of the cell
        ////						int left = objCellUIElement.RectInsideBorders.Location.X + ugBasisSalesPeriod.Location.X;
        ////						int top = objCellUIElement.RectInsideBorders.Location.Y + ugBasisSalesPeriod.Location.Y;
        ////						int width = objCellUIElement.RectInsideBorders.Width;
        ////						int height = objCellUIElement.RectInsideBorders.Height;
        ////   						 
        ////						//   Set the DateRangeSelector's size and location equal to the cell's size and location
        ////						dateRangeSelectorSales.SetBounds(left, top, width, height);
        ////      
        ////						//   Show the combobox control over the cell, and give it focus
        ////						dateRangeSelectorSales.Visible = true;
        ////						dateRangeSelectorSales.Focus();
        ////						dateRangeSelectorSales.BringToFront();
        ////						dateRangeSelectorSales.Text = aCell.Value.ToString();
        ////						if (_errorCells != null && _errorCells.Contains(aCell) )
        ////						{
        ////							string msg = aCell.Tag.ToString();
        ////							dateRangeSelectorSales.SetImage(ErrorImage);
        ////							this.toolTip1.Active = true;
        ////							//this.toolTip1.SetToolTip(dateRangeSelectorSales,msg);
        ////							
        ////						}
        ////						else
        ////						{			
        ////							dateRangeSelectorSales.SetImage(null);	
        ////							this.toolTip1.Active = false;
        ////						} 
        ////					}
        //				//}
        //			}
        //			catch (Exception ex)
        //			{
        //				HandleException(ex);
        //			}
        //		}
        // END Issue 4818

        private void ugBasisNodeVersion_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            bool errorFound = false;
            int rowseq = -1;
            string errorMessage = string.Empty, productID;
            try
            {
                switch (e.Cell.Column.Key)
                {
                    case "Merchandise":

                        if (_skipAfterCellUpdate) return;
                        if (e.Cell.Value.ToString().Trim().Length == 0)
                            return;

                        try
                        {
                            foreach (Infragistics.Win.ValueListItem vli in ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems)
                            {
                                if (Convert.ToInt32(vli.DataValue, CultureInfo.CurrentUICulture) == Convert.ToInt32(e.Cell.Value, CultureInfo.CurrentUICulture))
                                {
                                    rowseq = Convert.ToInt32(vli.DataValue, CultureInfo.CurrentUICulture);
                                    break;
                                }
                            }
                        }
                        // catch if value is not integer so that it can be check to determine if it is a product
                        catch (System.FormatException)
                        {
                            rowseq = -1;
                        }

                        if (rowseq != -1)
                        {
                            DataRow row = _merchDataTable2.Rows[rowseq];
                            if (row != null)
                            {
                                eMerchandiseType MerchandiseType = (eMerchandiseType)(Convert.ToInt32(row["leveltypename"], CultureInfo.CurrentUICulture));

                                switch (MerchandiseType)
                                {
                                    case eMerchandiseType.Node:
                                        e.Cell.Row.Cells["BasisHNRID"].Value = Convert.ToInt32(row["key"], CultureInfo.CurrentUICulture);
                                        e.Cell.Row.Cells["BasisPHRID"].Value = Include.NoRID;
                                        e.Cell.Row.Cells["BasisPHLSequence"].Value = 0;
                                        break;
                                    case eMerchandiseType.HierarchyLevel:
                                        e.Cell.Row.Cells["BasisPHRID"].Value = HP.Key;
                                        e.Cell.Row.Cells["BasisPHLSequence"].Value = Convert.ToInt32(row["key"], CultureInfo.CurrentUICulture);
                                        e.Cell.Row.Cells["BasisHNRID"].Value = Include.NoRID;
                                        break;
                                    case eMerchandiseType.OTSPlanLevel:
                                        e.Cell.Row.Cells["BasisPHRID"].Value = Include.NoRID;
                                        e.Cell.Row.Cells["BasisHNRID"].Value = Include.NoRID;
                                        e.Cell.Row.Cells["BasisPHLSequence"].Value = 0;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            object cellValue = null;
                            productID = e.Cell.Value.ToString().Trim();
                            HierarchyNodeProfile hnp = GetNodeProfile(productID);
                            if (hnp.Key == Include.NoRID)
                            {
                                errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
                                    productID);
                                errorFound = true;
                            }
                            else
                            {
                                //								HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(key);

                                e.Cell.Row.Cells["BasisHNRID"].Value = hnp.Key;
                                e.Cell.Row.Cells["BasisPHRID"].Value = Include.NoRID;
                                e.Cell.Row.Cells["BasisPHLSequence"].Value = 0;
                                AddNodeToMerchandiseCombo2(hnp);
                                if (!_basisNodeInList)
                                {
                                    Infragistics.Win.ValueListItem vli = new Infragistics.Win.ValueListItem();
                                    vli.DataValue = ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Count;
                                    vli.DisplayText = hnp.Text;
                                    vli.Tag = Convert.ToString(hnp.Key, CultureInfo.CurrentUICulture);
                                    ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Add(vli);
                                    cellValue = vli.DataValue;
                                }
                                else
                                {
                                    foreach (Infragistics.Win.ValueListItem vli in ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems)
                                    {
                                        if (vli.DisplayText == hnp.Text)
                                        {
                                            cellValue = vli.DataValue;
                                            break;
                                        }
                                    }
                                }
                                _skipAfterCellUpdate = true;
                                e.Cell.Value = cellValue;
                                _skipAfterCellUpdate = false;
                                ugBasisNodeVersion.UpdateData();
                            }
                        }
                        break;
                }
                if (!_changedByCode)
                    BasisChangesMade = true;
                if (errorFound)
                {
                    e.Cell.Appearance.Image = ErrorImage;
                    e.Cell.Tag = errorMessage;
                }
                else
                {
                    e.Cell.Appearance.Image = null;
                    e.Cell.Tag = null;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugBasisNodeVersion_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            if (_basisIsPopulated)
            {
                BasisChangesMade = true;
            }
        }

        // BEGIN Issue 4818
        //		private void ugBasisSalesPeriod_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        //		{	
        //			if (_salesPeriodIsPopulated)
        //			{
        //				BasisChangesMade = true;
        //			}	
        //		}
        // END Issue 4818

        private void ugBasisNodeVersion_AfterRowsDeleted(object sender, System.EventArgs e)
        {
            BasisChangesMade = true;
        }


        //		// BEGIN Issue 4393 stodd 10.3.2007
        //		private void ugBasisSalesPeriod_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
        //		{
        //			if (_matrixProcessed && !_changedByCode)
        //			{
        //				if (!ReprocessWarningOK("the Basis"))
        //				{
        //					//_returnToCell = e.Cell;
        //					e.Cancel = true;
        //				}
        //			}
        //			_changedByCode = false;
        //		}
        //		// END Issue 4393 

        // BEGIN Issue 4818
        //		private void ugBasisSalesPeriod_AfterRowsDeleted(object sender, System.EventArgs e)
        //		{
        //			BasisChangesMade = true;
        //		}

        //		private void ugBasisSalesPeriod_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        //		{
        //			if (_salesPeriodIsPopulated)
        //			{
        //				BasisChangesMade = true;
        //			}
        //		}
        //
        //		private void ugBasisSalesPeriod_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        //		{
        //			try
        //			{
        //				if (FunctionSecurity.AllowUpdate)
        //				{
        //					CalendarDateSelector frmCalDtSelector = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{SAB});
        //					frmCalDtSelector.AllowReoccurring = false;
        //					frmCalDtSelector.AllowDynamic = true;
        //					frmCalDtSelector.AllowDynamicToStoreOpen = false;
        //					frmCalDtSelector.AllowDynamicToPlan= false;
        //
        //					if (e.Cell.Row.Cells["DateRange"].Value != null &&
        //						e.Cell.Row.Cells["DateRange"].Value != System.DBNull.Value &&
        //						e.Cell.Row.Cells["DateRange"].Text.Length > 0)
        //					{
        //						frmCalDtSelector.DateRangeRID = Convert.ToInt32(e.Cell.Row.Cells["cdrRid"].Value, CultureInfo.CurrentUICulture);
        //					}
        //
        //					
        //					frmCalDtSelector.AnchorDate = SAB.ClientServerSession.Calendar.CurrentDate;
        //					
        //					DialogResult DateRangeResult = frmCalDtSelector.ShowDialog();
        //					if (DateRangeResult == DialogResult.OK)
        //					{
        //						
        //						DateRangeProfile SelectedDateRange = (DateRangeProfile)frmCalDtSelector.Tag;
        //						e.Cell.Value = SelectedDateRange.DisplayDate;
        //						if (!_changedByCode)  // Issue 4393 stodd 10.17.2007
        //						{
        //							//				e.Cell.Tag = SelectedDateRange;
        //							// for some reason have to clear the cell before it can be updated??
        //							if (e.Cell.Row.Cells["cdrRid"].Value != System.DBNull.Value)
        //							{
        //								e.Cell.Row.Cells["cdrRid"].Value = System.DBNull.Value;
        //							}
        //							e.Cell.Row.Cells["cdrRid"].Value = SelectedDateRange.Key;
        //							if (SelectedDateRange.DateRangeType == eCalendarRangeType.Dynamic)
        //							{
        //								if (SelectedDateRange.RelativeTo == eDateRangeRelativeTo.Plan)
        //									e.Cell.Appearance.Image = DynamicToPlanImage;
        //								else
        //									e.Cell.Appearance.Image = DynamicToCurrentImage;
        //							}
        //							else
        //							{
        //								e.Cell.Appearance.Image = null;
        //							}
        //						}
        //						else
        //						{
        //							ChangePending = true;
        //							btnSave.Enabled = true;
        //						}
        //						_changedByCode = false;
        //						
        //					}
        ////					frmCalDtSelector.Remove();
        //				}
        //			}
        //			catch(Exception exception)
        //			{
        //				HandleException(exception);
        //			}
        //		}
        // END Issue 4818

        private void ugVelocityGrades_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            try
            {
                
                // Begin TT#325 - JSmith - drag/drop method/workflow into merch field
                //ObjectDragEnter(e);
                Image_DragEnter(sender, e);
                e.Effect = DragDropEffects.None;
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    TreeNodeClipboardList cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
                    {
                        e.Effect = DragDropEffects.All;
                    }
                }
                // End TT#325 - JSmith - drag/drop method/workflow into merch field
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        // Begin TT#325 - JSmith - drag/drop method/workflow into merch field
        void ugVelocityGrades_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }
        // End TT#325 - JSmith - drag/drop method/workflow into merch field

        private void ugVelocityGrades_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            TreeNodeClipboardList cbList = null;
            try
            {
                //ClipboardProfile cbp = GetClipboardData(eClipboardDataType.HierarchyNode);
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    if (FunctionSecurity.IsReadOnly)
                    {
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));

                    }
                    else
                    {
                        ResetMatrix();  // MID Track #6471 - Foreign Key violation after drag/drop then save
                        VelocityGrades_Populate(cbList.ClipboardProfile.Key);
                        VelocityGradesChangesMade = true;
                    }
                }
            }
            catch (BadDataInClipboardException)
            {
                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugSellThruPcts_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            try
            {
                // Begin TT#325 - JSmith - drag/drop method/workflow into merch field
                //ObjectDragEnter(e);
                Image_DragEnter(sender, e);
                e.Effect = DragDropEffects.None;
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    TreeNodeClipboardList cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
                    {
                        e.Effect = DragDropEffects.All;
                    }
                }
                // End TT#325 - JSmith - drag/drop method/workflow into merch field
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        // Begin TT#325 - JSmith - drag/drop method/workflow into merch field
        void ugSellThruPcts_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }
        // End TT#325 - JSmith - drag/drop method/workflow into merch field

        private void ugSellThruPcts_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            TreeNodeClipboardList cbList;
            HierarchyNodeClipboardList hnList;
            try
            {
                //ClipboardProfile cbp = GetClipboardData(eClipboardDataType.HierarchyNode);
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
                    {
                        if (FunctionSecurity.IsReadOnly)
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));

                        }
                        else
                        {
                            SellThruPcts_Populate(cbList.ClipboardProfile.Key);
                            SellThruPctsChangesMade = true;
                        }
                    }
                }
                else if (e.Data.GetDataPresent(typeof(HierarchyNodeClipboardList)))
                {
                    hnList = (HierarchyNodeClipboardList)e.Data.GetData(typeof(HierarchyNodeClipboardList));
                    if (FunctionSecurity.IsReadOnly)
                    {
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));

                    }
                    else
                    {
                        SellThruPcts_Populate(hnList.ClipboardProfile.Key);
                        SellThruPctsChangesMade = true;
                    }
                }
            }
            catch (BadDataInClipboardException)
            {
                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugVelocityGrades_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
        {
            VelocityGradesChangesMade = true;
            _rebuildMatrix = true;

            // Begin TT#5244 - JSmith - Velocity Method throws Severe error message if Grades are deleted and then added back
            if (FormLoaded)
            {
                CheckInteractive();
            }
            // End TT#5244 - JSmith - Velocity Method throws Severe error message if Grades are deleted and then added back
        }

        private void ugSellThruPcts_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
        {
            VelocityGradesChangesMade = true;
            _rebuildMatrix = true;

            // Begin TT#5244 - JSmith - Velocity Method throws Severe error message if Grades are deleted and then added back
            if (FormLoaded)
            {
                CheckInteractive();
            }
            // End TT#5244 - JSmith - Velocity Method throws Severe error message if Grades are deleted and then added back
        }

        private void ugVelocityGrades_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            // BEGIN TT#987 - Velocity Min/Max changes not re-setting matirx data
            //_rebuildMatrix = true;
            // BEGIN TT#987 - Velocity Min/Max changes not re-setting matirx data
            // sort in descending order by boundary
            this.ugVelocityGrades.DisplayLayout.Bands[0].SortedColumns.Clear();
            this.ugVelocityGrades.DisplayLayout.Bands[0].SortedColumns.Add("Boundary", true);
        }

        private void ugVelocityGrades_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
        {
            if (!MatrixWarningOK(e.Rows[0].Cells["Boundary"].Column.Header.Caption))
                e.Cancel = true;
        }


        private void ugVelocityGrades_AfterRowsDeleted(object sender, System.EventArgs e)
        {
            _dsVelocity.Tables["VelocityGrade"].AcceptChanges();
            VelocityGradesChangesMade = true;
            _rebuildMatrix = true;

            // Begin TT#5244 - JSmith - Velocity Method throws Severe error message if Grades are deleted and then added back
            if (FormLoaded)
            {
                CheckInteractive();
            }
            // End TT#5244 - JSmith - Velocity Method throws Severe error message if Grades are deleted and then added back
        }

        private void ugSellThruPcts_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
        {
            if (!MatrixWarningOK(e.Rows[0].Cells["SellThruIndex"].Column.Header.Caption))
                e.Cancel = true;
        }

        private void ugSellThruPcts_AfterRowsDeleted(object sender, System.EventArgs e)
        {
            _dsVelocity.Tables["SellThru"].AcceptChanges();
            VelocityGradesChangesMade = true;
            _rebuildMatrix = true;

            // Begin TT#5244 - JSmith - Velocity Method throws Severe error message if Grades are deleted and then added back
            if (FormLoaded)
            {
                CheckInteractive();
            }
            // End TT#5244 - JSmith - Velocity Method throws Severe error message if Grades are deleted and then added back
        }

        private void ugSellThruPcts_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            if (e.Cell.Column.Key == "SellThruIndex")
            {
                _rebuildMatrix = true;
                // sort in descending order by sell thru %
                this.ugSellThruPcts.DisplayLayout.Bands[0].SortedColumns.Clear();
                this.ugSellThruPcts.DisplayLayout.Bands[0].SortedColumns.Add("SellThruIndex", true);
                _sellThruPctsDataTable.DefaultView.Sort = "SellThruIndex DESC";
                _setRowPosition = true;
            }
        }

        private void ugSellThruPcts_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            if (_sellThruPctsIsPopulated)
            {
                VelocityGradesChangesMade = true;
            }
        }

        private void ugVelocityGrades_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            if (_velocityGradesIsPopulated)
            {
                VelocityGradesChangesMade = true;
                // BEGIN TT#987 - Velocity Min/Max changes not re-setting matirx data
                ChangePending = true;
                if (_matrixProcessed)
                {
                    if (cbxInteractive.Checked)
                    {
                        cbxInteractive.Checked = false;
                        cbxInteractive.Checked = true;
                    }
                }
                // END TT#987 - Velocity Min/Max changes not re-setting matirx data
            }

        }

        private void ugMatrix_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                int setValue, boundary, pst;
                double qty;
                string colKey;
                if (_matrixProcessed)
                {
                    if (e.Cell.Column.Tag != null)
                    {
                        setValue = Convert.ToInt32(this.cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
                        boundary = Convert.ToInt32(e.Cell.Row.Cells["Boundary"].Value, CultureInfo.CurrentUICulture);
                        colKey = Convert.ToString(e.Cell.Column.Key, CultureInfo.CurrentUICulture);
                        switch (e.Cell.Column.Tag.ToString())
                        {
                            case "Rule":
                                pst = Convert.ToInt32(colKey.Substring(4), CultureInfo.CurrentUICulture);
                                eVelocityRuleType ruleType;
                                if (e.Cell.Value == System.DBNull.Value)
                                {
                                    ruleType = eVelocityRuleType.None;
                                }
                                else
                                {
                                    ruleType = (eVelocityRuleType)e.Cell.Value;
                                }
                                _trans.VelocitySetMatrixCellType(setValue, boundary, pst, ruleType);
                                break;

                            case "Qty":
                                pst = Convert.ToInt32(colKey.Substring(3), CultureInfo.CurrentUICulture);
                                if (e.Cell.Value == System.DBNull.Value)
                                    qty = 0;
                                else
                                    qty = Convert.ToDouble(e.Cell.Value, CultureInfo.CurrentUICulture);
                                _trans.VelocitySetMatrixCellQty(setValue, boundary, pst, qty);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugMatrix_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            if (_matrixIsPopulated)
                MatrixChangesMade = true;
            if (_matrixProcessed)
                btnChanges.Enabled = true;
        }

        // Begin MID Track 4858 - JSmith - Security changes
        //		private void btnProcess_Click(object sender, System.EventArgs e)
        //		{
        //			try
        //			{
        //				ProcessAction(eMethodType.Velocity);
        //				// as part of the  processing we saved the info, so it should be changed to update.
        //				if (!ErrorFound)
        //				{
        //					_velocityMethod.Method_Change_Type = eChangeType.update;
        //					btnSave.Text = "&Update";
        //				}
        //			}
        //			catch(Exception ex)
        //			{
        //				HandleException(ex);
        //			}
        //		}

        protected override void Call_btnProcess_Click()
        {
            try
            {
				// BEGIN TT#696-MD - Stodd - add "active process"
				//====================================================
				// Checks to be sure there are valid selected headers
				//====================================================
				if (!OkToProcess(this, eMethodType.Velocity))
				{
					return;
				}
				// END TT#696-MD - Stodd - add "active process"

                ProcessAction(eMethodType.Velocity);
                // as part of the  processing we saved the info, so it should be changed to update.
                if (!ErrorFound)
                {
                    _velocityMethod.Method_Change_Type = eChangeType.update;
                    btnSave.Text = "&Update";
                    //Begin TT#299 - stodd - null ref changing attribute set
                    _afterProcess = true;
                    //Begin TT#299 - stodd - null ref changing attribute set
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // End MID Track 4858

        private void ugMatrix_AfterCellListCloseUp(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                int i = e.Cell.Column.Index;
                int sel = e.Cell.Column.ValueList.SelectedItemIndex;

                if (sel == -1)
                {
                    e.Cell.Row.Cells[i + 1].Value = System.DBNull.Value;
                    e.Cell.Row.Cells[i + 1].Activation = Activation.Disabled;
                    return;
                }

                DataRow dr = _defOnHandRules.Rows[sel];
                eVelocityRuleRequiresQuantity vrq = (eVelocityRuleRequiresQuantity)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
                if ((eVelocityRuleType)vrq == eVelocityRuleType.None)
                {
                    e.Cell.Column.ValueList.SelectedItemIndex = -1;
                    e.Cell.Row.Cells[i + 1].Appearance.Image = null;
                    e.Cell.Row.Cells[i + 1].Value = System.DBNull.Value;
                    e.Cell.Row.Cells[i + 1].Activation = Activation.Disabled;
                    return;
                }

                if (Enum.IsDefined(typeof(eVelocityRuleRequiresQuantity), vrq))
                {
                    e.Cell.Row.Cells[i + 1].Activation = Activation.AllowEdit;
                    switch (vrq)
                    {
                        case eVelocityRuleRequiresQuantity.WeeksOfSupply:
                        case eVelocityRuleRequiresQuantity.ShipUpToQty:
                            //e.Cell.Row.Cells[i+1].= 4;
                            break;
                        default:
                            //txtOHQuantity.MaxLength = 6;
                            break;
                    }
                    if (e.Cell.Row.Cells[i + 1].Value != System.DBNull.Value)
                        FormatQuantityCell(vrq, e.Cell.Row.Cells[i + 1]);
                }
                else
                {
                    e.Cell.Row.Cells[i + 1].Appearance.Image = null;
                    e.Cell.Row.Cells[i + 1].Tag = null;
                    e.Cell.Row.Cells[i + 1].Value = System.DBNull.Value;
                    e.Cell.Row.Cells[i + 1].Activation = Activation.Disabled;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void FormatQuantityCell(eVelocityRuleRequiresQuantity aVRQ, UltraGridCell aCell)
        {
            double dblValue = 0;
            try
            {
                switch (aVRQ)
                {
                    case eVelocityRuleRequiresQuantity.WeeksOfSupply:
                        dblValue = Math.Round(Convert.ToDouble(aCell.Value, CultureInfo.CurrentUICulture), 1);
                        break;
                    case eVelocityRuleRequiresQuantity.ShipUpToQty:
                        dblValue = Math.Round(Convert.ToDouble(aCell.Value, CultureInfo.CurrentUICulture), 0);
                        break;
                    case eVelocityRuleRequiresQuantity.AbsoluteQuantity:
                        dblValue = Math.Round(Convert.ToDouble(aCell.Value, CultureInfo.CurrentUICulture), 0);
                        //txtOHQuantity.MaxLength = 6;
                        break;
                }
                aCell.Value = dblValue;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnApply_Click(object sender, System.EventArgs e)
        {
            bool clearRules = false;
            bool clearQty = false;
            double qty = 0;
            int setValue, boundary, pst;
            eVelocityRuleType ruleType;
            try
            {
                setValue = Convert.ToInt32(this.cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
                eVelocityRuleRequiresQuantity vrq = (eVelocityRuleRequiresQuantity)(Convert.ToInt32(cboOHRule.SelectedValue, CultureInfo.CurrentUICulture));
                if ((eVelocityRuleType)vrq == eVelocityRuleType.None)
                {
                    clearRules = true;
                    clearQty = true;
                }
                else if (Enum.IsDefined(typeof(eVelocityRuleRequiresQuantity), vrq))
                {
                    if (!ValidQuantity(txtOHQuantity))
                    {
                        string text = MIDText.GetTextOnly(eMIDTextCode.msg_ErrorsFoundReviewCorrect);
                        MessageBox.Show(text);
                        return;
                    }
                    else
                        qty = Convert.ToDouble(txtOHQuantity.Text, CultureInfo.CurrentUICulture);
                }
                else
                {
                    clearQty = true;
                }
                for (int i = 0; i < _screenMatrixDataTable.Rows.Count; i++)
                {
                    if (_screenMatrixDataTable.Rows[i]["Grade"].ToString().Trim() != "Total:")
                    {
                        DataRow row = _screenMatrixDataTable.Rows[i];

                        //BEGIN TT#153 – add variables to velocity matrix - apicchetti
                        int intBoundary = 0;
                        if (row["Boundary"].ToString().Trim() != "")
                        {
                            intBoundary = Convert.ToInt32(row["Boundary"]);
                        }
                        boundary = Convert.ToInt32(intBoundary, CultureInfo.CurrentUICulture);
                        //END TT#153 – add variables to velocity matrix - apicchetti
                        //(changed to check for the total line or else a DBNull will not convert error is received

                        for (int j = 10; j < _screenMatrixDataTable.Columns.Count; j++)
                        {
                            DataColumn col = _screenMatrixDataTable.Columns[j];
                            if (col.ColumnName.Substring(0, 4) == "Rule")
                            {
                                pst = Convert.ToInt32(col.ColumnName.Substring(4), CultureInfo.CurrentUICulture);
                                if (clearRules)
                                {
                                    row[col] = System.DBNull.Value;
                                    ruleType = eVelocityRuleType.None;
                                }
                                else
                                {
                                    row[col] = Convert.ToInt32(vrq, CultureInfo.CurrentUICulture);
                                    ruleType = (eVelocityRuleType)vrq;
                                }
                                if (_matrixProcessed)
                                    _trans.VelocitySetMatrixCellType(setValue, boundary, pst, ruleType);
                            }
                            else if (col.ColumnName.Substring(0, 3) == "Qty")
                            {
                                pst = Convert.ToInt32(col.ColumnName.Substring(3), CultureInfo.CurrentUICulture);
                                if (clearQty)
                                {
                                    row[col] = System.DBNull.Value;
                                    qty = 0;
                                }
                                else
                                {
                                    row[col] = Convert.ToDouble(qty, CultureInfo.CurrentUICulture);
                                }
                                if (_matrixProcessed)
                                    _trans.VelocitySetMatrixCellQty(setValue, boundary, pst, qty);
                            }
                        }
                    }

                    foreach (UltraGridRow row in ugMatrix.Rows)
                    {
                        for (int j = 9; j < row.Band.Columns.Count; j++)
                        {
                            if (row.Cells[j].Column.Tag.ToString() == "Qty")
                            {
                                if (clearQty)
                                    row.Cells[j].Activation = Activation.Disabled;
                                else
                                    row.Cells[j].Activation = Activation.AllowEdit;
                            }
                        }
                    }
                    MatrixChangesMade = true;
                    if (_matrixProcessed)
                        btnChanges.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void ugBasisNodeVersion_BeforeCellUpdate(object sender, Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs e)
        {
            if (_matrixProcessed && !_changedByCode)
            {
                if (!ReprocessWarningOK(e.Cell.Column.Header.Caption))
                {
                    _returnToCell = e.Cell;
                    e.Cancel = true;
                    // BEGIN Issue 4391/4393 stodd 10.4.2007
                    if (e.Cell.Column.Key == "BasisHNRID")
                    {
                        _changedByCode = true;
                        return;
                    }
                    if (e.Cell.Column.Key == "DateRange")
                    {
                        _changedByCode = true;
                        return;
                    }
                    // END Issue 4391/4393 stodd 10.4.2007
                }
                _changedByCode = false;
            }
        }

        //		private void ugBasisSalesPeriod_BeforeCellUpdate(object sender, Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs e)
        //		{
        //			// BEGIN MID Track #3878 - JSmith - Receive Weight has changed message
        //			if (_matrixProcessed && !_changedByCode)
        //			// END MID Track #3878
        //			{
        //				if (!ReprocessWarningOK(e.Cell.Column.Header.Caption))
        //				{
        //					_returnToCell = e.Cell;
        //					e.Cancel = true;
        //					if (e.Cell.Column.Key == "DateRange")
        //					{
        //						_changedByCode = true;
        //						return;
        //					}
        //				}
        //				// BEGIN Issue 4393  stodd 10.3.2007
        //				// Moved this code up with in brackets
        //				// BEGIN MID Track #3878 - JSmith - Receive Weight has changed message
        //				_changedByCode = false;
        //				// END MID Track #3878
        //				// END Issue 4393
        //			}
        //		}

        private void ugVelocityGrades_BeforeCellUpdate(object sender, Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key == "Boundary")
                {
                    if (!MatrixWarningOK(e.Cell.Column.Header.Caption))
                    {
                        _returnToCell = e.Cell;
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        // Begin TT#3617 - JSmith - Velocity throws Null Ref error when a change is made to the Boundaries
        void ugVelocityGrades_AfterExitEditMode(object sender, EventArgs e)
        {
            if (ugVelocityGrades.ActiveCell != null)
            {
                if (ugVelocityGrades.ActiveCell.Column.Key == "Boundary" &&
                    _matrixEverProcessed)
                {
                    MatrixTab_Load();
                }
            }
        }
        // End TT#3617 - JSmith - Velocity throws Null Ref error when a change is made to the Boundaries

        private void ugSellThruPcts_BeforeCellUpdate(object sender, Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs e)
        {
            if (!MatrixWarningOK(e.Cell.Column.Header.Caption))
            {
                _returnToCell = e.Cell;
                e.Cancel = true;
            }
        }
        private void ugGrid_BeforeCellDeactivate(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Infragistics.Win.UltraWinGrid.UltraGrid ugGrid;
            if (_returnToCell != null)
            {
                ugGrid = (Infragistics.Win.UltraWinGrid.UltraGrid)sender;
                e.Cancel = true;
                ugGrid.ActiveCell = _returnToCell;
                ugGrid.PerformAction(UltraGridAction.EnterEditMode, false, false);
                _returnToCell = null;

            }
        }

        private bool InInteractiveMode()
        {
            return (_trans != null && _trans.AllocationCriteriaExists && _trans.StyleView != null) ? true : false;
        }

        private bool InteractiveWarningOK(string aChangedItem)
        {
            DialogResult diagResult;
            string errorMessage = string.Empty;
            bool continueProcess = true;
            try
            {

                errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteStoreDetailWarning),
                    aChangedItem);
                errorMessage += Environment.NewLine + MIDText.GetTextOnly((int)eMIDTextCode.msg_ContinueQuestion);
                diagResult = MessageBox.Show(errorMessage, _thisTitle,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (diagResult == System.Windows.Forms.DialogResult.No)
                    continueProcess = false;
                else
                {
                    _trans.StyleView.Close();
                    continueProcess = true;
                }

            }
            catch (Exception ex)
            {
                HandleException(ex);
                continueProcess = false;
            }
            return continueProcess;
        }

        private bool MatrixWarningOK(string aChangedItem)
        {
            DialogResult diagResult;
            string errorMessage = string.Empty;
            bool continueProcess = true;
            try
            {
                // BEGIN TT#854 - AGallagher - Velocity - When chaning attributes the user received a message that they can view as "Read Only"
                //if (_dbMatrixDataTable.Rows.Count > 0)
                if (_matrixProcessed)
                // END TT#854 - AGallagher - Velocity - When chaning attributes the user received a message that they can view as "Read Only"
                {
                    errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_VelocityMatrixDeleteWarning),
                        aChangedItem);
                    errorMessage += Environment.NewLine + MIDText.GetTextOnly((int)eMIDTextCode.msg_ContinueQuestion);
                    diagResult = MessageBox.Show(errorMessage, _thisTitle,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (diagResult == System.Windows.Forms.DialogResult.No)
                        continueProcess = false;
                    else
                    {
                        if (InInteractiveMode())
                        {
                            _trans.StyleView.Close();
                        }
                        ResetMatrix();
                        continueProcess = true;
                    }
                }
                else
                    continueProcess = true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                continueProcess = false;
            }
            return continueProcess;
        }

        // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
        private bool MatrixModeWarningOK(string aChangedItem)
        {
            DialogResult diagResult;
            string errorMessage = string.Empty;
            bool continueProcess = true;
            try
            {
                // BEGIN TT#949 - AGallagher - Velocity - holds old rules when changing the sell thru index on the grade tab within the method - should give message the matrix will clear due to change
                // this is the same change as TT#854 
                //if (_dbMatrixDataTable.Rows.Count > 0)
                //if (_matrixProcessed) // TT#960 - AGallagher - Velocity - Spread Average (#7) - Screen Processing Oversites 
                // BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                //if (_matrixProcessed || !_matrixProcessed) // TT#960 - AGallagher - Velocity - Spread Average (#7) - Screen Processing Oversites 
                // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                // END TT#949 - AGallagher - Velocity - holds old rules when changing the sell thru index on the grade tab within the method - should give message the matrix will clear due to change
                {
                    errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_VelocityMatrixDeleteWarning),
                        aChangedItem);
                    errorMessage += Environment.NewLine + MIDText.GetTextOnly((int)eMIDTextCode.msg_ContinueQuestion);
                    diagResult = MessageBox.Show(errorMessage, _thisTitle,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (diagResult == System.Windows.Forms.DialogResult.No)
                    {
                        continueProcess = false;
                        //_matrixModeChanged = false;  // TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                    }
                    else
                    {
                        if (InInteractiveMode())
                        {
                            _trans.StyleView.Close();
                        }
                        // BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                        //_groupLevelDataTable.Clear();
                        //_attributeChanged = true;
                        //_trans = null;  
                        // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                        ResetMatrix();
                        //LoadAttributeSetValues((int)cbxAttributeSet.SelectedValue); // TT#960 - AGallagher - Velocity - Spread Average (#7) - Screen Processing Oversites 
                        continueProcess = true;
                        //_matrixModeChanged = true;  // TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                    }
                }
                // BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                //else
                //continueProcess = true;
                // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
            }
            catch (Exception ex)
            {
                HandleException(ex);
                continueProcess = false;
            }
            return continueProcess;
        }
        // END TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 

        private void ResetMatrix()
        {
            try
            {
                _dbMatrixDataTable.Clear();
                _dbMatrixDataTable.AcceptChanges();
                _screenMatrixDataTable.Clear();     // TT#130 - RMatelic - Database Foreign Key Violation deleting a Grade or Sell Thru then Save 
                _matrixProcessed = false;
                MatrixChangesMade = true;
                _statsCalculated = false;
                this.btnView.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ProcessInteractive);
                this.btnChanges.Enabled = false;
                ResetGroupData();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }


        private bool ReprocessWarningOK(string aChangedItem)
        {
            DialogResult diagResult;
            string errorMessage = string.Empty;
            bool continueProcess = true;
            try
            {
                //if (_dbMatrixDataTable.Rows.Count > 0 && _matrixProcessed)
                if (_matrixProcessed)
                {
                    errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_VelocityMatrixReprocessWarning),
                        aChangedItem);
                    errorMessage += Environment.NewLine + MIDText.GetTextOnly((int)eMIDTextCode.msg_ContinueQuestion);
                    diagResult = MessageBox.Show(errorMessage, _thisTitle,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (diagResult == System.Windows.Forms.DialogResult.No)
                        continueProcess = false;
                    else
                    {
                        if (InInteractiveMode())
                        {
                            // Begin TT#975 - RMatelic - Velocity Detail Screen when selecting  "Set"  from the top left corner selection for "All Store" or "Set"  received a Null Reference Exception
                            //_trans.StyleView.Close();
                            if (!_updateFromStoreDetail)
                            {
                                _trans.StyleView.Close();

                            }
                            // End TT#975
                        }
                        CheckInteractive();  // TT#4795 - Summary: Velocity Method -> GA regression testing-> flip switch Set on Basis tab-> matrix tab throws error if select process interactive before checking activate matrix -> expect a friendlier message
                        ResetMatrixForReprocess();
                        continueProcess = true;
                    }
                }
                else
                    continueProcess = true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                continueProcess = false;
            }
            return continueProcess;
        }

        private void ResetMatrixForReprocess()
        {
            try
            {
                _matrixProcessed = false;
                ResetStoreData();
                _statsCalculated = false;
                this.btnView.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ProcessInteractive);
                this.btnChanges.Enabled = false;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ResetStoreData()
        {
            try
            {
                int setValue = Convert.ToInt32(this.cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
                //				string grade;
                for (int i = 0; i < _screenMatrixDataTable.Rows.Count; i++)
                {
                    DataRow row = _screenMatrixDataTable.Rows[i];
                    //					grade = Convert.ToString(row["Grade"],CultureInfo.CurrentUICulture); 
                    //				
                    //					row["TotalSales"] = _trans.VelocityGetMatrixGradeTotBasisSales(setValue, grade);
                    //					row["AvgSales"] = _trans.VelocityGetMatrixGradeAvgBasisSales(setValue, grade);
                    //					row["PctTotalSales"] =_trans.VelocityGetMatrixGradeAvgBasisSalesPctTot(setValue, grade);
                    //					row["AvgSalesIdx"] = _trans.VelocityGetMatrixGradeAvgBasisSalesIdx(setValue, grade);

                    row["TotalSales"] = System.DBNull.Value;
                    row["AvgSales"] = System.DBNull.Value;
                    row["PctTotalSales"] = System.DBNull.Value;
                    row["AvgSalesIdx"] = System.DBNull.Value;

                    //BEGIN TT#153 – add variables to velocity matrix - apicchetti
                    row["TotalNumStores"] = System.DBNull.Value;
                    row["StockPercentOfTotal"] = System.DBNull.Value;
                    row["AvgStock"] = System.DBNull.Value;
                    row["AllocationPercentOfTotal"] = System.DBNull.Value;
                    //END TT#153 – add variables to velocity matrix - apicchetti

                    for (int j = 10; j < _screenMatrixDataTable.Columns.Count; j++)
                    {
                        DataColumn col = _screenMatrixDataTable.Columns[j];

                        if (col.ColumnName.Substring(0, 3) == "Qty" || col.ColumnName.Substring(0, 4) == "Rule")
                        {	//skip
                        }
                        else if (col.ColumnName.Substring(0, 6) == "Stores")
                        {
                            row[col] = System.DBNull.Value;
                        }
                        else if (col.ColumnName.Substring(0, 6) == "AvgWOS")
                        {
                            row[col] = System.DBNull.Value;
                        }
                        // BEGIN TT#960 - AGallagher - Velocity - Spread Average (#7) - Screen Processing Oversites
                        if (rdoMatrixModeAverage.Checked == true && col.ColumnName.Substring(0, 3) == "Qty")
                        {
                            row[col] = System.DBNull.Value;
                        }
                        if (rdoMatrixModeAverage.Checked == true && col.ColumnName.Substring(0, 4) == "Rule")
                        {
                            row[col] = System.DBNull.Value;
                        }
                        // END TT#960 - AGallagher - Velocity - Spread Average (#7) - Screen Processing Oversites 
                    }
                }
                SetNoOnHandStores(false);
                _addBandGroups = false;
                FormatScreenMatrix();
                ResetGroupData();
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void ResetGroupData()
        {
            try
            {
                foreach (DataRow row in _groupDataTable.Rows)
                {
                    row["AvgWOS"] = System.DBNull.Value;
                    row["PctSellThru"] = System.DBNull.Value;
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
        private void ReloadRuleAndQty()
        {
            try
            {
                int setValue = Convert.ToInt32(this.cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
                Hashtable ht = (Hashtable)_trans.Velocity.GroupLvlMtrxData;
                VelocityMethod.GroupLvlMatrix glm = (VelocityMethod.GroupLvlMatrix)ht[setValue];
                // BEGIN TT#945 - AGallagher - Velocity - Matrix Mode = avg, Spread Option  = Index .  Would expect the matrix cells to populate with the average store of the matrix FWOS 
                // Hashtable matrixCells = (Hashtable)glm.MatrixCells;
                Hashtable matrixCells_HT = (Hashtable)glm.MatrixCells;
                SortedList matrixCells = new SortedList();

                foreach (string key in matrixCells_HT.Keys)
                {
                    matrixCells.Add(key, matrixCells_HT[key]);
                }

                // END TT#945 - AGallagher - Velocity - Matrix Mode = avg, Spread Option  = Index .  Would expect the matrix cells to populate with the average store of the matrix FWOS 
                int sellThruCount = _sellThruPctsDataTable.Rows.Count;

                for (int i = 0; i < _screenMatrixDataTable.Rows.Count; i++)
                {
                    DataRow row = _screenMatrixDataTable.Rows[i];
                    // BEGIN TT#945 - AGallagher - Velocity - Matrix Mode = avg, Spread Option  = Index .  Would expect the matrix cells to populate with the average store of the matrix FWOS 
                    //for (int j = 10; j < _screenMatrixDataTable.Columns.Count; j++)
                    for (int j = 0; j < sellThruCount; j++)
                    // END TT#945 - AGallagher - Velocity - Matrix Mode = avg, Spread Option  = Index .  Would expect the matrix cells to populate with the average store of the matrix FWOS 
                    {
                        // BEGIN TT#945 - AGallagher - Velocity - Matrix Mode = avg, Spread Option  = Index .  Would expect the matrix cells to populate with the average store of the matrix FWOS 
                        for (int j2 = 10 + (j * 4); j2 < 14 + (j * 4); j2++)
                        {
                            //DataColumn col = _screenMatrixDataTable.Columns[j];
                            //int matrixCol = (j - 10) / sellThruCount;
                            DataColumn col = _screenMatrixDataTable.Columns[j2];
                            int matrixCol = j;
                            // END TT#945 - AGallagher - Velocity - Matrix Mode = avg, Spread Option  = Index .  Would expect the matrix cells to populate with the average store of the matrix FWOS 
                            string cellKey = i.ToString() + "," + matrixCol.ToString();
                            if (col.ColumnName.Substring(0, 4) == "Rule" && (rdoMatrixModeAverage.Checked == true))
                            {

                                VelocityMethod.GroupLvlCell glc = (VelocityMethod.GroupLvlCell)matrixCells[cellKey];

                                // BEGIN TT#1083 - AGallagher - Velocity - Velocity when switching from All Stores to Set there is residual (information) in the Matrix
                                row[col] = System.DBNull.Value;
                                if ((glc != null) && (glc.CellRuleType == (eVelocityRuleType)Convert.ToInt32(cboMatrixModeAvgRule.SelectedValue, CultureInfo.CurrentUICulture)) && (_velocityMethod.CalculateAverageUsingChain == true) && (glc.CellRuleTypeQty > 0) && (glc.CellChnStores > 0))
                                //if ((glc != null) && glc.CellRuleType == (eVelocityRuleType)Convert.ToInt32(cboMatrixModeAvgRule.SelectedValue, CultureInfo.CurrentUICulture) && (glc.CellRuleTypeQty > 0))
                                // END TT#1083 - AGallagher - Velocity - Velocity when switching from All Stores to Set there is residual (information) in the Matrix
                                {
                                    row[col] = (eVelocityRuleType)Convert.ToInt32(cboMatrixModeAvgRule.SelectedValue, CultureInfo.CurrentUICulture);
                                }
                                // BEGIN TT#1083 - AGallagher - Velocity - Velocity when switching from All Stores to Set there is residual (information) in the Matrix
                                if ((glc != null) && (glc.CellRuleType == (eVelocityRuleType)Convert.ToInt32(cboMatrixModeAvgRule.SelectedValue, CultureInfo.CurrentUICulture)) && (_velocityMethod.CalculateAverageUsingChain == false) && (glc.CellRuleTypeQty > 0) && (glc.CellGrpStores > 0))
                                {
                                    row[col] = (eVelocityRuleType)Convert.ToInt32(cboMatrixModeAvgRule.SelectedValue, CultureInfo.CurrentUICulture);
                                }
                                // END TT#1083 - AGallagher - Velocity - Velocity when switching from All Stores to Set there is residual (information) in the Matrix

                            }
                            if (col.ColumnName.Substring(0, 3) == "Qty" && (rdoMatrixModeAverage.Checked == true))
                            {
                                VelocityMethod.GroupLvlCell glc = (VelocityMethod.GroupLvlCell)matrixCells[cellKey];

                                // BEGIN TT#1083 - AGallagher - Velocity - Velocity when switching from All Stores to Set there is residual (information) in the Matrix
                                row[col] = System.DBNull.Value;
                                if ((glc != null) && (glc.CellRuleType == (eVelocityRuleType)Convert.ToInt32(cboMatrixModeAvgRule.SelectedValue, CultureInfo.CurrentUICulture)) && (_velocityMethod.CalculateAverageUsingChain == true) && (glc.CellRuleTypeQty > 0) && (glc.CellChnStores > 0))
                                //if ((glc != null) && glc.CellRuleType == (eVelocityRuleType)Convert.ToInt32(cboMatrixModeAvgRule.SelectedValue, CultureInfo.CurrentUICulture) && (glc.CellRuleTypeQty > 0))
                                // END TT#1083 - AGallagher - Velocity - Velocity when switching from All Stores to Set there is residual (information) in the Matrix
                                {
                                    row[col] = Convert.ToDouble(glc.CellRuleTypeQty, CultureInfo.CurrentUICulture);
                                }
                                // BEGIN TT#1083 - AGallagher - Velocity - Velocity when switching from All Stores to Set there is residual (information) in the Matrix
                                if ((glc != null) && (glc.CellRuleType == (eVelocityRuleType)Convert.ToInt32(cboMatrixModeAvgRule.SelectedValue, CultureInfo.CurrentUICulture)) && (_velocityMethod.CalculateAverageUsingChain == false) && (glc.CellRuleTypeQty > 0) && (glc.CellGrpStores > 0))
                                {
                                    row[col] = Convert.ToDouble(glc.CellRuleTypeQty, CultureInfo.CurrentUICulture);
                                }
                                // END TT#1083 - AGallagher - Velocity - Velocity when switching from All Stores to Set there is residual (information) in the Matrix
                            }
                        }
                    }

                }  // TT#945 - AGallagher - Velocity - Matrix Mode = avg, Spread Option  = Index .  Would expect the matrix cells to populate with the average store of the matrix FWOS 
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        // END TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 

        private void cbxInteractive_CheckedChanged(object sender, System.EventArgs e)
        {

            if (_trans != null && _trans.StyleView != null)
            {
                _trans.StyleView.Close();
            }

            try
            {
                if (_cbxReset)
                {
                    _cbxReset = false;
                    return;
                }

                if (!cbxInteractive.Checked)
                {
                    if (InInteractiveMode())
                    {
                        if (!InteractiveWarningOK(this.cbxInteractive.Text))
                        {
                            _cbxReset = true;
                            cbxInteractive.Checked = true;
                            return;
                        }
                    }
                    ResetMatrixForReprocess();
                    cboComponent.SelectedIndex = -1;
                    cboComponent.Enabled = false;
                    SetNoOnHandStores(false);
                    btnView.Enabled = false;
                    btnChanges.Enabled = false;
                    btnProcess.Enabled = true;
                    // begin TT#1185 - Verify ENQ before Update
					// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
					if (_trans != null && !_isProcessingAssortment)
                    //if (_trans != null)
					// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
                    {
                        _trans.DequeueHeaders();
                        _trans.Dispose();
                        _trans = null;
                    }
                    // end TT#1185 - Verify ENQ before Update
                }
                else
                {
                    // BEGIN TT#2033-MD - AGallagher - Process Vel on an Asst Header and receive mssg 80269: No header selected or selected header requiers a Save.  Do not expect to receive this mssg.
                    if (AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID != Include.NoRID &&  _selectedHeaderList.Count == 0)  // use assortment header list
                    {
                        _selectedHeaderList = SAB.AssortmentSelectedHeaderEvent.GetSelectedHeaders(this, AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID, eMethodType.Velocity);
                    }
                    // END TT#2033-MD - AGallagher - Process Vel on an Asst Header and receive mssg 80269: No header selected or selected header requiers a Save.  Do not expect to receive this mssg.
                    if (_selectedHeaderList.Count == 0)
                    {
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoHeaderSelectedOnWorkspace));
                        _cbxReset = true;
                        cbxInteractive.Checked = false;
                        return;
                    }
                    // begin TT#1185 - Verify ENQ before Update
                    // removed unnecessary comments for readablility
                    //else
                    //{

					// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
					//bool isProcessingAssortment = false;	// TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
                    //BEGIN TT#849-MD-DOConnell - No values are returned when processing Velocity Interactive on a header in the allocation workspace while an Assortment is open
                    if (AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID != Include.NoRID)	// Use assortment
                    {
						// Begin TT#1034 - MD - stodd - Resource Locks are GONE after Group Allocation Velocity - 
                        ApplicationSessionTransaction aTrans = GetAssortmentViewWindowTransaction(AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID);
                        if (aTrans != null)
                        {
                            _isProcessingAssortment = true;
                            _trans = aTrans;
                            //AllocationProfileList headerList = (AllocationProfileList)_trans.GetMasterProfileList(eProfileType.Allocation);
                            //AllocationSubtotalProfile grandTotal = this._trans.GetAllocationGrandTotalProfile();
                        }
						// End TT#1034 - MD - stodd - Resource Locks are GONE after Group Allocation Velocity - 
                    //}

                    //if (_isProcessingAssortment)
                    //{
                    //    //_trans = av.Transaction;


                    //    //AllocationProfileList headerList = (AllocationProfileList)_trans.GetMasterProfileList(eProfileType.Allocation);
                    //    //AllocationSubtotalProfile grandTotal = this._trans.GetAllocationGrandTotalProfile();
                    }
                    else
                    {
                    //    _isProcessingAssortment = false;	// TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
						_trans = SAB.ApplicationServerSession.CreateTransaction();
						string enqMessage = string.Empty;
						if (!_trans.EnqueueSelectedHeaders(out enqMessage))
						{
                            // Begin TT#4515 - stodd - enqueue message
                            //enqMessage =
                            //    MIDText.GetTextOnly(eMIDTextCode.msg_al_HeaderEnqFailed)
                            //    + System.Environment.NewLine
                            //    + enqMessage;
                            // Begin TT#4515 - stodd - enqueue message
							SAB.MessageCallback.HandleMessage
								(
									enqMessage,
									"Header Lock Conflict",
									System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk
								);
							_cbxReset = true;
							cbxInteractive.Checked = false;
							return;
						}
                    }
                    //END TT#849-MD-DOConnell - No values are returned when processing Velocity Interactive on a header in the allocation workspace while an Assortment is open
                    // end TT#1185 - Verify ENQ before Update
                    if (_matrixProcessed)
                        _compReset = true;
                    // (RonM) - BEGIN MID Track #2410: Allow interactive processing with multiple headers
                    // Removed 'if /else' condition; added MatrixTab_Load()
                    //if (_selectedHeaderList.Count == 1)
                    //{
                    SelectedHeaderProfile shp = (SelectedHeaderProfile)_selectedHeaderList[0];
                    //GetBalanceInd();   // TT#673 - AGallagher - Velocity - Disable Balance option on WUB header - rework requested not to gray or modify balance   
                    GetPacksAndColors();
                    GetGradesPstFromHeader(shp.Key);
                    if (_screenMatrixDataTable.Rows.Count < 1)
                    {
                        MatrixTab_Load();
                        _rebuildMatrix = false;
                    }
                    //}
                    //else
                    //{
                    //	RemoveBulk();
                    //}
                    //  (RonM) - END MID Track #2410

                    cboComponent.SelectedValue = (int)eVelocityMethodComponentType.Total;
                    if (_matrixProcessed)
                        SetNoOnHandStores(true);
                    //}  // TT#1185 - Verify ENQ before Update
                    cboComponent.Enabled = true;
                    btnView.Enabled = true;
                    btnProcess.Enabled = false;
                }
                for (int i = 6; i < this.ugMatrix.DisplayLayout.Bands[0].Columns.Count; i++)
                {
                    switch (this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Tag.ToString())
                    {
                        case "Stores":
                        case "AvgWOS":
                            this.ugMatrix.DisplayLayout.Bands[0].Columns[i].Hidden = !cbxInteractive.Checked;
                            break;
                    }
                }
                ShowHideColHeaders(_matrixColumnHeaders);

                if (!cbxInteractive.Checked)
                    ResetGroupData();

                // Begin TT#3033/TT#671-MD - RMatelic Velocity Method - Avg Matrix Mode - Greyed Out
                SetMatrixActivation();
                // End TT#3033/TT#671-MD  
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnView_Click(object sender, System.EventArgs e)
        {
            bool blResult = false;
            try
            {
                // (CSMITH) - BEG MID Track #2410: Allow interactive processing with multiple headers
                //				if (_selectedHeaderList.Count > 1)
                //				{
                //					MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MultHeadersSelectedOnWorkspace));
                //					return;
                //				}
                //				else
                //				{
                // (CSMITH) - END MID Track #2410
                //ErrorFound = ValidateAndSetFields();
                ErrorFound = InteractiveValidateandSetFields();
                if (!ErrorFound)
                {
                    this.Cursor = Cursors.WaitCursor;

                    if (_matrixProcessed)
                    {
                        ViewStoreDetail();
                    }
                    else
                    {
                        // begin TT#406 Unhandled exception when checking / unchecking balance
                        blResult = ProcessInteractive();
                        ////tt#288 - Added a return value from ProcessInteractive to evalute to determine how to handle total line function - apicchetti
                        // end TT#406 Unhandled exception when checking / unchecking balance
                        //blResult = 	ProcessInteractive(true);				
                        // Begin TT#299 - stodd - 
                        _afterProcess = true;
                        // End TT#299 - stodd - 

                        // begin TT#1195 - Verify ENQ before Update
                        //if (_interactiveCtr > 1) //TT#328 - checking for the number of interactive processes, if we are past the initial one, the enqueue security should be bypassed - apicchetti
                        //{
                        //    BypassSecurityEnqueueCheck = true;
                        //}
                        // end TT#1185 - Verify ENQ before Update
                    }


                }
                // (CSMITH) - BEG MID Track #2410: Allow interactive processing with multiple headers
                //				}
                // (CSMITH) - END MID Track #2410
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                //tt#288 - Handle total line with return value
				//Begin TT#1417 - JScott - Velocity gives unhandled exception error is color is selected as basis and header has multiple colors
				//if (blResult == false)
				if (!blResult || !_matrixProcessed)
				//End TT#1417 - JScott - Velocity gives unhandled exception error is color is selected as basis and header has multiple colors
				{
                    ugMatrix_AddTotalLine(true);
                }
                else
                {
                    ugMatrix_AddTotalLine(false);
                }
            }

            this.Cursor = Cursors.Default;
        }
        // copied from WorkflowMethodBase and modified 
        private bool InteractiveValidateandSetFields()
        {
            try
            {
                ErrorFound = false;
                SetCommonFields();
                //if (!ValidateCommonFields())
                //{
                //	ErrorFound = true;
                //}
                if (!ValidateSpecificFields())
                {
                    ErrorFound = true;
                }

                if (ErrorFound)
                {
                    HandleErrors();
                    string text = MIDText.GetTextOnly(eMIDTextCode.msg_ErrorsFoundReviewCorrect);
                    MessageBox.Show(text);
                    //return true;
                }
                else
                {
                    SetSpecificFields();
                    SetObject();
                    //LoadCommonFields();
                }
            }
            catch (Exception err)
            {
                HandleException(err);
            }

            return ErrorFound;
        }
        //private bool ProcessInteractive(bool NewTrans) // TT#406 Unhandled exception when checking / unchecking balance
        private bool ProcessInteractive()                // TT#406 Unhandled exception when checking / unchecking balance
        {
            try
            {
                // begin TT#406 Unhandled exception when checking / unchecking balance
                if (!_updateFromStoreDetail)    // TT#975 - RMatelic - Velocity Detail Screen when selecting  "Set" gets null reference >>> added 'if...' condition 
                {
                    // begin TT#1185 - Verify ENQ before Update
                    //if (_trans != null)
                    //{
                    //    _trans.Dispose();
                    //}
                    //_trans = SAB.ApplicationServerSession.CreateTransaction();
                    // end TT#1185 - Verify ENQ before Update
 
                    if (_velocityMethod.Key == Include.NoRID)
                    {
                        _velocityMethod.Key = Include.UndefinedVelocityMethodRID;
                    }
                    // Begin TT#983 - RMatelic - Velocity Detail Screen when selecting All Stores or Set user is prompted that the matrix will be reprocessed, but the grid is not cleared entirely
                    //_trans.CreateVelocityMethod(SAB, _velocityMethod.Key);
                    // End TT#983
                }
                // Begin TT#983 - RMatelic - Velocity Detail Screen when selecting All Stores or Set user is prompted that the matrix will be reprocessed, but the grid is not cleared entirely
                else
                {
                    _trans.ResetVelocityMethod();
                }

				// Begin TT#1019 - md - For headers within a Group Allocation, Prohibit allocation-type actions in the review screens and allocation workspace - 
                int assrtRid = AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID;
                if (assrtRid != Include.NoRID)
                {
                    _trans = SAB.AssortmentTransactionEvent.GetAssortmentTransaction(this, assrtRid);

                    //ProfileList apl1 = _trans.GetMasterProfileList(eProfileType.Allocation);
                    //ProfileList apl2 = _trans.GetMasterProfileList(eProfileType.AssortmentMember);


					// Begin TT#1019 - MD - stodd - interactive velocity vs read-only group allocation - 
                    if (_trans.DataState == eDataState.ReadOnly)
                    {
                        throw new GroupAllocationAssortmentReadOnlyException(MIDText.GetText(eMIDTextCode.msg_as_NoProcessingMethodReadOnly),
                            AssortmentActiveProcessToolbarHelper.ActiveProcess.screenType,
                            AssortmentActiveProcessToolbarHelper.ActiveProcess.screenTitle);
                    }
					// End TT#1019 - MD - stodd - interactive velocity vs read-only group allocation - 
                }
                // Begin TT#3149 - RMatelic - Error when Changing Attribute in Velocity >>> TT#694 set _trans = null when Attribute is changed 
				// Begin TT#952 - MD - stodd - add matrix to Group Allocation Review
                else 
                {
                    if (_trans == null)
                    {
                        _trans = SAB.ApplicationServerSession.CreateTransaction();
                    }
                    SelectedHeaderList selectedHeaderList = (SelectedHeaderList)_trans.GetProfileList(eProfileType.SelectedHeader);
                    _trans.LoadHeadersInTransaction(selectedHeaderList);

                }

                //SelectedHeaderList selectedHeaderList = (SelectedHeaderList)_trans.GetProfileList(eProfileType.SelectedHeader);
                //_trans.LoadHeadersInTransaction(selectedHeaderList);
				// End TT#952 - MD - stodd - add matrix to Group Allocation Review
				
                //=======================================================================================
                // This only needs to be checked if headers are selected from the allocation workspace.
                // You can tell that is true if the assrtRid == Include.NoRID.
                //=======================================================================================
                if (assrtRid == Include.NoRID)
                {
                    ArrayList headerInGAList = new ArrayList();
                    if (_trans.ContainsGroupAllocationHeaders(ref headerInGAList))
                    {
                        throw new HeaderInGroupAllocationException(MIDText.GetText(eMIDTextCode.msg_al_HeaderBelongsToGroupAllocation), headerInGAList);
                    }
                }
				// End TT#1019 - md - For headers within a Group Allocation, Prohibit allocation-type actions in the review screens and allocation workspace - 

                // End TT#3149 
                _trans.CreateVelocityMethod(SAB, _velocityMethod.Key);
                // End TT#983
                UpdateTransData();
                _trans.VelocityLoadDataArrays();
                _trans.Velocity.BasisChangesMade = _basisChangesMade;	// TT#4522 - stodd - velocity matrix wrong
                _trans.ProcessInteractiveVelocity();

                //if (_trans != null)
                //{
                //    _trans.Dispose();
                //}

                //if(NewTrans == true || _trans == null) //tt#315 - locked transaction - apicchetti
                //{
                //    _trans = SAB.ApplicationServerSession.CreateTransaction();
                //}

                //if (_velocityMethod.Key == Include.NoRID)
                //    _velocityMethod.Key = Include.UndefinedVelocityMethodRID;

                //_trans.CreateVelocityMethod(SAB, _velocityMethod.Key);
                //_trans.Velocity.Component = this._velocityMethod.Component; // MID Track 3182 Velocity not recognizing specific component
                //UpdateTransData();
                //_trans.VelocityLoadDataArrays();

                //_trans.ProcessInteractiveVelocity();
                ////tt#152 - velocity balance - apicchetti
                //_trans.ProcessInteractiveVelocity(cbxBalance.Checked);
                ////tt#152 - set the balance flag in the velocity method
                // end TT#406 Unhandled exception when checking / unchecking balance

                // begin MID Track 4094 'color' not recognized by velocity
                eAllocationActionStatus actionStatus = _trans.AllocationActionAllHeaderStatus;
                if (actionStatus != eAllocationActionStatus.ActionCompletedSuccessfully)
                {
                    ErrorFound = true;
                    string message = MIDText.GetTextOnly((int)actionStatus);
                    MessageBox.Show(message, _thisTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // begin TT#241 - MD - JEllis - Header Enqueue Process
                    if (actionStatus == eAllocationActionStatus.HeaderEnqueueFailed
                        || actionStatus == eAllocationActionStatus.NoHeaderResourceLocks)
                    {
                        if (_trans.SummaryView != null)
                        {
                            MIDRetail.Windows.SummaryView frmSummaryView = (MIDRetail.Windows.SummaryView)_trans.SummaryView;
                            if (ErrorFound)
                            {
                                frmSummaryView.ErrorFound = true;
                            }
                            frmSummaryView.Close();
                        }
                        if (_trans.StyleView != null)
                        {
                            MIDRetail.Windows.StyleView frmStyleView = (MIDRetail.Windows.StyleView)_trans.StyleView;
                            if (ErrorFound)
                            {
                                frmStyleView.ErrorFound = true;
                            }
                            frmStyleView.Close();
                        }
                        if (_trans.AssortmentView != null)
                        {
                            MIDRetail.Windows.AssortmentView frmAssortmentView = (MIDRetail.Windows.AssortmentView)_trans.AssortmentView;
                            if (ErrorFound)
                            {
                                frmAssortmentView.ErrorFound = true;
                            }
                            frmAssortmentView.Close();
                        }
                        if (_trans.SizeView != null)
                        {
                            MIDRetail.Windows.SizeView frmSizeView = (MIDRetail.Windows.SizeView)_trans.SizeView;
                            if (ErrorFound)
                            {
                                frmSizeView.ErrorFound = true;
                            }
                            frmSizeView.Close();
                        }
                        if (_trans.VelocityWindow != null)
                        {
                            Close();
                        }
                    }
                    // end TT#241 - MD - JEllis - Header Enqueue Process
                }
                // end MID Track 4094 'color' not recognized by velocity
                if (!ErrorFound)
                {
                    _statsCalculated = true;
                    GetStoreData();
                    // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
                    ApplyViewToGridLayout(Convert.ToInt32(cboMatrixView.SelectedValue, CultureInfo.CurrentUICulture));
                    // End TT#231  
                    this.btnView.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_VelocityStoreDetail);
                    _matrixProcessed = true;
                    _matrixEverProcessed = true;  // TT#3617 - JSmith - Velocity throws Null Ref error when a change is made to the Boundaries
                    _basisChangesMade = false;	// TT#4522 - stodd - velocity matrix wrong
                }

                return true; //tt#288 - Added a return value for evaluation to determine how to handle total line function - apicchetti
            }
			// Begin TT#1019 - md - For headers within a Group Allocation, Prohibit allocation-type actions in the review screens and allocation workspace - 
            catch (HeaderInGroupAllocationException err)
            {
                string headerListMsg = string.Empty;
                foreach (string headerId in err.HeaderList)
                {
                    if (headerListMsg.Length > 0)
                        headerListMsg += ", " + headerId;
                    else
                        headerListMsg = " " + headerId;
                }
                MessageBox.Show(err.Message + headerListMsg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
			// Begin TT#1019 - MD - stodd - interactive velocity vs read-only group allocation - 
            catch (GroupAllocationAssortmentReadOnlyException err)
            {
                string errorMessage = err.Message;
                errorMessage = errorMessage.Replace("{0}", err.GroupType);
                errorMessage = errorMessage.Replace("{1}", err.GroupName);
                SAB.ClientServerSession.Audit.Add_Msg(
                    eMIDMessageLevel.Error,
                    eMIDTextCode.msg_as_NoProcessingMethodReadOnly,
                    errorMessage,
                    "Interactive Velocity");

                //string message = MIDText.GetTextOnly((int)eAllocationActionStatus.NoActionPerformed);
                MessageBox.Show(errorMessage, "Interactive Velocity", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
			// End TT#1019 - MD - stodd - interactive velocity vs read-only group allocation - 
			// End TT#1019 - md - For headers within a Group Allocation, Prohibit allocation-type actions in the review screens and allocation workspace - 
            catch (Exception ex)
            {
                HandleException(ex);
                return false;  //tt#288 - Added a return value for evaluation to determine how to handle total line function - apicchetti
            }
            finally
            {
                // Begin TT#4792 - stodd - GA-IN velocity basis is color, but headers in group are different colors-> get Velocity basis error-> go back to basis tab and accidentally check Reconcile and apply-> Object ref error message
                if (_matrixProcessed)
                {
                    _processintctr++;
                }
                // End TT#4792 - stodd - GA-IN velocity basis is color, but headers in group are different colors-> get Velocity basis error-> go back to basis tab and accidentally check Reconcile and apply-> Object ref error message
            }

        }

        private void UpdateTransData()
        {
            _trans.VelocityStoreGroupRID = _velocityMethod.StoreGroupRID;
            _trans.VelocityOTSPlanHNRID = _velocityMethod.OTSPlanHNRID;
            _trans.VelocityOTSPlanPHRID = _velocityMethod.OTSPlanPHRID;
            _trans.VelocityOTSPlanPHLSeq = _velocityMethod.OTSPlanPHLSeq;
            _trans.VelocityOTS_Begin_CDR_RID = _velocityMethod.OTS_Begin_CDR_RID;
            _trans.VelocityOTS_Ship_To_CDR_RID = _velocityMethod.OTS_Ship_To_CDR_RID;
            _trans.VelocityUseSimilarStoreHistory = _velocityMethod.UseSimilarStoreHistory;
            _trans.VelocityCalculateAverageUsingChain = _velocityMethod.CalculateAverageUsingChain;
            _trans.VelocityCalculateGradeVariableType = _velocityMethod.GradeVariableType; //TT#855-MD -jsobek -Velocity Enhancements 
            _trans.VelocityBalanceToHeaderInd = _velocityMethod.BalanceToHeaderInd; //TT#855-MD -jsobek -Velocity Enhancements 
            _trans.VelocityDetermineShipQtyUsingBasis = _velocityMethod.DetermineShipQtyUsingBasis;
            _trans.VelocityTrendPctContribution = _velocityMethod.TrendPctContribution;
            // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
            _trans.VelocityApplyMinMaxInd = _velocityMethod.ApplyMinMaxInd;
            // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
            _trans.VelocityInventoryInd = _velocityMethod.InventoryInd;  // TT#1287 - AGallagher - Inventory Min/Max
            // BEGIN TT#1566 - AGallagher - Velocity inventory basis is sub class (same as ots forecast)  Not getting expected results
            _trans.Velocity.MERCH_PHL_SEQ = _velocityMethod.MERCH_PHL_SEQ;
            _trans.Velocity.MERCH_PH_RID = _velocityMethod.MERCH_PH_RID;
            _trans.Velocity.MERCH_HN_RID = _velocityMethod.MERCH_HN_RID;
            // END TT#1566 - AGallagher - Velocity inventory basis is sub class (same as ots forecast)  Not getting expected results
            _trans.VelocityDSVelocity = _velocityMethod.DSVelocity;
            _trans.Velocity.Component = this._velocityMethod.Component;  // TT#406 Unhandled exception when checking / unchecking balance
            _trans.Velocity.Balance = cbxBalance.Checked; // TT#406 Unhandled exception when check/unchecking balance
            _trans.Velocity.Reconcile = cbxReconcile.Checked;  //TT#2617 - AGallagher - Velocity when Reconcile is checked on a new method do not get expected results, if the method is Saved and used again I do get expected results
			// Begin TT#4522 - stodd - velocity matrix wrong
            if (BasisChangesMade)
            {
                _trans.ClearAllocationCubeGroup();
            }
			// End TT#4522 - stodd - velocity matrix wrong
            _trans.VelocityIsInteractive = true;
        }
        private void GetStoreData()
        {
            //int setValue, pst, boundary;  // "pst" not used  // TT#1185 - Verify ENQ before Update (unrelated) 
            int setValue,  boundary;        // "pst" not used  // TT#1185 - Verify ENQ before Update (unrelated)
            string grade, rowHeader;
            try
            {
                setValue = Convert.ToInt32(this.cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
                for (int i = 0; i < _screenMatrixDataTable.Rows.Count; i++)
                {
                    DataRow row = _screenMatrixDataTable.Rows[i];

                    //tt#153 - Velocity Matrix Variable - apicchetti
                    if (row["Grade"].ToString().Trim() != "Total:")
                    {
                        grade = Convert.ToString(row["Grade"], CultureInfo.CurrentUICulture);
                        boundary = Convert.ToInt32(row["Boundary"], CultureInfo.CurrentUICulture);

                        row["TotalSales"] = _trans.VelocityGetMatrixGradeTotBasisSales(setValue, grade);
                        row["AvgSales"] = _trans.VelocityGetMatrixGradeAvgBasisSales(setValue, grade);
                        row["PctTotalSales"] = _trans.VelocityGetMatrixGradeAvgBasisSalesPctTot(setValue, grade);
                        row["AvgSalesIdx"] = _trans.VelocityGetMatrixGradeAvgBasisSalesIdx(setValue, grade);

                        //BEGIN TT#153 – add variables to velocity matrix - apicchetti
                        row["TotalNumStores"] = _trans.VelocityGetMatrixGradeTotalNumberOfStores(setValue, grade);
                        row["AvgStock"] = _trans.VelocityGetMatrixGradeAvgStock(setValue, grade);
                        row["StockPercentOfTotal"] = _trans.VelocityGetMatrixGradeStockPercentOfTotal(setValue, grade);
                        row["AllocationPercentOfTotal"] = _trans.VelocityGetMatrixGradeAllocationPercentOfTotal(setValue, grade);
                        ////END TT#153 – add variables to velocity matrix - apicchetti
                        // begin TT#586 Velocity Variables not calculated correctly
                        int sellThruIdx;
                        for (int sellThruRow = 0; sellThruRow < _sellThruPctsDataTable.Rows.Count; sellThruRow++)
                        {
                            sellThruIdx = Convert.ToInt32(_sellThruPctsDataTable.Rows[sellThruRow]["SellThruIndex"]);
                            row["Stores" + sellThruIdx.ToString()] = _trans.VelocityGetMatrixCellStores(setValue, boundary, sellThruIdx);
                            //row["AvgWOS" + sellThruIdx.ToString()] = _trans.VelocityGetMatrixCellAvgWOS(setValue, boundary, sellThruIdx, true);  // TT#586 Store counts differ between matrix and detail
                            row["AvgWOS" + sellThruIdx.ToString()] = _trans.VelocityGetMatrixCellAvgWOS(setValue, boundary, sellThruIdx);  // TT#586 store counts differ between matrix and detail
                        }
                        //for (int j = 10; j < _screenMatrixDataTable.Columns.Count; j++)
                        //{
                        //    DataColumn col = _screenMatrixDataTable.Columns[j];

                        //    if (col.ColumnName.Substring(0, 3) == "Qty" || col.ColumnName.Substring(0, 4) == "Rule")
                        //    {	//skip
                        //    }
                        //    else if (col.ColumnName.Substring(0, 6) == "Stores")
                        //    {
                        //        pst = Convert.ToInt32(col.ColumnName.Substring(6), CultureInfo.CurrentUICulture);
                        //        row[col] = _trans.VelocityGetMatrixCellStores(setValue, boundary, pst);
                        //    }
                        //    else if (col.ColumnName.Substring(0, 6) == "AvgWOS")
                        //    {
                        //        pst = Convert.ToInt32(col.ColumnName.Substring(6), CultureInfo.CurrentUICulture);
                        //        row[col] = _trans.VelocityGetMatrixCellAvgWOS(setValue, boundary, pst);
                        //    }
                        //}
                        // end TT#586 Velocity variables not calculated correctly
                    }
                }
                SetNoOnHandStores(true);
                _addBandGroups = false;
                FormatScreenMatrix();

                foreach (DataRow row in _groupDataTable.Rows)
                {
                    rowHeader = Convert.ToString(row["RowHeader"], CultureInfo.CurrentUICulture);
                    if (rowHeader == _lblAllStores)
                    {
                        row["AvgWOS"] = _trans.VelocityGetMatrixChainAvgWOS(setValue);
                        row["PctSellThru"] = _trans.VelocityGetMatrixChainPctSellThru(setValue);
                    }
                    else
                    {
                        row["AvgWOS"] = _trans.VelocityGetMatrixGroupAvgWOS(setValue);
                        row["PctSellThru"] = _trans.VelocityGetMatrixGroupPctSellThru(setValue);
                    }
                }
                // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                if (rdoMatrixModeAverage.Checked == true)
                {
                    ReloadRuleAndQty();
                }
                // END TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        // begin TT#1185 - Verify ENQ before Update
        ////begin TT#311 - Enqueue flag - apicchetti
        //bool _bypassSecurityEnqueueCheck = false;
        //public bool BypassSecurityEnqueueCheck
        //{
        //    get
        //    {
        //        return _bypassSecurityEnqueueCheck;
        //    }

        //    set
        //    {
        //        _bypassSecurityEnqueueCheck = value;
        //    }
        //}
        ////end TT#311 - Enqueue flag - apicchetti
        // end TT#1185 - Verify ENQ before update

        private void ViewStoreDetail()
        {
            MIDRetail.Windows.StyleView frmStyleView;
            try
            {
                if (!_trans.AllocationCriteriaExists || _trans.StyleView == null)
                {
                    _trans.VelocityWindow = this;
                    _trans.AllocationWorkspaceExplorer = EAB.AllocationWorkspaceExplorer;
                    _avs = new AllocationViewSelection(EAB, _trans);
                    _avs.MdiParent = this.MdiParent;
                    //_avs.BypassSecurityEnqueueCheck = _bypassSecurityEnqueueCheck;  // TT#1185 - Verify ENQ before Update
                    // Begin TT#3138 - RMatelic - Velocity Detail when you go in to the screen the scroll bar is the length of the screen and you can't scroll
                    _windowIsMaximized = (this.WindowState == FormWindowState.Maximized) ? true : false;
                    if (_windowIsMaximized)
                    {
                        this.WindowState = FormWindowState.Normal;
                    }
                    // End TT#3138
                    _avs.DetermineWindow(eAllocationSelectionViewType.Velocity);
                }
                else
                {
                    frmStyleView = (MIDRetail.Windows.StyleView)_trans.StyleView;
                    frmStyleView.Activate();
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void SetNoOnHandStores(bool AddNumStores)
        {
            try
            {
                if (AddNumStores)
                {
                    int setValue = Convert.ToInt32(this.cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
                    int numStores = _trans.VelocityGetMatrixNoOnHandStores(setValue);
                    gbxNoOH.Text = Convert.ToString(numStores, CultureInfo.CurrentUICulture) + " " + _noOnHandLabel;
                }
                else
                    gbxNoOH.Text = _noOnHandLabel;
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
        #region Window Close
        override protected void BeforeClosing()
        {
            try
            {
                if (InInteractiveMode())
                {
                    _trans.StyleView.Close();
                    //_trans.DequeueHeaders(); // TT#1185 - Verify ENQ before Update  // MID TT#29 - JEllis - Headers not released on Dequeue
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            // begin MID TT#29 - JEllis - Headers not released on Dequeue
            finally
            {
				// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
				// If Assortment headers were processed, we don't want them dequeued. The assortment review window will handle that.
                if (_trans != null && !_isProcessingAssortment)
				// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
                {
                    _trans.DequeueHeaders();
                }
            }
            // end MID TT#29 JEllis - Headers not released on Dequeue
        }

        override protected void AfterClosing()
        {
            try
            {
				// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
				// If Assortment headers were processed, we don't want them dequeued. The assortment review window will handle that.
				if (_trans != null && !_isProcessingAssortment)
				// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
                    _trans.VelocityWindow = null;
            }
            catch (Exception ex)
            {
                HandleException(ex, "VelocityMethod.AfterClosing");
            }
        }
        #endregion


        private void radAvgChain_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!FormLoaded) return;
            if (radAvgChain.Checked)
            {	// BEGIN MID Track #2761 - sync Velocity & Store Detail windows
                _velocityMethod.CalculateAverageUsingChain = true;
                // begin TT#587 Matrix Totals Wrong
                if (_matrixProcessed)
                {
                    if (_averageReset)
                    {
                        _averageReset = false;
                        return;
                    }
                    if (!ReprocessWarningOK(this.gbxAverage.Text))
                    {
                        _averageReset = true;
                        _velocityMethod.CalculateAverageUsingChain = true;
                    }
                    // Begin TT#975 - RMatelic - Velocity Detail Screen when selecting  "Set"  from the top left corner selection for "All Store" or "Set"  received a Null Reference Exception
                    else
                    {
                        if (_updateFromStoreDetail)
                        {
                            btnView_Click(btnView, null);   // This will Process Interactive
                        }
                    }
                    // End TT#975
                }
                // end TT#587 Matrix Totals Wrong
                ChangePending = true;
                CheckStoreDetail();
            }	// END MID Track #2761 
        }

        private void radAvgSet_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!FormLoaded) return;
            if (radAvgSet.Checked)
            {	// BEGIN MID Track #2761 - sync Velocity & Store Detail windows
                //_velocityMethod.CalculateAverageUsingChain = false;
                // begin TT#587 Matrix Totals Wrong
                if (_matrixProcessed)
                {
                    if (_averageReset)
                    {
                        _averageReset = false;
                        return;
                    }
                    if (!ReprocessWarningOK(this.gbxAverage.Text))
                    {
                        _averageReset = true;
                        _velocityMethod.CalculateAverageUsingChain = false;
                    }
                    // Begin TT#975 - RMatelic - Velocity Detail Screen when selecting  "Set"  from the top left corner selection for "All Store" or "Set"  received a Null Reference Exception
                    else
                    {
                        if (_updateFromStoreDetail)
                        {
                            btnView_Click(btnView, null);   // This will Process Interactive
                        }
                    }
                    // End TT#975
                }
                // end TT#587 Matrix Totals Wrong
                ChangePending = true;
                CheckStoreDetail();
            }	// END MID Track #2761 
        }

        // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
        private void radApplyMinMaxNone_CheckedChanged(object sender, System.EventArgs e)
        {
            // BEGIN TT#987 - Velocity Min/Max changes not re-setting matirx data
            //if (!FormLoaded) return;
            //if (radApplyMinMaxNone)
            //{
            //    _ApplyMinMaxInd = 'N';
            //    SetSpecificFields();
            //    if (_trans != null)
            //    { UpdateTransData(); }
            //    ChangePending = true;
            //    CheckStoreDetail();
            //}
            // BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
            //if (FormLoaded && radApplyMinMaxNone.Checked == true)
            if (FormLoaded && (radApplyMinMaxNone.Checked == true) && (radApplyMinMaxNone.Focused == false))
            { _ApplyMinMaxInd = 'N';
            gbxMinMaxOpt.Enabled = false;    // TT#1287 - AGallagher - Inventory Min/Max 
            }
            if (FormLoaded && (radApplyMinMaxNone.Checked == true) && (radApplyMinMaxNone.Focused == true))
            {
                _ApplyMinMaxInd = 'N';
                gbxMinMaxOpt.Enabled = false;    // TT#1287 - AGallagher - Inventory Min/Max 
                //ChangePending = true;
                if (_matrixProcessed)
                {
                    // BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                    //if (cbxInteractive.Checked)
                    //{
                    //    cbxInteractive.Checked = false;
                    //    cbxInteractive.Checked = true;
                    //}
                    CheckInteractive();
                    // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                }
                // END TT#987 - Velocity Min/Max changes not re-setting matirx data
            }
            // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
        }

        private void radApplyMinMaxStore_CheckedChanged(object sender, System.EventArgs e)
        {
            // BEGIN TT#987 - Velocity Min/Max changes not re-setting matirx data
            //if (!FormLoaded) return;
            //if (radApplyMinMaxStore.Checked)
            //{
            //    _ApplyMinMaxInd = 'S';
            //    SetSpecificFields();
            //    if (_trans != null)
            //    { UpdateTransData(); }
            //    ChangePending = true;
            //    CheckStoreDetail();
            //}
            // BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
            //if (FormLoaded && radApplyMinMaxStore.Checked == true)
            if (FormLoaded && (radApplyMinMaxStore.Checked == true) && (radApplyMinMaxStore.Focused == false))
            { _ApplyMinMaxInd = 'S'; 
            gbxMinMaxOpt.Enabled = false;    // TT#1287 - AGallagher - Inventory Min/Max 
            }
            //ChangePending = true;
            if (FormLoaded && (radApplyMinMaxStore.Checked == true) && (radApplyMinMaxStore.Focused == true))
            {
                _ApplyMinMaxInd = 'S';
                gbxMinMaxOpt.Enabled = false;    // TT#1287 - AGallagher - Inventory Min/Max 
                if (_matrixProcessed)
                {
                    // BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                    //if (cbxInteractive.Checked)
                    //{
                    //    cbxInteractive.Checked = false;
                    //    cbxInteractive.Checked = true;
                    //}
                    CheckInteractive();
                    // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                }
                // END TT#987 - Velocity Min/Max changes not re-setting matirx data
            }
            // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
        }
        private void radApplyMinMaxVelocity_CheckedChanged(object sender, System.EventArgs e)
        {
            // BEGIN TT#987 - Velocity Min/Max changes not re-setting matirx data
            //if (!FormLoaded) return;
            //if (radApplyMinMaxVelocity.Checked)
            //{
            //    _ApplyMinMaxInd = 'V';
            //    SetSpecificFields();
            //    if (_trans != null)
            //    { UpdateTransData(); }
            //    ChangePending = true;
            //    CheckStoreDetail();
            //}
            // BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
            //if (FormLoaded && radApplyMinMaxVelocity.Checked == true)
            if (FormLoaded && (radApplyMinMaxVelocity.Checked == true) && (radApplyMinMaxVelocity.Focused == false))
            { _ApplyMinMaxInd = 'V'; 
            gbxMinMaxOpt.Enabled = true;    // TT#1287 - AGallagher - Inventory Min/Max 
            }
            if (FormLoaded && (radApplyMinMaxVelocity.Checked == true) && (radApplyMinMaxVelocity.Focused == true))
            {
                _ApplyMinMaxInd = 'V';
                gbxMinMaxOpt.Enabled = true;    // TT#1287 - AGallagher - Inventory Min/Max 
                //ChangePending = true;
                if (_matrixProcessed)
                {
                    // BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                    //if (cbxInteractive.Checked)
                    //{
                    //    cbxInteractive.Checked = false;
                    //    cbxInteractive.Checked = true;
                    //}
                    CheckInteractive();
                    // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                }
                // END TT#987 - Velocity Min/Max changes not re-setting matirx data
            }
            // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
        }
        // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)

        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        private void radAllocationMinMax_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FormLoaded && (radAllocationMinMax.Checked == true) && (radAllocationMinMax.Focused == false))
            { 
                _InventoryInd = 'A';
                cboInventoryBasis.Enabled = false;
                cboInventoryBasis.Text = null;
            }
            if (FormLoaded && (radAllocationMinMax.Checked == true) && (radAllocationMinMax.Focused == true))
            {
                _InventoryInd = 'A';
                cboInventoryBasis.Enabled = false;
                cboInventoryBasis.Text = null;

                if (_matrixProcessed)
                {
                   CheckInteractive();
                }
            }
        }

        private void radInventoryMinMax_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FormLoaded && (radInventoryMinMax.Checked == true) && (radInventoryMinMax.Focused == false))
            {
                _InventoryInd = 'I';
                cboInventoryBasis.Enabled = true;
                LoadMerchandiseCombo();
                LoadGenAllocValues();
            }
            if (FormLoaded && (radInventoryMinMax.Checked == true) && (radInventoryMinMax.Focused == true))
            {
                _InventoryInd = 'I';
                cboInventoryBasis.Enabled = true;
                LoadMerchandiseCombo();
                LoadGenAllocValues();
                if (_matrixProcessed)
                {
                    CheckInteractive();
                }
            }
        }
        // END TT#1287 - AGallagher - Inventory Min/Max

        // BEGIN MID Track #2761 - sync Velocity & Store Detail windows
        private void CheckStoreDetail()
        {
            // Begin TT#983 - RMatelic - Velocity Detail Screen when selecting All Stores or Set user is prompted that the matrix will be reprocessed, but the grid is not cleared entirely
            //if (_matrixProcessed)
            if (_matrixProcessed && !_updateFromStoreDetail)
            // End TT#983
            {
                _trans.VelocityCalculateAverageUsingChain = _velocityMethod.CalculateAverageUsingChain;
                GetStoreData();
                // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
                ApplyViewToGridLayout(Convert.ToInt32(cboMatrixView.SelectedValue, CultureInfo.CurrentUICulture));
                // End TT#231  
                btnChanges.Enabled = true;
                if (!_updateFromStoreDetail && InInteractiveMode())
                {
                    MIDRetail.Windows.StyleView frmStyleView;
                    frmStyleView = (MIDRetail.Windows.StyleView)_trans.StyleView;
                    frmStyleView.UpdateFromVelocityWindow();
                }
            }
        }
        // END MID Track #2761 

        private void radShipBasis_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!FormLoaded) return;
            if (radShipBasis.Checked)
            {
                if (_matrixProcessed)
                {
                    if (_shipReset)
                    {
                        _shipReset = false;
                        return;
                    }
                    if (!ReprocessWarningOK(this.gbxShip.Text))
                    {
                        _shipReset = true;
                        radHeaderStyle.Checked = true;
                    }
                }
                ChangePending = true;
            }

        }

        private void radHeaderStyle_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!FormLoaded) return;
            if (radHeaderStyle.Checked)
            {
                if (_matrixProcessed)
                {
                    if (_shipReset)
                    {
                        _shipReset = false;
                        return;
                    }
                    if (!ReprocessWarningOK(this.gbxShip.Text))
                    {
                        _shipReset = true;
                        radShipBasis.Checked = true;
                    }
                }
                ChangePending = true;
            }

        }

        private void cboComponent_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            int idx;
            try
            {
                if (FormLoaded)
                {
                    if (_matrixProcessed)
                    {
                        if (_compReset)
                        {
                            _compReset = false;
                            return;
                        }
                        idx = this.cboComponent.SelectedIndex;
                        if (!ReprocessWarningOK(this.lblComponent.Text))
                        {
                            _compReset = true;
                            this.cboComponent.SelectedIndex = _prevCompIndex;
                            return;
                        }
                        else
                            this.cboComponent.SelectedIndex = idx;
                    }
                }
                if (this.cboComponent.SelectedValue != null)
                {
                    _prevCompIndex = Convert.ToInt32(this.cboComponent.SelectedIndex, CultureInfo.CurrentUICulture);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }
        //begin MID Track #2449 Apply Changes button not enabled when qty changed
        // This code was actually here but was not connected to the control.  
        // Used Designer to designate this event for the control 
        private void txtNoOHQuantity_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (_matrixProcessed)
                btnChanges.Enabled = true;
        }
        //end MID Track #2449
        private void txtNoOHQuantity_TextChanged(object sender, System.EventArgs e)
        {
            int setValue;
            double qty;
            try
            {
                if (!ValidQuantity(txtNoOHQuantity))
                {
                    string text = MIDText.GetTextOnly(eMIDTextCode.msg_ErrorsFoundReviewCorrect);
                    MessageBox.Show(text);
                    return;
                }
                if (txtNoOHQuantity.Text.Trim().Length == 0)
                    return;
                qty = Convert.ToDouble(txtNoOHQuantity.Text, CultureInfo.CurrentUICulture);
                if (_matrixProcessed)
                {
                    setValue = Convert.ToInt32(this.cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
                    _trans.VelocitySetMatrixNoOnHandRuleQty(setValue, qty);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnChanges_Click(object sender, System.EventArgs e)
        {
            MIDRetail.Windows.StyleView frmStyleView;
            try
            {
                //ErrorFound = ValidateAndSetFields();
                ErrorFound = InteractiveValidateandSetFields();
                if (!ErrorFound)
                {
                    this.Cursor = Cursors.WaitCursor;
                    _trans.Velocity.Balance = cbxBalance.Checked; // TT#406 Unhandled exception when check/unchecking balance
                    _trans.VelocityApplyMatrixChanges();
                    _statsCalculated = true;
                    GetStoreData();
                    // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
                    ApplyViewToGridLayout(Convert.ToInt32(cboMatrixView.SelectedValue, CultureInfo.CurrentUICulture));
                    // End TT#231  

                    // begin TT#406 Unhandled exception when check/unchecking balance
                    //if (_balancechanged == true)
                    //{
                    //    ProcessInteractive(false);
                    //}
                    // end TT#406 Unhandled exception when check/unchecking balance

                    if (InInteractiveMode())
                    {
                        frmStyleView = (MIDRetail.Windows.StyleView)_trans.StyleView;
                        frmStyleView.ReloadGridData();
                    }
                    this.btnChanges.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            this.Cursor = Cursors.Default;

            ugMatrix_AddTotalLine(false);
            _balancechanged = false;

            //BypassSecurityEnqueueCheck = true; //tt#311 - bypass enqueue checks - apicchetti  // TT#1185 - Verify ENQ before Update

        }

        // Begin Track #4872 - JSmith - Global/User Attributes
        override protected void BuildAttributeList()
        {
            ProfileList profileList;
            int currValue;
            try
            {
                if (cboStoreAttribute.SelectedValue != null &&
                    cboStoreAttribute.SelectedValue.GetType() == typeof(System.Int32))
                {
                    currValue = Convert.ToInt32(cboStoreAttribute.SelectedValue);
                }
                else
                {
                    currValue = Include.NoRID;
                }
                profileList = GetStoreGroupList(_velocityMethod.Method_Change_Type, _velocityMethod.GlobalUserType, false);
                cboStoreAttribute.Initialize(SAB, FunctionSecurity, profileList.ArrayList, _velocityMethod.GlobalUserType == eGlobalUserType.User);
                if (currValue != Include.NoRID)
                {
                    cboStoreAttribute.SelectedValue = currValue;
                }
            }
            catch
            {
                throw;
            }
        }
        // End Track #4872

        override protected bool ApplySecurity()	// Track 5871 stodd
        {
            bool securityOk = true; // track #5871 stodd
            if (FunctionSecurity.AllowUpdate)
            {
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
            }

            if (FunctionSecurity.AllowExecute)
            {
                btnProcess.Enabled = true;
            }
            else
            {
                btnProcess.Enabled = false;
            }
            return securityOk;	// track 5871 stodd
        }

        private void gbxAverage_Enter(object sender, System.EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, System.EventArgs e)
        {

        }

        private void tabVelocity_DragEnter(object sender, DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void tabVelocity_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        private void ugWorkflows_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
            //ugld.ApplyDefaults(e);
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
            //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
            // End TT#1164
            //End TT#169
        }

        // Begin Track #4872 - JSmith - Global/User Attributes
        private void frmVelocityMethod_Load(object sender, EventArgs e)
        {
            if (cboStoreAttribute.ReplaceAttribute)
            {
                ChangePending = true;
            }
            if (SAB.ClientServerSession.GlobalOptions.EnableVelocityGradeOptions == false)
            {
                this.gbxGrade.Visible = false;
            }
        }

        private void cboStoreAttribute_DragEnter(object sender, DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void cboStoreAttribute_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        // Begin Track #6074
        // Begin TT # 91 - stodd
        //private void cbxBasisGrades_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (FormLoaded)
        //        {
        //            if (_matrixProcessed)
        //            {
        //                if (_gradesByBasisReset)
        //                {
        //                    _gradesByBasisReset = false;
        //                    return;
        //                }
        //                if (!ReprocessWarningOK(this.cbxBasisGrades.Text))
        //                {
        //                    _gradesByBasisReset = true;
        //                    this.cbxBasisGrades.Checked = !this.cbxBasisGrades.Checked;
        //                    return;
        //                }
        //            }
        //            ChangePending = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}
        // End TT # 91 - stodd
        // End Track #6074

        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        private void LoadMerchandiseCombo()
        {
            try
            {
                cboInventoryBasis.DataSource = MerchandiseDataTable;
                cboInventoryBasis.DisplayMember = "text";
                cboInventoryBasis.ValueMember = "seqno";
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }
            private void LoadGenAllocValues()
		{
			try
			{
                //if (_velocityMethod.Method_Change_Type == eChangeType.update)
                if (_velocityMethod.Method_Change_Type == eChangeType.update || _velocityMethod.Method_Change_Type == eChangeType.add)
				{
					//Load Merchandise Node or Level Text to combo box
					HierarchyNodeProfile hnp;
					if (_velocityMethod.MERCH_HN_RID != Include.NoRID)
					{
                        hnp = SAB.HierarchyServerSession.GetNodeData(_velocityMethod.MERCH_HN_RID, true, true);
						AddNodeToMerchandiseCombo3 ( hnp );
					}
					else
					{ 
						if (_velocityMethod.MERCH_PH_RID != Include.NoRID)
							SetComboToLevel(_velocityMethod.MERCH_PHL_SEQ);
						else
							cboInventoryBasis.SelectedIndex = 0;
					}
				}	
						
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
        // END TT#1287 - AGallagher - Inventory Min/Max


        private void cbxBalance_CheckedChanged(object sender, EventArgs e)
        {
            _velocityMethod.Balance = cbxBalance.Checked;

            // BEGIN TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
            if (cbxBalance.Checked == true)
            {
                cbxBalanceToHeader.Checked = false; //TT#855-MD -jsobek -Velocity Enhancements 
                cbxReconcile.Checked = true;
                cbxReconcile.Enabled = false;
                _velocityMethod.Reconcile = cbxReconcile.Checked;
            }
            else
            { cbxReconcile.Enabled = true; }
            // END TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance

            _balancechanged = true; //tt#290 - Velocity balance check issues - apicchetti

            // BEGIN TT#842 - AGallagher - Velocity - Velocity Balance - with Layers
            // BEGIN TT#1015 - AGallagher - Velocity - Apply Change and Proicess Interactive buttons
            // if (_processintctr > 0)
            // if (_processintctr > 0 && btnView.Enabled == false)
            // END TT#1015 - AGallagher - Velocity - Apply Change and Proicess Interactive buttons
            //{
            //  btnChanges.Enabled = true; //tt#290 - Velocity balance check issues - apicchetti
            //}
            // BEGIN TT#1103 - AGallagher - Velocity Interactive Received Null Reference when deselecting the Balance and Applying Changes.
            // btnChanges.Enabled = true;  // TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
            if (_selectedHeaderList.Count > 0 && _processintctr > 0 && cbxInteractive.Checked == true)
            { btnChanges.Enabled = true; } 
            // END TT#1103 - AGallagher - Velocity Interactive Received Null Reference when deselecting the Balance and Applying Changes.
            
            if (btnView.Enabled == false && cbxInteractive.Checked == true)
            {
                btnView.Enabled = true;
                cbxInteractive.Checked = false;
                cbxInteractive.Checked = true;
                ChangePending = true;
                btnChanges.Enabled = false;  // TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
            }
            else
                if (_processintctr > 0 && btnView.Enabled == false)
                {
                    btnChanges.Enabled = true;
                }
            // END TT#842 - AGallagher - Velocity - Velocity Balance - with Layers
        }

        // End Track #4872


        // BEGIN TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
        private void cbxReconcile_CheckedChanged(object sender, EventArgs e)
        {
            _velocityMethod.Reconcile = cbxReconcile.Checked;

            // BEGIN TT#1103 - AGallagher - Velocity Interactive Received Null Reference when deselecting the Balance and Applying Changes.
            // btnChanges.Enabled = true;
            if (_selectedHeaderList.Count > 0 && _processintctr > 0 && cbxInteractive.Checked == true)
            { btnChanges.Enabled = true; } 
            // END TT#1103 - AGallagher - Velocity Interactive Received Null Reference when deselecting the Balance and Applying Changes.
            if (btnView.Enabled == false && cbxInteractive.Checked == true)
            {
                btnView.Enabled = true;
                cbxInteractive.Checked = false;
                cbxInteractive.Checked = true;
                ChangePending = true;
                btnChanges.Enabled = false;
            }
            else
                if (_processintctr > 0 && btnView.Enabled == false)
                {
                    btnChanges.Enabled = true;
                }
        }
        // END TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance

        //Begin TT#855-MD -jsobek -Velocity Enhancements 
        private void cbxBalanceToHeader_CheckedChanged(object sender, EventArgs e)
        {
            
            if (cbxBalanceToHeader.Checked) 
            {
                _velocityMethod.BalanceToHeaderInd = '1';
                this.cbxBalance.Checked = false;
                cbxReconcile.Checked = true;
                cbxReconcile.Enabled = false;
            }
            else
            {
                _velocityMethod.BalanceToHeaderInd = '0';
                cbxReconcile.Enabled = true;
            }
            
            CheckInteractive();
            //if (FormLoaded == false)
            //{
            //    return; //do nothing if the form is not loaded
            //}
            //if (rdoMatrixModeBalanceHeader.Checked == true)
            //{
            //    DisableAverageRuleUI();
            //    gbxMatrixDefault.Enabled = false;
            //    CheckInteractive();
            //    SetMatrixActivation();
            //}
        }
        //End TT#855-MD -jsobek -Velocity Enhancements 

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        private void cboMatrixView_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                int viewRID;
                if (!_bindingView)
                {
                    viewRID = Convert.ToInt32(cboMatrixView.SelectedValue, CultureInfo.CurrentUICulture);
                    if (viewRID != _lastSelectedViewRID)
                    {
                        _lastSelectedViewRID = viewRID;
                        ApplyViewToGridLayout(viewRID);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // End TT#231 

        //Begin TT#262 - added to tell the form what function it is being activated from
        bool _activateFromStyleView = false;
        public bool ActivatedFromStyleView
        {
            get
            {
                return _activateFromStyleView;
            }

            set
            {
                _activateFromStyleView = value;
            }
        }

        //Ende TT#262 - added to tell the form what function it is being activated from

        //Begin TT#855-MD -jsobek -Velocity Enhancements 
        private void DisableAverageRuleUI()
        {
            cboMatrixModeAvgRule.Enabled = false;
            txtMatrixModeAvgRule.Enabled = false;
            gbxSpreadOption.Enabled = false;

            rdoSpreadOptionSmooth.Checked = true;
            txtMatrixModeAvgRule.Text = string.Empty;
            cboMatrixModeAvgRule.Text = string.Empty;
            cboMatrixModeAvgRule.Text = null;
            txtMatrixModeAvgRule.Text = null;
        }
        private void EnableAverageRuleUI()
        {
            cboMatrixModeAvgRule.Enabled = true;
            txtMatrixModeAvgRule.Enabled = true; 
            gbxSpreadOption.Enabled = true;
        }
  
        private void rdoMatrixModeNormal_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded == false)
            {
                return; //do nothing if the form is not loaded
            }
            if (rdoMatrixModeNormal.Checked == true)
            {
                DisableAverageRuleUI();
                gbxMatrixDefault.Enabled = true;
                CheckInteractive();
                SetMatrixActivation();
            }
        }

     

        private void rdoMatrixModeAverage_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded == false)
            {
                return; //do nothing if the form is not loaded
            }
            if (rdoMatrixModeAverage.Checked == true)
            {
                if (MatrixModeWarningOK(this.gbxMatrixMode.Text))
                {
                    EnableAverageRuleUI();
                    gbxMatrixDefault.Enabled = false;
                    CheckInteractive();
                    SetMatrixActivation();
                }
                else //revert to normal matrix mode
                {
                    rdoMatrixModeNormal.Checked = true;
                }
            }
        }

        private void rdoGradeVariableSales_CheckedChanged(object sender, EventArgs e)
        {
            if (radGradeVariableSales.Checked)
            {
                CheckInteractive();
            }
        }
        private void rdoGradeVariableStock_CheckedChanged(object sender, EventArgs e)
        {
            if (radGradeVariableStock.Checked)
            {
                CheckInteractive();
            }
        }
        //End TT#855-MD -jsobek -Velocity Enhancements 

        //Begin TT#637 - Velocity - Spread Average - APicchetti
        // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing

        //private void rdoMatrixMode_CheckedChanged(object sender, EventArgs e)
        //{
        //    // BEGIN TT#725-MD - AGallagher - Velocity - Velocity when Apply changes is selected the changes are not applied.  The user must deselect and select Activate Matrix and process interactive.  User should be able to Apply Changes.
        //    //if (FormLoaded && (rdoMatrixModeNormal.Checked == true) && (rdoMatrixModeNormal.Focused == true))
        //    //{

        //    //    rdoMatrixModeNormal.Checked = true;
        //    //    _matrixModeChanged = false;
        //    //    cboMatrixModeAvgRule.Enabled = false;
        //    //    txtMatrixModeAvgRule.Enabled = false;
        //    //    // Begin TT#3033/TT#671-MD - RMatelic Velocity Method - Avg Matrix Mode - Greyed Out
        //    //    //ugMatrix.Enabled = true;
        //    //    // End TT#3033/TT#671-MD  
        //    //    gbxSpreadOption.Enabled = false;
        //    //    rdoSpreadOptionSmooth.Checked = true;
        //    //    txtMatrixModeAvgRule.Text = string.Empty;
        //    //    cboMatrixModeAvgRule.Text = string.Empty;
        //    //    cboMatrixModeAvgRule.Text = null;
        //    //    txtMatrixModeAvgRule.Text = null;
        //    //    gbxMatrixDefault.Enabled = true;
        //    //    CheckInteractive();

        //    //}
        //    //// END TT#725-MD - AGallagher - Velocity - Velocity when Apply changes is selected the changes are not applied.  The user must deselect and select Activate Matrix and process interactive.  User should be able to Apply Changes.
        //    //// BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
        //    ////Begin TT#855-MD -jsobek -Velocity Enhancements -FIX for pre-existing bug??
        //    ////if (FormLoaded && (rdoMatrixModeNormal.Checked == true) && (rdoMatrixModeAverage.Focused == false))
        //    //if (FormLoaded && (rdoMatrixModeNormal.Checked == true) && (rdoMatrixModeAverage.Focused == false))
        //    ////End TT#855-MD -jsobek -Velocity Enhancements -FIX for pre-existing bug??
        //    //{

        //    //    rdoMatrixModeNormal.Checked = true;
        //    //    _matrixModeChanged = false;
        //    //    cboMatrixModeAvgRule.Enabled = false;
        //    //    txtMatrixModeAvgRule.Enabled = false;
        //    //    // Begin TT#3033/TT#671-MD - RMatelic Velocity Method - Avg Matrix Mode - Greyed Out
        //    //    //ugMatrix.Enabled = true;
        //    //    // End TT#3033/TT#671-MD  
        //    //    gbxSpreadOption.Enabled = false;
        //    //    rdoSpreadOptionSmooth.Checked = true;
        //    //    txtMatrixModeAvgRule.Text = string.Empty;
        //    //    cboMatrixModeAvgRule.Text = string.Empty;
        //    //    cboMatrixModeAvgRule.Text = null;
        //    //    txtMatrixModeAvgRule.Text = null;
        //    //    gbxMatrixDefault.Enabled = true;
        //    //    //CheckInteractive();

        //    //}

        //    ////Begin TT#855-MD -jsobek -Velocity Enhancements
        //    //if (FormLoaded && (rdoMatrixModeBalanceHeader.Checked == true) && (rdoMatrixModeBalanceHeader.Focused == true))
        //    //{
        //    //    rdoMatrixModeBalanceHeader.Checked = true;
        //    //    _matrixModeChanged = false;
        //    //    cboMatrixModeAvgRule.Enabled = false;
        //    //    txtMatrixModeAvgRule.Enabled = false;

        //    //    gbxSpreadOption.Enabled = false;
        //    //    rdoSpreadOptionSmooth.Checked = true;
        //    //    txtMatrixModeAvgRule.Text = string.Empty;
        //    //    cboMatrixModeAvgRule.Text = string.Empty;
        //    //    cboMatrixModeAvgRule.Text = null;
        //    //    txtMatrixModeAvgRule.Text = null;
        //    //    gbxMatrixDefault.Enabled = true;
        //    //    CheckInteractive();
        //    //}
        //    //if (FormLoaded && (rdoMatrixModeBalanceHeader.Checked == true) && (rdoMatrixModeBalanceHeader.Focused == false))
        //    //{
        //    //    rdoMatrixModeNormal.Checked = true;
        //    //    _matrixModeChanged = false;
        //    //    cboMatrixModeAvgRule.Enabled = false;
        //    //    txtMatrixModeAvgRule.Enabled = false;

        //    //    gbxSpreadOption.Enabled = false;
        //    //    rdoSpreadOptionSmooth.Checked = true;
        //    //    txtMatrixModeAvgRule.Text = string.Empty;
        //    //    cboMatrixModeAvgRule.Text = string.Empty;
        //    //    cboMatrixModeAvgRule.Text = null;
        //    //    txtMatrixModeAvgRule.Text = null;
        //    //    gbxMatrixDefault.Enabled = true;
        //    //    //CheckInteractive();
        //    //}
        //    ////End TT#855-MD -jsobek -Velocity Enhancements

        //    //if (FormLoaded && (rdoMatrixModeNormal.Checked == true) && (rdoMatrixModeAverage.Focused == true))
        //    //{

        //    //    rdoMatrixModeNormal.Checked = true;
        //    //    _matrixModeChanged = false;
        //    //    cboMatrixModeAvgRule.Enabled = false;
        //    //    txtMatrixModeAvgRule.Enabled = false;
        //    //    // Begin TT#3033/TT#671-MD - RMatelic Velocity Method - Avg Matrix Mode - Greyed Out
        //    //    //ugMatrix.Enabled = true;
        //    //    // End TT#3033/TT#671-MD  
        //    //    gbxSpreadOption.Enabled = false;
        //    //    rdoSpreadOptionSmooth.Checked = true;
        //    //    txtMatrixModeAvgRule.Text = string.Empty;
        //    //    cboMatrixModeAvgRule.Text = string.Empty;
        //    //    cboMatrixModeAvgRule.Text = null;
        //    //    txtMatrixModeAvgRule.Text = null;
        //    //    gbxMatrixDefault.Enabled = true;
        //    //    CheckInteractive();

        //    //}

        //    //if (FormLoaded && (rdoMatrixModeAverage.Checked == true) && (rdoMatrixModeAverage.Focused == false))
        //    //{
        //    //    cboMatrixModeAvgRule.Enabled = true;
        //    //    txtMatrixModeAvgRule.Enabled = true;
        //    //    // Begin TT#3033/TT#671-MD - RMatelic Velocity Method - Avg Matrix Mode - Greyed Out
        //    //    //ugMatrix.Enabled = false;
        //    //    // End TT#3033/TT#671-MD  
        //    //    gbxSpreadOption.Enabled = true;
        //    //    gbxMatrixDefault.Enabled = false;
        //    //    _matrixModeChanged = true;
        //    //    //CheckInteractive();
        //    //}

        //    //if (FormLoaded && (rdoMatrixModeAverage.Checked == true) && (rdoMatrixModeAverage.Focused == true))
        //    //{
        //    //    if (_matrixModeChanged == false)
        //    //    {
        //    //        if (MatrixModeWarningOK(this.gbxMatrixMode.Text))
        //    //        {
        //    //            cboMatrixModeAvgRule.Enabled = true;
        //    //            txtMatrixModeAvgRule.Enabled = true;
        //    //            // Begin TT#3033/TT#671-MD - RMatelic Velocity Method - Avg Matrix Mode - Greyed Out
        //    //            //ugMatrix.Enabled = false;
        //    //            // End TT#3033/TT#671-MD  
        //    //            gbxSpreadOption.Enabled = true;
        //    //            gbxMatrixDefault.Enabled = false;
        //    //            _matrixModeChanged = true;
        //    //            CheckInteractive();
        //    //        }
        //    //        else
        //    //        {
        //    //            rdoMatrixModeNormal.Checked = true;
        //    //            _matrixModeChanged = false;
        //    //            cboMatrixModeAvgRule.Enabled = false;
        //    //            txtMatrixModeAvgRule.Enabled = false;
        //    //            // Begin TT#3033/TT#671-MD - RMatelic Velocity Method - Avg Matrix Mode - Greyed Out
        //    //            //ugMatrix.Enabled = true;
        //    //            // End TT#3033/TT#671-MD  
        //    //            gbxSpreadOption.Enabled = false;
        //    //            rdoSpreadOptionSmooth.Checked = true;
        //    //            txtMatrixModeAvgRule.Text = string.Empty;
        //    //            cboMatrixModeAvgRule.Text = string.Empty;
        //    //            cboMatrixModeAvgRule.Text = null;
        //    //            txtMatrixModeAvgRule.Text = null;
        //    //            gbxMatrixDefault.Enabled = true;
        //    //            CheckInteractive();
        //    //        }
        //    //    }
        //    //}

        //    SetMatrixModeUI();

        //    // Begin TT#3033/TT#671-MD - RMatelic Velocity Method - Avg Matrix Mode - Greyed Out
        //    SetMatrixActivation();
        //    // End TT#3033/TT#671-MD  

        //    //if (FormLoaded)
        //    //{
        //    //    if (rdoMatrixModeNormal.Checked == true)
        //    //    {
        //    //        cboMatrixModeAvgRule.Enabled = false;
        //    //        txtMatrixModeAvgRule.Enabled = false;
        //    //        ugMatrix.Enabled = true;
        //    //        gbxSpreadOption.Enabled = false;
        //    //        rdoSpreadOptionSmooth.Checked = true;
        //    //        txtMatrixModeAvgRule.Text = string.Empty;
        //    //        cboMatrixModeAvgRule.Text = string.Empty;
        //    //        cboMatrixModeAvgRule.Text = null;
        //    //        txtMatrixModeAvgRule.Text = null;
        //    //        gbxMatrixDefault.Enabled = true;
        //    //        // BEGIN TT#960 - AGallagher - Velocity - Spread Average (#7) - Screen Processing Oversites
        //    //        _matrixModeChanged = false;
        //    //        cbxInteractive.Checked = false;
        //    //        cbxInteractive.Checked = true;
        //    //        ChangePending = true;
        //    //        // END TT#960 - AGallagher - Velocity - Spread Average (#7) - Screen Processing Oversites
        //    //    }
        //    //    else if (rdoMatrixModeAverage.Checked == true)
        //    //    {
        //    //        // BEGIN TT#960 - AGallagher - Velocity - Spread Average (#7) - Screen Processing Oversites
        //    //        //if (_attributeSetChanged == false)
        //    //        if (_matrixModeChanged == false)
        //    //        // END TT#960 - AGallagher - Velocity - Spread Average (#7) - Screen Processing Oversites
        //    //        {
        //    //            if (MatrixModeWarningOK(this.gbxMatrixMode.Text))
        //    //            {
        //    //                cboMatrixModeAvgRule.Enabled = true;
        //    //                txtMatrixModeAvgRule.Enabled = true;
        //    //                ugMatrix.Enabled = false;
        //    //                gbxSpreadOption.Enabled = true;
        //    //                gbxMatrixDefault.Enabled = false;
        //    //                // BEGIN TT#960 - AGallagher - Velocity - Spread Average (#7) - Screen Processing Oversites
        //    //                _matrixModeChanged = true;
        //    //                cbxInteractive.Checked = false;
        //    //                cbxInteractive.Checked = true;
        //    //                ChangePending = true;
        //    //                // END TT#960 - AGallagher - Velocity - Spread Average (#7) - Screen Processing Oversites
        //    //            }
        //    //            else
        //    //            {
        //    //                rdoMatrixModeNormal.Checked = true;
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //    // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)    
        //}
        // END TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing

        private void cboMatrixModeAvgRule_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // BEGIN TT#960 - AGallagher - Velocity - Spread Average (#7) - Screen Processing Oversites
            // txtMatrixModeAvgRule.Text = null;
            // BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
            //if (FormLoaded && rdoMatrixModeAverage.Checked == true)
            if (FormLoaded && (rdoMatrixModeAverage.Checked == true) && (cboMatrixModeAvgRule.Focused == true))
                // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                if (_matrixProcessed)
                {
                    // BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                    //if (cbxInteractive.Checked)
                    //{
                    //    cbxInteractive.Checked = false;
                    //    cbxInteractive.Checked = true;
                    //    ChangePending = true;
                    //}
                    CheckInteractive();
                    // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                }
            // END TT#960 - AGallagher - Velocity - Spread Average (#7) - Screen Processing Oversites
        }
        //End TT#637 - Velocity - Spread Average - APicchetti


        // BEGIN TT#960 - AGallagher - Velocity - Spread Average (#7) - Screen Processing Oversites 
        private void txtMatrixModeAvgRule_TextChanged(object sender, System.EventArgs e)
        {
            // BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
            //if (FormLoaded && rdoMatrixModeAverage.Checked == true)
            if (FormLoaded && (rdoMatrixModeAverage.Checked == true) && (txtMatrixModeAvgRule.Focused == true))
            // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
            {
                // BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                //double quantity;
                //string errorMessage = null;
                //bool errorFound = false;
                //if (txtMatrixModeAvgRule.Text.Trim().Length == 0)
                //{
                //    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                //    ErrorProvider.SetError(txtMatrixModeAvgRule, errorMessage);
                //    errorFound = true;  // TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                //}
                //if (errorFound == false)
                //{
                //    quantity = Convert.ToDouble(txtMatrixModeAvgRule.Text, CultureInfo.CurrentUICulture);
                //    if (quantity < 1)
                //    {
                //        errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
                //        ErrorProvider.SetError(txtMatrixModeAvgRule, errorMessage);
                //        errorFound = true;
                //    }
                //}
                // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                if (_matrixProcessed)
                {
                    // BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                    //if (cbxInteractive.Checked)
                    //{
                    //    cbxInteractive.Checked = false;
                    //    cbxInteractive.Checked = true;
                    //    ChangePending = true;
                    //}
                    CheckInteractive();
                    // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                }
            }
        }

        private void rdoSpreadOptionIdx_CheckedChanged(object sender, EventArgs e)
        {
            // BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
            //if (FormLoaded && rdoMatrixModeAverage.Checked == true)
            if (FormLoaded && (rdoSpreadOptionIdx.Checked == true) && (rdoSpreadOptionIdx.Focused == true))
                // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                if (_matrixProcessed)
                {
                    // BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                    //if (cbxInteractive.Checked)
                    //{
                    //    cbxInteractive.Checked = false;
                    //    cbxInteractive.Checked = true;
                    //    ChangePending = true;
                    //}
                    CheckInteractive();
                    // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                }
        }
        private void rdoSpreadOptionSmooth_CheckedChanged(object sender, EventArgs e)
        {
            // BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
            //if (FormLoaded && rdoMatrixModeAverage.Checked == true)
            if (FormLoaded && (rdoSpreadOptionSmooth.Checked == true) && (rdoSpreadOptionSmooth.Focused == true))
                // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                if (_matrixProcessed)
                {
                    // BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                    //if (cbxInteractive.Checked)
                    //{
                    //    cbxInteractive.Checked = false;
                    //    cbxInteractive.Checked = true;
                    //    ChangePending = true;
                    //}
                    CheckInteractive();
                    // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
                }
        }
        // END TT#960 - AGallagher - Velocity - Spread Average (#7) - Screen Processing Oversites 


        // BEGIN TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)
		// Begin TT# 4730 - stodd - NullReferenceException in Velocity after opening group as Headers in Style Review
        private void CheckInteractive()
        {
            CheckInteractive(true);
        }
        // END TT#1060 - AGallagher - Velocity - Enhancement #637 (User Conference #7)

        private void CheckInteractive(bool setChangePending)
        {
            if (cbxInteractive.Checked)
            {
                cbxInteractive.Checked = false;
                cbxInteractive.Checked = true;
                if (setChangePending)
                {
                    ChangePending = true;
                }
            }
        }
		// End TT# 4730 - stodd - NullReferenceException in Velocity after opening group as Headers in Style Review

        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        private void cboInventoryBasis_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            if (FormLoaded &&
                !FormIsClosing)
            {
                ErrorProvider.SetError(cboInventoryBasis, string.Empty);
                _lastMerchIndex = cboInventoryBasis.SelectedIndex;
                // BEGIN TT#1562 - AGallagher - Velocity Method the Inventory Basis Setting is does not Save when creating a new method it goes to the default of Style/SKU.
                if (FormLoaded && (cboInventoryBasis.Focused == true))
                { SetSpecificFields(); }
                // END TT#1562 - AGallagher - Velocity Method the Inventory Basis Setting is does not Save when creating a new method it goes to the default of Style/SKU.
                ChangePending = true;
            }
            if (_matrixProcessed)
            {
                CheckInteractive();
            }
        }
        #region "DragNDrop Node to cboInventoryBasis"
        private void cboInventoryBasis_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                // Begin MID Track #4956 - JSmith - Merchandise error (handle wheel mouse and arrow keys)
                if (e.KeyCode == Keys.Up ||
                    e.KeyCode == Keys.Down)
                {
                    return;
                }
                // End MID Track #4956

                _textChanged = true;

                if (_lastMerchIndex == -1)
                {
                    _lastMerchIndex = cboInventoryBasis.SelectedIndex;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cboInventoryBasis_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string errorMessage;

            try
            {
                if (cboInventoryBasis.Text == string.Empty)
                {
                    cboInventoryBasis.SelectedIndex = _lastMerchIndex;
                    _priorError = false;
                }
                else
                {
                    if (_textChanged)
                    {
                        _textChanged = false;

                        HierarchyNodeProfile hnp = GetNodeProfile2(cboInventoryBasis.Text);
                        if (hnp.Key == Include.NoRID)
                        {
                            _priorError = true;

                            errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), cboInventoryBasis.Text);
                            ErrorProvider.SetError(cboInventoryBasis, errorMessage);
                            MessageBox.Show(errorMessage);

                            e.Cancel = true;
                        }
                        else
                        {
                            AddNodeToMerchandiseCombo3(hnp);
                            _priorError = false;
                        }
                    }
                    // JBolles - MID Track #5020 - Prevent errored textbox from resetting its value before error is corrected
                    //					else if (_priorError)
                    //					{
                    //						cboInventoryBasis.SelectedIndex = _lastMerchIndex;
                    //					}
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cboInventoryBasis_Validated(object sender, System.EventArgs e)
        {
            try
            {
                // JBolles - MID Track #5020 - Prevent errored textbox from resetting its value before error is corrected
                if (!_priorError)
                {
                    ErrorProvider.SetError(cboInventoryBasis, string.Empty);
                    _textChanged = false;
                    _priorError = false;
                    _lastMerchIndex = -1;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cboInventoryBasis_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            TreeNodeClipboardList cbList;
            try
            {
                //ClipboardProfile cbp = GetClipboardData(eClipboardDataType.HierarchyNode);
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    //Begin Track #5378 - color and size not qualified
                    //				HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbp.Key);
                    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);
                    //End Track #5378
                    AddNodeToMerchandiseCombo3(hnp);
                    if (FormLoaded)
                    {
                        ChangePending = true;
                        SetSpecificFields(); // TT#1562 - AGallagher - Velocity Method the Inventory Basis Setting is does not Save when creating a new method it goes to the default of Style/SKU.
                    }
                }
            }
            catch (BadDataInClipboardException)
            {
                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private HierarchyNodeProfile GetNodeProfile2(string aProductID)
        {
            string productID;
            string[] pArray;

            try
            {
                productID = aProductID.Trim();
                pArray = productID.Split(new char[] { '[' });
                productID = pArray[0].Trim();

                //				return SAB.HierarchyServerSession.GetNodeData(productID);
                HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
                EditMsgs em = new EditMsgs();
                return hm.NodeLookup(ref em, productID, false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AddNodeToMerchandiseCombo3(HierarchyNodeProfile hnp)
        {
            try
            {
                DataRow myDataRow;
                bool nodeFound = false;
                int nodeRID = Include.NoRID;
                int levIndex;
                for (levIndex = 0;
                    levIndex < MerchandiseDataTable.Rows.Count; levIndex++)
                {
                    myDataRow = MerchandiseDataTable.Rows[levIndex];
                    if ((eMerchandiseType)(Convert.ToInt32(myDataRow["leveltypename"], CultureInfo.CurrentUICulture)) == eMerchandiseType.Node)
                    {
                        nodeRID = (Convert.ToInt32(myDataRow["key"], CultureInfo.CurrentUICulture));
                        if (hnp.Key == nodeRID)
                        {
                            nodeFound = true;
                            break;
                        }
                    }
                }
                if (!nodeFound)
                {
                    myDataRow = MerchandiseDataTable.NewRow();
                    myDataRow["seqno"] = MerchandiseDataTable.Rows.Count;
                    myDataRow["leveltypename"] = eMerchandiseType.Node;
                    myDataRow["text"] = hnp.Text;
                    myDataRow["key"] = hnp.Key;
                    MerchandiseDataTable.Rows.Add(myDataRow);

                    cboInventoryBasis.SelectedIndex = MerchandiseDataTable.Rows.Count - 1;
                }
                else
                {
                    cboInventoryBasis.SelectedIndex = levIndex;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void SetComboToLevel(int seq)
        {
            try
            {
                DataRow myDataRow;
                for (int levIndex = 0;
                    levIndex < MerchandiseDataTable.Rows.Count; levIndex++)
                {
                    myDataRow = MerchandiseDataTable.Rows[levIndex];
                    if (Convert.ToInt32(myDataRow["seqno"], CultureInfo.CurrentUICulture) == seq)
                    {
                        cboInventoryBasis.SelectedIndex = levIndex;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        private void cboInventoryBasis_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            // Begin TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
            Merchandise_DragEnter(sender, e);
            //try
            //{
            //    ObjectDragEnter(e);
            //    Image_DragEnter(sender, e);
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            // End TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
        }

        private void cboInventoryBasis_DragOver(object sender, DragEventArgs e)
        {
            // Begin TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
            //Image_DragOver(sender, e);
            Merchandise_DragOver(sender, e);
            // End TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
        }
              
        #endregion
        // END TT#1287 - AGallagher - Inventory Min/Max

        private void cboInventoryBasis_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboInventoryBasis_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboMatrixModeAvgRule_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboMatrixModeAvgRule_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboOHRule_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboOHRule_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboNoOHRule_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboNoOHRule_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboComponent_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboComponent_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboStoreAttribute_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cbxAttributeSet_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cbxAttributeSet_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboMatrixView_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboMatrixView_SelectionChangeCommitted(source, new EventArgs());
        }

		// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
		private void frmVelocityMethod_FormClosing(object sender, FormClosingEventArgs e)
		{
            // Begin TT#5285 - JSmith - Null reference when apply changes after cancel close
            if (FormIsClosing)  
            {
            // End TT#5285 - JSmith - Null reference when apply changes after cancel close
                if (_trans != null)
                {
                    _trans.Velocity = null;
                }
            // Begin TT#5285 - JSmith - Null reference when apply changes after cancel close
            }
            // End TT#5285 - JSmith - Null reference when apply changes after cancel close
		}
		// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
    }
}

