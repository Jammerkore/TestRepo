// Begin Track #4872 - JSmith - Global/User Attributes
// Renamed cboStoreGroupLevel to cbxStoreGroupLevel so it would not get protected in read only mode.
// End Track #4872 
using System;
using System.IO;
using System.Globalization;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;

using Infragistics.Win.UltraWinGrid;
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{  

	/// <summary>
	/// Summary description for Method.
	/// </summary>
	public class frmOTSPlanMethod : WorkflowMethodFormBase
	{
		#region Member Variables

		private System.Windows.Forms.TabPage tabProperties;
        private System.Windows.Forms.TabPage tabMethod;
        private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TabControl tabOTSMethod;

		// Begin MID Issue 2612 - stodd
		HierarchyNodeSecurityProfile _hierNodeSecurity;
		// End MID Issue 2612 - stodd
		private DataTable _dtSource;           // DataTable containing %contribution Basis info.
		private DataTable _dtSourceTYNode;     // DataTable for Node/Version Plan Basis info for TY.
		private DataTable _dtSourceLYNode;     // DataTable for Node/Version Plan Basis info for LY.
		private DataTable _dtSourceTrendNode;  // DataTable for Node/Version Plan Basis info for Trend.
		private DataTable _dtStoreGrades;
		private DataSet _dsStockMinMax;

		private CalendarDateSelector _frm;
		private bool _resetStoreGroup = false;
		private bool _newSetMethod = false;
		private bool _InitialPopulate = false;
//		private string _strMethodType;
		private string _sglOrigVal;
        //private DataTable _dtForecastVers;
		private MIDRetail.Business.OTSPlanMethod _OTSPlanMethod = null;
		private GroupLevelFunctionProfile _GLFProfile = null;
		private ProfileList _GLFProfileList = null;
		private int _nodeRID = Include.NoRID;
		private UltraGrid _gridMouseDownIsFrom;
		private int _prevFuncTypeValue;
		private int _prevSetValue;
		private string _errors = null;
		private TabPage _currentTYLYTabPage = null;
		private bool _merchValChanged = false;
		private bool _setReset = false;
		private bool _setChanged;
        // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
        private bool _storeGroupLevelChanged;
        // End TT#3
		private int _dummyKey;
        // BEGIN Issue 5420 KJohnson - rdoEqualizeYesTY not holding it value
        private bool _defaultEqualizeTY = false;
        private bool _defaultEqualizeLY = false;
        private bool _defaultEqualizeTrend = false;
        // END Issue 5420 KJohnson
		private bool _defaultChecked = false;
		private int _defaultStoreGroupLevelRid = Include.NoRID;
		private int _defaultFunctionType;
        // Begin TT#2647 - JSmith - Delays in OTS Method
        private GroupLevelFunctionProfile _defaultGLFProfile = null;
        private Dictionary<int, HierarchyNodeProfile> _nodesByRID;
        private Dictionary<string, HierarchyNodeProfile> _nodesByID;
        // End TT#2647 - JSmith - Delays in OTS Method
        // BEGIN TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 
        private char _ApplyTrendOptionsInd;
        private float _ApplyTrendOptionsWOSValue;
        // END TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 
		private StoreGradeList _storeGradeList; 
//		private ProfileList _versionProfList;  Removed. Issue 4858
        //private bool _setRowPosition = true;
		private bool _dragAndDrop = false;
		private bool _loadingSet = false;
        //Begin Track #4371 - JSmith - Multi-level forecasting.
		private int _currentLowLevelNode = Include.NoRID;
		private int _longestBranch = Include.NoRID;
		private int _longestHighestGuest = Include.NoRID;
        //private bool _rowExpanding = false;
		//End Track #4371
		private string _txtInheritedFrom = "Inherited from ";
		private int _currentMerchandiseIndex = -2;
		bool _MerchCellListClose = false;
        // BEGIN MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
        private bool _InitializingRow = false;
        // END MID Track #5954
		// BEGIN TT#696 - Overrid Low Level Model going blank on Close.
		System.Windows.Forms.Form _overrideLowLevelfrm;
		// End TT#696 - Overrid Low Level Model going blank on Close.
		// BEGIN TT#1609 - stodd - null ref changing set.
		private HierarchyNodeProfile _previousHierNode = null;
		// END TT#1609 - stodd - null ref changing set.
        // Begin TT#1647 - JSmith - Mins/max disappearing in the OTS forecast method
        private bool _useDefaultSetTrue = false;
        // End TT#1647
        private int _SelectedValue = -2; //TT#7 - RBeck - Dynamic dropdowns
		/// <summary>
		/// Gets the id of the node.
		/// </summary>
		public int NodeRID 
		{
			get { return _nodeRID ; }
		}

		//		public bool FormLoadError
		//		{
		//			get	{return _FormLoadError;}
		//		}

        // Begin TT#2647 - JSmith - Delays in OTS Method
        private GroupLevelFunctionProfile DefaultGLFProfile
        {
            get 
            {
                if (_defaultGLFProfile == null)
                {
                    _defaultGLFProfile = (GroupLevelFunctionProfile)_GLFProfileList.FindKey(_defaultStoreGroupLevelRid);
                }
                return _defaultGLFProfile; 
            }
            set
            {
                _defaultGLFProfile = value;
            }
        }
        // End TT#2647 - JSmith - Delays in OTS Method

		#region BasisGrid declarations
		//private Bitmap picDynamic;
		//private Bitmap picRelToPlan;
		//private Bitmap picRelToCurrent;
		private Bitmap picInclude;
		private Bitmap picExclude;

		//We need to track the active cell, the active row, 
		//and the "INC_EXC_IND" column of the active row. This is all because we need
		//to display a button for the include/exclude function (instead of using
		//the built-in checkbox). 
		private UltraGridRow m_aRow;
		private UltraGridCell m_includeCell;
		//		private MRSCalendar _mrsCalCont = new MRSCalendar();

		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ImageList Icons;
		private System.Windows.Forms.ContextMenu GridContextMenu;
		private System.Windows.Forms.MenuItem mnuDelete;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Panel pnlTYLYTrend;
		private System.Windows.Forms.TabPage tabPageTY;
		private System.Windows.Forms.TabPage tabPageLY;
		private System.Windows.Forms.TabPage tabPageApplyTrend;
		private Infragistics.Win.UltraWinGrid.UltraGrid gridTYNodeVersion;
		private Infragistics.Win.UltraWinGrid.UltraGrid gridLYNodeVersion;
		private Infragistics.Win.UltraWinGrid.UltraGrid gridTrendNodeVersion;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.MenuItem mnuInsert;
		private System.Windows.Forms.TextBox txtTolerance;
		private System.Windows.Forms.Label lblHigh;
		private System.Windows.Forms.Label lblLow;
		private System.Windows.Forms.TextBox txtHigh;
		private System.Windows.Forms.TextBox txtLow;
		private System.Windows.Forms.RadioButton radNone;
		private System.Windows.Forms.RadioButton radLimits;
		private System.Windows.Forms.RadioButton radTolerance;
		private System.Windows.Forms.Button btnTYIncExc;
		private System.Windows.Forms.Button btnLYIncExc;
		private System.Windows.Forms.Button btnTrendIncExc;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.CheckBox cbxAltLY;
		private System.Windows.Forms.CheckBox cbxAltTrend;
		private System.Windows.Forms.TabControl tabTYLYTrend;
		private System.Windows.Forms.TabPage tabPageCaps;
        private System.Windows.Forms.GroupBox grpTrendCaps;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.CheckBox chkOTSStock;
		private System.Windows.Forms.CheckBox chkOTSSales;
		private System.Windows.Forms.TextBox txtOTSHNDesc;
		private MIDRetail.Windows.Controls.MIDDateRangeSelector midDateRangeSelectorOTSPlan;
		private System.Windows.Forms.TabPage tabSetMethods;
        private System.Windows.Forms.GroupBox grpGroupLevelMethod;
		private System.Windows.Forms.TabControl tabSetMethod;
		private System.Windows.Forms.TabPage tabCriteria;
		private System.Windows.Forms.Label lblWidth30;
        private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label lblGLGroupBy;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel pnlPercentContribution;
		private System.Windows.Forms.GroupBox grpContributionBasis;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdPctContBasis;
		private System.Windows.Forms.Button btnIncExc;
		private System.Windows.Forms.TabPage tabStockMinMax;
		private Infragistics.Win.UltraWinGrid.UltraGrid gridStockMinMax;
		private System.Windows.Forms.CheckBox chkUseDefault;
		private System.Windows.Forms.CheckBox chkClear;
		private System.Windows.Forms.CheckBox chkPlan;
		private System.Windows.Forms.CheckBox chkDefault;
		private System.Windows.Forms.Button btnCopy;
		private System.Windows.Forms.GroupBox gbChainForecast;
		private System.Windows.Forms.GroupBox gbOptions;
		private System.Windows.Forms.GroupBox gbStoreForecast;
		private System.Windows.Forms.Label lblMerchandise;
		private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.Label lblDateRange;
		private System.Windows.Forms.Label lblAttribute;
        // Begin Track #4872 - JSmith - Global/User Attributes
        //private MIDRetail.Windows.Controls.MIDComboBoxEnh cboStoreGroups;
        private MIDAttributeComboBox cboStoreGroups;
        // End Track #4872
		private System.Windows.Forms.Label lblSet;
        private System.Windows.Forms.Button btnExcludeLowLevels;
		private System.Windows.Forms.CheckBox chkLowLevels;
        private System.Windows.Forms.CheckBox chkHighLevel;
		private System.Windows.Forms.RadioButton rdoMethod;
		private System.Windows.Forms.RadioButton rdoHierarchy;
		private System.Windows.Forms.RadioButton rdoNone;
		private System.Windows.Forms.Label lblLowLevelDefault;
		private System.Windows.Forms.Label lblLowLevelMerchandise;
		private System.Windows.Forms.CheckBox chkApplyMinMaxes;
		private System.Windows.Forms.MenuItem mnuExpandAll;
		private System.Windows.Forms.MenuItem mnuCollapseAll;
		private System.Windows.Forms.MenuItem mnuDeleteAll;
		private System.Windows.Forms.MenuItem mnuSeparator;
		private System.Windows.Forms.Label lblEqualizeWgtTY;
		private System.Windows.Forms.RadioButton rdoEqualizeYesTY;
		private System.Windows.Forms.RadioButton rdoEqualizeNoTY;
		private System.Windows.Forms.Label lblEqualizeWgtLY;
		private System.Windows.Forms.RadioButton rdoEqualizeYesLY;
		private System.Windows.Forms.RadioButton rdoEqualizeNoLY;
		private System.Windows.Forms.Label lblEqualizeWgtTrend;
		private System.Windows.Forms.RadioButton rdoEqualizeYesTrend;
		private System.Windows.Forms.RadioButton rdoEqualizeNoTrend;
        private UltraGrid ugWorkflows;
        private GroupBox gbxTrendOptions;
        private RadioButton radChainWOS;
        private RadioButton radChainPlan;
        private TextBox txtPlugChainWOS;
        private RadioButton radPlugChainWOS;
        private RadioButton rdoDefault;
        private MIDComboBoxEnh cboOverride;
        private MIDComboBoxEnh cboMerchandise;
        private MIDComboBoxEnh cbxStoreGroupLevel;
        private MIDComboBoxEnh cboFuncType;
        private MIDComboBoxEnh cboGLGroupBy;
        private MIDComboBoxEnh cboLowLevels;
        private MIDComboBoxEnh cboPlanVers;
        private MIDComboBoxEnh cboChainVers;
        private MIDComboBoxEnh cboModel;
        private System.Windows.Forms.CheckBox cbxProjCurrWkSales;
		private System.Windows.Forms.ToolTip toolTip1;
		#endregion

		#endregion
		
		#region Initialize & Dispose
		
		
		/// <summary>
		/// Create a new OTSPlanMethod
		/// </summary>
		/// <param name="SAB"></param>
		//		public void NewOTSPlanMethod(SessionAddressBlock SAB)//9-17]
		public frmOTSPlanMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB) : base (SAB, aEAB, eMIDTextCode.frm_OTSPlanMethod, eWorkflowMethodType.Method)
		{
			try
			{
				InitializeComponent();

				if (SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.CustomVariablesDefined)
				{
					cboModel.Visible = true;
					lblModel.Visible = true;
				}
				else
				{
					cboModel.Visible = false;
					lblModel.Visible = false;
				}
				UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserOTSPlan);
				GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalOTSPlan);

                // Begin TT#2647 - JSmith - Delays in OTS Method
                _nodesByRID = new Dictionary<int,HierarchyNodeProfile>();
                _nodesByID = new Dictionary<string, HierarchyNodeProfile>();
                // End TT#2647 - JSmith - Delays in OTS Method
				
			}
			catch(Exception ex)
			{
				HandleException(ex, "NewOTSPlanMethod");
				FormLoadError = true;
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )//9-17
		{
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                //remove handlers
                //Begin TT#7 - RBeck - Dynamic dropdowns
                this.cboGLGroupBy.SelectionChangeCommitted -= new System.EventHandler(this.cboGLGroupBy_SelectionChangeCommitted);
                this.cboChainVers.SelectionChangeCommitted -= new System.EventHandler(this.cboChainVers_SelectionChangeCommitted);
                this.cboModel.SelectionChangeCommitted -= new System.EventHandler(this.cboModel_SelectionChangeCommitted);
                //End   TT#7 - RBeck - Dynamic dropdowns
                this.cbxStoreGroupLevel.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreGroupLevel_SelectionChangeCommitted);
                this.cboFuncType.SelectedValueChanged -= new System.EventHandler(this.cboFuncType_SelectedValueChanged);
                this.cboFuncType.SelectionChangeCommitted -= new System.EventHandler(this.cboFuncType_SelectionChangeCommitted);
                this.grdPctContBasis.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdPctContBasis_InitializeRow);
                this.grdPctContBasis.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
                this.grdPctContBasis.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.grdPctContBasis_MouseUp);
                this.grdPctContBasis.AfterColRegionScroll -= new Infragistics.Win.UltraWinGrid.ColScrollRegionEventHandler(this.grdPctContBasis_AfterColRegionScroll);
                this.grdPctContBasis.AfterRowsDeleted -= new System.EventHandler(this.grdPctContBasis_AfterRowsDeleted);
                this.grdPctContBasis.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.GridMouseEnterElement);
                this.grdPctContBasis.DragDrop -= new System.Windows.Forms.DragEventHandler(this.grdPctContBasis_DragDrop);
                this.grdPctContBasis.AfterColPosChanged -= new Infragistics.Win.UltraWinGrid.AfterColPosChangedEventHandler(this.grdPctContBasis_AfterColPosChanged);
                this.grdPctContBasis.BeforeCellActivate -= new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.grdPctContBasis_BeforeCellActivate);
                this.grdPctContBasis.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.grdPctContBasis_BeforeRowsDeleted);
                this.grdPctContBasis.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.grdPctContBasis_BeforeRowInsert);
                this.grdPctContBasis.DragOver -= new System.Windows.Forms.DragEventHandler(this.grdPctContBasis_DragOver);
                this.grdPctContBasis.AfterExitEditMode -= new System.EventHandler(this.grdPctContBasis_AfterExitEditMode);
                this.grdPctContBasis.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdPctContBasis_ClickCellButton);
                this.grdPctContBasis.AfterRowExpanded -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdPctContBasis_AfterRowExpanded);
                this.grdPctContBasis.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdPctContBasis_AfterCellUpdate);
                this.grdPctContBasis.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdPctContBasis_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(grdPctContBasis);
                //End TT#169
                this.grdPctContBasis.AfterRowRegionScroll -= new Infragistics.Win.UltraWinGrid.RowScrollRegionEventHandler(this.grdPctContBasis_AfterRowRegionScroll);
                this.grdPctContBasis.BeforeCellDeactivate -= new System.ComponentModel.CancelEventHandler(this.grdPctContBasis_BeforeCellDeactivate);
                this.grdPctContBasis.AfterRowCollapsed -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdPctContBasis_AfterRowCollapsed);
                this.GridContextMenu.Popup -= new System.EventHandler(this.GridContextMenu_Popup);
                this.mnuDelete.Click -= new System.EventHandler(this.mnuDelete_Click);
                this.mnuInsert.Click -= new System.EventHandler(this.mnuInsert_Click);
                this.mnuExpandAll.Click -= new System.EventHandler(this.mnuExpandAll_Click);
                this.mnuCollapseAll.Click -= new System.EventHandler(this.mnuCollapseAll_Click);
                this.mnuDeleteAll.Click += new System.EventHandler(this.mnuDeleteAll_Click);
                this.btnIncExc.Click -= new System.EventHandler(this.btnIncExc_Click);
                this.gridStockMinMax.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.gridStockMinMax_InitializeRow);
                this.gridStockMinMax.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
                this.gridStockMinMax.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridStockMinMax_CellChange);
                this.gridStockMinMax.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.gridStockMinMax_BeforeRowInsert);
                this.gridStockMinMax.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridStockMinMax_ClickCellButton);
                this.gridStockMinMax.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridStockMinMax_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(gridStockMinMax);
                //End TT#169
                this.chkUseDefault.Click -= new System.EventHandler(this.chkUseDefault_Click);
                this.chkUseDefault.CheckedChanged -= new System.EventHandler(this.chkUseDefault_CheckedChanged);
                this.chkUseDefault.EnabledChanged -= new EventHandler(chkUseDefault_EnabledChanged); // TT#375-MD - JSmith - stock minimum value went to the use default set and it was not there.  Expectation is the use default set would have the stock minimum value
                this.chkClear.CheckedChanged -= new System.EventHandler(this.chkClear_CheckedChanged);
                this.chkDefault.Click -= new System.EventHandler(this.chkDefault_Click);
                this.chkDefault.CheckedChanged -= new System.EventHandler(this.chkDefault_CheckedChanged);
                this.cboStoreGroups.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreGroups_SelectionChangeCommitted);
                this.cboStoreGroups.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboStoreGroups_DragDrop);
                this.cboStoreGroups.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboStoreGroups_DragEnter);
                this.midDateRangeSelectorOTSPlan.Load -= new System.EventHandler(this.midDateRangeSelectorOTSPlan_Load);
                this.midDateRangeSelectorOTSPlan.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.midDateRangeSelectorOTSPlan_ClickCellButton);
                this.midDateRangeSelectorOTSPlan.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.midDateRangeSelectorOTSPlan_OnSelection);
                this.txtOTSHNDesc.Validating -= new System.ComponentModel.CancelEventHandler(this.txtOTSHNDesc_Validating);
                this.txtOTSHNDesc.Validated -= new System.EventHandler(this.txtOTSHNDesc_Validated);
                this.txtOTSHNDesc.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtOTSHNDesc_DragDrop);
                this.txtOTSHNDesc.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtOTSHNDesc_DragEnter);
                this.txtOTSHNDesc.DragOver -= new System.Windows.Forms.DragEventHandler(this.txtOTSHNDesc_DragOver);
                this.cboPlanVers.SelectionChangeCommitted -= new System.EventHandler(this.cboPlanVers_SelectionChangeCommitted);
                // Begin MID Track 4858 - JSmith - Security changes
                //				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
                //				this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
                //				this.btnProcess.Click -= new System.EventHandler(this.btnProcess_Click);
                // End MID Track 4858
                this.tabTYLYTrend.SelectedIndexChanged -= new System.EventHandler(this.tabTYLYTrend_SelectedIndexChanged);

                // Begin TT#802 - JSmith - Hierarchy node image does not drag over trend tab
                this.tabTYLYTrend.DragOver -= new System.Windows.Forms.DragEventHandler(this.control_DragOver);
                this.tabTYLYTrend.DragEnter -= new System.Windows.Forms.DragEventHandler(this.control_DragEnter);
                this.gridTYNodeVersion.DragEnter -= new System.Windows.Forms.DragEventHandler(this.control_DragEnter);
                this.gridLYNodeVersion.DragEnter -= new System.Windows.Forms.DragEventHandler(this.control_DragEnter);
                this.gridTrendNodeVersion.DragEnter -= new System.Windows.Forms.DragEventHandler(this.control_DragEnter);
                this.gridStockMinMax.DragOver -= new System.Windows.Forms.DragEventHandler(this.control_DragOver);
                this.gridStockMinMax.DragEnter -= new System.Windows.Forms.DragEventHandler(this.control_DragEnter);
                // End TT#802

                this.btnTYIncExc.Click -= new System.EventHandler(this.btnTYIncExc_Click);
                this.gridTYNodeVersion.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.gridTYNodeVersion_InitializeRow);
                this.gridTYNodeVersion.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
                this.gridTYNodeVersion.AfterColRegionScroll -= new Infragistics.Win.UltraWinGrid.ColScrollRegionEventHandler(this.gridTYNodeVersion_AfterColRegionScroll);
                this.gridTYNodeVersion.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.GridMouseEnterElement);
                this.gridTYNodeVersion.DragDrop -= new System.Windows.Forms.DragEventHandler(this.gridTYNodeVersion_DragDrop);
                this.gridTYNodeVersion.AfterColPosChanged -= new Infragistics.Win.UltraWinGrid.AfterColPosChangedEventHandler(this.gridTYNodeVersion_AfterColPosChanged);
                this.gridTYNodeVersion.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.gridTYNodeVersion_BeforeRowInsert);
                this.gridTYNodeVersion.DragOver -= new System.Windows.Forms.DragEventHandler(this.gridTYNodeVersion_DragOver);
                this.gridTYNodeVersion.AfterExitEditMode -= new System.EventHandler(this.gridTYNodeVersion_AfterExitEditMode);
                this.gridTYNodeVersion.AfterRowExpanded -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.gridTYNodeVersion_AfterRowExpanded);
                this.gridTYNodeVersion.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridTYNodeVersion_AfterCellUpdate);
                this.gridTYNodeVersion.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridTYNodeVersion_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(gridTYNodeVersion);
                //End TT#169
                this.gridTYNodeVersion.AfterRowRegionScroll -= new Infragistics.Win.UltraWinGrid.RowScrollRegionEventHandler(this.gridTYNodeVersion_AfterRowRegionScroll);
                this.gridTYNodeVersion.BeforeCellDeactivate -= new System.ComponentModel.CancelEventHandler(this.gridTYNodeVersion_BeforeCellDeactivate);
                this.gridTYNodeVersion.AfterRowCollapsed -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.gridTYNodeVersion_AfterRowCollapsed);
                this.cbxAltLY.CheckedChanged -= new System.EventHandler(this.cbxAltLY_CheckedChanged);
                this.btnLYIncExc.Click -= new System.EventHandler(this.btnLYIncExc_Click);
                this.gridLYNodeVersion.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.gridLYNodeVersion_InitializeRow);
                this.gridLYNodeVersion.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
                this.gridLYNodeVersion.AfterColRegionScroll -= new Infragistics.Win.UltraWinGrid.ColScrollRegionEventHandler(this.gridLYNodeVersion_AfterColRegionScroll);
                this.gridLYNodeVersion.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.GridMouseEnterElement);
                this.gridLYNodeVersion.DragDrop -= new System.Windows.Forms.DragEventHandler(this.gridLYNodeVersion_DragDrop);
                this.gridLYNodeVersion.AfterColPosChanged -= new Infragistics.Win.UltraWinGrid.AfterColPosChangedEventHandler(this.gridLYNodeVersion_AfterColPosChanged);
                this.gridLYNodeVersion.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.gridLYNodeVersion_BeforeRowInsert);
                this.gridLYNodeVersion.DragOver -= new System.Windows.Forms.DragEventHandler(this.gridLYNodeVersion_DragOver);
                this.gridLYNodeVersion.AfterExitEditMode -= new System.EventHandler(this.gridLYNodeVersion_AfterExitEditMode);
                this.gridLYNodeVersion.AfterRowExpanded -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.gridLYNodeVersion_AfterRowExpanded);
                this.gridLYNodeVersion.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridLYNodeVersion_AfterCellUpdate);
                this.gridLYNodeVersion.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridLYNodeVersion_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(gridLYNodeVersion);
                //End TT#169
                this.gridLYNodeVersion.AfterRowRegionScroll -= new Infragistics.Win.UltraWinGrid.RowScrollRegionEventHandler(this.gridLYNodeVersion_AfterRowRegionScroll);
                this.gridLYNodeVersion.BeforeCellDeactivate -= new System.ComponentModel.CancelEventHandler(this.gridLYNodeVersion_BeforeCellDeactivate);
                this.gridLYNodeVersion.AfterRowCollapsed -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.gridLYNodeVersion_AfterRowCollapsed);
                this.cbxAltTrend.CheckedChanged -= new System.EventHandler(this.cbxAltTrend_CheckedChanged);
                this.cbxProjCurrWkSales.CheckedChanged -= new System.EventHandler(this.cbxProjCurrWkSales_CheckedChanged);
                this.btnTrendIncExc.Click -= new System.EventHandler(this.btnTrendIncExc_Click);
                this.gridTrendNodeVersion.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.gridTrendNodeVersion_InitializeRow);
                this.gridTrendNodeVersion.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
                this.gridTrendNodeVersion.AfterColRegionScroll -= new Infragistics.Win.UltraWinGrid.ColScrollRegionEventHandler(this.gridTrendNodeVersion_AfterColRegionScroll);
                this.gridTrendNodeVersion.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.GridMouseEnterElement);
                this.gridTrendNodeVersion.DragDrop -= new System.Windows.Forms.DragEventHandler(this.gridTrendNodeVersion_DragDrop);
                this.gridTrendNodeVersion.AfterColPosChanged -= new Infragistics.Win.UltraWinGrid.AfterColPosChangedEventHandler(this.gridTrendNodeVersion_AfterColPosChanged);
                this.gridTrendNodeVersion.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.gridTrendNodeVersion_BeforeRowInsert);
                this.gridTrendNodeVersion.DragOver -= new System.Windows.Forms.DragEventHandler(this.gridTrendNodeVersion_DragOver);
                this.gridTrendNodeVersion.AfterExitEditMode -= new System.EventHandler(this.gridTrendNodeVersion_AfterExitEditMode);
                this.gridTrendNodeVersion.AfterRowExpanded -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.gridTrendNodeVersion_AfterRowExpanded);
                this.gridTrendNodeVersion.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridTrendNodeVersion_AfterCellUpdate);
                this.gridTrendNodeVersion.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridTrendNodeVersion_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(gridTrendNodeVersion);
                //End TT#169
                this.gridTrendNodeVersion.AfterRowRegionScroll -= new Infragistics.Win.UltraWinGrid.RowScrollRegionEventHandler(this.gridTrendNodeVersion_AfterRowRegionScroll);
                this.gridTrendNodeVersion.BeforeCellDeactivate -= new System.ComponentModel.CancelEventHandler(this.gridTrendNodeVersion_BeforeCellDeactivate);
                this.gridTrendNodeVersion.AfterRowCollapsed -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.gridTrendNodeVersion_AfterRowCollapsed);
                this.radNone.CheckedChanged -= new System.EventHandler(this.radNone_CheckedChanged);
                this.radTolerance.CheckedChanged -= new System.EventHandler(this.radTolerance_CheckedChanged);
                this.radLimits.CheckedChanged -= new System.EventHandler(this.radLimits_CheckedChanged);
                this.Load -= new System.EventHandler(this.frmOTSPlanMethod_Load);
                //Begin Track #4371 - JSmith - Multi-level forecasting.
                this.btnExcludeLowLevels.Click -= new System.EventHandler(this.btnExcludeLowLevels_Click);
                this.cboLowLevels.SelectionChangeCommitted -= new System.EventHandler(this.cboLowLevels_SelectionChangeCommitted);
                this.chkHighLevel.CheckedChanged -= new System.EventHandler(this.chkHighLevel_CheckedChanged);
                this.chkHighLevel.MouseHover -= new System.EventHandler(this.chkHighLevel_MouseHover);
                this.chkLowLevels.MouseHover -= new System.EventHandler(this.chkLowLevels_MouseHover);
                //End Track #4371
                // BEGIN TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 
                this.radChainPlan.CheckedChanged -= new System.EventHandler(this.radChainPlan_CheckedChanged);
                this.radChainWOS.CheckedChanged -= new System.EventHandler(this.radChainWOS_CheckedChanged);
                this.radPlugChainWOS.CheckedChanged -= new System.EventHandler(this.radPlugChainWOS_CheckedChanged);
                this.txtPlugChainWOS.TextChanged -= new System.EventHandler(this.txtPlugChainWOS_TextChanged);
                // END TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 


                this.cboPlanVers.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboPlanVers_MIDComboBoxPropertiesChangedEvent);
                this.cboLowLevels.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboLowLevels_MIDComboBoxPropertiesChangedEvent);
                this.cboOverride.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboOverride_MIDComboBoxPropertiesChangedEvent);
                this.cboChainVers.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboChainVers_MIDComboBoxPropertiesChangedEvent);
                this.cboModel.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboModel_MIDComboBoxPropertiesChangedEvent);
                this.cbxStoreGroupLevel.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cbxStoreGroupLevel_MIDComboBoxPropertiesChangedEvent);
                this.cboStoreGroups.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboStoreGroups_MIDComboBoxPropertiesChangedEvent);
                this.cboGLGroupBy.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboGLGroupBy_MIDComboBoxPropertiesChangedEvent);
                this.cboFuncType.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboFuncType_MIDComboBoxPropertiesChangedEvent);
                this.cboMerchandise.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboMerchandise_MIDComboBoxPropertiesChangedEvent);


                if (ApplicationTransaction != null)
                {
                    ApplicationTransaction.Dispose();
                }
            }
			base.Dispose( disposing );
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
            this.tabOTSMethod = new System.Windows.Forms.TabControl();
            this.tabMethod = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbxTrendOptions = new System.Windows.Forms.GroupBox();
            this.txtPlugChainWOS = new System.Windows.Forms.TextBox();
            this.radPlugChainWOS = new System.Windows.Forms.RadioButton();
            this.radChainWOS = new System.Windows.Forms.RadioButton();
            this.radChainPlan = new System.Windows.Forms.RadioButton();
            this.gbStoreForecast = new System.Windows.Forms.GroupBox();
            this.cboPlanVers = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboLowLevels = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboOverride = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.chkHighLevel = new System.Windows.Forms.CheckBox();
            this.btnExcludeLowLevels = new System.Windows.Forms.Button();
            this.chkLowLevels = new System.Windows.Forms.CheckBox();
            this.lblDateRange = new System.Windows.Forms.Label();
            this.lblMerchandise = new System.Windows.Forms.Label();
            this.txtOTSHNDesc = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.midDateRangeSelectorOTSPlan = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.gbChainForecast = new System.Windows.Forms.GroupBox();
            this.cboChainVers = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.label10 = new System.Windows.Forms.Label();
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.cboModel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblModel = new System.Windows.Forms.Label();
            this.chkOTSStock = new System.Windows.Forms.CheckBox();
            this.chkOTSSales = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.pnlTYLYTrend = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabTYLYTrend = new System.Windows.Forms.TabControl();
            this.tabPageTY = new System.Windows.Forms.TabPage();
            this.rdoEqualizeNoTY = new System.Windows.Forms.RadioButton();
            this.rdoEqualizeYesTY = new System.Windows.Forms.RadioButton();
            this.lblEqualizeWgtTY = new System.Windows.Forms.Label();
            this.btnTYIncExc = new System.Windows.Forms.Button();
            this.gridTYNodeVersion = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.GridContextMenu = new System.Windows.Forms.ContextMenu();
            this.mnuExpandAll = new System.Windows.Forms.MenuItem();
            this.mnuCollapseAll = new System.Windows.Forms.MenuItem();
            this.mnuSeparator = new System.Windows.Forms.MenuItem();
            this.mnuDelete = new System.Windows.Forms.MenuItem();
            this.mnuDeleteAll = new System.Windows.Forms.MenuItem();
            this.mnuInsert = new System.Windows.Forms.MenuItem();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tabPageLY = new System.Windows.Forms.TabPage();
            this.rdoEqualizeNoLY = new System.Windows.Forms.RadioButton();
            this.rdoEqualizeYesLY = new System.Windows.Forms.RadioButton();
            this.lblEqualizeWgtLY = new System.Windows.Forms.Label();
            this.cbxAltLY = new System.Windows.Forms.CheckBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.btnLYIncExc = new System.Windows.Forms.Button();
            this.gridLYNodeVersion = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tabPageApplyTrend = new System.Windows.Forms.TabPage();
            this.cbxProjCurrWkSales = new System.Windows.Forms.CheckBox();
            this.rdoEqualizeNoTrend = new System.Windows.Forms.RadioButton();
            this.rdoEqualizeYesTrend = new System.Windows.Forms.RadioButton();
            this.lblEqualizeWgtTrend = new System.Windows.Forms.Label();
            this.cbxAltTrend = new System.Windows.Forms.CheckBox();
            this.btnTrendIncExc = new System.Windows.Forms.Button();
            this.gridTrendNodeVersion = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tabPageCaps = new System.Windows.Forms.TabPage();
            this.grpTrendCaps = new System.Windows.Forms.GroupBox();
            this.radNone = new System.Windows.Forms.RadioButton();
            this.radTolerance = new System.Windows.Forms.RadioButton();
            this.txtTolerance = new System.Windows.Forms.TextBox();
            this.radLimits = new System.Windows.Forms.RadioButton();
            this.lblHigh = new System.Windows.Forms.Label();
            this.txtHigh = new System.Windows.Forms.TextBox();
            this.lblLow = new System.Windows.Forms.Label();
            this.txtLow = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tabSetMethods = new System.Windows.Forms.TabPage();
            this.grpGroupLevelMethod = new System.Windows.Forms.GroupBox();
            this.cbxStoreGroupLevel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblSet = new System.Windows.Forms.Label();
            this.lblAttribute = new System.Windows.Forms.Label();
            this.cboStoreGroups = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.btnCopy = new System.Windows.Forms.Button();
            this.tabSetMethod = new System.Windows.Forms.TabControl();
            this.tabCriteria = new System.Windows.Forms.TabPage();
            this.lblWidth30 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cboGLGroupBy = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboFuncType = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblGLGroupBy = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlPercentContribution = new System.Windows.Forms.Panel();
            this.grpContributionBasis = new System.Windows.Forms.GroupBox();
            this.grdPctContBasis = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnIncExc = new System.Windows.Forms.Button();
            this.tabStockMinMax = new System.Windows.Forms.TabPage();
            this.cboMerchandise = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.rdoDefault = new System.Windows.Forms.RadioButton();
            this.chkApplyMinMaxes = new System.Windows.Forms.CheckBox();
            this.rdoNone = new System.Windows.Forms.RadioButton();
            this.rdoHierarchy = new System.Windows.Forms.RadioButton();
            this.rdoMethod = new System.Windows.Forms.RadioButton();
            this.lblLowLevelDefault = new System.Windows.Forms.Label();
            this.lblLowLevelMerchandise = new System.Windows.Forms.Label();
            this.gridStockMinMax = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.chkUseDefault = new System.Windows.Forms.CheckBox();
            this.chkClear = new System.Windows.Forms.CheckBox();
            this.chkPlan = new System.Windows.Forms.CheckBox();
            this.chkDefault = new System.Windows.Forms.CheckBox();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Icons = new System.Windows.Forms.ImageList(this.components);
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.tabOTSMethod.SuspendLayout();
            this.tabMethod.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbxTrendOptions.SuspendLayout();
            this.gbStoreForecast.SuspendLayout();
            this.gbChainForecast.SuspendLayout();
            this.gbOptions.SuspendLayout();
            this.pnlTYLYTrend.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabTYLYTrend.SuspendLayout();
            this.tabPageTY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTYNodeVersion)).BeginInit();
            this.tabPageLY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLYNodeVersion)).BeginInit();
            this.tabPageApplyTrend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTrendNodeVersion)).BeginInit();
            this.tabPageCaps.SuspendLayout();
            this.grpTrendCaps.SuspendLayout();
            this.tabSetMethods.SuspendLayout();
            this.grpGroupLevelMethod.SuspendLayout();
            this.tabSetMethod.SuspendLayout();
            this.tabCriteria.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlPercentContribution.SuspendLayout();
            this.grpContributionBasis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPctContBasis)).BeginInit();
            this.tabStockMinMax.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridStockMinMax)).BeginInit();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(635, 544);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(547, 544);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(16, 544);
            // 
            // pnlGlobalUser
            // 
            this.pnlGlobalUser.Location = new System.Drawing.Point(600, 8);
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(307, 8);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(109, 8);
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(8, 10);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // tabOTSMethod
            // 
            this.tabOTSMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabOTSMethod.Controls.Add(this.tabMethod);
            this.tabOTSMethod.Controls.Add(this.tabSetMethods);
            this.tabOTSMethod.Controls.Add(this.tabProperties);
            this.tabOTSMethod.Location = new System.Drawing.Point(16, 40);
            this.tabOTSMethod.Name = "tabOTSMethod";
            this.tabOTSMethod.SelectedIndex = 0;
            this.tabOTSMethod.Size = new System.Drawing.Size(685, 496);
            this.tabOTSMethod.TabIndex = 3;
            this.tabOTSMethod.DragEnter += new System.Windows.Forms.DragEventHandler(this.control_DragEnter);
            this.tabOTSMethod.DragOver += new System.Windows.Forms.DragEventHandler(this.control_DragOver);
            // 
            // tabMethod
            // 
            this.tabMethod.Controls.Add(this.panel1);
            this.tabMethod.Controls.Add(this.pnlTYLYTrend);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(677, 470);
            this.tabMethod.TabIndex = 1;
            this.tabMethod.Text = "Method";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.gbxTrendOptions);
            this.panel1.Controls.Add(this.gbStoreForecast);
            this.panel1.Controls.Add(this.gbChainForecast);
            this.panel1.Controls.Add(this.gbOptions);
            this.panel1.Location = new System.Drawing.Point(8, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(666, 460);
            this.panel1.TabIndex = 0;
            // 
            // gbxTrendOptions
            // 
            this.gbxTrendOptions.Controls.Add(this.txtPlugChainWOS);
            this.gbxTrendOptions.Controls.Add(this.radPlugChainWOS);
            this.gbxTrendOptions.Controls.Add(this.radChainWOS);
            this.gbxTrendOptions.Controls.Add(this.radChainPlan);
            this.gbxTrendOptions.Location = new System.Drawing.Point(7, 338);
            this.gbxTrendOptions.Name = "gbxTrendOptions";
            this.gbxTrendOptions.Size = new System.Drawing.Size(646, 81);
            this.gbxTrendOptions.TabIndex = 4;
            this.gbxTrendOptions.TabStop = false;
            this.gbxTrendOptions.Text = "Trend Options:";
            // 
            // txtPlugChainWOS
            // 
            this.txtPlugChainWOS.Location = new System.Drawing.Point(419, 36);
            this.txtPlugChainWOS.Name = "txtPlugChainWOS";
            this.txtPlugChainWOS.Size = new System.Drawing.Size(100, 20);
            this.txtPlugChainWOS.TabIndex = 3;
            this.txtPlugChainWOS.TextChanged += new System.EventHandler(this.txtPlugChainWOS_TextChanged);
            // 
            // radPlugChainWOS
            // 
            this.radPlugChainWOS.AutoSize = true;
            this.radPlugChainWOS.Location = new System.Drawing.Point(296, 36);
            this.radPlugChainWOS.Name = "radPlugChainWOS";
            this.radPlugChainWOS.Size = new System.Drawing.Size(108, 17);
            this.radPlugChainWOS.TabIndex = 2;
            this.radPlugChainWOS.TabStop = true;
            this.radPlugChainWOS.Text = "Plug Chain WOS:";
            this.radPlugChainWOS.UseVisualStyleBackColor = true;
            this.radPlugChainWOS.CheckedChanged += new System.EventHandler(this.radPlugChainWOS_CheckedChanged);
            // 
            // radChainWOS
            // 
            this.radChainWOS.AutoSize = true;
            this.radChainWOS.Location = new System.Drawing.Point(160, 36);
            this.radChainWOS.Name = "radChainWOS";
            this.radChainWOS.Size = new System.Drawing.Size(84, 17);
            this.radChainWOS.TabIndex = 1;
            this.radChainWOS.TabStop = true;
            this.radChainWOS.Text = "Chain WOS:";
            this.radChainWOS.UseVisualStyleBackColor = true;
            this.radChainWOS.CheckedChanged += new System.EventHandler(this.radChainWOS_CheckedChanged);
            // 
            // radChainPlan
            // 
            this.radChainPlan.AutoSize = true;
            this.radChainPlan.Location = new System.Drawing.Point(25, 35);
            this.radChainPlan.Name = "radChainPlan";
            this.radChainPlan.Size = new System.Drawing.Size(84, 17);
            this.radChainPlan.TabIndex = 0;
            this.radChainPlan.TabStop = true;
            this.radChainPlan.Text = "Chain Plans:";
            this.radChainPlan.UseVisualStyleBackColor = true;
            this.radChainPlan.CheckedChanged += new System.EventHandler(this.radChainPlan_CheckedChanged);
            // 
            // gbStoreForecast
            // 
            this.gbStoreForecast.Controls.Add(this.cboPlanVers);
            this.gbStoreForecast.Controls.Add(this.cboLowLevels);
            this.gbStoreForecast.Controls.Add(this.cboOverride);
            this.gbStoreForecast.Controls.Add(this.chkHighLevel);
            this.gbStoreForecast.Controls.Add(this.btnExcludeLowLevels);
            this.gbStoreForecast.Controls.Add(this.chkLowLevels);
            this.gbStoreForecast.Controls.Add(this.lblDateRange);
            this.gbStoreForecast.Controls.Add(this.lblMerchandise);
            this.gbStoreForecast.Controls.Add(this.txtOTSHNDesc);
            this.gbStoreForecast.Controls.Add(this.label8);
            this.gbStoreForecast.Controls.Add(this.midDateRangeSelectorOTSPlan);
            this.gbStoreForecast.Location = new System.Drawing.Point(7, 6);
            this.gbStoreForecast.Name = "gbStoreForecast";
            this.gbStoreForecast.Size = new System.Drawing.Size(646, 119);
            this.gbStoreForecast.TabIndex = 3;
            this.gbStoreForecast.TabStop = false;
            this.gbStoreForecast.Text = "Store Forecast";
            // 
            // cboPlanVers
            // 
            this.cboPlanVers.AutoAdjust = true;
            this.cboPlanVers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboPlanVers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPlanVers.AutoScroll = true;
            this.cboPlanVers.DataSource = null;
            this.cboPlanVers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPlanVers.DropDownWidth = 225;
            this.cboPlanVers.FormattingEnabled = false;
            this.cboPlanVers.Location = new System.Drawing.Point(97, 51);
            this.cboPlanVers.Margin = new System.Windows.Forms.Padding(0);
            this.cboPlanVers.Name = "cboPlanVers";
            this.cboPlanVers.Size = new System.Drawing.Size(225, 21);
            this.cboPlanVers.TabIndex = 17;
            this.cboPlanVers.Tag = null;
            this.cboPlanVers.SelectionChangeCommitted += new System.EventHandler(this.cboPlanVers_SelectionChangeCommitted);
            this.cboPlanVers.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboPlanVers_MIDComboBoxPropertiesChangedEvent);
            // 
            // cboLowLevels
            // 
            this.cboLowLevels.AutoAdjust = true;
            this.cboLowLevels.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboLowLevels.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboLowLevels.AutoScroll = true;
            this.cboLowLevels.DataSource = null;
            this.cboLowLevels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLowLevels.DropDownWidth = 206;
            this.cboLowLevels.FormattingEnabled = false;
            this.cboLowLevels.Location = new System.Drawing.Point(421, 48);
            this.cboLowLevels.Margin = new System.Windows.Forms.Padding(0);
            this.cboLowLevels.Name = "cboLowLevels";
            this.cboLowLevels.Size = new System.Drawing.Size(206, 21);
            this.cboLowLevels.TabIndex = 24;
            this.cboLowLevels.Tag = null;
            this.cboLowLevels.SelectionChangeCommitted += new System.EventHandler(this.cboLowLevels_SelectionChangeCommitted);
            this.cboLowLevels.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboLowLevels_MIDComboBoxPropertiesChangedEvent);
            // 
            // cboOverride
            // 
            this.cboOverride.AutoAdjust = true;
            this.cboOverride.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboOverride.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboOverride.AutoScroll = true;
            this.cboOverride.DataSource = null;
            this.cboOverride.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOverride.DropDownWidth = 136;
            this.cboOverride.FormattingEnabled = false;
            this.cboOverride.Location = new System.Drawing.Point(492, 79);
            this.cboOverride.Margin = new System.Windows.Forms.Padding(0);
            this.cboOverride.Name = "cboOverride";
            this.cboOverride.Size = new System.Drawing.Size(136, 21);
            this.cboOverride.TabIndex = 27;
            this.cboOverride.Tag = null;
            this.cboOverride.SelectionChangeCommitted += new System.EventHandler(this.cboOverride_SelectionChangeCommitted);
            this.cboOverride.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboOverride_MIDComboBoxPropertiesChangedEvent);
            // 
            // chkHighLevel
            // 
            this.chkHighLevel.Location = new System.Drawing.Point(421, 17);
            this.chkHighLevel.Name = "chkHighLevel";
            this.chkHighLevel.Size = new System.Drawing.Size(83, 24);
            this.chkHighLevel.TabIndex = 26;
            this.chkHighLevel.Text = "High Level";
            this.chkHighLevel.CheckedChanged += new System.EventHandler(this.chkHighLevel_CheckedChanged);
            this.chkHighLevel.MouseHover += new System.EventHandler(this.chkHighLevel_MouseHover);
            // 
            // btnExcludeLowLevels
            // 
            this.btnExcludeLowLevels.Location = new System.Drawing.Point(365, 79);
            this.btnExcludeLowLevels.Name = "btnExcludeLowLevels";
            this.btnExcludeLowLevels.Size = new System.Drawing.Size(110, 23);
            this.btnExcludeLowLevels.TabIndex = 25;
            this.btnExcludeLowLevels.Text = "Exclude Low Levels";
            this.btnExcludeLowLevels.Click += new System.EventHandler(this.btnExcludeLowLevels_Click);
            // 
            // chkLowLevels
            // 
            this.chkLowLevels.Location = new System.Drawing.Point(545, 18);
            this.chkLowLevels.Name = "chkLowLevels";
            this.chkLowLevels.Size = new System.Drawing.Size(83, 24);
            this.chkLowLevels.TabIndex = 23;
            this.chkLowLevels.Text = "Low Levels";
            this.chkLowLevels.CheckedChanged += new System.EventHandler(this.chkLowLevels_CheckedChanged);
            this.chkLowLevels.MouseHover += new System.EventHandler(this.chkLowLevels_MouseHover);
            // 
            // lblDateRange
            // 
            this.lblDateRange.Location = new System.Drawing.Point(14, 88);
            this.lblDateRange.Name = "lblDateRange";
            this.lblDateRange.Size = new System.Drawing.Size(72, 16);
            this.lblDateRange.TabIndex = 19;
            this.lblDateRange.Text = "Date Range";
            // 
            // lblMerchandise
            // 
            this.lblMerchandise.Location = new System.Drawing.Point(14, 24);
            this.lblMerchandise.Name = "lblMerchandise";
            this.lblMerchandise.Size = new System.Drawing.Size(72, 16);
            this.lblMerchandise.TabIndex = 18;
            this.lblMerchandise.Text = "Merchandise:";
            // 
            // txtOTSHNDesc
            // 
            this.txtOTSHNDesc.AllowDrop = true;
            this.txtOTSHNDesc.Location = new System.Drawing.Point(97, 16);
            this.txtOTSHNDesc.Name = "txtOTSHNDesc";
            this.txtOTSHNDesc.Size = new System.Drawing.Size(224, 20);
            this.txtOTSHNDesc.TabIndex = 0;
            this.txtOTSHNDesc.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtOTSHNDesc_DragDrop);
            this.txtOTSHNDesc.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtOTSHNDesc_DragEnter);
            this.txtOTSHNDesc.DragOver += new System.Windows.Forms.DragEventHandler(this.txtOTSHNDesc_DragOver);
            this.txtOTSHNDesc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtOTSHNDesc_KeyPress);
            this.txtOTSHNDesc.Validating += new System.ComponentModel.CancelEventHandler(this.txtOTSHNDesc_Validating);
            this.txtOTSHNDesc.Validated += new System.EventHandler(this.txtOTSHNDesc_Validated);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(14, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 16);
            this.label8.TabIndex = 6;
            this.label8.Text = "Version:";
            // 
            // midDateRangeSelectorOTSPlan
            // 
            this.midDateRangeSelectorOTSPlan.DateRangeForm = null;
            this.midDateRangeSelectorOTSPlan.DateRangeRID = 0;
            this.midDateRangeSelectorOTSPlan.Enabled = false;
            this.midDateRangeSelectorOTSPlan.Location = new System.Drawing.Point(97, 87);
            this.midDateRangeSelectorOTSPlan.Name = "midDateRangeSelectorOTSPlan";
            this.midDateRangeSelectorOTSPlan.Size = new System.Drawing.Size(224, 24);
            this.midDateRangeSelectorOTSPlan.TabIndex = 12;
            this.midDateRangeSelectorOTSPlan.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.midDateRangeSelectorOTSPlan_OnSelection);
            this.midDateRangeSelectorOTSPlan.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.midDateRangeSelectorOTSPlan_ClickCellButton);
            this.midDateRangeSelectorOTSPlan.Load += new System.EventHandler(this.midDateRangeSelectorOTSPlan_Load);
            // 
            // gbChainForecast
            // 
            this.gbChainForecast.Controls.Add(this.cboChainVers);
            this.gbChainForecast.Controls.Add(this.label10);
            this.gbChainForecast.Location = new System.Drawing.Point(7, 135);
            this.gbChainForecast.Name = "gbChainForecast";
            this.gbChainForecast.Size = new System.Drawing.Size(646, 96);
            this.gbChainForecast.TabIndex = 1;
            this.gbChainForecast.TabStop = false;
            this.gbChainForecast.Text = "Chain Forecast";
            // 
            // cboChainVers
            // 
            this.cboChainVers.AutoAdjust = true;
            this.cboChainVers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboChainVers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboChainVers.AutoScroll = true;
            this.cboChainVers.DataSource = null;
            this.cboChainVers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboChainVers.DropDownWidth = 224;
            this.cboChainVers.FormattingEnabled = false;
            this.cboChainVers.Location = new System.Drawing.Point(97, 24);
            this.cboChainVers.Margin = new System.Windows.Forms.Padding(0);
            this.cboChainVers.Name = "cboChainVers";
            this.cboChainVers.Size = new System.Drawing.Size(224, 21);
            this.cboChainVers.TabIndex = 18;
            this.cboChainVers.Tag = null;
            this.cboChainVers.SelectionChangeCommitted += new System.EventHandler(this.cboChainVers_SelectionChangeCommitted);
            this.cboChainVers.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboChainVers_MIDComboBoxPropertiesChangedEvent);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(14, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(78, 16);
            this.label10.TabIndex = 10;
            this.label10.Text = "Chain Version:";
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.cboModel);
            this.gbOptions.Controls.Add(this.lblModel);
            this.gbOptions.Controls.Add(this.chkOTSStock);
            this.gbOptions.Controls.Add(this.chkOTSSales);
            this.gbOptions.Controls.Add(this.label9);
            this.gbOptions.Location = new System.Drawing.Point(7, 241);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Size = new System.Drawing.Size(646, 88);
            this.gbOptions.TabIndex = 2;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Options";
            // 
            // cboModel
            // 
            this.cboModel.AutoAdjust = true;
            this.cboModel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboModel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboModel.AutoScroll = true;
            this.cboModel.DataSource = null;
            this.cboModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModel.DropDownWidth = 206;
            this.cboModel.FormattingEnabled = false;
            this.cboModel.Location = new System.Drawing.Point(97, 24);
            this.cboModel.Margin = new System.Windows.Forms.Padding(0);
            this.cboModel.Name = "cboModel";
            this.cboModel.Size = new System.Drawing.Size(206, 21);
            this.cboModel.TabIndex = 10;
            this.cboModel.Tag = null;
            this.cboModel.SelectionChangeCommitted += new System.EventHandler(this.cboModel_SelectionChangeCommitted);
            this.cboModel.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboModel_MIDComboBoxPropertiesChangedEvent);
            // 
            // lblModel
            // 
            this.lblModel.Location = new System.Drawing.Point(14, 28);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(78, 16);
            this.lblModel.TabIndex = 9;
            this.lblModel.Text = "Model:";
            // 
            // chkOTSStock
            // 
            this.chkOTSStock.Location = new System.Drawing.Point(156, 59);
            this.chkOTSStock.Name = "chkOTSStock";
            this.chkOTSStock.Size = new System.Drawing.Size(56, 16);
            this.chkOTSStock.TabIndex = 5;
            this.chkOTSStock.Text = "Stock";
            // 
            // chkOTSSales
            // 
            this.chkOTSSales.Location = new System.Drawing.Point(97, 59);
            this.chkOTSSales.Name = "chkOTSSales";
            this.chkOTSSales.Size = new System.Drawing.Size(48, 16);
            this.chkOTSSales.TabIndex = 4;
            this.chkOTSSales.Text = "Sales";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(14, 59);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 16);
            this.label9.TabIndex = 8;
            this.label9.Text = "Balance:";
            // 
            // pnlTYLYTrend
            // 
            this.pnlTYLYTrend.Controls.Add(this.groupBox2);
            this.pnlTYLYTrend.Location = new System.Drawing.Point(915, 27);
            this.pnlTYLYTrend.Name = "pnlTYLYTrend";
            this.pnlTYLYTrend.Size = new System.Drawing.Size(632, 336);
            this.pnlTYLYTrend.TabIndex = 10;
            this.pnlTYLYTrend.VisibleChanged += new System.EventHandler(this.pnlTYLYTrend_VisibleChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tabTYLYTrend);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(8, -2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(616, 339);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Basis";
            // 
            // tabTYLYTrend
            // 
            this.tabTYLYTrend.AllowDrop = true;
            this.tabTYLYTrend.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabTYLYTrend.Controls.Add(this.tabPageTY);
            this.tabTYLYTrend.Controls.Add(this.tabPageLY);
            this.tabTYLYTrend.Controls.Add(this.tabPageApplyTrend);
            this.tabTYLYTrend.Controls.Add(this.tabPageCaps);
            this.tabTYLYTrend.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabTYLYTrend.Location = new System.Drawing.Point(16, 15);
            this.tabTYLYTrend.Name = "tabTYLYTrend";
            this.tabTYLYTrend.SelectedIndex = 0;
            this.tabTYLYTrend.Size = new System.Drawing.Size(584, 318);
            this.tabTYLYTrend.TabIndex = 11;
            this.tabTYLYTrend.SelectedIndexChanged += new System.EventHandler(this.tabTYLYTrend_SelectedIndexChanged);
            this.tabTYLYTrend.DragEnter += new System.Windows.Forms.DragEventHandler(this.control_DragEnter);
            this.tabTYLYTrend.DragOver += new System.Windows.Forms.DragEventHandler(this.control_DragOver);
            // 
            // tabPageTY
            // 
            this.tabPageTY.Controls.Add(this.rdoEqualizeNoTY);
            this.tabPageTY.Controls.Add(this.rdoEqualizeYesTY);
            this.tabPageTY.Controls.Add(this.lblEqualizeWgtTY);
            this.tabPageTY.Controls.Add(this.btnTYIncExc);
            this.tabPageTY.Controls.Add(this.gridTYNodeVersion);
            this.tabPageTY.Controls.Add(this.groupBox5);
            this.tabPageTY.Location = new System.Drawing.Point(4, 22);
            this.tabPageTY.Name = "tabPageTY";
            this.tabPageTY.Size = new System.Drawing.Size(576, 292);
            this.tabPageTY.TabIndex = 0;
            this.tabPageTY.Text = "TY";
            // 
            // rdoEqualizeNoTY
            // 
            this.rdoEqualizeNoTY.Checked = true;
            this.rdoEqualizeNoTY.Location = new System.Drawing.Point(500, 2);
            this.rdoEqualizeNoTY.Name = "rdoEqualizeNoTY";
            this.rdoEqualizeNoTY.Size = new System.Drawing.Size(41, 18);
            this.rdoEqualizeNoTY.TabIndex = 14;
            this.rdoEqualizeNoTY.TabStop = true;
            this.rdoEqualizeNoTY.Text = "No";
            this.rdoEqualizeNoTY.CheckedChanged += new System.EventHandler(this.rdoEqualizeNoTY_CheckedChanged);
            // 
            // rdoEqualizeYesTY
            // 
            this.rdoEqualizeYesTY.Location = new System.Drawing.Point(458, 2);
            this.rdoEqualizeYesTY.Name = "rdoEqualizeYesTY";
            this.rdoEqualizeYesTY.Size = new System.Drawing.Size(43, 18);
            this.rdoEqualizeYesTY.TabIndex = 13;
            this.rdoEqualizeYesTY.Text = "Yes";
            this.rdoEqualizeYesTY.CheckedChanged += new System.EventHandler(this.rdoEqualizeYesTY_CheckedChanged);
            // 
            // lblEqualizeWgtTY
            // 
            this.lblEqualizeWgtTY.Location = new System.Drawing.Point(347, 2);
            this.lblEqualizeWgtTY.Name = "lblEqualizeWgtTY";
            this.lblEqualizeWgtTY.Size = new System.Drawing.Size(106, 18);
            this.lblEqualizeWgtTY.TabIndex = 12;
            this.lblEqualizeWgtTY.Text = "Equalize Weighting:";
            this.lblEqualizeWgtTY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnTYIncExc
            // 
            this.btnTYIncExc.Location = new System.Drawing.Point(366, 52);
            this.btnTYIncExc.Name = "btnTYIncExc";
            this.btnTYIncExc.Size = new System.Drawing.Size(75, 23);
            this.btnTYIncExc.TabIndex = 11;
            this.btnTYIncExc.Visible = false;
            this.btnTYIncExc.Click += new System.EventHandler(this.btnTYIncExc_Click);
            // 
            // gridTYNodeVersion
            // 
            this.gridTYNodeVersion.AllowDrop = true;
            this.gridTYNodeVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridTYNodeVersion.ContextMenu = this.GridContextMenu;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.gridTYNodeVersion.DisplayLayout.Appearance = appearance1;
            this.gridTYNodeVersion.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.gridTYNodeVersion.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.gridTYNodeVersion.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.gridTYNodeVersion.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.gridTYNodeVersion.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.gridTYNodeVersion.DisplayLayout.Override.RowSelectorWidth = 12;
            this.gridTYNodeVersion.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.gridTYNodeVersion.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.gridTYNodeVersion.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.gridTYNodeVersion.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.gridTYNodeVersion.Location = new System.Drawing.Point(40, 27);
            this.gridTYNodeVersion.Name = "gridTYNodeVersion";
            this.gridTYNodeVersion.Size = new System.Drawing.Size(496, 254);
            this.gridTYNodeVersion.TabIndex = 0;
            this.gridTYNodeVersion.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridTYNodeVersion_AfterCellUpdate);
            this.gridTYNodeVersion.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridTYNodeVersion_InitializeLayout);
            this.gridTYNodeVersion.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.gridTYNodeVersion_InitializeRow);
            this.gridTYNodeVersion.AfterExitEditMode += new System.EventHandler(this.gridTYNodeVersion_AfterExitEditMode);
            this.gridTYNodeVersion.AfterRowsDeleted += new System.EventHandler(this.gridTYNodeVersion_AfterRowsDeleted);
            this.gridTYNodeVersion.AfterColRegionScroll += new Infragistics.Win.UltraWinGrid.ColScrollRegionEventHandler(this.gridTYNodeVersion_AfterColRegionScroll);
            this.gridTYNodeVersion.AfterRowRegionScroll += new Infragistics.Win.UltraWinGrid.RowScrollRegionEventHandler(this.gridTYNodeVersion_AfterRowRegionScroll);
            this.gridTYNodeVersion.AfterRowCollapsed += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.gridTYNodeVersion_AfterRowCollapsed);
            this.gridTYNodeVersion.AfterRowExpanded += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.gridTYNodeVersion_AfterRowExpanded);
            this.gridTYNodeVersion.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grid_CellChange);
            this.gridTYNodeVersion.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridTYNodeVersion_ClickCellButton);
            this.gridTYNodeVersion.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grid_AfterCellListCloseUp);
            this.gridTYNodeVersion.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.gridTYNodeVersion_BeforeCellDeactivate);
            this.gridTYNodeVersion.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.gridTYNodeVersion_BeforeRowInsert);
            this.gridTYNodeVersion.AfterColPosChanged += new Infragistics.Win.UltraWinGrid.AfterColPosChangedEventHandler(this.gridTYNodeVersion_AfterColPosChanged);
            this.gridTYNodeVersion.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.GridMouseEnterElement);
            this.gridTYNodeVersion.DragDrop += new System.Windows.Forms.DragEventHandler(this.gridTYNodeVersion_DragDrop);
            this.gridTYNodeVersion.DragEnter += new System.Windows.Forms.DragEventHandler(this.control_DragEnter);
            this.gridTYNodeVersion.DragOver += new System.Windows.Forms.DragEventHandler(this.gridTYNodeVersion_DragOver);
            this.gridTYNodeVersion.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // GridContextMenu
            // 
            this.GridContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuExpandAll,
            this.mnuCollapseAll,
            this.mnuSeparator,
            this.mnuDelete,
            this.mnuDeleteAll,
            this.mnuInsert});
            this.GridContextMenu.Popup += new System.EventHandler(this.GridContextMenu_Popup);
            // 
            // mnuExpandAll
            // 
            this.mnuExpandAll.Index = 0;
            this.mnuExpandAll.Text = "Expand All";
            this.mnuExpandAll.Click += new System.EventHandler(this.mnuExpandAll_Click);
            // 
            // mnuCollapseAll
            // 
            this.mnuCollapseAll.Index = 1;
            this.mnuCollapseAll.Text = "Collapse All";
            this.mnuCollapseAll.Click += new System.EventHandler(this.mnuCollapseAll_Click);
            // 
            // mnuSeparator
            // 
            this.mnuSeparator.Index = 2;
            this.mnuSeparator.Text = "-";
            // 
            // mnuDelete
            // 
            this.mnuDelete.Index = 3;
            this.mnuDelete.Text = "Delete";
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // mnuDeleteAll
            // 
            this.mnuDeleteAll.Index = 4;
            this.mnuDeleteAll.Text = "Delete All";
            this.mnuDeleteAll.Click += new System.EventHandler(this.mnuDeleteAll_Click);
            // 
            // mnuInsert
            // 
            this.mnuInsert.Index = 5;
            this.mnuInsert.Text = "Insert";
            this.mnuInsert.Click += new System.EventHandler(this.mnuInsert_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Location = new System.Drawing.Point(26, 15);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(530, 275);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            // 
            // tabPageLY
            // 
            this.tabPageLY.Controls.Add(this.rdoEqualizeNoLY);
            this.tabPageLY.Controls.Add(this.rdoEqualizeYesLY);
            this.tabPageLY.Controls.Add(this.lblEqualizeWgtLY);
            this.tabPageLY.Controls.Add(this.cbxAltLY);
            this.tabPageLY.Controls.Add(this.linkLabel1);
            this.tabPageLY.Controls.Add(this.btnLYIncExc);
            this.tabPageLY.Controls.Add(this.gridLYNodeVersion);
            this.tabPageLY.Controls.Add(this.groupBox4);
            this.tabPageLY.Location = new System.Drawing.Point(4, 22);
            this.tabPageLY.Name = "tabPageLY";
            this.tabPageLY.Size = new System.Drawing.Size(576, 292);
            this.tabPageLY.TabIndex = 1;
            this.tabPageLY.Text = "LY";
            this.tabPageLY.Click += new System.EventHandler(this.tabPageLY_Click);
            // 
            // rdoEqualizeNoLY
            // 
            this.rdoEqualizeNoLY.Checked = true;
            this.rdoEqualizeNoLY.Enabled = false;
            this.rdoEqualizeNoLY.Location = new System.Drawing.Point(500, 2);
            this.rdoEqualizeNoLY.Name = "rdoEqualizeNoLY";
            this.rdoEqualizeNoLY.Size = new System.Drawing.Size(41, 18);
            this.rdoEqualizeNoLY.TabIndex = 9;
            this.rdoEqualizeNoLY.TabStop = true;
            this.rdoEqualizeNoLY.Text = "No";
            this.rdoEqualizeNoLY.CheckedChanged += new System.EventHandler(this.rdoEqualizeNoLY_CheckedChanged);
            // 
            // rdoEqualizeYesLY
            // 
            this.rdoEqualizeYesLY.Enabled = false;
            this.rdoEqualizeYesLY.Location = new System.Drawing.Point(458, 2);
            this.rdoEqualizeYesLY.Name = "rdoEqualizeYesLY";
            this.rdoEqualizeYesLY.Size = new System.Drawing.Size(43, 18);
            this.rdoEqualizeYesLY.TabIndex = 8;
            this.rdoEqualizeYesLY.Text = "Yes";
            this.rdoEqualizeYesLY.CheckedChanged += new System.EventHandler(this.rdoEqualizeYesLY_CheckedChanged);
            // 
            // lblEqualizeWgtLY
            // 
            this.lblEqualizeWgtLY.Location = new System.Drawing.Point(347, 2);
            this.lblEqualizeWgtLY.Name = "lblEqualizeWgtLY";
            this.lblEqualizeWgtLY.Size = new System.Drawing.Size(106, 18);
            this.lblEqualizeWgtLY.TabIndex = 7;
            this.lblEqualizeWgtLY.Text = "Equalize Weighting:";
            this.lblEqualizeWgtLY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxAltLY
            // 
            this.cbxAltLY.Location = new System.Drawing.Point(226, 2);
            this.cbxAltLY.Name = "cbxAltLY";
            this.cbxAltLY.Size = new System.Drawing.Size(86, 18);
            this.cbxAltLY.TabIndex = 6;
            this.cbxAltLY.Text = "Alternate";
            this.cbxAltLY.CheckedChanged += new System.EventHandler(this.cbxAltLY_CheckedChanged);
            // 
            // linkLabel1
            // 
            this.linkLabel1.Location = new System.Drawing.Point(61, 1);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(0, 1);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "linkLabel1";
            // 
            // btnLYIncExc
            // 
            this.btnLYIncExc.Location = new System.Drawing.Point(165, 47);
            this.btnLYIncExc.Name = "btnLYIncExc";
            this.btnLYIncExc.Size = new System.Drawing.Size(75, 23);
            this.btnLYIncExc.TabIndex = 3;
            this.btnLYIncExc.Visible = false;
            this.btnLYIncExc.Click += new System.EventHandler(this.btnLYIncExc_Click);
            // 
            // gridLYNodeVersion
            // 
            this.gridLYNodeVersion.AllowDrop = true;
            this.gridLYNodeVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridLYNodeVersion.ContextMenu = this.GridContextMenu;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.gridLYNodeVersion.DisplayLayout.Appearance = appearance7;
            this.gridLYNodeVersion.DisplayLayout.InterBandSpacing = 10;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.gridLYNodeVersion.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.TextHAlignAsString = "Left";
            appearance9.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.gridLYNodeVersion.DisplayLayout.Override.HeaderAppearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.gridLYNodeVersion.DisplayLayout.Override.RowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.gridLYNodeVersion.DisplayLayout.Override.RowSelectorAppearance = appearance11;
            this.gridLYNodeVersion.DisplayLayout.Override.RowSelectorWidth = 12;
            this.gridLYNodeVersion.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.gridLYNodeVersion.DisplayLayout.Override.SelectedRowAppearance = appearance12;
            this.gridLYNodeVersion.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.gridLYNodeVersion.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.gridLYNodeVersion.Location = new System.Drawing.Point(40, 27);
            this.gridLYNodeVersion.Name = "gridLYNodeVersion";
            this.gridLYNodeVersion.Size = new System.Drawing.Size(496, 254);
            this.gridLYNodeVersion.TabIndex = 0;
            this.gridLYNodeVersion.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridLYNodeVersion_AfterCellUpdate);
            this.gridLYNodeVersion.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridLYNodeVersion_InitializeLayout);
            this.gridLYNodeVersion.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.gridLYNodeVersion_InitializeRow);
            this.gridLYNodeVersion.AfterExitEditMode += new System.EventHandler(this.gridLYNodeVersion_AfterExitEditMode);
            this.gridLYNodeVersion.AfterRowsDeleted += new System.EventHandler(this.gridLYNodeVersion_AfterRowsDeleted);
            this.gridLYNodeVersion.AfterColRegionScroll += new Infragistics.Win.UltraWinGrid.ColScrollRegionEventHandler(this.gridLYNodeVersion_AfterColRegionScroll);
            this.gridLYNodeVersion.AfterRowRegionScroll += new Infragistics.Win.UltraWinGrid.RowScrollRegionEventHandler(this.gridLYNodeVersion_AfterRowRegionScroll);
            this.gridLYNodeVersion.AfterRowCollapsed += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.gridLYNodeVersion_AfterRowCollapsed);
            this.gridLYNodeVersion.AfterRowExpanded += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.gridLYNodeVersion_AfterRowExpanded);
            this.gridLYNodeVersion.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grid_CellChange);
            this.gridLYNodeVersion.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridLYNodeVersion_ClickCellButton);
            this.gridLYNodeVersion.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grid_AfterCellListCloseUp);
            this.gridLYNodeVersion.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.gridLYNodeVersion_BeforeCellDeactivate);
            this.gridLYNodeVersion.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.gridLYNodeVersion_BeforeRowInsert);
            this.gridLYNodeVersion.AfterColPosChanged += new Infragistics.Win.UltraWinGrid.AfterColPosChangedEventHandler(this.gridLYNodeVersion_AfterColPosChanged);
            this.gridLYNodeVersion.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.GridMouseEnterElement);
            this.gridLYNodeVersion.DragDrop += new System.Windows.Forms.DragEventHandler(this.gridLYNodeVersion_DragDrop);
            this.gridLYNodeVersion.DragEnter += new System.Windows.Forms.DragEventHandler(this.control_DragEnter);
            this.gridLYNodeVersion.DragOver += new System.Windows.Forms.DragEventHandler(this.gridLYNodeVersion_DragOver);
            this.gridLYNodeVersion.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // groupBox4
            // 
            this.groupBox4.Location = new System.Drawing.Point(26, 15);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(530, 275);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Enter += new System.EventHandler(this.groupBox4_Enter);
            // 
            // tabPageApplyTrend
            // 
            this.tabPageApplyTrend.Controls.Add(this.cbxProjCurrWkSales);
            this.tabPageApplyTrend.Controls.Add(this.rdoEqualizeNoTrend);
            this.tabPageApplyTrend.Controls.Add(this.rdoEqualizeYesTrend);
            this.tabPageApplyTrend.Controls.Add(this.lblEqualizeWgtTrend);
            this.tabPageApplyTrend.Controls.Add(this.cbxAltTrend);
            this.tabPageApplyTrend.Controls.Add(this.btnTrendIncExc);
            this.tabPageApplyTrend.Controls.Add(this.gridTrendNodeVersion);
            this.tabPageApplyTrend.Controls.Add(this.groupBox3);
            this.tabPageApplyTrend.Location = new System.Drawing.Point(4, 22);
            this.tabPageApplyTrend.Name = "tabPageApplyTrend";
            this.tabPageApplyTrend.Size = new System.Drawing.Size(576, 292);
            this.tabPageApplyTrend.TabIndex = 2;
            this.tabPageApplyTrend.Text = "Apply Trend To";
            // 
            // cbxProjCurrWkSales
            // 
            this.cbxProjCurrWkSales.Location = new System.Drawing.Point(40, -1);
            this.cbxProjCurrWkSales.Name = "cbxProjCurrWkSales";
            this.cbxProjCurrWkSales.Size = new System.Drawing.Size(161, 21);
            this.cbxProjCurrWkSales.TabIndex = 18;
            this.cbxProjCurrWkSales.Text = "Project Current Week Sales";
            this.cbxProjCurrWkSales.CheckedChanged += new System.EventHandler(this.cbxProjCurrWkSales_CheckedChanged);
            // 
            // rdoEqualizeNoTrend
            // 
            this.rdoEqualizeNoTrend.Checked = true;
            this.rdoEqualizeNoTrend.Enabled = false;
            this.rdoEqualizeNoTrend.Location = new System.Drawing.Point(500, 2);
            this.rdoEqualizeNoTrend.Name = "rdoEqualizeNoTrend";
            this.rdoEqualizeNoTrend.Size = new System.Drawing.Size(41, 18);
            this.rdoEqualizeNoTrend.TabIndex = 8;
            this.rdoEqualizeNoTrend.TabStop = true;
            this.rdoEqualizeNoTrend.Text = "No";
            this.rdoEqualizeNoTrend.CheckedChanged += new System.EventHandler(this.rdoEqualizeNoTrend_CheckedChanged);
            // 
            // rdoEqualizeYesTrend
            // 
            this.rdoEqualizeYesTrend.Enabled = false;
            this.rdoEqualizeYesTrend.Location = new System.Drawing.Point(458, 2);
            this.rdoEqualizeYesTrend.Name = "rdoEqualizeYesTrend";
            this.rdoEqualizeYesTrend.Size = new System.Drawing.Size(43, 18);
            this.rdoEqualizeYesTrend.TabIndex = 7;
            this.rdoEqualizeYesTrend.Text = "Yes";
            this.rdoEqualizeYesTrend.CheckedChanged += new System.EventHandler(this.rdoEqualizeYesTrend_CheckedChanged);
            // 
            // lblEqualizeWgtTrend
            // 
            this.lblEqualizeWgtTrend.Location = new System.Drawing.Point(347, 2);
            this.lblEqualizeWgtTrend.Name = "lblEqualizeWgtTrend";
            this.lblEqualizeWgtTrend.Size = new System.Drawing.Size(106, 18);
            this.lblEqualizeWgtTrend.TabIndex = 6;
            this.lblEqualizeWgtTrend.Text = "Equalize Weighting:";
            this.lblEqualizeWgtTrend.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxAltTrend
            // 
            this.cbxAltTrend.Location = new System.Drawing.Point(226, 2);
            this.cbxAltTrend.Name = "cbxAltTrend";
            this.cbxAltTrend.Size = new System.Drawing.Size(86, 18);
            this.cbxAltTrend.TabIndex = 5;
            this.cbxAltTrend.Text = "Alternate";
            this.cbxAltTrend.CheckedChanged += new System.EventHandler(this.cbxAltTrend_CheckedChanged);
            // 
            // btnTrendIncExc
            // 
            this.btnTrendIncExc.Location = new System.Drawing.Point(258, 39);
            this.btnTrendIncExc.Name = "btnTrendIncExc";
            this.btnTrendIncExc.Size = new System.Drawing.Size(75, 23);
            this.btnTrendIncExc.TabIndex = 3;
            this.btnTrendIncExc.Visible = false;
            this.btnTrendIncExc.Click += new System.EventHandler(this.btnTrendIncExc_Click);
            // 
            // gridTrendNodeVersion
            // 
            this.gridTrendNodeVersion.AllowDrop = true;
            this.gridTrendNodeVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridTrendNodeVersion.ContextMenu = this.GridContextMenu;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.gridTrendNodeVersion.DisplayLayout.Appearance = appearance13;
            this.gridTrendNodeVersion.DisplayLayout.InterBandSpacing = 10;
            appearance14.BackColor = System.Drawing.Color.Transparent;
            this.gridTrendNodeVersion.DisplayLayout.Override.CardAreaAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.ForeColor = System.Drawing.Color.Black;
            appearance15.TextHAlignAsString = "Left";
            appearance15.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.gridTrendNodeVersion.DisplayLayout.Override.HeaderAppearance = appearance15;
            appearance16.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.gridTrendNodeVersion.DisplayLayout.Override.RowAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.gridTrendNodeVersion.DisplayLayout.Override.RowSelectorAppearance = appearance17;
            this.gridTrendNodeVersion.DisplayLayout.Override.RowSelectorWidth = 12;
            this.gridTrendNodeVersion.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.ForeColor = System.Drawing.Color.Black;
            this.gridTrendNodeVersion.DisplayLayout.Override.SelectedRowAppearance = appearance18;
            this.gridTrendNodeVersion.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.gridTrendNodeVersion.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.gridTrendNodeVersion.Location = new System.Drawing.Point(40, 27);
            this.gridTrendNodeVersion.Name = "gridTrendNodeVersion";
            this.gridTrendNodeVersion.Size = new System.Drawing.Size(496, 254);
            this.gridTrendNodeVersion.TabIndex = 0;
            this.gridTrendNodeVersion.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridTrendNodeVersion_AfterCellUpdate);
            this.gridTrendNodeVersion.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridTrendNodeVersion_InitializeLayout);
            this.gridTrendNodeVersion.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.gridTrendNodeVersion_InitializeRow);
            this.gridTrendNodeVersion.AfterExitEditMode += new System.EventHandler(this.gridTrendNodeVersion_AfterExitEditMode);
            this.gridTrendNodeVersion.AfterRowsDeleted += new System.EventHandler(this.gridTrendNodeVersion_AfterRowsDeleted);
            this.gridTrendNodeVersion.AfterColRegionScroll += new Infragistics.Win.UltraWinGrid.ColScrollRegionEventHandler(this.gridTrendNodeVersion_AfterColRegionScroll);
            this.gridTrendNodeVersion.AfterRowRegionScroll += new Infragistics.Win.UltraWinGrid.RowScrollRegionEventHandler(this.gridTrendNodeVersion_AfterRowRegionScroll);
            this.gridTrendNodeVersion.AfterRowCollapsed += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.gridTrendNodeVersion_AfterRowCollapsed);
            this.gridTrendNodeVersion.AfterRowExpanded += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.gridTrendNodeVersion_AfterRowExpanded);
            this.gridTrendNodeVersion.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridTrendNodeVersion_ClickCellButton);
            this.gridTrendNodeVersion.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grid_AfterCellListCloseUp);
            this.gridTrendNodeVersion.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.gridTrendNodeVersion_BeforeCellDeactivate);
            this.gridTrendNodeVersion.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.gridTrendNodeVersion_BeforeRowInsert);
            this.gridTrendNodeVersion.AfterColPosChanged += new Infragistics.Win.UltraWinGrid.AfterColPosChangedEventHandler(this.gridTrendNodeVersion_AfterColPosChanged);
            this.gridTrendNodeVersion.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.GridMouseEnterElement);
            this.gridTrendNodeVersion.DragDrop += new System.Windows.Forms.DragEventHandler(this.gridTrendNodeVersion_DragDrop);
            this.gridTrendNodeVersion.DragEnter += new System.Windows.Forms.DragEventHandler(this.control_DragEnter);
            this.gridTrendNodeVersion.DragOver += new System.Windows.Forms.DragEventHandler(this.gridTrendNodeVersion_DragOver);
            this.gridTrendNodeVersion.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(26, 15);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(530, 275);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            // 
            // tabPageCaps
            // 
            this.tabPageCaps.Controls.Add(this.grpTrendCaps);
            this.tabPageCaps.Location = new System.Drawing.Point(4, 22);
            this.tabPageCaps.Name = "tabPageCaps";
            this.tabPageCaps.Size = new System.Drawing.Size(576, 292);
            this.tabPageCaps.TabIndex = 3;
            this.tabPageCaps.Text = "Trend Caps";
            // 
            // grpTrendCaps
            // 
            this.grpTrendCaps.Controls.Add(this.radNone);
            this.grpTrendCaps.Controls.Add(this.radTolerance);
            this.grpTrendCaps.Controls.Add(this.txtTolerance);
            this.grpTrendCaps.Controls.Add(this.radLimits);
            this.grpTrendCaps.Controls.Add(this.lblHigh);
            this.grpTrendCaps.Controls.Add(this.txtHigh);
            this.grpTrendCaps.Controls.Add(this.lblLow);
            this.grpTrendCaps.Controls.Add(this.txtLow);
            this.grpTrendCaps.Location = new System.Drawing.Point(32, 16);
            this.grpTrendCaps.Name = "grpTrendCaps";
            this.grpTrendCaps.Size = new System.Drawing.Size(512, 248);
            this.grpTrendCaps.TabIndex = 0;
            this.grpTrendCaps.TabStop = false;
            this.grpTrendCaps.Text = "Trend Caps";
            // 
            // radNone
            // 
            this.radNone.Location = new System.Drawing.Point(136, 32);
            this.radNone.Name = "radNone";
            this.radNone.Size = new System.Drawing.Size(55, 16);
            this.radNone.TabIndex = 19;
            this.radNone.Text = "None";
            this.radNone.CheckedChanged += new System.EventHandler(this.radNone_CheckedChanged);
            // 
            // radTolerance
            // 
            this.radTolerance.Location = new System.Drawing.Point(136, 72);
            this.radTolerance.Name = "radTolerance";
            this.radTolerance.Size = new System.Drawing.Size(82, 16);
            this.radTolerance.TabIndex = 17;
            this.radTolerance.Text = "Tolerance:";
            this.radTolerance.CheckedChanged += new System.EventHandler(this.radTolerance_CheckedChanged);
            // 
            // txtTolerance
            // 
            this.txtTolerance.Location = new System.Drawing.Point(256, 72);
            this.txtTolerance.MaxLength = 5;
            this.txtTolerance.Name = "txtTolerance";
            this.txtTolerance.Size = new System.Drawing.Size(56, 20);
            this.txtTolerance.TabIndex = 13;
            // 
            // radLimits
            // 
            this.radLimits.Location = new System.Drawing.Point(136, 120);
            this.radLimits.Name = "radLimits";
            this.radLimits.Size = new System.Drawing.Size(58, 16);
            this.radLimits.TabIndex = 18;
            this.radLimits.Text = "Limits:";
            this.radLimits.CheckedChanged += new System.EventHandler(this.radLimits_CheckedChanged);
            // 
            // lblHigh
            // 
            this.lblHigh.Location = new System.Drawing.Point(224, 122);
            this.lblHigh.Name = "lblHigh";
            this.lblHigh.Size = new System.Drawing.Size(30, 16);
            this.lblHigh.TabIndex = 0;
            this.lblHigh.Text = "High";
            // 
            // txtHigh
            // 
            this.txtHigh.Location = new System.Drawing.Point(256, 120);
            this.txtHigh.MaxLength = 7;
            this.txtHigh.Name = "txtHigh";
            this.txtHigh.Size = new System.Drawing.Size(56, 20);
            this.txtHigh.TabIndex = 14;
            // 
            // lblLow
            // 
            this.lblLow.Location = new System.Drawing.Point(224, 154);
            this.lblLow.Name = "lblLow";
            this.lblLow.Size = new System.Drawing.Size(28, 16);
            this.lblLow.TabIndex = 1;
            this.lblLow.Text = "Low";
            // 
            // txtLow
            // 
            this.txtLow.Location = new System.Drawing.Point(256, 152);
            this.txtLow.MaxLength = 7;
            this.txtLow.Name = "txtLow";
            this.txtLow.Size = new System.Drawing.Size(56, 20);
            this.txtLow.TabIndex = 15;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 80);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            // 
            // tabSetMethods
            // 
            this.tabSetMethods.Controls.Add(this.grpGroupLevelMethod);
            this.tabSetMethods.Location = new System.Drawing.Point(4, 22);
            this.tabSetMethods.Name = "tabSetMethods";
            this.tabSetMethods.Size = new System.Drawing.Size(677, 470);
            this.tabSetMethods.TabIndex = 2;
            this.tabSetMethods.Text = "Set Methods";
            // 
            // grpGroupLevelMethod
            // 
            this.grpGroupLevelMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpGroupLevelMethod.Controls.Add(this.cbxStoreGroupLevel);
            this.grpGroupLevelMethod.Controls.Add(this.lblSet);
            this.grpGroupLevelMethod.Controls.Add(this.lblAttribute);
            this.grpGroupLevelMethod.Controls.Add(this.cboStoreGroups);
            this.grpGroupLevelMethod.Controls.Add(this.btnCopy);
            this.grpGroupLevelMethod.Controls.Add(this.tabSetMethod);
            this.grpGroupLevelMethod.Controls.Add(this.chkUseDefault);
            this.grpGroupLevelMethod.Controls.Add(this.chkClear);
            this.grpGroupLevelMethod.Controls.Add(this.chkPlan);
            this.grpGroupLevelMethod.Controls.Add(this.chkDefault);
            this.grpGroupLevelMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpGroupLevelMethod.Location = new System.Drawing.Point(8, 8);
            this.grpGroupLevelMethod.Name = "grpGroupLevelMethod";
            this.grpGroupLevelMethod.Size = new System.Drawing.Size(664, 456);
            this.grpGroupLevelMethod.TabIndex = 2;
            this.grpGroupLevelMethod.TabStop = false;
            this.grpGroupLevelMethod.Text = "Set Method";
            // 
            // cbxStoreGroupLevel
            // 
            this.cbxStoreGroupLevel.AutoAdjust = true;
            this.cbxStoreGroupLevel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbxStoreGroupLevel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxStoreGroupLevel.AutoScroll = true;
            this.cbxStoreGroupLevel.DataSource = null;
            this.cbxStoreGroupLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxStoreGroupLevel.DropDownWidth = 211;
            this.cbxStoreGroupLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxStoreGroupLevel.FormattingEnabled = false;
            this.cbxStoreGroupLevel.Location = new System.Drawing.Point(358, 33);
            this.cbxStoreGroupLevel.Margin = new System.Windows.Forms.Padding(0);
            this.cbxStoreGroupLevel.Name = "cbxStoreGroupLevel";
            this.cbxStoreGroupLevel.Size = new System.Drawing.Size(211, 21);
            this.cbxStoreGroupLevel.TabIndex = 10;
            this.cbxStoreGroupLevel.Tag = null;
            this.cbxStoreGroupLevel.SelectionChangeCommitted += new System.EventHandler(this.cboStoreGroupLevel_SelectionChangeCommitted);
            this.cbxStoreGroupLevel.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cbxStoreGroupLevel_MIDComboBoxPropertiesChangedEvent);
            // 
            // lblSet
            // 
            this.lblSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSet.Location = new System.Drawing.Point(302, 39);
            this.lblSet.Name = "lblSet";
            this.lblSet.Size = new System.Drawing.Size(48, 16);
            this.lblSet.TabIndex = 20;
            this.lblSet.Text = "Set:";
            // 
            // lblAttribute
            // 
            this.lblAttribute.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAttribute.Location = new System.Drawing.Point(302, 16);
            this.lblAttribute.Name = "lblAttribute";
            this.lblAttribute.Size = new System.Drawing.Size(48, 16);
            this.lblAttribute.TabIndex = 18;
            this.lblAttribute.Text = "Attribute:";
            // 
            // cboStoreGroups
            // 
            this.cboStoreGroups.AllowDrop = true;
            this.cboStoreGroups.AllowUserAttributes = false;
            this.cboStoreGroups.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStoreGroups.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStoreGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStoreGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboStoreGroups.Location = new System.Drawing.Point(358, 6);
            this.cboStoreGroups.Name = "cboStoreGroups";
            this.cboStoreGroups.Size = new System.Drawing.Size(211, 21);
            this.cboStoreGroups.TabIndex = 19;
            this.cboStoreGroups.SelectionChangeCommitted += new System.EventHandler(this.cboStoreGroups_SelectionChangeCommitted);
            this.cboStoreGroups.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboStoreGroups_MIDComboBoxPropertiesChangedEvent);
            this.cboStoreGroups.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboStoreGroups_DragEnter);
            this.cboStoreGroups.DragOver += new System.Windows.Forms.DragEventHandler(this.cboStoreGroups_DragOver);
            // 
            // btnCopy
            // 
            this.btnCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopy.Location = new System.Drawing.Point(576, 31);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 23);
            this.btnCopy.TabIndex = 2;
            this.btnCopy.Text = "Co&py";
            // 
            // tabSetMethod
            // 
            this.tabSetMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabSetMethod.Controls.Add(this.tabCriteria);
            this.tabSetMethod.Controls.Add(this.tabStockMinMax);
            this.tabSetMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabSetMethod.Location = new System.Drawing.Point(8, 36);
            this.tabSetMethod.Name = "tabSetMethod";
            this.tabSetMethod.SelectedIndex = 0;
            this.tabSetMethod.Size = new System.Drawing.Size(648, 412);
            this.tabSetMethod.TabIndex = 9;
            this.tabSetMethod.DragEnter += new System.Windows.Forms.DragEventHandler(this.control_DragEnter);
            this.tabSetMethod.DragOver += new System.Windows.Forms.DragEventHandler(this.control_DragOver);
            // 
            // tabCriteria
            // 
            this.tabCriteria.Controls.Add(this.lblWidth30);
            this.tabCriteria.Controls.Add(this.panel2);
            this.tabCriteria.Controls.Add(this.pnlPercentContribution);
            this.tabCriteria.Location = new System.Drawing.Point(4, 22);
            this.tabCriteria.Name = "tabCriteria";
            this.tabCriteria.Size = new System.Drawing.Size(640, 386);
            this.tabCriteria.TabIndex = 0;
            this.tabCriteria.Text = "Criteria";
            // 
            // lblWidth30
            // 
            this.lblWidth30.Location = new System.Drawing.Point(528, 16);
            this.lblWidth30.Name = "lblWidth30";
            this.lblWidth30.Size = new System.Drawing.Size(35, 10);
            this.lblWidth30.TabIndex = 4;
            this.lblWidth30.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.cboGLGroupBy);
            this.panel2.Controls.Add(this.cboFuncType);
            this.panel2.Controls.Add(this.lblGLGroupBy);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.Location = new System.Drawing.Point(8, 7);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(488, 33);
            this.panel2.TabIndex = 1;
            // 
            // cboGLGroupBy
            // 
            this.cboGLGroupBy.AutoAdjust = true;
            this.cboGLGroupBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboGLGroupBy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboGLGroupBy.AutoScroll = true;
            this.cboGLGroupBy.DataSource = null;
            this.cboGLGroupBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGLGroupBy.DropDownWidth = 160;
            this.cboGLGroupBy.FormattingEnabled = false;
            this.cboGLGroupBy.Location = new System.Drawing.Point(312, 6);
            this.cboGLGroupBy.Margin = new System.Windows.Forms.Padding(0);
            this.cboGLGroupBy.Name = "cboGLGroupBy";
            this.cboGLGroupBy.Size = new System.Drawing.Size(160, 21);
            this.cboGLGroupBy.TabIndex = 21;
            this.cboGLGroupBy.Tag = null;
            this.cboGLGroupBy.SelectionChangeCommitted += new System.EventHandler(this.cboGLGroupBy_SelectionChangeCommitted);
            this.cboGLGroupBy.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboGLGroupBy_MIDComboBoxPropertiesChangedEvent);
            // 
            // cboFuncType
            // 
            this.cboFuncType.AutoAdjust = false;
            this.cboFuncType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboFuncType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboFuncType.AutoScroll = true;
            this.cboFuncType.DataSource = null;
            this.cboFuncType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFuncType.DropDownWidth = 160;
            this.cboFuncType.FormattingEnabled = false;
            this.cboFuncType.Location = new System.Drawing.Point(56, 6);
            this.cboFuncType.Margin = new System.Windows.Forms.Padding(0);
            this.cboFuncType.Name = "cboFuncType";
            this.cboFuncType.Size = new System.Drawing.Size(160, 21);
            this.cboFuncType.TabIndex = 20;
            this.cboFuncType.Tag = null;
            this.cboFuncType.SelectionChangeCommitted += new System.EventHandler(this.cboFuncType_SelectionChangeCommitted);
            this.cboFuncType.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboFuncType_MIDComboBoxPropertiesChangedEvent);
            this.cboFuncType.SelectedValueChanged += new System.EventHandler(this.cboFuncType_SelectedValueChanged);
            // 
            // lblGLGroupBy
            // 
            this.lblGLGroupBy.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblGLGroupBy.Location = new System.Drawing.Point(247, 11);
            this.lblGLGroupBy.Name = "lblGLGroupBy";
            this.lblGLGroupBy.Size = new System.Drawing.Size(64, 16);
            this.lblGLGroupBy.TabIndex = 14;
            this.lblGLGroupBy.Text = "Smooth By:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.Location = new System.Drawing.Point(7, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 16);
            this.label4.TabIndex = 12;
            this.label4.Text = "Method:";
            // 
            // pnlPercentContribution
            // 
            this.pnlPercentContribution.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPercentContribution.Controls.Add(this.grpContributionBasis);
            this.pnlPercentContribution.Location = new System.Drawing.Point(0, 44);
            this.pnlPercentContribution.Name = "pnlPercentContribution";
            this.pnlPercentContribution.Size = new System.Drawing.Size(632, 340);
            this.pnlPercentContribution.TabIndex = 9;
            this.pnlPercentContribution.VisibleChanged += new System.EventHandler(this.pnlPercentContribution_VisibleChanged);
            // 
            // grpContributionBasis
            // 
            this.grpContributionBasis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpContributionBasis.Controls.Add(this.grdPctContBasis);
            this.grpContributionBasis.Controls.Add(this.btnIncExc);
            this.grpContributionBasis.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpContributionBasis.Location = new System.Drawing.Point(8, 0);
            this.grpContributionBasis.Name = "grpContributionBasis";
            this.grpContributionBasis.Size = new System.Drawing.Size(629, 336);
            this.grpContributionBasis.TabIndex = 3;
            this.grpContributionBasis.TabStop = false;
            this.grpContributionBasis.Text = "Basis";
            // 
            // grdPctContBasis
            // 
            this.grdPctContBasis.AllowDrop = true;
            this.grdPctContBasis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdPctContBasis.ContextMenu = this.GridContextMenu;
            appearance19.BackColor = System.Drawing.Color.White;
            appearance19.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.grdPctContBasis.DisplayLayout.Appearance = appearance19;
            this.grdPctContBasis.DisplayLayout.InterBandSpacing = 10;
            appearance20.BackColor = System.Drawing.Color.Transparent;
            this.grdPctContBasis.DisplayLayout.Override.CardAreaAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance21.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance21.ForeColor = System.Drawing.Color.Black;
            appearance21.TextHAlignAsString = "Left";
            appearance21.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdPctContBasis.DisplayLayout.Override.HeaderAppearance = appearance21;
            appearance22.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdPctContBasis.DisplayLayout.Override.RowAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance23.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdPctContBasis.DisplayLayout.Override.RowSelectorAppearance = appearance23;
            this.grdPctContBasis.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdPctContBasis.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance24.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance24.ForeColor = System.Drawing.Color.Black;
            this.grdPctContBasis.DisplayLayout.Override.SelectedRowAppearance = appearance24;
            this.grdPctContBasis.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdPctContBasis.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.grdPctContBasis.Location = new System.Drawing.Point(8, 16);
            this.grdPctContBasis.Name = "grdPctContBasis";
            this.grdPctContBasis.Size = new System.Drawing.Size(599, 312);
            this.grdPctContBasis.TabIndex = 10;
            this.grdPctContBasis.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdPctContBasis_AfterCellUpdate);
            this.grdPctContBasis.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdPctContBasis_InitializeLayout);
            this.grdPctContBasis.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdPctContBasis_InitializeRow);
            this.grdPctContBasis.AfterExitEditMode += new System.EventHandler(this.grdPctContBasis_AfterExitEditMode);
            this.grdPctContBasis.AfterRowsDeleted += new System.EventHandler(this.grdPctContBasis_AfterRowsDeleted);
            this.grdPctContBasis.AfterColRegionScroll += new Infragistics.Win.UltraWinGrid.ColScrollRegionEventHandler(this.grdPctContBasis_AfterColRegionScroll);
            this.grdPctContBasis.AfterRowRegionScroll += new Infragistics.Win.UltraWinGrid.RowScrollRegionEventHandler(this.grdPctContBasis_AfterRowRegionScroll);
            this.grdPctContBasis.AfterRowCollapsed += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdPctContBasis_AfterRowCollapsed);
            this.grdPctContBasis.AfterRowExpanded += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdPctContBasis_AfterRowExpanded);
            this.grdPctContBasis.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdPctContBasis_ClickCellButton);
            this.grdPctContBasis.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grid_AfterCellListCloseUp);
            this.grdPctContBasis.BeforeCellActivate += new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.grdPctContBasis_BeforeCellActivate);
            this.grdPctContBasis.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.grdPctContBasis_BeforeCellDeactivate);
            this.grdPctContBasis.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.grdPctContBasis_BeforeRowInsert);
            this.grdPctContBasis.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.grdPctContBasis_BeforeRowsDeleted);
            this.grdPctContBasis.AfterColPosChanged += new Infragistics.Win.UltraWinGrid.AfterColPosChangedEventHandler(this.grdPctContBasis_AfterColPosChanged);
            this.grdPctContBasis.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.GridMouseEnterElement);
            this.grdPctContBasis.DragDrop += new System.Windows.Forms.DragEventHandler(this.grdPctContBasis_DragDrop);
            this.grdPctContBasis.DragEnter += new System.Windows.Forms.DragEventHandler(this.control_DragEnter);
            this.grdPctContBasis.DragOver += new System.Windows.Forms.DragEventHandler(this.grdPctContBasis_DragOver);
            this.grdPctContBasis.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            this.grdPctContBasis.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdPctContBasis_MouseUp);
            // 
            // btnIncExc
            // 
            this.btnIncExc.Location = new System.Drawing.Point(16, 80);
            this.btnIncExc.Name = "btnIncExc";
            this.btnIncExc.Size = new System.Drawing.Size(75, 23);
            this.btnIncExc.TabIndex = 8;
            this.btnIncExc.Click += new System.EventHandler(this.btnIncExc_Click);
            // 
            // tabStockMinMax
            // 
            this.tabStockMinMax.Controls.Add(this.cboMerchandise);
            this.tabStockMinMax.Controls.Add(this.rdoDefault);
            this.tabStockMinMax.Controls.Add(this.chkApplyMinMaxes);
            this.tabStockMinMax.Controls.Add(this.rdoNone);
            this.tabStockMinMax.Controls.Add(this.rdoHierarchy);
            this.tabStockMinMax.Controls.Add(this.rdoMethod);
            this.tabStockMinMax.Controls.Add(this.lblLowLevelDefault);
            this.tabStockMinMax.Controls.Add(this.lblLowLevelMerchandise);
            this.tabStockMinMax.Controls.Add(this.gridStockMinMax);
            this.tabStockMinMax.Location = new System.Drawing.Point(4, 22);
            this.tabStockMinMax.Name = "tabStockMinMax";
            this.tabStockMinMax.Size = new System.Drawing.Size(640, 386);
            this.tabStockMinMax.TabIndex = 1;
            this.tabStockMinMax.Text = "Stock Min/Max";
            this.tabStockMinMax.Visible = false;
            // 
            // cboMerchandise
            // 
            this.cboMerchandise.AutoAdjust = true;
            this.cboMerchandise.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboMerchandise.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboMerchandise.AutoScroll = true;
            this.cboMerchandise.DataSource = null;
            this.cboMerchandise.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMerchandise.DropDownWidth = 321;
            this.cboMerchandise.FormattingEnabled = false;
            this.cboMerchandise.Location = new System.Drawing.Point(98, 9);
            this.cboMerchandise.Margin = new System.Windows.Forms.Padding(0);
            this.cboMerchandise.Name = "cboMerchandise";
            this.cboMerchandise.Size = new System.Drawing.Size(321, 21);
            this.cboMerchandise.TabIndex = 1;
            this.cboMerchandise.Tag = null;
            this.cboMerchandise.SelectionChangeCommitted += new System.EventHandler(this.cboMerchandise_SelectionChangeCommitted);
            this.cboMerchandise.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboMerchandise_MIDComboBoxPropertiesChangedEvent);
            // 
            // rdoDefault
            // 
            this.rdoDefault.Enabled = false;
            this.rdoDefault.Location = new System.Drawing.Point(390, 34);
            this.rdoDefault.Name = "rdoDefault";
            this.rdoDefault.Size = new System.Drawing.Size(61, 24);
            this.rdoDefault.TabIndex = 8;
            this.rdoDefault.Text = "Default";
            this.rdoDefault.CheckedChanged += new System.EventHandler(this.rdoDefault_CheckedChanged);
            // 
            // chkApplyMinMaxes
            // 
            this.chkApplyMinMaxes.Checked = true;
            this.chkApplyMinMaxes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkApplyMinMaxes.Location = new System.Drawing.Point(18, 33);
            this.chkApplyMinMaxes.Name = "chkApplyMinMaxes";
            this.chkApplyMinMaxes.Size = new System.Drawing.Size(97, 24);
            this.chkApplyMinMaxes.TabIndex = 7;
            this.chkApplyMinMaxes.Text = "Apply Min/Max";
            this.chkApplyMinMaxes.CheckedChanged += new System.EventHandler(this.chkApplyMinMaxes_CheckedChanged);
            // 
            // rdoNone
            // 
            this.rdoNone.Location = new System.Drawing.Point(454, 34);
            this.rdoNone.Name = "rdoNone";
            this.rdoNone.Size = new System.Drawing.Size(52, 24);
            this.rdoNone.TabIndex = 6;
            this.rdoNone.Text = "None";
            this.rdoNone.CheckedChanged += new System.EventHandler(this.rdoNone_CheckedChanged);
            // 
            // rdoHierarchy
            // 
            this.rdoHierarchy.Location = new System.Drawing.Point(317, 34);
            this.rdoHierarchy.Name = "rdoHierarchy";
            this.rdoHierarchy.Size = new System.Drawing.Size(71, 24);
            this.rdoHierarchy.TabIndex = 5;
            this.rdoHierarchy.Text = "Hierarchy";
            this.rdoHierarchy.CheckedChanged += new System.EventHandler(this.rdoHierarchy_CheckedChanged);
            // 
            // rdoMethod
            // 
            this.rdoMethod.Location = new System.Drawing.Point(242, 34);
            this.rdoMethod.Name = "rdoMethod";
            this.rdoMethod.Size = new System.Drawing.Size(71, 24);
            this.rdoMethod.TabIndex = 4;
            this.rdoMethod.Text = "Method";
            this.rdoMethod.CheckedChanged += new System.EventHandler(this.rdoMethod_CheckedChanged);
            // 
            // lblLowLevelDefault
            // 
            this.lblLowLevelDefault.Location = new System.Drawing.Point(134, 38);
            this.lblLowLevelDefault.Name = "lblLowLevelDefault";
            this.lblLowLevelDefault.Size = new System.Drawing.Size(103, 14);
            this.lblLowLevelDefault.TabIndex = 3;
            this.lblLowLevelDefault.Text = "Low Levels Default:";
            // 
            // lblLowLevelMerchandise
            // 
            this.lblLowLevelMerchandise.Location = new System.Drawing.Point(21, 11);
            this.lblLowLevelMerchandise.Name = "lblLowLevelMerchandise";
            this.lblLowLevelMerchandise.Size = new System.Drawing.Size(72, 18);
            this.lblLowLevelMerchandise.TabIndex = 2;
            this.lblLowLevelMerchandise.Text = "Merchandise:";
            // 
            // gridStockMinMax
            // 
            this.gridStockMinMax.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridStockMinMax.ContextMenu = this.GridContextMenu;
            this.gridStockMinMax.Location = new System.Drawing.Point(16, 60);
            this.gridStockMinMax.Name = "gridStockMinMax";
            this.gridStockMinMax.Size = new System.Drawing.Size(608, 316);
            this.gridStockMinMax.TabIndex = 0;
            this.gridStockMinMax.Text = "Stock Min/Max";
            this.gridStockMinMax.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridStockMinMax_AfterCellUpdate);
            this.gridStockMinMax.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridStockMinMax_InitializeLayout);
            this.gridStockMinMax.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.gridStockMinMax_InitializeRow);
            this.gridStockMinMax.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridStockMinMax_CellChange);
            this.gridStockMinMax.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridStockMinMax_ClickCellButton);
            this.gridStockMinMax.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.gridStockMinMax_BeforeRowInsert);
            this.gridStockMinMax.DragEnter += new System.Windows.Forms.DragEventHandler(this.control_DragEnter);
            this.gridStockMinMax.DragOver += new System.Windows.Forms.DragEventHandler(this.control_DragOver);
            this.gridStockMinMax.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // chkUseDefault
            // 
            this.chkUseDefault.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseDefault.Location = new System.Drawing.Point(154, 16);
            this.chkUseDefault.Name = "chkUseDefault";
            this.chkUseDefault.Size = new System.Drawing.Size(88, 16);
            this.chkUseDefault.TabIndex = 8;
            this.chkUseDefault.Text = "Use Default";
            this.chkUseDefault.CheckedChanged += new System.EventHandler(this.chkUseDefault_CheckedChanged);
            this.chkUseDefault.Click += new System.EventHandler(this.chkUseDefault_Click);
            this.chkUseDefault.EnabledChanged += new EventHandler(chkUseDefault_EnabledChanged);
            // 
            // chkClear
            // 
            this.chkClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkClear.Location = new System.Drawing.Point(247, 16);
            this.chkClear.Name = "chkClear";
            this.chkClear.Size = new System.Drawing.Size(64, 16);
            this.chkClear.TabIndex = 6;
            this.chkClear.Text = "Clear";
            this.chkClear.CheckedChanged += new System.EventHandler(this.chkClear_CheckedChanged);
            // 
            // chkPlan
            // 
            this.chkPlan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPlan.Location = new System.Drawing.Point(85, 16);
            this.chkPlan.Name = "chkPlan";
            this.chkPlan.Size = new System.Drawing.Size(64, 16);
            this.chkPlan.TabIndex = 5;
            this.chkPlan.Text = "Plan";
            // 
            // chkDefault
            // 
            this.chkDefault.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDefault.Location = new System.Drawing.Point(16, 16);
            this.chkDefault.Name = "chkDefault";
            this.chkDefault.Size = new System.Drawing.Size(64, 16);
            this.chkDefault.TabIndex = 4;
            this.chkDefault.Text = "Default";
            this.chkDefault.CheckedChanged += new System.EventHandler(this.chkDefault_CheckedChanged);
            this.chkDefault.Click += new System.EventHandler(this.chkDefault_Click);
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.ugWorkflows);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(677, 470);
            this.tabProperties.TabIndex = 0;
            this.tabProperties.Text = "Properties";
            // 
            // ugWorkflows
            // 
            this.ugWorkflows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance25.BackColor = System.Drawing.Color.White;
            appearance25.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugWorkflows.DisplayLayout.Appearance = appearance25;
            this.ugWorkflows.DisplayLayout.InterBandSpacing = 10;
            appearance26.BackColor = System.Drawing.Color.Transparent;
            this.ugWorkflows.DisplayLayout.Override.CardAreaAppearance = appearance26;
            appearance27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance27.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance27.ForeColor = System.Drawing.Color.Black;
            appearance27.TextHAlignAsString = "Left";
            appearance27.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugWorkflows.DisplayLayout.Override.HeaderAppearance = appearance27;
            appearance28.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.Override.RowAppearance = appearance28;
            appearance29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance29.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorAppearance = appearance29;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugWorkflows.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance30.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance30.ForeColor = System.Drawing.Color.Black;
            this.ugWorkflows.DisplayLayout.Override.SelectedRowAppearance = appearance30;
            this.ugWorkflows.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugWorkflows.Location = new System.Drawing.Point(16, 19);
            this.ugWorkflows.Name = "ugWorkflows";
            this.ugWorkflows.Size = new System.Drawing.Size(632, 427);
            this.ugWorkflows.TabIndex = 3;
            this.ugWorkflows.Text = "Workflows";
            // 
            // Icons
            // 
            this.Icons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.Icons.ImageSize = new System.Drawing.Size(16, 16);
            this.Icons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // frmOTSPlanMethod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(723, 574);
            this.Controls.Add(this.tabOTSMethod);
            this.Name = "frmOTSPlanMethod";
            this.Load += new System.EventHandler(this.frmOTSPlanMethod_Load);
            this.Controls.SetChildIndex(this.tabOTSMethod, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.pnlGlobalUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.tabOTSMethod.ResumeLayout(false);
            this.tabMethod.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.gbxTrendOptions.ResumeLayout(false);
            this.gbxTrendOptions.PerformLayout();
            this.gbStoreForecast.ResumeLayout(false);
            this.gbStoreForecast.PerformLayout();
            this.gbChainForecast.ResumeLayout(false);
            this.gbOptions.ResumeLayout(false);
            this.pnlTYLYTrend.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabTYLYTrend.ResumeLayout(false);
            this.tabPageTY.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTYNodeVersion)).EndInit();
            this.tabPageLY.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridLYNodeVersion)).EndInit();
            this.tabPageApplyTrend.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTrendNodeVersion)).EndInit();
            this.tabPageCaps.ResumeLayout(false);
            this.grpTrendCaps.ResumeLayout(false);
            this.grpTrendCaps.PerformLayout();
            this.tabSetMethods.ResumeLayout(false);
            this.grpGroupLevelMethod.ResumeLayout(false);
            this.tabSetMethod.ResumeLayout(false);
            this.tabCriteria.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pnlPercentContribution.ResumeLayout(false);
            this.grpContributionBasis.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPctContBasis)).EndInit();
            this.tabStockMinMax.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridStockMinMax)).EndInit();
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
  
		/// <summary>
		/// Create a new OTSPlanMethod
		/// </summary>
		override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				//***************************************
				// disabled this until they are working
				//***************************************
				this.btnCopy.Enabled = false;
				this.btnCopy.Visible = false;
				this.chkClear.Enabled = false;
				this.chkClear.Visible = false;
				this.lblGLGroupBy.Enabled = true;
				this.lblGLGroupBy.Visible = true;
				this.cboGLGroupBy.Enabled = true;
                this.cboFuncType.Enabled = true;  // issue 5420
                this.rdoEqualizeYesTY.Enabled = true; // issue 5420
                this.rdoEqualizeNoTY.Enabled = true; // issue 5420
                this.rdoEqualizeYesLY.Enabled = true; // issue 5420
                this.rdoEqualizeNoLY.Enabled = true; // issue 5420
                this.rdoEqualizeYesTrend.Enabled = true; // issue 5420
                this.rdoEqualizeNoTrend.Enabled = true; // issue 5420
                this.cboGLGroupBy.Visible = true;

				midDateRangeSelectorOTSPlan.DateRangeRID = Include.UndefinedCalendarDateRange;

				_OTSPlanMethod = new OTSPlanMethod(SAB, Include.NoRID);
				base.NewWorkflowMethod(aParentNode, eSecurityFunctions.ForecastMethodsUserOTSPlan, eSecurityFunctions.ForecastMethodsGlobalOTSPlan);
	
				_GLFProfileList = _OTSPlanMethod.GLFProfileList;

				_GLFProfile = new GroupLevelFunctionProfile(Include.NoRID);
	
				Common_Load();

				this.chkUseDefault.Enabled = false;

				//Begin Track #4371 - JSmith - Multi-level forecasting.
				chkHighLevel.Enabled = false;
				chkLowLevels.Enabled = false;
				cboLowLevels.Enabled = false;
				btnExcludeLowLevels.Enabled = false;
				//End Track #4371

			}
			catch(Exception ex)
			{
				HandleException(ex, "NewOTSPlanMethod");
				FormLoadError = true;
			}
		}
		/// <summary>
		/// Opens an existing OTS Plan Method. //Eventually combine with NewOTSPlanMethod method
		/// 		/// Seperate for debugging & initial development
		/// </summary>
		/// <param name="aMethodRID">method_RID</param>
		/// <param name="aLockStatus">The lock status of the data to be displayed</param>
		override public void UpdateWorkflowMethod(int aMethodRID, int aNodeRID, MIDWorkflowMethodTreeNode aNode, eLockStatus aLockStatus)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				//***************************************
				// disabled this until they are working
				//***************************************
				this.btnCopy.Enabled = false;
				this.btnCopy.Visible = false;
				this.chkClear.Enabled = false;
				this.chkClear.Visible = false;
				this.lblGLGroupBy.Enabled = true;
				this.lblGLGroupBy.Visible = true;
				this.cboGLGroupBy.Enabled = true;
                this.cboFuncType.Enabled = true;  // issue 5420
                this.rdoEqualizeYesTY.Enabled = true; // issue 5420
                this.rdoEqualizeNoTY.Enabled = true; // issue 5420
                this.rdoEqualizeYesLY.Enabled = true; // issue 5420
                this.rdoEqualizeNoLY.Enabled = true; // issue 5420
                this.rdoEqualizeYesTrend.Enabled = true; // issue 5420
                this.rdoEqualizeNoTrend.Enabled = true; // issue 5420
                this.cboGLGroupBy.Visible = true;

				_OTSPlanMethod = new OTSPlanMethod(SAB, aMethodRID);
				base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.ForecastMethodsUserOTSPlan, eSecurityFunctions.ForecastMethodsGlobalOTSPlan);
				_nodeRID = _OTSPlanMethod.Plan_HN_RID;

				_GLFProfileList = _OTSPlanMethod.GLFProfileList;

				foreach (GroupLevelFunctionProfile glfp in _GLFProfileList.ArrayList)
				{
					if (glfp.Default_IND)
					{
						_defaultChecked = true;
						_defaultFunctionType = (int)glfp.GLFT_ID;
						this._defaultStoreGroupLevelRid = glfp.Key;
                        // Begin TT#2647 - JSmith - Delays in OTS Method
                        DefaultGLFProfile = glfp;
                        // End TT#2647 - JSmith - Delays in OTS Method
						break;
					}
				}
				
				Common_Load();

                // Begin TT#4560 - JSmith - Can change Trend Options when in Read Only
                SetReadOnly(FunctionSecurity.AllowUpdate);
                // End TT#4560 - JSmith - Can change Trend Options when in Read Only

				ToggleSaveCaption(false);
			}
			catch(Exception ex)
			{
				HandleException(ex, "InitializeOTSPlanMethod");
				FormLoadError = true;
			}
		}

		/// <summary>
		/// Deletes an OTS Plan Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		override public bool DeleteWorkflowMethod(int aMethodRID)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{       
				_OTSPlanMethod = new OTSPlanMethod(SAB, aMethodRID);
				return Delete();
			}
			catch(DatabaseForeignKeyViolation)
			{
				throw;
			}
			catch (Exception err)
			{
				HandleException(err);
				FormLoadError = true;
			}

			return true;
		}

		/// <summary>
		/// Renames an OTS Plan Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <param name="aNewName">The new name of the workflow or method</param>
		override public bool RenameWorkflowMethod(int aMethodRID, string aNewName)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{       
				_OTSPlanMethod = new OTSPlanMethod(SAB, aMethodRID);
				return Rename(aNewName);
			}
			catch (Exception err)
			{
				HandleException(err);
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
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{       
				_OTSPlanMethod = new OTSPlanMethod(SAB, aMethodRID);
				ProcessAction(eMethodType.OTSPlan, true);
			}
			catch (Exception ex)
			{
				HandleException(ex);
				FormLoadError = true;
			}
		}

		private void SetText()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
				this.btnClose.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
				this.btnProcess.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Process);
				this.chkPlan.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Forecast);
				// BEGIN Issue 4884 stodd 12.6.2007
				this.tabPageTY.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_TY);
				this.tabPageLY.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_LY);
				this.tabPageApplyTrend.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ApplyTrendTo);
				this.tabPageCaps.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_TrendCaps);
				this.tabCriteria.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Criteria);
				this.tabStockMinMax.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_StockMinMax);
				this.tabMethod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Method);
				this.tabProperties.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Properties);
				this.tabSetMethods.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SetMethods);
				// END Issue 4884
				//Begin Track #4371 - JSmith - Multi-level forecasting.
				this.btnExcludeLowLevels.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OverrideLowVersion);
				this.chkLowLevels.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ForecastLowLevels);
				this.chkHighLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ForecastHighLevel);
				//End Track #4371
				this.chkApplyMinMaxes.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ApplyMinMaxes);
				this.lblLowLevelDefault.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_InheritFrom);
				this.rdoMethod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_HighLevel);
				this.rdoHierarchy.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Hierarchy);
				this.rdoNone.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_NoneNoBrackets);
				_txtInheritedFrom = MIDText.GetTextOnly(eMIDTextCode.lbl_Inherited_From);
                // BEGIN TT#619- AGallagher - OTS Forecast - Chain Plan not required (#46)
                this.gbxTrendOptions.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_TrendOptions);
                this.radChainPlan.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_TrendOptionsChainPlans);
                this.radChainWOS.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_TrendOptionsChainWOS);
                this.radPlugChainWOS.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_TrendOptionsPlugChainWOS);
                // END TT#619- AGallagher - OTS Forecast - Chain Plan not required (#46) 
                this.cbxProjCurrWkSales.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Project_Curr_WK_Sls);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}
		private void Common_Load()//9-17
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				// Removed. Issue 4858 stodd 10.30.2007 
				//_versionProfList = SAB.ClientServerSession.GetUserForecastVersions();

				SetText();

				grdPctContBasis.DisplayLayout.ValueLists.Add("Merchandise");
				gridTYNodeVersion.DisplayLayout.ValueLists.Add("Merchandise");
				gridLYNodeVersion.DisplayLayout.ValueLists.Add("Merchandise");
				gridTrendNodeVersion.DisplayLayout.ValueLists.Add("Merchandise");
				LoadMethods();//9-18

				//setup images to be used in the grid.
				//picRelToPlan = new Bitmap(GraphicsDirectory + "\\RelToPlan.bmp");
				//picRelToCurrent = new Bitmap(GraphicsDirectory + "\\RelToCurrent.bmp");
				picInclude = new Bitmap(GraphicsDirectory + "\\include.gif");
				picExclude = new Bitmap(GraphicsDirectory + "\\exclude.gif");

				//the following two controls will be used to 
				this.btnIncExc.Visible = false;
	
				//Setups, bindings, etc.
				CreateComboLists();
                // Begin TT#1044 - JSmith - Apply Trend to TAb is taking an unusual amount of time to apply a basis- this iwill be a problem for ANF
                //_dtSource = this.GetDataSourceGroupLevelBasis();			// Issue 4818
                //_dtSource.DefaultView.RowFilter = "SGL_RID = " + this.cbxStoreGroupLevel.SelectedValue.ToString() +
                //        " AND (" + "TYLY_TYPE_ID = " + ((int)eTyLyType.NonTyLy).ToString() +
                //        " OR TYLY_TYPE_ID = 0)"; //MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
                //grdPctContBasis.DataSource = _dtSource;

                ////********************************************
                //// Set data data sources for TYLY trend grids
                ////********************************************
                
                //_dtSourceTYNode = this.GetDataSourceGroupLevelBasis();	// Issue 4818
                //_dtSourceTYNode.DefaultView.RowFilter = "SGL_RID = " +  this.cbxStoreGroupLevel.SelectedValue.ToString() +
                //    " AND " + "TYLY_TYPE_ID = " + ((int)eTyLyType.TyLy).ToString();
                //gridTYNodeVersion.DataSource = _dtSourceTYNode;

                //_dtSourceLYNode = this.GetDataSourceGroupLevelBasis();	// Issue 4818
                //_dtSourceLYNode.DefaultView.RowFilter = "SGL_RID = " +  this.cbxStoreGroupLevel.SelectedValue.ToString() +
                //    " AND " + "TYLY_TYPE_ID = " + ((int)eTyLyType.AlternateLy).ToString();
                //gridLYNodeVersion.DataSource = _dtSourceLYNode;
                
                //_dtSourceTrendNode = this.GetDataSourceGroupLevelBasis();	// Issue 4818
                //_dtSourceTrendNode.DefaultView.RowFilter = "SGL_RID = " +  this.cbxStoreGroupLevel.SelectedValue.ToString() +
                //    " AND " + "TYLY_TYPE_ID = " + ((int)eTyLyType.AlternateApplyTo).ToString();
                //gridTrendNodeVersion.DataSource = _dtSourceTrendNode;

                _dtSource = this.GetDataSourceGroupLevelBasis(eTyLyType.NonTyLy);
                _dtSource.DefaultView.RowFilter = "SGL_RID = " + this.cbxStoreGroupLevel.SelectedValue.ToString();
                grdPctContBasis.DataSource = _dtSource;

                _dtSourceTYNode = this.GetDataSourceGroupLevelBasis(eTyLyType.TyLy);
                _dtSourceTYNode.DefaultView.RowFilter = "SGL_RID = " + this.cbxStoreGroupLevel.SelectedValue.ToString();
                gridTYNodeVersion.DataSource = _dtSourceTYNode;

                _dtSourceLYNode = this.GetDataSourceGroupLevelBasis(eTyLyType.AlternateLy);
                _dtSourceLYNode.DefaultView.RowFilter = "SGL_RID = " + this.cbxStoreGroupLevel.SelectedValue.ToString();
                gridLYNodeVersion.DataSource = _dtSourceLYNode;

                // Begin TT#337-MD - JSmith - OTS Forecast - Apply to Tab on Available store set goes blank after running
                //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
                //if (!this.cbxProjCurrWkSales.Checked)
                //{
                //    _dtSourceTrendNode = this.GetDataSourceGroupLevelBasis(eTyLyType.AlternateApplyTo);
                //_dtSourceTrendNode.DefaultView.RowFilter = "SGL_RID = " + this.cbxStoreGroupLevel.SelectedValue.ToString();
                //    gridTrendNodeVersion.DataSource = _dtSourceTrendNode;
                //}
                //else
                //{
                //    _dtSourceTrendNode = this.GetDataSourceGroupLevelBasis(eTyLyType.ProjectCurrWkSales);
                //    // Begin TT#271-MD - JSmith - Null reference in OTS Plan Method if check "check project current week sales"
                //    //_dtSourceTrendNode.DefaultView.RowFilter = "SGL_RID = " + this.cbxStoreGroupLevel.SelectedValue.ToString();
                //    _dtSourceTrendNode.DefaultView.RowFilter = "SGL_RID = " + this.cbxStoreGroupLevel.SelectedValue.ToString();
                //    // End TT#271-MD - JSmith - Null reference in OTS Plan Method if check "check project current week sales"
                //    gridTrendNodeVersion.DataSource = _dtSourceTrendNode;
                //}
                //END TT#43 - MD - DOConnell - Projected Sales Enhancement
                // End TT#1044

                _dtSourceTrendNode = this.GetDataSourceGroupLevelBasis(eTyLyType.AlternateApplyTo);
                DataTable dt = this.GetDataSourceGroupLevelBasis(eTyLyType.ProjectCurrWkSales);
                _dtSourceTrendNode.Merge(dt);
                _dtSourceTrendNode.DefaultView.RowFilter = "SGL_RID = " + this.cbxStoreGroupLevel.SelectedValue.ToString();
                gridTrendNodeVersion.DataSource = _dtSourceTrendNode;
                // End TT#337-MD - JSmith - OTS Forecast - Apply to Tab on Available store set goes blank after running


				// put the panel where it really goes...
				this.Controls.Remove(this.pnlTYLYTrend);
				this.tabCriteria.Controls.Add(this.pnlTYLYTrend);
				this.pnlTYLYTrend.Location = pnlPercentContribution.Location;

				if (this.cboFuncType.Text == MIDText.GetTextOnly((int)eGroupLevelFunctionType.PercentContribution))
				{
					this.pnlPercentContribution.Visible = true;
					this.pnlTYLYTrend.Visible = false;
				}
				else if (this.cboFuncType.Text == MIDText.GetTextOnly((int)eGroupLevelFunctionType.TyLyTrend))
				{
					this.pnlPercentContribution.Visible = false;
					this.pnlTYLYTrend.Visible = true;
				}

				if (this._OTSPlanMethod.OverrideLowLevelRid != Include.NoRID)
				{
					ModelsData modelData = new ModelsData();
                    cboOverride.SelectedValue = _OTSPlanMethod.OverrideLowLevelRid;
				}

                LoadOverrideModelComboBox(cboOverride.ComboBox, _OTSPlanMethod.OverrideLowLevelRid, _OTSPlanMethod.CustomOLL_RID); //TT#7 - RBeck - Dynamic dropdowns

                LoadProperties();

                // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
               this.rdoDefault.Enabled = false;
                // End TT#3

				_merchValChanged = false;
				//Begin TT#875 - JScott - Add Base Code to Support A&F Custom Features

                // Begin TT#1054 - JSmith - Relieve Intransit not working.
                //string appSetShowWeightedBasis = System.Configuration.ConfigurationSettings.AppSettings["ShowWeightedBasis"];
                string appSetShowWeightedBasis = MIDConfigurationManager.AppSettings["ShowWeightedBasis"];
                // End TT#1054

				if (appSetShowWeightedBasis != null &&
					appSetShowWeightedBasis.ToLower(CultureInfo.CurrentUICulture) != "true" && appSetShowWeightedBasis.ToLower(CultureInfo.CurrentUICulture) != "yes" &&
					appSetShowWeightedBasis.ToLower(CultureInfo.CurrentUICulture) != "t" && appSetShowWeightedBasis.ToLower(CultureInfo.CurrentUICulture) != "y")
				{
					lblEqualizeWgtTY.Visible = false;
					rdoEqualizeYesTY.Visible = false;
					rdoEqualizeNoTY.Visible = false;
					lblEqualizeWgtLY.Visible = false;
					rdoEqualizeYesLY.Visible = false;
					rdoEqualizeNoLY.Visible = false;
					lblEqualizeWgtTrend.Visible = false;
					rdoEqualizeYesTrend.Visible = false;
					rdoEqualizeNoTrend.Visible = false;
				}
				//End TT#875 - JScott - Add Base Code to Support A&F Custom Features

                //BEGIN TT#110-MD-VStuart - In Use Tool
                tabOTSMethod.Controls.Remove(tabProperties);
                //END TT#110-MD-VStuart - In Use Tool
            }

			catch( Exception exception )
			{
				HandleException(exception);
			}
		}
 
		#region Form Load
		/// <summary>
		/// Load Properties Tab, OTS Plan Method specific & Set Method areas of Method tab
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmOTSPlanMethod_Load(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				//				FormLoaded = true;
                // Begin Track #4872 - JSmith - Global/User Attributes
                if (cboStoreGroups.ReplaceAttribute)
                {
                    ChangePending = true;
                }
                // End Track #4872

                // BEGIN MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
                FormLoaded = true;
                // Begin TT#2647 - JSmith - Delays in OTS Method
                //UpdateSetsUsingDefault();
                // End TT#2647 - JSmith - Delays in OTS Method
                // END MID Track #5954
			}
			catch(Exception ex)
			{
				HandleException(ex, "OTSPlanMethod_Load");
			}
			
		}
		#endregion

		#region Tabs (NON BASIS GRID Items)

		#region Calls to Loading of Defaults and current values
		/// <summary>
		/// Load Methods Tab of OTS Plan Method
		/// </summary>
		private void LoadMethods()//9-18
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				//OK If New
				//Populate Plan and Chain Version combos
				PopulateVersionCombos();//9-17

				//OK If New
				//Populate Method and Smooth By combos
				PopulateCommonCriteria();//9-17

				_InitialPopulate = true;

				//OK If New
				PopultateStoreAttribCombo();//9-17

				//OK If New
				PopulateSetMethod();//9-18
				
				//OK If New
				LoadOTSPlanValues();//9-17

				PopulateModelCombo();

				//Begin TT#891 - JScott - OTS Forecast % Contribution not getting expected stock results.
				EnableTrendControls();

				//End TT#891 - JScott - OTS Forecast % Contribution not getting expected stock results.
				_InitialPopulate = false;
			}
			catch(Exception ex)
			{
				HandleException(ex, "LoadMethods");
			}
		}

		/// <summary>
		/// Load Properties Tab of OTS Plan Method
		/// </summary>
		private void LoadProperties()//9-17
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				LoadCommon();
				LoadWorkflows();
			}
			catch(Exception ex)
			{
				HandleException(ex, "LoadProperties");
			}
		}
		#endregion
		
		#region Populate OTS Plan Default Values

		//9-17 Jeff's working on locking logic
		//		/// <summary>
		//		/// Compare 
		//		/// </summary>
		//		private void CompareOTSPlanProfileToDB()
		//		{
		//			//Need to check if any method values are different in DT than DB
		//			try
		//			{
		//				if (SAB.ClientServerSession.WMEBusiness.IsProfileEqualToDB(_OTSPlanMethod))
		//					//OK
		//					return;
		//				else
		//					//Do what?
		//					MessageBox.Show("Client session OTS Plan and DB contain different data");
		//			}
		//			catch(Exception ex)
		//			{
		//				HandleException(ex, "CompareOTSPlanProfileToDB");
		//			}
		//		}

		/// <summary>
		/// Load GroupLevelFunction class with default values if _newOTSPlan = false.
		/// If doesn't populate then _newOTSPlan = true
		/// </summary>
		private void PopulateSetMethod()//9-18
		{		
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				//Is there a set method within the method profile?
				_newSetMethod = true;//9-18	
				if (_GLFProfileList.Count > 0)
				{	
					_newSetMethod = false;
					_GLFProfile = (GroupLevelFunctionProfile)_GLFProfileList[0];
					this.cbxStoreGroupLevel.SelectedValue = _GLFProfile.Key;
					_prevSetValue = _GLFProfile.Key;
				}

				LoadSetMethod();
					
			}
			catch(Exception ex)
			{
				HandleException(ex, "PopulateSetMethod");
			}
		}	

		/// <summary>
		/// Populates Method, Group By, Spread By, and Relative To (hidden except on Current Trend)
		/// </summary>
		private void PopulateCommonCriteria()//9-17
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				// removed functions not available yet...
				// to put back full list, uncomment out next statement and delete this one
				PopulateCommonCriteria((int)eGroupLevelFunctionType.PercentContribution,
					(int)eGroupLevelFunctionType.TyLyTrend, this.cboFuncType.ComboBox,
					Convert.ToInt32(eGroupLevelFunctionType.PercentContribution, CultureInfo.CurrentUICulture).ToString(CultureInfo.CurrentUICulture));
                _prevFuncTypeValue = Convert.ToInt32(eGroupLevelFunctionType.PercentContribution, CultureInfo.CurrentUICulture);//TT#7 - RBeck - Dynamic dropdowns

				PopulateCommonCriteria((int)eGroupLevelSmoothBy.StoreSet,
					(int)eGroupLevelSmoothBy.None, this.cboGLGroupBy.ComboBox,
                    Convert.ToInt32(eGroupLevelSmoothBy.None, CultureInfo.CurrentUICulture).ToString(CultureInfo.CurrentUICulture));    //TT#7 - RBeck - Dynamic dropdowns
			}
			catch(Exception ex)
			{
				HandleException(ex, "PopulateCommonCriteria");
			}
		}

		/// <summary>
		/// Generic method to populate all MIDComboBoxes that use MIDText.GetLabels function
		/// </summary>
		/// <param name="startVal">enum start value</param>
		/// 		/// <param name="endVal">enum end value</param>
		/// <param name="CboBox">MIDComboBox control name</param>
		/// <param name="selectVal">selected value for combo box</param>
		private void PopulateCommonCriteria(int startVal, int endVal, ComboBox CboBox, string selectVal)//9-17

		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				DataTable dt = MIDEnvironment.CreateDataTable();
				dt = MIDText.GetLabels(startVal, endVal);
				CboBox.DisplayMember = "TEXT_VALUE";
				CboBox.ValueMember = "TEXT_CODE";
				CboBox.DataSource = dt;

				CboBox.SelectedValue = selectVal;

				//HideCommonTextCols(midCboBox);
			}
			catch(Exception ex)
			{
				HandleException(ex, "PopulateCommonCriteria");
			}
		}

		/// <summary>
		///Populate all values of Version & Chain Version midComboBoxes of OTS Plan Group Box
		/// </summary>
		private void PopulateVersionCombos()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				// Setup Versions DataTable for Listboxes
//				_dtForecastVers = MIDEnvironment.CreateDataTable("Versions");
//				_dtForecastVers.Columns.Add("Description", typeof(string));
//				_dtForecastVers.Columns.Add("Key", typeof(int));
//
//				_dtForecastVers.Rows.Add(new object[] {"", Include.NoRID});
//
//				foreach (VersionProfile verProf in _versionProfList)
//				{
//					// Begin Issue 4562 - stodd - 8.6.07
//					if (verProf.Key == Include.FV_ActualRID ||
//						verProf.ChainSecurity.AccessDenied ||
//						// If Blended AND the forecast version isn't equal to itself.
//						(verProf.IsBlendedVersion && verProf.ForecastVersionRID != verProf.Key))
//					{
//						// Do not include this version
//					}
//					else
//					{
//						_dtForecastVers.Rows.Add(new object[] {verProf.Description, verProf.Key});
//					}
//					// End Issue 4562
//				}

				// BEGIN Issue 4858 stodd 10.30.2007 forecast methods security
				ProfileList chainVersionProfList = base.GetForecastVersionList(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Chain, true, _OTSPlanMethod.Chain_FV_RID);	// Track 5871
                cboChainVers.DisplayMember = "Description";
				cboChainVers.ValueMember = "Key";
				cboChainVers.DataSource = chainVersionProfList.ArrayList;
				// END Issue 4858 stodd 10.30.2007 forecast methods security


////				DataTable dt = _dtForecastVers.Copy();
//				// Setup Versions DataTable for Listboxes
//				DataTable dt = MIDEnvironment.CreateDataTable("Versions");
//				dt.Columns.Add("Description", typeof(string));
//				dt.Columns.Add("Key", typeof(int));
//
//				dt.Rows.Add(new object[] {"", Include.NoRID});
//
//				foreach (VersionProfile verProf in _versionProfList)
//				{
//					// Begin Issue 4562 - stodd - 8.6.07
//					if (verProf.Key == Include.FV_ActualRID ||
//						verProf.StoreSecurity.AccessDenied ||
//						// If Blended AND the forecast version isn't equal to itself.
//						(verProf.IsBlendedVersion && verProf.ForecastVersionRID != verProf.Key))
//					{
//						// Do not include this version
//					}
//					else
//					{
//						dt.Rows.Add(new object[] {verProf.Description, verProf.Key});
//					}
//					// End Issue 4562
//				}

				// BEGIN Issue 4858 stodd 10.30.2007 forecast methods security
                //Begin Track #5858 - JSmith - Validating store security only
                //ProfileList storeVersionProfList = base.GetForecastVersionList(ePlanBasisType.Plan, eSecurityTypes.Store, true, _OTSPlanMethod.Plan_FV_RID);
				ProfileList storeVersionProfList = base.GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Store, true, _OTSPlanMethod.Plan_FV_RID, true);	// Track #5871
                //End Track #5858
				cboPlanVers.DisplayMember = "Description";
				cboPlanVers.ValueMember = "Key";
				cboPlanVers.DataSource = storeVersionProfList.ArrayList;
				// END Issue 4858 stodd 10.30.2007 forecast methods security
			}
			catch(Exception ex)
			{
				HandleException(ex, "PopulateVersionCombos");
			}
		}
 
		/// <summary>
		/// Populate Store Attribute midComboBox of the Properties tab just under the
		/// OTS Plan Group Box
		/// </summary>
		private void PopultateStoreAttribCombo()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
			{
                // Begin Track #4872 - JSmith - Global/User Attributes
                //ProfileList pl = SAB.StoreServerSession.GetStoreGroupListViewList();
                //ProfileList pl = GetStoreGroupList(_OTSPlanMethod.Method_Change_Type, _OTSPlanMethod.GlobalUserType, false);
                //cboStoreGroups.Initialize(SAB, FunctionSecurity, pl.ArrayList, _OTSPlanMethod.GlobalUserType == eGlobalUserType.User);
                BuildAttributeList();
				
                //this.cboStoreGroups.ValueMember = "Key";
                //this.cboStoreGroups.DisplayMember = "Name";
                //this.cboStoreGroups.DataSource = pl.ArrayList;
                // End Track #4872

                //Begin Track #5858 - KJohnson - Validating store security only
                // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                //cboStoreGroups.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreGroups);
                cboStoreGroups.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreGroups, FunctionSecurity, _OTSPlanMethod.GlobalUserType == eGlobalUserType.User);
                // End TT#44
                //End Track #5858
				// Set Attribute to OTS planning default
				GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
				gop.LoadOptions();
				this.cboStoreGroups.SelectedValue = gop.OTSPlanStoreGroupRID;
                // Begin TT#284-MD - JSmith - OTS Forecast Method errors on open
                // BEGIN TT#7 - RBeck - Dynamic dropdowns
                ////EventArgs z = new EventArgs();
                //this.cboStoreGroups_DropDownClosed(this, z);
                // END TT#7 - RBeck - Dynamic dropdowns
                // End TT#284-MD - JSmith - OTS Forecast Method errors on open
			}
			catch(Exception ex)
			{
				HandleException(ex, "PopultateStoreAttribCombo");
			}
		}

		// MID TRACK #4371 - JBolles - Multi-level forecasting
		private void PopulateLowerLevelsCombo()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				cboMerchandise.Items.Clear();

				cboMerchandise.Items.Add(new ComboObject(_OTSPlanMethod.Plan_HN_RID, txtOTSHNDesc.Text));

				_OTSPlanMethod.PopulateOverrideList();

				foreach(LowLevelVersionOverrideProfile exclude in _OTSPlanMethod.LowlevelOverrideList)
				{
					if(!exclude.Exclude)
						cboMerchandise.Items.Add(new ComboObject(exclude.Key, exclude.NodeProfile.Text));
				}
			
				if (cboMerchandise.Items.Count > 0)
					cboMerchandise.SelectedIndex = 0;

			}
			catch (Exception ex)
			{
				HandleException(ex, "PopulateLowerLevelsCombo");
			}
		}
		// END MID TRACK #4371

		private void PopulateModelCombo()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				ForecastModelProfileList pl = new ForecastModelProfileList(true);
				ForecastModelProfile fmp = new ForecastModelProfile(Include.NoRID);
				fmp.ModelID = "";
				pl.Add(fmp);
				
				this.cboModel.ValueMember = "Key";
				this.cboModel.DisplayMember = "ModelID";
				this.cboModel.DataSource = pl.ArrayList;

				if (_OTSPlanMethod.Method_Change_Type == eChangeType.add
					&& _OTSPlanMethod.ForecastModelRid == Include.NoRID)
				{
					if (pl.Count == 2)  // one model + null entry
						cboModel.SelectedValue = pl[0].Key;
					else
						cboModel.SelectedValue = _OTSPlanMethod.ForecastModelRid;
				}
				else
					cboModel.SelectedValue = _OTSPlanMethod.ForecastModelRid;
			}
			catch(Exception ex)
			{
				HandleException(ex, "PopultateModelCombo");
			}
		}

		/// <summary>
		/// Check if Set Method Items or Basis Items have changed from what
		/// was loaded, if changed warn user (ok, cancel), if not proceed.
		/// todo - soft label
		/// </summary>
		/// <param name="key"></param>
		private void PopulateStoreGLComboCheck(string key)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			PopulateStoreGLCombo(key);
		}
		/// <summary>
		/// Populate all values of the Store Group Levels (based on key from Store Attribute)
		/// of the Store Group Level midComboBox in the Set Method Group Box
		/// </summary>
		/// <param name="key"></param>
		private void PopulateStoreGLCombo(string key)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
                ProfileList pl = StoreMgmt.StoreGroup_GetLevelListViewList(Convert.ToInt32(key, CultureInfo.CurrentUICulture)); // SAB.StoreServerSession.GetStoreGroupLevelListViewList(Convert.ToInt32(key, CultureInfo.CurrentUICulture));
				
				this.cbxStoreGroupLevel.ValueMember = "Key";
				this.cbxStoreGroupLevel.DisplayMember = "Name";
				this.cbxStoreGroupLevel.DataSource = pl.ArrayList;

				StoreGroupLevelListViewProfile sglp = (StoreGroupLevelListViewProfile)pl[0];
				this.cbxStoreGroupLevel.SelectedValue = sglp.Key;
			}
			catch(Exception ex)
			{
				HandleException(ex, "PopulateStoreGLCombo");
			}
		}
		private void midDateRangeSelectorOTSPlan_Load(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				_frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{SAB});
				midDateRangeSelectorOTSPlan.DateRangeForm = _frm;
			}
			catch(Exception ex)
			{
				HandleException(ex, "midDateRangeSelectorOTSPlan_Load");
			}
		}

		/// <summary>
		/// After selection is made on midDateRangeSelector - OTS Plan Method
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void midDateRangeSelectorOTSPlan_OnSelection(object sender, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				DateRangeProfile dr = e.SelectedDateRange;

				SetBasisDates(_dtSource);
				SetBasisDates(_dtSourceTYNode);
				SetBasisDates(_dtSourceLYNode);
				SetBasisDates(_dtSourceTrendNode);

				grdPctContBasis.Refresh();
				gridTYNodeVersion.Refresh();
				gridLYNodeVersion.Refresh();
				gridTrendNodeVersion.Refresh();

				if (dr != null)
				{
					LoadDateRangeText(dr, midDateRangeSelectorOTSPlan);
					// BEGIN Issue 5119
					if (FormLoaded)
					{
						ChangePending = true;
					}
					// END Issue 5119
				}

				
			}
			catch(Exception ex)
			{
				HandleException(ex, "midDateRangeSelectorOTSPlan_OnSelection");
			}
		}

		private void SetBasisDates(DataTable aBasisDataTable)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			int i;
			int drID;
			DateRangeProfile drProf;
			string drText;
			try
			{
				for (i = 0; i < aBasisDataTable.Rows.Count; i++)
				{
					//Fill in the DateRange selector's display text.
					// Begin ISSUE # 3018 - stodd
					DataRow aRow = aBasisDataTable.Rows[i];
					if (aRow.RowState != DataRowState.Deleted)
					{		
						// End ISSUE # 3018 - stodd
						if (aBasisDataTable.Rows[i]["CDR_RID"] != System.DBNull.Value)
						{
							drID = Convert.ToInt32(aBasisDataTable.Rows[i]["CDR_RID"], CultureInfo.CurrentUICulture);

							if (drID != Include.UndefinedCalendarDateRange)
							{
								if (midDateRangeSelectorOTSPlan.DateRangeRID != Include.UndefinedCalendarDateRange)
								{
									drProf = SAB.ClientServerSession.Calendar.GetDateRange(drID, midDateRangeSelectorOTSPlan.DateRangeRID);
								}
								else
								{
									drProf = SAB.ClientServerSession.Calendar.GetDateRange(drID, SAB.ClientServerSession.Calendar.CurrentDate);
								}

								drText = SAB.ClientServerSession.Calendar.GetDisplayDate(drProf);

								aBasisDataTable.Rows[i]["DateRange"] = drText;
							}
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		#endregion

		#region Load OTS Plan Values
		private void LoadCommon()//9-17
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				this.txtName.Text = _OTSPlanMethod.Name;
				this.txtDesc.Text = _OTSPlanMethod.Method_Description;	

				// This fixes the text showing bolder/differently from the other text
				if (!FunctionSecurity.AllowUpdate)
				{
					this.txtName.Enabled = false;
					this.txtDesc.Enabled = false;
				}
                // Begin Track #5476 - JSmith - Opening as wrong user/global type
                //if (_OTSPlanMethod.User_RID == Include.GetGlobalUserRID())
                //    radGlobal.Checked = true;
                //else
                //    radUser.Checked = true;
                // End Track #5476
			}
			catch(Exception ex)
			{
				HandleException(ex, "LoadCommon");
			}
		}

		/// <summary>
		/// If _newOTSPlan then load default OTS Plan values.
		/// Else, OTSPlan default values are already populated.
		/// </summary>
		private void LoadOTSPlanValues()//9-17
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
                //Begin Track #5858 - KJohnson - Validating store security only
                txtOTSHNDesc.Tag = new MIDMerchandiseTextBoxTag(SAB, txtOTSHNDesc, eMIDControlCode.form_OTSPlanMethod, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
                //End Track #5858
				if (_OTSPlanMethod.Method_Change_Type == eChangeType.update)
				{
					//Load Text
					//Begin Track #5378 - color and size not qualified
//					HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSPlanMethod.Plan_HN_RID);
                     // Begin TT#2647 - JSmith - Delays in OTS Method
                    //HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSPlanMethod.Plan_HN_RID, true, true);
                    HierarchyNodeProfile hnp =null;
                    
                    if (!_nodesByRID.TryGetValue(_OTSPlanMethod.Plan_HN_RID, out hnp))
                    {
                        hnp = SAB.HierarchyServerSession.GetNodeData(_OTSPlanMethod.Plan_HN_RID, true, true);
                        _nodesByRID.Add(_OTSPlanMethod.Plan_HN_RID, hnp);
                        if (!_nodesByID.ContainsKey(hnp.NodeID))
                        {
                            _nodesByID.Add(hnp.NodeID, hnp);
                        }
                    }
                    // End TT#2647 - JSmith - Delays in OTS Method
					//End Track #5378
					LoadOTSPlanText(hnp);

					//Load Combos
					this.cboPlanVers.SelectedValue = _OTSPlanMethod.Plan_FV_RID;
					this.cboChainVers.SelectedValue = _OTSPlanMethod.Chain_FV_RID;
					this.cboStoreGroups.SelectedValue = _OTSPlanMethod.SG_RID;
                   
                    // Begin TT#1530 - RMatelic - Record locking violations from Store Attribute change
                    // Begin Track #4872 - JSmith - Global/User Attributes
                    //if (cboStoreGroups.ContinueReadOnly)
                    //{
                    //    SetMethodReadOnly();
                    //}
                    // End Track #4872
                    if (FunctionSecurity.AllowUpdate)
                    {
                        if (cboStoreGroups.ContinueReadOnly)
                        {
                            SetMethodReadOnly();
                        }
                    }
                    else
                    {
                        cboStoreGroups.Enabled = false;
                    }
                    // End TT#1530

                    this.cboModel.SelectedValue = _OTSPlanMethod.ForecastModelRid;

					//Load CheckBoxes
					chkOTSSales.Checked = _OTSPlanMethod.Bal_Sales_Ind;
					chkOTSStock.Checked = _OTSPlanMethod.Bal_Stock_Ind;		// Issue 4809 stodd 12.10.2007

                    // BEGIN TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46)
                    //Load RadioButtons
                    switch (_OTSPlanMethod.ApplyTrendOptionsInd)
                    {
                        case 'C':
                            this.radChainPlan.Checked = true;
                            _ApplyTrendOptionsInd = 'C';
                            _ApplyTrendOptionsWOSValue = 0;
                            break;
                        case 'W':
                            this.radChainWOS.Checked = true;
                            _ApplyTrendOptionsInd = 'W';
                            _ApplyTrendOptionsWOSValue = 0;
                            break;
                        case 'S':
                            this.radPlugChainWOS.Checked = true;
                            _ApplyTrendOptionsInd = 'S';
                            break;
                    }
                    // END TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46)
                    _ApplyTrendOptionsWOSValue = _OTSPlanMethod.ApplyTrendOptionsWOSValue; 
                    if (_ApplyTrendOptionsWOSValue > 0)
                        txtPlugChainWOS.Text = _ApplyTrendOptionsWOSValue.ToString();
                    // BEGIN TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46)

                    // END TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46)

					//Begin Track #4371 - JSmith - Multi-level forecasting.
					chkHighLevel.Checked = _OTSPlanMethod.HighLevelInd;
					chkLowLevels.Checked = _OTSPlanMethod.LowLevelsInd;
					PopulateLowerLevelsCombo();

					if (!chkLowLevels.Checked)
					{
						cboLowLevels.Enabled = false;
						btnExcludeLowLevels.Enabled = false;
						lblLowLevelMerchandise.Visible = false;
						cboMerchandise.Visible = false;
						lblLowLevelDefault.Visible = true;  // 4573 (now shows "inherited from")
						rdoMethod.Visible = false;  // 4573
//						rdoHierarchy.Visible = false;
//						rdoNone.Visible = false;
					}
					else
					{
                        this.cboLowLevels.Text = FindItemByValue(cboLowLevels.ComboBox);//TT#7 - RBeck - Dynamic dropdowns
						PopulateBasisMerchandiseValueList();
						lblLowLevelMerchandise.Visible = true;
						cboMerchandise.Visible = true;
						lblLowLevelDefault.Visible = true;  // 4573
						rdoMethod.Visible = true;  // 4573
//						rdoHierarchy.Visible = true;
//						rdoNone.Visible = true;
					}
					//End Track #4371

					//Load Calendar Data
					DateRangeProfile dr = SAB.ClientServerSession.Calendar.GetDateRange(_OTSPlanMethod.CDR_RID);
					LoadDateRangeText(dr, midDateRangeSelectorOTSPlan);

				}	
				else //Else Defaults already loaded
				{
					lblLowLevelMerchandise.Visible = false;
					cboMerchandise.Visible = false;
//					lblLowLevelDefault.Visible = false;
					rdoMethod.Visible = false;
//					rdoHierarchy.Visible = false;
//					rdoNone.Visible = false;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex, "LoadOTSPlanValues");
			}
		}

		//Begin Track #4371 - JBolles - Multi-level forecasting.
		private string FindItemByValue(ComboBox combo)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			eLowLevelsType lowLevelType = _OTSPlanMethod.LowLevelsType;

			foreach(LowLevelCombo thisCombo in combo.Items)
			{
				if(thisCombo.LowLevelType == lowLevelType)
				{
					switch(lowLevelType)
					{
						case eLowLevelsType.LevelOffset:
							if(thisCombo.LowLevelOffset == _OTSPlanMethod.LowLevelsOffset)
								return thisCombo.ToString();
							
							break;
						default:
							if(thisCombo.LowLevelSequence == _OTSPlanMethod.LowLevelsSequence)
								return thisCombo.ToString();

							break;
					}
				}
			}

			return String.Empty;
		}
		//End Track #4371

		/// <summary>
		/// Load Set Method & Generic area of Criteria Tab
		/// </summary>
		private void LoadSetMethod()//9-18
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				if (!_newSetMethod)
				{
					_loadingSet = true;
					this.cboFuncType.SelectedValue = Convert.ToInt32(_GLFProfile.GLFT_ID, CultureInfo.CurrentUICulture).ToString(CultureInfo.CurrentUICulture);
					_prevFuncTypeValue = Convert.ToInt32(this.cboFuncType.SelectedValue); 
					_currentTYLYTabPage = tabPageTY;					 

					this.cboGLGroupBy.SelectedValue = Convert.ToInt32(_GLFProfile.GLSB_ID, CultureInfo.CurrentUICulture).ToString(CultureInfo.CurrentUICulture);
					_sglOrigVal = _GLFProfile.Key.ToString(CultureInfo.CurrentUICulture);

					//Load Check Boxes
                    chkDefault.Checked = _GLFProfile.Default_IND;
                    
                    if (FunctionSecurity.AllowUpdate)
                    {
                        if (_defaultChecked)
                        {
                            chkUseDefault.Enabled = true;
                            chkDefault.Enabled = false;
                        }
                        else
                        {
                            chkUseDefault.Enabled = false;
                            chkDefault.Enabled = true;
                        }
                        //...unless of course THIS is the default set
                        if (chkDefault.Checked)
                        {
                            chkDefault.Enabled = true;
                            chkUseDefault.Enabled = false;
                        }
                    }

					chkPlan.Checked = _GLFProfile.Plan_IND;
                    chkUseDefault.Checked = _GLFProfile.Use_Default_IND;
                    chkClear.Checked = _GLFProfile.Clear_IND;

                    if (_GLFProfile.Use_Default_IND || _GLFProfile.Clear_IND)
                        CheckAndDisablePlan();
                    
					radNone.Checked = true;
					ProfileList pl = _GLFProfile.Trend_Caps;
					foreach (TrendCapsProfile tcp in pl)
					{
						if (tcp.TrendCapID == eTrendCapID.None)
							radNone.Checked = true;
						else if (tcp.TrendCapID == eTrendCapID.Tolerance)
						{
							radTolerance.Checked = true;
							txtTolerance.Text = Convert.ToString(tcp.TolPct, CultureInfo.CurrentUICulture);
						}
						else if (tcp.TrendCapID == eTrendCapID.Limits)
						{
							radLimits.Checked = true;
							if (tcp.HighLimit != Include.UndefinedDouble)
								txtHigh.Text = Convert.ToString(tcp.HighLimit, CultureInfo.CurrentUICulture);
							if (tcp.LowLimit  != Include.UndefinedDouble)
								txtLow.Text = Convert.ToString(tcp.LowLimit, CultureInfo.CurrentUICulture);
						}
					}

					this.cbxAltLY.Checked = _GLFProfile.LY_Alt_IND;
                    // Begin TT#2587 - JSmith - Equalize weighting flips back to yes after saving
                    this.cbxAltTrend.Checked = _GLFProfile.Trend_Alt_IND;
                    // End TT#2587 - JSmith - Equalize weighting flips back to yes after saving

                    if (_GLFProfile.TY_Weight_Multiple_Basis_Ind)
                    {
                        // BEGIN Issue 5420 KJohnson - rdoEqualizeYesTY not holding it value
                        if (chkUseDefault.Checked)
                        {
                            if (_defaultEqualizeTY)
                            {
                                this.rdoEqualizeYesTY.Checked = true;
                                this.rdoEqualizeNoTY.Checked = false;
                            }
                            else
                            {
                                this.rdoEqualizeYesTY.Checked = false;
                                this.rdoEqualizeNoTY.Checked = true;
                            }
                        }
                        else
                        {
                            this.rdoEqualizeYesTY.Checked = true;
                            this.rdoEqualizeNoTY.Checked = false;
                        }
                        // End Issue 5420 KJohnson
                    }
                    else
                    {
                        // BEGIN Issue 5420 KJohnson - rdoEqualizeYesTY not holding it value
                        if (chkUseDefault.Checked) 
                        {
                            if (_defaultEqualizeTY)
                            {
                                this.rdoEqualizeYesTY.Checked = true;
                                this.rdoEqualizeNoTY.Checked = false;
                            }
                            else 
                            {
                                this.rdoEqualizeYesTY.Checked = false;
                                this.rdoEqualizeNoTY.Checked = true;
                            }
                        }
                        else
                        {
                            this.rdoEqualizeYesTY.Checked = false;
                            this.rdoEqualizeNoTY.Checked = true;
                        }
                        // End Issue 5420 KJohnson
                    }

					if(_GLFProfile.LY_Weight_Multiple_Basis_Ind)
					{
                        // BEGIN Issue 5420 KJohnson - rdoEqualizeYesTY not holding it value
                        if (chkUseDefault.Checked)
                        {
                            if (_defaultEqualizeLY)
                            {
                                this.rdoEqualizeYesLY.Checked = true;
                                this.rdoEqualizeNoLY.Checked = false;
                            }
                            else
                            {
                                this.rdoEqualizeYesLY.Checked = false;
                                this.rdoEqualizeNoLY.Checked = true;
                            }
                        }
                        else
                        {
                            this.rdoEqualizeYesLY.Checked = true;
                            this.rdoEqualizeNoLY.Checked = false;
                        }
                        // End Issue 5420 KJohnson
					}
					else
					{
                        // BEGIN Issue 5420 KJohnson - rdoEqualizeYesTY not holding it value
                        if (chkUseDefault.Checked)
                        {
                            if (_defaultEqualizeLY)
                            {
                                this.rdoEqualizeYesLY.Checked = true;
                                this.rdoEqualizeNoLY.Checked = false;
                            }
                            else
                            {
                                this.rdoEqualizeYesLY.Checked = false;
                                this.rdoEqualizeNoLY.Checked = true;
                            }
                        }
                        else
                        {
                            this.rdoEqualizeYesLY.Checked = false;
                            this.rdoEqualizeNoLY.Checked = true;
                        }
                        // End Issue 5420 KJohnson
					}

					if(_GLFProfile.Apply_Weight_Multiple_Basis_Ind)
					{
                        // BEGIN Issue 5420 KJohnson - rdoEqualizeYesTY not holding it value
                        if (chkUseDefault.Checked)
                        {
                            if (_defaultEqualizeTrend)
                            {
                                this.rdoEqualizeYesTrend.Checked = true;
                                this.rdoEqualizeNoTrend.Checked = false;
                            }
                            else
                            {
                                this.rdoEqualizeYesTrend.Checked = false;
                                this.rdoEqualizeNoTrend.Checked = true;
                            }
                        }
                        else
                        {
                            this.rdoEqualizeYesTrend.Checked = true;
                            this.rdoEqualizeNoTrend.Checked = false;
                        }
                        // End Issue 5420 KJohnson
                    }
					else
					{
                        // BEGIN Issue 5420 KJohnson - rdoEqualizeYesTY not holding it value
                        if (chkUseDefault.Checked)
                        {
                            if (_defaultEqualizeTrend)
                            {
                                this.rdoEqualizeYesTrend.Checked = true;
                                this.rdoEqualizeNoTrend.Checked = false;
                            }
                            else
                            {
                                this.rdoEqualizeYesTrend.Checked = false;
                                this.rdoEqualizeNoTrend.Checked = true;
                            }
                        }
                        else
                        {
                            this.rdoEqualizeYesTrend.Checked = false;
                            this.rdoEqualizeNoTrend.Checked = true;
                        }
                        // End Issue 5420 KJohnson
					}

					if (!this.cbxAltLY.Checked)
					{
						this.gridLYNodeVersion.Enabled = false;
						this.rdoEqualizeNoLY.Enabled = false;
						this.rdoEqualizeYesLY.Enabled = false;
					}

                    // Begin TT#2587 - JSmith - Equalize weighting flips back to yes after saving
                    //this.cbxAltTrend.Checked = _GLFProfile.Trend_Alt_IND;
                    // End TT#2587 - JSmith - Equalize weighting flips back to yes after saving
					if (!this.cbxAltTrend.Checked)
					{
						this.gridTrendNodeVersion.Enabled = false;
						this.rdoEqualizeNoTrend.Enabled = false;
						this.rdoEqualizeYesTrend.Enabled = false;
					}

                    //BEGIN TT#43-MD-DOConnell- Project Sales Enhancement
                    this.cbxProjCurrWkSales.Checked = _GLFProfile.Proj_Curr_Wk_Sales_IND;
                    //END TT#43-MD-DOConnell- Project Sales Enhancement
					_loadingSet = false;
				}
				else
				{
					chkDefault.Checked = false;
                    // Begin TT#1530 - RMatelic - Record locking violations from Store Attribute change
					//chkPlan.Enabled = true;
                    chkPlan.Enabled = FunctionSecurity.AllowUpdate;
                    // End TT#1530
					chkPlan.Checked = false;
					chkUseDefault.Checked = false;
					chkUseDefault.Enabled = false;
					chkClear.Checked = false;

                    cbxProjCurrWkSales.Checked = false; //TT#331 - MD - DOConnell - OTS Forecast get an error when trying to go to the TY tab and cannot populate the other sets basis data.

					if (_defaultChecked)
					{
                        // Begin TT#1530 - RMatelic - Record locking violations from Store Attribute change
                        //chkUseDefault.Enabled = true;
                        chkUseDefault.Enabled = FunctionSecurity.AllowUpdate;
                        // End TT#1530
						chkDefault.Enabled = false;
					}
					else
					{
						chkUseDefault.Enabled = false;
                        // Begin TT#1530 - RMatelic - Record locking violations from Store Attribute change
                        //chkDefault.Enabled = true;
                        chkDefault.Enabled = FunctionSecurity.AllowUpdate;
                        // End TT#1530
					}

					_GLFProfile = new GroupLevelFunctionProfile(Convert.ToInt32(cbxStoreGroupLevel.SelectedValue, CultureInfo.CurrentUICulture));
					this.cboGLGroupBy.SelectedValue = Convert.ToInt32(eGroupLevelSmoothBy.None, CultureInfo.CurrentUICulture).ToString(CultureInfo.CurrentUICulture);
                    // BEGIN Issue 5420 KJohnson - rdoEqualizeYesTY not holding it value
                    this.rdoEqualizeYesTY.Checked = false;
                    this.rdoEqualizeNoTY.Checked = true;

                    this.rdoEqualizeYesLY.Checked = false;
                    this.rdoEqualizeNoLY.Checked = true;

                    this.rdoEqualizeYesTrend.Checked = false;
                    this.rdoEqualizeNoTrend.Checked = true;
                    // END Issue 5420 KJohnson
					_sglOrigVal = cbxStoreGroupLevel.SelectedValue.ToString();
					//_GLFType = cboFuncType.SelectedValue.ToString();
					//_GLSBy = cboGLGroupBy.SelectedValue.ToString();
					radNone.Checked = true;
					this.gridLYNodeVersion.Enabled = false;
					this.gridTrendNodeVersion.Enabled = false;
					// Begin Issue 4218 stodd 
					this.cbxAltLY.Checked = false;
					this.cbxAltTrend.Checked = false;
					// End issue 4218
				}
				if (this.cboFuncType.Text == MIDText.GetTextOnly((int)eGroupLevelFunctionType.PercentContribution))
				{
					this.pnlPercentContribution.Visible = true;
					this.pnlTYLYTrend.Visible = false;
				}
				else if (this.cboFuncType.Text == MIDText.GetTextOnly((int)eGroupLevelFunctionType.TyLyTrend))
				{
					this.pnlPercentContribution.Visible = false;
					this.pnlTYLYTrend.Visible = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "LoadSetMethod");
			}
		}

		/// <summary>
		/// Load text of Hierarchy Node to OTS Plan
		/// </summary>
		/// <param name="hnp">Loaded HierarchyNodeProfile</param>
		private void LoadOTSPlanText(HierarchyNodeProfile hnp)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				this.txtOTSHNDesc.Text = hnp.Text;
				if (!FunctionSecurity.AllowUpdate)
				{
					this.txtOTSHNDesc.Enabled = false;
				}
				//Add RID to Control's Tag (for later use)
				int lAddTag = hnp.Key;
                //Begin Track #5858 - KJohnson - Validating store security only
                ((MIDTag)(txtOTSHNDesc.Tag)).MIDTagData = hnp;
                //End Track #5858
				// BEGIN Issue 4858 stodd 11.2.2007
				bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
				base.ApplyCanUpdate(canUpdate);
                //Begin Track #5858 - JSmith - Validating store security only
                //base.ValidatePlanNodeSecurity(txtOTSHNDesc);
                base.ValidateStorePlanNodeSecurity(txtOTSHNDesc);
                //End Track #5858
				// END Issue 4858

			}
			catch(Exception ex)
			{
				HandleException(ex, "LoadOTSPlanText");
			}
		}

		/// <summary>
		/// Populate OTSPlan MIDDateRangeSelector
		/// </summary>
		/// <param name="dr">Populated MRSDateRage</param>
		/// <param name="midDRSel">MIDDateRangeSelector Control to be populated</param>
		private void LoadDateRangeText(DateRangeProfile dr, Controls.MIDDateRangeSelector midDRSel)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				midDRSel.Text= dr.DisplayDate;
			
				//Add RID to Control's Tag (for later use)
				int lAddTag = dr.Key;
			
				midDRSel.Tag = lAddTag;
				midDRSel.DateRangeRID = lAddTag;

				//Display Dynamic picture or not
				if (dr.DateRangeType == eCalendarRangeType.Dynamic)
					midDRSel.SetImage(this.DynamicToCurrentImage);
				else
					midDRSel.SetImage(null);
				//=========================================================
				// Override the image if this is a dynamic switch date.
				//=========================================================
				if (dr.IsDynamicSwitch)
					midDRSel.SetImage(this.DynamicSwitchImage);
			}
			catch(Exception ex)
			{
				HandleException(ex, "LoadDateRangeText");
			}
		}

		#endregion

		#region Properties Tab - Workflows
		/// <summary>
        /// Fill the workflow grid
		/// </summary>
		private void LoadWorkflows()//9-17
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                GetOTSPLANWorkflows(_OTSPlanMethod.Key, ugWorkflows);

            }
            catch (Exception ex)
            {
                HandleException(ex, "LoadWorkflows");
            }
		}
		
		#endregion

		private void LoadPctContribution()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		private void LoadTYLYTrend()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
		}
		#region Command Buttons

		//Create string of error messages
		private string HandleErrorMsgs(ArrayList al, string sectionName)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			string strErr = null;
			string s = null;

			if(al.Count > 1)
				s = "s";

			strErr = "\n" + sectionName + " Error" + s + ":\n";

			foreach(string err in al)
			{
				strErr = strErr + err + "\n";
			}
			return strErr;
		}

		protected override void Call_btnSave_Click()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				base.btnSave_Click();
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}	
		}
		// End MID Track 4858

		private void midDateRangeSelectorOTSPlan_ClickCellButton(object sender, CellEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			// tells the date range selector the currently selected date range RID
			if (midDateRangeSelectorOTSPlan.Tag != null)
				((CalendarDateSelector)midDateRangeSelectorOTSPlan.DateRangeForm).DateRangeRID = (int)midDateRangeSelectorOTSPlan.Tag;
			//midDateRangeSelectorOTSPlan.DateRangeRID = (int)midDateRangeSelectorOTSPlan.Tag;

			((CalendarDateSelector)midDateRangeSelectorOTSPlan.DateRangeForm).AllowDynamicToStoreOpen = false;
			((CalendarDateSelector)midDateRangeSelectorOTSPlan.DateRangeForm).AllowDynamicToPlan = false;
			((CalendarDateSelector)midDateRangeSelectorOTSPlan.DateRangeForm).RestrictToOnlyWeeks = true;
			((CalendarDateSelector)midDateRangeSelectorOTSPlan.DateRangeForm).AllowDynamicSwitch = true;
			
			// tells the control to show the date range selector form
			midDateRangeSelectorOTSPlan.ShowSelector();
		}

		#endregion

		#region DragNDrop

		#region "DragNDrop Node to OTS Plan"
		private void txtOTSHNDesc_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
                // Begin TT#1332 - JSmith - Object Reference Error when changing data in an existing OTS method
                int originalHNRID = _OTSPlanMethod.Plan_HN_RID;
                DialogResult dr = DialogResult.Yes;
				// BEGIN TT#1609 - STodd - Object Reference Error when changing data in an existing OTS method
				//if (_OTSPlanMethod.LowLevelsInd)
				if (chkLowLevels.Checked)
				// END TT#1609 - STodd 
                {
                    dr = MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_ConfirmReplaceMerchandise),
                        MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanMethod), MessageBoxButtons.YesNo);

                    if (dr == DialogResult.No)
                    {
                        return;
                    }
                }
                // End TT#1332

                //Begin Track #5858 - Kjohnson - Validating store security only
                bool isSuccessfull = ((MIDTextBoxTag)(((TextBox)sender).Tag)).TextBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)(((TextBox)sender).Tag)).MIDTagData;

                    _nodeRID = hnp.Key;
                    _OTSPlanMethod.Plan_HN_RID = _nodeRID;

                    ChangePending = true;
                    ApplySecurity();

                    //Begin Track #4371 - JSmith - Multi-level forecasting.
                    if (chkLowLevels.Enabled && chkLowLevels.Checked)
                    {
                        RemoveOverrideLowLevelModel();  // TT#4227 - JSmith - Changing merchandise in method when custom override low level model is defined results in error when attempting to display model
                        
                        _OTSPlanMethod.LowlevelOverrideList.Clear();
                        PopulateLowLevels(hnp);
                    }
                    else
                    {
                        chkHighLevel.Enabled = true;
                        chkHighLevel.Checked = true;
                        chkLowLevels.Enabled = true;
                        chkLowLevels.Checked = false;
                    }
                    //End Track #4371

                    // Begin TT#1332 - JSmith - Object Reference Error when changing data in an existing OTS method
                    if (originalHNRID != Include.NoRID &&
                        originalHNRID != _OTSPlanMethod.Plan_HN_RID)
                    {
                        UpdateGLFPNodes(originalHNRID, _OTSPlanMethod.Plan_HN_RID);
                        if (chkLowLevels.Checked)
                        {
                            chkLowLevels.Checked = false;
                            chkLowLevels.Enabled = true;	// TT#1609 - stodd - null ref error
                        }
                    }
                    // End TT#1332
                }
                //End Track #5858 - Kjohnson
				//End Track #4371
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

        // Begin TT#4227 - JSmith - Changing merchandise in method when custom override low level model is defined results in error when attempting to display model
        private void RemoveOverrideLowLevelModel()
        {
            OverrideLowLevelProfile ollp;
            int OverrideLowLevelRid = _OTSPlanMethod.OverrideLowLevelRid;
            int CustomOLL_RID = _OTSPlanMethod.CustomOLL_RID;
            cboOverride.SelectedIndex = 0;
            if (OverrideLowLevelRid != Include.NoRID)
            {
                ollp = new OverrideLowLevelProfile(OverrideLowLevelRid);
                // Delete if custom model
                if (ollp.User_RID == Include.CustomUserRID)
                {
                    ollp.DeleteModel(OverrideLowLevelRid);
                }
            }
            if (CustomOLL_RID != Include.NoRID)
            {
                ollp = new OverrideLowLevelProfile(CustomOLL_RID);
                // Delete if custom model
                if (ollp.User_RID == Include.CustomUserRID)
                {
                    ollp.DeleteModel(CustomOLL_RID);
                }
            }
        }
        // End TT#4227 - JSmith - Changing merchandise in method when custom override low level model is defined results in error when attempting to display model

        // Begin TT#1332 - JSmith - Object Reference Error when changing data in an existing OTS method
        /// <summary>
        /// Update the node key in the GroupLevelNodeFunction objects to new node 
        /// dropping all low level nodes.
        /// </summary>
        /// <param name="originalHNRID">The original node key</param>
        /// <param name="newHNRID">The new node key</param>
        private void UpdateGLFPNodes(int originalHNRID, int newHNRID)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            ArrayList nodes = new ArrayList();
            try
            {
                foreach (GroupLevelFunctionProfile glfp in _OTSPlanMethod.GLFProfileList.ArrayList)
                {
                    nodes.Clear();
                    // update node in objects
                    foreach (GroupLevelNodeFunction glnf in glfp.Group_Level_Nodes.Values)
                    {
                        if (glnf.HN_RID == originalHNRID)
                        {
                            glnf.HN_RID = newHNRID;
                            nodes.Add(glnf);
                        }
                    }
                    // transfer updated nodes back to hashtable
                    glfp.Group_Level_Nodes.Clear();
                    if (nodes.Count > 0)
                    {
                        foreach (GroupLevelNodeFunction glnf in nodes)
                        {
                            glfp.Group_Level_Nodes.Add(glnf.HN_RID, glnf);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // End TT#1332

		private void txtOTSHNDesc_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Image_DragEnter(sender, e);
		}

		private void txtOTSHNDesc_DragOver(object sender, DragEventArgs e)
		{

        //TT#695  - Begin - MD - RBeck - Drag and drop of size merchandise causes error
            TreeNodeClipboardList cbList;
            if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
            {
                cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);
                if (hnp == null || hnp.LevelType == eHierarchyLevelType.Size)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }
                else
                {
                    Image_DragOver(sender, e);
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        //TT#695  - End - MD - RBeck - Drag and drop of size merchandise causes error
            // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            //Image_DragOver(sender, e);
		}

		#endregion

		#endregion

		#region React to Changes (Combos, etc.)

		/// <summary>
		/// Change cboChainVers to selected value of cboPlanVers when cboPlanVers changes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboPlanVers_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				if (this.cboPlanVers.SelectedValue != null)
				{
                    //if ((int)this.cboChainVers.SelectedValue != (int)this.cboPlanVers.SelectedValue)  //TT#736 - MD - ComboBox causes a NullReferenceException - RBeck
                    if (Convert.ToInt32(cboChainVers.SelectedValue, CultureInfo.CurrentUICulture) != (int)this.cboPlanVers.SelectedValue) 
					{
						this.cboChainVers.SelectedValue =  this.cboPlanVers.SelectedValue;
					}
					if (FormLoaded)
					{
						ChangePending = true;
						// BEGIN Issue 4858 stodd
						_OTSPlanMethod.Plan_FV_RID = (int)this.cboPlanVers.SelectedValue;
						bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
						base.ApplyCanUpdate(canUpdate);
						// END Issue 4858
					}
                    //Begin Track #5858 - JSmith - Validating store security only
                    //base.ValidatePlanVersionSecurity(cboPlanVers.ComboBox);
                    base.ValidateStorePlanVersionSecurity(cboPlanVers.ComboBox);                    // TT#7 - RBeck - Dynamic dropdowns
                    //End Track #5858
				}
			}
			catch(Exception ex)
			{
				HandleException(ex, "cboPlanVers_SelectionChangeCommitted");
			}
		}

		private void cboModel_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		//Begin Track #4371 - JSmith - Multi-level forecasting.
		private void chkLowLevels_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (FormLoaded)
			{
				ChangePending = true;
			}
			if (chkLowLevels.Checked)
			{
				cboLowLevels.Enabled = true;
				btnExcludeLowLevels.Enabled = true;
				PopulateLowLevels(null);
				lblLowLevelDefault.Visible = true;	// Issue 4573 stodd 8.10.07
			}
			else
			{
				cboLowLevels.Enabled = false;
				btnExcludeLowLevels.Enabled = false;
				RemoveBasisMerchandiseValueList();
				lblLowLevelDefault.Visible = true;	// Issue 4573 stodd 8.21.07
				lblLowLevelDefault.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_InheritFrom); // Issue 4573 stodd 8.21.07
			}

			ToggleLowLevelMinMaxDisplay(chkLowLevels.Checked);
		}

		private void ToggleLowLevelMinMaxDisplay(bool showLowLevels)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			// Hide drop down
			lblLowLevelMerchandise.Visible = showLowLevels;
			cboMerchandise.Visible = showLowLevels;
			
			// Hide Method radio buttons
//			lblLowLevelDefault.Visible = showLowLevels;
//			rdoHierarchy.Visible = showLowLevels;
			rdoMethod.Visible = showLowLevels;
//			rdoNone.Visible = showLowLevels;
		}

		private void chkHighLevel_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (FormLoaded)
			{
				ChangePending = true;
				PopulateLowerLevelsCombo();
			}
		}

		private void cboLowLevels_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
					if (cboLowLevels.SelectedIndex != -1)
					{
						_OTSPlanMethod.LowLevelsType = ((LowLevelCombo)cboLowLevels.SelectedItem).LowLevelType;
						_OTSPlanMethod.LowLevelsOffset = ((LowLevelCombo)cboLowLevels.SelectedItem).LowLevelOffset;
						_OTSPlanMethod.LowLevelsSequence = ((LowLevelCombo)cboLowLevels.SelectedItem).LowLevelSequence;
						_OTSPlanMethod.LowlevelOverrideList.Clear();
						if (_OTSPlanMethod.Plan_HN_RID == Include.NoRID)
						{
                            //Begin Track #5858 - KJohnson - Validating store security only
                            HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)txtOTSHNDesc.Tag).MIDTagData;
                            _OTSPlanMethod.Plan_HN_RID = hnp.Key;
                            _OTSPlanMethod.Orig_Plan_HN_RID = hnp.Key;
                            //End Track #5858
						}
						//_OTSPlanMethod.PopulateExcludeList(_OTSPlanMethod.Key);
						PopulateLowerLevelsCombo();
						PopulateBasisMerchandiseValueList();
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		// BEGIN Override Low Level enhancment
		private void btnExcludeLowLevels_Click(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (FormLoaded)
			{
				Cursor.Current = Cursors.WaitCursor;
				try
				{
					string lowLevelText = string.Empty;
					if (cboLowLevels.SelectedIndex != -1)
						lowLevelText = cboLowLevels.Items[cboLowLevels.SelectedIndex].ToString();

					System.Windows.Forms.Form parentForm;
					parentForm = this.MdiParent;

					object[] args = null;
					//System.Windows.Forms.Form frm;
					// Begin Track #5909 - stodd
					FunctionSecurityProfile methodSecurity;
					if (radGlobal.Checked)
						methodSecurity = GlobalSecurity;
					else
						methodSecurity = UserSecurity;
					args = new object[] { SAB, _OTSPlanMethod.OverrideLowLevelRid, _OTSPlanMethod.Plan_HN_RID, _OTSPlanMethod.Plan_FV_RID, lowLevelText, _OTSPlanMethod.CustomOLL_RID, methodSecurity };
					// End Track #5909 - stodd
					_overrideLowLevelfrm = GetForm(typeof(frmOverrideLowLevelModel), args, false);
					parentForm = this.MdiParent;
					_overrideLowLevelfrm.MdiParent = parentForm;
					_overrideLowLevelfrm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
					_overrideLowLevelfrm.Show();
					_overrideLowLevelfrm.BringToFront();
					((frmOverrideLowLevelModel)_overrideLowLevelfrm).OnOverrideLowLevelCloseHandler += new frmOverrideLowLevelModel.OverrideLowLevelCloseEventHandler(OnOverrideLowLevelCloseHandler);
				}
				finally
				{
					Cursor.Current = Cursors.Default;
				}
			}
		}

		private void OnOverrideLowLevelCloseHandler(object source, OverrideLowLevelCloseEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				if (FormLoaded)
				{
					if (_OTSPlanMethod.OverrideLowLevelRid != e.aOllRid)
						ChangePending = true;
					_OTSPlanMethod.OverrideLowLevelRid = e.aOllRid;
					if (_OTSPlanMethod.CustomOLL_RID != e.aCustomOllRid)
					{
						_OTSPlanMethod.CustomOLL_RID = e.aCustomOllRid;
						UpdateMethodCustomOLLRid(_OTSPlanMethod.Key, _OTSPlanMethod.CustomOLL_RID);
					}

					// BEGIN TT#696 - Overrid Low Level Model going blank on Close.
					if (_overrideLowLevelfrm.DialogResult != DialogResult.Cancel)
					{
                        LoadOverrideModelComboBox(cboOverride.ComboBox, e.aOllRid, _OTSPlanMethod.CustomOLL_RID); //TT#7 - RBeck - Dynamic dropdowns
						PopulateLowerLevelsCombo();
					}

                    _overrideLowLevelfrm = null;
					// End TT#696 - Overrid Low Level Model going blank on Close.
				}
			}
			catch
			{
				throw;
			}
			
		}
		// END Override Low Level enhancment

		private void PopulateLowLevels(HierarchyNodeProfile aHierarchyNodeProfile)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				HierarchyProfile hierProf;
				cboLowLevels.Items.Clear();
				if (aHierarchyNodeProfile == null)
				{
					if (_nodeRID == Include.NoRID)
					{
						return;
					}
					//Begin Track #5378 - color and size not qualified
//					aHierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeData(_nodeRID, false);
                    // Begin TT#2647 - JSmith - Delays in OTS Method
                    //aHierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeData(_nodeRID, false, true);
                    if (!_nodesByRID.TryGetValue(_nodeRID, out aHierarchyNodeProfile))
                    {
                        aHierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeData(_nodeRID, false, true);
                        _nodesByRID.Add(_nodeRID, aHierarchyNodeProfile);
                        if (!_nodesByID.ContainsKey(aHierarchyNodeProfile.NodeID))
                        {
                            _nodesByID.Add(aHierarchyNodeProfile.NodeID, aHierarchyNodeProfile);
                        }
                    }
                    // End TT#2647 - JSmith - Delays in OTS Method
					//End Track #5378
				}
				
				if (aHierarchyNodeProfile != null)
				{
					cboLowLevels.Enabled = true;
					btnExcludeLowLevels.Enabled = true;
					hierProf = SAB.HierarchyServerSession.GetHierarchyData(aHierarchyNodeProfile.HierarchyRID);
					if (hierProf.HierarchyType == eHierarchyType.organizational)
					{
						for (int i = aHierarchyNodeProfile.HomeHierarchyLevel + 1; i <= hierProf.HierarchyLevels.Count; i++)
						{
							HierarchyLevelProfile hlp = (HierarchyLevelProfile)hierProf.HierarchyLevels[i];
							cboLowLevels.Items.Add(
								new LowLevelCombo(eLowLevelsType.HierarchyLevel,
								//Begin Track #5866 - JScott - Matrix Balance does not work
								//0,
								i - aHierarchyNodeProfile.HomeHierarchyLevel,
								//End Track #5866 - JScott - Matrix Balance does not work
								hlp.Key,
								hlp.LevelID));
						}
					}
					else
					{
						HierarchyProfile mainHierProf = SAB.HierarchyServerSession.GetMainHierarchyData();

						if (_currentLowLevelNode != aHierarchyNodeProfile.Key)
						{
							_longestHighestGuest = SAB.HierarchyServerSession.GetHighestGuestLevel(aHierarchyNodeProfile.Key);
						}
						int highestGuestLevel = _longestHighestGuest;

						// add guest levels to comboBox
						if ((highestGuestLevel != int.MaxValue) && (aHierarchyNodeProfile.HomeHierarchyType != eHierarchyType.alternate)) // TT#55 - KJohnson - Override Level option needs to reflect Low level already selected(in all review screens and methods with override level option)
						{
							for (int i = highestGuestLevel; i <= mainHierProf.HierarchyLevels.Count; i++)
							{
								if (i == 0)
								{
									cboLowLevels.Items.Add(
										new LowLevelCombo(eLowLevelsType.HierarchyLevel,
										0,
										0,
										"Root"));
								}
								else
								{
									HierarchyLevelProfile hlp = (HierarchyLevelProfile)mainHierProf.HierarchyLevels[i];
									cboLowLevels.Items.Add(
										new LowLevelCombo(eLowLevelsType.HierarchyLevel,
										//Begin Track #5866 - JScott - Matrix Balance does not work
										//0,
										i,
										//End Track #5866 - JScott - Matrix Balance does not work
										hlp.Key,
										hlp.LevelID));
								}
							}
						}

						// add offsets to comboBox
						if (_currentLowLevelNode != aHierarchyNodeProfile.Key)
						{
                            //BEGIN TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly
                            //_longestBranch = SAB.HierarchyServerSession.GetLongestBranch(aHierarchyNodeProfile.Key);
                            DataTable hierarchyLevels = SAB.HierarchyServerSession.GetHierarchyDescendantLevels(aHierarchyNodeProfile.Key);
                            _longestBranch = hierarchyLevels.Rows.Count - 1;
                            //END TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly
						}
						int longestBranchCount = _longestBranch; 
						int offset = 0;
						for (int i = 0; i < longestBranchCount; i++)
						{
							++offset;
							cboLowLevels.Items.Add(
								new LowLevelCombo(eLowLevelsType.LevelOffset,
								offset,
								0,
								null));
						}
					}
					if (cboLowLevels.Items.Count > 0)
					{
						cboLowLevels.SelectedIndex = 0;
					}
					//_OTSPlanMethod.LowlevelExcludeList.Clear();
					_currentLowLevelNode = aHierarchyNodeProfile.Key;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Track #4371 

		/// <summary>
		/// Change cboStoreGroupLevel selected value based on key of cboStoreGroups
		/// (Change Store_Group_Levels based on selected Store_Group)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboStoreGroups_SelectionChangeCommitted(object sender, System.EventArgs e)
		{	
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				this.Cursor = Cursors.WaitCursor;
                //***********************************************************************************************
				// _resetStoreGroup is used to catch when a user changes the storeGroup, but then decides not
				// to.  When this happens we have to change the store group back to it's original value...which
				// causes us to come right back into this method. 
				// _resetStoreGroup allows us to skip this method the second time around.
				//***********************************************************************************************
				if (_resetStoreGroup)
				{
					_resetStoreGroup = false;
				}
				else
				{
					DialogResult dr = DialogResult.Yes;
					if (!_InitialPopulate)
					{
						_resetStoreGroup = false;
						dr=MessageBox.Show("Changing the current Attribute will abandon any changes to the Sets below.  Are you sure you want to continue?",
							MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanMethod), MessageBoxButtons.YesNo);
					}

                            if (dr==DialogResult.No) 
                            {
                                _resetStoreGroup = true;
                                this.cboStoreGroups.SelectedValue = this._OTSPlanMethod.SG_RID;
                            }
                            else if (dr==DialogResult.Yes) 
                            {
                                if (this.cboStoreGroups.SelectedValue != null)
                                {
                                    _setChanged = true;
                                    if (!_InitialPopulate)
                                    {
                                        this._defaultChecked = false;
                                    }
                                    if (!(_newSetMethod))
                                        PopulateStoreGLComboCheck(this.cboStoreGroups.SelectedValue.ToString());
                                    else
                                        PopulateStoreGLCombo(this.cboStoreGroups.SelectedValue.ToString());

                                }
			
                                if (!_InitialPopulate)
                                {
                                    this._GLFProfileList.Clear();
                                    SetMethodValues(_GLFProfile.GLF_Change_Type);
                                    // BEGIN Issue 5295 stodd
                                    if(cboMerchandise.Visible)
                                        cboMerchandise.SelectedIndex = 0;
                                    // END Issue
                                    cboStoreGroupLevelChange();
                                    ClearGLNFUI();
                                }
                                _setChanged = false;

                                if (FormLoaded)
                                {
                                    ChangePending = true;
                                }
                            } 
                        }
                        if (FormLoaded)
                        {
                            ChangePending = true;
                        }
                    }
                    catch(Exception ex)
                    {
                        HandleException(ex, "cboStoreGroups_SelectionChangeCommitted");
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }

        private void cboStoreGroups_DragDrop(object sender, DragEventArgs e)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            //Begin Track #5858 - Kjohnson - Validating store security only
            try
            {
                bool isSuccessfull = ((MIDComboBoxTag)(((MIDComboBoxEnh)sender).Tag)).ComboBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    ChangePending = true;

                    // Begin TT#284-MD - JSmith - OTS Forecast Method errors on open
// BEGIN TT#7 - RBeck - Dynamic dropdowns                    
                    ////EventArgs z = new EventArgs();
                    //this.cboStoreGroups_DropDownClosed(this, z);                 
// END TT#7 - RBeck - Dynamic dropdowns
                    // End TT#284-MD - JSmith - OTS Forecast Method errors on open
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
            //End Track #5858
        }

		private void cboStoreGroupLevel_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				if (!_InitialPopulate)
				{
                    // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                    _storeGroupLevelChanged = true;
                    // End TT#3
					if (_setReset)
					{
						_setReset = false;
						return;
					}
					if (!_setChanged)
					{
						if (!ValidBasis())
						{
							string text = MIDText.GetTextOnly(eMIDTextCode.msg_ErrorsFoundReviewCorrect);
							MessageBox.Show(text);
							_setReset = true;
							cbxStoreGroupLevel.SelectedValue = _prevSetValue;
						}
						else
						{
                            // Begin TT#2647 - JSmith - Delays in OTS Method
                            //// Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                            //bool isDefaultSet = _GLFProfile.Default_IND;
                            // End TT#3
                            // End TT#2647 - JSmith - Delays in OTS Method
//							SetStockMinMax();
							ComboObject selectedNode = (ComboObject)cboMerchandise.SelectedItem;
							SetSetMethodValues(_GLFProfile.GLF_Change_Type);
                            // Begin TT#2647 - JSmith - Delays in OTS Method
                            bool isDefaultSet = _GLFProfile.Default_IND;
                            // End TT#2647 - JSmith - Delays in OTS Method
							cboStoreGroupLevelChange();
							_prevSetValue = Convert.ToInt32(cbxStoreGroupLevel.SelectedValue,CultureInfo.CurrentUICulture);
							ClearGLNFUI();
							cboMerchandise.SelectedItem = selectedNode;
//							populateGLNF(false);
                            // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
							//BEGIN TT#331 - MD - DOConnell - OTS Forecast get an error when trying to go to the TY tab and cannot populate the other sets basis data.
                            //if (isDefaultSet)
                            //{
                                // Begin TT#1674 - JSmith - Mins/max disappearing in the OTS forecast method
                                GroupLevelFunctionProfile saveGLFProfile = _GLFProfile;
                                // END TT#1674
                                // Begin TT#2647 - JSmith - Delays in OTS Method
                                //foreach (GroupLevelFunctionProfile glfp in _OTSPlanMethod.GLFProfileList.ArrayList)
                                //{
                                //    if (glfp.Use_Default_IND)
                                //    {
                                //        CopyMinMax(glfp.Key, false);
                                //    }
                                //}
                                if (ChangePending)
                                {
                                    UpdateSetsUsingDefault();
                                }
                                LoadSetMethod();
                                //for (int i = 0; i < _OTSPlanMethod.GLFProfileList.Count; i++)
                                //{
                                //    GroupLevelFunctionProfile glfp = (GroupLevelFunctionProfile)_OTSPlanMethod.GLFProfileList[i];
                                //    if (glfp.Use_Default_IND)
                                //    {
                                //        glfp = DefaultGLFProfile.CopyTo(glfp, SAB.ClientServerSession, false);
                                //    }
                                //}
                                // End TT#2647 - JSmith - Delays in OTS Method
                                // Begin TT#1674 - JSmith - Mins/max disappearing in the OTS forecast method
                                _GLFProfile = saveGLFProfile;
                                populateGLNF(true);
                                // END TT#1674
                            //}
                            // End TT#3
						}
                        if (!_GLFProfile.Filled)
                        {
                            this.tabTYLYTrend.SelectedTab = this.tabPageTY;
                        }
						//END TT#331 - MD - DOConnell - OTS Forecast get an error when trying to go to the TY tab and cannot populate the other sets basis data.
					}
				}
			}
			catch(Exception ex)
			{
				HandleException(ex, "cboStoreGroupLevel_SelectionChangeCommitted");
			}
            // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
            finally
            {
                _storeGroupLevelChanged = false;
            }
            // End TT#3
		}

		//Combo box in Set Method Group Box
		private void cboStoreGroupLevelChange()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				int glfKey = Convert.ToInt32(this.cbxStoreGroupLevel.SelectedValue, CultureInfo.CurrentUICulture);

				if (_GLFProfileList.Contains(glfKey))
				{
					_newSetMethod = false;
					//ToggleSaveCaption(false);
					_GLFProfile = (GroupLevelFunctionProfile)_GLFProfileList.FindKey(glfKey);
				}
				else
				{
					_newSetMethod = true;
                    // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                    _GLFProfile = new GroupLevelFunctionProfile(glfKey);
                    // End TT#3
					//ToggleSaveCaption(true);
				}

                // Begin TT#1044 - JSmith - Apply Trend to TAb is taking an unusual amount of time to apply a basis- this iwill be a problem for ANF
                //if (_dtSource != null)
                //    _dtSource.DefaultView.RowFilter = "SGL_RID = " +  glfKey.ToString() +
                //        " AND (" + "TYLY_TYPE_ID = " + ((int)eTyLyType.NonTyLy).ToString() +
                //        " OR TYLY_TYPE_ID = 0)"; //MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen

                //if (_dtSourceTYNode != null)
                //    _dtSourceTYNode.DefaultView.RowFilter = "SGL_RID = " +  glfKey.ToString() +
                //        " AND " + "TYLY_TYPE_ID = " + ((int)eTyLyType.TyLy).ToString();
				
                //if (_dtSourceLYNode !=  null)
                //    _dtSourceLYNode.DefaultView.RowFilter = "SGL_RID = " +  glfKey.ToString() +
                //        " AND " + "TYLY_TYPE_ID = " + ((int)eTyLyType.AlternateLy).ToString();
		 
                //if (_dtSourceTrendNode !=  null)
                //    _dtSourceTrendNode.DefaultView.RowFilter = "SGL_RID = " +  glfKey.ToString() +
                //        " AND " + "TYLY_TYPE_ID = " + ((int)eTyLyType.AlternateApplyTo).ToString();

                // Begin TT#2647 - JSmith - Delays in OTS Method
                if (_GLFProfile.Use_Default_IND)
                {
                    glfKey = _defaultStoreGroupLevelRid;
                }
                // End TT#2647 - JSmith - Delays in OTS Method

                if (_dtSource != null)
                    _dtSource.DefaultView.RowFilter = "SGL_RID = " + glfKey.ToString(); //MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen

                if (_dtSourceTYNode != null)
                    _dtSourceTYNode.DefaultView.RowFilter = "SGL_RID = " + glfKey.ToString();

                if (_dtSourceLYNode != null)
                    _dtSourceLYNode.DefaultView.RowFilter = "SGL_RID = " + glfKey.ToString();

                if (_dtSourceTrendNode != null)
                    _dtSourceTrendNode.DefaultView.RowFilter = "SGL_RID = " + glfKey.ToString();
                // End TT#1044
				 
				LoadSetMethod();
			}
			catch(Exception ex)
			{
				HandleException(ex, "cboStoreGroupLevelChange");
			}
		}


		private void ClearChildrenFilters(Infragistics.Win.UltraWinGrid.RowsCollection RowsCollection )
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			// clear the filter for this rows collection 
			RowsCollection.ColumnFilters.ClearAllFilters();        

			foreach(Infragistics.Win.UltraWinGrid.UltraGridRow row in RowsCollection)
			{
				if (row.HasChild())
					// this line assumes that you only have one child band per parent
					// this code would need to be modified if you have have multiple child bands
					//' per parent band
					// ClearChildrenFilters(row.ChildBands(0).Rows)

					// if you have multiple child bands then you would need something like this
					//                 
					foreach (Infragistics.Win.UltraWinGrid.UltraGridChildBand band in row.ChildBands)
						ClearChildrenFilters(band.Rows);                
			}
		}   


		/// <summary>
		/// Display Save or Update caption of btnSave button
		/// </summary>
		/// <param name="isSave"></param>
		private void ToggleSaveCaption(bool isSave)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (isSave)
			{
				btnSave.Text = "&Save";
				return;
			}
			btnSave.Text = "&Update";
		}

		private void chkDefault_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				// begin MID Track 2375 
				
				
				
				//chkUseDefault.Checked = false;
				//chkUseDefault.Enabled = false;

				// when the default indicator on a Set is changed from true to false,
				// all Sets that had Use Default equal to true must be changed to equal false.
				if (!chkDefault.Checked && _defaultStoreGroupLevelRid == _GLFProfile.Key)
				{
                    // Begin TT#2647 - JSmith - Delays in OTS Method
                    UpdateSetsUsingDefault();
                    // End TT#2647 - JSmith - Delays in OTS Method

					this._OTSPlanMethod.SetAllUseDefaultToFalse();
				}
				//end MID Track 2375
	
                // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                //if (FormLoaded)
                if (FormLoaded &&
                    !_storeGroupLevelChanged)
                // End TT#3
				{
					ChangePending = true;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex, "chkDefault_CheckedChanged");
			}
		}

		private void chkDefault_Click(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (chkDefault.Checked)
			{
				_defaultChecked = true;

                // BEGIN Issue 5420 KJohnson - rdoEqualizeYesTY not holding it value
                if (this.rdoEqualizeYesTY.Checked)
                {
                    _defaultEqualizeTY = true;
                }
                else
                {
                    _defaultEqualizeTY = false;
                }

                if (this.rdoEqualizeYesLY.Checked)
                {
                    _defaultEqualizeLY = true;
                }
                else
                {
                    _defaultEqualizeLY = false;
                }

                if (this.rdoEqualizeYesTrend.Checked)
                {
                    _defaultEqualizeTrend = true;
                }
                else
                {
                    _defaultEqualizeTrend = false;
                }
                // END Issue 5420 KJohnson

				if(this.cboFuncType.SelectedValue != null)
					_defaultFunctionType = (int)this.cboFuncType.SelectedValue;
				else
					_defaultFunctionType = (int)Include.UndefinedDouble;

                // Begin TT#2647 - JSmith - Delays in OTS Method
                //if (this.cbxStoreGroupLevel.SelectedValue != null)
                //    _defaultStoreGroupLevelRid = (int)cbxStoreGroupLevel.SelectedValue;
                //else
                //    _defaultStoreGroupLevelRid = Include.NoRID;
                if (this.cbxStoreGroupLevel.SelectedValue != null)
                {
                    _defaultStoreGroupLevelRid = (int)cbxStoreGroupLevel.SelectedValue;
                }
                else
                {
                    _defaultStoreGroupLevelRid = Include.NoRID;
                    DefaultGLFProfile = null;
                }
                // End TT#2647 - JSmith - Delays in OTS Method

			}
			else
			{
				_defaultChecked = false;
				_defaultStoreGroupLevelRid = Include.NoRID;

			}
		}

		/// <summary>
		/// chkClear Check box changed, if checked, check and disable Plan.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void chkClear_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				if (chkClear.Checked)
					CheckAndDisablePlan();
				else
					if (!chkUseDefault.Checked)
					chkPlan.Enabled = true;
				
				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex, "chkClear_CheckedChanged");
			}
		}

		/// <summary>
		/// chkUseDefault Check box changed, if checked, check and disable Plan.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void chkUseDefault_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				if (chkUseDefault.Checked)
				{
                    // Begin TT#1674 - JSmith - Mins/max disappearing in the OTS forecast method
                    _useDefaultSetTrue = true;
                    // End TT#1674
					// IF this Store Group Level has UseDefault checked And it's function type is
					// different from the current default, change it to match the current default.
					if ((int)cboFuncType.SelectedValue != _defaultFunctionType)
					{
                        this.rdoEqualizeYesTY.Checked = true;  // issue 5420
                        this.rdoEqualizeNoTY.Checked = false;  // issue 5420
                        this.rdoEqualizeYesLY.Checked = true;  // issue 5420
                        this.rdoEqualizeNoLY.Checked = false;  // issue 5420
                        this.rdoEqualizeYesTrend.Checked = true;  // issue 5420
                        this.rdoEqualizeNoTrend.Checked = false;  // issue 5420
                        // Begin TT#2647 - JSmith - Delays in OTS Method
                        //SetFunctionTypeSameAsDefault();
                        // End TT#2647 - JSmith - Delays in OTS Method
					}

                    // Begin TT#2647 - JSmith - Delays in OTS Method
                    if (FormLoaded &&
                        !_GLFProfile.Use_Default_IND)
                    {
                        // Begin TT#3420 - JSmith - Store Forecast Stock Min-Max at Low Level not being observed.
                        foreach (GroupLevelNodeFunction glnf in _GLFProfile.Group_Level_Nodes.Values)
                        {
                            glnf.MinMaxInheritType = eMinMaxInheritType.Default;
                        }
                        // End TT#3420 - JSmith - Store Forecast Stock Min-Max at Low Level not being observed.
                        _GLFProfile = DefaultGLFProfile.CopyTo(_GLFProfile, SAB.ClientServerSession, false, true);
                        _GLFProfile.Default_IND = false;
                        _GLFProfile.Use_Default_IND = true;
                        if (!_OTSPlanMethod.GLFProfileList.Contains(_GLFProfile.Key))
                            _OTSPlanMethod.GLFProfileList.Add(_GLFProfile);
                        else
                        {
                            _OTSPlanMethod.GLFProfileList.Remove(_GLFProfile);
                            _OTSPlanMethod.GLFProfileList.Add(_GLFProfile);
                        }
                        //cboStoreGroupLevel_SelectionChangeCommitted(cbxStoreGroupLevel.ComboBox, new System.EventArgs());
                        cboStoreGroupLevelChange();
                        ClearGLNFUI();
                        populateGLNF(true);
                    }
                    // End TT#2647 - JSmith - Delays in OTS Method

					CheckAndDisablePlan();
					if (this._defaultFunctionType != 0)  // a default has been selected
						cboFuncType.SelectedValue = this._defaultFunctionType;

				}
				else
				{
					if (!chkClear.Checked)
						chkPlan.Enabled = true;

                    // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                    if (rdoDefault.Checked)
                    {
                        rdoNone.Checked = true;
                    }
                    // End TT#3

                    // Begin TT#2647 - JSmith - Delays in OTS Method
                    if (FormLoaded &&
                        _GLFProfile.Use_Default_IND)
                    {
                        _GLFProfile = DefaultGLFProfile.CopyTo(_GLFProfile, SAB.ClientServerSession, false, true);
                        _GLFProfile.Default_IND = false;
                        _GLFProfile.Use_Default_IND = false;
                        if (!_OTSPlanMethod.GLFProfileList.Contains(_GLFProfile.Key))
                            _OTSPlanMethod.GLFProfileList.Add(_GLFProfile);
                        else
                        {
                            _OTSPlanMethod.GLFProfileList.Remove(_GLFProfile);
                            _OTSPlanMethod.GLFProfileList.Add(_GLFProfile);
                        }
                        SetFunctionTypeSameAsDefault();
                        //cboStoreGroupLevelChange();
                        cboStoreGroupLevel_SelectionChangeCommitted(cbxStoreGroupLevel.ComboBox, new System.EventArgs());
                    }
                    // End TT#2647 - JSmith - Delays in OTS Method
				}


				// copy default basis and disable grids if 'Use Default' is checked.
				if (_defaultFunctionType == (int)eGroupLevelFunctionType.PercentContribution)
				{
					if (chkUseDefault.Checked)
					{
						this.grdPctContBasis.Enabled = false;
						this.cboGLGroupBy.Enabled = false;  // issue 3816
                        this.cboFuncType.Enabled = false;  // issue 5420
                        this.rdoEqualizeYesTY.Enabled = false; // issue 5420
                        this.rdoEqualizeNoTY.Enabled = false; // issue 5420
                        this.rdoEqualizeYesLY.Enabled = false; // issue 5420
                        this.rdoEqualizeNoLY.Enabled = false; // issue 5420
                        this.rdoEqualizeYesTrend.Enabled = false; // issue 5420
                        this.rdoEqualizeNoTrend.Enabled = false; // issue 5420
                        // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                        if (FormLoaded &&
                            !_loadingSet)
                        {
                            rdoDefault.Checked = true;
                        }
                        // End TT#3
                    }
					else
                    {
						this.grdPctContBasis.Enabled = true;
						this.cboGLGroupBy.Enabled = true;  // issue 3816
                        this.cboFuncType.Enabled = true;  // issue 5420
                        this.rdoEqualizeYesTY.Enabled = true; // issue 5420
                        this.rdoEqualizeNoTY.Enabled = true; // issue 5420
                        this.rdoEqualizeYesLY.Enabled = true; // issue 5420
                        this.rdoEqualizeNoLY.Enabled = true; // issue 5420
                        this.rdoEqualizeYesTrend.Enabled = true; // issue 5420
                        this.rdoEqualizeNoTrend.Enabled = true; // issue 5420
                        // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                        if (FormLoaded &&
                            !_loadingSet)
                        {
                            rdoDefault.Checked = false;
                        }
                        // End TT#3
                    }
                }
				else if (_defaultFunctionType == (int)eGroupLevelFunctionType.TyLyTrend)
				{
					if (chkUseDefault.Checked)
					{
						this.gridTYNodeVersion.Enabled = false;
						this.gridLYNodeVersion.Enabled = false;
						this.gridTrendNodeVersion.Enabled = false;
						cbxAltLY.Enabled = false;
						this.cbxAltTrend.Enabled = false;
						this.grpTrendCaps.Enabled = false;
						this.cboGLGroupBy.Enabled = false;  // issue 3816
                        this.cboFuncType.Enabled = false;  // issue 5420
                        this.rdoEqualizeYesTY.Enabled = false; // issue 5420
                        this.rdoEqualizeNoTY.Enabled = false; // issue 5420
                        this.rdoEqualizeYesLY.Enabled = false; // issue 5420
                        this.rdoEqualizeNoLY.Enabled = false; // issue 5420
                        this.rdoEqualizeYesTrend.Enabled = false; // issue 5420
                        this.rdoEqualizeNoTrend.Enabled = false; // issue 5420
                        // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                        if (FormLoaded &&
                            !_loadingSet)
                        {
                            rdoDefault.Checked = true;
                        }
                        // End TT#3
                    }
					else
					{
						// Begin Track #5954 stodd
						if (cbxAltLY.Checked)
						{
							this.gridLYNodeVersion.Enabled = true;
							this.rdoEqualizeYesLY.Enabled = true; // issue 5420
							this.rdoEqualizeNoLY.Enabled = true; // issue 5420
						}
						else
						{
							this.gridLYNodeVersion.Enabled = false;
							this.rdoEqualizeYesLY.Enabled = false; // issue 5420
							this.rdoEqualizeNoLY.Enabled = false; // issue 5420
						}

						if (cbxAltTrend.Checked)
						{
							this.gridTrendNodeVersion.Enabled = true;
							this.rdoEqualizeYesTrend.Enabled = true; // issue 5420
							this.rdoEqualizeNoTrend.Enabled = true; // issue 5420
						}
						else
						{
							this.gridTrendNodeVersion.Enabled = false;
							this.rdoEqualizeYesTrend.Enabled = false; // issue 5420
							this.rdoEqualizeNoTrend.Enabled = false; // issue 5420
						}

						this.gridTYNodeVersion.Enabled = true;
						cbxAltLY.Enabled = true;
						this.cbxAltTrend.Enabled = true;
						this.grpTrendCaps.Enabled = true;
						this.cboGLGroupBy.Enabled = true;  // issue 3816
                        this.cboFuncType.Enabled = true;  // issue 5420
                        this.rdoEqualizeYesTY.Enabled = true; // issue 5420
                        this.rdoEqualizeNoTY.Enabled = true; // issue 5420
						// End Track #5954 stodd
                        // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                        if (FormLoaded &&
                            !_loadingSet)
                        {
                            rdoDefault.Checked = false;
                        }
                        // End TT#3
                    }
				}

                // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                //if (FormLoaded)
                if (FormLoaded &&
                    !_storeGroupLevelChanged)
                // End TT#3
				{
					ChangePending = true;
				}
				
			}
			catch(Exception ex)
			{
				HandleException(ex, "chkUseDefault_CheckedChanged");
			}
            // Begin TT#1674 - JSmith - Mins/max disappearing in the OTS forecast method
            finally
            {
                _useDefaultSetTrue = false;
            }
            // End TT#1674
		}

		private void chkUseDefault_Click(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				if (chkUseDefault.Checked)
				{
					// If rows exist in the grid for this set, make sure the user is willing to 
					// overlay them with the defualt basus data.
					if ((_defaultFunctionType == (int)eGroupLevelFunctionType.PercentContribution
						&& this.grdPctContBasis.Rows.Count > 0) ||
						(_defaultFunctionType == (int)eGroupLevelFunctionType.TyLyTrend
						&& this.gridTYNodeVersion.Rows.Count > 0))
					{
						DialogResult dr;
                        // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                        //string msg = "Checking the 'Use Default' option will load the basis data of the designated Default set into this set. Do you wish to proceed?";
                        string msg = "Checking the 'Use Default' option will load the data of the designated Default set into this set. Do you wish to proceed?";
                        // End TT#3
						dr=MessageBox.Show(msg, MIDText.GetTextOnly(eMIDTextCode.lbl_Method), MessageBoxButtons.YesNo);
						if (dr==DialogResult.No) 
						{
							chkUseDefault.Checked = false;
							return;
						}
					}
				}

				// If everythings OK, set function type (Store group level) the same as default
			}
			catch(Exception ex)
			{
				HandleException(ex, "chkUseDefault_CheckedChanged");
			}
		}

        // Begin TT#375-MD - JSmith - stock minimum value went to the use default set and it was not there.  Expectation is the use default set would have the stock minimum value
        void chkUseDefault_EnabledChanged(object sender, EventArgs e)
        {
            rdoDefault.Enabled = chkUseDefault.Enabled;
        }
        // End TT#375-MD - JSmith - stock minimum value went to the use default set and it was not there.  Expectation is the use default set would have the stock minimum value

        // Begin TT#2647 - JSmith - Delays in OTS Method
        private void SetFunctionTypeSameAsDefault()
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				// copy default basis and disable grids if 'Use Default' is checked.
				if (_defaultFunctionType == (int)eGroupLevelFunctionType.PercentContribution)
				{
					if (chkUseDefault.Checked)
					{
						CopyPercentContributionBasis((int)cbxStoreGroupLevel.SelectedValue);
                        // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                        CopyMinMax((int)cbxStoreGroupLevel.SelectedValue, true);
                        // End TT#3
					}

				}
				else if (_defaultFunctionType == (int)eGroupLevelFunctionType.TyLyTrend)
				{
					if (chkUseDefault.Checked)
					{
						CopyTYLYTrendBasis((int)cbxStoreGroupLevel.SelectedValue);
                        // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                        CopyMinMax((int)cbxStoreGroupLevel.SelectedValue, true);
                        // End TT#3
					}
				}

                // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                if (chkUseDefault.Checked)
                {
                    int selectedValue = GetSelectedValue();

                    GroupLevelNodeFunction glnf = (GroupLevelNodeFunction)_GLFProfile.Group_Level_Nodes[selectedValue];

                    PopulateStockMinMax(glnf);
                }
                // End TT#3
			}
			catch(Exception ex)
			{
				HandleException(ex, "chkUseDefault_CheckedChanged");
			}
		}

		// Begin Issue 3816 - stodd
		private eGroupLevelSmoothBy GetDefaultSmoothing()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			eGroupLevelSmoothBy smoothBy = eGroupLevelSmoothBy.None;
			// This IS the Default group
			if ((int)cbxStoreGroupLevel.SelectedValue == this._defaultStoreGroupLevelRid)
			{
				smoothBy = (eGroupLevelSmoothBy)((int)cboGLGroupBy.SelectedValue);
			}
			else
			{
				foreach (GroupLevelFunctionProfile glfp in _OTSPlanMethod.GLFProfileList.ArrayList)
				{
					if (glfp.Default_IND)
					{
						smoothBy = glfp.GLSB_ID;
						break;
					}
				}
			}
			return smoothBy;
		}
		// end issue 3816

		/// <summary>
		/// Check and disable plan
		/// </summary>
		private void CheckAndDisablePlan()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			chkPlan.Checked = true;
			chkPlan.Enabled = false;
		}

		/// <summary>
		/// Decide which Basis to load based on the Function Type
		/// </summary>
		/// <param name="FunctionType">eGroupLevelFunctionType enum</param>
		private void LoadBasis(eGroupLevelFunctionType FunctionType)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			switch (FunctionType)
			{
				case eGroupLevelFunctionType.PercentContribution:
					LoadPctContribution();
					break;
				case eGroupLevelFunctionType.TyLyTrend:
					LoadTYLYTrend();
					break;
			}
		}
		#endregion

		#region Validation

		#region Method Validation
		/// <summary>
		/// Validate Mehtod_Name and SG_RID to be added/updated in Method.
		/// Method_Type_ID and User_RID are parameters to open the form.
		/// TODO - Soft Labels
		/// </summary>
		/// <returns>bool - Method Values are valid</returns>
		private void ValidateMethod(ref ArrayList errMethodList)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			//Method_Name
			if (txtName.Text.Trim() == "")
			{
				errMethodList.Add("Invalid Method Name");
			}
			//Store_Group (Store Attribute)
			if (Convert.ToInt32(cboStoreGroups.SelectedValue, CultureInfo.CurrentUICulture) <= 0)
			{
				errMethodList.Add("Invalid Store Attribute");
			}	
		}
		#endregion

		#region OTS Plan Validation

		/// <summary>
		/// Do Existence check for simple validation
		/// todo 'soft labels'
		/// </summary>
		/// <returns></returns>
		private void ValidateOTSPlan(ref ArrayList errOTSPlanList)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				//OTS Plan Merchandise
				int lAddTag = Include.NoRID;

                //Begin Track #5858 - KJohnson - Validating store security only
                if (((MIDTag)(txtOTSHNDesc.Tag)).MIDTagData != null)
                {
                    HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)txtOTSHNDesc.Tag).MIDTagData;
                    lAddTag = hnp.Key;
                }
                else
                {
                    errOTSPlanList.Add("Required field Product Name is invalid.");
                }
                //End Track #5858

				//Plan Version
				if (Convert.ToInt32(cboPlanVers.SelectedValue, CultureInfo.CurrentUICulture) <= 0)
				{
					errOTSPlanList.Add("Invalid Plan Version.");
				}

				//Date Range
				if (midDateRangeSelectorOTSPlan.Tag != null)
					lAddTag = (int)midDateRangeSelectorOTSPlan.Tag;
				else
				{
					errOTSPlanList.Add("Required field Date Range is invalid.");
				}
			
				//Chain Version
				if (Convert.ToInt32(cboChainVers.SelectedValue, CultureInfo.CurrentUICulture) <= 0)
				{
					errOTSPlanList.Add("Invalid Chain Version.");
				}

				//todo - 6-6-03 Vickie said: don't worry about validating the following:
				//Validate ChainVersion Exists (Chain_ForeCast_Week)
				//if not, show warning..don't stop the show.
				//Need to convert CDR_RID to TIME_ID (pk in C_F_W table).

				//return true;
			}
			catch (Exception err)
			{
				HandleException(err);
				//MessageBox.Show("Error in Validate OTS Plan");
			}
		}

		#endregion

		#region Set Method Validation
		
		/// <summary>
		/// Do Existence check for simple validation
		/// todo 'soft labels'
		/// </summary>
		/// <returns></returns>
		private void ValidateSetMethods(ref ArrayList errSetMethodList)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
                // Begin Track #4872 - JSmith - Global/User Attributes
                if (cboStoreGroups.SelectedIndex == Include.Undefined)
                {
                    errSetMethodList.Add("Invalid " + lblAttribute.Text);
                    ErrorProvider.SetError(cboStoreGroups, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                }
                else
                {
                    ErrorProvider.SetError(cboStoreGroups, string.Empty);
                }
                // End Track #4872

				//Store Group Level (Store Set)
				if (Convert.ToInt32(cbxStoreGroupLevel.SelectedValue, CultureInfo.CurrentUICulture) <= 0)
				{
					errSetMethodList.Add("Invalid Store Set");
				}

				//Seasonalize
				if (_GLFProfile.GLFT_ID == eGroupLevelFunctionType.CurrentTrend ||
					_GLFProfile.GLFT_ID == eGroupLevelFunctionType.AverageSales)
				{
					//			if (Convert.ToInt32(midcboSeasonHN.SelectedValue) <= 0)
					//			{
					//				MessageBox.Show("Invalid Seasonalize Product");
					//				return false;
					//			}
					//
					//			if (_glf.Season_HN_RID != Convert.ToInt32(midcboSeasonHN.SelectedValue))
					//				_SetMethodChanges = true;

					//				_glf.Season_HN_RID = Convert.ToInt32(midcboSeasonHN.SelectedValue);

					//				if (chkSeasonalize.Checked)
					//				{
					//					if (_glf.Season_Ind != '1')
					//						_SetMethodChanges = true;
					//					_glf.Season_Ind = '1';
					//				}
					//				else
					//				{
					//					if (_glf.Season_Ind != '0')
					//						_SetMethodChanges = true;
					//					_glf.Season_Ind = '0';
					//				}

				}

				//Function Type
				if (Convert.ToInt32(this.cboFuncType.SelectedValue, CultureInfo.CurrentUICulture) <= 0)
				{
					errSetMethodList.Add("Invalid Function Type");
				}

				//Group By (Smooth By)
				if (Convert.ToInt32(cboGLGroupBy.SelectedValue, CultureInfo.CurrentUICulture) <= 0)
				{
					errSetMethodList.Add("Invalid Group By");
				}
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

		#endregion

		/// <summary>
		/// Do Existence check for simple validation
		/// todo 'soft labels'
		/// </summary>
		/// <returns></returns>
		private void ValidateBasis(ref ArrayList errBasisList)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				DataSet ds = (DataSet)grdPctContBasis.DataSource;
				DataTable dt = ds.Tables[0];

				if (dt.Rows.Count == 0)
				{
					errBasisList.Add("Invalid Basis - Must have at least 1 line item");
				}
			}
			catch (Exception err)
			{
				HandleException(err, "ValidateBasis");
			}
		}

		#endregion

		#region Detect Changes

		#region Method Changes

		//TODO - Method changes seperate from Method PK changes
		//TODO - Method PK changes - cascade in DB.
		private bool MethodChanges(out bool sgChg)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			sgChg = false;
			try
			{
				//Method Name
				if (_OTSPlanMethod.Name != this.txtName.Text)
					return true;

				//Method Description
				if (_OTSPlanMethod.Method_Description != this.txtDesc.Text)
					return true;
			
				//SG_RID
				if (_OTSPlanMethod.SG_RID != Convert.ToInt32(this.cboStoreGroups.SelectedValue, CultureInfo.CurrentUICulture))
				{
					sgChg = true;
					return true;
				}

				//Global and User Radio Buttons
				if (radGlobal.Checked)
				{
					if (_OTSPlanMethod.GlobalUserType != eGlobalUserType.Global)
						return true;
				}
				else
				{
					if (_OTSPlanMethod.GlobalUserType != eGlobalUserType.User)
						return true;
				}

				return false;
			}
			catch
			{
				MessageBox.Show("Error in Method Changes");
				return false;
			}
		}

		#endregion

		#region OTS Plan Changes
		private bool OTSPlanChanges()//9-18 (needs revision)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				//OTS Plan Merchandise
				int lAddTag = 0;
                //Begin Track #5858 - KJohnson - Validating store security only
                if (((MIDTag)(txtOTSHNDesc.Tag)).MIDTagData != null)
                {
                    HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)txtOTSHNDesc.Tag).MIDTagData;
                    lAddTag = hnp.Key;

					if (_OTSPlanMethod.Plan_HN_RID != lAddTag)
						return true;
				}
                //End Track #5858

				//Plan Version
				if (_OTSPlanMethod.Plan_FV_RID != Convert.ToInt32(cboPlanVers.SelectedValue, CultureInfo.CurrentUICulture))
					return true;

				//Date Range
				if (midDateRangeSelectorOTSPlan.Tag != null)
				{
					lAddTag = (int)midDateRangeSelectorOTSPlan.Tag;

					if (_OTSPlanMethod.CDR_RID != lAddTag)
						return true;
				}

				//Chain Version
				if (_OTSPlanMethod.Chain_FV_RID != Convert.ToInt32(cboChainVers.SelectedValue, CultureInfo.CurrentUICulture))
					return true;

				//Balance
				//Sales
				if (chkOTSSales.Checked != _OTSPlanMethod.Bal_Sales_Ind)
					return true;

				//Stock
				if (chkOTSStock.Checked != _OTSPlanMethod.Bal_Stock_Ind)
					return true;

				return false;
			}
			catch(Exception ex)
			{
				HandleException(ex, "OTSPlanChanges");
				return false;
			}
		}
		#endregion

		/// <summary>
		/// Don't allow user to change Method_Name...should be done from explorer
		/// Reason:  This pk cascades thru too many tables that are modified thru this
		/// screen.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tabOTSMethod_SelectedIndexChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			TabPage tp  = this.tabOTSMethod.SelectedTab;

			LoadBasis((eGroupLevelFunctionType)Convert.ToInt32(this.cboFuncType.SelectedValue, CultureInfo.CurrentUICulture));	
		}

		#endregion

		#region Set class values to be saved
	
		#region Set Method Values
		/// <summary>
		/// Set the Method values (currently uses Method class - Maybe will use Method Profile)
		/// </summary>
		/// <returns>bool - Values were able/unable to be set.</returns>
		private void SetMethodValues(eChangeType changeType)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				//Method_Name -- Required
				_OTSPlanMethod.Name = this.txtName.Text;

								
				//Method Description -- Optional
				_OTSPlanMethod.Method_Description = this.txtDesc.Text;

				//Store_Group (Store Attribute) -- Required
				_OTSPlanMethod.SG_RID = Convert.ToInt32(this.cboStoreGroups.SelectedValue, CultureInfo.CurrentUICulture);
			
				//Global vs User Radio Button -- Required (Global Default)
				if (radGlobal.Checked)
					_OTSPlanMethod.User_RID = Include.GetGlobalUserRID();
				else
					_OTSPlanMethod.User_RID = SAB.ClientServerSession.UserRID;

			}
			catch(Exception err)
			{
				HandleException(err);
			}
		}
		#endregion

		#region Set OTS Plan Values
		private void SetOTSPlanValues(eChangeType changeType)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				int lAddTag = Include.NoRID;
				//OTS Plan Merchandise
                //Begin Track #5858 - KJohnson - Validating store security only
                if (((MIDTag)(txtOTSHNDesc.Tag)).MIDTagData != null)
                {
                    HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)txtOTSHNDesc.Tag).MIDTagData;
                    lAddTag = hnp.Key;
                }
                else
                {
                    lAddTag = Include.NoRID;
                }
                //End Track #5858
			
				//OTS_PMF.Plan_HN_RID = lAddTag;
				_OTSPlanMethod.Plan_HN_RID = lAddTag;
				_OTSPlanMethod.Orig_Plan_HN_RID = lAddTag;

				//Plan Version
				_OTSPlanMethod.Plan_FV_RID = (int)cboPlanVers.SelectedValue;

				//Date Range
				lAddTag = (int)midDateRangeSelectorOTSPlan.Tag;
				_OTSPlanMethod.CDR_RID = lAddTag;
			
				//Chain Version
				_OTSPlanMethod.Chain_FV_RID = (int)cboChainVers.SelectedValue;

				//Balance
				_OTSPlanMethod.Bal_Sales_Ind = chkOTSSales.Checked;

                // BEGIN TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 
                _OTSPlanMethod.ApplyTrendOptionsInd = _ApplyTrendOptionsInd;
                _OTSPlanMethod.ApplyTrendOptionsWOSValue = _ApplyTrendOptionsWOSValue; 
                // END TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 

				//Stock
				_OTSPlanMethod.Bal_Stock_Ind = chkOTSStock.Checked;

				//Forecast Model
				if (cboModel.SelectedValue == null)
					_OTSPlanMethod.ForecastModelRid = Include.NoRID;
				else
					_OTSPlanMethod.ForecastModelRid = (int)cboModel.SelectedValue;

				// BEGIN MID Track #4371 - Justin Bolles - Low Level Forecast
				_OTSPlanMethod.HighLevelInd = chkHighLevel.Checked;
				_OTSPlanMethod.LowLevelsInd = chkLowLevels.Checked;
				// END MID Track #4371

				//Add, update, delete, none
				//_OTSPlanMethod.OTSPlan_Method_Change_Type = changeType;
			}
			catch(Exception err)
			{
				HandleException(err);
			}
		}
		#endregion

		#region Set Set Method Values
		private void SetSetMethodValues(eChangeType changeType)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			int defaultSGLRID = 0;
			
			try
			{
				//Set Method CheckBoxes
				//Don't check for default if new Method
				if (_OTSPlanMethod.Method_Change_Type == eChangeType.update)
				{
					if (chkDefault.Checked)
					{
						//Check for existing Default value within Set
						defaultSGLRID = _OTSPlanMethod.GetDefaultGLFRid();
						if (defaultSGLRID == _GLFProfile.Key || defaultSGLRID == Include.NoRID)
						{
							_GLFProfile.Default_IND = true;
						}
						else
						{
							DialogResult dr;
							dr=MessageBox.Show("Set Method Default already set. Click Yes to change this to the Default.  Click No keep current default.",
								MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanMethod), MessageBoxButtons.YesNo);
							if (dr==DialogResult.No) 
							{	//Change Default to false
								_GLFProfile.Default_IND = false;
								chkDefault.Checked = false;
							}
							else if (dr==DialogResult.Yes)
							{
								// Change the original default Group Level funcftion to false
								GroupLevelFunctionProfile prevDefaultGlfp = (GroupLevelFunctionProfile)_GLFProfileList.FindKey(defaultSGLRID);
								prevDefaultGlfp.Default_IND = false;
								// Set this Group Level Fucntion to now be the default
								_GLFProfile.Default_IND = true;
							}
						}
					}
					else
						//_glf.Default_Ind = _False;
						_GLFProfile.Default_IND = false;
				}
				else
				{
                    _GLFProfile.Default_IND = chkDefault.Checked;
				}

				// If this IS the default, the user has changed the function type so we must change the 
				// default function type.
				if (_GLFProfile.Default_IND == true)
				{
					if (cboFuncType.SelectedValue != null)
					{
						_defaultFunctionType = (int)this.cboFuncType.SelectedValue;
					}
				}

				_GLFProfile.Plan_IND = chkPlan.Checked;

                _GLFProfile.Use_Default_IND = chkUseDefault.Checked;

				_GLFProfile.Clear_IND = chkClear.Checked;


				//Seasonalize
				if (_GLFProfile.GLFT_ID == eGroupLevelFunctionType.CurrentTrend ||
					_GLFProfile.GLFT_ID == eGroupLevelFunctionType.AverageSales)
				{


				}
				else
				{
					_GLFProfile.Season_HN_RID = Include.DefaultPlanHnRID;
					_GLFProfile.Season_IND = false;
				}

				//Function Type
				_GLFProfile.GLFT_ID = (eGroupLevelFunctionType)Convert.ToInt32(cboFuncType.SelectedValue, CultureInfo.CurrentUICulture);

				//Group By (Smooth By)
				_GLFProfile.GLSB_ID = (eGroupLevelSmoothBy)Convert.ToInt32(cboGLGroupBy.SelectedValue, CultureInfo.CurrentUICulture);

				_GLFProfile.Filled = true;
				_GLFProfile.GLF_Change_Type = changeType;
				_GLFProfile.LY_Alt_IND = cbxAltLY.Checked;
				_GLFProfile.Trend_Alt_IND = cbxAltTrend.Checked;

				switch (_GLFProfile.GLFT_ID)
				{
					case eGroupLevelFunctionType.PercentContribution:
						SetBasisPercentContribution();
						break;

					case eGroupLevelFunctionType.TyLyTrend:
						SetBasisTYLYTrend();
						break;
				}
                //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
                _GLFProfile.Proj_Curr_Wk_Sales_IND = cbxProjCurrWkSales.Checked;
                //END TT#43 - MD - DOConnell - Projected Sales Enhancement

				// set stock min/max
//				SetStockMinMax();

				if (!_OTSPlanMethod.GLFProfileList.Contains(_GLFProfile.Key))
					_OTSPlanMethod.GLFProfileList.Add(_GLFProfile);
				else
				{
					_OTSPlanMethod.GLFProfileList.Remove(_GLFProfile);
					_OTSPlanMethod.GLFProfileList.Add(_GLFProfile);
				}
			}
			catch(Exception err)
			{
				HandleException(err, "SetSetMethodValues");
			}
		}

		public void SetBasisPercentContribution()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			int index = -1;
			// rebuild grades each time
			_GLFProfile.GroupLevelBasis.Clear();
			foreach(  UltraGridRow gridRow in grdPctContBasis.Rows )
			{
				int sglRid = Convert.ToInt32(gridRow.Cells["SGL_RID"].Value, CultureInfo.CurrentUICulture);

				// Make sure we only update the ones for this group level function
				if (sglRid == _GLFProfile.Key)
				{
					// BEGIN Issue 4818
					GroupLevelBasisProfile glbp = new GroupLevelBasisProfile(index);

					if (gridRow.Cells["HN_RID"].Value == DBNull.Value)
						glbp.Basis_HN_RID = Include.NoRID;
					else
						glbp.Basis_HN_RID = Convert.ToInt32(gridRow.Cells["HN_RID"].Value);
					glbp.Basis_FV_RID = Convert.ToInt32(gridRow.Cells["FV_RID"].Value);
					glbp.Basis_Weight = Convert.ToDouble(gridRow.Cells["WEIGHT"].Value);
					glbp.Basis_CDR_RID = Convert.ToInt32(gridRow.Cells["CDR_RID"].Value);
					glbp.Basis_ExcludeInd = Include.ConvertCharToBool( Convert.ToChar(gridRow.Cells["INC_EXC_IND"].Value) );
					glbp.Basis_TyLyType = (eTyLyType)Convert.ToInt32(gridRow.Cells["TYLY_TYPE_ID"].Value);
					//bpp.Basis_TyLyType = DataCommon.eTyLyType.NonTyLy;
					// Begin Issue 4422 - stodd
					glbp.MerchType = (eMerchandiseType)Convert.ToInt32(gridRow.Cells["MERCH_TYPE"].Value);
					if (gridRow.Cells["MERCH_PH_RID"].Value == DBNull.Value)
					{
						glbp.MerchPhRid = Include.NoRID;
					}
					else
					{
						glbp.MerchPhRid = Convert.ToInt32(gridRow.Cells["MERCH_PH_RID"].Value);
					}
					if (gridRow.Cells["MERCH_PHL_SEQUENCE"].Value == DBNull.Value)
					{
						glbp.MerchPhlSequence = 0;
					}
					else
					{
						glbp.MerchPhlSequence = Convert.ToInt32(gridRow.Cells["MERCH_PHL_SEQUENCE"].Value);
					}
					if (gridRow.Cells["MERCH_OFFSET"].Value == DBNull.Value)
					{
						glbp.MerchOffset = 0;
					}
					else
					{
						glbp.MerchOffset = Convert.ToInt32(gridRow.Cells["MERCH_OFFSET"].Value);
					}
					// End Issue 4422
					_GLFProfile.GroupLevelBasis.Add(glbp);
					// END Issue 4818
					index--;
				}
			}
		}

		public void SetBasisTYLYTrend()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            _dummyKey = -1;
			_GLFProfile.GroupLevelBasis.Clear();
			_GLFProfile.Trend_Caps.Clear();

            SetTYLYTrendBasis(gridTYNodeVersion, eTyLyType.TyLy);
			SetTYLYTrendBasis(gridLYNodeVersion, eTyLyType.AlternateLy);

            //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
            if (!this.cbxProjCurrWkSales.Checked)
            {
                SetTYLYTrendBasis(gridTrendNodeVersion, eTyLyType.AlternateApplyTo);
            }
            else
            {
                SetTYLYTrendBasis(gridTrendNodeVersion, eTyLyType.ProjectCurrWkSales);
            }
            //END TT#43 - MD - DOConnell - Projected Sales Enhancement

            TrendCapsProfile tcp = new TrendCapsProfile(-1);
			//tcp.Key = Convert.ToInt32(gridRow.Cells["SGL_RID"].Value, CultureInfo.CurrentUICulture);
			if (radNone.Checked)
			{	
				tcp.TrendCapID = eTrendCapID.None;
				tcp.TolPct = Include.UndefinedDouble;
				tcp.HighLimit = Include.UndefinedDouble;
				tcp.LowLimit = Include.UndefinedDouble;
			}
			else if (radTolerance.Checked)
			{
				tcp.TrendCapID = eTrendCapID.Tolerance; 
				tcp.TolPct = Convert.ToDouble(txtTolerance.Text, CultureInfo.CurrentUICulture);
				tcp.HighLimit = Include.UndefinedDouble;
				tcp.LowLimit = Include.UndefinedDouble;
			}
			else if (radLimits.Checked)
			{
				tcp.TrendCapID = eTrendCapID.Limits;
				tcp.TolPct = Include.UndefinedDouble;
				if (txtHigh.Text != string.Empty)
					tcp.HighLimit = Convert.ToDouble(txtHigh.Text, CultureInfo.CurrentUICulture);
				else
					tcp.HighLimit = Include.UndefinedDouble;
				if (txtLow.Text != string.Empty)
					tcp.LowLimit = Convert.ToDouble(txtLow.Text, CultureInfo.CurrentUICulture);
				else
					tcp.LowLimit = Include.UndefinedDouble;
			}
			_GLFProfile.Trend_Caps.Add(tcp);
		}


		/// <summary>
		/// copies the data from the StockMinMax grid into the Group Level Node Function list
		/// </summary>
		public void SetStockMinMax()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			int sglRid;
			int boundary;
			int hnRid;
			int min;
			int max;
			GroupLevelNodeFunction glnf = null;

			foreach(UltraGridRow gridRow in this.gridStockMinMax.Rows)
			{
				if (gridRow.ChildBands.Count > 0)
				{
					sglRid = Convert.ToInt32(gridRow.Cells["SGL_RID"].Value, CultureInfo.CurrentUICulture);
					hnRid = Convert.ToInt32(gridRow.Cells["HN_RID"].Value, CultureInfo.CurrentUICulture);
					boundary = Convert.ToInt32(gridRow.Cells["Boundary"].Value, CultureInfo.CurrentUICulture);

					if(glnf == null)
					{
						glnf = (GroupLevelNodeFunction)_GLFProfile.Group_Level_Nodes[hnRid];
						if(glnf == null) return;
						glnf.Stock_MinMax.Clear();
					}

					if (sglRid == glnf.SglRID)
					{
						// Defaults for the grades are stored in the grade rows.
						// so we build a "default' type row for the Stock Min Max information
						StockMinMaxProfile smmp = new StockMinMaxProfile(glnf.Stock_MinMax.MinValue - 1);
						smmp.DateRangeRid = Include.UndefinedCalendarDateRange;
						if (gridRow.Cells["Minimum"].Value == System.DBNull.Value)
							smmp.MinimumStock = (int)Include.UndefinedDouble;
						else
							smmp.MinimumStock = Convert.ToInt32(gridRow.Cells["Minimum"].Value, CultureInfo.CurrentUICulture);
						if (gridRow.Cells["Maximum"].Value == System.DBNull.Value)
							smmp.MaximumStock = (int)Include.UndefinedDouble;
						else
							smmp.MaximumStock = Convert.ToInt32(gridRow.Cells["Maximum"].Value, CultureInfo.CurrentUICulture);
						smmp.StoreGroupLevelRid = sglRid;
						smmp.Boundary = boundary;
						smmp.HN_RID = hnRid;
						if (smmp.MinimumStock != (int)Include.UndefinedDouble ||
							smmp.MaximumStock != (int)Include.UndefinedDouble)
						{
							glnf.Stock_MinMax.Add(smmp);
						}

						// it's the child rows that really contain the entries for each grade and date range
						foreach (UltraGridRow childRow in gridRow.ChildBands[0].Rows)
						{
							sglRid = Convert.ToInt32(childRow.Cells["SGL_RID"].Value, CultureInfo.CurrentUICulture);
							hnRid = Convert.ToInt32(gridRow.Cells["HN_RID"].Value, CultureInfo.CurrentUICulture);

							glnf = (GroupLevelNodeFunction)_GLFProfile.Group_Level_Nodes[hnRid];
							
							boundary = Convert.ToInt32(childRow.Cells["BOUNDARY"].Value, CultureInfo.CurrentUICulture);
							if (childRow.Cells["MIN_STOCK"].Value == System.DBNull.Value)
								min = (int)Include.UndefinedDouble;
							else
								min = Convert.ToInt32(childRow.Cells["MIN_STOCK"].Value, CultureInfo.CurrentUICulture);
							if (childRow.Cells["MAX_STOCK"].Value == System.DBNull.Value)
								max = (int)Include.UndefinedDouble;
							else
								max = Convert.ToInt32(childRow.Cells["MAX_STOCK"].Value, CultureInfo.CurrentUICulture);

							smmp = new StockMinMaxProfile(glnf.Stock_MinMax.MinValue - 1);
							
							if(childRow.Cells["CDR_RID"].Value == System.DBNull.Value)
								smmp.DateRangeRid = (int)Include.UndefinedDouble;
							else
								smmp.DateRangeRid = Convert.ToInt32(childRow.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture);
							
							smmp.MinimumStock = min;
							smmp.MaximumStock = max;
							smmp.StoreGroupLevelRid = sglRid;
							smmp.HN_RID = hnRid;
							smmp.Boundary = boundary;
					
							if (smmp.MinimumStock != (int)Include.UndefinedDouble ||
								smmp.MaximumStock != (int)Include.UndefinedDouble)
							{
								glnf.Stock_MinMax.Add(smmp);
							}
						}
					}
				}	
			}
	
			//_GLFProfile.Trend_Caps.Add(tcp);
		}

		// BEGIN Issue 4818
		public void SetTYLYTrendBasis(UltraGrid aNodeGrid, eTyLyType aTYLYType)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			int sglRid;
			//int index = -1;
			foreach(  UltraGridRow gridRow in aNodeGrid.Rows )
			{
				sglRid = Convert.ToInt32(gridRow.Cells["SGL_RID"].Value, CultureInfo.CurrentUICulture);

				// Make sure we only update the ones for this group level function
				if (sglRid == _GLFProfile.Key)
				{
					GroupLevelBasisProfile glbp = new GroupLevelBasisProfile(_dummyKey);
				
					if (gridRow.Cells["HN_RID"].Value == DBNull.Value)
						glbp.Basis_HN_RID = Include.NoRID;
					else
						glbp.Basis_HN_RID = Convert.ToInt32(gridRow.Cells["HN_RID"].Value);
					glbp.Basis_FV_RID = Convert.ToInt32(gridRow.Cells["FV_RID"].Value);
					glbp.Basis_Weight = Convert.ToDouble(gridRow.Cells["WEIGHT"].Value);
					glbp.Basis_CDR_RID = Convert.ToInt32(gridRow.Cells["CDR_RID"].Value);
					glbp.Basis_ExcludeInd = Include.ConvertCharToBool( Convert.ToChar(gridRow.Cells["INC_EXC_IND"].Value) );
					//bpp.Basis_TyLyType = (eTyLyType)Convert.ToInt32(gridRow.Cells["TYLY_TYPE_ID"].Value);
					glbp.Basis_TyLyType = aTYLYType;
					// Begin Issue 4422 - stodd
					glbp.MerchType = (eMerchandiseType)Convert.ToInt32(gridRow.Cells["MERCH_TYPE"].Value);
					if (gridRow.Cells["MERCH_PH_RID"].Value == DBNull.Value)
					{
						glbp.MerchPhRid = Include.NoRID;
					}
					else
					{
						glbp.MerchPhRid = Convert.ToInt32(gridRow.Cells["MERCH_PH_RID"].Value);
					}
					if (gridRow.Cells["MERCH_PHL_SEQUENCE"].Value == DBNull.Value)
					{
						glbp.MerchPhlSequence = 0;
					}
					else
					{
						glbp.MerchPhlSequence = Convert.ToInt32(gridRow.Cells["MERCH_PHL_SEQUENCE"].Value);
					}
					if (gridRow.Cells["MERCH_OFFSET"].Value == DBNull.Value)
					{
						glbp.MerchOffset = 0;
					}
					else
					{
						glbp.MerchOffset = Convert.ToInt32(gridRow.Cells["MERCH_OFFSET"].Value);
					}
					// End Issue 4422
					_GLFProfile.GroupLevelBasis.Add(glbp);
					
					_dummyKey--;
				}
			}
			// END Issue 4818
		}	
		#endregion


		#endregion

		#endregion

		#region BASIS - % Contribution
		/// <summary>
		/// Creates a list for use on the "Version" column, which is a dropdown.
		/// </summary>
		private void CreateComboLists()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			//Add a list to the grids, and name it "Version".
			grdPctContBasis.DisplayLayout.ValueLists.Add("Version");
			gridTYNodeVersion.DisplayLayout.ValueLists.Add("Version");
			gridLYNodeVersion.DisplayLayout.ValueLists.Add("Version");
			gridTrendNodeVersion.DisplayLayout.ValueLists.Add("Version");

			// BEGIN Issue 4858 stodd 10.30.2007 forecast methods security
			// This sets the grid so when the value of a value list is empty, it shows the text (blank) 
			// instead of displaying the key (-1).
			grdPctContBasis.DisplayLayout.ValueLists["Version"].DisplayStyle = Infragistics.Win.ValueListDisplayStyle.DisplayText;
			gridTYNodeVersion.DisplayLayout.ValueLists["Version"].DisplayStyle = Infragistics.Win.ValueListDisplayStyle.DisplayText;
			gridLYNodeVersion.DisplayLayout.ValueLists["Version"].DisplayStyle = Infragistics.Win.ValueListDisplayStyle.DisplayText;
			gridTrendNodeVersion.DisplayLayout.ValueLists["Version"].DisplayStyle = Infragistics.Win.ValueListDisplayStyle.DisplayText;
			// END Issue 4858 stodd 10.30.2007 forecast methods security

		
			//Loop through the user version list and manually add value and text to the lists.
			// BEGIN Issue 4858 stodd 10.30.2007 forecast methods security
			ProfileList versionProfList = base.GetForecastVersionList(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Store);	// Track #5871
			// END Issue 4858 stodd 10.30.2007 forecast methods security
			for (int i = 0; i < versionProfList.Count; i++)
			{
				VersionProfile vp = (VersionProfile)versionProfList[i];

                //****Must Declair vli1, vli2, vli3, vli4 below or else "BIG BUG" happens.*******

				Infragistics.Win.ValueListItem vli1 = new Infragistics.Win.ValueListItem();
				vli1.DataValue= vp.Key;
				vli1.DisplayText = vp.Description;
				grdPctContBasis.DisplayLayout.ValueLists["Version"].ValueListItems.Add(vli1);

                Infragistics.Win.ValueListItem vli2 = new Infragistics.Win.ValueListItem();
                vli2.DataValue = vp.Key;
                vli2.DisplayText = vp.Description;
				gridTYNodeVersion.DisplayLayout.ValueLists["Version"].ValueListItems.Add(vli2);

                Infragistics.Win.ValueListItem vli3 = new Infragistics.Win.ValueListItem();
                vli3.DataValue = vp.Key;
                vli3.DisplayText = vp.Description;
                gridLYNodeVersion.DisplayLayout.ValueLists["Version"].ValueListItems.Add(vli3);

                Infragistics.Win.ValueListItem vli4 = new Infragistics.Win.ValueListItem();
                vli4.DataValue = vp.Key;
                vli4.DisplayText = vp.Description;
                gridTrendNodeVersion.DisplayLayout.ValueLists["Version"].ValueListItems.Add(vli4);

                vli1.Dispose();
                vli2.Dispose();
                vli3.Dispose();
                vli4.Dispose();
			}
		}
		private void PopulateBasisMerchandiseValueList()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			//int r = this.MerchandiseDataTable.Rows.Count;
			RemoveBasisMerchandiseValueList();
			LowLevelCombo llc = (LowLevelCombo)cboLowLevels.SelectedItem;

			Infragistics.Win.ValueListItem vli = new Infragistics.Win.ValueListItem();
			vli.DataValue = 0;
			vli.DisplayText = llc.ToString();
			vli.Tag = 0;
			// Add the new value to the list.
			grdPctContBasis.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Add(vli);
			vli = new Infragistics.Win.ValueListItem();
			vli.DataValue = 0;
			vli.DisplayText = llc.ToString();
			vli.Tag = 0;
			gridTYNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Add(vli);
			vli = new Infragistics.Win.ValueListItem();
			vli.DataValue = 0;
			vli.DisplayText = llc.ToString();
			vli.Tag = 0;
			gridLYNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Add(vli);
			vli = new Infragistics.Win.ValueListItem();
			vli.DataValue = 0;
			vli.DisplayText = llc.ToString();
			vli.Tag = 0;
			gridTrendNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Add(vli);
			// Reconnect the list to the Merchandise column

			if (grdPctContBasis.DisplayLayout.Bands[0].Columns.Exists("Merchandise"))
			{
				grdPctContBasis.DisplayLayout.Bands[0].Columns["Merchandise"].ValueList = grdPctContBasis.DisplayLayout.ValueLists["Merchandise"];
				gridTYNodeVersion.DisplayLayout.Bands[0].Columns["Merchandise"].ValueList = gridTYNodeVersion.DisplayLayout.ValueLists["Merchandise"];
				gridLYNodeVersion.DisplayLayout.Bands[0].Columns["Merchandise"].ValueList = gridLYNodeVersion.DisplayLayout.ValueLists["Merchandise"];
				gridTrendNodeVersion.DisplayLayout.Bands[0].Columns["Merchandise"].ValueList = gridTrendNodeVersion.DisplayLayout.ValueLists["Merchandise"];
			}
		}

		private void RemoveBasisMerchandiseValueList()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			int index = grdPctContBasis.DisplayLayout.ValueLists.IndexOf("Merchandise");
			if (index > -1)
				grdPctContBasis.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Clear();
			index = gridTYNodeVersion.DisplayLayout.ValueLists.IndexOf("Merchandise");
			if (index > -1)
				gridTYNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Clear();
			index = gridLYNodeVersion.DisplayLayout.ValueLists.IndexOf("Merchandise");
			if (index > -1)
				gridLYNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Clear();
			index = gridTrendNodeVersion.DisplayLayout.ValueLists.IndexOf("Merchandise");
			if (index > -1)
				gridTrendNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Clear();
		}

		/// <summary>
		/// Creates a dataset for use as the % contr grid's data source.
		/// </summary>
		/// <returns>DataSet</returns>
        // Begin TT#1044 - JSmith - Apply Trend to TAb is taking an unusual amount of time to apply a basis- this iwill be a problem for ANF
        //private DataTable GetDataSourceGroupLevelBasis()	// Issue 4818
        private DataTable GetDataSourceGroupLevelBasis(eTyLyType tyLyType)	// Issue 4818
        // End TT#1044
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			//*****************************************************************************
			// This gets ALL of the basis info (both basis plan AND basis range) for the OTS plan method.  
			// The list is then 
			// filtered for each Store group level (attr set)
			//*****************************************************************************

			// BEGIN Issue 4818
            // Begin TT#1044 - JSmith - Apply Trend to TAb is taking an unusual amount of time to apply a basis- this iwill be a problem for ANF
            //DataTable dt = _OTSPlanMethod.GetGroupLevelBasis(_OTSPlanMethod.Key);
            DataTable dt = _OTSPlanMethod.GetGroupLevelBasis(_OTSPlanMethod.Key, tyLyType);
            // End TT#1044
			dt.TableName = "GroupLevelBasisTable";
			dt.Columns["BASIS_SEQ"].ColumnName = "SEQ";	

			dt.Columns.Add("Merchandise");
			dt.Columns.Add("Version");
			dt.Columns.Add("DateRange");
			dt.Columns.Add("Picture");
			dt.Columns.Add("IncludeButton");
			// END Issue 4818

			dt.AcceptChanges();

			return dt;
		}

		/// <summary>
		/// creates a dataset with the grades as the parent and any previously defined
		/// stock min and max records as children.
		/// </summary>
		/// <returns></returns>
		private DataSet GetDataSourceStockMinMax(GroupLevelNodeFunction GLNF)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			ArrayList delRows = new ArrayList();
//			ArrayList delProfiles = new ArrayList();

			DataTable dt = MIDEnvironment.CreateDataTable("StockMinMax");
			dt.Columns.Add("METHOD_RID", typeof(int));
			dt.Columns.Add("SGL_RID", typeof(int));
			dt.Columns.Add("BOUNDARY", typeof(int));
			dt.Columns.Add("HN_RID", typeof(int));
			dt.Columns.Add("CDR_RID", typeof(int));
			dt.Columns.Add("MIN_STOCK", typeof(int));
			dt.Columns.Add("MAX_STOCK", typeof(int));
			dt.Columns.Add("StoreGrade", typeof(string));
			dt.Columns.Add("DateRange", typeof(string));
			dt.Columns.Add("Picture");

			dt.AcceptChanges();

			foreach(StockMinMaxProfile smmp in GLNF.Stock_MinMax)
			{
				DataRow row = dt.NewRow();
				row["StoreGrade"] = "(Default)";
				row["METHOD_RID"] = smmp.MethodRid;
				row["SGL_RID"] = smmp.StoreGroupLevelRid;
				row["BOUNDARY"] = smmp.Boundary;
				row["HN_RID"] = smmp.HN_RID;
				row["CDR_RID"] = smmp.DateRangeRid;
				if(smmp.MinimumStock == -1)
					row["MIN_STOCK"] = DBNull.Value;
				else
					row["MIN_STOCK"] = smmp.MinimumStock.ToString();

				if(smmp.MaximumStock == -1)
					row["MAX_STOCK"] = DBNull.Value;
				else
					row["MAX_STOCK"] = smmp.MaximumStock.ToString();

				dt.Rows.Add(row);

				int dateRange = Convert.ToInt32(row["CDR_RID"],CultureInfo.CurrentUICulture);
				if (dateRange == Include.UndefinedCalendarDateRange)
				{
					delRows.Add(row);
//					delProfiles.Add(smmp);
					int sglRid = Convert.ToInt32(row["SGL_RID"],CultureInfo.CurrentUICulture);
					int hnRid = Convert.ToInt32(row["HN_RID"], CultureInfo.CurrentUICulture);
					int boundary = Convert.ToInt32(row["BOUNDARY"],CultureInfo.CurrentUICulture);
					foreach (DataRow gradeRow in _dtStoreGrades.Rows)
					{
						int gradeBoundary = Convert.ToInt32(gradeRow["Boundary"],CultureInfo.CurrentUICulture);
						int gradeSglRid = Convert.ToInt32(gradeRow["SGL_RID"],CultureInfo.CurrentUICulture);
						int gradeHnRid = Convert.ToInt32(gradeRow["HN_RID"],CultureInfo.CurrentUICulture);

						if (boundary == gradeBoundary && sglRid == gradeSglRid && gradeHnRid == hnRid)
						{
							gradeRow["Minimum"] = row["MIN_STOCK"];
							gradeRow["Maximum"] = row["MAX_STOCK"];
						}
					}
				}
			}

			foreach(DataRow row in delRows)
			{
				dt.Rows.Remove(row);
			}
			
//			foreach (StockMinMaxProfile smmp in delProfiles)
//			{
//				GLNF.Stock_MinMax.Remove(smmp);
//			}

			dt.AcceptChanges();
			_dtStoreGrades.AcceptChanges();

			DataSet ds = MIDEnvironment.CreateDataSet("stock Min Max");
			ds.Tables.Add(this._dtStoreGrades);
			ds.Tables.Add(dt);
			
			DataColumn [] parentColumns = {ds.Tables["Store Grade"].Columns["Boundary"],
											  ds.Tables["Store Grade"].Columns["SGL_RID"],
											  ds.Tables["Store Grade"].Columns["HN_RID"]};
			DataColumn [] childColumns = {ds.Tables["StockMinMax"].Columns["BOUNDARY"],
											 ds.Tables["StockMinMax"].Columns["SGL_RID"],
											 ds.Tables["StockMinMax"].Columns["HN_RID"]};

			ds.Relations.Add("Grades", parentColumns, childColumns,false);

			//ds.DefaultViewManager.DataViewSettings["Store Grade"].RowFilter = "SGL_RID = " +  this.cboStoreGroupLevel.SelectedValue.ToString();
			//ds.DefaultViewManager.DataViewSettings["StockMinMax"].RowFilter = "SGL_RID = " +  this.cboStoreGroupLevel.SelectedValue.ToString();

			//			ds.Relations.Add("Grades",
			//				ds.Tables["Store Grade"].Columns["Boundary"],
			//				ds.Tables["StockMinMax"].Columns["BOUNDARY"], false);
			//			ds.Relations.Add("SGL",
			//				ds.Tables["Store Grade"].Columns["SGL_RID"],
			//				ds.Tables["StockMinMax"].Columns["SGL_RID"], false);


			return ds;
		}

		private DataTable BuildStockMinMaxDBTable(GroupLevelNodeFunction GLNF)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			//*****************************************************************************
			// This gets ALL of the basis info (both basis plan AND basis range) for the OTS plan method.  
			// The list is then 
			// filtered for each Store group level (attr set)
			//*****************************************************************************

            //Begin Track #5858 - KJohnson - Validating store security only
            //_storeGradeList = SAB.HierarchyServerSession.GetStoreGradeList((int)txtOTSHNDesc.Tag, false, true);
            HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)txtOTSHNDesc.Tag).MIDTagData;
            _storeGradeList = SAB.HierarchyServerSession.GetStoreGradeList(hnp.Key, false, true);
            //End Track #5858

			_dtStoreGrades = MIDEnvironment.CreateDataTable("Store Grade");
			_dtStoreGrades.Columns.Add("SGL_RID", typeof(int));
			_dtStoreGrades.Columns.Add("HN_RID", typeof(int));
			_dtStoreGrades.Columns.Add("StoreGrade", typeof(string));
			_dtStoreGrades.Columns.Add("Boundary", typeof(int));
			_dtStoreGrades.Columns.Add("Picture");
			_dtStoreGrades.Columns.Add("DateRange", typeof(string));
			_dtStoreGrades.Columns.Add("Minimum", typeof(int));
			_dtStoreGrades.Columns.Add("Maximum", typeof(int));

			StoreGroupListViewProfile sg = (StoreGroupListViewProfile)cboStoreGroups.SelectedItem;
            ProfileList sgll = StoreMgmt.StoreGroup_GetLevelListViewList(sg.Key); //SAB.StoreServerSession.GetStoreGroupLevelListViewList(sg.Key);

			// Fill in rest of the grades
			foreach (StoreGroupLevelListViewProfile sglp in sgll.ArrayList)
			{
				if(sglp.Key != GLNF.SglRID) continue;

				// Add DEFAULT 
				DataRow newRow = _dtStoreGrades.NewRow();
				newRow["Boundary"] = -1;
				newRow["StoreGrade"] = "Default";
				newRow["DateRange"] = "(Default)";
				newRow["SGL_RID"] = sglp.Key;
				newRow["HN_RID"] = GLNF.HN_RID;

				_dtStoreGrades.Rows.Add(newRow);

				foreach(StoreGradeProfile sgp in _storeGradeList.ArrayList)
				{
					newRow = _dtStoreGrades.NewRow();
					newRow["Boundary"] = sgp.Key;
					newRow["StoreGrade"] = sgp.StoreGrade;
					newRow["DateRange"] = "(Default)";
					newRow["SGL_RID"] = sglp.Key;
					newRow["HN_RID"] = GLNF.HN_RID;
					_dtStoreGrades.Rows.Add(newRow);
				}
			}

			_dtStoreGrades.AcceptChanges();

			return _dtStoreGrades;
		}

		#region Grid Events (mainly formatting stuff)
		private void grdPctContBasis_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				//The following information pertains to the formatting of the grid.

				//NOTE: Bands[0] refers to the "Basis" grid on the form.

				//				grdPctContBasis.SupportThemes = false;
				//grdPctContBasis.DisplayLayout.AutoFitColumns = true;

				// BEGIN MID Track #3792 - replace obsolete method 
				//e.Layout.AutoFitColumns = true;
				//e.Layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
				// END MID Track #3792

				//hide the key columns.
				e.Layout.Bands[0].Columns["METHOD_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["SGL_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["HN_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["FV_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["CDR_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["INC_EXC_IND"].Hidden = true;
				// Issue # 4422
				e.Layout.Bands[0].Columns["MERCH_TYPE"].Hidden = true;
				e.Layout.Bands[0].Columns["MERCH_PH_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["MERCH_PHL_SEQUENCE"].Hidden = true;
				e.Layout.Bands[0].Columns["MERCH_OFFSET"].Hidden = true;
				// End Issue # 4422
				e.Layout.Bands[0].Columns["SEQ"].Hidden = true;
				e.Layout.Bands[0].Columns["Picture"].Hidden = true;
				e.Layout.Bands[0].Columns["TYLY_TYPE_ID"].Hidden = true;
			
				//Prevent the user from re-arranging columns.
				grdPctContBasis.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;

				if (FunctionSecurity.AllowUpdate)
				{
					grdPctContBasis.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
					grdPctContBasis.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
				}
				else
				{
					grdPctContBasis.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
					grdPctContBasis.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
				}

				//Set the header captions.
				grdPctContBasis.DisplayLayout.Bands[0].Columns["Merchandise"].Header.VisiblePosition = 1;
				grdPctContBasis.DisplayLayout.Bands[0].Columns["Merchandise"].Header.Caption = "Merchandise";
				grdPctContBasis.DisplayLayout.Bands[0].Columns["Merchandise"].Width = 200;	//Track #5955 stodd
				grdPctContBasis.DisplayLayout.Bands[0].Columns["Version"].Header.VisiblePosition = 2;
				grdPctContBasis.DisplayLayout.Bands[0].Columns["Version"].Header.Caption = "Version";
				grdPctContBasis.DisplayLayout.Bands[0].Columns["Version"].Width = 90;	//Track #5955 stodd
				grdPctContBasis.DisplayLayout.Bands[0].Columns["DateRange"].Header.VisiblePosition = 3;
				grdPctContBasis.DisplayLayout.Bands[0].Columns["DateRange"].Header.Caption = "Date Range";
				grdPctContBasis.DisplayLayout.Bands[0].Columns["WEIGHT"].Header.VisiblePosition = 5;
				grdPctContBasis.DisplayLayout.Bands[0].Columns["WEIGHT"].Header.Caption = "Weight";
				grdPctContBasis.DisplayLayout.Bands[0].Columns["WEIGHT"].Width = 45; 	//Track #5955 stodd	
				grdPctContBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Header.VisiblePosition = 6;
				grdPctContBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Header.Caption = " ";

				if (chkLowLevels.Checked)
					grdPctContBasis.DisplayLayout.Bands[0].Columns["Merchandise"].ValueList = grdPctContBasis.DisplayLayout.ValueLists["Merchandise"];

				//Make some columns readonly.
				e.Layout.Bands[0].Columns["Picture"].CellActivation = Activation.NoEdit;
		
				//make the "Version" column a drop down list.
				grdPctContBasis.DisplayLayout.Bands[0].Columns["Version"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				grdPctContBasis.DisplayLayout.Bands[0].Columns["Version"].ValueList = grdPctContBasis.DisplayLayout.ValueLists["Version"];
		
				//the "IncludeButton" column (two spaces) is the column that contains buttons
				//to include/exclude a basis detail. 
				grdPctContBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
				grdPctContBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].CellAppearance.ImageVAlign = Infragistics.Win.VAlign.Middle;
				grdPctContBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Width = 20;
				grdPctContBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].CellActivation = Activation.NoEdit;
				// Begin Track #5955 stodd
				grdPctContBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
				grdPctContBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].ButtonDisplayStyle = ButtonDisplayStyle.Always;
				grdPctContBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].CellButtonAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
				grdPctContBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].CellButtonAppearance.ImageVAlign = Infragistics.Win.VAlign.Middle;
				// End Track #5955 stodd

				//Make the "INC_EXC_IND" column a checkbox column.
				grdPctContBasis.DisplayLayout.Bands[0].Columns["INC_EXC_IND"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
		
				//Set the width of the "DateRange" column so that the DateRangeSelector can fit.
				grdPctContBasis.DisplayLayout.Bands[0].Columns["DateRange"].Width = 210;
				grdPctContBasis.DisplayLayout.Bands[0].Columns["DateRange"].Style  = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
				e.Layout.Bands[0].Columns["DateRange"].CellActivation = Activation.NoEdit;

				//the following code tweaks the "Add New" buttons (which come with the grid).
				grdPctContBasis.DisplayLayout.Bands[0].AddButtonCaption = "Add New Basis Details";
				grdPctContBasis.DisplayLayout.Bands[0].AddButtonToolTipText = "Click to add new basis details.";
				grdPctContBasis.DisplayLayout.AddNewBox.Hidden = false;
				grdPctContBasis.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;
			}
			catch(Exception err)
			{
				HandleException(err);
			}
		}

		private void grdPctContBasis_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			//Set the width of the "DateRange" column so that the DateRangeSelector can fit.
			//grdPctContBasis.DisplayLayout.Bands[0].Columns["DateRange"].Width = 128;	// Track 5955

			//Based on indicator, add picInclude or not
			if (e.Row.Band == grdPctContBasis.DisplayLayout.Bands[0] &&
				//Convert.ToBoolean(Convert.ToInt32(e.Row.Cells["INC_EXC_IND"].Value)) == true)
				(e.Row.Cells["INC_EXC_IND"].Value.ToString() == "0"))
			{
				e.Row.Cells["IncludeButton"].Appearance.Image = picInclude;
				e.Row.Cells["IncludeButton"].ButtonAppearance.Image = picInclude;	// Track #5955 stodd
			}
			else if (e.Row.Band == grdPctContBasis.DisplayLayout.Bands[0] &&
				//Convert.ToBoolean(e.Row.Cells["INC_EXC_IND"].Value) == false)
				(e.Row.Cells["INC_EXC_IND"].Value.ToString() != "0"))
			{
				e.Row.Cells["IncludeButton"].Appearance.Image = picExclude;
				e.Row.Cells["IncludeButton"].ButtonAppearance.Image = picExclude;	// Track #5955 stodd
			}
 
			//Populate cell w/text description of Date Range
			if (e.Row.Cells["CDR_RID"].Value.ToString() != "")
			{
				DateRangeProfile dr;
				if (midDateRangeSelectorOTSPlan.Tag != null)
					dr = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(e.Row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture),(int)midDateRangeSelectorOTSPlan.Tag);
				else
					dr = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(e.Row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture));

				e.Row.Cells["DateRange"].Value = dr.DisplayDate;
				e.Row.Cells["CDR_RID"].Value = dr.Key;

				if (dr.DateRangeType == eCalendarRangeType.Dynamic)
				{
					switch (dr.RelativeTo)
					{
						case eDateRangeRelativeTo.Current:
							e.Row.Cells["DateRange"].Appearance.Image = this.DynamicToCurrentImage;
							break;
						case eDateRangeRelativeTo.Plan:
							e.Row.Cells["DateRange"].Appearance.Image = this.DynamicToPlanImage;
							break;
						default:
							e.Row.Cells["DateRange"].Appearance.Image = null;
							break;
					}
				}
			}

			//Populate cell w/text description of Hierarchy Node
			if (e.Row.Cells["HN_RID"].Value.ToString() != string.Empty)
			{
				// Begin Issue 4422 - stodd
				HierarchyNodeProfile hnp = new HierarchyNodeProfile(Include.NoRID);
				int nodeKey = Convert.ToInt32(e.Row.Cells["HN_RID"].Value, CultureInfo.CurrentUICulture);
				if (nodeKey == 0)  // Means node is in value list drop down
				{
					_merchValChanged = true;
					// End MID Issue 3443/3494
					e.Row.Cells["Merchandise"].Value = 0;
					// Begin MID Issue 3443/3494 stodd
					_merchValChanged = false;
				}
				else if (nodeKey != Include.NoRID)
				{
					//Begin Track #5378 - color and size not qualified
//					hnp = SAB.HierarchyServerSession.GetNodeData(nodeKey);
					
                    // Begin TT#2647 - JSmith - Delays in OTS Method
                    //hnp = SAB.HierarchyServerSession.GetNodeData(nodeKey, true, true);
                    if (!_nodesByRID.TryGetValue(nodeKey, out hnp))
                    {
                        hnp = SAB.HierarchyServerSession.GetNodeData(nodeKey, true, true);
                        _nodesByRID.Add(nodeKey, hnp);
                        if (!_nodesByID.ContainsKey(hnp.NodeID))
                        {
                            _nodesByID.Add(hnp.NodeID, hnp);
                        }
                    }
                    // End TT#2647 - JSmith - Delays in OTS Method
					//End Track #5378
					//				e.Row.Cells["Merchandise"].Value = hnp.NodeID;
					// Begin MID Issue 3443/3494 stodd
					_merchValChanged = true;
					// End MID Issue 3443/3494
					e.Row.Cells["Merchandise"].Value = hnp.Text;
					// Begin MID Issue 3443/3494 stodd
					_merchValChanged = false;
					// End MID Issue 3443/3494
				}
				// End Issue 4422
			}

			//Populate cell w/text description of Forecast Version
			if (e.Row.Cells["FV_RID"].Value.ToString() != string.Empty)
			{
				DataTable dt;
				ForecastVersion fv = new ForecastVersion();
				dt = fv.GetForecastVersions();
				dt.PrimaryKey = new DataColumn[] {dt.Columns["FV_RID"]};
				DataRow row = dt.Rows.Find(e.Row.Cells["FV_RID"].Value);

				if (row != null)
					e.Row.Cells["Version"].Value = row["DESCRIPTION"]; 
			}
		}

		private void grdPctContBasis_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			// we can use the same logic the TY/LY grids use...
			AllGrids_ClickCellButton(e, "CDR_RID", false, true);
		}

		private void grdPctContBasis_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			//if the user clicked the right mouse, he/she probably wants to see the
			//context menu. We have only one item in the context menu: the "DELETE"
			//command. In order to delete a row, we must select the whole row first.

			if (e.Button == MouseButtons.Left) return;

			//get the row the mouse is on.
			//Get the GUI element where the mouse cursor is. (so that later on
			//we can retrieve the row and the cell based on the mouse location.)
			Infragistics.Win.UIElement aUIElement;
			Point pt = new Point(e.X, e.Y);
			aUIElement = grdPctContBasis.DisplayLayout.UIElement.ElementFromPoint(pt);

			if (aUIElement == null) 
			{					
				return;
			}

			//Retrieve the row where the mouse is.
			UltraGridRow aRow;
			aRow = (UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow)); 

			if (aRow == null) 
			{
				return;
			}
			
			aRow.Selected = true;
		}

		private void grdPctContBasis_BeforeCellActivate(object sender, Infragistics.Win.UltraWinGrid.CancelableCellEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		private void grdPctContBasis_BeforeCellDeactivate(object sender, System.ComponentModel.CancelEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			//this.DateRangeSelector.Visible = false;
			this.btnIncExc.Visible = false;
		}

		private void grdPctContBasis_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (CheckInsertCondition(grdPctContBasis, e) == false)
			{
				e.Cancel = true;
				return;
			}

			try
			{
				this.Cursor = Cursors.WaitCursor;

				// BEGIN MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
				int sglRid = Convert.ToInt32(this.cbxStoreGroupLevel.SelectedValue,CultureInfo.CurrentUICulture);
				// END MID Track #5954

				//Handle the row Sequence (pk for the grid)
				DataRow basisRow = _dtSource.NewRow();

				int seq=1;
				if (_dtSource.Rows.Count == 0)
				{
					basisRow["SEQ"] = 1;
				}
				else
				{
					// BEGIN MID ISSUE #2833 - stodd
					int lastIndex = _dtSource.Rows.Count-1;
					for (int i=lastIndex;i>-1;i--)
					{
						DataRow lastRow = _dtSource.Rows[i];
						if (lastRow.RowState != DataRowState.Deleted)
						{
							// BEGIN MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
							if ((int)lastRow["SGL_RID"] == sglRid)
							{
								if (lastRow["SEQ"] == DBNull.Value)
									seq = 1;
								else
									seq = Convert.ToInt32(lastRow["SEQ"], CultureInfo.CurrentUICulture) + 1;
								break;
							}
							// END MID Track #5954
						}
					}

					basisRow["SEQ"] = seq;
					// END MID ISSUE #2833 - stodd
				}
				
				basisRow["INC_EXC_IND"] = "0";
				basisRow["WEIGHT"] = 1;
				basisRow["SGL_RID"] = sglRid; //MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
				basisRow["TYLY_TYPE_ID"] = (int)eTyLyType.NonTyLy;

				_dtSource.Rows.Add(basisRow);

				//_dsSource.Tables["BasisTable"].Rows.Add(basisRow);

				//Set the active row to this newly added Basis row.
				grdPctContBasis.ActiveRow = grdPctContBasis.Rows[grdPctContBasis.Rows.Count - 1];

				//Since we've already added the necessary information in the underlying
				//datatable, we want to cancel out because if we don't, the grid will
				//add another blank row (in addition to the row we just added to the datatable).
				e.Cancel = true;

				this.Cursor = Cursors.Default;
			}
			catch
			{}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}
		/// <summary>
		/// checks to make sure that the previous row, if there is one, is completely
		/// filled out. If any information is missing, return false to the calling
		/// procedure to indicate that it should not proceed adding another row.
		/// </summary>
		/// <returns></returns>
		private bool CheckInsertCondition(UltraGrid aGrid, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (aGrid.Rows.Count > 0)
			{
				//Find the last Details row and check its values. 			
				UltraGridRow aRow = aGrid.Rows[aGrid.Rows.Count-1];

				if (aRow.Cells["Merchandise"].Value.ToString() == string.Empty)
				{
					MessageBox.Show("You must finish filling out the previous row.\r\nData Missing: Merchandise.", "Error");
					return false;
				}
				if (aRow.Cells["Version"].Value.ToString() == string.Empty)
				{
					MessageBox.Show("You must finish filling out the previous row.\r\nData Missing: Version.", "Error");
					return false;
				}
				if (aRow.Cells["DateRange"].Value.ToString() == "")
				{
					MessageBox.Show("You must finish filling out the previous row.\r\nData Missing: Date Range.", "Error");
					return false;
				}
			}
			return true;
		}

		//private void grdPctContBasis_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		//{
		//    try
		//    {
		//        //Hide the following controls by default. If (and only if) we need to 
		//        //show them, we'll have code later in this section to do that.
		//        //DateRangeSelector.Visible = false;
		//        btnIncExc.Visible = false;

		//        //Get the GUI element where the mouse cursor is. (so that later on
		//        //we can retrieve the row and the cell based on the mouse location.)
		//        Infragistics.Win.UIElement aUIElement;
		//        Point pt = new Point(e.X, e.Y);
		//        aUIElement = grdPctContBasis.DisplayLayout.UIElement.ElementFromPoint(pt);

		//        if (aUIElement == null) 
		//        {					
		//            return;
		//        }

		//        //Retrieve the row where the mouse is.
		//        UltraGridRow aRow;
		//        aRow = (UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow)); 

		//        if (aRow == null) 
		//        {
		//            return;
		//        }

		//        //Retrieve the cell where the mouse is.
		//        UltraGridCell aCell = (UltraGridCell)aUIElement.GetContext(typeof(UltraGridCell)); 
		//        if (aCell == null) 
		//        {
		//            return;
		//        }

		//        //If the cell is the "DateRange" cell or the include/exclude button cell,
		//        //put a DateRangeSelector or a button in it.
		//        //				if (aCell == aRow.Cells["DateRange"])
		//        //				{
		//        //					m_aRow = aRow; //m_aRow is a form-level variable and the DateRangeSelector's click event needs it.
		//        //
		//        //					//We need to get the size and location of the cell, and we can only
		//        //					//get that by retrieving the UIElement associated with that cell.
		//        //					CellUIElement objCellUIElement = (CellUIElement)aCell.GetUIElement(this.grdPctContBasis.ActiveRowScrollRegion, this.grdPctContBasis.ActiveColScrollRegion);
		//        //					if ( objCellUIElement == null ) { return; }
		//        //
		//        //					//   Get the size and location of the cell
		//        //					int left = objCellUIElement.RectInsideBorders.Location.X + this.grdPctContBasis.Location.X;
		//        //					int top = objCellUIElement.RectInsideBorders.Location.Y + this.grdPctContBasis.Location.Y;
		//        //					int width = objCellUIElement.RectInsideBorders.Width;
		//        //					int height = objCellUIElement.RectInsideBorders.Height;
		//        //   		
		//        //					//   Set the DateRangeSelector's size and location equal to the cell's size and location
		//        //					this.DateRangeSelector.SetBounds(left, top, width, height);
		//        //      
		//        //					//   Show the combobox control over the cell, and give it focus
		//        //					DateRangeSelector.Visible = true;
		//        //					DateRangeSelector.Focus();
		//        //					DateRangeSelector.BringToFront();
		//        //					DateRangeSelector.Text = aCell.Value.ToString();
		//        //				}


		//        //if (aCell == aRow.Cells["IncludeButton"])
		//        //{
		//        //    m_aRow = aRow; //m_aRow is a form-level variable and the Include column (which is hidden) needs it.

		//        //    //We need to get the size and location of the cell, and we can only
		//        //    //get that by retrieving the UIElement associated with that cell.
		//        //    CellUIElement objCellUIElement = (CellUIElement)aCell.GetUIElement(this.grdPctContBasis.ActiveRowScrollRegion, this.grdPctContBasis.ActiveColScrollRegion);
		//        //    if ( objCellUIElement == null ) { return; }

		//        //    //   Get the size and location of the cell
		//        //    int left = objCellUIElement.RectInsideBorders.Location.X + this.grdPctContBasis.Location.X;
		//        //    int top = objCellUIElement.RectInsideBorders.Location.Y + this.grdPctContBasis.Location.Y;
		//        //    int width = objCellUIElement.RectInsideBorders.Width;
		//        //    int height = objCellUIElement.RectInsideBorders.Height;
	
		//        //    //   Set the combobox's size and location equal to the cell's size and location
		//        //    this.btnIncExc.SetBounds(left, top, width, height);

		//        //    m_includeCell = aRow.Cells["INC_EXC_IND"];					

		//        //    if (m_includeCell.Value.ToString() == "0")
		//        //        //if (Convert.ToBoolean(m_includeCell.Value) == true)
		//        //    {
		//        //        this.btnIncExc.Image = picInclude;
		//        //    }
		//        //    else if (m_includeCell.Value.ToString() == "1")
		//        //        //else if (Convert.ToBoolean(m_includeCell.Value) == false)
		//        //    {
		//        //        this.btnIncExc.Image = picExclude;
		//        //    }

		//        //    //   Show the combobox control over the cell, and give it focus
		//        //    btnIncExc.Visible = true;
		//        //    // Begin track #5955 stodd
		//        //    //btnIncExc.Focus();
		//        //    // End track #5955 stodd
		//        //    btnIncExc.BringToFront();
		//        //}
		//    }
		//    catch (Exception ex)
		//    {
		//        HandleException(ex);
		//    }
		//}

		private void grdPctContBasis_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			//if (e.Rows.Length == _dsSource.Tables["BasisTable"].Rows.Count)
			if (e.Rows.Length == _dtSource.Rows.Count)
			{
				//the user is trying to delete ALL the details rows. Prevent it.
				//e.DisplayPromptMsg = false;
				//MessageBox.Show("The delete cannot be performed because at least one Basis must exist.\r\n",
				//	"Error", MessageBoxButtons.OK);
				//e.Cancel = true;
			}																		  
		}


		private void btnIncExc_Click(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (m_includeCell.Value.ToString() == "0")
			//if (Convert.ToBoolean(m_includeCell.Value) == true)
			{
				//It's currently included. So we'll exclude it.
				//m_includeCell.Value = false;
				m_includeCell.Value = "1";

				UltraGridCell picCell = m_aRow.Cells["IncludeButton"];
				picCell.Appearance.Image = picExclude;
				btnIncExc.Image = picExclude;
			}
			else if (m_includeCell.Value.ToString() == "1")
			//else if (Convert.ToBoolean(m_includeCell.Value) == false)
			{
				//It's currently excluded. So we'll include it.
				//m_includeCell.Value = true;
				m_includeCell.Value = "0";

				UltraGridCell picCell = m_aRow.Cells["IncludeButton"];
				picCell.Appearance.Image = picInclude;
				btnIncExc.Image = picInclude;
			}
		}

		private void grdPctContBasis_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (e.Cell == e.Cell.Row.Cells["Merchandise"])
			{
				if (FormLoaded && !_dragAndDrop)
				{
					if (_merchValChanged)
					{
						_merchValChanged = false;
						return;
					}
					
					int rowseq = -1;
					//============================================================================================= 
					// This code is catching where a value is selected from the merchandise drop down as opposed 
					// to one being entered. If the value is not numeric, it looks it up as a product.
					//=============================================================================================
					if (_MerchCellListClose)
					{
						try
						{
							rowseq = Convert.ToInt32(e.Cell.Value, CultureInfo.CurrentUICulture);
						}
						catch (System.FormatException)
						{
							rowseq = -1;
						}
						catch (System.InvalidCastException)
						{
							rowseq = -1;
						}
						finally
						{
							_MerchCellListClose = false;
						}
					}

					if (rowseq == -1)
					{
						string productID = e.Cell.Value.ToString().Trim();
						if (productID.Length > 0)
						{
							_nodeRID = GetNodeText(ref productID);
							if (_nodeRID == Include.NoRID)
							{
								string errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
									productID );
								MessageBox.Show(errorMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);		
							}
							else 
							{
								_merchValChanged = true;
								e.Cell.Value = productID;
								e.Cell.Row.Cells["HN_RID"].Value = _nodeRID;
								e.Cell.Row.Cells["MERCH_TYPE"].Value = eMerchandiseType.Node;
								e.Cell.Row.Cells["MERCH_PH_RID"].Value = Include.NoRID;
								e.Cell.Row.Cells["MERCH_PHL_SEQUENCE"].Value = 0;
								e.Cell.Row.Cells["MERCH_OFFSET"].Value = 0;
							}
						}
					}
					else
					{
						e.Cell.Row.Cells["HN_RID"].Value = 0;
						e.Cell.Row.Cells["MERCH_TYPE"].Value = eMerchandiseType.SameNode;
						e.Cell.Row.Cells["MERCH_PH_RID"].Value = Include.NoRID;
						e.Cell.Row.Cells["MERCH_PHL_SEQUENCE"].Value = 0;
						e.Cell.Row.Cells["MERCH_OFFSET"].Value = 0;

					}

				}
			}
			else if (e.Cell == e.Cell.Row.Cells["Version"])
			{
				int selectedIndex = grdPctContBasis.DisplayLayout.ValueLists["Version"].SelectedIndex;

				if (selectedIndex != -1)
					e.Cell.Row.Cells["FV_RID"].Value = Convert.ToInt32(grdPctContBasis.DisplayLayout.ValueLists["Version"].ValueListItems[selectedIndex].DataValue, CultureInfo.CurrentUICulture);
			}

            // Begin TT#2647 - JSmith - Delays in OTS Method
            //// Begin Issue 3816 - stodd
            //int sgl = (int)cbxStoreGroupLevel.SelectedValue;
            //if (sgl == _defaultStoreGroupLevelRid)
            //    UpdateSetsUsingDefault();
            //// End issue 3816
            // End TT#2647 - JSmith - Delays in OTS Method
		}

		private void grdPctContBasis_AfterRowsDeleted(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			//update row states so that the deleted row really DOES disappear.
			//_dsSource.AcceptChanges();

			//Reassign the SEQuences of each row after delete.
			int seq = 1;
			for (int i = 0; i < _dtSource.Rows.Count; i ++)
			{
				DataRow dr = _dtSource.Rows[i];
				if (dr.RowState != DataRowState.Deleted)
					dr["SEQ"] = seq++;
			}

			// BEGIN Issue 5357 stodd
			_dtSource.AcceptChanges();

            // Begin TT#2647 - JSmith - Delays in OTS Method
            //int sgl = (int)cbxStoreGroupLevel.SelectedValue;
            //if (sgl == _defaultStoreGroupLevelRid)
            //    UpdateSetsUsingDefault();
            // End TT#2647 - JSmith - Delays in OTS Method
			// END Issue 5357 stodd
		}

		private void mnuDelete_Click(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (_gridMouseDownIsFrom.Name == gridStockMinMax.Name)
			{
				if (_gridMouseDownIsFrom.Selected.Rows.Count > 0)
				{
					ChangePending = true;
//					_gridMouseDownIsFrom.BeginUpdate();
//					if (_gridMouseDownIsFrom.ActiveRow.Band.Key == "Store Grade")
//					{
//						_gridMouseDownIsFrom.ActiveRow.Cells["Minimum"].Value = System.DBNull.Value;
//						_gridMouseDownIsFrom.ActiveRow.Cells["Maximum"].Value = System.DBNull.Value;
//					}
//					else
//					{
////						_gridMouseDownIsFrom.ActiveRow.Delete();
//						_gridMouseDownIsFrom.DeleteSelectedRows();
//					}
					ArrayList rowsToDelete = new ArrayList();
					foreach (UltraGridRow gridRow in _gridMouseDownIsFrom.Selected.Rows)
					{
						if (gridRow.Band.Key == "Store Grade")
						{
							gridRow.Cells["Minimum"].Value = System.DBNull.Value;
							gridRow.Cells["Maximum"].Value = System.DBNull.Value;
						}
						else
						{
							rowsToDelete.Add(gridRow);
						}
					}
					foreach (UltraGridRow gridRow in rowsToDelete)
					{
						gridRow.Delete(false);
					}
					_dsStockMinMax.AcceptChanges();
					_gridMouseDownIsFrom.EndUpdate();
				}
			}
			else
			{
				_gridMouseDownIsFrom.DeleteSelectedRows();
			}
		}

		private void mnuInsert_Click(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			ChangePending = true;
			_gridMouseDownIsFrom.DisplayLayout.Bands[0].AddNew();
		}

		private void mnuExpandAll_Click(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			_gridMouseDownIsFrom.Rows.ExpandAll(true);
		}

		private void mnuCollapseAll_Click(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			_gridMouseDownIsFrom.Rows.CollapseAll(true);
		}

		private void mnuDeleteAll_Click(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				string message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDelete);
				message = message.Replace("{0}", MIDText.GetTextOnly(eMIDTextCode.lbl_Stock_MinMax));
				if (MessageBox.Show (message,  this.Text,
					MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
					== DialogResult.Yes)
				{
					
					DeleteAllMinMaxes();
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void DeleteAllMinMaxes()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				ChangePending = true;
				gridStockMinMax.BeginUpdate();
				ArrayList rowsToDelete = new ArrayList();
				foreach (UltraGridRow gridRow in gridStockMinMax.Rows)
				{
					gridRow.Cells["Minimum"].Value = System.DBNull.Value;
					gridRow.Cells["Maximum"].Value = System.DBNull.Value;

					if (gridRow.HasChild())
					{
						foreach (UltraGridRow childRow in gridRow.ChildBands[0].Rows)
						{
							rowsToDelete.Add(childRow);
						}
					}
				}
				foreach (UltraGridRow childRow in rowsToDelete)
				{
					childRow.Delete(false);
				}
				_dsStockMinMax.AcceptChanges();
				gridStockMinMax.EndUpdate();
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		#endregion

		#region Drag Drop: the "Merchandise" column of the grid...

		/// <summary>
		/// Get the cell where the mouse is. If (and only if) the cell is in 
		/// Band[0] (details grid) and the column is "Description", 
		/// set the effect to ALL.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void grdPctContBasis_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            TreeNodeClipboardList cbList = null;
			try
			{
                Image_DragOver(sender, e);

				// BEGIN Track #5357 stodd
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
				{
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    //if (cbp.ClipboardDataType != eClipboardDataType.HierarchyNode)
                    if (cbList.ClipboardDataType != eProfileType.HierarchyNode)
					{
						e.Effect = DragDropEffects.None;
						return;
					}
				}
				else
				{
					e.Effect = DragDropEffects.None;
					return;
				}
				// END Track #5357

				Infragistics.Win.UIElement aUIElement;

				//				Point ptParent = PointToClient(new Point(e.X, e.Y));
				//
				//				int X = ptParent.X - this.tabOTSMethod.Left - this.grpGroupLevelMethod.Left - this.tabCriteria.Left - this.pnlPercentContribution.Left  - this.grpContributionBasis.Left - this.grdPctContBasis.Left -lblWidth30.Height; //- grdPctContBasis.Rows[0].Height;  //-lblWidth30.Height; //- 8;
				//				int Y = ptParent.Y - this.tabOTSMethod.Top - this.grpGroupLevelMethod.Top - this.tabCriteria.Top - this.pnlPercentContribution.Top - this.grpContributionBasis.Top - this.grdPctContBasis.Top - (this.Height - this.ClientSize.Height) -lblWidth30.Width; //grdPctContBasis.Rows[0].Cells["Merchandise"].Column.Width; //grdPctContBasis.Rows[0].Cells["Merchandise"] //-lblWidth30.Width; //- 30;//used to be  -10
				//				int z = this.grdPctContBasis.DisplayLayout.Bands[0].Columns["Merchandise"].Width;
				//			
				//				Point realPoint = new Point(X, Y);
				//				
				//				aUIElement = grdPctContBasis.DisplayLayout.UIElement.ElementFromPoint(realPoint);

				aUIElement = grdPctContBasis.DisplayLayout.UIElement.ElementFromPoint(grdPctContBasis.PointToClient(new Point(e.X, e.Y)));

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
				
				if (aCell == aRow.Cells["Merchandise"] && FunctionSecurity.AllowUpdate)
				{
					e.Effect = DragDropEffects.All;
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

		private void grdPctContBasis_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			HierarchyNodeProfile hnp;
            TreeNodeClipboardList cbList = null;
			try
			{
				Infragistics.Win.UIElement aUIElement;

				//				Point ptParent = PointToClient(new Point(e.X, e.Y));
				//
				//				int X = ptParent.X - this.tabOTSMethod.Left - this.grpGroupLevelMethod.Left - this.tabCriteria.Left - this.pnlPercentContribution.Left - this.grpContributionBasis.Left - this.grdPctContBasis.Left - lblWidth30.Height; //- grdPctContBasis.Rows[0].Height;  //-lblWidth30.Height; //- 8;
				//				int Y = ptParent.Y - this.tabOTSMethod.Top - this.grpGroupLevelMethod.Top - this.tabCriteria.Top - this.pnlPercentContribution.Top - this.grpContributionBasis.Top - this.grdPctContBasis.Top - (this.Height - this.ClientSize.Height) - lblWidth30.Width; //- grdPctContBasis.Rows[0].Cells["Merchandise"].Column.Width; //- lblWidth30.Width; //- 30;//used to be  -10
				//				int z = this.grdPctContBasis.DisplayLayout.Bands[0].Columns["Merchandise"].Width;
				//
				//				Point realPoint = new Point(X, Y);
				//	
				//				aUIElement = grdPctContBasis.DisplayLayout.UIElement.ElementFromPoint(realPoint);

				aUIElement = grdPctContBasis.DisplayLayout.UIElement.ElementFromPoint(grdPctContBasis.PointToClient(new Point(e.X, e.Y)));

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
                        // Create a new instance of the DataObject interface.
                        if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                        {
                            cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                            //IDataObject data = Clipboard.GetDataObject();

                            //if (data.GetDataPresent(ClipboardProfile.Format.Name))
                            //{
                            try
                            {
                                //cbp = GetClipboardData(eClipboardDataType.HierarchyNode);
                                // Issue 4422
                                //Begin Track #5378 - color and size not qualified
                                //								hnp = SAB.HierarchyServerSession.GetNodeData(cbp.Key);
                                // Begin TT#2647 - JSmith - Delays in OTS Method
                                //hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);
                                if (!_nodesByRID.TryGetValue(cbList.ClipboardProfile.Key, out hnp))
                                {
                                    hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);
                                    _nodesByRID.Add(cbList.ClipboardProfile.Key, hnp);
                                    if (!_nodesByID.ContainsKey(hnp.NodeID))
                                    {
                                        _nodesByID.Add(hnp.NodeID, hnp);
                                    }
                                }
                                // End TT#2647 - JSmith - Delays in OTS Method
                                //End Track #5378

                                aRow.Cells["MERCH_TYPE"].Value = (int)eMerchandiseType.Node;
                                aRow.Cells["MERCH_PH_RID"].Value = Include.NoRID;
                                aRow.Cells["MERCH_PHL_SEQUENCE"].Value = 0;
                                aRow.Cells["MERCH_OFFSET"].Value = 0;

                                bool nodeFound = false;
                                foreach (Infragistics.Win.ValueListItem vli in grdPctContBasis.DisplayLayout.ValueLists["Merchandise"].ValueListItems)
                                {
                                    if (vli.DisplayText == hnp.Text)
                                    {
                                        nodeFound = true;
                                        break;
                                    }
                                }
                                if (!nodeFound)
                                {
                                    Infragistics.Win.ValueListItem vli = new Infragistics.Win.ValueListItem();
                                    vli.DataValue = grdPctContBasis.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Count;
                                    vli.DisplayText = hnp.Text; ;
                                    vli.Tag = Convert.ToString(hnp.Key, CultureInfo.CurrentUICulture);
                                    grdPctContBasis.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Add(vli);
                                    //cellValue = vli.DataValue;	
                                }

                                // End Issue 4422
                                _dragAndDrop = true;
                                aCell.Value = hnp.Text;
                                aRow.Cells["HN_RID"].Value = hnp.Key;
                                _dragAndDrop = false;

                                // Begin TT#2647 - JSmith - Delays in OTS Method
                                //// BEGIN MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
                                //int sgl = (int)cbxStoreGroupLevel.SelectedValue;
                                //if (sgl == _defaultStoreGroupLevelRid)
                                //    UpdateSetsUsingDefault();
                                //// END MID Track #5954
                                // End TT#2647 - JSmith - Delays in OTS Method
                            }
                            // Begin Issue 5357 stodd 7.24.2008
                            catch (BadDataInClipboardException)
                            {
                                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
                                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            catch
                            {
                                throw;
                            }
                            // End Issue 5357 stodd 7.24.2008
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
		#endregion

		#region Misc events to hide controls used for the grid.
		private void grdPctContBasis_AfterColPosChanged(object sender, Infragistics.Win.UltraWinGrid.AfterColPosChangedEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnIncExc.Visible = false;
			//DateRangeSelector.Visible = false;
		}

		private void grdPctContBasis_AfterColRegionScroll(object sender, Infragistics.Win.UltraWinGrid.ColScrollRegionEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnIncExc.Visible = false;
			//DateRangeSelector.Visible = false;
		}

		private void grdPctContBasis_AfterExitEditMode(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnIncExc.Visible = false;
			//DateRangeSelector.Visible = false;
		}

		private void grdPctContBasis_AfterRowCollapsed(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnIncExc.Visible = false;
			//DateRangeSelector.Visible = false;
		}

		private void grdPctContBasis_AfterRowExpanded(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnIncExc.Visible = false;
			//DateRangeSelector.Visible = false;
		}
		
		private void grdPctContBasis_AfterRowRegionScroll(object sender, Infragistics.Win.UltraWinGrid.RowScrollRegionEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnIncExc.Visible = false;
			//DateRangeSelector.Visible = false;
		}		
		#endregion

		// Begin MID Track 4858 - JSmith - Security changes
//		private void btnProcess_Click(object sender, System.EventArgs e)
//		{
//			//			Cursor.Current = Cursors.WaitCursor;
//
//			try
//			{
//				if (ValidateAdditionalFieldsForProcess())
//				{
//					ProcessAction(eMethodType.OTSPlan);
//
//					// as part of the  processing we saved the info (if there were no errors), 
//					// so the method change type should be changed to update.
//					// Begin MID Tracker 2911 - stodd
//					if (!this.ErrorFound)
//					{
//						_OTSPlanMethod.Method_Change_Type = eChangeType.update;
//						btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
//					}
//					// End MID tracker 2911
//				}
//
//			}
//			catch(Exception ex)
//			{
//				HandleException(ex, "btnProcess_Click");
//			}
//
//			//			Cursor.Current = Cursors.Default;
//		}

		protected override void Call_btnProcess_Click()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				if (ValidateAdditionalFieldsForProcess())
				{
					ProcessAction(eMethodType.OTSPlan);

					// as part of the  processing we saved the info (if there were no errors), 
					// so the method change type should be changed to update.
					if (!this.ErrorFound)
					{
						_OTSPlanMethod.Method_Change_Type = eChangeType.update;
						btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
					}
				}

			}
			catch(Exception ex)
			{
				HandleException(ex, "Call_btnProcess_Click");
			}
		}
		// End MID Track 4858

		private bool ValidateAdditionalFieldsForProcess()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			bool methodFieldsValid = true;
//Begin Track #5219 - JScott - Method Read-only when Version is inactive - Part 2
//			int lAddTag = (int)txtOTSHNDesc.Tag;
//			// HeirarchyNode
//			if (lAddTag != Include.NoRID)
//			{
//				ErrorProvider.SetError (txtOTSHNDesc,string.Empty);
//			}
//			else
//			{
//				methodFieldsValid = false;
//				ErrorProvider.SetError (txtOTSHNDesc,"Product Name is a required field when processing now.");
//			}
//
//			//Plan Version
//			if ((int)cboPlanVers.SelectedValue == Include.NoRID)
//			{
//				methodFieldsValid = false;
//				ErrorProvider.SetError (cboPlanVers,"Plan Version is a required field when processing now.");
//			}
//			else
//			{
//				ErrorProvider.SetError (cboPlanVers,string.Empty);
//			}
//
//			//Chain Version
//			if ((int)cboChainVers.SelectedValue == Include.NoRID)
//			{
//				methodFieldsValid = false;
//				ErrorProvider.SetError (cboChainVers,"Chain Version is a required field when processing now.");
//			}
//			else
//			{
//				ErrorProvider.SetError (cboChainVers,string.Empty);
//			}
//
//			_GLFProfileList = _OTSPlanMethod.GLFProfileList;
//
//			foreach (GroupLevelFunctionProfile glfp in _GLFProfileList.ArrayList)
//			{
//				foreach (BasisPlanProfile basis in glfp.Basis_Plan.ArrayList)
//				{
//					if (basis.Basis_HN_RID == Include.NoRID)
//					{
//						string errorMessage = "One or more Merchandise fields in the basis are empty. These are required " +
//							"when procesisng now.";
//						MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//						methodFieldsValid = false;
//						break;
//					}
//					if (!methodFieldsValid)
//						break;
//				}
//			}
			// HeirarchyNode
            //Begin Track #5858 - KJohnson - Validating store security only
            if (((MIDTag)(txtOTSHNDesc.Tag)).MIDTagData == null ||
                ((HierarchyNodeProfile)((MIDTag)(txtOTSHNDesc.Tag)).MIDTagData).Key == Include.NoRID)
			{
				methodFieldsValid = false;
				ErrorProvider.SetError (txtOTSHNDesc, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
			}
            // Begin Track #5926 - JSmith - Save As when no security
            //else
            //{
            //    //Begin Track #5858 - JSmith - Validating store security only
            //    //if (!base.ValidatePlanNodeSecurity(txtOTSHNDesc))
            //    if (!base.ValidateStorePlanNodeSecurity(txtOTSHNDesc))
            //    //End Track #5858
            //    {
            //        methodFieldsValid = false;
            //    }
            //    else
            //    {
            //        ErrorProvider.SetError (txtOTSHNDesc,string.Empty);
            //    }
            //}
            // End Track #5926

			//Plan Version
            //if ((int)cboPlanVers.SelectedValue == Include.NoRID)  //TT#736 - MD - ComboBox causes a NullReferenceException - RBeck 
			if ((int)cboPlanVers.SelectedIndex == Include.NoRID)
			{
				methodFieldsValid = false;
				ErrorProvider.SetError (cboPlanVers, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
			}
            // Begin Track #5926 - JSmith - Save As when no security
            //else
            //{
            //    //Begin Track #5858 - JSmith - Validating store security only
            //    //if (!base.ValidatePlanVersionSecurity(this.cboPlanVers))
            //    if (!base.ValidateStorePlanVersionSecurity(this.cboPlanVers))
            //    //End Track #5858
            //    {
            //        methodFieldsValid = false;
            //    }
            //    else
            //    {
            //        ErrorProvider.SetError (cboPlanVers,string.Empty);
            //    }
            //}
            // End Track #5926

			//Chain Version
            //if ((int)cboChainVers.SelectedValue == Include.NoRID)  //TT#736 - MD - ComboBox causes a NullReferenceException - RBeck
			if ((int)cboChainVers.SelectedIndex == Include.NoRID)
			{
				methodFieldsValid = false;
				ErrorProvider.SetError (cboChainVers, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
			}
            // Begin Track #5926 - JSmith - Save As when no security
            //else
            //{
            //    //Begin Track #5858 - JSmith - Validating store security only
            //    //if (!base.ValidatePlanVersionSecurity(this.cboChainVers))
            //    if (!base.ValidateStorePlanVersionSecurity(this.cboChainVers))
            //    //End Track #5858
            //    {
            //        methodFieldsValid = false;
            //    }
            //    else
            //    {
            //        ErrorProvider.SetError (cboChainVers,string.Empty);
            //    }
            //}
            // End Track #5926

			_GLFProfileList = _OTSPlanMethod.GLFProfileList;

			foreach (GroupLevelFunctionProfile glfp in _GLFProfileList.ArrayList)
			{
				foreach (GroupLevelBasisProfile basis in glfp.GroupLevelBasis.ArrayList)
				{
					if (basis.Basis_HN_RID == Include.NoRID)
					{
						string errorMessage = "One or more Merchandise fields in the basis are empty and are required.";
						MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
						methodFieldsValid = false;
						break;
					}
					if (!methodFieldsValid)
						break;
				}
			}
//End Track #5219 - JScott - Method Read-only when Version is inactive - Part 2

			return methodFieldsValid;
		}



		#endregion

		#region WorkflowMethodFormBase Overrides 

		/// <summary>
		/// Gets if workflow or method.
		/// </summary>
		override protected eWorkflowMethodIND WorkflowMethodInd()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			return eWorkflowMethodIND.Methods;	
		}

		/// <summary>
		/// Use to set the method name, description, user and global radio buttons
		/// </summary>
		override protected void SetCommonFields()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			WorkflowMethodName = txtName.Text;
			WorkflowMethodDescription = txtDesc.Text;
			GlobalRadioButton = radGlobal;
			UserRadioButton = radUser;

			ErrorProvider.SetError (txtName,string.Empty);
			ErrorProvider.SetError (txtDesc,string.Empty);
			ErrorProvider.SetError (pnlGlobalUser,string.Empty);
		}

		/// <summary>
		/// Use to set the specific fields in method object before updating
		/// </summary>
		override protected void SetSpecificFields()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				// Sets high level/common fields
				SetMethodValues(_OTSPlanMethod.Method_Change_Type);
				//Set OTS Plan fields
				SetOTSPlanValues(_OTSPlanMethod.Method_Change_Type);
				//Store Group Level Function and related Basis
 				SetSetMethodValues(_OTSPlanMethod.Method_Change_Type);
                //Fill Children With Default Data
                // Begin TT#2647 - JSmith - Delays in OTS Method
                //PropagateDefautItemToChildren();
                UpdateSetsUsingDefault();
                // End TT#2647 - JSmith - Delays in OTS Method
			}
			catch(Exception err)
			{
				HandleException(err);
			}
		}

        //Begin Track #5420 - KJohnson - Default not pushing out screen changes to child elements
        private void PropagateDefautItemToChildren()
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                int currKey = _GLFProfile.Key;
                // End TT#3
                //--Get Default Item--------
                GroupLevelFunctionProfile defaultGLFP = new GroupLevelFunctionProfile(Include.NoRID);
                //ProfileList defaultGLFProfileList = new ProfileList(eProfileType.GroupLevelFunction);	// TT#3 - stodd
                foreach (GroupLevelFunctionProfile glfp in _OTSPlanMethod.GLFProfileList.ArrayList)
                {
                    if (glfp.Default_IND)
                    {
                        defaultGLFP = new GroupLevelFunctionProfile(glfp.Key);
                        defaultGLFP.Default_IND = glfp.Default_IND;
                        defaultGLFP.Plan_IND = glfp.Plan_IND;
                        defaultGLFP.Use_Default_IND = glfp.Use_Default_IND;
                        defaultGLFP.Clear_IND = glfp.Clear_IND;
                        defaultGLFP.Season_IND = glfp.Season_IND;
                        defaultGLFP.Season_HN_RID = glfp.Season_HN_RID;
                        defaultGLFP.GLFT_ID = glfp.GLFT_ID;
                        defaultGLFP.GLSB_ID = glfp.GLSB_ID;
                        defaultGLFP.LY_Alt_IND = glfp.LY_Alt_IND;
                        defaultGLFP.Trend_Alt_IND = glfp.Trend_Alt_IND;
                        defaultGLFP.TY_Weight_Multiple_Basis_Ind = glfp.TY_Weight_Multiple_Basis_Ind;
                        defaultGLFP.LY_Weight_Multiple_Basis_Ind = glfp.LY_Weight_Multiple_Basis_Ind;
                        defaultGLFP.Apply_Weight_Multiple_Basis_Ind = glfp.Apply_Weight_Multiple_Basis_Ind;
                        defaultGLFP.GLF_Change_Type = glfp.GLF_Change_Type;
                        // Copies in Basis info AND trend caps
                        // BEGIN Issue 4818
                        foreach (GroupLevelBasisProfile glbp in glfp.GroupLevelBasis)
                        {
                            defaultGLFP.GroupLevelBasis.Add(glbp.Copy());
                        }
                        // END Issue 4818
                        foreach (TrendCapsProfile tcp in glfp.Trend_Caps)
                        {
                            defaultGLFP.Trend_Caps.Add(tcp.Copy());
                        }
                        foreach (int Key in glfp.Group_Level_Nodes.Keys)
                        {
                            defaultGLFP.Group_Level_Nodes.Add(Key, glfp.Group_Level_Nodes[Key]);
                        }
                        break;
                    }
                }

				// Begin TT#3 - stodd - Forecasting issues with Min/Max grid
				////--Record Child Items In List--------
				//foreach (GroupLevelFunctionProfile glfp in _OTSPlanMethod.GLFProfileList.ArrayList)
				//{
				//    if (glfp.Use_Default_IND)
				//    {
				//        defaultGLFP.Default_IND = false;
				//        defaultGLFP.Use_Default_IND = true;
				//        defaultGLFP.Key = glfp.Key;
				//        defaultGLFProfileList.Add(defaultGLFP);
				//    }
				//}

				////--Replace Children With Defaults Data--------
				//foreach (GroupLevelFunctionProfile glfp in defaultGLFProfileList)
				//{
				//    // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
				//    //_OTSPlanMethod.GLFProfileList.Remove(glfp);
				//    //_OTSPlanMethod.GLFProfileList.Add(glfp);
				//    // End TT#3
				//}
				// End TT#3 - stodd - Forecasting issues with Min/Max grid


                foreach (GroupLevelFunctionProfile glfp in _OTSPlanMethod.GLFProfileList.ArrayList)
                {
                    if (glfp.Use_Default_IND)
                    {
                        CopyMinMax(glfp.Key, false);
                        // Begin TT#1147 - JSmith -  Foreacst Audit Report not accurate for New Store Set
                        glfp.GroupLevelBasis.Clear();
                        foreach (GroupLevelBasisProfile groupLevelBasisProfile in defaultGLFP.GroupLevelBasis)
                        {
                            glfp.GroupLevelBasis.Add(groupLevelBasisProfile.Copy());
                        }
                        // End TT#1147
                    }
                }

                _GLFProfileList = _OTSPlanMethod.GLFProfileList;
				DebugGroupLevelForecastList(_GLFProfileList);

                // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                _GLFProfile = (GroupLevelFunctionProfile)_GLFProfileList.FindKey(currKey);
                // End TT#3

                // Begin TT#1874 - JSmith - Min Dissapearing
                PopulateStockMinMax((GroupLevelNodeFunction)_GLFProfile.Group_Level_Nodes[GetSelectedValue()]);
                // End TT#1874

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        //End Track #5420

		/// <summary>
		/// Use to validate the fields that are specific to this method type
		/// </summary>
		override protected bool ValidateSpecificFields()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			bool methodFieldsValid = true;
			try
			{
				if (this._OTSPlanMethod.Method_Change_Type != DataCommon.eChangeType.delete)
				{
					int lAddTag = Include.NoRID;

//Begin Track #5219 - JScott - Method Read-only when Version is inactive - Part 2
					methodFieldsValid = ValidateAdditionalFieldsForProcess();

//End Track #5219 - JScott - Method Read-only when Version is inactive - Part 2
					if (midDateRangeSelectorOTSPlan.Tag != null)
					{
						lAddTag = (int)midDateRangeSelectorOTSPlan.Tag;
						ErrorProvider.SetError (midDateRangeSelectorOTSPlan,string.Empty);
					}
					else
					{
						methodFieldsValid = false;
						ErrorProvider.SetError (midDateRangeSelectorOTSPlan,"Required field Date Range is invalid.");
					}
			
					if (!ValidBasis())
						methodFieldsValid = false;
//Begin Track #5219 - JScott - Method Read-only when Version is inactive - Part 2
//
//					// BEGIN Issue 4858
//					if (!base.ValidatePlanNodeSecurity(txtOTSHNDesc))
//						methodFieldsValid = false;
//					if (!base.ValidatePlanVersionSecurity(this.cboPlanVers))
//						methodFieldsValid = false;
//					// END Issue 4858
//End Track #5219 - JScott - Method Read-only when Version is inactive - Part 2

                    //Begin TT#15 - JSmith - Save allowed when required fields not specified
                    if (!chkHighLevel.Checked && !chkLowLevels.Checked)
                    {
                        string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneOptionRequired);
                        ErrorProvider.SetError(chkHighLevel, errorMessage);
                        ErrorProvider.SetError(chkLowLevels, errorMessage);
                        methodFieldsValid = false;
                    }
                    else
                    {
                        ErrorProvider.SetError(chkHighLevel, string.Empty);
                        ErrorProvider.SetError(chkLowLevels, string.Empty);
                    }
                    //End TT#15
				}
            //TT#736 - Begin - MD - ComboBox causes a NullReferenceException - RBeck
                if (chkLowLevels.Checked && cboLowLevels.SelectedItem == null)
                {
                    methodFieldsValid = false;
                    ErrorProvider.SetError(cboLowLevels, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_LowLevelsNotDefined));
                }
            //TT#736 - End - MD - ComboBox causes a NullReferenceException - RBeck
			}
			catch (Exception ex)
			{
				HandleException(ex);
				methodFieldsValid = false;
			}
			return methodFieldsValid;
		}

		override public bool VerifySecurity()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			return true;
		}

		/// <summary>
		/// Use to set the errors to the screen
		/// </summary>
		override protected void HandleErrors()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (!WorkflowMethodNameValid)
			{
				ErrorProvider.SetError (txtName,WorkflowMethodNameMessage);
			}
			else
			{
				ErrorProvider.SetError (txtName,string.Empty);
			}
			if (!WorkflowMethodDescriptionValid)
			{
				ErrorProvider.SetError (txtDesc,WorkflowMethodDescriptionMessage);
			}
			else
			{
				ErrorProvider.SetError (txtDesc,string.Empty);
			}
			if (!UserGlobalValid)
			{
				ErrorProvider.SetError (pnlGlobalUser,UserGlobalMessage);
			}
			else
			{
				ErrorProvider.SetError (pnlGlobalUser,string.Empty);
			}
		}

		/// <summary>
		/// Use to set the specific method object before updating
		/// </summary>
		override protected void SetObject()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				ABM = _OTSPlanMethod;
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		/// <summary>
		/// Use to return the explorer node selected when form was opened
		/// </summary>
		override protected MIDWorkflowMethodTreeNode GetExplorerNode()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			return ExplorerNode;
		}

		#endregion WorkflowMethodFormBase Overrides

		private void txtDesc_TextChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		private void cboFuncType_SelectedValueChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			// if UseDefault was checked before AND the user is now changing to a different Fucntion type,
			// it can't be the same function type as the default so it's unchecked.  whew!
            // Begin TT#1674 - JSmith - Mins/max disappearing in the OTS forecast method
			//chkUseDefault.Checked = false;
            if (!_useDefaultSetTrue)
            {
                chkUseDefault.Checked = false;
            }
            // End TT#1674
		}

		private void cboFuncType_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			bool showMessage = false;
			string messageMethod = string.Empty;
			if (!FormLoaded)
				return;

            if (!_storeGroupLevelChanged)
            {
                ChangePending = true;
            }

			switch (_prevFuncTypeValue)
			{
				case (int)eGroupLevelFunctionType.PercentContribution:
					if (grdPctContBasis.Rows.Count > 0)
					{
						messageMethod = MIDText.GetTextOnly((int)eGroupLevelFunctionType.PercentContribution);
						//showMessage = true;
					}
					break;	
				case (int)eGroupLevelFunctionType.TyLyTrend:
					if ( gridTYNodeVersion.Rows.Count > 0 
						||	gridLYNodeVersion.Rows.Count > 0	  
						||	gridTrendNodeVersion.Rows.Count > 0 )
					{
						messageMethod = MIDText.GetTextOnly((int)eGroupLevelFunctionType.TyLyTrend);
						//showMessage = true;
					}
					break;	
			}
			if (showMessage)
			{
				DialogResult dr = DialogResult.Yes;
						
				dr=MessageBox.Show("Changing the method type will delete the current " + messageMethod + 
					" method information for this set upon saving.  Are you sure you want to continue?",
					MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanMethod), MessageBoxButtons.YesNo);
			
				if (dr==DialogResult.No) 
				{
					this.cboFuncType.SelectedValue = _prevFuncTypeValue;
				}
				else if (dr==DialogResult.Yes) 
				{
					if (this.chkDefault.Checked == true)
						_defaultFunctionType = (int)cboFuncType.SelectedValue;
				}
			}

			//			// if UseDefault was checked before AND the user is now changing to a different Fucntion type,
			//			// it can't be the same function type as the default so it's unchecked.  whew!
			//			chkUseDefault.Checked = false;

			if (this.cboFuncType.Text == MIDText.GetTextOnly((int)eGroupLevelFunctionType.PercentContribution))
			{
				this.pnlPercentContribution.Visible = true;
				this.pnlTYLYTrend.Visible = false;
			}
			else if (this.cboFuncType.Text == MIDText.GetTextOnly((int)eGroupLevelFunctionType.TyLyTrend))
			{
				this.pnlPercentContribution.Visible = false;
				this.pnlTYLYTrend.Visible = true;
				_currentTYLYTabPage = tabPageTY;
			}
			_prevFuncTypeValue = Convert.ToInt32(this.cboFuncType.SelectedValue);

			//Begin TT#891 - JScott - OTS Forecast % Contribution not getting expected stock results.
			EnableTrendControls();
			//End TT#891 - JScott - OTS Forecast % Contribution not getting expected stock results.
		}

		//Begin TT#891 - JScott - OTS Forecast % Contribution not getting expected stock results.
		private void EnableTrendControls()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (this.cboFuncType.Text == MIDText.GetTextOnly((int)eGroupLevelFunctionType.PercentContribution))
			{
				radChainPlan.Checked = true;
				radChainWOS.Enabled = false;
				radPlugChainWOS.Enabled = false;
			}
			else
			{
				radChainWOS.Enabled = true;
				radPlugChainWOS.Enabled = true;
			}
		}

		//End TT#891 - JScott - OTS Forecast % Contribution not getting expected stock results.
		private void gridTYNodeVersion_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			TyLyGrid_InitializeLayout(e);
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
            //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
            // End TT#1164
            //End TT#169
		}


		private void gridLYNodeVersion_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			TyLyGrid_InitializeLayout(e);
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
            //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
            // End TT#1164
            //End TT#169
		}

		private void gridTrendNodeVersion_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			TyLyGrid_InitializeLayout(e);
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
            //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
            // End TT#1164
            //End TT#169
		}

		private void TyLyGrid_InitializeLayout(Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				//The following information pertains to the formatting of the grid.

				//e.Layout.AutoFitColumns = true;
				e.Layout.Bands[0].ColHeadersVisible = true;
				//Prevent the user from re-arranging columns.
				e.Layout.Override.AllowColMoving = AllowColMoving.NotAllowed;

				//hide the key columns.
				e.Layout.Bands[0].Columns["METHOD_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["SGL_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["HN_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["FV_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["INC_EXC_IND"].Hidden = true;
				e.Layout.Bands[0].Columns["SEQ"].Hidden = true;
				e.Layout.Bands[0].Columns["TYLY_TYPE_ID"].Hidden = true;
				// Issue # 4422
				e.Layout.Bands[0].Columns["MERCH_TYPE"].Hidden = true;
				e.Layout.Bands[0].Columns["MERCH_PH_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["MERCH_PHL_SEQUENCE"].Hidden = true;
				e.Layout.Bands[0].Columns["MERCH_OFFSET"].Hidden = true;
				// Issue #4422
				// BEGIN Issue 4814
				e.Layout.Bands[0].Columns["CDR_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["Picture"].Hidden = true;
				// END Issue 4814

				//Set the header captions.
				e.Layout.Bands[0].Columns["Merchandise"].Header.VisiblePosition = 1;
				e.Layout.Bands[0].Columns["Merchandise"].Header.Caption = "Merchandise";
				e.Layout.Bands[0].Columns["Merchandise"].Width = 160;
				if (chkLowLevels.Checked)
					e.Layout.Bands[0].Columns["Merchandise"].ValueList = e.Layout.ValueLists["Merchandise"];
				e.Layout.Bands[0].Columns["Version"].Header.VisiblePosition = 2;
				e.Layout.Bands[0].Columns["Version"].Width = 75;
				e.Layout.Bands[0].Columns["Version"].Header.Caption = "Version";
				e.Layout.Bands[0].Columns["IncludeButton"].Header.Caption = " ";
		
				//make the "Version" column a drop down list.
				e.Layout.Bands[0].Columns["Version"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				e.Layout.Bands[0].Columns["Version"].ValueList = e.Layout.ValueLists["Version"];
		
				//Make the "INC_EXC_IND" column a checkbox column.
				e.Layout.Bands[0].Columns["INC_EXC_IND"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

				// BEGIN Issue 4818
				e.Layout.Bands[0].Columns["DateRange"].Header.Caption = "Date Range";
				e.Layout.Bands[0].Columns["WEIGHT"].Header.Caption = "Weight";
				e.Layout.Bands[0].Columns["DateRange"].Header.VisiblePosition = 3;
                //e.Layout.Bands[0].Columns["DateRange"].AutoEdit = false;
                e.Layout.Bands[0].Columns["DateRange"].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
				e.Layout.Bands[0].Columns["DateRange"].Width = 160;
				e.Layout.Bands[0].Columns["DateRange"].Style  = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
				e.Layout.Bands[0].Columns["DateRange"].CellActivation = Activation.NoEdit;

				e.Layout.Bands[0].Columns["WEIGHT"].Header.VisiblePosition = 4;
				e.Layout.Bands[0].Columns["WEIGHT"].Width = 45;
				// END Issue 4818

				if (FunctionSecurity.AllowUpdate)
				{
					e.Layout.Override.AllowAddNew = AllowAddNew.Yes;
					e.Layout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
					e.Layout.Bands[0].AddButtonCaption = "Add New Merchandise/Version";
					e.Layout.Bands[0].AddButtonToolTipText = "Click to add new Merchandise Node & Version information";
					e.Layout.AddNewBox.Hidden = false;
					e.Layout.AddNewBox.Style = AddNewBoxStyle.Compact;
				} 
				else
				{
					e.Layout.Override.AllowAddNew = AllowAddNew.No;
					e.Layout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
				} 

				//the "IncludeButton" column (two spaces) is the column that contains buttons
				//to include/exclude a basis detail. 
				e.Layout.Bands[0].Columns["IncludeButton"].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
				e.Layout.Bands[0].Columns["IncludeButton"].CellAppearance.ImageVAlign = Infragistics.Win.VAlign.Middle;
				e.Layout.Bands[0].Columns["IncludeButton"].Width = 20;
				// Begin Track #5955 stodd
				e.Layout.Bands[0].Columns["IncludeButton"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
				e.Layout.Bands[0].Columns["IncludeButton"].ButtonDisplayStyle = ButtonDisplayStyle.Always;
				e.Layout.Bands[0].Columns["IncludeButton"].CellButtonAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
				e.Layout.Bands[0].Columns["IncludeButton"].CellButtonAppearance.ImageVAlign = Infragistics.Win.VAlign.Middle;
				// End Track #5955 stodd
			}
			catch(Exception err)
			{
				HandleException(err);
			}
		}

		private void GridContextMenu_Popup(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			foreach (MenuItem mi in GridContextMenu.MenuItems)
			{
				mi.Enabled = false;
				mi.Visible = false;
			}
			if (FunctionSecurity.AllowUpdate &&
				_gridMouseDownIsFrom != null)
			{
				if (_gridMouseDownIsFrom.Name == gridStockMinMax.Name)
				{
//					GridContextMenu.MenuItems[1].Enabled = false; // Insert
//					GridContextMenu.MenuItems[1].Visible = false; // Insert
					mnuDelete.Enabled = true;
					mnuDelete.Visible = true;
					mnuDeleteAll.Enabled = true;
					mnuDeleteAll.Visible = true;
					mnuSeparator.Visible = true;
					mnuCollapseAll.Enabled = true;
					mnuCollapseAll.Visible = true;
					mnuExpandAll.Enabled = true;
					mnuExpandAll.Visible = true;
				}
				else
				{
//					GridContextMenu.MenuItems[1].Enabled = true; // Insert
//					GridContextMenu.MenuItems[1].Visible = true; // Insert
					if (_gridMouseDownIsFrom.Selected.Rows.Count > 0)
					{
						mnuDelete.Enabled = true;
						mnuDelete.Visible = true;
					}
					mnuInsert.Enabled = true;
					mnuInsert.Visible = true;
				}
			}
//			else
//			{
//				GridContextMenu.MenuItems[1].Enabled = false; // Insert
//				GridContextMenu.MenuItems[1].Visible = false; // Insert
//				GridContextMenu.MenuItems[0].Enabled = false; // Delete
//				GridContextMenu.MenuItems[0].Visible = false; // Delete
//			}
		}

		private void menu_Click(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		private void GridMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			UltraGrid grid;

			try
			{
				if (sender.GetType() == typeof(UltraGrid))
				{
					grid = (UltraGrid)sender;

					// event does not fire when in edit mode
					//					if (e.Button == MouseButtons.Right)
					//					{
					_gridMouseDownIsFrom = grid;

					//					}
				}
				
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

	
		private void radNone_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (radNone.Checked == false) return;
			txtTolerance.Text = string.Empty;
			txtHigh.Text = string.Empty;
			txtLow.Text = string.Empty;
			txtHigh.Enabled = false;
			txtLow.Enabled = false;
			txtTolerance.Enabled = false;
		}
		private void radTolerance_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (radTolerance.Checked == false) return;
			
			txtHigh.Text = string.Empty;
			txtLow.Text = string.Empty;
			txtHigh.Enabled = false;
			txtLow.Enabled = false;
			txtTolerance.Enabled = true;
		}

		private void radLimits_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (radLimits.Checked == false) return;
			
			txtTolerance.Text = string.Empty;
			txtHigh.Enabled = true;
			txtLow.Enabled = true;
			txtTolerance.Enabled = false;
		}

		
		private void gridTYNodeVersion_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			TyLyGrid_InitializeRow(gridTYNodeVersion, e);	
		}


		private void gridLYNodeVersion_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			// BEGIN MID Track #5954 - KJohnson - Basis weighting, exclusions, and equalizer issues
			TyLyGrid_InitializeRow(gridLYNodeVersion, e);	
			// END MID Track #5954
		}

		private void gridTrendNodeVersion_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			TyLyGrid_InitializeRow(gridTrendNodeVersion, e);	
		}


		private void TyLyGrid_InitializeRow(UltraGrid aGrid, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			// stodd 4.4.2008 Issue 5314
			// Added the 'if not dragAndDrop' around code. When a node was being dropped on the grid it was going
			// through this logic. The node info has already been set by this time, so doing it again added an
			// extra 1/2 second to the drop.
			// stodd 4.6.2008 Issue 5315
			// Found another instance where it was going through this code when it did need to--during the 
			// SetBasisDates() method...but only sometimes. This was causing the basis date display to be
			// incorrect.
			if (_dragAndDrop || e.ReInitialize)
			{
				// Skip it.
			}
			else
			{
				//Based on indicator, add picInclude or not
				if (e.Row.Band == aGrid.DisplayLayout.Bands[0] &&
					(e.Row.Cells["INC_EXC_IND"].Value.ToString() == "0"))
				{
					e.Row.Cells["IncludeButton"].Appearance.Image = picInclude;
					e.Row.Cells["IncludeButton"].ButtonAppearance.Image = picInclude;	// Track #5955 stodd
				}
				else if (e.Row.Band == aGrid.DisplayLayout.Bands[0] &&
					(e.Row.Cells["INC_EXC_IND"].Value.ToString() != "0"))
				{
					e.Row.Cells["IncludeButton"].Appearance.Image = picExclude;
					e.Row.Cells["IncludeButton"].ButtonAppearance.Image = picExclude;	// Track #5955 stodd
				}
 			
				//Populate cell w/text description of Hierarchy Node
				if (e.Row.Cells["HN_RID"].Value.ToString() != "")
				{
					// Begin Issue 4422 - stodd
					HierarchyNodeProfile hnp = new HierarchyNodeProfile(Include.NoRID);
					int nodeKey = Convert.ToInt32(e.Row.Cells["HN_RID"].Value, CultureInfo.CurrentUICulture);
					if (nodeKey == 0)  // Means node is in value list drop down
					{
						_merchValChanged = true;
						// End MID Issue 3443/3494
						e.Row.Cells["Merchandise"].Value = 0;
						// Begin MID Issue 3443/3494 stodd
						_merchValChanged = false;
					}
					else if (nodeKey != Include.NoRID)
					{
						//Begin Track #5378 - color and size not qualified
//						hnp = SAB.HierarchyServerSession.GetNodeData(nodeKey);
						
                        // Begin TT#2647 - JSmith - Delays in OTS Method
                        //hnp = SAB.HierarchyServerSession.GetNodeData(nodeKey, true, true);
                        if (!_nodesByRID.TryGetValue(nodeKey, out hnp))
                        {
                            hnp = SAB.HierarchyServerSession.GetNodeData(nodeKey, true, true);
                            _nodesByRID.Add(nodeKey, hnp);
                            if (!_nodesByID.ContainsKey(hnp.NodeID))
                            {
                                _nodesByID.Add(hnp.NodeID, hnp);
                            }
                        }
                        // End TT#2647 - JSmith - Delays in OTS Method
						//End Track #5378
						//				e.Row.Cells["Merchandise"].Value = hnp.NodeID;
						// Begin MID Issue 3443/3494 stodd
						_merchValChanged = true;
						// End MID Issue 3443/3494
						e.Row.Cells["Merchandise"].Value = hnp.Text;
						// Begin MID Issue 3443/3494 stodd
						_merchValChanged = false;
						// End MID Issue 3443/3494
					}
					// End Issue 4422
				}
				// BEGIN Issue 4818
				if (e.Row.Cells["CDR_RID"].Value.ToString() != "")
				{
					DateRangeProfile dr;
					if (midDateRangeSelectorOTSPlan.Tag != null)
						dr = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(e.Row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture),(int)midDateRangeSelectorOTSPlan.Tag);
					else
						dr = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(e.Row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture));

					e.Row.Cells["DateRange"].Value = dr.DisplayDate;
					e.Row.Cells["CDR_RID"].Value = dr.Key;

					if (dr.DateRangeType == eCalendarRangeType.Dynamic)
					{
						switch (dr.RelativeTo)
						{
							case eDateRangeRelativeTo.Current:
								e.Row.Cells["DateRange"].Appearance.Image = this.DynamicToCurrentImage;
								break;
							case eDateRangeRelativeTo.Plan:
								e.Row.Cells["DateRange"].Appearance.Image = this.DynamicToPlanImage;
								break;
							default:
								e.Row.Cells["DateRange"].Appearance.Image = null;
								break;
						}
					}
				}
				// END Issue 4818	

				//Populate cell w/text description of Forecast Version
				if (e.Row.Cells["FV_RID"].Value.ToString() != "")
				{
					DataTable dt;
					ForecastVersion fv = new ForecastVersion();
					dt = fv.GetForecastVersions();
					dt.PrimaryKey = new DataColumn[] {dt.Columns["FV_RID"]};
					DataRow row = dt.Rows.Find(e.Row.Cells["FV_RID"].Value);

					if (row != null)
						e.Row.Cells["Version"].Value = row["DESCRIPTION"]; 
				}
			}
		}
		
		private void btnTYIncExc_Click(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (m_includeCell.Value.ToString() == "0")
			{
				//It's currently included. So we'll exclude it.
				m_includeCell.Value = "1";

				UltraGridCell picCell = m_aRow.Cells["IncludeButton"];
				picCell.Appearance.Image = picExclude;
				btnTYIncExc.Image = picExclude;
			}
			else if (m_includeCell.Value.ToString() == "1")
			{
				//It's currently excluded. So we'll include it.
				m_includeCell.Value = "0";

				UltraGridCell picCell = m_aRow.Cells["IncludeButton"];
				picCell.Appearance.Image = picInclude;
				btnTYIncExc.Image = picInclude;
			}
		}

		private void btnLYIncExc_Click(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (m_includeCell.Value.ToString() == "0")
			{
				//It's currently included. So we'll exclude it.
				m_includeCell.Value = "1";

				UltraGridCell picCell = m_aRow.Cells["IncludeButton"];
				picCell.Appearance.Image = picExclude;
				btnLYIncExc.Image = picExclude;
			}
			else if (m_includeCell.Value.ToString() == "1")
			{
				//It's currently excluded. So we'll include it.
				m_includeCell.Value = "0";

				UltraGridCell picCell = m_aRow.Cells["IncludeButton"];
				picCell.Appearance.Image = picInclude;
				btnLYIncExc.Image = picInclude;
			}
		}

		private void btnTrendIncExc_Click(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (m_includeCell.Value.ToString() == "0")
			{
				//It's currently included. So we'll exclude it.
				m_includeCell.Value = "1";

				UltraGridCell picCell = m_aRow.Cells["IncludeButton"];
				picCell.Appearance.Image = picExclude;
				btnTrendIncExc.Image = picExclude;
			}
			else if (m_includeCell.Value.ToString() == "1")
			{
				//It's currently excluded. So we'll include it.
				m_includeCell.Value = "0";

				UltraGridCell picCell = m_aRow.Cells["IncludeButton"];
				picCell.Appearance.Image = picInclude;
				btnTrendIncExc.Image = picInclude;
			}
		}

		private void gridTrendNodeVersion_AfterColPosChanged(object sender, Infragistics.Win.UltraWinGrid.AfterColPosChangedEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnTrendIncExc.Visible = false;
		}

		private void gridTrendNodeVersion_AfterColRegionScroll(object sender, Infragistics.Win.UltraWinGrid.ColScrollRegionEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnTrendIncExc.Visible = false;
		}

		private void gridTrendNodeVersion_AfterExitEditMode(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnTrendIncExc.Visible = false;
		}

		private void gridTrendNodeVersion_AfterRowCollapsed(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnTrendIncExc.Visible = false;
		}

		private void gridTrendNodeVersion_AfterRowExpanded(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnTrendIncExc.Visible = false;
		}

		private void gridTrendNodeVersion_AfterRowRegionScroll(object sender, Infragistics.Win.UltraWinGrid.RowScrollRegionEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnTrendIncExc.Visible = false;
		}

		private void gridTrendNodeVersion_BeforeCellDeactivate(object sender, System.ComponentModel.CancelEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnTrendIncExc.Visible = false;
		}

		private void gridLYNodeVersion_BeforeCellDeactivate(object sender, System.ComponentModel.CancelEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnLYIncExc.Visible = false;
		}

		private void gridLYNodeVersion_AfterColPosChanged(object sender, Infragistics.Win.UltraWinGrid.AfterColPosChangedEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnLYIncExc.Visible = false;
		}

		private void gridLYNodeVersion_AfterColRegionScroll(object sender, Infragistics.Win.UltraWinGrid.ColScrollRegionEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnLYIncExc.Visible = false;
		}

		private void gridLYNodeVersion_AfterExitEditMode(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnLYIncExc.Visible = false;
		}

		private void gridLYNodeVersion_AfterRowCollapsed(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnLYIncExc.Visible = false;
		}

		private void gridLYNodeVersion_AfterRowExpanded(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnLYIncExc.Visible = false;
		}

		private void gridLYNodeVersion_AfterRowRegionScroll(object sender, Infragistics.Win.UltraWinGrid.RowScrollRegionEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnLYIncExc.Visible = false;
		}

		private void gridTYNodeVersion_BeforeCellDeactivate(object sender, System.ComponentModel.CancelEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnTYIncExc.Visible = false;
		}

		private void gridTYNodeVersion_AfterColPosChanged(object sender, Infragistics.Win.UltraWinGrid.AfterColPosChangedEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnTYIncExc.Visible = false;
		}

		private void gridTYNodeVersion_AfterColRegionScroll(object sender, Infragistics.Win.UltraWinGrid.ColScrollRegionEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnTYIncExc.Visible = false;
		}

		private void gridTYNodeVersion_AfterExitEditMode(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnTYIncExc.Visible = false;
		}

		private void gridTYNodeVersion_AfterRowCollapsed(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnTYIncExc.Visible = false;
		}

		private void gridTYNodeVersion_AfterRowExpanded(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnTYIncExc.Visible = false;
		}

		private void gridTYNodeVersion_AfterRowRegionScroll(object sender, Infragistics.Win.UltraWinGrid.RowScrollRegionEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			btnTYIncExc.Visible = false;
		}

		private void gridTYTimeWeight_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			AllGrids_ClickCellButton(e, "CDR_RID", false, true);
		}

		private void gridLYTimeWeight_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			AllGrids_ClickCellButton(e, "CDR_RID", false, true);
		}

		private void gridTrendTimeWeight_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			AllGrids_ClickCellButton(e, "CDR_RID", true, true);
		}

		// BEGIN Issue 4818 stodd 12.18.2007
		private void gridTrendNodeVersion_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			AllGrids_ClickCellButton(e, "CDR_RID", true, true);
		}

		private void gridLYNodeVersion_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			AllGrids_ClickCellButton(e, "CDR_RID", false, true);
		}

		private void gridTYNodeVersion_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			AllGrids_ClickCellButton(e, "CDR_RID", false, true);
		}
		// END Issue 4818

		private void AllGrids_ClickCellButton(Infragistics.Win.UltraWinGrid.CellEventArgs e, string dateRangeRidName, bool singleDate, bool allowDynamic)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				// Begin Track #6206 - JSmith - Cannot enter mins by month
//				if (e.Cell == e.Cell.Row.Cells["IncludeButton"])
				if (e.Cell.Row.Band.Columns.Exists("IncludeButton") &&
					e.Cell == e.Cell.Row.Cells["IncludeButton"])
				// End Track #6206
				{
					//m_aRow = aRow; //m_aRow is a form-level variable and the Include column (which is hidden) needs it.

					//We need to get the size and location of the cell, and we can only
					//get that by retrieving the UIElement associated with that cell.
					//CellUIElement objCellUIElement = (CellUIElement)aCell.GetUIElement(this.grdPctContBasis.ActiveRowScrollRegion, this.grdPctContBasis.ActiveColScrollRegion);
					//if ( objCellUIElement == null ) { return; }

					////   Get the size and location of the cell
					//int left = objCellUIElement.RectInsideBorders.Location.X + this.grdPctContBasis.Location.X;
					//int top = objCellUIElement.RectInsideBorders.Location.Y + this.grdPctContBasis.Location.Y;
					//int width = objCellUIElement.RectInsideBorders.Width;
					//int height = objCellUIElement.RectInsideBorders.Height;

					////   Set the combobox's size and location equal to the cell's size and location
					//this.btnIncExc.SetBounds(left, top, width, height);

                    // Begin TT#4560 - JSmith - Can change Trend Options when in Read Only
                    if (!FunctionSecurity.AllowUpdate)
                    {
                        return;
                    }
                    // End TT#4560 - JSmith - Can change Trend Options when in Read Only

					m_includeCell = e.Cell.Row.Cells["INC_EXC_IND"];

					if (m_includeCell.Value.ToString() == "0")
					{
						e.Cell.Row.Cells["INC_EXC_IND"].Value = "1";
						e.Cell.Appearance.Image = picExclude;
						e.Cell.ButtonAppearance.Image = picExclude;
					}
					else if (m_includeCell.Value.ToString() == "1")
					{
						e.Cell.Row.Cells["INC_EXC_IND"].Value = "0";
						e.Cell.Appearance.Image = picInclude;
						e.Cell.ButtonAppearance.Image = picInclude;
					}
				}


				if (e.Cell == e.Cell.Row.Cells["DateRange"])
				{

					if (FunctionSecurity.AllowUpdate)
					{
						CalendarDateSelector frmCalDtSelector = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[] { SAB });
						frmCalDtSelector.AllowReoccurring = false;
						frmCalDtSelector.AllowDynamic = allowDynamic;
						// Begin Issue 4000 - stodd
						frmCalDtSelector.AllowDynamicToStoreOpen = false;
						// end Issue 4000
						// Begin Issue 4344 - stodd 3.26.07
						frmCalDtSelector.RestrictToSingleDate = singleDate;
						// end Issue 4344

						if (e.Cell.Row.Cells["DateRange"].Value != null &&
							e.Cell.Row.Cells["DateRange"].Value != System.DBNull.Value &&
							e.Cell.Row.Cells["DateRange"].Text.Length > 0)
						{
							frmCalDtSelector.DateRangeRID = Convert.ToInt32(e.Cell.Row.Cells[dateRangeRidName].Value, CultureInfo.CurrentUICulture);
						}
						if (midDateRangeSelectorOTSPlan.DateRangeRID != Include.UndefinedCalendarDateRange)
						{
							frmCalDtSelector.AnchorDateRangeRID = midDateRangeSelectorOTSPlan.DateRangeRID;
							// Begin issue 3208 - stodd 2/15/06
							frmCalDtSelector.AnchorDateRelativeTo = eDateRangeRelativeTo.Plan;
							// End issue 3208 - stodd 2/15/06
						}
						else
						{
							frmCalDtSelector.AnchorDate = SAB.ClientServerSession.Calendar.CurrentDate;
							// Begin issue 3208 - stodd 2/15/06
							frmCalDtSelector.AnchorDateRelativeTo = eDateRangeRelativeTo.Current;
							// End issue 3208 - stodd 2/15/06
						}
						DialogResult DateRangeResult = frmCalDtSelector.ShowDialog();
						if (DateRangeResult == DialogResult.OK)
						{
							DateRangeProfile SelectedDateRange = (DateRangeProfile)frmCalDtSelector.Tag;
							e.Cell.Value = SelectedDateRange.DisplayDate;
							//				e.Cell.Tag = SelectedDateRange;
							// for some reason have to clear the cell before it can be updated??
							if (e.Cell.Row.Cells[dateRangeRidName].Value != System.DBNull.Value)
							{
								e.Cell.Row.Cells[dateRangeRidName].Value = System.DBNull.Value;
							}
							e.Cell.Row.Cells[dateRangeRidName].Value = SelectedDateRange.Key;
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
							ChangePending = true;
							btnSave.Enabled = true;
						}
						//					frmCalDtSelector.Remove();
					}
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		private void gridTYNodeVersion_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			TyLyGrid_DragDrop(gridTYNodeVersion,e);
		}

		private void gridTYNodeVersion_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            // Begin TT#802 - JSmith - Hierarchy node image does not drag over trend tab
            Image_DragOver(sender, e);
            // End TT#802
			TyLyGrid_DragOver(gridTYNodeVersion,e);
		}

		private void gridLYNodeVersion_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			TyLyGrid_DragDrop(gridLYNodeVersion,e);
		}

		private void gridLYNodeVersion_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            // Begin TT#802 - JSmith - Hierarchy node image does not drag over trend tab
            Image_DragOver(sender, e);
            // End TT#802
			TyLyGrid_DragOver(gridLYNodeVersion,e);
		}

		private void gridTrendNodeVersion_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			TyLyGrid_DragDrop(gridTrendNodeVersion,e);
		}

		private void gridTrendNodeVersion_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            // Begin TT#802 - JSmith - Hierarchy node image does not drag over trend tab
            Image_DragOver(sender, e);
            // End TT#802
			TyLyGrid_DragOver(gridTrendNodeVersion,e);
		}

		private void TyLyGrid_DragOver(UltraGrid aGrid, System.Windows.Forms.DragEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            TreeNodeClipboardList cbList = null;
			try
			{
				// BEGIN Track #5357 stodd
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
				{
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    //if (cbp.ClipboardDataType != eClipboardDataType.HierarchyNode)
                    if (cbList.ClipboardDataType != eProfileType.HierarchyNode)
					{
						e.Effect = DragDropEffects.None;
						return;
					}
				}
				else
				{
					e.Effect = DragDropEffects.None;
					return;
				}
				// END Track #5357

				Infragistics.Win.UIElement aUIElement;

				//				Point ptParent = PointToClient(new Point(e.X, e.Y));
				//
				//				int X = ptParent.X - this.tabOTSMethod.Left - this.grpGroupLevelMethod.Left - this.tabCriteria.Left - this.pnlPercentContribution.Left  - this.grpContributionBasis.Left - aGrid.Left -lblWidth30.Height; //- grdPctContBasis.Rows[0].Height;  //-lblWidth30.Height; //- 8;
				//				int Y = ptParent.Y - this.tabOTSMethod.Top - this.grpGroupLevelMethod.Top - this.tabCriteria.Top - this.pnlPercentContribution.Top - this.grpContributionBasis.Top - aGrid.Top - (this.Height - this.ClientSize.Height) -lblWidth30.Width; //grdPctContBasis.Rows[0].Cells["Merchandise"].Column.Width; //grdPctContBasis.Rows[0].Cells["Merchandise"] //-lblWidth30.Width; //- 30;//used to be  -10
				//				int z = aGrid.DisplayLayout.Bands[0].Columns["Merchandise"].Width;
			
				//				Point realPoint = new Point(X, Y);
				
				aUIElement = aGrid.DisplayLayout.UIElement.ElementFromPoint(aGrid.PointToClient(new Point(e.X, e.Y)));

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
				
				if (aCell == aRow.Cells["Merchandise"] && FunctionSecurity.AllowUpdate)
				{
					e.Effect = DragDropEffects.All;
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

		private void TyLyGrid_DragDrop(UltraGrid aGrid, System.Windows.Forms.DragEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			HierarchyNodeProfile hnp = null;
            TreeNodeClipboardList cbList;
            HierarchyNodeClipboardList hnList;
			try
			{
				Infragistics.Win.UIElement aUIElement;

				//				Point ptParent = PointToClient(new Point(e.X, e.Y));
				//
				//				int X = ptParent.X - this.tabOTSMethod.Left - this.grpGroupLevelMethod.Left - this.tabCriteria.Left - this.pnlPercentContribution.Left - this.grpContributionBasis.Left - this.grdPctContBasis.Left - lblWidth30.Height; //- grdPctContBasis.Rows[0].Height;  //-lblWidth30.Height; //- 8;
				//				int Y = ptParent.Y - this.tabOTSMethod.Top - this.grpGroupLevelMethod.Top - this.tabCriteria.Top - this.pnlPercentContribution.Top - this.grpContributionBasis.Top - this.grdPctContBasis.Top - (this.Height - this.ClientSize.Height) - lblWidth30.Width; //- grdPctContBasis.Rows[0].Cells["Merchandise"].Column.Width; //- lblWidth30.Width; //- 30;//used to be  -10
				//				int z = aGrid.DisplayLayout.Bands[0].Columns["Merchandise"].Width;
				//
				//				Point realPoint = new Point(X, Y);
				//	
				//				aUIElement = aGrid.DisplayLayout.UIElement.ElementFromPoint(realPoint);
				aUIElement = aGrid.DisplayLayout.UIElement.ElementFromPoint(aGrid.PointToClient(new Point(e.X, e.Y)));

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
                        if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                        {
                            cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                            
                            // Begin TT#2647 - JSmith - Delays in OTS Method
                            //hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);
                            if (!_nodesByRID.TryGetValue(cbList.ClipboardProfile.Key, out hnp))
                            {
                                hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);
                                _nodesByRID.Add(cbList.ClipboardProfile.Key, hnp);
                                if (!_nodesByID.ContainsKey(hnp.NodeID))
                                {
                                    _nodesByID.Add(hnp.NodeID, hnp);
                                }
                            }
                            // End TT#2647 - JSmith - Delays in OTS Method
                        }
                        else if (e.Data.GetDataPresent(typeof(HierarchyNodeClipboardList)))
                        {
                          hnList = (HierarchyNodeClipboardList)e.Data.GetData(typeof(HierarchyNodeClipboardList));
                         
                          // Begin TT#2647 - JSmith - Delays in OTS Method
                          // hnp = SAB.HierarchyServerSession.GetNodeData(hnList.ClipboardProfile.Key, true, true);
                          if (!_nodesByRID.TryGetValue(hnList.ClipboardProfile.Key, out hnp))
                          {
                              hnp = SAB.HierarchyServerSession.GetNodeData(hnList.ClipboardProfile.Key, true, true);
                              _nodesByRID.Add(hnList.ClipboardProfile.Key, hnp);
                              if (!_nodesByID.ContainsKey(hnp.NodeID))
                              {
                                  _nodesByID.Add(hnp.NodeID, hnp);
                              }
                          }
                            // End TT#2647 - JSmith - Delays in OTS Method
                        }
                        else
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
                        }

                        if (hnp != null)
                        {
                            try
                            {
                                aRow.Cells["MERCH_TYPE"].Value = (int)eMerchandiseType.Node;
                                aRow.Cells["MERCH_PH_RID"].Value = Include.NoRID;
                                aRow.Cells["MERCH_PHL_SEQUENCE"].Value = 0;
                                aRow.Cells["MERCH_OFFSET"].Value = 0;

                                bool nodeFound = false;
                                foreach (Infragistics.Win.ValueListItem vli in aGrid.DisplayLayout.ValueLists["Merchandise"].ValueListItems)
                                {
                                    if (vli.DisplayText == hnp.Text)
                                    {
                                        nodeFound = true;
                                        break;
                                    }
                                }
                                if (!nodeFound)
                                {
                                    Infragistics.Win.ValueListItem vli = new Infragistics.Win.ValueListItem();
                                    vli.DataValue = aGrid.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Count;
                                    vli.DisplayText = hnp.Text;
                                    vli.Tag = Convert.ToString(hnp.Key, CultureInfo.CurrentUICulture);
                                    aGrid.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Add(vli);
                                }
                                _dragAndDrop = true;
                                aCell.Value = hnp.Text;
                                aRow.Cells["HN_RID"].Value = hnp.Key;
                                _dragAndDrop = false;

                                // Begin TT#2647 - JSmith - Delays in OTS Method
                                //int sgl = (int)cbxStoreGroupLevel.SelectedValue;
                                //if (sgl == _defaultStoreGroupLevelRid)
                                //    UpdateSetsUsingDefault();
                                // End TT#2647 - JSmith - Delays in OTS Method

                                // Begin TT#2647 - JSmith - Delays in OTS Method
                                if (FormLoaded)
                                {
                                    ChangePending = true;
                                }
                                // End TT#2647 - JSmith - Delays in OTS Method
                            }
                            // BEGIN issue 5357 stodd 7.24.2008
                            catch (BadDataInClipboardException)
                            {
                                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
                                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            catch
                            {
                                throw;
                            }
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

		private void gridTYNodeVersion_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (CheckInsertCondition(gridTYNodeVersion, e) == false)
			{
				e.Cancel = true;
				return;
			}

			try
			{
				this.Cursor = Cursors.WaitCursor;

				// BEGIN MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
				int sglRid = Convert.ToInt32(this.cbxStoreGroupLevel.SelectedValue,CultureInfo.CurrentUICulture);
				// END MID Track #5954

				//Handle the row Sequence (pk for the grid)
				DataRow basisRow = (DataRow)_dtSourceTYNode.NewRow();
				if (_dtSourceTYNode.Rows.Count == 0)
				{
					basisRow["SEQ"] = 1;
				}
				else
				{
					// BEGIN MID ISSUE #2833 - stodd
					int seq=1;
					int lastIndex = _dtSourceTYNode.Rows.Count-1;
					for (int i=lastIndex;i>-1;i--)
					{
						DataRow lastRow = _dtSourceTYNode.Rows[i];
						if (lastRow.RowState != DataRowState.Deleted)
						{
							// BEGIN MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
							if ((int)lastRow["SGL_RID"] == sglRid)
							{
								if (lastRow["SEQ"] == DBNull.Value)
									seq = 1;
								else
									seq = Convert.ToInt32(lastRow["SEQ"], CultureInfo.CurrentUICulture) + 1;
								break;
							}
							// END MID Track #5954
						}
					}
					basisRow["SEQ"] = seq;
					// End MID ISSUE #2833 - stodd
				}
				basisRow["INC_EXC_IND"] = "0";
				basisRow["SGL_RID"] = sglRid; //MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
				basisRow["TYLY_TYPE_ID"] = (int)eTyLyType.TyLy;
				basisRow["WEIGHT"] = 1;

				_dtSourceTYNode.Rows.Add(basisRow);

				//Set the active row to this newly added Basis row.
				gridTYNodeVersion.ActiveRow = gridTYNodeVersion.Rows[gridTYNodeVersion.Rows.Count - 1];

                // Begin TT#2647 - JSmith - Delays in OTS Method
                if (FormLoaded)
                {
                    ChangePending = true;
                }
                // End TT#2647 - JSmith - Delays in OTS Method

				//Since we've already added the necessary information in the underlying
				//datatable, we want to cancel out because if we don't, the grid will
				//add another blank row (in addition to the row we just added to the datatable).
				e.Cancel = true;

				this.Cursor = Cursors.Default;
			}
			catch
			{}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

//		private void gridTYTimeWeight_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
//		{
//			if (CheckInsertCondition_DateRange(gridTYTimeWeight, e) == false)
//			{
//				e.Cancel = true;
//				return;
//			}
//
//			try
//			{
//				this.Cursor = Cursors.WaitCursor;
//
//				//Handle the row Sequence (pk for the grid)
//				DataRow basisRow = (DataRow)_dtSourceTYTime.NewRow();
//				if (_dtSourceTYTime.Rows.Count == 0)
//				{
//					basisRow["SEQ"] = 1;
//				}
//				else
//				{
//					// BEGIN MID ISSUE #2833 - stodd
//					int seq=1;
//					int lastIndex = _dtSourceTYTime.Rows.Count-1;
//					for (int i=lastIndex;i>-1;i--)
//					{
//						DataRow lastRow = _dtSourceTYTime.Rows[i];
//						if (lastRow.RowState != DataRowState.Deleted)
//						{
//							if (lastRow["SEQ"] == DBNull.Value)
//								seq = 1;
//							else
//								seq = Convert.ToInt32(lastRow["SEQ"], CultureInfo.CurrentUICulture) + 1;
//							break;
//						}
//					}
//					basisRow["SEQ"] = seq;
//					// End MID ISSUE #2833 - stodd
//				}
//				basisRow["INC_EXC_IND"] = "0";
//				basisRow["WEIGHT"] = 1;
//				basisRow["SGL_RID"] = Convert.ToInt32(this.cboStoreGroupLevel.SelectedValue,CultureInfo.CurrentUICulture);
//				basisRow["TYLY_TYPE_ID"] = (int)eTyLyType.TyLy;
//
//				_dtSourceTYTime.Rows.Add(basisRow);
//
//				//Set the active row to this newly added Basis row.
//				gridTYTimeWeight.ActiveRow = gridTYTimeWeight.Rows[gridTYTimeWeight.Rows.Count - 1];
//
//				//Since we've already added the necessary information in the underlying
//				//datatable, we want to cancel out because if we don't, the grid will
//				//add another blank row (in addition to the row we just added to the datatable).
//				e.Cancel = true;
//
//				this.Cursor = Cursors.Default;
//			}
//			catch
//			{}
//			finally
//			{
//				this.Cursor = Cursors.Default;
//			}
//		}

		private void gridLYNodeVersion_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (CheckInsertCondition(gridLYNodeVersion, e) == false)
			{
				e.Cancel = true;
				return;
			}
		
			try
			{
				this.Cursor = Cursors.WaitCursor;

				// BEGIN MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
				int sglRid = Convert.ToInt32(this.cbxStoreGroupLevel.SelectedValue,CultureInfo.CurrentUICulture);
				// END MID Track #5954

				//Handle the row Sequence (pk for the grid)
				DataRow basisRow = (DataRow)_dtSourceLYNode.NewRow();
				if (_dtSourceLYNode.Rows.Count == 0)
				{
					basisRow["SEQ"] = 1;
				}
				else
				{
					// BEGIN MID ISSUE #2833 - stodd
					int seq=1;
					int lastIndex = _dtSourceLYNode.Rows.Count-1;
					for (int i=lastIndex;i>-1;i--)
					{
						DataRow lastRow = _dtSourceLYNode.Rows[i];
						if (lastRow.RowState != DataRowState.Deleted)
						{
							// BEGIN MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
							if ((int)lastRow["SGL_RID"] == sglRid)
							{
								if (lastRow["SEQ"] == DBNull.Value)
									seq = 1;
								else
									seq = Convert.ToInt32(lastRow["SEQ"], CultureInfo.CurrentUICulture) + 1;
								break;
							}
							// END MID Track #5954
						}
					}
					basisRow["SEQ"] = seq;
					// BEGIN MID ISSUE #2833 - stodd
				}
				basisRow["INC_EXC_IND"] = "0";
				basisRow["SGL_RID"] = sglRid; //MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
				basisRow["TYLY_TYPE_ID"] = (int)eTyLyType.AlternateLy;
				basisRow["WEIGHT"] = 1;  //Issue 4818

	
				_dtSourceLYNode.Rows.Add(basisRow);

				//Set the active row to this newly added Basis row.
				gridLYNodeVersion.ActiveRow = gridLYNodeVersion.Rows[gridLYNodeVersion.Rows.Count - 1];

				//Since we've already added the necessary information in the underlying
				//datatable, we want to cancel out because if we don't, the grid will
				//add another blank row (in addition to the row we just added to the datatable).
				e.Cancel = true;

				this.Cursor = Cursors.Default;
			}
			catch
			{}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

//		private void gridLYTimeWeight_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
//		{
//			if (CheckInsertCondition_DateRange(gridLYTimeWeight, e) == false)
//			{
//				e.Cancel = true;
//				return;
//			}
//		
//			try
//			{
//				this.Cursor = Cursors.WaitCursor;
//
//
//				//Handle the row Sequence (pk for the grid)
//				DataRow basisRow = (DataRow)_dtSourceLYTime.NewRow();
//				if (_dtSourceLYTime.Rows.Count == 0)
//				{
//					basisRow["SEQ"] = 1;
//				}
//				else
//				{
//					// BEGIN MID ISSUE #2833 - stodd
//					int seq=1;
//					int lastIndex = _dtSourceLYTime.Rows.Count-1;
//					for (int i=lastIndex;i>-1;i--)
//					{
//						DataRow lastRow = _dtSourceLYTime.Rows[i];
//						if (lastRow.RowState != DataRowState.Deleted)
//						{
//							if (lastRow["SEQ"] == DBNull.Value)
//								seq = 1;
//							else
//								seq = Convert.ToInt32(lastRow["SEQ"], CultureInfo.CurrentUICulture) + 1;
//							break;
//						}
//					}
//					basisRow["SEQ"] = seq;
//					// BEGIN MID ISSUE #2833 - stodd
//				}
//				basisRow["INC_EXC_IND"] = "0";
//				basisRow["WEIGHT"] = 1;
//				basisRow["SGL_RID"] = Convert.ToInt32(this.cboStoreGroupLevel.SelectedValue,CultureInfo.CurrentUICulture);
//				basisRow["TYLY_TYPE_ID"] = (int)eTyLyType.AlternateLy;
//
//				_dtSourceLYTime.Rows.Add(basisRow);
//
//				//Set the active row to this newly added Basis row.
//				gridLYTimeWeight.ActiveRow = gridLYTimeWeight.Rows[gridLYTimeWeight.Rows.Count - 1];
//
//				//Since we've already added the necessary information in the underlying
//				//datatable, we want to cancel out because if we don't, the grid will
//				//add another blank row (in addition to the row we just added to the datatable).
//				e.Cancel = true;
//
//				this.Cursor = Cursors.Default;
//			}
//			catch
//			{}
//			finally
//			{
//				this.Cursor = Cursors.Default;
//			}
//		}

		private void gridTrendNodeVersion_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (CheckInsertCondition(gridTrendNodeVersion, e) == false)
			{
				e.Cancel = true;
				return;
			}
		
			try
			{
				this.Cursor = Cursors.WaitCursor;

				// BEGIN MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
				int sglRid = Convert.ToInt32(this.cbxStoreGroupLevel.SelectedValue,CultureInfo.CurrentUICulture);
				// END MID Track #5954

				//Handle the row Sequence (pk for the grid)
				DataRow basisRow = (DataRow)_dtSourceTrendNode.NewRow();
				if (_dtSourceTrendNode.Rows.Count == 0)
				{
					basisRow["SEQ"] = 1;
				}
				else
				{
					// BEGIN MID ISSUE #2833 - stodd
					int seq=1;
					int lastIndex = _dtSourceTrendNode.Rows.Count-1;
					for (int i=lastIndex;i>-1;i--)
					{
						DataRow lastRow = _dtSourceTrendNode.Rows[i];
						if (lastRow.RowState != DataRowState.Deleted)
						{
							// BEGIN MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
							if ((int)lastRow["SGL_RID"] == sglRid)
							{
								if (lastRow["SEQ"] == DBNull.Value)
									seq = 1;
								else
									seq = Convert.ToInt32(lastRow["SEQ"], CultureInfo.CurrentUICulture) + 1;
								break;
							}
							// END MID Track #5954
						}
					}
					basisRow["SEQ"] = seq;
					// BEGIN MID ISSUE #2833 - stodd
				}
				basisRow["INC_EXC_IND"] = "0";
				basisRow["SGL_RID"] = sglRid; //MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
                //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
                if (!this.cbxProjCurrWkSales.Checked)
                {
                    basisRow["TYLY_TYPE_ID"] = (int)eTyLyType.AlternateApplyTo;  // Issue 4344 - couldn't insert new row
                }
                else
                {
                    basisRow["TYLY_TYPE_ID"] = (int)eTyLyType.ProjectCurrWkSales;  // Issue 4344 - couldn't insert new row
                }
                //END TT#43 - MD - DOConnell - Projected Sales Enhancement
				basisRow["WEIGHT"] = 1;  //Issue 4818
	
				_dtSourceTrendNode.Rows.Add(basisRow);

				//Set the active row to this newly added Basis row.
				gridTrendNodeVersion.ActiveRow = gridTrendNodeVersion.Rows[gridTrendNodeVersion.Rows.Count - 1];

				//Since we've already added the necessary information in the underlying
				//datatable, we want to cancel out because if we don't, the grid will
				//add another blank row (in addition to the row we just added to the datatable).
				e.Cancel = true;

				this.Cursor = Cursors.Default;
			}
			catch
			{}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

//		private void gridTrendTimeWeight_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
//		{
//			if (CheckInsertCondition_DateRange(gridTrendTimeWeight, e) == false)
//			{
//				e.Cancel = true;
//				return;
//			}
//		
//			try
//			{
//				this.Cursor = Cursors.WaitCursor;
//
//
//				//Handle the row Sequence (pk for the grid)
//				DataRow basisRow = (DataRow)_dtSourceTrendTime.NewRow();
//				if (_dtSourceTrendTime.Rows.Count == 0)
//				{
//					basisRow["SEQ"] = 1;
//				}
//				else
//				{
//					// BEGIN MID ISSUE #2833 - stodd
//					int seq=1;
//					int lastIndex = _dtSourceTrendTime.Rows.Count-1;
//					for (int i=lastIndex;i>-1;i--)
//					{
//						DataRow lastRow = _dtSourceTrendTime.Rows[i];
//						if (lastRow.RowState != DataRowState.Deleted)
//						{
//							if (lastRow["SEQ"] == DBNull.Value)
//								seq = 1;
//							else
//								seq = Convert.ToInt32(lastRow["SEQ"], CultureInfo.CurrentUICulture) + 1;
//							break;
//						}
//					}
//					basisRow["SEQ"] = seq;
//					// BEGIN MID ISSUE #2833 - stodd
//				}
//				basisRow["INC_EXC_IND"] = "0";
//				basisRow["WEIGHT"] = 1;
//				basisRow["SGL_RID"] = Convert.ToInt32(this.cboStoreGroupLevel.SelectedValue,CultureInfo.CurrentUICulture);
//				basisRow["TYLY_TYPE_ID"] = (int)eTyLyType.AlternateApplyTo;
//
//				_dtSourceTrendTime.Rows.Add(basisRow);
//
//
//				//Set the active row to this newly added Basis row.
//				// Begin ISSUE 4344 - stodd
//				gridTrendTimeWeight.ActiveRow = gridTrendTimeWeight.Rows[gridTrendTimeWeight.Rows.Count - 1];
//				//End ISSUE 4344
//				//Since we've already added the necessary information in the underlying
//				//datatable, we want to cancel out because if we don't, the grid will
//				//add another blank row (in addition to the row we just added to the datatable).
//				e.Cancel = true;
//
//				this.Cursor = Cursors.Default;
//			}
//			catch
//			{}
//			finally
//			{
//				this.Cursor = Cursors.Default;
//			}
//		}

		private void gridTYNodeVersion_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (e.Cell == e.Cell.Row.Cells["Merchandise"])
			{
				if (FormLoaded && !_dragAndDrop)
				{
					if (_merchValChanged)
					{
						_merchValChanged = false;
						return;
					}
					int rowseq = -1;
					//============================================================================================= 
					// This code is catching where a value is selected from the merchandise drop down as opposed 
					// to one being entered. If the value is not numeric, it looks it up as a product.
					//=============================================================================================
					if (_MerchCellListClose)
					{
						try
						{
							rowseq = Convert.ToInt32(e.Cell.Value, CultureInfo.CurrentUICulture);
						}
						catch (System.FormatException)
						{
							rowseq = -1;
						}
						catch (System.InvalidCastException)
						{
							rowseq = -1;
						}
						finally
						{
							_MerchCellListClose = false;
						}
					}

					if (rowseq == -1)
					{
						string productID = e.Cell.Value.ToString().Trim();
						if (productID.Length > 0)
						{
							_nodeRID = GetNodeText(ref productID);
							if (_nodeRID == Include.NoRID)
							{
								string errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
									productID );
								MessageBox.Show(errorMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);		
							}
							else 
							{
								_merchValChanged = true;
								e.Cell.Value = productID;
								e.Cell.Row.Cells["HN_RID"].Value = _nodeRID;
								e.Cell.Row.Cells["MERCH_TYPE"].Value = eMerchandiseType.Node;
								e.Cell.Row.Cells["MERCH_PH_RID"].Value = Include.NoRID;
								e.Cell.Row.Cells["MERCH_PHL_SEQUENCE"].Value = 0;
								e.Cell.Row.Cells["MERCH_OFFSET"].Value = 0;
							}
						}
					}
					else
					{
						e.Cell.Row.Cells["HN_RID"].Value = 0;  // Issue 5428 Stodd
						e.Cell.Row.Cells["MERCH_TYPE"].Value = eMerchandiseType.SameNode;
						e.Cell.Row.Cells["MERCH_PH_RID"].Value = Include.NoRID;
						e.Cell.Row.Cells["MERCH_PHL_SEQUENCE"].Value = 0;
						e.Cell.Row.Cells["MERCH_OFFSET"].Value = 0;

					}
				}
			}
			else if (e.Cell == e.Cell.Row.Cells["Version"])
			{
				int selectedIndex = gridTYNodeVersion.DisplayLayout.ValueLists["Version"].SelectedIndex;

				if (selectedIndex != -1)
					e.Cell.Row.Cells["FV_RID"].Value = Convert.ToInt32(gridTYNodeVersion.DisplayLayout.ValueLists["Version"].ValueListItems[selectedIndex].DataValue, CultureInfo.CurrentUICulture);
			}

            // Begin TT#2647 - JSmith - Delays in OTS Method
            //// Begin Issue 3816 - stodd
            //int sgl = (int)cbxStoreGroupLevel.SelectedValue;
            //if (sgl == _defaultStoreGroupLevelRid)
            //    UpdateSetsUsingDefault();
            //// End issue 3816
            // End TT#2647 - JSmith - Delays in OTS Method
		}
		
		 
		private void gridLYNodeVersion_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (e.Cell == e.Cell.Row.Cells["Merchandise"])
			{
				if (FormLoaded && !_dragAndDrop)
				{
					if (_merchValChanged)
					{
						_merchValChanged = false;
						return;
					}
	
					int rowseq = -1;
					//============================================================================================= 
					// This code is catching where a value is selected from the merchandise drop down as opposed 
					// to one being entered. If the value is not numeric, it looks it up as a product.
					//=============================================================================================
					if (_MerchCellListClose)
					{
						try
						{
							rowseq = Convert.ToInt32(e.Cell.Value, CultureInfo.CurrentUICulture);
						}
						catch (System.FormatException)
						{
							rowseq = -1;
						}
						catch (System.InvalidCastException)
						{
							rowseq = -1;
						}
						finally
						{
							_MerchCellListClose = false;
						}
					}

					if (rowseq == -1)
					{
						string productID = e.Cell.Value.ToString().Trim();
						if (productID.Length > 0)
						{
							_nodeRID = GetNodeText(ref productID);
							if (_nodeRID == Include.NoRID)
							{
								string errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
									productID );
								MessageBox.Show(errorMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);		
							}
							else 
							{
								_merchValChanged = true;
								e.Cell.Value = productID;
								e.Cell.Row.Cells["HN_RID"].Value = _nodeRID;
								e.Cell.Row.Cells["MERCH_TYPE"].Value = eMerchandiseType.Node;
								e.Cell.Row.Cells["MERCH_PH_RID"].Value = Include.NoRID;
								e.Cell.Row.Cells["MERCH_PHL_SEQUENCE"].Value = 0;
								e.Cell.Row.Cells["MERCH_OFFSET"].Value = 0;
							}
						}
					}
					else
					{
						e.Cell.Row.Cells["HN_RID"].Value = 0; // TT# 178 - stodd - OTS Forecast Review Ty/LY when low level class receive error
						e.Cell.Row.Cells["MERCH_TYPE"].Value = eMerchandiseType.SameNode;
						e.Cell.Row.Cells["MERCH_PH_RID"].Value = Include.NoRID;
						e.Cell.Row.Cells["MERCH_PHL_SEQUENCE"].Value = 0;
						e.Cell.Row.Cells["MERCH_OFFSET"].Value = 0;

					}
				}
			}
			else if (e.Cell == e.Cell.Row.Cells["Version"])
			{
				int selectedIndex = gridLYNodeVersion.DisplayLayout.ValueLists["Version"].SelectedIndex;

				if (selectedIndex != -1)
					e.Cell.Row.Cells["FV_RID"].Value = Convert.ToInt32(gridLYNodeVersion.DisplayLayout.ValueLists["Version"].ValueListItems[selectedIndex].DataValue, CultureInfo.CurrentUICulture);
			}

            // Begin TT#2647 - JSmith - Delays in OTS Method
            //// Begin Issue 3816 - stodd
            //int sgl = (int)cbxStoreGroupLevel.SelectedValue;
            //if (sgl == _defaultStoreGroupLevelRid)
            //    UpdateSetsUsingDefault();
            //// End issue 3816
            // End TT#2647 - JSmith - Delays in OTS Method
		}

		private void gridTrendNodeVersion_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (e.Cell == e.Cell.Row.Cells["Merchandise"])
			{
				if (FormLoaded && !_dragAndDrop)
				{
					if (_merchValChanged)
					{
						_merchValChanged = false;
						return;
					}
					int rowseq = -1;
					//============================================================================================= 
					// This code is catching where a value is selected from the merchandise drop down as opposed 
					// to one being entered. If the value is not numeric, it looks it up as a product.
					//=============================================================================================
					if (_MerchCellListClose)
					{
						try
						{
							rowseq = Convert.ToInt32(e.Cell.Value, CultureInfo.CurrentUICulture);
						}
						catch (System.FormatException)
						{
							rowseq = -1;
						}
						catch (System.InvalidCastException)
						{
							rowseq = -1;
						}
						finally
						{
							_MerchCellListClose = false;
						}
					}

					if (rowseq == -1)
					{
						string productID = e.Cell.Value.ToString().Trim();
						if (productID.Length > 0)
						{
							_nodeRID = GetNodeText(ref productID);
							if (_nodeRID == Include.NoRID)
							{
								string errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
									productID );
								MessageBox.Show(errorMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);		
							}
							else 
							{
								_merchValChanged = true;
								e.Cell.Value = productID;
								e.Cell.Row.Cells["HN_RID"].Value = _nodeRID;
								e.Cell.Row.Cells["MERCH_TYPE"].Value = eMerchandiseType.Node;
								e.Cell.Row.Cells["MERCH_PH_RID"].Value = Include.NoRID;
								e.Cell.Row.Cells["MERCH_PHL_SEQUENCE"].Value = 0;
								e.Cell.Row.Cells["MERCH_OFFSET"].Value = 0;
							}
						}
					}
					else
					{
						e.Cell.Row.Cells["HN_RID"].Value = 0; // TT# 171 - stodd - OTS Forecast Review Ty/LY when low level class receive error
						e.Cell.Row.Cells["MERCH_TYPE"].Value = eMerchandiseType.SameNode;
						e.Cell.Row.Cells["MERCH_PH_RID"].Value = Include.NoRID;
						e.Cell.Row.Cells["MERCH_PHL_SEQUENCE"].Value = 0;
						e.Cell.Row.Cells["MERCH_OFFSET"].Value = 0;

					}
					
				}
			}
			else if (e.Cell == e.Cell.Row.Cells["Version"])
			{
				int selectedIndex = gridTrendNodeVersion.DisplayLayout.ValueLists["Version"].SelectedIndex;

				if (selectedIndex != -1)
					e.Cell.Row.Cells["FV_RID"].Value = Convert.ToInt32(gridTrendNodeVersion.DisplayLayout.ValueLists["Version"].ValueListItems[selectedIndex].DataValue, CultureInfo.CurrentUICulture);
			}
            // Begin TT#2647 - JSmith - Delays in OTS Method
            //// Begin Issue 3816 - stodd
            //int sgl = (int)cbxStoreGroupLevel.SelectedValue;
            //if (sgl == _defaultStoreGroupLevelRid)
            //    UpdateSetsUsingDefault();
            //// End issue 3816
            // End TT#2647 - JSmith - Delays in OTS Method
		}

		private void cbxAltLY_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (!cbxAltLY.Checked && gridLYNodeVersion.Rows.Count > 0)
			{
				// Begin Issue 4218 stodd
				DialogResult dr = DialogResult.Yes;

				if (!_loadingSet)
				{
					string msg = "Unchecking Alternate wil reload This Year's data. Do you wish to proceed?";  
					dr=MessageBox.Show(msg, MIDText.GetTextOnly(eMIDTextCode.lbl_Method), MessageBoxButtons.YesNo);
				}
				// End Issue 4218
				if (dr==DialogResult.Yes) 
				{
					SyncLastYearGrids();
				}
				else
				{
					cbxAltLY.Checked = true;
					return;
				}
			}
			// Begin Issue 4218
			// If useDefault is checked, these grid should always be disabled
			if (this.chkUseDefault.Checked)
			{
				this.gridLYNodeVersion.Enabled = false;
				this.rdoEqualizeNoLY.Enabled = false;
				this.rdoEqualizeYesLY.Enabled = false;
			}
			else
			{
				this.gridLYNodeVersion.Enabled = cbxAltLY.Checked;
				this.rdoEqualizeYesLY.Enabled = cbxAltLY.Checked;
				this.rdoEqualizeNoLY.Enabled = cbxAltLY.Checked;
			}
			// end Issue 4218
			_GLFProfile.LY_Alt_IND = cbxAltLY.Checked;

            // Begin TT#2647 - JSmith - Delays in OTS Method
            if (FormLoaded)
            {
                ChangePending = true;
            }
            // End TT#2647 - JSmith - Delays in OTS Method
		}

		private void cbxAltTrend_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (!cbxAltTrend.Checked && gridTrendNodeVersion.Rows.Count > 0)
			{
				// Begin Issue 4218 stodd
				DialogResult dr = DialogResult.Yes;

				if (!_loadingSet)
				{
					string msg = "Unchecking Alternate wil reload This Year's data. Do you wish to proceed?";  
					dr=MessageBox.Show(msg, MIDText.GetTextOnly(eMIDTextCode.lbl_Method), MessageBoxButtons.YesNo);
				}
				// End Issue 4218				
				if (dr==DialogResult.Yes) 
				{
					SyncApplyTrendGrids();
				}
				else
				{
					cbxAltTrend.Checked = true;
					return;
				}
			}
			// Begin Issue 4218
			// If useDefault is checked, these grid should always be disabled
			if (this.chkUseDefault.Checked)
			{
				this.gridTrendNodeVersion.Enabled = false;
				this.rdoEqualizeNoTrend.Enabled = false;
				this.rdoEqualizeYesTrend.Enabled = false;
			}
			else
			{
				this.gridTrendNodeVersion.Enabled = cbxAltTrend.Checked;
				this.rdoEqualizeNoTrend.Enabled = cbxAltTrend.Checked;
				this.rdoEqualizeYesTrend.Enabled = cbxAltTrend.Checked;
			}
			// End Issue 4218
			_GLFProfile.Trend_Alt_IND = cbxAltTrend.Checked;
		}

        private void cbxProjCurrWkSales_CheckedChanged(object sender, System.EventArgs e)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            _GLFProfile.Proj_Curr_Wk_Sales_IND = cbxProjCurrWkSales.Checked;

            // Begin TT#2647 - JSmith - Delays in OTS Method
            if (FormLoaded)
            {
                if (!_storeGroupLevelChanged)
                {
                    ChangePending = true;
                }
            }
            // End TT#2647 - JSmith - Delays in OTS Method

        }

		private void txtOTSHNDesc_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			_previousHierNode = (HierarchyNodeProfile)((MIDTag)((TextBox)sender).Tag).MIDTagData;
//            string productID; 
//            // Begin MID Issue 3443/3494 - stodd
//            if (txtOTSHNDesc.Modified)
//                // End MID Issue 3443/3494 - stodd
//            {
//                if (txtOTSHNDesc.Text.Trim().Length > 0)
//                {
//                    productID = txtOTSHNDesc.Text.Trim();
//                    _nodeRID = GetNodeText(ref productID);
//                    if (_nodeRID == Include.NoRID)
//                        MessageBox.Show(productID + " is not valid; please enter or drag and drop a node from the tree");
//                    else 
//                    {
//                        //Begin Track #5858 - KJohnson - Validating store security only
//                        txtOTSHNDesc.Text = productID;
//                        ((MIDTag)(txtOTSHNDesc.Tag)).MIDTagData = _nodeRID;
//                        //End Track #5858

//                        // BEGIN Issue 4858 stodd
//                        _OTSPlanMethod.Plan_HN_RID = _nodeRID; 
//                        // END Issue 4858
//                        // BEGIN Issue 4858 stodd 11.2.2007
//                        bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
//                        base.ApplyCanUpdate(canUpdate);
//                        //Begin Track #5858 - JSmith - Validating store security only
//                        //base.ValidatePlanNodeSecurity(txtOTSHNDesc);
//                        base.ValidateStorePlanNodeSecurity(txtOTSHNDesc);
//                        //End Track #5858
//                        // END Issue 4858

//                        // Begin MID Issue 2612 - stodd
//                        // Done primarily to be sure the user has authority to update the new node 
//                        //ApplySecurity();
//                        // End MID Issue 2612

//                        //Begin Track #4371 - JSmith - Multi-level forecasting.
//                        //Begin Track #5378 - color and size not qualified
////						HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_nodeRID, false);
//                        HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_nodeRID, true, true);
//                        //End Track #5378
//                        if (chkLowLevels.Enabled && chkLowLevels.Checked)
//                        {
//                            _OTSPlanMethod.LowlevelExcludeList.Clear();
//                            PopulateLowLevels(hnp);
//                        }
//                        else
//                        {
//                            chkHighLevel.Enabled = true;
//                            chkHighLevel.Checked = true;
//                            chkLowLevels.Enabled = true;
//                            chkLowLevels.Checked = false;
//                        }
//                        //End Track #4371
//                    }
//                }
//                else
//                {
//                    this.gridStockMinMax.DataSource = null;

//                    //Begin Track #5858 - KJohnson - Validating store security only
//                    ((MIDTag)(txtOTSHNDesc.Tag)).MIDTagData = null;
//                    //End Track #5858
//                }
//            }
		}

        private void txtOTSHNDesc_Validated(object sender, EventArgs e)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				HierarchyNodeProfile hnp = null;
				// Begin TT#1609 - STodd - Object Reference Error when changing data in an existing OTS method
				int originalHNRID = _OTSPlanMethod.Plan_HN_RID;
				DialogResult dr = DialogResult.Yes;
				//if (_OTSPlanMethod.LowLevelsInd)
				if (chkLowLevels.Checked)
				{
					dr = MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_ConfirmReplaceMerchandise),
						MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanMethod), MessageBoxButtons.YesNo);

					if (dr == DialogResult.No)
					{
						hnp = _previousHierNode;
						if (hnp == null)
						{
							txtOTSHNDesc.Text = string.Empty;
							((MIDTag)txtOTSHNDesc.Tag).MIDTagData = null;
						}
						else
						{
							txtOTSHNDesc.Text = hnp.Text;
							((MIDTag)txtOTSHNDesc.Tag).MIDTagData = hnp;
						}
						return;
					}
				}
				// End TT#1609

                //Begin Track #5858 - KJohnson- Validating store security only
                if ((((TextBox)sender).Text.Trim() == string.Empty) && (((TextBox)sender).Tag != null))
                {
                    this.gridStockMinMax.DataSource = null;
					chkLowLevels.Checked = false;	// TT#1609 - stodd - null ref error
                }
                else
                {
                    hnp = (HierarchyNodeProfile)((MIDTag)((TextBox)sender).Tag).MIDTagData;
                    _nodeRID = hnp.Key;

                    _OTSPlanMethod.Plan_HN_RID = _nodeRID;

                    ChangePending = true;
                    ApplySecurity();

                    //// BEGIN Issue 4858 stodd
                    //_OTSPlanMethod.Plan_HN_RID = _nodeRID;
                    //// END Issue 4858
                    //// BEGIN Issue 4858 stodd 11.2.2007
                    //bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
                    //base.ApplyCanUpdate(canUpdate);
                    ////Begin Track #5858 - JSmith - Validating store security only
                    ////base.ValidatePlanNodeSecurity(txtOTSHNDesc);
                    //base.ValidateStorePlanNodeSecurity(txtOTSHNDesc);
                    ////End Track #5858
                    //// END Issue 4858

                    //// Begin MID Issue 2612 - stodd
                    //// Done primarily to be sure the user has authority to update the new node 
                    ////ApplySecurity();
                    //// End MID Issue 2612

                    //Begin Track #4371 - JSmith - Multi-level forecasting.
                    if (chkLowLevels.Enabled && chkLowLevels.Checked)
                    {
                        RemoveOverrideLowLevelModel();  // TT#4227 - JSmith - Changing merchandise in method when custom override low level model is defined results in error when attempting to display model

                        _OTSPlanMethod.LowlevelOverrideList.Clear();
                        PopulateLowLevels(hnp);
                    }
                    else
                    {
                        chkHighLevel.Enabled = true;
                        chkHighLevel.Checked = true;
                        chkLowLevels.Enabled = true;
                        chkLowLevels.Checked = false;
                    }
                    //End Track #4371

					// Begin TT#1609 - STodd - Object Reference Error when changing data in an existing OTS method
					if (originalHNRID != Include.NoRID &&
						originalHNRID != _OTSPlanMethod.Plan_HN_RID)
					{
						UpdateGLFPNodes(originalHNRID, _OTSPlanMethod.Plan_HN_RID);
						if (chkLowLevels.Checked)
						{
							chkLowLevels.Checked = false;
							chkLowLevels.Enabled = true;
						}
					}
					//End Track #1609
                }
                //End Track #5858
            }
            catch (Exception err)
            {
                HandleException(err);
            }
        }

		private void txtOTSHNDesc_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		private int GetNodeText(ref string aProductID)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			string desc = string.Empty;
			try
			{
				string productID = aProductID;
				string[] pArray = productID.Split(new char[] {'['});
				productID = pArray[0].Trim(); 
				//				HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(productID);
				HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
				EditMsgs em = new EditMsgs();
				//Begin Track #5378 - color and size not qualified
//				HierarchyNodeProfile hnp =  hm.NodeLookup(ref em, productID, false);
				
                // Begin TT#2647 - JSmith - Delays in OTS Method
                //HierarchyNodeProfile hnp =  hm.NodeLookup(ref em, productID, false, true);
                HierarchyNodeProfile hnp = null;
                if (!_nodesByID.TryGetValue(productID, out hnp))
                {
                    hnp = hm.NodeLookup(ref em, productID, false, true);
                    _nodesByID.Add(productID, hnp);
                    if (!_nodesByRID.ContainsKey(hnp.Key))
                    {
                        _nodesByRID.Add(hnp.Key, hnp);
                    }
                }
                // End TT#2647 - JSmith - Delays in OTS Method
				//End Track #5378
				if (hnp.Key == Include.NoRID)
					return Include.NoRID;
				else 
				{
					aProductID =  hnp.Text;
					return hnp.Key;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
				return Include.NoRID;
			}
		}

		private void tabTYLYTrend_SelectedIndexChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (this.tabTYLYTrend.SelectedTab.Name != _currentTYLYTabPage.Name)
			{
				if (!ValidBasis())
				{
					_errors = null;
					string text = MIDText.GetTextOnly(eMIDTextCode.msg_ErrorsFoundReviewCorrect);
					MessageBox.Show(text);
				}
				else
				{
					_currentTYLYTabPage = this.tabTYLYTrend.SelectedTab;
				}
			}
		}

		private bool ValidBasis()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
  			bool isValid = true;
			if (tabSetMethod.SelectedIndex == 0)	// Issue 5544
			{
			switch (Convert.ToInt32(this.cboFuncType.SelectedValue,CultureInfo.CurrentUICulture))
			{
				case (int)eGroupLevelFunctionType.PercentContribution:
					if (!ValidPctContribution())
						isValid = false;
					break;
				case (int)eGroupLevelFunctionType.TyLyTrend:
				switch (_currentTYLYTabPage.Name)
				{
					case "tabPageTY":
						if (!ValidTabTYLYTrend(gridTYNodeVersion, rdoEqualizeYesTY.Checked))
						{
							isValid = false;
                        //TT#741 - Begin - MD - ComboBox NullReference Exception red icon not displayed - RBeck 
                            this.tabOTSMethod.SelectedIndex = 1;
                            this.tabSetMethod.SelectedIndex = 0;
                        //TT#741 - End - MD - ComboBox NullReference Exception red icon not displayed - RBeck  
							this.tabTYLYTrend.SelectedTab = this.tabPageTY;
						}
						else
							SyncLYApplyTrendGrids();
						break;
					case "tabPageLY":
						if (!ValidTabTYLYTrend(gridLYNodeVersion, rdoEqualizeYesLY.Checked))
						{
							isValid = false;
                        //TT#741 - Begin - MD - ComboBox NullReference Exception red icon not displayed - RBeck 
                            this.tabOTSMethod.SelectedIndex = 1;
                            this.tabSetMethod.SelectedIndex = 0;
                        //TT#741 - End - MD - ComboBox NullReference Exception red icon not displayed - RBeck 
							this.tabTYLYTrend.SelectedTab = this.tabPageLY;
						}
						break;
					case "tabPageApplyTrend":
						if (!ValidTabTYLYTrend(gridTrendNodeVersion, rdoEqualizeYesTrend.Checked))
						{
							isValid = false;
                        //TT#741 - Begin - MD - ComboBox NullReference Exception red icon not displayed - RBeck 
                            this.tabOTSMethod.SelectedIndex = 1;
                            this.tabSetMethod.SelectedIndex = 0;
                        //TT#741 - End - MD - ComboBox NullReference Exception red icon not displayed - RBeck 
							this.tabTYLYTrend.SelectedTab = this.tabPageApplyTrend;
						}
						break;
					case "tabPageCaps":
						if (!ValidTabTrendCaps())
						{
							isValid = false;
                        //TT#741 - Begin - End - MD - ComboBox NullReference Exception red icon not displayed - RBeck 
                            this.tabOTSMethod.SelectedIndex = 1;
                            this.tabSetMethod.SelectedIndex = 0;
                        //TT#741 - End - MD - ComboBox NullReference Exception red icon not displayed - RBeck 
							this.tabTYLYTrend.SelectedTab = this.tabPageCaps;
						}
						break;
				}
					break;
				}
			}


			if (!ValidStockMinMax())
			{
				isValid = false;
            //TT#741 - Begin - MD - ComboBox NullReference Exception red icon not displayed - RBeck 
                this.tabOTSMethod.SelectedIndex = 1;
                this.tabSetMethod.SelectedIndex = 1;
             //TT#741 - End - MD - ComboBox NullReference Exception red icon not displayed - RBeck 
				this.tabSetMethod.SelectedTab = this.tabStockMinMax;
			}

			return isValid;
		}

		private string AddErrorMessage(eMIDTextCode textCode)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			string error = null;
			try
			{
				error = SAB.ClientServerSession.Audit.GetText(textCode);
				_errors += Environment.NewLine + "     "  + error;
				return error;
			}
			catch( Exception exception )
			{
				error = exception.Message;
				HandleException(exception);
				return error;
			}
		}

		private string AddErrorMessage(EditMsgs.Message emm)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			string error = null;
			try
			{
				if (emm.code != 0)
				{
					error = SAB.ClientServerSession.Audit.GetText(emm.code);
				}
				else
				{
					error = emm.msg;
				}
				_errors += Environment.NewLine + "     "  + error;
				return error;
			}
			catch( Exception exception )
			{
				HandleException(exception);
				error = exception.Message;
				return error;
			}
		}

		private bool ValidPctContribution()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			bool errorFound = false;
			try
			{
				ErrorProvider.SetError (grdPctContBasis,string.Empty);

				if (grdPctContBasis.Rows.Count == 0 && this.chkPlan.Checked == true)
				{
					string errorMessage = "A required basis is missing for the Attribute Set.";
					grdPctContBasis.Tag = errorMessage;
					ErrorProvider.SetError (grdPctContBasis,errorMessage);
					errorFound = true;
				}


				foreach (UltraGridRow gridRow in grdPctContBasis.Rows)
				{
					//Begin TT#692 - stodd - OTS forecast method abends when basis node is left blank
					if (!ValidVersion(gridRow.Cells["Merchandise"]))
					{
						errorFound = true;
					}
					//End TT#692 - stodd - OTS forecast method abends when basis node is left blank
					if (!ValidVersion(gridRow.Cells["Version"]))
					{
						errorFound = true;
					}
					if (!ValidDateRange(gridRow.Cells["DateRange"]))
					{
						errorFound = true;
					}
					if (!ValidWeight(gridRow.Cells["WEIGHT"]))
					//if (!ValidWeight(gridRow)) //MID Track #5954 - KJohnson - Basis weighting, exclusions, and equalizer issues
					{
						errorFound = true;
					}
				}
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

		private bool ValidTabTYLYTrend(UltraGrid aGrid, bool EqualizeWeighting)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				// BEGIN Issue 4818
				string errorMessage = null;
				bool errorFound = false;
				ErrorProvider.SetError (aGrid,string.Empty);
				
				if (aGrid.Rows.Count == 0 && this.chkPlan.Checked == true)
				{
					errorMessage = "A required basis is missing for an Attribute Set.";
					aGrid.Tag = errorMessage;
					ErrorProvider.SetError (aGrid,errorMessage);
					errorFound = true;
				}
				
				if (!errorFound)
				{
					// BEGIN Issue 5415 stodd
					decimal totWeight = 0; 
					// END Issue 5415 stodd
					foreach (UltraGridRow gridRow in aGrid.Rows)
					{
						//Begin TT#692 - stodd - OTS forecast method abends when basis node is left blank
                        if (!ValidateMerchandise(gridRow.Cells["Merchandise"]))
						{
							errorFound = true;
						}
						//End TT#692 - stodd - OTS forecast method abends when basis node is left blank
						if (!ValidVersion(gridRow.Cells["Version"]))
						{
							errorFound = true;
						}
						if (!ValidDateRange(gridRow.Cells["DateRange"]))
						{
							errorFound = true;
						}
						//if (!ValidWeight(gridRow)) //MID Track #5954 - KJohnson - Basis weighting, exclusions, and equalizer issues
						if (!ValidWeight(gridRow.Cells["WEIGHT"]))
						{
							errorFound = true;
						}
						// END Issue 4818
						else if (EqualizeWeighting)
						{
							// BEGIN Issue 5415 stodd
							decimal cellValue = Convert.ToDecimal(gridRow.Cells["WEIGHT"].Value, CultureInfo.CurrentUICulture);
							// BEGIN Issue 5954 stodd
							bool exclude = Include.ConvertCharToBool(Convert.ToChar(gridRow.Cells["INC_EXC_IND"].Value, CultureInfo.CurrentUICulture));
							// END Issue 5954 stodd
							// END Issue 5415 stodd

							if (cellValue > 1)
							{
                                this.tabOTSMethod.SelectedIndex = 1; //TT#741 - MD - ComboBox NullReference Exception red icon not displayed - RBeck 
                                errorMessage = string.Format
									(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MaximumValueExceeded), cellValue, "1");
								errorFound = true;
							}
							else
							{
								// BEGIN Issue 5954 stodd
								//if (!exclude)
								//{
									totWeight += cellValue;
								//}
								// END Issue 5954 stodd
							}
						}
					}
                    if (aGrid.Rows.Count > 0) // BEGIN MID Track #5954 - KJohnson - (Issue 13) Basis weighting, exclusions, and equalizer issues
                    {
                        if (EqualizeWeighting && totWeight != 1)
                        {
                            this.tabOTSMethod.SelectedIndex = 1; //TT#741 - MD - ComboBox NullReference Exception red icon not displayed - RBeck  
                            errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_BasisWeightTotalInvalid);
                            aGrid.Tag = errorMessage;
                            ErrorProvider.SetError(aGrid, errorMessage);
                            errorFound = true;
                        }
                    }
				}
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

		//Begin TT#692 - stodd - OTS forecast method abends when basis node is left blank
		private bool ValidateMerchandise(UltraGridCell gridCell)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			bool errorFound = false;
			string errorMessage = null;
			try
			{
				if (((MIDTag)(txtOTSHNDesc.Tag)).MIDTagData == null)
				{
					errorMessage = String.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired), "Merchandise");
					errorFound = true;
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
		//End TT#692 - stodd

		private bool ValidVersion (UltraGridCell gridCell)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			bool errorFound = false;
			string errorMessage = null;
			try
			{
				if (gridCell.Value.ToString() == string.Empty)
				{
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					errorFound = true;
				}
			}
			catch( Exception error)
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
				//gridCell.Tag = key;
				return true;
			}
		}

		private bool ValidDateRange (UltraGridCell gridCell)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			bool errorFound = false;
			string errorMessage = null;
			try
			{
				if (gridCell.Value.ToString() == string.Empty)
				{
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					errorFound = true;
				}
			}
			catch( Exception error)
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
				//gridCell.Appearance.Image = null;
				gridCell.Tag = null;
				//gridCell.Tag = key;
				return true;
			}
		}

		//private bool ValidWeight (UltraGridRow gridRow)
		//{
		//    bool errorFound = false;
		//    string errorMessage = null;
		//    double dblValue;
		//    try
		//    {
		//        // BEGIN MID Track #5954 - KJohnson - Basis weighting, exclusions, and equalizer issues
		//        if (gridRow.Cells["WEIGHT"].Value.ToString() == string.Empty)
		//        {
		//            errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
		//            errorFound = true;
		//        }
		//        else
		//        {
		//            dblValue = Convert.ToDouble(gridRow.Cells["WEIGHT"].Value.ToString(), CultureInfo.CurrentUICulture);
		//            if (dblValue <= 0)
		//            {
		//                if (gridRow.Cells["INC_EXC_IND"].Value.ToString() == "0") 
		//                {
		//                    errorMessage = "Weight cannot be zero or negative";
		//                    errorFound = true;
		//                }
		//            }
		//            else
		//            {
		//                dblValue = Math.Round(dblValue,2);
		//                //_weightValChanged = true;
		//                gridRow.Cells["WEIGHT"].Value  = dblValue;
		//            }
		//        }	
		//        // END MID Track #5954
		//    }
		//    catch( Exception error)
		//    {
		//        string exceptionMessage = error.Message;
		//        errorMessage = error.Message;
		//        errorFound = true;
		//    }

		//    if (errorFound)
		//    {
		//        // BEGIN MID Track #5954 - KJohnson - Basis weighting, exclusions, and equalizer issues
		//        gridRow.Cells["WEIGHT"].Appearance.Image = ErrorImage;
		//        gridRow.Cells["WEIGHT"].Tag = errorMessage;
		//        // END MID Track #5954
		//        return false;
		//    }
		//    else
		//    {
		//        // BEGIN MID Track #5954 - KJohnson - Basis weighting, exclusions, and equalizer issues
		//        gridRow.Cells["WEIGHT"].Appearance.Image = null;
		//        gridRow.Cells["WEIGHT"].Tag = null;
		//        // END MID Track #5954
		//        return true;
		//    }
		//}

		private bool ValidWeight (UltraGridCell gridCell)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			bool errorFound = false;
			string errorMessage = null;
			double dblValue;
			try
			{
				if (gridCell.Value.ToString() == string.Empty)
				{
                    this.tabOTSMethod.SelectedIndex = 1; //TT#741 - MD - ComboBox NullReference Exception red icon not displayed - RBeck 
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					errorFound = true;
				}
				else
				{
					dblValue = Convert.ToDouble(gridCell.Value.ToString(), CultureInfo.CurrentUICulture);
					if (dblValue <= 0)
					{
                        this.tabOTSMethod.SelectedIndex = 1; //TT#741 - MD - ComboBox NullReference Exception red icon not displayed - RBeck 
                        // BEGIN MID Track #6047 - KJohnson - New message
                        errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_WeightCannotBeZeroOrNegative);
                        // END MID Track #6047
						errorFound = true;
					}
					else
					{
						dblValue = Math.Round(dblValue,2);
						//_weightValChanged = true;
						gridCell.Value  = dblValue;
					}
				}	

			}
			catch( Exception error)
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

		private bool ValidTabTrendCaps ()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			bool errorFound = false;
			string errorMessage = string.Empty, strValue;
			ErrorProvider.SetError (txtTolerance,string.Empty);
			ErrorProvider.SetError (txtHigh,string.Empty);
			ErrorProvider.SetError (txtLow,string.Empty);
			try
			{
				if (radTolerance.Checked)
				{
					if ( txtTolerance.Text.Trim().Length == 0)
					{
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
						ErrorProvider.SetError (txtTolerance,errorMessage);
						errorFound = true;
					}
					else
					{
						strValue = txtTolerance.Text.Trim();
						if (!ValidTrendCap(eTrendCapID.Tolerance,ref strValue, ref errorMessage))
						{
							ErrorProvider.SetError (txtTolerance,errorMessage);
							errorFound = true;
						}
						else	
						{
							txtTolerance.Text = strValue;
						}
					}
				}
				else if (radLimits.Checked)
				{
					if ( txtHigh.Text.Trim().Length == 0 && txtLow.Text.Trim().Length == 0)
					{
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
						ErrorProvider.SetError (txtHigh,errorMessage);
						ErrorProvider.SetError (txtLow,errorMessage);
						errorFound = true;
					}
					else
					{
						if (txtHigh.Text.Trim().Length > 0)
						{
							strValue = txtHigh.Text.Trim();
							if (!ValidTrendCap(eTrendCapID.Limits,ref strValue, ref errorMessage))
							{
								ErrorProvider.SetError (txtHigh,errorMessage);
								errorFound = true;
							}
							else	
							{
								txtHigh.Text = strValue;
							}
						}
						if (txtLow.Text.Trim().Length > 0)
						{
							strValue = txtLow.Text.Trim();
							if (!ValidTrendCap(eTrendCapID.Limits,ref strValue, ref errorMessage))
							{
								ErrorProvider.SetError (txtLow,errorMessage);
								errorFound = true;
							}
							else	
							{
								txtLow.Text = strValue;
							}
						}
						if (!errorFound && txtHigh.Text.Length > 0 && txtLow.Text.Length > 0)
						{
							double high = Convert.ToDouble(txtHigh.Text, CultureInfo.CurrentUICulture);
							double low = Convert.ToDouble(txtLow.Text, CultureInfo.CurrentUICulture);
							if (low >= high)
							{
								errorMessage = "High limit must be greater than low limit"; 
								ErrorProvider.SetError (txtHigh,errorMessage);
								ErrorProvider.SetError (txtLow,errorMessage);
								errorFound = true;
							}
						}	
					}
				}
				if (errorFound)
				{
					//em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
					return false;
				}
				else
					return true;
			}
			catch( Exception error)
			{
				string exceptionMessage = error.Message;
				errorMessage = error.Message;
				errorFound = true;
				return false;
			}
		}

		private bool ValidTrendCap(eTrendCapID aTrendCapID,ref string aValue, ref string aMessage)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			double dblValue;
			bool valueIsValid = true;
			try
			{
				dblValue = Convert.ToDouble(aValue, CultureInfo.CurrentUICulture);
				if (dblValue <= 0)
				{
					aMessage = "Value must be greater than zero";
					valueIsValid = false;
				}
				else
				{
					if ( aTrendCapID == eTrendCapID.Tolerance && dblValue > 100)
					{
						aMessage = "Value cannot exceed 100";
						valueIsValid = false;
					}
					aValue = Convert.ToString(Math.Round(dblValue,2));
				}
			}
			catch( Exception ex)
			{
				HandleException(ex);
			}
			return valueIsValid;
		}	

		private bool ValidStockMinMax()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				string errorMessage = null;
				bool errorFound = false;
				int min = 0, max = 0;
				//ErrorProvider.SetError (gridStockMinMax,string.Empty);
                // Begin TT#272-MD - JSmith - Version 5.0 - General screen cleanup
                if (gridStockMinMax == null ||
                    gridStockMinMax.Rows == null)
                {
                    return true;
                }
                // End TT#272-MD - JSmith - Version 5.0 - General screen cleanup
				
				foreach (UltraGridRow gridRow in gridStockMinMax.Rows)
				{
					if (!ValidStockValue(gridRow.Cells["Minimum"]))
					{
						errorFound = true;
					}
					if (!ValidStockValue(gridRow.Cells["Maximum"]))
					{
						errorFound = true;
					}
					if (!errorFound)
					{
						if (gridRow.Cells["Minimum"].Value != System.DBNull.Value &&
							gridRow.Cells["Maximum"].Value != System.DBNull.Value)
						{
							min = Convert.ToInt32(gridRow.Cells["Minimum"].Value, CultureInfo.CurrentUICulture);
							max = Convert.ToInt32(gridRow.Cells["Maximum"].Value, CultureInfo.CurrentUICulture);
							if (max < min)
							{
								errorMessage = "Maximum must be greater than Minimum";
								errorFound = true;
								gridRow.Cells["Minimum"].Appearance.Image = ErrorImage;
								gridRow.Cells["Minimum"].Tag = errorMessage;
								gridRow.Cells["Maximum"].Appearance.Image = ErrorImage;
								gridRow.Cells["Maximum"].Tag = errorMessage;
							}
						}
					}

					if (gridRow.HasChild())
					{
						foreach (UltraGridRow childRow in gridRow.ChildBands[0].Rows)
						{
							if (!ValidDateRange(childRow.Cells["CDR_RID"]))
							{
								errorFound = true;
							}
							if (!ValidStockValue(childRow.Cells["MIN_STOCK"]))
							{
								errorFound = true;
							}
							if (!ValidStockValue(childRow.Cells["MAX_STOCK"]))
							{
								errorFound = true;
							}
							if (!errorFound)
							{
								if (childRow.Cells["MIN_STOCK"].Value != System.DBNull.Value &&
									childRow.Cells["MAX_STOCK"].Value != System.DBNull.Value)
								{
									min = Convert.ToInt32(childRow.Cells["MIN_STOCK"].Value, CultureInfo.CurrentUICulture);
									max = Convert.ToInt32(childRow.Cells["MAX_STOCK"].Value, CultureInfo.CurrentUICulture);
									if (max < min)
									{
										errorMessage = "Maximum must be greater than Minimum";
										errorFound = true;
										childRow.Cells["MIN_STOCK"].Appearance.Image = ErrorImage;
										childRow.Cells["MIN_STOCK"].Tag = errorMessage;
										childRow.Cells["MAX_STOCK"].Appearance.Image = ErrorImage;
										childRow.Cells["MAX_STOCK"].Tag = errorMessage;
									}
								}
							}
						}

					}
				}	
					
				if (errorFound)
				{
					return false;
				}
				else
				{
					SetStockMinMax();
					return true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return false; 
			}
		}

		private bool ValidStockValue (UltraGridCell gridCell)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			bool errorFound = false;
			string errorMessage = null;
			int intValue;
			try
			{
				if (gridCell.Value != System.DBNull.Value)
				{
					intValue = Convert.ToInt32(gridCell.Value.ToString(), CultureInfo.CurrentUICulture);
					if (intValue < 0)
					{
						errorMessage = "Stock Minimum/Maximum cannot be negative";
						errorFound = true;
					}
					else
					{
						gridCell.Value  = intValue;
					}
				}
			}
			catch( Exception error)
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

		private void SyncLYApplyTrendGrids()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				// Make sure these have all changes applied
				_dtSourceTYNode.AcceptChanges();

				// Begin Issue 4217 stodd - grids using default where re-syncing when they shouldn't
				//if (_GLFProfile.LY_Alt_IND == false)
				if (!cbxAltLY.Checked)
				{
					SyncLastYearGrids();
				}
				//if (_GLFProfile.Trend_Alt_IND == false)
				if (!cbxAltTrend.Checked)
				{
					SyncApplyTrendGrids();
				}
				// End Issue 4217
			}
			catch( Exception ex)
			{
				HandleException(ex);
			}

		}

		private void SyncLastYearGrids()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			int i;
			Infragistics.Win.UltraWinGrid.InitializeRowEventArgs args;
			try
			{
				ArrayList deleteRows = new ArrayList();

				if (_dtSourceLYNode.Rows.Count > 0)
				{
					for  (i=_dtSourceLYNode.Rows.Count - 1; i >= 0; i--)
					{
						DataRow lyRow = _dtSourceLYNode.Rows[i];
						// Begin Issue 4217 stodd - code was re-syncing ALL sets instead of just the current set
						int sglRid = Convert.ToInt32(lyRow["SGL_RID"], CultureInfo.CurrentUICulture);
						if (sglRid == this._GLFProfile.Key)
						{
							eTyLyType rowType = (eTyLyType)Convert.ToInt32(lyRow["TYLY_TYPE_ID"], CultureInfo.CurrentUICulture);
							if (rowType == eTyLyType.AlternateLy)
								deleteRows.Add(lyRow);
						}
						// End Issue 4217 stodd - code was re-syncing ALL sets instead of just the current set
					}
					// Since most arrays don't like you removing rows as you loop through it, we cache
					// the rows and delete them now.
					foreach (DataRow row in deleteRows)
					{
						_dtSourceLYNode.Rows.Remove(row);
					}
				}
			
				deleteRows.Clear();

//				if (_dtSourceLYTime.Rows.Count > 0)
//				{
//					for  (i=_dtSourceLYTime.Rows.Count - 1; i >= 0; i--)
//					{
//						DataRow lyRow = _dtSourceLYTime.Rows[i];
//						// Begin Issue 4217 stodd - code was re-syncing ALL sets instead of just the current set
//						int sglRid = Convert.ToInt32(lyRow["SGL_RID"], CultureInfo.CurrentUICulture);
//						if (sglRid == this._GLFProfile.Key)
//						{
//
//							eTyLyType rowType = (eTyLyType)Convert.ToInt32(lyRow["TYLY_TYPE_ID"], CultureInfo.CurrentUICulture);
//							if (rowType == eTyLyType.AlternateLy)
//								deleteRows.Add(lyRow);
//						}
//						// End Issue 4217 stodd - code was re-syncing ALL sets instead of just the current set
//					}
//					// Since most arrays don't like you removing rows as you loop through it, we cache
//					// the rows and delete them now.
//					foreach (DataRow row in deleteRows)
//					{
//						_dtSourceLYTime.Rows.Remove(row);
//					}
//				}

				foreach ( DataRow tyRow in _dtSourceTYNode.Rows)
				{
					// Begin Issue 4217 stodd - code was re-syncing ALL sets instead of just the current set
					int sglRid = Convert.ToInt32(tyRow["SGL_RID"], CultureInfo.CurrentUICulture);
					if (sglRid == this._GLFProfile.Key)
					{
						// Begin #5954 stodd
						// Begin #6066 stodd
						rdoEqualizeYesLY.Checked = rdoEqualizeYesTY.Checked;
						rdoEqualizeNoLY.Checked = rdoEqualizeNoTY.Checked;
						// END #6066 stodd
						//_GLFProfile.LY_Weight_Multiple_Basis_Ind = _GLFProfile.TY_Weight_Multiple_Basis_Ind;
						//if (_GLFProfile.LY_Weight_Multiple_Basis_Ind)
						//    rdoEqualizeYesLY.Checked = true;
						//else
						//    rdoEqualizeNoLY.Checked = true;
						// End #5954
						eTyLyType rowType = (eTyLyType)Convert.ToInt32(tyRow["TYLY_TYPE_ID"], CultureInfo.CurrentUICulture);
						if (rowType == eTyLyType.TyLy)
						{
							// BEGIN MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
							DataRow lyRow = (DataRow)_dtSourceLYNode.NewRow();
							if (_dtSourceLYNode.Rows.Count == 0)
							{
								lyRow["SEQ"] = 1;
							}
							else
							{
								// BEGIN MID ISSUE #2833 - stodd
								int seq = 1;
								int lastIndex = _dtSourceLYNode.Rows.Count - 1;
								for (int j = lastIndex; j > -1; j--)
								{
									DataRow lastRow = _dtSourceLYNode.Rows[j];
									if (lastRow.RowState != DataRowState.Deleted)
									{
										// BEGIN MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
										if ((int)lastRow["SGL_RID"] == sglRid)
										{
											if (lastRow["SEQ"] == DBNull.Value)
												seq = 1;
											else
												seq = Convert.ToInt32(lastRow["SEQ"], CultureInfo.CurrentUICulture) + 1;
											break;
										}
										// END MID Track #5954
									}
								}
								lyRow["SEQ"] = seq;
								// BEGIN MID ISSUE #2833 - stodd
							}

							lyRow["METHOD_RID"] = this._OTSPlanMethod.Key;
							// END MID Track #5954

							lyRow["SGL_RID"] = tyRow["SGL_RID"];
							lyRow["HN_RID"] = tyRow["HN_RID"];
							lyRow["FV_RID"] = tyRow["FV_RID"];
							lyRow["INC_EXC_IND"] = tyRow["INC_EXC_IND"];
							lyRow["TYLY_TYPE_ID"] = (int)eTyLyType.AlternateLy;
							lyRow["Merchandise"] = tyRow["Merchandise"];
							lyRow["Version"]  = tyRow["Version"];
							lyRow["IncludeButton"] = tyRow["IncludeButton"];
							lyRow["MERCH_TYPE"] = tyRow["MERCH_TYPE"];
							lyRow["MERCH_PH_RID"] = tyRow["MERCH_PH_RID"];
							lyRow["MERCH_PHL_SEQUENCE"] = tyRow["MERCH_PHL_SEQUENCE"];
							lyRow["MERCH_OFFSET"] = tyRow["MERCH_OFFSET"];

							// BEGIN Issue 4818
							lyRow["WEIGHT"] = tyRow["WEIGHT"];
							DateRangeProfile dr = SAB.ClientServerSession.Calendar.GetDateRange((int)tyRow["CDR_RID"]);
							DateRangeProfile lydr = null;
							if (midDateRangeSelectorOTSPlan.Tag != null)
							{
								int planCDRKey = (int)midDateRangeSelectorOTSPlan.Tag;
								if (planCDRKey != Include.UndefinedCalendarDateRange)
								{
									WeekProfile FirstWeekOfPlan = SAB.ClientServerSession.Calendar.GetFirstWeekOfRange(planCDRKey); 
									lydr = SAB.ClientServerSession.Calendar.GetSameRangeForLastYear(dr, FirstWeekOfPlan);
								}
								else
								{
									WeekProfile FirstWeekOfTY = SAB.ClientServerSession.Calendar.GetFirstWeekOfRange((int)tyRow["CDR_RID"]); 
									lydr = SAB.ClientServerSession.Calendar.GetSameRangeForLastYear(dr, FirstWeekOfTY);
								}
							}
							else
							{
								WeekProfile FirstWeekOfTY = SAB.ClientServerSession.Calendar.GetFirstWeekOfRange((int)tyRow["CDR_RID"]); 
								lydr = SAB.ClientServerSession.Calendar.GetSameRangeForLastYear(dr, FirstWeekOfTY);
							}
							lyRow["CDR_RID"] = lydr.Key;		
							lyRow["DateRange"] = lydr.DisplayDate;
							lyRow["Picture"] = tyRow["Picture"];
							// END Issue 4818

							_dtSourceLYNode.Rows.Add(lyRow);
						}
					}
					// End Issue 4217 stodd - code was re-syncing ALL sets instead of just the current set
				}
				 
//				foreach ( DataRow tyTRow in _dtSourceTYTime.Rows)
//				{
//					// Begin Issue 4217 stodd - code was re-syncing ALL sets instead of just the current set
//					int sglRid = Convert.ToInt32(tyTRow["SGL_RID"], CultureInfo.CurrentUICulture);
//					if (sglRid == this._GLFProfile.Key)
//					{
//						eTyLyType rowType = (eTyLyType)Convert.ToInt32(tyTRow["TYLY_TYPE_ID"], CultureInfo.CurrentUICulture);
//						if (rowType == eTyLyType.TyLy)
//						{
//							DataRow lyTRow = _dtSourceLYTime.NewRow();
//					
//							lyTRow["SGL_RID"] = tyTRow["SGL_RID"];
//							lyTRow["INC_EXC_IND"] = tyTRow["INC_EXC_IND"];
//							lyTRow["BR_WEIGHT"] = tyTRow["BR_WEIGHT"];
//							lyTRow["TYLY_TYPE_ID"] = (int)eTyLyType.AlternateLy;
//							DateRangeProfile dr = SAB.ClientServerSession.Calendar.GetDateRange((int)tyTRow["BR_CDR_RID"]);
//							DateRangeProfile lydr = null;
//							if (midDateRangeSelectorOTSPlan.Tag != null)
//							{
//								int planCDRKey = (int)midDateRangeSelectorOTSPlan.Tag;
//								if (planCDRKey != Include.UndefinedCalendarDateRange)
//								{
//									WeekProfile FirstWeekOfPlan = SAB.ClientServerSession.Calendar.GetFirstWeekOfRange(planCDRKey); 
//									lydr = SAB.ClientServerSession.Calendar.GetSameRangeForLastYear(dr, FirstWeekOfPlan);
//								}
//								else
//								{
//									WeekProfile FirstWeekOfTY = SAB.ClientServerSession.Calendar.GetFirstWeekOfRange((int)tyTRow["BR_CDR_RID"]); 
//									lydr = SAB.ClientServerSession.Calendar.GetSameRangeForLastYear(dr, FirstWeekOfTY);
//								}
//							}
//							else
//							{
//								WeekProfile FirstWeekOfTY = SAB.ClientServerSession.Calendar.GetFirstWeekOfRange((int)tyTRow["BR_CDR_RID"]); 
//								lydr = SAB.ClientServerSession.Calendar.GetSameRangeForLastYear(dr, FirstWeekOfTY);
//							}
//							lyTRow["BR_CDR_RID"] = lydr.Key;		
//							lyTRow["DateRange"] = lydr.DisplayDate;
//							lyTRow["Picture"] = tyTRow["Picture"];
//				
//							_dtSourceLYTime.Rows.Add(lyTRow);
//						}
//					}
//					// End Issue 4217 stodd - code was re-syncing ALL sets instead of just the current set
//				}

                // BEGIN MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
                _InitializingRow = true;    //MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
				foreach (UltraGridRow row in gridLYNodeVersion.Rows)
				{
					args = new InitializeRowEventArgs(row,true);
					TyLyGrid_InitializeRow(gridLYNodeVersion, args);

					// BEGIN Issue 4818
					DateRangeProfile lydr = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture));
					if (lydr.DateRangeType == eCalendarRangeType.Dynamic)
					{
						switch (lydr.RelativeTo)
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
					// END Issue 4818
				}
                _InitializingRow = false;    //MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen

//				foreach (UltraGridRow row in gridLYTimeWeight.Rows)
//				{
//					DateRangeProfile lydr = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(row.Cells["BR_CDR_RID"].Value, CultureInfo.CurrentUICulture));
//					if (lydr.DateRangeType == eCalendarRangeType.Dynamic)
//					{
//						switch (lydr.RelativeTo)
//						{
//							case eDateRangeRelativeTo.Current:
//								row.Cells["DateRange"].Appearance.Image = this.DynamicToCurrentImage;
//								break;
//							case eDateRangeRelativeTo.Plan:
//								row.Cells["DateRange"].Appearance.Image = this.DynamicToPlanImage;
//								break;
//							default:
//								row.Cells["DateRange"].Appearance.Image = null;
//								break;
//						}
//					}
//				} 
		
			}
			catch( Exception ex)
			{
				HandleException(ex);
			}
		}

		private void SyncApplyTrendGrids()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			int i;
			Infragistics.Win.UltraWinGrid.InitializeRowEventArgs args;
			try
			{
				ArrayList deleteRows = new ArrayList();

				if (_dtSourceTrendNode.Rows.Count > 0)
				{
					for  (i=_dtSourceTrendNode.Rows.Count - 1; i >= 0; i--)
					{
						DataRow lyRow = _dtSourceTrendNode.Rows[i];
						// Begin Issue 4217 stodd - code was re-syncing ALL sets instead of just the current set
						int sglRid = Convert.ToInt32(lyRow["SGL_RID"], CultureInfo.CurrentUICulture);
						if (sglRid == this._GLFProfile.Key)
						{
							eTyLyType rowType = (eTyLyType)Convert.ToInt32(lyRow["TYLY_TYPE_ID"], CultureInfo.CurrentUICulture);
							if (rowType == eTyLyType.AlternateApplyTo)
								deleteRows.Add(lyRow);
						}
						// End Issue 4217 stodd - code was re-syncing ALL sets instead of just the current set
					}
					// Since most arrays don't like you removing rows as you loop through it, we cache
					// the rows and delete them now.
					foreach (DataRow row in deleteRows)
					{
						_dtSourceTrendNode.Rows.Remove(row);
					}
				}

				deleteRows.Clear();

//				if (_dtSourceTrendTime.Rows.Count > 0)
//				{
//					for  (i=_dtSourceTrendTime.Rows.Count - 1; i >= 0; i--)
//					{
//						DataRow lyRow = _dtSourceTrendTime.Rows[i];
//						// Begin Issue 4217 stodd - code was re-syncing ALL sets instead of just the current set
//						int sglRid = Convert.ToInt32(lyRow["SGL_RID"], CultureInfo.CurrentUICulture);
//						if (sglRid == this._GLFProfile.Key)
//						{
//							eTyLyType rowType = (eTyLyType)Convert.ToInt32(lyRow["TYLY_TYPE_ID"], CultureInfo.CurrentUICulture);
//							if (rowType == eTyLyType.AlternateApplyTo)
//								deleteRows.Add(lyRow);
//						}
//						// End Issue 4217 stodd - code was re-syncing ALL sets instead of just the current set
//					}
//					foreach (DataRow row in deleteRows)
//					{
//						_dtSourceTrendTime.Rows.Remove(row);
//					}
//				}
				
				foreach ( DataRow tyRow in _dtSourceTYNode.Rows)
				{
					// Begin Issue 4217 stodd - code was re-syncing ALL sets instead of just the current set
					int sglRid = Convert.ToInt32(tyRow["SGL_RID"], CultureInfo.CurrentUICulture);
					if (sglRid == this._GLFProfile.Key)
					{
						// Begin #5954 stodd
						// Begin #6066 stodd
						rdoEqualizeYesTrend.Checked = rdoEqualizeYesTY.Checked;
						rdoEqualizeNoTrend.Checked = rdoEqualizeNoTY.Checked;
						// End #6066 stodd
						//_GLFProfile.Apply_Weight_Multiple_Basis_Ind = _GLFProfile.TY_Weight_Multiple_Basis_Ind;
						//if (_GLFProfile.Apply_Weight_Multiple_Basis_Ind)
						//    this.rdoEqualizeYesTrend.Checked = true;
						//else
						//    this.rdoEqualizeNoTrend.Checked = true;
						// End #5954
						eTyLyType rowType = (eTyLyType)Convert.ToInt32(tyRow["TYLY_TYPE_ID"], CultureInfo.CurrentUICulture);
						if (rowType == eTyLyType.TyLy)
						{
							DataRow trRow = (DataRow)_dtSourceTrendNode.NewRow();
					 
							trRow["SGL_RID"] = tyRow["SGL_RID"];
							trRow["HN_RID"] = tyRow["HN_RID"];
							trRow["FV_RID"] = tyRow["FV_RID"];
							trRow["INC_EXC_IND"] = tyRow["INC_EXC_IND"];
							trRow["TYLY_TYPE_ID"] = (int)eTyLyType.AlternateApplyTo;
							trRow["Merchandise"] = tyRow["Merchandise"];
							trRow["Version"]  = tyRow["Version"];
							trRow["IncludeButton"] = tyRow["IncludeButton"];
							trRow["MERCH_TYPE"] = tyRow["MERCH_TYPE"];
							trRow["MERCH_PH_RID"] = tyRow["MERCH_PH_RID"];
							trRow["MERCH_PHL_SEQUENCE"] = tyRow["MERCH_PHL_SEQUENCE"];
							trRow["MERCH_OFFSET"] = tyRow["MERCH_OFFSET"];

							trRow["WEIGHT"] = tyRow["WEIGHT"];
							DateRangeProfile dr = null;
							if (midDateRangeSelectorOTSPlan.Tag != null)
							{
								int planCDRKey = (int)midDateRangeSelectorOTSPlan.Tag;
                                if (planCDRKey != Include.UndefinedCalendarDateRange)
                                {
                                    DateRangeProfile planDr = SAB.ClientServerSession.Calendar.GetDateRange(planCDRKey);
                                    dr = SAB.ClientServerSession.Calendar.GetSameRangeForLastYear(planDr);
                                    dr = SAB.ClientServerSession.Calendar.ConvertToDynamicToPlan(dr, planDr);
                                }
                                else
                                // Begin TT#3838 - JSmith - Get System Invalid Cast Exception when clicking on LY tab in OTS Forecast Method
                                //dr = SAB.ClientServerSession.Calendar.GetDateRange((int)trRow["CDR_RID"]);
                                {
                                    dr = SAB.ClientServerSession.Calendar.GetDateRange((int)tyRow["CDR_RID"]);
                                }
                                // End TT#3838 - JSmith - Get System Invalid Cast Exception when clicking on LY tab in OTS Forecast Method
							}
							else	
							{
                                // Begin TT#3838 - JSmith - Get System Invalid Cast Exception when clicking on LY tab in OTS Forecast Method
                                //dr = SAB.ClientServerSession.Calendar.GetDateRange((int)trRow["CDR_RID"]);
                                dr = SAB.ClientServerSession.Calendar.GetDateRange((int)tyRow["CDR_RID"]);
                                // End TT#3838 - JSmith - Get System Invalid Cast Exception when clicking on LY tab in OTS Forecast Method
							}
							DateRangeProfile trdr = SAB.ClientServerSession.Calendar.GetRangeAsFirstWeekOfRange(dr);
							trRow["CDR_RID"] = trdr.Key;		
							trRow["DateRange"] = trdr.DisplayDate;
							trRow["Picture"] = tyRow["Picture"];


							_dtSourceTrendNode.Rows.Add(trRow);
						}
					}
					// End Issue 4217 stodd - code was re-syncing ALL sets instead of just the current set
				}

//				foreach ( DataRow tyTRow in _dtSourceTYTime.Rows)
//				{
//					// Begin Issue 4217 stodd - code was re-syncing ALL sets instead of just the current set
//					int sglRid = Convert.ToInt32(tyTRow["SGL_RID"], CultureInfo.CurrentUICulture);
//					if (sglRid == this._GLFProfile.Key)
//					{
//						eTyLyType rowType = (eTyLyType)Convert.ToInt32(tyTRow["TYLY_TYPE_ID"], CultureInfo.CurrentUICulture);
//						if (rowType == eTyLyType.TyLy)
//						{
//							DataRow trTRow = (DataRow)_dtSourceTrendTime.NewRow();
//
//							trTRow["SGL_RID"] = tyTRow["SGL_RID"];
//							trTRow["INC_EXC_IND"] = tyTRow["INC_EXC_IND"];
//							trTRow["BR_WEIGHT"] = tyTRow["BR_WEIGHT"];
//							trTRow["TYLY_TYPE_ID"] = (int)eTyLyType.AlternateApplyTo;
//							DateRangeProfile dr = null;
//							if (midDateRangeSelectorOTSPlan.Tag != null)
//							{
//								int planCDRKey = (int)midDateRangeSelectorOTSPlan.Tag;
//								if (planCDRKey != Include.UndefinedCalendarDateRange)
//								{
//									DateRangeProfile planDr = SAB.ClientServerSession.Calendar.GetDateRange(planCDRKey);
//									dr = SAB.ClientServerSession.Calendar.GetSameRangeForLastYear(planDr);
//									// BEGIN MID Issue # 2790 stodd
//									dr = SAB.ClientServerSession.Calendar.ConvertToDynamicToPlan(dr, planDr);
//									// END MID Issue # 2790 stodd
//								}
//								else
//									dr = SAB.ClientServerSession.Calendar.GetDateRange((int)tyTRow["BR_CDR_RID"]);
//							}
//							else	
//							{
//								dr = SAB.ClientServerSession.Calendar.GetDateRange((int)tyTRow["BR_CDR_RID"]);
//							}
//							DateRangeProfile trdr = SAB.ClientServerSession.Calendar.GetRangeAsFirstWeekOfRange(dr);
//							trTRow["BR_CDR_RID"] = trdr.Key;		
//							trTRow["DateRange"] = trdr.DisplayDate;
//							trTRow["Picture"] = tyTRow["Picture"];
//
//							_dtSourceTrendTime.Rows.Add(trTRow);
//						}
//					}
//					// End Issue 4217 stodd - code was re-syncing ALL sets instead of just the current set
//				}

				foreach (UltraGridRow row in gridTrendNodeVersion.Rows)
				{
					args = new InitializeRowEventArgs(row,true);
					TyLyGrid_InitializeRow(gridTrendNodeVersion, args);

					// BEGIN Issue 4818
					DateRangeProfile trdr = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture));
					if (trdr.DateRangeType == eCalendarRangeType.Dynamic)
					{
						switch (trdr.RelativeTo)
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
					// END Isue 4818
				}

//				foreach (UltraGridRow row in gridTrendTimeWeight.Rows)
//				{
//					DateRangeProfile trdr = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(row.Cells["BR_CDR_RID"].Value, CultureInfo.CurrentUICulture));
//					if (trdr.DateRangeType == eCalendarRangeType.Dynamic)
//					{
//						switch (trdr.RelativeTo)
//						{
//							case eDateRangeRelativeTo.Current:
//								row.Cells["DateRange"].Appearance.Image = this.DynamicToCurrentImage;
//								break;
//							case eDateRangeRelativeTo.Plan:
//								row.Cells["DateRange"].Appearance.Image = this.DynamicToPlanImage;
//								break;
//							default:
//								row.Cells["DateRange"].Appearance.Image = null;
//								break;
//						}
//					}
//				} 
			}
			catch( Exception ex)
			{
				HandleException(ex);
			}
		}

		// Begin Issue 3816 - stodd
		private void CopySmoothing(int copyToRid)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			eGroupLevelSmoothBy defaultSmoothBy = this.GetDefaultSmoothing();

			if ((int)cbxStoreGroupLevel.SelectedValue == copyToRid)
				cboGLGroupBy.SelectedValue = (int)defaultSmoothBy;
			else
			{
				GroupLevelFunctionProfile glfp = (GroupLevelFunctionProfile)_GLFProfileList.FindKey(copyToRid);
				glfp.GLSB_ID = defaultSmoothBy;
			}
		}
		// end issue 3816

        // BEGIN Issue 5420 KJohnson - rdoEqualizeYesTY not holding it value
        private void CopyEqualize(int copyToRid)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            if ((int)cbxStoreGroupLevel.SelectedValue == copyToRid)
            {
                if (_defaultEqualizeTY == true)
                {
                    this.rdoEqualizeYesTY.Checked = true;
                    this.rdoEqualizeNoTY.Checked = false;
                }
                else 
                {
                    this.rdoEqualizeYesTY.Checked = false;
                    this.rdoEqualizeNoTY.Checked = true;
                }

                if (_defaultEqualizeLY == true)
                {
                    this.rdoEqualizeYesLY.Checked = true;
                    this.rdoEqualizeNoLY.Checked = false;
                }
                else 
                {
                    this.rdoEqualizeYesLY.Checked = false;
                    this.rdoEqualizeNoLY.Checked = true;
                }

                if (_defaultEqualizeTrend == true)
                {
                    this.rdoEqualizeYesTrend.Checked = true;
                    this.rdoEqualizeNoTrend.Checked = false;
                }
                else 
                {
                    this.rdoEqualizeYesTrend.Checked = false;
                    this.rdoEqualizeNoTrend.Checked = true;
                }
            }
        }
        // END Issue 5420 KJohnson

		private void CopyPercentContributionBasis(int copyToRid)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				CopySmoothing(copyToRid);	// Issue 3816 - stodd
				CopyBasis(copyToRid, _dtSource);
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
        private void CopyMinMax(int copyToRid, bool aForceCopyValues)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                GroupLevelFunctionProfile defaultGLFP = null;
                GroupLevelFunctionProfile setDefaultGLFP = null;
                GroupLevelNodeFunction setDefaultGLNF = null;

                // get affected group level profiles
                foreach (GroupLevelFunctionProfile glfp in _GLFProfileList.ArrayList)
                {
                    if (glfp.Default_IND)
                    {
                        defaultGLFP = glfp;
                    }
                    else if (glfp.Key == copyToRid)
                    {
                        setDefaultGLFP = glfp;
                    }
                }

                if (defaultGLFP != null)
                {
                    // check for GroupLevelFunctionProfile of group level being set to default
                    if (setDefaultGLFP != null)  // if found, clear values
                    {
                        // process each node in the GroupLevelFunctionProfile
						foreach (GroupLevelNodeFunction glnf in defaultGLFP.Group_Level_Nodes.Values)
						{
							// check for GroupLevelNodeFunction in group level setting to default
							setDefaultGLNF = (GroupLevelNodeFunction)setDefaultGLFP.Group_Level_Nodes[glnf.HN_RID];
							//Begin TT#989 - JScott - Created an OTS Forecast with Attribute Sets 1st set is the Default, went to set up the 2nd set and after I selected Forecast and Use Default received a Null Reference Exception
							//if (setDefaultGLNF.MinMaxInheritType == eMinMaxInheritType.Default ||
							//aForceCopyValues)
							//{
							//    if (setDefaultGLNF == null)  // if not exist, create new
							//    {
							//        setDefaultGLNF = new GroupLevelNodeFunction();
							//        setDefaultGLNF.HN_RID = glnf.HN_RID;
							//        setDefaultGLNF.SglRID = copyToRid;
							//        setDefaultGLNF.MethodRID = _OTSPlanMethod.Key;
							//        setDefaultGLFP.Group_Level_Nodes.Add(glnf.HN_RID, setDefaultGLNF);
							//    }
							//    else  // if exists, clear current min/max values
							//    {
							//        setDefaultGLNF.Stock_MinMax.Clear();
							//    }
							//}
							if (setDefaultGLNF == null)  // if not exist, create new
							{
								setDefaultGLNF = new GroupLevelNodeFunction();
								setDefaultGLNF.HN_RID = glnf.HN_RID;
								setDefaultGLNF.SglRID = copyToRid;
								setDefaultGLNF.MethodRID = _OTSPlanMethod.Key;
                                // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                                setDefaultGLNF.ApplyMinMaxesInd = glnf.ApplyMinMaxesInd;
                                setDefaultGLNF.MinMaxInheritType = eMinMaxInheritType.Default;
                                // End TT#3
								setDefaultGLFP.Group_Level_Nodes.Add(glnf.HN_RID, setDefaultGLNF);
							}
							else  // if exists, clear current min/max values
							{
								if (setDefaultGLNF.MinMaxInheritType == eMinMaxInheritType.Default ||
								aForceCopyValues)
								{
									setDefaultGLNF.Stock_MinMax.Clear();
								}
							}
							//End TT#989 - JScott - Created an OTS Forecast with Attribute Sets 1st set is the Default, went to set up the 2nd set and after I selected Forecast and Use Default received a Null Reference Exception
						}
                    }
                    else  // if not found, create new
                    {
                        setDefaultGLFP = new GroupLevelFunctionProfile(copyToRid);
						//Begin TT#989 - JScott - Created an OTS Forecast with Attribute Sets 1st set is the Default, went to set up the 2nd set and after I selected Forecast and Use Default received a Null Reference Exception
						foreach (GroupLevelNodeFunction glnf in defaultGLFP.Group_Level_Nodes.Values)
						{
							setDefaultGLNF = new GroupLevelNodeFunction();
							setDefaultGLNF.HN_RID = glnf.HN_RID;
							setDefaultGLNF.SglRID = copyToRid;
							setDefaultGLNF.MethodRID = _OTSPlanMethod.Key;
                            // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                            setDefaultGLNF.ApplyMinMaxesInd = glnf.ApplyMinMaxesInd;
                            setDefaultGLNF.MinMaxInheritType = eMinMaxInheritType.Default;
                            // End TT#3
							setDefaultGLFP.Group_Level_Nodes.Add(glnf.HN_RID, setDefaultGLNF);
						}
						//End TT#989 - JScott - Created an OTS Forecast with Attribute Sets 1st set is the Default, went to set up the 2nd set and after I selected Forecast and Use Default received a Null Reference Exception
						_GLFProfileList.Add(setDefaultGLFP);
                    }

                    // copy default values to group level being set to default
                    foreach (GroupLevelNodeFunction glnf in defaultGLFP.Group_Level_Nodes.Values)
                    {

                        setDefaultGLNF = (GroupLevelNodeFunction)setDefaultGLFP.Group_Level_Nodes[glnf.HN_RID];
                        if (setDefaultGLNF.MinMaxInheritType == eMinMaxInheritType.Default ||
                            aForceCopyValues)
                        {
                            foreach (StockMinMaxProfile smmp in glnf.Stock_MinMax)
                            {
                                StockMinMaxProfile copySMMP = smmp.Copy(SAB.ClientServerSession, true);
                                copySMMP.StoreGroupLevelRid = copyToRid;
                                setDefaultGLNF.Stock_MinMax.Add(copySMMP);
                            }
                        }
                    }
                }

				// Begin TT#3 - stodd - Forecasting issues with Min/Max grid
				// I commented out this code. I was reasearching why certain sets get changed from "useDefault=true" to "useDefault=false"
				// after the "Process" button was pressed. It seemed odd to set the _GLFProfile to be the last setDefaultGLFP found.
				// On a hunch I commented it out and it corrected the problem. I didn't notice any other ill affects from doing so.
				// Side note: I also comment out some no longer needed code that was messing with the default Set settings. These two peices of 
				// code may have been working hand-in-hand, but with the recent changes, I don't think they are neccessary.
                // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                //_GLFProfile = setDefaultGLFP;
                _GLFProfile = (GroupLevelFunctionProfile)_GLFProfileList.FindKey(copyToRid);
                // End TT#3
				// Begin TT#3 - stodd

                int selectedValue = GetSelectedValue();

                setDefaultGLNF = (GroupLevelNodeFunction)_GLFProfile.Group_Level_Nodes[selectedValue];

                // Begin TT#1874 - JSmith - Min Dissappearing
                if (setDefaultGLNF == null)
                {
                    setDefaultGLNF = new GroupLevelNodeFunction();
                    setDefaultGLNF.HN_RID = selectedValue;
                    setDefaultGLNF.SglRID = _GLFProfile.Key;
                    setDefaultGLNF.MethodRID = _OTSPlanMethod.Key;
                    _GLFProfile.Group_Level_Nodes.Add(selectedValue, setDefaultGLNF);
                }
                // End TT#1874

                PopulateStockMinMax(setDefaultGLNF);
            }
            catch
            {
                throw;
            }
        } 
        // End TT#3

		private void CopyTYLYTrendBasis(int copyToRid)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
                CopyEqualize(copyToRid);  // Issue 5420 - KJohnson
				CopySmoothing(copyToRid);  // Issue 3816 - stodd
				CopyBasis(copyToRid, _dtSourceTYNode);
				//CopyBasis(copyToRid, _dtSourceTYTime);	// Issue 4818
				CopyBasis(copyToRid, _dtSourceLYNode);
				//CopyBasis(copyToRid, _dtSourceLYTime);	// Issue 4818
				CopyBasis(copyToRid, _dtSourceTrendNode);
				//CopyBasis(copyToRid,_dtSourceTrendTime);	// Issue 4818

				// Since the trend caps data is stored in the GLF profile list,
				// we need to find it and set those same selections for this set.
				CopyTrend(copyToRid);
			}
			catch
			{
				throw;
			}
		}

		private void CopyBasis(int copyToRid, DataTable aDataTable)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			ArrayList oldRows = new ArrayList();
			ArrayList newRows = new ArrayList();

			foreach ( DataRow basisRow in aDataTable.Rows)
			{
				int rid = Convert.ToInt32(basisRow["SGL_RID"], CultureInfo.CurrentUICulture);
				if (rid == copyToRid)
				{
					oldRows.Add(basisRow);
				}

				if (rid == _defaultStoreGroupLevelRid)
				{
					DataRow newRow = aDataTable.NewRow();

					Array basisData = basisRow.ItemArray;
					for (int i=0;i<basisData.Length;i++)
					{
						newRow[i] = basisData.GetValue(i);
					}
					newRow["SGL_RID"] = copyToRid;

					// Begin Issue 3816 - stodd
					if (newRow.Table.Columns.Contains("CDR_RID"))
					{
						if (basisRow["CDR_RID"] != DBNull.Value)
						{
							int cdr = Convert.ToInt32(basisRow["CDR_RID"], CultureInfo.CurrentUICulture);
							DateRangeProfile drp = SAB.ApplicationServerSession.Calendar.GetDateRange(cdr);
							if (drp.Name.Trim().Length == 0)  // only dup date ranges that DON'T have a name
							{
								drp = SAB.ApplicationServerSession.Calendar.GetDateRangeClone(cdr);
								newRow["CDR_RID"] = drp.Key;
							}
						}
					}
					// end Issue 3816

					newRows.Add(newRow);
				}
			}
			// remove any old rows
			foreach (DataRow oldRow in oldRows)
			{
				aDataTable.Rows.Remove(oldRow);
			}
			// Add new defualt basis rows
			foreach (DataRow newRow in newRows)
			{
				aDataTable.Rows.Add(newRow);
			}
		}
	
		private void CopyTrend(int copyToRid)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				if (_defaultStoreGroupLevelRid == (int)cbxStoreGroupLevel.SelectedValue)
				{
					GroupLevelFunctionProfile glfp = (GroupLevelFunctionProfile)_GLFProfileList.FindKey(copyToRid);
					foreach (TrendCapsProfile tcp in glfp.Trend_Caps)
					{
						if (radNone.Checked)
						{	
							tcp.TrendCapID = eTrendCapID.None;
							tcp.TolPct = Include.UndefinedDouble;
							tcp.HighLimit = Include.UndefinedDouble;
							tcp.LowLimit = Include.UndefinedDouble;
						}
						else if (radTolerance.Checked)
						{
							tcp.TrendCapID = eTrendCapID.Tolerance; 
							try
							{
								tcp.TolPct = Convert.ToDouble(txtTolerance.Text, CultureInfo.CurrentUICulture);
							}
							catch
							{
								tcp.TolPct = Include.UndefinedDouble;
							}
							tcp.HighLimit = Include.UndefinedDouble;
							tcp.LowLimit = Include.UndefinedDouble;
						}
						else if (radLimits.Checked)
						{
							tcp.TrendCapID = eTrendCapID.Limits;
							tcp.TolPct = Include.UndefinedDouble;
							if (txtHigh.Text != string.Empty)
								tcp.HighLimit = Convert.ToDouble(txtHigh.Text, CultureInfo.CurrentUICulture);
							else
								tcp.HighLimit = Include.UndefinedDouble;
							if (txtLow.Text != string.Empty)
								tcp.LowLimit = Convert.ToDouble(txtLow.Text, CultureInfo.CurrentUICulture);
							else
								tcp.LowLimit = Include.UndefinedDouble;
						}
					}

					glfp.LY_Alt_IND = this.cbxAltLY.Checked;
					glfp.Trend_Alt_IND = this.cbxAltTrend.Checked;

                    //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
                    glfp.Proj_Curr_Wk_Sales_IND = this.cbxProjCurrWkSales.Checked;
                    //END TT#43 - MD - DOConnell - Projected Sales Enhancement
				}
				else
				{
					foreach (GroupLevelFunctionProfile glfp in _GLFProfileList.ArrayList)
					{
						if (glfp.Key == _defaultStoreGroupLevelRid)
						{
							foreach (TrendCapsProfile tcp in glfp.Trend_Caps)
							{
								if (tcp.TrendCapID == eTrendCapID.None)
									radNone.Checked = true;
								else if (tcp.TrendCapID == eTrendCapID.Tolerance)
								{
									radTolerance.Checked = true;
									txtTolerance.Text = Convert.ToString(tcp.TolPct, CultureInfo.CurrentUICulture);
								}
								else if (tcp.TrendCapID == eTrendCapID.Limits)
								{
									radLimits.Checked = true;
									if (tcp.HighLimit != Include.UndefinedDouble)
										txtHigh.Text = Convert.ToString(tcp.HighLimit, CultureInfo.CurrentUICulture);
									if (tcp.LowLimit  != Include.UndefinedDouble)
										txtLow.Text = Convert.ToString(tcp.LowLimit, CultureInfo.CurrentUICulture);
								}
							}
							this.cbxAltLY.Checked = glfp.LY_Alt_IND;
							this.cbxAltTrend.Checked = glfp.Trend_Alt_IND;
						}
					}
				}
			}
			catch
			{
				throw;
			}
		}

		// Begin Issue 3816 - stodd
		/// <summary>
		/// updates all of the sets using default to match the default.
		/// Called after changes have been made to default.
		/// </summary>
        /// 
        private void UpdateSetsUsingDefault()
        {
        //   // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                if (FormLoaded && !_dragAndDrop && !_InitializingRow)     //MID Track #5954 - KJohnson - Issue #6 - changing the default causes unreconciled data to happen
                {
                    // Begin TT#374-MD - JSmith - default set selected projected sales set using the default did not reflect the selection
                    //foreach (GroupLevelFunctionProfile glfp in _OTSPlanMethod.GLFProfileList.ArrayList)
                    //{
                    //    if (glfp.Use_Default_IND)
                    //    {
                    //        if (_defaultFunctionType == (int)eGroupLevelFunctionType.PercentContribution)
                    //        {
                    //            CopyPercentContributionBasis(glfp.Key);
                    //        }
                    //        else if (_defaultFunctionType == (int)eGroupLevelFunctionType.TyLyTrend)
                    //        {
                    //            CopyTYLYTrendBasis(glfp.Key);
                    //        }
                    //    }
                    //}
                    for (int i = 0; i < _OTSPlanMethod.GLFProfileList.Count; i++)
                    {
                        GroupLevelFunctionProfile glfp = (GroupLevelFunctionProfile)_OTSPlanMethod.GLFProfileList[i];
                        if (glfp.Use_Default_IND)
                        {
                            glfp = DefaultGLFProfile.CopyTo(glfp, SAB.ClientServerSession, false, true);
                            glfp.Default_IND = false;
                            glfp.Use_Default_IND = true;
                            if (_defaultFunctionType == (int)eGroupLevelFunctionType.PercentContribution)
                            {
                                CopyPercentContributionBasis(glfp.Key);
                            }
                            else if (_defaultFunctionType == (int)eGroupLevelFunctionType.TyLyTrend)
                            {
                                CopyTYLYTrendBasis(glfp.Key);
                            }
                        }
                    }
                    // End TT#374-MD - JSmith - default set selected projected sales set using the default did not reflect the selection
                }
            }
            catch
            {
                throw;
            }
        }
        // End Issue 3816 - stodd

		private void GridMouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			Infragistics.Win.UltraWinGrid.UltraGrid grid = (UltraGrid)sender;
			try
			{
				ShowUltraGridToolTip(grid, e);
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void gridStockMinMax_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				//e.Layout.AutoFitColumns = true;

				//Prevent the user from re-arranging columns.
				gridStockMinMax.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;

				gridStockMinMax.DisplayLayout.Bands[0].Columns["Boundary"].Hidden = true;
				gridStockMinMax.DisplayLayout.Bands[0].Columns["SGL_RID"].Hidden = true;
				gridStockMinMax.DisplayLayout.Bands[0].Columns["Picture"].Hidden = true;
				gridStockMinMax.DisplayLayout.Bands[0].Columns["HN_RID"].Hidden = true;

				gridStockMinMax.DisplayLayout.Bands[0].Columns["StoreGrade"].Width = 180;
				gridStockMinMax.DisplayLayout.Bands[0].Columns["StoreGrade"].CellActivation = Activation.NoEdit;
				gridStockMinMax.DisplayLayout.Bands[0].Columns["StoreGrade"].Header.VisiblePosition = 1;
				gridStockMinMax.DisplayLayout.Bands[0].Columns["StoreGrade"].Header.Caption = "Store Grade";
				gridStockMinMax.DisplayLayout.Bands[0].Columns["DateRange"].Header.Caption = " ";
				gridStockMinMax.DisplayLayout.Bands[0].Columns["DateRange"].Header.VisiblePosition = 2;
				//gridStockMinMax.DisplayLayout.Bands[0].Columns["Picture"].Width = 20;
				//gridStockMinMax.DisplayLayout.Bands[0].Columns["Picture"].Header.Caption = " ";
				//gridStockMinMax.DisplayLayout.Bands[0].Columns["Picture"].Header.VisiblePosition = 3;
				
				//Make some columns readonly.
				gridStockMinMax.DisplayLayout.Bands[0].Columns["StoreGrade"].CellActivation = Activation.NoEdit;
				gridStockMinMax.DisplayLayout.Bands[0].Columns["DateRange"].CellActivation = Activation.NoEdit;
				//gridStockMinMax.DisplayLayout.Bands[0].Columns["Picture"].CellActivation = Activation.NoEdit;

				// this basically disables the "store grade" add new box
				gridStockMinMax.DisplayLayout.Bands[0].Override.AllowAddNew = AllowAddNew.No;

				gridStockMinMax.DisplayLayout.Bands[1].Columns["BOUNDARY"].Hidden = true;
				gridStockMinMax.DisplayLayout.Bands[1].Columns["StoreGrade"].Header.VisiblePosition = 1;
				gridStockMinMax.DisplayLayout.Bands[1].Columns["StoreGrade"].Header.Caption = " ";
				gridStockMinMax.DisplayLayout.Bands[1].Columns["DateRange"].Header.VisiblePosition = 2;
				gridStockMinMax.DisplayLayout.Bands[1].Columns["DateRange"].Header.Caption = "Date Range";
				gridStockMinMax.DisplayLayout.Bands[1].Columns["DateRange"].Width = 180;
				gridStockMinMax.DisplayLayout.Bands[1].Columns["DateRange"].CellActivation = Activation.NoEdit;

				//gridStockMinMax.DisplayLayout.Bands[1].Columns["Picture"].Width = 20;
				//gridStockMinMax.DisplayLayout.Bands[1].Columns["Picture"].Header.Caption = " ";
				//gridStockMinMax.DisplayLayout.Bands[1].Columns["Picture"].Header.VisiblePosition = 3;
				//gridStockMinMax.DisplayLayout.Bands[1].Columns["Picture"].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
				//gridStockMinMax.DisplayLayout.Bands[1].Columns["Picture"].CellAppearance.ImageVAlign = Infragistics.Win.VAlign.Middle;
				gridStockMinMax.DisplayLayout.Bands[1].Columns["Picture"].Hidden = true;
				gridStockMinMax.DisplayLayout.Bands[1].Columns["METHOD_RID"].Hidden = true;
				gridStockMinMax.DisplayLayout.Bands[1].Columns["SGL_RID"].Hidden = true;
				gridStockMinMax.DisplayLayout.Bands[1].Columns["CDR_RID"].Hidden = true;
				gridStockMinMax.DisplayLayout.Bands[1].Columns["HN_RID"].Hidden = true;
				gridStockMinMax.DisplayLayout.Bands[1].Columns["MIN_STOCK"].Header.Caption = "Minimum ";
				gridStockMinMax.DisplayLayout.Bands[1].Columns["MIN_STOCK"].Width = 80;
				gridStockMinMax.DisplayLayout.Bands[1].Columns["MAX_STOCK"].Header.Caption = "Maximum";
				gridStockMinMax.DisplayLayout.Bands[1].Columns["MAX_STOCK"].Width = 80;
				gridStockMinMax.DisplayLayout.Bands[1].Columns["DateRange"].Style  = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;

				gridStockMinMax.DisplayLayout.Bands[0].Columns["Minimum"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				gridStockMinMax.DisplayLayout.Bands[0].Columns["Maximum"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				gridStockMinMax.DisplayLayout.Bands[1].Columns["MIN_STOCK"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				gridStockMinMax.DisplayLayout.Bands[1].Columns["MAX_STOCK"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;


				//Make some columns readonly.
				gridStockMinMax.DisplayLayout.Bands[1].Columns["StoreGrade"].CellActivation = Activation.NoEdit;
				//gridStockMinMax.DisplayLayout.Bands[1].Columns["Picture"].CellActivation = Activation.NoEdit;

				gridStockMinMax.DisplayLayout.Bands[1].AddButtonCaption = "Add New Stock Min/Max";
				gridStockMinMax.DisplayLayout.Bands[1].AddButtonToolTipText = "Click to add new Stock Min/Max.";
				gridStockMinMax.DisplayLayout.AddNewBox.Hidden = false;
				gridStockMinMax.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;

                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer' 
                //FormatColumns(this.gridStockMinMax);
                //End TT#169

				if (!FunctionSecurity.AllowUpdate)
				{
					gridStockMinMax.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
					gridStockMinMax.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

        //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer' 
        //private void FormatColumns(Infragistics.Win.UltraWinGrid.UltraGrid ultragid)
        //{
        //    try
        //    {
        //        foreach (Infragistics.Win.UltraWinGrid.UltraGridBand oBand in ultragid.DisplayLayout.Bands)
        //        {
        //            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn oColumn in oBand.Columns)
        //            {
        //                switch (oColumn.DataType.ToString())
        //                {
        //                    case "System.Int32":
        //                        oColumn.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
        //                        oColumn.Format = "#,###,##0";
        //                        oColumn.MaskInput = "9999999";
        //                        oColumn.PromptChar = ' ';
        //                        break;
        //                    case "System.Double":
        //                        oColumn.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
        //                        oColumn.Format = "#,###,###.00";
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        HandleException(exception);
        //    }
        //}
        //End TT#169

		private void gridStockMinMax_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			//			if (CheckInsertCondition_DateRange(gridTYTimeWeight, e) == false)
			//			{
			//				e.Cancel = true;
			//				return;
			//			}

			try
			{
				this.Cursor = Cursors.WaitCursor;

				int boundary = Convert.ToInt32(e.ParentRow.Cells["Boundary"].Value, CultureInfo.CurrentUICulture);
				int hnRID = Convert.ToInt32(e.ParentRow.Cells["HN_RID"].Value, CultureInfo.CurrentUICulture);

				//Handle the row Sequence (pk for the grid)
				DataRow newRow = (DataRow)_dsStockMinMax.Tables["StockMinMax"].NewRow();
				newRow["METHOD_RID"] = this._OTSPlanMethod.Key;
				newRow["SGL_RID"] = Convert.ToInt32(this.cbxStoreGroupLevel.SelectedValue,CultureInfo.CurrentUICulture);
				newRow["BOUNDARY"] = boundary;
				newRow["HN_RID"] = hnRID;

				_dsStockMinMax.Tables["StockMinMax"].Rows.Add(newRow);

				//Set the active row to this newly added Basis row.
				//				int lastChild = gridStockMinMax.ActiveRow.ChildBands[0].Rows.Count;
				//gridStockMinMax.ActiveRow = gridStockMinMax.ActiveRow.ChildBands[0].Rows[--lastChild];
				gridStockMinMax.ActiveRow = e.ParentRow;
				//Since we've already added the necessary information in the underlying
				//datatable, we want to cancel out because if we don't, the grid will
				//add another blank row (in addition to the row we just added to the datatable).
				e.Cancel = true;

				this.Cursor = Cursors.Default;
			}
			catch(Exception err)
			{
				HandleException(err);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		private void gridStockMinMax_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (e.Row.Band.Index == 1)
			{
				//Populate cell w/text description of Date Range
				if (e.Row.Cells["CDR_RID"].Value.ToString() != "")
				{
					DateRangeProfile dr;
					if (midDateRangeSelectorOTSPlan.Tag != null)
						dr = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(e.Row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture),(int)midDateRangeSelectorOTSPlan.Tag);
					else
						dr = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(e.Row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture));

					e.Row.Cells["DateRange"].Value = dr.DisplayDate;
					e.Row.Cells["CDR_RID"].Value = dr.Key;
					e.Row.Cells["StoreGrade"].Value = "(Default)";

					if (dr.DateRangeType == eCalendarRangeType.Dynamic)
					{
						switch (dr.RelativeTo)
						{
							case eDateRangeRelativeTo.Current:
								e.Row.Cells["DateRange"].Appearance.Image = DynamicToCurrentImage;
								break;
							case eDateRangeRelativeTo.Plan:
								e.Row.Cells["DateRange"].Appearance.Image = DynamicToPlanImage;
								break;
							default:
								e.Row.Cells["DateRange"].Appearance.Image = null;
								break;
						}
					}
				}
				else
				{
					e.Row.Cells["StoreGrade"].Value = "(Default)";
				}
			}
		}

		private void gridStockMinMax_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			// we can use the same logic the TY/LY grids use...
			AllGrids_ClickCellButton(e, "CDR_RID", false, false);
		}

		private void gridStockMinMax_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (FormLoaded)
			{
				GroupLevelNodeFunction GLNFunction;
				int selectedValue = GetSelectedValue();

				// Begin Issue 4529 stodd 08.21.07
				if (selectedValue != Include.Undefined)
				{
					if(_GLFProfile.Group_Level_Nodes.ContainsKey(selectedValue))
					{
						GLNFunction = (GroupLevelNodeFunction)_GLFProfile.Group_Level_Nodes[selectedValue];
					}
					else
					{
						GLNFunction = new GroupLevelNodeFunction();
						GLNFunction.HN_RID = selectedValue;
						GLNFunction.SglRID = _GLFProfile.Key;
						GLNFunction.MethodRID = _OTSPlanMethod.Key;
						_GLFProfile.Group_Level_Nodes.Add(selectedValue, GLNFunction);
					}

					_InitialPopulate = true;
					ChangePending = true;
					// Stodd
					if (GLNFunction.isHighLevel)
					{
						if (this.rdoHierarchy.Checked)
						{
							_OTSPlanMethod.MinMaxInheritedFrom = eInheritedFrom.None;
							rdoNone.Checked = true;
							InheritanceProvider.SetError (gridStockMinMax,string.Empty);
						}
					}
					else
					{
						_OTSPlanMethod.MinMaxInheritedFrom = eInheritedFrom.None;
						rdoNone.Checked = true;
						InheritanceProvider.SetError (gridStockMinMax,string.Empty);
					}
					_InitialPopulate = false;
				}
				// End Issue 4529
			}
		}

        //private void radGlobal_CheckedChanged(object sender, System.EventArgs e)
        //{
        //    if (radGlobal.Checked)
        //    {
        //        FunctionSecurity = GlobalSecurity;
        //    }
        //    ApplySecurity();
        //}

        //private void radUser_CheckedChanged(object sender, System.EventArgs e)
        //{
        //    if (radUser.Checked)
        //    {
        //        FunctionSecurity = UserSecurity;
        //    }
        //    ApplySecurity();
        //}

        // Begin Track #4872 - JSmith - Global/User Attributes
        override protected void BuildAttributeList()
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            ProfileList pl;
            int currValue;
            bool setInitialPopulate = false;
            try
            {
                // if not _InitialPopulate, set to true so warning does not appear
                if (!_InitialPopulate)
                {
                    _InitialPopulate = true;
                    setInitialPopulate = true;
                }
                if (cboStoreGroups.SelectedValue != null &&
                    cboStoreGroups.SelectedValue.GetType() == typeof(System.Int32))
                {
                    currValue = Convert.ToInt32(cboStoreGroups.SelectedValue);
                }
                else
                {
                    currValue = Include.NoRID;
                }
                pl = GetStoreGroupList(_OTSPlanMethod.Method_Change_Type, _OTSPlanMethod.GlobalUserType, false);
                cboStoreGroups.Initialize(SAB, FunctionSecurity, pl.ArrayList, _OTSPlanMethod.GlobalUserType == eGlobalUserType.User);
                if (currValue != Include.NoRID)
                {
                    cboStoreGroups.SelectedValue = currValue;
                }
// BEGIN TT#7 - RBeck - Dynamic dropdowns
                cboStoreGroups.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
                cboStoreGroups.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
                AdjustTextWidthComboBox_DropDown(cboStoreGroups);
// END TT#7 - RBeck - Dynamic dropdowns
            }
            catch
            {
                throw;
            }
            finally
            {
                if (setInitialPopulate)
                {
                    _InitialPopulate = false;
                }
            }
        }
        // End Track #4872

		override protected bool ApplySecurity()	// track 5871 stodd
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
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

			// Begin MID Issue 2612 - stodd
			
			if (_nodeRID != Include.NoRID)
			{
				_hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_nodeRID, (int)eSecurityTypes.Store);
				if (!_hierNodeSecurity.AllowUpdate)
				{
					btnProcess.Enabled = false;
				}
			}

            if (securityOk)
                securityOk = (((MIDControlTag)(txtOTSHNDesc.Tag)).IsAuthorized(eSecurityTypes.Store, eSecuritySelectType.Update));


            if (!base.ValidateStorePlanVersionSecurity(this.cboPlanVers.ComboBox))          // TT#7 - RBeck - Dynamic dropdowns
            {
                securityOk = false;
            }
            else
            {
                ErrorProvider.SetError(cboPlanVers, string.Empty);
            }

            if (!base.ValidateStorePlanVersionSecurity(this.cboChainVers.ComboBox))         // TT#7 - RBeck - Dynamic dropdowns
            {
                securityOk = false;
            }
            else
            {
                ErrorProvider.SetError(cboChainVers, string.Empty);
            }
            // End Track #5926

			// End issue 2612
			return securityOk;	// track 5871 stodd
		}

		// Begin Issue 3816 - stodd
		private void cboGLGroupBy_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (FormLoaded)
			{
                // Begin TT#2647 - JSmith - Delays in OTS Method
                //int sgl = (int)cbxStoreGroupLevel.SelectedValue;
                //if (sgl == _defaultStoreGroupLevelRid)
                //    UpdateSetsUsingDefault();
                // End TT#2647 - JSmith - Delays in OTS Method

                if (!_storeGroupLevelChanged)
                {
                    ChangePending = true;
                }
			}
		}

		private void txtHigh_Leave(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (FormLoaded)
			{
                // Begin TT#2647 - JSmith - Delays in OTS Method
                //int sgl = (int)cbxStoreGroupLevel.SelectedValue;
                //if (sgl == _defaultStoreGroupLevelRid)
                //    UpdateSetsUsingDefault();
                // End TT#2647 - JSmith - Delays in OTS Method
			}
		}

		private void txtLow_Leave(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (FormLoaded)
			{
                // Begin TT#2647 - JSmith - Delays in OTS Method
                //int sgl = (int)cbxStoreGroupLevel.SelectedValue;
                //if (sgl == _defaultStoreGroupLevelRid)
                //    UpdateSetsUsingDefault();
                // End TT#2647 - JSmith - Delays in OTS Method
			}
		}

		private void txtTolerance_Leave(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (FormLoaded)
			{
                // Begin TT#2647 - JSmith - Delays in OTS Method
                //int sgl = (int)cbxStoreGroupLevel.SelectedValue;
                //if (sgl == _defaultStoreGroupLevelRid)
                //    UpdateSetsUsingDefault();
                // End TT#2647 - JSmith - Delays in OTS Method
			}
		}
		// End issue 3816

		private void chkOTSSales_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		private void chkOTSStock_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		private void cboChainVers_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (FormLoaded)
			{
				ChangePending = true;
				// BEGIN Issue 4858 stodd
				this._OTSPlanMethod.Chain_FV_RID = (int)this.cboChainVers.SelectedValue;
				bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
				base.ApplyCanUpdate(canUpdate);
				// END Issue 4858 stodd
			}
		}

		private void txtOTSHNDesc_TextChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		private void chkPlan_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		private void grid_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		//Begin Track #4371 - JSmith - Multi-level forecasting.
		private void chkHighLevel_MouseHover(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			string message = null;
			if (chkHighLevel.Checked)
			{
				message = MIDText.GetTextOnly(eMIDTextCode.tt_UncheckToNotForecast);
			}
			else
			{
				message = MIDText.GetTextOnly(eMIDTextCode.tt_CheckToForecast);
			}
			message = message.Replace("{0}", txtOTSHNDesc.Text);
			ShowToolTip(sender, e, message);
		}

		private void chkLowLevels_MouseHover(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			string message = null;
			if (chkLowLevels.Checked)
			{
				message = MIDText.GetTextOnly(eMIDTextCode.tt_UncheckToNotForecastLowLevels);
			}
			else
			{
				message = MIDText.GetTextOnly(eMIDTextCode.tt_CheckToForecastLowLevels);
			}
			ShowToolTip(sender, e, message);
		}

		private void rdoHierarchy_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if(rdoHierarchy.Checked)
			{
                // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                //ResetMinMaxDisplay(eMinMaxInheritType.Hierarchy);
                if (!_storeGroupLevelChanged)
                {
                    ResetMinMaxDisplay(eMinMaxInheritType.Hierarchy);
                }
                gridStockMinMax.Enabled = true;
                // End TT#3
			}
		}

		private void rdoMethod_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if(rdoMethod.Checked)
			{
                // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                //ResetMinMaxDisplay(eMinMaxInheritType.Method);
                if (!_storeGroupLevelChanged)
                {
                    ResetMinMaxDisplay(eMinMaxInheritType.Method);
                }
                gridStockMinMax.Enabled = true;
                // End TT#3
			}
		}

		private void rdoNone_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if(rdoNone.Checked)
			{
                // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                //ResetMinMaxDisplay(eMinMaxInheritType.None);
                if (!_storeGroupLevelChanged)
                {
                    ResetMinMaxDisplay(eMinMaxInheritType.None);
                }
                gridStockMinMax.Enabled = true;
                // End TT#3
			}
		}

        // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
        private void rdoDefault_CheckedChanged(object sender, EventArgs e)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (FormLoaded &&
                rdoDefault.Checked)
            {
                if (!_storeGroupLevelChanged)
                {
                    ResetMinMaxDisplay(eMinMaxInheritType.Default, true);
                }
            }
        }
        // End TT#3

        // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
        private void ResetMinMaxDisplay(eMinMaxInheritType inheritType)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (!_storeGroupLevelChanged)
            {
                ResetMinMaxDisplay(inheritType, false);
            }
        }
        // End TT#3

        // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
        //private void ResetMinMaxDisplay(eMinMaxInheritType inheritType)
        private void ResetMinMaxDisplay(eMinMaxInheritType inheritType, bool setupUI)
        // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			GroupLevelNodeFunction GLNFunction;
			int selectedValue = GetSelectedValue();

			// Begin Issue 4529 stodd 08.21.07
			if (selectedValue != Include.Undefined)
			{
				if(_GLFProfile.Group_Level_Nodes.ContainsKey(selectedValue))
				{
					GLNFunction = (GroupLevelNodeFunction)_GLFProfile.Group_Level_Nodes[selectedValue];
				}
				else
				{
					GLNFunction = new GroupLevelNodeFunction();
					GLNFunction.HN_RID = selectedValue;
					GLNFunction.SglRID = _GLFProfile.Key;
					GLNFunction.MethodRID = _OTSPlanMethod.Key;
					_GLFProfile.Group_Level_Nodes.Add(selectedValue, GLNFunction);
				}

				GLNFunction.MinMaxInheritType = inheritType;
				if(!_InitialPopulate)
                    // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                    //populateGLNF(false);
                    populateGLNF(setupUI);
                // End TT#3
			}
			// End Issue 4529
		}
		private void ReloadNodeStockMinMax(GroupLevelNodeFunction GLNFunction)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			// Stodd
			if(GLNFunction.MinMaxInheritType == eMinMaxInheritType.None && !GLNFunction.isHighLevel)
			{
				ComboObject highLevelObject = (ComboObject)cboMerchandise.Items[0];
				int highLevelSelectedValue = highLevelObject.Key;
				if(_GLFProfile.Group_Level_Nodes.ContainsKey(highLevelSelectedValue))
				{
					GroupLevelNodeFunction HighLevelGLNFunction = (GroupLevelNodeFunction)_GLFProfile.Group_Level_Nodes[highLevelSelectedValue];
						
					if(HighLevelGLNFunction.Stock_MinMax.Count == 0)
						_OTSPlanMethod.ReloadNodeStockMinMax(GLNFunction);
					else
					{
						GLNFunction.Stock_MinMax.Clear();
						foreach(StockMinMaxProfile smmp in HighLevelGLNFunction.Stock_MinMax)
						{
							StockMinMaxProfile newSMMP = new StockMinMaxProfile(smmp.Key);
							newSMMP.HN_RID = GLNFunction.HN_RID;
							newSMMP.MethodRid = smmp.MethodRid;
							newSMMP.StoreGroupLevelRid = smmp.StoreGroupLevelRid;
							newSMMP.Boundary = smmp.Boundary;
							newSMMP.DateRangeRid = smmp.DateRangeRid;
							newSMMP.MaximumStock = smmp.MaximumStock;
							newSMMP.MinimumStock = smmp.MinimumStock;
							GLNFunction.Stock_MinMax.Add(newSMMP);
						}
					}
				}
			}
			else
			{
				_OTSPlanMethod.ReloadNodeStockMinMax(GLNFunction);
			}
		}

		private void chkApplyMinMaxes_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			GroupLevelNodeFunction GLNFunction;
			int selectedValue = GetSelectedValue();

			if(_GLFProfile.Group_Level_Nodes.ContainsKey(selectedValue))
			{
				GLNFunction = (GroupLevelNodeFunction)_GLFProfile.Group_Level_Nodes[selectedValue];
				GLNFunction.ApplyMinMaxesInd = chkApplyMinMaxes.Checked;
			}
		}

		private void cboMerchandise_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (!ValidStockMinMax())
			{
				cboMerchandise.SelectedIndex = _currentMerchandiseIndex;
			}
			else
			{
				_currentMerchandiseIndex = cboMerchandise.SelectedIndex;
				populateGLNF(true);
		
				// Begin Issue 4573 stodd 08.20.07
				if(this.chkLowLevels.Checked)
				{
					if(cboMerchandise.SelectedIndex == 0)
						lblLowLevelDefault.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_LowLevelDefault);
					else	
						lblLowLevelDefault.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_LowLevelSetting);
				}
				else
					lblLowLevelDefault.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_InheritFrom);
				// End Issue 4573					
			}
		}

		private void populateGLNF(bool setupUI)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			GroupLevelNodeFunction GLNFunction;
			
			int selectedValue = GetSelectedValue();

			// Begin Issue 4529 stodd 08.21.07
			if (selectedValue != Include.Undefined)
			{
				if(_GLFProfile.Group_Level_Nodes.ContainsKey(selectedValue))
				{
					GLNFunction = (GroupLevelNodeFunction)_GLFProfile.Group_Level_Nodes[selectedValue];

					// stodd
					if(cboMerchandise.SelectedIndex == 0)
					{
						GLNFunction.isHighLevel = true;
					}

					// stodd
					if(GLNFunction.MinMaxInheritType == eMinMaxInheritType.Hierarchy)
						ReloadNodeStockMinMax(GLNFunction);

					if(setupUI) SetupGLNFUI(GLNFunction);
					// Populate MinMax
					PopulateStockMinMax(GLNFunction);
				}
				else
				{
					// Setup new object
					GLNFunction = new GroupLevelNodeFunction();
					GLNFunction.HN_RID = selectedValue;
					GLNFunction.MethodRID = _OTSPlanMethod.Key;
					GLNFunction.SglRID = (int)cbxStoreGroupLevel.SelectedValue;
					GLNFunction.ApplyMinMaxesInd = true;
									
					// If high level and low levels are enabled, default to Method
					if(cboMerchandise.SelectedIndex == 0)
					{
						GLNFunction.MinMaxInheritType = chkLowLevels.Checked ? eMinMaxInheritType.Method : eMinMaxInheritType.None;
						GLNFunction.isHighLevel = true;
					}
					else
					{
						// If low level, find out what the high level is doing
						ComboObject highLevelObject = (ComboObject)cboMerchandise.Items[0];
						int highLevelSelectedValue = highLevelObject.Key;
						if(_GLFProfile.Group_Level_Nodes.ContainsKey(highLevelSelectedValue))
						{
							GroupLevelNodeFunction HighLevelGLNFunction = (GroupLevelNodeFunction)_GLFProfile.Group_Level_Nodes[highLevelSelectedValue];
							GLNFunction.MinMaxInheritType = HighLevelGLNFunction.MinMaxInheritType;
						}
						else
						{
							// Apply default if High Level is not found
							GLNFunction.MinMaxInheritType = eMinMaxInheritType.Method;
						}
						GLNFunction.isHighLevel = false;
					}

					// Add new object to hashtable
					ReloadNodeStockMinMax(GLNFunction);				
					_GLFProfile.Group_Level_Nodes.Add(selectedValue, GLNFunction);
					
					// Setup UI
					if(setupUI)SetupGLNFUI(GLNFunction);
					// Populate MinMax
					PopulateStockMinMax(GLNFunction);
				}
			}
			// End Issue 4529
		}

		private int GetSelectedValue()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if(cboMerchandise.Visible == true)
			{
				ComboObject selectedItem = (ComboObject)cboMerchandise.SelectedItem;
				if(selectedItem != null)
					return selectedItem.Key;
				else
				{
                    //Begin Track #5858 - KJohnson - Validating store security only
                    // Begin Issue 4529 stodd 08.21.07
                    if (((MIDTag)(txtOTSHNDesc.Tag)).MIDTagData != null)
                    {
                        HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)txtOTSHNDesc.Tag).MIDTagData;
                        return hnp.Key;
                    }
                    else
                    {
                        return Include.Undefined;
                    }
					// End Issue 4529
                    //End Track #5858
				}
			}
			else
			{
                //Begin Track #5858 - KJohnson - Validating store security only
                // Begin Issue 4529 stodd 08.21.07
                if (((MIDTag)(txtOTSHNDesc.Tag)).MIDTagData != null)
                {
                    HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)txtOTSHNDesc.Tag).MIDTagData;
                    return hnp.Key;
                }
                else
                {
                    return Include.Undefined;
                }
				// End Issue 4529
                //End Track #5858
			}
		}

		private void PopulateStockMinMax(GroupLevelNodeFunction GLNF)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			BuildStockMinMaxDBTable(GLNF);
			_dsStockMinMax = this.GetDataSourceStockMinMax(GLNF);
			this.gridStockMinMax.DataSource = null;
			this.gridStockMinMax.DataSource = _dsStockMinMax;

			HierarchyNodeProfile inheritanceProfile = null;
			string inheritMsg = null;
			switch (GLNF.MinMaxInheritType)
			{
				case eMinMaxInheritType.Hierarchy:
					//Begin Track #5378 - color and size not qualified
//					inheritanceProfile = SAB.HierarchyServerSession.GetNodeData(_OTSPlanMethod.MinMaxInheritedFromNodeRID);
					
                    // Begin TT#2647 - JSmith - Delays in OTS Method
                    //inheritanceProfile = SAB.HierarchyServerSession.GetNodeData(_OTSPlanMethod.MinMaxInheritedFromNodeRID, false, true);
                    if (!_nodesByRID.TryGetValue(_OTSPlanMethod.MinMaxInheritedFromNodeRID, out inheritanceProfile))
                    {
                        inheritanceProfile = SAB.HierarchyServerSession.GetNodeData(_OTSPlanMethod.MinMaxInheritedFromNodeRID, false, true);
                        _nodesByRID.Add(_OTSPlanMethod.MinMaxInheritedFromNodeRID, inheritanceProfile);
                        if (!_nodesByID.ContainsKey(inheritanceProfile.NodeID))
                        {
                            _nodesByID.Add(inheritanceProfile.NodeID, inheritanceProfile);
                        }
                    }
                    // End TT#2647 - JSmith - Delays in OTS Method
					//End Track #5378
					inheritMsg = _txtInheritedFrom + inheritanceProfile.Text;
					if (_OTSPlanMethod.MinMaxAttributeMismatch)
					{
						inheritMsg += Environment.NewLine + "Attribute Mismatch";
					}
					InheritanceProvider.SetError (this.gridStockMinMax,inheritMsg);
					break;
				case eMinMaxInheritType.Method:
					if (!GLNF.isHighLevel)
					{
						inheritMsg = _txtInheritedFrom + " Method";
						if (_OTSPlanMethod.MinMaxAttributeMismatch)
						{
							inheritMsg += Environment.NewLine + "Attribute Mismatch";
						}
						InheritanceProvider.SetError (this.gridStockMinMax,inheritMsg);
					}
					else
					{
						InheritanceProvider.SetError (this.gridStockMinMax,string.Empty);
					}
					break;
				default:
					InheritanceProvider.SetError (this.gridStockMinMax,string.Empty);
					break;
			}
		}

		private void SetupGLNFUI(GroupLevelNodeFunction GLNFunction)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			chkApplyMinMaxes.Checked = GLNFunction.ApplyMinMaxesInd;

			switch(GLNFunction.MinMaxInheritType)
			{
				case eMinMaxInheritType.Hierarchy:
					rdoHierarchy.Checked = true;
					rdoMethod.Checked = false;
					rdoNone.Checked = false;
                    // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                    rdoDefault.Checked = false;
                    gridStockMinMax.Enabled = true;
                    // End TT#3
					break;
				case eMinMaxInheritType.Method:
					rdoHierarchy.Checked = false;
					rdoMethod.Checked = true;
					rdoNone.Checked = false;
                    // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                    rdoDefault.Checked = false;
                    gridStockMinMax.Enabled = true;
                    // End TT#3
					break;
				case eMinMaxInheritType.None:
					rdoHierarchy.Checked = false;
					rdoMethod.Checked = false;
                    rdoNone.Checked = true;
                    // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                    rdoDefault.Checked = false;
                    gridStockMinMax.Enabled = true;
                    // End TT#3
					break;
                // Begin TT#3 - JSmith - Forecasting issues with Min/Max grid
                case eMinMaxInheritType.Default:
                    rdoHierarchy.Checked = false;
                    rdoMethod.Checked = false;
                    rdoNone.Checked = false;
                    rdoDefault.Checked = true;
                    gridStockMinMax.Enabled = false;
                    break;
                // End TT#3
			}
		}

		private void ClearGLNFUI()
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			// BEGIN Issue 5295 stodd
//			if(cboMerchandise.Visible)
//				cboMerchandise.SelectedIndex = 0;
			// END ISSUE 5295

			populateGLNF(true);

//			rdoNone.Checked = false;
//			rdoHierarchy.Checked = false;
//			rdoMethod.Checked = false;
//			GroupLevelNodeFunction GLNF = new GroupLevelNodeFunction();
//			GLNF.HN_RID= (int)this.txtOTSHNDesc.Tag;
//			GLNF.SglRID = (int)this.cboStoreGroupLevel.SelectedValue;
//			PopulateStockMinMax(GLNF);
//			DeleteAllMinMaxes();
		}

		private void gridStockMinMax_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
//			SetStockMinMax();
		}

		private void tabPageLY_Click(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		private void groupBox4_Enter(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
		}



		//End Track #4371

        private void rdoEqualizeYesTY_CheckedChanged(object sender, System.EventArgs e)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            _GLFProfile.TY_Weight_Multiple_Basis_Ind = true;

            // BEGIN Issue 5420 KJohnson - rdoEqualizeYesTY not holding it value
            if (chkDefault.Checked)
            {
                _defaultEqualizeTY = true;
            }
            // END Issue 5420 KJohnson

            if (!cbxAltLY.Checked)
                rdoEqualizeYesLY.Checked = true;

            if (!cbxAltTrend.Checked)
                rdoEqualizeYesTrend.Checked = true;

            // Begin TT#2647 - JSmith - Delays in OTS Method
            if (FormLoaded)
            {
                ChangePending = true;
            }
            // End TT#2647 - JSmith - Delays in OTS Method
        }

        private void rdoEqualizeNoTY_CheckedChanged(object sender, System.EventArgs e)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            _GLFProfile.TY_Weight_Multiple_Basis_Ind = false;

            // BEGIN Issue 5420 KJohnson - rdoEqualizeYesTY not holding it value
            if (chkDefault.Checked) 
            {
                _defaultEqualizeTY = false;
            }
            // END Issue 5420 KJohnson

            if (!cbxAltLY.Checked)
                rdoEqualizeYesLY.Checked = false;

            if (!cbxAltTrend.Checked)
                rdoEqualizeYesTrend.Checked = false;

            // Begin TT#2647 - JSmith - Delays in OTS Method
            if (FormLoaded)
            {
                ChangePending = true;
            }
            // End TT#2647 - JSmith - Delays in OTS Method
        }

		private void rdoEqualizeYesLY_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			_GLFProfile.LY_Weight_Multiple_Basis_Ind = true;

            // BEGIN Issue 5420 KJohnson - rdoEqualizeYesTY not holding it value
            if (chkDefault.Checked)
            {
                _defaultEqualizeLY = true;
            }
            // END Issue 5420 KJohnson

            // Begin TT#2647 - JSmith - Delays in OTS Method
            if (FormLoaded)
            {
                ChangePending = true;
            }
            // End TT#2647 - JSmith - Delays in OTS Method
		}

		private void rdoEqualizeNoLY_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			_GLFProfile.LY_Weight_Multiple_Basis_Ind = false;

            // BEGIN Issue 5420 KJohnson - rdoEqualizeYesTY not holding it value
            if (chkDefault.Checked)
            {
                _defaultEqualizeLY = false;
            }
            // END Issue 5420 KJohnson

            // Begin TT#2647 - JSmith - Delays in OTS Method
            if (FormLoaded)
            {
                ChangePending = true;
            }
            // End TT#2647 - JSmith - Delays in OTS Method
        }

		private void rdoEqualizeYesTrend_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			_GLFProfile.Apply_Weight_Multiple_Basis_Ind = true;

            // BEGIN Issue 5420 KJohnson - rdoEqualizeYesTY not holding it value
            if (chkDefault.Checked)
            {
                _defaultEqualizeTrend = true;
            }
            // END Issue 5420 KJohnson

            // Begin TT#2647 - JSmith - Delays in OTS Method
            if (FormLoaded)
            {
                ChangePending = true;
            }
            // End TT#2647 - JSmith - Delays in OTS Method
        }

		private void rdoEqualizeNoTrend_CheckedChanged(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			_GLFProfile.Apply_Weight_Multiple_Basis_Ind = false;

            // BEGIN Issue 5420 KJohnson - rdoEqualizeYesTY not holding it value
            if (chkDefault.Checked)
            {
                _defaultEqualizeTrend = false;
            }
            // END Issue 5420 KJohnson

            // Begin TT#2647 - JSmith - Delays in OTS Method
            if (FormLoaded)
            {
                ChangePending = true;
            }
            // End TT#2647 - JSmith - Delays in OTS Method
        }

        public void control_DragEnter(object sender, DragEventArgs e)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Image_DragEnter(sender, e);
        }

        public void control_DragOver(object sender, DragEventArgs e)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Image_DragOver(sender, e);
        }

		private void grid_AfterCellListCloseUp(object sender, CellEventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (e.Cell == e.Cell.Row.Cells["Merchandise"])
			{
				_MerchCellListClose = true;
			}
		}

        private void cboOverride_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (FormLoaded)
			{
				_OTSPlanMethod.OverrideLowLevelRid = ((ComboObject)cboOverride.SelectedItem).Key;
				ChangePending = true;
			}
        }

		// BEGIN Issue 5357 stodd
		private void gridTYNodeVersion_AfterRowsDeleted(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			_dtSourceTYNode.AcceptChanges();

            // Begin TT#2647 - JSmith - Delays in OTS Method
            //int sgl = (int)cbxStoreGroupLevel.SelectedValue;
            //if (sgl == _defaultStoreGroupLevelRid)
            //    UpdateSetsUsingDefault();
            // End TT#2647 - JSmith - Delays in OTS Method
		}

		private void gridLYNodeVersion_AfterRowsDeleted(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			_dtSourceLYNode.AcceptChanges();

            // Begin TT#2647 - JSmith - Delays in OTS Method
            //int sgl = (int)cbxStoreGroupLevel.SelectedValue;
            //if (sgl == _defaultStoreGroupLevelRid)
            //    UpdateSetsUsingDefault();
            // End TT#2647 - JSmith - Delays in OTS Method
		}

		private void gridTrendNodeVersion_AfterRowsDeleted(object sender, System.EventArgs e)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			_dtSourceTrendNode.AcceptChanges();

            // Begin TT#2647 - JSmith - Delays in OTS Method
            //int sgl = (int)cbxStoreGroupLevel.SelectedValue;
            //if (sgl == _defaultStoreGroupLevelRid)
            //    UpdateSetsUsingDefault();
            // End TT#2647 - JSmith - Delays in OTS Method
		}

        private void cboStoreGroups_DragEnter(object sender, DragEventArgs e)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Image_DragEnter(sender, e);
        }

        private void cboStoreGroups_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }
        // END Issue 5357 stodd

        // BEGIN TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 
        private void radChainPlan_CheckedChanged(object sender, EventArgs e)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (radChainPlan.Checked)
            {
                _ApplyTrendOptionsInd = 'C';
				// _ApplyTrendOptionsWOSValue = 0;   // TT#619 - Stodd - OTS Forecast - Chain Plan not required (#46) 
            }
        }

        private void radChainWOS_CheckedChanged(object sender, EventArgs e)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (radChainWOS.Checked)
            {
                _ApplyTrendOptionsInd = 'W';
				// _ApplyTrendOptionsWOSValue = 0;    // TT#619 - Stodd - OTS Forecast - Chain Plan not required (#46)
            }
        }

        private void radPlugChainWOS_CheckedChanged(object sender, EventArgs e)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (radPlugChainWOS.Checked)
			{
				_ApplyTrendOptionsInd = 'S';
				txtPlugChainWOS.Enabled = true;
			}
			else
			{
				txtPlugChainWOS.Enabled = false;
			}
        }

        private void txtPlugChainWOS_TextChanged(object sender, System.EventArgs e)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
				// BEGIN TT#619 - Stodd - OTS Forecast - Chain Plan not required (#46)
				try
				{
					if (txtPlugChainWOS.Text == null)
						_ApplyTrendOptionsWOSValue = 0;
					else
						_ApplyTrendOptionsWOSValue = (float)Convert.ToDouble(txtPlugChainWOS.Text);
				}
				catch
				{
					_ApplyTrendOptionsWOSValue = 0;
				}
				// END TT#619 - Stodd - OTS Forecast - Chain Plan not required (#46)
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // END TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 

		private void DebugGroupLevelForecastList(ProfileList GLFProfileList)
		{
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
			foreach (GroupLevelFunctionProfile glfp in GLFProfileList.ArrayList)
			{
				Debug.WriteLine(glfp.GLFT_ID + " Key: " + glfp.Key
					+ " isDefault: " + glfp.Default_IND
					+ " useDefault: " + glfp.Use_Default_IND
					+ " Forecast: " + glfp.Plan_IND);
			}
		}

        // Begin TT#272-MD - JSmith - Version 5.0 - General screen cleanup 
        private void pnlPercentContribution_VisibleChanged(object sender, EventArgs e)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (pnlPercentContribution.Visible)
            {
                this.pnlPercentContribution.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            }
        }

        private void pnlTYLYTrend_VisibleChanged(object sender, EventArgs e)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (pnlTYLYTrend.Visible)
            {
                this.pnlTYLYTrend.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            }
        }
        // End TT#272-MD - JSmith - Version 5.0 - General screen cleanup 

        private void cboPlanVers_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.cboPlanVers_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboLowLevels_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.cboLowLevels_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboOverride_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.cboOverride_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboChainVers_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.cboChainVers_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboModel_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.cboModel_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cbxStoreGroupLevel_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.cboStoreGroupLevel_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboStoreGroups_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.cboStoreGroups_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboGLGroupBy_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.cboGLGroupBy_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboFuncType_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.cboFuncType_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboMerchandise_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
           // Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.cboMerchandise_SelectionChangeCommitted(source, new EventArgs());
        }

        // Begin TT#284-MD - JSmith - OTS Forecast Method errors on open 
////Begin TT#7 R Beck - Dynamic dropdowns
//        /// <summary>
//        /// Change cboStoreGroupLevel selected value at DropDownClosed based on key of cboStoreGroups
//        /// (Change Store_Group_Levels based on selected Store_Group)
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void cboStoreGroups_DropDownClosed(object sender, EventArgs e)
//        {
//            try
//            {
//                this.Cursor = Cursors.WaitCursor;
//                return;
 
//                DialogResult dr = DialogResult.Yes;
//                if (!_InitialPopulate && _SelectedValue != (int)cboStoreGroups.SelectedValue) //TT#7 R Beck - Dynamic dropdowns
//                {
//                    _resetStoreGroup = false;
//                    dr = MessageBox.Show("Changing the current Attribute will abandon any changes to the Sets below.  Are you sure you want to continue?",
//                        MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanMethod), MessageBoxButtons.YesNo);
//                }

//                if (dr == DialogResult.No)
//                {
//                    _resetStoreGroup = true;
//                    this.cboStoreGroups.SelectedValue = this._OTSPlanMethod.SG_RID;
//                }
//                else if (dr == DialogResult.Yes)
//                {
//                    if (this.cboStoreGroups.SelectedValue != null  )
//                    {
//                        _SelectedValue = (int)cboStoreGroups.SelectedValue;    //TT#7 R Beck - Dynamic dropdowns

//                        _setChanged = true;
//                        if (!_InitialPopulate)
//                        {
//                            this._defaultChecked = false;
//                        }
//                        if (!(_newSetMethod))
//                            PopulateStoreGLComboCheck(this.cboStoreGroups.SelectedValue.ToString());
//                        else
//                            PopulateStoreGLCombo(this.cboStoreGroups.SelectedValue.ToString());

//                    }

//                    if (!_InitialPopulate)
//                    {
//                        this._GLFProfileList.Clear();
//                        SetMethodValues(_GLFProfile.GLF_Change_Type);
//                        // BEGIN Issue 5295 stodd
//                        if (cboMerchandise.Visible)
//                            cboMerchandise.SelectedIndex = 0;
//                        // END Issue
//                        cboStoreGroupLevelChange();
//                        ClearGLNFUI();
//                    }
//                    _setChanged = false;

//                    if (FormLoaded)
//                    {
//                        ChangePending = true;
//                    }
//                }

//                if (FormLoaded)
//                {
//                    ChangePending = true;
//                }
//            }
//            catch (Exception ex)
//            {
//                HandleException(ex, "cboStoreGroups_DropDownClosed");
//            }
//            finally
//            {
//                this.Cursor = Cursors.Default;
//            }
//        }

////End    TT#7 R Beck - Dynamic dropdowns
        // End TT#284-MD - JSmith - OTS Forecast Method errors on open 

	}

	

	// moved to its own source module because being in this module caused problems for designer
//	#region Workflow DataGrid Formatting
//	//delegate required by custom column style
//	public delegate int delegateGetIconIndexForRow(int row);
//
//	public class DataGridIconTextColumn : DataGridTextBoxColumn
//	{
//		private ImageList _icons;
//		delegateGetIconIndexForRow _getIconIndex;
//		
//		public DataGridIconTextColumn(ImageList Icons, delegateGetIconIndexForRow getIconIndex)
//		{
//			_icons = Icons;
//			_getIconIndex = getIconIndex;
//		}
//
//		protected override void Paint(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, System.Windows.Forms.CurrencyManager source, int rowNum, System.Drawing.Brush backBrush, System.Drawing.Brush foreBrush, bool alignToRight)
//		{
//			try
//			{
//				Image icon1 = this._icons.Images[_getIconIndex(rowNum)];
//				Rectangle rect = new Rectangle(bounds.X, bounds.Y, icon1.Size.Width, bounds.Height);
//				g.FillRectangle(backBrush, rect);
//				g.DrawImage(icon1, rect);
//
//				bounds.X = bounds.X + rect.Width;
//				bounds.Width = bounds.Width - rect.Width;
//				base.Paint(g, bounds, source, rowNum, backBrush, foreBrush, alignToRight);
//			}
//			
//			catch(Exception ex)
//			{
//				string exceptionMessage = ex.Message;
//			}
//		}
//	}
//	#endregion


	//	#region PropertyChangeEventArgs Class
	//
	//	public class PropertyChangeEventArgs : EventArgs
	//	{
	//		ApplicationBaseMethod _abm;
	//		Profile _p;
	//		bool _formClosing;
	//		
	//		public PropertyChangeEventArgs(ApplicationBaseMethod ABM)
	//		{
	//			_abm = ABM;
	//			_formClosing = false;
	//		}
	//		public PropertyChangeEventArgs(Profile p)
	//		{
	//			_p = p;
	//			_formClosing = false;
	//		}
	//		public bool FormClosing 
	//		{
	//			get { return _formClosing ; }
	//			set { _formClosing = value; }
	//		}
	//		public Profile p
	//		{
	//			get { return _p ; }
	//			set { _p = value; }
	//		}
	//		public ApplicationBaseMethod ABM 
	//		{
	//			get { return _abm ; }
	//			set { _abm = value; }
	//		}
	//	}
	//
	//	#endregion
}
