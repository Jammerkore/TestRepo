// Begin Track #4872 - JSmith - Global/User Attributes
// Renamed cboSet to cbxSet so it would not get protected in read only mode.
// End Track #4872
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinMaskedEdit;


using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for ForecastModifySales.
	/// </summary>
	public class frmModifySalesMethod : MIDRetail.Windows.WorkflowMethodFormBase
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TabControl tabModifySales;
        private System.Windows.Forms.TabPage tabPageMethod;
		private System.Windows.Forms.TabControl tabMatrix;
		private System.Windows.Forms.TabPage tabPageGrades;
        private System.Windows.Forms.TabPage tabPageMatrix;
        // Begin Track #4872 - JSmith - Global/User Attributes
        //private MIDRetail.Windows.Controls.MIDComboBoxEnh cboAttribute;
        private MIDAttributeComboBox cboAttribute;
        // End Track #4872
        private System.Windows.Forms.TextBox txtMerchandise;
		private Infragistics.Win.UltraWinGrid.UltraGrid gridSellThru;
		private Infragistics.Win.UltraWinGrid.UltraGrid gridGrades;
		private MIDRetail.Windows.Controls.MIDDateRangeSelector midDateRangeSelector;
		private Infragistics.Win.UltraWinGrid.UltraGrid gridMatrix;
		private OTSForecastModifySales _OTSForecastModifySales;

		private HierarchyNodeSecurityProfile _hierNodeSecurity;
		private ArrayList _userRIDList;
		//private StoreFilterData _storeFilterData;
        private FilterData _storeFilterData;
		private System.Windows.Forms.Label lblSet;
		private System.Windows.Forms.Label lblAttribute;
		private System.Windows.Forms.Label lblFilter;
		private System.Windows.Forms.Label lblDateRange;
		private System.Windows.Forms.Label lblMerchandise;
		private string _thisTitle;
		private int _nodeRID = Include.NoRID;
		private DataTable _dtMatrixGrid;
		private DataTable _dtRuleLabels;
		private DataTable _dtRules;
		private System.Windows.Forms.ContextMenu menuGrades;
		private System.Windows.Forms.ContextMenu menuSellThru;
        private bool _rebuildMatrix = false;
		private UltraGridCell _returnToCell = null;
        //private System.Windows.Forms.ImageList Icons;
		private UltraMaskedEdit _editorInt;
		private System.Windows.Forms.RadioButton rbAllStores;
		private System.Windows.Forms.RadioButton rbSet;
		private System.Windows.Forms.GroupBox gbAverage;
		private UltraMaskedEdit _editorDec;
		private int _currSet;
		private int _currAttribute;
		private bool _attributeReset = false;
        private TabPage tabPageProperties;
        private UltraGrid ugWorkflows;
        private MIDComboBoxEnh cboFilter;
        private MIDComboBoxEnh cbxSet;
		private ProfileList _versionProfileList;

		public frmModifySalesMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB) : base (SAB, aEAB, eMIDTextCode.frm_ForecastModifySales, eWorkflowMethodType.Method)
		{
			try
			{
				InitializeComponent();
				
				UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserOTSModifySales);
				GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalOTSModifySales);
			
			}
			catch(Exception ex)
			{
				HandleException(ex, "NewOTSForecastModifySales");
				FormLoadError = true;
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}

				this.txtMerchandise.Validating -= new System.ComponentModel.CancelEventHandler(this.txtMerchandise_Validating);
                this.txtMerchandise.Validated -= new System.EventHandler(this.txtMerchandise_Validated);
				this.txtMerchandise.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragDrop);
				this.txtMerchandise.TextChanged -= new System.EventHandler(this.txtMerchandise_TextChanged);
				this.txtMerchandise.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragEnter);
				this.midDateRangeSelector.Load -= new System.EventHandler(this.midDateRangeSelector_Load);
				this.midDateRangeSelector.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.midDateRangeSelector_ClickCellButton);
				this.midDateRangeSelector.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.midDateRangeSelector_OnSelection);
				this.cbxSet.SelectionChangeCommitted -= new System.EventHandler(this.cboSet_SelectionChangeCommitted);
				this.cboAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboAttribute_SelectionChangeCommitted);
                this.cboAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboAttribute_DragDrop);
                this.cboAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboAttribute_DragEnter);
                this.cboFilter.SelectionChangeCommitted -= new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
                this.cboFilter.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
                this.cboFilter.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
                this.tabMatrix.SelectedIndexChanged -= new System.EventHandler(this.tabMatrix_SelectedIndexChanged);
                //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
                this.cbxSet.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cbxSet_MIDComboBoxPropertiesChangedEvent);
                this.cboAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboAttribute_MIDComboBoxPropertiesChangedEvent);
                this.cboFilter.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboFilter_MIDComboBoxPropertiesChangedEvent);
                //End TT#316
				this.gridSellThru.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.gridSellThru_BeforeRowsDeleted);
				this.gridSellThru.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridSellThru_AfterCellUpdate);
				this.gridSellThru.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridSellThru_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(gridSellThru);
                //End TT#169
				this.gridSellThru.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.gridSellThru_MouseEnterElement);
				this.gridSellThru.BeforeCellDeactivate -= new System.ComponentModel.CancelEventHandler(this.ugGrid_BeforeCellDeactivate);
				this.gridSellThru.BeforeCellUpdate -= new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.gridSellThru_BeforeCellUpdate);
				this.gridSellThru.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.gridSellThru_AfterRowInsert);
				this.gridGrades.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.gridGrades_BeforeRowsDeleted);
				this.gridGrades.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridGrades_AfterCellUpdate);
				this.gridGrades.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridGrades_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(gridGrades);
                //End TT#169
				this.gridGrades.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.gridGrades_MouseEnterElement);
				this.gridGrades.BeforeCellDeactivate -= new System.ComponentModel.CancelEventHandler(this.ugGrid_BeforeCellDeactivate);
				this.gridGrades.BeforeCellUpdate -= new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.gridGrades_BeforeCellUpdate);
				this.gridGrades.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.gridGrades_AfterRowInsert);
				this.gridMatrix.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridMatrix_AfterCellUpdate);
				this.gridMatrix.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridMatrix_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(gridMatrix);
                //End TT#169
				this.gridMatrix.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.gridMatrix_MouseEnterElement);
				this.gridMatrix.AfterCellListCloseUp -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridMatrix_AfterCellListCloseUp);
				// Begin MID Track 4858 - JSmith - Security changes
//				this.btnProcess.Click -= new System.EventHandler(this.btnProcess_Click);
//				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
//				this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
				// End MID Track 4858
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.tabModifySales = new System.Windows.Forms.TabControl();
            this.tabPageMethod = new System.Windows.Forms.TabPage();
            this.cboFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.gbAverage = new System.Windows.Forms.GroupBox();
            this.rbSet = new System.Windows.Forms.RadioButton();
            this.rbAllStores = new System.Windows.Forms.RadioButton();
            this.txtMerchandise = new System.Windows.Forms.TextBox();
            this.midDateRangeSelector = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.lblFilter = new System.Windows.Forms.Label();
            this.lblDateRange = new System.Windows.Forms.Label();
            this.lblMerchandise = new System.Windows.Forms.Label();
            this.tabMatrix = new System.Windows.Forms.TabControl();
            this.tabPageGrades = new System.Windows.Forms.TabPage();
            this.gridSellThru = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.gridGrades = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tabPageMatrix = new System.Windows.Forms.TabPage();
            this.cbxSet = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.gridMatrix = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.cboAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.lblAttribute = new System.Windows.Forms.Label();
            this.lblSet = new System.Windows.Forms.Label();
            this.tabPageProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.menuGrades = new System.Windows.Forms.ContextMenu();
            this.menuSellThru = new System.Windows.Forms.ContextMenu();
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.tabModifySales.SuspendLayout();
            this.tabPageMethod.SuspendLayout();
            this.gbAverage.SuspendLayout();
            this.tabMatrix.SuspendLayout();
            this.tabPageGrades.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSellThru)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridGrades)).BeginInit();
            this.tabPageMatrix.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMatrix)).BeginInit();
            this.tabPageProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(641, 547);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(554, 547);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(11, 547);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // tabModifySales
            // 
            this.tabModifySales.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabModifySales.Controls.Add(this.tabPageMethod);
            this.tabModifySales.Controls.Add(this.tabPageProperties);
            this.tabModifySales.Location = new System.Drawing.Point(8, 54);
            this.tabModifySales.Name = "tabModifySales";
            this.tabModifySales.SelectedIndex = 0;
            this.tabModifySales.Size = new System.Drawing.Size(710, 484);
            this.tabModifySales.TabIndex = 24;
            // 
            // tabPageMethod
            // 
            this.tabPageMethod.Controls.Add(this.cboFilter);
            this.tabPageMethod.Controls.Add(this.gbAverage);
            this.tabPageMethod.Controls.Add(this.txtMerchandise);
            this.tabPageMethod.Controls.Add(this.midDateRangeSelector);
            this.tabPageMethod.Controls.Add(this.lblFilter);
            this.tabPageMethod.Controls.Add(this.lblDateRange);
            this.tabPageMethod.Controls.Add(this.lblMerchandise);
            this.tabPageMethod.Controls.Add(this.tabMatrix);
            this.tabPageMethod.Location = new System.Drawing.Point(4, 22);
            this.tabPageMethod.Name = "tabPageMethod";
            this.tabPageMethod.Size = new System.Drawing.Size(702, 458);
            this.tabPageMethod.TabIndex = 0;
            this.tabPageMethod.Text = "Method";
            // 
            // cboFilter
            // 
            this.cboFilter.AllowDrop = true;
            this.cboFilter.AutoAdjust = true;
            this.cboFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboFilter.AutoScroll = true;
            this.cboFilter.DataSource = null;
            this.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilter.DropDownWidth = 202;
            this.cboFilter.Location = new System.Drawing.Point(357, 12);
            this.cboFilter.Margin = new System.Windows.Forms.Padding(0);
            this.cboFilter.Name = "cboFilter";
            this.cboFilter.Size = new System.Drawing.Size(202, 21);
            this.cboFilter.TabIndex = 7;
            this.cboFilter.Tag = null;
            this.cboFilter.SelectionChangeCommitted += new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
            this.cboFilter.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboFilter_MIDComboBoxPropertiesChangedEvent);
            this.cboFilter.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
            this.cboFilter.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
            this.cboFilter.DragOver += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
            // 
            // gbAverage
            // 
            this.gbAverage.Controls.Add(this.rbSet);
            this.gbAverage.Controls.Add(this.rbAllStores);
            this.gbAverage.Location = new System.Drawing.Point(358, 39);
            this.gbAverage.Name = "gbAverage";
            this.gbAverage.Size = new System.Drawing.Size(201, 45);
            this.gbAverage.TabIndex = 13;
            this.gbAverage.TabStop = false;
            this.gbAverage.Text = "<average>";
            // 
            // rbSet
            // 
            this.rbSet.Location = new System.Drawing.Point(124, 16);
            this.rbSet.Name = "rbSet";
            this.rbSet.Size = new System.Drawing.Size(54, 19);
            this.rbSet.TabIndex = 12;
            this.rbSet.Text = "<set>";
            // 
            // rbAllStores
            // 
            this.rbAllStores.Location = new System.Drawing.Point(26, 16);
            this.rbAllStores.Name = "rbAllStores";
            this.rbAllStores.Size = new System.Drawing.Size(86, 19);
            this.rbAllStores.TabIndex = 11;
            this.rbAllStores.Text = "<all stores>";
            // 
            // txtMerchandise
            // 
            this.txtMerchandise.AllowDrop = true;
            this.txtMerchandise.Location = new System.Drawing.Point(93, 12);
            this.txtMerchandise.Name = "txtMerchandise";
            this.txtMerchandise.Size = new System.Drawing.Size(180, 20);
            this.txtMerchandise.TabIndex = 6;
            this.txtMerchandise.TextChanged += new System.EventHandler(this.txtMerchandise_TextChanged);
            this.txtMerchandise.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragDrop);
            this.txtMerchandise.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragEnter);
            this.txtMerchandise.DragOver += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragOver);
            this.txtMerchandise.DragLeave += new System.EventHandler(this.txtMerchandise_DragLeave);
            this.txtMerchandise.Validating += new System.ComponentModel.CancelEventHandler(this.txtMerchandise_Validating);
            this.txtMerchandise.Validated += new System.EventHandler(this.txtMerchandise_Validated);
            // 
            // midDateRangeSelector
            // 
            this.midDateRangeSelector.DateRangeForm = null;
            this.midDateRangeSelector.DateRangeRID = 0;
            this.midDateRangeSelector.Enabled = false;
            this.midDateRangeSelector.Location = new System.Drawing.Point(94, 41);
            this.midDateRangeSelector.Name = "midDateRangeSelector";
            this.midDateRangeSelector.Size = new System.Drawing.Size(181, 21);
            this.midDateRangeSelector.TabIndex = 10;
            this.midDateRangeSelector.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.midDateRangeSelector_OnSelection);
            this.midDateRangeSelector.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.midDateRangeSelector_ClickCellButton);
            this.midDateRangeSelector.Load += new System.EventHandler(this.midDateRangeSelector_Load);
            // 
            // lblFilter
            // 
            this.lblFilter.Location = new System.Drawing.Point(318, 18);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(36, 17);
            this.lblFilter.TabIndex = 3;
            this.lblFilter.Text = "Filter:";
            // 
            // lblDateRange
            // 
            this.lblDateRange.Location = new System.Drawing.Point(15, 45);
            this.lblDateRange.Name = "lblDateRange";
            this.lblDateRange.Size = new System.Drawing.Size(74, 17);
            this.lblDateRange.TabIndex = 2;
            this.lblDateRange.Text = "Date Range:";
            // 
            // lblMerchandise
            // 
            this.lblMerchandise.Location = new System.Drawing.Point(15, 18);
            this.lblMerchandise.Name = "lblMerchandise";
            this.lblMerchandise.Size = new System.Drawing.Size(74, 17);
            this.lblMerchandise.TabIndex = 1;
            this.lblMerchandise.Text = "Merchandise:";
            // 
            // tabMatrix
            // 
            this.tabMatrix.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMatrix.Controls.Add(this.tabPageGrades);
            this.tabMatrix.Controls.Add(this.tabPageMatrix);
            this.tabMatrix.Location = new System.Drawing.Point(8, 90);
            this.tabMatrix.Name = "tabMatrix";
            this.tabMatrix.SelectedIndex = 0;
            this.tabMatrix.Size = new System.Drawing.Size(687, 362);
            this.tabMatrix.TabIndex = 0;
            this.tabMatrix.SelectedIndexChanged += new System.EventHandler(this.tabMatrix_SelectedIndexChanged);
            // 
            // tabPageGrades
            // 
            this.tabPageGrades.Controls.Add(this.gridSellThru);
            this.tabPageGrades.Controls.Add(this.gridGrades);
            this.tabPageGrades.Location = new System.Drawing.Point(4, 22);
            this.tabPageGrades.Name = "tabPageGrades";
            this.tabPageGrades.Size = new System.Drawing.Size(679, 336);
            this.tabPageGrades.TabIndex = 0;
            this.tabPageGrades.Text = "Grades";
            // 
            // gridSellThru
            // 
            this.gridSellThru.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gridSellThru.Location = new System.Drawing.Point(370, 7);
            this.gridSellThru.Name = "gridSellThru";
            this.gridSellThru.Size = new System.Drawing.Size(163, 309);
            this.gridSellThru.TabIndex = 1;
            this.gridSellThru.Text = "Sell Thru Percents";
            this.gridSellThru.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridSellThru_AfterCellUpdate);
            this.gridSellThru.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridSellThru_InitializeLayout);
            this.gridSellThru.AfterRowsDeleted += new System.EventHandler(this.gridSellThru_AfterRowsDeleted);
            this.gridSellThru.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.gridSellThru_AfterRowInsert);
            this.gridSellThru.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.ugGrid_BeforeCellDeactivate);
            this.gridSellThru.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.gridSellThru_BeforeCellUpdate);
            this.gridSellThru.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.gridSellThru_BeforeRowsDeleted);
            this.gridSellThru.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.gridSellThru_MouseEnterElement);
            // 
            // gridGrades
            // 
            this.gridGrades.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gridGrades.Location = new System.Drawing.Point(73, 7);
            this.gridGrades.Name = "gridGrades";
            this.gridGrades.Size = new System.Drawing.Size(267, 309);
            this.gridGrades.TabIndex = 0;
            this.gridGrades.Text = "Grades";
            this.gridGrades.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridGrades_AfterCellUpdate);
            this.gridGrades.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridGrades_InitializeLayout);
            this.gridGrades.AfterRowsDeleted += new System.EventHandler(this.gridGrades_AfterRowsDeleted);
            this.gridGrades.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.gridGrades_AfterRowInsert);
            this.gridGrades.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.ugGrid_BeforeCellDeactivate);
            this.gridGrades.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.gridGrades_BeforeCellUpdate);
            this.gridGrades.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.gridGrades_BeforeRowsDeleted);
            this.gridGrades.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.gridGrades_MouseEnterElement);
            // 
            // tabPageMatrix
            // 
            this.tabPageMatrix.Controls.Add(this.cbxSet);
            this.tabPageMatrix.Controls.Add(this.gridMatrix);
            this.tabPageMatrix.Controls.Add(this.cboAttribute);
            this.tabPageMatrix.Controls.Add(this.lblAttribute);
            this.tabPageMatrix.Controls.Add(this.lblSet);
            this.tabPageMatrix.Location = new System.Drawing.Point(4, 22);
            this.tabPageMatrix.Name = "tabPageMatrix";
            this.tabPageMatrix.Size = new System.Drawing.Size(679, 336);
            this.tabPageMatrix.TabIndex = 1;
            this.tabPageMatrix.Text = "Sales Matrix";
            // 
            // cbxSet
            // 
            this.cbxSet.AutoAdjust = true;
            this.cbxSet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbxSet.AutoScroll = true;
            this.cbxSet.DataSource = null;
            this.cbxSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSet.DropDownWidth = 202;
            this.cbxSet.Location = new System.Drawing.Point(363, 9);
            this.cbxSet.Margin = new System.Windows.Forms.Padding(0);
            this.cbxSet.Name = "cbxSet";
            this.cbxSet.Size = new System.Drawing.Size(202, 21);
            this.cbxSet.TabIndex = 9;
            this.cbxSet.Tag = null;
            this.cbxSet.SelectionChangeCommitted += new System.EventHandler(this.cboSet_SelectionChangeCommitted);
            this.cbxSet.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cbxSet_MIDComboBoxPropertiesChangedEvent);
            // 
            // gridMatrix
            // 
            this.gridMatrix.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMatrix.Location = new System.Drawing.Point(5, 39);
            this.gridMatrix.Name = "gridMatrix";
            this.gridMatrix.Size = new System.Drawing.Size(669, 282);
            this.gridMatrix.TabIndex = 0;
            this.gridMatrix.Text = "Sales Matrix";
            this.gridMatrix.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridMatrix_AfterCellUpdate);
            this.gridMatrix.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridMatrix_InitializeLayout);
            this.gridMatrix.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridMatrix_AfterCellListCloseUp);
            this.gridMatrix.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.gridMatrix_MouseEnterElement);
            // 
            // cboAttribute
            // 
            this.cboAttribute.AllowDrop = true;
            this.cboAttribute.AllowUserAttributes = false;
            this.cboAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAttribute.Location = new System.Drawing.Point(79, 9);
            this.cboAttribute.Name = "cboAttribute";
            this.cboAttribute.Size = new System.Drawing.Size(202, 21);
            this.cboAttribute.TabIndex = 8;
            this.cboAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboAttribute_SelectionChangeCommitted);
            this.cboAttribute.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cboAttribute.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboAttribute_DragDrop);
            this.cboAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboAttribute_DragEnter);
            this.cboAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cboAttribute_DragOver);
            // 
            // lblAttribute
            // 
            this.lblAttribute.Location = new System.Drawing.Point(15, 13);
            this.lblAttribute.Name = "lblAttribute";
            this.lblAttribute.Size = new System.Drawing.Size(51, 17);
            this.lblAttribute.TabIndex = 4;
            this.lblAttribute.Text = "Attribute:";
            // 
            // lblSet
            // 
            this.lblSet.Location = new System.Drawing.Point(320, 12);
            this.lblSet.Name = "lblSet";
            this.lblSet.Size = new System.Drawing.Size(36, 17);
            this.lblSet.TabIndex = 5;
            this.lblSet.Text = "Set:";
            // 
            // tabPageProperties
            // 
            this.tabPageProperties.Controls.Add(this.ugWorkflows);
            this.tabPageProperties.Location = new System.Drawing.Point(4, 22);
            this.tabPageProperties.Name = "tabPageProperties";
            this.tabPageProperties.Size = new System.Drawing.Size(702, 458);
            this.tabPageProperties.TabIndex = 1;
            this.tabPageProperties.Text = "Properties";
            // 
            // ugWorkflows
            // 
            this.ugWorkflows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugWorkflows.DisplayLayout.Appearance = appearance1;
            this.ugWorkflows.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ugWorkflows.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugWorkflows.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugWorkflows.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ugWorkflows.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ugWorkflows.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugWorkflows.Location = new System.Drawing.Point(43, 13);
            this.ugWorkflows.Name = "ugWorkflows";
            this.ugWorkflows.Size = new System.Drawing.Size(616, 430);
            this.ugWorkflows.TabIndex = 2;
            // 
            // frmModifySalesMethod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(726, 578);
            this.Controls.Add(this.tabModifySales);
            this.Name = "frmModifySalesMethod";
            this.Text = "Modify Sales";
            this.Load += new System.EventHandler(this.frmModifySalesMethod_Load);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.Controls.SetChildIndex(this.tabModifySales, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.pnlGlobalUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.tabModifySales.ResumeLayout(false);
            this.tabPageMethod.ResumeLayout(false);
            this.tabPageMethod.PerformLayout();
            this.gbAverage.ResumeLayout(false);
            this.tabMatrix.ResumeLayout(false);
            this.tabPageGrades.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSellThru)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridGrades)).EndInit();
            this.tabPageMatrix.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMatrix)).EndInit();
            this.tabPageProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// Create a new Forecast Spread Method
		/// </summary>
		override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
		{
			try
			{
				_OTSForecastModifySales = new OTSForecastModifySales(SAB, Include.NoRID);
				ABM = _OTSForecastModifySales;
				base.NewWorkflowMethod(aParentNode, eSecurityFunctions.ForecastMethodsUserOTSModifySales, eSecurityFunctions.ForecastMethodsGlobalOTSModifySales);				

				Common_Load();
				
			}
			catch(Exception ex)
			{
				HandleException(ex, "NewOTSForecastModifySales");
				FormLoadError = true;
			}
		}

		// BEGIN Issue 4323 - stodd 2.21.07 - fix process from workflow explorer
		/// <summary>
		/// Processes a method.
		/// </summary>
		/// <param name="aWorkflowRID">The record ID of the method</param>
		override public void ProcessWorkflowMethod(int aMethodRID)
		{
			try
			{       
				_OTSForecastModifySales = new OTSForecastModifySales(SAB, aMethodRID);
				ProcessAction(eMethodType.ForecastModifySales, true);
			}
			catch (Exception ex)
			{
				HandleException(ex);
				FormLoadError = true;
			}
		}
		// End Issue 4323 - stodd 2.21.07 - fix process from workflow explorer


		/// <summary>
		/// Opens an existing Matrix Method. //Eventually combine with NewOTSForecastCopyMethod method
		/// 		/// Seperate for debugging & initial development
		/// </summary>
		/// <param name="aMethodRID">method_RID</param>
		/// <param name="aLockStatus">The lock status of the data to be displayed</param>
		override public void UpdateWorkflowMethod(int aMethodRID, int aNodeRID, MIDWorkflowMethodTreeNode aNode, eLockStatus aLockStatus)
		{
			try
			{
				_OTSForecastModifySales = new OTSForecastModifySales(SAB, aMethodRID);
				base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.ForecastMethodsUserOTSModifySales, eSecurityFunctions.ForecastMethodsGlobalOTSModifySales);

				Common_Load();

			}
			catch(Exception ex)
			{
				HandleException(ex, "UpdateMethodModifySales");
				FormLoadError = true;
			}
		}

		/// <summary>
		/// Deletes a Forecast Modify Sales Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		override public bool DeleteWorkflowMethod(int aMethodRID)
		{
			try
			{       
				_OTSForecastModifySales = new OTSForecastModifySales(SAB, aMethodRID);
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
		/// Renames a Forecast Modify Sales Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <param name="aNewName">The new name of the workflow or method</param>
		override public bool RenameWorkflowMethod(int aMethodRID, string aNewName)
		{
			try
			{       
				_OTSForecastModifySales = new OTSForecastModifySales(SAB, aMethodRID);
				return Rename(aNewName);
			}
			catch (Exception err)
			{
				HandleException(err);
				FormLoadError = true;
			}
			return false;
		}

		private void Common_Load()
		{
			try
			{
				_editorInt = new UltraMaskedEdit();
				_editorInt.InputMask = "99999999";
				_editorDec = new UltraMaskedEdit();
				_editorDec.InputMask = "999999.99";	
				_editorDec.DisplayMode = MaskMode.Raw;
				_editorDec.DataMode = MaskMode.Raw;

				_storeFilterData = new FilterData();

				_versionProfileList = SAB.ClientServerSession.GetUserForecastVersions();

				SetText();

				eMIDTextCode textCode =  eMIDTextCode.frm_ForecastModifySales;
				_dtRuleLabels = MIDText.GetLabels((int) eModifySalesRuleType.None, (int)eModifySalesRuleType.StockToSalesMaximum);

				_thisTitle = MIDText.GetTextOnly((int)textCode);

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

				PopulateStoreAttributes();			
				PopulateFilter();

				_dtRules = _OTSForecastModifySales.MatrixDataTable.Copy();
				LoadMethodData();

				LoadWorkflows();

                //BEGIN TT#110-MD-VStuart - In Use Tool
                tabModifySales.Controls.Remove(tabPageProperties);
                //END TT#110-MD-VStuart - In Use Tool
            }
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void SetText()
		{
			try
			{
				this.lblName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Method) + ":";
				this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
				this.btnClose.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
				this.btnProcess.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Process);
				this.lblMerchandise.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise) + ":";
				this.lblDateRange.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTimePeriod) + ":";	
				this.lblAttribute.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Attribute) + ":";	
				this.lblSet.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Set) + ":";	
				this.lblFilter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter) + ":";
				this.tabPageMethod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Method);
				this.tabPageProperties.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Properties);
				this.tabPageGrades.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Grade);
				this.tabPageMatrix.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SalesMatrix);
				this.gbAverage.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Average);
				this.rbAllStores.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AllStores);
				this.rbSet.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Set);
                this.gridSellThru.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Sell_Thru_Pct);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void PopulateFilter()
		{
			DataTable dtFilter;

			try
			{
				cboFilter.Items.Clear();
                // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
                //cboFilter.Items.Add(new FilterNameCombo(Include.UndefinedStoreFilter, Include.GlobalUserRID, "(none)"));
                cboFilter.Items.Add(GetRemoveFilterRow());
                // End TT#2669 - JSmith - Unable to remove a filter from a wokflow

                dtFilter = _storeFilterData.FilterRead(filterTypes.StoreFilter, eProfileType.FilterStore, _userRIDList); //TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions

				foreach (DataRow row in dtFilter.Rows)
				{
					cboFilter.Items.Add(
						new FilterNameCombo(Convert.ToInt32(row["FILTER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToString(row["FILTER_NAME"], CultureInfo.CurrentUICulture)));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void LoadMethodData()
		{
			try
			{
				midDateRangeSelector.DateRangeRID = Include.UndefinedCalendarDateRange;

                //Begin Track #5858 - KJohnson - Validating store security only
                txtMerchandise.Tag = new MIDMerchandiseTextBoxTag(SAB, txtMerchandise, eMIDControlCode.form_ForecastModifySales, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
                // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                //cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
                //cboAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboAttribute);
                cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter.ComboBox, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, FunctionSecurity, _OTSForecastModifySales.GlobalUserType == eGlobalUserType.User);
                cboAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboAttribute, FunctionSecurity, _OTSForecastModifySales.GlobalUserType == eGlobalUserType.User);
                // End TT#44
                //End Track #5858
				if (_OTSForecastModifySales.Method_Change_Type != eChangeType.add)
				{
														
					this.txtName.Text = _OTSForecastModifySales.Name;
					this.txtDesc.Text = _OTSForecastModifySales.Method_Description;
								
					if (_OTSForecastModifySales.User_RID == Include.GetGlobalUserRID())
						radGlobal.Checked = true;
					else
						radUser.Checked = true;
					//LoadWorkflows();
				}

				if (_OTSForecastModifySales.HierNodeRid > 0)
				{
					//Begin Track #5378 - color and size not qualified
//					HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSForecastModifySales.HierNodeRid);
                    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSForecastModifySales.HierNodeRid, true, true);
					//End Track #5378
                    //Begin Track #5858 - KJohnson - Validating store security only
                    txtMerchandise.Text = hnp.Text;
                    ((MIDTag)(txtMerchandise.Tag)).MIDTagData = hnp;
                    //End Track #5858
				}

				if (_OTSForecastModifySales.DateRangeRid > 0 && _OTSForecastModifySales.DateRangeRid != Include.UndefinedCalendarDateRange)
				{
					DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(_OTSForecastModifySales.DateRangeRid);
					midDateRangeSelector.Text = drp.DisplayDate;
					midDateRangeSelector.DateRangeRID = drp.Key;
				}

				cboFilter.SelectedIndex = cboFilter.Items.IndexOf(new FilterNameCombo(_OTSForecastModifySales.Filter, -1, ""));
				this.cboAttribute.SelectedValue = _OTSForecastModifySales.SG_RID;
                // Begin Track #4872 - JSmith - Global/User Attributes
                if (cboAttribute.ContinueReadOnly)
                {
                    SetMethodReadOnly();
                    _currAttribute = (int)cboAttribute.SelectedValue;
                }
                //_currAttribute = (int)cboAttribute.SelectedValue;
                // End Track #4872

				_currSet = (int)cbxSet.SelectedValue;

				if (_OTSForecastModifySales.AverageBy == eStoreAverageBy.Set)
					rbSet.Checked = true;
				else
					rbAllStores.Checked = true;

				gridGrades.DataSource = this._OTSForecastModifySales.GradesDataTable;
				gridSellThru.DataSource = _OTSForecastModifySales.SellThruDataTable;
				BuildMatrix(_currSet);				
			}
			catch(Exception ex)
			{
				HandleException(ex, "LoadMethodData");
			}
		}

		private void PopulateStoreAttributes()
		{
            try
			{
                // Begin Track #4872 - JSmith - Global/User Attributes
                //ProfileList pl = SAB.StoreServerSession.GetStoreGroupListViewList();
                BuildAttributeList();

                //this.cboAttribute.ValueMember = "Key";
                //this.cboAttribute.DisplayMember = "Name";
                //this.cboAttribute.DataSource = pl.ArrayList;
                // End Track #4872

//BEGIN TT#7 - RBeck - Dynamic dropdowns
                AdjustTextWidthComboBox_DropDown(cboAttribute);
//END   TT#7 - RBeck - Dynamic dropdowns

			}
			catch(Exception ex)
			{
				HandleException(ex);
				FormLoadError = true;
			}
		}

		private void cboAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			int idx = 0;
			try
			{ 
				if (FormLoaded)
				{	
					if (_attributeReset)
					{
						_attributeReset = false;
						return;
					}	
					//=======================================================================================
					// The current rules are saved because the warning below needs to know if there is any
					// data in the grid for the current attribute. If the user continues, it will all be removed.
					//=======================================================================================
					this.SetMatrixRules(_currSet);
					idx = this.cboAttribute.SelectedIndex;
					if (!MatrixWarningOK(this.lblAttribute.Text))
					{
						_attributeReset = true;
						cboAttribute.SelectedValue = _currAttribute;
						return;
					}
					else
					{
						this.cboAttribute.SelectedIndex = idx;
						_currAttribute = (int)cboAttribute.SelectedValue;
						PopulateStoreAttributeSet(this.cboAttribute.SelectedValue.ToString()); 
						_currSet = (int)cbxSet.SelectedValue;
						// Build Matrix Grid
						_dtRules.Clear();
						BuildMatrix(_currSet);
						ChangePending = true;
					}
				}
				else
				{
                    // Begin Track #4872 - JSmith - Global/User Attributes
                    //PopulateStoreAttributeSet(this.cboAttribute.SelectedValue.ToString());
                    //_currSet = (int)cbxSet.SelectedValue;
                    if (cboAttribute.SelectedValue != null)
                    {
                        PopulateStoreAttributeSet(this.cboAttribute.SelectedValue.ToString());
                        _currSet = (int)cbxSet.SelectedValue;
                    }
                    // End Track #4872
				}
				
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		
        private void cboAttribute_DragDrop(object sender, DragEventArgs e)
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
				ProfileList pl = new ProfileList(eProfileType.StoreGroupLevelListView);
//				StoreGroupLevelListViewProfile sgllvp = new StoreGroupLevelListViewProfile(Include.NoRID);
//				sgllvp.Name = "(none)";
//				pl.Add(sgllvp);

				int sgRid = Convert.ToInt32(key, CultureInfo.CurrentUICulture);
				if (sgRid != Include.NoRID)
				{
                    pl = StoreMgmt.StoreGroup_GetLevelListViewList(sgRid); //SAB.StoreServerSession.GetStoreGroupLevelListViewList(sgRid);
//					foreach (StoreGroupLevelListViewProfile sgllvp2 in pl2.ArrayList)
//					{
//						pl.Add(sgllvp2);
//					}
				}

				this.cbxSet.ValueMember = "Key";
				this.cbxSet.DisplayMember = "Name";
				this.cbxSet.DataSource = pl.ArrayList;

				if (this.cbxSet.Items.Count > 0)	
				{
					this.cbxSet.SelectedIndex = 0;
					//_prevSetValue = Convert.ToInt32(cboAttributeSet.SelectedValue,CultureInfo.CurrentUICulture);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}	

		private void BuildGradesContextMenu()
		{
			try
			{
				MenuItem mnuItemInsertGrade = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert));
				MenuItem mnuItemDeleteGrade = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Delete));
				menuGrades.MenuItems.Add(mnuItemInsertGrade);
				menuGrades.MenuItems.Add(mnuItemDeleteGrade);
				mnuItemInsertGrade.Click += new System.EventHandler(this.menuGradesItemInsert_Click);
				mnuItemDeleteGrade.Click += new System.EventHandler(this.menuGradesItemDelete_Click);
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void BuildSellThruContextMenu()
		{
			try
			{
				MenuItem mnuItemInsertSell = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert));
				MenuItem mnuItemDeleteSell = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Delete));
				menuSellThru.MenuItems.Add(mnuItemInsertSell);
				menuSellThru.MenuItems.Add(mnuItemDeleteSell);
				mnuItemInsertSell.Click += new System.EventHandler(this.menuSellThruItemInsert_Click);
				mnuItemDeleteSell.Click += new System.EventHandler(this.menuSellThruItemDelete_Click);
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void menuGradesItemInsert_Click(object sender, System.EventArgs e)
		{
			try
			{
				gridGrades.DisplayLayout.Bands[0].AddNew();
				ChangePending = true;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}	

		private void menuGradesItemDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (gridGrades.Selected.Rows.Count > 0)
				{
					gridGrades.DeleteSelectedRows();
					_OTSForecastModifySales.GradesDataTable.AcceptChanges();
					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void menuSellThruItemInsert_Click(object sender, System.EventArgs e)
		{
			try
			{
				gridSellThru.DisplayLayout.Bands[0].AddNew();
				ChangePending = true;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}	

		private void menuSellThruItemDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (gridSellThru.Selected.Rows.Count > 0)
				{
					gridSellThru.DeleteSelectedRows();
					_OTSForecastModifySales.SellThruDataTable.AcceptChanges();
					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		#region Properties Tab - Workflows
		/// <summary>
		/// Fill the workflow grid
		/// </summary>
		private void LoadWorkflows()
		{
			try
			{
                 GetOTSPLANWorkflows(_OTSForecastModifySales.Key, ugWorkflows);
			}
			catch(Exception ex)
			{
				HandleException(ex, "LoadWorkflows");
			}
		}
		#endregion

		private void CheckMerchandiseGrades(int hnKey)
		{
			try
			{				
				if (_OTSForecastModifySales.GradesDataTable.Rows.Count > 0)
				{
					string msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_StoreGradesAlreadyExist);
					msg = msg.Replace("{0}","Merchandise");

					if (MessageBox.Show(msg,
						this.Text, MessageBoxButtons.YesNo,
						MessageBoxIcon.Question,
						MessageBoxDefaultButton.Button2,
						MessageBoxOptions.DefaultDesktopOnly)
						== DialogResult.Yes)
					{
						if (MatrixWarningOK(gridGrades.Rows[0].Cells["BOUNDARY"].Column.Header.Caption))
						{
							LoadMerchandiseGrades(hnKey);
							_rebuildMatrix = true;
						}
					}
				}
				else
				{
					LoadMerchandiseGrades(hnKey);
					_rebuildMatrix = true;
				}
			}
			catch
			{
				throw;
			}
		}

		private void LoadMerchandiseGrades(int hnKey)
		{
			try
			{
				_OTSForecastModifySales.GradesDataTable.Rows.Clear();
				StoreGradeList sgl = this.ApplicationTransaction.GetStoreGradeList(hnKey);
				foreach (StoreGradeProfile aGrade in sgl)
				{
					DataRow newRow = _OTSForecastModifySales.GradesDataTable.NewRow();
					newRow["METHOD_RID"] = _OTSForecastModifySales.Key;
					newRow["BOUNDARY"] = aGrade.Boundary;
					newRow["GRADE_CODE"] = aGrade.StoreGrade;
					_OTSForecastModifySales.GradesDataTable.Rows.Add(newRow);
				}
				_OTSForecastModifySales.GradesDataTable.AcceptChanges();
				gridGrades.DataSource = _OTSForecastModifySales.GradesDataTable;
			}
			catch
			{
				throw;
			}
		}

		private void CheckMerchandiseSellThru(int hnKey)
		{
			try
			{				
				if (_OTSForecastModifySales.SellThruDataTable.Rows.Count > 0)
				{
					string msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SellThruPctsAlreadyExist);

					if (MessageBox.Show(msg,
						this.Text, MessageBoxButtons.YesNo,
						MessageBoxIcon.Question,
						MessageBoxDefaultButton.Button2,
						MessageBoxOptions.DefaultDesktopOnly)
						== DialogResult.Yes)
					{
						if (MatrixWarningOK(gridSellThru.Rows[0].Cells["SELL_THRU"].Column.Header.Caption))
						{
							LoadMerchandiseSellThru(hnKey);
							_rebuildMatrix = true;
						}
					}
				}
				else
				{
					LoadMerchandiseSellThru(hnKey);
					_rebuildMatrix = true;
				}
			}
			catch
			{
				throw;
			}
		}

		private void LoadMerchandiseSellThru(int hnKey)
		{
			try
			{
				_OTSForecastModifySales.SellThruDataTable.Rows.Clear();
                //int seq = 1;
				SellThruPctList stpl = this.ApplicationTransaction.GetSellThruPctList(hnKey);
				foreach (SellThruPctProfile aSellThru in stpl)
				{
					DataRow newRow = _OTSForecastModifySales.SellThruDataTable.NewRow();
					newRow["METHOD_RID"] = _OTSForecastModifySales.Key;
					newRow["SELL_THRU"] = aSellThru.SellThruPct;
					_OTSForecastModifySales.SellThruDataTable.Rows.Add(newRow);
				}
				_OTSForecastModifySales.SellThruDataTable.AcceptChanges();
				gridSellThru.DataSource = _OTSForecastModifySales.SellThruDataTable;
			}
			catch
			{
				throw;
			}
		}

		private void BuildMatrix(int sglRid)
		{
			try
			{
				_OTSForecastModifySales.SellThruDataTable.AcceptChanges();
				_OTSForecastModifySales.GradesDataTable.AcceptChanges();

				int sellThruCnt = this._OTSForecastModifySales.SellThruDataTable.Rows.Count;
				int gradeCnt = this._OTSForecastModifySales.GradesDataTable.Rows.Count;
			
				string grade;
				int index = 0, prevIndex = 0, boundary;
			
				_dtMatrixGrid = MIDEnvironment.CreateDataTable("Matrix");
			
				DataColumn dataColumn;

				//Create Columns and rows for datatable

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.String");
				dataColumn.ColumnName = "GRADE_CODE";
				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Grade);
				dataColumn.ReadOnly = false;
				dataColumn.Unique = true;
				_dtMatrixGrid.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Int32");
				dataColumn.ColumnName = "Boundary";
				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Boundary);
				dataColumn.ReadOnly = false;
				dataColumn.Unique = true;
				_dtMatrixGrid.Columns.Add(dataColumn);

				string colCaption = string.Empty; 
			 
				this._OTSForecastModifySales.SellThruDataTable.DefaultView.Sort = "SELL_THRU DESC";	
		
				for (int i = 0; i < sellThruCnt; i++)
				{
					DataRowView dr = _OTSForecastModifySales.SellThruDataTable.DefaultView[i];
					index = (int)dr["SELL_THRU"];
					if (i == 0)
					{
						colCaption = ">" + Convert.ToString(index,CultureInfo.CurrentUICulture);
					}
					else if (i == _OTSForecastModifySales.SellThruDataTable.Rows.Count - 1)
					{
						colCaption = Convert.ToString(index,CultureInfo.CurrentUICulture) + "-" 
							+ Convert.ToString(prevIndex,CultureInfo.CurrentUICulture); 
					}
					else
					{
						colCaption = Convert.ToString(index + 1 ,CultureInfo.CurrentUICulture) + "-" 
							+ Convert.ToString(prevIndex,CultureInfo.CurrentUICulture);
					}
							
					prevIndex = index;
				
					dataColumn = new DataColumn();
					dataColumn.DataType = System.Type.GetType("System.Int32");
					dataColumn.ColumnName = "Rule" + index.ToString();
					dataColumn.Caption = colCaption;	
					dataColumn.ReadOnly = false;
					dataColumn.Unique = false;
					_dtMatrixGrid.Columns.Add(dataColumn);
				
					dataColumn = new DataColumn();
					dataColumn.DataType = System.Type.GetType("System.Double");
					dataColumn.ColumnName = "Qty" + index.ToString();
					dataColumn.Caption = colCaption;	
					dataColumn.ReadOnly = false;
					dataColumn.Unique = false;
					_dtMatrixGrid.Columns.Add(dataColumn);
				}

				foreach (DataRow row in _OTSForecastModifySales.GradesDataTable.Rows)
				{
					grade = Convert.ToString(row["GRADE_CODE"],CultureInfo.CurrentUICulture);
					boundary = Convert.ToInt32(row["BOUNDARY"], CultureInfo.CurrentUICulture);
					_dtMatrixGrid.Rows.Add(new object[] {grade, boundary});
				}	

				_dtMatrixGrid.AcceptChanges();

				LoadMatrixValues(sglRid);

				_dtMatrixGrid.AcceptChanges();

				gridMatrix.DataSource = _dtMatrixGrid;

				_rebuildMatrix = false;
			}
			catch
			{
				throw;
			}
		}

		private void LoadMatrixValues(int sglRid)
		{
			try
			{
				int idx = 0;
				if (this._dtRules.Rows.Count > 0)
				{
					_dtRules.DefaultView.RowFilter = "SGL_RID = " +  sglRid.ToString();
					_dtRules.DefaultView.Sort = "BOUNDARY DESC, SELL_THRU DESC";
					if (_dtMatrixGrid.DefaultView.Count > 0)
					{
						for (int i = 0; i < _dtMatrixGrid.Rows.Count; i++)
						{
							DataRow row = _dtMatrixGrid.Rows[i];
							int boundary = Convert.ToInt32(row["BOUNDARY"],CultureInfo.CurrentUICulture);
							object [] keys = new object[2];
							keys[0] = boundary.ToString(); 
							for (int j = 2; j < _dtMatrixGrid.Columns.Count; j++)
							{
								int foundRowIdx = -1;
								DataColumn col = _dtMatrixGrid.Columns[j];
													
								if (col.ColumnName.Substring(0,3) == "Qty") 
									idx = Convert.ToInt32(col.ColumnName.Substring(3),CultureInfo.CurrentUICulture);
								else if (col.ColumnName.Substring(0,4) == "Rule") 
									idx = Convert.ToInt32(col.ColumnName.Substring(4),CultureInfo.CurrentUICulture);
							
								keys[1] = idx.ToString();
								foundRowIdx = _dtRules.DefaultView.Find(keys);
								if (foundRowIdx >= 0 )
								{	
									DataRowView dvRow = _dtRules.DefaultView[foundRowIdx];
									if ( col.ColumnName.Substring(0,3) == "Qty")
									{
										if (   dvRow["MATRIX_RULE_QUANTITY"] == System.DBNull.Value 
											|| Convert.ToDouble(dvRow["MATRIX_RULE_QUANTITY"],CultureInfo.CurrentUICulture) == Include.UndefinedDouble)
											row[col] = System.DBNull.Value;
										else
										{
											row[col] = dvRow["MATRIX_RULE_QUANTITY"];
										}
									}	
									else if (col.ColumnName.Substring(0,4) == "Rule") 
									{
										row[col] = dvRow["MATRIX_RULE"];
									}
								}
							}	
						}
					}
				}
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		#region Overrides
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
			//int storeFilterRID;
			try
			{
                //Begin Track #5858 - KJohnson - Validating store security only
                HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)txtMerchandise.Tag).MIDTagData;
                _OTSForecastModifySales.HierNodeRid = hnp.Key;
                //End Track #5858
				_OTSForecastModifySales.DateRangeRid = midDateRangeSelector.DateRangeRID;
				if (cboFilter.SelectedItem == null)
				{
					_OTSForecastModifySales.Filter = Include.NoRID;
				}
				else
				{
					_OTSForecastModifySales.Filter = ((FilterNameCombo)cboFilter.SelectedItem).FilterRID;
				}
				_OTSForecastModifySales.SG_RID = (int)cboAttribute.SelectedValue;
				if (rbSet.Checked)
					_OTSForecastModifySales.AverageBy = eStoreAverageBy.Set;
				else
					_OTSForecastModifySales.AverageBy = eStoreAverageBy.AllStores;

				_dtMatrixGrid.AcceptChanges();
				SetMatrixRules(_currSet);

				_OTSForecastModifySales.MatrixDataTable = _dtRules.Copy();
				_OTSForecastModifySales.GradesDataTable.AcceptChanges();
				_OTSForecastModifySales.SellThruDataTable.AcceptChanges();
			}
			catch
			{
				throw;
			}
		}

		private void SetMatrixRules(int sglRid)
		{
			try
			{
				//=====================================
				// Remove previous values for sglRid
				//=====================================
				DataRow [] rows = _dtRules.Select("SGL_RID = " + sglRid.ToString());
				foreach (DataRow row in rows)
				{
					_dtRules.Rows.Remove(row);
				}

				//================================
				// Add current values for sglRid
				//================================
				foreach (DataRow row in _dtMatrixGrid.Rows)
				{
					int boundary = Convert.ToInt32(row["BOUNDARY"],CultureInfo.CurrentUICulture);
					int sellThruIndex = 0;
					int rule = 0;
					double qty = 0;
				
					for (int j = 2; j < _dtMatrixGrid.Columns.Count; j++)
					{
						DataColumn col = _dtMatrixGrid.Columns[j];
						if (col.ColumnName.Substring(0,4) == "Rule") 
						{
							sellThruIndex = Convert.ToInt32(col.ColumnName.Substring(4),CultureInfo.CurrentUICulture);
							if (row[col] != System.DBNull.Value)
							{
								rule = Convert.ToInt32(row[col],CultureInfo.CurrentUICulture);
								j++;
								col = _dtMatrixGrid.Columns[j];
								if (row[col] != System.DBNull.Value)
									qty = Convert.ToDouble(row[col],CultureInfo.CurrentUICulture);
								else
									qty = Include.UndefinedDouble;
                                //int i = 5;
								_dtRules.Rows.Add(new object[] {_OTSForecastModifySales.Key, sglRid, boundary, sellThruIndex, rule, qty});
							}
						}
					}
				}	
				_dtRules.AcceptChanges();
			}
			catch
			{
				throw;
			}
		}



		/// <summary>
		/// Use to validate the fields that are specific to this method type
		/// </summary>
		override protected bool ValidateSpecificFields()
		{
			bool methodFieldsValid = true;
			try
			{
				if (txtMerchandise.Text.Trim().Length == 0)
				{
					methodFieldsValid = false;
					ErrorProvider.SetError (txtMerchandise,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
				}
				else
				{
					ErrorProvider.SetError (txtMerchandise,string.Empty);
				}

				if (this.midDateRangeSelector.DateRangeRID == Include.UndefinedCalendarDateRange)
				{
					methodFieldsValid = false;
					ErrorProvider.SetError (midDateRangeSelector,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
				}
				else
				{
					ErrorProvider.SetError (midDateRangeSelector,string.Empty);
				}

                // Begin Track #4872 - JSmith - Global/User Attributes
                if (cboAttribute.SelectedIndex == Include.Undefined)
                {
                    methodFieldsValid = false;
                    ErrorProvider.SetError(cboAttribute, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                }
                else
                {
                    ErrorProvider.SetError(cboAttribute, string.Empty);
                }
                // End Track #4872

//				_OTSForecastModifySales.GradesDataTable.AcceptChanges();
//				_OTSForecastModifySales.SellThruDataTable.AcceptChanges();
				EditMsgs em = new EditMsgs();
				if (methodFieldsValid) 
				{
					if (!ValidateGradesTab(ref em))
					{
						methodFieldsValid = false;
						this.tabMatrix.SelectedTab = this.tabPageGrades;
					}
				}

				if (methodFieldsValid) 
				{
					if (!ValidateMatrixTab())
					{
						methodFieldsValid = false;
						this.tabMatrix.SelectedTab = this.tabPageMatrix;
					}
				}

				if (methodFieldsValid) 
				{
					if (_rebuildMatrix)
					{
						this.BuildMatrix(_currSet);
					}
				}

				return methodFieldsValid;
			}
			catch(Exception exception)
			{
				HandleException(exception);
				return methodFieldsValid;
			}
		}

		private bool ValidateUserForModifedVersion()
		{
			ErrorProvider.SetError (btnProcess,string.Empty);
			string errorMessage = string.Empty;

			try
			{
				bool isValid = false;
				foreach (VersionProfile verProf in _versionProfileList)
				{
					if (verProf.Key == Include.FV_ModifiedRID)
					{
						if (verProf.StoreSecurity.AllowUpdate)
						{
							isValid = true;
						}
						break;
					}
				}

				if (!isValid)
				{
					errorMessage = "User does not have store update authority for the Modifed Forecast Version.";
					btnProcess.Tag = errorMessage;
					ErrorProvider.SetError (btnProcess,errorMessage);
				}
				return isValid;
			}
			catch
			{
				throw;
			}
		}

		#region Validate Grades Tab
		private bool ValidateGradesTab(ref EditMsgs em)
		{
			string errorMessage = string.Empty;
			bool errorFound = false;
			ErrorProvider.SetError (gridGrades,string.Empty);
			ErrorProvider.SetError (gridSellThru,string.Empty);
	 
			try
			{
//				if (gridGrades.Rows.Count < 2)
//				{
//					//errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
//					errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_EntriesLessThanMinimum),
//						Include.MinVelocityGrades.ToString(CultureInfo.CurrentUICulture),
//						MIDText.GetTextOnly(eMIDTextCode.lbl_Velocity_Grades));				
//					gridGrades.Tag = errorMessage;
//					ErrorProvider.SetError (gridGrades,errorMessage);
//					errorFound = true;
//				}
//				else
//				{
					ValidateGrades(ref em);
//				}

				if (gridSellThru.Rows.Count == 0)
				{
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					gridSellThru.Tag = errorMessage;
					ErrorProvider.SetError (gridSellThru,errorMessage);
					errorFound = true;
				}
				else
				{
					ValidSellThru(ref em);
				}

				if (em.ErrorFound || errorFound)
				{
					return false;
				}
				else
				{
					this._OTSForecastModifySales.GradesDataTable.AcceptChanges();
					this._OTSForecastModifySales.SellThruDataTable.AcceptChanges();
					return true;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
				return false;
			}
		}

		private bool ValidateGrades(ref EditMsgs em)
		{
			int lastBoundary = 0;
			string errorMessage = null;
			// sort in descending order by boundary
			this.gridGrades.DisplayLayout.Bands[0].SortedColumns.Clear();
			this.gridGrades.DisplayLayout.Bands[0].SortedColumns.Add("Boundary", true);
			UltraGridRow lastRow = null;
			
			foreach(  UltraGridRow gridRow in gridGrades.Rows )
			{
				lastRow = gridRow;
				if (!IsGradeValid(gridRow.Cells["GRADE_CODE"], ref errorMessage))
				{
					em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
				}
				
				if (!IsBoundaryValid(gridRow.Cells["BOUNDARY"], ref errorMessage))
				{
					em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());

				}
				else
				{
					lastBoundary = Convert.ToInt32(gridRow.Cells["BOUNDARY"].Value, CultureInfo.CurrentUICulture);
				}
			}

			if (lastBoundary != 0)
			{
				errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_LastBoundaryNotZero);
				em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
				lastRow.Cells["BOUNDARY"].Appearance.Image = ErrorImage;
				lastRow.Cells["BOUNDARY"].Tag = errorMessage;
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

		private bool IsGradeValid(UltraGridCell gridCell, ref string errorMessage)
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
					// make sure grades are unique
					foreach(  UltraGridRow gridRow in this.gridGrades.Rows )
					{
						//						if (!gridRow.IsActiveRow)
						if (gridCell.Row != gridRow)
						{
							if ((string)gridCell.Text == (string)gridRow.Cells["GRADE_CODE"].Text)
							{
								errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_VelocityGradesNotUnique);
								errorFound = true;
								break;
							}
						}
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

		private bool IsBoundaryValid(UltraGridCell gridCell, ref string errorMessage)
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
						foreach(  UltraGridRow gridRow in this.gridGrades.Rows )
						{
							if (gridCell.Row != gridRow)	// Don't check yourself
							{
								if (Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture) == Convert.ToInt32(gridRow.Cells["BOUNDARY"].Text, CultureInfo.CurrentUICulture))
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
			catch( Exception error)
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

		
		private bool ValidSellThru(ref EditMsgs em)
		{
			try
			{
				int lastPct = 0;
				UltraGridRow lastRow = null;
				string errorMessage = null;
				// sort in descending order by sell thru %
				this.gridSellThru.DisplayLayout.Bands[0].SortedColumns.Clear();
				this.gridSellThru.DisplayLayout.Bands[0].SortedColumns.Add("SELL_THRU", true);
			
				foreach(  UltraGridRow gridRow in gridSellThru.Rows )
				{
					lastRow = gridRow;
					if (!IsSellThruValid(gridRow.Cells["SELL_THRU"], ref errorMessage))
					{
						em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
					}
					else
					{
						lastPct = Convert.ToInt32(gridRow.Cells["SELL_THRU"].Value, CultureInfo.CurrentUICulture);
					}
				}

				if (lastPct != 0)
				{
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_LastSellThruPctNotZero);
					em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
					lastRow.Cells["SELL_THRU"].Appearance.Image = ErrorImage;
					lastRow.Cells["SELL_THRU"].Tag = errorMessage;
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
			catch( Exception ex )
			{
				em.AddMsg(eMIDMessageLevel.Error, ex.Message,  this.ToString());
				HandleException(ex);
			}
			
			return false;
		}
		private bool IsSellThruValid(UltraGridCell gridCell, ref string errorMessage)
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
						foreach(  UltraGridRow gridRow in this.gridSellThru.Rows )
						{
							if (gridCell.Row != gridRow)	// Don't check yourself
							{
								if (gridRow.Cells["SELL_THRU"].Text.Trim().Length > 0)
								{
									if (Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture) == Convert.ToInt32(gridRow.Cells["SELL_THRU"].Text, CultureInfo.CurrentUICulture))
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
			catch( Exception error)
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
		#endregion Validate Grades Tab

		#region Validate Matrix Tab
		private bool ValidateMatrixTab()
		{
			string errorMessage = string.Empty;
			bool errorFound = false;
			ErrorProvider.SetError (gridMatrix,string.Empty);
			try
			{
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
				HandleException (ex);
				return false;
			}		 
		}

		private bool ValidMatrix()
		{
			bool errorFound = false;
			try
			{
				foreach(  UltraGridRow gridRow in gridMatrix.Rows )
				{
					foreach ( UltraGridCell cell in gridRow.Cells )	
					{
						if (cell.Column.Tag != null && cell.Column.Tag.ToString() == "Rule"
							&& cell.Text.TrimEnd() != string.Empty)
						{
							if (!IsMatrixQuantityValid(cell))
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
				HandleException (ex);
				return false;
			}	
		}
		private bool IsMatrixQuantityValid(UltraGridCell gridCell)
		{
			bool errorFound = false;
			string errorMessage = string.Empty;
			double quantity;
			try
			{
				int i = gridCell.Column.Index;
				eModifySalesRuleRequiresQty aRule = (eModifySalesRuleRequiresQty)Convert.ToInt32(gridCell.Value, CultureInfo.CurrentUICulture);
				if (Enum.IsDefined(typeof(eModifySalesRuleRequiresQty), aRule))
				{
					if (gridCell.Row.Cells[i+1].Text.Trim() == string.Empty)
					{
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
						errorFound = true;
					}
					else
					{
						quantity = Convert.ToDouble(gridCell.Row.Cells[i+1].Text, CultureInfo.CurrentUICulture);
						
						if (quantity < 0 )
						{
							errorMessage =  SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositive);
							errorFound = true;
						}
					}
				}
				if (errorFound)
				{
					gridCell.Row.Cells[i+1].Appearance.Image = ErrorImage;
					gridCell.Row.Cells[i+1].Tag = errorMessage;
					return false;
				}
				else
				{
					gridCell.Row.Cells[i+1].Appearance.Image = null;
					gridCell.Row.Cells[i+1].Tag = null;
					return true;
				}
			}
			catch (Exception ex)
			{	
				HandleException (ex);
				return false;
			}	
		}
		#endregion Validate Matrix Tab

		/// <summary>
		/// Use to set the errors to the screen
		/// </summary>
		override protected void HandleErrors()
		{
			if (!WorkflowMethodNameValid)
			{
				ErrorProvider.SetError (txtName,WorkflowMethodNameMessage);
			}
			else
			{
				ErrorProvider.SetError (txtName,"");
			}
			if (!WorkflowMethodDescriptionValid)
			{
				ErrorProvider.SetError (txtDesc,WorkflowMethodDescriptionMessage);
			}
			else
			{
				ErrorProvider.SetError (txtDesc,"");
			}
			if (!UserGlobalValid)
			{
				ErrorProvider.SetError (this.pnlGlobalUser,UserGlobalMessage);
			}
			else
			{
				ErrorProvider.SetError (pnlGlobalUser,"");
			}
		}

		/// <summary>
		/// Use to set the specific method object before updating
		/// </summary>
		override protected void SetObject()
		{
			try
			{
				ABM = _OTSForecastModifySales;
			}
			catch(Exception ex)
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

		#endregion End Overrides

		private void cboSet_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{	
				this.SetMatrixRules(_currSet);
				_currSet = (int)cbxSet.SelectedValue;
				this.BuildMatrix(_currSet);
			}
		}

		private void cboFilter_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{	
				ChangePending = true;
			}

            // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
            if (cboFilter.SelectedIndex != -1)
            {
                if (((FilterNameCombo)cboFilter.SelectedItem).FilterRID == Include.Undefined)
                {
                    cboFilter.SelectedIndex = -1;
                }
            }
            // End TT#2669 - JSmith - Unable to remove a filter from a wokflow
		}
        //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
        void cbxSet_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboSet_SelectionChangeCommitted(source, new EventArgs());
        }
        void cboAttribute_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboAttribute_SelectionChangeCommitted(source, new EventArgs());
        }
        void cboFilter_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboFilter_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316

        private void cboFilter_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
        }

        private void cboFilter_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            //Begin Track #5858 - Kjohnson - Validating store security only
            try
            {
                bool isSuccessfull = ((MIDComboBoxTag)(((ComboBox)sender).Tag)).ComboBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    ChangePending = true;
                    ((MIDComboBoxEnh)((ComboBox)sender).Parent).FirePropertyChangeEvent();
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
            //End Track #5858
        }
		private void gridGrades_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			try
			{
				gridGrades.DisplayLayout.GroupByBox.Hidden = true;

				e.Layout.Bands[0].Columns["METHOD_RID"].Hidden = true;
				gridGrades.DisplayLayout.Bands[0].Columns["GRADE_CODE"].Width = 75;
				gridGrades.DisplayLayout.Bands[0].Columns["BOUNDARY"].Width = 75;
				gridGrades.DisplayLayout.Bands[0].Columns["BOUNDARY"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;

				//Prevent the user from re-arranging columns.
				gridGrades.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;

				if (FunctionSecurity.AllowUpdate)
				{
					gridGrades.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
					gridGrades.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
					BuildGradesContextMenu();
					gridGrades.ContextMenu = menuGrades;

				}
				else
				{
					gridGrades.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
					gridGrades.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
				}

				//Set the header captions.
				gridGrades.DisplayLayout.Bands[0].Columns["GRADE_CODE"].Header.VisiblePosition = 1;
				gridGrades.DisplayLayout.Bands[0].Columns["GRADE_CODE"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Grade);
				gridGrades.DisplayLayout.Bands[0].Columns["BOUNDARY"].Header.VisiblePosition = 2;
				gridGrades.DisplayLayout.Bands[0].Columns["BOUNDARY"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Boundary);
		
				gridGrades.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_Grade);
				gridGrades.DisplayLayout.Bands[0].AddButtonToolTipText = MIDText.GetTextOnly(eMIDTextCode.lbl_Grade);
				gridGrades.DisplayLayout.AddNewBox.Hidden = false;
				gridGrades.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;
			}
			catch(Exception err)
			{
				HandleException(err);
			}
		}

		private void gridSellThru_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			try
			{
				gridSellThru.DisplayLayout.GroupByBox.Hidden = true;

				e.Layout.Bands[0].Columns["METHOD_RID"].Hidden = true;

				gridSellThru.DisplayLayout.Appearance.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
				gridSellThru.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Default;
				gridSellThru.DisplayLayout.Bands[0].Columns["SELL_THRU"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				gridSellThru.DisplayLayout.Bands[0].Columns["SELL_THRU"].Width = 75;
		
				//Prevent the user from re-arranging columns.
				gridSellThru.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;

				if (FunctionSecurity.AllowUpdate)
				{
					gridSellThru.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
					gridSellThru.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
					BuildSellThruContextMenu();
					gridSellThru.ContextMenu = menuSellThru;
				}
				else
				{
					gridSellThru.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
					gridSellThru.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
				}

				//Set the header captions.
				gridSellThru.DisplayLayout.Bands[0].Columns["SELL_THRU"].Header.VisiblePosition = 0;
				gridSellThru.DisplayLayout.Bands[0].Columns["SELL_THRU"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Sell_Thru_Pct);
                gridSellThru.DisplayLayout.Bands[0].Columns["SELL_THRU"].Width = gridSellThru.DisplayLayout.Bands[0].Columns["SELL_THRU"].CalculateAutoResizeWidth(1, true);
                gridSellThru.Width = gridSellThru.DisplayLayout.Bands[0].Columns["SELL_THRU"].Width + 85;
		
				gridSellThru.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_Sell_Thru_Pct);
				gridSellThru.DisplayLayout.Bands[0].AddButtonToolTipText = MIDText.GetTextOnly(eMIDTextCode.lbl_Sell_Thru_Pct);
				gridSellThru.DisplayLayout.AddNewBox.Hidden = false;
				gridSellThru.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;
			}
			catch(Exception err)
			{
				HandleException(err);
			}
		}

		private void gridMatrix_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			this.gridMatrix.DisplayLayout.AddNewBox.Hidden = true;
			this.gridMatrix.DisplayLayout.GroupByBox.Hidden = true;
			this.gridMatrix.DisplayLayout.GroupByBox.Prompt = "";
			this.gridMatrix.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.gridMatrix.DisplayLayout.Bands[0].AddButtonCaption = "";
		
			this.gridMatrix.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;

			this.gridMatrix.DisplayLayout.Bands[0].Columns["GRADE_CODE"].Width = 40;
			this.gridMatrix.DisplayLayout.Bands[0].Columns["GRADE_CODE"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Grade);
		
			this.gridMatrix.DisplayLayout.Bands[0].Columns["Boundary"].Width = 68;
			this.gridMatrix.DisplayLayout.Bands[0].Columns["Boundary"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Boundary);

			if (!FormLoaded)
			{
				//Add a list to the grid, and name it "Rule".
				this.gridMatrix.DisplayLayout.ValueLists.Add("Rule");
			 
				foreach (DataRow row in _dtRuleLabels.Rows)
				{
					Infragistics.Win.ValueListItem vli = new Infragistics.Win.ValueListItem();
					vli.DataValue = row["TEXT_CODE"];
					vli.DisplayText = row["TEXT_VALUE"].ToString();
					this.gridMatrix.DisplayLayout.ValueLists["Rule"].ValueListItems.Add(vli);
					vli.Dispose();
				}
			}
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //FormatColumns(this.gridMatrix);
            //End TT#169
			
			for (int i = 2; i < this.gridMatrix.DisplayLayout.Bands[0].Columns.Count; i++)
			{
				this.gridMatrix.DisplayLayout.Bands[0].Columns[i].SortIndicator = SortIndicator.Disabled;

				if (SqlInt32.Mod(i,2) == 0)
				{
					this.gridMatrix.DisplayLayout.Bands[0].Columns[i].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Rule);
					this.gridMatrix.DisplayLayout.Bands[0].Columns[i].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					this.gridMatrix.DisplayLayout.Bands[0].Columns[i].ValueList = gridMatrix.DisplayLayout.ValueLists["Rule"];
					this.gridMatrix.DisplayLayout.Bands[0].Columns[i].Tag  = "Rule";
					this.gridMatrix.DisplayLayout.Bands[0].Columns[i].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
				}
				else
				{
					this.gridMatrix.DisplayLayout.Bands[0].Columns[i].Header.Caption =  MIDText.GetTextOnly(eMIDTextCode.lbl_Qty);
					this.gridMatrix.DisplayLayout.Bands[0].Columns[i].Tag  = "Qty";
					this.gridMatrix.DisplayLayout.Bands[0].Columns[i].Width = 55;
					this.gridMatrix.DisplayLayout.Bands[0].Columns[i].MaxLength = 9;
				}
			}	

			bool activateQty ; 
			eModifySalesRuleRequiresQty aRule = eModifySalesRuleRequiresQty.SalesModifier;
			foreach (UltraGridRow row in gridMatrix.Rows)
			{
				row.Cells["GRADE_CODE"].Activation = Activation.NoEdit;
				row.Cells["Boundary"].Activation = Activation.NoEdit;
				for (int j = 2; j < row.Band.Columns.Count; j++)
				{
					activateQty = false; 
					if (row.Cells[j].Column.Tag.ToString() == "Rule")
					{
						if (row.Cells[j].Value != System.DBNull.Value)
						{
							aRule = (eModifySalesRuleRequiresQty)(Convert.ToInt32(row.Cells[j].Value, CultureInfo.CurrentUICulture));
							if (Enum.IsDefined(typeof(eModifySalesRuleRequiresQty), aRule))
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
						{
							row.Cells[j].Activation = Activation.AllowEdit;
							SetEditor(aRule, row.Cells[j]);
						}
						else
							row.Cells[j].Activation = Activation.Disabled;
					}
				}
			}
			// sort in descending order by boundary
			this.gridMatrix.DisplayLayout.Bands[0].SortedColumns.Clear();
			this.gridMatrix.DisplayLayout.Bands[0].SortedColumns.Add("Boundary", true);
			
			UltraGridBand band = this.gridMatrix.DisplayLayout.Bands[0];
			band.Groups.Clear();
			if (band.Groups.Count == 0)
			{
				band.LevelCount = 1;
				band.Override.AllowGroupMoving = AllowGroupMoving.NotAllowed;

				UltraGridGroup  group0 = new UltraGridGroup();
				group0 = band.Groups.Add("Group1", "   ");
				group0.Columns.Add(band.Columns["GRADE_CODE"], 0, 0);
				group0.Columns.Add(band.Columns["Boundary"], 1, 0);
				
		
				for (int i = 2; i < band.Columns.Count; i+=2)
				{
					UltraGridGroup group  = new UltraGridGroup();
					group  = band.Groups.Add("Group" + i.ToString(), _dtMatrixGrid.Columns[i].Caption);
					group.Columns.Add(band.Columns[i], 0, 0);
					group.Columns.Add(band.Columns[i+1], 1, 0); 
					group.CellAppearance.BorderColor = System.Drawing.Color.Black;

				}
			}

//			if (gridMatrix.DisplayLayout.MaxColScrollRegions != 2)
//			{
//				this.gridMatrix.DisplayLayout.MaxColScrollRegions = 2;
//				int colScrollWidth = this.gridMatrix.DisplayLayout.Bands[0].Columns["GRADE_CODE"].Width;
//				colScrollWidth += this.gridMatrix.DisplayLayout.Bands[0].Columns["Boundary"].Width;
//				colScrollWidth += gridMatrix.DisplayLayout.Bands[0].RowSelectorWidthResolved;  // Add in row selector
//				this.gridMatrix.DisplayLayout.ColScrollRegions[0].Width = colScrollWidth;
//				this.gridMatrix.DisplayLayout.ColScrollRegions[0].Split (this.gridMatrix.DisplayLayout.ColScrollRegions[0].Width);
//				this.gridMatrix.DisplayLayout.Bands[0].Columns["GRADE_CODE"].Header.ExclusiveColScrollRegion = this.gridMatrix.DisplayLayout.ColScrollRegions[0];
//				this.gridMatrix.DisplayLayout.Bands[0].Columns["Boundary"].Header.ExclusiveColScrollRegion = this.gridMatrix.DisplayLayout.ColScrollRegions[0];
//			}

			//===========================
			// Don't allow row deletion
			//===========================
			foreach(UltraGridBand ugb in this.gridMatrix.DisplayLayout.Bands)
			{
				ugb.Override.AllowDelete = DefaultableBoolean.False;
			}
			this.gridMatrix.ContextMenu = null;

			// BEGIN Issue 5136 stodd 1.25.2008 
			this.gridMatrix.DisplayLayout.Bands[0].Columns["Boundary"].Format = "####0";
			// END Issue 5136  
		}

		private void gridMatrix_AfterCellListCloseUp(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{	
				int i = e.Cell.Column.Index;
				int sel = e.Cell.Column.ValueList.SelectedItemIndex;

				if (sel == -1) 
				{
					e.Cell.Row.Cells[i+1].Value = System.DBNull.Value;
					e.Cell.Row.Cells[i+1].Activation = Activation.Disabled;
					return;
				}
 
				DataRow dr = _dtRuleLabels.Rows[sel];
				eModifySalesRuleRequiresQty aRule = (eModifySalesRuleRequiresQty)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
				if ((eModifySalesRuleType)aRule == eModifySalesRuleType.None)
				{
					e.Cell.Column.ValueList.SelectedItemIndex = -1;
					e.Cell.Row.Cells[i+1].Appearance.Image = null;
					e.Cell.Row.Cells[i+1].Value = System.DBNull.Value;
					e.Cell.Row.Cells[i+1].Activation = Activation.Disabled;
					return;
				}

				if (Enum.IsDefined(typeof(eModifySalesRuleRequiresQty), aRule))
				{
					e.Cell.Row.Cells[i+1].Activation = Activation.AllowEdit;
					SetEditor(aRule,e.Cell.Row.Cells[i+1]);
                    // Begin MID Track 6322 - KJohnson - Qty removed when selecting rule
                    if (e.Cell.DataChanged)
                    {
                        e.Cell.Row.Cells[i + 1].Value = System.DBNull.Value;
                        _dtMatrixGrid.AcceptChanges();
                    }
                    // End MID Track 6322

					if (e.Cell.Row.Cells[i+1].Value != System.DBNull.Value)
						FormatQuantityCell(aRule,e.Cell.Row.Cells[i+1]);
				}
				else
				{
					e.Cell.Row.Cells[i+1].Appearance.Image = null;
					e.Cell.Row.Cells[i+1].Tag = null;
					e.Cell.Row.Cells[i+1].Value = System.DBNull.Value;
					e.Cell.Row.Cells[i+1].Activation = Activation.Disabled;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void FormatQuantityCell(eModifySalesRuleRequiresQty aRule,UltraGridCell aCell)
		{
			double dblValue = 0;
			try
			{	
				switch (aRule)
				{
					case eModifySalesRuleRequiresQty.PlugSales:
						dblValue = Math.Round(Convert.ToDouble(aCell.Value,CultureInfo.CurrentUICulture),0);
						break;
					case eModifySalesRuleRequiresQty.SalesIndex:
						dblValue = Math.Round(Convert.ToDouble(aCell.Value,CultureInfo.CurrentUICulture),2);
						break;
					case eModifySalesRuleRequiresQty.SalesModifier:
						dblValue = Math.Round(Convert.ToDouble(aCell.Value,CultureInfo.CurrentUICulture),2);
						break;
					case eModifySalesRuleRequiresQty.StockToSalesIndex:
						dblValue = Math.Round(Convert.ToDouble(aCell.Value,CultureInfo.CurrentUICulture),2);
						break;
                    // Begin MID Track 6322 - KJohnson - Qty removed when selecting rule
                    case eModifySalesRuleRequiresQty.StockToSalesMaximum:
                        dblValue = Math.Round(Convert.ToDouble(aCell.Value, CultureInfo.CurrentUICulture), 2);
                        break;
                    case eModifySalesRuleRequiresQty.StockToSalesMinmum:
                        dblValue = Math.Round(Convert.ToDouble(aCell.Value, CultureInfo.CurrentUICulture), 2);
                        break;
                    // End MID Track 6322
                    case eModifySalesRuleRequiresQty.StockToSalesRatio:
						aCell.Column.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
						dblValue = Math.Round(Convert.ToDouble(aCell.Value,CultureInfo.CurrentUICulture),2);
						break;
				}
				aCell.Value = dblValue; 
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void SetEditor(eModifySalesRuleRequiresQty aRule,UltraGridCell aCell)
		{
			try
			{	
				switch (aRule)
				{
					case eModifySalesRuleRequiresQty.PlugSales:
						aCell.EditorControl = _editorInt;
						//aCell.EditorControl = null;
						break;
					case eModifySalesRuleRequiresQty.SalesIndex:
					case eModifySalesRuleRequiresQty.SalesModifier:
                    case eModifySalesRuleRequiresQty.StockToSalesIndex:
                    // Begin MID Track 6322 - KJohnson - Qty removed when selecting rule
                    case eModifySalesRuleRequiresQty.StockToSalesMaximum:
                    case eModifySalesRuleRequiresQty.StockToSalesMinmum:
                    // End MID Track 6322
                    case eModifySalesRuleRequiresQty.StockToSalesRatio:
						//aCell.EditorControl = _editorDec;
						aCell.EditorControl = null;
						break;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void gridMatrix_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		private void ugGrid_BeforeCellDeactivate(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Infragistics.Win.UltraWinGrid.UltraGrid  ugGrid;
			if (_returnToCell != null)
			{
				ugGrid = (Infragistics.Win.UltraWinGrid.UltraGrid)sender;
				e.Cancel = true;
				ugGrid.ActiveCell = _returnToCell;
				ugGrid.PerformAction(UltraGridAction.EnterEditMode,false,false);
				_returnToCell = null;
				
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
        //                    oColumn.Format = "#########";
        //                    break;
        //                case "System.Double":
        //                    oColumn.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
        //                    oColumn.Format = "#######.##";
        //                    break;
        //            }
        //        }
        //    }
        //}
        //End TT#169

		// Begin MID Track 4858 - JSmith - Security changes
//		private void btnProcess_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				ProcessAction(eMethodType.ForecastModifySales);
//				// as part of the  processing we saved the info, so it should be changed to update.
//				if (!ErrorFound)
//				{
//					this._OTSForecastModifySales.Method_Change_Type = eChangeType.update;
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
				ProcessAction(eMethodType.ForecastModifySales);
				// as part of the  processing we saved the info, so it should be changed to update.
				if (!ErrorFound)
				{
					this._OTSForecastModifySales.Method_Change_Type = eChangeType.update;
					btnSave.Text = "&Update";
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}
		// End MID Track 4858

		override public bool VerifySecurity()
		{
			// BEGIN Issue 4323 - stodd 2.21.07 - fix process from workflow explorer
			_versionProfileList = SAB.ClientServerSession.GetUserForecastVersions();
			// END Issue 4323 - stodd 2.21.07 - fix process from workflow explorer
			return ValidateUserForModifedVersion();
		}

		// Begin MID Track 4858 - JSmith - Security changes
//		private void btnSave_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				Save_Click(false);
//
//				if (!ErrorFound)
//				{
//					// Now that this one has been saved, it should be changed to update.
//					_OTSForecastModifySales.Method_Change_Type = eChangeType.update;
//					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
//				}
//			}
//			catch(Exception err)
//			{
//				HandleException(err);
//			}
//		}
//
//		private void btnClose_Click(object sender, System.EventArgs e)
//		{
//			Close();
//		}

		protected override void Call_btnSave_Click()
		{
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

		private void midDateRangeSelector_Load(object sender, System.EventArgs e)
		{
			try
			{
				CalendarDateSelector frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{SAB});
				midDateRangeSelector.DateRangeForm = frm;
				frm.DateRangeRID = midDateRangeSelector.DateRangeRID;
				frm.AllowDynamicToPlan = false;
				frm.AllowDynamicToStoreOpen = false;
				frm.AllowDynamicSwitch = true;
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void midDateRangeSelector_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				// tells the date range selector the currently selected date range RID
				if (midDateRangeSelector.Tag != null)
					((CalendarDateSelector)midDateRangeSelector.DateRangeForm).DateRangeRID = (int)midDateRangeSelector.Tag;			
				// tells the control to show the date range selector form
				midDateRangeSelector.ShowSelector();
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void midDateRangeSelector_OnSelection(object source, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
		{
			try
			{
				if (FormLoaded)
					ChangePending = true;

				DateRangeProfile dr = e.SelectedDateRange;

				if (dr != null)
				{
					midDateRangeSelector.Text= dr.DisplayDate;
			
					//Add RID to Control's Tag (for later use)
					int lAddTag = dr.Key;
			
					midDateRangeSelector.Tag = lAddTag;
					midDateRangeSelector.DateRangeRID = lAddTag;

					//Display Dynamic picture or not
					if (dr.DateRangeType == eCalendarRangeType.Dynamic)
						midDateRangeSelector.SetImage(this.DynamicToCurrentImage);
					else
						midDateRangeSelector.SetImage(null);
					//=========================================================
					// Override the image if this is a dynamic switch date.
					//=========================================================
					if (dr.IsDynamicSwitch)
						midDateRangeSelector.SetImage(this.DynamicSwitchImage);
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void txtMerchandise_TextChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
				ChangePending = true;
		}

		private void txtMerchandise_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
            Merchandise_DragEnter(sender, e);
		}

        private void txtMerchandise_DragOver(object sender, DragEventArgs e)
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
            //Image_DragOver(sender, e);
        }

		private void txtMerchandise_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
            //IDataObject data;
            //ClipboardProfile cbp;
            //HierarchyClipboardData MIDTreeNode_cbd;

			try
			{
                //Begin Track #5858 - Kjohnson - Validating store security only
                bool isSuccessfull = ((MIDTextBoxTag)(((TextBox)sender).Tag)).TextBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)(((TextBox)sender).Tag)).MIDTagData;

                    _OTSForecastModifySales.HierNodeRid = hnp.Key;
                    _nodeRID = hnp.Key;
                    this.CheckMerchandiseGrades(hnp.Key);
                    this.CheckMerchandiseSellThru(hnp.Key);
                    if (tabMatrix.SelectedTab == tabPageMatrix && _rebuildMatrix)
                    {
                        BuildMatrix(_currSet);
                    }

                    ChangePending = true;
                    ApplySecurity();
                }
                //End Track #5858 - Kjohnson

//                // Create a new instance of the DataObject interface.

//                data = Clipboard.GetDataObject();

//                //If the data is ClipboardProfile, then retrieve the data
				
//                if (data.GetDataPresent(ClipboardProfile.Format.Name))
//                {
//                    cbp = (ClipboardProfile)data.GetData(ClipboardProfile.Format.Name);

//                    if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode)
//                    {
//                        if (cbp.ClipboardData.GetType() == typeof(HierarchyClipboardData))
//                        {
//                            MIDTreeNode_cbd = (HierarchyClipboardData)cbp.ClipboardData;
//                            //Begin Track #5378 - color and size not qualified
////							HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbp.Key);
//                            HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbp.Key, false, true);
//                            //End Track #5378

//                            //Begin Track #5858 - KJohnson - Validating store security only
//                            ((TextBox)sender).Text = hnp.Text;
//                            ((MIDTag)(((TextBox)sender).Tag)).MIDTagData = hnp;
//                            //End Track #5858

//                            _OTSForecastModifySales.HierNodeRid = cbp.Key;
//                            _nodeRID = cbp.Key;
//                            this.CheckMerchandiseGrades(cbp.Key);
//                            this.CheckMerchandiseSellThru(cbp.Key);
//                            if (tabMatrix.SelectedTab == tabPageMatrix  && _rebuildMatrix)
//                            {
//                                BuildMatrix(_currSet);
//                            }
//                            ApplySecurity();
//                        }
//                        else
//                        {
//                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard));
//                        }
//                    }
//                    else
//                    {
//                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
//                    }
//                }
//                else
//                {
//                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
//                }
			}
            catch (BadDataInClipboardException)
            {
                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception exc)
			{
				HandleException(exc);
			}
		}

        private void txtMerchandise_DragLeave(object sender, EventArgs e)
        {
            Image_DragLeave (sender, e);
        }

        private void txtMerchandise_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
        //    string productID; 
        //    if (txtMerchandise.Modified)
        //    {
        //        if (txtMerchandise.Text.Trim().Length > 0)
        //        {
        //            productID = txtMerchandise.Text.Trim();
        //            _nodeRID = GetNodeText(ref productID);
        //            if (_nodeRID == Include.NoRID)
        //                MessageBox.Show(productID + " is not valid; please enter or drag and drop a node from the tree");
        //            else 
        //            {
        //                if (txtMerchandise.Text != productID || (int)((MIDTag)(txtMerchandise.Tag)).MIDTagData != _nodeRID)
        //                {
        //                    //Begin Track #5858 - KJohnson - Validating store security only
        //                    txtMerchandise.Text = productID;
        //                    ((MIDTag)(txtMerchandise.Tag)).MIDTagData = _nodeRID;
        //                    //End Track #5858

        //                    this.CheckMerchandiseGrades(_nodeRID);
        //                    this.CheckMerchandiseSellThru(_nodeRID);
        //                    ApplySecurity();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //Begin Track #5858 - KJohnson - Validating store security only
        //            ((MIDTag)(txtMerchandise.Tag)).MIDTagData = null;
        //            //End Track #5858
        //        }
        //    }
        }

        private void txtMerchandise_Validated(object sender, EventArgs e)
        {
            try
            {
                //Begin Track #5858 - KJohnson- Validating store security only
                if ((((TextBox)sender).Text.Trim() == string.Empty) && (((TextBox)sender).Tag != null))
                {
                    //Put Shut Down Code Here
                }
                else
                {
                    HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)((TextBox)sender).Tag).MIDTagData;
                    _nodeRID = hnp.Key;
                    // Begin Track #6267 - JSmith - Unable to save a new method
                    _OTSForecastModifySales.HierNodeRid = hnp.Key;
                    // End Track #6267

                    this.CheckMerchandiseGrades(_nodeRID);
                    this.CheckMerchandiseSellThru(_nodeRID);

                    ChangePending = true;
                    ApplySecurity();
                }
                //End Track #5858
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Begin Track #4872 - JSmith - Global/User Attributes
        override protected void BuildAttributeList()
        {
            ProfileList pl;
            int currValue;
            try
            {
                if (cboAttribute.SelectedValue != null &&
                    cboAttribute.SelectedValue.GetType() == typeof(System.Int32))
                {
                    currValue = Convert.ToInt32(cboAttribute.SelectedValue);
                }
                else
                {
                    currValue = Include.NoRID;
                }
                pl = GetStoreGroupList(_OTSForecastModifySales.Method_Change_Type, _OTSForecastModifySales.GlobalUserType, false);
                // Begin TT#44 - KJohnson - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                cboAttribute.Initialize(SAB, FunctionSecurity, pl.ArrayList, _OTSForecastModifySales.GlobalUserType == eGlobalUserType.User);
                // End TT#44 
                if (currValue != Include.NoRID)
                {
                    cboAttribute.SelectedValue = currValue;
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
            //Begin Track #5858 - JSmith - Validating store security only
            string errorMessage = string.Empty;
            string item = string.Empty;
            //End Track #5858

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

			if (_nodeRID != Include.NoRID)
			{
				_hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_nodeRID, (int)eSecurityTypes.Store);
				if (!_hierNodeSecurity.AllowUpdate)
				{
					btnProcess.Enabled = false;
					btnSave.Enabled = false;
                    //Begin Track #5858 - JSmith - Validating store security only
                    securityOk = false;
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
                    item = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);
                    errorMessage = errorMessage + item + ".";
                    ErrorProvider.SetError(txtMerchandise, errorMessage);
                    //End Track #5858
				}
			}
            securityOk = (((MIDControlTag)(txtMerchandise.Tag)).IsAuthorized(eSecurityTypes.Store, eSecuritySelectType.Update));

            bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
            base.ApplyCanUpdate(canUpdate);
            if (!canUpdate)
            {
                if (FunctionSecurity.IsReadOnly
                    || txtMerchandise.Text.Trim().Length == 0)
                {
                    // Skip
                }
                else
                {
                    securityOk = false;
                }
            }
			return securityOk; // track 5871 stodd
		}

		private int GetNodeText(ref string aProductID)
		{
			string desc = string.Empty;
			try
			{
				string productID = aProductID;
				string[] pArray = productID.Split(new char[] {'['});
				productID = pArray[0].Trim(); 
//				HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(productID);
				HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
				EditMsgs em = new EditMsgs();
				HierarchyNodeProfile hnp =  hm.NodeLookup(ref em, productID, false);
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

		private bool MatrixWarningOK (string aChangedItem)
		{
			DialogResult diagResult;
			string errorMessage = string.Empty;
			bool continueProcess = true;
			try
			{
				if (DoesMatrixGridHaveData())
				{
					errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_VelocityMatrixDeleteWarning),
						aChangedItem );	
					errorMessage += Environment.NewLine + MIDText.GetTextOnly((int)eMIDTextCode.msg_ContinueQuestion);
					diagResult = MessageBox.Show(errorMessage,_thisTitle,
						MessageBoxButtons.YesNo,  MessageBoxIcon.Warning);
					if (diagResult == System.Windows.Forms.DialogResult.No)
						continueProcess = false;
					else
					{
						// Clear Matrix values
						_dtRules.Clear();
						_dtMatrixGrid.Clear();
						_rebuildMatrix = true;
						continueProcess = true;
					}
				}
				else
					continueProcess = true;
			}
			catch(Exception ex)
			{
				HandleException(ex);
				continueProcess = false;
			}
			return continueProcess; 
		}

		private bool DoesMatrixGridHaveData()
		{
			bool hasData = false;
			try
			{
				if (_dtRules.Rows.Count > 0)
				{
					return true;
				}
				else
				{
					foreach (DataRow row in _dtMatrixGrid.Rows)
					{
						for (int j = 2; j < _dtMatrixGrid.Columns.Count; j++)
						{
							DataColumn col = _dtMatrixGrid.Columns[j];
							if (col.ColumnName.Substring(0,4) == "Rule") 
							{
								if (row[col] != System.DBNull.Value)
								{
									hasData = true;
									break;
								}
							}
						}
						if (hasData)
							break;
					}	
					return hasData;
				}
			}
			catch
			{
				throw;
			}
		}

        private void tabMatrix_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				bool errorsFound = false;
				if (this.tabMatrix.SelectedTab == this.tabPageMatrix)
				{
					if (_rebuildMatrix)
					{
					
						EditMsgs em = new EditMsgs();
						if (!ValidateGradesTab(ref em))
						{
							errorsFound = true;
							this.tabMatrix.SelectedTab = this.tabPageGrades;
						}
								
						if (errorsFound)
						{
							string text = MIDText.GetTextOnly(eMIDTextCode.msg_ErrorsFoundReviewCorrect);
							MessageBox.Show(text);
						}

						if (!errorsFound)
							this.BuildMatrix(_currSet);
					}
				}

				if (this.tabMatrix.SelectedTab == this.tabPageGrades)
				{
					if (!ValidateMatrixTab())
					{
						errorsFound = true;
						this.tabMatrix.SelectedTab = this.tabPageMatrix;
					}
								
					if (errorsFound)
					{
						string text = MIDText.GetTextOnly(eMIDTextCode.msg_ErrorsFoundReviewCorrect);
						MessageBox.Show(text);
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private void gridGrades_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				_rebuildMatrix = true;
			}
		}

		private void gridGrades_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
		{
			if (!MatrixWarningOK(e.Rows[0].Cells["BOUNDARY"].Column.Header.Caption))
				e.Cancel = true;
		}

		private void gridGrades_BeforeCellUpdate(object sender, Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs e)
		{
			if (!MatrixWarningOK(e.Cell.Column.Header.Caption))
			{
				_returnToCell = e.Cell;
				e.Cancel = true;
			}
		}

		private void gridGrades_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			try
			{
				ShowUltraGridToolTip(gridGrades, e);
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void gridGrades_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			_rebuildMatrix = true;
		}

		private void gridSellThru_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				_rebuildMatrix = true;
			}
		}

		private void gridSellThru_BeforeCellUpdate(object sender, Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs e)
		{
			if (!MatrixWarningOK(e.Cell.Column.Header.Caption))
			{
				_returnToCell = e.Cell;
				e.Cancel = true;
			}
		}

		private void gridSellThru_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
		{
			if (!MatrixWarningOK(e.Rows[0].Cells["SELL_THRU"].Column.Header.Caption))
				e.Cancel = true;
		}

		private void gridSellThru_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			_rebuildMatrix = true;
		}

		private void gridSellThru_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			try
			{
				ShowUltraGridToolTip(gridSellThru, e);
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void gridMatrix_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			try
			{
				ShowUltraGridToolTip(gridMatrix, e);
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void gridGrades_AfterRowsDeleted(object sender, System.EventArgs e)
		{
			_rebuildMatrix = true;
		}

		private void gridSellThru_AfterRowsDeleted(object sender, System.EventArgs e)
		{
			_rebuildMatrix = true;
		}

        // Begin Track #4872 - JSmith - Global/User Attributes
        private void frmModifySalesMethod_Load(object sender, EventArgs e)
        {
            if (cboAttribute.ReplaceAttribute)
            {
                ChangePending = true;
            }
        }

        private void cboAttribute_DragEnter(object sender, DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void cboAttribute_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        private void cboFilter_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }
        // End Track #4872

	}

}
