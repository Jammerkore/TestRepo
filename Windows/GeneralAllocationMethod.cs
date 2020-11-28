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
	/// Summary description for GeneralAllocationMethod.
	/// </summary>
	public class GeneralAllocationMethod : WorkflowMethodFormBase
	{
		#region Properties

		private System.Windows.Forms.TabPage tabMethod;
		private System.Windows.Forms.TabPage tabProperties;
		private System.Windows.Forms.PictureBox pictureBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ImageList Icons;

		private bool _newGenAlloc = false;
//		private string _strMethodType;
		private AllocationGeneralMethod _allocationGeneralMethod = null;
		private int _nodeRID = -1;
		private bool _textChanged = false;
		private bool _priorError = false;
		private int _lastMerchIndex = -1;
		private MIDRetail.Windows.Controls.MIDDateRangeSelector midDateRangeSelectorShip;
		private MIDRetail.Windows.Controls.MIDDateRangeSelector midDateRangeSelectorBeg;
		private System.Windows.Forms.TabControl tabGenAllocMethod;
		private System.Windows.Forms.RadioButton rbPercent;
		private System.Windows.Forms.RadioButton rbUnits;
		private System.Windows.Forms.TextBox textReserve;
		private DateRangeProfile Begdr = null;
		private DateRangeProfile Shipdr= null;
		private DateRangeProfile _origBegdr = null;
		private DateRangeProfile _origShipdr= null;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugWorkflows;
		private System.Windows.Forms.GroupBox gbxWarehouse;
		private System.Windows.Forms.Label lblShip;
		private System.Windows.Forms.Label lblBegin;
		private System.Windows.Forms.GroupBox gbxOTS;
		private System.Windows.Forms.Label lblMerchandise;
		private System.Windows.Forms.Label lblReserve;
		private Label lblReserveAsPacks;
		private Label lblReserveAsBulk;
		private TextBox txtReserveAsPacks;
		private TextBox txtReserveAsBulk;
        private MIDComboBoxEnh cbomerchandise;
		private System.Windows.Forms.GroupBox gbxTarget;
		
		/// <summary>
		/// Gets the id of the node.
		/// </summary>
		public int NodeRID 
		{
			get { return _nodeRID ; }
		}

		#endregion

		public GeneralAllocationMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB) : base (SAB, aEAB, eMIDTextCode.frm_GeneralAllocationMethod, eWorkflowMethodType.Method)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserGeneralAllocation);
			GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalGeneralAllocation);
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

				this.textReserve.TextChanged -= new System.EventHandler(this.textReserve_TextChanged);
				this.rbPercent.CheckedChanged -= new System.EventHandler(this.rbPercent_CheckedChanged);
				this.rbUnits.CheckedChanged -= new System.EventHandler(this.rbUnits_CheckedChanged);
				this.midDateRangeSelectorShip.Load -= new System.EventHandler(this.midDateRangeSelectorShip_Load);
				this.midDateRangeSelectorShip.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.midDateRangeSelectorShip_ClickCellButton);
				this.midDateRangeSelectorShip.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.midDateRangeSelectorShip_OnSelection);
				this.midDateRangeSelectorBeg.Load -= new System.EventHandler(this.midDateRangeSelectorBeg_Load);
				this.midDateRangeSelectorBeg.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.midDateRangeSelectorBeg_ClickCellButton);
				this.midDateRangeSelectorBeg.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.midDateRangeSelectorBeg_OnSelection);
				this.cbomerchandise.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.cbomerchandise_KeyDown);
				this.cbomerchandise.Validating -= new System.ComponentModel.CancelEventHandler(this.cbomerchandise_Validating);
				this.cbomerchandise.Validated -= new System.EventHandler(this.cbomerchandise_Validated);
				this.cbomerchandise.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cbomerchandise_DragDrop);
                this.cbomerchandise.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cbomerchandise_DragEnter);
                this.cbomerchandise.DragOver -= new System.Windows.Forms.DragEventHandler(this.cbomerchandise_DragOver);
                //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
                this.cbomerchandise.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cbomerchandise_MIDComboBoxPropertiesChangedEvent);
                //End TT#316
                // Begin MID Track 4858 - JSmith - Security changes
//				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
//				this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
//				this.btnProcess.Click -= new System.EventHandler(this.btnProcess_Click);
				// End MID Track 4858

				if (ApplicationTransaction != null)
				{
					ApplicationTransaction.Dispose();
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
            this.Icons = new System.Windows.Forms.ImageList(this.components);
            this.tabGenAllocMethod = new System.Windows.Forms.TabControl();
            this.tabMethod = new System.Windows.Forms.TabPage();
            this.gbxWarehouse = new System.Windows.Forms.GroupBox();
            this.rbUnits = new System.Windows.Forms.RadioButton();
            this.rbPercent = new System.Windows.Forms.RadioButton();
            this.lblReserveAsPacks = new System.Windows.Forms.Label();
            this.lblReserveAsBulk = new System.Windows.Forms.Label();
            this.txtReserveAsPacks = new System.Windows.Forms.TextBox();
            this.txtReserveAsBulk = new System.Windows.Forms.TextBox();
            this.lblReserve = new System.Windows.Forms.Label();
            this.textReserve = new System.Windows.Forms.TextBox();
            this.gbxTarget = new System.Windows.Forms.GroupBox();
            this.midDateRangeSelectorShip = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.lblShip = new System.Windows.Forms.Label();
            this.lblBegin = new System.Windows.Forms.Label();
            this.midDateRangeSelectorBeg = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.gbxOTS = new System.Windows.Forms.GroupBox();
            this.cbomerchandise = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblMerchandise = new System.Windows.Forms.Label();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.tabGenAllocMethod.SuspendLayout();
            this.tabMethod.SuspendLayout();
            this.gbxWarehouse.SuspendLayout();
            this.gbxTarget.SuspendLayout();
            this.gbxOTS.SuspendLayout();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(608, 504);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(512, 504);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(40, 504);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // Icons
            // 
            this.Icons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.Icons.ImageSize = new System.Drawing.Size(16, 16);
            this.Icons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tabGenAllocMethod
            // 
            this.tabGenAllocMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabGenAllocMethod.Controls.Add(this.tabMethod);
            this.tabGenAllocMethod.Controls.Add(this.tabProperties);
            this.tabGenAllocMethod.Location = new System.Drawing.Point(36, 64);
            this.tabGenAllocMethod.Name = "tabGenAllocMethod";
            this.tabGenAllocMethod.SelectedIndex = 0;
            this.tabGenAllocMethod.Size = new System.Drawing.Size(648, 423);
            this.tabGenAllocMethod.TabIndex = 14;
            // 
            // tabMethod
            // 
            this.tabMethod.Controls.Add(this.gbxWarehouse);
            this.tabMethod.Controls.Add(this.gbxTarget);
            this.tabMethod.Controls.Add(this.gbxOTS);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(640, 397);
            this.tabMethod.TabIndex = 1;
            this.tabMethod.Text = "Method";
            // 
            // gbxWarehouse
            // 
            this.gbxWarehouse.Controls.Add(this.rbUnits);
            this.gbxWarehouse.Controls.Add(this.rbPercent);
            this.gbxWarehouse.Controls.Add(this.lblReserveAsPacks);
            this.gbxWarehouse.Controls.Add(this.lblReserveAsBulk);
            this.gbxWarehouse.Controls.Add(this.txtReserveAsPacks);
            this.gbxWarehouse.Controls.Add(this.txtReserveAsBulk);
            this.gbxWarehouse.Controls.Add(this.lblReserve);
            this.gbxWarehouse.Controls.Add(this.textReserve);
            this.gbxWarehouse.Location = new System.Drawing.Point(24, 264);
            this.gbxWarehouse.Name = "gbxWarehouse";
            this.gbxWarehouse.Size = new System.Drawing.Size(451, 91);
            this.gbxWarehouse.TabIndex = 3;
            this.gbxWarehouse.TabStop = false;
            this.gbxWarehouse.Text = "Warehouse";
            // 
            // rbUnits
            // 
            this.rbUnits.Location = new System.Drawing.Point(113, 53);
            this.rbUnits.Name = "rbUnits";
            this.rbUnits.Size = new System.Drawing.Size(48, 20);
            this.rbUnits.TabIndex = 20;
            this.rbUnits.TabStop = true;
            this.rbUnits.Text = "Units";
            this.rbUnits.CheckedChanged += new System.EventHandler(this.rbUnits_CheckedChanged);
            // 
            // rbPercent
            // 
            this.rbPercent.Location = new System.Drawing.Point(43, 53);
            this.rbPercent.Name = "rbPercent";
            this.rbPercent.Size = new System.Drawing.Size(64, 20);
            this.rbPercent.TabIndex = 19;
            this.rbPercent.TabStop = true;
            this.rbPercent.Text = "Percent";
            this.rbPercent.CheckedChanged += new System.EventHandler(this.rbPercent_CheckedChanged);
            // 
            // lblReserveAsPacks
            // 
            this.lblReserveAsPacks.AutoSize = true;
            this.lblReserveAsPacks.Enabled = false;
            this.lblReserveAsPacks.Location = new System.Drawing.Point(215, 53);
            this.lblReserveAsPacks.Name = "lblReserveAsPacks";
            this.lblReserveAsPacks.Size = new System.Drawing.Size(35, 13);
            this.lblReserveAsPacks.TabIndex = 24;
            this.lblReserveAsPacks.Text = "label2";
            // 
            // lblReserveAsBulk
            // 
            this.lblReserveAsBulk.AutoSize = true;
            this.lblReserveAsBulk.Enabled = false;
            this.lblReserveAsBulk.Location = new System.Drawing.Point(215, 23);
            this.lblReserveAsBulk.Name = "lblReserveAsBulk";
            this.lblReserveAsBulk.Size = new System.Drawing.Size(35, 13);
            this.lblReserveAsBulk.TabIndex = 23;
            this.lblReserveAsBulk.Text = "label1";
            // 
            // txtReserveAsPacks
            // 
            this.txtReserveAsPacks.AcceptsReturn = true;
            this.txtReserveAsPacks.Enabled = false;
            this.txtReserveAsPacks.Location = new System.Drawing.Point(326, 51);
            this.txtReserveAsPacks.Name = "txtReserveAsPacks";
            this.txtReserveAsPacks.Size = new System.Drawing.Size(100, 20);
            this.txtReserveAsPacks.TabIndex = 22;
            this.txtReserveAsPacks.TextChanged += new System.EventHandler(this.txtReserveAsPacks_TextChanged);
            // 
            // txtReserveAsBulk
            // 
            this.txtReserveAsBulk.Enabled = false;
            this.txtReserveAsBulk.Location = new System.Drawing.Point(326, 20);
            this.txtReserveAsBulk.Name = "txtReserveAsBulk";
            this.txtReserveAsBulk.Size = new System.Drawing.Size(100, 20);
            this.txtReserveAsBulk.TabIndex = 21;
            this.txtReserveAsBulk.TextChanged += new System.EventHandler(this.txtReserveAsBulk_TextChanged);
            // 
            // lblReserve
            // 
            this.lblReserve.Location = new System.Drawing.Point(24, 20);
            this.lblReserve.Name = "lblReserve";
            this.lblReserve.Size = new System.Drawing.Size(48, 16);
            this.lblReserve.TabIndex = 6;
            this.lblReserve.Text = "Reserve";
            this.lblReserve.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textReserve
            // 
            this.textReserve.Location = new System.Drawing.Point(96, 20);
            this.textReserve.MaxLength = 10;
            this.textReserve.Name = "textReserve";
            this.textReserve.Size = new System.Drawing.Size(80, 20);
            this.textReserve.TabIndex = 18;
            this.textReserve.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textReserve.TextChanged += new System.EventHandler(this.textReserve_TextChanged);
            // 
            // gbxTarget
            // 
            this.gbxTarget.Controls.Add(this.midDateRangeSelectorShip);
            this.gbxTarget.Controls.Add(this.lblShip);
            this.gbxTarget.Controls.Add(this.lblBegin);
            this.gbxTarget.Controls.Add(this.midDateRangeSelectorBeg);
            this.gbxTarget.Location = new System.Drawing.Point(24, 24);
            this.gbxTarget.Name = "gbxTarget";
            this.gbxTarget.Size = new System.Drawing.Size(328, 136);
            this.gbxTarget.TabIndex = 2;
            this.gbxTarget.TabStop = false;
            this.gbxTarget.Text = "Target";
            // 
            // midDateRangeSelectorShip
            // 
            this.midDateRangeSelectorShip.DateRangeForm = null;
            this.midDateRangeSelectorShip.DateRangeRID = 0;
            this.midDateRangeSelectorShip.Enabled = false;
            this.midDateRangeSelectorShip.Location = new System.Drawing.Point(96, 64);
            this.midDateRangeSelectorShip.Name = "midDateRangeSelectorShip";
            this.midDateRangeSelectorShip.Size = new System.Drawing.Size(160, 24);
            this.midDateRangeSelectorShip.TabIndex = 16;
            this.midDateRangeSelectorShip.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.midDateRangeSelectorShip_OnSelection);
            this.midDateRangeSelectorShip.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.midDateRangeSelectorShip_ClickCellButton);
            this.midDateRangeSelectorShip.Load += new System.EventHandler(this.midDateRangeSelectorShip_Load);
            // 
            // lblShip
            // 
            this.lblShip.Location = new System.Drawing.Point(24, 64);
            this.lblShip.Name = "lblShip";
            this.lblShip.Size = new System.Drawing.Size(72, 16);
            this.lblShip.TabIndex = 10;
            this.lblShip.Text = "Shipping To";
            this.lblShip.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblBegin
            // 
            this.lblBegin.Location = new System.Drawing.Point(24, 24);
            this.lblBegin.Name = "lblBegin";
            this.lblBegin.Size = new System.Drawing.Size(56, 16);
            this.lblBegin.TabIndex = 6;
            this.lblBegin.Text = "Beginning";
            // 
            // midDateRangeSelectorBeg
            // 
            this.midDateRangeSelectorBeg.DateRangeForm = null;
            this.midDateRangeSelectorBeg.DateRangeRID = 0;
            this.midDateRangeSelectorBeg.Enabled = false;
            this.midDateRangeSelectorBeg.Location = new System.Drawing.Point(96, 24);
            this.midDateRangeSelectorBeg.Name = "midDateRangeSelectorBeg";
            this.midDateRangeSelectorBeg.Size = new System.Drawing.Size(160, 24);
            this.midDateRangeSelectorBeg.TabIndex = 15;
            this.midDateRangeSelectorBeg.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.midDateRangeSelectorBeg_OnSelection);
            this.midDateRangeSelectorBeg.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.midDateRangeSelectorBeg_ClickCellButton);
            this.midDateRangeSelectorBeg.Load += new System.EventHandler(this.midDateRangeSelectorBeg_Load);
            // 
            // gbxOTS
            // 
            this.gbxOTS.Controls.Add(this.cbomerchandise);
            this.gbxOTS.Controls.Add(this.lblMerchandise);
            this.gbxOTS.Location = new System.Drawing.Point(24, 160);
            this.gbxOTS.Name = "gbxOTS";
            this.gbxOTS.Size = new System.Drawing.Size(328, 103);
            this.gbxOTS.TabIndex = 1;
            this.gbxOTS.TabStop = false;
            this.gbxOTS.Text = "OTS Plan Basis";
            // 
            // cbomerchandise
            //
			this.cbomerchandise.AllowDrop = true;
            this.cbomerchandise.IgnoreFocusLost = true;
            this.cbomerchandise.AutoAdjust = true;
            this.cbomerchandise.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cbomerchandise.DataSource = null;
            this.cbomerchandise.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cbomerchandise.DropDownWidth = 216;
            this.cbomerchandise.Location = new System.Drawing.Point(96, 32);
            this.cbomerchandise.Margin = new System.Windows.Forms.Padding(0);
            this.cbomerchandise.Name = "cbomerchandise";
            this.cbomerchandise.Size = new System.Drawing.Size(216, 21);
            this.cbomerchandise.TabIndex = 17;
            this.cbomerchandise.Tag = null;
            this.cbomerchandise.Validating += new System.ComponentModel.CancelEventHandler(this.cbomerchandise_Validating);
            this.cbomerchandise.DragOver += new System.Windows.Forms.DragEventHandler(this.cbomerchandise_DragOver);
            this.cbomerchandise.SelectionChangeCommitted += new System.EventHandler(this.cbomerchandise_SelectionChangeCommitted);
            this.cbomerchandise.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cbomerchandise_MIDComboBoxPropertiesChangedEvent);
			this.cbomerchandise.DragDrop += new System.Windows.Forms.DragEventHandler(this.cbomerchandise_DragDrop);
            this.cbomerchandise.DragEnter += new System.Windows.Forms.DragEventHandler(this.cbomerchandise_DragEnter);
            this.cbomerchandise.Validated += new System.EventHandler(this.cbomerchandise_Validated);
			this.cbomerchandise.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbomerchandise_KeyDown);
            // 
            // lblMerchandise
            // 
            this.lblMerchandise.Location = new System.Drawing.Point(16, 32);
            this.lblMerchandise.Name = "lblMerchandise";
            this.lblMerchandise.Size = new System.Drawing.Size(72, 16);
            this.lblMerchandise.TabIndex = 6;
            this.lblMerchandise.Text = "Merchandise";
            this.lblMerchandise.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.ugWorkflows);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(640, 397);
            this.tabProperties.TabIndex = 0;
            this.tabProperties.Text = "Properties";
            this.tabProperties.Visible = false;
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
            this.ugWorkflows.Location = new System.Drawing.Point(16, 16);
            this.ugWorkflows.Name = "ugWorkflows";
            this.ugWorkflows.Size = new System.Drawing.Size(608, 368);
            this.ugWorkflows.TabIndex = 0;
            this.ugWorkflows.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugWorkflows_InitializeLayout);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // GeneralAllocationMethod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(720, 541);
            this.Controls.Add(this.tabGenAllocMethod);
            this.Name = "GeneralAllocationMethod";
            this.Text = "General Allocation Method";
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
            this.Controls.SetChildIndex(this.tabGenAllocMethod, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.pnlGlobalUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.tabGenAllocMethod.ResumeLayout(false);
            this.tabMethod.ResumeLayout(false);
            this.gbxWarehouse.ResumeLayout(false);
            this.gbxWarehouse.PerformLayout();
            this.gbxTarget.ResumeLayout(false);
            this.gbxOTS.ResumeLayout(false);
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
				_allocationGeneralMethod = new AllocationGeneralMethod(SAB,Include.NoRID);
				ABM = _allocationGeneralMethod;
				base.NewWorkflowMethod(aParentNode, eSecurityFunctions.AllocationMethodsUserGeneralAllocation, eSecurityFunctions.AllocationMethodsGlobalGeneralAllocation);

				Common_Load(aParentNode.GlobalUserType);				 

			}
			catch(Exception ex)
			{
				HandleException(ex, "GeneralAllocationMethod Constructor");
				FormLoadError = true;
			}
		}
		/// <summary>
		/// Opens an existing General Allocation Method.
		/// </summary>
		/// <param name="aMethodRID">aMethodRID</param>
		/// <param name="aLockStatus">The lock status of the data to be displayed</param>
		override public void UpdateWorkflowMethod(int aMethodRID, int aNodeRID, MIDWorkflowMethodTreeNode aNode, eLockStatus aLockStatus)
		{
			try
			{       
				_nodeRID = aNodeRID;
				_allocationGeneralMethod = new AllocationGeneralMethod(SAB,aMethodRID);
				base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.AllocationMethodsUserGeneralAllocation, eSecurityFunctions.AllocationMethodsGlobalGeneralAllocation);
			
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
                _allocationGeneralMethod = new AllocationGeneralMethod(SAB,aMethodRID);
				return Delete();
			}
			catch(DatabaseForeignKeyViolation keyVio)
			{
				throw keyVio;
			}
			catch (Exception err)
			{
				HandleException(err);
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
				_allocationGeneralMethod = new AllocationGeneralMethod(SAB,aMethodRID);
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
				_allocationGeneralMethod = new AllocationGeneralMethod(SAB,aMethodRID);
				ProcessAction(eMethodType.GeneralAllocation, true);
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
				SetText();		
	
				Name = MIDText.GetTextOnly((int)eMethodType.GeneralAllocation);
				if (_allocationGeneralMethod.Method_Change_Type == eChangeType.add)
				{
					BuildDataTables();
					LoadMerchandiseCombo();
				}
				else
					if (FunctionSecurity.AllowUpdate)
				{
					LoadMethods();
				}
				else
				{
					LoadMethods();
				}

                LoadWorkflows();

				if (_allocationGeneralMethod.Reserve == Include.UndefinedReserve)
				{
					this.rbPercent.Enabled = false;
					this.rbUnits.Enabled = false;
					// BEGIN TT#667 - Stodd - Pre-allocate Reserve
					txtReserveAsBulk.Enabled = false;
					txtReserveAsPacks.Enabled = false;
					// End TT#667 - Stodd - Pre-allocate Reserve
				}
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
				if (_allocationGeneralMethod.Method_Change_Type == eChangeType.update)
					this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
				else
					this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
	
				this.gbxTarget.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Target);
				this.lblBegin.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Beginning);	
				this.lblShip.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ShippingTo);	
				this.gbxOTS.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OTSPlan) + " "
									+ MIDText.GetTextOnly(eMIDTextCode.lbl_Basis);
				
				this.lblMerchandise.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);
				this.gbxWarehouse.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Warehouse);
				this.lblReserve.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Reserve);
				this.rbPercent.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Percent);
				this.rbUnits.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Units);
				this.btnClose.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
				this.btnProcess.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Process);

				// BEGIN TT#667 - Stodd - Pre-allocate Reserve
				this.lblReserveAsBulk.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ReserveAsBulk);
				this.lblReserveAsPacks.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ReserveAsPacks);
				// END TT#667 - Stodd - Pre-allocate Reserve
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}
		#region Form Load

		#region Properties Tab - Workflows
		/// <summary>
		/// Fill the workflow grid
		/// </summary>
		private void LoadWorkflows()//9-17
		{
			try
			{
				GetWorkflows(_allocationGeneralMethod.Key, ugWorkflows);
                //BEGIN TT#110-MD-VStuart - In Use Tool
                tabGenAllocMethod.Controls.Remove(tabProperties);
                //END TT#110-MD-VStuart - In Use Tool
            }
			catch(Exception ex)
			{
				HandleException(ex, "LoadWorkflows");
			}
		}
		#endregion

		private void LoadMethods()
		{
			try
			{
//				_InitialPopulate = true;
				
				BuildDataTables();
				LoadMerchandiseCombo();
								
				if (!_newGenAlloc)
				{											
					this.txtName.Text = _allocationGeneralMethod.Name;
					this.txtDesc.Text = _allocationGeneralMethod.Method_Description;
								
					if (_allocationGeneralMethod.User_RID == Include.GetGlobalUserRID())
						radGlobal.Checked = true;
					else
						radUser.Checked = true;
		
				}
				//OK If New
				LoadGenAllocValues();

//				_InitialPopulate = false;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void LoadMerchandiseCombo()
		{
			try
			{
				cbomerchandise.DataSource = MerchandiseDataTable;
				cbomerchandise.DisplayMember = "text";
				cbomerchandise.ValueMember = "seqno";
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
				if (_allocationGeneralMethod.Method_Change_Type == eChangeType.update)
				{
					//Load Calendar Data
					if (_allocationGeneralMethod.Begin_CDR_RID != Include.UndefinedCalendarDateRange)
					{
						Begdr = SAB.ClientServerSession.Calendar.GetDateRange(_allocationGeneralMethod.Begin_CDR_RID);
						LoadDateRangeText(Begdr, midDateRangeSelectorBeg);
						_origBegdr = Begdr;
					}
					if (_allocationGeneralMethod.Ship_To_CDR_RID != Include.UndefinedCalendarDateRange)
					{
						Shipdr = SAB.ClientServerSession.Calendar.GetDateRange(_allocationGeneralMethod.Ship_To_CDR_RID);
						LoadDateRangeText(Shipdr, midDateRangeSelectorShip);
						_origShipdr = Shipdr;
					}
					//Load Merchandise Node or Level Text to combo box
					HierarchyNodeProfile hnp;
					if (_allocationGeneralMethod.Merch_HN_RID != Include.NoRID)
					{
						//Begin Track #5378 - color and size not qualified
//						hnp = SAB.HierarchyServerSession.GetNodeData(_allocationGeneralMethod.Merch_HN_RID);
                        hnp = SAB.HierarchyServerSession.GetNodeData(_allocationGeneralMethod.Merch_HN_RID, true, true);
						//End Track #5378
						AddNodeToMerchandiseCombo ( hnp );
					}
					else
					{ 
						if (_allocationGeneralMethod.Merch_PH_RID != Include.NoRID)
							SetComboToLevel(_allocationGeneralMethod.Merch_PHL_Sequence);
						else
							cbomerchandise.SelectedIndex = 0;
					}
					//Load Reserve
					if (_allocationGeneralMethod.Reserve != Include.UndefinedReserve)
					{
						textReserve.Text = Convert.ToString(_allocationGeneralMethod.Reserve, CultureInfo.CurrentUICulture);
						//Load Radio button
						if (_allocationGeneralMethod.Percent_Ind)
						{
							this.rbPercent.Checked = true;
							this.rbUnits.Checked = false;
						}
						else
						{
							this.rbPercent.Checked = false;
							this.rbUnits.Checked = true;
						}
					}

					// BEGIN TT#667 - Stodd - Pre-allocate Reserve
					if (_allocationGeneralMethod.ReserveAsBulk == 0)
					{
						txtReserveAsBulk.Text = string.Empty;
					}
					else
					{
						txtReserveAsBulk.Text = _allocationGeneralMethod.ReserveAsBulk.ToString();
					}
					if (_allocationGeneralMethod.ReserveAsPacks == 0)
					{
						txtReserveAsPacks.Text = string.Empty;
					}
					else
					{
						txtReserveAsPacks.Text = _allocationGeneralMethod.ReserveAsPacks.ToString();
					}
					// END TT#667 - Stodd - Pre-allocate Reserve

				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		#endregion

		private void LoadDateRangeText(DateRangeProfile dr, Controls.MIDDateRangeSelector midDRSel)
		{
			try
			{
				if ( dr.DisplayDate != null)
				{
					midDRSel.Text= dr.DisplayDate;
				}
				else
				{
					midDRSel.Text= string.Empty;
				}
                //Add RID to Control's Tag (for later use)
				int lAddTag = dr.Key;
			
				midDRSel.Tag = lAddTag;

				 
				CheckDynPictures();
				
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void CheckDynPictures()
		{
			//Display Dynamic picture or not
			if (Begdr!= null)
				if (Begdr.DateRangeType == eCalendarRangeType.Dynamic)
					midDateRangeSelectorBeg.SetImage(ReoccurringImage);
				else
					midDateRangeSelectorBeg.SetImage(null);
			else
				midDateRangeSelectorBeg.SetImage(null);

			if (Shipdr!= null)
				if (Shipdr.DateRangeType == eCalendarRangeType.Dynamic) 
					midDateRangeSelectorShip.SetImage(ReoccurringImage);
				else
					midDateRangeSelectorShip.SetImage(null);
			else
				if (Begdr == null)
					midDateRangeSelectorShip.SetImage(null);
		}
		private void midDateRangeSelectorBeg_ClickCellButton(object sender, CellEventArgs e)
		{
			if (midDateRangeSelectorBeg.Tag !=null)
			{
				((CalendarDateSelector)midDateRangeSelectorBeg.DateRangeForm).DateRangeRID = (int)midDateRangeSelectorBeg.Tag;
			} 
			midDateRangeSelectorBeg.ShowSelector();
		}

		private void midDateRangeSelectorShip_ClickCellButton(object sender, CellEventArgs e)
		{
			if (midDateRangeSelectorShip.Tag !=null)
			{
				((CalendarDateSelector)midDateRangeSelectorShip.DateRangeForm).DateRangeRID = (int)midDateRangeSelectorShip.Tag;
			} 
			midDateRangeSelectorShip.ShowSelector();
		}

		private void midDateRangeSelectorBeg_Load(object sender, System.EventArgs e)
		{
			try
			{
				CalendarDateSelector frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{SAB});
				midDateRangeSelectorBeg.DateRangeForm = frm;
				frm.RestrictToSingleDate = true;
				frm.AllowDynamicToCurrent = true;
				frm.AllowDynamicToPlan = false;
				frm.AllowDynamicToStoreOpen = false;
			}
			catch(Exception ex)
			{
				HandleException(ex, "midDateRangeSelectorBeg_Load");
			}
		
		}

		private void midDateRangeSelectorShip_Load(object sender, System.EventArgs e)
		{
			try
			{
				CalendarDateSelector frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{SAB});
				midDateRangeSelectorShip.DateRangeForm = frm;
				frm.RestrictToSingleDate = true;
				frm.AllowDynamicToCurrent = true;
				frm.AllowDynamicToPlan = false;
				frm.AllowDynamicToStoreOpen = false;
			}
			catch(Exception ex)
			{
				HandleException(ex, "midDateRangeSelectorShip_Load");
			}
		}
		
		/// <summary>
		/// After selection is made on midDateRangeSelector - General Allocation
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void midDateRangeSelectorBeg_OnSelection(object sender, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
		{
			try
			{
				if (!e.SelectionCanceled)
				{
					ChangePending = true;
					Begdr = e.SelectedDateRange;
					LoadDateRangeText(Begdr, midDateRangeSelectorBeg);
				}
			}
			catch(Exception ex)
			{
				HandleException(ex, "midDateRangeSelectorBeg_OnSelection");
			}
		}
		private void midDateRangeSelectorShip_OnSelection(object sender, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
		{
			try
			{
				if (!e.SelectionCanceled)
				{
					ChangePending = true;
					Shipdr = e.SelectedDateRange;
					LoadDateRangeText(Shipdr, midDateRangeSelectorShip);
				}
			}
			catch(Exception ex)
			{
				HandleException(ex, "midDateRangeSelectorShip_OnSelection");
			}
		}
		
		private void textReserve_TextChanged(object sender, System.EventArgs e)
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
					rbPercent.Checked = false;
					rbPercent.Enabled = false;
					rbUnits.Checked = false;
					rbUnits.Enabled = false;

					// BEGIN TT#667 - Stodd - Pre-allocate Reserve
					txtReserveAsBulk.Enabled = false;
					txtReserveAsPacks.Enabled = false;
					txtReserveAsBulk.Text = string.Empty;
					txtReserveAsPacks.Text = string.Empty;
					lblReserveAsBulk.Enabled = false;
					lblReserveAsPacks.Enabled = false;
					// END TT#667 - Stodd - Pre-allocate Reserve

					((TextBox)sender).Focus();
					return;
				}	
				else
				{	
					rbPercent.Enabled = true; 
					rbPercent.TabStop = true; 
					rbUnits.Enabled = true;
					rbUnits.TabStop = true;

					// BEGIN TT#667 - Stodd - Pre-allocate Reserve
					txtReserveAsBulk.Enabled = true;
					txtReserveAsPacks.Enabled = true;
					lblReserveAsBulk.Enabled = true;
					lblReserveAsPacks.Enabled = true;
					// End TT#667 - Stodd - Pre-allocate Reserve
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

		private void txtReserveAsBulk_TextChanged(object sender, EventArgs e)
		{

		}

		private void txtReserveAsPacks_TextChanged(object sender, EventArgs e)
		{

		}

		private void rbPercent_CheckedChanged(object sender, System.EventArgs e)
		{
			ErrorProvider.SetError(textReserve,string.Empty);
			if (rbPercent.Checked == false) return;
			 
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
				}

				string inStr = textReserve.Text.ToString(CultureInfo.CurrentUICulture).Trim();
				if (inStr == "")
				{
					return;
				}	
				decimal outdec = System.Math.Abs(Convert.ToDecimal(inStr, CultureInfo.CurrentUICulture));
			
				if (outdec > 100)
				{
					throw new Exception();
				}
			}
			catch
			{
				string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeBetween0And100);
				ErrorProvider.SetError(textReserve,errorMessage);
				MessageBox.Show(errorMessage);
				textReserve.Focus();
				return;
			}
			 
		}

		private void rbUnits_CheckedChanged(object sender, System.EventArgs e)
		{
			ErrorProvider.SetError(textReserve,string.Empty);
			if (rbUnits.Checked == false) return;
			
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
				}

				string inStr = textReserve.Text.ToString(CultureInfo.CurrentUICulture).Trim();
				if (inStr == string.Empty) return;
					 	
				decimal outdec = Convert.ToDecimal(inStr, CultureInfo.CurrentUICulture);
				int outint = Convert.ToInt32(outdec, CultureInfo.CurrentUICulture);
				textReserve.Text = Convert.ToString(outint, CultureInfo.CurrentUICulture); 
			}		 
			catch
			{
				string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
				ErrorProvider.SetError(textReserve,errorMessage);
				MessageBox.Show(errorMessage);
				textReserve.Focus();
				return;
			}
			 
		}
		#region "DragNDrop Node to cbomerchandise"
		private void cbomerchandise_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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
					_lastMerchIndex = cbomerchandise.SelectedIndex;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void cbomerchandise_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string errorMessage;

			try
			{
				if (cbomerchandise.Text == string.Empty)
				{
					cbomerchandise.SelectedIndex = _lastMerchIndex;
					_priorError = false;
				}
				else
				{
					if (_textChanged)
					{
						_textChanged = false;

						HierarchyNodeProfile hnp = GetNodeProfile(cbomerchandise.Text);
						if (hnp.Key == Include.NoRID)
						{
							_priorError = true;

							errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), cbomerchandise.Text);
							ErrorProvider.SetError(cbomerchandise, errorMessage);
							MessageBox.Show(errorMessage);

							e.Cancel = true;
						}
						else 
						{
							AddNodeToMerchandiseCombo(hnp);
							_priorError = false;
						}	
					}
					// JBolles - MID Track #5020 - Prevent errored textbox from resetting its value before error is corrected
//					else if (_priorError)
//					{
//						cbomerchandise.SelectedIndex = _lastMerchIndex;
//					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void cbomerchandise_Validated(object sender, System.EventArgs e)
		{
			try
			{
				// JBolles - MID Track #5020 - Prevent errored textbox from resetting its value before error is corrected
				if(!_priorError)
				{
					ErrorProvider.SetError(cbomerchandise, string.Empty);
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

		private void cbomerchandise_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
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
                    AddNodeToMerchandiseCombo(hnp);
                    if (FormLoaded)
                    {
                        ChangePending = true;
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


		private HierarchyNodeProfile GetNodeProfile(string aProductID)
		{
			string productID;
			string[] pArray;

			try
			{
				productID = aProductID.Trim();
				pArray = productID.Split(new char[] {'['});
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

		private void  AddNodeToMerchandiseCombo (HierarchyNodeProfile hnp )
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

					cbomerchandise.SelectedIndex = MerchandiseDataTable.Rows.Count - 1;
				}
				else
				{
					cbomerchandise.SelectedIndex = levIndex;
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
						cbomerchandise.SelectedIndex = levIndex;
						break;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

		}

        
        private void cbomerchandise_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            // Begin TT#296 - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
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
            // End TT#296 - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
        }

        private void cbomerchandise_DragOver(object sender, DragEventArgs e)
        {
            // Begin TT#296 - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
            Merchandise_DragOver(sender, e);
            //Image_DragOver(sender, e);
            // End TT#296 - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
        }
        

		#endregion
		#region Save Button	
		// Begin MID Track 4858 - JSmith - Security changes
//		private void btnSave_Click(object sender, System.EventArgs e)
//		{
//				try
//				{
//                    Save_Click(true);
//				}
//				catch( Exception exception )
//				{
//					HandleException(exception);
//				}		
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
		#endregion
		
		// Begin MID Track 4858 - JSmith - Security changes
//		private void btnProcess_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				ProcessAction(eMethodType.GeneralAllocation);
//
//				// as part of the  processing we saved the info, so it should be changed to update.
//				
//				if (!ErrorFound)
//				{
//					_allocationGeneralMethod.Method_Change_Type = eChangeType.update;
//					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);			
//				}
//			}
//			catch(Exception ex)
//			{
//				HandleException(ex, "btnProcess_Click");
//			}
//		}

//		private void btnClose_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				Cancel_Click();
//			}
//			catch (Exception ex)
//			{
//				HandleException(ex);
//			}
//		}

		protected override void Call_btnProcess_Click()
		{
			try
			{
				// BEGIN TT#696-MD - Stodd - add "active process"
				//// BEGIN TT#497-MD - stodd -  Methods will not process with Method open
				//SelectedHeaderList selectedHeaderList = null;
				////bool isProcessingInAssortment = false;
				//bool useAssortmentHeaders = false;

				//useAssortmentHeaders = UseAssortmentSelectedHeaders();
				//if (useAssortmentHeaders)
				//{
				//    selectedHeaderList = SAB.AssortmentSelectedHeaderEvent.GetSelectedHeaders(this, eMethodType.GeneralAllocation);
				//}
				//else
				//{
				//    // BEGIN MID Track #6022 - DB error trying to process new unsaved header
				//    selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
				//}
				//// END TT#497-MD - stodd -  Methods will not process with Method open

				//if (selectedHeaderList.Count == 0)
				//{
				//    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoHeaderSelectedOnWorkspace));

				//    return;
				//}
				//// END MID Track #6022

				//====================================================
				// Checks to be sure there are valid selected headers
				//====================================================
				if (!OkToProcess(this, eMethodType.GeneralAllocation))
				{
					return;
				}
				// END TT#696-MD - Stodd - add "active process"	

				ProcessAction(eMethodType.GeneralAllocation);

				// as part of the  processing we saved the info, so it should be changed to update.
				
				if (!ErrorFound)
				{
					_allocationGeneralMethod.Method_Change_Type = eChangeType.update;
					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);			
				}
			}
			catch(Exception ex)
			{
				HandleException(ex, "Call_btnProcess_Click");
			}
		}
		// End MID Track 4858
	
		private bool MethodChanges()
		{
			try
			{
				//Method Name
				if (_allocationGeneralMethod.Name != this.txtName.Text)
					return true;

				//Method Description
				if (_allocationGeneralMethod.Method_Description != this.txtDesc.Text)
					return true;
			
				//Global and User Radio Buttons
				if (radGlobal.Checked)
				{
					if (_allocationGeneralMethod.GlobalUserType != eGlobalUserType.Global)
						return true;
				}
				else
				{
					if (_allocationGeneralMethod.GlobalUserType != eGlobalUserType.User)
						return true;
				}

				return false;
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return false;
			}
		}
		#region General Allocation Changes

		
		private void cbomerchandise_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (FormLoaded &&
				!FormIsClosing)
			{
				ErrorProvider.SetError(cbomerchandise, string.Empty);
				_lastMerchIndex = cbomerchandise.SelectedIndex;
				ChangePending = true;
			}
		}
        //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
        void cbomerchandise_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cbomerchandise_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316
		#endregion
	 
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

			// Default to All Stores
			_allocationGeneralMethod.SG_RID = Include.AllStoreFilterRID;
			int lAddTag;

			//Beginning Target Time Period  
			if (midDateRangeSelectorBeg.Tag !=null)
			{
                lAddTag = (int)midDateRangeSelectorBeg.Tag;
                _allocationGeneralMethod.Begin_CDR_RID = lAddTag;
			}               
			else
			{
				_allocationGeneralMethod.Begin_CDR_RID = Include.UndefinedCalendarDateRange; 
			}

			//Shipping To Time Period  
			if (midDateRangeSelectorShip.Tag !=null)
			{
				lAddTag = (int)midDateRangeSelectorShip.Tag;
				_allocationGeneralMethod.Ship_To_CDR_RID = lAddTag;
				
			}
			else
			{
				_allocationGeneralMethod.Ship_To_CDR_RID = Include.UndefinedCalendarDateRange; 
			}

			//Merchandise Level
			DataRow myDataRow = MerchandiseDataTable.Rows[cbomerchandise.SelectedIndex];
			eMerchandiseType MerchandiseType = (eMerchandiseType)(Convert.ToInt32(myDataRow["leveltypename"], CultureInfo.CurrentUICulture)); 
			_allocationGeneralMethod.MerchandiseType = MerchandiseType;

			switch(MerchandiseType)
			{
				case eMerchandiseType.Node:
					_allocationGeneralMethod.Merch_HN_RID = Convert.ToInt32(myDataRow["key"], CultureInfo.CurrentUICulture);
					break;
				case eMerchandiseType.HierarchyLevel:
					_allocationGeneralMethod.Merch_PHL_Sequence = Convert.ToInt32(myDataRow["key"], CultureInfo.CurrentUICulture);
					_allocationGeneralMethod.Merch_PH_RID = HP.Key;
					_allocationGeneralMethod.Merch_HN_RID = Include.NoRID;
					break;
				case eMerchandiseType.OTSPlanLevel:
					_allocationGeneralMethod.Merch_HN_RID = Include.NoRID;
					_allocationGeneralMethod.Merch_PH_RID = Include.NoRID;
					_allocationGeneralMethod.Merch_PHL_Sequence = 0;
					break;
			}
			
			// Reserve amount
			if (textReserve.Text != null && textReserve.Text.Trim() != string.Empty)
				_allocationGeneralMethod.Reserve = Convert.ToDouble(textReserve.Text, CultureInfo.CurrentUICulture);
			else
				_allocationGeneralMethod.Reserve = Include.UndefinedReserve;

			//Reserve Ind
			_allocationGeneralMethod.Percent_Ind = rbPercent.Checked;

			// BEGIN TT#667 - Stodd - Pre-allocate Reserve
			if (txtReserveAsBulk.Text != null && txtReserveAsBulk.Text.Trim() != string.Empty)
			{
				_allocationGeneralMethod.ReserveAsBulk = double.Parse(txtReserveAsBulk.Text);
			}
			else
			{
				_allocationGeneralMethod.ReserveAsBulk = 0;
			}

			if (txtReserveAsPacks.Text != null && txtReserveAsPacks.Text.Trim() != string.Empty)
			{
				_allocationGeneralMethod.ReserveAsPacks = double.Parse(txtReserveAsPacks.Text);
			}
			else
			{
				_allocationGeneralMethod.ReserveAsPacks = 0;
			}
			// END TT#667 - Stodd - Pre-allocate Reserve

		}

		/// <summary>
		/// Use to validate the fields that are specific to this method type
		/// </summary>
		override protected bool ValidateSpecificFields()
		{
			bool methodFieldsValid = true;
			string errorMessage;
			//initialize all fields to not having an error
			ErrorProvider.SetError (midDateRangeSelectorBeg,string.Empty);
			ErrorProvider.SetError (midDateRangeSelectorShip,string.Empty);
			ErrorProvider.SetError (cbomerchandise,string.Empty);
			ErrorProvider.SetError (textReserve,string.Empty);
			// get posting week
			WeekProfile currentWeek = SAB.HierarchyServerSession.GetCurrentDate().Week;
		
			// At least 1 General Allocation column must be present 
			if (   (midDateRangeSelectorBeg.Text.Trim() == string.Empty) 
				&& (midDateRangeSelectorShip.Text.Trim() == string.Empty)  
				&& (cbomerchandise.Text.Trim() == string.Empty) 
				&& (textReserve.Text.Trim() == string.Empty)
				)
			{
				methodFieldsValid = false;
				ErrorProvider.SetError (midDateRangeSelectorBeg,"At least one of the General Allocation fields is required; please enter.");
				ErrorProvider.SetError (midDateRangeSelectorShip,"At least one of the General Allocation fields is required; please enter.");
				ErrorProvider.SetError (cbomerchandise,"At least one of the General Allocation fields is required; please enter.");
				ErrorProvider.SetError (textReserve,"At least one of the General Allocation fields is required; please enter.");
				return methodFieldsValid;
			}	
			if (midDateRangeSelectorBeg.Text.Trim() != string.Empty)
			{
				if (Begdr.DateRangeType == eCalendarRangeType.Dynamic) 
				{
					if (Begdr.StartDateKey < 0) 
					{
						methodFieldsValid = false;
						ErrorProvider.SetError (midDateRangeSelectorBeg,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DateLessThanSalesWeek));
						return methodFieldsValid;
					}	
				}
				else if (Begdr.StartDateKey < currentWeek.Key)
				{
					methodFieldsValid = false;
					ErrorProvider.SetError (midDateRangeSelectorBeg,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DateLessThanSalesWeek));
					return methodFieldsValid;
				}
			} 
				
			// BEGIN MID Trak #3399 - Remove edit requiring Shipping To when Beginning data is enetered 
			
			//// Shipping  date is required if begginning date is present 
			//if (midDateRangeSelectorBeg.Text.Trim() != string.Empty 
			//	&& midDateRangeSelectorShip.Text.Trim() == string.Empty)
			//{
			//	methodFieldsValid = false;
			//	ErrorProvider.SetError (midDateRangeSelectorShip,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
			//	return methodFieldsValid;
			//}
			
			// END MID Trak #3399
 
			if (midDateRangeSelectorBeg.Text.Trim() != string.Empty 
				&& midDateRangeSelectorShip.Text.Trim() != string.Empty
				&& Begdr.DateRangeType != Shipdr.DateRangeType)
			{
				methodFieldsValid = false;
				errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DatesMustBeSameType);
				ErrorProvider.SetError (midDateRangeSelectorBeg,errorMessage);
				ErrorProvider.SetError (midDateRangeSelectorShip,errorMessage);
				return methodFieldsValid;
			}
			if (midDateRangeSelectorShip.Text.Trim() != string.Empty)
			{
				if (Shipdr.DateRangeType == eCalendarRangeType.Dynamic) 
				{
                    // BEGIN TT#5559 - AGallagher - GA Method_Shipping To Current Mid Week BOP - System Allows
                    //if (Shipdr.StartDateKey < 0)
                    if (Shipdr.StartDateKey <= 0)
                    // END TT#5559 - AGallagher - GA Method_Shipping To Current Mid Week BOP - System Allows
					{
						methodFieldsValid = false;
						ErrorProvider.SetError (midDateRangeSelectorShip,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DateLessThanSalesWeek));
						return methodFieldsValid;
					}	
				}
                // BEGIN TT#5559 - AGallagher - GA Method_Shipping To Current Mid Week BOP - System Allows
                //else if (Shipdr.StartDateKey < currentWeek.Key)
                else if (Shipdr.StartDateKey <= currentWeek.Key)
                // END TT#5559 - AGallagher - GA Method_Shipping To Current Mid Week BOP - System Allows
				{
					methodFieldsValid = false;
					ErrorProvider.SetError (midDateRangeSelectorShip,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DateLessThanSalesWeek));
					return methodFieldsValid;
				}
			} 
			if (midDateRangeSelectorBeg.Text.Trim() != string.Empty
				&& midDateRangeSelectorShip.Text.Trim() != string.Empty)
			{
				//if (Begdr.DateRangeType == eCalendarRangeType.Dynamic 
				//	&&  Begdr.StartDate >  Shipdr.StartDate)
				if ( Begdr.StartDateKey >  Shipdr.StartDateKey)
					
				{
					methodFieldsValid = false;
					ErrorProvider.SetError (midDateRangeSelectorBeg,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DateCannotBeGreater));
					return methodFieldsValid;
				}	
			}
		
			double reserve = 0;
			string inStr = textReserve.Text.ToString(CultureInfo.CurrentUICulture).Trim();
			if (inStr != string.Empty)
			{
				try
				{
					reserve = double.Parse(textReserve.Text);
					string outStr = System.Math.Abs(Convert.ToDecimal(inStr, CultureInfo.CurrentUICulture)).ToString();
				
					if (inStr != outStr)
					{
						methodFieldsValid = false;
						ErrorProvider.SetError (textReserve,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ReserveQtyCannotBeNeg));
					}
					else if (rbPercent.Checked == true)
					{
						//double dblValue;
						decimal outdec = System.Math.Abs(Convert.ToDecimal(inStr, CultureInfo.CurrentUICulture));
						if (outdec > 100)
						{
							methodFieldsValid = false;
							ErrorProvider.SetError (textReserve,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeBetween0And100));
						}
						else
						{
							reserve = Convert.ToDouble(outdec,CultureInfo.CurrentUICulture);
							reserve = Math.Round(reserve,2);
							textReserve.Text = reserve.ToString(CultureInfo.CurrentUICulture);
						}	
					}
					else if (rbUnits.Checked == false)
					{
							methodFieldsValid = false;
							//ErrorProvider.SetError (gbxWarehouse,"If Reserve amount is entered, either Percent or Units must be indicated.");
							ErrorProvider.SetError(rbPercent, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
					}
				}
				catch
				{	
					methodFieldsValid = false;
					ErrorProvider.SetError(gbxWarehouse,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
				}
			}

			//=====================================
			// Reserve As Bulk & Reserve As Packs
			//=====================================
			double reserveAsBulk = 0;
			double reserveAsPacks = 0;
			bool reserveOk = true;
			try
			{
				if (txtReserveAsBulk.Text.Trim() != string.Empty)
				{
					reserveAsBulk = double.Parse(txtReserveAsBulk.Text);
				}
			}
			catch
			{
				reserveOk = false;
				methodFieldsValid = false;
				ErrorProvider.SetError(txtReserveAsBulk, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
			}
			try
			{
				if (txtReserveAsPacks.Text.Trim() != string.Empty)
				{
					reserveAsPacks = double.Parse(txtReserveAsPacks.Text);
				}
			}
			catch
			{
				reserveOk = false;
				methodFieldsValid = false;
				ErrorProvider.SetError(txtReserveAsPacks, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
			}

			if (reserveOk)
			{
				if (rbPercent.Checked)
				{
					double totalPct = reserveAsBulk + reserveAsPacks;
					if (reserveAsBulk > 0 || reserveAsPacks > 0)
					{
						if (totalPct != 100)
						{
							methodFieldsValid = false;
							ErrorProvider.SetError(txtReserveAsBulk, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustEqual100));
							ErrorProvider.SetError(txtReserveAsPacks, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustEqual100));
						}
					}
				}
				if (rbUnits.Checked)
				{
					double totalUnits = reserveAsBulk + reserveAsPacks;
					if (reserveAsBulk > 0 || reserveAsPacks > 0)
					{
						if (totalUnits != reserve)
						{
							methodFieldsValid = false;
							ErrorProvider.SetError(txtReserveAsBulk, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_AsBulkAsPackNotEqualReserve));
							ErrorProvider.SetError(txtReserveAsPacks, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_AsBulkAsPackNotEqualReserve));
						}
					}
					int iReserveAsBulk = (int)reserveAsBulk;
					if (iReserveAsBulk != reserveAsBulk)
					{
						methodFieldsValid = false;
						ErrorProvider.SetError(txtReserveAsBulk, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidUnits));
					}
					int iReserveAsPacks = (int)reserveAsPacks;
					if (iReserveAsPacks != reserveAsPacks)
					{
						methodFieldsValid = false;
						ErrorProvider.SetError(txtReserveAsPacks, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidUnits));
					}
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
			try
			{
				ABM = _allocationGeneralMethod;
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
			return ExplorerNode;
		}

        // Begin TT#2062-MD - JSmith - "Can not call base method" error when change from Global to User
		override protected void BuildAttributeList()
        {
        }
		// End TT#2062-MD - JSmith - "Can not call base method" error when change from Global to User

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
			return securityOk;	// Track 5871 stodd
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
	}
}
 
 	
 
 

