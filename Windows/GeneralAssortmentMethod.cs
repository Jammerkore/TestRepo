using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Collections;
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
	public class frmGeneralAssortment : WorkflowMethodFormBase
	{
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabMethod;
		private System.Windows.Forms.TabPage tabProperties;
		private System.Windows.Forms.TabControl tabControl2;
		private System.Windows.Forms.TabPage tabBasis;
		private System.Windows.Forms.TabPage tabStoreGrades;
		private System.Windows.Forms.ComboBox cboFilter;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtReserve;
		private System.Windows.Forms.RadioButton radPercent;
        private System.Windows.Forms.RadioButton radUnits;
		private System.Windows.Forms.CheckBox cbxOnhand;
		private System.Windows.Forms.CheckBox cbxIntransit;
		private System.Windows.Forms.CheckBox cbxSimilarStores;
		private System.Windows.Forms.GroupBox groupBox3;
		private Infragistics.Win.UltraWinGrid.UltraGrid gridBasis;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.RadioButton radReceipts;
		private System.Windows.Forms.RadioButton radSales;
		private Infragistics.Win.UltraWinGrid.UltraGrid gridStoreGrades;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.RadioButton radIndexBoundary;
		private System.Windows.Forms.RadioButton radUnitsBoundary;
        private System.Windows.Forms.RadioButton radStock;
		private System.Windows.Forms.Label lblReserve;
		private System.Windows.Forms.Label lblFilter;
		private System.Windows.Forms.Label lblVariable;
		private MIDWorkflowMethodTreeNode _explorerNode = null;
		private GeneralAssortmentMethod _assortmentGeneralMethod;
		private ProfileList _storeGroupList;
		private System.Windows.Forms.ImageList Icons;
        //private DataTable _dtWorkflows;
		private DataTable _dtBasis;
        private DataTable _dtStoreGrades;
		private ProfileList _versionProfList;
		private Image _dynamicToPlanImage;
		private Image _dynamicToCurrentImage;
		private bool _dragAndDrop = false;
		private bool _merchValChanged = false;
		private System.Windows.Forms.ContextMenu menuGrid;
		private System.Windows.Forms.MenuItem menuItemDelete;
		private System.Windows.Forms.MenuItem menuItemInsert;
		private int _nodeRid;
		private System.Windows.Forms.RadioButton radSet;
		private System.Windows.Forms.RadioButton radAllStore;
		private System.Windows.Forms.GroupBox gbAverage;
		private System.Windows.Forms.Label lblAttribute;
        // Begin Track #4872 - JSmith - Global/User Attributes
        //private System.Windows.Forms.ComboBox cboStoreAttribute;
        private MIDAttributeComboBox cboStoreAttribute;
        // End Track #4872
		private UltraGrid _mouseDownGrid;
		private StoreGradeList _storeGradeList;

        public frmGeneralAssortment(SessionAddressBlock SAB, ExplorerAddressBlock aEAB)
            : base(SAB, aEAB, eMIDTextCode.frm_GeneralAssortmentMethod, eWorkflowMethodType.Method)
		{
			AllowDragDrop = true;
			InitializeComponent();

			UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentMethodsUserGeneralAssortment);
			GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentMethodsGlobalGeneralAssortment);
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
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabMethod = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblReserve = new System.Windows.Forms.Label();
            this.txtReserve = new System.Windows.Forms.TextBox();
            this.radPercent = new System.Windows.Forms.RadioButton();
            this.radUnits = new System.Windows.Forms.RadioButton();
            this.cbxSimilarStores = new System.Windows.Forms.CheckBox();
            this.cbxIntransit = new System.Windows.Forms.CheckBox();
            this.cbxOnhand = new System.Windows.Forms.CheckBox();
            this.cboFilter = new System.Windows.Forms.ComboBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabBasis = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radStock = new System.Windows.Forms.RadioButton();
            this.radSales = new System.Windows.Forms.RadioButton();
            this.radReceipts = new System.Windows.Forms.RadioButton();
            this.lblVariable = new System.Windows.Forms.Label();
            this.gridBasis = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.menuGrid = new System.Windows.Forms.ContextMenu();
            this.menuItemDelete = new System.Windows.Forms.MenuItem();
            this.menuItemInsert = new System.Windows.Forms.MenuItem();
            this.tabStoreGrades = new System.Windows.Forms.TabPage();
            this.gbAverage = new System.Windows.Forms.GroupBox();
            this.radAllStore = new System.Windows.Forms.RadioButton();
            this.radSet = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radUnitsBoundary = new System.Windows.Forms.RadioButton();
            this.radIndexBoundary = new System.Windows.Forms.RadioButton();
            this.gridStoreGrades = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboStoreAttribute = new MIDAttributeComboBox();
            this.lblAttribute = new System.Windows.Forms.Label();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.Icons = new System.Windows.Forms.ImageList(this.components);
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabMethod.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabBasis.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBasis)).BeginInit();
            this.tabStoreGrades.SuspendLayout();
            this.gbAverage.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridStoreGrades)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(639, 563);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(558, 563);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(13, 563);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabMethod);
            this.tabControl1.Controls.Add(this.tabProperties);
            this.tabControl1.Location = new System.Drawing.Point(9, 64);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(710, 493);
            this.tabControl1.TabIndex = 3;
            // 
            // tabMethod
            // 
            this.tabMethod.Controls.Add(this.groupBox1);
            this.tabMethod.Controls.Add(this.cbxSimilarStores);
            this.tabMethod.Controls.Add(this.cbxIntransit);
            this.tabMethod.Controls.Add(this.cbxOnhand);
            this.tabMethod.Controls.Add(this.cboFilter);
            this.tabMethod.Controls.Add(this.lblFilter);
            this.tabMethod.Controls.Add(this.tabControl2);
            this.tabMethod.Controls.Add(this.groupBox3);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(702, 467);
            this.tabMethod.TabIndex = 0;
            this.tabMethod.Text = "Method";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblReserve);
            this.groupBox1.Controls.Add(this.txtReserve);
            this.groupBox1.Controls.Add(this.radPercent);
            this.groupBox1.Controls.Add(this.radUnits);
            this.groupBox1.Location = new System.Drawing.Point(232, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 37);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
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
            this.txtReserve.TabIndex = 1;
            this.txtReserve.TextChanged += new System.EventHandler(this.txtReserve_TextChanged);
            // 
            // radPercent
            // 
            this.radPercent.Location = new System.Drawing.Point(143, 10);
            this.radPercent.Name = "radPercent";
            this.radPercent.Size = new System.Drawing.Size(62, 24);
            this.radPercent.TabIndex = 2;
            this.radPercent.Text = "Percent";
            this.radPercent.Click += new System.EventHandler(this.RadioButtonClick_Click);
            // 
            // radUnits
            // 
            this.radUnits.Location = new System.Drawing.Point(225, 10);
            this.radUnits.Name = "radUnits";
            this.radUnits.Size = new System.Drawing.Size(62, 24);
            this.radUnits.TabIndex = 3;
            this.radUnits.Text = "Units";
            this.radUnits.Click += new System.EventHandler(this.RadioButtonClick_Click);
            // 
            // cbxSimilarStores
            // 
            this.cbxSimilarStores.Location = new System.Drawing.Point(342, 440);
            this.cbxSimilarStores.Name = "cbxSimilarStores";
            this.cbxSimilarStores.Size = new System.Drawing.Size(90, 19);
            this.cbxSimilarStores.TabIndex = 9;
            this.cbxSimilarStores.Text = "Similar Stores";
            this.cbxSimilarStores.Click += new System.EventHandler(this.cbxSimilarStores_Click);
            // 
            // cbxIntransit
            // 
            this.cbxIntransit.Location = new System.Drawing.Point(230, 440);
            this.cbxIntransit.Name = "cbxIntransit";
            this.cbxIntransit.Size = new System.Drawing.Size(68, 19);
            this.cbxIntransit.TabIndex = 8;
            this.cbxIntransit.Text = "Intransit";
            this.cbxIntransit.Click += new System.EventHandler(this.cbxIntransit_Click);
            // 
            // cbxOnhand
            // 
            this.cbxOnhand.Location = new System.Drawing.Point(77, 440);
            this.cbxOnhand.Name = "cbxOnhand";
            this.cbxOnhand.Size = new System.Drawing.Size(109, 19);
            this.cbxOnhand.TabIndex = 7;
            this.cbxOnhand.Text = "Current On Hand";
            this.cbxOnhand.Click += new System.EventHandler(this.cbxOnhand_Click);
            // 
            // cboFilter
            // 
            this.cboFilter.Location = new System.Drawing.Point(75, 25);
            this.cboFilter.Name = "cboFilter";
            this.cboFilter.Size = new System.Drawing.Size(144, 21);
            this.cboFilter.TabIndex = 2;
            this.cboFilter.SelectedIndexChanged += new System.EventHandler(this.cboFilter_SelectedIndexChanged);
            // 
            // lblFilter
            // 
            this.lblFilter.Location = new System.Drawing.Point(16, 28);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(40, 16);
            this.lblFilter.TabIndex = 1;
            this.lblFilter.Text = "Filter:";
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
            this.tabControl2.Size = new System.Drawing.Size(676, 325);
            this.tabControl2.TabIndex = 0;
            this.tabControl2.DragOver += new System.Windows.Forms.DragEventHandler(this.tabControl2_DragOver);
            this.tabControl2.DragEnter += new System.Windows.Forms.DragEventHandler(this.tabControl2_DragEnter);
            // 
            // tabBasis
            // 
            this.tabBasis.Controls.Add(this.panel2);
            this.tabBasis.Controls.Add(this.gridBasis);
            this.tabBasis.Location = new System.Drawing.Point(4, 22);
            this.tabBasis.Name = "tabBasis";
            this.tabBasis.Size = new System.Drawing.Size(668, 299);
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
            this.radStock.Click += new System.EventHandler(this.RadioButtonClick_Click);
            this.radStock.CheckedChanged += new System.EventHandler(this.radStock_CheckedChanged);
            // 
            // radSales
            // 
            this.radSales.Location = new System.Drawing.Point(150, 7);
            this.radSales.Name = "radSales";
            this.radSales.Size = new System.Drawing.Size(56, 15);
            this.radSales.TabIndex = 2;
            this.radSales.Text = "Sales";
            this.radSales.Click += new System.EventHandler(this.RadioButtonClick_Click);
            this.radSales.CheckedChanged += new System.EventHandler(this.radSales_CheckedChanged);
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
            this.radReceipts.Click += new System.EventHandler(this.RadioButtonClick_Click);
            this.radReceipts.CheckedChanged += new System.EventHandler(this.radReceipts_CheckedChanged);
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
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.gridBasis.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.gridBasis.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.gridBasis.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.gridBasis.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.gridBasis.DisplayLayout.Override.CellAppearance = appearance8;
            this.gridBasis.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.gridBasis.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.gridBasis.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.gridBasis.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.gridBasis.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.gridBasis.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.gridBasis.DisplayLayout.Override.RowAppearance = appearance11;
            this.gridBasis.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.gridBasis.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.gridBasis.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.gridBasis.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.gridBasis.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.gridBasis.Location = new System.Drawing.Point(14, 41);
            this.gridBasis.Name = "gridBasis";
            this.gridBasis.Size = new System.Drawing.Size(641, 257);
            this.gridBasis.TabIndex = 0;
            this.gridBasis.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridBasis_ClickCellButton);
            this.gridBasis.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridMouseDown);
            this.gridBasis.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridBasis_InitializeLayout);
            this.gridBasis.DragOver += new System.Windows.Forms.DragEventHandler(this.gridBasis_DragOver);
            this.gridBasis.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.gridBasis_AfterRowInsert);
            this.gridBasis.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridBasis_AfterCellUpdate);
            this.gridBasis.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridBasis_CellChange);
            this.gridBasis.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.gridBasis_MouseEnterElement);
            this.gridBasis.DragDrop += new System.Windows.Forms.DragEventHandler(this.gridBasis_DragDrop);
            this.gridBasis.DragEnter += new System.Windows.Forms.DragEventHandler(this.gridBasis_DragEnter);
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
            // tabStoreGrades
            // 
            this.tabStoreGrades.Controls.Add(this.gbAverage);
            this.tabStoreGrades.Controls.Add(this.groupBox4);
            this.tabStoreGrades.Controls.Add(this.gridStoreGrades);
            this.tabStoreGrades.Location = new System.Drawing.Point(4, 22);
            this.tabStoreGrades.Name = "tabStoreGrades";
            this.tabStoreGrades.Size = new System.Drawing.Size(520, 301);
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
            // 
            // radSet
            // 
            this.radSet.Location = new System.Drawing.Point(91, 17);
            this.radSet.Name = "radSet";
            this.radSet.Size = new System.Drawing.Size(45, 17);
            this.radSet.TabIndex = 1;
            this.radSet.Text = "Set";
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
            // 
            // radUnitsBoundary
            // 
            this.radUnitsBoundary.Location = new System.Drawing.Point(73, 18);
            this.radUnitsBoundary.Name = "radUnitsBoundary";
            this.radUnitsBoundary.Size = new System.Drawing.Size(55, 15);
            this.radUnitsBoundary.TabIndex = 1;
            this.radUnitsBoundary.Text = "Units";
            this.radUnitsBoundary.Click += new System.EventHandler(this.RadioButtonClick_Click);
            this.radUnitsBoundary.CheckedChanged += new System.EventHandler(this.radUnitsBoundary_CheckedChanged);
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
            this.radIndexBoundary.Click += new System.EventHandler(this.RadioButtonClick_Click);
            this.radIndexBoundary.CheckedChanged += new System.EventHandler(this.radIndexBoundary_CheckedChanged);
            // 
            // gridStoreGrades
            // 
            this.gridStoreGrades.AllowDrop = true;
            this.gridStoreGrades.ContextMenu = this.menuGrid;
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.gridStoreGrades.DisplayLayout.Appearance = appearance13;
            this.gridStoreGrades.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.gridStoreGrades.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.gridStoreGrades.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridStoreGrades.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.gridStoreGrades.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.gridStoreGrades.DisplayLayout.GroupByBox.Hidden = true;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridStoreGrades.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.gridStoreGrades.DisplayLayout.MaxColScrollRegions = 1;
            this.gridStoreGrades.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gridStoreGrades.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.gridStoreGrades.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.gridStoreGrades.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.gridStoreGrades.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.gridStoreGrades.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.gridStoreGrades.DisplayLayout.Override.CellAppearance = appearance20;
            this.gridStoreGrades.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.gridStoreGrades.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.gridStoreGrades.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlignAsString = "Left";
            this.gridStoreGrades.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.gridStoreGrades.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.gridStoreGrades.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.gridStoreGrades.DisplayLayout.Override.RowAppearance = appearance23;
            this.gridStoreGrades.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.gridStoreGrades.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.gridStoreGrades.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.gridStoreGrades.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.gridStoreGrades.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.gridStoreGrades.Location = new System.Drawing.Point(14, 49);
            this.gridStoreGrades.Name = "gridStoreGrades";
            this.gridStoreGrades.Size = new System.Drawing.Size(494, 249);
            this.gridStoreGrades.TabIndex = 0;
            this.gridStoreGrades.TabStop = false;
            this.gridStoreGrades.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridMouseDown);
            this.gridStoreGrades.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridStoreGrades_InitializeLayout);
            this.gridStoreGrades.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.gridStoreGrades_MouseEnterElement);
            this.gridStoreGrades.DragDrop += new System.Windows.Forms.DragEventHandler(this.gridStoreGrades_DragDrop);
            this.gridStoreGrades.DragEnter += new System.Windows.Forms.DragEventHandler(this.gridStoreGrades_DragEnter);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cboStoreAttribute);
            this.groupBox3.Controls.Add(this.lblAttribute);
            this.groupBox3.Location = new System.Drawing.Point(9, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(530, 86);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.Location = new System.Drawing.Point(66, 50);
            this.cboStoreAttribute.Name = "cboStoreAttribute";
            this.cboStoreAttribute.Size = new System.Drawing.Size(144, 21);
            this.cboStoreAttribute.TabIndex = 9;
            // 
            // lblAttribute
            // 
            this.lblAttribute.Location = new System.Drawing.Point(7, 52);
            this.lblAttribute.Name = "lblAttribute";
            this.lblAttribute.Size = new System.Drawing.Size(53, 17);
            this.lblAttribute.TabIndex = 8;
            this.lblAttribute.Text = "Attribute:";
            // 
            // tabProperties
            // 
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(544, 469);
            this.tabProperties.TabIndex = 1;
            this.tabProperties.Text = "Properties";
            // 
            // Icons
            // 
            this.Icons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.Icons.ImageSize = new System.Drawing.Size(16, 16);
            this.Icons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // frmGeneralAssortment
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(726, 598);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmGeneralAssortment";
            this.Text = "General Assortment Method";
            this.Load += new System.EventHandler(this.frmGeneralAssortment_Load);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.pnlGlobalUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabMethod.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabBasis.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridBasis)).EndInit();
            this.tabStoreGrades.ResumeLayout(false);
            this.gbAverage.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridStoreGrades)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void frmGeneralAssortment_Load(object sender, System.EventArgs e)
		{
			SetScreenText();
			// Removes the Properties Tab (for workflows) until
			// Assortment workflows are available.
			this.tabControl1.TabPages.Remove(this.tabProperties);

            // Begin Track #4872 - JSmith - Global/User Attributes
            if (cboStoreAttribute.ReplaceAttribute)
            {
                ChangePending = true;
            }
            // End Track #4872
		}

		/// <summary>
		/// Fills in the text for all of the screen objects
		/// </summary>
		private void SetScreenText()
		{
			tabMethod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Method);
			tabProperties.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Properties);
			lblFilter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter);
			lblReserve.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Reserve);
			radPercent.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Percent);
			radUnits.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Units);
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

		/// <summary>
		/// Opens a new General Assortment Method.
		/// </summary>
		override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
		{
			try
			{
				_assortmentGeneralMethod = new GeneralAssortmentMethod(SAB,Include.NoRID);
                ABM = _assortmentGeneralMethod;
                base.NewWorkflowMethod(aParentNode, eSecurityFunctions.AllocationMethodsUserGeneralAssortment, eSecurityFunctions.AllocationMethodsGlobalGeneralAssortment);
				
				this.radStock.Checked = true;
				
				Common_Load(aParentNode.GlobalUserType);

				GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
				gop.LoadOptions();
				cboStoreAttribute.SelectedValue = gop.OTSPlanStoreGroupRID;

			}
			catch(Exception ex)
			{
				HandleException(ex, "GeneralAssortmentMethod Constructor");
				FormLoadError = true;
			}
		}

		/// <summary>
		/// Opens an existing General Assortment Method.
		/// </summary>
		/// <param name="aMethodRID">aMethodRID</param>
		/// <param name="aLockStatus">The lock status of the data to be displayed</param>
		override public void UpdateWorkflowMethod(int aMethodRID, int aNodeRID, MIDWorkflowMethodTreeNode aNode, eLockStatus aLockStatus)
		{
			try
			{       
				_assortmentGeneralMethod = new GeneralAssortmentMethod(SAB,aMethodRID);
			
				Common_Load(aNode.GlobalUserType);
			}
			catch (Exception err)
			{
				HandleException(err);
				FormLoadError = true;
			}
		}

		/// <summary>
		/// Deletes a General Allocation Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		override public bool DeleteWorkflowMethod(int aMethodRID)
		{
			try
			{       
				_assortmentGeneralMethod = new GeneralAssortmentMethod(SAB,aMethodRID);
                return Delete();
			}
			catch(DatabaseForeignKeyViolation)
			{
				throw;
			}
			catch (Exception ex)
            {
                HandleException(ex, "DeleteWorkflowMethod");
                FormLoadError = true;
            }

            return true;
		}

		/// <summary>
		/// Renames a General Allocation Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <param name="aNewName">The new name of the workflow or method</param>
		override public bool RenameWorkflowMethod(int aMethodRID, string aNewName)
		{
			try
			{
                _assortmentGeneralMethod = new GeneralAssortmentMethod(SAB, aMethodRID);
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
			try
			{       
//				_allocationGeneralMethod = new AllocationGeneralMethod(SAB,aMethodRID);
//				ProcessAction(eMethodType.GeneralAllocation, true);
			}
			catch (Exception ex)
			{
				HandleException(ex);
				FormLoadError = true;
			}
		}

		private void Common_Load(eGlobalUserType aGlobalUserType)//9-17
		{
			try
			{
				//SetText();	
				_versionProfList = SAB.ClientServerSession.GetUserForecastVersions();
				_dynamicToPlanImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToPlanImage);
				_dynamicToCurrentImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToCurrentImage);

				Name = MIDText.GetTextOnly((int)eMethodType.GeneralAssortment);
				if (_assortmentGeneralMethod.Method_Change_Type == eChangeType.add)
				{
					LoadCombos();
					LoadGrids();
				}
				else
				{
					if (FunctionSecurity.AllowUpdate)
					{
						LoadMethod();
					}
					else
					{
						LoadMethod();
					}
				}

				if (!FunctionSecurity.AllowUpdate)
				{
					foreach (UltraGridBand ugr in gridBasis.DisplayLayout.Bands)
					{
						ugr.Override.AllowDelete = DefaultableBoolean.False;
					}
					foreach (UltraGridBand ugr in gridStoreGrades.DisplayLayout.Bands)
					{
						ugr.Override.AllowDelete = DefaultableBoolean.False;
					}
				}

				if (_assortmentGeneralMethod.ReserveAmount == Include.UndefinedReserve)
				{
					this.radPercent.Enabled = false;
					this.radUnits.Enabled = false;
				}

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

		private void LoadCombos()
		{
			try
			{
				LoadFilterCombo();
				LoadAttributeCombo();
				LoadVersionValuelist();
			}
			catch
			{
				throw;
			}
		}

		private void LoadFilterCombo()
		{
			try
			{
				StoreFilterData storeFilterData = new StoreFilterData();

				ArrayList userRIDList = new ArrayList();
				userRIDList.Add(Include.GlobalUserRID);
				userRIDList.Add(SAB.ClientServerSession.UserRID);
				DataTable dtStoreFilter = storeFilterData.StoreFilter_Read(userRIDList);

				// Add 'empty' or 'none' row
				cboFilter.Items.Add(new FilterNameCombo(Include.NoRID, Include.GlobalUserRID," "));	// Issue 3806
				
				foreach (DataRow row in dtStoreFilter.Rows)
				{
					cboFilter.Items.Add(
						new FilterNameCombo(Convert.ToInt32(row["STORE_FILTER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToString(row["STORE_FILTER_NAME"], CultureInfo.CurrentUICulture)));
				}
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
                // Begin Track #4872 - JSmith - Global/User Attributes
                //ProfileList _storeGroupList = SAB.StoreServerSession.GetStoreGroupListViewList();
                ProfileList _storeGroupList = GetStoreGroupList(_assortmentGeneralMethod.Method_Change_Type, _assortmentGeneralMethod.GlobalUserType, false);
                cboStoreAttribute.Initialize(SAB, FunctionSecurity, _storeGroupList.ArrayList, _assortmentGeneralMethod.GlobalUserType == eGlobalUserType.User);

                //this.cboStoreAttribute.ValueMember = "Key";
                //this.cboStoreAttribute.DisplayMember = "Name";
                //this.cboStoreAttribute.DataSource = _storeGroupList.ArrayList;
                // End Track #4872
			}
			catch
			{
				throw;
			}
		}

		private void LoadVersionValuelist()
		{
			//Add a list to the grids, and name it "Version".
			gridBasis.DisplayLayout.ValueLists.Add("Version");
		
			//Loop through the user version list and manually add value and text to the lists.
			for (int i = 0; i < _versionProfList.Count; i++)
			{
				VersionProfile vp = (VersionProfile)_versionProfList[i];
				Infragistics.Win.ValueListItem vli = new Infragistics.Win.ValueListItem();

				vli.DataValue= vp.Key;
				vli.DisplayText = vp.Description;
				gridBasis.DisplayLayout.ValueLists["Version"].ValueListItems.Add(vli);

				vli.Dispose();
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
				_dtBasis = this._assortmentGeneralMethod.BasisDataTable;

				_dtBasis.Columns.Add("Merchandise",		System.Type.GetType("System.String"));
				_dtBasis.Columns.Add("Version",			System.Type.GetType("System.String"));
				_dtBasis.Columns.Add("HorizonDateRange",		System.Type.GetType("System.String"));

				// Loads in display versions of the RIDs
				foreach (DataRow aRow in _dtBasis.Rows)
				{
					int hierNodeRid = Convert.ToInt32(aRow["HN_RID"],CultureInfo.CurrentUICulture);
					HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(hierNodeRid);
					aRow["Merchandise"] = hnp.Text;
					int versionRid = Convert.ToInt32(aRow["FV_RID"],CultureInfo.CurrentUICulture);
					VersionProfile vp = (VersionProfile)_versionProfList.FindKey(versionRid);
					aRow["Version"] = vp.Description;
					int cdrRid = Convert.ToInt32(aRow["HORIZON_CDR_RID"],CultureInfo.CurrentUICulture);
					DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(cdrRid);
					aRow["HorizonDateRange"] = drp.DisplayDate;
				}

				_dtBasis.AcceptChanges();

				gridBasis.DataSource = _dtBasis;
			}
			catch
			{
				throw;
			}
		}
        /// <summary>
        /// Used at window initial load to fill in previously defined store grades
        /// </summary>
		private void LoadStoreGradesGrid()
		{
			try
			{
				_dtStoreGrades = _assortmentGeneralMethod.StoreGradesDataTable;

				

				gridStoreGrades.DataSource = _dtStoreGrades;
			}
			catch
			{
				throw;
			}
		}

        /// <summary>
        /// called when a new merchandise node is placed in the basis. 
        /// Uses only the node from the first basis defined.
        /// </summary>
        private void LoadMerchandiseStoreGrades(int hnKey)
        {
            try
            {
                //SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HeaderMaximumWarning)
                if (_dtStoreGrades.Rows.Count > 0)
                {
                    string msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_StoreGradesAlreadyExist);
					msg = msg.Replace("{0}","first basis merchandise");

                    if (MessageBox.Show(msg,
                        this.Text, MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2,
                        MessageBoxOptions.DefaultDesktopOnly)
                                == DialogResult.Yes)
                    {
                        LoadStoreGrades(hnKey);
                    }

                }
                else
                {
                    LoadStoreGrades(hnKey);
                }
            }
            catch
            {
                throw;
            }
        }

        private void LoadStoreGrades(int hnKey)
        {
            try
            {
                _dtStoreGrades.Rows.Clear();
                int seq = 1;
                StoreGradeList sgl = this.ApplicationTransaction.GetStoreGradeList(hnKey);
                foreach (StoreGradeProfile aGrade in sgl)
                {
                    DataRow newRow = _dtStoreGrades.NewRow();
                    newRow["METHOD_RID"] = this._assortmentGeneralMethod.Key;
                    newRow["STORE_GRADE_SEQUENCE"] = seq++;
                    newRow["BOUNDARY_INDEX"] = aGrade.Boundary;
                    //newRow["BOUNDARY_UNITS"] = 0;
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

		private void LoadMethod()
		{
			try
			{
				LoadCombos();
				LoadGrids();

                // Begin Track #4872 - JSmith - Global/User Attributes
                //if (_assortmentGeneralMethod.SG_RID != Include.NoRID)
                //    this.cboStoreAttribute.SelectedValue = _assortmentGeneralMethod.SG_RID;
                if (_assortmentGeneralMethod.SG_RID != Include.NoRID)
                {
                    this.cboStoreAttribute.SelectedValue = _assortmentGeneralMethod.SG_RID;
                    if (cboStoreAttribute.ContinueReadOnly)
                    {
                        SetMethodReadOnly();
                    }
                }
                // End Track #4872
									
				if (_assortmentGeneralMethod.FilterRid > 0 && _assortmentGeneralMethod.FilterRid != Include.UndefinedStoreFilter)
					cboFilter.SelectedIndex = cboFilter.Items.IndexOf(new FilterNameCombo(_assortmentGeneralMethod.FilterRid, -1, ""));
				
				//Load Reserve
				if (_assortmentGeneralMethod.ReserveAmount != Include.UndefinedReserve)
				{
					txtReserve.Text = Convert.ToString(_assortmentGeneralMethod.ReserveAmount, CultureInfo.CurrentUICulture);
					if (_assortmentGeneralMethod.ReserveType == eReserveType.Percent)
					{
						this.radPercent.Checked = true;
					}
					else if (_assortmentGeneralMethod.ReserveType == eReserveType.Units)
					{
						this.radUnits.Checked = true;
					}
				}

				if (_assortmentGeneralMethod.AverageBy == eStoreAverageBy.AllStores)
				{
					this.radAllStore.Checked = true;
				}
				else if (_assortmentGeneralMethod.AverageBy == eStoreAverageBy.Set)
				{
					this.radSet.Checked = true;
				}

				if (_assortmentGeneralMethod.OnHandInd)
					this.cbxOnhand.Checked = true;
				else
					this.cbxOnhand.Checked = false;
				if (_assortmentGeneralMethod.IntransitInd)
					this.cbxIntransit.Checked = true;
				else
					this.cbxIntransit.Checked = false;
				if (_assortmentGeneralMethod.SimilarStoreInd)
					this.cbxSimilarStores.Checked = true;
				else
					this.cbxSimilarStores.Checked = false;

				if (_assortmentGeneralMethod.VariableType == eGeneralAssortmentVariableType.Receipts)
					radReceipts.Checked = true;
				else if (_assortmentGeneralMethod.VariableType == eGeneralAssortmentVariableType.Stock)
					radStock.Checked = true;
				else if (_assortmentGeneralMethod.VariableType == eGeneralAssortmentVariableType.Sales)
					radSales.Checked = true;

				if (_assortmentGeneralMethod.GradeBoundary == eGradeBoundary.Index)
				{
					this.radIndexBoundary.Checked = false;
					this.radIndexBoundary.Checked = true;
				}
				else if (_assortmentGeneralMethod.GradeBoundary == eGradeBoundary.Units)
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
			try
			{
				_dtBasis.AcceptChanges();
				_dtStoreGrades.AcceptChanges();

				// Reserve amount
				if (txtReserve.Text != null && txtReserve.Text.Trim() != string.Empty)
					_assortmentGeneralMethod.ReserveAmount = Convert.ToDouble(txtReserve.Text, CultureInfo.CurrentUICulture);
				else
					_assortmentGeneralMethod.ReserveAmount = Include.UndefinedReserve;

				//Reserve Ind
				_assortmentGeneralMethod.ReserveType = eReserveType.Unknown;
				if (radPercent.Checked)
					_assortmentGeneralMethod.ReserveType = eReserveType.Percent;
				else if (radUnits.Checked)
					_assortmentGeneralMethod.ReserveType = eReserveType.Units;
				else
					_assortmentGeneralMethod.ReserveType = eReserveType.Unknown;

				// Filter
				if (cboFilter.Text.Trim() != string.Empty)
				{
					_assortmentGeneralMethod.FilterRid = ((FilterNameCombo)cboFilter.SelectedItem).FilterRID;
				}
				else
				{
					_assortmentGeneralMethod.FilterRid = Include.UndefinedStoreFilter;
				}
		
				// Store Attribute
				if (cboStoreAttribute.Text.Trim() != string.Empty)
				{
					_assortmentGeneralMethod.SG_RID = Convert.ToInt32(cboStoreAttribute.SelectedValue,CultureInfo.CurrentUICulture);
				}
				else
				{
					_assortmentGeneralMethod.SG_RID = Include.NoRID;
				}

				// Average By
				if (this.radAllStore.Checked)
					_assortmentGeneralMethod.AverageBy = eStoreAverageBy.AllStores;
				else
					_assortmentGeneralMethod.AverageBy = eStoreAverageBy.Set;

				// Variable
				if (radReceipts.Checked)
					_assortmentGeneralMethod.VariableType = eGeneralAssortmentVariableType.Receipts;
				else if (radStock.Checked)
					_assortmentGeneralMethod.VariableType = eGeneralAssortmentVariableType.Stock;
				else
					_assortmentGeneralMethod.VariableType = eGeneralAssortmentVariableType.Sales;
		
				// Checkboxes
				_assortmentGeneralMethod.OnHandInd = cbxOnhand.Checked;
				_assortmentGeneralMethod.IntransitInd = cbxIntransit.Checked;
				_assortmentGeneralMethod.SimilarStoreInd = cbxSimilarStores.Checked;

				// Grade Boundary
				if (radIndexBoundary.Checked)
					_assortmentGeneralMethod.GradeBoundary = eGradeBoundary.Index;
				else
					_assortmentGeneralMethod.GradeBoundary = eGradeBoundary.Units;
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

			methodFieldsValid = ValidReserve();

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
					int key = GetNodeRid(ref productID);
					if (key == Include.NoRID)
					{
						errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
							productID );	
						errorFound = true;
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
		override protected void HandleErrors()
		{

		}

		/// <summary>
		/// Use to set the specific method object before updating
		/// </summary>
		override protected void SetObject()
		{
			try
			{
				ABM = _assortmentGeneralMethod;
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
			return _explorerNode;
		}

		#endregion WorkflowMethodFormBase Overrides		

		private void gridBasis_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			try
			{
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
                //ugld.ApplyDefaults(e);
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
                //End TT#169
                e.Layout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.Default;
				e.Layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
				gridBasis.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

				//hide the key columns.
				e.Layout.Bands[0].Columns["METHOD_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["BASIS_SEQUENCE"].Hidden = true;
				e.Layout.Bands[0].Columns["HN_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["FV_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["HORIZON_CDR_RID"].Hidden = true;				
			
				//Prevent the user from re-arranging columns.
				gridBasis.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;

				if (FunctionSecurity.AllowUpdate)
				{
					gridBasis.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
					gridBasis.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
				}
				else
				{
					gridBasis.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
					gridBasis.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
				}

				//Set the header captions.
				gridBasis.DisplayLayout.Bands[0].Columns["Merchandise"].Header.VisiblePosition = 1;
				gridBasis.DisplayLayout.Bands[0].Columns["Merchandise"].Header.Caption = "Merchandise";
				gridBasis.DisplayLayout.Bands[0].Columns["Version"].Header.VisiblePosition = 2;
				gridBasis.DisplayLayout.Bands[0].Columns["Version"].Header.Caption = "Version";
				gridBasis.DisplayLayout.Bands[0].Columns["HorizonDateRange"].Header.VisiblePosition = 3;
				gridBasis.DisplayLayout.Bands[0].Columns["HorizonDateRange"].Header.Caption = "Horizon Date Range";
				gridBasis.DisplayLayout.Bands[0].Columns["WEIGHT"].Header.VisiblePosition = 4;
				gridBasis.DisplayLayout.Bands[0].Columns["WEIGHT"].Header.Caption = "Weight";

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
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
                //ugld.ApplyDefaults(e);
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
                //End TT#169
                e.Layout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.Default;
				e.Layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
				gridStoreGrades.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

				//hide the key columns.
				e.Layout.Bands[0].Columns["METHOD_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["STORE_GRADE_SEQUENCE"].Hidden = true;

			
				//Prevent the user from re-arranging columns.
				gridStoreGrades.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;

				if (FunctionSecurity.AllowUpdate)
				{
					gridStoreGrades.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
					gridStoreGrades.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
				}
				else
				{
					gridStoreGrades.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
					gridStoreGrades.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
				}

				//Set the header captions.
				gridStoreGrades.DisplayLayout.Bands[0].Columns["GRADE_CODE"].Header.VisiblePosition = 1;
				gridStoreGrades.DisplayLayout.Bands[0].Columns["GRADE_CODE"].Header.Caption = "Grade";
				gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_INDEX"].Header.VisiblePosition = 2;
				gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_INDEX"].Header.Caption = "Boundary Index";
				gridStoreGrades.DisplayLayout.Bands[0].Columns["BOUNDARY_UNITS"].Header.VisiblePosition = 3;
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

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				Save_Click(true);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}		
		}

        protected override void Call_btnProcess_Click()
		{
			try
			{
				ProcessAction(eMethodType.GeneralAssortment);

				// as part of the  processing we saved the info, so it should be changed to update.
				if (!ErrorFound)
				{
					_assortmentGeneralMethod.Method_Change_Type = eChangeType.update;
					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "btnProcess_Click");
			}
		}

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
                                        LoadMerchandiseStoreGrades(hnp.Key);
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
						frmCalDtSelector.DateRangeRID = Convert.ToInt32(e.Cell.Row.Cells["HORIZON_CDR_RID"].Value, CultureInfo.CurrentUICulture);
					}

					frmCalDtSelector.AnchorDate = SAB.ClientServerSession.Calendar.CurrentDate;
					frmCalDtSelector.AnchorDateRelativeTo = eDateRangeRelativeTo.Current;
					frmCalDtSelector.AllowDynamicToStoreOpen = false;
					frmCalDtSelector.AllowDynamicToPlan = false;

					dateRangeResult = frmCalDtSelector.ShowDialog();

					if (dateRangeResult == DialogResult.OK)
					{
						selectedDateRange = (DateRangeProfile)frmCalDtSelector.Tag;

						e.Cell.Value = selectedDateRange.DisplayDate;
						e.Cell.Row.Cells["HORIZON_CDR_RID"].Value = selectedDateRange.Key;

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
			e.Row.Cells["WEIGHT"].Value = 1;
		}

		private void gridBasis_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		private void cboFilter_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
				ChangePending = true;
		}

		private void cboStoreAttribute_SelectedIndexChanged(object sender, System.EventArgs e)
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

		private void radGlobal_Click(object sender, System.EventArgs e)
		{
			if (radGlobal.Checked)
			{
				FunctionSecurity = GlobalSecurity;
			}
			ApplySecurity();
		}

		private void radUser_Click(object sender, System.EventArgs e)
		{
			if (radGlobal.Checked)
			{
				FunctionSecurity = UserSecurity;
			}
			ApplySecurity();
		}

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
                                LoadMerchandiseStoreGrades(_nodeRid);
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
                    if (FunctionSecurity.IsReadOnly)
                    {
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));

                    }
                    else
                    {
                        StoreGrades_Populate(cbList.ClipboardProfile.Key);
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

		private void StoreGrades_Populate(int nodeRID)
		{
			try
			{
				//			string minStock, maxStock, minAd, maxAd, minColor, maxColor;
                //int count = 0, col = 0;
                int count = 0;
                //int minStock, maxStock, minAd, minColor, maxColor;
				
				_assortmentGeneralMethod.StoreGradesDataTable.Clear();
				_assortmentGeneralMethod.StoreGradesDataTable.AcceptChanges();
			
				_storeGradeList = SAB.HierarchyServerSession.GetStoreGradeList(nodeRID, false, true);
				//bool[,] cellIsNull = new Boolean [_storeGradeList.Count,5]; 
				int seq = 0;
				foreach(StoreGradeProfile sgp in _storeGradeList)
				{
					_dtStoreGrades.Rows.Add(new object[] { this._assortmentGeneralMethod.Key, seq++, sgp.Boundary,   
																				DBNull.Value, sgp.StoreGrade,});
					++count;
				}
				gridStoreGrades.DataSource = _dtStoreGrades;
//				for (int i = 0; i <  _storeGradeList.Count; i++)
//				{
//					for (int j = 0; j < 5; j++)
//					{
//						if (cellIsNull[i,j])
//						{
//							ugStoreGrades.Rows[i].Cells[j+3].Value = System.DBNull.Value;
//						}
//					}	
//				}

				FunctionSecurityProfile securityLevel = FunctionSecurity;
				
				if (FunctionSecurity.AllowUpdate)
				{
					this.gridStoreGrades.DisplayLayout.AddNewBox.Hidden = false;
				}
				else
				{
					this.gridStoreGrades.DisplayLayout.AddNewBox.Hidden = true;
				}
				
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

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
			if (this.radReceipts.Checked)
			{
				this.cbxIntransit.Enabled = true;
				this.cbxOnhand.Enabled = true;
			}
		}

		private void radSales_CheckedChanged(object sender, EventArgs e)
		{
			if (this.radSales.Checked)
			{
				this.cbxIntransit.Enabled = false;
				this.cbxOnhand.Enabled = false;
				this.cbxIntransit.Checked = false;
				this.cbxOnhand.Checked = false;
			}
		}

		private void radStock_CheckedChanged(object sender, EventArgs e)
		{
			if (this.radStock.Checked)
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


	}
}
