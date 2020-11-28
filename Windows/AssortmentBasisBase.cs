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
	/// Summary description for GeneralAssortmentMethod.
	/// </summary>
	public class AssortmentBasisBase : MIDFormBase
	{
		//public delegate void AssortmentPropertiesSaveEventHandler(object source, AssortmentPropertiesSaveEventArgs e);
		//public event AssortmentPropertiesSaveEventHandler OnAssortmentPropertiesSaveHandler;

		//public delegate void AssortmentPropertiesCloseEventHandler(object source, AssortmentPropertiesCloseEventArgs e);
		//public event AssortmentPropertiesCloseEventHandler OnAssortmentPropertiesCloseHandler;

		private System.ComponentModel.IContainer components;
		//private MIDWorkflowMethodTreeNode _explorerNode = null;
		//private GeneralAssortmentMethod _assortmentGeneralMethod;
		//private ProfileList _storeGroupList;
		private System.Windows.Forms.ImageList Icons;
		private DataTable _dtBasis;
        private DataTable _dtStoreGrades;
        private ProfileList _versionProfList;
		private Image _dynamicToPlanImage;
		private Image _dynamicToCurrentImage;
		private bool _dragAndDrop = false;
		private bool _merchValChanged = false;
		protected System.Windows.Forms.ContextMenu menuGrid;
		private System.Windows.Forms.MenuItem menuItemDelete;
		private System.Windows.Forms.MenuItem menuItemInsert;
		private int _nodeRid;
		private UltraGrid _mouseDownGrid;
		protected MIDAttributeComboBox cboStoreAttribute;
		protected Label lblAttribute;
		protected CheckBox cbxSimilarStores;
		protected CheckBox cbxIntransit;
		protected CheckBox cbxOnhand;
		protected TabControl tabControl2;
		private TabPage tabBasis;
		protected Panel panel2;
		protected RadioButton radStock;
		protected RadioButton radSales;
		protected RadioButton radReceipts;
		protected Label lblVariable;
		private UltraGrid gridBasis;
		private TabPage tabStoreGrades;
		protected GroupBox gbAverage;
		protected RadioButton radAllStore;
		protected RadioButton radSet;
		protected GroupBox groupBox4;
		protected RadioButton radUnitsBoundary;
		protected RadioButton radIndexBoundary;
		private UltraGrid gridStoreGrades;
		protected CheckBox cbxCommitted;
		protected Button btOk;
		protected Button btnCancel;
		//private StoreGradeList _storeGradeList;
		private ApplicationSessionTransaction _trans;

		private AssortmentProfile _asp;
		private bool _readOnly;
		private eChangeType _changeType;
		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		//private AssortmentBasisReader _basisReader;
		//End TT#2 - JScott - Assortment Planning - Phase 2
		//private int _variableNumber;
		private bool _inDesigner = false;
		private eGradesLoadedFrom _gradesLoadedFrom = eGradesLoadedFrom.None;
		private int _hnRidGradesLoadedFrom = Include.NoRID;
		private eAssortmentBasisLoadedFrom _loadedFrom;

		// All the stores in these lists line up.
		private ProfileList _storeProfileList;
		private List<int> _storeList = new List<int>();
		private List<int> _storeValueList = new List<int>();
		private List<int> _storeSetList = new List<int>();
		private List<int> _storeIndexList = new List<int>();
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

		//=========================================================
		// Work area for controls on this form.
		// Loaded either from a single assortment profile or from 
		// the user assortment critera.
		//=========================================================
		private int _key;
		private int _storeAttributeRid;
		private eAssortmentVariableType _variableType;
		private int _variableNumber;
		private bool _inclOnhand;
		private bool _inclIntransit;
		private bool _inclSimStores;
		private bool _inclCommitted;
		private eStoreAverageBy _averageBy;
		private eGradeBoundary _gradeBoundary;

		private List<AssortmentBasis> _basisList;
        //private ProfileList _storeGradeList;          // TT#488 - MD - Jellis - Group Allocaton
		private ProfileList _assortmentStoreGradeList;	// TT#488-MD - STodd - Group Allocation 
		private ExplorerAddressBlock _eab;
		private ArrayList _selectedHeaderKeyList;
        private ArrayList _selectedAssortmentKeyList; // TT#488 - MD - Jellis - Group Allocation
		private int _anchorNodeRid;
		// Begin TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
        private int _anchorDateRangeRid = Include.UndefinedCalendarDateRange;
        private DateRangeProfile _anchorDateRangeProfile;
       // End TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
        // Begin TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
        private int _beginDayDateRangeRid = Include.UndefinedCalendarDateRange;
        private DateRangeProfile _beginDayDateRangeProfile;
        // End TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
        private FunctionSecurityProfile _assortmentReview;
        private FunctionSecurityProfile _assortmentReviewAssortment;
        private FunctionSecurityProfile _assortmentReviewContent;
        private FunctionSecurityProfile _assortmentReviewCharacteristic;

		// BEGIN TT#207-MD - stodd - Renaming assortment doesn't rename placeholders
		private bool _assortmentIdChanged = false;
		// END TT#207-MD - stodd - Renaming assortment doesn't rename placeholders
		private bool _okClicked = false;	// TT#508 - md - stodd - enqueue error
        private bool _assortmentBasisChanged = false;   // TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO

		//=========================
		// Constructors
		//=========================

		public AssortmentBasisBase()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			_inDesigner = true;
            // Begin TT#1725 - RMatelic - "Hide" Committed
            cbxCommitted.Visible = false;
            // End TT#1725
		}

		public AssortmentBasisBase(
			SessionAddressBlock aSAB,
			ExplorerAddressBlock aEAB,
			AssortmentProfile ap,
			bool aReadOnly)
			: base(aSAB)
		{
			try
			{
				_eab = aEAB;
				_asp = ap;
				_readOnly = aReadOnly;
				_trans = null;
				_loadedFrom = eAssortmentBasisLoadedFrom.AssortmentProperties;
				AllowDragDrop = true;
				InitializeComponent();
                // Begin TT#1725 - RMatelic - "Hide" Committed
                cbxCommitted.Visible = false;
                // End TT#1725
			}
			catch
			{
				throw;
			}
		}

		public AssortmentBasisBase(
			SessionAddressBlock aSAB,
			ExplorerAddressBlock aEAB,
			ApplicationSessionTransaction trans)
			: base(aSAB)
		{
			try
			{
				_eab = aEAB;
				_trans = trans;
				_asp = null;
				_readOnly = false;
				_loadedFrom = eAssortmentBasisLoadedFrom.UserSelectionCriteria;
				AllowDragDrop = true;
				InitializeComponent();
                // Begin TT#1725 - RMatelic - "Hide" Committed
                cbxCommitted.Visible = false;
                // End TT#1725
			}
			catch
			{
				throw;
			}
		}

		//=========================
		// Properties
		//=========================
        // Begin TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO
        public AssortmentProfile AssortmentProfile
		{
			get { return _asp; }
			set { _asp = value; }
		}
        // End TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO

		public Image DynamicToPlanImage
		{
			get { return _dynamicToPlanImage; }
			set { _dynamicToPlanImage = value; }
		}

		public Image DynamicToCurrentImage
		{
			get { return _dynamicToCurrentImage; }
			set { _dynamicToCurrentImage = value; }
		}

		public eGradesLoadedFrom GradesLoadedFrom
		{
			get { return _gradesLoadedFrom; }
			set { _gradesLoadedFrom = value; }
		}

		public List<AssortmentBasis> BasisList
		{
			get { return _basisList; }
			set { _basisList = value; }
		}

		public ExplorerAddressBlock EAB
		{
			get { return _eab; }
			set { _eab = value; }
		}

		public int AnchorNodeRid
		{
			get { return _anchorNodeRid; }
			set { _anchorNodeRid = value; }
		}

		// Begin TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
        public int AnchorDateRangeRid
        {
            get { return _anchorDateRangeRid; }
            set 
            {
                _anchorDateRangeRid = value;
                _anchorDateRangeProfile = SAB.ApplicationServerSession.Calendar.GetDateRange(_anchorDateRangeRid);
            }
        }
		// End TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 

        // Begin TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
        public int BeginDayDateRangeRid
        {
            get { return _beginDayDateRangeRid; }
            set
            {
                _beginDayDateRangeRid = value;
                _beginDayDateRangeProfile = SAB.ApplicationServerSession.Calendar.GetDateRange(_anchorDateRangeRid);
            }
        }
        // End TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working

		// BEGIN TT#207-MD - stodd - Renaming assortment doesn't rename placeholders
		public bool AssortmentIdChanged
		{
			get { return _assortmentIdChanged; }
			set { _assortmentIdChanged = value; }
		}
		// END TT#207-MD - stodd - Renaming assortment doesn't rename placeholders

		// BEGIN TT#508-MD - stodd - enqueue error
		public bool OkClicked
		{
			get { return _okClicked; }
			set { _okClicked = value; }
		}
		// END TT#508-MD - stodd - enqueue error

        // Begin TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO
        public bool AssortmentBasisChanged
        {
            get { return _assortmentBasisChanged; }
        }
        // End TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO

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
                this.radStock.CheckedChanged -= new System.EventHandler(this.radStock_CheckedChanged);
                this.radSales.CheckedChanged -= new System.EventHandler(this.radSales_CheckedChanged);
                this.radReceipts.CheckedChanged -= new System.EventHandler(this.radReceipts_CheckedChanged);
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
            this.menuGrid = new System.Windows.Forms.ContextMenu();
            this.menuItemDelete = new System.Windows.Forms.MenuItem();
            this.menuItemInsert = new System.Windows.Forms.MenuItem();
            this.Icons = new System.Windows.Forms.ImageList(this.components);
            this.cbxCommitted = new System.Windows.Forms.CheckBox();
            this.lblAttribute = new System.Windows.Forms.Label();
            this.cbxSimilarStores = new System.Windows.Forms.CheckBox();
            this.cbxIntransit = new System.Windows.Forms.CheckBox();
            this.cbxOnhand = new System.Windows.Forms.CheckBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabBasis = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radStock = new System.Windows.Forms.RadioButton();
            this.radSales = new System.Windows.Forms.RadioButton();
            this.radReceipts = new System.Windows.Forms.RadioButton();
            this.lblVariable = new System.Windows.Forms.Label();
            this.gridBasis = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tabStoreGrades = new System.Windows.Forms.TabPage();
            this.gbAverage = new System.Windows.Forms.GroupBox();
            this.radAllStore = new System.Windows.Forms.RadioButton();
            this.radSet = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radUnitsBoundary = new System.Windows.Forms.RadioButton();
            this.radIndexBoundary = new System.Windows.Forms.RadioButton();
            this.gridStoreGrades = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cboStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.tabControl2.SuspendLayout();
            this.tabBasis.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBasis)).BeginInit();
            this.tabStoreGrades.SuspendLayout();
            this.gbAverage.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridStoreGrades)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // menuGrid
            // 
            this.menuGrid.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemDelete,
            this.menuItemInsert});
            this.menuGrid.Popup += new System.EventHandler(this.menuGrid_Popup);
            // 
            // menuItemDelete
            // 
            this.menuItemDelete.Index = 0;
            this.menuItemDelete.Text = "Delete";
            this.menuItemDelete.Click += new System.EventHandler(this.menuItemDelete_Click);
            // 
            // menuItemInsert
            // 
            this.menuItemInsert.Index = 1;
            this.menuItemInsert.Text = "Insert";
            this.menuItemInsert.Click += new System.EventHandler(this.menuItemInsert_Click);
            // 
            // Icons
            // 
            this.Icons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.Icons.ImageSize = new System.Drawing.Size(16, 16);
            this.Icons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // cbxCommitted
            // 
            this.cbxCommitted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxCommitted.AutoSize = true;
            this.cbxCommitted.Location = new System.Drawing.Point(494, 342);
            this.cbxCommitted.Name = "cbxCommitted";
            this.cbxCommitted.Size = new System.Drawing.Size(75, 17);
            this.cbxCommitted.TabIndex = 14;
            this.cbxCommitted.Text = "Committed";
            this.cbxCommitted.UseVisualStyleBackColor = true;
            this.cbxCommitted.CheckedChanged += new System.EventHandler(this.cbxCommitted_CheckedChanged);
            // 
            // lblAttribute
            // 
            this.lblAttribute.Location = new System.Drawing.Point(17, 54);
            this.lblAttribute.Name = "lblAttribute";
            this.lblAttribute.Size = new System.Drawing.Size(53, 17);
            this.lblAttribute.TabIndex = 11;
            this.lblAttribute.Text = "Attribute:";
            // 
            // cbxSimilarStores
            // 
            this.cbxSimilarStores.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxSimilarStores.Location = new System.Drawing.Point(361, 340);
            this.cbxSimilarStores.Name = "cbxSimilarStores";
            this.cbxSimilarStores.Size = new System.Drawing.Size(90, 19);
            this.cbxSimilarStores.TabIndex = 9;
            this.cbxSimilarStores.Text = "Similar Stores";
            this.cbxSimilarStores.Click += new System.EventHandler(this.cbxSimilarStores_Click);
            // 
            // cbxIntransit
            // 
            this.cbxIntransit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxIntransit.Location = new System.Drawing.Point(254, 340);
            this.cbxIntransit.Name = "cbxIntransit";
            this.cbxIntransit.Size = new System.Drawing.Size(68, 19);
            this.cbxIntransit.TabIndex = 8;
            this.cbxIntransit.Text = "Intransit";
            this.cbxIntransit.Click += new System.EventHandler(this.cbxIntransit_Click);
            // 
            // cbxOnhand
            // 
            this.cbxOnhand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxOnhand.Location = new System.Drawing.Point(106, 340);
            this.cbxOnhand.Name = "cbxOnhand";
            this.cbxOnhand.Size = new System.Drawing.Size(109, 19);
            this.cbxOnhand.TabIndex = 7;
            this.cbxOnhand.Text = "Current On Hand";
            this.cbxOnhand.Click += new System.EventHandler(this.cbxOnhand_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.AllowDrop = true;
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabBasis);
            this.tabControl2.Controls.Add(this.tabStoreGrades);
            this.tabControl2.Location = new System.Drawing.Point(8, 101);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(676, 227);
            this.tabControl2.TabIndex = 0;
            this.tabControl2.DragEnter += new System.Windows.Forms.DragEventHandler(this.tabControl2_DragEnter);
            this.tabControl2.DragOver += new System.Windows.Forms.DragEventHandler(this.tabControl2_DragOver);
            // 
            // tabBasis
            // 
            this.tabBasis.Controls.Add(this.panel2);
            this.tabBasis.Controls.Add(this.gridBasis);
            this.tabBasis.Location = new System.Drawing.Point(4, 22);
            this.tabBasis.Name = "tabBasis";
            this.tabBasis.Size = new System.Drawing.Size(668, 201);
            this.tabBasis.TabIndex = 0;
            this.tabBasis.Text = "Basis";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.radStock);
            this.panel2.Controls.Add(this.radSales);
            this.panel2.Controls.Add(this.radReceipts);
            this.panel2.Controls.Add(this.lblVariable);
            this.panel2.Location = new System.Drawing.Point(13, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(493, 32);
            this.panel2.TabIndex = 1;
            // 
            // radStock
            // 
            this.radStock.Location = new System.Drawing.Point(229, 8);
            this.radStock.Name = "radStock";
            this.radStock.Size = new System.Drawing.Size(56, 15);
            this.radStock.TabIndex = 3;
            this.radStock.Text = "Stock";
            this.radStock.CheckedChanged += new System.EventHandler(this.radStock_CheckedChanged);
            this.radStock.Click += new System.EventHandler(this.RadioButtonClick_Click);
            // 
            // radSales
            // 
            this.radSales.Location = new System.Drawing.Point(150, 7);
            this.radSales.Name = "radSales";
            this.radSales.Size = new System.Drawing.Size(56, 15);
            this.radSales.TabIndex = 2;
            this.radSales.Text = "Sales";
            this.radSales.CheckedChanged += new System.EventHandler(this.radSales_CheckedChanged);
            this.radSales.Click += new System.EventHandler(this.RadioButtonClick_Click);
            // 
            // radReceipts
            // 
            this.radReceipts.Checked = true;
            this.radReceipts.Location = new System.Drawing.Point(61, 7);
            this.radReceipts.Name = "radReceipts";
            this.radReceipts.Size = new System.Drawing.Size(66, 15);
            this.radReceipts.TabIndex = 1;
            this.radReceipts.TabStop = true;
            this.radReceipts.Text = "Receipts";
            this.radReceipts.CheckedChanged += new System.EventHandler(this.radReceipts_CheckedChanged);
            this.radReceipts.Click += new System.EventHandler(this.RadioButtonClick_Click);
            // 
            // lblVariable
            // 
            this.lblVariable.Location = new System.Drawing.Point(6, 8);
            this.lblVariable.Name = "lblVariable";
            this.lblVariable.Size = new System.Drawing.Size(54, 14);
            this.lblVariable.TabIndex = 0;
            this.lblVariable.Text = "Variable:";
            // 
            // gridBasis
            // 
            this.gridBasis.AllowDrop = true;
            this.gridBasis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridBasis.ContextMenu = this.menuGrid;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.gridBasis.DisplayLayout.Appearance = appearance1;
            this.gridBasis.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.gridBasis.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.gridBasis.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridBasis.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.gridBasis.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.gridBasis.DisplayLayout.GroupByBox.Hidden = true;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridBasis.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.gridBasis.DisplayLayout.MaxColScrollRegions = 1;
            this.gridBasis.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gridBasis.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            this.gridBasis.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.gridBasis.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            this.gridBasis.DisplayLayout.Override.CardAreaAppearance = appearance6;
            appearance7.BorderColor = System.Drawing.Color.Silver;
            appearance7.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.gridBasis.DisplayLayout.Override.CellAppearance = appearance7;
            this.gridBasis.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.gridBasis.DisplayLayout.Override.CellPadding = 0;
            appearance8.BackColor = System.Drawing.SystemColors.Control;
            appearance8.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance8.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance8.BorderColor = System.Drawing.SystemColors.Window;
            this.gridBasis.DisplayLayout.Override.GroupByRowAppearance = appearance8;
            appearance9.TextHAlignAsString = "Left";
            this.gridBasis.DisplayLayout.Override.HeaderAppearance = appearance9;
            this.gridBasis.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.gridBasis.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance10.BackColor = System.Drawing.SystemColors.Window;
            appearance10.BorderColor = System.Drawing.Color.Silver;
            this.gridBasis.DisplayLayout.Override.RowAppearance = appearance10;
            this.gridBasis.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance11.BackColor = System.Drawing.SystemColors.ControlLight;
            this.gridBasis.DisplayLayout.Override.TemplateAddRowAppearance = appearance11;
            this.gridBasis.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.gridBasis.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.gridBasis.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.gridBasis.Location = new System.Drawing.Point(14, 41);
            this.gridBasis.Name = "gridBasis";
            this.gridBasis.Size = new System.Drawing.Size(641, 159);
            this.gridBasis.TabIndex = 0;
            this.gridBasis.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridBasis_AfterCellUpdate);
            this.gridBasis.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridBasis_InitializeLayout);
            this.gridBasis.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.gridBasis_AfterRowInsert);
            this.gridBasis.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridBasis_CellChange);
            this.gridBasis.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridBasis_ClickCellButton);
            this.gridBasis.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.gridBasis_MouseEnterElement);
            this.gridBasis.DragDrop += new System.Windows.Forms.DragEventHandler(this.gridBasis_DragDrop);
            this.gridBasis.DragEnter += new System.Windows.Forms.DragEventHandler(this.gridBasis_DragEnter);
            this.gridBasis.DragOver += new System.Windows.Forms.DragEventHandler(this.gridBasis_DragOver);
            this.gridBasis.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridMouseDown);
            // 
            // tabStoreGrades
            // 
            this.tabStoreGrades.Controls.Add(this.gbAverage);
            this.tabStoreGrades.Controls.Add(this.groupBox4);
            this.tabStoreGrades.Controls.Add(this.gridStoreGrades);
            this.tabStoreGrades.Location = new System.Drawing.Point(4, 22);
            this.tabStoreGrades.Name = "tabStoreGrades";
            this.tabStoreGrades.Size = new System.Drawing.Size(668, 201);
            this.tabStoreGrades.TabIndex = 1;
            this.tabStoreGrades.Text = "Store Grades";
            // 
            // gbAverage
            // 
            this.gbAverage.Controls.Add(this.radAllStore);
            this.gbAverage.Controls.Add(this.radSet);
            this.gbAverage.Location = new System.Drawing.Point(171, 6);
            this.gbAverage.Name = "gbAverage";
            this.gbAverage.Size = new System.Drawing.Size(145, 39);
            this.gbAverage.TabIndex = 7;
            this.gbAverage.TabStop = false;
            this.gbAverage.Text = "Average";
            // 
            // radAllStore
            // 
            this.radAllStore.Checked = true;
            this.radAllStore.Location = new System.Drawing.Point(8, 17);
            this.radAllStore.Name = "radAllStore";
            this.radAllStore.Size = new System.Drawing.Size(72, 16);
            this.radAllStore.TabIndex = 0;
            this.radAllStore.TabStop = true;
            this.radAllStore.Text = "All Stores";
            this.radAllStore.CheckedChanged += new System.EventHandler(this.RadioButtonClick_Click);
            // 
            // radSet
            // 
            this.radSet.Location = new System.Drawing.Point(91, 17);
            this.radSet.Name = "radSet";
            this.radSet.Size = new System.Drawing.Size(45, 17);
            this.radSet.TabIndex = 1;
            this.radSet.Text = "Set";
            this.radSet.CheckedChanged += new System.EventHandler(this.RadioButtonClick_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radUnitsBoundary);
            this.groupBox4.Controls.Add(this.radIndexBoundary);
            this.groupBox4.Location = new System.Drawing.Point(17, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(145, 39);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Grade Boundary";
            this.groupBox4.Visible = false;   // TT#1989-MD - AGallagher - Unit Boundaries are to be hidden
            // 
            // radUnitsBoundary
            // 
            this.radUnitsBoundary.Location = new System.Drawing.Point(73, 18);
            this.radUnitsBoundary.Name = "radUnitsBoundary";
            this.radUnitsBoundary.Size = new System.Drawing.Size(55, 15);
            this.radUnitsBoundary.TabIndex = 1;
            this.radUnitsBoundary.Text = "Units";
            this.radUnitsBoundary.CheckedChanged += new System.EventHandler(this.radUnitsBoundary_CheckedChanged);
            this.radUnitsBoundary.Click += new System.EventHandler(this.RadioButtonClick_Click);
            this.radUnitsBoundary.Visible = false;   // TT#1989-MD - AGallagher - Unit Boundaries are to be hidden 
            // 
            // radIndexBoundary
            // 
            this.radIndexBoundary.Checked = true;
            this.radIndexBoundary.Location = new System.Drawing.Point(11, 18);
            this.radIndexBoundary.Name = "radIndexBoundary";
            this.radIndexBoundary.Size = new System.Drawing.Size(55, 15);
            this.radIndexBoundary.TabIndex = 0;
            this.radIndexBoundary.TabStop = true;
            this.radIndexBoundary.Text = "Index";
            this.radIndexBoundary.CheckedChanged += new System.EventHandler(this.radIndexBoundary_CheckedChanged);
            this.radIndexBoundary.Click += new System.EventHandler(this.RadioButtonClick_Click);
            this.radIndexBoundary.Visible = false;   // TT#1989-MD - AGallagher - Unit Boundaries are to be hidden
            // 
            // gridStoreGrades
            // 
            this.gridStoreGrades.AllowDrop = true;
            this.gridStoreGrades.ContextMenu = this.menuGrid;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            appearance12.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.gridStoreGrades.DisplayLayout.Appearance = appearance12;
            this.gridStoreGrades.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.gridStoreGrades.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance13.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance13.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance13.BorderColor = System.Drawing.SystemColors.Window;
            this.gridStoreGrades.DisplayLayout.GroupByBox.Appearance = appearance13;
            appearance14.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridStoreGrades.DisplayLayout.GroupByBox.BandLabelAppearance = appearance14;
            this.gridStoreGrades.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.gridStoreGrades.DisplayLayout.GroupByBox.Hidden = true;
            appearance15.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance15.BackColor2 = System.Drawing.SystemColors.Control;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridStoreGrades.DisplayLayout.GroupByBox.PromptAppearance = appearance15;
            this.gridStoreGrades.DisplayLayout.MaxColScrollRegions = 1;
            this.gridStoreGrades.DisplayLayout.MaxRowScrollRegions = 1;
            appearance16.BackColor = System.Drawing.SystemColors.Window;
            appearance16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gridStoreGrades.DisplayLayout.Override.ActiveCellAppearance = appearance16;
            this.gridStoreGrades.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.gridStoreGrades.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            this.gridStoreGrades.DisplayLayout.Override.CardAreaAppearance = appearance17;
            appearance18.BorderColor = System.Drawing.Color.Silver;
            appearance18.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.gridStoreGrades.DisplayLayout.Override.CellAppearance = appearance18;
            this.gridStoreGrades.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.gridStoreGrades.DisplayLayout.Override.CellPadding = 0;
            appearance19.BackColor = System.Drawing.SystemColors.Control;
            appearance19.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance19.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance19.BorderColor = System.Drawing.SystemColors.Window;
            this.gridStoreGrades.DisplayLayout.Override.GroupByRowAppearance = appearance19;
            appearance20.TextHAlignAsString = "Left";
            this.gridStoreGrades.DisplayLayout.Override.HeaderAppearance = appearance20;
            this.gridStoreGrades.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.gridStoreGrades.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance21.BackColor = System.Drawing.SystemColors.Window;
            appearance21.BorderColor = System.Drawing.Color.Silver;
            this.gridStoreGrades.DisplayLayout.Override.RowAppearance = appearance21;
            this.gridStoreGrades.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance22.BackColor = System.Drawing.SystemColors.ControlLight;
            this.gridStoreGrades.DisplayLayout.Override.TemplateAddRowAppearance = appearance22;
            this.gridStoreGrades.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.gridStoreGrades.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.gridStoreGrades.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.gridStoreGrades.Location = new System.Drawing.Point(14, 49);
            this.gridStoreGrades.Name = "gridStoreGrades";
            this.gridStoreGrades.Size = new System.Drawing.Size(494, 249);
            this.gridStoreGrades.TabIndex = 0;
            this.gridStoreGrades.TabStop = false;
            this.gridStoreGrades.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridStoreGrades_InitializeLayout);
            this.gridStoreGrades.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridStoreGrades_CellChange);
            this.gridStoreGrades.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.gridStoreGrades_MouseEnterElement);
            this.gridStoreGrades.DragDrop += new System.Windows.Forms.DragEventHandler(this.gridStoreGrades_DragDrop);
            this.gridStoreGrades.DragEnter += new System.Windows.Forms.DragEventHandler(this.gridStoreGrades_DragEnter);
            this.gridStoreGrades.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridMouseDown);
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.Location = new System.Drawing.Point(503, 454);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 7;
            this.btOk.Text = "button1";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(605, 454);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "button2";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.AllowDrop = true;
            this.cboStoreAttribute.AllowUserAttributes = false;
            this.cboStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStoreAttribute.Location = new System.Drawing.Point(76, 52);
            this.cboStoreAttribute.Name = "cboStoreAttribute";
            this.cboStoreAttribute.Size = new System.Drawing.Size(144, 21);
            this.cboStoreAttribute.TabIndex = 12;
            // 
            // AssortmentBasisBase
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(726, 489);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.cboStoreAttribute);
            this.Controls.Add(this.lblAttribute);
            this.Controls.Add(this.cbxSimilarStores);
            this.Controls.Add(this.cbxIntransit);
            this.Controls.Add(this.cbxOnhand);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.cbxCommitted);
            this.Name = "AssortmentBasisBase";
            this.Load += new System.EventHandler(this.AssortmentBasisBase_Load);
            this.Controls.SetChildIndex(this.cbxCommitted, 0);
            this.Controls.SetChildIndex(this.tabControl2, 0);
            this.Controls.SetChildIndex(this.cbxOnhand, 0);
            this.Controls.SetChildIndex(this.cbxIntransit, 0);
            this.Controls.SetChildIndex(this.cbxSimilarStores, 0);
            this.Controls.SetChildIndex(this.lblAttribute, 0);
            this.Controls.SetChildIndex(this.cboStoreAttribute, 0);
            this.Controls.SetChildIndex(this.btOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.tabBasis.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridBasis)).EndInit();
            this.tabStoreGrades.ResumeLayout(false);
            this.gbAverage.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridStoreGrades)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void AssortmentBasisBase_Load(object sender, System.EventArgs e)
		{
			_selectedHeaderKeyList = new ArrayList();
            _selectedAssortmentKeyList = new ArrayList(); // TT#488 - MD - Jellis - Group Allocation

            //LoadBase();   // TT#2 - RMatelic commented out; was causing designers of AssortmentViewSelection & AssortmentProperties to not open correctly
		}

		/// <summary>
		/// Fills in the text for all of the screen objects
		/// </summary>
		protected void SetScreenText()
		{
			cbxCommitted.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Committed);
			btOk.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
			btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
			lblAttribute.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Attribute);
			radAllStore.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AllStores);
			radSet.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Set);
			tabBasis.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Basis);
			tabStoreGrades.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Store_Grades);
			lblVariable.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Variable);
			radReceipts.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Receipts);
			radSales.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Sales);
			radStock.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Stock);
			cbxOnhand.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_CurrentOnHand);
			cbxIntransit.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Intransit);
			cbxSimilarStores.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Similar_Store);
			radIndexBoundary.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Index);
			radUnitsBoundary.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Units);

		}

		//private void Common_Load(eGlobalUserType aGlobalUserType)
		protected void LoadBase()
		{
			try
			{
                _assortmentReview = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentReview);
                _assortmentReviewAssortment = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentReviewAssortment);
                _assortmentReviewContent = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentReviewContent);
                _assortmentReviewCharacteristic = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentReviewCharacteristic);
                if (_loadedFrom == eAssortmentBasisLoadedFrom.AssortmentProperties
                    || _loadedFrom == eAssortmentBasisLoadedFrom.GroupAllocation)
                {
                    _key = _asp.Key;
                    _storeAttributeRid = _asp.AssortmentStoreGroupRID;
                    _variableType = _asp.AssortmentVariableType;
                    _variableNumber = _asp.AssortmentVariableNumber;
                    _inclOnhand = _asp.AssortmentIncludeOnhand;
                    _inclIntransit = _asp.AssortmentIncludeIntransit;
                    _inclSimStores = _asp.AssortmentIncludeSimilarStores;
                    // Begin TT#1725 - RMatelic - "Hide" Committed
                    //_inclCommitted = _asp.AssortmentIncludeCommitted;
                    _inclCommitted = false;
                    // End TT#1725
                    _averageBy = _asp.AssortmentAverageBy;
                    _gradeBoundary = _asp.AssortmentGradeBoundary;
                    _basisList = _asp.AssortmentBasisList;
                    //_storeGradeList = _asp.StoreGradeList;  // TT#488 - MD - Jellis - Group Allocation
					_assortmentStoreGradeList = _asp.AssortmentStoreGradeList;	// TT#488-MD - STodd - Group Allocation 
                }
                else if (_loadedFrom == eAssortmentBasisLoadedFrom.UserSelectionCriteria)
                {
                    _key = _trans.AssortmentUserRid;
                    _storeAttributeRid = _trans.AssortmentStoreAttributeRid;
                    _variableType = _trans.AssortmentVariableType;
                    _variableNumber = _trans.AssortmentVariableNumber;
                    _inclOnhand = _trans.AssortmentIncludeOnhand;
                    _inclIntransit = _trans.AssortmentIncludeIntransit;
                    _inclSimStores = _trans.AssortmentIncludeSimStore;
                    // Begin TT#1725 - RMatelic - "Hide" Committed
                    //_inclCommitted = _trans.AssortmentIncludeCommitted;
                    _inclCommitted = false;
                    // End TT#1725
                    _averageBy = _trans.AssortmentAverageBy;
                    _gradeBoundary = _trans.AssortmentGradeBoundary;
                    FillBasisList(_trans.AssortmentBasisDataTable);
                    FillStoreGradeList(_trans.AssortmentStoreGradeDataTable);
                }
                else
                {
                    _key = Include.NoRID;
                    _storeAttributeRid = Include.UndefinedStoreGroupRID;
                    _variableType = eAssortmentVariableType.None;
                    _variableNumber = Include.NoRID;
                    _inclOnhand = false;
                    _inclIntransit = false;
                    _inclSimStores = false;
                    _inclCommitted = false;
                    _averageBy = eStoreAverageBy.None;
                    _gradeBoundary = eGradeBoundary.Unknown;
                    _basisList = new List<AssortmentBasis>();
                    //_storeGradeList = new ProfileList(eProfileType.StoreGrade);  // TT#488 - MD - Jellis - Group Allocation
					_assortmentStoreGradeList = _asp.AssortmentStoreGradeList;	// TT#488-MD - STodd - Group Allocation 
                }

                _versionProfList = SAB.ClientServerSession.GetUserForecastVersions();
                _dynamicToPlanImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToPlanImage);
                _dynamicToCurrentImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToCurrentImage);

                LoadCombos();
                LoadVersionValuelist();
                LoadGrids();
                LoadScreen();

                if (this.radStock.Checked)
                {
                    this.cbxIntransit.Enabled = true;
                    this.cbxOnhand.Enabled = true;
                }
                else
                {
                    this.cbxIntransit.Enabled = false;
                    this.cbxOnhand.Enabled = false;
                }
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}		
		}

		protected void LoadCombos()
		{
			try
			{
				LoadAttributeCombo();
			}
			catch
			{
				throw;
			}
		}

		

		private void LoadAttributeCombo()
		{
            try
			{
                ProfileList _storeGroupList = GetStoreGroupList(false);
                cboStoreAttribute.Initialize(SAB, FunctionSecurity, _storeGroupList.ArrayList, true);

                AdjustTextWidthComboBox_DropDown(cboStoreAttribute);//TT#7 - RBeck - Dynamic dropdowns
			}
			catch
			{
				throw;
			}
		}

		protected ProfileList GetStoreGroupList(bool aAddEmptyGroup)
		{
			ProfileList al = null;
			StoreGroupListViewProfile sgp;
			try
			{
                al = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.All, true); //SAB.StoreServerSession.GetStoreGroupListViewList(eStoreGroupSelectType.All, true);

				if (aAddEmptyGroup)
				{
					sgp = new StoreGroupListViewProfile(Include.NoRID);
					sgp.Name = string.Empty;
					al.Add(sgp);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			return al;
		}

		private void LoadVersionValuelist()
		{
			//Add a list to the grids, and name it "Version".
			if (!gridBasis.DisplayLayout.ValueLists.Exists("Version"))
			{
				gridBasis.DisplayLayout.ValueLists.Add("Version");

				//Loop through the user version list and manually add value and text to the lists.
				for (int i = 0; i < _versionProfList.Count; i++)
				{
					VersionProfile vp = (VersionProfile)_versionProfList[i];
					Infragistics.Win.ValueListItem vli = new Infragistics.Win.ValueListItem();

					vli.DataValue = vp.Key;
					vli.DisplayText = vp.Description;
					gridBasis.DisplayLayout.ValueLists["Version"].ValueListItems.Add(vli);

					vli.Dispose();
				}
			}
		}

		private void LoadGrids()
		{
			try
			{
				LoadBasisGrid();
				LoadStoreGradesGrid();
				// No Assortment workflows at this time
				//LoadWorkflowGrid();
			}
			catch
			{
				throw;
			}
		}

		private void LoadBasisGrid()
		{
			try
			{
				_dtBasis = MIDEnvironment.CreateDataTable("Assortment Basis");

				_dtBasis.Columns.Add("HDR_RID", System.Type.GetType("System.Int32"));
				_dtBasis.Columns.Add("BASIS_SEQUENCE", System.Type.GetType("System.Int32"));
				_dtBasis.Columns.Add("HN_RID", System.Type.GetType("System.Int32"));
				_dtBasis.Columns.Add("FV_RID", System.Type.GetType("System.Int32"));
				_dtBasis.Columns.Add("CDR_RID", System.Type.GetType("System.Int32"));
				_dtBasis.Columns.Add("WEIGHT", System.Type.GetType("System.Double"));
				_dtBasis.Columns.Add("Merchandise",		System.Type.GetType("System.String"));
				_dtBasis.Columns.Add("Version",			System.Type.GetType("System.String"));
				_dtBasis.Columns.Add("HorizonDateRange", System.Type.GetType("System.String"));

				int seq = 0;
				// Loads in display versions of the RIDs
				foreach (AssortmentBasis ab in _basisList)
				{
					DataRow aRow = _dtBasis.NewRow();
                    if (_asp == null)
                    {
                        aRow["HDR_RID"] = Include.NoRID;
                    }
                    else
                    {
                        aRow["HDR_RID"] = _asp.Key;
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

				gridBasis.DataSource = _dtBasis;
                SetHorizonDateDynamicImage(); //TT#1497 - MD - DOConnell - when selecting a date range that is dynamic to plan there is no icon indicating it is dynamic in the Horizon Date Range.
                
            }
			catch
			{
				throw;
			}
		}

        //BEGIN TT#1497 - MD - DOConnell - when selecting a date range that is dynamic to plan there is no icon indicating it is dynamic in the Horizon Date Range.
        /// <summary>
        /// Used at window initial load to add Dynamic Icon to the Horizon Date if needed.
        /// </summary>
        private void SetHorizonDateDynamicImage()
        { 
            try
            {
                if (_basisList.Count == gridBasis.Rows.Count)
                {
                    for (int i = 0; i < _basisList.Count; i++)
                    {
                        if (_basisList[i].HierarchyNodeProfile.Key == Convert.ToInt32(gridBasis.Rows[i].Cells["HN_RID"].Value))
                        {
                            if (_basisList[i].HorizonDate.DateRangeType == eCalendarRangeType.Dynamic)
                            {
                                switch (_basisList[i].HorizonDate.RelativeTo)
                                {
                                    case eDateRangeRelativeTo.Current:
                                        gridBasis.Rows[i].Cells["HorizonDateRange"].Appearance.Image = this.DynamicToCurrentImage;
                                        break;
                                    case eDateRangeRelativeTo.Plan:
                                        gridBasis.Rows[i].Cells["HorizonDateRange"].Appearance.Image = this.DynamicToPlanImage;
                                        break;
                                    default:
                                        gridBasis.Rows[i].Cells["HorizonDateRange"].Appearance.Image = null;
                                        break;
                                }
                            }
                            else
                            {
                                gridBasis.Rows[i].Cells["HorizonDateRange"].Appearance.Image = null;
                                //break;
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
        //END TT#1497 - MD - DOConnell - when selecting a date range that is dynamic to plan there is no icon indicating it is dynamic in the Horizon Date Range.

        // Begin TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
        protected void AnchorStoreGradesGrid()
        {
            this.gridStoreGrades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
        }
        // Begin TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working

        /// <summary>
        /// Used at window initial load to fill in previously defined store grades
        /// </summary>
		private void LoadStoreGradesGrid()
		{
			try
			{
				_dtStoreGrades = MIDEnvironment.CreateDataTable("Store Grades");

				_dtStoreGrades.Columns.Add("HDR_RID", System.Type.GetType("System.Int32"));
				_dtStoreGrades.Columns.Add("STORE_GRADE_SEQ", System.Type.GetType("System.Int32"));
				_dtStoreGrades.Columns.Add("BOUNDARY_INDEX", System.Type.GetType("System.Int32"));
				_dtStoreGrades.Columns.Add("BOUNDARY_UNITS", System.Type.GetType("System.Int32"));
				_dtStoreGrades.Columns.Add("GRADE_CODE", System.Type.GetType("System.String"));

				int seq = 0;
				foreach (StoreGradeProfile sgp in _assortmentStoreGradeList.ArrayList)	// TT#488-MD - STodd - Group Allocation 
				{
					DataRow aRow = _dtStoreGrades.NewRow();
                    if (_asp == null)
                    {
                        aRow["HDR_RID"] = Include.NoRID;
                    }
                    else
                    {
                        aRow["HDR_RID"] = _asp.Key;
                    }
					aRow["STORE_GRADE_SEQ"] = seq++;
					aRow["BOUNDARY_INDEX"] = sgp.Boundary;
					aRow["BOUNDARY_UNITS"] = sgp.BoundaryUnits;
					aRow["GRADE_CODE"] = sgp.StoreGrade;

					_dtStoreGrades.Rows.Add(aRow);
				}

				_dtStoreGrades.AcceptChanges();

				gridStoreGrades.DataSource = _dtStoreGrades;
			}
			catch
			{
				throw;
			}
		}

		///// <summary>
		///// called when a new merchandise node is placed in the basis. 
		///// Uses only the node from the first basis defined.
		///// </summary>
		protected void LoadMerchandiseStoreGrades(int hnKey, eGradesLoadedFrom loadFrom)
		{
			try
			{
				if (_gradesLoadedFrom == eGradesLoadedFrom.Basis)
				{
					if (_dtStoreGrades.Rows.Count > 0)
					{
						if (_hnRidGradesLoadedFrom != hnKey)
						{
							string msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_StoreGradesAlreadyExist);
							msg = msg.Replace("{0}", "first basis merchandise");

							if (MessageBox.Show(msg,
								this.Text, MessageBoxButtons.YesNo,
								MessageBoxIcon.Question,
								MessageBoxDefaultButton.Button2,
								MessageBoxOptions.DefaultDesktopOnly)
										== DialogResult.Yes)
							{
								LoadStoreGrades(hnKey);
								_gradesLoadedFrom = loadFrom;
								_hnRidGradesLoadedFrom = hnKey;
							}
						}
					}
				}
				else if (_gradesLoadedFrom == eGradesLoadedFrom.ApplyTo)
				{
					if (_hnRidGradesLoadedFrom != hnKey)
					{
						string msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_StoreGradesAlreadyExist);
						msg = msg.Replace("{0}", "Apply To merchandise");

						if (MessageBox.Show(msg,
							this.Text, MessageBoxButtons.YesNo,
							MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2,
							MessageBoxOptions.DefaultDesktopOnly)
									== DialogResult.Yes)
						{
							LoadStoreGrades(hnKey);
							_gradesLoadedFrom = loadFrom;
							_hnRidGradesLoadedFrom = hnKey;
						}
					}
				}
				else if (_gradesLoadedFrom == eGradesLoadedFrom.None)
				{
					LoadStoreGrades(hnKey);
					_gradesLoadedFrom = loadFrom;
					_hnRidGradesLoadedFrom = hnKey;
				}
			}
			catch
			{
				throw;
			}
		}

        protected void LoadStoreGrades(int hnKey)
        {
            try
            {
                _dtStoreGrades.Rows.Clear();
                int seq = 1;
                StoreGradeList sgl = this.ApplicationTransaction.GetStoreGradeList(hnKey);
                foreach (StoreGradeProfile aGrade in sgl)
                {
                    DataRow newRow = _dtStoreGrades.NewRow();
                    newRow["HDR_RID"] = _key;
                    newRow["STORE_GRADE_SEQ"] = seq++;
                    newRow["BOUNDARY_INDEX"] = aGrade.Boundary;
					newRow["BOUNDARY_UNITS"] = aGrade.BoundaryUnits;
                    newRow["GRADE_CODE"] = aGrade.StoreGrade;
                    _dtStoreGrades.Rows.Add(newRow);
                }
                _dtStoreGrades.AcceptChanges();
                gridStoreGrades.DataSource = _dtStoreGrades;
            }
            catch
            {
                throw;
            }
        }

		/// <summary>
		/// Used to unload a basis dataTable into a list
		/// </summary>
		/// <param name="dt"></param>
		private void FillBasisList(DataTable dt)
		{
			try
			{
                //BEGIN TT#1460 - DOConnell - Severe Error-> Invalid Calendar Date
                DayProfile anchoreDay = null;
                //DayProfile anchoreDay = SAB.ApplicationServerSession.Calendar.GetFirstDayOfRange(_anchorDateRangeRid);	// TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
                if (dt.Rows.Count > 0)
                {
                    anchoreDay = SAB.ApplicationServerSession.Calendar.GetFirstDayOfRange(_anchorDateRangeRid);	// TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
                }
                //END TT#1460 - DOConnell - Severe Error-> Invalid Calendar Date
                _basisList = new List<AssortmentBasis>();
				foreach (DataRow aRow in dt.Rows)
				{
					int hierNodeRid = Convert.ToInt32(aRow["HN_RID"], CultureInfo.CurrentUICulture);
					int versionRid = Convert.ToInt32(aRow["FV_RID"], CultureInfo.CurrentUICulture);
					int dateRangeRid = Convert.ToInt32(aRow["CDR_RID"], CultureInfo.CurrentUICulture);
					float weight = (float)Convert.ToDouble(aRow["WEIGHT"], CultureInfo.CurrentUICulture);

                    AssortmentBasis ab = new AssortmentBasis(SAB, _trans, hierNodeRid, versionRid, dateRangeRid, weight, anchoreDay);	// TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
					_basisList.Add(ab);
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Used to unload a storegrades dataTable into a list
		/// </summary>
		/// <param name="dt"></param>
		private void FillStoreGradeList(DataTable dt)
		{
			try
			{
				_assortmentStoreGradeList = new ProfileList(eProfileType.StoreGrade);	// TT#488-MD - STodd - Group Allocation 
				foreach (DataRow aRow in dt.Rows)
				{
					int seq = Convert.ToInt32(aRow["STORE_GRADE_SEQ"], CultureInfo.CurrentUICulture);
					int boundary = Convert.ToInt32(aRow["BOUNDARY_INDEX"], CultureInfo.CurrentUICulture);
					int boundaryUnits = Convert.ToInt32(aRow["BOUNDARY_UNITS"], CultureInfo.CurrentUICulture);
					string gradeCode = aRow["GRADE_CODE"].ToString().Trim();
					StoreGradeProfile sgp = new StoreGradeProfile(boundary);
					sgp.Boundary = boundary;
					sgp.StoreGrade = gradeCode;
					sgp.BoundaryUnits = boundaryUnits;
					_assortmentStoreGradeList.Add(sgp);		// TT#488-MD - STodd - Group Allocation 
				}
			}
			catch
			{
				throw;
			}
		}

		private void LoadWorkflowGrid()
		{
			try
			{
				//GetWorkflows(_assortmentGeneralMethod.Key, ugWorkflows);	
			}
			catch
			{
				throw;
			}
		}

		protected void LoadScreen()
		{
			try
			{


				LoadCombos();
				//LoadGrids(); //TT#1497 - MD - DOConnell - when selecting a date range that is dynamic to plan there is no icon indicating it is dynamic in the Horizon Date Range.

                if (_storeAttributeRid != Include.NoRID)
                {
					this.cboStoreAttribute.SelectedValue = _storeAttributeRid;
                    if (cboStoreAttribute.ContinueReadOnly)
                    {
                       // SetMethodReadOnly();
                    }
                }

				if (_averageBy == eStoreAverageBy.AllStores)
				{
					this.radAllStore.Checked = true;
				}
				else if (_averageBy == eStoreAverageBy.Set)
				{
					this.radSet.Checked = true;
				}

				if (_inclOnhand)
					this.cbxOnhand.Checked = true;
				else
					this.cbxOnhand.Checked = false;
				if (_inclIntransit)
					this.cbxIntransit.Checked = true;
				else
					this.cbxIntransit.Checked = false;
				if (_inclSimStores)
					this.cbxSimilarStores.Checked = true;
				else
					this.cbxSimilarStores.Checked = false;
				if (_inclCommitted)
					this.cbxCommitted.Checked = true;
				else
					this.cbxCommitted.Checked = false;

				if (_variableType == eAssortmentVariableType.Receipts)
					radReceipts.Checked = true;
				else if (_variableType == eAssortmentVariableType.Stock)
					radStock.Checked = true;
				else if (_variableType == eAssortmentVariableType.Sales)
					radSales.Checked = true;

				if (_gradeBoundary == eGradeBoundary.Index)
				{
					this.radIndexBoundary.Checked = false;
					this.radIndexBoundary.Checked = true;
				}
				else if (_gradeBoundary == eGradeBoundary.Units)
				{
					this.radUnitsBoundary.Checked = true;
				}

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
		//override protected eWorkflowMethodIND WorkflowMethodInd()
		//{
		//    return eWorkflowMethodIND.Methods;	
		//}

		/// <summary>
		/// Use to set the method name, description, user and global radio buttons
		/// </summary>
		//override protected void SetCommonFields()
		//{
		//    WorkflowMethodName = txtName.Text;
		//    WorkflowMethodDescription = txtDesc.Text;
		//    GlobalRadioButton = radGlobal;
		//    UserRadioButton = radUser;
		//}

		virtual protected void SetSpecificFields()
		{
			throw new Exception("Can not call base method: SetSpecificFields()");
		}

		virtual protected bool ValidateSpecificFields()
		{
			throw new Exception("Can not call base method: ValidateSpecificFields()");
		}

		virtual protected void SaveWindow()
		{
			throw new Exception("Can not call base method: SaveWindow()");
		}

		virtual protected bool Process()
		{
			// Should be overridden as needed.
			return false;
		}


		/// <summary>
		/// Use to set the specific fields before updating
		/// </summary>
		protected void SetBase()
		{
			try
			{
                DayProfile anchoreDay = SAB.ApplicationServerSession.Calendar.GetFirstDayOfRange(_anchorDateRangeRid);	// TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 

				// Store Attribute
				if (cboStoreAttribute.Text.Trim() != string.Empty)
				{
					_storeAttributeRid = Convert.ToInt32(cboStoreAttribute.SelectedValue, CultureInfo.CurrentUICulture);
				}
				else
				{
					_storeAttributeRid = Include.NoRID;
				}

				// Average By
				if (this.radAllStore.Checked)
					_averageBy = eStoreAverageBy.AllStores;
				else
					_averageBy = eStoreAverageBy.Set;

				// Variable
				if (radReceipts.Checked)
					_variableType = eAssortmentVariableType.Receipts;
				else if (radStock.Checked)
					_variableType = eAssortmentVariableType.Stock;
				else
					_variableType = eAssortmentVariableType.Sales;

				// Checkboxes
                // Begin TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
                //_inclOnhand = cbxOnhand.Checked;
                //_inclIntransit = cbxIntransit.Checked;
                _inclOnhand = true;
                _inclIntransit = true;
                // End TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
				_inclSimStores = cbxSimilarStores.Checked;
				_inclCommitted = cbxCommitted.Checked;

				// Grade Boundary
				if (radIndexBoundary.Checked)
					_gradeBoundary = eGradeBoundary.Index;
				else
					_gradeBoundary = eGradeBoundary.Units;


				List<int> hierNodeList = new List<int>();
				List<int> versionList = new List<int>();
				List<int> dateRangeList = new List<int>();
				List<double> weightList = new List<double>();
				_basisList.Clear();
				bool filled = false;
				foreach (DataRow aRow in _dtBasis.Rows)
				{
					int hnRid = Convert.ToInt32(aRow["HN_RID"]);
					int verRid = Convert.ToInt32(aRow["FV_RID"]);
					int cdrRid = Convert.ToInt32(aRow["CDR_RID"]);
					float weight = (float)Convert.ToDouble(aRow["WEIGHT"]);
                    // Begin RMatelic mod
					//AssortmentBasis ab = new AssortmentBasis(SAB, _asp.AppSessionTransaction, hnRid, verRid, cdrRid, weight);
                    AssortmentBasis ab;
                    if (_loadedFrom == eAssortmentBasisLoadedFrom.AssortmentProperties
                        || _loadedFrom == eAssortmentBasisLoadedFrom.GroupAllocation)
                    {
                        ab = new AssortmentBasis(SAB, _asp.AppSessionTransaction, hnRid, verRid, cdrRid, weight, anchoreDay);	// TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
                    }
                    else
                    {
                        ab = new AssortmentBasis(SAB, _trans, hnRid, verRid, cdrRid, weight, anchoreDay);	// TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
                    }
                    // End RMatelic mod
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
				if (_trans == null)
				{
					_trans = _asp.AppSessionTransaction;
				}
                if (_anchorNodeRid < 1)
                {
                    _anchorNodeRid = (int)hierNodeList[0];
                }

                AllocationProfileList alp = (AllocationProfileList)_trans.GetMasterProfileList(eProfileType.Allocation);
                if (_asp == null)
                {
                    //BEGIN TT#221 - MD - DOConnell - Allocation Workspace right click  Review>Select >Assortment Receive an InvalidCastException
                    //_asp = (AssortmentProfile)alp.ArrayList[0];

                    for (int i = 0; i < alp.ArrayList.Count; i++)
                    {
                        // Begin TT#4988 - BVaughan - Performance
                        #if DEBUG
                        if ((alp.ArrayList[i] is AssortmentProfile && !((AllocationProfile)alp.ArrayList[i]).isAssortmentProfile) || (!(alp.ArrayList[i] is AssortmentProfile) && ((AllocationProfile)alp.ArrayList[i]).isAssortmentProfile))
                        {
                            throw new Exception("Object does not match AssortmentProfile in SetBase()");
                        }
                        #endif
                        //if (alp.ArrayList[i] is AssortmentProfile)
                        if (((AllocationProfile)alp.ArrayList[i]).isAssortmentProfile)
                        // End TT#4988 - BVaughan - Performance
                        {
                            _asp = (AssortmentProfile)alp.ArrayList[i];
                            break;
                        }
                    }
                    //END TT#221 - MD - DOConnell - Allocation Workspace right click  Review>Select >Assortment Receive an InvalidCastException
                }



				//Begin TT#2 - JScott - Assortment Planning - Phase 2
				//_basisReader = new AssortmentBasisReader(SAB, _trans, _anchorNodeRid, hierNodeList, versionList, dateRangeList, weightList, Include.AllStoreGroupRID, false, false, false, false, null);
				//_variableNumber = _basisReader.GetVariableNumber(_variableType);

				////_trans = null;
				//_basisReader = null;
                _asp.AssortmentIncludeSimilarStores = _inclSimStores;  //TT#1984-MD - AGallagher - Similar Store when checked not using similar store values.
                _asp.AssortmentIncludeOnhand = _inclOnhand;  //TT#1991-MD - AGallagher - Current On Hand checked or unchecked can get the same values.  Would expect them to be different.
                _asp.AssortmentIncludeIntransit = _inclIntransit;  //TT#1991-MD - AGallagher - Current On Hand checked or unchecked can get the same values.  Would expect them to be different.

                // Begin TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO
				// Must set basis list before accessing BasisReader or uses previous basis values to open the cube.
				if (_loadedFrom == eAssortmentBasisLoadedFrom.AssortmentProperties
                    || _loadedFrom == eAssortmentBasisLoadedFrom.GroupAllocation)
                {
                    _asp.AssortmentBasisList = _basisList;
                }
				// End TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO

				_variableNumber = _asp.BasisReader.GetVariableNumber(_variableType);
				//End TT#2 - JScott - Assortment Planning - Phase 2

				_assortmentStoreGradeList.Clear();	// TT#488-MD - STodd - Group Allocation 
				foreach (DataRow aRow in _dtStoreGrades.Rows)
				{
					//int seq = Convert.ToInt32(aRow["STORE_GRADE_SEQ"], CultureInfo.CurrentUICulture);
					// BEGIN TT#1596 - Null ref adding new grade
					int boundary = 0;
					if (aRow["BOUNDARY_INDEX"] != DBNull.Value)
					{
						boundary = Convert.ToInt32(aRow["BOUNDARY_INDEX"], CultureInfo.CurrentUICulture);
					}
					int boundaryUnits = 0;
					if (aRow["BOUNDARY_UNITS"] != DBNull.Value)
					{
						boundaryUnits = Convert.ToInt32(aRow["BOUNDARY_UNITS"], CultureInfo.CurrentUICulture);
					}
					// END TT#1596 - Null ref adding new grade
					string gradeCode = aRow["GRADE_CODE"].ToString().Trim();
					StoreGradeProfile sgp = new StoreGradeProfile(boundary);
					sgp.Boundary = boundary;
					sgp.StoreGrade = gradeCode;
					sgp.BoundaryUnits = boundaryUnits;
					_assortmentStoreGradeList.Add(sgp);		// TT#488-MD - STodd - Group Allocation 
				}

				if (_loadedFrom == eAssortmentBasisLoadedFrom.AssortmentProperties
                    || _loadedFrom == eAssortmentBasisLoadedFrom.GroupAllocation)
				{
					_asp.Key = _key;
					_asp.AssortmentStoreGroupRID = _storeAttributeRid;
					_asp.AssortmentVariableType = _variableType;
					_asp.AssortmentVariableNumber = _variableNumber;
					_asp.AssortmentIncludeOnhand = _inclOnhand;
					_asp.AssortmentIncludeIntransit = _inclIntransit;
					_asp.AssortmentIncludeSimilarStores = _inclSimStores;
					_asp.AssortmentIncludeCommitted = _inclCommitted;
					_asp.AssortmentAverageBy = _averageBy;
					_asp.AssortmentGradeBoundary = _gradeBoundary;
					_asp.AssortmentBasisList = _basisList;
                    //_asp.StoreGradeList = _storeGradeList;  // TT#488 - MD - Jellis -Group Allocation
					_asp.AssortmentStoreGradeList = _assortmentStoreGradeList;	// TT#488-MD - STodd - Group Allocation 
				}
				else if (_loadedFrom == eAssortmentBasisLoadedFrom.UserSelectionCriteria)
				{
					//_trans.AssortmentUserRid = _key;
					_trans.AssortmentStoreAttributeRid = _storeAttributeRid;
					_trans.AssortmentVariableType = _variableType;
					_trans.AssortmentVariableNumber = _variableNumber;
					_trans.AssortmentIncludeOnhand = _inclOnhand;
					_trans.AssortmentIncludeIntransit = _inclIntransit;
					_trans.AssortmentIncludeSimStore = _inclSimStores;
					_trans.AssortmentIncludeCommitted = _inclCommitted;
					_trans.AssortmentAverageBy = _averageBy;
					_trans.AssortmentGradeBoundary = _gradeBoundary;
					_trans.AssortmentBasisDataTable = _dtBasis;
					_trans.AssortmentStoreGradeDataTable = _dtStoreGrades;
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Use to validate the fields that are specific to this method type
		/// </summary>
		protected bool ValidateFields()
		{
			bool methodFieldsValid = true;

			if (cboStoreAttribute.SelectedIndex == Include.Undefined)
			{
				methodFieldsValid = false;
				ErrorProvider.SetError(cboStoreAttribute, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
			}
			else
			{
				ErrorProvider.SetError(cboStoreAttribute, string.Empty);
			}

			_dtBasis.AcceptChanges();
			_dtStoreGrades.AcceptChanges();

			if (methodFieldsValid)
				methodFieldsValid = ValidBasisGrid();

			if (methodFieldsValid)
				methodFieldsValid = ValidStoreGradeGrid();

			return methodFieldsValid;
		}

		private bool ValidBasisGrid()
		{
			bool errorFound = false;
			try
			{
				ErrorProvider.SetError (gridBasis,string.Empty);

				if (gridBasis.Rows.Count == 0)
				{
					string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_RequiredBasisMissing);
					gridBasis.Tag = errorMessage;
					ErrorProvider.SetError (gridBasis,errorMessage);
					errorFound = true;
				}


				foreach (UltraGridRow gridRow in gridBasis.Rows)
				{
					if (!ValidMerchandise(gridRow.Cells["Merchandise"]))
					{
						errorFound = true;
					}
					if (!ValidVersion(gridRow.Cells["Version"]))
					{
						errorFound = true;
					}
					if (!ValidDateRange(gridRow.Cells["HorizonDateRange"]))
					{
						errorFound = true;
					}
					if (!ValidWeight(gridRow.Cells["WEIGHT"]))
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
		private bool ValidMerchandise (UltraGridCell gridCell)
		{
			bool errorFound = false;
			string errorMessage = string.Empty;
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
//					foreach (Infragistics.Win.ValueListItem vli in ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems)
//					{
//						if ( Convert.ToInt32(vli.DataValue, CultureInfo.CurrentUICulture) == Convert.ToInt32(gridCell.Value, CultureInfo.CurrentUICulture))
//						{
//							rowseq = Convert.ToInt32(vli.DataValue, CultureInfo.CurrentUICulture);
//							break;
//						}
//
//					}	
//					if (rowseq != -1)
//					{
//						DataRow row = _merchDataTable2.Rows[rowseq];
//						if (row != null)
//							return true;
//					}
					//int key = GetNodeRid(ref productID);
					//if (key == Include.NoRID)
					//{
					//    errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
					//        productID);
					//    errorFound = true;
					//}
					//else
					//{
					//    ApplicationSessionTransaction aTrans = SAB.ApplicationServerSession.CreateTransaction();
					//    HierarchyNodeProfile hierarchyNodeProfile = aTrans.GetPlanLevelData(key);
					//    if (hierarchyNodeProfile == null)
					//    {
					//        errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_PlanLevelUndetermined),
					//            productID);
					//        errorFound = true;
					//    }
					//}
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

		private bool ValidStoreGradeGrid()
		{
			bool errorFound = false;
			try
			{
				ErrorProvider.SetError (gridStoreGrades,string.Empty);

//				if (gridStoreGrades.Rows.Count == 0)
//				{
//					string errorMessage = "A required Store Grade is missing.";
//					gridStoreGrades.Tag = errorMessage;
//					ErrorProvider.SetError (gridStoreGrades,errorMessage);
//					errorFound = true;
//				}

				bool indexUsed = false;
				bool unitsUsed = false;
				foreach (UltraGridRow gridRow in gridStoreGrades.Rows)
				{
					if (!ValidGrade(gridRow.Cells["GRADE_CODE"]))
					{
						errorFound = true;
					}
					if (!ValidIndexAndUnits(gridRow.Cells["BOUNDARY_INDEX"], gridRow.Cells["BOUNDARY_UNITS"], ref indexUsed, ref unitsUsed))
					{
						errorFound = true;
					}

				}

				//if (indexUsed && unitsUsed)
				//{
				//    string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_IndexOrUnitsNotBoth);
				//    gridStoreGrades.Tag = errorMessage;
				//    ErrorProvider.SetError (gridStoreGrades,errorMessage);
				//    errorFound = true;
				//}

				if (!errorFound && !unitsUsed && this.radUnitsBoundary.Checked)
                {
                    string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UnitGradeBoundarySelected);
                    gridStoreGrades.Tag = errorMessage;
                    ErrorProvider.SetError(gridStoreGrades, errorMessage);
                    errorFound = true;
                }

                if (!errorFound && !indexUsed && this.radIndexBoundary.Checked)
                {
                    string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_IndexGradeBoundarySelected);
                    gridStoreGrades.Tag = errorMessage;
                    ErrorProvider.SetError(gridStoreGrades, errorMessage);
                    errorFound = true;
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

		private bool ValidGrade (UltraGridCell gridCell)
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
				return true;
			}
		}

		private bool ValidIndexAndUnits (UltraGridCell indexGridCell, UltraGridCell unitsGridCell, ref bool indexedUsed, ref bool unitsUsed)
		{
			bool errorFound = false;
			string errorMessage = null;
			double dblValue;
		
			if (indexGridCell.Value.ToString() == string.Empty && unitsGridCell.Value.ToString() == string.Empty)
			{
				errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MissingIndexUnitValues);
				errorFound = true;
				indexGridCell.Appearance.Image = ErrorImage;
				indexGridCell.Tag = errorMessage;
				unitsGridCell.Appearance.Image = ErrorImage;
				unitsGridCell.Tag = errorMessage;
			}
			//else if (indexGridCell.Value.ToString() != string.Empty && unitsGridCell.Value.ToString() != string.Empty)
			//{
			//    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_IndexOrUnitsNotBoth);
			//    errorFound = true;
			//    indexGridCell.Appearance.Image = ErrorImage;
			//    indexGridCell.Tag = errorMessage;
			//    unitsGridCell.Appearance.Image = ErrorImage;
			//    unitsGridCell.Tag = errorMessage;
			//}
			else
			{
				// INDEX
				try
				{
					if (indexGridCell.Value.ToString() != string.Empty)
					{
						indexedUsed = true;
						dblValue = Convert.ToDouble(indexGridCell.Value.ToString(), CultureInfo.CurrentUICulture);
						if (dblValue < 0)
						{
							errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
							errorFound = true;
							indexGridCell.Appearance.Image = ErrorImage;
							indexGridCell.Tag = errorMessage;
						}
						else
						{
							dblValue = Math.Round(dblValue,2);
							indexGridCell.Value  = dblValue;
						}
					}
				}
				catch (Exception error)
				{
					string exceptionMessage = error.Message;
					errorMessage = error.Message;
					errorFound = true;
					indexGridCell.Appearance.Image = ErrorImage;
					indexGridCell.Tag = errorMessage;
				}
				// UNITS
				try
				{
					if (unitsGridCell.Value.ToString() != string.Empty)
					{
						unitsUsed = true;
						dblValue = Convert.ToDouble(unitsGridCell.Value.ToString(), CultureInfo.CurrentUICulture);
						if (dblValue < 0)
						{
							errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
							errorFound = true;
							unitsGridCell.Appearance.Image = ErrorImage;
							unitsGridCell.Tag = errorMessage;
						}
						else
						{
                            //dblValue = dblValue;
							unitsGridCell.Value  = dblValue;
						}
					}
				}
				catch (Exception error)
				{
					string exceptionMessage = error.Message;
					errorMessage = error.Message;
					errorFound = true;
					unitsGridCell.Appearance.Image = ErrorImage;
					unitsGridCell.Tag = errorMessage;
				}
			}	

			if (errorFound)
			{
				return false;
			}
			else
			{
				indexGridCell.Appearance.Image = null;
				indexGridCell.Tag = null;
				unitsGridCell.Appearance.Image = null;
				unitsGridCell.Tag = null;
				return true;
			}
		}


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

		/// <summary>
		/// Use to return the explorer node selected when form was opened
		/// </summary>
		//override protected MIDWorkflowMethodTreeNode GetExplorerNode()
		//{
		//    return _explorerNode;
		//}

		#endregion WorkflowMethodFormBase Overrides		

		private void gridBasis_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			try
			{
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
                ugld.ApplyDefaults(e, false);	// stodd - merge issue
                e.Layout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.Default;
				e.Layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
				gridBasis.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

				//hide the key columns.
				e.Layout.Bands[0].Columns["HDR_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["BASIS_SEQUENCE"].Hidden = true;
				e.Layout.Bands[0].Columns["HN_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["FV_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["CDR_RID"].Hidden = true;				
			
				//Prevent the user from re-arranging columns.
				gridBasis.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;

				//if (FunctionSecurity.AllowUpdate)
				//{
				//    gridBasis.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
				//    gridBasis.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
				//}
				//else
				//{
				//    gridBasis.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
				//    gridBasis.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
				//}

				//Set the header captions.
				gridBasis.DisplayLayout.Bands[0].Columns["Merchandise"].Header.VisiblePosition = 1;
				gridBasis.DisplayLayout.Bands[0].Columns["Merchandise"].Header.Caption = "Merchandise";
				gridBasis.DisplayLayout.Bands[0].Columns["Version"].Header.VisiblePosition = 2;
				gridBasis.DisplayLayout.Bands[0].Columns["Version"].Header.Caption = "Version";
				gridBasis.DisplayLayout.Bands[0].Columns["HorizonDateRange"].Header.VisiblePosition = 3;
				gridBasis.DisplayLayout.Bands[0].Columns["HorizonDateRange"].Header.Caption = "Horizon Date Range";
				gridBasis.DisplayLayout.Bands[0].Columns["WEIGHT"].Header.VisiblePosition = 4;
				gridBasis.DisplayLayout.Bands[0].Columns["WEIGHT"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Percent);

				//make the "Version" column a drop down list.
				gridBasis.DisplayLayout.Bands[0].Columns["Version"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				gridBasis.DisplayLayout.Bands[0].Columns["Version"].ValueList = gridBasis.DisplayLayout.ValueLists["Version"];
				
				//Set the width of the "DateRange" column so that the DateRangeSelector can fit.
				gridBasis.DisplayLayout.Bands[0].Columns["HorizonDateRange"].Width = 160;
				gridBasis.DisplayLayout.Bands[0].Columns["HorizonDateRange"].Style  = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
				e.Layout.Bands[0].Columns["HorizonDateRange"].CellActivation = Activation.NoEdit;

				//the following code tweaks the "Add New" buttons (which come with the grid).
				gridBasis.DisplayLayout.AddNewBox.Hidden = false;
				gridBasis.DisplayLayout.Bands[0].AddButtonToolTipText = "Click to add new basis details.";
				gridBasis.DisplayLayout.Bands[0].AddButtonCaption = "Basis";
				gridBasis.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;
			}
			catch
			{
				throw;
			}

		}

		private void gridStoreGrades_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{

			try
			{
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
                ugld.ApplyDefaults(e, false);  // Stodd - merge issue
                e.Layout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.Default;
				e.Layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
				gridStoreGrades.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

				//hide the key columns.
				e.Layout.Bands[0].Columns["HDR_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["STORE_GRADE_SEQ"].Hidden = true;

			
				//Prevent the user from re-arranging columns.
				gridStoreGrades.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;

				//if (FunctionSecurity.AllowUpdate)
				//{
				//    gridStoreGrades.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
				//    gridStoreGrades.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
				//}
				//else
				//{
				//    gridStoreGrades.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
				//    gridStoreGrades.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
				//}

				//Set the header captions.
				gridStoreGrades.DisplayLayout.Bands[0].Columns["GRADE_CODE"].Header.VisiblePosition = 1;
				gridStoreGrades.DisplayLayout.Bands[0].Columns["GRADE_CODE"].Header.Caption = "Grade";
				gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_INDEX"].Header.VisiblePosition = 2;
				gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_INDEX"].Header.Caption = "Boundary Index";
				// BEGIN TT#1989-MD - AGallagher - Unit Boundaries are to be hidden
                //gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_UNITS"].Header.VisiblePosition = 3;
                gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_UNITS"].Hidden = true;
                // END TT#1989-MD - AGallagher - Unit Boundaries are to be hidden
				gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_UNITS"].Header.Caption = "Boundary Units";

				//the following code tweaks the "Add New" buttons (which come with the grid).
				gridStoreGrades.DisplayLayout.Bands[0].AddButtonToolTipText = "Click to add new Store Grade.";
				gridStoreGrades.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;
				gridStoreGrades.DisplayLayout.AddNewBox.Hidden = false;
				gridStoreGrades.DisplayLayout.Bands[0].AddButtonCaption = "Store Grade";

				if (radIndexBoundary.Checked)
			    {
					gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_UNITS"].CellActivation = Activation.Disabled;
					gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_UNITS"].Header.Enabled = false;

					gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_INDEX"].CellActivation = Activation.AllowEdit;
					gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_INDEX"].Header.Enabled = true;
                }
				else
                {
                    gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_INDEX"].CellActivation = Activation.Disabled;
                    gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_INDEX"].Header.Enabled = false;

                    gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_UNITS"].CellActivation = Activation.AllowEdit;
                    gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_UNITS"].Header.Enabled = true;
                }
			}
			catch
			{
				throw;
			}
		}

		private void btCancel_Click(object sender, System.EventArgs e)
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
		virtual public void btnOk_Click(object sender, System.EventArgs e)
		{
			try
			{
				_okClicked = true;	// TT#508 - md - stodd - enqueue error
				if (Save(eUpdateMode.Update))
				{
					
                    //Begin TT#1938 - DOConnell - Tried to save and assortment before selecting the OK button and it does not show in the Explorer.
					//Process();
                    //End TT#1938 - DOConnell - Tried to save and assortment before selecting the OK button and it does not show in the Explorer.
					NextWindow(eAllocationSelectionViewType.Assortment);
					this.Close();
                    //BEGIN TT#67-MD - stodd - no longer needed 
					//BEGIN TT#9-MD - DOConnell - Cannot drag and drop header into assortment 
                    //_trans.DequeueHeaders();
					//END TT#9-MD - DOConnell - Cannot drag and drop header into assortment 
                    //END TT#67-MD - stodd - no longer needed
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}	
		}

		protected bool Save(eUpdateMode aUpdateMode)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				//if (ChangePending)
				//{
					if (ValidateSpecificFields())
					{
						if (ValidateFields())
						{
							SetSpecificFields();
							SetBase();
							SaveWindow();
                            //Begin TT#1938 - DOConnell - Tried to save and assortment before selecting the OK button and it does not show in the Explorer.
							// Begin TT#831-MD - stodd - Need, Need%, & VSW Onhand incorrect 
							// move this to AssortmentProperties.NextWindow() so if would occur after the headers were loaded into the transaction.
                            //Process();
							// End TT#831-MD - stodd - Need, Need%, & VSW Onhand incorrect 
                            //End TT#1938 - DOConnell - Tried to save and assortment before selecting the OK button and it does not show in the Explorer.
							//BEGIN TT#9-MD - DOConnell - Cannot drag and drop header into assortment 
                            //_trans.DequeueHeaders();
							//END TT#9-MD - DOConnell - Cannot drag and drop header into assortment
                            ChangePending = false;
							return true;
						}
						else
						{
							return false;
						}
					}
					else
					{
						return false;
					}
				//}
				//else
				//{
				//    return true;
				//}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		

		override protected bool SaveChanges()
		{
			try
			{
				if (Save(eUpdateMode.Update))
				{
					ErrorFound = false;
				}
				else
				{
					ErrorFound = true;
				}

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		virtual protected void NextWindow(eAllocationSelectionViewType viewType)
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


				ApplicationSessionTransaction processTransaction = _trans;

				if (processTransaction == null)
				{
					processTransaction = NewTransFromSelectedHeaders();
				}
				
				if (processTransaction == null)
				{
					return;
				}

				AssortmentViewSelection avs = new AssortmentViewSelection(EAB, SAB, processTransaction, null, false);
				if (this.ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer")
				{
					avs.MdiParent = this.ParentForm;
				}
				else
				{
					avs.MdiParent = this.ParentForm.Owner;
				}
				avs.DetermineWindow(viewType);
			}
			catch (Exception ex)
			{
				HandleException(ex);
				Cursor.Current = Cursors.Default;
			}
		}

		private ApplicationSessionTransaction NewTransFromSelectedHeaders()
		{
			try
			{
				ApplicationSessionTransaction newTrans = SAB.ApplicationServerSession.CreateTransaction();

				//int selHdrCount = _eab.AssortmentWorkspaceExplorer.GetSelectedHeaders();

				//if (selHdrCount > 0)
				//{
					newTrans.NewAllocationMasterProfileList();

					_selectedHeaderKeyList.Clear();
                    _selectedAssortmentKeyList.Clear(); // TT#488 - MD - Jellis - Group Allocation

					if (_loadedFrom == eAssortmentBasisLoadedFrom.AssortmentProperties
                        || _loadedFrom == eAssortmentBasisLoadedFrom.GroupAllocation)
					{
						GetAllHeadersInAssortment(_key);
					}
					else if (_loadedFrom == eAssortmentBasisLoadedFrom.UserSelectionCriteria)
					{


					}

					int[] selectedHeaderArray = new int[_selectedHeaderKeyList.Count];
					_selectedHeaderKeyList.CopyTo(selectedHeaderArray);
                     // begin TT#488 - MD - Jellis - Group Allocation
                    int[] selectedAssortmentArray = new int[_selectedAssortmentKeyList.Count];
                    _selectedAssortmentKeyList.CopyTo(selectedAssortmentArray);
                     // end TT#488 - MD - Jellis - Group Allocation

					// load the selected headers in the Application session transaction
					newTrans.LoadHeaders(selectedAssortmentArray, selectedHeaderArray); // TT#488 - MD - Jellis - Group Allocation
				//}
				return newTrans;
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
                    // begin TT#488 - MD - Jellis - Group Allocation
                    if (hdrRID == aAsrtRID)
                    {
                        _selectedAssortmentKeyList.Add(hdrRID);
                    }
                    else
                    {
                        // end TT#488 - MD - Jellis - Group Allocation
                        _selectedHeaderKeyList.Add(hdrRID);
                    } // TT#488 - MD - Jellis - Group Allocation
				}
			}
			catch
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
                if (_assortmentReviewAssortment.AccessDenied && _assortmentReviewContent.AccessDenied && _assortmentReviewCharacteristic.AccessDenied)
                {
                    string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UnauthorizedFunctionAccess);                                     
                    MessageBox.Show(errorMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                foreach (AllocationHeaderProfile ahp in headerList)
                {
                    nodeFunctionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(ahp.StyleHnRID, eSecurityFunctions.AssortmentReview, (int)eSecurityTypes.Allocation);
                    if (!nodeFunctionSecurity.AllowUpdate)
                    {
                        OKToEnqueue = false;
                        break;
                    }
                }

                if (OKToEnqueue)
                {
					// BEGIN Stodd - 4.0 to 4.1 Manual merge
                    //processTransaction.EnqueueHeaders();
                    //if (processTransaction.HeadersEnqueued)
					string enqMessage = string.Empty;
					//BEGIN TT#9-MD - DOConnell - Cannot drag and drop header into assortment 
					//if (processTransaction.EnqueueSelectedHeaders(out enqMessage))
                    
                    List<int> selectedHdrs = new List<int>();
                    foreach (AllocationHeaderProfile ahp in headerList)
                    {
                        selectedHdrs.Add(ahp.Key);
                    }

                    if (processTransaction.EnqueueHeaders(processTransaction.GetHeadersToEnqueue(selectedHdrs), out enqMessage))
					//END TT#9-MD - DOConnell - Cannot drag and drop header into assortment 
                    {
                        processTransaction.DataState = eDataState.Updatable;
                    }
                    else
                    {
                        processTransaction.DataState = eDataState.ReadOnly;
                    }
					// END Stodd - 4.0 to 4.1 Manual merge
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
                HandleException(ex);
                return false;
            }
		}

		//protected override void AfterClosing()
		//{
		//    try
		//    {
		//        base.AfterClosing();

		//        if (OnAssortmentPropertiesCloseHandler != null)
		//        {
		//            OnAssortmentPropertiesCloseHandler(this, new AssortmentPropertiesCloseEventArgs());
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleException(exc);
		//    }
		//}

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

		private void gridBasis_DragEnter(object sender, DragEventArgs e)
		{
			Image_DragEnter(sender, e);
		}

		private void gridBasis_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			HierarchyNodeProfile hnp;
            TreeNodeClipboardList cbList = null;
            TreeNodeClipboardProfile cbProf = null;
			try
			{
				Infragistics.Win.UIElement aUIElement;

				aUIElement = gridBasis.DisplayLayout.UIElement.ElementFromPoint(gridBasis.PointToClient(new Point(e.X, e.Y)));

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
                        ////object cellValue = null;	
                        if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
						{
                            cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                            //if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode)
                            if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
							{
                                //if (cbp.ClipboardData.GetType() == typeof(HierarchyClipboardData))
                                //{
                                //    MIDTreeNode_cbd = (HierarchyClipboardData)cbp.ClipboardData;

                                hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key);
									_dragAndDrop = true;
									aRow.Cells["HN_RID"].Value = hnp.Key;
									//_skipAfterCellUpdate = true;
									aRow.Cells["Merchandise"].Value = hnp.Text;
									_dragAndDrop = false;
									gridBasis.UpdateData();

                                    if (aRow.Index == 0)  // First row only
                                        LoadMerchandiseStoreGrades(hnp.Key, eGradesLoadedFrom.Basis);
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

		private void gridBasis_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			try
			{
				Image_DragOver(sender, e);
				Infragistics.Win.UIElement aUIElement;
				aUIElement = gridBasis.DisplayLayout.UIElement.ElementFromPoint(gridBasis.PointToClient(new Point(e.X, e.Y)));

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

		private void gridBasis_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			CalendarDateSelector frmCalDtSelector;
			DialogResult dateRangeResult;
			DateRangeProfile selectedDateRange;

			try
			{
				if (e.Cell.Column.Key == "HorizonDateRange")
				{

					frmCalDtSelector = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{SAB});

					if (e.Cell.Row.Cells["HorizonDateRange"].Value != null &&
						e.Cell.Row.Cells["HorizonDateRange"].Value != System.DBNull.Value &&
						e.Cell.Row.Cells["HorizonDateRange"].Text.Length > 0)
					{
						frmCalDtSelector.DateRangeRID = Convert.ToInt32(e.Cell.Row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture);
						// Begin TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
                        DateRangeProfile drp = SAB.ApplicationServerSession.Calendar.GetDateRange(frmCalDtSelector.DateRangeRID);

                        if (drp.DateRangeType == eCalendarRangeType.Dynamic)
                        {
                            if (_anchorDateRangeRid == Include.UndefinedCalendarDateRange)
                            {
                                string msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_as_MissingAnchorDate);
                                MessageBox.Show(msg, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.lbl_Date_Range), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
						// End TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
					}
					// Begin TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
                    else
                    {
                        frmCalDtSelector.DefaultToStatic = true;
                    }

					//frmCalDtSelector.AnchorDate = SAB.ClientServerSession.Calendar.CurrentDate;
					//frmCalDtSelector.AnchorDateRelativeTo = eDateRangeRelativeTo.Current;

					frmCalDtSelector.AllowDynamicToStoreOpen = false;
                    frmCalDtSelector.AllowDynamicToCurrent = false;  //TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
                    frmCalDtSelector.AllowDynamicToPlan = true;
                    if (_anchorDateRangeRid == Include.UndefinedCalendarDateRange)
                    {
                        frmCalDtSelector.AllowDynamicToPlan = false;  //TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
                        frmCalDtSelector.AllowDynamic = false;	// TT#1445-MD - stodd - Calendar Date Range display is incorrect - 
                    }
                    else
                    {
                        frmCalDtSelector.AnchorDateRangeRID = _anchorDateRangeRid;
                        frmCalDtSelector.AnchorDateRelativeTo = eDateRangeRelativeTo.Plan;	// TT#1445-MD - stodd - Calendar Date Range display is incorrect - 
                    }

					// End TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
                    //frmCalDtSelector.AllowDynamicToPlan = false;  //TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
					frmCalDtSelector.StartPosition = FormStartPosition.CenterParent;
                    frmCalDtSelector.AllowDynamicToCurrent = false;  //TT#659 - MD - DOConnell - Delivery and Horizon dates should not default to Dynamic relative to current
                    
					dateRangeResult = frmCalDtSelector.ShowDialog();

					if (dateRangeResult == DialogResult.OK)
					{
						selectedDateRange = (DateRangeProfile)frmCalDtSelector.Tag;

						e.Cell.Value = selectedDateRange.DisplayDate;
						e.Cell.Row.Cells["CDR_RID"].Value = selectedDateRange.Key;

						if (selectedDateRange.DateRangeType == eCalendarRangeType.Dynamic)
						{
							if (selectedDateRange.RelativeTo == eDateRangeRelativeTo.Plan)
							{
								e.Cell.Appearance.Image = _dynamicToPlanImage;
							}
							else
							{
								e.Cell.Appearance.Image = _dynamicToCurrentImage;
							}
						}
						else
						{
							e.Cell.Appearance.Image = null;
						}
                        ChangePending = true;   // TT#1442_MD - stodd - Assortment Properties window doesn't recognize the data change when the basis date is changed
					}

					e.Cell.CancelUpdate();
					gridBasis.PerformAction(UltraGridAction.DeactivateCell);
				}
			
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		private void gridBasis_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			e.Row.Cells["WEIGHT"].Value = 100;
		}

		private void gridBasis_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
                _assortmentBasisChanged = true;   // TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO
			}
		}

		private void cboFilter_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (FormLoaded)
				ChangePending = true;
		}

		private void cboStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (FormLoaded)
				ChangePending = true;
		}

		private void cbxOnhand_Click(object sender, System.EventArgs e)
		{
			if (FormLoaded)
				ChangePending = true;
		}

		private void cbxIntransit_Click(object sender, System.EventArgs e)
		{
			if (FormLoaded)
				ChangePending = true;
		}

		private void cbxCommitted_CheckedChanged(object sender, EventArgs e)
		{
			if (FormLoaded)
				ChangePending = true;
		}

		private void cbxSimilarStores_Click(object sender, System.EventArgs e)
		{
			if (FormLoaded)
				ChangePending = true;
		}

		private void RadioButtonClick_Click(object sender, System.EventArgs e)
		{
			if (FormLoaded)
				ChangePending = true;
		}

		//private void radGlobal_Click(object sender, System.EventArgs e)
		//{
		//    if (radGlobal.Checked)
		//    {
		//        FunctionSecurity = GlobalSecurity;
		//    }
		//    ApplySecurity();
		//}

		//private void radUser_Click(object sender, System.EventArgs e)
		//{
		//    if (radGlobal.Checked)
		//    {
		//        FunctionSecurity = UserSecurity;
		//    }
		//    ApplySecurity();
		//}

		//override protected bool ApplySecurity()	// Track 5871 stodd
		//{
		//    bool securityOk = true; // track #5871 stodd
		//    if (FunctionSecurity.AllowUpdate)
		//    {
		//        btnSave.Enabled = true;
		//    }
		//    else
		//    {
		//        btnSave.Enabled = false;
		//    }

		//    if (FunctionSecurity.AllowExecute)
		//    {
		//        btnProcess.Enabled = true;
		//    }
		//    else
		//    {
		//        btnProcess.Enabled = false;
		//    }
		//    return securityOk;	// track 5871 stodd
		//}

		private void gridBasis_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			if (e.Cell == e.Cell.Row.Cells["Merchandise"])
			{
				if (FormLoaded && !_dragAndDrop)
				{
					if (_merchValChanged)
					{
						_merchValChanged = false;
						return;
					}
					string productID = e.Cell.Value.ToString().Trim();
					if (productID.Length > 0)
					{
						_nodeRid = GetNodeRid(ref productID);
						if (_nodeRid == Include.NoRID)
						{
							string errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
								productID );
							MessageBox.Show(errorMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);		
						}
						else 
						{
							_merchValChanged = true;
							e.Cell.Value = productID;
							e.Cell.Row.Cells["HN_RID"].Value = _nodeRid;

                            if (e.Cell.Row.Index == 0)  // First row only
                                LoadMerchandiseStoreGrades(_nodeRid, eGradesLoadedFrom.Basis);
						}
					}
				}
			}
			if (e.Cell == e.Cell.Row.Cells["Version"])
			{
				int selectedIndex = gridBasis.DisplayLayout.ValueLists["Version"].SelectedIndex;

				if (selectedIndex != -1)
					e.Cell.Row.Cells["FV_RID"].Value = Convert.ToInt32(gridBasis.DisplayLayout.ValueLists["Version"].ValueListItems[selectedIndex].DataValue, CultureInfo.CurrentUICulture);
			}
		}

		private void gridBasis_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
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

		private void gridStoreGrades_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
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

		private void gridStoreGrades_CellChange(object sender, CellEventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		private void menuGrid_Popup(object sender, System.EventArgs e)
		{
			
		}

		private void menuItemDelete_Click(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
			_mouseDownGrid.DeleteSelectedRows(true);
		}

		private void menuItemInsert_Click(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
			_mouseDownGrid.DisplayLayout.Bands[0].AddNew();
		}
//		private void menuItemSort_Click(object sender, System.EventArgs e)
//		{
//			if (gridStoreGrades.Rows.Count > 0)
//			{
//				if (gridStoreGrades.Rows[0].Cells[1].Value != null)
//					gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_INDEX"].SortIndicator = SortIndicator.Descending;
//				else if (gridStoreGrades.Rows[0].Cells[2].Value != null)
//					gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_UNITS"].SortIndicator = SortIndicator.Descending;
//			}
//		}

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

		private void radIndexBoundary_CheckedChanged(object sender, System.EventArgs e)
		{
            if (radIndexBoundary.Checked)
            {
				gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_UNITS"].CellActivation = Activation.Disabled;
				gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_UNITS"].Header.Enabled = false;

				gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_INDEX"].CellActivation = Activation.AllowEdit;
				gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_INDEX"].Header.Enabled = true;
			}
			else
            {
                gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_INDEX"].CellActivation = Activation.Disabled;
                gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_INDEX"].Header.Enabled = false;

                gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_UNITS"].CellActivation = Activation.AllowEdit;
                gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_UNITS"].Header.Enabled = true;
            }
		}

		private void radUnitsBoundary_CheckedChanged(object sender, System.EventArgs e)
		{
		
		}

		private void gridStoreGrades_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
            TreeNodeClipboardList cbList = null;
			try
			{
                //ClipboardProfile cbp = GetClipboardData(eClipboardDataType.HierarchyNode);
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
					//if (FunctionSecurity.IsReadOnly)
					//{
					//    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));

					//}
					//else
                    {
                        LoadStoreGrades(cbList.ClipboardProfile.Key);
                    }
                }
			}
			catch (BadDataInClipboardException)
			{
				MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
					this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

//        private void StoreGrades_Populate(int nodeRID)
//        {
//            try
//            {
//                int count = 0;

//                _asp.StoreGradeList.Clear();
//                //_assortmentGeneralMethod.StoreGradesDataTable.Clear();
//                //_assortmentGeneralMethod.StoreGradesDataTable.AcceptChanges();
			
//                _storeGradeList = SAB.HierarchyServerSession.GetStoreGradeList(nodeRID, false, true);
//                //bool[,] cellIsNull = new Boolean [_storeGradeList.Count,5]; 
//                int seq = 0;
//                foreach(StoreGradeProfile sgp in _storeGradeList)
//                {
//                    _dtStoreGrades.Rows.Add(new object[] { _asp.Key, seq++, sgp.Boundary,   
//                                                                                DBNull.Value, sgp.StoreGrade,});
//                    ++count;
//                }
//                gridStoreGrades.DataSource = _dtStoreGrades;
////				for (int i = 0; i <  _storeGradeList.Count; i++)
////				{
////					for (int j = 0; j < 5; j++)
////					{
////						if (cellIsNull[i,j])
////						{
////							ugStoreGrades.Rows[i].Cells[j+3].Value = System.DBNull.Value;
////						}
////					}	
////				}

//                FunctionSecurityProfile securityLevel = FunctionSecurity;
				
//                if (FunctionSecurity.AllowUpdate)
//                {
//                    this.gridStoreGrades.DisplayLayout.AddNewBox.Hidden = false;
//                }
//                else
//                {
//                    this.gridStoreGrades.DisplayLayout.AddNewBox.Hidden = true;
//                }
				
//            }
//            catch( Exception exception )
//            {
//                HandleException(exception);
//            }
//        }

		private void gridStoreGrades_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			try
			{
				ObjectDragEnter(e);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void radReceipts_CheckedChanged(object sender, EventArgs e)
		{
            if ((this.radReceipts.Checked == true) && (this.radReceipts.Focused == true))  // TT#2116-MD - AGallagher - Assortment Review Navigation
			{
				this.cbxIntransit.Enabled = true;
				this.cbxOnhand.Enabled = true;
			}
		}

		private void radSales_CheckedChanged(object sender, EventArgs e)
		{
            if ((this.radSales.Checked == true) && (this.radSales.Focused == true))  // TT#2116-MD - AGallagher - Assortment Review Navigation
			{
				this.cbxIntransit.Enabled = false;
				this.cbxOnhand.Enabled = false;
				this.cbxIntransit.Checked = false;
				this.cbxOnhand.Checked = false;
			}
		}

		private void radStock_CheckedChanged(object sender, EventArgs e)
		{
			if ((this.radStock.Checked == true) && (this.radStock.Focused == true))  // TT#2116-MD - AGallagher - Assortment Review Navigation
            {
				this.cbxIntransit.Enabled = true;
				this.cbxOnhand.Enabled = true;
			}
		}

		private void tabControl2_DragEnter(object sender, DragEventArgs e)
		{
			Image_DragEnter(sender, e);
		}

		private void tabControl2_DragOver(object sender, DragEventArgs e)
		{
			Image_DragOver(sender, e);
		}

		//public void HandleExceptions(Exception exc)
		//{
		//    Debug.WriteLine(exc.ToString());
		//    MessageBox.Show(exc.ToString());
		//}

//		private void StoreGrades_Define()
//		{
//			try
//			{
        //				_storeGradesDataTable = MIDEnvironment.CreateDataTable("storeGradesDataTable");
//
//			
//				DataColumn dataColumn;
//
//				//Create Columns and rows for datatable
//				dataColumn = new DataColumn();
//				dataColumn.DataType = System.Type.GetType("System.Int32");
//				dataColumn.ColumnName = "RowPosition";
//				dataColumn.Caption = "RowPosition";
//				dataColumn.ReadOnly = false;
//				dataColumn.Unique = false;
//				dataColumn.AllowDBNull = true;
//				_storeGradesDataTable.Columns.Add(dataColumn);
//
//				dataColumn = new DataColumn();
//				dataColumn.DataType = System.Type.GetType("System.String");
//				dataColumn.ColumnName = "Grade";
//				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Grade);
//				dataColumn.ReadOnly = false;
//				dataColumn.Unique = true;
//				dataColumn.AllowDBNull = true;
//				_storeGradesDataTable.Columns.Add(dataColumn);
//
//				dataColumn = new DataColumn();
//				dataColumn.DataType = System.Type.GetType("System.Int32");
//				dataColumn.ColumnName = "Boundary";
//				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Boundary);
//				dataColumn.ReadOnly = false;
//				dataColumn.Unique = false;
//				dataColumn.AllowDBNull = true;
//				_storeGradesDataTable.Columns.Add(dataColumn);
//
//				dataColumn = new DataColumn();
//				dataColumn.DataType = System.Type.GetType("System.Int32");
//				dataColumn.ColumnName = "WOS Index";
//				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_WOS_Index);
//				dataColumn.ReadOnly = false;
//				dataColumn.Unique = false;
//				dataColumn.AllowDBNull = true;
//				_storeGradesDataTable.Columns.Add(dataColumn);
//
//				dataColumn = new DataColumn();
//				dataColumn.DataType = System.Type.GetType("System.String");
//				dataColumn.ColumnName = "Min Stock";
//				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Min_Stock);
//				dataColumn.ReadOnly = false;
//				dataColumn.Unique = false;
//				dataColumn.AllowDBNull = true;
//				_storeGradesDataTable.Columns.Add(dataColumn);
//
//				dataColumn = new DataColumn();
//				dataColumn.DataType = System.Type.GetType("System.String");
//				dataColumn.ColumnName = "Max Stock";
//				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Max_Stock);
//				dataColumn.ReadOnly = false;
//				dataColumn.Unique = false;
//				dataColumn.AllowDBNull = true;
//				_storeGradesDataTable.Columns.Add(dataColumn);
//
//				dataColumn = new DataColumn();
//				dataColumn.DataType = System.Type.GetType("System.String");
//				dataColumn.ColumnName = "Min Ad";
//				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Min_Ad);
//				dataColumn.ReadOnly = false;
//				dataColumn.Unique = false;
//				dataColumn.AllowDBNull = true;
//				_storeGradesDataTable.Columns.Add(dataColumn);
//
//				//			dataColumn = new DataColumn();
//				//			dataColumn.DataType = System.Type.GetType("System.String");
//				//			dataColumn.ColumnName = "Max Ad";
//				//			dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Max_Ad);
//				//			dataColumn.ReadOnly = false;
//				//			dataColumn.Unique = false;
//				//			_storeGradesDataTable.Columns.Add(dataColumn);
//
//				dataColumn = new DataColumn();
//				dataColumn.DataType = System.Type.GetType("System.String");
//				dataColumn.ColumnName = "Color Min";
//				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Color_Min);
//				dataColumn.ReadOnly = false;
//				dataColumn.Unique = false;
//				dataColumn.AllowDBNull = true;
//				_storeGradesDataTable.Columns.Add(dataColumn);
//
//				dataColumn = new DataColumn();
//				dataColumn.DataType = System.Type.GetType("System.String");
//				dataColumn.ColumnName = "Color Max";
//				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Color_Max);
//				dataColumn.ReadOnly = false;
//				dataColumn.Unique = false;
//				dataColumn.AllowDBNull = true;
//				_storeGradesDataTable.Columns.Add(dataColumn);
//
//				//make grade column the primary key
//				DataColumn[] PrimaryKeyColumn = new DataColumn[1];
//				PrimaryKeyColumn[0] = _storeGradesDataTable.Columns["Grade"];
//				_storeGradesDataTable.PrimaryKey = PrimaryKeyColumn;
//			}
//			catch( Exception exception )
//			{
//				HandleException(exception);
//			}
//		}

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

	
		private void CalcStoreIndex(int setKey, ProfileList storeList, List<int> valueList, double storeAvg)
		{
			try
			{
				//=====================
				// Calc Store Indexes
				//=====================
				foreach (StoreProfile store in storeList.ArrayList)
				{
					if (_storeToListHash.ContainsKey(store.Key))
					{
						int idx = (int)_storeToListHash[store.Key];
						// Save store set
						_storeSetList[idx] = setKey;
						double storeIndex = 0.0d;
						if (storeAvg != 0.0d)
						{
							storeIndex = (valueList[idx] * 100) / storeAvg;
						}
						_storeIndexList[idx] = (int)storeIndex;
						_avgStoreList[idx] = (int)storeAvg;
					}
				}
				_setAvgHash.Add(setKey, storeAvg);
			}
			catch
			{
				throw;
			}
		}

		private double CalcStoreAverage(int setKey, ProfileList storeList, List<int> valueList)
		{
			try
			{
				//====================
				// Calc Average Store
				//====================
				double totalAmt = 0;
				double storeCount = 0;
				double avg = 0;
				foreach (StoreProfile store in storeList.ArrayList)
				{
					//==========================================================================
					// If all the stores in a set is sent to this method, some of the stores
					// may not be in out filtered list. We check first thing to see if the
					// store is in our list.
					//==========================================================================
					if (_storeToListHash.ContainsKey(store.Key))
					{
						int idx = (int)_storeToListHash[store.Key];
						//if (valueList[idx] > 0)
						//{
						totalAmt += valueList[idx];
						storeCount++;
						//}
					}
				}
				if (storeCount > 0)
				{
					avg = (int)((totalAmt / storeCount) + .5);
				}

				_setTotalHash.Add(setKey, (int)totalAmt);
				_setStoreCountHash.Add(setKey, (int)storeCount);

				return avg;
			}
			catch
			{
				throw;
			}
		}

		private void InitStoreLists()
		{
			_storeSetList = new List<int>(_storeList.Count);
			_storeIndexList = new List<int>(_storeList.Count);
			_avgStoreList = new List<int>(_storeList.Count);
			_storeIntransitList = new List<int>(_storeList.Count);
			_storeNeedList = new List<int>(_storeList.Count);
			_storePctNeedList = new List<decimal>(_storeList.Count);

			foreach (int i in _storeList)
			{
				_storeSetList.Add(0);
				_storeIndexList.Add(0);
				_avgStoreList.Add(0);
				_storeIntransitList.Add(0);
				_storeNeedList.Add(0);
				_storePctNeedList.Add(0);
			}
		}

		

		
	}


}
