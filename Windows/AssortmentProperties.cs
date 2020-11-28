using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
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
	/// Summary description for Assortment Properties.
	/// </summary>
	public class frmAssortmentProperties :  MIDRetail.Windows.AssortmentBasisBase
	{
		public delegate void AssortmentPropertiesChangeEventHandler(object source, AssortmentPropertiesChangeEventArgs e);
		public event AssortmentPropertiesChangeEventHandler OnAssortmentPropertiesChangeHandler;

		//public delegate void AssortmentPropertiesSaveEventHandler(object source, AssortmentPropertiesSaveEventArgs e);
		//public event AssortmentPropertiesSaveEventHandler OnAssortmentPropertiesSaveHandler;

		public delegate void AssortmentPropertiesCloseEventHandler(object source, AssortmentPropertiesCloseEventArgs e);
		public event AssortmentPropertiesCloseEventHandler OnAssortmentPropertiesCloseHandler;

		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MenuItem menuItemDelete;
		private System.Windows.Forms.MenuItem menuItemInsert;
		private int _nodeRid;
		private UltraGrid _mouseDownGrid;
		private TabPage tabProperties;
		private GroupBox gbApplyTo;
		private TextBox txtMerchandise;
		private Label lblDelivery;
		private Label lblMerchandise;
		private MIDDateRangeSelector midDateRangeSelector;
		private GroupBox groupBox1;
		private Label lblReserve;
		private TextBox txtReserve;
		private RadioButton radPercent;
		private RadioButton radUnits;
		private TabControl tabControl1;
		private Label lblAssortment;
		private TextBox txtAssortmentId;
		private TextBox txtAssortmentDesc;
		private AssortmentProfile _asp;
		private bool _readOnly;
        private bool _assortmentIDChanged;
		private eChangeType _changeType;
		private AssortmentBasisReader _basisReader;
		private int _variableNumber;
		private MIDTreeNode _parentNode;
		private ArrayList _selectedHeaderKeyList;
        private ArrayList _selectedAssortmentKeyList; // TT#488 - MD - Jellis - Group Allocation

		// All the stores in these lists line up.
		private ProfileList _storeProfileList;
		private List<int> _storeList = new List<int>();
		private List<int> _storeValueList = new List<int>();
		private List<int> _storeSetList = new List<int>();
		private List<double> _storeIndexList = new List<double>();
		private List<int> _avgStoreList = new List<int>();
		private List<int> _storeIntransitList = new List<int>();
		private List<int> _storeNeedList = new List<int>();
		private List<decimal> _storePctNeedList = new List<decimal>();
		//==========================================================
		private Hashtable _setAvgHash = new Hashtable();
		private Hashtable _setTotalHash = new Hashtable();
		private Hashtable _setStoreCountHash = new Hashtable();
		private Hashtable _storeIndexHash = new Hashtable();
		private Hashtable _storeSetHash = new Hashtable();
		private Hashtable _storeToListHash = new Hashtable();
		//==========================================================
		private bool _reprocess = false;
        private bool _assortmentPropertiesChanged = false;   // TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO

        private HierarchyProfile _mainHp;                   // Begin TT#1289 - RMatelic -Apply To merchandise needs to be restricted to Style and above
        private HierarchyLevelProfile _hlpStyle;            // End TT#1289
        private HierarchyLevelProfile _hlpParentOfStyle;    // Begin TT#2091 - RMatelic - Assortment Properties- 'Apply To'  Merchandise node needs to be a hierachy level of Parent of Style or higher in the hierarchy.
        private string _previousTextValue;
        private Label lblBeginDay;
        private MIDDateRangeSelector beginDayDateRangeSelector;                  // End TT#2091

		// BEGIN TT#343-MD - stodd - When creating a new Assortment it is set as Read Only
		ApplicationSessionTransaction _transaction;		
		// END TT#343-MD - stodd - When creating a new Assortment it is set as Read Only

        private bool _assortmentMemberListbuilt = false;  // TT#2101-MD - JSmith - Matrix values change when beg and Del dates changed without REDO being selected.
			
        //=========================
		// Constructors
		//=========================

		public frmAssortmentProperties(
			SessionAddressBlock aSAB,
			ExplorerAddressBlock aEAB,
			MIDTreeNode parentNode,
			AssortmentProfile ap,
			bool aReadOnly)
			: base(aSAB, aEAB, ap, aReadOnly)
		{
			try
			{
				_asp = ap;
				// BEGIN TT#343-MD - stodd - When creating a new Assortment it is set as Read Only
				_transaction = _asp.AppSessionTransaction;
				// END TT#343-MD - stodd - When creating a new Assortment it is set as Read Only
				_readOnly = aReadOnly;
				_parentNode = parentNode;

				AllowDragDrop = true;
				InitializeComponent();
                //AnchorStoreGradesGrid();  // TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
			}
			catch
			{
				throw;
			}
		}

		public frmAssortmentProperties()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            //AnchorStoreGradesGrid();  // TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
			//_inDesigner = true;
		}

		//=========================
		// Methods
		//=========================

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
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);
                this.radReceipts.CheckedChanged -= new System.EventHandler(this.radReceipts_CheckedChanged);
                this.radSales.CheckedChanged -= new System.EventHandler(this.radSales_CheckedChanged);
                this.radStock.CheckedChanged -= new System.EventHandler(this.radStock_CheckedChanged);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
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
            this.menuItemDelete = new System.Windows.Forms.MenuItem();
            this.menuItemInsert = new System.Windows.Forms.MenuItem();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.gbApplyTo = new System.Windows.Forms.GroupBox();
            this.lblBeginDay = new System.Windows.Forms.Label();
            this.beginDayDateRangeSelector = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.txtMerchandise = new System.Windows.Forms.TextBox();
            this.lblDelivery = new System.Windows.Forms.Label();
            this.lblMerchandise = new System.Windows.Forms.Label();
            this.midDateRangeSelector = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblReserve = new System.Windows.Forms.Label();
            this.txtReserve = new System.Windows.Forms.TextBox();
            this.radPercent = new System.Windows.Forms.RadioButton();
            this.radUnits = new System.Windows.Forms.RadioButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.lblAssortment = new System.Windows.Forms.Label();
            this.txtAssortmentId = new System.Windows.Forms.TextBox();
            this.txtAssortmentDesc = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            this.gbAverage.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.tabProperties.SuspendLayout();
            this.gbApplyTo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.Location = new System.Drawing.Point(75, 25);
            this.cboStoreAttribute.TabIndex = 3;
            this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cboStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted_1);
            // 
            // lblAttribute
            // 
            this.lblAttribute.Location = new System.Drawing.Point(16, 28);
            // 
            // cbxSimilarStores
            // 
            this.cbxSimilarStores.Location = new System.Drawing.Point(72, 447);
            // 
            // cbxIntransit
            // 
            this.cbxIntransit.Location = new System.Drawing.Point(364, 447);
            // 
            // cbxOnhand
            // 
            this.cbxOnhand.Location = new System.Drawing.Point(216, 447);
            // 
            // tabControl2
            // 
            this.tabControl2.Location = new System.Drawing.Point(25, 199);
            this.tabControl2.Size = new System.Drawing.Size(676, 310);
            // 
            // radStock
            // 
            this.radStock.CheckedChanged += new System.EventHandler(this.radStock_CheckedChanged);
            // 
            // radSales
            // 
            this.radSales.CheckedChanged += new System.EventHandler(this.radSales_CheckedChanged);
            // 
            // radReceipts
            // 
            this.radReceipts.CheckedChanged += new System.EventHandler(this.radReceipts_CheckedChanged);
            // 
            // cbxCommitted
            // 
            this.cbxCommitted.Location = new System.Drawing.Point(494, 449);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(503, 561);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(605, 561);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // menuItemDelete
            // 
            this.menuItemDelete.Index = -1;
            this.menuItemDelete.Text = "Delete";
            this.menuItemDelete.Click += new System.EventHandler(this.menuItemDelete_Click);
            // 
            // menuItemInsert
            // 
            this.menuItemInsert.Index = -1;
            this.menuItemInsert.Text = "Insert";
            this.menuItemInsert.Click += new System.EventHandler(this.menuItemInsert_Click);
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.cbxCommitted);
            this.tabProperties.Controls.Add(this.gbApplyTo);
            this.tabProperties.Controls.Add(this.cboStoreAttribute);
            this.tabProperties.Controls.Add(this.lblAttribute);
            this.tabProperties.Controls.Add(this.groupBox1);
            this.tabProperties.Controls.Add(this.cbxSimilarStores);
            this.tabProperties.Controls.Add(this.cbxIntransit);
            this.tabProperties.Controls.Add(this.cbxOnhand);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(702, 475);
            this.tabProperties.TabIndex = 0;
            this.tabProperties.Text = "Properties";
            this.tabProperties.Controls.SetChildIndex(this.cbxOnhand, 0);
            this.tabProperties.Controls.SetChildIndex(this.cbxIntransit, 0);
            this.tabProperties.Controls.SetChildIndex(this.cbxSimilarStores, 0);
            this.tabProperties.Controls.SetChildIndex(this.groupBox1, 0);
            this.tabProperties.Controls.SetChildIndex(this.lblAttribute, 0);
            this.tabProperties.Controls.SetChildIndex(this.cboStoreAttribute, 0);
            this.tabProperties.Controls.SetChildIndex(this.gbApplyTo, 0);
            this.tabProperties.Controls.SetChildIndex(this.cbxCommitted, 0);
            // 
            // gbApplyTo
            // 
            this.gbApplyTo.Controls.Add(this.lblBeginDay);
            this.gbApplyTo.Controls.Add(this.beginDayDateRangeSelector);
            this.gbApplyTo.Controls.Add(this.txtMerchandise);
            this.gbApplyTo.Controls.Add(this.lblDelivery);
            this.gbApplyTo.Controls.Add(this.lblMerchandise);
            this.gbApplyTo.Controls.Add(this.midDateRangeSelector);
            this.gbApplyTo.Location = new System.Drawing.Point(353, 4);
            this.gbApplyTo.Name = "gbApplyTo";
            this.gbApplyTo.Size = new System.Drawing.Size(290, 120);
            this.gbApplyTo.TabIndex = 13;
            this.gbApplyTo.TabStop = false;
            this.gbApplyTo.Text = "Apply To";
            // 
            // lblBeginDay
            // 
            this.lblBeginDay.AutoSize = true;
            this.lblBeginDay.Location = new System.Drawing.Point(37, 56);
            this.lblBeginDay.Name = "lblBeginDay";
            this.lblBeginDay.Size = new System.Drawing.Size(56, 13);
            this.lblBeginDay.TabIndex = 9;
            this.lblBeginDay.Text = "BeginDay:";
            // 
            // beginDayDateRangeSelector
            // 
            this.beginDayDateRangeSelector.DateRangeForm = null;
            this.beginDayDateRangeSelector.DateRangeRID = 0;
            this.beginDayDateRangeSelector.Location = new System.Drawing.Point(97, 50);
            this.beginDayDateRangeSelector.Name = "beginDayDateRangeSelector";
            this.beginDayDateRangeSelector.Size = new System.Drawing.Size(174, 24);
            this.beginDayDateRangeSelector.TabIndex = 10;
            this.beginDayDateRangeSelector.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.beginDayDateRangeSelector_OnSelection);
            this.beginDayDateRangeSelector.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.beginDayDateRangeSelector_ClickCellButton);
            this.beginDayDateRangeSelector.Load += new System.EventHandler(this.beginDayDateRangeSelector_Load);
            // 
            // txtMerchandise
            // 
            this.txtMerchandise.AllowDrop = true;
            this.txtMerchandise.Location = new System.Drawing.Point(97, 20);
            this.txtMerchandise.Name = "txtMerchandise";
            this.txtMerchandise.Size = new System.Drawing.Size(173, 20);
            this.txtMerchandise.TabIndex = 4;
            this.txtMerchandise.TextChanged += new System.EventHandler(this.txtMerchandise_TextChanged);
            this.txtMerchandise.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragDrop);
            this.txtMerchandise.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragEnter);
            this.txtMerchandise.DragOver += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragOver);
            this.txtMerchandise.DragLeave += new System.EventHandler(this.txtMerchandise_DragLeave);
            this.txtMerchandise.Validating += new System.ComponentModel.CancelEventHandler(this.txtMerchandise_Validating);
            this.txtMerchandise.Validated += new System.EventHandler(this.txtMerchandise_Validated);
            // 
            // lblDelivery
            // 
            this.lblDelivery.AutoSize = true;
            this.lblDelivery.Location = new System.Drawing.Point(45, 86);
            this.lblDelivery.Name = "lblDelivery";
            this.lblDelivery.Size = new System.Drawing.Size(48, 13);
            this.lblDelivery.TabIndex = 2;
            this.lblDelivery.Text = "Delivery:";
            // 
            // lblMerchandise
            // 
            this.lblMerchandise.AutoSize = true;
            this.lblMerchandise.Location = new System.Drawing.Point(22, 24);
            this.lblMerchandise.Name = "lblMerchandise";
            this.lblMerchandise.Size = new System.Drawing.Size(71, 13);
            this.lblMerchandise.TabIndex = 1;
            this.lblMerchandise.Text = "Merchandise:";
            // 
            // midDateRangeSelector
            // 
            this.midDateRangeSelector.DateRangeForm = null;
            this.midDateRangeSelector.DateRangeRID = 0;
            this.midDateRangeSelector.Location = new System.Drawing.Point(97, 80);
            this.midDateRangeSelector.Name = "midDateRangeSelector";
            this.midDateRangeSelector.Size = new System.Drawing.Size(174, 24);
            this.midDateRangeSelector.TabIndex = 8;
            this.midDateRangeSelector.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.midDateRangeSelector_OnSelection);
            this.midDateRangeSelector.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.midDateRangeSelector_ClickCellButton);
            this.midDateRangeSelector.Load += new System.EventHandler(this.midDateRangeSelector_Load);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblReserve);
            this.groupBox1.Controls.Add(this.txtReserve);
            this.groupBox1.Controls.Add(this.radPercent);
            this.groupBox1.Controls.Add(this.radUnits);
            this.groupBox1.Location = new System.Drawing.Point(14, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 37);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // lblReserve
            // 
            this.lblReserve.Location = new System.Drawing.Point(16, 15);
            this.lblReserve.Name = "lblReserve";
            this.lblReserve.Size = new System.Drawing.Size(56, 16);
            this.lblReserve.TabIndex = 0;
            this.lblReserve.Text = "Reserve:";
            // 
            // txtReserve
            // 
            this.txtReserve.Location = new System.Drawing.Point(72, 12);
            this.txtReserve.Name = "txtReserve";
            this.txtReserve.Size = new System.Drawing.Size(56, 20);
            this.txtReserve.TabIndex = 5;
            this.txtReserve.TextChanged += new System.EventHandler(this.txtReserve_TextChanged);
            // 
            // radPercent
            // 
            this.radPercent.Location = new System.Drawing.Point(143, 10);
            this.radPercent.Name = "radPercent";
            this.radPercent.Size = new System.Drawing.Size(62, 24);
            this.radPercent.TabIndex = 6;
            this.radPercent.Text = "Percent";
            this.radPercent.Click += new System.EventHandler(this.RadioButtonClick_Click);
            // 
            // radUnits
            // 
            this.radUnits.Location = new System.Drawing.Point(225, 10);
            this.radUnits.Name = "radUnits";
            this.radUnits.Size = new System.Drawing.Size(62, 24);
            this.radUnits.TabIndex = 7;
            this.radUnits.Text = "Units";
            this.radUnits.Click += new System.EventHandler(this.RadioButtonClick_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabProperties);
            this.tabControl1.Location = new System.Drawing.Point(9, 47);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(710, 501);
            this.tabControl1.TabIndex = 3;
            // 
            // lblAssortment
            // 
            this.lblAssortment.AutoSize = true;
            this.lblAssortment.Location = new System.Drawing.Point(13, 13);
            this.lblAssortment.Name = "lblAssortment";
            this.lblAssortment.Size = new System.Drawing.Size(62, 13);
            this.lblAssortment.TabIndex = 4;
            this.lblAssortment.Text = "Assortment:";
            // 
            // txtAssortmentId
            // 
            this.txtAssortmentId.Location = new System.Drawing.Point(79, 10);
            this.txtAssortmentId.Name = "txtAssortmentId";
            this.txtAssortmentId.Size = new System.Drawing.Size(239, 20);
            this.txtAssortmentId.TabIndex = 1;
            this.txtAssortmentId.TextChanged += new System.EventHandler(this.txtAssortmentId_TextChanged);
            this.txtAssortmentId.Validated += new System.EventHandler(this.txtAssortmentId_Validated);
            // 
            // txtAssortmentDesc
            // 
            this.txtAssortmentDesc.Location = new System.Drawing.Point(330, 10);
            this.txtAssortmentDesc.Name = "txtAssortmentDesc";
            this.txtAssortmentDesc.Size = new System.Drawing.Size(239, 20);
            this.txtAssortmentDesc.TabIndex = 2;
            this.txtAssortmentDesc.TextChanged += new System.EventHandler(this.txtAssortmentDesc_TextChanged);
            // 
            // frmAssortmentProperties
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(726, 595);
            this.Controls.Add(this.txtAssortmentDesc);
            this.Controls.Add(this.txtAssortmentId);
            this.Controls.Add(this.lblAssortment);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmAssortmentProperties";
            this.Text = "Assortment Properties";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAssortmentProperties_FormClosing);
            this.Load += new System.EventHandler(this.frmAssortmentProperties_Load);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.Controls.SetChildIndex(this.lblAssortment, 0);
            this.Controls.SetChildIndex(this.txtAssortmentId, 0);
            this.Controls.SetChildIndex(this.txtAssortmentDesc, 0);
            this.Controls.SetChildIndex(this.btOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.tabControl2, 0);
            this.panel2.ResumeLayout(false);
            this.gbAverage.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.tabProperties.ResumeLayout(false);
            this.tabProperties.PerformLayout();
            this.gbApplyTo.ResumeLayout(false);
            this.gbApplyTo.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        // Begin TT#2101-MD - JSmith - Matrix values change when beg and Del dates changed without REDO being selected.
        override protected void BeforeClosing()
        {
            try
            {
                if (_assortmentMemberListbuilt
                    && !OkClicked)
                {
                    _transaction.DequeueHeaders();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "AssortmentView.BeforeClosing");
            }
        }
        // End TT#2101-MD - JSmith - Matrix values change when beg and Del dates changed without REDO being selected.

        private void frmAssortmentProperties_Load(object sender, System.EventArgs e)
		{
			FormLoaded = false;
			_selectedHeaderKeyList = new ArrayList();
            _selectedAssortmentKeyList = new ArrayList(); // TT#488 - MD - JEllis - Group Allocation
			SetScreenText();

            // Begin TT#2 - JSmith - Assortment Security
            //FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAssortment);
            FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentProperties);
            // End TT#2
			//_AssortmentUserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersUser);
			//_AssortmentGlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersGlobal);

			if (_readOnly)
			{
				FunctionSecurity.SetReadOnly();
				this.Text += " [Read Only]";	//BEGIN TT#572-MD - stodd - Unrelated issue. Read only not showing in title.
			}
			SetReadOnly(FunctionSecurity.AllowUpdate);

            if (cboStoreAttribute.ReplaceAttribute)
            {
                ChangePending = true;
            }

			if (_asp.Key == Include.NoRID)
			{
				_changeType = eChangeType.add;
				this.radReceipts.Checked = true;  // TT#2 - stodd
                this.cbxOnhand.Enabled = false;  // TT#1979-MD - AGallagher - Asst Properties_ Current On hand and Intransit_when Active inconsistent
                this.cbxIntransit.Enabled = false; // TT#1979-MD - AGallagher - Asst Properties_ Current On hand and Intransit_when Active inconsistent

				Common_Load();

                // Begin TT#5124 - JSmith - Performance
                //GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
                //gop.LoadOptions();
                //cboStoreAttribute.SelectedValue = gop.AllocationStoreGroupRID;	// TT#2 - stodd
                cboStoreAttribute.SelectedValue = SAB.ClientServerSession.GlobalOptions.AllocationStoreGroupRID;
                // End TT#5124 - JSmith - Performance
				// BEGIN TT#217-MD - stodd - Running methods from the explorer on Assortments
				txtAssortmentId.Select();
				// END TT#217-MD - stodd - Running methods from the explorer on Assortments
			}
			else
			{
				_changeType = eChangeType.update;
				Common_Load();
			}

            // Begin TT#1289 - RMatelic -Apply To merchandise needs to be restricted to Style and above
            _mainHp = SAB.HierarchyServerSession.GetMainHierarchyData();
             for (int level = 1; level <= _mainHp.HierarchyLevels.Count; level++)
            {
                // Begin TT#2091 - RMatelic - Assortment Properties- 'Apply To'  Merchandise node needs to be a hierachy level of Parent of Style or higher in the hierarchy.
                _hlpParentOfStyle = _hlpStyle;
                 // End TT#2091
                _hlpStyle = (HierarchyLevelProfile)_mainHp.HierarchyLevels[level];
                if (_hlpStyle.LevelType == eHierarchyLevelType.Style)
                {
                    break;
                }
            }
            // End TT#1289  

            // Begin TT#2014-MD - JSmith - Assortment Security
             if (!btOk.Enabled)
             {
                 FunctionSecurityProfile AssortmentReviewAssortment = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentReviewAssortment);
                 FunctionSecurityProfile AssortmentReviewContent = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentReviewContent);
                 FunctionSecurityProfile AssortmentReviewCharacteristic = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentReviewCharacteristic);
                 if (AssortmentReviewAssortment.AllowView
                     || AssortmentReviewAssortment.AllowUpdate
                     || AssortmentReviewContent.AllowView
                     || AssortmentReviewContent.AllowUpdate
                     || AssortmentReviewCharacteristic.AllowView
                     || AssortmentReviewCharacteristic.AllowUpdate
                     )
                 {
                     btOk.Enabled = true;
                 }
             }
            // End TT#2014-MD - JSmith - Assortment Security

			FormLoaded = true;
		}

		/// <summary>
		/// Fills in the text for all of the screen objects
		/// </summary>
		new private void SetScreenText()
		{
			base.SetScreenText();

			lblAssortment.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Assortment);
			gbApplyTo.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ApplyTo);
			lblMerchandise.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);
			lblDelivery.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Delivery);
            lblBeginDay.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Beginning);           // TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working

			tabProperties.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Properties);
			lblReserve.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Reserve);
			radPercent.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Percent);
			radUnits.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Units);
		}

		//private void Common_Load(eGlobalUserType aGlobalUserType)
		private void Common_Load()
		{
			try
			{
				base.LoadBase();

				Name = MIDText.GetTextOnly((int)eMethodType.GeneralAssortment);
				if (_changeType == eChangeType.add)
				{
					//LoadCombos();
					//LoadGrids();
				}
				else
				{
					//if (FunctionSecurity.AllowUpdate)
					//{
					//	LoadProperties();
					//}
					//else
					//{
						//LoadProperties();
					//}
				}
				LoadProperties();

				if (_asp.AssortmentReserveAmount == Include.UndefinedReserve)
				{
					this.radPercent.Enabled = false;
					this.radUnits.Enabled = false;
				}

			}
			catch( Exception exception )
			{
				HandleException(exception);
			}		
		}

		new private void LoadCombos()
		{
			try
			{
				base.LoadCombos();
			}
			catch
			{
				throw;
			}
		}

		private void LoadProperties()
		{
			try
			{
                // Begin TT#2101-MD - JSmith - Matrix values change when beg and Del dates changed without REDO being selected.
                if (_asp.HeaderRID != Include.NoRID)
                {
                    AddSelectedHeadersToTrans(aTrans: _transaction);
                    // Must replace with and test on profile in list or get an out of sync error.
                    _asp = _transaction.GetAssortmentProfileFromList();
                    if (_asp != null)
                    {
                        base.AssortmentProfile = _asp;
                        if (_asp.AssortmentAllocationStarted)
                        // End TT#2104-MD - JSmith - Create Asst- add PH do not allocate- open same asst and change the dates - select Ok recieve system exception Error Asst  Instances out of Sync.
                        {
                            midDateRangeSelector.Enabled = false;
                            txtMerchandise.Enabled = false;
                            beginDayDateRangeSelector.Enabled = false;
                        }
                    }
                    _assortmentMemberListbuilt = true;
                }
                // End TT#2101-MD - JSmith - Matrix values change when beg and Del dates changed without REDO being selected.

				//base.LoadScreen(); //TT#1497 - MD - DOConnell - when selecting a date range that is dynamic to plan there is no icon indicating it is dynamic in the Horizon Date Range.
				txtAssortmentId.Text = _asp.HeaderID;
				txtAssortmentDesc.Text = _asp.HeaderDescription;
				midDateRangeSelector.DateRangeRID = Include.UndefinedCalendarDateRange;
				txtMerchandise.Tag = new MIDMerchandiseTextBoxTag(SAB, txtMerchandise, eMIDControlCode.form_AssortmentProperties, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
													
				//Load Reserve
				if (_asp.AssortmentReserveAmount != Include.UndefinedReserve)
				{
					txtReserve.Text = Convert.ToString(_asp.AssortmentReserveAmount, CultureInfo.CurrentUICulture);
					if (_asp.AssortmentReserveType == eReserveType.Percent)
					{
						this.radPercent.Checked = true;
					}
					else if (_asp.AssortmentReserveType == eReserveType.Units)
					{
						this.radUnits.Checked = true;
					}
				}

                if (_asp.AssortmentVariableType == eAssortmentVariableType.Receipts)
                {
                    radReceipts.Checked = true;
                    this.cbxOnhand.Enabled = false;  // TT#1979-MD - AGallagher - Asst Properties_ Current On hand and Intransit_when Active inconsistent
                    this.cbxIntransit.Enabled = false;   // TT#1979-MD - AGallagher - Asst Properties_ Current On hand and Intransit_when Active inconsistent
                }
                else if (_asp.AssortmentVariableType == eAssortmentVariableType.Stock)
                { 
                    radStock.Checked = true;
                    this.cbxOnhand.Enabled = true;  // TT#1979-MD - AGallagher - Asst Properties_ Current On hand and Intransit_when Active inconsistent
                    this.cbxIntransit.Enabled = true;   // TT#1979-MD - AGallagher - Asst Properties_ Current On hand and Intransit_when Active inconsistent
                }
                else if (_asp.AssortmentVariableType == eAssortmentVariableType.Sales)
                { 
                    radSales.Checked = true;
                    this.cbxOnhand.Enabled = false;  // TT#1979-MD - AGallagher - Asst Properties_ Current On hand and Intransit_when Active inconsistent
                    this.cbxIntransit.Enabled = false;   // TT#1979-MD - AGallagher - Asst Properties_ Current On hand and Intransit_when Active inconsistent
                }

				if (_asp.AssortmentAnchorNodeRid > 0)
				{
					HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_asp.AssortmentAnchorNodeRid, true, true);
					txtMerchandise.Text = hnp.Text;
					((MIDTag)(txtMerchandise.Tag)).MIDTagData = hnp;
                    _previousTextValue = txtMerchandise.Text;   // TT#2091 - RMatelic - Assortment Properties- 'Apply To'  Merchandise node needs to be a hierachy level of Parent of Style or higher in the hierarchy.
				}

				if (_asp.AssortmentCalendarDateRangeRid > 0 && _asp.AssortmentCalendarDateRangeRid != Include.UndefinedCalendarDateRange)
				{
					DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(_asp.AssortmentCalendarDateRangeRid);
					midDateRangeSelector.Text = drp.DisplayDate;
					midDateRangeSelector.DateRangeRID = drp.Key;
                    midDateRangeSelector.Tag = drp.Key;
                    AnchorDateRangeRid = drp.Key;	// TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
				}

                // Begin TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
                if (_asp.AssortmentBeginDayCalendarDateRangeRid > Include.UndefinedCalendarDateRange)
                {
                    DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(_asp.AssortmentBeginDayCalendarDateRangeRid);
                    beginDayDateRangeSelector.Text = drp.DisplayDate;
                    beginDayDateRangeSelector.DateRangeRID = drp.Key;
                    beginDayDateRangeSelector.Tag = drp.Key;
                }
				// End TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working

                // Begin TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
                cbxOnhand.Visible = false;
                cbxIntransit.Visible = false;
                // End TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.

                //// Begin TT#2101-MD - JSmith - Matrix values change when beg and Del dates changed without REDO being selected.
                //if (_asp.HeaderRID != Include.NoRID)
                //{
                //    AddSelectedHeadersToTrans(aTrans:_transaction);
                //    // Must replace with and test on profile in list or get an out of sync error.
                //    // Begin TT#2104-MD - JSmith - Create Asst- add PH do not allocate- open same asst and change the dates - select Ok recieve system exception Error Asst  Instances out of Sync.
                //    //AssortmentProfile asp = _transaction.GetAssortmentProfileFromList();
                //    //if (asp != null
                //    //    && asp.AssortmentAllocationStarted)
                //    _asp = _transaction.GetAssortmentProfileFromList();
                //    if (_asp != null
                //        && _asp.AssortmentAllocationStarted)
                //    // End TT#2104-MD - JSmith - Create Asst- add PH do not allocate- open same asst and change the dates - select Ok recieve system exception Error Asst  Instances out of Sync.
                //    {
                //        midDateRangeSelector.Enabled = false;
                //        txtMerchandise.Enabled = false;
                //        beginDayDateRangeSelector.Enabled = false;
                //    }
                //    _assortmentMemberListbuilt = true;
                //}
                //// End TT#2101-MD - JSmith - Matrix values change when beg and Del dates changed without REDO being selected.
			}	
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		#region WorkflowMethodFormBase Overrides 

		/// <summary>
		/// Use to set the specific fields before updating
		/// </summary>
		override protected void SetSpecificFields()
		{
			try
			{
                _asp.SaveAssortmentMembers = false;  // TT#2109-MD - JSmith - Change the Beginning and Delivery Date in an Asst and the Delivery Date is incorrect.

				// BEGIN TT#207-MD - stodd - Renaming assortment doesn't rename placeholders
				if (_asp.HeaderID != txtAssortmentId.Text)
				{
					AssortmentIdChanged = true;
				}
				// END TT#207-MD - stodd - Renaming assortment doesn't rename placeholders

				_asp.HeaderID = txtAssortmentId.Text;
				_asp.HeaderDescription = txtAssortmentDesc.Text;

				HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)txtMerchandise.Tag).MIDTagData;
                // Begin TT#2109-MD - JSmith - Change the Beginning and Delivery Date in an Asst and the Delivery Date is incorrect.
                //_asp.AssortmentAnchorNodeRid = hnp.Key;
                if (_asp.AssortmentAnchorNodeRid != hnp.Key)
                {
                    _asp.AssortmentAnchorNodeRid = hnp.Key;
                    _asp.PlanHnRID = Include.DefaultPlanHnRID;
                    _asp.SaveAssortmentMembers = true;
                }
                // End TT#2109-MD - JSmith - Change the Beginning and Delivery Date in an Asst and the Delivery Date is incorrect.
				
				base.AnchorNodeRid = hnp.Key;

                // Begin TT#2109-MD - JSmith - Change the Beginning and Delivery Date in an Asst and the Delivery Date is incorrect.
                //__asp.AssortmentCalendarDateRangeRid = midDateRangeSelector.DateRangeRID;
                if (_asp.AssortmentCalendarDateRangeRid != midDateRangeSelector.DateRangeRID)
                {
                    _asp.AssortmentCalendarDateRangeRid = midDateRangeSelector.DateRangeRID;
                    if (_asp.Key != Include.NoRID)
                    {
                        _asp.ResetShipDates();
                    }
                    _asp.AssortmentApplyToDate = null;
                    _asp.SaveAssortmentMembers = true;
                }
                // End TT#2109-MD - JSmith - Change the Beginning and Delivery Date in an Asst and the Delivery Date is incorrect.
				
                // Begin TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
                if (beginDayDateRangeSelector.DateRangeRID >= Include.UndefinedCalendarDateRange)
                {
                    // Begin TT#2109-MD - JSmith - Change the Beginning and Delivery Date in an Asst and the Delivery Date is incorrect.
                    //_asp.AssortmentBeginDayCalendarDateRangeRid = beginDayDateRangeSelector.DateRangeRID;
                    if (_asp.AssortmentBeginDayCalendarDateRangeRid != beginDayDateRangeSelector.DateRangeRID)
                    {
                        _asp.AssortmentBeginDayCalendarDateRangeRid = beginDayDateRangeSelector.DateRangeRID;
                        _asp.BeginDay = Include.UndefinedDate;
                        _asp.AssortmentBeginDay = null;
                        _asp.SaveAssortmentMembers = true;
                    }
                    // End TT#2109-MD - JSmith - Change the Beginning and Delivery Date in an Asst and the Delivery Date is incorrect.
                }
                // End TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working

				// Reserve amount
				if (txtReserve.Text != null && txtReserve.Text.Trim() != string.Empty)
					_asp.AssortmentReserveAmount = Convert.ToDouble(txtReserve.Text, CultureInfo.CurrentUICulture);
				else
					_asp.AssortmentReserveAmount = Include.UndefinedReserve;

				//Reserve Ind
				_asp.AssortmentReserveType = eReserveType.Unknown;
				if (radPercent.Checked)
					_asp.AssortmentReserveType = eReserveType.Percent;
				else if (radUnits.Checked)
					_asp.AssortmentReserveType = eReserveType.Units;


				_asp.AssortmentUserRid = SAB.ClientServerSession.UserRID;
				_asp.AssortmentLastProcessDt = DateTime.Now;

                // Begin TT#2 - RMatelic - commented out these 2 lines which added placeholders everytime
                //HierarchyNodeList nList = SAB.HierarchyServerSession.GetPlaceholderStyles(_asp.AssortmentAnchorNodeRid, 1, 0, _asp.Key);//_asp.PlanHnRID;
                //_asp.StyleHnRID = ((HierarchyNodeProfile)nList[0]).Key;
                if (_asp.Key < 0)
                {
                    // Begin TT#1599 - RMatelic - Removing a Placeholder (style) from the Contents tab, does not remove it from the Merch Explorer.
                    // Deleting an Assortment also does not delete the Placeholders from the Merch Explorer.
                    // It was determined as part of this Test Track that an Assortment Node is not necessary. 
                    //HierarchyMaintenance hierMaint = new HierarchyMaintenance(SAB);
                    //HierarchyNodeProfile asrtHnp = hierMaint.GetAssortmentNode();
                    // End TT#1599

					_asp.StyleHnRID = hnp.Key;	// TT#2 - stodd 
                }
                // End TT#2 
				_asp.AsrtAnchorNodeRid = hnp.Key;	// TT#1487 - stodd

				MIDException midException;
				if (!_asp.SetHeaderType(eHeaderType.Assortment, out midException))
				{
					throw midException;
				}
				//_asp.Assortment = true;

                // Begin TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO
                _assortmentPropertiesChanged = ChangePending;
                // End TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO

			}
			catch
			{
				throw;
			}
		}


		override protected void SaveWindow()
		{
			// BEGIN TT#207-MD - stodd - Renaming assortment doesn't rename placeholders
			List<GenericEnqueue> enqueuePlaceholderList = null;
			// END TT#207-MD - stodd - Renaming assortment doesn't rename placeholders
			try
			{
				//_asp.AppSessionTransaction.AddAllocationProfile(_asp);	// TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
				if (!_asp.HeaderDataRecord.ConnectionIsOpen)
				{
					_asp.HeaderDataRecord.OpenUpdateConnection();
				}

				try
				{
                    //if (_asp.HeaderRID == Include.NoRID)
                    if (_asp.HeaderRID == Include.NoRID || ChangePending)
					{
						_asp.WriteHeader();
						if (!_asp.HeaderDataRecord.ConnectionIsOpen)
						{
							_asp.HeaderDataRecord.OpenUpdateConnection();
						}
					}
					_asp.WriteAssortment();

					// BEGIN TT#207-MD - stodd - Renaming assortment doesn't rename placeholders
					if (AssortmentIdChanged)
					{
						enqueuePlaceholderList = UpdateHeaderIdsOnPlaceholders(_asp);
					}
					// END TT#207-MD - stodd - Renaming assortment doesn't rename placeholders

					_asp.HeaderDataRecord.CommitData();
				}
				finally
				{
					// BEGIN TT#207-MD - stodd - Renaming assortment doesn't rename placeholders
					if (enqueuePlaceholderList != null)
					{
						foreach (GenericEnqueue ge in enqueuePlaceholderList)
						{
							ge.DequeueGeneric();
						}
					}
					// END TT#207-MD - stodd - Renaming assortment doesn't rename placeholders
					_asp.HeaderDataRecord.CloseUpdateConnection();
					ChangePending = false;
					_asp.AppSessionTransaction.RemoveAllocationProfile(_asp);
					// BEGIN TT#508-MD - stodd -  Correct Assortment Properties Enqueue
					// Since we are in the process of saving...
					// If we got here because we are heading to the Assortment Review, we want to remove this event handler so 
					//     the enqueue is not removed.
					// If we're here because of a save after "Cancel" has be pressed, we want to keep the event handler and
					//     let the header be dequeued.
					// BEGIN TT#508-MD - stodd -  Correct Assortment Properties Enqueue
					//if (!base.CancelClicked && !_userClosedForm)
					if (this.OkClicked)
					{
						OnAssortmentPropertiesCloseHandler = null;
					}
					// END TT#508-MD - stodd -  Correct Assortment Properties Enqueue
					// END TT#508-MD - stodd -  Correct Assortment Properties Enqueue
                    // Begin #1286 - RMatelic - Asst - At what point does the Assortment show in the Assortment Workspace
                    // ReloadUpdatedAssortments method doesn't take into account the Assortment Workspace Filter so change to Refresh
                    //int[] hdrRIDs = new int[1];
                    //hdrRIDs[0] = _asp.Key;
                    //EAB.AssortmentWorkspaceExplorer.ReloadUpdatedAssortments(hdrRIDs);
                    EAB.AssortmentWorkspaceExplorer.IRefresh();
                    // End TT#1286 
				}
			}
			catch
			{
				throw;
			}
		}

		// BEGIN TT#207-MD - stodd - Renaming assortment doesn't rename placeholders
		private List<GenericEnqueue> UpdateHeaderIdsOnPlaceholders(AssortmentProfile asp)
		{
			List<GenericEnqueue> enqueueList = new List<GenericEnqueue>(); 

			// Enqueue placeholders
			DataTable dtPlaceholders = _asp.HeaderDataRecord.GetPlaceholdersForAssortment(asp.Key);
			foreach (DataRow row in dtPlaceholders.Rows)
			{
				int phKey = int.Parse(row["HDR_RID"].ToString());
				AllocationHeaderProfile apPlaceholder = (AllocationHeaderProfile)SAB.HeaderServerSession.GetHeaderData(phKey, false, false, true);
				AssortmentHeaderProfile asrtPlaceholder = new AssortmentHeaderProfile(apPlaceholder.HeaderID, apPlaceholder.Key);
				GenericEnqueue objEnqueuePh = EnqueueObject(asrtPlaceholder, false);
				if (objEnqueuePh == null)
				{
					return enqueueList;
				}
				enqueueList.Add(objEnqueuePh);
			}

			foreach (DataRow row in dtPlaceholders.Rows)
			{
				int phKey = int.Parse(row["HDR_RID"].ToString());
				string oldId = row["HDR_ID"].ToString();
				int index = oldId.IndexOf("PhStyle");
				string phNewName = asp.HeaderID + " " + oldId.Substring(index);
				_asp.HeaderDataRecord.AssortmentHeader_Update(phKey, phNewName);
			}

			return enqueueList;
		}

		private GenericEnqueue EnqueueObject(AssortmentHeaderProfile ahp, bool aAllowReadOnly)
		{
			GenericEnqueue objEnqueue;
			string errMsg;

			try
			{
				//objEnqueue = new GenericEnqueue(eLockType.Assortment, aAssortmentProf.Key, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);
				objEnqueue = new GenericEnqueue(eLockType.Assortment, ahp.Key, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);

				try
				{
					objEnqueue.EnqueueGeneric();
				}
				catch (GenericConflictException)
				{
					//errMsg = "The Assortment \"" + ahp.HeaderID + "\" is in use by User " + ((GenericConflict)objEnqueue.ConflictList[0]).UserName + ".";
					errMsg = "Placeholder: " + ahp.HeaderID + " for this assortment is in use by " + ((GenericConflict)objEnqueue.ConflictList[0]).UserName + " and cannot be renamed.";
					MessageBox.Show(errMsg);
					//MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
					
				}

				return objEnqueue;
			}
			catch
			{
				throw;
			}
		}
		// END TT#207-MD - stodd - Renaming assortment doesn't rename placeholders

		/// <summary>
		/// Use to validate the fields that are specific to this method type
		/// </summary>
		override protected bool ValidateSpecificFields()
		{
			// This is dones because after saving the window, the change pending switch is 
			// set back to false. This way, during processing the code can determine whether
			// it needs to reprocess the assortment because of a change or not.
			_reprocess = ChangePending;
			bool methodFieldsValid = true;

			if (txtAssortmentId.Text == string.Empty)
			{
				methodFieldsValid = false;
				ErrorProvider.SetError(txtAssortmentId, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
			}
			else
			{
				ErrorProvider.SetError(txtAssortmentId, string.Empty);
			}
			if (txtAssortmentId.Text.Length > 50)
			{
				methodFieldsValid = false;
				ErrorProvider.SetError(txtAssortmentId, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueExceedsMaximum));
			}

			// BEGIN TT#2200 - stodd - validate node upon save
			if (!ValidReserve())
			{
				methodFieldsValid = false;
			}
			// END TT#2200 - stodd - 

			if (txtMerchandise.Text.Trim().Length == 0)
			{
				methodFieldsValid = false;
				ErrorProvider.SetError(txtMerchandise, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
			}
			else
			{
				// BEGIN TT#2200 - stodd - validate node upon save
				if (!ValidAnchorNodeLevel())
				{
					methodFieldsValid = false;
				}
				// END TT#2200 - stodd - 
			}

			if (this.midDateRangeSelector.DateRangeRID == Include.UndefinedCalendarDateRange)
			{
				methodFieldsValid = false;
				ErrorProvider.SetError(midDateRangeSelector, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
			}
			else
			{
				ErrorProvider.SetError(midDateRangeSelector, string.Empty);
			}

			return methodFieldsValid;
		}

		private int GetNodeRid(ref string aProductID)
		{
			string desc = string.Empty;
			try
			{
				string productID = aProductID;
				string[] pArray = productID.Split(new char[] {'['});
				productID = pArray[0].Trim(); 
				HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(productID);
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

		private bool ValidVersion (UltraGridCell gridCell)
		{
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
		private bool ValidWeight (UltraGridCell gridCell)
		{
			bool errorFound = false;
			string errorMessage = null;
			double dblValue;
			try
			{
				if (gridCell.Value.ToString() == string.Empty)
				{
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					errorFound = true;
				}
				else
				{
					dblValue = Convert.ToDouble(gridCell.Value.ToString(), CultureInfo.CurrentUICulture);
					if (dblValue < 0)
					{
						
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
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

		private bool ValidReserve()
		{
			//initialize all fields to not having an error
			ErrorProvider.SetError (txtReserve,string.Empty);
			bool methodFieldsValid = true;

			string inStr = txtReserve.Text.ToString(CultureInfo.CurrentUICulture).Trim();
			if (inStr != string.Empty)
			{
				try
				{
					string outStr = System.Math.Abs(Convert.ToDecimal(inStr, CultureInfo.CurrentUICulture)).ToString();
				
					if (inStr != outStr)
					{
						methodFieldsValid = false;
						ErrorProvider.SetError (txtReserve,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ReserveQtyCannotBeNeg));
					}
					else if (radPercent.Checked == true)
					{
						double dblValue;
						decimal outdec = System.Math.Abs(Convert.ToDecimal(inStr, CultureInfo.CurrentUICulture));
						if (outdec > 100)
						{
							methodFieldsValid = false;
							ErrorProvider.SetError (txtReserve,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeBetween0And100));
						}
						else
						{
							dblValue = Convert.ToDouble(outdec,CultureInfo.CurrentUICulture);
							dblValue = Math.Round(dblValue,2);
							txtReserve.Text = dblValue.ToString(CultureInfo.CurrentUICulture);
						}	
					}
				}
				catch
				{	
					methodFieldsValid = false;
					ErrorProvider.SetError(txtReserve,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
				}
			}

			return methodFieldsValid;
		}

		// BEGIN TT#2200 - stodd - Validate node upon save
		private bool ValidAnchorNodeLevel()
		{
			ErrorProvider.SetError(txtMerchandise, string.Empty);
			bool methodFieldsValid = true;
			try
			{
				HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)txtMerchandise.Tag).MIDTagData;
				if (hnp.HomeHierarchyLevel >= _hlpStyle.Level)
				{
					methodFieldsValid = false;
					ErrorProvider.SetError(txtMerchandise, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_InvalidMerchandiseLevel));
				}
				return methodFieldsValid;
			}
			catch
			{
				throw;

			}
		}
		// END TT#2200 - stodd - 

		/// <summary>
		/// Use to set the errors to the screen
		/// </summary>
		//override protected void HandleErrors()
		//{

		//}

		/// <summary>
		/// Use to set the specific method object before updating
		/// </summary>
		//override protected void SetObject()
		//{
		//    try
		//    {
		//        ABM = _assortmentGeneralMethod;
		//    }
		//    catch(Exception exception)
		//    {
		//        HandleException(exception);
		//    }
		//}

		#endregion WorkflowMethodFormBase Overrides		


		private void txtAssortmentId_TextChanged(object sender, System.EventArgs e)
		{
            if (FormLoaded)
            {
                ChangePending = true;
                _assortmentIDChanged = true;
            }
		}

		private void txtAssortmentId_Validated(object sender, System.EventArgs e)
		{
			// Begin TT#1180 - stodd
			//if (FormLoaded)
			//{
			//    txtAssortmentDesc.Text = txtAssortmentId.Text;
			//}
			// End TT#1180 - stodd

		}

		private void txtAssortmentDesc_TextChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
				ChangePending = true;
		}

		private void midDateRangeSelector_Load(object sender, System.EventArgs e)
		{
			try
			{
				CalendarDateSelector frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[] { SAB });
				midDateRangeSelector.DateRangeForm = frm;
				frm.DateRangeRID = midDateRangeSelector.DateRangeRID;
				frm.AllowDynamicToPlan = false;
				frm.AllowDynamicToStoreOpen = false;
				frm.AllowDynamicSwitch = false;  //TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
                //frm.AllowDynamicSwitch = true;  //TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
				frm.RestrictToOnlyWeeks = true;
				frm.RestrictToSingleDate = true;
                frm.DefaultToStatic = true; //TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
                frm.OverrideNullAnchorDateDefaults = true;  //TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
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
					midDateRangeSelector.Text = dr.DisplayDate;

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

                    AnchorDateRangeRid = dr.Key;	// TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
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
			Image_DragOver(sender, e);
            // Begin TT#1289 - RMatelic -Apply To merchandise needs to be restricted to Style and above
            bool validDrag = true;
            TreeNodeClipboardList cbList;
            if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
            {
                cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));

                if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
                {
                    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_mainHp.Key, cbList.ClipboardProfile.Key);
                    // Begin TT#2091 - RMatelic - Assortment Properties- 'Apply To'  Merchandise node needs to be a hierachy level of Parent of Style or higher in the hierarchy.
                    //if (hnp.HomeHierarchyLevel > _hlpStyle.Level)
                    if (hnp.HomeHierarchyLevel >= _hlpStyle.Level)
                    // End TT#2091
                    {
                        validDrag = false;
                    }
                    // Begin TT#2108-MD - JSmith - An Alternate Hierachy can be used in the Apply to for Merchandise.  This is not allowed because when charging intransit it would not know where to put it.
                    else if (hnp.HomeHierarchyType == eHierarchyType.alternate)
                    {
                        validDrag = false;
                    }
                    // End TT#2108-MD - JSmith - An Alternate Hierachy can be used in the Apply to for Merchandise.  This is not allowed because when charging intransit it would not know where to put it.

                }
                else
                {
                    validDrag = false;
                }
            }
            else
            {
                validDrag = false;
            }
            if (validDrag)
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
            // End TT#1289
		}

		private void txtMerchandise_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			try
			{
				bool isSuccessfull = ((MIDTextBoxTag)(((TextBox)sender).Tag)).TextBox_DragDrop(sender, e);

				if (isSuccessfull)
				{
					HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)(((TextBox)sender).Tag)).MIDTagData;

					_asp.AssortmentAnchorNodeRid = hnp.Key;
					_nodeRid = hnp.Key;
					LoadMerchandiseStoreGrades(_nodeRid, eGradesLoadedFrom.ApplyTo);
					ChangePending = true;
					//ApplySecurity();
				}
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
			Image_DragLeave(sender, e);
		}

		private void txtMerchandise_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			
		}

		private void txtMerchandise_Validated(object sender, EventArgs e)
		{
			try
			{
				if ((((TextBox)sender).Text.Trim() == string.Empty) && (((TextBox)sender).Tag != null))
				{
					//Put Shut Down Code Here
				}
				else
				{
					HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)((TextBox)sender).Tag).MIDTagData;
                    // Begin TT#2091 - RMatelic - Assortment Properties- 'Apply To'  Merchandise node needs to be a hierachy level of Parent of Style or higher in the hierarchy.
                    if (hnp.HomeHierarchyLevel >= _hlpStyle.Level)
                    {
                        string errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_InvalidMerchandiseLevel), _hlpParentOfStyle.LevelID);
                        MessageBox.Show(errorMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtMerchandise.Text = _previousTextValue;
                        return;
                    }
                    // Begin TT#2108-MD - JSmith - An Alternate Hierachy can be used in the Apply to for Merchandise.  This is not allowed because when charging intransit it would not know where to put it.
                    else if (hnp.HomeHierarchyType == eHierarchyType.alternate)
                    {
                        string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NodeCannotBeAlternate);
                        MessageBox.Show(errorMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtMerchandise.Text = _previousTextValue;
                        return;
                    }
                    // End TT#2108-MD - JSmith - An Alternate Hierachy can be used in the Apply to for Merchandise.  This is not allowed because when charging intransit it would not know where to put it.
                    _previousTextValue = txtMerchandise.Text;
                    // End TT#2091  
					_nodeRid = hnp.Key;
					_asp.AssortmentAnchorNodeRid = hnp.Key;
					LoadMerchandiseStoreGrades(_nodeRid, eGradesLoadedFrom.ApplyTo);
					ChangePending = true;
					//ApplySecurity();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		//=========================
		// Processing Assortment
		//=========================
		override protected bool Process()
		{
			try
			{
				bool doCommit = false;
				bool processed = false;

				// BEGIN TT#343-MD - stodd - When creating a new Assortment it is set as Read Only
				//ApplicationSessionTransaction appTransaction = SAB.ApplicationServerSession.CreateTransaction();
				// END TT#343-MD - stodd - When creating a new Assortment it is set as Read Only

				//=====================================================
				// sets the assortment info in the Assortment profile
				//=====================================================
				//AssortmentProfile asp = (AssortmentProfile)aAssortmentProfile;

				// If an update connection is already open, then lets use it.
				// Otherwise, we open one...and we'll commit and close when we're done.
				if (!_asp.HeaderDataRecord.ConnectionIsOpen)
				{
					_asp.HeaderDataRecord.OpenUpdateConnection();
					doCommit = true;
				}
				try
				{
					//took out To compile - stodd
					//asp.SetAssortmentHeader(asp.HeaderRID, _reserveAmount, _reserveType, SG_RID, _variableType, _variableNumber,
					//                    _onHandInd, _intransitInd, _simStoreInd, _averageBy, this.Key, aApplicationTransaction.UserRid,
					//                    DateTime.Now);

					// This lists should already be correct from the Save();
					////============================================================
					//// sets the assortment store grades in the Assortment profile
					////============================================================
					//List<string> gradeCodeList = new List<string>();
					//List<double> boundaryList = new List<double>();
					//List<double> boundaryUnitsList = new List<double>();
					//foreach (DataRow aRow in _dtStoreGrades.Rows)
					//{
					//    gradeCodeList.Add(aRow["GRADE_CODE"].ToString());
					//    boundaryList.Add(Convert.ToDouble(aRow["BOUNDARY_INDEX"], CultureInfo.CurrentUICulture));
					//    boundaryUnitsList.Add(Convert.ToDouble(aRow["BOUNDARY_UNITS"], CultureInfo.CurrentUICulture));
					//}
					//_asp.SetAssortmentGrades(_asp.HeaderRID, gradeCodeList, boundaryList, boundaryUnitsList);

					//=====================================================
					// sets the assortment basis in the Assortment profile
					//=====================================================
					List<int> hierNodeList = new List<int>();
					List<int> versionList = new List<int>();
					List<int> dateRangeList = new List<int>();
					List<double> weightList = new List<double>();
					//foreach (DataRow aRow in _dtBasis.Rows)
					foreach (AssortmentBasis ab in _asp.AssortmentBasisList)
					{
						hierNodeList.Add(ab.HierarchyNodeProfile.Key);
						versionList.Add(ab.VersionProfile.Key);
						dateRangeList.Add(ab.HorizonDate.Key);
						weightList.Add(ab.Weight);

						//hierNodeList.Add(Convert.ToInt32(aRow["HN_RID"], CultureInfo.CurrentUICulture));
						//versionList.Add(Convert.ToInt32(aRow["FV_RID"], CultureInfo.CurrentUICulture));
						//dateRangeList.Add(Convert.ToInt32(aRow["HORIZON_CDR_RID"], CultureInfo.CurrentUICulture));
						//weightList.Add(Convert.ToDouble(aRow["WEIGHT"], CultureInfo.CurrentUICulture));
					}
					_asp.SetAssortmentBasis(_asp.HeaderRID, hierNodeList, versionList, dateRangeList, weightList);

                    // Begin TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO
                    if (_assortmentPropertiesChanged)
                    {
                        if (_asp.IsAssortment
                        && _asp.BeginDay != _asp.AssortmentBeginDay.Date)
                        {
                            _asp.BeginDayIsSet = false;
                            _asp.BeginDay = Include.UndefinedDate;
                            _asp.BeginDay = _asp.AssortmentBeginDay.Date;
                        }

                        if (_asp.IsAssortment
                            && _asp.ShipToDay != _asp.AssortmentApplyToDate.Date)
                        {
                            _asp.ResetAssortmentStoreShipDates();
                            _asp.ShipToDay = Include.UndefinedDate;
                            _asp.ShipToDay = _asp.AssortmentApplyToDate.Date;
                        }
                    }
                    // End TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO

                    // Begin TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO
                    //if (_asp.AssortmentSummaryProfile == null)
                    if (_asp.AssortmentSummaryProfile == null
                        || _assortmentPropertiesChanged)  // re-init the assortment summary
                    // End TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO
					{
						_asp.BuildAssortmentSummary();
						//_asp.AssortmentSummaryProfile = new AssortmentSummaryProfile(_asp.Key, SAB, sgp, _asp.StoreGradeList);
					}
					else
					{
                        StoreGroupProfile sgp = StoreMgmt.StoreGroup_GetFilled(_asp.AssortmentStoreGroupRID); //SAB.StoreServerSession.GetStoreGroupFilled(_asp.AssortmentStoreGroupRID);
						_asp.AssortmentSummaryProfile.ReinitializeSummary(sgp, _asp.AssortmentStoreGradeList);		// TT#488-MD - STodd - Group Allocation 
					}
					//============================================================
					// Calculates and sets the store bases values for the method
					//============================================================

                    // Begin TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO
                    bool refreshBasisData = false;
                    if (_assortmentPropertiesChanged)
                    {
                        _asp.AssortmentSummaryProfile.ClearAssortmentSummaryTable();
                        _reprocess = true;
                        refreshBasisData = AssortmentBasisChanged;
                    }

					// BEGIN TT#343-MD - stodd - When creating a new Assortment it is set as Read Only
                    // Begin TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO
                    //_asp.AssortmentSummaryProfile.Process(_transaction, _asp.AssortmentAnchorNodeRid, _asp.AssortmentVariableType, hierNodeList, versionList, dateRangeList, weightList, _asp.AssortmentIncludeSimilarStores,
                    //    _asp.AssortmentIncludeIntransit, _asp.AssortmentIncludeOnhand, _asp.AssortmentIncludeCommitted, _asp.AssortmentAverageBy, _reprocess, false);
					_asp.AssortmentSummaryProfile.Process(_transaction, _asp.AssortmentAnchorNodeRid, _asp.AssortmentVariableType, hierNodeList, versionList, dateRangeList, weightList, _asp.AssortmentIncludeSimilarStores,
                        _asp.AssortmentIncludeIntransit, _asp.AssortmentIncludeOnhand, _asp.AssortmentIncludeCommitted, _asp.AssortmentAverageBy, _reprocess, refreshBasisData);
                    // End TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO
					// END TT#343-MD - stodd - When creating a new Assortment it is set as Read Only

					if (_reprocess)
					{
						_asp.AssortmentSummaryProfile.WriteAssortmentStoreSummary(_asp.HeaderDataRecord);

						if (doCommit)
						{
							_asp.HeaderDataRecord.CommitData();
						}
					}

                    // Begin TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO
                    if (_assortmentPropertiesChanged)
                    {
                        _asp.AssortmentSummaryProfile.RereadStoreSummaryData();
                        _asp.AssortmentSummaryProfile.BuildSummary(_asp.LastSglRidUsedInSummary);
                    }
                    // End TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO
				
					processed = true;

                    if (_changeType == eChangeType.add || _assortmentIDChanged)
                    {
						AssortmentHeaderProfile ahp = new AssortmentHeaderProfile(_asp.HeaderID, _asp.Key);
						AssortmentPropertiesChangeEventArgs eventArgs = new AssortmentPropertiesChangeEventArgs(_parentNode, ahp);
                        if (OnAssortmentPropertiesChangeHandler != null)
                        {
                            OnAssortmentPropertiesChangeHandler(this, eventArgs);
                        }
                        if (_assortmentIDChanged)
                        {
                            _assortmentIDChanged = false;
                        }
                    }
                }
				finally
				{
					if (doCommit)
					{
						_asp.HeaderDataRecord.CloseUpdateConnection();
					}
                    // Begin TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO
                    _assortmentPropertiesChanged = false;
                    // End TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO
				}

				return processed;

			}
			catch
			{

				throw;
			}
		}

		protected override void AfterClosing()
		{
			try
			{
				base.AfterClosing();

				if (OnAssortmentPropertiesCloseHandler != null)
				{
					OnAssortmentPropertiesCloseHandler(this, new AssortmentPropertiesCloseEventArgs());
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		//protected override void Call_btnProcess_Click()
		//{
		//    try
		//    {
		//        ProcessAction(eMethodType.GeneralAssortment);

		//        // as part of the  processing we saved the info, so it should be changed to update.
		//        if (!ErrorFound)
		//        {
		//            _assortmentGeneralMethod.Method_Change_Type = eChangeType.update;
		//            btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        HandleException(ex, "btnProcess_Click");
		//    }
		//}

		private void txtReserve_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
				}

				string inStr = ((TextBox)sender).Text.ToString(CultureInfo.CurrentUICulture).Trim();
				
				if (inStr == string.Empty) 
				{
					radPercent.Checked = false;
					radPercent.Enabled = false;
					radUnits.Checked = false;
					radUnits.Enabled = false;
					
					((TextBox)sender).Focus();

					return;
				}	
				else
				{	
					radPercent.Enabled = true; 
					radPercent.TabStop = true; 
					radUnits.Enabled = true;
					radUnits.TabStop = true;

					if (!radPercent.Checked && !radUnits.Checked)
						radPercent.Checked = true;
				}
			}
			catch
			{
				MessageBox.Show("Please enter a positive floating point number");
				((TextBox)sender).Text = string.Empty;
				((TextBox)sender).Focus();
				return;
			}
		}


		private void RadioButtonClick_Click(object sender, System.EventArgs e)
		{
			if (FormLoaded)
				ChangePending = true;
		}


		private void menuGrid_Popup(object sender, System.EventArgs e)
		{
			
		}

		private void menuItemDelete_Click(object sender, System.EventArgs e)
		{
			_mouseDownGrid.DeleteSelectedRows(true);
		}

		private void menuItemInsert_Click(object sender, System.EventArgs e)
		{
			_mouseDownGrid.DisplayLayout.Bands[0].AddNew();
		}

		private void gridMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			UltraGrid grid;

			try
			{
				if (sender.GetType() == typeof(UltraGrid))
				{
					grid = (UltraGrid)sender;

					if (e.Button == MouseButtons.Right)
					{
						_mouseDownGrid = grid;
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}


		public void HandleExceptions(Exception exc)
		{
			Debug.WriteLine(exc.ToString());
			MessageBox.Show(exc.ToString());
		}

		///// <summary>
		///// Even though only one variable type was requested in the method (_variableType).
		///// All three (Sales, Stock, and Receipts) are figured out and saved.
		///// </summary>
		///// <param name="aSAB"></param>
		///// <param name="aApplicationTransaction"></param>
		///// <param name="aStoreFilterRID"></param>
		///// <param name="asp"></param>
		///// <param name="hierNodeList"></param>
		///// <param name="versionList"></param>
		///// <param name="dateRangeList"></param>
		///// <param name="weightList"></param>
		//private void ProcessVariables(
		//    SessionAddressBlock aSAB,
		//    ApplicationSessionTransaction aApplicationTransaction,
		//    AssortmentProfile asp,
		//    List<int> hierNodeList,
		//    List<int> versionList,
		//    List<int> dateRangeList,
		//    List<double> weightList
		//    )
		//{
		//    try
		//    {
		//        //=================
		//        // Fill Store List
		//        //=================
		//        FillStoreList(aSAB, aApplicationTransaction, hierNodeList);

		//        //===================================
		//        // read basis store values and total
		//        //===================================
		//        _basisReader = new AssortmentBasisReader(aSAB, aApplicationTransaction,
		//            hierNodeList, versionList, dateRangeList, weightList, _asp.AssortmentStoreGroupRID,
		//            _asp.AssortmentIncludeSimilarStores, _asp.AssortmentIncludeIntransit, _asp.AssortmentIncludeOnhand,
		//            _asp.AssortmentIncludeCommitted, _storeProfileList);

		//        //===================================================================================
		//        // Even though only one variable type was requested in the method (_variableType).
		//        // All three (Sales, Stock, & Receipts) are figured out and saved.
		//        //===================================================================================
		//        asp.ClearTotalAssortment();

		//        eAssortmentVariableType tempVarType = eAssortmentVariableType.Sales;
		//        int variableNumber = ReadBasisVariable(aSAB, tempVarType);
		//        asp.SetAssortmentStoreSummary(asp.HeaderRID, variableNumber, _storeList, _storeSetList, _storeIndexList, _storeValueList, _avgStoreList,
		//                        _storeIntransitList, _storeNeedList, _storePctNeedList);

		//        tempVarType = eAssortmentVariableType.Stock;
		//        variableNumber = ReadBasisVariable(aSAB, tempVarType);
		//        asp.SetAssortmentStoreSummary(asp.HeaderRID, variableNumber, _storeList, _storeSetList, _storeIndexList, _storeValueList, _avgStoreList,
		//                        _storeIntransitList, _storeNeedList, _storePctNeedList);

		//        tempVarType = eAssortmentVariableType.Receipts;
		//        variableNumber = ReadBasisVariable(aSAB, tempVarType);
		//        asp.SetAssortmentStoreSummary(asp.HeaderRID, variableNumber, _storeList, _storeSetList, _storeIndexList, _storeValueList, _avgStoreList,
		//                        _storeIntransitList, _storeNeedList, _storePctNeedList);

		//        //=======================================================================================
		//        // Rebuilds the assortment summary from the total assortment data just set by the method
		//        //=======================================================================================
		//        asp.RebuildAssortmentSummary();

		//        //DataTable ddt = asp.GetSummaryInformation(SG_RID);
		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//}

		private void FillStoreList(SessionAddressBlock aSAB, ApplicationSessionTransaction aApplicationTransaction, List<int> hierNodeList)
		{
			try
			{
				_storeList.Clear();				
                _storeProfileList = StoreMgmt.StoreProfiles_GetActiveStoresList(); //aSAB.StoreServerSession.GetActiveStoresList();
				
				// Place store keys in a list for later use.
				for (int i = 0; i < _storeProfileList.Count; i++)
				{
					StoreProfile store = (StoreProfile)_storeProfileList[i];
					_storeList.Add(store.Key);
					_storeToListHash.Add(store.Key, i);
				}
			}
			catch
			{
				throw;
			}
		}

		//private int ReadBasisVariable(SessionAddressBlock sab, eAssortmentVariableType variableType)
		//{
		//    try
		//    {
		//        //==========================================
		//        // Init Store Lists to get ready to fill
		//        //==========================================
		//        InitStoreLists();
		//        double totalUnits = _basisReader.GetBasisTotalUnits(variableType);
		//        _storeValueList = _basisReader.GetBasisStoreUnits(variableType);
		//        int variableNumber = _basisReader.GetVariableNumber(variableType);
		//        //==============================================================================
		//        // Update Gen Assortment Method's variable number, if it's the variable chosen
		//        // on the method.
		//        //==============================================================================
		//        if (_asp.AssortmentVariableType == variableType)
		//            _asp.AssortmentVariableNumber = variableNumber;

		//        //====================
		//        // Get Intransit
		//        //====================
		//        double totalIntransit = _basisReader.GetPlanActualTotalUnits(eAssortmentVariableType.Intransit);
		//        _storeIntransitList = _basisReader.GetPlanActualStoreUnits(eAssortmentVariableType.Intransit);

		//        //====================
		//        // Get need
		//        //====================
		//        double totalNeed = _basisReader.GetPlanActualTotalUnits(eAssortmentVariableType.Need);
		//        _storeNeedList = _basisReader.GetPlanActualStoreUnits(eAssortmentVariableType.Need);


		//        if (variableType == eAssortmentVariableType.Stock ||
		//            variableType == eAssortmentVariableType.Receipts)
		//        {
		//            //============================
		//            // Include Intransit in value
		//            //============================
		//            if (_asp.AssortmentIncludeIntransit)
		//            {
		//                totalUnits += totalIntransit;
		//                // Add list to _storeValueList
		//                for (int i = 0; i < _storeValueList.Count; i++)
		//                {
		//                    _storeValueList[i] += _storeIntransitList[i];
		//                }
		//            }
		//            //=========================
		//            // Include OnHand in value
		//            //=========================
		//            if (_asp.AssortmentIncludeOnhand)
		//            {
		//                double totalOnHand = _basisReader.GetPlanActualTotalUnits(eAssortmentVariableType.Onhand);
		//                List<int> storeOnHandList = _basisReader.GetPlanActualStoreUnits(eAssortmentVariableType.Onhand);
		//                totalUnits += totalOnHand;
		//                // Add list to _storeValueList
		//                for (int i = 0; i < _storeValueList.Count; i++)
		//                {
		//                    _storeValueList[i] += storeOnHandList[i];
		//                }
		//            }
		//        }

		//        _setTotalHash.Clear();
		//        _setAvgHash.Clear();
		//        _setStoreCountHash.Clear();

		//        StoreGroupProfile sgp = sab.StoreServerSession.GetStoreGroupFilled(_asp.AssortmentStoreGroupRID);
		//        if (_asp.AssortmentAverageBy == eStoreAverageBy.Set)
		//        {
		//            foreach (StoreGroupLevelProfile sglp in sgp.GroupLevels.ArrayList)
		//            {
		//                double storeAvg = CalcStoreAverage(sglp.Key, sglp.Stores, _storeValueList);
		//                CalcStoreIndex(sglp.Key, sglp.Stores, _storeValueList, storeAvg);
		//            }
		//            //DebugSetTotals();
		//        }
		//        else
		//        {
		//            double storeAvg = CalcStoreAverage(1, _storeProfileList, _storeValueList);
		//            foreach (StoreGroupLevelProfile sglp in sgp.GroupLevels.ArrayList)
		//            {
		//                CalcStoreIndex(sglp.Key, sglp.Stores, _storeValueList, storeAvg);
		//            }

		//            //DebugSetTotals();
		//        }

		//        return variableNumber;
		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//}

		//private void CalcStoreIndex(int setKey, ProfileList storeList, List<int> valueList, double storeAvg)
		//{
		//    try
		//    {
		//        //=====================
		//        // Calc Store Indexes
		//        //=====================
		//        foreach (StoreProfile store in storeList.ArrayList)
		//        {
		//            if (_storeToListHash.ContainsKey(store.Key))
		//            {
		//                int idx = (int)_storeToListHash[store.Key];
		//                // Save store set
		//                _storeSetList[idx] = setKey;
		//                double storeIndex = 0.0d;
		//                if (storeAvg != 0.0d)
		//                {
		//                    storeIndex = (valueList[idx] * 100) / storeAvg;
		//                }
		//                _storeIndexList[idx] = (int)storeIndex;
		//                _avgStoreList[idx] = (int)storeAvg;
		//            }
		//        }
		//        _setAvgHash.Add(setKey, storeAvg);
		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//}

		//private double CalcStoreAverage(int setKey, ProfileList storeList, List<int> valueList)
		//{
		//    try
		//    {
		//        //====================
		//        // Calc Average Store
		//        //====================
		//        double totalAmt = 0;
		//        double storeCount = 0;
		//        double avg = 0;
		//        foreach (StoreProfile store in storeList.ArrayList)
		//        {
		//            //==========================================================================
		//            // If all the stores in a set is sent to this method, some of the stores
		//            // may not be in out filtered list. We check first thing to see if the
		//            // store is in our list.
		//            //==========================================================================
		//            if (_storeToListHash.ContainsKey(store.Key))
		//            {
		//                int idx = (int)_storeToListHash[store.Key];
		//                //if (valueList[idx] > 0)
		//                //{
		//                totalAmt += valueList[idx];
		//                storeCount++;
		//                //}
		//            }
		//        }
		//        if (storeCount > 0)
		//        {
		//            avg = (int)((totalAmt / storeCount) + .5);
		//        }

		//        _setTotalHash.Add(setKey, (int)totalAmt);
		//        _setStoreCountHash.Add(setKey, (int)storeCount);

		//        return avg;
		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//}

		private void cboStoreAttribute_SelectionChangeCommitted_1(object sender, EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}
        //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
        void cboStoreAttribute_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboStoreAttribute_SelectionChangeCommitted_1(this, new EventArgs());
        }
        //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

		override protected void NextWindow(eAllocationSelectionViewType viewType)
		{
			try
			{
				//if (EAB.AssortmentWorkspaceExplorer.GetSelectedHeaders().Count > 0)
				//{
				//    foreach (int key in _selectedHeaderKeyList)
				//    {
				//        if (key < 1)
				//        {
				//            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HeadersChanged));
				//            return;
				//        }
				//    }
				//}

				//======================================================================================================================
				// When a first time user accesses assortment, no assortment filter has been created yet. This creates the filter, so 
				// later the transaction knows to creat assortment profiles instead of allocation Profiles.
				//======================================================================================================================
                //AssortmentWorkspaceFilterProfile assrtWorkFilterProfile = new AssortmentWorkspaceFilterProfile(SAB.ClientServerSession.UserRID); //TT#1313-MD -jsobek -Header Filters -do not re-filter in the AWS
                //assrtWorkFilterProfile.WriteFilter(); //TT#1313-MD -jsobek -Header Filters -do not re-filter in the AWS

				// BEGIN TT#343-MD - stodd - When creating a new Assortment it is set as Read Only
				//ApplicationSessionTransaction processTransaction = NewTransFromSelectedHeaders();


				if (_transaction == null)
				{
					_transaction = NewTransFromSelectedHeaders();
				}
				else
				{
					AddSelectedHeadersToTrans(_transaction);
				}

				//AssortmentViewSelection avs = new AssortmentViewSelection(EAB, SAB, processTransaction, null, false);
				//if (this.ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer")
				//{
				//    avs.MdiParent = this.ParentForm;
				//}
				//else
				//{
				//    avs.MdiParent = this.ParentForm.Owner;
				//}

                Process();	// TT#831-MD - stodd - Need, Need%, & VSW Onhand incorrect 

				//System.EventArgs args = new EventArgs();
				Cursor.Current = Cursors.WaitCursor;
				_transaction.CreateAssortmentViewSelectionCriteria();
				// Begin TT#854 - MD - stodd - enqueue error
				// Added "true". Otherwise the method would load headers that are never used.
				_transaction.CreateAllocationViewSelectionCriteria(true);
				// END TT#854 - MD - stodd - enqueue error

				_transaction.NewAssortmentCriteriaHeaderList();

				AllocationHeaderProfileList headerList = (AllocationHeaderProfileList)_transaction.GetMasterProfileList(eProfileType.AllocationHeader);

				if (_transaction.AssortmentStoreAttributeRid == Include.NoRID)
                {
					_transaction.AssortmentStoreAttributeRid = SAB.ClientServerSession.GlobalOptions.AllocationStoreGroupRID;
                }
				if (_transaction.AllocationStoreAttributeID == Include.NoRID)
                {
					_transaction.AllocationStoreAttributeID = SAB.ClientServerSession.GlobalOptions.AllocationStoreGroupRID;
                } 
				// END TT#343-MD - stodd - When creating a new Assortment it is set as Read Only 
				try
				{
					MIDFormBase frm = null;
					System.Windows.Forms.Form parentForm;
					System.ComponentModel.CancelEventArgs args = new CancelEventArgs();


					//if (!_trans.VelocityCriteriaExists)
					//{
					//    AllocationSubtotalProfileList subtotalList = (AllocationSubtotalProfileList)_trans.GetMasterProfileList(eProfileType.AllocationSubtotal);
					//    if (subtotalList != null)
					//    {
					//        foreach (AllocationSubtotalProfile asp in subtotalList)
					//        {
					//            asp.RemoveAllSubtotalMembers();
					//        }
					//        _trans.RemoveAllocationSubtotalProfileList();
					//    }

					// BEGIN TT#343-MD - stodd - When creating a new Assortment it is set as Read Only
					//_transaction.SetCriteriaHeaderList(headerList);	// TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
					if (CheckSecurityEnqueue(_transaction, headerList))
					// END TT#343-MD - stodd - When creating a new Assortment it is set as Read Only
                    {
                        // Close this form
                        parentForm = this.MdiParent;
                        this.Close();
                        this.MdiParent = null;

                        try
                        {
							// BEGIN TT#343-MD - stodd - When creating a new Assortment it is set as Read Only
							_transaction.AssortmentViewLoadedFrom = eAssortmentBasisLoadedFrom.AssortmentProperties;
							frm = new MIDRetail.Windows.AssortmentView(EAB, _transaction, eAssortmentWindowType.Assortment);
							// END TT#343-MD - stodd - When creating a new Assortment it is set as Read Only
                            ((AssortmentView)frm).Initialize();

                            frm.MdiParent = parentForm;
                            //frm.WindowState = FormWindowState.Maximized;  // Begin TT#441-MD - RMatelic - Placeholder fields are not editable >>> unrelated; move maximize to after Show()
                            frm.Show();
                            frm.WindowState = FormWindowState.Maximized;    // End TT#441-MD 
                            if (frm.ExceptionCaught)
                            {
                                frm.Close();
                                frm.MdiParent = null;
                            }
                        }
                        catch (Exception ex)
                        {
                            frm.Close();
                            frm.MdiParent = null;
                            HandleException(ex, frm.Name);
                        }
                    }
				}
				catch (Exception ex)
				{
					// BEGIN TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
					// This was hiding exceptions
					if (!this.IsDisposed)
					{
						HandleException(ex);
					}
					// END TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
				}
			}
			catch (Exception ex)
			{
				// BEGIN TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
				// This was hiding exception
				if (!this.IsDisposed)
				{
					HandleException(ex);
				}
				// END TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private ApplicationSessionTransaction NewTransFromSelectedHeaders()
		{
			try
			{
				ApplicationSessionTransaction newTrans = SAB.ApplicationServerSession.CreateTransaction();
				// BEGIN TT#343-MD - stodd - When creating a new Assortment it is set as Read Only
				AddSelectedHeadersToTrans(newTrans);

				return newTrans;
				// END TT#343-MD - stodd - When creating a new Assortment it is set as Read Only
			}
			catch
			{
				throw;
			}
		}

		// BEGIN TT#343-MD - stodd - When creating a new Assortment it is set as Read Only
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
                _selectedAssortmentKeyList.Clear();  // TT#488 - MD -  Jellis - Group Allocation

				GetAllHeadersInAssortment(_asp.Key);

				int[] selectedHeaderArray = new int[_selectedHeaderKeyList.Count];
				_selectedHeaderKeyList.CopyTo(selectedHeaderArray);
                int[] selectedAssortmentArray = new int[_selectedAssortmentKeyList.Count]; // TT#488 - MD - Jellis - Group Allocation
                _selectedAssortmentKeyList.CopyTo(selectedAssortmentArray);                 // TT#488 - MD - Jellis - Group Allocation

				// BEGIN TT#66-MD - stodd - values not saving to Allocation profile
				string enqMessage = string.Empty;
                List<int> hdrList = new List<int>(selectedAssortmentArray); // TT#488 - MD - Jellis - Group Allocation
                hdrList.AddRange(selectedHeaderArray);                      // TT#488 - MD - Jellis - Group Allocation
                //List<int> hdrList = new List<int>(selectedHeaderArray);   // TT#488 - MD - Jellis - Group Allocation
				bool success = aTrans.EnqueueHeaders(hdrList, out enqMessage);
				// END TT#66-MD - stodd - 

				// load the selected headers in the Application session transaction
                // begin TT#488 - MD - Jellis - Group Allocation
                //aTrans.LoadAssortmentMemberHeaders(selectedHeaderArray);	// TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
                aTrans.CreateMasterAssortmentMemberListFromSelectedHeaders(selectedAssortmentArray, selectedHeaderArray);
                // end TT#488 - MD - Jellis - Group Allocation
			}
			catch
			{
				throw;
			}
		}
		// END TT#343-MD - stodd - When creating a new Assortment it is set as Read Only
		

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
                    if (hdrRID != aAsrtRID)  // TT#488 - MD - Jellis - Group Allocation
                    {
                        _selectedHeaderKeyList.Add(hdrRID);
                        // begin TT#488 - MD - Jellis - Group Allocation
                    }                        
                    else
                    {
                        _selectedAssortmentKeyList.Add(hdrRID);
                    }
                    // end TT#488 - MD - Jellis - Group Allocation
				}
			}
			catch
			{
				throw;
			}
		}

		private void frmAssortmentProperties_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

        // BEGIN TT#1979-MD - AGallagher - Asst Properties_ Current On hand and Intransit_when Active inconsistent
        private void radStock_CheckedChanged(object sender, EventArgs e)
        {
            if ((this.radStock.Checked == true) && (this.radStock.Focused == true))
            {
                cbxOnhand.Enabled = true;
                cbxIntransit.Enabled = true;
            }
         }

        private void radSales_CheckedChanged(object sender, EventArgs e)
        {
            if ((this.radSales.Checked == true) && (this.radSales.Focused == true))
            {
                cbxOnhand.Checked = false;
                cbxOnhand.Enabled = false;
                cbxIntransit.Checked = false;
                cbxIntransit.Enabled = false;
            }
        }

        private void radReceipts_CheckedChanged(object sender, EventArgs e)
        {
            if ((this.radReceipts.Checked == true) && (this.radReceipts.Focused == true))
			{
            cbxOnhand.Checked = false;
            cbxOnhand.Enabled = false;
            cbxIntransit.Checked = false;
            cbxIntransit.Enabled = false;
            }
        }

        // Begin TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
        private void beginDayDateRangeSelector_OnSelection(object source, DateRangeSelectorEventArgs e)
        {
            try
            {
                if (FormLoaded)
                    ChangePending = true;

                DateRangeProfile dr = e.SelectedDateRange;

                if (dr != null)
                {
                    beginDayDateRangeSelector.Text = dr.DisplayDate;

                    //Add RID to Control's Tag (for later use)
                    int lAddTag = dr.Key;

                    beginDayDateRangeSelector.Tag = lAddTag;
                    beginDayDateRangeSelector.DateRangeRID = lAddTag;

                    //Display Dynamic picture or not
                    if (dr.DateRangeType == eCalendarRangeType.Dynamic)
                        beginDayDateRangeSelector.SetImage(this.DynamicToCurrentImage);
                    else
                        beginDayDateRangeSelector.SetImage(null);
                    //=========================================================
                    // Override the image if this is a dynamic switch date.
                    //=========================================================
                    if (dr.IsDynamicSwitch)
                        beginDayDateRangeSelector.SetImage(this.DynamicSwitchImage);

                    BeginDayDateRangeRid = dr.Key;	
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void beginDayDateRangeSelector_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                // tells the date range selector the currently selected date range RID
                if (beginDayDateRangeSelector.Tag != null)
                    ((CalendarDateSelector)beginDayDateRangeSelector.DateRangeForm).DateRangeRID = (int)beginDayDateRangeSelector.Tag;
                // tells the control to show the date range selector form
                beginDayDateRangeSelector.ShowSelector();
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void beginDayDateRangeSelector_Load(object sender, EventArgs e)
        {
            try
            {
                CalendarDateSelector frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[] { SAB });
                beginDayDateRangeSelector.DateRangeForm = frm;
                frm.DateRangeRID = beginDayDateRangeSelector.DateRangeRID;
                frm.AllowDynamicToPlan = false;
                frm.AllowDynamicToStoreOpen = false;
                frm.AllowDynamicSwitch = false; 
                frm.RestrictToOnlyWeeks = true;
                frm.RestrictToSingleDate = true;
                frm.DefaultToStatic = true; 
                frm.OverrideNullAnchorDateDefaults = true; 
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }
        // End TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
	}
    // END TT#1979-MD - AGallagher - Asst Properties_ Current On hand and Intransit_when Active inconsistent
	public class AssortmentPropertiesChangeEventArgs : EventArgs
	{
		private MIDTreeNode _parentNode;
		// BEGIN TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
		//private AssortmentProfile _assortmentProf;
		private AssortmentHeaderProfile _ahp;

		//public AssortmentPropertiesChangeEventArgs(MIDTreeNode aParentNode, AssortmentProfile aAssortmentProf)
		public AssortmentPropertiesChangeEventArgs(MIDTreeNode aParentNode, AssortmentHeaderProfile ahp)
		{
			_parentNode = aParentNode;
			//_assortmentProf = aAssortmentProf;
			_ahp = ahp;
		}

		public MIDTreeNode ParentNode
		{
			get
			{
				return _parentNode;
			}
		}

		//public AssortmentProfile AssortmentProfile
		public AssortmentHeaderProfile AssortmentHeaderProfile
		{
			get
			{
				return _ahp;
			}
		}
		// END TT#365-MD - stodd - In Assrt explorer change node profile to an AllocationHeaderProfile
	}

	public class AssortmentPropertiesCloseEventArgs : EventArgs
	{
	}

	
}
