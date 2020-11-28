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
	/// Summary description for DC Fulfillment Method.
	/// </summary>
	public class frmDCFulfillmentMethod : WorkflowMethodFormBase
	{
        private System.Windows.Forms.PictureBox pictureBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ImageList Icons;

		private DCFulfillmentMethod _DCFulfillmentMethod = null;
		private int _nodeRID = -1;
		private TabPage tabMethod;
        private TabControl tabDCFulfillmentMethod;
        private TabPage tabProperties;
        private GroupBox gbxMasterSplitOptions;
        private MIDAttributeComboBox cboPrioritizeHeadersBy;
        private Label lblPrioritizeHeadersBy;
        private UltraGrid ugWorkflows;
        private GroupBox gboAppplyOverageTo;
        private RadioButton rbHeadersDescending;
        private GroupBox gbxOrderStoresBy;
        private GroupBox groupBox1;
        private RadioButton rbProportional;
        private RadioButton rbDCFulfillment;
        private UltraGrid ugOrderStoresBy;
        private CheckBox cbxApplyMinimums;
        private GroupBox gbxStoreOrder;
        private RadioButton rbStoresDescending;
        private RadioButton rbStoresAscending;
        private ContextMenu mnuGrids;
        private GroupBox gbxDCFulfillmentSplitBy;
        private GroupBox gbxMinimums;
        private RadioButton radApplyAllStores;
        private Label lblMinimums;
        private RadioButton radApplyAllocQty;
        private GroupBox gbxReserve;
        private RadioButton radPostSplit;
        private RadioButton radPreSplit;
        private Label lblReserve;
        private GroupBox gbxSplitBy;
        private RadioButton radSplitDCStore;
        private RadioButton radSplitStoreDC;
        private Label lblSplit;
        private GroupBox gbxWithinDC;
        private RadioButton radWithinDCProportional;
        private RadioButton radWithinDCFill;
        private Label lblWithinDC;
        private RadioButton rbHeadersAscending;

        #region Properties
        /// <summary>
		/// Gets the id of the node.
		/// </summary>
		public int NodeRID 
		{
			get { return _nodeRID ; }
		}

		#endregion

        public frmDCFulfillmentMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB)
            : base(SAB, aEAB, eMIDTextCode.frm_DCFulfillment, eWorkflowMethodType.Method)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserDCFulfillment);
			GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalDCFulfillment);
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
                this.rbStoresDescending.CheckedChanged -= new System.EventHandler(this.rbStoresDescending_CheckedChanged);
                this.rbStoresAscending.CheckedChanged -= new System.EventHandler(this.rbStoresAscending_CheckedChanged);
                this.ugOrderStoresBy.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugOrderStoresBy_InitializeLayout);
                this.ugOrderStoresBy.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugOrderStoresBy_AfterRowInsert);
                this.ugOrderStoresBy.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugOrderStoresBy_MouseEnterElement);
                this.cbxApplyMinimums.CheckedChanged -= new System.EventHandler(this.cbxApplyMinimums_CheckedChanged);
                this.rbProportional.CheckedChanged -= new System.EventHandler(this.rbProportional_CheckedChanged);
                this.rbDCFulfillment.CheckedChanged -= new System.EventHandler(this.rbDCFulfillment_CheckedChanged);
                this.rbHeadersDescending.CheckedChanged -= new System.EventHandler(this.rbHeadersDescending_CheckedChanged);
                this.rbHeadersAscending.CheckedChanged -= new System.EventHandler(this.rbHeadersAscending_CheckedChanged);
                this.cboPrioritizeHeadersBy.MIDComboBoxPropertiesChangedEvent -= new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboPrioritizeHeadersBy_MIDComboBoxPropertiesChangedEvent);
                this.cboPrioritizeHeadersBy.SelectionChangeCommitted -= new System.EventHandler(this.cboPrioritizeHeadersBy_SelectionChangeCommitted);
                this.radSplitStoreDC.CheckedChanged -= new System.EventHandler(this.radSplitStoreDC_CheckedChanged);
                this.radSplitDCStore.CheckedChanged -= new System.EventHandler(this.radSplitDCStore_CheckedChanged);
                this.radPreSplit.CheckedChanged -= new System.EventHandler(this.radPreSplit_CheckedChanged);
                this.radPostSplit.CheckedChanged -= new System.EventHandler(this.radPostSplit_CheckedChanged);
                this.radApplyAllocQty.CheckedChanged -= new System.EventHandler(this.radApplyAllocQty_CheckedChanged);
                this.radApplyAllStores.CheckedChanged -= new System.EventHandler(this.radApplyAllStores_CheckedChanged);
                this.radWithinDCFill.CheckedChanged -= new System.EventHandler(this.radWithinDCFill_CheckedChanged);
                this.radWithinDCProportional.CheckedChanged -= new System.EventHandler(this.radWithinDCProportional_CheckedChanged);
               
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridLayout ultraGridLayout1 = new Infragistics.Win.UltraWinGrid.UltraGridLayout();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            this.Icons = new System.Windows.Forms.ImageList(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabMethod = new System.Windows.Forms.TabPage();
            this.gbxDCFulfillmentSplitBy = new System.Windows.Forms.GroupBox();
            this.gbxWithinDC = new System.Windows.Forms.GroupBox();
            this.radWithinDCProportional = new System.Windows.Forms.RadioButton();
            this.radWithinDCFill = new System.Windows.Forms.RadioButton();
            this.lblWithinDC = new System.Windows.Forms.Label();
            this.gbxMinimums = new System.Windows.Forms.GroupBox();
            this.radApplyAllStores = new System.Windows.Forms.RadioButton();
            this.lblMinimums = new System.Windows.Forms.Label();
            this.radApplyAllocQty = new System.Windows.Forms.RadioButton();
            this.gbxReserve = new System.Windows.Forms.GroupBox();
            this.radPostSplit = new System.Windows.Forms.RadioButton();
            this.radPreSplit = new System.Windows.Forms.RadioButton();
            this.lblReserve = new System.Windows.Forms.Label();
            this.gbxSplitBy = new System.Windows.Forms.GroupBox();
            this.radSplitDCStore = new System.Windows.Forms.RadioButton();
            this.radSplitStoreDC = new System.Windows.Forms.RadioButton();
            this.lblSplit = new System.Windows.Forms.Label();
            this.gbxOrderStoresBy = new System.Windows.Forms.GroupBox();
            this.gbxStoreOrder = new System.Windows.Forms.GroupBox();
            this.rbStoresDescending = new System.Windows.Forms.RadioButton();
            this.rbStoresAscending = new System.Windows.Forms.RadioButton();
            this.ugOrderStoresBy = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.mnuGrids = new System.Windows.Forms.ContextMenu();
            this.gbxMasterSplitOptions = new System.Windows.Forms.GroupBox();
            this.cbxApplyMinimums = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbProportional = new System.Windows.Forms.RadioButton();
            this.rbDCFulfillment = new System.Windows.Forms.RadioButton();
            this.gboAppplyOverageTo = new System.Windows.Forms.GroupBox();
            this.rbHeadersDescending = new System.Windows.Forms.RadioButton();
            this.rbHeadersAscending = new System.Windows.Forms.RadioButton();
            this.cboPrioritizeHeadersBy = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.lblPrioritizeHeadersBy = new System.Windows.Forms.Label();
            this.tabDCFulfillmentMethod = new System.Windows.Forms.TabControl();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabMethod.SuspendLayout();
            this.gbxDCFulfillmentSplitBy.SuspendLayout();
            this.gbxWithinDC.SuspendLayout();
            this.gbxMinimums.SuspendLayout();
            this.gbxReserve.SuspendLayout();
            this.gbxSplitBy.SuspendLayout();
            this.gbxOrderStoresBy.SuspendLayout();
            this.gbxStoreOrder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugOrderStoresBy)).BeginInit();
            this.gbxMasterSplitOptions.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gboAppplyOverageTo.SuspendLayout();
            this.tabDCFulfillmentMethod.SuspendLayout();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(608, 554);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(512, 554);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(40, 554);
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
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tabMethod
            // 
            this.tabMethod.Controls.Add(this.gbxDCFulfillmentSplitBy);
            this.tabMethod.Controls.Add(this.gbxOrderStoresBy);
            this.tabMethod.Controls.Add(this.gbxMasterSplitOptions);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(640, 447);
            this.tabMethod.TabIndex = 1;
            this.tabMethod.Text = "Method";
            // 
            // gbxDCFulfillmentSplitBy
            // 
            this.gbxDCFulfillmentSplitBy.Controls.Add(this.gbxWithinDC);
            this.gbxDCFulfillmentSplitBy.Controls.Add(this.gbxMinimums);
            this.gbxDCFulfillmentSplitBy.Controls.Add(this.gbxReserve);
            this.gbxDCFulfillmentSplitBy.Controls.Add(this.gbxSplitBy);
            this.gbxDCFulfillmentSplitBy.Location = new System.Drawing.Point(20, 123);
            this.gbxDCFulfillmentSplitBy.Name = "gbxDCFulfillmentSplitBy";
            this.gbxDCFulfillmentSplitBy.Size = new System.Drawing.Size(596, 72);
            this.gbxDCFulfillmentSplitBy.TabIndex = 7;
            this.gbxDCFulfillmentSplitBy.TabStop = false;
            this.gbxDCFulfillmentSplitBy.Text = "DC Fulfillment Options";
            // 
            // gbxWithinDC
            // 
            this.gbxWithinDC.Controls.Add(this.radWithinDCProportional);
            this.gbxWithinDC.Controls.Add(this.radWithinDCFill);
            this.gbxWithinDC.Controls.Add(this.lblWithinDC);
            this.gbxWithinDC.Location = new System.Drawing.Point(304, 13);
            this.gbxWithinDC.Name = "gbxWithinDC";
            this.gbxWithinDC.Size = new System.Drawing.Size(254, 25);
            this.gbxWithinDC.TabIndex = 4;
            this.gbxWithinDC.TabStop = false;
            // 
            // radWithinDCProportional
            // 
            this.radWithinDCProportional.AutoSize = true;
            this.radWithinDCProportional.Location = new System.Drawing.Point(164, 6);
            this.radWithinDCProportional.Name = "radWithinDCProportional";
            this.radWithinDCProportional.Size = new System.Drawing.Size(84, 17);
            this.radWithinDCProportional.TabIndex = 3;
            this.radWithinDCProportional.TabStop = true;
            this.radWithinDCProportional.Text = "Proportional:";
            this.radWithinDCProportional.UseVisualStyleBackColor = true;
            this.radWithinDCProportional.CheckedChanged += new System.EventHandler(this.radWithinDCProportional_CheckedChanged);
            // 
            // radWithinDCFill
            // 
            this.radWithinDCFill.AutoSize = true;
            this.radWithinDCFill.Location = new System.Drawing.Point(67, 6);
            this.radWithinDCFill.Name = "radWithinDCFill";
            this.radWithinDCFill.Size = new System.Drawing.Size(40, 17);
            this.radWithinDCFill.TabIndex = 2;
            this.radWithinDCFill.TabStop = true;
            this.radWithinDCFill.Text = "Fill:";
            this.radWithinDCFill.UseVisualStyleBackColor = true;
            this.radWithinDCFill.CheckedChanged += new System.EventHandler(this.radWithinDCFill_CheckedChanged);
            // 
            // lblWithinDC
            // 
            this.lblWithinDC.AutoSize = true;
            this.lblWithinDC.Location = new System.Drawing.Point(3, 8);
            this.lblWithinDC.Name = "lblWithinDC";
            this.lblWithinDC.Size = new System.Drawing.Size(58, 13);
            this.lblWithinDC.TabIndex = 1;
            this.lblWithinDC.Text = "Within DC:";
            // 
            // gbxMinimums
            // 
            this.gbxMinimums.Controls.Add(this.radApplyAllStores);
            this.gbxMinimums.Controls.Add(this.lblMinimums);
            this.gbxMinimums.Controls.Add(this.radApplyAllocQty);
            this.gbxMinimums.Location = new System.Drawing.Point(302, 40);
            this.gbxMinimums.Name = "gbxMinimums";
            this.gbxMinimums.Size = new System.Drawing.Size(254, 27);
            this.gbxMinimums.TabIndex = 2;
            this.gbxMinimums.TabStop = false;
            this.gbxMinimums.Visible = false;
            // 
            // radApplyAllStores
            // 
            this.radApplyAllStores.AutoSize = true;
            this.radApplyAllStores.Location = new System.Drawing.Point(68, 10);
            this.radApplyAllStores.Name = "radApplyAllStores";
            this.radApplyAllStores.Size = new System.Drawing.Size(62, 17);
            this.radApplyAllStores.TabIndex = 2;
            this.radApplyAllStores.TabStop = true;
            this.radApplyAllStores.Text = "Pre-split";
            this.radApplyAllStores.UseVisualStyleBackColor = true;
            this.radApplyAllStores.Visible = false;
            this.radApplyAllStores.CheckedChanged += new System.EventHandler(this.radApplyAllStores_CheckedChanged);
            // 
            // lblMinimums
            // 
            this.lblMinimums.AutoSize = true;
            this.lblMinimums.Location = new System.Drawing.Point(6, 10);
            this.lblMinimums.Name = "lblMinimums";
            this.lblMinimums.Size = new System.Drawing.Size(56, 13);
            this.lblMinimums.TabIndex = 1;
            this.lblMinimums.Text = "Minimums:";
            // 
            // radApplyAllocQty
            // 
            this.radApplyAllocQty.AutoSize = true;
            this.radApplyAllocQty.Location = new System.Drawing.Point(165, 8);
            this.radApplyAllocQty.Name = "radApplyAllocQty";
            this.radApplyAllocQty.Size = new System.Drawing.Size(67, 17);
            this.radApplyAllocQty.TabIndex = 0;
            this.radApplyAllocQty.TabStop = true;
            this.radApplyAllocQty.Text = "Post-split";
            this.radApplyAllocQty.UseVisualStyleBackColor = true;
            this.radApplyAllocQty.Visible = false;
            this.radApplyAllocQty.CheckedChanged += new System.EventHandler(this.radApplyAllocQty_CheckedChanged);
            // 
            // gbxReserve
            // 
            this.gbxReserve.Controls.Add(this.radPostSplit);
            this.gbxReserve.Controls.Add(this.radPreSplit);
            this.gbxReserve.Controls.Add(this.lblReserve);
            this.gbxReserve.Location = new System.Drawing.Point(7, 39);
            this.gbxReserve.Name = "gbxReserve";
            this.gbxReserve.Size = new System.Drawing.Size(254, 25);
            this.gbxReserve.TabIndex = 1;
            this.gbxReserve.TabStop = false;
            // 
            // radPostSplit
            // 
            this.radPostSplit.AutoSize = true;
            this.radPostSplit.Location = new System.Drawing.Point(167, 7);
            this.radPostSplit.Name = "radPostSplit";
            this.radPostSplit.Size = new System.Drawing.Size(70, 17);
            this.radPostSplit.TabIndex = 2;
            this.radPostSplit.TabStop = true;
            this.radPostSplit.Text = "Post-split:";
            this.radPostSplit.UseVisualStyleBackColor = true;
            this.radPostSplit.CheckedChanged += new System.EventHandler(this.radPostSplit_CheckedChanged);
            // 
            // radPreSplit
            // 
            this.radPreSplit.AutoSize = true;
            this.radPreSplit.Location = new System.Drawing.Point(62, 6);
            this.radPreSplit.Name = "radPreSplit";
            this.radPreSplit.Size = new System.Drawing.Size(62, 17);
            this.radPreSplit.TabIndex = 1;
            this.radPreSplit.TabStop = true;
            this.radPreSplit.Text = "Pre-split";
            this.radPreSplit.UseVisualStyleBackColor = true;
            this.radPreSplit.CheckedChanged += new System.EventHandler(this.radPreSplit_CheckedChanged);
            // 
            // lblReserve
            // 
            this.lblReserve.AutoSize = true;
            this.lblReserve.Location = new System.Drawing.Point(4, 7);
            this.lblReserve.Name = "lblReserve";
            this.lblReserve.Size = new System.Drawing.Size(50, 13);
            this.lblReserve.TabIndex = 0;
            this.lblReserve.Text = "Reserve:";
            // 
            // gbxSplitBy
            // 
            this.gbxSplitBy.Controls.Add(this.radSplitDCStore);
            this.gbxSplitBy.Controls.Add(this.radSplitStoreDC);
            this.gbxSplitBy.Controls.Add(this.lblSplit);
            this.gbxSplitBy.Location = new System.Drawing.Point(7, 12);
            this.gbxSplitBy.Name = "gbxSplitBy";
            this.gbxSplitBy.Size = new System.Drawing.Size(254, 27);
            this.gbxSplitBy.TabIndex = 0;
            this.gbxSplitBy.TabStop = false;
            // 
            // radSplitDCStore
            // 
            this.radSplitDCStore.AutoSize = true;
            this.radSplitDCStore.Location = new System.Drawing.Point(166, 5);
            this.radSplitDCStore.Name = "radSplitDCStore";
            this.radSplitDCStore.Size = new System.Drawing.Size(92, 17);
            this.radSplitDCStore.TabIndex = 2;
            this.radSplitDCStore.TabStop = true;
            this.radSplitDCStore.Text = "DC then Store";
            this.radSplitDCStore.UseVisualStyleBackColor = true;
            this.radSplitDCStore.CheckedChanged += new System.EventHandler(this.radSplitDCStore_CheckedChanged);
            // 
            // radSplitStoreDC
            // 
            this.radSplitStoreDC.AutoSize = true;
            this.radSplitStoreDC.Location = new System.Drawing.Point(62, 6);
            this.radSplitStoreDC.Name = "radSplitStoreDC";
            this.radSplitStoreDC.Size = new System.Drawing.Size(92, 17);
            this.radSplitStoreDC.TabIndex = 1;
            this.radSplitStoreDC.TabStop = true;
            this.radSplitStoreDC.Text = "Store then DC";
            this.radSplitStoreDC.UseVisualStyleBackColor = true;
            this.radSplitStoreDC.CheckedChanged += new System.EventHandler(this.radSplitStoreDC_CheckedChanged);
            // 
            // lblSplit
            // 
            this.lblSplit.AutoSize = true;
            this.lblSplit.Location = new System.Drawing.Point(4, 9);
            this.lblSplit.Name = "lblSplit";
            this.lblSplit.Size = new System.Drawing.Size(44, 13);
            this.lblSplit.TabIndex = 0;
            this.lblSplit.Text = "Split by:";
            // 
            // gbxOrderStoresBy
            // 
            this.gbxOrderStoresBy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbxOrderStoresBy.Controls.Add(this.gbxStoreOrder);
            this.gbxOrderStoresBy.Controls.Add(this.ugOrderStoresBy);
            this.gbxOrderStoresBy.Location = new System.Drawing.Point(20, 201);
            this.gbxOrderStoresBy.Name = "gbxOrderStoresBy";
            this.gbxOrderStoresBy.Size = new System.Drawing.Size(596, 229);
            this.gbxOrderStoresBy.TabIndex = 3;
            this.gbxOrderStoresBy.TabStop = false;
            // 
            // gbxStoreOrder
            // 
            this.gbxStoreOrder.Controls.Add(this.rbStoresDescending);
            this.gbxStoreOrder.Controls.Add(this.rbStoresAscending);
            this.gbxStoreOrder.Location = new System.Drawing.Point(384, 4);
            this.gbxStoreOrder.Name = "gbxStoreOrder";
            this.gbxStoreOrder.Size = new System.Drawing.Size(206, 29);
            this.gbxStoreOrder.TabIndex = 8;
            this.gbxStoreOrder.TabStop = false;
            // 
            // rbStoresDescending
            // 
            this.rbStoresDescending.AutoSize = true;
            this.rbStoresDescending.Location = new System.Drawing.Point(105, 9);
            this.rbStoresDescending.Name = "rbStoresDescending";
            this.rbStoresDescending.Size = new System.Drawing.Size(82, 17);
            this.rbStoresDescending.TabIndex = 1;
            this.rbStoresDescending.Text = "Descending";
            this.rbStoresDescending.UseVisualStyleBackColor = true;
            this.rbStoresDescending.CheckedChanged += new System.EventHandler(this.rbStoresDescending_CheckedChanged);
            // 
            // rbStoresAscending
            // 
            this.rbStoresAscending.AutoSize = true;
            this.rbStoresAscending.Location = new System.Drawing.Point(16, 8);
            this.rbStoresAscending.Name = "rbStoresAscending";
            this.rbStoresAscending.Size = new System.Drawing.Size(75, 17);
            this.rbStoresAscending.TabIndex = 0;
            this.rbStoresAscending.Text = "Ascending";
            this.rbStoresAscending.UseVisualStyleBackColor = true;
            this.rbStoresAscending.CheckedChanged += new System.EventHandler(this.rbStoresAscending_CheckedChanged);
            // 
            // ugOrderStoresBy
            // 
            this.ugOrderStoresBy.AllowDrop = true;
            this.ugOrderStoresBy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ugOrderStoresBy.ContextMenu = this.mnuGrids;
            this.ugOrderStoresBy.DisplayLayout.AddNewBox.Hidden = false;
            this.ugOrderStoresBy.DisplayLayout.AddNewBox.Prompt = " Add ...";
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugOrderStoresBy.DisplayLayout.Appearance = appearance1;
            ultraGridBand1.AddButtonCaption = " Action";
            this.ugOrderStoresBy.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.ugOrderStoresBy.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ugOrderStoresBy.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugOrderStoresBy.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugOrderStoresBy.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugOrderStoresBy.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ugOrderStoresBy.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugOrderStoresBy.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ugOrderStoresBy.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ugOrderStoresBy.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugOrderStoresBy.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            ultraGridLayout1.AddNewBox.Hidden = false;
            this.ugOrderStoresBy.Layouts.Add(ultraGridLayout1);
            this.ugOrderStoresBy.Location = new System.Drawing.Point(22, 34);
            this.ugOrderStoresBy.Name = "ugOrderStoresBy";
            this.ugOrderStoresBy.Size = new System.Drawing.Size(568, 176);
            this.ugOrderStoresBy.TabIndex = 7;
            this.ugOrderStoresBy.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugOrderStoresBy_InitializeLayout);
            this.ugOrderStoresBy.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugOrderStoresBy_AfterRowInsert);
            this.ugOrderStoresBy.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugOrderStoresBy_MouseEnterElement);
            // 
            // gbxMasterSplitOptions
            // 
            this.gbxMasterSplitOptions.Controls.Add(this.cbxApplyMinimums);
            this.gbxMasterSplitOptions.Controls.Add(this.groupBox1);
            this.gbxMasterSplitOptions.Controls.Add(this.gboAppplyOverageTo);
            this.gbxMasterSplitOptions.Controls.Add(this.cboPrioritizeHeadersBy);
            this.gbxMasterSplitOptions.Controls.Add(this.lblPrioritizeHeadersBy);
            this.gbxMasterSplitOptions.Location = new System.Drawing.Point(20, 19);
            this.gbxMasterSplitOptions.Name = "gbxMasterSplitOptions";
            this.gbxMasterSplitOptions.Size = new System.Drawing.Size(596, 98);
            this.gbxMasterSplitOptions.TabIndex = 2;
            this.gbxMasterSplitOptions.TabStop = false;
            this.gbxMasterSplitOptions.Text = "Master Split Options";
            // 
            // cbxApplyMinimums
            // 
            this.cbxApplyMinimums.AutoSize = true;
            this.cbxApplyMinimums.Location = new System.Drawing.Point(168, 22);
            this.cbxApplyMinimums.Name = "cbxApplyMinimums";
            this.cbxApplyMinimums.Size = new System.Drawing.Size(101, 17);
            this.cbxApplyMinimums.TabIndex = 5;
            this.cbxApplyMinimums.Text = "Apply Minimums";
            this.cbxApplyMinimums.UseVisualStyleBackColor = true;
            this.cbxApplyMinimums.CheckedChanged += new System.EventHandler(this.cbxApplyMinimums_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbProportional);
            this.groupBox1.Controls.Add(this.rbDCFulfillment);
            this.groupBox1.Location = new System.Drawing.Point(6, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(149, 70);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // rbProportional
            // 
            this.rbProportional.AutoSize = true;
            this.rbProportional.Location = new System.Drawing.Point(16, 40);
            this.rbProportional.Name = "rbProportional";
            this.rbProportional.Size = new System.Drawing.Size(81, 17);
            this.rbProportional.TabIndex = 1;
            this.rbProportional.Text = "Proportional";
            this.rbProportional.UseVisualStyleBackColor = true;
            this.rbProportional.CheckedChanged += new System.EventHandler(this.rbProportional_CheckedChanged);
            // 
            // rbDCFulfillment
            // 
            this.rbDCFulfillment.AutoSize = true;
            this.rbDCFulfillment.Location = new System.Drawing.Point(16, 15);
            this.rbDCFulfillment.Name = "rbDCFulfillment";
            this.rbDCFulfillment.Size = new System.Drawing.Size(89, 17);
            this.rbDCFulfillment.TabIndex = 0;
            this.rbDCFulfillment.Text = "DC Fulfillment";
            this.rbDCFulfillment.UseVisualStyleBackColor = true;
            this.rbDCFulfillment.CheckedChanged += new System.EventHandler(this.rbDCFulfillment_CheckedChanged);
            // 
            // gboAppplyOverageTo
            // 
            this.gboAppplyOverageTo.Controls.Add(this.rbHeadersDescending);
            this.gboAppplyOverageTo.Controls.Add(this.rbHeadersAscending);
            this.gboAppplyOverageTo.Location = new System.Drawing.Point(308, 43);
            this.gboAppplyOverageTo.Name = "gboAppplyOverageTo";
            this.gboAppplyOverageTo.Size = new System.Drawing.Size(209, 41);
            this.gboAppplyOverageTo.TabIndex = 3;
            this.gboAppplyOverageTo.TabStop = false;
            // 
            // rbHeadersDescending
            // 
            this.rbHeadersDescending.AutoSize = true;
            this.rbHeadersDescending.Location = new System.Drawing.Point(105, 15);
            this.rbHeadersDescending.Name = "rbHeadersDescending";
            this.rbHeadersDescending.Size = new System.Drawing.Size(82, 17);
            this.rbHeadersDescending.TabIndex = 1;
            this.rbHeadersDescending.Text = "Descending";
            this.rbHeadersDescending.UseVisualStyleBackColor = true;
            this.rbHeadersDescending.CheckedChanged += new System.EventHandler(this.rbHeadersDescending_CheckedChanged);
            // 
            // rbHeadersAscending
            // 
            this.rbHeadersAscending.AutoSize = true;
            this.rbHeadersAscending.Location = new System.Drawing.Point(16, 15);
            this.rbHeadersAscending.Name = "rbHeadersAscending";
            this.rbHeadersAscending.Size = new System.Drawing.Size(75, 17);
            this.rbHeadersAscending.TabIndex = 0;
            this.rbHeadersAscending.Text = "Ascending";
            this.rbHeadersAscending.UseVisualStyleBackColor = true;
            this.rbHeadersAscending.CheckedChanged += new System.EventHandler(this.rbHeadersAscending_CheckedChanged);
            // 
            // cboPrioritizeHeadersBy
            // 
            this.cboPrioritizeHeadersBy.AllowDrop = true;
            this.cboPrioritizeHeadersBy.AllowUserAttributes = false;
            this.cboPrioritizeHeadersBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboPrioritizeHeadersBy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPrioritizeHeadersBy.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboPrioritizeHeadersBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPrioritizeHeadersBy.FormattingEnabled = true;
            this.cboPrioritizeHeadersBy.Location = new System.Drawing.Point(426, 19);
            this.cboPrioritizeHeadersBy.Name = "cboPrioritizeHeadersBy";
            this.cboPrioritizeHeadersBy.Size = new System.Drawing.Size(164, 21);
            this.cboPrioritizeHeadersBy.TabIndex = 0;
            this.cboPrioritizeHeadersBy.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboPrioritizeHeadersBy_MIDComboBoxPropertiesChangedEvent);
            this.cboPrioritizeHeadersBy.SelectionChangeCommitted += new System.EventHandler(this.cboPrioritizeHeadersBy_SelectionChangeCommitted);
            // 
            // lblPrioritizeHeadersBy
            // 
            this.lblPrioritizeHeadersBy.AutoSize = true;
            this.lblPrioritizeHeadersBy.Location = new System.Drawing.Point(303, 22);
            this.lblPrioritizeHeadersBy.Name = "lblPrioritizeHeadersBy";
            this.lblPrioritizeHeadersBy.Size = new System.Drawing.Size(104, 13);
            this.lblPrioritizeHeadersBy.TabIndex = 1;
            this.lblPrioritizeHeadersBy.Text = "Prioritize Headers By";
            // 
            // tabDCFulfillmentMethod
            // 
            this.tabDCFulfillmentMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabDCFulfillmentMethod.Controls.Add(this.tabMethod);
            this.tabDCFulfillmentMethod.Controls.Add(this.tabProperties);
            this.tabDCFulfillmentMethod.Location = new System.Drawing.Point(36, 64);
            this.tabDCFulfillmentMethod.Name = "tabDCFulfillmentMethod";
            this.tabDCFulfillmentMethod.SelectedIndex = 0;
            this.tabDCFulfillmentMethod.Size = new System.Drawing.Size(648, 473);
            this.tabDCFulfillmentMethod.TabIndex = 14;
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.ugWorkflows);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(640, 447);
            this.tabProperties.TabIndex = 2;
            this.tabProperties.Text = "Properties";
            this.tabProperties.UseVisualStyleBackColor = true;
            // 
            // ugWorkflows
            // 
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugWorkflows.DisplayLayout.Appearance = appearance7;
            this.ugWorkflows.DisplayLayout.InterBandSpacing = 10;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.ugWorkflows.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.TextHAlignAsString = "Left";
            appearance9.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugWorkflows.DisplayLayout.Override.HeaderAppearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.Override.RowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorAppearance = appearance11;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugWorkflows.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.ugWorkflows.DisplayLayout.Override.SelectedRowAppearance = appearance12;
            this.ugWorkflows.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugWorkflows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugWorkflows.Location = new System.Drawing.Point(0, 0);
            this.ugWorkflows.Name = "ugWorkflows";
            this.ugWorkflows.Size = new System.Drawing.Size(640, 447);
            this.ugWorkflows.TabIndex = 2;
            // 
            // frmDCFulfillmentMethod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(720, 591);
            this.Controls.Add(this.tabDCFulfillmentMethod);
            this.Name = "frmDCFulfillmentMethod";
            this.Text = "DC Fulfillment Method";
            this.Controls.SetChildIndex(this.tabDCFulfillmentMethod, 0);
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.pnlGlobalUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabMethod.ResumeLayout(false);
            this.gbxDCFulfillmentSplitBy.ResumeLayout(false);
            this.gbxWithinDC.ResumeLayout(false);
            this.gbxWithinDC.PerformLayout();
            this.gbxMinimums.ResumeLayout(false);
            this.gbxMinimums.PerformLayout();
            this.gbxReserve.ResumeLayout(false);
            this.gbxReserve.PerformLayout();
            this.gbxSplitBy.ResumeLayout(false);
            this.gbxSplitBy.PerformLayout();
            this.gbxOrderStoresBy.ResumeLayout(false);
            this.gbxStoreOrder.ResumeLayout(false);
            this.gbxStoreOrder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugOrderStoresBy)).EndInit();
            this.gbxMasterSplitOptions.ResumeLayout(false);
            this.gbxMasterSplitOptions.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gboAppplyOverageTo.ResumeLayout(false);
            this.gboAppplyOverageTo.PerformLayout();
            this.tabDCFulfillmentMethod.ResumeLayout(false);
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		

		/// <summary>
		/// Opens a new DC Fulfillment Method.
		/// </summary>
		override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
		{
			try
			{
				_DCFulfillmentMethod = new DCFulfillmentMethod(SAB,Include.NoRID);
				ABM = _DCFulfillmentMethod;
				base.NewWorkflowMethod(aParentNode, eSecurityFunctions.AllocationMethodsUserDCFulfillment, eSecurityFunctions.AllocationMethodsGlobalDCFulfillment);

				Common_Load(aParentNode.GlobalUserType);				 

			}
			catch(Exception ex)
			{
				HandleException(ex, "DC Fulfillment Method Constructor");
				FormLoadError = true;
			}
		}
		/// <summary>
		/// Opens an existing DC Fulfillment Method.
		/// </summary>
		/// <param name="aMethodRID">aMethodRID</param>
		/// <param name="aLockStatus">The lock status of the data to be displayed</param>
		override public void UpdateWorkflowMethod(int aMethodRID, int aNodeRID, MIDWorkflowMethodTreeNode aNode, eLockStatus aLockStatus)
		{
			try
			{       
				_nodeRID = aNodeRID;
				_DCFulfillmentMethod = new DCFulfillmentMethod(SAB,aMethodRID);
				base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.AllocationMethodsUserDCFulfillment, eSecurityFunctions.AllocationMethodsGlobalDCFulfillment);
			
				Common_Load(aNode.GlobalUserType);
			}
			catch (Exception err)
			{
				HandleException(err);
				FormLoadError = true;
			}
		}

		/// <summary>
		/// Deletes a DC Fulfillment Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		override public bool DeleteWorkflowMethod(int aMethodRID)
		{
			try
			{       
                _DCFulfillmentMethod = new DCFulfillmentMethod(SAB,aMethodRID);
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
		/// Renames a DC Fulfillment Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <param name="aNewName">The new name of the workflow or method</param>
		override public bool RenameWorkflowMethod(int aMethodRID, string aNewName)
		{
			try
			{
                _DCFulfillmentMethod = new DCFulfillmentMethod(SAB, aMethodRID);
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
                _DCFulfillmentMethod = new DCFulfillmentMethod(SAB, aMethodRID);
				ProcessAction(eMethodType.DCFulfillment, true);
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
	
				Name = MIDText.GetTextOnly((int)eMethodType.DCFulfillment);
                if (_DCFulfillmentMethod.Method_Change_Type == eChangeType.add)
                {
                    Format_Title(eDataState.New, eMIDTextCode.frm_DCFulfillment, null);
                }
                else if (FunctionSecurity.AllowUpdate)
                {
                    Format_Title(eDataState.Updatable, eMIDTextCode.frm_DCFulfillment, _DCFulfillmentMethod.Name);
                }
                else
                {
                    Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_DCFulfillment, _DCFulfillmentMethod.Name);
                }

                if (FunctionSecurity.AllowExecute)
                {
                    btnProcess.Enabled = true;
                }
                else
                {
                    btnProcess.Enabled = false;
                }

                BuildHeaderCharList();
                cboPrioritizeHeadersBy.SelectedValue = _DCFulfillmentMethod.PrioritizeBy;

                if (_DCFulfillmentMethod.SplitOption == eDCFulfillmentSplitOption.Proportional)
                {
                    rbProportional.Checked = true;
                }
                else if (_DCFulfillmentMethod.SplitOption == eDCFulfillmentSplitOption.DCFulfillment)
                {
                    rbDCFulfillment.Checked = true;
                }

                cbxApplyMinimums.Checked = _DCFulfillmentMethod.ApplyMinimumsInd;

                if (_DCFulfillmentMethod.HeadersOrder == eDCFulfillmentHeadersOrder.Descending)
                {
                    rbHeadersDescending.Checked = true;
                }
                else if (_DCFulfillmentMethod.HeadersOrder == eDCFulfillmentHeadersOrder.Ascending)
                {
                    rbHeadersAscending.Checked = true;
                }

                if (_DCFulfillmentMethod.StoresOrder == eDCFulfillmentStoresOrder.Descending)
                {
                    rbStoresDescending.Checked = true;
                }
                else if (_DCFulfillmentMethod.StoresOrder == eDCFulfillmentStoresOrder.Ascending)
                {
                    rbStoresAscending.Checked = true;
                }

                if (_DCFulfillmentMethod.Split_By_Option == eDCFulfillmentSplitByOption.SplitByDC)
                {
                    radSplitDCStore.Checked = true;
                }
                else
                {
                    radSplitStoreDC.Checked = true;
                } 

                if (_DCFulfillmentMethod.Split_By_Reserve == eDCFulfillmentReserve.ReservePostSplit)
                {
                    radPostSplit.Checked = true;
                }
                else
                {
                    radPreSplit.Checked = true;
                }

                if (_DCFulfillmentMethod.Apply_By == eDCFulfillmentMinimums.ApplyByQty)
                {
                    radApplyAllocQty.Checked = true;
                }
                else
                {
                    radApplyAllStores.Checked = true;
                }

                if (_DCFulfillmentMethod.Within_Dc == eDCFulfillmentWithinDC.Fill)
                {
                    radWithinDCFill.Checked = true;
                }
                else
                {
                    radWithinDCProportional.Checked = true;
                }
 
                ugOrderStoresBy.DataSource = _DCFulfillmentMethod.dtStoreOrder;

                // Hide proportional until phase 3
                //rbProportional.Visible = false;

                LoadWorkflows();

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
                if (_DCFulfillmentMethod.Method_Change_Type == eChangeType.update)
                {
                    this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
                }
                else
                {
                    this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
                }

                this.gbxMasterSplitOptions.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentMasterSplitOptions);
                this.rbDCFulfillment.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillment);
                this.rbProportional.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentProportional);
                this.cbxApplyMinimums.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentApplyMinimums);
                this.lblPrioritizeHeadersBy.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentPrioritizeHeadersBy);
                this.rbHeadersAscending.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentHeadersAscending);
                this.rbHeadersDescending.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentHeadersDescending);
                this.rbStoresAscending.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentStoresAscending);
                this.rbStoresDescending.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentStoresDescending);
                this.lblSplit.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentSplitBy);
                this.radSplitStoreDC.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentSplitByStore);
                this.radSplitDCStore.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentSplitByDC);
                this.lblReserve.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentReserve);
                this.radPreSplit.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentReservePreSplit);
                this.radPostSplit.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentReservePostSplit);
                this.lblMinimums.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentMinimumsApply);
                this.radApplyAllocQty.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentReservePostSplit);
                this.radApplyAllStores.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentReservePreSplit);
                this.radWithinDCFill.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentWithinDCFill);
                this.radWithinDCProportional.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentWithinDCProportional);
                this.gbxDCFulfillmentSplitBy.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentOptions);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}
        private void BuildHeaderCharList()
        {
            try
            {
                HeaderCharList hcl = new HeaderCharList();
                cboPrioritizeHeadersBy.DataSource = hcl.BuildHeaderCharList();
                cboPrioritizeHeadersBy.DisplayMember = "Text";
                cboPrioritizeHeadersBy.ValueMember = "Key";
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
                GetWorkflows(_DCFulfillmentMethod.Key, ugWorkflows);
                tabDCFulfillmentMethod.Controls.Remove(tabProperties);
            }
			catch(Exception ex)
			{
				HandleException(ex, "LoadWorkflows");
			}
		}
		#endregion

		#region Save Button	
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
		#endregion
		
	

		protected override void Call_btnProcess_Click()
		{
			try
			{
				//====================================================
				// Checks to be sure there are valid selected headers
				//====================================================
				if (!OkToProcess(this, eMethodType.DCFulfillment))
				{
					return;
				}

				ProcessAction(eMethodType.DCFulfillment);

				// as part of the  processing we saved the info, so it should be changed to update.
				
				if (!ErrorFound)
				{
					_DCFulfillmentMethod.Method_Change_Type = eChangeType.update;
					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);			
				}
			}
			catch(Exception ex)
			{
				HandleException(ex, "Call_btnProcess_Click");
			}
		}
	
		private bool MethodChanges()
		{
			try
			{
				//Method Name
				if (_DCFulfillmentMethod.Name != this.txtName.Text)
					return true;

				//Method Description
				if (_DCFulfillmentMethod.Method_Description != this.txtDesc.Text)
					return true;
			
				//Global and User Radio Buttons
				if (radGlobal.Checked)
				{
					if (_DCFulfillmentMethod.GlobalUserType != eGlobalUserType.Global)
						return true;
				}
				else
				{
					if (_DCFulfillmentMethod.GlobalUserType != eGlobalUserType.User)
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
		
        #region DC Fulfillment Method Changes
        private void cboPrioritizeHeadersBy_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboPrioritizeHeadersBy_SelectionChangeCommitted(source, new EventArgs());
        }

        private void cboPrioritizeHeadersBy_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
                try
                {
                    if (cboPrioritizeHeadersBy.SelectedIndex != -1)
                    {
                        HeaderFieldCharEntry hfce = (HeaderFieldCharEntry)cboPrioritizeHeadersBy.SelectedItem;
                        _DCFulfillmentMethod.PrioritizeType = hfce.Type;
                        _DCFulfillmentMethod.FieldDataType = hfce.FieldDataType;
                        if (hfce.Type == 'C')
                        {
                            _DCFulfillmentMethod.Hcg_RID = hfce.Key;
                            _DCFulfillmentMethod.HeaderField = Include.NoRID;
                        }
                        else
                        {
                            _DCFulfillmentMethod.HeaderField = hfce.Key;
                            _DCFulfillmentMethod.Hcg_RID = Include.NoRID;
                        }
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }
        
        private void rbHeadersAscending_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && rbHeadersAscending.Checked)
            {
                ChangePending = true;
                _DCFulfillmentMethod.HeadersOrder = eDCFulfillmentHeadersOrder.Ascending;
            }
        }

        private void rbHeadersDescending_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && rbHeadersDescending.Checked)
            {
                ChangePending = true;
                _DCFulfillmentMethod.HeadersOrder = eDCFulfillmentHeadersOrder.Descending;
            }
        }

        private void cbxApplyMinimums_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
                _DCFulfillmentMethod.ApplyMinimumsInd = cbxApplyMinimums.Checked;
            }
        }

        private void rbDCFulfillment_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && rbDCFulfillment.Checked)
            {
                ChangePending = true;
                _DCFulfillmentMethod.SplitOption = eDCFulfillmentSplitOption.DCFulfillment;
            }
            if (rbDCFulfillment.Checked)
            {
                cbxApplyMinimums.Enabled = true;
                gbxDCFulfillmentSplitBy.Enabled = true;
            }
        }

        private void rbProportional_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && rbProportional.Checked)
            {
                ChangePending = true;
                _DCFulfillmentMethod.SplitOption = eDCFulfillmentSplitOption.Proportional;
            }
            if (rbProportional.Checked)
            {
                cbxApplyMinimums.Enabled = false;
                gbxDCFulfillmentSplitBy.Enabled = false;
            }
        }

        private void rbStoresAscending_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && rbStoresAscending.Checked)
            {
                ChangePending = true;
                _DCFulfillmentMethod.StoresOrder = eDCFulfillmentStoresOrder.Ascending;
            }
        }

        private void rbStoresDescending_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && rbStoresDescending.Checked)
            {
                ChangePending = true;
                _DCFulfillmentMethod.StoresOrder = eDCFulfillmentStoresOrder.Descending;
            }
        }

        private void radSplitStoreDC_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && radSplitStoreDC.Checked)
            {
                ChangePending = true;
                _DCFulfillmentMethod.Split_By_Option = eDCFulfillmentSplitByOption.SplitByStore;
            }
        }
        private void radSplitDCStore_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && radSplitDCStore.Checked)
            {
                ChangePending = true;
                _DCFulfillmentMethod.Split_By_Option = eDCFulfillmentSplitByOption.SplitByDC;
            }
        }
        private void radPreSplit_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && radPreSplit.Checked)
            {
                ChangePending = true;
                _DCFulfillmentMethod.Split_By_Reserve = eDCFulfillmentReserve.ReservePreSplit;
            }
        }
        private void radPostSplit_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && radPostSplit.Checked)
            {
                ChangePending = true;
                _DCFulfillmentMethod.Split_By_Reserve = eDCFulfillmentReserve.ReservePostSplit;
            }
        }
        private void radApplyAllocQty_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && radApplyAllocQty.Checked)
            {
                ChangePending = true;
                _DCFulfillmentMethod.Apply_By = eDCFulfillmentMinimums.ApplyByQty;
            }
        }
        private void radApplyAllStores_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && radApplyAllStores.Checked)
            {
                ChangePending = true;
                _DCFulfillmentMethod.Apply_By = eDCFulfillmentMinimums.ApplyFirst;
            }
        }

        private void radWithinDCFill_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && radWithinDCFill.Checked)
            {
                ChangePending = true;
                _DCFulfillmentMethod.Within_Dc = eDCFulfillmentWithinDC.Fill;
            }
        }

        private void radWithinDCProportional_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && radWithinDCProportional.Checked)
            {
                ChangePending = true;
                _DCFulfillmentMethod.Within_Dc = eDCFulfillmentWithinDC.Proportional;
            }
        }
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
            if (cboPrioritizeHeadersBy.SelectedIndex != -1)
            {
                HeaderFieldCharEntry hfce = (HeaderFieldCharEntry)cboPrioritizeHeadersBy.SelectedItem;
                _DCFulfillmentMethod.PrioritizeType = hfce.Type;
                _DCFulfillmentMethod.FieldDataType = hfce.FieldDataType;
                if (hfce.Type == 'C')
                {
                    _DCFulfillmentMethod.Hcg_RID = hfce.Key;
                    _DCFulfillmentMethod.HeaderField = Include.NoRID;
                }
                else
                {
                    _DCFulfillmentMethod.HeaderField = hfce.Key;
                    _DCFulfillmentMethod.Hcg_RID = Include.NoRID;
                }
            }
		}

		/// <summary>
		/// Use to validate the fields that are specific to this method type
		/// </summary>
		override protected bool ValidateSpecificFields()
		{
            bool methodFieldsValid = true;

            string errorMessage = null;

            foreach (UltraGridRow row in ugOrderStoresBy.Rows)
            {
                row.Cells["DIST_CENTER"].Appearance.Image = null;
                row.Cells["DIST_CENTER"].Tag = null;
                row.Cells["SCG_RID"].Appearance.Image = null;
                row.Cells["SCG_RID"].Tag = null;

                if (row.Cells["DIST_CENTER"].Text.Length > 0 &&
                    row.Cells["SCG_RID"].Text.Length == 0)
                {
                    row.Cells["SCG_RID"].Appearance.Image = ErrorImage;
                    row.Cells["SCG_RID"].Tag = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                    methodFieldsValid = false;
                }
                else if (row.Cells["DIST_CENTER"].Text.Length == 0 &&
                    row.Cells["SCG_RID"].Text.Length > 0)
                {
                    row.Cells["DIST_CENTER"].Appearance.Image = ErrorImage;
                    row.Cells["DIST_CENTER"].Tag = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                    methodFieldsValid = false;
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
				ABM = _DCFulfillmentMethod;
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
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
        }

        private void ugOrderStoresBy_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            AddOrderStoresByValueLists();
            OrderStoresByGridLayout();
            PopulateOrderStoresByValueLists();
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
            BuildGridContextMenu();
        }

        private void AddOrderStoresByValueLists()
        {
            try
            {
                this.ugOrderStoresBy.DisplayLayout.ValueLists.Clear();
                this.ugOrderStoresBy.DisplayLayout.ValueLists.Add("StoreChars");
                this.ugOrderStoresBy.DisplayLayout.ValueLists.Add("DistCenter");
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void PopulateOrderStoresByValueLists()
        {
            try
            {
                Infragistics.Win.ValueListItem valListItem;

                this.ugOrderStoresBy.DisplayLayout.ValueLists["StoreChars"].ValueListItems.Clear();

                StoreData storeCharMaint = new StoreData();
                DataTable dt = storeCharMaint.StoreCharGroup_Read();
                foreach (DataRow row in dt.Rows)
                {
                    if ((eStoreCharType)Convert.ToInt32(row["SCG_TYPE"]) == eStoreCharType.number)
                    {
                        valListItem = new Infragistics.Win.ValueListItem();
                        valListItem.DataValue = Convert.ToInt32(row["SCG_RID"]);
                        valListItem.DisplayText = Convert.ToString(row["SCG_ID"]);
                        this.ugOrderStoresBy.DisplayLayout.ValueLists["StoreChars"].ValueListItems.Add(valListItem);
                    }
                }

                this.ugOrderStoresBy.DisplayLayout.ValueLists["DistCenter"].ValueListItems.Clear();

                Header headerData = new Header();
                dt = headerData.GetDistCenters();
                foreach (DataRow row in dt.Rows)
                {
                    valListItem = new Infragistics.Win.ValueListItem();
                    valListItem.DataValue = Convert.ToString(row["DIST_CENTER"]);
                    valListItem.DisplayText = Convert.ToString(row["DIST_CENTER"]);
                    this.ugOrderStoresBy.DisplayLayout.ValueLists["DistCenter"].ValueListItems.Add(valListItem);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void OrderStoresByGridLayout()
        {
            try
            {
                this.ugOrderStoresBy.DisplayLayout.AddNewBox.Hidden = false;
                this.ugOrderStoresBy.DisplayLayout.GroupByBox.Hidden = true;
                this.ugOrderStoresBy.DisplayLayout.GroupByBox.Prompt = string.Empty;
                this.ugOrderStoresBy.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentDC);
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Override.RowSelectors = DefaultableBoolean.True;

                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["METHOD_RID"].Hidden = true;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["SEQ"].Hidden = true;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["DIST_CENTER"].Width = 250;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["DIST_CENTER"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["DIST_CENTER"].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["DIST_CENTER"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["DIST_CENTER"].ValueList = ugOrderStoresBy.DisplayLayout.ValueLists["DistCenter"];
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["SCG_RID"].Width = 250;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["SCG_RID"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["SCG_RID"].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["SCG_RID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["SCG_RID"].ValueList = ugOrderStoresBy.DisplayLayout.ValueLists["StoreChars"];

                this.ugOrderStoresBy.DisplayLayout.CaptionVisible = DefaultableBoolean.True;
                this.ugOrderStoresBy.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentOrderStoresByGrid);
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["SCG_RID"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentStoreCharacteristic);
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["DIST_CENTER"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentDC);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugOrderStoresBy_AfterRowInsert(object sender, RowEventArgs e)
        {
            e.Row.Cells["METHOD_RID"].Value = _DCFulfillmentMethod.Key;
            e.Row.Cells["SEQ"].Value = ugOrderStoresBy.Rows.Count;
        }

        private void ugOrderStoresBy_MouseEnterElement(object sender, UIElementEventArgs e)
        {
            ShowUltraGridToolTip(ugOrderStoresBy, e);
        }

        private void BuildGridContextMenu()
        {
            MenuItem mnuItemInsert = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert));
            MenuItem mnuItemInsertBefore = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_Before));
            MenuItem mnuItemInsertAfter = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_after));
            MenuItem mnuItemDelete = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Delete));
            MenuItem mnuItemDeleteAll = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_DeleteAll));

            mnuGrids.MenuItems.Add(mnuItemInsert);
            mnuItemInsert.MenuItems.Add(mnuItemInsertBefore);
            mnuItemInsert.MenuItems.Add(mnuItemInsertAfter);
            mnuGrids.MenuItems.Add(mnuItemDelete);
            mnuGrids.MenuItems.Add(mnuItemDeleteAll);

            mnuItemInsert.Click += new System.EventHandler(this.mnuGridsItemInsert_Click);
            mnuItemInsertBefore.Click += new System.EventHandler(this.mnuGridsItemInsertBefore_Click);
            mnuItemInsertAfter.Click += new System.EventHandler(this.mnuGridsItemInsertAfter_Click);
            mnuItemDelete.Click += new System.EventHandler(this.mnuGridsItemDelete_Click);
            mnuItemDeleteAll.Click += new System.EventHandler(this.mnuGridsItemDeleteAll_Click);

            ugOrderStoresBy.ContextMenu = mnuGrids;
        }

        private void mnuGridsItemInsert_Click(object sender, System.EventArgs e)
        {
        }

        private void mnuGridsItemInsertBefore_Click(object sender, System.EventArgs e)
        {
            try
            {
                int rowPosition = 0;
                if (ugOrderStoresBy.Rows.Count > 0)
                {
                    if (this.ugOrderStoresBy.ActiveRow == null) return;
                    rowPosition = Convert.ToInt32(this.ugOrderStoresBy.ActiveRow.Cells["SEQ"].Value, CultureInfo.CurrentUICulture);
                    // increment the position of the active row to end of grid
                    foreach (UltraGridRow gridRow in ugOrderStoresBy.Rows)
                    {
                        if (Convert.ToInt32(gridRow.Cells["SEQ"].Value, CultureInfo.CurrentUICulture) >= rowPosition)
                        {
                            gridRow.Cells["SEQ"].Value = Convert.ToInt32(gridRow.Cells["SEQ"].Value, CultureInfo.CurrentUICulture) + 1;
                        }
                    }
                }
                UltraGridRow addedRow = this.ugOrderStoresBy.DisplayLayout.Bands[0].AddNew();
      
                addedRow.Cells["SEQ"].Value = rowPosition;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].SortedColumns.Clear();
                this.ugOrderStoresBy.DisplayLayout.Bands[0].SortedColumns.Add("SEQ", false);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void mnuGridsItemInsertAfter_Click(object sender, System.EventArgs e)
        {
            try
            {
                int rowPosition = 0;
                if (ugOrderStoresBy.Rows.Count > 0)
                {
                    if (this.ugOrderStoresBy.ActiveRow == null) return;
                    rowPosition = Convert.ToInt32(this.ugOrderStoresBy.ActiveRow.Cells["SEQ"].Value, CultureInfo.CurrentUICulture);
                    // increment the position of the active row to end of grid
                    foreach (UltraGridRow gridRow in ugOrderStoresBy.Rows)
                    {
                        if (Convert.ToInt32(gridRow.Cells["SEQ"].Value, CultureInfo.CurrentUICulture) > rowPosition)
                        {
                            gridRow.Cells["SEQ"].Value = Convert.ToInt32(gridRow.Cells["SEQ"].Value, CultureInfo.CurrentUICulture) + 1;
                        }
                    }
                }
  
                UltraGridRow addedRow = this.ugOrderStoresBy.DisplayLayout.Bands[0].AddNew();
  
                addedRow.Cells["SEQ"].Value = rowPosition + 1;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].SortedColumns.Clear();
                this.ugOrderStoresBy.DisplayLayout.Bands[0].SortedColumns.Add("SEQ", false);
             }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void mnuGridsItemDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                ugOrderStoresBy.DeleteSelectedRows();
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void mnuGridsItemDeleteAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                _DCFulfillmentMethod.dtStoreOrder.Clear();
                _DCFulfillmentMethod.dtStoreOrder.AcceptChanges();
                ChangePending = true;
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        
	}
}